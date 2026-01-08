// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormPopover : ILayeredShadowFormOpacity
    {
        Popover.Config config;
        internal bool topMost = false;
        Form? form;

        int ArrowSize = 8;
        public LayeredFormPopover(Popover.Config _config)
        {
            config = _config;
            CloseMode = CloseMode.Click;
            topMost = config.Control.SetTopMost(Handle);
            Font = config.Font ?? config.Control.Font;

            Helper.GDI(g =>
            {
                Radius = (int)(config.Radius * Dpi);
                ArrowSize = (int)(config.ArrowSize * Dpi);

                int sp = (int)Math.Round(config.Gap * Dpi), paddingx = (int)(config.Padding.Width * Dpi),
                paddingy = (int)(config.Padding.Height * Dpi), paddingx2 = paddingx * 2, paddingy2 = paddingy * 2;
                Padding = new Padding(paddingx, paddingy, paddingx, paddingy);
                if (config.Content is Control control)
                {
                    control.Parent = this;
                    control.BackColor = config.Back ?? Colour.BgElevated.Get(name, config.ColorScheme);
                    control.ForeColor = config.Fore ?? Colour.Text.Get(name, config.ColorScheme);
                    Win32.WindowTheme(control, config.ColorScheme);
                    Helper.DpiAuto(config.Dpi ?? Dpi, control);
                    int w = control.Width;
                    int h;
                    if (_config.Title == null)
                    {
                        h = control.Height;
                        rectContent = new Rectangle(paddingx, paddingy, w, control.Height);
                    }
                    else
                    {
                        using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                        {
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, w);
                            h = sizeTitle.Height + sp + control.Height;
                            rectTitle = new Rectangle(paddingx, paddingy, w, sizeTitle.Height + sp);
                            rectContent = new Rectangle(rectTitle.X, rectTitle.Bottom, w, h - sizeTitle.Height - sp);
                        }
                    }
                    rectContent.Offset(shadow, shadow);
                    control.Size = new Size(control.Width, control.Height);
                    SetSize(w + paddingx2, h + paddingy2);
                    BeginInvoke(() =>
                    {
                        tempContent = new Bitmap(control.Width, control.Height);
                        control.DrawToBitmap(tempContent, new Rectangle(0, 0, tempContent.Width, tempContent.Height));
                    });
                }
                else if (config.Content is IList<Popover.TextRow> list)
                {
                    rtext = true;

                    if (_config.Title == null)
                    {
                        var _texts = new List<int[]>(list.Count);
                        int has_x = 0, max_h = 0;
                        foreach (var txt in list)
                        {
                            if (txt.Call != null) hasmouse = true;
                            var sizeContent = g.MeasureString(txt.Text, txt.Font ?? Font);
                            int txt_w = sizeContent.Width + (int)(txt.Gap * Dpi);
                            _texts.Add(new int[] { paddingx + has_x, paddingy, txt_w });
                            if (max_h < sizeContent.Height) max_h = sizeContent.Height;
                            has_x += txt_w;
                        }
                        var texts = new List<InRect>(_texts.Count);
                        for (int i = 0; i < _texts.Count; i++)
                        {
                            var txt = _texts[i];
                            texts.Add(new InRect(list[i], new Rectangle(txt[0], txt[1], txt[2], max_h)));
                        }
                        rectsContent = texts.ToArray();
                        rectContent = new Rectangle(paddingx, paddingy, has_x, max_h);
                        SetSize(has_x + paddingx2, max_h + paddingy2);
                    }
                    else
                    {
                        using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                        {
                            var sizeTitle = g.MeasureString(config.Title, fontTitle);

                            var _texts = new List<int[]>(list.Count);
                            int has_x = 0, max_h = 0;
                            foreach (var txt in list)
                            {
                                if (txt.Call != null) hasmouse = true;
                                var sizeContent = g.MeasureString(txt.Text, txt.Font ?? Font);
                                int txt_w = sizeContent.Width + (int)(txt.Gap * Dpi);
                                _texts.Add(new int[] { paddingx + has_x, paddingy + sizeTitle.Height + sp, txt_w });
                                if (max_h < sizeContent.Height) max_h = sizeContent.Height;
                                has_x += txt_w;
                            }
                            var texts = new List<InRect>(_texts.Count);
                            for (int i = 0; i < _texts.Count; i++)
                            {
                                var txt = _texts[i];
                                texts.Add(new InRect(list[i], new Rectangle(txt[0], txt[1], txt[2], max_h)));
                            }
                            rectsContent = texts.ToArray();

                            int w = has_x > sizeTitle.Width ? has_x : sizeTitle.Width, h = sizeTitle.Height + sp + max_h;

                            rectTitle = new Rectangle(paddingx, paddingy, w, sizeTitle.Height + sp);
                            rectContent = new Rectangle(rectTitle.X, rectTitle.Bottom, w, max_h);

                            SetSize(w + paddingx2, h + paddingy2);
                        }
                    }
                }
                else
                {
                    rtext = true;
                    var content = config.Content.ToString();

                    if (_config.Title == null)
                    {
                        var sizeContent = g.MeasureString(content, Font);
                        int w = sizeContent.Width, h = sizeContent.Height;
                        rectContent = new Rectangle(paddingx, paddingy, w, h);
                        SetSize(w + paddingx2, h + paddingy2);
                    }
                    else
                    {
                        using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                        {
                            Size sizeTitle = g.MeasureString(config.Title, fontTitle), sizeContent = g.MeasureString(content, Font);
                            int w = sizeContent.Width > sizeTitle.Width ? sizeContent.Width : sizeTitle.Width, h = sizeTitle.Height + sp + sizeContent.Height;

                            rectTitle = new Rectangle(paddingx, paddingy, w, sizeTitle.Height + sp);
                            rectContent = new Rectangle(rectTitle.X, rectTitle.Bottom, w, h - sizeTitle.Height - sp);

                            SetSize(w + paddingx2, h + paddingy2);
                        }
                    }
                }
            });
            if (config.CustomPoint.HasValue)
            {
                new CalculateCoordinate(this, config.CustomPoint.Value, TargetRect, Radius, ArrowSize, shadow, shadow2, config.Offset).Auto(config.ArrowAlign, true, out int x, out int y, out ArrowLine);
                SetLocation(x, y);
            }
            else
            {
                new CalculateCoordinate(this, config.Control, TargetRect, Radius, ArrowSize, shadow, shadow2, config.Offset).Auto(config.ArrowAlign, true, out int x, out int y, out ArrowLine);
                SetLocation(x, y);
            }
        }

        public void LoadOffset(Rectangle rect)
        {
            new CalculateCoordinate(this, config.Control, TargetRect, Radius, ArrowSize, shadow, shadow2, rect).Auto(config.ArrowAlign, true, out int x, out int y, out ArrowLine);
            SetLocation(x, y);
            PrintCache();
            if (form == null) return;
            var flocation = new Point(TargetRect.Location.X + rectContent.X, TargetRect.Location.Y + rectContent.Y);
            var fsize = new Size(rectContent.Width, rectContent.Height);
            form.Location = flocation;
            form.MaximumSize = form.MinimumSize = form.Size = fsize;
        }

        public override string name => nameof(Popover);

        public override void LoadOK()
        {
            if (IsHandleCreated)
            {
                if (config.Content is Control control) BeginInvoke(() => LoadContent(control));
                else base.LoadOK();
            }
            else base.LoadOK();
            if (config.AutoClose > 0)
            {
                ITask.Run(() =>
                {
                    Thread.Sleep(config.AutoClose * 1000);
                    IClose();
                });
            }
        }

        Bitmap? tempContent;
        Form? parent;
        void LoadContent(Control control)
        {
            var flocation = new Point(TargetRect.Location.X + rectContent.X, TargetRect.Location.Y + rectContent.Y);
            var fsize = new Size(rectContent.Width, rectContent.Height);
            form = new DoubleBufferForm(this, control, config.Focus)
            {
                StartPosition = FormStartPosition.Manual,
                FormBorderStyle = FormBorderStyle.None,
                Location = flocation,
                MaximumSize = fsize,
                MinimumSize = fsize,
                Size = fsize
            };
            control.Disposed += Control_Disposed;
            form.Show(this);
            PARENT = form;
            parent = control.FindPARENT();
            config.OnControlLoad?.Invoke();
            control.ControlEvent();
            if (config.Content is ControlEvent controlEvent) controlEvent.LoadCompleted();
            base.LoadOK();
            if (parent != null) parent.VisibleChanged += Parent_VisibleChanged;
        }
        private void Parent_VisibleChanged(object? sender, EventArgs e)
        {
            if (form == null) return;
            form.Visible = parent!.Visible;
        }

        private void Control_Disposed(object? sender, EventArgs e) => IClose();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (config.Content is Control control)
            {
                if (config.OnClosing != null)
                {
                    config.OnClosing(config, e);
                    if (e.Cancel) return;
                }
                if (control.IsDisposed)
                {
                    form?.Dispose();
                    base.OnFormClosing(e);
                    return;
                }
                tempContent = new Bitmap(control.Width, control.Height);
                control.DrawToBitmap(tempContent, new Rectangle(0, 0, tempContent.Width, tempContent.Height));
                if (form != null) form.Location = Helper.OffScreenArea(form.Width * 2, form.Height * 2);
            }
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            tempContent?.Dispose();
            tempContent = null;
            if (parent != null) parent.VisibleChanged -= Parent_VisibleChanged;
            if (config.Content is Control control)
            {
                control.Disposed -= Control_Disposed;
                control.Dispose();
            }
#pragma warning disable CS8625
            config.Content = null;
#pragma warning restore CS8625
            form?.Dispose();
            base.Dispose(disposing);
        }

        Rectangle rectTitle, rectContent;
        InRect[]? rectsContent;
        bool rtext = false;
        bool hasmouse = false;

        #region 渲染

        readonly FormatFlags stringLeft = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.EllipsisCharacter,
            stringCenter = FormatFlags.Center | FormatFlags.NoWrap;

        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            if (config.Title != null || rtext)
            {
                using (var brush = new SolidBrush(config.Fore ?? Colour.Text.Get(name, config.ColorScheme)))
                {
                    using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                    {
                        g.String(config.Title, fontTitle, brush, rectTitle, stringLeft);
                    }
                    if (rtext)
                    {
                        if (config.Content is IList<Popover.TextRow> list && rectsContent != null)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                var txt = list[i];
                                if (txt.Fore.HasValue)
                                {
                                    using (var fore = new SolidBrush(txt.Fore.Value))
                                    {
                                        g.String(txt.Text, txt.Font ?? Font, fore, rectsContent[i].Rect, stringCenter);
                                    }
                                }
                                else g.String(txt.Text, txt.Font ?? Font, brush, rectsContent[i].Rect, stringCenter);
                            }
                        }
                        else g.String(config.Content.ToString(), Font, brush, rectContent, stringLeft);
                    }
                }
            }
        }

        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(config.Back ?? Colour.BgElevated.Get(name, config.ColorScheme)))
            {
                g.Fill(brush, path);
                if (shadow == 0)
                {
                    int bor = (int)(Dpi), bor2 = bor * 2;
                    using (var path2 = new Rectangle(rect.X + bor, rect.Y + bor, rect.Width - bor2, rect.Height - bor2).RoundPath(Radius))
                    {
                        g.Draw(Colour.BorderColor.Get(name, config.ColorScheme), bor, path2);
                    }
                    if (tempContent != null) g.Image(tempContent, rectContent);
                    return;
                }
                if (ArrowLine != null) g.FillPolygon(brush, ArrowLine);
            }
            if (tempContent != null) g.Image(tempContent, rectContent);
        }

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (hasmouse && rectsContent != null)
            {
                foreach (var it in rectsContent)
                {
                    if (it.Text.Call != null && it.Rect.Contains(e.X, e.Y))
                    {
                        SetCursor(true);
                        return;
                    }
                }
                SetCursor(false);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (hasmouse && rectsContent != null && e.Button == MouseButtons.Left)
            {
                foreach (var it in rectsContent)
                {
                    if (it.Text.Call != null && it.Rect.Contains(e.X, e.Y))
                    {
                        it.Text.Call();
                        return;
                    }
                }
            }
            base.OnMouseClick(e);
        }

        #endregion

        class InRect
        {
            public InRect(Popover.TextRow text, Rectangle rect)
            {
                Text = text;
                Rect = rect;
            }
            public Popover.TextRow Text { get; set; }
            public Rectangle Rect { get; set; }
        }
    }
}