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
    partial class Markdown
    {
        #region 布局状态

        readonly List<MDVisualLine> lines = new List<MDVisualLine>();
        readonly List<MDPaint> paints = new List<MDPaint>();
        /// <summary>标题锚点 → 内容坐标 Y（布局时登记，供 #锚点 跳转定位）</summary>
        readonly Dictionary<string, int> anchorPos = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        /// <summary>待定位锚点（布局尚未完成时暂存，布局结束后执行滚动）</summary>
        string? pendingAnchor;
        int[] blockLine = new int[0];
        int[] blockPaint = new int[0];
        MDDocument? doc, docOld;
        int contentHeight;
        bool layouting;

        readonly Dictionary<Font, int> fontKeys = new Dictionary<Font, int>();
        readonly Dictionary<string, int> widthCache = new Dictionary<string, int>(StringComparer.Ordinal);
        readonly Dictionary<Font, int> heightCache = new Dictionary<Font, int>();
        int fontKeySeq;

        /// <summary>行内代码 em 像素（CSS font-size: 85%，行高 ≈ 1.22em 反推）</summary>
        internal float InlineCodeEm(Canvas g) => HeightOf(g, FontCode) * 0.82F;

        /// <summary>行内代码左右内边距（CSS padding .4em，em 基于 code 自身字号）</summary>
        internal int InlineCodePadX(Canvas g) => Math.Max(2, (int)Math.Round(InlineCodeEm(g) * 0.4F));

        /// <summary>行内代码上下内边距（CSS padding .2em）</summary>
        internal int InlineCodePadY(Canvas g) => Math.Max(1, (int)Math.Round(InlineCodeEm(g) * 0.2F));

        /// <summary>内部访问器：布局装饰列表（单元测试 / 诊断用）</summary>
        internal List<MDPaint> TestPaints => paints;
        /// <summary>内部访问器：布局行列表（单元测试 / 诊断用）</summary>
        internal List<MDVisualLine> TestLines => lines;

        /// <summary>
        /// 重新解析并布局
        /// </summary>
        void LoadLayout(bool full = false)
        {
            if (layouting) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            if (!IsHandleCreated) return;
            layouting = true;
            try
            {
                this.GDI(g => LayoutCore(g, rect, full ? 0 : FirstChangedBlock()));
            }
            finally { layouting = false; }
        }

        /// <summary>
        /// 计算首个发生变化块（用于流式增量布局）
        /// </summary>
        int FirstChangedBlock()
        {
            if (doc == null) return 0;
            if (docOld == null || docOld.Blocks.Count == 0) return 0;
            if (doc.Blocks.Count < docOld.Blocks.Count) return docOld.Blocks.Count - 1;
            int count = Math.Min(doc.Blocks.Count, docOld.Blocks.Count);
            for (int i = 0; i < count; i++)
            {
                if (doc.Blocks[i].Hash != docOld.Blocks[i].Hash) return i;
            }
            // 仅尾部新增
            return count > 0 ? count - 1 : 0;
        }

        void LayoutCore(Canvas g, Rectangle rect, int fromBlock)
        {
            var document = doc;
            if (document == null) return;
            EnsureFont();
            rect = rect.PaddingRect(Padding);
            int reserve = ScrollBar.ShowY ? ScrollBar.SIZE : 0;
            for (int pass = 0; pass < 2; pass++)
            {
                int w = rect.Width - reserve;
                if (w <= 0) return;
                LayoutBlocks(g, document, w, pass == 0 ? fromBlock : 0);
                ScrollBar.SetVrSize(contentHeight, rect);
                int now = ScrollBar.ShowY ? ScrollBar.SIZE : 0;
                if (now == reserve) break;
                reserve = now;
            }
            docOld = document;
            // 延迟锚点定位：文档刚切换 / 句柄刚创建时布局在这一步才就绪
            if (pendingAnchor != null)
            {
                string key = pendingAnchor;
                pendingAnchor = null;
                int y = FindAnchorY(key);
                if (y >= 0) ApplyAnchor(y);
            }
            Invalidate();
        }

        void LayoutBlocks(Canvas g, MDDocument document, int width, int fromBlock)
        {
            var blocks = document.Blocks;
            int count = blocks.Count;
            int y = 0;
            if (fromBlock > 0 && fromBlock <= count && blockLine.Length == docOld?.Blocks.Count && lines.Count > 0)
            {
                int lineStart = blockLine[fromBlock];
                if (lineStart < lines.Count) lines.RemoveRange(lineStart, lines.Count - lineStart);
                int paintStart = blockPaint[fromBlock];
                if (paintStart < paints.Count) paints.RemoveRange(paintStart, paints.Count - paintStart);
                if (lines.Count > 0) y = lines[lines.Count - 1].Bottom;
                var newLine = new int[count];
                var newPaint = new int[count];
                Array.Copy(blockLine, newLine, Math.Min(blockLine.Length, fromBlock));
                Array.Copy(blockPaint, newPaint, Math.Min(blockPaint.Length, fromBlock));
                blockLine = newLine;
                blockPaint = newPaint;
            }
            else
            {
                fromBlock = 0;
                lines.Clear();
                paints.Clear();
                anchorPos.Clear();
                blockLine = new int[count];
                blockPaint = new int[count];
            }

            int gap = BlockGap(g);
            for (int i = fromBlock; i < count; i++)
            {
                var block = blocks[i];
                blockLine[i] = lines.Count;
                blockPaint[i] = paints.Count;
                if (block.Kind != MDBlockKind.Blank && lines.Count > 0) y += gap;
                y = LayoutBlock(g, i, block, 0, width, y);
            }
            contentHeight = y + gap;
        }

        int LayoutBlock(Canvas g, int bi, MDBlock block, int x, int width, int y)
        {
            return LayoutBlock(g, bi, block, x, width, y, null);
        }

        int LayoutBlock(Canvas g, int bi, MDBlock block, int x, int width, int y, Color? fore)
        {
            float dpi = Dpi;
            switch (block.Kind)
            {
                case MDBlockKind.Blank:
                    return y;
                case MDBlockKind.Rule:
                    {
                        int gap = BlockGap(g);
                        int ty = y + gap / 2;
                        int thick = Math.Max(1, (int)Math.Round(dpi));
                        paints.Add(new MDPaint { Rect = new Rectangle(x, ty, width, thick), Back = ColorRule });
                        return ty + thick;
                    }
                case MDBlockKind.Code:
                    return LayoutCode(g, bi, block, x, width, y);
                case MDBlockKind.Quote:
                    return LayoutQuote(g, bi, block, x, width, y);
                case MDBlockKind.Table:
                    return LayoutTable(g, bi, block, x, width, y);
                case MDBlockKind.ListUnordered:
                case MDBlockKind.ListOrdered:
                    return LayoutList(g, bi, block, x, width, y);
                case MDBlockKind.Heading1:
                case MDBlockKind.Heading2:
                case MDBlockKind.Heading3:
                case MDBlockKind.Heading4:
                case MDBlockKind.Heading5:
                case MDBlockKind.Heading6:
                    {
                        if (block.Anchor != null) anchorPos[block.Anchor] = y;
                        var font = FontHeading(block.Kind);
                        var runs = BuildRuns(block.Inlines, font, fore ?? ColorHeading);
                        var ls = WrapRuns(g, bi, runs, x, width, y, block.Align);
                        lines.AddRange(ls);
                        int bottom = ls.Count > 0 ? ls[ls.Count - 1].Bottom : y;
                        if (block.Kind == MDBlockKind.Heading1 || block.Kind == MDBlockKind.Heading2)
                        {
                            int thick = Math.Max(1, (int)Math.Round(dpi));
                            int by = bottom + (int)Math.Round(4 * dpi);
                            paints.Add(new MDPaint { Rect = new Rectangle(x, by, width, thick), Back = ColorHeadingBorder });
                            bottom = by + thick;
                        }
                        return bottom;
                    }
                default:
                    {
                        // 引用内文本、提示头使用继承色，其余默认前景色
                        Color runFore = fore ?? (block.AlertHeader && block.Alert != MDAlert.None ? ColorAlert(block.Alert) : ColorFore);
                        return LayoutParagraph(g, bi, block, x, width, y, runFore);
                    }
            }
        }

        #endregion

        #region 段落

        /// <summary>
        /// 段落布局：提示块标题行（AlertHeader）在文本前绘制图标
        /// </summary>
        int LayoutParagraph(Canvas g, int bi, MDBlock block, int x, int width, int y, Color runFore)
        {
            float dpi = Dpi;
            int textX = x;
            if (block.AlertHeader && block.Alert != MDAlert.None)
            {
                string? icon = AlertIcon(block.Alert);
                if (!string.IsNullOrEmpty(icon))
                {
                    int lh = HeightOf(g, Font);
                    int iconSize = (int)Math.Round(lh * 0.92F);
                    int gap = (int)Math.Round(lh * 0.45F);
                    paints.Add(new MDPaint
                    {
                        Rect = new Rectangle(x, y + (lh - iconSize) / 2, iconSize, iconSize),
                        Back = runFore,
                        Svg = icon,
                    });
                    textX = x + iconSize + gap;
                }
            }
            var runs = BuildRuns(block.Inlines, Font, runFore);
            var ls = WrapRuns(g, bi, runs, textX, width - (textX - x), y, block.Align);
            lines.AddRange(ls);
            return ls.Count > 0 ? ls[ls.Count - 1].Bottom : y;
        }

        #endregion

        #region 代码块

        int LayoutCode(Canvas g, int bi, MDBlock block, int x, int width, int y)
        {
            var font = FontCode;
            int lh = HeightOf(g, font);
            int pad = (int)Math.Round(lh * codePadding);
            int padx = (int)Math.Round(pad * 1.2F);
            var src = block.Raw ?? "";
            var codeLines = MarkdownParser.SplitLines(src);
            bool diff = MarkdownHighlighter.IsDiff(block.Lang, src);
            int h = codeLines.Count * lh + pad * 2;
            paints.Add(new MDPaint
            {
                Rect = new Rectangle(x, y, width, h),
                Back = ColorCodeBack,
                Radius = radius * Dpi,
            });
            int cy = y + pad;
            int textX = x + padx;
            int textW = width - padx * 2;
            bool inComment = false;
            var segs = new List<MDSeg>(8);
            foreach (var rawLine in codeLines)
            {
                // Tab 等宽展开为空格，保证多层级缩进对齐（Raw 保留原文用于复制
                string text = rawLine.IndexOf('\t') >= 0 ? ExpandTabs(rawLine) : rawLine;
                var line = new MDVisualLine { Block = bi, Y = cy, Height = lh, Raw = rawLine };
                if (diff)
                {
                    var token = MarkdownHighlighter.DiffLine(text);
                    var back = ColorDiffBack(token);
                    if (back.A > 0) paints.Add(new MDPaint { Rect = new Rectangle(x + padx, cy, Math.Max(1, width - padx * 2), lh), Back = back });
                    line.Fragments.Add(new MDFragment
                    {
                        Text = text,
                        Font = font,
                        Fore = ColorToken(token),
                        Md = text,
                        X = textX,
                        Y = cy,
                        Width = Measure(g, text, font),
                        Height = lh,
                    });
                }
                else
                {
                    MarkdownHighlighter.Tokenize(text, block.Lang, ref inComment, segs);
                    int fx = textX;
                    foreach (var seg in segs)
                    {
                        string value = text.Substring(seg.Start, seg.Length);
                        int w = Measure(g, value, font);
                        line.Fragments.Add(new MDFragment
                        {
                            Text = value,
                            Font = font,
                            Fore = ColorToken(seg.Token),
                            X = fx,
                            Y = cy,
                            Width = w,
                            Height = lh,
                        });
                        fx += w;
                    }
                }
                if (textW > 0 && line.Fragments.Count > 0)
                {
                    var last = line.Fragments[line.Fragments.Count - 1];
                    if (last.X + last.Width > textX + textW)
                    {
                        // 超出可视宽度：截断显示
                        int maxRight = textX + textW;
                        for (int i = line.Fragments.Count - 1; i >= 0; i--)
                        {
                            var frag = line.Fragments[i];
                            if (frag.X >= maxRight) line.Fragments.RemoveAt(i);
                            else break;
                        }
                    }
                }
                lines.Add(line);
                cy += lh;
            }
            return y + h;
        }

        /// <summary>
        /// 代码行 Tab 展开为空格（4 列制表位）
        /// </summary>
        static string ExpandTabs(string text)
        {
            var sb = new System.Text.StringBuilder(text.Length + 8);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '\t')
                {
                    int n = 4 - (sb.Length % 4);
                    sb.Append(' ', n);
                }
                else sb.Append(c);
            }
            return sb.ToString();
        }

        #endregion

        #region 引用

        int LayoutQuote(Canvas g, int bi, MDBlock block, int x, int width, int y)
        {
            float dpi = Dpi;
            int indent = (int)Math.Round(quoteIndent * dpi);
            int barW = Math.Max(2, (int)Math.Round(3 * dpi));
            bool alert = block.Alert != MDAlert.None;
            var barColor = alert ? ColorAlert(block.Alert) : ColorQuoteBar;
            var startY = y;
            var children = block.Children;
            int bottom = y;
            if (children != null && children.Count > 0)
            {
                // 引用文本灰、提示块正文默认色（标题/列表等元素继承）
                Color? fore = alert ? null : (quoteFore ?? ColorQuoteFore);
                int gap = BlockGap(g);
                bool first = true;
                foreach (var child in children)
                {
                    if (child.Kind != MDBlockKind.Blank)
                    {
                        if (!first) y += gap;
                        first = false;
                    }
                    y = LayoutBlock(g, bi, child, x + indent, width - indent, y, fore);
                }
                bottom = y;
            }
            else
            {
                var fore = alert && block.AlertHeader ? ColorAlert(block.Alert) : (quoteFore ?? ColorQuoteFore);
                bottom = LayoutParagraph(g, bi, block, x + indent, width - indent, y, fore);
            }
            // 竖线覆盖整个引用块（多行内容连贯）
            paints.Add(new MDPaint
            {
                Rect = new Rectangle(x, startY, barW, Math.Max(1, bottom - startY)),
                Back = barColor,
                Radius = barW / 2F,
            });
            return bottom;
        }

        #endregion

        #region 列表

        int LayoutList(Canvas g, int bi, MDBlock block, int x, int width, int y)
        {
            float dpi = Dpi;
            int indent = (int)Math.Round(listIndent * dpi);
            int level = block.Level < 0 ? 0 : block.Level;
            int offset = level * indent;
            string marker = block.Kind == MDBlockKind.ListOrdered ? block.Order.ToString() + "." : BulletText(level);
            int markerW = Measure(g, block.Kind == MDBlockKind.ListOrdered ? marker + " " : marker + " ", Font);
            int textX = x + offset + markerW;
            int textW = width - offset - markerW;
            if (textW < 40) textW = 40;

            var runs = BuildRuns(block.Inlines, Font, ColorFore);
            var ls = WrapRuns(g, bi, runs, textX, textW, y, MDAlign.None);
            if (ls.Count > 0)
            {
                var first = ls[0];
                first.Fragments.Insert(0, new MDFragment
                {
                    Text = marker,
                    Font = Font,
                    Fore = ColorFore,
                    Md = "",
                    X = x + offset,
                    Y = first.Y,
                    Width = markerW,
                    Height = first.Height,
                });
            }
            lines.AddRange(ls);
            return ls.Count > 0 ? ls[ls.Count - 1].Bottom : y;
        }

        static string BulletText(int level)
        {
            switch (level % 3)
            {
                case 0: return "•";
                case 1: return "◦";
                default: return "▪";
            }
        }

        #endregion

        #region 表格

        int LayoutTable(Canvas g, int bi, MDBlock block, int x, int width, int y)
        {
            var rows = block.Rows;
            if (rows == null || rows.Count == 0) return y;
            float dpi = Dpi;
            // 单元格内边距按字体占比（GitHub padding: 6px 13px @ 16px 字号）
            int lineH = HeightOf(g, Font);
            int pad, padV;
            if (tableCellPadding > 0)
            {
                pad = (int)Math.Round(tableCellPadding * dpi);
                padV = Math.Max(3, (int)Math.Round(pad * 0.5F));
            }
            else
            {
                pad = Math.Max(3, (int)Math.Round(lineH * 0.61F));
                padV = Math.Max(3, (int)Math.Round(lineH * 0.28F));
            }
            int cols = 0;
            foreach (var row in rows) if (row.Count > cols) cols = row.Count;
            if (cols == 0) return y;

            int[] colW = new int[cols];
            bool hasWidth = false;
            if (block.ColWidth != null)
            {
                float rest = 100F;
                int auto = 0;
                for (int i = 0; i < cols; i++)
                {
                    float val = i < block.ColWidth.Length ? block.ColWidth[i] : 0;
                    if (val > 0)
                    {
                        hasWidth = true;
                        colW[i] = (int)Math.Round(width * val / 100F);
                        rest -= val;
                    }
                    else if (val < 0)
                    {
                        hasWidth = true;
                        colW[i] = (int)Math.Round(-val * dpi);
                        rest -= (-val * dpi / width) * 100F;
                    }
                    else auto++;
                }
                if (hasWidth)
                {
                    int used = 0;
                    for (int i = 0; i < cols; i++) if (colW[i] > 0) used += colW[i];
                    int left = width - used;
                    if (auto > 0 && left > 0)
                    {
                        int each = left / auto;
                        for (int i = 0; i < cols; i++) if (colW[i] <= 0) colW[i] = each;
                    }
                    else if (left < 0)
                    {
                        float scale = width / (float)used;
                        for (int i = 0; i < cols; i++) colW[i] = (int)Math.Round(colW[i] * scale);
                    }
                }
            }
            if (!hasWidth)
            {
                // 按内容自适应（GitHub 风格：表格宽度由内容决定，不强制铺满）
                // 列宽必须同时满足两种测量口径，否则会出现"整行明明放得下、最后一个字却被换行"：
                //   ① RunsWidth：整串测量，决定实际绘制（同一 run 的相邻 token 合并成片段后再测量）
                //   ② TokenWidth：逐 token 累加，决定换行判定（WrapRuns 逐个 token 比较 cx + t.Width > width）
                // 两者相差 1px 量级：整串测量会略过行尾空白，逐 token 累加则每个 token 各自向上取整。
                int slack = Math.Max(1, (int)Math.Ceiling(2 * dpi));
                for (int c = 0; c < cols; c++)
                {
                    int max = 0;
                    for (int r = 0; r < rows.Count; r++)
                    {
                        var row = rows[r];
                        if (c >= row.Count) continue;
                        // 表头按粗体测量，保证列宽足够
                        var font = r == 0 && block.HasHeader ? FontBold : Font;
                        var runs = BuildRuns(row[c].Inlines, font, ColorFore);
                        int w = Math.Max(RunsWidth(g, runs), TokenWidth(g, runs));
                        if (w > max) max = w;
                    }
                    colW[c] = max + pad * 2 + slack;
                }
                int total = 0;
                for (int c = 0; c < cols; c++) total += colW[c];
                if (total > width)
                {
                    // 内容感知压缩：窄列保持内容宽度，只让宽列吸收压缩量（与浏览器表格一致）。
                    // 等比例压缩会把"刚好够用"的窄列也压出换行，出现容器明明放得下、整列却在换字的观感。
                    int minCol = Math.Max((int)Math.Round(40 * dpi), 1);
                    var order = new int[cols];
                    for (int c = 0; c < cols; c++) order[c] = c;
                    Array.Sort(order, (a, b) => colW[a].CompareTo(colW[b]));
                    var keepCol = new bool[cols];
                    int avail = width, left = cols;
                    foreach (var c in order)
                    {
                        if (left <= 0) break;
                        // 该列能放进剩余空间的平均份额：先固定，不再参与压缩
                        if (colW[c] * left <= avail)
                        {
                            keepCol[c] = true;
                            avail -= colW[c];
                            left--;
                        }
                        else break;
                    }
                    if (left > 0)
                    {
                        int share = Math.Max(minCol, avail / left);
                        for (int c = 0; c < cols; c++) if (!keepCol[c]) colW[c] = share;
                        int used = 0;
                        for (int c = 0; c < cols; c++) used += colW[c];
                        if (used > width)
                        {
                            // 容器过窄，连保底宽度都排不下：按比例再压一档，宁可挤也不要溢出容器
                            float scale = width / (float)used;
                            for (int c = 0; c < cols; c++)
                            {
                                if (!keepCol[c]) colW[c] = Math.Max(1, (int)Math.Round(colW[c] * scale));
                            }
                        }
                        else
                        {
                            // 取整余数补给最后一个可压缩列，避免表格右侧多出 1px 空隙
                            int extra = width - used;
                            if (extra > 0)
                            {
                                for (int c = cols - 1; c >= 0; c--)
                                {
                                    if (keepCol[c]) continue;
                                    colW[c] += extra;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            int tableW = 0;
            for (int c = 0; c < cols; c++) tableW += colW[c];

            int thick = Math.Max(1, (int)Math.Round(dpi));
            var border = ColorTableBorder;

            #region 测量（父容器尺寸由内容决定）

            int rowCount = rows.Count;
            var cellLines = new List<MDVisualLine>[rowCount][];
            var rowHeights = new int[rowCount][];
            var rowTop = new int[rowCount + 1];
            rowTop[0] = y;
            for (int r = 0; r < rowCount; r++)
            {
                var row = rows[r];
                bool header = r == 0 && block.HasHeader;
                var cells = new List<MDVisualLine>[cols];
                int maxLines = 1;
                int cx = x;
                for (int c = 0; c < cols; c++)
                {
                    int cw = colW[c];
                    var runs = c < row.Count ? BuildRuns(row[c].Inlines, header ? FontBold : Font, ColorFore) : new List<MDRun>(0);
                    MDAlign align = MDAlign.None;
                    if (block.ColAlign != null && c < block.ColAlign.Length) align = block.ColAlign[c];
                    var ls = WrapRuns(g, bi, runs, cx + pad, cw - pad * 2, 0, align);
                    cells[c] = ls;
                    if (ls.Count > maxLines) maxLines = ls.Count;
                    cx += cw;
                }
                var heights = new int[maxLines];
                for (int c = 0; c < cols; c++)
                {
                    var ls = cells[c];
                    for (int k = 0; k < ls.Count; k++)
                    {
                        if (ls[k].Height > heights[k]) heights[k] = ls[k].Height;
                    }
                }
                cellLines[r] = cells;
                rowHeights[r] = heights;
                int rowH = padV * 2;
                for (int k = 0; k < maxLines; k++) rowH += heights[k];
                rowTop[r + 1] = rowTop[r] + Math.Max(1, rowH);
            }
            int tableH = Math.Max(1, rowTop[rowCount] - y);

            #endregion

            #region 父容器（外边框 + 背景 + 网格线）

            // 背景层按容器圆角裁切：首行只圆上两角、末行只圆下两角，中间行直角
            for (int r = 0; r < rowCount; r++)
            {
                bool header = r == 0 && block.HasHeader;
                Color back = header ? ColorTableHeaderBack : (r % 2 == 0 ? ColorTableStripe : Style.Db.BgBase);
                if (back.A > 0)
                {
                    paints.Add(new MDPaint
                    {
                        Rect = new Rectangle(x, rowTop[r], tableW, rowTop[r + 1] - rowTop[r]),
                        Back = back,
                        Radius = 0,
                        Corners = TAlignRound.ALL,
                    });
                }
                // 横向分隔线（最后一行由容器外框承担，避免重叠加粗）
                if (r < rowCount - 1)
                    paints.Add(new MDPaint { Rect = new Rectangle(x, rowTop[r + 1] - thick, tableW, thick), Back = border });
            }
            // 纵向分隔线贯穿容器上下
            int vx = x;
            for (int c = 0; c < cols - 1; c++)
            {
                vx += colW[c];
                paints.Add(new MDPaint { Rect = new Rectangle(vx, y, thick, tableH), Back = border });
            }
            // 容器外框：圆角描边（压住网格线端头，四角保持干净）
            paints.Add(new MDPaint
            {
                Rect = new Rectangle(x, y, tableW, tableH),
                Border = border,
                BorderWidth = thick,
            });

            #endregion

            #region 内容（单元格文本落在容器内）

            for (int r = 0; r < rowCount; r++)
            {
                var cells = cellLines[r];
                var heights = rowHeights[r];
                int ly = rowTop[r] + padV;
                for (int k = 0; k < heights.Length; k++)
                {
                    var line = new MDVisualLine { Block = bi, Y = ly, Height = heights[k], Row = r };
                    for (int c = 0; c < cols; c++)
                    {
                        var ls = cells[c];
                        if (k >= ls.Count) continue;
                        var src = ls[k];
                        int dy = src.Y;
                        foreach (var frag in src.Fragments)
                        {
                            line.Fragments.Add(new MDFragment
                            {
                                Text = frag.Text,
                                Font = frag.Font,
                                Fore = frag.Fore,
                                X = frag.X,
                                Y = ly + (frag.Y - dy),
                                Width = frag.Width,
                                Height = frag.Height,
                                Kind = frag.Kind,
                                Url = frag.Url,
                                Src = frag.Src,
                                Md = frag.Md,
                                Cell = c,
                                Strike = frag.Strike,
                            });
                        }
                    }
                    lines.Add(line);
                    ly += heights[k];
                }
            }

            #endregion

            return y + tableH;
        }

        #endregion

        #region 内联排版

        List<MDRun> BuildRuns(List<MDInline> inlines, Font baseFont, Color baseFore)
        {
            EnsureFont();
            var runs = new List<MDRun>(inlines.Count);
            foreach (var inline in inlines)
            {
                switch (inline.Kind)
                {
                    case MDInlineKind.Bold:
                        runs.Add(Run(inline, FontBold, baseFore));
                        break;
                    case MDInlineKind.Italic:
                        runs.Add(Run(inline, FontItalic, baseFore));
                        break;
                    case MDInlineKind.BoldItalic:
                        runs.Add(Run(inline, FontBoldItalic, baseFore));
                        break;
                    case MDInlineKind.Code:
                        {
                            // 继承父级字重（表头 th / 标题内 code 为粗体，与 GitHub 一致）
                            bool bold = baseFont.Bold;
                            runs.Add(Run(inline, bold ? FontCodeBold : FontCode, ColorInlineCodeFore));
                            break;
                        }
                    case MDInlineKind.Strike:
                        var strike = Run(inline, baseFont, baseFore);
                        strike.Strike = true;
                        runs.Add(strike);
                        break;
                    case MDInlineKind.Sub:
                        runs.Add(Run(inline, FontSub, baseFore));
                        break;
                    case MDInlineKind.Sup:
                        runs.Add(Run(inline, FontSup, baseFore));
                        break;
                    case MDInlineKind.Link:
                        runs.Add(Run(inline, baseFont, ColorLink));
                        break;
                    case MDInlineKind.Image:
                        {
                            var run = new MDRun
                            {
                                Text = inline.Text,
                                Font = baseFont,
                                Fore = baseFore,
                                Kind = MDInlineKind.Image,
                                Src = inline.Url,
                                Url = inline.Href,
                                Md = inline.Md,
                                Width = inline.Width,
                                Height = inline.Height,
                            };
                            runs.Add(run);
                            break;
                        }
                    case MDInlineKind.Break:
                        runs.Add(new MDRun { Text = "", Font = baseFont, Fore = baseFore, Kind = MDInlineKind.Break });
                        break;
                    default:
                        runs.Add(Run(inline, baseFont, baseFore));
                        break;
                }
            }
            return runs;
        }

        static MDRun Run(MDInline inline, Font font, Color fore)
        {
            return new MDRun
            {
                Text = inline.Text,
                Font = font,
                Fore = fore,
                Kind = inline.Kind,
                Url = inline.Url,
                Md = inline.Md,
            };
        }

        internal int RunsWidth(Canvas g, List<MDRun> runs)
        {
            int w = 0;
            foreach (var run in runs)
            {
                w += MeasureStatic(g, run.Text, run.Font);
                // 行内代码背景含左右内边距（与 FlushLine 一致）
                if (run.Kind == MDInlineKind.Code) w += InlineCodePadX(g) * 2;
            }
            return w;
        }

        /// <summary>
        /// 逐 token 累加的文本宽度（与 WrapRuns 的换行判定口径完全一致）
        /// </summary>
        /// <remarks>
        /// 换行判定是"每个 token 单独测量后累加"，而 <see cref="RunsWidth"/> 是整串测量，
        /// 两者在行尾空白 / 逐 token 取整上有 1px 量级差异。
        /// 表格列宽自适应若只按整串测量，就会把刚好放得下的一整行判为溢出而换行。
        /// </remarks>
        int TokenWidth(Canvas g, List<MDRun> runs)
        {
            var tokens = new List<MDTokenItem>(runs.Count * 2);
            for (int i = 0; i < runs.Count; i++)
            {
                var run = runs[i];
                if (run.Kind == MDInlineKind.Break) continue;
                if (run.Kind == MDInlineKind.Image)
                {
                    // 图片按内联盒宽度计入（未加载时为占位尺寸）
                    tokens.Add(new MDTokenItem(i, 0, 0, ImageSize(run, int.MaxValue).Width, false, false));
                    continue;
                }
                if (run.Text.Length == 0) continue;
                // 不限宽：只做分词，不做按宽度拆分（列宽由结果决定，而不是先给一个宽度）
                AddTokens(g, i, run, tokens, int.MaxValue);
            }
            int w = 0;
            foreach (var t in tokens)
            {
                w += t.Width;
                // 行内代码内边距：整段只计一次（与 FlushLine 单片段一致）
                if (t.Start == 0 && runs[t.Run].Kind == MDInlineKind.Code) w += InlineCodePadX(g) * 2;
            }
            return w;
        }

        /// <summary>
        /// 换行判定需要额外计入的宽度（行内代码片段的左右内边距）
        /// </summary>
        /// <remarks>
        /// 片段的左右内边距是在 <see cref="FlushLine"/> 里计入片段宽度的；换行判定若不预先计入，
        /// 含行内代码的行就会超出内容宽度、压住右侧内边距（Padding.Right）——纯文本行不受影响。
        /// 同一个 code 运行内连续的 token 属于同一片段，只在片段起点计一次；
        /// 行内换行后片段被拆开，新行首的片段重新计一次（与 FlushLine 的片段划分口径一致）。
        /// </remarks>
        int TokenExtra(Canvas g, List<MDRun> runs, List<MDTokenItem> tokens, int index, int start)
        {
            var cur = tokens[index];
            if (runs[cur.Run].Kind != MDInlineKind.Code) return 0;
            if (index > start)
            {
                var prev = tokens[index - 1];
                if (prev.Run == cur.Run && prev.Start + prev.Len == cur.Start) return 0;
            }
            return InlineCodePadX(g) * 2;
        }

        /// <summary>
        /// 将内联内容排版为视觉行
        /// </summary>
        List<MDVisualLine> WrapRuns(Canvas g, int bi, List<MDRun> runs, int x, int width, int y, MDAlign align)
        {
            var result = new List<MDVisualLine>();
            if (runs.Count == 0) return result;
            if (width < 10) width = 10;
            var tokens = new List<MDTokenItem>(runs.Count * 2);
            for (int i = 0; i < runs.Count; i++)
            {
                var run = runs[i];
                if (run.Kind == MDInlineKind.Break)
                {
                    tokens.Add(new MDTokenItem(i, 0, 0, 0, true, false));
                    continue;
                }
                if (run.Kind == MDInlineKind.Image)
                {
                    tokens.Add(new MDTokenItem(i, 0, 0, 0, false, false));
                    continue;
                }
                if (run.Text.Length == 0) continue;
                AddTokens(g, i, run, tokens, width);
            }

            int cy = y;
            int start = 0, cx = 0;
            // 当前行是否已有内容：用指针（start）判断会误伤，图片未加载时宽度为 0 会让 cx 一直是 0，
            // 于是行首空白判断把“图片 + 空格”整段跳过（图片被丢弃）——必须单独记状态
            bool lineEmpty = true;
            for (int i = 0; i <= tokens.Count; i++)
            {
                if (i == tokens.Count)
                {
                    FlushLine(g, bi, runs, tokens, start, tokens.Count - 1, x, width, cy, align, result);
                    break;
                }
                var t = tokens[i];
                if (t.Break)
                {
                    FlushLine(g, bi, runs, tokens, start, i - 1, x, width, cy, align, result);
                    if (result.Count > 0) cy = result[result.Count - 1].Bottom;
                    start = i + 1;
                    cx = 0;
                    lineEmpty = true;
                    continue;
                }
                if (runs[t.Run].Kind == MDInlineKind.Image)
                {
                    // 图片按内联盒参与排版：同行放得下就接着排，放不下才换行
                    // 尺寸总是 > 0（未加载时返回占位尺寸），保证 cx 推进、换行与占位框正确
                    int iw = ImageSize(runs[t.Run], width).Width;
                    if (!lineEmpty && cx + iw > width)
                    {
                        FlushLine(g, bi, runs, tokens, start, i - 1, x, width, cy, align, result);
                        if (result.Count > 0) cy = result[result.Count - 1].Bottom;
                        start = i;
                        cx = 0;
                        lineEmpty = true;
                    }
                    cx += iw;
                    lineEmpty = false;
                    continue;
                }
                // white-space: break-spaces —— 行内代码内的空白保留，不按行首空白丢弃
                if (t.Space && lineEmpty && runs[t.Run].Kind != MDInlineKind.Code)
                {
                    // 行首空白：丢弃该 token
                    start = i + 1;
                    continue;
                }
                if (!lineEmpty && cx + TokenExtra(g, runs, tokens, i, start) + t.Width > width)
                {
                    FlushLine(g, bi, runs, tokens, start, i - 1, x, width, cy, align, result);
                    if (result.Count > 0) cy = result[result.Count - 1].Bottom;
                    start = i;
                    cx = 0;
                    lineEmpty = true;
                    // break-spaces：行内代码内的空白保留到下一行
                    if (t.Space && runs[t.Run].Kind != MDInlineKind.Code)
                    {
                        start = i + 1;
                        continue;
                    }
                }
                cx += TokenExtra(g, runs, tokens, i, start) + t.Width;
                lineEmpty = false;
            }
            return result;
        }

        void FlushLine(Canvas g, int bi, List<MDRun> runs, List<MDTokenItem> tokens, int start, int end, int x, int width, int y, MDAlign align, List<MDVisualLine> result)
        {
            if (end < start) return;
            var line = new MDVisualLine { Block = bi, Y = y, Align = align };
            int h = 0, fx = x;
            int i = start;
            while (i <= end)
            {
                var t = tokens[i];
                var run = runs[t.Run];
                if (run.Kind == MDInlineKind.Image)
                {
                    // 内联图片：按当前排版位置落位，不再独占一行
                    var imgFrag = BuildImageFragment(g, run, fx, width, y, MDAlign.None);
                    line.Fragments.Add(imgFrag);
                    int imgH = imgFrag.Height + (int)Math.Round(4 * Dpi);
                    if (imgH > h) h = imgH;
                    fx += imgFrag.Width;
                    i++;
                    continue;
                }
                int runIndex = t.Run, first = t.Start, len = t.Len;
                int j = i + 1;
                while (j <= end && tokens[j].Run == runIndex && tokens[j].Start == first + len)
                {
                    len += tokens[j].Len;
                    j++;
                }
                string text = run.Text.Substring(first, len);
                int w = Measure(g, text, run.Font);
                int offset = 0;
                if (run.Kind == MDInlineKind.Sub) offset = (int)(HeightOf(g, run.Font) * 0.22F);
                else if (run.Kind == MDInlineKind.Sup) offset = -(int)(HeightOf(g, run.Font) * 0.32F);
                else if (run.Kind == MDInlineKind.Code)
                {
                    // GitHub 行内代码：padding .2em .4em 计入片段宽度（背景含左右空隙，不与相邻文本重叠），字体按行高垂直居中
                    int lineH = HeightOf(g, Font), runH = HeightOf(g, run.Font);
                    if (runH < lineH) offset = (lineH - runH) / 2;
                    w += InlineCodePadX(g) * 2;
                }
                line.Fragments.Add(new MDFragment
                {
                    Text = text,
                    Font = run.Font,
                    Fore = run.Fore,
                    Kind = run.Kind,
                    Url = run.Kind == MDInlineKind.Link ? run.Url : null,
                    Strike = run.Strike,
                    Md = run.Md,
                    X = fx,
                    Y = y + offset,
                    Width = w,
                    Height = HeightOf(g, run.Font),
                });
                int th = HeightOf(g, run.Font) + offset;
                if (th > h) h = th;
                fx += w;
                i = j;
            }
            if (h <= 0) h = HeightOf(g, Font);
            line.Height = h;
            if (line.Fragments.Count == 0) return;
            ApplyAlign(line, x, width, align);
            result.Add(line);
        }

        static void ApplyAlign(MDVisualLine line, int x, int width, MDAlign align)
        {
            if (align == MDAlign.None || align == MDAlign.Left) return;
            int min = int.MaxValue, max = int.MinValue;
            foreach (var frag in line.Fragments)
            {
                if (frag.X < min) min = frag.X;
                if (frag.X + frag.Width > max) max = frag.X + frag.Width;
            }
            if (min == int.MaxValue) return;
            int delta = align == MDAlign.Center ? x + (width - (max - min)) / 2 - min : x + width - max;
            if (delta == 0) return;
            foreach (var frag in line.Fragments) frag.X += delta;
        }

        /// <summary>
        /// 计算图片显示尺寸（指定宽高优先，其次原始尺寸，均受容器宽度约束）
        /// </summary>
        /// <remarks>
        /// 图片未加载（或加载失败）时不会返回 0，而是返回占位尺寸：
        /// 返回 0 会让排版认为该内联盒没有宽度，导致换行判断失效、行首空白逻辑把图片丢弃。
        /// </remarks>
        Size ImageSize(MDRun run, int maxWidth)
        {
            float dpi = Dpi;
            string? src = run.Src;
            int iw = 0, ih = 0;
            var img = GetImage(src);
            if (img != null)
            {
                iw = img.Width;
                ih = img.Height;
            }
            int dw = run.Width > 0 ? (int)Math.Round(run.Width * dpi) : 0;
            int dh = run.Height > 0 ? (int)Math.Round(run.Height * dpi) : 0;
            if (iw > 0 && ih > 0)
            {
                if (dw > 0 && dh > 0) { }
                else if (dw > 0) dh = (int)Math.Round(ih * (dw / (float)iw));
                else if (dh > 0) dw = (int)Math.Round(iw * (dh / (float)ih));
                else
                {
                    dw = iw;
                    dh = ih;
                    if (dw > maxWidth)
                    {
                        dh = (int)Math.Round(dh * (maxWidth / (float)dw));
                        dw = maxWidth;
                    }
                }
                if (dw > maxWidth && dw > 0)
                {
                    dh = (int)Math.Round(dh * (maxWidth / (float)dw));
                    dw = maxWidth;
                }
            }
            return new Size(dw, dh);
        }

        MDFragment BuildImageFragment(Canvas g, MDRun run, int x, int width, int y, MDAlign align)
        {
            var size = ImageSize(run, width);
            int fx = x;
            if (align == MDAlign.Center) fx = x + (width - size.Width) / 2;
            else if (align == MDAlign.Right) fx = x + width - size.Width;
            return new MDFragment
            {
                Text = run.Text,
                Font = run.Font,
                Fore = run.Fore,
                Kind = MDInlineKind.Image,
                Src = run.Src,
                Url = run.Url,
                X = fx,
                Y = y,
                Width = size.Width,
                Height = size.Height,
            };
        }

        void AddTokens(Canvas g, int runIndex, MDRun run, List<MDTokenItem> tokens, int width)
        {
            string text = run.Text;
            int i = 0, n = text.Length;
            while (i < n)
            {
                char c = text[i];
                int len;
                bool space = false;
                if (c == ' ' || c == '\t')
                {
                    len = 1;
                    while (i + len < n && (text[i + len] == ' ' || text[i + len] == '\t')) len++;
                    space = true;
                }
                else if (char.IsSurrogate(c)) len = GraphemeLen(text, i);
                else if (IsBreakChar(c)) len = 1;
                else
                {
                    len = 1;
                    while (i + len < n && !char.IsWhiteSpace(text[i + len]) && !IsBreakChar(text[i + len]) && !char.IsSurrogate(text[i + len])) len++;
                }
                // 组合标记必须并入当前字素：否则 VS16 / 键帽 / ZWJ 会被裁剪成独立单字符 token，
                // 换行时落单后被 GDI+ 渲染为缺字方块（emoji 只剩一个字符）
                if (i + len < n && IsGraphemeExtend(text[i + len])) len = GraphemeLen(text, i);
                if (len > 24)
                {
                    len = 24;
                    // 避免截断代理对 / 组合标记（表情等）
                    while (len > 1 && (char.IsSurrogate(text[i + len - 1]) || (i + len < n && IsGraphemeExtend(text[i + len])))) len--;
                }
                string val = text.Substring(i, len);
                int w = Measure(g, val, run.Font);
                if (w > width && len > 1)
                {
                    // 超长内容逐字素拆分（代理对 / 组合标记整体测量，避免拆散表情）
                    int k = 0;
                    while (k < len)
                    {
                        int cl = GraphemeLen(text, i + k);
                        tokens.Add(new MDTokenItem(runIndex, i + k, cl, Measure(g, text.Substring(i + k, cl), run.Font), false, false));
                        k += cl;
                    }
                }
                else tokens.Add(new MDTokenItem(runIndex, i, len, w, false, space));
                i += len;
            }
        }

        static bool IsBreakChar(char c) => c >= 0x2E80 && !char.IsSurrogate(c);

        /// <summary>
        /// 取一个完整字素的长度（代理对 / 变体选择符 / 键帽 / ZWJ 连字序列 / 肤色修饰符）
        /// </summary>
        /// <remarks>
        /// 起始字符可以是 BMP 字符（如 ⬇ U+2B07）——后续的 VS16 等组合标记必须并入同一字素，
        /// 否则会被裁剪成独立单字符，换行时落单并被 GDI+ 渲染为缺字方块。
        /// </remarks>
        static int GraphemeLen(string text, int i)
        {
            int n = text.Length;
            int len = char.IsHighSurrogate(text[i]) && i + 1 < n && char.IsLowSurrogate(text[i + 1]) ? 2 : 1;
            while (i + len < n)
            {
                char d = text[i + len];
                if (d == '\u200D')
                {
                    // ZWJ 连字：连字符本身 + 后随 emoji
                    len++;
                    if (i + len + 1 < n && char.IsHighSurrogate(text[i + len]) && char.IsLowSurrogate(text[i + len + 1])) len += 2;
                    continue;
                }
                if (d == '\uFE0E' || d == '\uFE0F' || d == '\u20E3')
                {
                    // 变体选择符 / 键帽
                    len++;
                    continue;
                }
                // 肤色修饰符 U+1F3FB–U+1F3FF
                if (char.IsHighSurrogate(d) && d == '\uD83C' && i + len + 1 < n && text[i + len + 1] >= '\uDFFB' && text[i + len + 1] <= '\uDFFF')
                {
                    len += 2;
                    continue;
                }
                break;
            }
            return len;
        }

        /// <summary>是否为字素扩展标记（必须并入前一个字素，不可独立成 token）</summary>
        static bool IsGraphemeExtend(char c)
        {
            if (c == '\uFE0E' || c == '\uFE0F' || c == '\u20E3' || c == '\u200D') return true;
            return char.IsHighSurrogate(c) && c == '\uD83C';
        }

        #endregion

        #region 测量

        int BlockGap(Canvas g) => Math.Max(2, (int)Math.Round(HeightOf(g, Font) * blockSpacing));

        int Measure(Canvas g, string text, Font? font)
        {
            if (text.Length == 0 || font == null) return 0;
            int key;
            if (!fontKeys.TryGetValue(font, out key))
            {
                key = ++fontKeySeq;
                fontKeys[font] = key;
            }
            string cacheKey = key.ToString() + "\0" + text;
            int result;
            if (widthCache.TryGetValue(cacheKey, out result)) return result;
            result = g.MeasureText(text, font).Width;
            if (widthCache.Count > 6000) widthCache.Clear();
            widthCache[cacheKey] = result;
            return result;
        }

        static int MeasureStatic(Canvas g, string text, Font? font) => text.Length == 0 || font == null ? 0 : g.MeasureText(text, font).Width;

        int HeightOf(Canvas g, Font? font)
        {
            int result;
            if (font == null) return 12;
            if (heightCache.TryGetValue(font, out result)) return result;
            result = g.MeasureText("M\0g", font).Height;
            heightCache[font] = result;
            return result;
        }

        void InvalidateMeasure()
        {
            widthCache.Clear();
            heightCache.Clear();
            fontKeys.Clear();
            fontKeySeq = 0;
        }

        #endregion
    }

    /// <summary>
    /// 排版单元
    /// </summary>
    internal struct MDTokenItem
    {
        public int Run;
        public int Start;
        public int Len;
        public int Width;
        public bool Break;
        public bool Space;

        public MDTokenItem(int run, int start, int len, int width, bool brk, bool space)
        {
            Run = run;
            Start = start;
            Len = len;
            Width = width;
            Break = brk;
            Space = space;
        }
    }
}