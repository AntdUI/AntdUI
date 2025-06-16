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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormCalendar : ILayeredFormOpacityDown
    {
        #region 初始化

        DateTime? minDate, maxDate;
        bool ValueTimeHorizontal = false, ShowButtonToDay = true;
        TAMode ColorScheme;
        int shadow = 10, shadow2 = 20;
        public LayeredFormCalendar(DatePicker _control, Rectangle rect_read, DateTime? date, Action<DateTime> _action, Action<object> _action_btns, Func<DateTime[], List<DateBadge>?>? _badge_action = null)
        {
            ColorScheme = _control.ColorScheme;
            _control.Parent.SetTopMost(Handle);
            control = _control;
            minDate = _control.MinDate;
            maxDate = _control.MaxDate;
            badge_action = _badge_action;
            PARENT = _control;
            action = _action;
            action_btns = _action_btns;
            ShowTime = _control.ShowTime;
            ShowButtonToDay = _control.ShowButtonToDay;
            ValueTimeHorizontal = _control.ValueTimeHorizontal;
            hover_lefts = new ITaskOpacity(name, this);
            hover_left = new ITaskOpacity(name, this);
            hover_rights = new ITaskOpacity(name, this);
            hover_right = new ITaskOpacity(name, this);
            hover_year = new ITaskOpacity(name, this);
            hover_month = new ITaskOpacity(name, this);
            hover_button = new ITaskOpacity(name, this);
            hover_buttonok = new ITaskOpacity(name, this);
            scrollY_left = new ScrollY(this);
            scrollY_h = new ScrollY(this);
            scrollY_m = new ScrollY(this);
            scrollY_s = new ScrollY(this);
            showType = PickerType = _control.Picker;
            Culture = new CultureInfo(CultureID);
            YDR = CultureID.StartsWith("en");

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = _control.Radius;
            else
            {
                Radius = _control.Radius * dpi;
                t_one_width = (int)(t_one_width * dpi);
                t_top = (int)(t_top * dpi);
                t_time = (int)(t_time * dpi);
                t_time_height = (int)(t_time_height * dpi);
                t_button = (int)(t_button * dpi);
                left_button = (int)(left_button * dpi);
                year_width = (int)(year_width * dpi);
                year2_width = (int)(year2_width * dpi);
                month_width = (int)(month_width * dpi);
            }

            if (_control.Presets.Count > 0)
            {
                left_buttons = new List<CalendarButton>(_control.Presets.Count);
                int y = 0;
                foreach (object it in _control.Presets)
                {
                    left_buttons.Add(new CalendarButton(y, it));
                    y++;
                }
                t_x = left_button;
            }
            if (ShowTime)
            {
                t_width = t_x + t_one_width + t_time * 3;
                button_text = Localization.Get("Now", "此刻");
            }
            else t_width = t_x + t_one_width;

            rect_lefts = new Rectangle(t_x + shadow, shadow, t_top, t_top);
            rect_left = new Rectangle(t_x + shadow + t_top, shadow, t_top, t_top);
            rect_rights = new Rectangle(t_x + t_one_width + shadow - t_top, shadow, t_top, t_top);
            rect_right = new Rectangle(t_x + t_one_width + shadow - t_top * 2, shadow, t_top, t_top);

            int gap = (int)(4 * dpi), t_width2 = t_one_width / 2;
            rect_year2 = new Rectangle(t_x + shadow + (t_one_width - year2_width) / 2, shadow, year2_width, t_top);
            if (YDR)
            {
                YearFormat = "yyyy";
                MonthFormat = "MMM";
                MondayButton = "Mon";
                TuesdayButton = "Tue";
                WednesdayButton = "Wed";
                ThursdayButton = "Thu";
                FridayButton = "Fri";
                SaturdayButton = "Sat";
                SundayButton = "Sun";

                rect_month = new Rectangle(t_x + shadow + t_width2 - year_width - gap, shadow, year_width, t_top);
                rect_year = new Rectangle(t_x + shadow + t_width2 + gap, shadow, month_width, t_top);
                s_f_L = Helper.SF(lr: StringAlignment.Near); s_f_R = Helper.SF(lr: StringAlignment.Far);
            }
            else
            {
                YearFormat = "yyyy年";
                MonthFormat = "MM月";
                MondayButton = "一";
                TuesdayButton = "二";
                WednesdayButton = "三";
                ThursdayButton = "四";
                FridayButton = "五";
                SaturdayButton = "六";
                SundayButton = "日";

                rect_year = new Rectangle(t_x + shadow + t_width2 - year_width - gap, shadow, year_width, t_top);
                rect_month = new Rectangle(t_x + shadow + t_width2 + gap, shadow, month_width, t_top);
                s_f_L = Helper.SF(lr: StringAlignment.Far); s_f_R = Helper.SF(lr: StringAlignment.Near);
            }

            Font = new Font(_control.Font.FontFamily, 11.2F);

            SelDate = date;
            Date = date ?? DateNow;

            var point = _control.PointToScreen(Point.Empty);
            int r_w = t_width + shadow2, r_h;
            if (calendar_day == null) r_h = 348 + shadow2;
            else r_h = (t_top + (ShowButtonToDay ? t_button : 0)) + (12 * 2) + (int)Math.Ceiling((calendar_day[calendar_day.Count - 1].y + 2) * (t_one_width - 16) / 7F) + shadow2;

            SetSize(r_w, r_h);
            rect_button = new Rectangle(t_x + shadow + (t_one_width - year_width) / 2, r_h - t_button - (shadow2 - 8), year_width, t_button);
            if (ShowTime)
            {
                int t_time_w = t_time * 3;
                rect_buttonok = new Rectangle(t_x + shadow + t_one_width, rect_button.Y, t_time_w, t_button);
            }
            CLocation(point, _control.Placement, _control.DropDownArrow, shadow, r_w, r_h, rect_read, ref Inverted, ref ArrowAlign);
            t_h = r_h;
            if (OS.Win7OrLower) Select();
        }

        public override string name => nameof(DatePicker);

        #endregion

        #region 属性

        #region 参数

        IControl control;

        bool ShowTime = false;
        float Radius = 6;
        int t_width = 288, t_h = 0, t_one_width = 288, t_x = 0, left_button = 120, t_top = 34, t_button = 38, t_time = 56, t_time_height = 30;
        int year_width = 60, year2_width = 90, month_width = 60;
        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;

        CultureInfo Culture;
        string CultureID = Localization.Get("ID", "zh-CN"),
            button_text = Localization.Get("ToDay", "今天"),
            OKButton = Localization.Get("OK", "确定"),
            YearFormat, MonthFormat,
            MondayButton, TuesdayButton, WednesdayButton, ThursdayButton, FridayButton, SaturdayButton, SundayButton;

        bool YDR = false;

        ScrollY scrollY_left, scrollY_h, scrollY_m, scrollY_s;
        List<CalendarButton>? left_buttons;

        /// <summary>
        /// 回调
        /// </summary>
        Action<DateTime> action;
        Action<object> action_btns;
        Func<DateTime[], List<DateBadge>?>? badge_action;
        Dictionary<string, DateBadge> badge_list = new Dictionary<string, DateBadge>();

        #endregion

        #region 日期

        public DateTime? SelDate;
        DateTime _Date;
        DateTime DateNow = DateTime.Now;
        List<Calendari>? calendar_year, calendar_month, calendar_day;
        List<CalendarT>? calendar_time;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;
                size_day = size_month = size_year = true;
                calendar_day = GetCalendar(value);

                #region 添加时间

                if (ShowTime && calendar_time == null)
                {
                    calendar_time = new List<CalendarT>(24 + 120);
                    for (int i = 0; i < 24; i++) calendar_time.Add(new CalendarT(0, 0, i, i));
                    for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(1, 1, i, i));
                    for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(2, 2, i, i));
                }

                #endregion

                #region 添加月

                var _calendar_month = new List<Calendari>(12);
                int x_m = 0, y_m = 0;
                for (int i = 0; i < 12; i++)
                {
                    var d_m = new DateTime(value.Year, i + 1, 1);
                    _calendar_month.Add(new Calendari(0, x_m, y_m, d_m.ToString(MonthFormat, Culture), d_m, d_m.ToString("yyyy-MM"), minDate, maxDate));
                    x_m++;
                    if (x_m > 2)
                    {
                        y_m++;
                        x_m = 0;
                    }
                }
                calendar_month = _calendar_month;

                #endregion

                #region 添加年

                int syear = value.Year - 1;
                if (!value.Year.ToString().EndsWith("0"))
                {
                    string temp = value.Year.ToString();
                    syear = int.Parse(temp.Substring(0, temp.Length - 1) + "0") - 1;
                }
                var _calendar_year = new List<Calendari>(12);
                int x_y = 0, y_y = 0;
                if (syear < 1) syear = 1;
                for (int i = 0; i < 12; i++)
                {
                    var d_y = new DateTime(syear + i, value.Month, 1);
                    _calendar_year.Add(new Calendari(i == 0 ? 0 : 1, x_y, y_y, d_y.ToString("yyyy"), d_y, d_y.ToString("yyyy"), minDate, maxDate));
                    x_y++;
                    if (x_y > 2)
                    {
                        y_y++;
                        x_y = 0;
                    }
                }
                year_str = _calendar_year[1].date_str + "-" + _calendar_year[_calendar_year.Count - 2].date_str;
                calendar_year = _calendar_year;

                #endregion

                if (badge_action != null)
                {
                    var oldval = value;
                    ITask.Run(() =>
                    {
                        var dir = badge_action(new DateTime[] { calendar_day[0].date, calendar_day[calendar_day.Count - 1].date });
                        if (_Date == oldval)
                        {
                            badge_list.Clear();
                            if (dir == null)
                            {
                                if (RunAnimation) DisposeTmp();
                                else Print();
                                return;
                            }
#if NET40 || NET46 || NET48
                            foreach (var it in dir) badge_list.Add(it.Date, it);
#else
                            foreach (var it in dir) badge_list.TryAdd(it.Date, it);
#endif
                            if (RunAnimation) DisposeTmp();
                            else Print();
                        }
                    });
                }

                hover_left.Enable = Helper.DateExceedMonth(value.AddMonths(-1), minDate, maxDate);
                hover_right.Enable = Helper.DateExceedMonth(value.AddMonths(1), minDate, maxDate);
                hover_lefts.Enable = Helper.DateExceedYear(value.AddYears(-1), minDate, maxDate);
                hover_rights.Enable = Helper.DateExceedYear(value.AddYears(1), minDate, maxDate);
            }
        }

        string year_str = "";
        List<Calendari> GetCalendar(DateTime now)
        {
            var calendaris = new List<Calendari>(28);
            int days = DateTime.DaysInMonth(now.Year, now.Month);
            var now1 = new DateTime(now.Year, now.Month, 1);
            int day_ = 0;
            switch (now1.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    day_ = 1;
                    break;
                case DayOfWeek.Wednesday:
                    day_ = 2;
                    break;
                case DayOfWeek.Thursday:
                    day_ = 3;
                    break;
                case DayOfWeek.Friday:
                    day_ = 4;
                    break;
                case DayOfWeek.Saturday:
                    day_ = 5;
                    break;
                case DayOfWeek.Sunday:
                    day_ = 6;
                    break;
            }
            if (day_ > 0)
            {
                var date1 = now.AddMonths(-1);
                int days2 = DateTime.DaysInMonth(date1.Year, date1.Month);
                for (int i = 0; i < day_; i++)
                {
                    int day3 = days2 - i;
                    calendaris.Insert(0, new Calendari(0, (day_ - 1) - i, 0, day3.ToString(), new DateTime(date1.Year, date1.Month, day3), minDate, maxDate));
                }
            }
            int x = day_, y = 0;
            for (int i = 0; i < days; i++)
            {
                int day = i + 1;
                calendaris.Add(new Calendari(1, x, y, day.ToString(), new DateTime(now.Year, now.Month, day), minDate, maxDate));
                x++;
                if (x > 6)
                {
                    y++;
                    x = 0;
                }
            }
            if (x < 7)
            {
                var date1 = now.AddMonths(1);
                int day2 = 0;
                for (int i = x; i < 7; i++)
                {
                    int day3 = day2 + 1;
                    calendaris.Add(new Calendari(2, x, y, day3.ToString(), new DateTime(date1.Year, date1.Month, day3), minDate, maxDate));
                    x++; day2++;
                }
                if (y < 5)
                {
                    y++;
                    for (int i = 0; i < 7; i++)
                    {
                        int day3 = day2 + 1;
                        calendaris.Add(new Calendari(2, i, y, day3.ToString(), new DateTime(date1.Year, date1.Month, day3), minDate, maxDate));
                        day2++;
                    }
                }
            }
            return calendaris;
        }

        #endregion

        #endregion

        #region 渲染

        StringFormat s_f = Helper.SF(), s_f_LE = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        StringFormat s_f_L, s_f_R;
        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(shadow, shadow, rect.Width - shadow2, rect.Height - shadow2);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path = rect_read.RoundPath(Radius))
                {
                    DrawShadow(g, rect);
                    using (var brush = new SolidBrush(Colour.BgElevated.Get("DatePicker", ColorScheme)))
                    {
                        g.Fill(brush, path);
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                    }
                }

                #region 方向

                using (var pen_arrow = new Pen(Colour.TextTertiary.Get("DatePicker", ColorScheme), 1.6F * Config.Dpi))
                using (var pen_arrow_hover = new Pen(Colour.Text.Get("DatePicker", ColorScheme), pen_arrow.Width))
                using (var pen_arrow_enable = new Pen(Colour.FillSecondary.Get("DatePicker", ColorScheme), pen_arrow.Width))
                {
                    if (hover_lefts.Animation)
                    {
                        using (var pen_arrow_hovers = new Pen(pen_arrow.Color.BlendColors(hover_lefts.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                        {
                            g.DrawLines(pen_arrow_hovers, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                            g.DrawLines(pen_arrow_hovers, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                        }
                    }
                    else if (hover_lefts.Switch)
                    {
                        g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                        g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                    }
                    else if (hover_lefts.Enable)
                    {
                        g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                        g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                    }
                    else
                    {
                        g.DrawLines(pen_arrow_enable, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                        g.DrawLines(pen_arrow_enable, TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F));
                    }

                    if (hover_rights.Animation)
                    {
                        using (var pen_arrow_hovers = new Pen(pen_arrow.Color.BlendColors(hover_rights.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                        {
                            g.DrawLines(pen_arrow_hovers, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                            g.DrawLines(pen_arrow_hovers, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                        }
                    }
                    else if (hover_rights.Switch)
                    {
                        g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                        g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                    }
                    else if (hover_rights.Enable)
                    {
                        g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                        g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                    }
                    else
                    {
                        g.DrawLines(pen_arrow_enable, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                        g.DrawLines(pen_arrow_enable, TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F));
                    }

                    if (ShowType == 0)
                    {
                        if (hover_left.Animation)
                        {
                            using (var pen_arrow_hovers = new Pen(pen_arrow.Color.BlendColors(hover_left.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                            {
                                g.DrawLines(pen_arrow_hovers, TAlignMini.Left.TriangleLines(rect_left, .26F));
                            }
                        }
                        else if (hover_left.Switch) g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(rect_left, .26F));
                        else if (hover_left.Enable) g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(rect_left, .26F));
                        else g.DrawLines(pen_arrow_enable, TAlignMini.Left.TriangleLines(rect_left, .26F));

                        if (hover_right.Animation)
                        {
                            using (var pen_arrow_hovers = new Pen(pen_arrow.Color.BlendColors(hover_right.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                            {
                                g.DrawLines(pen_arrow_hovers, TAlignMini.Right.TriangleLines(rect_right, .26F));
                            }
                        }
                        else if (hover_right.Switch) g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(rect_right, .26F));
                        else if (hover_right.Enable) g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(rect_right, .26F));
                        else g.DrawLines(pen_arrow_enable, TAlignMini.Right.TriangleLines(rect_right, .26F));
                    }
                }

                #endregion

                if (ShowType == TDatePicker.Month && calendar_month != null) PrintMonth(g, rect_read, calendar_month);
                else if (ShowType == TDatePicker.Year && calendar_year != null) PrintYear(g, rect_read, calendar_year);
                else if (calendar_day != null) PrintDay(g, rect_read, calendar_day);
            }
            return original_bmp;
        }

        #region 渲染帮助

        #region 年模式

        /// <summary>
        /// 渲染年模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintYear(Canvas g, Rectangle rect_read, List<Calendari> datas)
        {
            var color_fore = Colour.TextBase.Get("DatePicker", ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                Rectangle rect_l = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width, t_top);

                if (hover_year.Animation) g.String(year_str, font, color_fore.BlendColors(hover_year.Value, Colour.Primary.Get("DatePicker", ColorScheme)), rect_l, s_f);
                else if (hover_year.Switch) g.String(year_str, font, Colour.Primary.Get("DatePicker", ColorScheme), rect_l, s_f);
                else g.String(year_str, font, color_fore, rect_l, s_f);
            }
            if (size_year) LayoutYear(rect_read);
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get("DatePicker", ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get("DatePicker", ColorScheme)))
            using (var brush_fore = new SolidBrush(color_fore))
            {
                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        if (SelDate.HasValue && SelDate.Value.ToString("yyyy") == it.date_str)
                        {
                            g.Fill(Colour.Primary.Get("DatePicker", ColorScheme), path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get("DatePicker", ColorScheme), it.rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker", ColorScheme), path);
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), Config.Dpi, path);
                            g.String(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), Config.Dpi, path);
                            g.String(it.v, Font, brush_fore_disable, it.rect, s_f);
                        }
                    }
                }
            }
        }

        #endregion

        #region 月模式

        /// <summary>
        /// 渲染月模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintMonth(Canvas g, Rectangle rect_read, List<Calendari> datas)
        {
            var color_fore = Colour.TextBase.Get("DatePicker", ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                var rect_l = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width, t_top);
                string yearStr = _Date.ToString(YearFormat, Culture);
                if (hover_year.Animation) g.String(yearStr, font, color_fore.BlendColors(hover_year.Value, Colour.Primary.Get("DatePicker", ColorScheme)), rect_l, s_f);
                else if (hover_year.Switch) g.String(yearStr, font, Colour.Primary.Get("DatePicker", ColorScheme), rect_l, s_f);
                else g.String(yearStr, font, color_fore, rect_l, s_f);
            }
            if (size_month) LayoutMonth(rect_read);
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get("DatePicker", ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get("DatePicker", ColorScheme)))
            using (var brush_fore = new SolidBrush(color_fore))
            {
                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        if (SelDate.HasValue && SelDate.Value.ToString("yyyy-MM") == it.date_str)
                        {
                            g.Fill(Colour.Primary.Get("DatePicker", ColorScheme), path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get("DatePicker", ColorScheme), it.rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker", ColorScheme), path);
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), Config.Dpi, path);
                            g.String(it.v, Font, brush_fore, it.rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), Config.Dpi, path);
                            g.String(it.v, Font, brush_fore_disable, it.rect, s_f);
                        }
                    }
                }
            }
        }

        #endregion

        #region 天模式

        Rectangle rect_read_left, rect_read_h, rect_read_m, rect_read_s;
        /// <summary>
        /// 渲染天模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintDay(Canvas g, Rectangle rect_read, List<Calendari> datas)
        {
            var color_fore = Colour.TextBase.Get("DatePicker", ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture), monthStr = _Date.ToString(MonthFormat, Culture);
                if (hover_year.Animation) g.String(yearStr, font, color_fore.BlendColors(hover_year.Value, Colour.Primary.Get("DatePicker", ColorScheme)), rect_year, s_f_L);
                else if (hover_year.Switch) g.String(yearStr, font, Colour.Primary.Get("DatePicker", ColorScheme), rect_year, s_f_L);
                else g.String(yearStr, font, color_fore, rect_year, s_f_L);

                if (hover_month.Animation) g.String(monthStr, font, color_fore.BlendColors(hover_month.Value, Colour.Primary.Get("DatePicker", ColorScheme)), rect_month, s_f_R);
                else if (hover_month.Switch) g.String(monthStr, font, Colour.Primary.Get("DatePicker", ColorScheme), rect_month, s_f_R);
                else g.String(monthStr, font, color_fore, rect_month, s_f_R);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get("DatePicker", ColorScheme)))
            {
                g.Fill(brush_split, new Rectangle(t_x + rect_read.X, rect_read.Y + t_top, t_one_width, 1));
                if (ShowButtonToDay) g.Fill(brush_split, new Rectangle(t_x + rect_read.X, rect_button.Y, rect_read.Width - t_x, 1));
                else g.Fill(brush_split, new Rectangle(t_x + rect_read.X + t_one_width, rect_button.Y, rect_read.Width - t_x - t_one_width, 1));
                if (ShowTime) g.Fill(brush_split, new Rectangle(t_x + rect_read.X + t_one_width, rect_read.Y, 1, rect_read.Height));
                if (left_buttons != null) g.Fill(brush_split, new Rectangle(t_x + rect_read.X, rect_read.Y, 1, rect_read.Height));
            }
            int size = (t_one_width - 16) / 7, y = rect_read.Y + t_top + 12;
            using (var brush = new SolidBrush(Colour.Text.Get("DatePicker", ColorScheme)))
            {
                g.String(MondayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8, y, size, size), s_f);
                g.String(TuesdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size, y, size, size), s_f);
                g.String(WednesdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 2, y, size, size), s_f);
                g.String(ThursdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 3, y, size, size), s_f);
                g.String(FridayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 4, y, size, size), s_f);
                g.String(SaturdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 5, y, size, size), s_f);
                g.String(SundayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 6, y, size, size), s_f);
            }
            if (size_day) LayoutDate(rect_read);
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get("DatePicker", ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get("DatePicker", ColorScheme)))
            using (var brush_active = new SolidBrush(Colour.Primary.Get("DatePicker", ColorScheme)))
            using (var brush_active_fore = new SolidBrush(Colour.PrimaryColor.Get("DatePicker", ColorScheme)))
            using (var brush_error = new SolidBrush(Colour.Error.Get("DatePicker", ColorScheme)))
            {
                foreach (var it in datas)
                {
                    if (DateNow.ToString("yyyy-MM-dd") == it.date_str)
                    {
                        using (var path = it.rect_read.RoundPath(Radius))
                        {
                            g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), Config.Dpi, path);
                        }
                    }
                }

                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        if (SelDate.HasValue && SelDate.Value.ToString("yyyy-MM-dd") == it.date_str)
                        {
                            g.Fill(brush_active, path);
                            g.String(it.v, Font, brush_active_fore, it.rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker", ColorScheme), path);
                            g.String(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                            g.String(it.v, Font, brush_fore_disable, it.rect, s_f);
                        }
                    }
                }
                if (rect_read.Height > t_button)
                {
                    if (left_buttons != null)
                    {
                        var state = g.Save();
                        g.SetClip(new Rectangle(rect_read.X, rect_read.Y, left_button, rect_read.Height));
                        g.TranslateTransform(rect_read.X, rect_read.Y - scrollY_left.Value);
                        foreach (var it in left_buttons)
                        {
                            using (var path = it.rect_read.RoundPath(Radius))
                            {
                                if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker", ColorScheme), path);
                                g.String(it.v, Font, brush_fore, it.rect_text, s_f_LE);
                            }
                        }
                        g.Restore(state);
                        scrollY_left.Paint(g);
                    }
                    if (calendar_time != null)
                    {
                        var state = g.Save();
                        int tx = t_x + rect_read.X + t_one_width;
                        g.SetClip(new Rectangle(tx, rect_read.Y, t_time * 3, rect_button.Y - shadow));
                        using (var brush_bg = new SolidBrush(Colour.PrimaryBg.Get("DatePicker", ColorScheme)))
                        using (var brush_hove = new SolidBrush(Colour.FillTertiary.Get("DatePicker", ColorScheme)))
                        {
                            g.TranslateTransform(tx, shadow - scrollY_h.Value);
                            for (int i = 0; i < calendar_time.Count; i++)
                            {
                                if (i == 24)
                                {
                                    g.ResetTransform();
                                    g.TranslateTransform(tx, shadow - scrollY_m.Value);
                                }
                                else if (i == 84)
                                {
                                    g.ResetTransform();
                                    g.TranslateTransform(tx, shadow - scrollY_s.Value);
                                }
                                var it = calendar_time[i];
                                bool hashover = false;
                                if (SelDate.HasValue)
                                {
                                    switch (it.rx)
                                    {
                                        case 0:
                                            if (it.t == SelDate.Value.Hour) hashover = true;
                                            break;
                                        case 1:
                                            if (it.t == SelDate.Value.Minute) hashover = true;
                                            break;
                                        case 2:
                                            if (it.t == SelDate.Value.Second) hashover = true;
                                            break;
                                    }
                                }
                                if (hashover || it.hover)
                                {
                                    using (var path = it.rect_read.RoundPath(Radius))
                                    {
                                        if (hashover) g.Fill(brush_bg, path);
                                        if (it.hover) g.Fill(brush_hove, path);
                                    }
                                }
                                g.String(it.v, Font, brush_fore, it.rect_read, s_f);
                            }
                        }
                        g.Restore(state);
                        scrollY_h.Paint(g);
                        scrollY_m.Paint(g);
                        scrollY_s.Paint(g);

                        if (hover_buttonok.Animation) g.String(OKButton, Font, brush_active.Color.BlendColors(hover_buttonok.Value, Colour.PrimaryActive.Get("DatePicker", ColorScheme)), rect_buttonok, s_f);
                        else if (hover_buttonok.Switch) g.String(OKButton, Font, Colour.PrimaryActive.Get("DatePicker", ColorScheme), rect_buttonok, s_f);
                        else g.String(OKButton, Font, brush_active, rect_buttonok, s_f);
                    }
                }

                if (ShowButtonToDay)
                {
                    if (hover_button.Animation) g.String(button_text, Font, brush_active.Color.BlendColors(hover_button.Value, Colour.PrimaryActive.Get("DatePicker", ColorScheme)), rect_button, s_f);
                    else if (hover_button.Switch) g.String(button_text, Font, Colour.PrimaryActive.Get("DatePicker", ColorScheme), rect_button, s_f);
                    else g.String(button_text, Font, brush_active, rect_button, s_f);
                }

                if (badge_list.Count > 0)
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, it.rect, g);
                    }
                }
            }
        }

        #endregion

        #endregion

        SafeBitmap? shadow_temp;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">客户区域</param>
        void DrawShadow(Canvas g, Rectangle rect)
        {
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null)
                {
                    shadow_temp?.Dispose();
                    using (var path = new Rectangle(shadow, shadow, rect.Width - shadow2, rect.Height - shadow2).RoundPath(Radius))
                    {
                        shadow_temp = path.PaintShadow(rect.Width, rect.Height);
                    }
                }
                g.Image(shadow_temp.Bitmap, rect, .2F);
            }
        }

        #endregion

        #region 布局

        TDatePicker PickerType, showType;
        TDatePicker ShowType
        {
            get => showType;
            set
            {
                if (showType == value) return;
                showType = value;
                LoadLayout();
            }
        }

        bool size_day = true, size_month = true, size_year = true;

        void LoadLayout()
        {
            if (left_buttons != null)
            {
                t_x = ShowType == 0 ? left_button : 0;

                rect_lefts = new Rectangle(t_x + shadow, shadow, t_top, t_top);
                rect_left = new Rectangle(t_x + shadow + t_top, shadow, t_top, t_top);
                rect_rights = new Rectangle(t_x + t_one_width + shadow - t_top, shadow, t_top, t_top);
                rect_right = new Rectangle(t_x + t_one_width + shadow - t_top * 2, shadow, t_top, t_top);

                int gap = (int)(4 * Config.Dpi), t_width2 = t_one_width / 2;
                rect_year2 = new Rectangle(t_x + shadow + (t_one_width - year2_width) / 2, shadow, year2_width, t_top);
                if (YDR)
                {
                    rect_month = new Rectangle(t_x + shadow + t_width2 - year_width - gap, shadow, year_width, t_top);
                    rect_year = new Rectangle(t_x + shadow + t_width2 + gap, shadow, month_width, t_top);
                }
                else
                {
                    rect_year = new Rectangle(t_x + shadow + t_width2 - year_width - gap, shadow, year_width, t_top);
                    rect_month = new Rectangle(t_x + shadow + t_width2 + gap, shadow, month_width, t_top);
                }
            }
            if (showType == TDatePicker.Date)
            {
                SetSize(t_width + shadow2, t_h);
                var rect_read = new Rectangle(shadow, shadow, t_width, t_h - shadow2);
                LayoutDate(rect_read);
            }
            else
            {
                SetSize(t_one_width + shadow2, t_h);
                var rect_read = new Rectangle(shadow, shadow, t_one_width, t_h - shadow2);
                switch (showType)
                {
                    case TDatePicker.Year:
                        LayoutYear(rect_read);
                        break;
                    case TDatePicker.Month:
                        LayoutMonth(rect_read);
                        break;
                }
            }
        }

        void LayoutYear(Rectangle rect)
        {
            if (calendar_year == null) return;
            int size_w = (rect.Width - 16) / 3, size_h = (rect.Width - 16) / 7 * 2, y = rect.Y + t_top;
            size_year = false;
            foreach (var it in calendar_year) it.rect = new Rectangle(rect.X + 8 + (size_w * it.x), y + (size_h * it.y), size_w, size_h);
        }
        void LayoutMonth(Rectangle rect)
        {
            if (calendar_month == null) return;
            int size_w = (rect.Width - 16) / 3, size_h = (rect.Width - 16) / 7 * 2, y = rect.Y + t_top;
            size_month = false;
            foreach (var it in calendar_month) it.rect = new Rectangle(rect.X + 8 + (size_w * it.x), y + (size_h * it.y), size_w, size_h);
        }
        void LayoutDate(Rectangle rect)
        {
            if (calendar_day == null) return;
            int size = (t_one_width - 16) / 7, y = rect.Y + t_top + 12 + size;

            size_day = false;
            int size_one = (int)(size * .666F);
            foreach (var it in calendar_day) it.SetRect(new Rectangle(t_x + rect.X + 8 + (size * it.x), y + (size * it.y), size, size), size_one);
            if (calendar_time != null)
            {
                int size_time_one = (int)(t_time * .857F);
                int size_time_height_one = (int)(t_time_height * .93F);

                int endh = rect_button.Y;
                var rect_s_h = new Rectangle(t_x, rect.Y + 8, rect.X + t_one_width + t_time, endh - rect.Y - 8);
                rect_read_h = new Rectangle(rect_s_h.Right - t_time, 0, t_time, endh);
                rect_read_m = new Rectangle(rect_s_h.Right, 0, t_time, endh);
                rect_read_s = new Rectangle(rect_s_h.Right + t_time, 0, t_time, endh);
                scrollY_h.SizeChange(rect_s_h);
                rect_s_h.Width += t_time;
                scrollY_m.SizeChange(rect_s_h);
                rect_s_h.Width += t_time;
                scrollY_s.SizeChange(rect_s_h);

                int endh2 = endh - rect.Y * 2 - (t_time_height - size_time_height_one);
                if (ValueTimeHorizontal)
                {
                    int exceed = shadow;
                    scrollY_h.SetVrSize(t_time_height * (24 + exceed), endh2);
                    scrollY_m.SetVrSize(t_time_height * (60 + exceed), endh2);
                    scrollY_s.SetVrSize(t_time_height * (60 + exceed), endh2);
                }
                else
                {
                    scrollY_h.SetVrSize(t_time_height * 24, endh2);
                    scrollY_m.SetVrSize(t_time_height * 60, endh2);
                    scrollY_s.SetVrSize(t_time_height * 60, endh2);
                }

                int _x = (t_time - size_time_one) / 2, _y = rect.Y + (t_time_height - size_time_height_one) / 2;
                foreach (var it in calendar_time)
                {
                    var rect_n = new Rectangle(t_time * it.x, t_time_height * it.y, t_time, t_time_height);
                    it.rect = new Rectangle(t_x + rect.X + t_one_width + rect_n.X, rect.Y + rect_n.Y, rect_n.Width, rect_n.Height);
                    it.rect_read = new Rectangle(rect_n.X + _x, rect_n.Y + _y, size_time_one, size_time_height_one);
                }

                if (SelDate.HasValue) ScrollYTime(calendar_time, SelDate.Value);
            }
            if (left_buttons != null)
            {
                int btn_one = (int)(left_button * .9F), btn_height_one = (int)(t_time_height * .93F), btn_one2 = (int)(left_button * .8F);

                rect_read_left = new Rectangle(rect.X, rect.Y, t_x, t_h - rect.Y * 2);

                scrollY_left.SizeChange(new Rectangle(rect.X, rect.Y + 8, t_x, t_h - (8 + rect.Y) * 2));
                scrollY_left.SetVrSize(t_time_height * left_buttons.Count, t_h - shadow2 - rect.Y * 2);

                int _x = (left_button - btn_one) / 2, _x2 = (btn_one - btn_one2) / 2, _y = rect.Y + (t_time_height - btn_height_one) / 2;
                foreach (var it in left_buttons)
                {
                    var rect_n = new Rectangle(0, t_time_height * it.y, left_button, t_time_height);
                    it.rect_read = new Rectangle(rect_n.X + _x, rect_n.Y + _y, btn_one, btn_height_one);
                    it.rect = new Rectangle(rect.X + rect_n.X, rect.Y + rect_n.Y, rect_n.Width, rect_n.Height);

                    it.rect_text = new Rectangle(rect.X + _x2, it.rect_read.Y, btn_one2, it.rect_read.Height);
                }
            }
        }

        #endregion

        #region 鼠标

        ITaskOpacity hover_button, hover_buttonok, hover_lefts, hover_left, hover_rights, hover_right, hover_year, hover_month;
        Rectangle rect_button = new Rectangle(-20, -20, 10, 10), rect_buttonok = new Rectangle(-20, -20, 10, 10);
        Rectangle rect_lefts = new Rectangle(-20, -20, 10, 10), rect_left = new Rectangle(-20, -20, 10, 10);
        Rectangle rect_rights = new Rectangle(-20, -20, 10, 10), rect_right = new Rectangle(-20, -20, 10, 10);
        Rectangle rect_year = new Rectangle(-20, -20, 10, 10), rect_year2 = new Rectangle(-20, -20, 10, 10), rect_month = new Rectangle(-20, -20, 10, 10);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (RunAnimation) return;
            base.OnMouseDown(e);
            if (left_buttons != null && rect_read_left.Contains(e.X, e.Y)) if (!scrollY_left.MouseDown(e.Location)) return;
            if (ShowTime)
            {
                if (rect_read_h.Contains(e.X, e.Y)) scrollY_h.MouseDown(e.Location);
                else if (rect_read_m.Contains(e.X, e.Y)) scrollY_m.MouseDown(e.Location);
                else if (rect_read_s.Contains(e.X, e.Y)) scrollY_s.MouseDown(e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RunAnimation) return;
            if (scrollY_left.MouseMove(e.Location) && scrollY_h.MouseMove(e.Location) && scrollY_m.MouseMove(e.Location) && scrollY_s.MouseMove(e.Location))
            {
                int count = 0, hand = 0;
                bool _hover_lefts = rect_lefts.Contains(e.X, e.Y),
                 _hover_rights = rect_rights.Contains(e.X, e.Y),
                 _hover_left = (ShowType == 0 && rect_left.Contains(e.X, e.Y)),
                 _hover_right = (ShowType == 0 && rect_right.Contains(e.X, e.Y)),
                 _hover_button = (ShowType == 0 && ShowButtonToDay && rect_button.Contains(e.X, e.Y)),
                 _hover_buttonok = (ShowType == 0 && ShowTime && rect_buttonok.Contains(e.X, e.Y));

                bool _hover_year = false, _hover_month = false;
                if (ShowType != TDatePicker.Year)
                {
                    _hover_year = ShowType == 0 ? rect_year.Contains(e.X, e.Y) : rect_year2.Contains(e.X, e.Y);
                    _hover_month = rect_month.Contains(e.X, e.Y);
                }

                if (_hover_lefts != hover_lefts.Switch) count++;
                if (_hover_left != hover_left.Switch) count++;
                if (_hover_rights != hover_rights.Switch) count++;
                if (_hover_right != hover_right.Switch) count++;

                if (_hover_year != hover_year.Switch) count++;
                if (_hover_month != hover_month.Switch) count++;
                if (_hover_button != hover_button.Switch) count++;
                if (_hover_buttonok != hover_buttonok.Switch) count++;

                hover_lefts.Switch = _hover_lefts;
                hover_left.Switch = _hover_left;
                hover_rights.Switch = _hover_rights;
                hover_right.Switch = _hover_right;
                hover_year.Switch = _hover_year;
                hover_month.Switch = _hover_month;
                hover_button.Switch = _hover_button;
                hover_buttonok.Switch = _hover_buttonok;
                if (hover_lefts.Switch || hover_left.Switch || hover_rights.Switch || hover_right.Switch || hover_year.Switch || hover_month.Switch || hover_button.Switch || hover_buttonok.Switch) hand++;
                else
                {
                    if (ShowType == TDatePicker.Month)
                    {
                        if (calendar_month != null)
                        {
                            foreach (var it in calendar_month)
                            {
                                bool hove = it.enable && it.rect.Contains(e.X, e.Y);
                                if (it.hover != hove) count++;
                                it.hover = hove;
                                if (it.hover) hand++;
                            }
                        }
                    }
                    else if (ShowType == TDatePicker.Year)
                    {
                        if (calendar_year != null)
                        {
                            foreach (var it in calendar_year)
                            {
                                bool hove = it.enable && it.rect.Contains(e.X, e.Y);
                                if (it.hover != hove) count++;
                                it.hover = hove;
                                if (it.hover) hand++;
                            }
                        }
                    }
                    else
                    {
                        if (calendar_day != null)
                        {
                            foreach (var it in calendar_day)
                            {
                                bool hove = it.enable && it.rect.Contains(e.X, e.Y);
                                if (it.hover != hove) count++;
                                it.hover = hove;
                                if (it.hover) hand++;
                            }
                        }
                        if (left_buttons != null)
                        {
                            foreach (var it in left_buttons)
                            {
                                if (it.Contains(e.X, e.Y, 0, scrollY_left.Value, out var change)) hand++;
                                if (change) count++;
                            }
                        }
                        if (calendar_time != null)
                        {
                            foreach (var it in calendar_time)
                            {
                                switch (it.rx)
                                {
                                    case 1:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_m.Value, out var change1)) hand++;
                                        if (change1) count++;
                                        break;
                                    case 2:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_s.Value, out var change2)) hand++;
                                        if (change2) count++;
                                        break;
                                    case 0:
                                    default:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_h.Value, out var change0)) hand++;
                                        if (change0) count++;
                                        break;
                                }
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
            if (RunAnimation) return;
            scrollY_left.Leave();
            scrollY_h.Leave();
            scrollY_m.Leave();
            scrollY_s.Leave();
            hover_lefts.Switch = false;
            hover_left.Switch = false;
            hover_rights.Switch = false;
            hover_right.Switch = false;
            hover_year.Switch = false;
            hover_month.Switch = false;
            hover_button.Switch = false;
            hover_buttonok.Switch = false;
            if (calendar_year != null)
            {
                foreach (var it in calendar_year) it.hover = false;
            }
            if (calendar_month != null)
            {
                foreach (var it in calendar_month) it.hover = false;
            }
            if (calendar_day != null)
            {
                foreach (var it in calendar_day) it.hover = false;
            }
            if (calendar_time != null)
            {
                foreach (var it in calendar_time) it.hover = false;
            }
            SetCursor(false);
            Print();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (RunAnimation) return;
            scrollY_left.MouseUp(e.Location);
            scrollY_h.MouseUp(e.Location);
            scrollY_m.MouseUp(e.Location);
            scrollY_s.MouseUp(e.Location);
            if (e.Button == MouseButtons.Left)
            {
                if (rect_lefts.Contains(e.X, e.Y))
                {
                    if (hover_lefts.Enable)
                    {
                        if (ShowType == TDatePicker.Year) Date = _Date.AddYears(-10);
                        else Date = _Date.AddYears(-1);
                        Print();
                    }
                    return;
                }
                else if (rect_rights.Contains(e.X, e.Y))
                {
                    if (hover_rights.Enable)
                    {
                        if (ShowType == TDatePicker.Year) Date = _Date.AddYears(10);
                        else Date = _Date.AddYears(1);
                        Print();
                    }
                    return;
                }
                else if (ShowType == TDatePicker.Date && rect_left.Contains(e.X, e.Y))
                {
                    if (hover_left.Enable)
                    {
                        Date = _Date.AddMonths(-1);
                        Print();
                    }
                    return;
                }
                else if (ShowType == TDatePicker.Date && rect_right.Contains(e.X, e.Y))
                {
                    if (hover_right.Enable)
                    {
                        Date = _Date.AddMonths(1);
                        Print();
                    }
                    return;
                }
                else if ((ShowType == TDatePicker.Date && rect_year.Contains(e.X, e.Y)) || (ShowType != 0 && rect_year2.Contains(e.X, e.Y)))
                {
                    ShowType = TDatePicker.Year;
                    Print();
                    return;
                }
                else if (ShowType == TDatePicker.Date && ShowButtonToDay && rect_button.Contains(e.X, e.Y))
                {
                    SelDate = Date = DateNow = DateTime.Now;
                    action(SelDate.Value);
                    if (ShowTime && calendar_time != null)
                    {
                        ScrollYTime(calendar_time, SelDate.Value);
                        Print();
                    }
                    else IClose();
                    return;
                }
                else if (ShowType == TDatePicker.Date && ShowTime && rect_buttonok.Contains(e.X, e.Y))
                {
                    if (SelDate.HasValue)
                    {
                        action(SelDate.Value);
                        IClose();
                    }
                    return;
                }
                else if (rect_month.Contains(e.X, e.Y))
                {
                    ShowType = TDatePicker.Month;
                    Print();
                    return;
                }
                else
                {
                    if (ShowType == TDatePicker.Month)
                    {
                        if (calendar_month != null)
                        {
                            foreach (var it in calendar_month)
                            {
                                if (it.enable && it.rect.Contains(e.X, e.Y))
                                {
                                    Date = it.date;
                                    if (PickerType < ShowType) ShowType = TDatePicker.Date;
                                    else
                                    {
                                        SelDate = it.date;
                                        action(SelDate.Value);
                                        IClose();
                                        return;
                                    }
                                    Print();
                                    return;
                                }
                            }
                        }
                    }
                    else if (ShowType == TDatePicker.Year)
                    {
                        if (calendar_year != null)
                        {
                            foreach (var it in calendar_year)
                            {
                                if (it.enable && it.rect.Contains(e.X, e.Y))
                                {
                                    Date = it.date;
                                    if (PickerType < ShowType) ShowType = TDatePicker.Month;
                                    else
                                    {
                                        SelDate = it.date;
                                        action(SelDate.Value);
                                        IClose();
                                        return;
                                    }
                                    Print();
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (calendar_day != null)
                        {
                            foreach (var it in calendar_day)
                            {
                                if (it.enable && it.rect.Contains(e.X, e.Y))
                                {
                                    if (ShowTime)
                                    {
                                        if (SelDate.HasValue) SelDate = new DateTime(it.date.Year, it.date.Month, it.date.Day, SelDate.Value.Hour, SelDate.Value.Minute, SelDate.Value.Second);
                                        else
                                        {
                                            var DateNow = DateTime.Now;
                                            SelDate = new DateTime(it.date.Year, it.date.Month, it.date.Day, DateNow.Hour, DateNow.Minute, DateNow.Second);
                                        }
                                        action(SelDate.Value);
                                        if (calendar_time != null) ScrollYTime(calendar_time, SelDate.Value);
                                        Print();
                                        return;
                                    }
                                    else SelDate = it.date;
                                    action(SelDate.Value);
                                    IClose();
                                    return;
                                }
                            }
                        }
                        if (left_buttons != null)
                        {
                            foreach (var it in left_buttons)
                            {
                                if (it.Contains(e.X, e.Y, 0, scrollY_left.Value, out _))
                                {
                                    action_btns(it.Tag);
                                    IClose();
                                    return;
                                }
                            }
                        }

                        if (calendar_time != null)
                        {
                            foreach (var it in calendar_time)
                            {
                                switch (it.rx)
                                {
                                    case 1:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_m.Value, out _))
                                        {
                                            if (SelDate.HasValue) SelDate = new DateTime(SelDate.Value.Year, SelDate.Value.Month, SelDate.Value.Day, SelDate.Value.Hour, it.t, SelDate.Value.Second);
                                            if (ValueTimeHorizontal && SelDate.HasValue) ScrollYTime(calendar_time, SelDate.Value);
                                            Print();
                                            return;
                                        }
                                        break;
                                    case 2:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_s.Value, out _))
                                        {
                                            if (SelDate.HasValue) SelDate = new DateTime(SelDate.Value.Year, SelDate.Value.Month, SelDate.Value.Day, SelDate.Value.Hour, SelDate.Value.Minute, it.t);
                                            if (ValueTimeHorizontal && SelDate.HasValue) ScrollYTime(calendar_time, SelDate.Value);
                                            Print();
                                            return;
                                        }
                                        break;
                                    case 0:
                                    default:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_h.Value, out _))
                                        {
                                            if (SelDate.HasValue) SelDate = new DateTime(SelDate.Value.Year, SelDate.Value.Month, SelDate.Value.Day, it.t, SelDate.Value.Minute, SelDate.Value.Second);
                                            if (ValueTimeHorizontal && SelDate.HasValue) ScrollYTime(calendar_time, SelDate.Value);
                                            Print();
                                            return;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (RunAnimation) return;
            if (e.Delta != 0)
            {
                if (left_buttons != null && rect_read_left.Contains(e.X, e.Y))
                {
                    scrollY_left.MouseWheel(e.Delta);
                    Print();
                    base.OnMouseWheel(e);
                    return;
                }
                if (ShowTime)
                {
                    if (rect_read_h.Contains(e.X, e.Y))
                    {
                        scrollY_h.MouseWheel(e.Delta);
                        Print();
                    }
                    else if (rect_read_m.Contains(e.X, e.Y))
                    {
                        scrollY_m.MouseWheel(e.Delta);
                        Print();
                    }
                    else if (rect_read_s.Contains(e.X, e.Y))
                    {
                        scrollY_s.MouseWheel(e.Delta);
                        Print();
                    }
                    else MouseWheelDay(e);
                }
                else MouseWheelDay(e);
            }
            base.OnMouseWheel(e);
        }

        void MouseWheelDay(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (ShowType == TDatePicker.Month)
                {
                    if (hover_lefts.Enable) Date = _Date.AddYears(-1);
                    else return;
                }
                else if (ShowType == TDatePicker.Year)
                {
                    if (hover_lefts.Enable) Date = _Date.AddYears(-10);
                    else return;
                }
                else
                {
                    if (hover_left.Enable) Date = _Date.AddMonths(-1);
                    else return;
                }
                Print();
            }
            else
            {
                if (ShowType == TDatePicker.Month)
                {
                    if (hover_rights.Enable) Date = _Date.AddYears(1);
                    else return;
                }
                else if (ShowType == TDatePicker.Year)
                {
                    if (hover_rights.Enable) Date = _Date.AddYears(10);
                    else return;
                }
                else
                {
                    if (hover_right.Enable) Date = _Date.AddMonths(1);
                    else return;
                }
                Print();
            }
        }

        #endregion

        void ScrollYTime(List<CalendarT> calendar_time, DateTime d)
        {
            CalendarT? find_h = calendar_time.Find(a => a.rx == 0 && a.t == d.Hour),
                find_m = calendar_time.Find(a => a.rx == 1 && a.t == d.Minute),
                find_s = calendar_time.Find(a => a.rx == 2 && a.t == d.Second);

            int start = 4;
            if (find_h != null) scrollY_h.Value = find_h.rect.Y - start;
            if (find_m != null) scrollY_m.Value = find_m.rect.Y - start;
            if (find_s != null) scrollY_s.Value = find_s.rect.Y - start;
        }

        protected override void Dispose(bool disposing)
        {
            hover_lefts?.Dispose(); hover_left?.Dispose(); hover_rights?.Dispose(); hover_right?.Dispose(); hover_year?.Dispose(); hover_month?.Dispose(); hover_button?.Dispose(); hover_buttonok?.Dispose();
            base.Dispose(disposing);
        }
    }

    internal class Calendari
    {
        public Calendari(int _t, int _x, int _y, string _v, DateTime _date, string str, DateTime? min, DateTime? max)
        {
            t = _t;
            x = _x;
            y = _y;
            v = _v;
            date = _date;
            date_str = str;
            enable = Helper.DateExceedRelax(_date, min, max);
        }
        public Calendari(int _t, int _x, int _y, string _v, DateTime _date, DateTime? min, DateTime? max)
        {
            t = _t;
            x = _x;
            y = _y;
            v = _v;
            date = _date;
            date_str = _date.ToString("yyyy-MM-dd");
            enable = Helper.DateExceed(_date, min, max);
        }

        public bool hover { get; set; }

        Rectangle _rect;
        public Rectangle rect
        {
            get => _rect;
            set
            {
                rect_read = new Rectangle(value.X + 4, value.Y + 4, value.Width - 8, value.Height - 8);
                _rect = value;
            }
        }

        internal void SetRect(Rectangle value, int gap)
        {
            int xy = (value.Width - gap) / 2;
            rect_read = new Rectangle(value.X + xy, value.Y + xy, gap, gap);
            _rect = value;
        }

        internal void SetRectG(Rectangle value, float gap)
        {
            int w = (int)(value.Width * gap), h = (int)(value.Height * gap);
            rect_read = new Rectangle(value.X + (value.Width - w) / 2, value.Y + (value.Height - h) / 2, w, h);
            _rect = value;
        }

        public Rectangle rect_read;
        public Rectangle rect_f;
        public Rectangle rect_l;
        public int x { get; set; }
        public int y { get; set; }
        public int t { get; set; }
        public string v { get; set; }
        public DateTime date { get; set; }
        public string date_str { get; set; }
        public bool enable { get; set; }
    }
    internal class CalendarT
    {
        public CalendarT(int _x, int _rx, int _y, int _t)
        {
            x = _x;
            rx = _rx;
            y = _y;
            t = _t;
            v = _t.ToString().PadLeft(2, '0');
        }

        public bool hover { get; set; }
        public Rectangle rect { get; set; }
        public Rectangle rect_read { get; set; }

        internal bool Contains(int x, int y, float sx, float sy, out bool change)
        {
            if (rect.Contains(x + (int)sx, y + (int)sy))
            {
                change = SetHover(true);
                return true;
            }
            else
            {
                change = SetHover(false);
                return false;
            }
        }
        internal bool SetHover(bool val)
        {
            bool change = false;
            if (val)
            {
                if (!hover) change = true;
                hover = true;
            }
            else
            {
                if (hover) change = true;
                hover = false;
            }
            return change;
        }

        public int x { get; set; }
        public int rx { get; set; }
        public int y { get; set; }
        public int t { get; set; }
        public string v { get; set; }
    }
    internal class CalendarButton
    {
        public CalendarButton(int _y, object _v)
        {
            y = _y;
            v = _v.ToString();
            Tag = _v;
        }
        public bool hover { get; set; }
        public Rectangle rect { get; set; }
        public Rectangle rect_read { get; set; }
        public Rectangle rect_text { get; set; }

        internal bool Contains(int x, int y, float sx, float sy, out bool change)
        {
            if (rect.Contains(x + (int)sx, y + (int)sy))
            {
                change = SetHover(true);
                return true;
            }
            else
            {
                change = SetHover(false);
                return false;
            }
        }
        internal bool SetHover(bool val)
        {
            bool change = false;
            if (val)
            {
                if (!hover) change = true;
                hover = true;
            }
            else
            {
                if (hover) change = true;
                hover = false;
            }
            return change;
        }

        public int y { get; set; }
        public string? v { get; set; }
        public object Tag { get; set; }
    }
}