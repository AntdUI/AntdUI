// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace AntdUI
{
    partial class Markdown
    {
        #region 颜色

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
                OnPropertyChanged(nameof(Fore));
            }
        }

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(Back));
            }
        }

        Color? headingFore;
        /// <summary>
        /// 标题颜色
        /// </summary>
        [Description("标题颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? HeadingFore
        {
            get => headingFore;
            set
            {
                if (headingFore == value) return;
                headingFore = value;
                Invalidate();
                OnPropertyChanged(nameof(HeadingFore));
            }
        }

        Color? linkFore;
        /// <summary>
        /// 超链接颜色
        /// </summary>
        [Description("超链接颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LinkFore
        {
            get => linkFore;
            set
            {
                if (linkFore == value) return;
                linkFore = value;
                Invalidate();
                OnPropertyChanged(nameof(LinkFore));
            }
        }

        Color? codeFore;
        /// <summary>
        /// 代码文本颜色
        /// </summary>
        [Description("代码文本颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeFore
        {
            get => codeFore;
            set
            {
                if (codeFore == value) return;
                codeFore = value;
                Invalidate();
                OnPropertyChanged(nameof(CodeFore));
            }
        }

        Color? codeBack;
        /// <summary>
        /// 代码块背景颜色
        /// </summary>
        [Description("代码块背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeBack
        {
            get => codeBack;
            set
            {
                if (codeBack == value) return;
                codeBack = value;
                Invalidate();
                OnPropertyChanged(nameof(CodeBack));
            }
        }

        Color? inlineCodeFore;
        /// <summary>
        /// 行内代码文本颜色
        /// </summary>
        [Description("行内代码文本颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? InlineCodeFore
        {
            get => inlineCodeFore;
            set
            {
                if (inlineCodeFore == value) return;
                inlineCodeFore = value;
                Invalidate();
                OnPropertyChanged(nameof(InlineCodeFore));
            }
        }

        Color? inlineCodeBack;
        /// <summary>
        /// 行内代码背景颜色
        /// </summary>
        [Description("行内代码背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? InlineCodeBack
        {
            get => inlineCodeBack;
            set
            {
                if (inlineCodeBack == value) return;
                inlineCodeBack = value;
                Invalidate();
                OnPropertyChanged(nameof(InlineCodeBack));
            }
        }

        Color? quoteBar;
        /// <summary>
        /// 引用竖线颜色
        /// </summary>
        [Description("引用竖线颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? QuoteBar
        {
            get => quoteBar;
            set
            {
                if (quoteBar == value) return;
                quoteBar = value;
                Invalidate();
                OnPropertyChanged(nameof(QuoteBar));
            }
        }

        Color? quoteFore;
        /// <summary>
        /// 引用文本颜色
        /// </summary>
        [Description("引用文本颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? QuoteFore
        {
            get => quoteFore;
            set
            {
                if (quoteFore == value) return;
                quoteFore = value;
                Invalidate();
                OnPropertyChanged(nameof(QuoteFore));
            }
        }

        Color? ruleColor;
        /// <summary>
        /// 分隔线颜色
        /// </summary>
        [Description("分隔线颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? RuleColor
        {
            get => ruleColor;
            set
            {
                if (ruleColor == value) return;
                ruleColor = value;
                Invalidate();
                OnPropertyChanged(nameof(RuleColor));
            }
        }

        Color? tableBorder;
        /// <summary>
        /// 表格边框颜色
        /// </summary>
        [Description("表格边框颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TableBorder
        {
            get => tableBorder;
            set
            {
                if (tableBorder == value) return;
                tableBorder = value;
                Invalidate();
                OnPropertyChanged(nameof(TableBorder));
            }
        }

        Color? tableHeaderBack;
        /// <summary>
        /// 表格表头背景颜色
        /// </summary>
        [Description("表格表头背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TableHeaderBack
        {
            get => tableHeaderBack;
            set
            {
                if (tableHeaderBack == value) return;
                tableHeaderBack = value;
                Invalidate();
                OnPropertyChanged(nameof(TableHeaderBack));
            }
        }

        Color? tableStripe;
        /// <summary>
        /// 表格斑马纹颜色（rowIndex % 2 == 0 行背景）
        /// </summary>
        [Description("表格斑马纹颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TableStripe
        {
            get => tableStripe;
            set
            {
                if (tableStripe == value) return;
                tableStripe = value;
                Invalidate();
                OnPropertyChanged(nameof(TableStripe));
            }
        }

        Color? codeKeyword, codeString, codeComment, codeNumber, codeType;

        /// <summary>代码关键字颜色</summary>
        [Description("代码关键字颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeKeyword
        {
            get => codeKeyword;
            set { if (codeKeyword == value) return; codeKeyword = value; Invalidate(); OnPropertyChanged(nameof(CodeKeyword)); }
        }

        /// <summary>代码字符串颜色</summary>
        [Description("代码字符串颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeString
        {
            get => codeString;
            set { if (codeString == value) return; codeString = value; Invalidate(); OnPropertyChanged(nameof(CodeString)); }
        }

        /// <summary>代码注释颜色</summary>
        [Description("代码注释颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeComment
        {
            get => codeComment;
            set { if (codeComment == value) return; codeComment = value; Invalidate(); OnPropertyChanged(nameof(CodeComment)); }
        }

        /// <summary>代码数字颜色</summary>
        [Description("代码数字颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeNumber
        {
            get => codeNumber;
            set { if (codeNumber == value) return; codeNumber = value; Invalidate(); OnPropertyChanged(nameof(CodeNumber)); }
        }

        /// <summary>代码类型颜色</summary>
        [Description("代码类型颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeType
        {
            get => codeType;
            set { if (codeType == value) return; codeType = value; Invalidate(); OnPropertyChanged(nameof(CodeType)); }
        }

        #endregion

        #region 提示块配置

        string? alertNoteIcon = "InfoCircleOutlined";
        /// <summary>
        /// Note 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Note 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("InfoCircleOutlined")]
        public string? AlertNoteIcon
        {
            get => alertNoteIcon;
            set
            {
                if (alertNoteIcon == value) return;
                alertNoteIcon = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertNoteIcon));
            }
        }

        Color? alertNoteColor;
        /// <summary>
        /// Note 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Note 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertNoteColor
        {
            get => alertNoteColor;
            set
            {
                if (alertNoteColor == value) return;
                alertNoteColor = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertNoteColor));
            }
        }

        string? alertTipIcon = "BulbOutlined";
        /// <summary>
        /// Tip 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Tip 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("BulbOutlined")]
        public string? AlertTipIcon
        {
            get => alertTipIcon;
            set
            {
                if (alertTipIcon == value) return;
                alertTipIcon = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertTipIcon));
            }
        }

        Color? alertTipColor;
        /// <summary>
        /// Tip 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Tip 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertTipColor
        {
            get => alertTipColor;
            set
            {
                if (alertTipColor == value) return;
                alertTipColor = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertTipColor));
            }
        }

        string? alertImportantIcon = "InfoOutlined";
        /// <summary>
        /// Important 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Important 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("InfoOutlined")]
        public string? AlertImportantIcon
        {
            get => alertImportantIcon;
            set
            {
                if (alertImportantIcon == value) return;
                alertImportantIcon = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertImportantIcon));
            }
        }

        Color? alertImportantColor;
        /// <summary>
        /// Important 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Important 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertImportantColor
        {
            get => alertImportantColor;
            set
            {
                if (alertImportantColor == value) return;
                alertImportantColor = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertImportantColor));
            }
        }

        string? alertWarningIcon = "WarningOutlined";
        /// <summary>
        /// Warning 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Warning 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("WarningOutlined")]
        public string? AlertWarningIcon
        {
            get => alertWarningIcon;
            set
            {
                if (alertWarningIcon == value) return;
                alertWarningIcon = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertWarningIcon));
            }
        }

        Color? alertWarningColor;
        /// <summary>
        /// Warning 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Warning 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertWarningColor
        {
            get => alertWarningColor;
            set
            {
                if (alertWarningColor == value) return;
                alertWarningColor = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertWarningColor));
            }
        }

        string? alertCautionIcon = "StopOutlined";
        /// <summary>
        /// Caution 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Caution 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("StopOutlined")]
        public string? AlertCautionIcon
        {
            get => alertCautionIcon;
            set
            {
                if (alertCautionIcon == value) return;
                alertCautionIcon = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertCautionIcon));
            }
        }

        Color? alertCautionColor;
        /// <summary>
        /// Caution 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Caution 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertCautionColor
        {
            get => alertCautionColor;
            set
            {
                if (alertCautionColor == value) return;
                alertCautionColor = value;
                Invalidate();
                OnPropertyChanged(nameof(AlertCautionColor));
            }
        }

        #endregion

        #region 布局参数

        float blockSpacing = 0.62F;
        /// <summary>
        /// 块间距（相对行高倍数）
        /// </summary>
        [Description("块间距（相对行高倍数）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0.62F)]
        public float BlockSpacing
        {
            get => blockSpacing;
            set
            {
                if (blockSpacing == value) return;
                blockSpacing = value;
                LoadLayout();
                OnPropertyChanged(nameof(BlockSpacing));
            }
        }

        float codePadding = 0.62F;
        /// <summary>
        /// 代码块内边距（相对行高倍数）
        /// </summary>
        [Description("代码块内边距（相对行高倍数）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0.62F)]
        public float CodePadding
        {
            get => codePadding;
            set
            {
                if (codePadding == value) return;
                codePadding = value;
                LoadLayout();
                OnPropertyChanged(nameof(CodePadding));
            }
        }

        int quoteIndent = 16;
        /// <summary>
        /// 引用缩进
        /// </summary>
        [Description("引用缩进"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(16)]
        public int QuoteIndent
        {
            get => quoteIndent;
            set
            {
                if (quoteIndent == value) return;
                quoteIndent = value;
                LoadLayout();
                OnPropertyChanged(nameof(QuoteIndent));
            }
        }

        int listIndent = 22;
        /// <summary>
        /// 列表层级缩进
        /// </summary>
        [Description("列表层级缩进"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(22)]
        public int ListIndent
        {
            get => listIndent;
            set
            {
                if (listIndent == value) return;
                listIndent = value;
                LoadLayout();
                OnPropertyChanged(nameof(ListIndent));
            }
        }

        int tableCellPadding = 0;
        /// <summary>
        /// 表格单元格左右内边距（0 = 自动按字体占比，对应 GitHub padding: 6px 13px）
        /// </summary>
        [Description("表格单元格左右内边距（0 = 自动按字体占比 6px/13px）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0)]
        public int TableCellPadding
        {
            get => tableCellPadding;
            set
            {
                if (tableCellPadding == value) return;
                tableCellPadding = value;
                LoadLayout();
                OnPropertyChanged(nameof(TableCellPadding));
            }
        }

        int radius = 6;
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

        #endregion

        #region 颜色解析（GitHub 风格）

        /// <summary>
        /// 是否深色模式
        /// </summary>
        bool IsDark => ColorScheme == TAMode.Dark || (ColorScheme == TAMode.Auto && Config.IsDark);

        internal Color ColorFore => fore ?? (IsDark ? Color.FromArgb(230, 237, 243) : Color.FromArgb(31, 35, 40));
        internal Color ColorHeading => headingFore ?? (IsDark ? Color.FromArgb(230, 237, 243) : Color.FromArgb(31, 35, 40));
        internal Color ColorLink => linkFore ?? (IsDark ? Color.FromArgb(47, 129, 247) : Color.FromArgb(9, 105, 218));
        internal Color ColorCodeFore => codeFore ?? (IsDark ? Color.FromArgb(230, 237, 243) : Color.FromArgb(31, 35, 40));
        internal Color ColorCodeBack => codeBack ?? (IsDark ? Color.FromArgb(22, 27, 34) : Color.FromArgb(246, 248, 250));
        internal Color ColorInlineCodeFore => inlineCodeFore ?? (IsDark ? Color.FromArgb(230, 237, 243) : Color.FromArgb(31, 35, 40));
        /// <summary>行内代码背景（GitHub 浅色 #818b981f = rgba(129,139,152,.12)；深色 rgba(110,118,129,.4)）</summary>
        internal Color ColorInlineCodeBack => inlineCodeBack ?? (IsDark ? Color.FromArgb(102, 110, 118, 129) : Color.FromArgb(31, 129, 139, 152));
        internal Color ColorQuoteFore => quoteFore ?? (IsDark ? Color.FromArgb(139, 148, 158) : Color.FromArgb(89, 99, 110));
        internal Color ColorQuoteBar => quoteBar ?? (IsDark ? Color.FromArgb(61, 68, 77) : Color.FromArgb(208, 215, 222));
        internal Color ColorRule => ruleColor ?? (IsDark ? Color.FromArgb(33, 38, 45) : Color.FromArgb(208, 215, 222));
        internal Color ColorTableBorder => tableBorder ?? Style.Db.BorderColor;
        /// <summary>表头背景（GitHub 风格默认白底透明，可显式设置）</summary>
        internal Color ColorTableHeaderBack => tableHeaderBack ?? Style.Db.BgBase;
        /// <summary>表格斑马纹背景（rowIndex % 2 == 0 行，GitHub #f6f8fa / 深色 #151b23）</summary>
        internal Color ColorTableStripe => tableStripe ?? (IsDark ? Color.FromArgb(21, 27, 35) : Color.FromArgb(246, 248, 250));
        /// <summary>标题下边框（GitHub h1/h2）</summary>
        internal Color ColorHeadingBorder => IsDark ? Color.FromArgb(33, 38, 45) : Color.FromArgb(216, 222, 228);

        internal Color ColorToken(MDToken token)
        {
            bool dark = IsDark;
            switch (token)
            {
                case MDToken.Keyword: return codeKeyword ?? (dark ? Color.FromArgb(255, 123, 114) : Color.FromArgb(207, 34, 46));
                case MDToken.String: return codeString ?? (dark ? Color.FromArgb(165, 214, 255) : Color.FromArgb(10, 48, 105));
                case MDToken.Comment: return codeComment ?? (dark ? Color.FromArgb(139, 148, 158) : Color.FromArgb(89, 99, 110));
                case MDToken.Number: return codeNumber ?? (dark ? Color.FromArgb(121, 192, 255) : Color.FromArgb(5, 80, 174));
                case MDToken.Type: return codeType ?? (dark ? Color.FromArgb(255, 166, 87) : Color.FromArgb(149, 56, 0));
                case MDToken.Add: return ColorDiffFore(MDToken.Add);
                case MDToken.Del: return ColorDiffFore(MDToken.Del);
                case MDToken.Meta: return ColorDiffFore(MDToken.Meta);
                default: return ColorCodeFore;
            }
        }

        internal Color ColorDiffFore(MDToken token)
        {
            if (IsDark)
            {
                switch (token)
                {
                    case MDToken.Add: return Color.FromArgb(175, 245, 180);
                    case MDToken.Del: return Color.FromArgb(255, 220, 215);
                    case MDToken.Meta: return Color.FromArgb(139, 148, 158);
                }
            }
            else
            {
                switch (token)
                {
                    case MDToken.Add: return Color.FromArgb(17, 99, 41);
                    case MDToken.Del: return Color.FromArgb(130, 7, 30);
                    case MDToken.Meta: return Color.FromArgb(87, 96, 106);
                }
            }
            return ColorCodeFore;
        }

        /// <summary>
        /// Diff 行背景（GitHub 风格，透明表示无背景）
        /// </summary>
        internal Color ColorDiffBack(MDToken token)
        {
            if (IsDark)
            {
                switch (token)
                {
                    case MDToken.Add: return Color.FromArgb(14, 68, 41);
                    case MDToken.Del: return Color.FromArgb(103, 6, 12);
                    case MDToken.Meta: return Color.FromArgb(22, 27, 34);
                }
            }
            else
            {
                switch (token)
                {
                    case MDToken.Add: return Color.FromArgb(230, 255, 236);
                    case MDToken.Del: return Color.FromArgb(255, 235, 233);
                    case MDToken.Meta: return Color.FromArgb(234, 238, 242);
                }
            }
            return Color.Transparent;
        }

        /// <summary>
        /// 提示块颜色（优先用户配置，否则 GitHub 主题默认色卡）
        /// </summary>
        internal Color ColorAlert(MDAlert alert)
        {
            switch (alert)
            {
                case MDAlert.Note:
                    if (alertNoteColor.HasValue) return alertNoteColor.Value;
                    return IsDark ? Color.FromArgb(47, 129, 247) : Color.FromArgb(9, 105, 218);
                case MDAlert.Tip:
                    if (alertTipColor.HasValue) return alertTipColor.Value;
                    return IsDark ? Color.FromArgb(63, 185, 80) : Color.FromArgb(26, 127, 55);
                case MDAlert.Important:
                    if (alertImportantColor.HasValue) return alertImportantColor.Value;
                    return IsDark ? Color.FromArgb(163, 113, 247) : Color.FromArgb(130, 80, 223);
                case MDAlert.Warning:
                    if (alertWarningColor.HasValue) return alertWarningColor.Value;
                    return IsDark ? Color.FromArgb(210, 153, 34) : Color.FromArgb(154, 103, 0);
                case MDAlert.Caution:
                    if (alertCautionColor.HasValue) return alertCautionColor.Value;
                    return IsDark ? Color.FromArgb(248, 81, 73) : Color.FromArgb(207, 34, 46);
            }
            return ColorQuoteBar;
        }

        /// <summary>
        /// 提示块图标（图标名称或 SVG 代码，由 Canvas.Svg 解析渲染）
        /// </summary>
        internal string? AlertIcon(MDAlert alert)
        {
            switch (alert)
            {
                case MDAlert.Note: return alertNoteIcon;
                case MDAlert.Tip: return alertTipIcon;
                case MDAlert.Important: return alertImportantIcon;
                case MDAlert.Warning: return alertWarningIcon;
                case MDAlert.Caution: return alertCautionIcon;
            }
            return null;
        }

        /// <summary>
        /// 选中文本的背景色
        /// </summary>
        internal Color ColorSelection
        {
            get
            {
                if (selectionColor.HasValue) return selectionColor.Value;
                var primary = Colour.Primary.Get(ColorScheme, nameof(Markdown), Name);
                return Color.FromArgb(IsDark ? 120 : 102, primary);
            }
        }

        #endregion

        #region 字体缓存

        Font? fontBold, fontItalic, fontBoldItalic, fontCode, fontCodeBold, fontSub, fontSup, fontH1, fontH2, fontH3, fontH4, fontH5, fontH6;
        bool font_ok;

        void EnsureFont()
        {
            if (font_ok) return;
            font_ok = true;
            var family = Font.FontFamily;
            float size = Font.Size;
            fontBold = new Font(family, size, FontStyle.Bold);
            fontItalic = new Font(family, size, FontStyle.Italic);
            fontBoldItalic = new Font(family, size, FontStyle.Bold | FontStyle.Italic);
            fontSub = new Font(family, size * 0.75F);
            fontSup = new Font(family, size * 0.75F);
            fontH1 = new Font(family, size * 2F, FontStyle.Bold);
            fontH2 = new Font(family, size * 1.5F, FontStyle.Bold);
            fontH3 = new Font(family, size * 1.25F, FontStyle.Bold);
            fontH4 = new Font(family, size * 1.1F, FontStyle.Bold);
            fontH5 = new Font(family, size, FontStyle.Bold);
            fontH6 = new Font(family, size * 0.92F, FontStyle.Bold);
            if (codeFont != null)
            {
                fontCode = (Font)codeFont.Clone();
                fontCodeBold = new Font(codeFont, codeFont.Style | FontStyle.Bold);
            }
            else
            {
                fontCode = new Font(FontFamily.GenericMonospace, size * 0.85F);
                fontCodeBold = new Font(FontFamily.GenericMonospace, size * 0.85F, FontStyle.Bold);
            }
        }

        void DisposeFont()
        {
            font_ok = false;
            fontBold?.Dispose();
            fontItalic?.Dispose();
            fontBoldItalic?.Dispose();
            fontCode?.Dispose();
            fontCodeBold?.Dispose();
            fontSub?.Dispose();
            fontSup?.Dispose();
            fontH1?.Dispose();
            fontH2?.Dispose();
            fontH3?.Dispose();
            fontH4?.Dispose();
            fontH5?.Dispose();
            fontH6?.Dispose();
            fontBold = fontItalic = fontBoldItalic = fontCode = fontCodeBold = fontSub = fontSup = fontH1 = fontH2 = fontH3 = fontH4 = fontH5 = fontH6 = null;
        }

        internal Font FontBold { get { EnsureFont(); return fontBold!; } }
        internal Font FontItalic { get { EnsureFont(); return fontItalic!; } }
        internal Font FontBoldItalic { get { EnsureFont(); return fontBoldItalic!; } }
        internal Font FontCode { get { EnsureFont(); return fontCode!; } }
        /// <summary>行内代码粗体（表头 th / 标题内 code 继承 font-weight: 600）</summary>
        internal Font FontCodeBold { get { EnsureFont(); return fontCodeBold!; } }
        internal Font FontSub { get { EnsureFont(); return fontSub!; } }
        internal Font FontSup { get { EnsureFont(); return fontSup!; } }

        internal Font FontHeading(MDBlockKind kind)
        {
            EnsureFont();
            switch (kind)
            {
                case MDBlockKind.Heading1: return fontH1!;
                case MDBlockKind.Heading2: return fontH2!;
                case MDBlockKind.Heading3: return fontH3!;
                case MDBlockKind.Heading4: return fontH4!;
                case MDBlockKind.Heading5: return fontH5!;
                default: return fontH6!;
            }
        }

        #endregion
    }
}