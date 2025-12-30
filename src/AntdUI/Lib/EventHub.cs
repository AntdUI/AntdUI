// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Concurrent;

namespace AntdUI
{
    public static class EventHub
    {
        static ConcurrentDictionary<int, WeakReference> dic = new ConcurrentDictionary<int, WeakReference>();

        public static void AddListener(this IEventListener listener)
        {
            var id = listener.GetHashCode();
            if (dic.TryAdd(id, new WeakReference(listener))) listener.Disposed += (s, e) => dic.TryRemove(id, out _);
        }

        internal static void Add(this BaseForm listener)
        {
            var id = listener.GetHashCode();
            if (dic.TryAdd(id, new WeakReference(listener))) listener.Disposed += (s, e) => dic.TryRemove(id, out _);
        }

        public static void Dispatch(EventType id, object? tag = null)
        {
            foreach (var item in dic)
            {
                if (item.Value.IsAlive)
                {
                    if (item.Value.Target is IEventListener listener) listener.HandleEvent(id, tag);
                    else if (item.Value.Target is BaseForm baseForm) baseForm.themeConfig!.HandleEvent(id, tag);
                }
                else dic.TryRemove(item.Key, out _);
            }
        }
    }

    /// <summary>
    /// 事件监听者
    /// </summary>
    public interface IEventListener
    {
        void HandleEvent(EventType id, object? tag);

        int GetHashCode();

        event EventHandler? Disposed;
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
        /// 主题主色调 改变
        /// </summary>
        THEME_PRIMARY = 20,
        /// <summary>
        /// Window 状态改变
        /// </summary>
        WINDOW_STATE = 70,
        /// <summary>
        /// 自定义
        /// </summary>
        DIV = 100
    }
}