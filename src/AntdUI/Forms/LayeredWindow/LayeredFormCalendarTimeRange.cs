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
    public class LayeredFormCalendarTimeRange : ILayeredFormOpacityDown
    {
        DateTime? minDate, maxDate;
        bool ValueTimeHorizontal = false;
        public LayeredFormCalendarTimeRange(DatePickerRange _control, Rectangle rect_read, DateTime[]? date, Action<DateTime[]> _action, Action<object> _action_btns, Func<DateTime[], List<DateBadge>?>? _badge_action = null)
        {
            _control.Parent.SetTopMost(Handle);
            control = _control;
            ValueTimeHorizontal = _control.ValueTimeHorizontal;
            minDate = _control.MinDate;
            maxDate = _control.MaxDate;
            badge_action = _badge_action;
            PARENT = _control;
            action = _action;
            action_btns = _action_btns;
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

            Culture = new CultureInfo(CultureID);
            YDR = CultureID.StartsWith("en");

            float dpi = Config.Dpi;
            if (dpi != 1F)
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
            else Radius = _control.Radius;

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
            t_width = t_x + t_one_width + t_time * 3; button_text = Localization.Get("Now", "此刻");

            rect_lefts = new Rectangle(t_x + 10, 10, t_top, t_top);
            rect_left = new Rectangle(t_x + 10 + t_top, 10, t_top, t_top);
            rect_rights = new Rectangle(t_x + t_one_width + 10 - t_top, 10, t_top, t_top);
            rect_right = new Rectangle(t_x + t_one_width + 10 - t_top * 2, 10, t_top, t_top);

            int gap = (int)(4 * dpi), t_width2 = t_one_width / 2;
            rect_year2 = new Rectangle(t_x + 10 + (t_one_width - year2_width) / 2, 10, year2_width, t_top);
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

                rect_month = new Rectangle(t_x + 10 + t_width2 - year_width - gap, 10, year_width, t_top);
                rect_year = new Rectangle(t_x + 10 + t_width2 + gap, 10, month_width, t_top);

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

                rect_year = new Rectangle(t_x + 10 + t_width2 - year_width - gap, 10, year_width, t_top);
                rect_month = new Rectangle(t_x + 10 + t_width2 + gap, 10, month_width, t_top);

                s_f_L = Helper.SF(lr: StringAlignment.Far); s_f_R = Helper.SF(lr: StringAlignment.Near);
            }

            Font = new Font(_control.Font.FontFamily, 11.2F);

            SelTime = date;
            Date = date == null ? DateNow : date[0];

            var point = _control.PointToScreen(Point.Empty);
            int r_w = t_width + 20, r_h;
            if (calendar_day == null) r_h = 348 + 20;
            else r_h = (t_top + t_button) + (12 * 2) + (int)Math.Ceiling((calendar_day[calendar_day.Count - 1].y + 2) * (t_one_width - 16) / 7F) + 20;
            SetSize(r_w, r_h);
            t_h = r_h;
            rect_button = new Rectangle(t_x + 10 + (t_one_width - year_width) / 2, r_h - t_button - (20 - 8), year_width, t_button);

            int t_time_w = t_time * 3;
            rect_buttonok = new Rectangle(t_x + 10 + t_one_width, rect_button.Y, t_time_w, t_button);
            CLocation(point, _control.Placement, _control.DropDownArrow, 10, r_w, r_h, rect_read, ref Inverted, ref ArrowAlign);
        }

        public override string name => nameof(DatePicker);

        #region 属性

        #region 参数

        IControl control;

        float Radius = 6;
        int t_width = 288, t_one_width = 288, t_h = 0, t_x = 0, left_button = 120, t_top = 34, t_button = 38, t_time = 56, t_time_height = 30;
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
        List<CalendarButton>? left_buttons = null;

        /// <summary>
        /// 回调
        /// </summary>
        Action<DateTime[]> action;
        Action<object> action_btns;
        Func<DateTime[], List<DateBadge>?>? badge_action;
        Dictionary<string, DateBadge> badge_list = new Dictionary<string, DateBadge>();

        #endregion

        #region 日期

        DateTime[]? SelData;

        DateTime[]? _SelTime;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime[]? SelTime
        {
            get => _SelTime;
            set
            {
                _SelTime = value;
                if (value == null) SelData = null;
                else SelData = new DateTime[] { new DateTime(value[0].Year, value[0].Month, value[0].Day), new DateTime(value[1].Year, value[1].Month, value[1].Day) };
            }
        }
        DateTime _Date;
        DateTime DateNow = DateTime.Now;
        List<Calendari>? calendar_year = null;
        List<Calendari>? calendar_month = null;
        List<Calendari>? calendar_day = null;
        List<CalendarT>? calendar_time = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;
                sizeday = size_month = size_year = true;
                calendar_day = GetCalendar(value);

                #region 添加时间

                if (calendar_time == null)
                {
                    calendar_time = new List<CalendarT>(24 + 120);
                    for (int i = 0; i < 24; i++) calendar_time.Add(new CalendarT(0, i, i));
                    for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(1, i, i));
                    for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(2, i, i));
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

        bool sizeday = true, size_month = true, size_year = true;
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
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path = rect_read.RoundPath(Radius))
                {
                    DrawShadow(g, rect);
                    using (var brush = new SolidBrush(Colour.BgElevated.Get("DatePicker")))
                    {
                        g.Fill(brush, path);
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                    }
                }

                #region 方向

                using (var pen_arrow = new Pen(Colour.TextTertiary.Get("DatePicker"), 1.6F * Config.Dpi))
                using (var pen_arrow_hover = new Pen(Colour.Text.Get("DatePicker"), pen_arrow.Width))
                using (var pen_arrow_enable = new Pen(Colour.FillSecondary.Get("DatePicker"), pen_arrow.Width))
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

                    if (showType == 0)
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

                if (showType == 1 && calendar_month != null) PrintMonth(g, rect_read, calendar_month);
                else if (showType == 2 && calendar_year != null) PrintYear(g, rect_read, calendar_year);
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
            Color color_fore_disable = Colour.TextQuaternary.Get("DatePicker"), color_bg_disable = Colour.FillTertiary.Get("DatePicker"), color_fore = Colour.TextBase.Get("DatePicker");

            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                var rect_l = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width, t_top);

                if (hover_year.Animation) g.String(year_str, font, color_fore.BlendColors(hover_year.Value, Colour.Primary.Get("DatePicker")), rect_l, s_f);
                else if (hover_year.Switch) g.String(year_str, font, Colour.Primary.Get("DatePicker"), rect_l, s_f);
                else g.String(year_str, font, color_fore, rect_l, s_f);
            }

            int size_w = (rect_read.Width - 16) / 3, size_h = (rect_read.Width - 16) / 7 * 2;
            int y = rect_read.Y + t_top;
            if (size_year)
            {
                size_year = false;
                foreach (var it in datas) it.rect = new Rectangle(rect_read.X + 8 + (size_w * it.x), y + (size_h * it.y), size_w, size_h);
            }
            foreach (var it in datas)
            {
                using (var path = it.rect_read.RoundPath(Radius))
                {
                    if (SelData != null && (SelData[0].ToString("yyyy") == it.date_str || (SelData.Length > 1 && SelData[1].ToString("yyyy") == it.date_str)))
                    {
                        g.Fill(Colour.Primary.Get("DatePicker"), path);
                        g.String(it.v, Font, Colour.PrimaryColor.Get("DatePicker"), it.rect, s_f);
                    }
                    else if (it.enable)
                    {
                        if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker"), path);
                        if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker"), Config.Dpi, path);
                        g.String(it.v, Font, it.t == 1 ? color_fore : color_fore_disable, it.rect, s_f);
                    }
                    else
                    {
                        g.Fill(color_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                        if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker"), Config.Dpi, path);
                        g.String(it.v, Font, color_fore_disable, it.rect, s_f);
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
            Color color_fore_disable = Colour.TextQuaternary.Get("DatePicker"), color_bg_disable = Colour.FillTertiary.Get("DatePicker"), color_fore = Colour.TextBase.Get("DatePicker");
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                var rect_l = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width, t_top);
                string yearStr = _Date.ToString(YearFormat, Culture);
                if (hover_year.Animation) g.String(yearStr, font, color_fore.BlendColors(hover_year.Value, Colour.Primary.Get("DatePicker")), rect_l, s_f);
                else if (hover_year.Switch) g.String(yearStr, font, Colour.Primary.Get("DatePicker"), rect_l, s_f);
                else g.String(yearStr, font, color_fore, rect_l, s_f);
            }

            int size_w = (rect_read.Width - 16) / 3, size_h = (rect_read.Width - 16) / 7 * 2;
            int y = rect_read.Y + t_top;
            if (size_month)
            {
                size_month = false;
                foreach (var it in datas) it.rect = new Rectangle(rect_read.X + 8 + (size_w * it.x), y + (size_h * it.y), size_w, size_h);
            }
            foreach (var it in datas)
            {
                using (var path = it.rect_read.RoundPath(Radius))
                {
                    if (SelData != null && (SelData[0].ToString("yyyy-MM") == it.date_str || (SelData.Length > 1 && SelData[1].ToString("yyyy-MM") == it.date_str)))
                    {
                        g.Fill(Colour.Primary.Get("DatePicker"), path);
                        g.String(it.v, Font, Colour.PrimaryColor.Get("DatePicker"), it.rect, s_f);
                    }
                    else if (it.enable)
                    {
                        if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker"), path);
                        if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker"), Config.Dpi, path);
                        g.String(it.v, Font, color_fore, it.rect, s_f);
                    }
                    else
                    {
                        g.Fill(color_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                        if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker"), Config.Dpi, path);
                        g.String(it.v, Font, color_fore_disable, it.rect, s_f);
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
            Color color_fore = Colour.TextBase.Get("DatePicker");
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture), monthStr = _Date.ToString(MonthFormat, Culture);

                if (hover_year.Animation) g.String(yearStr, font, color_fore.BlendColors(hover_year.Value, Colour.Primary.Get("DatePicker")), rect_year, s_f_L);
                else if (hover_year.Switch) g.String(yearStr, font, Colour.Primary.Get("DatePicker"), rect_year, s_f_L);
                else g.String(yearStr, font, color_fore, rect_year, s_f_L);

                if (hover_month.Animation) g.String(monthStr, font, color_fore.BlendColors(hover_month.Value, Colour.Primary.Get("DatePicker")), rect_month, s_f_R);
                else if (hover_month.Switch) g.String(monthStr, font, Colour.Primary.Get("DatePicker"), rect_month, s_f_R);
                else g.String(monthStr, font, color_fore, rect_month, s_f_R);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get("DatePicker")))
            {
                g.Fill(brush_split, new RectangleF(t_x + rect_read.X, rect_read.Y + t_top, t_one_width, Config.Dpi));
                g.Fill(brush_split, new RectangleF(t_x + rect_read.X, rect_button.Y, rect_read.Width - t_x, Config.Dpi));
                g.Fill(brush_split, new RectangleF(t_x + rect_read.X + t_one_width, rect_read.Y, Config.Dpi, rect_read.Height));
                if (left_buttons != null) g.Fill(brush_split, new RectangleF(t_x + rect_read.X, rect_read.Y, Config.Dpi, rect_read.Height));
            }
            int y = rect_read.Y + t_top + 12;
            int size = (t_one_width - 16) / 7;

            using (var brush = new SolidBrush(Colour.Text.Get("DatePicker")))
            {
                g.String(MondayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8, y, size, size), s_f);
                g.String(TuesdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size, y, size, size), s_f);
                g.String(WednesdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 2, y, size, size), s_f);
                g.String(ThursdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 3, y, size, size), s_f);
                g.String(FridayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 4, y, size, size), s_f);
                g.String(SaturdayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 5, y, size, size), s_f);
                g.String(SundayButton, Font, brush, new Rectangle(t_x + rect_read.X + 8 + size * 6, y, size, size), s_f);
            }

            y += size;
            if (sizeday)
            {
                sizeday = false;
                int size_one = (int)(size * 0.666F);
                foreach (var it in datas)
                {
                    it.SetRect(new Rectangle(t_x + rect_read.X + 8 + (size * it.x), y + (size * it.y), size, size), size_one);
                }
                if (calendar_time != null)
                {
                    int size_time_one = (int)(t_time * 0.857F);
                    int size_time_height_one = (int)(t_time_height * 0.93F);

                    int endh = rect_button.Y;
                    var rect_s_h = new Rectangle(t_x, rect_read.Y + 8, rect_read.X + t_one_width + t_time, endh - rect_read.Y - 8);
                    rect_read_h = new Rectangle(rect_s_h.Right - t_time, 0, t_time, endh);
                    rect_read_m = new Rectangle(rect_s_h.Right, 0, t_time, endh);
                    rect_read_s = new Rectangle(rect_s_h.Right + t_time, 0, t_time, endh);
                    scrollY_h.SizeChange(rect_s_h);
                    rect_s_h.Width += t_time;
                    scrollY_m.SizeChange(rect_s_h);
                    rect_s_h.Width += t_time;
                    scrollY_s.SizeChange(rect_s_h);

                    int endh2 = endh - rect_read.Y * 2 - (t_time_height - size_time_height_one);
                    if (ValueTimeHorizontal)
                    {
                        int exceed = 10;
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

                    int _x = (t_time - size_time_one) / 2, _y = rect_read.Y + (t_time_height - size_time_height_one) / 2;
                    foreach (var it in calendar_time)
                    {
                        var rect_n = new Rectangle(t_time * it.x, t_time_height * it.y, t_time, t_time_height);
                        it.rect = new Rectangle(t_x + rect_read.X + t_one_width + rect_n.X, rect_read.Y + rect_n.Y, rect_n.Width, rect_n.Height);
                        it.rect_read = new Rectangle(rect_n.X + _x, rect_n.Y + _y, size_time_one, size_time_height_one);
                    }

                    if (SelTime != null) ScrollYTime(calendar_time, SelTime[0]);
                }
                if (left_buttons != null)
                {
                    int btn_one = (int)(left_button * .9F), btn_height_one = (int)(t_time_height * .93F), btn_one2 = (int)(left_button * .8F);

                    rect_read_left = new Rectangle(rect_read.X, rect_read.Y, t_x, t_h - rect_read.Y * 2);

                    scrollY_left.SizeChange(new Rectangle(rect_read.X, rect_read.Y + 8, t_x, t_h - (8 + rect_read.Y) * 2));
                    scrollY_left.SetVrSize(t_time_height * left_buttons.Count, t_h - 20 - rect_read.Y * 2);

                    int _x = (left_button - btn_one) / 2, _x2 = (btn_one - btn_one2) / 2, _y = rect_read.Y + (t_time_height - btn_height_one) / 2;
                    foreach (var it in left_buttons)
                    {
                        var rect_n = new Rectangle(0, t_time_height * it.y, left_button, t_time_height);
                        it.rect_read = new Rectangle(rect_n.X + _x, rect_n.Y + _y, btn_one, btn_height_one);
                        it.rect = new Rectangle(rect_read.X + rect_n.X, rect_read.Y + rect_n.Y, rect_n.Width, rect_n.Height);

                        it.rect_text = new Rectangle(rect_read.X + _x2, it.rect_read.Y, btn_one2, it.rect_read.Height);
                    }
                }
            }

            Color color_fore_disable = Colour.TextQuaternary.Get("DatePicker"), color_bg_disable = Colour.FillTertiary.Get("DatePicker"), color_bg_active = Colour.Primary.Get("DatePicker"), color__bg_activebg = Colour.PrimaryBg.Get("DatePicker"), color_fore_active = Colour.PrimaryColor.Get("DatePicker");
            if (isEnd > 0)
            {
                if (SData.HasValue && HoverData.HasValue)
                {
                    //渲染范围
                    if (HoverData.Value != SData.Value && HoverData.Value > SData.Value) PrintCalendarMutual(g, SData.Value, HoverData.Value, color_bg_active, color__bg_activebg, datas);
                    else
                    {
                        foreach (var it in datas)
                        {
                            if (it.t == 1 && it.date_str == SData.Value.ToString("yyyy-MM-dd"))
                            {
                                using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                                {
                                    g.Fill(color_bg_active, path_l);
                                }
                            }
                        }
                    }
                }
                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        if ((HoverData.HasValue && SData.HasValue) && it.date < SData.Value)
                        {
                            g.Fill(color_bg_disable, new RectangleF(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                            g.String(it.v, Font, color_fore_disable, it.rect, s_f);
                        }
                        else if ((HoverData.HasValue && SData.HasValue) && it.t == 1 && (it.date_str == SData.Value.ToString("yyyy-MM-dd") || it.date_str == HoverData.Value.ToString("yyyy-MM-dd"))) g.String(it.v, Font, color_fore_active, it.rect, s_f);
                        else
                        {
                            if (it.hover) g.Fill(color_bg_disable, path);
                            g.String(it.v, Font, it.t == 1 ? color_fore : color_fore_disable, it.rect, s_f);
                        }
                    }
                }
            }
            else
            {
                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        bool hand = true;
                        if (it.t == 1)
                        {
                            if (SData.HasValue && SData.Value.ToString("yyyy-MM-dd") == it.date_str)
                            {
                                g.Fill(color_bg_active, path);
                                g.String(it.v, Font, color_fore_active, it.rect, s_f);
                                hand = false;
                            }
                            else if (!STime.HasValue && SelData != null)
                            {
                                if (SelData.Length > 1)
                                {
                                    if (SelData[0] == SelData[1])
                                    {
                                        if (SelData[0].ToString("yyyy-MM-dd") == it.date_str)
                                        {
                                            g.Fill(color_bg_active, path);
                                            g.String(it.v, Font, color_fore_active, it.rect, s_f);
                                            hand = false;
                                        }
                                    }
                                    else if (SelData[0] <= it.date && SelData[1] >= it.date)
                                    {
                                        //范围
                                        if (SelData[0].ToString("yyyy-MM-dd") == it.date_str)
                                        {
                                            //前面
                                            g.Fill(color__bg_activebg, new RectangleF(it.rect_read.Right, it.rect_read.Y, it.rect.Width - it.rect_read.Width, it.rect_read.Height));
                                            using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                                            {
                                                g.Fill(color_bg_active, path_l);
                                            }
                                            g.String(it.v, Font, color_fore_active, it.rect, s_f);
                                        }
                                        else if (SelData[1].ToString("yyyy-MM-dd") == it.date_str)
                                        {
                                            //后面
                                            g.Fill(color__bg_activebg, new RectangleF(it.rect.X, it.rect_read.Y, it.rect_read.Width, it.rect_read.Height));
                                            using (var path_r = it.rect_read.RoundPath(Radius, false, true, true, false))
                                            {
                                                g.Fill(color_bg_active, path_r);
                                            }
                                            g.String(it.v, Font, color_fore_active, it.rect, s_f);
                                        }
                                        else
                                        {
                                            g.Fill(color__bg_activebg, new RectangleF(it.rect.X - 1F, it.rect_read.Y, it.rect.Width + 2F, it.rect_read.Height));
                                            g.String(it.v, Font, color_fore, it.rect, s_f);
                                        }
                                        hand = false;
                                    }
                                }
                                else if (SelData[0].ToString("yyyy-MM-dd") == it.date_str)
                                {
                                    g.Fill(color_bg_active, path);
                                    g.String(it.v, Font, color_fore_active, it.rect, s_f);
                                    hand = false;
                                }
                            }
                        }
                        if (hand)
                        {
                            if (SData.HasValue && it.date_str == SData.Value.ToString("yyyy-MM-dd"))
                            {
                                g.Fill(color_bg_disable, new RectangleF(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                                g.String(it.v, Font, color_fore_disable, it.rect, s_f);
                            }
                            else if (it.enable)
                            {
                                if (it.hover) g.Fill(color_bg_disable, path);
                                g.String(it.v, Font, it.t == 1 ? color_fore : color_fore_disable, it.rect, s_f);
                            }
                            else
                            {
                                g.Fill(color_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                                g.String(it.v, Font, color_fore_disable, it.rect, s_f);
                            }
                        }
                    }
                }
            }

            if (badge_list.Count > 0)
            {
                foreach (var it in datas)
                {
                    if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, it.rect, g);
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
                            if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker"), path);
                            g.String(it.v, Font, color_fore, it.rect_text, s_f_LE);
                        }
                    }
                    g.Restore(state);
                    scrollY_left.Paint(g);
                }

                if (calendar_time != null)
                {
                    var state = g.Save();
                    int tx = t_x + rect_read.X + t_one_width;
                    g.SetClip(new Rectangle(tx, rect_read.Y, t_time * 3, rect_button.Y - 10));
                    using (var brush_bg = new SolidBrush(Colour.PrimaryBg.Get("DatePicker")))
                    {
                        g.TranslateTransform(tx, 10 - scrollY_h.Value);
                        for (int i = 0; i < calendar_time.Count; i++)
                        {
                            if (i == 24)
                            {
                                g.ResetTransform();
                                g.TranslateTransform(tx, 10 - scrollY_m.Value);
                            }
                            else if (i == 84)
                            {
                                g.ResetTransform();
                                g.TranslateTransform(tx, 10 - scrollY_s.Value);
                            }
                            var it = calendar_time[i];
                            using (var path = it.rect_read.RoundPath(Radius))
                            {
                                if (ETime.HasValue)
                                {
                                    switch (it.x)
                                    {
                                        case 0:
                                            if (it.t == ETime.Value.Hour) g.Fill(brush_bg, path);
                                            break;
                                        case 1:
                                            if (it.t == ETime.Value.Minute) g.Fill(brush_bg, path);
                                            break;
                                        case 2:
                                            if (it.t == ETime.Value.Second) g.Fill(brush_bg, path);
                                            break;
                                    }
                                }
                                else if (STime.HasValue)
                                {
                                    switch (it.x)
                                    {
                                        case 0:
                                            if (it.t == STime.Value.Hour) g.Fill(brush_bg, path);
                                            break;
                                        case 1:
                                            if (it.t == STime.Value.Minute) g.Fill(brush_bg, path);
                                            break;
                                        case 2:
                                            if (it.t == STime.Value.Second) g.Fill(brush_bg, path);
                                            break;
                                    }
                                }
                                else if (SelTime != null && SelTime.Length > 0)
                                {
                                    switch (it.x)
                                    {
                                        case 0:
                                            if (it.t == SelTime[0].Hour) g.Fill(brush_bg, path);
                                            break;
                                        case 1:
                                            if (it.t == SelTime[0].Minute) g.Fill(brush_bg, path);
                                            break;
                                        case 2:
                                            if (it.t == SelTime[0].Second) g.Fill(brush_bg, path);
                                            break;
                                    }
                                }
                                if (it.hover) g.Fill(Colour.FillTertiary.Get("DatePicker"), path);
                                g.String(it.v, Font, color_fore, it.rect_read, s_f);
                            }
                        }
                    }
                    g.Restore(state);
                    scrollY_h.Paint(g);
                    scrollY_m.Paint(g);
                    scrollY_s.Paint(g);

                    if (hover_buttonok.Animation) g.String(OKButton, Font, color_bg_active.BlendColors(hover_buttonok.Value, Colour.PrimaryActive.Get("DatePicker")), rect_buttonok, s_f);
                    else if (hover_buttonok.Switch) g.String(OKButton, Font, Colour.PrimaryActive.Get("DatePicker"), rect_buttonok, s_f);
                    else g.String(OKButton, Font, color_bg_active, rect_buttonok, s_f);
                }
            }
            if (hover_button.Animation) g.String(button_text, Font, color_bg_active.BlendColors(hover_button.Value, Colour.PrimaryActive.Get("DatePicker")), rect_button, s_f);
            else if (hover_button.Switch)
            {
                using (var brush_hove = new SolidBrush(Colour.PrimaryActive.Get("DatePicker")))
                {
                    g.String(button_text, Font, brush_hove, rect_button, s_f);
                }
            }
            else g.String(button_text, Font, color_bg_active, rect_button, s_f);

            #region 渲染当天

            string nowstr = DateNow.ToString("yyyy-MM-dd");
            if (HoverData.HasValue && SData.HasValue)
            {
                if (SData.Value.ToString("yyyy-MM-dd") == nowstr || HoverData.Value.ToString("yyyy-MM-dd") == nowstr) return;
            }
            if (SelData != null && SelData.Length > 0)
            {
                if (SelData.Length > 1)
                {
                    if (SelData[1].ToString("yyyy-MM-dd") == nowstr) return;
                }
                else if (SelData[0].ToString("yyyy-MM-dd") == nowstr) return;
            }
            foreach (var it in datas)
            {
                if (nowstr == it.date_str)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        g.Draw(Colour.Primary.Get("DatePicker"), Config.Dpi, path);
                    }
                }
            }

            #endregion
        }

        void PrintCalendarMutual(Canvas g, DateTime STime, DateTime ETime, Color color_bg_active, Color color_bg_activebg, List<Calendari> datas)
        {
            foreach (var it in datas)
            {
                if (it.t == 1)
                {
                    if (it.date_str == STime.ToString("yyyy-MM-dd"))
                    {
                        g.Fill(color_bg_activebg, new RectangleF(it.rect_read.Right, it.rect_read.Y, it.rect.Width - it.rect_read.Width, it.rect_read.Height));
                        using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                        {
                            g.Fill(color_bg_active, path_l);
                        }
                    }
                    else if (it.date_str == ETime.ToString("yyyy-MM-dd"))
                    {
                        g.Fill(color_bg_activebg, new RectangleF(it.rect.X, it.rect_read.Y, it.rect_read.Width, it.rect_read.Height));
                        using (var path_r = it.rect_read.RoundPath(Radius, false, true, true, false))
                        {
                            g.Fill(color_bg_active, path_r);
                        }
                    }
                    else if (it.date > STime && it.date < ETime)
                    {
                        g.Fill(color_bg_activebg, new RectangleF(it.rect.X - 1F, it.rect_read.Y, it.rect.Width + 2F, it.rect_read.Height));
                    }
                }
            }
        }

        #endregion

        #endregion

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">客户区域</param>
        void DrawShadow(Canvas g, Rectangle rect)
        {
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null || shadow_temp.PixelFormat == System.Drawing.Imaging.PixelFormat.DontCare)
                {
                    shadow_temp?.Dispose();
                    using (var path = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20).RoundPath(Radius))
                    {
                        shadow_temp = path.PaintShadow(rect.Width, rect.Height);
                    }
                }
                g.Image(shadow_temp, rect, 0.2F);
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
            if (rect_read_h.Contains(e.X, e.Y)) scrollY_h.MouseDown(e.Location);
            else if (rect_read_m.Contains(e.X, e.Y)) scrollY_m.MouseDown(e.Location);
            else if (rect_read_s.Contains(e.X, e.Y)) scrollY_s.MouseDown(e.Location);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RunAnimation) return;
            if (scrollY_left.MouseMove(e.Location) && scrollY_h.MouseMove(e.Location) && scrollY_m.MouseMove(e.Location) && scrollY_s.MouseMove(e.Location))
            {
                int count = 0, hand = 0;
                bool _hover_lefts = rect_lefts.Contains(e.X, e.Y),
                 _hover_rights = rect_rights.Contains(e.X, e.Y),
                 _hover_left = (showType == 0 && rect_left.Contains(e.X, e.Y)),
                 _hover_right = (showType == 0 && rect_right.Contains(e.X, e.Y)),
                 _hover_button = (showType == 0 && rect_button.Contains(e.X, e.Y)),
                 _hover_buttonok = (showType == 0 && rect_buttonok.Contains(e.X, e.Y));

                bool _hover_year = false, _hover_month = false;
                if (showType != 2)
                {
                    _hover_year = showType == 0 ? rect_year.Contains(e.X, e.Y) : rect_year2.Contains(e.X, e.Y);
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
                    if (showType == 1)
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
                    else if (showType == 2)
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
                                if (it.hover)
                                {
                                    if (isEnd == 1) HoverTime = it.date;
                                    hand++;
                                }
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
                                switch (it.x)
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
                foreach (var it in calendar_year)
                {
                    it.hover = false;
                }
            }
            if (calendar_month != null)
            {
                foreach (var it in calendar_month)
                {
                    it.hover = false;
                }
            }
            if (calendar_day != null)
            {
                foreach (var it in calendar_day)
                {
                    it.hover = false;
                }
            }
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

        int showType = 0;

        void CSize()
        {
            if (left_buttons != null)
            {
                t_x = showType == 0 ? left_button : 0;

                rect_lefts = new Rectangle(t_x + 10, 10, t_top, t_top);
                rect_left = new Rectangle(t_x + 10 + t_top, 10, t_top, t_top);
                rect_rights = new Rectangle(t_x + t_one_width + 10 - t_top, 10, t_top, t_top);
                rect_right = new Rectangle(t_x + t_one_width + 10 - t_top * 2, 10, t_top, t_top);

                int gap = (int)(4 * Config.Dpi), t_width2 = t_one_width / 2;
                rect_year2 = new Rectangle(t_x + 10 + (t_one_width - year2_width) / 2, 10, year2_width, t_top);
                if (YDR)
                {
                    rect_month = new Rectangle(t_x + 10 + t_width2 - year_width - gap, 10, year_width, t_top);
                    rect_year = new Rectangle(t_x + 10 + t_width2 + gap, 10, month_width, t_top);
                }
                else
                {
                    rect_year = new Rectangle(t_x + 10 + t_width2 - year_width - gap, 10, year_width, t_top);
                    rect_month = new Rectangle(t_x + 10 + t_width2 + gap, 10, month_width, t_top);
                }
            }
            if (showType == 0) SetSize(t_width + 20, t_h);
            else SetSize(t_one_width + 20, t_h);
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
                        if (showType == 2) Date = _Date.AddYears(-10);
                        else Date = _Date.AddYears(-1);
                        Print();
                    }
                    return;
                }
                else if (rect_rights.Contains(e.X, e.Y))
                {
                    if (hover_rights.Enable)
                    {
                        if (showType == 2) Date = _Date.AddYears(10);
                        else Date = _Date.AddYears(1);
                        Print();
                    }
                    return;
                }
                else if (showType == 0 && rect_left.Contains(e.X, e.Y))
                {
                    if (hover_left.Enable)
                    {
                        Date = _Date.AddMonths(-1);
                        Print();
                    }
                    return;
                }
                else if (showType == 0 && rect_right.Contains(e.X, e.Y))
                {
                    if (hover_right.Enable)
                    {
                        Date = _Date.AddMonths(1);
                        Print();
                    }
                    return;
                }
                else if ((showType == 0 && rect_year.Contains(e.X, e.Y)) || (showType != 0 && rect_year2.Contains(e.X, e.Y)))
                {
                    showType = 2;
                    CSize();
                    Print();
                    return;
                }
                else if (showType == 0 && rect_button.Contains(e.X, e.Y))
                {
                    if (isEnd > 0)
                    {
                        isEnd = 2;
                        ETime = HoverTime = DateTime.Now;
                        if (calendar_time != null) ScrollYTime(calendar_time, ETime.Value);
                    }
                    else if (isEnd == 0)
                    {
                        STime = DateTime.Now;
                        if (calendar_time != null) ScrollYTime(calendar_time, STime.Value);
                    }
                    Print();
                    return;
                }
                else if (showType == 0 && rect_buttonok.Contains(e.X, e.Y))
                {
                    if (isEnd > 0 && STime.HasValue && ETime.HasValue)
                    {
                        if (STime.Value == ETime.Value) SelTime = new DateTime[] { ETime.Value, ETime.Value };
                        else if (STime.Value < ETime.Value) SelTime = new DateTime[] { STime.Value, ETime.Value };
                        else SelTime = new DateTime[] { ETime.Value, STime.Value };
                        action(SelTime);
                        IClose();
                    }
                    else if (isEnd == 0)
                    {
                        if (STime.HasValue)
                        {
                            ETime = SData;
                            HoverTime = STime.Value;
                            isEnd = 1;
                            if (calendar_time != null && ETime.HasValue) ScrollYTime(calendar_time, ETime.Value);
                            Print();
                        }
                    }
                    return;
                }
                else if (rect_month.Contains(e.X, e.Y))
                {
                    showType = 1;
                    CSize();
                    Print();
                    return;
                }
                else
                {
                    if (showType == 1)
                    {
                        if (calendar_month != null)
                        {
                            foreach (var it in calendar_month)
                            {
                                if (it.enable && it.rect.Contains(e.X, e.Y))
                                {
                                    Date = it.date;
                                    showType = 0;
                                    CSize();
                                    Print();
                                    return;
                                }
                            }
                        }
                    }
                    else if (showType == 2)
                    {
                        if (calendar_year != null)
                        {
                            foreach (var it in calendar_year)
                            {
                                if (it.enable && it.rect.Contains(e.X, e.Y))
                                {
                                    Date = it.date;
                                    showType = 1;
                                    CSize();
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
                                    if (isEnd == 2)
                                    {
                                        STime = ETime = HoverTime = null;
                                        STime = it.date;
                                        isEnd = 0;
                                        if (calendar_time != null) ScrollYTime(calendar_time, STime.Value);
                                    }
                                    else if (isEnd == 1)
                                    {
                                        ETime = it.date;
                                        isEnd = 2;
                                        if (calendar_time != null) ScrollYTime(calendar_time, ETime.Value);
                                    }
                                    else if (isEnd == 0)
                                    {
                                        if (STime.HasValue) STime = new DateTime(it.date.Year, it.date.Month, it.date.Day, STime.Value.Hour, STime.Value.Minute, STime.Value.Second);
                                        else
                                        {
                                            STime = it.date;
                                            if (calendar_time != null) ScrollYTime(calendar_time, STime.Value);
                                        }
                                    }
                                    Print();
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
                                switch (it.x)
                                {
                                    case 1:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_m.Value, out _))
                                        {
                                            if (isEnd > 0)
                                            {
                                                if (ETime.HasValue) ETime = new DateTime(ETime.Value.Year, ETime.Value.Month, ETime.Value.Day, ETime.Value.Hour, it.t, ETime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    ETime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, it.t, 0);
                                                }
                                                if (ValueTimeHorizontal) ScrollYTime(calendar_time, ETime.Value);
                                            }
                                            else if (isEnd == 0)
                                            {
                                                if (STime.HasValue) STime = new DateTime(STime.Value.Year, STime.Value.Month, STime.Value.Day, STime.Value.Hour, it.t, STime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    STime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, it.t, 0);
                                                }
                                                if (ValueTimeHorizontal) ScrollYTime(calendar_time, STime.Value);
                                            }
                                            Print();
                                            return;
                                        }
                                        break;
                                    case 2:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_s.Value, out _))
                                        {
                                            if (isEnd > 0)
                                            {
                                                if (ETime.HasValue) ETime = new DateTime(ETime.Value.Year, ETime.Value.Month, ETime.Value.Day, ETime.Value.Hour, ETime.Value.Minute, it.t);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    ETime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, 0, it.t);
                                                }
                                                if (ValueTimeHorizontal) ScrollYTime(calendar_time, ETime.Value);
                                            }
                                            else if (isEnd == 0)
                                            {
                                                if (STime.HasValue) STime = new DateTime(STime.Value.Year, STime.Value.Month, STime.Value.Day, STime.Value.Hour, STime.Value.Minute, it.t);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    STime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, 0, it.t);
                                                }
                                                if (ValueTimeHorizontal) ScrollYTime(calendar_time, STime.Value);
                                            }
                                            Print();
                                            return;
                                        }
                                        break;
                                    case 0:
                                    default:
                                        if (it.Contains(e.X, e.Y, 0, scrollY_h.Value, out _))
                                        {
                                            if (isEnd > 0)
                                            {
                                                if (ETime.HasValue) ETime = new DateTime(ETime.Value.Year, ETime.Value.Month, ETime.Value.Day, it.t, ETime.Value.Minute, ETime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    ETime = new DateTime(datenow.Year, datenow.Month, datenow.Day, it.t, 0, 0);
                                                }
                                                if (ValueTimeHorizontal) ScrollYTime(calendar_time, ETime.Value);
                                            }
                                            else if (isEnd == 0)
                                            {
                                                if (STime.HasValue) STime = new DateTime(STime.Value.Year, STime.Value.Month, STime.Value.Day, it.t, STime.Value.Minute, STime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    STime = new DateTime(datenow.Year, datenow.Month, datenow.Day, it.t, 0, 0);
                                                }
                                                if (ValueTimeHorizontal) ScrollYTime(calendar_time, STime.Value);
                                            }
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

        int isEnd = 0;
        DateTime? SData, EData, HoverData;
        DateTime? _STime, _ETime, _HoverTime;
        DateTime? STime
        {
            get => _STime;
            set
            {
                _STime = value;
                if (value.HasValue) SData = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day);
                else SData = null;
            }
        }
        DateTime? ETime
        {
            get => _ETime;
            set
            {
                _ETime = value;
                if (value.HasValue) EData = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day);
                else EData = null;
            }
        }

        DateTime? HoverTime
        {
            get => _HoverTime;
            set
            {
                _HoverTime = value;
                if (value.HasValue) HoverData = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day);
                else HoverData = null;
            }
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
            base.OnMouseWheel(e);
        }

        void MouseWheelDay(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (showType == 1)
                {
                    if (hover_lefts.Enable) Date = _Date.AddYears(-1);
                    else return;
                }
                else if (showType == 2)
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
                if (showType == 1)
                {
                    if (hover_rights.Enable) Date = _Date.AddYears(1);
                    else return;
                }
                else if (showType == 2)
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
            CalendarT? find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour),
                find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute),
                find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);

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
}