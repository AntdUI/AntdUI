// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormMenuDown : ILayeredShadowForm, SubLayeredForm
    {
        #region 初始化

        TAMode ColorScheme;
        bool isdark = false;
        List<IMenuItem> Items;
        Size DPadding;
        Color? backColor, BackHover, BackActive, foreColor, ForeActive;
        float IconRatio = 0.7F, IconGap = 0.25F;

        ScrollBar ScrollBar;
        public LayeredFormMenuDown(Menu control, int radius, Rectangle rect, IList<MenuItem> items)
        {
            CloseMode = CloseMode.Leave;
            ColorScheme = control.ColorScheme;
            isdark = Config.IsDark || control.ColorScheme == TAMode.Dark;
            control.Parent.SetTopMost(Handle);
            SetDpi(control);
            PARENT = control;
            select_x = 0;
            Font = control.Font;
            if (control.ShowSubBack) backColor = control.BackColor;
            foreColor = control.ForeColor;
            ForeActive = control.ForeActive;
            BackHover = control.BackHover;
            BackActive = control.BackActive;
            DPadding = control.DropDownPadding;
            IconRatio = control.DropIconRatio;
            IconGap = control.DropIconGap;
            Radius = (int)(radius * Dpi);
            ScrollBar = new ScrollBar(this, ColorScheme);
            var point = control.PointToScreen(Point.Empty);
            Items = LoadLayout(items, point);
            var screen = Screen.FromPoint(point).WorkingArea;
            int offsetX = (int)(control.DropDownOffset.Width * Dpi), offsetY = (int)(control.DropDownOffset.Height * Dpi);
            int x, y;
            if (control.IsModeHorizontal)
            {
                // 水平模式：子菜单显示在主菜单项下方
                x = point.X + rect.X;
                y = point.Y + rect.Bottom;

                // 左右对齐判断
                if (x + TargetRect.Width > screen.Right) x = point.X + rect.Right - TargetRect.Width;

                // 上下方向判断：计算可用空间并决定显示方向
                int spaceBelow = screen.Bottom - y - offsetY, spaceAbove = point.Y + rect.Y - screen.Top - offsetY;
                if ((spaceBelow < TargetRect.Height && spaceAbove > spaceBelow && spaceAbove >= TargetRect.Height))
                {
                    animateConfig.Inverted = true;
                    y = point.Y + rect.Y - TargetRect.Height + shadow2 - offsetY; // 向上
                }
                else y += offsetY; // 向下
                x += offsetX;
            }
            else
            {
                // 垂直模式：子菜单显示在主菜单项右侧
                x = point.X + control.Width - rect.X - shadow + offsetX;
                y = point.Y + rect.Y + shadow + offsetY;
            }

            // 边界限制
            x = System.Math.Max(screen.Left, System.Math.Min(x, screen.Right - TargetRect.Width));
            y = System.Math.Max(screen.Top, System.Math.Min(y, screen.Bottom - TargetRect.Height));

            SetLocationO(x, y);
            Init();
        }

        SubLayeredForm? lay;
        object? Guid;
        public LayeredFormMenuDown(Menu control, int sx, LayeredFormMenuDown parent, int radius, float itemHeight, Rectangle rect, object guid, MenuItemCollection items)
        {
            Guid = guid;
            ColorScheme = control.ColorScheme;
            isdark = Config.IsDark || control.ColorScheme == TAMode.Dark;
            control.Parent.SetTopMost(Handle);
            SetDpi(control);
            select_x = sx;
            PARENT = control;
            Font = parent.Font;
            lay = parent;
            backColor = parent.backColor;
            foreColor = parent.foreColor;
            ForeActive = parent.ForeActive;
            BackHover = parent.BackHover;
            BackActive = parent.BackActive;
            DPadding = parent.DPadding;
            IconRatio = parent.IconRatio;
            IconGap = parent.IconGap;
            Radius = radius;
            parent.Disposed += (a, b) => Dispose();
            ScrollBar = new ScrollBar(this, ColorScheme);
            Items = LoadLayout(items, control.PointToScreen(Point.Empty));

            CLocation(parent, rect, false, 0);
            Init();
        }

        public override string name => nameof(AntdUI.Menu);

        public ILayeredForm? SubForm() => subForm;
        LayeredFormMenuDown? subForm;
        public override bool EnableSafetyTriangleZone => true;
        public override Rectangle? OnSafetyTriangleZone(int x, int y) => subForm?.TargetRect;

        void Init()
        {
            if (OS.Win7OrLower) Select();
            KeyCall = keys =>
            {
                int _select_x = -1;
                if (PARENT is Menu manu) _select_x = manu.select_x;
                if (select_x == _select_x)
                {
                    if (keys == Keys.Escape)
                    {
                        IClose();
                        return true;
                    }
                    if (keys == Keys.Enter)
                    {
                        if (hoveindex > -1)
                        {
                            if (Items[hoveindex] is OMenuItem it && OnClick(it)) return true;
                        }
                    }
                    else if (keys == Keys.Up)
                    {
                        int tmp = IMouseWheel(1);
                        if (tmp == hoveindex) return true;
                        hoveindex = tmp;
                        foreach (var item in Items) item.Hover = false;
                        if (Items[hoveindex] is OMenuItem it) FocusItem(it);
                        return true;
                    }
                    else if (keys == Keys.Down)
                    {
                        int tmp = IMouseWheel(-1);
                        if (tmp == hoveindex) return true;
                        hoveindex = tmp;
                        foreach (var item in Items) item.Hover = false;
                        if (Items[hoveindex] is OMenuItem it) FocusItem(it);
                        return true;
                    }
                    else if (keys == Keys.Left)
                    {
                        if (_select_x > 0)
                        {
                            if (PARENT is Menu menu2) menu2.select_x--;
                            IClose();
                        }
                        return true;
                    }
                    else if (keys == Keys.Right)
                    {
                        if (hoveindex > -1)
                        {
                            if (Items[hoveindex] is OMenuItem it && it.Sub != null && it.Sub.Count > 0)
                            {
                                subForm?.IClose();
                                subForm = null;
                                OpenDown(it);
                                if (PARENT is Menu menu2) menu2.select_x++;
                            }
                        }
                        return true;
                    }
                }
                return false;
            };
        }

        int IMouseWheel(int delta)
        {
            int count = Items.Count, errcount = 0, newIndex;
            if (delta > 0)
            {
                newIndex = hoveindex <= 0 ? Items.Count - 1 : hoveindex - 1;
                while (WheelItem(Items[newIndex]))
                {
                    errcount++;
                    if (errcount > count) return -1;
                    newIndex--;
                    if (newIndex < 0) newIndex = count - 1;
                }
            }
            else
            {
                newIndex = hoveindex >= Items.Count - 1 ? 0 : hoveindex + 1;
                while (WheelItem(Items[newIndex]))
                {
                    errcount++;
                    if (errcount > count) return -1;
                    newIndex++;
                    if (newIndex > count - 1) newIndex = 0;
                }
            }
            return newIndex;
        }
        bool WheelItem(object? it)
        {
            if (it is DMenuItem) return true;
            return false;
        }

        #endregion

        #region 渲染

        readonly FormatFlags sf = FormatFlags.Left | FormatFlags.VerticalCenter;
        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(backColor ?? Colour.BgElevated.Get(name, ColorScheme)))
            {
                g.Fill(brush, path);
                if (shadow == 0)
                {
                    int bor = (int)(Dpi), bor2 = bor * 2;
                    using (var path2 = new Rectangle(rect.X + bor, rect.Y + bor, rect.Width - bor2, rect.Height - bor2).RoundPath(Radius))
                    {
                        g.Draw(Colour.BorderColor.Get(name, ColorScheme), bor, path2);
                    }
                }
            }
        }
        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            if (ScrollBar.ShowY) g.TranslateTransform(0, -ScrollBar.ValueY);
            using (var brush_split = new SolidBrush(Colour.Split.Get(name, ColorScheme)))
            {
                if (foreColor.HasValue)
                {
                    using (var brush = new SolidBrush(foreColor.Value))
                    {
                        foreach (var it in Items)
                        {
                            if (it is OMenuItem menu) DrawItem(g, brush, menu);
                            else g.Fill(brush_split, it.Rect);
                        }
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(Colour.Text.Get(name, ColorScheme)))
                    {
                        foreach (var it in Items)
                        {
                            if (it is OMenuItem menu) DrawItem(g, brush, menu);
                            else g.Fill(brush_split, it.Rect);
                        }
                    }
                }
            }
            g.Restore(state);
            ScrollBar.Paint(g, ColorScheme);
        }

        void DrawItem(Canvas g, SolidBrush brush, OMenuItem it)
        {
            if (it.Val.Enabled)
            {
                if (isdark)
                {
                    if (it.Val.Select)
                    {
                        using (var path = it.Rect.RoundPath(Radius))
                        {
                            g.Fill(BackActive ?? Colour.Primary.Get(name, ColorScheme), path);
                        }
                        using (var brush_select = new SolidBrush(ForeActive ?? Colour.TextBase.Get(name, ColorScheme)))
                        {
                            g.DrawText(it.Val.Text, it.Val.Font ?? Font, brush_select, it.RectText, sf);
                        }
                        PaintIcon(g, it, brush.Color);
                    }
                    else
                    {
                        if (it.Hover)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackHover ?? Colour.FillTertiary.Get(name, ColorScheme), path);
                            }
                        }
                        g.DrawText(it.Val.Text, it.Val.Font ?? Font, brush, it.RectText, sf);
                        PaintIcon(g, it, brush.Color);
                    }
                }
                else
                {
                    if (it.Val.Select)
                    {
                        using (var path = it.Rect.RoundPath(Radius))
                        {
                            g.Fill(BackActive ?? Colour.PrimaryBg.Get(name, ColorScheme), path);
                        }
                        using (var brush_select = new SolidBrush(ForeActive ?? Colour.TextBase.Get(name, ColorScheme)))
                        {
                            g.DrawText(it.Val.Text, it.Val.Font ?? Font, brush_select, it.RectText, sf);
                        }
                    }
                    else
                    {
                        if (it.Hover)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackHover ?? Colour.FillTertiary.Get(name, ColorScheme), path);
                            }
                        }
                        g.DrawText(it.Val.Text, it.Val.Font ?? Font, brush, it.RectText, sf);
                    }
                    PaintIcon(g, it, brush.Color);
                }
            }
            else
            {
                if (it.Val.Select)
                {
                    if (isdark)
                    {
                        using (var path = it.Rect.RoundPath(Radius))
                        {
                            g.Fill(BackActive ?? Colour.Primary.Get(name, ColorScheme), path);
                        }
                    }
                    else
                    {
                        using (var path = it.Rect.RoundPath(Radius))
                        {
                            g.Fill(BackActive ?? Colour.PrimaryBg.Get(name, ColorScheme), path);
                        }
                    }
                }
                using (var fore = new SolidBrush(Colour.TextQuaternary.Get(name, ColorScheme)))
                {
                    g.DrawText(it.Val.Text, it.Val.Font ?? Font, fore, it.RectText, sf);
                }
                PaintIcon(g, it, brush.Color);
            }
            if (it.has_sub) PaintArrow(g, it, brush.Color);
        }
        void PaintIcon(Canvas g, OMenuItem it, Color fore)
        {
            if (it.Val.Icon != null) g.Image(it.Val.Icon, it.RectIcon);
            if (it.Val.IconSvg != null) g.GetImgExtend(it.Val.IconSvg, it.RectIcon, fore);
        }
        void PaintArrow(Canvas g, OMenuItem it, Color color)
        {
            using (var pen = new Pen(color, Dpi * 1.4F))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, it.RectArrow.TriangleLinesHorizontal(-1, .7F));
            }
        }

        #endregion

        #region 布局

        int tmp_padd = 0;
        List<IMenuItem> LoadLayout(IList<MenuItem> items, Point point) => this.GDI(g => LoadLayout(g, items, point));
        List<IMenuItem> LoadLayout(Canvas g, IList<MenuItem> items, Point point)
        {
            var text_height = g.MeasureString(Config.NullText, Font).Height;

            int sp = (int)Dpi, padd = (int)(text_height * .18F), padd2 = padd * 2, gap_x = (int)(DPadding.Width * Dpi), gap_y = (int)(DPadding.Height * Dpi),
            icon_size = (int)(text_height * IconRatio), icon_gap = (int)(text_height * IconGap), item_height = text_height + gap_y * 2, icon_xy = (item_height - icon_size) / 2,
            gap_x2 = gap_x * 2, gap_y2 = gap_y * 2;

            tmp_padd = padd;

            #region 计算最大区域

            int maxw = ItemMaxWidth(g, items, text_height, icon_size, icon_gap), maxwr = maxw + gap_x2;

            int y = 0, sy = 0, count = 0, divider_count = 0;
            var lists = new List<IMenuItem>(items.Count);
            foreach (var it in items)
            {
                if (it.Visible)
                {
                    if (it is MenuDividerItem)
                    {
                        divider_count++;
                        var item = new DMenuItem(new Rectangle(padd, padd2 + y, maxwr, sp));
                        y += padd2 + sp;
                        lists.Add(item);
                    }
                    else
                    {
                        var rect = new Rectangle(padd, padd + y, maxwr, item_height);
                        int ux = gap_x, uw = gap_x2;
                        var item = new OMenuItem(it, rect);
                        if (it.HasIcon)
                        {
                            int tmp = icon_size + icon_gap;
                            item.RectIcon = new Rectangle(rect.X + ux, rect.Y + icon_xy, icon_size, icon_size);
                            ux += tmp;
                            uw += tmp;
                        }
                        if (it.CanExpand)
                        {
                            item.RectArrow = new Rectangle(rect.Right - gap_x - icon_size, rect.Y + icon_xy, icon_size, icon_size);
                            uw += icon_size + icon_gap;
                        }
                        item.RectText = new Rectangle(rect.X + ux, rect.Y, rect.Width - uw, rect.Height);
                        if (it.Select && sy == 0) sy = y;
                        y += item_height;
                        lists.Add(item);
                        count++;
                    }
                }
            }

            #endregion

            int maxh = item_height * count + padd2;
            if (divider_count > 0) maxh += divider_count * (padd2 + sp);
            int h = maxh, w = maxw + padd2 + gap_x2;

            var screen = Screen.FromPoint(point).WorkingArea;
            if (h > screen.Height - shadow2)
            {
                h = screen.Height - shadow2;
                ScrollBar.SizeChange(new Rectangle(0, 0, w, h));
                ScrollBar.SetVrSize(0, maxh);
                if (sy > 0) ScrollBar.ValueY = sy;
            }

            SetSize(w, h);
            return lists;
        }
        int ItemMaxWidth(Canvas g, IList<MenuItem> items, int text_height, int icon_size, int icon_gap)
        {
            int tmp = 0;
            foreach (var it in items)
            {
                int tmp2 = g.MeasureText(it.Text, Font).Width;
                if (it.HasIcon) tmp2 += icon_size + icon_gap;
                if (it.items != null && it.items.Count > 0) tmp2 += icon_size + icon_gap;

                if (tmp2 > tmp) tmp = tmp2;
            }
            return tmp;
        }

        public void FocusItem(OMenuItem item)
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
                    if (Items[i] is OMenuItem it && it.Val.Enabled)
                    {
                        if (it.Contains(x, y + sy, out var change))
                        {
                            hand++;
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
                if (PARENT is Menu menu) menu.select_x = select_x;

                if (Items[hoveindex] is OMenuItem it && it.Sub != null && it.Sub.Count > 0 && PARENT != null) OpenDown(it);
            }
        }
        protected override void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseUp() && OnTouchUp() && down)
            {
                down = false;
                int sy = ScrollBar.Value;
                foreach (var item in Items)
                {
                    if (item is OMenuItem it && it.Val.Enabled && it.Contains(x, y + sy, out _) && OnClick(it)) return;
                }
            }
            else down = false;
        }

        protected override void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta) => ScrollBar.MouseWheel(delta);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        bool OnClick(OMenuItem it)
        {
            if (it.Sub == null || it.Sub.Count == 0)
            {
                if (PARENT is Menu menu) menu.DropDownChange(it.Val);
                CloseSub();
                return true;
            }
            else
            {
                if (subForm == null) OpenDown(it);
                else
                {
                    if (subForm.Guid == it.Val) return false;
                    subForm?.IClose();
                    subForm = null;
                }
            }
            return false;
        }

        void OpenDown(OMenuItem it)
        {
            var rect = new Rectangle(it.Rect.X + tmp_padd, it.Rect.Y - ScrollBar.ValueY - tmp_padd, it.Rect.Width, it.Rect.Height);
            if (PARENT is Menu menu)
            {
                subForm = new LayeredFormMenuDown(menu, select_x + 1, this, Radius, tmp_padd + it.Rect.Height / 2F, rect, it.Val, it.Sub);
                subForm.Show(this);
            }
        }
        void CloseSub()
        {
            IClose();
            var item = this;
            while (item.lay is LayeredFormMenuDown form)
            {
                if (item == form) return;
                form.IClose();
                item = form;
            }
        }

        public override void IClosing()
        {
            if (select_x == 0)
            {
                var item = this;
                while (item.lay is LayeredFormMenuDown form)
                {
                    if (item == form) return;
                    form.IClose();
                    item = form;
                }
            }
        }

        #endregion

        #region 列模型

        internal class OMenuItem : IMenuItem
        {
            public OMenuItem(MenuItem _val, Rectangle rect)
            {
                Sub = _val.Sub;
                if (_val.CanExpand) has_sub = true;
                Rect = rect;
                Val = _val;
            }

            public MenuItem Val { get; set; }

            /// <summary>
            /// 子选项
            /// </summary>
            public MenuItemCollection Sub { get; set; }
            internal bool has_sub { get; set; }

            public Rectangle RectIcon { get; set; }


            internal Rectangle RectArrow { get; set; }

            internal bool SetHover(bool val)
            {
                bool change = false;
                if (val)
                {
                    if (!Hover) change = true;
                    Hover = true;
                }
                else
                {
                    if (Hover) change = true;
                    Hover = false;
                }
                return change;
            }

            internal bool Contains(int x, int y, out bool change)
            {
                if (Rect.Contains(x, y))
                {
                    change = SetHover(true);
                    return true;
                }
                else
                {
                    change = SetHover(false);
                    return false;
                }
            }

            public Rectangle RectText { get; set; }
        }
        internal class DMenuItem : IMenuItem
        {
            public DMenuItem(Rectangle rect)
            {
                Rect = rect;
            }
        }

        internal class IMenuItem
        {
            public bool Hover { get; set; }

            public Rectangle Rect { get; set; }
        }

        #endregion
    }
}