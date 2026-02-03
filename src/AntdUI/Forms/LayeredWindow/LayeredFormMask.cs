// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
        Form owner;
        Form? form;
        Control? control;
        public LayeredFormMask(Form _owner) : base(240)
        {
            owner = _owner;
            TopMost = _owner.TopMost;
            SetDpi(_owner);
            HasBor = owner.FormFrame(out Radius, out Bor);
            if (owner is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(owner.Size);
                SetLocation(owner.Location);
                Size = owner.Size;
                Location = owner.Location;
            }
        }
        public LayeredFormMask(Form _owner, Control _control) : base(240)
        {
            owner = _owner;
            control = _control;
            SetDpi(_owner, _control);
            TopMost = _owner.TopMost;
            var point = _control.PointToScreen(Point.Empty);
            SetSize(_control.Size);
            SetLocation(point);
            Size = _control.Size;
            Location = point;
            if (_control is IControl icontrol) RenderRegion = () => icontrol.RenderRegion;
        }

        public LayeredFormMask SetForm(Form _form)
        {
            form = _form;
            return this;
        }

        public override string name => "Mask";
        Func<GraphicsPath>? RenderRegion;

        Control[]? list;
        protected override void OnLoad(EventArgs e)
        {
            if (control == null)
            {
                if (owner is Window window)
                {
                    SetSize(window.Size);
                    SetLocation(window.Location);
                    Size = window.Size;
                    Location = window.Location;
                }
                else
                {
                    SetSize(owner.Size);
                    SetLocation(owner.Location);
                    Size = owner.Size;
                    Location = owner.Location;
                }
                owner.VisibleChanged += Parent_VisibleChanged;
            }
            else
            {
                var point = control.PointToScreen(Point.Empty);
                SetLocation(point);
                SetSize(control.Size);
                Size = control.Size;
                Location = point;
                var tmps = control.FindPARENTs();
                list = tmps.ToArray();
                foreach (var control in list)
                {
                    if (control is TabPage page) page.ShowedChanged += Parent_VisibleChanged;
                    control.VisibleChanged += Parent_VisibleChanged;
                    control.Disposed += Parent_Disposed;
                }
            }
            owner.LocationChanged += Parent_LocationChanged;
            owner.SizeChanged += Parent_SizeChanged;
            LoadVisible();
            base.OnLoad(e);
        }


        private void Parent_Disposed(object? sender, EventArgs e) => IClose();
        private void Parent_VisibleChanged(object? sender, EventArgs e) => LoadVisible();
        private void Parent_LocationChanged(object? sender, EventArgs e)
        {
            if (control == null)
            {
                if (owner is Window window)
                {
                    SetLocation(window.Location);
                    Location = window.Location;
                }
                else
                {
                    SetLocation(owner.Location);
                    Location = owner.Location;
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
                if (owner is Window window)
                {
                    SetSize(window.Size);
                    SetLocation(window.Location);
                    Size = window.Size;
                    Location = window.Location;
                }
                else
                {
                    SetSize(owner.Size);
                    SetLocation(owner.Location);
                    Size = owner.Size;
                    Location = owner.Location;
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

        bool visible = false;
        void LoadVisible()
        {
            var tmp = GetVisible();
            if (visible == tmp) return;
            visible = tmp;
            temp?.Dispose();
            temp = null;
            Print();
        }
        bool GetVisible()
        {
            if (list == null) return owner.Visible;
            else
            {
                foreach (var control in list)
                {
                    if (control is TabPage page)
                    {
                        if (!page.Showed) return false;
                    }
                    else if (!control.Visible) return false;
                }
                return true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (list == null) owner.VisibleChanged -= Parent_VisibleChanged;
            else
            {
                foreach (var control in list)
                {
                    control.Disposed -= Parent_Disposed;
                    control.VisibleChanged -= Parent_VisibleChanged;
                    if (control is TabPage page) page.ShowedChanged -= Parent_VisibleChanged;
                }
            }
            owner.LocationChanged -= Parent_LocationChanged;
            owner.SizeChanged -= Parent_SizeChanged;
            temp?.Dispose();
            temp = null;
            base.Dispose(disposing);
        }

        Bitmap? temp;
        public override Bitmap? PrintBit()
        {
            Rectangle rect_read = TargetRectXY, rect = HasBor ? new Rectangle(Bor, 0, rect_read.Width - Bor * 2, rect_read.Height - Bor) : rect_read;
            if (temp == null || (temp.Width != rect_read.Width || temp.Height != rect_read.Height))
            {
                temp?.Dispose();
                temp = new Bitmap(rect_read.Width, rect_read.Height);
                if (visible)
                {
                    using (var g = Graphics.FromImage(temp).High(Dpi))
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
            }
            if (temp == null) return null;
            return new Bitmap(temp);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (form == null) return;
            try
            {
                form.Close();
            }
            catch { }
            IClose();
        }
    }

    public interface FormNoBar
    {
    }
}