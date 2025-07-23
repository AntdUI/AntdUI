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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormFloatButton : FormFloatButton, IEventListener
    {
        public override FloatButton.Config config { get; }

        int BadgeSize = 6, ShadowXY;
        public LayeredFormFloatButton(FloatButton.Config _config)
        {
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
                _config.Gap = (int)Math.Round(_config.Gap * dpi);
                ShadowXY = _config.Gap / 2;
                int size = _config.Size, t_size = size + _config.Gap, icon_size = (int)(size * 0.45F), xy = (size - icon_size) / 2;
                int hasx = 0, hasy = 0;
                if (_config.Vertical)
                {
                    foreach (var it in _config.Btns)
                    {
                        it.PropertyChanged += Notify_PropertyChanged;
                        it.rect = new Rectangle(hasx, hasy, t_size, t_size);
                        it.rect_read = new Rectangle(hasx + ShadowXY, hasy + ShadowXY, size, size);
                        SetIconSize(it, size, xy, icon_size, dpi);
                        hasy += t_size;
                    }
                    SetSize(size + _config.Gap, hasy);
                }
                else
                {
                    foreach (var it in _config.Btns)
                    {
                        it.PropertyChanged += Notify_PropertyChanged;
                        it.rect = new Rectangle(hasx, hasy, t_size, t_size);
                        it.rect_read = new Rectangle(hasx + ShadowXY, hasy + ShadowXY, size, size);
                        SetIconSize(it, size, xy, icon_size, dpi);
                        hasx += t_size;
                    }
                    SetSize(hasx, size + _config.Gap);
                }
            });
            SetPoint();
            config.Form.LocationChanged += Form_LSChanged;
            config.Form.SizeChanged += Form_LSChanged;
        }

        public override string name => nameof(FloatButton);

        private void Notify_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender == null || e.PropertyName == null) return;
            if (e.PropertyName == "Loading")
            {
                var loading = HasLoading;
                if (Loading == loading) return;
                Loading = loading;
                if (loading)
                {
                    ThreadLoading = new ITask(this, i =>
                    {
                        foreach (var it in config.Btns)
                        {
                            if (it.Loading) it.AnimationLoadingValue = i;
                        }
                        Print();
                        return Loading;
                    }, 10, 360, 6, () =>
                    {
                        Print();
                    });
                }
                else
                {
                    ThreadLoading?.Dispose();
                    ThreadLoading = null;
                    Print();
                }
            }
            else Print();
        }

        #region Loading

        bool Loading = false;
        bool HasLoading
        {
            get
            {
                foreach (var it in config.Btns)
                {
                    if (it.Loading) return true;
                }
                return false;
            }
        }
        ITask? ThreadLoading;

        #endregion

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

        void SetIconSize(FloatButton.ConfigBtn it, int size, int xy, int icon_size, float dpi)
        {
            if (it.IconSize.HasValue)
            {
                if (it.IconSize.Value.Width == it.IconSize.Value.Height)
                {
                    int icon_size_div = (int)(it.IconSize.Value.Width * dpi);
                    it.rect_icon = new Rectangle(it.rect_read.X + (size - icon_size_div) / 2, it.rect_read.Y + (size - icon_size_div) / 2, icon_size_div, icon_size_div);
                }
                else
                {
                    int icon_size_w = (int)(it.IconSize.Value.Width * dpi), icon_size_h = (int)(it.IconSize.Value.Height * dpi);
                    it.rect_icon = new Rectangle(it.rect_read.X + (size - icon_size_w) / 2, it.rect_read.Y + (size - icon_size_h) / 2, icon_size_w, icon_size_h);
                }
            }
            else it.rect_icon = new Rectangle(it.rect_read.X + xy, it.rect_read.Y + xy, icon_size, icon_size);
        }

        private void Form_LSChanged(object? sender, EventArgs e)
        {
            if (SetPoint()) Print();
        }

        #region 渲染

        readonly StringFormat stringCenter = Helper.SF_NoWrap();

        int use_primary = 0;
        public override Bitmap PrintBit()
        {
            use_primary = 0;
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                foreach (var it in config.Btns)
                {
                    using (var path = DrawShadow(g, it))
                    {
                        if (it.Loading)
                        {
                            Color back, fore;
                            switch (it.Type)
                            {
                                case TTypeMini.Primary:
                                    use_primary++;
                                    back = Colour.Primary.Get("FloatButton");
                                    fore = Colour.PrimaryColor.Get("FloatButton");
                                    break;
                                case TTypeMini.Success:
                                    back = Colour.Success.Get("FloatButton");
                                    fore = Colour.SuccessColor.Get("FloatButton");
                                    break;
                                case TTypeMini.Error:
                                    back = Colour.Error.Get("FloatButton");
                                    fore = Colour.ErrorColor.Get("FloatButton");
                                    break;
                                case TTypeMini.Warn:
                                    back = Colour.Warning.Get("FloatButton");
                                    fore = Colour.WarningColor.Get("FloatButton");
                                    break;
                                case TTypeMini.Info:
                                    back = Colour.Info.Get("FloatButton");
                                    fore = Colour.InfoColor.Get("FloatButton");
                                    break;
                                default:
                                    back = Colour.BgElevated.Get("FloatButton");
                                    fore = Colour.Text.Get("FloatButton");
                                    break;
                            }
                            if (it.Fore.HasValue) fore = it.Fore.Value;

                            g.Fill(back, path);

                            float loading_size = it.rect_read.Height * 0.06F;
                            using (var pen = new Pen(Colour.Fill.Get("FloatButton"), loading_size))
                            using (var brush = new Pen(fore, pen.Width))
                            {
                                g.DrawEllipse(pen, it.rect_icon);
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, it.rect_icon, it.AnimationLoadingValue, it.LoadingValue * 360F);
                            }
                        }
                        else
                        {
                            Color back, back_hover, fore;
                            if (it.Enabled)
                            {
                                switch (it.Type)
                                {
                                    case TTypeMini.Primary:
                                        use_primary++;
                                        back = Colour.Primary.Get("FloatButton");
                                        back_hover = Colour.PrimaryHover.Get("FloatButton");
                                        fore = Colour.PrimaryColor.Get("FloatButton");
                                        break;
                                    case TTypeMini.Success:
                                        back = Colour.Success.Get("FloatButton");
                                        back_hover = Colour.SuccessHover.Get("FloatButton");
                                        fore = Colour.SuccessColor.Get("FloatButton");
                                        break;
                                    case TTypeMini.Error:
                                        back = Colour.Error.Get("FloatButton");
                                        back_hover = Colour.ErrorHover.Get("FloatButton");
                                        fore = Colour.ErrorColor.Get("FloatButton");
                                        break;
                                    case TTypeMini.Warn:
                                        back = Colour.Warning.Get("FloatButton");
                                        back_hover = Colour.WarningHover.Get("FloatButton");
                                        fore = Colour.WarningColor.Get("FloatButton");
                                        break;
                                    case TTypeMini.Info:
                                        back = Colour.Info.Get("FloatButton");
                                        back_hover = Colour.InfoHover.Get("FloatButton");
                                        fore = Colour.InfoColor.Get("FloatButton");
                                        break;
                                    default:
                                        back = Colour.BgElevated.Get("FloatButton");
                                        back_hover = Colour.FillSecondary.Get("FloatButton");
                                        fore = Colour.Text.Get("FloatButton");
                                        break;
                                }
                                if (it.Fore.HasValue) fore = it.Fore.Value;
                            }
                            else
                            {
                                back = back_hover = Colour.FillTertiary.Get("FloatButton");
                                fore = Colour.TextQuaternary.Get("FloatButton");
                            }

                            g.Fill(back, path);
                            if (it.hover) g.Fill(back_hover, path);
                            if (it.IconSvg != null) g.GetImgExtend(it.IconSvg, it.rect_icon, fore);
                            else if (it.Icon != null) g.Image(it.Icon, it.rect_icon);
                            else g.String(it.Text, Font, fore, it.rect_read, stringCenter);
                        }
                        it.PaintBadge(Font, it.rect_read, g, TAMode.Auto);
                    }
                }
            }
            return original_bmp;
        }

        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        GraphicsPath DrawShadow(Canvas g, FloatButton.ConfigBtn it)
        {
            bool round = it.Round;
            float radius = round ? it.rect_read.Height : it.Radius * Config.Dpi;
            var path = Helper.RoundPath(it.rect_read, radius, round);
            if (Config.ShadowEnabled && it.Enabled)
            {
                if (it.shadow_temp == null || (it.shadow_temp.Width != it.rect.Width || it.shadow_temp.Height != it.rect.Height))
                {
                    it.shadow_temp?.Dispose();
                    using (var path2 = Helper.RoundPath(new Rectangle(ShadowXY, ShadowXY, it.rect_read.Width, it.rect_read.Height), radius, round))
                    {
                        it.shadow_temp = path2.PaintShadowO(it.rect.Width, it.rect.Height, 14);
                    }
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix { Matrix33 = 0.2F };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.Image(it.shadow_temp, new Rectangle(it.rect.X, it.rect.Y + 6, it.rect.Width, it.rect.Height), 0, 0, it.rect.Width, it.rect.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return path;
        }

        #endregion

        #region 鼠标

        TooltipForm? tooltipForm;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int count = 0, hand = 0;
            foreach (var it in config.Btns)
            {
                if (it.Enabled && !it.Loading && it.rect.Contains(e.Location))
                {
                    hand++;
                    if (!it.hover)
                    {
                        it.hover = true;
                        count++;
                        var tooltip = it.Tooltip;
                        if (tooltip != null)
                        {
                            var _rect = TargetRect;
                            var rect = new Rectangle(_rect.X + it.rect.X, _rect.Y + it.rect.Y, it.rect.Width, it.rect.Height);
                            if (tooltipForm == null)
                            {
                                tooltipForm = new TooltipForm(config.Form, rect, tooltip, new TooltipConfig
                                {
                                    Font = Font,
                                    ArrowAlign = config.Align.AlignMiniReverse(config.Vertical),
                                });
                                tooltipForm.Show(this);
                            }
                            else tooltipForm.SetText(rect, tooltip);
                        }
                    }
                }
                else
                {
                    if (it.hover)
                    {
                        it.hover = false;
                        count++;
                    }
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
                if (it.hover)
                {
                    it.hover = false;
                    count++;
                }
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
                    if (it.Enabled && !it.Loading && it.rect.Contains(e.Location))
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
            ThreadLoading?.Dispose();
            config.Form.LocationChanged -= Form_LSChanged;
            config.Form.SizeChanged -= Form_LSChanged;
            foreach (var it in config.Btns)
            {
                it.shadow_temp?.Dispose();
                it.PropertyChanged -= Notify_PropertyChanged;
            }
            base.Dispose(disposing);
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.THEME:
                    Print();
                    break;
                case EventType.THEME_PRIMARY:
                    if (use_primary > 0) Print();
                    break;
            }
        }

        #endregion
    }

    public abstract class FormFloatButton : ILayeredFormOpacity
    {
        public abstract FloatButton.Config config { get; }
    }
}