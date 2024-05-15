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
    //绘图层
    public partial class BorderlessFormShadow : Form
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
                cp.Parent = IntPtr.Zero;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get => true;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x21:
                    m.Result = new IntPtr(3);
                    return;
                case 0xa0:
                case 0x200:
                    form.ResizableMouseMove(PointToClient(MousePosition));
                    break;
                case 0xa1:
                case 0x201:
                    form.ResizableMouseDownInternal();
                    break;
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
            if (form.WindowState == FormWindowState.Maximized) Visible = false;
            else
            {
                Visible = true;
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
            int size2 = form.Shadow * 2;
            rect_read = new Rectangle(form.Shadow, form.Shadow, form.Width, form.Height);
            shadow_rect = new Rectangle(form.Left - form.Shadow, form.Top - form.Shadow, rect_read.Width + size2, rect_read.Height + size2);
        }

        #endregion

        #region 渲染

        public void Print()
        {
            if (IsHandleCreated && shadow_rect.Width > 0 && shadow_rect.Height > 0)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        Print();
                    }));
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
            int shadow = form.Shadow, shadow2 = form.Shadow * 2, shadow4 = form.Shadow * 4, shadow6 = form.Shadow * 6;
            if (bitbmp == null)
            {
                bitbmp = new Bitmap(shadow6, shadow6);
                using (var g = Graphics.FromImage(bitbmp).High())
                {
                    using (var path = new Rectangle(shadow, shadow, shadow6 - shadow2, shadow6 - shadow2).RoundPath(form.Radius))
                    {
                        using (var brush = new SolidBrush(form.ShadowColor))
                        {
                            g.FillPath(brush, path);
                        }
                        Helper.Blur(bitbmp, shadow);
                    }
                }
            }
            var bitmap = new Bitmap(shadow_rect.Width, shadow_rect.Height);
            using (var g = Graphics.FromImage(bitmap).High())
            {
                using (var path = rect_read.RoundPath(form.Radius * Config.Dpi))
                {
                    using (var path2 = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path2.AddPath(path, false);
                        path2.AddRectangle(new Rectangle(0, 0, shadow_rect.Width, shadow_rect.Height));
                        g.SetClip(path2);
                    }
                    g.DrawImage(bitbmp, new Rectangle(0, 0, shadow2, shadow2), new Rectangle(0, 0, shadow2, shadow2), GraphicsUnit.Pixel);
                    g.DrawImage(bitbmp, new Rectangle(0, shadow2, shadow, bitmap.Height - shadow4), new Rectangle(0, shadow2, shadow, bitbmp.Height - shadow4), GraphicsUnit.Pixel);
                    g.DrawImage(bitbmp, new Rectangle(0, bitmap.Height - shadow2, shadow2, shadow2), new Rectangle(0, bitbmp.Height - shadow2, shadow2, shadow2), GraphicsUnit.Pixel);

                    g.DrawImage(bitbmp, new Rectangle(shadow2, bitmap.Height - shadow, bitmap.Width - shadow4, shadow), new Rectangle(shadow2, bitbmp.Height - shadow, bitbmp.Width - shadow4, shadow), GraphicsUnit.Pixel);
                    g.DrawImage(bitbmp, new Rectangle(bitmap.Width - shadow2, bitmap.Height - shadow2, shadow2, shadow2), new Rectangle(bitbmp.Width - shadow2, bitbmp.Height - shadow2, shadow2, shadow2), GraphicsUnit.Pixel);

                    g.DrawImage(bitbmp, new Rectangle(bitmap.Width - shadow, shadow2, shadow, bitmap.Height - shadow4), new Rectangle(bitbmp.Width - shadow, shadow2, shadow, bitbmp.Height - shadow4), GraphicsUnit.Pixel);
                    g.DrawImage(bitbmp, new Rectangle(bitmap.Width - shadow2, 0, shadow2, shadow2), new Rectangle(bitbmp.Width - shadow2, 0, shadow2, shadow2), GraphicsUnit.Pixel);

                    g.DrawImage(bitbmp, new Rectangle(shadow2, 0, bitmap.Width - shadow4, shadow), new Rectangle(shadow2, 0, bitbmp.Width - shadow4, shadow), GraphicsUnit.Pixel);

                    g.ResetClip();
                    if (form.BorderWidth > 0)
                    {
                        using (var pen = new Pen(form.BorderColor, form.BorderWidth * Config.Dpi))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
            return bitmap;
        }

        #endregion
    }
}