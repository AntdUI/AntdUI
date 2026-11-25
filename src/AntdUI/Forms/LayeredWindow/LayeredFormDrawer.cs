// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormDrawer : ILayeredForm, LayeredFormAsynLoad
    {
        int FrmRadius = 0, FrmBor = 0;
        bool HasBor = false;
        Drawer.Config config;
        int padding = 24;
        ILayeredForm? formMask;
        public bool isclose = false;

        #region 构造

        /// <summary>
        /// 统一构造。Z 序与前台焦点全部由 <paramref name="session"/> 托管，
        /// 不再调用 SetTopMost / owner.Activate()（三套 Z 序机制混用是层级错乱的根源）。
        /// </summary>
        public LayeredFormDrawer(Drawer.Config _config, ILayeredForm? mask = null, LayeredWindowSession? session = null)
        {
            config = _config;
            formMask = mask;
            Session = session;
            // 保留 SetTopMost：它同时维护 topMost 字段（供子浮层继承置顶）
            // 与父为 Popover 时的 CloseLockedAdd 关闭锁；实际 Z 序仍由会话统一裁决。
            SetTopMost(config.Form, Handle);
            SetDpi(config.Form);
            SetFont(config.Form.Font);
            if (Config.ShadowEnabled) shadow_size = (int)(Config.ShadowSize * Dpi) * 2;
            padding = (int)Math.Round(config.Padding * Dpi);
            Padding = new Padding(padding);
            HasBor = Helper.FormFrame(config.Form, out FrmRadius, out FrmBor);
            config.Content.BackColor = Colour.BgElevated.Get(config.ColorScheme, nameof(Drawer));
            config.Content.ForeColor = Colour.Text.Get(config.ColorScheme, nameof(Drawer));
            SetPoint();
            SetSize(start_W, start_H);
            SetLocation(start_X, start_Y);
            int p2 = padding * 2, contentW, contentH;
            if (vertical)
            {
                contentW = end_W - p2;
                contentH = end_H - shadow_size - p2;
            }
            else
            {
                contentW = end_W - shadow_size - p2;
                contentH = end_H - p2;
            }
            if (config.Content.Tag is Size) { }
            else
            {
                config.Content.Tag = config.Content.Size;
                Win32.WindowTheme(config.Content, Config.IsDark);
                Helper.DpiAuto(Dpi, config.Content);
            }
            config.Content.Bounds = Helper.OffScreenArea(contentW, contentH);

            // P1 根源修复：真控件懒创建。
            // 动画期间只用这张截图做离屏渲染，DoubleBufferForm 根本不 Show，
            // 因此"快速点击打断动画时卡出 DoubleBufferForm"在物理上不再可能发生。
            tempContent = config.Content.CaptureControl();

            config.Form.LocationChanged += Form_LocationChanged;
            config.Form.SizeChanged += Form_SizeChanged;

            if (mask != null && config.MaskClosable)
            {
                mask.Click += (s1, e1) =>
                {
                    isclose = true;
                    IClose();
                };
            }
        }

        int shadow_size = 0;
        bool vertical = false;

        #endregion

        void SetPoint()
        {
            switch (config.Align)
            {
                case TAlignMini.Top:
                    vertical = true;
                    start_H = 0;
                    end_H = (int)(config.Content.Height * Dpi) + padding * 2 + shadow_size;
                    if (config.Form is Window windowT)
                    {
                        start_W = end_W = windowT.Width;
                        start_X = end_X = windowT.Left;
                        start_Y = end_Y = windowT.Top;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Bottom:
                    vertical = true;
                    start_H = 0;
                    end_H = (int)(config.Content.Height * Dpi) + padding * 2 + shadow_size;
                    if (config.Form is Window windowB)
                    {
                        start_W = end_W = windowB.Width;
                        start_X = end_X = windowB.Left;
                        start_Y = windowB.Top + windowB.Height;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = config.Form.Top + config.Form.Height;
                    }
                    end_Y = start_Y - end_H;
                    break;
                case TAlignMini.Left:
                    start_W = 0;
                    end_W = (int)(config.Content.Width * Dpi) + padding * 2 + shadow_size;
                    if (config.Form is Window windowL)
                    {
                        start_H = end_H = windowL.Height;
                        start_X = end_X = windowL.Left;
                        start_Y = end_Y = windowL.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Right:
                default:
                    start_W = 0;
                    end_W = (int)(config.Content.Width * Dpi) + padding * 2 + shadow_size;
                    if (config.Form is Window windowR)
                    {
                        start_H = end_H = windowR.Height;
                        start_X = windowR.Left + windowR.Width;
                        start_Y = end_Y = windowR.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = config.Form.Left + config.Form.Width;
                        start_Y = end_Y = config.Form.Top;
                    }
                    end_X = start_X - end_W;
                    break;
            }
        }

        Bitmap? tempContent;
        private void Form_SizeChanged(object? sender, EventArgs e)
        {
            if (config.Form.WindowState == FormWindowState.Minimized) return;
            switch (config.Align)
            {
                case TAlignMini.Top:
                    if (config.Form is Window windowT)
                    {
                        start_W = end_W = windowT.Width;
                        start_X = end_X = windowT.Left;
                        start_Y = end_Y = windowT.Top;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Bottom:
                    if (config.Form is Window windowB)
                    {
                        start_W = end_W = windowB.Width;
                        start_X = end_X = windowB.Left;
                        start_Y = windowB.Top + windowB.Height;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = config.Form.Top + config.Form.Height;
                    }
                    end_Y = start_Y - end_H;
                    break;
                case TAlignMini.Left:
                    if (config.Form is Window windowL)
                    {
                        start_H = end_H = windowL.Height;
                        start_X = end_X = windowL.Left;
                        start_Y = end_Y = windowL.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Right:
                default:
                    if (config.Form is Window windowR)
                    {
                        start_H = end_H = windowR.Height;
                        start_X = windowR.Left + windowR.Width;
                        start_Y = end_Y = windowR.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = config.Form.Left + config.Form.Width;
                        start_Y = end_Y = config.Form.Top;
                    }
                    end_X = start_X - end_W;
                    break;
            }
            if (task_animation == null)
            {
                SetLocation(end_X, end_Y);
                SetSize(end_W, end_H);
                if (form != null)
                {
                    isok = false;
                    var rect = Ang();
                    form.Bounds = rect;
                    isok = true;
                }
                RefreshFull();
            }
        }

        private void Form_LocationChanged(object? sender, EventArgs e)
        {
            if (config.Form.WindowState == FormWindowState.Minimized)
            {
                SetLocation(-end_W * 2, -end_H * 2);
                if (task_animation == null)
                {
                    if (form != null) form.Location = Helper.OffScreenLocation(form.Width, form.Height);
                    RefreshFull();
                }
                return;
            }
            switch (config.Align)
            {
                case TAlignMini.Top:
                    if (config.Form is Window windowT)
                    {
                        start_W = end_W = windowT.Width;
                        start_X = end_X = windowT.Left;
                        start_Y = end_Y = windowT.Top;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Bottom:
                    if (config.Form is Window windowB)
                    {
                        start_W = end_W = windowB.Width;
                        start_X = end_X = windowB.Left;
                        start_Y = windowB.Top + windowB.Height;
                    }
                    else
                    {
                        start_W = end_W = config.Form.Width;
                        start_X = end_X = config.Form.Left;
                        start_Y = config.Form.Top + config.Form.Height;
                    }
                    end_Y = start_Y - end_H;
                    break;
                case TAlignMini.Left:
                    if (config.Form is Window windowL)
                    {
                        start_H = end_H = windowL.Height;
                        start_X = end_X = windowL.Left;
                        start_Y = end_Y = windowL.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = end_X = config.Form.Left;
                        start_Y = end_Y = config.Form.Top;
                    }
                    break;
                case TAlignMini.Right:
                default:
                    if (config.Form is Window windowR)
                    {
                        start_H = end_H = windowR.Height;
                        start_X = windowR.Left + windowR.Width;
                        start_Y = end_Y = windowR.Top;
                    }
                    else
                    {
                        start_H = end_H = config.Form.Height;
                        start_X = config.Form.Left + config.Form.Width;
                        start_Y = end_Y = config.Form.Top;
                    }
                    end_X = start_X - end_W;
                    break;
            }
            if (task_animation == null)
            {
                SetLocation(end_X, end_Y);
                if (form != null) form.Location = Ang().Location;
                RefreshFull();
            }
        }

        #region 动画

        int start_X = 0, end_X = 0, start_Y = 0, end_Y = 0;
        int start_W = 0, end_W = 0, start_H = 0, end_H = 0;
        AnimationTask? task_animation;
        bool run_end = false, ok_end = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Config.HasAnimation(nameof(Drawer)))
            {
                var t = Animation.TotalFrames(10, 100);
                int sleep = config.Mask ? 100 : 0;
                task_animation = new AnimationTask(new AnimationFixed2Config((i, val) => SetAnimateWindow(val), 10, t, null, true).SetSleep(sleep).SetEnd(() =>
                {
                    task_animation = null;
                    if (IsHandleCreated)
                    {
                        // 世代号闸门：动画末帧排队的 ShowContent 带当前 gen，
                        // 若此前已被 BeginClose 递增，回调会被 CanReveal 直接拒绝。
                        var gen = Generation;
                        BeginInvoke(() => ShowContent(gen));
                    }
                    SetAnimateValue(end_X, end_Y, end_W, end_H, 255);
                }));
            }
            else
            {
                SetAnimateValue(end_X, end_Y, end_W, end_H, 255);
                var gen = Generation;
                BeginInvoke(() => ShowContent(gen));
            }
        }

        #region 控件（懒创建）

        Form? form;
        bool contentShown = false, aborted = false;

        /// <summary>
        /// 只在 State == Open 且世代号未变时才创建并显示真控件。
        /// 这是"Mask 最底 → 面板截图动画 → DoubleBufferForm 最上/最后"的落地实现。
        /// </summary>
        void ShowContent(int gen)
        {
            if (!CanReveal(gen) || contentShown) return;
            contentShown = true;
            LoadContent();
            if (form == null) return;
            var rect = Ang();
            form.Bounds = rect;
            Session?.MarkOpen();
            Session?.Stack();
            config.OnLoad?.Invoke();
            IsLoad = false;
            LoadCompleted?.Invoke();
            config.Content.SizeChanged += Content_SizeChanged;
            config.Content.ControlEvent();
            if (config.Content is ControlEvent controlEvent) controlEvent.LoadCompleted();
        }

        void LoadContent()
        {
            var rect = Ang();
            var hidelocation = Helper.OffScreenArea(rect.Width, rect.Height);
            if (config.Content is Form form_)
            {
                form_.BackColor = Colour.BgElevated.Get(config.ColorScheme, nameof(Drawer));
                form_.FormBorderStyle = FormBorderStyle.None;
                form_.Bounds = hidelocation;
                form_.StartPosition = FormStartPosition.Manual;
                form = form_;
            }
            else
            {
                // focus=false → ShowWithoutActivation=true 且 WM_MOUSEACTIVATE 返回 MA_NOACTIVATE：
                // 内容窗体不抢前台，主窗体从会话开始到结束始终是前台窗口，
                // 因此浮层隐藏后不会"激活其它软件窗口"，也无需 Activate() 补救。
                form = new DoubleBufferForm(this, config.Content, false)
                {
                    BackColor = Colour.BgElevated.Get(config.ColorScheme, nameof(Drawer)),
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.Manual,
                    Bounds = hidelocation
                };
            }
            if (form is DoubleBufferForm dbf) dbf.Session = Session;      // OnHandleCreated 自动注册为 Content 层
            else Session?.Attach(form, LayeredRole.Content);

            if (!config.Dispose && config.Content.Tag is Size size)
            {
                form.FormClosing += (a, b) =>
                {
                    config.Content.Dock = DockStyle.None;
                    config.Content.Bounds = Helper.OffScreenArea(size.Width, size.Height);
                    config.Form.Controls.Add(config.Content);
                };
            }
            config.Content.Disposed += (a, b) =>
            {
                config.Content.SizeChanged -= Content_SizeChanged;
                Close();
            };
            form.Show(this);
        }

        bool isok = true;
        private void Content_SizeChanged(object? sender, EventArgs e)
        {
            if (form == null) return;
            if (isok)
            {
                isok = false;
                var size = config.Content.Size;
                switch (config.Align)
                {
                    case TAlignMini.Top:
                        end_H = (int)(size.Height * Dpi) + padding * 2 + shadow_size;
                        if (config.Form is Window windowT)
                        {
                            start_W = end_W = windowT.Width;
                            start_X = end_X = windowT.Left;
                            start_Y = end_Y = windowT.Top;
                        }
                        else
                        {
                            start_W = end_W = config.Form.Width;
                            start_X = end_X = config.Form.Left;
                            start_Y = end_Y = config.Form.Top;
                        }
                        break;
                    case TAlignMini.Bottom:
                        end_H = (int)(size.Height * Dpi) + padding * 2 + shadow_size;
                        if (config.Form is Window windowB)
                        {
                            start_W = end_W = windowB.Width;
                            start_X = end_X = windowB.Left;
                            start_Y = windowB.Top + windowB.Height;
                        }
                        else
                        {
                            start_W = end_W = config.Form.Width;
                            start_X = end_X = config.Form.Left;
                            start_Y = config.Form.Top + config.Form.Height;
                        }
                        end_Y = start_Y - end_H;
                        break;
                    case TAlignMini.Left:
                        end_W = (int)(size.Width * Dpi) + padding * 2 + shadow_size;
                        if (config.Form is Window windowL)
                        {
                            start_H = end_H = windowL.Height;
                            start_X = end_X = windowL.Left;
                            start_Y = end_Y = windowL.Top;
                        }
                        else
                        {
                            start_H = end_H = config.Form.Height;
                            start_X = end_X = config.Form.Left;
                            start_Y = end_Y = config.Form.Top;
                        }
                        break;
                    case TAlignMini.Right:
                    default:
                        end_W = (int)(size.Width * Dpi) + padding * 2 + shadow_size;
                        if (config.Form is Window windowR)
                        {
                            start_H = end_H = windowR.Height;
                            start_X = windowR.Left + windowR.Width;
                            start_Y = end_Y = windowR.Top;
                        }
                        else
                        {
                            start_H = end_H = config.Form.Height;
                            start_X = config.Form.Left + config.Form.Width;
                            start_Y = end_Y = config.Form.Top;
                        }
                        end_X = start_X - end_W;
                        break;
                }
                SetLocation(end_X, end_Y);
                SetSize(end_W, end_H);
                var rect = Ang();
                form.Bounds = rect;
                RefreshFull();
                ITask.Run(() =>
                {
                    System.Threading.Thread.Sleep(500);
                    isok = true;
                });
            }
        }

        /// <summary>
        /// 是否正在加载
        /// </summary>
        [Description("是否正在加载"), Category(nameof(CategoryAttribute.Data)), DefaultValue(true)]
        public bool IsLoad { get; set; } = true;

        /// <summary>
        /// 加载完成回调
        /// </summary>
        [Description("加载完成回调"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        public Action? LoadCompleted { get; set; }

        Rectangle Ang()
        {
            switch (config.Align)
            {
                case TAlignMini.Top: return new Rectangle(end_X + padding, end_Y + padding, end_W - padding * 2, end_H - shadow_size - padding * 2);
                case TAlignMini.Bottom: return new Rectangle(end_X + padding, end_Y + padding + shadow_size, end_W - padding * 2, end_H - shadow_size - padding * 2);
                case TAlignMini.Left: return new Rectangle(end_X + padding, end_Y + padding, end_W - shadow_size - padding * 2, end_H - padding * 2);
                case TAlignMini.Right:
                default: return new Rectangle(end_X + padding + shadow_size, end_Y + padding, end_W - shadow_size - padding * 2, end_H - padding * 2);
            }
        }

        #endregion

        #region 设置动画参数 / 帧提交

        void SetAnimateValueX(int x, int w, byte _alpha)
        {
            if (TargetRect.X != x || TargetRect.Width != w || alpha != _alpha)
            {
                SetLocationX(x);
                SetSizeW(w);
                alpha = _alpha;
                QueueFrame();
            }
        }
        void SetAnimateValueY(int y, int h, byte _alpha)
        {
            if (TargetRect.Y != y || TargetRect.Height != h || alpha != _alpha)
            {
                SetLocationY(y);
                SetSizeH(h);
                alpha = _alpha;
                QueueFrame();
            }
        }
        void SetAnimateValue(int x, int y, int w, int h, byte _alpha)
        {
            SetLocation(x, y);
            SetSize(w, h);
            alpha = _alpha;
            QueueFrame(true); // 最终帧同步提交，保证 ShowContent 之前白底已完整
        }

        /// <summary>关闭动画统一走与打开动画完全相同的位置/尺寸公式，从当前进度平滑续播到 0。</summary>
        void SetAnimateWindow(float val)
        {
            if (vertical) SetAnimateValueY(start_Y + (int)((end_Y - start_Y) * val), (int)(end_H * val), (byte)(255 * val));
            else SetAnimateValueX(start_X + (int)((end_X - start_X) * val), (int)(end_W * val), (byte)(255 * val));
        }

        #endregion

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            isclose = true;
            formMask?.IClose();
        }

        internal Action? DClose;
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var r = false;
            if (DClose != null)
            {
                r = true;
                DClose();
                DClose = null;
            }
            task_animation?.Dispose();
            if (!ok_end)
            {
                // P1：进入 Closing 即刻递增世代号，撤不回的 BeginInvoke(ShowContent) 会被闸门拒绝
                Session?.BeginClose();
                config.Form.LocationChanged -= Form_LocationChanged;
                config.Form.SizeChanged -= Form_SizeChanged;
                tempContent?.Dispose();
                tempContent = config.Content.CaptureControl();
                InvalidateFull();
                if (form != null) form.Location = Helper.OffScreenLocation(form.Width, form.Height);
                e.Cancel = true;
                if (Config.HasAnimation(nameof(Drawer)))
                {
                    if (!run_end)
                    {
                        run_end = true;
                        var t = Animation.TotalFrames(10, 100);
                        var oldval = task_animation?.Tag;
                        task_animation = new AnimationTask(new AnimationFixed2Config((i, val) => SetAnimateWindow(val), 10, t, oldval, false).SetSleep(r ? 200 : 0).SetEnd(() =>
                        {
                            task_animation = null;
                            ok_end = true;
                            IClose(true);
                        }));
                    }
                }
                else
                {
                    ok_end = true;
                    IClose(true);
                }
            }
            base.OnFormClosing(e);
        }

        /// <summary>打开动画尚未开始/已被打断时的收口，不触发用户 OnClose 回调、不释放用户控件。</summary>
        internal void Abort()
        {
            if (aborted) return;
            aborted = true;
            isclose = true;
            task_animation?.Dispose();
            Session?.BeginClose();
            // DisplayDelay 打断发生在后台线程，Dispose 必须回到 UI 线程执行，避免跨线程销毁控件
            Action dispose = () => { if (!IsDisposed) Dispose(); };
            if (IsHandleCreated)
            {
                try { BeginInvoke(dispose); return; }
                catch { }
            }
            dispose();
        }

        protected override void Dispose(bool disposing)
        {
            config.Form.LocationChanged -= Form_LocationChanged;
            config.Form.SizeChanged -= Form_SizeChanged;
            if (!aborted && config.Dispose) config.Content.Dispose();
            tempContent?.Dispose();
            tempContent = null;
            InvalidateFull();
            form?.Dispose();
            form = null;
            task_animation?.Dispose();
            task_animation = null;
            if (!aborted) config.OnClose?.Invoke();
            config.OnClose = null;
            shadow_temp?.Dispose();
            shadow_temp = null;
            // 根源修复：删除 config.ManualActivateParent 分支里的 owner.Activate()。
            // 焦点归还统一由会话在所有浮层销毁之后、且确实被动丢失时才执行一次。
            Session?.BeginClose();
            base.Dispose(disposing);
        }

        public void IRClose()
        {
            ok_end = true;
            IClose(true);
        }

        #endregion

        #region 渲染（P2：全尺寸位图缓存 + pptSrc 揭示式裁剪 + 帧合并）

        /// <summary>end_W×end_H 全尺寸位图，只生成一次；每帧仅改 pptDst / pSize / pptSrc / alpha。</summary>
        Bitmap? fullBitmap;
        readonly object frameLock = new object();
        Rectangle pendingRect;
        Point pendingSrc;
        byte pendingAlpha;
        int frameQueued;

        /// <summary>
        /// 帧合并：动画线程只更新"最新帧"参数并用 BeginInvoke 异步提交，
        /// UI 线程积压时自动丢弃中间帧，不再同步 Invoke 打满消息循环。
        /// </summary>
        void QueueFrame(bool now = false)
        {
            pendingRect = new Rectangle(TargetRect.X, TargetRect.Y, TargetRect.Width, TargetRect.Height);
            // 位图左上角与窗口左上角绑定：图像随窗口左上角平移，揭示量由 pSize 决定。
            pendingSrc = Point.Empty;
            pendingAlpha = alpha;
            if (now) { FlushFrameCore(); return; }
            if (Interlocked.Exchange(ref frameQueued, 1) == 0)
            {
                try { BeginInvoke(new Action(FlushFrameCore)); }
                catch { Interlocked.Exchange(ref frameQueued, 0); }
            }
        }

        void FlushFrameCore()
        {
            Interlocked.Exchange(ref frameQueued, 0);
            IntPtr h;
            if (!CanRender(out h)) return;
            Rectangle rect = pendingRect;
            Point src = pendingSrc;
            byte a = pendingAlpha;
            alpha = a;
            ResetRenderCache();
            Print(BuildBitmap(rect.Width, rect.Height), rect);
            //// 快动画：全尺寸位图只生成一次，每帧仅改 pptDst/pSize/pptSrc/alpha。
            //Bitmap? bmp;
            //lock (frameLock)
            //{
            //    if (fullBitmap == null) fullBitmap = BuildFullBitmap();
            //    bmp = fullBitmap;
            //}
            //if (bmp == null) return;
            //Print(bmp, rect, src);
        }

        /// <summary>位图内容/尺寸变化后强制重建并让下次提交重新选入 HBITMAP。</summary>
        void InvalidateFull()
        {
            lock (frameLock)
            {
                fullBitmap?.Dispose();
                fullBitmap = null;
            }
            ResetRenderCache();
        }

        /// <summary>静态场景（主窗体移动/缩放、内容尺寸变化）走同步 Print 重绘。</summary>
        void RefreshFull()
        {
            lock (frameLock)
            {
                fullBitmap?.Dispose();
                fullBitmap = null;
            }
            Print(true);
        }

        /// <summary>PrintBit 供 Printmap/Print 使用，返回副本（由调用方释放）。</summary>
        public override Bitmap? PrintBit()
        {
            Bitmap? bmp;
            lock (frameLock) bmp = fullBitmap ??= BuildFullBitmap();
            if (bmp == null) return null;
            return new Bitmap(bmp);
        }

        Bitmap BuildFullBitmap() => BuildBitmap(end_W, end_H);

        /// <summary>按给定尺寸画圆角+阴影+内容。缓存路径传 end 尺寸；实时圆角路径传当前帧尺寸。</summary>
        Bitmap BuildBitmap(int w, int h)
        {
            w = Math.Max(1, w);
            h = Math.Max(1, h);
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp).High(Dpi))
            {
                var rect_t = new Rectangle(0, 0, w, h);
                var rect = HasBor && w > FrmBor * 2 && h > FrmBor
                    ? new Rectangle(FrmBor, 0, w - FrmBor * 2, h - FrmBor) : rect_t;
                var rect_read = DrawShadow(g, rect);
                using (var path = rect_read.RoundPath(FrmRadius))
                {
                    g.Fill(Colour.BgElevated.Get(config.ColorScheme, nameof(Drawer)), path);
                    if (tempContent != null) g.Image(tempContent, new Rectangle(rect_read.X + padding, rect_read.Y + padding, tempContent.Width, tempContent.Height));
                }
            }
            return bmp;
        }

        SafeBitmap? shadow_temp;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">客户区域</param>
        Rectangle DrawShadow(Canvas g, Rectangle rect)
        {
            if (shadow_size > 0)
            {
                int size = shadow_size * 2, size2 = size * 2;
                switch (config.Align)
                {
                    case TAlignMini.Top:
                        if (shadow_temp == null || shadow_temp.Width != end_W)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X, rect.Y + shadow_size, end_W, size).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(end_W, size2, shadow_size);
                            }
                        }
                        g.Image(shadow_temp.Bitmap, new Rectangle(rect.Y, rect.Bottom - size2, rect.Width, size2), Config.ShadowOpacity);
                        return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - shadow_size);
                    case TAlignMini.Bottom:
                        if (shadow_temp == null || shadow_temp.Width != end_W)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X, rect.Y + shadow_size, end_W, size).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(end_W, size2, shadow_size);
                            }
                        }
                        g.Image(shadow_temp.Bitmap, new Rectangle(rect.Y, rect.Y, rect.Width, size2), Config.ShadowOpacity);
                        return new Rectangle(rect.X, rect.Y + shadow_size, rect.Width, rect.Height - shadow_size);
                    case TAlignMini.Left:
                        if (shadow_temp == null || shadow_temp.Height != end_H)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X + shadow_size, rect.Y, size, end_H).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(size2, end_H, shadow_size);
                            }
                        }
                        g.Image(shadow_temp.Bitmap, new Rectangle(rect.Right - size2, rect.Y, size2, rect.Height), Config.ShadowOpacity);
                        return new Rectangle(rect.X, rect.Y, rect.Width - shadow_size, rect.Height);
                    case TAlignMini.Right:
                    default:
                        if (shadow_temp == null || shadow_temp.Height != end_H)
                        {
                            shadow_temp?.Dispose();
                            using (var path = new Rectangle(rect.X + shadow_size, rect.Y, size, end_H).RoundPath(FrmRadius))
                            {
                                shadow_temp = path.PaintShadow(size2, end_H, shadow_size);
                            }
                        }
                        g.Image(shadow_temp.Bitmap, new Rectangle(rect.X, rect.Y, size2, rect.Height), Config.ShadowOpacity);
                        return new Rectangle(rect.X + shadow_size, rect.Y, rect.Width - shadow_size, rect.Height);
                }
            }
            return rect;
        }

        #endregion
    }
}