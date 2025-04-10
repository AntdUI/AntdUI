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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing.Text;

namespace FontTest
{
    public partial class UserControl1 : UserControl
    {
        string familieName;
        public UserControl1(string familie_name)
        {
            familieName = familie_name;
            InitializeComponent();

            using (var fonts = new InstalledFontCollection())
            {
                // 遍历字体集合
                var items = new List<string>(fonts.Families.Length);
                foreach (var familie in fonts.Families)
                {
                    items.Add(familie.Name);
                }
                select1.Items.AddRange(items.ToArray());
            }
            select1.SelectedValue = familieName;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            select1.SelectedIndexChanged += select1_SelectedIndexChanged;
            inputNumber1.ValueChanged += inputNumber1_ValueChanged;
            input1.TextChanged += input1_TextChanged;
        }

        public void LoadData()
        {
            BeginInvoke(LoadD);
        }

        public void LoadD()
        {
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();
            foreach (var item in input1.Text.Split(new string[] { "、", "，", ",", "。", " ", "\t" }, StringSplitOptions.None))
            {
                var text = item.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    if (text.Length > 1) text = text.Substring(0, 1);
                    using (var font = new Font(select1.Text, (int)inputNumber1.Value))
                    {
                        int size = AntdUI.Helper.GDI(g =>
                        {
                            return (int)(AntdUI.Helper.Size(g.MeasureString("中文Qq", font)).Height * 1.2F);
                        });
                        var pic = new PictureBox
                        {
                            Margin = new Padding(1, 1, 0, 0),
                            Size = new Size(size, size),
                            Image = GetFont(font, size, text, out var oy)
                        };
                        flowLayoutPanel1.Controls.Add(pic);
                    }
                }
            }
            flowLayoutPanel1.ResumeLayout();
        }

        StringFormat s_f = AntdUI.Helper.SF_NoWrap();
        Bitmap GetFont(Font font, int size, string text, out int oy)
        {
            var bmp = new Bitmap(size, size);
            float cs = size / 2F;
            oy = 0;
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
            }
            return bmp;
        }

        private void select1_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            LoadData();
        }

        private void inputNumber1_ValueChanged(object sender, AntdUI.DecimalEventArgs e)
        {
            LoadData();
        }

        private void input1_TextChanged(object? sender, EventArgs e)
        {
            LoadData();
        }
    }
}
