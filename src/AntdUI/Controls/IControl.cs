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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntdUI
{
    [ToolboxItem(false)]
    [Localizable(true)]
    public class IControl : Control, BadgeConfig
    {
        public IControl(ControlType ctype = ControlType.Default)
        {
            switch (ctype)
            {
                case ControlType.Default:
                    SetStyle(ControlStyles.ContainerControl |
                       ControlStyles.AllPaintingInWmPaint |
                       ControlStyles.OptimizedDoubleBuffer |
                       ControlStyles.ResizeRedraw |
                       ControlStyles.DoubleBuffer |
                       ControlStyles.SupportsTransparentBackColor |
                       ControlStyles.UserPaint, true);
                    SetStyle(ControlStyles.Selectable, false);
                    break;
                case ControlType.Select:
                    SetStyle(ControlStyles.ContainerControl | ControlStyles.Selectable |
                       ControlStyles.AllPaintingInWmPaint |
                       ControlStyles.OptimizedDoubleBuffer |
                       ControlStyles.ResizeRedraw |
                       ControlStyles.DoubleBuffer |
                       ControlStyles.SupportsTransparentBackColor |
                       ControlStyles.UserPaint, true);
                    break;
                case ControlType.Button:
                    SetStyle(ControlStyles.ContainerControl | ControlStyles.Selectable |
                       ControlStyles.AllPaintingInWmPaint |
                       ControlStyles.OptimizedDoubleBuffer |
                       ControlStyles.ResizeRedraw |
                       ControlStyles.DoubleBuffer |
                       ControlStyles.SupportsTransparentBackColor |
                       ControlStyles.UserPaint, true);
                    SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
                    break;
            }
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

        /// <summary>
        /// 指示是否已启用该控件
        /// </summary>
        [Description("指示是否已启用该控件"), Category("行为"), DefaultValue(true)]
        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                if (InvokeRequired) Invoke(new Action(() => base.Enabled = value));
                else base.Enabled = value;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            visible = base.Visible;
            base.OnVisibleChanged(e);
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
        public Task Spin(Action<Spin.Config> action, Action? end = null) => Spin(new Spin.Config(), action, end);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="text">加载文本</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public Task Spin(string text, Action<Spin.Config> action, Action? end = null) => Spin(new Spin.Config { Text = text }, action, end);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="config">自定义配置</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public Task Spin(Spin.Config config, Action<Spin.Config> action, Action? end = null) => AntdUI.Spin.open(this, config, action, end);

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
        public virtual Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding);

        internal void IOnSizeChanged()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(IOnSizeChanged));
                return;
            }
            OnSizeChanged(EventArgs.Empty);
        }

        static bool disableDataBinding = false;
#if NET40
        public void OnPropertyChanged(string propertyName)
#else
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
#endif
        {
            if (disableDataBinding) return;
            try
            {
                foreach (Binding it in DataBindings)
                {
                    if (it.PropertyName == propertyName)
                    {
                        it.WriteValue();
                        return;
                    }
                }
            }
            catch (NotSupportedException) { disableDataBinding = true; }
        }

        #region 鼠标

        CursorType oldcursor = CursorType.Default;
        public void SetCursor(bool val) => SetCursor(val ? CursorType.Hand : CursorType.Default);
        public void SetCursor(CursorType cursor = CursorType.Default)
        {
            if (oldcursor == cursor) return;
            oldcursor = cursor;
            bool flag = true;
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
                    flag = false;
                    SetCursor(Cursors.SizeAll);
                    break;
                case CursorType.VSplit:
                    flag = false;
                    SetCursor(Cursors.VSplit);
                    break;
                case CursorType.Default:
                default:
                    SetCursor(DefaultCursor);
                    break;
            }
            SetWindow(flag);
        }
        void SetCursor(Cursor cursor)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetCursor(cursor)));
                return;
            }
            Cursor = cursor;
        }

        bool setwindow = false;
        void SetWindow(bool flag)
        {
            if (setwindow == flag) return;
            setwindow = flag;
            var form = Parent.FindPARENT();
            if (form is BaseForm baseForm) baseForm.EnableHitTest = setwindow;
        }

        [Description("悬停光标"), Category("光标"), DefaultValue(typeof(Cursor), "Hand")]
        public virtual Cursor HandCursor { get; set; } = Cursors.Hand;

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
                int moveX = oldX - x, moveY = oldY - y, moveXa = Math.Abs(moveX), moveYa = Math.Abs(moveY), threshold = (int)(Config.TouchThreshold * Config.Dpi);
                if (mdownd > 0 || (moveXa > threshold || moveYa > threshold))
                {
                    oldMY = moveY;
                    if (mdownd > 0)
                    {
                        if (mdownd == 1) OnTouchScrollY(-moveY);
                        else OnTouchScrollX(-moveX);
                        oldX = x;
                        oldY = y;
                        return false;
                    }
                    else
                    {
                        if (moveYa > moveXa) mdownd = 1;
                        else mdownd = 2;
                        oldX = x;
                        oldY = y;
                        return false;
                    }
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

        const int WM_POINTERDOWN = 0x0246, WM_POINTERUP = 0x0247;
        const int WM_LBUTTONDOWN = 0x0201, WM_LBUTTONUP = 0x0202;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (Config.TouchClickEnabled)
            {
                switch (m.Msg)
                {
                    case WM_POINTERDOWN:
                        Vanara.PInvoke.User32.PostMessage(m.HWnd, WM_LBUTTONDOWN, m.WParam, m.LParam);
                        break;
                    case WM_POINTERUP:
                        Vanara.PInvoke.User32.PostMessage(m.HWnd, WM_LBUTTONUP, m.WParam, m.LParam);
                        break;
                    default:
                        base.WndProc(ref m);
                        return;
                }
            }
            else base.WndProc(ref m);
        }

        #endregion

        #region 拖拽

        /// <summary>
        /// 拖拽文件夹处理
        /// </summary>
        [Description("拖拽文件夹处理"), Category("行为"), DefaultValue(true)]
        public bool HandDragFolder { get; set; } = true;

        protected virtual void OnDragEnter()
        { }
        protected virtual void OnDragLeave()
        { }

        FileDropHandler? fileDrop = null;
        /// <summary>
        /// 使用管理员权限拖拽上传
        /// </summary>
        public void UseAdmin()
        {
            if (fileDrop == null) fileDrop = new FileDropHandler(this);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (DragChanged == null) return;
            if (AllowDrop)
            {
                OnDragEnter();
                if (DragState(e.Data)) e.Effect = DragDropEffects.All;
                else e.Effect = DragDropEffects.None;
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            OnDragLeave();
        }

        internal Func<string[], string[]?>? ONDRAG;
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if (DragChanged == null) return;
            if (DragData(e.Data, out var files))
            {
                if (ONDRAG == null) DragChanged(this, new StringsEventArgs(files));
                else
                {
                    var r = ONDRAG(files);
                    if (r != null) DragChanged(this, new StringsEventArgs(r));
                }
            }
            OnDragLeave();
        }

        bool DragState(IDataObject? Data)
        {
            if (DragData(Data, out var files))
            {
                if (ONDRAG == null) return true;
                else
                {
                    var r = ONDRAG(files);
                    if (r == null) return false;
                    return true;
                }
            }
            return false;
        }

        bool DragData(IDataObject? Data, out string[] files)
        {
            if (Data == null)
            {
                files = new string[0];
                return false;
            }
            foreach (string format in Data.GetFormats())
            {
                if (Data.GetData(format) is string[] tmp && tmp.Length > 0)
                {
                    if (HandDragFolder)
                    {
                        var list = new System.Collections.Generic.List<string>(tmp.Length);
                        foreach (var it in tmp)
                        {
                            if (System.IO.File.Exists(it)) list.Add(it);
                            else list.AddRange(DragDataDirTree(it));
                        }
                        files = list.ToArray();
                    }
                    else files = tmp;
                    return true;
                }
            }
            files = new string[0];
            return false;
        }

        System.Collections.Generic.List<string> DragDataDirTree(string dir)
        {
            var dirinfo = new System.IO.DirectoryInfo(dir);
            var files = dirinfo.GetFiles();
            var dirs = dirinfo.GetDirectories();
            var list = new System.Collections.Generic.List<string>(files.Length + dirs.Length);
            foreach (var it in files) list.Add(it.FullName);
            foreach (var it in dirs) list.AddRange(DragDataDirTree(it.FullName));
            return list;
        }

        #region 事件

        /// <summary>
        /// Bool 类型事件
        /// </summary>
        public delegate void DragEventHandler(object sender, StringsEventArgs e);

        /// <summary>
        /// 文件拖拽后时发生
        /// </summary>
        [Description("文件拖拽后时发生"), Category("行为")]
        public event DragEventHandler? DragChanged = null;
        internal void OnDragChanged(string[] files) => DragChanged?.Invoke(this, new StringsEventArgs(files));

        #endregion

        protected override void Dispose(bool disposing)
        {
            fileDrop?.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }

    public enum ControlType
    {
        Default,
        Select,
        Button
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
        /// 徽标模式（镂空）
        /// </summary>
        bool BadgeMode { get; set; }

        /// <summary>
        /// 徽标背景颜色
        /// </summary>
        Color? BadgeBack { get; set; }

        /// <summary>
        /// 徽标偏移X
        /// </summary>
        int BadgeOffsetX { get; set; }

        /// <summary>
        /// 徽标偏移Y
        /// </summary>
        int BadgeOffsetY { get; set; }
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