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
    /// DockPane — single-region tabbed container. Composes <see cref="TabHeader"/> for multi-content panes,
    /// parents the active <see cref="IDockContent.ContentControl"/> into a child host Panel.
    /// Renders its own title bar via the Canvas API (theme-aware).
    /// </summary>
    [ToolboxItem(false)]
    public class DockPane : IControl
    {
        #region Fields

        internal DockPanel Owner { get; }
        readonly List<IDockContent> contents = new List<IDockContent>();
        readonly TabHeader tabs;
        readonly Panel host;
        int selectedIndex = -1;
        bool internalSelect;

        // Geometry cached from OnDraw / Relayout.
        Rectangle titleRect;
        Rectangle closeRect;
        Rectangle pinRect;
        Rectangle maxRect;
        bool hoverClose;
        bool hoverPin;
        bool hoverMax;

        // Notification flash: a sine-wave pulse blended into the title background while an inactive pane
        // has a new notification. Auto-stops when the pane becomes active (user saw it) or duration elapses.
        System.Windows.Forms.Timer? flashTimer;
        long flashStartTicks;
        int flashDurationMs;
        float flashAmplitude; // 0..1 — sampled each tick; combined with palette in OnDraw.

        #endregion

        #region Construction

        public DockPane(DockPanel owner)
        {
            Owner = owner;
            contentsReadOnly = contents.AsReadOnly();
            BackColor = Color.Transparent;
            Dock = DockStyle.None;

            tabs = new TabHeader
            {
                ShowAdd = false,
                DragSort = true,
                Dock = DockStyle.None,
                Visible = false
            };
            tabs.TabChanged += Tabs_TabChanged;
            tabs.TabClosing += Tabs_TabClosing;
            tabs.MouseDown += Tabs_MouseDown;
            tabs.MouseMove += Tabs_MouseMove;
            tabs.MouseUp += Tabs_MouseUp;

            host = new Panel { BackColor = Color.Transparent, Dock = DockStyle.None };

            Controls.Add(tabs);
            Controls.Add(host);
        }

        #endregion

        #region Properties

        /// <summary>Position inside the owning <see cref="DockPanel"/>.</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DockPosition Position { get; internal set; }

        /// <summary>Current visible state (Docked / Float / AutoHide).</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DockState State { get; internal set; } = DockState.Docked;

        /// <summary>Relative size hint (0..1) of the pane along its dock axis.</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float SizeRatio { get; internal set; } = 0.25f;

        /// <summary>Contents hosted in this pane.</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ReadOnlyCollection<IDockContent> Contents { get { return contentsReadOnly; } }
        readonly ReadOnlyCollection<IDockContent> contentsReadOnly;

        /// <summary>Index of the active tab (or -1 if empty).</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { SelectContent(value, true); }
        }

        /// <summary>Currently-visible content (or null).</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDockContent? ActiveContent
        {
            get
            {
                if (selectedIndex < 0 || selectedIndex >= contents.Count) return null;
                return contents[selectedIndex];
            }
        }

        int titleHeight = 26;
        /// <summary>Height of the title bar (unscaled; multiplied by Dpi at layout time).</summary>
        [Description("Title bar height"), Category("Layout"), DefaultValue(26)]
        public int TitleHeight
        {
            get { return titleHeight; }
            set
            {
                if (titleHeight == value) return;
                titleHeight = value;
                PerformLayout();
                Invalidate();
            }
        }

        int tabHeight = 28;
        [Description("Tab strip height"), Category("Layout"), DefaultValue(28)]
        public int TabHeight
        {
            get { return tabHeight; }
            set
            {
                if (tabHeight == value) return;
                tabHeight = value;
                PerformLayout();
                Invalidate();
            }
        }

        #endregion

        #region Content API

        internal void AddContent(IDockContent content)
        {
            if (content == null || contents.Contains(content)) return;
            contents.Add(content);
            content.OwnerPane = this;

            var item = new TagTabItem(content.DockTitle, content.DockIconSvg);
            item.ID = content.PersistId;
            item.ShowClose = content.CanClose;
            tabs.Items.Add(item);

            var ctl = content.ContentControl;
            ctl.Visible = false;
            ctl.Dock = DockStyle.Fill;
            if (!host.Controls.Contains(ctl)) host.Controls.Add(ctl);

            // First content auto-selects; subsequent adds preserve the user's current tab.
            if (selectedIndex < 0) SelectContent(0, true);

            UpdateTabVisibility();
            PerformLayout();
            Invalidate();
        }

        internal bool RemoveContent(IDockContent content, bool dispose)
        {
            int idx = contents.IndexOf(content);
            if (idx < 0) return false;

            contents.RemoveAt(idx);
            if (idx < tabs.Items.Count) tabs.Items.RemoveAt(idx);

            var ctl = content.ContentControl;
            if (host.Controls.Contains(ctl)) host.Controls.Remove(ctl);
            if (content.OwnerPane == this) content.OwnerPane = null;

            if (contents.Count == 0) selectedIndex = -1;
            else
            {
                int newSel = idx;
                if (newSel >= contents.Count) newSel = contents.Count - 1;
                SelectContent(newSel, true);
            }

            UpdateTabVisibility();
            PerformLayout();
            Invalidate();

            if (dispose)
            {
                try { ctl.Dispose(); } catch { }
            }
            return true;
        }

        internal bool Contains(IDockContent content) { return contents.Contains(content); }

        void SelectContent(int index, bool updateTab)
        {
            if (index < -1 || index >= contents.Count) return;
            if (selectedIndex == index) return;
            selectedIndex = index;
            for (int i = 0; i < contents.Count; i++)
            {
                var c = contents[i].ContentControl;
                bool shouldBeVisible = (i == index);
                if (c.Visible != shouldBeVisible) c.Visible = shouldBeVisible;
            }
            if (updateTab && tabs.Items.Count > 0 && index >= 0 && tabs.SelectedIndex != index)
            {
                internalSelect = true;
                try { tabs.SelectedIndex = index; }
                finally { internalSelect = false; }
            }
            Invalidate();
            if (index >= 0 && index < contents.Count) Owner.NotifyActiveContent(contents[index]);
        }

        void UpdateTabVisibility()
        {
            tabs.Visible = contents.Count > 1;
        }

        #endregion

        #region TabHeader integration

        void Tabs_TabChanged(object? sender, TabChangedEventArgs e)
        {
            if (internalSelect) return;
            SelectContent(e.Index, false);
        }

        void Tabs_TabClosing(object? sender, TabCloseEventArgs e)
        {
            if (e.Index < 0 || e.Index >= contents.Count) return;
            var c = contents[e.Index];
            if (!c.CanClose) { e.Cancel = true; return; }
            e.Cancel = true; // prevent TabHeader removal — we re-sync below (either Hide or RemoveContent)
            // Fire the cancellable pre-close event first.
            if (!Owner.RaiseBeforeContentClosed(c)) return;
            if (c.HideOnClose) Owner.HideContent(c);
            else Owner.RemoveContentCore(c);
        }

        // Drag a tab out of the strip → detach that content as a floating window.
        // TabHeader.DragSort already handles reorder within the strip; we only intervene once the cursor
        // leaves the strip's client rect by more than the escape threshold.
        bool tabDragArmed;
        void Tabs_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || Owner.Locked) return;
            tabDragArmed = true;
        }
        void Tabs_MouseUp(object? sender, MouseEventArgs e) { tabDragArmed = false; }
        void Tabs_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!tabDragArmed || Owner.Locked) return;
            if ((e.Button & MouseButtons.Left) == 0) { tabDragArmed = false; return; }
            int escape = (int)(30 * Dpi);
            var bounds = tabs.ClientRectangle;
            if (e.Y >= -escape && e.Y <= bounds.Height + escape && e.X >= -escape && e.X <= bounds.Width + escape) return;

            var content = ActiveContent;
            if (content == null || !content.CanFloat) return;
            tabDragArmed = false;
            try { Win32.User32.ReleaseCapture(); } catch { /* ignore */ }
            tabs.Capture = false;
            Owner.BeginDragFloat(content, tabs.PointToScreen(e.Location));
        }

        #endregion

        #region Layout

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Relayout();
        }

        internal void Relayout()
        {
            var r = ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;
            float dpi = Dpi;
            int _titleH = (int)(titleHeight * dpi);
            int _tabH = (int)(tabHeight * dpi);
            int pad = (int)(1 * dpi);

            titleRect = new Rectangle(r.X, r.Y, r.Width, _titleH);

            int hostTop = r.Y + _titleH;
            int hostBottom = r.Bottom - pad;

            if (tabs.Visible)
            {
                var tabRect = new Rectangle(r.X + pad, hostBottom - _tabH, r.Width - pad * 2, _tabH);
                tabs.SetBounds(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height);
                hostBottom = tabRect.Y;
            }

            host.SetBounds(r.X + pad, hostTop, r.Width - pad * 2, Math.Max(0, hostBottom - hostTop));

            // Title icons (right-anchored): close, then maximize (if enabled), then pin. When the maximize
            // icon is hidden, pin collapses into its slot so close/pin stay visually adjacent.
            float sz = 16 * dpi;
            float gap = 4 * dpi;
            float py = titleRect.Y + (titleRect.Height - sz) / 2f;
            float pxR = titleRect.Right - 6 * dpi - sz;
            closeRect = new Rectangle((int)pxR, (int)py, (int)sz, (int)sz);
            bool showMaxSlot = Owner.AllowMaximize;
            float pxM = pxR - gap - sz;
            maxRect = showMaxSlot
                ? new Rectangle((int)pxM, (int)py, (int)sz, (int)sz)
                : Rectangle.Empty;
            float pxP = (showMaxSlot ? pxM : pxR) - gap - sz;
            pinRect = new Rectangle((int)pxP, (int)py, (int)sz, (int)sz);
        }

        #endregion

        #region Notification flash

        /// <summary>Start a gentle pulsing highlight on the title bar for <paramref name="durationMs"/> ms
        /// (default 3 seconds). Stops early if the pane becomes focused. Ignored if the pane is already focused.</summary>
        public void Flash(int durationMs = 3000)
        {
            if (Owner.ActiveContent == ActiveContent && ActiveContent != null) return; // already in view — no need
            // Respect the global animation toggle — a 3-second pulsing title bar is still motion.
            if (AntdUI.Config.Animation == false) return;
            flashDurationMs = Math.Max(500, durationMs);
            flashStartTicks = Environment.TickCount;
            if (flashTimer == null)
            {
                flashTimer = new System.Windows.Forms.Timer { Interval = 16 };
                flashTimer.Tick += OnFlashTick;
            }
            flashTimer.Start();
        }

        void OnFlashTick(object? sender, EventArgs e)
        {
            long elapsed = Environment.TickCount - flashStartTicks;
            if (elapsed >= flashDurationMs || (Owner.ActiveContent == ActiveContent && ActiveContent != null))
            {
                flashTimer!.Stop();
                flashAmplitude = 0;
                Invalidate();
                return;
            }
            // Two-Hz sine (2 cycles per second), half-rectified so amplitude sits in 0..1.
            double t = elapsed / 1000.0;
            flashAmplitude = (float)(0.5 + 0.5 * Math.Sin(t * 2 * Math.PI * 1.5));
            Invalidate();
        }

        static Color BlendColors(Color from, Color to, float t)
        {
            if (t <= 0) return from;
            if (t >= 1) return to;
            int r = (int)(from.R + (to.R - from.R) * t);
            int g = (int)(from.G + (to.G - from.G) * t);
            int b = (int)(from.B + (to.B - from.B) * t);
            return Color.FromArgb(r, g, b);
        }

        #endregion

        #region Rendering

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var r = ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;
            float dpi = Dpi;
            var scheme = Owner.ColorScheme;

            // Resolve colour tokens once per paint.
            string name = nameof(DockPanel);
            var colBg = Colour.BgContainer.Get(name, scheme);
            var colTitleBgFocus = Colour.PrimaryBg.Get(name, scheme);
            var colTitleBgIdle = Colour.BgElevated.Get(name, scheme);
            var colPrimary = Colour.Primary.Get(name, scheme);
            var colText = Colour.Text.Get(name, scheme);
            var colTextSecondary = Colour.TextSecondary.Get(name, scheme);
            var colTextTertiary = Colour.TextTertiary.Get(name, scheme);
            var colBorder = Colour.BorderColor.Get(name, scheme);

            g.Fill(colBg, r);

            var active = ActiveContent;
            bool hasFocus = Owner.ActiveContent == active && active != null;
            // If the pane is flashing (unseen notification) and not currently focused, blend the title bar
            // toward Colour.WarningBg by the current pulse amplitude so the "hey, look at me" cue reads.
            Color titleBg = hasFocus ? colTitleBgFocus : colTitleBgIdle;
            if (flashAmplitude > 0 && !hasFocus)
            {
                var warnBg = Colour.WarningBg.Get(name, scheme);
                titleBg = BlendColors(titleBg, warnBg, flashAmplitude);
            }
            g.Fill(titleBg, titleRect);

            if (active != null)
            {
                int iconSize = (int)(14 * dpi);
                int titlePad = (int)(8 * dpi);
                int iconGap = (int)(6 * dpi);
                int tx = titleRect.X + titlePad;
                int iy = titleRect.Y + (titleRect.Height - iconSize) / 2;

                if (!string.IsNullOrEmpty(active.DockIconSvg))
                {
                    var iconRect = new Rectangle(tx, iy, iconSize, iconSize);
                    using (var bmp = SvgExtend.GetImgExtend(active.DockIconSvg!, iconRect, colTextSecondary))
                    {
                        if (bmp != null) g.Image(bmp, iconRect);
                    }
                    tx += iconSize + iconGap;
                }

                var titleColor = hasFocus ? colPrimary : colText;
                // Determine leftmost icon to clamp the title text — pin (if visible), else maximize, else close.
                bool showPin = active.CanAutoHide && !Owner.Locked;
                bool showMax = Owner.AllowMaximize && !Owner.Locked && State != DockState.AutoHide;
                bool showClose = active.CanClose && !Owner.Locked;
                int textRight = showPin ? pinRect.X - titlePad
                              : showMax ? maxRect.X - titlePad
                              : showClose ? closeRect.X - titlePad
                              : titleRect.Right - titlePad;
                var textRect = new Rectangle(tx, titleRect.Y, Math.Max(0, textRight - tx), titleRect.Height);
                g.String(active.DockTitle ?? string.Empty, Owner.Font, titleColor, textRect,
                    FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis);

                if (showPin)
                {
                    var pinColor = hoverPin ? colPrimary : colTextTertiary;
                    string pinSvg = State == DockState.AutoHide ? SvgDb.IcoPinOutlined : SvgDb.IcoPinFilled;
                    using (var bmp = SvgExtend.GetImgExtend(pinSvg, pinRect, pinColor))
                    {
                        if (bmp != null) g.Image(bmp, pinRect);
                    }
                }

                if (showMax)
                {
                    var maxColor = hoverMax ? colPrimary : colTextTertiary;
                    string maxSvg = Owner.MaximizedPane == this ? SvgDb.IcoAppRestore : SvgDb.IcoAppMax;
                    using (var bmp = SvgExtend.GetImgExtend(maxSvg, maxRect, maxColor))
                    {
                        if (bmp != null) g.Image(bmp, maxRect);
                    }
                }

                if (showClose)
                {
                    var closeColor = hoverClose ? colPrimary : colTextTertiary;
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoClose, closeRect, closeColor))
                    {
                        if (bmp != null) g.Image(bmp, closeRect);
                    }
                }
            }

            g.DrawLine(colBorder, 1 * dpi, new PointF(r.X, titleRect.Bottom), new PointF(r.Right, titleRect.Bottom));
            g.Draw(colBorder, 1.5f * dpi, r);
            base.OnDraw(e);
        }

        #endregion

        #region Mouse — drag-to-float + hit-test icons

        Point dragOrigin;
        bool dragArmed;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            if (!Owner.Locked && closeRect.Contains(e.Location) && ActiveContent != null && ActiveContent.CanClose)
            {
                var c = ActiveContent;
                if (!Owner.RaiseBeforeContentClosed(c)) return;
                if (c.HideOnClose) Owner.HideContent(c);
                else Owner.RemoveContentCore(c);
                return;
            }
            if (Owner.AllowMaximize && !Owner.Locked && maxRect.Contains(e.Location) && State != DockState.AutoHide)
            {
                Owner.ToggleMaximize(this);
                return;
            }
            if (!Owner.Locked && pinRect.Contains(e.Location) && ActiveContent != null && ActiveContent.CanAutoHide)
            {
                if (State == DockState.AutoHide)
                {
                    var back = Position == DockPosition.None ? DockPosition.Left : Position;
                    Owner.DockContent(ActiveContent, back);
                }
                else Owner.AutoHideContent(ActiveContent);
                return;
            }
            if (titleRect.Contains(e.Location) && ActiveContent != null)
            {
                // Arm the drag BEFORE activating — ActivateContent calls BringToFront, which on some
                // Windows configurations cancels the pending mouse-button state that OnMouseMove relies
                // on to detect drag. Activation is deferred to MouseUp if no drag happens.
                if (!Owner.Locked)
                {
                    dragOrigin = e.Location;
                    dragArmed = true;
                }
                else Owner.ActivateContent(ActiveContent);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (!Owner.AllowMaximize || Owner.Locked) return;
            if (e.Button != MouseButtons.Left) return;
            if (!titleRect.Contains(e.Location)) return;
            if (closeRect.Contains(e.Location) || maxRect.Contains(e.Location) || pinRect.Contains(e.Location)) return;
            if (State == DockState.AutoHide || State == DockState.Float) return;
            Owner.ToggleMaximize(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool newHoverClose = closeRect.Contains(e.Location);
            bool newHoverPin = pinRect.Contains(e.Location);
            bool newHoverMax = maxRect.Contains(e.Location);
            if (newHoverClose != hoverClose || newHoverPin != hoverPin || newHoverMax != hoverMax)
            {
                hoverClose = newHoverClose;
                hoverPin = newHoverPin;
                hoverMax = newHoverMax;
                Invalidate();
            }

            if (dragArmed && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                int dx = Math.Abs(e.X - dragOrigin.X);
                int dy = Math.Abs(e.Y - dragOrigin.Y);
                int threshold = (int)(6 * Dpi);
                if (dx > threshold || dy > threshold)
                {
                    dragArmed = false;
                    var content = ActiveContent;
                    if (content == null) return;
                    if (State == DockState.Float)
                    {
                        // Already floating — move the hosting DockFloatWindow rather than re-floating.
                        var host = FindForm() as DockFloatWindow;
                        if (host != null)
                        {
                            try { Win32.User32.ReleaseCapture(); } catch { /* ignore */ }
                            Capture = false;
                            host.BeginSystemDrag(false);
                        }
                    }
                    else if (content.CanFloat)
                    {
                        // Release our capture BEFORE spawning the float window; the float window's BeginSystemDrag
                        // will issue its own ReleaseCapture, but that's on ITS HWND — it doesn't free ours.
                        // Without this, Windows still thinks this pane owns the pointer and the synthesized
                        // WM_SYSCOMMAND/SC_MOVE on the new HWND can't engage the native move loop.
                        try { Win32.User32.ReleaseCapture(); } catch { /* ignore */ }
                        Capture = false;
                        Owner.BeginDragFloat(content, PointToScreen(e.Location));
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            // If the user clicked the title without moving past the threshold, we didn't reach the
            // drag-initiation path — activate now so clicks still focus the pane.
            if (dragArmed && e.Button == MouseButtons.Left && titleRect.Contains(e.Location) && ActiveContent != null)
            {
                Owner.ActivateContent(ActiveContent);
            }
            dragArmed = false;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (hoverClose || hoverPin || hoverMax)
            {
                hoverClose = false;
                hoverPin = false;
                hoverMax = false;
                Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                flashTimer?.Stop();
                flashTimer?.Dispose();
                flashTimer = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
