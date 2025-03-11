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

namespace AntdUI
{
    partial class Table
    {
        internal static void PaintButton(Canvas g, Font font, int gap, Rectangle rect_read, CellButton btn, bool enable)
        {
            float _radius = (btn.Shape == TShape.Round || btn.Shape == TShape.Circle) ? rect_read.Height : btn.Radius * Config.Dpi;

            if (btn.Type == TTypeMini.Default)
            {
                Color _fore = Colour.DefaultColor.Get("Button"), _color = Colour.Primary.Get("Button"), _back_hover, _back_active;
                if (btn.BorderWidth > 0)
                {
                    _back_hover = Colour.PrimaryHover.Get("Button");
                    _back_active = Colour.PrimaryActive.Get("Button");
                }
                else
                {
                    _back_hover = Colour.FillSecondary.Get("Button");
                    _back_active = Colour.Fill.Get("Button");
                }
                if (btn.Fore.HasValue) _fore = btn.Fore.Value;
                if (btn.BackHover.HasValue) _back_hover = btn.BackHover.Value;
                if (btn.BackActive.HasValue) _back_active = btn.BackActive.Value;

                using (var path = PathButton(rect_read, btn, _radius))
                {
                    #region 动画

                    if (btn.AnimationClick)
                    {
                        float maxw = rect_read.Width + (gap * btn.AnimationClickValue), maxh = rect_read.Height + (gap * btn.AnimationClickValue),
                            alpha = 100 * (1F - btn.AnimationClickValue);
                        using (var path_click = new RectangleF(rect_read.X + (rect_read.Width - maxw) / 2F, rect_read.Y + (rect_read.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, btn.Shape))
                        {
                            path_click.AddPath(path, false);
                            g.Fill(Helper.ToColor(alpha, _color), path_click);
                        }
                    }

                    #endregion

                    if (enable && btn.Enabled)
                    {
                        if (!btn.Ghost)
                        {
                            #region 绘制阴影

                            using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                            {
                                path_shadow.AddPath(path, false);
                                g.Fill(Colour.FillQuaternary.Get("Button"), path_shadow);
                            }

                            #endregion

                            g.Fill(btn.DefaultBack ?? Colour.DefaultBg.Get("Button"), path);
                        }
                        if (btn.BorderWidth > 0)
                        {
                            float border = btn.BorderWidth * Config.Dpi;
                            if (btn.ExtraMouseDown)
                            {
                                g.Draw(_back_active, border, path);
                                PaintButton(g, font, btn, _back_active, rect_read);
                            }
                            else if (btn.AnimationHover)
                            {
                                var colorHover = Helper.ToColor(btn.AnimationHoverValue, _back_hover);
                                g.Draw(Colour.DefaultBorder.Get("Button").BlendColors(colorHover), border, path);
                                PaintButton(g, font, btn, _fore.BlendColors(colorHover), rect_read);
                            }
                            else if (btn.ExtraMouseHover)
                            {
                                g.Draw(_back_hover, border, path);
                                PaintButton(g, font, btn, _back_hover, rect_read);
                            }
                            else
                            {
                                g.Draw(btn.DefaultBorderColor ?? Colour.DefaultBorder.Get("Button"), border, path);
                                PaintButton(g, font, btn, _fore, rect_read);
                            }
                        }
                        else
                        {
                            if (btn.ExtraMouseDown) g.Fill(_back_active, path);
                            else if (btn.AnimationHover) g.Fill(Helper.ToColor(btn.AnimationHoverValue, _back_hover), path);
                            else if (btn.ExtraMouseHover) g.Fill(_back_hover, path);
                            PaintButton(g, font, btn, _fore, rect_read);
                        }
                    }
                    else
                    {
                        if (btn.BorderWidth > 0) g.Fill(Colour.FillTertiary.Get("Button"), path);
                        PaintButton(g, font, btn, Colour.TextQuaternary.Get("Button"), rect_read);
                    }
                }
            }
            else
            {
                Color _fore, _back, _back_hover, _back_active;
                switch (btn.Type)
                {
                    case TTypeMini.Error:
                        _back = Colour.Error.Get("Button");
                        _fore = Colour.ErrorColor.Get("Button");
                        _back_hover = Colour.ErrorHover.Get("Button");
                        _back_active = Colour.ErrorActive.Get("Button");
                        break;
                    case TTypeMini.Success:
                        _back = Colour.Success.Get("Button");
                        _fore = Colour.SuccessColor.Get("Button");
                        _back_hover = Colour.SuccessHover.Get("Button");
                        _back_active = Colour.SuccessActive.Get("Button");
                        break;
                    case TTypeMini.Info:
                        _back = Colour.Info.Get("Button");
                        _fore = Colour.InfoColor.Get("Button");
                        _back_hover = Colour.InfoHover.Get("Button");
                        _back_active = Colour.InfoActive.Get("Button");
                        break;
                    case TTypeMini.Warn:
                        _back = Colour.Warning.Get("Button");
                        _fore = Colour.WarningColor.Get("Button");
                        _back_hover = Colour.WarningHover.Get("Button");
                        _back_active = Colour.WarningActive.Get("Button");
                        break;
                    case TTypeMini.Primary:
                    default:
                        _back = Colour.Primary.Get("Button");
                        _fore = Colour.PrimaryColor.Get("Button");
                        _back_hover = Colour.PrimaryHover.Get("Button");
                        _back_active = Colour.PrimaryActive.Get("Button");
                        break;
                }

                if (btn.Fore.HasValue) _fore = btn.Fore.Value;
                if (btn.Back.HasValue) _back = btn.Back.Value;
                if (btn.BackHover.HasValue) _back_hover = btn.BackHover.Value;
                if (btn.BackActive.HasValue) _back_active = btn.BackActive.Value;

                using (var path = PathButton(rect_read, btn, _radius))
                {
                    #region 动画

                    if (btn.AnimationClick)
                    {
                        float maxw = rect_read.Width + (gap * btn.AnimationClickValue), maxh = rect_read.Height + (gap * btn.AnimationClickValue),
                            alpha = 100 * (1F - btn.AnimationClickValue);
                        using (var path_click = new RectangleF(rect_read.X + (rect_read.Width - maxw) / 2F, rect_read.Y + (rect_read.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, btn.Shape))
                        {
                            path_click.AddPath(path, false);
                            g.Fill(Helper.ToColor(alpha, _back), path_click);
                        }
                    }

                    #endregion

                    if (btn.Ghost)
                    {
                        #region 绘制背景

                        if (btn.BorderWidth > 0)
                        {
                            float border = btn.BorderWidth * Config.Dpi;
                            if (btn.ExtraMouseDown)
                            {
                                g.Draw(_back_active, border, path);
                                PaintButton(g, font, btn, _back_active, rect_read);
                            }
                            else if (btn.AnimationHover)
                            {
                                var colorHover = Helper.ToColor(btn.AnimationHoverValue, _back_hover);
                                g.Draw(((enable && btn.Enabled) ? _back : Colour.FillTertiary.Get("Button")).BlendColors(colorHover), border, path);
                                PaintButton(g, font, btn, _back.BlendColors(colorHover), rect_read);
                            }
                            else if (btn.ExtraMouseHover)
                            {
                                g.Draw(_back_hover, border, path);
                                PaintButton(g, font, btn, _back_hover, rect_read);
                            }
                            else
                            {
                                if (enable && btn.Enabled)
                                {
                                    using (var brushback = btn.BackExtend.BrushEx(rect_read, _back))
                                    {
                                        g.Draw(brushback, border, path);
                                    }
                                }
                                else g.Draw(Colour.FillTertiary.Get("Button"), border, path);
                                PaintButton(g, font, btn, (enable && btn.Enabled) ? _back : Colour.TextQuaternary.Get("Button"), rect_read);
                            }
                        }
                        else PaintButton(g, font, btn, (enable && btn.Enabled) ? _back : Colour.TextQuaternary.Get("Button"), rect_read);

                        #endregion
                    }
                    else
                    {
                        #region 绘制阴影

                        if (enable && btn.Enabled)
                        {
                            using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                            {
                                path_shadow.AddPath(path, false);
                                g.Fill(_back.rgba(Config.Mode == TMode.Dark ? 0.15F : 0.1F), path_shadow);
                            }
                        }

                        #endregion

                        #region 绘制背景

                        if (enable && btn.Enabled)
                        {
                            using (var brush = btn.BackExtend.BrushEx(rect_read, _back))
                            {
                                g.Fill(brush, path);
                            }
                        }
                        else g.Fill(Colour.FillTertiary.Get("Button"), path);

                        if (btn.ExtraMouseDown) g.Fill(_back_active, path);
                        else if (btn.AnimationHover) g.Fill(Helper.ToColor(btn.AnimationHoverValue, _back_hover), path);
                        else if (btn.ExtraMouseHover) g.Fill(_back_hover, path);

                        #endregion

                        PaintButton(g, font, btn, (enable && btn.Enabled) ? _fore : Colour.TextQuaternary.Get("Button"), rect_read);
                    }
                }
            }
        }

        #region 渲染帮助

        static GraphicsPath PathButton(RectangleF rect_read, CellButton btn, float _radius)
        {
            if (btn.Shape == TShape.Circle)
            {
                var path = new GraphicsPath();
                path.AddEllipse(rect_read);
                return path;
            }
            return rect_read.RoundPath(_radius);
        }

        static void PaintButton(Canvas g, Font font, CellButton btn, Color color, Rectangle rect_read)
        {
            if (string.IsNullOrEmpty(btn.Text))
            {
                var font_size = g.MeasureString(Config.NullText, font);
                //没有文字
                var rect = PaintButtonImageRectCenter(btn, font_size, rect_read);
                if (PaintButtonImageNoText(g, btn, color, rect) && btn.ShowArrow)
                {
                    int size = (int)(font_size.Height * btn.IconRatio);
                    var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                    PaintButtonTextArrow(g, btn, rect_arrow, color);
                }
            }
            else
            {
                var font_size = g.MeasureString(btn.Text ?? Config.NullText, font);
                bool has_left = btn.HasIcon, has_right = btn.ShowArrow;
                Rectangle rect_text;
                if (has_left || has_right)
                {
                    if (has_left && has_right)
                    {
                        rect_text = Button.RectAlignLR(g, btn.textLine, false, font, btn.IconPosition, btn.IconRatio, btn.IconGap, font_size, rect_read, out var rect_l, out var rect_r);
                        PaintButtonPaintImage(g, btn, color, rect_l);
                        PaintButtonTextArrow(g, btn, rect_r, color);
                    }
                    else if (has_left)
                    {
                        rect_text = Button.RectAlignL(g, btn.textLine, false, false, font, btn.IconPosition, btn.IconRatio, btn.IconGap, font_size, rect_read, out var rect_l);
                        PaintButtonPaintImage(g, btn, color, rect_l);
                    }
                    else
                    {
                        rect_text = Button.RectAlignR(g, btn.textLine, false, font, btn.IconPosition, btn.IconRatio, btn.IconGap, font_size, rect_read, out var rect_r);
                        PaintButtonTextArrow(g, btn, rect_r, color);
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                    PaintButtonTextAlign(btn, rect_read, ref rect_text);
                }
                using (var brush = new SolidBrush(color))
                {
                    g.String(btn.Text, font, brush, rect_text, btn.stringFormat);
                }
            }
        }
        static void PaintButtonTextArrow(Canvas g, CellButton btn, Rectangle rect, Color color)
        {
            using (var pen = new Pen(color, 2F * Config.Dpi))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                if (btn.IsLink)
                {
                    var state = g.Save();
                    float size_arrow = rect.Width / 2F;
                    g.TranslateTransform(rect.X + size_arrow, rect.Y + size_arrow);
                    g.RotateTransform(-90F);
                    g.DrawLines(pen, new RectangleF(-size_arrow, -size_arrow, rect.Width, rect.Height).TriangleLines(btn.ArrowProg));
                    g.ResetTransform();
                    g.Restore(state);
                }
                else g.DrawLines(pen, rect.TriangleLines(btn.ArrowProg));
            }
        }

        static void PaintButtonTextAlign(CellButton btn, Rectangle rect_read, ref Rectangle rect_text)
        {
            switch (btn.TextAlign)
            {
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    rect_text.Y = rect_read.Y;
                    rect_text.Height = rect_read.Height;
                    break;
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                case ContentAlignment.TopCenter:
                    rect_text.Height = rect_read.Height - rect_text.Y;
                    break;
            }
        }

        /// <summary>
        /// 渲染图片（没有文字）
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rect">区域</param>
        static bool PaintButtonImageNoText(Canvas g, CellButton btn, Color? color, Rectangle rect)
        {
            if (btn.AnimationImageHover)
            {
                PaintButtonCoreImage(g, btn, rect, color, 1F - btn.AnimationImageHoverValue);
                PaintButtonCoreImageHover(g, btn, rect, color, btn.AnimationImageHoverValue);
                return false;
            }
            else
            {
                if (btn.ExtraMouseHover)
                {
                    if (PaintButtonCoreImageHover(g, btn, rect, color)) return false;
                }
                if (PaintButtonCoreImage(g, btn, rect, color)) return false;
            }
            return true;
        }

        static bool PaintButtonCoreImage(Canvas g, CellButton btn, Rectangle rect, Color? color, float opacity = 1F)
        {
            if (btn.IconSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(btn.IconSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.Image(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (btn.Icon != null)
            {
                g.Image(btn.Icon, rect, opacity);
                return true;
            }
            return false;
        }

        static bool PaintButtonCoreImageHover(Canvas g, CellButton btn, Rectangle rect, Color? color, float opacity = 1F)
        {
            if (btn.IconHoverSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(btn.IconHoverSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.Image(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (btn.IconHover != null)
            {
                g.Image(btn.IconHover, rect, opacity);
                return true;
            }
            return false;
        }

        static Rectangle PaintButtonImageRectCenter(CellButton btn, Size font_size, Rectangle rect_read)
        {
            int w = (int)(font_size.Height * btn.IconRatio * 1.125F);
            return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - w) / 2, w, w);
        }

        /// <summary>
        /// 渲染图片
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rectl">图标区域</param>
        static void PaintButtonPaintImage(Canvas g, CellButton btn, Color? color, Rectangle rectl)
        {
            if (btn.AnimationImageHover)
            {
                PaintButtonCoreImage(g, btn, rectl, color, 1F - btn.AnimationImageHoverValue);
                PaintButtonCoreImageHover(g, btn, rectl, color, btn.AnimationImageHoverValue);
                return;
            }
            else
            {
                if (btn.ExtraMouseHover)
                {
                    if (PaintButtonCoreImageHover(g, btn, rectl, color)) return;
                }
                PaintButtonCoreImage(g, btn, rectl, color);
            }
        }

        #endregion

        internal static void PaintLink(Canvas g, Font font, Rectangle rect_read, CellLink link, bool enable)
        {
            if (link.ExtraMouseDown) g.String(link.Text, font, Colour.PrimaryActive.Get("Button"), rect_read, link.stringFormat);
            else if (link.AnimationHover) g.String(link.Text, font, Colour.Primary.Get("Button").BlendColors(link.AnimationHoverValue, Colour.PrimaryHover.Get("Button")), rect_read, link.stringFormat);
            else if (link.ExtraMouseHover) g.String(link.Text, font, Colour.PrimaryHover.Get("Button"), rect_read, link.stringFormat);
            else g.String(link.Text, font, ((enable && link.Enabled) ? Colour.Primary.Get("Button") : Colour.TextQuaternary.Get("Button")), rect_read, link.stringFormat);
        }
    }
}