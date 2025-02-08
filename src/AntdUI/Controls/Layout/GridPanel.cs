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
using System.Linq;
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
                if (IsHandleCreated) IOnSizeChanged();
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
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged("Gap");
            }
        }

        bool pauseLayout = false;
        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value)
                {
                    Invalidate();
                    IOnSizeChanged();
                }
                OnPropertyChanged("PauseLayout");
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
                if (container is GridPanel parent && parent.IsHandleCreated)
                {
                    if (parent.PauseLayout) return false;
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
                            // 最终坐标数据（列宽，行高）每一条数据为一行控件的数据
                            var data = new Dictionary<List<int>, int>();
                            // 分割成row跟columns
                            var temp = Span.Split('-', '\n');
                            // 分割为rows数组
                            var rows = temp[0].Split(';');
                            /* 分割columns数组，注意，还是以行为主，这个数组的count应与rows数组count对应。
                             * 即整行控件统一用一个行高。
                             * 同时兼容之前没有设置行高的代码
                             */
                            var columns = temp.Length == 2 ? temp[1]?.Split(' ') : new string[0];

                            // 已使用行高
                            int use_height = 0;

                            for (int i = 0; i < rows.Length; i++)
                            {
                                var row = rows[i];

                                if (!string.IsNullOrEmpty(row))
                                {
                                    // 获得当前行的列数量（也就是控件数量）
                                    var abs = row.Split(' ', ',');
                                    // 定义当前行的控件x坐标(列宽)
                                    var xObjTemp = new List<object>(abs.Length);
                                    // 已使用列宽
                                    int use_width = 0;

                                    foreach (string xaxis in abs)
                                    {
                                        var x = xaxis.Trim();
                                        if (x.EndsWith("%") && float.TryParse(x.TrimEnd('%'), out var xF)) xObjTemp.Add(xF / 100F);
                                        else if (int.TryParse(x, out var xi))
                                        {
                                            int uw = (int)Math.Round(xi * Config.Dpi);
                                            xObjTemp.Add(uw);
                                            use_width += uw;
                                        }
                                        else if (float.TryParse(x, out float xF2)) xObjTemp.Add(xF2);
                                    }

                                    int read_width = rect.Width - use_width;
                                    var x_temp = new List<int>(xObjTemp.Count);

                                    foreach (var it in xObjTemp)
                                    {
                                        if (it is float f) x_temp.Add((int)Math.Round(read_width * f));
                                        else if (it is int ix) x_temp.Add(ix);
                                    }

                                    // 转换后实际行高
                                    int height = 0;
                                    if (columns != null && columns.Length > 0)
                                    {
                                        if (i < columns.Length)
                                        {
                                            // 获得当前行的行高
                                            var yaxis = columns[i];
                                            var y = yaxis.Trim();

                                            // 剩余行高
                                            int read_height = rect.Height - use_height;
                                            if (y.EndsWith("%") && float.TryParse(y.TrimEnd('%'), out var yF)) height = (int)Math.Round(read_height * (yF / 100F));
                                            else if (int.TryParse(y, out var iy))
                                            {
                                                int uh = (int)Math.Round(iy * Config.Dpi);
                                                height = uh;
                                                use_height += uh;
                                            }
                                            else if (float.TryParse(y, out float yF2)) height = (int)Math.Round(read_height * yF2);
                                        }
                                        else height = -999;
                                    }
                                    else height = -999;

                                    if (x_temp.Count > 0)
                                        data.Add(x_temp, height);
                                }
                            }

                            if (data.Count > 0)
                            {
                                Rectangle[] rects;
                                if (data.Count > 1)
                                {
                                    var tmp_rects = new List<Rectangle>();
                                    int hasx = 0, hasy = 0;

                                    foreach (var item in data)
                                    {
                                        int y = item.Value == -999 ? rect.Height / data.Count : item.Value;
                                        foreach (var x in item.Key)
                                        {
                                            tmp_rects.Add(new Rectangle(rect.X + hasx, rect.Y + hasy, x, y));
                                            hasx += x;
                                        }
                                        hasx = 0;
                                        hasy += y;
                                    }
                                    rects = tmp_rects.ToArray();
                                }
                                else
                                {
                                    var xt = data.First().Key;
                                    var tmp_rects = new List<Rectangle>(xt.Count);
                                    int hasx = 0, hasy = 0;
                                    foreach (var x in xt)
                                    {
                                        tmp_rects.Add(new Rectangle(rect.X + hasx, rect.Y + hasy, x, rect.Height));
                                        hasx += x;
                                    }
                                    rects = tmp_rects.ToArray();
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