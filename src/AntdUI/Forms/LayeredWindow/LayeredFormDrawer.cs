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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormDrawer : ILayeredForm
    {
        int FrmRadius = 0, FrmBor = 0;
        bool HasBor = false;
        Drawer.Config config;
        int padding = 24;
        ILayeredForm? formMask = null;
        public bool isclose = false;
        public LayeredFormDrawer(Drawer.Config _config, ILayeredForm mask) : this(_config)
        {
            formMask = mask;
            if (config.MaskClosable)
            {
                mask.Click += (s1, e1) =>
                {
                    isclose = true;
                    IClose();
                };
            }
        }
        public LayeredFormDrawer(Drawer.Config _config)
        {
            config = _config;
            TopMost = config.Form.TopMost;
            Font = config.Form.Font;

            padding = (int)Math.Round(config.Padding * Config.Dpi);
            Padding = new Padding(padding);
            var version = OS.Version;
            if (version.Major >= 10 && version.Build > 22000) FrmRadius = 8; //Win11
            if (config.Form is Window)
            {
                //无边框处理
            }
            else if (config.Form.FormBorderStyle != FormBorderStyle.None && config.Form.WindowState != FormWindowState.Maximized)
            {
                HasBor = true;
                FrmBor = 8;
            }
            config.Content.BackColor = Style.Db.BgElevated;
            config.Content.ForeColor = Style.Db.Text;
            SetPoint();
            SetSize(start_W, start_H);
            SetLocation(start_X, start_Y);
            if (vertical) tempContent = new Bitmap(end_W - padding * 2, end_H - 20 - padding * 2);
            else tempContent = new Bitmap(end_W - 20 - padding * 2, end_H - padding * 2);
            if (config.Content.Tag is Size) { }
            else
            {
                config.Content.Tag = config.Content.Size;
                Helper.DpiAuto(Config.Dpi, config.Content);
            }
            config.Content.Size = new Size(tempContent.Width, tempContent.Height);
            LoadContent();
            config.Content.DrawToBitmap(tempContent, new Rectangle(0, 0, tempContent.Width, tempContent.Height));
            config.Form.LocationChanged += Form_LocationChanged;
            config.Form.SizeChanged += Form_SizeChanged;
        }

        bool vertical = false;
        void SetPoint()
        {
            switch (config.Align)
            {
                case TAlignMini.Top:
                    vertical = true;
                    start_H = 0;
                    end_H = (int)(config.Content.Height * Config.Dpi) + padding * 2 + 20;
                    if (config.Form is Window windowT)
                    {
                        start_W = end_W = windowT.Width;
                        start_X = end_X = windowT.Left;
                        start_Y = end_Y = windowT.Top;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Bottom:
                    vertical = true;
                    start_H = 0;
                    end_H = (int)(config.Content.Height * Config.Dpi) + padding * 2 + 20;
                    if (config.Form is Window windowB)
                    {
                        start_W = end_W = windowB.Width;
                        start_X = end_X = windowB.Left;
                        start_Y = windowB.Top + windowB.Height;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = config.Form.Top + config.Form.Height;
                    }
                    end_Y = start_Y - end_H;
                    break;
                case TAlignMini.Left:
                    start_W = 0;
                    end_W = (int)(config.Content.Width * Config.Dpi) + padding * 2 + 20;
                    if (config.Form is Window windowL)
                    {
                        start_H = end_H = windowL.Height;
                        start_X = end_X = windowL.Left;
                        start_Y = end_Y = windowL.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Right:
                default:
                    start_W = 0;
                    end_W = (int)(config.Content.Width * Config.Dpi) + padding * 2 + 20;
                    if (config.Form is Window windowR)
                    {
                        start_H = end_H = windowR.Height;
                        start_X = windowR.Left + windowR.Width;
                        start_Y = end_Y = windowR.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = config.Form.Left + config.Form.Width;
                        start_Y = end_Y = config.Form.Top;
                    }
                    end_X = start_X - end_W;
                    break;
            }
        }

        Bitmap? tempContent;
        private void Form_SizeChanged(object? sender, EventArgs e)
        {
            switch (config.Align)
            {
                case TAlignMini.Top:
                    if (config.Form is Window windowT)
                    {
                        start_W = end_W = windowT.Width;
                        start_X = end_X = windowT.Left;
                        start_Y = end_Y = windowT.Top;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Bottom:
                    if (config.Form is Window windowB)
                    {
                        start_W = end_W = windowB.Width;
                        start_X = end_X = windowB.Left;
                        start_Y = windowB.Top + windowB.Height;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = config.Form.Top + config.Form.Height;
                    }
                    end_Y = start_Y - end_H;
                    break;
                case TAlignMini.Left:
                    if (config.Form is Window windowL)
                    {
                        start_H = end_H = windowL.Height;
                        start_X = end_X = windowL.Left;
                        start_Y = end_Y = windowL.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Right:
                default:
                    if (config.Form is Window windowR)
                    {
                        start_H = end_H = windowR.Height;
                        start_X = windowR.Left + windowR.Width;
                        start_Y = end_Y = windowR.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = config.Form.Left + config.Form.Width;
                        start_Y = end_Y = config.Form.Top;
                    }
                    end_X = start_X - end_W;
                    break;
            }
            if (task_start == null)
            {
                SetLocation(end_X, end_Y);
                SetSize(end_W, end_H);
                if (form != null)
                {
                    isok = false;
                    var rect = Ang();
                    form.Location = rect.Location;
                    form.Size = rect.Size;
                    isok = true;
                }
                Print();
            }
        }

        private void Form_LocationChanged(object? sender, EventArgs e)
        {
            switch (config.Align)
            {
                case TAlignMini.Top:
                    if (config.Form is Window windowT)
                    {
                        start_W = end_W = windowT.Width;
                        start_X = end_X = windowT.Left;
                        start_Y = end_Y = windowT.Top;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Bottom:
                    if (config.Form is Window windowB)
                    {
                        start_W = end_W = windowB.Width;
                        start_X = end_X = windowB.Left;
                        start_Y = windowB.Top + windowB.Height;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = config.Form.Top + config.Form.Height;
                    }
                    end_Y = start_Y - end_H;
                    break;
                case TAlignMini.Left:
                    if (config.Form is Window windowL)
                    {
                        start_H = end_H = windowL.Height;
                        start_X = end_X = windowL.Left;
                        start_Y = end_Y = windowL.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Right:
                default:
                    if (config.Form is Window windowR)
                    {
                        start_H = end_H = windowR.Height;
                        start_X = windowR.Left + windowR.Width;
                        start_Y = end_Y = windowR.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = config.Form.Left + config.Form.Width;
                        start_Y = end_Y = config.Form.Top;
                    }
                    end_X = start_X - end_W;
                    break;
            }
            if (task_start == null)
            {
                SetLocation(end_X, end_Y);
                if (form != null) form.Location = Ang().Location;
                Print();
            }
        }

        #region 动画

        int start_X = 0, end_X = 0, start_Y = 0, end_Y = 0;
        int start_W = 0, end_W = 0, start_H = 0, end_H = 0;
        ITask? task_start = null;
        bool run_end = false, ok_end = false;
        protected override void OnLoad(EventArgs e)
        {
            if (Config.Animation)
            {
                var t = Animation.TotalFrames(10, 100);
                int sleep = config.Mask ? 200 : 0;
                task_start = new ITask(vertical ? i =>
                {
                    var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                    SetAnimateValueY(start_Y + (int)((end_Y - start_Y) * val), (int)(end_H * val), (byte)(255 * val));
                    return true;
                }
                : i =>
                {
                    var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                    SetAnimateValueX(start_X + (int)((end_X - start_X) * val), (int)(end_W * val), (byte)(255 * val));
                    return true;
                }, 10, t, () =>
                {
                    if (IsHandleCreated) BeginInvoke(new Action(ShowContent));
                    SetAnimateValue(end_X, end_Y, end_W, end_H, 255);
                    task_start = null;
                }, sleep);
                base.OnLoad(e);
            }
            else
            {
                SetAnimateValue(end_X, end_Y, end_W, end_H, 255);
                base.OnLoad(e);
                ShowContent();
            }
        }

        #region 控件

        Form? form = null;
        void LoadContent()
        {
            var rect = Ang();
            var hidelocation = new Point(-rect.Width, -rect.Height);
            if (config.Content is Form form_)
            {
                form_.BackColor = Style.Db.BgElevated;
                form_.FormBorderStyle = FormBorderStyle.None;
                form_.Location = hidelocation;
                form_.ClientSize = rect.Size;
                form = form_;
            }
            else
            {
                form = new DoubleBufferForm(this, config.Content)
                {
                    BackColor = Style.Db.BgElevated,
                    FormBorderStyle = FormBorderStyle.None,
                    Location = hidelocation,
                    ClientSize = rect.Size
                };
            }
            if (!config.Dispose && config.Content.Tag is Size size)
            {
                form.FormClosing += (a, b) =>
                {
                    config.Content.Dock = DockStyle.None;
                    config.Content.Size = size;
                    config.Content.Location = new Point(-config.Content.Width, -config.Content.Height);
                    config.Form.Controls.Add(config.Content);
                };
            }
            config.Content.Disposed += (a, b) =>
            {
                config.Content.SizeChanged -= Content_SizeChanged;
                Close();
            };
            form.Show(this);
            form.Location = hidelocation;
            form.ClientSize = rect.Size;
        }

        void ShowContent()
        {
            if (form == null) return;
            var rect = Ang();
            if (form.ClientSize != rect.Size) form.ClientSize = rect.Size;
            form.Location = rect.Location;
            config.OnLoad?.Invoke();
            LoadOK?.Invoke();
            if (config.Content is DrawerLoad idrawer) idrawer.LoadOK();
            config.Content.SizeChanged += Content_SizeChanged;
            tempContent?.Dispose();
            tempContent = null;
        }

        bool isok = true;
        private void Content_SizeChanged(object? sender, EventArgs e)
        {
            if (form == null) return;
            if (isok)
            {
                isok = false;
                var size = config.Content.Size;
                switch (config.Align)
                {
                    case TAlignMini.Top:
                        end_H = (int)(size.Height * Config.Dpi) + padding * 2 + 20;
                        if (config.Form is Window windowT)
                        {
                            start_W = end_W = windowT.Width;
                            start_X = end_X = windowT.Left;
                            start_Y = end_Y = windowT.Top;
                        }
                        else
                        {
                            start_W = end_W = config.Form.Width;
                            start_X = end_X = config.Form.Left;
                            start_Y = end_Y = config.Form.Top;
                        }
                        break;
                    case TAlignMini.Bottom:
                        end_H = (int)(size.Height * Config.Dpi) + padding * 2 + 20;
                        if (config.Form is Window windowB)
                        {
                            start_W = end_W = windowB.Width;
                            start_X = end_X = windowB.Left;
                            start_Y = windowB.Top + windowB.Height;
                        }
                        else
                        {
                            start_W = end_W = config.Form.Width;
                            start_X = end_X = config.Form.Left;
                            start_Y = config.Form.Top + config.Form.Height;
                        }
                        end_Y = start_Y - end_H;
                        break;
                    case TAlignMini.Left:
                        end_W = (int)(size.Width * Config.Dpi) + padding * 2 + 20;
                        if (config.Form is Window windowL)
                        {
                            start_H = end_H = windowL.Height;
                            start_X = end_X = windowL.Left;
                            start_Y = end_Y = windowL.Top;
                        }
                        else
                        {
                            start_H = end_H = config.Form.Height;
                            start_X = end_X = config.Form.Left;
                            start_Y = end_Y = config.Form.Top;
                        }
                        break;
                    case TAlignMini.Right:
                    default:
                        end_W = (int)(size.Width * Config.Dpi) + padding * 2 + 20;
                        if (config.Form is Window windowR)
                        {
                            start_H = end_H = windowR.Height;
                            start_X = windowR.Left + windowR.Width;
                            start_Y = end_Y = windowR.Top;
                        }
                        else
                        {
                            start_H = end_H = config.Form.Height;
                            start_X = config.Form.Left + config.Form.Width;
                            start_Y = end_Y = config.Form.Top;
                        }
                        end_X = start_X - end_W;
                        break;
                }
                SetLocation(end_X, end_Y);
                SetSize(end_W, end_H);
                var rect = Ang();
                form.Location = rect.Location;
                form.Size = rect.Size;
                Print();
                ITask.Run(() =>
                {
                    System.Threading.Thread.Sleep(500);
                    isok = true;
                });
            }
        }

        internal Action? LoadOK = null;

        Rectangle Ang()
        {
            switch (config.Align)
            {
                case TAlignMini.Top: return new Rectangle(end_X + padding, end_Y + padding, end_W - padding * 2, end_H - 20 - padding * 2);
                case TAlignMini.Bottom: return new Rectangle(end_X + padding, end_Y + padding + 20, end_W - padding * 2, end_H - 20 - padding * 2);
                case TAlignMini.Left: return new Rectangle(end_X + padding, end_Y + padding, end_W - 20 - padding * 2, end_H - padding * 2);
                case TAlignMini.Right:
                default: return new Rectangle(end_X + padding + 20, end_Y + padding, end_W - 20 - padding * 2, end_H - padding * 2);
            }
        }

        #endregion

        #region 设置动画参数

        void SetAnimateValueX(int x, int w, byte _alpha)
        {
            if (TargetRect.X != x || TargetRect.Width != w || alpha != _alpha)
            {
                SetLocationX(x);
                SetSizeW(w);
                alpha = _alpha;
                Print();
            }
        }
        void SetAnimateValueY(int y, int h, byte _alpha)
        {
            if (TargetRect.Y != y || TargetRect.Height != h || alpha != _alpha)
            {
                SetLocationY(y);
                SetSizeH(h);
                alpha = _alpha;
                Print();
            }
        }
        void SetAnimateValue(int x, int y, int w, int h, byte _alpha)
        {
            if (TargetRect.X != x || TargetRect.Y != y || TargetRect.Width != w || TargetRect.Height != h || alpha != _alpha)
            {
                SetLocation(x, y);
                SetSize(w, h);
                alpha = _alpha;
                Print();
            }
        }

        #endregion

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            isclose = true;
            formMask?.IClose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            task_start?.Dispose();
            if (!ok_end)
            {
                config.Form.LocationChanged -= Form_LocationChanged;
                config.Form.SizeChanged -= Form_SizeChanged;

                tempContent = new Bitmap(config.Content.Width, config.Content.Height);
                config.Content.DrawToBitmap(tempContent, new Rectangle(0, 0, tempContent.Width, tempContent.Height));
                form?.Hide();
                e.Cancel = true;
                if (Config.Animation)
                {
                    if (!run_end)
                    {
                        run_end = true;
                        var t = Animation.TotalFrames(10, 100);
                        new ITask(vertical ? (i) =>
                        {
                            var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            SetAnimateValueY(end_Y - (int)((end_Y - start_Y) * val), (int)(end_H * (1F - val)), (byte)(255 * (1F - val)));
                            return true;
                        }
                        : (i) =>
                        {
                            var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            SetAnimateValueX(end_X - (int)((end_X - start_X) * val), (int)(end_W * (1F - val)), (byte)(255 * (1F - val)));
                            return true;
                        }, 10, t, () =>
                        {
                            ok_end = true;
                            IClose(true);
                        });
                    }
                }
                else
                {
                    ok_end = true;
                    IClose(true);
                }
            }
            base.OnClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            config.Form.LocationChanged -= Form_LocationChanged;
            config.Form.SizeChanged -= Form_SizeChanged;
            if (config.Dispose) config.Content.Dispose();
            tempContent?.Dispose();
            form?.Dispose();
            task_start?.Dispose();
            config.OnClose?.Invoke();
            config.OnClose = null;
            base.Dispose(disposing);
        }

        public void IRClose()
        {
            ok_end = true;
            IClose(true);
        }

        #endregion

        #region 渲染

        public override Bitmap PrintBit()
        {
            Rectangle rect;
            if (HasBor) rect = new Rectangle(FrmBor, 0, TargetRect.Width - FrmBor * 2, TargetRect.Height - FrmBor);
            else rect = TargetRectXY;
            var original_bmp = new Bitmap(TargetRect.Width, TargetRect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                var rect_read = DrawShadow(g, rect);
                using (var path = rect_read.RoundPath(FrmRadius))
                {
                    g.Fill(Style.Db.BgElevated, path);
                    if (tempContent != null) g.Image(tempContent, new Rectangle(rect_read.X + padding, rect_read.Y + padding, tempContent.Width, tempContent.Height));
                }
            }
            return original_bmp;
        }

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">客户区域</param>
        Rectangle DrawShadow(Canvas g, Rectangle rect)
        {
            var matrix = new ColorMatrix { Matrix33 = 0.3F };
            switch (config.Align)
            {
                case TAlignMini.Top:
                    if (Config.ShadowEnabled)
                    {
                        if (shadow_temp == null || shadow_temp.Width != end_W)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X, rect.Y + 20, end_W, 40).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(end_W, 80, 20);
                            }
                        }
                        using (var attributes = new ImageAttributes())
                        {
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            g.Image(shadow_temp, new Rectangle(rect.Y, rect.Bottom - 80, rect.Width, 80), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                        }
                    }
                    return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - 20);
                case TAlignMini.Bottom:
                    if (Config.ShadowEnabled)
                    {
                        if (shadow_temp == null || shadow_temp.Width != end_W)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X, rect.Y + 20, end_W, 40).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(end_W, 80, 20);
                            }
                        }
                        using (var attributes = new ImageAttributes())
                        {
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            g.Image(shadow_temp, new Rectangle(rect.Y, rect.Y, rect.Width, 80), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                        }
                    }
                    return new Rectangle(rect.X, rect.Y + 20, rect.Width, rect.Height - 20);
                case TAlignMini.Left:
                    if (Config.ShadowEnabled)
                    {
                        if (shadow_temp == null || shadow_temp.Height != end_H)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X + 20, rect.Y, 40, end_H).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(80, end_H, 20);
                            }
                        }
                        using (var attributes = new ImageAttributes())
                        {
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            g.Image(shadow_temp, new Rectangle(rect.Right - 80, rect.Y, 80, rect.Height), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                        }
                    }
                    return new Rectangle(rect.X, rect.Y, rect.Width - 20, rect.Height);
                case TAlignMini.Right:
                default:
                    if (Config.ShadowEnabled)
                    {
                        if (shadow_temp == null || shadow_temp.Height != end_H)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X + 20, rect.Y, 40, end_H).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(80, end_H, 20);
                            }
                        }
                        using (var attributes = new ImageAttributes())
                        {
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            g.Image(shadow_temp, new Rectangle(rect.X, rect.Y, 80, rect.Height), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                        }
                    }
                    return new Rectangle(rect.X + 20, rect.Y, rect.Width - 20, rect.Height);
            }
        }

        #endregion
    }

    public interface DrawerLoad
    {
        void LoadOK();
    }
}