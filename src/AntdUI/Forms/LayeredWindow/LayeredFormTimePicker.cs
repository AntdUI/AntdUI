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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormTimePicker : ILayeredShadowForm
    {
        TAMode ColorScheme;
        public LayeredFormTimePicker(TimePicker control, TimeSpan date, Action<TimeSpan> _action)
        {
            PARENT = control;
            Font = control.Font;
            ColorScheme = control.ColorScheme;
            control.Parent.SetTopMost(Handle);
            ValueTimeHorizontal = control.ValueTimeHorizontal;
            ShowButtonNow = control.ShowButtonNow;
            ShowH = control.Format.Contains("H");
            ShowM = control.Format.Contains("m");
            ShowS = control.Format.Contains("s");
            action = _action;
            SelDate = date;

            ScrollH = new ScrollBar(this, control.ColorScheme);
            ScrollM = new ScrollBar(this, control.ColorScheme);
            ScrollS = new ScrollBar(this, control.ColorScheme);
            hover_button = new ITaskOpacity(name, this);
            hover_buttonok = new ITaskOpacity(name, this);

            #region 数据

            calendar_time = new List<CalendarT>(24 + 120);

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

            #endregion

            float dpi = Config.Dpi;
            if (dpi == 1F) Radius = control.Radius;
            else
            {
                ArrowSize = (int)(8 * dpi);
                Radius = (int)(control.Radius * dpi);
            }
            LoadLayout(count);
            ScrollTime();
            var tmpAlign = CLocation(control, control.Placement, control.DropDownArrow, ArrowSize);
            if (control.DropDownArrow) ArrowAlign = tmpAlign;
            if (OS.Win7OrLower) Select();
        }

        public override string name => nameof(TimePicker);

        void ScrollTime()
        {
            CalendarT? find_h = calendar_time.Find(a => a.rx == 0 && a.t == SelDate.Hours),
                find_m = calendar_time.Find(a => a.rx == 1 && a.t == SelDate.Minutes),
                find_s = calendar_time.Find(a => a.rx == 2 && a.t == SelDate.Seconds);

            if (find_h != null) ScrollH.Value = find_h.rect.Y;
            if (find_m != null) ScrollM.Value = find_m.rect.Y;
            if (find_s != null) ScrollS.Value = find_s.rect.Y;
        }

        #region 属性

        internal TimeSpan SelDate;

        #region 参数

        bool ShowH = true, ShowM = true, ShowS = true, ShowButtonNow = true, ValueTimeHorizontal = false;

        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;

        ScrollBar ScrollH, ScrollM, ScrollS;

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

        string button_text = Localization.Get("Now", "此刻");
        string OKButton = Localization.Get("OK", "确定");
        StringFormat s_f = Helper.SF();
        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(Colour.BgElevated.Get(nameof(DatePicker), ColorScheme)))
            {
                g.Fill(brush, path);
                if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect));
            }
        }
        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            using (var brush_fore = new SolidBrush(Colour.TextBase.Get(nameof(DatePicker), ColorScheme)))
            using (var brush_bg = new SolidBrush(Colour.PrimaryBg.Get(nameof(DatePicker), ColorScheme)))
            {
                var state2 = g.Save();
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
                        switch (it.rx)
                        {
                            case 0:
                                if (it.t == SelDate.Hours) g.Fill(brush_bg, path);
                                break;
                            case 1:
                                if (it.t == SelDate.Minutes) g.Fill(brush_bg, path);
                                break;
                            case 2:
                                if (it.t == SelDate.Seconds) g.Fill(brush_bg, path);
                                break;
                        }
                        if (it.hover) g.Fill(Colour.FillTertiary.Get(nameof(DatePicker), ColorScheme), path);
                        g.String(it.v, Font, brush_fore, it.rect_read, s_f);
                    }
                }
                g.Restore(state2);
                ScrollH.Paint(g);
                ScrollM.Paint(g);
                ScrollS.Paint(g);

                var color_active = Colour.Primary.Get(nameof(DatePicker), ColorScheme);

                if (ShowButtonNow)
                {
                    if (hover_button.Animation) g.String(button_text, Font, color_active.BlendColors(hover_button.Value, Colour.PrimaryActive.Get(nameof(DatePicker), ColorScheme)), rect_button, s_f);
                    else if (hover_button.Switch) g.String(button_text, Font, Colour.PrimaryActive.Get(nameof(DatePicker), ColorScheme), rect_button, s_f);
                    else g.String(button_text, Font, color_active, rect_button, s_f);
                }

                if (hover_buttonok.Animation) g.String(OKButton, Font, color_active.BlendColors(hover_buttonok.Value, Colour.PrimaryActive.Get(nameof(DatePicker), ColorScheme)), rect_buttonok, s_f);
                else if (hover_buttonok.Switch) g.String(OKButton, Font, Colour.PrimaryActive.Get(nameof(DatePicker), ColorScheme), rect_buttonok, s_f);
                else g.String(OKButton, Font, color_active, rect_buttonok, s_f);
            }
        }

        Rectangle rect_read_h, rect_read_m, rect_read_s;

        #endregion

        #region 布局

        void LoadLayout(int count)
        {
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font);
                int sp = (int)(size.Height * 0.2F), sp2 = sp * 2;
                int t_time_height = (int)(size.Height * 1.56F),
                t_time = (int)(size.Height * 2.9F),
                t_button = (int)(size.Height * 1.96F),
                t_height = t_time_height * 7,
                size_time_one = (int)(t_time * .857F), size_time_height_one = (int)(t_time_height * .93F), tmp = 0;

                if (ShowH)
                {
                    rect_read_h = new Rectangle(tmp, 0, t_time, t_height);
                    ScrollH.SizeChange(rect_read_h);
                    tmp += t_time;
                }
                if (ShowM)
                {
                    rect_read_m = new Rectangle(tmp, 0, t_time, t_height);
                    ScrollM.SizeChange(rect_read_m);
                    tmp += t_time;
                }
                if (ShowS)
                {
                    rect_read_s = new Rectangle(tmp, 0, t_time, t_height);
                    ScrollS.SizeChange(rect_read_s);
                    tmp += t_time;
                }

                if (ValueTimeHorizontal)
                {
                    int exceed = t_height / t_time_height;
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
                    it.rect = new Rectangle(t_time * it.x, t_time_height * it.y, t_time, t_time_height);
                    it.rect_read = new Rectangle(it.rect.X + _x, it.rect.Y + _y, size_time_one, size_time_height_one);
                }

                int t_width = t_time * count;
                SetSize(t_width, t_height + t_button);
                rect_button = new Rectangle(0, t_height, t_width / 2, t_button);
                if (ShowButtonNow) rect_buttonok = new Rectangle(rect_button.Right, rect_button.Top, rect_button.Width, rect_button.Height);
                else rect_buttonok = new Rectangle(rect_button.X, rect_button.Top, t_width, rect_button.Height);
            });
        }

        #endregion

        #region 鼠标

        ITaskOpacity hover_button, hover_buttonok;
        Rectangle rect_button = new Rectangle(-20, -20, 10, 10), rect_buttonok = new Rectangle(-20, -20, 10, 10);

        protected override void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ShowH && rect_read_h.Contains(x, y)) ScrollH.MouseDown(x, y);
            else if (ShowM && rect_read_m.Contains(x, y)) ScrollM.MouseDown(x, y);
            else if (ShowS && rect_read_s.Contains(x, y)) ScrollS.MouseDown(x, y);
        }

        protected override void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollH.MouseMove(x, y) && ScrollM.MouseMove(x, y) && ScrollS.MouseMove(x, y))
            {
                int count = 0, hand = 0;
                bool _hover_button = ShowButtonNow && rect_button.Contains(x, y),
                 _hover_buttonok = rect_buttonok.Contains(x, y);

                if (_hover_button != hover_button.Switch) count++;
                if (_hover_buttonok != hover_buttonok.Switch) count++;

                hover_button.Switch = _hover_button;
                hover_buttonok.Switch = _hover_buttonok;
                if (hover_button.Switch || hover_buttonok.Switch) hand++;
                else
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
                if (count > 0) Print();
                SetCursor(hand > 0);
            }
            else SetCursor(false);
        }

        protected override void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (ScrollH.MouseUp() && ScrollM.MouseUp() && ScrollS.MouseUp())
            {
                if (button == MouseButtons.Left)
                {
                    if (ShowButtonNow && rect_button.Contains(x, y))
                    {
                        DateNow = DateTime.Now;
                        SelDate = new TimeSpan(DateNow.Hour, DateNow.Minute, DateNow.Second);
                        action(SelDate);
                        ScrollTime();
                        Print();
                        return;
                    }
                    else if (rect_buttonok.Contains(x, y))
                    {
                        action(SelDate);
                        IClose();
                        return;
                    }

                    foreach (var it in calendar_time)
                    {
                        switch (it.rx)
                        {
                            case 1:
                                if (it.Contains(x, y + ScrollM.Value))
                                {
                                    SelDate = new TimeSpan(SelDate.Hours, it.t, SelDate.Seconds);
                                    Print();
                                    if (ValueTimeHorizontal) ScrollTime();
                                    return;
                                }
                                break;
                            case 2:
                                if (it.Contains(x, y + ScrollS.Value))
                                {
                                    SelDate = new TimeSpan(SelDate.Hours, SelDate.Minutes, it.t);
                                    Print();
                                    if (ValueTimeHorizontal) ScrollTime();
                                    return;
                                }
                                break;
                            case 0:
                            default:
                                if (it.Contains(x, y + ScrollH.Value))
                                {
                                    SelDate = new TimeSpan(it.t, SelDate.Minutes, SelDate.Seconds);
                                    Print();
                                    if (ValueTimeHorizontal) ScrollTime();
                                    return;
                                }
                                break;
                        }
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (RunAnimation) return;
            ScrollH.Leave();
            ScrollM.Leave();
            ScrollS.Leave();
            foreach (var it in calendar_time) it.hover = false;
            SetCursor(false);
            Print();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (delta != 0)
            {
                if (ShowH && rect_read_h.Contains(x, y)) ScrollH.MouseWheel(delta);
                else if (ShowM && rect_read_m.Contains(x, y)) ScrollM.MouseWheel(delta);
                else if (ShowS && rect_read_s.Contains(x, y)) ScrollS.MouseWheel(delta);
            }
        }

        #endregion
    }
}