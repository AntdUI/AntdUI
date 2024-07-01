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
    /// Select 多选器
    /// </summary>
    /// <remarks>下拉多选器。</remarks>
    [Description("Select 多选器")]
    [ToolboxItem(true)]
    [DefaultEvent("SelectedValueChanged")]
    public class SelectMultiple : Input, SubLayeredForm
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
        /// 最大选中数量
        /// </summary>
        [Description("最大选中数量"), Category("行为"), DefaultValue(0)]
        public int MaxChoiceCount { get; set; }

        /// <summary>
        /// 下拉箭头是否显示
        /// </summary>
        [Description("下拉箭头是否显示"), Category("外观"), DefaultValue(false)]
        public bool DropDownArrow { get; set; } = false;

        /// <summary>
        /// 焦点时展开下拉
        /// </summary>
        [Description("焦点时展开下拉"), Category("行为"), DefaultValue(true)]
        public bool FocusExpandDropdown { get; set; } = true;

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

        internal override bool HasValue { get => selectedValue.Length > 0; }
        /// <summary>
        /// 选中值
        /// </summary>
        [Browsable(false)]
        public object[] SelectedValue
        {
            get => selectedValue;
            set
            {
                if (selectedValue == value) return;
                selectedValue = value;
                if (value == null || items == null || items.Count == 0)
                {
                    ClearSelect();
                    SelectedValueChanged?.Invoke(this, selectedValue);
                    return;
                }
                CalculateRect();
                Invalidate();
                SelectedValueChanged?.Invoke(this, selectedValue);
            }
        }

        /// <summary>
        /// 全选项目
        /// </summary>
        public void SelectAllItems()
        {
            if (items == null) return;
            var selecteds = new List<object>(items.Count);
            foreach (object it in items)
            {
                if (it is DividerSelectItem) { }
                else selecteds.Add(it);
            }
            selectedValue = selecteds.ToArray();
            CalculateRect();
            SetCaretPostion();

            if (subForm == null) return;
            subForm.selectedValue = selecteds;
            subForm.Print();
        }

        /// <summary>
        /// SelectedValue 属性值更改时发生
        /// </summary>
        [Description("SelectedValue 属性值更改时发生"), Category("行为")]
        public event ObjectsEventHandler? SelectedValueChanged = null;

        string filtertext = "";
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (HasFocus)
            {
                filtertext = Text;
                TextFocus = true;
                if (textFocus) subForm?.TextChange(Text);
            }
        }

        object[] selectedValue = new object[0];
        /// <summary>
        /// 清空选中
        /// </summary>
        public void ClearSelect()
        {
            Text = "";
            selectedValue = new object[0];
            CalculateRect();
            SetCaretPostion();
            Invalidate();

            if (subForm == null) return;
            subForm.selectedValue = new List<object>(0);
            subForm.Print();
        }
        internal override void IBackSpaceKey()
        {
            if (selectedValue.Length > 0)
            {
                var tmp = new List<object>(selectedValue.Length);
                tmp.AddRange(selectedValue);
                tmp.RemoveAt(tmp.Count - 1);
                SelectedValue = tmp.ToArray();

                if (subForm == null) return;
                subForm.selectedValue = new List<object>(selectedValue.Length);
                subForm.selectedValue.AddRange(selectedValue);
                subForm.Print();
            }
        }

        #endregion

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

        internal override void PaintRIcon(Graphics g, Rectangle rect_r)
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

        RectangleF[] rect_lefts = new RectangleF[0];
        RectangleF[] rect_left_txts = new RectangleF[0];
        RectangleF[] rect_left_dels = new RectangleF[0];

        internal override bool HasLeft() => selectedValue.Length > 0;

        internal override int UseLeft(Rectangle rect_read, bool delgap)
        {
            if (selectedValue.Length > 0)
            {
                return Helper.GDI(g =>
                {
                    int height = g.MeasureString(Config.NullText, Font).Size().Height, del_icon = (int)(height * 0.4);
                    List<RectangleF> _rect_left = new List<RectangleF>(selectedValue.Length), _rect_left_txt = new List<RectangleF>(selectedValue.Length), _rect_left_del = new List<RectangleF>(selectedValue.Length);
                    int y = (rect_read.Height - height) / 2, use = delgap ? 0 : y, gap = (int)(2 * Config.Dpi);
                    for (int i = 0; i < selectedValue.Length; i++)
                    {
                        var it = selectedValue[i];
                        var size = g.MeasureString(it.ToString(), Font).Size();
                        var size2 = g.MeasureString("+" + (selectedValue.Length - i), Font).Size();
                        int use_base = use + size.Width + height + gap;
                        if (use_base + (size2.Width + gap) > rect_read.Width)
                        {
                            //超出
                            _rect_left_txt.Add(new RectangleF(rect_read.X + use, rect_read.Y + y, size2.Width, height));
                            rect_left_txts = _rect_left_txt.ToArray();
                            rect_left_dels = _rect_left_del.ToArray();
                            rect_lefts = _rect_left.ToArray();
                            return use + size2.Width + gap;
                        }
                        var rect = new RectangleF(rect_read.X + use, rect_read.Y + y, size.Width, height);
                        _rect_left_txt.Add(rect);
                        float gapdelxy = (height - del_icon) / 2;
                        _rect_left_del.Add(new RectangleF(rect.Right + gapdelxy / 2, rect.Y + gapdelxy, del_icon, del_icon));
                        rect.Width += height;
                        _rect_left.Add(rect);
                        use += size.Width + height + gap;
                    }
                    rect_left_txts = _rect_left_txt.ToArray();
                    rect_left_dels = _rect_left_del.ToArray();
                    rect_lefts = _rect_left.ToArray();
                    return use - (delgap ? 0 : gap);
                });
            }
            return 0;
        }

        internal override void PaintOtherBor(Graphics g, RectangleF rect_read, float radius, Color back, Color borderColor, Color borderActive)
        {
            if (selectedValue.Length > 0 && rect_lefts.Length > 0)
            {
                using (var brush = new SolidBrush(Style.Db.TagDefaultColor))
                {
                    for (int i = 0; i < rect_lefts.Length; i++)
                    {
                        var it = selectedValue[i];
                        using (var path = rect_lefts[i].RoundPath(radius))
                        {
                            using (var brushbg = new SolidBrush(Style.Db.TagDefaultBg))
                            {
                                g.FillPath(brushbg, path);
                            }
                        }
                        g.PaintIconClose(rect_left_dels[i], Style.Db.TagDefaultColor);
                        g.DrawString(it.ToString(), Font, brush, rect_left_txts[i], sf_center);
                    }
                    if (rect_lefts.Length != selectedValue.Length)
                    {
                        g.DrawString("+" + (selectedValue.Length - rect_lefts.Length), Font, brush, rect_left_txts[rect_left_txts.Length - 1], sf_center);
                    }
                }
            }
        }
        internal override bool IMouseDown(Point e)
        {
            if (selectedValue.Length > 0 && rect_left_dels.Length == selectedValue.Length)
            {
                for (int i = 0; i < selectedValue.Length; i++)
                {
                    if (rect_left_dels[i].Contains(e))
                    {
                        var tmp = new List<object>(selectedValue.Length);
                        tmp.AddRange(selectedValue);
                        tmp.RemoveAt(i);
                        SelectedValue = tmp.ToArray();

                        if (subForm == null) return true;
                        subForm.selectedValue = new List<object>(selectedValue.Length);
                        subForm.selectedValue.AddRange(selectedValue);
                        subForm.Print();

                        return true;
                    }
                }
            }
            return false;
        }
        internal override bool IMouseMove(Point e)
        {
            if (selectedValue.Length > 0 && rect_left_dels.Length == selectedValue.Length)
            {
                for (int i = 0; i < selectedValue.Length; i++)
                {
                    if (rect_left_dels[i].Contains(e)) { return true; }
                }
            }
            return false;
        }

        #endregion

        #region 动画

        LayeredFormSelectMultiple? subForm = null;
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

        #endregion

        #region 焦点

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
                    if (FocusExpandDropdown && !ReadOnly && items != null && items.Count > 0)
                    {
                        if (subForm == null)
                        {
                            var objs = new List<object>(items.Count);
                            foreach (var it in items)
                            {
                                objs.Add(it);
                            }
                            Expand = true;
                            subForm = new LayeredFormSelectMultiple(this, ReadRectangle, objs, filtertext);
                            subForm.Disposed += (a, b) =>
                            {
                                subForm = null;
                                Expand = false;
                                TextFocus = false;
                            };
                            subForm.Show(this);
                        }
                    }
                    else { textFocus = false; return; }
                }
                else filtertext = "";
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
            if (selectedValue.Length > 0) ClearSelect();
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
                if (HasFocus)
                {
                    if (textFocus) return;
                    TextFocus = !textFocus;
                }
                else Focus();
            }
        }

        #endregion
    }
}