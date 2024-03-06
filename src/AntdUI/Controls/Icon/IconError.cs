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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI.Icon
{
    /// <summary>
    /// 失败图标
    /// </summary>
    [Description("Icon 失败图标")]
    [ToolboxItem(true)]
    public class IconError : IControl
    {
        #region 属性

        Color? back;
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        Color? color;
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        public Color? Color
        {
            get => color;
            set
            {
                if (color == value) return;
                color = value;
                Invalidate();
            }
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            var g = e.Graphics.High();
            float dot_size = rect.Width > rect.Height ? rect.Height : rect.Width;
            var rect_dot = new RectangleF((rect.Width - dot_size) / 2, (rect.Height - dot_size) / 2, dot_size, dot_size);
            using (var brush = back.Brush(Style.Db.Error))
            {
                g.FillEllipse(brush, rect_dot);
            }
            g.PaintIconError(rect_dot, color.HasValue ? color.Value : Style.Db.BgBase);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        internal PointF[] PaintArrow(RectangleF rect)
        {
            float size = rect.Height * 0.15F, size2 = rect.Height * 0.2F, size3 = rect.Height * 0.26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        #endregion

        #endregion
    }
}