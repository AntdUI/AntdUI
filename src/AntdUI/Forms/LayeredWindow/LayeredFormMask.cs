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
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormMask : ILayeredFormOpacity
    {
        int Radius = 0, Bor = 0;
        bool HasBor = false;
        Form form;
        public LayeredFormMask(Form _form)
        {
            form = _form;
            TopMost = _form.TopMost;
            if (form.WindowState != FormWindowState.Maximized)
            {
                if (form is BorderlessForm borderless) Radius = (int)(borderless.Radius * Config.Dpi);
                else if (Helper.OSVersionWin11) Radius = (int)(8 * Config.Dpi); //Win11
                if (form is Window || form is FormNoBar)
                {
                    //无边框处理
                }
                else if (form.FormBorderStyle != FormBorderStyle.None)
                {
                    HasBor = true;
                    Bor = (int)(7 * Config.Dpi);
                }
            }
            if (form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                Size = form.Size;
                Location = form.Location;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                Size = form.Size;
                Location = form.Location;
            }
            form.LocationChanged += Form_LSChanged;
            form.SizeChanged += Form_LSChanged;
            base.OnLoad(e);
        }

        private void Form_LSChanged(object? sender, EventArgs e)
        {
            if (form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                Size = form.Size;
                Location = form.Location;
            }
            temp?.Dispose(); temp = null;
            Print();
        }

        protected override void Dispose(bool disposing)
        {
            form.LocationChanged -= Form_LSChanged;
            form.SizeChanged -= Form_LSChanged;
            temp?.Dispose();
            temp = null;
            base.Dispose(disposing);
        }
        Bitmap? temp = null;
        public override Bitmap PrintBit()
        {
            Rectangle rect;
            if (HasBor) rect = new Rectangle(Bor, 0, TargetRect.Width - Bor * 2, TargetRect.Height - Bor);
            else rect = TargetRectXY;
            if (temp == null || (temp.Width != TargetRect.Width || temp.Height != TargetRect.Height))
            {
                temp?.Dispose();
                temp = new Bitmap(TargetRect.Width, TargetRect.Height);
                using (var g = Graphics.FromImage(temp).High())
                {
                    using (var brush = new SolidBrush(Color.FromArgb(115, 0, 0, 0)))
                    {
                        if (Radius > 0)
                        {
                            using (var path = rect.RoundPath(Radius))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else g.FillRectangle(brush, rect);
                    }
                }
            }
            return (Bitmap)temp.Clone();
        }
    }

    public interface FormNoBar
    {
    }
}