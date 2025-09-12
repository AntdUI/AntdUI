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
    internal class LayeredFormMenuDown : ILayeredShadowForm, SubLayeredForm
    {
        #region 初始化

        TAMode ColorScheme;
        bool isdark = false;
        List<OMenuItem> Items;
        Size DPadding;
        Color? backColor, BackHover, BackActive, foreColor, ForeActive;
        float IconRatio = 0.7F, IconGap = 0.25F;

        ScrollBar ScrollBar;
        public LayeredFormMenuDown(Menu control, int radius, Rectangle rect, IList<MenuItem> items)
        {
            MessageCloseMouseLeave = true;
            ColorScheme = control.ColorScheme;
            isdark = Config.IsDark || control.ColorScheme == TAMode.Dark;
            control.Parent.SetTopMost(Handle);
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
            Radius = (int)(radius * Config.Dpi);
            ScrollBar = new ScrollBar(this, ColorScheme);
            var point = control.PointToScreen(Point.Empty);
            Items = LoadLayout(items, point);
            if (control.Mode == TMenuMode.Horizontal) CLocation(control, TAlignFrom.BL, rect, true, -shadow);
            else
            {
                var screen = Screen.FromPoint(point).WorkingArea;
                int x = point.X + control.Width - rect.X - shadow, y = point.Y + rect.Y + shadow;
                if (screen.Right < x + TargetRect.Width) x = x - ((x + TargetRect.Width) - screen.Right) + shadow;
                if (screen.Bottom < y + TargetRect.Height) y = y - ((y + TargetRect.Height) - screen.Bottom) + shadow;
                SetLocationO(x, y);
            }
            Init();
        }

        SubLayeredForm? lay;

        public LayeredFormMenuDown(Menu control, int sx, LayeredFormMenuDown parent, int radius, float itemHeight, Rectangle rect, MenuItemCollection items)
        {
            ColorScheme = control.ColorScheme;
            isdark = Config.IsDark || control.ColorScheme == TAMode.Dark;
            control.Parent.SetTopMost(Handle);
            select_x = sx;
            PARENT = control;
            Font = parent.Font;
            lay = parent;
            backColor = parent.backColor;
            foreColor = parent.foreColor;
            ForeActive = parent.ForeActive;
            BackHover = parent.BackHover;
            BackActive = parent.BackActive;
            Radius = radius;
            parent.Disposed += (a, b) => Dispose();
            ScrollBar = new ScrollBar(this, ColorScheme);
            Items = LoadLayout(items, control.PointToScreen(Point.Empty));

            CLocation(parent, rect, false, 0);
            Init();
        }

        public override string name => nameof(Menu);

        public ILayeredForm? SubForm() => subForm;
        LayeredFormMenuDown? subForm;
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
                            var it = Items[hoveindex];
                            if (OnClick(it)) return true;
                        }
                    }
                    else if (keys == Keys.Up)
                    {
                        hoveindex--;
                        if (hoveindex < 0) hoveindex = Items.Count - 1;
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
                        foreach (var it in Items) it.Hover = false;
                        FocusItem(Items[hoveindex]);
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
                            var it = Items[hoveindex];
                            if (it.Sub != null && it.Sub.Count > 0)
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

        #endregion

        #region 渲染

        StringFormat sf = Helper.SF(lr: StringAlignment.Near);
        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(backColor ?? Colour.BgElevated.Get(name, ColorScheme)))
            {
                g.Fill(brush, path);
            }
        }
        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            if (ScrollBar.ShowY) g.TranslateTransform(0, -ScrollBar.ValueY);
            if (foreColor.HasValue)
            {
                using (var brush = new SolidBrush(foreColor.Value))
                {
                    foreach (var it in Items) DrawItem(g, brush, it);
                }
            }
            else
            {
                using (var brush = new SolidBrush(Colour.Text.Get("Menu", ColorScheme)))
                {
                    foreach (var it in Items) DrawItem(g, brush, it);
                }
            }
            g.Restore(state);
            ScrollBar.Paint(g);
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
                            g.Fill(BackActive ?? Colour.Primary.Get("Menu", ColorScheme), path);
                        }
                        using (var brush_select = new SolidBrush(ForeActive ?? Colour.TextBase.Get("Menu", ColorScheme)))
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
                                g.Fill(BackHover ?? Colour.FillTertiary.Get("Menu", ColorScheme), path);
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
                            g.Fill(BackActive ?? Colour.PrimaryBg.Get("Menu", ColorScheme), path);
                        }
                        using (var brush_select = new SolidBrush(ForeActive ?? Colour.TextBase.Get("Menu", ColorScheme)))
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
                                g.Fill(BackHover ?? Colour.FillTertiary.Get("Menu", ColorScheme), path);
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
                            g.Fill(BackActive ?? Colour.Primary.Get("Menu", ColorScheme), path);
                        }
                    }
                    else
                    {
                        using (var path = it.Rect.RoundPath(Radius))
                        {
                            g.Fill(BackActive ?? Colour.PrimaryBg.Get("Menu", ColorScheme), path);
                        }
                    }
                }
                using (var fore = new SolidBrush(Colour.TextQuaternary.Get("Menu", ColorScheme)))
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
        void PaintArrow(Canvas g, OMenuItem item, Color color)
        {
            var state = g.Save();
            int size = item.RectArrow.Width, size_arrow = size / 2;
            g.TranslateTransform(item.RectArrow.X + size_arrow, item.RectArrow.Y + size_arrow);
            g.RotateTransform(-90F);
            using (var pen = new Pen(color, Config.Dpi * 1.4F))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, item.RectArrow.Width, item.RectArrow.Height).TriangleLines(-1, .7F));
            }
            g.Restore(state);
        }

        #endregion

        #region 布局

        int tmp_padd = 0;
        List<OMenuItem> LoadLayout(IList<MenuItem> items, Point point) => Helper.GDI(g => LoadLayout(g, items, point));
        List<OMenuItem> LoadLayout(Canvas g, IList<MenuItem> items, Point point)
        {
            var text_height = g.MeasureString(Config.NullText, Font).Height;

            int sp = (int)Config.Dpi, padd = (int)(text_height * .18F), padd2 = padd * 2, gap_x = (int)(DPadding.Width * Config.Dpi), gap_y = (int)(DPadding.Height * Config.Dpi),
            icon_size = (int)(text_height * IconRatio), icon_gap = (int)(text_height * IconGap), item_height = text_height + gap_y * 2, icon_xy = (item_height - icon_size) / 2,
            gap_x2 = gap_x * 2, gap_y2 = gap_y * 2;

            tmp_padd = padd;

            #region 计算最大区域

            int maxw = ItemMaxWidth(g, items, text_height, icon_size, icon_gap), maxwr = maxw + gap_x2;

            int y = 0, sy = 0, count = 0;
            var lists = new List<OMenuItem>(items.Count);
            foreach (var it in items)
            {
                if (it.Visible)
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

            #endregion

            int maxh = item_height * count + padd2;
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
                    var it = Items[i];
                    if (it.Val.Enabled)
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
                var it = Items[hoveindex];
                if (it.Sub != null && it.Sub.Count > 0 && PARENT != null) OpenDown(it);
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
                    if (it.Val.Enabled && it.Contains(x, y + sy, out _))
                    {
                        if (OnClick(it)) return;
                    }
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
                IClose();
                return true;
            }
            else
            {
                if (subForm == null) OpenDown(it);
                else
                {
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
                subForm = new LayeredFormMenuDown(menu, select_x + 1, this, Radius, tmp_padd + it.Rect.Height / 2F, rect, it.Sub);
                subForm.Show(this);
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

        internal class OMenuItem
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

            public bool Hover { get; set; }

            internal Rectangle RectArrow { get; set; }

            public Rectangle Rect { get; set; }
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

        #endregion
    }
}