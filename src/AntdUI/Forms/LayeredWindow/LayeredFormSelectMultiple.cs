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
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormSelectMultiple : ILayeredFormOpacityDown
    {
        int MaxCount = 4, MaxChoiceCount = 4;
        internal float Radius = 0;
        internal List<object> selectedValue;
        int r_w = 0;
        List<ObjectItem> Items;
        public LayeredFormSelectMultiple(SelectMultiple control, Rectangle rect_read, List<object> items, string filtertext)
        {
            control.Parent.SetTopMost(Handle);
            PARENT = control;
            scrollY = new ScrollY(this);
            MaxCount = control.MaxCount;
            MaxChoiceCount = control.MaxChoiceCount;
            Font = control.Font;
            selectedValue = new List<object>(control.SelectedValue.Length);
            selectedValue.AddRange(control.SelectedValue);
            Radius = (int)(control.radius * Config.Dpi);
            Items = new List<ObjectItem>(items.Count);
            Init(control, control.Placement, control.DropDownArrow, control.ListAutoWidth, rect_read, items, filtertext);
        }

        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        void Init(SelectMultiple control, TAlignFrom Placement, bool ShowArrow, bool ListAutoWidth, Rectangle rect_read, List<object> items, string? filtertext = null)
        {
            int y = 10, w = rect_read.Width;
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
                    foreach (var obj in items) InitReadList(obj, ref btext, ref ui_online, ref ui_icon, ref ui_arrow);
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
                int text_height = font_size - y2, gap = (text_height - gap_y) / 2;
                for (int i = 0; i < items.Count; i++) ReadList(items[i], i, w, y2, gap_x, gap_y, gap, font_size, text_height, ref item_count, ref divider_count, ref y, ref selY);
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
            if (filtertext == null || string.IsNullOrEmpty(filtertext)) EndHeight = y + 10;
            else EndHeight = TextChangeCore(filtertext);
            var point = control.PointToScreen(Point.Empty);
            MyPoint(point, control, EndHeight, Placement, ShowArrow, rect_read);

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

        void MyPoint(Point point, Control control, int height, TAlignFrom Placement, bool ShowArrow, Rectangle rect_read) => CLocation(control, point, Placement, ShowArrow, ArrowSize, r_w, height, rect_read, ref Inverted, ref ArrowAlign);

        void ReadList(object obj, int i, int w, int y2, int gap_x, int gap_y, int gap, int font_size, int text_height, ref int item_count, ref int divider_count, ref int y, ref int selY, bool NoIndex = true)
        {
            if (obj is SelectItem it)
            {
                item_count++;
                Rectangle rect_bg = new Rectangle(10 + gap_y, y, w - y2, font_size), rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, text_height);
                Items.Add(new ObjectItem(it, i, rect_bg, gap_y, gap, rect_text) { NoIndex = NoIndex });
                if (selectedValue == it.Tag) selY = y;
                y += font_size;
            }
            else if (obj is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
            {
                item_count++;
                Rectangle rect_bg = new Rectangle(10 + gap_y, y, w - y2, font_size), rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, text_height);
                Items.Add(new ObjectItem(group, i, rect_bg, rect_text));
                if (selectedValue == obj) selY = y;
                y += font_size;
                foreach (var item in group.Sub) ReadList(item, i, w, y2, gap_x, gap_y, gap, font_size, text_height, ref item_count, ref divider_count, ref y, ref selY, false);
            }
            else if (obj is DividerSelectItem)
            {
                divider_count++;
                Items.Add(new ObjectItem(new Rectangle(10 + gap_y, y + (gap_y - 1) / 2, w - y2, 1)));
                y += gap_y;
            }
            else
            {
                item_count++;
                Rectangle rect_bg = new Rectangle(10 + gap_y, y, w - y2, font_size), rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, text_height);
                Items.Add(new ObjectItem(obj, i, rect_bg, rect_text) { NoIndex = NoIndex });
                if (selectedValue == obj) selY = y;
                y += font_size;
            }
        }
        void InitReadList(object obj, ref string btext, ref bool ui_online, ref bool ui_icon, ref bool ui_arrow)
        {
            if (obj is SelectItem it)
            {
                string text = it.Text + it.SubText;
                if (text.Length > btext.Length) btext = text;
                if (it.Online > -1) ui_online = true;
                if (it.Icon != null) ui_icon = true;
                else if (it.IconSvg != null) ui_icon = true;
                if (it.Sub != null && it.Sub.Count > 0) ui_arrow = true;
            }
            else if (obj is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
            {
                foreach (var item in group.Sub) InitReadList(item, ref btext, ref ui_online, ref ui_icon, ref ui_arrow);
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

        internal void TextChange(string val)
        {
            int count = 0;
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
                for (int i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    if (it.ID > -1)
                    {
                        if (it.Contains(val))
                        {
                            showcount++;
                            if (it.Text.ToLower() == val)
                            {
                                it.Hover = true;
                                hoveindex = i;
                                count++;
                            }
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
                int height;
                if (nodata)
                {
                    height = 80;
                    SetSizeH(height);
                }
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

                        int text_height = font_size - y2, gap = (text_height - gap_y) / 2;
                        foreach (var it in Items)
                        {
                            if (it.ID > -1 && it.Show)
                            {
                                list_count++;
                                var rect_bg = new Rectangle(10 + gap_y, y, w - y2, font_size);
                                it.SetRect(rect_bg, new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, rect_bg.Height - y2), gap, gap_y);
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
                        y += 10;
                        SetSizeH(y);
                    });
                    height = y;
                }
                EndHeight = height;
                if (PARENT is SelectMultiple control) MyPoint(control, height);
                shadow_temp?.Dispose();
                shadow_temp = null;
                Print();
            }
        }
        internal int TextChangeCore(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                nodata = false;
                foreach (var it in Items) it.Show = true;
            }
            else
            {
                val = val.ToLower();
                int showcount = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    if (it.ID > -1)
                    {
                        if (it.Contains(val))
                        {
                            showcount++;
                            if (it.Text.ToLower() == val)
                            {
                                it.Hover = true;
                                hoveindex = i;
                            }
                            it.Show = true;
                        }
                        else it.Show = false;
                    }
                }
                nodata = showcount == 0;
            }
            if (nodata) return 80;
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

                    int text_height = font_size - y2, gap = (text_height - gap_y) / 2;
                    foreach (var it in Items)
                    {
                        if (it.ID > -1 && it.Show)
                        {
                            list_count++;
                            var rect_bg = new Rectangle(10 + gap_y, y, w - y2, font_size);
                            it.SetRect(rect_bg, new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x * 2, rect_bg.Height - y2), gap, gap_y);
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
                });
                return y + 10;
            }
        }

        void MyPoint(SelectMultiple control, int height) => MyPoint(control.PointToScreen(Point.Empty), control, height, control.Placement, control.DropDownArrow, control.ReadRectangle);

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
                    if (it.Show && it.Enable && it.ID > -1 && it.Contains(e.Location, 0, (int)scrollY.Value, out _))
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
            if (it.Group && it.Val is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
            {
                int count = 0;
                foreach (var item in group.Sub)
                {
                    var value = ReadValue(item);
                    if (selectedValue.Contains(value))
                    {
                        count++;
                        break;
                    }
                }
                if (count > 0)
                {
                    foreach (var item in group.Sub)
                    {
                        var value = ReadValue(item);
                        if (selectedValue.Contains(value)) selectedValue.Remove(value);
                    }
                }
                else
                {
                    foreach (var item in group.Sub)
                    {
                        var value = ReadValue(item);
                        if (!selectedValue.Contains(value)) selectedValue.Add(value);
                    }
                }
            }
            else if (selectedValue.Contains(ReadValue(it.Val))) selectedValue.Remove(ReadValue(it.Val));
            else
            {
                if (MaxChoiceCount > 0 && selectedValue.Count >= MaxChoiceCount) return;
                selectedValue.Add(ReadValue(it.Val));
            }
            if (PARENT is SelectMultiple select) select.SelectedValue = selectedValue.ToArray();
            down = false;
            Print();
        }

        object ReadValue(object obj)
        {
            if (obj is SelectItem it) return it.Tag;
            return obj;
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
                    if (it.Enable)
                    {
                        if (it.Contains(e.Location, 0, (int)scrollY.Value, out var change)) hoveindex = i;
                        if (change) count++;
                    }
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
                        string emptytext = Localization.Provider?.GetLocalizedString("NoData") ?? "暂无数据";
                        using (var brush = new SolidBrush(Color.FromArgb(180, Style.Db.Text)))
                        { g.DrawStr(emptytext, Font, brush, rect_read, Helper.stringFormatCenter2); }
                    }
                    else
                    {
                        g.SetClip(path);
                        g.TranslateTransform(0, -scrollY.Value);
                        using (var brush = new SolidBrush(Style.Db.Text))
                        using (var brush_back_hover = new SolidBrush(Style.Db.FillTertiary))
                        using (var brush_sub = new SolidBrush(Style.Db.TextQuaternary))
                        using (var brush_fore = new SolidBrush(Style.Db.TextTertiary))
                        using (var brush_split = new SolidBrush(Style.Db.Split))
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
                                            if (it.Group)
                                            {
                                                DrawItem(g, brush, brush_sub, brush_back_hover, brush_fore, brush_split, it);
                                                oldsel = -1;
                                            }
                                            else
                                            {
                                                bool isn = IFNextSelect(i + 1);
                                                if (oldsel == -1)
                                                {
                                                    if (isn)
                                                    {
                                                        oldsel = i;
                                                        DrawItemSelect(g, brush, brush_sub, brush_split, it, true, true, false, false);
                                                    }
                                                    else DrawItemSelect(g, brush, brush_sub, brush_split, it, true, true, true, true);
                                                }
                                                else
                                                {
                                                    if (isn) DrawItemSelect(g, brush, brush_sub, brush_split, it, false, false, false, false);
                                                    else DrawItemSelect(g, brush, brush_sub, brush_split, it, false, false, true, true);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DrawItem(g, brush, brush_sub, brush_back_hover, brush_fore, brush_split, it);
                                            oldsel = -1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var it in Items)
                                {
                                    if (it.Show) DrawItemR(g, brush, brush_back_hover, brush_split, it);
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

        void DrawItemSelect(Graphics g, SolidBrush brush, SolidBrush subbrush, SolidBrush brush_split, ObjectItem it, bool TL, bool TR, bool BR, bool BL)
        {
            if (it.ID == -1) g.FillRectangle(brush_split, it.Rect);
            else
            {
                using (var brush_back = new SolidBrush(Style.Db.PrimaryBg))
                {
                    using (var path = it.Rect.RoundPath(Radius, TL, TR, BR, BL))
                    {
                        g.FillPath(brush_back, path);
                    }
                }
                if (it.SubText != null)
                {
                    var size = g.MeasureString(it.Text, Font);
                    var rectSubText = new RectangleF(it.RectText.X + size.Width, it.RectText.Y, it.RectText.Width - size.Width, it.RectText.Height);
                    g.DrawStr(it.SubText, Font, subbrush, rectSubText, stringFormatLeft);
                }
                DrawTextIconSelect(g, it);
                g.PaintIconCore(new Rectangle(it.Rect.Right - it.Rect.Height, it.Rect.Y, it.Rect.Height, it.Rect.Height), SvgDb.IcoSuccessGhost, Style.Db.Primary, .46F);
                if (it.Online.HasValue)
                {
                    using (var brush_online = new SolidBrush(it.OnlineCustom ?? (it.Online == 1 ? Style.Db.Success : Style.Db.Error)))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
                if (it.has_sub) DrawArrow(g, it, Style.Db.TextBase);
            }
        }

        void DrawItem(Graphics g, SolidBrush brush, SolidBrush subbrush, SolidBrush brush_back_hover, SolidBrush brush_fore, SolidBrush brush_split, ObjectItem it)
        {
            if (it.ID == -1) g.FillRectangle(brush_split, it.Rect);
            else if (it.Group) g.DrawStr(it.Text, Font, brush_fore, it.RectText, stringFormatLeft);
            else
            {
                if (it.SubText != null)
                {
                    var size = g.MeasureString(it.Text, Font);
                    var rectSubText = new RectangleF(it.RectText.X + size.Width, it.RectText.Y, it.RectText.Width - size.Width, it.RectText.Height);
                    g.DrawStr(it.SubText, Font, subbrush, rectSubText, stringFormatLeft);
                }
                if (MaxChoiceCount > 0 && selectedValue.Count >= MaxChoiceCount) DrawTextIcon(g, it, subbrush);
                else
                {
                    if (it.Hover)
                    {
                        using (var path = it.Rect.RoundPath(Radius))
                        {
                            g.FillPath(brush_back_hover, path);
                        }
                    }
                    DrawTextIcon(g, it, brush);
                }
                if (it.Online.HasValue)
                {
                    using (var brush_online = new SolidBrush(it.OnlineCustom ?? (it.Online == 1 ? Style.Db.Success : Style.Db.Error)))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
                if (it.has_sub) DrawArrow(g, it, Style.Db.TextBase);
            }
        }
        void DrawItemR(Graphics g, SolidBrush brush, SolidBrush brush_back_hover, SolidBrush brush_split, ObjectItem it)
        {
            if (it.ID == -1) g.FillRectangle(brush_split, it.Rect);
            else if (selectedValue.Contains(it.Val) || it.Val is SelectItem item && selectedValue.Contains(item.Tag))
            {
                using (var brush_back = new SolidBrush(Style.Db.PrimaryBg))
                {
                    g.FillRectangle(brush_back, it.Rect);
                }
                DrawTextIconSelect(g, it);
                g.PaintIconCore(new Rectangle(it.Rect.Right - it.Rect.Height, it.Rect.Y, it.Rect.Height, it.Rect.Height), SvgDb.IcoSuccessGhost, Style.Db.Primary, .46F);
            }
            else
            {
                if (it.Hover) g.FillRectangle(brush_back_hover, it.Rect);
                DrawTextIcon(g, it, brush);
            }
            if (it.Online.HasValue)
            {
                using (var brush_online = new SolidBrush(it.OnlineCustom ?? (it.Online == 1 ? Style.Db.Success : Style.Db.Error)))
                {
                    g.FillEllipse(brush_online, it.RectOnline);
                }
            }
            if (it.has_sub) DrawArrow(g, it, Style.Db.TextBase);
        }

        void DrawTextIconSelect(Graphics g, ObjectItem it)
        {
            if (it.Enable)
            {
                using (var fore = new SolidBrush(Style.Db.TextBase))
                {
                    g.DrawStr(it.Text, Font, fore, it.RectText, stringFormatLeft);
                }
            }
            else
            {
                using (var fore = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.DrawStr(it.Text, Font, fore, it.RectText, stringFormatLeft);
                }
            }
            DrawIcon(g, it, Style.Db.TextBase);
        }
        void DrawTextIcon(Graphics g, ObjectItem it, SolidBrush brush)
        {
            if (it.Enable) g.DrawStr(it.Text, Font, brush, it.RectText, stringFormatLeft);
            else
            {
                using (var fore = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.DrawStr(it.Text, Font, fore, it.RectText, stringFormatLeft);
                }
            }
            DrawIcon(g, it, brush.Color);
        }
        void DrawIcon(Graphics g, ObjectItem it, Color color)
        {
            if (it.IconSvg != null)
            {
                using (var bmp = SvgExtend.GetImgExtend(it.IconSvg, it.RectIcon, color))
                {
                    if (bmp != null)
                    {
                        if (it.Enable) g.DrawImage(bmp, it.RectIcon);
                        else g.DrawImage(bmp, it.RectIcon, 0.25F);
                        return;
                    }
                }
            }
            if (it.Icon != null)
            {
                if (it.Enable) g.DrawImage(it.Icon, it.RectIcon);
                else g.DrawImage(it.Icon, it.RectIcon, 0.25F);
            }
        }
        void DrawArrow(Graphics g, ObjectItem item, Color color)
        {
            int size = item.arr_rect.Width, size_arrow = size / 2;
            g.TranslateTransform(item.arr_rect.X + size_arrow, item.arr_rect.Y + size_arrow);
            g.RotateTransform(-90F);
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, item.arr_rect.Width, item.arr_rect.Height).TriangleLines(-1, .2F));
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
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null || (shadow_temp.Width != shadow_width || shadow_temp.Height != shadow_height))
                {
                    shadow_temp?.Dispose();
                    using (var path = new Rectangle(10, 10, shadow_width - 20, shadow_height - 20).RoundPath(Radius))
                    {
                        shadow_temp = path.PaintShadow(shadow_width, shadow_height);
                    }
                }
                g.DrawImage(shadow_temp, rect_client, 0.2F);
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