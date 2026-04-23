// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// DockAutoHideStrip — edge tab strip for auto-hidden contents.
    /// Hover over a tab for 300 ms opens a flyout; hovering out of the flyout for 500 ms closes it.
    /// A click on a tab pins the flyout open (disables close timer). The flyout slides in/out with
    /// ease-out cubic over 150 ms, matching OutlookBar's animation pattern.
    /// </summary>
    [ToolboxItem(false)]
    public class DockAutoHideStrip : IControl
    {
        readonly DockPanel owner;
        readonly List<IDockContent> items = new List<IDockContent>();
        readonly DockPosition edge;

        Rectangle[] tabs = new Rectangle[0];
        int hoverIndex = -1;
        int activeIndex = -1;
        DockFlyoutOverlay? flyout;

        // Hover-open timer: starts on OnMouseMove, fires after 300ms unless cursor leaves.
        readonly System.Windows.Forms.Timer hoverOpenTimer;
        int pendingOpenIndex = -1;

        public DockAutoHideStrip(DockPanel owner, DockPosition edge)
        {
            this.owner = owner;
            this.edge = edge;
            Dock = DockStyle.None;
            BackColor = Color.Transparent;
            Visible = false;
            itemsReadOnly = items.AsReadOnly();

            hoverOpenTimer = new System.Windows.Forms.Timer { Interval = 300 };
            hoverOpenTimer.Tick += HoverOpenTimer_Tick;
        }

        [Browsable(false)]
        public bool HasContent { get { return items.Count > 0; } }

#if NET40
        readonly ReadOnlyCollection<IDockContent> itemsReadOnly;
        [Browsable(false)]
        public ReadOnlyCollection<IDockContent> Contents  => itemsReadOnly;
#else
        readonly IReadOnlyList<IDockContent> itemsReadOnly;
        [Browsable(false)]
        public IReadOnlyList<IDockContent> Contents => itemsReadOnly;
#endif

        internal void AddContent(IDockContent content)
        {
            if (!items.Contains(content)) items.Add(content);
            Visible = true;
            owner.PerformLayout();
            RelayoutTabs();
            Invalidate();
        }

        internal void RemoveContent(IDockContent content)
        {
            items.Remove(content);
            if (items.Count == 0) Visible = false;
            RelayoutTabs();
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RelayoutTabs();
        }

        void RelayoutTabs()
        {
            if (items.Count == 0) { tabs = new Rectangle[0]; return; }
            float dpi = Dpi;
            int pad = (int)(2 * dpi);
            int tabW = (int)(92 * dpi);
            int tabH = (int)(20 * dpi);
            tabs = new Rectangle[items.Count];
            bool horizontal = edge == DockPosition.Top || edge == DockPosition.Bottom;
            if (horizontal)
            {
                int x = pad;
                for (int i = 0; i < items.Count; i++)
                {
                    tabs[i] = new Rectangle(x, pad, tabW, Math.Max(tabH, ClientRectangle.Height - pad * 2));
                    x += tabW + pad;
                }
            }
            else
            {
                int y = pad;
                for (int i = 0; i < items.Count; i++)
                {
                    tabs[i] = new Rectangle(pad, y, Math.Max(tabH, ClientRectangle.Width - pad * 2), tabW);
                    y += tabW + pad;
                }
            }
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var r = ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;
            var scheme = owner.ColorScheme;
            string name = nameof(DockPanel);
            float dpi = Dpi;

            var colBg = Colour.BgElevated.Get(name, scheme);
            var colTabHot = Colour.PrimaryBg.Get(name, scheme);
            var colTabIdle = Colour.FillSecondary.Get(name, scheme);
            var colTextHot = Colour.Primary.Get(name, scheme);
            var colTextIdle = Colour.Text.Get(name, scheme);
            var colTabBorder = Colour.BorderColor.Get(name, scheme);
            float tabRadius = 2 * dpi;

            g.Fill(colBg, r);
            // Each tab gets its own crisp border — the strip itself draws no outer edge line
            // (the strip just hosts the tabs; any separator between strip and adjacent pane is the pane's responsibility).
            for (int i = 0; i < tabs.Length; i++)
            {
                var rect = tabs[i];
                bool hot = i == hoverIndex || i == activeIndex;
                using (var path = rect.RoundPath(tabRadius))
                {
                    g.Fill(hot ? colTabHot : colTabIdle, path);
                    g.Draw(colTabBorder, 1.1f * dpi, path);
                }
                DrawStripTitle(g, items[i].DockTitle ?? string.Empty, hot ? colTextHot : colTextIdle, rect);
            }
            base.OnDraw(e);
        }

        void DrawStripTitle(Canvas g, string title, Color fg, Rectangle rect)
        {
            bool horizontal = edge == DockPosition.Top || edge == DockPosition.Bottom;
            if (horizontal)
            {
                g.String(title, owner.Font, fg, rect, FormatFlags.Center);
                return;
            }
            float angle = edge == DockPosition.Left ? -90f : 90f;
            var cx = rect.X + rect.Width / 2f;
            var cy = rect.Y + rect.Height / 2f;
            var state = g.Save();
            try
            {
                g.TranslateTransform(cx, cy);
                g.RotateTransform(angle);
                var rotated = new RectangleF(-rect.Height / 2f, -rect.Width / 2f, rect.Height, rect.Width);
                g.String(title, owner.Font, fg, rotated, FormatFlags.Center);
            }
            finally
            {
                g.Restore(state);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int h = -1;
            for (int i = 0; i < tabs.Length; i++) if (tabs[i].Contains(e.Location)) { h = i; break; }
            if (h != hoverIndex) { hoverIndex = h; Invalidate(); }
            // If the user hovers over a different (closed) tab, cancel any pending open and schedule for the new one.
            if (h != pendingOpenIndex)
            {
                hoverOpenTimer.Stop();
                pendingOpenIndex = h;
                if (h >= 0)
                {
                    // If the flyout is already showing this tab, don't reopen.
                    if (flyout == null || activeIndex != h) hoverOpenTimer.Start();
                }
            }
            // User is re-hovering the strip — cancel any pending close from the flyout side.
            flyout?.CancelClose();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (hoverIndex != -1) { hoverIndex = -1; Invalidate(); }
            hoverOpenTimer.Stop();
            pendingOpenIndex = -1;
            // If a flyout is open and we moved off the strip without entering it, start close countdown.
            flyout?.BeginCloseCountdown();
        }

        void HoverOpenTimer_Tick(object? sender, EventArgs e)
        {
            hoverOpenTimer.Stop();
            if (pendingOpenIndex < 0 || pendingOpenIndex >= items.Count) return;
            OpenFlyout(pendingOpenIndex, pinned: false);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].Contains(e.Location))
                {
                    // Click pins the flyout open — cancels any close timer.
                    if (activeIndex == i && flyout != null) { flyout.Pin(); return; }
                    OpenFlyout(i, pinned: true);
                    return;
                }
            }
        }

        void OpenFlyout(int index, bool pinned)
        {
            hoverOpenTimer.Stop();
            if (activeIndex == index && flyout != null) { if (pinned) flyout.Pin(); return; }
            CloseFlyout();
            activeIndex = index;
            flyout = new DockFlyoutOverlay(owner, this, items[index], edge);
            flyout.Closed += () => { flyout = null; activeIndex = -1; Invalidate(); };
            flyout.Show();
            if (pinned) flyout.Pin();
            Invalidate();
        }

        void CloseFlyout()
        {
            if (flyout != null) { try { flyout.Close(); } catch { } flyout = null; }
            activeIndex = -1;
        }

        internal void RefreshFlyoutBounds()
        {
            flyout?.UpdateBounds();
        }

        internal void ReturnFromFlyout(IDockContent content, bool pin)
        {
            CloseFlyout();
            if (pin)
            {
                RemoveContent(content);
                content.DockState = DockState.Docked;
                var pos = edge == DockPosition.None ? DockPosition.Left : edge;
                owner.DockContent(content, pos);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Close the active flyout before we go — otherwise its IMessageFilter and internal timers leak.
                CloseFlyout();
                hoverOpenTimer.Stop();
                hoverOpenTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>Inline overlay: adds a <see cref="DockPane"/> as a child of the <see cref="DockPanel"/>.
        /// Supports hover-open / hover-close semantics with a slide animation and a pin toggle.
        /// Dismisses on outside click via an <see cref="IMessageFilter"/> (once pinned or hovered-into).</summary>
        sealed class DockFlyoutOverlay : IMessageFilter
        {
            readonly DockPanel dockPanel;
            readonly DockPosition edge;
            readonly DockPane pane;
            readonly IDockContent content;
            bool closing;
            bool pinned;

            Rectangle targetBounds;
            Rectangle startBounds;

            // Slide animation
            readonly System.Windows.Forms.Timer animTimer;
            long animStartTicks;
            const int AnimDurationMs = 150;
            bool animating;
            bool animClosing; // true = sliding out

            // Hover-close timer (500 ms after mouse leaves the flyout)
            readonly System.Windows.Forms.Timer closeTimer;

            public event Action? Closed;

            public DockFlyoutOverlay(DockPanel dockPanel, DockAutoHideStrip strip, IDockContent content, DockPosition edge)
            {
                this.dockPanel = dockPanel;
                this.content = content;
                this.edge = edge;

                pane = new DockPane(dockPanel);
                pane.State = DockState.AutoHide;
                pane.Position = edge;
                pane.Dock = DockStyle.None;
                pane.AddContent(content);
                content.DockStateChanged += OnContentDockStateChanged;

                pane.MouseEnter += OnPaneMouseEnter;
                pane.MouseLeave += OnPaneMouseLeave;

                animTimer = new System.Windows.Forms.Timer { Interval = 16 };
                animTimer.Tick += OnAnimTick;
                closeTimer = new System.Windows.Forms.Timer { Interval = 500 };
                closeTimer.Tick += (s, e) => { closeTimer.Stop(); if (!pinned) dockPanel.BeginInvoke(new Action(Close)); };
            }

            public void Pin()
            {
                pinned = true;
                closeTimer.Stop();
            }

            public void CancelClose()
            {
                closeTimer.Stop();
            }

            public void BeginCloseCountdown()
            {
                if (pinned) return;
                closeTimer.Stop();
                closeTimer.Start();
            }

            public void Show()
            {
                dockPanel.Controls.Add(pane);
                ComputeTargetBounds();
                // Slide in from a zero-size rect adjacent to the strip.
                startBounds = CollapsedBoundsFor(targetBounds);
                pane.Bounds = startBounds;
                pane.BringToFront();
                Application.AddMessageFilter(this);
                StartAnimation(closing: false);
            }

            public void Close()
            {
                if (closing) return;
                closing = true;
                closeTimer.Stop();
                animTimer.Stop();
                Application.RemoveMessageFilter(this);
                content.DockStateChanged -= OnContentDockStateChanged;
                pane.MouseEnter -= OnPaneMouseEnter;
                pane.MouseLeave -= OnPaneMouseLeave;
                if (pane.Contains(content)) pane.RemoveContent(content, dispose: false);
                if (dockPanel.Controls.Contains(pane)) dockPanel.Controls.Remove(pane);
                try { pane.Dispose(); } catch { }
                try { animTimer.Dispose(); } catch { }
                try { closeTimer.Dispose(); } catch { }
                Closed?.Invoke();
            }

            public void UpdateBounds()
            {
                ComputeTargetBounds();
                if (!animating) pane.Bounds = targetBounds;
            }

            void OnPaneMouseEnter(object? sender, EventArgs e)
            {
                closeTimer.Stop();
            }

            void OnPaneMouseLeave(object? sender, EventArgs e)
            {
                if (pinned) return;
                closeTimer.Stop();
                closeTimer.Start();
            }

            void ComputeTargetBounds()
            {
                var area = dockPanel.ClientRectangle;
                float dpi = 1f; try { dpi = dockPanel.Dpi; } catch { }
                // Strip width on-screen is AutoHideStripSize * dpi (see DockPanel.PerformLayoutPanes);
                // using the raw property here offsets the flyout by ~1 strip-width at HiDPI.
                int strip = (int)(dockPanel.AutoHideStripSize * dpi);
                int majorW = (int)Math.Max(260 * dpi, area.Width * 0.3f);
                int majorH = (int)Math.Max(220 * dpi, area.Height * 0.35f);
                switch (edge)
                {
                    case DockPosition.Left:
                        targetBounds = new Rectangle(strip, 0, majorW, area.Height);
                        break;
                    case DockPosition.Right:
                        targetBounds = new Rectangle(area.Width - strip - majorW, 0, majorW, area.Height);
                        break;
                    case DockPosition.Top:
                        targetBounds = new Rectangle(0, strip, area.Width, majorH);
                        break;
                    case DockPosition.Bottom:
                        targetBounds = new Rectangle(0, area.Height - strip - majorH, area.Width, majorH);
                        break;
                    default:
                        targetBounds = new Rectangle(strip, 0, majorW, area.Height);
                        break;
                }
            }

            Rectangle CollapsedBoundsFor(Rectangle full)
            {
                switch (edge)
                {
                    case DockPosition.Left: return new Rectangle(full.X, full.Y, 0, full.Height);
                    case DockPosition.Right: return new Rectangle(full.Right, full.Y, 0, full.Height);
                    case DockPosition.Top: return new Rectangle(full.X, full.Y, full.Width, 0);
                    case DockPosition.Bottom: return new Rectangle(full.X, full.Bottom, full.Width, 0);
                }
                return new Rectangle(full.X, full.Y, 0, full.Height);
            }

            void StartAnimation(bool closing)
            {
                if (!dockPanel.IsHandleCreated || AntdUI.Config.Animation == false)
                {
                    pane.Bounds = closing ? CollapsedBoundsFor(targetBounds) : targetBounds;
                    // Without this, the Close() path in OnAnimTick is never reached when animations are
                    // disabled — the flyout collapses to a zero-size rect but stays parented and the
                    // message filter + event subscriptions leak.
                    if (closing) Close();
                    return;
                }
                animClosing = closing;
                animStartTicks = Environment.TickCount;
                animating = true;
                animTimer.Start();
            }

            void OnAnimTick(object? sender, EventArgs e)
            {
                long elapsed = Environment.TickCount - animStartTicks;
                double t = (double)elapsed / AnimDurationMs;
                if (t >= 1.0)
                {
                    animTimer.Stop();
                    animating = false;
                    pane.Bounds = animClosing ? CollapsedBoundsFor(targetBounds) : targetBounds;
                    if (animClosing) Close();
                    return;
                }
                double eased = 1.0 - Math.Pow(1.0 - t, 3);
                Rectangle from, to;
                if (animClosing) { from = targetBounds; to = CollapsedBoundsFor(targetBounds); }
                else { from = CollapsedBoundsFor(targetBounds); to = targetBounds; }
                pane.Bounds = Interpolate(from, to, eased);
            }

            static Rectangle Interpolate(Rectangle a, Rectangle b, double t)
            {
                int x = (int)Math.Round(a.X + (b.X - a.X) * t);
                int y = (int)Math.Round(a.Y + (b.Y - a.Y) * t);
                int w = (int)Math.Round(a.Width + (b.Width - a.Width) * t);
                int h = (int)Math.Round(a.Height + (b.Height - a.Height) * t);
                return new Rectangle(x, y, w, h);
            }

            void OnContentDockStateChanged(object? sender, EventArgs e)
            {
                if (closing) return;
                if (content.DockState != DockState.AutoHide)
                {
                    dockPanel.BeginInvoke(new Action(Close));
                }
            }

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                if (closing) return false;
                var wm = (Win32.User32.WindowMessage)m.Msg;
                if (wm == Win32.User32.WindowMessage.WM_LBUTTONDOWN ||
                    wm == Win32.User32.WindowMessage.WM_RBUTTONDOWN ||
                    wm == Win32.User32.WindowMessage.WM_MBUTTONDOWN ||
                    wm == Win32.User32.WindowMessage.WM_NCLBUTTONDOWN)
                {
                    var screen = Cursor.Position;
                    var paneScreen = pane.RectangleToScreen(pane.ClientRectangle);
                    if (!paneScreen.Contains(screen))
                    {
                        dockPanel.BeginInvoke(new Action(Close));
                    }
                }
                return false;
            }
        }
    }

}
