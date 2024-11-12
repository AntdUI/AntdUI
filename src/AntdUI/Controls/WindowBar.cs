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
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// WindowBar 窗口栏
    /// </summary>
    /// <remarks>窗口栏。</remarks>
    [Description("WindowBar 窗口栏")]
    [ToolboxItem(true)]
    [Designer(typeof(IControlDesigner))]
    public class WindowBar : IControl, IEventListener
    {
        #region 属性

        TAMode mode = TAMode.Auto;
        [Description("色彩模式"), Category("外观"), DefaultValue(TAMode.Auto)]
        public TAMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                DisposeBmp();
                Invalidate();
            }
        }

        string? text = null;
        [Description("文字"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => this.GetLangI(LocalizationText, text);
            set
            {
                if (text == value) return;
                text = value;
                Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        string? desc = null;
        [Description("副标题"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? SubText
        {
            get => this.GetLangI(LocalizationSubText, desc);
            set
            {
                if (desc == value) return;
                desc = value;
                Invalidate();
            }
        }

        [Description("副标题"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSubText { get; set; }

        int useLeft = 0;

        [Description("左侧使用"), Category("外观"), DefaultValue(0)]
        public int UseLeft
        {
            get => useLeft;
            set
            {
                if (useLeft == value) return;
                useLeft = value;
                Invalidate();
            }
        }

        bool useSystemStyleColor = false;
        [Description("使用系统颜色"), Category("外观"), DefaultValue(false)]
        public bool UseSystemStyleColor
        {
            get => useSystemStyleColor;
            set
            {
                if (useSystemStyleColor == value) return;
                useSystemStyleColor = value;
                DisposeBmp();
                Invalidate();
            }
        }

        [Description("点击退出关闭"), Category("行为"), DefaultValue(false)]
        public bool CancelButton { get; set; }

        #region 图标

        bool showicon = true;
        [Description("是否显示图标"), Category("外观"), DefaultValue(true)]
        public bool ShowIcon
        {
            get => showicon;
            set
            {
                if (showicon == value) return;
                showicon = value;
                Invalidate();
            }
        }

        Image? icon = null;
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidate();
            }
        }

        string? iconSvg = null;
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidate();
            }
        }

        #endregion

        #region 加载动画

        bool loading = false;
        int AnimationLoadingValue = 0;
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                ThreadLoading?.Dispose();
                if (loading)
                {
                    ThreadLoading = new ITask(this, () =>
                    {
                        AnimationLoadingValue += 6;
                        if (AnimationLoadingValue > 360) AnimationLoadingValue = 0;
                        Invalidate();
                        return loading;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                }
                else Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            hove_close.Dispose();
            hove_max.Dispose();
            hove_min.Dispose();
            ThreadLoading?.Dispose();
            temp_logo?.Dispose();
            temp_min?.Dispose();
            temp_max?.Dispose();
            temp_restore?.Dispose();
            temp_close?.Dispose();
            temp_close_hover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadLoading = null;

        #endregion

        #region 按钮

        bool maximizeBox = true;
        /// <summary>
        /// 是否显示最大化按钮
        /// </summary>
        [Description("是否显示最大化按钮"), Category("外观"), DefaultValue(true)]
        public bool MaximizeBox
        {
            get => maximizeBox;
            set
            {
                if (maximizeBox == value) return;
                maximizeBox = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        bool minimizeBox = true;
        /// <summary>
        /// 是否显示最小化按钮
        /// </summary>
        [Description("是否显示最小化按钮"), Category("外观"), DefaultValue(true)]
        public bool MinimizeBox
        {
            get => minimizeBox;
            set
            {
                if (minimizeBox == value) return;
                minimizeBox = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        bool isMax = false;
        public bool IsMax
        {
            get => isMax;
            set
            {
                if (isMax == value) return;
                isMax = value;
                Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 是否可以拖动位置
        /// </summary>
        [Description("是否可以拖动位置"), Category("行为"), DefaultValue(true)]
        public bool DragMove { get; set; } = true;

        [Description("关闭按钮大小"), Category("行为"), DefaultValue(48)]
        public int CloseSize { get; set; } = 48;

        #region 线条

        bool showDivider = false;
        /// <summary>
        /// 显示线
        /// </summary>
        [Description("显示线"), Category("线"), DefaultValue(false)]
        public bool DividerShow
        {
            get => showDivider;
            set
            {
                if (showDivider == value) return;
                showDivider = value;
                Invalidate();
            }
        }

        Color? dividerColor;
        /// <summary>
        /// 线颜色
        /// </summary>
        [Description("线颜色"), Category("线"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? DividerColor
        {
            get => dividerColor;
            set
            {
                if (dividerColor == value) return;
                dividerColor = value;
                if (showDivider) Invalidate();
            }
        }

        float dividerthickness = 1F;
        /// <summary>
        /// 线厚度
        /// </summary>
        [Description("线厚度"), Category("线"), DefaultValue(1F)]
        public float DividerThickness
        {
            get => dividerthickness;
            set
            {
                if (dividerthickness == value) return;
                dividerthickness = value;
                if (showDivider) Invalidate();
            }
        }

        int dividerMargin = 0;
        /// <summary>
        /// 线边距
        /// </summary>
        [Description("线边距"), Category("线"), DefaultValue(0)]
        public int DividerMargin
        {
            get => dividerMargin;
            set
            {
                if (dividerMargin == value) return;
                dividerMargin = value;
                if (showDivider) Invalidate();
            }
        }

        #endregion

        #endregion

        public override Rectangle DisplayRectangle
        {
            get => ClientRectangle.PaddingRect(Padding, 0, 0, hasr, 0);
        }

        readonly StringFormat stringLeft = Helper.SF_ALL(lr: StringAlignment.Near);

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect_ = ClientRectangle;
            var rect = rect_.PaddingRect(Padding, UseLeft, 0, 0, 0);
            var g = e.Graphics.High();

            var size = g.MeasureString(Text ?? Config.NullText, Font);
            bool showLeft = false;
            Color fore = Style.Db.Text, forebase = Style.Db.TextBase, foreSecondary = Style.Db.TextSecondary,
                fillsecondary = Style.Db.FillSecondary;
            if (useSystemStyleColor)
            {
                forebase = ForeColor;
                if (mode == TAMode.Dark)
                {
                    fore = Style.rgba(forebase, 0.85F);
                    foreSecondary = Style.rgba(forebase, 0.65F);
                    fillsecondary = Style.rgba(forebase, 0.12F);
                }
                else
                {
                    fore = Style.rgba(forebase, 0.88F);
                    foreSecondary = Style.rgba(forebase, 0.65F);
                    fillsecondary = Style.rgba(forebase, 0.06F);
                }
            }
            else if (mode == TAMode.Light)
            {
                forebase = Color.Black;
                fore = Style.rgba(forebase, 0.88F);
                foreSecondary = Style.rgba(forebase, 0.65F);
                fillsecondary = Style.rgba(forebase, 0.06F);
            }
            else if (mode == TAMode.Dark)
            {
                forebase = Color.White;
                fore = Style.rgba(forebase, 0.85F);
                foreSecondary = Style.rgba(forebase, 0.65F);
                fillsecondary = Style.rgba(forebase, 0.12F);
            }

            int icon_size = size.Height, iocn_xy = (rect.Height - icon_size) / 2;
            if (loading || iconSvg != null || icon != null || showicon)
            {
                var rect_icon = new Rectangle(rect.X + iocn_xy, rect.Y + iocn_xy, icon_size, icon_size);
                if (loading)
                {
                    using (var brush = new Pen(Color.FromArgb(170, fore), 3f))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect_icon, AnimationLoadingValue, 100);
                    }
                    showLeft = true;
                }
                else if (iconSvg != null)
                {
                    if (PrintLogo(g, iconSvg, fore, new Rectangle(rect.X + iocn_xy, rect.Y + iocn_xy, icon_size, icon_size))) showLeft = true;
                }
                if (!showLeft)
                {
                    if (icon != null)
                    {
                        g.Image(icon, rect_icon);
                        showLeft = true;
                    }
                    else if (showicon)
                    {
                        var form = Parent.FindPARENT();
                        if (form != null && form.Icon != null)
                        {
                            g.Icon(form.Icon, rect_icon);
                            showLeft = true;
                        }
                    }
                }

                if (showLeft)
                {
                    int use = iocn_xy * 2 + icon_size;
                    rect = new Rectangle(rect.X + use, rect.Y, rect.Width - use, rect.Height);
                }
                else if (rect.X == 0) rect = new Rectangle(rect.X + iocn_xy, rect.Y, rect.Width - iocn_xy, rect.Height);
            }
            else if (rect.X == 0) rect = new Rectangle(rect.X + iocn_xy, rect.Y, rect.Width - iocn_xy, rect.Height);

            using (var brush = new SolidBrush(forebase))
            {
                g.String(Text, Font, brush, rect, stringLeft);
                if (SubText != null)
                {
                    using (var brushsub = new SolidBrush(foreSecondary))
                    {
                        g.String(SubText, Font, brushsub, new Rectangle(rect.X + size.Width, rect.Y, rect.Width - size.Width, rect.Height), stringLeft);
                    }
                }
            }

            #region 按钮

            int btn_size = (int)(size.Height * 1.2F), btn_x = (rect_close.Width - btn_size) / 2, btn_y = (rect_close.Height - btn_size) / 2;
            var rect_close_icon = new Rectangle(rect_close.X + btn_x, rect_close.Y + btn_y, btn_size, btn_size);
            if (hove_close.Down)
            {
                using (var brush = new SolidBrush(Style.Db.ErrorActive))
                {
                    g.Fill(brush, rect_close);
                }
                PrintCloseHover(g, rect_close_icon);
            }
            else if (hove_close.Animation)
            {
                using (var brush = new SolidBrush(Helper.ToColor(hove_close.Value, Style.Db.Error)))
                {
                    g.Fill(brush, rect_close);
                }
                PrintClose(g, fore, rect_close_icon);
                g.GetImgExtend(SvgDb.IcoAppClose, rect_close_icon, Helper.ToColor(hove_close.Value, Style.Db.ErrorColor));
            }
            else if (hove_close.Switch)
            {
                using (var brush = new SolidBrush(Style.Db.Error))
                {
                    g.Fill(brush, rect_close);
                }
                PrintCloseHover(g, rect_close_icon);
            }
            else PrintClose(g, fore, rect_close_icon);

            if (maximizeBox)
            {
                var rect_max_icon = new Rectangle(rect_max.X + btn_x, rect_max.Y + btn_y, btn_size, btn_size);
                if (hove_max.Animation)
                {
                    using (var brush = new SolidBrush(Helper.ToColor(hove_max.Value, fillsecondary)))
                    {
                        g.Fill(brush, rect_max);
                    }
                }
                else if (hove_max.Switch)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.Fill(brush, rect_max);
                    }
                }
                if (hove_max.Down)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.Fill(brush, rect_max);
                    }
                }
                if (IsMax) PrintRestore(g, fore, rect_max_icon);
                else PrintMax(g, fore, rect_max_icon);
            }
            if (minimizeBox)
            {
                var rect_min_icon = new Rectangle(rect_min.X + btn_x, rect_min.Y + btn_y, btn_size, btn_size);
                if (hove_min.Animation)
                {
                    using (var brush = new SolidBrush(Helper.ToColor(hove_min.Value, fillsecondary)))
                    {
                        g.Fill(brush, rect_min);
                    }
                }
                else if (hove_min.Switch)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.Fill(brush, rect_min);
                    }
                }
                if (hove_min.Down)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.Fill(brush, rect_min);
                    }
                }
                PrintMin(g, fore, rect_min_icon);
            }

            #endregion

            this.PaintBadge(g);

            if (showDivider)
            {
                float thickness = dividerthickness * Config.Dpi;
                int margin = (int)(dividerMargin * Config.Dpi);
                using (var brush = dividerColor.Brush(Style.Db.Split))
                {
                    g.Fill(brush, new RectangleF(rect_.X + margin, rect_.Bottom - thickness, rect_.Width - margin * 2, thickness));
                }
            }
            base.OnPaint(e);
        }

        #region 渲染帮助

        Bitmap? temp_logo = null, temp_min = null, temp_max = null, temp_restore = null, temp_close = null, temp_close_hover = null;

        void PrintClose(Canvas g, Color color, Rectangle rect_icon)
        {
            if (temp_close == null || temp_close.Width != rect_icon.Width)
            {
                temp_close?.Dispose();
                temp_close = SvgExtend.GetImgExtend(SvgDb.IcoAppClose, rect_icon, color);
            }
            if (temp_close != null) g.Image(temp_close, rect_icon);
        }
        void PrintCloseHover(Canvas g, Rectangle rect_icon)
        {
            if (temp_close_hover == null || temp_close_hover.Width != rect_icon.Width)
            {
                temp_close_hover?.Dispose();
                temp_close_hover = SvgExtend.GetImgExtend(SvgDb.IcoAppClose, rect_icon, Style.Db.ErrorColor);
            }
            if (temp_close_hover != null) g.Image(temp_close_hover, rect_icon);
        }
        void PrintMax(Canvas g, Color color, Rectangle rect_icon)
        {
            if (temp_max == null || temp_max.Width != rect_icon.Width)
            {
                temp_max?.Dispose();
                temp_max = SvgExtend.GetImgExtend(SvgDb.IcoAppMax, rect_icon, color);
            }
            if (temp_max != null) g.Image(temp_max, rect_icon);
        }
        void PrintRestore(Canvas g, Color color, Rectangle rect_icon)
        {
            if (temp_restore == null || temp_restore.Width != rect_icon.Width)
            {
                temp_restore?.Dispose();
                temp_restore = SvgExtend.GetImgExtend(SvgDb.IcoAppRestore, rect_icon, color);
            }
            if (temp_restore != null) g.Image(temp_restore, rect_icon);
        }
        void PrintMin(Canvas g, Color color, Rectangle rect_icon)
        {
            if (temp_min == null || temp_min.Width != rect_icon.Width)
            {
                temp_min?.Dispose();
                temp_min = SvgExtend.GetImgExtend(SvgDb.IcoAppMin, rect_icon, color);
            }
            if (temp_min != null) g.Image(temp_min, rect_icon);
        }
        bool PrintLogo(Canvas g, string svg, Color color, Rectangle rect_icon)
        {
            if (temp_logo == null || temp_logo.Width != rect_icon.Width)
            {
                temp_logo?.Dispose();
                temp_logo = SvgExtend.GetImgExtend(svg, rect_icon, color);
            }
            if (temp_logo != null) { g.Image(temp_logo, rect_icon); return true; }
            return false;
        }

        void DisposeBmp()
        {
            temp_logo?.Dispose();
            temp_min?.Dispose();
            temp_max?.Dispose();
            temp_restore?.Dispose();
            temp_close?.Dispose();
            temp_logo = null;
            temp_min = null;
            temp_max = null;
            temp_restore = null;
            temp_close = null;
        }

        #endregion

        #endregion

        int hasr = 0;
        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle.PaddingRect(Padding);
            if (CloseSize > 0)
            {
                int btn_size = (maximizeBox || minimizeBox) ? (int)Math.Round(CloseSize * Config.Dpi) : (int)Math.Round((CloseSize - 8) * Config.Dpi);
                rect_close = new Rectangle(rect.Right - btn_size, rect.Y, btn_size, rect.Height);
                hasr = btn_size;
                int left = rect_close.Left;
                if (maximizeBox)
                {
                    rect_max = new Rectangle(left - btn_size, rect.Y, btn_size, rect.Height);
                    left -= btn_size;
                    hasr += btn_size;
                }
                if (minimizeBox)
                {
                    rect_min = new Rectangle(left - btn_size, rect.Y, btn_size, rect.Height);
                    hasr += btn_size;
                }
            }
            else hasr = 0;

            if (DragMove)
            {
                var form = Parent.FindPARENT();
                if (form != null)
                {
                    if (form is LayeredFormDrawer) return;
                    if (form is BaseForm form_win) IsMax = form_win.IsMax;
                    else IsMax = form.WindowState == FormWindowState.Maximized;
                }
            }
            base.OnSizeChanged(e);
        }

        #region 动画

        ITaskOpacity hove_close, hove_max, hove_min;
        public WindowBar() { hove_close = new ITaskOpacity(this); hove_max = new ITaskOpacity(this); hove_min = new ITaskOpacity(this); }

        #endregion

        #region 鼠标

        Rectangle rect_close, rect_max, rect_min;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool _close = rect_close.Contains(e.Location), _max = rect_max.Contains(e.Location), _min = rect_min.Contains(e.Location);
            if (_close != hove_close.Switch || _max != hove_max.Switch || _min != hove_min.Switch)
            {
                Color fillsecondary = Style.Db.FillSecondary;
                if (mode == TAMode.Light) fillsecondary = Style.rgba(0, 0, 0, 0.06F);
                else if (mode == TAMode.Dark) fillsecondary = Style.rgba(255, 255, 255, 0.12F);

                hove_max.MaxValue = hove_min.MaxValue = fillsecondary.A;
                hove_close.Switch = _close;
                hove_max.Switch = _max;
                hove_min.Switch = _min;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hove_close.Switch = hove_max.Switch = hove_min.Switch = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                hove_close.Down = rect_close.Contains(e.Location);
                hove_max.Down = rect_max.Contains(e.Location);
                hove_min.Down = rect_min.Contains(e.Location);
                if (hove_close.Down || hove_max.Down || hove_min.Down) return;
                if (DragMove)
                {
                    var form = Parent.FindPARENT();
                    if (form != null)
                    {
                        if (form is LayeredFormDrawer) return;
                        if (e.Clicks > 1)
                        {
                            if (maximizeBox)
                            {
                                if (form is BaseForm form_win) IsMax = form_win.MaxRestore();
                                else
                                {
                                    if (form.WindowState == FormWindowState.Maximized)
                                    {
                                        IsMax = false;
                                        form.WindowState = FormWindowState.Normal;
                                    }
                                    else
                                    {
                                        IsMax = true;
                                        form.WindowState = FormWindowState.Maximized;
                                    }
                                }
                                return;
                            }
                        }
                        else
                        {
                            if (form is BaseForm form_win) form_win.DraggableMouseDown();
                            else
                            {
                                Vanara.PInvoke.User32.ReleaseCapture();
                                Vanara.PInvoke.User32.SendMessage(form.Handle, 0x0112, 61456 | 2, IntPtr.Zero);
                            }
                        }
                    }
                }
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (hove_close.Down && rect_close.Contains(e.Location)) Parent.FindPARENT()?.Close();
            else if (hove_max.Down && rect_max.Contains(e.Location))
            {
                var form = Parent.FindPARENT();
                if (form != null)
                {
                    if (form is LayeredFormDrawer) return;
                    if (form is BaseForm form_win) IsMax = form_win.MaxRestore();
                    else
                    {
                        if (form.WindowState == FormWindowState.Maximized)
                        {
                            IsMax = false;
                            form.WindowState = FormWindowState.Normal;
                        }
                        else
                        {
                            IsMax = true;
                            form.WindowState = FormWindowState.Maximized;
                        }
                    }
                }
            }
            else if (hove_min.Down && rect_min.Contains(e.Location))
            {
                var form = Parent.FindPARENT();
                if (form != null)
                {
                    if (form is LayeredFormDrawer) return;
                    form.WindowState = FormWindowState.Minimized;
                }
            }
            hove_close.Down = hove_max.Down = hove_min.Down = false;
            base.OnMouseUp(e);
        }

        #region 主题变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
        }
        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.THEME:
                    DisposeBmp();
                    Invalidate();
                    break;
                case EventType.WINDOW_STATE:
                    if (tag is bool state) IsMax = state;
                    break;
            }
        }

        #endregion

        #endregion

        #region 按钮点击

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (CancelButton && (keyData & (Keys.Alt | Keys.Control)) == Keys.None)
            {
                Keys keyCode = keyData & Keys.KeyCode;
                switch (keyCode)
                {
                    case Keys.Escape:
                        Parent.FindPARENT()?.Close();
                        return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        #endregion
    }
}