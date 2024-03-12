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

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        static StringFormat stringLeft = Helper.SF_NoWrap(lr: StringAlignment.Near);
        static StringFormat stringCenter = Helper.SF_NoWrap();
        static StringFormat stringRight = Helper.SF_NoWrap(lr: StringAlignment.Far);

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            if (rows == null)
            {
                if (Empty)
                {
                    var rect = ClientRectangle;
                    using (var fore = new SolidBrush(Style.Db.Text))
                    {
                        if (EmptyImage == null) g.DrawString(EmptyText, Font, fore, rect, stringCenter);
                        else
                        {
                            int gap = (int)(_gap * Config.Dpi);
                            var size = g.MeasureString(EmptyText, Font);
                            RectangleF rect_img = new RectangleF(rect.X + (rect.Width - EmptyImage.Width) / 2F, rect.Y + (rect.Height - EmptyImage.Height) / 2F - size.Height, EmptyImage.Width, EmptyImage.Height),
                                rect_font = new RectangleF(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);
                            g.DrawImage(EmptyImage, rect_img);
                            g.DrawString(EmptyText, Font, fore, rect_font, stringCenter);
                        }
                    }
                }
                base.OnPaint(e);
                return;
            }
            float sx = scrollX.Value, sy = scrollY.Value;
            using (var fore = new SolidBrush(Style.Db.Text))
            using (var brush_split = new SolidBrush(Style.Db.BorderColor))
            {
                g.TranslateTransform(0, -sy);
                var shows = new List<RowTemplate>();
                if (fixedHeader)
                {
                    foreach (var it in rows)
                    {
                        it.SHOW = !it.IsColumn && it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + scrollY.Height + it.RECT.Height;
                        if (it.SHOW)
                        {
                            shows.Add(it);
                            PaintTableBg(g, it);
                        }
                    }

                    if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows)
                    {
                        foreach (var cel in it.cells) PaintItem(g, cel, fore);
                    }

                    g.ResetTransform();

                    PaintTableBgHeader(g, rows[0]);

                    g.TranslateTransform(-sx, 0);

                    PaintTableHeader(g, rows[0], fore);

                    if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);
                }
                else
                {
                    foreach (var it in rows)
                    {
                        it.SHOW = it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + scrollY.Height + it.RECT.Height;
                        if (it.SHOW)
                        {
                            shows.Add(it);
                            if (it.IsColumn) PaintTableBgHeader(g, it);
                            else PaintTableBg(g, it);
                        }
                    }
                    if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows)
                    {
                        if (it.IsColumn) PaintTableHeader(g, it, fore);
                        else foreach (var cel in it.cells) PaintItem(g, cel, fore);
                    }
                    if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);
                }

                g.ResetTransform();

                #region 渲染浮动列

                if (fixedColumnL != null || fixedColumnR != null)
                {
                    if (fixedColumnL != null && sx > 0)
                    {
                        showFixedColumnL = true;
                        var rect = ClientRectangle;
                        var last = shows[shows.Count - 1].cells[fixedColumnL[fixedColumnL.Length - 1]];
                        using (var bmp = new Bitmap(last.RECT.Right, last.RECT.Bottom))
                        {
                            using (var g2 = Graphics.FromImage(bmp).High())
                            {
                                g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                g2.Clear(Style.Db.BgBase);
                                g2.TranslateTransform(0, -sy);
                                foreach (var row in shows)
                                {
                                    if (row.IsColumn) { PaintTableBgHeader(g2, row); PaintTableHeader(g2, row, fore); }
                                    else PaintTableBg(g2, row);
                                }
                                foreach (var row in shows)
                                {
                                    foreach (int fixedIndex in fixedColumnL)
                                    {
                                        if (!row.IsColumn) PaintItem(g2, row.cells[fixedIndex], fore);
                                    }
                                }
                                if (dividers.Length > 0) foreach (var divider in dividers) g2.FillRectangle(brush_split, divider);
                                g2.ResetTransform();
                                if (fixedHeader)
                                {
                                    PaintTableBgHeader(g2, rows[0]);
                                    PaintTableHeader(g2, rows[0], fore);
                                }
                                if (dividerHs.Length > 0) foreach (var divider in dividerHs) g2.FillRectangle(brush_split, divider);
                            }
                            var rect_show = new Rectangle(rect.X + bmp.Width - _gap, rect.Y, _gap * 2, bmp.Height);
                            using (var brush = new LinearGradientBrush(rect_show, Style.Db.FillSecondary, Color.Transparent, 0F))
                            {
                                g.FillRectangle(brush, rect_show);
                            }
                            g.DrawImage(bmp, rect.X, rect.Y, bmp.Width, bmp.Height);
                        }
                    }
                    else showFixedColumnL = false;
                    if (fixedColumnR != null && scrollX.Show)
                    {
                        var rect = ClientRectangle;
                        var lastrow = shows[shows.Count - 1];
                        TCell first = lastrow.cells[fixedColumnR[fixedColumnR.Length - 1]],
                            last = lastrow.cells[fixedColumnR[0]];
                        if (sx + Width < last.RECT.Right)
                        {
                            sFixedR = last.RECT.Right - Width;
                            showFixedColumnR = true;
                            int w = last.RECT.Right - first.RECT.Left;
                            using (var bmp = new Bitmap(w, last.RECT.Bottom))
                            {
                                using (var g2 = Graphics.FromImage(bmp).High())
                                {
                                    g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                    g2.Clear(Style.Db.BgBase);
                                    g2.TranslateTransform(0, -sy);
                                    foreach (var row in shows)
                                    {
                                        if (row.IsColumn)
                                        {
                                            PaintTableBgHeader(g2, row);
                                            g2.ResetTransform();
                                            g2.TranslateTransform(-first.RECT.Left, -sy);
                                            PaintTableHeader(g2, row, fore);
                                            g2.ResetTransform();
                                            g2.TranslateTransform(0, -sy);
                                        }
                                        else PaintTableBg(g2, row);
                                    }
                                    g2.TranslateTransform(-first.RECT.Left, 0);
                                    foreach (var row in shows)
                                    {
                                        foreach (int fixedIndex in fixedColumnR)
                                        {
                                            if (!row.IsColumn) PaintItem(g2, row.cells[fixedIndex], fore);
                                        }
                                    }
                                    g2.ResetTransform();
                                    g2.TranslateTransform(0, -sy);
                                    if (dividers.Length > 0) foreach (var divider in dividers) g2.FillRectangle(brush_split, divider);
                                    g2.ResetTransform();
                                    if (fixedHeader)
                                    {
                                        PaintTableBgHeader(g2, rows[0]);
                                        g2.TranslateTransform(-first.RECT.Left, 0);
                                        PaintTableHeader(g2, rows[0], fore);
                                    }
                                    g2.ResetTransform();
                                    g2.TranslateTransform(-first.RECT.Left, 0);
                                    if (dividerHs.Length > 0) foreach (var divider in dividerHs) g2.FillRectangle(brush_split, divider);
                                }
                                var rect_show = new Rectangle(rect.Right - bmp.Width - _gap, rect.Y, _gap * 2, bmp.Height);
                                using (var brush = new LinearGradientBrush(rect_show, Color.Transparent, Style.Db.FillSecondary, 0F))
                                {
                                    g.FillRectangle(brush, rect_show);
                                }
                                g.DrawImage(bmp, rect.Right - bmp.Width, rect.Y, bmp.Width, bmp.Height);
                            }
                        }
                        else showFixedColumnR = false;
                    }
                    else showFixedColumnR = false;
                }
                else showFixedColumnL = showFixedColumnR = false;

                #endregion

                if (bordered)
                {
                    using (var pen = new Pen(brush_split.Color, dividerHs[0].Width))
                    {
                        g.DrawRectangle(pen, rect_divider);
                    }
                }
            }

            scrollX.Paint(g);
            scrollY.Paint(g);

            base.OnPaint(e);
        }
        void PaintTableBgHeader(Graphics g, RowTemplate row)
        {
            using (var brush = new SolidBrush(Style.Db.TagDefaultBg))
            {
                g.FillRectangle(brush, row.RECT);
            }
        }
        void PaintTableHeader(Graphics g, RowTemplate row, SolidBrush fore)
        {
            using (var column_font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                foreach (TCellColumn column in row.cells)
                {
                    if (column.tag is ColumnCheck) PaintCheck(g, row, column);
                    else g.DrawString(column.value, column_font, fore, column.rect, StringF(column.align));
                }
            }
        }
        void PaintTableBg(Graphics g, RowTemplate row)
        {
            if (row.Checked)
            {
                using (var brush = rowSelectedBg.Brush(Style.Db.PrimaryBg))
                {
                    g.FillRectangle(brush, row.RECT);
                }
            }
            if (row.AnimationHover)
            {
                using (var brush = new SolidBrush(Color.FromArgb((int)(row.AnimationHoverValue * Style.Db.FillSecondary.A), Style.Db.FillSecondary)))
                {
                    g.FillRectangle(brush, row.RECT);
                }
            }
            else if (row.Hover)
            {
                using (var brush = new SolidBrush(Style.Db.FillSecondary))
                {
                    g.FillRectangle(brush, row.RECT);
                }
            }
        }
        void PaintItem(Graphics g, TCell it, SolidBrush fore)
        {
            if (it is TCellCheck check) PaintCheck(g, check);
            else if (it is TCellRadio radio) PaintRadio(g, radio);
            else if (it is Template obj)
            {
                foreach (var o in obj.value) o.Paint(g, it, Font, fore);
            }
            else if (it is TCellText text) g.DrawString(text.value, Font, fore, text.rect, StringF(text.align));
        }

        #region 渲染按钮

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

                using (var path = Path(rect_read, btn, _radius))
                {
                    #region 动画

                    if (template.AnimationClick)
                    {
                        float maxw = rect_read.Width + (gap * template.AnimationClickValue), maxh = rect_read.Height + (gap * template.AnimationClickValue);
                        int a = (int)(100 * (1f - template.AnimationClickValue));
                        using (var brush = new SolidBrush(Color.FromArgb(a, _color)))
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

                            using (var brush = new SolidBrush(Style.Db.DefaultBg))
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
                                PaintText(g, font, btn, _back_active, rect_read);
                            }
                            else if (template.AnimationHover)
                            {
                                var colorHover = Color.FromArgb(template.AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintText(g, font, btn, _fore, colorHover, rect_read);
                            }
                            else if (template.ExtraMouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintText(g, font, btn, _back_hover, rect_read);
                            }
                            else
                            {
                                using (var brush = new Pen(Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintText(g, font, btn, _fore, rect_read);
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
                                using (var brush = new Pen(Style.Db.BorderColorDisable, btn.BorderWidth * Config.Dpi))
                                {
                                    g.DrawPath(brush, path);
                                }
                            }
                            else if (template.AnimationHover)
                            {
                                using (var brush = new SolidBrush(Color.FromArgb(template.AnimationHoverValue, _back_hover)))
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
                            PaintText(g, font, btn, _fore, rect_read);
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
                        PaintText(g, font, btn, Style.Db.TextQuaternary, rect_read);
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

                using (var path = Path(rect_read, btn, _radius))
                {
                    #region 动画

                    if (template.AnimationClick)
                    {
                        float maxw = rect_read.Width + (gap * template.AnimationClickValue), maxh = rect_read.Height + (gap * template.AnimationClickValue);
                        int a = (int)(100 * (1f - template.AnimationClickValue));
                        using (var brush = new SolidBrush(Color.FromArgb(a, _back)))
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
                                PaintText(g, font, btn, _back_active, rect_read);
                            }
                            else if (template.AnimationHover)
                            {
                                var colorHover = Color.FromArgb(template.AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(btn.Enabled ? _back : Style.Db.FillTertiary, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintText(g, font, btn, _back, colorHover, rect_read);
                            }
                            else if (template.ExtraMouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintText(g, font, btn, _back_hover, rect_read);
                            }
                            else
                            {
                                using (var brush = new Pen(btn.Enabled ? _back : Style.Db.FillTertiary, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintText(g, font, btn, btn.Enabled ? _back : Style.Db.TextQuaternary, rect_read);
                            }
                        }
                        else PaintText(g, font, btn, btn.Enabled ? _back : Style.Db.TextQuaternary, rect_read);

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
                            var colorHover = Color.FromArgb(template.AnimationHoverValue, _back_hover);
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

                        PaintText(g, font, btn, btn.Enabled ? _fore : Style.Db.TextQuaternary, rect_read);
                    }
                }
            }
        }

        #region 渲染帮助

        internal static GraphicsPath Path(RectangleF rect_read, CellButton btn, float _radius)
        {
            if (btn.Shape == TShape.Circle)
            {
                var path = new GraphicsPath();
                path.AddEllipse(rect_read);
                return path;
            }
            return rect_read.RoundPath(_radius);
        }
        internal static void PaintText(Graphics g, Font font, CellButton btn, Color color, Rectangle rect_read)
        {
            var font_size = g.MeasureString(btn.Text, font);
            var rect = rect_read.IconRect(font_size.Height, false, btn.ShowArrow, false, false);
            if (btn.ShowArrow)
            {
                using (var pen = new Pen(color, 2F * Config.Dpi))
                {
                    pen.StartCap = pen.EndCap = LineCap.Round;
                    if (btn.IsLink)
                    {
                        float size2 = rect.r.Width / 2F;
                        g.TranslateTransform(rect.r.X + size2, rect.r.Y + size2);
                        g.RotateTransform(-90F);
                        g.DrawLines(pen, new RectangleF(-size2, -size2, rect.r.Width, rect.r.Height).TriangleLines(btn.ArrowProg));
                        g.ResetTransform();
                    }
                    else
                    {
                        g.DrawLines(pen, rect.r.TriangleLines(btn.ArrowProg));
                    }
                }
            }
            using (var brush = new SolidBrush(color))
            {
                g.DrawString(btn.Text, font, brush, rect.text, btn.stringFormat);
            }
        }
        internal static void PaintText(Graphics g, Font font, CellButton btn, Color color, Color colorHover, Rectangle rect_read)
        {
            var font_size = g.MeasureString(btn.Text, font);
            var rect = rect_read.IconRect(font_size.Height, false, btn.ShowArrow, false, false);
            if (btn.ShowArrow)
            {
                using (var pen = new Pen(color, 2F * Config.Dpi))
                using (var penHover = new Pen(colorHover, pen.Width))
                {
                    penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                    if (btn.IsLink)
                    {
                        float size2 = rect.r.Width / 2F;
                        g.TranslateTransform(rect.r.X + size2, rect.r.Y + size2);
                        g.RotateTransform(-90F);
                        var rect_arrow = new RectangleF(-size2, -size2, rect.r.Width, rect.r.Height).TriangleLines(btn.ArrowProg);
                        g.DrawLines(pen, rect_arrow);
                        g.DrawLines(penHover, rect_arrow);
                        g.ResetTransform();
                    }
                    else
                    {
                        var rect_arrow = rect.r.TriangleLines(btn.ArrowProg);
                        g.DrawLines(pen, rect_arrow);
                        g.DrawLines(penHover, rect_arrow);
                    }
                }
            }


            using (var brush = new SolidBrush(color))
            using (var brushHover = new SolidBrush(colorHover))
            {
                g.DrawString(btn.Text, font, brush, rect.text, btn.stringFormat);
                g.DrawString(btn.Text, font, brushHover, rect.text, btn.stringFormat);
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
                var colorHover = Color.FromArgb(template.AnimationHoverValue, Style.Db.PrimaryHover);
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

        #endregion

        #region 渲染复选框

        #region 复选框

        void PaintCheck(Graphics g, RowTemplate it, TCellColumn check)
        {
            using (var path_check = Helper.RoundPath(check.rect, check_radius, false))
            {
                if (it.AnimationCheck)
                {
                    int a = (int)(255 * it.AnimationCheckValue);

                    if (it.CheckState == CheckState.Indeterminate || (it.checkStateOld == CheckState.Indeterminate && !it.Checked))
                    {
                        using (var brush = new Pen(Style.Db.BorderColor, check_border))
                        {
                            g.DrawPath(brush, path_check);
                        }
                        using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.Primary)))
                        {
                            g.FillRectangle(brush, PaintBlock(check.rect));
                        }
                    }
                    else
                    {
                        float dot = check.rect.Width * 0.3F;

                        using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.Primary)))
                        {
                            g.FillPath(brush, path_check);
                        }
                        using (var brush = new Pen(Color.FromArgb(a, Style.Db.BgBase), check_border))
                        {
                            g.DrawLines(brush, PaintArrow(check.rect));
                        }

                        if (it.Checked)
                        {
                            float max = check.rect.Height + check.rect.Height * it.AnimationCheckValue;
                            int a2 = (int)(100 * (1f - it.AnimationCheckValue));
                            using (var brush = new SolidBrush(Color.FromArgb(a2, Style.Db.Primary)))
                            {
                                g.FillEllipse(brush, new RectangleF(check.rect.X + (check.rect.Width - max) / 2F, check.rect.Y + (check.rect.Height - max) / 2F, max, max));
                            }
                        }
                        using (var brush = new Pen(Style.Db.Primary, check_border))
                        {
                            g.DrawPath(brush, path_check);
                        }
                    }
                }
                else if (it.CheckState == CheckState.Indeterminate)
                {
                    using (var brush = new Pen(Style.Db.BorderColor, check_border))
                    {
                        g.DrawPath(brush, path_check);
                    }
                    using (var brush = new SolidBrush(Style.Db.Primary))
                    {
                        g.FillRectangle(brush, PaintBlock(check.rect));
                    }
                }
                else if (it.Checked)
                {
                    using (var brush = new SolidBrush(Style.Db.Primary))
                    {
                        g.FillPath(brush, path_check);
                    }
                    using (var brush = new Pen(Style.Db.BgBase, check_border))
                    {
                        g.DrawLines(brush, PaintArrow(check.rect));
                    }
                }
                else
                {
                    using (var brush = new Pen(Style.Db.BorderColor, check_border))
                    {
                        g.DrawPath(brush, path_check);
                    }
                }
            }
        }
        void PaintCheck(Graphics g, TCellCheck check)
        {
            using (var path_check = Helper.RoundPath(check.rect, check_radius, false))
            {
                if (check.AnimationCheck)
                {
                    int a = (int)(255 * check.AnimationCheckValue);

                    using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.Primary)))
                    {
                        g.FillPath(brush, path_check);
                    }
                    using (var brush = new Pen(Color.FromArgb(a, Style.Db.BgBase), check_border))
                    {
                        g.DrawLines(brush, PaintArrow(check.rect));
                    }

                    if (check.Checked)
                    {
                        float max = check.rect.Height + check.rect.Height * check.AnimationCheckValue;
                        int a2 = (int)(100 * (1f - check.AnimationCheckValue));
                        using (var brush = new SolidBrush(Color.FromArgb(a2, Style.Db.Primary)))
                        {
                            g.FillEllipse(brush, new RectangleF(check.rect.X + (check.rect.Width - max) / 2F, check.rect.Y + (check.rect.Height - max) / 2F, max, max));
                        }
                    }
                    using (var brush = new Pen(Style.Db.Primary, check_border))
                    {
                        g.DrawPath(brush, path_check);
                    }
                }
                else if (check.Checked)
                {
                    using (var brush = new SolidBrush(Style.Db.Primary))
                    {
                        g.FillPath(brush, path_check);
                    }
                    using (var brush = new Pen(Style.Db.BgBase, check_border))
                    {
                        g.DrawLines(brush, PaintArrow(check.rect));
                    }
                }
                else
                {
                    using (var brush = new Pen(Style.Db.BorderColor, check_border))
                    {
                        g.DrawPath(brush, path_check);
                    }
                }
            }
        }

        #endregion

        #region 单选框

        void PaintRadio(Graphics g, TCellRadio radio)
        {
            var color = Style.Db.Primary;
            var dot_size = radio.rect.Height;
            if (radio.AnimationCheck)
            {
                float dot = dot_size * 0.3F;
                using (var path = new GraphicsPath())
                {
                    float dot_ant = dot_size - dot * radio.AnimationCheckValue, dot_ant2 = dot_ant / 2F;
                    int a = (int)(255 * radio.AnimationCheckValue);
                    path.AddEllipse(radio.rect);
                    path.AddEllipse(new RectangleF(radio.rect.X + dot_ant2, radio.rect.Y + dot_ant2, radio.rect.Width - dot_ant, radio.rect.Height - dot_ant));
                    using (var brush = new SolidBrush(Color.FromArgb(a, color)))
                    {
                        g.FillPath(brush, path);
                    }
                }
                if (radio.Checked)
                {
                    float max = radio.rect.Height + radio.rect.Height * radio.AnimationCheckValue;
                    int a2 = (int)(100 * (1f - radio.AnimationCheckValue));
                    using (var brush = new SolidBrush(Color.FromArgb(a2, color)))
                    {
                        g.FillEllipse(brush, new RectangleF(radio.rect.X + (radio.rect.Width - max) / 2F, radio.rect.Y + (radio.rect.Height - max) / 2F, max, max));
                    }
                }
                using (var brush = new Pen(color, 2F))
                {
                    g.DrawEllipse(brush, radio.rect);
                }
            }
            else if (radio.Checked)
            {
                float dot = dot_size * 0.3F, dot2 = dot / 2F;
                using (var brush = new Pen(Color.FromArgb(250, color), dot))
                {
                    g.DrawEllipse(brush, new RectangleF(radio.rect.X + dot2, radio.rect.Y + dot2, radio.rect.Width - dot, radio.rect.Height - dot));
                }
                using (var brush = new Pen(color, 2F))
                {
                    g.DrawEllipse(brush, radio.rect);
                }
            }
            else
            {
                using (var brush = new Pen(Style.Db.BorderColor, 2F))
                {
                    g.DrawEllipse(brush, radio.rect);
                }
            }
        }

        #endregion

        RectangleF PaintBlock(RectangleF rect)
        {
            float size = rect.Height * 0.2F, size2 = size * 2F;
            return new RectangleF(rect.X + size, rect.Y + size, rect.Width - size2, rect.Height - size2);
        }
        PointF[] PaintArrow(RectangleF rect)
        {
            float size = rect.Height * 0.15F, size2 = rect.Height * 0.2F, size3 = rect.Height * 0.26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        #endregion

        static StringFormat StringF(ColumnAlign align)
        {
            switch (align)
            {
                case ColumnAlign.Center: return stringCenter;
                case ColumnAlign.Right: return stringRight;
                case ColumnAlign.Left:
                default: return stringLeft;
            }
        }
    }
}