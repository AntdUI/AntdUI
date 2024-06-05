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

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            if (rect_read.Width > 0 && rect_read.Height > 0)
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

                    PaintClick(g, path, _borderActive, _radius);

                    if (enabled)
                    {
                        using (var brush = new SolidBrush(_back))
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
                                using (var brush = new Pen(Color.FromArgb(AnimationHoverValue, _borderHover), borderWidth))
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
            base.OnPaint(e);
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
                    g.DrawString(prefixText, Font, fore, rect_l, sf_center);
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
                using (var brush = new SolidBrush(hover_clear ? Style.Db.TextTertiary : Style.Db.TextQuaternary))
                {
                    g.FillEllipse(brush, rect_r);
                }
                g.PaintIconError(rect_r, Style.Db.BgBase);
            }
            else if (suffixText != null)
            {
                using (var fore = new SolidBrush(_fore))
                {
                    g.DrawString(suffixText, Font, fore, rect_r, sf_center);
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
            else g.SetClip(new Rectangle(rect_text.X, 0, rect_text.Width, Height));
            if (cache_font != null)
            {
                g.TranslateTransform(-ScrollX, -ScrollY);
                if (selectionLength > 0 && cache_font.Length > selectionStartTemp)
                {
                    try
                    {
                        int end = selectionStartTemp + selectionLength - 1;
                        if (end > cache_font.Length - 1) end = cache_font.Length - 1;
                        var first = cache_font[selectionStartTemp];
                        using (var brush = new SolidBrush(selection))
                        {
                            for (int i = selectionStartTemp; i <= end; i++)
                            {
                                var last = cache_font[i];
                                if (first.rect.Y != last.rect.Y || last.retun > 0)
                                {
                                    //先渲染上一行
                                    if (i > 0) g.FillRectangle(brush, new Rectangle(first.rect.X, first.rect.Y, cache_font[i - 1].rect.Right - first.rect.X, first.rect.Height));
                                    if (i == end) g.FillRectangle(brush, last.rect);
                                    first = last;
                                }
                                else if (i == end) g.FillRectangle(brush, new Rectangle(first.rect.X, first.rect.Y, last.rect.Right - first.rect.X, first.rect.Height));
                            }
                        }
                    }
                    catch { }
                }
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
                                    if (IsPassWord) g.DrawString(PassWordChar, Font, fore, it.rect, sf_font);
                                    else if (it.emoji) g.DrawString(it.text, font, fore, it.rect, sf_font);
                                    else g.DrawString(it.text, Font, fore, it.rect, sf_font);
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
                                if (IsPassWord) g.DrawString(PassWordChar, Font, fore, it.rect, sf_font);
                                else g.DrawString(it.text, Font, fore, it.rect, sf_font);
                            }
                        }
                    }
                }
                g.ResetTransform();
            }
            else if (placeholderText != null)
            {
                using (var fore = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.DrawString(placeholderText, Font, fore, rect_text, sf_placeholder);
                }
            }
        }

        internal virtual void PaintRIcon(Graphics g, Rectangle rect) { }

        internal virtual void PaintOtherBor(Graphics g, RectangleF rect_read, float radius, Color back, Color borderColor, Color borderActive) { }

        #region 点击动画

        internal void PaintClick(Graphics g, GraphicsPath path, Color color, float radius)
        {
            if (AnimationFocus)
            {
                if (AnimationFocusValue > 0)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(AnimationFocusValue, color)))
                    {
                        var rect = ClientRectangle.PaddingRect(Padding);
                        using (var path_click = Helper.RoundPath(rect, radius, round))
                        {
                            path_click.AddPath(path, false);
                            g.FillPath(brush, path_click);
                        }
                    }
                }
            }
            else if (ExtraMouseDown && Margins > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(30, color)))
                {
                    var rect = ClientRectangle.PaddingRect(Padding);
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
            get => ClientRectangle.PaddingRect(Padding).ReadRect(Margins + (int)(borderWidth * Config.Dpi / 2F), JoinLeft, JoinRight);
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
                if (showCaret) Win32.SetCaretPos(CurrentCaret.X - scrollx, CurrentCaret.Y - scrolly);
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
                Invalidate();
                if (showCaret) Win32.SetCaretPos(CurrentCaret.X - scrollx, CurrentCaret.Y - scrolly);
            }
        }

        bool ScrollXShow = false, ScrollYShow = false, ScrollYDown = false;
        void ScrollTo(Rectangle r)
        {
            if (ScrollYShow)
            {
                int y = CurrentCaret.Y - scrolly;
                if (y < rect_text.Y) ScrollY -= rect_text.Height;
                else if (y + CurrentCaret.Height > rect_text.Height) ScrollY += rect_text.Height;
            }
            else if (ScrollXShow)
            {
                int x = CurrentCaret.X - scrollx;
                if (x < rect_text.X) ScrollX -= r.Width;
                else if (x + CurrentCaret.Width > rect_text.Width) ScrollX += r.Width;
            }
            else ScrollX = ScrollY = 0;
        }

        #endregion
    }
}