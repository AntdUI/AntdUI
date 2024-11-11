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
    internal class LayeredFormMenuDown : ILayeredFormOpacityDown, SubLayeredForm
    {
        internal float Radius = 0;
        bool isauto = true, isdark = false;
        List<OMenuItem> Items;
        Color? BackHover, BackActive, foreColor, ForeActive;
        public LayeredFormMenuDown(Menu control, int radius, Rectangle rect_read, MenuItemCollection items)
        {
            MessageCloseMouseLeave = true;
            isauto = control.Theme == TAMode.Auto;
            isdark = Config.IsDark || control.Theme == TAMode.Dark;
            control.Parent.SetTopMost(Handle);
            PARENT = control;
            select_x = 0;
            Font = control.Font;
            foreColor = control.ForeColor;
            ForeActive = control.ForeActive;
            BackHover = control.BackHover;
            BackActive = control.BackActive;
            Radius = (int)(radius * Config.Dpi);
            Items = new List<OMenuItem>(items.Count);
            Init(control, rect_read, items);
        }

        public LayeredFormMenuDown(Menu parent, int sx, LayeredFormMenuDown control, float radius, Rectangle rect_read, MenuItemCollection items)
        {
            isauto = parent.Theme == TAMode.Auto;
            isdark = Config.IsDark || parent.Theme == TAMode.Dark;
            parent.Parent.SetTopMost(Handle);
            select_x = sx;
            PARENT = parent;
            Font = control.Font;
            foreColor = control.foreColor;
            ForeActive = control.ForeActive;
            BackHover = control.BackHover;
            BackActive = control.BackActive;
            Radius = radius;
            control.Disposed += (a, b) => { Dispose(); };
            Items = new List<OMenuItem>(items.Count);
            Init(control, rect_read, items);
        }

        public ILayeredForm? SubForm() => subForm;
        LayeredFormMenuDown? subForm = null;
        void Init(Control control, Rectangle rect_read, MenuItemCollection items)
        {
            int y = 10, w = rect_read.Width;
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font);
                int gap_y = (int)(5 * Config.Dpi), gap_x = (int)(12 * Config.Dpi), gap_x2 = gap_x * 2, gap_y2 = gap_y * 2;
                int font_size = size.Height + gap_y2;
                y += gap_y;

                #region AutoWidth

                int b_w = size.Width + gap_x2;
                bool ui_icon = false, ui_arrow = false;
                foreach (var obj in items)
                {
                    if (obj.Text != null)
                    {
                        var size3 = g.MeasureString(obj.Text, Font);
                        if (size3.Width > b_w) b_w = size3.Width;
                    }
                    if (obj.HasIcon) ui_icon = true;
                    if (obj.CanExpand) ui_arrow = true;
                }
                if (ui_icon) b_w += font_size;
                if (ui_arrow) b_w += (int)Math.Ceiling(font_size * 0.6F);

                w = b_w + gap_x2;
                if (rect_read.Width > w) w = rect_read.Width;

                #endregion

                int item_count = 0, divider_count = 0;
                int text_height = font_size - gap_y2;
                foreach (var it in items)
                {
                    item_count++;
                    Rectangle rect_bg = new Rectangle(10 + gap_y, y, w - gap_y2, font_size), rect_text = new Rectangle(rect_bg.X + gap_x, rect_bg.Y + gap_y, rect_bg.Width - gap_x2, text_height);
                    Items.Add(new OMenuItem(it, rect_bg, gap_y, rect_text));
                    y += font_size;
                }
                var vr = (font_size * item_count) + (gap_y * divider_count);
                y = 10 + gap_y2 + vr;
            });
            int h = y + 10;
            SetSize(w + 20, h);
            if (control is LayeredFormMenuDown)
            {
                var point = control.PointToScreen(Point.Empty);
                SetLocation(point.X + rect_read.Width, point.Y + rect_read.Y - 10);
            }
            else
            {
                if (control is Menu menu && menu.Mode == TMenuMode.Horizontal) SetLocation(rect_read.X - 10, rect_read.Bottom);
                else SetLocation(rect_read.Right, rect_read.Y);
            }
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
                    if (nodata) return false;
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

        StringFormat stringFormatLeft = Helper.SF(lr: StringAlignment.Near);
        public void FocusItem(OMenuItem item)
        {
            if (item.SetHover(true)) Print();
        }

        /// <summary>
        /// 是否显示暂无数据
        /// </summary>
        bool nodata = false;

        #region 鼠标

        internal int select_x = 0;
        int hoveindex = -1;

        protected override void OnMouseUp(MouseEventArgs e)
        {
            foreach (var it in Items)
            {
                if (RunAnimation) return;
                if (it.Show && it.Val.Enabled && it.Contains(e.Location, out _))
                {
                    if (OnClick(it)) return;
                }
            }
            base.OnMouseUp(e);
        }

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
            if (PARENT is Menu menu)
            {
                subForm = new LayeredFormMenuDown(menu, select_x + 1, this, Radius, new Rectangle(it.Rect.X, it.Rect.Y - 0, it.Rect.Width, it.Rect.Height), it.Sub);
                subForm.Show(this);
            }
        }

        int hoveindexold = -1;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RunAnimation) return;
            hoveindex = -1;

            int count = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                var it = Items[i];
                if (it.Show && it.Val.Enabled)
                {
                    if (it.Contains(e.Location, out var change)) hoveindex = i;
                    if (change) count++;
                }
            }
            if (count > 0) Print();
            base.OnMouseMove(e);
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

        #endregion

        readonly StringFormat s_f = Helper.SF_NoWrap();
        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).HighLay())
            {
                using (var path = rect_read.RoundPath(Radius))
                {
                    DrawShadow(g, rect);
                    if (isauto) g.Fill(Style.Db.BgElevated, path);
                    else if (isdark) g.Fill("#1F1F1F".ToColor(), path);
                    else g.Fill(Color.White, path);
                }

                if (nodata)
                {
                    string emptytext = Localization.Get("NoData", "暂无数据");
                    using (var brush = new SolidBrush(Color.FromArgb(180, Style.Db.Text)))
                    { g.String(emptytext, Font, brush, rect_read, s_f); }
                }
                else
                {
                    if (foreColor.HasValue)
                    {
                        using (var brush = new SolidBrush(foreColor.Value))
                        {
                            foreach (var it in Items)
                            {
                                if (it.Show) DrawItem(g, brush, it);
                            }
                        }
                    }
                    else if (isauto)
                    {
                        using (var brush = new SolidBrush(Style.Db.Text))
                        {
                            foreach (var it in Items)
                            {
                                if (it.Show) DrawItem(g, brush, it);
                            }
                        }
                    }
                    else if (isdark)
                    {
                        using (var brush = new SolidBrush(Color.White))
                        {
                            foreach (var it in Items)
                            {
                                if (it.Show) DrawItem(g, brush, it);
                            }
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(Color.Black))
                        {
                            foreach (var it in Items)
                            {
                                if (it.Show) DrawItem(g, brush, it);
                            }
                        }
                    }
                }
            }
            return original_bmp;
        }

        void DrawItem(Canvas g, SolidBrush brush, OMenuItem it)
        {
            if (it.Val.Enabled)
            {
                if (isauto)
                {
                    if (isdark)
                    {
                        if (it.Val.Select)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? Style.Db.Primary, path);
                            }
                            using (var brush_select = new SolidBrush(ForeActive ?? Style.Db.TextBase))
                            {
                                g.String(it.Val.Text, it.Val.Font ?? Font, brush_select, it.RectText, stringFormatLeft);
                            }
                            PaintIcon(g, it, brush.Color);
                        }
                        else
                        {
                            if (it.Hover)
                            {
                                using (var path = it.Rect.RoundPath(Radius))
                                {
                                    g.Fill(BackHover ?? Style.Db.FillTertiary, path);
                                }
                            }
                            g.String(it.Val.Text, it.Val.Font ?? Font, brush, it.RectText, stringFormatLeft);
                            PaintIcon(g, it, brush.Color);
                        }
                    }
                    else
                    {
                        if (it.Val.Select)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? Style.Db.PrimaryBg, path);
                            }
                            using (var brush_select = new SolidBrush(ForeActive ?? Style.Db.TextBase))
                            {
                                g.String(it.Val.Text, it.Val.Font ?? Font, brush_select, it.RectText, stringFormatLeft);
                            }
                        }
                        else
                        {
                            if (it.Hover)
                            {
                                using (var path = it.Rect.RoundPath(Radius))
                                {
                                    g.Fill(BackHover ?? Style.Db.FillTertiary, path);
                                }
                            }
                            g.String(it.Val.Text, it.Val.Font ?? Font, brush, it.RectText, stringFormatLeft);
                        }
                        PaintIcon(g, it, brush.Color);
                    }
                }
                else
                {
                    if (isdark)
                    {
                        if (it.Val.Select)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? "#1668DC".ToColor(), path);
                            }
                            using (var brush_select = new SolidBrush(ForeActive ?? Color.White))
                            {
                                g.String(it.Val.Text, it.Val.Font ?? Font, brush_select, it.RectText, stringFormatLeft);
                            }
                            PaintIcon(g, it, brush.Color);
                        }
                        else
                        {
                            if (it.Hover)
                            {
                                using (var path = it.Rect.RoundPath(Radius))
                                {
                                    g.Fill(BackHover ?? Style.rgba(255, 255, 255, 0.08F), path);
                                }
                            }
                            g.String(it.Val.Text, it.Val.Font ?? Font, brush, it.RectText, stringFormatLeft);
                            PaintIcon(g, it, brush.Color);
                        }
                    }
                    else
                    {
                        if (it.Val.Select)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? Style.Db.PrimaryBg, path);
                            }
                            using (var brush_select = new SolidBrush(ForeActive ?? Style.Db.TextBase))
                            {
                                g.String(it.Val.Text, it.Val.Font ?? Font, brush_select, it.RectText, stringFormatLeft);
                            }
                        }
                        else
                        {
                            if (it.Hover)
                            {
                                using (var path = it.Rect.RoundPath(Radius))
                                {
                                    g.Fill(BackHover ?? Style.Db.FillTertiary, path);
                                }
                            }
                            g.String(it.Val.Text, it.Val.Font ?? Font, brush, it.RectText, stringFormatLeft);
                        }
                        PaintIcon(g, it, brush.Color);
                    }
                }
            }
            else
            {
                if (it.Val.Select)
                {
                    if (isauto)
                    {
                        if (isdark)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? Style.Db.Primary, path);
                            }
                        }
                        else
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? Style.Db.PrimaryBg, path);
                            }
                        }
                    }
                    else
                    {
                        if (isdark)
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? "#1668DC".ToColor(), path);
                            }
                        }
                        else
                        {
                            using (var path = it.Rect.RoundPath(Radius))
                            {
                                g.Fill(BackActive ?? Style.Db.PrimaryBg, path);
                            }
                        }
                    }
                }
                using (var fore = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.String(it.Val.Text, it.Val.Font ?? Font, fore, it.RectText, stringFormatLeft);
                }
                PaintIcon(g, it, brush.Color);
            }
            if (it.has_sub) PaintArrow(g, it, brush.Color);
        }
        void PaintIcon(Canvas g, OMenuItem it, Color fore)
        {
            if (it.Val.Icon != null) g.Image(it.Val.Icon, it.RectIcon);
            else if (it.Val.IconSvg != null) g.GetImgExtend(it.Val.IconSvg, it.RectIcon, fore);
        }
        void PaintArrow(Canvas g, OMenuItem item, Color color)
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
        }

        Bitmap? shadow_temp = null;
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
                g.Image(shadow_temp, rect, 0.2F);
            }
        }

        #region 列模型

        internal class OMenuItem
        {
            public OMenuItem(MenuItem _val, Rectangle rect, int gap_y, Rectangle rect_text)
            {
                Sub = _val.Sub;
                if (_val.CanExpand) has_sub = true;
                Rect = rect;
                if (_val.HasIcon)
                {
                    RectIcon = new Rectangle(rect_text.X, rect_text.Y, rect_text.Height, rect_text.Height);
                    RectText = new Rectangle(rect_text.X + gap_y + rect_text.Height, rect_text.Y, rect_text.Width - rect_text.Height - gap_y, rect_text.Height);
                }
                else RectText = rect_text;
                arr_rect = new Rectangle(Rect.Right - Rect.Height - gap_y, Rect.Y, Rect.Height, Rect.Height);
                Show = true;
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
            public bool Show { get; set; }

            internal Rectangle arr_rect { get; set; }

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

            internal bool Contains(Point point, out bool change)
            {
                if (Rect.Contains(point))
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