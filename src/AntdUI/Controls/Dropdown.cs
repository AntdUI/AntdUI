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
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Dropdown 选择器
    /// </summary>
    /// <seealso cref="Button"/>
    /// <remarks>向下弹出的列表。</remarks>
    [Description("Dropdown 下拉菜单")]
    [ToolboxItem(true)]
    [DefaultEvent("SelectedValueChanged")]
    public class Dropdown : Button, SubLayeredForm
    {
        #region 属性

        /// <summary>
        /// 列表自动宽度
        /// </summary>
        [Description("列表自动宽度"), Category("行为"), DefaultValue(true)]
        public bool ListAutoWidth { get; set; } = true;

        /// <summary>
        /// 触发下拉的行为
        /// </summary>
        [Description("触发下拉的行为"), Category("行为"), DefaultValue(Trigger.Click)]
        public Trigger Trigger { get; set; } = Trigger.Click;

        /// <summary>
        /// 菜单弹出位置
        /// </summary>
        [Description("菜单弹出位置"), Category("行为"), DefaultValue(TAlignFrom.BL)]
        public TAlignFrom Placement { get; set; } = TAlignFrom.BL;

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
        /// 点击到最里层（无节点才能点击）
        /// </summary>
        [Description("点击到最里层（无节点才能点击）"), Category("行为"), DefaultValue(false)]
        public bool ClickEnd { get; set; }

        /// <summary>
        /// 为空依旧下拉
        /// </summary>
        [Description("为空依旧下拉"), Category("外观"), DefaultValue(false)]
        public bool Empty { get; set; }

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

        /// <summary>
        /// 选中值
        /// </summary>
        [Browsable(false)]
        [Description("选中值"), Category("数据"), DefaultValue(null)]
        public object? SelectedValue { get; set; }

        /// <summary>
        /// SelectedValue 属性值更改时发生
        /// </summary>
        [Description("SelectedValue 属性值更改时发生"), Category("行为")]
        public event ObjectNEventHandler? SelectedValueChanged;

        protected virtual void OnSelectedValueChanged(object? e) => SelectedValueChanged?.Invoke(this, new ObjectNEventArgs(e));

        /// <summary>
        /// 点击项时发生
        /// </summary>
        [Description("点击项时发生"), Category("行为")]
        public event ObjectNEventHandler? ItemClick;

        protected virtual void OnItemClick(object? e) => ItemClick?.Invoke(this, new ObjectNEventArgs(e));

        internal void DropDownChange(object value)
        {
            if (SelectedValue == value) OnItemClick(value);
            else
            {
                SelectedValue = value;
                OnItemClick(value);
                OnSelectedValueChanged(value);
            }
            select_x = 0;
            subForm = null;
        }

        #endregion

        #endregion

        #region 动画

        LayeredFormSelectDown? subForm;
        public ILayeredForm? SubForm() => subForm;

        AnimationTask? ThreadExpand;
        bool expand = false;
        bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                if (ShowArrow && Config.HasAnimation(nameof(Dropdown)))
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

        #endregion

        #region 鼠标

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (Trigger == Trigger.Click)
            {
                ClickDown();
                return;
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (Trigger == Trigger.DoubleClick)
            {
                ClickDown();
                return;
            }
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (Trigger == Trigger.Hover && subForm == null) ClickDown();
            base.OnMouseEnter(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            subForm?.IClose();
            base.OnLostFocus(e);
        }

        internal int select_x = 0;
        void ClickDown()
        {
            if (items != null && items.Count > 0 || Empty)
            {
                if (subForm == null)
                {
                    List<object> objs;
                    if (items != null && items.Count > 0)
                    {
                        objs = new List<object>(items.Count);
                        foreach (var it in items) objs.Add(it);
                    }
                    else objs = new List<object>(0);
                    Expand = true;
                    subForm = new LayeredFormSelectDown(this, objs);
                    subForm.Disposed += (a, b) =>
                    {
                        select_x = 0;
                        subForm = null;
                        Expand = false;
                    };
                    subForm.Show(this);
                }
                else subForm?.IClose();
            }
            else subForm?.IClose();
        }

        #endregion
    }
}