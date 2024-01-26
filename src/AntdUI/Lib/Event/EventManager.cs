// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace AntdUI
{
    public class EventManager
    {
        public static EventManager Instance
        {
            get
            {
                if (_instance == null) _instance = new EventManager();
                return _instance;
            }
        }

        internal static EventManager? _instance;

        internal IEventHub _eventHub;

        public EventManager()
        {
            _eventHub = new EventHub();
        }

        /// <summary>
        /// 添加监听事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener"></param>
        public void AddListener(int eventId, IEventListener listener)
        {
            _eventHub.AddListener(eventId, listener);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="args">事件参数可通过 EventArgs.CreateEventArgs 创建</param>
        public void Dispatch(int eventId, IEventArgs? args = null)
        {
            _eventHub.Dispatch(eventId, args);
        }

        /// <summary>
        /// 移除监听事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener"></param>
        public void RemoveListener(int eventId, IEventListener listener)
        {
            _eventHub.RemoveListener(eventId, listener);
        }
    }
}