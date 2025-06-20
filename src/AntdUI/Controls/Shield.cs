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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Shield 徽章
    /// </summary>
    /// <remarks>展示设备信号。</remarks>
    [Description("Shield 徽章")]
    [ToolboxItem(true)]
    public class Shield:IControl
    {
        #region 属性默认值
        public Shield()
        {
            base.Padding = new Padding(5);
            SubjectColor = Color.FromArgb(84, 84, 84);
            StatusColor = Color.FromArgb(17, 130, 195);
            SubjectText = "Licenses";
            StatusText = "Apache 2.0";
        }
        #endregion

        #region 属性

        Color? subjectColor;
        /// <summary>
        /// 填充颜色
        /// </summary>
        [Description("标题颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? SubjectColor
        {
            get => subjectColor;
            set
            {
                if (subjectColor == value) return;
                subjectColor = value;
                Invalidate();
                OnPropertyChanged(nameof(SubjectColor));
            }
        }


        Color? statusColor;
        /// <summary>
        /// 填充颜色
        /// </summary>
        [Description("状态颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? StatusColor
        {
            get => statusColor;
            set
            {
                if (statusColor == value) return;
                statusColor = value;
                Invalidate();
                OnPropertyChanged(nameof(StatusColor));
            }
        }

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
                OnPropertyChanged(nameof(Radius));
            }
        }
        #region 文本

        string? subject;
        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("标题"), Category("外观"), DefaultValue(null)]
        public string? SubjectText
        {
            get => subject;
            set
            {
                if (subject == value) return;
                subject = value;
                Invalidate();
                OnPropertyChanged(nameof(SubjectText));
            }
        }

        string? status;
        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("状态"), Category("外观"), DefaultValue(null)]
        public string? StatusText
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;
                Invalidate();
                OnPropertyChanged(nameof(StatusText));
            }
        }

        #endregion

        #endregion
        #region 渲染
        protected override void OnPaint(PaintEventArgs e)
        {
            var _rect = ClientRectangle;
            var g = e.Graphics.High();
            var subjectSize = g.MeasureString(SubjectText, Font);
            subjectSize = new Size(subjectSize.Width + Padding.Size.Width, subjectSize.Height + Padding.Size.Height);
            var statusSize = g.MeasureString(StatusText, Font);
            statusSize = new Size(statusSize.Width + Padding.Size.Width, statusSize.Height + Padding.Size.Height);
            var rect = new Rectangle((_rect.Width - (subjectSize.Width + statusSize.Width)) / 2, (_rect.Height - subjectSize.Height) / 2, subjectSize.Width + statusSize.Width, subjectSize.Height);
            float _radius = radius * Config.Dpi;
            using (var path_pain = rect.RoundPath(_radius))
            {
                using (var brush = new SolidBrush(StatusColor ?? Colour.Text.Get("Shield", ColorScheme)))
                {
                    g.Fill(brush, path_pain);
                }
                using (var bmp = new Bitmap(_rect.Width, _rect.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).High())
                    {
                        Color _color = SubjectColor?? Colour.Text.Get("Shield", ColorScheme);
                        g2.Fill(_color, path_pain);
                        g2.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        g2.Fill(Brushes.Transparent, new RectangleF(rect.X + subjectSize.Width, 0, rect.Width, bmp.Height));
                    }
                    g.Image(bmp, _rect);
                }
                var subjectRect = new Rectangle(rect.X + Padding.Left, rect.Y + Padding.Top, subjectSize.Width - Padding.Size.Width, subjectSize.Height - Padding.Size.Height);
                using (var brush = new SolidBrush(Color.White))
                {
                    g.String(SubjectText, Font, brush, subjectRect);
                }

                var statusRect = new Rectangle(rect.X + subjectSize.Width + Padding.Left, rect.Y + Padding.Top, statusSize.Width - Padding.Size.Width, statusSize.Height - Padding.Size.Height);
                using (var brush = new SolidBrush(Color.White))
                {
                    g.String(StatusText, Font, brush, statusRect);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }
    }

    #endregion

}
