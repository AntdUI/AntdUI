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

using System.Collections.Concurrent;
using System.Windows.Forms;

namespace AntdUI
{
    public static class EventHub
    {
        static ConcurrentDictionary<Control, IEventListener> dic = new ConcurrentDictionary<Control, IEventListener>();

        public static void AddListener(this Control control)
        {
            if (control is IEventListener listener && dic.TryAdd(control, listener))
            {
                control.Disposed += (s, e) =>
                {
                    dic.TryRemove(control, out _);
                };
            }
        }

        public static void Dispatch(EventType id, object? tag = null)
        {
            foreach (var item in dic) item.Value.HandleEvent(id, tag);
        }
    }

    /// <summary>
    /// 事件监听者
    /// </summary>
    public interface IEventListener
    {
        void HandleEvent(EventType id, object? tag);
    }

    public enum EventType
    {
        /// <summary>
        /// DPI 改变
        /// </summary>
        DPI = 1,
        /// <summary>
        /// 主题 改变
        /// </summary>
        THEME = 2,
        /// <summary>
        /// 语言 改变
        /// </summary>
        LANG = 3,
        /// <summary>
        /// Window 状态改变
        /// </summary>
        WINDOW_STATE = 70
    }
}