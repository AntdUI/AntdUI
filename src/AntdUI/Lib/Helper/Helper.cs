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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public static partial class Helper
    {
        public static float Calculate(this float val, float add) => (float)Math.Round(val + add, 3);

        /// <summary>
        /// SizeF转Size（向上取整）
        /// </summary>
        /// <param name="size">SizeF</param>
        public static Size Size(this SizeF size) => new Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));

        /// <summary>
        /// SizeF转Size（向上取整）
        /// </summary>
        /// <param name="size">SizeF</param>
        public static Size Size(this SizeF size, float p) => new Size((int)Math.Ceiling(size.Width + p), (int)Math.Ceiling(size.Height + p));

        public static Size Size(this Size size, int p)
        {
            if (p > 0)
            {
                var s = (int)(p * Config.Dpi);
                size.Width += s;
                size.Height += s;
            }
            return size;
        }

        public static Size Size(this Size size, int w, int h)
        {
            if (w > 0) size.Width += (int)(w * Config.Dpi);
            if (h > 0) size.Height += (int)(h * Config.Dpi);
            return size;
        }

        public static Size SizeEm(this Size size, Font font) => new Size(size.Width + (int)Math.Round(size.Width * 0.135f), size.Height + (int)Math.Round(font.Size * 0.17f));

        public static Color ToColor(float alpha, Color color) => ToColor((int)alpha, color);

        public static Color ToColorN(float val, Color color) => ToColor((int)(val * color.A), color);

        public static Color ToColor(int alpha, Color color) => Color.FromArgb(Style.rgbbyte(alpha), color);

        public static Form? FindPARENT(this Control? control, bool mdi = false)
        {
            if (control == null) return null;
            if (control is DoubleBufferForm formd)
            {
                if (formd.Tag is Form form) return form;
                else if (formd.Parent != null) return FindPARENT(formd.Parent, mdi);
                return formd;
            }
            else if (control is Form form)
            {
                if (mdi) return form;
                return form.ParentForm ?? form;
            }
            else if (control.Parent != null) return FindPARENT(control.Parent, mdi);
            return null;
        }
        public static Form? FindPARENT(this Target? target, bool mdi = false)
        {
            if (target == null) return null;
            if (target.Value is DoubleBufferForm formd)
            {
                if (formd.Tag is Form form) return form;
                else if (formd.Parent != null) return FindPARENT(formd.Parent, mdi);
                return formd;
            }
            else if (target.Value is Form form)
            {
                if (mdi) return form;
                return form.ParentForm ?? form;
            }
            else if (target.Value is Control control) return FindPARENT(control.Parent, mdi);
            return null;
        }

        public static List<Control> FindPARENTs(this Control control, bool mdi = false)
        {
            var list = new List<Control>(2);
            if (control is DoubleBufferForm formd)
            {
                list.Add(formd);
                if (formd.Tag is Form form)
                {
                    list.Add(form);
                    return list;
                }
                else if (formd.Parent != null)
                {
                    var tmp = FindPARENTs(formd.Parent, mdi);
                    if (tmp != null) list.AddRange(tmp);
                    return list;
                }
                else return list;
            }
            else if (control is Form form)
            {
                if (mdi) list.Add(form);
                else list.Add(form.ParentForm ?? form);
                return list;
            }
            else if (control.Parent != null)
            {
                list.Add(control);
                var tmp = FindPARENTs(control.Parent, mdi);
                if (tmp != null) list.AddRange(tmp);
            }
            else list.Add(control);
            return list;
        }

        public static bool SetTopMost(this Control? control, IntPtr hand)
        {
            var form = control.FindPARENT();
            if (form != null && form.TopMost || (form is LayeredFormPopover layered && layered.topMost) || (form is LayeredFormDrawer layeredDrawer && layeredDrawer.topMost) || (form is LayeredFormTour layeredTour && layeredTour.topMost))
            {
                SetTopMost(hand);
                return true;
            }
            return false;
        }
        public static bool SetTopMost(this Target? target, IntPtr hand)
        {
            var form = target.FindPARENT();
            if (form != null && form.TopMost || (form is LayeredFormPopover layered && layered.topMost) || (form is LayeredFormDrawer layeredDrawer && layeredDrawer.topMost) || (form is LayeredFormTour layeredTour && layeredTour.topMost))
            {
                SetTopMost(hand);
                return true;
            }
            return false;
        }

        public static void SetTopMost(IntPtr hand) => Vanara.PInvoke.User32.SetWindowPos(hand, new IntPtr(-1), 0, 0, 0, 0, Vanara.PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);

        public static bool AreDateTimeArraysEqual(DateTime[]? array1, DateTime[]? array2)
        {
            // 两个都为null，视为相等
            if (array1 == null && array2 == null) return true;

            // 其中一个为null，另一个不为null，视为不相等
            if (array1 == null || array2 == null) return false;

            // 长度不同，视为不相等
            if (array1.Length != array2.Length) return false;

            // 逐个比较元素
            for (int i = 0; i < array1.Length; i++)
            {
                if (!array1[i].Equals(array2[i])) return false;
            }

            // 所有元素都相等
            return true;
        }

        public static bool Wait(this System.Threading.WaitHandle? handle, bool close = true)
        {
            if (handle == null) return true;
            try
            {
                if (handle.SafeWaitHandle.IsClosed) return close;
                handle.WaitOne();
                if (handle.SafeWaitHandle.IsClosed) return close;
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static bool Wait(this System.Threading.WaitHandle? handle, int timeout, bool close = true)
        {
            if (handle == null) return true;
            try
            {
                handle.WaitOne(timeout);
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

        public static bool DateExceedMonth(DateTime date, int num, DateTime? min, DateTime? max)
        {
            try
            {
                return DateExceedMonth(date.AddMonths(num), min, max);
            }
            catch { return false; }
        }
        public static bool DateExceedMonth(DateTime date, DateTime? min, DateTime? max)
        {
            // 检查目标月份是否早于minDate所在的月份
            if (min.HasValue)
            {
                // 如果目标月份比minDate的月份还早，禁用
                if (date.Year < min.Value.Year || (date.Year == min.Value.Year && date.Month < min.Value.Month)) return false;
            }
            // 检查目标月份是否晚于maxDate所在的月份
            if (max.HasValue)
            {
                // 如果目标月份比maxDate的月份还晚，禁用
                if (date.Year > max.Value.Year || (date.Year == max.Value.Year && date.Month > max.Value.Month)) return false;
            }
            // 目标月份在允许范围内
            return true;
        }

        public static bool DateExceedYear(DateTime date, int num, DateTime? min, DateTime? max)
        {
            try
            {
                return DateExceedYear(date.AddYears(num), min, max);
            }
            catch { return false; }
        }
        public static bool DateExceedYear(DateTime date, DateTime? min, DateTime? max)
        {
            if (min.HasValue && min.Value >= date) return false;
            if (max.HasValue)
            {
                if (max.Value.Year == date.Year) return true;
                if (max.Value <= date) return false;
            }
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
            if (control.InvokeRequired) return ITask.Invoke(control, new Func<string?>(() => ClipboardGetText()));
            return ClipboardGetText();
        }
        public static string? ClipboardGetText()
        {
            if (Win32.GetClipBoardText(out var text)) return text;
            else
            {
                try
                {
                    return Clipboard.GetText();
                }
                catch
                {
                    return null;
                }
            }
        }
        public static bool ClipboardSetText(this Control control, string? text)
        {
            if (control.InvokeRequired) return ITask.Invoke(control, new Func<bool>(() => ClipboardSetText(text)));
            return ClipboardSetText(text);
        }
        public static bool ClipboardSetText(string? text)
        {
            if (Win32.SetClipBoardText(text)) return true;
            else
            {
                try
                {
                    if (text == null) Clipboard.Clear();
                    else Clipboard.SetText(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        public static bool IsAdmin()
        {
            using (var id = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                return new System.Security.Principal.WindowsPrincipal(id).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// 判断文本是否包含拼音
        /// </summary>
        /// <param name="search">搜索文字</param>
        /// <param name="text">全文本</param>
        /// <param name="py">拼音</param>
        /// <param name="select">是否需要选中</param>
        /// <returns>返回权重</returns>
        public static int SearchContains(string search, string text, string[] py, out bool select)
        {
            select = false;
            int gear = py.Length, score = 0;
            if (text == search)
            {
                select = true;
                score += gear * 10;
            }
            search = search.ToLower();
            if (text == search)
            {
                select = true;
                score += gear * 8;
            }
            foreach (var pinyin in py)
            {
                if (pinyin == search)
                {
                    select = true;
                    score += gear * 3;
                }
                else if (pinyin.StartsWith(search)) score += gear * 2;
                else if (pinyin.Contains(search)) score += gear;
                gear--;
            }
            return score;
        }

        public static List<object>? SearchWeightSort(this List<iItemSearchWeigth> list)
        {
            if (list.Count > 0)
            {
                list.Sort((x, y) => -x.Weight.CompareTo(y.Weight));
                var result = new List<object>(list.Count);
                foreach (var it in list) result.Add(it.Value);
                return result;
            }
            return null;
        }
        public static List<T>? SearchWeightSort<T>(this List<iItemSearchWeigth> list)
        {
            if (list.Count > 0)
            {
                list.Sort((x, y) => -x.Weight.CompareTo(y.Weight));
                var result = new List<T>(list.Count);
                foreach (var it in list)
                {
                    if (it.Value is T n) result.Add(n);
                }
                return result;
            }
            return null;
        }
        public static List<T>? SearchWeightSort<T>(this List<ItemSearchWeigth<T>> list)
        {
            if (list.Count > 0)
            {
                list.Sort((x, y) => -x.Weight.CompareTo(y.Weight));
                var result = new List<T>(list.Count);
                foreach (var it in list) result.Add(it.Value);
                return result;
            }
            return null;
        }
        public static object[]? SearchWeightSortArray(this List<iItemSearchWeigth> list)
        {
            if (list.Count > 0)
            {
                list.Sort((x, y) => -x.Weight.CompareTo(y.Weight));
                var result = new List<object>(list.Count);
                foreach (var it in list) result.Add(it.Value);
                return result.ToArray();
            }
            return null;
        }
        public static T[]? SearchWeightSortArray<T>(this List<iItemSearchWeigth> list)
        {
            if (list.Count > 0)
            {
                list.Sort((x, y) => -x.Weight.CompareTo(y.Weight));
                var result = new List<T>(list.Count);
                foreach (var it in list)
                {
                    if (it.Value is T n) result.Add(n);
                }
                return result.ToArray();
            }
            return null;
        }
        public static T[]? SearchWeightSortArray<T>(this List<ItemSearchWeigth<T>> list)
        {
            if (list.Count > 0)
            {
                list.Sort((x, y) => -x.Weight.CompareTo(y.Weight));
                var result = new List<T>(list.Count);
                foreach (var it in list) result.Add(it.Value);
                return result.ToArray();
            }
            return null;
        }
    }

    internal class AnchorDock
    {
        public AnchorDock(Control control)
        {
            Dock = control.Dock;
            Anchor = control.Anchor;
            Rect = new Rectangle(control.Left, control.Top, control.Width, control.Height);
        }
        public DockStyle Dock { get; set; }
        public AnchorStyles Anchor { get; set; }
        public Rectangle Rect { get; set; }
    }

    public class RectTextLR
    {
        public Rectangle text { get; set; }
        public Rectangle l { get; set; }
        public Rectangle r { get; set; }
    }
}