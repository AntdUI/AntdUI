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

using System.Drawing.Text;

namespace FontTest
{
    public partial class Main : AntdUI.BaseForm
    {
        public Main()
        {
            InitializeComponent();
        }

        StringFormat s_f = AntdUI.Helper.SF_NoWrap();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }

        void LoadData()
        {
            int size = (int)(140 * AntdUI.Config.Dpi);
            using (var fonts = new InstalledFontCollection())
            {
                // 遍历字体集合
                foreach (var familie in fonts.Families)
                {
                    if (familie.Name.StartsWith("Arial") || familie.Name.StartsWith("HarmonyOS") || familie.Name.StartsWith("Microsoft JhengHei") || familie.Name.StartsWith("Microsoft YaHei") || familie.Name.StartsWith("仿") || familie.Name.StartsWith("宋") || familie.Name.StartsWith("微软") || familie.Name.Contains("思源") || familie.Name.Contains("楷体") || familie.Name.Contains("隶书") || familie.Name.Contains("黑体"))
                    {
                        using (var font = new Font(familie, 62))
                        using (var font_min = new Font(familie, 8))
                        {
                            var pic = new PictureBox
                            {
                                Margin = new Padding(1, 1, 0, 0),
                                Name = familie.Name,
                                Size = new Size(size, size),
                                Image = GetFont(font, font_min, size, "不", out var oy)
                            };
                            flowLayoutPanel1.Controls.Add(pic);
                            pic.Click += Pic_Click;
                        }
                    }
                }
            }
        }

        private void Pic_Click(object? sender, EventArgs e)
        {
            if (sender is PictureBox pic)
            {
                var control = new UserControl1(pic.Name);
                var config = AntdUI.Drawer.config(this, control);
                config.OnLoad = control.LoadData;
                AntdUI.Drawer.open(config);
            }
        }

        Bitmap GetFont(Font font, Font font_min, int size, string text, out int oy)
        {
            var bmp = new Bitmap(size, size);
            float cs = size / 2F;
            oy = 0;
            bool r = false;
            using (var g = AntdUI.Helper.High(Graphics.FromImage(bmp)))
            {
                #region 绘制原始

                using (var bmp_o = new Bitmap(size, size))
                {
                    using (var g_o = AntdUI.Helper.High(Graphics.FromImage(bmp_o)))
                    {
                        g_o.String(text, font, Brushes.Black, new Rectangle(0, 0, bmp.Width, bmp.Height), s_f);
                    }

                    AntdUI.CorrectionTextRendering.TextRealY(bmp_o, out var ry, out var rheight);

                    #region 绘制线

                    g.Fill(Brushes.Black, new Rectangle(0, 0, size, 1));
                    g.Fill(Brushes.Black, new Rectangle(0, size - 1, size, 1));
                    g.Fill(Brushes.Black, new Rectangle(0, 0, 1, size));
                    g.Fill(Brushes.Black, new Rectangle(size - 1, 0, 1, size));
                    using (var brush = new SolidBrush(Color.FromArgb(180, Color.Green)))
                    {
                        g.Fill(brush, new RectangleF(0, cs - 1, size, 2));
                    }

                    #endregion

                    var font_size = AntdUI.Helper.Size(g.MeasureString(text, font));
                    int y = (bmp.Height - font_size.Height) / 2, height = font_size.Height;

                    float ready = ry + rheight / 2F;
                    int xc = (int)Math.Round(cs - ready);
                    if (xc == 0)
                    {
                        r = true;
                        using (var brush = new SolidBrush(Color.FromArgb(100, Color.Green)))
                        {
                            g.Fill(brush, new Rectangle(0, ry, size, 1));
                            g.Fill(brush, new Rectangle(0, ry + rheight, size, 1));
                        }
                        using (var brush = new SolidBrush(Color.Green))
                        {
                            g.String(text, font, brush, new Rectangle(0, 0, bmp.Width, bmp.Height), s_f);
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(100, 255, 0, 0)))
                        {
                            g.Fill(brush, new Rectangle(0, ry, size, 1));
                            g.Fill(brush, new Rectangle(0, ry + rheight, size, 1));
                        }
                        oy = xc;
                        using (var brush = new SolidBrush(Color.FromArgb(40, 255, 0, 0)))
                        {
                            g.String(text, font, brush, new Rectangle(0, 0, bmp.Width, bmp.Height), s_f);
                        }
                        using (var brush = new SolidBrush(Color.Green))
                        {
                            g.String(text, font, brush, new Rectangle(0, xc, bmp.Width, bmp.Height), s_f);
                        }
                    }
                }

                #endregion

                g.String(font.Name, font_min, r ? Brushes.Green : Brushes.Black, 1, 1);
            }
            return bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            LoadData();
        }
    }
}
