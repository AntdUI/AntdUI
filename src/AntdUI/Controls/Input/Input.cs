// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Input 输入框
    /// </summary>
    /// <remarks>通过鼠标或键盘输入内容，是最基础的表单域的包装。</remarks>
    [Description("Input 输入框")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [DefaultEvent("TextChanged")]
    public partial class Input : IControl
    {
        public Input() : base(ControlType.Select)
        {
            base.BackColor = Color.Transparent;
            CaretInfo = new ICaret(this);
            ScrollY = new ScrollYInfo(this);
            ScrollX = new ScrollInfo(this);
        }

        #region 属性

        /// <summary>
        /// 原装背景颜色
        /// </summary>
        [Description("原装背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(typeof(Color), "Transparent")]
        public Color OriginalBackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        internal Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(BackColor));
            }
        }

        string? backExtend = null;
        /// <summary>
        /// 背景渐变色
        /// </summary>
        [Description("背景渐变色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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

        Image? backImage;
        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public new Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImage));
            }
        }

        TFit backFit = TFit.Fill;
        /// <summary>
        /// 背景图片布局
        /// </summary>
        [Description("背景图片布局"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TFit.Fill)]
        public new TFit BackgroundImageLayout
        {
            get => backFit;
            set
            {
                if (backFit == value) return;
                backFit = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImageLayout));
            }
        }

        #endregion

        Color selection = Color.FromArgb(102, 0, 127, 255);
        /// <summary>
        /// 选中颜色
        /// </summary>
        [Description("选中颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(typeof(Color), "102, 0, 127, 255")]
        public Color SelectionColor
        {
            get => selection;
            set
            {
                if (selection == value) return;
                selection = value;
                Invalidate();
                OnPropertyChanged(nameof(SelectionColor));
            }
        }

        #region 光标

        /// <summary>
        /// 光标颜色
        /// </summary>
        [Description("光标颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CaretColor { get; set; }

        /// <summary>
        /// 光标速度
        /// </summary>
        [Description("光标速度"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(1000)]
        public int CaretSpeed { get; set; } = 1000;

        #endregion

        #region 边框

        internal float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(BorderWidth));
            }
        }

        internal Color? borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        /// <summary>
        /// 悬停边框颜色
        /// </summary>
        [Description("悬停边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderHover { get; set; }

        /// <summary>
        /// 激活边框颜色
        /// </summary>
        [Description("激活边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderActive { get; set; }

        #endregion

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(4)]
        public int WaveSize { get; set; } = 4;

        internal int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        internal bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(Round));
            }
        }

        TType status = TType.None;
        /// <summary>
        /// 设置校验状态
        /// </summary>
        [Description("设置校验状态"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TType.None)]
        public TType Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;
                Invalidate();
                OnPropertyChanged(nameof(Status));
            }
        }

        #region 图标

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        float? iconratioRight;
        /// <summary>
        /// 右图标比例
        /// </summary>
        [Description("右图标比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public float? IconRatioRight
        {
            get => iconratioRight;
            set
            {
                if (iconratioRight == value) return;
                iconratioRight = value;
                if (HasSuffix)
                {
                    CalculateRect();
                    Invalidate();
                }
                OnPropertyChanged(nameof(IconRatioRight));
            }
        }

        float icongap = .25F;
        /// <summary>
        /// 图标与文字间距比例
        /// </summary>
        [Description("图标与文字间距比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.25F)]
        public float IconGap
        {
            get => icongap;
            set
            {
                if (icongap == value) return;
                icongap = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(IconGap));
            }
        }

        float paddgap = .4F;
        /// <summary>
        /// 边框间距比例
        /// </summary>
        [Description("边框间距比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.4F)]
        public float PaddGap
        {
            get => paddgap;
            set
            {
                if (paddgap == value) return;
                paddgap = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(PaddGap));
            }
        }

        #region 前缀

        Image? prefix;
        /// <summary>
        /// 前缀
        /// </summary>
        [Description("前缀"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Image? Prefix
        {
            get => prefix;
            set
            {
                if (prefix == value) return;
                prefix = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(Prefix));
            }
        }

        string? prefixSvg;
        /// <summary>
        /// 前缀SVG
        /// </summary>
        [Description("前缀SVG"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? PrefixSvg
        {
            get => prefixSvg;
            set
            {
                if (prefixSvg == value) return;
                prefixSvg = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(PrefixSvg));
            }
        }

        string? prefixText;
        /// <summary>
        /// 前缀文本
        /// </summary>
        [Description("前缀文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Localizable(true)]
        public string? PrefixText
        {
            get => this.GetLangI(LocalizationPrefixText, prefixText);
            set
            {
                if (prefixText == value) return;
                prefixText = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(PrefixText));
            }
        }

        [Description("前缀文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationPrefixText { get; set; }

        Color? prefixFore;
        /// <summary>
        /// 前缀前景色
        /// </summary>
        [Description("前缀前景色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? PrefixFore
        {
            get => prefixFore;
            set
            {
                if (prefixFore == value) return;
                prefixFore = value;
                if (HasPrefix) Invalidate();
                OnPropertyChanged(nameof(PrefixFore));
            }
        }

        /// <summary>
        /// 前缀宽度
        /// </summary>
        [Description("前缀宽度"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public int? PrefixWidth { get; set; }

        /// <summary>
        /// 前缀对齐方式
        /// </summary>
        [Description("前缀对齐方式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(FormatFlags.Center | FormatFlags.NoWrap)]
        public FormatFlags PrefixFormat { get; set; } = FormatFlags.Center | FormatFlags.NoWrap;

        /// <summary>
        /// 是否包含前缀
        /// </summary>
        public virtual bool HasPrefix => prefixSvg != null || prefix != null;

        #endregion

        #region 后缀

        Image? suffix;
        /// <summary>
        /// 后缀
        /// </summary>
        [Description("后缀"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Image? Suffix
        {
            get => suffix;
            set
            {
                if (suffix == value) return;
                suffix = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(Suffix));
            }
        }

        string? suffixSvg;
        /// <summary>
        /// 后缀SVG
        /// </summary>
        [Description("后缀SVG"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? SuffixSvg
        {
            get => suffixSvg;
            set
            {
                if (suffixSvg == value) return;
                suffixSvg = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(SuffixSvg));
            }
        }

        string? suffixText;
        /// <summary>
        /// 后缀文本
        /// </summary>
        [Description("后缀文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Localizable(true)]
        public string? SuffixText
        {
            get => this.GetLangI(LocalizationSuffixText, suffixText);
            set
            {
                if (suffixText == value) return;
                suffixText = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(SuffixText));
            }
        }

        [Description("后缀文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSuffixText { get; set; }

        Color? suffixFore;
        /// <summary>
        /// 后缀前景色
        /// </summary>
        [Description("后缀前景色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? SuffixFore
        {
            get => suffixFore;
            set
            {
                if (suffixFore == value) return;
                suffixFore = value;
                if (HasSuffix) Invalidate();
                OnPropertyChanged(nameof(SuffixFore));
            }
        }

        /// <summary>
        /// 后缀宽度
        /// </summary>
        [Description("后缀宽度"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public int? SuffixWidth { get; set; }

        /// <summary>
        /// 后缀对齐方式
        /// </summary>
        [Description("后缀对齐方式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(FormatFlags.Center | FormatFlags.NoWrap)]
        public FormatFlags SuffixFormat { get; set; } = FormatFlags.Center | FormatFlags.NoWrap;

        /// <summary>
        /// 是否包含后缀
        /// </summary>
        public virtual bool HasSuffix => suffixSvg != null || suffix != null;

        #endregion

        #endregion

        #region 组合

        TJoinMode joinMode = TJoinMode.None;
        /// <summary>
        /// 组合模式
        /// </summary>
        [Description("组合模式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TJoinMode.None)]
        public TJoinMode JoinMode
        {
            get => joinMode;
            set
            {
                if (joinMode == value) return;
                joinMode = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(JoinMode));
            }
        }

        /// <summary>
        /// 连接左边
        /// </summary>
        [Obsolete("use JoinMode"), Browsable(false), Description("连接左边"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool JoinLeft { get; set; }

        /// <summary>
        /// 连接右边
        /// </summary>
        [Obsolete("use JoinMode"), Browsable(false), Description("连接右边"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool JoinRight { get; set; }

        #endregion

        bool allowclear = false, is_clear = false, is_clear_down = false;
        bool is_prefix_down = false, is_suffix_down = false;
        /// <summary>
        /// 支持清除
        /// </summary>
        [Description("支持清除"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public virtual bool AllowClear
        {
            get => allowclear;
            set
            {
                if (allowclear == value) return;
                allowclear = value;
                if (OnAllowClear())
                {
                    CalculateRect();
                    Invalidate();
                }
                OnPropertyChanged(nameof(AllowClear));
            }
        }

        bool OnAllowClear()
        {
            bool _is_clear = !ReadOnly && allowclear && _mouseHover && (!isempty || HasValue);
            if (is_clear == _is_clear) return false;
            is_clear = _is_clear;
            return true;
        }

        bool autoscroll = false;
        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        [Description("是否显示滚动条"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public virtual bool AutoScroll
        {
            get => autoscroll;
            set
            {
                if (autoscroll == value) return;
                autoscroll = value;
                Invalidate();
                OnPropertyChanged(nameof(AutoScroll));
            }
        }

        /// <summary>
        /// 处理快捷键
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete, Description("处理快捷键"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool HandShortcutKeys { get; set; } = true;

        /// <summary>
        /// 适配系统助记词
        /// </summary>
        [Description("适配系统助记词"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool AdapterSystemMnemonic { get; set; }

        #endregion

        #region 原生属性

        #region 文本

        internal bool isempty = true;

        /// <summary>
        /// 文本是否为空
        /// </summary>
        public bool IsTextEmpty => isempty;

        string _text = "";
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue("")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        public override string Text
        {
            get => this.GetLangIN(LocalizationText, _text);
#pragma warning disable CS8765
            set
            {
                if (SetText(value, false)) Invalidate();
            }
#pragma warning restore CS8765
        }

        /// <summary>
        /// 设置文本后选中到最后
        /// </summary>
        [Description("设置文本后选中到最后"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool SetTextSelectionEnd { get; set; }

        /// <summary>
        /// 文本总行
        /// </summary>
        [Description("文本总行"), Category(nameof(CategoryAttribute.Data)), DefaultValue(0)]
        public int TextTotalLine { get; private set; } = 0;

        /// <summary>
        /// 多行文本集合
        /// </summary>
        [Description("多行文本集合"), Category(nameof(CategoryAttribute.Appearance)), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Editor("System.Windows.Forms.Design.StringArrayEditor", typeof(UITypeEditor))]
        public string[] Lines
        {
            get => _text.Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.None);
            set
            {
                string newText;
                if (value == null || value.Length == 0) newText = string.Empty;
                else newText = string.Join(Environment.NewLine, value);
                if (_text != newText) Text = newText;
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        #endregion

        /// <summary>
        /// Emoji字体
        /// </summary>
        [Description("Emoji字体"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? EmojiFont { get; set; }

        #region 原生文本框

        ImeMode imeMode = ImeMode.NoControl;
        /// <summary>
        /// IME(输入法编辑器)状态
        /// </summary>
        [Description("IME(输入法编辑器)状态"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(ImeMode.NoControl)]
        public new ImeMode ImeMode
        {
            get => imeMode;
            set
            {
                if (imeMode == value) return;
                imeMode = value;
                SetImeMode(value);
                OnPropertyChanged(nameof(ImeMode));
            }
        }

        void SetImeMode(ImeMode value)
        {
            if (InvokeRequired) Invoke(() => SetImeMode(value));
            else base.ImeMode = value;
        }

        int selectionStart = 0, selectionStartTemp = 0, selectionLength = 0;
        /// <summary>
        /// 所选文本的起点
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int SelectionStart
        {
            get => selectionStart;
            set
            {
                if (SetSelectionStart(value)) Invalidate();
            }
        }

        bool SetSelectionStart(int value, bool caret = true)
        {
            bool r = false;
            if (value < 0) value = 0;
            else if (value > 0)
            {
                if (cache_font == null) value = 0;
                else if (value > cache_font.Count) value = cache_font.Count;
            }
            if (selectionStart == value)
            {
                if (caret) r = SetCaretPostion(value + 1, false);
                return false;
            }
            selectionStart = selectionStartTemp = value;
            if (caret)
            {
                if (cache_font == null || cache_font.Count <= value) r = SetCaretPostion(value + 1, false);
                else
                {
                    var it = cache_font[value];
                    r = SetCaretPostion(value + (it.ret ? 0 : 1), false);
                }
            }
            OnPropertyChanged(nameof(SelectionStart));
            return r;
        }

        /// <summary>
        /// 所选文本的长度
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int SelectionLength
        {
            get => selectionLength;
            set
            {
                if (SetSelectionLength(value)) Invalidate();
            }
        }

        bool SetSelectionLength(int value)
        {
            if (selectionLength == value) return false;
            selectionLength = value;
            OnPropertyChanged(nameof(SelectionLength));
            return true;
        }

        #endregion

        /// <summary>
        /// 多行编辑是否允许输入制表符
        /// </summary>
        [Description("多行编辑是否允许输入制表符"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool AcceptsTab { get; set; }

        /// <summary>
        /// 失去焦点时是否隐藏选定内容
        /// </summary>
        [Description("失去焦点时是否隐藏选定内容"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool HideSelection { get; set; } = true;

        /// <summary>
        /// 使用默认右键菜单
        /// </summary>
        [Description("使用默认右键菜单"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool UseContextMenu { get; set; } = true;

        /// <summary>
        /// 焦点离开清空选中
        /// </summary>
        [Obsolete("use HideSelection"), Description("焦点离开清空选中"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool LostFocusClearSelection
        {
            get => HideSelection;
            set => HideSelection = value;
        }

        bool readOnly = false;
        /// <summary>
        /// 只读
        /// </summary>
        [Description("只读"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public virtual bool ReadOnly
        {
            get => readOnly;
            set
            {
                if (readOnly == value) return;
                readOnly = value;
                SetImeMode(value ? ImeMode.Disable : imeMode);
                OnPropertyChanged(nameof(ReadOnly));
            }
        }

        bool multiline = false;
        /// <summary>
        /// 多行文本
        /// </summary>
        [Description("多行文本"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public virtual bool Multiline
        {
            get => multiline;
            set
            {
                if (multiline == value) return;
                multiline = value;
                if (multiline)
                {
                    sf_placeholder &= ~FormatFlags.VerticalCenter & ~FormatFlags.NoWrap;
                    sf_placeholder |= FormatFlags.Top;
                }
                else
                {
                    sf_placeholder &= ~FormatFlags.Top;
                    sf_placeholder |= FormatFlags.VerticalCenter | FormatFlags.NoWrap;
                }
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(Multiline));
            }
        }

        bool wordwrap = true;
        /// <summary>
        /// 自动换行
        /// </summary>
        [Description("自动换行"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public virtual bool WordWrap
        {
            get => wordwrap;
            set
            {
                if (wordwrap == value) return;
                wordwrap = value;
                if (multiline)
                {
                    CalculateRect();
                    Invalidate();
                }
                OnPropertyChanged(nameof(WordWrap));
            }
        }

        int lineheight = 0;
        [Description("多行行高"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(0)]
        public int LineHeight
        {
            get => lineheight;
            set
            {
                if (lineheight == value) return;
                lineheight = value;
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(LineHeight));
            }
        }

        HorizontalAlignment textalign = HorizontalAlignment.Left;
        /// <summary>
        /// 文本对齐方向
        /// </summary>
        [Description("文本对齐方向"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get => textalign;
            set
            {
                if (textalign == value) return;
                textalign = value;
                sf_placeholder = textalign.SetAlignment(sf_placeholder);
                CalculateRect();
                Invalidate();
                OnPropertyChanged(nameof(TextAlign));
            }
        }

        #region 水印

        string? placeholderText;
        /// <summary>
        /// 水印文本
        /// </summary>
        [Description("水印文本"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(null)]
        [Localizable(true)]
        public virtual string? PlaceholderText
        {
            get => this.GetLangI(LocalizationPlaceholderText, placeholderText);
            set
            {
                if (placeholderText == value) return;
                placeholderText = value;
                if (isempty && ShowPlaceholder) Invalidate();
                OnPropertyChanged(nameof(PlaceholderText));
            }
        }

        [Description("水印文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationPlaceholderText { get; set; }

        Color? placeholderColor;
        /// <summary>
        /// 水印颜色
        /// </summary>
        [Description("水印颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? PlaceholderColor
        {
            get => placeholderColor;
            set
            {
                if (placeholderColor == value) return;
                placeholderColor = value;
                if (isempty && ShowPlaceholder) Invalidate();
                OnPropertyChanged(nameof(PlaceholderColor));
            }
        }

        string? placeholderColorExtend;
        /// <summary>
        /// 水印渐变色
        /// </summary>
        [Description("水印渐变色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? PlaceholderColorExtend
        {
            get => placeholderColorExtend;
            set
            {
                if (placeholderColorExtend == value) return;
                placeholderColorExtend = value;
                if (isempty && ShowPlaceholder) Invalidate();
                OnPropertyChanged(nameof(PlaceholderColorExtend));
            }
        }

        #endregion

        /// <summary>
        /// 文本最大长度
        /// </summary>
        [Description("文本最大长度"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(32767)]
        public int MaxLength { get; set; } = 32767;

        /// <summary>
        /// 形态
        /// </summary>
        [Description("形态"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TVariant.Outlined)]
        public TVariant Variant { get; set; } = TVariant.Outlined;

        #region 密码框

        bool IsPassWord = false;
        string PassWordChar = "●";

        bool useSystemPasswordChar = false;
        /// <summary>
        /// 使用密码框
        /// </summary>
        [Description("使用密码框"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public virtual bool UseSystemPasswordChar
        {
            get => useSystemPasswordChar;
            set
            {
                if (useSystemPasswordChar == value) return;
                useSystemPasswordChar = value;
                SetPassWord();
                OnPropertyChanged(nameof(UseSystemPasswordChar));
            }
        }

        char passwordChar = '\0';
        /// <summary>
        /// 自定义密码字符
        /// </summary>
        [Description("自定义密码字符"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue((char)0)]
        public virtual char PasswordChar
        {
            get => passwordChar;
            set
            {
                if (passwordChar == value) return;
                passwordChar = value;
                SetPassWord();
                OnPropertyChanged(nameof(PasswordChar));
            }
        }

        /// <summary>
        /// 密码可以复制
        /// </summary>
        [Description("密码可以复制"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool PasswordCopy { get; set; }

        /// <summary>
        /// 密码可以粘贴
        /// </summary>
        [Description("密码可以粘贴"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool PasswordPaste { get; set; } = true;

        void SetPassWord()
        {
            if (passwordChar != '\0')
            {
                PassWordChar = passwordChar.ToString();
                IsPassWord = true;
            }
            else if (useSystemPasswordChar)
            {
                PassWordChar = "●";
                IsPassWord = true;
            }
            else IsPassWord = false;
            SetImeMode(IsPassWord ? ImeMode.Disable : imeMode);
            FixFontWidth(true);
            Invalidate();
        }

        #endregion

        #endregion

        #region 方法

        #region 修改文本

        /// <summary>
        /// 将文本追加到当前文本中
        /// </summary>
        /// <param name="text">追加的文本</param>
        public void AppendText(string text)
        {
            int start = cache_font == null ? 0 : (cache_font[cache_font.Count - 1].i + 1);
            bool set_t = SetTextAppend(text, start, out _), set_s = SetSelectionStart(selectionStart + GraphemeSplitter.EachCount(text));
            if (set_t || set_s) Invalidate();
        }

        /// <summary>
        /// 追加文本到末尾
        /// </summary>
        /// <param name="text">追加的文本</param>
        /// <param name="config">文本配置</param>
        public void AppendText(string text, TextOpConfig config)
        {
            int start = cache_font == null ? 0 : (cache_font[cache_font.Count - 1].i + 1), start_tmp = selectionStart;
            bool set_t = SetTextAppend(text, start, out int length), set_style = false, set_s = false;
            if (config.Font != null || config.Fore.HasValue || config.Back.HasValue)
            {
                var data = new TextStyle(start, length, config.Font, config.Fore, config.Back);
                set_style = SetStyle(data, false);
            }
            switch (config.CursorBehavior)
            {
                case CursorBehavior.EndOfNewText:
                    set_s = SetSelectionStart(start_tmp + length);
                    break;
                case CursorBehavior.EndOfWholeText:
                    if (cache_font != null) set_s = SetSelectionStart(cache_font.Count);
                    break;
            }
            if (config.Redraw && (set_t || set_s || set_style)) Invalidate();
        }

        /// <summary>
        /// 插入文本
        /// </summary>
        /// <param name="startIndex">开始位置</param>
        /// <param name="text">文本</param>
        /// <param name="config">文本配置</param>
        public void InsertText(int startIndex, string text, TextOpConfig config)
        {
            if (cache_font == null)
            {
                AppendText(text, config);
                return;
            }
            int start = selectionStartTemp, end = selectionLength;
            if (end > 0) SetTextRemove(ref cache_font, start, end, true);
            bool set_t = SetTextIn(text, startIndex, out int length), set_style = false, set_s = false;
            if (config.Font != null || config.Fore.HasValue || config.Back.HasValue)
            {
                var data = new TextStyle(start, length, config.Font, config.Fore, config.Back);
                set_style = SetStyle(data, false);
            }
            switch (config.CursorBehavior)
            {
                case CursorBehavior.EndOfNewText:
                    set_s = SetSelectionStart(start + length);
                    break;
                case CursorBehavior.EndOfWholeText:
                    if (cache_font != null) set_s = SetSelectionStart(cache_font.Count);
                    break;
                case CursorBehavior.StartOfNewText:
                    set_s = SetSelectionStart(start);
                    break;
            }
            if (config.Redraw && (set_t || set_s || set_style)) Invalidate();
        }

        #endregion

        /// <summary>
        /// 清除所有文本
        /// </summary>
        public void Clear() => Text = "";

        /// <summary>
        /// 清除撤消缓冲区信息
        /// </summary>
        public void ClearUndo() => history_Log.Clear();

        /// <summary>
        /// 复制
        /// </summary>
        public void Copy()
        {
            if (IsPassWord && !PasswordCopy) return;
            var text = GetSelectionText();
            if (string.IsNullOrEmpty(text)) return;
            this.ClipboardSetText(text);
        }

        /// <summary>
        /// 剪贴
        /// </summary>
        public void Cut()
        {
            if (IsPassWord && !PasswordCopy) return;
            var text = GetSelectionText();
            if (string.IsNullOrEmpty(text)) return;
            this.ClipboardSetText(text);
            ProcessBackSpaceKey();
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        public void Paste()
        {
            if (IsPassWord && !PasswordPaste) return;
            var strText = this.ClipboardGetText();
            if (strText == null || string.IsNullOrEmpty(strText)) return;
            var chars = new List<string>(strText.Length);
            foreach (char key in strText)
            {
                if (Verify(key, out var change)) chars.Add(change ?? key.ToString());
            }
            if (chars.Count > 0) EnterText(string.Join("", chars));
        }

        /// <summary>
        /// 取消全部选中
        /// </summary>
        public void DeselectAll() => SelectionLength = 0;

        /// <summary>
        /// 撤消
        /// </summary>
        public void Undo()
        {
            if (IsPassWord && !PasswordCopy) return;
            if (history_Log.Count > 0)
            {
                int index;
                if (history_I == -1)
                {
                    index = history_Log.Count - 1;
                    AddHistoryRecord();
                }
                else index = history_I - 1;
                if (index > -1)
                {
                    var it = history_Log[index];
                    history_I = index;
                    bool set_t = SetText(it.Text, true), set_s = SetSelectionStart(it.SelectionStart), set_e = SetSelectionLength(it.SelectionLength);
                    if (set_t || set_s || set_e) Invalidate();
                }
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public void Redo()
        {
            if (IsPassWord && !PasswordCopy) return;
            if (history_Log.Count > 0 && history_I > -1)
            {
                int index = history_I + 1;
                if (history_Log.Count > index)
                {
                    var it = history_Log[index];
                    history_I = index;
                    bool set_t = SetText(it.Text, true), set_s = SetSelectionStart(it.SelectionStart), set_e = SetSelectionLength(it.SelectionLength);
                    if (set_t || set_s || set_e) Invalidate();
                }
            }
        }

        /// <summary>
        /// 是否可以撤消
        /// </summary>
        [Browsable(false)]
        public bool CanUndo
        {
            get
            {
                if (IsPassWord && !PasswordCopy) return false;
                if (history_Log.Count > 0)
                {
                    int index;
                    if (history_I == -1) index = history_Log.Count - 1;
                    else index = history_I - 1;
                    if (index > -1) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否可以重做
        /// </summary>
        [Browsable(false)]
        public bool CanRedo
        {
            get
            {
                if (IsPassWord && !PasswordCopy) return false;
                if (history_Log.Count > 0 && history_I > -1)
                {
                    int index = history_I + 1;
                    if (history_Log.Count > index) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 文本选择范围
        /// </summary>
        /// <param name="start">第一个字符的位置</param>
        /// <param name="length">字符长度</param>
        /// <param name="rd">是否渲染</param>
        public void Select(int start, int length)
        {
            bool set_s = SetSelectionStart(start), set_e = SetSelectionLength(length);
            if (set_s || set_e) Invalidate();
        }

        #region 样式

        TextStyle[]? styles;
        /// <summary>
        /// 设置样式
        /// </summary>
        /// <param name="start">第一个字符的位置</param>
        /// <param name="length">字符长度</param>
        /// <param name="font">字体</param>
        /// <param name="fore">文本颜色</param>
        /// <param name="back">背景颜色</param>
        public bool SetStyle(int start, int length, Font? font = null, Color? fore = null, Color? back = null) => SetStyle(new TextStyle(start, length, font, fore, back));

        /// <summary>
        /// 设置样式
        /// </summary>
        public bool SetStyle(TextStyle style, bool rd = true)
        {
            if (styles == null) styles = new TextStyle[] { style };
            else
            {
                var tmp = new List<TextStyle>(styles.Length + 1);
                tmp.AddRange(styles);
                tmp.Add(style);
                styles = tmp.ToArray();
            }
            if (ApplyStyle(style))
            {
                if (rd) Invalidate();
                return true;
            }
            return false;
        }

        void SetStyle()
        {
            if (styles == null) return;
            foreach (var it in styles) ApplyStyle(it);
        }
        bool ApplyStyle(TextStyle style)
        {
            if (cache_font == null) return false;
            int len = style.Start + style.Length;
            if (style.Start < 0 || len > cache_font.Count) return false;
            for (int i = style.Start; i < len; i++)
            {
                var it = cache_font[i];
                it.font = style.Font;
                it.fore = style.Fore;
                it.back = style.Back;
            }
            return true;
        }

        /// <summary>
        /// 清空样式
        /// </summary>
        public void ClearStyle(bool r = true)
        {
            if (styles == null) return;
            styles = null;
            if (cache_font == null) return;
            for (int i = 0; i < cache_font.Count; i++)
            {
                var it = cache_font[i];
                it.font = null;
                it.fore = null;
                it.back = null;
            }
            if (r) Invalidate();
        }

        public class TextStyle
        {
            public TextStyle(int start, int length)
            {
                Start = start;
                Length = length;
            }

            public TextStyle(int start, int length, Font? font, Color? fore, Color? back)
            {
                Start = start;
                Length = length;
                Font = font;
                Fore = fore;
                Back = back;
            }

            public int Start { get; set; }
            public int Length { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 文本颜色
            /// </summary>
            public Color? Fore { get; set; }

            /// <summary>
            /// 背景色
            /// </summary>
            public Color? Back { get; set; }

            #region 设置

            public TextStyle Set(int start, int length)
            {
                Start = start;
                Length = length;
                return this;
            }
            public TextStyle SetStart(int value)
            {
                Start = value;
                return this;
            }
            public TextStyle SetLength(int value)
            {
                Length = value;
                return this;
            }
            public TextStyle SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public TextStyle SetFore(Color? value)
            {
                Fore = value;
                return this;
            }
            public TextStyle SetBack(Color? value)
            {
                Back = value;
                return this;
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// 选择最后文本
        /// </summary>
        public void SelectLast()
        {
            if (cache_font == null) return;
            bool set_s = SetSelectionStart(cache_font.Count), set_e = SetSelectionLength(0), set_caret = SetCaretPostion(cache_font.Count + 1);
            if (set_s || set_e || set_caret) Invalidate();
        }

        /// <summary>
        /// 选择所有文本
        /// </summary>
        public void SelectAll()
        {
            if (cache_font == null) return;
            bool set_s = SetSelectionStart(0), set_e = SetSelectionLength(cache_font.Count), set_caret = SetCaretPostion(cache_font.Count + 1);
            if (set_s || set_e || set_caret) Invalidate();
        }

        /// <summary>
        /// 当前位置插入文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="ismax">是否限制MaxLength</param>
        public void EnterText(string text, bool ismax = true)
        {
            CloseContextMenu();
            if (ReadOnly || BanInput) return;
            int offset = 0, rdcount = 0;
            if (cache_font == null)
            {
                if (ismax && text.Length > MaxLength)
                {
                    text = text.Substring(0, MaxLength);
                    if (text.Length == 0) return;
                }
                AddHistoryRecord();
                if (SetText(text, true, false)) rdcount++;
            }
            else
            {
                if (selectionLength > 0)
                {
                    int start = selectionStartTemp, end = selectionLength;
                    if (ismax && (cache_font.Count - end + text.Length) > MaxLength)
                    {
                        if (MaxLength > cache_font.Count)
                        {
                            text = text.Substring(0, end);
                            if (text.Length == 0) return;
                        }
                        else return;
                    }
                    AddHistoryRecord();
                    SetTextRemove(ref cache_font, start, end, true);
                    if (SetTextIn(text, start, out _, false)) rdcount++;
                    if (SetSelectionLength(0)) rdcount++;
                    offset = start;
                }
                else
                {
                    int start = selectionStart - 1;
                    if (ismax && (cache_font.Count + text.Length) > MaxLength)
                    {
                        if (MaxLength > cache_font.Count)
                        {
                            text = text.Substring(0, MaxLength - cache_font.Count);
                            if (text.Length == 0) return;
                        }
                        else return;
                    }
                    AddHistoryRecord();
                    if (SetTextIn(text, start + 1, out _, false)) rdcount++;
                    offset = start + 1;
                }
            }
            int len = GraphemeSplitter.EachCount(text);
            if (SetSelectionStart(offset + len)) rdcount++;
            OnTextChanged(EventArgs.Empty);
            OnPropertyChanged(nameof(Text));
            if (rdcount > 0) Invalidate();
        }

        /// <summary>
        /// 获取设置当前选中文本
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("获取设置当前选中文本"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        public virtual string? SelectedText
        {
            get => GetSelectionText();
            set
            {
                if (value == null || string.IsNullOrEmpty(value)) SelectionLength = 0;
                else
                {
                    int index = _text.IndexOf(value);
                    if (index > -1)
                    {
                        bool set_s = SetSelectionStart(index), set_e = SetSelectionLength(value.Length);
                        if (set_s || set_e) Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前选中文本
        /// </summary>
        public string? GetSelectionText()
        {
            if (cache_font == null) return null;
            else
            {
                if (selectionLength > 0)
                {
                    int start = selectionStartTemp, end = selectionLength;
                    int end_temp = start + end;
                    if (end_temp > cache_font.Count) end_temp = cache_font.Count;
                    var texts = new List<string>(end);
                    foreach (var it in cache_font)
                    {
                        if (it.i >= start && end_temp > it.i) texts.Add(it.text);
                    }
                    return string.Join("", texts);
                }
                return null;
            }
        }

        /// <summary>
        /// 内容滚动到当前插入符号位置
        /// </summary>
        public void ScrollToCaret()
        {
            if (cache_font != null)
            {
                Rectangle r;
                if (CurrentPosIndex >= cache_font.Count) r = cache_font[cache_font.Count - 1].rect;
                else r = cache_font[CurrentPosIndex].rect;
                ScrollY.Value = r.Bottom;
            }
        }

        /// <summary>
        /// 内容滚动到最下面
        /// </summary>
        public void ScrollToEnd() => ScrollY.Value = ScrollY.Max;

        /// <summary>
        /// 滚动到指定行
        /// </summary>
        public void ScrollLine(int i)
        {
            int lineHeight = CaretInfo.Height + (lineheight > 0 ? (int)(lineheight * Dpi) : 0);
            ScrollY.Value = lineHeight * i;
        }

        #region 字符串搜索

        /// <summary>
        /// 查找指定字符串在文本中首次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <returns>首次出现的字符簇索引，未找到返回 -1</returns>
        public int IndexOf(string value) => IndexOf(value, 0, StringComparison.CurrentCulture);

        /// <summary>
        /// 从指定位置开始查找字符串首次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引</param>
        /// <returns>首次出现的字符簇索引，未找到返回 -1</returns>
        public int IndexOf(string value, int startIndex) => IndexOf(value, startIndex, StringComparison.CurrentCulture);

        /// <summary>
        /// 从指定位置开始，在指定范围内查找字符串首次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引</param>
        /// <param name="count">要搜索的字符簇数量</param>
        /// <returns>首次出现的字符簇索引，未找到返回 -1</returns>
        public int IndexOf(string value, int startIndex, int count) => IndexOf(value, startIndex, count, StringComparison.CurrentCulture);

        /// <summary>
        /// 查找指定字符串在文本中首次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="comparisonType">字符串比较类型</param>
        /// <returns>首次出现的字符簇索引，未找到返回 -1</returns>
        public int IndexOf(string value, StringComparison comparisonType) => IndexOf(value, 0, comparisonType);

        /// <summary>
        /// 从指定位置开始查找字符串首次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引</param>
        /// <param name="comparisonType">字符串比较类型</param>
        /// <returns>首次出现的字符簇索引，未找到返回 -1</returns>
        public int IndexOf(string value, int startIndex, StringComparison comparisonType)
        {
            if (cache_font == null) return -1;
            return IndexOf(cache_font, value, startIndex, null, comparisonType);
        }

        /// <summary>
        /// 从指定位置开始，在指定范围内查找字符串首次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引</param>
        /// <param name="count">要搜索的字符簇数量</param>
        /// <param name="comparisonType">字符串比较类型</param>
        /// <returns>首次出现的字符簇索引，未找到返回 -1</returns>
        public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
        {
            if (cache_font == null) return -1;
            return IndexOf(cache_font, value, startIndex, count, comparisonType);
        }

        int IndexOf(List<CacheFont> cache_font, string value, int startIndex, int? count, StringComparison comparisonType)
        {
            if (startIndex < 0) startIndex = 0;
            if (startIndex >= cache_font.Count) return -1;
            if (count.HasValue)
            {
                if (count < 0) count = 0;
                if (startIndex + count > cache_font.Count) count = cache_font.Count - startIndex;
            }
            else count = cache_font.Count - startIndex;

            var valueParts = IndexSplitter(value);
            int valueLength = valueParts.Length;
            if (valueLength == 0 || valueLength > count) return -1;

            for (int i = startIndex; i <= startIndex + count - valueLength; i++)
            {
                if (IndexOf(cache_font, i, valueLength, valueParts, comparisonType)) return i;
            }
            return -1;
        }

        bool IndexOf(List<CacheFont> cache_font, int start, int valueLength, string[] valueParts, StringComparison comparisonType)
        {
            for (int i = 0; i < valueLength; i++)
            {
                int currentPos = start + i;
                if (currentPos >= cache_font.Count) return false;
                if (!string.Equals(cache_font[currentPos].text, valueParts[i], comparisonType)) return false;
            }
            return true;
        }

        /// <summary>
        /// 查找指定字符串在文本中最后一次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <returns>最后一次出现的字符簇索引，未找到返回 -1</returns>
        public int LastIndexOf(string value)
        {
            if (cache_font == null) return -1;
            return LastIndexOf(cache_font, value, cache_font.Count - 1, null, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// 从指定位置开始向前查找字符串最后一次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引（从该位置向前搜索）</param>
        /// <returns>最后一次出现的字符簇索引，未找到返回 -1</returns>
        public int LastIndexOf(string value, int startIndex) => LastIndexOf(value, startIndex, StringComparison.CurrentCulture);

        /// <summary>
        /// 从指定位置开始，在指定范围内向前查找字符串最后一次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引（从该位置向前搜索）</param>
        /// <param name="count">要搜索的字符簇数量</param>
        /// <returns>最后一次出现的字符簇索引，未找到返回 -1</returns>
        public int LastIndexOf(string value, int startIndex, int count) => LastIndexOf(value, startIndex, count, StringComparison.CurrentCulture);

        /// <summary>
        /// 查找指定字符串在文本中最后一次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="comparisonType">字符串比较类型</param>
        /// <returns>最后一次出现的字符簇索引，未找到返回 -1</returns>
        public int LastIndexOf(string value, StringComparison comparisonType)
        {
            if (cache_font == null) return -1;
            return LastIndexOf(value, cache_font.Count - 1, comparisonType);
        }

        /// <summary>
        /// 从指定位置开始向前查找字符串最后一次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引（从该位置向前搜索）</param>
        /// <param name="comparisonType">字符串比较类型</param>
        /// <returns>最后一次出现的字符簇索引，未找到返回 -1</returns>
        public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
        {
            if (cache_font == null) return -1;
            return LastIndexOf(cache_font, value, startIndex, null, comparisonType);
        }

        /// <summary>
        /// 从指定位置开始，在指定范围内向前查找字符串最后一次出现的位置（基于字符簇索引）
        /// </summary>
        /// <param name="value">要查找的字符串</param>
        /// <param name="startIndex">开始搜索的字符簇索引（从该位置向前搜索）</param>
        /// <param name="count">要搜索的字符簇数量</param>
        /// <param name="comparisonType">字符串比较类型</param>
        /// <returns>最后一次出现的字符簇索引，未找到返回 -1</returns>
        public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
        {
            if (cache_font == null) return -1;
            return LastIndexOf(cache_font, value, startIndex, count, comparisonType);
        }

        int LastIndexOf(List<CacheFont> cache_font, string value, int startIndex, int? count, StringComparison comparisonType)
        {
            if (startIndex < 0) startIndex = 0;
            if (startIndex >= cache_font.Count) startIndex = cache_font.Count - 1;
            if (count.HasValue)
            {
                if (count < 0) count = 0;
                if (count > startIndex + 1) count = startIndex + 1;
            }
            else count = startIndex + 1;

            var valueParts = IndexSplitter(value);
            int valueLength = valueParts.Length;
            if (valueLength == 0 || valueLength > count) return -1;

            int endIndex = startIndex - count.Value + 1;
            if (endIndex < 0) endIndex = 0;

            for (int i = startIndex; i >= endIndex; i--)
            {
                if (i + valueLength > cache_font.Count) continue;
                if (IndexOf(cache_font, i, valueLength, valueParts, comparisonType)) return i;
            }
            return -1;
        }

        string[] IndexSplitter(string value)
        {
            var valueParts = new List<string>(value.Length);
            GraphemeSplitter.Each(value, (txt, ntype) => valueParts.Add(txt));
            return valueParts.ToArray();
        }

        #endregion

        #region 字符串操作

        /// <summary>
        /// 从指定字符簇位置开始截取子字符串
        /// </summary>
        /// <param name="startIndex">开始截取的字符簇索引</param>
        /// <returns>从指定位置开始的子字符串</returns>
        public string Substring(int startIndex)
        {
            if (cache_font == null) return "";
            return Substring(cache_font, startIndex, cache_font.Count - startIndex);
        }

        /// <summary>
        /// 从指定字符簇位置开始截取指定长度的子字符串
        /// </summary>
        /// <param name="startIndex">开始截取的字符簇索引</param>
        /// <param name="length">要截取的字符簇数量</param>
        /// <returns>截取的子字符串</returns>
        public string Substring(int startIndex, int length)
        {
            if (cache_font == null) return "";
            if (length < 0) length = 0;
            if (startIndex + length > cache_font.Count) length = cache_font.Count - startIndex;
            return Substring(cache_font, startIndex, length);
        }

        string Substring(List<CacheFont> cache_font, int startIndex, int length)
        {
            if (startIndex < 0) startIndex = 0;
            if (startIndex >= cache_font.Count) return "";
            var result = new System.Text.StringBuilder(length);
            for (int i = startIndex; i < startIndex + length; i++) result.Append(cache_font[i].text);
            return result.ToString();
        }

        #endregion

        #endregion

        #region 重写

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            FixFontWidth(true);
        }

        #region 焦点

        bool AnimationFocus = false;
        int AnimationFocusValue = 0;

        /// <summary>
        /// 是否存在焦点
        /// </summary>
        [Browsable(false)]
        [Description("是否存在焦点"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool HasFocus { get; private set; }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            HasFocus = true;
            CaretInfo.Show = true;
            ExtraMouseDown = true;
            if (Helper.IsTouch()) Helper.OpenOsk();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            HasFocus = false;
            CaretInfo.Show = false;
            if (HideSelection) SelectionLength = 0;
            ExtraMouseDown = false;
            base.OnLostFocus(e);
        }

        #endregion

        #region 系统消息

        IntPtr m_hIMC;
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x0102:
                case 0x0109:
                    IKeyPress((char)m.WParam.ToInt32());
                    break;
                //case 0x0286:
                //    break;
                case 0x100://WM_KEYDOWN
                case 0x0104://WM_SYSKEYDOWN
                    HandKeyDown(GetKeyBoard(m.WParam.ToInt32()));
                    break;
                //case 0x101://WM_KEYUP
                //case 0x0105://WM_SYSKEYUP
                //    if (HandKeyUp(GetKeyBoard(m.WParam.ToInt32()))) return;
                //    break;
                case Win32.Imm32.WM_IME_STARTCOMPOSITION:
                    m_hIMC = Win32.Imm32.ImmGetContext(Handle);
                    OnImeStartPrivate(m_hIMC);
                    break;
                case Win32.Imm32.WM_IME_ENDCOMPOSITION:
                    Win32.Imm32.ImmReleaseContext(Handle, m_hIMC);
                    break;
                case Win32.Imm32.WM_IME_COMPOSITION:
                    if (((int)m.LParam & Win32.Imm32.GCS_RESULTSTR) == Win32.Imm32.GCS_RESULTSTR)
                    {
#if NET40 || NET46 || NET48 || NET6_0
                        m.Result = (IntPtr)1;
#else
                        m.Result = 1;
#endif
                        OnImeResultStrPrivate(m_hIMC, Win32.Imm32.ImmGetCompositionString(m_hIMC, Win32.Imm32.GCS_RESULTSTR));
                        return;
                    }
                    break;
                case 0x007B:
                    CloseContextMenu();
                    if (UseContextMenu) OnOpenContextMenu();
                    break;
            }
            base.WndProc(ref m);
        }

        Form? _contextMenu;
        void CloseContextMenu()
        {
            _contextMenu?.Close();
            _contextMenu = null;
        }
        protected virtual void OnOpenContextMenu()
        {
            var items = new List<IContextMenuStripItem>(9);
            if (readOnly)
            {
                items.Add(new ContextMenuStripItem().SetText("复制", "{id}").SetID("Copy").SetSubText("Ctrl+C").SetEnabled(selectionLength > 0));
                items.Add(new ContextMenuStripItemDivider());
                items.Add(new ContextMenuStripItem().SetText("全选", "{id}").SetID("SelectAll").SetSubText("Ctrl+A").SetEnabled(!isempty));
            }
            else
            {
                bool canUndo = CanUndo, canRedo = CanRedo;
                if (canUndo || canRedo)
                {
                    if (canUndo) items.Add(new ContextMenuStripItem().SetText("撤销", "{id}").SetID("Undo").SetSubText("Ctrl+Z"));
                    if (canRedo) items.Add(new ContextMenuStripItem().SetText("重做", "{id}").SetID("Redo").SetSubText("Ctrl+Y"));
                    items.Add(new ContextMenuStripItemDivider());
                }
                if (!IsPassWord || (IsPassWord && PasswordCopy))
                {
                    items.Add(new ContextMenuStripItem().SetText("剪切", "{id}").SetID("Cut").SetSubText("Ctrl+X").SetEnabled(selectionLength > 0));
                    items.Add(new ContextMenuStripItem().SetText("复制", "{id}").SetID("Copy").SetSubText("Ctrl+C").SetEnabled(selectionLength > 0));
                }
                if (!IsPassWord || (IsPassWord && PasswordPaste)) items.Add(new ContextMenuStripItem().SetText("粘贴", "{id}").SetID("Paste").SetSubText("Ctrl+V"));

                items.Add(new ContextMenuStripItem().SetText("删除", "{id}").SetID("Delete").SetSubText("Del").SetEnabled(selectionLength > 0));
                items.Add(new ContextMenuStripItemDivider());
                items.Add(new ContextMenuStripItem().SetText("全选", "{id}").SetID("SelectAll").SetSubText("Ctrl+A").SetEnabled(!isempty));
            }

            var config = new ContextMenuStrip.Config(this, item =>
            {
                switch (item.ID)
                {
                    case "Undo":
                        Undo();
                        break;
                    case "Redo":
                        Redo();
                        break;
                    case "Cut":
                        Cut();
                        break;
                    case "Copy":
                        Copy();
                        break;
                    case "Paste":
                        Paste();
                        break;
                    case "Delete":
                        ProcessBackSpaceKey(false);
                        break;
                    case "SelectAll":
                        SelectAll();
                        break;
                }
            },
            items.ToArray());
            if (!_mouseHover) config.SetLocation(PointToScreen(CaretInfo.Rect.Location));
            _contextMenu = config.open();
        }

        #endregion

        #region 虚拟

        /// <summary>
        /// 禁止输入
        /// </summary>
        protected virtual bool BanInput => false;
        protected virtual bool HasValue => false;
        protected virtual bool ModeRange => false;
        protected virtual void ModeRangeCaretPostion(bool Null) { }

        /// <summary>
        /// 是否显示水印
        /// </summary>
        protected virtual bool ShowPlaceholder => true;
        protected virtual bool HasLeft() => false;
        protected virtual int[] UseLeft(Rectangle rect, int font_height) => new int[] { 0, 0 };
        protected virtual void UseLeftAutoHeight(int height, int y) { }

        protected virtual void IBackSpaceKey() { }

        protected virtual bool IMouseDown(int x, int y) => false;
        protected virtual bool IMouseMove(int x, int y) => false;
        protected virtual bool IMouseUp(int x, int y) => false;

        /// <summary>
        /// 清空值
        /// </summary>
        protected virtual void OnClearValue() => Text = "";

        /// <summary>
        /// 点击内容
        /// </summary>
        protected virtual void OnClickContent() { }

        /// <summary>
        /// 改变鼠标状态
        /// </summary>
        /// <param name="Hover">移入</param>
        /// <param name="Focus">焦点</param>
        protected virtual void ChangeMouseHover(bool Hover, bool Focus) { }

        #endregion

        protected virtual bool HasAnimation => true;

        #endregion

        #region 光标

        internal ICaret CaretInfo;

        #region 得到光标位置

        protected virtual void SetCaretPostion(ref int x, ref int y)
        {
        }

        /// <summary>
        /// 通过坐标系查找光标位置
        /// </summary>
        CacheCaret? GetCaretPostion(int x, int y)
        {
            SetCaretPostion(ref x, ref y);
            if (cache_caret == null) return null;
            else return FindNearestFont(x, y, cache_caret);
        }

        /// <summary>
        /// 寻找最近的矩形和距离的辅助方法
        /// </summary>
        CacheCaret FindNearestFont(int x, int y, CacheCaret[] cache_caret)
        {
            CacheCaret first = cache_caret[0], last = cache_caret[cache_caret.Length - 1];
            if (multiline)
            {
                if (x < first.x && y < first.y) return first;
                else if (x > last.x && y > last.y) return last;
            }
            else
            {
                if (x < first.x) return first;
                else if (x > last.x) return last;
            }
            var findy = FindNearestFontY(y, cache_caret);
            int index = 0;
            long minDiff = Math.Abs((long)cache_caret[0].x - x);
            for (int i = 1; i < cache_caret.Length; i++)
            {
                if (cache_caret[i].y == findy)
                {
                    long currentDiff = Math.Abs((long)(cache_caret[i].x) - x);

                    // 如果当前差值更小，更新记录
                    if (currentDiff < minDiff)
                    {
                        minDiff = currentDiff;
                        index = i;
                        if (minDiff == 0) break;
                    }
                }
            }
            return cache_caret[index];
        }
        int FindNearestFontY(int y, CacheCaret[] cache_caret)
        {
            int index = 0;
            int offset = CaretInfo.Height / 2;
            long minDiff = Math.Abs((long)cache_caret[0].y - y);
            for (int i = 1; i < cache_caret.Length; i++)
            {
                long currentDiff = Math.Abs((long)(cache_caret[i].y + offset) - y);

                // 如果当前差值更小，更新记录
                if (currentDiff < minDiff)
                {
                    minDiff = currentDiff;
                    index = i;
                    if (minDiff == 0) break;
                }
            }
            return cache_caret[index].y;
        }

        #endregion

        int CurrentPosIndex = 0;
        internal bool SetCaretPostion(bool rd = true) => SetCaretPostion(CurrentPosIndex, rd);
        internal bool SetCaretPostion(int PosIndex, bool rd = true)
        {
            if (PosIndex < 0) return false;
            CurrentPosIndex = PosIndex;
            if (cache_caret == null)
            {
                if (ModeRange) ModeRangeCaretPostion(true);
                else
                {
                    if (textalign == HorizontalAlignment.Center) CaretInfo.X = rect_text.X + rect_text.Width / 2;
                    else if (textalign == HorizontalAlignment.Right) CaretInfo.X = rect_text.Right;
                }
            }
            else
            {
                if (PosIndex > cache_caret.Length - 1)
                {
                    PosIndex = cache_caret.Length - 1;
                    return false;
                }
                var r = CaretInfo.SetXY(cache_caret, PosIndex, out var rd2);
                if (ModeRange) ModeRangeCaretPostion(false);
                return ScrollIFTo(r, rd) || rd2;
            }
            CaretInfo.flag = true;
            return true;
        }

        void OnImeStartPrivate(IntPtr hIMC)
        {
            try
            {
                var point = CaretInfo.Rect.Location;
                point.Offset(0, -ScrollY.Value);
                var CandidateForm = new Win32.Imm32.CANDIDATEFORM()
                {
                    dwStyle = Win32.Imm32.CFS_CANDIDATEPOS,
                    ptCurrentPos = point,
                };
                Win32.Imm32.ImmSetCandidateWindow(hIMC, ref CandidateForm);
                var CompositionForm = new Win32.Imm32.COMPOSITIONFORM()
                {
                    dwStyle = Win32.Imm32.CFS_FORCE_POSITION,
                    ptCurrentPos = point,
                };
                Win32.Imm32.ImmSetCompositionWindow(hIMC, ref CompositionForm);
                var logFont = new Win32.Imm32.LOGFONT()
                {
                    lfHeight = CaretInfo.Rect.Height,
                    lfFaceName = Font.Name + "\0"
                };
                Win32.Imm32.ImmSetCompositionFont(hIMC, ref logFont);
            }
            catch { }
        }
        void OnImeResultStrPrivate(IntPtr hIMC, string? strResult)
        {
            try
            {
                var CompositionForm = new Win32.Imm32.COMPOSITIONFORM()
                {
                    dwStyle = Win32.Imm32.CFS_FORCE_POSITION,
                    ptCurrentPos = CaretInfo.Rect.Location
                };
                Win32.Imm32.ImmSetCompositionWindow(hIMC, ref CompositionForm);
                if (strResult != null && !string.IsNullOrEmpty(strResult))
                {
                    var chars = new List<string>(strResult.Length);
                    foreach (char key in strResult)
                    {
                        if (Verify(key, out var change)) chars.Add(change ?? key.ToString());
                    }
                    if (chars.Count > 0) EnterText(string.Join("", chars));
                }
            }
            catch { }
        }

        protected virtual bool Verify(char key, out string? change)
        {
            if (VerifyChar == null)
            {
                change = null;
                return true;
            }
            var args = new InputVerifyCharEventArgs(key);
            VerifyChar(this, args);
            change = args.ReplaceText;
            return args.Result;
        }

        internal class ICaret
        {
            Input control;
            public ICaret(Input input)
            {
                control = input;
                Rect = new Rectangle(-1, -1000, input.Dpi < 1 ? 1 : (int)input.Dpi, 0);
            }

            public Rectangle Rect;

            public Rectangle SetXY(CacheCaret[] cache_font, int i, out bool rd)
            {
                var it = cache_font[i];
                rd = SetXY(it.x, it.y);
                return new Rectangle(it.x, it.y, Rect.Width, Rect.Height);
            }
            public bool SetXY(int x, int y)
            {
                if (Rect.X == x && Rect.Y == y && flag) return false;
                Rect.X = x;
                Rect.Y = y;
                flag = true;
                return true;
            }

            public int X
            {
                get => Rect.X;
                set
                {
                    if (Rect.X == value && flag) return;
                    Rect.X = value;
                    flag = true;
                    control.Invalidate();
                }
            }
            public int Y => Rect.Y;
            public int Width => Rect.Width;
            public int Height
            {
                get => Rect.Height;
                set => Rect.Height = value;
            }

            public bool ReadShow = false;

            bool show = false;
            public bool Show
            {
                get => show;
                set
                {
                    if (show == value) return;
                    show = value;
                    CaretPrint?.Dispose();
                    if (control.IsHandleCreated)
                    {
                        if (show)
                        {
                            flag = true;
                            if (ReadShow)
                            {
                                show = false;
                                return;
                            }
                            CaretPrint = new AnimationTask(control, () =>
                            {
                                Flag = !flag;
                                return show;
                            }, control.CaretSpeed, null, control.CaretSpeed);
                            if (control.SetCaretPostion()) control.Invalidate();
                        }
                        else CaretPrint = null;
                        control.Invalidate();
                    }
                }
            }

            internal bool flag = false;
            public bool Flag
            {
                get => flag;
                set
                {
                    if (flag == value) return;
                    flag = value;
                    if (show) control.Invalidate();
                }
            }

            AnimationTask? CaretPrint;

            public void Dispose() => CaretPrint?.Dispose();
        }

        #endregion

        #region 闪烁动画

        Color? colorBlink;
        AnimationTask? ThreadAnimateBlink;
        /// <summary>
        /// 闪烁动画状态
        /// </summary>
        public bool AnimationBlinkState = false;

        /// <summary>
        /// 开始闪烁动画
        /// </summary>
        /// <param name="interval">动画间隔时长（毫秒）</param>
        /// <param name="colors">色彩值</param>
        public void AnimationBlink(int interval, params Color[] colors)
        {
            ThreadAnimateBlink?.Dispose();
            if (colors.Length > 1)
            {
                AnimationBlinkState = true;
                int index = 0, len = colors.Length;
                ThreadAnimateBlink = new AnimationTask(this, () =>
                {
                    colorBlink = colors[index];
                    index++;
                    if (index > len - 1) index = 0;
                    Invalidate();
                    return AnimationBlinkState;
                }, interval, Invalidate);
            }
        }

        /// <summary>
        /// 开始闪烁动画（颜色过度动画）
        /// </summary>
        /// <param name="interval">动画间隔时长（毫秒）</param>
        /// <param name="colors">色彩值</param>
        public void AnimationBlinkTransition(int interval, params Color[] colors) => AnimationBlinkTransition(interval, 10, AnimationType.Liner, colors);

        /// <summary>
        /// 开始闪烁动画（颜色过度动画）
        /// </summary>
        /// <param name="interval">动画间隔时长（毫秒）</param>
        /// <param name="transition_interval">过度动画间隔时长（毫秒）</param>
        /// <param name="animationType">过度动画类型</param>
        /// <param name="colors">色彩值</param>
        public void AnimationBlinkTransition(int interval, int transition_interval, AnimationType animationType, params Color[] colors)
        {
            ThreadAnimateBlink?.Dispose();
            if (colors.Length > 1 && interval > transition_interval)
            {
                AnimationBlinkState = true;
                int index = 0, len = colors.Length;
                Color tmp = colors[index];
                index++;
                if (index > len - 1) index = 0;
                var t = Animation.TotalFrames(transition_interval, interval);
                ThreadAnimateBlink = new AnimationTask(this, () =>
                {
                    Color start = tmp, end = colors[index];
                    index++;
                    if (index > len - 1) index = 0;
                    tmp = end;
                    new AnimationTask(i =>
                    {
                        var prog = Animation.Animate(i, t, 1F, animationType);
                        colorBlink = start.BlendColors(Helper.ToColorN(prog, end));
                        Invalidate();
                        return AnimationBlinkState;
                    }, transition_interval, t, () =>
                    {
                        colorBlink = end;
                        Invalidate();
                    }).Wait();
                    return AnimationBlinkState;
                });
            }
        }

        public void StopAnimationBlink()
        {
            AnimationBlinkState = false;
            ThreadAnimateBlink?.Dispose();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ThreadFocus?.Dispose();
            ThreadHover?.Dispose();
            CaretInfo.Dispose();
            ThreadAnimateBlink?.Dispose();
            fix_cache_font.Dispose();
            base.Dispose(disposing);
        }
        AnimationTask? ThreadHover;
        AnimationTask? ThreadFocus;
    }
}