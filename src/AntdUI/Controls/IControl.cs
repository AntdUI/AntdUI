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
    [Localizable(true)]
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

        bool visible = true;
        /// <summary>
        /// 确定该控件是可见的还是隐藏的
        /// </summary>
        [Description("确定该控件是可见的还是隐藏的"), Category("行为"), DefaultValue(true)]
        public new bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;

                if (InvokeRequired) Invoke(new Action(() => base.Visible = value));
                else base.Visible = value;
            }
        }

        bool enabled = true;
        /// <summary>
        /// 指示是否已启用该控件
        /// </summary>
        [Description("指示是否已启用该控件"), Category("行为"), DefaultValue(true)]
        public new bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;
                enabled = value;

                if (InvokeRequired) Invoke(new Action(() => base.Enabled = value));
                else base.Enabled = value;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            visible = base.Visible;
            base.OnVisibleChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            enabled = base.Enabled;
            base.OnEnabledChanged(e);
        }

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

        string? badgeSvg = null;
        [Description("徽标SVG"), Category("徽标"), DefaultValue(null)]
        public string? BadgeSvg
        {
            get => badgeSvg;
            set
            {
                if (badgeSvg == value) return;
                badgeSvg = value;
                Invalidate();
            }
        }

        TAlignFrom badgeAlign = TAlignFrom.TR;
        [Description("徽标方向"), Category("徽标"), DefaultValue(TAlignFrom.TR)]
        public TAlignFrom BadgeAlign
        {
            get => badgeAlign;
            set
            {
                if (badgeAlign == value) return;
                badgeAlign = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        float badgeSize = .6F;
        [Description("徽标比例"), Category("徽标"), DefaultValue(.6F)]
        public float BadgeSize
        {
            get => badgeSize;
            set
            {
                if (badgeSize == value) return;
                badgeSize = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        bool badgeMode = false;
        [Description("徽标模式（镂空）"), Category("徽标"), DefaultValue(false)]
        public bool BadgeMode
        {
            get => badgeMode;
            set
            {
                if (badgeMode == value) return;
                badgeMode = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        Color? badgeback = null;
        [Description("徽标背景颜色"), Category("徽标"), DefaultValue(null)]
        public Color? BadgeBack
        {
            get => badgeback;
            set
            {
                if (badgeback == value) return;
                badgeback = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        int badgeOffsetX = 1, badgeOffsetY = 1;
        /// <summary>
        /// 徽标偏移X
        /// </summary>
        [Description("徽标偏移X"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetX
        {
            get => badgeOffsetX;
            set
            {
                if (badgeOffsetX == value) return;
                badgeOffsetX = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        /// <summary>
        /// 徽标偏移Y
        /// </summary>
        [Description("徽标偏移Y"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetY
        {
            get => badgeOffsetY;
            set
            {
                if (badgeOffsetY == value) return;
                badgeOffsetY = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

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

        #region 鼠标

        CursorType oldcursor = CursorType.Default;
        public void SetCursor(bool val) => SetCursor(val ? CursorType.Hand : CursorType.Default);
        public void SetCursor(CursorType cursor = CursorType.Default)
        {
            if (oldcursor == cursor) return;
            oldcursor = cursor;
            switch (cursor)
            {
                case CursorType.Hand:
                    SetCursor(HandCursor);
                    break;
                case CursorType.IBeam:
                    SetCursor(Cursors.IBeam);
                    break;
                case CursorType.No:
                    SetCursor(Cursors.No);
                    break;
                case CursorType.SizeAll:
                    SetCursor(Cursors.SizeAll);
                    break;
                case CursorType.VSplit:
                    SetCursor(Cursors.VSplit);
                    break;
                case CursorType.Default:
                default:
                    SetCursor(DefaultCursor);
                    break;
            }
        }
        void SetCursor(Cursor cursor)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    SetCursor(cursor);
                }));
                return;
            }
            Cursor = cursor;
        }

        [Description("悬停光标"), Category("光标"), DefaultValue(typeof(Cursor), "Hand")]
        public virtual Cursor HandCursor { get; set; } = Cursors.Hand;

        #endregion

        #region 渲染文本

        internal void PaintText(Canvas g, string? text, Rectangle path, StringFormat stringFormat, bool enabled)
        {
            using (var brush = new SolidBrush(enabled ? ForeColor : Style.Db.TextQuaternary))
            {
                g.String(text, Font, brush, path, stringFormat);
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
                taskTouch?.Dispose();
                taskTouch = null;
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

        ITask? taskTouch = null;
        protected virtual bool OnTouchUp()
        {
            taskTouch?.Dispose();
            taskTouch = null;
            mdown = false;
            if (mdownd > 0)
            {
                if (mdownd == 1)
                {
                    int moveY = oldMY, moveYa = Math.Abs(moveY);
                    if (moveYa > 10)
                    {
                        // 缓冲动画
                        int duration = (int)Math.Ceiling(moveYa * .1F), incremental = moveYa / 2, sleep = 20;
                        if (moveY > 0)
                        {
                            taskTouch = new ITask(this, () =>
                            {
                                if (moveYa > 0 && OnTouchScrollY(-incremental))
                                {
                                    moveYa -= duration;
                                    return true;
                                }
                                return false;
                            }, sleep);
                        }
                        else
                        {
                            taskTouch = new ITask(this, () =>
                            {
                                if (moveYa > 0 && OnTouchScrollY(incremental))
                                {
                                    moveYa -= duration;
                                    return true;
                                }
                                return false;
                            }, sleep);
                        }
                    }
                }
                return false;
            }
            return true;
        }
        protected void OnTouchCancel()
        {
            taskTouch?.Dispose();
            taskTouch = null;
            mdown = false;
        }
        protected virtual bool OnTouchScrollX(int value) => false;
        protected virtual bool OnTouchScrollY(int value) => false;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            taskTouch?.Dispose();
            taskTouch = null;
            base.OnMouseWheel(e);
        }

        #endregion
    }

    public enum CursorType
    {
        Default,
        Hand,
        IBeam,
        No,
        SizeAll,
        VSplit,
    }

    public interface BadgeConfig
    {
        /// <summary>
        /// 徽标内容
        /// </summary>
        string? Badge { get; set; }

        /// <summary>
        /// 徽标SVG
        /// </summary>
        string? BadgeSvg { get; set; }

        /// <summary>
        /// 徽标方向
        /// </summary>
        TAlignFrom BadgeAlign { get; set; }

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