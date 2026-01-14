// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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

        public static bool WindowTheme(Form form, bool dark, bool one = false)
        {
            var r = UseImmersiveDarkMode(form.Handle, dark);
            if (r)
            {
                if (one && !dark) return r;
                var code = dark ? "DarkMode_Explorer" : "ClearMode_Explorer";
                foreach (Control it in form.Controls) WindowTheme(it, code);
            }
            return r;
        }
        public static void WindowTheme(Control control)
        {
            if (Config.IsDark) WindowTheme(control, "DarkMode_Explorer");
        }
        public static void WindowTheme(Control control, TAMode mode)
        {
            switch (mode)
            {
                case TAMode.Light:
                    WindowTheme(control, "ClearMode_Explorer");
                    break;
                case TAMode.Dark:
                    WindowTheme(control, "DarkMode_Explorer");
                    break;
                case TAMode.Auto:
                default:
                    var code = Config.IsDark ? "DarkMode_Explorer" : "ClearMode_Explorer";
                    WindowTheme(control, code);
                    break;
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

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref uint attrValue, int attrSize);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string? pszSubIdList);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_BORDER_COLOR = 34;
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

        public static bool SetWindowBorderColor(IntPtr handle, System.Drawing.Color color)
        {
            try
            {
                uint rgb = color.R | (uint)color.G << 8 | (uint)color.B << 16;
                //uint rgb = dark ? 0x00232323u : 0x00E0E0E0u;
                return DwmSetWindowAttribute(handle, DWMWA_BORDER_COLOR, ref rgb, sizeof(uint)) == 0;
            }
            catch { }
            return false;
        }

        #endregion
    }
}