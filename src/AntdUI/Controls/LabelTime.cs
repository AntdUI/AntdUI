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
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// LabelTime 时间文本
    /// </summary>
    /// <remarks>显示时间文本。</remarks>
    [Description("LabelTime 时间文本")]
    [ToolboxItem(true)]
    [DefaultProperty("ShowTime")]
    public class LabelTime : IControl
    {
        #region 属性

        bool showTime = true;
        [Description("外观"), Category("是否显示秒"), DefaultValue(true)]
        public bool ShowTime
        {
            get => showTime;
            set
            {
                if (showTime == value) return;
                showTime = value;
                Invalidate();
            }
        }

        #endregion

        public LabelTime()
        {
            new Thread(TaskLong)
            {
                IsBackground = true
            }.Start();
        }

        string show_tmp = "";
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            var g = e.Graphics.High();
            string[] time = GTime();
            show_tmp = string.Join("", time);
            using (var brush_sub = new SolidBrush(ForeColor.rgba(.8F)))
            using (var font = new Font(Font.FontFamily, rect.Height * .72F, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                var size = g.MeasureString(time[0], font, 10000, s_f_l);
                var rect_time = new Rectangle(rect.X, rect.Y, size.Width, rect.Height);
                g.String(time[1], font, ForeColor, rect_time, s_f_l);
                int h2 = rect_time.Height / 2, r = rect_time.Width + (int)(size.Height * .24F), w2 = rect.Width - r;
                using (var font_sub = new Font(Font.FontFamily, font.Size * .36F, GraphicsUnit.Pixel))
                {
                    g.String(time[2], font_sub, brush_sub, new Rectangle(rect.X + r, rect.Y, w2, h2), s_f_r1);
                    g.String(time[3], font_sub, brush_sub, new Rectangle(rect.X + r, rect.Y + h2, w2, h2), s_f_r2);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        readonly StringFormat s_f_l = Helper.SF_NoWrap(StringAlignment.Center, StringAlignment.Far),
            s_f_r1 = Helper.SF_NoWrap(StringAlignment.Far, StringAlignment.Near), s_f_r2 = Helper.SF_NoWrap(StringAlignment.Near, StringAlignment.Near);

        void TaskLong()
        {
            while (!IsDisposed)
            {
                Thread.Sleep(ShowTime ? 1000 : 10000);
                if (IsDisposed) break;
                string[] time = GTime();
                string tmp = string.Join("", time);
                if (tmp != show_tmp) Invalidate();
            }
        }

        string[] GTime()
        {
            DateTime now = DateTime.Now;
            if (ShowTime)
            {
                return new string[4]
                {
                "24:59:59",
                now.ToString("HH:mm:ss"),
                now.ToString("MM-dd"),
                now.ToString("dddd")
                };
            }
            return new string[4]
            {
            "24:59",
            now.ToString("HH:mm"),
            now.ToString("MM-dd"),
            now.ToString("dddd")
            };
        }
    }
}