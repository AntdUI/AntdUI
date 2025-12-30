// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
    /// <seealso cref="Input"/>
    /// <remarks>下拉选择器。</remarks>
    [Description("Select 选择器")]
    [ToolboxItem(true)]
    [DefaultEvent("SelectedIndexChanged")]
    public class Select : Input, SubLayeredForm, IEventListener
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
        /// 下拉圆角
        /// </summary>
        [Description("下拉圆角"), Category("外观"), DefaultValue(null)]
        public int? DropDownRadius { get; set; }

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
        /// 下拉文本方向
        /// </summary>
        [Description("下拉文本方向"), Category("外观"), DefaultValue(TAlign.Left)]
        public TAlign DropDownTextAlign { get; set; } = TAlign.Left;

        /// <summary>
        /// 是否允许回车键触发下拉框
        /// </summary>
        [Description("是否允许回车键触发下拉框"), Category("行为"), DefaultValue(true)]
        public bool EnterDropDown { get; set; } = true;

        /// <summary>
        /// 下拉为空关闭
        /// </summary>
        [Description("下拉为空关闭"), Category("行为"), DefaultValue(false)]
        public bool DropDownEmptyClose { get; set; }

        /// <summary>
        /// 点击到最里层（无节点才能点击）
        /// </summary>
        [Description("点击到最里层（无节点才能点击）"), Category("行为"), DefaultValue(false)]
        public bool ClickEnd { get; set; }

        /// <summary>
        /// 点击切换下拉
        /// </summary>
        [Description("点击切换下拉"), Category("行为"), DefaultValue(true)]
        public bool ClickSwitchDropdown { get; set; } = true;

        /// <summary>
        /// 是否显示关闭图标
        /// </summary>
        [Description("是否显示关闭图标"), Category("行为"), DefaultValue(false)]
        public bool CloseIcon { get; set; }

        /// <summary>
        /// 为空依旧下拉
        /// </summary>
        [Description("为空依旧下拉"), Category("外观"), DefaultValue(false)]
        public bool Empty { get; set; }

        /// <summary>
        /// 自动设置下拉前缀
        /// </summary>
        [Description("自动设置下拉前缀"), Category("外观"), DefaultValue(false)]
        public bool AutoPrefixSvg { get; set; }

        /// <summary>
        /// 自动设置文本
        /// </summary>
        [Description("自动设置文本"), Category("外观"), DefaultValue(true)]
        public bool AutoText { get; set; } = true;

        /// <summary>
        /// 鼠标滚轮修改值
        /// </summary>
        [Description("鼠标滚轮修改值"), Category("交互"), DefaultValue(true)]
        public bool WheelModifyEnabled { get; set; } = true;

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
                filtertext = null;
                if (items == null || items.Count == 0 || value == -1) ChangeValueNULL();
                else
                {
                    var obj = items[value];
                    if (obj == null) return;
                    ChangeValue(value, obj);
                }
                if (_list) Invalidate();
                OnPropertyChanged(nameof(SelectedIndex));
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
                filtertext = null;
                if (value == null || items == null || items.Count == 0) ChangeValueNULL();
                else SetChangeValue(items, value);
                if (_list) Invalidate();
                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        int selectedIndexX = -1;
        int selectedIndex = -1;
        object? selectedValue;
        SelectItem? selectedItem;

        void ChangeValueNULL()
        {
            Text = "";
            selectedItem = null;
            selectedValue = null;
            selectedIndex = -1;
            OnSelectedValueChanged(selectedValue);
            OnSelectedIndexChanged(selectedIndex);
        }

        void ChangeValue(int value, object? obj)
        {
            selectedIndex = value;
            if (obj is SelectItem it)
            {
                selectedItem = it;
                selectedValue = it.Tag;
                if (AutoPrefixSvg) PrefixSvg = it.IconSvg;
                if (AutoText) Text = it.Text;
            }
            else
            {
                selectedItem = null;
                selectedValue = obj;
                if (AutoText)
                {
                    if (obj == null) Text = "";
                    else Text = obj.ToString() ?? "";
                }
            }
            OnSelectedValueChanged(selectedValue);
            OnSelectedIndexChanged(selectedIndex);
        }
        void SetChangeValue(BaseCollection items, object val)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (val.Equals(item))
                {
                    ChangeValue(i, item);
                    return;
                }
                else if (item is SelectItem it && it.Tag.Equals(val))
                {
                    ChangeValue(i, item);
                    return;
                }
                else if (item is SelectItem itp && itp.Sub != null && itp.Sub.Count > 0)
                {
                    foreach (var sub in itp.Sub)
                    {
                        if (val.Equals(sub))
                        {
                            ChangeValue(i, sub);
                            return;
                        }
                        else if (sub is SelectItem it2 && it2.Tag.Equals(val))
                        {
                            ChangeValue(i, sub);
                            return;
                        }
                    }
                }
                else if (item is GroupSelectItem group && group.Sub != null && group.Sub.Count > 0)
                {
                    foreach (var sub in group.Sub)
                    {
                        if (val.Equals(sub))
                        {
                            ChangeValue(i, sub);
                            return;
                        }
                        else if (sub is SelectItem it2 && it2.Tag.Equals(val))
                        {
                            ChangeValue(i, sub);
                            return;
                        }
                    }
                }
            }
            ChangeValue(items.IndexOf(val), val);
        }

        internal void DropDownChange(int x, int y, object value, SelectItem? item, string text)
        {
            selectedIndexX = x;
            selectedIndex = y;
            selectedItem = item;
            selectedValue = value;
            if (AutoText) Text = text;
            if (AutoPrefixSvg && item != null) PrefixSvg = item.IconSvg;
            OnSelectedValueChanged(selectedValue);
            OnSelectedIndexChanged(selectedIndex);
            SelectedIndexsChanged?.Invoke(this, new IntXYEventArgs(selectedIndexX, selectedIndex));
            OnPropertyChanged(nameof(SelectedIndex));
            OnPropertyChanged(nameof(SelectedValue));
            ExpandDrop = false;
            select_x = 0;
            subForm = null;
        }

        internal bool DropDownClose(object value)
        {
            if (ClosedItem == null) return false;
            ClosedItem(this, new ObjectNEventArgs(value));
            return true;
        }

        #endregion

        /// <summary>
        /// SelectedIndex 属性值更改时发生
        /// </summary>
        [Description("SelectedIndex 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? SelectedIndexChanged;

        protected virtual void OnSelectedIndexChanged(int e) => SelectedIndexChanged?.Invoke(this, new IntEventArgs(e));

        /// <summary>
        /// 多层树结构更改时发生
        /// </summary>
        [Description("多层树结构更改时发生"), Category("行为"), Obsolete("use SelectedValueChanged")]
        public event IntXYEventHandler? SelectedIndexsChanged;

        /// <summary>
        /// SelectedValue 属性值更改时发生
        /// </summary>
        [Description("SelectedValue 属性值更改时发生"), Category("行为")]
        public event ObjectNEventHandler? SelectedValueChanged;

        protected virtual void OnSelectedValueChanged(object? e) => SelectedValueChanged?.Invoke(this, new ObjectNEventArgs(e));

        /// <summary>
        /// 下拉展开 属性值更改时发生
        /// </summary>
        [Description("下拉展开 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? ExpandDropChanged;

        protected virtual void OnExpandDropChanged(bool e) => ExpandDropChanged?.Invoke(this, new BoolEventArgs(e));

        /// <summary>
        /// 关闭某项 时发生
        /// </summary>
        [Description("关闭某项 时发生"), Category("行为")]
        public event ObjectNEventHandler? ClosedItem;

        public delegate IList<object>? FilterEventHandler(object sender, string value);

        /// <summary>
        /// 控制筛选 Text更改时发生
        /// </summary>
        [Description("控制筛选 Text更改时发生"), Category("行为")]
        public event FilterEventHandler? FilterChanged;

        string? filtertext;
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (HasFocus)
            {
                if (filtertext == Text) return;
                filtertext = Text;
                if (FilterChanged == null)
                {
                    if (expandDrop) subForm?.TextChange(Text);
                    else ExpandDrop = true;
                }
                else
                {
                    string temp = filtertext;
                    ITask.Run(() =>
                    {
                        var list = FilterChanged(this, temp);
                        if (filtertext == temp)
                        {
                            Items.Clear();
                            if (list == null || list.Count == 0) subForm?.IClose();
                            else
                            {
                                Items.AddRange(list);
                                if (subForm == null) ShowLayeredForm(list);
                                else subForm.TextChange(list);
                            }
                        }
                    });
                }
            }
        }

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
                using (var pen = new Pen(Colour.TextQuaternary.Get(nameof(Select), ColorScheme), 2F))
                {
                    pen.StartCap = pen.EndCap = LineCap.Round;
                    g.DrawLines(pen, rect_r.TriangleLinesVertical(ArrowProg));
                }
            }
        }

        #endregion

        #endregion

        #region 动画

        LayeredFormSelectDown? subForm;
        public ILayeredForm? SubForm() => subForm;

        AnimationTask? ThreadExpand;
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
                    ThreadExpand = new AnimationTask(new AnimationFixedConfig(i =>
                    {
                        ArrowProg = i;
                        Invalidate();
                    }, 10, Animation.TotalFrames(10, 100), value, AnimationType.Ball).SetArrow());
                }
                else ArrowProg = value ? 1F : -1F;
            }
        }

        internal int select_x = 0;

        #endregion

        #region 下拉菜单

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
                        if (Empty && subForm == null) ShowLayeredForm(new List<object>(0));
                        else
                        {
                            subForm?.IClose();
                            subForm = null;
                            expandDrop = false;
                        }
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
                    subForm = null;
                    filtertext = "";
                }
                OnExpandDropChanged(value);
            }
        }

        void ShowLayeredForm(IList<object> list)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(() => ShowLayeredForm(list));
                    return;
                }
                Expand = true;
                subForm = new LayeredFormSelectDown(this, list, filtertext);
                subForm.Show(this);
                subForm.Disposed += (a, b) =>
                {
                    select_x = 0;
                    subForm = null;
                    Expand = false;
                    ExpandDrop = false;
                };
            }
            catch
            {
                subForm = null;
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            var r = base.ProcessCmdKey(ref msg, keyData);
            switch (keyData)
            {
                case Keys.Down:
                    ExpandDrop = true;
                    return true;
                case Keys.Enter:
                    if (EnterDropDown) ExpandDrop = true;
                    break;
            }
            return r;
        }
        protected override void OnLostFocus(EventArgs e)
        {
            ExpandDrop = false;
            base.OnLostFocus(e);
        }

        #endregion

        #region 鼠标

        protected override void OnClearValue()
        {
            if (selectedIndex > -1 || selectedValue != null || !isempty)
            {
                filtertext = null;
                ChangeValueNULL();
                Invalidate();
            }
        }

        protected override void OnClickContent()
        {
            if (_list || ClickSwitchDropdown) ExpandDrop = !expandDrop;
            else
            {
                if (HasFocus)
                {
                    if (expandDrop) return;
                    ExpandDrop = !expandDrop;
                }
                else Focus();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (ReadOnly || !WheelModifyEnabled || items == null || items.Count == 0) return;
            SelectedIndex = IMouseWheel(items, e.Delta);
            if (e is HandledMouseEventArgs handled) handled.Handled = true;
        }

        int IMouseWheel(BaseCollection items, int delta)
        {
            int count = items.Count, errcount = 0, newIndex;
            if (delta > 0)
            {
                newIndex = SelectedIndex <= 0 ? items.Count - 1 : SelectedIndex - 1;
                while (WheelItem(items[newIndex]))
                {
                    errcount++;
                    if (errcount > count) return -1;
                    newIndex--;
                    if (newIndex < 0) newIndex = count - 1;
                }
            }
            else
            {
                newIndex = SelectedIndex >= items.Count - 1 ? 0 : SelectedIndex + 1;
                while (WheelItem(items[newIndex]))
                {
                    errcount++;
                    if (errcount > count) return -1;
                    newIndex++;
                    if (newIndex > count - 1) newIndex = 0;
                }
            }
            return newIndex;
        }
        bool WheelItem(object? it)
        {
            if (it is DividerSelectItem) return true;
            return false;
        }

        #endregion

        #region 语言

        protected override void OnHandleCreated(EventArgs e)
        {
            this.AddListener();
            base.OnHandleCreated(e);
        }
        public void HandleEvent(EventType id, object? tag)
        {
            if (selectedItem == null) return;
            switch (id)
            {
                case EventType.LANG:
                    if (AutoText) Text = selectedItem.Text;
                    break;
            }
        }

        #endregion
    }

    public class DividerSelectItem : ISelectItem
    {
    }
    public class SelectItem : ISelectItem, iSelectItem
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
            _text = tag.ToString() ?? string.Empty;
            Tag = tag;
        }
        public SelectItem(string text, object tag)
        {
            _text = text;
            Tag = tag;
        }
        /// <summary>
        /// 在线状态
        /// </summary>
        public int? Online { get; set; }
        /// <summary>
        /// 在线自定义颜色
        /// </summary>
        public Color? OnlineCustom { get; set; }

        public Image? Icon { get; set; }

        public string? IconSvg { get; set; }

        /// <summary>
        /// 图标比例
        /// </summary>
        public float? IconRatio { get; set; }

        string _text;
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get => Localization.GetLangIN(LocalizationText, _text, new string?[] { "{id}", Tag.ToString() });
            set => _text = value;
        }

        /// <summary>
        /// 国际化（文本）
        /// </summary>
        public string? LocalizationText { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        string? subText;
        /// <summary>
        /// 子文本
        /// </summary>
        public string? SubText
        {
            get => Localization.GetLangI(LocalizationSubText, subText, new string?[] { "{id}", Tag.ToString() });
            set => subText = value;
        }

        /// <summary>
        /// 国际化（子文本）
        /// </summary>
        public string? LocalizationSubText { get; set; }

        /// <summary>
        /// 子选项
        /// </summary>
        public IList<object>? Sub { get; set; }

        public int MaxCount { get; set; }

        public object Tag { get; set; }

        #region 主题

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color? Fore { get; set; }

        /// <summary>
        /// 子文字颜色
        /// </summary>
        public Color? ForeSub { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        public Color? BackActive { get; set; }

        /// <summary>
        /// 激活背景渐变色
        /// </summary>
        public string? BackActiveExtend { get; set; }

        #endregion

        #region 标签

        /// <summary>
        /// 标签文字颜色
        /// </summary>
        public Color? TagFore { get; set; }

        /// <summary>
        /// 标签背景颜色
        /// </summary>
        public Color? TagBack { get; set; }

        /// <summary>
        /// 标签背景渐变色
        /// </summary>
        public string? TagBackExtend { get; set; }

        #endregion

        #region 设置

        public SelectItem SetText(string value, string? localization = null)
        {
            _text = value;
            LocalizationText = localization;
            return this;
        }

        public SelectItem SetSubText(string? value, string? localization = null)
        {
            subText = value;
            LocalizationSubText = localization;
            return this;
        }

        public SelectItem SetEnable(bool value = false)
        {
            Enable = value;
            return this;
        }

        #region Online

        public SelectItem SetOnline(int? value = 1)
        {
            Online = value;
            return this;
        }

        public SelectItem SetOnline(Color? value)
        {
            OnlineCustom = value;
            return this;
        }

        public SelectItem SetOnline(int? value, Color? color)
        {
            Online = value;
            OnlineCustom = color;
            return this;
        }

        #endregion

        #region 图标

        public SelectItem SetIcon(Image? img)
        {
            Icon = img;
            return this;
        }

        public SelectItem SetIcon(string? svg)
        {
            IconSvg = svg;
            return this;
        }

        public SelectItem SetIconRatio(float? value)
        {
            IconRatio = value;
            return this;
        }

        #endregion

        public SelectItem SetSub(params object[] value)
        {
            Sub = value;
            return this;
        }

        public SelectItem SetSub(int value)
        {
            MaxCount = value;
            return this;
        }

        #region 主题

        public SelectItem SetFore(Color? value)
        {
            Fore = value;
            return this;
        }
        public SelectItem SetForeSub(Color? value)
        {
            ForeSub = value;
            return this;
        }

        public SelectItem SetFore(Color? value, Color? sub)
        {
            Fore = value;
            ForeSub = sub;
            return this;
        }
        public SelectItem SetBackActive(Color? value)
        {
            BackActive = value;
            return this;
        }
        public SelectItem SetBackActive(string? value)
        {
            BackActiveExtend = value;
            return this;
        }

        #endregion

        #region 标签

        public SelectItem SetTagFore(Color? value)
        {
            TagFore = value;
            return this;
        }
        public SelectItem SetTagBack(Color? value)
        {
            TagBack = value;
            return this;
        }
        public SelectItem SetTagBack(string? value)
        {
            TagBackExtend = value;
            return this;
        }

        #endregion

        #endregion

        public override string ToString() => Text;
    }

    internal interface iSelectItem
    {
        /// <summary>
        /// 在线状态
        /// </summary>
        int? Online { get; set; }
        /// <summary>
        /// 在线自定义颜色
        /// </summary>
        Color? OnlineCustom { get; set; }

        Image? Icon { get; set; }

        string? IconSvg { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// 子文本
        /// </summary>
        string? SubText { get; set; }

        /// <summary>
        /// 子选项
        /// </summary>
        IList<object>? Sub { get; set; }

        object Tag { get; set; }

        #region 主题

        /// <summary>
        /// 文字颜色
        /// </summary>
        Color? Fore { get; set; }

        /// <summary>
        /// 子文字颜色
        /// </summary>
        Color? ForeSub { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        Color? BackActive { get; set; }

        /// <summary>
        /// 激活背景渐变色
        /// </summary>
        string? BackActiveExtend { get; set; }

        #endregion

        #region 标签

        /// <summary>
        /// 标签文字颜色
        /// </summary>
        Color? TagFore { get; set; }

        /// <summary>
        /// 标签背景颜色
        /// </summary>
        Color? TagBack { get; set; }

        /// <summary>
        /// 标签背景渐变色
        /// </summary>
        string? TagBackExtend { get; set; }

        #endregion
    }

    public class GroupSelectItem : ISelectItem
    {
        public GroupSelectItem(string title)
        {
            _title = title;
        }

        string _title;
        /// <summary>
        /// 文本
        /// </summary>
        public string Title
        {
            get => Localization.GetLangIN(LocalizationTitle, _title);
            set => _title = value;
        }

        /// <summary>
        /// 国际化（文本）
        /// </summary>
        public string? LocalizationTitle { get; set; }

        /// <summary>
        /// 子选项
        /// </summary>
        public IList<object>? Sub { get; set; }

        public object? Tag { get; set; }

        public override string ToString() => Title;
    }
    public class ISelectItem { }

    public class iItemSearchWeigth
    {
        public iItemSearchWeigth(int weigth, object value)
        {
            Weight = weigth;
            Value = value;
        }
        public int Weight { get; set; }
        public virtual object Value { get; set; }
    }
    public class ItemSearchWeigth<T> : iItemSearchWeigth
    {
        public ItemSearchWeigth(int weigth, T value) : base(weigth, value!)
        {
            Value = value;
        }
        public new T Value { get; set; }
    }

    internal class ObjectItem : iSelectItem
    {
        public ObjectItem(object item, int i, Rectangle rect, Rectangle rect_text)
        {
            I = i;
            Tag = item;
            Text = item.ToString() ?? string.Empty;
            Rect = rect;
            RectText = rect_text;
        }

        public ObjectItem(GroupSelectItem item, int i, Rectangle rect, Rectangle rect_text)
        {
            I = i;
            Group = true;
            Tag = item;
            Text = item.Title;
            Rect = rect;
            RectText = rect_text;
        }

        public ObjectItem(SelectItem item, int i, Rectangle rect)
        {
            I = i;
            Rect = rect;
            Online = item.Online;
            OnlineCustom = item.OnlineCustom;
            Icon = item.Icon;
            IconSvg = item.IconSvg;
            Text = item.Text;
            SubText = item.SubText;
            Enable = item.Enable;
            Sub = item.Sub;
            if (Sub != null && Sub.Count > 0) HasSub = true;
            Select = item;
            Tag = item.Tag;
            TagBack = item.TagBack;
            TagBackExtend = item.TagBackExtend;
            TagFore = item.TagFore;
            Fore = item.Fore;
            ForeSub = item.ForeSub;
            BackActive = item.BackActive;
            BackActiveExtend = item.BackActiveExtend;
            MaxCount = item.MaxCount;
        }

        /// <summary>
        /// 线条布局
        /// </summary>
        public ObjectItem(int i, Rectangle rect)
        {
            I = i;
            Text = null!;
            SID = false;
            Tag = Rect = rect;
        }

        #region 属性

        /// <summary>
        /// 在线状态
        /// </summary>
        public int? Online { get; set; }
        /// <summary>
        /// 在线自定义颜色
        /// </summary>
        public Color? OnlineCustom { get; set; }

        public Image? Icon { get; set; }

        public string? IconSvg { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 子文本
        /// </summary>
        public string? SubText { get; set; }

        /// <summary>
        /// 子选项
        /// </summary>
        public IList<object>? Sub { get; set; }

        public int MaxCount { get; set; }

        public object Tag { get; set; }
        public SelectItem? Select { get; set; }

        #region 主题

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color? Fore { get; set; }

        /// <summary>
        /// 子文字颜色
        /// </summary>
        public Color? ForeSub { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        public Color? BackActive { get; set; }

        /// <summary>
        /// 激活背景渐变色
        /// </summary>
        public string? BackActiveExtend { get; set; }

        #endregion

        #region 标签

        /// <summary>
        /// 标签文字颜色
        /// </summary>
        public Color? TagFore { get; set; }

        /// <summary>
        /// 标签背景颜色
        /// </summary>
        public Color? TagBack { get; set; }

        /// <summary>
        /// 标签背景渐变色
        /// </summary>
        public string? TagBackExtend { get; set; }

        #endregion

        #endregion

        public bool HasSub { get; set; }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => IconSvg != null || Icon != null;

        public Rectangle RectIcon { get; set; }
        public Rectangle RectOnline { get; set; }

        public bool Hover { get; set; }
        public bool Group { get; set; }

        public bool SID { get; set; } = true;
        public bool NoIndex { get; set; }
        public int I { get; set; }

        public Rectangle Rect { get; set; }
        public Rectangle RectText { get; set; }
        public Rectangle RectArrow { get; set; }
        public Rectangle RectClose { get; set; }
        public bool HoverClose { get; set; }

        public bool SetHover(bool val)
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
        public bool Contains(int x, int y, int sx, int sy, out bool change)
        {
            if (SID && Rect.Contains(x + sx, y + sy))
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
    }

    internal class ObjectItemCheck : iSelectItem
    {
        public ObjectItemCheck(object item, Rectangle rect, Rectangle rect_text, Rectangle rect_check)
        {
            Tag = item;
            Text = item.ToString() ?? string.Empty;
            Rect = rect;
            RectText = rect_text;
            RectCheck = rect_check;
        }

        public ObjectItemCheck(GroupSelectItem item, Rectangle rect, Rectangle rect_text)
        {
            Group = true;
            Tag = item;
            Text = item.Title;
            Rect = rect;
            RectText = rect_text;
        }

        public ObjectItemCheck(SelectItem item, Rectangle rect, Rectangle rect_check)
        {
            Online = item.Online;
            OnlineCustom = item.OnlineCustom;
            Icon = item.Icon;
            IconSvg = item.IconSvg;
            Text = item.Text;
            SubText = item.SubText;
            Enable = item.Enable;
            Sub = item.Sub;
            if (Sub != null && Sub.Count > 0) HasSub = true;
            Tag = item.Tag;
            TagBack = item.TagBack;
            TagBackExtend = item.TagBackExtend;
            TagFore = item.TagFore;
            Fore = item.Fore;
            ForeSub = item.ForeSub;
            BackActive = item.BackActive;
            BackActiveExtend = item.BackActiveExtend;
            Rect = rect;
            RectCheck = rect_check;
            MaxCount = item.MaxCount;
        }

        public ObjectItemCheck(Rectangle rect)
        {
            Text = null!;
            SID = false;
            Tag = Rect = rect;
        }

        #region 属性

        /// <summary>
        /// 在线状态
        /// </summary>
        public int? Online { get; set; }
        /// <summary>
        /// 在线自定义颜色
        /// </summary>
        public Color? OnlineCustom { get; set; }

        public Image? Icon { get; set; }

        public string? IconSvg { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 子文本
        /// </summary>
        public string? SubText { get; set; }

        /// <summary>
        /// 子选项
        /// </summary>
        public IList<object>? Sub { get; set; }

        public int MaxCount { get; set; }

        public object Tag { get; set; }

        #region 主题

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color? Fore { get; set; }

        /// <summary>
        /// 子文字颜色
        /// </summary>
        public Color? ForeSub { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        public Color? BackActive { get; set; }

        /// <summary>
        /// 激活背景渐变色
        /// </summary>
        public string? BackActiveExtend { get; set; }

        #endregion

        #region 标签

        /// <summary>
        /// 标签文字颜色
        /// </summary>
        public Color? TagFore { get; set; }

        /// <summary>
        /// 标签背景颜色
        /// </summary>
        public Color? TagBack { get; set; }

        /// <summary>
        /// 标签背景渐变色
        /// </summary>
        public string? TagBackExtend { get; set; }

        #endregion

        #endregion

        public bool HasSub { get; set; }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => IconSvg != null || Icon != null;

        public Rectangle RectIcon { get; set; }
        public Rectangle RectOnline { get; set; }

        public bool Hover { get; set; }
        public bool Group { get; set; }

        public bool SID { get; set; } = true;
        public bool NoIndex { get; set; }

        public Rectangle Rect { get; set; }
        public Rectangle RectCheck { get; set; }
        public Rectangle RectText { get; set; }
        public Rectangle RectArrow { get; set; }
        public Rectangle RectClose { get; set; }
        public bool HoverClose { get; set; }

        public bool SetHover(bool val)
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
        public bool Contains(int x, int y, int sx, int sy, out bool change)
        {
            if (SID && Rect.Contains(x + sx, y + sy))
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
    }

    internal class ItemIndex
    {
        public ItemIndex(IList<object> list)
        {
            List = list;
            Dir = new Dictionary<object, int>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (Dir.ContainsKey(item)) continue;
                Dir.Add(item, i);
            }
        }

        public IList<object> List { get; private set; }
        public Dictionary<object, int> Dir { get; private set; }

        public int this[object obj]
        {
            get
            {
                if (Dir.TryGetValue(obj, out var i)) return i;
                return -1;
            }
        }
    }
}