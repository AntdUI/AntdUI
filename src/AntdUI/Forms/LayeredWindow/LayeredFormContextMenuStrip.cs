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
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormContextMenuStrip : ILayeredFormOpacity, SubLayeredForm
    {
        ContextMenuStrip.Config config;
        public override bool MessageEnable => true;
        public override bool MessageCloseSub => true;

        Font FontSub;
        float radius = 0;
        public LayeredFormContextMenuStrip(ContextMenuStrip.Config _config)
        {
            if (_config.TopMost)
            {
                PARENT = this;
                Helper.SetTopMost(Handle);
                MessageCloseMouseLeave = true;
            }
            else _config.Control.SetTopMost(Handle);
            var point = _config.Location ?? MousePosition;
            maxalpha = 250;
            config = _config;
            Font = config.Font ?? config.Control.Font;
            FontSub = Font;
            rectsContent = Init(config.Items);
            scrollY = new ScrollY(this);
            switch (config.Align)
            {
                case TAlign.BL:
                case TAlign.LB:
                    point.X -= TargetRect.Width;
                    point.Offset(10, -10);
                    break;
                case TAlign.TL:
                case TAlign.LT:
                    point.X -= TargetRect.Width;
                    point.Y -= TargetRect.Height;
                    point.Offset(10, 10);
                    break;
                case TAlign.Left:
                    point.X -= TargetRect.Width;
                    point.Y -= TargetRect.Height / 2;
                    point.Offset(10, 0);
                    break;
                case TAlign.Right:
                    point.Y -= TargetRect.Height / 2;
                    point.Offset(-10, 0);
                    break;
                case TAlign.Top:
                    point.X -= TargetRect.Width / 2;
                    point.Y -= TargetRect.Height;
                    point.Offset(0, 10);
                    break;
                case TAlign.Bottom:
                    point.X -= TargetRect.Width / 2;
                    point.Offset(0, -10);
                    break;
                case TAlign.TR:
                case TAlign.RT:
                    point.Y -= TargetRect.Height;
                    point.Offset(-10, 10);
                    break;
                case TAlign.BR:
                case TAlign.RB:
                default:
                    point.Offset(-10, -10);
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
                        if (it.Tag != null && it.Tag.Sub != null && it.Tag.Sub.Length > 0)
                        {
                            if (subForm == null)
                            {
                                subForm = new LayeredFormContextMenuStrip(config, this, new Point(TargetRect.X + (it.Rect.X + it.Rect.Width) - 20, TargetRect.Y + it.Rect.Y - 20 - (scrollY.Show ? (int)scrollY.Value : 0)), it.Tag.Sub);
                                subForm.Show(this);
                            }
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

        ScrollY scrollY;

        public LayeredFormContextMenuStrip(ContextMenuStrip.Config _config, LayeredFormContextMenuStrip parent, Point point, IContextMenuStripItem[] subs)
        {
            PARENT = parent;
            MessageCloseMouseLeave = true;
            maxalpha = 250;
            config = _config;
            Font = config.Font ?? config.Control.Font;
            FontSub = Font;
            if (_config.TopMost) Helper.SetTopMost(Handle);
            else _config.Control.SetTopMost(Handle);
            rectsContent = Init(subs);
            scrollY = new ScrollY(this);
            Init(point);
        }

        void Init(Point point)
        {
            var screen = Screen.FromPoint(point).WorkingArea;
            if (point.X < screen.X) point.X = screen.X;
            else if (point.X > (screen.X + screen.Width) - TargetRect.Width) point.X = screen.X + screen.Width - TargetRect.Width;

            if (TargetRect.Height > screen.Height)
            {
                int gap_y = rectsContent[0].y / 2 / 2, vr = TargetRect.Height, height = screen.Height;
                scrollY.Rect = new Rectangle(TargetRect.Width - gap_y - scrollY.SIZE, 10 + gap_y, scrollY.SIZE, height - 20 - gap_y * 2);
                scrollY.Show = true;
                scrollY.SetVrSize(vr, height);
                SetSizeH(height);
            }

            if (point.Y < screen.Y) point.Y = screen.Y;
            else if (point.Y > (screen.Y + screen.Height) - TargetRect.Height) point.Y = screen.Y + screen.Height - TargetRect.Height;

            SetLocation(point);
        }

        bool has_subtext = false;
        InRect[] Init(IContextMenuStripItem[] Items)
        {
            return Helper.GDI(g =>
            {
                var dpi = Config.Dpi;
                radius = (int)(config.Radius * dpi);
                int split = (int)Math.Round(1F * dpi), sp = (int)Math.Round(8F * dpi), spm = sp / 2, padding = (int)Math.Round(16 * dpi), padding2 = padding * 2;
                Padding = new Padding(padding);
                var _rectsContent = new List<InRect>(Items.Length);
                int usew = 0, useh = 0, has_icon = 0, has_checked = 0, has_sub = 0;
                foreach (var it in Items)
                {
                    if (it is ContextMenuStripItem item)
                    {
                        if (!has_subtext && item.SubText != null) has_subtext = true;
                        var size = g.MeasureString(item.Text + item.SubText, Font);
                        int w = (int)Math.Ceiling(size.Width), hc = (int)Math.Ceiling(size.Height), h = hc + sp;
                        if (has_sub == 0 && (item.Sub != null && item.Sub.Length > 0)) has_sub = hc;
                        if (has_icon == 0 && (item.Icon != null || item.IconSvg != null)) has_icon = (int)(hc * 0.68F);
                        if (has_checked == 0 && item.Checked) has_checked = (int)(hc * 0.8F);

                        if (w > usew) usew = w;
                        _rectsContent.Add(new InRect(item, padding + useh, h));
                        useh += h + spm;
                    }
                    else if (it is ContextMenuStripItemDivider divider)
                    {
                        _rectsContent.Add(new InRect(padding + useh, sp));
                        useh += sp;
                    }
                }
                if (has_subtext) FontSub = new Font(FontSub.FontFamily, FontSub.Size * 0.8F);

                int use_r;
                if (has_icon > 0 || has_checked > 0)
                {
                    if (has_icon > 0 && has_checked > 0) use_r = has_icon + has_checked + spm * 3;
                    else if (has_icon > 0) use_r = has_icon + spm;
                    else use_r = has_checked + spm;
                }
                else use_r = 0;

                int sp2 = sp * 2, x = padding + use_r;
                usew += use_r;
                int readw = usew + has_sub + padding2 + sp2;
                foreach (var it in _rectsContent)
                {
                    if (it.Tag == null) it.Rect = new Rectangle(10, it.y + (it.h - split) / 2, readw - 20, split);
                    else
                    {
                        it.Rect = new Rectangle(padding, it.y, usew + has_sub + sp2, it.h);

                        if (it.Tag.Sub != null && it.Tag.Sub.Length > 0) it.RectSub = new Rectangle(it.Rect.Right - spm - has_sub, it.y + (it.h - has_sub) / 2, has_sub, has_sub);

                        int usex = padding + spm;
                        if (has_checked > 0)
                        {
                            if (it.Tag.Checked) it.RectCheck = new Rectangle(usex + spm, it.y + (it.h - has_checked) / 2, has_checked, has_checked);
                            usex += has_checked + sp;
                            it.RectT = new Rectangle(x + sp, it.y, usew - use_r - spm, it.h);
                        }
                        else it.RectT = new Rectangle(x + sp, it.y, usew - use_r, it.h);
                        if (has_icon > 0 && it.Tag.Icon != null || it.Tag.IconSvg != null) it.RectIcon = new Rectangle(usex + spm, it.y + (it.h - has_icon) / 2, has_icon, has_icon);
                    }
                }

                SetSize(readw, useh - spm + padding2);
                return _rectsContent.ToArray();
            });
        }

        protected override void OnDeactivate(EventArgs e)
        {
            IClose();
            subForm?.IClose();
            subForm = null;
            base.OnDeactivate(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (has_subtext) FontSub.Dispose();
            subForm?.IClose();
            subForm = null;
            resetEvent?.WaitDispose();
            resetEvent = null;
            base.Dispose(disposing);
        }

        InRect[] rectsContent;

        #region 渲染

        readonly StringFormat stringLeft = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        readonly StringFormat stringRight = Helper.SF_Ellipsis(lr: StringAlignment.Far);

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path_sh = DrawShadow(g, rect, rect_read))
                {
                    using (var brush = new SolidBrush(Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path_sh);
                    }
                    using (var brush = new SolidBrush(Style.Db.Text))
                    using (var brushSplit = new SolidBrush(Style.Db.Split))
                    using (var brushSecondary = new SolidBrush(Style.Db.TextSecondary))
                    using (var brushEnabled = new SolidBrush(Style.Db.TextQuaternary))
                    {
                        if (scrollY.Show)
                        {
                            g.SetClip(path_sh);
                            g.TranslateTransform(0, -scrollY.Value);
                        }
                        foreach (var it in rectsContent)
                        {
                            if (it.Tag == null) g.FillRectangle(brushSplit, it.Rect);
                            else
                            {
                                if (it.Hover)
                                {
                                    using (var path = Helper.RoundPath(it.Rect, radius))
                                    {
                                        using (var brush_hover = new SolidBrush(Style.Db.PrimaryBg))
                                        {
                                            g.FillPath(brush_hover, path);
                                        }
                                    }
                                }
                                if (it.Tag.Enabled)
                                {
                                    g.DrawStr(it.Tag.SubText, FontSub, brushSecondary, it.RectT, stringRight);
                                    if (it.Tag.Fore.HasValue)
                                    {
                                        using (var brush_fore = new SolidBrush(it.Tag.Fore.Value))
                                        {
                                            g.DrawStr(it.Tag.Text, Font, brush_fore, it.RectT, stringLeft);
                                        }
                                    }
                                    else g.DrawStr(it.Tag.Text, Font, brush, it.RectT, stringLeft);

                                    if (it.Tag.Sub != null && it.Tag.Sub.Length > 0)
                                    {
                                        using (var pen = new Pen(Style.Db.TextSecondary, 2F * Config.Dpi))
                                        {
                                            pen.StartCap = pen.EndCap = LineCap.Round;
                                            g.DrawLines(pen, TAlignMini.Right.TriangleLines(it.RectSub));
                                        }
                                    }
                                    if (it.Tag.Checked)
                                    {
                                        using (var pen = new Pen(Style.Db.Primary, 3F * Config.Dpi))
                                        {
                                            g.DrawLines(pen, PaintArrow(it.RectCheck));
                                        }
                                    }
                                    if (it.Tag.IconSvg != null)
                                    {
                                        using (var bmp = it.Tag.IconSvg.SvgToBmp(it.RectIcon.Width, it.RectIcon.Height, it.Tag.Fore ?? brush.Color))
                                        {
                                            if (bmp != null) g.DrawImage(bmp, it.RectIcon);
                                        }
                                    }
                                    else if (it.Tag.Icon != null) g.DrawImage(it.Tag.Icon, it.RectIcon);
                                }
                                else
                                {
                                    g.DrawStr(it.Tag.SubText, FontSub, brushEnabled, it.RectT, stringRight);
                                    g.DrawStr(it.Tag.Text, Font, brushEnabled, it.RectT, stringLeft);

                                    if (it.Tag.Sub != null && it.Tag.Sub.Length > 0)
                                    {
                                        using (var pen = new Pen(Style.Db.TextQuaternary, 2F * Config.Dpi))
                                        {
                                            pen.StartCap = pen.EndCap = LineCap.Round;
                                            g.DrawLines(pen, TAlignMini.Right.TriangleLines(it.RectSub));
                                        }
                                    }
                                    if (it.Tag.Checked)
                                    {
                                        using (var pen = new Pen(Style.Db.Primary, 3F * Config.Dpi))
                                        {
                                            g.DrawLines(pen, PaintArrow(it.RectCheck));
                                        }
                                    }
                                    if (it.Tag.IconSvg != null)
                                    {
                                        using (var bmp = it.Tag.IconSvg.SvgToBmp(it.RectIcon.Width, it.RectIcon.Height, brushEnabled.Color))
                                        {
                                            if (bmp != null) g.DrawImage(bmp, it.RectIcon);
                                        }
                                    }
                                    else if (it.Tag.Icon != null) g.DrawImage(it.Tag.Icon, it.RectIcon);
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

        internal PointF[] PaintArrow(Rectangle rect)
        {
            float size = rect.Height * 0.15F, size2 = rect.Height * 0.2F, size3 = rect.Height * 0.26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="rect_read">真实区域</param>
        GraphicsPath DrawShadow(Graphics g, Rectangle rect_client, Rectangle rect_read)
        {
            var path = rect_read.RoundPath(radius);
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
                {
                    shadow_temp?.Dispose();
                    shadow_temp = path.PaintShadow(rect_client.Width, rect_client.Height);
                }
                g.DrawImage(shadow_temp, rect_client, 0.2F);
            }
            return path;
        }

        #endregion

        #region 鼠标

        int select_index = -1;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (scrollY.MouseDown(e.Location))
            {
                OnTouchDown(e.X, e.Y);
                select_index = -1;
                if (e.Button == MouseButtons.Left)
                {
                    int y = scrollY.Show ? (int)scrollY.Value : 0;
                    for (int i = 0; i < rectsContent.Length; i++)
                    {
                        var it = rectsContent[i];
                        if (it.Tag != null && it.Tag.Enabled && it.Rect.Contains(e.X, e.Y + y))
                        {
                            select_index = i;
                            it.Down = true;
                            base.OnMouseDown(e);
                            return;
                        }
                    }
                }
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (scrollY.MouseUp(e.Location) && OnTouchUp())
            {
                int y = scrollY.Show ? (int)scrollY.Value : 0;
                foreach (var it in rectsContent)
                {
                    if (it.Down)
                    {
                        if (it.Rect.Contains(e.X, e.Y + y))
                        {
                            if (it.Tag != null)
                            {
                                if (it.Tag.Sub == null || it.Tag.Sub.Length == 0)
                                {
                                    IClose();
                                    LayeredFormContextMenuStrip item = this;
                                    while (item.PARENT is LayeredFormContextMenuStrip form && item.PARENT != form)
                                    {
                                        form.IClose();
                                        item = form;
                                    }
                                    resetEvent = new ManualResetEvent(false);
                                    ITask.Run(() =>
                                    {
                                        if (Config.Animation && resetEvent.Wait(false)) return;
                                        if (config.CallSleep > 0) Thread.Sleep(config.CallSleep);
                                        config.Control.BeginInvoke(new Action(() =>
                                        {
                                            config.Call(it.Tag);
                                        }));
                                    });
                                }
                            }
                        }
                        it.Down = false;
                        return;
                    }
                }
            }
            base.OnMouseUp(e);
        }

        bool ClickItem(InRect it)
        {
            if (it.Tag != null)
            {
                if (it.Tag.Sub == null || it.Tag.Sub.Length == 0)
                {
                    IClose();
                    LayeredFormContextMenuStrip item = this;
                    while (item.PARENT is LayeredFormContextMenuStrip form)
                    {
                        form.IClose();
                        item = form;
                    }
                    resetEvent = new ManualResetEvent(false);
                    ITask.Run(() =>
                    {
                        if (resetEvent.Wait(false)) return;
                        if (config.CallSleep > 0) Thread.Sleep(config.CallSleep);
                        config.Control.BeginInvoke(new Action(() =>
                        {
                            config.Call(it.Tag);
                        }));
                    });
                }
                else
                {
                    if (subForm == null)
                    {
                        subForm = new LayeredFormContextMenuStrip(config, this, new Point(TargetRect.X + (it.Rect.X + it.Rect.Width) - 20, TargetRect.Y + it.Rect.Y - 20 - (scrollY.Show ? (int)scrollY.Value : 0)), it.Tag.Sub);
                        subForm.Show(this);
                    }
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

        void FocusItem(InRect it)
        {
            if (it.SetHover(true))
            {
                if (scrollY.Show) scrollY.Value = it.Rect.Y - it.Rect.Height;
                Print();
            }
        }

        int oldSub = -1;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (scrollY.MouseMove(e.Location) && OnTouchMove(e.X, e.Y))
            {
                int count = 0, hand = -1;
                int y = scrollY.Show ? (int)scrollY.Value : 0;
                for (int i = 0; i < rectsContent.Length; i++)
                {
                    var it = rectsContent[i];
                    if (it.Tag == null) continue;
                    if (it.Tag.Enabled)
                    {
                        bool hover = it.Rect.Contains(e.X, e.Y + y);
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
                    if (it.Tag != null && it.Tag.Sub != null && it.Tag.Sub.Length > 0)
                    {
                        subForm = new LayeredFormContextMenuStrip(config, this, new Point(TargetRect.X + (it.Rect.X + it.Rect.Width) - 20, TargetRect.Y + it.Rect.Y - 20 - (scrollY.Show ? (int)scrollY.Value : 0)), it.Tag.Sub);
                        subForm.Show(this);
                    }
                }
                else
                {
                    oldSub = -1;
                    subForm?.IClose();
                    subForm = null;
                    SetCursor(false);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollY(int value) => scrollY.MouseWheel(value);

        ManualResetEvent? resetEvent;

        LayeredFormContextMenuStrip? subForm = null;
        ILayeredForm? SubLayeredForm.SubForm() => subForm;

        #endregion

        class InRect
        {
            public InRect(ContextMenuStripItem tag, int _y, int _h)
            {
                Tag = tag;
                y = _y;
                h = _h;
            }
            public InRect(int _y, int _h)
            {
                y = _y;
                h = _h;
            }
            public ContextMenuStripItem? Tag { get; set; }
            public bool Hover { get; set; }
            public bool Down { get; set; }
            public Rectangle RectT { get; set; }
            public Rectangle RectIcon { get; set; }
            public Rectangle RectCheck { get; set; }
            public Rectangle RectSub { get; set; }
            public Rectangle Rect { get; set; }
            public int y { get; set; }
            public int h { get; set; }

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