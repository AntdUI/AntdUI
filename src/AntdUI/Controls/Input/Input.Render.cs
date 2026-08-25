// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        #region 渲染

        readonly internal FormatFlags sf_center = FormatFlags.Center | FormatFlags.NoWrap;
        readonly internal FormatFlags sf_center_rtl = FormatFlags.Center | FormatFlags.NoWrap | FormatFlags.DirectionRightToLeft;
        internal FormatFlags sf_placeholder = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;

        public new virtual void Invalidate() => base.Invalidate();

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            Rectangle rect = e.Rect.PaddingRect(Padding), rect_read = rect.ReadRect((WaveSize + borderWidth / 2F) * Dpi, joinMode, JoinLeft, JoinRight);
            IPaint(g, rect, rect_read);
            base.OnDraw(e);
        }

        internal void IPaint(Canvas g, Rectangle rect, Rectangle rect_read)
        {
            float _radius = round ? rect_read.Height : radius * Dpi;
            if (backImage != null) g.Image(rect_read, backImage, backFit, _radius, false);
            using (var path = Helper.PathJoin(rect_read, _radius, joinMode, JoinLeft, JoinRight))
            {
                Color _back = back ?? Colour.BgContainer.Get(ColorScheme, nameof(Input), Name),
                    _fore = fore ?? Colour.Text.Get(ColorScheme, nameof(Input), Name),
                   _border = borderColor ?? Colour.BorderColor.Get(ColorScheme, nameof(Input), Name),
                   _borderHover = BorderHover ?? Colour.PrimaryHover.Get(ColorScheme, nameof(Input), Name),
               _borderActive = BorderActive ?? Colour.Primary.Get(ColorScheme, nameof(Input), Name);

                switch (status)
                {
                    case TType.Success:
                        _border = Colour.SuccessBorder.Get(ColorScheme, nameof(Input), Name);
                        _borderHover = Colour.SuccessHover.Get(ColorScheme, nameof(Input), Name);
                        _borderActive = Colour.Success.Get(ColorScheme, nameof(Input), Name);
                        break;
                    case TType.Error:
                        _border = Colour.ErrorBorder.Get(ColorScheme, nameof(Input), Name);
                        _borderHover = Colour.ErrorHover.Get(ColorScheme, nameof(Input), Name);
                        _borderActive = Colour.Error.Get(ColorScheme, nameof(Input), Name);
                        break;
                    case TType.Warn:
                        _border = Colour.WarningBorder.Get(ColorScheme, nameof(Input), Name);
                        _borderHover = Colour.WarningHover.Get(ColorScheme, nameof(Input), Name);
                        _borderActive = Colour.Warning.Get(ColorScheme, nameof(Input), Name);
                        break;
                }

                PaintClick(g, path, rect, _borderActive, _radius);

                if (Enabled)
                {
                    using (var brush = backExtend.BrushEx(rect_read, _back))
                    {
                        g.Fill(brush, path);
                    }
                    PaintLoadingWave(g, path, rect_read);
                    PaintIcon(g, _fore);
                    PaintText(g, _fore, rect_read.Right, rect_read.Bottom);
                    PaintOtherBor(g, rect_read, _radius, _back, _border, _borderActive);
                    PaintScroll(g, rect_read, _radius);
                    if (borderWidth > 0)
                    {
                        var borWidth = borderWidth * Dpi;
                        if (Variant == TVariant.Underlined)
                        {
                            if (AnimationHover) g.DrawLine(_border.BlendColors(AnimationHoverValue, _borderHover), borWidth, rect_read.X, rect_read.Bottom, rect_read.Right, rect_read.Bottom);
                            else if (ExtraMouseDown) g.DrawLine(_borderActive, borWidth, rect_read.X, rect_read.Bottom, rect_read.Right, rect_read.Bottom);
                            else if (ExtraMouseHover) g.DrawLine(_borderHover, borWidth, rect_read.X, rect_read.Bottom, rect_read.Right, rect_read.Bottom);
                            else if (AnimationBlinkState && colorBlink.HasValue) g.DrawLine(colorBlink.Value, borWidth, rect_read.X, rect_read.Bottom, rect_read.Right, rect_read.Bottom);
                            else g.DrawLine(_border, borWidth, rect_read.X, rect_read.Bottom, rect_read.Right, rect_read.Bottom);
                        }
                        else
                        {
                            if (AnimationHover) g.Draw(_border.BlendColors(AnimationHoverValue, _borderHover), borWidth, path);
                            else if (ExtraMouseDown) g.Draw(_borderActive, borWidth, path);
                            else if (ExtraMouseHover) g.Draw(_borderHover, borWidth, path);
                            else if (AnimationBlinkState && colorBlink.HasValue) g.Draw(colorBlink.Value, borWidth, path);
                            else g.Draw(_border, borWidth, path);
                        }
                    }
                }
                else
                {
                    g.Fill(Colour.FillTertiary.GetSymbol(ColorScheme, "bgDisabled", nameof(Input), Name), path);
                    PaintLoadingWave(g, path, rect_read);
                    var fore = Colour.TextQuaternary.GetSymbol(ColorScheme, "foreDisabled", nameof(Input), Name);
                    PaintIcon(g, fore);
                    PaintText(g, fore, rect_read.Right, rect_read.Bottom);
                    PaintOtherBor(g, rect_read, _radius, _back, _border, _borderActive);
                    PaintScroll(g, rect_read, _radius);
                    if (borderWidth > 0) g.Draw(_border, borderWidth * Dpi, path);
                }
            }
        }

        void PaintLoadingWave(Canvas g, GraphicsPath path, Rectangle rect)
        {
            if (loading && LoadingWaveValue > 0)
            {
                using (var brush = new SolidBrush(LoadingWaveColor ?? Colour.Fill.Get(ColorScheme, nameof(Button), Name)))
                {
                    if (LoadingWaveValue >= 1) g.Fill(brush, path);
                    else if (LoadingWaveCount > 0)
                    {
                        var state = g.Save();
                        g.SetClip(path);
                        g.ResetTransform();
                        int len = (int)(LoadingWaveSize * Dpi), count = LoadingWaveCount * 2 + 2;
                        if (count < 6) count = 6;
                        if (LoadingWaveVertical)
                        {
                            int pvalue = (int)(rect.Height * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                pvalue = rect.Height - pvalue + rect.Y;
                                int wd = rect.Width / LoadingWaveCount, wd2 = wd * 2, pvalue2 = pvalue - len, rr = rect.X + wd * count;
                                using (var path_line = new GraphicsPath())
                                {
                                    g.TranslateTransform(-(wd + wd2 * (AnimationLoadingWaveValue / 100F)), 0);
                                    path_line.AddLine(rr, pvalue, rr, rect.Bottom);
                                    path_line.AddLine(rr, rect.Bottom, rect.X, rect.Bottom);
                                    path_line.AddLine(rect.X, rect.Bottom, rect.X, pvalue);
                                    bool to = true;
                                    var line = new List<PointF>(count);
                                    for (int i = 0; i < count + 1; i++)
                                    {
                                        line.Add(new PointF(rect.X + wd * i, to ? pvalue : pvalue2));
                                        to = !to;
                                    }
                                    path_line.AddCurve(line.ToArray());
                                    g.Fill(brush, path_line);
                                }
                            }
                        }
                        else
                        {
                            int pvalue = (int)(rect.Width * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                pvalue += rect.X;
                                int wd = rect.Height / LoadingWaveCount, wd2 = wd * 2, pvalue2 = pvalue + len, rb = rect.Y + wd * count;

                                using (var path_line = new GraphicsPath())
                                {
                                    g.TranslateTransform(0, -(wd + wd2 * (AnimationLoadingWaveValue / 100F)));
                                    path_line.AddLine(pvalue, rb, rect.X, rb);
                                    path_line.AddLine(rect.X, rb, rect.X, rect.Y);
                                    path_line.AddLine(rect.X, rect.Y, pvalue, rect.Y);
                                    bool to = true;
                                    var line = new List<PointF>(count);
                                    for (int i = 0; i < count + 1; i++)
                                    {
                                        line.Add(new PointF(to ? pvalue : pvalue2, rect.Y + wd * i));
                                        to = !to;
                                    }
                                    path_line.AddCurve(line.ToArray());
                                    g.Fill(brush, path_line);
                                }
                            }
                        }
                        g.Restore(state);
                    }
                    else
                    {
                        if (LoadingWaveVertical)
                        {
                            int pvalue = (int)(rect.Height * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                var state = g.Save();
                                g.SetClip(new Rectangle(rect.X, rect.Y + rect.Height - pvalue, rect.Width, pvalue));
                                g.Fill(brush, path);
                                g.Restore(state);
                            }
                        }
                        else
                        {
                            int pvalue = (int)(rect.Width * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                var state = g.Save();
                                g.SetClip(new Rectangle(rect.X, rect.Y, pvalue, rect.Height));
                                g.Fill(brush, path);
                                g.Restore(state);
                            }
                        }
                    }
                }
            }
        }

        void PaintScroll(Canvas g, Rectangle rect_read, float _radius)
        {
            if (autoscroll && (ScrollY.Show || ScrollX.Show))
            {
                int SIZE = (int)(16 * Dpi), SIZE_BAR = (int)(6 * Dpi),
                    SIZE_MINIY = (int)((Config.ScrollMinSizeY ?? SystemInformation.VerticalScrollBarThumbHeight) * Dpi), SIZE_MINIX = (int)((Config.ScrollMinSizeX ?? SystemInformation.HorizontalScrollBarThumbWidth) * Dpi);
                var bg = Colour.TextBase.Get(ColorScheme, nameof(Input), Name);
                if (ScrollX.Show)
                {
                    if (ScrollY.Show)
                    {
                        ScrollX.Rect = new Rectangle(rect_read.X, rect_read.Bottom - SIZE, rect_read.Width - SIZE, SIZE);
                        if (IsPaintScroll())
                        {
                            var color = Color.FromArgb(10, bg);
                            if ((joinMode == TJoinMode.Right || joinMode == TJoinMode.Top) || JoinLeft) g.Fill(color, ScrollX.Rect);
                            else
                            {
                                using (var pathScroll = Helper.RoundPath(ScrollX.Rect, _radius, false, false, false, true))
                                {
                                    g.Fill(color, pathScroll);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScrollX.Rect = new Rectangle(rect_read.X, rect_read.Bottom - SIZE, rect_read.Width, SIZE);
                        if (IsPaintScroll())
                        {
                            var color = Color.FromArgb(10, bg);
                            g.Fill(color, ScrollX.Rect);
                        }
                    }
                    float val = ScrollX.Value, VrValue = ScrollX.Max + ScrollX.Rect.Width, gap = (ScrollX.Rect.Height - SIZE_BAR) / 2F, gap2 = gap * 2, min = SIZE_MINIX + gap2;
                    float widthfull = ((ScrollX.Rect.Width / VrValue) * ScrollX.Rect.Width), width = widthfull - SIZE;
                    if (width < min) width = min;
                    else if (width < SIZE) width = SIZE;
                    float x = val == 0 ? 0 : (val / (VrValue - ScrollX.Rect.Width)) * (ScrollX.Rect.Width - width);
                    ScrollX.Slider = new RectangleF(ScrollX.Rect.X + x + gap, ScrollX.Rect.Y + gap, width - gap2, SIZE_BAR);

                    if (widthfull < SIZE_MINIX) widthfull = SIZE_MINIX;
                    else if (widthfull < SIZE) widthfull = SIZE;
                    ScrollX.SliderFull = widthfull;

                    int alpha = ScrollX.Hover ? 141 : 110;
                    using (var path = ScrollX.Slider.RoundPath(ScrollX.Slider.Width))
                    {
                        g.Fill(Color.FromArgb(alpha, bg), path);
                    }
                }
                if (ScrollY.Show)
                {
                    ScrollY.Rect = new Rectangle(rect_read.Right - SIZE, rect_read.Y, SIZE, rect_read.Height);
                    if (IsPaintScroll())
                    {
                        var color = Color.FromArgb(10, bg);
                        if (joinMode == TJoinMode.Left || JoinRight) g.Fill(color, ScrollY.Rect);
                        else if (joinMode == TJoinMode.Top)
                        {
                            using (var pathScroll = Helper.RoundPath(ScrollY.Rect, _radius, false, true, false, false))
                            {
                                g.Fill(color, pathScroll);
                            }
                        }
                        else if (joinMode == TJoinMode.Bottom)
                        {
                            using (var pathScroll = Helper.RoundPath(ScrollY.Rect, _radius, false, false, true, false))
                            {
                                g.Fill(color, pathScroll);
                            }
                        }
                        else
                        {
                            using (var pathScroll = Helper.RoundPath(ScrollY.Rect, _radius, false, true, true, false))
                            {
                                g.Fill(color, pathScroll);
                            }
                        }
                    }
                    float val = ScrollY.Value, VrValue = ScrollY.Max + ScrollY.Rect.Height, gap = (ScrollY.Rect.Width - SIZE_BAR) / 2F, gap2 = gap * 2, min = SIZE_MINIY + gap2;
                    float heightfull = ((ScrollY.Rect.Height / VrValue) * ScrollY.Rect.Height), height = heightfull - SIZE;
                    if (height < min) height = min;
                    else if (height < SIZE) height = SIZE;
                    float y = val == 0 ? 0 : (val / (VrValue - ScrollY.Rect.Height)) * (ScrollY.Rect.Height - height);
                    ScrollY.Slider = new RectangleF(ScrollY.Rect.X + gap, ScrollY.Rect.Y + y + gap, SIZE_BAR, height - gap2);

                    if (heightfull < SIZE_MINIY) heightfull = SIZE_MINIY;
                    else if (heightfull < SIZE) heightfull = SIZE;
                    ScrollY.SliderFull = heightfull;

                    int alpha = ScrollY.Hover ? 141 : 110;
                    using (var path = ScrollY.Slider.RoundPath(ScrollY.Slider.Width))
                    {
                        g.Fill(Color.FromArgb(alpha, bg), path);
                    }
                }
            }
        }

        bool IsPaintScroll()
        {
            if (Config.ScrollBarHide)
            {
                if (ScrollY.Show && ScrollX.Show) return ScrollY.Hover || ScrollX.Hover;
                return ScrollY.Hover;
            }
            else return true;
        }

        #region 渲染帮助

        void PaintIcon(Canvas g, Color _fore)
        {
            string? prefixText = PrefixText, suffixText = SuffixText;
            if (prefixText != null) g.String(prefixText, Font, prefixFore ?? _fore, rect_l, PrefixFormat);
            else if (prefixSvg != null) g.Svg(prefixSvg, rect_l, prefixFore ?? fore ?? Colour.Text.Get(ColorScheme, nameof(Input), Name));
            else if (prefix != null) g.Image(prefix, rect_l);

            if (loading && LoadingValue > -1) g.PaintLoading(LoadingIcon, LoadingSvg, rect_r, AnimationLoadingValue, LoadingValue, Colour.TextQuaternary.Get(ColorScheme, nameof(Input), Name), CaretInfo.Height, 0.06F);
            else if (is_clear) g.Svg(SvgDb.IcoError, rect_r, hover_clear ? Colour.TextTertiary.Get(ColorScheme, nameof(Input), Name) : Colour.TextQuaternary.Get(ColorScheme, nameof(Input), Name));
            else if (suffixText != null) g.String(suffixText, Font, suffixFore ?? _fore, rect_r, SuffixFormat);
            else if (suffixSvg != null) g.Svg(suffixSvg, rect_r, suffixFore ?? fore ?? Colour.Text.Get(ColorScheme, nameof(Input), Name));
            else if (suffix != null) g.Image(suffix, rect_r);
            else PaintRIcon(g, rect_r);
        }

        void PaintText(Canvas g, Color _fore, int w, int h)
        {
            var state = g.Save();
            if (multiline) g.SetClip(rect_text);
            else if (RECTDIV.HasValue) g.SetClip(RECTDIV.Value);
            else g.SetClip(new Rectangle(rect_text.X, 0, rect_text.Width, Height));
            if (cache_font != null) PaintText(g, _fore, w, h, cache_font);
            else if (ShowPlaceholder && PlaceholderText != null)
            {
                using (var fore = placeholderColorExtend.BrushEx(rect_text, placeholderColor ?? Colour.TextQuaternary.Get(ColorScheme, nameof(Input), Name)))
                {
                    g.DrawText(PlaceholderText, Font, fore, rect_text, IsRTL ? sf_placeholder | FormatFlags.DirectionRightToLeft : sf_placeholder);
                }
            }
            if (CaretInfo.Show && CaretInfo.Flag)
            {
                g.ResetClip();
                if (multiline) g.SetClip(new Rectangle(0, rect_text.Y, w, rect_text.Height));
                else if (RECTDIV.HasValue) g.SetClip(RECTDIV.Value);
                else g.SetClip(new Rectangle(0, 0, w, Height));

                g.TranslateTransform(-ScrollX.Value, -ScrollY.Value);
                using (var brush = new SolidBrush(CaretColor ?? _fore))
                {
                    g.Fill(brush, CaretInfo.Rect);
                }
            }
            g.Restore(state);
        }
        void PaintText(Canvas g, Color _fore, int w, int h, List<CacheFont> cache_font)
        {
            var state = g.Save();
            g.TranslateTransform(-ScrollX.Value, -ScrollY.Value);
            var tmp = PCSText(g, _fore, w, h, cache_font);
            if (styles != null)
            {
                foreach (var it in tmp)
                {
                    if (it.back.HasValue) g.Fill(it.back.Value, it.rect);
                }
            }
            PaintTextSelected(g, cache_font);
            PaintText(g, _fore, cache_font, tmp);
            g.Restore(state);
        }
        List<CacheFont> PCSText(Canvas g, Color _fore, int w, int h, List<CacheFont> cache_font)
        {
            var tmp = new List<CacheFont>(cache_font.Count);
            if (IsPassWord)
            {
                if (ScrollY.Show)
                {
                    foreach (var it in cache_font)
                    {
                        if (it.ret) continue;
                        bool show = it.rect.Y > ScrollY.Value - it.rect.Height && it.rect.Bottom < ScrollY.Value + h + it.rect.Height;
                        if (show) tmp.Add(it);
                    }
                }
                else if (ScrollX.Show)
                {
                    foreach (var it in cache_font)
                    {
                        if (it.ret) continue;
                        bool show = it.rect.X > ScrollX.Value - it.rect.Width && it.rect.Right < ScrollX.Value + w + it.rect.Width;
                        if (show) tmp.Add(it);
                    }
                }
                else
                {
                    foreach (var it in cache_font)
                    {
                        if (it.ret) continue;
                        tmp.Add(it);
                    }
                }
            }
            else
            {
                if (ScrollY.Show)
                {
                    foreach (var it in cache_font)
                    {
                        if (it.hide) continue;
                        bool show = it.rect.Y > ScrollY.Value - it.rect.Height && it.rect.Bottom < ScrollY.Value + h + it.rect.Height;
                        if (show) tmp.Add(it);
                    }
                }
                else if (ScrollX.Show)
                {
                    foreach (var it in cache_font)
                    {
                        if (it.hide) continue;
                        bool show = it.rect.X > ScrollX.Value - it.rect.Width && it.rect.Right < ScrollX.Value + w + it.rect.Width;
                        if (show) tmp.Add(it);
                    }
                }
                else
                {
                    foreach (var it in cache_font)
                    {
                        if (it.hide) continue;
                        tmp.Add(it);
                    }
                }
            }
            return tmp;
        }
        void PaintText(Canvas g, Color _fore, List<CacheFont> cache_font, List<CacheFont> tmp)
        {
            using (var fore = new SolidBrush(_fore))
            {
                if (IsPassWord)
                {
                    foreach (var it in tmp) g.String(PassWordChar, it.font ?? Font, fore, it.rect, sf_center);
                }
                else if (IsRTL)
                {
                    PaintTextRTL(g, tmp, fore, _fore);
                }
                else if (HasEmoji)
                {
                    using (var font = new Font(EmojiFont ?? Config.EmojiFont, Font.Size))
                    {
                        foreach (var it in tmp)
                        {
                            if (it.emoji)
                            {
                                if (SvgDb.Emoji.TryGetValue(it.text, out var svg)) g.Svg(svg, it.rect, fore.Color);
                                else StringEmoji(g, it.text, font, it, fore);
                            }
                            else String(g, Font, it, fore);
                        }
                    }
                }
                else
                {
                    foreach (var it in tmp) String(g, Font, it, fore);
                }
            }
        }
        /// <summary>
        /// RTL 渲染：将连续同样式、非 Emoji 的 RTL（及两侧被 RTL 夹住的中性）字符簇聚合为整段文本一次性绘制，由 GDI+ 完成双向排版与连写整形
        /// </summary>
        void PaintTextRTL(Canvas g, List<CacheFont> tmp, Brush fore, Color fore_color)
        {
            Font? emojiFont = null;
            try
            {
                int i = 0;
                while (i < tmp.Count)
                {
                    var it = tmp[i];
                    if (it.emoji)
                    {
                        if (emojiFont == null) emojiFont = new Font(EmojiFont ?? Config.EmojiFont, Font.Size);
                        if (SvgDb.Emoji.TryGetValue(it.text, out var svg)) g.Svg(svg, it.rect, fore_color);
                        else StringEmoji(g, it.text, emojiFont, it, fore);
                        i++;
                        continue;
                    }
                    if (it.ret || it.dir != TextDirection.RTL)
                    {
                        String(g, Font, it, fore);
                        i++;
                        continue;
                    }
                    // 扩展 run：连续同样式的 RTL/中性簇（Emoji、换行、LTR、跨行、样式变化终止）
                    int end = i;
                    for (int j = i + 1; j < tmp.Count; j++)
                    {
                        var it2 = tmp[j];
                        if (it2.emoji || it2.ret || it2.dir == TextDirection.LTR || it2.rect.Y != it.rect.Y) break;
                        if (it2.font != it.font || it2.fore != it.fore || it2.back != it.back) break;
                        end = j;
                    }
                    // 去掉尾部未被 RTL 夹住的中性簇
                    while (end > i && tmp[end].dir == TextDirection.Neutral) end--;
                    if (end == i)
                    {
                        String(g, Font, it, fore);
                        i++;
                        continue;
                    }
                    var sb = new System.Text.StringBuilder();
                    int minX = int.MaxValue, maxX = int.MinValue;
                    for (int k = i; k <= end; k++)
                    {
                        var c = tmp[k];
                        sb.Append(c.text);
                        if (c.rect.X < minX) minX = c.rect.X;
                        if (c.rect.Right > maxX) maxX = c.rect.Right;
                    }
                    g.String(sb.ToString(), it.font ?? Font, it.fore, fore, Rectangle.FromLTRB(minX, it.rect.Y, maxX, it.rect.Bottom), sf_center_rtl);
                    i = end + 1;
                }
            }
            finally
            {
                if (emojiFont != null) emojiFont.Dispose();
            }
        }
        void PaintTextSelected(Canvas g, List<CacheFont> cache_font)
        {
            if (selectionLength > 0 && cache_font.Count > selectionStartTemp && !BanInput)
            {
                try
                {
                    int start = selectionStartTemp, end = start + selectionLength - 1;
                    if (end > cache_font.Count - 1) end = cache_font.Count - 1;
                    var first = cache_font[start];
                    using (var brush = new SolidBrush(selection))
                    {
                        var list = new Dictionary<int, CacheFont>(6);
                        for (int i = start; i <= end; i++)
                        {
                            var it = cache_font[i];
                            if (it.ret) list.Add(it.line + 1, it);
                            else
                            {
                                if (list.ContainsKey(it.line)) list.Remove(it.line);
                                g.Fill(brush, it.rect);
                            }
                        }
                        foreach (var it in list) g.Fill(brush, it.Value.rect.X, it.Value.rect.Y, it.Value.width, it.Value.rect.Height);
                    }
                }
                catch { }
            }
        }

        void String(Canvas g, Font font, CacheFont cache, Brush brush) => g.String(cache.text, cache.font ?? font, cache.fore, brush, cache.rect, sf_center);
        void StringEmoji(Canvas g, string? text, Font font, CacheFont cache, Brush brush) => g.String(text, cache.font ?? font, cache.fore, brush, cache.rect, sf_center);

        protected virtual void PaintRIcon(Canvas g, Rectangle rect) { }

        protected virtual void PaintOtherBor(Canvas g, Rectangle rect_read, float radius, Color back, Color borderColor, Color borderActive) { }

        #region 点击动画

        internal void PaintClick(Canvas g, GraphicsPath path, Rectangle rect, Color color, float radius)
        {
            if (AnimationFocus)
            {
                if (AnimationFocusValue > 0)
                {
                    using (var path_click = Helper.PathJoin(rect, radius, joinMode, JoinLeft, JoinRight))
                    {
                        path_click.AddPath(path, false);
                        g.Fill(Helper.ToColor(AnimationFocusValue, color), path_click);
                    }
                }
            }
            else if (ExtraMouseDown && WaveSize > 0)
            {
                using (var path_click = Helper.PathJoin(rect, radius, joinMode, JoinLeft, JoinRight))
                {
                    path_click.AddPath(path, false);
                    g.Fill(Color.FromArgb(30, color), path_click);
                }
            }
        }

        #endregion

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Dpi, joinMode, JoinLeft, JoinRight);

        public override GraphicsPath RenderRegion => Helper.PathJoin(ReadRectangle, radius, Dpi, round, joinMode, JoinLeft, JoinRight);

        #endregion

        #endregion
    }
}