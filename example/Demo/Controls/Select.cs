// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Select : UserControl
    {
        AntdUI.BaseForm form;
        public Select(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            select5.Items.AddRange(new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("one"){
                    Sub = new List<object>{
                        new AntdUI.SelectItem("子菜单1"){
                            Sub=new List<object>{ new AntdUI.SelectItem("sub menu") {
                                Sub=new List<object>{
                                    "one st menu item","two nd menu item","three rd menu item"
                                }
                            } }
                        }.SetText("子菜单1","Select.sub menu 1"),
                        new AntdUI.SelectItem("子菜单2").SetText("子菜单2","Select.sub menu 2")
                    }
                },
                new AntdUI.SelectItem("two"){ Sub=new List<object>{ "five menu item", "six six six menu item"} },
            });
        }
    }
}