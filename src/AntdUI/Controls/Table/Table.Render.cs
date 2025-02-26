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
using System.Linq;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect = ClientRectangle;
            if (rows == null)
            {
                if (Empty) PaintEmpty(g, rect, 0);
                base.OnPaint(e);
                return;
            }
            try
            {
                if (columnfont == null)
                {
                    using (var column_font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                    {
                        PaintTable(g, rows, rect, column_font);
                    }
                }
                else PaintTable(g, rows, rect, columnfont);
                if (emptyHeader && Empty && rows.Length == 1) PaintEmpty(g, rect, rows[0].RECT.Height);
            }
            catch { }
            ScrollBar.Paint(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintTable(Canvas g, RowTemplate[] rows, Rectangle rect, Font column_font)
        {
            float _radius = radius * Config.Dpi;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            using (var brush_fore = new SolidBrush(fore ?? Colour.Text.Get("Table")))
            using (var brush_foreEnable = new SolidBrush(fore ?? Colour.TextQuaternary.Get("Table")))
            using (var brush_forecolumn = new SolidBrush(columnfore ?? fore ?? Colour.Text.Get("Table")))
            using (var brush_split = new SolidBrush(borderColor ?? Colour.BorderColor.Get("Table")))
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
                        int showIndex = 0;
                        foreach (var it in rows)
                        {
                            int y = it.RECT.Y - sy, b = it.RECT.Bottom - sy;
                            it.SHOW = it.ShowExpand && !it.IsColumn && (rect_read.Contains(rect_read.X, y) || rect_read.Contains(rect_read.X, b) || (it.RECT.Height > rect_read.Height && rect_read.Y > y && rect_read.Bottom < b));
                            if (it.SHOW)
                            {
                                shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex))));
                                showIndex++;
                            }
                        }

                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows) PaintBgRowFront(g, it);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows) PaintBgRowItem(g, it.row);

                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        foreach (var it in shows) PaintBg(g, it.row);

                        if (dividers.Length > 0) foreach (var divider in dividers) g.Fill(brush_split, divider);

                        g.ResetTransform();
                        g.TranslateTransform(-sx, -sy);
                        foreach (var it in shows) PaintForeItem(g, it, brush_fore, brush_foreEnable);
                        g.ResetTransform();

                        g.ResetClip();

                        PaintTableBgHeader(g, rows[0], _radius);

                        g.TranslateTransform(-sx, 0);

                        PaintTableHeader(g, rows[0], brush_forecolumn, column_font, _radius);

                        if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.Fill(brush_split, divider);
                    }
                    else
                    {
                        int showIndex = 0;
                        foreach (var it in rows)
                        {
                            it.SHOW = it.ShowExpand && it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + rect_read.Height + it.RECT.Height;
                            if (it.SHOW)
                            {
                                shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex))));
                                showIndex++;
                            }
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

                        if (dividers.Length > 0) foreach (var divider in dividers) g.Fill(brush_split, divider);

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
                        if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.Fill(brush_split, divider);
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
                    int showIndex = 0;
                    for (int index_r = 1; index_r < rows.Length; index_r++)
                    {
                        var it = rows[index_r];
                        it.SHOW = it.RECT.Y > sy - it.RECT.Height && it.RECT.Bottom < sy + rect_read.Height + it.RECT.Height;
                        if (it.SHOW)
                        {
                            shows.Add(new StyleRow(it, SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(it.RECORD, it.INDEX, showIndex))));
                            showIndex++;
                        }
                    }

                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows) PaintBgRowFront(g, it);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows) PaintBgRowItem(g, it.row);

                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    foreach (var it in shows) PaintBg(g, it.row);

                    if (dividers.Length > 0) foreach (var divider in dividers) g.Fill(brush_split, divider);

                    g.ResetTransform();
                    g.TranslateTransform(-sx, -sy);
                    foreach (var it in shows) PaintForeItem(g, it, brush_fore, brush_foreEnable);
                    if (bordered)
                    {
                        g.ResetTransform();
                        g.TranslateTransform(-sx, 0);
                    }
                    if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.Fill(brush_split, divider);
                }

                g.ResetClip();
                g.ResetTransform();

                PaintMergeCells(g, rows, sx, sy, brush_split.Color, brush_fore, brush_foreEnable);

                #region 渲染浮动列

                if (shows.Count > 0 && (fixedColumnL != null || fixedColumnR != null))
                {
                    PaintFixedColumnL(g, rect, rows, shows, brush_fore, brush_foreEnable, brush_forecolumn, column_font, brush_split, sx, sy, _radius);
                    PaintFixedColumnR(g, rect, rows, shows, brush_fore, brush_foreEnable, brush_forecolumn, column_font, brush_split, sx, sy, _radius);
                }
                else showFixedColumnL = showFixedColumnR = false;

                #endregion

                if (bordered)
                {
                    var splitsize = dividerHs.Length > 0 ? dividerHs[0].Width : (int)Config.Dpi;
                    if (_radius > 0)
                    {
                        using (var pen = new Pen(brush_split.Color, splitsize))
                        {
                            if (visibleHeader)
                            {
                                using (var path = Helper.RoundPath(rect_divider, _radius, true, true, false, false))
                                {
                                    g.Draw(pen, path);
                                }
                            }
                            else
                            {
                                using (var path = Helper.RoundPath(rect_divider, _radius))
                                {
                                    g.Draw(pen, path);
                                }
                            }
                        }
                    }
                    else g.Draw(brush_split.Color, splitsize, rect_divider);
                }
            }
        }

        #region 表头

        void PaintTableBgHeader(Canvas g, RowTemplate row, float radius)
        {
            using (var brush = new SolidBrush(columnback ?? Colour.TagDefaultBg.Get("Table")))
            {
                if (radius > 0)
                {
                    using (var path = Helper.RoundPath(row.RECT, radius, true, true, false, false))
                    {
                        g.Fill(brush, path);
                    }
                }
                else g.Fill(brush, row.RECT);
            }
            foreach (var cel in row.cells)
            {
                if (cel.COLUMN.ColStyle != null && cel.COLUMN.ColStyle.BackColor.HasValue)
                {
                    using (var brush = new SolidBrush(cel.COLUMN.ColStyle.BackColor.Value))
                    {
                        g.Fill(brush, cel.RECT);
                    }
                }
            }
        }
        void PaintTableHeader(Canvas g, RowTemplate row, SolidBrush fore, Font column_font, float radius)
        {
            foreach (TCellColumn column in row.cells)
            {
                if (column.COLUMN.SortOrder)
                {
                    g.GetImgExtend("CaretUpFilled", column.rect_up, column.COLUMN.SortMode == SortMode.ASC ? Colour.Primary.Get("Table") : Colour.TextQuaternary.Get("Table"));
                    g.GetImgExtend("CaretDownFilled", column.rect_down, column.COLUMN.SortMode == SortMode.DESC ? Colour.Primary.Get("Table") : Colour.TextQuaternary.Get("Table"));
                }
                if (column.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle) PaintCheck(g, column, columnCheck);
                else
                {
                    if (column.COLUMN.ColStyle != null && column.COLUMN.ColStyle.ForeColor.HasValue)
                    {
                        using (var brush = new SolidBrush(column.COLUMN.ColStyle.ForeColor.Value))
                        {
                            g.String(column.value, column_font, brush, column.RECT_REAL, StringFormat(column.COLUMN, true));
                        }
                    }
                    else g.String(column.value, column_font, fore, column.RECT_REAL, StringFormat(column.COLUMN, true));
                }
            }
            if (dragHeader == null) return;
            foreach (var column in row.cells)
            {
                if (dragHeader.i == column.INDEX)
                {
                    using (var brush = new SolidBrush(Colour.FillSecondary.Get("Table")))
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
                if (dragHeader.im == column.INDEX)
                {
                    using (var brush_split = new SolidBrush(Colour.BorderColor.Get("Table")))
                    {
                        int sp = (int)(2 * Config.Dpi);
                        if (dragHeader.last) g.Fill(brush_split, new Rectangle(column.RECT.Right - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                        else g.Fill(brush_split, new Rectangle(column.RECT.X - sp, column.RECT.Y, sp * 2, column.RECT.Height));
                    }
                }
            }
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
                g.Fill(rowSelectedBg ?? Colour.PrimaryBg.Get("Table"), row.row.RECT);
                if (selectedIndex.Contains(row.row.INDEX) && row.row.Select) g.Fill(Color.FromArgb(40, Colour.PrimaryActive.Get("Table")), row.row.RECT);
            }
            foreach (var cel in row.row.cells)
            {
                if (cel.COLUMN.Style != null && cel.COLUMN.Style.BackColor.HasValue) g.Fill(cel.COLUMN.Style.BackColor.Value, cel.RECT);
            }
        }

        /// <summary>
        /// 渲染背景行
        /// </summary>
        void PaintBg(Canvas g, RowTemplate row)
        {
            if (dragBody != null)
            {
                if (dragBody.i == row.INDEX) g.Fill(Colour.FillSecondary.Get("Table"), row.RECT);
                else if (dragBody.im == row.INDEX)
                {
                    using (var brush_split = new SolidBrush(Colour.BorderColor.Get("Table")))
                    {
                        int sp = (int)(2 * Config.Dpi);
                        if (dragBody.last) g.Fill(brush_split, new Rectangle(row.RECT.X, row.RECT.Bottom - sp, row.RECT.Width, sp * 2));
                        else g.Fill(brush_split, new Rectangle(row.RECT.X, row.RECT.Y - sp, row.RECT.Width, sp * 2));
                    }
                }
            }
            else
            {
                if (row.AnimationHover) g.Fill(Helper.ToColorN(row.AnimationHoverValue, Colour.FillSecondary.Get("Table")), row.RECT);
                else if (row.Hover) g.Fill(rowHoverBg ?? Colour.FillSecondary.Get("Table"), row.RECT);
            }
        }

        #region 单元格

        /// <summary>
        /// 渲染背景行
        /// </summary>
        void PaintBgRowItem(Canvas g, RowTemplate row)
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
                if (it is TCellCheck check) PaintCheck(g, check, enable);
                else if (it is TCellRadio radio) PaintRadio(g, radio, enable);
                else if (it is TCellSwitch _switch) PaintSwitch(g, _switch, enable);
                else if (it is TCellSort sort)
                {
                    if (sort.AnimationHover)
                    {
                        using (var brush = new SolidBrush(Helper.ToColorN(sort.AnimationHoverValue, Colour.FillTertiary.Get("Table"))))
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
                            g.Fill(Colour.FillTertiary.Get("Table"), path_sort);
                        }
                    }
                    SvgExtend.GetImgExtend(g, "HolderOutlined", sort.RECT_ICO, fore.Color);
                }
                else if (it is Template template)
                {
                    g.SetClip(it.RECT);
                    foreach (var item in template.Value) item.Paint(g, Font, enable, fore);
                }
                else if (it is TCellText text)
                {
                    g.SetClip(it.RECT);
                    g.String(text.value, Font, fore, text.RECT_REAL, StringFormat(text.COLUMN));
                }
                if (dragHeader != null && dragHeader.i == it.INDEX) g.Fill(Colour.FillSecondary.Get("Table"), it.RECT);
                if (it.ROW.CanExpand && it.ROW.KeyTreeINDEX == columnIndex)
                {
                    using (var path_check = Helper.RoundPath(it.ROW.RectExpand, check_radius, false))
                    {
                        g.Fill(Colour.BgBase.Get("Table"), path_check);
                        g.Draw(Colour.BorderColor.Get("Table"), check_border, path_check);
                        PaintArrow(g, it.ROW, fore, it.ROW.Expand ? 90 : 0);
                    }
                }
            }
            catch { }
            g.Restore(state);
        }

        #endregion

        void PaintArrow(Canvas g, RowTemplate item, SolidBrush color, int ArrowProg)
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

        void PaintFixedColumnL(Canvas g, Rectangle rect, RowTemplate[] rows, List<StyleRow> shows, SolidBrush fore, SolidBrush foreEnable, SolidBrush forecolumn, Font column_font, SolidBrush brush_split, int sx, int sy, float radius)
        {
            if (fixedColumnL != null && sx > 0)
            {
                showFixedColumnL = true;
                var last = shows[shows.Count - 1].row.cells[fixedColumnL[fixedColumnL.Count - 1]];
                var rect_Fixed = new Rectangle(rect.X, rect.Y, last.RECT.Right, last.RECT.Bottom);

                #region 绘制阴影

                if (_gap > 0)
                {
                    var rect_show = new Rectangle(rect.X + last.RECT.Right - _gap, rect.Y, _gap * 2, last.RECT.Bottom);
                    using (var brush = new LinearGradientBrush(rect_show, Colour.FillSecondary.Get("Table"), Color.Transparent, 0F))
                    {
                        g.Fill(brush, rect_show);
                    }
                }
                g.Fill(Colour.BgBase.Get("Table"), rect_Fixed);

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
                        if (!row.row.IsColumn) PaintItemFixed(g, row.row.cells[fixedIndex], row.row.ENABLE, row.row.ENABLE ? fore : foreEnable, row.style);
                    }
                }
                if (dividers.Length > 0) foreach (var divider in dividers) g.Fill(brush_split, divider);
                g.ResetTransform();
                if (fixedHeader)
                {
                    PaintTableBgHeader(g, rows[0], radius);
                    PaintTableHeader(g, rows[0], forecolumn, column_font, radius);
                }
                if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.Fill(brush_split, divider);
                g.ResetClip();
            }
            else showFixedColumnL = false;
        }
        void PaintFixedColumnR(Canvas g, Rectangle rect, RowTemplate[] rows, List<StyleRow> shows, SolidBrush fore, SolidBrush foreEnable, SolidBrush forecolumn, Font column_font, SolidBrush brush_split, int sx, int sy, float radius)
        {
            if (fixedColumnR != null && ScrollBar.ShowX)
            {
                try
                {
                    var lastrow = shows[shows.Count - 1];
                    CELL first = lastrow.row.cells[fixedColumnR[fixedColumnR.Count - 1]], last = lastrow.row.cells[fixedColumnR[0]];
                    if (sx + rect.Width < last.RECT.Right)
                    {
                        sFixedR = last.RECT.Right - rect.Width;
                        showFixedColumnR = true;
                        int w = last.RECT.Right - first.RECT.Left;

                        var rect_Fixed = new Rectangle(rect.Right - w, rect.Y, last.RECT.Right, last.RECT.Bottom);

                        #region 绘制阴影

                        if (_gap > 0)
                        {
                            var rect_show = new Rectangle(rect.Right - w - _gap, rect.Y, _gap * 2, last.RECT.Bottom);
                            using (var brush = new LinearGradientBrush(rect_show, Color.Transparent, Colour.FillSecondary.Get("Table"), 0F))
                            {
                                g.Fill(brush, rect_show);
                            }
                        }
                        g.Fill(Colour.BgBase.Get("Table"), rect_Fixed);

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
                                    PaintItemFixed(g, row.row.cells[fixedIndex], row.row.ENABLE, row.row.ENABLE ? fore : foreEnable, row.style);
                                }
                            }
                        }
                        g.ResetTransform();
                        g.TranslateTransform(0, -sy);
                        if (dividers.Length > 0) foreach (var divider in dividers) g.Fill(brush_split, divider);
                        g.ResetTransform();
                        if (fixedHeader)
                        {
                            PaintTableBgHeader(g, rows[0], radius);
                            g.TranslateTransform(-sFixedR, 0);
                            PaintTableHeader(g, rows[0], forecolumn, column_font, radius);
                        }
                        g.ResetTransform();
                        g.TranslateTransform(-sFixedR, 0);
                        if (dividerHs.Length > 0) foreach (var divider in dividerHs) g.Fill(brush_split, divider);

                        g.ResetTransform();
                        g.ResetClip();
                    }
                    else showFixedColumnR = false;
                }
                catch { }
            }
            else showFixedColumnR = false;
        }

        #endregion

        #endregion

        #region 合并

        void PaintMergeCells(Canvas g, RowTemplate[] rows, int sx, int sy, Color split_color, SolidBrush fore, SolidBrush foreEnable)
        {
            if (CellRanges == null || CellRanges.Length == 0) return;
            g.TranslateTransform(-sx, -sy);
            int sps = dividerHs.Length > 0 ? dividerHs[0].Width : (int)Config.Dpi;
            var sps2 = sps / 2F;
            using (var bg = new SolidBrush(Colour.BgBase.Get("Table")))
            {
                foreach (var it in CellRanges)
                {
                    PaintMergeCells(g, rows, bg, split_color, fore, foreEnable, sps, sps2, it);
                }
            }
            g.ResetClip();
            g.ResetTransform();
        }

        void PaintMergeCells(Canvas g, RowTemplate[] rows, SolidBrush bg, Color split_color, SolidBrush fore, SolidBrush foreEnable, int sps, float sps2, CellRange range)
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

        void PaintMergeCells(Canvas g, SolidBrush bg, Color split_color, SolidBrush fore, SolidBrush foreEnable, int sps, float sps2, CELL first, CELL last)
        {
            var state = g.Save();
            var rect = RectMergeCells(first, last, out bool fz);
            g.Fill(bg, rect);
            if (first.ROW.AnimationHover) g.Fill(Helper.ToColorN(first.ROW.AnimationHoverValue, Colour.FillSecondary.Get("Table")), rect);
            else if (first.ROW.Hover) g.Fill(rowHoverBg ?? Colour.FillSecondary.Get("Table"), rect);
            g.Draw(split_color, sps, new RectangleF(rect.X + sps2, rect.Y + sps2, rect.Width, rect.Height));

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

        #region 渲染复选框

        #region 复选框

        void PaintCheck(Canvas g, TCellColumn check, ColumnCheck columnCheck)
        {
            using (var path_check = Helper.RoundPath(check.RECT_REAL, check_radius, false))
            {
                if (columnCheck.AnimationCheck)
                {
                    g.Fill(Colour.BgBase.Get("Checkbox"), path_check);
                    var alpha = 255 * columnCheck.AnimationCheckValue;
                    if (columnCheck.CheckState == CheckState.Indeterminate || (columnCheck.checkStateOld == CheckState.Indeterminate && !columnCheck.Checked))
                    {
                        g.Draw(Colour.BorderColor.Get("Checkbox"), check_border, path_check);
                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Checkbox")), PaintBlock(check.RECT_REAL));
                    }
                    else
                    {
                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Checkbox")), path_check);
                        using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get("Checkbox")), check_border * 2))
                        {
                            g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                        }
                        if (columnCheck.Checked)
                        {
                            float max = check.RECT_REAL.Height + check.RECT_REAL.Height * columnCheck.AnimationCheckValue, alpha2 = 100 * (1F - columnCheck.AnimationCheckValue);
                            using (var brush = new SolidBrush(Helper.ToColor(alpha2, Colour.Primary.Get("Checkbox"))))
                            {
                                g.FillEllipse(brush, new RectangleF(check.RECT_REAL.X + (check.RECT_REAL.Width - max) / 2F, check.RECT_REAL.Y + (check.RECT_REAL.Height - max) / 2F, max, max));
                            }
                        }
                        g.Draw(Colour.Primary.Get("Checkbox"), check_border, path_check);
                    }
                }
                else if (columnCheck.CheckState == CheckState.Indeterminate)
                {
                    g.Fill(Colour.BgBase.Get("Checkbox"), path_check);
                    g.Draw(Colour.BorderColor.Get("Checkbox"), check_border, path_check);
                    g.Fill(Colour.Primary.Get("Checkbox"), PaintBlock(check.RECT_REAL));
                }
                else if (columnCheck.Checked)
                {
                    g.Fill(Colour.Primary.Get("Checkbox"), path_check);
                    using (var brush = new Pen(Colour.BgBase.Get("Checkbox"), check_border * 2))
                    {
                        g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                    }
                }
                else
                {
                    g.Fill(Colour.BgBase.Get("Checkbox"), path_check);
                    g.Draw(Colour.BorderColor.Get("Checkbox"), check_border, path_check);
                }
            }
        }
        void PaintCheck(Canvas g, TCellCheck check, bool enable)
        {
            using (var path = Helper.RoundPath(check.RECT_REAL, check_radius, false))
            {
                if (enable)
                {
                    if (check.AnimationCheck)
                    {
                        g.Fill(Colour.BgBase.Get("Checkbox"), path);

                        var alpha = 255 * check.AnimationCheckValue;

                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Checkbox")), path);
                        using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get("Checkbox")), check_border * 2))
                        {
                            g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                        }

                        if (check.Checked)
                        {
                            float max = check.RECT_REAL.Height + check.RECT_REAL.Height * check.AnimationCheckValue, alpha2 = 100 * (1F - check.AnimationCheckValue);
                            using (var brush = new SolidBrush(Helper.ToColor(alpha2, Colour.Primary.Get("Checkbox"))))
                            {
                                g.FillEllipse(brush, new RectangleF(check.RECT_REAL.X + (check.RECT_REAL.Width - max) / 2F, check.RECT_REAL.Y + (check.RECT_REAL.Height - max) / 2F, max, max));
                            }
                        }
                        g.Draw(Colour.Primary.Get("Checkbox"), check_border, path);
                    }
                    else if (check.Checked)
                    {
                        g.Fill(Colour.Primary.Get("Checkbox"), path);
                        using (var brush = new Pen(Colour.BgBase.Get("Checkbox"), check_border * 2))
                        {
                            g.DrawLines(brush, PaintArrow(check.RECT_REAL));
                        }
                    }
                    else
                    {
                        g.Fill(Colour.BgBase.Get("Checkbox"), path);
                        g.Draw(Colour.BorderColor.Get("Checkbox"), check_border, path);
                    }
                }
                else
                {
                    g.Fill(Colour.FillQuaternary.Get("Checkbox"), path);
                    if (check.Checked) g.DrawLines(Colour.TextQuaternary.Get("Checkbox"), check_border * 2, PaintArrow(check.RECT_REAL));
                    g.Draw(Colour.BorderColorDisable.Get("Checkbox"), check_border, path);
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
                g.FillEllipse(Colour.BgBase.Get("Radio"), radio.RECT_REAL);
                if (radio.AnimationCheck)
                {
                    float dot = dot_size * 0.3F;
                    using (var path = new GraphicsPath())
                    {
                        float dot_ant = dot_size - dot * radio.AnimationCheckValue, dot_ant2 = dot_ant / 2F, alpha = 255 * radio.AnimationCheckValue;
                        path.AddEllipse(radio.RECT_REAL);
                        path.AddEllipse(new RectangleF(radio.RECT_REAL.X + dot_ant2, radio.RECT_REAL.Y + dot_ant2, radio.RECT_REAL.Width - dot_ant, radio.RECT_REAL.Height - dot_ant));
                        g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Radio")), path);
                    }
                    if (radio.Checked)
                    {
                        float max = radio.RECT_REAL.Height + radio.RECT_REAL.Height * radio.AnimationCheckValue, alpha2 = 100 * (1F - radio.AnimationCheckValue);
                        g.FillEllipse(Helper.ToColor(alpha2, Colour.Primary.Get("Radio")), new RectangleF(radio.RECT_REAL.X + (radio.RECT_REAL.Width - max) / 2F, radio.RECT_REAL.Y + (radio.RECT_REAL.Height - max) / 2F, max, max));
                    }
                    g.DrawEllipse(Colour.Primary.Get("Radio"), check_border, radio.RECT_REAL);
                }
                else if (radio.Checked)
                {
                    float dot = dot_size * 0.3F, dot2 = dot / 2F;
                    g.DrawEllipse(Color.FromArgb(250, Colour.Primary.Get("Radio")), dot, new RectangleF(radio.RECT_REAL.X + dot2, radio.RECT_REAL.Y + dot2, radio.RECT_REAL.Width - dot, radio.RECT_REAL.Height - dot));
                    g.DrawEllipse(Colour.Primary.Get("Radio"), check_border, radio.RECT_REAL);
                }
                else g.DrawEllipse(Colour.BorderColor.Get("Radio"), check_border, radio.RECT_REAL);
            }
            else
            {
                g.FillEllipse(Colour.FillQuaternary.Get("Radio"), radio.RECT_REAL);
                if (radio.Checked)
                {
                    float dot = dot_size / 2F, dot2 = dot / 2F;
                    g.FillEllipse(Colour.TextQuaternary.Get("Radio"), new RectangleF(radio.RECT_REAL.X + dot2, radio.RECT_REAL.Y + dot2, radio.RECT_REAL.Width - dot, radio.RECT_REAL.Height - dot));
                }
                g.DrawEllipse(Colour.BorderColorDisable.Get("Radio"), check_border, radio.RECT_REAL);
            }
        }

        #endregion

        #region 开关

        void PaintSwitch(Canvas g, TCellSwitch _switch, bool enable)
        {
            var color = Colour.Primary.Get("Switch");
            using (var path = _switch.RECT_REAL.RoundPath(_switch.RECT_REAL.Height))
            {
                using (var brush = new SolidBrush(Colour.TextQuaternary.Get("Switch")))
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
                    g.FillEllipse(enable ? Colour.BgBase.Get("Switch") : Color.FromArgb(200, Colour.BgBase.Get("Switch")), dot_rect);
                }
                else if (_switch.Checked)
                {
                    var colorhover = Colour.PrimaryHover.Get("Switch");
                    g.Fill(color, path);
                    if (_switch.AnimationHover) g.Fill(Helper.ToColorN(_switch.AnimationHoverValue, colorhover), path);
                    else if (_switch.ExtraMouseHover) g.Fill(colorhover, path);
                    var dot_rect = new RectangleF(_switch.RECT_REAL.X + gap + _switch.RECT_REAL.Width - _switch.RECT_REAL.Height, _switch.RECT_REAL.Y + gap, _switch.RECT_REAL.Height - gap2, _switch.RECT_REAL.Height - gap2);
                    g.FillEllipse(enable ? Colour.BgBase.Get("Switch") : Color.FromArgb(200, Colour.BgBase.Get("Switch")), dot_rect);
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
                    g.FillEllipse(enable ? Colour.BgBase.Get("Switch") : Color.FromArgb(200, Colour.BgBase.Get("Switch")), dot_rect);
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

        void PaintEmpty(Canvas g, Rectangle rect, int offset)
        {
            string emptytext = EmptyText ?? Localization.Get("NoData", "暂无数据");
            using (var brush = new SolidBrush(fore ?? Colour.Text.Get("Table")))
            {
                if (offset > 0)
                {
                    rect.Offset(0, offset);
                    rect.Height -= offset;
                }
                if (EmptyImage == null) g.String(emptytext, Font, brush, rect, StringFormat(ColumnAlign.Center));
                else
                {
                    int gap = (int)(_gap * Config.Dpi);
                    var size = g.MeasureString(emptytext, Font);
                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - EmptyImage.Width) / 2, rect.Y + (rect.Height - EmptyImage.Height) / 2 - size.Height, EmptyImage.Width, EmptyImage.Height),
                        rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);
                    g.Image(EmptyImage, rect_img);
                    g.String(emptytext, Font, brush, rect_font, StringFormat(ColumnAlign.Center));
                }
            }
        }

        public static StringFormat StringFormat(Column column, bool isColumn) =>
          isColumn ? StringFormat(column.ColAlign ?? column.Align, LineBreak: column.ColBreak) : StringFormat(column);

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
    }
}