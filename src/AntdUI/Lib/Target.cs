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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public class Target
    {
        #region 构造函数

        public Target(Form form)
        {
            Value = form;
            IsForm = true;
        }
        public Target(IWin32Window form)
        {
            Value = form;
            IsForm = true;
        }
        public Target(Control control)
        {
            Value = control;
            IsControl = true;
        }
        internal Target()
        {
        }

        #endregion

        public static Target Null => new Target();

        public object? Value { get; set; }

        public bool IsForm { get; private set; }
        public bool IsControl { get; private set; }

        public Form? GetForm
        {
            get
            {
                if (Value is Form t) return t;
                return null;
            }
        }
        public Control? GetControl
        {
            get
            {
                if (Value is Control t) return t;
                return null;
            }
        }

        public bool IsCreated(out bool invoke, out Control? obj)
        {
            obj = null;
            invoke = false;
            if (Value is Control t && t.IsHandleCreated)
            {
                obj = t;
                invoke = t.InvokeRequired;
                return true;
            }
            else if (Value is IWin32Window) return true;
            return false;
        }
        public void SetFont(Font? font, Form form)
        {
            if (Value is Control t) form.Font = font ?? t.Font;
            else if (font != null) form.Font = font;
        }
        public void SetFontConfig(Font? font, Form form)
        {
            if (Value is Control t) form.Font = font ?? t.Font;
            else
            {
                if (font != null) form.Font = font;
                else if (Config.Font != null) form.Font = Config.Font;
            }
        }
        public void SetIcon(Form form)
        {
            if (Value is Form t) form.Icon = t.Icon;
        }

        public void Show(Form form)
        {
            if (Value is Control t) form.Show(t);
            else if (Value is IWin32Window win32Window) form.Show(win32Window);
            else form.Show();
        }

        public DialogResult ShowDialog(Form form)
        {
            if (Value is Control t) return form.ShowDialog(t);
            else if (Value is IWin32Window win32Window) return form.ShowDialog(win32Window);
            else return form.ShowDialog();
        }

        public void BeginInvoke(Action action)
        {
            if (Value is Control t) t.BeginInvoke(action);
            else action();
        }
    }
}