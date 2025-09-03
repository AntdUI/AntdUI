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
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormContextMenuStrip : ILayeredShadowFormOpacity, SubLayeredForm
    {
        ContextMenuStrip.Config config;
        public override bool MessageEnable => true;
        public override bool MessageCloseSub => true;
        public override bool MessageClickMe => false;

        Font? FontSub;
        public LayeredFormContextMenuStrip(ContextMenuStrip.Config _config) : base(250)
        {
            var point = _config.Location ?? MousePosition;
            PARENT = this;
            if (_config.TopMost)
            {
                Helper.SetTopMost(Handle);
                MessageCloseMouseLeave = true;
            }
            else _config.Control.SetTopMost(Handle);
            config = _config;
            Font = config.Font ?? config.Control.Font;
            rectsContent = LoadLayout(config.Items);
            ScrollBar = new ScrollBar(this, TAMode.Auto);
            switch (config.Align)
            {
                case TAlign.BL:
                case TAlign.LB:
                    point.X -= TargetRect.Width - shadow2;
                    break;
                case TAlign.TL:
                case TAlign.LT:
                    point.X -= TargetRect.Width - shadow2;
                    point.Y -= TargetRect.Height - shadow2;
                    break;
                case TAlign.Left:
                    point.X -= TargetRect.Width - shadow2;
                    point.Y -= TargetRect.Height / 2;
                    break;
                case TAlign.Right:
                    point.Y -= TargetRect.Height / 2;
                    break;
                case TAlign.Top:
                    point.X -= TargetRect.Width / 2;
                    point.Y -= TargetRect.Height - shadow2;
                    break;
                case TAlign.Bottom:
                    point.X -= TargetRect.Width / 2;
                    break;
                case TAlign.TR:
                case TAlign.RT:
                    point.Y -= TargetRect.Height - shadow2;
                    break;
            }
            Init(point);
            KeyCall = keys =>
            {
                if (keys == Keys.Escape)
                {
                    IClose();
                    return true;
                }
                if (keys == Keys.Enter)
                {
                    if (select_index > -1)
                    {
                        var it = rectsContent[select_index];
                        if (ClickItem(it)) return true;
                    }
                }
                else if (keys == Keys.Up)
                {
                    select_index--;
                    if (select_index < 0) select_index = rectsContent.Length - 1;
                    while (rectsContent[select_index].Tag == null)
                    {
                        select_index--;
                        if (select_index < 0) select_index = rectsContent.Length - 1;
                    }
                    foreach (var it in rectsContent) it.Hover = false;
                    FocusItem(rectsContent[select_index]);
                    return true;
                }
                else if (keys == Keys.Down)
                {
                    if (select_index == -1) select_index = 0;
                    else
                    {
                        select_index++;
                        if (select_index > rectsContent.Length - 1) select_index = 0;
                    }
                    while (rectsContent[select_index].Tag == null)
                    {
                        select_index++;
                        if (select_index > rectsContent.Length - 1) select_index = 0;
                    }
                    foreach (var it in rectsContent) it.Hover = false;
                    FocusItem(rectsContent[select_index]);
                    return true;
                }
                else if (keys == Keys.Left)
                {
                    IClose();
                    return true;
                }
                else if (keys == Keys.Right)
                {
                    if (select_index > -1)
                    {
                        var it = rectsContent[select_index];
                        if (it.Tag is ContextMenuStripItem item && item.Sub != null && item.Sub.Length > 0)
                        {
                            if (subForm == null) OpenDown(item, it.Rect, item.Sub);
                            else
                            {
                                subForm?.IClose();
                                subForm = null;
                            }
                            return true;
                        }
                    }
                    return true;
                }
                return false;
            };
        }

        ScrollBar ScrollBar;

        public LayeredFormContextMenuStrip(ContextMenuStrip.Config _config, LayeredFormContextMenuStrip parent, Point point, IContextMenuStripItem[] subs) : base(250)
        {
            PARENT = parent;
            config = _config;
            Font = config.Font ?? config.Control.Font;
            if (_config.TopMost) Helper.SetTopMost(Handle);
            else _config.Control.SetTopMost(Handle);
            rectsContent = LoadLayout(subs);
            ScrollBar = new ScrollBar(this, TAMode.Auto);
            Init(point);
        }

        public override string name => nameof(AntdUI.ContextMenuStrip);


        #region 布局

        void Init(Point point)
        {
            var screen = Screen.FromPoint(point).WorkingArea;
            if (point.X < screen.X) point.X = screen.X;
            else if (point.X > (screen.X + screen.Width) - TargetRect.Width + shadow) point.X = screen.X + screen.Width - TargetRect.Width + shadow;

            if (TargetRect.Height > screen.Height)
            {
                int vr = TargetRect.Height - shadow2, height = screen.Height - shadow2;
                SetSizeH(height);
                ScrollBar.SizeChange(new Rectangle(0, 0, TargetRect.Width - ScrollBar.SIZE, height));
                ScrollBar.SetVrSize(0, vr);
                SetLocation(point.X - shadow, screen.Y);
            }
            else
            {
                if (point.Y < screen.Y) point.Y = screen.Y;
                else if (point.Y > (screen.Y + screen.Height) - TargetRect.Height + shadow) point.Y = screen.Y + screen.Height - TargetRect.Height + shadow;
                SetLocation(point.X - shadow, point.Y - shadow);
            }
            if (OS.Win7OrLower) Select();
        }

        InRect[] LoadLayout(IContextMenuStripItem[] Items)
        {
            return Helper.GDI(g =>
            {
                var dpi = Config.Dpi;
                Radius = (int)(config.Radius * dpi);

                var list = new List<InRect>(Items.Length);
                int text_height = g.MeasureString(Config.NullText, Font).Height;

                int split = (int)Math.Round(1 * dpi), gap = (int)(text_height * config.Gap), icon_size = (int)(text_height * config.IconRatio), icon_gap = (int)(text_height * config.IconGap);
                int check_size = (int)(text_height * config.CheckRatio), gap_y = (int)(text_height * config.PaddRatio[1]), gap_x = (int)(text_height * config.PaddRatio[0]), gap2 = gap * 2, gap_x2 = gap_x * 2, gap_y2 = gap_y * 2;
                int item_height = text_height + gap_y2, icon_xy = (item_height - icon_size) / 2, check_xy = (item_height - check_size) / 2;

                ItemMaxWidth(Items, out bool has_checked, out bool has_icon, out bool has_subText, out bool has_subs);

                #region 计算最大宽度

                int maxw = 0;
                foreach (var it in Items)
                {
                    if (it is ContextMenuStripItem item)
                    {
                        list.Add(new InRect(item));
                        var size = g.MeasureText(item.Text + item.SubText, Font);
                        int tmp2 = size.Width;
                        if (has_checked) tmp2 += check_size + icon_gap;
                        if (has_icon) tmp2 += icon_size + icon_gap;
                        if (item.Sub != null && item.Sub.Length > 0) tmp2 += icon_size + icon_gap;
                        if (tmp2 > maxw) maxw = tmp2;
                    }
                    else if (it is ContextMenuStripItemDivider divider) list.Add(new InRect(divider));
                }
                if (has_subText) FontSub = new Font(Font.FontFamily, Font.Size * .8F);

                #endregion

                int w = maxw + gap_x2, y = 0;
                foreach (var it in list)
                {
                    if (it.Tag is ContextMenuStripItem item)
                    {
                        it.Rect = new Rectangle(gap, y + gap, w, item_height);
                        int x = it.Rect.X + gap_x, usx = 0;
                        if (has_checked)
                        {
                            it.RectCheck = new Rectangle(x, it.Rect.Y + check_xy, check_size, check_size);
                            x += check_size + icon_gap;
                            usx += check_size + icon_gap;
                        }
                        if (has_icon)
                        {
                            it.RectIcon = new Rectangle(x, it.Rect.Y + icon_xy, icon_size, icon_size);
                            x += icon_size + icon_gap;
                            usx += icon_size + icon_gap;
                        }
                        if (item.Sub != null && item.Sub.Length > 0)
                        {
                            it.RectSub = new Rectangle(it.Rect.Right - icon_gap - icon_size, it.Rect.Y + icon_xy, icon_size, icon_size);
                            usx += icon_size + icon_gap;
                        }
                        it.RectText = new Rectangle(x, it.Rect.Y + gap_y, maxw - usx, text_height);
                        y += item_height + gap2;
                    }
                    else if (it.Tag is ContextMenuStripItemDivider divider)
                    {
                        it.Rect = new Rectangle(gap, y, w, split);
                        y += split;
                    }
                }

                SetSize(maxw + gap2 + gap_x2, y);
                return list.ToArray();
            });
        }
        void ItemMaxWidth(IContextMenuStripItem[] items, out bool has_checked, out bool has_icon, out bool has_subText, out bool has_subs)
        {
            has_checked = has_icon = has_subText = has_subs = false;
            foreach (var it in items)
            {
                if (it is ContextMenuStripItem item)
                {
                    if (!has_checked && item.Checked) has_checked = true;
                    if (!has_icon && item.HasIcon) has_icon = true;
                    if (!has_subText && item.SubText != null) has_subText = true;
                    if (!has_subs && item.Sub != null && item.Sub.Length > 0) has_subs = true;
                    if (has_checked && has_icon && has_subText && has_subs) return;
                }
            }
        }

        #endregion

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            if (OS.Win7OrLower)
            {
                if (TargetRect.Contains(MousePosition)) return;
            }
            IClose();
            subForm?.IClose();
            subForm = null;
        }

        protected override void Dispose(bool disposing)
        {
            FontSub?.Dispose();
            subForm?.IClose();
            subForm = null;
            resetEvent?.WaitDispose();
            resetEvent = null;
            base.Dispose(disposing);
        }

        InRect[] rectsContent;

        #region 渲染

        readonly StringFormat sfl = Helper.SF_Ellipsis(lr: StringAlignment.Near), sfr = Helper.SF_Ellipsis(lr: StringAlignment.Far);
        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(Colour.BgElevated.Get("ContextMenuStrip")))
            {
                g.Fill(brush, path);
            }
        }
        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            using (var brush = new SolidBrush(Colour.Text.Get("ContextMenuStrip")))
            using (var brushSplit = new SolidBrush(Colour.Split.Get("ContextMenuStrip")))
            using (var brushSecondary = new SolidBrush(Colour.TextSecondary.Get("ContextMenuStrip")))
            using (var brushEnabled = new SolidBrush(Colour.TextQuaternary.Get("ContextMenuStrip")))
            {
                g.TranslateTransform(0, -ScrollBar.Value);
                foreach (var it in rectsContent)
                {
                    if (it.Tag is ContextMenuStripItem item)
                    {
                        if (it.Hover)
                        {
                            using (var path = Helper.RoundPath(it.Rect, Radius))
                            {
                                g.Fill(Colour.PrimaryBg.Get("ContextMenuStrip"), path);
                            }
                        }
                        if (item.Enabled)
                        {
                            if (FontSub != null) g.DrawText(item.SubText, FontSub, brushSecondary, it.RectText, sfr);
                            if (item.Fore.HasValue)
                            {
                                using (var brush_fore = new SolidBrush(item.Fore.Value))
                                {
                                    g.DrawText(item.Text, Font, brush_fore, it.RectText, sfl);
                                }
                            }
                            else g.DrawText(item.Text, Font, brush, it.RectText, sfl);

                            if (item.Sub != null && item.Sub.Length > 0)
                            {
                                using (var pen = new Pen(Colour.TextSecondary.Get("ContextMenuStrip"), 2F * Config.Dpi))
                                {
                                    pen.StartCap = pen.EndCap = LineCap.Round;
                                    g.DrawLines(pen, TAlignMini.Right.TriangleLines(it.RectSub));
                                }
                            }
                            if (item.Checked)
                            {
                                using (var pen = new Pen(Colour.Primary.Get("ContextMenuStrip"), 3F * Config.Dpi))
                                {
                                    g.DrawLines(pen, PaintArrow(it.RectCheck));
                                }
                            }
                            if (item.IconSvg != null)
                            {
                                using (var bmp = item.IconSvg.SvgToBmp(it.RectIcon.Width, it.RectIcon.Height, item.Fore ?? brush.Color))
                                {
                                    if (bmp != null) g.Image(bmp, it.RectIcon);
                                }
                            }
                            else if (item.Icon != null) g.Image(item.Icon, it.RectIcon);
                        }
                        else
                        {
                            if (FontSub != null) g.DrawText(item.SubText, FontSub, brushEnabled, it.RectText, sfr);
                            g.DrawText(item.Text, Font, brushEnabled, it.RectText, sfl);

                            if (item.Sub != null && item.Sub.Length > 0)
                            {
                                using (var pen = new Pen(Colour.TextQuaternary.Get("ContextMenuStrip"), 2F * Config.Dpi))
                                {
                                    pen.StartCap = pen.EndCap = LineCap.Round;
                                    g.DrawLines(pen, TAlignMini.Right.TriangleLines(it.RectSub));
                                }
                            }
                            if (item.Checked)
                            {
                                using (var pen = new Pen(Colour.Primary.Get("ContextMenuStrip"), 3F * Config.Dpi))
                                {
                                    g.DrawLines(pen, PaintArrow(it.RectCheck));
                                }
                            }
                            if (item.IconSvg != null)
                            {
                                using (var bmp = item.IconSvg.SvgToBmp(it.RectIcon.Width, it.RectIcon.Height, brushEnabled.Color))
                                {
                                    if (bmp != null) g.Image(bmp, it.RectIcon);
                                }
                            }
                            else if (item.Icon != null) g.Image(item.Icon, it.RectIcon);
                        }
                    }
                    else if (it.Tag is ContextMenuStripItemDivider item_divider) g.Fill(brushSplit, it.Rect);
                }
                g.Restore(state);
                ScrollBar.Paint(g);
            }
        }

        PointF[] PaintArrow(Rectangle rect)
        {
            float size = rect.Height * .15F, size2 = rect.Height * .2F, size3 = rect.Height * .26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        #endregion

        #region 鼠标

        int select_index = -1;
        InRect? MDown;
        protected override void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseDown(x, y))
            {
                OnTouchDown(x, y);
                select_index = -1;
                if (button == MouseButtons.Left)
                {
                    int ry = ScrollBar.Show ? ScrollBar.Value : 0;
                    for (int i = 0; i < rectsContent.Length; i++)
                    {
                        var it = rectsContent[i];
                        if (it.Tag is ContextMenuStripItem item && item.Enabled && it.Rect.Contains(x, y + ry))
                        {
                            select_index = i;
                            MDown = it;
                            return;
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseUp() && OnTouchUp())
            {
                int ry = ScrollBar.Show ? ScrollBar.Value : 0;
                if (MDown == null) return;
                var it = MDown;
                MDown = null;
                if (it.Rect.Contains(x, y + ry)) ClickItem(it);
            }
        }

        bool ClickItem(InRect it)
        {
            if (it.Tag is ContextMenuStripItem item)
            {
                if (item.Sub == null || item.Sub.Length == 0)
                {
                    if (Config.HasAnimation(name))
                    {
                        IClose();
                        CloseSub();
                        resetEvent = new ManualResetEvent(false);
                        ITask.Run(() =>
                        {
                            if (resetEvent.Wait(false)) return;
                            if (config.CallSleep > 0) Thread.Sleep(config.CallSleep);
                            config.Control.BeginInvoke(new Action(() => config.Call(item)));
                        });
                    }
                    else
                    {
                        if (config.CallSleep > 0)
                        {
                            IClose();
                            CloseSub();
                            ITask.Run(() =>
                            {
                                Thread.Sleep(config.CallSleep);
                                config.Control.BeginInvoke(new Action(() => config.Call(item)));
                            });
                        }
                        else
                        {
                            IClose();
                            CloseSub();
                            config.Call(item);
                        }
                    }
                }
                else
                {
                    if (subForm == null) OpenDown(item, it.Rect, item.Sub);
                    else
                    {
                        subForm?.IClose();
                        subForm = null;
                    }
                }
                return true;
            }
            return false;
        }

        void CloseSub()
        {
            LayeredFormContextMenuStrip item = this;
            while (item.PARENT is LayeredFormContextMenuStrip form)
            {
                if (item == form) return;
                form.IClose();
                item = form;
            }
        }

        void FocusItem(InRect it)
        {
            if (it.SetHover(true))
            {
                if (ScrollBar.Show) ScrollBar.Value = it.Rect.Y - it.Rect.Height;
                Print();
            }
        }

        int oldSub = -1;
        protected override void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollBar.MouseMove(x, y) && OnTouchMove(x, y))
            {
                int count = 0, hand = -1;
                int ry = ScrollBar.Show ? ScrollBar.Value : 0;
                for (int i = 0; i < rectsContent.Length; i++)
                {
                    var it = rectsContent[i];
                    if (it.Tag is ContextMenuStripItem item)
                    {
                        if (item.Enabled)
                        {
                            bool hover = it.Rect.Contains(x, y + ry);
                            if (hover) hand = i;
                            if (it.Hover != hover)
                            {
                                it.Hover = hover;
                                count++;
                            }
                        }
                        else
                        {
                            if (it.Hover != false)
                            {
                                it.Hover = false;
                                count++;
                            }
                        }
                    }
                }
                SetCursor(hand > 0);
                if (count > 0) Print();
                select_index = hand;
                if (hand > -1)
                {
                    SetCursor(true);
                    if (oldSub == hand) return;
                    var it = rectsContent[hand];
                    oldSub = hand;
                    subForm?.IClose();
                    subForm = null;
                    if (it.Tag is ContextMenuStripItem item && item.Sub != null && item.Sub.Length > 0) OpenDown(item, it.Rect, item.Sub);
                }
                else
                {
                    oldSub = -1;
                    subForm?.IClose();
                    subForm = null;
                    SetCursor(false);
                }
            }
        }
        void OpenDown(ContextMenuStripItem item, Rectangle rect, IContextMenuStripItem[] sub)
        {
            var trect = TargetRect;
            subForm = new LayeredFormContextMenuStrip(config, this, new Point(trect.X + trect.Width - rect.X - shadow2, trect.Y + rect.Y + shadow / 2 - ScrollBar.Value), sub);
            subForm.Show(this);
        }

        protected override void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta) => ScrollBar.MouseWheel(delta);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        ManualResetEvent? resetEvent;

        LayeredFormContextMenuStrip? subForm;
        ILayeredForm? SubLayeredForm.SubForm() => subForm;

        #endregion

        class InRect
        {
            public InRect(IContextMenuStripItem tag)
            {
                Tag = tag;
            }

            public IContextMenuStripItem Tag { get; set; }

            public bool Hover { get; set; }

            #region 区域

            public Rectangle Rect { get; set; }
            public Rectangle RectCheck { get; set; }
            public Rectangle RectIcon { get; set; }
            /// <summary>
            /// 文本区域
            /// </summary>
            public Rectangle RectText { get; set; }
            public Rectangle RectSub { get; set; }

            #endregion

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
        }
    }
}