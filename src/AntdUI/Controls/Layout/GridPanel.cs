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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace AntdUI
{
    /// <summary>
    /// GridPanel 格栅布局
    /// </summary>
    [Description("GridPanel 格栅布局")]
    [ToolboxItem(true)]
    [DefaultProperty("Span")]
    [Designer(typeof(IControlDesigner))]
    public class GridPanel : IControl
    {
        public override Rectangle DisplayRectangle => ClientRectangle.DeflateRect(Padding);

        /// <summary>
        /// 跨度
        /// </summary>
        [Description("跨度"), Category("外观"), DefaultValue("50% 50%;50% 50%")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        public string Span
        {
            get => layoutengine.Span;
            set
            {
                if (layoutengine.Span == value) return;
                layoutengine.Span = value;
                IOnSizeChanged();
                OnPropertyChanged("Span");
            }
        }

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => layoutengine.Gap;
            set
            {
                if (layoutengine.Gap == value) return;
                layoutengine.Gap = value;
                IOnSizeChanged();
                OnPropertyChanged("Gap");
            }
        }

        #region 布局

        protected override void OnHandleCreated(EventArgs e)
        {
            IOnSizeChanged();
            base.OnHandleCreated(e);
        }

        GridLayout layoutengine = new GridLayout();
        public override LayoutEngine LayoutEngine => layoutengine;
        internal class GridLayout : LayoutEngine
        {
            /// <summary>
            /// 内容大小
            /// </summary>
            public string Span { get; set; } = "50% 50%;50% 50%";

            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; }

            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                if (container is GridPanel parent)
                {
                    var rect = parent.DisplayRectangle;
                    if (!string.IsNullOrEmpty(Span) && parent.Controls.Count > 0)
                    {
                        var controls = new List<Control>(parent.Controls.Count);
                        foreach (Control it in parent.Controls)
                        {
                            if (it.Visible) controls.Insert(0, it);
                        }
                        if (controls.Count > 0)
                        {
                            int data_count = 0;
                            var rows = Span.Split(';', '\n');
                            var data = new List<int[]>(rows.Length);
                            foreach (var row in rows)
                            {
                                if (!string.IsNullOrEmpty(row))
                                {
                                    var cels = row.Split(' ', ',');
                                    var cels_tmp = new List<object>(cels.Length);
                                    int use_width = 0;
                                    foreach (string it in cels)
                                    {
                                        var cel = it.Trim();
                                        if (cel.EndsWith("%") && float.TryParse(cel.TrimEnd('%'), out var f)) cels_tmp.Add(f / 100F);
                                        else if (int.TryParse(cel, out var i))
                                        {
                                            int uw = (int)Math.Round(i * Config.Dpi);
                                            cels_tmp.Add(uw);
                                            use_width += uw;
                                        }
                                        else if (float.TryParse(cel, out float f2)) cels_tmp.Add(f2);
                                    }
                                    int read_width = rect.Width - use_width;
                                    var cel_tmp = new List<int>(cels_tmp.Count);
                                    foreach (var it in cels_tmp)
                                    {
                                        if (it is float f) cel_tmp.Add((int)Math.Round(read_width * f));
                                        else if (it is int i) cel_tmp.Add(i);
                                    }
                                    if (cel_tmp.Count > 0)
                                    {
                                        data_count += cel_tmp.Count;
                                        data.Add(cel_tmp.ToArray());
                                    }
                                }
                            }
                            if (data_count > 0)
                            {
                                Rectangle[] rects;
                                if (data_count == 1) rects = new Rectangle[1] { rect };
                                else
                                {
                                    if (data.Count > 1)
                                    {
                                        int height_one = rect.Height / data.Count;
                                        var tmp_rects = new List<Rectangle>();
                                        int hasx = 0, hasy = 0;
                                        foreach (var item in data)
                                        {
                                            foreach (var it in item)
                                            {
                                                tmp_rects.Add(new Rectangle(rect.X + hasx, rect.Y + hasy, it, height_one));
                                                hasx += it;
                                            }
                                            hasx = 0;
                                            hasy += height_one;
                                        }
                                        rects = tmp_rects.ToArray();
                                    }
                                    else
                                    {
                                        var tmp_rects = new List<Rectangle>(data[0].Length);
                                        int hasx = 0, hasy = 0;
                                        foreach (var it in data[0])
                                        {
                                            tmp_rects.Add(new Rectangle(rect.X + hasx, rect.Y + hasy, it, rect.Height));
                                            hasx += it;
                                        }
                                        rects = tmp_rects.ToArray();
                                    }
                                }
                                HandLayout(controls, rects);
                            }
                        }
                    }
                }
                return false;
            }

            void HandLayout(List<Control> controls, Rectangle[] rects)
            {
                int gap = (int)Math.Round(Gap * Config.Dpi), gap2 = gap * 2;
                int max_len = controls.Count;
                if (rects.Length < controls.Count) max_len = rects.Length;
                for (int i = 0; i < max_len; i++)
                {
                    Control control = controls[i];
                    Point point = rects[i].Location;
                    point.Offset(control.Margin.Left + gap, control.Margin.Top + gap);
                    control.Location = point;
                    control.Size = new Size(rects[i].Width - control.Margin.Horizontal - gap2, rects[i].Height - control.Margin.Vertical - gap2);
                }
            }
        }

        #endregion
    }
}