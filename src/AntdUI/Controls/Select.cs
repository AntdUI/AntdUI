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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Select 选择器
    /// </summary>
    /// <remarks>下拉选择器。</remarks>
    [Description("Select 选择器")]
    [ToolboxItem(true)]
    [DefaultEvent("SelectedIndexChanged")]
    public class Select : Input, SubLayeredForm
    {
        #region 属性

        bool _list = false;
        /// <summary>
        /// 是否列表样式
        /// </summary>
        [Description("是否列表样式"), Category("外观"), DefaultValue(false)]
        public bool List
        {
            get => _list;
            set
            {
                if (_list == value) return;
                _list = value;
                ReadShowCaret = value;
                if (value) ShowCaret = false;
            }
        }

        /// <summary>
        /// 菜单弹出位置
        /// </summary>
        [Description("菜单弹出位置"), Category("行为"), DefaultValue(TAlignFrom.BL)]
        public TAlignFrom Placement { get; set; } = TAlignFrom.BL;

        /// <summary>
        /// 是否列表自动宽度
        /// </summary>
        [Description("是否列表自动宽度"), Category("行为"), DefaultValue(false)]
        public bool ListAutoWidth { get; set; } = false;

        /// <summary>
        /// 列表最多显示条数
        /// </summary>
        [Description("列表最多显示条数"), Category("行为"), DefaultValue(4)]
        public int MaxCount { get; set; } = 4;

        /// <summary>
        /// 下拉箭头是否显示
        /// </summary>
        [Description("下拉箭头是否显示"), Category("外观"), DefaultValue(false)]
        public bool DropDownArrow { get; set; } = false;

        /// <summary>
        /// 点击到最里层（无节点才能点击）
        /// </summary>
        [Description("点击到最里层（无节点才能点击）"), Category("行为"), DefaultValue(false)]
        public bool ClickEnd { get; set; } = false;

        #region 数据

        BaseCollection? items;
        /// <summary>
        /// 数据
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor", typeof(UITypeEditor))]
        [Description("集合"), Category("数据")]
        public BaseCollection Items
        {
            get
            {
                items ??= new BaseCollection();
                return items;
            }
            set => items = value;
        }

        #region 操作值

        /// <summary>
        /// 选中序号
        /// </summary>
        [Description("选中序号"), Category("数据"), DefaultValue(-1)]
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex == value) return;
                if (items == null || items.Count == 0 || value == -1) ChangeValueNULL();
                else
                {
                    var obj = items[value];
                    if (obj == null) return;
                    ChangeValue(value, obj);
                }
                if (_list) Invalidate();
            }
        }

        /// <summary>
        /// 选中值
        /// </summary>
        [Browsable(false)]
        [Description("选中值"), Category("数据"), DefaultValue(null)]
        public object? SelectedValue
        {
            get => selectedValue;
            set
            {
                if (selectedValue == value) return;
                if (value == null || items == null || items.Count == 0) ChangeValueNULL();
                else ChangeValue(items.IndexOf(value), value);
                if (_list) Invalidate();
            }
        }

        int selectedIndexX = -1;
        int selectedIndex = -1;
        object? selectedValue = null;
        void ChangeValueNULL()
        {
            Text = null;
            selectedValue = null;
            selectedIndex = -1;
            SelectedValueChanged?.Invoke(this, selectedValue);
            SelectedIndexChanged?.Invoke(this, selectedIndex);
        }

        void ChangeValue(int value, object obj)
        {
            selectedIndex = value;
            if (obj is SelectItem it)
            {
                selectedValue = it.Tag;
                Text = it.Text;
            }
            else
            {
                selectedValue = obj;
                Text = obj.ToString();
            }
            SelectedValueChanged?.Invoke(this, selectedValue);
            SelectedIndexChanged?.Invoke(this, selectedIndex);
        }
        void ChangeValue(int x, int y, object obj)
        {
            selectedIndexX = x;
            selectedIndex = y;
            if (obj is SelectItem it)
            {
                selectedValue = it.Tag;
                Text = it.Text;
            }
            else
            {
                selectedValue = obj;
                Text = obj.ToString();
            }
            SelectedValueChanged?.Invoke(this, selectedValue);
            SelectedIndexsChanged?.Invoke(this, selectedIndexX, selectedIndex);
        }

        internal void DropDownChange(int i)
        {
            selectedIndexX = 0;
            ChangeValue(i, items[i]);
            TextFocus = false;
            select_x = 0;
            subForm = null;
        }

        internal bool DropDownChange()
        {
            return SelectedIndexsChanged == null;
        }
        internal void DropDownChange(int x, int y, object value)
        {
            ChangeValue(x, y, value);
            TextFocus = false;
            select_x = 0;
            subForm = null;
        }

        #endregion

        /// <summary>
        /// SelectedIndex 属性值更改时发生
        /// </summary>
        [Description("SelectedIndex 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? SelectedIndexChanged = null;

        /// <summary>
        /// 多层树结构更改时发生
        /// </summary>
        [Description("多层树结构更改时发生"), Category("行为")]
        public event IntXYEventHandler? SelectedIndexsChanged = null;
        public delegate void IntXYEventHandler(object sender, int x, int y);

        /// <summary>
        /// SelectedValue 属性值更改时发生
        /// </summary>
        [Description("SelectedValue 属性值更改时发生"), Category("行为")]
        public event ObjectNEventHandler? SelectedValueChanged = null;

        #endregion

        #endregion

        #region 渲染

        #region 自带图标

        bool showicon = true;
        /// <summary>
        /// 是否显示图标
        /// </summary>
        [Description("是否显示图标"), Category("外观"), DefaultValue(true)]
        public bool ShowIcon
        {
            get => showicon;
            set
            {
                if (showicon == value) return;
                showicon = value;
                CalculateRect();
                Invalidate();
            }
        }

        public override bool HasSuffix
        {
            get => showicon;
        }

        internal override void PaintR(Graphics g, Rectangle rect_r)
        {
            if (showicon)
            {
                using (var pen = new Pen(Style.Db.TextQuaternary, 2F))
                {
                    pen.StartCap = pen.EndCap = LineCap.Round;
                    g.DrawLines(pen, rect_r.TriangleLines(ArrowProg));
                }
            }
        }

        #endregion

        #endregion

        #region 动画

        ILayeredForm? subForm = null;
        public ILayeredForm? SubForm() { return subForm; }

        ITask? ThreadExpand = null;
        float ArrowProg = -1F;
        bool expand = false;
        bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                if (Config.Animation)
                {
                    ThreadExpand?.Dispose();
                    var t = Animation.TotalFrames(10, 100);
                    if (value)
                    {
                        ThreadExpand = new ITask((i) =>
                        {
                            ArrowProg = Animation.Animate(i, t, 2F, AnimationType.Ball) - 1F;
                            Invalidate();
                            return true;
                        }, 10, t, () =>
                        {
                            ArrowProg = 1;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadExpand = new ITask((i) =>
                        {
                            ArrowProg = -(Animation.Animate(i, t, 2F, AnimationType.Ball) - 1F);
                            Invalidate();
                            return true;
                        }, 10, t, () =>
                        {
                            ArrowProg = -1;
                            Invalidate();
                        });
                    }
                }
                else ArrowProg = value ? 1F : -1F;
            }
        }

        internal int select_x = 0;
        bool textFocus = false;
        bool TextFocus
        {
            get => textFocus;
            set
            {
                if (textFocus != value)
                {
                    textFocus = value;
                    subForm?.IClose();
                }
                if (value)
                {
                    if (items != null && items.Count > 0)
                    {
                        if (subForm == null)
                        {
                            var objs = new List<object>(items.Count);
                            foreach (var it in items)
                            {
                                objs.Add(it);
                            }
                            Expand = true;
                            subForm = new LayeredFormSelectDown(this, ReadRectangle, objs);
                            subForm.Disposed += (a, b) =>
                            {
                                select_x = 0;
                                subForm = null;
                                Expand = false;
                                TextFocus = false;
                            };
                            subForm.Show(this);
                        }
                    }
                    else { textFocus = false; return; }
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (ReadShowCaret)
            {
                base.OnGotFocus(e); return;
            }
            TextFocus = true;
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            TextFocus = false;
            base.OnLostFocus(e);
        }

        #endregion

        #region 鼠标

        internal override void OnClearValue()
        {
            if (selectedIndex > -1)
            {
                ChangeValueNULL();
                Invalidate();
            }
            else ClickDown();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            ClickDown();
            base.OnMouseClick(e);
        }

        void ClickDown()
        {
            if (_list)
            {
                Focus();
                TextFocus = !textFocus;
            }
            else
            {
                if (Focused)
                {
                    if (textFocus) return;
                    TextFocus = !textFocus;
                }
                else Focus();
            }
        }

        #endregion
    }

    public class DividerSelectItem : ISelectItem
    {
    }
    public class SelectItem : ISelectItem
    {
        public SelectItem(int online, Image? ico, string text, object tag) : this(text, tag)
        {
            Online = online;
            Icon = ico;
        }
        public SelectItem(int online, Image? ico, object tag) : this(tag)
        {
            Online = online;
            Icon = ico;
        }
        public SelectItem(Image? ico, string text, object tag) : this(text, tag)
        {
            Icon = ico;
        }
        public SelectItem(Image? ico, string text) : this(text)
        {
            Icon = ico;
        }
        public SelectItem(Image? ico, object tag) : this(tag)
        {
            Icon = ico;
        }
        public SelectItem(int online, string text, object tag) : this(text, tag)
        {
            Online = online;
        }
        public SelectItem(int online, object tag) : this(tag)
        {
            Online = online;
        }

        public SelectItem(object tag)
        {
            Text = tag.ToString() ?? string.Empty;
            Tag = tag;
        }
        public SelectItem(string text, object tag)
        {
            Text = text;
            Tag = tag;
        }
        /// <summary>
        /// 在线状态
        /// </summary>
        public int? Online { get; set; }
        public Image? Icon { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// 子选项
        /// </summary>
        public List<object>? Sub { get; set; }

        public object Tag { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
    public class ISelectItem { }

    internal class ObjectItem
    {
        public ObjectItem(object _val, int _i, RectangleF rect, RectangleF rect_text)
        {
            Rect = rect;
            RectText = rect_text;
            Show = true;
            Val = _val;
            Text = _val.ToString() ?? string.Empty;
            PY = Pinyin.Pinyin.GetPinyin(Text).ToLower();
            PYS = Pinyin.Pinyin.GetInitials(Text).ToLower();
            ID = _i;
        }
        public ObjectItem(SelectItem _val, int _i, RectangleF rect, int gap_y, float gap, RectangleF rect_text)
        {
            Sub = _val.Sub;
            if (Sub != null && Sub.Count > 0) has_sub = true;
            Rect = rect;
            if (_val.Online > -1 || _val.Icon != null)
            {
                if (_val.Online > -1 && _val.Icon != null)
                {
                    float h2 = rect_text.Height * 0.7F;
                    RectOnline = new RectangleF(rect_text.X + (h2 - gap_y) / 2F, rect_text.Y + gap, gap_y, gap_y);
                    RectIcon = new RectangleF(rect_text.X + h2, rect_text.Y, rect_text.Height, rect_text.Height);
                    RectText = new RectangleF(rect_text.X + h2 + gap_y + rect_text.Height, rect_text.Y, rect_text.Width - rect_text.Height - gap_y - h2, rect_text.Height);
                }
                else if (_val.Online > -1)
                {
                    RectOnline = new RectangleF(rect_text.X + gap, rect_text.Y + gap, gap_y, gap_y);
                    RectText = new RectangleF(rect_text.X + gap_y + rect_text.Height, rect_text.Y, rect_text.Width - rect_text.Height - gap_y, rect_text.Height);
                }
                else
                {
                    RectIcon = new RectangleF(rect_text.X, rect_text.Y, rect_text.Height, rect_text.Height);
                    RectText = new RectangleF(rect_text.X + gap_y + rect_text.Height, rect_text.Y, rect_text.Width - rect_text.Height - gap_y, rect_text.Height);
                }
            }
            else RectText = rect_text;
            arr_rect = new RectangleF(Rect.Right - Rect.Height - gap_y, Rect.Y, Rect.Height, Rect.Height);

            Show = true;
            Val = _val;
            Online = _val.Online;
            Icon = _val.Icon;
            Text = _val.Text;
            PY = Pinyin.Pinyin.GetPinyin(_val.Text).ToLower();
            PYS = Pinyin.Pinyin.GetInitials(_val.Text).ToLower();
            ID = _i;
        }

        public ObjectItem(RectangleF rect)
        {
            ID = -1;
            Rect = rect;
            //RectIcon = RectOnline = RectText = Rectangle.Empty;
            Show = true;
        }
        public object Val { get; set; }

        /// <summary>
        /// 子选项
        /// </summary>
        public List<object>? Sub { get; set; }
        internal bool has_sub { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public int? Online { get; set; }
        public Image? Icon { get; set; }
        public RectangleF RectIcon { get; set; }
        public RectangleF RectOnline { get; set; }

        string PY { get; set; }
        string PYS { get; set; }
        public string Text { get; set; }
        public bool Contains(string val)
        {
            return Text.Contains(val) || PY.Contains(val) || PYS.Contains(val);
        }

        public int ID { get; set; }

        public bool Hover { get; set; }
        public bool Show { get; set; }

        internal RectangleF arr_rect { get; set; }

        public RectangleF Rect { get; set; }
        internal bool SetHover(bool val)
        {
            bool change = false;
            if (val)
            {
                if (!Hover) change = true;
                Hover = true;
            }
            else
            {
                if (Hover) change = true;
                Hover = false;
            }
            return change;
        }
        internal bool Contains(Point point, float x, float y, out bool change)
        {
            if (ID > -1 && Rect.Contains(new PointF(point.X + x, point.Y + y)))
            {
                change = SetHover(true);
                return true;
            }
            else
            {
                change = SetHover(false);
                return false;
            }
        }

        public RectangleF RectText { get; set; }
    }
}