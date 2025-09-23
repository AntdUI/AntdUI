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

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormMask : ILayeredFormOpacity
    {
        int Radius = 0, Bor = 0;
        bool HasBor = false;
        Form form;
        Control? control;
        public LayeredFormMask(Form _form) : base(240)
        {
            form = _form;
            TopMost = _form.TopMost;
            HasBor = form.FormFrame(out Radius, out Bor);
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
        public LayeredFormMask(Form _form, Control _control) : base(240)
        {
            form = _form;
            control = _control;
            TopMost = _form.TopMost;
            var point = _control.PointToScreen(Point.Empty);
            SetSize(_control.Size);
            SetLocation(point);
            Size = _control.Size;
            Location = point;
            if (_control is IControl icontrol) RenderRegion = () => icontrol.RenderRegion;
        }

        public override string name => "Mask";
        Func<GraphicsPath>? RenderRegion;

        protected override void OnLoad(EventArgs e)
        {
            if (control == null)
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
            }
            else
            {
                var point = control.PointToScreen(Point.Empty);
                SetLocation(point);
                SetSize(control.Size);
                Size = control.Size;
                Location = point;
            }
            form.LocationChanged += Parent_LocationChanged;
            form.SizeChanged += Parent_SizeChanged;
            base.OnLoad(e);
        }

        private void Parent_LocationChanged(object? sender, EventArgs e)
        {
            if (control == null)
            {
                if (form is Window window)
                {
                    SetLocation(window.Location);
                    Location = window.Location;
                }
                else
                {
                    SetLocation(form.Location);
                    Location = form.Location;
                }
            }
            else
            {
                var point = control.PointToScreen(Point.Empty);
                SetLocation(point);
                Location = point;
            }
            PrintCache();
        }
        private void Parent_SizeChanged(object? sender, EventArgs e)
        {
            if (control == null)
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
            }
            else
            {
                var point = control.PointToScreen(Point.Empty);
                SetLocation(point);
                SetSize(control.Size);
                Size = control.Size;
                Location = point;
            }
            temp?.Dispose();
            temp = null;
            Print();
        }

        protected override void Dispose(bool disposing)
        {
            form.LocationChanged -= Parent_LocationChanged;
            form.SizeChanged -= Parent_SizeChanged;
            temp?.Dispose();
            temp = null;
            base.Dispose(disposing);
        }

        Bitmap? temp;
        public override Bitmap PrintBit()
        {
            Rectangle rect_read = TargetRectXY, rect = HasBor ? new Rectangle(Bor, 0, rect_read.Width - Bor * 2, rect_read.Height - Bor) : rect_read;
            if (temp == null || (temp.Width != rect_read.Width || temp.Height != rect_read.Height))
            {
                temp?.Dispose();
                temp = new Bitmap(rect_read.Width, rect_read.Height);
                using (var g = Graphics.FromImage(temp).High())
                {
                    using (var brush = new SolidBrush(Color.FromArgb(115, 0, 0, 0)))
                    {
                        if (RenderRegion == null)
                        {
                            if (Radius > 0)
                            {
                                using (var path = rect.RoundPath(Radius))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, rect);
                        }
                        else
                        {
                            using (var path = RenderRegion())
                            {
                                g.Fill(brush, path);
                            }
                        }
                    }
                }
            }
            return new Bitmap(temp);
        }
    }

    public interface FormNoBar
    {
    }
}