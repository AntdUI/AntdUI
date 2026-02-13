// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        public static void DpiAuto(float dpi, Control control)
        {
            if (dpi == 1F)
            {
                if (control is Window window && window.StartPosition == FormStartPosition.CenterScreen)
                {
                    var size = window.sizeInit ?? window.ClientSize;
                    var screen = Screen.FromPoint(window.Location).WorkingArea;
                    window.Location = new Point(screen.X + (screen.Width - size.Width) / 2, screen.Y + (screen.Height - size.Height) / 2);
                }
                return;
            }
            if (Config.DpiMode == DpiMode.Compatible)
            {
                DpiCompatible(dpi, control);
                return;
            }
            if (control is Form form)
            {
                switch (form.AutoScaleMode)
                {
                    case AutoScaleMode.Font:
                    case AutoScaleMode.Dpi:
                        break;
                    default:
                        if (form.WindowState == FormWindowState.Maximized)
                        {
                            DpiCompatible(dpi, control);
                            return;
                        }
                        DpiLS(dpi, form, DpiInfo(form.Controls));
                        break;
                }
            }
            else DpiLS(dpi, DpiInfo(control));
        }
        public static void DpiChangeAuto(float dpi, float dpiold, Control control)
        {
            if (dpi == dpiold) return;
            var revert_dpi = 1F / dpiold;
            if (Config.DpiMode == DpiMode.Compatible)
            {
                DpiCompatible(revert_dpi, control);
                DpiCompatible(dpi, control);
                return;
            }
            if (control is Form form)
            {
                switch (form.AutoScaleMode)
                {
                    case AutoScaleMode.Font:
                    case AutoScaleMode.Dpi:
                        break;
                    default:
                        DpiCompatible(revert_dpi, control);
                        DpiCompatible(dpi, control);
                        break;
                }
            }
            else DpiLS(dpi, DpiInfo(control));
        }

        public static void DpiCompatible(float dpi, Control control) => control.Scale(new SizeF(dpi, dpi));

        static Dictionary<Control, AnchorDock> DpiInfo(Control control)
        {
            var dir = new Dictionary<Control, AnchorDock>(control.Controls.Count + 1) {
                {control, new AnchorDock(control) }
            };
            foreach (Control item in control.Controls)
            {
                dir.Add(item, new AnchorDock(item));
                if (item.Controls.Count > 0) DpiInfo(ref dir, item.Controls);
            }
            return dir;
        }
        static Dictionary<Control, AnchorDock> DpiInfo(Control.ControlCollection controls)
        {
            var dir = new Dictionary<Control, AnchorDock>(controls.Count);
            foreach (Control control in controls)
            {
                dir.Add(control, new AnchorDock(control));
                if (control.Controls.Count > 0) DpiInfo(ref dir, control.Controls);
            }
            return dir;
        }
        static void DpiInfo(ref Dictionary<Control, AnchorDock> dir, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                dir.Add(control, new AnchorDock(control));
                if (controls.Count > 0) DpiInfo(ref dir, control.Controls);
            }
        }

        static void DpiLS(float dpi, Form form, Dictionary<Control, AnchorDock> info)
        {
            if (form is Window window)
            {
                DpiForm(dpi, window, window.sizeInit ?? window.ClientSize, out var point, out var size);
                Size max = window.MaximumSize, min = window.MinimumSize;
                window.MaximumSize = window.MinimumSize = window.ClientSize = size;
                window.Location = point;
                window.MinimumSize = min;
                window.MaximumSize = max;
            }
            else
            {
                DpiForm(dpi, form, form.ClientSize, out var point, out var size);
                form.ClientSize = size;
                form.Location = point;
            }

            DpiLS(dpi, info);
        }
        static void DpiLS(float dpi, Dictionary<Control, AnchorDock> info)
        {
            foreach (var item in info)
            {
                var control = item.Key;
                if (!control.MaximumSize.IsEmpty) control.MaximumSize = new Size((int)(control.MaximumSize.Width * dpi), (int)(control.MaximumSize.Height * dpi));
                if (!control.MinimumSize.IsEmpty) control.MinimumSize = new Size((int)(control.MinimumSize.Width * dpi), (int)(control.MinimumSize.Height * dpi));
                control.Padding = SetPadding(dpi, control.Padding);
                control.Margin = SetPadding(dpi, control.Margin);
                control.Size = new Size((int)(item.Value.Rect.Width * dpi), (int)(item.Value.Rect.Height * dpi));
                control.Location = new Point((int)(item.Value.Rect.X * dpi), (int)(item.Value.Rect.Y * dpi));
                if (control is TableLayoutPanel tableLayout)
                {
                    foreach (ColumnStyle it in tableLayout.ColumnStyles)
                    {
                        if (it.SizeType == SizeType.Absolute) it.Width *= dpi;
                    }
                    foreach (RowStyle it in tableLayout.RowStyles)
                    {
                        if (it.SizeType == SizeType.Absolute) it.Height *= dpi;
                    }
                }
                else if (control is TabControl tab && tab.ItemSize.Width > 1 && tab.ItemSize.Height > 1) tab.ItemSize = new Size((int)(tab.ItemSize.Width * dpi), (int)(tab.ItemSize.Height * dpi));
                else if (control is SplitContainer splitContainer)
                {
                    splitContainer.SplitterWidth = (int)(splitContainer.SplitterWidth * dpi);
                    if (splitContainer.Panel1MinSize > 0) splitContainer.Panel1MinSize = (int)(splitContainer.Panel1MinSize * dpi);
                    if (splitContainer.Panel2MinSize > 0) splitContainer.Panel2MinSize = (int)(splitContainer.Panel2MinSize * dpi);
                }
            }
        }

        static void DpiForm(float dpi, Form form, Size csize, out Point point, out Size size)
        {
            size = new Size((int)(csize.Width * dpi), (int)(csize.Height * dpi));
            var screen = Screen.FromPoint(form.Location).WorkingArea;
            if (size.Width > screen.Width && size.Height > screen.Height)
            {
                if (csize.Width > screen.Width && csize.Height > screen.Height)
                {
                    size = screen.Size;
                    point = screen.Location;
                }
                else
                {
                    size = csize;
                    point = form.Location;
                }
            }
            else
            {
                if (size.Width > screen.Width) size.Width = screen.Width;
                if (size.Height > screen.Height) size.Height = screen.Height;
                point = new Point(form.Left + (csize.Width - size.Width) / 2, form.Top + (csize.Height - size.Height) / 2);
                if (point.X < 0 || point.Y < 0) point = form.Location;
            }
            if (form.StartPosition == FormStartPosition.CenterScreen) point = new Point(screen.X + (screen.Width - size.Width) / 2, screen.Y + (screen.Height - size.Height) / 2);
            if (!form.MaximumSize.IsEmpty) form.MaximumSize = new Size((int)(form.MaximumSize.Width * dpi), (int)(form.MaximumSize.Height * dpi));
            if (!form.MinimumSize.IsEmpty) form.MinimumSize = new Size((int)(form.MinimumSize.Width * dpi), (int)(form.MinimumSize.Height * dpi));
            form.Padding = SetPadding(dpi, form.Padding);
            form.Margin = SetPadding(dpi, form.Margin);
        }

        internal static Padding SetPadding(float dpi, Padding padding)
        {
            if (padding.All == 0) return padding;
            else if (padding.All > 0) return new Padding((int)(padding.All * dpi));
            else return new Padding((int)(padding.Left * dpi), (int)(padding.Top * dpi), (int)(padding.Right * dpi), (int)(padding.Bottom * dpi));
        }

        internal static void ControlEvent(this Control control)
        {
            if (control is GridPanel gridpanel) gridpanel.IOnSizeChanged();
            else if (control is FlowPanel flowpanel) flowpanel.IOnSizeChanged();
            else if (control is StackPanel stackpanel) stackpanel.IOnSizeChanged();
            foreach (Control it in control.Controls) ControlEvent(it);
        }

        #region Win32

        public static float GetScreenDpi(Control control)
        {
            var targetScreen = Screen.FromPoint(Control.MousePosition); // 根据坐标找到对应屏幕
            var rawDpi = GetScreenDpiByApi(targetScreen);
            if (rawDpi.HasValue) return rawDpi.Value;
#if NET40 || NET46 || NET48
            return Config.Dpi;
#else
            return control.DeviceDpi / 96F;
#endif
        }

        /// <summary>
        /// 通过API获取指定屏幕的DPI（无临时窗体，高效）
        /// </summary>
        /// <param name="targetScreen">目标屏幕</param>
        /// <param name="dpiType">DPI类型（默认有效DPI）</param>
        /// <returns>指定屏幕的DPI值（X/Y通常相等）</returns>
        public static float? GetScreenDpiByApi(Screen targetScreen, Win32.MonitorDpiType dpiType = Win32.MonitorDpiType.MDT_Effective_DPI)
        {
            // Win8.1以下系统（Build < 9600）不支持该API，返回默认96DPI
            if (OS.Version.Build < 9600) return null;
            // 稳妥获取屏幕句柄hMonitor（避免依赖Screen.HashCode的内部实现）
            Rectangle screenRect = targetScreen.Bounds;
            var hMonitor = Win32.MonitorFromRect(ref screenRect, Win32.MONITOR_DEFAULTTONEAREST);
            if (Win32.GetDpiForMonitor(hMonitor, dpiType, out uint dpiX, out uint dpiY) == 0) return GetDpi(dpiX, dpiY);
            return null;
        }

        public static float GetDpi(uint dpiX, uint dpiY) => Math.Max(dpiX, dpiY) / 96F;
        public static float GetDpi(int dpiX, int dpiY) => Math.Max(dpiX, dpiY) / 96F;
        public static float GetDpi(float dpiX, float dpiY) => Math.Max(dpiX, dpiY) / 96F;

        #endregion
    }
}