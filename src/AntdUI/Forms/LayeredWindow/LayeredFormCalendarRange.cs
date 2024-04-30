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
    public class LayeredFormCalendarRange : ILayeredFormOpacityDown
    {
        public LayeredFormCalendarRange(DatePickerRange _control, RectangleF rect_read, DateTime[]? date, Action<DateTime[]> _action, Action<object> _action_btns, Func<DateTime[], List<DateBadge>?>? _badge_action = null)
        {
            _control.Parent.SetTopMost(Handle);
            control = _control;
            badge_action = _badge_action;
            PARENT = _control;
            action = _action;
            action_btns = _action_btns;
            hover_lefts = new ITaskOpacity(this);
            hover_left = new ITaskOpacity(this);
            hover_rights = new ITaskOpacity(this);
            hover_right = new ITaskOpacity(this);
            hover_year = new ITaskOpacity(this);
            hover_month = new ITaskOpacity(this);
            hover_year_r = new ITaskOpacity(this);
            hover_month_r = new ITaskOpacity(this);
            scrollY_left = new ScrollY(this);

            float dpi = Config.Dpi;
            if (dpi != 1F)
            {
                t_one_width = (int)(t_one_width * dpi);
                t_top = (int)(t_top * dpi);
                t_time = (int)(t_time * dpi);
                t_time_height = (int)(t_time_height * dpi);
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
            t_width = t_x + t_one_width * 2;

            rect_lefts = new RectangleF(t_x + 10, 10, t_top, t_top);
            rect_left = new RectangleF(t_x + 10 + t_top, 10, t_top, t_top);
            rect_rights = new RectangleF(t_width + 10 - t_top, 10, t_top, t_top);
            rect_right = new RectangleF(t_width + 10 - t_top * 2, 10, t_top, t_top);

            rect_year = new RectangleF(t_x + 10 + t_one_width / 2 - year_width, 10, year_width, t_top);
            rect_year2 = new RectangleF(t_x + 10 + (t_one_width - year2_width) / 2, 10, year2_width, t_top);
            rect_month = new RectangleF(t_x + 10 + t_one_width / 2, 10, month_width, t_top);

            rect_year_r = new RectangleF(rect_year.Left + t_one_width, rect_year.Y, rect_year.Width, rect_year.Height);
            rect_month_r = new RectangleF(rect_month.Left + t_one_width, rect_month.Y, rect_month.Width, rect_month.Height);

            Font = new Font(_control.Font.FontFamily, 11.2F);
            SelDate = date;
            Date = date == null ? DateNow : date[0];

            var point = _control.PointToScreen(Point.Empty);
            SetSize(t_width + 20, 0);

            if (calendar_day == null) EndHeight = 348 + 20;
            else EndHeight = t_top + (12 * 2) + (int)Math.Ceiling((calendar_day[calendar_day.Count - 1].y + 2) * (t_one_width - 16) / 7F) + 20;
            Placement = _control.Placement;
            switch (Placement)
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

        #region 参数

        IControl control;
        int Radius = 6;
        int t_one_width = 288, t_width = 288, t_x = 0, left_button = 120, t_top = 34, t_time = 56, t_time_height = 30;
        int year_width = 60, year2_width = 88, month_width = 40;
        TAlignFrom Placement = TAlignFrom.BL;
        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        List<CalendarButton>? left_buttons = null;
        ScrollY scrollY_left;
        string YearButton = Localization.Provider?.GetLocalizedString("Year") ?? "年",
            MonthButton = Localization.Provider?.GetLocalizedString("Month") ?? "月",
            MondayButton = Localization.Provider?.GetLocalizedString("Mon") ?? "一",
            TuesdayButton = Localization.Provider?.GetLocalizedString("Tue") ?? "二",
            WednesdayButton = Localization.Provider?.GetLocalizedString("Wed") ?? "三",
            ThursdayButton = Localization.Provider?.GetLocalizedString("Thu") ?? "四",
            FridayButton = Localization.Provider?.GetLocalizedString("Fri") ?? "五",
            SaturdayButton = Localization.Provider?.GetLocalizedString("Sat") ?? "六",
            SundayButton = Localization.Provider?.GetLocalizedString("Sun") ?? "日";

        /// <summary>
        /// 回调
        /// </summary>
        Action<DateTime[]> action;
        Action<object> action_btns;
        Func<DateTime[], List<DateBadge>?>? badge_action;
        Dictionary<string, DateBadge> badge_list = new Dictionary<string, DateBadge>();

        #endregion

        #region 日期

        public DateTime[]? SelDate;
        DateTime _Date, _Date_R;
        DateTime DateNow = DateTime.Now;
        List<Calendari>? calendar_year = null;
        List<Calendari>? calendar_month = null;
        List<Calendari>? calendar_day = null;
        List<Calendari>? calendar_day2 = null;
        public DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;
                _Date_R = value.AddMonths(1);
                sizeday = size_month = size_year = true;
                calendar_day = GetCalendar(value);
                calendar_day2 = GetCalendar(_Date_R);

                #region 添加月

                var _calendar_month = new List<Calendari>(12);
                int x_m = 0, y_m = 0;
                for (int i = 0; i < 12; i++)
                {
                    var d_m = new DateTime(value.Year, i + 1, 1);
                    _calendar_month.Add(new Calendari(0, x_m, y_m, d_m.ToString("MM") + MonthButton, d_m, d_m.ToString("yyyy-MM")));
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
                    _calendar_year.Add(new Calendari(i == 0 ? 0 : 1, x_y, y_y, d_y.ToString("yyyy"), d_y, d_y.ToString("yyyy")));
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
                                Print();
                                return;
                            }
#if NET40 || NET46 || NET48
                            foreach (var it in dir) badge_list.Add(it.Date, it);
#else
                            foreach (var it in dir) badge_list.TryAdd(it.Date, it);
#endif
                            Print();
                        }
                    });
                }
            }
        }

        string year_str = "";

        bool sizeday = true, size_month = true, size_year = true;
        List<Calendari> GetCalendar(DateTime now)
        {
            List<Calendari> calendaris = new List<Calendari>(28);
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
                    calendaris.Add(new Calendari(0, (day_ - 1) - i, 0, day3.ToString(), new DateTime(date1.Year, date1.Month, day3)));
                }
            }
            int x = day_, y = 0;
            for (int i = 0; i < days; i++)
            {
                int day = i + 1;
                calendaris.Add(new Calendari(1, x, y, day.ToString(), new DateTime(now.Year, now.Month, day)));
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
                    calendaris.Add(new Calendari(2, x, y, day3.ToString(), new DateTime(date1.Year, date1.Month, day3)));
                    x++; day2++;
                }
                if (y < 5)
                {
                    y++;
                    for (int i = 0; i < 7; i++)
                    {
                        int day3 = day2 + 1;
                        calendaris.Add(new Calendari(2, i, y, day3.ToString(), new DateTime(date1.Year, date1.Month, day3)));
                        day2++;
                    }
                }
            }
            return calendaris;
        }

        #endregion

        #endregion

        #region 鼠标

        ITaskOpacity hover_lefts, hover_left, hover_rights, hover_right, hover_year, hover_month, hover_year_r, hover_month_r;
        RectangleF rect_lefts = new RectangleF(-20, -20, 10, 10), rect_left = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_rights = new RectangleF(-20, -20, 10, 10), rect_right = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_year = new RectangleF(-20, -20, 10, 10), rect_year2 = new RectangleF(-20, -20, 10, 10), rect_month = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_year_r = new RectangleF(-20, -20, 10, 10), rect_month_r = new RectangleF(-20, -20, 10, 10);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (left_buttons != null && rect_read_left.Contains(e.Location)) if (!scrollY_left.MouseDown(e.Location)) return;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (scrollY_left.MouseMove(e.Location))
            {
                int count = 0, hand = 0;
                bool _hover_lefts = rect_lefts.Contains(e.Location),
                 _hover_rights = rect_rights.Contains(e.Location),
                 _hover_left = (showType == 0 && rect_left.Contains(e.Location)),
                 _hover_right = (showType == 0 && rect_right.Contains(e.Location));

                bool _hover_year = false, _hover_month = false, _hover_year_r = false, _hover_month_r = false;
                if (showType != 2)
                {
                    _hover_year = showType == 0 ? rect_year.Contains(e.Location) : rect_year2.Contains(e.Location);
                    _hover_month = rect_month.Contains(e.Location);
                    _hover_year_r = rect_year_r.Contains(e.Location);
                    _hover_month_r = rect_month_r.Contains(e.Location);
                }

                if (_hover_lefts != hover_lefts.Switch) count++;
                if (_hover_left != hover_left.Switch) count++;
                if (_hover_rights != hover_rights.Switch) count++;
                if (_hover_right != hover_right.Switch) count++;

                if (_hover_year != hover_year.Switch) count++;
                if (_hover_month != hover_month.Switch) count++;
                if (_hover_year_r != hover_year_r.Switch) count++;
                if (_hover_month_r != hover_month_r.Switch) count++;

                hover_lefts.Switch = _hover_lefts;
                hover_left.Switch = _hover_left;
                hover_rights.Switch = _hover_rights;
                hover_right.Switch = _hover_right;
                hover_year.Switch = _hover_year;
                hover_month.Switch = _hover_month;
                hover_year_r.Switch = _hover_year_r;
                hover_month_r.Switch = _hover_month_r;
                if (hover_lefts.Switch || hover_left.Switch || hover_rights.Switch || hover_right.Switch || hover_year.Switch || hover_month.Switch || hover_year_r.Switch || hover_month_r.Switch)
                {
                    hand++;
                }
                else
                {
                    if (showType == 1)
                    {
                        if (calendar_month != null)
                        {
                            foreach (var it in calendar_month)
                            {
                                bool hove = it.rect.Contains(e.Location);
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
                                bool hove = it.rect.Contains(e.Location);
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
                                bool hove = it.rect.Contains(e.Location);
                                if (it.hover != hove) count++;
                                it.hover = hove;
                                if (it.hover)
                                {
                                    if (isend) oldtime2 = it.date;
                                    hand++;
                                }
                            }
                        }
                        if (calendar_day2 != null)
                        {
                            foreach (var it in calendar_day2)
                            {
                                bool hove = it.rect.Contains(e.Location);
                                if (it.hover != hove) count++;
                                it.hover = hove;
                                if (it.hover)
                                {
                                    if (isend) oldtime2 = it.date;
                                    hand++;
                                }
                            }
                        }
                        if (left_buttons != null)
                        {
                            foreach (var it in left_buttons)
                            {
                                if (it.Contains(e.Location, 0, scrollY_left.Value, out var change)) hand++;
                                if (change) count++;
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
            scrollY_left.Leave();
            hover_lefts.Switch = false;
            hover_left.Switch = false;
            hover_rights.Switch = false;
            hover_right.Switch = false;
            hover_year.Switch = false;
            hover_month.Switch = false;
            hover_year_r.Switch = false;
            hover_month_r.Switch = false;
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
            if (calendar_day2 != null)
            {
                foreach (var it in calendar_day2)
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
            if (left_buttons != null) t_x = showType == 0 ? left_button : 0;

            if (showType == 0)
            {
                t_width = t_x + t_one_width * 2;
                if (calendar_day == null) EndHeight = 348 + 20;
                else EndHeight = t_top * 2 + (12 * 2) + (int)Math.Ceiling((calendar_day[calendar_day.Count - 1].y + 2) * (t_one_width - 16) / 7F) + 20;
            }
            else
            {
                t_width = t_x + t_one_width;
                if (calendar_day == null) EndHeight = 348 + 20;
                else EndHeight = t_top * 2 + (12 * 2) + (int)Math.Ceiling((calendar_day[calendar_day.Count - 1].y + 2) * (t_one_width - 16) / 7F) + 20;
            }
            SetSize(t_width + 20, EndHeight);

            if (showType == 0)
            {
                rect_lefts = new RectangleF(t_x + 10, 10, t_top, t_top);
                rect_left = new RectangleF(t_x + 10 + t_top, 10, t_top, t_top);
                rect_rights = new RectangleF(t_width + 10 - t_top, 10, t_top, t_top);
                rect_right = new RectangleF(t_width + 10 - t_top * 2, 10, t_top, t_top);

                rect_year = new RectangleF(t_x + 10 + t_one_width / 2 - year_width, 10, year_width, t_top);
                rect_year2 = new RectangleF(t_x + 10 + (t_one_width - year2_width) / 2, 10, year2_width, t_top);
                rect_month = new RectangleF(t_x + 10 + t_one_width / 2, 10, month_width, t_top);

                rect_year_r = new RectangleF(rect_year.Left + t_one_width, rect_year.Y, rect_year.Width, rect_year.Height);
                rect_month_r = new RectangleF(rect_month.Left + t_one_width, rect_month.Y, rect_month.Width, rect_month.Height);
            }
            else
            {
                rect_lefts = new RectangleF(t_x + 10, 10, t_top, t_top);
                rect_left = new RectangleF(t_x + 10 + t_top, 10, t_top, t_top);
                rect_rights = new RectangleF(t_one_width + 10 - t_top, 10, t_top, t_top);
                rect_right = new RectangleF(t_one_width + 10 - t_top * 2, 10, t_top, t_top);

                rect_year = new RectangleF(t_x + 10 + t_one_width / 2 - year_width, 10, year_width, t_top);
                rect_year2 = new RectangleF(t_x + 10 + (t_one_width - year2_width) / 2, 10, year2_width, t_top);
                rect_month = new RectangleF(t_x + 10 + t_one_width / 2, 10, month_width, t_top);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            scrollY_left.MouseUp(e.Location);
            if (e.Button == MouseButtons.Left)
            {
                if (rect_lefts.Contains(e.Location))
                {
                    if (showType == 2) Date = _Date.AddYears(-10);
                    else Date = _Date.AddYears(-1);
                    Print();
                    return;
                }
                else if (rect_rights.Contains(e.Location))
                {
                    if (showType == 2) Date = _Date.AddYears(10);
                    else Date = _Date.AddYears(1);
                    Print();
                    return;
                }
                else if (showType == 0 && rect_left.Contains(e.Location))
                {
                    Date = _Date.AddMonths(-1);
                    Print();
                    return;
                }
                else if (showType == 0 && rect_right.Contains(e.Location))
                {
                    Date = _Date.AddMonths(1);
                    Print();
                    return;
                }
                else if ((showType == 0 && (rect_year.Contains(e.Location) || rect_year_r.Contains(e.Location))) || (showType != 0 && rect_year2.Contains(e.Location)))
                {
                    showType = 2;
                    CSize();
                    Print();
                    return;
                }
                else if (rect_month.Contains(e.Location) || rect_month_r.Contains(e.Location))
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
                                if (it.rect.Contains(e.Location))
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
                                if (it.rect.Contains(e.Location))
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
                                if (it.rect.Contains(e.Location))
                                {
                                    if (SetDate(it)) return;
                                    IClose();
                                    return;
                                }
                            }
                        }
                        if (calendar_day2 != null)
                        {
                            foreach (var it in calendar_day2)
                            {
                                if (it.rect.Contains(e.Location))
                                {
                                    if (SetDate(it)) return;
                                    IClose();
                                    return;
                                }
                            }
                        }
                        if (left_buttons != null)
                        {
                            foreach (var it in left_buttons)
                            {
                                if (it.Contains(e.Location, 0, scrollY_left.Value, out _))
                                {
                                    action_btns(it.Tag);
                                    IClose();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            base.OnMouseUp(e);
        }

        bool isend = false;
        DateTime oldtime;
        DateTime? oldtime2;
        bool SetDate(Calendari item)
        {
            if (isend)
            {
                if (oldtime == item.date) SelDate = new DateTime[] { item.date, item.date };
                else if (oldtime < item.date) SelDate = new DateTime[] { oldtime, item.date };
                else SelDate = new DateTime[] { item.date, oldtime };
                action(SelDate);
                isend = false;
                return false;
            }
            SelDate = null;
            oldtime2 = oldtime = item.date;
            isend = true;
            Print();
            return true;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (left_buttons != null && rect_read_left.Contains(e.Location))
                {
                    scrollY_left.MouseWheel(e.Delta);
                    Print();
                    base.OnMouseWheel(e);
                    return;
                }
                if (e.Delta > 0)
                {
                    if (showType == 1) Date = _Date.AddYears(-1);
                    else if (showType == 2) Date = _Date.AddYears(-10);
                    else Date = _Date.AddMonths(-1);
                    Print();
                }
                else
                {
                    if (showType == 1) Date = _Date.AddYears(1);
                    else if (showType == 2) Date = _Date.AddYears(10);
                    else Date = _Date.AddMonths(1);
                    Print();
                }
            }
            base.OnMouseWheel(e);
        }

        #endregion

        bool init = false;
        public override void LoadOK()
        { init = true; Print(); }

        float AnimationBarValue = 0;
        public void SetArrow(float x)
        {
            AnimationBarValue = x;
            if (init) Print();
        }

        #region 渲染

        StringFormat stringFormatC = Helper.SF();
        StringFormat stringFormatL = Helper.SF(lr: StringAlignment.Far);
        StringFormat stringFormatLE = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        StringFormat stringFormatR = Helper.SF(lr: StringAlignment.Near);
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
                        if (ArrowAlign != TAlign.None)
                        {
                            if (AnimationBarValue != 0F)
                            {
                                g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, new RectangleF(rect_read.X + AnimationBarValue, rect_read.Y, rect_read.Width, rect_read.Height)));
                            }
                            else g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                        }
                    }
                }

                #region 方向

                using (var pen_arrow = new Pen(Style.Db.TextTertiary, 1.6F))
                using (var pen_arrow_hover = new Pen(Style.Db.Text, 1.6F))
                {
                    if (hover_lefts.Animation)
                    {
                        PointF[] tl1 = TAlignMini.Left.TriangleLines(new RectangleF(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), 0.26F),
                            tl2 = TAlignMini.Left.TriangleLines(new RectangleF(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), 0.26F);
                        g.DrawLines(pen_arrow, tl1);
                        g.DrawLines(pen_arrow, tl2);
                        using (var pen_arrow_hovers = new Pen(Color.FromArgb(hover_lefts.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                        {
                            g.DrawLines(pen_arrow_hovers, tl1);
                            g.DrawLines(pen_arrow_hovers, tl2);
                        }
                    }
                    else if (hover_lefts.Switch)
                    {
                        g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(new RectangleF(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), 0.26F));
                        g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(new RectangleF(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), 0.26F));

                    }
                    else
                    {
                        g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(new RectangleF(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), 0.26F));
                        g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(new RectangleF(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), 0.26F));
                    }

                    if (hover_rights.Animation)
                    {
                        PointF[] tl1 = TAlignMini.Right.TriangleLines(new RectangleF(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), 0.26F),
                            tl2 = TAlignMini.Right.TriangleLines(new RectangleF(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), 0.26F);
                        g.DrawLines(pen_arrow, tl1);
                        g.DrawLines(pen_arrow, tl2);
                        using (var pen_arrow_hovers = new Pen(Color.FromArgb(hover_rights.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                        {
                            g.DrawLines(pen_arrow_hovers, tl1);
                            g.DrawLines(pen_arrow_hovers, tl2);
                        }
                    }
                    else if (hover_rights.Switch)
                    {
                        g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(new RectangleF(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), 0.26F));
                        g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(new RectangleF(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), 0.26F));

                    }
                    else
                    {
                        g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(new RectangleF(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), 0.26F));
                        g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(new RectangleF(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), 0.26F));
                    }

                    if (showType == 0)
                    {
                        if (hover_left.Animation)
                        {
                            var tl = TAlignMini.Left.TriangleLines(rect_left, 0.26F);
                            g.DrawLines(pen_arrow, tl);
                            using (var pen_arrow_hovers = new Pen(Color.FromArgb(hover_left.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                            {
                                g.DrawLines(pen_arrow_hovers, tl);
                            }
                        }
                        else if (hover_left.Switch)
                        {
                            g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(rect_left, 0.26F));

                        }
                        else
                        {
                            g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(rect_left, 0.26F));
                        }

                        if (hover_right.Animation)
                        {
                            var tl = TAlignMini.Right.TriangleLines(rect_right, 0.26F);
                            g.DrawLines(pen_arrow, tl);
                            using (var pen_arrow_hovers = new Pen(Color.FromArgb(hover_right.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                            {
                                g.DrawLines(pen_arrow_hovers, tl);
                            }
                        }
                        else if (hover_right.Switch)
                        {
                            g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(rect_right, 0.26F));

                        }
                        else
                        {
                            g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(rect_right, 0.26F));
                        }
                    }
                }

                #endregion

                if (showType == 1 && calendar_month != null) PrintMonth(g, rect_read, calendar_month);
                else if (showType == 2 && calendar_year != null) PrintYear(g, rect_read, calendar_year);
                else if (calendar_day != null && calendar_day2 != null) PrintDay(g, rect_read, calendar_day, calendar_day2);
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
        void PrintYear(Graphics g, Rectangle rect_read, List<Calendari> datas)
        {
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    RectangleF rect_l = new RectangleF(rect_read.X, rect_read.Y, rect_read.Width, t_top);

                    if (hover_year.Animation)
                    {
                        g.DrawString(year_str, font, brush_fore, rect_l, stringFormatC);
                        using (var brush_hove = new SolidBrush(Color.FromArgb(hover_year.Value, Style.Db.Primary)))
                        {
                            g.DrawString(year_str, font, brush_hove, rect_l, stringFormatC);
                        }
                    }
                    else if (hover_year.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(year_str, font, brush_hove, rect_l, stringFormatC);
                        }
                    }
                    else g.DrawString(year_str, font, brush_fore, rect_l, stringFormatC);
                }

                float size_w = (rect_read.Width - 16) / 3F, size_h = (rect_read.Width - 16) / 7F * 2F;
                float y = rect_read.Y + t_top;
                if (size_year)
                {
                    size_year = false;
                    foreach (var it in datas)
                    {
                        it.rect = new RectangleF(rect_read.X + 8F + (size_w * it.x), y + (size_h * it.y), size_w, size_h);
                    }
                }
                using (var brush_fore_disable = new SolidBrush(Style.Db.TextQuaternary))
                {
                    foreach (var it in datas)
                    {
                        using (var path = it.rect_read.RoundPath(Radius))
                        {
                            if (SelDate != null && (SelDate[0].ToString("yyyy") == it.date_str || (SelDate.Length > 1 && SelDate[1].ToString("yyyy") == it.date_str)))
                            {
                                using (var brush_hove = new SolidBrush(Style.Db.Primary))
                                {
                                    g.FillPath(brush_hove, path);
                                }

                                using (var brush_active_fore = new SolidBrush(Style.Db.PrimaryColor))
                                {
                                    g.DrawString(it.v, Font, brush_active_fore, it.rect, stringFormatC);
                                }
                            }
                            else
                            {
                                if (it.hover)
                                {
                                    using (var brush_hove = new SolidBrush(Style.Db.FillTertiary))
                                    {
                                        g.FillPath(brush_hove, path);
                                    }
                                }
                                if (DateNow.ToString("yyyy-MM-dd") == it.date_str)
                                {
                                    using (var brush_hove = new Pen(Style.Db.Primary, 0.1F))
                                    {
                                        g.DrawPath(brush_hove, path);
                                    }
                                }
                                g.DrawString(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, stringFormatC);
                            }
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
        void PrintMonth(Graphics g, Rectangle rect_read, List<Calendari> datas)
        {
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    var rect_l = new RectangleF(rect_read.X, rect_read.Y, rect_read.Width, t_top);

                    if (hover_year.Animation)
                    {
                        g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_l, stringFormatC);
                        using (var brush_hove = new SolidBrush(Color.FromArgb(hover_year.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_l, stringFormatC);
                        }
                    }
                    else if (hover_year.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_l, stringFormatC);
                        }
                    }
                    else g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_l, stringFormatC);
                }

                float size_w = (rect_read.Width - 16) / 3F, size_h = (rect_read.Width - 16) / 7F * 2F;
                float y = rect_read.Y + t_top;
                if (size_month)
                {
                    size_month = false;
                    foreach (var it in datas)
                    {
                        it.rect = new RectangleF(rect_read.X + 8F + (size_w * it.x), y + (size_h * it.y), size_w, size_h);
                    }
                }
                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        if (SelDate != null && (SelDate[0].ToString("yyyy-MM") == it.date_str || (SelDate.Length > 1 && SelDate[1].ToString("yyyy-MM") == it.date_str)))
                        {
                            using (var brush_hove = new SolidBrush(Style.Db.Primary))
                            {
                                g.FillPath(brush_hove, path);
                            }

                            using (var brush_active_fore = new SolidBrush(Style.Db.PrimaryColor))
                            {
                                g.DrawString(it.v, Font, brush_active_fore, it.rect, stringFormatC);
                            }
                        }
                        else
                        {
                            if (it.hover)
                            {
                                using (var brush_hove = new SolidBrush(Style.Db.FillTertiary))
                                {
                                    g.FillPath(brush_hove, path);
                                }
                            }
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str)
                            {
                                using (var brush_hove = new Pen(Style.Db.Primary, 0.1F))
                                {
                                    g.DrawPath(brush_hove, path);
                                }
                            }

                            g.DrawString(it.v, Font, brush_fore, it.rect, stringFormatC);
                        }
                    }
                }
            }
        }

        #endregion

        #region 天模式

        Rectangle rect_read_left;
        /// <summary>
        /// 渲染天模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintDay(Graphics g, Rectangle rect_read, List<Calendari> datas, List<Calendari> datas2)
        {
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                float xm = t_one_width / 2F;
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    RectangleF rect_l = new RectangleF(t_x + rect_read.X, rect_read.Y, xm, t_top), rect_r = new RectangleF(t_x + rect_read.X + xm, rect_read.Y, xm, t_top);

                    if (hover_year.Animation)
                    {
                        g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_l, stringFormatL);
                        using (var brush_hove = new SolidBrush(Color.FromArgb(hover_year.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_l, stringFormatL);
                        }
                    }
                    else if (hover_year.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_l, stringFormatL);
                        }
                    }
                    else g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_l, stringFormatL);

                    if (hover_month.Animation)
                    {
                        g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_fore, rect_r, stringFormatR);
                        using (var brush_hove = new SolidBrush(Color.FromArgb(hover_month.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_hove, rect_r, stringFormatR);
                        }
                    }
                    else if (hover_month.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_hove, rect_r, stringFormatR);
                        }
                    }
                    else g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_fore, rect_r, stringFormatR);

                    #region 右

                    RectangleF rect_r_l = new RectangleF(rect_l.X + t_one_width, rect_l.Y, rect_l.Width, rect_l.Height), rect_r_r = new RectangleF(rect_r.X + t_one_width, rect_r.Y, rect_r.Width, rect_r.Height);
                    if (hover_year_r.Animation)
                    {
                        g.DrawString(_Date_R.ToString("yyyy") + YearButton, font, brush_fore, rect_r_l, stringFormatL);
                        using (var brush_hove = new SolidBrush(Color.FromArgb(hover_year_r.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date_R.ToString("yyyy") + YearButton, font, brush_hove, rect_r_l, stringFormatL);
                        }
                    }
                    else if (hover_year_r.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date_R.ToString("yyyy") + YearButton, font, brush_hove, rect_r_l, stringFormatL);
                        }
                    }
                    else g.DrawString(_Date_R.ToString("yyyy") + YearButton, font, brush_fore, rect_r_l, stringFormatL);

                    if (hover_month_r.Animation)
                    {
                        g.DrawString(_Date_R.ToString("MM") + MonthButton, font, brush_fore, rect_r_r, stringFormatR);
                        using (var brush_hove = new SolidBrush(Color.FromArgb(hover_month_r.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date_R.ToString("MM") + MonthButton, font, brush_hove, rect_r_r, stringFormatR);
                        }
                    }
                    else if (hover_month_r.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date_R.ToString("MM") + MonthButton, font, brush_hove, rect_r_r, stringFormatR);
                        }
                    }
                    else g.DrawString(_Date_R.ToString("MM") + MonthButton, font, brush_fore, rect_r_r, stringFormatR);

                    #endregion

                }

                using (var brush_split = new SolidBrush(Style.Db.Split))
                {
                    g.FillRectangle(brush_split, new RectangleF(t_x + rect_read.X, rect_read.Y + t_top, t_width - t_x, 1F));
                    if (left_buttons != null) g.FillRectangle(brush_split, new RectangleF(t_x + rect_read.X, rect_read.Y, 1F, rect_read.Height));
                }
                float y = rect_read.Y + t_top + 12;
                float size = (t_one_width - 16) / 7F;
                using (var brush = new SolidBrush(Style.Db.Text))
                {
                    float x = t_x + rect_read.X + 8F, x2 = t_x + rect_read.X + t_one_width + 8F;
                    g.DrawString(MondayButton, Font, brush, new RectangleF(x, y, size, size), stringFormatC);
                    g.DrawString(TuesdayButton, Font, brush, new RectangleF(x + size, y, size, size), stringFormatC);
                    g.DrawString(WednesdayButton, Font, brush, new RectangleF(x + size * 2F, y, size, size), stringFormatC);
                    g.DrawString(ThursdayButton, Font, brush, new RectangleF(x + size * 3F, y, size, size), stringFormatC);
                    g.DrawString(FridayButton, Font, brush, new RectangleF(x + size * 4F, y, size, size), stringFormatC);
                    g.DrawString(SaturdayButton, Font, brush, new RectangleF(x + size * 5F, y, size, size), stringFormatC);
                    g.DrawString(SundayButton, Font, brush, new RectangleF(x + size * 6F, y, size, size), stringFormatC);

                    g.DrawString(MondayButton, Font, brush, new RectangleF(x2, y, size, size), stringFormatC);
                    g.DrawString(TuesdayButton, Font, brush, new RectangleF(x2 + size, y, size, size), stringFormatC);
                    g.DrawString(WednesdayButton, Font, brush, new RectangleF(x2 + size * 2F, y, size, size), stringFormatC);
                    g.DrawString(ThursdayButton, Font, brush, new RectangleF(x2 + size * 3F, y, size, size), stringFormatC);
                    g.DrawString(FridayButton, Font, brush, new RectangleF(x2 + size * 4F, y, size, size), stringFormatC);
                    g.DrawString(SaturdayButton, Font, brush, new RectangleF(x2 + size * 5F, y, size, size), stringFormatC);
                    g.DrawString(SundayButton, Font, brush, new RectangleF(x2 + size * 6F, y, size, size), stringFormatC);
                }

                y += size;
                if (sizeday)
                {
                    sizeday = false;
                    float size_one = size * 0.666F;
                    foreach (var it in datas)
                    {
                        it.SetRect(new RectangleF(t_x + rect_read.X + 8F + (size * it.x), y + (size * it.y), size, size), size_one);
                    }
                    foreach (var it in datas2)
                    {
                        it.SetRect(new RectangleF(t_x + rect_read.X + t_one_width + 8F + (size * it.x), y + (size * it.y), size, size), size_one);
                    }

                    if (left_buttons != null)
                    {
                        float btn_one = left_button * 0.9F, btn_height_one = t_time_height * 0.93F, btn_one2 = left_button * 0.8F;

                        rect_read_left = new Rectangle(rect_read.X, rect_read.Y, t_x, EndHeight - rect_read.Y * 2);

                        scrollY_left.SizeChange(new Rectangle(rect_read.X, rect_read.Y + 8, t_x, EndHeight - (8 + rect_read.Y) * 2));
                        scrollY_left.SetVrSize(t_time_height * left_buttons.Count, EndHeight - 20 - rect_read.Y * 2);

                        float _x = (left_button - btn_one) / 2F, _x2 = (btn_one - btn_one2) / 2F, _y = rect_read.Y + (t_time_height - btn_height_one) / 2F;
                        foreach (var it in left_buttons)
                        {
                            var rect_n = new RectangleF(0, t_time_height * it.y, left_button, t_time_height);
                            it.rect_read = new RectangleF(rect_n.X + _x, rect_n.Y + _y, btn_one, btn_height_one);
                            it.rect = new RectangleF(rect_read.X + rect_n.X, rect_read.Y + rect_n.Y, rect_n.Width, rect_n.Height);

                            it.rect_text = new RectangleF(rect_read.X + _x2, it.rect_read.Y, btn_one2, it.rect_read.Height);
                        }
                    }
                }

                using (var brush_bg_disable = new SolidBrush(Style.Db.FillTertiary))
                using (var brush_fore_disable = new SolidBrush(Style.Db.TextQuaternary))
                using (var brush_bg_active = new SolidBrush(Style.Db.Primary))
                using (var brush_bg_activebg = new SolidBrush(Style.Db.PrimaryBg))
                using (var brush_fore_active = new SolidBrush(Style.Db.PrimaryColor))
                {
                    if (oldtime2.HasValue)
                    {
                        if (oldtime2.Value != oldtime && oldtime2.Value > oldtime)
                        {
                            PrintCalendarMutual(g, oldtime2.Value, brush_bg_active, brush_bg_activebg, datas);
                            PrintCalendarMutual(g, oldtime2.Value, brush_bg_active, brush_bg_activebg, datas2);
                        }
                        else
                        {
                            foreach (var it in datas)
                            {
                                if (it.t == 1 && it.date == oldtime)
                                {
                                    using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                                    {
                                        g.FillPath(brush_bg_active, path_l);
                                    }
                                }
                            }
                            foreach (var it in datas2)
                            {
                                if (it.t == 1 && it.date == oldtime)
                                {
                                    using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                                    {
                                        g.FillPath(brush_bg_active, path_l);
                                    }
                                }
                            }
                        }
                    }

                    PrintCalendar(g, brush_fore, brush_fore_disable, brush_bg_disable, brush_bg_active, brush_bg_activebg, brush_fore_active, datas);
                    PrintCalendar(g, brush_fore, brush_fore_disable, brush_bg_disable, brush_bg_active, brush_bg_activebg, brush_fore_active, datas2);

                    if (rect_read.Height > t_time_height)
                    {
                        if (left_buttons != null)
                        {
                            using (var bmp = new Bitmap(left_button, rect_read.Height))
                            {
                                using (var g2 = Graphics.FromImage(bmp).HighLay())
                                {
                                    g2.TranslateTransform(0, -scrollY_left.Value);
                                    foreach (var it in left_buttons)
                                    {
                                        using (var path = it.rect_read.RoundPath(Radius))
                                        {
                                            if (it.hover)
                                            {
                                                using (var brush_hove = new SolidBrush(Style.Db.FillTertiary))
                                                {
                                                    g2.FillPath(brush_hove, path);
                                                }
                                            }
                                            g2.DrawString(it.v, Font, brush_fore, it.rect_text, stringFormatLE);
                                        }
                                    }
                                }
                                g.DrawImage(bmp, new Rectangle(rect_read.X, rect_read.Y, bmp.Width, bmp.Height));
                            }
                            scrollY_left.Paint(g);
                        }
                    }
                }
            }
        }
        void PrintCalendarMutual(Graphics g, DateTime oldtime2, Brush brush_bg_active, Brush brush_bg_activebg, List<Calendari> datas)
        {
            foreach (var it in datas)
            {
                if (it.t == 1)
                {
                    if (it.date > oldtime && it.date < oldtime2)
                    {
                        g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect.X - 1F, it.rect_read.Y, it.rect.Width + 2F, it.rect_read.Height));
                    }
                    else if (it.date == oldtime)
                    {
                        g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect_read.Right, it.rect_read.Y, it.rect.Width - it.rect_read.Width, it.rect_read.Height));
                        using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                        {
                            g.FillPath(brush_bg_active, path_l);
                        }
                    }
                    else if (it.date == oldtime2)
                    {
                        g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect.X, it.rect_read.Y, it.rect_read.Width, it.rect_read.Height));
                        using (var path_r = it.rect_read.RoundPath(Radius, false, true, true, false))
                        {
                            g.FillPath(brush_bg_active, path_r);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 渲染日期面板
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="brush_fore">文字颜色</param>
        /// <param name="brush_fore_disable">文字禁用颜色</param>
        /// <param name="brush_bg_disable">背景禁用颜色</param>
        /// <param name="brush_bg_active">激活主题色</param>
        /// <param name="brush_bg_activebg">激活背景色</param>
        /// <param name="brush_fore_active">激活字体色</param>
        /// <param name="datas">DATA</param>
        void PrintCalendar(Graphics g, Brush brush_fore, Brush brush_fore_disable, Brush brush_bg_disable, Brush brush_bg_active, Brush brush_bg_activebg, Brush brush_fore_active, List<Calendari> datas)
        {
            foreach (var it in datas)
            {
                using (var path = it.rect_read.RoundPath(Radius))
                {
                    bool hand = true;
                    if (it.t == 1 && SelDate != null)
                    {
                        if (SelDate.Length > 1)
                        {
                            if (SelDate[0] == SelDate[1])
                            {
                                if (SelDate[0].ToString("yyyy-MM-dd") == it.date_str)
                                {
                                    g.FillPath(brush_bg_active, path);
                                    g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                                    hand = false;
                                }
                            }
                            else if (SelDate[0] <= it.date && SelDate[1] >= it.date)
                            {
                                //范围
                                if (SelDate[0].ToString("yyyy-MM-dd") == it.date_str)
                                {
                                    //前面
                                    g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect_read.Right, it.rect_read.Y, it.rect.Width - it.rect_read.Width, it.rect_read.Height));
                                    using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                                    {
                                        g.FillPath(brush_bg_active, path_l);
                                    }
                                    g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                                }
                                else if (SelDate[1].ToString("yyyy-MM-dd") == it.date_str)
                                {
                                    //后面
                                    g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect.X, it.rect_read.Y, it.rect_read.Width, it.rect_read.Height));
                                    using (var path_r = it.rect_read.RoundPath(Radius, false, true, true, false))
                                    {
                                        g.FillPath(brush_bg_active, path_r);
                                    }
                                    g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                                }
                                else
                                {
                                    g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect.X - 1F, it.rect_read.Y, it.rect.Width + 2F, it.rect_read.Height));
                                    g.DrawString(it.v, Font, brush_fore, it.rect, stringFormatC);
                                }
                                hand = false;
                            }
                        }
                        else if (SelDate[0].ToString("yyyy-MM-dd") == it.date_str)
                        {
                            g.FillPath(brush_bg_active, path);
                            g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                            hand = false;
                        }
                    }
                    if (hand)
                    {
                        if (oldtime2.HasValue && it.date < oldtime)
                        {
                            g.FillRectangle(brush_bg_disable, new RectangleF(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                            g.DrawString(it.v, Font, brush_fore_disable, it.rect, stringFormatC);
                        }
                        else if (oldtime2.HasValue && it.t == 1 && (it.date == oldtime || it.date == oldtime2.Value)) g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                        else
                        {
                            if (it.hover) g.FillPath(brush_bg_disable, path);
                            g.DrawString(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, stringFormatC);
                        }
                    }
                }
            }

            string nowstr = DateNow.ToString("yyyy-MM-dd");
            if (oldtime2.HasValue)
            {
                if (oldtime.ToString("yyyy-MM-dd") == nowstr || oldtime2.Value.ToString("yyyy-MM-dd") == nowstr) return;
            }

            if (badge_list.Count > 0)
            {
                using (var font = new Font(control.Font.FontFamily, control.BadgeSize))
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, font, it.rect, g);
                    }
                }
            }

            if (SelDate != null && SelDate.Length > 0)
            {
                if (SelDate.Length > 1)
                {
                    if (SelDate[1].ToString("yyyy-MM-dd") == nowstr) return;
                }
                else if (SelDate[0].ToString("yyyy-MM-dd") == nowstr) return;
            }
            foreach (var it in datas)
            {
                if (nowstr == it.date_str)
                {
                    using (var path = it.rect_read.RoundPath(Radius))
                    {
                        using (var pen_active = new Pen(Style.Db.Primary, 0.1F))
                        {
                            g.DrawPath(pen_active, path);
                        }
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

        protected override void Dispose(bool disposing)
        {
            hover_lefts?.Dispose(); hover_left?.Dispose(); hover_rights?.Dispose(); hover_right?.Dispose(); hover_year?.Dispose(); hover_month?.Dispose();
            hover_year_r?.Dispose(); hover_month_r?.Dispose();
            base.Dispose(disposing);
        }
    }
}