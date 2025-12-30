// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// 本地化
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// 本地化提供程序
        /// </summary>
        public static ILocalization? Provider { get; set; }

        #region 默认语言

        /// <summary>
        /// 默认语言
        /// </summary>
        public static string? DefaultLanguage { get; set; }

        static string? currentLanguage;
        public static string CurrentLanguage
        {
            get
            {
                currentLanguage ??= Thread.CurrentThread.CurrentUICulture.Name;
                return currentLanguage;
            }
        }

        #endregion

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="lang">语言name</param>
        public static void SetLanguage(this string lang)
        {
            var culture = new System.Globalization.CultureInfo(lang);
            currentLanguage = culture.Name;
            Thread.CurrentThread.CurrentUICulture = culture;
            EventHub.Dispatch(EventType.LANG, culture);
        }

        public static string Get(string id, string def) => Provider?.GetLocalizedString(id) ?? def;

        #region 获取

        public static string? GetLangI(this Control control, string? id, string? def)
        {
            if (id == null) return def;
            if (DefaultLanguage == CurrentLanguage) return def;
            return Provider?.GetLocalizedString(id.Replace("{id}", control.Name)) ?? def;
        }
        public static string GetLangIN(this Control control, string? id, string def)
        {
            if (id == null) return def;
            if (DefaultLanguage == CurrentLanguage) return def;
            return Provider?.GetLocalizedString(id.Replace("{id}", control.Name)) ?? def;
        }

        public static string? GetLangI(string? id, string? def)
        {
            if (id == null) return def;
            if (DefaultLanguage == CurrentLanguage) return def;
            return Provider?.GetLocalizedString(id) ?? def;
        }

        public static string? GetLangI(string? id, string? def, params string?[][] dir)
        {
            if (id == null) return def;
            if (DefaultLanguage == CurrentLanguage) return def;
            if (dir.Length > 0)
            {
                foreach (var it in dir)
                {
                    string? k = it[0], v = it[1];
                    if (k == null || v == null) continue;
                    id = id.Replace(k, v);
                }
            }
            return Provider?.GetLocalizedString(id) ?? def;
        }

        public static string GetLangIN(string? id, string def, params string?[][] dir)
        {
            if (id == null) return def;
            if (DefaultLanguage == CurrentLanguage) return def;
            if (dir.Length > 0)
            {
                foreach (var it in dir)
                {
                    string? k = it[0], v = it[1];
                    if (k == null || v == null) continue;
                    id = id.Replace(k, v);
                }
            }
            return Provider?.GetLocalizedString(id) ?? def;
        }

        #endregion

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="form">加载语言的窗口</param>
        public static void LoadLanguage<T>(this Form form)
        {
            var resources = new ComponentResourceManager(typeof(T));
            resources.ApplyResources(form, "$this");
            Loading(form, resources);
            if (form is BaseForm baseForm && baseForm.AutoHandDpi) baseForm.AutoDpi(baseForm);
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="resources">语言资源</param>
        static void Loading(Control control, ComponentResourceManager resources)
        {
            if (control is MenuStrip ms)
            {
                //将资源与控件对应
                resources.ApplyResources(control, control.Name);
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items) Loading(c, resources);
                }
            }

            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                Loading(c, resources);
            }
        }

        /// <summary>
        /// 遍历菜单
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="resources">语言资源</param>
        static void Loading(ToolStripMenuItem item, ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem tsmi)
            {
                resources.ApplyResources(item, item.Name!);
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem c in tsmi.DropDownItems) Loading(c, resources);
                }
            }
        }
    }

    public interface ILocalization
    {
        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <returns>本地化字符串</returns>
        string? GetLocalizedString(string key);
    }
}