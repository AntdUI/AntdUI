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
    internal class LayeredFormSelectDown : ILayeredFormOpacityDown, SubLayeredForm
    {
        int MaxCount = 0;
        Size DPadding;
        internal float Radius = 0;
        bool ClickEnd = false;
        object? selectedValue;
        int r_w = 0;
        List<ObjectItem> Items;
        public LayeredFormSelectDown(Select control, IList<object> items, string filtertext)
        {
            control.Parent.SetTopMost(Handle);
            PARENT = control;
            ClickEnd = control.ClickEnd;
            select_x = 0;
            scrollY = new ScrollY(this);
            MaxCount = control.MaxCount;
            Font = control.Font;
            selectedValue = control.SelectedValue;
            Radius = (int)(control.radius * Config.Dpi);
            DPadding = control.DropDownPadding;
            Items = new List<ObjectItem>(items.Count);
            Init(control, control.Placement, control.DropDownArrow, control.ListAutoWidth, control.ReadRectangle, items, filtertext);
        }
        public LayeredFormSelectDown(Dropdown control, int radius, IList<object> items)
        {
            control.Parent.SetTopMost(Handle);
            PARENT = control;
            ClickEnd = control.ClickEnd;
            MessageCloseMouseLeave = control.Trigger == Trigger.Hover;
            select_x = 0;
            scrollY = new ScrollY(this);
            MaxCount = control.MaxCount;
            Font = control.Font;
            Radius = (int)(radius * Config.Dpi);
            DPadding = control.DropDownPadding;
            Items = new List<ObjectItem>(items.Count);
            Init(control, control.Placement, control.DropDownArrow, control.ListAutoWidth, control.ReadRectangle, items);
        }
        public LayeredFormSelectDown(Tabs control, int radius, IList<object> items, object? sValue, Rectangle rect_ctls)
        {
            MessageCloseMouseLeave = true;
            control.Parent.SetTopMost(Handle);
            PARENT = control;
            ClickEnd = false;
            select_x = 0;
            scrollY = new ScrollY(this);
            MaxCount = 7;
            Font = control.Font;
            selectedValue = sValue;
            Radius = (int)(radius * Config.Dpi);
            DPadding = new Size(12, 5);
            Items = new List<ObjectItem>(items.Count);
            TAlignFrom align;
            switch (control.Alignment)
            {
                case TabAlignment.Bottom:
                    align = TAlignFrom.TR;
                    break;
                case TabAlignment.Left:
                    align = TAlignFrom.TL;
                    break;
                case TabAlignment.Right:
                    align = TAlignFrom.TR;
                    break;
                default:
                    align = TAlignFrom.BR;
                    break;
            }
            Init(control, align, false, true, rect_ctls, items);
        }

        public LayeredFormSelectDown(Select control, int sx, LayeredFormSelectDown ocontrol, float radius, Rectangle rect_read, IList<object> items, int sel = -1)
        {
            ClickEnd = control.ClickEnd;
            selectedValue = control.SelectedValue;
            scrollY = new ScrollY(this);
            DPadding = control.DropDownPadding;
            Items = new List<ObjectItem>(items.Count);
            InitObj(control, sx, ocontrol, radius, rect_read, items, sel);
        }
        public LayeredFormSelectDown(Dropdown control, int sx, LayeredFormSelectDown ocontrol, float radius, Rectangle rect_read, IList<object> items, int sel = -1)
        {
            ClickEnd = control.ClickEnd;
            scrollY = new ScrollY(this);
            DPadding = control.DropDownPadding;
            Items = new List<ObjectItem>(items.Count);
            InitObj(control, sx, ocontrol, radius, rect_read, items, sel);
        }

        void InitObj(Control parent, int sx, LayeredFormSelectDown control, float radius, Rectangle rect_read, IList<object> items, int sel)
        {
            parent.Parent.SetTopMost(Handle);
            select_x = sx;
            PARENT = parent;
            Font = control.Font;
            Radius = radius;

            control.Disposed += (a, b) => { Dispose(); };

            Init(control, TAlignFrom.BL, false, true, rect_read, items);

            if (sel > -1)
            {
                try
                {
                    hoveindex = sel;
                    Items[hoveindex].SetHover(true);
                }
                catch { }
            }
        }

        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        public ILayeredForm? SubForm() => subForm;
        LayeredFormSelectDown? subForm = null;
        void Init(Control control, TAlignFrom Placement, bool ShowArrow, bool ListAutoWidth, Rectangle rect_read, IList<object> items, string? filtertext = null)
        {
            int y = 10, w = rect_read.Width;
            r_w = w;
            var point = control.PointToScreen(Point.Empty);
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font).Size();
                int sp = (int)(1 * Config.Dpi), gap = (int)(4 * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi), gap_x = (int)(DPadding.Width * Config.Dpi),
                gap2 = gap * 2, gap_x2 = gap_x * 2, gap_y2 = gap_y * 2,
                text_height = size.Height, item_height = text_height + gap_y2;
                y += gap;
                if (ListAutoWidth)
                {
                    int b_w = size.Width + gap_x2;
                    bool ui_online = false, ui_icon = false, ui_arrow = false;
                    foreach (var obj in items) InitReadList(g, obj, ref b_w, ref ui_online, ref ui_icon, ref ui_arrow);
                    if (ui_icon || ui_online)
                    {
                        if (ui_icon && ui_online) b_w += text_height + gap_y2;
                        else if (ui_icon) b_w += text_height;
                        else b_w += gap_y;
                    }
                    if (ui_arrow) b_w += gap_y2;
                    w = r_w = b_w + gap_x2 + gap2;
                }
                else stringFormatLeft.Trimming = StringTrimming.EllipsisCharacter;
                stringFormatLeft.FormatFlags = StringFormatFlags.NoWrap;

                int selY = -1;
                int item_count = 0, divider_count = 0;
                for (int i = 0; i < items.Count; i++) ReadList(items[i], i, w, item_height, text_height, gap, gap2, gap_x, gap_x2, gap_y, gap_y2, sp, ref item_count, ref divider_count, ref y, ref selY);
                var vr = (item_height * item_count) + (gap_y * divider_count);
                if (MaxCount > 0)
                {
                    if (Items.Count > MaxCount)
                    {
                        y = 10 + gap2 + (item_height * MaxCount);
                        scrollY.Rect = new Rectangle(w - gap, 10 + gap, 20, (item_height * MaxCount));
                        scrollY.Show = true;
                        scrollY.SetVrSize(vr, scrollY.Rect.Height);
                        if (selY > -1) scrollY.val = scrollY.SetValue(selY - 10 - gap);
                    }
                    else y = 10 + gap2 + vr;
                }
                else
                {
                    var screen = Screen.FromPoint(point).WorkingArea;
                    int sh;
                    if (ShowArrow) sh = point.Y + control.Height + 20 + ArrowSize + gap2;
                    else sh = point.Y + control.Height + 20 + gap2;

                    int ry = 10 + gap2 + vr;
                    if (ry > (screen.Height - point.Y))
                    {
                        MaxCount = (int)Math.Floor((screen.Height - sh) / (item_height * 1.0)) - 1;
                        if (MaxCount < 1) MaxCount = 1;
                        y = 10 + gap2 + (item_height * MaxCount);
                        scrollY.Rect = new Rectangle(w - gap, 10 + gap, 20, (item_height * MaxCount));
                        scrollY.Show = true;
                        scrollY.SetVrSize(vr, scrollY.Rect.Height);
                        if (selY > -1) scrollY.val = scrollY.SetValue(selY - 10 - gap);
                    }
                    else y = ry;
                }
            });
            int r_h;
            if (filtertext == null || string.IsNullOrEmpty(filtertext)) r_h = y + 10;
            else r_h = TextChangeCore(filtertext);
            SetSize(w + 20, r_h);
            if (control is LayeredFormSelectDown) SetLocation(point.X + rect_read.Width, point.Y + rect_read.Y - 10);
            else MyPoint(point, control, Placement, ShowArrow, rect_read);

            KeyCall = keys =>
            {
                int _select_x = -1;
                if (PARENT is Select select) _select_x = select.select_x;
                else if (PARENT is Dropdown dropdown) _select_x = dropdown.select_x;
                if (select_x == _select_x)
                {
                    if (keys == Keys.Escape)
                    {
                        IClose();
                        return true;
                    }
                    if (nodata) return false;
                    if (keys == Keys.Enter)
                    {
                        if (hoveindex > -1)
                        {
                            var it = Items[hoveindex];
                            if (it.ID != -1 && OnClick(it)) return true;
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
                        }
                        while (Items[hoveindex].ShowAndID)
                        {
                            hoveindex++;
                            if (hoveindex > Items.Count - 1) hoveindex = 0;
                        }
                        foreach (var it in Items) it.Hover = false;
                        FocusItem(Items[hoveindex]);
                        return true;
                    }
                    else if (keys == Keys.Left)
                    {
                        if (_select_x > 0)
                        {
                            if (PARENT is Select select2) select2.select_x--;
                            else if (PARENT is Dropdown dropdown2) dropdown2.select_x--;
                            IClose();
                            return true;
                        }
                    }
                    else if (keys == Keys.Right)
                    {
                        if (hoveindex > -1)
                        {
                            var it = Items[hoveindex];
                            if (it.Sub != null && it.Sub.Count > 0)
                            {
                                subForm?.IClose();
                                subForm = null;
                                OpenDown(it, it.Sub, 0);
                                if (PARENT is Select select2) select2.select_x++;
                                else if (PARENT is Dropdown dropdown2) dropdown2.select_x++;
                            }
                            return true;
                        }
                    }
                }
                return false;
            };
        }

        void MyPoint(Point point, Control control, TAlignFrom Placement, bool ShowArrow, Rectangle rect_read) => CLocation(point, Placement, ShowArrow, 10, r_w + 20, TargetRect.Height, rect_read, ref Inverted, ref ArrowAlign);

        StringFormat stringFormatLeft = Helper.SF(lr: StringAlignment.Near);

        /// <summary>
        /// 计算坐标
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="i">序号</param>
        /// <param name="width">宽度</param>
        /// <param name="item_height">项高度</param>
        /// <param name="text_height">字体高度</param>
        /// <param name="gap"></param>
        /// <param name="gap2"></param>
        /// <param name="gap_x"></param>
        /// <param name="gap_x2"></param>
        /// <param name="gap_y"></param>
        /// <param name="gap_y2"></param>
        /// <param name="sp">分割线大小</param>
        /// <param name="item_count">项数量</param>
        /// <param name="divider_count">分隔线数量</param>
        /// <param name="y">Y</param>
        /// <param name="select_y">选中序号</param>
        void ReadList(object value, int i, int width, int item_height, int text_height, int gap, int gap2, int gap_x, int gap_x2, int gap_y, int gap_y2, int sp, ref int item_count, ref int divider_count, ref int y, ref int select_y, bool NoIndex = true)
        {
            if (value is DividerSelectItem)
            {
                divider_count++;
                Items.Add(new ObjectItem(new Rectangle(10 + gap_y, y + (gap_y - sp) / 2, width - gap_y2, sp)));
                y += gap_y;
            }
            else
            {
                item_count++;
                Rectangle rect = new Rectangle(10 + gap, y, width - gap2, item_height), rect_text = new Rectangle(rect.X + gap_x, rect.Y + gap_y, rect.Width - gap_x2, text_height);
                if (value is SelectItem it)
                {
                    Items.Add(new ObjectItem(it, i, rect, rect_text, gap_x, gap_x2, gap_y, gap_y2) { NoIndex = NoIndex });
                    if (selectedValue == it.Tag) select_y = y;
                    y += item_height;
                }
                else if (value is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
                {
                    Items.Add(new ObjectItem(group, i, rect, rect_text));
                    if (selectedValue == value) select_y = y;
                    y += item_height;
                    foreach (var item in group.Sub) ReadList(item, i, width, item_height, text_height, gap, gap2, gap_x, gap_x2, gap_y, gap_y2, sp, ref item_count, ref divider_count, ref y, ref select_y, false);
                }
                else
                {
                    Items.Add(new ObjectItem(value, i, rect, rect_text) { NoIndex = NoIndex });
                    if (selectedValue == value) select_y = y;
                    y += item_height;
                }
            }
        }

        void InitReadList(Graphics g, object obj, ref int btext, ref bool ui_online, ref bool ui_icon, ref bool ui_arrow)
        {
            if (obj is SelectItem it)
            {
                string text = it.Text + it.SubText;
                var size = g.MeasureString(text, Font).Size();
                if (size.Width > btext) btext = size.Width;
                if (it.Online > -1) ui_online = true;
                if (it.Icon != null) ui_icon = true;
                else if (it.IconSvg != null) ui_icon = true;
                if (it.Sub != null && it.Sub.Count > 0) ui_arrow = true;
            }
            else if (obj is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
            {
                foreach (var item in group.Sub) InitReadList(g, item, ref btext, ref ui_online, ref ui_icon, ref ui_arrow);
            }
            else if (obj is DividerSelectItem)
            {
            }
            else
            {
                var text = obj.ToString();
                if (text == null) return;
                var size = g.MeasureString(text, Font).Size();
                if (size.Width > btext) btext = size.Width;
            }
        }

        public void FocusItem(ObjectItem item)
        {
            if (item.SetHover(true))
            {
                if (scrollY.Show) scrollY.Value = item.Rect.Y - item.Rect.Height;
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
                        var size = g.MeasureString(Config.NullText, Font).Size();
                        int sp = (int)(1 * Config.Dpi), gap = (int)(4 * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi), gap_x = (int)(DPadding.Width * Config.Dpi),
                        gap2 = gap * 2, gap_x2 = gap_x * 2, gap_y2 = gap_y * 2,
                        text_height = size.Height, item_height = text_height + gap_y2;
                        y += gap;
                        foreach (var it in Items)
                        {
                            if (it.ID > -1 && it.Show)
                            {
                                list_count++;
                                Rectangle rect_bg = new Rectangle(10 + gap, y, w - gap2, item_height),
                                rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x2, text_height);
                                it.SetRect(rect_bg, rect_text, gap_x, gap_x2, gap_y, gap_y2);
                                y += item_height;
                            }
                        }

                        var vr = item_height * list_count;
                        if (list_count > MaxCount)
                        {
                            y = 10 + gap2 + (item_height * MaxCount);
                            scrollY.Rect = new Rectangle(w - gap, 10 + gap, 20, (item_height * MaxCount));
                            scrollY.Show = true;
                            scrollY.SetVrSize(vr, scrollY.Rect.Height);
                        }
                        else
                        {
                            y = 10 + gap2 + vr;
                            scrollY.Show = false;
                        }
                        y += 10;
                        SetSizeH(y);
                    });
                    height = y;
                }
                SetSizeH(height);
                MyPoint();
                shadow_temp?.Dispose();
                shadow_temp = null;
                Print();
            }
        }
        internal void TextChange(string val, IList<object> items)
        {
            int selY = -1, y_ = 0;
            int item_count = 0, divider_count = 0;
            Items.Clear();
            for (int i = 0; i < items.Count; i++) ReadList(items[i], i, 20, 10, 10, 0, 0, 0, 0, 0, 0, 1, ref item_count, ref divider_count, ref y_, ref selY);
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
                        var size = g.MeasureString(Config.NullText, Font).Size();
                        int sp = (int)(1 * Config.Dpi), gap = (int)(4 * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi), gap_x = (int)(DPadding.Width * Config.Dpi),
                        gap2 = gap * 2, gap_x2 = gap_x * 2, gap_y2 = gap_y * 2,
                        text_height = size.Height, item_height = text_height + gap_y2;
                        y += gap;
                        foreach (var it in Items)
                        {
                            if (it.ID > -1 && it.Show)
                            {
                                list_count++;
                                Rectangle rect_bg = new Rectangle(10 + gap, y, w - gap2, item_height),
                                rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x2, text_height);
                                it.SetRect(rect_bg, rect_text, gap_x, gap_x2, gap_y, gap_y2);
                                y += item_height;
                            }
                        }

                        var vr = item_height * list_count;
                        if (list_count > MaxCount)
                        {
                            y = 10 + gap2 + (item_height * MaxCount);
                            scrollY.Rect = new Rectangle(w - gap, 10 + gap, 20, (item_height * MaxCount));
                            scrollY.Show = true;
                            scrollY.SetVrSize(vr, scrollY.Rect.Height);
                        }
                        else
                        {
                            y = 10 + gap2 + vr;
                            scrollY.Show = false;
                        }
                        y += 10;
                        SetSizeH(y);
                    });
                    height = y;
                }
                SetSizeH(height);
                MyPoint();
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
                    var size = g.MeasureString(Config.NullText, Font).Size();
                    int sp = (int)(1 * Config.Dpi), gap = (int)(4 * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi), gap_x = (int)(DPadding.Width * Config.Dpi),
                    gap2 = gap * 2, gap_x2 = gap_x * 2, gap_y2 = gap_y * 2,
                    text_height = size.Height, item_height = text_height + gap_y2;
                    y += gap;
                    foreach (var it in Items)
                    {
                        if (it.ID > -1 && it.Show)
                        {
                            list_count++;
                            Rectangle rect_bg = new Rectangle(10 + gap, y, w - gap2, item_height),
                            rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x2, text_height);
                            it.SetRect(rect_bg, rect_text, gap_x, gap_x2, gap_y, gap_y2);
                            y += item_height;
                        }
                    }

                    var vr = item_height * list_count;
                    if (list_count > MaxCount)
                    {
                        y = 10 + gap2 + (item_height * MaxCount);
                        scrollY.Rect = new Rectangle(w - gap, 10 + gap, 20, (item_height * MaxCount));
                        scrollY.Show = true;
                        scrollY.SetVrSize(vr, scrollY.Rect.Height);
                    }
                    else
                    {
                        y = 10 + gap2 + vr;
                        scrollY.Show = false;
                    }
                });
                return y + 10;
            }
        }

        void MyPoint()
        {
            if (PARENT is Select select) MyPoint(select.PointToScreen(Point.Empty), select, select.Placement, select.DropDownArrow, select.ReadRectangle);
            else if (PARENT is Dropdown dropdown) MyPoint(dropdown.PointToScreen(Point.Empty), dropdown, dropdown.Placement, dropdown.DropDownArrow, dropdown.ReadRectangle);
        }

        #endregion

        /// <summary>
        /// 是否显示暂无数据
        /// </summary>
        bool nodata = false;

        internal ScrollY scrollY;

        #region 鼠标

        internal int select_x = 0;
        int hoveindex = -1;
        bool down = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (scrollY.MouseDown(e.Location))
            {
                OnTouchDown(e.X, e.Y);
                down = true;
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (scrollY.MouseUp(e.Location) && OnTouchUp() && down)
            {
                if (RunAnimation) return;
                foreach (var it in Items)
                {
                    if (it.Show && it.Enable && it.ID > -1 && it.Contains(e.Location, 0, (int)scrollY.Value, out _))
                    {
                        if (OnClick(it)) return;
                    }
                }
            }
            down = false;
            base.OnMouseUp(e);
        }

        bool OnClick(ObjectItem it)
        {
            if (!ClickEnd || it.Sub == null || it.Sub.Count == 0)
            {
                selectedValue = it.Val;
                OnCall(it);
                down = false;
                IClose();
                return true;
            }
            else
            {
                if (subForm == null) OpenDown(it, it.Sub);
                else
                {
                    subForm?.IClose();
                    subForm = null;
                }
            }
            return false;
        }

        void OnCall(ObjectItem it)
        {
            if (PARENT is Select select)
            {
                if (select_x == 0 && it.NoIndex)
                {
                    if (select.DropDownChange()) select.DropDownChange(it.ID);
                    else select.DropDownChange(select_x, it.ID, it.Val);
                }
                else select.DropDownChange(select_x, it.ID, it.Val);
            }
            else if (PARENT is Dropdown dropdown) dropdown.DropDownChange(it.Val);
            else if (PARENT is Tabs tabs) tabs.SelectedIndex = it.ID;
        }

        void OpenDown(ObjectItem it, IList<object> sub, int tag = -1)
        {
            if (PARENT is Select select)
            {
                subForm = new LayeredFormSelectDown(select, select_x + 1, this, Radius, new Rectangle(it.Rect.X, (int)(it.Rect.Y - scrollY.Value), it.Rect.Width, it.Rect.Height), sub, tag);
                subForm.Show(this);
            }
            else if (PARENT is Dropdown dropdown)
            {
                subForm = new LayeredFormSelectDown(dropdown, select_x + 1, this, Radius, new Rectangle(it.Rect.X, (int)(it.Rect.Y - scrollY.Value), it.Rect.Width, it.Rect.Height), sub, tag);
                subForm.Show(this);
            }
        }

        int hoveindexold = -1;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RunAnimation) return;
            hoveindex = -1;
            if (scrollY.MouseMove(e.Location) && OnTouchMove(e.X, e.Y))
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
            if (hoveindexold == hoveindex) return;
            hoveindexold = hoveindex;
            subForm?.IClose();
            subForm = null;
            if (hoveindex > -1)
            {
                if (PARENT is Select select) select.select_x = select_x;
                else if (PARENT is Dropdown dropdown) dropdown.select_x = select_x;
                var it = Items[hoveindex];
                if (it.Sub != null && it.Sub.Count > 0 && PARENT != null) OpenDown(it, it.Sub);
            }
        }

        #endregion

        readonly StringFormat s_f = Helper.SF_NoWrap();
        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path = rect_read.RoundPath(Radius))
                {
                    DrawShadow(g, rect);
                    using (var brush = new SolidBrush(Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path);
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                    }
                    if (nodata)
                    {
                        string emptytext = Localization.Provider?.GetLocalizedString("NoData") ?? "暂无数据";
                        using (var brush = new SolidBrush(Color.FromArgb(180, Style.Db.Text)))
                        { g.DrawStr(emptytext, Font, brush, rect_read, s_f); }
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
                            foreach (var it in Items)
                            {
                                if (it.Show) DrawItem(g, brush, brush_sub, brush_back_hover, brush_fore, brush_split, it);
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

        void DrawItem(Graphics g, SolidBrush brush, SolidBrush subbrush, SolidBrush brush_back_hover, SolidBrush brush_fore, SolidBrush brush_split, ObjectItem it)
        {
            if (it.ID == -1) g.FillRectangle(brush_split, it.Rect);
            else if (it.Group) g.DrawStr(it.Text, Font, brush_fore, it.RectText, stringFormatLeft);
            else if (selectedValue == it.Val || it.Val is SelectItem item && item.Tag == selectedValue)
            {
                using (var brush_back = new SolidBrush(Style.Db.PrimaryBg))
                {
                    using (var path = it.Rect.RoundPath(Radius))
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
            }
            else
            {
                if (it.Hover)
                {
                    using (var path = it.Rect.RoundPath(Radius))
                    {
                        g.FillPath(brush_back_hover, path);
                    }
                }
                if (it.SubText != null)
                {
                    var size = g.MeasureString(it.Text, Font);
                    var rectSubText = new RectangleF(it.RectText.X + size.Width, it.RectText.Y, it.RectText.Width - size.Width, it.RectText.Height);
                    g.DrawStr(it.SubText, Font, subbrush, rectSubText, stringFormatLeft);
                }
                DrawTextIcon(g, it, brush);
            }
            if (it.Online.HasValue)
            {
                Color color = it.OnlineCustom ?? (it.Online == 1 ? Style.Db.Success : Style.Db.Error);
                using (var brush_online = new SolidBrush(it.Enable ? color : Color.FromArgb(Style.Db.TextQuaternary.A, color)))
                {
                    g.FillEllipse(brush_online, it.RectOnline);
                }
            }
            if (it.has_sub) DrawArrow(g, it, Style.Db.TextBase);
        }

        void DrawTextIconSelect(Graphics g, ObjectItem it)
        {
            using (var font = new Font(Font, FontStyle.Bold))
            {
                if (it.Enable)
                {
                    using (var fore = new SolidBrush(Style.Db.TextBase))
                    {
                        g.DrawStr(it.Text, font, fore, it.RectText, stringFormatLeft);
                    }
                }
                else
                {
                    using (var fore = new SolidBrush(Style.Db.TextQuaternary))
                    {
                        g.DrawStr(it.Text, font, fore, it.RectText, stringFormatLeft);
                    }
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
            int size = item.RectArrow.Width, size_arrow = size / 2;
            g.TranslateTransform(item.RectArrow.X + size_arrow, item.RectArrow.Y + size_arrow);
            g.RotateTransform(-90F);
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, item.RectArrow.Width, item.RectArrow.Height).TriangleLines(-1, .2F));
            }
            g.ResetTransform();
            g.TranslateTransform(0, -scrollY.Value);
        }

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">客户区域</param>
        void DrawShadow(Graphics g, Rectangle rect)
        {
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null)
                {
                    shadow_temp?.Dispose();
                    using (var path = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20).RoundPath(Radius))
                    {
                        shadow_temp = path.PaintShadow(rect.Width, rect.Height);
                    }
                }
                g.DrawImage(shadow_temp, rect, 0.2F);
            }
        }

        #region 滚动条

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (RunAnimation) return;
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollY(int value) => scrollY.MouseWheel(value);

        #endregion
    }
}