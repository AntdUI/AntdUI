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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
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
    public class BaseForm : Form
    {
        public BaseForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        public void SetCursor(bool val)
        {
            if (InvokeRequired)
            {
                Invoke(() => SetCursor(val));
                return;
            }
            Cursor = val ? Cursors.Hand : DefaultCursor;
        }

        #region 主题

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
                if (IsHandleCreated) Win32.WindowTheme(this, value);
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

        internal ThemeConfig? themeConfig;
        public ThemeConfig Theme()
        {
            themeConfig = new ThemeConfig(this);
            return themeConfig;
        }

        internal void SetTheme()
        {
            if (mode == TAMode.Auto && themeConfig != null)
            {
                dark = Config.IsDark;
                themeConfig.Change(dark);
                EventHub.Add(this);
            }
            if (mode == TAMode.Dark || (mode == TAMode.Auto && Config.Mode == TMode.Dark)) Win32.WindowTheme(this, true);
        }

        #endregion

        #region 程序

        FormBorderStyle formBorderStyle = FormBorderStyle.Sizable;
        [Description("指示窗体的边框和标题栏的外观和行为"), Category("行为"), DefaultValue(FormBorderStyle.Sizable)]
        public new FormBorderStyle FormBorderStyle
        {
            get => formBorderStyle;
            set
            {
                if (formBorderStyle == value) return;
                base.FormBorderStyle = formBorderStyle = value;
            }
        }

        public virtual void RefreshDWM() { }

        /// <summary>
        /// 最小化
        /// </summary>
        public virtual void Min() => WindowState = FormWindowState.Minimized;

        public virtual bool IsMax => WindowState == FormWindowState.Maximized;

        /// <summary>
        /// 最大化/还原
        /// </summary>
        public virtual bool MaxRestore()
        {
            if (IsFull)
            {
                base.FormBorderStyle = formBorderStyle;
                IsFull = false;
            }
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                RefreshDWM();
                return false;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                RefreshDWM();
                return true;
            }
        }

        /// <summary>
        /// 最大化
        /// </summary>
        public virtual void Max()
        {
            if (IsFull)
            {
                base.FormBorderStyle = formBorderStyle;
                IsFull = false;
            }
            WindowState = FormWindowState.Maximized;
            RefreshDWM();
        }

        /// <summary>
        /// 全屏/还原
        /// </summary>
        public virtual bool FullRestore()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                NoFull();
                return false;
            }
            else
            {
                Full();
                return true;
            }
        }

        public bool IsFull = false;
        /// <summary>
        /// 全屏
        /// </summary>
        public virtual void Full()
        {
            if (IsFull) return;
            IsFull = true;
            base.FormBorderStyle = FormBorderStyle.None;
            if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
            WindowState = FormWindowState.Maximized;
            RefreshDWM();
        }

        public virtual void NoFull()
        {
            if (IsFull)
            {
                IsFull = false;
                base.FormBorderStyle = formBorderStyle;
                WindowState = FormWindowState.Normal;
                RefreshDWM();
            }
            else if (IsMax) MaxRestore();
        }

        #endregion

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual bool AutoHandDpi { get; set; } = true;

        #region DPI

        public float Dpi() => Config.Dpi;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (DesignMode) return;
            if (AutoHandDpi) AutoDpi(Dpi(), this);
        }

        public void AutoDpi(Control control) => AutoDpi(Dpi(), control);

        public void AutoDpi(float dpi, Control control) => Helper.DpiAuto(dpi, control);

        #endregion

        #region 交互

        #region 拖动窗口

        /// <summary>
        /// 拖动窗口（鼠标按下）
        /// </summary>
        public virtual void DraggableMouseDown()
        {
            if (IsFull) return;
            ReleaseCapture();
            SendMessage(Handle, 0x0112, 61456 | 2, IntPtr.Zero);
        }

        #endregion

        #region 调整窗口大小

        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseMove()
        {
            if (WindowState == FormWindowState.Normal)
            {
                var retval = HitTest(PointToClient(MousePosition));
                if (retval != HitTestValues.HTNOWHERE)
                {
                    var mode = retval;
                    if (mode != HitTestValues.HTCLIENT)
                    {
                        SetCursorHit(mode);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <param name="point">客户端坐标</param>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseMove(Point point)
        {
            if (WindowState == FormWindowState.Normal)
            {
                var retval = HitTest(point);
                if (retval != HitTestValues.HTNOWHERE)
                {
                    var mode = retval;
                    if (mode != HitTestValues.HTCLIENT)
                    {
                        SetCursorHit(mode);
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool is_resizable;
        /// <summary>
        /// 整窗口大小（鼠标按下）
        /// </summary>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseDown()
        {
            Point pointScreen = MousePosition;
            var mode = HitTest(PointToClient(pointScreen));
            if (mode != HitTestValues.HTCLIENT)
            {
                is_resizable = true;
                SetCursorHit(mode);
                ReleaseCapture();
                SendMessage(Handle, WindowMessage.WM_NCLBUTTONDOWN, mode, Macros.MAKELPARAM(pointScreen.X, pointScreen.Y));
                is_resizable = false;
                return true;
            }
            return false;
        }

        #endregion

        #region 鼠标

        /// <summary>
        /// 鼠标拖拽大小使能
        /// </summary>
        [Description("鼠标拖拽大小使能"), Category("交互"), DefaultValue(true)]
        public bool EnableHitTest { get; set; } = true;
        internal HitTestValues HitTest(Point point)
        {
            if (Window.CanHandMessage && EnableHitTest)
            {
                float htSize = 8F * Config.Dpi, htSize2 = htSize * 2;
                GetWindowRect(Handle, out var lpRect);

                var rect = new Rectangle(Point.Empty, lpRect.Size);

                var hitTestValue = HitTestValues.HTCLIENT;
                var x = point.X;
                var y = point.Y;

                if (x < rect.Left + htSize2 && y < rect.Top + htSize2) hitTestValue = HitTestValues.HTTOPLEFT;
                else if (x >= rect.Left + htSize2 && x <= rect.Right - htSize2 && y <= rect.Top + htSize) hitTestValue = HitTestValues.HTTOP;
                else if (x > rect.Right - htSize2 && y <= rect.Top + htSize2) hitTestValue = HitTestValues.HTTOPRIGHT;
                else if (x <= rect.Left + htSize && y >= rect.Top + htSize2 && y <= rect.Bottom - htSize2) hitTestValue = HitTestValues.HTLEFT;
                else if (x >= rect.Right - htSize && y >= rect.Top * 2 + htSize && y <= rect.Bottom - htSize2) hitTestValue = HitTestValues.HTRIGHT;
                else if (x <= rect.Left + htSize2 && y >= rect.Bottom - htSize2) hitTestValue = HitTestValues.HTBOTTOMLEFT;
                else if (x > rect.Left + htSize2 && x < rect.Right - htSize2 && y >= rect.Bottom - htSize) hitTestValue = HitTestValues.HTBOTTOM;
                else if (x >= rect.Right - htSize2 && y >= rect.Bottom - htSize2) hitTestValue = HitTestValues.HTBOTTOMRIGHT;

                return hitTestValue;
            }
            else return HitTestValues.HTCLIENT;
        }

        internal void SetCursorHit(HitTestValues mode)
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

        internal void LoadCursors(int id)
        {
            var handle = LoadCursor(IntPtr.Zero, id);
            if (handle == IntPtr.Zero) return;
            User32.SetCursor(handle);
        }

        #endregion

        #endregion

        #region 按钮点击

        internal Action? ONESC;
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ONESC == null) return base.ProcessDialogKey(keyData);
            if ((keyData & (Keys.Alt | Keys.Control)) == Keys.None)
            {
                Keys keyCode = keyData & Keys.KeyCode;
                switch (keyCode)
                {
                    case Keys.Escape:
                        ONESC();
                        return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region 委托

#if NET40 || NET46 || NET48

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IAsyncResult BeginInvoke(Action method) => BeginInvoke(method, null);

        public void Invoke(Action method) => _ = Invoke(method, null);
        public T Invoke<T>(Func<T> method) => (T)Invoke(method, null);

#endif

        #endregion
    }

    public class ThemeConfig
    {
        BaseForm form;
        public ThemeConfig(BaseForm _form)
        {
            form = _form;
        }

        #region 回调

        Action? callLight, callDark;
        Action<bool>? oncall;

        /// <summary>
        /// 设置回调
        /// </summary>
        public ThemeConfig Call(Action<bool>? call)
        {
            oncall = call;
            return this;
        }

        /// <summary>
        /// 浅色模式回调
        /// </summary>
        /// <param name="light">浅色模式</param>
        public ThemeConfig Light(Action? light = null)
        {
            callLight = light;
            return this;
        }

        /// <summary>
        /// 深色模式回调
        /// </summary>
        /// <param name="dark">深色模式</param>
        public ThemeConfig Dark(Action? dark = null)
        {
            callDark = dark;
            return this;
        }

        #endregion

        #region 背景

        Color? backLight, foreLight, backDark, foreDark;

        #region 浅色模式颜色

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public ThemeConfig Light(Color back, Color fore)
        {
            backLight = back;
            foreLight = fore;
            return this;
        }

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public ThemeConfig Light(Color back) => Light(back, Color.Black);

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public ThemeConfig Light(string back, string fore) => Light(back.ToColor(), fore.ToColor());

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public ThemeConfig Light(string back) => Light(back, "000000");

        #endregion

        #region 深色模式颜色

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public ThemeConfig Dark(Color back, Color fore)
        {
            backDark = back;
            foreDark = fore;
            return this;
        }

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public ThemeConfig Dark(Color back) => Dark(back, Color.White);

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public ThemeConfig Dark(string back, string fore) => Dark(back.ToColor(), fore.ToColor());

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public ThemeConfig Dark(string back) => Dark(back, "ffffff");

        #endregion

        PageHeader? pageheader;
        Color? headerLight, headerDark;
        /// <summary>
        /// 设置页面头部颜色
        /// </summary>
        /// <param name="header">页头</param>
        /// <param name="light">浅色背景色</param>
        /// <param name="dark">深色背景色</param>
        public ThemeConfig Header(PageHeader header, Color light, Color dark)
        {
            pageheader = header;
            headerLight = light;
            headerDark = dark;
            return this;
        }

        /// <summary>
        /// 设置页面头部颜色
        /// </summary>
        /// <param name="header">页头</param>
        /// <param name="light">浅色背景色</param>
        /// <param name="dark">深色背景色</param>
        public ThemeConfig Header(PageHeader header, string light, string dark)
        {
            pageheader = header;
            headerLight = light.ToColor();
            headerDark = dark.ToColor();
            return this;
        }

        #endregion

        #region 按钮

        Button? btn;
        /// <summary>
        /// 设置按钮 Toggle
        /// </summary>
        public ThemeConfig Button(Button? button)
        {
            btn = button;
            return this;
        }

        #endregion

        public void Change(bool dark)
        {
            form.Dark = dark;
            if (btn != null) btn.Toggle = dark;
            if (dark)
            {
                callDark?.Invoke();
                if (pageheader != null && headerDark.HasValue) pageheader.BackColor = headerDark.Value;
                if (backDark.HasValue) form.BackColor = backDark.Value;
                if (foreDark.HasValue) form.ForeColor = foreDark.Value;
            }
            else
            {
                callLight?.Invoke();
                if (pageheader != null && headerLight.HasValue) pageheader.BackColor = headerLight.Value;
                if (backLight.HasValue) form.BackColor = backLight.Value;
                if (foreLight.HasValue) form.ForeColor = foreLight.Value;
            }
            oncall?.Invoke(dark);
        }

        internal void HandleEvent(EventType id, object? tag)
        {
            if (id == EventType.THEME && tag is TMode mode) Change(mode == TMode.Dark);
        }
    }
}