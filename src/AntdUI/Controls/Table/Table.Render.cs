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
        static StringFormat stringLeft = Helper.SF_NoWrap(lr: StringAlignment.Near), stringCenter = Helper.SF_NoWrap(), stringRight = Helper.SF_NoWrap(lr: StringAlignment.Far);
        static StringFormat stringLeftEllipsis = Helper.SF_ALL(lr: StringAlignment.Near), stringCenterEllipsis = Helper.SF_ALL(), stringRightEllipsis = Helper.SF_ALL(lr: StringAlignment.Far);
        static StringFormat stringLeftN = Helper.SF(lr: StringAlignment.Near), stringCenterN = Helper.SF(), stringRightN = Helper.SF(lr: StringAlignment.Far);

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect = ClientRectangle;
            if (rows == null)
            {
                if (Empty) PaintEmpty(g, rect);
                base.OnPaint(e);
                return;
            }

            int sx = scrollBar.ValueX, sy = scrollBar.ValueY;
            using (var fore = new SolidBrush(Style.Db.Text))
            using (var brush_split = new SolidBrush(Style.Db.BorderColor))
            {
                var shows = new List<RowTemplate>();
                if (fixedHeader)
                {
                    foreach (var it in rows)
                    {
                        it.SHOW = !it.IsColumn && it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + rect_read.Height + it.RECT.Height;
                        if (it.SHOW) shows.Add(it);
                    }

                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows) PaintTableBgFront(g, it);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows) foreach (var cel in it.cells) PaintItemBack(g, cel);

                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows)
                    {
                        PaintTableBg(g, it);
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
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in rows)
                    {
                        it.SHOW = it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + rect_read.Height + it.RECT.Height;
                        if (it.SHOW) shows.Add(it);
                    }
                    foreach (var it in shows)
                    {
                        if (!it.IsColumn)
                        {
                            PaintTableBgFront(g, it);
                            foreach (var cel in it.cells) PaintItemBack(g, cel);
                        }
                    }
                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows)
                    {
                        if (it.IsColumn) PaintTableBgHeader(g, it);
                        else PaintTableBg(g, it);
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
                    PaintFixedColumnL(g, rect, rows, shows, fore, brush_split, sx, sy);
                    PaintFixedColumnR(g, rect, rows, shows, fore, brush_split, sx, sy);
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
            if (EmptyHeader && Empty && rows.Length == 1) PaintEmpty(g, rect);
            scrollBar.Paint(g);
            base.OnPaint(e);
        }

        void PaintTableBgHeader(Graphics g, RowTemplate row)
        {
            using (var brush = new SolidBrush(Style.Db.TagDefaultBg))
            {
                g.FillRectangle(brush, row.RECT);
            }
        }

        string arrow_up_svg = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M858.9 689L530.5 308.2c-9.4-10.9-27.5-10.9-37 0L165.1 689c-12.2 14.2-1.2 35 18.5 35h656.8c19.7 0 30.7-20.8 18.5-35z\"></path></svg>", arrow_down_svg = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M840.4 300H183.6c-19.7 0-30.7 20.8-18.5 35l328.4 380.8c9.4 10.9 27.5 10.9 37 0L858.9 335c12.2-14.2 1.2-35-18.5-35z\"></path></svg>";
        void PaintTableHeader(Graphics g, RowTemplate row, SolidBrush fore)
        {
            using (var column_font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                if (dragHeader != null)
                {
                    foreach (TCellColumn column in row.cells)
                    {
                        if (column.column.SortOrder)
                        {
                            using (var bmp = SvgExtend.GetImgExtend(arrow_up_svg, column.rect_up, column.column.SortMode == 1 ? Style.Db.Primary : Style.Db.TextQuaternary))
                            {
                                if (bmp != null)
                                    g.DrawImage(bmp, column.rect_up);
                            }
                            using (var bmp = SvgExtend.GetImgExtend(arrow_down_svg, column.rect_down, column.column.SortMode == 2 ? Style.Db.Primary : Style.Db.TextQuaternary))
                            {
                                if (bmp != null)
                                    g.DrawImage(bmp, column.rect_down);
                            }
                        }
                        if (column.column is ColumnCheck) PaintCheck(g, row, column);
                        else g.DrawString(column.value, column_font, fore, column.rect, StringF(column.column.ColAlign ?? column.column.Align));

                        if (dragHeader.i == column.INDEX)
                        {
                            using (var brush = new SolidBrush(Style.Db.FillSecondary))
                            {
                                g.FillRectangle(brush, column.RECT);
                            }
                        }
                        if (dragHeader.im == column.INDEX)
                        {
                            using (var brush_split = new SolidBrush(Style.Db.BorderColor))
                            {
                                int sp = (int)(2 * Config.Dpi);
                                if (dragHeader.last) g.FillRectangle(brush_split, new Rectangle(column.RECT.Right - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                                else g.FillRectangle(brush_split, new Rectangle(column.RECT.X - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                            }
                        }
                    }
                }
                else
                {
                    foreach (TCellColumn column in row.cells)
                    {
                        if (column.column.SortOrder)
                        {
                            using (var bmp = SvgExtend.GetImgExtend(arrow_up_svg, column.rect_up, column.column.SortMode == 1 ? Style.Db.Primary : Style.Db.TextQuaternary))
                            {
                                if (bmp != null)
                                    g.DrawImage(bmp, column.rect_up);
                            }
                            using (var bmp = SvgExtend.GetImgExtend(arrow_down_svg, column.rect_down, column.column.SortMode == 2 ? Style.Db.Primary : Style.Db.TextQuaternary))
                            {
                                if (bmp != null)
                                    g.DrawImage(bmp, column.rect_down);
                            }
                        }
                        if (column.column is ColumnCheck) PaintCheck(g, row, column);
                        else g.DrawString(column.value, column_font, fore, column.rect, StringF(column.column.ColAlign ?? column.column.Align));
                    }
                }
            }
        }
        void PaintTableBgFront(Graphics g, RowTemplate row)
        {
            var style = SetRowStyle?.Invoke(this, row.RECORD, row.INDEX);
            if (style != null)
            {
                using (var brush = new SolidBrush(style.BackColor))
                {
                    g.FillRectangle(brush, row.RECT);
                }
            }
            if (selectedIndex == row.INDEX || row.Checked)
            {
                using (var brush = rowSelectedBg.Brush(Style.Db.PrimaryBg))
                {
                    g.FillRectangle(brush, row.RECT);
                }
                if (selectedIndex == row.INDEX && row.Checked)
                {
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        g.FillRectangle(brush, row.RECT);
                    }
                }
            }
        }
        void PaintTableBg(Graphics g, RowTemplate row)
        {
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
        void PaintItemBack(Graphics g, TCell it)
        {
            if (it is Template obj)
            {
                foreach (var o in obj.value) o.PaintBack(g, it);
            }
        }
        void PaintItem(Graphics g, TCell it, SolidBrush fore)
        {
            if (it is TCellCheck check) PaintCheck(g, check);
            else if (it is TCellRadio radio) PaintRadio(g, radio);
            else if (it is TCellSwitch _switch) PaintSwitch(g, _switch);
            else if (it is Template obj)
            {
                foreach (var o in obj.value) o.Paint(g, it, Font, fore);
            }
            else if (it is TCellText text) g.DrawString(text.value, Font, fore, text.rect, StringF(text.column));
            if (dragHeader != null && dragHeader.i == it.INDEX)
            {
                using (var brush = new SolidBrush(Style.Db.FillSecondary))
                {
                    g.FillRectangle(brush, it.RECT);
                }
            }
        }

        void PaintFixedColumnL(Graphics g, Rectangle rect, RowTemplate[] rows, List<RowTemplate> shows, SolidBrush fore, SolidBrush brush_split, int sx, int sy)
        {
            if (fixedColumnL != null && sx > 0)
            {
                showFixedColumnL = true;
                var last = shows[shows.Count - 1].cells[fixedColumnL[fixedColumnL.Count - 1]];
                var rect_Fixed = new Rectangle(rect.X, rect.Y, last.RECT.Right, last.RECT.Bottom);

                #region 绘制阴影

                var rect_show = new Rectangle(rect.X + last.RECT.Right - _gap, rect.Y, _gap * 2, last.RECT.Bottom);
                using (var brush = new LinearGradientBrush(rect_show, Style.Db.FillSecondary, Color.Transparent, 0F))
                {
                    g.FillRectangle(brush, rect_show);
                }

                using (var brush = new SolidBrush(Style.Db.BgBase))
                {
                    g.FillRectangle(brush, rect_Fixed);
                }

                #endregion

                g.SetClip(rect_Fixed);
                g.TranslateTransform(0, -sy);
                foreach (var row in shows)
                {
                    if (row.IsColumn) { PaintTableBgHeader(g, row); PaintTableHeader(g, row, fore); }
                    else
                    {
                        PaintTableBgFront(g, row);
                        foreach (var cel in row.cells) PaintItemBack(g, cel);
                        PaintTableBg(g, row);
                    }
                }
                foreach (var row in shows)
                {
                    foreach (int fixedIndex in fixedColumnL)
                    {
                        if (!row.IsColumn) PaintItem(g, row.cells[fixedIndex], fore);
                    }
                }
                if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);
                g.ResetTransform();
                if (fixedHeader)
                {
                    PaintTableBgHeader(g, rows[0]);
                    PaintTableHeader(g, rows[0], fore);
                }
                if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);
                g.ResetClip();
            }
            else showFixedColumnL = false;
        }
        void PaintFixedColumnR(Graphics g, Rectangle rect, RowTemplate[] rows, List<RowTemplate> shows, SolidBrush fore, SolidBrush brush_split, int sx, int sy)
        {
            if (fixedColumnR != null && scrollBar.ShowX)
            {
                var lastrow = shows[shows.Count - 1];
                TCell first = lastrow.cells[fixedColumnR[fixedColumnR.Count - 1]], last = lastrow.cells[fixedColumnR[0]];
                if (sx + rect.Width < last.RECT.Right)
                {
                    sFixedR = last.RECT.Right - rect.Width;
                    showFixedColumnR = true;
                    int w = last.RECT.Right - first.RECT.Left;

                    var rect_Fixed = new Rectangle(rect.Right - w, rect.Y, last.RECT.Right, last.RECT.Bottom);

                    #region 绘制阴影

                    var rect_show = new Rectangle(rect.Right - w - _gap, rect.Y, _gap * 2, last.RECT.Bottom);
                    using (var brush = new LinearGradientBrush(rect_show, Color.Transparent, Style.Db.FillSecondary, 0F))
                    {
                        g.FillRectangle(brush, rect_show);
                    }

                    using (var brush = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillRectangle(brush, rect_Fixed);
                    }

                    #endregion

                    g.SetClip(rect_Fixed);
                    g.TranslateTransform(0, -sy);
                    foreach (var row in shows)
                    {
                        if (row.IsColumn)
                        {
                            PaintTableBgHeader(g, row);
                            g.ResetTransform();
                            g.TranslateTransform(-sFixedR, -sy);
                            PaintTableHeader(g, row, fore);
                            g.ResetTransform();
                            g.TranslateTransform(0, -sy);
                        }
                        else
                        {
                            PaintTableBgFront(g, row);
                            PaintTableBg(g, row);
                        }
                    }
                    g.ResetTransform();
                    g.TranslateTransform(-sFixedR, -sy);
                    foreach (var row in shows)
                    {
                        foreach (int fixedIndex in fixedColumnR)
                        {
                            if (!row.IsColumn)
                            {
                                PaintItemBack(g, row.cells[fixedIndex]);
                                PaintItem(g, row.cells[fixedIndex], fore);
                            }
                        }
                    }
                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);
                    g.ResetTransform();
                    if (fixedHeader)
                    {
                        PaintTableBgHeader(g, rows[0]);
                        g.TranslateTransform(-sFixedR, 0);
                        PaintTableHeader(g, rows[0], fore);
                    }
                    g.ResetTransform();
                    g.TranslateTransform(-sFixedR, 0);
                    if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);

                    g.ResetTransform();
                    g.ResetClip();
                }
                else showFixedColumnR = false;
            }
            else showFixedColumnR = false;
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

        #region 单选框

        void PaintSwitch(Graphics g, TCellSwitch _switch)
        {
            var color = Style.Db.Primary;
            using (var path = _switch.rect.RoundPath(_switch.rect.Height))
            {
                using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.FillPath(brush, path);
                    if (_switch.AnimationHover)
                    {
                        int a = (int)(brush.Color.A * _switch.AnimationHoverValue);
                        using (var brush2 = new SolidBrush(Color.FromArgb(a, brush.Color)))
                        {
                            g.FillPath(brush2, path);
                        }
                    }
                    else if (_switch.ExtraMouseHover) g.FillPath(brush, path);
                }
                float gap = (int)(2 * Config.Dpi), gap2 = gap * 2F;
                if (_switch.AnimationCheck)
                {
                    int a = (int)(255 * _switch.AnimationCheckValue);
                    using (var brush = new SolidBrush(Color.FromArgb(a, color)))
                    {
                        g.FillPath(brush, path);
                    }
                    var dot_rect = new RectangleF(_switch.rect.X + gap + (_switch.rect.Width - _switch.rect.Height) * _switch.AnimationCheckValue, _switch.rect.Y + gap, _switch.rect.Height - gap2, _switch.rect.Height - gap2);
                    using (var brush = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                }
                else if (_switch.Checked)
                {
                    var colorhover = Style.Db.PrimaryHover;
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillPath(brush, path);
                    }
                    if (_switch.AnimationHover)
                    {
                        int a = (int)(colorhover.A * _switch.AnimationHoverValue);
                        using (var brush2 = new SolidBrush(Color.FromArgb(a, colorhover)))
                        {
                            g.FillPath(brush2, path);
                        }
                    }
                    else if (_switch.ExtraMouseHover)
                    {
                        using (var brush = new SolidBrush(colorhover))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    var dot_rect = new RectangleF(_switch.rect.X + gap + _switch.rect.Width - _switch.rect.Height, _switch.rect.Y + gap, _switch.rect.Height - gap2, _switch.rect.Height - gap2);
                    using (var brush = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                    if (_switch.Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = _switch.rect.Height * .1F;
                        using (var brush = new Pen(color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            try
                            {
                                g.DrawArc(brush, dot_rect2, _switch.LineAngle, _switch.LineWidth * 3.6F);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    var dot_rect = new RectangleF(_switch.rect.X + gap, _switch.rect.Y + gap, _switch.rect.Height - gap2, _switch.rect.Height - gap2);
                    using (var brush = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                    if (_switch.Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = _switch.rect.Height * .1F;
                        using (var brush = new Pen(color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            try
                            {
                                g.DrawArc(brush, dot_rect2, _switch.LineAngle, _switch.LineWidth * 3.6F);
                            }
                            catch { }
                        }
                    }
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

        void PaintEmpty(Graphics g, Rectangle rect)
        {
            using (var fore = new SolidBrush(Style.Db.Text))
            {
                string emptytext = EmptyText ?? Localization.Provider?.GetLocalizedString("NoData") ?? "暂无数据";
                if (EmptyImage == null) g.DrawString(emptytext, Font, fore, rect, stringCenter);
                else
                {
                    int gap = (int)(_gap * Config.Dpi);
                    var size = g.MeasureString(emptytext, Font);
                    RectangleF rect_img = new RectangleF(rect.X + (rect.Width - EmptyImage.Width) / 2F, rect.Y + (rect.Height - EmptyImage.Height) / 2F - size.Height, EmptyImage.Width, EmptyImage.Height),
                        rect_font = new RectangleF(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);
                    g.DrawImage(EmptyImage, rect_img);
                    g.DrawString(emptytext, Font, fore, rect_font, stringCenter);
                }
            }
        }

        static StringFormat StringF(Column column)
        {
            if (column.LineBreak)
            {
                switch (column.Align)
                {
                    case ColumnAlign.Center: return stringCenterN;
                    case ColumnAlign.Right: return stringRightN;
                    case ColumnAlign.Left:
                    default: return stringLeftN;
                }
            }
            else if (column.Ellipsis)
            {
                switch (column.Align)
                {
                    case ColumnAlign.Center: return stringCenterEllipsis;
                    case ColumnAlign.Right: return stringRightEllipsis;
                    case ColumnAlign.Left:
                    default: return stringLeftEllipsis;
                }
            }
            else
            {
                switch (column.Align)
                {
                    case ColumnAlign.Center: return stringCenter;
                    case ColumnAlign.Right: return stringRight;
                    case ColumnAlign.Left:
                    default: return stringLeft;
                }
            }
        }
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