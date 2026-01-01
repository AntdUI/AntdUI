// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormDatePickerRange : ILayeredShadowForm
    {
        #region 初始化

        DatePickerRange control;
        DateTime? minDate, maxDate;
        TAMode ColorScheme;
        public LayeredFormDatePickerRange(DatePickerRange _control, bool endFocused, int bar, Action<DateTime[]> _action, Action<object> _action_btns, Func<DateTime[], List<DateBadge>?>? _badge_action)
        {
            PARENT = control = _control;
            ColorScheme = _control.ColorScheme;
            _control.Parent.SetTopMost(Handle);
            Font = _control.Font;
            minDate = _control.MinDate;
            maxDate = _control.MaxDate;
            if (_control.InteractiveReset) EndFocused = false;
            else EndFocused = endFocused;
            AnimationBarValue = bar;

            showType = PickerType = _control.Picker;
            badge_action = _badge_action;
            action = _action;
            action_btns = _action_btns;

            if (_control.Presets.Count > 0)
            {
                ScrollButtons = new ScrollBar(this, control.ColorScheme);
                int count = _control.Presets.Count;
                left_buttons = new List<CalendarButton>(count);
                for (int i = 0; i < count; i++) left_buttons.Add(new CalendarButton(i, _control.Presets[i]!));
            }

            #region 国际化

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
                s_f_L = FormatFlags.Left | FormatFlags.VerticalCenter; s_f_R = FormatFlags.Right | FormatFlags.VerticalCenter;
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
                s_f_L = FormatFlags.Right | FormatFlags.VerticalCenter; s_f_R = FormatFlags.Left | FormatFlags.VerticalCenter;
            }

            #endregion

            if (Dpi == 1F) Radius = _control.radius;
            else
            {
                ArrowSize = (int)(8 * Dpi);
                Radius = (int)(_control.radius * Dpi);
            }
            SelDate = _control.Value;
            Date = SelDate == null ? DateTime.Now : SelDate[0];
            if (SelDate != null && SelDate.Length > 1)
            {
                if (endFocused)
                {
                    oldTime = SelDate[1];
                    oldTimeHover = SelDate[0];
                }
                else
                {
                    oldTime = SelDate[0];
                    oldTimeHover = SelDate[1];
                }
            }

            LoadLayout();

            CLocation(_control, _control.Placement, _control.DropDownArrow, ArrowSize);
            if (OS.Win7OrLower) Select();
        }

        #region 箭头

        int ArrowSize = 8;

        float AnimationBarValue = 0;
        public void SetArrow(float x)
        {
            if (AnimationBarValue == x) return;
            AnimationBarValue = x;
            if (RunAnimation) DisposeTmp();
            else Print();
        }

        public bool EndFocused = false;

        public void SetDateS(DateTime date, bool r = true)
        {
            EndFocused = true;
            if (control.Value != null && control.Value.Length > 1)
            {
                oldTime = date;
                if (r) action(new DateTime[] { control.Value[0], date });
            }
            else oldTimeHover = oldTime = date;
            if (r)
            {
                Date = date;
                Print();
            }
        }

        public void SetDateE(DateTime sdate, DateTime edate, bool r = true)
        {
            EndFocused = false;
            DateTime[] dates;
            if (sdate == edate) dates = new DateTime[] { edate, edate };
            else if (sdate < edate) dates = new DateTime[] { sdate, edate };
            else dates = new DateTime[] { edate, sdate };
            if (r)
            {
                oldTime = sdate;
                oldTimeHover = edate;
                Date = edate;
                Print();
            }
            else action(dates);
        }
        bool SetDate(ItemCalendari item)
        {
            if (EndFocused && oldTime.HasValue)
            {
                SetDateE(oldTime.Value, item.date, false);
                return false;
            }
            SetDateS(item.date, false);
            Print();
            return true;
        }

        #endregion

        public DateTime[]? SelDate;
        DateTime? oldTime, oldTimeHover;

        DateTime _Date, _Date_R;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;

                calendar_day = CalendarHelper.Day(value, minDate, maxDate);
                if (PickerType == TDatePicker.Date)
                {
                    _Date_R = value.AddMonths(1);
                    calendar_day2 = CalendarHelper.Day(_Date_R, minDate, maxDate);
                }

                if (PickerType == TDatePicker.Month)
                {
                    _Date_R = value.AddYears(1);
                    calendar_month2 = CalendarHelper.Month(_Date_R, minDate, maxDate, Culture, MonthFormat);
                }
                calendar_month = CalendarHelper.Month(value, minDate, maxDate, Culture, MonthFormat);

                if (PickerType == TDatePicker.Year)
                {
                    _Date_R = value.AddYears(12);
                    calendar_year2 = CalendarHelper.Year(_Date_R, minDate, maxDate, out year_str2);
                }
                calendar_year = CalendarHelper.Year(value, minDate, maxDate, out year_str);

                rect_left.Enable = Helper.DateExceedMonth(value.AddMonths(-1), minDate, maxDate);
                rect_right.Enable = Helper.DateExceedMonth(value.AddMonths(1), minDate, maxDate);
                rect_lefts.Enable = Helper.DateExceedYear(value.AddYears(-1), minDate, maxDate);
                rect_rights.Enable = Helper.DateExceedYear(value.AddYears(1), minDate, maxDate);
                LoadLayoutDiv();

                if (badge_action == null) return;
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
        }

        #region 回调

        /// <summary>
        /// 回调
        /// </summary>
        Action<DateTime[]> action;
        Action<object> action_btns;
        Func<DateTime[], List<DateBadge>?>? badge_action;
        Dictionary<string, DateBadge> badge_list = new Dictionary<string, DateBadge>();

        #endregion

        List<ItemCalendari>? calendar_year, calendar_month, calendar_day;
        List<ItemCalendari>? calendar_year2, calendar_month2, calendar_day2;
        List<CalendarButton>? left_buttons;
        ScrollBar? ScrollButtons;

        string year_str = "", year_str2 = "";

        public override string name => nameof(DatePicker);

        CultureInfo Culture;
        string CultureID = Localization.Get("ID", "zh-CN"),
            YearFormat, MonthFormat,
            MondayButton, TuesdayButton, WednesdayButton, ThursdayButton, FridayButton, SaturdayButton, SundayButton;

        bool YDR = false;

        #endregion

        #region 渲染

        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(Colour.BgElevated.Get(name, ColorScheme)))
            {
                g.Fill(brush, path);
                if (shadow == 0)
                {
                    int bor = (int)(Dpi), bor2 = bor * 2;
                    using (var path2 = new Rectangle(rect.X + bor, rect.Y + bor, rect.Width - bor2, rect.Height - bor2).RoundPath(Radius))
                    {
                        g.Draw(Colour.BorderColor.Get(name, ColorScheme), bor, path2);
                    }
                    return;
                }
                if (ArrowLine != null)
                {
                    if (AnimationBarValue != 0F)
                    {
                        var state = g.Save();
                        g.TranslateTransform(AnimationBarValue, 0);
                        g.FillPolygon(brush, ArrowLine);
                        g.Restore(state);
                    }
                    else g.FillPolygon(brush, ArrowLine);
                }
            }
        }

        readonly FormatFlags s_f = FormatFlags.Center, s_f_LE = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.EllipsisCharacter;
        FormatFlags s_f_L, s_f_R;
        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            using (var pen_arrow = new Pen(Colour.TextTertiary.Get(name, ColorScheme), 1.6F * Dpi))
            using (var pen_arrow_hover = new Pen(Colour.Text.Get(name, ColorScheme), pen_arrow.Width))
            using (var pen_arrow_enable = new Pen(Colour.FillSecondary.Get(name, ColorScheme), pen_arrow.Width))
            {
                if (rect_lefts.Hover)
                {
                    g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(rect_lefts.Rects[0], .26F));
                    g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(rect_lefts.Rects[1], .26F));
                }
                else if (rect_lefts.Enable)
                {
                    g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(rect_lefts.Rects[0], .26F));
                    g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(rect_lefts.Rects[1], .26F));
                }
                else
                {
                    g.DrawLines(pen_arrow_enable, TAlignMini.Left.TriangleLines(rect_lefts.Rects[0], .26F));
                    g.DrawLines(pen_arrow_enable, TAlignMini.Left.TriangleLines(rect_lefts.Rects[1], .26F));
                }

                if (rect_rights.Hover)
                {
                    g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(rect_rights.Rects[0], .26F));
                    g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(rect_rights.Rects[1], .26F));
                }
                else if (rect_rights.Enable)
                {
                    g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(rect_rights.Rects[0], .26F));
                    g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(rect_rights.Rects[1], .26F));
                }
                else
                {
                    g.DrawLines(pen_arrow_enable, TAlignMini.Right.TriangleLines(rect_rights.Rects[0], .26F));
                    g.DrawLines(pen_arrow_enable, TAlignMini.Right.TriangleLines(rect_rights.Rects[1], .26F));
                }

                if (showType == TDatePicker.Date)
                {
                    if (rect_left.Hover) g.DrawLines(pen_arrow_hover, TAlignMini.Left.TriangleLines(rect_left.Rect, .26F));
                    else if (rect_left.Enable) g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(rect_left.Rect, .26F));
                    else g.DrawLines(pen_arrow_enable, TAlignMini.Left.TriangleLines(rect_left.Rect, .26F));

                    if (rect_right.Hover) g.DrawLines(pen_arrow_hover, TAlignMini.Right.TriangleLines(rect_right.Rect, .26F));
                    else if (rect_right.Enable) g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(rect_right.Rect, .26F));
                    else g.DrawLines(pen_arrow_enable, TAlignMini.Right.TriangleLines(rect_right.Rect, .26F));
                }
            }
            if (showType == PickerType)
            {
                if (left_buttons != null)
                {
                    g.TranslateTransform(0, -ScrollButtons!.Value);
                    using (var brush_fore = new SolidBrush(Colour.TextBase.Get(name, ColorScheme)))
                    {
                        foreach (var it in left_buttons)
                        {
                            using (var path = it.rect_read.RoundPath(Radius))
                            {
                                if (it.hover) g.Fill(Colour.FillTertiary.Get(name, ColorScheme), path);
                                g.String(it.v, Font, brush_fore, it.rect_text, s_f_LE);
                            }
                        }
                    }
                    g.Restore(state);
                    ScrollButtons.Paint(g, ColorScheme);
                }


                switch (showType)
                {
                    case TDatePicker.Date:
                        PrintDay(g, state, rect, calendar_day!, calendar_day2!);
                        break;
                    case TDatePicker.Month:
                        PrintMonth(g, state, rect, calendar_month!, calendar_month2!);
                        break;
                    case TDatePicker.Year:
                        PrintYear(g, state, rect, calendar_year!, calendar_year2!);
                        break;
                }
            }
            else
            {
                switch (showType)
                {
                    case TDatePicker.Date:
                        PrintDay(g, state, rect, calendar_day!, calendar_day2!);
                        break;
                    case TDatePicker.Month:
                        PrintMonth(g, state, rect, calendar_month!);
                        break;
                    case TDatePicker.Year:
                        PrintYear(g, state, rect, calendar_year!);
                        break;
                }
            }
        }

        #region Year

        void PrintYear(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas)
        {
            var color_fore = Colour.TextBase.Get(name, ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                g.String(year_str, font, color_fore, rect_year2.Rect, s_f);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get(name, ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            PrintYM(g, state, rect_read, datas, "yyyy");
        }
        void PrintYear(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas, List<ItemCalendari> datas2)
        {
            var color_fore = Colour.TextBase.Get(name, ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                g.String(year_str, font, color_fore, rect_year2.Rect, s_f);
                g.String(year_str2, font, color_fore, rect_year2_r.Rect, s_f);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get(name, ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            PrintYM(g, state, rect_read, datas, "yyyy");
            PrintYM(g, state, rect_read, datas2, "yyyy", "R_");
        }

        #endregion

        #region Month

        void PrintMonth(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas)
        {
            var color_fore = Colour.TextBase.Get(name, ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture);
                if (rect_year2.Hover) g.String(yearStr, font, Colour.Primary.Get(name, ColorScheme), rect_year2.Rect, s_f);
                else g.String(yearStr, font, color_fore, rect_year2.Rect, s_f);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get(name, ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            PrintYM(g, state, rect_read, datas, "yyyy-MM");
        }
        void PrintMonth(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas, List<ItemCalendari> datas2)
        {
            var color_fore = Colour.TextBase.Get(name, ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture);
                if (rect_year2.Hover) g.String(yearStr, font, Colour.Primary.Get(name, ColorScheme), rect_year2.Rect, s_f);
                else g.String(yearStr, font, color_fore, rect_year2.Rect, s_f);

                string year2Str = _Date_R.ToString(YearFormat, Culture);
                if (rect_year2_r.Hover) g.String(year2Str, font, Colour.Primary.Get(name, ColorScheme), rect_year2_r.Rect, s_f);
                else g.String(year2Str, font, color_fore, rect_year2_r.Rect, s_f);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get(name, ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            PrintYM(g, state, rect_read, datas, "yyyy-MM");
            PrintYM(g, state, rect_read, datas2, "yyyy-MM", "R_");
        }

        #endregion

        #region Day

        void PrintDay(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas, List<ItemCalendari> datas2)
        {
            var color_fore = Colour.TextBase.Get(name, ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture), monthStr = _Date.ToString(MonthFormat, Culture);
                if (rect_year.Hover) g.String(yearStr, font, Colour.Primary.Get(name, ColorScheme), rect_year.Rect, s_f_L);
                else g.String(yearStr, font, color_fore, rect_year.Rect, s_f_L);

                if (rect_month.Hover) g.String(monthStr, font, Colour.Primary.Get(name, ColorScheme), rect_month.Rect, s_f_R);
                else g.String(monthStr, font, color_fore, rect_month.Rect, s_f_R);

                #region 右

                string year2Str = _Date_R.ToString(YearFormat, Culture), month2Str = _Date_R.ToString(MonthFormat, Culture);
                if (rect_year_r.Hover) g.String(year2Str, font, Colour.Primary.Get(name, ColorScheme), rect_year_r.Rect, s_f_L);
                else g.String(year2Str, font, color_fore, rect_year_r.Rect, s_f_L);

                if (rect_month_r.Hover) g.String(month2Str, font, Colour.Primary.Get(name, ColorScheme), rect_month_r.Rect, s_f_R);
                else g.String(month2Str, font, color_fore, rect_month_r.Rect, s_f_R);

                #endregion
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get(name, ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            using (var brush = new SolidBrush(Colour.Text.Get(name, ColorScheme)))
            {
                g.String(MondayButton, Font, brush, rect_mon, s_f);
                g.String(TuesdayButton, Font, brush, rect_tue, s_f);
                g.String(WednesdayButton, Font, brush, rect_wed, s_f);
                g.String(ThursdayButton, Font, brush, rect_thu, s_f);
                g.String(FridayButton, Font, brush, rect_fri, s_f);
                g.String(SaturdayButton, Font, brush, rect_sat, s_f);
                g.String(SundayButton, Font, brush, rect_sun, s_f);

                g.String(MondayButton, Font, brush, rect_mon2, s_f);
                g.String(TuesdayButton, Font, brush, rect_tue2, s_f);
                g.String(WednesdayButton, Font, brush, rect_wed2, s_f);
                g.String(ThursdayButton, Font, brush, rect_thu2, s_f);
                g.String(FridayButton, Font, brush, rect_fri2, s_f);
                g.String(SaturdayButton, Font, brush, rect_sat2, s_f);
                g.String(SundayButton, Font, brush, rect_sun2, s_f);
            }
            PrintDay(g, state, rect_read, datas);
            PrintDay(g, state, rect_read, datas2, "R_");

        }
        void PrintDay(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas, string? p = null)
        {
            using (var brush_fore = new SolidBrush(Colour.TextBase.Get(name, ColorScheme)))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get(name, ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get(name, ColorScheme)))
            using (var brush_active = new SolidBrush(Colour.Primary.Get(name, ColorScheme)))
            using (var brush_active_bg = new SolidBrush(Colour.PrimaryBg.Get(name, ColorScheme)))
            using (var brush_active_fore = new SolidBrush(Colour.PrimaryColor.Get(name, ColorScheme)))
            {
                var now = DateTime.Now;
                foreach (var it in datas)
                {
                    var rect = rect_div[p + it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        switch (IfDSType(it.date, it.date_str, "yyyy-MM-dd"))
                        {
                            case 0:
                                if (it.enable)
                                {
                                    if (rect.Hover) g.Fill(brush_bg_disable, path);
                                    if (it.t == 1) g.String(it.v, Font, brush_fore, rect_div[p + it.id].Rect, s_f);
                                    else g.String(it.v, Font, brush_fore_disable, rect_div[p + it.id].Rect, s_f);
                                }
                                else
                                {
                                    g.Fill(brush_bg_disable, new Rectangle(rect.Rect.X, rect.RectRead.Y, rect.Rect.Width, rect.RectRead.Height));
                                    g.String(it.v, Font, brush_fore_disable, rect_div[p + it.id].Rect, s_f);
                                }
                                if (now.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get(name, ColorScheme), bor, path);
                                break;
                            case 1:
                                g.Fill(brush_active, path);
                                g.String(it.v, Font, brush_active_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                            case 2:
                                g.Fill(brush_active_bg, new RectangleF(rect.Rect.X, rect.RectRead.Y, rect.Rect.Width, rect.RectRead.Height));
                                g.String(it.v, Font, brush_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                            case 3:// ←
                                g.Fill(brush_active_bg, new RectangleF(rect.RectRead.Right, rect.RectRead.Y, rect.Rect.Width - rect.RectRead.Width, rect.RectRead.Height));
                                using (var path_l = rect.RectRead.RoundPath(Radius, true, false, false, true))
                                {
                                    g.Fill(brush_active, path_l);
                                }
                                g.String(it.v, Font, brush_active_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                            case 4:// →
                                g.Fill(brush_active_bg, new RectangleF(rect.Rect.X, rect.RectRead.Y, rect.RectRead.Width, rect.RectRead.Height));
                                using (var path_r = rect.RectRead.RoundPath(Radius, false, true, true, false))
                                {
                                    g.Fill(brush_active, path_r);
                                }
                                g.String(it.v, Font, brush_active_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                        }
                    }
                }
                if (badge_list.Count > 0)
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, rect_div[p + it.id].Rect, g);
                    }
                }
            }
        }
        void PrintYM(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas, string f, string? p = null)
        {
            using (var brush_fore = new SolidBrush(Colour.TextBase.Get(name, ColorScheme)))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get(name, ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get(name, ColorScheme)))
            using (var brush_active = new SolidBrush(Colour.Primary.Get(name, ColorScheme)))
            using (var brush_active_bg = new SolidBrush(Colour.PrimaryBg.Get(name, ColorScheme)))
            using (var brush_active_fore = new SolidBrush(Colour.PrimaryColor.Get(name, ColorScheme)))
            {
                var now = DateTime.Now;
                foreach (var it in datas)
                {
                    var rect = rect_div[p + it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        switch (IfDSType(it.date, it.date_str, f))
                        {
                            case 0:
                                if (it.enable)
                                {
                                    if (rect.Hover) g.Fill(brush_bg_disable, path);
                                    g.String(it.v, Font, brush_fore, rect_div[p + it.id].Rect, s_f);
                                }
                                else
                                {
                                    g.Fill(brush_bg_disable, new Rectangle(rect.Rect.X, rect.RectRead.Y, rect.Rect.Width, rect.RectRead.Height));
                                    g.String(it.v, Font, brush_fore_disable, rect_div[p + it.id].Rect, s_f);
                                }
                                if (now.ToString(f) == it.date_str) g.Draw(Colour.Primary.Get(name, ColorScheme), bor, path);
                                break;
                            case 1:
                                g.Fill(brush_active, path);
                                g.String(it.v, Font, brush_active_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                            case 2:
                                g.Fill(brush_active_bg, new RectangleF(rect.Rect.X, rect.RectRead.Y, rect.Rect.Width, rect.RectRead.Height));
                                g.String(it.v, Font, brush_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                            case 3:// ←
                                g.Fill(brush_active_bg, new RectangleF(rect.RectRead.Right, rect.RectRead.Y, rect.Rect.Width - rect.RectRead.Width, rect.RectRead.Height));
                                using (var path_l = rect.RectRead.RoundPath(Radius, true, false, false, true))
                                {
                                    g.Fill(brush_active, path_l);
                                }
                                g.String(it.v, Font, brush_active_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                            case 4:// →
                                g.Fill(brush_active_bg, new RectangleF(rect.Rect.X, rect.RectRead.Y, rect.RectRead.Width, rect.RectRead.Height));
                                using (var path_r = rect.RectRead.RoundPath(Radius, false, true, true, false))
                                {
                                    g.Fill(brush_active, path_r);
                                }
                                g.String(it.v, Font, brush_active_fore, rect_div[p + it.id].Rect, s_f);
                                break;
                        }
                    }
                }
                if (badge_list.Count > 0)
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, rect_div[p + it.id].Rect, g);
                    }
                }
            }
        }

        #endregion

        int IfDSType(DateTime date, string date_str, string f)
        {
            if (oldTimeHover.HasValue && oldTime.HasValue)
            {
                bool c = oldTime.Value < oldTimeHover.Value;
                if (date_str == oldTime.Value.ToString(f))
                {
                    if (oldTime.Value == oldTimeHover.Value) return 1;
                    if (c) return 3;// ←
                    else return 4;// →
                }
                else if (date_str == oldTimeHover.Value.ToString(f))
                {
                    if (oldTime.Value == oldTimeHover.Value) return 1;
                    if (c) return 4;// →
                    else return 3;// ←
                }
                if (c)
                {
                    if (date < oldTimeHover.Value && date > oldTime.Value) return 2;
                }
                else
                {
                    if (date > oldTimeHover.Value && date < oldTime.Value) return 2;
                }
            }
            return 0;
        }

        #endregion

        #region 布局

        TDatePicker showType, PickerType;
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

        void LoadLayout() => Helper.GDI(g => LoadLayout(g));
        void LoadLayout(Canvas g)
        {
            ClearShadow();
            var size = g.MeasureString(Config.NullText, Font);
            bor = (int)(size.Height * 0.1F);
            int sp = (int)(size.Height * 0.2F), sp2 = sp * 2;
            int t_time_height = (int)(size.Height * 1.56F), t_time = (int)(size.Height * 2.9F), t_button = (int)(size.Height * 1.96F), t_top = (int)(size.Height * 1.76F);
            int year_width = (int)(size.Height * 3.12F), yearR_width = (int)(size.Height * 4.66F);
            int t_width = t_time * 5 + sp2, t_x = 0;

            if (showType == PickerType && left_buttons != null) t_x = (int)(size.Height * 6.22F);

            rect_lefts.SetRect(t_x, 0, t_top, t_top).SetRectArrows(sp);
            rect_left.SetRect(t_x + t_top, 0, t_top, t_top);
            if (showType == TDatePicker.Date)
            {
                int rw2 = t_x + t_width * 2 + sp2, t_width2 = t_width / 2;
                rect_rights.SetRect(rw2 - t_top, 0, t_top, t_top).SetRectArrows(sp);
                rect_right.SetRect(rw2 - t_top * 2, 0, t_top, t_top);

                rect_year2.SetRect(t_x + (t_width - yearR_width) / 2, 0, yearR_width, t_top);
                if (YDR)
                {
                    rect_month.SetRect(t_x + t_width2 - year_width - sp, 0, year_width, t_top);
                    rect_year.SetRect(t_x + t_width2 + sp, 0, year_width, t_top);

                    rect_month_r.SetRect(t_x + t_width + t_width2 - year_width - sp, 0, year_width, t_top);
                    rect_year_r.SetRect(t_x + t_width + t_width2 + sp, 0, year_width, t_top);
                }
                else
                {
                    rect_year.SetRect(t_x + t_width2 - year_width - sp, 0, year_width, t_top);
                    rect_month.SetRect(t_x + t_width2 + sp, 0, year_width, t_top);

                    rect_year_r.SetRect(t_x + t_width + t_width2 - year_width - sp, 0, year_width, t_top);
                    rect_month_r.SetRect(t_x + t_width + t_width2 + sp, 0, year_width, t_top);
                }
            }
            else
            {
                if (showType == PickerType)
                {
                    int rw2 = t_x + t_width * 2 + sp2, t_width2 = t_width / 2;
                    rect_rights.SetRect(rw2 - t_top, 0, t_top, t_top).SetRectArrows(sp);

                    int tmp = (t_width - yearR_width) / 2;
                    rect_year2.SetRect(t_x + tmp, 0, yearR_width, t_top);
                    rect_year2_r.SetRect(t_x + t_width + tmp, 0, yearR_width, t_top);

                    if (YDR)
                    {
                        rect_month.SetRect(t_x + t_width2 - year_width - sp, 0, year_width, t_top);
                        rect_year.SetRect(t_x + t_width2 + sp, 0, year_width, t_top);

                        rect_month_r.SetRect(t_x + t_width + t_width2 - year_width - sp, 0, year_width, t_top);
                        rect_year_r.SetRect(t_x + t_width + t_width2 + sp, 0, year_width, t_top);
                    }
                    else
                    {
                        rect_year.SetRect(t_x + t_width2 - year_width - sp, 0, year_width, t_top);
                        rect_month.SetRect(t_x + t_width2 + sp, 0, year_width, t_top);

                        rect_year_r.SetRect(t_x + t_width + t_width2 - year_width - sp, 0, year_width, t_top);
                        rect_month_r.SetRect(t_x + t_width + t_width2 + sp, 0, year_width, t_top);
                    }
                }
                else
                {
                    int rw2 = t_x + t_width, t_width2 = t_width / 2;
                    rect_rights.SetRect(rw2 - t_top, 0, t_top, t_top).SetRectArrows(sp);
                    rect_right.SetRect(rw2 - t_top * 2, 0, t_top, t_top);

                    rect_year2.SetRect(t_x + (t_width - yearR_width) / 2, 0, yearR_width, t_top);
                    if (YDR)
                    {
                        rect_month.SetRect(t_x + t_width2 - year_width - sp, 0, year_width, t_top);
                        rect_year.SetRect(t_x + t_width2 + sp, 0, year_width, t_top);
                    }
                    else
                    {
                        rect_year.SetRect(t_x + t_width2 - year_width - sp, 0, year_width, t_top);
                        rect_month.SetRect(t_x + t_width2 + sp, 0, year_width, t_top);
                    }
                }
            }

            int rw = t_x + t_width, r_h = 0;
            float line2 = Dpi, line = line2 / 2;
            if (showType == TDatePicker.Date)
            {
                rw += t_width + sp2;
                int item_size = (t_width - sp2) / 7, y = t_top + sp2;

                #region 为每列创建布局

                int cx = t_x + sp2, cy = t_top + sp2 + item_size;

                rect_mon = new Rectangle(cx, y, item_size, item_size);
                rect_tue = new Rectangle(rect_mon.X + item_size, y, item_size, item_size);
                rect_wed = new Rectangle(rect_tue.X + item_size, y, item_size, item_size);
                rect_thu = new Rectangle(rect_wed.X + item_size, y, item_size, item_size);
                rect_fri = new Rectangle(rect_thu.X + item_size, y, item_size, item_size);
                rect_sat = new Rectangle(rect_fri.X + item_size, y, item_size, item_size);
                rect_sun = new Rectangle(rect_sat.X + item_size, y, item_size, item_size);

                int item_size_one = (int)(item_size * .666F);
                int xy = (item_size - item_size_one) / 2;

                var rect_day = new List<RectCalendari>(42 * 2);
                for (int _x = 0; _x < 7; _x++)
                {
                    for (int _y = 0; _y < 6; _y++)
                    {
                        rect_day.Add(new RectCalendari(_x, _y).SetRect(new Rectangle(cx + (item_size * _x), cy + (item_size * _y), item_size, item_size), xy, item_size_one));
                    }
                }
                cx = t_x + t_width + sp2;
                rect_mon2 = new Rectangle(cx, y, item_size, item_size);
                rect_tue2 = new Rectangle(rect_mon2.X + item_size, y, item_size, item_size);
                rect_wed2 = new Rectangle(rect_tue2.X + item_size, y, item_size, item_size);
                rect_thu2 = new Rectangle(rect_wed2.X + item_size, y, item_size, item_size);
                rect_fri2 = new Rectangle(rect_thu2.X + item_size, y, item_size, item_size);
                rect_sat2 = new Rectangle(rect_fri2.X + item_size, y, item_size, item_size);
                rect_sun2 = new Rectangle(rect_sat2.X + item_size, y, item_size, item_size);

                for (int _x = 0; _x < 7; _x++)
                {
                    for (int _y = 0; _y < 6; _y++)
                    {
                        rect_day.Add(new RectCalendari("R_" + _x + "_" + _y).SetRect(new Rectangle(cx + (item_size * _x), cy + (item_size * _y), item_size, item_size), xy, item_size_one));
                    }
                }
                rect_div = new Dictionary<string, RectCalendari>(rect_day.Count);
                foreach (var it in rect_day) rect_div.Add(it.id, it);
                foreach (var it in calendar_day!) rect_div[it.id].Enable = it.enable;
                foreach (var it in calendar_day2!) rect_div["R_" + it.id].Enable = it.enable;

                #endregion

                r_h = t_top + sp2 * 2 + item_size * 7;

                LoadLayoutButton(t_x, r_h, t_time_height);

                #region 线

                var dlist = new List<RectangleF>(2) { new RectangleF(t_x, t_top - line, rw - t_x, line2) };
                if (t_x > 0) dlist.Add(new RectangleF(t_x - line, 0, line2, r_h));
                rects_split = dlist.ToArray();

                #endregion
            }
            else if (showType == TDatePicker.Month)
            {
                if (showType == PickerType)
                {
                    rw += t_width + sp2;
                    int item_w = (t_width - sp2) / 3, item_h = (int)(item_w * .74F), y = t_top + sp2;
                    r_h = t_top + sp2 * 2 + item_h * 4;

                    #region 为每列创建布局

                    int cx = t_x + sp2, cy = t_top + sp2;

                    int item_size_w = (int)(item_w * .666F), item_size_h = (int)(item_w * .364F);
                    int item_x = (item_w - item_size_w) / 2, item_y = (item_h - item_size_h) / 2;

                    var rect_month = new List<RectCalendari>(12 * 2);
                    for (int _x = 0; _x < 3; _x++)
                    {
                        for (int _y = 0; _y < 4; _y++)
                        {
                            rect_month.Add(new RectCalendari(_x, _y).SetRect(new Rectangle(cx + (item_w * _x), cy + (item_h * _y), item_w, item_h), item_x, item_size_w, item_y, item_size_h));
                        }
                    }

                    cx = t_x + t_width + sp2;

                    for (int _x = 0; _x < 3; _x++)
                    {
                        for (int _y = 0; _y < 4; _y++)
                        {
                            rect_month.Add(new RectCalendari("R_" + _x + "_" + _y).SetRect(new Rectangle(cx + (item_w * _x), cy + (item_h * _y), item_w, item_h), item_x, item_size_w, item_y, item_size_h));
                        }
                    }

                    rect_div = new Dictionary<string, RectCalendari>(rect_month.Count);
                    foreach (var it in rect_month) rect_div.Add(it.id, it);
                    foreach (var it in calendar_month!) rect_div[it.id].Enable = it.enable;
                    foreach (var it in calendar_month2!) rect_div["R_" + it.id].Enable = it.enable;

                    #endregion

                    LoadLayoutButton(t_x, r_h, t_time_height);

                    #region 线

                    var dlist = new List<RectangleF>(2) { new RectangleF(t_x, t_top - line, rw - t_x, line2) };
                    if (t_x > 0) dlist.Add(new RectangleF(t_x - line, 0, line2, r_h));
                    rects_split = dlist.ToArray();

                    #endregion
                }
                else
                {
                    int item_w = (t_width - sp2) / 3, item_h = (int)(item_w * .74F), y = t_top + sp2;
                    r_h = t_top + sp2 * 2 + item_h * 4;

                    #region 为每列创建布局

                    int cx = t_x + sp2, cy = t_top + sp2;

                    int item_size_w = (int)(item_w * .666F), item_size_h = (int)(item_w * .364F);
                    int item_x = (item_w - item_size_w) / 2, item_y = (item_h - item_size_h) / 2;

                    var rect_month = new List<RectCalendari>(12);
                    for (int _x = 0; _x < 3; _x++)
                    {
                        for (int _y = 0; _y < 4; _y++)
                        {
                            rect_month.Add(new RectCalendari(_x, _y).SetRect(new Rectangle(cx + (item_w * _x), cy + (item_h * _y), item_w, item_h), item_x, item_size_w, item_y, item_size_h));
                        }
                    }
                    rect_div = new Dictionary<string, RectCalendari>(rect_month.Count);
                    foreach (var it in rect_month) rect_div.Add(it.id, it);
                    foreach (var it in calendar_month!) rect_div[it.id].Enable = it.enable;

                    #endregion

                    #region 线

                    rects_split = new RectangleF[] { new RectangleF(t_x, t_top - line, t_width, line2) };

                    #endregion
                }
            }
            else if (showType == TDatePicker.Year)
            {
                if (showType == PickerType)
                {
                    rw += t_width + sp2;
                    int item_w = (t_width - sp2) / 3, item_h = (int)(item_w * .74F), y = t_top + sp2;
                    r_h = t_top + sp2 * 2 + item_h * 4;

                    #region 为每列创建布局

                    int cx = t_x + sp2, cy = t_top + sp2;

                    int item_size_w = (int)(item_w * .666F), item_size_h = (int)(item_w * .364F);
                    int item_x = (item_w - item_size_w) / 2, item_y = (item_h - item_size_h) / 2;

                    var rect_year = new List<RectCalendari>(12 * 2);
                    for (int _x = 0; _x < 3; _x++)
                    {
                        for (int _y = 0; _y < 4; _y++)
                        {
                            rect_year.Add(new RectCalendari(_x, _y).SetRect(new Rectangle(cx + (item_w * _x), cy + (item_h * _y), item_w, item_h), item_x, item_size_w, item_y, item_size_h));
                        }
                    }
                    cx = t_x + t_width + sp2;

                    for (int _x = 0; _x < 3; _x++)
                    {
                        for (int _y = 0; _y < 4; _y++)
                        {
                            rect_year.Add(new RectCalendari("R_" + _x + "_" + _y).SetRect(new Rectangle(cx + (item_w * _x), cy + (item_h * _y), item_w, item_h), item_x, item_size_w, item_y, item_size_h));
                        }
                    }

                    rect_div = new Dictionary<string, RectCalendari>(rect_year.Count);
                    foreach (var it in rect_year) rect_div.Add(it.id, it);
                    foreach (var it in calendar_year!) rect_div[it.id].Enable = it.enable;
                    foreach (var it in calendar_year2!) rect_div["R_" + it.id].Enable = it.enable;

                    #endregion

                    LoadLayoutButton(t_x, r_h, t_time_height);

                    #region 线

                    var dlist = new List<RectangleF>(2) { new RectangleF(t_x, t_top - line, rw - t_x, line2) };
                    if (t_x > 0) dlist.Add(new RectangleF(t_x - line, 0, line2, r_h));
                    rects_split = dlist.ToArray();

                    #endregion
                }
                else
                {
                    int item_w = (t_width - sp2) / 3, item_h = (int)(item_w * .74F), y = t_top + sp2;
                    r_h = t_top + sp2 * 2 + item_h * 4;

                    #region 为每列创建布局

                    int cx = t_x + sp2, cy = t_top + sp2;

                    int item_size_w = (int)(item_w * .666F), item_size_h = (int)(item_w * .364F);
                    int item_x = (item_w - item_size_w) / 2, item_y = (item_h - item_size_h) / 2;

                    var rect_year = new List<RectCalendari>(12);
                    for (int _x = 0; _x < 3; _x++)
                    {
                        for (int _y = 0; _y < 4; _y++)
                        {
                            rect_year.Add(new RectCalendari(_x, _y).SetRect(new Rectangle(cx + (item_w * _x), cy + (item_h * _y), item_w, item_h), item_x, item_size_w, item_y, item_size_h));
                        }
                    }
                    rect_div = new Dictionary<string, RectCalendari>(rect_year.Count);
                    foreach (var it in rect_year) rect_div.Add(it.id, it);
                    foreach (var it in calendar_year!) rect_div[it.id].Enable = it.enable;

                    #endregion

                    #region 线

                    rects_split = new RectangleF[] { new RectangleF(t_x, t_top - line, t_width, line2) };

                    #endregion
                }
            }

            SetSize(rw, r_h);
        }
        void LoadLayoutDiv()
        {
            if (rect_div.Count == 0) return;
            switch (showType)
            {
                case TDatePicker.Date:
                    if (calendar_day == null || calendar_day2 == null) return;
                    foreach (var it in calendar_day) rect_div[it.id].Enable = it.enable;
                    foreach (var it in calendar_day2) rect_div["R_" + it.id].Enable = it.enable;
                    break;
                case TDatePicker.Month:
                    if (calendar_month == null || calendar_month2 == null) return;
                    foreach (var it in calendar_month) rect_div[it.id].Enable = it.enable;
                    foreach (var it in calendar_month2) rect_div["R_" + it.id].Enable = it.enable;
                    break;
                case TDatePicker.Year:
                    if (calendar_year == null || calendar_year2 == null) return;
                    foreach (var it in calendar_year) rect_div[it.id].Enable = it.enable;
                    foreach (var it in calendar_year2) rect_div["R_" + it.id].Enable = it.enable;
                    break;
            }
        }

        void LoadLayoutButton(int t_x, int r_h, int t_time_height)
        {
            if (left_buttons == null) return;
            rect_read_left = new Rectangle(0, 0, t_x, r_h);

            ScrollButtons!.SizeChange(rect_read_left);
            ScrollButtons.SetVrSize(t_time_height * left_buttons.Count);

            int bsp = (int)(t_x * .02F), bsp2 = bsp * 2, bsp22 = bsp2 * 2;
            foreach (var it in left_buttons)
            {
                var rect_btn = new Rectangle(0, t_time_height * it.y, t_x, t_time_height);
                it.rect = rect_btn;
                it.rect_read = new Rectangle(rect_btn.X + bsp, rect_btn.Y + bsp, rect_btn.Width - bsp2, rect_btn.Height - bsp2);
                it.rect_text = new Rectangle(rect_btn.X + bsp22, rect_btn.Y, rect_btn.Width - bsp22, rect_btn.Height);
            }
        }

        #endregion

        #region 鼠标

        int bor = 1;
        RectHover rect_lefts = new RectHover(), rect_left = new RectHover();
        RectHover rect_rights = new RectHover(), rect_right = new RectHover();
        RectHover rect_year = new RectHover(), rect_month = new RectHover(),
            rect_year_r = new RectHover(), rect_month_r = new RectHover(), rect_year2 = new RectHover(), rect_year2_r = new RectHover();
        RectangleF[] rects_split = new RectangleF[0];
        Rectangle rect_mon, rect_tue, rect_wed, rect_thu, rect_fri, rect_sat, rect_sun;
        Rectangle rect_mon2, rect_tue2, rect_wed2, rect_thu2, rect_fri2, rect_sat2, rect_sun2;
        Rectangle rect_read_left;
        Dictionary<string, RectCalendari> rect_div = new Dictionary<string, RectCalendari>(0);
        int ox, oy;
        protected override void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            ox = x;
            oy = y;
            if ((ScrollButtons?.MouseDown(x, y) ?? true)) OnTouchDown(x, y);
        }
        protected override void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if ((ScrollButtons?.MouseMove(x, y) ?? true) && OnTouchMove(x, y))
            {
                int count = 0, hand = 0;
                if (rect_lefts.Contains(x, y, ref count)) hand++;
                if (rect_rights.Contains(x, y, ref count)) hand++;
                if (showType == TDatePicker.Date)
                {
                    if (rect_left.Contains(x, y, ref count)) hand++;
                    if (rect_right.Contains(x, y, ref count)) hand++;
                    if (rect_year.Contains(x, y, ref count)) hand++;
                    if (rect_month.Contains(x, y, ref count)) hand++;
                    if (rect_year_r.Contains(x, y, ref count)) hand++;
                    if (rect_month_r.Contains(x, y, ref count)) hand++;
                }
                else if (showType == TDatePicker.Month)
                {
                    if (rect_year2.Contains(x, y, ref count)) hand++;
                }
                if (showType == TDatePicker.Date && (calendar_day != null && calendar_day2 != null))
                {
                    foreach (var it in calendar_day)
                    {
                        var rect = rect_div[it.id];
                        if (rect.Contains(x, y, ref count))
                        {
                            if (EndFocused) oldTimeHover = it.date;
                            hand++;
                        }
                    }
                    foreach (var it in calendar_day2)
                    {
                        var rect = rect_div["R_" + it.id];
                        if (rect.Contains(x, y, ref count))
                        {
                            if (EndFocused) oldTimeHover = it.date;
                            hand++;
                        }
                    }
                }
                else if (showType == TDatePicker.Month && (calendar_month != null && calendar_month2 != null))
                {
                    if (rect_year2_r.Contains(x, y, ref count)) hand++;
                    foreach (var it in calendar_month)
                    {
                        var rect = rect_div[it.id];
                        if (rect.Contains(x, y, ref count))
                        {
                            if (EndFocused) oldTimeHover = it.date;
                            hand++;
                        }
                    }
                    foreach (var it in calendar_month2)
                    {
                        var rect = rect_div["R_" + it.id];
                        if (rect.Contains(x, y, ref count))
                        {
                            if (EndFocused) oldTimeHover = it.date;
                            hand++;
                        }
                    }
                }
                else if (showType == TDatePicker.Year && (calendar_year != null && calendar_year2 != null))
                {
                    foreach (var it in calendar_year)
                    {
                        var rect = rect_div[it.id];
                        if (rect.Contains(x, y, ref count))
                        {
                            if (EndFocused) oldTimeHover = it.date;
                            hand++;
                        }
                    }
                    foreach (var it in calendar_year2)
                    {
                        var rect = rect_div["R_" + it.id];
                        if (rect.Contains(x, y, ref count))
                        {
                            if (EndFocused) oldTimeHover = it.date;
                            hand++;
                        }
                    }
                }
                else
                {
                    foreach (var it in rect_div)
                    {
                        if (it.Value.Contains(x, y, ref count)) hand++;
                    }
                }
                if (showType == PickerType)
                {
                    if (left_buttons != null)
                    {
                        int sx = y + ScrollButtons!.ValueY;
                        foreach (var it in left_buttons)
                        {
                            if (it.Contains(x, sx, ref count)) hand++;
                        }
                    }
                }
                if (count > 0) Print();
                SetCursor(hand > 0);
            }
        }
        protected override void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if ((ScrollButtons?.MouseUp() ?? true) && OnTouchUp())
            {
                if (button == MouseButtons.Left)
                {
                    if (rect_lefts.Contains(x, y))
                    {
                        if (ShowType == TDatePicker.Year) Date = _Date.AddYears(-10);
                        else Date = _Date.AddYears(-1);
                        Print();
                    }
                    else if (rect_rights.Contains(x, y))
                    {
                        if (ShowType == TDatePicker.Year) Date = _Date.AddYears(10);
                        else Date = _Date.AddYears(1);
                        Print();
                    }
                    if (showType == PickerType)
                    {
                        if (left_buttons != null)
                        {
                            int sx = y + ScrollButtons!.ValueY;
                            foreach (var it in left_buttons)
                            {
                                if (it.Contains(x, sx))
                                {
                                    action_btns(it.Tag);
                                    IClose();
                                    return;
                                }
                            }
                        }
                    }
                    if (showType == TDatePicker.Date)
                    {
                        if (rect_left.Contains(x, y))
                        {
                            Date = _Date.AddMonths(-1);
                            Print();
                        }
                        else if (rect_right.Contains(x, y))
                        {
                            Date = _Date.AddMonths(1);
                            Print();
                        }
                        else if (rect_year.Contains(x, y) || rect_year_r.Contains(x, y))
                        {
                            ShowType = TDatePicker.Year;
                            Print();
                        }
                        else if (rect_month.Contains(x, y) || rect_month_r.Contains(x, y))
                        {
                            ShowType = TDatePicker.Month;
                            Print();
                        }
                        else
                        {
                            if (calendar_day != null)
                            {
                                foreach (var it in calendar_day)
                                {
                                    if (it.enable)
                                    {
                                        if (rect_div[it.id].Contains(x, y))
                                        {
                                            if (SetDate(it)) return;
                                            IClose();
                                            return;
                                        }
                                    }
                                }
                            }
                            if (calendar_day2 != null)
                            {
                                foreach (var it in calendar_day2)
                                {
                                    if (it.enable)
                                    {
                                        if (rect_div["R_" + it.id].Contains(x, y))
                                        {
                                            if (SetDate(it)) return;
                                            IClose();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (showType == TDatePicker.Month)
                    {
                        if (rect_year2.Contains(x, y))
                        {
                            ShowType = TDatePicker.Year;
                            Print();
                        }
                        else
                        {
                            if (calendar_month2 != null && calendar_month != null)
                            {
                                if (rect_year2_r.Contains(x, y))
                                {
                                    ShowType = TDatePicker.Year;
                                    Print();
                                    return;
                                }
                                foreach (var it in calendar_month)
                                {
                                    if (it.enable)
                                    {
                                        if (rect_div[it.id].Contains(x, y))
                                        {
                                            if (SetDate(it)) return;
                                            IClose();
                                            return;
                                        }
                                    }
                                }
                                foreach (var it in calendar_month2)
                                {
                                    if (it.enable)
                                    {
                                        if (rect_div["R_" + it.id].Contains(x, y))
                                        {
                                            if (SetDate(it)) return;
                                            IClose();
                                            return;
                                        }
                                    }
                                }
                            }
                            else if (calendar_month != null)
                            {
                                foreach (var it in calendar_month)
                                {
                                    if (it.enable)
                                    {
                                        if (rect_div[it.id].Contains(x, y))
                                        {
                                            Date = it.date;
                                            ShowType = TDatePicker.Date;
                                            Print();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (showType == TDatePicker.Year)
                    {
                        if (calendar_year2 != null && calendar_year != null)
                        {
                            foreach (var it in calendar_year)
                            {
                                if (it.enable)
                                {
                                    if (rect_div[it.id].Contains(x, y))
                                    {
                                        if (SetDate(it)) return;
                                        IClose();
                                        return;
                                    }
                                }
                            }
                            foreach (var it in calendar_year2)
                            {
                                if (it.enable)
                                {
                                    if (rect_div["R_" + it.id].Contains(x, y))
                                    {
                                        if (SetDate(it)) return;
                                        IClose();
                                        return;
                                    }
                                }
                            }
                        }
                        else if (calendar_year != null)
                        {
                            foreach (var it in calendar_year)
                            {
                                if (it.enable)
                                {
                                    if (rect_div[it.id].Contains(x, y))
                                    {
                                        Date = it.date;
                                        ShowType = TDatePicker.Month;
                                        Print();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollButtons != null && rect_read_left.Contains(x, y)) ScrollButtons.MouseWheel(delta);
            else
            {
                if (delta > 0)
                {
                    if (ShowType == TDatePicker.Month)
                    {
                        if (rect_lefts.Enable) Date = _Date.AddYears(-1);
                        else return;
                    }
                    else if (ShowType == TDatePicker.Year)
                    {
                        if (rect_lefts.Enable) Date = _Date.AddYears(-10);
                        else return;
                    }
                    else
                    {
                        if (rect_left.Enable) Date = _Date.AddMonths(-1);
                        else return;
                    }
                    Print();
                }
                else
                {
                    if (ShowType == TDatePicker.Month)
                    {
                        if (rect_rights.Enable) Date = _Date.AddYears(1);
                        else return;
                    }
                    else if (ShowType == TDatePicker.Year)
                    {
                        if (rect_rights.Enable) Date = _Date.AddYears(10);
                        else return;
                    }
                    else
                    {
                        if (rect_right.Enable) Date = _Date.AddMonths(1);
                        else return;
                    }
                    Print();
                }
            }
        }
        protected override bool OnTouchScrollY(int value)
        {
            if (ScrollButtons != null && ScrollButtons.Contains(ox, oy)) return ScrollButtons.MouseWheelYCore(value);
            return false;
        }

        #endregion
    }
}