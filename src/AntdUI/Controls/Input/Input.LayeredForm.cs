// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;

namespace AntdUI
{
    internal class InputByLayeredForm : Input
    {
        internal Action? TakePaint;
        public override void Invalidate() => TakePaint?.Invoke();

        [Description("连接左边"), Category("外观"), DefaultValue(0)]
        public int MOffset { get; set; }
        protected override void SetCaretPostion(ref int x, ref int y)
        {
            x += Left - MOffset;
            y += Top - MOffset;
        }

        protected override bool HasAnimation => false;
    }
    internal class InputNumberByLayeredForm : InputNumber
    {
        internal Action? TakePaint;
        public override void Invalidate() => TakePaint?.Invoke();

        [Description("连接左边"), Category("外观"), DefaultValue(0)]
        public int MOffset { get; set; }
        protected override void SetCaretPostion(ref int x, ref int y)
        {
            x += Left - MOffset;
            y += Top - MOffset;
        }

        protected override bool HasAnimation => false;
    }
}