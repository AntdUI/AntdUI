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
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormSelectDown : ILayeredShadowForm, SubLayeredForm
    {
        TAMode ColorScheme;
        string keyid;
        int MaxCount = 0, select_x = 0;
        Size DPadding;
        bool ClickEnd = false, CloseIcon = false, DropNoMatchClose = false, AutoWidth = true;
        List<ObjectItem> Items;
        ItemIndex ItemOS;
        object? selectedValue;

        #region 初始化

        public LayeredFormSelectDown(Select control, IList<object> items, string? filtertext)
        {
            keyid = nameof(AntdUI.Select);
            PARENT = control;
            Font = control.Font;
            ColorScheme = control.ColorScheme;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = control.SelectedValue;
            ClickEnd = control.ClickEnd;
            CloseIcon = control.CloseIcon;
            DropNoMatchClose = control.DropDownEmptyClose;
            MaxCount = control.MaxCount;
            DPadding = control.DropDownPadding;
            AutoWidth = control.ListAutoWidth;
            ItemOS = new ItemIndex(items);
            sf = Helper.SF(control.DropDownTextAlign);
            sf.FormatFlags = StringFormatFlags.NoWrap;

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = control.DropDownRadius ?? control.radius;
            else
            {
                ArrowSize = (int)(8 * dpi);
                Radius = (int)(control.DropDownRadius ?? control.radius * dpi);
            }
            Items = LoadLayout(AutoWidth, control.ReadRectangle.Width, ItemOS.List, filtertext, true);

            var tmpAlign = CLocation(control, control.Placement, control.DropDownArrow, ArrowSize);
            if (control.DropDownArrow) ArrowAlign = tmpAlign;
            Init();
        }
        public LayeredFormSelectDown(Dropdown control, IList<object> items)
        {
            keyid = nameof(Dropdown);
            PARENT = control;
            Font = control.Font;
            ColorScheme = control.ColorScheme;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = control.SelectedValue;
            ClickEnd = control.ClickEnd;
            MaxCount = control.MaxCount;
            DPadding = control.DropDownPadding;
            AutoWidth = control.ListAutoWidth;
            ItemOS = new ItemIndex(items);
            sf = Helper.SF(control.DropDownTextAlign);
            sf.FormatFlags = StringFormatFlags.NoWrap;

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = control.DropDownRadius ?? control.Radius;
            else
            {
                ArrowSize = (int)(8 * dpi);
                Radius = (int)(control.DropDownRadius ?? control.Radius * dpi);
            }
            Items = LoadLayout(AutoWidth, control.ReadRectangle.Width, ItemOS.List, null, true);

            var tmpAlign = CLocation(control, control.Placement, control.DropDownArrow, ArrowSize);
            if (control.DropDownArrow) ArrowAlign = tmpAlign;
            Init();
        }
        public LayeredFormSelectDown(Tabs control, IList<object> items, int radius, object? sValue, Rectangle rect)
        {
            keyid = nameof(Tabs);
            PARENT = control;
            Font = control.Font;
            ColorScheme = control.ColorScheme;
            MessageCloseMouseLeave = true;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            MaxCount = 7;
            DPadding = new Size(12, 5);
            selectedValue = sValue;
            ItemOS = new ItemIndex(items);
            sf = Helper.SF(TAlign.Left);
            sf.FormatFlags = StringFormatFlags.NoWrap;

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = radius;
            else
            {
                ArrowSize = (int)(8 * dpi);
                Radius = (int)(radius * dpi);
            }
            Items = LoadLayout(AutoWidth, 0, ItemOS.List, null, true);
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
            CLocation(control, align, rect, false, ArrowSize, true);
            Init();
        }
        public LayeredFormSelectDown(Table control, IList<object> items, ICell cell, Rectangle rect)
        {
            keyid = nameof(Table);
            PARENT = control;
            Font = control.Font;
            ColorScheme = control.ColorScheme;
            Tag = cell;
            MessageCloseMouseLeave = true;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = cell.DropDownValue;
            ClickEnd = cell.DropDownClickEnd;
            MaxCount = cell.DropDownMaxCount;
            DPadding = cell.DropDownPadding;
            ItemOS = new ItemIndex(items);
            sf = Helper.SF(cell.DropDownTextAlign);
            sf.FormatFlags = StringFormatFlags.NoWrap;

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = cell.DropDownRadius ?? control.Radius;
            else
            {
                ArrowSize = (int)(8 * dpi);
                Radius = (int)(cell.DropDownRadius ?? control.Radius * dpi);
            }
            Items = LoadLayout(AutoWidth, 0, ItemOS.List, null, true);

            var tmpAlign = CLocation(control, cell.DropDownPlacement, rect, cell.DropDownArrow, ArrowSize, true);
            if (cell.DropDownArrow) ArrowAlign = tmpAlign;
            Init();
        }

        SubLayeredForm? lay;

        #region 子

        float tmpItemHeight = 0F;
        public LayeredFormSelectDown(Select control, int sx, LayeredFormSelectDown parent, int radius, int arrowSize, float itemHeight, Rectangle rect, IList<object> items, int sel = -1)
        {
            select_x = sx;
            keyid = nameof(AntdUI.Select);
            PARENT = control;
            Font = control.Font;
            lay = parent;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = control.SelectedValue;
            ClickEnd = parent.ClickEnd;
            CloseIcon = parent.CloseIcon;
            DropNoMatchClose = control.DropDownEmptyClose;
            DPadding = parent.DPadding;
            ItemOS = new ItemIndex(items);
            sf = Helper.SF(control.DropDownTextAlign);
            sf.FormatFlags = StringFormatFlags.NoWrap;

            Radius = radius;
            ArrowSize = arrowSize;

            control.Disposed += (a, b) => Dispose();

            Items = LoadLayout(AutoWidth, 0, ItemOS.List, null, true);
            tmpItemHeight = itemHeight;
            var tmpAlign = CLocation(parent, rect, control.DropDownArrow, ArrowSize);
            if (control.DropDownArrow) ArrowAlign = tmpAlign;
            Init();
        }
        public LayeredFormSelectDown(Dropdown control, int sx, LayeredFormSelectDown parent, int radius, int arrowSize, float itemHeight, Rectangle rect, IList<object> items, int sel = -1)
        {
            select_x = sx;
            keyid = nameof(Dropdown);
            PARENT = control;
            Font = control.Font;
            lay = parent;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = control.SelectedValue;
            ClickEnd = parent.ClickEnd;
            CloseIcon = parent.CloseIcon;
            DPadding = parent.DPadding;
            ItemOS = new ItemIndex(items);
            sf = Helper.SF(control.DropDownTextAlign);
            sf.FormatFlags = StringFormatFlags.NoWrap;

            Radius = radius;
            ArrowSize = arrowSize;

            control.Disposed += (a, b) => Dispose();

            Items = LoadLayout(AutoWidth, 0, ItemOS.List, null, true);
            tmpItemHeight = itemHeight;
            var tmpAlign = CLocation(parent, rect, control.DropDownArrow, ArrowSize);
            if (control.DropDownArrow) ArrowAlign = tmpAlign;
            Init();
        }
        public LayeredFormSelectDown(Table control, ICell cell, int sx, LayeredFormSelectDown parent, int radius, int arrowSize, float itemHeight, Rectangle rect, IList<object> items, int sel = -1)
        {
            select_x = sx;
            keyid = nameof(Table);
            PARENT = control;
            Font = control.Font;
            lay = parent;
            Tag = cell;
            control.Parent.SetTopMost(Handle);
            ScrollBar = new ScrollBar(this, control.ColorScheme);
            selectedValue = cell.DropDownValue;
            ClickEnd = parent.ClickEnd;
            CloseIcon = parent.CloseIcon;
            DPadding = parent.DPadding;
            ItemOS = new ItemIndex(items);
            sf = Helper.SF(cell.DropDownTextAlign);
            sf.FormatFlags = StringFormatFlags.NoWrap;

            Radius = radius;
            ArrowSize = arrowSize;

            control.Disposed += (a, b) => Dispose();

            Items = LoadLayout(AutoWidth, 0, ItemOS.List, null, true);
            tmpItemHeight = itemHeight;
            var tmpAlign = CLocation(parent, rect, cell.DropDownArrow, ArrowSize);
            if (cell.DropDownArrow) ArrowAlign = tmpAlign;
            Init();
        }

        #endregion

        void Init()
        {
            if (OS.Win7OrLower) Select();
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
                            if (it.SID && OnClick(it)) return true;
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
                        }
                        while (!Items[hoveindex].SID)
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

        #endregion

        #region 参数

        public override string name => keyid;

        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        ScrollBar ScrollBar;
        bool nodata = false;

        public ILayeredForm? SubForm() => subForm;
        LayeredFormSelectDown? subForm;

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
            if (nodata) g.PaintEmpty(rect, Font, Color.FromArgb(180, Colour.Text.Get(keyid, ColorScheme)));
            else
            {
                int sy = ScrollBar.Value;
                g.TranslateTransform(0, -sy);
                using (var brush = new SolidBrush(Colour.Text.Get(keyid, ColorScheme)))
                using (var brush_back_hover = new SolidBrush(Colour.FillTertiary.Get(keyid, ColorScheme)))
                using (var brush_sub = new SolidBrush(Colour.TextQuaternary.Get(keyid, ColorScheme)))
                using (var brush_fore = new SolidBrush(Colour.TextTertiary.Get(keyid, ColorScheme)))
                using (var brush_split = new SolidBrush(Colour.Split.Get(keyid, ColorScheme)))
                {
                    foreach (var it in Items)
                    {
                        if (it.Rect.Bottom < sy || it.Rect.Top > sy + rect.Height) continue;
                        DrawItem(g, brush, brush_sub, brush_back_hover, brush_fore, brush_split, it);
                    }
                    g.Restore(state);
                    ScrollBar.Paint(g);
                }
            }
        }

        void DrawItem(Canvas g, SolidBrush brush, SolidBrush subbrush, SolidBrush brush_back_hover, SolidBrush brush_fore, SolidBrush brush_split, ObjectItem it)
        {
            if (it.SID)
            {
                if (it.Group) g.DrawText(it.Text, Font, brush_fore, it.RectText, sf);
                else if (selectedValue == it.Tag)
                {
                    using (var path = it.Rect.RoundPath(Radius))
                    {
                        using (var bg = it.BackActiveExtend.BrushEx(it.Rect, it.BackActive ?? Colour.PrimaryBg.Get(keyid, ColorScheme)))
                        {
                            g.Fill(bg, path);
                        }
                    }
                    if (it.SubText != null)
                    {
                        var size = g.MeasureText(it.Text, Font);
                        var rectSubText = new Rectangle(it.RectText.X + size.Width, it.RectText.Y, it.RectText.Width - size.Width, it.RectText.Height);
                        if (it.ForeSub.HasValue) g.DrawText(it.SubText, Font, it.ForeSub.Value, rectSubText, sf);
                        else g.DrawText(it.SubText, Font, subbrush, rectSubText, sf);
                    }
                    DrawTextIconSelect(g, it);
                }
                else
                {
                    if (it.Hover)
                    {
                        using (var path = it.Rect.RoundPath(Radius))
                        {
                            g.Fill(brush_back_hover, path);
                        }
                    }
                    if (it.SubText != null)
                    {
                        var size = g.MeasureText(it.Text, Font);
                        var rectSubText = new Rectangle(it.RectText.X + size.Width, it.RectText.Y, it.RectText.Width - size.Width, it.RectText.Height);
                        if (it.ForeSub.HasValue) g.DrawText(it.SubText, Font, it.ForeSub.Value, rectSubText, sf);
                        else g.DrawText(it.SubText, Font, subbrush, rectSubText, sf);
                    }
                    DrawTextIcon(g, it, brush, it.Fore);
                }
                if (it.Online.HasValue)
                {
                    Color color = it.OnlineCustom ?? (it.Online == 1 ? Colour.Success.Get(keyid, ColorScheme) : Colour.Error.Get(keyid, ColorScheme));
                    using (var brush_online = new SolidBrush(it.Enable ? color : Color.FromArgb(Colour.TextQuaternary.Get(keyid, ColorScheme).A, color)))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
                if (it.HasSub) DrawArrow(g, it, Colour.TextBase.Get(keyid, ColorScheme));
                else if (CloseIcon)
                {
                    if (it.HoverClose)
                    {
                        using (var path = it.RectClose.RoundPath((int)(4 * Config.Dpi)))
                        {
                            g.Fill(Colour.FillSecondary.Get(keyid, ColorScheme), path);
                        }
                        g.PaintIconClose(it.RectClose, Colour.Text.Get(keyid, ColorScheme), .7F);
                    }
                    else g.PaintIconClose(it.RectClose, Colour.TextTertiary.Get(keyid, ColorScheme), .7F);
                }
            }
            else g.Fill(brush_split, it.Rect);
        }
        void DrawTextIconSelect(Canvas g, ObjectItem it)
        {
            using (var font = new Font(Font, FontStyle.Bold))
            {
                if (it.Enable)
                {
                    using (var fore = new SolidBrush(Colour.TextBase.Get(keyid, ColorScheme)))
                    {
                        g.DrawText(it.Text, font, fore, it.RectText, sf);
                    }
                }
                else
                {
                    using (var fore = new SolidBrush(Colour.TextQuaternary.Get(keyid, ColorScheme)))
                    {
                        g.DrawText(it.Text, font, fore, it.RectText, sf);
                    }
                }
            }
            DrawIcon(g, it, Colour.TextBase.Get(keyid, ColorScheme));
        }
        void DrawTextIcon(Canvas g, ObjectItem it, SolidBrush brush, Color? color)
        {
            if (it.Enable)
            {
                if (color.HasValue) g.DrawText(it.Text, Font, color.Value, it.RectText, sf);
                else g.DrawText(it.Text, Font, brush, it.RectText, sf);
            }
            else
            {
                using (var fore = new SolidBrush(Colour.TextQuaternary.Get(keyid, ColorScheme)))
                {
                    g.DrawText(it.Text, Font, fore, it.RectText, sf);
                }
            }
            DrawIcon(g, it, color ?? brush.Color);
        }
        void DrawIcon(Canvas g, ObjectItem it, Color color)
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
        void DrawArrow(Canvas g, ObjectItem it, Color color)
        {
            var state = g.Save();
            int size = it.RectArrow.Width, size_arrow = size / 2;
            g.TranslateTransform(it.RectArrow.X + size_arrow, it.RectArrow.Y + size_arrow);
            g.RotateTransform(-90F);
            using (var pen = new Pen(color, Config.Dpi * 1.4F))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, it.RectArrow.Width, it.RectArrow.Height).TriangleLines(-1, .7F));
            }
            g.Restore(state);
        }

        #endregion

        #region 布局

        int tmpW = 0, tmp_padd = 0;
        List<ObjectItem> LoadLayout(bool autoWidth, int width, IList<object> items, string? search, bool init = false) => Helper.GDI(g => LoadLayout(g, autoWidth, width, SearchList(items, search), init));
        List<ObjectItem> LoadLayout(Canvas g, bool autoWidth, int width, IList<object> items, bool init)
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

                int item_count = 0, divider_count = 0, y = 0, sy = 0;
                var lists = new List<ObjectItem>(items.Count);
                foreach (var value in items)
                {
                    int index = ItemOS[value];
                    if (value is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
                    {
                        item_count++;
                        var rect = new Rectangle(padd, padd + y, maxwr, item_height);
                        lists.Add(new ObjectItem(group, index, rect, new Rectangle(rect.X + gap_x, rect.Y, rect.Width - gap_x2, rect.Height)));
                        y += item_height;
                        for (int i = 0; i < group.Sub.Count; i++)
                        {
                            var sub = group.Sub[i];
                            lists.Add(ItemC(sub, i, ref item_count, ref divider_count, ref y, padd, padd2, sp, gap_x, gap_x2, icon_size, icon_gap, icon_xy, item_height, text_height, maxwr, ref sy, false));
                        }
                    }
                    else lists.Add(ItemC(value, index, ref item_count, ref divider_count, ref y, padd, padd2, sp, gap_x, gap_x2, icon_size, icon_gap, icon_xy, item_height, text_height, maxwr, ref sy));
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
                else if (animateConfig.Inverted)
                {
                    var tr = TargetRect;
                    SetLocationY(tr.Y - (h - (tr.Height - shadow2)));
                }
                SetSize(w, h);
                return lists;
            }
            else
            {
                nodata = true;
                int w = width, h = text_height * 5;
                if (autoWidth) w = (int)(g.MeasureText(Localization.Get("NoData", "暂无数据"), Font).Width * 1.6F);
                if (init) tmpW = w;
                else if (animateConfig.Inverted)
                {
                    var tr = TargetRect;
                    SetLocationY(tr.Y - (h - (tr.Height - shadow2)));
                }
                if (DropNoMatchClose) IClose();
                else SetSize(w, h);
                return new List<ObjectItem>(0);
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
                else if (CloseIcon) tmp += text_height + icon_gap;
                return tmp;
            }
            else if (obj is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0) return ItemMaxWidth(g, group.Sub, text_height, icon_size, icon_gap);
            else if (obj is DividerSelectItem) return 0;
            else
            {
                var text = obj.ToString();
                if (text == null) return 0;
                var tmp = g.MeasureText(text, Font).Width;
                if (CloseIcon) tmp += text_height + icon_gap;
                return tmp;
            }
        }
        ObjectItem ItemC(object value, int i, ref int item_count, ref int divider_count, ref int y, int padd, int padd2, int sp, int gap_x, int gap_x2, int icon_size, int icon_gap, int icon_xy, int item_height, int text_height, int maxwr, ref int sy, bool no_id = true)
        {
            ObjectItem item;
            if (value is DividerSelectItem)
            {
                divider_count++;
                item = new ObjectItem(i, new Rectangle(padd, padd2 + y, maxwr, sp));
                y += padd2 + sp;
            }
            else
            {
                item_count++;
                var rect = new Rectangle(padd, padd + y, maxwr, item_height);
                if (value is SelectItem it)
                {
                    int ux = gap_x, uw = gap_x2;
                    item = new ObjectItem(it, i, rect) { NoIndex = no_id };
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
                    else if (CloseIcon)
                    {
                        int dot_xy = (item_height - text_height) / 2;
                        item.RectClose = new Rectangle(rect.Right - gap_x - text_height + dot_xy, rect.Y + dot_xy, text_height, text_height);
                        uw += text_height;
                    }
                    item.RectText = new Rectangle(rect.X + ux, rect.Y, rect.Width - uw, rect.Height);
                }
                else
                {
                    if (CloseIcon)
                    {
                        int dot_xy = (item_height - text_height) / 2;
                        var rect_close = new Rectangle(rect.Right - gap_x - text_height + dot_xy, rect.Y + dot_xy, text_height, text_height);
                        item = new ObjectItem(value, i, rect, new Rectangle(rect.X + gap_x, rect.Y, rect.Width - gap_x2 - text_height, rect.Height)) { NoIndex = no_id, RectClose = rect_close };
                    }
                    else item = new ObjectItem(value, i, rect, new Rectangle(rect.X + gap_x, rect.Y, rect.Width - gap_x2, rect.Height)) { NoIndex = no_id };
                }
                if (selectedValue == item.Tag) sy = y;
                y += item_height;
            }
            return item;
        }

        public void FocusItem(ObjectItem item)
        {
            if (item.SetHover(true))
            {
                if (ScrollBar.ShowY) ScrollBar.ValueY = item.Rect.Y - item.Rect.Height;
                Print();
            }
        }

        #endregion

        #region 鼠标

        bool down = false;
        protected override void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseDown(x, y))
            {
                OnTouchDown(x, y);
                down = true;
            }
        }

        int hoveindex = -1, hoveindexold = -1;
        protected override void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseMove(x, y) && OnTouchMove(x, y))
            {
                hoveindex = -1;
                int count = 0, hand = 0, sy = ScrollBar.Value;
                if (CloseIcon)
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        var it = Items[i];
                        if (it.Enable)
                        {
                            if (it.HasSub)
                            {
                                if (it.Contains(x, y, 0, sy, out var change))
                                {
                                    if (!it.Group) hand++;
                                    hoveindex = i;
                                }
                                if (change) count++;
                            }
                            else
                            {
                                if (it.Contains(x, y, 0, sy, out var change))
                                {
                                    if (!it.Group) hand++;
                                    hoveindex = i;
                                    bool hover = it.RectClose.Contains(x, y + sy);
                                    if (it.HoverClose == hover)
                                    {
                                        if (change) count++;
                                    }
                                    else
                                    {
                                        it.HoverClose = hover;
                                        count++;
                                    }

                                }
                                else if (it.HoverClose)
                                {
                                    it.HoverClose = false;
                                    count++;
                                }
                                else if (change) count++;
                            }
                        }
                    }
                }
                else
                {
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
                if (PARENT is Select select) select.select_x = select_x;
                else if (PARENT is Dropdown dropdown) dropdown.select_x = select_x;
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
                if (CloseIcon)
                {
                    foreach (var it in Items)
                    {
                        if (it.Enable && it.SID && it.Contains(x, y, 0, sy, out _))
                        {
                            if (it.RectClose.Contains(x, y + sy) && PARENT is Select select && select.DropDownClose(it.Tag))
                            {
                                IClose();
                                return;
                            }
                            else if (OnClick(it)) return;
                        }
                    }
                }
                else
                {
                    foreach (var it in Items)
                    {
                        if (it.Enable && it.SID && it.Contains(x, y, 0, sy, out _))
                        {
                            if (OnClick(it)) return;
                        }
                    }
                }
            }
            else down = false;
        }
        bool OnClick(ObjectItem it)
        {
            if (!ClickEnd || it.Sub == null || it.Sub.Count == 0)
            {
                selectedValue = it.Tag;
                OnCall(it);
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
            if (PARENT is Select select) select.DropDownChange(select_x, it.I, it.Tag, it.Select, it.Text);
            else if (PARENT is Dropdown dropdown) dropdown.DropDownChange(it.Tag);
            else if (PARENT is Tabs tabs)
            {
                if (it.Tag is TabPage page) tabs.MouseChangeIndex(page);
            }
            else if (Tag is ICell table) table.DropDownValueChanged?.Invoke(it.Tag);
        }

        void OpenDown(ObjectItem it, IList<object> sub, int tag = -1)
        {
            var rect = new Rectangle(it.Rect.X + tmp_padd, it.Rect.Y - ScrollBar.ValueY - tmp_padd, it.Rect.Width, it.Rect.Height);
            if (PARENT is Select select)
            {
                subForm = new LayeredFormSelectDown(select, select_x + 1, this, Radius, ArrowSize, tmp_padd + it.Rect.Height / 2F, rect, sub, tag);
                subForm.Show(this);
            }
            else if (PARENT is Dropdown dropdown)
            {
                subForm = new LayeredFormSelectDown(dropdown, select_x + 1, this, Radius, ArrowSize, tmp_padd + it.Rect.Height / 2F, rect, sub, tag);
                subForm.Show(this);
            }
            else if (PARENT is Table table && Tag is ICell cell)
            {
                subForm = new LayeredFormSelectDown(table, cell, select_x + 1, this, Radius, ArrowSize, tmp_padd + it.Rect.Height / 2F, rect, sub, tag);
                subForm.Show(this);
            }
        }
        public override void IClosing()
        {
            if (select_x == 0)
            {
                var item = this;
                while (item.lay is LayeredFormSelectDown form)
                {
                    if (item == form) return;
                    form.IClose();
                    item = form;
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (RunAnimation) return;
            ScrollBar.Leave();
            SetCursor(false);
            Print();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta) => ScrollBar.MouseWheel(delta);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        #endregion

        #region 筛选

        #region 主动搜索

        /// <summary>
        /// 搜索指定文本
        /// </summary>
        /// <param name="search"></param>
        public void TextChange(string search)
        {
            Items = LoadLayout(AutoWidth, tmpW, ItemOS.List, search);
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