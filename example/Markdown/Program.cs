// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace MarkdownDemo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            AntdUI.Config.TextRenderingHighQuality = true;
            AntdUI.SvgDb.Emoji = AntdUI.FluentFlat.Emoji;
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.Run(new MainForm());
        }
    }
}