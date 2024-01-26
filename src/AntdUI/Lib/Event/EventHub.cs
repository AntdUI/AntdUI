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

using System.Collections.Generic;

namespace AntdUI
{
    public class EventHub : IEventHub
    {
        private Dictionary<int, List<IEventListener>> _eventDic = new Dictionary<int, List<IEventListener>>();

        public void AddListener(int eventId, IEventListener listener)
        {
            if (_eventDic == null) return;
            _eventDic.TryGetValue(eventId, out var list);
            if (list == null)
            {
                list = new List<IEventListener>();
                _eventDic[eventId] = list;
            }
            list.Add(listener);
        }

        public void RemoveListener(int eventId, IEventListener listener)
        {
            if (_eventDic == null) return;
            _eventDic.TryGetValue(eventId, out var list);
            if (list != null && list.Contains(listener))
            {
                list.Remove(listener);
            }
        }

        public void Dispatch(int eventId, IEventArgs? args)
        {
            if (_eventDic == null) return;
            _eventDic.TryGetValue(eventId, out var list);
            if (list == null || list.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i]?.HandleEvent(eventId, args);
            }
        }
    }

    public interface IEventHub
    {
        void AddListener(int eventId, IEventListener listener);
        void RemoveListener(int eventId, IEventListener listener);
        void Dispatch(int eventId, IEventArgs? args);
    }

    /// <summary>
    /// 事件监听者
    /// </summary>
    public interface IEventListener
    {
        void HandleEvent(int eventId, IEventArgs? args);
    }
    public interface IEventArgs
    {
    }
}