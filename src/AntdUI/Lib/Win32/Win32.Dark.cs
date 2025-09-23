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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Win32
    {
        public static bool IsCompositionEnabled
        {
            get
            {
                try
                {
                    int enabled = 0;
                    DwmIsCompositionEnabled(ref enabled);
                    return enabled == 1;
                }
                catch { }
                return false;
            }
        }

        public static bool WindowTheme(Form form, bool dark)
        {
            var r = UseImmersiveDarkMode(form.Handle, dark);
            if (r)
            {
                var code = dark ? "DarkMode_Explorer" : "ClearMode_Explorer";
                foreach (Control it in form.Controls) WindowTheme(it, code);
            }
            return r;
        }
        public static void WindowTheme(Control control)
        {
            if (Config.IsDark) WindowTheme(control, "DarkMode_Explorer");
        }
        public static void WindowTheme(Control control, TMode? colorScheme)
        {
            if (colorScheme.HasValue)
            {
                var code = colorScheme == TMode.Dark ? "DarkMode_Explorer" : "ClearMode_Explorer";
                WindowTheme(control, code);
            }
            else
            {
                var code = Config.IsDark ? "DarkMode_Explorer" : "ClearMode_Explorer";
                WindowTheme(control, code);
            }
        }
        public static void WindowTheme(Control control, bool dark)
        {
            var code = dark ? "DarkMode_Explorer" : "ClearMode_Explorer";
            WindowTheme(control, code);
        }
        static void WindowTheme(Control control, string code)
        {
            if (HasScrollbar(control, out bool set))
            {
                if (set) SetWindowTheme(control.Handle, code, null);
                foreach (Control it in control.Controls)
                {
                    if (HasScrollbar(it, out bool set2))
                    {
                        if (set2) SetWindowTheme(it.Handle, code, null);
                        foreach (Control item in it.Controls) WindowTheme(item, code);
                    }
                }
            }
        }
        static bool HasScrollbar(Control control, out bool set)
        {
            set = false;
            if (control is IControl) return false;
            if (control is ScrollableControl scrollableControl)
            {
                set = scrollableControl.AutoScroll;
                return true;
            }
            if (control is System.Windows.Forms.Panel panel)
            {
                set = panel.AutoScroll;
                return true;
            }
            if (control is TextBoxBase) return true;
            if (control is ListBox) return true;
            if (control is DataGridView) return true;
            if (control is TreeView) return true;
            if (control is SplitContainer) return true;
            if (control is CheckedListBox) return true;
            if (control is WebBrowser) return true;
            return false;
        }

        #region Win32

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string? pszSubIdList);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (OS.Win10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (OS.Win10OrGreater(18985)) attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }

            return false;
        }

        #endregion
    }
}