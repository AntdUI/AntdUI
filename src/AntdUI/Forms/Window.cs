// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntdUI
{
    public class Window : BaseForm, IMessageFilter
    {
        #region 属性

        bool resizable = true;
        [Description("调整窗口大小"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool Resizable
        {
            get => resizable;
            set
            {
                if (resizable == value) return;
                resizable = value;
                HandMessage();
            }
        }

        /// <summary>
        /// 窗口边框颜色
        /// </summary>
        [Description("窗口边框颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Color? BorderColor { get; set; }

        WState winState = WState.Restore;
        WState WinState
        {
            set
            {
                if (winState == value) return;
                winState = value;
                if (IsHandleCreated) HandMessage();
                EventHub.Dispatch(EventType.WINDOW_STATE);
            }
        }

        protected virtual bool UseMessageFilter => false;
        void HandMessage()
        {
            ReadMessage = winState == WState.Restore && resizable;
            if (UseMessageFilter) IsAddMessage = true;
            else IsAddMessage = ReadMessage;
        }

        bool ReadMessage = false;
        bool _isaddMessage = false;
        bool IsAddMessage
        {
            set
            {
                if (_isaddMessage == value) return;
                _isaddMessage = value;
                if (value) Application.AddMessageFilter(this);
                else Application.RemoveMessageFilter(this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            IsAddMessage = false;
            base.Dispose(disposing);
        }

        /// <summary>
        /// 确定窗体是否出现在 Windows 任务栏中
        /// </summary>
        [Description("确定窗体是否出现在 Windows 任务栏中"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public new bool ShowInTaskbar
        {
            get => base.ShowInTaskbar;
            set
            {
                if (base.ShowInTaskbar == value) return;
                if (IsHandleCreated)
                {
                    Size max = MaximumSize, min = MinimumSize;
                    MaximumSize = MinimumSize = base.ClientSize;
                    if (InvokeRequired) { Invoke(() => base.ShowInTaskbar = value); }
                    else base.ShowInTaskbar = value;
                    MinimumSize = min;
                    MaximumSize = max;
                    oldmargin = 0;
                    if (DwmEnabled) DwmArea();
                }
                else base.ShowInTaskbar = value;
            }
        }

        #endregion

        bool DwmEnabled = true;
        protected override void OnHandleCreated(EventArgs e)
        {
            DwmEnabled = Win32.IsCompositionEnabled;
            Win32.User32.DisableProcessWindowsGhosting();
            if (WindowState != FormWindowState.Maximized) SetSize(base.ClientSize);
            HandMessage();
            if (DwmEnabled) DwmArea();
            base.OnHandleCreated(e);
        }

        void SetSize(Size size)
        {
            Size max = MaximumSize, min = MinimumSize;
            MaximumSize = MinimumSize = base.ClientSize = size;
            MinimumSize = min;
            MaximumSize = max;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch ((Win32.User32.WindowMessage)m.Msg)
            {
                case Win32.User32.WindowMessage.WM_DESTROY:
                    oldmargin = -1;
                    break;
                case Win32.User32.WindowMessage.WM_ACTIVATE:
                    if (DwmEnabled) DwmArea();
                    break;
                case Win32.User32.WindowMessage.WM_NCPAINT:
                    if (!DwmEnabled)
                    {
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;
                case Win32.User32.WindowMessage.WM_NCACTIVATE:
                    bool active = m.WParam != IntPtr.Zero;
                    EventHub.Dispatch(EventType.WINDOW_ACTIVATE, new object[] { this, active });
                    if (!DwmEnabled)
                    {
                        m.Result = TRUE;
                        return;
                    }
                    break;
                case Win32.User32.WindowMessage.WM_NCCALCSIZE when m.WParam != IntPtr.Zero:
                    if (WmNCCalcSize(ref m)) return;
                    break;
                case Win32.User32.WindowMessage.WM_SIZE:
                    WmSize(ref m);
                    break;
                case Win32.User32.WindowMessage.WM_DWMCOMPOSITIONCHANGED:
                    oldmargin = -1;
                    DwmEnabled = Win32.IsCompositionEnabled;
                    if (DwmEnabled) DwmArea();
                    break;
                default:
                    if (WmGhostingHandler(m)) return;
                    break;
            }
            try
            {
                base.WndProc(ref m);
            }
            catch { }
        }

        static IntPtr TRUE = new IntPtr(1);
        bool WmGhostingHandler(System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x00AE:
                case 0x00AF:
                case 0xC1BC:
                    m.Result = IntPtr.Zero;
                    break;
            }
            return false;
        }

        bool iszoomed = false;
        bool ISZoomed()
        {
            bool value = Win32.User32.IsZoomed(Handle);
            if (iszoomed == value) return value;
            iszoomed = value;
            if (DwmEnabled) DwmArea();
            return value;
        }

        int oldmargin = -1;
        void DwmArea()
        {
            int margin;
            if (iszoomed || IsFull) margin = 0;
            else margin = 1;
            if (oldmargin == margin) return;
            oldmargin = margin;
            Win32.DwmApi.DwmExtendFrameIntoClientArea(Handle, new Win32.DwmApi.MARGINS(margin));
        }

        protected override void SetThemeOK(bool dark) => Win32.SetWindowBorderColor(Handle, BorderColor);

        public override void RefreshDWM()
        {
            if (DwmEnabled) DwmArea();
        }

        #region 区域

        /// <summary>
        /// 获取或设置窗体的位置
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new Point Location
        {
            get
            {
                if (winState == WState.Restore) return base.Location;
                return ScreenRectangle.Location;
            }
            set => base.Location = value;
        }

        /// <summary>
        /// 控件的顶部坐标
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Top
        {
            get => Location.Y;
            set => base.Top = value;
        }

        /// <summary>
        /// 控件的左侧坐标
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Left
        {
            get => Location.X;
            set => base.Left = value;
        }

        /// <summary>
        /// 控件的右坐标
        /// </summary>
        public new int Right => ScreenRectangle.Right;

        /// <summary>
        /// 控件的底部坐标
        /// </summary>
        public new int Bottom => ScreenRectangle.Bottom;

        /// <summary>
        /// 获取或设置窗体的大小
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size
        {
            get
            {
                if (winState == WState.Restore) return base.Size;
                return ScreenRectangle.Size;
            }
            set
            {
                sizeNormal = null;
                base.Size = value;
            }
        }

        /// <summary>
        /// 获取或设置窗体的客户端区域的大小
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        public new Size ClientSize
        {
            get => base.ClientSize;
            set
            {
                sizeNormal = null;
                base.ClientSize = value;
            }
        }

        /// <summary>
        /// 控件的宽度
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Width
        {
            get => Size.Width;
            set
            {
                sizeNormal = null;
                base.Width = value;
            }
        }

        /// <summary>
        /// 控件的高度
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Height
        {
            get => Size.Height;
            set
            {
                sizeNormal = null;
                base.Height = value;
            }
        }

        /// <summary>
        /// 获取或设置窗体屏幕区域
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ScreenRectangle
        {
            get
            {
                if (winState == WState.Restore) return new Rectangle(base.Location, base.Size);
                var rect = ClientRectangle;
                var point = RectangleToScreen(Rectangle.Empty);
                return new Rectangle(point.Location, rect.Size);
            }
            set
            {
                sizeNormal = null;
                Bounds = value;
            }
        }

        #endregion

        #region 交互

        #region 调整窗口大小

        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <returns>可以调整</returns>
        public override bool ResizableMouseMove()
        {
            var retval = HitTest(PointToClient(MousePosition));
            if (retval != Win32.User32.HitTestValues.HTNOWHERE)
            {
                var mode = retval;
                if (mode != Win32.User32.HitTestValues.HTCLIENT && winState == WState.Restore)
                {
                    SetCursorHit(mode);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <param name="point">客户端坐标</param>
        /// <returns>可以调整</returns>
        public override bool ResizableMouseMove(Point point)
        {
            var retval = HitTest(point);
            if (retval != Win32.User32.HitTestValues.HTNOWHERE)
            {
                var mode = retval;
                if (mode != Win32.User32.HitTestValues.HTCLIENT && winState == WState.Restore)
                {
                    SetCursorHit(mode);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 鼠标

        public override bool IsMax => winState == WState.Maximize;

        public static bool CanHandMessage = true;
        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (is_resizable) return OnPreFilterMessage(m);
            if (CanHandMessage && ReadMessage)
            {
                switch (m.Msg)
                {
                    case 0xa0:
                    case 0x200:
                        if (isMe(m.HWnd))
                        {
                            if (ResizableMouseMove(PointToClient(MousePosition))) return true;
                        }
                        break;
                    case 0xa1:
                    case 0x201:
                        if (isMe(m.HWnd))
                        {
                            if (ResizableMouseDown()) return true;
                        }
                        break;
                }
            }
            return OnPreFilterMessage(m);
        }

        protected virtual bool OnPreFilterMessage(System.Windows.Forms.Message m) => false;

        bool isMe(IntPtr intPtr)
        {
            var frm = FromHandle(intPtr);
            if (frm == this || GetParent(frm) == this) return true;
            return false;
        }

        static Control? GetParent(Control? control)
        {
            try
            {
                if (control != null && control.IsHandleCreated && control.Parent != null)
                {
                    if (control is Form) return control;
                    return GetParent(control.Parent);
                }
            }
            catch { }
            return control;
        }

        #endregion

        #endregion

        #region WindowMessage Handlers

        const nint SIZE_RESTORED = 0;
        const nint SIZE_MINIMIZED = 1;
        const nint SIZE_MAXIMIZED = 2;
        void WmSize(ref System.Windows.Forms.Message m)
        {
            if (m.WParam == SIZE_MINIMIZED) WinState = WState.Minimize;
            else if (m.WParam == SIZE_MAXIMIZED) WinState = WState.Maximize;
            else if (m.WParam == SIZE_RESTORED)
            {
                WinState = WState.Restore;
                sizeNormal = ClientSize;
            }
        }

        bool WmNCCalcSize(ref System.Windows.Forms.Message m)
        {
            if (ISZoomed())
            {
                var nccsp = Win32.PtrToStructure<Win32.RECT>(m.LParam);
                var borders = GetNonClientMetrics();
                nccsp.top -= borders.Top;
                nccsp.top += borders.Bottom;
                Marshal.StructureToPtr(nccsp, m.LParam, false);
                m.Result = new IntPtr(0x0400);
                return false;
            }
            else
            {
                m.Result = TRUE;
                return true;
            }
        }

        Size? sizeNormal;

        #endregion

        #region Frameless Crack

        protected override void SetClientSizeCore(int x, int y)
        {
            if (DesignMode) Size = new Size(x, y);
            else base.SetClientSizeCore(x, y);
        }

        protected Padding GetNonClientMetrics()
        {
            var screenRect = ClientRectangle;
            screenRect.Offset(-Bounds.Left, -Bounds.Top);
            var rect = new Win32.RECT(screenRect);
            Win32.User32.AdjustWindowRectEx(ref rect, (Win32.User32.WindowStyles)CreateParams.Style, false, (Win32.User32.WindowStylesEx)CreateParams.ExStyle);
            return new Padding
            {
                Top = screenRect.Top - rect.top,
                Left = screenRect.Left - rect.left,
                Bottom = rect.bottom - screenRect.Bottom,
                Right = rect.right - screenRect.Right
            };
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
#if NET40 || NET46 || NET48
            if (!DesignMode && WindowState == FormWindowState.Normal && sizeNormal.HasValue) base.SetBoundsCore(x, y, sizeNormal.Value.Width, sizeNormal.Value.Height, BoundsSpecified.None);
#else
            if (OS.Win7OrLower && !DesignMode && WindowState == FormWindowState.Normal && sizeNormal.HasValue) base.SetBoundsCore(x, y, sizeNormal.Value.Width, sizeNormal.Value.Height, BoundsSpecified.None);
#endif
            else base.SetBoundsCore(x, y, width, height, specified);
        }

        #endregion
    }

    public enum WState
    {
        Restore,
        Maximize,
        Minimize
    }
}