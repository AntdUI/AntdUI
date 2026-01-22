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
        /// 线条样式
        /// </summary>
        public class StyleLine : IStyle
        {
            Tabs? owner;
            public StyleLine() { }
            public StyleLine(Tabs tabs) { owner = tabs; }

            #region 属性

            int size = 3;
            /// <summary>
            /// 条大小
            /// </summary>
            [Description("条大小"), Category("样式"), DefaultValue(3)]
            public int Size
            {
                get => size;
                set
                {
                    if (size == value) return;
                    size = value;
                    owner?.LoadLayout(true);
                }
            }

            int padding = 8;
            /// <summary>
            /// 条边距
            /// </summary>
            [Description("条边距"), Category("样式"), DefaultValue(8)]
            public int Padding
            {
                get => padding;
                set
                {
                    if (padding == value) return;
                    padding = value;
                    owner?.LoadLayout(true);
                }
            }

            int radius = 0;
            /// <summary>
            /// 条圆角
            /// </summary>
            [Description("条圆角"), Category("样式"), DefaultValue(0)]
            public int Radius
            {
                get => radius;
                set
                {
                    if (radius == value) return;
                    radius = value;
                    owner?.Invalidate(true);
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

            int backsize = 1;
            /// <summary>
            /// 条背景大小
            /// </summary>
            [Description("条背景大小"), Category("样式"), DefaultValue(1)]
            public int BackSize
            {
                get => backsize;
                set
                {
                    if (backsize == value) return;
                    backsize = value;
                    owner?.LoadLayout(true);
                }
            }

            /// <summary>
            /// 条背景
            /// </summary>
            [Description("条背景"), Category("样式"), DefaultValue(null)]
            [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
            public Color? Back { get; set; }

            #endregion

            #region 布局

            Rectangle rect_ful, rect_line_top;
            TabPageRect[] rects = new TabPageRect[0];
            public void LoadLayout(Tabs tabs, Rectangle rect, TabCollection items)
            {
                rect_ful = rect;
                owner = tabs;
                rects = Helper.GDI(g =>
                {
                    int gap = (int)(tabs.Gap * tabs.Dpi), gap2 = gap * 2, xy = 0;
                    int barSize = (int)(Size * tabs.Dpi), barPadding = (int)(Padding * tabs.Dpi), barPadding2 = barPadding * 2;
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

                                    if (it.Key.HasIcon && width == 0) width = ico_size + ico_gap;
                                    else
                                    {
                                        if (it.Key.HasIcon) width += ico_size + ico_gap * 2;
                                        if (close) width += close_size + ico_gap;
                                    }

                                    rect_it = new Rectangle(rect.X + xy, y, width + gap2, xy2);

                                    if (close)
                                    {
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, true).SetLine(barPadding, barPadding2, barSize));
                                        else rect_list.Add(new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, true).SetLine(barPadding, barPadding2, barSize));
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, true).SetLine(barPadding, barPadding2, barSize));
                                        else rect_list.Add(new TabPageRect(rect_it, it.Value, gap, true).SetLine(barPadding, barPadding2, barSize));
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(0, 0, 0, xy2 + tabs.Margin.Horizontal);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * tabs.Dpi);
                                rect_line_top = new Rectangle(0, rect.Bottom - xy2, rect.Width + tabs.Margin.Horizontal, barBackSize);
                            }
                            owner.scroll_max = xy - rect.Width;
                            owner.scroll_show = xy > rect.Width;
                            break;
                        case TabAlignment.Left:
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    bool close = closable && !it.Key.ReadOnly;
                                    // 计算项高度：当竖排文字时要包含 icon / text / close 的垂直空间
                                    int textHeight = it.Value.Height;
                                    int itemHeight;
                                    if (owner.IsRotate)
                                    {
                                        int iconPart = it.Key.HasIcon ? ico_size + ico_gap : 0;
                                        int closePart = (close) ? close_size + ico_gap : 0;
                                        // top gap + iconPart + text + closePart + bottom gap
                                        itemHeight = gap + iconPart + textHeight + closePart + gap;
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon && textHeight == 0) itemHeight = ico_size + gap;
                                        else itemHeight = textHeight + gap;
                                    }

                                    Rectangle rect_it = new Rectangle(rect.X, rect.Y + xy, xy2, itemHeight);
                                    Rectangle rect_line = new Rectangle(rect_it.X + xy2 - barSize, rect_it.Y + barPadding, barSize, rect_it.Height - barPadding2);
                                    TabPageRect p;
                                    if (owner.IsRotate)
                                    {
                                        // 使用简单构造后手动设置 ico/close/text 布局（避免原构造器的横向约束）
                                        p = new TabPageRect(rect_it, it.Value, gap, true, rw_tmp).SetLine(rect_line);

                                        // 根据旋转方向决定 icon 在上还是在下：顺时针 90 度 -> icon 在上；逆时针 -> icon 在下
                                        bool iconOnTop = owner.Rotate == TRotate.Clockwise_90;
                                        bool hasIcon = it.Key.HasIcon;
                                        bool hasClose = close;

                                        // icon 水平居中，垂直放置（上或下）
                                        if (hasIcon)
                                        {
                                            int ico_x = rect_it.X + (rect_it.Width - ico_size) / 2;
                                            int ico_y = iconOnTop ? rect_it.Y + gap : rect_it.Bottom - gap - ico_size;
                                            p.Rect_Ico = new Rectangle(ico_x, ico_y, ico_size, ico_size);
                                        }
                                        else p.Rect_Ico = Rectangle.Empty;

                                        // close 水平居中，放在另一端（与 icon 相对）
                                        if (hasClose)
                                        {
                                            int cs = close_size;
                                            int cx = rect_it.X + (rect_it.Width - cs) / 2;
                                            int cy = iconOnTop ? rect_it.Bottom - gap - cs : rect_it.Y + gap;
                                            p.Rect_Close = new Rectangle(cx, cy, cs, cs);
                                        }
                                        else p.Rect_Close = Rectangle.Empty;

                                        // 根据 icon 与 close 实际放置位置，分别在上/下端预留空间，文本区域为中间区
                                        int textTop = rect_it.Y + gap;
                                        int textBottom = rect_it.Bottom - gap;

                                        // 如果图标在上，从顶部推进
                                        if (hasIcon && iconOnTop) textTop += ico_size + ico_gap;
                                        // 如果关闭在上，从顶部推进（当 close 放上时）
                                        if (hasClose && !iconOnTop) textTop += close_size + ico_gap;

                                        // 如果关闭在下，从底部收缩
                                        if (hasClose && iconOnTop) textBottom -= close_size + ico_gap;
                                        // 如果图标在下，从底部收缩
                                        if (hasIcon && !iconOnTop) textBottom -= ico_size + ico_gap;

                                        int th = Math.Max(0, textBottom - textTop);
                                        int textW = rw_tmp ?? it.Value.Width;
                                        int tx = rect_it.X + Math.Max(0, (rect_it.Width - textW) / 2);
                                        p.Rect_Text = new Rectangle(tx, textTop, textW, th);
                                    }
                                    else
                                    {
                                        // 原有横向布局（图标/文本/关闭在一行）
                                        if (close)
                                        {
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, true, rw_tmp, rw).SetLine(rect_line);
                                            else p = new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, true, rw_tmp, rw).SetLine(rect_line);
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, true, rw_tmp).SetLine(rect_line);
                                            else p = new TabPageRect(rect_it, it.Value, gap, true, rw_tmp).SetLine(rect_line);
                                        }
                                    }

                                    rect_list.Add(p);
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(xy2 + tabs.Margin.Vertical, 0, 0, 0);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * tabs.Dpi);
                                rect_line_top = new Rectangle(rect.X + xy2 - barBackSize, 0, barBackSize, rect.Height + tabs.Margin.Vertical);
                            }
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
                                    Rectangle rect_line = new Rectangle(rect_it.X, rect_it.Y + barPadding, barSize, rect_it.Height - barPadding2);

                                    TabPageRect p;
                                    if (owner.IsRotate)
                                    {
                                        p = new TabPageRect(rect_it, it.Value, gap, true, rw_tmp).SetLine(rect_line);
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
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, false, rw_tmp, rw).SetLine(rect_line);
                                            else p = new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, false, rw_tmp, rw).SetLine(rect_line);
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon) p = new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, false, rw_tmp).SetLine(rect_line);
                                            else p = new TabPageRect(rect_it, it.Value, gap, false, rw_tmp).SetLine(rect_line);
                                        }
                                    }

                                    rect_list.Add(p);
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(0, 0, xy2 + tabs.Margin.Vertical, 0);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * tabs.Dpi);
                                rect_line_top = new Rectangle(x, 0, barBackSize, rect.Height + tabs.Margin.Vertical);
                            }
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

                                    if (it.Key.HasIcon && width == 0) width = ico_size + ico_gap;
                                    else
                                    {
                                        if (it.Key.HasIcon) width += ico_size + ico_gap * 2;
                                        if (close) width += close_size + ico_gap;
                                    }

                                    rect_it = new Rectangle(rect.X + xy, rect.Y, width + gap2, xy2);

                                    if (close)
                                    {
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, close_size, ico_gap, true).SetLineB(barPadding, barPadding2, barSize));
                                        else rect_list.Add(new TabPageRect(rect_it, false, it.Value, gap, close_size, ico_gap, true).SetLineB(barPadding, barPadding2, barSize));
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, gap, ico_size, ico_gap, true).SetLineB(barPadding, barPadding2, barSize));
                                        else rect_list.Add(new TabPageRect(rect_it, it.Value, gap, true).SetLineB(barPadding, barPadding2, barSize));
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(0, xy2 + tabs.Margin.Horizontal, 0, 0);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * tabs.Dpi);
                                rect_line_top = new Rectangle(0, rect.Y + xy2 - barBackSize, rect.Width + tabs.Margin.Horizontal, barBackSize);
                            }
                            owner.scroll_max = xy - rect.Width;
                            owner.scroll_show = xy > rect.Width;
                            break;
                    }
                    if (owner.scroll_show) owner.scroll_max += owner.SizeExceed(rect, rect_list[0].Rect, rect_list[rect_list.Count - 1].Rect);
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
                                        item.Rect_Line.Offset(0, oy);
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
                                        item.Rect_Line.Offset(ox, 0);
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
                                int h;
                                if (it.Key.HasIcon && it.Value.Height == 0) h = ico_size + gap;
                                else h = it.Value.Height + gap;
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
                if (rects.Length > 0)
                {
                    if (BackSize > 0)
                    {
                        using (var brush = new SolidBrush(Back ?? Colour.BorderSecondary.Get(nameof(Tabs), owner.ColorScheme)))
                        {
                            g.Fill(brush, rect_line_top);
                        }
                    }
                    if (owner.scroll_show) g.SetClip(owner.PaintExceedPre(rect_ful, rects[rects.Length - 1].Rect));
                    else g.SetClip(rect_ful);
                    using (var brush_fore = new SolidBrush(owner.ForeColor ?? Colour.Text.Get(nameof(Tabs), owner.ColorScheme)))
                    using (var brush_fill = new SolidBrush(owner.Fill ?? Colour.Primary.Get(nameof(Tabs), owner.ColorScheme)))
                    using (var brush_active = new SolidBrush(owner.FillActive ?? Colour.PrimaryActive.Get(nameof(Tabs), owner.ColorScheme)))
                    using (var brush_hover = new SolidBrush(owner.FillHover ?? Colour.PrimaryHover.Get(nameof(Tabs), owner.ColorScheme)))
                    {
                        if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                        if (AnimationBar)
                        {
                            PaintBar(g, AnimationBarValue, brush_fill);
                            int i = 0;
                            foreach (var page in items)
                            {
                                if (page.Visible)
                                {
                                    if (owner.SelectedIndex == i) PaintText(g, rects[i], owner, page, brush_fill);
                                    else if (owner.hover_i == i) PaintText(g, rects[i], owner, page, brush_hover);
                                    else PaintText(g, rects[i], owner, page, brush_fore);
                                }
                                i++;
                            }
                        }
                        else
                        {
                            int i = 0;
                            if (owner.pageMove == null)
                            {
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.SelectedIndex == i)//是否选中
                                        {
                                            PaintBar(g, rects[i].Rect_Line, brush_fill);
                                            PaintText(g, rects[i], owner, page, brush_fill);
                                        }
                                        else if (owner.hover_i == i) PaintText(g, rects[i], owner, page, owner.pageDown == page ? brush_active : brush_hover);
                                        else PaintText(g, rects[i], owner, page, brush_fore);
                                    }
                                    i++;
                                }
                            }
                            else
                            {
                                int sel = 0;
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.pageMove == page) sel = i;
                                        else PaintItem(owner, g, i, page, brush_fore, brush_fill, brush_hover, brush_active);
                                    }
                                    i++;
                                }
                                var state = g.Save();
                                g.TranslateTransform(-owner.offsetx, -owner.offsety);
                                PaintItem(owner, g, sel, owner.pageMove, brush_fore, brush_fill, brush_hover, brush_active);
                                g.Restore(state);
                            }
                        }

                        if (owner.scroll_show) owner.PaintExceed(g, brush_fore.Color, (int)(radius * owner.Dpi), rect_ful, rects[onetmp].Rect, rects[rects.Length - 1].Rect, false);
                    }
                }
            }

            void PaintItem(Tabs owner, Canvas g, int i, TabPage page, SolidBrush brush_fore, SolidBrush brush_fill, SolidBrush brush_hover, SolidBrush brush_active)
            {
                if (owner.SelectedIndex == i)
                {
                    PaintBar(g, rects[i].Rect_Line, brush_fill);
                    PaintText(g, rects[i], owner, page, brush_fill);
                }
                else if (owner.hover_i == i) PaintText(g, rects[i], owner, page, owner.pageDown == page ? brush_active : brush_hover);
                else PaintText(g, rects[i], owner, page, brush_fore);
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
            void PaintBar(Canvas g, RectangleF rect, SolidBrush brush)
            {
                if (radius > 0)
                {
                    using (var path = rect.RoundPath(radius * g.Dpi))
                    {
                        g.Fill(brush, path);
                    }
                }
                else g.Fill(brush, rect);
            }
            void PaintBar(Canvas g, Rectangle rect, SolidBrush brush)
            {
                if (radius > 0)
                {
                    using (var path = rect.RoundPath(radius * g.Dpi))
                    {
                        g.Fill(brush, path);
                    }
                }
                else g.Fill(brush, rect);
            }

            #endregion

            public TabPageRect GetTabRect(int i) => rects[i];

            #region 动画

            bool AnimationBar = false;
            RectangleF AnimationBarValue;
            AnimationTask? ThreadBar;

            void SetRect(int old, int value)
            {
                if (owner == null || owner.items == null || rects.Length == 0) return;
                if (owner.items.ListExceed(value)) return;
                if (owner.items.ListExceed(old))
                {
                    AnimationBarValue = rects[value].Rect_Line;
                    return;
                }
                ThreadBar?.Dispose();
                TabPageRect oldTab = rects[old], newTab = rects[value];
                owner.TabFocusMove(oldTab, newTab, value, rects.Length);
                Helper.GDI(g =>
                {
                    RectangleF OldValue = oldTab.Rect_Line, NewValue = newTab.Rect_Line;
                    if (AnimationBarValue.Height == 0) AnimationBarValue = OldValue;
                    if (Config.HasAnimation(nameof(Tabs)))
                    {
                        if (owner.alignment == TabAlignment.Left || owner.alignment == TabAlignment.Right)
                        {
                            if (OldValue.X == NewValue.X)
                            {
                                AnimationBarValue.X = OldValue.X;
                                AnimationBar = true;
                                float p_val = Math.Abs(NewValue.Y - AnimationBarValue.Y) * 0.09F, p_w_val = Math.Abs(NewValue.Height - AnimationBarValue.Height) * 0.1F, p_val2 = (NewValue.Y - AnimationBarValue.Y) * 0.5F;
                                ThreadBar = new AnimationTask(new AnimationLoopConfig(owner, () =>
                                {
                                    if (AnimationBarValue.Height != NewValue.Height)
                                    {
                                        if (NewValue.Height > OldValue.Height)
                                        {
                                            AnimationBarValue.Height += p_w_val;
                                            if (AnimationBarValue.Height > NewValue.Height) AnimationBarValue.Height = NewValue.Height;
                                        }
                                        else
                                        {
                                            AnimationBarValue.Height -= p_w_val;
                                            if (AnimationBarValue.Height < NewValue.Height) AnimationBarValue.Height = NewValue.Height;
                                        }
                                    }
                                    if (NewValue.Y > OldValue.Y)
                                    {
                                        if (AnimationBarValue.Y > p_val2) AnimationBarValue.Y += p_val / 2F;
                                        else AnimationBarValue.Y += p_val;
                                        if (AnimationBarValue.Y > NewValue.Y)
                                        {
                                            AnimationBarValue.Y = NewValue.Y;
                                            owner.Invalidate();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        AnimationBarValue.Y -= p_val;
                                        if (AnimationBarValue.Y < NewValue.Y)
                                        {
                                            AnimationBarValue.Y = NewValue.Y;
                                            owner.Invalidate();
                                            return false;
                                        }
                                    }
                                    owner.Invalidate();
                                    return true;
                                }, 10).SetEnd(() =>
                                {
                                    AnimationBarValue = NewValue;
                                    AnimationBar = false;
                                    owner.Invalidate();
                                }));
                                return;
                            }
                        }
                        else
                        {
                            if (OldValue.Y == NewValue.Y)
                            {
                                AnimationBarValue.Y = OldValue.Y;
                                AnimationBar = true;
                                float p_val = Math.Abs(NewValue.X - AnimationBarValue.X) * 0.09F, p_w_val = Math.Abs(NewValue.Width - AnimationBarValue.Width) * 0.1F, p_val2 = (NewValue.X - AnimationBarValue.X) * 0.5F;
                                ThreadBar = new AnimationTask(new AnimationLoopConfig(owner, () =>
                                {
                                    if (AnimationBarValue.Width != NewValue.Width)
                                    {
                                        if (NewValue.Width > OldValue.Width)
                                        {
                                            AnimationBarValue.Width += p_w_val;
                                            if (AnimationBarValue.Width > NewValue.Width) AnimationBarValue.Width = NewValue.Width;
                                        }
                                        else
                                        {
                                            AnimationBarValue.Width -= p_w_val;
                                            if (AnimationBarValue.Width < NewValue.Width) AnimationBarValue.Width = NewValue.Width;
                                        }
                                    }
                                    if (NewValue.X > OldValue.X)
                                    {
                                        if (AnimationBarValue.X > p_val2) AnimationBarValue.X += p_val / 2F;
                                        else AnimationBarValue.X += p_val;
                                        if (AnimationBarValue.X > NewValue.X)
                                        {
                                            AnimationBarValue.X = NewValue.X;
                                            owner.Invalidate();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        AnimationBarValue.X -= p_val;
                                        if (AnimationBarValue.X < NewValue.X)
                                        {
                                            AnimationBarValue.X = NewValue.X;
                                            owner.Invalidate();
                                            return false;
                                        }
                                    }
                                    owner.Invalidate();
                                    return true;
                                }, 10).SetEnd(() =>
                                {
                                    AnimationBarValue = NewValue;
                                    AnimationBar = false;
                                    owner.Invalidate();
                                }));
                                return;
                            }
                        }
                    }
                    AnimationBarValue = NewValue;
                    owner.Invalidate();
                });
            }

            #endregion

            public void SelectedIndexChanged(int i, int old) => SetRect(old, i);
            public void Dispose() => ThreadBar?.Dispose();

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
        }
    }
}