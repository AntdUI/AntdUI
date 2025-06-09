﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormSelectMultiple : ISelectMultiple
    {
        #region 变量

        /// <summary>
        /// 是否显示暂无数据
        /// </summary>
        bool nodata = false;

        internal ScrollY scrollY;

        #endregion

        #region 初始化

        int MaxCount = 4, MaxChoiceCount = 4;
        Size DPadding;
        bool DropNoMatchClose = false;
        internal float Radius = 0;
        internal List<object> selectedValue;
        int r_w = 0;
        List<ObjectItem> Items;
        ObjectItem[]? ItemsSearch;
        TAMode ColorScheme;
        TAlign DropDownTextAlign = TAlign.Left;
        public LayeredFormSelectMultiple(SelectMultiple control, Rectangle rect_read, IList<object> items, string filtertext)
        {
            ColorScheme = control.ColorScheme;
            control.Parent.SetTopMost(Handle);
            PARENT = control;
            scrollY = new ScrollY(this);
            DropNoMatchClose = control.DropDownEmptyClose;
            MaxCount = control.MaxCount;
            MaxChoiceCount = control.MaxChoiceCount;
            Font = control.Font;
            selectedValue = new List<object>(control.SelectedValue.Length);
            selectedValue.AddRange(control.SelectedValue);
            Radius = (int)(control.radius * Config.Dpi);
            DPadding = control.DropDownPadding;
            DropDownTextAlign = control.DropDownTextAlign;
            sf = Helper.SF(DropDownTextAlign);
            Items = new List<ObjectItem>(items.Count);
            Init(control, control.Placement, control.DropDownArrow, control.ListAutoWidth, rect_read, items, filtertext);
        }

        public override string name => nameof(AntdUI.Select);

        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        void Init(SelectMultiple control, TAlignFrom Placement, bool ShowArrow, bool ListAutoWidth, Rectangle rect_read, IList<object> items, string? filtertext = null)
        {
            if (OS.Win7OrLower) Select();
            int y = 10, w = rect_read.Width;
            r_w = w;
            int r_h;
            if (items.Count > 0)
            {
                Helper.GDI(g =>
                {
                    var size = g.MeasureString(Config.NullText, Font);
                    int sp = (int)Config.Dpi, gap = (int)(4 * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi), gap_x = (int)(DPadding.Width * Config.Dpi),
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
                        w = r_w = b_w + gap_x2 + gap2 + gap_y2;
                    }
                    else sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.FormatFlags = StringFormatFlags.NoWrap;

                    int selY = -1;
                    int item_count = 0, divider_count = 0;
                    for (int i = 0; i < items.Count; i++) ReadList(items[i], i, w, item_height, text_height, gap, gap2, gap_x, gap_x2, gap_y, gap_y2, sp, ref item_count, ref divider_count, ref y, ref selY);
                    var vr = (item_height * item_count) + (gap_y * divider_count);
                    if (Items.Count > MaxCount)
                    {
                        y = 10 + gap2 + (item_height * MaxCount);
                        scrollY.Rect = new Rectangle(w - gap, 10 + gap, 20, (item_height * MaxCount));
                        scrollY.Show = true;
                        scrollY.SetVrSize(vr, scrollY.Rect.Height);
                        if (selY > -1) scrollY.val = scrollY.SetValue(selY - 10 - gap_y);
                    }
                    else y = 10 + gap2 + vr;
                });
                if (filtertext == null || string.IsNullOrEmpty(filtertext)) r_h = y + 10;
                else r_h = TextChangeCore(filtertext);
            }
            else
            {
                nodata = true;
                if (DropNoMatchClose)
                {
                    IClose();
                    return;
                }
                if (ListAutoWidth) r_w = w = (int)(136 * Config.Dpi);
                else
                {
                    int er = (int)(136 * Config.Dpi);
                    if (w < er) r_w = w = er;
                }
                r_h = (int)(100 * Config.Dpi);
            }
            SetSize(w + 20, r_h);
            var point = control.PointToScreen(Point.Empty);
            MyPoint(point, control, Placement, ShowArrow, rect_read);

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

        void MyPoint(Point point, Control control, TAlignFrom Placement, bool ShowArrow, Rectangle rect_read) => CLocation(point, Placement, ShowArrow, 10, r_w + 20, TargetRect.Height, rect_read, ref Inverted, ref ArrowAlign);

        void MyPoint(SelectMultiple control) => MyPoint(control.PointToScreen(Point.Empty), control, control.Placement, control.DropDownArrow, control.ReadRectangle);

        #endregion

        #region 渲染

        StringFormat sf;
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
                    using (var brush = new SolidBrush(Colour.BgElevated.Get("Select", ColorScheme)))
                    {
                        g.Fill(brush, path);
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                    }
                    if (nodata) g.PaintEmpty(rect_read, Font, Color.FromArgb(180, Colour.Text.Get("Select", ColorScheme)));
                    else
                    {
                        g.SetClip(path);
                        g.TranslateTransform(0, -scrollY.Value);
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
                                    if (it.Show)
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

        void DrawItemSelect(Canvas g, SolidBrush subbrush, SolidBrush brush_split, ObjectItem it, bool TL, bool TR, bool BR, bool BL)
        {
            if (it.ID == -1) g.Fill(brush_split, it.Rect);
            else
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
                g.PaintIconCore(new Rectangle(it.Rect.Right - it.Rect.Height, it.Rect.Y, it.Rect.Height, it.Rect.Height), SvgDb.IcoSuccessGhost, Colour.Primary.Get("Select", ColorScheme), .46F);
                if (it.Online.HasValue)
                {
                    using (var brush_online = new SolidBrush(it.OnlineCustom ?? (it.Online == 1 ? Colour.Success.Get("Select", ColorScheme) : Colour.Error.Get("Select", ColorScheme))))
                    {
                        g.FillEllipse(brush_online, it.RectOnline);
                    }
                }
            }
        }

        void DrawItem(Canvas g, SolidBrush brush, SolidBrush subbrush, SolidBrush brush_back_hover, SolidBrush brush_fore, SolidBrush brush_split, ObjectItem it)
        {
            if (it.ID == -1) g.Fill(brush_split, it.Rect);
            else if (it.Group) g.DrawText(it.Text, Font, brush_fore, it.RectText, sf);
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
            }
        }
        void DrawItemR(Canvas g, SolidBrush brush, SolidBrush brush_back_hover, SolidBrush brush_split, ObjectItem it)
        {
            if (it.ID == -1) g.Fill(brush_split, it.Rect);
            else if (selectedValue.Contains(it.Val) || it.Val is SelectItem item && selectedValue.Contains(item.Tag))
            {
                using (var brush_back = new SolidBrush(Colour.PrimaryBg.Get("Select", ColorScheme)))
                {
                    g.Fill(brush_back, it.Rect);
                }
                DrawTextIconSelect(g, it);
                g.PaintIconCore(new Rectangle(it.Rect.Right - it.Rect.Height, it.Rect.Y, it.Rect.Height, it.Rect.Height), SvgDb.IcoSuccessGhost, Colour.Primary.Get("Select", ColorScheme), .46F);
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
        }

        void DrawTextIconSelect(Canvas g, ObjectItem it)
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
                using (var fore = new SolidBrush(Colour.TextQuaternary.Get("Select", ColorScheme)))
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

        SafeBitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">客户区域</param>
        void DrawShadow(Canvas g, Rectangle rect)
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
                g.Image(shadow_temp.Bitmap, rect, .2F);
            }
        }

        #endregion

        #region 鼠标

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
                int sy = (int)scrollY.Value;
                foreach (var it in Items)
                {
                    if (it.Show && it.Enable && it.ID > -1 && it.Contains(e.X, e.Y, 0, sy, out _))
                    {
                        OnClick(it);
                        return;
                    }
                }
            }
            down = false;
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RunAnimation) return;
            hoveindex = -1;
            if (scrollY.MouseMove(e.Location) && OnTouchMove(e.X, e.Y))
            {
                int count = 0, sy = (int)scrollY.Value;
                for (int i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    if (it.Enable)
                    {
                        if (it.Contains(e.X, e.Y, 0, sy, out var change)) hoveindex = i;
                        if (change) count++;
                    }
                }
                if (count > 0) Print();
            }
            base.OnMouseMove(e);
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

        #region 滚动条

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (RunAnimation) return;
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollY(int value) => scrollY.MouseWheelCore(value);

        #endregion

        #endregion

        #region 布局

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
                    Items.Add(new ObjectItem(it, i, rect, rect_text, false, gap_x, gap_x2, gap_y, gap_y2) { NoIndex = NoIndex });
                    if (selectedValue == it.Tag) select_y = y;
                    y += item_height;
                }
                else if (value is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
                {
                    Items.Add(new ObjectItem(group, i, rect, rect_text, false, gap_x, gap_x2, gap_y, gap_y2));
                    if (selectedValue == value) select_y = y;
                    y += item_height;
                    foreach (var item in group.Sub) ReadList(item, i, width, item_height, text_height, gap, gap2, gap_x, gap_x2, gap_y, gap_y2, sp, ref item_count, ref divider_count, ref y, ref select_y, false);
                }
                else
                {
                    Items.Add(new ObjectItem(value, i, rect, rect_text, false, gap_x, gap_x2, gap_y, gap_y2) { NoIndex = NoIndex });
                    if (selectedValue == value) select_y = y;
                    y += item_height;
                }
            }
        }

        void InitReadList(Canvas g, object obj, ref int btext, ref bool ui_online, ref bool ui_icon, ref bool ui_arrow)
        {
            if (obj is SelectItem it)
            {
                string text = it.Text + it.SubText;
                var size = g.MeasureText(text, Font);
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
                var size = g.MeasureText(text, Font);
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

        public override void TextChange(string val)
        {
            ItemsSearch = null;
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
                int showcount = 0;
                var listSearch = new List<ItemSearchWeigth<ObjectItem>>(Items.Count);
                for (int i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    if (it.ID > -1)
                    {
                        int score = it.Contains(val, out var select);
                        if (score > 0)
                        {
                            listSearch.Add(new ItemSearchWeigth<ObjectItem>(score, it));
                            showcount++;
                            if (select)
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
                ItemsSearch = listSearch.SearchWeightSortArray();
                nodata = showcount == 0;
                if (DropNoMatchClose && nodata)
                {
                    IClose();
                    return;
                }
            }
            if (count > 0)
            {
                int height;
                if (nodata)
                {
                    height = (int)(100 * Config.Dpi);
                    SetSizeH(height);
                }
                else
                {
                    scrollY.val = 0;
                    int y = 10, w = r_w, list_count = 0;
                    Helper.GDI(g =>
                    {
                        var size = g.MeasureString(Config.NullText, Font);
                        int sp = (int)Config.Dpi, gap = (int)(4 * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi), gap_x = (int)(DPadding.Width * Config.Dpi),
                        gap2 = gap * 2, gap_x2 = gap_x * 2, gap_y2 = gap_y * 2,
                        text_height = size.Height, item_height = text_height + gap_y2;
                        y += gap;
                        foreach (var it in GetItems())
                        {
                            if (it.ID > -1 && it.Show)
                            {
                                list_count++;
                                Rectangle rect_bg = new Rectangle(10 + gap, y, w - gap2, item_height),
                                rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x2, text_height);
                                it.SetRectAuto(rect_bg, rect_text, false, gap_x, gap_x2, gap_y, gap_y2);
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
                if (PARENT is SelectMultiple control) MyPoint(control);
                shadow_temp?.Dispose();
                shadow_temp = null;
                Print();
            }
        }
        internal int TextChangeCore(string val)
        {
            ItemsSearch = null;
            if (string.IsNullOrEmpty(val))
            {
                nodata = false;
                foreach (var it in Items) it.Show = true;
            }
            else
            {
                int showcount = 0;
                var listSearch = new List<ItemSearchWeigth<ObjectItem>>(Items.Count);
                for (int i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    if (it.ID > -1)
                    {
                        int score = it.Contains(val, out var select);
                        if (score > 0)
                        {
                            listSearch.Add(new ItemSearchWeigth<ObjectItem>(score, it));
                            showcount++;
                            if (select)
                            {
                                it.Hover = true;
                                hoveindex = i;
                            }
                            it.Show = true;
                        }
                        else it.Show = false;
                    }
                }
                ItemsSearch = listSearch.SearchWeightSortArray();
                nodata = showcount == 0;
                if (DropNoMatchClose && nodata)
                {
                    IClose();
                    return 1;
                }
            }
            if (nodata) return (int)(100 * Config.Dpi);
            else
            {
                scrollY.val = 0;
                int y = 10, w = r_w, list_count = 0;
                Helper.GDI(g =>
                {
                    var size = g.MeasureString(Config.NullText, Font);
                    int sp = (int)Config.Dpi, gap = (int)(4 * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi), gap_x = (int)(DPadding.Width * Config.Dpi),
                    gap2 = gap * 2, gap_x2 = gap_x * 2, gap_y2 = gap_y * 2,
                    text_height = size.Height, item_height = text_height + gap_y2;
                    y += gap;
                    foreach (var it in GetItems())
                    {
                        if (it.ID > -1 && it.Show)
                        {
                            list_count++;
                            Rectangle rect_bg = new Rectangle(10 + gap, y, w - gap2, item_height),
                            rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x2, text_height);
                            it.SetRectAuto(rect_bg, rect_text, false, gap_x, gap_x2, gap_y, gap_y2);
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

        IList<ObjectItem> GetItems()
        {
            if (ItemsSearch == null) return Items;
            else return ItemsSearch;
        }

        #endregion
    }

    internal abstract class ISelectMultiple : ILayeredFormOpacityDown
    {
        public virtual void SetValues(object[] value) { }
        public virtual void SetValues(List<object> value) { }
        public virtual void ClearValues() { }
        public virtual void TextChange(string val)
        { }
    }
}