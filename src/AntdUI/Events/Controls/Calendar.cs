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
using System.Drawing;

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
    }

    public delegate void CalendarPaintEventHandler(object sender, CalendarPaintEventArgs e);
    public delegate void CalendarPaintBeginEventHandler(object sender, CalendarPaintBeginEventArgs e);
}