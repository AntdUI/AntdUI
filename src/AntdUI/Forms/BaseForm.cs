﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
using System.Windows.Forms;

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
        public void Min()
        {
            WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 最大化/还原
        /// </summary>
        public void MaxRestore()
        {
            if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
            else WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// 最大化
        /// </summary>
        public void Max()
        {
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 全屏/还原
        /// </summary>
        public void FullRestore()
        {
            if (WindowState == FormWindowState.Maximized) NoFull();
            else Full();
        }

        /// <summary>
        /// 全屏
        /// </summary>
        public void Full()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        public void NoFull()
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
            if (dpi != 1F)
            {
                var dir = Helper.DpiSuspend(control.Controls);
                Helper.DpiLS(dpi, control);
                Helper.DpiResume(dir, control.Controls);
            }
        }

        #endregion
    }
}