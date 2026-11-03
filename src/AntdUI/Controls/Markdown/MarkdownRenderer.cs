// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// Markdown 排版引擎（与窗口解耦）
    /// </summary>
    /// <remarks>
    /// 解析 / 排版 / 绘制 / 选区全部基于内容坐标（左上角为原点，不含滚动），
    /// <see cref="Markdown"/> 控件与聊天列表的 Markdown 消息气泡共用同一套实现：
    /// 宿主只需提供可用宽度、Graphics 与重绘回调，即可获得与控件完全一致的渲染效果。
    /// </remarks>
    internal sealed partial class MarkdownRenderer
    {
        internal MarkdownRenderer(Font font, float dpi, TAMode scheme, string name)
        {
            Font = font;
            Dpi = dpi;
            ColorScheme = scheme;
            Name = name;
        }

        #region 宿主回调

        /// <summary>视觉变化，需要宿主重绘</summary>
        internal Action? Changed;
        /// <summary>内容/样式变化，需要宿主重新排版</summary>
        internal Action? Relayout;
        /// <summary>样式属性变化（用于宿主转发属性通知）</summary>
        internal Action<string?>? PropertyChanged;
        /// <summary>需要外部加载远程图片</summary>
        internal Action<string, string?>? ImageRequest;

        void Invalidate() => Changed?.Invoke();
        void LoadLayout(bool full = false) => Relayout?.Invoke();
        void OnPropertyChanged(string? name) => PropertyChanged?.Invoke(name);

        #endregion

        #region 宿主环境

        /// <summary>颜色方案键（与控件类型名一致，供 Colour.Get 按控件定制）</summary>
        internal const string ColorKey = "Markdown";

        /// <summary>缩放（由宿主提供）</summary>
        internal float Dpi;
        /// <summary>基准字体（由宿主提供）</summary>
        internal Font Font;
        /// <summary>色彩模式（由宿主提供）</summary>
        internal TAMode ColorScheme;
        /// <summary>控件名称（用于按控件定制颜色）</summary>
        internal string Name;
        /// <summary>相对路径基路径</summary>
        internal string? BasePath;
        /// <summary>超链接始终显示下划线</summary>
        internal bool LinkUnderline;

        /// <summary>
        /// 同步宿主环境（缩放 / 主题 / 名称），不影响排版
        /// </summary>
        internal void SyncEnv(float dpi, TAMode scheme, string name)
        {
            Dpi = dpi;
            ColorScheme = scheme;
            Name = name;
        }

        /// <summary>
        /// 同步宿主环境（含字体），字体变化会释放旧字体缓存
        /// </summary>
        /// <returns>字体是否发生变化（变化后宿主必须重新排版，否则片段仍引用已释放字体）</returns>
        internal bool SetHost(Font font, float dpi, TAMode scheme, string name)
        {
            SyncEnv(dpi, scheme, name);
            if (ReferenceEquals(Font, font)) return false;
            Font = font;
            DisposeFont();
            InvalidateMeasure();
            return true;
        }

        #endregion

        #region 文档

        readonly MarkdownParser parser = new MarkdownParser();

        /// <summary>
        /// 解析文本（不排版）
        /// </summary>
        internal void Parse(string? text)
        {
            try { doc = parser.Parse(text ?? ""); }
            catch { doc = new MDDocument(); }
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        internal void Clear()
        {
            doc = null;
            docOld = null;
            lines.Clear();
            paints.Clear();
            blockLine = new int[0];
            blockPaint = new int[0];
            anchorPos.Clear();
            contentHeight = 0;
            lastWidth = 0;
            ClearSelection();
        }

        /// <summary>
        /// 排版到指定宽度（内容坐标，宽度为可用内容宽度）
        /// </summary>
        internal void Layout(Canvas g, int width, bool full = true)
        {
            if (layouting) return;
            var document = doc;
            if (document == null) return;
            if (width <= 0) return;
            // 宽度变化后旧行坐标全部失效，必须完整重建
            if (width != lastWidth)
            {
                lastWidth = width;
                full = true;
            }
            layouting = true;
            try
            {
                EnsureFont();
                LayoutBlocks(g, document, width, full ? 0 : FirstChangedBlock());
                docOld = document;
            }
            finally { layouting = false; }
        }

        /// <summary>
        /// 内容高度
        /// </summary>
        internal int ContentHeight => contentHeight;

        /// <summary>
        /// 是否已有排版内容
        /// </summary>
        internal bool HasContent => lines.Count > 0;

        /// <summary>
        /// 当前文档
        /// </summary>
        internal MDDocument? Document => doc;

        /// <summary>
        /// 内容实际宽度（最右侧片段右边界），用于气泡收缩
        /// </summary>
        internal int ContentWidth
        {
            get
            {
                int max = 0;
                foreach (var line in lines)
                {
                    foreach (var frag in line.Fragments)
                    {
                        if (frag.X + frag.Width > max) max = frag.X + frag.Width;
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// 锚点定位（内容坐标 Y）
        /// </summary>
        internal bool TryGetAnchor(string key, out int y) => anchorPos.TryGetValue(key, out y);

        /// <summary>
        /// 文档锚点列表
        /// </summary>
        internal List<MarkdownAnchor> Anchors()
        {
            var result = new List<MarkdownAnchor>();
            CollectAnchors(doc?.Blocks, result);
            return result;
        }

        void CollectAnchors(List<MDBlock>? blocks, List<MarkdownAnchor> result)
        {
            if (blocks == null) return;
            foreach (var block in blocks)
            {
                if (block.Anchor != null && block.Kind >= MDBlockKind.Heading1 && block.Kind <= MDBlockKind.Heading6)
                {
                    int y;
                    if (!anchorPos.TryGetValue(block.Anchor, out y)) y = 0;
                    result.Add(new MarkdownAnchor(block.Anchor, block.Raw ?? "", (int)block.Kind - (int)MDBlockKind.Heading1 + 1, y));
                }
                if (block.Children != null) CollectAnchors(block.Children, result);
            }
        }

        #endregion

        #region 绘制

        /// <summary>
        /// 绘制到指定区域
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="rect">内容区域（内容坐标原点映射到该矩形左上角）</param>
        /// <param name="sy">滚动偏移</param>
        /// <param name="viewH">可视高度</param>
        internal void Draw(Canvas g, Rectangle rect, int sy, int viewH)
        {
            if (rect.Width <= 0 || viewH <= 0 || lines.Count == 0 && paints.Count == 0) return;
            var brushes = new Dictionary<Color, SolidBrush>(8);
            try
            {
                var state = g.Save();
                g.TranslateTransform(rect.X, rect.Y - sy);
                DrawPaints(g, brushes, sy, viewH, paints.ToArray());
                DrawSelection(g, sy, viewH);
                DrawLines(g, brushes, sy, viewH);
                g.Restore(state);
            }
            finally
            {
                foreach (var brush in brushes.Values) brush.Dispose();
            }
        }

        /// <summary>
        /// 悬停的链接地址（为空表示无悬停）
        /// </summary>
        internal string? HoverUrl
        {
            get => hoverUrl;
            set
            {
                if (hoverUrl == value) return;
                hoverUrl = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 命中的片段（内容坐标）
        /// </summary>
        internal MDFragment? HitTest(int x, int y) => HitTestFragment(x, y);

        #endregion

        #region 链接解析

        /// <summary>
        /// 解析链接（类型 / 解析后地址 / 锚点 / 目标文件）
        /// </summary>
        internal MarkdownLinkEventArgs BuildLink(string url)
        {
            string resolved = ResolveUrl(url);
            string? anchor = null;
            string? file = null;
            var kind = MarkdownLinkKind.External;
            if (url.Length > 0 && url[0] == '#')
            {
                // 文档内锚点跳转
                kind = MarkdownLinkKind.Anchor;
                anchor = NormalizeAnchor(url.Substring(1));
            }
            else if (!StartsWithProtocol(url))
            {
                int hash = url.IndexOf('#');
                string path = hash < 0 ? url : url.Substring(0, hash);
                if (hash >= 0) anchor = NormalizeAnchor(url.Substring(hash + 1));
                // 相对/绝对路径的 Markdown 文件：基于 BasePath 定位
                if (IsMarkdownFile(path))
                {
                    string target = ResolveUrl(path);
                    try
                    {
                        if (System.IO.File.Exists(target))
                        {
                            kind = MarkdownLinkKind.Document;
                            file = System.IO.Path.GetFullPath(target);
                        }
                    }
                    catch { }
                }
            }
            return new MarkdownLinkEventArgs(url, resolved, kind, anchor, file);
        }

        static bool IsMarkdownFile(string path)
        {
            string ext = System.IO.Path.GetExtension(path);
            if (ext.Length == 0) return false;
            switch (ext.ToLowerInvariant())
            {
                case ".md":
                case ".markdown":
                case ".mdown":
                case ".mkd":
                case ".mkdn":
                case ".mdx":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 锚点标准化：URL 解码 + GitHub slug 化（同时兼容 #标题 与 #title-slug 两种写法）
        /// </summary>
        internal static string NormalizeAnchor(string? anchor)
        {
            if (string.IsNullOrEmpty(anchor)) return "";
            string value = anchor!.Trim();
            if (value.Length > 0 && value[0] == '#') value = value.Substring(1);
            try { value = Uri.UnescapeDataString(value); } catch { }
            return MarkdownParser.Slug(value);
        }

        #endregion

        #region 装饰

        /// <summary>
        /// 内容末尾的光标位置（内容坐标，用于流式输出的闪烁光标）
        /// </summary>
        internal Rectangle LastCaret(int width, int height)
        {
            if (lines.Count == 0) return new Rectangle(0, 0, width, Math.Max(2, height));
            var line = lines[lines.Count - 1];
            int x = LineRight(line) + 2;
            return new Rectangle(x, line.Y + 2, width, Math.Max(2, line.Height - 4));
        }

        #endregion

        #region 释放

        internal void Dispose()
        {
            DisposeFont();
            DisposeImages();
            lines.Clear();
            paints.Clear();
            anchorPos.Clear();
            doc = null;
            docOld = null;
        }

        #endregion
    }
}
