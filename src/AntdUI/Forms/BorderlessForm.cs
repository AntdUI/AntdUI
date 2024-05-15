// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
using System.Windows.Forms;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace AntdUI
{
    public class BorderlessForm : BaseForm, IMessageFilter
    {
        public BorderlessForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            UpdateStyles();
            base.FormBorderStyle = FormBorderStyle.None;
        }

        #region 属性

        bool CanMessageFilter { get => shadow < 4; }

        int shadow = 10;
        /// <summary>
        /// 阴影大小
        /// </summary>
        [Description("阴影大小"), Category("外观"), DefaultValue(10)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                if (value > 0)
                {
                    ShowSkin();
                    skin?.ISize();
                    skin?.ClearShadow();
                    skin?.Print();
                }
                else
                {
                    skin?.Close(); skin = null;
                }
            }
        }

        Color shadowColor = Color.FromArgb(100, 0, 0, 0);
        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"), Category("外观"), DefaultValue(typeof(Color), "100, 0, 0, 0")]
        public Color ShadowColor
        {
            get => shadowColor;
            set
            {
                if (shadowColor == value) return;
                shadowColor = value;
                skin?.ClearShadow();
                skin?.Print();
            }
        }

        int borderWidth = 1;
        [Description("边框宽度"), Category("外观"), DefaultValue(1)]
        public int BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                skin?.Print();
            }
        }

        Color borderColor = Color.FromArgb(180, 0, 0, 0);
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("外观"), DefaultValue(typeof(Color), "180, 0, 0, 0")]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                skin?.Print();
            }
        }

        int radius = 0;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                SetReion();
                skin?.ClearShadow();
                skin?.Print();
            }
        }

        #endregion

        #region 重载事件

        protected override void OnLoad(EventArgs e)
        {
            SetReion();
            base.OnLoad(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ShowSkin();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel) skin?.Close();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible && shadow > 0 && !DesignMode) ShowSkin();
            else
            {
                if (skin != null) skin.Visible = false;
            }
            base.OnVisibleChanged(e);
        }

        BorderlessFormShadow? skin = null;
        void ShowSkin()
        {
            if (Visible && shadow > 0 && !DesignMode)
            {
                if (skin != null) skin.Visible = true;
                else
                {
                    skin = new BorderlessFormShadow(this);
                    skin.Show(this);
                }
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            skin?.OnLocationChange();
            base.OnLocationChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            skin?.OnSizeChange();
            SetReion();
            base.OnSizeChanged(e);
        }

        internal HWND handle { get; private set; }
        readonly IntPtr TRUE = new IntPtr(1);

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            var msg = (WindowMessage)m.Msg;
            switch (msg)
            {
                case WindowMessage.WM_NCHITTEST:
                    m.Result = TRUE;
                    return;
                case WindowMessage.WM_MOUSEMOVE:
                case WindowMessage.WM_NCMOUSEMOVE:
                    if (ReadMessage) ResizableMouseMove(PointToClient(MousePosition));
                    break;
                case WindowMessage.WM_LBUTTONDOWN:
                case WindowMessage.WM_NCLBUTTONDOWN:
                    if (ReadMessage) ResizableMouseDownInternal();
                    break;
            }
            base.WndProc(ref m);
        }

        #endregion

        /// <summary>
        /// 窗体圆角
        /// </summary>
        void SetReion()
        {
            if (Region != null) Region.Dispose();
            var rect = ClientRectangle;
            if (rect.Width > 0 && rect.Height > 0)
            {
                using (var path = rect.RoundPath(radius * Config.Dpi))
                {
                    var region = new Region(path);
                    path.Widen(Pens.White);
                    region.Union(path);
                    Region = region;
                }
            }
        }

        #region BASE

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

        bool dark = false;
        /// <summary>
        /// 深色模式
        /// </summary>
        [Description("深色模式"), Category("外观"), DefaultValue(false)]
        public bool Dark
        {
            get => dark;
            set
            {
                if (dark == value) return;
                dark = value;
                mode = dark ? TAMode.Dark : TAMode.Light;
                if (IsHandleCreated) DarkUI.UseImmersiveDarkMode(Handle, value);
            }
        }

        TAMode mode = TAMode.Auto;
        /// <summary>
        /// 色彩模式
        /// </summary>
        [Description("色彩模式"), Category("外观"), DefaultValue(TAMode.Auto)]
        public TAMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                if (mode == TAMode.Dark || (mode == TAMode.Auto || Config.Mode == TMode.Dark)) Dark = true;
                else Dark = false;
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
            }
        }

        void HandMessage()
        {
            ReadMessage = CanMessageFilter && winState == WState.Restore && resizable;
            IsAddMessage = ReadMessage;
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

        protected override void OnHandleCreated(EventArgs e)
        {
            handle = new HWND(Handle);
            base.OnHandleCreated(e);
            if (mode == TAMode.Dark || (mode == TAMode.Auto || Config.Mode == TMode.Dark)) DarkUI.UseImmersiveDarkMode(Handle, true);
            DisableProcessWindowsGhosting();
            HandMessage();
        }

        #region 交互

        #region 拖动窗口

        /// <summary>
        /// 拖动窗口（鼠标按下）
        /// </summary>
        public void DraggableMouseDown()
        {
            ReleaseCapture();
            SendMessage(Handle, 0x0112, 61456 | 2, IntPtr.Zero);
        }

        #endregion

        #region 调整窗口大小

        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <returns>可以调整</returns>
        public bool ResizableMouseMove()
        {
            var retval = HitTest(PointToClient(MousePosition));
            if (retval != HitTestValues.HTNOWHERE)
            {
                var mode = retval;
                if (mode != HitTestValues.HTCLIENT && base.WindowState == FormWindowState.Normal)
                {
                    down = true;
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
        public bool ResizableMouseMove(Point point)
        {
            var retval = HitTest(point);
            if (retval != HitTestValues.HTNOWHERE)
            {
                var mode = retval;
                if (mode != HitTestValues.HTCLIENT && base.WindowState == FormWindowState.Normal)
                {
                    down = true;
                    SetCursorHit(mode);
                    return true;
                }
            }
            return false;
        }

        bool down = true;
        /// <summary>
        /// 整窗口大小（鼠标按下）
        /// </summary>
        /// <returns>可以调整</returns>
        public bool ResizableMouseDown()
        {
            Point pointScreen = MousePosition;
            var mode = HitTest(PointToClient(pointScreen));
            if (mode != HitTestValues.HTCLIENT)
            {
                SetCursorHit(mode);
                ReleaseCapture();
                PostMessage(Handle, (uint)WindowMessage.WM_NCLBUTTONDOWN, (IntPtr)mode, Macros.MAKELPARAM(pointScreen.X, pointScreen.Y));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 整窗口大小（鼠标按下）
        /// </summary>
        /// <returns>可以调整</returns>
        internal bool ResizableMouseDownInternal()
        {
            Point pointScreen = MousePosition;
            var mode = HitTest(PointToClient(pointScreen));
            if (mode != HitTestValues.HTCLIENT)
            {
                SetCursorHit(mode);
                ReleaseCapture();
                PostMessage(Handle, (uint)WindowMessage.WM_NCLBUTTONDOWN, (IntPtr)mode, Macros.MAKELPARAM(pointScreen.X, pointScreen.Y));
                if (down)
                {
                    down = false;
                    return true;
                }
                return false;
            }
            return false;
        }

        #endregion

        #region 鼠标

        HitTestValues HitTest(Point point)
        {
            float htSize = 8F * Config.Dpi, htSize2 = htSize * 2;
            GetWindowRect(Handle, out var lpRect);

            var rect = new Rectangle(Point.Empty, lpRect.Size);

            var hitTestValue = HitTestValues.HTCLIENT;
            var x = point.X;
            var y = point.Y;

            if (x < rect.Left + htSize2 && y < rect.Top + htSize2)
            {
                hitTestValue = HitTestValues.HTTOPLEFT;
            }
            else if (x >= rect.Left + htSize2 && x <= rect.Right - htSize2 && y <= rect.Top + htSize)
            {
                hitTestValue = HitTestValues.HTTOP;
            }
            else if (x > rect.Right - htSize2 && y <= rect.Top + htSize2)
            {
                hitTestValue = HitTestValues.HTTOPRIGHT;
            }
            else if (x <= rect.Left + htSize && y >= rect.Top + htSize2 && y <= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTLEFT;
            }
            else if (x >= rect.Right - htSize && y >= rect.Top * 2 + htSize && y <= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTRIGHT;
            }
            else if (x <= rect.Left + htSize2 && y >= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTBOTTOMLEFT;
            }
            else if (x > rect.Left + htSize2 && x < rect.Right - htSize2 && y >= rect.Bottom - htSize)
            {
                hitTestValue = HitTestValues.HTBOTTOM;
            }
            else if (x >= rect.Right - htSize2 && y >= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTBOTTOMRIGHT;
            }

            return hitTestValue;
        }

        void SetCursorHit(HitTestValues mode)
        {
            switch (mode)
            {
                case HitTestValues.HTTOP:
                case HitTestValues.HTBOTTOM:
                    LoadCursors(32645);
                    break;
                case HitTestValues.HTLEFT:
                case HitTestValues.HTRIGHT:
                    LoadCursors(32644);
                    break;
                case HitTestValues.HTTOPLEFT:
                case HitTestValues.HTBOTTOMRIGHT:
                    LoadCursors(32642);
                    break;
                case HitTestValues.HTTOPRIGHT:
                case HitTestValues.HTBOTTOMLEFT:
                    LoadCursors(32643);
                    break;
            }
        }

        void LoadCursors(int id)
        {
            var handle = LoadCursor(lpCursorName: Macros.MAKEINTRESOURCE(id));
            var oldCursor = User32.SetCursor(handle);
            oldCursor.Close();
        }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (ReadMessage)
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
                            if (ResizableMouseDownInternal()) return true;
                        }
                        break;
                }
            }
            return OnPreFilterMessage(m);
        }

        protected virtual bool OnPreFilterMessage(System.Windows.Forms.Message m) { return false; }

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

        #endregion
    }
}