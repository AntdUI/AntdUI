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

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormSelectMultipleCheck : ISelectMultiple, SubLayeredForm
    {
        #region 初始化

        TAMode ColorScheme;
        int MaxCount = 0, MaxChoiceCount = 0;
        Size DPadding;
        bool DropNoMatchClose = false, AutoWidth = true;
        List<ObjectItemCheck> Items;
        IList<object> ItemOS;
        List<object> selectedValue;
        public LayeredFormSelectMultipleCheck(SelectMultiple control, IList<object> items, string? filtertext)
        {
            PARENT = control;
            Font = control.Font;
            ColorScheme = control.ColorScheme;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = new List<object>(control.SelectedValue.Length);
            selectedValue.AddRange(control.SelectedValue);
            DropNoMatchClose = control.DropDownEmptyClose;
            MaxCount = control.MaxCount;
            MaxChoiceCount = control.MaxChoiceCount;
            DPadding = control.DropDownPadding;
            AutoWidth = control.ListAutoWidth;
            ItemOS = items;
            sf = Helper.SF(control.DropDownTextAlign);

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = control.radius;
            else
            {
                ArrowSize = (int)(8 * dpi);
                Radius = (int)(control.radius * dpi);
            }
            Items = LoadLayout(AutoWidth, control.ReadRectangle.Width, ItemOS, filtertext, true);

            var tmpAlign = CLocation(control, control.Placement, control.DropDownArrow, ArrowSize);
            if (control.DropDownArrow) ArrowAlign = tmpAlign;
            Init();

        }

        SubLayeredForm? lay;

        float tmpItemHeight = 0F;
        public LayeredFormSelectMultipleCheck(SelectMultiple control, int sx, LayeredFormSelectMultipleCheck parent, int radius, int arrowSize, float itemHeight, Rectangle rect, IList<object> items, int sel = -1)
        {
            select_x = sx;
            PARENT = control;
            Font = control.Font;
            lay = parent;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = new List<object>(control.SelectedValue.Length);
            selectedValue.AddRange(control.SelectedValue);
            DropNoMatchClose = control.DropDownEmptyClose;
            DPadding = parent.DPadding;
            AutoWidth = true;
            ItemOS = items;
            sf = Helper.SF(control.DropDownTextAlign);

            Radius = radius;
            ArrowSize = arrowSize;

            control.Disposed += (a, b) => Dispose();

            Items = LoadLayout(AutoWidth, 0, ItemOS, null, true);
            tmpItemHeight = itemHeight;
            var tmpAlign = CLocation(parent, rect, control.DropDownArrow, ArrowSize);
            if (control.DropDownArrow) ArrowAlign = tmpAlign;
            Init();
        }

        public void Rload(List<object> value)
        {
            selectedValue = new List<object>(value.Count);
            selectedValue.AddRange(value);
            Print();
        }

        void Init()
        {
            if (OS.Win7OrLower) Select();
            KeyCall = keys =>
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
                        if (it.SID) { OnClick(it); return true; }
                    }
                }
                else if (keys == Keys.Up)
                {
                    hoveindex--;
                    if (hoveindex < 0) hoveindex = Items.Count - 1;
                    while (!Items[hoveindex].SID)
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
                        while (!Items[hoveindex].SID)
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

        #endregion

        #region 参数

        public override string name => nameof(AntdUI.Select);

        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        ScrollBar ScrollBar;
        bool nodata = false;

        public ILayeredForm? SubForm() => subForm;
        LayeredFormSelectMultipleCheck? subForm;

        #endregion

        #region 渲染

        StringFormat sf;
        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(Colour.BgElevated.Get(name, ColorScheme)))
            {
                g.Fill(brush, path);
                if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, tmpItemHeight));
            }
        }
        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            if (nodata) g.PaintEmpty(rect, Font, Color.FromArgb(180, Colour.Text.Get(name, ColorScheme)));
            else
            {
                g.TranslateTransform(0, -ScrollBar.Value);
                using (var brush = new SolidBrush(Colour.Text.Get("Select", ColorScheme)))
                using (var brush_back_hover = new SolidBrush(Colour.FillTertiary.Get("Select", ColorScheme)))
                using (var brush_sub = new SolidBrush(Colour.TextQuaternary.Get("Select", ColorScheme)))
                using (var brush_fore = new SolidBrush(Colour.TextTertiary.Get("Select", ColorScheme)))
                using (var brush_split = new SolidBrush(Colour.Split.Get("Select", ColorScheme)))
                {
                    if (Radius > 0)
                    {
                        int oldsel = -1;
                        for (int i = 0; i < Items.Count; i++)
                        {
                            var it = Items[i];

                            //判断下一个是不是连续的
                            if (selectedValue.Contains(it.Tag))
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
                                            DrawItemSelect(g, brush_sub, brush_split, it, true, true, false, false);
                                        }
                                        else DrawItemSelect(g, brush_sub, brush_split, it, true, true, true, true);
                                    }
                                    else
                                    {
                                        if (isn) DrawItemSelect(g, brush_sub, brush_split, it, false, false, false, false);
                                        else DrawItemSelect(g, brush_sub, brush_split, it, false, false, true, true);
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
                    else
                    {
                        foreach (var it in Items) DrawItemR(g, brush, brush_back_hover, brush_split, it);
                    }
                }
                g.Restore(state);
                ScrollBar.Paint(g);
            }
        }

        bool IFNextSelect(int start)
        {
            for (int i = start; i < Items.Count; i++)
            {
                var it = Items[i];
                if (it != null)
                {
                    if (selectedValue.Contains(it.Tag)) return true;
                    else return false;
                }
            }
            return false;
        }

        void DrawItemSelect(Canvas g, SolidBrush subbrush, SolidBrush brush_split, ObjectItemCheck it, bool TL, bool TR, bool BR, bool BL)
        {
            if (it.SID)
            {
                using (var path = it.Rect.RoundPath(Radius, TL, TR, BR, BL))
                {
                    using (var brush = it.BackActiveExtend.BrushEx(it.Rect, it.BackActive ?? Colour.PrimaryBg.Get("Select", ColorScheme)))
                    {
                        g.Fill(brush, path);
                    }
                }
                if (it.SubText != null)
                {
                    var size = g.MeasureText(it.Text, Font);
                    var rectSubText = new Rectangle(it.RectText.X + size.Width, it.RectText.Y, it.RectText.Width - size.Width, it.RectText.Height);
                    g.DrawText(it.SubText, Font, subbrush, rectSubText, sf);
                }
                DrawTextIconSelect(g, it);
                if (it.Online.HasValue)
                {
                    using (var brush_online = new SolidBrush(it.OnlineCustom ?? (it.Online == 1 ? Colour.Success.Get("Select", ColorScheme) : Colour.Error.Get("Select", ColorScheme))))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
                if (it.HasSub) DrawArrow(g, it, Colour.TextBase.Get("Select", ColorScheme));
            }
            else g.Fill(brush_split, it.Rect);
        }

        void DrawItem(Canvas g, SolidBrush brush, SolidBrush subbrush, SolidBrush brush_back_hover, SolidBrush brush_fore, SolidBrush brush_split, ObjectItemCheck it)
        {
            if (it.SID)
            {
                if (it.Group) g.DrawText(it.Text, Font, brush_fore, it.RectText, sf);
                else
                {
                    if (it.SubText != null)
                    {
                        var size = g.MeasureText(it.Text, Font);
                        var rectSubText = new Rectangle(it.RectText.X + size.Width, it.RectText.Y, it.RectText.Width - size.Width, it.RectText.Height);
                        if (it.ForeSub.HasValue) g.DrawText(it.SubText, Font, it.ForeSub.Value, rectSubText, sf);
                        else g.DrawText(it.SubText, Font, subbrush, rectSubText, sf);
                    }
                    if (MaxChoiceCount > 0 && selectedValue.Count >= MaxChoiceCount) DrawTextIcon(g, it, subbrush, null);
                    else
                    {
                        if (it.Hover)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(brush_back_hover, path);
                            }
                        }
                        DrawTextIcon(g, it, brush, it.Fore);
                    }
                    if (it.Online.HasValue)
                    {
                        using (var brush_online = new SolidBrush(it.OnlineCustom ?? (it.Online == 1 ? Colour.Success.Get("Select", ColorScheme) : Colour.Error.Get("Select", ColorScheme))))
                        {
                            g.FillEllipse(brush_online, it.RectOnline);
                        }
                    }
                    if (it.HasSub) DrawArrow(g, it, Colour.TextBase.Get("Select", ColorScheme));
                }
            }
            else g.Fill(brush_split, it.Rect);
        }
        void DrawItemR(Canvas g, SolidBrush brush, SolidBrush brush_back_hover, SolidBrush brush_split, ObjectItemCheck it)
        {
            if (it.SID)
            {
                if (selectedValue.Contains(it.Tag))
                {
                    using (var brush_back = new SolidBrush(Colour.PrimaryBg.Get("Select", ColorScheme)))
                    {
                        g.Fill(brush_back, it.Rect);
                    }
                    DrawTextIconSelect(g, it);
                }
                else
                {
                    if (it.Hover) g.Fill(brush_back_hover, it.Rect);
                    DrawTextIcon(g, it, brush, it.Fore);
                }
                if (it.Online.HasValue)
                {
                    using (var brush_online = new SolidBrush(it.OnlineCustom ?? (it.Online == 1 ? Colour.Success.Get("Select", ColorScheme) : Colour.Error.Get("Select", ColorScheme))))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
                if (it.HasSub) DrawArrow(g, it, Colour.TextBase.Get("Select", ColorScheme));
            }
            else g.Fill(brush_split, it.Rect);
        }

        void DrawTextIconSelect(Canvas g, ObjectItemCheck it)
        {
            if (it.Enable)
            {
                using (var fore = new SolidBrush(Colour.TextBase.Get("Select", ColorScheme)))
                {
                    g.DrawText(it.Text, Font, fore, it.RectText, sf);
                }
            }
            else
            {
                using (var fore = new SolidBrush(Colour.TextQuaternary.Get("Select", ColorScheme)))
                {
                    g.DrawText(it.Text, Font, fore, it.RectText, sf);
                }
            }
            DrawIcon(g, it, Colour.TextBase.Get("Select", ColorScheme));

            using (var path = it.RectCheck.RoundPath(Radius / 2))
            {
                g.Fill(Colour.Primary.Get("Select", ColorScheme), path);
                using (var brush = new Pen(Colour.BgBase.Get("Select", ColorScheme), 2.6F * Config.Dpi))
                {
                    g.DrawLines(brush, it.RectCheck.CheckArrow());
                }
            }
        }
        void DrawTextIcon(Canvas g, ObjectItemCheck it, SolidBrush brush, Color? color)
        {
            if (it.Enable)
            {
                if (color.HasValue) g.DrawText(it.Text, Font, color.Value, it.RectText, sf);
                else g.DrawText(it.Text, Font, brush, it.RectText, sf);
            }
            else
            {
                using (var fore = new SolidBrush(Colour.TextQuaternary.Get("Select", ColorScheme)))
                {
                    g.DrawText(it.Text, Font, fore, it.RectText, sf);
                }
            }
            DrawIcon(g, it, color ?? brush.Color);

            using (var path = it.RectCheck.RoundPath(Radius / 2))
            {
                g.Draw(Colour.BorderColor.Get("Select", ColorScheme), 2F * Config.Dpi, path);
            }
        }
        void DrawIcon(Canvas g, ObjectItemCheck it, Color color)
        {
            if (it.Icon != null)
            {
                if (it.Enable) g.Image(it.Icon, it.RectIcon);
                else g.Image(it.Icon, it.RectIcon, 0.25F);
            }
            if (it.IconSvg != null)
            {
                using (var bmp = SvgExtend.GetImgExtend(it.IconSvg, it.RectIcon, color))
                {
                    if (bmp != null)
                    {
                        if (it.Enable) g.Image(bmp, it.RectIcon);
                        else g.Image(bmp, it.RectIcon, 0.25F);
                    }
                }
            }
        }
        void DrawArrow(Canvas g, ObjectItemCheck item, Color color)
        {
            var state = g.Save();
            int size = item.RectArrow.Width, size_arrow = size / 2;
            g.TranslateTransform(item.RectArrow.X + size_arrow, item.RectArrow.Y + size_arrow);
            g.RotateTransform(-90F);
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, item.RectArrow.Width, item.RectArrow.Height).TriangleLines(-1, .2F));
            }
            g.Restore(state);
        }

        #endregion

        #region 布局

        int tmpW = 0, tmp_padd = 0;
        List<ObjectItemCheck> LoadLayout(bool autoWidth, int width, IList<object> items, string? search, bool init = false) => Helper.GDI(g => LoadLayout(g, autoWidth, width, SearchList(items, search), init));
        List<ObjectItemCheck> LoadLayout(Canvas g, bool autoWidth, int width, IList<object> items, bool init)
        {
            var text_height = g.MeasureString(Config.NullText, Font).Height;
            if (items.Count > 0)
            {
                nodata = false;
                int sp = (int)Config.Dpi, padd = (int)(text_height * .18F), padd2 = padd * 2, gap_x = (int)(DPadding.Width * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi),
                icon_size = (int)(text_height * .7F), icon_gap = (int)(text_height * .25F), item_height = text_height + gap_y * 2, icon_xy = (item_height - icon_size) / 2,
                gap_x2 = gap_x * 2, gap_y2 = gap_y * 2;

                tmp_padd = padd;

                #region 计算最大区域

                int maxw, maxwr;
                if (autoWidth)
                {
                    maxw = ItemMaxWidth(g, items, text_height, icon_size, icon_gap);
                    maxwr = maxw + gap_x2;
                }
                else
                {
                    maxwr = width - padd2;
                    maxw = maxwr - gap_x2;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                }
                sf.FormatFlags = StringFormatFlags.NoWrap;

                int item_count = 0, divider_count = 0, y = 0, sy = 0;
                var lists = new List<ObjectItemCheck>(items.Count);
                foreach (var value in items)
                {
                    if (value is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
                    {
                        item_count++;
                        var rect = new Rectangle(padd, padd + y, maxwr, item_height);
                        lists.Add(new ObjectItemCheck(group, rect, new Rectangle(rect.X + gap_x, rect.Y, rect.Width - gap_x2, rect.Height)));
                        y += item_height;
                        foreach (var sub in group.Sub) lists.Add(ItemC(sub, ref item_count, ref divider_count, ref y, padd, padd2, sp, gap_x, gap_x2, icon_size, icon_gap, icon_xy, item_height, text_height, maxwr, ref sy, false));
                    }
                    else lists.Add(ItemC(value, ref item_count, ref divider_count, ref y, padd, padd2, sp, gap_x, gap_x2, icon_size, icon_gap, icon_xy, item_height, text_height, maxwr, ref sy));
                }

                #endregion

                int maxh = item_height * item_count + padd2;
                if (divider_count > 0) maxh += divider_count * (padd2 + sp);
                int h = maxh, w = maxw + padd2 + gap_x2;
                if (MaxCount > 0 && item_count > MaxCount)
                {
                    if (autoWidth) w += ScrollBar.SIZE - padd;
                    h = item_height * MaxCount + padd2;
                    ScrollBar.SizeChange(new Rectangle(0, 0, w, h));
                    ScrollBar.SetVrSize(0, maxh);
                    if (sy > 0) ScrollBar.ValueY = sy;
                }
                else ScrollBar.SetVrSize(0, 0);
                if (init) tmpW = w;
                SetSize(w, h);
                return lists;
            }
            else
            {
                nodata = true;
                int w = width;
                if (autoWidth) w = (int)(g.MeasureText(Localization.Get("NoData", "暂无数据"), Font).Width * 1.6F);
                if (init) tmpW = w;
                if (DropNoMatchClose) IClose();
                else SetSize(w, text_height * 5);
                return new List<ObjectItemCheck>(0);
            }
        }
        int ItemMaxWidth(Canvas g, IList<object> items, int text_height, int icon_size, int icon_gap)
        {
            int tmp = 0;
            foreach (var item in items)
            {
                int tmp2 = ItemMaxWidth(g, item, text_height, icon_size, icon_gap);
                if (tmp2 > tmp) tmp = tmp2;
            }
            return tmp;
        }
        int ItemMaxWidth(Canvas g, object obj, int text_height, int icon_size, int icon_gap)
        {
            if (obj is SelectItem it)
            {
                int tmp = g.MeasureText(it.Text + it.SubText, Font).Width;
                if (it.Online > -1) tmp += icon_size;
                if (it.Icon != null || it.IconSvg != null) tmp += icon_size + icon_gap;
                if (it.Sub != null && it.Sub.Count > 0) tmp += icon_size + icon_gap;
                return tmp + text_height + icon_gap;
            }
            else if (obj is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0) return ItemMaxWidth(g, group.Sub, text_height, icon_size, icon_gap);
            else if (obj is DividerSelectItem) return 0;
            else
            {
                var text = obj.ToString();
                if (text == null) return 0;
                var size = g.MeasureText(text, Font);
                return size.Width + text_height + icon_gap;
            }
        }
        ObjectItemCheck ItemC(object value, ref int item_count, ref int divider_count, ref int y, int padd, int padd2, int sp, int gap_x, int gap_x2, int icon_size, int icon_gap, int icon_xy, int item_height, int text_height, int maxwr, ref int sy, bool no_id = true)
        {
            ObjectItemCheck item;
            if (value is DividerSelectItem)
            {
                divider_count++;
                item = new ObjectItemCheck(new Rectangle(padd, padd2 + y, maxwr, sp));
                y += padd2 + sp;
            }
            else
            {
                item_count++;
                var rect = new Rectangle(padd, padd + y, maxwr, item_height);
                int ux = gap_x, uw = gap_x2 + text_height;
                int check_xy = (item_height - text_height) / 2;
                var rect_check = new Rectangle(rect.X + ux, rect.Y + check_xy, text_height, text_height);
                ux += text_height + icon_gap;
                if (value is SelectItem it)
                {
                    item = new ObjectItemCheck(it, rect, rect_check) { NoIndex = no_id };
                    if (it.Online > -1)
                    {
                        int dot_xy = (item_height - icon_gap) / 2;
                        item.RectOnline = new Rectangle(rect.X + ux + icon_gap / 2, rect.Y + dot_xy, icon_gap, icon_gap);
                        ux += icon_size;
                        uw += icon_size;
                    }
                    if (item.HasIcon)
                    {
                        int tmp = icon_size + icon_gap;
                        item.RectIcon = new Rectangle(rect.X + ux, rect.Y + icon_xy, icon_size, icon_size);
                        ux += tmp;
                        uw += tmp;
                    }
                    if (item.HasSub)
                    {
                        item.RectArrow = new Rectangle(rect.Right - gap_x - icon_size, rect.Y + icon_xy, icon_size, icon_size);
                        uw += icon_size + icon_gap;
                    }
                    item.RectText = new Rectangle(rect.X + ux, rect.Y, rect.Width - uw, rect.Height);
                }
                else item = new ObjectItemCheck(value, rect, new Rectangle(rect.X + ux, rect.Y, rect.Width - uw, rect.Height), rect_check) { NoIndex = no_id };
                if (sy == 0 && selectedValue.Contains(item.Tag)) sy = y;
                y += item_height;
            }
            return item;
        }

        public void FocusItem(ObjectItemCheck item)
        {
            if (item.SetHover(true))
            {
                if (ScrollBar.ShowY) ScrollBar.ValueY = item.Rect.Y - item.Rect.Height;
                Print();
            }
        }

        #endregion

        #region 鼠标

        internal int select_x = 0;
        int hoveindex = -1, hoveindexold = -1;
        bool down = false;
        protected override void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseDown(x, y))
            {
                OnTouchDown(x, y);
                down = true;
            }
        }
        protected override void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseMove(x, y) && OnTouchMove(x, y))
            {
                hoveindex = -1;
                int count = 0, hand = 0, sy = ScrollBar.Value;
                for (int i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    if (it.Enable)
                    {
                        if (it.Contains(x, y, 0, sy, out var change))
                        {
                            if (!it.Group) hand++;
                            hoveindex = i;
                        }
                        if (change) count++;
                    }
                }
                if (count > 0) Print();
                SetCursor(hand > 0);
            }
            else SetCursor(false);
            if (hoveindexold == hoveindex) return;
            hoveindexold = hoveindex;
            subForm?.IClose();
            subForm = null;
            if (hoveindex > -1)
            {
                if (PARENT is SelectMultiple select) select.select_x = select_x;
                var it = Items[hoveindex];
                if (it.Sub != null && it.Sub.Count > 0 && PARENT != null) OpenDown(it, it.Sub);
            }
        }
        protected override void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseUp() && OnTouchUp() && down)
            {
                down = false;
                int sy = ScrollBar.Value;
                foreach (var it in Items)
                {
                    if (it.Enable && it.SID && it.Contains(x, y, 0, sy, out _))
                    {
                        OnClick(it);
                        return;
                    }
                }
            }
            else down = false;
        }

        void OnClick(ObjectItemCheck it)
        {
            if (it.Group && it.Tag is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
            {
                int count = 0;
                foreach (var item in group.Sub)
                {
                    if (selectedValue.Contains(item))
                    {
                        count++;
                        break;
                    }
                }
                if (count > 0)
                {
                    foreach (var item in group.Sub)
                    {
                        if (selectedValue.Contains(item)) selectedValue.Remove(item);
                    }
                }
                else
                {
                    foreach (var item in group.Sub)
                    {
                        if (!selectedValue.Contains(item)) selectedValue.Add(item);
                    }
                }
            }
            else
            {
                if (it.Sub == null || it.Sub.Count == 0)
                {
                    if (selectedValue.Contains(it.Tag)) selectedValue.Remove(it.Tag);
                    else
                    {
                        if (MaxChoiceCount > 0 && selectedValue.Count >= MaxChoiceCount) return;
                        selectedValue.Add(it.Tag);
                    }
                }
                else
                {
                    if (selectedValue.Contains(it.Tag))
                    {
                        selectedValue.Remove(it.Tag);
                        DelValues(it.Sub);
                    }
                    else
                    {
                        selectedValue.Add(it.Tag);
                        AddValues(it.Sub);
                    }
                    subForm?.Rload(selectedValue);
                }
            }
            if (PARENT is SelectMultiple select) select.SelectedValue = selectedValue.ToArray();
            down = false;
            Print();
        }

        void OpenDown(ObjectItemCheck it, IList<object> sub, int tag = -1)
        {
            var rect = new Rectangle(it.Rect.X + tmp_padd, it.Rect.Y - ScrollBar.ValueY - tmp_padd, it.Rect.Width, it.Rect.Height);
            if (PARENT is SelectMultiple select)
            {
                subForm = new LayeredFormSelectMultipleCheck(select, select_x + 1, this, Radius, ArrowSize, tmp_padd + it.Rect.Height / 2F, rect, sub, tag);
                subForm.Show(this);
            }
        }
        public override void IClosing()
        {
            if (select_x == 0)
            {
                var item = this;
                while (item.lay is LayeredFormSelectMultipleCheck form)
                {
                    if (item == form) return;
                    form.IClose();
                    item = form;
                }
            }
        }
        void AddValues(IList<object> sub)
        {
            foreach (var it in sub)
            {
                if (it is SelectItem sit)
                {
                    if (!selectedValue.Contains(sit.Tag)) selectedValue.Add(sit.Tag);
                    if (sit.Sub != null && sit.Sub.Count > 0) AddValues(sit.Sub);
                }
                else if (!selectedValue.Contains(it)) selectedValue.Add(it);
            }
        }
        void DelValues(IList<object> sub)
        {
            foreach (var it in sub)
            {
                if (it is SelectItem sit)
                {
                    selectedValue.Remove(sit.Tag);
                    if (sit.Sub != null && sit.Sub.Count > 0) DelValues(sit.Sub);
                }
                else selectedValue.Remove(it);
            }
        }

        protected override void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (delta != 0) ScrollBar.MouseWheel(delta);
        }
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        #endregion

        #region 方法

        public override void SetValues(object[] value)
        {
            selectedValue = new List<object>(value.Length);
            selectedValue.AddRange(value);
            Print();
        }
        public override void SetValues(List<object> value)
        {
            selectedValue = value;
            Print();
        }
        public override void ClearValues()
        {
            selectedValue = new List<object>(0);
            Print();
        }

        #endregion

        #region 筛选

        #region 主动搜索

        /// <summary>
        /// 搜索指定文本
        /// </summary>
        /// <param name="search"></param>
        public override void TextChange(string search)
        {
            Items = LoadLayout(AutoWidth, tmpW, ItemOS, search);
            PrintAndClear();
        }

        /// <summary>
        /// 搜索放入自己的结果
        /// </summary>
        /// <param name="items"></param>
        public void TextChange(IList<object> items)
        {
            Items = LoadLayout(AutoWidth, tmpW, items, null);
            PrintAndClear();
        }

        #endregion

        IList<object> SearchList(IList<object> items, string? search)
        {
            if (search == null || string.IsNullOrEmpty(search)) return items;
            else
            {
                object? select = null;
                var listSearch = new List<ItemSearchWeigth<object>>(items.Count);
                SearchList(items, search, ref listSearch, ref select);
                return listSearch.SearchWeightSort() ?? new List<object>(0);
            }
        }
        void SearchList(IList<object> items, string search, ref List<ItemSearchWeigth<object>> listSearch, ref object? select)
        {
            foreach (var it in items)
            {
                if (it is DividerSelectItem) continue;
                else if (it is SelectItem selectItem)
                {
                    string pinyin = selectItem.Text + selectItem.SubText;
                    var PY = new string[] {
                        pinyin.ToLower(),
                        Pinyin.GetPinyin(pinyin).ToLower(),
                        Pinyin.GetInitials(pinyin).ToLower()
                    };
                    int score = Helper.SearchContains(search, pinyin, PY, out bool select_item);
                    if (score > 0)
                    {
                        listSearch.Add(new ItemSearchWeigth<object>(score, it));
                        if (select_item) select = select_item;
                    }
                }
                else if (it is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
                {
                    string pinyin = group.Title;
                    var PY = new string[] {
                        pinyin.ToLower(),
                        Pinyin.GetPinyin(pinyin).ToLower(),
                        Pinyin.GetInitials(pinyin).ToLower()
                    };
                    int score = Helper.SearchContains(search, pinyin, PY, out bool select_item);
                    if (score > 0)
                    {
                        listSearch.Add(new ItemSearchWeigth<object>(score, it));
                        if (select_item) select = select_item;
                    }
                    SearchList(group.Sub, search, ref listSearch, ref select);
                }
                else
                {
                    var pinyin = it.ToString();
                    if (pinyin == null) continue;
                    var PY = new string[] {
                        pinyin.ToLower(),
                        Pinyin.GetPinyin(pinyin).ToLower(),
                        Pinyin.GetInitials(pinyin).ToLower()
                    };
                    int score = Helper.SearchContains(search, pinyin, PY, out bool select_item);
                    if (score > 0)
                    {
                        listSearch.Add(new ItemSearchWeigth<object>(score, it));
                        if (select_item) select = select_item;
                    }
                }
            }
        }

        #endregion
    }
}