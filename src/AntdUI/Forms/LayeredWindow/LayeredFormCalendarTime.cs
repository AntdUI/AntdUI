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
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormCalendarTime : ILayeredFormOpacityDown
    {
        public LayeredFormCalendarTime(TimePicker _control, RectangleF rect_read, TimeSpan date, Action<TimeSpan> _action)
        {
            control = _control;
            PARENT = _control;
            action = _action;
            scrollY_h = new ScrollY(this);
            scrollY_m = new ScrollY(this);
            scrollY_s = new ScrollY(this);

            hover_button = new ITaskOpacity(this);
            hover_buttonok = new ITaskOpacity(this);

            #region 数据

            calendar_time = new List<CalendarT>(24 + 120);
            for (int i = 0; i < 24; i++) calendar_time.Add(new CalendarT(0, i, i));
            for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(1, i, i));
            for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(2, i, i));

            #endregion

            float dpi = Config.Dpi;
            if (dpi != 1F)
            {
                Radius = _control.Radius * dpi;
                t_height = (int)(t_height * dpi);
                t_time = (int)(t_time * dpi);
                t_time_height = (int)(t_time_height * dpi);
                t_button = (int)(t_button * dpi);
            }
            else Radius = _control.Radius;

            t_width = t_time * 3;

            Font = new Font(_control.Font.FontFamily, 11.2F);

            SelDate = date;
            //Date = date.HasValue ? date.Value : DateNow;

            var point = _control.PointToScreen(Point.Empty);
            EndHeight = t_height + t_button + 20;
            SetSize(t_width + 20, 0);

            #region 布局

            float size_time_one = t_time * 0.857F;
            float size_time_height_one = t_time_height * 0.93F;

            var rect_s_h = new Rectangle(10, 18, t_time, t_height - 8);
            rect_read_h = new Rectangle(rect_s_h.Right - t_time, rect_s_h.Y, t_time, rect_s_h.Height);
            rect_read_m = new Rectangle(rect_s_h.Right, rect_s_h.Y, t_time, rect_s_h.Height);
            rect_read_s = new Rectangle(rect_s_h.Right + t_time, rect_s_h.Y, t_time, rect_s_h.Height);
            scrollY_h.SizeChange(rect_s_h);
            rect_s_h.Width += t_time;
            scrollY_m.SizeChange(rect_s_h);
            rect_s_h.Width += t_time;
            scrollY_s.SizeChange(rect_s_h);

            int endh2 = t_height - (t_time_height - (int)size_time_height_one);
            scrollY_h.SetVrSize(t_time_height * 24, endh2);
            scrollY_m.SetVrSize(t_time_height * 60, endh2);
            scrollY_s.SetVrSize(t_time_height * 60, endh2);

            float _x = (t_time - size_time_one) / 2F, _y = (t_time_height - size_time_height_one) / 2F;
            foreach (var it in calendar_time)
            {
                it.rect = new RectangleF(10 + t_time * it.x, 10 + 4 + t_time_height * it.y, t_time, t_time_height);
                it.rect_read = new RectangleF(it.rect.X + _x, it.rect.Y + _y, size_time_one, size_time_height_one);
            }

            var find_h = calendar_time.Find(a => a.x == 0 && a.t == SelDate.Hours);
            if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
            var find_m = calendar_time.Find(a => a.x == 1 && a.t == SelDate.Minutes);
            if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
            var find_s = calendar_time.Find(a => a.x == 2 && a.t == SelDate.Seconds);
            if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;

            #endregion

            rect_button = new Rectangle(10, 10 + t_height, t_width / 2, t_button);
            rect_buttonok = new Rectangle(rect_button.Right, rect_button.Top, rect_button.Width, rect_button.Height);

            switch (_control.Placement)
            {
                case TAlignFrom.Top:
                    Inverted = true;
                    if (_control.DropDownArrow)
                    {
                        ArrowAlign = TAlign.Top;
                        SetLocation(point.X + (_control.Width - (t_width + 20)) / 2, point.Y - EndHeight + 10 - ArrowSize);
                    }
                    else SetLocation(point.X + (_control.Width - (t_width + 20)) / 2, point.Y - EndHeight + 10);
                    break;
                case TAlignFrom.TL:
                    Inverted = true;
                    if (_control.DropDownArrow)
                    {
                        ArrowAlign = TAlign.TL;
                        SetLocation(point.X + (int)rect_read.X - 10, point.Y - EndHeight + 10 - ArrowSize);
                    }
                    else SetLocation(point.X + (int)rect_read.X - 10, point.Y - EndHeight + 10);
                    break;
                case TAlignFrom.TR:
                    Inverted = true;
                    if (_control.DropDownArrow)
                    {
                        ArrowAlign = TAlign.TR;
                        SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - t_width - 10, point.Y - EndHeight + 10 - ArrowSize);
                    }
                    else SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - t_width - 10, point.Y - EndHeight + 10);
                    break;
                case TAlignFrom.Bottom:
                    if (_control.DropDownArrow)
                    {
                        ArrowAlign = TAlign.Bottom;
                        SetLocation(point.X + (_control.Width - (t_width + 20)) / 2, point.Y + _control.Height - 10 + ArrowSize);
                    }
                    else SetLocation(point.X + (_control.Width - (t_width + 20)) / 2, point.Y + _control.Height - 10);
                    break;
                case TAlignFrom.BR:
                    if (_control.DropDownArrow)
                    {
                        ArrowAlign = TAlign.BR;
                        SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - t_width - 10, point.Y + _control.Height - 10 + ArrowSize);
                    }
                    else SetLocation(point.X + (int)(rect_read.X + rect_read.Width) - t_width - 10, point.Y + _control.Height - 10);
                    break;
                case TAlignFrom.BL:
                default:
                    if (_control.DropDownArrow)
                    {
                        ArrowAlign = TAlign.BL;
                        SetLocation(point.X + (int)rect_read.X - 10, point.Y + _control.Height - 10 + ArrowSize);
                    }
                    else SetLocation(point.X + (int)rect_read.X - 10, point.Y + _control.Height - 10);
                    break;

            }
        }

        #region 属性

        TimeSpan SelDate;

        #region 参数

        IControl control;

        float Radius = 6;
        int t_width = 0, t_button = 38, t_time = 56, t_height = 224, t_time_height = 30;
        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;

        ScrollY scrollY_h, scrollY_m, scrollY_s;

        /// <summary>
        /// 回调
        /// </summary>
        Action<TimeSpan> action;

        #endregion

        #region 日期

        DateTime DateNow = DateTime.Now;
        List<CalendarT> calendar_time;

        #endregion

        #endregion

        #region 渲染

        string button_text = Localization.Provider?.GetLocalizedString("Now") ?? "此刻";
        string OKButton = Localization.Provider?.GetLocalizedString("OK") ?? "确定";
        StringFormat stringFormatC = Helper.SF();
        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path = rect_read.RoundPath(Radius))
                {
                    DrawShadow(g, rect, rect.Width, EndHeight);
                    using (var brush = new SolidBrush(Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path);
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                    }
                }

                using (var brush_fore = new SolidBrush(Style.Db.TextBase))
                {
                    using (var bmp = new Bitmap(t_width + 20, t_height + 20))
                    using (var brush_bg = new SolidBrush(Style.Db.PrimaryBg))
                    {
                        using (var g2 = Graphics.FromImage(bmp).HighLay())
                        {
                            g2.TranslateTransform(0, -scrollY_h.Value);
                            for (int i = 0; i < calendar_time.Count; i++)
                            {
                                if (i == 24)
                                {
                                    g2.ResetTransform();
                                    g2.TranslateTransform(0, -scrollY_m.Value);
                                }
                                else if (i == 84)
                                {
                                    g2.ResetTransform();
                                    g2.TranslateTransform(0, -scrollY_s.Value);
                                }
                                var it = calendar_time[i];
                                using (var path = it.rect_read.RoundPath(Radius))
                                {
                                    switch (it.x)
                                    {
                                        case 0:
                                            if (it.t == SelDate.Hours) g2.FillPath(brush_bg, path);
                                            break;
                                        case 1:
                                            if (it.t == SelDate.Minutes) g2.FillPath(brush_bg, path);
                                            break;
                                        case 2:
                                            if (it.t == SelDate.Seconds) g2.FillPath(brush_bg, path);
                                            break;
                                    }
                                    if (it.hover)
                                    {
                                        using (var brush_hove = new SolidBrush(Style.Db.FillTertiary))
                                        {
                                            g2.FillPath(brush_hove, path);
                                        }
                                    }
                                    g2.DrawString(it.v, Font, brush_fore, it.rect_read, stringFormatC);
                                }
                            }
                        }
                        g.DrawImage(bmp, new Rectangle(0, 10, bmp.Width, bmp.Height), new Rectangle(0, 10, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                    }
                    scrollY_h.Paint(g);
                    scrollY_m.Paint(g);
                    scrollY_s.Paint(g);

                    using (var brush_active = new SolidBrush(Style.Db.Primary))
                    {
                        if (hover_button.Animation)
                        {
                            g.DrawString(button_text, Font, brush_active, rect_button, stringFormatC);
                            using (var brush_hove = new SolidBrush(Color.FromArgb(hover_button.Value, Style.Db.PrimaryActive)))
                            {
                                g.DrawString(button_text, Font, brush_hove, rect_button, stringFormatC);
                            }
                        }
                        else if (hover_button.Switch)
                        {
                            using (var brush_hove = new SolidBrush(Style.Db.PrimaryActive))
                            {
                                g.DrawString(button_text, Font, brush_hove, rect_button, stringFormatC);
                            }
                        }
                        else g.DrawString(button_text, Font, brush_active, rect_button, stringFormatC);

                        if (hover_buttonok.Animation)
                        {
                            g.DrawString(OKButton, Font, brush_active, rect_buttonok, stringFormatC);
                            using (var brush_hove = new SolidBrush(Color.FromArgb(hover_buttonok.Value, Style.Db.PrimaryActive)))
                            {
                                g.DrawString(OKButton, Font, brush_hove, rect_buttonok, stringFormatC);
                            }
                        }
                        else if (hover_buttonok.Switch)
                        {
                            using (var brush_hove = new SolidBrush(Style.Db.PrimaryActive))
                            {
                                g.DrawString(OKButton, Font, brush_hove, rect_buttonok, stringFormatC);
                            }
                        }
                        else g.DrawString(OKButton, Font, brush_active, rect_buttonok, stringFormatC);
                    }
                }
            }
            return original_bmp;
        }

        Rectangle rect_read_h, rect_read_m, rect_read_s;

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="shadow_width">最终阴影宽度</param>
        /// <param name="shadow_height">最终阴影高度</param>
        void DrawShadow(Graphics g, Rectangle rect_client, int shadow_width, int shadow_height)
        {
            if (shadow_temp == null || (shadow_temp.Width != shadow_width || shadow_temp.Height != shadow_height))
            {
                shadow_temp?.Dispose();
                using (var path = new Rectangle(10, 10, shadow_width - 20, shadow_height - 20).RoundPath(Radius))
                {
                    shadow_temp = path.PaintShadow(shadow_width, shadow_height);
                }
            }
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix { Matrix33 = 0.2F };
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(shadow_temp, rect_client, 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        #endregion

        #region 鼠标

        ITaskOpacity hover_button, hover_buttonok;
        Rectangle rect_button = new Rectangle(-20, -20, 10, 10), rect_buttonok = new Rectangle(-20, -20, 10, 10);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (rect_read_h.Contains(e.Location)) scrollY_h.MouseDown(e.Location);
            else if (rect_read_m.Contains(e.Location)) scrollY_m.MouseDown(e.Location);
            else if (rect_read_s.Contains(e.Location)) scrollY_s.MouseDown(e.Location);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (scrollY_h.MouseMove(e.Location) && scrollY_m.MouseMove(e.Location) && scrollY_s.MouseMove(e.Location))
            {
                int count = 0, hand = 0;
                bool _hover_button = rect_button.Contains(e.Location),
                 _hover_buttonok = rect_buttonok.Contains(e.Location);

                if (_hover_button != hover_button.Switch) count++;
                if (_hover_buttonok != hover_buttonok.Switch) count++;

                hover_button.Switch = _hover_button;
                hover_buttonok.Switch = _hover_buttonok;
                if (hover_button.Switch || hover_buttonok.Switch)
                {
                    hand++;
                }
                else
                {
                    if (calendar_time != null)
                    {
                        foreach (var it in calendar_time)
                        {
                            switch (it.x)
                            {
                                case 1:
                                    if (it.Contains(e.Location, 0, scrollY_m.Value, out var change1)) hand++;
                                    if (change1) count++;
                                    break;
                                case 2:
                                    if (it.Contains(e.Location, 0, scrollY_s.Value, out var change2)) hand++;
                                    if (change2) count++;
                                    break;
                                case 0:
                                default:
                                    if (it.Contains(e.Location, 0, scrollY_h.Value, out var change0)) hand++;
                                    if (change0) count++;
                                    break;
                            }
                        }
                    }
                }
                if (count > 0) Print();
                SetCursor(hand > 0);
            }
            else SetCursor(false);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            scrollY_h.Leave();
            scrollY_m.Leave();
            scrollY_s.Leave();
            if (calendar_time != null)
            {
                foreach (var it in calendar_time)
                {
                    it.hover = false;
                }
            }
            SetCursor(false);
            Print();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            scrollY_h.MouseUp(e.Location);
            scrollY_m.MouseUp(e.Location);
            scrollY_s.MouseUp(e.Location);
            if (e.Button == MouseButtons.Left)
            {
                if (rect_button.Contains(e.Location))
                {
                    DateNow = DateTime.Now;
                    SelDate = new TimeSpan(DateNow.Hour, DateNow.Minute, DateNow.Second);
                    action(SelDate);
                    var find_h = calendar_time.Find(a => a.x == 0 && a.t == SelDate.Hours);
                    if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                    var find_m = calendar_time.Find(a => a.x == 1 && a.t == SelDate.Minutes);
                    if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                    var find_s = calendar_time.Find(a => a.x == 2 && a.t == SelDate.Seconds);
                    if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                    Print();
                    return;
                }
                else if (rect_buttonok.Contains(e.Location))
                {
                    action(SelDate);
                    IClose();
                    return;
                }

                foreach (var it in calendar_time)
                {
                    switch (it.x)
                    {
                        case 1:
                            if (it.Contains(e.Location, 0, scrollY_m.Value, out _))
                            {
                                SelDate = new TimeSpan(SelDate.Hours, it.t, SelDate.Seconds);
                                Print();
                                return;
                            }
                            break;
                        case 2:
                            if (it.Contains(e.Location, 0, scrollY_s.Value, out _))
                            {
                                SelDate = new TimeSpan(SelDate.Hours, SelDate.Minutes, it.t);
                                Print();
                                return;
                            }
                            break;
                        case 0:
                        default:
                            if (it.Contains(e.Location, 0, scrollY_h.Value, out _))
                            {
                                SelDate = new TimeSpan(it.t, SelDate.Minutes, SelDate.Seconds);
                                Print();
                                return;
                            }
                            break;
                    }
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (rect_read_h.Contains(e.Location))
                {
                    scrollY_h.MouseWheel(e.Delta);
                    Print();
                }
                else if (rect_read_m.Contains(e.Location))
                {
                    scrollY_m.MouseWheel(e.Delta);
                    Print();
                }
                else if (rect_read_s.Contains(e.Location))
                {
                    scrollY_s.MouseWheel(e.Delta);
                    Print();
                }
            }
            base.OnMouseWheel(e);
        }

        #endregion
    }
}