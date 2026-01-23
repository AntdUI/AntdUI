// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace ChatUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AntdUI.Config.TextRenderingHighQuality = true;
            AntdUI.Config.Theme().Header("f3f3f3", "111111").Light("f7f7f7").Dark("202020").FormBorderColor();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            AntdUI.SvgDb.Emoji = AntdUI.FluentFlat.Emoji;
            Application.Run(new Main());
        }
    }
}