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
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Dropdown 选择器
    /// </summary>
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

        /// <summary>
        /// SelectedValue 属性值更改时发生
        /// </summary>
        [Description("SelectedValue 属性值更改时发生"), Category("行为")]
        public event ObjectNEventHandler? SelectedValueChanged = null;

        internal void DropDownChange(object value)
        {
            SelectedValueChanged?.Invoke(this, new ObjectNEventArgs(value));
            select_x = 0;
            subForm = null;
        }

        #endregion

        #endregion

        #region 动画

        LayeredFormSelectDown? subForm = null;
        public ILayeredForm? SubForm() => subForm;

        ITask? ThreadExpand = null;
        bool expand = false;
        bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                if (ShowArrow && Config.Animation)
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
                    subForm = new LayeredFormSelectDown(this, Radius, objs);
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