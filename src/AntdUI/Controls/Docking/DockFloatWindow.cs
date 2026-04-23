// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// DockFloatWindow — floating container that hosts one <see cref="DockPane"/>.
    /// Detects drag-to-dock against the owning <see cref="DockPanel"/> via a <see cref="DockNavigator"/>.
    /// Inherits shadow/resize/DWM chrome from <see cref="BorderlessForm"/>.
    /// </summary>
    [ToolboxItem(false)]
    public class DockFloatWindow : BorderlessForm
    {
        readonly DockPanel owner;
        readonly DockPane pane;
        // navigator != null is the drag-in-progress indicator (implicit; no separate flag needed —
        // WM_MOVING only fires between WM_ENTERSIZEMOVE and WM_EXITSIZEMOVE, so the navigator lifetime
        // exactly matches the move-loop window).
        DockNavigator? navigator;

        public DockFloatWindow(DockPanel owner)
        {
            this.owner = owner;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            pane = new DockPane(owner)
            {
                State = DockState.Float,
                Dock = DockStyle.Fill
            };
            Controls.Add(pane);
            // DPI-scaled minimum size + corner radius. Can't use Dpi in the ctor (handle not created
            // yet), so defer to OnHandleCreated which fires once the window materializes.
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            float dpi = 1f; try { dpi = owner.IsHandleCreated ? owner.Dpi : Dpi; } catch { }
            MinimumSize = new Size((int)(220 * dpi), (int)(140 * dpi));
            Radius = (int)(6 * dpi);
        }

        internal DockPane Pane { get { return pane; } }

        [Browsable(false)]
        public IDockContent? ActiveContent { get { return pane.ActiveContent; } }

        internal bool IsEmpty { get { return pane.Contents.Count == 0; } }

        internal void AddContent(IDockContent content)
        {
            pane.AddContent(content);
            content.DockState = DockState.Float;
            if (!string.IsNullOrEmpty(content.DockTitle)) Text = content.DockTitle;
        }

        internal void RemoveContent(IDockContent content)
        {
            pane.RemoveContent(content, false);
        }

        internal bool Contains(IDockContent content) { return pane.Contains(content); }

        /// <summary>Begin a native window drag. Defers to the next message-pump iteration so that
        /// <c>Show()</c>-triggered activation and paint messages clear the queue before the synthesized
        /// caption-press arrives. This is the pragmatic workaround for the fact that we're creating a
        /// brand-new HWND mid-drag rather than promoting the existing one (the legacy pattern).</summary>
        // WM_NCLBUTTONDOWN does not work on BorderlessForm because WM_NCHITTEST always returns HTCLIENT
        // (no real non-client area). SC_MOVE bypasses NCHITTEST and enters the native move loop directly.
        const int SC_MOVE = 0xF010;
        const int HTCAPTION = 2;

        public void BeginSystemDrag(bool async = false)
        {
            if (!IsHandleCreated) return;
            Action act = new Action(() =>
            {
                if (!IsHandleCreated || IsDisposed) return;
                try
                {
                    Win32.User32.ReleaseCapture();
                    // Raw int literals — the typed (uint/IntPtr) overload silently misroutes this
                    // combination and the native move-loop never engages. 0x0112 = WM_SYSCOMMAND,
                    // 61456|2 = SC_MOVE|HTCAPTION.
                    Win32.User32.SendMessage(Handle, 0x0112, 61456 | 2, IntPtr.Zero);
                }
                catch { /* ignore */ }
            });

            if (async) BeginInvoke(act);
            else act();
        }

        #region Drag tracking — show navigator, detect zone

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch ((Win32.User32.WindowMessage)m.Msg)
            {
                case Win32.User32.WindowMessage.WM_ENTERSIZEMOVE: OnDragBegin(); break;
                case Win32.User32.WindowMessage.WM_EXITSIZEMOVE: OnDragEnd(); break;
                case Win32.User32.WindowMessage.WM_MOVING: OnDragMove(); break;
            }
            base.WndProc(ref m);
        }

        void OnDragBegin()
        {
            if (navigator == null) navigator = new DockNavigator();
            navigator.Show(owner);
        }

        void OnDragMove()
        {
            if (navigator == null) return;
            var screen = Cursor.Position;
            navigator.Update(screen);
        }

        void OnDragEnd()
        {
            if (navigator == null) return;
            var zone = navigator.HitZone!;
            var target = navigator.HitPane!;
            navigator.Dispose();
            navigator = null;
            if (zone == DockZone.None) return;

            // Move all contents back into the DockPanel
            var list = new List<IDockContent>(pane.Contents);
            for (int i = 0; i < list.Count; i++)
            {
                pane.RemoveContent(list[i], false);
                owner.DropFromFloat(list[i], zone.Value, target);
            }
            BeginInvoke(new Action(Close));
        }

        #endregion

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (navigator != null) { try { navigator.Dispose(); } catch { } navigator = null; }
            base.OnFormClosed(e);
        }
    }
}
