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
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Calendar 日历
    /// </summary>
    /// <remarks>按照日历形式展示数据的容器。</remarks>
    [Description("Calendar 日历")]
    [ToolboxItem(true)]
    [DefaultProperty("Date")]
    [DefaultEvent("DateChanged")]
    public class Calendar : IControl
    {
        public Calendar()
        {
            hover_lefts = new ITaskOpacity(this);
            hover_left = new ITaskOpacity(this);
            hover_rights = new ITaskOpacity(this);
            hover_right = new ITaskOpacity(this);
            hover_year = new ITaskOpacity(this);
            hover_month = new ITaskOpacity(this);
            hover_button = new ITaskOpacity(this);
            Date = DateNow;

            Culture = new CultureInfo(CultureID);
            YDR = CultureID.StartsWith("en");
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
                s_f_L = Helper.SF(lr: StringAlignment.Far); s_f_R = Helper.SF(lr: StringAlignment.Near);
            }
        }

        #region 属性

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        bool full = false;
        /// <summary>
        /// 是否撑满
        /// </summary>
        [Description("是否撑满"), Category("外观"), DefaultValue(false)]
        public bool Full
        {
            get => full;
            set
            {
                if (full == value) return;
                full = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        bool chinese = false;
        /// <summary>
        /// 显示农历
        /// </summary>
        [Description("显示农历"), Category("外观"), DefaultValue(false)]
        public bool ShowChinese
        {
            get => chinese;
            set
            {
                if (chinese == value) return;
                chinese = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        bool showButtonToDay = true;
        /// <summary>
        /// 显示今天
        /// </summary>
        [Description("显示今天"), Category("外观"), DefaultValue(true)]
        public bool ShowButtonToDay
        {
            get => showButtonToDay;
            set
            {
                if (showButtonToDay == value) return;
                showButtonToDay = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        Dictionary<string, DateBadge> badge_list = new Dictionary<string, DateBadge>();
        /// <summary>
        /// 日期徽标回调
        /// </summary>
        public Func<DateTime[], List<DateBadge>?>? BadgeAction = null;

        #region 日期

        List<Calendari>? calendar_year = null;
        List<Calendari>? calendar_month = null;
        List<Calendari>? calendar_day = null;

        DateTime _value = DateTime.Now;
        /// <summary>
        /// 控件当前日期
        /// </summary>
        [Description("控件当前日期"), Category("数据")]
        public DateTime Value
        {
            get => _value;
            set
            {
                _value = value;
                DateChanged?.Invoke(this, new DateTimeEventArgs(_value));
                Invalidate();
                LoadBadge();
            }
        }

        DateTime? minDate;
        /// <summary>
        /// 最小日期
        /// </summary>
        [Description("最小日期"), Category("数据"), DefaultValue(null)]
        public DateTime? MinDate
        {
            get => minDate;
            set
            {
                if (minDate == value) return;
                minDate = value;
                Date = _Date;
                Invalidate();
            }
        }

        DateTime? maxDate = null;
        /// <summary>
        /// 最大日期
        /// </summary>
        [Description("最大日期"), Category("数据"), DefaultValue(null)]
        public DateTime? MaxDate
        {
            get => maxDate;
            set
            {
                if (maxDate == value) return;
                maxDate = value;
                Date = _Date;
                Invalidate();
            }
        }

        DateTime _Date;
        DateTime DateNow = DateTime.Now;
        DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;
                calendar_day = GetCalendar(value);

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

                OnSizeChanged(EventArgs.Empty);

                LoadBadge();

                hover_left.Enable = Helper.DateExceed(value.AddMonths(-1), minDate, maxDate);
                hover_right.Enable = Helper.DateExceed(value.AddMonths(1), minDate, maxDate);
                hover_lefts.Enable = Helper.DateExceed(value.AddYears(-1), minDate, maxDate);
                hover_rights.Enable = Helper.DateExceed(value.AddYears(1), minDate, maxDate);
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

        /// <summary>
        /// 加载徽标
        /// </summary>
        public void LoadBadge()
        {
            if (BadgeAction != null && calendar_day != null)
            {
                var oldval = _Date;
                ITask.Run(() =>
                {
                    var dir = BadgeAction(new DateTime[] { calendar_day[0].date, calendar_day[calendar_day.Count - 1].date });
                    if (_Date == oldval)
                    {
                        badge_list.Clear();
                        if (dir == null)
                        {
                            Invalidate();
                            return;
                        }
#if NET40 || NET46 || NET48
                        foreach (var it in dir) badge_list.Add(it.Date, it);
#else
                        foreach (var it in dir) badge_list.TryAdd(it.Date, it);
#endif
                        Invalidate();
                    }
                });
            }
        }

        #endregion

        #region 参数

        CultureInfo Culture;
        string CultureID = Localization.Get("ID", "zh-CN"),
            button_text = Localization.Get("ToDay", "今天"),
            OKButton = Localization.Get("OK", "确定"),
            YearFormat, MonthFormat,
            MondayButton, TuesdayButton, WednesdayButton, ThursdayButton, FridayButton, SaturdayButton, SundayButton;
        bool YDR = false;

        #endregion

        #endregion

        #region 渲染

        StringFormat s_f = Helper.SF(), s_f_LE = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        StringFormat s_f_L, s_f_R;
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect = ClientRectangle;
            var rect_read = ReadRectangle;

            using (var path = rect_read.RoundPath(radius * Config.Dpi))
            {
                g.Fill(Style.Db.BgElevated, path);
            }

            #region 方向

            using (var pen_arrow = new Pen(Style.Db.TextTertiary, 1.6F * Config.Dpi))
            using (var pen_arrow_hover = new Pen(Style.Db.Text, pen_arrow.Width))
            using (var pen_arrow_enable = new Pen(Style.Db.FillSecondary, pen_arrow.Width))
            {
                if (hover_lefts.Animation)
                {
                    PointF[] tl1 = TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X - 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F),
                        tl2 = TAlignMini.Left.TriangleLines(new Rectangle(rect_lefts.X + 4, rect_lefts.Y, rect_lefts.Width, rect_lefts.Height), .26F);
                    g.DrawLines(pen_arrow, tl1);
                    g.DrawLines(pen_arrow, tl2);
                    using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_lefts.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                    {
                        g.DrawLines(pen_arrow_hovers, tl1);
                        g.DrawLines(pen_arrow_hovers, tl2);
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
                    PointF[] tl1 = TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X - 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F),
                        tl2 = TAlignMini.Right.TriangleLines(new Rectangle(rect_rights.X + 4, rect_rights.Y, rect_rights.Width, rect_rights.Height), .26F);
                    g.DrawLines(pen_arrow, tl1);
                    g.DrawLines(pen_arrow, tl2);
                    using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_rights.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                    {
                        g.DrawLines(pen_arrow_hovers, tl1);
                        g.DrawLines(pen_arrow_hovers, tl2);
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
                        var tl = TAlignMini.Left.TriangleLines(rect_left, .26F);
                        g.DrawLines(pen_arrow, tl);
                        using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_left.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                        {
                            g.DrawLines(pen_arrow_hovers, tl);
                        }
                    }
                    else if (hover_left.Switch) g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(rect_left, .26F));
                    else if (hover_left.Enable) g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(rect_left, .26F));
                    else g.DrawLines(pen_arrow_enable, TAlignMini.Left.TriangleLines(rect_left, .26F));

                    if (hover_right.Animation)
                    {
                        var tl = TAlignMini.Right.TriangleLines(rect_right, .26F);
                        g.DrawLines(pen_arrow, tl);
                        using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_right.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
                        {
                            g.DrawLines(pen_arrow_hovers, tl);
                        }
                    }
                    else if (hover_right.Switch) g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(rect_right, .26F));
                    else if (hover_right.Enable) g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(rect_right, .26F));
                    else g.DrawLines(pen_arrow_enable, TAlignMini.Right.TriangleLines(rect_right, .26F));
                }
            }

            #endregion

            if (showType == 1 && calendar_month != null) PrintMonth(g, rect_read, radius, calendar_month);
            else if (showType == 2 && calendar_year != null) PrintYear(g, rect_read, radius, calendar_year);
            else if (calendar_day != null) PrintDay(g, rect_read, radius, calendar_day);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        #region 年模式

        Rectangle rect_year_l;
        /// <summary>
        /// 渲染年模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintYear(Canvas g, Rectangle rect_read, float radius, List<Calendari> datas)
        {
            using (var brush_fore_disable = new SolidBrush(Style.Db.TextQuaternary))
            using (var brush_bg_disable = new SolidBrush(Style.Db.FillTertiary))
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    if (hover_year.Animation)
                    {
                        g.String(year_str, font, brush_fore, rect_year_l, s_f);
                        using (var brush_hove = new SolidBrush(Helper.ToColor(hover_year.Value, Style.Db.Primary)))
                        {
                            g.String(year_str, font, brush_hove, rect_year_l, s_f);
                        }
                    }
                    else if (hover_year.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.String(year_str, font, brush_hove, rect_year_l, s_f);
                        }
                    }
                    else g.String(year_str, font, brush_fore, rect_year_l, s_f);
                }
                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(radius))
                    {
                        if (_value.ToString("yyyy") == it.date_str)
                        {
                            g.Fill(Style.Db.Primary, path);
                            g.String(it.v, Font, Style.Db.PrimaryColor, it.rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (it.hover) g.Fill(Style.Db.FillTertiary, path);
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Style.Db.Primary, Config.Dpi, path);
                            g.String(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Style.Db.Primary, Config.Dpi, path);
                            g.String(it.v, Font, brush_fore_disable, it.rect, s_f);
                        }
                    }
                }
            }
        }

        #endregion

        #region 月模式

        Rectangle rect_month_l;
        /// <summary>
        /// 渲染月模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintMonth(Canvas g, Rectangle rect_read, float radius, List<Calendari> datas)
        {
            using (var brush_fore_disable = new SolidBrush(Style.Db.TextQuaternary))
            using (var brush_bg_disable = new SolidBrush(Style.Db.FillTertiary))
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    string yearStr = _Date.ToString(YearFormat, Culture);
                    if (hover_year.Animation)
                    {
                        g.String(yearStr, font, brush_fore, rect_month_l, s_f);
                        g.String(yearStr, font, Helper.ToColor(hover_year.Value, Style.Db.Primary), rect_month_l, s_f);
                    }
                    else if (hover_year.Switch) g.String(yearStr, font, Style.Db.Primary, rect_month_l, s_f);
                    else g.String(yearStr, font, brush_fore, rect_month_l, s_f);
                }

                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(radius))
                    {
                        if (_value.ToString("yyyy-MM") == it.date_str)
                        {
                            g.Fill(Style.Db.Primary, path);
                            g.String(it.v, Font, Style.Db.PrimaryColor, it.rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (it.hover) g.Fill(Style.Db.FillTertiary, path);
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Style.Db.Primary, Config.Dpi, path);
                            g.String(it.v, Font, brush_fore, it.rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Style.Db.Primary, Config.Dpi, path);
                            g.String(it.v, Font, brush_fore_disable, it.rect, s_f);
                        }
                    }
                }
            }
        }

        #endregion

        #region 天模式

        RectangleF rect_day_split1, rect_day_split2;
        Rectangle[]? rect_day_s;

        /// <summary>
        /// 渲染天模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintDay(Canvas g, Rectangle rect_read, float radius, List<Calendari> datas)
        {
            if (rect_day_s == null) return;
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    string yearStr = _Date.ToString(YearFormat, Culture), monthStr = _Date.ToString(MonthFormat, Culture);
                    if (hover_year.Animation)
                    {
                        g.String(yearStr, font, brush_fore, rect_year, s_f_L);
                        g.String(yearStr, font, Helper.ToColor(hover_year.Value, Style.Db.Primary), rect_year, s_f_L);
                    }
                    else if (hover_year.Switch) g.String(yearStr, font, Style.Db.Primary, rect_year, s_f_L);
                    else g.String(yearStr, font, brush_fore, rect_year, s_f_L);

                    if (hover_month.Animation)
                    {
                        g.String(monthStr, font, brush_fore, rect_month, s_f_R);
                        g.String(monthStr, font, Helper.ToColor(hover_month.Value, Style.Db.Primary), rect_month, s_f_R);
                    }
                    else if (hover_month.Switch) g.String(monthStr, font, Style.Db.Primary, rect_month, s_f_R);
                    else g.String(monthStr, font, brush_fore, rect_month, s_f_R);
                }

                using (var brush_split = new SolidBrush(Style.Db.Split))
                {
                    g.Fill(brush_split, rect_day_split1);
                    if (showButtonToDay) g.Fill(brush_split, rect_day_split2);
                }
                using (var brush = new SolidBrush(Style.Db.Text))
                {
                    g.String(MondayButton, Font, brush, rect_day_s[0], s_f);
                    g.String(TuesdayButton, Font, brush, rect_day_s[1], s_f);
                    g.String(WednesdayButton, Font, brush, rect_day_s[2], s_f);
                    g.String(ThursdayButton, Font, brush, rect_day_s[3], s_f);
                    g.String(FridayButton, Font, brush, rect_day_s[4], s_f);
                    g.String(SaturdayButton, Font, brush, rect_day_s[5], s_f);
                    g.String(SundayButton, Font, brush, rect_day_s[6], s_f);
                }
                using (var brush_fore_disable = new SolidBrush(Style.Db.TextQuaternary))
                using (var brush_bg_disable = new SolidBrush(Style.Db.FillTertiary))
                using (var brush_active = new SolidBrush(Style.Db.Primary))
                using (var brush_active_fore = new SolidBrush(Style.Db.PrimaryColor))
                using (var brush_error = new SolidBrush(Style.Db.Error))
                {
                    PaintToDayFrame(g, datas, DateNow.ToString("yyyy-MM-dd"), radius);

                    if (chinese)
                    {
                        using (var font4 = new Font(Font.FontFamily, Font.Size * .76F, Font.Style))
                        {
                            using (var brush_fore_c = new SolidBrush(Style.Db.TextSecondary))
                            {
                                foreach (var it in datas)
                                {
                                    using (var path = it.rect_read.RoundPath(radius))
                                    {
                                        var cdate = ChineseCalendar.ChineseDate.From(it.date);
                                        if (_value.ToString("yyyy-MM-dd") == it.date_str)
                                        {
                                            g.Fill(brush_active, path);
                                            g.String(cdate.DayString, font4, brush_active_fore, it.rect_l, s_f);
                                            g.String(it.v, Font, brush_active_fore, it.rect_f, s_f);
                                        }
                                        else if (it.enable)
                                        {
                                            if (it.hover) g.Fill(Style.Db.FillTertiary, path);
                                            g.String(cdate.DayString, font4, brush_fore_c, it.rect_l, s_f);
                                            g.String(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect_f, s_f);
                                        }
                                        else
                                        {
                                            g.Fill(brush_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                                            g.String(cdate.DayString, font4, brush_fore_disable, it.rect_l, s_f);
                                            g.String(it.v, Font, brush_fore_disable, it.rect_f, s_f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in datas)
                        {
                            using (var path = it.rect_read.RoundPath(radius))
                            {
                                if (_value.ToString("yyyy-MM-dd") == it.date_str)
                                {
                                    g.Fill(brush_active, path);
                                    g.String(it.v, Font, brush_active_fore, it.rect, s_f);
                                }
                                else if (it.enable)
                                {
                                    if (it.hover) g.Fill(Style.Db.FillTertiary, path);
                                    g.String(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, s_f);
                                }
                                else
                                {
                                    g.Fill(brush_bg_disable, new Rectangle(it.rect.X, it.rect_read.Y, it.rect.Width, it.rect_read.Height));
                                    g.String(it.v, Font, brush_fore_disable, it.rect, s_f);
                                }
                            }
                        }
                    }
                    if (showButtonToDay)
                    {
                        if (hover_button.Animation)
                        {
                            g.String(button_text, Font, brush_active, rect_button, s_f);
                            using (var brush_hove = new SolidBrush(Helper.ToColor(hover_button.Value, Style.Db.PrimaryActive)))
                            {
                                g.String(button_text, Font, brush_hove, rect_button, s_f);
                            }
                        }
                        else if (hover_button.Switch)
                        {
                            using (var brush_hove = new SolidBrush(Style.Db.PrimaryActive))
                            {
                                g.String(button_text, Font, brush_hove, rect_button, s_f);
                            }
                        }
                        else g.String(button_text, Font, brush_active, rect_button, s_f);
                    }
                    if (badge_list.Count > 0)
                    {
                        using (var font = new Font(Font.FontFamily, Font.Size * BadgeSize))
                        {
                            foreach (var it in datas)
                            {
                                if (badge_list.TryGetValue(it.date_str, out var find)) this.PaintBadge(find, font, it.rect, g);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制今天边框
        /// </summary>
        internal static void PaintToDayFrame(Canvas g, IList<Calendari> datas, string dateNow, float radius)
        {
            foreach (var it in datas)
            {
                if (dateNow == it.date_str)
                {
                    using (var path = it.rect_read.RoundPath(radius))
                    {
                        g.Draw(Style.Db.Primary, Config.Dpi, path);
                    }
                    return;
                }
            }
        }

        #endregion

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding);
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = radius * Config.Dpi;
                return rect_read.RoundPath(_radius);
            }
        }

        #endregion

        #endregion

        #region 鼠标

        ITaskOpacity hover_button, hover_lefts, hover_left, hover_rights, hover_right, hover_year, hover_month;
        Rectangle rect_button = new Rectangle(-20, -20, 10, 10);
        Rectangle rect_lefts = new Rectangle(-20, -20, 10, 10), rect_left = new Rectangle(-20, -20, 10, 10);
        Rectangle rect_rights = new Rectangle(-20, -20, 10, 10), rect_right = new Rectangle(-20, -20, 10, 10);
        Rectangle rect_year = new Rectangle(-20, -20, 10, 10), rect_year2 = new Rectangle(-20, -20, 10, 10), rect_month = new Rectangle(-20, -20, 10, 10);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int count = 0, hand = 0;
            bool _hover_lefts = rect_lefts.Contains(e.X, e.Y),
             _hover_rights = rect_rights.Contains(e.X, e.Y),
             _hover_left = (showType == 0 && rect_left.Contains(e.X, e.Y)),
             _hover_right = (showType == 0 && rect_right.Contains(e.X, e.Y)),
             _hover_button = (showType == 0 && showButtonToDay && rect_button.Contains(e.X, e.Y));

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

            hover_lefts.Switch = _hover_lefts;
            hover_left.Switch = _hover_left;
            hover_rights.Switch = _hover_rights;
            hover_right.Switch = _hover_right;
            hover_year.Switch = _hover_year;
            hover_month.Switch = _hover_month;
            hover_button.Switch = _hover_button;
            if (hover_lefts.Switch || hover_left.Switch || hover_rights.Switch || hover_right.Switch || hover_year.Switch || hover_month.Switch || hover_button.Switch) hand++;
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
                            if (it.hover) hand++;
                        }
                    }
                }
            }
            if (count > 0) Invalidate();
            SetCursor(hand > 0);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hover_lefts.Switch = false;
            hover_left.Switch = false;
            hover_rights.Switch = false;
            hover_right.Switch = false;
            hover_year.Switch = false;
            hover_month.Switch = false;
            hover_button.Switch = false;
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
            SetCursor(false);
            Invalidate();
            base.OnMouseLeave(e);
        }

        int showType = 0;
        void ChangeType(int type)
        {
            if (type == showType) return;
            showType = type;
            OnSizeChanged(EventArgs.Empty);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (rect_lefts.Contains(e.X, e.Y))
                {
                    if (hover_lefts.Enable)
                    {
                        if (showType == 2) Date = _Date.AddYears(-10);
                        else Date = _Date.AddYears(-1);
                        Invalidate();
                    }
                    return;
                }
                else if (rect_rights.Contains(e.X, e.Y))
                {
                    if (hover_rights.Enable)
                    {
                        if (showType == 2) Date = _Date.AddYears(10);
                        else Date = _Date.AddYears(1);
                        Invalidate();
                    }
                    return;
                }
                else if (showType == 0 && rect_left.Contains(e.X, e.Y))
                {
                    if (hover_left.Enable)
                    {
                        Date = _Date.AddMonths(-1);
                        Invalidate();
                    }
                    return;
                }
                else if (showType == 0 && rect_right.Contains(e.X, e.Y))
                {
                    if (hover_right.Enable)
                    {
                        Date = _Date.AddMonths(1);
                        Invalidate();
                    }
                    return;
                }
                else if ((showType == 0 && rect_year.Contains(e.X, e.Y)) || (showType != 0 && rect_year2.Contains(e.X, e.Y)))
                {
                    ChangeType(2);
                    return;
                }
                else if (showType == 0 && showButtonToDay && rect_button.Contains(e.X, e.Y))
                {
                    Value = Date = DateNow = DateTime.Now;
                    return;
                }
                else if (rect_month.Contains(e.X, e.Y))
                {
                    ChangeType(1);
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
                                    ChangeType(0);
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
                                    ChangeType(1);
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
                                    Value = it.date;
                                    return;
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
            if (e.Delta != 0) MouseWheelDay(e);
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
                Invalidate();
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
                Invalidate();
            }
        }


        #endregion

        #region 坐标

        protected override void OnSizeChanged(EventArgs e)
        {
            float dpi = Config.Dpi;

            var rect = ReadRectangle;

            int t_top = 34, t_button = showButtonToDay ? 38 : 0;
            int year_width = 60, year2_width = 88, month_width = 40;
            if (dpi != 1F)
            {
                t_top = (int)(t_top * dpi);
                if (showButtonToDay) t_button = (int)(t_button * dpi);
                year_width = (int)(year_width * dpi);
                year2_width = (int)(year2_width * dpi);
                month_width = (int)(month_width * dpi);
            }

            rect_lefts = new Rectangle(rect.X, rect.Y, t_top, t_top);
            rect_left = new Rectangle(rect.X + t_top, rect.Y, t_top, t_top);
            rect_rights = new Rectangle(rect.X + rect.Width - t_top, rect.Y, t_top, t_top);
            rect_right = new Rectangle(rect.X + rect.Width - t_top * 2, rect.Y, t_top, t_top);

            int gap = (int)(4 * Config.Dpi), xm = rect.Width / 2;
            rect_year2 = new Rectangle(rect.X + (rect.Width - year2_width) / 2, rect.Y, year2_width, t_top);
            rect_button = new Rectangle(rect.X, rect.Bottom - t_button, rect.Width, t_button);
            if (YDR)
            {
                rect_month = new Rectangle(rect.X + xm - year_width - gap, rect.Y, year_width, t_top);
                rect_year = new Rectangle(rect.X + xm + gap, rect.Y, month_width, t_top);
            }
            else
            {
                rect_year = new Rectangle(rect.X + xm - year_width - gap, rect.Y, year_width, t_top);
                rect_month = new Rectangle(rect.X + xm + gap, rect.Y, month_width, t_top);
            }

            #region 计算坐标

            int gap_day = (int)(8 * dpi), gap_day2 = gap_day * 2;
            if (showType == 1)
            {
                //月
                rect_month_l = new Rectangle(rect.X, rect.Y, rect.Width, t_top);
                int y = rect.Y + t_top;
                int size_w = (rect.Width - gap_day2) / 3, size_h = (rect.Height - t_top - gap_day2) / 4;
                if (calendar_month != null)
                {
                    foreach (var it in calendar_month) it.rect = new Rectangle(rect.X + gap_day + (size_w * it.x), y + gap_day + (size_h * it.y), size_w, size_h);
                }
            }
            else if (showType == 2)
            {
                //年
                rect_year_l = new Rectangle(rect.X, rect.Y, rect.Width, t_top);
                int y = rect.Y + t_top;
                int size_w = (rect.Width - gap_day2) / 3, size_h = (rect.Height - t_top - gap_day2) / 4;
                if (calendar_year != null)
                {
                    foreach (var it in calendar_year) it.rect = new Rectangle(rect.X + gap_day + (size_w * it.x), y + gap_day + (size_h * it.y), size_w, size_h);
                }
            }
            else
            {
                if (chinese)
                {
                    int y = rect.Y + t_top + 12;
                    int size_w = (rect.Width - gap_day2) / 7, size_h = (rect.Height - t_top - t_button - gap_day2) / 7;

                    rect_day_split1 = new RectangleF(rect.X, rect.Y + t_top, rect.Width, Config.Dpi);
                    if (showButtonToDay) rect_day_split2 = new RectangleF(rect.X, rect_button.Y - .5F, rect.Width, Config.Dpi);

                    rect_day_s = new Rectangle[]{
                        new Rectangle(rect.X + gap_day, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 2, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 3, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 4, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 5, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 6, y, size_w, size_h)
                    };
                    y += size_h;
                    if (calendar_day != null)
                    {
                        foreach (var it in calendar_day)
                        {
                            it.SetRectG(new Rectangle(rect.X + gap_day + (size_w * it.x), y + (size_h * it.y), size_w, size_h), .92F);
                            it.rect_f = new Rectangle(it.rect_read.X, it.rect_read.Y, it.rect_read.Width, it.rect_read.Height - it.rect_read.Height / 4);
                            it.rect_l = new Rectangle(it.rect_read.X, it.rect_read.Y + it.rect_read.Height / 2, it.rect_read.Width, it.rect_read.Height / 2);
                        }
                    }
                }
                else if (full)
                {
                    int y = rect.Y + t_top + 12;
                    int size_w = (rect.Width - gap_day2) / 7, size_h = (rect.Height - t_top - t_button - gap_day2) / 7;

                    rect_day_split1 = new RectangleF(rect.X, rect.Y + t_top, rect.Width, Config.Dpi);
                    if (showButtonToDay) rect_day_split2 = new RectangleF(rect.X, rect_button.Y - .5F, rect.Width, Config.Dpi);

                    rect_day_s = new Rectangle[]{
                        new Rectangle(rect.X + gap_day, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 2, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 3, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 4, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 5, y, size_w, size_h),
                        new Rectangle(rect.X + gap_day + size_w * 6, y, size_w, size_h)
                    };
                    y += size_h;
                    if (calendar_day != null)
                    {
                        foreach (var it in calendar_day) it.SetRectG(new Rectangle(rect.X + gap_day + (size_w * it.x), y + (size_h * it.y), size_w, size_h), .92F);
                    }
                }
                else
                {
                    int y = rect.Y + t_top + 12;
                    int size_w = (rect.Width - gap_day2) / 7, size_h = (rect.Height - t_top - t_button - gap_day2) / 7, size = size_w;
                    if (size_w > size_h)
                    {
                        size = size_h;
                        gap_day2 = rect.Width - size * 7;
                        gap_day = gap_day2 / 2;
                    }

                    rect_day_split1 = new RectangleF(rect.X, rect.Y + t_top, rect.Width, Config.Dpi);
                    if (showButtonToDay) rect_day_split2 = new RectangleF(rect.X, rect_button.Y - .5F, rect.Width, Config.Dpi);

                    rect_day_s = new Rectangle[]{
                        new Rectangle(rect.X + gap_day, y, size, size),
                        new Rectangle(rect.X + gap_day + size, y, size, size),
                        new Rectangle(rect.X + gap_day + size * 2, y, size, size),
                        new Rectangle(rect.X + gap_day + size * 3, y, size, size),
                        new Rectangle(rect.X + gap_day + size * 4, y, size, size),
                        new Rectangle(rect.X + gap_day + size * 5, y, size, size),
                        new Rectangle(rect.X + gap_day + size * 6, y, size, size)
                    };
                    y += size;
                    if (calendar_day != null)
                    {
                        int size_one = (int)(size * .666F);
                        foreach (var it in calendar_day) it.SetRect(new Rectangle(rect.X + gap_day + (size * it.x), y + (size * it.y), size, size), size_one);
                    }
                }
            }

            #endregion

            base.OnSizeChanged(e);
        }

        #endregion

        #region 事件

        [Description("日期 改变时发生"), Category("行为")]
        public event DateTimeEventHandler? DateChanged;

        #endregion

        protected override void Dispose(bool disposing)
        {
            hover_lefts?.Dispose(); hover_left?.Dispose(); hover_rights?.Dispose(); hover_right?.Dispose(); hover_year?.Dispose(); hover_month?.Dispose(); hover_button?.Dispose();
            base.Dispose(disposing);
        }
    }
}