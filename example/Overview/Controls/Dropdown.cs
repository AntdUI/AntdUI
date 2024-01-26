// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace Overview.Controls
{
    public partial class Dropdown : UserControl
    {
        public Dropdown()
        {
            InitializeComponent();
            button17.Items.Add(new AntdUI.SelectItem(Properties.Resources.bg1, "汉尼拔 Hannibal"));
            dropdown1.Items.AddRange(new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("one st menu item"),
                new AntdUI.SelectItem("two nd menu item"),
                new AntdUI.SelectItem("three rd menu item"){
                    Sub = new List<object>{
                        new AntdUI.SelectItem("子菜单1"){
                            Sub=new List<object>{ new AntdUI.SelectItem("sub menu") {
                                Sub=new List<object>{
                                    "one st menu item","two nd menu item","three rd menu item"
                                }
                            } }
                        },
                        new AntdUI.SelectItem( "子菜单2")
                    }
                },
                new AntdUI.SelectItem("four menu item"){ Sub=new List<object>{ "five menu item", "six six six menu item"} },
            });
            dropdown1.Items.AddRange(new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("one st menu item"),
                new AntdUI.SelectItem("two nd menu item"),
                new AntdUI.SelectItem("three rd menu item"){
                    Sub = new List<object>{
                        new AntdUI.SelectItem("子菜单1"){
                            Sub=new List<object>{ new AntdUI.SelectItem("sub menu") {
                                Sub=new List<object>{
                                    "one st menu item","two nd menu item","three rd menu item"
                                }
                            } }
                        },
                        new AntdUI.SelectItem( "子菜单2")
                    }
                },
                new AntdUI.SelectItem("four menu item"){ Sub=new List<object>{ "five menu item", "six six six menu item"} },
            });
        }

        private void dropdown1_SelectedValueChanged(object sender, object value)
        {
            AntdUI.Message.info((Form)Parent, "已选中：" + value, Font);
        }
    }
}