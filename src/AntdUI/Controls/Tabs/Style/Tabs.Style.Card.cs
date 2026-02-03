// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Tabs
    {
        /// <summary>
        /// 卡片样式
        /// </summary>
        public class StyleCard : IStyle
        {
            Tabs? owner;
            public StyleCard() { }
            public StyleCard(Tabs tabs) { owner = tabs; }

            #region 属性

            int radius = 6;
            /// <summary>
            /// 卡片圆角
            /// </summary>
            [Description("卡片圆角"), Category("卡片"), DefaultValue(6)]
            public int Radius
            {
                get => radius;
                set
                {
                    if (radius == value) return;
                    radius = value;
                    owner?.Invalidate();
                }
            }

            int bordersize = 1;
            /// <summary>
            /// 边框大小
            /// </summary>
            [Description("边框大小"), Category("卡片"), DefaultValue(1)]
            public int Border
            {
                get => bordersize;
                set
                {
                    if (bordersize == value) return;
                    bordersize = value;
                    owner?.LoadLayout(true);
                }
            }

            Color? border;
            /// <summary>
            /// 卡片边框颜色
            /// </summary>
            [Description("卡片边框颜色"), Category("卡片"), DefaultValue(null)]
            [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
            public Color? BorderColor
            {
                get => border;
                set
                {
                    if (border == value) return;
                    border = value;
                    owner?.Invalidate();
                }
            }

            /// <summary>
            /// 卡片边框激活颜色
            /// </summary>
            [Description("卡片边框激活颜色"), Category("卡片"), DefaultValue(null)]
            [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
            public Color? BorderActive { get; set; }

            Color? fill;
            /// <summary>
            /// 卡片颜色
            /// </summary>
            [Description("卡片颜色"), Category("卡片"), DefaultValue(null)]
            [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
            public Color? Fill
            {
                get => fill;
                set
                {
                    if (fill == value) return;
                    fill = value;
                    owner?.Invalidate();
                }
            }

            /// <summary>
            /// 卡片悬停颜色
            /// </summary>
            [Description("卡片悬停颜色"), Category("卡片"), DefaultValue(null)]
            [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
            public Color? FillHover { get; set; }

            /// <summary>
            /// 卡片激活颜色
            /// </summary>
            [Description("卡片激活颜色"), Category("卡片"), DefaultValue(null)]
            [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
            public Color? FillActive { get; set; }

            int gap = 2;
            /// <summary>
            /// 卡片间距
            /// </summary>
            [Description("卡片间距"), Category("卡片"), DefaultValue(2)]
            public int Gap
            {
                get => gap;
                set
                {
                    if (gap == value) return;
                    gap = value;
                    owner?.LoadLayout(true);
                }
            }

            bool closable = false;
            /// <summary>
            /// 可关闭
            /// </summary>
            [Description("可关闭"), Category("卡片"), DefaultValue(false)]
            public bool Closable
            {
                get => closable;
                set
                {
                    if (closable == value) return;
                    closable = value;
                    owner?.LoadLayout(true);
                }
            }

            #endregion

            #region 布局

            TabPageRect[] rects = new TabPageRect[0];
            public void LoadLayout(Tabs tabs, Rectangle rect, TabCollection items)
            {
                owner = tabs;
                rects = tabs.GDI(g =>
                {
                    int gap = (int)(tabs.Gap * owner.Dpi), gap2 = gap * 2, xy = 0, cardgap = (int)(Gap * owner.Dpi);

                    var rect_list = new List<TabPageRect>(items.Count);
                    var rect_dir = GetDir(tabs, g, items, gap, gap2, out int ico_size, out int ico_gap, out int close_size, out int xy2, out int rw);
                    int? rw_tmp = tabs.TextCenter ? rw : null;
                    switch (tabs.Alignment)
                    {
                        case TabAlignment.Bottom:
                            int y = rect.Bottom - xy2;
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    bool close = closable && !it.Key.ReadOnly;
                                    Rectangle rect_it;
                                    int width = it.Value.Width;

                                    if (it.Key.HasIcon && width == 0)
                                    {
                                        width = ico_size + ico_gap;
                                        if (close) width += close_size + ico_gap;
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon) width += ico_size + ico_gap * 2;
                                        if (close) width += close_size + ico_gap;
                                    }

                                    rect_it = new Rectangle(rect.X + xy, y, width + gap2, xy2);

                                    if (close)
                                    {
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, true));
                                        else rect_list.Add(new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, true));
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, true));
                                        else rect_list.Add(new TabPageRect(rect_it, it.Value, gap, true));
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(0, 0, 0, xy2 + tabs.Margin.Top);
                            owner.scroll_max = xy - rect.Width;
                            owner.scroll_show = xy > rect.Width;
                            break;
                        case TabAlignment.Left:
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    bool close = closable && !it.Key.ReadOnly;

                                    int textHeight = it.Value.Height;
                                    int itemHeight;
                                    if (owner.IsRotate)
                                    {
                                        int iconPart = it.Key.HasIcon ? ico_size + ico_gap : 0;
                                        int closePart = (close) ? close_size + ico_gap : 0;
                                        itemHeight = gap + iconPart + textHeight + closePart + gap;
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon && textHeight == 0) itemHeight = ico_size + gap;
                                        else itemHeight = textHeight + gap;
                                    }

                                    Rectangle rect_it = new Rectangle(rect.X, rect.Y + xy, xy2, itemHeight);
                                    TabPageRect p;
                                    if (owner.IsRotate)
                                    {
                                        p = new TabPageRect(rect_it, it.Value, gap, true, rw_tmp);
                                        bool iconOnTop = owner.Rotate == TRotate.Clockwise_90;
                                        bool hasIcon = it.Key.HasIcon;
                                        bool hasClose = close;

                                        if (hasIcon)
                                        {
                                            int ico_x = rect_it.X + (rect_it.Width - ico_size) / 2;
                                            int ico_y = iconOnTop ? rect_it.Y + gap : rect_it.Bottom - gap - ico_size;
                                            p.Rect_Ico = new Rectangle(ico_x, ico_y, ico_size, ico_size);
                                        }
                                        else p.Rect_Ico = Rectangle.Empty;

                                        if (hasClose)
                                        {
                                            int cs = close_size;
                                            int cx = rect_it.X + (rect_it.Width - cs) / 2;
                                            int cy = iconOnTop ? rect_it.Bottom - gap - cs : rect_it.Y + gap;
                                            p.Rect_Close = new Rectangle(cx, cy, cs, cs);
                                        }
                                        else p.Rect_Close = Rectangle.Empty;

                                        int textTop = rect_it.Y + gap;
                                        int textBottom = rect_it.Bottom - gap;

                                        if (hasIcon && iconOnTop) textTop += ico_size + ico_gap;
                                        if (hasClose && !iconOnTop) textTop += close_size + ico_gap;

                                        if (hasClose && iconOnTop) textBottom -= close_size + ico_gap;
                                        if (hasIcon && !iconOnTop) textBottom -= ico_size + ico_gap;

                                        int th = Math.Max(0, textBottom - textTop);
                                        int textW = rw_tmp ?? it.Value.Width;
                                        int tx = rect_it.X + Math.Max(0, (rect_it.Width - textW) / 2);
                                        p.Rect_Text = new Rectangle(tx, textTop, textW, th);
                                    }
                                    else
                                    {
                                        if (close)
                                        {
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, true, rw_tmp, rw);
                                            else p = new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, true, rw_tmp, rw);
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, true, rw_tmp);
                                            else p = new TabPageRect(rect_it, it.Value, gap, true, rw_tmp);
                                        }
                                    }

                                    rect_list.Add(p);
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(xy2 + tabs.Margin.Left, 0, 0, 0);
                            owner.scroll_max = xy - rect.Height;
                            owner.scroll_show = xy > rect.Height;
                            break;
                        case TabAlignment.Right:
                            int x = rect.Right - xy2;
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    bool close = closable && !it.Key.ReadOnly;

                                    int textHeight = it.Value.Height;
                                    int itemHeight;
                                    if (owner.IsRotate)
                                    {
                                        int iconPart = it.Key.HasIcon ? ico_size + ico_gap : 0;
                                        int closePart = (close) ? close_size + ico_gap : 0;
                                        itemHeight = gap + iconPart + textHeight + closePart + gap;
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon && textHeight == 0) itemHeight = ico_size + gap;
                                        else itemHeight = textHeight + gap;
                                    }

                                    Rectangle rect_it = new Rectangle(x, rect.Y + xy, xy2, itemHeight);
                                    TabPageRect p;
                                    if (owner.IsRotate)
                                    {
                                        p = new TabPageRect(rect_it, it.Value, gap, true, rw_tmp);
                                        bool iconOnTop = owner.Rotate == TRotate.Clockwise_90;
                                        bool hasIcon = it.Key.HasIcon;
                                        bool hasClose = close;

                                        if (hasIcon)
                                        {
                                            int ico_x = rect_it.X + (rect_it.Width - ico_size) / 2;
                                            int ico_y = iconOnTop ? rect_it.Y + gap : rect_it.Bottom - gap - ico_size;
                                            p.Rect_Ico = new Rectangle(ico_x, ico_y, ico_size, ico_size);
                                        }
                                        else p.Rect_Ico = Rectangle.Empty;

                                        if (hasClose)
                                        {
                                            int cs = close_size;
                                            int cx = rect_it.X + (rect_it.Width - cs) / 2;
                                            int cy = iconOnTop ? rect_it.Bottom - gap - cs : rect_it.Y + gap;
                                            p.Rect_Close = new Rectangle(cx, cy, cs, cs);
                                        }
                                        else p.Rect_Close = Rectangle.Empty;

                                        int textTop = rect_it.Y + gap;
                                        int textBottom = rect_it.Bottom - gap;

                                        if (hasIcon && iconOnTop) textTop += ico_size + ico_gap;
                                        if (hasClose && !iconOnTop) textTop += close_size + ico_gap;

                                        if (hasClose && iconOnTop) textBottom -= close_size + ico_gap;
                                        if (hasIcon && !iconOnTop) textBottom -= ico_size + ico_gap;

                                        int th = Math.Max(0, textBottom - textTop);
                                        int textW = rw_tmp ?? it.Value.Width;
                                        int tx = rect_it.X + Math.Max(0, (rect_it.Width - textW) / 2);
                                        p.Rect_Text = new Rectangle(tx, textTop, textW, th);
                                    }
                                    else
                                    {
                                        if (close)
                                        {
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, false, rw_tmp, rw);
                                            else p = new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, false, rw_tmp, rw);
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, false, rw_tmp);
                                            else p = new TabPageRect(rect_it, it.Value, gap, false, rw_tmp);
                                        }
                                    }

                                    rect_list.Add(p);
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(0, 0, xy2 + tabs.Margin.Left, 0);
                            owner.scroll_max = xy - rect.Height;
                            owner.scroll_show = xy > rect.Height;
                            break;
                        case TabAlignment.Top:
                        default:
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    bool close = closable && !it.Key.ReadOnly;
                                    Rectangle rect_it;
                                    int width = it.Value.Width;

                                    if (it.Key.HasIcon && width == 0)
                                    {
                                        width = ico_size + ico_gap;
                                        if (close) width += close_size + ico_gap;
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon) width += ico_size + ico_gap * 2;
                                        if (close) width += close_size + ico_gap;
                                    }

                                    rect_it = new Rectangle(rect.X + xy, rect.Y, width + gap2, xy2);

                                    if (close)
                                    {
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, true));
                                        else rect_list.Add(new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, true));
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon)
                                        {
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, true));
                                        }
                                        else
                                        {
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, gap, true));
                                        }
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(0, xy2 + tabs.Margin.Top, 0, 0);
                            owner.scroll_max = xy - rect.Width;
                            owner.scroll_show = xy > rect.Width;
                            break;
                    }
                    if (owner.scroll_show) owner.scroll_max += owner.SizeExceed(owner.ClientRectangle, rect_list[0].Rect, rect_list[rect_list.Count - 1].Rect);
                    else
                    {
                        owner.scroll_x = owner.scroll_y = 0;
                        if (tabs.centered)
                        {
                            switch (tabs.Alignment)
                            {
                                case TabAlignment.Left:
                                case TabAlignment.Right:
                                    int oy = (rect.Height - xy) / 2;
                                    foreach (var item in rect_dir) item.Key.SetOffset(0, oy);
                                    foreach (var item in rect_list)
                                    {
                                        item.Rect.Offset(0, oy);
                                        item.Rect_Text.Offset(0, oy);
                                        item.Rect_Ico.Offset(0, oy);
                                        item.Rect_Close.Offset(0, oy);
                                    }
                                    break;
                                case TabAlignment.Top:
                                case TabAlignment.Bottom:
                                default:
                                    int ox = (rect.Width - xy) / 2;
                                    foreach (var item in rect_dir) item.Key.SetOffset(ox, 0);
                                    foreach (var item in rect_list)
                                    {
                                        item.Rect.Offset(ox, 0);
                                        item.Rect_Text.Offset(ox, 0);
                                        item.Rect_Ico.Offset(ox, 0);
                                        item.Rect_Close.Offset(ox, 0);
                                    }
                                    break;
                            }
                        }
                    }
                    return rect_list.ToArray();
                });
            }
            int onetmp = 0;
            Dictionary<TabPage, Size> GetDir(Tabs owner, Canvas g, TabCollection items, int gap, int gap2, out int ico_size, out int ico_gap, out int close_size, out int sizewh, out int rw)
            {
                sizewh = rw = 0;
                var size_t = g.MeasureString(Config.NullText, owner.Font);
                var rect_dir = new Dictionary<TabPage, Size>(items.Count);
                int i = 0;
                int? tmp = null;
                foreach (var item in items)
                {
                    var size = g.MeasureString(item.Text, owner.Font);
                    if (owner.IsRotate) size = new Size(size.Height, size.Width);
                    rect_dir.Add(item, size);
                    if (rw < size.Width) rw = size.Width;
                    if (item.Visible && tmp == null) tmp = i;
                    i++;
                }
                onetmp = tmp ?? 0;
                ico_size = (int)(size_t.Height * owner.iconratio);
                ico_gap = (int)(size_t.Height * owner.icongap);
                close_size = (int)(size_t.Height * (owner.iconratio * .8F));
                switch (owner.Alignment)
                {
                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        if (closable)
                        {
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    int w;
                                    if (it.Key.HasIcon) w = owner.IsRotate ? Math.Max(rw, ico_size) + gap2 : it.Value.Width + gap2 + ico_size + ico_gap;
                                    else w = owner.IsRotate ? rw + gap2 : it.Value.Width + gap2;
                                    w += close_size + ico_gap;
                                    if (sizewh < w) sizewh = w;
                                }
                            }
                        }
                        else
                        {
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    int w;
                                    if (it.Key.HasIcon) w = owner.IsRotate ? Math.Max(rw, ico_size) + gap2 : it.Value.Width + gap2 + ico_size + ico_gap;
                                    else w = owner.IsRotate ? rw + gap2 : it.Value.Width + gap2;
                                    if (sizewh < w) sizewh = w;
                                }
                            }
                        }
                        break;
                    case TabAlignment.Top:
                    case TabAlignment.Bottom:
                    default:
                        foreach (var it in rect_dir)
                        {
                            if (it.Key.Visible)
                            {
                                int h = it.Value.Height > 0 ? it.Value.Height + gap : ico_size + gap;
                                if (sizewh < h) sizewh = h;
                            }
                        }
                        break;
                }

                return owner.HandItemSize(rect_dir, ref sizewh);
            }

            #endregion

            #region 渲染

            public void Paint(Tabs owner, Canvas g, TabCollection items)
            {
                if (rects.Length == items.Count)
                {
                    using (var brush_fore = new SolidBrush(owner.ForeColor ?? Colour.Text.Get(nameof(Tabs), owner.ColorScheme)))
                    using (var brush_fill = new SolidBrush(owner.Fill ?? Colour.Primary.Get(nameof(Tabs), owner.ColorScheme)))
                    {
                        var rect_t = owner.ClientRectangle;
                        int radius = (int)(Radius * owner.Dpi), bor = (int)(bordersize * owner.Dpi), bor2 = bor * 6, bor22 = bor2 * 2, borb2 = bor / 2;
                        TabPage? sel = null;
                        int select = owner.SelectedIndex;
                        var rect_one = rects[onetmp].Rect;
                        switch (owner.Alignment)
                        {
                            case TabAlignment.Bottom:
                                int read_b_h = rect_one.Height + rect_one.X;
                                var rect_s_b = new Rectangle(rect_t.X, rect_t.Bottom - read_b_h, rect_t.Width, read_b_h);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_b, rect_one));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_b);
                                sel = PaintTABS(owner, g, items, radius, brush_fore, bor, false, false, true, true);
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var state = g.Save();
                                    if (owner.pageMove == sel) g.TranslateTransform(-owner.offsetx, -owner.offsety);

                                    var rect_page = sel.Rect;
                                    if (bor > 0)
                                    {
                                        float ly = rect_page.Y + borb2;
                                        var rect_card = new RectangleF(rect_page.X + borb2, rect_page.Y - borb2, rect_page.Width - bor, rect_page.Height + borb2);
                                        var rect_line = new Rectangle(rect_page.X - bor, rect_page.Y + bor, rect_page.Width + bor2, rect_page.Height + bor);
                                        PaintTABS(owner, g, rect_t, rect_page, radius, rect_card, rect_line, rect_t.X, ly, rect_t.Right, ly, bor, false, false, true, true);
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                    }
                                    else
                                    {
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                        using (var path = Helper.RoundPath(rect_page, radius, false, false, true, true))
                                        using (var brush_bg_active = new SolidBrush(FillActive ?? Colour.BgContainer.Get(nameof(Tabs), owner.ColorScheme)))
                                        {
                                            g.Fill(brush_bg_active, path);
                                        }
                                    }
                                    PaintText(g, rects[select], owner, sel, brush_fill);
                                    g.Restore(state);
                                }
                                break;
                            case TabAlignment.Left:
                                var rect_s_l = new Rectangle(rect_t.X, rect_t.Y, rect_one.Right, rect_t.Height);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_l, rect_one));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_l);
                                sel = PaintTABS(owner, g, items, radius, brush_fore, bor, true, false, false, true);
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var state = g.Save();
                                    if (owner.pageMove == sel) g.TranslateTransform(-owner.offsetx, -owner.offsety);

                                    var rect_page = sel.Rect;
                                    if (bor > 0)
                                    {
                                        float lx = rect_page.Right - borb2;
                                        var rect_card = new RectangleF(rect_page.X - borb2, rect_page.Y + borb2, rect_page.Width + borb2, rect_page.Height - bor);
                                        var rect_line = new Rectangle(rect_page.X - borb2, rect_page.Y - bor, rect_page.Width, rect_page.Height + bor2);
                                        PaintTABS(owner, g, rect_t, rect_page, radius, rect_card, rect_line, lx, rect_t.Y, lx, rect_t.Bottom, bor, true, false, false, true);
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                    }
                                    else
                                    {
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                        using (var path = Helper.RoundPath(rect_page, radius, true, false, false, true))
                                        using (var brush_bg_active = new SolidBrush(FillActive ?? Colour.BgContainer.Get(nameof(Tabs), owner.ColorScheme)))
                                        {
                                            g.Fill(brush_bg_active, path);
                                        }
                                    }
                                    PaintText(g, rects[select], owner, sel, brush_fill);
                                    g.Restore(state);
                                }
                                break;
                            case TabAlignment.Right:
                                int read_r_w = rect_one.Width + rect_one.Y;
                                var rect_s_r = new Rectangle(rect_t.Right - read_r_w, rect_t.Y, read_r_w, rect_t.Height);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_r, rect_one));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_r);
                                sel = PaintTABS(owner, g, items, radius, brush_fore, bor, false, true, true, false);
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var state = g.Save();
                                    if (owner.pageMove == sel) g.TranslateTransform(-owner.offsetx, -owner.offsety);

                                    var rect_page = sel.Rect;
                                    if (bor > 0)
                                    {
                                        float lx = rect_page.X + borb2;
                                        var rect_card = new RectangleF(rect_page.X - borb2, rect_page.Y + borb2, rect_page.Width + borb2, rect_page.Height - bor);
                                        var rect_line = new Rectangle(rect_page.X + bor, rect_page.Y - bor, rect_page.Width + bor, rect_page.Height + bor2);
                                        PaintTABS(owner, g, rect_t, rect_page, radius, rect_card, rect_line, lx, rect_t.Y, lx, rect_t.Bottom, bor, false, true, true, false);
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                    }
                                    else
                                    {
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                        using (var path = Helper.RoundPath(rect_page, radius, false, true, true, false))
                                        using (var brush_bg_active = new SolidBrush(FillActive ?? Colour.BgContainer.Get(nameof(Tabs), owner.ColorScheme)))
                                        {
                                            g.Fill(brush_bg_active, path);
                                        }
                                    }
                                    PaintText(g, rects[select], owner, sel, brush_fill);
                                    g.Restore(state);
                                }
                                break;
                            case TabAlignment.Top:
                            default:
                                var rect_s_t = new Rectangle(rect_t.X, rect_t.Y, rect_t.Width, rect_one.Bottom);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_t, rect_one));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_t);
                                sel = PaintTABS(owner, g, items, radius, brush_fore, bor, true, true, false, false);
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var state = g.Save();
                                    if (owner.pageMove == sel) g.TranslateTransform(-owner.offsetx, -owner.offsety);

                                    var rect_page = sel.Rect;
                                    if (bor > 0)
                                    {
                                        float ly = rect_page.Bottom - borb2;
                                        var rect_card = new RectangleF(rect_page.X + borb2, rect_page.Y - borb2, rect_page.Width - bor, rect_page.Height + borb2);
                                        var rect_line = new Rectangle(rect_page.X - bor, rect_page.Y - bor, rect_page.Width + bor2, rect_page.Height);
                                        PaintTABS(owner, g, rect_t, rect_page, radius, rect_card, rect_line, rect_t.X, ly, rect_t.Right, ly, bor, true, true, false, false);
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                    }
                                    else
                                    {
                                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                        using (var path = Helper.RoundPath(rect_page, radius, true, true, false, false))
                                        using (var brush_bg_active = new SolidBrush(FillActive ?? Colour.BgContainer.Get(nameof(Tabs), owner.ColorScheme)))
                                        {
                                            g.Fill(brush_bg_active, path);
                                        }
                                    }
                                    PaintText(g, rects[select], owner, sel, brush_fill);
                                    g.Restore(state);
                                }
                                break;
                        }
                        g.ResetClip();
                        if (owner.scroll_show) owner.PaintExceed(g, brush_fore.Color, radius, rect_t, rect_one, rects[rects.Length - 1].Rect, true);
                    }
                }
            }

            TabPage? PaintTABS(Tabs owner, Canvas g, TabCollection items, int radius, SolidBrush brush_fore, int bor, bool TL, bool TR, bool BR, bool BL)
            {
                int i = 0;
                TabPage? sel = null;
                using (var brush_active = new SolidBrush(owner.FillActive ?? Colour.PrimaryActive.Get(nameof(Tabs), owner.ColorScheme)))
                using (var brush_hover = new SolidBrush(owner.FillHover ?? Colour.PrimaryHover.Get(nameof(Tabs), owner.ColorScheme)))
                using (var brush_bg = new SolidBrush(Fill ?? Colour.FillQuaternary.Get(nameof(Tabs), owner.ColorScheme)))
                using (var brush_bg_hover = new SolidBrush(FillHover ?? Colour.FillQuaternary.Get(nameof(Tabs), owner.ColorScheme)))
                {
                    foreach (var page in items)
                    {
                        if (page.Visible)
                        {
                            if (owner.SelectedIndex == i) sel = page;
                            else
                            {
                                var state = g.Save();
                                if (owner.pageMove == page) g.TranslateTransform(-owner.offsetx, -owner.offsety);

                                using (var path = Helper.RoundPath(page.Rect, radius, TL, TR, BR, BL))
                                {
                                    g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                    if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get(nameof(Tabs), owner.ColorScheme), bor, path);
                                    if (owner.hover_i == i) PaintText(g, rects[i], owner, page, owner.pageDown == page ? brush_active : brush_hover);
                                    else PaintText(g, rects[i], owner, page, brush_fore);
                                }
                                g.Restore(state);
                            }
                        }
                        i++;
                    }
                }
                return sel;
            }
            void PaintTABS(Tabs owner, Canvas gg, Rectangle rect_t, Rectangle rect_page, int radius, RectangleF rect_card, Rectangle rect_line, float x, float y, float x2, float y2, int bor, bool TL, bool TR, bool BR, bool BL)
            {
                try
                {
                    using (var bmp = new Bitmap(rect_t.Width, rect_t.Height))
                    {
                        using (var g = Graphics.FromImage(bmp).High(owner.Dpi))
                        {
                            using (var path = Helper.RoundPath(rect_page, radius, TL, TR, BR, BL))
                            using (var pen_bg = new Pen(BorderActive ?? Colour.BorderColor.Get(nameof(Tabs), owner.ColorScheme), bor))
                            using (var brush_bg_active = new SolidBrush(FillActive ?? Colour.BgContainer.Get(nameof(Tabs), owner.ColorScheme)))
                            {
                                g.DrawLine(pen_bg, x, y, x2, y2);
                                if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                using (var path_card = Helper.RoundPath(rect_card, radius, TL, TR, BR, BL))
                                {
                                    g.Fill(brush_bg_active, path_card);
                                }
                                g.SetClip(rect_line);
                                g.Draw(pen_bg, path);
                            }
                        }

                        gg.Image(bmp, rect_t);
                    }
                }
                catch { }
            }
            void PaintText(Canvas g, TabPageRect rects, Tabs owner, TabPage page, SolidBrush brush)
            {
                if (page.Enabled)
                {
                    if (page.HasIcon)
                    {
                        if (page.Icon != null) g.Image(page.Icon, rects.Rect_Ico);
                        if (page.IconSvg != null) g.GetImgExtend(page.IconSvg, rects.Rect_Ico, brush.Color);
                    }
                    if (closable)
                    {
                        if (rects.hover_close == null) g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme));
                        else if (rects.hover_close.Animation) g.PaintIconClose(rects.Rect_Close, Helper.ToColor(rects.hover_close.Value + Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme).A, Colour.Text.Get(nameof(Tabs), owner.ColorScheme)));
                        else if (rects.hover_close.Switch) g.PaintIconClose(rects.Rect_Close, Colour.Text.Get(nameof(Tabs), owner.ColorScheme));
                        else g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme));
                    }
                    PaintTextCore(g, rects, owner, page, brush);
                }
                else
                {
                    if (page.HasIcon)
                    {
                        if (page.Icon != null) g.Image(page.Icon, rects.Rect_Ico);
                        if (page.IconSvg != null) g.GetImgExtend(page.IconSvg, rects.Rect_Ico, Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme));
                    }
                    if (closable)
                    {
                        if (rects.hover_close == null) g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme));
                        else if (rects.hover_close.Animation) g.PaintIconClose(rects.Rect_Close, Helper.ToColor(rects.hover_close.Value + Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme).A, Colour.Text.Get(nameof(Tabs), owner.ColorScheme)));
                        else if (rects.hover_close.Switch) g.PaintIconClose(rects.Rect_Close, Colour.Text.Get(nameof(Tabs), owner.ColorScheme));
                        else g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme));
                    }
                    using (var brush_e = new SolidBrush(Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme)))
                    {
                        PaintTextCore(g, rects, owner, page, brush_e);
                    }
                }
                owner.PaintBadge(g, page, rects.Rect_Text);
            }
            void PaintTextCore(Canvas g, TabPageRect rects, Tabs owner, TabPage page, SolidBrush brush)
            {
                if (owner.IsRotate)
                {
                    var state = g.Save();
                    float cx = rects.Rect_Text.X + rects.Rect_Text.Width / 2f, cy = rects.Rect_Text.Y + rects.Rect_Text.Height / 2f;
                    g.TranslateTransform(cx, cy);
                    float angle = owner.Rotate == TRotate.Clockwise_90 ? 90f : -90f;
                    g.RotateTransform(angle);
                    var drawRect = new RectangleF(-rects.Rect_Text.Height / 2f, -rects.Rect_Text.Width / 2f, rects.Rect_Text.Height, rects.Rect_Text.Width);
                    g.String(page.Text, owner.Font, brush, drawRect, owner.s_c);
                    g.Restore(state);
                }
                else g.String(page.Text, owner.Font, brush, rects.Rect_Text, owner.s_c);
            }

            #endregion

            public TabPageRect GetTabRect(int i) => rects[i];

            public void SelectedIndexChanged(int i, int old)
            {
                if (owner == null) return;
                if (old > -1 && i > -1)
                {
                    TabPageRect oldTab = rects[old], newTab = rects[i];
                    if (owner.TabFocusMove(oldTab, newTab, i, rects.Length)) return;
                }
                owner.Invalidate();
            }

            public void Dispose()
            {
                foreach (var item in rects)
                {
                    item.hover_close?.Dispose();
                }
            }

            #region 鼠标

            public bool MouseClick(TabPage page, int i, int x, int y)
            {
                if (owner == null) return false;
                if (closable && !page.ReadOnly)
                {
                    if (rects[i].Rect_Close.Contains(x, y))
                    {
                        bool flag = owner.IOnClosingPage(page);
                        if (flag) owner.Pages.Remove(page);
                        return true;
                    }
                }
                return false;
            }
            public void MouseMove(int x, int y)
            {
                if (owner == null) return;
                if (closable)
                {
                    int i = 0;
                    foreach (var item in rects)
                    {
                        item.hover_close ??= new ITaskOpacity(nameof(Tabs), owner);
                        if (i == owner.hover_i)
                        {
                            item.hover_close.MaxValue = Colour.Text.Get(nameof(Tabs), owner.ColorScheme).A - Colour.TextQuaternary.Get(nameof(Tabs), owner.ColorScheme).A;
                            item.hover_close.Switch = item.Rect_Close.Contains(x, y);
                        }
                        else item.hover_close.Switch = false;
                        i++;
                    }
                }
            }

            public void MouseLeave()
            {
                if (closable)
                {
                    foreach (var item in rects)
                    {
                        if (item.hover_close == null) continue;
                        item.hover_close.Switch = false;
                    }
                }
            }

            #endregion
        }
    }
}