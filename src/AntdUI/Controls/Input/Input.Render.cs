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
                Rectangle rect = _rect.PaddingRect(Padding), rect_read = rect.ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, JoinLeft, JoinRight);
                IPaint(e.Graphics.High(), rect, rect_read);
            }
            base.OnPaint(e);
        }

        internal void IPaint(Graphics g, Rectangle rect, Rectangle rect_read)
        {
            float _radius = round ? rect_read.Height : radius * Config.Dpi;
            bool enabled = Enabled;
            if (backImage != null) g.PaintImg(rect_read, backImage, backFit, _radius, false);
            using (var path = Path(rect_read, _radius))
            {
                Color _back = back ?? Style.Db.BgContainer,
                    _fore = fore ?? Style.Db.Text,
                   _border = borderColor ?? Style.Db.BorderColor,
                   _borderHover = BorderHover ?? Style.Db.PrimaryHover,
               _borderActive = BorderActive ?? Style.Db.Primary;

                switch (status)
                {
                    case TType.Success:
                        _border = Style.Db.SuccessBorder;
                        _borderHover = Style.Db.SuccessHover;
                        _borderActive = Style.Db.Success;
                        break;
                    case TType.Error:
                        _border = Style.Db.ErrorBorder;
                        _borderHover = Style.Db.ErrorHover;
                        _borderActive = Style.Db.Error;
                        break;
                    case TType.Warn:
                        _border = Style.Db.WarningBorder;
                        _borderHover = Style.Db.WarningHover;
                        _borderActive = Style.Db.Warning;
                        break;
                }

                PaintClick(g, path, rect, _borderActive, _radius);

                if (enabled)
                {
                    using (var brush = backExtend.BrushEx(rect_read, _back))
                    {
                        g.FillPath(brush, path);
                    }
                    PaintIcon(g, _fore);
                    PaintText(g, _fore, rect_read.Right, rect_read.Bottom);
                    g.ResetClip();
                    PaintOtherBor(g, rect_read, _radius, _back, _border, _borderActive);
                    PaintScroll(g, rect_read, _radius);
                    if (borderWidth > 0)
                    {
                        if (AnimationHover)
                        {
                            using (var brush = new Pen(_border, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                            using (var brush = new Pen(Helper.ToColor(AnimationHoverValue, _borderHover), borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                        else if (ExtraMouseDown)
                        {
                            using (var brush = new Pen(_borderActive, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                        else if (ExtraMouseHover)
                        {
                            using (var brush = new Pen(_borderHover, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                        else
                        {
                            using (var brush = new Pen(_border, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(Style.Db.FillTertiary))
                    {
                        g.FillPath(brush, path);
                    }
                    PaintIcon(g, Style.Db.TextQuaternary);
                    PaintText(g, Style.Db.TextQuaternary, rect_read.Right, rect_read.Bottom);
                    g.ResetClip();
                    PaintOtherBor(g, rect_read, _radius, _back, _border, _borderActive);
                    PaintScroll(g, rect_read, _radius);
                    if (borderWidth > 0)
                    {
                        using (var brush = new Pen(_border, borderWidth))
                        {
                            g.DrawPath(brush, path);
                        }
                    }
                }
            }
        }

        void PaintScroll(Graphics g, Rectangle rect_read, float _radius)
        {
            if (ScrollYShow && autoscroll)
            {
                int SIZE = 20;

                ScrollRect = new Rectangle(rect_read.Right - SIZE, rect_read.Y, SIZE, rect_read.Height);
                using (var brush = new SolidBrush(Color.FromArgb(10, Style.Db.TextBase)))
                {
                    if (JoinRight) g.FillRectangle(brush, ScrollRect);
                    else
                    {
                        using (var pathScroll = Helper.RoundPath(ScrollRect, _radius, false, true, true, false))
                        {
                            g.FillPath(brush, pathScroll);
                        }
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
                using (var brush = new SolidBrush(Color.FromArgb(141, Style.Db.TextBase)))
                {
                    using (var path = ScrollSlider.RoundPath(ScrollSlider.Width))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
        }

        #region 渲染帮助

        void PaintIcon(Graphics g, Color _fore)
        {
            if (prefixText != null)
            {
                using (var fore = new SolidBrush(_fore))
                {
                    g.DrawStr(prefixText, Font, fore, rect_l, sf_center);
                }
            }
            else if (prefixSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(prefixSvg, rect_l, fore ?? Style.Db.Text))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_l);
                }
            }
            else if (prefix != null) g.DrawImage(prefix, rect_l);

            if (is_clear)
            {
                using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoError, rect_r, hover_clear ? Style.Db.TextTertiary : Style.Db.TextQuaternary))
                {
                    if (bmp != null) g.DrawImage(bmp, rect_r);
                }
            }
            else if (suffixText != null)
            {
                using (var fore = new SolidBrush(_fore))
                {
                    g.DrawStr(suffixText, Font, fore, rect_r, sf_center);
                }
            }
            else if (suffixSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(suffixSvg, rect_r, fore ?? Style.Db.Text))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_r);
                }
            }
            else if (suffix != null) g.DrawImage(suffix, rect_r);
            else PaintRIcon(g, rect_r);
        }

        void PaintText(Graphics g, Color _fore, int w, int h)
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
                                    if (IsPassWord) g.DrawStr(PassWordChar, Font, fore, it.rect, sf_font);
                                    else if (it.emoji) g.DrawStr(it.text, font, fore, it.rect, sf_font);
                                    else g.DrawStr(it.text, Font, fore, it.rect, sf_font);
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
                                if (IsPassWord) g.DrawStr(PassWordChar, Font, fore, it.rect, sf_font);
                                else g.DrawStr(it.text, Font, fore, it.rect, sf_font);
                            }
                        }
                    }
                }
                g.ResetTransform();
            }
            else if (placeholderText != null && ShowPlaceholder)
            {
                using (var fore = placeholderColorExtend.BrushEx(rect_text, placeholderColor ?? Style.Db.TextQuaternary))
                {
                    g.DrawStr(placeholderText, Font, fore, rect_text, sf_placeholder);
                }
            }

            if (showCaret && showCaretFlag)
            {
                g.TranslateTransform(-ScrollX, -ScrollY);
                using (var brush = new SolidBrush(CaretColor ?? _fore))
                {
                    g.FillRectangle(brush, CurrentCaret);
                }
                g.ResetTransform();
            }
        }
        void PaintTextSelected(Graphics g, CacheFont[] cache_font)
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
                        for (int i = start; i <= end; i++)
                        {
                            var it = cache_font[i];
                            bool p = true;
                            if (it.ret && it.ret_has) p = false;
                            if (p) g.FillRectangle(brush, it.rect);
                        }
                    }
                }
                catch { }
            }
        }

        protected virtual void PaintRIcon(Graphics g, Rectangle rect) { }

        protected virtual void PaintOtherBor(Graphics g, RectangleF rect_read, float radius, Color back, Color borderColor, Color borderActive) { }

        #region 点击动画

        internal void PaintClick(Graphics g, GraphicsPath path, Rectangle rect, Color color, float radius)
        {
            if (AnimationFocus)
            {
                if (AnimationFocusValue > 0)
                {
                    using (var brush = new SolidBrush(Helper.ToColor(AnimationFocusValue, color)))
                    {
                        using (var path_click = Helper.RoundPath(rect, radius, round))
                        {
                            path_click.AddPath(path, false);
                            g.FillPath(brush, path_click);
                        }
                    }
                }
            }
            else if (ExtraMouseDown && WaveSize > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(30, color)))
                {
                    using (var path_click = Helper.RoundPath(rect, radius, round))
                    {
                        path_click.AddPath(path, false);
                        g.FillPath(brush, path_click);
                    }
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
                showCaretFlag = true;
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
                    int y = CurrentCaret.Y - scrolly;
                    if (y < rect_text.Y) ScrollY = r.Y;
                    else if (y + CurrentCaret.Height > rect_text.Height) ScrollY = r.Bottom;
                }
                else if (ScrollXShow)
                {
                    int x = CurrentCaret.X - scrollx;
                    if (x < rect_text.X) ScrollX = r.X;
                    else if (x + CurrentCaret.Width > rect_text.Width) ScrollX = r.Right;
                }
                else ScrollX = ScrollY = 0;
            }
            ITask.Run(() => { ScrollTo(r); });
        }
        void ScrollTo(Rectangle r)
        {
            if (ScrollYShow)
            {
                int tosize = CurrentCaret.Height;
                while (true)
                {
                    int y = CurrentCaret.Y - scrolly;
                    if (y < rect_text.Y)
                    {
                        int value = ScrollY - tosize;
                        ScrollY = value;
                        if (ScrollY != value) return;
                        System.Threading.Thread.Sleep(50);
                    }
                    else if (y + CurrentCaret.Height > rect_text.Height)
                    {
                        int value = ScrollY + tosize;
                        ScrollY = value;
                        if (ScrollY != value) return;
                        System.Threading.Thread.Sleep(50);
                    }
                    else return;
                }
            }
            else if (ScrollXShow)
            {
                int tosize = r.Width;
                while (true)
                {
                    int x = CurrentCaret.X - scrollx;
                    if (x < rect_text.X)
                    {
                        int value = ScrollX - tosize;
                        ScrollX = value;
                        if (ScrollX != value) return;
                        System.Threading.Thread.Sleep(50);
                    }
                    else if (x + CurrentCaret.Width > rect_text.Width)
                    {
                        int value = ScrollX + tosize;
                        ScrollX = value;
                        if (ScrollX != value) return;
                        System.Threading.Thread.Sleep(50);
                    }
                    else return;
                }
            }
            else ScrollX = ScrollY = 0;
        }

        #endregion
    }
}