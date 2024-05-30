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
    public class LayeredFormCalendarTimeRange : ILayeredFormOpacityDown
    {
        public LayeredFormCalendarTimeRange(DatePickerRange _control, RectangleF rect_read, DateTime[]? date, Action<DateTime[]> _action, Action<object> _action_btns, Func<DateTime[], List<DateBadge>?>? _badge_action = null)
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
            hover_button = new ITaskOpacity(this);
            hover_buttonok = new ITaskOpacity(this);
            scrollY_left = new ScrollY(this);
            scrollY_h = new ScrollY(this);
            scrollY_m = new ScrollY(this);
            scrollY_s = new ScrollY(this);
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
            t_width = t_x + t_one_width + t_time * 3; button_text = Localization.Provider?.GetLocalizedString("Now") ?? "此刻";

            rect_lefts = new RectangleF(t_x + 10, 10, t_top, t_top);
            rect_left = new RectangleF(t_x + 10 + t_top, 10, t_top, t_top);
            rect_rights = new RectangleF(t_x + t_one_width + 10 - t_top, 10, t_top, t_top);
            rect_right = new RectangleF(t_x + t_one_width + 10 - t_top * 2, 10, t_top, t_top);

            rect_year = new RectangleF(t_x + 10 + t_one_width / 2 - year_width, 10, year_width, t_top);
            rect_year2 = new RectangleF(t_x + 10 + (t_one_width - year2_width) / 2, 10, year2_width, t_top);
            rect_month = new RectangleF(t_x + 10 + t_one_width / 2, 10, month_width, t_top);

            Font = new Font(_control.Font.FontFamily, 11.2F);

            SelTime = date;
            Date = date == null ? DateNow : date[0];

            var point = _control.PointToScreen(Point.Empty);
            if (calendar_day == null) EndHeight = 348 + 20;
            else EndHeight = (t_top + t_button) + (12 * 2) + (int)Math.Ceiling((calendar_day[calendar_day.Count - 1].y + 2) * (t_one_width - 16) / 7F) + 20;
            SetSize(t_width + 20, 0);
            rect_button = new RectangleF(t_x + 10 + (t_one_width - year_width) / 2, EndHeight - t_button - (20 - 8), year_width, t_button);

            int t_time_w = t_time * 3;
            rect_buttonok = new RectangleF(t_x + 10 + t_one_width, rect_button.Y, t_time_w, t_button);

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

        #region 参数

        IControl control;

        float Radius = 6;
        int t_width = 288, t_one_width = 288, t_x = 0, left_button = 120, t_top = 34, t_button = 38, t_time = 56, t_time_height = 30;
        int year_width = 60, year2_width = 88, month_width = 40;
        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        string button_text = Localization.Provider?.GetLocalizedString("ToDay") ?? "今天",
            OKButton = Localization.Provider?.GetLocalizedString("OK") ?? "确定",
            YearButton = Localization.Provider?.GetLocalizedString("Year") ?? "年",
            MonthButton = Localization.Provider?.GetLocalizedString("Month") ?? "月",
            MondayButton = Localization.Provider?.GetLocalizedString("Mon") ?? "一",
            TuesdayButton = Localization.Provider?.GetLocalizedString("Tue") ?? "二",
            WednesdayButton = Localization.Provider?.GetLocalizedString("Wed") ?? "三",
            ThursdayButton = Localization.Provider?.GetLocalizedString("Thu") ?? "四",
            FridayButton = Localization.Provider?.GetLocalizedString("Fri") ?? "五",
            SaturdayButton = Localization.Provider?.GetLocalizedString("Sat") ?? "六",
            SundayButton = Localization.Provider?.GetLocalizedString("Sun") ?? "日";

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
        DateTime _Date, _Date_R;
        DateTime DateNow = DateTime.Now;
        List<Calendari>? calendar_year = null;
        List<Calendari>? calendar_month = null;
        List<Calendari>? calendar_day = null;
        List<CalendarT>? calendar_time = null;
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
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
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
                            if (SelData != null && (SelData[0].ToString("yyyy") == it.date_str || (SelData.Length > 1 && SelData[1].ToString("yyyy") == it.date_str)))
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
                        if (SelData != null && (SelData[0].ToString("yyyy-MM") == it.date_str || (SelData.Length > 1 && SelData[1].ToString("yyyy-MM") == it.date_str)))
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

        Rectangle rect_read_left, rect_read_h, rect_read_m, rect_read_s;
        /// <summary>
        /// 渲染天模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintDay(Graphics g, Rectangle rect_read, List<Calendari> datas)
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
                }

                using (var brush_split = new SolidBrush(Style.Db.Split))
                {
                    g.FillRectangle(brush_split, new RectangleF(t_x + rect_read.X, rect_read.Y + t_top, t_one_width, 1F));
                    g.FillRectangle(brush_split, new RectangleF(t_x + rect_read.X, rect_button.Y, rect_read.Width - t_x, 1F));
                    g.FillRectangle(brush_split, new RectangleF(t_x + rect_read.X + t_one_width, rect_read.Y, 1F, rect_read.Height));
                    if (left_buttons != null) g.FillRectangle(brush_split, new RectangleF(t_x + rect_read.X, rect_read.Y, 1F, rect_read.Height));
                }
                float y = rect_read.Y + t_top + 12;
                float size = (t_one_width - 16) / 7F;
                using (var brush = new SolidBrush(Style.Db.Text))
                {
                    g.DrawString(MondayButton, Font, brush, new RectangleF(t_x + rect_read.X + 8F, y, size, size), stringFormatC);
                    g.DrawString(TuesdayButton, Font, brush, new RectangleF(t_x + rect_read.X + 8F + size, y, size, size), stringFormatC);
                    g.DrawString(WednesdayButton, Font, brush, new RectangleF(t_x + rect_read.X + 8F + size * 2F, y, size, size), stringFormatC);
                    g.DrawString(ThursdayButton, Font, brush, new RectangleF(t_x + rect_read.X + 8F + size * 3F, y, size, size), stringFormatC);
                    g.DrawString(FridayButton, Font, brush, new RectangleF(t_x + rect_read.X + 8F + size * 4F, y, size, size), stringFormatC);
                    g.DrawString(SaturdayButton, Font, brush, new RectangleF(t_x + rect_read.X + 8F + size * 5F, y, size, size), stringFormatC);
                    g.DrawString(SundayButton, Font, brush, new RectangleF(t_x + rect_read.X + 8F + size * 6F, y, size, size), stringFormatC);
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
                    if (calendar_time != null)
                    {
                        float size_time_one = t_time * 0.857F;
                        float size_time_height_one = t_time_height * 0.93F;

                        int endh = (int)rect_button.Y;
                        var rect_s_h = new Rectangle(t_x, rect_read.Y + 8, rect_read.X + t_one_width + t_time, endh - rect_read.Y - 8);
                        rect_read_h = new Rectangle(rect_s_h.Right - t_time, 0, t_time, endh);
                        rect_read_m = new Rectangle(rect_s_h.Right, 0, t_time, endh);
                        rect_read_s = new Rectangle(rect_s_h.Right + t_time, 0, t_time, endh);
                        scrollY_h.SizeChange(rect_s_h);
                        rect_s_h.Width += t_time;
                        scrollY_m.SizeChange(rect_s_h);
                        rect_s_h.Width += t_time;
                        scrollY_s.SizeChange(rect_s_h);

                        int endh2 = endh - rect_read.Y * 2 - (t_time_height - (int)size_time_height_one);
                        scrollY_h.SetVrSize(t_time_height * 24, endh2);
                        scrollY_m.SetVrSize(t_time_height * 60, endh2);
                        scrollY_s.SetVrSize(t_time_height * 60, endh2);

                        float _x = (t_time - size_time_one) / 2F, _y = rect_read.Y + (t_time_height - size_time_height_one) / 2F;
                        foreach (var it in calendar_time)
                        {
                            var rect_n = new RectangleF(t_time * it.x, t_time_height * it.y, t_time, t_time_height);
                            it.rect = new RectangleF(t_x + rect_read.X + t_one_width + rect_n.X, rect_read.Y + rect_n.Y, rect_n.Width, rect_n.Height);
                            it.rect_read = new RectangleF(rect_n.X + _x, rect_n.Y + _y, size_time_one, size_time_height_one);
                        }

                        if (SelTime != null)
                        {
                            var d = SelTime[0];
                            var find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour);
                            if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                            var find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute);
                            if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                            var find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);
                            if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                        }
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
                    if (isEnd > 0)
                    {
                        if (SData.HasValue && HoverData.HasValue)
                        {
                            //渲染范围
                            if (HoverData.Value != SData.Value && HoverData.Value > SData.Value) PrintCalendarMutual(g, SData.Value, HoverData.Value, brush_bg_active, brush_bg_activebg, datas);
                            else
                            {
                                foreach (var it in datas)
                                {
                                    if (it.t == 1 && it.date_str == SData.Value.ToString("yyyy-MM-dd"))
                                    {
                                        using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                                        {
                                            g.FillPath(brush_bg_active, path_l);
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
                                    g.FillRectangle(brush_bg_disable, new RectangleF(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                                    g.DrawString(it.v, Font, brush_fore_disable, it.rect, stringFormatC);
                                }
                                else if ((HoverData.HasValue && SData.HasValue) && it.t == 1 && (it.date_str == SData.Value.ToString("yyyy-MM-dd") || it.date_str == HoverData.Value.ToString("yyyy-MM-dd"))) g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                                else
                                {
                                    if (it.hover) g.FillPath(brush_bg_disable, path);
                                    g.DrawString(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, stringFormatC);
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
                                        g.FillPath(brush_bg_active, path);
                                        g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
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
                                                    g.FillPath(brush_bg_active, path);
                                                    g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                                                    hand = false;
                                                }
                                            }
                                            else if (SelData[0] <= it.date && SelData[1] >= it.date)
                                            {
                                                //范围
                                                if (SelData[0].ToString("yyyy-MM-dd") == it.date_str)
                                                {
                                                    //前面
                                                    g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect_read.Right, it.rect_read.Y, it.rect.Width - it.rect_read.Width, it.rect_read.Height));
                                                    using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                                                    {
                                                        g.FillPath(brush_bg_active, path_l);
                                                    }
                                                    g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                                                }
                                                else if (SelData[1].ToString("yyyy-MM-dd") == it.date_str)
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
                                        else if (SelData[0].ToString("yyyy-MM-dd") == it.date_str)
                                        {
                                            g.FillPath(brush_bg_active, path);
                                            g.DrawString(it.v, Font, brush_fore_active, it.rect, stringFormatC);
                                            hand = false;
                                        }
                                    }
                                }
                                if (hand)
                                {
                                    if (SData.HasValue && it.date_str == SData.Value.ToString("yyyy-MM-dd"))
                                    {
                                        g.FillRectangle(brush_bg_disable, new RectangleF(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                                        g.DrawString(it.v, Font, brush_fore_disable, it.rect, stringFormatC);
                                    }
                                    else
                                    {
                                        if (it.hover) g.FillPath(brush_bg_disable, path);
                                        g.DrawString(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, stringFormatC);
                                    }
                                }
                            }
                        }
                    }

                    if (badge_list.Count > 0)
                    {
                        using (var font = new Font(control.Font.FontFamily, control.Font.Size * control.BadgeSize))
                        {
                            foreach (var it in datas)
                            {
                                if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, font, it.rect, g);
                            }
                        }
                    }

                    if (rect_read.Height > t_button)
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

                        if (calendar_time != null)
                        {
                            using (var bmp = new Bitmap(t_time * 3, rect_read.Height - t_button))
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
                                            if (ETime.HasValue)
                                            {
                                                switch (it.x)
                                                {
                                                    case 0:
                                                        if (it.t == ETime.Value.Hour) g2.FillPath(brush_bg, path);
                                                        break;
                                                    case 1:
                                                        if (it.t == ETime.Value.Minute) g2.FillPath(brush_bg, path);
                                                        break;
                                                    case 2:
                                                        if (it.t == ETime.Value.Second) g2.FillPath(brush_bg, path);
                                                        break;
                                                }
                                            }
                                            else if (STime.HasValue)
                                            {
                                                switch (it.x)
                                                {
                                                    case 0:
                                                        if (it.t == STime.Value.Hour) g2.FillPath(brush_bg, path);
                                                        break;
                                                    case 1:
                                                        if (it.t == STime.Value.Minute) g2.FillPath(brush_bg, path);
                                                        break;
                                                    case 2:
                                                        if (it.t == STime.Value.Second) g2.FillPath(brush_bg, path);
                                                        break;
                                                }
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
                                g.DrawImage(bmp, new Rectangle(t_x + rect_read.X + t_one_width, rect_read.Y, bmp.Width, bmp.Height));
                            }

                            scrollY_h.Paint(g);
                            scrollY_m.Paint(g);
                            scrollY_s.Paint(g);

                            if (hover_buttonok.Animation)
                            {
                                g.DrawString(OKButton, Font, brush_bg_active, rect_buttonok, stringFormatC);
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
                            else g.DrawString(OKButton, Font, brush_bg_active, rect_buttonok, stringFormatC);
                        }
                    }
                    if (hover_button.Animation)
                    {
                        g.DrawString(button_text, Font, brush_bg_active, rect_button, stringFormatC);
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
                    else g.DrawString(button_text, Font, brush_bg_active, rect_button, stringFormatC);

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
                                using (var pen_active = new Pen(Style.Db.Primary, 0.1F))
                                {
                                    g.DrawPath(pen_active, path);
                                }
                            }
                        }
                    }

                    #endregion
                }
            }
        }

        void PrintCalendarMutual(Graphics g, DateTime STime, DateTime ETime, Brush brush_bg_active, Brush brush_bg_activebg, List<Calendari> datas)
        {
            foreach (var it in datas)
            {
                if (it.t == 1)
                {
                    if (it.date_str == STime.ToString("yyyy-MM-dd"))
                    {
                        g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect_read.Right, it.rect_read.Y, it.rect.Width - it.rect_read.Width, it.rect_read.Height));
                        using (var path_l = it.rect_read.RoundPath(Radius, true, false, false, true))
                        {
                            g.FillPath(brush_bg_active, path_l);
                        }
                    }
                    else if (it.date_str == ETime.ToString("yyyy-MM-dd"))
                    {
                        g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect.X, it.rect_read.Y, it.rect_read.Width, it.rect_read.Height));
                        using (var path_r = it.rect_read.RoundPath(Radius, false, true, true, false))
                        {
                            g.FillPath(brush_bg_active, path_r);
                        }
                    }
                    else if (it.date > STime && it.date < ETime)
                    {
                        g.FillRectangle(brush_bg_activebg, new RectangleF(it.rect.X - 1F, it.rect_read.Y, it.rect.Width + 2F, it.rect_read.Height));
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

        #region 鼠标

        ITaskOpacity hover_button, hover_buttonok, hover_lefts, hover_left, hover_rights, hover_right, hover_year, hover_month;
        RectangleF rect_button = new RectangleF(-20, -20, 10, 10), rect_buttonok = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_lefts = new RectangleF(-20, -20, 10, 10), rect_left = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_rights = new RectangleF(-20, -20, 10, 10), rect_right = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_year = new RectangleF(-20, -20, 10, 10), rect_year2 = new RectangleF(-20, -20, 10, 10), rect_month = new RectangleF(-20, -20, 10, 10);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (left_buttons != null && rect_read_left.Contains(e.Location)) if (!scrollY_left.MouseDown(e.Location)) return;

            if (rect_read_h.Contains(e.Location)) scrollY_h.MouseDown(e.Location);
            else if (rect_read_m.Contains(e.Location)) scrollY_m.MouseDown(e.Location);
            else if (rect_read_s.Contains(e.Location)) scrollY_s.MouseDown(e.Location);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (scrollY_left.MouseMove(e.Location) && scrollY_h.MouseMove(e.Location) && scrollY_m.MouseMove(e.Location) && scrollY_s.MouseMove(e.Location))
            {
                int count = 0, hand = 0;
                bool _hover_lefts = rect_lefts.Contains(e.Location),
                 _hover_rights = rect_rights.Contains(e.Location),
                 _hover_left = (showType == 0 && rect_left.Contains(e.Location)),
                 _hover_right = (showType == 0 && rect_right.Contains(e.Location)),
                 _hover_button = (showType == 0 && rect_button.Contains(e.Location)),
                 _hover_buttonok = (showType == 0 && rect_buttonok.Contains(e.Location));

                bool _hover_year = false, _hover_month = false;
                if (showType != 2)
                {
                    _hover_year = showType == 0 ? rect_year.Contains(e.Location) : rect_year2.Contains(e.Location);
                    _hover_month = rect_month.Contains(e.Location);
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
                if (hover_lefts.Switch || hover_left.Switch || hover_rights.Switch || hover_right.Switch || hover_year.Switch || hover_month.Switch || hover_button.Switch || hover_buttonok.Switch)
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
                                    if (isEnd == 1) HoverTime = it.date;
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

                rect_lefts = new RectangleF(t_x + 10, 10, t_top, t_top);
                rect_left = new RectangleF(t_x + 10 + t_top, 10, t_top, t_top);
                rect_rights = new RectangleF(t_x + t_one_width + 10 - t_top, 10, t_top, t_top);
                rect_right = new RectangleF(t_x + t_one_width + 10 - t_top * 2, 10, t_top, t_top);

                rect_year = new RectangleF(t_x + 10 + t_one_width / 2 - year_width, 10, year_width, t_top);
                rect_year2 = new RectangleF(t_x + 10 + (t_one_width - year2_width) / 2, 10, year2_width, t_top);
                rect_month = new RectangleF(t_x + 10 + t_one_width / 2, 10, month_width, t_top);
            }
            if (showType == 0) SetSize(t_width + 20, EndHeight);
            else SetSize(t_one_width + 20, EndHeight);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            scrollY_left.MouseUp(e.Location);
            scrollY_h.MouseUp(e.Location);
            scrollY_m.MouseUp(e.Location);
            scrollY_s.MouseUp(e.Location);
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
                else if ((showType == 0 && rect_year.Contains(e.Location)) || (showType != 0 && rect_year2.Contains(e.Location)))
                {
                    showType = 2;
                    if (left_buttons != null) CSize();
                    Print();
                    return;
                }
                else if (showType == 0 && rect_button.Contains(e.Location))
                {
                    if (isEnd > 0)
                    {
                        isEnd = 2;
                        ETime = HoverTime = DateTime.Now;
                        if (calendar_time != null)
                        {
                            var d = ETime.Value;
                            var find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour);
                            if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                            var find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute);
                            if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                            var find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);
                            if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                        }
                    }
                    else if (isEnd == 0)
                    {
                        STime = DateTime.Now;
                        if (calendar_time != null)
                        {
                            var d = STime.Value;
                            var find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour);
                            if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                            var find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute);
                            if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                            var find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);
                            if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                        }
                    }
                    Print();
                    return;
                }
                else if (showType == 0 && rect_buttonok.Contains(e.Location))
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
                            if (calendar_time != null && ETime.HasValue)
                            {
                                var d = ETime.Value;
                                var find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour);
                                if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                                var find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute);
                                if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                                var find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);
                                if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                            }
                            Print();
                        }
                    }
                    return;
                }
                else if (rect_month.Contains(e.Location))
                {
                    showType = 1;
                    if (left_buttons != null) CSize();
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
                                    if (left_buttons != null) CSize();
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
                                    if (left_buttons != null) CSize();
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
                                    if (isEnd == 2)
                                    {
                                        STime = ETime = HoverTime = null;
                                        STime = it.date;
                                        isEnd = 0;
                                        if (calendar_time != null)
                                        {
                                            var d = STime.Value;
                                            var find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour);
                                            if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                                            var find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute);
                                            if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                                            var find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);
                                            if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                                        }
                                    }
                                    else if (isEnd == 1)
                                    {
                                        ETime = it.date;
                                        isEnd = 2;
                                        if (calendar_time != null)
                                        {
                                            var d = ETime.Value;
                                            var find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour);
                                            if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                                            var find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute);
                                            if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                                            var find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);
                                            if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                                        }
                                    }
                                    else if (isEnd == 0)
                                    {
                                        if (STime.HasValue) STime = new DateTime(it.date.Year, it.date.Month, it.date.Day, STime.Value.Hour, STime.Value.Minute, STime.Value.Second);
                                        else
                                        {
                                            STime = it.date;
                                            if (calendar_time != null)
                                            {
                                                var d = STime.Value;
                                                var find_h = calendar_time.Find(a => a.x == 0 && a.t == d.Hour);
                                                if (find_h != null) scrollY_h.Value = find_h.rect.Y - find_h.rect.Height;
                                                var find_m = calendar_time.Find(a => a.x == 1 && a.t == d.Minute);
                                                if (find_m != null) scrollY_m.Value = find_m.rect.Y - find_m.rect.Height;
                                                var find_s = calendar_time.Find(a => a.x == 2 && a.t == d.Second);
                                                if (find_s != null) scrollY_s.Value = find_s.rect.Y - find_s.rect.Height;
                                            }
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
                                if (it.Contains(e.Location, 0, scrollY_left.Value, out _))
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
                                        if (it.Contains(e.Location, 0, scrollY_m.Value, out _))
                                        {
                                            if (isEnd > 0)
                                            {
                                                if (ETime.HasValue) ETime = new DateTime(ETime.Value.Year, ETime.Value.Month, ETime.Value.Day, ETime.Value.Hour, it.t, ETime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    ETime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, it.t, 0);
                                                }
                                            }
                                            else if (isEnd == 0)
                                            {
                                                if (STime.HasValue) STime = new DateTime(STime.Value.Year, STime.Value.Month, STime.Value.Day, STime.Value.Hour, it.t, STime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    STime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, it.t, 0);
                                                }
                                            }
                                            Print();
                                            return;
                                        }
                                        break;
                                    case 2:
                                        if (it.Contains(e.Location, 0, scrollY_s.Value, out _))
                                        {
                                            if (isEnd > 0)
                                            {
                                                if (ETime.HasValue) ETime = new DateTime(ETime.Value.Year, ETime.Value.Month, ETime.Value.Day, ETime.Value.Hour, ETime.Value.Minute, it.t);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    ETime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, 0, it.t);
                                                }
                                            }
                                            else if (isEnd == 0)
                                            {
                                                if (STime.HasValue) STime = new DateTime(STime.Value.Year, STime.Value.Month, STime.Value.Day, STime.Value.Hour, STime.Value.Minute, it.t);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    STime = new DateTime(datenow.Year, datenow.Month, datenow.Day, 0, 0, it.t);
                                                }
                                            }
                                            Print();
                                            return;
                                        }
                                        break;
                                    case 0:
                                    default:
                                        if (it.Contains(e.Location, 0, scrollY_h.Value, out _))
                                        {
                                            if (isEnd > 0)
                                            {
                                                if (ETime.HasValue) ETime = new DateTime(ETime.Value.Year, ETime.Value.Month, ETime.Value.Day, it.t, ETime.Value.Minute, ETime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    ETime = new DateTime(datenow.Year, datenow.Month, datenow.Day, it.t, 0, 0);
                                                }
                                            }
                                            else if (isEnd == 0)
                                            {
                                                if (STime.HasValue) STime = new DateTime(STime.Value.Year, STime.Value.Month, STime.Value.Day, it.t, STime.Value.Minute, STime.Value.Second);
                                                else
                                                {
                                                    var datenow = DateTime.Now;
                                                    STime = new DateTime(datenow.Year, datenow.Month, datenow.Day, it.t, 0, 0);
                                                }
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
            if (e.Delta != 0)
            {
                if (left_buttons != null && rect_read_left.Contains(e.Location))
                {
                    scrollY_left.MouseWheel(e.Delta);
                    Print();
                    base.OnMouseWheel(e);
                    return;
                }
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
                else MouseWheelDay(e);
            }
            base.OnMouseWheel(e);
        }

        void MouseWheelDay(MouseEventArgs e)
        {
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


        #endregion

        protected override void Dispose(bool disposing)
        {
            hover_lefts?.Dispose(); hover_left?.Dispose(); hover_rights?.Dispose(); hover_right?.Dispose(); hover_year?.Dispose(); hover_month?.Dispose(); hover_button?.Dispose(); hover_buttonok?.Dispose();
            base.Dispose(disposing);
        }
    }
}