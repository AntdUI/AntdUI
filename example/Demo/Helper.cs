// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Demo
{
    public static class Helper
    {
        static Font FontHeader = new Font("Microsoft YaHei UI", 12F), FontDivider = new Font("Microsoft YaHei UI", 10F);
        public static UserControl AddPage(string id, string key, string desc, List<ViewPage> pages)
        {
            var panel = new AntdUI.In.Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                TabIndex = 1
            };
            var header = new AntdUI.PageHeader
            {
                Text = id + " " + key,
                Description = desc,
                LocalizationText = id,
                LocalizationDescription = id + ".Description",
                Dock = DockStyle.Top,
                Font = FontHeader,
                Name = "header1",
                Padding = new Padding(0, 0, 0, 10),
                Size = new Size(845, 74),
                TabIndex = 0,
                UseTitleFont = true
            };
            var controls = new List<Control>(pages.Count * 2);
            foreach (var it in pages)
            {
                it.Control.Dock = DockStyle.Top;
                var divider = new AntdUI.Divider
                {
                    Dock = DockStyle.Top,
                    Font = FontDivider,
                    LocalizationText = id + ".{id}",
                    Name = it.Name,
                    Orientation = AntdUI.TOrientation.Left,
                    Size = new Size(0, 28),
                    Text = it.Text
                };
                controls.InsertRange(0, it.Control, divider);
            }
            panel.Controls.AddRange(controls.ToArray());

            var control = new UserControl
            {
                Font = FontHeader
            };
            control.Controls.AddRange(panel, header);
            return control;
        }

        public static void TestClickButton(this AntdUI.Button btn)
        {
            btn.LoadingValue = 0.3F;
            btn.LoadingWaveValue = 0;
            if (btn.Tag == null)
            {
                btn.Tag = "tom";
                OpenPopoverTip(btn, "再次点击会有惊喜 😁", "ButtonTipAgain");
                return;
            }
            btn.Tag = null;
            AntdUI.ITask.Run(() =>
            {
                btn.Loading = true;
                var tip = OpenPopoverTip(btn, "这是加载效果 🌸", "ButtonTipLoading");
                Thread.Sleep(2200);

                tip?.Dispose();
                tip = OpenPopoverTip(btn, "我现在将它禁用了", "ButtonTipEnabled");
                btn.Enabled = false;

                Thread.Sleep(2200);

                btn.Enabled = true;
                Thread.Sleep(500);
                tip?.Dispose();
                tip = OpenPopoverTip(btn, "水波进度加载效果 🌊", "ButtonTipWave");
                Thread.Sleep(500);
                if (btn.Text == null)
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        btn.LoadingWaveValue = i / 100F;
                        Thread.Sleep(20);
                    }
                    Thread.Sleep(500);
                }
                else
                {
                    for (int i = 0; i <= 50; i++)
                    {
                        btn.LoadingWaveValue = i / 100F;
                        Thread.Sleep(20);
                    }
                    Thread.Sleep(500);
                    tip?.Dispose();
                    tip = OpenPopoverTip(btn, "移除加载圈 💀", "ButtonTipWaveZero");
                    Thread.Sleep(1000);
                    btn.LoadingValue = -1;
                    for (int i = 0; i <= 50; i++)
                    {
                        var prog = i / 100F;
                        btn.LoadingWaveValue = 0.5F + prog;
                        Thread.Sleep(20);
                    }
                }
                tip?.Dispose();
                tip = OpenPopoverTip(btn, "来点骚操作 👽", "ButtonTipNB");
                Thread.Sleep(200);
                for (int i = 0; i <= 100; i++)
                {
                    var prog = i / 100F;
                    btn.LoadingWaveValue = 1F - prog;
                    btn.LoadingValue = prog;
                    Thread.Sleep(20);
                }
                for (int i = 0; i <= 70; i++)
                {
                    btn.LoadingValue = 1F - i / 100F;
                    Thread.Sleep(10);
                }
                Thread.Sleep(100);
                btn.Loading = false;
                tip?.Dispose();
                tip = OpenPopoverTip(btn, "☺️ 谢谢惠顾", "ButtonThank");
            });
        }

        static Form OpenPopoverTip(AntdUI.IControl control, string text, string localization)
        {
            return AntdUI.Popover.open(new AntdUI.Popover.Config(control, AntdUI.Localization.Get(localization, text)).SetArrow(AntdUI.TAlign.Top).SetAutoClose(2).SetBack(AntdUI.Style.Get(AntdUI.Colour.BgSpotlight)).SetFore(AntdUI.Style.Get(AntdUI.Colour.TextSpotlight)));
        }
    }
}