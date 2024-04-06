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
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormSelectMultiple : ILayeredFormOpacityDown
    {
        Control textBox;
        int MaxCount = 4, MaxChoiceCount = 4;
        internal float Radius = 0;
        internal List<object> selectedValue;
        int r_w = 0;
        readonly List<ObjectItem> Items = new List<ObjectItem>();
        public LayeredFormSelectMultiple(SelectMultiple control, RectangleF rect_read, List<object> items)
        {
            var form = control.Parent.FindPARENT();
            if (form != null) TopMost = form.TopMost;
            PARENT = control;
            scrollY = new ScrollY(this);
            textBox = control;
            MaxCount = control.MaxCount;
            MaxChoiceCount = control.MaxChoiceCount;
            Font = control.Font;
            selectedValue = new List<object>(control.SelectedValue.Length);
            selectedValue.AddRange(control.SelectedValue);
            Radius = (int)(control.radius * Config.Dpi);
            Init(control, control.Placement, control.DropDownArrow, control.ListAutoWidth, rect_read, items);
        }

        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        void Init(SelectMultiple control, TAlignFrom Placement, bool ShowArrow, bool ListAutoWidth, RectangleF rect_read, List<object> items)
        {
            int y = 10, w = (int)rect_read.Width;
            r_w = w;

            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font).Size(2);
                int gap_y = (int)Math.Ceiling(size.Height * 0.227F), gap_x = (int)Math.Ceiling(size.Height * 0.54F);
                int font_size = size.Height + gap_y * 2;
                var y2 = gap_y * 2;
                y += gap_y;

                if (ListAutoWidth)
                {
                    string btext = "";
                    bool ui_online = false, ui_icon = false, ui_arrow = false;
                    foreach (var obj in items)
                    {
                        if (obj is SelectItem it)
                        {
                            string text = it.Text;
                            if (text.Length > btext.Length) btext = text;
                            if (it.Online > -1) ui_online = true;
                            if (it.Icon != null) ui_icon = true;
                            if (it.Sub != null && it.Sub.Count > 0) ui_arrow = true;
                        }
                        else if (obj is DividerSelectItem)
                        {
                        }
                        else
                        {
                            string? text = obj.ToString();
                            if (text != null) if (text.Length > btext.Length) btext = text;
                        }
                    }
                    var size3 = g.MeasureString(btext, Font);
                    int b_w = (int)Math.Ceiling(size3.Width) + 42;
                    if (ui_icon && ui_online) b_w += font_size * 2;
                    else if (ui_icon || ui_online) b_w += font_size;
                    if (ui_arrow) b_w += (int)Math.Ceiling(font_size * 0.6F) * 2;
                    else b_w += (int)Math.Ceiling(font_size * 0.6F);
                }
                else
                {
                    stringFormatLeft.Trimming = StringTrimming.EllipsisCharacter; stringFormatLeft.FormatFlags = StringFormatFlags.NoWrap;
                }

                int selY = -1;
                int item_count = 0, divider_count = 0;
                int text_height = font_size - y2;
                float gap = (text_height - gap_y) / 2F;
                for (int i = 0; i < items.Count; i++)
                {
                    var obj = items[i];
                    if (obj is SelectItem it)
                    {
                        item_count++;
                        RectangleF rect_bg = new RectangleF(10 + gap_y, y, w - y2, font_size),
                            rect_text = new RectangleF(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, text_height);
                        Items.Add(new ObjectItem(it, i, rect_bg, gap_y, gap, rect_text));
                        if (selY == -1 && selectedValue.Contains(it.Tag)) selY = y;
                    }
                    else if (obj is DividerSelectItem)
                    {
                        divider_count++;
                        Items.Add(new ObjectItem(new RectangleF(10 + gap_y, y + (gap_y - 1F) / 2F, w - y2, 1)));
                        y += gap_y;
                        continue;
                    }
                    else
                    {
                        item_count++;
                        RectangleF rect_bg = new RectangleF(10 + gap_y, y, w - y2, font_size),
                            rect_text = new RectangleF(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, text_height);
                        Items.Add(new ObjectItem(obj, i, rect_bg, rect_text));
                        if (selY == -1 && selectedValue.Contains(obj)) selY = y;
                    }
                    y += font_size;
                }
                var vr = (font_size * item_count) + (gap_y * divider_count);
                if (Items.Count > MaxCount)
                {
                    y = 10 + gap_y * 2 + (font_size * MaxCount);
                    scrollY.Rect = new Rectangle(w - gap_y, 10 + gap_y, 20, (font_size * MaxCount));
                    scrollY.Show = true;
                    scrollY.SetVrSize(vr, scrollY.Rect.Height);
                    if (selY > -1) scrollY.val = scrollY.SetValue(selY - 10 - gap_y);
                }
                else y = 10 + gap_y * 2 + vr;
            });

            SetSizeW(w + 20);
            EndHeight = y + 10;
            var point = control.PointToScreen(Point.Empty);
            switch (Placement)
            {
                case TAlignFrom.Top:
                    Inverted = true;
                    if (ShowArrow)
                    {
                        ArrowAlign = TAlign.Top;
                        SetLocation(point.X + (control.Width - (r_w + 20)) / 2, point.Y - EndHeight + 10 - ArrowSize);
                    }
                    else SetLocation(point.X + (control.Width - (r_w + 20)) / 2, point.Y - EndHeight + 10);
                    break;
                case TAlignFrom.TL:
                    Inverted = true;
                    if (ShowArrow)
                    {
                        ArrowAlign = TAlign.TL;
                        SetLocation(point.X + (int)rect_read.X - 10, point.Y - EndHeight + 10 - ArrowSize);
                    }
                    else SetLocation(point.X + (int)rect_read.X - 10, point.Y - EndHeight + 10);
                    break;
                case TAlignFrom.TR:
                    Inverted = true;
                    if (ShowArrow)
                    {
                        ArrowAlign = TAlign.TR;
                        SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - r_w - 10, point.Y - EndHeight + 10 - ArrowSize);
                    }
                    else SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - r_w - 10, point.Y - EndHeight + 10);
                    break;
                case TAlignFrom.Bottom:
                    if (ShowArrow)
                    {
                        ArrowAlign = TAlign.Bottom;
                        SetLocation(point.X + (control.Width - (r_w + 20)) / 2, point.Y + control.Height - 10 + ArrowSize);
                    }
                    else SetLocation(point.X + (control.Width - (r_w + 20)) / 2, point.Y + control.Height - 10);
                    break;
                case TAlignFrom.BR:
                    if (ShowArrow)
                    {
                        ArrowAlign = TAlign.BR;
                        SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - r_w - 10, point.Y + control.Height - 10 + ArrowSize);
                    }
                    else SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - r_w - 10, point.Y + control.Height - 10);
                    break;
                case TAlignFrom.BL:
                default:
                    if (ShowArrow)
                    {
                        ArrowAlign = TAlign.BL;
                        SetLocation(point.X + (int)rect_read.X - 10, point.Y + control.Height - 10 + ArrowSize);
                    }
                    else SetLocation(point.X + (int)rect_read.X - 10, point.Y + control.Height - 10);
                    break;

            }

            KeyCall = keys =>
            {
                if (keys == Keys.Escape)
                {
                    Dispose(); return true;
                }
                if (keys == Keys.Enter)
                {
                    if (hoveindex > -1)
                    {
                        var it = Items[hoveindex];
                        if (it.ID != -1) { OnClick(it); return true; }
                    }
                }
                else if (keys == Keys.Up)
                {
                    hoveindex--;
                    if (hoveindex < 0) hoveindex = Items.Count - 1;
                    while (Items[hoveindex].ShowAndID)
                    {
                        hoveindex--;
                        if (hoveindex < 0) hoveindex = Items.Count - 1;
                    }
                    foreach (var it in Items) it.Hover = false;
                    FocusItem(Items[hoveindex]);
                    return true;
                }
                else if (keys == Keys.Down)
                {
                    if (hoveindex == -1) hoveindex = 0;
                    else
                    {
                        hoveindex++;
                        if (hoveindex > Items.Count - 1) hoveindex = 0;
                        while (Items[hoveindex].ShowAndID)
                        {
                            hoveindex++;
                            if (hoveindex > Items.Count - 1) hoveindex = 0;
                        }
                    }
                    foreach (var it in Items) it.Hover = false;
                    FocusItem(Items[hoveindex]);
                    return true;
                }
                return false;
            };
        }

        StringFormat stringFormatLeft = Helper.SF(lr: StringAlignment.Near);
        public void FocusItem(ObjectItem item)
        {
            if (item.SetHover(true))
            {
                scrollY.Value = item.Rect.Y - item.Rect.Height;
                Print();
            }
        }

        internal bool tag1 = true;

        #region 筛选

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            textBox.TextChanged += TextBox_TextChanged;
        }

        protected override void Dispose(bool disposing)
        {
            textBox.TextChanged -= TextBox_TextChanged;
            base.Dispose(disposing);
        }

        void TextBox_TextChanged(object? sender, EventArgs e)
        {
            int count = 0;
            var val = textBox.Text;
            if (string.IsNullOrEmpty(val))
            {
                nodata = false;
                foreach (var it in Items)
                {
                    if (!it.Show)
                    {
                        it.Show = true;
                        count++;
                    }
                }
            }
            else
            {
                val = val.ToLower();
                int showcount = 0;
                foreach (var it in Items)
                {
                    if (it.ID > -1)
                    {
                        if (it.Contains(val))
                        {
                            showcount++;
                            if (!it.Show)
                            {
                                it.Show = true;
                                count++;
                            }
                        }
                        else
                        {
                            if (it.Show)
                            {
                                it.Show = false;
                                count++;
                            }
                        }
                    }
                }
                nodata = showcount == 0;
            }
            if (count > 0)
            {
                if (nodata) SetSizeH(80);
                else
                {
                    scrollY.val = 0;
                    int y = 10, w = r_w, list_count = 0;
                    Helper.GDI(g =>
                    {
                        var size = g.MeasureString(Config.NullText, Font).Size(2);
                        int gap_y = (int)Math.Ceiling(size.Height * 0.227F), gap_x = (int)Math.Ceiling(size.Height * 0.54F);
                        int font_size = size.Height + gap_y * 2;
                        var y2 = gap_y * 2;
                        y += gap_y;

                        int text_height = font_size - y2;
                        float gap = (text_height - gap_y) / 2F;
                        foreach (var it in Items)
                        {
                            if (it.Show)
                            {
                                list_count++;
                                var rect_bg = new RectangleF(10 + gap_y, y, w - y2, font_size);
                                it.SetRect(rect_bg, new RectangleF(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, rect_bg.Height - y2), gap, gap_y);
                                y += font_size;
                            }
                        }

                        var vr = font_size * list_count;
                        if (list_count > MaxCount)
                        {
                            y = 10 + gap_y * 2 + (font_size * MaxCount);
                            scrollY.Rect = new Rectangle(w - gap_y, 10 + gap_y, 20, (font_size * MaxCount));
                            scrollY.Show = true;
                            scrollY.SetVrSize(vr, scrollY.Rect.Height);
                        }
                        else
                        {
                            y = 10 + gap_y * 2 + vr;
                            scrollY.Show = false;
                        }
                        SetSizeH(y + 10);
                    });
                }
                Print();
            }
        }

        #endregion

        /// <summary>
        /// 是否显示暂无数据
        /// </summary>
        bool nodata = false;

        internal ScrollY scrollY;

        #region 鼠标

        int hoveindex = -1;
        bool down = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (scrollY.MouseDown(e.Location)) down = true;
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            scrollY.MouseUp(e.Location);
            if (down)
            {
                foreach (var it in Items)
                {
                    if (it.Show && it.ID > -1 && it.Contains(e.Location, 0, scrollY.Value, out _))
                    {
                        OnClick(it); return;
                    }
                }
            }
            down = false;
            base.OnMouseUp(e);
        }
        void OnClick(ObjectItem it)
        {
            if (selectedValue.Contains(it.Val)) selectedValue.Remove(it.Val);
            else
            {
                if (MaxChoiceCount > 0 && selectedValue.Count >= MaxChoiceCount) return;
                selectedValue.Add(it.Val);
            }
            if (PARENT is SelectMultiple select) select.SelectedValue = selectedValue.ToArray();
            down = false;
            Print();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            hoveindex = -1;
            if (scrollY.MouseMove(e.Location))
            {
                int count = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    if (it.Contains(e.Location, 0, scrollY.Value, out var change)) hoveindex = i;
                    if (change) count++;
                }
                if (count > 0) Print();
            }
            base.OnMouseMove(e);
        }

        #endregion

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path = rect_read.RoundPath(Radius))
                {
                    DrawShadow(g, rect, rect.Width, EndHeight);
                    using (var brush = new SolidBrush(Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path);
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                    }
                    if (nodata)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(180, Style.Db.Text)))
                        { g.DrawString("暂无数据", Font, brush, rect_read, Helper.stringFormatCenter2); }
                    }
                    else
                    {
                        g.SetClip(path);
                        g.TranslateTransform(0, -scrollY.Value);
                        using (var brush = new SolidBrush(Style.Db.Text))
                        {
                            if (Radius > 0)
                            {
                                int oldsel = -1;
                                for (int i = 0; i < Items.Count; i++)
                                {
                                    var it = Items[i];
                                    if (it != null && it.Show)
                                    {
                                        //判断下一个是不是连续的
                                        if (selectedValue.Contains(it.Val) || it.Val is SelectItem item && selectedValue.Contains(item.Tag))
                                        {
                                            bool isn = IFNextSelect(i + 1);
                                            if (oldsel == -1)
                                            {
                                                if (isn)
                                                {
                                                    oldsel = i;
                                                    DrawItemSelect(g, it, true, true, false, false);
                                                }
                                                else DrawItemSelect(g, it, true, true, true, true);
                                            }
                                            else
                                            {
                                                if (isn) DrawItemSelect(g, it, false, false, false, false);
                                                else DrawItemSelect(g, it, false, false, true, true);
                                            }
                                        }
                                        else
                                        {
                                            DrawItem(g, brush, it);
                                            oldsel = -1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var it in Items)
                                {
                                    if (it.Show) DrawItemR(g, brush, it);
                                }
                            }
                        }
                        g.ResetTransform();
                        g.ResetClip();
                        scrollY.Paint(g);
                    }
                }
            }
            return original_bmp;
        }
        bool IFNextSelect(int start)
        {
            for (int i = start; i < Items.Count; i++)
            {
                var it = Items[i];
                if (it != null && it.Show)
                {
                    if (selectedValue.Contains(it.Val) || it.Val is SelectItem item && selectedValue.Contains(item.Tag)) return true;
                    else return false;
                }
            }
            return false;
        }
        void DrawItemSelect(Graphics g, ObjectItem it, bool TL, bool TR, bool BR, bool BL)
        {
            if (it.ID == -1)
            {
                using (var brush_back = new SolidBrush(Style.Db.Split))
                {
                    g.FillRectangle(brush_back, it.Rect);
                }
            }
            else
            {
                using (var brush_back = new SolidBrush(Style.Db.PrimaryBg))
                {
                    using (var path = it.Rect.RoundPath(Radius, TL, TR, BR, BL))
                    {
                        g.FillPath(brush_back, path);
                    }
                }
                using (var brush_select = new SolidBrush(Style.Db.TextBase))
                {
                    g.DrawString(it.Text, Font, brush_select, it.RectText, stringFormatLeft);
                }
                g.PaintIconComplete(new RectangleF(it.Rect.Right - it.Rect.Height, it.Rect.Y, it.Rect.Height, it.Rect.Height), Style.Db.Primary);

                if (it.Online.HasValue)
                {
                    using (var brush_online = new SolidBrush(it.Online == 1 ? Style.Db.Success : Style.Db.Error))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
                if (it.Icon != null) g.DrawImage(it.Icon, it.RectIcon);
                if (it.has_sub) PanintArrow(g, it, Style.Db.TextBase);
            }
        }

        void DrawItem(Graphics g, SolidBrush brush, ObjectItem it)
        {
            if (it.ID == -1)
            {
                using (var brush_back = new SolidBrush(Style.Db.Split))
                {
                    g.FillRectangle(brush_back, it.Rect);
                }
            }
            else
            {
                if (MaxChoiceCount > 0 && selectedValue.Count >= MaxChoiceCount)
                {
                    using (var brush_fore = new SolidBrush(Style.Db.TextQuaternary))
                    {
                        g.DrawString(it.Text, Font, brush_fore, it.RectText, stringFormatLeft);
                    }
                }
                else
                {
                    if (it.Hover)
                    {
                        using (var brush_back = new SolidBrush(Style.Db.FillTertiary))
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.FillPath(brush_back, path);
                            }
                        }
                    }
                    g.DrawString(it.Text, Font, brush, it.RectText, stringFormatLeft);
                }
                if (it.Online.HasValue)
                {
                    using (var brush_online = new SolidBrush(it.Online == 1 ? Style.Db.Success : Style.Db.Error))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
                if (it.Icon != null) g.DrawImage(it.Icon, it.RectIcon);
                if (it.has_sub) PanintArrow(g, it, Style.Db.TextBase);
            }
        }
        void DrawItemR(Graphics g, SolidBrush brush, ObjectItem it)
        {
            if (it.ID == -1)
            {
                using (var brush_back = new SolidBrush(Style.Db.Split))
                {
                    g.FillRectangle(brush_back, it.Rect);
                }
            }
            else if (selectedValue.Contains(it.Val) || it.Val is SelectItem item && selectedValue.Contains(item.Tag))
            {
                using (var brush_back = new SolidBrush(Style.Db.PrimaryBg))
                {
                    g.FillRectangle(brush_back, it.Rect);
                }
                using (var brush_select = new SolidBrush(Style.Db.TextBase))
                {
                    g.DrawString(it.Text, Font, brush_select, it.RectText, stringFormatLeft);
                }
                g.PaintIconComplete(new RectangleF(it.Rect.Right - it.Rect.Height, it.Rect.Y, it.Rect.Height, it.Rect.Height), Style.Db.Primary);
            }
            else
            {
                if (it.Hover)
                {
                    using (var brush_back = new SolidBrush(Style.Db.FillTertiary))
                    {
                        g.FillRectangle(brush_back, it.Rect);
                    }
                }
                g.DrawString(it.Text, Font, brush, it.RectText, stringFormatLeft);
            }
            if (it.Online.HasValue)
            {
                using (var brush_online = new SolidBrush(it.Online == 1 ? Style.Db.Success : Style.Db.Error))
                {
                    g.FillEllipse(brush_online, it.RectOnline);
                }
            }
            if (it.Icon != null) g.DrawImage(it.Icon, it.RectIcon);
            if (it.has_sub) PanintArrow(g, it, Style.Db.TextBase);
        }
        void PanintArrow(Graphics g, ObjectItem item, Color color)
        {
            float size = item.arr_rect.Width, size2 = size / 2F;
            g.TranslateTransform(item.arr_rect.X + size2, item.arr_rect.Y + size2);
            g.RotateTransform(-90F);
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new RectangleF(-size2, -size2, item.arr_rect.Width, item.arr_rect.Height).TriangleLines(-1, .2F));
            }
            g.ResetTransform();
            g.TranslateTransform(0, -scrollY.Value);
        }

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="shadow_width">最终阴影宽度</param>
        /// <param name="shadow_height">最终阴影高度</param>
        void DrawShadow(Graphics g, Rectangle rect_client, int shadow_width, int shadow_height)
        {
            if (shadow_temp == null || (shadow_temp.Width != shadow_width || shadow_temp.Height != shadow_height))
            {
                shadow_temp?.Dispose();
                using (var path = new Rectangle(10, 10, shadow_width - 20, shadow_height - 20).RoundPath(Radius))
                {
                    shadow_temp = path.PaintShadow(shadow_width, shadow_height);
                }
            }
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix { Matrix33 = 0.2F };
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(shadow_temp, rect_client, 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        #region 滚动条

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion
    }
}