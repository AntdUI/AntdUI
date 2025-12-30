// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Dropdown : UserControl
    {
        AntdUI.BaseForm form;
        public Dropdown(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            button17.Items.Add(new AntdUI.SelectItem(Properties.Resources.bg1, "汉尼拔 Hannibal"));
            dropdown1.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("one st menu item"),
                new AntdUI.SelectItem("two nd menu item"),
                new AntdUI.SelectItem("three rd menu item")
                {
                    Sub = new List<object>
                    {
                        new AntdUI.SelectItem("子菜单1")
                        {
                            Sub = new List<object>
                            {
                                new AntdUI.SelectItem("sub menu")
                                {
                                    Sub = new List<object>
                                    {
                                        "one st menu item",
                                        "two nd menu item",
                                        "three rd menu item"
                                    }
                                }
                            }
                        },
                        new AntdUI.SelectItem("子菜单2")
                    }
                },
                new AntdUI.SelectItem("four menu item")
                {
                    Sub = new List<object>
                    {
                        "five menu item",
                        "six six six menu item"
                    }
                },
            });
        }

        private void dropdown1_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            AntdUI.Message.info(form, "已选中：" + e.Value, Font);
        }
    }
}