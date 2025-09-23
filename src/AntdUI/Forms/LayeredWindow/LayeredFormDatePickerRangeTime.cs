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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormDatePickerRangeTime : ILayeredShadowForm
    {
        #region 初始化

        DatePickerRange control;
        DateTime? minDate, maxDate;
        TAMode ColorScheme;
        bool ShowH = false, ShowM = false, ShowS = false,
            ValueTimeHorizontal = false;
        public LayeredFormDatePickerRangeTime(DatePickerRange _control, bool endFocused, int bar, Action<DateTime[]> _action, Action<object> _action_btns, Func<DateTime[], List<DateBadge>?>? _badge_action)
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

            ShowH = control.Format.Contains("H");
            ShowM = control.Format.Contains("m");
            ShowS = control.Format.Contains("s");
            ValueTimeHorizontal = _control.ValueTimeHorizontal;

            showType = _control.Picker;
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

            #endregion

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = _control.radius;
            else
            {
                ArrowSize = (int)(8 * dpi);
                Radius = (int)(_control.radius * dpi);
            }
            SelDate = _control.Value;
            if (SelDate == null) Date = DateNow;
            else Date = EndFocused ? SelDate[1] : SelDate[0];

            LoadLayout();

            var tmpAlign = CLocation(_control, _control.Placement, _control.DropDownArrow, ArrowSize, true);
            if (_control.DropDownArrow) ArrowAlign = tmpAlign;
            if (OS.Win7OrLower) Select();
        }

        #region 箭头

        TAlign ArrowAlign = TAlign.None;
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

        #endregion

        DateTime DateNow = DateTime.Now;

        public DateTime[]? SelDate;
        DateTime? oldTime, oldTimeStart;

        DateTime _Date;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;
                calendar_day = CalendarHelper.Day(value, minDate, maxDate);

                calendar_time ??= CalendarHelper.Time(ShowH, ShowM, ShowS);

                calendar_month = CalendarHelper.Month(value, minDate, maxDate, Culture, MonthFormat);

                calendar_year = CalendarHelper.Year(value, minDate, maxDate, out year_str);

                rect_left.Enable = Helper.DateExceedMonth(value.AddMonths(-1), minDate, maxDate);
                rect_right.Enable = Helper.DateExceedMonth(value.AddMonths(1), minDate, maxDate);
                rect_lefts.Enable = Helper.DateExceedYear(value.AddYears(-1), minDate, maxDate);
                rect_rights.Enable = Helper.DateExceedYear(value.AddYears(1), minDate, maxDate);

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
        List<CalendarT>? calendar_time;
        List<CalendarButton>? left_buttons;
        ScrollBar? ScrollButtons;
        ScrollBar ScrollH, ScrollM, ScrollS;

        string year_str = "";

        public override string name => nameof(DatePicker);

        CultureInfo Culture;
        string CultureID = Localization.Get("ID", "zh-CN"),
            button_text = Localization.Get("Now", "此刻"), OKButton = Localization.Get("OK", "确定"),
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
                if (ArrowAlign != TAlign.None)
                {
                    if (AnimationBarValue != 0F) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, new RectangleF(rect.X + AnimationBarValue, rect.Y, rect.Width, rect.Height)));
                    else g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect));
                }
            }
        }

        StringFormat s_f = Helper.SF(), s_f_LE = Helper.SF_Ellipsis(lr: StringAlignment.Near);
        StringFormat s_f_L, s_f_R;
        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            using (var pen_arrow = new Pen(Colour.TextTertiary.Get("DatePicker", ColorScheme), 1.6F * Config.Dpi))
            using (var pen_arrow_hover = new Pen(Colour.Text.Get("DatePicker", ColorScheme), pen_arrow.Width))
            using (var pen_arrow_enable = new Pen(Colour.FillSecondary.Get("DatePicker", ColorScheme), pen_arrow.Width))
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
            var color_fore = Colour.TextBase.Get("DatePicker", ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                g.String(year_str, font, color_fore, rect_year2.Rect, s_f);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get("DatePicker", ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get("DatePicker", ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get("DatePicker", ColorScheme)))
            {
                foreach (var it in datas)
                {
                    var rect = rect_div[it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        if ((SelDate != null && oldTime == null && (EndFocused ? SelDate[1] : SelDate[0]).ToString("yyyy") == it.date_str) || (oldTime.HasValue && oldTime.Value.ToString("yyyy") == it.date_str))
                        {
                            g.Fill(Colour.Primary.Get("DatePicker", ColorScheme), path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get("DatePicker", ColorScheme), rect_div[it.id].Rect, s_f);
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
                        if (DateNow.ToString("yyyy") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), bor, path);
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
            var color_fore = Colour.TextBase.Get("DatePicker", ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture);
                if (rect_year2.Hover) g.String(yearStr, font, Colour.Primary.Get("DatePicker", ColorScheme), rect_year2.Rect, s_f);
                else g.String(yearStr, font, color_fore, rect_year2.Rect, s_f);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get("DatePicker", ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get("DatePicker", ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get("DatePicker", ColorScheme)))
            {
                foreach (var it in datas)
                {
                    var rect = rect_div[it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        if ((SelDate != null && oldTime == null && (EndFocused ? SelDate[1] : SelDate[0]).ToString("yyyy-MM") == it.date_str) || (oldTime.HasValue && oldTime.Value.ToString("yyyy-MM") == it.date_str))
                        {
                            g.Fill(Colour.Primary.Get("DatePicker", ColorScheme), path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get("DatePicker", ColorScheme), rect_div[it.id].Rect, s_f);
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
                        if (DateNow.ToString("yyyy-MM") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), bor, path);
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
            var color_fore = Colour.TextBase.Get("DatePicker", ColorScheme);
            using (var font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                string yearStr = _Date.ToString(YearFormat, Culture), monthStr = _Date.ToString(MonthFormat, Culture);
                if (rect_year.Hover) g.String(yearStr, font, Colour.Primary.Get("DatePicker", ColorScheme), rect_year.Rect, s_f_L);
                else g.String(yearStr, font, color_fore, rect_year.Rect, s_f_L);

                if (rect_month.Hover) g.String(monthStr, font, Colour.Primary.Get("DatePicker", ColorScheme), rect_month.Rect, s_f_R);
                else g.String(monthStr, font, color_fore, rect_month.Rect, s_f_R);
            }

            using (var brush_split = new SolidBrush(Colour.Split.Get("DatePicker", ColorScheme)))
            {
                foreach (var it in rects_split) g.Fill(brush_split, it);
            }
            using (var brush = new SolidBrush(Colour.Text.Get("DatePicker", ColorScheme)))
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
            using (var brush_fore_disable = new SolidBrush(Colour.TextQuaternary.Get("DatePicker", ColorScheme)))
            using (var brush_bg_disable = new SolidBrush(Colour.FillTertiary.Get("DatePicker", ColorScheme)))
            using (var brush_active = new SolidBrush(Colour.Primary.Get("DatePicker", ColorScheme)))
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
                    ScrollButtons.Paint(g);
                }
                if (calendar_time != null)
                {
                    var state2 = g.Save();
                    using (var brush_bg = new SolidBrush(Colour.PrimaryBg.Get("DatePicker", ColorScheme)))
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
                                if (oldTime.HasValue)
                                {
                                    var tmp = oldTime.Value;
                                    switch (it.rx)
                                    {
                                        case 0:
                                            if (it.t == tmp.Hour) g.Fill(brush_bg, path);
                                            break;
                                        case 1:
                                            if (it.t == tmp.Minute) g.Fill(brush_bg, path);
                                            break;
                                        case 2:
                                            if (it.t == tmp.Second) g.Fill(brush_bg, path);
                                            break;
                                    }
                                }
                                else if (SelDate != null)
                                {
                                    var tmp = EndFocused ? (oldTime ?? SelDate[1]) : SelDate[0];
                                    switch (it.rx)
                                    {
                                        case 0:
                                            if (it.t == tmp.Hour) g.Fill(brush_bg, path);
                                            break;
                                        case 1:
                                            if (it.t == tmp.Minute) g.Fill(brush_bg, path);
                                            break;
                                        case 2:
                                            if (it.t == tmp.Second) g.Fill(brush_bg, path);
                                            break;
                                    }
                                }
                                if (it.hover) g.Fill(brush_bg_disable, path);
                                g.String(it.v, Font, brush_fore, it.rect_read, s_f);
                            }
                        }
                    }
                    g.Restore(state2);
                    ScrollH.Paint(g);
                    ScrollM.Paint(g);
                    ScrollS.Paint(g);
                    if (rect_buttonok.Hover) g.String(OKButton, Font, Colour.PrimaryActive.Get("DatePicker", ColorScheme), rect_buttonok.Rect, s_f);
                    else g.String(OKButton, Font, brush_active, rect_buttonok.Rect, s_f);
                }

                foreach (var it in datas)
                {
                    var rect = rect_div[it.id];
                    using (var path = rect.RectRead.RoundPath(Radius))
                    {
                        if ((SelDate != null && oldTime == null && (EndFocused ? SelDate[1] : SelDate[0]).ToString("yyyy-MM-dd") == it.date_str) || (oldTime.HasValue && oldTime.Value.ToString("yyyy-MM-dd") == it.date_str))
                        {
                            g.Fill(brush_active, path);
                            g.String(it.v, Font, Colour.PrimaryColor.Get("DatePicker", ColorScheme), rect_div[it.id].Rect, s_f);
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
                        if (DateNow.ToString("yyyy-MM-dd") == it.date_str) g.Draw(Colour.Primary.Get("DatePicker", ColorScheme), bor, path); ;
                    }
                }
                if (badge_list.Count > 0)
                {
                    foreach (var it in datas)
                    {
                        if (badge_list.TryGetValue(it.date_str, out var find)) control.PaintBadge(find, rect_div[it.id].Rect, g);
                    }
                }

                if (rect_button.Hover) g.String(button_text, Font, Colour.PrimaryActive.Get("DatePicker", ColorScheme), rect_button.Rect, s_f);
                else g.String(button_text, Font, brush_active, rect_button.Rect, s_f);
            }
        }

        #endregion

        #region 布局

        TDatePicker showType;
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
            float line2 = Config.Dpi, line = line2 / 2;
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

                r_h = t_top + t_button + sp2 * 2 + item_size * 7;

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

                #region Time

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

                    if (SelDate != null) ScrollTime(calendar_time, oldTime ?? (EndFocused ? SelDate[1] : SelDate[0]));
                }

                dlist.Add(new RectangleF(rw - line, 0, line2, r_h));
                rect_buttonok.SetRect(rw, rect_button.Rect.Y, t_time_w, t_button);
                rw += t_time_w;

                #endregion

                dlist.Add(new RectangleF(t_x, rect_button.Rect.Y - line, rw - t_x, line2));
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
                    if (rect_button.Contains(x, y, ref count)) hand++;
                    if (rect_buttonok.Contains(x, y, ref count)) hand++;
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
                        else if (rect_button.Contains(x, y))
                        {
                            //此刻
                            oldTime = Date = DateNow = DateTime.Parse(DateTime.Now.ToString(control.Format));
                            Print();
                            ScrollTime(calendar_time!, oldTime.Value);
                        }
                        else if (rect_buttonok.Contains(x, y))
                        {
                            if (SelDate == null)
                            {
                                if (oldTime.HasValue)
                                {
                                    if (EndFocused)
                                    {
                                        if (oldTimeStart.HasValue)
                                        {
                                            if (oldTime.Value > oldTimeStart.Value) SelDate = new DateTime[] { oldTimeStart.Value, oldTime.Value };
                                            else SelDate = new DateTime[] { oldTime.Value, oldTimeStart.Value };
                                            action(SelDate);
                                            IClose();
                                        }
                                        return;
                                    }
                                    oldTimeStart = oldTime;
                                    oldTime = null;
                                    EndFocused = true;
                                    Print();
                                }
                            }
                            else
                            {
                                if (EndFocused)
                                {
                                    if (oldTime.HasValue)
                                    {
                                        SelDate[1] = oldTime.Value;
                                        action(SelDate);
                                    }
                                    IClose();
                                }
                                else
                                {
                                    if (oldTime.HasValue)
                                    {
                                        SelDate[0] = oldTime.Value;
                                        action(SelDate);
                                    }
                                    IClose();
                                }
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
                                            if (oldTime.HasValue) oldTime = new DateTime(it.date.Year, it.date.Month, it.date.Day, oldTime.Value.Hour, oldTime.Value.Minute, oldTime.Value.Second);
                                            else if (SelDate == null)
                                            {
                                                DateNow = DateTime.Now;
                                                oldTime = new DateTime(it.date.Year, it.date.Month, it.date.Day, DateNow.Hour, DateNow.Minute, DateNow.Second);
                                            }
                                            else
                                            {
                                                var tmp = EndFocused ? SelDate[1] : SelDate[0];
                                                oldTime = new DateTime(it.date.Year, it.date.Month, it.date.Day, tmp.Hour, tmp.Minute, tmp.Second);
                                            }
                                            Print();
                                            if (calendar_time != null) ScrollTime(calendar_time, oldTime.Value);
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
                                                if (oldTime.HasValue) oldTime = new DateTime(oldTime.Value.Year, oldTime.Value.Month, oldTime.Value.Day, oldTime.Value.Hour, it.t, oldTime.Value.Second);
                                                else if (SelDate == null)
                                                {
                                                    DateNow = DateTime.Now;
                                                    oldTime = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 0, it.t, 0);
                                                }
                                                else
                                                {
                                                    var tmp = EndFocused ? SelDate[1] : SelDate[0];
                                                    oldTime = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour, it.t, tmp.Second);
                                                }
                                                Print();
                                                if (ValueTimeHorizontal) ScrollTime(calendar_time, oldTime.Value);
                                                return;
                                            }
                                            break;
                                        case 2:
                                            if (it.Contains(x, y + ScrollS.Value))
                                            {
                                                if (oldTime.HasValue) oldTime = new DateTime(oldTime.Value.Year, oldTime.Value.Month, oldTime.Value.Day, oldTime.Value.Hour, oldTime.Value.Minute, it.t);
                                                else if (SelDate == null)
                                                {
                                                    DateNow = DateTime.Now;
                                                    oldTime = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 0, 0, it.t);
                                                }
                                                else
                                                {
                                                    var tmp = EndFocused ? SelDate[1] : SelDate[0];
                                                    oldTime = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute, it.t);
                                                }
                                                Print();
                                                if (ValueTimeHorizontal) ScrollTime(calendar_time, oldTime.Value);
                                                return;
                                            }
                                            break;
                                        case 0:
                                        default:
                                            if (it.Contains(x, y + ScrollH.Value))
                                            {
                                                if (oldTime.HasValue) oldTime = new DateTime(oldTime.Value.Year, oldTime.Value.Month, oldTime.Value.Day, it.t, oldTime.Value.Minute, oldTime.Value.Second);
                                                else if (SelDate == null)
                                                {
                                                    DateNow = DateTime.Now;
                                                    oldTime = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, it.t, 0, 0);
                                                }
                                                else
                                                {
                                                    var tmp = EndFocused ? SelDate[1] : SelDate[0];
                                                    oldTime = new DateTime(tmp.Year, tmp.Month, tmp.Day, it.t, tmp.Minute, tmp.Second);
                                                }
                                                Print();
                                                if (ValueTimeHorizontal) ScrollTime(calendar_time, oldTime.Value);
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
                        if (calendar_year != null)
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

        protected override void Dispose(bool disposing)
        {
            s_f.Dispose();
            s_f_LE.Dispose();
            s_f_L.Dispose();
            s_f_R.Dispose();

            base.Dispose(disposing);
        }
    }
}