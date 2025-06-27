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
    [ProvideProperty("Index", typeof(Control))]
    public class GridPanel : ContainerPanel, IExtenderProvider
    {
        #region 属性

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
                OnPropertyChanged(nameof(Span));
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
                OnPropertyChanged(nameof(Gap));
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
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        #endregion

        #region Index 排序

        public bool CanExtend(object extendee) => extendee is Control control && control.Parent == this;

        Dictionary<Control, int> Map = new Dictionary<Control, int>();

        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("Index"), Description("排序"), DefaultValue(-1)]
        public int GetIndex(Control control) => IndexExists(control);

        public void SetIndex(Control control, int index)
        {
            int old = IndexExists(control);
            if (old == index) return;
            if (index > -1)
            {
                if (old == -1) Map.Add(control, index);
                else Map[control] = index;
            }
            else if (old > -1) Map.Remove(control);
            if (IsHandleCreated) IOnSizeChanged();
        }

        int IndexExists(Control control)
        {
            if (Map.TryGetValue(control, out int index)) return index;
            return -1;
        }

        #endregion

        #region 布局
        public override Rectangle DisplayRectangle => ClientRectangle.DeflateRect(Padding);

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

            enum RV_TYPE
            {
                CONTROL,
                SPRING
            }

            struct RV
            {
                public RV_TYPE type;
                public object value;
                public static implicit operator RV(int v) => new RV() { type = RV_TYPE.CONTROL, value = v };
                public static implicit operator int(RV v) => v.value is int ? (int)v.value : 0;

                public static implicit operator RV(float v) => new RV() { type = RV_TYPE.CONTROL, value = v };
                public static implicit operator float(RV v) => v.value is float ? (float)v.value : 0;

                public static implicit operator RV(RV_TYPE v) => new RV() { type = v };
                public static implicit operator RV_TYPE(RV v) => v.type;
                public bool I => value is int;
                public bool F => value is float;
            }

            struct RV_F
            {
                public RV_TYPE type;
                public float value;
                public static implicit operator RV_F(float v) => new RV_F() { type = RV_TYPE.CONTROL, value = v };
                public static implicit operator float(RV_F v) => v.value;

                public static implicit operator RV_F(RV_TYPE v) => new RV_F() { type = v };
                public static implicit operator RV_TYPE(RV_F v) => v.type;
            }

            struct RV_I
            {
                public RV_TYPE type;
                public int value;
                public static implicit operator RV_I(int v) => new RV_I() { type = RV_TYPE.CONTROL, value = v };
                public static implicit operator int(RV_I v) => v.value;

                public static implicit operator RV_I(RV_TYPE v) => new RV_I() { type = v };
                public static implicit operator RV_TYPE(RV_I v) => v.type;
            }

            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                if (container is GridPanel parent && parent.IsHandleCreated)
                {
                    if (parent.PauseLayout) return false;
                    var rect = parent.DisplayRectangle;
                    if (!string.IsNullOrEmpty(Span) && parent.Controls.Count > 0)
                    {
                        var controls = SyncMap(parent);
                        if (controls.Count > 0)
                        {
                            string[] tmp = Span.Split('-', '\n'), rows = tmp[0].Split(';');
                            var data = new List<List<RV_I>>(rows.Length);
                            int i = 0;
                            var celltmp = new Dictionary<int, RV>(rows.Length);
                            foreach (var it in rows)
                            {
                                if (string.IsNullOrEmpty(it)) continue;
                                string row = it;
                                int index = row.IndexOf(":");
                                if (index > -1)
                                {
                                    if (index == row.Length - 1) continue;
                                    if (index > 0)
                                    {
                                        var value = row.Substring(0, index);
                                        if (value.EndsWith("%") && float.TryParse(value.TrimEnd('%'), out var percentageValue)) celltmp.Add(i, percentageValue / 100F);
                                        else if (int.TryParse(value, out var intValue)) celltmp.Add(i, (int)Math.Round(intValue * Config.Dpi));
                                        else if (float.TryParse(value, out float floatValue)) celltmp.Add(i, floatValue);
                                        else continue;

                                        row = row.Substring(index + 1);
                                    }
                                    else row = row.Substring(1);
                                }
                                var x_tmp = GetRows(row, rect.Width);
                                if (x_tmp.Count > 0)
                                {
                                    data.Add(x_tmp);
                                    i++;
                                }
                            }
                            if (tmp.Length == 2)
                            {
                                i = 0;
                                foreach (var item in GetRows(tmp[1]))
                                {
                                    if (celltmp.ContainsKey(i))
                                    {
                                        i++;
                                        continue;
                                    }
                                    celltmp.Add(i, item);
                                    i++;
                                }
                            }
                            int real_height = rect.Height;
                            var cells = GetRows(celltmp, real_height, out real_height);
                            if (data.Count > 0)
                            {
                                Rectangle[] rects;
                                if (data.Count > 1)
                                {
                                    int rcount = data.Count - cells.Count;
                                    var tmp_rects = new List<Rectangle>();
                                    int hasx = 0, hasy = 0;
                                    i = 0;
                                    foreach (var item in data)
                                    {
                                        int y = cells.TryGetValue(i, out var value) ? value : real_height / rcount;
                                        foreach (var x in item)
                                        {
                                            if (x.type == RV_TYPE.CONTROL) tmp_rects.Add(new Rectangle(rect.X + hasx, rect.Y + hasy, x, y));
                                            hasx += x;
                                        }
                                        hasx = 0;
                                        hasy += y;
                                        i++;
                                    }
                                    rects = tmp_rects.ToArray();
                                }
                                else
                                {
                                    var xt = data[0];
                                    var tmp_rects = new List<Rectangle>(xt.Count);
                                    int hasx = 0, hasy = 0;
                                    foreach (var x in xt)
                                    {
                                        if (x.type == RV_TYPE.CONTROL) tmp_rects.Add(new Rectangle(rect.X + hasx, rect.Y + hasy, x, rect.Height));
                                        hasx += x;
                                    }
                                    rects = tmp_rects.ToArray();
                                }
                                HandLayout(controls, rects, rect);
                            }
                        }
                    }
                }
                return false;
            }

            #region 排序

            List<Control> SyncMap(GridPanel parent)
            {
                int count = parent.Controls.Count, i = count;
                var tmp = new List<IList>(count);
                foreach (Control it in parent.Controls)
                {
                    if (it.Visible)
                    {
                        if (parent.Map.TryGetValue(it, out int index)) tmp.Insert(0, new IList(it, index));
                        else tmp.Insert(0, new IList(it, i));
                        i--;
                    }
                }
                tmp.Sort((a, b) => a.Index.CompareTo(b.Index));
                var controls = new List<Control>(tmp.Count);
                foreach (var it in tmp) controls.Add(it.Control);
                return controls;
            }

            class IList
            {
                public IList(Control control, int index)
                {
                    Control = control;
                    Index = index;
                }

                public Control Control { get; set; }
                public int Index { get; set; }
            }

            #endregion

            List<RV_I> GetRows(string cells, int value) => GetRows(GetRows(cells), value);
            List<RV> GetRows(string cells)
            {
                var arr = cells.Split(' ', ',');
                var tmp = new List<RV>(arr.Length);
                foreach (string it in arr)
                {
                    var str = it.Trim();
                    RV tmp_rv; bool spring = false;
                    if (str.StartsWith("S"))
                    {
                        spring = true;
                        str = str.TrimStart('S');
                    }

                    if (spring && str.Length == 0) tmp_rv = 0;
                    else if (str.EndsWith("%") && float.TryParse(str.TrimEnd('%'), out var percentageValue)) tmp_rv = percentageValue / 100F;
                    else if (int.TryParse(str, out var intValue)) tmp_rv = (int)Math.Round(intValue * Config.Dpi);
                    else if (float.TryParse(str, out float floatValue)) tmp_rv = floatValue;
                    else continue;

                    if (spring) tmp_rv.type = RV_TYPE.SPRING;
                    tmp.Add(tmp_rv);
                }
                return tmp;
            }
            List<RV_I> GetRows(List<RV> tmp, int value)
            {
                int use = 0;
                foreach (var it in tmp)
                {
                    if (it.I) use += it;
                }
                int real = value - use;
                var temp = new List<RV_I>(tmp.Count);
                foreach (var it in tmp)
                {
                    RV_I tmp_rv;
                    if (it.F) tmp_rv = (int)Math.Round(real * (float)it);
                    else if (it.I) tmp_rv = (int)it;
                    else continue;
                    tmp_rv.type = it.type;
                    temp.Add(tmp_rv);
                }
                return temp;
            }
            Dictionary<int, RV_I> GetRows(Dictionary<int, RV> tmp, int value, out int real)
            {
                int use = 0;
                foreach (var it in tmp)
                {
                    if (it.Value.I) use += it.Value;
                }
                real = value - use;
                var temp = new Dictionary<int, RV_I>(tmp.Count);
                foreach (var it in tmp)
                {
                    RV_I tmp_rv;
                    if (it.Value.F) tmp_rv = (int)Math.Round(real * (float)it.Value);
                    else if (it.Value.I) tmp_rv = (int)it.Value;
                    else continue;
                    tmp_rv.type = it.Value.type;
                    temp.Add(it.Key, tmp_rv);
                }
                return temp;
            }

            void HandLayout(List<Control> controls, Rectangle[] rects, Rectangle rect)
            {
                if (rects.Length == 0 || controls.Count == 0)
                    return;
                int gap = (int)Math.Round(Gap * Config.Dpi), gap2 = gap * 2;
                int max_len = controls.Count;
                if (rects.Length < controls.Count)
                {
                    max_len = rects.Length;
                    for (int i = max_len - 1; i < controls.Count; i++) controls[i].Location = new Point(rect.Width, rect.Y);
                }
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

        protected override void OnDraw(DrawEventArgs e)
        {
            PaintBack(e.Canvas);
            base.OnDraw(e);
        }
    }
}