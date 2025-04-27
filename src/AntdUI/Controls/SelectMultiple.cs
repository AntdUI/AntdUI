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
using System.Drawing.Drawing2D;
using System.Linq;
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

        /// <summary>
        /// 复选框模式
        /// </summary>
        [Description("复选框模式"), Category("行为"), DefaultValue(false)]
        public bool CheckMode { get; set; }

        /// <summary>
        /// 自动高度
        /// </summary>
        [Description("自动高度"), Category("行为"), DefaultValue(false)]
        public bool AutoHeight { get; set; }

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
        public bool ListAutoWidth { get; set; }

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
        public bool DropDownArrow { get; set; }

        /// <summary>
        /// 下拉边距
        /// </summary>
        [Description("下拉边距"), Category("外观"), DefaultValue(typeof(Size), "12, 5")]
        public Size DropDownPadding { get; set; } = new Size(12, 5);

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(2)]
        public int Gap { get; set; } = 2;

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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object[] SelectedValue
        {
            get => selectedValue;
            set
            {
                if (selectedValue == value) return;
                selectedValue = value;
                if (value.Length == 0 || items == null || items.Count == 0)
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
            foreach (var it in items)
            {
                if (it is DividerSelectItem) { }
                else if (it is SelectItem item) selecteds.Add(item.Tag);
                else selecteds.Add(it);
            }
            if (selectedValue.SequenceEqual(selecteds)) return;
            selectedValue = selecteds.ToArray();
            CalculateRect();
            SetCaretPostion();
            subForm?.SetValues(selecteds);
            SelectedValueChanged?.Invoke(this, new ObjectsEventArgs(selectedValue));
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
            if (selectedValue.Length > 0)
            {
                TerminateExpand = true;
                SelectedValue = new object[0];
            }
            base.OnClearValue();
            CalculateRect();
            Invalidate();
            subForm?.ClearValues();
        }

        protected override void IBackSpaceKey()
        {
            if (selectedValue.Length > 0)
            {
                var tmp = new List<object>(selectedValue.Length);
                tmp.AddRange(selectedValue);
                tmp.RemoveAt(tmp.Count - 1);
                SelectedValue = tmp.ToArray();
                subForm?.SetValues(selectedValue);
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

        public override bool HasSuffix => showicon;

        protected override void PaintRIcon(Canvas g, Rectangle rect_r)
        {
            if (showicon)
            {
                using (var pen = new Pen(Colour.TextQuaternary.Get("Select", ColorScheme), 2F))
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

        protected override int[] UseLeft(Rectangle rect_read, int font_height, bool delgap)
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
                    var _style_left = new List<SelectItem?>(selectedValue.Length);
                    List<Rectangle> _rect_left = new List<Rectangle>(selectedValue.Length), _rect_left_txt = new List<Rectangle>(selectedValue.Length), _rect_left_del = new List<Rectangle>(selectedValue.Length);
                    int gap = (int)(Gap * Config.Dpi), gap2 = gap * 2, gap4 = gap2 * 2, height = font_height + gap2, del_icon = (int)(height * .4);
                    if (AutoHeight || rect_read.Height > height * 2)
                    {
                        //多行
                        int y = gap, usex = delgap ? 0 : y, usey = 0;
                        for (int i = 0; i < selectedValue.Length; i++)
                        {
                            var it = selectedValue[i];
                            string? showtext;
                            SelectItem? style = null;
                            if (style_dir.TryGetValue(it, out var find))
                            {
                                style = find;
                                showtext = find.Text;
                            }
                            else showtext = it.ToString();

                            int sizeWidth = g.MeasureString(showtext, Font).Width + gap4,
                            size2Width = g.MeasureString("+" + (selectedValue.Length - i), Font).Width + gap4;
                            int use_base_x = usex + sizeWidth + height + gap;
                            if (use_base_x + (size2Width + gap) > rect_read.Width)
                            {
                                if (AutoHeight)
                                {
                                    usey += height + gap;
                                    usex = delgap ? 0 : y;
                                }
                                else if ((usey + height + gap) + (height + gap) > rect_read.Height)//超出
                                {
                                    _rect_left_txt.Add(new Rectangle(rect_read.X + usex, rect_read.Y + y + usey, size2Width, height));
                                    style_left = _style_left.ToArray();
                                    rect_left_txts = _rect_left_txt.ToArray();
                                    rect_left_dels = _rect_left_del.ToArray();
                                    rect_lefts = _rect_left.ToArray();
                                    if (_rect_left_txt.Count == 1) return new int[] { size2Width + gap, usey };
                                    return new int[] { usex + size2Width + gap, usey };
                                }
                                else
                                {
                                    usey += height + gap;
                                    usex = delgap ? 0 : y;
                                }
                            }
                            _style_left.Add(style);
                            if (enable_dir.Contains(it) || !canDelete)
                            {
                                var rect = new Rectangle(rect_read.X + usex, rect_read.Y + y + usey, sizeWidth, height);
                                _rect_left_txt.Add(rect);
                                _rect_left_del.Add(new Rectangle(-10, -10, 0, 0));
                                _rect_left.Add(rect);
                                usex += sizeWidth + gap;
                            }
                            else
                            {
                                var rect = new Rectangle(rect_read.X + usex, rect_read.Y + y + usey, sizeWidth, height);
                                _rect_left_txt.Add(rect);
                                int gapdelxy = (height - del_icon) / 2;
                                _rect_left_del.Add(new Rectangle(rect.Right + gapdelxy / 2, rect.Y + gapdelxy, del_icon, del_icon));
                                rect.Width += height;
                                _rect_left.Add(rect);
                                usex += sizeWidth + height + gap;
                            }
                        }
                        style_left = _style_left.ToArray();
                        rect_left_txts = _rect_left_txt.ToArray();
                        rect_left_dels = _rect_left_del.ToArray();
                        rect_lefts = _rect_left.ToArray();
                        return new int[] { usex - (delgap ? 0 : gap), usey };
                    }
                    else
                    {
                        int y = (rect_read.Height - height) / 2, use = delgap ? 0 : y;
                        for (int i = 0; i < selectedValue.Length; i++)
                        {
                            var it = selectedValue[i];
                            string? showtext;
                            SelectItem? style = null;
                            if (style_dir.TryGetValue(it, out var find))
                            {
                                style = find;
                                showtext = find.Text;
                            }
                            else showtext = it.ToString();

                            int sizeWidth = g.MeasureString(showtext, Font).Width + gap4,
                            size2Width = g.MeasureString("+" + (selectedValue.Length - i), Font).Width + gap4;
                            int use_base = use + sizeWidth + height + gap;
                            if (use_base + (size2Width + gap) > rect_read.Width)
                            {
                                //超出
                                _rect_left_txt.Add(new Rectangle(rect_read.X + use, rect_read.Y + y, size2Width, height));
                                style_left = _style_left.ToArray();
                                rect_left_txts = _rect_left_txt.ToArray();
                                rect_left_dels = _rect_left_del.ToArray();
                                rect_lefts = _rect_left.ToArray();
                                if (_rect_left_txt.Count == 1) return new int[] { size2Width + gap, 0 };
                                return new int[] { use + size2Width + gap, 0 };
                            }
                            _style_left.Add(style);
                            if (enable_dir.Contains(it) || !canDelete)
                            {
                                var rect = new Rectangle(rect_read.X + use, rect_read.Y + y, sizeWidth, height);
                                _rect_left_txt.Add(rect);
                                _rect_left_del.Add(new Rectangle(-10, -10, 0, 0));
                                _rect_left.Add(rect);
                                use += sizeWidth + gap;
                            }
                            else
                            {
                                var rect = new Rectangle(rect_read.X + use, rect_read.Y + y, sizeWidth, height);
                                _rect_left_txt.Add(rect);
                                int gapdelxy = (height - del_icon) / 2;
                                _rect_left_del.Add(new Rectangle(rect.Right + gapdelxy / 2, rect.Y + gapdelxy, del_icon, del_icon));
                                rect.Width += height;
                                _rect_left.Add(rect);
                                use += sizeWidth + height + gap;
                            }
                        }
                        style_left = _style_left.ToArray();
                        rect_left_txts = _rect_left_txt.ToArray();
                        rect_left_dels = _rect_left_del.ToArray();
                        rect_lefts = _rect_left.ToArray();
                        return new int[] { use - (delgap ? 0 : gap), 0 };
                    }
                });
            }
            return new int[] { 0, 0 };
        }

        protected override void UseLeftAutoHeight(int height, int y)
        {
            if (AutoHeight)
            {
                int pr = (int)Math.Round((WaveSize + BorderWidth / 2F) * Config.Dpi) * 2, gap = (int)(Gap * Config.Dpi) * 4;
                Height = (height + Padding.Top + Padding.Vertical + pr) + y + gap;
            }
        }

        protected override void PaintOtherBor(Canvas g, RectangleF rect_read, float radius, Color back, Color borderColor, Color borderActive)
        {
            if (selectedValue.Length > 0 && style_left.Length == rect_lefts.Length)
            {
                using (var brush = new SolidBrush(Colour.TagDefaultColor.Get("Select", ColorScheme)))
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
                                    g.Fill(Colour.TagDefaultBg.Get("Select", ColorScheme), path);
                                    var rect_del = rect_left_dels[i];
                                    if (rect_del.Width > 0 && rect_del.Height > 0) g.PaintIconClose(rect_del, Colour.TagDefaultColor.Get("Select", ColorScheme));
                                    g.String(it.ToString(), Font, brush, rect_left_txts[i], sf_center);
                                }
                                else
                                {
                                    using (var brushbg = style.TagBackExtend.BrushEx(rect_lefts[i], style.TagBack ?? Colour.TagDefaultBg.Get("Select", ColorScheme)))
                                    {
                                        g.Fill(brushbg, path);
                                    }
                                    if (style.TagFore.HasValue)
                                    {
                                        var rect_del = rect_left_dels[i];
                                        if (rect_del.Width > 0 && rect_del.Height > 0) g.PaintIconClose(rect_del, style.TagFore.Value);
                                        using (var brushf = new SolidBrush(style.TagFore.Value))
                                        {
                                            g.String(style.Text, Font, brushf, rect_left_txts[i], sf_center);
                                        }
                                    }
                                    else
                                    {
                                        var rect_del = rect_left_dels[i];
                                        if (rect_del.Width > 0 && rect_del.Height > 0) g.PaintIconClose(rect_del, Colour.TagDefaultColor.Get("Select", ColorScheme));
                                        g.String(style.Text, Font, brush, rect_left_txts[i], sf_center);
                                    }
                                }
                            }
                        }
                    }
                    if (rect_lefts.Length != selectedValue.Length)
                    {
                        g.String("+" + (selectedValue.Length - rect_lefts.Length), Font, brush, rect_left_txts[rect_left_txts.Length - 1], sf_center);
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
                    subForm.SetValues(selectedValue);
                }
                select_del = -1;
                return true;
            }
            select_del = -1;
            return false;
        }

        #endregion

        #region 动画

        ISelectMultiple? subForm = null;
        public ILayeredForm? SubForm() => subForm;

        ITask? ThreadExpand = null;
        float ArrowProg = -1F;
        bool expand = false;
        /// <summary>
        /// 箭头是否展开(UI)
        /// </summary>
        [Browsable(false)]
        [Description("箭头是否展开(UI)"), Category("外观"), DefaultValue(false)]
        public bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                if (Config.HasAnimation(nameof(AntdUI.Select)))
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

        #endregion

        #region 焦点

        bool expandDrop = false;
        /// <summary>
        /// 展开下拉菜单
        /// </summary>
        [Browsable(false)]
        [Description("展开下拉菜单"), Category("行为"), DefaultValue(false)]
        public bool ExpandDrop
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
                BeginInvoke(() => ShowLayeredForm(list));
                return;
            }
            Expand = true;
            if (CheckMode) subForm = new LayeredFormSelectMultipleCheck(this, ReadRectangle, list, filtertext);
            else subForm = new LayeredFormSelectMultiple(this, ReadRectangle, list, filtertext);
            subForm.Disposed += (a, b) =>
            {
                select_x = 0;
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
                case Keys.Enter:
                    ExpandDrop = true;
                    break;
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
                SelectedValue = new object[0];
            }
            base.OnClearValue();
        }

        protected override void OnClickContent() => ExpandDrop = !expandDrop;

        #endregion
    }
}