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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AntdUI
{
    partial class Tabs
    {
        internal class TabControlDesigner : ParentControlDesigner
        {
            public new Tabs Control => (Tabs)base.Control;

            #region 初始化

            public IDesignerHost? DesignerHost { get; private set; }
            public ISelectionService? SelectionService { get; private set; }

            public override void Initialize(IComponent component)
            {
                base.Initialize(component);
                DesignerHost = (IDesignerHost)GetService(typeof(IDesignerHost))!;
                SelectionService = (ISelectionService)GetService(typeof(ISelectionService))!;
            }

            DesignerActionListCollection? actionLists;
            public override DesignerActionListCollection ActionLists
            {
                get
                {
                    if (actionLists == null)
                    {
                        actionLists = base.ActionLists;
                        actionLists.Add(new TabControlActionList(Control));
                    }
                    return actionLists;
                }
            }

            #endregion

            protected override bool GetHitTest(Point point)
            {
                var point_ = Control.PointToClient(point);
                foreach (var tab in Control.Pages)
                {
                    if (tab.Contains(point_.X, point_.Y)) return true;
                }
                return base.GetHitTest(point);
            }
        }

        internal class TabControlActionList : DesignerActionList
        {
            #region Properties

            /// <summary>
            /// Gets the associated <see cref="TabControl"/>.
            /// </summary>
            public Tabs Control { get; private set; }

            /// <summary>
            /// Gets the designer host.
            /// </summary>
            public IDesignerHost DesignerHost { get; private set; }

            /// <summary>
            /// Gets the selection service.
            /// </summary>
            public ISelectionService SelectionService { get; private set; }

            #endregion

            #region Actions

            public TabAlignment Alignment
            {
                get { return Control.Alignment; }
                set { GetPropertyByName("Alignment").SetValue(Control, value); }
            }

            public void AddTab()
            {
                if (DesignerHost != null)
                {
                    var tab = (TabPage)DesignerHost.CreateComponent(typeof(TabPage));
                    var nameProp = TypeDescriptor.GetProperties(tab)["Name"];
                    var name = (string?)nameProp?.GetValue(tab);
                    var textProp = TypeDescriptor.GetProperties(tab)["Text"];
                    textProp?.SetValue(tab, name);
                    Control.Pages.Add(tab);
                    Control.SelectedIndex = Control.Pages.IndexOf(tab);
                }
            }

            protected void RemoveTab()
            {
                if (DesignerHost != null)
                {
                    if (Control.Pages.Count > 1)
                    {
                        int index = Control.SelectedIndex;
                        //DesignerHost.DestroyComponent(tab);
                        //if (index == Control.Tabs.Count)
                        //    index = Control.Tabs.Count - 1;
                        Control.SelectedIndex = index;
                    }
                }
            }

            #endregion

            #region Constructor

            public TabControlActionList(IComponent component) : base(component)
            {
                Control = (Tabs)component;
                DesignerHost = (IDesignerHost)GetService(typeof(IDesignerHost))!;
                SelectionService = (ISelectionService)GetService(typeof(ISelectionService))!;
            }

            #endregion

            #region Helper Methods

            private PropertyDescriptor GetPropertyByName(string propName)
            {
                var prop = TypeDescriptor.GetProperties(Control)[propName];
                if (prop == null) throw new ArgumentException("Unknown property.", propName);
                else return prop;
            }

            #endregion

            #region Overrides

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                return new DesignerActionItemCollection() {
                    new DesignerActionHeaderItem("外观"),
                    new DesignerActionHeaderItem("数据"),
                    new DesignerActionPropertyItem("Alignment", "选项卡位置", "外观", "确定选项卡是否显示在控件的顶部、底部、左侧或右侧(在左侧或右侧将隐式地分为多行)。"),
                    new DesignerActionMethodItem(this, "AddTab", "添加选项卡", "数据", "向控件添加新选项卡"),
                    new DesignerActionMethodItem(this, "RemoveTab", "移除选项卡", "数据" ,"删除当前选项卡")
                };
            }

            #endregion
        }
    }
}