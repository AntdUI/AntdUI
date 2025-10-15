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
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            UpdateStyles();
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
                if (MessageEnable) messageHandler = new MessageHandler(this);
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
            handle = null;
            FunRun = false;
            messageHandler?.Dispose();
            messageHandler = null;
            base.Dispose(disposing);
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
                cp.Parent = IntPtr.Zero;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation => UFocus;

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

        int countClose = 0;
        public virtual void IClosing() { }
        public void IClose(bool isdispose = false)
        {
            try
            {
                if (IsDisposed) return;
                if (InvokeRequired)
                {
                    Invoke(() => IClose(isdispose));
                    return;
                }
                if (countClose > 2) isdispose = true;
                if (isdispose)
                {
                    IClosing();
                    Dispose();
                    return;
                }
                countClose++;
                Close();
            }
            catch { }
        }

        #region 属性

        /// <summary>
        /// 加载后绑定消息
        /// </summary>
        [Description("加载后绑定消息"), Category("行为"), DefaultValue(true)]
        public virtual bool CanLoadMessage { get; set; } = true;

        /// <summary>
        /// 消息使能
        /// </summary>
        [Description("消息使能"), Category("行为"), DefaultValue(false)]
        public virtual bool MessageEnable { get; set; }

        /// <summary>
        /// 点击子菜单关闭（true后点击子菜单不关闭）
        /// </summary>
        [Description("点击子菜单关闭"), Category("行为"), DefaultValue(false)]
        public virtual bool MessageCloseSub { get; set; }

        /// <summary>
        /// 点击自己是否关闭（true 点击自己不关闭）
        /// </summary>
        [Description("点击自己是否关闭"), Category("行为"), DefaultValue(true)]
        public virtual bool MessageClickMe { get; set; } = true;

        /// <summary>
        /// 鼠标离开关闭
        /// </summary>
        [Description("鼠标离开关闭"), Category("行为"), DefaultValue(false)]
        public virtual bool MessageCloseMouseLeave { get; set; }

        #endregion

        #region 鼠标键盘消息

        public void IMOUSECLICK()
        {
            var mousePosition = MousePosition;
            if (!target_rect.Contains(mousePosition))
            {
                try
                {
                    if (PARENT != null && PARENT.IsHandleCreated)
                    {
                        if (MessageClickMe)
                        {
                            if (ContainsPosition(PARENT, mousePosition)) return;
                            if (new Rectangle(PARENT.PointToScreen(Point.Empty), PARENT.Size).Contains(mousePosition)) return;
                        }

                        #region 判断内容

                        if (MessageCloseSub && FunSub(PARENT, mousePosition)) return;

                        #endregion
                    }
                    IClose();
                }
                catch { }
            }
        }

        public void IMOUSELEAVE()
        {
            if (MessageCloseMouseLeave)
            {
                var mousePosition = MousePosition;
                if (!target_rect.Contains(mousePosition))
                {
                    try
                    {
                        if (PARENT != null && PARENT.IsHandleCreated)
                        {
                            if (ContainsPosition(PARENT, mousePosition)) return;
                            if (new Rectangle(PARENT.PointToScreen(Point.Empty), PARENT.Size).Contains(mousePosition)) return;

                            #region 判断内容

                            if (MessageCloseSub && FunSub(PARENT, mousePosition)) return;

                            #endregion
                        }
                        IClose();
                    }
                    catch { }
                }
            }
        }

        public bool IKEYS(Keys keys)
        {
            if (KeyCall == null) return false;
            return KeyCall(keys);
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

                if (control is SubLayeredForm subForm)
                {
                    var subform = subForm.SubForm();
                    if (subform != null) count += ContainsPosition(subform, mousePosition);
                }
            }
            catch { }
            return count;
        }

        #endregion

        #region 鼠标悬停

        ITask? taskHover;
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
                    OnMouseHover(-1, -1);
                    taskHover = new ITask(this, () =>
                    {
                        if (hoverStopwatch.Elapsed < timeHover) return true;
                        BeginInvoke(() => OnMouseHover(oldx, oldy));
                        return false;
                    }, Config.MouseHoverDelay, () => taskHover = null, Config.MouseHoverDelay);
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

        ITask? taskTouch;
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