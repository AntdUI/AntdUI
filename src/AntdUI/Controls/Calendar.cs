﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
                DateChanged?.Invoke(this, _value);
                Invalidate();
                LoadBadge();
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

                OnSizeChanged(EventArgs.Empty);

                LoadBadge();
            }
        }

        string year_str = "";
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

        #endregion

        #endregion

        #region 渲染

        StringFormat stringFormatC = Helper.SF();
        StringFormat stringFormatL = Helper.SF(lr: StringAlignment.Far);
        StringFormat stringFormatLE = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        StringFormat stringFormatR = Helper.SF(lr: StringAlignment.Near);
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect = ClientRectangle;
            var rect_read = ReadRectangle;

            using (var path = rect_read.RoundPath(radius * Config.Dpi))
            {
                using (var brush = new SolidBrush(Style.Db.BgElevated))
                {
                    g.FillPath(brush, path);
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
                    using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_lefts.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
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
                    using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_rights.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
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
                        using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_left.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
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
                        using (var pen_arrow_hovers = new Pen(Helper.ToColor(hover_right.Value, pen_arrow_hover.Color), pen_arrow_hover.Width))
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

            if (showType == 1 && calendar_month != null) PrintMonth(g, rect_read, radius, calendar_month);
            else if (showType == 2 && calendar_year != null) PrintYear(g, rect_read, radius, calendar_year);
            else if (calendar_day != null) PrintDay(g, rect_read, radius, calendar_day);

            base.OnPaint(e);
        }

        #region 渲染帮助

        #region 年模式

        RectangleF rect_year_l;
        /// <summary>
        /// 渲染年模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintYear(Graphics g, Rectangle rect_read, float radius, List<Calendari> datas)
        {
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    if (hover_year.Animation)
                    {
                        g.DrawString(year_str, font, brush_fore, rect_year_l, stringFormatC);
                        using (var brush_hove = new SolidBrush(Helper.ToColor(hover_year.Value, Style.Db.Primary)))
                        {
                            g.DrawString(year_str, font, brush_hove, rect_year_l, stringFormatC);
                        }
                    }
                    else if (hover_year.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(year_str, font, brush_hove, rect_year_l, stringFormatC);
                        }
                    }
                    else g.DrawString(year_str, font, brush_fore, rect_year_l, stringFormatC);
                }
                using (var brush_fore_disable = new SolidBrush(Style.Db.TextQuaternary))
                {
                    foreach (var it in datas)
                    {
                        using (var path = it.rect_read.RoundPath(radius))
                        {
                            if (_value.ToString("yyyy") == it.date_str)
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

        RectangleF rect_month_l;
        /// <summary>
        /// 渲染月模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintMonth(Graphics g, Rectangle rect_read, float radius, List<Calendari> datas)
        {
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    if (hover_year.Animation)
                    {
                        g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_month_l, stringFormatC);
                        using (var brush_hove = new SolidBrush(Helper.ToColor(hover_year.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_month_l, stringFormatC);
                        }
                    }
                    else if (hover_year.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_month_l, stringFormatC);
                        }
                    }
                    else g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_month_l, stringFormatC);
                }

                foreach (var it in datas)
                {
                    using (var path = it.rect_read.RoundPath(radius))
                    {
                        if (_value.ToString("yyyy-MM") == it.date_str)
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

        RectangleF rect_day_l, rect_day_r, rect_day_split1, rect_day_split2;
        RectangleF[]? rect_day_s;

        /// <summary>
        /// 渲染天模式
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_read">真实区域</param>
        /// <param name="datas">数据</param>
        void PrintDay(Graphics g, Rectangle rect_read, float radius, List<Calendari> datas)
        {
            if (rect_day_s == null) return;
            using (var brush_fore = new SolidBrush(Style.Db.TextBase))
            {
                using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    if (hover_year.Animation)
                    {
                        g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_day_l, stringFormatL);
                        using (var brush_hove = new SolidBrush(Helper.ToColor(hover_year.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_day_l, stringFormatL);
                        }
                    }
                    else if (hover_year.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_hove, rect_day_l, stringFormatL);
                        }
                    }
                    else g.DrawString(_Date.ToString("yyyy") + YearButton, font, brush_fore, rect_day_l, stringFormatL);

                    if (hover_month.Animation)
                    {
                        g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_fore, rect_day_r, stringFormatR);
                        using (var brush_hove = new SolidBrush(Helper.ToColor(hover_month.Value, Style.Db.Primary)))
                        {
                            g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_hove, rect_day_r, stringFormatR);
                        }
                    }
                    else if (hover_month.Switch)
                    {
                        using (var brush_hove = new SolidBrush(Style.Db.Primary))
                        {
                            g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_hove, rect_day_r, stringFormatR);
                        }
                    }
                    else g.DrawString(_Date.ToString("MM") + MonthButton, font, brush_fore, rect_day_r, stringFormatR);
                }

                using (var brush_split = new SolidBrush(Style.Db.Split))
                {
                    g.FillRectangle(brush_split, rect_day_split1);
                    g.FillRectangle(brush_split, rect_day_split2);
                }
                using (var brush = new SolidBrush(Style.Db.Text))
                {
                    g.DrawString(MondayButton, Font, brush, rect_day_s[0], stringFormatC);
                    g.DrawString(TuesdayButton, Font, brush, rect_day_s[1], stringFormatC);
                    g.DrawString(WednesdayButton, Font, brush, rect_day_s[2], stringFormatC);
                    g.DrawString(ThursdayButton, Font, brush, rect_day_s[3], stringFormatC);
                    g.DrawString(FridayButton, Font, brush, rect_day_s[4], stringFormatC);
                    g.DrawString(SaturdayButton, Font, brush, rect_day_s[5], stringFormatC);
                    g.DrawString(SundayButton, Font, brush, rect_day_s[6], stringFormatC);
                }
                using (var brush_fore_disable = new SolidBrush(Style.Db.TextQuaternary))
                using (var brush_active = new SolidBrush(Style.Db.Primary))
                using (var brush_active_fore = new SolidBrush(Style.Db.PrimaryColor))
                using (var brush_error = new SolidBrush(Style.Db.Error))
                {
                    if (chinese)
                    {
                        foreach (var it in datas)
                        {
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str)
                            {
                                using (var path = it.rect_read.RoundPath(radius))
                                {
                                    using (var pen_active = new Pen(Style.Db.Primary, 0.1F))
                                    {
                                        g.DrawPath(pen_active, path);
                                    }
                                }
                            }
                        }
                        using (var font4 = new Font(Font.FontFamily, Font.Size * 0.76F, Font.Style))
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
                                            g.FillPath(brush_active, path);
                                            g.DrawString(cdate.DayString, font4, brush_active_fore, it.rect_l, stringFormatC);
                                            g.DrawString(it.v, Font, brush_active_fore, it.rect_f, stringFormatC);
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
                                            g.DrawString(cdate.DayString, font4, brush_fore_c, it.rect_l, stringFormatC);
                                            g.DrawString(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect_f, stringFormatC);
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
                            if (DateNow.ToString("yyyy-MM-dd") == it.date_str)
                            {
                                using (var path = it.rect_read.RoundPath(radius))
                                {
                                    using (var pen_active = new Pen(Style.Db.Primary, 0.1F))
                                    {
                                        g.DrawPath(pen_active, path);
                                    }
                                }
                            }
                        }
                        foreach (var it in datas)
                        {
                            using (var path = it.rect_read.RoundPath(radius))
                            {
                                if (_value.ToString("yyyy-MM-dd") == it.date_str)
                                {
                                    g.FillPath(brush_active, path);
                                    g.DrawString(it.v, Font, brush_active_fore, it.rect, stringFormatC);
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
                                    g.DrawString(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, it.rect, stringFormatC);
                                }
                            }
                        }
                    }
                    if (hover_button.Animation)
                    {
                        g.DrawString(button_text, Font, brush_active, rect_button, stringFormatC);
                        using (var brush_hove = new SolidBrush(Helper.ToColor(hover_button.Value, Style.Db.PrimaryActive)))
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
        RectangleF rect_button = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_lefts = new RectangleF(-20, -20, 10, 10), rect_left = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_rights = new RectangleF(-20, -20, 10, 10), rect_right = new RectangleF(-20, -20, 10, 10);
        RectangleF rect_year = new RectangleF(-20, -20, 10, 10), rect_year2 = new RectangleF(-20, -20, 10, 10), rect_month = new RectangleF(-20, -20, 10, 10);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int count = 0, hand = 0;
            bool _hover_lefts = rect_lefts.Contains(e.Location),
             _hover_rights = rect_rights.Contains(e.Location),
             _hover_left = (showType == 0 && rect_left.Contains(e.Location)),
             _hover_right = (showType == 0 && rect_right.Contains(e.Location)),
             _hover_button = (showType == 0 && rect_button.Contains(e.Location));

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

            hover_lefts.Switch = _hover_lefts;
            hover_left.Switch = _hover_left;
            hover_rights.Switch = _hover_rights;
            hover_right.Switch = _hover_right;
            hover_year.Switch = _hover_year;
            hover_month.Switch = _hover_month;
            hover_button.Switch = _hover_button;
            if (hover_lefts.Switch || hover_left.Switch || hover_rights.Switch || hover_right.Switch || hover_year.Switch || hover_month.Switch || hover_button.Switch)
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
                if (rect_lefts.Contains(e.Location))
                {
                    if (showType == 2) Date = _Date.AddYears(-10);
                    else Date = _Date.AddYears(-1);
                    Invalidate();
                    return;
                }
                else if (rect_rights.Contains(e.Location))
                {
                    if (showType == 2) Date = _Date.AddYears(10);
                    else Date = _Date.AddYears(1);
                    Invalidate();
                    return;
                }
                else if (showType == 0 && rect_left.Contains(e.Location))
                {
                    Date = _Date.AddMonths(-1);
                    Invalidate();
                    return;
                }
                else if (showType == 0 && rect_right.Contains(e.Location))
                {
                    Date = _Date.AddMonths(1);
                    Invalidate();
                    return;
                }
                else if ((showType == 0 && rect_year.Contains(e.Location)) || (showType != 0 && rect_year2.Contains(e.Location)))
                {
                    ChangeType(2);
                    return;
                }
                else if (showType == 0 && rect_button.Contains(e.Location))
                {
                    Value = Date = DateNow = DateTime.Now;
                    return;
                }
                else if (rect_month.Contains(e.Location))
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
                                if (it.rect.Contains(e.Location))
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
                                if (it.rect.Contains(e.Location))
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
                                if (it.rect.Contains(e.Location))
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
                if (showType == 1) Date = _Date.AddYears(-1);
                else if (showType == 2) Date = _Date.AddYears(-10);
                else Date = _Date.AddMonths(-1);
                Invalidate();
            }
            else
            {
                if (showType == 1) Date = _Date.AddYears(1);
                else if (showType == 2) Date = _Date.AddYears(10);
                else Date = _Date.AddMonths(1);
                Invalidate();
            }
        }


        #endregion

        #region 坐标

        protected override void OnSizeChanged(EventArgs e)
        {
            float dpi = Config.Dpi;

            var rect = ReadRectangle;

            int t_top = 34, t_button = 38;
            int year_width = 60, year2_width = 88, month_width = 40;
            if (dpi != 1F)
            {
                t_top = (int)(t_top * dpi);
                t_button = (int)(t_button * dpi);
                year_width = (int)(year_width * dpi);
                year2_width = (int)(year2_width * dpi);
                month_width = (int)(month_width * dpi);
            }

            rect_lefts = new RectangleF(rect.X, rect.Y, t_top, t_top);
            rect_left = new RectangleF(rect.X + t_top, rect.Y, t_top, t_top);
            rect_rights = new RectangleF(rect.X + rect.Width - t_top, rect.Y, t_top, t_top);
            rect_right = new RectangleF(rect.X + rect.Width - t_top * 2, rect.Y, t_top, t_top);

            rect_year = new RectangleF(rect.X + rect.Width / 2 - year_width, rect.Y, year_width, t_top);
            rect_year2 = new RectangleF(rect.X + (rect.Width - year2_width) / 2, rect.Y, year2_width, t_top);
            rect_month = new RectangleF(rect.X + rect.Width / 2, rect.Y, month_width, t_top);
            rect_button = new RectangleF(rect.X, rect.Bottom - t_button, rect.Width, t_button);

            #region 计算坐标

            float gap_day = (int)(8 * dpi), gap_day2 = gap_day * 2;
            if (showType == 1)
            {
                //月
                rect_month_l = new RectangleF(rect.X, rect.Y, rect.Width, t_top);
                float y = rect.Y + t_top;
                float size_w = (rect.Width - gap_day2) / 3F, size_h = (rect.Height - t_top - gap_day2) / 4F;
                if (calendar_month != null)
                {
                    foreach (var it in calendar_month) it.rect = new RectangleF(rect.X + gap_day + (size_w * it.x), y + gap_day + (size_h * it.y), size_w, size_h);
                }
            }
            else if (showType == 2)
            {
                //年
                rect_year_l = new RectangleF(rect.X, rect.Y, rect.Width, t_top);
                float y = rect.Y + t_top;
                float size_w = (rect.Width - gap_day2) / 3F, size_h = (rect.Height - t_top - gap_day2) / 4F;
                if (calendar_year != null)
                {
                    foreach (var it in calendar_year) it.rect = new RectangleF(rect.X + gap_day + (size_w * it.x), y + gap_day + (size_h * it.y), size_w, size_h);
                }
            }
            else
            {
                if (chinese)
                {
                    float y = rect.Y + t_top + 12, xm = rect.Width / 2F;
                    float size_w = (rect.Width - gap_day2) / 7F, size_h = (rect.Height - t_top - t_button - gap_day2) / 7F;

                    rect_day_l = new RectangleF(rect.X, rect.Y, xm, t_top);
                    rect_day_r = new RectangleF(rect.X + xm, rect.Y, xm, t_top);
                    rect_day_split1 = new RectangleF(rect.X, rect.Y + t_top, rect.Width, 1F);
                    rect_day_split2 = new RectangleF(rect.X, rect_button.Y - 0.5F, rect.Width, 1);

                    rect_day_s = new RectangleF[]{
                        new RectangleF(rect.X + gap_day, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 2F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 3F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 4F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 5F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 6F, y, size_w, size_h)
                    };
                    y += size_h;
                    if (calendar_day != null)
                    {
                        foreach (var it in calendar_day)
                        {
                            it.SetRectG(new RectangleF(rect.X + gap_day + (size_w * it.x), y + (size_h * it.y), size_w, size_h), 0.92F);
                            it.rect_f = new RectangleF(it.rect_read.X, it.rect_read.Y, it.rect_read.Width, it.rect_read.Height - it.rect_read.Height / 4);
                            it.rect_l = new RectangleF(it.rect_read.X, it.rect_read.Y + it.rect_read.Height / 2, it.rect_read.Width, it.rect_read.Height / 2);
                        }
                    }
                }
                else if (full)
                {
                    float y = rect.Y + t_top + 12, xm = rect.Width / 2F;
                    float size_w = (rect.Width - gap_day2) / 7F, size_h = (rect.Height - t_top - t_button - gap_day2) / 7F;

                    rect_day_l = new RectangleF(rect.X, rect.Y, xm, t_top);
                    rect_day_r = new RectangleF(rect.X + xm, rect.Y, xm, t_top);
                    rect_day_split1 = new RectangleF(rect.X, rect.Y + t_top, rect.Width, 1F);
                    rect_day_split2 = new RectangleF(rect.X, rect_button.Y - 0.5F, rect.Width, 1);

                    rect_day_s = new RectangleF[]{
                        new RectangleF(rect.X + gap_day, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 2F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 3F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 4F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 5F, y, size_w, size_h),
                        new RectangleF(rect.X + gap_day + size_w * 6F, y, size_w, size_h)
                    };
                    y += size_h;
                    if (calendar_day != null)
                    {
                        foreach (var it in calendar_day) it.SetRectG(new RectangleF(rect.X + gap_day + (size_w * it.x), y + (size_h * it.y), size_w, size_h), 0.92F);
                    }
                }
                else
                {
                    float y = rect.Y + t_top + 12, xm = rect.Width / 2F;
                    float size_w = (rect.Width - gap_day2) / 7F, size_h = (rect.Height - t_top - t_button - gap_day2) / 7F;
                    float size = size_w;
                    if (size_w > size_h)
                    {
                        size = size_h;
                        gap_day2 = rect.Width - size * 7F;
                        gap_day = gap_day2 / 2;
                    }

                    rect_day_l = new RectangleF(rect.X, rect.Y, xm, t_top);
                    rect_day_r = new RectangleF(rect.X + xm, rect.Y, xm, t_top);
                    rect_day_split1 = new RectangleF(rect.X, rect.Y + t_top, rect.Width, 1F);
                    rect_day_split2 = new RectangleF(rect.X, rect_button.Y - 0.5F, rect.Width, 1);

                    rect_day_s = new RectangleF[]{
                        new RectangleF(rect.X + gap_day, y, size, size),
                        new RectangleF(rect.X + gap_day + size, y, size, size),
                        new RectangleF(rect.X + gap_day + size * 2F, y, size, size),
                        new RectangleF(rect.X + gap_day + size * 3F, y, size, size),
                        new RectangleF(rect.X + gap_day + size * 4F, y, size, size),
                        new RectangleF(rect.X + gap_day + size * 5F, y, size, size),
                        new RectangleF(rect.X + gap_day + size * 6F, y, size, size)
                    };
                    y += size;
                    if (calendar_day != null)
                    {
                        float size_one = size * 0.666F;
                        foreach (var it in calendar_day) it.SetRect(new RectangleF(rect.X + gap_day + (size * it.x), y + (size * it.y), size, size), size_one);
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