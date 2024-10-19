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

        protected override bool BanInput => _list;

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
                CaretInfo.ReadShow = value;
                if (value) CaretInfo.Show = false;
            }
        }

        bool canDelete = true;
        /// <summary>
        /// 是否可以删除
        /// </summary>
        [Description("是否可以删除"), Category("交互"), DefaultValue(true)]
        public bool CanDelete
        {
            get => canDelete;
            set
            {
                if (canDelete == value) return;
                canDelete = value;
                CalculateRect();
                Invalidate();
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

        protected override bool HasValue => selectedValue.Length > 0;
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
                    SelectedValueChanged?.Invoke(this, new ObjectsEventArgs(selectedValue));
                    return;
                }
                CalculateRect();
                Invalidate();
                SelectedValueChanged?.Invoke(this, new ObjectsEventArgs(selectedValue));
            }
        }

        protected override bool ShowPlaceholder => selectedValue.Length == 0;

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
        bool TerminateExpand = false;
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (HasFocus)
            {
                if (TerminateExpand) { TerminateExpand = false; return; }
                filtertext = Text;
                ExpandDrop = true;
                if (expandDrop) subForm?.TextChange(Text);
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
        protected override void IBackSpaceKey()
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

        protected override void PaintRIcon(Graphics g, Rectangle rect_r)
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

        Rectangle[] rect_lefts = new Rectangle[0];
        Rectangle[] rect_left_txts = new Rectangle[0];
        Rectangle[] rect_left_dels = new Rectangle[0];
        SelectItem?[] style_left = new SelectItem?[0];

        protected override bool HasLeft() => selectedValue.Length > 0;

        protected override int UseLeft(Rectangle rect_read, bool delgap)
        {
            if (selectedValue.Length > 0)
            {
                var style_dir = new Dictionary<object, SelectItem>(selectedValue.Length);
                var enable_dir = new List<object>(selectedValue.Length);
                if (items != null && items.Count > 0)
                {
                    foreach (var it in items)
                    {
                        if (it is SelectItem item)
                        {
                            style_dir.Add(item.Tag, item);
                            if (!item.Enable) enable_dir.Add(item.Tag);
                        }
                    }
                }
                return Helper.GDI(g =>
                {
                    int height = g.MeasureString(Config.NullText, Font).Size().Height, del_icon = (int)(height * 0.4);
                    var _style_left = new List<SelectItem?>(selectedValue.Length);
                    List<Rectangle> _rect_left = new List<Rectangle>(selectedValue.Length), _rect_left_txt = new List<Rectangle>(selectedValue.Length), _rect_left_del = new List<Rectangle>(selectedValue.Length);
                    int y = (rect_read.Height - height) / 2, use = delgap ? 0 : y, gap = (int)(2 * Config.Dpi);
                    for (int i = 0; i < selectedValue.Length; i++)
                    {
                        var it = selectedValue[i];
                        string? showtext;
                        SelectItem? style = null;
                        if (style_dir.TryGetValue(it, out var find)) { style = find; showtext = find.Text; }
                        else showtext = it.ToString();

                        var size = g.MeasureString(showtext, Font).Size();
                        var size2 = g.MeasureString("+" + (selectedValue.Length - i), Font).Size();
                        int use_base = use + size.Width + height + gap;
                        if (use_base + (size2.Width + gap) > rect_read.Width)
                        {
                            //超出
                            _rect_left_txt.Add(new Rectangle(rect_read.X + use, rect_read.Y + y, size2.Width, height));
                            style_left = _style_left.ToArray();
                            rect_left_txts = _rect_left_txt.ToArray();
                            rect_left_dels = _rect_left_del.ToArray();
                            rect_lefts = _rect_left.ToArray();
                            if (_rect_left_txt.Count == 1) return size2.Width + gap;
                            return use + size2.Width + gap;
                        }
                        _style_left.Add(style);
                        if (enable_dir.Contains(it) || !canDelete)
                        {
                            var rect = new Rectangle(rect_read.X + use, rect_read.Y + y, size.Width, height);
                            _rect_left_txt.Add(rect);
                            _rect_left_del.Add(new Rectangle(-10, -10, 0, 0));
                            _rect_left.Add(rect);
                            use += size.Width + gap;
                        }
                        else
                        {
                            var rect = new Rectangle(rect_read.X + use, rect_read.Y + y, size.Width, height);
                            _rect_left_txt.Add(rect);
                            int gapdelxy = (height - del_icon) / 2;
                            _rect_left_del.Add(new Rectangle(rect.Right + gapdelxy / 2, rect.Y + gapdelxy, del_icon, del_icon));
                            rect.Width += height;
                            _rect_left.Add(rect);
                            use += size.Width + height + gap;
                        }
                    }
                    style_left = _style_left.ToArray();
                    rect_left_txts = _rect_left_txt.ToArray();
                    rect_left_dels = _rect_left_del.ToArray();
                    rect_lefts = _rect_left.ToArray();
                    return use - (delgap ? 0 : gap);
                });
            }
            return 0;
        }

        protected override void PaintOtherBor(Graphics g, RectangleF rect_read, float radius, Color back, Color borderColor, Color borderActive)
        {
            if (selectedValue.Length > 0 && style_left.Length == rect_lefts.Length)
            {
                using (var brush = new SolidBrush(Style.Db.TagDefaultColor))
                {
                    if (rect_lefts.Length > 0)
                    {
                        for (int i = 0; i < rect_lefts.Length; i++)
                        {
                            var it = selectedValue[i];
                            var style = style_left[i];
                            using (var path = rect_lefts[i].RoundPath(radius))
                            {
                                if (style == null)
                                {
                                    using (var brushbg = new SolidBrush(Style.Db.TagDefaultBg))
                                    {
                                        g.FillPath(brushbg, path);
                                    }
                                    var rect_del = rect_left_dels[i];
                                    if (rect_del.Width > 0 && rect_del.Height > 0) g.PaintIconClose(rect_del, Style.Db.TagDefaultColor);
                                    g.DrawStr(it.ToString(), Font, brush, rect_left_txts[i], sf_center);
                                }
                                else
                                {
                                    using (var brushbg = style.TagBackExtend.BrushEx(rect_lefts[i], style.TagBack ?? Style.Db.TagDefaultBg))
                                    {
                                        g.FillPath(brushbg, path);
                                    }
                                    if (style.TagFore.HasValue)
                                    {
                                        var rect_del = rect_left_dels[i];
                                        if (rect_del.Width > 0 && rect_del.Height > 0) g.PaintIconClose(rect_del, style.TagFore.Value);
                                        using (var brushf = new SolidBrush(style.TagFore.Value))
                                        {
                                            g.DrawStr(style.Text, Font, brushf, rect_left_txts[i], sf_center);
                                        }
                                    }
                                    else
                                    {
                                        var rect_del = rect_left_dels[i];
                                        if (rect_del.Width > 0 && rect_del.Height > 0) g.PaintIconClose(rect_del, Style.Db.TagDefaultColor);
                                        g.DrawStr(style.Text, Font, brush, rect_left_txts[i], sf_center);
                                    }
                                }
                            }
                        }
                    }
                    if (rect_lefts.Length != selectedValue.Length)
                    {
                        g.DrawStr("+" + (selectedValue.Length - rect_lefts.Length), Font, brush, rect_left_txts[rect_left_txts.Length - 1], sf_center);
                    }
                }
            }
        }

        int select_del = -1;
        protected override bool IMouseDown(Point e)
        {
            select_del = -1;
            if (selectedValue.Length > 0 && rect_left_dels.Length > 0)
            {
                int len = selectedValue.Length > rect_left_dels.Length ? rect_left_dels.Length : selectedValue.Length;
                for (int i = 0; i < len; i++)
                {
                    if (rect_left_dels[i].Contains(e)) { select_del = i; return true; }
                }
            }
            return false;
        }
        protected override bool IMouseMove(Point e)
        {
            if (selectedValue.Length > 0 && rect_left_dels.Length > 0)
            {
                int len = selectedValue.Length > rect_left_dels.Length ? rect_left_dels.Length : selectedValue.Length;
                for (int i = 0; i < len; i++)
                {
                    if (rect_left_dels[i].Contains(e)) return true;
                }
            }
            return false;
        }
        protected override bool IMouseUp(Point e)
        {
            if (select_del > -1)
            {
                if (rect_left_dels[select_del].Contains(e))
                {
                    var tmp = new List<object>(selectedValue.Length);
                    tmp.AddRange(selectedValue);
                    tmp.RemoveAt(select_del);
                    SelectedValue = tmp.ToArray();

                    if (subForm == null) return true;
                    subForm.selectedValue = new List<object>(selectedValue.Length);
                    subForm.selectedValue.AddRange(selectedValue);
                    subForm.Print();
                }
                select_del = -1;
                return true;
            }
            select_del = -1;
            return false;
        }

        #endregion

        #region 动画

        LayeredFormSelectMultiple? subForm = null;
        public ILayeredForm? SubForm() => subForm;

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

        bool expandDrop = false;
        /// <summary>
        /// 展开下拉菜单
        /// </summary>
        bool ExpandDrop
        {
            get => expandDrop;
            set
            {
                if (expandDrop == value) return;
                expandDrop = value;
                if (value)
                {
                    if (ReadOnly || items == null || items.Count == 0)
                    {
                        subForm?.IClose();
                        expandDrop = false;
                    }
                    else
                    {
                        if (subForm == null)
                        {
                            var objs = new List<object>(items.Count);
                            foreach (var it in items) objs.Add(it);
                            ShowLayeredForm(objs);
                        }
                    }
                }
                else
                {
                    subForm?.IClose();
                    filtertext = "";
                }
            }
        }

        void ShowLayeredForm(IList<object> list)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => { ShowLayeredForm(list); }));
                return;
            }
            Expand = true;
            subForm = new LayeredFormSelectMultiple(this, ReadRectangle, list, filtertext);
            subForm.Disposed += (a, b) =>
            {
                subForm = null;
                Expand = false;
                ExpandDrop = false;
            };
            subForm.Show(this);
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Down:
                    ExpandDrop = true;
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            ExpandDrop = false;
        }

        #endregion

        #region 鼠标

        protected override void OnClearValue()
        {
            if (selectedValue.Length > 0)
            {
                TerminateExpand = true;
                ClearSelect();
            }
        }

        protected override void OnClickContent()
        {
            ExpandDrop = !expandDrop;
        }

        #endregion
    }
}