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
    internal class BorderlessFormShadow : Form
    {
        BorderlessForm form;
        //带参构造
        public BorderlessFormShadow(BorderlessForm main)
        {
            form = main;
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            UpdateStyles();
            //置顶窗体
            //TopMost = BorderlessForm.TopMost;
            //BorderlessForm.BringToFront();
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            Icon = form.Icon;
            ShowIcon = false;
            Text = form.Text;
            ISize();
        }

        #region 无焦点窗体

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x08000000 | 0x00080000;
                if (form != null && form.ShadowPierce) cp.ExStyle |= 0x20;
                cp.Parent = IntPtr.Zero;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation => true;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (form.Resizable)
            {
                switch (m.Msg)
                {
                    case 0x21:
                        m.Result = new IntPtr(3);
                        return;
                    case 0xa0:
                    case 0x200:
                        if (form.ResizableMouseMove(PointToClient(MousePosition))) return;
                        break;
                    case 0xa1:
                    case 0x201:
                        if (form.ResizableMouseDown()) return;
                        break;
                }
            }
            base.WndProc(ref m);
        }

        #endregion

        #region 初始化

        public void ClearShadow()
        {
            bitbmp?.Dispose();
            bitbmp = null;
        }

        public void OnSizeChange()
        {
            if (form.IsMax) Visible = false;
            else
            {
                if (form.Visible) Visible = true;
                ISize();
                Print();
            }
        }
        public void OnLocationChange()
        {
            ISize();
            Print();
        }

        Rectangle shadow_rect = new Rectangle(0, 0, 0, 0), rect_read;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ISize();
            Print();
        }

        #endregion

        #region 控件层相关事件

        public void ISize()
        {
            int shadow = (int)(form.Shadow * Config.Dpi), shadow2 = shadow * 2;
            rect_read = new Rectangle(shadow, shadow, form.Width, form.Height);
            shadow_rect = new Rectangle(form.Left - shadow, form.Top - shadow, rect_read.Width + shadow2, rect_read.Height + shadow2);
        }

        #endregion

        #region 渲染

        public void Print()
        {
            if (IsHandleCreated && shadow_rect.Width > 0 && shadow_rect.Height > 0)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(Print));
                    return;
                }
                try
                {
                    using (var bmp = PrintBit())
                    {
                        if (bmp == null) return;
                        Win32.SetBits(bmp, shadow_rect, Handle, 255);
                    }
                    GC.Collect();
                }
                catch { }
            }
        }

        Bitmap? bitbmp = null;
        Bitmap PrintBit()
        {
            int radius = (int)(form.Radius * Config.Dpi), shadow = (int)(form.Shadow * Config.Dpi), shadow2 = shadow * 2, shadow4 = shadow * 4, shadow6 = shadow * 6;
            if (bitbmp == null)
            {
                bitbmp = new Bitmap(shadow6, shadow6);
                using (var g = Graphics.FromImage(bitbmp).High())
                {
                    using (var path = new Rectangle(shadow, shadow, shadow6 - shadow2, shadow6 - shadow2).RoundPath(radius))
                    {
                        g.Fill(form.ShadowColor, path);
                        Helper.Blur(bitbmp, shadow);
                    }
                }
            }
            var bitmap = new Bitmap(shadow_rect.Width, shadow_rect.Height);
            using (var g = Graphics.FromImage(bitmap).High())
            {
                using (var path = rect_read.RoundPath(radius))
                {
                    using (var path2 = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path2.AddPath(path, false);
                        path2.AddRectangle(new Rectangle(0, 0, shadow_rect.Width, shadow_rect.Height));
                        g.SetClip(path2);
                    }
                    int r = shadow2 + radius;

                    g.Image(bitbmp, new Rectangle(0, 0, shadow2, shadow2), new Rectangle(0, 0, shadow2, shadow2), GraphicsUnit.Pixel);
                    g.Image(bitbmp, new Rectangle(0, shadow2, shadow, bitmap.Height - shadow4), new Rectangle(0, shadow2, shadow, bitbmp.Height - shadow4), GraphicsUnit.Pixel);
                    g.Image(bitbmp, new Rectangle(0, bitmap.Height - shadow2, shadow2, shadow2), new Rectangle(0, bitbmp.Height - shadow2, shadow2, shadow2), GraphicsUnit.Pixel);

                    g.Image(bitbmp, new Rectangle(shadow2, bitmap.Height - shadow, bitmap.Width - shadow4, shadow), new Rectangle(shadow2, bitbmp.Height - shadow, bitbmp.Width - shadow4, shadow), GraphicsUnit.Pixel);
                    g.Image(bitbmp, new Rectangle(bitmap.Width - shadow2, bitmap.Height - shadow2, shadow2, shadow2), new Rectangle(bitbmp.Width - shadow2, bitbmp.Height - shadow2, shadow2, shadow2), GraphicsUnit.Pixel);

                    g.Image(bitbmp, new Rectangle(bitmap.Width - shadow, shadow2, shadow, bitmap.Height - shadow4), new Rectangle(bitbmp.Width - shadow, shadow2, shadow, bitbmp.Height - shadow4), GraphicsUnit.Pixel);
                    g.Image(bitbmp, new Rectangle(bitmap.Width - shadow2, 0, shadow2, shadow2), new Rectangle(bitbmp.Width - shadow2, 0, shadow2, shadow2), GraphicsUnit.Pixel);

                    g.Image(bitbmp, new Rectangle(shadow2, 0, bitmap.Width - shadow4, shadow), new Rectangle(shadow2, 0, bitbmp.Width - shadow4, shadow), GraphicsUnit.Pixel);

                    g.ResetClip();
                    if (form.BorderWidth > 0) g.Draw(form.BorderColor, form.BorderWidth * Config.Dpi, path);
                }
            }
            return bitmap;
        }

        #endregion
    }
}