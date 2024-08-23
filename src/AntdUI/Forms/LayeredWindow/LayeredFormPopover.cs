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
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormPopover : ILayeredFormOpacity
    {
        Popover.Config config;
        public override bool MessageEnable => true;
        public override bool MessageCloseSub => true;
        Form? form = null;
        public LayeredFormPopover(Popover.Config _config)
        {
            maxalpha = 255;
            config = _config;
            config.Control.SetTopMost(Handle);
            Font = config.Font ?? config.Control.Font;

            Helper.GDI(g =>
            {
                var dpi = Config.Dpi;

                int sp = (int)Math.Round(8F * dpi), padding = (int)Math.Round(16 * dpi), padding2 = padding * 2;
                Padding = new Padding(padding);

                if (config.Content is Control control)
                {
                    control.BackColor = Style.Db.BgElevated;
                    control.ForeColor = Style.Db.Text;
                    int w = (int)Math.Round(control.Width * dpi) + 2;
                    control.Width = w;

                    int h;
                    if (_config.Title == null)
                    {
                        h = control.Height;
                        rectContent = new RectangleF(padding, padding, w, control.Height);
                    }
                    else
                    {
                        using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                        {
                            var sizeTitle = g.MeasureString(config.Title, fontTitle, w);
                            h = (int)Math.Round(sizeTitle.Height + sp + control.Height);
                            rectTitle = new RectangleF(padding, padding, w, sizeTitle.Height + sp);
                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, w, h - sizeTitle.Height - sp);
                        }
                    }
                    tempContent = new Bitmap(control.Width, control.Height);
                    if (Config.Dpi != 1F) Helper.DpiAuto(Config.Dpi, control);
                    control.Size = new Size(tempContent.Width, tempContent.Height);
                    control.DrawToBitmap(tempContent, new Rectangle(0, 0, tempContent.Width, tempContent.Height));
                    SetSize(w + padding2, h + padding2);
                }
                else if (config.Content is IList<Popover.TextRow> list)
                {
                    rtext = true;

                    if (_config.Title == null)
                    {
                        var _texts = new List<float[]>(list.Count);
                        float has_x = 0, max_h = 0;
                        foreach (var txt in list)
                        {
                            if (txt.Call != null) hasmouse = true;
                            var sizeContent = g.MeasureString(txt.Text, txt.Font ?? Font);
                            float txt_w = sizeContent.Width + txt.Gap * dpi;
                            _texts.Add(new float[] { padding + has_x, padding, txt_w });
                            if (max_h < sizeContent.Height) max_h = sizeContent.Height;
                            has_x += txt_w;
                        }
                        var texts = new List<InRect>(_texts.Count);
                        for (int i = 0; i < _texts.Count; i++)
                        {
                            var txt = _texts[i];
                            texts.Add(new InRect(list[i], new RectangleF(txt[0], txt[1], txt[2], max_h)));
                        }
                        rectsContent = texts.ToArray();
                        rectContent = new RectangleF(padding, padding, has_x, max_h);
                        SetSize((int)has_x + padding2, (int)max_h + padding2);
                    }
                    else
                    {
                        using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                        {
                            var sizeTitle = g.MeasureString(config.Title, fontTitle);

                            var _texts = new List<float[]>(list.Count);
                            float has_x = 0, max_h = 0;
                            foreach (var txt in list)
                            {
                                if (txt.Call != null) hasmouse = true;
                                var sizeContent = g.MeasureString(txt.Text, txt.Font ?? Font);
                                float txt_w = sizeContent.Width + txt.Gap * dpi;
                                _texts.Add(new float[] { padding + has_x, padding + sizeTitle.Height + sp, txt_w });
                                if (max_h < sizeContent.Height) max_h = sizeContent.Height;
                                has_x += txt_w;
                            }
                            var texts = new List<InRect>(_texts.Count);
                            for (int i = 0; i < _texts.Count; i++)
                            {
                                var txt = _texts[i];
                                texts.Add(new InRect(list[i], new RectangleF(txt[0], txt[1], txt[2], max_h)));
                            }
                            rectsContent = texts.ToArray();

                            int w = (int)Math.Ceiling(has_x > sizeTitle.Width ? has_x : sizeTitle.Width), h = (int)Math.Round(sizeTitle.Height + sp + max_h);

                            rectTitle = new RectangleF(padding, padding, w, sizeTitle.Height + sp);
                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, w, max_h);

                            SetSize(w + padding2, h + padding2);
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
                        int w = (int)Math.Ceiling(sizeContent.Width), h = (int)Math.Round(sizeContent.Height);
                        rectContent = new RectangleF(padding, padding, w, h);
                        SetSize(w + padding2, h + padding2);
                    }
                    else
                    {
                        using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                        {
                            SizeF sizeTitle = g.MeasureString(config.Title, fontTitle), sizeContent = g.MeasureString(content, Font);
                            int w = (int)Math.Ceiling(sizeContent.Width > sizeTitle.Width ? sizeContent.Width : sizeTitle.Width), h = (int)Math.Round(sizeTitle.Height + sp + sizeContent.Height);

                            rectTitle = new RectangleF(padding, padding, w, sizeTitle.Height + sp);
                            rectContent = new RectangleF(rectTitle.X, rectTitle.Bottom, w, h - sizeTitle.Height - sp);

                            SetSize(w + padding2, h + padding2);
                        }
                    }
                }
            });

            var point = config.Control.PointToScreen(Point.Empty);
            if (config.Offset is RectangleF rectf) SetLocation(config.ArrowAlign.AlignPoint(new Rectangle(point.X + (int)rectf.X, point.Y + (int)rectf.Y, (int)rectf.Width, (int)rectf.Height), TargetRect.Width, TargetRect.Height));
            else if (config.Offset is Rectangle rect) SetLocation(config.ArrowAlign.AlignPoint(new Rectangle(point.X + rect.X, point.Y + rect.Y, rect.Width, rect.Height), TargetRect.Width, TargetRect.Height));
            else SetLocation(config.ArrowAlign.AlignPoint(point, config.Control.Size, TargetRect.Width, TargetRect.Height));
        }

        public override void LoadOK()
        {
            if (IsHandleCreated)
            {
                if (config.Content is Control control)
                {
                    BeginInvoke(new Action(() =>
                    {
                        LoadContent(control);
                    }));
                }
            }
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
        void LoadContent(Control control)
        {
            var flocation = new Point(TargetRect.Location.X + (int)rectContent.X, TargetRect.Location.Y + (int)rectContent.Y);
            var fsize = new Size((int)rectContent.Width, (int)rectContent.Height);
            form = new DoubleBufferForm(control)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = flocation,
                MaximumSize = fsize,
                MinimumSize = fsize,
                Size = fsize
            };
            control.Disposed += (a, b) =>
            {
                Close();
            };
            form.Show(this);
            form.Location = flocation;
            PARENT = form;
            config.OnControlLoad?.Invoke();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (config.Content is Control control)
            {
                if (control.IsDisposed)
                {
                    form?.Dispose();
                    base.OnClosing(e);
                    return;
                }
                tempContent = new Bitmap(control.Width, control.Height);
                control.DrawToBitmap(tempContent, new Rectangle(0, 0, tempContent.Width, tempContent.Height));
                form?.Hide();
            }
            base.OnClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (config.Content is Control control)
            {
                control.Dispose();
                form?.Dispose();
            }
            base.Dispose(disposing);
        }

        RectangleF rectTitle, rectContent;
        InRect[]? rectsContent;
        bool rtext = false;
        bool hasmouse = false;

        #region 渲染

        readonly StringFormat stringLeft = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        readonly StringFormat stringCenter = Helper.SF_NoWrap();

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path = DrawShadow(g, rect, rect_read))
                {
                    using (var brush = new SolidBrush(Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path);
                        if (config.ArrowAlign != TAlign.None) g.FillPolygon(brush, config.ArrowAlign.AlignLines(config.ArrowSize, rect, rect_read));
                    }
                    if (tempContent != null) g.DrawImage(tempContent, rectContent);
                }

                if (config.Title != null || rtext)
                {
                    using (var brush = new SolidBrush(Style.Db.Text))
                    {
                        using (var fontTitle = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                        {
                            g.DrawStr(config.Title, fontTitle, brush, rectTitle, stringLeft);
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
                                            g.DrawStr(txt.Text, txt.Font ?? Font, fore, rectsContent[i].Rect, stringCenter);
                                        }
                                    }
                                    else g.DrawStr(txt.Text, txt.Font ?? Font, brush, rectsContent[i].Rect, stringCenter);
                                }
                            }
                            else g.DrawStr(config.Content.ToString(), Font, brush, rectContent, stringLeft);
                        }
                    }
                }
            }
            return original_bmp;
        }

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="rect_read">真实区域</param>
        GraphicsPath DrawShadow(Graphics g, Rectangle rect_client, RectangleF rect_read)
        {
            var path = rect_read.RoundPath((int)(config.Radius * Config.Dpi));
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (hasmouse && rectsContent != null)
            {
                foreach (var it in rectsContent)
                {
                    if (it.Text.Call != null && it.Rect.Contains(e.Location))
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
                    if (it.Text.Call != null && it.Rect.Contains(e.Location))
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
            public InRect(Popover.TextRow text, RectangleF rect)
            {
                Text = text;
                Rect = rect;
            }
            public Popover.TextRow Text { get; set; }
            public RectangleF Rect { get; set; }
        }
    }
}