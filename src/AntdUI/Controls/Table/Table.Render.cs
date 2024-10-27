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
        internal static StringFormat stringLeft = Helper.SF_NoWrap(lr: StringAlignment.Near), stringCenter = Helper.SF_NoWrap(), stringRight = Helper.SF_NoWrap(lr: StringAlignment.Far);
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
            if (columnfont == null)
            {
                using (var column_font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    PaintTable(g, rows, rect, column_font);
                }
            }
            else PaintTable(g, rows, rect, columnfont);
            if (emptyHeader && Empty && rows.Length == 1) PaintEmpty(g, rect);
            ScrollBar.Paint(g);
            base.OnPaint(e);
        }

        void PaintTable(Graphics g, RowTemplate[] rows, Rectangle rect, Font column_font)
        {
            float _radius = radius * Config.Dpi;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            using (var fore = new SolidBrush(Style.Db.Text))
            using (var forecolumn = new SolidBrush(columnfore ?? Style.Db.Text))
            using (var brush_split = new SolidBrush(borderColor ?? Style.Db.BorderColor))
            {
                var shows = new List<StyleRow>(rows.Length);
                if (visibleHeader)
                {
                    if (_radius > 0)
                    {
                        using (var path = Helper.RoundPath(rect_divider, _radius, true, true, false, false))
                        {
                            g.SetClip(path);
                        }
                    }
                    if (fixedHeader)
                    {
                        foreach (var it in rows)
                        {
                            it.SHOW = it.ShowExpand && !it.IsColumn && (rect_read.Contains(rect_read.X, it.RECT.Y - sy) || rect_read.Contains(rect_read.X, it.RECT.Bottom - sy));
                            if (it.SHOW) shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX))));
                        }

                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows) PaintBgRowFront(g, it);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows) PaintBgRowItem(g, it.row);

                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows) PaintBg(g, it.row);

                        if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows) PaintForeItem(g, it, fore);
                        g.ResetTransform();

                        g.ResetClip();

                        PaintTableBgHeader(g, rows[0], _radius);

                        g.TranslateTransform(-sx, 0);

                        PaintTableHeader(g, rows[0], forecolumn, column_font, _radius);

                        if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);
                    }
                    else
                    {
                        foreach (var it in rows)
                        {
                            it.SHOW = it.ShowExpand && it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + rect_read.Height + it.RECT.Height;
                            if (it.SHOW) shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX))));
                        }
                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows)
                        {
                            if (!it.row.IsColumn) PaintBgRowFront(g, it);
                        }
                        g.TranslateTransform(-sx, 0);
                        foreach (var it in shows)
                        {
                            if (!it.row.IsColumn) PaintBgRowItem(g, it.row);
                        }

                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows)
                        {
                            if (it.row.IsColumn) PaintTableBgHeader(g, it.row, _radius);
                            else PaintBg(g, it.row);
                        }

                        if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows)
                        {
                            if (it.row.IsColumn) PaintTableHeader(g, it.row, forecolumn, column_font, _radius);
                            else PaintForeItem(g, it, fore);
                        }
                        if (bordered)
                        {
                            g.ResetTransform();
                            g.TranslateTransform(-sx, 0);
                        }
                        if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);
                    }
                }
                else
                {
                    if (_radius > 0)
                    {
                        using (var path = Helper.RoundPath(rect_divider, _radius))
                        {
                            g.SetClip(path);
                        }
                    }
                    rows[0].SHOW = false;
                    for (int index_r = 1; index_r < rows.Length; index_r++)
                    {
                        var it = rows[index_r];
                        it.SHOW = it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + rect_read.Height + it.RECT.Height;
                        if (it.SHOW) shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX))));
                    }

                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows) PaintBgRowFront(g, it);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows) PaintBgRowItem(g, it.row);

                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows) PaintBg(g, it.row);

                    if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows) PaintForeItem(g, it, fore);
                    if (bordered)
                    {
                        g.ResetTransform();
                        g.TranslateTransform(-sx, 0);
                    }
                    if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);
                }

                g.ResetClip();
                g.ResetTransform();

                #region 渲染浮动列

                if (shows.Count > 0 && (fixedColumnL != null || fixedColumnR != null))
                {
                    PaintFixedColumnL(g, rect, rows, shows, fore, forecolumn, column_font, brush_split, sx, sy, _radius);
                    PaintFixedColumnR(g, rect, rows, shows, fore, forecolumn, column_font, brush_split, sx, sy, _radius);
                }
                else showFixedColumnL = showFixedColumnR = false;

                #endregion

                if (bordered)
                {
                    var splitsize = dividerHs.Length > 0 ? dividerHs[0].Width : (int)(1F * Config.Dpi);
                    if (_radius > 0)
                    {
                        using (var pen = new Pen(brush_split.Color, splitsize))
                        {
                            if (visibleHeader)
                            {
                                using (var path = Helper.RoundPath(rect_divider, _radius, true, true, false, false))
                                {
                                    g.DrawPath(pen, path);
                                }
                            }
                            else
                            {
                                using (var path = Helper.RoundPath(rect_divider, _radius))
                                {
                                    g.DrawPath(pen, path);
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var pen = new Pen(brush_split.Color, splitsize))
                        {
                            g.DrawRectangle(pen, rect_divider);
                        }
                    }
                }
            }
        }

        #region 表头

        void PaintTableBgHeader(Graphics g, RowTemplate row, float radius)
        {
            using (var brush = new SolidBrush(columnback ?? Style.Db.TagDefaultBg))
            {
                if (radius > 0)
                {
                    using (var path = Helper.RoundPath(row.RECT, radius, true, true, false, false))
                    {
                        g.FillPath(brush, path);
                    }
                }
                else g.FillRectangle(brush, row.RECT);
            }
            foreach (var cel in row.cells)
            {
                if (cel.COLUMN.ColStyle != null && cel.COLUMN.ColStyle.BackColor.HasValue)
                {
                    using (var brush = new SolidBrush(cel.COLUMN.ColStyle.BackColor.Value))
                    {
                        g.FillRectangle(brush, cel.RECT);
                    }
                }
            }
        }
        void PaintTableHeader(Graphics g, RowTemplate row, SolidBrush fore, Font column_font, float radius)
        {
            foreach (TCellColumn column in row.cells)
            {
                if (column.COLUMN.SortOrder)
                {
                    g.GetImgExtend(SvgDb.IcoArrowUp, column.rect_up, column.COLUMN.SortMode == 1 ? Style.Db.Primary : Style.Db.TextQuaternary);
                    g.GetImgExtend(SvgDb.IcoArrowDown, column.rect_down, column.COLUMN.SortMode == 2 ? Style.Db.Primary : Style.Db.TextQuaternary);
                }
                if (column.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle) PaintCheck(g, column, columnCheck);
                else
                {
                    if (column.COLUMN.ColStyle != null && column.COLUMN.ColStyle.ForeColor.HasValue)
                    {
                        using (var brush = new SolidBrush(column.COLUMN.ColStyle.ForeColor.Value))
                        {
                            g.DrawStr(column.value, column_font, brush, column.RECT_REAL, StringF(column.COLUMN.ColAlign ?? column.COLUMN.Align));
                        }
                    }
                    else g.DrawStr(column.value, column_font, fore, column.RECT_REAL, StringF(column.COLUMN.ColAlign ?? column.COLUMN.Align));
                }
            }
            if (dragHeader == null) return;
            foreach (var column in row.cells)
            {
                if (dragHeader.i == column.INDEX)
                {
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        if (radius > 0)
                        {
                            if (column.INDEX == 0)
                            {
                                using (var path = Helper.RoundPath(column.RECT, radius, true, false, false, false))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else if (column.INDEX == row.cells.Length - 1)
                            {
                                using (var path = Helper.RoundPath(column.RECT, radius, false, true, false, false))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else g.FillRectangle(brush, column.RECT);
                        }
                        else g.FillRectangle(brush, column.RECT);
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

        #endregion

        #region 表体

        /// <summary>
        /// 渲染背景行（前置）
        /// </summary>
        void PaintBgRowFront(Graphics g, StyleRow row)
        {
            if (row.style != null && row.style.BackColor.HasValue)
            {
                using (var brush = new SolidBrush(row.style.BackColor.Value))
                {
                    g.FillRectangle(brush, row.row.RECT);
                }
            }
            if (selectedIndex == row.row.INDEX || row.row.Select)
            {
                using (var brush = rowSelectedBg.Brush(Style.Db.PrimaryBg))
                {
                    g.FillRectangle(brush, row.row.RECT);
                }
                if (selectedIndex == row.row.INDEX && row.row.Select)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(40, Style.Db.PrimaryActive)))
                    {
                        g.FillRectangle(brush, row.row.RECT);
                    }
                }
            }

            foreach (var cel in row.row.cells)
            {
                if (cel.COLUMN.Style != null && cel.COLUMN.Style.BackColor.HasValue)
                {
                    using (var brush = new SolidBrush(cel.COLUMN.Style.BackColor.Value))
                    {
                        g.FillRectangle(brush, cel.RECT);
                    }
                }
            }
        }

        /// <summary>
        /// 渲染背景行
        /// </summary>
        void PaintBg(Graphics g, RowTemplate row)
        {
            if (row.AnimationHover)
            {
                using (var brush = new SolidBrush(Helper.ToColorN(row.AnimationHoverValue, Style.Db.FillSecondary)))
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

        #region 单元格

        /// <summary>
        /// 渲染背景行
        /// </summary>
        void PaintBgRowItem(Graphics g, RowTemplate row)
        {
            foreach (var cel in row.cells) PaintItemBg(g, cel);
        }

        /// <summary>
        /// 渲染单元格背景
        /// </summary>
        void PaintItemBg(Graphics g, TCell it)
        {
            if (it is Template obj)
            {
                foreach (var o in obj.value) o.Value.PaintBack(g);
            }
        }

        /// <summary>
        /// 渲染前景行
        /// </summary>
        void PaintForeItem(Graphics g, StyleRow row, SolidBrush fore)
        {
            if (selectedIndex == row.row.INDEX && rowSelectedFore.HasValue)
            {
                using (var brush = new SolidBrush(rowSelectedFore.Value))
                {
                    for (int i = 0; i < row.row.cells.Length; i++) PaintItem(g, row.row.cells[i], brush);
                }
            }
            else if (row.style != null && row.style.ForeColor.HasValue)
            {
                using (var brush = new SolidBrush(row.style.ForeColor.Value))
                {
                    for (int i = 0; i < row.row.cells.Length; i++) PaintItem(g, row.row.cells[i], brush);
                }
            }
            else for (int i = 0; i < row.row.cells.Length; i++) PaintItem(g, row.row.cells[i], fore);
        }

        #region 渲染单元格

        void PaintItem(Graphics g, TCell it, SolidBrush fore)
        {
            if (it.COLUMN.Style == null || it.COLUMN.Style.ForeColor == null) PaintItem(g, it.INDEX, it, fore);
            else
            {
                using (var brush = new SolidBrush(it.COLUMN.Style.ForeColor.Value))
                {
                    PaintItem(g, it.INDEX, it, brush);
                }
            }
        }

        /// <summary>
        /// 渲染单元格（浮动）
        /// </summary>
        void PaintItemFixed(Graphics g, TCell it, SolidBrush fore, CellStyleInfo? style)
        {
            if (selectedIndex == it.ROW.INDEX && rowSelectedFore.HasValue)
            {
                using (var brush = new SolidBrush(rowSelectedFore.Value))
                {
                    PaintItem(g, it.INDEX, it, brush);
                }
            }
            else if (style != null && style.ForeColor.HasValue)
            {
                using (var brush = new SolidBrush(style.ForeColor.Value))
                {
                    PaintItem(g, it.INDEX, it, brush);
                }
            }
            else if (it.COLUMN.Style == null || it.COLUMN.Style.ForeColor == null) PaintItem(g, it.INDEX, it, fore);
            else
            {
                using (var brush = new SolidBrush(it.COLUMN.Style.ForeColor.Value))
                {
                    PaintItem(g, it.INDEX, it, brush);
                }
            }
        }

        void PaintItem(Graphics g, int columnIndex, TCell it, SolidBrush fore)
        {
            if (it is TCellCheck check) PaintCheck(g, check);
            else if (it is TCellRadio radio) PaintRadio(g, radio);
            else if (it is TCellSwitch _switch) PaintSwitch(g, _switch);
            else if (it is Template obj)
            {
                foreach (var o in obj.value) o.Value.Paint(g, Font, fore);
            }
            else if (it is TCellText text)
            {
                var state = g.Save();
                g.SetClip(it.RECT);
                g.DrawStr(text.value, Font, fore, text.RECT_REAL, StringF(text.COLUMN));
                g.Restore(state);
            }
            if (dragHeader != null && dragHeader.i == it.INDEX)
            {
                using (var brush = new SolidBrush(Style.Db.FillSecondary))
                {
                    g.FillRectangle(brush, it.RECT);
                }
            }
            if (it.ROW.CanExpand && it.ROW.KeyTreeINDEX == columnIndex)
            {
                using (var path_check = Helper.RoundPath(it.ROW.RectExpand, check_radius, false))
                {
                    using (var brush_bg = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillPath(brush_bg, path_check);
                    }
                    using (var brush = new Pen(Style.Db.BorderColor, check_border))
                    {
                        g.DrawPath(brush, path_check);
                    }
                    PaintArrow(g, it.ROW, fore, it.ROW.Expand ? 90 : 0);
                }
            }
        }

        #endregion

        void PaintArrow(Graphics g, RowTemplate item, SolidBrush color, int ArrowProg)
        {
            int size = item.RectExpand.Width, size_arrow = size / 2;
            var state = g.Save();
            g.TranslateTransform(item.RectExpand.X + size_arrow, item.RectExpand.Y + size_arrow);
            g.RotateTransform(-90F + ArrowProg);
            using (var pen = new Pen(color, check_border * 2))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, item.RectExpand.Width, item.RectExpand.Height).TriangleLines(-1, .6F));
            }
            g.Restore(state);
        }

        #endregion

        #region 浮动列

        void PaintFixedColumnL(Graphics g, Rectangle rect, RowTemplate[] rows, List<StyleRow> shows, SolidBrush fore, SolidBrush forecolumn, Font column_font, SolidBrush brush_split, int sx, int sy, float radius)
        {
            if (fixedColumnL != null && sx > 0)
            {
                showFixedColumnL = true;
                var last = shows[shows.Count - 1].row.cells[fixedColumnL[fixedColumnL.Count - 1]];
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
                    if (row.row.IsColumn) { PaintTableBgHeader(g, row.row, radius); PaintTableHeader(g, row.row, forecolumn, column_font, radius); }
                    else
                    {
                        PaintBgRowFront(g, row);
                        PaintBgRowItem(g, row.row);
                        PaintBg(g, row.row);
                    }
                }
                foreach (var row in shows)
                {
                    foreach (int fixedIndex in fixedColumnL)
                    {
                        if (!row.row.IsColumn) PaintItemFixed(g, row.row.cells[fixedIndex], fore, row.style);
                    }
                }
                if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);
                g.ResetTransform();
                if (fixedHeader)
                {
                    PaintTableBgHeader(g, rows[0], radius);
                    PaintTableHeader(g, rows[0], forecolumn, column_font, radius);
                }
                if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.FillRectangle(brush_split, divider);
                g.ResetClip();
            }
            else showFixedColumnL = false;
        }
        void PaintFixedColumnR(Graphics g, Rectangle rect, RowTemplate[] rows, List<StyleRow> shows, SolidBrush fore, SolidBrush forecolumn, Font column_font, SolidBrush brush_split, int sx, int sy, float radius)
        {
            if (fixedColumnR != null && ScrollBar.ShowX)
            {
                var lastrow = shows[shows.Count - 1];
                TCell first = lastrow.row.cells[fixedColumnR[fixedColumnR.Count - 1]], last = lastrow.row.cells[fixedColumnR[0]];
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
                        if (row.row.IsColumn)
                        {
                            PaintTableBgHeader(g, row.row, radius);
                            g.ResetTransform();
                            g.TranslateTransform(-sFixedR, -sy);
                            PaintTableHeader(g, row.row, forecolumn, column_font, radius);
                            g.ResetTransform();
                            g.TranslateTransform(0, -sy);
                        }
                        else
                        {
                            PaintBgRowFront(g, row);
                            PaintBg(g, row.row);
                        }
                    }
                    g.ResetTransform();
                    g.TranslateTransform(-sFixedR, -sy);
                    foreach (var row in shows)
                    {
                        foreach (int fixedIndex in fixedColumnR)
                        {
                            if (!row.row.IsColumn)
                            {
                                PaintItemBg(g, row.row.cells[fixedIndex]);
                                PaintItemFixed(g, row.row.cells[fixedIndex], fore, row.style);
                            }
                        }
                    }
                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    if (dividers.Length > 0) foreach (var divider in dividers) g.FillRectangle(brush_split, divider);
                    g.ResetTransform();
                    if (fixedHeader)
                    {
                        PaintTableBgHeader(g, rows[0], radius);
                        g.TranslateTransform(-sFixedR, 0);
                        PaintTableHeader(g, rows[0], forecolumn, column_font, radius);
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

        #endregion

        #endregion

        #region 渲染复选框

        #region 复选框

        void PaintCheck(Graphics g, TCellColumn check, ColumnCheck columnCheck)
        {
            using (var path_check = Helper.RoundPath(check.RECT_REAL, check_radius, false))
            {
                if (columnCheck.AnimationCheck)
                {
                    using (var brush_bg = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillPath(brush_bg, path_check);
                    }
                    var alpha = 255 * columnCheck.AnimationCheckValue;
                    if (columnCheck.CheckState == CheckState.Indeterminate || (columnCheck.checkStateOld == CheckState.Indeterminate && !columnCheck.Checked))
                    {
                        using (var brush = new Pen(Style.Db.BorderColor, check_border))
                        {
                            g.DrawPath(brush, path_check);
                        }
                        using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.Primary)))
                        {
                            g.FillRectangle(brush, PaintBlock(check.RECT_REAL));
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.Primary)))
                        {
                            g.FillPath(brush, path_check);
                        }
                        using (var brush = new Pen(Helper.ToColor(alpha, Style.Db.BgBase), check_border * 2))
                        {
                            g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                        }
                        if (columnCheck.Checked)
                        {
                            float max = check.RECT_REAL.Height + check.RECT_REAL.Height * columnCheck.AnimationCheckValue, alpha2 = 100 * (1F - columnCheck.AnimationCheckValue);
                            using (var brush = new SolidBrush(Helper.ToColor(alpha2, Style.Db.Primary)))
                            {
                                g.FillEllipse(brush, new RectangleF(check.RECT_REAL.X + (check.RECT_REAL.Width - max) / 2F, check.RECT_REAL.Y + (check.RECT_REAL.Height - max) / 2F, max, max));
                            }
                        }
                        using (var brush = new Pen(Style.Db.Primary, check_border))
                        {
                            g.DrawPath(brush, path_check);
                        }
                    }
                }
                else if (columnCheck.CheckState == CheckState.Indeterminate)
                {
                    using (var brush_bg = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillPath(brush_bg, path_check);
                    }
                    using (var brush = new Pen(Style.Db.BorderColor, check_border))
                    {
                        g.DrawPath(brush, path_check);
                    }
                    using (var brush = new SolidBrush(Style.Db.Primary))
                    {
                        g.FillRectangle(brush, PaintBlock(check.RECT_REAL));
                    }
                }
                else if (columnCheck.Checked)
                {
                    using (var brush = new SolidBrush(Style.Db.Primary))
                    {
                        g.FillPath(brush, path_check);
                    }
                    using (var brush = new Pen(Style.Db.BgBase, check_border * 2))
                    {
                        g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                    }
                }
                else
                {
                    using (var brush_bg = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillPath(brush_bg, path_check);
                    }
                    using (var brush = new Pen(Style.Db.BorderColor, check_border))
                    {
                        g.DrawPath(brush, path_check);
                    }
                }
            }
        }
        void PaintCheck(Graphics g, TCellCheck check)
        {
            using (var path_check = Helper.RoundPath(check.RECT_REAL, check_radius, false))
            {
                if (check.AnimationCheck)
                {
                    using (var brush_bg = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillPath(brush_bg, path_check);
                    }

                    var alpha = 255 * check.AnimationCheckValue;

                    using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.Primary)))
                    {
                        g.FillPath(brush, path_check);
                    }
                    using (var brush = new Pen(Helper.ToColor(alpha, Style.Db.BgBase), check_border * 2))
                    {
                        g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                    }

                    if (check.Checked)
                    {
                        float max = check.RECT_REAL.Height + check.RECT_REAL.Height * check.AnimationCheckValue, alpha2 = 100 * (1F - check.AnimationCheckValue);
                        using (var brush = new SolidBrush(Helper.ToColor(alpha2, Style.Db.Primary)))
                        {
                            g.FillEllipse(brush, new RectangleF(check.RECT_REAL.X + (check.RECT_REAL.Width - max) / 2F, check.RECT_REAL.Y + (check.RECT_REAL.Height - max) / 2F, max, max));
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
                    using (var brush = new Pen(Style.Db.BgBase, check_border * 2))
                    {
                        g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                    }
                }
                else
                {
                    using (var brush_bg = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillPath(brush_bg, path_check);
                    }
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
            var dot_size = radio.RECT_REAL.Height;
            using (var brush_bg = new SolidBrush(Style.Db.BgBase))
            {
                g.FillEllipse(brush_bg, radio.RECT_REAL);
            }
            if (radio.AnimationCheck)
            {
                float dot = dot_size * 0.3F;
                using (var path = new GraphicsPath())
                {
                    float dot_ant = dot_size - dot * radio.AnimationCheckValue, dot_ant2 = dot_ant / 2F, alpha = 255 * radio.AnimationCheckValue;
                    path.AddEllipse(radio.RECT_REAL);
                    path.AddEllipse(new RectangleF(radio.RECT_REAL.X + dot_ant2, radio.RECT_REAL.Y + dot_ant2, radio.RECT_REAL.Width - dot_ant, radio.RECT_REAL.Height - dot_ant));
                    using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.Primary)))
                    {
                        g.FillPath(brush, path);
                    }
                }
                if (radio.Checked)
                {
                    float max = radio.RECT_REAL.Height + radio.RECT_REAL.Height * radio.AnimationCheckValue, alpha2 = 100 * (1F - radio.AnimationCheckValue);
                    using (var brush = new SolidBrush(Helper.ToColor(alpha2, Style.Db.Primary)))
                    {
                        g.FillEllipse(brush, new RectangleF(radio.RECT_REAL.X + (radio.RECT_REAL.Width - max) / 2F, radio.RECT_REAL.Y + (radio.RECT_REAL.Height - max) / 2F, max, max));
                    }
                }
                using (var brush = new Pen(Style.Db.Primary, check_border))
                {
                    g.DrawEllipse(brush, radio.RECT_REAL);
                }
            }
            else if (radio.Checked)
            {
                float dot = dot_size * 0.3F, dot2 = dot / 2F;
                using (var brush = new Pen(Color.FromArgb(250, Style.Db.Primary), dot))
                {
                    g.DrawEllipse(brush, new RectangleF(radio.RECT_REAL.X + dot2, radio.RECT_REAL.Y + dot2, radio.RECT_REAL.Width - dot, radio.RECT_REAL.Height - dot));
                }
                using (var brush = new Pen(Style.Db.Primary, check_border))
                {
                    g.DrawEllipse(brush, radio.RECT_REAL);
                }
            }
            else
            {
                using (var brush = new Pen(Style.Db.BorderColor, check_border))
                {
                    g.DrawEllipse(brush, radio.RECT_REAL);
                }
            }
        }

        #endregion

        #region 开关

        void PaintSwitch(Graphics g, TCellSwitch _switch)
        {
            var color = Style.Db.Primary;
            using (var path = _switch.RECT_REAL.RoundPath(_switch.RECT_REAL.Height))
            {
                using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.FillPath(brush, path);
                    if (_switch.AnimationHover)
                    {
                        using (var brush2 = new SolidBrush(Helper.ToColorN(_switch.AnimationHoverValue, brush.Color)))
                        {
                            g.FillPath(brush2, path);
                        }
                    }
                    else if (_switch.ExtraMouseHover) g.FillPath(brush, path);
                }
                float gap = (int)(2 * Config.Dpi), gap2 = gap * 2F;
                if (_switch.AnimationCheck)
                {
                    var alpha = 255 * _switch.AnimationCheckValue;
                    using (var brush = new SolidBrush(Helper.ToColor(alpha, color)))
                    {
                        g.FillPath(brush, path);
                    }
                    var dot_rect = new RectangleF(_switch.RECT_REAL.X + gap + (_switch.RECT_REAL.Width - _switch.RECT_REAL.Height) * _switch.AnimationCheckValue, _switch.RECT_REAL.Y + gap, _switch.RECT_REAL.Height - gap2, _switch.RECT_REAL.Height - gap2);
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
                        using (var brush2 = new SolidBrush(Helper.ToColorN(_switch.AnimationHoverValue, colorhover)))
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
                    var dot_rect = new RectangleF(_switch.RECT_REAL.X + gap + _switch.RECT_REAL.Width - _switch.RECT_REAL.Height, _switch.RECT_REAL.Y + gap, _switch.RECT_REAL.Height - gap2, _switch.RECT_REAL.Height - gap2);
                    using (var brush = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                    if (_switch.Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = _switch.RECT_REAL.Height * .1F;
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
                    var dot_rect = new RectangleF(_switch.RECT_REAL.X + gap, _switch.RECT_REAL.Y + gap, _switch.RECT_REAL.Height - gap2, _switch.RECT_REAL.Height - gap2);
                    using (var brush = new SolidBrush(Style.Db.BgBase))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                    if (_switch.Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = _switch.RECT_REAL.Height * .1F;
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
                if (EmptyImage == null) g.DrawStr(emptytext, Font, fore, rect, stringCenter);
                else
                {
                    int gap = (int)(_gap * Config.Dpi);
                    var size = g.MeasureString(emptytext, Font).Size();
                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - EmptyImage.Width) / 2, rect.Y + (rect.Height - EmptyImage.Height) / 2 - size.Height, EmptyImage.Width, EmptyImage.Height),
                        rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);
                    g.DrawImage(EmptyImage, rect_img);
                    g.DrawStr(emptytext, Font, fore, rect_font, stringCenter);
                }
            }
        }

        internal static StringFormat StringF(Column column)
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