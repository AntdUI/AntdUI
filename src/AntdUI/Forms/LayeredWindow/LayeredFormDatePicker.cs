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
    public class LayeredFormDatePicker : ILayeredShadowForm
    {
        #region 初始化

        DatePicker control;
        DateTime? minDate, maxDate;
        TAMode ColorScheme;
        bool ShowTime = false, ShowH = false, ShowM = false, ShowS = false,
            ValueTimeHorizontal = false, ShowButtonToDay = true;
        public LayeredFormDatePicker(DatePicker _control, Action<DateTime> _action, Action<object> _action_btns, Func<DateTime[], List<DateBadge>?>? _badge_action)
        {
            PARENT = control = _control;
            ColorScheme = _control.ColorScheme;
            _control.Parent.SetTopMost(Handle);
            Font = _control.Font;
            minDate = _control.MinDate;
            maxDate = _control.MaxDate;
            ShowH = control.Format.Contains("H");
            ShowM = control.Format.Contains("m");
            ShowS = control.Format.Contains("s");
            ShowTime = ShowH || ShowM || ShowS;
            ShowButtonToDay = _control.ShowButtonToDay;
            ValueTimeHorizontal = _control.ValueTimeHorizontal;
            if (ShowTime) button_text = Localization.Get("Now", "此刻");
            else button_text = Localization.Get("ToDay", "今天");

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
            ScrollH = new ScrollBar(this, control.ColorScheme);
            ScrollM = new ScrollBar(this, control.ColorScheme);
            ScrollS = new ScrollBar(this, control.ColorScheme);

            #region 国际化

            Culture = new CultureInfo(CultureID);
            YDR = CultureID.StartsWith("en");
            Helper.InitLanguage(YDR, out YearFormat, out MonthFormat, out MondayButton, out TuesdayButton, out WednesdayButton, out ThursdayButton, out FridayButton, out SaturdayButton, out SundayButton, out s_f_L, out s_f_R);

            #endregion

            if (Dpi == 1F) Radius = _control.radius;
            else
            {
                ArrowSize = (int)(8 * Dpi);
                Radius = (int)(_control.radius * Dpi);
            }
            SelDate = _control.Value;
            Date = SelDate ?? DateTime.Now;

            LoadLayout();

            CLocation(_control, _control.Placement, _control.DropDownArrow, ArrowSize);
            if (OS.Win7OrLower) Select();
        }

        int ArrowSize = 8;

        public DateTime? SelDate;

        DateTime _Date;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;
                calendar_day = CalendarHelper.Day(value, minDate, maxDate);

                if (ShowTime && calendar_time == null) calendar_time = CalendarHelper.Time(ShowH, ShowM, ShowS);

                calendar_month = CalendarHelper.Month(value, minDate, maxDate, Culture, MonthFormat);

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
        Action<DateTime> action;
        Action<object> action_btns;
        Func<DateTime[], List<DateBadge>?>? badge_action;
        Dictionary<string, DateBadge> badge_list = new Dictionary<string, DateBadge>();

        #endregion

        List<ItemCalendari>? calendar_year, calendar_month, calendar_day;
        List<CalendarT>? calendar_time;
        List<CalendarButton>? left_buttons;
        ScrollBar? ScrollButtons;
        ScrollBar ScrollH, ScrollM, ScrollS;

        string year_str = "";

        public override string name => nameof(DatePicker);

        CultureInfo Culture;
        string CultureID = Localization.Get("ID", "zh-CN"),
            button_text, OKButton = Localization.Get("OK", "确定"),
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
                if (ArrowLine != null) g.FillPolygon(brush, ArrowLine);
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
            switch (showType)
            {
                case TDatePicker.Date:
                    PrintDay(g, state, rect, calendar_day!);
                    break;
                case TDatePicker.Month:
                    PrintMonth(g, state, rect, calendar_month!);
                    break;
                case TDatePicker.Year:
                    PrintYear(g, state, rect, calendar_year!);
                    break;
            }
        }

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
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get(name, ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get(name, ColorScheme)))
            {
                var now = DateTime.Now;
                foreach (var it in datas)
                {
                    var rect = rect_div[it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        if (SelDate.HasValue && SelDate.Value.ToString("yyyy") == it.date_str)
                        {
                            g.Fill(Colour.Primary.Get(name, ColorScheme), path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get(name, ColorScheme), rect_div[it.id].Rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (rect.Hover) g.Fill(brush_bg_disable, path);
                            g.String(it.v, Font, brush_fore, rect_div[it.id].Rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(rect.Rect.X, rect.RectRead.Y, rect.Rect.Width, rect.RectRead.Height));
                            g.String(it.v, Font, brush_fore_disable, rect_div[it.id].Rect, s_f);
                        }
                        if (now.ToString("yyyy") == it.date_str) g.Draw(Colour.Primary.Get(name, ColorScheme), bor, path);
                    }
                }
                if (badge_list.Count > 0)
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, rect_div[it.id].Rect, g);
                    }
                }
            }
        }
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
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get(name, ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get(name, ColorScheme)))
            {
                var now = DateTime.Now;
                foreach (var it in datas)
                {
                    var rect = rect_div[it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        if (SelDate.HasValue && SelDate.Value.ToString("yyyy-MM") == it.date_str)
                        {
                            g.Fill(Colour.Primary.Get(name, ColorScheme), path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get(name, ColorScheme), rect_div[it.id].Rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (rect.Hover) g.Fill(brush_bg_disable, path);
                            g.String(it.v, Font, brush_fore, rect_div[it.id].Rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(rect.Rect.X, rect.RectRead.Y, rect.Rect.Width, rect.RectRead.Height));
                            g.String(it.v, Font, brush_fore_disable, rect_div[it.id].Rect, s_f);
                        }
                        if (now.ToString("yyyy-MM") == it.date_str) g.Draw(Colour.Primary.Get(name, ColorScheme), bor, path);
                    }
                }
                if (badge_list.Count > 0)
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, rect_div[it.id].Rect, g);
                    }
                }
            }
        }
        void PrintDay(Canvas g, GraphicsState state, Rectangle rect_read, List<ItemCalendari> datas)
        {
            var color_fore = Colour.TextBase.Get(name, ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture), monthStr = _Date.ToString(MonthFormat, Culture);
                if (rect_year.Hover) g.String(yearStr, font, Colour.Primary.Get(name, ColorScheme), rect_year.Rect, s_f_L);
                else g.String(yearStr, font, color_fore, rect_year.Rect, s_f_L);

                if (rect_month.Hover) g.String(monthStr, font, Colour.Primary.Get(name, ColorScheme), rect_month.Rect, s_f_R);
                else g.String(monthStr, font, color_fore, rect_month.Rect, s_f_R);
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
            }
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get(name, ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get(name, ColorScheme)))
            using (var brush_active = new SolidBrush(Colour.Primary.Get(name, ColorScheme)))
            {
                if (left_buttons != null)
                {
                    g.TranslateTransform(0, -ScrollButtons!.Value);
                    foreach (var it in left_buttons)
                    {
                        using (var path = it.rect_read.RoundPath(Radius))
                        {
                            if (it.hover) g.Fill(brush_bg_disable, path);
                            g.String(it.v, Font, brush_fore, it.rect_text, s_f_LE);
                        }
                    }
                    g.Restore(state);
                    ScrollButtons.Paint(g, ColorScheme);
                }
                if (calendar_time != null)
                {
                    var state2 = g.Save();
                    using (var brush_bg = new SolidBrush(Colour.PrimaryBg.Get(name, ColorScheme)))
                    {
                        int type = -1;
                        for (int i = 0; i < calendar_time.Count; i++)
                        {
                            var it = calendar_time[i];
                            if (type != it.rx)
                            {
                                type = it.rx;
                                g.Restore(state2);
                                state2 = g.Save();
                                switch (type)
                                {
                                    case 0:
                                        g.SetClip(rect_read_h);
                                        g.TranslateTransform(0, -ScrollH.Value);
                                        break;
                                    case 1:
                                        g.SetClip(rect_read_m);
                                        g.TranslateTransform(0, -ScrollM.Value);
                                        break;
                                    case 2:
                                        g.SetClip(rect_read_s);
                                        g.TranslateTransform(0, -ScrollS.Value);
                                        break;
                                }
                            }
                            using (var path = it.rect_read.RoundPath(Radius))
                            {
                                if (SelDate.HasValue)
                                {
                                    switch (it.rx)
                                    {
                                        case 0:
                                            if (it.t == SelDate.Value.Hour) g.Fill(brush_bg, path);
                                            break;
                                        case 1:
                                            if (it.t == SelDate.Value.Minute) g.Fill(brush_bg, path);
                                            break;
                                        case 2:
                                            if (it.t == SelDate.Value.Second) g.Fill(brush_bg, path);
                                            break;
                                    }
                                }
                                if (it.hover) g.Fill(brush_bg_disable, path);
                                g.String(it.v, Font, brush_fore, it.rect_read, s_f);
                            }
                        }
                    }
                    g.Restore(state2);
                    ScrollH.Paint(g, ColorScheme);
                    ScrollM.Paint(g, ColorScheme);
                    ScrollS.Paint(g, ColorScheme);
                    if (rect_buttonok.Hover) g.String(OKButton, Font, Colour.PrimaryActive.Get(name, ColorScheme), rect_buttonok.Rect, s_f);
                    else g.String(OKButton, Font, brush_active, rect_buttonok.Rect, s_f);
                }

                var now = DateTime.Now;
                foreach (var it in datas)
                {
                    var rect = rect_div[it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        if (SelDate.HasValue && SelDate.Value.ToString("yyyy-MM-dd") == it.date_str)
                        {
                            g.Fill(brush_active, path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get(name, ColorScheme), rect_div[it.id].Rect, s_f);
                        }
                        else if (it.enable)
                        {
                            if (rect.Hover) g.Fill(brush_bg_disable, path);
                            g.String(it.v, Font, it.t == 1 ? brush_fore : brush_fore_disable, rect_div[it.id].Rect, s_f);
                        }
                        else
                        {
                            g.Fill(brush_bg_disable, new Rectangle(rect.Rect.X, rect.RectRead.Y, rect.Rect.Width, rect.RectRead.Height));
                            g.String(it.v, Font, brush_fore_disable, rect_div[it.id].Rect, s_f);
                        }
                        if (now.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get(name, ColorScheme), bor, path); ;
                    }
                }
                if (badge_list.Count > 0)
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, rect_div[it.id].Rect, g);
                    }
                }

                if (ShowButtonToDay)
                {
                    if (rect_button.Hover) g.String(button_text, Font, Colour.PrimaryActive.Get(name, ColorScheme), rect_button.Rect, s_f);
                    else g.String(button_text, Font, brush_active, rect_button.Rect, s_f);
                }
            }
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
            var size = g.MeasureString(Config.NullText, Font);
            bor = (int)(size.Height * 0.1F);
            int sp = (int)(size.Height * 0.2F), sp2 = sp * 2;
            int t_time_height = (int)(size.Height * 1.56F), t_time = (int)(size.Height * 2.9F), t_button = (int)(size.Height * 1.96F), t_top = (int)(size.Height * 1.76F);
            int year_width = (int)(size.Height * 3.12F), year2_width = (int)(size.Height * 4.66F);
            int t_width = t_time * 5 + sp2, t_width2 = t_width / 2, t_x = 0;

            if (showType == TDatePicker.Date && left_buttons != null) t_x = (int)(size.Height * 6.22F);

            rect_lefts.SetRect(t_x, 0, t_top, t_top).SetRectArrows(sp);
            rect_left.SetRect(t_x + t_top, 0, t_top, t_top);
            rect_rights.SetRect(t_x + t_width - t_top, 0, t_top, t_top).SetRectArrows(sp);
            rect_right.SetRect(t_x + t_width - t_top * 2, 0, t_top, t_top);

            rect_year2.SetRect(t_x + (t_width - year2_width) / 2, 0, year2_width, t_top);
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

            int rw = t_width + t_x, r_h = 0;
            float line2 = Dpi, line = line2 / 2;
            if (showType == TDatePicker.Date)
            {
                int item_size = (t_width - sp2) / 7, y = t_top + sp2;
                rect_mon = new Rectangle(t_x + sp2, y, item_size, item_size);
                rect_tue = new Rectangle(rect_mon.X + item_size, y, item_size, item_size);
                rect_wed = new Rectangle(rect_tue.X + item_size, y, item_size, item_size);
                rect_thu = new Rectangle(rect_wed.X + item_size, y, item_size, item_size);
                rect_fri = new Rectangle(rect_thu.X + item_size, y, item_size, item_size);
                rect_sat = new Rectangle(rect_fri.X + item_size, y, item_size, item_size);
                rect_sun = new Rectangle(rect_sat.X + item_size, y, item_size, item_size);

                #region 为每列创建布局

                int cx = t_x + sp2, cy = t_top + sp2 + item_size;

                int item_size_one = (int)(item_size * .666F);
                int xy = (item_size - item_size_one) / 2;

                var rect_day = new List<RectCalendari>(42);
                for (int _x = 0; _x < 7; _x++)
                {
                    for (int _y = 0; _y < 6; _y++)
                    {
                        rect_day.Add(new RectCalendari(_x, _y).SetRect(new Rectangle(cx + (item_size * _x), cy + (item_size * _y), item_size, item_size), xy, item_size_one));
                    }
                }
                rect_div = new Dictionary<string, RectCalendari>(rect_day.Count);
                foreach (var it in rect_day) rect_div.Add(it.id, it);
                foreach (var it in calendar_day!) rect_div[it.id].Enable = it.enable;

                #endregion

                r_h = t_top + (ShowButtonToDay ? t_button : 0) + sp2 * 2 + item_size * 7;

                rect_button.SetRect(t_x + (t_width - year_width) / 2, r_h - t_button, year_width, t_button);

                #region 自定义按钮

                if (left_buttons != null)
                {
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

                #region 线

                var dlist = new List<RectangleF>(3) { new RectangleF(t_x, t_top - line, t_width, line2) };
                if (ShowTime)
                {
                    int tcount = 0;
                    if (ShowH) tcount++;
                    if (ShowM) tcount++;
                    if (ShowS) tcount++;
                    int t_time_w = t_time * tcount;

                    if (calendar_time != null)
                    {
                        int size_time_one = (int)(t_time * .857F), size_time_height_one = (int)(t_time_height * .93F);

                        var rect_time = new Rectangle(rw, 0, t_time, r_h - t_button);
                        int tmp = rect_time.Right - t_time;
                        if (ShowH)
                        {
                            rect_read_h = new Rectangle(tmp, rect_time.Y, t_time, rect_time.Height);
                            ScrollH.SizeChange(rect_read_h);
                            tmp += t_time;
                        }
                        if (ShowM)
                        {
                            rect_read_m = new Rectangle(tmp, rect_time.Y, t_time, rect_time.Height);
                            ScrollM.SizeChange(rect_read_m);
                            tmp += t_time;
                        }
                        if (ShowS)
                        {
                            rect_read_s = new Rectangle(tmp, rect_time.Y, t_time, rect_time.Height);
                            ScrollS.SizeChange(rect_read_s);
                            tmp += t_time;
                        }

                        if (ValueTimeHorizontal)
                        {
                            int exceed = rect_time.Height / t_time_height;
                            int max = t_time_height * (60 + exceed);
                            ScrollH.SetVrSize(t_time_height * (24 + exceed));
                            ScrollM.SetVrSize(max);
                            ScrollS.SetVrSize(max);
                        }
                        else
                        {
                            int max = t_time_height * 60;
                            ScrollH.SetVrSize(t_time_height * 24);
                            ScrollM.SetVrSize(max);
                            ScrollS.SetVrSize(max);
                        }

                        int _x = (t_time - size_time_one) / 2, _y = (t_time_height - size_time_height_one) / 2;
                        foreach (var it in calendar_time)
                        {
                            it.rect = new Rectangle(rw + t_time * it.x, t_time_height * it.y, t_time, t_time_height);
                            it.rect_read = new Rectangle(it.rect.X + _x, it.rect.Y + _y, size_time_one, size_time_height_one);
                        }

                        if (SelDate.HasValue) ScrollTime(calendar_time, SelDate.Value);
                    }

                    dlist.Add(new RectangleF(rw - line, 0, line2, r_h));
                    rect_buttonok.SetRect(rw, rect_button.Rect.Y, t_time_w, t_button);
                    rw += t_time_w;
                }
                if (ShowButtonToDay) dlist.Add(new RectangleF(t_x, rect_button.Rect.Y - line, rw - t_x, line2));
                else dlist.Add(new RectangleF(t_x + t_width, rect_button.Rect.Y - line, rw - t_x - t_width, line2));
                if (t_x > 0) dlist.Add(new RectangleF(t_x - line, 0, line2, r_h));
                rects_split = dlist.ToArray();

                #endregion
            }
            else if (showType == TDatePicker.Month)
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
            else if (showType == TDatePicker.Year)
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

            SetSize(rw, r_h);
        }
        void LoadLayoutDiv()
        {
            if (rect_div.Count == 0) return;
            switch (showType)
            {
                case TDatePicker.Date:
                    if (calendar_day == null) return;
                    foreach (var it in calendar_day) rect_div[it.id].Enable = it.enable;
                    break;
                case TDatePicker.Month:
                    if (calendar_month == null) return;
                    foreach (var it in calendar_month) rect_div[it.id].Enable = it.enable;
                    break;
                case TDatePicker.Year:
                    if (calendar_year == null) return;
                    foreach (var it in calendar_year) rect_div[it.id].Enable = it.enable;
                    break;
            }
        }
        void ScrollTime(List<CalendarT> calendar_time, DateTime d)
        {
            CalendarT? find_h = calendar_time.Find(a => a.rx == 0 && a.t == d.Hour),
                find_m = calendar_time.Find(a => a.rx == 1 && a.t == d.Minute),
                find_s = calendar_time.Find(a => a.rx == 2 && a.t == d.Second);

            if (find_h != null) ScrollH.Value = find_h.rect.Y;
            if (find_m != null) ScrollM.Value = find_m.rect.Y;
            if (find_s != null) ScrollS.Value = find_s.rect.Y;
        }

        #endregion

        #region 鼠标

        int bor = 1;
        RectHover rect_button = new RectHover(), rect_buttonok = new RectHover();
        RectHover rect_lefts = new RectHover(), rect_left = new RectHover();
        RectHover rect_rights = new RectHover(), rect_right = new RectHover();
        RectHover rect_year = new RectHover(), rect_year2 = new RectHover(), rect_month = new RectHover();
        RectangleF[] rects_split = new RectangleF[0];
        Rectangle rect_mon, rect_tue, rect_wed, rect_thu, rect_fri, rect_sat, rect_sun;
        Rectangle rect_read_left, rect_read_h, rect_read_m, rect_read_s;
        Dictionary<string, RectCalendari> rect_div = new Dictionary<string, RectCalendari>(0);
        int ox, oy;
        protected override void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            ox = x;
            oy = y;
            if ((ScrollButtons?.MouseDown(x, y) ?? true) && ScrollH.MouseDown(x, y) && ScrollM.MouseDown(x, y) && ScrollS.MouseDown(x, y)) OnTouchDown(x, y);
        }
        protected override void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if ((ScrollButtons?.MouseMove(x, y) ?? true) && ScrollH.MouseMove(x, y) && ScrollM.MouseMove(x, y) && ScrollS.MouseMove(x, y) && OnTouchMove(x, y))
            {
                int count = 0, hand = 0;
                if (rect_lefts.Contains(x, y, ref count)) hand++;
                if (rect_rights.Contains(x, y, ref count)) hand++;
                if (showType == TDatePicker.Date)
                {
                    if (rect_left.Contains(x, y, ref count)) hand++;
                    if (rect_right.Contains(x, y, ref count)) hand++;
                    if (ShowButtonToDay && rect_button.Contains(x, y, ref count)) hand++;
                    if (ShowTime && rect_buttonok.Contains(x, y, ref count)) hand++;
                    if (rect_year.Contains(x, y, ref count)) hand++;
                    if (rect_month.Contains(x, y, ref count)) hand++;
                    if (left_buttons != null)
                    {
                        int sx = y + ScrollButtons!.ValueY;
                        foreach (var it in left_buttons)
                        {
                            if (it.Contains(x, sx, ref count)) hand++;
                        }
                    }
                    if (calendar_time != null)
                    {
                        foreach (var it in calendar_time)
                        {
                            switch (it.rx)
                            {
                                case 1:
                                    if (it.Contains(x, y + ScrollM.Value, ref count)) hand++;
                                    break;
                                case 2:
                                    if (it.Contains(x, y + ScrollS.Value, ref count)) hand++;
                                    break;
                                case 0:
                                default:
                                    if (it.Contains(x, y + ScrollH.Value, ref count)) hand++;
                                    break;
                            }
                        }
                    }
                }
                else if (showType == TDatePicker.Month)
                {
                    if (rect_year2.Contains(x, y, ref count)) hand++;
                }
                foreach (var it in rect_div)
                {
                    if (it.Value.Contains(x, y, ref count)) hand++;
                }
                if (count > 0) Print();
                SetCursor(hand > 0);
            }
        }
        protected override void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if ((ScrollButtons?.MouseUp() ?? true) && ScrollH.MouseUp() && ScrollM.MouseUp() && ScrollS.MouseUp() && OnTouchUp())
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
                        else if (rect_year.Contains(x, y))
                        {
                            ShowType = TDatePicker.Year;
                            Print();
                        }
                        else if (rect_month.Contains(x, y))
                        {
                            ShowType = TDatePicker.Month;
                            Print();
                        }
                        else if (ShowButtonToDay && rect_button.Contains(x, y))
                        {
                            SelDate = Date = DateTime.Now;
                            action(SelDate.Value);
                            if (ShowTime && calendar_time != null)
                            {
                                Print();
                                ScrollTime(calendar_time, SelDate.Value);
                            }
                            else IClose();
                        }
                        else if (ShowTime && rect_buttonok.Contains(x, y))
                        {
                            if (SelDate.HasValue)
                            {
                                action(SelDate.Value);
                                IClose();
                            }
                        }
                        else
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
                            if (calendar_day != null)
                            {
                                foreach (var it in calendar_day)
                                {
                                    if (it.enable)
                                    {
                                        if (rect_div[it.id].Contains(x, y))
                                        {
                                            if (ShowTime)
                                            {
                                                if (SelDate.HasValue) SelDate = new DateTime(it.date.Year, it.date.Month, it.date.Day, SelDate.Value.Hour, SelDate.Value.Minute, SelDate.Value.Second);
                                                else
                                                {
                                                    var now = DateTime.Now;
                                                    SelDate = new DateTime(it.date.Year, it.date.Month, it.date.Day, now.Hour, now.Minute, now.Second);
                                                }
                                                action(SelDate.Value);
                                                Print();
                                                if (calendar_time != null) ScrollTime(calendar_time, SelDate.Value);
                                                return;
                                            }
                                            else SelDate = it.date;
                                            action(SelDate.Value);
                                            IClose();
                                            return;
                                        }
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
                                            if (it.Contains(x, y + ScrollM.Value))
                                            {
                                                if (SelDate.HasValue) SelDate = new DateTime(SelDate.Value.Year, SelDate.Value.Month, SelDate.Value.Day, SelDate.Value.Hour, it.t, SelDate.Value.Second);
                                                else
                                                {
                                                    var now = DateTime.Now;
                                                    SelDate = new DateTime(now.Year, now.Month, now.Day, 0, it.t, 0);
                                                }
                                                Print();
                                                if (ValueTimeHorizontal) ScrollTime(calendar_time, SelDate.Value);
                                                return;
                                            }
                                            break;
                                        case 2:
                                            if (it.Contains(x, y + ScrollS.Value))
                                            {
                                                if (SelDate.HasValue) SelDate = new DateTime(SelDate.Value.Year, SelDate.Value.Month, SelDate.Value.Day, SelDate.Value.Hour, SelDate.Value.Minute, it.t);
                                                else
                                                {
                                                    var now = DateTime.Now;
                                                    SelDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, it.t);
                                                }
                                                Print();
                                                if (ValueTimeHorizontal) ScrollTime(calendar_time, SelDate.Value);
                                                return;
                                            }
                                            break;
                                        case 0:
                                        default:
                                            if (it.Contains(x, y + ScrollH.Value))
                                            {
                                                if (SelDate.HasValue) SelDate = new DateTime(SelDate.Value.Year, SelDate.Value.Month, SelDate.Value.Day, it.t, SelDate.Value.Minute, SelDate.Value.Second);
                                                else
                                                {
                                                    var now = DateTime.Now;
                                                    SelDate = new DateTime(now.Year, now.Month, now.Day, it.t, 0, 0);
                                                }
                                                Print();
                                                if (ValueTimeHorizontal) ScrollTime(calendar_time, SelDate.Value);
                                                return;
                                            }
                                            break;
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
                            if (calendar_month != null)
                            {
                                foreach (var it in calendar_month)
                                {
                                    if (it.enable)
                                    {
                                        if (rect_div[it.id].Contains(x, y))
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
                        }
                    }
                    else if (showType == TDatePicker.Year)
                    {
                        if (calendar_year != null)
                        {
                            foreach (var it in calendar_year)
                            {
                                if (it.enable)
                                {
                                    if (rect_div[it.id].Contains(x, y))
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
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollButtons != null && rect_read_left.Contains(x, y)) ScrollButtons.MouseWheel(delta);
            else if (ScrollH.Contains(x, y)) ScrollH.MouseWheel(delta);
            else if (ScrollM.Contains(x, y)) ScrollM.MouseWheel(delta);
            else if (ScrollS.Contains(x, y)) ScrollS.MouseWheel(delta);
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
            else if (ScrollH.Contains(ox, oy)) return ScrollH.MouseWheelYCore(value);
            else if (ScrollM.Contains(ox, oy)) return ScrollM.MouseWheelYCore(value);
            else if (ScrollS.Contains(ox, oy)) return ScrollS.MouseWheelYCore(value);
            return false;
        }

        #endregion
    }

    internal class CalendarHelper
    {
        public static List<ItemCalendari> Day(DateTime now, DateTime? minDate, DateTime? maxDate)
        {
            var calendaris = new List<ItemCalendari>(42);
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
                    calendaris.Insert(0, new ItemCalendari(0, (day_ - 1) - i, 0, day3.ToString(), new DateTime(date1.Year, date1.Month, day3), minDate, maxDate));
                }
            }
            int x = day_, y = 0;
            for (int i = 0; i < days; i++)
            {
                int day = i + 1;
                calendaris.Add(new ItemCalendari(1, x, y, day.ToString(), new DateTime(now.Year, now.Month, day), minDate, maxDate));
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
                    calendaris.Add(new ItemCalendari(2, x, y, day3.ToString(), new DateTime(date1.Year, date1.Month, day3), minDate, maxDate));
                    x++; day2++;
                }
                if (y < 5)
                {
                    y++;
                    for (int i = 0; i < 7; i++)
                    {
                        int day3 = day2 + 1;
                        calendaris.Add(new ItemCalendari(2, i, y, day3.ToString(), new DateTime(date1.Year, date1.Month, day3), minDate, maxDate));
                        day2++;
                    }
                }
            }
            return calendaris;
        }
        public static List<CalendarT> Time(bool ShowH, bool ShowM, bool ShowS)
        {
            var calendar_time = new List<CalendarT>(24 + 120);
            int count = 0;
            if (ShowH)
            {
                for (int i = 0; i < 24; i++) calendar_time.Add(new CalendarT(count, 0, i, i));
                count++;
            }
            if (ShowM)
            {
                for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(count, 1, i, i));
                count++;
            }
            if (ShowS)
            {
                for (int i = 0; i < 60; i++) calendar_time.Add(new CalendarT(count, 2, i, i));
                count++;
            }
            return calendar_time;
        }
        public static List<ItemCalendari> Month(DateTime now, DateTime? minDate, DateTime? maxDate, CultureInfo Culture, string format)
        {
            var calendaris = new List<ItemCalendari>(12);
            int x_m = 0, y_m = 0;
            for (int i = 0; i < 12; i++)
            {
                var d_m = new DateTime(now.Year, i + 1, 1);
                calendaris.Add(new ItemCalendari(0, x_m, y_m, d_m.ToString(format, Culture), d_m, d_m.ToString("yyyy-MM"), minDate, maxDate));
                x_m++;
                if (x_m > 2)
                {
                    y_m++;
                    x_m = 0;
                }
            }
            return calendaris;
        }
        public static List<ItemCalendari> Year(DateTime now, DateTime? minDate, DateTime? maxDate, out string year_str)
        {
            int syear = now.Year - 1;
            if (!now.Year.ToString().EndsWith("0"))
            {
                string temp = now.Year.ToString();
                syear = int.Parse(temp.Substring(0, temp.Length - 1) + "0") - 1;
            }
            var calendaris = new List<ItemCalendari>(12);
            int x_y = 0, y_y = 0;
            if (syear < 1) syear = 1;
            for (int i = 0; i < 12; i++)
            {
                var d_y = new DateTime(syear + i, now.Month, 1);
                calendaris.Add(new ItemCalendari(i == 0 ? 0 : 1, x_y, y_y, d_y.ToString("yyyy"), d_y, d_y.ToString("yyyy"), minDate, maxDate));
                x_y++;
                if (x_y > 2)
                {
                    y_y++;
                    x_y = 0;
                }
            }
            year_str = calendaris[1].date_str + "-" + calendaris[calendaris.Count - 2].date_str;
            return calendaris;
        }
    }

    internal class RectHover
    {
        public bool Hover { get; set; }

        public bool Enable { get; set; } = true;

        public Rectangle Rect;
        public Rectangle[] Rects = new Rectangle[0];

        public bool Contains(int x, int y, ref int count)
        {
            if (Enable && Rect.Contains(x, y))
            {
                if (Hover) return true;
                Hover = true;
                count++;
                return true;
            }
            else
            {
                if (Hover)
                {
                    Hover = false;
                    count++;
                }
                return false;
            }
        }
        public bool Contains(int x, int y) => Enable && Rect.Contains(x, y);

        public RectHover SetRect(int x, int y, int w, int h)
        {
            Rect = new Rectangle(x, y, w, h);
            return this;
        }
        public RectHover SetRect(Rectangle[] rect)
        {
            Rects = rect;
            return this;
        }

        public RectHover SetRectArrows(int sp)
        {
            Rects = new Rectangle[2] {
                new Rectangle(Rect.X - sp, Rect.Y, Rect.Width, Rect.Height),
                new Rectangle(Rect.X + sp, Rect.Y, Rect.Width, Rect.Height)
            };
            return this;
        }
    }

    internal class RectCalendari
    {
        public RectCalendari(int x, int y)
        {
            id = x + "_" + y;
        }
        public RectCalendari(string _id)
        {
            id = _id;
        }

        public bool Hover { get; set; }
        public bool Enable { get; set; }

        public bool Contains(int x, int y, ref int count)
        {
            if (Enable && Rect.Contains(x, y))
            {
                if (Hover) return true;
                Hover = true;
                count++;
                return true;
            }
            else
            {
                if (Hover)
                {
                    Hover = false;
                    count++;
                }
                return false;
            }
        }
        public bool Contains(int x, int y) => Enable && Rect.Contains(x, y);

        public Rectangle Rect;
        public Rectangle RectRead;

        public RectCalendari SetRect(Rectangle value, int xy, int gap)
        {
            RectRead = new Rectangle(value.X + xy, value.Y + xy, gap, gap);
            Rect = value;
            return this;
        }

        public RectCalendari SetRect(Rectangle value, int x, int w, int y, int h)
        {
            RectRead = new Rectangle(value.X + x, value.Y + y, w, h);
            Rect = value;
            return this;
        }

        public string id { get; set; }
    }

    #region 项

    internal class ItemCalendari
    {
        public ItemCalendari(int _t, int _x, int _y, string _v, DateTime _date, string str, DateTime? min, DateTime? max)
        {
            id = _x + "_" + _y;
            t = _t;
            x = _x;
            y = _y;
            v = _v;
            date = _date;
            date_str = str;
            enable = Helper.DateExceedRelax(_date, min, max);
        }
        public ItemCalendari(int _t, int _x, int _y, string _v, DateTime _date, DateTime? min, DateTime? max)
        {
            id = _x + "_" + _y;
            t = _t;
            x = _x;
            y = _y;
            v = _v;
            date = _date;
            date_str = _date.ToString("yyyy-MM-dd");
            enable = Helper.DateExceed(_date, min, max);
        }

        public string id { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public int t { get; private set; }
        public string v { get; set; }
        public DateTime date { get; set; }
        public string date_str { get; set; }
        public bool enable { get; set; }
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
        public bool Contains(int x, int y, ref int count)
        {
            if (rect.Contains(x, y))
            {
                if (hover) return true;
                hover = true;
                count++;
                return true;
            }
            else
            {
                if (hover)
                {
                    hover = false;
                    count++;
                }
                return false;
            }
        }
        public bool Contains(int x, int y) => rect.Contains(x, y);

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
        public bool Contains(int x, int y, ref int count)
        {
            if (rect.Contains(x, y))
            {
                if (hover) return true;
                hover = true;
                count++;
                return true;
            }
            else
            {
                if (hover)
                {
                    hover = false;
                    count++;
                }
                return false;
            }
        }
        public bool Contains(int x, int y) => rect.Contains(x, y);

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

    #endregion
}