// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>浮层会话状态。</summary>
    internal enum LayeredSessionState
    {
        Opening,
        Open,
        Closing,
        Closed
    }

    /// <summary>浮层在 Z 序栈中的角色（自底向上）。</summary>
    internal enum LayeredRole
    {
        Mask = 0,
        Panel = 1,
        Content = 2
    }

    /// <summary>
    /// 多层级浮层会话。
    /// <para>所有由 Mask + 面板 + DoubleBufferForm 组成的浮层组合（Drawer / Modal / Popover，
    /// 以及用户直接调用 ShowDialogMask / FormMask）都经由本会话统一托管：</para>
    /// <list type="bullet">
    /// <item>Z 序：一次 SetWindowPos(SWP_NOACTIVATE) 自底向上确定性重排，整栈位于同一 topmost 组；</item>
    /// <item>前台焦点：会话期间不主动激活任何窗口，仅在确实被动丢失且用户未主动切走时归还一次；</item>
    /// <item>动画世代号：打断动画时递增，使所有在途 BeginInvoke 回调失效。</item>
    /// </list>
    /// </summary>
    internal sealed class LayeredWindowSession
    {
        static readonly object sync = new object();
        static readonly List<LayeredWindowSession> sessions = new List<LayeredWindowSession>();

        public static LayeredWindowSession Create(Form owner, bool topMost)
        {
            var session = new LayeredWindowSession(owner, topMost);
            lock (sync) sessions.Add(session);
            return session;
        }

        static void Remove(LayeredWindowSession session)
        {
            lock (sync) sessions.Remove(session);
        }

        /// <summary>把会话绑定到任意承载窗体（不引入公开接口，避免影响 public 类型签名）。</summary>
        public static void BindHost(Form form, LayeredWindowSession session)
        {
            if (form is ILayeredForm layered) layered.Session = session;
            else if (form is LayeredFormModal modal) modal.Session = session;
            else if (form is DoubleBufferForm buffer) buffer.Session = session;
        }

        readonly Form owner;
        readonly bool topMost;
        readonly List<Layer> layers = new List<Layer>();

        int generation;
        LayeredSessionState state = LayeredSessionState.Opening;
        bool finished;
        int goneCount;
        int totalCount;

        LayeredWindowSession(Form _owner, bool _topMost)
        {
            owner = _owner;
            topMost = _topMost;
        }

        class Layer
        {
            public IntPtr Handle;
            public LayeredRole Role;
            public Form? Form;
            public EventHandler? HandleCreatedHandler;
            public bool Gone;
        }

        public int Generation => generation;
        public LayeredSessionState State => state;
        public Form Owner => owner;
        public bool TopMost => topMost;
        public bool IsClosingOrClosed => state == LayeredSessionState.Closing || state == LayeredSessionState.Closed;

        /// <summary>注册一层浮层；句柄尚未创建时延迟到 HandleCreated 再参与重排。</summary>
        public void Attach(Form form, LayeredRole role)
        {
            if (finished || form == null) return;
            var layer = new Layer { Form = form, Role = role };
            lock (layers)
            {
                layers.Add(layer);
                totalCount++;
            }
            form.FormClosed += Layer_FormClosed;
            form.Disposed += Layer_Disposed;
            if (form.IsHandleCreated && form.Handle != IntPtr.Zero) layer.Handle = form.Handle;
            else
            {
                EventHandler? handler = null;
                handler = (s, e) =>
                {
                    form.HandleCreated -= handler;
                    layer.HandleCreatedHandler = null;
                    layer.Handle = form.Handle;
                    Stack();
                };
                layer.HandleCreatedHandler = handler;
                form.HandleCreated += handler;
            }
            Stack();
        }

        void Layer_FormClosed(object? sender, FormClosedEventArgs e)
        {
            var form = sender as Form;
            if (form != null) OnLayerGone(form);
        }

        void Layer_Disposed(object? sender, EventArgs e)
        {
            var form = sender as Form;
            if (form != null) OnLayerGone(form);
        }

        void OnLayerGone(Form form)
        {
            bool allGone;
            lock (layers)
            {
                foreach (var layer in layers)
                {
                    if (layer.Gone || !ReferenceEquals(layer.Form, form)) continue;
                    layer.Gone = true;
                    layer.Handle = IntPtr.Zero;
                    goneCount++;
                }
                allGone = totalCount > 0 && goneCount >= totalCount;
            }
            form.FormClosed -= Layer_FormClosed;
            form.Disposed -= Layer_Disposed;
            // FormClosed 与 Disposed 任一路径到达都要允许收口，
            // 否则"FormClosed 先到并解绑 Disposed"会让 Finish 永远不触发。
            if (allGone) TryFinishLater(0);
        }

        /// <summary>按 Owner → Mask → Panel → Content 确定性重排；整栈统一进入同一 Z 序组，全程不激活。</summary>
        public void Stack()
        {
            if (IsClosingOrClosed) return;
            IntPtr[] handles;
            lock (layers)
            {
                var ordered = new List<Layer>(layers);
                ordered.Sort((a, b) => ((int)a.Role).CompareTo((int)b.Role));
                var tmp = new List<IntPtr>(ordered.Count);
                // 不把主窗体放入重排栈：它是 owned 窗口链的基底，显式抬升会抢其它应用的 Z 序。
                // 只需让 Mask → Panel → Content 进入同一 Z 序组并按顺序排列。
                foreach (var layer in ordered)
                {
                    if (!layer.Gone && layer.Handle != IntPtr.Zero) tmp.Add(layer.Handle);
                }
                handles = tmp.ToArray();
            }
            LayeredZOrder.Stack(topMost, handles);
        }

        /// <summary>打断动画：使所有在途 BeginInvoke 回调失效。</summary>
        public int NextGeneration() => Interlocked.Increment(ref generation);

        /// <summary>世代号闸门：迟到回调一律拒绝。</summary>
        public bool CanReveal(int gen)
        {
            if (finished || gen != generation) return false;
            return state == LayeredSessionState.Opening || state == LayeredSessionState.Open;
        }

        public void MarkOpen()
        {
            if (state == LayeredSessionState.Opening) state = LayeredSessionState.Open;
        }

        /// <summary>会话开始关闭：采样前台窗口（用于区分"用户主动切走"与"被动丢失"）。</summary>
        public void BeginClose()
        {
            if (state == LayeredSessionState.Closed) return;
            state = LayeredSessionState.Closing;
            NextGeneration();
            // 兜底：若所有层已释放但事件链异常未收口，仍由延迟检查完成 Finish
            TryFinishLater(0);
        }

        void TryFinishLater(int retry)
        {
            try
            {
                if (owner.IsHandleCreated && !owner.IsDisposed)
                {
                    owner.BeginInvoke(new Action(() => CheckFinish(retry)));
                    return;
                }
            }
            catch { }
            CheckFinish(retry);
        }

        void CheckFinish(int retry)
        {
            if (finished) return;
            bool pending;
            lock (layers) pending = totalCount > 0 && goneCount < totalCount;
            if (pending)
            {
                if (retry < 8) TryFinishLater(retry + 1);
                return;
            }
            Finish();
        }

        /// <summary>所有层销毁后收口：仅做一次条件式焦点归还。</summary>
        public void Finish()
        {
            if (finished) return;
            finished = true;
            state = LayeredSessionState.Closed;
            Remove(this);
        }
    }

    /// <summary>确定性 Z 序重排（不激活任何窗口）。</summary>
    internal static class LayeredZOrder
    {
        static readonly Win32.User32.SetWindowPosFlags Flags =
            Win32.User32.SetWindowPosFlags.SWP_NOMOVE |
            Win32.User32.SetWindowPosFlags.SWP_NOSIZE |
            Win32.User32.SetWindowPosFlags.SWP_NOACTIVATE |
            Win32.User32.SetWindowPosFlags.SWP_NOOWNERZORDER;

        /// <summary>
        /// 按自底向上的顺序重排。每次 HWND_TOP / HWND_TOPMOST 都会把目标放到组顶，
        /// 因此按数组顺序依次调用即可让最终 Z 序严格等于数组顺序。
        /// </summary>
        public static void Stack(bool topMost, params IntPtr[] bottomToTop)
        {
            if (bottomToTop == null || bottomToTop.Length == 0) return;
            var insertAfter = topMost ? new IntPtr(-1) : IntPtr.Zero;
            foreach (var handle in bottomToTop)
            {
                if (handle == IntPtr.Zero) continue;
                try
                {
                    Win32.User32.SetWindowPos(handle, insertAfter, 0, 0, 0, 0, Flags);
                }
                catch { }
            }
        }
    }
}