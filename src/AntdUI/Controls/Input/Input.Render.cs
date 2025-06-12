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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        #region 渲染

        StringFormat sf_font = Helper.SF_MEASURE_FONT();
        internal StringFormat sf_center = Helper.SF_NoWrap();
        internal StringFormat sf_placeholder = Helper.SF_ALL(lr: StringAlignment.Near);

        internal Action? TakePaint;
        public new void Invalidate()
        {
            if (TakePaint == null) base.Invalidate();
            else TakePaint();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var _rect = ClientRectangle;
            if (_rect.Width > 0 && _rect.Height > 0)
            {
                var g = e.Graphics.High();
                Rectangle rect = _rect.PaddingRect(Padding), rect_read = rect.ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, joinMode, JoinLeft, JoinRight);
                IPaint(g, rect, rect_read);
                this.PaintBadge(g);
                base.OnPaint(e);
            }
        }

        internal void IPaint(Canvas g, Rectangle rect, Rectangle rect_read)
        {
            float _radius = round ? rect_read.Height : radius * Config.Dpi;
            if (backImage != null) g.Image(rect_read, backImage, backFit, _radius, false);
            using (var path = Path(rect_read, _radius))
            {
                Color _back = back ?? Colour.BgContainer.Get("Input", ColorScheme),
                    _fore = fore ?? Colour.Text.Get("Input", ColorScheme),
                   _border = borderColor ?? Colour.BorderColor.Get("Input", ColorScheme),
                   _borderHover = BorderHover ?? Colour.PrimaryHover.Get("Input", ColorScheme),
               _borderActive = BorderActive ?? Colour.Primary.Get("Input", ColorScheme);

                switch (status)
                {
                    case TType.Success:
                        _border = Colour.SuccessBorder.Get("Input", ColorScheme);
                        _borderHover = Colour.SuccessHover.Get("Input", ColorScheme);
                        _borderActive = Colour.Success.Get("Input", ColorScheme);
                        break;
                    case TType.Error:
                        _border = Colour.ErrorBorder.Get("Input", ColorScheme);
                        _borderHover = Colour.ErrorHover.Get("Input", ColorScheme);
                        _borderActive = Colour.Error.Get("Input", ColorScheme);
                        break;
                    case TType.Warn:
                        _border = Colour.WarningBorder.Get("Input", ColorScheme);
                        _borderHover = Colour.WarningHover.Get("Input", ColorScheme);
                        _borderActive = Colour.Warning.Get("Input", ColorScheme);
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
                        var borWidth = borderWidth * Config.Dpi;
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
                    g.Fill(Colour.FillTertiary.Get("Input", ColorScheme), path);
                    PaintIcon(g, Colour.TextQuaternary.Get("Input", ColorScheme));
                    PaintText(g, Colour.TextQuaternary.Get("Input", ColorScheme), rect_read.Right, rect_read.Bottom);
                    PaintOtherBor(g, rect_read, _radius, _back, _border, _borderActive);
                    PaintScroll(g, rect_read, _radius);
                    if (borderWidth > 0) g.Draw(_border, borderWidth * Config.Dpi, path);
                }
            }
        }

        void PaintScroll(Canvas g, Rectangle rect_read, float _radius)
        {
            if (ScrollYShow && autoscroll)
            {
                int SIZE = (int)(16 * Config.Dpi), SIZE_BAR = (int)(6 * Config.Dpi), SIZE_MINIY = (int)(Config.ScrollMinSizeY * Config.Dpi);
                ScrollRect = new Rectangle(rect_read.Right - SIZE, rect_read.Y, SIZE, rect_read.Height);
                if (IsPaintScroll())
                {
                    var color = Color.FromArgb(10, Colour.TextBase.Get("Input", ColorScheme));
                    if (JoinRight) g.Fill(color, ScrollRect);
                    else
                    {
                        using (var pathScroll = Helper.RoundPath(ScrollRect, _radius, false, true, true, false))
                        {
                            g.Fill(color, pathScroll);
                        }
                    }
                }
                float val = scrolly, VrValue = ScrollYMax + ScrollRect.Height, gap = (ScrollRect.Width - SIZE_BAR) / 2, min = SIZE_MINIY + gap * 2;
                float heightfull = ((ScrollRect.Height / VrValue) * ScrollRect.Height), height = heightfull - SIZE;
                if (height < min) height = min;
                else if (height < SIZE) height = SIZE;
                float y = val == 0 ? 0 : (val / (VrValue - ScrollRect.Height)) * (ScrollRect.Height - height);
                ScrollSlider = new RectangleF(ScrollRect.X + gap, ScrollRect.Y + y + gap, SIZE_BAR, height - gap * 2);

                if (heightfull < SIZE_MINIY) heightfull = SIZE_MINIY;
                else if (heightfull < SIZE) heightfull = SIZE;
                ScrollSliderFull = heightfull;

                int alpha = ScrollHover ? 141 : 110;
                using (var path = ScrollSlider.RoundPath(ScrollSlider.Width))
                {
                    g.Fill(Color.FromArgb(alpha, Colour.TextBase.Get("Input", ColorScheme)), path);
                }
            }
        }

        bool IsPaintScroll()
        {
            if (Config.ScrollBarHide) return ScrollHover;
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
            else if (prefixSvg != null) g.GetImgExtend(prefixSvg, rect_l, prefixFore ?? fore ?? Colour.Text.Get("Input", ColorScheme));
            else if (prefix != null) g.Image(prefix, rect_l);

            if (is_clear) g.GetImgExtend(SvgDb.IcoError, rect_r, hover_clear ? Colour.TextTertiary.Get("Input", ColorScheme) : Colour.TextQuaternary.Get("Input", ColorScheme));
            else if (suffixText != null)
            {
                using (var fore = new SolidBrush(suffixFore ?? _fore))
                {
                    g.String(suffixText, Font, fore, rect_r, sf_center);
                }
            }
            else if (suffixSvg != null) g.GetImgExtend(suffixSvg, rect_r, suffixFore ?? fore ?? Colour.Text.Get("Input", ColorScheme));
            else if (suffix != null) g.Image(suffix, rect_r);
            else PaintRIcon(g, rect_r);
        }

        void PaintText(Canvas g, Color _fore, int w, int h)
        {
            if (multiline) g.SetClip(rect_text);
            else if (RECTDIV.HasValue) g.SetClip(RECTDIV.Value);
            else g.SetClip(new Rectangle(rect_text.X, 0, rect_text.Width, Height));
            if (cache_font != null) PaintText(g, _fore, w, h, cache_font);
            else if (PlaceholderText != null && ShowPlaceholder)
            {
                using (var fore = placeholderColorExtend.BrushEx(rect_text, placeholderColor ?? Colour.TextQuaternary.Get("Input", ColorScheme)))
                {
                    g.DrawText(PlaceholderText, Font, fore, rect_text, sf_placeholder);
                }
            }
            g.ResetClip();
            if (CaretInfo.Show && CaretInfo.Flag)
            {
                if (multiline) g.SetClip(new Rectangle(0, rect_text.Y, w, rect_text.Height));
                else if (RECTDIV.HasValue) g.SetClip(RECTDIV.Value);
                else g.SetClip(new Rectangle(0, 0, w, Height));

                g.TranslateTransform(-ScrollX, -ScrollY);
                using (var brush = new SolidBrush(CaretColor ?? _fore))
                {
                    g.Fill(brush, CaretInfo.Rect);
                }
                g.ResetTransform();
                g.ResetClip();
            }
        }
        void PaintText(Canvas g, Color _fore, int w, int h, CacheFont[] cache_font)
        {
            g.TranslateTransform(-ScrollX, -ScrollY);
            if (ScrollYShow)
            {
                var tmp = new List<CacheFont>(cache_font.Length);
                foreach (var it in cache_font)
                {
                    it.show = it.rect.Y > ScrollY - it.rect.Height && it.rect.Bottom < ScrollY + h + it.rect.Height;
                    if (it.show) tmp.Add(it);
                }
                PaintText(g, _fore, cache_font, tmp);
            }
            else if (ScrollXShow)
            {
                var tmp = new List<CacheFont>(cache_font.Length);
                foreach (var it in cache_font)
                {
                    it.show = it.rect.X > ScrollX - it.rect.Width && it.rect.Right < ScrollX + w + it.rect.Width;
                    if (it.show) tmp.Add(it);
                }
                PaintText(g, _fore, cache_font, tmp);
            }
            else PaintText(g, _fore, cache_font, cache_font);
            g.ResetTransform();
        }
        void PaintText(Canvas g, Color _fore, CacheFont[] cache_font, IList<CacheFont> tmp)
        {
            if (styles != null)
            {
                foreach (var it in tmp)
                {
                    if (it.back.HasValue) g.Fill(it.back.Value, it.rect);
                }
            }
            PaintTextSelected(g, cache_font);
            using (var fore = new SolidBrush(_fore))
            {
                if (HasEmoji)
                {
                    using (var font = new Font(EmojiFont, Font.Size))
                    {
                        foreach (var it in tmp)
                        {
                            if (IsPassWord) String(g, PassWordChar, Font, it, fore);
                            else if (it.emoji)
                            {
                                if (SvgDb.Emoji.TryGetValue(it.text, out var svg)) SvgExtend.GetImgExtend(g, svg, it.rect, fore.Color);
                                else StringEmoji(g, it.text, font, it, fore);
                            }
                            else String(g, it.text, Font, it, fore);
                        }
                    }
                }
                else
                {
                    foreach (var it in tmp)
                    {
                        if (it.ret) continue;
                        if (IsPassWord) String(g, PassWordChar, Font, it, fore);
                        else String(g, it.text, Font, it, fore);
                    }
                }
            }
            g.ResetTransform();
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

        void String(Canvas g, string? text, Font font, CacheFont cache, Brush brush)
        {
            if (cache.fore.HasValue) g.String(text, cache.font ?? font, cache.fore.Value, cache.rect, sf_font);
            else g.String(text, cache.font ?? font, brush, cache.rect, sf_font);
        }

        void StringEmoji(Canvas g, string? text, Font font, CacheFont cache, Brush brush)
        {
            var rect = new Rectangle(cache.rect.X - 20, cache.rect.Y - 20, cache.rect.Width + 40, cache.rect.Height + 40);
            if (cache.fore.HasValue) g.String(text, cache.font ?? font, cache.fore.Value, rect, sf_font);
            else g.String(text, cache.font ?? font, brush, rect, sf_font);
        }

        protected virtual void PaintRIcon(Canvas g, Rectangle rect) { }

        protected virtual void PaintOtherBor(Canvas g, RectangleF rect_read, float radius, Color back, Color borderColor, Color borderActive) { }

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

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, joinMode, JoinLeft, JoinRight);

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = round ? rect_read.Height : radius * Config.Dpi;
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

        #region 滚动条

        Rectangle ScrollRect;
        RectangleF ScrollSlider;
        float ScrollSliderFull;
        bool scrollhover = false;
        bool ScrollHover
        {
            get => scrollhover;
            set
            {
                if (scrollhover == value) return;
                scrollhover = value;
                Invalidate();
            }
        }
        int scrollx = 0, scrolly = 0, ScrollXMin = 0, ScrollXMax = 0, ScrollYMax = 0;
        int ScrollX
        {
            get => scrollx;
            set
            {
                if (value > ScrollXMax) value = ScrollXMax;
                if (value < ScrollXMin) value = ScrollXMin;
                if (scrollx == value) return;
                scrollx = value;
                Invalidate();
                if (SyncScrollObj is Input input) input.ScrollX = scrollx;
            }
        }
        int ScrollY
        {
            get => scrolly;
            set
            {
                if (value > ScrollYMax) value = ScrollYMax;
                if (value < 0) value = 0;
                if (scrolly == value) return;
                scrolly = value;
                CaretInfo.flag = true;
                Invalidate();
                if (SyncScrollObj is Input input) input.ScrollY = scrolly;
            }
        }

        bool ScrollXShow = false, ScrollYShow = false, ScrollYDown = false;
        void ScrollIFTo(Rectangle r)
        {
            if (SpeedScrollTo)
            {
                if (ScrollYShow)
                {
                    int y = CaretInfo.Y - scrolly;
                    if (y < rect_text.Y) ScrollY = r.Y;
                    else if (y + CaretInfo.Height > rect_text.Height) ScrollY = r.Bottom;
                }
                else if (ScrollXShow)
                {
                    int x = CaretInfo.X - scrollx;
                    if (x < rect_text.X) ScrollX = r.X;
                    else if (x + CaretInfo.Width > rect_text.Width) ScrollX = r.Right;
                }
                else ScrollX = ScrollY = 0;
            }
            ITask.Run(() => ScrollTo(r));
        }
        void ScrollTo(Rectangle r)
        {
            if (ScrollYShow)
            {
                int tosize = CaretInfo.Height;
                int count = 0;
                while (true)
                {
                    int y = CaretInfo.Y - scrolly;
                    if (y < rect_text.Y)
                    {
                        int value = ScrollY - tosize;
                        ScrollY = value;
                        if (ScrollY != value) return;
                        count++;
                        SleepGear(count);
                    }
                    else if (y + CaretInfo.Height > rect_text.Bottom)
                    {
                        int value = ScrollY + tosize;
                        ScrollY = value;
                        if (ScrollY != value) return;
                        count++;
                        SleepGear(count);
                    }
                    else return;
                }
            }
            else if (ScrollXShow)
            {
                int tosize = r.Width;
                int count = 0;
                while (true)
                {
                    int x = CaretInfo.X - scrollx;
                    if (x < rect_text.X)
                    {
                        int value = ScrollX - tosize;
                        ScrollX = value;
                        if (ScrollX != value) return;
                        count++;
                        SleepGear(count);
                    }
                    else if (x + CaretInfo.Width > rect_text.Right)
                    {
                        int value = ScrollX + tosize;
                        ScrollX = value;
                        if (ScrollX != value) return;
                        count++;
                        SleepGear(count);
                    }
                    else return;
                }
            }
            else ScrollX = ScrollY = 0;
        }

        void SleepGear(int count)
        {
            if (count > 7) System.Threading.Thread.Sleep(1);
            else if (count > 5) System.Threading.Thread.Sleep(10);
            else if (count > 3) System.Threading.Thread.Sleep(30);
            else System.Threading.Thread.Sleep(50);
        }

        object? SyncScrollObj;
        public Input SyncScroll(Input input)
        {
            SyncScrollObj = input;
            return this;
        }

        #endregion
    }
}