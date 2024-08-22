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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormFloatButton : ILayeredFormOpacity, IEventListener
    {
        FloatButton.Config config;
        int BadgeSize = 6, ShadowXY = 20, ShadowS = 40;
        public LayeredFormFloatButton(FloatButton.Config _config)
        {
            maxalpha = 255;
            config = _config;
            TopMost = config.TopMost;
            if (!config.TopMost) config.Form.SetTopMost(Handle);
            Font = config.Font == null ? config.Form.Font : config.Font;

            Helper.GDI(g =>
            {
                var dpi = Config.Dpi;
                BadgeSize = (int)Math.Round(BadgeSize * dpi);
                _config.MarginX = (int)Math.Round(_config.MarginX * dpi);
                _config.MarginY = (int)Math.Round(_config.MarginY * dpi);
                _config.Size = (int)Math.Round(_config.Size * dpi);
                int size = _config.Size, t_size = size + ShadowS, icon_size = (int)(size * 0.45F), xy = (size - icon_size) / 2;
                int hasx = 0, hasy = 0;
                if (_config.Vertical)
                {
                    foreach (var it in _config.Btns)
                    {
                        it.PropertyChanged += Notify_PropertyChanged;
                        it.rect = new Rectangle(hasx, hasy, t_size, t_size);
                        it.rect_read = new Rectangle(hasx + ShadowXY, hasy + ShadowXY, size, size);
                        it.rect_icon = new Rectangle(it.rect_read.X + xy, it.rect_read.Y + xy, icon_size, icon_size);
                        hasy += t_size;
                    }
                    SetSize(size + ShadowS, t_size * _config.Btns.Length);
                }
                else
                {
                    foreach (var it in _config.Btns)
                    {
                        it.PropertyChanged += Notify_PropertyChanged;
                        it.rect = new Rectangle(hasx, hasy, t_size, t_size);
                        it.rect_read = new Rectangle(hasx + ShadowXY, hasy + ShadowXY, size, size);
                        it.rect_icon = new Rectangle(it.rect_read.X + xy, it.rect_read.Y + xy, icon_size, icon_size);
                        hasx += t_size;
                    }
                    SetSize(t_size * _config.Btns.Length, size + ShadowS);
                }
            });
            SetPoint();
            config.Form.LocationChanged += Form_LSChanged;
            config.Form.SizeChanged += Form_LSChanged;
        }

        private void Notify_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender == null || e.PropertyName == null) return;
            Print();
        }

        bool SetPoint()
        {
            if (config.Control == null)
            {
                var point = config.Form.Location;
                SetPoint(point.X, point.Y, config.Form.Width, config.Form.Height);
            }
            else
            {
                if (config.Control.IsDisposed)
                {
                    IClose();
                    return false;
                }
                var point = config.Control.PointToScreen(Point.Empty);
                SetPoint(point.X, point.Y, config.Control.Width, config.Control.Height);
            }
            return true;
        }

        void SetPoint(int x, int y, int w, int h)
        {
            switch (config.Align)
            {
                case TAlign.TL:
                case TAlign.LT:
                    SetLocation(new Point(x + config.MarginY, y + config.MarginY));
                    break;
                case TAlign.Top:
                    SetLocation(new Point(x + (w - TargetRect.Width) / 2, y + config.MarginY));
                    break;
                case TAlign.TR:
                case TAlign.RT:
                    SetLocation(new Point(x + w - config.MarginX - TargetRect.Width, y + config.MarginY));
                    break;
                case TAlign.Left:
                    SetLocation(new Point(x + config.MarginY, y + (h - TargetRect.Height) / 2));
                    break;
                case TAlign.Right:
                    SetLocation(new Point(x + w - config.MarginX - TargetRect.Width, y + (h - TargetRect.Height) / 2));
                    break;

                case TAlign.BL:
                case TAlign.LB:
                    SetLocation(new Point(x + config.MarginY, y + h - config.MarginY - TargetRect.Height));
                    break;
                case TAlign.Bottom:
                    SetLocation(new Point(x + (w - TargetRect.Width) / 2, y + h - config.MarginY - TargetRect.Height));
                    break;

                case TAlign.RB:
                case TAlign.BR:
                default:
                    SetLocation(new Point(x + w - config.MarginX - TargetRect.Width, y + h - config.MarginY - TargetRect.Height));
                    break;
            }
        }

        private void Form_LSChanged(object? sender, EventArgs e)
        {
            if (SetPoint()) Print();
        }

        #region 渲染

        readonly StringFormat stringCenter = Helper.SF_NoWrap();

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                foreach (var it in config.Btns)
                {
                    using (var path = DrawShadow(g, it))
                    {
                        Color back, back_hover, fore;
                        switch (it.Type)
                        {
                            case TTypeMini.Primary:
                                back = Style.Db.Primary;
                                back_hover = Style.Db.PrimaryHover;
                                fore = Style.Db.PrimaryColor;
                                break;
                            case TTypeMini.Success:
                                back = Style.Db.Success;
                                back_hover = Style.Db.SuccessHover;
                                fore = Style.Db.SuccessColor;
                                break;
                            case TTypeMini.Error:
                                back = Style.Db.Error;
                                back_hover = Style.Db.ErrorHover;
                                fore = Style.Db.ErrorColor;
                                break;
                            case TTypeMini.Warn:
                                back = Style.Db.Warning;
                                back_hover = Style.Db.WarningHover;
                                fore = Style.Db.WarningColor;
                                break;
                            case TTypeMini.Info:
                                back = Style.Db.Info;
                                back_hover = Style.Db.InfoHover;
                                fore = Style.Db.InfoColor;
                                break;
                            default:
                                back = Style.Db.BgElevated;
                                back_hover = Style.Db.FillSecondary;
                                fore = Style.Db.Text;
                                break;
                        }
                        using (var brush = new SolidBrush(back))
                        {
                            g.FillPath(brush, path);
                        }
                        if (it.hover)
                        {
                            using (var brush = new SolidBrush(back_hover))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        if (it.IconSvg != null)
                        {
                            using (var _bmp = SvgExtend.GetImgExtend(it.IconSvg, it.rect_icon, fore))
                            {
                                if (_bmp != null) g.DrawImage(_bmp, it.rect_icon);
                            }
                        }
                        else if (it.Icon != null) g.DrawImage(it.Icon, it.rect_icon);
                        else
                        {
                            using (var brush = new SolidBrush(fore))
                            {
                                g.DrawString(it.Text, Font, brush, it.rect_read, stringCenter);
                            }
                        }
                        PrintBadge(g, it);
                    }
                }
            }
            return original_bmp;
        }

        readonly StringFormat stringBadge = Helper.SF_NoWrap();

        void PrintBadge(Graphics g, FloatButton.ConfigBtn it)
        {
            if (it.Badge != null)
            {
                var color = it.BadgeBack ?? Style.Db.Error;

                using (var brush_fore = new SolidBrush(Style.Db.ErrorColor))
                {
                    if (it.Badge == " ")
                    {
                        var rect_badge = new Rectangle(it.rect_read.Right - BadgeSize, it.rect_read.Y, BadgeSize, BadgeSize);
                        using (var brush = new SolidBrush(color))
                        {
                            g.FillEllipse(brush, rect_badge);
                            using (var pen = new Pen(brush_fore.Color, 1F))
                            {
                                g.DrawEllipse(pen, rect_badge);
                            }
                        }
                    }
                    else
                    {
                        using (var font = new Font(Font.FontFamily, it.BadgeSize))
                        {
                            var size = g.MeasureString(it.Badge, font);
                            float size_badge = size.Height * 1.2F, size_badge2 = size_badge * 0.4F;
                            if (size.Height > size.Width)
                            {
                                var rect_badge = new RectangleF(it.rect_read.Right + size_badge2 - size_badge, it.rect_read.Y - size_badge2, size_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    g.FillEllipse(brush, rect_badge);
                                    using (var pen = new Pen(brush_fore.Color, 1F))
                                    {
                                        g.DrawEllipse(pen, rect_badge);
                                    }
                                }
                                g.DrawString(it.Badge, font, brush_fore, rect_badge, stringBadge);
                            }
                            else
                            {
                                var w_badge = size.Width * 1.2F;
                                var rect_badge = new RectangleF(it.rect_read.Right + size_badge2 - w_badge, it.rect_read.Y - size_badge2, w_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    using (var path = rect_badge.RoundPath(rect_badge.Height))
                                    {
                                        g.FillPath(brush, path);
                                        using (var pen = new Pen(brush_fore.Color, 1F))
                                        {
                                            g.DrawPath(pen, path);
                                        }
                                    }
                                }
                                g.DrawString(it.Badge, font, brush_fore, rect_badge, stringBadge);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        GraphicsPath DrawShadow(Graphics g, FloatButton.ConfigBtn it)
        {
            bool round = it.Round || it.Shape == TShape.Circle;
            float radius = round ? it.rect_read.Height : it.Radius * Config.Dpi;
            var path = Helper.RoundPath(it.rect_read, radius, round);
            if (Config.ShadowEnabled)
            {
                if (it.shadow_temp == null || (it.shadow_temp.Width != it.rect.Width || it.shadow_temp.Height != it.rect.Height))
                {
                    it.shadow_temp?.Dispose();
                    using (var path2 = Helper.RoundPath(new Rectangle(ShadowXY, ShadowXY, it.rect_read.Width, it.rect_read.Height), radius, round))
                    {
                        it.shadow_temp = path2.PaintShadow(it.rect.Width, it.rect.Height, 14);
                    }
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix { Matrix33 = 0.2F };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(it.shadow_temp, new Rectangle(it.rect.X, it.rect.Y + 6, it.rect.Width, it.rect.Height), 0, 0, it.rect.Width, it.rect.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return path;
        }

        #endregion

        #region 鼠标

        TooltipForm? tooltipForm = null;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int count = 0, hand = 0;
            foreach (var it in config.Btns)
            {
                if (it.rect.Contains(e.Location))
                {
                    hand++;
                    if (!it.hover)
                    {
                        it.hover = true;
                        count++;
                        if (it.Tooltip != null)
                        {
                            var _rect = TargetRect;
                            var rect = new Rectangle(_rect.X + it.rect.X, _rect.Y + it.rect.Y, it.rect.Width, it.rect.Height);
                            if (tooltipForm == null)
                            {
                                tooltipForm = new TooltipForm(config.Form, rect, it.Tooltip, new TooltipConfig
                                {
                                    Font = Font,
                                    ArrowAlign = config.Align.AlignMiniReverse(config.Vertical),
                                });
                                tooltipForm.Show(this);
                            }
                            else tooltipForm.SetText(rect, it.Tooltip);
                        }
                    }
                }
                else
                {
                    if (it.hover) { it.hover = false; count++; }
                }
            }
            SetCursor(hand > 0);
            if (count > 0) Print();
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            tooltipForm?.IClose();
            tooltipForm = null;
            int count = 0;
            foreach (var it in config.Btns)
            {
                if (it.hover) { it.hover = false; count++; }
            }
            SetCursor(false);
            if (count > 0) Print();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (var it in config.Btns)
                {
                    if (it.rect.Contains(e.Location))
                    {
                        config.Call.Invoke(it);
                        return;
                    }
                }
            }
            base.OnMouseClick(e);
        }

        #endregion

        #region 主题

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
        }

        protected override void Dispose(bool disposing)
        {
            config.Form.LocationChanged -= Form_LSChanged;
            config.Form.SizeChanged -= Form_LSChanged;
            foreach (var it in config.Btns) it.PropertyChanged -= Notify_PropertyChanged;
            base.Dispose(disposing);
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.THEME:
                    Print();
                    break;
            }
        }

        #endregion
    }
}