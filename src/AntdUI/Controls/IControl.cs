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

        float badgeSize = .6F;
        [Description("徽标比例"), Category("徽标"), DefaultValue(.6F)]
        public float BadgeSize
        {
            get => badgeSize;
            set
            {
                if (badgeSize != value)
                {
                    badgeSize = value;
                    if (badge != null) Invalidate();
                }
            }
        }

        bool badgeMode = false;
        [Description("徽标模式（镂空）"), Category("徽标"), DefaultValue(false)]
        public bool BadgeMode
        {
            get => badgeMode;
            set
            {
                if (badgeMode != value)
                {
                    badgeMode = value;
                    if (badge != null) Invalidate();
                }
            }
        }

        Color? badgeback = null;
        [Description("徽标背景颜色"), Category("徽标"), DefaultValue(null)]
        public Color? BadgeBack
        {
            get => badgeback;
            set
            {
                if (badgeback != value)
                {
                    badgeback = value;
                    if (badge != null) Invalidate();
                }
            }
        }

        /// <summary>
        /// 徽标偏移X
        /// </summary>
        [Description("徽标偏移X"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetX { get; set; } = 1;

        /// <summary>
        /// 徽标偏移Y
        /// </summary>
        [Description("徽标偏移Y"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetY { get; set; } = 1;

        #endregion

        #endregion

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public void Spin(Action action, Action? end = null)
        {
            Spin(new Spin.Config(), action, end);
        }

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="text">加载文本</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public void Spin(string text, Action action, Action? end = null)
        {
            Spin(new Spin.Config { Text = text }, action, end);
        }

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="config">自定义配置</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public void Spin(Spin.Config config, Action action, Action? end = null)
        {
            AntdUI.Spin.open(this, config, action, end);
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

        internal void IOnSizeChanged()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    IOnSizeChanged();
                }));
                return;
            }
            OnSizeChanged(EventArgs.Empty);
        }


        public void SetCursor(bool val)
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
                g.DrawStr(text, Font, brush, path, stringFormat);
            }
        }
        internal void PaintText(Graphics g, string? text, RectangleF path, StringFormat stringFormat, bool enabled)
        {
            using (var brush = new SolidBrush(enabled ? ForeColor : Style.Db.TextQuaternary))
            {
                g.DrawStr(text, Font, brush, path, stringFormat);
            }
        }

        #endregion

        #endregion

        #region 触屏

        bool mdown = false;
        int mdownd = 0, oldX, oldY;
        protected virtual void OnTouchDown(int x, int y)
        {
            oldMY = 0;
            oldX = x;
            oldY = y;
            if (Config.TouchEnabled)
            {
                mdownd = 0;
                mdown = true;
            }
        }

        int oldMY = 0;
        protected virtual bool OnTouchMove(int x, int y)
        {
            if (mdown)
            {
                int moveX = oldX - x, moveY = oldY - y, moveXa = Math.Abs(moveX), moveYa = Math.Abs(moveY);
                oldMY = moveY;
                if (mdownd > 0)
                {
                    if (mdownd == 1) OnTouchScrollY(-moveY);
                    else OnTouchScrollX(-moveX);
                    oldX = x;
                    oldY = y;
                    return false;
                }
                else if (moveXa > 2 || moveYa > 2)
                {
                    if (moveYa > moveXa)
                    {
                        mdownd = 1;
                        OnTouchScrollY(-moveY);
                    }
                    else
                    {
                        mdownd = 2;
                        OnTouchScrollX(-moveX);
                    }
                    oldX = x;
                    oldY = y;
                    return false;
                }
            }
            return true;
        }
        protected virtual bool OnTouchUp()
        {
            mdown = false;
            if (mdownd > 0)
            {
                if (mdownd == 1)
                {
                    int moveY = oldMY, moveYa = Math.Abs(moveY);
                    if (moveYa > 2)
                    {
                        // 缓冲动画
                        int duration = (int)(moveYa * .1F), incremental = moveYa / 2, sleep = 20;
                        if (moveY > 0)
                        {
                            ITask.Run(() =>
                            {
                                while (moveYa > 0)
                                {
                                    OnTouchScrollY(-incremental);
                                    moveYa -= duration;
                                    System.Threading.Thread.Sleep(sleep);
                                }
                            });
                        }
                        else
                        {
                            ITask.Run(() =>
                            {
                                while (moveYa > 0)
                                {
                                    OnTouchScrollY(incremental);
                                    moveYa -= duration;
                                    System.Threading.Thread.Sleep(sleep);
                                }
                            });
                        }
                    }
                }
                return false;
            }
            return true;
        }
        protected virtual void OnTouchScrollX(int value) { }
        protected virtual void OnTouchScrollY(int value) { }

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