// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Demo
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arge)
        {
#if !NET10_0
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
#endif
            var command = string.Join(" ", arge);
            AntdUI.Localization.DefaultLanguage = "zh-CN";
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en")) AntdUI.Localization.Provider = new Localizer();
            AntdUI.Config.Theme().Dark("#000", "#fff").Light("#fff", "#000");
            AntdUI.Config.TextRenderingHighQuality = true;
            AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            AntdUI.Config.SetEmptyImageSvg(Properties.Resources.icon_empty, Properties.Resources.icon_empty_dark);
            AntdUI.SvgDb.Emoji = AntdUI.FluentFlat.Emoji;
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            if (command == "m") Application.Run(new Main());
            else if (command == "color") Application.Run(new Colors());
            else if (command == "tab") Application.Run(new TabHeaderForm());
            else Application.Run(new Overview(command == "t"));
        }
    }
}