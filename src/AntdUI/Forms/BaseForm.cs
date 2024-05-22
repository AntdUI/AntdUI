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
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace AntdUI
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
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

        #region 程序

        /// <summary>
        /// 最小化
        /// </summary>
        public virtual void Min()
        {
            WindowState = FormWindowState.Minimized;
        }

        public virtual bool IsMax
        {
            get => WindowState == FormWindowState.Maximized;
        }

        /// <summary>
        /// 最大化/还原
        /// </summary>
        public virtual bool MaxRestore()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                return false;
            }
            else
            {
                WindowState = FormWindowState.Maximized; return true;
            }
        }

        /// <summary>
        /// 最大化
        /// </summary>
        public virtual void Max()
        {
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 全屏/还原
        /// </summary>
        public virtual bool FullRestore()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                NoFull();
                return false;
            }
            else
            {
                Full();
                return true;
            }
        }

        /// <summary>
        /// 全屏
        /// </summary>
        public virtual void Full()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        public virtual void NoFull()
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;
        }

        #endregion

        public virtual bool AutoHandDpi { get; set; } = true;

        #region DPI

        public float Dpi()
        {
            float dpi = 1F;
#if NET40 || NET46 || NET48
            using (var bmp = new System.Drawing.Bitmap(1, 1))
            {
                using (var g = System.Drawing.Graphics.FromImage(bmp))
                {
                    Config.SetDpi(g);
                    dpi = Config.Dpi;
                }
            }
#else
            dpi = DeviceDpi / 96F;
#endif
            Config.SetDpi(dpi);
            return dpi;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (AutoHandDpi) AutoDpi(Dpi(), this);
            base.OnLoad(e);
        }

        public void AutoDpi(Control control)
        {
            AutoDpi(Dpi(), control);
        }

        public void AutoDpi(float dpi, Control control)
        {
            if (dpi != 1F) Helper.DpiLS(dpi, control);
        }

        #endregion

        #region 交互

        #region 拖动窗口

        /// <summary>
        /// 拖动窗口（鼠标按下）
        /// </summary>
        public virtual void DraggableMouseDown()
        {
            ReleaseCapture();
            SendMessage(Handle, 0x0112, 61456 | 2, IntPtr.Zero);
        }

        #endregion

        #region 调整窗口大小

        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseMove()
        {
            if (WindowState == FormWindowState.Normal)
            {
                var retval = HitTest(PointToClient(MousePosition));
                if (retval != HitTestValues.HTNOWHERE)
                {
                    var mode = retval;
                    if (mode != HitTestValues.HTCLIENT)
                    {
                        SetCursorHit(mode);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 调整窗口大小（鼠标移动）
        /// </summary>
        /// <param name="point">客户端坐标</param>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseMove(Point point)
        {
            if (WindowState == FormWindowState.Normal)
            {
                var retval = HitTest(point);
                if (retval != HitTestValues.HTNOWHERE)
                {
                    var mode = retval;
                    if (mode != HitTestValues.HTCLIENT)
                    {
                        SetCursorHit(mode);
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool is_resizable;
        /// <summary>
        /// 整窗口大小（鼠标按下）
        /// </summary>
        /// <returns>可以调整</returns>
        public virtual bool ResizableMouseDown()
        {
            Point pointScreen = MousePosition;
            var mode = HitTest(PointToClient(pointScreen));
            if (mode != HitTestValues.HTCLIENT)
            {
                is_resizable = true;
                SetCursorHit(mode);
                ReleaseCapture();
                SendMessage(Handle, (uint)WindowMessage.WM_NCLBUTTONDOWN, (IntPtr)mode, Macros.MAKELPARAM(pointScreen.X, pointScreen.Y));
                is_resizable = false;
                return true;
            }
            return false;
        }

        #endregion

        #region 鼠标

        internal HitTestValues HitTest(Point point)
        {
            float htSize = 8F * Config.Dpi, htSize2 = htSize * 2;
            GetWindowRect(Handle, out var lpRect);

            var rect = new Rectangle(Point.Empty, lpRect.Size);

            var hitTestValue = HitTestValues.HTCLIENT;
            var x = point.X;
            var y = point.Y;

            if (x < rect.Left + htSize2 && y < rect.Top + htSize2)
            {
                hitTestValue = HitTestValues.HTTOPLEFT;
            }
            else if (x >= rect.Left + htSize2 && x <= rect.Right - htSize2 && y <= rect.Top + htSize)
            {
                hitTestValue = HitTestValues.HTTOP;
            }
            else if (x > rect.Right - htSize2 && y <= rect.Top + htSize2)
            {
                hitTestValue = HitTestValues.HTTOPRIGHT;
            }
            else if (x <= rect.Left + htSize && y >= rect.Top + htSize2 && y <= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTLEFT;
            }
            else if (x >= rect.Right - htSize && y >= rect.Top * 2 + htSize && y <= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTRIGHT;
            }
            else if (x <= rect.Left + htSize2 && y >= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTBOTTOMLEFT;
            }
            else if (x > rect.Left + htSize2 && x < rect.Right - htSize2 && y >= rect.Bottom - htSize)
            {
                hitTestValue = HitTestValues.HTBOTTOM;
            }
            else if (x >= rect.Right - htSize2 && y >= rect.Bottom - htSize2)
            {
                hitTestValue = HitTestValues.HTBOTTOMRIGHT;
            }

            return hitTestValue;
        }

        internal void SetCursorHit(HitTestValues mode)
        {
            switch (mode)
            {
                case HitTestValues.HTTOP:
                case HitTestValues.HTBOTTOM:
                    LoadCursors(32645);
                    break;
                case HitTestValues.HTLEFT:
                case HitTestValues.HTRIGHT:
                    LoadCursors(32644);
                    break;
                case HitTestValues.HTTOPLEFT:
                case HitTestValues.HTBOTTOMRIGHT:
                    LoadCursors(32642);
                    break;
                case HitTestValues.HTTOPRIGHT:
                case HitTestValues.HTBOTTOMLEFT:
                    LoadCursors(32643);
                    break;
            }
        }

        internal void LoadCursors(int id)
        {
            var handle = LoadCursor(lpCursorName: Macros.MAKEINTRESOURCE(id));
            var oldCursor = User32.SetCursor(handle);
            oldCursor.Close();
        }

        #endregion

        #endregion
    }
}