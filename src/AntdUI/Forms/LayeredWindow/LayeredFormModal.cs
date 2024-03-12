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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormModal : Window
    {
        Modal.Config config;
        public LayeredFormModal(Modal.Config _config)
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            UpdateStyles();

            Resizable = false;

            config = _config;
            TopMost = config.Form.TopMost;
            close_button = new ITaskOpacity(this);

            #region InitializeComponent

            SuspendLayout();

            BackColor = Style.Db.BgElevated;
            Size = new Size(416, 160);
            Font = config.Font == null ? config.Form.Font : config.Font;
            ForeColor = Style.Db.TextBase;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            btn_ok.AutoSizeMode = TAutoSize.Width;
            btn_ok.Dock = DockStyle.Right;
            btn_ok.Location = new Point(304, 0);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new Size(64, 38);
            btn_ok.TabIndex = 0;
            btn_ok.Type = config.OkType;
            btn_ok.Text = config.OkText;
            btn_ok.Click += btn_ok_Click;

            if (config.CancelText != null)
            {
                btn_no = new Button();
                btn_no.AutoSizeMode = TAutoSize.Width;
                btn_no.BorderWidth = 1F;
                btn_no.Dock = DockStyle.Right;
                btn_no.Location = new Point(240, 0);
                btn_no.Name = "btn_no";
                btn_no.Size = new Size(64, 38);
                btn_no.TabIndex = 1;
                btn_no.Text = config.CancelText;
                btn_no.Click += btn_no_Click;
            }

            var panel1 = new Panel
            {
                Dock = DockStyle.Bottom,
                Back = Style.Db.BgElevated,
                Size = new Size(368, 38)
            };
            if (btn_no != null) panel1.Controls.Add(btn_no);
            panel1.Controls.Add(btn_ok);
            if (config.Btns != null)
            {
                var btns = new List<Button>(config.Btns.Length);
                foreach (var btn in config.Btns)
                {
                    var _btn = new Button
                    {
                        AutoSizeMode = TAutoSize.Width,
                        Dock = DockStyle.Right,
                        Size = new Size(64, 38),
                        Name = btn.Name,
                        Text = btn.Text,
                        Type = btn.Type,
                        Back = btn.Back,
                        Fore = btn.Fore,
                        Tag = btn.Tag
                    };
                    panel1.Controls.Add(_btn);
                    btns.Insert(0, _btn);
                }
                foreach (var btn in btns)
                {
                    btn.BringToFront();
                    btn.Click += (a, b) =>
                    {
                        config.OnBtns?.Invoke(btn);
                        Close();
                    };
                }
            }
            Controls.Add(panel1);
            Controls.Add(new System.Windows.Forms.Panel
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(386, 100),
                Size = new Size(60, 90)
            });

            if (config.Keyboard)
            {
                if (btn_no == null) AcceptButton = CancelButton = btn_ok;
                else
                {
                    AcceptButton = btn_ok;
                    CancelButton = btn_no;
                }
            }

            #endregion

            Helper.GDI(g =>
            {
                var dpi = Config.Dpi;

                int butt_h = (int)Math.Round(38 * dpi), gap = (int)Math.Round(8F * dpi), paddingx = (int)Math.Round(24 * dpi), paddingy = (int)Math.Round(20 * dpi),
                    w = (int)Math.Round(config.Width * dpi), wp = w - paddingx * 2;
                Padding = new Padding(paddingx, paddingy, paddingx, paddingy);

                panel1.Height = butt_h;

                using (var fontTitle = new Font(Font.FontFamily, Font.Size * 1.14F, FontStyle.Bold))
                {
                    if (config.Content is Control control)
                    {
                        rectsContent = new RectangleF[0];
                        w = (int)Math.Round(control.Width * dpi);
                        if (dpi != 1F)
                        {
                            var dir = Helper.DpiSuspend(control.Controls);
                            Helper.DpiLS(dpi, control);
                            Helper.DpiResume(dir, control.Controls);
                        }
                        wp = w - paddingx * 2;
                        control.Width = w;
                        Controls.Add(control);
                        if (_config.Icon == TType.None)
                        {
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, wp);

                            int h = (int)Math.Round(sizeTitle.Height + gap + control.Height + butt_h);

                            rectTitle = new RectangleF(paddingx, paddingy, wp, sizeTitle.Height + gap);
                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, wp, h - butt_h - sizeTitle.Height - gap);
                            MinimumSize = MaximumSize = Size = new Size(w, h + paddingy * 2);
                        }
                        else
                        {
                            int icon_size = (int)Math.Round(22 * dpi), icon_size_x = (int)Math.Round(12 * dpi);
                            wp -= icon_size + icon_size_x;
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, wp);
                            int h = (int)Math.Round(sizeTitle.Height + gap + control.Height + butt_h);

                            rectTitle = new RectangleF(paddingx + icon_size + icon_size_x, paddingy, wp, sizeTitle.Height + gap);
                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, wp, h - butt_h - sizeTitle.Height - gap);
                            control.Location = new Point((int)rectContent.X, (int)rectContent.Y);
                            rectIcon = new RectangleF(paddingx, rectTitle.Y + (rectTitle.Height - icon_size) / 2F, icon_size, icon_size);

                            MinimumSize = MaximumSize = Size = new Size(w, h + paddingy * 2);
                        }
                        control.Location = new Point((int)rectContent.X, (int)rectContent.Y);
                        control.Size = new Size((int)rectContent.Width, (int)rectContent.Height);
                    }
                    else if (config.Content is IList<Modal.TextLine> list)
                    {
                        rtext = true;
                        var texts = new List<RectangleF>(list.Count);
                        if (_config.Icon == TType.None)
                        {
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, wp);
                            rectTitle = new RectangleF(paddingx, paddingy, wp, sizeTitle.Height + gap);

                            float has_y = paddingy + sizeTitle.Height + gap;
                            float h_temp = 0;
                            foreach (var txt in list)
                            {
                                var sizeContent = g.MeasureString(txt.Text, txt.Font == null ? Font : txt.Font, wp);
                                float txt_h = sizeContent.Height + txt.Gap * dpi;
                                texts.Add(new RectangleF(rectTitle.X, has_y, wp, txt_h));
                                has_y += txt_h;
                                h_temp += txt_h;
                            }

                            int h = (int)Math.Round(sizeTitle.Height + gap + h_temp + butt_h);

                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, wp, h - butt_h - sizeTitle.Height - gap);

                            MinimumSize = MaximumSize = Size = new Size(w, h + paddingy * 2);
                        }
                        else
                        {
                            int icon_size = (int)Math.Round(22 * dpi), icon_size_x = (int)Math.Round(12 * dpi);
                            wp -= icon_size + icon_size_x;
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, wp);
                            rectTitle = new RectangleF(paddingx + icon_size + icon_size_x, paddingy, wp, sizeTitle.Height + gap);
                            rectIcon = new RectangleF(paddingx, rectTitle.Y + (rectTitle.Height - icon_size) / 2F, icon_size, icon_size);

                            float has_y = paddingy + sizeTitle.Height + gap;
                            float h_temp = 0;
                            foreach (var txt in list)
                            {
                                var sizeContent = g.MeasureString(txt.Text, txt.Font == null ? Font : txt.Font, wp);
                                float txt_h = sizeContent.Height + txt.Gap * dpi;
                                texts.Add(new RectangleF(rectTitle.X, has_y, wp, txt_h));
                                has_y += txt_h;
                                h_temp += txt_h;
                            }

                            int h = (int)Math.Round(sizeTitle.Height + gap + h_temp + butt_h);

                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, wp, h - butt_h - sizeTitle.Height - gap);

                            MinimumSize = MaximumSize = Size = new Size(w, h + paddingy * 2);
                        }
                        rectsContent = texts.ToArray();
                        if (config.CloseIcon)
                        {
                            float close_size = 22F * dpi;
                            rect_close = new RectangleF(rectTitle.Right - close_size, rectTitle.Y, close_size, close_size);
                        }
                    }
                    else
                    {
                        rectsContent = new RectangleF[0];
                        rtext = true;
                        var content = config.Content.ToString();
                        if (_config.Icon == TType.None)
                        {
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, wp);
                            var sizeContent = g.MeasureString(content, Font, wp);
                            int h = (int)Math.Round(sizeTitle.Height + gap + sizeContent.Height + butt_h);

                            rectTitle = new RectangleF(paddingx, paddingy, wp, sizeTitle.Height + gap);
                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, wp, h - butt_h - sizeTitle.Height - gap);

                            MinimumSize = MaximumSize = Size = new Size(w, h + paddingy * 2);
                        }
                        else
                        {
                            int icon_size = (int)Math.Round(22 * dpi), icon_size_x = (int)Math.Round(12 * dpi);
                            wp -= icon_size + icon_size_x;
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, wp);
                            var sizeContent = g.MeasureString(content, Font, wp);
                            int h = (int)Math.Round(sizeTitle.Height + gap + sizeContent.Height + butt_h);

                            rectTitle = new RectangleF(paddingx + icon_size + icon_size_x, paddingy, wp, sizeTitle.Height + gap);
                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, wp, h - butt_h - sizeTitle.Height - gap);

                            rectIcon = new RectangleF(paddingx, rectTitle.Y + (rectTitle.Height - icon_size) / 2F, icon_size, icon_size);

                            MinimumSize = MaximumSize = Size = new Size(w, h + paddingy * 2);
                        }
                        if (config.CloseIcon)
                        {
                            float close_size = 22F * dpi;
                            rect_close = new RectangleF(rectTitle.Right - close_size, rectTitle.Y, close_size, close_size);
                        }
                    }
                }
            });

            ResumeLayout();
            panel1.MouseMove += Window_MouseDown;
        }

        bool min = true;

        public override bool AutoHandDpi { get; set; } = false;

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            close_button.Dispose();
            if (config.Content is Control control) control.Dispose();
        }

        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            if (config.MaskClosable && isclose && min && m.Msg == 0x86 && m.WParam == (IntPtr)1 && m.LParam == (IntPtr)0)
            {
                DialogResult = DialogResult.No;
                return;
            }
            base.DefWndProc(ref m);
        }

        void Window_MouseDown(object? sender, MouseEventArgs e)
        {
            DraggableMouseDown();
        }

        RectangleF rectIcon, rectTitle, rectContent;
        RectangleF[] rectsContent;
        bool rtext = false;

        readonly StringFormat stringLeft = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        readonly StringFormat stringTL = Helper.SF_Ellipsis(StringAlignment.Near, StringAlignment.Near);
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            if (config.Icon != TType.None)
            {
                switch (config.Icon)
                {
                    case TType.Success:
                        using (var brush = new SolidBrush(Style.Db.Success))
                        {
                            g.FillEllipse(brush, rectIcon);
                        }
                        g.PaintIconComplete(rectIcon, Style.Db.BgBase);
                        break;
                    case TType.Info:
                        using (var brush = new SolidBrush(Style.Db.Info))
                        {
                            g.FillEllipse(brush, rectIcon);
                        }
                        g.PaintIconInfo(rectIcon, Style.Db.BgBase);
                        break;
                    case TType.Warn:
                        using (var brush = new SolidBrush(Style.Db.Warning))
                        {
                            g.FillEllipse(brush, rectIcon);
                        }
                        g.PaintIconWarn(rectIcon, Style.Db.BgBase);
                        break;
                    case TType.Error:
                        using (var brush = new SolidBrush(Style.Db.Error))
                        {
                            g.FillEllipse(brush, rectIcon);
                        }
                        g.PaintIconError(rectIcon, Style.Db.BgBase);
                        break;
                }
            }
            if (config.CloseIcon)
            {
                if (close_button.Animation)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(close_button.Value, Style.Db.FillSecondary)))
                    {
                        using (var path = rect_close.RoundPath((int)(4 * Config.Dpi)))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    g.PaintIconError(rect_close, Style.Db.Text, 0.7F);
                }
                else if (close_button.Switch)
                {
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        using (var path = rect_close.RoundPath((int)(4 * Config.Dpi)))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    g.PaintIconError(rect_close, Style.Db.Text, 0.7F);
                }
                else
                {
                    g.PaintIconError(rect_close, Style.Db.TextTertiary, 0.7F);
                }
            }
            using (var brush = new SolidBrush(Style.Db.Text))
            {
                using (var fontTitle = new Font(Font.FontFamily, Font.Size * 1.14F, FontStyle.Bold))
                {
                    g.DrawString(config.Title, fontTitle, brush, rectTitle, stringLeft);
                }
                if (rtext)
                {
                    if (config.Content is IList<Modal.TextLine> list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            var txt = list[i];
                            if (txt.Fore.HasValue)
                            {
                                using (var fore = new SolidBrush(txt.Fore.Value))
                                {
                                    g.DrawString(txt.Text, txt.Font == null ? Font : txt.Font, fore, rectsContent[i], stringLeft);
                                }
                            }
                            else g.DrawString(txt.Text, txt.Font == null ? Font : txt.Font, brush, rectsContent[i], stringLeft);
                        }
                    }
                    else g.DrawString(config.Content.ToString(), Font, brush, rectContent, stringTL);
                }
            }
            base.OnPaint(e);
        }

        #region 鼠标

        ITaskOpacity close_button;
        RectangleF rect_close;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (config.CloseIcon)
            {
                close_button.MaxValue = Style.Db.FillSecondary.A;
                close_button.Switch = rect_close.Contains(e.Location);
                SetCursor(close_button.Switch);
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            min = false;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            min = true;
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && config.CloseIcon && rect_close.Contains(e.Location))
            {
                base.OnMouseUp(e);
                return;
            }
            DraggableMouseDown();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && config.CloseIcon && rect_close.Contains(e.Location))
            {
                DialogResult = DialogResult.No;
                return;
            }
            base.OnMouseUp(e);
        }

        private void btn_no_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
        bool isclose = true;
        private void btn_ok_Click(object? sender, EventArgs e)
        {
            if (config.OnOk == null) DialogResult = DialogResult.OK;
            else
            {
                isclose = false;
                btn_ok.Loading = true;
                ITask.Run(() =>
                {
                    bool result = false;
                    try
                    {
                        result = config.OnOk(config);
                    }
                    catch { }
                    isclose = true;
                    btn_ok.Loading = false;
                    if (result)
                    {
                        System.Threading.Thread.Sleep(100);
                        BeginInvoke(new Action(() =>
                        {
                            DialogResult = DialogResult.OK;
                        }));
                    }
                });
            }
        }

        Button btn_ok = new Button();
        Button? btn_no;

        #endregion
    }
}