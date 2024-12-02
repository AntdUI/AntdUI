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
// GITEE: https://gitee.com/antdui/AntdUI
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
                Rectangle rect = _rect.PaddingRect(Padding), rect_read = rect.ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, JoinLeft, JoinRight);
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
                Color _back = back ?? Colour.BgContainer.Get("Input"),
                    _fore = fore ?? Colour.Text.Get("Input"),
                   _border = borderColor ?? Colour.BorderColor.Get("Input"),
                   _borderHover = BorderHover ?? Colour.PrimaryHover.Get("Input"),
               _borderActive = BorderActive ?? Colour.Primary.Get("Input");

                switch (status)
                {
                    case TType.Success:
                        _border = Colour.SuccessBorder.Get("Input");
                        _borderHover = Colour.SuccessHover.Get("Input");
                        _borderActive = Colour.Success.Get("Input");
                        break;
                    case TType.Error:
                        _border = Colour.ErrorBorder.Get("Input");
                        _borderHover = Colour.ErrorHover.Get("Input");
                        _borderActive = Colour.Error.Get("Input");
                        break;
                    case TType.Warn:
                        _border = Colour.WarningBorder.Get("Input");
                        _borderHover = Colour.WarningHover.Get("Input");
                        _borderActive = Colour.Warning.Get("Input");
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
                        if (AnimationHover) g.Draw(_border.BlendColors(AnimationHoverValue, _borderHover), borWidth, path);
                        else if (ExtraMouseDown) g.Draw(_borderActive, borWidth, path);
                        else if (ExtraMouseHover) g.Draw(_borderHover, borWidth, path);
                        else g.Draw(_border, borWidth, path);
                    }
                }
                else
                {
                    g.Fill(Colour.FillTertiary.Get("Input"), path);
                    PaintIcon(g, Colour.TextQuaternary.Get("Input"));
                    PaintText(g, Colour.TextQuaternary.Get("Input"), rect_read.Right, rect_read.Bottom);
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
                int SIZE = 20;

                ScrollRect = new Rectangle(rect_read.Right - SIZE, rect_read.Y, SIZE, rect_read.Height);
                var color = Color.FromArgb(10, Colour.TextBase.Get("Input"));
                if (JoinRight) g.Fill(color, ScrollRect);
                else
                {
                    using (var pathScroll = Helper.RoundPath(ScrollRect, _radius, false, true, true, false))
                    {
                        g.Fill(color, pathScroll);
                    }
                }

                float val = scrolly, VrValue = ScrollYMax + ScrollRect.Height;
                float height = ((ScrollRect.Height / VrValue) * ScrollRect.Height) - 20;
                if (height < SIZE) height = SIZE;
                float y = val == 0 ? 0 : (val / (VrValue - ScrollRect.Height)) * (ScrollRect.Height - height);
                if (ScrollHover) ScrollSlider = new RectangleF(ScrollRect.X + 6, ScrollRect.Y + y, 8, height);
                else ScrollSlider = new RectangleF(ScrollRect.X + 7, ScrollRect.Y + y, 6, height);
                if (ScrollSlider.Y < 10) ScrollSlider.Y = 10;
                else if (ScrollSlider.Y > ScrollRect.Height - height - 6) ScrollSlider.Y = ScrollRect.Height - height - 6;
                using (var path = ScrollSlider.RoundPath(ScrollSlider.Width))
                {
                    g.Fill(Color.FromArgb(141, Colour.TextBase.Get("Input")), path);
                }
            }
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
            else if (prefixSvg != null) g.GetImgExtend(prefixSvg, rect_l, prefixFore ?? fore ?? Colour.Text.Get("Input"));
            else if (prefix != null) g.Image(prefix, rect_l);

            if (is_clear) g.GetImgExtend(SvgDb.IcoError, rect_r, hover_clear ? Colour.TextTertiary.Get("Input") : Colour.TextQuaternary.Get("Input"));
            else if (suffixText != null)
            {
                using (var fore = new SolidBrush(suffixFore ?? _fore))
                {
                    g.String(suffixText, Font, fore, rect_r, sf_center);
                }
            }
            else if (suffixSvg != null) g.GetImgExtend(suffixSvg, rect_r, suffixFore ?? fore ?? Colour.Text.Get("Input"));
            else if (suffix != null) g.Image(suffix, rect_r);
            else PaintRIcon(g, rect_r);
        }

        void PaintText(Canvas g, Color _fore, int w, int h)
        {
            if (multiline) g.SetClip(rect_text);
            else if (RECTDIV.HasValue) g.SetClip(RECTDIV.Value);
            else g.SetClip(new Rectangle(rect_text.X, 0, rect_text.Width, Height));
            if (cache_font != null)
            {
                g.TranslateTransform(-ScrollX, -ScrollY);
                PaintTextSelected(g, cache_font);
                using (var fore = new SolidBrush(_fore))
                {
                    if (HasEmoji)
                    {
                        using (var font = new Font(EmojiFont, Font.Size))
                        {
                            foreach (var it in cache_font)
                            {
                                it.show = it.rect.Y > ScrollY - it.rect.Height && it.rect.Bottom < ScrollY + h + it.rect.Height;
                                if (it.show)
                                {
                                    if (IsPassWord) g.String(PassWordChar, Font, fore, it.rect, sf_font);
                                    else if (it.emoji) g.String(it.text, font, fore, it.rect, sf_font);
                                    else g.String(it.text, Font, fore, it.rect, sf_font);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in cache_font)
                        {
                            it.show = it.rect.Y > ScrollY - it.rect.Height && it.rect.Bottom < ScrollY + h + it.rect.Height;
                            if (it.show)
                            {
                                if (IsPassWord) g.String(PassWordChar, Font, fore, it.rect, sf_font);
                                else g.String(it.text, Font, fore, it.rect, sf_font);
                            }
                        }
                    }
                }
                g.ResetTransform();
            }
            else if (PlaceholderText != null && ShowPlaceholder)
            {
                using (var fore = placeholderColorExtend.BrushEx(rect_text, placeholderColor ?? Colour.TextQuaternary.Get("Input")))
                {
                    g.String(PlaceholderText, Font, fore, rect_text, sf_placeholder);
                }
            }
            g.ResetClip();
            if (CaretInfo.Show && CaretInfo.Flag)
            {
                g.TranslateTransform(-ScrollX, -ScrollY);
                using (var brush = new SolidBrush(CaretColor ?? _fore))
                {
                    g.Fill(brush, CaretInfo.Rect);
                }
                g.ResetTransform();
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

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, JoinLeft, JoinRight);
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = round ? rect_read.Height : radius * Config.Dpi;
                return Path(rect_read, _radius);
            }
        }

        internal GraphicsPath Path(RectangleF rect_read, float _radius)
        {
            if (JoinLeft && JoinRight) return rect_read.RoundPath(0);
            else if (JoinRight) return rect_read.RoundPath(_radius, true, false, false, true);
            else if (JoinLeft) return rect_read.RoundPath(_radius, false, true, true, false);
            return rect_read.RoundPath(_radius);
        }

        #endregion

        #endregion

        #region 滚动条

        Rectangle ScrollRect;
        RectangleF ScrollSlider;
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
            ITask.Run(() => { ScrollTo(r); });
        }
        void ScrollTo(Rectangle r)
        {
            if (ScrollYShow)
            {
                int tosize = CaretInfo.Height;
                int count = 0;
                var oldy = new List<int>(2);
                while (true)
                {
                    int y = CaretInfo.Y - scrolly;
                    oldy.Add(y);
                    if (oldy.Count > 1)
                    {
                        if (oldy.Contains(y)) return;
                        else oldy.Clear();
                    }
                    if (y < rect_text.Y)
                    {
                        int value = ScrollY - tosize;
                        ScrollY = value;
                        if (ScrollY != value) return;
                        count++;
                        if (count < 4) System.Threading.Thread.Sleep(50);
                    }
                    else if (y + CaretInfo.Height > rect_text.Height)
                    {
                        int value = ScrollY + tosize;
                        ScrollY = value;
                        if (ScrollY != value) return;
                        count++;
                        if (count < 4) System.Threading.Thread.Sleep(50);
                    }
                    else return;
                }
            }
            else if (ScrollXShow)
            {
                int tosize = r.Width;
                int count = 0;
                var oldx = new List<int>(2);
                while (true)
                {
                    int x = CaretInfo.X - scrollx;
                    oldx.Add(x);
                    if (oldx.Count > 1)
                    {
                        if (oldx.Contains(x)) return;
                        else oldx.Clear();
                    }
                    if (x < rect_text.X)
                    {
                        int value = ScrollX - tosize;
                        ScrollX = value;
                        if (ScrollX != value) return;
                        count++;
                        if (count < 5) System.Threading.Thread.Sleep(50);
                    }
                    else if (x + CaretInfo.Width > rect_text.Width)
                    {
                        int value = ScrollX + tosize;
                        ScrollX = value;
                        if (ScrollX != value) return;
                        count++;
                        if (count < 5) System.Threading.Thread.Sleep(50);
                    }
                    else return;
                }

            }
            else ScrollX = ScrollY = 0;
        }

        #endregion
    }
}