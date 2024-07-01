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
        static void PaintButton(Graphics g, Font font, int gap, Rectangle rect_read, TemplateButton template, CellButton btn)
        {
            float _radius = (btn.Shape == TShape.Round || btn.Shape == TShape.Circle) ? rect_read.Height : btn.Radius * Config.Dpi;

            if (btn.Type == TTypeMini.Default)
            {
                Color _fore = Style.Db.DefaultColor, _color = Style.Db.Primary, _back_hover, _back_active;
                if (btn.BorderWidth > 0)
                {
                    _back_hover = Style.Db.PrimaryHover;
                    _back_active = Style.Db.PrimaryActive;
                }
                else
                {
                    _back_hover = Style.Db.FillSecondary;
                    _back_active = Style.Db.Fill;
                }
                if (btn.Fore.HasValue) _fore = btn.Fore.Value;
                if (btn.BackHover.HasValue) _back_hover = btn.BackHover.Value;
                if (btn.BackActive.HasValue) _back_active = btn.BackActive.Value;

                using (var path = PathButton(rect_read, btn, _radius))
                {
                    #region 动画

                    if (template.AnimationClick)
                    {
                        float maxw = rect_read.Width + (gap * template.AnimationClickValue), maxh = rect_read.Height + (gap * template.AnimationClickValue),
                            alpha = 100 * (1F - template.AnimationClickValue);
                        using (var brush = new SolidBrush(Helper.ToColor(alpha, _color)))
                        {
                            using (var path_click = new RectangleF(rect_read.X + (rect_read.Width - maxw) / 2F, rect_read.Y + (rect_read.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, btn.Shape))
                            {
                                path_click.AddPath(path, false);
                                g.FillPath(brush, path_click);
                            }
                        }
                    }

                    #endregion

                    if (btn.Enabled)
                    {
                        if (!btn.Ghost)
                        {
                            #region 绘制阴影

                            if (btn.Enabled)
                            {
                                using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                                {
                                    path_shadow.AddPath(path, false);
                                    using (var brush = new SolidBrush(Style.Db.FillQuaternary))
                                    {
                                        g.FillPath(brush, path_shadow);
                                    }
                                }
                            }

                            #endregion

                            using (var brush = new SolidBrush(btn.DefaultBack ?? Style.Db.DefaultBg))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        if (btn.BorderWidth > 0)
                        {
                            float border = btn.BorderWidth * Config.Dpi;
                            if (template.ExtraMouseDown)
                            {
                                using (var brush = new Pen(_back_active, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, _back_active, rect_read);
                            }
                            else if (template.AnimationHover)
                            {
                                var colorHover = Helper.ToColor(template.AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, _fore, colorHover, rect_read);
                            }
                            else if (template.ExtraMouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, _back_hover, rect_read);
                            }
                            else
                            {
                                using (var brush = new Pen(btn.DefaultBorderColor ?? Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, _fore, rect_read);
                            }
                        }
                        else
                        {
                            if (template.ExtraMouseDown)
                            {
                                using (var brush = new SolidBrush(_back_active))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else if (template.AnimationHover)
                            {
                                using (var brush = new SolidBrush(Helper.ToColor(template.AnimationHoverValue, _back_hover)))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else if (template.ExtraMouseHover)
                            {
                                using (var brush = new SolidBrush(_back_hover))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            PaintButton(g, font, btn, template, _fore, rect_read);
                        }
                    }
                    else
                    {
                        if (btn.BorderWidth > 0)
                        {
                            using (var brush = new SolidBrush(Style.Db.FillTertiary))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        PaintButton(g, font, btn, template, Style.Db.TextQuaternary, rect_read);
                    }
                }
            }
            else
            {
                Color _fore, _back, _back_hover, _back_active;
                switch (btn.Type)
                {
                    case TTypeMini.Error:
                        _back = Style.Db.Error;
                        _fore = Style.Db.ErrorColor;
                        _back_hover = Style.Db.ErrorHover;
                        _back_active = Style.Db.ErrorActive;
                        break;
                    case TTypeMini.Success:
                        _back = Style.Db.Success;
                        _fore = Style.Db.SuccessColor;
                        _back_hover = Style.Db.SuccessHover;
                        _back_active = Style.Db.SuccessActive;
                        break;
                    case TTypeMini.Info:
                        _back = Style.Db.Info;
                        _fore = Style.Db.InfoColor;
                        _back_hover = Style.Db.InfoHover;
                        _back_active = Style.Db.InfoActive;
                        break;
                    case TTypeMini.Warn:
                        _back = Style.Db.Warning;
                        _fore = Style.Db.WarningColor;
                        _back_hover = Style.Db.WarningHover;
                        _back_active = Style.Db.WarningActive;
                        break;
                    case TTypeMini.Primary:
                    default:
                        _back = Style.Db.Primary;
                        _fore = Style.Db.PrimaryColor;
                        _back_hover = Style.Db.PrimaryHover;
                        _back_active = Style.Db.PrimaryActive;
                        break;
                }

                if (btn.Fore.HasValue) _fore = btn.Fore.Value;
                if (btn.Back.HasValue) _back = btn.Back.Value;
                if (btn.BackHover.HasValue) _back_hover = btn.BackHover.Value;
                if (btn.BackActive.HasValue) _back_active = btn.BackActive.Value;

                using (var path = PathButton(rect_read, btn, _radius))
                {
                    #region 动画

                    if (template.AnimationClick)
                    {
                        float maxw = rect_read.Width + (gap * template.AnimationClickValue), maxh = rect_read.Height + (gap * template.AnimationClickValue),
                            alpha = 100 * (1F - template.AnimationClickValue);
                        using (var brush = new SolidBrush(Helper.ToColor(alpha, _back)))
                        {
                            using (var path_click = new RectangleF(rect_read.X + (rect_read.Width - maxw) / 2F, rect_read.Y + (rect_read.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, btn.Shape))
                            {
                                path_click.AddPath(path, false);
                                g.FillPath(brush, path_click);
                            }
                        }
                    }

                    #endregion

                    if (btn.Ghost)
                    {
                        #region 绘制背景

                        if (btn.BorderWidth > 0)
                        {
                            float border = btn.BorderWidth * Config.Dpi;
                            if (template.ExtraMouseDown)
                            {
                                using (var brush = new Pen(_back_active, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, _back_active, rect_read);
                            }
                            else if (template.AnimationHover)
                            {
                                var colorHover = Helper.ToColor(template.AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(btn.Enabled ? _back : Style.Db.FillTertiary, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, _back, colorHover, rect_read);
                            }
                            else if (template.ExtraMouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, _back_hover, rect_read);
                            }
                            else
                            {
                                using (var brush = new Pen(btn.Enabled ? _back : Style.Db.FillTertiary, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintButton(g, font, btn, template, btn.Enabled ? _back : Style.Db.TextQuaternary, rect_read);
                            }
                        }
                        else PaintButton(g, font, btn, template, btn.Enabled ? _back : Style.Db.TextQuaternary, rect_read);

                        #endregion
                    }
                    else
                    {
                        #region 绘制阴影

                        if (btn.Enabled)
                        {
                            using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                            {
                                path_shadow.AddPath(path, false);
                                using (var brush = new SolidBrush(_back.rgba(Config.Mode == TMode.Dark ? 0.15F : 0.1F)))
                                {
                                    g.FillPath(brush, path_shadow);
                                }
                            }
                        }

                        #endregion

                        #region 绘制背景

                        using (var brush = new SolidBrush(btn.Enabled ? _back : Style.Db.FillTertiary))
                        {
                            g.FillPath(brush, path);
                        }

                        if (template.ExtraMouseDown)
                        {
                            using (var brush = new SolidBrush(_back_active))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else if (template.AnimationHover)
                        {
                            var colorHover = Helper.ToColor(template.AnimationHoverValue, _back_hover);
                            using (var brush = new SolidBrush(colorHover))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else if (template.ExtraMouseHover)
                        {
                            using (var brush = new SolidBrush(_back_hover))
                            {
                                g.FillPath(brush, path);
                            }
                        }

                        #endregion

                        PaintButton(g, font, btn, template, btn.Enabled ? _fore : Style.Db.TextQuaternary, rect_read);
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

        static void PaintButton(Graphics g, Font font, CellButton btn, TemplateButton template, Color color, Rectangle rect_read)
        {
            if (string.IsNullOrEmpty(btn.Text))
            {
                var font_size = g.MeasureString(Config.NullText, font).Size();
                //没有文字
                var rect = PaintButtonImageRectCenter(btn, font_size, rect_read);
                if (PaintButtonImageNoText(g, btn, template, color, rect) && btn.ShowArrow)
                {
                    float size = font_size.Height * btn.IconRatio;
                    var rect_arrow = new RectangleF(rect_read.X + (rect_read.Width - size) / 2F, rect_read.Y + (rect_read.Height - size) / 2F, size, size);
                    using (var pen = new Pen(color, 2F * Config.Dpi))
                    {
                        pen.StartCap = pen.EndCap = LineCap.Round;
                        if (btn.IsLink)
                        {
                            var state = g.Save();
                            float size2 = rect_arrow.Width / 2F;
                            g.TranslateTransform(rect_arrow.X + size2, rect_arrow.Y + size2);
                            g.RotateTransform(-90F);
                            g.DrawLines(pen, new RectangleF(-size2, -size2, rect_arrow.Width, rect_arrow.Height).TriangleLines(btn.ArrowProg));
                            g.ResetTransform();
                            g.Restore(state);
                        }
                        else
                        {
                            g.DrawLines(pen, rect_arrow.TriangleLines(btn.ArrowProg));
                        }
                    }
                }
            }
            else
            {
                var font_size = g.MeasureString(btn.Text ?? Config.NullText, font).Size();
                bool has_left = btn.HasImage, has_rigth = btn.ShowArrow;
                Rectangle rect_text;
                if (has_left || has_rigth)
                {
                    int font_width = font_size.Width;
                    int icon_size = (int)(font_size.Height * btn.IconRatio), sps = (int)(font_size.Height * .4F), sps2 = sps * 2, sp = (int)(font_size.Height * .25F);

                    if (has_left && has_rigth)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);

                        rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                        Rectangle rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size),
                            rect_r = new Rectangle((rect_read.X + sps + rect_read.Width - sps2 - icon_size - sp) + sp, rect_l.Y, icon_size, icon_size);

                        PaintButtonPaintImage(g, btn, template, color, rect_l);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            g.FillEllipse(Brushes.AliceBlue, rect_r);
                            if (btn.IsLink)
                            {
                                var state = g.Save();
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(btn.ArrowProg));
                                g.ResetTransform();
                                g.Restore(state);
                            }
                            else
                            {
                                g.DrawLines(pen, rect_r.TriangleLines(btn.ArrowProg));
                            }
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);

                        rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                        var rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                        PaintButtonPaintImage(g, btn, template, color, rect_l);
                    }
                    else
                    {
                        rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2 - icon_size - sp, rect_read.Height - sps2);
                        var rect_r = new Rectangle(rect_text.Right + sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (btn.IsLink)
                            {
                                var state = g.Save();
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(btn.ArrowProg));
                                g.ResetTransform();
                                g.Restore(state);
                            }
                            else
                            {
                                g.DrawLines(pen, rect_r.TriangleLines(btn.ArrowProg));
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                }
                PaintButtonTextAlign(btn, rect_read, ref rect_text);
                using (var brush = new SolidBrush(color))
                {
                    g.DrawString(btn.Text, font, brush, rect_text, btn.stringFormat);
                }
            }
        }

        static void PaintButton(Graphics g, Font font, CellButton btn, TemplateButton template, Color color, Color colorHover, Rectangle rect_read)
        {
            if (string.IsNullOrEmpty(btn.Text))
            {
                var font_size = g.MeasureString(Config.NullText, font).Size();
                var rect = PaintButtonImageRectCenter(btn, font_size, rect_read);
                if (PaintButtonImageNoText(g, btn, template, color, rect))
                {
                    if (btn.ShowArrow)
                    {
                        float size = font_size.Height * btn.IconRatio;
                        var rect_arrow = new RectangleF(rect_read.X + (rect_read.Width - size) / 2F, rect_read.Y + (rect_read.Height - size) / 2F, size, size);
                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (btn.IsLink)
                            {
                                var state = g.Save();
                                float size2 = rect_arrow.Width / 2F;
                                g.TranslateTransform(rect_arrow.X + size2, rect_arrow.Y + size2);
                                g.RotateTransform(-90F);
                                var rect_arrow_lines = new RectangleF(-size2, -size2, rect_arrow.Width, rect_arrow.Height).TriangleLines(btn.ArrowProg);
                                g.DrawLines(pen, rect_arrow_lines);
                                g.DrawLines(penHover, rect_arrow_lines);
                                g.ResetTransform();
                                g.Restore(state);
                            }
                            else
                            {
                                var rect_arrow_lines = rect_arrow.TriangleLines(btn.ArrowProg);
                                g.DrawLines(pen, rect_arrow_lines);
                                g.DrawLines(penHover, rect_arrow_lines);
                            }
                        }
                    }
                }
                else PaintButtonImageNoText(g, btn, template, colorHover, rect);
            }
            else
            {
                var font_size = g.MeasureString(btn.Text ?? Config.NullText, font).Size();
                bool has_left = btn.HasImage, has_rigth = btn.ShowArrow;
                Rectangle rect_text;
                if (has_left || has_rigth)
                {
                    int font_width = font_size.Width;
                    int icon_size = (int)(font_size.Height * btn.IconRatio), sps = (int)(font_size.Height * .4F), sps2 = sps * 2, sp = (int)(font_size.Height * .25F);

                    if (has_left && has_rigth)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);

                        Rectangle rect_l, rect_r;


                        rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                        rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                        rect_r = new Rectangle((rect_read.X + sps + rect_read.Width - sps2 - icon_size - sp) + sp, rect_l.Y, icon_size, icon_size);


                        PaintButtonPaintImage(g, btn, template, color, rect_l);
                        PaintButtonPaintImage(g, btn, template, colorHover, rect_l);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (btn.IsLink)
                            {
                                var state = g.Save();
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                var rect_arrow = new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(btn.ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                                g.Restore(state);
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(btn.ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);
                        Rectangle rect_l;

                        rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                        rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                        PaintButtonPaintImage(g, btn, template, color, rect_l);
                        PaintButtonPaintImage(g, btn, template, colorHover, rect_l);
                    }
                    else
                    {
                        Rectangle rect_r;


                        rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2 - icon_size - sp, rect_read.Height - sps2);
                        rect_r = new Rectangle(rect_text.Right + sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);


                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (btn.IsLink)
                            {
                                var state = g.Save();
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                var rect_arrow = new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(btn.ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                                g.Restore(state);
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(btn.ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                }
                PaintButtonTextAlign(btn, rect_read, ref rect_text);
                using (var brush = new SolidBrush(color))
                using (var brushHover = new SolidBrush(colorHover))
                {
                    g.DrawString(btn.Text, font, brush, rect_text, btn.stringFormat);
                    g.DrawString(btn.Text, font, brushHover, rect_text, btn.stringFormat);
                }
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
        static bool PaintButtonImageNoText(Graphics g, CellButton btn, TemplateButton template, Color? color, Rectangle rect)
        {
            if (template.AnimationImageHover)
            {
                PaintButtonCoreImage(g, btn, template, rect, color, 1F - template.AnimationImageHoverValue);
                PaintButtonCoreImageHover(g, btn, template, rect, color, template.AnimationImageHoverValue);
                return false;
            }
            else
            {
                if (template.ExtraMouseHover)
                {
                    if (PaintButtonCoreImageHover(g, btn, template, rect, color)) return false;
                }
                if (PaintButtonCoreImage(g, btn, template, rect, color)) return false;
            }
            return true;
        }

        static bool PaintButtonCoreImage(Graphics g, CellButton btn, TemplateButton template, Rectangle rect, Color? color, float opacity = 1F)
        {
            if (btn.ImageSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(btn.ImageSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.DrawImage(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (btn.Image != null)
            {
                g.DrawImage(btn.Image, rect, opacity);
                return true;
            }
            return false;
        }

        static bool PaintButtonCoreImageHover(Graphics g, CellButton btn, TemplateButton template, Rectangle rect, Color? color, float opacity = 1F)
        {
            if (btn.ImageHoverSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(btn.ImageHoverSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.DrawImage(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (btn.ImageHover != null)
            {
                g.DrawImage(btn.ImageHover, rect, opacity);
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
        static void PaintButtonPaintImage(Graphics g, CellButton btn, TemplateButton template, Color? color, Rectangle rectl)
        {
            if (template.AnimationImageHover)
            {
                PaintButtonCoreImage(g, btn, template, rectl, color, 1F - template.AnimationImageHoverValue);
                PaintButtonCoreImageHover(g, btn, template, rectl, color, template.AnimationImageHoverValue);
                return;
            }
            else
            {
                if (template.ExtraMouseHover)
                {
                    if (PaintButtonCoreImageHover(g, btn, template, rectl, color)) return;
                }
                PaintButtonCoreImage(g, btn, template, rectl, color);
            }
        }

        #endregion

        static void PaintLink(Graphics g, Font font, Rectangle rect_read, TemplateButton template, CellLink link)
        {
            if (template.ExtraMouseDown)
            {
                using (var brush = new SolidBrush(Style.Db.PrimaryActive))
                {
                    g.DrawString(link.Text, font, brush, rect_read, link.stringFormat);
                }
            }
            else if (template.AnimationHover)
            {
                var colorHover = Helper.ToColor(template.AnimationHoverValue, Style.Db.PrimaryHover);
                using (var brush = new SolidBrush(Style.Db.Primary))
                {
                    g.DrawString(link.Text, font, brush, rect_read, link.stringFormat);
                }
                using (var brush = new SolidBrush(colorHover))
                {
                    g.DrawString(link.Text, font, brush, rect_read, link.stringFormat);
                }
            }
            else if (template.ExtraMouseHover)
            {
                using (var brush = new SolidBrush(Style.Db.PrimaryHover))
                {
                    g.DrawString(link.Text, font, brush, rect_read, link.stringFormat);
                }
            }
            else
            {
                using (var brush = new SolidBrush(link.Enabled ? Style.Db.Primary : Style.Db.TextQuaternary))
                {
                    g.DrawString(link.Text, font, brush, rect_read, link.stringFormat);
                }
            }
        }
    }
}