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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    public partial class Colors : BaseForm
    {
        public Colors()
        {
            InitializeComponent();
            Generate();
        }

        int padd = 20;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            padd = colorLight1.Margin.Top;
        }

        void Generate()
        {
            var colors = panel_primary.Back.Value.Generate();
            var colors_dark = colors[5].GenerateDark();
            panel11.Back = colors_dark[5];
            color_dark.TextDesc = "#" + panel11.Back.Value.ToHex();
            int i = 1;
            foreach (var color in colors)
            {
                var _panel = tablePanel.Controls.Find("colorLight" + i, false);
                if (_panel.Length > 0)
                {
                    if (_panel[0] is ColorPanel panel)
                    {
                        var mode = color.ColorMode();
                        panel.ForeColor = mode ? Color.Black : Color.White;
                        panel.BackColor = color;
                        panel.TextDesc = "#" + color.ToHex();
                    }
                }
                i++;
            }
            i = 1;
            foreach (var color in colors_dark)
            {
                var _panel = tablePanelDark.Controls.Find("colorDark" + i, false);
                if (_panel.Length > 0)
                {
                    if (_panel[0] is ColorPanel panel)
                    {
                        var mode = color.ColorMode();
                        panel.ForeColor = mode ? Color.Black : Color.White;
                        panel.BackColor = color;
                        panel.TextDesc = "#" + color.ToHex();
                    }
                }
                i++;
            }
        }

        void label15_Click(object sender, EventArgs e)
        {
            using (var dialog = new ColorDialog
            {
                Color = panel_primary.Back.Value
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    panel_primary.Back = dialog.Color;
                    color_primary.TextDesc = textBox1.Text = "#" + dialog.Color.ToHex();
                    Generate();
                }
            }
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            panel_primary.Back = textBox1.Text.ToColor();
            color_primary.TextDesc = "#" + panel_primary.Back.Value.ToHex();
            Generate();
        }

        void ColorPanel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is ColorPanel panel) panel.Margin = new Padding(0);
        }

        void ColorPanel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is ColorPanel panel) panel.Margin = new Padding(0, padd, 0, 0);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            foreach (ColorPanel panel in tablePanel.Controls) panel.Margin = new Padding(0, padd, 0, 0);
            foreach (ColorPanel panel in tablePanelDark.Controls) panel.Margin = new Padding(0, padd, 0, 0);
            base.OnMouseLeave(e);
        }

        void ColorPanel_Click(object sender, EventArgs e)
        {
            if (sender is ColorPanel panel)
            {
                textBox1.Text = panel.TextDesc;
                Clipboard.SetText(panel.TextDesc);
            }
        }
    }
}
