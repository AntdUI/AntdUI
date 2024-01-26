// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
        public LayeredFormMask(Form form)
        {
            var version = Environment.OSVersion.Version;
            if (version.Major >= 10 && version.Build > 22000) Radius = 8; //Win11
            if (form is Window)
            {
                //无边框处理
            }
            else if (form.FormBorderStyle != FormBorderStyle.None && form.WindowState != FormWindowState.Maximized)
            {
                HasBor = true;
                Bor = 8;
            }
            Tag = form;
            SetSize(form.Size);
            SetLocation(form.Location);
            Size = form.Size;
            Location = form.Location;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Tag is Form form)
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                Size = form.Size;
                Location = form.Location;
            }
            base.OnLoad(e);
        }

        public override Bitmap PrintBit()
        {
            Rectangle rect;
            if (HasBor) rect = new Rectangle(Bor, 0, TargetRect.Width - Bor * 2, TargetRect.Height - Bor);
            else rect = TargetRectXY;
            var original_bmp = new Bitmap(TargetRect.Width, TargetRect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
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
            return original_bmp;
        }
    }
}