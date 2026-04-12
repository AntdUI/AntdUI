// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using Demo.Controls;
using System.Collections.Generic;

namespace Demo
{
    public class Button
    {
        public static List<ViewPage> Page(AntdUI.BaseForm form)
        {
            return new List<ViewPage> {
                new ViewPage("dType", "按钮类型", new Button1(form)),
                new ViewPage("dIcon", "按钮图标", new Button2(form)),
                new ViewPage("dShape", "按钮形状", new Button3(form)),
                new ViewPage("dLink", "连接按钮", new Button4(form)),
                new ViewPage("dIconPosition", "图标位置", new Button5(form)),
                new ViewPage("dCombo", "组合按钮", new Button6(form)),
                new ViewPage("dGradient", "渐变按钮", new Button7(form)),
                new ViewPage("dToggle", "切换按钮", new Button8(form)),
                new ViewPage("dAnimate", "动画按钮", new Button9(form))
            };
        }
    }
}