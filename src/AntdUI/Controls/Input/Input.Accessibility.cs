// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        protected override AccessibleObject CreateAccessibilityInstance() => new InputAccessibleObject(this);

        /// <summary>
        /// 无障碍对象：将 Input 暴露为可编辑文本（MSAA Text 角色 + 值），经系统 MSAA/UIA 桥接为 Edit 控件类型，供屏幕阅读器、系统触摸键盘与第三方屏幕键盘识别并自动唤起
        /// </summary>
        class InputAccessibleObject : ControlAccessibleObject
        {
            readonly Input input;
            public InputAccessibleObject(Input owner) : base(owner)
            {
                input = owner;
            }

            /// <summary>
            /// 文本角色（MSAA ROLE_SYSTEM_TEXT，UIA 映射为 Edit）
            /// </summary>
            public override AccessibleRole Role => AccessibleRole.Text;

            public override string? Name => input.AccessibleName ?? input.PlaceholderText;

            public override string? Value
            {
                get => input.Text;
                set
                {
                    if (!input.readOnly && value != null) input.Text = value;
                }
            }

            public override AccessibleStates State
            {
                get
                {
                    var state = base.State;
                    if (input.readOnly) state |= AccessibleStates.ReadOnly;
                    return state;
                }
            }
        }
    }
}