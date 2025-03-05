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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntdUI.In
{
    public class FlowLayoutPanel : System.Windows.Forms.FlowLayoutPanel
    {
        ScrollY scrollY;
        public FlowLayoutPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
            UpdateStyles();
            scrollY = new ScrollY(this);
        }

        #region 只为了隐藏滚动条

        #region 滚动条状态

        bool scrollYVisible = false;
        bool ScrollYVisible
        {
            get => scrollYVisible;
            set
            {
                if (scrollYVisible == value) return;
                scrollYVisible = value;
                if (!value) scrollY.SetVrSize(0, 0);
                OnSizeChanged(EventArgs.Empty);
            }
        }
        public override Rectangle DisplayRectangle
        {
            get
            {
                var rect = ClientRectangle.DeflateRect(Padding);
                if (scrollYVisible) return new Rectangle(rect.X, rect.Y - VerticalScroll.Value, rect.Width - scrollY.Rect.Width, rect.Height);
                return rect;
            }
        }

        #endregion

        //https://www.5axxw.com/questions/content/noi6a2
        protected override void OnSizeChanged(EventArgs e)
        {
            LoadScroll();
            base.OnSizeChanged(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            LoadScroll();
            if (ScrollYVisible) scrollY.Paint(e.Graphics.High());
            base.OnPaint(e);
        }

        void LoadScroll()
        {
            ScrollYVisible = VerticalScroll.Visible;

            var rect = ClientRectangle;
            if (ScrollYVisible)
            {
                scrollY.SizeChange(rect);
                if (ScrollYVisible)
                {
                    scrollY.SetVrSize(VerticalScroll.Maximum, rect.Height);
                    scrollY.Value = VerticalScroll.Value;
                }
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                case WM_ERASEBKGND:
                case WM_NCCALCSIZE:
                    if (DesignMode || !AutoScroll) break;
                    ShowScrollBar(Handle, SB_SHOW_BOTH, false);
                    break;
                case WM_MOUSEWHEEL:
                    //int delta = (int)(m.WParam.ToInt64() >> 16);
                    //int direction = Math.Sign(delta);
                    ShowScrollBar(Handle, SB_SHOW_BOTH, false);
                    break;
            }
            base.WndProc(ref m);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (scrollY.MouseDown(e.Location, value => { VerticalScroll.Value = (int)value; })) base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            scrollY.MouseUp(e.Location);
            base.OnMouseUp(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            scrollY.Leave();
            base.OnMouseLeave(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (scrollY.MouseMove(e.Location, value => { VerticalScroll.Value = (int)value; })) base.OnMouseMove(e);
        }

        private const int WM_PAINT = 0x000F;
        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_NCCALCSIZE = 0x0083;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int SB_SHOW_VERT = 0x1;
        private const int SB_SHOW_BOTH = 0x3;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        #endregion
    }
}