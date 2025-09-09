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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            if (rows == null)
            {
                if (Empty) PaintEmpty(g, e.Rect, 0);
                base.OnDraw(e);
                return;
            }
            if (columnfont == null)
            {
                using (var column_font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    PaintTable(g, rows, e.Rect, e.Rect.PaddingRect(Padding, borderWidth), column_font);
                }
            }
            else PaintTable(g, rows, e.Rect, e.Rect.PaddingRect(Padding, borderWidth), columnfont);
            if (emptyHeader && Empty && rows.Length == 1) PaintEmpty(g, e.Rect, rows[0].RECT.Height);
            ScrollBar.Paint(g);
            base.OnDraw(e);
        }

        void PaintTable(Canvas g, RowTemplate[] rows, Rectangle rect, Rectangle rect_real, Font column_font)
        {
            float _radius = radius * Config.Dpi;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            using (var brush_fore = new SolidBrush(fore ?? Colour.Text.Get("Table", ColorScheme)))
            using (var brush_foreEnable = new SolidBrush(fore ?? Colour.TextQuaternary.Get("Table", ColorScheme)))
            using (var brush_forecolumn = new SolidBrush(columnfore ?? fore ?? Colour.Text.Get("Table", ColorScheme)))
            using (var pen_cell_split = new Pen(borderColor ?? Colour.BorderColor.Get("Table", ColorScheme), BorderCellWidth * Config.Dpi))
            {
                StyleRow[] shows, summarys;
                GraphicsPath? clipath = null;
                if (visibleHeader)
                {
                    if (_radius > 0)
                    {
                        clipath = Helper.RoundPath(rect_divider, _radius, true, true, false, false);
                        g.SetClip(clipath);
                    }
                    else g.SetClip(rect_divider);
                    if (fixedHeader)
                    {
                        DirtyByVisibleHeaderFixedHeader(rows, sy, out shows, out summarys);

                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows) PaintBgRowFront(g, it);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows) PaintItemBgRowStyle(g, it.row);

                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows) PaintBg(g, it.row);

                        if (dividers.Length > 0)
                        {
                            foreach (var divider in dividers) g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                        }

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows) PaintForeItem(g, it, brush_fore, brush_foreEnable);
                        g.ResetTransform();

                        PaintTableBgHeader(g, rows[0], _radius, sx);

                        g.TranslateTransform(-sx, 0);

                        PaintTableHeader(g, rows[0], brush_forecolumn, column_font, _radius);

                        if (sy == 0 && dividers.Length > 0)
                        {
                            g.ResetTransform();
                            g.TranslateTransform(0, -sy);
                            var divider = dividers[0];
                            g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                            g.ResetTransform();
                            g.TranslateTransform(-sx, 0);
                        }

                        if (dividerHs.Length > 0)
                        {
                            foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                        }
                    }
                    else
                    {
                        DirtyByVisibleHeader(rows, sy, out shows, out summarys);

                        g.TranslateTransform(0, -sy);
                        var showsNoColumn = new List<StyleRow>(shows.Length);
                        foreach (var it in shows)
                        {
                            if (it.row.IsOther)
                            {
                                showsNoColumn.Add(it);
                                PaintBgRowFront(g, it);
                            }
                        }
                        g.TranslateTransform(-sx, 0);
                        foreach (var it in showsNoColumn) PaintItemBgRowStyle(g, it.row);

                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows)
                        {
                            if (it.row.IsColumn) PaintTableBgHeader(g, it.row, _radius, sx);
                            else PaintBg(g, it.row);
                        }

                        if (dividers.Length > 0)
                        {
                            foreach (var divider in dividers) g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                        }

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows)
                        {
                            if (it.row.IsColumn) PaintTableHeader(g, it.row, brush_forecolumn, column_font, _radius);
                            else PaintForeItem(g, it, brush_fore, brush_foreEnable);
                        }
                        if (bordered)
                        {
                            g.ResetTransform();
                            g.TranslateTransform(-sx, 0);
                        }
                        if (dividerHs.Length > 0)
                        {
                            foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                        }
                    }
                }
                else
                {
                    if (_radius > 0)
                    {
                        clipath = Helper.RoundPath(rect_divider, _radius);
                        g.SetClip(clipath);
                    }
                    else g.SetClip(rect_divider);
                    DirtyByNone(rows, sy, out shows, out summarys);

                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows) PaintBgRowFront(g, it);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows) PaintItemBgRow(g, it.row);

                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows) PaintBg(g, it.row);

                    if (dividers.Length > 0)
                    {
                        foreach (var divider in dividers) g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                    }

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows) PaintForeItem(g, it, brush_fore, brush_foreEnable);
                    if (bordered)
                    {
                        g.ResetTransform();
                        g.TranslateTransform(-sx, 0);
                    }
                    if (dividerHs.Length > 0)
                    {
                        foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                    }
                }

                g.ResetClip();
                g.ResetTransform();

                PaintMergeCells(g, rows, sx, sy, pen_cell_split.Color, brush_fore, brush_foreEnable);

                #region 渲染浮动列

                if (shows.Length > 0 && ScrollBar.ShowX && (fixedColumnL != null || fixedColumnR != null))
                {
                    PaintFixedColumnL(g, rect, rect_read, rows, shows, brush_fore, brush_foreEnable, brush_forecolumn, column_font, pen_cell_split, sx, sy, _radius);
                    PaintFixedColumnR(g, rect, rect_read, rows, shows, brush_fore, brush_foreEnable, brush_forecolumn, column_font, pen_cell_split, sx, sy, _radius);
                }
                else showFixedColumnL = showFixedColumnR = false;

                if (summarys.Length > 0) PaintFixedSummary(g, rect, rect_read, summarys, brush_fore, brush_foreEnable, brush_forecolumn, column_font, pen_cell_split, sx, sy, _radius);

                #endregion

                if (bordered)
                {
                    if (clipath == null) g.Draw(pen_cell_split.Color, borderWidth * Config.Dpi, rect_divider);
                    else g.Draw(pen_cell_split.Color, borderWidth * Config.Dpi, clipath);
                }

                clipath?.Dispose();
            }
        }

        #region 脏渲染

        void DirtyByVisibleHeaderFixedHeader(RowTemplate[] rows, int sy, out StyleRow[] d_shows, out StyleRow[] d_summarys)
        {
            int showIndex = 0;
            List<StyleRow> shows = new List<StyleRow>(rows.Length), summarys = new List<StyleRow>(1);
            foreach (var it in rows)
            {
                if (it.Type == RowType.Summary)
                {
                    it.SHOW = true;
                    var item = new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex)));
                    shows.Add(item);
                    summarys.Add(item);
                }
                else
                {
                    it.SHOW = it.ShowExpand && it.Type == RowType.None && (it.RECT.Y >= sy && it.RECT.Y <= sy + rect_read.Height || it.RECT.Bottom >= sy && it.RECT.Bottom <= sy + rect_read.Height);
                    if (it.SHOW) shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex))));
                }
                showIndex++;
            }
            d_shows = shows.ToArray();
            d_summarys = summarys.ToArray();
        }
        void DirtyByVisibleHeader(RowTemplate[] rows, int sy, out StyleRow[] d_shows, out StyleRow[] d_summarys)
        {
            int showIndex = 0;
            List<StyleRow> shows = new List<StyleRow>(rows.Length), summarys = new List<StyleRow>(1);
            foreach (var it in rows)
            {
                if (it.Type == RowType.Summary)
                {
                    it.SHOW = true;
                    var item = new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex)));
                    shows.Add(item);
                    summarys.Add(item);
                }
                else
                {
                    it.SHOW = it.ShowExpand && (it.Type == RowType.None || it.Type == RowType.Column) && (it.RECT.Y >= sy && it.RECT.Y <= sy + rect_read.Height || it.RECT.Bottom >= sy && it.RECT.Bottom <= sy + rect_read.Height);
                    if (it.SHOW) shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex))));
                }
                showIndex++;
            }
            d_shows = shows.ToArray();
            d_summarys = summarys.ToArray();
        }
        void DirtyByNone(RowTemplate[] rows, int sy, out StyleRow[] d_shows, out StyleRow[] d_summarys)
        {
            int showIndex = 0;
            rows[0].SHOW = false;
            List<StyleRow> shows = new List<StyleRow>(rows.Length), summarys = new List<StyleRow>(1);
            for (int i = 1; i < rows.Length; i++)
            {
                var it = rows[i];
                if (it.Type == RowType.Summary)
                {
                    it.SHOW = true;
                    var item = new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex)));
                    shows.Add(item);
                    summarys.Add(item);
                }
                else
                {
                    it.SHOW = it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + rect_read.Height + it.RECT.Height;
                    if (it.SHOW) shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex))));
                }
                showIndex++;
            }
            d_shows = shows.ToArray();
            d_summarys = summarys.ToArray();
        }

        #endregion

        #region 表头

        void PaintTableBgHeader(Canvas g, RowTemplate row, float radius, int sx)
        {
            var save = g.Save();
            using (var brush = new SolidBrush(columnback ?? Colour.TagDefaultBg.Get("Table", ColorScheme)))
            {
                if (radius > 0)
                {
                    using (var path = Helper.RoundPath(row.RECT, radius, true, true, false, false))
                    {
                        g.Fill(brush, path);
                        g.SetClip(path, CombineMode.Intersect);
                        g.TranslateTransform(-sx, 0);
                        foreach (var cel in row.cells)
                        {
                            if (cel.COLUMN.ColStyle != null && cel.COLUMN.ColStyle.BackColor.HasValue) g.Fill(cel.COLUMN.ColStyle.BackColor.Value, cel.RECT);
                        }
                    }
                }
                else
                {
                    g.Fill(brush, row.RECT);
                    g.TranslateTransform(-sx, 0);
                    foreach (var cel in row.cells)
                    {
                        if (cel.COLUMN.ColStyle != null && cel.COLUMN.ColStyle.BackColor.HasValue) g.Fill(cel.COLUMN.ColStyle.BackColor.Value, cel.RECT);
                    }
                }
            }
            g.Restore(save);
        }
        void PaintTableHeader(Canvas g, RowTemplate row, SolidBrush fore, Font column_font, float radius)
        {
            if (StackedHeaderRows == null)
            {
                foreach (TCellColumn column in row.cells)
                {
                    if (column.COLUMN.SortOrder)
                    {
                        g.GetImgExtend("CaretUpFilled", column.rect_up, column.COLUMN.SortMode == SortMode.ASC ? Colour.Primary.Get("Table", ColorScheme) : Colour.TextQuaternary.Get("Table", ColorScheme));
                        g.GetImgExtend("CaretDownFilled", column.rect_down, column.COLUMN.SortMode == SortMode.DESC ? Colour.Primary.Get("Table", ColorScheme) : Colour.TextQuaternary.Get("Table", ColorScheme));
                    }
                    if (column.COLUMN.HasFilter)
                    {
                        g.GetImgExtend("FilterFilled", column.rect_filter, column.COLUMN.Filter!.Enabled ? Colour.Primary.Get("Table", ColorScheme) : Colour.TextQuaternary.Get("Table", ColorScheme));
                    }
                    if (column.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle) PaintCheck(g, column, columnCheck);
                    else
                    {
                        if (column.COLUMN.ColStyle != null && column.COLUMN.ColStyle.ForeColor.HasValue)
                        {
                            using (var brush = new SolidBrush(column.COLUMN.ColStyle.ForeColor.Value))
                            {
                                g.DrawText(column.value, column_font, brush, column.RECT_REAL, StringFormat(column.COLUMN, true));
                            }
                        }
                        else g.DrawText(column.value, column_font, fore, column.RECT_REAL, StringFormat(column.COLUMN, true));
                    }
                }
                if (dragHeader == null) return;
                if (dragHeader.enable)
                {
                    foreach (var column in row.cells)
                    {
                        int index = column.COLUMN.INDEX_REAL;
                        if (dragHeader.i == index)
                        {
                            using (var brush = new SolidBrush(Colour.FillSecondary.Get("Table", ColorScheme)))
                            {
                                if (radius > 0)
                                {
                                    if (column.INDEX == 0)
                                    {
                                        using (var path = Helper.RoundPath(column.RECT, radius, true, false, false, false))
                                        {
                                            g.Fill(brush, path);
                                        }
                                    }
                                    else if (column.INDEX == row.cells.Length - 1)
                                    {
                                        using (var path = Helper.RoundPath(column.RECT, radius, false, true, false, false))
                                        {
                                            g.Fill(brush, path);
                                        }
                                    }
                                    else g.Fill(brush, column.RECT);
                                }
                                else g.Fill(brush, column.RECT);
                            }
                        }
                        if (dragHeader.im == index)
                        {
                            using (var brush_split = new SolidBrush(Colour.BorderColor.Get("Table", ColorScheme)))
                            {
                                int sp = (int)(2 * Config.Dpi);
                                if (dragHeader.last) g.Fill(brush_split, new Rectangle(column.RECT.Right - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                                else g.Fill(brush_split, new Rectangle(column.RECT.X - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                            }
                        }
                    }
                }
            }
            else
            {
                int len = StackedHeaderRows.Length + 1;
                List<int> handkey = new List<int>(len), dskey = new List<int>(len);
                var state = g.Save();
                int rY = row.RECT.Y, rHeight = row.Height / len;
                int i = 0, gap = (int)(_gap.Width * Config.Dpi), gap2 = gap * 2;
                foreach (var it in StackedHeaderRows)
                {
                    foreach (var item in it.StackedColumns) PaintTableHeaderStacked(g, row, fore, column_font, ref handkey, ref dskey, rY, rHeight, item);
                    rY += rHeight;
                }
                int sy = row.RECT.Y + (rHeight / 2 * (len - 1));
                foreach (TCellColumn column in row.cells)
                {
                    if (dskey.Contains(i))
                    {
                        dividerHs[i][1] = rY + gap;
                        dividerHs[i][2] = rHeight - gap2;
                    }
                    if (handkey.Contains(column.INDEX))
                    {
                        column.offsety = sy;
                        g.TranslateTransform(0, sy);
                    }
                    if (column.COLUMN.SortOrder)
                    {
                        g.GetImgExtend("CaretUpFilled", column.rect_up, column.COLUMN.SortMode == SortMode.ASC ? Colour.Primary.Get("Table", ColorScheme) : Colour.TextQuaternary.Get("Table", ColorScheme));
                        g.GetImgExtend("CaretDownFilled", column.rect_down, column.COLUMN.SortMode == SortMode.DESC ? Colour.Primary.Get("Table", ColorScheme) : Colour.TextQuaternary.Get("Table", ColorScheme));
                    }
                    if (column.COLUMN.HasFilter)
                    {
                        g.GetImgExtend("FilterFilled", column.rect_filter, column.COLUMN.Filter!.Enabled ? Colour.Primary.Get("Table", ColorScheme) : Colour.TextQuaternary.Get("Table", ColorScheme));
                    }

                    if (column.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle) PaintCheck(g, column, columnCheck);
                    else
                    {
                        if (column.COLUMN.ColStyle != null && column.COLUMN.ColStyle.ForeColor.HasValue)
                        {
                            using (var brush = new SolidBrush(column.COLUMN.ColStyle.ForeColor.Value))
                            {
                                g.DrawText(column.value, column_font, brush, column.RECT_REAL, StringFormat(column.COLUMN, true));
                            }
                        }
                        else g.DrawText(column.value, column_font, fore, column.RECT_REAL, StringFormat(column.COLUMN, true));
                    }
                    g.Restore(state);
                    state = g.Save();
                    i++;
                }
                g.Restore(state);
                if (dragHeader == null) return;
                if (dragHeader.enable)
                {
                    foreach (var column in row.cells)
                    {
                        int index = column.COLUMN.INDEX_REAL;
                        if (dragHeader.i == index)
                        {
                            using (var brush = new SolidBrush(Colour.FillSecondary.Get("Table", ColorScheme)))
                            {
                                if (radius > 0)
                                {
                                    if (column.INDEX == 0)
                                    {
                                        using (var path = Helper.RoundPath(column.RECT, radius, true, false, false, false))
                                        {
                                            g.Fill(brush, path);
                                        }
                                    }
                                    else if (column.INDEX == row.cells.Length - 1)
                                    {
                                        using (var path = Helper.RoundPath(column.RECT, radius, false, true, false, false))
                                        {
                                            g.Fill(brush, path);
                                        }
                                    }
                                    else g.Fill(brush, column.RECT);
                                }
                                else g.Fill(brush, column.RECT);
                            }
                        }
                        if (dragHeader.im == index)
                        {
                            using (var brush_split = new SolidBrush(Colour.BorderColor.Get("Table", ColorScheme)))
                            {
                                int sp = (int)(2 * Config.Dpi);
                                if (dragHeader.last) g.Fill(brush_split, new Rectangle(column.RECT.Right - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                                else g.Fill(brush_split, new Rectangle(column.RECT.X - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                            }
                        }
                    }
                }
            }
        }
        void PaintTableHeaderStacked(Canvas g, RowTemplate row, SolidBrush fore, Font column_font, ref List<int> handkey, ref List<int> dskey, int rY, int rHeight, StackedColumn item)
        {
            if (item.ChildColumns.Length > 1)
            {
                CELL? first = null, last = null;
                int i = 0;
                foreach (var column in row.cells)
                {
                    if (column.COLUMN.Key == item.ChildColumns[0])
                    {
                        first = column;
                        dskey.Add(i);
                        handkey.Add(first.INDEX);
                        if (last == null) continue;
                        PaintTableHeaderStacked(g, fore, column_font, rY, rHeight, item, first, last);
                    }
                    else if (column.COLUMN.Key == item.ChildColumns[item.ChildColumns.Length - 1])
                    {
                        last = column;
                        if (first == null) return;
                        dskey.Add(i);
                        handkey.Add(last.INDEX);
                        PaintTableHeaderStacked(g, fore, column_font, rY, rHeight, item, first, last);
                        return;
                    }
                    else if (first != null)
                    {
                        dskey.Add(i);
                        handkey.Add(column.INDEX);
                    }
                    i++;
                }
            }
            else
            {
                string key = item.ChildColumns[0];
                int i = 0;
                foreach (var column in row.cells)
                {
                    if (column.COLUMN.Key == key)
                    {
                        handkey.Add(column.INDEX);
                        PaintTableHeaderStacked(g, fore, column_font, rY, rHeight, item, column);
                        return;
                    }
                    i++;
                }
            }
        }

        void PaintTableHeaderStacked(Canvas g, SolidBrush fore, Font column_font, int rY, int rHeight, StackedColumn item, CELL column)
        {
            var state = g.Save();
            var rect = new Rectangle(column.RECT_REAL.X, rY, column.RECT_REAL.Width, rHeight);
            if (item.ForeColor.HasValue) g.DrawText(item.HeaderText, column_font, item.ForeColor.Value, rect, StringFormat(column.COLUMN, true));
            else g.DrawText(item.HeaderText, column_font, fore, rect, StringFormat(column.COLUMN, true));
            g.Restore(state);
        }

        void PaintTableHeaderStacked(Canvas g, SolidBrush fore, Font column_font, int rY, int rHeight, StackedColumn item, CELL first, CELL last)
        {
            var state = g.Save();
            var rect = new Rectangle(first.RECT.X, rY, last.RECT.Right - first.RECT.X, rHeight);
            if (item.ForeColor.HasValue) g.DrawText(item.HeaderText, column_font, item.ForeColor.Value, rect, StringFormat(ColumnAlign.Center));
            else g.DrawText(item.HeaderText, column_font, fore, rect, StringFormat(ColumnAlign.Center));
            g.Restore(state);
        }

        #endregion

        #region 表体

        /// <summary>
        /// 渲染背景行（前置）
        /// </summary>
        void PaintBgRowFront(Canvas g, StyleRow row)
        {
            if (row.style != null && row.style.BackColor.HasValue) g.Fill(row.style.BackColor.Value, row.row.RECT);
            if (selectedIndex.Contains(row.row.INDEX) || row.row.Select)
            {
                g.Fill(rowSelectedBg ?? Colour.PrimaryBg.Get("Table", ColorScheme), row.row.RECT);
                if (selectedIndex.Contains(row.row.INDEX) && row.row.Select) g.Fill(Color.FromArgb(40, Colour.PrimaryActive.Get("Table", ColorScheme)), row.row.RECT);
            }
        }

        /// <summary>
        /// 渲染背景行
        /// </summary>
        void PaintBg(Canvas g, RowTemplate row)
        {
            RowPaintBegin?.Invoke(this, new TablePaintRowEventArgs(g, row.RECT, row.RECORD, row.INDEX));
            if (dragBody != null)
            {
                if (dragBody.i == row.INDEX) g.Fill(Colour.FillSecondary.Get("Table", ColorScheme), row.RECT);
                else if (dragBody.im == row.INDEX)
                {
                    using (var brush_split = new SolidBrush(Colour.BorderColor.Get("Table", ColorScheme)))
                    {
                        int sp = (int)(2 * Config.Dpi);
                        if (dragBody.last) g.Fill(brush_split, new Rectangle(row.RECT.X, row.RECT.Bottom - sp, row.RECT.Width, sp * 2));
                        else g.Fill(brush_split, new Rectangle(row.RECT.X, row.RECT.Y - sp, row.RECT.Width, sp * 2));
                    }
                }
            }
            else
            {
                if (row.AnimationHover) g.Fill(Helper.ToColorN(row.AnimationHoverValue, Colour.FillSecondary.Get("Table", ColorScheme)), row.RECT);
                else if (row.Hover) g.Fill(rowHoverBg ?? Colour.FillSecondary.Get("Table", ColorScheme), row.RECT);
            }
            RowPaint?.Invoke(this, new TablePaintRowEventArgs(g, row.RECT, row.RECORD, row.INDEX));
        }

        #region 单元格

        /// <summary>
        /// 渲染单元格背景
        /// </summary>
        void PaintItemBgRowStyle(Canvas g, RowTemplate row)
        {
            foreach (var cel in row.cells)
            {
                if (cel.COLUMN.Style != null && cel.COLUMN.Style.BackColor.HasValue) g.Fill(cel.COLUMN.Style.BackColor.Value, cel.RECT);
                PaintItemBg(g, cel);
            }
        }
        /// <summary>
        /// 渲染单元格背景
        /// </summary>
        void PaintItemBgRow(Canvas g, RowTemplate row)
        {
            foreach (var cel in row.cells) PaintItemBg(g, cel);
        }

        /// <summary>
        /// 渲染单元格背景
        /// </summary>
        void PaintItemBg(Canvas g, CELL it)
        {
            if (it is Template obj)
            {
                foreach (var o in obj.Value) o.PaintBack(g);
            }
        }

        /// <summary>
        /// 渲染前景行
        /// </summary>
        void PaintForeItem(Canvas g, StyleRow row, SolidBrush fore, SolidBrush foreEnable)
        {
            if (selectedIndex.Contains(row.row.INDEX) && rowSelectedFore.HasValue)
            {
                using (var brush = new SolidBrush(rowSelectedFore.Value))
                {
                    for (int i = 0; i < row.row.cells.Length; i++) PaintItem(g, row.row.cells[i], row.row.ENABLE, brush);
                }
            }
            else if (row.style != null && row.style.ForeColor.HasValue)
            {
                using (var brush = new SolidBrush(row.style.ForeColor.Value))
                {
                    for (int i = 0; i < row.row.cells.Length; i++) PaintItem(g, row.row.cells[i], row.row.ENABLE, brush);
                }
            }
            else for (int i = 0; i < row.row.cells.Length; i++) PaintItem(g, row.row.cells[i], row.row.ENABLE, row.row.ENABLE ? fore : foreEnable);
        }

        #region 渲染单元格

        void PaintItem(Canvas g, CELL it, bool enable, SolidBrush fore)
        {
            if (it.COLUMN.Style == null || it.COLUMN.Style.ForeColor == null) PaintItem(g, it.INDEX, it, enable, fore);
            else
            {
                using (var brush = new SolidBrush(it.COLUMN.Style.ForeColor.Value))
                {
                    PaintItem(g, it.INDEX, it, enable, brush);
                }
            }
        }

        /// <summary>
        /// 渲染单元格（浮动）
        /// </summary>
        void PaintItemFixed(Canvas g, CELL it, bool enable, SolidBrush fore, CellStyleInfo? style)
        {
            if (selectedIndex.Contains(it.ROW.INDEX) && rowSelectedFore.HasValue)
            {
                using (var brush = new SolidBrush(rowSelectedFore.Value))
                {
                    PaintItem(g, it.INDEX, it, enable, brush);
                }
            }
            else if (style != null && style.ForeColor.HasValue)
            {
                using (var brush = new SolidBrush(style.ForeColor.Value))
                {
                    PaintItem(g, it.INDEX, it, enable, brush);
                }
            }
            else if (it.COLUMN.Style == null || it.COLUMN.Style.ForeColor == null) PaintItem(g, it.INDEX, it, enable, fore);
            else
            {
                using (var brush = new SolidBrush(it.COLUMN.Style.ForeColor.Value))
                {
                    PaintItem(g, it.INDEX, it, enable, brush);
                }
            }
        }

        void PaintItem(Canvas g, int columnIndex, CELL it, bool enable, SolidBrush fore)
        {
            var state = g.Save();
            try
            {
                if (CellPaintBegin == null) PaintItemCore(g, columnIndex, it, enable, Font, fore);
                else
                {
                    var arge = new TablePaintBeginEventArgs(g, it.RECT, it.RECT_REAL, it.ROW.RECORD, it.ROW.INDEX, columnIndex, it.COLUMN);
                    CellPaintBegin(this, arge);
                    if (arge.Handled)
                    {
                        if (it.ROW.CanExpand && it.ROW.KeyTreeINDEX == columnIndex) PaintItemArrow(g, it, enable, fore);
                    }
                    else
                    {
                        if (arge.CellBack != null)
                        {
                            using (arge.CellBack)
                            {
                                g.Fill(arge.CellBack, it.RECT);
                            }
                        }
                        using (arge.CellFont)
                        using (arge.CellFore)
                        {
                            PaintItemCore(g, columnIndex, it, enable, arge.CellFont ?? Font, arge.CellFore ?? fore);
                        }
                    }
                }
                CellPaint?.Invoke(this, new TablePaintEventArgs(g, it.RECT, it.RECT_REAL, it.ROW.RECORD, it.ROW.INDEX, columnIndex, it.COLUMN));
            }
            catch { }
            g.Restore(state);
        }

        void PaintItemCore(Canvas g, int columnIndex, CELL it, bool enable, Font font, SolidBrush fore)
        {
            PaintItemFocus(g, it, enable);
            if (it is TCellCheck check) PaintCheck(g, check, enable);
            else if (it is TCellRadio radio) PaintRadio(g, radio, enable);
            else if (it is TCellSwitch _switch) PaintSwitch(g, _switch, enable);
            else if (it is TCellSelect select) PaintSelect(g, select, enable);
            else if (it is TCellSort sort)
            {
                if (sort.AnimationHover)
                {
                    using (var brush = new SolidBrush(Helper.ToColorN(sort.AnimationHoverValue, Colour.FillTertiary.Get("Table", ColorScheme))))
                    {
                        using (var path_sort = Helper.RoundPath(sort.RECT_REAL, check_radius))
                        {
                            g.Fill(brush, path_sort);
                        }
                    }
                }
                else if (sort.Hover)
                {
                    using (var path_sort = Helper.RoundPath(sort.RECT_REAL, check_radius))
                    {
                        g.Fill(Colour.FillTertiary.Get("Table", ColorScheme), path_sort);
                    }
                }
                SvgExtend.GetImgExtend(g, "HolderOutlined", sort.RECT_ICO, fore.Color);
            }
            else if (it is Template template)
            {
                g.SetClip(it.RECT, CombineMode.Intersect);
                foreach (var item in template.Value) item.Paint(g, font, enable, fore);
            }
            else if (it is TCellText text)
            {
                g.SetClip(it.RECT, CombineMode.Intersect);
                g.DrawText(text.value, font, fore, text.RECT_REAL, StringFormat(text.COLUMN));
            }
            if (dragHeader != null && dragHeader.enable && dragHeader.i == it.COLUMN.INDEX_REAL) g.Fill(Colour.FillSecondary.Get("Table", ColorScheme), it.RECT);
            if (it.ROW.CanExpand && it.ROW.KeyTreeINDEX == columnIndex) PaintItemArrow(g, it, enable, fore);
        }
        void PaintItemFocus(Canvas g, CELL it, bool enable)
        {
            if (focusedCell == null) return;
            if (it == focusedCell)
            {
                var style = CellFocusedStyle ?? Config.DefaultCellFocusedStyle;
                if (style == TableCellFocusedStyle.None) return;
                var state = g.Save();
                g.SetClip(it.RECT);
                switch (style)
                {
                    case TableCellFocusedStyle.Solid:
                        g.Fill(CellFocusedBg ?? Colour.BgBase.Get("Table", ColorScheme), it.RECT);
                        using (var pen = PaintItemFocus(it, false, (int)(Config.Dpi * 2)))
                        {
                            g.Draw(pen, it.RECT);
                        }
                        break;
                    case TableCellFocusedStyle.Dash:
                        using (var pen = PaintItemFocus(it, true, (int)(Config.Dpi)))
                        {
                            g.Draw(pen, it.RECT);
                        }
                        break;
                }
                g.Restore(state);
            }
        }
        Pen PaintItemFocus(CELL it, bool dash, int bor)
        {
            var pen = new Pen(CellFocusedBorder ?? Colour.PrimaryActive.Get("Table", ColorScheme), bor)
            {
                Alignment = PenAlignment.Inset
            };
            if (dash) pen.DashStyle = DashStyle.Dash;
            return pen;
        }
        void PaintItemArrow(Canvas g, CELL it, bool enable, SolidBrush fore)
        {
            switch (TreeArrowStyle)
            {
                case TableTreeStyle.Button:
                    using (var path_check = Helper.RoundPath(it.ROW.RectExpand, check_radius))
                    {
                        g.Fill(Colour.BgBase.Get("Table", ColorScheme), path_check);
                        g.Draw(Colour.BorderColor.Get("Table", ColorScheme), check_border, path_check);
                        PaintArrow(g, it.ROW, fore, it.ROW.Expand ? 90 : 0);
                    }
                    break;
                case TableTreeStyle.Arrow:
                    PaintArrow(g, it.ROW, fore, it.ROW.Expand ? 90 : 0);
                    break;
                case TableTreeStyle.ArrowFill:
                default:
                    PaintArrowFill(g, it.ROW, fore, it.ROW.Expand ? 90 : 0);
                    break;
            }
        }

        #endregion

        void PaintArrow(Canvas g, RowTemplate item, SolidBrush color, int ArrowProg)
        {
            var rect = PaintArrow(g, item, ArrowProg, out var state);
            using (var pen = new Pen(color, check_border * 2))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, rect.TriangleLines(-1, .6F));
            }
            g.Restore(state);
        }
        void PaintArrowFill(Canvas g, RowTemplate item, SolidBrush brush, float ArrowProg)
        {
            g.FillPolygon(brush, PaintArrow(g, item, ArrowProg, out var state).TriangleLines(-1, 0.8F));
            g.Restore(state);
        }
        Rectangle PaintArrow(Canvas g, RowTemplate item, float ArrowProg, out GraphicsState state)
        {
            state = g.Save();
            int size_arrow = item.RectExpand.Width / 2;
            g.TranslateTransform(item.RectExpand.X + size_arrow, item.RectExpand.Y + size_arrow);
            g.RotateTransform(-90F + ArrowProg);
            return new Rectangle(-size_arrow, -size_arrow, item.RectExpand.Width, item.RectExpand.Height);
        }

        #endregion

        #region 浮动列

        void PaintFixedColumnL(Canvas g, Rectangle rect, Rectangle rect_read, RowTemplate[] rows, StyleRow[] shows, SolidBrush fore, SolidBrush foreEnable, SolidBrush forecolumn, Font column_font, Pen pen_cell_split, int sx, int sy, float radius)
        {
            if (fixedColumnL != null && sx > 0)
            {
                showFixedColumnL = true;
                var last = shows[shows.Length - 1].row.cells[fixedColumnL[fixedColumnL.Count - 1]];
                var rect_Fixed = new Rectangle(rect.X, rect_read.Y, last.RECT.Right, rect_read.Height);

                GraphicsPath? clipath = null;

                #region 绘制阴影

                if (_gap.Width > 0)
                {
                    int gap = (int)(_gap.Width * Config.Dpi);
                    var rect_show = new Rectangle(last.RECT.Right - gap, rect_Fixed.Y, gap * 2, rect_Fixed.Height);
                    using (var brush = new LinearGradientBrush(rect_show, Colour.FillSecondary.Get("Table", ColorScheme), Color.Transparent, 0F))
                    {
                        g.Fill(brush, rect_show);
                    }
                }
                if (radius > 0)
                {
                    clipath = Helper.RoundPath(rect_Fixed, radius, true, false, false, !visibleHeader);
                    g.Fill(Colour.BgBase.Get("Table", ColorScheme), clipath);
                    g.SetClip(clipath);
                }
                else
                {
                    g.Fill(Colour.BgBase.Get("Table", ColorScheme), rect_Fixed);
                    g.SetClip(rect_Fixed);
                }

                #endregion

                g.TranslateTransform(0, -sy);
                foreach (var row in shows)
                {
                    if (row.row.IsColumn)
                    {
                        PaintTableBgHeader(g, row.row, radius, 0);
                        PaintTableHeader(g, row.row, forecolumn, column_font, radius);
                    }
                    else
                    {
                        PaintBgRowFront(g, row);
                        PaintItemBgRowStyle(g, row.row);
                        PaintBg(g, row.row);
                    }
                }

                foreach (var row in shows)
                {
                    foreach (int fixedIndex in fixedColumnL)
                    {
                        if (row.row.IsOther) PaintItemFixed(g, row.row.cells[fixedIndex], row.row.ENABLE, row.row.ENABLE ? fore : foreEnable, row.style);
                    }
                }

                if (dividers.Length > 0)
                {
                    foreach (var divider in dividers) g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                }

                g.ResetTransform();
                if (fixedHeader)
                {
                    PaintTableBgHeader(g, rows[0], radius, 0);
                    PaintTableHeader(g, rows[0], forecolumn, column_font, radius);
                    if (sy == 0 && dividers.Length > 0)
                    {
                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        var divider = dividers[0];
                        g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                        g.ResetTransform();
                    }
                }
                else g.TranslateTransform(0, bordered ? 0 : -sy);
                if (dividerHs.Length > 0)
                {
                    foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                }
                g.ResetTransform();
                g.ResetClip();
                clipath?.Dispose();
            }
            else showFixedColumnL = false;
        }
        void PaintFixedColumnR(Canvas g, Rectangle rect, Rectangle rect_read, RowTemplate[] rows, StyleRow[] shows, SolidBrush fore, SolidBrush foreEnable, SolidBrush forecolumn, Font column_font, Pen pen_cell_split, int sx, int sy, float radius)
        {
            if (fixedColumnR != null)
            {
                try
                {
                    var lastrow = shows[shows.Length - 1];
                    int scrollBar = ScrollBar.ShowY ? ScrollBar.SIZE : 0, rectR = rect_read.Right - scrollBar;
                    CELL first = lastrow.row.cells[fixedColumnR[fixedColumnR.Count - 1]], last = lastrow.row.cells[fixedColumnR[0]];
                    if (sx + rectR < last.RECT.Right)
                    {
                        showFixedColumnR = true;
                        sFixedR = last.RECT.Right - rectR;
                        int w = last.RECT.Right - first.RECT.Left + scrollBar;

                        var rect_Fixed = new Rectangle(rect_read.Right - w, rect_read.Y, w, rect_read.Height);

                        GraphicsPath? clipath = null;

                        #region 绘制阴影

                        if (_gap.Width > 0)
                        {
                            int gap = (int)(_gap.Width * Config.Dpi);
                            var rect_show = new Rectangle(rect_Fixed.X - gap, rect_Fixed.Y, gap * 2, rect_Fixed.Height);
                            using (var brush = new LinearGradientBrush(rect_show, Color.Transparent, Colour.FillSecondary.Get("Table", ColorScheme), 0F))
                            {
                                g.Fill(brush, rect_show);
                            }
                        }
                        if (radius > 0)
                        {
                            clipath = Helper.RoundPath(rect_Fixed, radius, false, true, !visibleHeader, false);
                            g.Fill(Colour.BgBase.Get("Table", ColorScheme), clipath);
                            g.SetClip(clipath);
                        }
                        else
                        {
                            g.Fill(Colour.BgBase.Get("Table", ColorScheme), rect_Fixed);
                            g.SetClip(rect_Fixed);
                        }

                        #endregion

                        foreach (var row in shows)
                        {
                            g.ResetTransform();
                            g.TranslateTransform(0, -sy);
                            if (row.row.IsColumn)
                            {
                                PaintTableBgHeader(g, row.row, radius, sFixedR);

                                g.ResetTransform();
                                g.TranslateTransform(-sFixedR, -sy);
                                PaintTableHeader(g, row.row, forecolumn, column_font, radius);
                            }
                            else
                            {
                                PaintBgRowFront(g, row);

                                g.ResetTransform();
                                g.TranslateTransform(-sFixedR, -sy);

                                PaintItemBgRowStyle(g, row.row);

                                g.ResetTransform();
                                g.TranslateTransform(0, -sy);
                                PaintBg(g, row.row);
                            }
                        }
                        g.ResetTransform();
                        g.TranslateTransform(-sFixedR, -sy);
                        foreach (var row in shows)
                        {
                            foreach (int fixedIndex in fixedColumnR)
                            {
                                if (row.row.IsOther)
                                {
                                    PaintItemBg(g, row.row.cells[fixedIndex]);
                                    PaintItemFixed(g, row.row.cells[fixedIndex], row.row.ENABLE, row.row.ENABLE ? fore : foreEnable, row.style);
                                }
                            }
                        }
                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);

                        if (dividers.Length > 0)
                        {
                            foreach (var divider in dividers) g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                        }

                        g.ResetTransform();
                        if (fixedHeader)
                        {
                            PaintTableBgHeader(g, rows[0], radius, sFixedR);
                            g.TranslateTransform(-sFixedR, 0);
                            PaintTableHeader(g, rows[0], forecolumn, column_font, radius);
                            g.ResetTransform();
                            if (sy == 0 && dividers.Length > 0)
                            {
                                g.TranslateTransform(0, -sy);
                                var divider = dividers[0];
                                g.DrawLine(pen_cell_split, divider[1], divider[0], divider[1] + divider[2], divider[0]);
                                g.ResetTransform();
                            }
                            g.TranslateTransform(-sFixedR, 0);
                        }
                        else
                        {
                            g.ResetTransform();
                            g.TranslateTransform(-sFixedR, bordered ? 0 : -sy);
                        }
                        if (dividerHs.Length > 0)
                        {
                            foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                        }
                        g.ResetTransform();
                        g.ResetClip();
                        clipath?.Dispose();
                    }
                    else showFixedColumnR = false;
                }
                catch { }
            }
            else showFixedColumnR = false;
        }

        void PaintFixedSummary(Canvas g, Rectangle rect, Rectangle rect_read, StyleRow[] shows, SolidBrush fore, SolidBrush foreEnable, SolidBrush forecolumn, Font column_font, Pen pen_cell_split, int sx, int sy, float radius)
        {
            if (ScrollBar.ShowY)
            {
                try
                {
                    var lastrow = shows[shows.Length - 1];
                    if (sy + rect_read.Height < lastrow.row.RECT.Bottom)
                    {
                        int scrollBar = ScrollBar.ShowX ? ScrollBar.SIZE : 0, h = lastrow.row.RECT.Bottom - shows[0].row.RECT.Y, sFixedB = lastrow.row.RECT.Bottom - rect_read.Bottom + scrollBar;

                        var rect_Fixed = new Rectangle(rect_read.X, rect_read.Bottom - h - scrollBar, rect_read.Width, h + scrollBar);

                        GraphicsPath? clipath = null;

                        #region 绘制阴影

                        if (_gap.Width > 0)
                        {
                            int gap = (int)(_gap.Width * Config.Dpi);
                            var rect_show = new Rectangle(rect_Fixed.X, rect_Fixed.Y - gap, rect_Fixed.Width, gap * 2);
                            using (var brush = new LinearGradientBrush(rect_show, Color.Transparent, Colour.FillSecondary.Get("Table", ColorScheme), 90F))
                            {
                                g.Fill(brush, rect_show);
                            }
                        }
                        if (radius > 0 && !visibleHeader)
                        {
                            clipath = Helper.RoundPath(rect_Fixed, radius, false, false, true, true);
                            g.Fill(Colour.BgBase.Get("Table", ColorScheme), clipath);
                            g.SetClip(clipath);
                        }
                        else
                        {
                            g.Fill(Colour.BgBase.Get("Table", ColorScheme), rect_Fixed);
                            g.SetClip(rect_Fixed);
                        }

                        #endregion

                        g.TranslateTransform(0, -sFixedB);
                        foreach (var row in shows) PaintBgRowFront(g, row);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sFixedB);
                        foreach (var row in shows) PaintItemBgRowStyle(g, row.row);

                        g.ResetTransform();
                        g.TranslateTransform(0, -sFixedB);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sFixedB);
                        foreach (var row in shows) PaintForeItem(g, row, fore, foreEnable);

                        g.ResetTransform();

                        g.TranslateTransform(-sx, 0);

                        if (dividerHs.Length > 0)
                        {
                            foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                        }

                        g.ResetTransform();
                        if (fixedColumnL != null || fixedColumnR != null)
                        {
                            PaintFixedSummaryL(g, rect, rect_read, shows, fore, foreEnable, forecolumn, column_font, pen_cell_split, sx, sFixedB, radius);
                            PaintFixedSummaryR(g, rect, rect_read, shows, fore, foreEnable, forecolumn, column_font, pen_cell_split, sx, sFixedB, radius);
                        }
                        g.ResetClip();
                        clipath?.Dispose();
                    }
                }
                catch { }
            }
        }

        void PaintFixedSummaryL(Canvas g, Rectangle rect, Rectangle rect_read, StyleRow[] shows, SolidBrush fore, SolidBrush foreEnable, SolidBrush forecolumn, Font column_font, Pen pen_cell_split, int sx, int sy, float radius)
        {
            if (fixedColumnL != null && sx > 0)
            {
                var save = g.Save();
                try
                {
                    var last = shows[shows.Length - 1].row.cells[fixedColumnL[fixedColumnL.Count - 1]];
                    var rect_Fixed = new Rectangle(rect.X, rect.Y, last.RECT.Right, rect.Height);

                    GraphicsPath? clipath = null;

                    #region 绘制阴影

                    if (_gap.Width > 0)
                    {
                        int gap = (int)(_gap.Width * Config.Dpi);
                        var rect_show = new Rectangle(rect.X + last.RECT.Right - gap, rect_Fixed.Y, gap * 2, rect_Fixed.Height);
                        using (var brush = new LinearGradientBrush(rect_show, Colour.FillSecondary.Get("Table", ColorScheme), Color.Transparent, 0F))
                        {
                            g.Fill(brush, rect_show);
                        }
                    }
                    if (radius > 0)
                    {
                        clipath = Helper.RoundPath(rect_Fixed, radius, true, false, false, !visibleHeader);
                        g.SetClip(clipath, CombineMode.Intersect);
                        g.Fill(Colour.BgBase.Get("Table", ColorScheme), clipath);
                    }
                    else
                    {
                        g.SetClip(rect_Fixed, CombineMode.Intersect);
                        g.Fill(Colour.BgBase.Get("Table", ColorScheme), rect_Fixed);
                    }

                    #endregion

                    g.TranslateTransform(0, -sy);
                    foreach (var row in shows)
                    {
                        PaintBgRowFront(g, row);
                        PaintItemBgRow(g, row.row);
                        foreach (int fixedIndex in fixedColumnL) PaintItemFixed(g, row.row.cells[fixedIndex], row.row.ENABLE, row.row.ENABLE ? fore : foreEnable, row.style);
                    }
                    g.ResetTransform();
                    if (!fixedHeader) g.TranslateTransform(0, bordered ? 0 : -sy);
                    if (dividerHs.Length > 0)
                    {
                        foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                    }

                    clipath?.Dispose();
                }
                catch { }
                g.Restore(save);
            }
        }
        void PaintFixedSummaryR(Canvas g, Rectangle rect, Rectangle rect_read, StyleRow[] shows, SolidBrush fore, SolidBrush foreEnable, SolidBrush forecolumn, Font column_font, Pen pen_cell_split, int sx, int sy, float radius)
        {
            if (fixedColumnR != null && ScrollBar.ShowX)
            {
                var save = g.Save();
                try
                {
                    var lastrow = shows[shows.Length - 1];
                    int scrollBar = ScrollBar.ShowY ? ScrollBar.SIZE : 0, rectR = rect_read.Right - scrollBar;
                    CELL first = lastrow.row.cells[fixedColumnR[fixedColumnR.Count - 1]], last = lastrow.row.cells[fixedColumnR[0]];
                    if (sx + rectR < last.RECT.Right)
                    {
                        int sFixedR = last.RECT.Right - rectR;
                        int w = last.RECT.Right - first.RECT.Left + scrollBar;

                        var rect_Fixed = new Rectangle(rect_read.Right - w, rect_read.Y, w, rect_read.Height);

                        GraphicsPath? clipath = null;

                        #region 绘制阴影

                        if (_gap.Width > 0)
                        {
                            int gap = (int)(_gap.Width * Config.Dpi);
                            var rect_show = new Rectangle(rect_Fixed.X - gap, rect_Fixed.Y, gap * 2, rect_Fixed.Height);
                            using (var brush = new LinearGradientBrush(rect_show, Color.Transparent, Colour.FillSecondary.Get("Table", ColorScheme), 0F))
                            {
                                g.Fill(brush, rect_show);
                            }
                        }
                        if (radius > 0)
                        {
                            clipath = Helper.RoundPath(rect_Fixed, radius, false, true, !visibleHeader, false);
                            g.SetClip(clipath, CombineMode.Intersect);
                            g.Fill(Colour.BgBase.Get("Table", ColorScheme), clipath);
                        }
                        else
                        {
                            g.SetClip(rect_Fixed, CombineMode.Intersect);
                            g.Fill(Colour.BgBase.Get("Table", ColorScheme), rect_Fixed);
                        }

                        #endregion

                        g.TranslateTransform(0, -sy);
                        foreach (var row in shows) PaintBgRowFront(g, row);
                        g.ResetTransform();
                        g.TranslateTransform(-sFixedR, -sy);
                        foreach (var row in shows)
                        {
                            foreach (int fixedIndex in fixedColumnR)
                            {
                                PaintItemBg(g, row.row.cells[fixedIndex]);
                                PaintItemFixed(g, row.row.cells[fixedIndex], row.row.ENABLE, row.row.ENABLE ? fore : foreEnable, row.style);
                            }
                        }
                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        g.ResetTransform();

                        g.TranslateTransform(-sFixedR, 0);
                        if (dividerHs.Length > 0)
                        {
                            foreach (var divider in dividerHs) g.DrawLine(pen_cell_split, divider[0], divider[1], divider[0], divider[1] + divider[2]);
                        }

                        clipath?.Dispose();
                    }
                }
                catch { }

                g.Restore(save);
            }
        }

        #endregion

        #endregion

        #region 合并

        void PaintMergeCells(Canvas g, RowTemplate[] rows, int sx, int sy, Color split_color, SolidBrush fore, SolidBrush foreEnable)
        {
            if (CellRanges == null || CellRanges.Length == 0) return;
            var state = g.Save();
            if (visibleHeader && fixedHeader) g.SetClip(new Rectangle(rect_read.X, rect_read.Y + rows[0].Height, rect_read.Width, rect_read.Height));
            g.TranslateTransform(-sx, -sy);
            int sps = (int)(BorderCellWidth * Config.Dpi), sps2 = sps * 2;
            using (var bg = new SolidBrush(Colour.BgBase.Get("Table", ColorScheme)))
            {
                foreach (var it in CellRanges) PaintMergeCells(g, rows, bg, split_color, fore, foreEnable, sps, sps2, it);
            }
            g.Restore(state);
        }

        void PaintMergeCells(Canvas g, RowTemplate[] rows, SolidBrush bg, Color split_color, SolidBrush fore, SolidBrush foreEnable, int sps, int sps2, CellRange range)
        {
            if (range.FirstRow == range.LastRow)
            {
                foreach (var item in rows)
                {
                    if (item.INDEX_REAL == range.FirstRow)
                    {
                        PaintMergeCells(g, bg, split_color, fore, foreEnable, sps, sps2, item.cells[range.FirstColumn], item.cells[range.LastColumn]);
                        return;
                    }
                }
            }
            else
            {
                CELL? first = null, last = null;
                foreach (var item in rows)
                {
                    if (item.INDEX_REAL == range.FirstRow)
                    {
                        first = item.cells[range.FirstColumn];
                        if (last == null) continue;
                        PaintMergeCells(g, bg, split_color, fore, foreEnable, sps, sps2, first, last);
                        return;
                    }
                    else if (item.INDEX_REAL == range.LastRow)
                    {
                        last = item.cells[range.LastColumn];
                        if (first == null) continue;
                        PaintMergeCells(g, bg, split_color, fore, foreEnable, sps, sps2, first, last);
                        return;
                    }
                }
            }
        }

        void PaintMergeCells(Canvas g, SolidBrush bg, Color split_color, SolidBrush fore, SolidBrush foreEnable, int sps, int sps2, CELL first, CELL last)
        {
            var state = g.Save();
            var rect = RectMergeCells(first, last, out bool fz);
            g.Fill(bg, rect);
            if (first.ROW.AnimationHover) g.Fill(Helper.ToColorN(first.ROW.AnimationHoverValue, Colour.FillSecondary.Get("Table", ColorScheme)), rect);
            else if (first.ROW.Hover) g.Fill(rowHoverBg ?? Colour.FillSecondary.Get("Table", ColorScheme), rect);

            if (BorderCellWidth > 1)
            {
                float sp2 = sps / 2F, x = rect.X - sp2, y = rect.Y - sp2, w = rect.Width + sps, h = rect.Height + sps;
                g.Fill(split_color, new RectangleF(x, y, w, sps));
                g.Fill(split_color, new RectangleF(x, y, sps, h));
                g.Fill(split_color, new RectangleF(x, rect.Bottom - sp2, w, sps));
                g.Fill(split_color, new RectangleF(rect.Right - sp2, y, sps, h));
            }
            else
            {
                g.Fill(split_color, new RectangleF(rect.X, rect.Y, rect.Width, sps));
                g.Fill(split_color, new RectangleF(rect.X, rect.Y, sps, rect.Height));
                g.Fill(split_color, new RectangleF(rect.X, rect.Bottom, rect.Width, sps));
                g.Fill(split_color, new RectangleF(rect.Right, rect.Y, sps, rect.Height));
            }

            #region 绘制内容

            if (fz) g.TranslateTransform((rect.Width - first.RECT.Width) / 2, -(rect.Height - first.RECT.Height) / 2);
            else g.TranslateTransform((rect.Width - first.RECT.Width) / 2, (rect.Height - first.RECT.Height) / 2);

            PaintItem(g, first, first.ROW.ENABLE, first.ROW.ENABLE ? fore : foreEnable);

            #endregion

            g.Restore(state);
        }

        Rectangle RectMergeCells(CELL first, CELL last, out bool fz)
        {
            fz = false;
            if (last.RECT.X >= first.RECT.X && last.RECT.Y >= first.RECT.Y) return new Rectangle(first.RECT.X, first.RECT.Y, last.RECT.Right - first.RECT.X, last.RECT.Bottom - first.RECT.Y);
            else if (last.RECT.X >= first.RECT.X)
            {
                fz = true;
                return new Rectangle(first.RECT.X, last.RECT.Y, last.RECT.Right - first.RECT.X, first.RECT.Bottom - last.RECT.Y);
            }
            else return new Rectangle(last.RECT.X, last.RECT.Y, first.RECT.Right - last.RECT.X, first.RECT.Bottom - last.RECT.Y);
        }

        #endregion

        #region 渲染复选/选择框

        #region 复选框

        void PaintCheck(Canvas g, TCellColumn check, ColumnCheck columnCheck)
        {
            using (var path_check = Helper.RoundPath(check.RECT_REAL, check_radius))
            {
                if (columnCheck.AnimationCheck)
                {
                    g.Fill(Colour.BgBase.Get("Checkbox", ColorScheme), path_check);
                    var alpha = 255 * columnCheck.AnimationCheckValue;
                    if (columnCheck.CheckState == CheckState.Indeterminate || (columnCheck.checkStateOld == CheckState.Indeterminate && !columnCheck.Checked))
                    {
                        g.Draw(Colour.BorderColor.Get("Checkbox", ColorScheme), check_border, path_check);
                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Checkbox", ColorScheme)), PaintBlock(check.RECT_REAL));
                    }
                    else
                    {
                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Checkbox", ColorScheme)), path_check);
                        using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get("Checkbox", ColorScheme)), check_border * 2))
                        {
                            g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                        }
                        if (columnCheck.Checked)
                        {
                            float max = check.RECT_REAL.Height + check.RECT_REAL.Height * columnCheck.AnimationCheckValue, alpha2 = 100 * (1F - columnCheck.AnimationCheckValue);
                            using (var brush = new SolidBrush(Helper.ToColor(alpha2, Colour.Primary.Get("Checkbox", ColorScheme))))
                            {
                                g.FillEllipse(brush, new RectangleF(check.RECT_REAL.X + (check.RECT_REAL.Width - max) / 2F, check.RECT_REAL.Y + (check.RECT_REAL.Height - max) / 2F, max, max));
                            }
                        }
                        g.Draw(Colour.Primary.Get("Checkbox", ColorScheme), check_border, path_check);
                    }
                }
                else if (columnCheck.CheckState == CheckState.Indeterminate)
                {
                    g.Fill(Colour.BgBase.Get("Checkbox", ColorScheme), path_check);
                    g.Draw(Colour.BorderColor.Get("Checkbox", ColorScheme), check_border, path_check);
                    g.Fill(Colour.Primary.Get("Checkbox", ColorScheme), PaintBlock(check.RECT_REAL));
                }
                else if (columnCheck.Checked)
                {
                    g.Fill(Colour.Primary.Get("Checkbox", ColorScheme), path_check);
                    using (var brush = new Pen(Colour.BgBase.Get("Checkbox", ColorScheme), check_border * 2))
                    {
                        g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                    }
                }
                else
                {
                    g.Fill(Colour.BgBase.Get("Checkbox", ColorScheme), path_check);
                    g.Draw(Colour.BorderColor.Get("Checkbox", ColorScheme), check_border, path_check);
                }
            }
        }
        void PaintCheck(Canvas g, TCellCheck check, bool enable)
        {
            using (var path = Helper.RoundPath(check.RECT_REAL, check_radius))
            {
                if (enable)
                {
                    if (check.AnimationCheck)
                    {
                        g.Fill(Colour.BgBase.Get("Checkbox", ColorScheme), path);

                        var alpha = 255 * check.AnimationCheckValue;

                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Checkbox", ColorScheme)), path);
                        using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get("Checkbox", ColorScheme)), check_border * 2))
                        {
                            g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                        }

                        if (check.Checked)
                        {
                            float max = check.RECT_REAL.Height + check.RECT_REAL.Height * check.AnimationCheckValue, alpha2 = 100 * (1F - check.AnimationCheckValue);
                            using (var brush = new SolidBrush(Helper.ToColor(alpha2, Colour.Primary.Get("Checkbox", ColorScheme))))
                            {
                                g.FillEllipse(brush, new RectangleF(check.RECT_REAL.X + (check.RECT_REAL.Width - max) / 2F, check.RECT_REAL.Y + (check.RECT_REAL.Height - max) / 2F, max, max));
                            }
                        }
                        g.Draw(Colour.Primary.Get("Checkbox", ColorScheme), check_border, path);
                    }
                    else if (check.Checked)
                    {
                        g.Fill(Colour.Primary.Get("Checkbox", ColorScheme), path);
                        using (var brush = new Pen(Colour.BgBase.Get("Checkbox", ColorScheme), check_border * 2))
                        {
                            g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                        }
                    }
                    else
                    {
                        g.Fill(Colour.BgBase.Get("Checkbox", ColorScheme), path);
                        g.Draw(Colour.BorderColor.Get("Checkbox", ColorScheme), check_border, path);
                    }
                }
                else
                {
                    g.Fill(Colour.FillQuaternary.Get("Checkbox", ColorScheme), path);
                    if (check.Checked) g.DrawLines(Colour.TextQuaternary.Get("Checkbox", ColorScheme), check_border * 2, PaintArrow(check.RECT_REAL));
                    g.Draw(Colour.BorderColorDisable.Get("Checkbox", ColorScheme), check_border, path);
                }
            }
        }

        #endregion

        #region 单选框

        void PaintRadio(Canvas g, TCellRadio radio, bool enable)
        {
            var dot_size = radio.RECT_REAL.Height;
            if (enable)
            {
                g.FillEllipse(Colour.BgBase.Get("Radio", ColorScheme), radio.RECT_REAL);
                if (radio.AnimationCheck)
                {
                    float dot = dot_size * 0.3F;
                    using (var path = new GraphicsPath())
                    {
                        float dot_ant = dot_size - dot * radio.AnimationCheckValue, dot_ant2 = dot_ant / 2F, alpha = 255 * radio.AnimationCheckValue;
                        path.AddEllipse(radio.RECT_REAL);
                        path.AddEllipse(new RectangleF(radio.RECT_REAL.X + dot_ant2, radio.RECT_REAL.Y + dot_ant2, radio.RECT_REAL.Width - dot_ant, radio.RECT_REAL.Height - dot_ant));
                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Radio", ColorScheme)), path);
                    }
                    if (radio.Checked)
                    {
                        float max = radio.RECT_REAL.Height + radio.RECT_REAL.Height * radio.AnimationCheckValue, alpha2 = 100 * (1F - radio.AnimationCheckValue);
                        g.FillEllipse(Helper.ToColor(alpha2, Colour.Primary.Get("Radio", ColorScheme)), new RectangleF(radio.RECT_REAL.X + (radio.RECT_REAL.Width - max) / 2F, radio.RECT_REAL.Y + (radio.RECT_REAL.Height - max) / 2F, max, max));
                    }
                    g.DrawEllipse(Colour.Primary.Get("Radio", ColorScheme), check_border, radio.RECT_REAL);
                }
                else if (radio.Checked)
                {
                    float dot = dot_size * 0.3F, dot2 = dot / 2F;
                    g.DrawEllipse(Color.FromArgb(250, Colour.Primary.Get("Radio", ColorScheme)), dot, new RectangleF(radio.RECT_REAL.X + dot2, radio.RECT_REAL.Y + dot2, radio.RECT_REAL.Width - dot, radio.RECT_REAL.Height - dot));
                    g.DrawEllipse(Colour.Primary.Get("Radio", ColorScheme), check_border, radio.RECT_REAL);
                }
                else g.DrawEllipse(Colour.BorderColor.Get("Radio", ColorScheme), check_border, radio.RECT_REAL);
            }
            else
            {
                g.FillEllipse(Colour.FillQuaternary.Get("Radio", ColorScheme), radio.RECT_REAL);
                if (radio.Checked)
                {
                    float dot = dot_size / 2F, dot2 = dot / 2F;
                    g.FillEllipse(Colour.TextQuaternary.Get("Radio", ColorScheme), new RectangleF(radio.RECT_REAL.X + dot2, radio.RECT_REAL.Y + dot2, radio.RECT_REAL.Width - dot, radio.RECT_REAL.Height - dot));
                }
                g.DrawEllipse(Colour.BorderColorDisable.Get("Radio", ColorScheme), check_border, radio.RECT_REAL);
            }
        }

        #endregion

        #region 开关

        void PaintSwitch(Canvas g, TCellSwitch _switch, bool enable)
        {
            var color = Colour.Primary.Get("Switch", ColorScheme);
            using (var path = _switch.RECT_REAL.RoundPath(_switch.RECT_REAL.Height))
            {
                using (var brush = new SolidBrush(Colour.TextQuaternary.Get("Switch", ColorScheme)))
                {
                    g.Fill(brush, path);
                    if (_switch.AnimationHover) g.Fill(Helper.ToColorN(_switch.AnimationHoverValue, brush.Color), path);
                    else if (_switch.ExtraMouseHover) g.Fill(brush, path);
                }
                float gap = (int)(2 * Config.Dpi), gap2 = gap * 2F;
                if (_switch.AnimationCheck)
                {
                    var alpha = 255 * _switch.AnimationCheckValue;
                    g.Fill(Helper.ToColor(alpha, color), path);
                    var dot_rect = new RectangleF(_switch.RECT_REAL.X + gap + (_switch.RECT_REAL.Width - _switch.RECT_REAL.Height) * _switch.AnimationCheckValue, _switch.RECT_REAL.Y + gap, _switch.RECT_REAL.Height - gap2, _switch.RECT_REAL.Height - gap2);
                    g.FillEllipse(enable ? Colour.BgBase.Get("Switch", ColorScheme) : Color.FromArgb(200, Colour.BgBase.Get("Switch", ColorScheme)), dot_rect);
                }
                else if (_switch.Checked)
                {
                    var colorhover = Colour.PrimaryHover.Get("Switch", ColorScheme);
                    g.Fill(color, path);
                    if (_switch.AnimationHover) g.Fill(Helper.ToColorN(_switch.AnimationHoverValue, colorhover), path);
                    else if (_switch.ExtraMouseHover) g.Fill(colorhover, path);
                    var dot_rect = new RectangleF(_switch.RECT_REAL.X + gap + _switch.RECT_REAL.Width - _switch.RECT_REAL.Height, _switch.RECT_REAL.Y + gap, _switch.RECT_REAL.Height - gap2, _switch.RECT_REAL.Height - gap2);
                    g.FillEllipse(enable ? Colour.BgBase.Get("Switch", ColorScheme) : Color.FromArgb(200, Colour.BgBase.Get("Switch", ColorScheme)), dot_rect);
                    if (_switch.Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = _switch.RECT_REAL.Height * .1F;
                        using (var brush = new Pen(color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, dot_rect2, _switch.LineAngle, _switch.LineWidth * 3.6F);
                        }
                    }
                }
                else
                {
                    var dot_rect = new RectangleF(_switch.RECT_REAL.X + gap, _switch.RECT_REAL.Y + gap, _switch.RECT_REAL.Height - gap2, _switch.RECT_REAL.Height - gap2);
                    g.FillEllipse(enable ? Colour.BgBase.Get("Switch", ColorScheme) : Color.FromArgb(200, Colour.BgBase.Get("Switch", ColorScheme)), dot_rect);
                    if (_switch.Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = _switch.RECT_REAL.Height * .1F;
                        using (var brush = new Pen(color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, dot_rect2, _switch.LineAngle, _switch.LineWidth * 3.6F);
                        }
                    }
                }
            }
        }

        #endregion

        #region 选择框

        void PaintSelect(Canvas g, TCellSelect select, bool enable)
        {
            if (select.value == null) return;
            var color = select.value.TagFore ?? fore ?? Colour.Text.Get("Select", ColorScheme);
            if (select.value.IconSvg != null) g.GetImgExtend(select.value.IconSvg, select.rect_icon, color);
            else if (select.value.Icon != null) g.Image(select.value.Icon, select.rect_icon);

            if (select.COLUMN.CellType != SelectCellType.Icon && select.rect_text != Rectangle.Empty) g.DrawText(select.value.Text, Font, color, select.rect_text);
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

        void PaintEmpty(Canvas g, Rectangle rect, int offset) => g.PaintEmpty(rect, Font, fore ?? Colour.Text.Get("Table", ColorScheme), EmptyText, EmptyImage, offset, StringFormat(ColumnAlign.Center));

        public static StringFormat StringFormat(Column column, bool isColumn) => isColumn ? StringFormat(column.ColAlign ?? column.Align, LineBreak: column.ColBreak) : StringFormat(column);

        public static StringFormat StringFormat(Column column) => StringFormat(column.Align, column.Ellipsis, column.LineBreak);

        static Dictionary<string, StringFormat> sf_cache = new Dictionary<string, StringFormat>(12) { { "Center00", Helper.SF_NoWrap() } };
        public static StringFormat StringFormat(ColumnAlign Align, bool Ellipsis = false, bool LineBreak = false)
        {
            var id = Align.ToString() + (Ellipsis ? 1 : 0) + (LineBreak ? 1 : 0);
            if (sf_cache.TryGetValue(id, out var value)) return value;
            else
            {
                if (Ellipsis && LineBreak)
                {
                    switch (Align)
                    {
                        case ColumnAlign.Center:
                            StringFormat resultCenter = Helper.SF_Ellipsis();
                            sf_cache.Add(id, resultCenter);
                            return resultCenter;
                        case ColumnAlign.Left:
                            StringFormat resultLeft = Helper.SF_Ellipsis(lr: StringAlignment.Near);
                            sf_cache.Add(id, resultLeft);
                            return resultLeft;
                        case ColumnAlign.Right:
                        default:
                            StringFormat resultRight = Helper.SF_Ellipsis(lr: StringAlignment.Far);
                            sf_cache.Add(id, resultRight);
                            return resultRight;
                    }
                }
                else if (Ellipsis)
                {
                    switch (Align)
                    {
                        case ColumnAlign.Center:
                            StringFormat resultCenter = Helper.SF_ALL();
                            sf_cache.Add(id, resultCenter);
                            return resultCenter;
                        case ColumnAlign.Left:
                            StringFormat resultLeft = Helper.SF_ALL(lr: StringAlignment.Near);
                            sf_cache.Add(id, resultLeft);
                            return resultLeft;
                        case ColumnAlign.Right:
                        default:
                            StringFormat resultRight = Helper.SF_ALL(lr: StringAlignment.Far);
                            sf_cache.Add(id, resultRight);
                            return resultRight;
                    }
                }
                else if (LineBreak)
                {
                    switch (Align)
                    {
                        case ColumnAlign.Center:
                            StringFormat resultCenter = Helper.SF();
                            sf_cache.Add(id, resultCenter);
                            return resultCenter;
                        case ColumnAlign.Left:
                            StringFormat resultLeft = Helper.SF(lr: StringAlignment.Near);
                            sf_cache.Add(id, resultLeft);
                            return resultLeft;
                        case ColumnAlign.Right:
                        default:
                            StringFormat resultRight = Helper.SF(lr: StringAlignment.Far);
                            sf_cache.Add(id, resultRight);
                            return resultRight;
                    }
                }
                else
                {
                    switch (Align)
                    {
                        case ColumnAlign.Center:
                            StringFormat resultCenter = Helper.SF_NoWrap();
                            sf_cache.Add(id, resultCenter);
                            return resultCenter;
                        case ColumnAlign.Left:
                            StringFormat resultLeft = Helper.SF_NoWrap(lr: StringAlignment.Near);
                            sf_cache.Add(id, resultLeft);
                            return resultLeft;
                        case ColumnAlign.Right:
                        default:
                            StringFormat resultRight = Helper.SF_NoWrap(lr: StringAlignment.Far);
                            sf_cache.Add(id, resultRight);
                            return resultRight;
                    }
                }
            }
        }

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding, borderWidth);

        public override GraphicsPath RenderRegion
        {
            get
            {
                if (rows == null)
                {
                    var pathnull = new GraphicsPath();
                    pathnull.AddRectangle(ReadRectangle);
                    return pathnull;
                }
                if (radius > 0)
                {
                    if (visibleHeader) return Helper.RoundPath(rect_divider, radius * Config.Dpi, true, true, false, false);
                    else return Helper.RoundPath(rect_divider, radius * Config.Dpi);
                }
                var path = new GraphicsPath();
                path.AddRectangle(rect_divider);
                return path;
            }
        }
    }
}