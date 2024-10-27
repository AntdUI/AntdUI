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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public static partial class Helper
    {
        public static float Calculate(this float val, float add)
        {
            return (float)Math.Round(val + add, 3);
        }

        /// <summary>
        /// SizeF转Size（向上取整）
        /// </summary>
        /// <param name="size">SizeF</param>
        public static Size Size(this SizeF size)
        {
            return new Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
        }

        /// <summary>
        /// SizeF转Size（向上取整）
        /// </summary>
        /// <param name="size">SizeF</param>
        public static Size Size(this SizeF size, float p)
        {
            return new Size((int)Math.Ceiling(size.Width + p), (int)Math.Ceiling(size.Height + p));
        }

        public static Color ToColor(float alpha, Color color)
        {
            return ToColor((int)alpha, color);
        }

        public static Color ToColorN(float val, Color color)
        {
            return ToColor((int)(val * color.A), color);
        }

        public static Color ToColor(int alpha, Color color)
        {
            if (alpha > 255) alpha = 255;
            else if (alpha < 0) alpha = 0;
            return Color.FromArgb(alpha, color);
        }

        public static Form? FindPARENT(this Control? control)
        {
            if (control == null) return null;
            if (control is DoubleBufferForm formd)
            {
                if (control.Tag is Form form) return form;
                else if (control.Parent != null) return FindPARENT(control.Parent);
                return formd;
            }
            else if (control is Form form) return form;
            else if (control.Parent != null) return FindPARENT(control.Parent);
            return null;
        }
        public static bool SetTopMost(this Control? control, IntPtr hand)
        {
            var form = control.FindPARENT();
            if (form != null && form.TopMost)
            {
                SetTopMost(hand);
                return true;
            }
            return false;
        }

        public static void SetTopMost(IntPtr hand)
        {
            Vanara.PInvoke.User32.SetWindowPos(hand, new IntPtr(-1), 0, 0, 0, 0, Vanara.PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);
        }

        public static bool Wait(this System.Threading.WaitHandle? handle, bool close = true)
        {
            if (handle == null) return true;
            try
            {
                handle.WaitOne();
                if (handle.SafeWaitHandle.IsClosed) return close;
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static bool SetWait(this System.Threading.EventWaitHandle? handle)
        {
            if (handle == null) return true;
            try
            {
                if (handle.SafeWaitHandle.IsClosed) return true;
                handle.Set();
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static bool ResetWait(this System.Threading.EventWaitHandle? handle)
        {
            if (handle == null) return true;
            try
            {
                if (handle.SafeWaitHandle.IsClosed) return true;
                handle.Reset();
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static void WaitDispose(this System.Threading.EventWaitHandle? handle, bool set = true)
        {
            if (handle == null) return;
            try
            {
                if (handle.SafeWaitHandle.IsClosed) return;
                if (set) handle.SetWait();
                else handle.ResetWait();
                handle.Dispose();
            }
            catch
            {
            }
        }

        public static bool Wait(this System.Threading.CancellationTokenSource? token)
        {
            try
            {
                if (token == null || token.IsCancellationRequested) return true;
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static bool Wait(this System.Threading.CancellationTokenSource? token, Control control)
        {
            try
            {
                if (token == null || token.IsCancellationRequested || control.IsDisposed) return true;
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static bool ListExceed(this IList? list, int index)
        {
            if (list == null || list.Count <= index || index < 0) return true;
            return false;
        }

        public static bool DateExceed(DateTime date, DateTime? min, DateTime? max)
        {
            if (min.HasValue && min.Value >= date) return false;
            if (max.HasValue && max.Value <= date) return false;
            return true;
        }

        public static bool DateExceedRelax(DateTime date, DateTime? min, DateTime? max)
        {
            if (min.HasValue && min.Value > date) return false;
            if (max.HasValue && max.Value < date) return false;
            return true;
        }

        #region 剪贴板

        public static string? ClipboardGetText(this Control control)
        {
            if (control.InvokeRequired)
            {
                string? r = null;
                control.Invoke(new Action(() =>
                {
                    r = ClipboardGetText();
                }));
                return r;
            }
            return ClipboardGetText();
        }
        public static string? ClipboardGetText()
        {
            try
            {
                return Win32.GetClipBoardText();
            }
            catch
            {
                return Clipboard.GetText();
            }
        }
        public static bool ClipboardSetText(this Control control, string? text)
        {
            if (control.InvokeRequired)
            {
                bool r = false;
                control.Invoke(new Action(() =>
                {
                    r = ClipboardSetText(text);
                }));
                return r;
            }
            return ClipboardSetText(text);
        }
        public static bool ClipboardSetText(string? text)
        {
            try
            {
                if (Win32.SetClipBoardText(text)) return true;
            }
            catch
            {
                if (text == null) Clipboard.Clear();
                else Clipboard.SetText(text);
                return true;
            }
            return false;
        }

        #endregion
    }

    internal class AnchorDock
    {
        public AnchorDock(Control control)
        {
            Dock = control.Dock;
            Anchor = control.Anchor;
            control.Dock = DockStyle.None;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        }
        public DockStyle Dock { get; set; }
        public AnchorStyles Anchor { get; set; }
    }

    public class RectTextLR
    {
        public Rectangle text { get; set; }
        public Rectangle l { get; set; }
        public Rectangle r { get; set; }
    }
}