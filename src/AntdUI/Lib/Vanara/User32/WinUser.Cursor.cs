// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
    public static partial class User32
    {
        /// <summary>
        /// 从指定的模块加载光标资源，或加载系统预定义的光标
        /// </summary>
        /// <param name="hInstance">
        /// 包含光标资源的模块实例句柄。如果要加载系统预定义光标，此参数应设为IntPtr.Zero
        /// </param>
        /// <param name="lpCursorName">
        /// 光标资源的标识符：
        /// - 若为系统预定义光标，应使用IDC_*常量（如32512表示箭头光标IDC_ARROW）
        /// - 若为自定义资源，可使用MAKEINTRESOURCE宏转换的资源ID
        /// </param>
        /// <returns>
        /// 成功时返回加载的光标句柄（IntPtr），失败时返回IntPtr.Zero
        /// </returns>
        /// <remarks>
        /// 系统预定义光标常量参考：
        /// - IDC_ARROW (32512)：标准箭头光标
        /// - IDC_IBEAM (32513)：文本输入I型光标
        /// - IDC_WAIT (32514)：等待（沙漏）光标
        /// - IDC_CROSS (32515)：十字准星光标
        /// 需在user32.dll可用的环境下调用（通常为Windows系统）
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        /// <summary>
        /// 设置当前线程的光标
        /// </summary>
        /// <param name="hCursor">要设置的光标句柄，由LoadCursor等函数返回</param>
        /// <returns>
        /// 返回之前的光标句柄（IntPtr），可用于后续恢复原始光标
        /// </returns>
        /// <remarks>
        /// 1. 此函数仅临时改变光标，当鼠标移动或系统处理其他消息时可能会恢复默认光标
        /// 2. 若要持久化自定义光标，通常需要处理WM_SETCURSOR消息
        /// 3. 传递IntPtr.Zero会恢复为默认光标
        /// 4. 需确保hCursor是有效的光标句柄，否则可能导致不可预期的行为
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr hCursor);
    }
}