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
                    owner?.LoadLayout();
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
                    owner?.LoadLayout();
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
                    owner?.Invalidate();
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
                    owner?.LoadLayout();
                }
            }

            /// <summary>
            /// 条背景
            /// </summary>
            [Description("条背景"), Category("样式"), DefaultValue(null)]
            [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
            public Color? Back { get; set; }

            Rectangle rect_ful, rect_line_top;
            TabPageRect[] rects = new TabPageRect[0];
            public void LoadLayout(Tabs tabs, Rectangle rect, TabCollection items)
            {
                rect_ful = rect;
                owner = tabs;
                rects = Helper.GDI(g =>
                {
                    int gap = (int)(tabs.Gap * Config.Dpi), gapI = gap / 2, xy = 0, xy2 = 0;
                    int barSize = (int)(Size * Config.Dpi), barPadding = (int)(Padding * Config.Dpi), barPadding2 = barPadding * 2;
                    var rect_list = new List<TabPageRect>(items.Count);
                    var rect_dir = GetDir(tabs, g, items, gap, gapI, out int ico_size, out xy2);
                    switch (tabs.Alignment)
                    {
                        case TabAlignment.Bottom:
                            int y = rect.Bottom - xy2;
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    Rectangle rect_it;
                                    if (it.Key.HasIcon)
                                    {
                                        rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + ico_size + gapI, xy2);
                                        rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X + barPadding, rect_it.Y, rect_it.Width - barPadding2, barSize), it.Value, ico_size, gap, gapI));
                                    }
                                    else
                                    {
                                        rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap, xy2);
                                        rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X + barPadding, rect_it.Y, rect_it.Width - barPadding2, barSize)));
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(0, 0, 0, xy2);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * Config.Dpi);
                                rect_line_top = new Rectangle(rect.X, rect.Bottom - xy2, rect.Width, barBackSize);
                            }
                            owner.scroll_max = xy - rect.Width;
                            owner.scroll_show = xy > rect.Width;
                            break;
                        case TabAlignment.Left:
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    Rectangle rect_it = new Rectangle(rect.X, rect.Y + xy, xy2, it.Value.Height + gap);
                                    if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X + xy2 - barSize, rect_it.Y + barPadding, barSize, rect_it.Height - barPadding2), it.Value, ico_size, gap, gapI));
                                    else rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X + xy2 - barSize, rect_it.Y + barPadding, barSize, rect_it.Height - barPadding2)));

                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(xy2, 0, 0, 0);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * Config.Dpi);
                                rect_line_top = new Rectangle(rect.X + xy2 - barBackSize, rect.Y, barBackSize, rect.Height);
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
                                    Rectangle rect_it = new Rectangle(x, rect.Y + xy, xy2, it.Value.Height + gap);
                                    if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X, rect_it.Y + barPadding, barSize, rect_it.Height - barPadding2), it.Value, ico_size, gap, gapI));
                                    else rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X, rect_it.Y + barPadding, barSize, rect_it.Height - barPadding2)));

                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(0, 0, xy2, 0);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * Config.Dpi);
                                rect_line_top = new Rectangle(x, rect.Y, barBackSize, rect.Height);
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
                                    Rectangle rect_it;
                                    if (it.Key.HasIcon)
                                    {
                                        rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + ico_size + gapI, xy2);
                                        rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X + barPadding, rect_it.Bottom - barSize, rect_it.Width - barPadding2, barSize), it.Value, ico_size, gap, gapI));
                                    }
                                    else
                                    {
                                        rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap, xy2);
                                        rect_list.Add(new TabPageRect(rect_it, new Rectangle(rect_it.X + barPadding, rect_it.Bottom - barSize, rect_it.Width - barPadding2, barSize)));
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            tabs.SetPadding(0, xy2, 0, 0);
                            if (BackSize > 0)
                            {
                                int barBackSize = (int)(BackSize * Config.Dpi);
                                rect_line_top = new Rectangle(rect.Left, rect.Y + xy2 - barBackSize, rect.Width, barBackSize);
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

            public void Paint(Tabs owner, Canvas g, TabCollection items)
            {
                if (rects.Length > 0)
                {
                    if (BackSize > 0)
                    {
                        using (var brush = new SolidBrush(Back ?? Colour.BorderSecondary.Get("Tabs")))
                        {
                            g.Fill(brush, rect_line_top);
                        }
                    }
                    if (owner.scroll_show) g.SetClip(owner.PaintExceedPre(rect_ful, rects[rects.Length - 1].Rect.Height));
                    else g.SetClip(rect_ful);
                    using (var brush_fore = new SolidBrush(owner.ForeColor ?? Colour.Text.Get("Tabs")))
                    using (var brush_fill = new SolidBrush(owner.Fill ?? Colour.Primary.Get("Tabs")))
                    using (var brush_active = new SolidBrush(owner.FillActive ?? Colour.PrimaryActive.Get("Tabs")))
                    using (var brush_hover = new SolidBrush(owner.FillHover ?? Colour.PrimaryHover.Get("Tabs")))
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
                            foreach (var page in items)
                            {
                                if (page.Visible)
                                {
                                    if (owner.SelectedIndex == i)//是否选中
                                    {
                                        PaintBar(g, rects[i].Rect_Line, brush_fill);
                                        PaintText(g, rects[i], owner, page, brush_fill);
                                    }
                                    else if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover);
                                    else PaintText(g, rects[i], owner, page, brush_fore);
                                }
                                i++;
                            }
                        }

                        if (owner.scroll_show) owner.PaintExceed(g, brush_fore.Color, (int)(radius * Config.Dpi), rect_ful, rects[0].Rect, rects[rects.Length - 1].Rect, false);
                    }
                }
            }

            public TabPageRect GetTabRect(int i) => rects[i];

            #region 辅助

            Dictionary<TabPage, Size> GetDir(Tabs owner, Canvas g, TabCollection items, int gap, int gapI, out int ico_size, out int sizewh)
            {
                sizewh = 0;
                var size_t = g.MeasureString(Config.NullText, owner.Font);
                var rect_dir = new Dictionary<TabPage, Size>(items.Count);
                foreach (var item in items)
                {
                    var size = g.MeasureString(item.Text, owner.Font);
                    rect_dir.Add(item, size);
                }
                ico_size = (int)(size_t.Height * owner.IconRatio);
                switch (owner.Alignment)
                {
                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        foreach (var it in rect_dir)
                        {
                            if (it.Key.Visible)
                            {
                                int w;
                                if (it.Key.HasIcon) w = it.Value.Width + ico_size + gap + gapI;
                                else w = it.Value.Width + gap;
                                if (sizewh < w) sizewh = w;
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
                                int h = it.Value.Height + gap;
                                if (sizewh < h) sizewh = h;
                            }
                        }
                        break;
                }

                return owner.HandItemSize(rect_dir, ref sizewh);
            }

            void PaintText(Canvas g, TabPageRect rects, Tabs owner, TabPage page, SolidBrush brush)
            {
                if (page.HasIcon)
                {
                    if (page.Icon != null) g.Image(page.Icon, rects.Rect_Ico);
                    else if (page.IconSvg != null) g.GetImgExtend(page.IconSvg, rects.Rect_Ico, brush.Color);
                }
                g.String(page.Text, owner.Font, brush, rects.Rect_Text, owner.s_c);
                owner.PaintBadge(g, page, rects.Rect_Text);
            }
            void PaintBar(Canvas g, RectangleF rect, SolidBrush brush)
            {
                if (radius > 0)
                {
                    using (var path = rect.RoundPath(radius * Config.Dpi))
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
                    using (var path = rect.RoundPath(radius * Config.Dpi))
                    {
                        g.Fill(brush, path);
                    }
                }
                else g.Fill(brush, rect);
            }

            #endregion

            #region 动画

            bool AnimationBar = false;
            RectangleF AnimationBarValue;
            ITask? ThreadBar = null;

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
                Helper.GDI(g =>
                {
                    RectangleF OldValue = rects[old].Rect_Line, NewValue = rects[value].Rect_Line;
                    if (AnimationBarValue.Height == 0) AnimationBarValue = OldValue;
                    if (Config.Animation)
                    {
                        if (owner.alignment == TabAlignment.Left || owner.alignment == TabAlignment.Right)
                        {
                            if (OldValue.X == NewValue.X)
                            {
                                AnimationBarValue.X = OldValue.X;
                                AnimationBar = true;
                                float p_val = Math.Abs(NewValue.Y - AnimationBarValue.Y) * 0.09F, p_w_val = Math.Abs(NewValue.Height - AnimationBarValue.Height) * 0.1F, p_val2 = (NewValue.Y - AnimationBarValue.Y) * 0.5F;
                                ThreadBar = new ITask(owner, () =>
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
                                }, 10, () =>
                                {
                                    AnimationBarValue = NewValue;
                                    AnimationBar = false;
                                    owner.Invalidate();
                                });
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
                                ThreadBar = new ITask(owner, () =>
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
                                }, 10, () =>
                                {
                                    AnimationBarValue = NewValue;
                                    AnimationBar = false;
                                    owner.Invalidate();
                                });
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
            public void Dispose()
            {
                ThreadBar?.Dispose();
            }

            public void MouseWheel(int delta)
            {
                if (owner != null && owner.scroll_show)
                {
                    switch (owner.Alignment)
                    {
                        case TabAlignment.Left:
                        case TabAlignment.Right:
                            owner.scroll_x = 0;
                            owner.scroll_y -= delta;
                            break;
                        case TabAlignment.Top:
                        case TabAlignment.Bottom:
                        default:
                            owner.scroll_y = 0;
                            owner.scroll_x -= delta;
                            break;
                    }
                }
            }
            public void MouseMove(int x, int y) { }
            public bool MouseClick(TabPage page, int i, int x, int y) => false;
            public void MouseLeave() { }
        }

        /// <summary>
        /// 卡片样式
        /// </summary>
        public class StyleCard : IStyle
        {
            Tabs? owner;
            public StyleCard() { }
            public StyleCard(Tabs tabs) { owner = tabs; }


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
                    owner?.LoadLayout();
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
                    owner?.LoadLayout();
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
                    owner?.LoadLayout();
                }
            }

            TabPageRect[] rects = new TabPageRect[0];
            public void LoadLayout(Tabs tabs, Rectangle rect, TabCollection items)
            {
                owner = tabs;
                rects = Helper.GDI(g =>
                {
                    int gap = (int)(tabs.Gap * Config.Dpi), gapI = gap / 2, xy = 0, xy2 = 0;
                    int cardgap = (int)(Gap * Config.Dpi);

                    var rect_list = new List<TabPageRect>(items.Count);
                    var rect_dir = GetDir(tabs, g, items, gap, out int ico_size, out int close_size, out xy2);

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
                                    if (close)
                                    {
                                        if (it.Key.HasIcon)
                                        {
                                            rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + ico_size + close_size + gap * 2, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                        }
                                        else
                                        {
                                            rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + close_size + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                        }
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon)
                                        {
                                            rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + ico_size + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        }
                                        else
                                        {
                                            rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it));
                                        }
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(0, 0, 0, xy2);
                            owner.scroll_max = xy - rect.Width;
                            owner.scroll_show = xy > rect.Width;
                            break;
                        case TabAlignment.Left:
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    bool close = closable && !it.Key.ReadOnly;
                                    Rectangle rect_it;
                                    if (close)
                                    {
                                        rect_it = new Rectangle(rect.X, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                        else rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                    }
                                    else
                                    {
                                        rect_it = new Rectangle(rect.X, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        else rect_list.Add(new TabPageRect(rect_it));
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(xy2, 0, 0, 0);
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
                                    Rectangle rect_it;
                                    if (close)
                                    {
                                        rect_it = new Rectangle(x, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                        else rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                    }
                                    else
                                    {
                                        rect_it = new Rectangle(x, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        else rect_list.Add(new TabPageRect(rect_it));
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Height + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(0, 0, xy2, 0);
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
                                    if (close)
                                    {
                                        if (it.Key.HasIcon)
                                        {
                                            rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + ico_size + close_size + gap * 2, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                        }
                                        else
                                        {
                                            rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + close_size + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                        }
                                    }
                                    else
                                    {
                                        if (it.Key.HasIcon)
                                        {
                                            rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + ico_size + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        }
                                        else
                                        {
                                            rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it));
                                        }
                                    }
                                    it.Key.SetRect(rect_it);
                                    xy += rect_it.Width + cardgap;
                                }
                                else rect_list.Add(new TabPageRect());
                            }
                            xy -= cardgap;
                            tabs.SetPadding(0, xy2, 0, 0);
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

            public void Paint(Tabs owner, Canvas g, TabCollection items)
            {
                if (rects.Length == items.Count)
                {
                    using (var brush_fore = new SolidBrush(owner.ForeColor ?? Colour.Text.Get("Tabs")))
                    using (var brush_fill = new SolidBrush(owner.Fill ?? Colour.Primary.Get("Tabs")))
                    using (var brush_active = new SolidBrush(owner.FillActive ?? Colour.PrimaryActive.Get("Tabs")))
                    using (var brush_hover = new SolidBrush(owner.FillHover ?? Colour.PrimaryHover.Get("Tabs")))
                    using (var brush_bg = new SolidBrush(Fill ?? Colour.FillQuaternary.Get("Tabs")))
                    using (var brush_bg_hover = new SolidBrush(FillHover ?? Colour.FillQuaternary.Get("Tabs")))
                    using (var brush_bg_active = new SolidBrush(FillActive ?? Colour.BgContainer.Get("Tabs")))
                    {
                        var rect_t = owner.ClientRectangle;
                        int radius = (int)(Radius * Config.Dpi), bor = (int)(bordersize * Config.Dpi), bor2 = bor * 6, bor22 = bor2 * 2;
                        float borb2 = bor / 2F;
                        TabPage? sel = null;
                        int i = 0, select = owner.SelectedIndex;
                        switch (owner.Alignment)
                        {
                            case TabAlignment.Bottom:
                                int read_b_h = rects[0].Rect.Height + rects[0].Rect.X;
                                var rect_s_b = new Rectangle(rect_t.X, rect_t.Bottom - read_b_h, rect_t.Width, read_b_h);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_b, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_b);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (select == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, false, false, true, true))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover);
                                                else PaintText(g, rects[i], owner, page, brush_fore);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetClip();
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, false, false, true, true))
                                    {
                                        if (bor > 0)
                                        {
                                            using (var pen_bg = new Pen(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor))
                                            {
                                                float ly = rect_page.Y + borb2;
                                                g.DrawLine(pen_bg, rect_t.X, ly, rect_t.Right, ly);
                                                if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                                using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X + borb2, rect_page.Y - borb2, rect_page.Width - bor, rect_page.Height + borb2), radius, false, false, true, true))
                                                {
                                                    g.Fill(brush_bg_active, path2);
                                                }
                                                g.SetClip(new Rectangle(rect_page.X - bor, rect_page.Y + bor, rect_page.Width + bor2, rect_page.Height + bor));
                                                g.Draw(pen_bg, path);
                                                g.ResetClip();
                                            }
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill);
                                    }
                                }
                                break;
                            case TabAlignment.Left:
                                var rect_s_l = new Rectangle(rect_t.X, rect_t.Y, rects[0].Rect.Right, rect_t.Height);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_l, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_l);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.SelectedIndex == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, true, false, false, true))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover);
                                                else PaintText(g, rects[i], owner, page, brush_fore);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetClip();
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, true, false, false, true))
                                    {
                                        if (bor > 0)
                                        {
                                            using (var pen_bg = new Pen(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor))
                                            {
                                                float lx = rect_page.Right - borb2;
                                                g.DrawLine(pen_bg, lx, rect_t.Y, lx, rect_t.Bottom);
                                                if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                                using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X - borb2, rect_page.Y + borb2, rect_page.Width + borb2, rect_page.Height - bor), radius, true, false, false, true))
                                                {
                                                    g.Fill(brush_bg_active, path2);
                                                }
                                                g.SetClip(new RectangleF(rect_page.X - borb2, rect_page.Y - bor, rect_page.Width, rect_page.Height + bor2));
                                                g.Draw(pen_bg, path);
                                                g.ResetClip();
                                            }
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill);
                                    }
                                }
                                break;
                            case TabAlignment.Right:
                                int read_r_w = rects[0].Rect.Width + rects[0].Rect.Y;
                                var rect_s_r = new Rectangle(rect_t.Right - read_r_w, rect_t.Y, read_r_w, rect_t.Height);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_r, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_r);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.SelectedIndex == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, false, true, true, false))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover);
                                                else PaintText(g, rects[i], owner, page, brush_fore);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetClip();
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, false, true, true, false))
                                    {
                                        if (bor > 0)
                                        {
                                            using (var pen_bg = new Pen(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor))
                                            {
                                                float lx = rect_page.X + borb2;
                                                g.DrawLine(pen_bg, lx, rect_t.Y, lx, rect_t.Bottom);
                                                if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                                using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X - borb2, rect_page.Y + borb2, rect_page.Width + borb2, rect_page.Height - bor), radius, false, true, true, false))
                                                {
                                                    g.Fill(brush_bg_active, path2);
                                                }
                                                g.SetClip(new Rectangle(rect_page.X + bor, rect_page.Y - bor, rect_page.Width + bor, rect_page.Height + bor2));
                                                g.Draw(pen_bg, path);
                                                g.ResetClip();
                                            }
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill);
                                    }
                                }
                                break;
                            case TabAlignment.Top:
                            default:
                                var rect_s_t = new Rectangle(rect_t.X, rect_t.Y, rect_t.Width, rects[0].Rect.Bottom);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_t, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_t);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.SelectedIndex == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, true, true, false, false))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover);
                                                else PaintText(g, rects[i], owner, page, brush_fore);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetClip();
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, true, true, false, false))
                                    {
                                        if (bor > 0)
                                        {
                                            using (var pen_bg = new Pen(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor))
                                            {
                                                float ly = rect_page.Bottom - borb2;
                                                g.DrawLine(pen_bg, rect_t.X, ly, rect_t.Right, ly);
                                                if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                                using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X + borb2, rect_page.Y - borb2, rect_page.Width - bor, rect_page.Height + borb2), radius, true, true, false, false))
                                                {
                                                    g.Fill(brush_bg_active, path2);
                                                }
                                                g.SetClip(new Rectangle(rect_page.X - bor, rect_page.Y - bor, rect_page.Width + bor2, rect_page.Height));
                                                g.Draw(pen_bg, path);
                                                g.ResetClip();
                                            }
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill);
                                    }
                                }
                                break;
                        }

                        if (owner.scroll_show) owner.PaintExceed(g, brush_fore.Color, radius, rect_t, rects[0].Rect, rects[rects.Length - 1].Rect, true);
                    }
                }
            }

            public TabPageRect GetTabRect(int i) => rects[i];

            #region 辅助

            Dictionary<TabPage, Size> GetDir(Tabs owner, Canvas g, TabCollection items, int gap, out int ico_size, out int close_size, out int sizewh)
            {
                sizewh = 0;
                var size_t = g.MeasureString(Config.NullText, owner.Font);
                var rect_dir = new Dictionary<TabPage, Size>(items.Count);
                foreach (var item in items)
                {
                    var size = g.MeasureString(item.Text, owner.Font);
                    rect_dir.Add(item, size);
                }
                ico_size = (int)(size_t.Height * owner.IconRatio);
                close_size = (int)(size_t.Height * (owner.IconRatio * .8F));
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
                                    if (it.Key.HasIcon) w = it.Value.Width + ico_size + gap * 2;
                                    else w = it.Value.Width + gap;
                                    w += ico_size + gap;
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
                                    if (it.Key.HasIcon) w = it.Value.Width + ico_size + gap * 2;
                                    else w = it.Value.Width + gap;
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
                                int h = it.Value.Height + gap;
                                if (sizewh < h) sizewh = h;
                            }
                        }
                        break;
                }

                return owner.HandItemSize(rect_dir, ref sizewh);
            }

            void PaintText(Canvas g, TabPageRect rects, Tabs owner, TabPage page, SolidBrush brush)
            {
                if (page.HasIcon)
                {
                    if (page.Icon != null) g.Image(page.Icon, rects.Rect_Ico);
                    else if (page.IconSvg != null) g.GetImgExtend(page.IconSvg, rects.Rect_Ico, brush.Color);
                }
                if (closable)
                {
                    if (rects.hover_close == null) g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get("Tabs"));
                    else if (rects.hover_close.Animation) g.PaintIconClose(rects.Rect_Close, Helper.ToColor(rects.hover_close.Value + Colour.TextQuaternary.Get("Tabs").A, Colour.Text.Get("Tabs")));
                    else if (rects.hover_close.Switch) g.PaintIconClose(rects.Rect_Close, Colour.Text.Get("Tabs"));
                    else g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get("Tabs"));
                }
                g.String(page.Text, owner.Font, brush, rects.Rect_Text, owner.s_c);
                owner.PaintBadge(g, page, rects.Rect_Text);
            }

            #endregion

            public void SelectedIndexChanged(int i, int old)
            {
                owner?.Invalidate();
            }

            public void Dispose()
            {
                foreach (var item in rects)
                {
                    item.hover_close?.Dispose();
                }
            }

            public bool MouseClick(TabPage page, int i, int x, int y)
            {
                if (owner == null) return false;
                if (closable && !page.ReadOnly)
                {
                    if (rects[i].Rect_Close.Contains(x, y))
                    {
                        bool flag = true;
                        if (owner.ClosingPage != null) flag = owner.ClosingPage.Invoke(owner, new ClosingPageEventArgs(page));
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
                        if (item.hover_close == null) item.hover_close = new ITaskOpacity(owner);
                        if (i == owner.hover_i)
                        {
                            item.hover_close.MaxValue = Colour.Text.Get("Tabs").A - Colour.TextQuaternary.Get("Tabs").A;
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
            public void MouseWheel(int delta)
            {
                if (owner != null && owner.scroll_show)
                {
                    switch (owner.Alignment)
                    {
                        case TabAlignment.Left:
                        case TabAlignment.Right:
                            owner.scroll_x = 0;
                            owner.scroll_y -= delta;
                            break;
                        case TabAlignment.Top:
                        case TabAlignment.Bottom:
                        default:
                            owner.scroll_y = 0;
                            owner.scroll_x -= delta;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 卡片样式2
        /// </summary>
        public class StyleCard2 : IStyle
        {
            Tabs? owner;
            public StyleCard2() { }
            public StyleCard2(Tabs tabs) { owner = tabs; }


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
                    owner?.LoadLayout();
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
                    owner?.LoadLayout();
                }
            }
            public enum CloseType { none, always, activate }
            CloseType closable = CloseType.none;
            /// <summary>
            /// 可关闭
            /// </summary>
            [Description("可关闭"), Category("卡片"), DefaultValue(false)]
            public CloseType Closable
            {
                get => closable;
                set
                {
                    if (closable == value) return;
                    closable = value;
                    owner?.LoadLayout();
                }
            }

            TabPageRect[] rects = new TabPageRect[0];
            public void LoadLayout(Tabs tabs, Rectangle rect, TabCollection items)
            {
                owner = tabs;
                rects = Helper.GDI(g =>
                {
                    int gap = (int)(tabs.Gap * Config.Dpi), gapI = gap / 2, xy = 0, xy2 = 0;
                    int cardgap = (int)(Gap * Config.Dpi);

                    var rect_list = new List<TabPageRect>(items.Count);
                    var rect_dir = GetDir(tabs, g, items, gap, out int ico_size, out int close_size, out xy2);
                    if (closable != CloseType.none)
                    {
                        switch (tabs.Alignment)
                        {
                            case TabAlignment.Bottom:
                                int y = rect.Bottom - xy2;
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it = new Rectangle();
                                        if (it.Key.ReadOnly)
                                        {
                                            if (it.Key.HasIcon)
                                            {
                                                rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + ico_size + gap, xy2);
                                                rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                            }
                                            else
                                            {
                                                rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap, xy2);
                                                rect_list.Add(new TabPageRect(rect_it));
                                            }
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon)
                                            {
                                                rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + ico_size + close_size + gap * 2, xy2);
                                                rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                            }
                                            else
                                            {
                                                rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + close_size + gap, xy2);
                                                rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                            }
                                        }
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Width + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(0, 0, 0, xy2 + 2);
                                owner.scroll_max = xy - rect.Width;
                                owner.scroll_show = xy > rect.Width;
                                break;
                            case TabAlignment.Left:
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it = new Rectangle(rect.X, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.ReadOnly)
                                        {
                                            if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                            else rect_list.Add(new TabPageRect(rect_it));
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon)
                                                rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                            else
                                                rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                        }
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Height + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(xy2 + 2, 0, 0, 0);
                                owner.scroll_max = xy - rect.Height;
                                owner.scroll_show = xy > rect.Height;
                                break;
                            case TabAlignment.Right:
                                int x = rect.Right - xy2;
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it = new Rectangle(x, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.ReadOnly)
                                        {
                                            if (it.Key.HasIcon)
                                                rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                            else
                                                rect_list.Add(new TabPageRect(rect_it));
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon)
                                                rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                            else
                                                rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                        }
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Height + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(0, 0, xy2 + 2, 0);
                                owner.scroll_max = xy - rect.Height;
                                owner.scroll_show = xy > rect.Height;
                                break;
                            case TabAlignment.Top:
                            default:
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it = new Rectangle();
                                        if (it.Key.ReadOnly)
                                        {
                                            if (it.Key.HasIcon)
                                            {
                                                rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + ico_size + gap, xy2);
                                                rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                            }
                                            else
                                            {
                                                rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap, xy2);
                                                rect_list.Add(new TabPageRect(rect_it));
                                            }
                                        }
                                        else
                                        {
                                            if (it.Key.HasIcon)
                                            {
                                                rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + ico_size + close_size + gap * 2, xy2);
                                                rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, close_size, gap, gapI));
                                            }
                                            else
                                            {
                                                rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + close_size + gap, xy2);
                                                rect_list.Add(new TabPageRect(rect_it, false, it.Value, close_size, gap, gapI));
                                            }
                                        }
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Width + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(0, xy2 + 2, 0, 0);
                                owner.scroll_max = xy - rect.Width;
                                owner.scroll_show = xy > rect.Width;
                                break;
                        }
                    }
                    else
                    {
                        switch (tabs.Alignment)
                        {
                            case TabAlignment.Bottom:
                                int y = rect.Bottom - xy2;
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it;
                                        if (it.Key.HasIcon)
                                        {
                                            rect_it = new Rectangle(rect.X + xy, y, it.Value.Width + gap + ico_size + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        }
                                        else
                                        {
                                            rect_it = new Rectangle(rect.X + xy, y + 2 + bordersize, it.Value.Width + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it));
                                        }
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Width + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(0, 0, 0, xy2 + 2);
                                owner.scroll_max = xy - rect.Width;
                                owner.scroll_show = xy > rect.Width;
                                break;
                            case TabAlignment.Left:
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it = new Rectangle(rect.X, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.HasIcon) rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        else rect_list.Add(new TabPageRect(rect_it));
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Height + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(xy2 + 2, 0, 0, 0);
                                owner.scroll_max = xy - rect.Height;
                                owner.scroll_show = xy > rect.Height;
                                break;
                            case TabAlignment.Right:
                                int x = rect.Right - xy2;
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it = new Rectangle(x, rect.Y + xy, xy2, it.Value.Height + gap);
                                        if (it.Key.HasIcon)
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        else
                                            rect_list.Add(new TabPageRect(rect_it));
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Height + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(0, 0, xy2 + 2, 0);
                                owner.scroll_max = xy - rect.Height;
                                owner.scroll_show = xy > rect.Height;
                                break;
                            case TabAlignment.Top:
                            default:
                                foreach (var it in rect_dir)
                                {
                                    if (it.Key.Visible)
                                    {
                                        Rectangle rect_it;
                                        if (it.Key.HasIcon)
                                        {
                                            rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap + ico_size + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it, it.Value, ico_size, gap, gapI));
                                        }
                                        else
                                        {
                                            rect_it = new Rectangle(rect.X + xy, rect.Y, it.Value.Width + gap, xy2);
                                            rect_list.Add(new TabPageRect(rect_it));
                                        }
                                        it.Key.SetRect(rect_it);
                                        xy += rect_it.Width + cardgap;
                                    }
                                    else rect_list.Add(new TabPageRect());
                                }
                                xy -= cardgap;
                                tabs.SetPadding(0, xy2 + 2, 0, 0);
                                owner.scroll_max = xy - rect.Width;
                                owner.scroll_show = xy > rect.Width;
                                break;
                        }
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

            public void Paint(Tabs owner, Canvas g, TabCollection items)
            {
                if (rects.Length == items.Count)
                {
                    using (var brush_fore = new SolidBrush(owner.ForeColor ?? Colour.Text.Get("Tabs")))
                    using (var brush_fill = new SolidBrush(owner.Fill ?? Colour.Primary.Get("Tabs")))
                    using (var brush_active = new SolidBrush(owner.FillActive ?? Colour.PrimaryActive.Get("Tabs")))
                    using (var brush_hover = new SolidBrush(owner.FillHover ?? Colour.PrimaryHover.Get("Tabs")))
                    using (var brush_bg = new SolidBrush(Fill ?? Colour.FillQuaternary.Get("Tabs")))
                    using (var brush_bg_hover = new SolidBrush(FillHover ?? Colour.FillQuaternary.Get("Tabs")))
                    using (var brush_bg_active = new SolidBrush(FillActive ?? Colour.BgContainer.Get("Tabs")))
                    {
                        var rect_t = owner.ClientRectangle;
                        int radius = (int)(Radius * Config.Dpi), bor = (int)(bordersize * Config.Dpi), bor2 = bor * 6, bor22 = bor2 * 2;
                        float borb2 = bor / 2F;
                        TabPage? sel = null;
                        int i = 0, select = owner.SelectedIndex;
                        switch (owner.Alignment)
                        {
                            case TabAlignment.Bottom:
                                int read_b_h = rects[0].Rect.Height + rects[0].Rect.X;
                                var rect_s_b = new Rectangle(rect_t.X, rect_t.Bottom - read_b_h, rect_t.Width, read_b_h);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_b, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_b);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (select == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, true, true, true, true))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover, true);
                                                else PaintText(g, rects[i], owner, page, brush_fore, closable == CloseType.always ? true : false);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, true, true, true, true))
                                    {
                                        if (bor > 0)
                                        {
                                            float ly = rect_page.Y + borb2;
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X + borb2, rect_page.Y - borb2, rect_page.Width - bor, rect_page.Height + borb2), radius, true, true, true, true))
                                            {
                                                g.Fill(brush_bg_active, path2);
                                            }
                                            g.Draw(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor, path);
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill, true);
                                    }
                                }
                                break;
                            case TabAlignment.Left:
                                var rect_s_l = new Rectangle(rect_t.X, rect_t.Y, rects[0].Rect.Right, rect_t.Height);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_l, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_l);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.SelectedIndex == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, true, true, true, true))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover, true);
                                                else PaintText(g, rects[i], owner, page, brush_fore, closable == CloseType.always ? true : false);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, true, true, true, true))
                                    {
                                        if (bor > 0)
                                        {
                                            float lx = rect_page.Right - borb2;
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X - borb2, rect_page.Y + borb2, rect_page.Width + borb2, rect_page.Height - bor), radius, true, true, true, true))
                                            {
                                                g.Fill(brush_bg_active, path2);
                                            }
                                            g.Draw(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor, path);
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill, true);
                                    }
                                }
                                break;
                            case TabAlignment.Right:
                                int read_r_w = rects[0].Rect.Width + rects[0].Rect.Y;
                                var rect_s_r = new Rectangle(rect_t.Right - read_r_w, rect_t.Y, read_r_w, rect_t.Height);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_r, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_r);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.SelectedIndex == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, true, true, true, true))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover, true);
                                                else PaintText(g, rects[i], owner, page, brush_fore, closable == CloseType.always ? true : false);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, true, true, true, true))
                                    {
                                        if (bor > 0)
                                        {
                                            float lx = rect_page.X + borb2;
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X - borb2, rect_page.Y + borb2, rect_page.Width + borb2, rect_page.Height - bor), radius, true, true, true, true))
                                            {
                                                g.Fill(brush_bg_active, path2);
                                            }
                                            g.Draw(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor, path);
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill, true);
                                    }
                                }
                                break;
                            case TabAlignment.Top:
                            default:
                                var rect_s_t = new Rectangle(rect_t.X, rect_t.Y, rect_t.Width, rects[0].Rect.Bottom);
                                if (owner.scroll_show)
                                {
                                    g.SetClip(owner.PaintExceedPre(rect_s_t, rects[0].Rect.Height));
                                    g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                }
                                else g.SetClip(rect_s_t);
                                foreach (var page in items)
                                {
                                    if (page.Visible)
                                    {
                                        if (owner.SelectedIndex == i) sel = page;
                                        else
                                        {
                                            using (var path = Helper.RoundPath(page.Rect, radius, true, true, true, true))
                                            {
                                                g.Fill(owner.hover_i == i ? brush_bg_hover : brush_bg, path);
                                                if (bor > 0) g.Draw(border ?? Colour.BorderSecondary.Get("Tabs"), bor, path);
                                                if (owner.hover_i == i) PaintText(g, rects[i], owner, page, page.MDown ? brush_active : brush_hover, true);
                                                else PaintText(g, rects[i], owner, page, brush_fore, (closable == CloseType.always && !page.ReadOnly) ? true : false);
                                            }
                                        }
                                    }
                                    i++;
                                }
                                g.ResetTransform();
                                if (sel != null)//是否选中
                                {
                                    var rect_page = sel.Rect;
                                    using (var path = Helper.RoundPath(rect_page, radius, true, true, true, true))
                                    {
                                        if (bor > 0)
                                        {
                                            float ly = rect_page.Bottom - borb2;
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            using (var path2 = Helper.RoundPath(new RectangleF(rect_page.X + borb2, rect_page.Y - borb2, rect_page.Width - bor, rect_page.Height + borb2), radius, true, true, true, true))
                                            {
                                                g.Fill(brush_bg_active, path2);
                                            }
                                            g.Draw(BorderActive ?? Colour.BorderColor.Get("Tabs"), bor, path);
                                        }
                                        else
                                        {
                                            if (owner.scroll_show) g.TranslateTransform(-owner.scroll_x, -owner.scroll_y);
                                            g.Fill(brush_bg_active, path);
                                        }
                                        PaintText(g, rects[select], owner, sel, brush_fill, !sel.ReadOnly);
                                    }
                                }
                                break;
                        }

                        if (owner.scroll_show) owner.PaintExceed(g, brush_fore.Color, radius, rect_t, rects[0].Rect, rects[rects.Length - 1].Rect, true);
                    }
                }
            }

            public TabPageRect GetTabRect(int i) => rects[i];

            #region 辅助

            Dictionary<TabPage, Size> GetDir(Tabs owner, Canvas g, TabCollection items, int gap, out int ico_size, out int close_size, out int sizewh)
            {
                sizewh = 0;
                var size_t = g.MeasureString(Config.NullText, owner.Font);
                var rect_dir = new Dictionary<TabPage, Size>(items.Count);
                foreach (var item in items)
                {
                    var size = g.MeasureString(item.Text, owner.Font);
                    rect_dir.Add(item, size);
                }
                ico_size = (int)(size_t.Height * owner.IconRatio);
                close_size = (int)(size_t.Height * (owner.IconRatio * .8F));
                switch (owner.Alignment)
                {
                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        if (closable != CloseType.none)
                        {
                            foreach (var it in rect_dir)
                            {
                                if (it.Key.Visible)
                                {
                                    int w;
                                    if (it.Key.HasIcon) w = it.Value.Width + ico_size + gap * 2;
                                    else w = it.Value.Width + gap;
                                    w += ico_size + gap;
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
                                    if (it.Key.HasIcon) w = it.Value.Width + ico_size + gap * 2;
                                    else w = it.Value.Width + gap;
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
                                int h = it.Value.Height + gap;
                                if (sizewh < h) sizewh = h;
                            }
                        }
                        break;
                }

                return owner.HandItemSize(rect_dir, ref sizewh);
            }

            void PaintText(Canvas g, TabPageRect rects, Tabs owner, TabPage page, SolidBrush brush, bool closshow = false)
            {
                if (page.HasIcon)
                {
                    if (page.Icon != null) g.Image(page.Icon, rects.Rect_Ico);
                    else if (page.IconSvg != null) g.GetImgExtend(page.IconSvg, rects.Rect_Ico, brush.Color);
                }
                if (closable != CloseType.none && closshow)
                {
                    if (rects.hover_close == null) g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get("Tabs"));
                    else if (rects.hover_close.Animation) g.PaintIconClose(rects.Rect_Close, Helper.ToColor(rects.hover_close.Value + Colour.TextQuaternary.Get("Tabs").A, Colour.Text.Get("Tabs")));
                    else if (rects.hover_close.Switch) g.PaintIconClose(rects.Rect_Close, Colour.Text.Get("Tabs"));
                    else g.PaintIconClose(rects.Rect_Close, Colour.TextQuaternary.Get("Tabs"));
                }
                g.String(page.Text, owner.Font, brush, rects.Rect_Text, owner.s_c);
                owner.PaintBadge(g, page, rects.Rect_Text);
            }

            #endregion

            public void SelectedIndexChanged(int i, int old) => owner?.Invalidate();

            public void Dispose()
            {
                foreach (var item in rects)
                {
                    item.hover_close?.Dispose();
                }
            }

            public bool MouseClick(TabPage page, int i, int x, int y)
            {
                if (owner == null) return false;
                if (closable != CloseType.none)
                {
                    if (rects[i].Rect_Close.Contains(x, y))
                    {
                        bool flag = true;
                        if (owner.ClosingPage != null) flag = owner.ClosingPage.Invoke(owner, new ClosingPageEventArgs(page));
                        if (flag) owner.Pages.Remove(page);
                        return true;
                    }
                }
                return false;
            }
            public void MouseMove(int x, int y)
            {
                if (owner == null) return;
                if (closable != CloseType.none)
                {
                    int i = 0;
                    foreach (var item in rects)
                    {
                        if (item.hover_close == null) item.hover_close = new ITaskOpacity(owner);
                        if (i == owner.hover_i)
                        {
                            item.hover_close.MaxValue = Colour.Text.Get("Tabs").A - Colour.TextQuaternary.Get("Tabs").A;
                            item.hover_close.Switch = item.Rect_Close.Contains(x, y);
                        }
                        else item.hover_close.Switch = false;
                        i++;
                    }
                }
            }

            public bool MouseMovePre(int x, int y) => false;

            public void MouseLeave()
            {
                if (closable != CloseType.none)
                {
                    foreach (var item in rects)
                    {
                        if (item.hover_close == null) continue;
                        item.hover_close.Switch = false;
                    }
                }
            }

            public void MouseWheel(int delta)
            {
                if (owner != null && owner.scroll_show)
                {
                    switch (owner.Alignment)
                    {
                        case TabAlignment.Left:
                        case TabAlignment.Right:
                            owner.scroll_x = 0;
                            owner.scroll_y -= delta;
                            break;
                        case TabAlignment.Top:
                        case TabAlignment.Bottom:
                        default:
                            owner.scroll_y = 0;
                            owner.scroll_x -= delta;
                            break;
                    }
                }
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public interface IStyle
        {
            void LoadLayout(Tabs owner, Rectangle rect, TabCollection items);
            void Paint(Tabs owner, Canvas g, TabCollection items);
            void SelectedIndexChanged(int i, int old);
            bool MouseClick(TabPage page, int i, int x, int y);
            void MouseMove(int x, int y);
            void MouseLeave();
            void MouseWheel(int delta);
            void Dispose();

            TabPageRect GetTabRect(int i);
        }
    }

    public class TabPageRect
    {
        public TabPageRect() { }

        #region Line

        public TabPageRect(Rectangle rect, Rectangle rect_line)
        {
            Rect = Rect_Text = rect;
            Rect_Line = rect_line;
        }
        public TabPageRect(Rectangle rect_it, Rectangle rect_line, Size size, int ico_size, int gap, int gapI)
        {
            Rect = rect_it;
            Rect_Line = rect_line;
            Rect_Text = new Rectangle(rect_it.X + ico_size + gapI, rect_it.Y, size.Width + gap, rect_it.Height);
            Rect_Ico = new Rectangle(rect_it.X + gapI, rect_it.Y + (rect_it.Height - ico_size) / 2, ico_size, ico_size);
        }

        #endregion

        #region Card

        public TabPageRect(Rectangle rect)
        {
            Rect = Rect_Text = rect;
        }
        public TabPageRect(Rectangle rect_it, Size size, int ico_size, int gap, int gapI)
        {
            Rect = rect_it;
            Rect_Text = new Rectangle(rect_it.X + ico_size + gap, rect_it.Y + gapI, size.Width + gap, rect_it.Height - gap);
            Rect_Ico = new Rectangle(rect_it.X + gap, rect_it.Y + (rect_it.Height - ico_size) / 2, ico_size, ico_size);
        }
        public TabPageRect(Rectangle rect_it, Size size, int ico_size, int close_size, int gap, int gapI)
        {
            Rect = rect_it;
            Rect_Text = new Rectangle(rect_it.X + ico_size + gap, rect_it.Y + gapI, size.Width + gap, rect_it.Height - gap);
            Rect_Ico = new Rectangle(rect_it.X + gap, rect_it.Y + (rect_it.Height - ico_size) / 2, ico_size, ico_size);
            Rect_Close = new Rectangle(rect_it.Right - gap - close_size, rect_it.Y + (rect_it.Height - close_size) / 2, close_size, close_size);
        }
        public TabPageRect(Rectangle rect_it, bool test, Size size, int close_size, int gap, int gapI)
        {
            Rect = rect_it;
            int y = rect_it.Y + (rect_it.Height - close_size) / 2;
            Rect_Text = new Rectangle(rect_it.X, rect_it.Y + gapI, size.Width + gap, rect_it.Height - gap);
            Rect_Close = new Rectangle(rect_it.Right - gap - close_size, y, close_size, close_size);
        }

        #endregion

        /// <summary>
        /// 总区域
        /// </summary>
        public Rectangle Rect;
        /// <summary>
        /// 线条区域
        /// </summary>
        public Rectangle Rect_Line;
        /// <summary>
        /// 文本区域
        /// </summary>
        public Rectangle Rect_Text;
        /// <summary>
        /// 图标区域
        /// </summary>
        public Rectangle Rect_Ico;
        /// <summary>
        /// 关闭按钮区域
        /// </summary>
        public Rectangle Rect_Close;

        internal ITaskOpacity? hover_close;
    }
}