// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// Markdown 块级元素类型
    /// </summary>
    internal enum MDBlockKind
    {
        /// <summary>空行</summary>
        Blank,
        /// <summary>段落</summary>
        Paragraph,
        Heading1,
        Heading2,
        Heading3,
        Heading4,
        Heading5,
        Heading6,
        /// <summary>代码块</summary>
        Code,
        /// <summary>引用</summary>
        Quote,
        /// <summary>无序列表</summary>
        ListUnordered,
        /// <summary>有序列表</summary>
        ListOrdered,
        /// <summary>分隔线</summary>
        Rule,
        /// <summary>表格</summary>
        Table,
    }

    /// <summary>
    /// Markdown 内联元素类型
    /// </summary>
    internal enum MDInlineKind
    {
        /// <summary>普通文本</summary>
        Text,
        /// <summary>粗体</summary>
        Bold,
        /// <summary>斜体</summary>
        Italic,
        /// <summary>粗斜体</summary>
        BoldItalic,
        /// <summary>行内代码</summary>
        Code,
        /// <summary>超链接</summary>
        Link,
        /// <summary>图片</summary>
        Image,
        /// <summary>删除线</summary>
        Strike,
        /// <summary>下标 sub</summary>
        Sub,
        /// <summary>上标 sup</summary>
        Sup,
        /// <summary>换行</summary>
        Break,
    }

    /// <summary>
    /// 水平对齐
    /// </summary>
    internal enum MDAlign
    {
        None,
        Left,
        Center,
        Right,
    }

    /// <summary>
    /// GitHub 风格提示块
    /// </summary>
    internal enum MDAlert
    {
        None,
        Note,
        Tip,
        Important,
        Warning,
        Caution,
    }

    /// <summary>
    /// 代码高亮标记
    /// </summary>
    internal enum MDToken
    {
        Plain,
        Keyword,
        String,
        Comment,
        Number,
        Type,
        /// <summary>Diff 新增</summary>
        Add,
        /// <summary>Diff 删除</summary>
        Del,
        /// <summary>Diff 元信息</summary>
        Meta,
    }

    /// <summary>
    /// 内联元素
    /// </summary>
    internal sealed class MDInline
    {
        public MDInlineKind Kind;
        public string Text = string.Empty;
        /// <summary>链接地址 / 图片地址</summary>
        public string? Url;
        /// <summary>图片被超链接包裹时的链接地址</summary>
        public string? Href;
        /// <summary>图片指定宽度（0 自动）</summary>
        public int Width;
        /// <summary>图片指定高度（0 自动）</summary>
        public int Height;
        /// <summary>该内联元素对应的 Markdown 原文（用于复制时保留格式）</summary>
        public string? Md;

        public MDInline(MDInlineKind kind, string text, string? url = null)
        {
            Kind = kind;
            Text = text;
            Url = url;
        }
    }

    /// <summary>
    /// 表格单元格
    /// </summary>
    internal sealed class MDCell
    {
        public List<MDInline> Inlines = new List<MDInline>(1);
    }

    /// <summary>
    /// 块级元素
    /// </summary>
    internal sealed class MDBlock
    {
        public MDBlockKind Kind;
        public string? Raw;
        /// <summary>代码语言</summary>
        public string? Lang;
        /// <summary>标题锚点（GitHub 风格 slug，供 #锚点 跳转使用）</summary>
        public string? Anchor;
        /// <summary>代码块是否已闭合（流式输出中可能为 false）</summary>
        public bool Closed = true;
        public List<MDInline> Inlines = new List<MDInline>(1);
        /// <summary>子块（块引用内部递归解析的标题 / 列表 / 嵌套引用等）</summary>
        public List<MDBlock>? Children;

        /// <summary>有序列表序号</summary>
        public int Order;
        /// <summary>列表层级（从 0 开始）</summary>
        public int Level;

        /// <summary>HTML 容器指定的对齐方式</summary>
        public MDAlign Align;
        public MDAlert Alert;
        /// <summary>是否为提示块标题行</summary>
        public bool AlertHeader;

        #region 表格

        public List<List<MDCell>>? Rows;
        public MDAlign[]? ColAlign;
        /// <summary>列宽百分比（0 自动）</summary>
        public float[]? ColWidth;
        public bool HasHeader;

        #endregion

        /// <summary>内容指纹，用于流式增量布局</summary>
        public int Hash;

        public MDBlock(MDBlockKind kind, string? raw = null)
        {
            Kind = kind;
            Raw = raw;
        }
    }

    /// <summary>
    /// 解析后的文档
    /// </summary>
    internal sealed class MDDocument
    {
        public List<MDBlock> Blocks = new List<MDBlock>();

        /// <summary>
        /// 计算全部块的内容指纹
        /// </summary>
        public void BuildHash()
        {
            foreach (var block in Blocks) block.Hash = MDBlockHasher.Hash(block);
        }
    }

    internal static class MDBlockHasher
    {
        public static int Hash(MDBlock block)
        {
            uint h = 2166136261;
            h = Add(h, (int)block.Kind);
            h = Add(h, (int)block.Align);
            h = Add(h, (int)block.Alert);
            h = Add(h, block.Level);
            h = Add(h, block.Order);
            h = Add(h, block.Closed ? 1 : 0);
            if (block.Raw != null) h = AddString(h, block.Raw);
            if (block.Lang != null) h = AddString(h, block.Lang);
            if (block.Children != null)
            {
                foreach (var child in block.Children) h = Add(h, child == null ? 0 : Hash(child));
            }
            if (block.Rows != null)
            {
                foreach (var row in block.Rows)
                {
                    if (row == null) continue;
                    foreach (var cell in row)
                    {
                        if (cell == null) continue;
                        foreach (var inline in cell.Inlines) h = AddInline(h, inline);
                    }
                }
            }
            else
            {
                foreach (var inline in block.Inlines) h = AddInline(h, inline);
            }
            return (int)h;
        }

        static uint AddInline(uint h, MDInline inline)
        {
            h = Add(h, (int)inline.Kind);
            h = Add(h, inline.Width);
            h = Add(h, inline.Height);
            h = AddString(h, inline.Text);
            if (inline.Url != null) h = AddString(h, inline.Url);
            if (inline.Href != null) h = AddString(h, inline.Href);
            return h;
        }

        static uint Add(uint h, int value)
        {
            h ^= (uint)value;
            h *= 16777619;
            return h;
        }

        static uint AddString(uint h, string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                h ^= value[i];
                h *= 16777619;
            }
            return h;
        }
    }

    /// <summary>
    /// 布局中间结构：一段同样式文本
    /// </summary>
    internal sealed class MDRun
    {
        public string Text = string.Empty;
        public Font? Font;
        public Color Fore;
        public MDInlineKind Kind;
        public string? Url;
        public string? Src;
        public bool Strike;
        /// <summary>对应的 Markdown 原文</summary>
        public string? Md;
        /// <summary>图片显示宽度</summary>
        public int Width;
        /// <summary>图片显示高度</summary>
        public int Height;
    }

    /// <summary>
    /// 可绘制片段
    /// </summary>
    internal sealed class MDFragment
    {
        public string Text = string.Empty;
        public Font? Font;
        public Color Fore;
        /// <summary>内容坐标</summary>
        public int X, Y, Width, Height;
        public MDInlineKind Kind;
        /// <summary>链接地址</summary>
        public string? Url;
        /// <summary>图片地址</summary>
        public string? Src;
        /// <summary>对应的 Markdown 原文</summary>
        public string? Md;
        /// <summary>表格列号（-1 非表格）</summary>
        public int Cell = -1;
        public bool Strike;

        public Rectangle Rect => new Rectangle(X, Y, Width, Height);

        public bool Hit(int x, int y) => x >= X && x < X + Width && y >= Y && y < Y + Height;
    }

    /// <summary>
    /// 视觉行（自动换行后的单个显示行）
    /// </summary>
    internal sealed class MDVisualLine
    {
        public int Block;
        public int Y;
        public int Height;
        public List<MDFragment> Fragments = new List<MDFragment>(1);
        public MDAlign Align;
        /// <summary>表格行索引（-1 非表格）</summary>
        public int Row = -1;
        /// <summary>该行对应的 Markdown 原文（代码块整行复制）</summary>
        public string? Raw;
        /// <summary>行内容宽度（用于对齐）</summary>
        public int Right = -1;

        public int Bottom => Y + Height;
    }

    /// <summary>
    /// 块装饰（背景 / 边框 / 线条）
    /// </summary>
    internal sealed class MDPaint
    {
        public Rectangle Rect;
        public Color Back;
        public float Radius;
        /// <summary>圆角方向（仅 Radius &gt; 0 时生效）</summary>
        public TAlignRound Corners = TAlignRound.ALL;
        /// <summary>边框颜色（A &gt; 0 时沿 Radius 圆角描边）</summary>
        public Color Border;
        /// <summary>边框宽度</summary>
        public float BorderWidth;
        /// <summary>图标（Ant Design 图标名称或 SVG 代码，非空时绘制图标，Back 为颜色）</summary>
        public string? Svg;
    }
}