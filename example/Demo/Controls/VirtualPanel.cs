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

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class VirtualPanel : UserControl
    {
        Form form;
        public VirtualPanel(Form _form)
        {
            form = _form;
            InitializeComponent();

            foreach (AntdUI.TJustifyContent it in Enum.GetValues(typeof(AntdUI.TJustifyContent)))
            {
                select1.Items.Add(it);
            }
            select1.SelectedValue = vpanel.JustifyContent;
            select1.SelectedValueChanged += Select1_SelectedValueChanged;

            foreach (AntdUI.TAlignItems it in Enum.GetValues(typeof(AntdUI.TAlignItems)))
            {
                select2.Items.Add(it);
            }
            select2.SelectedValue = vpanel.AlignItems;
            select2.SelectedValueChanged += Select2_SelectedValueChanged;

            foreach (AntdUI.TAlignContent it in Enum.GetValues(typeof(AntdUI.TAlignContent)))
            {
                select3.Items.Add(it);
            }
            select3.SelectedValue = vpanel.AlignContent;
            select3.SelectedValueChanged += Select3_SelectedValueChanged;

            LoadData();
        }

        void LoadData()
        {
            vpanel.Items.AddRange(new VItem[] {
                new VItem(Color.FromArgb(255, 127, 80),150),
                new VItem(Color.FromArgb(173, 216, 230),200),
                new VItem(Color.FromArgb(240, 230, 255),250),
                new VItem(Color.FromArgb(255, 192, 203)),


                new VItem(Color.FromArgb(255, 127, 80)),
                new VItem(Color.FromArgb(173, 216, 230)),
                new VItem(Color.FromArgb(255, 230, 140),200),
                new VItem(Color.FromArgb(255, 192, 203)),


                new VItem(Color.FromArgb(255, 127, 80)),
                new VItem(Color.FromArgb(173, 216, 230)),
                new VItem(Color.FromArgb(240, 230, 140)),
                new VItem(Color.FromArgb(255, 192, 203))
            });
        }

        private void Select1_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.TJustifyContent it) vpanel.JustifyContent = it;
        }

        private void Select2_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.TAlignItems it) vpanel.AlignItems = it;
        }

        private void Select3_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.TAlignContent it) vpanel.AlignContent = it;
        }

        private void checkbox1_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            vpanel.Items.Clear();
            if (e.Value)
            {
                vpanel.Items.AddRange(new VItem[] {
                new VItem(Color.FromArgb(255, 127, 80),100,150),
                new VItem(Color.FromArgb(173, 216, 230),100,200),
                new VItem(Color.FromArgb(240, 230, 255),100,250),
                new VItem(Color.FromArgb(255, 192, 203)),


                new VItem(Color.FromArgb(255, 127, 80)),
                new VItem(Color.FromArgb(173, 216, 230)),
                new VItem(Color.FromArgb(255, 230, 140),100,200),
                new VItem(Color.FromArgb(255, 192, 203)),


                new VItem(Color.FromArgb(255, 127, 80)),
                new VItem(Color.FromArgb(173, 216, 230)),
                new VItem(Color.FromArgb(240, 230, 140)),
                new VItem(Color.FromArgb(255, 192, 203))
                });
            }
            else LoadData();
            vpanel.Waterfall = e.Value;
        }

        class VItem : AntdUI.VirtualItem
        {
            public Color data;
            int width, height;
            public VItem(Color d, int size = 100) { data = d; width = height = size; }
            public VItem(Color d, int w, int h) { data = d; width = w; height = h; }

            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                using (var brush = new SolidBrush(data))
                {
                    g.Fill(brush, e.Rect);
                }
            }

            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                return new Size((int)(width * dpi), (int)(height * dpi));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkbox1.Checked = true;
            vpanel.Items.Clear();
            vpanel.Gap = 0;
            vpanel.Shadow = 10;
            vpanel.ShadowOpacityAnimation = true;
            vpanel.Radius = 20;
            vpanel.Waterfall = true;
            vpanel.PauseLayout = true;
            vpanel.Padding = new Padding(20, 0, 20, 0);
            int i = 0;
            var random = new Random();
            foreach (var item in Directory.GetFiles("imgs"))
            {
                int c = random.Next(2, 500), f = random.Next(2, 500);
                vpanel.Items.Add(new VImg(Image.FromFile(item), i, c, f));
                i++;
            }
            vpanel.PauseLayout = false;
        }


        class VImg : AntdUI.VirtualShadowItem
        {
            Image image;
            string name, desc;
            public VImg(Image img, int i, int c, int f)
            {
                image = img;
                switch (i)
                {
                    case 0:
                        name = "X新媒体|种草类";
                        break;
                    case 1:
                        name = "狂吃胡萝卜";
                        break;
                    case 2:
                        name = "小红书团队运营活动作品";
                        break;
                    case 3:
                        name = "BONJOUR!CHOU";
                        break;
                    case 4:
                        name = "工业设计";
                        break;
                    case 5:
                        name = "古装场景";
                        break;
                    case 6:
                        name = "工业设计";
                        break;
                    case 7:
                        name = "王林武";
                        break;
                    case 8:
                        name = "运营活动/闪屏";
                        break;
                    case 9:
                        name = "林逼逼";
                        break;
                    case 10:
                        name = "品牌-包装";
                        break;
                    case 11:
                        name = "LG▼科幻星际机甲异能";
                        break;
                    case 12:
                        name = "Baron朱";
                        break;
                    case 13:
                        name = "3D人物";
                        break;
                    case 14:
                        name = "科技/空间感/高清背景";
                        break;
                    case 15:
                        name = "酸性辐射";
                        break;
                    default:
                        name = "蚂蚁设计";
                        break;
                }
                desc = GetDesc(c, "采集") + " " + GetDesc(f, "粉丝");
            }

            string GetDesc(int v, string h)
            {
                if (v > 100) return Math.Round(v / 100.0, 1) + "w" + h;
                return v * 100 + h;
            }

            StringFormat s_f = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
            Bitmap bmp = null;
            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                if (bmp == null || bmp.Width != e.Rect.Width || bmp.Height != e.Rect.Height)
                {
                    bmp?.Dispose();
                    bmp = new Bitmap(e.Rect.Width, e.Rect.Height);
                    using (var g2 = AntdUI.Helper.High(Graphics.FromImage(bmp)))
                    {
                        g2.Image(new Rectangle(0, 0, bmp.Width, bmp.Height), image, AntdUI.TFit.Fill, e.Radius, AntdUI.TShape.Default);
                    }
                }
                g.Image(bmp, e.Rect);
                using (var path = AntdUI.Helper.RoundPath(e.Rect, e.Radius))
                {
                    g.Draw(AntdUI.Style.Db.BorderColor, 1.5F * dpi, path);
                }

                #region 渐变色

                int h1 = (int)(26 * dpi), h2 = (int)(20 * dpi), h3 = h2 / 2, th = h1 + h2 + h3;

                var rect_b = new Rectangle(e.Rect.X, e.Rect.Bottom - th, e.Rect.Width, th);
                using (var brush = new LinearGradientBrush(rect_b, Color.Transparent, Color.FromArgb(250, 0, 0, 0), 90))
                {
                    using (var path = AntdUI.Helper.RoundPath(rect_b, e.Radius, false, false, true, true))
                    {
                        g.Fill(brush, path);
                    }
                }

                #endregion

                int y = e.Rect.Bottom - h2 - h3;
                using (var font_desc = new Font(e.Panel.Font.FontFamily, 9F))
                {
                    g.String(desc, font_desc, Brushes.WhiteSmoke, new Rectangle(e.Rect.X, y, e.Rect.Width, h2), s_f);
                }
                using (var font_title = new Font(e.Panel.Font.FontFamily, 11F, FontStyle.Bold))
                {
                    g.String(name, font_title, Brushes.White, new Rectangle(e.Rect.X, y - h1, e.Rect.Width, h1), s_f);
                }
            }

            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                int count = 5, w = (e.Rect.Width - ((int)(20 * dpi) * count)) / count;
                float dpi_x = w * 1F / image.Width;
                int h = (int)(image.Height * dpi_x);
                return new Size(w, h);
            }
        }
    }
}