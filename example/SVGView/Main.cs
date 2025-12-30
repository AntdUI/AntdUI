// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace SVGView
{
    public partial class Main : AntdUI.Window
    {
        public Main()
        {
            InitializeComponent();
            input1.MaxLength = int.MaxValue;
        }

        private void button1_Click(object sender, MouseEventArgs e)
        {
            var svg = input1.Text;
            if (string.IsNullOrEmpty(svg)) return;
            input1.Text = Hand(svg.Trim(), e.Button == MouseButtons.Right);
        }

        void ReId(ref string svg, params string[] ids)
        {
            foreach (string id in ids) ReId(ref svg, id);
        }
        void ReId(ref string svg, string id)
        {
            if (svg.Contains(" " + id + "=\"") || svg.Contains("<" + id + "=\""))
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
        }

        void ReNodeOne(ref string svg, string id)
        {
            int i = svg.IndexOf("<" + id + ">");
            if (i > -1)
            {
                string first = svg.Substring(0, i), last = svg.Substring(i + id.Length + 2);
                last = last.Substring(last.IndexOf("</") + id.Length + 3).Trim();
                svg = first + last;
            }
        }

        void input1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var svg = input1.Text;
                if (svg == null) return;
                if (svg.Contains("viewBox=\\\"") && svg.Contains("\\\">"))
                {
                    input1.Text = svg.Replace("\\\"", "\"");
                    return;
                }
                button1.IconSvg = svg;
                int size = (int)Math.Floor((pictureBox1.Width > pictureBox1.Height ? pictureBox1.Height : pictureBox1.Width) * 0.8F);
                pictureBox1.Image = AntdUI.SvgExtend.SvgToBmp(svg, size, size, Color.Black);
            }
            catch { }
        }

        void button2_Click(object sender, MouseEventArgs e)
        {
            try
            {
                var svg = AntdUI.Helper.ClipboardGetText();
                if (string.IsNullOrEmpty(svg)) return;
                input1.Text = Hand(svg.Trim(), e.Button == MouseButtons.Right);
                AntdUI.Helper.ClipboardSetText(input1.Text);
            }
            catch { }
        }

        string Hand(string svg, bool more = false)
        {
            if (svg.StartsWith("<?xml")) svg = svg.Substring(svg.IndexOf(">") + 1).Trim();
            int i = svg.IndexOf(">") + 1;
            string first = svg.Substring(0, i), last = svg.Substring(i);
            ReId(ref first, "class", "version", "xmlns", "width", "height", "focusable", "data-icon", "aria-hidden", "t");
            ReNodeOne(ref last, "title");
            svg = first + last;
            if (more) ReId(ref svg, "fill");
            ReId(ref svg, "p-id");
            string _tmp = svg.Replace(" >", ">").Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("fill=\"white\"", "fill=\"#fff\"").Replace("fill=\"black\"", "fill=\"#000\"");
            while (_tmp.Contains("  ")) _tmp = _tmp.Replace("  ", " ");
            return _tmp.Replace("> <", "><").Trim();
        }
    }
}