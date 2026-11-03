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
    partial class MarkdownRenderer
    {
        /// <summary>
        /// 当前悬停的链接
        /// </summary>
        string? hoverUrl;

        void DrawPaints(Canvas g, Dictionary<Color, SolidBrush> brushes, int sy, int viewH, MDPaint[] paints)
        {
            int bottom = sy + viewH;
            int index = FirstVisiblePaint(sy, paints);
            foreach (var paint in paints)
            {
                if (paint.Rect.Y > bottom) break;
                if (paint.Rect.Bottom < sy) continue;
                if (paint.Svg != null)
                {
                    // 提示块图标（名称由 Canvas.Svg 解析内置图标库）
                    g.Svg(paint.Svg, paint.Rect, paint.Back);
                    continue;
                }
                if (paint.Back.A > 0 || paint.Border.A > 0)
                {
                    bool rounded = paint.Radius > 0;
                    if (rounded || paint.Border.A > 0)
                    {
                        using (var path = rounded ? paint.Rect.RoundPath(paint.Radius, paint.Corners) : paint.Rect.RoundPath(0F, paint.Corners))
                        {
                            if (paint.Back.A > 0) g.Fill(BrushOf(brushes, paint.Back), path);
                            if (paint.Border.A > 0) g.Draw(paint.Border, Math.Max(1, paint.BorderWidth), path);
                        }
                    }
                    else if (paint.Back.A > 0) g.Fill(BrushOf(brushes, paint.Back), paint.Rect);
                }
            }
        }

        /// <summary>
        /// 二分查找首个可见装饰（paints 按 Y 单调不减添加）
        /// </summary>
        int FirstVisiblePaint(int sy, MDPaint[] paints)
        {
            int lo = 0, hi = paints.Length - 1, result = paints.Length;
            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                if (paints[mid].Rect.Bottom >= sy)
                {
                    result = mid;
                    hi = mid - 1;
                }
                else lo = mid + 1;
            }
            return result;
        }

        void DrawLines(Canvas g, Dictionary<Color, SolidBrush> brushes, int sy, int viewH)
        {
            int count = lines.Count;
            if (count == 0) return;
            var tmp = lines.ToArray();
            int bottom = sy + viewH;
            int index = FirstVisibleLine(sy, tmp);
            foreach (var line in tmp)
            {
                if (line.Y > bottom) break;
                if (line.Bottom < sy) continue;
                foreach (var frag in line.Fragments) DrawFragment(g, brushes, frag, line);
            }
        }

        int FirstVisibleLine(int sy, MDVisualLine[] lines)
        {
            int lo = 0, hi = lines.Length - 1, result = lines.Length;
            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                if (lines[mid].Bottom >= sy)
                {
                    result = mid;
                    hi = mid - 1;
                }
                else lo = mid + 1;
            }
            return result;
        }

        void DrawFragment(Canvas g, Dictionary<Color, SolidBrush> brushes, MDFragment frag, MDVisualLine line)
        {
            if (frag.Font == null) return;
            switch (frag.Kind)
            {
                case MDInlineKind.Image:
                    DrawImage(g, brushes, frag);
                    return;
                case MDInlineKind.Code:
                    {
                        int padX = InlineCodePadX(g);
                        int padY = InlineCodePadY(g);
                        int h = frag.Height + padY * 2;
                        int y = line.Y + (line.Height - h) / 2;
                        var rectBack = new Rectangle(frag.X, y, frag.Width, h);
                        using (var brush = new SolidBrush(ColorInlineCodeBack))
                        {
                            using (var path = rectBack.RoundPath(6 * Dpi)) g.Fill(brush, path);
                        }
                        if (frag.Text.Length > 0) g.DrawText(frag.Text, frag.Font, BrushOf(brushes, frag.Fore), new Rectangle(frag.X + padX, rectBack.Y, frag.Width - padX * 2, rectBack.Height), FormatFlags.Center | FormatFlags.NoWrap);
                        return;
                    }
            }
            // 使用 DrawText 以支持 Emoji 渲染（无 Emoji 时内部等价 String）
            if (frag.Text.Length > 0) g.DrawText(frag.Text, frag.Font, BrushOf(brushes, frag.Fore), new Rectangle(frag.X, frag.Y, frag.Width, frag.Height), FormatFlags.Left | FormatFlags.Top | FormatFlags.NoWrap);
            if (frag.Strike)
            {
                int y = frag.Y + (int)(frag.Height * 0.58F);
                g.DrawLine(frag.Fore, Math.Max(1, Dpi), frag.X, y, frag.X + frag.Width, y);
            }
            if (frag.Url != null && frag.Url.Length > 0)
            {
                bool hover = frag.Url == hoverUrl;
                if (hover || LinkUnderline)
                {
                    int y = frag.Y + frag.Height - (int)Math.Round(2 * Dpi);
                    g.DrawLine(frag.Fore, Math.Max(1, Dpi), frag.X, y, frag.X + frag.Width, y);
                }
            }
        }

        void DrawImage(Canvas g, Dictionary<Color, SolidBrush> brushes, MDFragment frag)
        {
            var rect = frag.Rect;
            if (rect.Width <= 0 || rect.Height <= 0) return;
            var img = GetImage(frag.Src);
            if (img != null)
            {
                g.Image(img, rect);
                return;
            }
            // 占位
            var back = Colour.FillQuaternary.Get(ColorScheme, ColorKey, Name);
            var border = Colour.BorderSecondary.Get(ColorScheme, ColorKey, Name);
            var fore = Colour.TextTertiary.Get(ColorScheme, ColorKey, Name);
            using (var brush = new SolidBrush(back))
            {
                using (var path = rect.RoundPath(radius * Dpi)) g.Fill(brush, path);
            }
            using (var path = rect.RoundPath(radius * Dpi)) g.Draw(border, Math.Max(1, Dpi), path);
            string text = frag.Text;
            if (text.Length == 0) text = frag.Src ?? "Image";
            using (var brush = new SolidBrush(fore))
            {
                g.String(text, Font, brush, rect, FormatFlags.Center | FormatFlags.EllipsisCharacter);
            }
        }

        SolidBrush BrushOf(Dictionary<Color, SolidBrush> brushes, Color color)
        {
            SolidBrush? brush;
            if (brushes.TryGetValue(color, out brush)) return brush;
            brush = new SolidBrush(color);
            brushes[color] = brush;
            return brush;
        }

        #region 命中测试

        /// <summary>
        /// 命中的片段（内容坐标：y 已包含滚动偏移）
        /// </summary>
        MDFragment? HitTestFragment(int mx, int my)
        {
            int cy = my;
            int count = lines.Count;
            if (count == 0) return null;
            var tmp = lines.ToArray();
            int index = FirstVisibleLine(my, tmp);
            for (int i = index; i < count; i++)
            {
                var line = lines[i];
                if (line.Y > cy) break;
                if (line.Bottom < cy) continue;
                foreach (var frag in line.Fragments)
                {
                    if (frag.Hit(mx, cy)) return frag;
                }
            }
            return null;
        }

        #endregion
    }
}