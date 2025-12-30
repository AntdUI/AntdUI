// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public class CalendarPaintEventArgs : EventArgs
    {
        public CalendarPaintEventArgs(Canvas canvas, TDatePicker type, Rectangle rect, Rectangle rectreal, DateTime date, string text, bool enable, int radius)
        {
            g = canvas;
            Type = type;
            Rect = rect;
            RectReal = rectreal;
            Date = date;
            Text = text;
            Enable = enable;
            Radius = radius;
        }

        /// <summary>
        /// 画板
        /// </summary>
        public Canvas g { get; private set; }

        /// <summary>
        /// 区域
        /// </summary>
        public Rectangle Rect { get; private set; }

        /// <summary>
        /// 真实区域
        /// </summary>
        public Rectangle RectReal { get; private set; }

        public TDatePicker Type { get; private set; }

        public DateTime Date { get; private set; }

        public string Text { get; private set; }

        public bool Enable { get; private set; }

        public int Radius { get; private set; }
    }
    public class CalendarPaintBeginEventArgs : CalendarPaintEventArgs
    {
        public CalendarPaintBeginEventArgs(Canvas canvas, TDatePicker type, Rectangle rect, Rectangle rectreal, DateTime date, string text, bool enable, int radius) : base(canvas, type, rect, rectreal, date, text, enable, radius) { }

        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Handled { get; set; }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        #region 设置

        public CalendarPaintBeginEventArgs SetHandled(bool value = true)
        {
            Handled = value;
            return this;
        }
        public CalendarPaintBeginEventArgs SetOffset(int x, int y)
        {
            OffsetX = x;
            OffsetY = y;
            return this;
        }
        public CalendarPaintBeginEventArgs SetOffsetX(int value)
        {
            OffsetX = value;
            return this;
        }
        public CalendarPaintBeginEventArgs SetOffsetY(int value)
        {
            OffsetY = value;
            return this;
        }

        #endregion
    }

    public delegate void CalendarPaintEventHandler(object sender, CalendarPaintEventArgs e);
    public delegate void CalendarPaintBeginEventHandler(object sender, CalendarPaintBeginEventArgs e);

    public class CalendarMouseEventArgs : VMEventArgs<DateTime>
    {
        public CalendarMouseEventArgs(DateTime value, TDatePicker type, Rectangle rect, Rectangle rectreal, string text, bool enable, MouseEventArgs e) : base(value, e)
        {
            Type = type;
            Rect = rect;
            RectReal = rectreal;
            Text = text;
            Enable = enable;
        }

        /// <summary>
        /// 区域
        /// </summary>
        public Rectangle Rect { get; private set; }

        /// <summary>
        /// 真实区域
        /// </summary>
        public Rectangle RectReal { get; private set; }

        public TDatePicker Type { get; private set; }

        public string Text { get; private set; }

        public bool Enable { get; private set; }
    }

    /// <summary>
    /// DateTime 类型事件
    /// </summary>
    public delegate void CalendarMouseEventHandler(object sender, CalendarMouseEventArgs e);
}