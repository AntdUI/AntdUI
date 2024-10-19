﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vanara.PInvoke;
using static Vanara.PInvoke.DwmApi;
using static Vanara.PInvoke.User32;

namespace AntdUI
{
    public class Window : BaseForm, IMessageFilter
    {
        #region 属性

        bool resizable = true;
        [Description("调整窗口大小"), Category("行为"), DefaultValue(true)]
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

        WState winState = WState.Restore;
        WState WinState
        {
            set
            {
                if (winState == value) return;
                winState = value;
                if (IsHandleCreated) HandMessage();
                EventHub.Dispatch(EventType.WINDOW_STATE, winState == WState.Maximize);
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

        #endregion

        internal HWND handle { get; private set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            handle = new HWND(Handle);
            base.OnHandleCreated(e);
            if (FormBorderStyle == FormBorderStyle.None)
            {
                SetTheme();
                DisableProcessWindowsGhosting();
                HandMessage();
                DwmArea();
            }
            else
            {
                SetTheme();
                DisableProcessWindowsGhosting();
                if (WindowState != FormWindowState.Maximized)
                {
                    Size max = MaximumSize, min = MinimumSize;
                    sizeInit = ClientSize;
                    MaximumSize = MinimumSize = ClientSize = sizeInit.Value;
                    ClientSize = sizeInit.Value;
                    MinimumSize = min;
                    MaximumSize = max;
                }
                HandMessage();
                DwmArea();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            SetWindowPos(handle, HWND.NULL, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_FRAMECHANGED);
            base.OnLoad(e);
        }

        private void InvalidateNonclient()
        {
            if (!IsHandleCreated || IsDisposed) return;
            RedrawWindow(handle, null, HWND.NULL, RedrawWindowFlags.RDW_FRAME | RedrawWindowFlags.RDW_UPDATENOW | RedrawWindowFlags.RDW_VALIDATE);
            UpdateWindow(handle);
            SetWindowPos(handle, HWND.NULL, 0, 0, 0, 0, SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOCOPYBITS | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOREPOSITION | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            var msg = (WindowMessage)m.Msg;
            switch (msg)
            {
                case WindowMessage.WM_ACTIVATE:
                    DwmArea();
                    break;
                case WindowMessage.WM_NCCALCSIZE when m.WParam != IntPtr.Zero:
                    if (WmNCCalcSize(ref m)) return;
                    break;
                case WindowMessage.WM_NCACTIVATE:
                    if (WmNCActivate(ref m)) return;
                    break;
                case WindowMessage.WM_SIZE:
                    WmSize(ref m);
                    break;
            }
            if (WmGhostingHandler(m)) return;
            base.WndProc(ref m);
        }

        static IntPtr FALSE = new IntPtr(0);
        bool WmGhostingHandler(System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x00AE:
                case 0x00AF:
                case 0xC1BC:
                    m.Result = FALSE;
                    InvalidateNonclient();
                    break;
            }
            return false;
        }

        bool iszoomed = false;
        bool ISZoomed()
        {
            bool value = IsZoomed(handle);
            if (iszoomed == value) return value;
            iszoomed = value;
            DwmArea();
            return value;
        }

        void DwmArea()
        {
            int margin;
            if (iszoomed) margin = 0;
            else margin = 1;
            DwmExtendFrameIntoClientArea(handle, new MARGINS(margin));
        }

        #region 区域

        /// <summary>
        /// 获取或设置窗体的位置
        /// </summary>
        public new Point Location
        {
            get
            {
                if (winState == WState.Restore) return base.Location;
                return ScreenRectangle.Location;
            }
            set
            {
                sizeNormal = null;
                base.Location = value;
            }
        }

        /// <summary>
        /// 控件的顶部坐标
        /// </summary>
        public new int Top
        {
            get => Location.Y;
            set
            {
                sizeNormal = null;
                base.Top = value;
            }
        }

        /// <summary>
        /// 控件的左侧坐标
        /// </summary>
        public new int Left
        {
            get => Location.X;
            set
            {
                sizeNormal = null;
                base.Left = value;
            }
        }

        /// <summary>
        /// 控件的右坐标
        /// </summary>
        public new int Right
        {
            get => ScreenRectangle.Right;
        }

        /// <summary>
        /// 控件的底部坐标
        /// </summary>
        public new int Bottom
        {
            get => ScreenRectangle.Bottom;
        }

        /// <summary>
        /// 获取或设置窗体的大小
        /// </summary>
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
                sizeInit = ClientSize;
            }
        }

        /// <summary>
        /// 控件的宽度
        /// </summary>
        public new int Width
        {
            get => Size.Width;
            set
            {
                sizeNormal = null;
                base.Width = value;
                sizeInit = ClientSize;
            }
        }

        /// <summary>
        /// 控件的高度
        /// </summary>
        public new int Height
        {
            get => Size.Height;
            set
            {
                sizeNormal = null;
                base.Height = value;
                sizeInit = ClientSize;
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
                base.Location = value.Location;
                base.Size = value.Size;
                sizeInit = ClientSize;
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
            if (retval != HitTestValues.HTNOWHERE)
            {
                var mode = retval;
                if (mode != HitTestValues.HTCLIENT && winState == WState.Restore)
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
            if (retval != HitTestValues.HTNOWHERE)
            {
                var mode = retval;
                if (mode != HitTestValues.HTCLIENT && winState == WState.Restore)
                {
                    SetCursorHit(mode);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 鼠标

        public override bool IsMax
        {
            get => winState == WState.Maximize;
        }

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
                sizeNormal = ClientSize;
                WinState = WState.Restore;
                InvalidateNonclient();
                Invalidate();
            }
        }

        bool WmNCCalcSize(ref System.Windows.Forms.Message m)
        {
            if (FormBorderStyle == FormBorderStyle.None) return false;
            if (ISZoomed())
            {
#if NET40
                var nccsp = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
#else
                var nccsp = Marshal.PtrToStructure<RECT>(m.LParam);
#endif
                var borders = GetNonClientMetrics();

                nccsp.top -= borders.Top;
                nccsp.top += borders.Bottom;
                Marshal.StructureToPtr(nccsp, m.LParam, false);
                return false;
            }
            else
            {
                m.Result = new IntPtr(1);
                return true;
            }
        }

        internal Size? sizeInit;
        Size? sizeNormal;
        bool WmNCActivate(ref System.Windows.Forms.Message m)
        {
            if (m.HWnd == IntPtr.Zero) return false;
            if (IsIconic(m.HWnd)) return false;
            m.Result = DefWindowProc(m.HWnd, (uint)m.Msg, m.WParam, new IntPtr(-1));
            return true;
        }

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
            var rect = new RECT(screenRect);
            AdjustWindowRectEx(ref rect, (WindowStyles)CreateParams.Style, false, (WindowStylesEx)CreateParams.ExStyle);
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
            if (DesignMode) base.SetBoundsCore(x, y, width, height, specified);
            else if (WindowState == FormWindowState.Normal && sizeNormal.HasValue) base.SetBoundsCore(x, y, sizeNormal.Value.Width, sizeNormal.Value.Height, specified);
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