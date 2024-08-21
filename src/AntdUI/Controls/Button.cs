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
    /// Button 按钮
    /// </summary>
    /// <remarks>按钮用于开始一个即时操作。</remarks>
    [Description("Button 按钮")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Button : IControl, IButtonControl
    {
        IButton button;

        public Button()
        {
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
            base.BackColor = Color.Transparent;
            button = new IButton(this, () => BeforeAutoSize());
        }

        #region 属性

        #region 系统

        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => button.Back;
            set => button.Back = value;
        }

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => button.Fore;
            set => button.Fore = value;
        }

        #endregion

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Obsolete("使用 ForeColor 属性替代"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color? Fore
        {
            get => button.Fore;
            set => button.Fore = value;
        }

        #region 背景

        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Obsolete("使用 BackColor 属性替代"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color? Back
        {
            get => button.Back;
            set => button.Back = value;
        }

        /// <summary>
        /// 背景渐变色
        /// </summary>
        [Description("背景渐变色"), Category("外观"), DefaultValue(null)]
        public string? BackExtend
        {
            get => button.BackExtend;
            set => button.BackExtend = value;
        }

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackHover
        {
            get => button.BackHover;
            set => button.BackHover = value;
        }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackActive
        {
            get => button.BackActive;
            set => button.BackActive = value;
        }

        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public new Image? BackgroundImage
        {
            get => button.BackgroundImage;
            set => button.BackgroundImage = value;
        }

        /// <summary>
        /// 背景图片布局
        /// </summary>
        [Description("背景图片布局"), Category("外观"), DefaultValue(TFit.Fill)]
        public new TFit BackgroundImageLayout
        {
            get => button.BackgroundImageLayout;
            set => button.BackgroundImageLayout = value;
        }

        #endregion

        #region 默认样式

        /// <summary>
        /// Default模式背景颜色
        /// </summary>
        [Description("Default模式背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? DefaultBack
        {
            get => button.DefaultBack;
            set => button.DefaultBack = value;
        }

        /// <summary>
        /// Default模式边框颜色
        /// </summary>
        [Description("Default模式边框颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? DefaultBorderColor
        {
            get => button.DefaultBorderColor;
            set => button.DefaultBorderColor = value;
        }

        #endregion

        #region 边框

        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(0F)]
        public float BorderWidth
        {
            get => button.BorderWidth;
            set => button.BorderWidth = value;
        }

        #endregion

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category("外观"), DefaultValue(4)]
        public int WaveSize
        {
            get => button.WaveSize;
            set => button.WaveSize = value;
        }

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => button.Radius;
            set => button.Radius = value;
        }

        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category("外观"), DefaultValue(TShape.Default)]
        public TShape Shape
        {
            get => button.Shape;
            set => button.Shape = value;
        }

        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型"), Category("外观"), DefaultValue(TTypeMini.Default)]
        public TTypeMini Type
        {
            get => button.Type;
            set => button.Type = value;
        }

        /// <summary>
        /// 幽灵属性，使按钮背景透明
        /// </summary>
        [Description("幽灵属性，使按钮背景透明"), Category("外观"), DefaultValue(false)]
        public bool Ghost
        {
            get => button.Ghost;
            set => button.Ghost = value;
        }

        /// <summary>
        /// 响应真实区域
        /// </summary>
        [Description("响应真实区域"), Category("行为"), DefaultValue(false)]
        public bool RespondRealAreas { get; set; }

        /// <summary>
        /// 显示箭头
        /// </summary>
        [Description("显示箭头"), Category("行为"), DefaultValue(false)]
        public bool ShowArrow
        {
            get => button.ShowArrow;
            set => button.ShowArrow = value;
        }

        /// <summary>
        /// 箭头链接样式
        /// </summary>
        [Description("箭头链接样式"), Category("行为"), DefaultValue(false)]
        public bool IsLink
        {
            get => button.IsLink;
            set => button.IsLink = value;
        }

        /// <summary>
        /// 箭头角度
        /// </summary>
        [Browsable(false), Description("箭头角度"), Category("外观"), DefaultValue(-1F)]
        public float ArrowProg
        {
            get => button.ArrowProg;
            set => button.ArrowProg = value;
        }

        #region 文本

        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public new string? Text
        {
            get => button.Text;
            set => button.Text = value;
        }

        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("外观"), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign
        {
            get => button.TextAlign;
            set => button.TextAlign = value;
        }

        /// <summary>
        /// 文本超出自动处理
        /// </summary>
        [Description("文本超出自动处理"), Category("行为"), DefaultValue(false)]
        public bool AutoEllipsis
        {
            get => button.AutoEllipsis;
            set => button.AutoEllipsis = value;
        }

        /// <summary>
        /// 是否多行
        /// </summary>
        [Description("是否多行"), Category("行为"), DefaultValue(false)]
        public bool TextMultiLine
        {
            get => button.TextMultiLine;
            set => button.TextMultiLine = value;
        }

        #endregion

        #region 图标

        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(.7F)]
        public float IconRatio
        {
            get => button.IconRatio;
            set => button.IconRatio = value;
        }

        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => button.Icon;
            set => button.Icon = value;
        }

        /// <summary>
        /// 图标SVG
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => button.IconSvg;
            set => button.IconSvg = value;
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => button.HasIcon;

        /// <summary>
        /// 图标大小
        /// </summary>
        [Description("图标大小"), Category("外观"), DefaultValue(typeof(Size), "0, 0")]
        public Size IconSize
        {
            get => button.IconSize;
            set => button.IconSize = value;
        }

        /// <summary>
        /// 悬停图标
        /// </summary>
        [Description("悬停图标"), Category("外观"), DefaultValue(null)]
        public Image? IconHover
        {
            get => button.IconHover;
            set => button.IconHover = value;
        }

        /// <summary>
        /// 悬停图标SVG
        /// </summary>
        [Description("悬停图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconHoverSvg
        {
            get => button.IconHoverSvg;
            set => button.IconHoverSvg = value;
        }

        /// <summary>
        /// 悬停图标动画时长
        /// </summary>
        [Description("悬停图标动画时长"), Category("外观"), DefaultValue(200)]
        public int IconHoverAnimation
        {
            get => button.IconHoverAnimation;
            set => button.IconHoverAnimation = value;
        }

        /// <summary>
        /// 按钮图标组件的位置
        /// </summary>
        [Description("按钮图标组件的位置"), Category("外观"), DefaultValue(TAlignMini.Left)]
        public TAlignMini IconPosition
        {
            get => button.IconPosition;
            set => button.IconPosition = value;
        }

        #region Obsolete

        [Obsolete("请使用 Icon 代替")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? Image { get => Icon; set => Icon = value; }

        [Obsolete("请使用 IconSvg 代替")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? ImageSvg { get => IconSvg; set => IconSvg = value; }

        [Obsolete("请使用 IconSize 代替")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size ImageSize { get => IconSize; set => IconSize = value; }

        [Obsolete("请使用 IconHover 代替")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? ImageHover { get => IconHover; set => IconHover = value; }

        [Obsolete("请使用 IconHoverSvg 代替")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? ImageHoverSvg { get => IconHoverSvg; set => IconHoverSvg = value; }

        [Obsolete("请使用 HasIcon 代替")]
        public bool HasImage => HasIcon;

        #endregion

        #endregion

        #region 加载动画

        /// <summary>
        /// 加载状态
        /// </summary>
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading
        {
            get => button.Loading;
            set => button.Loading = value;
        }

        #endregion

        /// <summary>
        /// 连接左边
        /// </summary>
        [Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft
        {
            get => button.JoinLeft;
            set => button.JoinLeft = value;
        }

        /// <summary>
        /// 连接右边
        /// </summary>
        [Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight
        {
            get => button.JoinRight;
            set => button.JoinRight = value;
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            button.Paint(g, ClientRectangle.PaddingRect(Padding), ReadRectangle);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + button.BorderWidth / 2F) * Config.Dpi, button.Shape, button.JoinLeft, button.JoinRight);
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = (button.Shape == TShape.Round || button.Shape == TShape.Circle) ? rect_read.Height : button.Radius * Config.Dpi;
                return button.Path(rect_read, _radius);
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RespondRealAreas)
            {
                var rect_read = ReadRectangle;
                using (var path = button.Path(rect_read, button.Radius * Config.Dpi))
                {
                    button.MouseHover = path.IsVisible(e.Location);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (RespondRealAreas) return;
            button.MouseHover = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            button.MouseHover = false;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            button.MouseHover = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (CanClick(e.Location))
            {
                Focus();
                base.OnMouseDown(e);
                button.MouseDown = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (button.MouseDown)
            {
                if (CanClick(e.Location))
                {
                    base.OnMouseUp(e);
                    if (e.Button == MouseButtons.Left)
                    {
                        button.OnClick();
                        OnClick(e);
                    }
                    OnMouseClick(e);
                }
                button.MouseDown = false;
            }
            else base.OnMouseUp(e);
        }

        #endregion

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(false)]
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                if (base.AutoSize == value) return;
                base.AutoSize = value;
                if (value)
                {
                    if (autoSize == TAutoSize.None) autoSize = TAutoSize.Auto;
                }
                else autoSize = TAutoSize.None;
                BeforeAutoSize();
            }
        }

        TAutoSize autoSize = TAutoSize.None;
        /// <summary>
        /// 自动大小模式
        /// </summary>
        [Description("自动大小模式"), Category("外观"), DefaultValue(TAutoSize.None)]
        public TAutoSize AutoSizeMode
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
                base.AutoSize = autoSize != TAutoSize.None;
                BeforeAutoSize();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            BeforeAutoSize();
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (autoSize == TAutoSize.None) return base.GetPreferredSize(proposedSize);
            else if (autoSize == TAutoSize.Width) return new Size(PSize.Width, base.GetPreferredSize(proposedSize).Height);
            else if (autoSize == TAutoSize.Height) return new Size(base.GetPreferredSize(proposedSize).Width, PSize.Height);
            return PSize;
        }

        internal Size PSize
        {
            get
            {
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureString(button.Text ?? Config.NullText, Font).Size();
                    int gap = (int)(20 * Config.Dpi), wave = (int)(WaveSize * Config.Dpi);
                    if (button.Shape == TShape.Circle || string.IsNullOrEmpty(button.Text))
                    {
                        int s = font_size.Height + wave + gap;
                        return new Size(s, s);
                    }
                    else
                    {
                        int m = wave * 2;
                        if (button.JoinLeft || button.JoinRight) m = 0;
                        bool has_icon = button.Loading || HasIcon;
                        if (has_icon || button.ShowArrow)
                        {
                            if (has_icon && (IconPosition == TAlignMini.Top || IconPosition == TAlignMini.Bottom))
                            {
                                int size = (int)Math.Ceiling(font_size.Height * 1.2F);
                                return new Size(font_size.Width + m + gap + size, font_size.Height + wave + gap + size);
                            }
                            int height = font_size.Height + wave + gap;
                            if (has_icon && button.ShowArrow) return new Size(font_size.Width + m + gap + font_size.Height * 2, height);
                            else if (has_icon) return new Size(font_size.Width + m + gap + (int)Math.Ceiling(font_size.Height * 1.2F), height);
                            else return new Size(font_size.Width + m + gap + (int)Math.Ceiling(font_size.Height * .8F), height);
                        }
                        else return new Size(font_size.Width + m + gap, font_size.Height + wave + gap);
                    }
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        internal bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired)
            {
                bool flag = false;
                Invoke(new Action(() =>
                {
                    flag = BeforeAutoSize();
                }));
                return flag;
            }
            var PS = PSize;
            switch (autoSize)
            {
                case TAutoSize.Width:
                    if (Width == PS.Width) return true;
                    Width = PS.Width;
                    break;
                case TAutoSize.Height:
                    if (Height == PS.Height) return true;
                    Height = PS.Height;
                    break;
                case TAutoSize.Auto:
                default:
                    if (Width == PS.Width && Height == PS.Height) return true;
                    Size = PS;
                    break;
            }
            return false;
        }

        #endregion

        #region 按钮点击

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        /// <summary>
        /// 是否默认按钮
        /// </summary>
        public void NotifyDefault(bool value)
        {

        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        bool CanClick()
        {
            if (button.Loading) return false;
            else
            {
                if (RespondRealAreas)
                {
                    var e = PointToClient(MousePosition);
                    var rect_read = ReadRectangle;
                    using (var path = button.Path(rect_read, button.Radius * Config.Dpi))
                    {
                        return path.IsVisible(e);
                    }
                }
                else return true;
            }
        }
        bool CanClick(Point e)
        {
            if (button.Loading) return false;
            else
            {
                if (RespondRealAreas)
                {
                    var rect_read = ReadRectangle;
                    using (var path = button.Path(rect_read, button.Radius * Config.Dpi))
                    {
                        return path.IsVisible(e);
                    }
                }
                else return true;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler? DoubleClick
        {
            add => base.DoubleClick += value;
            remove => base.DoubleClick -= value;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event MouseEventHandler? MouseDoubleClick
        {
            add => base.MouseDoubleClick += value;
            remove => base.MouseDoubleClick -= value;
        }

        #endregion

        protected override void OnEnabledChanged(EventArgs e)
        {
            button.Enabled = Enabled;
            base.OnEnabledChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            button.Dispose();
            base.Dispose(disposing);
        }
    }


    class IButton : IDisposable
    {
        Func<Font> Font;
        Func<bool> BAutoSize;
        Action Invalidate;
        IControl control;

        public IButton(IControl _control, Func<bool> autoSize)
        {
            control = _control;
            Invalidate = () => control.Invalidate();
            Font = () => control.Font;
            BAutoSize = autoSize;
        }

        #region 属性

        bool enabled = true;
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;
                enabled = value;
                Invalidate();
            }
        }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        string? backExtend = null;
        /// <summary>
        /// 背景渐变色
        /// </summary>
        public string? BackExtend
        {
            get => backExtend;
            set
            {
                if (backExtend == value) return;
                backExtend = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        public Color? BackActive { get; set; }

        Image? backImage = null;
        /// <summary>
        /// 背景图片
        /// </summary>
        public Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
            }
        }

        TFit backFit = TFit.Fill;
        /// <summary>
        /// 背景图片布局
        /// </summary>
        public TFit BackgroundImageLayout
        {
            get => backFit;
            set
            {
                if (backFit == value) return;
                backFit = value;
                Invalidate();
            }
        }

        #endregion

        #region 默认样式

        Color? defaultback;
        /// <summary>
        /// Default模式背景颜色
        /// </summary>
        public Color? DefaultBack
        {
            get => defaultback;
            set
            {
                if (defaultback == value) return;
                defaultback = value;
                if (type == TTypeMini.Default) Invalidate();
            }
        }

        Color? defaultbordercolor;
        /// <summary>
        /// Default模式边框颜色
        /// </summary>
        public Color? DefaultBorderColor
        {
            get => defaultbordercolor;
            set
            {
                if (defaultbordercolor == value) return;
                defaultbordercolor = value;
                if (type == TTypeMini.Default) Invalidate();
            }
        }

        #endregion

        #region 边框

        float borderWidth = 0;
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 波浪大小
        /// </summary>
        public int WaveSize { get; set; } = 4;

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                if (BAutoSize()) Invalidate();
            }
        }

        TShape shape = TShape.Default;
        /// <summary>
        /// 形状
        /// </summary>
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                if (BAutoSize()) Invalidate();
            }
        }

        TTypeMini type = TTypeMini.Default;
        /// <summary>
        /// 类型
        /// </summary>
        public TTypeMini Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                Invalidate();
            }
        }

        bool ghost = false;
        /// <summary>
        /// 幽灵属性，使按钮背景透明
        /// </summary>
        public bool Ghost
        {
            get => ghost;
            set
            {
                if (ghost == value) return;
                ghost = value;
                Invalidate();
            }
        }

        public float ArrowProg = -1F;

        bool showArrow = false;
        /// <summary>
        /// 显示箭头
        /// </summary>
        public bool ShowArrow
        {
            get => showArrow;
            set
            {
                if (showArrow == value) return;
                showArrow = value;
                if (BAutoSize()) Invalidate();
            }
        }

        bool isLink = false;
        /// <summary>
        /// 箭头链接样式
        /// </summary>
        public bool IsLink
        {
            get => isLink;
            set
            {
                if (isLink == value) return;
                isLink = value;
                Invalidate();
            }
        }

        #region 文本

        bool textLine = false;
        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get => text;
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (text == value) return;
                text = value;
                if (text == null) textLine = false;
                else textLine = text.Contains(Environment.NewLine);
                if (BAutoSize()) Invalidate();
            }
        }

        StringFormat stringFormat = Helper.SF_NoWrap();
        ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("外观"), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                if (loading || HasIcon || showArrow)
                {
                    value = ContentAlignment.MiddleCenter;
                    if (textAlign == value) return;
                }
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
                Invalidate();
            }
        }

        bool autoEllipsis = false;
        /// <summary>
        /// 文本超出自动处理
        /// </summary>
        public bool AutoEllipsis
        {
            get => autoEllipsis;
            set
            {
                if (autoEllipsis == value) return;
                autoEllipsis = value;
                stringFormat.Trimming = value ? StringTrimming.EllipsisCharacter : StringTrimming.None;
            }
        }

        bool textMultiLine = false;
        /// <summary>
        /// 是否多行
        /// </summary>
        public bool TextMultiLine
        {
            get => textMultiLine;
            set
            {
                if (textMultiLine == value) return;
                textMultiLine = value;
                stringFormat.FormatFlags = value ? 0 : StringFormatFlags.NoWrap;
                Invalidate();
            }
        }

        #endregion

        #region 图标

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                if (BAutoSize()) Invalidate();
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
                if (BAutoSize()) Invalidate();
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
                if (BAutoSize()) Invalidate();
            }
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon
        {
            get => iconSvg != null || icon != null;
        }

        /// <summary>
        /// 图标大小
        /// </summary>
        [Description("图标大小"), Category("外观"), DefaultValue(typeof(Size), "0, 0")]
        public Size IconSize { get; set; } = new Size(0, 0);

        /// <summary>
        /// 悬停图标
        /// </summary>
        [Description("悬停图标"), Category("外观"), DefaultValue(null)]
        public Image? IconHover { get; set; } = null;

        /// <summary>
        /// 悬停图标SVG
        /// </summary>
        [Description("悬停图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconHoverSvg { get; set; } = null;

        /// <summary>
        /// 悬停图标动画时长
        /// </summary>
        [Description("悬停图标动画时长"), Category("外观"), DefaultValue(200)]
        public int IconHoverAnimation { get; set; } = 200;

        TAlignMini iconPosition = TAlignMini.Left;
        /// <summary>
        /// 按钮图标组件的位置
        /// </summary>
        [Description("按钮图标组件的位置"), Category("外观"), DefaultValue(TAlignMini.Left)]
        public TAlignMini IconPosition
        {
            get => iconPosition;
            set
            {
                if (iconPosition == value) return;
                iconPosition = value;
                if (BAutoSize()) Invalidate();
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
                control.SetCursor(_mouseHover && enabled && !value);
                BAutoSize();
                ThreadLoading?.Dispose();
                if (loading)
                {
                    AnimationClickValue = 0;
                    ThreadLoading = new ITask(control, i =>
                    {
                        AnimationLoadingValue = i;
                        Invalidate();
                        return loading;
                    }, 10, 360, 6, () =>
                    {
                        Invalidate();
                    });
                }
                else Invalidate();
            }
        }

        #endregion

        bool joinLeft = false;
        /// <summary>
        /// 连接左边
        /// </summary>
        [Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft
        {
            get => joinLeft;
            set
            {
                if (joinLeft == value) return;
                joinLeft = value;
                if (BAutoSize()) Invalidate();
            }
        }

        bool joinRight = false;
        /// <summary>
        /// 连接右边
        /// </summary>
        [Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight
        {
            get => joinRight;
            set
            {
                if (joinRight == value) return;
                joinRight = value;
                if (BAutoSize()) Invalidate();
            }
        }

        public void Dispose()
        {
            ThreadClick?.Dispose();
            ThreadHover?.Dispose();
            ThreadIconHover?.Dispose();
            ThreadLoading?.Dispose();
        }

        ITask? ThreadHover = null;
        ITask? ThreadIconHover = null;
        ITask? ThreadClick = null;
        ITask? ThreadLoading = null;

        #region 点击动画

        bool AnimationClick = false;
        float AnimationClickValue = 0;

        public void OnClick()
        {
            if (WaveSize > 0 && Config.Animation)
            {
                ThreadClick?.Dispose();
                AnimationClickValue = 0;
                AnimationClick = true;
                ThreadClick = new ITask(control, () =>
                {
                    if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                    else AnimationClickValue += AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                    if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                    Invalidate();
                    return true;
                }, 50, () =>
                {
                    AnimationClick = false;
                    Invalidate();
                });
            }
        }

        #endregion

        #region 悬停动画

        bool _mouseDown = false;
        public bool MouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
                Invalidate();
            }
        }

        int AnimationHoverValue = 0;
        bool AnimationHover = false;
        bool AnimationIconHover = false;
        float AnimationIconHoverValue = 0F;
        bool _mouseHover = false;
        public bool MouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                control.SetCursor(value && enabled && !loading);
                if (enabled)
                {
                    Color _back_hover;
                    switch (type)
                    {
                        case TTypeMini.Default:
                            if (borderWidth > 0) _back_hover = Style.Db.PrimaryHover;
                            else _back_hover = Style.Db.FillSecondary;
                            break;
                        case TTypeMini.Success:
                            _back_hover = Style.Db.SuccessHover;
                            break;
                        case TTypeMini.Error:
                            _back_hover = Style.Db.ErrorHover;
                            break;
                        case TTypeMini.Info:
                            _back_hover = Style.Db.InfoHover;
                            break;
                        case TTypeMini.Warn:
                            _back_hover = Style.Db.WarningHover;
                            break;
                        case TTypeMini.Primary:
                        default:
                            _back_hover = Style.Db.PrimaryHover;
                            break;
                    }

                    if (BackHover.HasValue) _back_hover = BackHover.Value;
                    if (Config.Animation)
                    {
                        if (IconHoverAnimation > 0 && HasIcon && (IconHoverSvg != null || IconHover != null))
                        {
                            ThreadIconHover?.Dispose();
                            AnimationIconHover = true;
                            var t = Animation.TotalFrames(10, IconHoverAnimation);
                            if (value)
                            {
                                ThreadIconHover = new ITask((i) =>
                                {
                                    AnimationIconHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationIconHoverValue = 1F;
                                    AnimationIconHover = false;
                                    Invalidate();
                                });
                            }
                            else
                            {
                                ThreadIconHover = new ITask((i) =>
                                {
                                    AnimationIconHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationIconHoverValue = 0F;
                                    AnimationIconHover = false;
                                    Invalidate();
                                });
                            }
                        }
                        if (_back_hover.A > 0)
                        {
                            int addvalue = _back_hover.A / 12;
                            ThreadHover?.Dispose();
                            AnimationHover = true;
                            if (value)
                            {
                                ThreadHover = new ITask(control, () =>
                                {
                                    AnimationHoverValue += addvalue;
                                    if (AnimationHoverValue > _back_hover.A) { AnimationHoverValue = _back_hover.A; return false; }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationHover = false;
                                    Invalidate();
                                });
                            }
                            else
                            {
                                ThreadHover = new ITask(control, () =>
                                {
                                    AnimationHoverValue -= addvalue;
                                    if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationHover = false;
                                    Invalidate();
                                });
                            }
                        }
                        else
                        {
                            AnimationHoverValue = _back_hover.A;
                            Invalidate();
                        }
                    }
                    else AnimationHoverValue = _back_hover.A;
                    Invalidate();
                }
            }
        }

        #endregion

        #endregion

        #region 渲染

        public void Paint(Graphics g, Rectangle rect, Rectangle rect_read)
        {
            float _radius = (shape == TShape.Round || shape == TShape.Circle) ? rect_read.Height : radius * Config.Dpi;

            if (backImage != null) g.PaintImg(rect_read, backImage, backFit, _radius, shape);

            if (type == TTypeMini.Default)
            {
                Color _fore = Style.Db.DefaultColor, _color = Style.Db.Primary, _back_hover, _back_active;
                if (borderWidth > 0)
                {
                    _back_hover = Style.Db.PrimaryHover;
                    _back_active = Style.Db.PrimaryActive;
                }
                else
                {
                    _back_hover = Style.Db.FillSecondary;
                    _back_active = Style.Db.Fill;
                }
                if (fore.HasValue) _fore = fore.Value;
                if (BackHover.HasValue) _back_hover = BackHover.Value;
                if (BackActive.HasValue) _back_active = BackActive.Value;
                if (loading)
                {
                    _fore = Color.FromArgb(165, _fore);
                    _color = Color.FromArgb(165, _color);
                }

                using (var path = Path(rect_read, _radius))
                {
                    #region 动画

                    if (AnimationClick)
                    {
                        float maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue);
                        if (shape == TShape.Circle)
                        {
                            if (maxw > maxh) maxw = maxh;
                            else maxh = maxw;
                        }
                        float alpha = 100 * (1F - AnimationClickValue);
                        using (var brush = new SolidBrush(Helper.ToColor(alpha, _color)))
                        {
                            using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, shape))
                            {
                                path_click.AddPath(path, false);
                                g.FillPath(brush, path_click);
                            }
                        }
                    }

                    #endregion

                    if (enabled)
                    {
                        if (!ghost)
                        {
                            #region 绘制阴影

                            if (WaveSize > 0)
                            {
                                using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                                {
                                    path_shadow.AddPath(path, false);
                                    using (var brush = new SolidBrush(Style.Db.FillQuaternary))
                                    {
                                        g.FillPath(brush, path_shadow);
                                    }
                                }
                            }

                            #endregion

                            using (var brush = new SolidBrush(defaultback ?? Style.Db.DefaultBg))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        if (borderWidth > 0)
                        {
                            float border = borderWidth * Config.Dpi;
                            if (MouseDown)
                            {
                                using (var brush = new Pen(_back_active, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_active, rect_read, enabled);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Helper.ToColor(AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _fore, colorHover, rect_read);
                            }
                            else if (MouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_hover, rect_read, enabled);
                            }
                            else
                            {
                                using (var brush = new Pen(defaultbordercolor ?? Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _fore, rect_read, enabled);
                            }
                        }
                        else
                        {
                            if (MouseDown)
                            {
                                using (var brush = new SolidBrush(_back_active))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else if (AnimationHover)
                            {
                                using (var brush = new SolidBrush(Helper.ToColor(AnimationHoverValue, _back_hover)))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else if (MouseHover)
                            {
                                using (var brush = new SolidBrush(_back_hover))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            PaintTextLoading(g, text, _fore, rect_read, enabled);
                        }
                    }
                    else
                    {
                        if (borderWidth > 0)
                        {
                            using (var brush = new SolidBrush(Style.Db.FillTertiary))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        PaintTextLoading(g, text, Style.Db.TextQuaternary, rect_read, enabled);
                    }
                }
            }
            else
            {
                Color _fore, _back, _back_hover, _back_active;
                switch (type)
                {
                    case TTypeMini.Error:
                        _back = Style.Db.Error;
                        _fore = Style.Db.ErrorColor;
                        _back_hover = Style.Db.ErrorHover;
                        _back_active = Style.Db.ErrorActive;
                        break;
                    case TTypeMini.Success:
                        _back = Style.Db.Success;
                        _fore = Style.Db.SuccessColor;
                        _back_hover = Style.Db.SuccessHover;
                        _back_active = Style.Db.SuccessActive;
                        break;
                    case TTypeMini.Info:
                        _back = Style.Db.Info;
                        _fore = Style.Db.InfoColor;
                        _back_hover = Style.Db.InfoHover;
                        _back_active = Style.Db.InfoActive;
                        break;
                    case TTypeMini.Warn:
                        _back = Style.Db.Warning;
                        _fore = Style.Db.WarningColor;
                        _back_hover = Style.Db.WarningHover;
                        _back_active = Style.Db.WarningActive;
                        break;
                    case TTypeMini.Primary:
                    default:
                        _back = Style.Db.Primary;
                        _fore = Style.Db.PrimaryColor;
                        _back_hover = Style.Db.PrimaryHover;
                        _back_active = Style.Db.PrimaryActive;
                        break;
                }

                if (fore.HasValue) _fore = fore.Value;
                if (back.HasValue) _back = back.Value;
                if (BackHover.HasValue) _back_hover = BackHover.Value;
                if (BackActive.HasValue) _back_active = BackActive.Value;
                if (loading) _back = Color.FromArgb(165, _back);

                using (var path = Path(rect_read, _radius))
                {
                    #region 动画

                    if (AnimationClick)
                    {
                        float maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue);
                        if (shape == TShape.Circle)
                        {
                            if (maxw > maxh) maxw = maxh;
                            else maxh = maxw;
                        }
                        float alpha = 100 * (1F - AnimationClickValue);
                        using (var brush = new SolidBrush(Helper.ToColor(alpha, _back)))
                        {
                            using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, shape))
                            {
                                path_click.AddPath(path, false);
                                g.FillPath(brush, path_click);
                            }
                        }
                    }

                    #endregion

                    if (ghost)
                    {
                        #region 绘制背景

                        if (borderWidth > 0)
                        {
                            float border = borderWidth * Config.Dpi;
                            if (MouseDown)
                            {
                                using (var brush = new Pen(_back_active, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_active, rect_read, enabled);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Helper.ToColor(AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(enabled ? _back : Style.Db.FillTertiary, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back, colorHover, rect_read);
                            }
                            else if (MouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_hover, rect_read, enabled);
                            }
                            else
                            {
                                if (enabled)
                                {
                                    using (var brushback = backExtend.BrushEx(rect_read, _back))
                                    {
                                        using (var brush = new Pen(brushback, border))
                                        {
                                            g.DrawPath(brush, path);
                                        }
                                    }
                                }
                                else
                                {
                                    using (var brush = new Pen(Style.Db.FillTertiary, border))
                                    {
                                        g.DrawPath(brush, path);
                                    }
                                }
                                PaintTextLoading(g, text, enabled ? _back : Style.Db.TextQuaternary, rect_read, enabled);
                            }
                        }
                        else PaintTextLoading(g, text, enabled ? _back : Style.Db.TextQuaternary, rect_read, enabled);

                        #endregion
                    }
                    else
                    {
                        #region 绘制阴影

                        if (enabled && WaveSize > 0)
                        {
                            using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                            {
                                path_shadow.AddPath(path, false);
                                using (var brush = new SolidBrush(_back.rgba(Config.Mode == TMode.Dark ? 0.15F : 0.1F)))
                                {
                                    g.FillPath(brush, path_shadow);
                                }
                            }
                        }

                        #endregion

                        #region 绘制背景

                        if (enabled)
                        {
                            using (var brush = backExtend.BrushEx(rect_read, _back))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else
                        {
                            using (var brush = new SolidBrush(Style.Db.FillTertiary))
                            {
                                g.FillPath(brush, path);
                            }
                        }

                        if (MouseDown)
                        {
                            using (var brush = new SolidBrush(_back_active))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else if (AnimationHover)
                        {
                            var colorHover = Helper.ToColor(AnimationHoverValue, _back_hover);
                            using (var brush = new SolidBrush(colorHover))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else if (MouseHover)
                        {
                            using (var brush = new SolidBrush(_back_hover))
                            {
                                g.FillPath(brush, path);
                            }
                        }

                        #endregion

                        PaintTextLoading(g, text, enabled ? _fore : Style.Db.TextQuaternary, rect_read, enabled);
                    }
                }
            }
        }

        #region 渲染帮助

        void PaintTextLoading(Graphics g, string? text, Color color, Rectangle rect_read, bool enabled)
        {
            var font_size = g.MeasureString(text ?? Config.NullText, Font()).Size();
            if (text == null)
            {
                //没有文字
                var rect = GetIconRectCenter(font_size, rect_read);
                if (loading)
                {
                    float loading_size = rect_read.Height * 0.06F;
                    using (var brush = new Pen(color, loading_size))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect, AnimationLoadingValue, 100);
                    }
                }
                else
                {
                    if (PaintIcon(g, color, rect, false, enabled) && showArrow)
                    {
                        int size = (int)(font_size.Height * IconRatio);
                        var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_arrow.Width / 2;
                                g.TranslateTransform(rect_arrow.X + size_arrow, rect_arrow.Y + size_arrow);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_arrow.Width, rect_arrow.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else g.DrawLines(pen, rect_arrow.TriangleLines(ArrowProg));
                        }
                    }
                }
            }
            else
            {
                bool has_left = loading || HasIcon, has_right = showArrow;
                Rectangle rect_text;
                if (has_left || has_right)
                {
                    if (has_left && has_right)
                    {
                        rect_text = RectAlignLR(g, textLine, Font(), iconPosition, iconratio, font_size, rect_read, out var rect_l, out var rect_r);

                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else PaintIcon(g, color, rect_l, true, enabled);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else g.DrawLines(pen, rect_r.TriangleLines(ArrowProg));
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        rect_text = RectAlignL(g, textLine, Font(), iconPosition, iconratio, font_size, rect_read, out var rect_l);
                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else PaintIcon(g, color, rect_l, true, enabled);
                    }
                    else
                    {
                        rect_text = RectAlignR(g, textLine, Font(), iconPosition, iconratio, font_size, rect_read, out var rect_r);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else g.DrawLines(pen, rect_r.TriangleLines(ArrowProg));
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                    PaintTextAlign(rect_read, ref rect_text);
                }
                using (var brush = new SolidBrush(color))
                {
                    g.DrawString(text, Font(), brush, rect_text, stringFormat);
                }
            }
        }
        void PaintTextLoading(Graphics g, string? text, Color color, Color colorHover, Rectangle rect_read)
        {
            var font_size = g.MeasureString(text ?? Config.NullText, Font()).Size();
            if (text == null)
            {
                var rect = GetIconRectCenter(font_size, rect_read);
                if (loading)
                {
                    float loading_size = rect_read.Height * 0.06F;
                    using (var brush = new Pen(color, loading_size))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect, AnimationLoadingValue, 100);
                    }
                }
                else
                {
                    if (PaintIcon(g, color, rect, false, true))
                    {
                        if (showArrow)
                        {
                            int size = (int)(font_size.Height * IconRatio);
                            var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                            using (var pen = new Pen(color, 2F * Config.Dpi))
                            using (var penHover = new Pen(colorHover, pen.Width))
                            {
                                pen.StartCap = pen.EndCap = LineCap.Round;
                                if (isLink)
                                {
                                    int size_arrow = rect_arrow.Width / 2;
                                    g.TranslateTransform(rect_arrow.X + size_arrow, rect_arrow.Y + size_arrow);
                                    g.RotateTransform(-90F);
                                    var rect_arrow_lines = new Rectangle(-size_arrow, -size_arrow, rect_arrow.Width, rect_arrow.Height).TriangleLines(ArrowProg);
                                    g.DrawLines(pen, rect_arrow_lines);
                                    g.DrawLines(penHover, rect_arrow_lines);
                                    g.ResetTransform();
                                }
                                else
                                {
                                    var rect_arrow_lines = rect_arrow.TriangleLines(ArrowProg);
                                    g.DrawLines(pen, rect_arrow_lines);
                                    g.DrawLines(penHover, rect_arrow_lines);
                                }
                            }
                        }
                    }
                    else PaintIcon(g, colorHover, rect, false, true);
                }
            }
            else
            {
                bool has_left = loading || HasIcon, has_right = showArrow;
                Rectangle rect_text;
                if (has_left || has_right)
                {
                    if (has_left && has_right)
                    {
                        rect_text = RectAlignLR(g, textLine, Font(), iconPosition, iconratio, font_size, rect_read, out var rect_l, out var rect_r);

                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else
                        {
                            PaintIcon(g, color, rect_l, true, true);
                            PaintIcon(g, colorHover, rect_l, true, true);
                        }

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                var rect_arrow = new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        rect_text = RectAlignL(g, textLine, Font(), iconPosition, iconratio, font_size, rect_read, out var rect_l);
                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else
                        {
                            PaintIcon(g, color, rect_l, true, true);
                            PaintIcon(g, colorHover, rect_l, true, true);
                        }
                    }
                    else
                    {
                        rect_text = RectAlignR(g, textLine, Font(), iconPosition, iconratio, font_size, rect_read, out var rect_r);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                var rect_arrow = new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                    PaintTextAlign(rect_read, ref rect_text);
                }
                using (var brush = new SolidBrush(color))
                using (var brushHover = new SolidBrush(colorHover))
                {
                    g.DrawString(text, Font(), brush, rect_text, stringFormat);
                    g.DrawString(text, Font(), brushHover, rect_text, stringFormat);
                }
            }
        }

        internal static Rectangle RectAlignL(Graphics g, bool textLine, Font font, TAlignMini iconPosition, float iconratio, Size font_size, Rectangle rect_read, out Rectangle rect_l)
        {
            int font_Height = font_size.Height;
            if (textLine && (iconPosition == TAlignMini.Top || iconPosition == TAlignMini.Bottom)) font_Height = g.MeasureString(Config.NullText, font).Size().Height;
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * .25F);
            Rectangle rect_text;
            switch (iconPosition)
            {
                case TAlignMini.Top:
                    int t_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                    rect_text = new Rectangle(rect_read.X, t_x + icon_size + sp, rect_read.Width, font_size.Height);
                    rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, t_x, icon_size, icon_size);
                    break;
                case TAlignMini.Bottom:
                    int b_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                    rect_text = new Rectangle(rect_read.X, b_x, rect_read.Width, font_size.Height);
                    rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, b_x + font_size.Height + sp, icon_size, icon_size);
                    break;
                case TAlignMini.Right:
                    int r_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp)) / 2);
                    rect_text = new Rectangle(r_x, rect_read.Y, font_size.Width, rect_read.Height);
                    rect_l = new Rectangle(r_x + font_size.Width + sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                    break;
                case TAlignMini.Left:
                default:
                    int l_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp)) / 2);
                    rect_text = new Rectangle(l_x + icon_size + sp, rect_read.Y, font_size.Width, rect_read.Height);
                    rect_l = new Rectangle(l_x, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                    break;
            }
            return rect_text;
        }
        internal static Rectangle RectAlignLR(Graphics g, bool textLine, Font font, TAlignMini iconPosition, float iconratio, Size font_size, Rectangle rect_read, out Rectangle rect_l, out Rectangle rect_r)
        {
            int font_Height = font_size.Height;
            if (textLine && (iconPosition == TAlignMini.Top || iconPosition == TAlignMini.Bottom)) font_Height = g.MeasureString(Config.NullText, font).Size().Height;
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * .25F), sps = (int)(font_size.Height * .4F);
            Rectangle rect_text;
            switch (iconPosition)
            {
                case TAlignMini.Top:
                    int t_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                    rect_text = new Rectangle(rect_read.X, t_x + icon_size + sp, rect_read.Width, font_size.Height);
                    rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, t_x, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, rect_text.Y + (rect_text.Height - icon_size) / 2, icon_size, icon_size);
                    break;
                case TAlignMini.Bottom:
                    int b_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                    rect_text = new Rectangle(rect_read.X, b_x, rect_read.Width, font_size.Height);
                    rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, b_x + font_size.Height + sp, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, rect_text.Y + (rect_text.Height - icon_size) / 2, icon_size, icon_size);
                    break;
                case TAlignMini.Right:
                    int r_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp + sps)) / 2), r_y = rect_read.Y + (rect_read.Height - icon_size) / 2;
                    rect_text = new Rectangle(r_x, rect_read.Y, font_size.Width, rect_read.Height);
                    rect_l = new Rectangle(r_x + font_size.Width + sp, r_y, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.X + sps, r_y, icon_size, icon_size);
                    break;
                case TAlignMini.Left:
                default:
                    int l_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp + sps)) / 2), l_y = rect_read.Y + (rect_read.Height - icon_size) / 2;
                    rect_text = new Rectangle(l_x + icon_size + sp, rect_read.Y, font_size.Width, rect_read.Height);
                    rect_l = new Rectangle(l_x, l_y, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, l_y, icon_size, icon_size);
                    break;
            }
            return rect_text;
        }
        internal static Rectangle RectAlignR(Graphics g, bool textLine, Font font, TAlignMini iconPosition, float iconratio, Size font_size, Rectangle rect_read, out Rectangle rect_r)
        {
            int font_Height = font_size.Height;
            if (textLine && (iconPosition == TAlignMini.Top || iconPosition == TAlignMini.Bottom)) font_Height = g.MeasureString(Config.NullText, font).Size().Height;
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * .25F), sps = (int)(font_size.Height * .4F), rsps = icon_size + sp;
            Rectangle rect_text;
            switch (iconPosition)
            {
                case TAlignMini.Bottom:
                case TAlignMini.Right:
                    rect_text = new Rectangle(rect_read.X + rsps, rect_read.Y, rect_read.Width - rsps, rect_read.Height);
                    rect_r = new Rectangle(rect_read.X + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                    break;
                case TAlignMini.Top:
                case TAlignMini.Left:
                default:
                    rect_text = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width - rsps, rect_read.Height);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                    break;
            }
            return rect_text;
        }

        void PaintTextAlign(Rectangle rect_read, ref Rectangle rect_text)
        {
            switch (textAlign)
            {
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    rect_text.Y = rect_read.Y;
                    rect_text.Height = rect_read.Height;
                    break;
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                case ContentAlignment.TopCenter:
                    rect_text.Height = rect_read.Height - rect_text.Y;
                    break;
            }
        }

        #region 渲染图标

        /// <summary>
        /// 居中的图标绘制区域
        /// </summary>
        /// <param name="font_size">字体大小</param>
        /// <param name="rect_read">客户区域</param>
        Rectangle GetIconRectCenter(Size font_size, Rectangle rect_read)
        {
            if (IconSize.Width > 0 && IconSize.Height > 0)
            {
                int w = (int)(IconSize.Width * Config.Dpi), h = (int)(IconSize.Height * Config.Dpi);
                return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - h) / 2, w, h);
            }
            else
            {
                int w = (int)(font_size.Height * IconRatio * 1.125F);
                return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - w) / 2, w, w);
            }
        }


        /// <summary>
        /// 渲染图标
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rect_o">区域</param>
        /// <param name="hastxt">包含文本</param>
        /// <param name="enabled">使能</param>
        bool PaintIcon(Graphics g, Color? color, Rectangle rect_o, bool hastxt, bool enabled)
        {
            var rect = hastxt ? GetIconRect(rect_o) : rect_o;
            if (AnimationIconHover)
            {
                PaintCoreIcon(g, rect, color, 1F - AnimationIconHoverValue);
                PaintCoreIconHover(g, rect, color, AnimationIconHoverValue);
                return false;
            }
            else
            {
                if (enabled)
                {
                    if (MouseHover)
                    {
                        if (PaintCoreIconHover(g, rect, color)) return false;
                    }
                    if (PaintCoreIcon(g, rect, color)) return false;
                }
                else
                {
                    if (MouseHover)
                    {
                        if (PaintCoreIconHover(g, rect, color, .3F)) return false;
                    }
                    if (PaintCoreIcon(g, rect, color, .3F)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 图标绘制区域
        /// </summary>
        /// <param name="rectl">图标区域</param>
        Rectangle GetIconRect(Rectangle rectl)
        {
            if (IconSize.Width > 0 && IconSize.Height > 0)
            {
                int w = (int)(IconSize.Width * Config.Dpi), h = (int)(IconSize.Height * Config.Dpi);
                return new Rectangle(rectl.X + (rectl.Width - w) / 2, rectl.Y + (rectl.Height - h) / 2, w, h);
            }
            else return rectl;
        }

        bool PaintCoreIcon(Graphics g, Rectangle rect, Color? color, float opacity = 1F) => PaintCoreIcon(g, icon, iconSvg, rect, color, opacity);
        bool PaintCoreIconHover(Graphics g, Rectangle rect, Color? color, float opacity = 1F) => PaintCoreIcon(g, IconHover, IconHoverSvg, rect, color, opacity);

        bool PaintCoreIcon(Graphics g, Image? icon, string? iconSvg, Rectangle rect, Color? color, float opacity = 1F)
        {
            if (iconSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(iconSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.DrawImage(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (icon != null)
            {
                g.DrawImage(icon, rect, opacity);
                return true;
            }
            return false;
        }

        #endregion

        public GraphicsPath Path(RectangleF rect_read, float _radius)
        {
            if (shape == TShape.Circle)
            {
                var path = new GraphicsPath();
                path.AddEllipse(rect_read);
                return path;
            }
            if (joinLeft && joinRight) return rect_read.RoundPath(0);
            else if (joinRight) return rect_read.RoundPath(_radius, true, false, false, true);
            else if (joinLeft) return rect_read.RoundPath(_radius, false, true, true, false);
            return rect_read.RoundPath(_radius);
        }

        #endregion

        #endregion
    }
}