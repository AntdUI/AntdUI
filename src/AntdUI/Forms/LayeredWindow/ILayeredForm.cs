// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public abstract class ILayeredForm : Form, IMessage
    {
        IntPtr? handle;
        IntPtr memDc;
        public ILayeredForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            InitDpi();
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Size = new Size(0, 0);
            actionLoadMessage = LoadMessage;
            actionCursor = val => SetCursor(val);
            memDc = Win32.CreateCompatibleDC(Win32.screenDC);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            handle = Handle;
            base.OnHandleCreated(e);
        }

        public Control? PARENT;
        public Func<Keys, bool>? KeyCall;

        Action actionLoadMessage;
        MessageHandler? messageHandler;
        public virtual void LoadMessage()
        {
            if (messageHandler == null)
            {
                if (InvokeRequired)
                {
                    Invoke(actionLoadMessage);
                    return;
                }
                if (CloseMode == CloseMode.None) return;
                messageHandler = new MessageHandler(this);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (CanLoadMessage) LoadMessage();
        }

        bool FunRun = true;
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch { }
            handle = null;
            FunRun = false;
            messageHandler?.Dispose();
            messageHandler = null;
            Win32.Dispose(memDc, ref hBitmap, ref oldBits);
            if (memDc == IntPtr.Zero) return;
            Win32.DeleteDC(memDc);
            memDc = IntPtr.Zero;
        }

        public virtual bool UFocus => true;

        public byte alpha = 10;

        public bool CanRender(out IntPtr han)
        {
            if (handle.HasValue && FunRun && target_rect.Width > 0 && target_rect.Height > 0)
            { han = handle.Value; return true; }
            han = IntPtr.Zero;
            return false;
        }

        #region 渲染坐标

        Rectangle target_rect = new Rectangle(-1000, -1000, 0, 0);
        /// <summary>
        /// 目标区域
        /// </summary>
        public Rectangle TargetRect => target_rect;
        public Rectangle TargetRectXY => new Rectangle(0, 0, target_rect.Width, target_rect.Height);

        public virtual void SetRect(Rectangle rect)
        {
            target_rect.X = rect.X;
            target_rect.Y = rect.Y;
            target_rect.Width = rect.Width;
            target_rect.Height = rect.Height;
        }
        public virtual void SetSize(Size size)
        {
            target_rect.Width = size.Width;
            target_rect.Height = size.Height;
        }
        public virtual void SetSize(int w, int h)
        {
            target_rect.Width = w;
            target_rect.Height = h;
        }
        public virtual void SetSize(int size)
        {
            target_rect.Width = target_rect.Height = size;
        }
        public virtual void SetSizeW(int w)
        {
            target_rect.Width = w;
        }
        public virtual void SetSizeH(int h)
        {
            target_rect.Height = h;
        }

        public virtual void SetLocation(Point point)
        {
            target_rect.X = point.X;
            target_rect.Y = point.Y;
        }
        public virtual void SetLocationX(int x)
        {
            target_rect.X = x;
        }
        public virtual void SetLocationY(int y)
        {
            target_rect.Y = y;
        }
        public virtual void SetLocation(int x, int y)
        {
            target_rect.X = x;
            target_rect.Y = y;
        }

        #endregion

        public abstract Bitmap? PrintBit();
        public Bitmap? Printmap()
        {
            RenderCache = false;
            Win32.Dispose(memDc, ref hBitmap, ref oldBits);
            return PrintBit();
        }

        public RenderResult Print(bool fore = false)
        {
            if (CanRender(out var handle))
            {
                try
                {
                    using (var bmp = Printmap())
                    {
                        if (bmp == null) return RenderResult.Skip;
                        return Render(handle, alpha, bmp, target_rect);
                    }
                }
                catch { }
                return RenderResult.Error;
            }
            else return RenderResult.Skip;
        }
        public RenderResult Print(Bitmap bmp)
        {
            if (CanRender(out var handle)) return Render(handle, alpha, bmp, target_rect);
            else return RenderResult.Skip;
        }
        public RenderResult Print(Bitmap bmp, Rectangle rect)
        {
            using (bmp)
            {
                if (CanRender(out var handle)) return Render(handle, alpha, bmp, rect);
                else return RenderResult.Skip;
            }
        }
        public RenderResult PrintCache(bool fore = false)
        {
            if (CanRender(out var handle))
            {
                try
                {
                    return Render(handle, alpha, target_rect);
                }
                catch { }
                return RenderResult.Error;
            }
            else return RenderResult.Skip;
        }

        public bool RenderCache = false;
        IntPtr hBitmap, oldBits;
        RenderResult Render(IntPtr handle, byte alpha, Bitmap bmp, Rectangle rect)
        {
            if (InvokeRequired)
            {
                try
                {
                    if (IsDisposed || Disposing) return RenderResult.Skip;
                    if (RenderCache) return Invoke(() => Win32.SetBits(memDc, rect, handle, alpha));
                    RenderCache = true;
                    return Invoke(() => Win32.SetBits(memDc, bmp, rect, handle, alpha, out hBitmap, out oldBits));
                }
                catch { }
            }
            if (RenderCache) return Win32.SetBits(memDc, rect, handle, alpha);
            RenderCache = true;
            return Win32.SetBits(memDc, bmp, rect, handle, alpha, out hBitmap, out oldBits);
        }
        RenderResult Render(IntPtr handle, byte alpha, Rectangle rect)
        {
            if (InvokeRequired)
            {
                try
                {
                    if (IsDisposed || Disposing) return RenderResult.Skip;
                    return Invoke(() => Win32.SetBits(memDc, rect, handle, alpha));
                }
                catch { }
            }
            return Win32.SetBits(memDc, rect, handle, alpha);
        }

        Action<bool> actionCursor;
        public void SetCursor(bool val)
        {
            if (InvokeRequired) Invoke(actionCursor, val);
            else Cursor = val ? Cursors.Hand : DefaultCursor;
        }

        #region 无焦点窗体

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x00080000 | 0x08000000 | 0x00000080;
                cp.Parent = PARENT?.Handle ?? IntPtr.Zero;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation => UFocus;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x02E0)
            {
                // 低字节是水平DPI，高字节是垂直DPI
                int dpiX = (int)(m.WParam.ToInt64() & 0xFFFF), dpiY = (int)(m.WParam.ToInt64() >> 16);
                InitDpi(dpiX);
            }
            else if (m.Msg == 0x000A) messageHandler?.SetEnabled(m.WParam != IntPtr.Zero);
            else if (UFocus && m.Msg == 0x21)
            {
                m.Result = new IntPtr(3);
                return;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region 关闭

        bool isClosing = false;
        public virtual void IClosing() { }
        public void IClose(bool isdispose = false)
        {
            if (isdispose) _Dispose();
            else _Close();
        }
        void _Close()
        {
            try
            {
                if (isClosing || IsDisposed) return;
                isClosing = true;
                if (InvokeRequired) Invoke(Close);
                else Close();
            }
            catch { }
        }
        void _Dispose()
        {
            try
            {
                if (IsDisposed) return;
                IClosing();
                if (InvokeRequired) Invoke(Dispose);
                else Dispose();
            }
            catch { }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 加载后绑定消息
        /// </summary>
        [Description("加载后绑定消息"), Category("行为"), DefaultValue(true)]
        public virtual bool CanLoadMessage { get; set; } = true;

        /// <summary>
        /// 关闭行为
        /// </summary>
        [Description("关闭行为"), Category("行为"), DefaultValue(CloseMode.None)]
        public CloseMode CloseMode { get; set; } = CloseMode.None;

        #endregion

        #region DPI

        public float Dpi { get; private set; }

        void InitDpi(int? dpi = null)
        {
            if (Config._dpi_custom.HasValue) Dpi = Config._dpi_custom.Value;
            else if (dpi.HasValue) Dpi = dpi.Value / 96F;
            else
            {
#if NET40 || NET46
                Dpi = Config.Dpi;
#else
                Dpi = DeviceDpi / 96F;
#endif
            }
        }

        #endregion

        #region 鼠标键盘消息

        public void IMOUSECLICK()
        {
            var mousePosition = MousePosition;
            if (ALLRECT().Contains(mousePosition)) return;
            IClose();
        }

        public void IMOUSEMOVE()
        {
            if (CloseMode.HasFlag(CloseMode.Leave))
            {
                var mousePosition = MousePosition;
                var rect = ALLRECT();
                if (IMOUSEMOVEAfter(mousePosition.X, mousePosition.Y, rect)) return;
                if (rect.Contains(mousePosition)) return;
                IClose();
            }
        }
        public virtual bool IMOUSEMOVEAfter(int x, int y, Rectangle rect) => false;

        public bool IKEYS(Keys keys)
        {
            if (KeyCall == null) return false;
            return KeyCall(keys);
        }

        Rectangle ALLRECT()
        {
            if (PARENT == null) return target_rect;
            var rect = new Rectangle(target_rect.X, target_rect.Y, target_rect.Width, target_rect.Height);
            try
            {
                if (!CloseMode.HasFlag(CloseMode.NoControl) && PARENT.IsHandleCreated) rect = Rectangle.Union(rect, new Rectangle(PARENT.PointToScreen(Point.Empty), PARENT.Size));
            }
            catch { }
            FunSub(PARENT, ref rect);
            return rect;
        }
        void FunSub(Control control, ref Rectangle rect)
        {
            if (control is SubLayeredForm subForm)
            {
                var subform = subForm.SubForm();
                if (subform != null)
                {
                    rect = Rectangle.Union(rect, subform.TargetRect);
                    FunSub(subform, ref rect);
                }
            }
        }

        #endregion

        #region 鼠标悬停

        AnimationTask? taskHover;
        TimeSpan timeHover;
        Stopwatch hoverStopwatch = new Stopwatch();
        int oldx = -1, oldy = -1;
        protected virtual bool CanMouseMove { get; set; }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (CanMouseMove)
            {
                if (oldx == e.X && oldy == e.Y) return;
                oldx = e.X;
                oldy = e.Y;
                hoverStopwatch.Reset();
                hoverStopwatch.Start();
                timeHover = hoverStopwatch.Elapsed + TimeSpan.FromMilliseconds(Config.MouseHoverDelay);
                if (taskHover == null)
                {
                    taskHover = new AnimationTask(new AnimationLoopConfig(this, () =>
                    {
                        if (hoverStopwatch.Elapsed < timeHover) return true;
                        BeginInvoke(() => OnMouseHover(oldx, oldy));
                        return false;
                    }, Config.MouseHoverDelay).SetEnd(() => taskHover = null).SetSleep(Config.MouseHoverDelay).SetPriority());
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            taskHover?.Dispose();
            taskHover = null;
            hoverStopwatch.Reset();
            oldx = -1;
            oldy = -1;
        }

        protected virtual void OnMouseHover(int x, int y)
        {
        }

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
                int moveX = oldX - x, moveY = oldY - y, moveXa = Math.Abs(moveX), moveYa = Math.Abs(moveY), threshold = (int)(Config.TouchThreshold * Dpi);
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

        AnimationTask? taskTouch;
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
                            taskTouch = new AnimationTask(new AnimationLoopConfig(this, () =>
                            {
                                if (moveYa > 0 && OnTouchScrollY(-incremental))
                                {
                                    moveYa -= duration;
                                    return true;
                                }
                                return false;
                            }, sleep).SetPriority());
                        }
                        else
                        {
                            taskTouch = new AnimationTask(new AnimationLoopConfig(this, () =>
                            {
                                if (moveYa > 0 && OnTouchScrollY(incremental))
                                {
                                    moveYa -= duration;
                                    return true;
                                }
                                return false;
                            }, sleep).SetPriority());
                        }
                    }
                }
                return false;
            }
            return true;
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

        #region 委托

#if NET40 || NET46 || NET48

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IAsyncResult BeginInvoke(Action method) => BeginInvoke(method, null);

        public void Invoke(Action method) => _ = Invoke(method, null);
        public T Invoke<T>(Func<T> method) => (T)Invoke(method, null);

#endif

        #endregion
    }

    public interface SubLayeredForm
    {
        ILayeredForm? SubForm();
    }
}