// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using Demo.Controls;
using System.Collections.Generic;

namespace Demo
{
    public class Menu
    {
        public static List<ViewPage> Page(AntdUI.BaseForm form)
        {
            return new List<ViewPage> {
                new ViewPage("Menu.dTop", "顶部导航", new Menu1(form)),
                new ViewPage("Menu.dMnemonic", "助记键", new Menu2(form)),
                new ViewPage("Menu.dInline", "内嵌菜单", new Menu3(form)) { H = 220 },
                new ViewPage("Menu.dInlineCollapse", "缩起内嵌菜单", new Menu4(form)) { H = 260 },
                new ViewPage("Menu.dVertical", "垂直菜单", new Menu5(form)) { H = 200 }
            };
        }
    }
}
