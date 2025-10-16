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
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntdUI.In
{
    public class FlowLayoutPanel : System.Windows.Forms.FlowLayoutPanel, IScrollBar
    {
        public FlowLayoutPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
            UpdateStyles();
            ScrollBar = new ScrollBar(this);
        }

        #region 属性

        ScrollBar ScrollBar;

        #region 为空

        [Description("是否显示空样式"), Category("外观"), DefaultValue(false)]
        public bool Empty { get; set; }

        [Description("数据为空显示文字"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? EmptyText { get; set; }

        [Description("数据为空显示图片"), Category("外观"), DefaultValue(null)]
        public Image? EmptyImage { get; set; }

        #endregion

        #endregion

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
                if (!value) ScrollBar.SetVrSize(0);
                OnSizeChanged(EventArgs.Empty);
            }
        }
        public override Rectangle DisplayRectangle
        {
            get
            {
                var rect = ClientRectangle.DeflateRect(Padding);
                if (scrollYVisible) rect.Width -= ScrollBar.SIZE;
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
            var g = e.Graphics.High();
            if (Empty && (Controls == null || Controls.Count == 0)) g.PaintEmpty(ClientRectangle, Font, Colour.Text.Get(nameof(FlowLayoutPanel)), EmptyText, EmptyImage);
            if (ScrollYVisible) ScrollBar.Paint(g);
            base.OnPaint(e);
        }

        void LoadScroll()
        {
            ScrollYVisible = VerticalScroll.Visible;

            var rect = ClientRectangle;
            if (ScrollYVisible)
            {
                ScrollBar.SizeChange(rect);
                if (ScrollYVisible)
                {
                    ScrollBar.SetVrSize(VerticalScroll.Maximum);
                    ScrollBar.Value = VerticalScroll.Value;
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
            if (ScrollBar.MouseDown(e.X, e.Y)) base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ScrollBar.MouseUp();
            base.OnMouseUp(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            ScrollBar.Leave();
            base.OnMouseLeave(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ScrollBar.MouseMove(e.X, e.Y)) base.OnMouseMove(e);
        }

        public void OnShowXChanged(bool value)
        {
        }

        public void OnShowYChanged(bool value)
        {
        }

        public void OnValueXChanged(int value)
        {
        }

        public void OnValueYChanged(int value) => VerticalScroll.Value = value;

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