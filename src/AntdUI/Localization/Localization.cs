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
                if (currentLanguage == null) currentLanguage = Thread.CurrentThread.CurrentUICulture.Name;
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
        internal static string? GetLangI(this IControl control, string? id, string? def)
        {
            if (id == null) return def;
            if (DefaultLanguage == CurrentLanguage) return def;
            return Provider?.GetLocalizedString(id.Replace("{id}", control.Name)) ?? def;
        }
        internal static string GetLangIN(this IControl control, string? id, string def)
        {
            if (id == null) return def;
            if (DefaultLanguage == CurrentLanguage) return def;
            return Provider?.GetLocalizedString(id.Replace("{id}", control.Name)) ?? def;
        }

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
                resources.ApplyResources(item, item.Name);
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