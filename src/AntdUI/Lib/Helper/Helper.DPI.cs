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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        internal static void DpiAuto(float dpi, Control control)
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
                control.Scale(new SizeF(dpi, dpi));
                DpiLS(dpi, control.Controls);
                return;
            }
            if (control is Form form)
            {
                if (form.WindowState == FormWindowState.Maximized)
                {
                    form.Scale(new SizeF(dpi, dpi));
                    return;
                }
                DpiLS(dpi, form, DpiInfo(form.Controls));
            }
            else DpiLS(dpi, DpiInfo(control));
        }
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
                else if (control is Panel panel) panel.padding = SetPadding(dpi, panel.padding);
                else if (control is HyperlinkLabel hyperlink) hyperlink.LinkPadding = SetPadding(dpi, hyperlink.LinkPadding);
                else if (control is TabHeader tabHeader)
                {
                    if (tabHeader.RightGap > 0) tabHeader.RightGap = (int)(tabHeader.RightGap * dpi);
                }
            }
        }
        static void DpiLS(float dpi, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is Panel panel) panel.padding = SetPadding(dpi, panel.padding);
                else if (control is HyperlinkLabel hyperlink) hyperlink.LinkPadding = SetPadding(dpi, hyperlink.LinkPadding);
                else if (control is TabHeader tabHeader)
                {
                    if (tabHeader.RightGap > 0) tabHeader.RightGap = (int)(tabHeader.RightGap * dpi);
                }
                if (controls.Count > 0) DpiLS(dpi, control.Controls);
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
    }
}