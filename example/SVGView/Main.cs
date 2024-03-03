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

namespace SVGView
{
    public partial class Main : AntdUI.Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, MouseEventArgs e)
        {
            string svg = input1.Text;
            int i = svg.IndexOf(">") + 1;
            string first = svg.Substring(0, i), last = svg.Substring(i);
            ReIdOne(ref first, "t");
            ReId(ref first, "class", "version", "xmlns", "width", "height");
            svg = first + last;
            if (e.Button == MouseButtons.Right) ReId(ref svg, "fill", "p-id");
            else ReId(ref svg, "p-id");
            input1.Text = svg.Replace(" >", ">");
        }

        void ReId(ref string svg, params string[] ids)
        {
            foreach (string id in ids) ReId(ref svg, id);
        }
        void ReId(ref string svg, string id)
        {
            int i = svg.IndexOf(id + "=\"");
            while (i != -1)
            {
                string first = svg.Substring(0, i), last = svg.Substring(i + id.Length + 2);
                last = last.Substring(last.IndexOf("\"") + 1).Trim();
                svg = first + last;
                i = svg.IndexOf(id + "=\"");
            }
        }

        void ReIdOne(ref string svg, string id)
        {
            int i = svg.IndexOf(id + "=\"");
            if (i > -1)
            {
                string first = svg.Substring(0, i), last = svg.Substring(i + id.Length + 2);
                last = last.Substring(last.IndexOf("\"") + 1).Trim();
                svg = first + last;
            }
        }

        private void input1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string svg = input1.Text;
                button1.ImageSvg = svg;
                int size = (int)Math.Floor((pictureBox1.Width > pictureBox1.Height ? pictureBox1.Height : pictureBox1.Width) * 0.8F);
                pictureBox1.Image = AntdUI.SvgExtend.SvgToBmp(svg, size, size, Color.Black);
            }
            catch { }
        }

        private void button2_Click(object sender, MouseEventArgs e)
        {
            try
            {
                string svg = Clipboard.GetText();
                int i = svg.IndexOf(">") + 1;
                string first = svg.Substring(0, i), last = svg.Substring(i);
                ReIdOne(ref first, "t");
                ReId(ref first, "class", "version", "xmlns", "width", "height");
                svg = first + last;
                if (e.Button == MouseButtons.Right) ReId(ref svg, "fill", "p-id");
                else ReId(ref svg, "p-id");
                input1.Text = svg.Replace(" >", ">");
                Clipboard.SetText(input1.Text);
            }
            catch { }
        }
    }
}