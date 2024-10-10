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
    /// PageHeader 页头
    /// </summary>
    /// <remarks>页头位于页容器中，页容器顶部，起到了内容概览和引导页级操作的作用。包括由面包屑、标题、页面内容简介、页面级操作等、页面级导航组成。</remarks>
    [Description("PageHeader 页头")]
    [ToolboxItem(true)]
    [Designer(typeof(IControlDesigner))]
    public class PageHeader : IControl, IEventListener
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
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

        [Description("使用标题大小"), Category("外观"), DefaultValue(false)]
        public bool UseTitleFont { get; set; } = false;

        [Description("标题使用粗体"), Category("外观"), DefaultValue(true)]
        public bool UseTextBold { get; set; } = true;

        string? desc = null;
        [Description("副标题"), Category("外观"), DefaultValue(null)]
        public string? SubText
        {
            get => desc;
            set
            {
                if (desc == value) return;
                desc = value;
                Invalidate();
            }
        }

        string? description = null;
        /// <summary>
        /// 描述文本
        /// </summary>
        [Description("描述文本"), Category("外观"), DefaultValue(null)]
        public string? Description
        {
            get => description;
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (description == value) return;
                description = value;
                Invalidate();
            }
        }

        int? gap = null;
        /// <summary>
        /// 间隔
        /// </summary>
        [Description("间隔"), Category("外观"), DefaultValue(null)]
        public int? Gap
        {
            get => gap;
            set
            {
                if (gap == value) return;
                gap = value;
                Invalidate();
            }
        }

        int subGap = 6;
        /// <summary>
        /// 副标题与标题间隔
        /// </summary>
        [Description("副标题与标题间隔"), Category("外观"), DefaultValue(6)]
        public int SubGap
        {
            get => subGap;
            set
            {
                if (subGap == value) return;
                subGap = value;
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
        public bool CancelButton { get; set; } = false;

        #region 图标

        bool showicon = false;
        [Description("是否显示图标"), Category("外观"), DefaultValue(false)]
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
            ThreadBack?.Dispose();
            hove_back.Dispose();
            hove_close.Dispose();
            hove_max.Dispose();
            hove_min.Dispose();
            ThreadLoading?.Dispose();
            temp_logo?.Dispose();
            temp_back?.Dispose();
            temp_back_hover?.Dispose();
            temp_back_down?.Dispose();
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

        bool AnimationBack = false;
        float AnimationBackValue = 0F;
        bool showback = false;
        /// <summary>
        /// 是否显示返回按钮
        /// </summary>
        [Description("是否显示返回按钮"), Category("外观"), DefaultValue(false)]
        public bool ShowBack
        {
            get => showback;
            set
            {
                if (showback == value) return;
                showback = value;
                if (Config.Animation && IsHandleCreated)
                {
                    ThreadBack?.Dispose();
                    AnimationBack = true;
                    var t = Animation.TotalFrames(10, 200);
                    var _rect = ClientRectangle;
                    var rect = new Rectangle(_rect.X, _rect.Y, _rect.Width - hasr, _rect.Height);
                    if (value)
                    {
                        ThreadBack = new ITask((i) =>
                        {
                            AnimationBackValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate(rect);
                            return true;
                        }, 10, t, () =>
                        {
                            AnimationBackValue = 1F;
                            AnimationBack = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadBack = new ITask((i) =>
                        {
                            AnimationBackValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate(rect);
                            return true;
                        }, 10, t, () =>
                        {
                            AnimationBackValue = 0F;
                            AnimationBack = false;
                            Invalidate();
                        });
                    }
                }
                else
                {
                    AnimationBackValue = value ? 1F : 0F;
                    Invalidate();
                }
            }
        }

        bool showButton = false;
        /// <summary>
        /// 是否显示标题栏按钮
        /// </summary>
        [Description("是否显示标题栏按钮"), Category("外观"), DefaultValue(false)]
        public bool ShowButton
        {
            get => showButton;
            set
            {
                if (showButton == value) return;
                showButton = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

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
                if (showButton)
                {
                    OnSizeChanged(EventArgs.Empty);
                    Invalidate();
                }
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
                if (showButton)
                {
                    OnSizeChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        bool isMax = false;
        /// <summary>
        /// 是否最大化
        /// </summary>
        [Description("是否最大化"), Category("外观"), DefaultValue(false)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMax
        {
            get => isMax;
            set
            {
                if (isMax == value) return;
                isMax = value;
                if (showButton) Invalidate();
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

        StringFormat stringLeft = Helper.SF_ALL(lr: StringAlignment.Near);

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect_ = ClientRectangle;
            var rect = rect_.PaddingRect(Padding, 0, 0, hasr, 0);
            var g = e.Graphics.High();

            #region 显示颜色

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

            #endregion

            if (UseTitleFont)
            {
                var size = g.MeasureString(Config.NullText, Font).Size();
                using (var fontTitle = new Font(Font.FontFamily, Font.Size * 1.44F, UseTextBold ? FontStyle.Bold : Font.Style))
                {
                    bool showDescription = false;
                    int heightDescription = rect.Height;
                    if (description != null)
                    {
                        showDescription = true;
                        heightDescription = rect.Height / 3;
                        rect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - heightDescription);
                    }
                    int u_x = IPaint(g, rect, fore, size.Height, 1.36F);
                    rect.X += u_x;
                    rect.Width -= u_x;
                    using (var brush = new SolidBrush(forebase))
                    {
                        var sizeTitle = g.MeasureString(text, fontTitle).Size();
                        g.DrawStr(text, fontTitle, brush, rect, stringLeft);
                        if (desc != null)
                        {
                            int desc_t_w = sizeTitle.Width + (int)(subGap * Config.Dpi);
                            using (var brushsub = new SolidBrush(foreSecondary))
                            {
                                g.DrawStr(desc, Font, brushsub, new Rectangle(rect.X + desc_t_w, rect.Y, rect.Width - desc_t_w, rect.Height), stringLeft);
                                if (showDescription) g.DrawStr(description, Font, brushsub, new Rectangle(rect.X, rect.Bottom, rect.Width, heightDescription), stringLeft);
                            }
                        }
                        else if (showDescription)
                        {
                            using (var brushsub = new SolidBrush(foreSecondary))
                            { g.DrawStr(description, Font, brushsub, new Rectangle(rect.X, rect.Bottom, rect.Width, heightDescription), stringLeft); }
                        }
                    }
                    if (showButton) IPaintButton(g, rect, fore, fillsecondary, size);
                }
            }
            else
            {
                var size = g.MeasureString(text ?? Config.NullText, Font).Size();
                bool showDescription = false;
                int heightDescription = rect.Height;
                if (description != null)
                {
                    showDescription = true;
                    heightDescription = rect.Height / 3;
                    rect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - heightDescription);
                }
                int u_x = IPaint(g, rect, fore, size.Height, 1F);
                rect.X += u_x;
                rect.Width -= u_x;
                using (var brush = new SolidBrush(forebase))
                {
                    g.DrawStr(text, Font, brush, rect, stringLeft);
                    if (desc != null)
                    {
                        int desc_t_w = size.Width + (int)(subGap * Config.Dpi);
                        using (var brushsub = new SolidBrush(foreSecondary))
                        {
                            g.DrawStr(desc, Font, brushsub, new Rectangle(rect.X + desc_t_w, rect.Y, rect.Width - desc_t_w, rect.Height), stringLeft);
                            if (showDescription) g.DrawStr(description, Font, brushsub, new Rectangle(rect.X, rect.Bottom, rect.Width, heightDescription), stringLeft);
                        }
                    }
                    else if (showDescription)
                    {
                        using (var brushsub = new SolidBrush(foreSecondary))
                        { g.DrawStr(description, Font, brushsub, new Rectangle(rect.X, rect.Bottom, rect.Width, heightDescription), stringLeft); }
                    }
                }
                if (showButton) IPaintButton(g, rect, fore, fillsecondary, size);
            }

            if (showDivider)
            {
                int thickness = (int)(dividerthickness * Config.Dpi), margin = (int)(dividerMargin * Config.Dpi);
                using (var brush = dividerColor.Brush(Style.Db.Split))
                {
                    g.FillRectangle(brush, new Rectangle(rect_.X + margin, rect_.Bottom - thickness, rect_.Width - margin * 2, thickness));
                }
            }
            base.OnPaint(e);
        }

        int IPaint(Graphics g, Rectangle rect, Color fore, int sHeight, float icon_ratio)
        {
            int u_x = 0;
            int _gap = (int)(gap.HasValue ? gap.Value * Config.Dpi : sHeight * .6F);
            int icon_size = (int)Math.Round(sHeight * .72F);
            if (showback || AnimationBack)
            {
                int backW = icon_size + _gap;
                if (AnimationBack) backW = (int)(backW * AnimationBackValue);
                if (showback)
                {
                    rect_back = new Rectangle(rect.X + u_x, rect.Y, backW + _gap, rect.Height);
                    var rect_icon = new Rectangle(rect.X + u_x + _gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                    if (hove_back.Down) PrintBackDown(g, rect_icon);
                    else if (hove_back.Animation) PrintBackHover(g, fore, rect_icon);
                    else if (hove_back.Switch) PrintBackHover(g, rect_icon);
                    else PrintBack(g, fore, rect_icon);
                }
                u_x += backW;
            }
            if (loading)
            {
                icon_size = sHeight;
                var rect_icon = new Rectangle(rect.X + u_x + _gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                using (var brush = new Pen(Color.FromArgb(170, fore), sHeight * .14F))
                {
                    brush.StartCap = brush.EndCap = LineCap.Round;
                    g.DrawArc(brush, rect_icon, AnimationLoadingValue, 100);
                }
                u_x += (icon_size + _gap);
            }
            else if (showicon)
            {
                icon_size = icon_ratio == 1 ? sHeight : (int)Math.Round(sHeight * icon_ratio);
                var rect_icon = new Rectangle(rect.X + u_x + _gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);

                bool showLeft = false;
                if (iconSvg != null)
                {
                    if (PrintLogo(g, iconSvg, fore, rect_icon)) showLeft = true;
                }
                if (!showLeft)
                {
                    if (icon != null)
                    {
                        g.DrawImage(icon, rect_icon);
                        showLeft = true;
                    }
                    else
                    {
                        var form = Parent.FindPARENT();
                        if (form != null && form.Icon != null)
                        {
                            g.DrawIcon(form.Icon, rect_icon);
                            showLeft = true;
                        }
                    }
                }
                u_x += (icon_size + _gap);
            }
            return u_x + _gap;
        }
        void IPaintButton(Graphics g, Rectangle rect, Color fore, Color fillsecondary, Size size)
        {
            int btn_size = (int)(size.Height * 1.2F), btn_x = (rect_close.Width - btn_size) / 2, btn_y = (rect_close.Height - btn_size) / 2;
            var rect_close_icon = new Rectangle(rect_close.X + btn_x, rect_close.Y + btn_y, btn_size, btn_size);
            if (hove_close.Down)
            {
                using (var brush = new SolidBrush(Style.Db.ErrorActive))
                {
                    g.FillRectangle(brush, rect_close);
                }
                PrintCloseHover(g, rect_close_icon);
            }
            else if (hove_close.Animation)
            {
                using (var brush = new SolidBrush(Helper.ToColor(hove_close.Value, Style.Db.Error)))
                {
                    g.FillRectangle(brush, rect_close);
                }
                PrintClose(g, fore, rect_close_icon);
                g.GetImgExtend(SvgDb.IcoAppClose, rect_close_icon, Helper.ToColor(hove_close.Value, Style.Db.ErrorColor));
            }
            else if (hove_close.Switch)
            {
                using (var brush = new SolidBrush(Style.Db.Error))
                {
                    g.FillRectangle(brush, rect_close);
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
                        g.FillRectangle(brush, rect_max);
                    }
                }
                else if (hove_max.Switch)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.FillRectangle(brush, rect_max);
                    }
                }
                if (hove_max.Down)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.FillRectangle(brush, rect_max);
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
                        g.FillRectangle(brush, rect_min);
                    }
                }
                else if (hove_min.Switch)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.FillRectangle(brush, rect_min);
                    }
                }
                if (hove_min.Down)
                {
                    using (var brush = new SolidBrush(fillsecondary))
                    {
                        g.FillRectangle(brush, rect_min);
                    }
                }
                PrintMin(g, fore, rect_min_icon);
            }
        }

        #region 渲染帮助

        Bitmap? temp_logo = null, temp_back = null, temp_back_hover = null, temp_back_down = null, temp_min = null, temp_max = null, temp_restore = null, temp_close = null, temp_close_hover = null;
        void PrintBack(Graphics g, Color color, Rectangle rect_icon)
        {
            if (temp_back == null || temp_back.Width != rect_icon.Width)
            {
                temp_back?.Dispose();
                temp_back = SvgExtend.GetImgExtend("ArrowLeftOutlined", rect_icon, color);
            }
            if (temp_back != null) g.DrawImage(temp_back, rect_icon);
        }
        void PrintBackHover(Graphics g, Color color, Rectangle rect_icon)
        {
            PrintBack(g, color, rect_icon);
            g.GetImgExtend("ArrowLeftOutlined", rect_icon, Helper.ToColor(hove_back.Value, Style.Db.Primary));
        }
        void PrintBackHover(Graphics g, Rectangle rect_icon)
        {
            if (temp_back_hover == null || temp_back_hover.Width != rect_icon.Width)
            {
                temp_back_hover?.Dispose();
                temp_back_hover = SvgExtend.GetImgExtend("ArrowLeftOutlined", rect_icon, Style.Db.Primary);
            }
            if (temp_back_hover != null) g.DrawImage(temp_back_hover, rect_icon);
        }
        void PrintBackDown(Graphics g, Rectangle rect_icon)
        {
            if (temp_back_down == null || temp_back_down.Width != rect_icon.Width)
            {
                temp_back_down?.Dispose();
                temp_back_down = SvgExtend.GetImgExtend("ArrowLeftOutlined", rect_icon, Style.Db.PrimaryActive);
            }
            if (temp_back_down != null) g.DrawImage(temp_back_down, rect_icon);
        }
        void PrintClose(Graphics g, Color color, Rectangle rect_icon)
        {
            if (temp_close == null || temp_close.Width != rect_icon.Width)
            {
                temp_close?.Dispose();
                temp_close = SvgExtend.GetImgExtend(SvgDb.IcoAppClose, rect_icon, color);
            }
            if (temp_close != null) g.DrawImage(temp_close, rect_icon);
        }
        void PrintCloseHover(Graphics g, Rectangle rect_icon)
        {
            if (temp_close_hover == null || temp_close_hover.Width != rect_icon.Width)
            {
                temp_close_hover?.Dispose();
                temp_close_hover = SvgExtend.GetImgExtend(SvgDb.IcoAppClose, rect_icon, Style.Db.ErrorColor);
            }
            if (temp_close_hover != null) g.DrawImage(temp_close_hover, rect_icon);
        }
        void PrintMax(Graphics g, Color color, Rectangle rect_icon)
        {
            if (temp_max == null || temp_max.Width != rect_icon.Width)
            {
                temp_max?.Dispose();
                temp_max = SvgExtend.GetImgExtend(SvgDb.IcoAppMax, rect_icon, color);
            }
            if (temp_max != null) g.DrawImage(temp_max, rect_icon);
        }
        void PrintRestore(Graphics g, Color color, Rectangle rect_icon)
        {
            if (temp_restore == null || temp_restore.Width != rect_icon.Width)
            {
                temp_restore?.Dispose();
                temp_restore = SvgExtend.GetImgExtend(SvgDb.IcoAppRestore, rect_icon, color);
            }
            if (temp_restore != null) g.DrawImage(temp_restore, rect_icon);
        }
        void PrintMin(Graphics g, Color color, Rectangle rect_icon)
        {
            if (temp_min == null || temp_min.Width != rect_icon.Width)
            {
                temp_min?.Dispose();
                temp_min = SvgExtend.GetImgExtend(SvgDb.IcoAppMin, rect_icon, color);
            }
            if (temp_min != null) g.DrawImage(temp_min, rect_icon);
        }
        bool PrintLogo(Graphics g, string svg, Color color, Rectangle rect_icon)
        {
            if (temp_logo == null || temp_logo.Width != rect_icon.Width)
            {
                temp_logo?.Dispose();
                temp_logo = SvgExtend.GetImgExtend(svg, rect_icon, color);
            }
            if (temp_logo != null) { g.DrawImage(temp_logo, rect_icon); return true; }
            return false;
        }

        void DisposeBmp()
        {
            temp_logo?.Dispose();
            temp_back?.Dispose();
            temp_back_hover?.Dispose();
            temp_back_down?.Dispose();
            temp_min?.Dispose();
            temp_max?.Dispose();
            temp_restore?.Dispose();
            temp_close?.Dispose();
            temp_logo = null;
            temp_back = temp_back_hover = temp_back_down = null;
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
            if (CloseSize > 0 && showButton)
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
                    if (form is BaseForm form_win) IsMax = form_win.IsMax;
                    else IsMax = form.WindowState == FormWindowState.Maximized;
                }
            }
            base.OnSizeChanged(e);
        }

        #region 动画

        ITask? ThreadBack = null;
        ITaskOpacity hove_back, hove_close, hove_max, hove_min;
        public PageHeader() { hove_back = new ITaskOpacity(this); hove_close = new ITaskOpacity(this); hove_max = new ITaskOpacity(this); hove_min = new ITaskOpacity(this); }

        #endregion

        #region 鼠标

        Rectangle rect_back, rect_close, rect_max, rect_min;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (showButton)
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
            }
            if (showback) hove_back.Switch = rect_back.Contains(e.Location);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hove_back.Switch = hove_close.Switch = hove_max.Switch = hove_min.Switch = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (showButton)
                {
                    hove_close.Down = rect_close.Contains(e.Location);
                    hove_max.Down = rect_max.Contains(e.Location);
                    hove_min.Down = rect_min.Contains(e.Location);
                    if (hove_close.Down || hove_max.Down || hove_min.Down) return;
                }
                if (showback)
                {
                    hove_back.Down = rect_back.Contains(e.Location);
                    if (hove_back.Down) return;
                }
                if (DragMove)
                {
                    var form = Parent.FindPARENT();
                    if (form != null)
                    {
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
            if (showButton)
            {
                if (hove_close.Down && rect_close.Contains(e.Location)) Parent.FindPARENT()?.Close();
                else if (hove_max.Down && rect_max.Contains(e.Location))
                {
                    var form = Parent.FindPARENT();
                    if (form != null)
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
                    }
                }
                else if (hove_min.Down && rect_min.Contains(e.Location))
                {
                    var form = Parent.FindPARENT();
                    if (form != null) form.WindowState = FormWindowState.Minimized;
                }
            }
            if (showback)
            {
                if (hove_back.Down && rect_back.Contains(e.Location)) BackClick?.Invoke(this, EventArgs.Empty);
            }
            hove_back.Down = hove_close.Down = hove_max.Down = hove_min.Down = false;
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

        #region 事件

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        public event EventHandler? BackClick;

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
                        if (showback && BackClick != null) BackClick(this, EventArgs.Empty);
                        else Parent.FindPARENT()?.Close();
                        return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        #endregion
    }
}