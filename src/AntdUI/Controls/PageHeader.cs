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
                DisposeBmp();
                Invalidate();
                OnPropertyChanged(nameof(Mode));
            }
        }

        string? text = null;
        /// <summary>
        /// 文字
        /// </summary>
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
                OnPropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// 国际化文字
        /// </summary>
        [Description("文字"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        /// <summary>
        /// 使用标题大小
        /// </summary>
        [Description("使用标题大小"), Category("外观"), DefaultValue(false)]
        public bool UseTitleFont { get; set; }

        /// <summary>
        /// 标题使用粗体
        /// </summary>
        [Description("标题使用粗体"), Category("外观"), DefaultValue(true)]
        public bool UseTextBold { get; set; } = true;

        /// <summary>
        /// 副标题居中
        /// </summary>
        [Description("副标题居中"), Category("外观"), DefaultValue(false)]
        public bool UseSubCenter { get; set; }

        bool useLeftMargin = true;
        /// <summary>
        /// 使用左边边距
        /// </summary>
        [Description("使用左边边距"), Category("外观"), DefaultValue(true)]
        public bool UseLeftMargin
        {
            get => useLeftMargin;
            set
            {
                if (useLeftMargin == value) return;
                useLeftMargin = value;
                SizeChange();
                IOnSizeChanged();
            }
        }

        string? desc = null;
        /// <summary>
        /// 副标题
        /// </summary>
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
                OnPropertyChanged(nameof(SubText));
            }
        }

        Font? descFont = null;
        /// <summary>
        /// 副标题字体
        /// </summary>
        [Description("副标题字体"), Category("外观"), DefaultValue(null)]
        public Font? SubFont
        {
            get => descFont;
            set
            {
                if (descFont == value) return;
                descFont = value;
                Invalidate();
                OnPropertyChanged(nameof(SubFont));
            }
        }

        /// <summary>
        /// 国际化副标题
        /// </summary>
        [Description("副标题"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSubText { get; set; }

        string? description = null;
        /// <summary>
        /// 描述文本
        /// </summary>
        [Description("描述文本"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? Description
        {
            get => this.GetLangI(LocalizationDescription, description);
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (description == value) return;
                description = value;
                Invalidate();
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// 国际化描述文本
        /// </summary>
        [Description("描述文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationDescription { get; set; }

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
                OnPropertyChanged(nameof(Gap));
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
                OnPropertyChanged(nameof(SubGap));
            }
        }

        bool useSystemStyleColor = false;
        /// <summary>
        /// 使用系统颜色
        /// </summary>
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
                OnPropertyChanged(nameof(UseSystemStyleColor));
            }
        }

        bool cancelButton = false;
        /// <summary>
        /// 点击退出关闭
        /// </summary>
        [Description("点击退出关闭"), Category("行为"), DefaultValue(false)]
        public bool CancelButton
        {
            get => cancelButton;
            set
            {
                if (cancelButton == value) return;
                cancelButton = value;
                if (IsHandleCreated) HandCancelButton(value);
            }
        }
        void HandCancelButton(bool value)
        {
            var form = Parent.FindPARENT();
            if (form is BaseForm formb)
            {
                if (value)
                {
                    formb.ONESC = () =>
                    {
                        if (showback && BackClick != null) BackClick(this, EventArgs.Empty);
                        else formb.Close();
                    };
                }
                else formb.ONESC = null;
            }
        }

        #region 图标

        bool showicon = false;
        /// <summary>
        /// 是否显示图标
        /// </summary>
        [Description("是否显示图标"), Category("外观"), DefaultValue(false)]
        public bool ShowIcon
        {
            get => showicon;
            set
            {
                if (showicon == value) return;
                showicon = value;
                Invalidate();
                OnPropertyChanged(nameof(ShowIcon));
            }
        }

        Image? icon = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidate();
                OnPropertyChanged(nameof(Icon));
            }
        }

        string? iconSvg = null;
        /// <summary>
        /// 图标SVG
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidate();
                OnPropertyChanged(nameof(IconSvg));
            }
        }

        float? iconratio;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(null)]
        public float? IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                Invalidate();
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        #endregion

        #region 加载动画

        bool loading = false;
        int AnimationLoadingValue = 0;
        /// <summary>
        /// 加载状态
        /// </summary>
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
                OnPropertyChanged(nameof(Loading));
            }
        }

        protected override void Dispose(bool disposing)
        {
            ThreadBack?.Dispose();
            hove_back.Dispose();
            hove_close.Dispose();
            hove_full.Dispose();
            hove_max.Dispose();
            hove_min.Dispose();
            ThreadLoading?.Dispose();
            temp_logo?.Dispose();
            temp_back?.Dispose();
            temp_back_hover?.Dispose();
            temp_back_down?.Dispose();
            temp_full?.Dispose();
            temp_full_restore?.Dispose();
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
                if (Config.HasAnimation(nameof(PageHeader)) && IsHandleCreated)
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
                OnPropertyChanged(nameof(ShowBack));
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
                SizeChange();
                IOnSizeChanged();
                Invalidate();
                OnPropertyChanged(nameof(ShowButton));
            }
        }

        bool fullBox = false;
        /// <summary>
        /// 是否显示全屏按钮
        /// </summary>
        [Description("是否显示全屏按钮"), Category("外观"), DefaultValue(false)]
        public bool FullBox
        {
            get => fullBox;
            set
            {
                if (fullBox == value) return;
                fullBox = value;
                if (showButton)
                {
                    SizeChange();
                    IOnSizeChanged();
                    Invalidate();
                }
                OnPropertyChanged(nameof(FullBox));
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
                    SizeChange();
                    IOnSizeChanged();
                    Invalidate();
                }
                OnPropertyChanged(nameof(MaximizeBox));
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
                    SizeChange();
                    IOnSizeChanged();
                    Invalidate();
                }
                OnPropertyChanged(nameof(MinimizeBox));
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

        bool isfull = false;
        /// <summary>
        /// 是否全屏
        /// </summary>
        [Description("是否全屏"), Category("外观"), DefaultValue(false)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFull
        {
            get => isfull;
            set
            {
                if (isfull == value) return;
                isfull = value;
                if (showButton) Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 是否可以拖动位置
        /// </summary>
        [Description("是否可以拖动位置"), Category("行为"), DefaultValue(true)]
        public bool DragMove { get; set; } = true;

        /// <summary>
        /// 关闭按钮大小
        /// </summary>
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
                OnPropertyChanged(nameof(DividerShow));
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
                OnPropertyChanged(nameof(DividerColor));
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
                OnPropertyChanged(nameof(DividerThickness));
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
                OnPropertyChanged(nameof(DividerMargin));
            }
        }

        #endregion

        #region 背景

        string? backExtend = null;
        /// <summary>
        /// 背景渐变色
        /// </summary>
        [Description("背景渐变色"), Category("外观"), DefaultValue(null)]
        public string? BackExtend
        {
            get => backExtend;
            set
            {
                if (backExtend == value) return;
                backExtend = value;
                Invalidate();
                OnPropertyChanged(nameof(BackExtend));
            }
        }

        #endregion

        #endregion

        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Padding, useLeftMargin ? hasl : 0, 0, hasr, 0);

        StringFormat stringLeft = Helper.SF_ALL(lr: StringAlignment.Near);
        StringFormat stringCenter = Helper.SF_ALL();

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect_ = ClientRectangle;
            if (rect_.Width == 0 || rect_.Height == 0) return;
            var rect = rect_.PaddingRect(Padding, 0, 0, hasr, 0);
            var g = e.Graphics.High();

            backExtend.BrushEx(rect_, g);

            #region 显示颜色

            Color fore = Colour.Text.Get("PageHeader", mode), forebase = Colour.TextBase.Get("PageHeader", mode), foreSecondary = Colour.TextSecondary.Get("PageHeader", mode),
                fillsecondary = Colour.FillSecondary.Get("PageHeader", mode);
            if (useSystemStyleColor) forebase = ForeColor;

            #endregion

            if (UseTitleFont)
            {
                using (var fontTitle = new Font(Font.FontFamily, Font.Size * 1.44F, UseTextBold ? FontStyle.Bold : Font.Style))
                {
                    IPaint(g, rect_, rect, g.MeasureString(Config.NullText, Font), iconratio ?? 1.36F, fontTitle, fore, forebase, foreSecondary, fillsecondary);
                }
            }
            else IPaint(g, rect_, rect, g.MeasureString(Text ?? Config.NullText, Font), iconratio ?? 1F, null, fore, forebase, foreSecondary, fillsecondary);
            this.PaintBadge(g);
            if (showDivider)
            {
                int thickness = (int)(dividerthickness * Config.Dpi), margin = (int)(dividerMargin * Config.Dpi);
                using (var brush = dividerColor.Brush(Colour.Split.Get("PageHeader")))
                {
                    g.Fill(brush, new Rectangle(rect_.X + margin, rect_.Bottom - thickness, rect_.Width - margin * 2, thickness));
                }
            }
            base.OnPaint(e);
        }

        void IPaint(Canvas g, Rectangle rect, Rectangle rect_real, Size size, float ratio, Font? fontTitle, Color fore, Color forebase, Color foreSecondary, Color fillsecondary)
        {
            bool showDescription = false;
            int heightDescription = rect_real.Height;
            if (Description != null)
            {
                showDescription = true;
                heightDescription = rect_real.Height / 3;
                rect_real = new Rectangle(rect_real.X, rect_real.Y, rect_real.Width, rect_real.Height - heightDescription);
            }
            int u_x = IPaint(g, rect_real, fore, size.Height, ratio);
            int rl = u_x;
            rect_real.X += u_x;
            rect_real.Width -= u_x;
            using (var brush = new SolidBrush(forebase))
            {
                int size_w = size.Width;
                if (fontTitle == null) g.String(Text, Font, brush, rect_real, stringLeft);
                else
                {
                    var sizeTitle = g.MeasureString(Text, fontTitle);
                    g.String(Text, fontTitle, brush, rect_real, stringLeft);
                    size_w = sizeTitle.Width;
                }
                rl += size_w;
                if (SubText != null)
                {
                    int desc_t_w = size_w + (int)(subGap * Config.Dpi);
                    using (var brushsub = new SolidBrush(foreSecondary))
                    {
                        if (UseSubCenter) g.String(SubText, descFont ?? Font, brushsub, rect, stringCenter);
                        else
                        {
                            g.String(SubText, descFont ?? Font, brushsub, new Rectangle(rect_real.X + desc_t_w, rect_real.Y, rect_real.Width - desc_t_w, rect_real.Height), stringLeft);
                            if (useLeftMargin) rl = u_x + desc_t_w + g.MeasureString(SubText, descFont ?? Font).Width;
                        }
                        if (showDescription) g.String(Description, Font, brushsub, new Rectangle(rect_real.X, rect_real.Bottom, rect_real.Width, heightDescription), stringLeft);
                    }
                }
                else if (showDescription)
                {
                    using (var brushsub = new SolidBrush(foreSecondary))
                    { g.String(Description, Font, brushsub, new Rectangle(rect_real.X, rect_real.Bottom, rect_real.Width, heightDescription), stringLeft); }
                }
            }
            hasl = rl;
            if (showButton) IPaintButton(g, rect_real, fore, fillsecondary, size);
        }

        public Rectangle GetTitleRect(Canvas g)
        {
            var rect = ClientRectangle.PaddingRect(Padding, 0, 0, hasr, 0);
            var size = g.MeasureString(Text ?? Config.NullText, Font);
            if (UseTitleFont)
            {
                using (var fontTitle = new Font(Font.FontFamily, Font.Size * 1.44F, UseTextBold ? FontStyle.Bold : Font.Style))
                {
                    var sizeTitle = g.MeasureString(Text, fontTitle);
                    rect.X += IPaintS(g, rect, size.Height, iconratio ?? 1.36F) / 2;
                    return new Rectangle(rect.X, rect.Y + (rect.Height - sizeTitle.Height) / 2, sizeTitle.Width, sizeTitle.Height);
                }
            }
            else
            {
                rect.X += IPaintS(g, rect, size.Height, iconratio ?? 1F) / 2;
                return new Rectangle(rect.X, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
            }
        }

        int IPaintS(Canvas g, Rectangle rect, int sHeight, float icon_ratio)
        {
            int u_x = 0;
            int _gap = (int)(gap.HasValue ? gap.Value * Config.Dpi : sHeight * .6F);
            int icon_size = (int)Math.Round(sHeight * .72F);
            if (showback || AnimationBack)
            {
                int backW = icon_size + _gap;
                if (AnimationBack) backW = (int)(backW * AnimationBackValue);
                u_x += backW;
            }
            if (loading)
            {
                icon_size = sHeight;
                u_x += (icon_size + _gap);
            }
            else if (showicon)
            {
                icon_size = icon_ratio == 1 ? sHeight : (int)Math.Round(sHeight * icon_ratio);
                u_x += (icon_size + _gap);
            }
            return u_x + _gap;
        }

        int IPaint(Canvas g, Rectangle rect, Color fore, int sHeight, float icon_ratio)
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
                using (var pen = new Pen(Colour.Fill.Get("PageHeader"), sHeight * .14F))
                using (var brush = new Pen(Color.FromArgb(170, fore), pen.Width))
                {
                    g.DrawEllipse(pen, rect_icon);
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
                        g.Image(icon, rect_icon);
                        showLeft = true;
                    }
                    else
                    {
                        var form = Parent.FindPARENT();
                        if (form != null && form.Icon != null)
                        {
                            g.Icon(form.Icon, rect_icon);
                            showLeft = true;
                        }
                    }
                }
                u_x += (icon_size + _gap);
            }
            return u_x + _gap;
        }
        void IPaintButton(Canvas g, Rectangle rect, Color fore, Color fillsecondary, Size size)
        {
            int btn_size = (int)(size.Height * 1.2F), btn_x = (rect_close.Width - btn_size) / 2, btn_y = (rect_close.Height - btn_size) / 2;
            var rect_close_icon = new Rectangle(rect_close.X + btn_x, rect_close.Y + btn_y, btn_size, btn_size);
            if (hove_close.Down)
            {
                g.Fill(Colour.ErrorActive.Get("PageHeader"), rect_close);
                PrintCloseHover(g, rect_close_icon);
            }
            else if (hove_close.Animation)
            {
                g.Fill(Helper.ToColor(hove_close.Value, Colour.Error.Get("PageHeader")), rect_close);
                PrintClose(g, fore, rect_close_icon);
                g.GetImgExtend(SvgDb.IcoAppClose, rect_close_icon, Helper.ToColor(hove_close.Value, Colour.ErrorColor.Get("PageHeader")));
            }
            else if (hove_close.Switch)
            {
                g.Fill(Colour.Error.Get("PageHeader"), rect_close);
                PrintCloseHover(g, rect_close_icon);
            }
            else PrintClose(g, fore, rect_close_icon);

            if (fullBox)
            {
                var rect_full_icon = new Rectangle(rect_full.X + btn_x, rect_full.Y + btn_y, btn_size, btn_size);
                if (hove_full.Animation) g.Fill(Helper.ToColor(hove_full.Value, fillsecondary), rect_full);
                else if (hove_full.Switch) g.Fill(fillsecondary, rect_full);
                if (hove_full.Down) g.Fill(fillsecondary, rect_full);
                if (IsFull) PrintFullRestore(g, fore, rect_full_icon);
                else PrintFull(g, fore, rect_full_icon);
            }

            if (maximizeBox)
            {
                var rect_max_icon = new Rectangle(rect_max.X + btn_x, rect_max.Y + btn_y, btn_size, btn_size);
                if (hove_max.Animation) g.Fill(Helper.ToColor(hove_max.Value, fillsecondary), rect_max);
                else if (hove_max.Switch) g.Fill(fillsecondary, rect_max);
                if (hove_max.Down) g.Fill(fillsecondary, rect_max);
                if (IsMax) PrintRestore(g, fore, rect_max_icon);
                else PrintMax(g, fore, rect_max_icon);
            }
            if (minimizeBox)
            {
                var rect_min_icon = new Rectangle(rect_min.X + btn_x, rect_min.Y + btn_y, btn_size, btn_size);
                if (hove_min.Animation) g.Fill(Helper.ToColor(hove_min.Value, fillsecondary), rect_min);
                else if (hove_min.Switch) g.Fill(fillsecondary, rect_min);
                if (hove_min.Down) g.Fill(fillsecondary, rect_min);
                PrintMin(g, fore, rect_min_icon);
            }
        }

        #region 渲染帮助

        Bitmap? temp_logo = null, temp_back = null, temp_back_hover = null, temp_back_down = null, temp_full = null, temp_full_restore = null, temp_min = null, temp_max = null, temp_restore = null, temp_close = null, temp_close_hover = null;
        void PrintBack(Canvas g, Color color, Rectangle rect_icon)
        {
            if (temp_back == null || temp_back.Width != rect_icon.Width)
            {
                temp_back?.Dispose();
                temp_back = SvgExtend.GetImgExtend("ArrowLeftOutlined", rect_icon, color);
            }
            if (temp_back != null) g.Image(temp_back, rect_icon);
        }
        void PrintBackHover(Canvas g, Color color, Rectangle rect_icon)
        {
            PrintBack(g, color, rect_icon);
            g.GetImgExtend("ArrowLeftOutlined", rect_icon, Helper.ToColor(hove_back.Value, Colour.Primary.Get("PageHeader")));
        }
        void PrintBackHover(Canvas g, Rectangle rect_icon)
        {
            if (temp_back_hover == null || temp_back_hover.Width != rect_icon.Width)
            {
                temp_back_hover?.Dispose();
                temp_back_hover = SvgExtend.GetImgExtend("ArrowLeftOutlined", rect_icon, Colour.Primary.Get("PageHeader"));
            }
            if (temp_back_hover != null) g.Image(temp_back_hover, rect_icon);
        }
        void PrintBackDown(Canvas g, Rectangle rect_icon)
        {
            if (temp_back_down == null || temp_back_down.Width != rect_icon.Width)
            {
                temp_back_down?.Dispose();
                temp_back_down = SvgExtend.GetImgExtend("ArrowLeftOutlined", rect_icon, Colour.PrimaryActive.Get("PageHeader"));
            }
            if (temp_back_down != null) g.Image(temp_back_down, rect_icon);
        }
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
                temp_close_hover = SvgExtend.GetImgExtend(SvgDb.IcoAppClose, rect_icon, Colour.ErrorColor.Get("PageHeader"));
            }
            if (temp_close_hover != null) g.Image(temp_close_hover, rect_icon);
        }
        void PrintFull(Canvas g, Color color, Rectangle rect_icon)
        {
            if (temp_full == null || temp_full.Width != rect_icon.Width)
            {
                temp_full?.Dispose();
                temp_full = SvgExtend.GetImgExtend(SvgDb.IcoAppFull, rect_icon, color);
            }
            if (temp_full != null) g.Image(temp_full, rect_icon);
        }
        void PrintFullRestore(Canvas g, Color color, Rectangle rect_icon)
        {
            if (temp_full_restore == null || temp_full_restore.Width != rect_icon.Width)
            {
                temp_full_restore?.Dispose();
                temp_full_restore = SvgExtend.GetImgExtend(SvgDb.IcoAppFullRestore, rect_icon, color);
            }
            if (temp_full_restore != null) g.Image(temp_full_restore, rect_icon);
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
            temp_back?.Dispose();
            temp_back_hover?.Dispose();
            temp_back_down?.Dispose();
            temp_full?.Dispose();
            temp_full_restore?.Dispose();
            temp_min?.Dispose();
            temp_max?.Dispose();
            temp_restore?.Dispose();
            temp_close?.Dispose();
            temp_logo = null;
            temp_back = temp_back_hover = temp_back_down = null;
            temp_full = null;
            temp_full_restore = null;
            temp_min = null;
            temp_max = null;
            temp_restore = null;
            temp_close = null;
        }

        #endregion

        #endregion

        bool setsize = false;
        int _hasl = 0, hasr = 0;
        int hasl
        {
            get => _hasl;
            set
            {
                if (_hasl == value) return;
                _hasl = value;
                setsize = true;
                SizeChange();
                if (useLeftMargin) IOnSizeChanged();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SizeChange();
        }

        void SizeChange()
        {
            if (setsize)
            {
                setsize = false;
                return;
            }
            var rect = ClientRectangle.PaddingRect(Padding);
            int rr = 0;
            if (CloseSize > 0 && showButton)
            {
                int btn_size = (fullBox || maximizeBox || minimizeBox) ? (int)Math.Round(CloseSize * Config.Dpi) : (int)Math.Round((CloseSize - 8) * Config.Dpi);
                rect_close = new Rectangle(rect.Right - btn_size, rect.Y, btn_size, rect.Height);
                rr = btn_size;
                int left = rect_close.Left;
                if (fullBox)
                {
                    rect_full = new Rectangle(left - btn_size, rect.Y, btn_size, rect.Height);
                    left -= btn_size;
                    rr += btn_size;
                }
                if (maximizeBox)
                {
                    rect_max = new Rectangle(left - btn_size, rect.Y, btn_size, rect.Height);
                    left -= btn_size;
                    rr += btn_size;
                }
                if (minimizeBox)
                {
                    rect_min = new Rectangle(left - btn_size, rect.Y, btn_size, rect.Height);
                    rr += btn_size;
                }
            }
            hasr = rr;
            if (DragMove)
            {
                var form = Parent.FindPARENT();
                if (form != null)
                {
                    if (form is LayeredFormDrawer) return;
                    if (form is BaseForm form_win)
                    {
                        IsMax = form_win.IsMax;
                        IsFull = form_win.IsFull;
                    }
                    else
                    {
                        IsMax = form.WindowState == FormWindowState.Maximized;
                        if (IsMax) IsFull = form.FormBorderStyle == FormBorderStyle.None;
                        else IsFull = false;
                    }
                }
            }
        }

        #region 动画

        ITask? ThreadBack = null;
        ITaskOpacity hove_back, hove_close, hove_full, hove_max, hove_min;
        public PageHeader()
        {
            var key = nameof(PageHeader);
            hove_back = new ITaskOpacity(key, this);
            hove_close = new ITaskOpacity(key, this);
            hove_full = new ITaskOpacity(key, this);
            hove_max = new ITaskOpacity(key, this);
            hove_min = new ITaskOpacity(key, this);
        }

        #endregion

        #region 鼠标

        Rectangle rect_back, rect_close, rect_full, rect_max, rect_min;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (showButton)
            {
                bool _close = rect_close.Contains(e.X, e.Y), _full = fullBox && rect_full.Contains(e.X, e.Y), _max = maximizeBox && rect_max.Contains(e.X, e.Y), _min = minimizeBox && rect_min.Contains(e.X, e.Y);
                if (_close != hove_close.Switch || _full != hove_full.Switch || _max != hove_max.Switch || _min != hove_min.Switch)
                {
                    var fillsecondary = Colour.FillSecondary.Get("PageHeader", mode);
                    hove_max.MaxValue = hove_min.MaxValue = hove_full.MaxValue = fillsecondary.A;
                    hove_close.Switch = _close;
                    hove_full.Switch = _full;
                    hove_max.Switch = _max;
                    hove_min.Switch = _min;
                }
            }
            if (showback) hove_back.Switch = rect_back.Contains(e.X, e.Y);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hove_back.Switch = hove_close.Switch = hove_full.Switch = hove_max.Switch = hove_min.Switch = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (showButton)
                {
                    hove_close.Down = rect_close.Contains(e.X, e.Y);
                    hove_full.Down = fullBox && rect_full.Contains(e.X, e.Y);
                    hove_max.Down = maximizeBox && rect_max.Contains(e.X, e.Y);
                    hove_min.Down = minimizeBox && rect_min.Contains(e.X, e.Y);
                    if (hove_close.Down || hove_full.Down || hove_max.Down || hove_min.Down) return;
                }
                if (showback)
                {
                    hove_back.Down = rect_back.Contains(e.X, e.Y);
                    if (hove_back.Down) return;
                }
                if (DragMove)
                {
                    var form = Parent.FindPARENT();
                    if (form != null)
                    {
                        if (form is LayeredFormDrawer || form is LayeredFormPopover) return;
                        if (e.Clicks > 1)
                        {
                            if (maximizeBox)
                            {
                                isfull = false;
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
                if (hove_close.Down && rect_close.Contains(e.X, e.Y)) Parent.FindPARENT()?.Close();
                else if (hove_full.Down && rect_full.Contains(e.X, e.Y))
                {
                    var form = Parent.FindPARENT();
                    if (form != null)
                    {
                        if (form is LayeredFormDrawer) return;
                        if (form is BaseForm form_win) IsFull = form_win.FullRestore();
                        else
                        {
                            if (form.WindowState == FormWindowState.Maximized)
                            {
                                IsFull = false;
                                form.FormBorderStyle = FormBorderStyle.Sizable;
                                form.WindowState = FormWindowState.Normal;
                            }
                            else
                            {
                                IsFull = true;
                                form.FormBorderStyle = FormBorderStyle.None;
                                form.WindowState = FormWindowState.Maximized;
                            }
                        }
                    }
                }
                else if (hove_max.Down && rect_max.Contains(e.X, e.Y))
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
                else if (hove_min.Down && rect_min.Contains(e.X, e.Y))
                {
                    var form = Parent.FindPARENT();
                    if (form != null)
                    {
                        if (form is LayeredFormDrawer) return;
                        if (form is BaseForm form_win) form_win.Min();
                        else form.WindowState = FormWindowState.Minimized;
                    }
                }
            }
            if (showback)
            {
                if (hove_back.Down && rect_back.Contains(e.X, e.Y)) BackClick?.Invoke(this, EventArgs.Empty);
            }
            hove_back.Down = hove_close.Down = hove_full.Down = hove_max.Down = hove_min.Down = false;
            base.OnMouseUp(e);
        }

        #region 主题变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (cancelButton) HandCancelButton(cancelButton);
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