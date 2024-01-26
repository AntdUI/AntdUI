// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Table 表格
    /// </summary>
    /// <remarks>展示行列数据。</remarks>
    [Description("Table 表格")]
    [ToolboxItem(true)]
    public class Table : IControl
    {
        #region 属性

        Column[]? columns = null;
        /// <summary>
        /// 表格列的配置
        /// </summary>
        [Browsable(false), Description("表格列的配置"), Category("数据"), DefaultValue(null)]
        public Column[]? Columns
        {
            get => columns;
            set
            {
                if (columns == value) return;
                if (dataSource == null)
                {
                    columns = value;
                    ExtractHeader();
                    return;
                }
                List<string> oldid = new List<string>(), id = new List<string>();
                if (columns != null) foreach (var col in columns) oldid.Add(col.Key);
                if (value != null) foreach (var col in value) id.Add(col.Key);
                columns = value;
                if (string.Join("", oldid) != string.Join("", id)) { ExtractHeader(); ExtractData(); }
                LoadLayout();
                Invalidate();
            }
        }

        object? dataSource = null;
        /// <summary>
        /// 数据数组
        /// </summary>
        [Browsable(false), Description("数据数组"), Category("数据"), DefaultValue(null)]
        public object? DataSource
        {
            get => dataSource;
            set
            {
                if (dataSource == value) return;
                dataSource = value;
                ExtractData();
                LoadLayout();
                Invalidate();
            }
        }

        int _gap = 12;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(12)]
        public int Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                LoadLayout();
                Invalidate();
            }
        }

        int _checksize = 16;
        /// <summary>
        /// 复选框大小
        /// </summary>
        [Description("复选框大小"), Category("外观"), DefaultValue(16)]
        public int CheckSize
        {
            get => _checksize;
            set
            {
                if (_checksize == value) return;
                _checksize = value;
                LoadLayout();
                Invalidate();
            }
        }

        bool fixedHeader = true;
        /// <summary>
        /// 固定表头
        /// </summary>
        [Description("固定表头"), Category("外观"), DefaultValue(true)]
        public bool FixedHeader
        {
            get => fixedHeader;
            set
            {
                if (fixedHeader == value) return;
                fixedHeader = value;
                Invalidate();
            }
        }

        bool bordered = false;
        /// <summary>
        /// 显示列边框
        /// </summary>
        [Description("显示列边框"), Category("外观"), DefaultValue(false)]
        public bool Bordered
        {
            get => bordered;
            set
            {
                if (bordered == value) return;
                bordered = value;
                Invalidate();
            }
        }

        TEditMode editmode = TEditMode.None;
        /// <summary>
        /// 编辑模式
        /// </summary>
        [Description("编辑模式"), Category("行为"), DefaultValue(TEditMode.None)]
        public TEditMode EditMode
        {
            get => editmode;
            set
            {
                if (editmode == value) return;
                editmode = value;
                Invalidate();
            }
        }

        #region 为空

        [Description("是否显示空样式"), Category("外观"), DefaultValue(true)]
        public bool Empty { get; set; } = true;

        string emptyText = "No data";
        [Description("数据为空显示文字"), Category("外观"), DefaultValue("No data")]
        public string EmptyText
        {
            get => emptyText;
            set
            {
                if (emptyText == value) return;
                emptyText = value;
                Invalidate();
            }
        }

        [Description("数据为空显示图片"), Category("外观"), DefaultValue(null)]
        public Image? EmptyImage { get; set; }

        #endregion

        #endregion

        #region 渲染

        static StringFormat stringLeft = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap };
        static StringFormat stringCenter = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap };
        static StringFormat stringRight = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap };

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
                            int gap = (int)(_gap * (g.DpiX / 96F));
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
            using (var brush_split = new SolidBrush(Style.Db.Split))
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
                    g.ResetTransform();
                    g.TranslateTransform(-sx, 0);
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

        #endregion

        #region 布局

        protected override void OnFontChanged(EventArgs e)
        {
            LoadLayout();
            Invalidate();
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout();
            base.OnSizeChanged(e);
        }

        internal RowTemplate[]? rows = null;
        RectangleF[] dividers = new RectangleF[0], dividerHs = new RectangleF[0];

        void LoadLayout()
        {
            var rect = ChangeLayout();
            scrollX.SizeChange(rect);
            if (scrollX.Show) scrollX.SizeChange(new Rectangle(rect.X, rect.Y, rect.Width - 20, rect.Height));
            scrollY.SizeChange(rect);
        }
        bool has_check = false;
        Rectangle ChangeLayout()
        {
            has_check = false;
            if (data_temp != null)
            {
                var _rows = new List<RowTemplate>();
                var _columns = new List<Column>();
                int processing = 0, check_count = 0;
                var col_width = new Dictionary<int, object>();
                if (columns == null) foreach (var it in data_temp.columns) _columns.Add(new Column(it.key, it.key));
                else
                {
                    int x = 0;
                    foreach (var it in columns)
                    {
                        _columns.Add(it);
                        if (it.Width != null)
                        {
                            if (it.Width.EndsWith("%") && float.TryParse(it.Width.TrimEnd('%'), out var f)) col_width.Add(x, f / 100F);
                            else if (int.TryParse(it.Width, out var i)) col_width.Add(x, (int)(i * Config.Dpi));
                        }
                        x++;
                    }
                }

                foreach (var row in data_temp.rows)
                {
                    var cells = new List<TCell>();
                    foreach (var column in _columns)
                    {
                        var value = row.cells[column.Key];
                        if (value is PropertyDescriptor prop) AddRows(ref cells, ref processing, ref check_count, column, row.record, prop);
                        else cells.Add(new TCellText(this, null, value, column, value?.ToString()));
                    }
                    if (cells.Count > 0) AddRows(ref _rows, cells.ToArray(), row.record);
                }

                if (_rows.Count > 0)
                {
                    #region 添加表头

                    var _cols = new List<TCellColumn>();
                    CheckState checkState = CheckState.Unchecked;
                    foreach (var it in _columns)
                    {
                        if (check_count > 0 && it is ColumnCheck check)
                        {
                            checkState = check_count == _rows.Count ? CheckState.Checked : CheckState.Indeterminate;
                            check.CheckState = checkState;
                        }
                        _cols.Add(new TCellColumn(this, it));
                    }
                    AddRowsColumn(ref _rows, _cols.ToArray(), dataSource, checkState);

                    #endregion

                    #region 计算坐标

                    float x = 0, y = 0;
                    bool is_exceed = false;

                    var rect = ClientRectangle.PaddingRect(Padding);

                    List<RectangleF> _dividerHs = new List<RectangleF>(), _dividers = new List<RectangleF>();

                    using (var temp_bmp = new Bitmap(1, 1))
                    {
                        using (var g = Graphics.FromImage(temp_bmp))
                        {
                            var dpi = g.DpiX / 96F;
                            check_radius = _checksize * 0.25F * dpi;
                            check_border = _checksize * 0.125F * dpi;
                            int check_size = (int)(_checksize * dpi), gap = (int)(_gap * dpi), gap2 = gap * 2;
                            float split = 1F * dpi, split2 = split / 2F;

                            #region 布局高宽

                            var temp_width_cell = new Dictionary<int, float>();
                            foreach (var row in _rows)
                            {
                                float max_height = 0;
                                for (int i = 0; i < row.cells.Length; i++)
                                {
                                    if (!temp_width_cell.ContainsKey(i)) temp_width_cell.Add(i, 0F);
                                    var it = row.cells[i];
                                    if (it is TCellCheck check)
                                    {
                                        if (max_height < gap2) max_height = gap2;
                                        temp_width_cell[i] = -1F;
                                    }
                                    else
                                    {
                                        var text_size = it.GetSize(g, Font, gap, gap2);
                                        if (max_height < text_size.Height) max_height = text_size.Height;
                                        if (temp_width_cell[i] < text_size.Width) temp_width_cell[i] = text_size.Width;
                                    }
                                }
                                row.Height = (int)Math.Round(max_height) + gap2;
                            }

                            int use_width = rect.Width;
                            float max_width = 0;
                            foreach (var it in temp_width_cell)
                            {
                                if (col_width.ContainsKey(it.Key))
                                {
                                    var value = col_width[it.Key];
                                    if (value is int val_int) max_width += val_int;
                                    if (value is float val_float) max_width += rect.Width * val_float;
                                }
                                else if (it.Value == -1F) use_width -= check_size * 2;
                                else max_width += it.Value;
                            }
                            var width_cell = new Dictionary<int, int>();
                            if (max_width > rect.Width)
                            {
                                is_exceed = true;
                                foreach (var it in temp_width_cell)
                                {
                                    if (col_width.ContainsKey(it.Key))
                                    {
                                        var value = col_width[it.Key];
                                        if (value is int val_int) width_cell.Add(it.Key, val_int);
                                        else if (value is float val_float) width_cell.Add(it.Key, (int)Math.Ceiling(rect.Width * val_float));
                                    }
                                    else if (it.Value == -1F)
                                    {
                                        int _check_size = check_size * 2;
                                        width_cell.Add(it.Key, _check_size);
                                    }
                                    else
                                    {
                                        int value = (int)Math.Ceiling(it.Value);
                                        width_cell.Add(it.Key, value);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var it in temp_width_cell)
                                {
                                    if (col_width.ContainsKey(it.Key))
                                    {
                                        var value = col_width[it.Key];
                                        if (value is int val_int) width_cell.Add(it.Key, val_int);
                                        else if (value is float val_float) width_cell.Add(it.Key, (int)Math.Ceiling(rect.Width * val_float));
                                    }
                                    else if (it.Value == -1F)
                                    {
                                        int _check_size = check_size * 2;
                                        width_cell.Add(it.Key, _check_size);
                                    }
                                    else
                                    {
                                        int value = (int)Math.Ceiling(use_width * (it.Value / max_width));
                                        width_cell.Add(it.Key, value);
                                    }
                                }
                            }

                            #endregion

                            #region 最终坐标

                            int use_y = rect.Y;
                            foreach (var row in _rows)
                            {
                                int use_x = rect.X;
                                row.RECT = new Rectangle(rect.X, use_y, rect.Width, row.Height);
                                for (int i = 0; i < row.cells.Length; i++)
                                {
                                    var it = row.cells[i];
                                    var _rect = new Rectangle(use_x, use_y, width_cell[i], row.RECT.Height);
                                    if (it is TCellCheck check) check.SetSize(_rect, check_size);
                                    else if (it is TCellRadio radio) radio.SetSize(_rect, check_size);
                                    else if (it is TCellColumn column)
                                    {
                                        it.SetSize(g, Font, _rect, gap, gap2);
                                        if (column.tag is ColumnCheck columnCheck)
                                        {
                                            columnCheck.PARENT = this;
                                            //全选
                                            column.rect = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
                                        }
                                        else
                                        {
                                            column.rect = new Rectangle(_rect.X + gap, _rect.Y + gap, _rect.Width - gap2, _rect.Height - gap2);
                                            if (x < column.rect.Right) x = column.rect.Right;
                                        }
                                    }
                                    else it.SetSize(g, Font, _rect, gap, gap2);

                                    if (x < _rect.Right) x = _rect.Right;
                                    if (y < _rect.Bottom) y = _rect.Bottom;
                                    use_x += width_cell[i];
                                }

                                if (row.IsColumn)
                                {
                                    if (bordered)
                                    {
                                        for (int i = 0; i < row.cells.Length - 1; i++)
                                        {
                                            var it = (TCellColumn)row.cells[i];
                                            _dividerHs.Add(new RectangleF(it.rect.Right + gap - split2, rect.Y, split, rect.Height));
                                        }
                                        _dividers.Add(new RectangleF(rect.X, row.RECT.Bottom - split2, rect.Width, split));
                                    }
                                    else
                                    {
                                        for (int i = 0; i < row.cells.Length - 1; i++)
                                        {
                                            var it = (TCellColumn)row.cells[i];
                                            _dividerHs.Add(new RectangleF(it.rect.Right + gap - split2, it.rect.Y, split, it.rect.Height));
                                        }
                                    }
                                }
                                else
                                {
                                    if (bordered) _dividers.Add(new RectangleF(rect.X, row.RECT.Bottom - split2, rect.Width, split));
                                    else _dividers.Add(new RectangleF(row.RECT.X, row.RECT.Bottom - split2, row.RECT.Width, split));
                                }
                                use_y += row.Height;
                            }

                            #endregion
                        }
                    }

                    dividerHs = _dividerHs.ToArray();
                    dividers = _dividers.ToArray();
                    rows = _rows.ToArray();

                    #endregion

                    scrollX.SetVrSize(is_exceed ? x : 0, rect.Width);
                    scrollY.SetVrSize(y, rect.Height);

                    if (processing == 0) { ThreadState?.Dispose(); ThreadState = null; }
                    else
                    {
                        if (Config.Animation && ThreadState == null)
                        {
                            ThreadState = new ITask(this, i =>
                            {
                                AnimationStateValue = i;
                                Invalidate();
                            }, 50, 1F, 0.05F);
                        }
                    }

                    return rect;
                }
                else
                {
                    ThreadState?.Dispose(); ThreadState = null;

                    scrollX.SetVrSize(0, 0);
                    scrollY.SetVrSize(0, 0);
                    dividers = new RectangleF[0];
                    rows = null;
                }
            }
            return Rectangle.Empty;
        }

        #region 动画

        ITask? ThreadState = null;
        internal float AnimationStateValue = 0;

        #endregion

        float check_radius = 0F, check_border = 2F;
        void AddRows(ref List<TCell> cells, ref int processing, ref int check_count, Column column, object ov, PropertyDescriptor prop)
        {
            if (column is ColumnCheck)
            {
                //复选框
                has_check = true;
                var value = prop.GetValue(ov);
                if (value is bool check && check)
                {
                    check_count++;
                    AddRows(ref cells, new TCellCheck(this, prop, ov, true));
                }
                else AddRows(ref cells, new TCellCheck(this, prop, ov, false));
            }
            else if (column is ColumnRadio)
            {
                //单选框
                has_check = true;
                var value = prop.GetValue(ov);
                if (value is bool check && check)
                {
                    check_count++;
                    AddRows(ref cells, new TCellRadio(this, prop, ov, true));
                }
                else AddRows(ref cells, new TCellRadio(this, prop, ov, false));
            }
            else
            {
                var value = prop.GetValue(ov);
                if (value is IList<ICell> icells) AddRows(ref cells, new Template(this, prop, ov, column, ref processing, icells));
                else if (value is ICell icell) AddRows(ref cells, new Template(this, prop, ov, column, ref processing, new ICell[] { icell }));
                else AddRows(ref cells, new TCellText(this, prop, ov, column, value?.ToString()));
            }
        }

        bool handcheck = false;
        List<object> has_notify = new List<object>();
        void AddRows(ref List<TCell> cells, TCell data)
        {
            cells.Add(data);
            if (data is Template template)
            {
                foreach (var it in template.value)
                {
                    if (it is TemplateBadge badge)
                    {
                        badge.Value.Changed = key =>
                        {
                            PropertyChanged(data, key);
                        };
                    }
                    else if (it is TemplateTag tag)
                    {
                        tag.Value.Changed = key =>
                        {
                            PropertyChanged(data, key);
                        };
                    }
                    else if (it is TemplateImage image)
                    {
                        image.Value.Changed = key =>
                        {
                            PropertyChanged(data, key);
                        };
                    }
                    else if (it is TemplateButton link)
                    {
                        link.Value.Changed = key =>
                        {
                            PropertyChanged(data, key);
                        };
                    }
                    else if (it is TemplateText text)
                    {
                        text.Value.Changed = key =>
                        {
                            PropertyChanged(data, key);
                        };
                    }
                }
            }
            if (data.VALUE is NotifyProperty notifya)
            {
                notifya.Changed = key =>
                {
                    PropertyChanged(data, key);
                };
            }
            else if (!has_notify.Contains(data.VALUE) && data.VALUE is INotifyPropertyChanged notify)
            {
                has_notify.Add(data.VALUE);
                notify.PropertyChanged += (a, b) =>
                {
                    if (b.PropertyName == null) return;
                    PropertyChanged(data, b.PropertyName);
                };
            }
        }
        void PropertyChanged(TCell data, string key)
        {
            if (columns == null || rows == null || handcheck || data.PROPERTY == null || key == null) return;
            //var value = data.PROPERTY.GetValue(data.VALUE);
            foreach (var it in columns)
            {
                if (it.Key == key)
                {
                    if (it is ColumnCheck check)
                    {
                        int check_count = 0;
                        for (int i = 1; i < rows.Length; i++)
                        {
                            foreach (var cell in rows[i].cells)
                            {
                                if (cell is TCellCheck checkCell)
                                {
                                    if (checkCell.Checked) check_count++;
                                    break;
                                }
                            }
                        }
                        if (rows.Length - 1 == check_count)
                        {
                            //全选
                            rows[0].CheckState = CheckState.Checked;
                        }
                        else if (check_count > 0) rows[0].CheckState = CheckState.Indeterminate;
                        else rows[0].CheckState = CheckState.Unchecked;
                        check.SetCheckState(rows[0].CheckState);
                    }
                    else Invalidate();
                }
            }
        }
        void AddRows(ref List<RowTemplate> rows, TCell[] cells, object Record)
        {
            var row = new RowTemplate(this, cells, Record);
            foreach (var it in row.cells) it.ROW = row;
            rows.Add(row);
        }
        void AddRowsColumn(ref List<RowTemplate> rows, TCell[] cells, object Record, CheckState check)
        {
            var row = new RowTemplate(this, cells, Record, check);
            foreach (var it in row.cells) it.ROW = row;
            rows.Insert(0, row);
        }

        ScrollX scrollX;
        ScrollY scrollY;
        public Table() { scrollX = new ScrollX(this); scrollY = new ScrollY(this); }

        protected override void Dispose(bool disposing)
        {
            ThreadState?.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region 鼠标

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (rows == null || CellClick == null) return;
            int sx = (int)scrollX.Value, sy = (int)scrollY.Value;
            int px = e.X + sx, py = e.Y + sy;
            for (int i_row = 0; i_row < rows.Length; i_row++)
            {
                var it = rows[i_row];
                if (it.Contains(e.X, py))
                {
                    if (showFixedColumnL && fixedColumnL != null)
                    {
                        foreach (var i_col in fixedColumnL)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X, py))
                            {
                                CellClick.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    if (showFixedColumnR && fixedColumnR != null)
                    {
                        foreach (var i_col in fixedColumnR)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X + sFixedR, py))
                            {
                                CellClick.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sFixedR, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    for (int i_col = 0; i_col < it.cells.Length; i_col++)
                    {
                        var item = it.cells[i_col];
                        if (item.CONTAINS(px, py))
                        {
                            CellClick.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sx, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                            return;
                        }
                    }
                    return;
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (rows == null) return;
            int sx = (int)scrollX.Value, sy = (int)scrollY.Value;
            int px = e.X + sx, py = e.Y + sy;
            for (int i_row = 0; i_row < rows.Length; i_row++)
            {
                var it = rows[i_row];
                if (it.Contains(e.X, py))
                {
                    if (showFixedColumnL && fixedColumnL != null)
                    {
                        foreach (var i_col in fixedColumnL)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X, py))
                            {
                                if (editmode == TEditMode.DoubleClick)
                                {
                                    //进入编辑模式
                                    EditModeClose();
                                    OnEditMode(it, item, i_row, i_col, 0, sy);
                                }
                                CellDoubleClick?.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    if (showFixedColumnR && fixedColumnR != null)
                    {
                        foreach (var i_col in fixedColumnR)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X + sFixedR, py))
                            {
                                if (editmode == TEditMode.DoubleClick)
                                {
                                    //进入编辑模式
                                    EditModeClose();
                                    OnEditMode(it, item, i_row, i_col, sFixedR, sy);
                                }
                                CellDoubleClick?.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sFixedR, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    for (int i_col = 0; i_col < it.cells.Length; i_col++)
                    {
                        var item = it.cells[i_col];
                        if (item.CONTAINS(px, py))
                        {
                            if (editmode == TEditMode.DoubleClick)
                            {
                                //进入编辑模式
                                EditModeClose();
                                OnEditMode(it, item, i_row, i_col, sx, sy);
                            }
                            CellDoubleClick?.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sx, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                            return;
                        }
                    }
                    return;
                }
            }
        }

        #region 鼠标按下

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (scrollY.MouseDown(e.Location) && scrollX.MouseDown(e.Location))
            {
                base.OnMouseDown(e);
                if (rows == null) return;
                int px = e.X + (int)scrollX.Value, py = e.Y + (int)scrollY.Value;
                for (int i_row = 0; i_row < rows.Length; i_row++)
                {
                    var it = rows[i_row];
                    if (it.IsColumn)
                    {
                        if (it.CONTAINS(e.X, py) || (fixedHeader && it.CONTAINS(e.X, e.Y)))
                        {
                            if (has_check)
                            {
                                if (showFixedColumnL && fixedColumnL != null)
                                {
                                    foreach (var i in fixedColumnL)
                                    {
                                        if (MouseDownRowColumn(e, it, (TCellColumn)it.cells[i], rows, e.X, py)) return;
                                    }
                                }
                                if (showFixedColumnR && fixedColumnR != null)
                                {
                                    foreach (var i in fixedColumnR)
                                    {
                                        if (MouseDownRowColumn(e, it, (TCellColumn)it.cells[i], rows, e.X + sFixedR, py)) return;
                                    }
                                }
                                foreach (TCellColumn item in it.cells)
                                {
                                    if (MouseDownRowColumn(e, it, item, rows, px, py)) return;
                                }
                            }
                            return;
                        }
                    }
                    else if (it.Contains(e.X, py))
                    {
                        if (showFixedColumnL && fixedColumnL != null)
                        {
                            foreach (var i in fixedColumnL) if (MouseDownRow(e, it.cells[i], e.X, py)) return;
                        }
                        if (showFixedColumnR && fixedColumnR != null)
                        {
                            foreach (var i in fixedColumnR) if (MouseDownRow(e, it.cells[i], e.X + sFixedR, py)) return;
                        }
                        for (int i_col = 0; i_col < it.cells.Length; i_col++) if (MouseDownRow(e, it.cells[i_col], px, py)) return;
                        return;
                    }
                }
            }
        }

        bool MouseDownRowColumn(MouseEventArgs e, RowTemplate it, TCellColumn cell, RowTemplate[] rows, int x, int y)
        {
            if (cell.tag is ColumnCheck columnCheck)
            {
                if (e.Button == MouseButtons.Left && cell.Contains(x, y))
                {
                    ChangeCheckOverall(rows, it, columnCheck, !columnCheck.Checked);
                    return true;
                }
            }
            return false;
        }
        bool MouseDownRow(MouseEventArgs e, TCell cell, int x, int y)
        {
            if (cell.CONTAINS(x, y))
            {
                cell.MouseDown = true;
                if (cell is Template template && template.HasBtn && e.Button == MouseButtons.Left)
                {
                    foreach (var item in template.value)
                    {
                        if (item is TemplateButton btn_template)
                        {
                            if (btn_template.Value.Enabled)
                            {
                                if (btn_template.RECT.Contains(x, y))
                                {
                                    btn_template.ExtraMouseDown = true;
                                    return true;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        #endregion

        #region 鼠标松开

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scrollY.MouseUp(e.Location);
            scrollX.MouseUp(e.Location);
            if (rows == null) return;
            int sx = (int)scrollX.Value, sy = (int)scrollY.Value;
            int px = e.X + sx, py = e.Y + sy;
            for (int i_row = 0; i_row < rows.Length; i_row++)
            {
                var it = rows[i_row];
                if (showFixedColumnL && fixedColumnL != null)
                {
                    foreach (var i in fixedColumnL) if (MouseUpRow(it, it.cells[i], e, i_row, i, e.X, py, 0, sy)) return;
                }
                if (showFixedColumnR && fixedColumnR != null)
                {
                    foreach (var i in fixedColumnR) if (MouseUpRow(it, it.cells[i], e, i_row, i, e.X + sFixedR, py, sFixedR, sy)) return;
                }
                for (int i_col = 0; i_col < it.cells.Length; i_col++)
                {
                    var item = it.cells[i_col];
                    if (MouseUpRow(it, item, e, i_row, i_col, px, py, sx, sy)) return;
                }
            }
        }

        Input? edit_input = null;
        bool MouseUpRow(RowTemplate it, TCell cell, MouseEventArgs e, int i_row, int i_col, int x, int y, int sx, int sy)
        {
            if (cell.MouseDown)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (cell is TCellCheck checkCell)
                    {
                        if (checkCell.Contains(x, y))
                        {
                            checkCell.Checked = !checkCell.Checked;
                            cell.PROPERTY?.SetValue(cell.VALUE, checkCell.Checked);
                            CheckedChanged?.Invoke(this, checkCell.Checked, it.RECORD, i_row, i_col);
                        }
                    }
                    else if (cell is TCellRadio radioCell)
                    {
                        if (radioCell.Contains(x, y) && !radioCell.Checked)
                        {
                            if (rows != null)
                            {
                                for (int i = 0; i < rows.Length; i++)
                                {
                                    if (i != i_row)
                                    {
                                        var cell2 = rows[i].cells[i_col];
                                        if (cell2 is TCellRadio radioCell2 && radioCell2.Checked)
                                        {
                                            radioCell2.Checked = false;
                                            cell2.PROPERTY?.SetValue(cell2.VALUE, radioCell2.Checked);
                                        }
                                    }
                                }
                            }
                            radioCell.Checked = true;
                            cell.PROPERTY?.SetValue(cell.VALUE, radioCell.Checked);
                            CheckedChanged?.Invoke(this, radioCell.Checked, it.RECORD, i_row, i_col);
                        }
                    }
                    else if (cell is Template template && template.HasBtn)
                    {
                        foreach (var item in template.value)
                        {
                            if (item is TemplateButton btn)
                            {
                                if (btn.ExtraMouseDown)
                                {
                                    if (btn.RECT.Contains(x, y))
                                    {
                                        if (btn.Value is CellButton) btn.Click();
                                        CellButtonClick?.Invoke(this, btn.Value, e, it.RECORD, i_row, i_col);
                                    }
                                    btn.ExtraMouseDown = false;
                                }
                            }
                        }
                    }
                    else if (editmode == TEditMode.Click)
                    {
                        //进入编辑模式
                        EditModeClose();
                        OnEditMode(it, cell, i_row, i_col, sx, sy);
                    }
                }
                cell.MouseDown = false;
                return true;
            }
            return false;
        }

        bool inEditMode = false;
        void EditModeClose()
        {
            if (edit_input != null)
            {
                scrollX.Change = scrollY.Change = null;
                edit_input?.Dispose();
                edit_input = null;
            }
            inEditMode = false;
        }
        void OnEditMode(RowTemplate it, TCell cell, int i_row, int i_col, int sx, int sy)
        {
            if (rows != null && cell is TCellText cellText)
            {
                object? value = null;
                if (cell.PROPERTY != null && cell.VALUE != null) value = cell.PROPERTY.GetValue(cell.VALUE);
                else value = cell.VALUE;

                bool isok = true;
                if (CellBeginEdit != null) isok = CellBeginEdit(this, value, it.RECORD, i_row, i_col);
                if (!isok) return;
                inEditMode = true;

                scrollX.Change = scrollY.Change = () => EditModeClose();
                BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        rows[i].hover = i == i_row;
                    }
                    int height = cell.RECT.Height;
                    using (var bmp = new Bitmap(1, 1))
                    {
                        using (var g = Graphics.FromImage(bmp))
                        {
                            height = (int)Math.Ceiling(g.MeasureString(Config.NullText, Font).Height * 1.66F);
                        }
                    }
                    if (value is int val_int)
                    {
                        edit_input = new InputNumber
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Value = val_int
                        };
                    }
                    else if (value is double val_double)
                    {
                        edit_input = new InputNumber
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Value = new decimal(val_double)
                        };
                    }
                    else if (value is float val_float)
                    {
                        edit_input = new InputNumber
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Value = new decimal(val_float)
                        };
                    }
                    else
                    {
                        edit_input = new Input
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Text = value?.ToString()
                        };
                    }
                    edit_input.KeyPress += (a, b) =>
                    {
                        if (b.KeyChar == 13 && a is TextBox input)
                        {
                            b.Handled = true;
                            bool isok_end = true;
                            if (CellEndEdit != null) isok_end = CellEndEdit(this, input.Text, it.RECORD, i_row, i_col);
                            if (isok_end)
                            {
                                cellText.value = edit_input.Text;
                                if (cell.PROPERTY != null)
                                {
                                    if (value is int)
                                    {
                                        if (int.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                    }
                                    else if (value is double)
                                    {
                                        if (double.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                    }
                                    else if (value is float)
                                    {
                                        if (float.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                    }
                                    else cell.PROPERTY.SetValue(cell.VALUE, edit_input.Text);
                                }
                                else if (it.RECORD is DataRow datarow)
                                {
                                    if (value is int)
                                    {
                                        if (int.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                    }
                                    else if (value is double)
                                    {
                                        if (double.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                    }
                                    else if (value is float)
                                    {
                                        if (float.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                    }
                                    else datarow[i_col] = edit_input.Text;
                                }
                                EditModeClose();
                            }
                        }
                    };
                    edit_input.LostFocus += (a, b) =>
                    {
                        EditModeClose();
                    };
                    Controls.Add(edit_input);
                }));
            }
        }

        #endregion

        #region 鼠标移动

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (scrollY.MouseMove(e.Location) && scrollX.MouseMove(e.Location))
            {
                if (rows == null || inEditMode) return;
                int hand = 0;
                int px = e.X + (int)scrollX.Value, py = e.Y + (int)scrollY.Value;
                foreach (RowTemplate it in rows)
                {
                    if (it.IsColumn)
                    {
                        if (it.CONTAINS(e.X, py) || (fixedHeader && it.CONTAINS(e.X, e.Y)))
                        {
                            if (has_check)
                            {
                                if (showFixedColumnL && fixedColumnL != null)
                                {
                                    foreach (var i in fixedColumnL) MouseMoveRowColumn((TCellColumn)it.cells[i], ref hand, e.X, py);
                                }
                                if (showFixedColumnR && fixedColumnR != null)
                                {
                                    foreach (var i in fixedColumnR) MouseMoveRowColumn((TCellColumn)it.cells[i], ref hand, e.X + sFixedR, py);
                                }
                                foreach (TCellColumn item in it.cells) MouseMoveRowColumn(item, ref hand, px, py);
                            }
                            for (int i = 1; i < rows.Length; i++) rows[i].Hover = false;
                            SetCursor(hand > 0);
                            return;
                        }
                    }
                    else if (it.Contains(e.X, py))
                    {
                        var hasi = new List<int>();
                        if (showFixedColumnL && fixedColumnL != null)
                        {
                            foreach (var i in fixedColumnL)
                            {
                                hasi.Add(i);
                                MouseMoveRow(it.cells[i], ref hand, e.X, py);
                            }
                        }
                        if (showFixedColumnR && fixedColumnR != null)
                        {
                            foreach (var i in fixedColumnR)
                            {
                                hasi.Add(i);
                                MouseMoveRow(it.cells[i], ref hand, e.X + sFixedR, py);
                            }
                        }
                        for (int i = 0; i < it.cells.Length; i++)
                        {
                            if (hasi.Contains(i)) continue;
                            MouseMoveRow(it.cells[i], ref hand, px, py);
                        }
                    }
                    else
                    {
                        foreach (var cel in it.cells)
                        {
                            if (cel is Template template && template.HasBtn)
                            {
                                foreach (var item in template.value)
                                {
                                    if (item is TemplateButton btn) btn.ExtraMouseHover = false;
                                }
                            }
                        }
                    }
                }
                SetCursor(hand > 0);
            }
            else ILeave();
        }

        void MouseMoveRowColumn(TCellColumn cel, ref int hand, int x, int y)
        {
            if (cel.tag is ColumnCheck)
            {
                if (cel.Contains(x, y)) hand++;
            }
        }
        void MouseMoveRow(TCell cel, ref int hand, int x, int y)
        {
            if (cel is TCellCheck checkCell)
            {
                if (checkCell.Contains(x, y)) hand++;
            }
            else if (cel is TCellRadio radioCell)
            {
                if (radioCell.Contains(x, y)) hand++;
            }
            else if (cel is Template template && template.HasBtn)
            {
                foreach (var item in template.value)
                {
                    if (item is TemplateButton btn_template)
                    {
                        if (btn_template.Value.Enabled)
                        {
                            btn_template.ExtraMouseHover = btn_template.RECT.Contains(x, y);
                            if (btn_template.ExtraMouseHover) hand++;
                        }
                        else btn_template.ExtraMouseHover = false;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 全局复选框改动时发生
        /// </summary>
        internal void ChangeCheckOverall(RowTemplate[] rows, RowTemplate it, ColumnCheck columnCheck, bool value)
        {
            handcheck = true;
            for (int i_row = 1; i_row < rows.Length; i_row++)
            {
                for (int i_col = 0; i_col < rows[i_row].cells.Length; i_col++)
                {
                    var item = rows[i_row].cells[i_col];
                    if (item is TCellCheck checkCell)
                    {
                        if (checkCell.Checked == value) continue;
                        checkCell.Checked = value;
                        CheckedChanged?.Invoke(this, value, rows[i_row].RECORD, i_row, i_col);
                        item.PROPERTY?.SetValue(item.VALUE, checkCell.Checked);
                    }
                }
            }
            it.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
            columnCheck.SetCheckState(it.CheckState);
            handcheck = false;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scrollX.Leave();
            scrollY.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scrollX.Leave();
            scrollY.Leave();
            ILeave();
        }

        void ILeave()
        {
            SetCursor(false);
            if (rows == null || inEditMode) return;
            foreach (var it in rows)
            {
                it.Hover = false;
                foreach (var cel in it.cells)
                {
                    if (cel is Template template && template.HasBtn)
                    {
                        foreach (var item in template.value)
                        {
                            if (item is TemplateButton btn) btn.ExtraMouseHover = false;
                        }
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 选中改变事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="value">数值</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate void CheckEventHandler(object sender, bool value, object record, int rowIndex, int columnIndex);

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="args">点击</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        /// <param name="rect">表格区域</param>
        public delegate void ClickEventHandler(object sender, MouseEventArgs args, object record, int rowIndex, int columnIndex, Rectangle rect);

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="args">点击</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate void ClickButtonEventHandler(object sender, CellLink btn, MouseEventArgs args, object record, int rowIndex, int columnIndex);

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event CheckEventHandler? CheckedChanged;

        /// <summary>
        /// CheckState类型事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="column">触发表头对象</param>
        /// <param name="value">数值</param>
        public delegate void CheckStateEventHandler(object sender, ColumnCheck column, CheckState value);

        /// <summary>
        /// 全局 CheckState 属性值更改时发生
        /// </summary>
        [Description("全局 CheckState 属性值更改时发生"), Category("行为")]
        public event CheckStateEventHandler? CheckedOverallChanged = null;

        internal void OnCheckedOverallChanged(ColumnCheck column, CheckState checkState)
        {
            CheckedOverallChanged?.Invoke(this, column, checkState);
        }

        /// <summary>
        /// 单击时发生
        /// </summary>
        [Description("单击时发生"), Category("行为")]
        public event ClickEventHandler? CellClick;

        /// <summary>
        /// 单击按钮时发生
        /// </summary>
        [Description("单击按钮时发生"), Category("行为")]
        public event ClickButtonEventHandler? CellButtonClick;

        /// <summary>
        /// 双击时发生
        /// </summary>
        [Description("双击时发生"), Category("行为")]
        public event ClickEventHandler? CellDoubleClick;

        #region 编辑

        /// <summary>
        /// 编辑前事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="value">数值</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate bool BeginEditEventHandler(object sender, object? value, object record, int rowIndex, int columnIndex);

        /// <summary>
        /// 编辑后事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="value">修改后值</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate bool EndEditEventHandler(object sender, string value, object record, int rowIndex, int columnIndex);

        /// <summary>
        /// 编辑前发生
        /// </summary>
        [Description("编辑前发生"), Category("行为")]
        public event BeginEditEventHandler? CellBeginEdit;
        /// <summary>
        /// 编辑前发生
        /// </summary>
        [Description("编辑前发生"), Category("行为")]
        public event EndEditEventHandler? CellEndEdit;

        #endregion

        #endregion

        #region 列

        /// <summary>
        /// 行数据
        /// </summary>
        internal class RowTemplate
        {
            Table PARENT;
            public RowTemplate(Table table, TCell[] cell, object value)
            {
                PARENT = table;
                cells = cell;
                RECORD = value;
            }
            public RowTemplate(Table table, TCell[] cell, object value, CheckState check)
            {
                PARENT = table;
                cells = cell;
                RECORD = value;
                IsColumn = true;
                checkState = check;
                _checked = check == CheckState.Checked;
            }

            /// <summary>
            /// 内部判断脏渲染
            /// </summary>
            internal bool SHOW { get; set; }

            /// <summary>
            /// 行区域
            /// </summary>
            public Rectangle RECT { get; set; }

            /// <summary>
            /// 原始行数据
            /// </summary>
            public object RECORD { get; set; }

            /// <summary>
            /// 列数据
            /// </summary>
            public TCell[] cells { get; set; }

            /// <summary>
            /// 行高度
            /// </summary>
            public int Height { get; set; }

            internal bool IsColumn = false;

            #region 悬浮状态

            internal bool hover = false;
            /// <summary>
            /// 是否移动
            /// </summary>
            internal bool Hover
            {
                get => hover;
                set
                {
                    if (hover == value) return;
                    hover = value;
                    if (SHOW)
                    {
                        if (Config.Animation)
                        {
                            ThreadHover?.Dispose();
                            AnimationHover = true;
                            var t = Animation.TotalFrames(20, 200);
                            if (value)
                            {
                                ThreadHover = new ITask((i) =>
                                {
                                    AnimationHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    PARENT.Invalidate();
                                    return true;
                                }, 20, t, () =>
                                {
                                    AnimationHover = false;
                                    AnimationHoverValue = 1;
                                    PARENT.Invalidate();
                                });
                            }
                            else
                            {
                                ThreadHover = new ITask((i) =>
                                {
                                    AnimationHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    PARENT.Invalidate();
                                    return true;
                                }, 20, t, () =>
                                {
                                    AnimationHover = false;
                                    AnimationHoverValue = 0;
                                    PARENT.Invalidate();
                                });
                            }
                        }
                        else PARENT.Invalidate();
                    }
                }
            }

            internal bool Contains(int x, int y)
            {
                if (CONTAINS(x, y))
                {
                    Hover = true;
                    return true;
                }
                else
                {
                    Hover = false;
                    return false;
                }
            }
            internal bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }

            internal float AnimationHoverValue = 0;
            internal bool AnimationHover = false;
            ITask? ThreadHover = null;

            #endregion

            #region 选中状态(总)

            internal bool AnimationCheck = false;
            internal float AnimationCheckValue = 0;

            ITask? ThreadCheck = null;

            bool _checked = false;
            [Description("选中状态"), Category("行为"), DefaultValue(false)]
            public bool Checked
            {
                get => _checked;
                set
                {
                    if (_checked == value) return;
                    _checked = value;
                    OnCheck();
                    CheckState = value ? CheckState.Checked : CheckState.Unchecked;
                }
            }

            internal CheckState checkStateOld = CheckState.Unchecked;
            CheckState checkState = CheckState.Unchecked;
            [Description("选中状态"), Category("行为"), DefaultValue(CheckState.Unchecked)]
            public CheckState CheckState
            {
                get => checkState;
                set
                {
                    if (checkState == value) return;
                    checkState = value;
                    bool __checked = value == CheckState.Checked;
                    if (_checked != __checked)
                    {
                        _checked = __checked;
                        OnCheck();
                    }
                    if (value != CheckState.Unchecked)
                    {
                        checkStateOld = value;
                        PARENT.Invalidate();
                    }
                }
            }

            void OnCheck()
            {
                ThreadCheck?.Dispose();
                if (PARENT.IsHandleCreated)
                {
                    if (SHOW && Config.Animation)
                    {
                        AnimationCheck = true;
                        if (_checked)
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                                if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                                if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                    }
                    else
                    {
                        AnimationCheckValue = _checked ? 1F : 0F;
                        PARENT.Invalidate();
                    }
                }
            }

            #endregion
        }

        #region 单元格

        /// <summary>
        /// 复选框
        /// </summary>
        class TCellCheck : TCell
        {
            public TCellCheck(Table table, PropertyDescriptor prop, object ov, bool value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
            }

            #region 选中状态

            internal bool AnimationCheck = false;
            internal float AnimationCheckValue = 0;

            ITask? ThreadCheck = null;

            bool _checked = false;
            [Description("选中状态"), Category("行为"), DefaultValue(false)]
            public bool Checked
            {
                get => _checked;
                set
                {
                    if (_checked == value) return;
                    _checked = value;
                    OnCheck();
                }
            }

            void OnCheck()
            {
                ThreadCheck?.Dispose();
                if (ROW.SHOW && PARENT.IsHandleCreated)
                {
                    if (Config.Animation)
                    {
                        AnimationCheck = true;
                        if (_checked)
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                                if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                                if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                    }
                    else
                    {
                        AnimationCheckValue = _checked ? 1F : 0F;
                        PARENT.Invalidate();
                    }
                }
            }

            #endregion

            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
            {
            }
            internal void SetSize(Rectangle _rect, int check_size)
            {
                RECT = _rect;
                rect = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
            }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            internal bool Contains(int x, int y)
            {
                return rect.Contains(x, y);
            }

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                return g.MeasureString(Config.NullText, font);
            }

            public bool MouseDown { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// 单选框
        /// </summary>
        class TCellRadio : TCell
        {
            public TCellRadio(Table table, PropertyDescriptor prop, object ov, bool value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
            }

            #region 选中状态

            internal bool AnimationCheck = false;
            internal float AnimationCheckValue = 0;

            ITask? ThreadCheck = null;

            bool _checked = false;
            [Description("选中状态"), Category("行为"), DefaultValue(false)]
            public bool Checked
            {
                get => _checked;
                set
                {
                    if (_checked == value) return;
                    _checked = value;
                    OnCheck();
                }
            }

            void OnCheck()
            {
                ThreadCheck?.Dispose();
                if (ROW.SHOW && PARENT.IsHandleCreated)
                {
                    if (Config.Animation)
                    {
                        AnimationCheck = true;
                        if (_checked)
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                                if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                                if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                    }
                    else
                    {
                        AnimationCheckValue = _checked ? 1F : 0F;
                        PARENT.Invalidate();
                    }
                }
            }

            #endregion

            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
            {
            }
            internal void SetSize(Rectangle _rect, int check_size)
            {
                RECT = _rect;
                rect = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
            }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            internal bool Contains(int x, int y)
            {
                return rect.Contains(x, y);
            }

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                return g.MeasureString(Config.NullText, font);
            }

            public bool MouseDown { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// 普通文本
        /// </summary>
        class TCellText : TCell
        {
            public TCellText(Table table, PropertyDescriptor? prop, object ov, Column column, string? _value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                align = column.Align;
                value = _value;
            }

            /// <summary>
            /// 值
            /// </summary>
            public string? value { get; set; }
            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }
            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
            {
                RECT = _rect;
                rect = new Rectangle(_rect.X + gap, _rect.Y + gap, _rect.Width - gap2, _rect.Height - gap2);
            }
            public ColumnAlign align { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(value, font);
                return new SizeF(size.Width + gap2, size.Height);
            }

            public bool MouseDown { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// 表头
        /// </summary>
        internal class TCellColumn : TCell
        {
            public TCellColumn(Table table, Column column)
            {
                PARENT = table;
                align = column.Align;
                tag = column;
                value = column.Title;
            }

            /// <summary>
            /// 值
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public ColumnAlign align { get; set; }
            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
            {
                RECT = _rect;
            }

            public Column tag { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            internal bool Contains(int x, int y)
            {
                return rect.Contains(x, y);
            }

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(value, font);
                return new SizeF(size.Width + gap2, size.Height);
            }

            public bool MouseDown { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        internal interface TCell
        {
            PropertyDescriptor? PROPERTY { get; set; }
            object VALUE { get; set; }
            Table PARENT { get; set; }
            RowTemplate ROW { get; set; }
            Rectangle RECT { get; set; }
            void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2);
            SizeF GetSize(Graphics g, Font font, int gap, int gap2);

            bool MouseDown { get; set; }

#if NET40 || NET46 || NET48
            bool CONTAINS(int x, int y);
#else
            internal bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        #region 容器

        /// <summary>
        /// 包裹容器
        /// </summary>
        class Template : TCell
        {
            public Template(Table table, PropertyDescriptor prop, object ov, Column column, ref int processing, IList<ICell> _value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                align = column.Align;
                var list = new List<ITemplate>();
                foreach (var it in _value)
                {
                    if (it is CellBadge badge)
                    {
                        if (badge.State == TState.Processing) processing++;
                        list.Add(new TemplateBadge(badge, this));
                    }
                    else if (it is CellTag tag) list.Add(new TemplateTag(tag, this));
                    else if (it is CellImage image) list.Add(new TemplateImage(image, this));
                    else if (it is CellLink link)
                    {
                        HasBtn = true;
                        list.Add(new TemplateButton(link, this));
                    }
                    else if (it is CellText text) list.Add(new TemplateText(text, this));
                    else if (it is CellProgress progress) list.Add(new TemplateProgress(progress, this));
                }
                value = list;
            }
            /// <summary>
            /// 值
            /// </summary>
            public IList<ITemplate> value { get; set; }

            internal bool HasBtn = false;

            public void SetSize(Graphics g, Font font, Rectangle _rect, int _gap, int _gap2)
            {
                RECT = _rect;
                int gap = _gap / 2, gap2 = _gap;
                int use_x;
                switch (align)
                {
                    case ColumnAlign.Center: use_x = _rect.X + (_rect.Width - USE_Width) / 2; break;
                    case ColumnAlign.Right: use_x = _rect.Right - USE_Width; break;
                    case ColumnAlign.Left:
                    default: use_x = _rect.X; break;
                }
                for (int i = 0; i < value.Count; i++)
                {
                    var it = value[i];
                    var size = SIZES[i];
                    it.SetRect(g, font, new Rectangle(use_x, _rect.Y, size.Width, _rect.Height), size, gap, gap2);
                    use_x += size.Width;
                }
            }
            public ColumnAlign align { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            Size[] SIZES;
            int USE_Width = 0;
            public SizeF GetSize(Graphics g, Font font, int _gap, int _gap2)
            {
                int gap = _gap / 2, gap2 = _gap;
                float w = 0, h = 0;
                var sizes = new List<Size>(value.Count);
                foreach (var it in value)
                {
                    var size = it.GetSize(g, font, gap, gap2);
                    sizes.Add(size);
                    w += size.Width;
                    if (h < size.Height) h = size.Height;
                }
                USE_Width = (int)Math.Ceiling(w);
                SIZES = sizes.ToArray();
                return new SizeF(USE_Width + (value.Count > 1 ? _gap2 : _gap), h);
            }
            public bool MouseDown { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// Text 文本
        /// </summary>
        class TemplateText : ITemplate
        {
            public TemplateText(CellText value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                if (Value.Fore.HasValue)
                {
                    using (var brush = new SolidBrush(Value.Fore.Value))
                    {
                        g.DrawString(Value.Text, Value.Font == null ? font : Value.Font, brush, Rect, StringF(PARENT.align));
                    }
                }
                else g.DrawString(Value.Text, Value.Font == null ? font : Value.Font, fore, Rect, StringF(PARENT.align));
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Value.Text, Value.Font == null ? font : Value.Font);
                return new Size((int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
            }

            public CellText Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// Tag 标签
        /// </summary>
        class TemplateTag : ITemplate
        {
            public TemplateTag(CellTag value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                using (var path = Rect.RoundPath(6))
                {
                    #region 绘制背景

                    Color _fore, _back, _bor;
                    switch (Value.Type)
                    {
                        case TTypeMini.Default:
                            _back = Style.Db.TagDefaultBg;
                            _fore = Style.Db.TagDefaultColor;
                            _bor = Style.Db.DefaultBorder;
                            break;
                        case TTypeMini.Error:
                            _back = Style.Db.ErrorBg;
                            _fore = Style.Db.Error;
                            _bor = Style.Db.ErrorBorder;
                            break;
                        case TTypeMini.Success:
                            _back = Style.Db.SuccessBg;
                            _fore = Style.Db.Success;
                            _bor = Style.Db.SuccessBorder;
                            break;
                        case TTypeMini.Info:
                            _back = Style.Db.InfoBg;
                            _fore = Style.Db.Info;
                            _bor = Style.Db.InfoBorder;
                            break;
                        case TTypeMini.Warn:
                            _back = Style.Db.WarningBg;
                            _fore = Style.Db.Warning;
                            _bor = Style.Db.WarningBorder;
                            break;
                        case TTypeMini.Primary:
                        default:
                            _back = Style.Db.PrimaryBg;
                            _fore = Style.Db.Primary;
                            _bor = Style.Db.Primary;
                            break;
                    }

                    if (Value.Fore.HasValue) _fore = Value.Fore.Value;
                    if (Value.Back.HasValue) _back = Value.Back.Value;

                    using (var brush = new SolidBrush(_back))
                    {
                        g.FillPath(brush, path);
                    }

                    if (Value.BorderWidth > 0)
                    {
                        float border = Value.BorderWidth * Config.Dpi;
                        using (var brush = new Pen(_bor, border))
                        {
                            g.DrawPath(brush, path);
                        }
                    }

                    #endregion

                    using (var brush = new SolidBrush(_fore))
                    {
                        g.DrawString(Value.Text, font, brush, Rect, stringCenter);
                    }
                }
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Value.Text, font);
                return new Size((int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
            }

            public CellTag Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// Badge 徽标
        /// </summary>
        class TemplateBadge : ITemplate
        {
            public TemplateBadge(CellBadge value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                Color color;
                if (Value.Fill.HasValue) color = Value.Fill.Value;
                else
                {
                    switch (Value.State)
                    {
                        case TState.Success:
                            color = Style.Db.Success; break;
                        case TState.Error:
                            color = Style.Db.Error; break;
                        case TState.Primary:
                        case TState.Processing:
                            color = Style.Db.Primary; break;
                        case TState.Warn:
                            color = Style.Db.Warning; break;
                        default:
                            color = Style.Db.TextQuaternary; break;
                    }
                }
                using (var brush = new SolidBrush(color))
                {
                    if (Value.State == TState.Processing && PARENT.PARENT != null)
                    {
                        float max = (TxtHeight - 6F) * PARENT.PARENT.AnimationStateValue;
                        int alpha = (int)(255 * (1f - PARENT.PARENT.AnimationStateValue));
                        using (var pen = new Pen(Color.FromArgb(alpha, brush.Color), 4F))
                        {
                            g.DrawEllipse(pen, new RectangleF(RectDot.X + (RectDot.Width - max) / 2F, RectDot.Y + (RectDot.Height - max) / 2F, max, max));
                        }
                    }
                    g.FillEllipse(brush, RectDot);
                }
                if (Value.Fore.HasValue)
                {
                    using (var brush = new SolidBrush(Value.Fore.Value))
                    {
                        g.DrawString(Value.Text, font, brush, Rect, StringF(PARENT.align));
                    }
                }
                else g.DrawString(Value.Text, font, fore, Rect, StringF(PARENT.align));
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                if (Value.Text == null)
                {
                    var size = g.MeasureString(Config.NullText, font);
                    int height = (int)Math.Ceiling(size.Height);
                    return new Size(height + gap2, (int)Math.Ceiling(size.Height));
                }
                else
                {
                    var size = g.MeasureString(Value.Text, font);
                    int height = (int)Math.Ceiling(size.Height);
                    return new Size((int)Math.Ceiling(size.Width) + height + gap2, height);
                }
            }

            int TxtHeight = 0;
            RectangleF Rect;
            RectangleF RectDot;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                TxtHeight = size.Height;
                float dot_size = size.Height / 2.5F;
                if (Value.Text == null) RectDot = new RectangleF(rect.X + (rect.Width - dot_size) / 2F, rect.Y + (rect.Height - dot_size) / 2F, dot_size, dot_size);
                else
                {
                    Rect = new RectangleF(rect.X + gap + size.Height, rect.Y, rect.Width - size.Height - gap2, rect.Height);
                    switch (PARENT.align)
                    {
                        case ColumnAlign.Center:
                            var sizec = g.MeasureString(Value.Text, font);
                            RectDot = new RectangleF(rect.X + (rect.Width - sizec.Width - sizec.Height + gap2) / 2F, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                            break;
                        case ColumnAlign.Right:
                            var sizer = g.MeasureString(Value.Text, font);
                            RectDot = new RectangleF(Rect.Right - sizer.Width - gap2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                            break;
                        case ColumnAlign.Left:
                        default:
                            RectDot = new RectangleF(rect.X + gap + (size.Height - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                            break;
                    }
                }
            }

            public CellBadge Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// Image 图片
        /// </summary>
        class TemplateImage : ITemplate
        {
            public TemplateImage(CellImage value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                float radius = Value.Radius * Config.Dpi;
                using (var path = Rect.RoundPath(radius))
                {
                    using (var bmp = new Bitmap(Rect.Width, Rect.Height))
                    {
                        using (var g2 = Graphics.FromImage(bmp).High())
                        {
                            if (Value.ImageSvg != null)
                            {
                                using (var bmpsvg = Value.ImageSvg.SvgToBmp(Rect.Width, Rect.Height, Value.FillSvg))
                                {
                                    g2.PaintImg(new RectangleF(0, 0, Rect.Width, Rect.Height), bmpsvg, Value.ImageFit);
                                }
                            }
                            else g2.PaintImg(new RectangleF(0, 0, Rect.Width, Rect.Height), Value.Image, Value.ImageFit);
                        }
                        using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                        {
                            brush.TranslateTransform(Rect.X, Rect.Y);
                            if (Value.Round) g.FillEllipse(brush, Rect);
                            else
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }

                    if (Value.BorderWidth > 0 && Value.BorderColor.HasValue)
                    {
                        float border = Value.BorderWidth * Config.Dpi;
                        using (var brush = new Pen(Value.BorderColor.Value, border))
                        {
                            g.DrawPath(brush, path);
                        }
                    }
                }
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                int w = size.Width - gap2, h = size.Height - gap2;
                Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                if (Value.Size.HasValue)
                {
                    return new Size((int)Math.Ceiling(Value.Size.Value.Width * Config.Dpi) + gap2, (int)Math.Ceiling(Value.Size.Value.Height * Config.Dpi) + gap2);
                }
                else
                {
                    int size = gap2 + (int)Math.Round(g.MeasureString(Config.NullText, font).Height);
                    return new Size(size, size);
                }
            }

            public CellImage Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// Button 按钮
        /// </summary>
        class TemplateButton : ITemplate
        {
            public TemplateButton(CellLink value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                if (Value is CellButton btn) PaintButton(g, font, PARENT.PARENT == null ? 12 : PARENT.PARENT._gap, Rect, this, btn);
                else PaintLink(g, font, Rect, this, Value);
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Value.Text, font);
                if (Value is CellButton btn && btn.ShowArrow) return new Size((int)Math.Ceiling(size.Width + size.Height) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
                else return new Size((int)Math.Ceiling(size.Width) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
            }

            #region 按钮

            #region 动画

            internal ITask? ThreadHover = null;

            internal bool _mouseDown = false;
            internal bool ExtraMouseDown
            {
                get => _mouseDown;
                set
                {
                    if (_mouseDown == value) return;
                    _mouseDown = value;
                    PARENT.PARENT?.Invalidate();
                }
            }

            internal int AnimationHoverValue = 0;
            internal bool AnimationHover = false;
            internal bool _mouseHover = false;
            internal virtual bool ExtraMouseHover
            {
                get => _mouseHover;
                set
                {
                    if (_mouseHover == value) return;
                    _mouseHover = value;
                    if (PARENT.PARENT == null) return;
                    var enabled = Value.Enabled;
                    if (enabled)
                    {
                        if (Value is CellButton btn)
                        {
                            Color _back_hover;
                            switch (btn.Type)
                            {
                                case TTypeMini.Error:
                                    _back_hover = Style.Db.ErrorHover;
                                    break;
                                case TTypeMini.Success:
                                    _back_hover = Style.Db.SuccessHover;
                                    break;
                                case TTypeMini.Info:
                                    _back_hover = Style.Db.InfoHover;
                                    break;
                                case TTypeMini.Warn:
                                    _back_hover = Style.Db.WarningHover;
                                    break;
                                case TTypeMini.Primary:
                                default:
                                    _back_hover = Style.Db.PrimaryHover;
                                    break;
                            }

                            if (btn.Type == TTypeMini.Default)
                            {
                                if (btn.BorderWidth > 0) _back_hover = Style.Db.PrimaryHover;
                                else _back_hover = Style.Db.FillSecondary;
                            }

                            if (btn.BackHover.HasValue) _back_hover = btn.BackHover.Value;
                            if (Config.Animation)
                            {
                                ThreadHover?.Dispose();
                                AnimationHover = true;
                                int addvalue = _back_hover.A / 12;
                                if (value)
                                {
                                    ThreadHover = new ITask(PARENT.PARENT, () =>
                                    {
                                        AnimationHoverValue += addvalue;
                                        if (AnimationHoverValue > _back_hover.A) { AnimationHoverValue = _back_hover.A; return false; }
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, () =>
                                    {
                                        AnimationHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                                else
                                {
                                    ThreadHover = new ITask(PARENT.PARENT, () =>
                                    {
                                        AnimationHoverValue -= addvalue;
                                        if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, () =>
                                    {
                                        AnimationHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                            }
                            else AnimationHoverValue = _back_hover.A;
                            PARENT.PARENT.Invalidate();
                        }
                        else
                        {
                            int a = Style.Db.PrimaryHover.A;
                            if (Config.Animation)
                            {
                                ThreadHover?.Dispose();
                                AnimationHover = true;
                                int addvalue = a / 12;
                                if (value)
                                {
                                    ThreadHover = new ITask(PARENT.PARENT, () =>
                                    {
                                        AnimationHoverValue += addvalue;
                                        if (AnimationHoverValue > a) { AnimationHoverValue = a; return false; }
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, () =>
                                    {
                                        AnimationHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                                else
                                {
                                    ThreadHover = new ITask(PARENT.PARENT, () =>
                                    {
                                        AnimationHoverValue -= addvalue;
                                        if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, () =>
                                    {
                                        AnimationHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                            }
                            else AnimationHoverValue = a;
                            PARENT.PARENT.Invalidate();
                        }
                    }
                }
            }

            #region 点击动画

            ITask? ThreadClick = null;
            internal bool AnimationClick = false;
            internal float AnimationClickValue = 0;
            internal void Click()
            {
                if (_mouseDown && Config.Animation && PARENT.PARENT != null)
                {
                    ThreadClick?.Dispose();
                    AnimationClickValue = 0;
                    AnimationClick = true;
                    ThreadClick = new ITask(PARENT.PARENT, () =>
                    {
                        if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                        else AnimationClickValue += AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                        if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                        PARENT.PARENT.Invalidate();
                        return true;
                    }, 50, () =>
                    {
                        AnimationClick = false;
                        PARENT.PARENT.Invalidate();
                    });
                }
            }

            #endregion

            #endregion

            #endregion

            public CellLink Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        /// <summary>
        /// Progress 进度条
        /// </summary>
        class TemplateProgress : ITemplate
        {
            public TemplateProgress(CellProgress value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                Color _color = Value.Fill.HasValue ? Value.Fill.Value : Style.Db.Primary, _back = Value.Back.HasValue ? Value.Back.Value : Style.Db.FillSecondary;
                if (Value.Shape == TShape.Circle)
                {
                    float w = Value.Radius * Config.Dpi;
                    using (var brush = new Pen(_back, w))
                    {
                        g.DrawEllipse(brush, Rect);
                    }
                    if (Value.Value > 0)
                    {
                        float max = 360F * Value.Value;
                        using (var brush = new Pen(_color, w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, Rect, -90, max);
                        }
                    }
                }
                else
                {
                    float radius = Value.Radius * Config.Dpi;
                    if (Value.Shape == TShape.Round) radius = Rect.Height;

                    using (var path = Rect.RoundPath(radius))
                    {
                        using (var brush = new SolidBrush(_back))
                        {
                            g.FillPath(brush, path);
                        }
                        if (Value.Value > 0)
                        {
                            var _w = Rect.Width * Value.Value;
                            if (_w > radius)
                            {
                                using (var path_prog = new RectangleF(Rect.X, Rect.Y, _w, Rect.Height).RoundPath(radius))
                                {
                                    using (var brush = new SolidBrush(_color))
                                    {
                                        g.FillPath(brush, path_prog);
                                    }
                                }
                            }
                            else
                            {
                                using (var brush = new SolidBrush(_color))
                                {
                                    g.FillEllipse(brush, new RectangleF(Rect.X, Rect.Y, _w, Rect.Height));
                                }
                            }
                        }
                    }
                }
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                int w = size.Width - gap2, h = size.Height;
                if (Value.Shape == TShape.Circle) h = size.Height - gap2;
                Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                int height = (int)Math.Round(g.MeasureString(Config.NullText, font).Height);
                if (Value.Shape == TShape.Circle)
                {
                    int size = gap2 + height;
                    return new Size(size, size);
                }
                else
                {
                    int size = gap2 + height;
                    return new Size(size * 2, height / 2);
                }
            }

            public CellProgress Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        interface ITemplate
        {
            /// <summary>
            /// 模板父级
            /// </summary>
            Template PARENT { get; set; }

            /// <summary>
            /// 真实区域
            /// </summary>
            Rectangle RECT { get; set; }

            /// <summary>
            /// 获取大小
            /// </summary>
            /// <param name="g">GDI</param>
            /// <param name="font">字体</param>
            /// <param name="gap">边距</param>
            /// <param name="gap2">边距2</param>
            Size GetSize(Graphics g, Font font, int gap, int gap2);

            /// <summary>
            /// 设置渲染位置坐标
            /// </summary>
            /// <param name="g"></param>
            /// <param name="font">字体</param>
            /// <param name="rect">区域</param>
            /// <param name="size">真实区域</param>
            /// <param name="gap">边距</param>
            /// <param name="gap2">边距2</param>
            void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2);

            void Paint(Graphics g, TCell it, Font font, SolidBrush fore);

#if NET40 || NET46 || NET48
            bool CONTAINS(int x, int y);
#else
            internal bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        #endregion

        #endregion

        #endregion

        #region 提取数据

        TempTable? data_temp = null;
        void ExtractData()
        {
            has_notify.Clear();
            data_temp = null;
            if (dataSource == null)
            {
                scrollX.val = scrollY.val = 0;
                return;
            }
            if (dataSource is DataTable table)
            {
                if (table.Columns.Count > 0 && table.Rows.Count > 0)
                {
                    var columns = new List<TempiColumn>();
                    var rows = new List<TempiRow>();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        columns.Add(new TempiColumn(i, table.Columns[i].ColumnName));
                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        if (row != null)
                        {
                            var cells = new Dictionary<string, object?>();
                            foreach (var it in columns)
                            {
                                cells.Add(it.key, row[it.key]);
                            }
                            if (cells.Count > 0) rows.Add(new TempiRow(i, row, cells));
                        }
                    }
                    data_temp = new TempTable(columns.ToArray(), rows.ToArray());
                }
            }
            else if (dataSource is IList list)
            {
                var columns = new List<TempiColumn>();
                var rows = new List<TempiRow>();
                for (int i = 0; i < list.Count; i++)
                {
                    var row = list[i];
                    if (row != null)
                    {
                        var cells = new Dictionary<string, object?>();
                        if (columns.Count == 0)
                        {
                            int index = 0;
                            foreach (PropertyDescriptor it in TypeDescriptor.GetProperties(row))
                            {
                                columns.Add(new TempiColumn(index, it.Name)); index++;
                                cells.Add(it.Name, it);
                            }
                        }
                        else
                        {
                            foreach (PropertyDescriptor it in TypeDescriptor.GetProperties(row))
                            {
                                cells.Add(it.Name, it);
                            }
                        }
                        if (cells.Count > 0) rows.Add(new TempiRow(i, row, cells));
                    }
                }
                data_temp = new TempTable(columns.ToArray(), rows.ToArray());
            }
        }

        bool showFixedColumnL = false, showFixedColumnR = false;
        int sFixedR = 0;
        int[]? fixedColumnL = null, fixedColumnR = null;
        void ExtractHeader()
        {
            if (columns == null) return;
            var dir = new List<int>();
            for (var i = 0; i < columns.Length; i++)
            {
                if (columns[i].Fixed) dir.Add(i);
            }
            if (dir.Count > 0)
            {
                List<int> _fixedColumnL = new List<int>(), _fixedColumnR = new List<int>();
                foreach (int i in dir)
                {
                    if (i == 0) _fixedColumnL.Add(i);
                    else if (_fixedColumnL.Count > 0 && _fixedColumnL[_fixedColumnL.Count - 1] + 1 == i)
                    {
                        _fixedColumnL.Add(i);
                    }
                }
                foreach (int it in _fixedColumnL) dir.Remove(it);
                if (dir.Count > 0)
                {
                    dir.Reverse();
                    foreach (int i in dir)
                    {
                        if (i == columns.Length - 1) _fixedColumnR.Add(i);
                        else if (_fixedColumnR.Count > 0 && _fixedColumnR[_fixedColumnR.Count - 1] - 1 == i)
                        {
                            _fixedColumnR.Add(i);
                        }
                    }
                }
                if (_fixedColumnL.Count > 0) fixedColumnL = _fixedColumnL.ToArray(); else fixedColumnL = null;
                if (_fixedColumnR.Count > 0) fixedColumnR = _fixedColumnR.ToArray(); else fixedColumnR = null;
            }
            else fixedColumnL = fixedColumnR = null;
        }

        class TempTable
        {
            public TempTable(TempiColumn[] _columns, TempiRow[] _rows)
            {
                columns = _columns;
                rows = _rows;
            }
            public TempiColumn[] columns { get; set; }
            public TempiRow[] rows { get; set; }
        }
        class TempiColumn
        {
            public TempiColumn(int index, string name)
            {
                i = index;
                key = name;
            }
            /// <summary>
            /// 表头名称
            /// </summary>
            public string key { get; set; }

            /// <summary>
            /// 列序号
            /// </summary>
            public int i { get; set; }
        }
        class TempiRow
        {
            public TempiRow(int index, object _record, Dictionary<string, object?> _cells)
            {
                i = index;
                record = _record;
                cells = _cells;
            }
            /// <summary>
            /// 行序号
            /// </summary>
            public int i { get; set; }

            /// <summary>
            /// 行原始数据
            /// </summary>
            public object record { get; set; }

            public Dictionary<string, object?> cells { get; set; }
        }

        #endregion
    }

    #region 表头

    /// <summary>
    /// 复选框表头
    /// </summary>
    public class ColumnCheck : Column
    {
        public ColumnCheck(string key) : base(key, "")
        {
        }

        /// <summary>
        /// 选中状态
        /// </summary>
        public CheckState CheckState { get; internal set; }

        /// <summary>
        /// 选中状态
        /// </summary>
        public bool Checked
        {
            get => CheckState == CheckState.Checked;
            set
            {
                if (PARENT == null || PARENT.rows == null) return;
                foreach (var it in PARENT.rows)
                {
                    if (it.IsColumn)
                    {
                        foreach (Table.TCellColumn item in PARENT.rows[0].cells)
                        {
                            if (item.tag is ColumnCheck columnCheck)
                            {
                                PARENT?.ChangeCheckOverall(PARENT.rows, it, columnCheck, value);
                                return;
                            }
                        }
                        return;
                    }
                }
            }
        }
        internal Table? PARENT { get; set; }
        internal void SetCheckState(CheckState checkState)
        {
            if (CheckState == checkState) return;
            CheckState = checkState;
            PARENT?.OnCheckedOverallChanged(this, checkState);
        }
    }

    /// <summary>
    /// 单选框表头
    /// </summary>
    public class ColumnRadio : Column
    {
        /// <summary>
        /// 单选框表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public ColumnRadio(string key, string title) : base(key, title)
        {
        }
    }

    /// <summary>
    /// 表头
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public Column(string key, string title)
        {
            Key = key;
            Title = title;
        }
        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        /// <param name="align">对齐方式</param>
        public Column(string key, string title, ColumnAlign align)
        {
            Key = key;
            Title = title;
            Align = align;
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 显示文字
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 对齐方式
        /// </summary>
        public ColumnAlign Align { get; set; } = ColumnAlign.Left;

        /// <summary>
        /// 列宽度
        /// </summary>
        public string? Width { get; set; }

        /// <summary>
        /// 超过宽度将自动省略
        /// </summary>
        public bool Ellipsis { get; set; }

        /// <summary>
        /// 列是否固定
        /// </summary>
        public bool Fixed { get; set; }
    }

    #endregion

    #region 丰富列

    /// <summary>
    /// 文字
    /// </summary>
    public class CellText : ICell
    {
        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="text">文本</param>
        public CellText(string text) { _text = text; }

        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="fore">文字颜色</param>
        public CellText(string text, Color fore)
        {
            _text = text;
            _fore = fore;
        }

        Color? _fore;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color? Fore
        {
            get => _fore;
            set
            {
                if (_fore == value) return;
                _fore = value;
                OnPropertyChanged("Fore");
            }
        }

        Font? _font;
        /// <summary>
        /// 字体
        /// </summary>
        public Font? Font
        {
            get => _font;
            set
            {
                if (_font == value) return;
                _font = value;
                OnPropertyChanged("Font");
            }
        }

        string? _text;
        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged("Text");
            }
        }
    }

    /// <summary>
    /// 徽标
    /// </summary>
    public class CellBadge : ICell
    {
        /// <summary>
        /// 徽标
        /// </summary>
        /// <param name="text">文本</param>
        public CellBadge(string text) { _text = text; }

        /// <summary>
        /// 徽标
        /// </summary>
        /// <param name="state">状态</param>
        public CellBadge(TState state) { _state = state; }

        /// <summary>
        /// 徽标
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="text">文本</param>
        public CellBadge(TState state, string text)
        {
            _state = state;
            _text = text;
        }

        Color? fore;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                OnPropertyChanged("Fore");
            }
        }

        Color? fill = null;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                OnPropertyChanged("Fill");
            }
        }

        TState _state = TState.Default;
        /// <summary>
        /// 状态
        /// </summary>
        public TState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                OnPropertyChanged("State");
            }
        }

        string? _text;
        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged("Text");
            }
        }
    }

    /// <summary>
    /// 标签
    /// </summary>
    public class CellTag : ICell
    {
        /// <summary>
        /// 标签
        /// </summary>
        /// <param name="text">文本</param>
        public CellTag(string text) { _text = text; }

        /// <summary>
        /// 标签
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="type">类型</param>
        public CellTag(string text, TTypeMini type)
        {
            _text = text;
            _type = type;
        }

        Color? fore;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                OnPropertyChanged("Fore");
            }
        }

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                OnPropertyChanged("Back");
            }
        }

        float borderwidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth
        {
            get => borderwidth;
            set
            {
                if (borderwidth == value) return;
                borderwidth = value;
                OnPropertyChanged("BorderWidth");
            }
        }

        TTypeMini _type = TTypeMini.Default;
        /// <summary>
        /// 类型
        /// </summary>
        public TTypeMini Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        string _text;
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged("Text");
            }
        }
    }

    /// <summary>
    /// 图片
    /// </summary>
    public class CellImage : ICell
    {
        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="img">图片</param>
        public CellImage(Bitmap img) { image = img; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="svg">SVG</param>
        public CellImage(string svg) { imageSvg = svg; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="svg">SVG</param>
        /// <param name="svgcolor">填充颜色</param>
        public CellImage(string svg, Color svgcolor) { imageSvg = svg; fillSvg = svgcolor; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="_radius">圆角</param>
        public CellImage(Bitmap img, int _radius) { image = img; radius = _radius; }

        #region 边框

        Color? bordercolor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color? BorderColor
        {
            get => bordercolor;
            set
            {
                if (bordercolor == value) return;
                bordercolor = value;
                if (borderwidth > 0) OnPropertyChanged("BorderColor");
            }
        }

        float borderwidth = 0F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth
        {
            get => borderwidth;
            set
            {
                if (borderwidth == value) return;
                borderwidth = value;
                OnPropertyChanged("BorderWidth");
            }
        }

        #endregion

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                OnPropertyChanged("Radius");
            }
        }

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                OnPropertyChanged("Round");
            }
        }

        /// <summary>
        /// 自定义大小
        /// </summary>
        public Size? Size { get; set; } = null;

        TFit imageFit = TFit.Cover;
        /// <summary>
        /// 图片布局
        /// </summary>
        public TFit ImageFit
        {
            get => imageFit;
            set
            {
                if (imageFit == value) return;
                imageFit = value;
                OnPropertyChanged("ImageFit");
            }
        }

        Bitmap image;
        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                OnPropertyChanged("Image");
            }
        }

        string? imageSvg = null;
        /// <summary>
        /// 图片SVG
        /// </summary>
        public string? ImageSvg
        {
            get => imageSvg;
            set
            {
                if (imageSvg == value) return;
                imageSvg = value;
                OnPropertyChanged("ImageSvg");
            }
        }

        Color? fillSvg;
        /// <summary>
        /// SVG填充颜色
        /// </summary>
        public Color? FillSvg
        {
            get => fillSvg;
            set
            {
                if (fillSvg == value) fillSvg = value;
                fillSvg = value;
                OnPropertyChanged("FillSvg");
            }
        }
    }

    /// <summary>
    /// 按钮
    /// </summary>
    public class CellButton : CellLink
    {
        /// <summary>
        /// 按钮
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">文本</param>
        public CellButton(string id, string text) : base(id, text) { }

        /// <summary>
        /// 按钮
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">文本</param>
        /// <param name="_type">类型</param>
        public CellButton(string id, string text, TTypeMini _type) : base(id, text) { type = _type; }

        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                OnPropertyChanged("Fore");
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                OnPropertyChanged("Back");
            }
        }

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        public Color? BackActive { get; set; }

        #endregion

        #region 边框

        internal float borderWidth = 0;
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                OnPropertyChanged("BorderWidth");
            }
        }

        #endregion

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                OnPropertyChanged("Radius");
            }
        }

        TShape shape = TShape.Default;
        /// <summary>
        /// 形状
        /// </summary>
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                OnPropertyChanged("Shape");
            }
        }

        TTypeMini type = TTypeMini.Default;
        /// <summary>
        /// 类型
        /// </summary>
        public TTypeMini Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged("Type");
            }
        }

        bool ghost = false;
        /// <summary>
        /// 幽灵属性，使按钮背景透明
        /// </summary>
        public bool Ghost
        {
            get => ghost;
            set
            {
                if (ghost == value) return;
                ghost = value;
                OnPropertyChanged("Ghost");
            }
        }

        internal float ArrowProg = -1F;
        bool showArrow = false;
        /// <summary>
        /// 下拉框箭头是否显示
        /// </summary>
        public bool ShowArrow
        {
            get => showArrow;
            set
            {
                if (showArrow == value) return;
                showArrow = value;
                OnPropertyChanged("ShowArrow");
            }
        }

        bool isLink = false;
        /// <summary>
        /// 下拉框箭头是否链接样式
        /// </summary>
        public bool IsLink
        {
            get => isLink;
            set
            {
                if (isLink == value) return;
                isLink = value;
                OnPropertyChanged("IsLink");
            }
        }

        #endregion
    }

    /// <summary>
    /// 超链接
    /// </summary>
    public class CellLink : ICell
    {
        /// <summary>
        /// 超链接
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">文本</param>
        public CellLink(string id, string text) { Id = id; _text = text; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        #region 文本

        string? _text = null;
        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        internal StringFormat stringFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap };

        ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 文本位置
        /// </summary>
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
                OnPropertyChanged("TextAlign");
            }
        }

        #endregion

        bool enabled = true;
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) enabled = value;
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
    }

    /// <summary>
    /// 进度条
    /// </summary>
    public class CellProgress : ICell
    {
        /// <summary>
        /// 进度条
        /// </summary>
        /// <param name="value">进度</param>
        public CellProgress(float value)
        {
            _value = value;
        }

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                OnPropertyChanged("Back");
            }
        }

        Color? fill;
        /// <summary>
        /// 进度条颜色
        /// </summary>
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                OnPropertyChanged("Fill");
            }
        }

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                OnPropertyChanged("Radius");
            }
        }

        TShape shape = TShape.Round;
        /// <summary>
        /// 形状
        /// </summary>
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                OnPropertyChanged("Shape");
            }
        }

        float _value = 0F;
        /// <summary>
        /// 进度条
        /// </summary>
        [Description("进度条 0.0-1.0"), Category("数据"), DefaultValue(0F)]
        public float Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                OnPropertyChanged("Icon");
            }
        }
    }

    public class ICell : NotifyProperty { }

    public class NotifyProperty
    {
        internal Action<string>? Changed { get; set; }
        public void OnPropertyChanged(string propertyName)
        {
            Changed?.Invoke(propertyName);
        }
    }

    #endregion

    /// <summary>
    /// 列的对齐方式
    /// </summary>
    public enum ColumnAlign
    {
        Left,
        Right,
        Center
    }
}