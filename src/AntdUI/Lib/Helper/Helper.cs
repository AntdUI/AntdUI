// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public static partial class Helper
    {
        /// <summary>
        /// 计算两个浮点数的和，并保留3位小数
        /// </summary>
        /// <param name="val">第一个浮点数</param>
        /// <param name="add">要添加的浮点数</param>
        /// <returns>计算结果</returns>
        public static float Calculate(this float val, float add) => (float)Math.Round(val + add, 3);

        /// <summary>
        /// SizeF转Size（向上取整）
        /// </summary>
        /// <param name="size">SizeF</param>
        public static Size Size(this SizeF size) => new Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));

        /// <summary>
        /// SizeF转Size（向上取整），并添加偏移量
        /// </summary>
        /// <param name="size">SizeF</param>
        /// <param name="p">要添加的偏移量</param>
        public static Size Size(this SizeF size, float p) => new Size((int)Math.Ceiling(size.Width + p), (int)Math.Ceiling(size.Height + p));

        /// <summary>
        /// 调整Size大小，根据DPI缩放因子添加偏移量
        /// </summary>
        /// <param name="size">原始Size</param>
        /// <param name="p">要添加的偏移量（像素单位）</param>
        /// <returns>调整后的Size</returns>
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

        /// <summary>
        /// 调整Size大小，根据Canvas的DPI缩放因子添加偏移量
        /// </summary>
        /// <param name="size">原始Size</param>
        /// <param name="g">Canvas对象</param>
        /// <param name="p">要添加的偏移量（像素单位）</param>
        /// <returns>调整后的Size</returns>
        public static Size Size(this Size size, Canvas g, int p)
        {
            if (p > 0)
            {
                var s = (int)(p * g.Dpi);
                size.Width += s;
                size.Height += s;
            }
            return size;
        }

        /// <summary>
        /// 调整Size大小，根据DPI缩放因子分别添加宽度和高度偏移量
        /// </summary>
        /// <param name="size">原始Size</param>
        /// <param name="w">要添加的宽度偏移量（像素单位）</param>
        /// <param name="h">要添加的高度偏移量（像素单位）</param>
        /// <returns>调整后的Size</returns>
        public static Size Size(this Size size, int w, int h)
        {
            if (w > 0) size.Width += (int)(w * Config.Dpi);
            if (h > 0) size.Height += (int)(h * Config.Dpi);
            return size;
        }

        /// <summary>
        /// 调整Size大小，根据Canvas的DPI缩放因子分别添加宽度和高度偏移量
        /// </summary>
        /// <param name="size">原始Size</param>
        /// <param name="g">Canvas对象</param>
        /// <param name="w">要添加的宽度偏移量（像素单位）</param>
        /// <param name="h">要添加的高度偏移量（像素单位）</param>
        /// <returns>调整后的Size</returns>
        public static Size Size(this Size size, Canvas g, int w, int h)
        {
            if (w > 0) size.Width += (int)(w * g.Dpi);
            if (h > 0) size.Height += (int)(h * g.Dpi);
            return size;
        }

        /// <summary>
        /// 根据字体大小调整Size，用于计算文本的最佳显示大小
        /// </summary>
        /// <param name="size">原始Size</param>
        /// <param name="font">字体对象</param>
        /// <returns>调整后的Size</returns>
        public static Size SizeEm(this Size size, Font font) => new Size(size.Width + (int)Math.Round(size.Width * 0.135f), size.Height + (int)Math.Round(font.Size * 0.17f));

        /// <summary>
        /// 根据浮点数alpha值创建带透明度的颜色
        /// </summary>
        /// <param name="alpha">透明度值（0-255）</param>
        /// <param name="color">原始颜色</param>
        /// <returns>带透明度的颜色</returns>
        public static Color ToColor(float alpha, Color color) => ToColor((int)alpha, color);

        /// <summary>
        /// 根据归一化的alpha值（0-1）创建带透明度的颜色
        /// </summary>
        /// <param name="val">归一化的透明度值（0-1）</param>
        /// <param name="color">原始颜色</param>
        /// <returns>带透明度的颜色</returns>
        public static Color ToColorN(float val, Color color) => ToColor((int)(val * color.A), color);

        /// <summary>
        /// 根据整数alpha值创建带透明度的颜色
        /// </summary>
        /// <param name="alpha">透明度值（0-255）</param>
        /// <param name="color">原始颜色</param>
        /// <returns>带透明度的颜色</returns>
        public static Color ToColor(int alpha, Color color) => Color.FromArgb(Style.rgbbyte(alpha), color);

        /// <summary>
        /// 查找控件的父级Form
        /// </summary>
        /// <param name="control">要查找的控件</param>
        /// <param name="mdi">是否查找MDI父窗口</param>
        /// <returns>找到的Form，找不到则返回null</returns>
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

        /// <summary>
        /// 查找Target的父级Form
        /// </summary>
        /// <param name="target">要查找的Target</param>
        /// <param name="mdi">是否查找MDI父窗口</param>
        /// <returns>找到的Form，找不到则返回null</returns>
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

        /// <summary>
        /// 查找控件的所有父级Form
        /// </summary>
        /// <param name="control">要查找的控件</param>
        /// <param name="mdi">是否查找MDI父窗口</param>
        /// <returns>父级Form列表</returns>
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

        /// <summary>
        /// 根据控件的TopMost属性设置窗口置顶
        /// </summary>
        /// <param name="control">参考控件</param>
        /// <param name="hand">要设置的窗口句柄</param>
        /// <returns>是否设置成功</returns>
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

        /// <summary>
        /// 根据Target的TopMost属性设置窗口置顶
        /// </summary>
        /// <param name="target">参考Target</param>
        /// <param name="hand">要设置的窗口句柄</param>
        /// <returns>是否设置成功</returns>
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

        /// <summary>
        /// 设置窗口置顶
        /// </summary>
        /// <param name="hand">要设置的窗口句柄</param>
        public static void SetTopMost(IntPtr hand) => Vanara.PInvoke.User32.SetWindowPos(hand, new IntPtr(-1), 0, 0, 0, 0, Vanara.PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);

        /// <summary>
        /// 等待WaitHandle信号
        /// </summary>
        /// <param name="handle">WaitHandle对象</param>
        /// <param name="close">如果句柄已关闭是否返回true</param>
        /// <returns>true表示已关闭或出错，false表示等待成功</returns>
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

        /// <summary>
        /// 等待WaitHandle信号，带超时时间
        /// </summary>
        /// <param name="handle">WaitHandle对象</param>
        /// <param name="timeout">超时时间（毫秒）</param>
        /// <param name="close">如果句柄已关闭是否返回true</param>
        /// <returns>true表示已关闭或出错，false表示等待成功</returns>
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

        /// <summary>
        /// 设置EventWaitHandle信号
        /// </summary>
        /// <param name="handle">EventWaitHandle对象</param>
        /// <returns>true表示已关闭或出错，false表示设置成功</returns>
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

        /// <summary>
        /// 重置EventWaitHandle信号
        /// </summary>
        /// <param name="handle">EventWaitHandle对象</param>
        /// <returns>true表示已关闭或出错，false表示重置成功</returns>
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

        /// <summary>
        /// 等待并释放EventWaitHandle
        /// </summary>
        /// <param name="handle">EventWaitHandle对象</param>
        /// <param name="set">释放前是否设置信号</param>
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

        /// <summary>
        /// 检查CancellationTokenSource是否已取消
        /// </summary>
        /// <param name="token">CancellationTokenSource对象</param>
        /// <returns>true表示已取消或出错，false表示正常</returns>
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

        /// <summary>
        /// 检查CancellationTokenSource是否已取消或控件是否已释放
        /// </summary>
        /// <param name="token">CancellationTokenSource对象</param>
        /// <param name="control">要检查的控件</param>
        /// <returns>true表示已取消、已释放或出错，false表示正常</returns>
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

        /// <summary>
        /// 检查列表索引是否越界
        /// </summary>
        /// <param name="list">要检查的列表</param>
        /// <param name="index">要检查的索引</param>
        /// <returns>true表示越界，false表示正常</returns>
        public static bool ListExceed(this IList? list, int index)
        {
            if (list == null || list.Count <= index || index < 0) return true;
            return false;
        }

#if NET40 || NET46 || NET48
        public static bool IsNull(this object? value)
#else
        public static bool IsNull([System.Diagnostics.CodeAnalysis.NotNullWhen(false)] this object? value)
#endif
        {
            if (value == null || value is DBNull) return true;
            return false;
        }

        #region 剪贴板

        /// <summary>
        /// 获取剪贴板文本（线程安全）
        /// </summary>
        /// <param name="control">用于调用Invoke的控件</param>
        /// <returns>剪贴板文本，获取失败返回null</returns>
        public static string? ClipboardGetText(this Control control)
        {
            if (control.InvokeRequired) return ITask.Invoke(control, new Func<string?>(() => ClipboardGetText()));
            return ClipboardGetText();
        }

        /// <summary>
        /// 获取剪贴板文本
        /// </summary>
        /// <returns>剪贴板文本，获取失败返回null</returns>
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

        /// <summary>
        /// 设置剪贴板文本（线程安全）
        /// </summary>
        /// <param name="control">用于调用Invoke的控件</param>
        /// <param name="text">要设置的文本，null表示清空剪贴板</param>
        /// <returns>是否设置成功</returns>
        public static bool ClipboardSetText(this Control control, string? text)
        {
            if (control.InvokeRequired) return ITask.Invoke(control, new Func<bool>(() => ClipboardSetText(text)));
            return ClipboardSetText(text);
        }

        /// <summary>
        /// 设置剪贴板文本
        /// </summary>
        /// <param name="text">要设置的文本，null表示清空剪贴板</param>
        /// <returns>是否设置成功</returns>
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

        /// <summary>
        /// 检查当前进程是否以管理员身份运行
        /// </summary>
        /// <returns>true表示以管理员身份运行，false表示不是</returns>
        public static bool IsAdmin()
        {
            using (var id = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                return new System.Security.Principal.WindowsPrincipal(id).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// 判断文本是否包含搜索词，支持拼音搜索，并返回匹配权重
        /// </summary>
        /// <param name="search">搜索文字</param>
        /// <param name="text">全文本</param>
        /// <param name="py">拼音数组</param>
        /// <param name="select">是否需要选中</param>
        /// <returns>匹配权重，值越大匹配度越高</returns>
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

        /// <summary>
        /// 根据权重对搜索结果进行排序
        /// </summary>
        /// <param name="list">搜索结果列表</param>
        /// <returns>排序后的对象列表</returns>
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

        /// <summary>
        /// 根据权重对搜索结果进行排序，并转换为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="list">搜索结果列表</param>
        /// <returns>排序后的指定类型列表</returns>
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

        /// <summary>
        /// 根据权重对泛型搜索结果进行排序
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="list">泛型搜索结果列表</param>
        /// <returns>排序后的指定类型列表</returns>
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

        /// <summary>
        /// 根据权重对搜索结果进行排序，并转换为对象数组
        /// </summary>
        /// <param name="list">搜索结果列表</param>
        /// <returns>排序后的对象数组</returns>
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

        /// <summary>
        /// 根据权重对搜索结果进行排序，并转换为指定类型数组
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="list">搜索结果列表</param>
        /// <returns>排序后的指定类型数组</returns>
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

        /// <summary>
        /// 根据权重对泛型搜索结果进行排序，并转换为指定类型数组
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="list">泛型搜索结果列表</param>
        /// <returns>排序后的指定类型数组</returns>
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

        /// <summary>
        /// 判断指定坐标是否在三角形内
        /// </summary>
        /// <param name="x">鼠标X坐标</param>
        /// <param name="y">鼠标Y坐标</param>
        /// <param name="trianglePoints">三角形的三个顶点（必须包含3个Point）</param>
        /// <returns>true=在内部，false=在外部</returns>
        public static bool IsPointInTriangle(int x, int y, Point[] trianglePoints)
        {
            // 校验参数：必须传入3个顶点
            Point A = trianglePoints[0], B = trianglePoints[1], C = trianglePoints[2];

            // 计算三个叉乘结果（判断P在各边的哪一侧）
            float cross1 = CrossProduct(B.X - A.X, B.Y - A.Y, x - A.X, y - A.Y), cross2 = CrossProduct(C.X - B.X, C.Y - B.Y, x - B.X, y - B.Y), cross3 = CrossProduct(A.X - C.X, A.Y - C.Y, x - C.X, y - C.Y);

            // 判断是否全部同向（都为正 或 都为负，包含0则在边上）
            bool isSameSign = (cross1 >= 0 && cross2 >= 0 && cross3 >= 0) || (cross1 <= 0 && cross2 <= 0 && cross3 <= 0);

            return isSameSign;
        }

        /// <summary>
        /// 计算二维向量叉乘（仅返回Z轴分量，代表方向）
        /// </summary>
        /// <param name="x1">向量1 X</param>
        /// <param name="y1">向量1 Y</param>
        /// <param name="x2">向量2 X</param>
        /// <param name="y2">向量2 Y</param>
        /// <returns>叉乘结果</returns>
        private static float CrossProduct(int x1, int y1, int x2, int y2) => (x1 * (float)y2) - (y1 * (float)x2);
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