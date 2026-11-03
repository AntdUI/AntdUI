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
    /// <summary>
    /// 样式属性（转发到排版引擎 <see cref="MarkdownRenderer"/>，聊天气泡复用同一套样式）
    /// </summary>
    partial class Markdown
    {
        #region 颜色

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fore
        {
            get => renderer.Fore;
            set => renderer.Fore = value;
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => renderer.Back;
            set => renderer.Back = value;
        }

        /// <summary>
        /// 标题颜色
        /// </summary>
        [Description("标题颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? HeadingFore
        {
            get => renderer.HeadingFore;
            set => renderer.HeadingFore = value;
        }

        /// <summary>
        /// 超链接颜色
        /// </summary>
        [Description("超链接颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LinkFore
        {
            get => renderer.LinkFore;
            set => renderer.LinkFore = value;
        }

        /// <summary>
        /// 代码文本颜色
        /// </summary>
        [Description("代码文本颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeFore
        {
            get => renderer.CodeFore;
            set => renderer.CodeFore = value;
        }

        /// <summary>
        /// 代码块背景颜色
        /// </summary>
        [Description("代码块背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeBack
        {
            get => renderer.CodeBack;
            set => renderer.CodeBack = value;
        }

        /// <summary>
        /// 行内代码文本颜色
        /// </summary>
        [Description("行内代码文本颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? InlineCodeFore
        {
            get => renderer.InlineCodeFore;
            set => renderer.InlineCodeFore = value;
        }

        /// <summary>
        /// 行内代码背景颜色
        /// </summary>
        [Description("行内代码背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? InlineCodeBack
        {
            get => renderer.InlineCodeBack;
            set => renderer.InlineCodeBack = value;
        }

        /// <summary>
        /// 引用竖线颜色
        /// </summary>
        [Description("引用竖线颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? QuoteBar
        {
            get => renderer.QuoteBar;
            set => renderer.QuoteBar = value;
        }

        /// <summary>
        /// 引用文本颜色
        /// </summary>
        [Description("引用文本颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? QuoteFore
        {
            get => renderer.QuoteFore;
            set => renderer.QuoteFore = value;
        }

        /// <summary>
        /// 分隔线颜色
        /// </summary>
        [Description("分隔线颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? RuleColor
        {
            get => renderer.RuleColor;
            set => renderer.RuleColor = value;
        }

        /// <summary>
        /// 表格边框颜色
        /// </summary>
        [Description("表格边框颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TableBorder
        {
            get => renderer.TableBorder;
            set => renderer.TableBorder = value;
        }

        /// <summary>
        /// 表格表头背景颜色
        /// </summary>
        [Description("表格表头背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TableHeaderBack
        {
            get => renderer.TableHeaderBack;
            set => renderer.TableHeaderBack = value;
        }

        /// <summary>
        /// 表格斑马纹颜色（rowIndex % 2 == 0 行背景）
        /// </summary>
        [Description("表格斑马纹颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TableStripe
        {
            get => renderer.TableStripe;
            set => renderer.TableStripe = value;
        }

        /// <summary>代码关键字颜色</summary>
        [Description("代码关键字颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeKeyword
        {
            get => renderer.CodeKeyword;
            set => renderer.CodeKeyword = value;
        }

        /// <summary>代码字符串颜色</summary>
        [Description("代码字符串颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeString
        {
            get => renderer.CodeString;
            set => renderer.CodeString = value;
        }

        /// <summary>代码注释颜色</summary>
        [Description("代码注释颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeComment
        {
            get => renderer.CodeComment;
            set => renderer.CodeComment = value;
        }

        /// <summary>代码数字颜色</summary>
        [Description("代码数字颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeNumber
        {
            get => renderer.CodeNumber;
            set => renderer.CodeNumber = value;
        }

        /// <summary>代码类型颜色</summary>
        [Description("代码类型颜色"), Category("高亮"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CodeType
        {
            get => renderer.CodeType;
            set => renderer.CodeType = value;
        }

        #endregion

        #region 提示块配置

        /// <summary>
        /// Note 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Note 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("InfoCircleOutlined")]
        public string? AlertNoteIcon
        {
            get => renderer.AlertNoteIcon;
            set => renderer.AlertNoteIcon = value;
        }

        /// <summary>
        /// Note 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Note 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertNoteColor
        {
            get => renderer.AlertNoteColor;
            set => renderer.AlertNoteColor = value;
        }

        /// <summary>
        /// Tip 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Tip 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("BulbOutlined")]
        public string? AlertTipIcon
        {
            get => renderer.AlertTipIcon;
            set => renderer.AlertTipIcon = value;
        }

        /// <summary>
        /// Tip 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Tip 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertTipColor
        {
            get => renderer.AlertTipColor;
            set => renderer.AlertTipColor = value;
        }

        /// <summary>
        /// Important 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Important 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("InfoOutlined")]
        public string? AlertImportantIcon
        {
            get => renderer.AlertImportantIcon;
            set => renderer.AlertImportantIcon = value;
        }

        /// <summary>
        /// Important 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Important 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertImportantColor
        {
            get => renderer.AlertImportantColor;
            set => renderer.AlertImportantColor = value;
        }

        /// <summary>
        /// Warning 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Warning 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("WarningOutlined")]
        public string? AlertWarningIcon
        {
            get => renderer.AlertWarningIcon;
            set => renderer.AlertWarningIcon = value;
        }

        /// <summary>
        /// Warning 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Warning 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertWarningColor
        {
            get => renderer.AlertWarningColor;
            set => renderer.AlertWarningColor = value;
        }

        /// <summary>
        /// Caution 提示块图标（Ant Design 图标名称或 SVG 代码）
        /// </summary>
        [Description("Caution 提示块图标（图标名称或 SVG 代码）"), Category("提示块"), DefaultValue("StopOutlined")]
        public string? AlertCautionIcon
        {
            get => renderer.AlertCautionIcon;
            set => renderer.AlertCautionIcon = value;
        }

        /// <summary>
        /// Caution 提示块颜色（标题 / 图标 / 竖线）
        /// </summary>
        [Description("Caution 提示块颜色"), Category("提示块"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? AlertCautionColor
        {
            get => renderer.AlertCautionColor;
            set => renderer.AlertCautionColor = value;
        }

        #endregion

        #region 布局参数

        /// <summary>
        /// 块间距（相对行高倍数）
        /// </summary>
        [Description("块间距（相对行高倍数）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0.62F)]
        public float BlockSpacing
        {
            get => renderer.BlockSpacing;
            set => renderer.BlockSpacing = value;
        }

        /// <summary>
        /// 代码块内边距（相对行高倍数）
        /// </summary>
        [Description("代码块内边距（相对行高倍数）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0.62F)]
        public float CodePadding
        {
            get => renderer.CodePadding;
            set => renderer.CodePadding = value;
        }

        /// <summary>
        /// 引用缩进
        /// </summary>
        [Description("引用缩进"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(16)]
        public int QuoteIndent
        {
            get => renderer.QuoteIndent;
            set => renderer.QuoteIndent = value;
        }

        /// <summary>
        /// 列表层级缩进
        /// </summary>
        [Description("列表层级缩进"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(22)]
        public int ListIndent
        {
            get => renderer.ListIndent;
            set => renderer.ListIndent = value;
        }

        /// <summary>
        /// 表格单元格左右内边距（0 = 自动按字体占比，对应 GitHub padding: 6px 13px）
        /// </summary>
        [Description("表格单元格左右内边距（0 = 自动按字体占比 6px/13px）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0)]
        public int TableCellPadding
        {
            get => renderer.TableCellPadding;
            set => renderer.TableCellPadding = value;
        }

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(6)]
        public int Radius
        {
            get => renderer.Radius;
            set => renderer.Radius = value;
        }

        /// <summary>
        /// 选中区域背景颜色
        /// </summary>
        [Description("选中区域背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? SelectionColor
        {
            get => renderer.SelectionColor;
            set => renderer.SelectionColor = value;
        }

        /// <summary>
        /// 是否允许鼠标选中内容并复制
        /// </summary>
        [Description("是否允许鼠标选中内容并复制"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool Selectable
        {
            get => renderer.Selectable;
            set => renderer.Selectable = value;
        }

        /// <summary>
        /// 代码字体
        /// </summary>
        [Description("代码字体"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Font? CodeFont
        {
            get => renderer.CodeFont;
            set => renderer.CodeFont = value;
        }

        #endregion
    }
}
