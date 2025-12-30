// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;

namespace AntdUI
{
    public class ThemeConfig : IThemeConfig
    {
        BaseForm form;
        public ThemeConfig(BaseForm _form)
        {
            form = _form;
        }

        public void Change(bool dark)
        {
            form.Dark = dark;
            if (btn != null) btn.Toggle = dark;
            if (dark)
            {
                callDark?.Invoke();
                if (pageheader != null && headerDark.HasValue) pageheader.BackColor = headerDark.Value;
                if (backDark.HasValue) form.BackColor = backDark.Value;
                if (foreDark.HasValue) form.ForeColor = foreDark.Value;
            }
            else
            {
                callLight?.Invoke();
                if (pageheader != null && headerLight.HasValue) pageheader.BackColor = headerLight.Value;
                if (backLight.HasValue) form.BackColor = backLight.Value;
                if (foreLight.HasValue) form.ForeColor = foreLight.Value;
            }
            oncall?.Invoke(dark);
        }

        internal void HandleEvent(EventType id, object? tag)
        {
            if (id == EventType.THEME && tag is TMode mode) Change(mode == TMode.Dark);
        }
    }

    public class IThemeConfig
    {
        #region 回调

        internal Action? callLight, callDark;
        internal Action<bool>? oncall;

        /// <summary>
        /// 设置回调
        /// </summary>
        public virtual IThemeConfig Call(Action<bool>? call)
        {
            oncall = call;
            return this;
        }

        /// <summary>
        /// 浅色模式回调
        /// </summary>
        /// <param name="light">浅色模式</param>
        public virtual IThemeConfig Light(Action? light = null)
        {
            callLight = light;
            return this;
        }

        /// <summary>
        /// 深色模式回调
        /// </summary>
        /// <param name="dark">深色模式</param>
        public virtual IThemeConfig Dark(Action? dark = null)
        {
            callDark = dark;
            return this;
        }

        #endregion

        #region 背景

        internal Color? backLight, foreLight, backDark, foreDark;

        #region 浅色模式颜色

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public virtual IThemeConfig Light(Color back, Color fore)
        {
            backLight = back;
            foreLight = fore;
            return this;
        }

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public virtual IThemeConfig Light(Color back) => Light(back, Color.Black);

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public virtual IThemeConfig Light(string back, string fore) => Light(back.ToColor(), fore.ToColor());

        /// <summary>
        /// 浅色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public virtual IThemeConfig Light(string back) => Light(back, "000000");

        #endregion

        #region 深色模式颜色

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public virtual IThemeConfig Dark(Color back, Color fore)
        {
            backDark = back;
            foreDark = fore;
            return this;
        }

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public virtual IThemeConfig Dark(Color back) => Dark(back, Color.White);

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        /// <param name="fore">文本颜色</param>
        public virtual IThemeConfig Dark(string back, string fore) => Dark(back.ToColor(), fore.ToColor());

        /// <summary>
        /// 深色模式颜色
        /// </summary>
        /// <param name="back">背景颜色</param>
        public IThemeConfig Dark(string back) => Dark(back, "ffffff");

        #endregion

        internal PageHeader? pageheader;
        internal Color? headerLight, headerDark;

        /// <summary>
        /// 设置页面头部颜色
        /// </summary>
        /// <param name="light">浅色背景色</param>
        /// <param name="dark">深色背景色</param>
        public virtual IThemeConfig Header(Color light, Color dark)
        {
            headerLight = light;
            headerDark = dark;
            return this;
        }

        /// <summary>
        /// 设置页面头部颜色
        /// </summary>
        /// <param name="light">浅色背景色</param>
        /// <param name="dark">深色背景色</param>
        public virtual IThemeConfig Header(string light, string dark)
        {
            headerLight = light.ToColor();
            headerDark = dark.ToColor();
            return this;
        }

        /// <summary>
        /// 设置页面头部颜色
        /// </summary>
        /// <param name="header">页头</param>
        /// <param name="light">浅色背景色</param>
        /// <param name="dark">深色背景色</param>
        public virtual IThemeConfig Header(PageHeader header, Color light, Color dark)
        {
            pageheader = header;
            headerLight = light;
            headerDark = dark;
            return this;
        }

        /// <summary>
        /// 设置页面头部颜色
        /// </summary>
        /// <param name="header">页头</param>
        /// <param name="light">浅色背景色</param>
        /// <param name="dark">深色背景色</param>
        public virtual IThemeConfig Header(PageHeader header, string light, string dark)
        {
            pageheader = header;
            headerLight = light.ToColor();
            headerDark = dark.ToColor();
            return this;
        }

        /// <summary>
        /// 清空页面头部颜色
        /// </summary>
        /// <param name="header">页头</param>
        public virtual IThemeConfig HeaderClear(PageHeader header)
        {
            pageheader = header;
            headerLight = headerDark = null;
            return this;
        }

        #endregion

        #region 按钮

        internal Button? btn;
        /// <summary>
        /// 设置按钮 Toggle
        /// </summary>
        public virtual IThemeConfig Button(Button? button)
        {
            btn = button;
            return this;
        }

        #endregion

        public IThemeConfig SetConfig(IThemeConfig config)
        {
            callLight ??= config.callLight;
            callDark ??= config.callDark;
            oncall ??= config.oncall;

            if (backLight == null) backLight = config.backLight;
            if (foreLight == null) foreLight = config.foreLight;
            if (backDark == null) backDark = config.backDark;
            if (foreDark == null) foreDark = config.foreDark;

            pageheader ??= config.pageheader;
            if (headerLight == null) headerLight = config.headerLight;
            if (headerDark == null) headerDark = config.headerDark;

            btn ??= config.btn;

            return this;
        }
    }
}