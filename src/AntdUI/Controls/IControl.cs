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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    [ToolboxItem(false)]
    public class IControl : Control, BadgeConfig
    {
        public IControl()
        {
            SetStyle(
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.DoubleBuffer |
               ControlStyles.SupportsTransparentBackColor |
               ControlStyles.ContainerControl |
               ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        #region 属性

        #region 徽标

        string? badge = null;
        [Description("徽标内容"), Category("徽标"), DefaultValue(null)]
        public string? Badge
        {
            get => badge;
            set
            {
                if (badge == value) return;
                badge = value;
                Invalidate();
            }
        }

        [Description("徽标大小"), Category("徽标"), DefaultValue(9F)]
        public float BadgeSize { get; set; } = 9F;

        [Description("徽标背景颜色"), Category("徽标"), DefaultValue(null)]
        public Color? BadgeBack { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public void Spin(Action action, Action? end = null)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    Spin(action, end);
                }));
                return;
            }
            var frm = new SpinForm(this, new Spin.Config());
            frm.Show(this);
            ITask.Run(() =>
            {
                try
                {
                    action();
                }
                catch { }
                frm.Invoke(new Action(() =>
                {
                    frm.Dispose();
                }));
            }, end);
        }

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="config">自定义配置</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public void Spin(Spin.Config config, Action action, Action? end = null)
        {
            var frm = new SpinForm(this, config);
            frm.Show(this);
            ITask.Run(() =>
            {
                try
                {
                    action();
                }
                catch { }
                frm.Invoke(new Action(() =>
                {
                    frm.Dispose();
                }));
            }, end);
        }

        #region 帮助类

        [Browsable(false)]
        public virtual GraphicsPath RenderRegion
        {
            get
            {
                var path = new GraphicsPath();
                path.AddRectangle(ClientRectangle);
                return path;
            }
        }

        /// <summary>
        /// 真实区域
        /// </summary>
        [Browsable(false)]
        public virtual Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding);
        }
        internal void IOnSizeChanged() { OnSizeChanged(EventArgs.Empty); }


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

        #region 渲染文本

        internal void PaintText(Graphics g, string? text, Rectangle path, StringFormat stringFormat, bool enabled)
        {
            using (var brush = new SolidBrush(enabled ? ForeColor : Style.Db.TextQuaternary))
            {
                g.DrawString(text, Font, brush, path, stringFormat);
            }
        }
        internal void PaintText(Graphics g, string? text, RectangleF path, StringFormat stringFormat, bool enabled)
        {
            using (var brush = new SolidBrush(enabled ? ForeColor : Style.Db.TextQuaternary))
            {
                g.DrawString(text, Font, brush, path, stringFormat);
            }
        }

        #endregion

        #endregion
    }

    public interface BadgeConfig
    {
        /// <summary>
        /// 徽标内容
        /// </summary>
        string? Badge { get; set; }

        /// <summary>
        /// 徽标大小
        /// </summary>
        float BadgeSize { get; set; }

        /// <summary>
        /// 徽标背景颜色
        /// </summary>
        Color? BadgeBack { get; set; }
    }

    public interface ShadowConfig
    {
        /// <summary>
        /// 阴影大小
        /// </summary>
        int Shadow { get; set; }

        /// <summary>
        /// 阴影颜色
        /// </summary>
        Color? ShadowColor { get; set; }

        /// <summary>
        /// 阴影透明度
        /// </summary>
        float ShadowOpacity { get; set; }

        /// <summary>
        /// 阴影偏移X
        /// </summary>
        int ShadowOffsetX { get; set; }

        /// <summary>
        /// 阴影偏移Y
        /// </summary>
        int ShadowOffsetY { get; set; }
    }
}