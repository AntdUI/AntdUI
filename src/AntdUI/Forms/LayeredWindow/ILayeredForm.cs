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
    public abstract class ILayeredForm : Form, IMessageFilter
    {
        public ILayeredForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            UpdateStyles();
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Size = new Size(0, 0);
        }

        internal Control? PARENT = null;
        internal Func<Keys, bool>? KeyCall = null;

        internal virtual bool CanLoadMessage { get; set; } = true;
        internal virtual void LoadMessage()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LoadMessage));
                return;
            }
            if (MessageEnable) Application.AddMessageFilter(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (CanLoadMessage) LoadMessage();
        }

        bool can_render = true;
        protected override void Dispose(bool disposing)
        {
            can_render = false;
            base.Dispose(disposing);
            Application.RemoveMessageFilter(this);
        }

        public virtual bool UFocus => true;

        public abstract Bitmap PrintBit();
        public byte alpha = 10;

        #region 渲染坐标

        Rectangle target_rect = new Rectangle(-1000, -1000, 0, 0);
        /// <summary>
        /// 目标区域
        /// </summary>
        public Rectangle TargetRect
        {
            get => target_rect;
        }
        public Rectangle TargetRectXY
        {
            get => new Rectangle(0, 0, target_rect.Width, target_rect.Height);
        }

        public void SetRect(Rectangle rect)
        {
            target_rect.X = rect.X;
            target_rect.Y = rect.Y;
            target_rect.Width = rect.Width;
            target_rect.Height = rect.Height;
        }
        public void SetSize(Size size)
        {
            target_rect.Width = size.Width;
            target_rect.Height = size.Height;
        }
        public void SetSize(int w, int h)
        {
            target_rect.Width = w;
            target_rect.Height = h;
        }
        public void SetSize(int size)
        {
            target_rect.Width = target_rect.Height = size;
        }
        public void SetSizeW(int w)
        {
            target_rect.Width = w;
        }
        public void SetSizeH(int h)
        {
            target_rect.Height = h;
        }

        public void SetLocation(Point point)
        {
            target_rect.X = point.X;
            target_rect.Y = point.Y;
        }
        public void SetLocationX(int x)
        {
            target_rect.X = x;
        }
        public void SetLocationY(int y)
        {
            target_rect.Y = y;
        }
        public void SetLocation(int x, int y)
        {
            target_rect.X = x;
            target_rect.Y = y;
        }

        #endregion

        public void Print()
        {
            if (IsHandleCreated && can_render && target_rect.Width > 0 && target_rect.Height > 0)
            {
                try
                {
                    using (var bmp = PrintBit())
                    {
                        if (bmp == null) return;
                        Render(bmp);
                    }
                    GC.Collect();
                }
                catch { }
            }
        }

        internal void Render(Bitmap bmp)
        {
            try
            {
                if (InvokeRequired) Invoke(new Action(() => { Render(bmp); }));
                else Win32.SetBits(bmp, target_rect, Handle, alpha);
            }
            catch { }
        }

        internal void SetCursor(bool val)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    SetCursor(val);
                }));
                return;
            }
            Cursor = val ? Cursors.Hand : DefaultCursor;
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
            get => UFocus;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (UFocus && m.Msg == 0x21)
            {
                m.Result = new IntPtr(3);
                return;
            }
            base.WndProc(ref m);
        }

        #endregion

        public void IClose(bool isdispose = false)
        {
            if (IsHandleCreated)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            IClose(isdispose);
                        }));
                        return;
                    }
                    Close();
                    if (isdispose) Dispose();
                }
                catch { }
            }
        }

        /// <summary>
        /// 点击外面关闭使能
        /// </summary>
        public virtual bool MessageEnable => false;
        public virtual bool MessageCloseSub => false;

        /// <summary>
        /// 鼠标离开关闭
        /// </summary>
        public bool MessageCloseMouseLeave { get; set; }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            //if (m.Msg == 0x31f || m.Msg == 0xc31a || m.Msg == 0x60 || m.Msg == 0xf || m.Msg == 0xc0a2 || m.Msg == 0x118 || m.Msg == 0x113) return false;
            //0x2a1 (WM_MOUSEHOVER)
            //0x2a3 (WM_MOUSELEAVE)
            if ((m.Msg == 0x201 || m.Msg == 0xa0))
            {
                var mousePosition = MousePosition;
                if (!target_rect.Contains(mousePosition))
                {
                    try
                    {
                        if (PARENT != null && PARENT.IsHandleCreated)
                        {
                            if (ContainsPosition(PARENT, mousePosition)) return false;
                            if (new Rectangle(PARENT.PointToScreen(Point.Empty), PARENT.Size).Contains(mousePosition)) return false;

                            #region 判断内容

                            if (MessageCloseSub && FunSub(PARENT, mousePosition)) return false;

                            #endregion
                        }
                        IClose();
                    }
                    catch { }
                    return false;
                }
            }
            else if (m.Msg == 0x2a3 && MessageCloseMouseLeave)
            {
                var mousePosition = MousePosition;
                if (!target_rect.Contains(mousePosition))
                {
                    try
                    {
                        if (PARENT != null && PARENT.IsHandleCreated)
                        {
                            if (ContainsPosition(PARENT, mousePosition)) return false;
                            if (new Rectangle(PARENT.PointToScreen(Point.Empty), PARENT.Size).Contains(mousePosition)) return false;

                            #region 判断内容

                            if (MessageCloseSub && FunSub(PARENT, mousePosition)) return false;

                            #endregion
                        }
                        IClose();
                    }
                    catch { }
                    return false;
                }
            }
            else if (m.Msg == 0x100 && KeyCall != null)
            {
                //0x100 (WM_KEYDOWN) 
                //0x101 (WM_KEYUP)
                var keys = (Keys)(int)m.WParam;
                return KeyCall.Invoke(keys);
            }
            return false;
        }

        bool FunSub(Control control, Point mousePosition)
        {
            if (control is SubLayeredForm subForm)
            {
                var subform = subForm.SubForm();
                if (subform != null && ContainsPosition(subform, mousePosition) > 0) return true;
            }
            if (control.Controls == null || control.Controls.Count == 0) return false;
            foreach (Control it in control.Controls)
            {
                if (FunSub(it, mousePosition)) return true;
            }
            return false;
        }

        bool ContainsPosition(Control control, Point mousePosition)
        {
            if (new Rectangle(control.PointToScreen(Point.Empty), control.Size).Contains(mousePosition)) return true;
            try
            {
                if (control is SubLayeredForm subForm)
                {
                    var subform = subForm.SubForm();
                    if (subform != null && ContainsPosition(subform, mousePosition) > 0) return true;
                }
            }
            catch { }

            return false;
        }

        int ContainsPosition(ILayeredForm control, Point mousePosition)
        {
            int count = 0;
            try
            {
                if (control.TargetRect.Contains(mousePosition)) count++;
                if (control is LayeredFormSelectDown layered && layered.SubForm != null) count += ContainsPosition(layered.SubForm, mousePosition);
            }
            catch { }
            return count;
        }
    }

    internal interface SubLayeredForm
    {
        ILayeredForm? SubForm();
    }
}