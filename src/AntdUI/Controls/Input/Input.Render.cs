// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class Input
    {
        #region 渲染

        internal FormatFlags sf_center = FormatFlags.Center | FormatFlags.NoWrap;
        internal FormatFlags sf_placeholder = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;

        internal Action? TakePaint;
        public new void Invalidate()
        {
            if (TakePaint == null) base.Invalidate();
            else TakePaint();
        }
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
            using (var path = Path(rect_read, _radius))
            {
                Color _back = back ?? Colour.BgContainer.Get(nameof(Input), ColorScheme),
                    _fore = fore ?? Colour.Text.Get(nameof(Input), ColorScheme),
                   _border = borderColor ?? Colour.BorderColor.Get(nameof(Input), ColorScheme),
                   _borderHover = BorderHover ?? Colour.PrimaryHover.Get(nameof(Input), ColorScheme),
               _borderActive = BorderActive ?? Colour.Primary.Get(nameof(Input), ColorScheme);

                switch (status)
                {
                    case TType.Success:
                        _border = Colour.SuccessBorder.Get(nameof(Input), ColorScheme);
                        _borderHover = Colour.SuccessHover.Get(nameof(Input), ColorScheme);
                        _borderActive = Colour.Success.Get(nameof(Input), ColorScheme);
                        break;
                    case TType.Error:
                        _border = Colour.ErrorBorder.Get(nameof(Input), ColorScheme);
                        _borderHover = Colour.ErrorHover.Get(nameof(Input), ColorScheme);
                        _borderActive = Colour.Error.Get(nameof(Input), ColorScheme);
                        break;
                    case TType.Warn:
                        _border = Colour.WarningBorder.Get(nameof(Input), ColorScheme);
                        _borderHover = Colour.WarningHover.Get(nameof(Input), ColorScheme);
                        _borderActive = Colour.Warning.Get(nameof(Input), ColorScheme);
                        break;
                }

                PaintClick(g, path, rect, _borderActive, _radius);

                if (Enabled)
                {
                    using (var brush = backExtend.BrushEx(rect_read, _back))
                    {
                        g.Fill(brush, path);
                    }
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
                    g.Fill(Colour.FillTertiary.Get(nameof(Input), "bgDisabled", ColorScheme), path);
                    var fore = Colour.TextQuaternary.Get(nameof(Input), "foreDisabled", ColorScheme);
                    PaintIcon(g, fore);
                    PaintText(g, fore, rect_read.Right, rect_read.Bottom);
                    PaintOtherBor(g, rect_read, _radius, _back, _border, _borderActive);
                    PaintScroll(g, rect_read, _radius);
                    if (borderWidth > 0) g.Draw(_border, borderWidth * Dpi, path);
                }
            }
        }

        void PaintScroll(Canvas g, Rectangle rect_read, float _radius)
        {
            if (autoscroll && (ScrollY.Show || ScrollX.Show))
            {
                int SIZE = (int)(16 * Dpi), SIZE_BAR = (int)(6 * Dpi), SIZE_MINIY = (int)(Config.ScrollMinSizeY * Dpi);
                var bg = Colour.TextBase.Get(nameof(Input), ColorScheme);
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
                    float val = ScrollX.Value, VrValue = ScrollX.Max + ScrollX.Rect.Width, gap = (ScrollX.Rect.Height - SIZE_BAR) / 2, min = SIZE_MINIY + gap * 2;
                    float widthfull = ((ScrollX.Rect.Width / VrValue) * ScrollX.Rect.Width), width = widthfull - SIZE;
                    if (width < min) width = min;
                    else if (width < SIZE) width = SIZE;
                    float x = val == 0 ? 0 : (val / (VrValue - ScrollX.Rect.Width)) * (ScrollX.Rect.Width - width);
                    ScrollX.Slider = new RectangleF(ScrollX.Rect.X + x + gap, ScrollX.Rect.Y + gap, width - gap * 2, SIZE_BAR);

                    if (widthfull < SIZE_MINIY) widthfull = SIZE_MINIY;
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
                    float val = ScrollY.Value, VrValue = ScrollY.Max + ScrollY.Rect.Height, gap = (ScrollY.Rect.Width - SIZE_BAR) / 2, min = SIZE_MINIY + gap * 2;
                    float heightfull = ((ScrollY.Rect.Height / VrValue) * ScrollY.Rect.Height), height = heightfull - SIZE;
                    if (height < min) height = min;
                    else if (height < SIZE) height = SIZE;
                    float y = val == 0 ? 0 : (val / (VrValue - ScrollY.Rect.Height)) * (ScrollY.Rect.Height - height);
                    ScrollY.Slider = new RectangleF(ScrollY.Rect.X + gap, ScrollY.Rect.Y + y + gap, SIZE_BAR, height - gap * 2);

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
            if (prefixText != null)
            {
                using (var fore = new SolidBrush(prefixFore ?? _fore))
                {
                    g.String(prefixText, Font, fore, rect_l, sf_center);
                }
            }
            else if (prefixSvg != null) g.GetImgExtend(prefixSvg, rect_l, prefixFore ?? fore ?? Colour.Text.Get(nameof(Input), ColorScheme));
            else if (prefix != null) g.Image(prefix, rect_l);

            if (is_clear) g.GetImgExtend(SvgDb.IcoError, rect_r, hover_clear ? Colour.TextTertiary.Get(nameof(Input), ColorScheme) : Colour.TextQuaternary.Get(nameof(Input), ColorScheme));
            else if (suffixText != null)
            {
                using (var fore = new SolidBrush(suffixFore ?? _fore))
                {
                    g.String(suffixText, Font, fore, rect_r, sf_center);
                }
            }
            else if (suffixSvg != null) g.GetImgExtend(suffixSvg, rect_r, suffixFore ?? fore ?? Colour.Text.Get(nameof(Input), ColorScheme));
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
            else if (PlaceholderText != null && ShowPlaceholder)
            {
                using (var fore = placeholderColorExtend.BrushEx(rect_text, placeholderColor ?? Colour.TextQuaternary.Get(nameof(Input), ColorScheme)))
                {
                    g.DrawText(PlaceholderText, Font, fore, rect_text, sf_placeholder);
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
        void PaintText(Canvas g, Color _fore, int w, int h, CacheFont[] cache_font)
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
        List<CacheFont> PCSText(Canvas g, Color _fore, int w, int h, CacheFont[] cache_font)
        {
            var tmp = new List<CacheFont>(cache_font.Length);
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
        void PaintText(Canvas g, Color _fore, CacheFont[] cache_font, List<CacheFont> tmp)
        {
            using (var fore = new SolidBrush(_fore))
            {
                if (IsPassWord)
                {
                    foreach (var it in tmp) g.String(PassWordChar, it.font ?? Font, fore, it.rect);
                }
                else if (HasEmoji)
                {
                    using (var font = new Font(EmojiFont, Font.Size))
                    {
                        foreach (var it in tmp)
                        {
                            if (it.emoji)
                            {
                                if (SvgDb.Emoji.TryGetValue(it.text, out var svg)) SvgExtend.GetImgExtend(g, svg, it.rect, fore.Color);
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
        void PaintTextSelected(Canvas g, CacheFont[] cache_font)
        {
            if (selectionLength > 0 && cache_font.Length > selectionStartTemp && !BanInput)
            {
                try
                {
                    int start = selectionStartTemp, end = start + selectionLength - 1;
                    if (end > cache_font.Length - 1) end = cache_font.Length - 1;
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

        void String(Canvas g, Font font, CacheFont cache, Brush brush)
        {
            if (cache.fore.HasValue) g.String(cache.text, cache.font ?? font, cache.fore.Value, cache.rect);
            else g.String(cache.text, cache.font ?? font, brush, cache.rect);
        }

        void StringEmoji(Canvas g, string? text, Font font, CacheFont cache, Brush brush)
        {
            var rect = new Rectangle(cache.rect.X - 20, cache.rect.Y - 20, cache.rect.Width + 40, cache.rect.Height + 40);
            if (cache.fore.HasValue) g.String(text, cache.font ?? font, cache.fore.Value, rect);
            else g.String(text, cache.font ?? font, brush, rect);
        }

        protected virtual void PaintRIcon(Canvas g, Rectangle rect) { }

        protected virtual void PaintOtherBor(Canvas g, Rectangle rect_read, float radius, Color back, Color borderColor, Color borderActive) { }

        #region 点击动画

        internal void PaintClick(Canvas g, GraphicsPath path, Rectangle rect, Color color, float radius)
        {
            if (AnimationFocus)
            {
                if (AnimationFocusValue > 0)
                {
                    using (var path_click = Helper.RoundPath(rect, radius, round))
                    {
                        path_click.AddPath(path, false);
                        g.Fill(Helper.ToColor(AnimationFocusValue, color), path_click);
                    }
                }
            }
            else if (ExtraMouseDown && WaveSize > 0)
            {
                using (var path_click = Helper.RoundPath(rect, radius, round))
                {
                    path_click.AddPath(path, false);
                    g.Fill(Color.FromArgb(30, color), path_click);
                }
            }
        }

        #endregion

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Dpi, joinMode, JoinLeft, JoinRight);

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = round ? rect_read.Height : radius * Dpi;
                return Path(rect_read, _radius);
            }
        }

        internal GraphicsPath Path(RectangleF rect, float radius)
        {
            switch (joinMode)
            {
                case TJoinMode.Left:
                    return rect.RoundPath(radius, true, false, false, true);
                case TJoinMode.Right:
                    return rect.RoundPath(radius, false, true, true, false);
                case TJoinMode.LR:
                case TJoinMode.TB:
                    return rect.RoundPath(0);
                case TJoinMode.Top:
                    return rect.RoundPath(radius, true, true, false, false);
                case TJoinMode.Bottom:
                    return rect.RoundPath(radius, false, false, true, true);
                case TJoinMode.None:
                default:
                    if (JoinLeft && JoinRight) return rect.RoundPath(0);
                    else if (JoinLeft) return rect.RoundPath(radius, false, true, true, false);
                    else if (JoinRight) return rect.RoundPath(radius, true, false, false, true);
                    return rect.RoundPath(radius);
            }
        }

        #endregion

        #endregion
    }
}