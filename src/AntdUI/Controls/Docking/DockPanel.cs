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
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// DockPanel — Main docking container. Manages docked panes (<see cref="DockPane"/>),
    /// floating windows (<see cref="DockFloatWindow"/>), and auto-hide edge strips (<see cref="DockAutoHideStrip"/>).
    /// Reuses AntdUI infrastructure only (TabHeader, BorderlessForm, Splitter primitives, Canvas theming).
    ///
    /// Edge splitters are inline: each docked edge-pane (Left/Right/Top/Bottom) gets a draggable
    /// gutter between it and the adjacent region. Hit-testing and drag are handled in this control's
    /// mouse overrides — no separate Splitter child control is added.
    ///
    /// Close-cancellation: subscribe to <see cref="BeforeContentClosed"/> to veto a close
    /// (via tab ×, title-bar ×, or <see cref="RemoveContent"/>) before any state mutates.
    /// </summary>
    [Description("DockPanel 停靠面板")]
    [ToolboxItem(true)]
    [DefaultEvent("ActiveContentChanged")]
    public class DockPanel : IControl
    {
        public DockPanel()
        {
            base.BackColor = Color.Transparent;
            stripLeft = new DockAutoHideStrip(this, DockPosition.Left);
            stripRight = new DockAutoHideStrip(this, DockPosition.Right);
            stripTop = new DockAutoHideStrip(this, DockPosition.Top);
            stripBottom = new DockAutoHideStrip(this, DockPosition.Bottom);
            Controls.Add(stripLeft);
            Controls.Add(stripRight);
            Controls.Add(stripTop);
            Controls.Add(stripBottom);
            panesReadOnly = panes.AsReadOnly();
            floatsReadOnly = floats.AsReadOnly();
        }

        #region Properties

        Color? back;
        [Description("Background"), Category("Appearance"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get { return back; }
            set { if (back == value) return; back = value; Invalidate(); }
        }

        int autoHideStripSize = 22;
        [Description("Auto-hide strip thickness"), Category("Layout"), DefaultValue(22)]
        public int AutoHideStripSize
        {
            get { return autoHideStripSize; }
            set { if (autoHideStripSize == value) return; autoHideStripSize = value; PerformLayout(); Invalidate(); }
        }

        int splitterSize = 4;
        [Description("Splitter bar thickness"), Category("Layout"), DefaultValue(4)]
        public int SplitterSize
        {
            get { return splitterSize; }
            set { if (splitterSize == value) return; splitterSize = value; PerformLayout(); Invalidate(); }
        }

        bool locked;
        /// <summary>When true, all layout-changing gestures (drag-to-float, drag-to-autohide,
        /// splitter drag, pin/close/maximize icons) are disabled. Consumers set this to lock down
        /// a fixed IDE layout without removing the dock surface itself.</summary>
        [Description("Lock layout — disables drag, splitters, and pane chrome buttons"), Category("Behavior"), DefaultValue(false)]
        public bool Locked
        {
            get { return locked; }
            set { if (locked == value) return; locked = value; Invalidate(); OnPropertyChanged(nameof(Locked)); }
        }

        bool allowMaximize;
        /// <summary>Enables the per-pane maximize button and double-click-to-maximize gesture. Off by default;
        /// most docking hosts don't want the full-bleed single-pane mode. Turning this off also hides the icon
        /// so the title bar stays clean.</summary>
        [Description("Allow per-pane maximize (icon + double-click)"), Category("Behavior"), DefaultValue(false)]
        public bool AllowMaximize
        {
            get { return allowMaximize; }
            set
            {
                if (allowMaximize == value) return;
                allowMaximize = value;
                if (!value && maximizedPane != null)
                {
                    // Force-restore: bypass ToggleMaximize's Locked guard so disabling the feature
                    // while the panel is locked still ends the maximized state (otherwise the pane
                    // would stay full-bleed with no visible way to exit).
                    maximizedPane = null;
                    PerformLayout();
                    Invalidate();
                    MaximizedPaneChanged?.Invoke(this, EventArgs.Empty);
                }
                else foreach (var pane in panes) pane.Invalidate();
                OnPropertyChanged(nameof(AllowMaximize));
            }
        }

        #endregion

        #region State

        readonly List<DockPane> panes = new List<DockPane>();
        readonly List<DockFloatWindow> floats = new List<DockFloatWindow>();
        readonly DockAutoHideStrip stripLeft, stripRight, stripTop, stripBottom;

        /// <summary>Enumerates the four auto-hide strips in (edge, strip) pairs — used by <see cref="DockPersistor"/> and tests.</summary>
        internal IEnumerable<Tuple<DockPosition, DockAutoHideStrip>> Strips()
        {
            yield return Tuple.Create(DockPosition.Left, stripLeft);
            yield return Tuple.Create(DockPosition.Right, stripRight);
            yield return Tuple.Create(DockPosition.Top, stripTop);
            yield return Tuple.Create(DockPosition.Bottom, stripBottom);
        }
        readonly ReadOnlyCollection<DockPane> panesReadOnly;
        readonly ReadOnlyCollection<DockFloatWindow> floatsReadOnly;

        /// <summary>All active (non-floating) panes in this DockPanel.</summary>
        [Browsable(false)]
        public ReadOnlyCollection<DockPane> Panes { get { return panesReadOnly; } }

        /// <summary>All float windows spawned by this DockPanel.</summary>
        [Browsable(false)]
        public ReadOnlyCollection<DockFloatWindow> FloatWindows { get { return floatsReadOnly; } }

        IDockContent? activeContent;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDockContent? ActiveContent
        {
            get { return activeContent; }
            private set
            {
                if (activeContent == value) return;
                activeContent = value;
                Invalidate();
                if (ActiveContentChanged != null)
                {
                    var args = value != null ? new DockContentEventArgs(value) : EventArgs.Empty;
                    ActiveContentChanged.Invoke(this, args);
                }
            }
        }

        internal void NotifyActiveContent(IDockContent content) { ActiveContent = content; }

        #endregion

        #region Events

        [Description("Active content changed"), Category("Behavior")]
        public event EventHandler? ActiveContentChanged;

        [Description("Content closed"), Category("Behavior")]
        public event DockContentEventHandler? ContentClosed;

        [Description("Content docked (dragged/dropped)"), Category("Behavior")]
        public event DockContentEventHandler? ContentDocked;

        /// <summary>
        /// Raised before a content is removed via close button, <see cref="RemoveContent"/>, or tab ×.
        /// Set <c>e.Cancel = true</c> to abort. Fires BEFORE any state mutation — safe to
        /// prompt the user or reject the close. Not raised for programmatic <see cref="HideContent"/>
        /// or state-transition flows (float, auto-hide).
        /// </summary>
        [Description("Cancellable: raised before a content is closed"), Category("Behavior")]
        public event DockContentCancelEventHandler? BeforeContentClosed;

        internal bool RaiseBeforeContentClosed(IDockContent content)
        {
            if (BeforeContentClosed == null) return true;
            var args = new DockContentCancelEventArgs(content);
            BeforeContentClosed.Invoke(this, args);
            return !args.Cancel;
        }

        #endregion

        #region Content operations

        /// <summary>Add a content at the specified position. Reuses an existing pane on that edge unless <paramref name="newPane"/> is true.</summary>
        public DockPane AddContent(IDockContent content, DockPosition position, float sizeRatio = 0.25f, bool newPane = false)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            DockPane? pane = newPane ? null : FindPaneFor(position);
            if (pane == null)
            {
                pane = new DockPane(this);
                pane.Position = position;
                pane.SizeRatio = sizeRatio;
                pane.State = DockState.Docked;
                panes.Add(pane);
                if (!Controls.Contains(pane)) Controls.Add(pane);
            }
            pane.AddContent(content);
            content.DockState = DockState.Docked;
            if (activeContent == null) ActiveContent = content;
            PerformLayout();
            Invalidate();
            if (ContentDocked != null) ContentDocked.Invoke(this, new DockContentEventArgs(content));
            return pane;
        }

        /// <summary>Remove a content entirely (raises <see cref="BeforeContentClosed"/> then <see cref="ContentClosed"/>).</summary>
        public void RemoveContent(IDockContent content)
        {
            if (content == null) return;
            if (!RaiseBeforeContentClosed(content)) return;
            RemoveContentCore(content);
        }

        /// <summary>Internal close path without re-raising BeforeContentClosed (callers that already gated the event).</summary>
        internal void RemoveContentCore(IDockContent content)
        {
            if (!TryExtractFromFloat(content))
            {
                var pane = content.OwnerPane;
                if (pane != null)
                {
                    pane.RemoveContent(content, false);
                    if (pane.Contents.Count == 0 && panes.Contains(pane)) DestroyPane(pane);
                }
            }
            if (activeContent == content) ActiveContent = ChooseFallbackActive();
            PerformLayout();
            Invalidate();
            if (ContentClosed != null) ContentClosed.Invoke(this, new DockContentEventArgs(content));
        }

        DockFloatWindow? FindFloatOwner(IDockContent content)
        {
            for (int i = 0; i < floats.Count; i++) if (floats[i].Contains(content)) return floats[i];
            return null;
        }

        DockCycleSwitcher? cycleSwitcher;
        bool cycleSwitcherEnabled = true;

        /// <summary>When true (default), <c>Ctrl+Tab</c> / <c>Ctrl+F6</c> shows a VS-style switcher popup
        /// that advances per press and commits on Ctrl release. Setting to false falls back to plain
        /// next-content cycling with no visual overlay.</summary>
        [Description("Ctrl+Tab shows VS-style switcher popup"), Category("Behavior"), DefaultValue(true)]
        public bool CycleSwitcherEnabled
        {
            get { return cycleSwitcherEnabled; }
            set { cycleSwitcherEnabled = value; }
        }

        /// <summary>Ctrl+F6 / Ctrl+Tab cycles through open contents (all docked panes + all float windows).
        /// Pressed anywhere inside the DockPanel (or one of its hosted controls) this routes focus
        /// to the next content's pane and makes it active.</summary>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            bool isCtrlF6 = keyData == (Keys.Control | Keys.F6) || keyData == (Keys.Control | Keys.Shift | Keys.F6);
            bool isCtrlTab = keyData == (Keys.Control | Keys.Tab) || keyData == (Keys.Control | Keys.Shift | Keys.Tab);
            if (isCtrlF6 || isCtrlTab)
            {
                bool forward = (keyData & Keys.Shift) != Keys.Shift;
                if (cycleSwitcherEnabled)
                {
                    if (cycleSwitcher != null && !cycleSwitcher.IsDisposed)
                    {
                        cycleSwitcher.Advance(forward);
                        return true;
                    }
                    var sw = DockCycleSwitcher.Open(this, forward);
                    if (sw != null)
                    {
                        cycleSwitcher = sw;
                        sw.FormClosed += (s, e) => { if (cycleSwitcher == sw) cycleSwitcher = null; };
                        return true;
                    }
                }
                if (CycleActiveContent(forward)) return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        bool CycleActiveContent(bool forward)
        {
            // Build a flat list of all visible contents in visual order: docked first, then floats.
            var all = new List<IDockContent>();
            for (int i = 0; i < panes.Count; i++) all.AddRange(panes[i].Contents);
            for (int i = 0; i < floats.Count; i++) all.AddRange(floats[i].Pane.Contents);
            if (all.Count == 0) return false;

            int idx = activeContent != null ? all.IndexOf(activeContent) : -1;
            if (forward) idx = (idx + 1) % all.Count;
            else idx = (idx - 1 + all.Count) % all.Count;
            ActivateContent(all[idx]);
            return true;
        }

        /// <summary>If the content lives in a float window, remove it there and close the window if now empty.
        /// Returns true if the content was handled through the float path. Centralizes the "don't leave an
        /// empty-pane float window stranded" invariant.</summary>
        bool TryExtractFromFloat(IDockContent content)
        {
            var floatOwner = FindFloatOwner(content);
            if (floatOwner == null) return false;
            floatOwner.RemoveContent(content);
            if (floatOwner.IsEmpty)
            {
                floats.Remove(floatOwner);
                try { floatOwner.Close(); } catch { /* ignore */ }
            }
            return true;
        }

        /// <summary>Make a content visible and focused (switching tabs if needed).</summary>
        public void ActivateContent(IDockContent content)
        {
            if (content == null) return;
            var pane = content.OwnerPane;
            if (pane != null)
            {
                int idx = -1;
                for (int i = 0; i < pane.Contents.Count; i++) if (pane.Contents[i] == content) { idx = i; break; }
                if (idx >= 0) pane.SelectedIndex = idx;
                ActiveContent = content;
                pane.BringToFront();
            }
            else
            {
                // Floating — bring the float window forward
                for (int i = 0; i < floats.Count; i++)
                {
                    if (floats[i].Contains(content)) { floats[i].Activate(); ActiveContent = content; break; }
                }
            }
        }

        /// <summary>Show content if hidden (re-adds to the last pane or a new one on <paramref name="fallbackPosition"/>).</summary>
        public void ShowContent(IDockContent content, DockPosition fallbackPosition = DockPosition.Right)
        {
            if (content == null) return;
            if (content.DockState != DockState.Hidden && content.OwnerPane != null) { ActivateContent(content); return; }
            content.DockState = DockState.Docked;
            AddContent(content, fallbackPosition);
        }

        /// <summary>Hide content (keeps it in memory but detaches from UI).</summary>
        public void HideContent(IDockContent content)
        {
            if (content == null) return;
            var pane = content.OwnerPane;
            if (pane != null)
            {
                pane.RemoveContent(content, false);
                if (pane.Contents.Count == 0) DestroyPane(pane);
            }
            content.DockState = DockState.Hidden;
            if (activeContent == content) ActiveContent = ChooseFallbackActive();
            PerformLayout();
            Invalidate();
        }

        /// <summary>Float a docked content into its own <see cref="DockFloatWindow"/>.</summary>
        public DockFloatWindow FloatContent(IDockContent content, Rectangle? bounds = null)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            var pane = content.OwnerPane;
            if (pane != null)
            {
                pane.RemoveContent(content, false);
                if (pane.Contents.Count == 0) DestroyPane(pane);
            }
            var fw = new DockFloatWindow(this);
            fw.AddContent(content);
            floats.Add(fw);
            if (bounds.HasValue) fw.Bounds = bounds.Value;
            else
            {
                var screenPt = Cursor.Position;
                fw.StartPosition = FormStartPosition.Manual;
                fw.Size = new Size((int)(480 * Dpi), (int)(320 * Dpi));
                fw.Location = new Point(screenPt.X - fw.Width / 2, screenPt.Y - (int)(12 * Dpi));
            }
            content.DockState = DockState.Float;
            var topForm = FindForm();
            if (topForm != null) fw.Show(topForm); else fw.Show();
            ActiveContent = content;
            PerformLayout();
            Invalidate();
            return fw;
        }

        /// <summary>Re-dock a floating or auto-hidden content at the given position.</summary>
        public DockPane DockContent(IDockContent content, DockPosition position = DockPosition.Right)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            // Detach from strips and the current pane before adding, so pinning from an auto-hide flyout leaves no stale references.
            stripLeft.RemoveContent(content);
            stripRight.RemoveContent(content);
            stripTop.RemoveContent(content);
            stripBottom.RemoveContent(content);
            var old = content.OwnerPane;
            if (old != null)
            {
                old.RemoveContent(content, dispose: false);
                if (old.Contents.Count == 0 && panes.Contains(old)) DestroyPane(old);
            }
            return AddContent(content, position);
        }

        /// <summary>Collapse the content into an auto-hide strip on its current (or fallback) edge.</summary>
        public void AutoHideContent(IDockContent content, DockPosition? edge = null)
        {
            if (content == null || !content.CanAutoHide) return;
            var pane = content.OwnerPane;
            DockPosition where = edge.HasValue ? edge.Value : (pane != null ? pane.Position : DockPosition.Left);
            if (where == DockPosition.Fill || where == DockPosition.None) where = DockPosition.Left;

            if (!TryExtractFromFloat(content) && pane != null)
            {
                pane.RemoveContent(content, false);
                if (pane.Contents.Count == 0 && panes.Contains(pane)) DestroyPane(pane);
            }
            content.DockState = DockState.AutoHide;
            StripFor(where).AddContent(content);
            if (activeContent == content) ActiveContent = ChooseFallbackActive();
            PerformLayout();
            Invalidate();
        }

        internal DockAutoHideStrip StripFor(DockPosition pos)
        {
            switch (pos)
            {
                case DockPosition.Left: return stripLeft;
                case DockPosition.Right: return stripRight;
                case DockPosition.Top: return stripTop;
                case DockPosition.Bottom: return stripBottom;
            }
            return stripLeft;
        }

        DockPane? maximizedPane;
        /// <summary>Currently maximized pane (or null). A maximized pane fills the full client area;
        /// sibling panes are temporarily hidden. Toggle via <see cref="ToggleMaximize"/>.</summary>
        [Browsable(false)]
        public DockPane? MaximizedPane => maximizedPane;

        /// <summary>Raised after <see cref="MaximizedPane"/> changes (null → pane, pane → null, pane → other pane).</summary>
        [Description("Maximized pane changed"), Category("Behavior")]
        public event EventHandler? MaximizedPaneChanged;

        /// <summary>Pulse the title bar of the pane hosting <paramref name="content"/> to draw the user's attention
        /// (e.g. "a new log line arrived in the Output pane"). No-op if the content is already focused.</summary>
        public void FlashContent(IDockContent content, int durationMs = 3000)
        {
            if (content == null) return;
            content.OwnerPane?.Flash(durationMs);
        }

        /// <summary>Toggle the maximized state of <paramref name="pane"/>. Passing null or the currently-maximized pane restores.</summary>
        public void ToggleMaximize(DockPane? pane)
        {
            if (locked) return;
            var target = (pane != null && pane == maximizedPane) ? null : pane;
            if (maximizedPane == target) return;
            maximizedPane = target;
            PerformLayout();
            Invalidate();
            MaximizedPaneChanged?.Invoke(this, EventArgs.Empty);
        }

        void DestroyPane(DockPane pane)
        {
            if (Controls.Contains(pane)) Controls.Remove(pane);
            panes.Remove(pane);
            pane.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Close the Ctrl+Tab switcher if the panel is torn down mid-cycle — otherwise the
                // borderless Form + its Application.MessageFilter would outlive the dock panel.
                try { cycleSwitcher?.Close(); } catch { /* ignore */ }
                cycleSwitcher = null;
            }
            base.Dispose(disposing);
        }

        DockPane? FindPaneFor(DockPosition position)
        {
            for (int i = 0; i < panes.Count; i++)
            {
                if (panes[i].Position == position && panes[i].State == DockState.Docked) return panes[i];
            }
            return null;
        }

        IDockContent? ChooseFallbackActive()
        {
            for (int i = 0; i < panes.Count; i++)
            {
                if (panes[i].ActiveContent != null) return panes[i].ActiveContent;
            }
            for (int i = 0; i < floats.Count; i++)
            {
                if (floats[i].ActiveContent != null) return floats[i].ActiveContent;
            }
            return null;
        }

        #endregion

        #region Drag-to-float helper (called by DockPane)

        internal void BeginDragFloat(IDockContent content, Point screenPoint)
        {
            // Start a float window at the cursor and let the BorderlessForm drag takeover
            var origin = new Rectangle(screenPoint.X - (int)(120 * Dpi), screenPoint.Y - (int)(8 * Dpi),
                (int)(480 * Dpi), (int)(320 * Dpi));
            var fw = FloatContent(content, origin);
            try
            {
                // Hand off to the form for immediate drag
                fw.BeginSystemDrag(true);
            }
            catch { }
        }

        internal DockPane DropFromFloat(IDockContent content, DockZone zone, DockPane? target)
        {
            // Inner zones split or tabify the hit pane.
            if (target != null && panes.Contains(target))
            {
                switch (zone)
                {
                    case DockZone.Center:
                        target.AddContent(content);
                        content.DockState = DockState.Docked;
                        ActiveContent = content;
                        PerformLayout();
                        Invalidate();
                        if (ContentDocked != null) ContentDocked.Invoke(this, new DockContentEventArgs(content));
                        return target;
                    case DockZone.InnerLeft: return SplitPane(target, DockPosition.Left, content);
                    case DockZone.InnerRight: return SplitPane(target, DockPosition.Right, content);
                    case DockZone.InnerTop: return SplitPane(target, DockPosition.Top, content);
                    case DockZone.InnerBottom: return SplitPane(target, DockPosition.Bottom, content);
                }
            }

            DockPosition pos;
            switch (zone)
            {
                case DockZone.OuterLeft: pos = DockPosition.Left; break;
                case DockZone.OuterRight: pos = DockPosition.Right; break;
                case DockZone.OuterTop: pos = DockPosition.Top; break;
                case DockZone.OuterBottom: pos = DockPosition.Bottom; break;
                case DockZone.Center:
                    if (target != null)
                    {
                        target.AddContent(content);
                        content.DockState = DockState.Docked;
                        ActiveContent = content;
                        PerformLayout();
                        Invalidate();
                        return target;
                    }
                    pos = DockPosition.Fill;
                    break;
                default: pos = DockPosition.Right; break;
            }
            return AddContent(content, pos);
        }

        /// <summary>
        /// Insert a new pane adjacent to <paramref name="target"/> on <paramref name="side"/>,
        /// splitting the target's current space 50/50. Simplified split model: since the
        /// existing layout is edge-based (not a full tree), inner-L/R/T/B splits are implemented
        /// by creating a sibling pane with the same edge <see cref="DockPane.Position"/> as the target
        /// and halving the target's <see cref="DockPane.SizeRatio"/>. For a Fill-pane target, the new
        /// pane takes the requested edge against the Fill region. This is a practical approximation
        /// of a full docking tree; a full group-tree model is deferred.
        /// </summary>
        internal DockPane SplitPane(DockPane target, DockPosition side, IDockContent content)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            DockPosition newPos;
            float newRatio;

            if (target.Position == DockPosition.Fill || target.Position == DockPosition.None)
            {
                // Splitting the fill region: the new pane grabs the requested edge.
                newPos = side;
                newRatio = 0.5f;
            }
            else
            {
                // Splitting an edge-docked pane. Keep the new pane on the same edge and halve ratios.
                newPos = target.Position;
                newRatio = Math.Max(0.1f, target.SizeRatio * 0.5f);
                target.SizeRatio = newRatio;
            }

            var pane = new DockPane(this);
            pane.Position = newPos;
            pane.SizeRatio = newRatio;
            pane.State = DockState.Docked;
            // Insert the new pane immediately before or after the target to influence paint order.
            int idx = panes.IndexOf(target);
            bool insertAfter = side == DockPosition.Right || side == DockPosition.Bottom;
            if (idx < 0) panes.Add(pane);
            else panes.Insert(insertAfter ? idx + 1 : idx, pane);
            if (!Controls.Contains(pane)) Controls.Add(pane);

            pane.AddContent(content);
            content.DockState = DockState.Docked;
            ActiveContent = content;
            PerformLayout();
            Invalidate();
            if (ContentDocked != null) ContentDocked.Invoke(this, new DockContentEventArgs(content));
            return pane;
        }

        internal DockPane? HitTestPane(Point screenPoint)
        {
            var client = PointToClient(screenPoint);
            for (int i = 0; i < panes.Count; i++)
            {
                if (panes[i].Bounds.Contains(client)) return panes[i];
            }
            return null;
        }

        #endregion

        #region Layout + edge splitters

        // Each edge-splitter's client-space rectangle, captured during layout.
        // null = no splitter on that edge (no pane there).
        Rectangle? splitLeftRect, splitRightRect, splitTopRect, splitBottomRect;
        DockPane? paneLeft, paneRight, paneTop, paneBottom;

        // Drag state
        enum SplitEdge { None, Left, Right, Top, Bottom }
        SplitEdge dragEdge = SplitEdge.None;
        SplitEdge hoverEdge = SplitEdge.None;
        Point dragStartPt;
        float dragStartRatio;
        Rectangle dragAvailAtStart;

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            PerformLayoutPanes();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            PerformLayoutPanes();
        }

        int batchDepth;
        /// <summary>Suspend re-layout while adding multiple contents in a batch.</summary>
        public void BeginBatch()
        {
            if (batchDepth == 0) SuspendLayout();
            batchDepth++;
        }
        /// <summary>Resume layout and flush a single pass.</summary>
        public void EndBatch()
        {
            if (batchDepth == 0) return;
            batchDepth--;
            if (batchDepth == 0)
            {
                ResumeLayout(false);
                PerformLayoutPanes();
                Invalidate();
            }
        }

        void PerformLayoutPanes()
        {
            if (batchDepth > 0) return;
            var r = ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;
            float dpi = Dpi;
            int strip = (int)(autoHideStripSize * dpi);
            int split = Math.Max(1, (int)(splitterSize * dpi));

            splitLeftRect = splitRightRect = splitTopRect = splitBottomRect = null;
            paneLeft = paneRight = paneTop = paneBottom = null;

            // Maximized short-circuit: a single pane fills the whole client area; siblings hidden.
            if (maximizedPane != null && panes.Contains(maximizedPane))
            {
                for (int i = 0; i < panes.Count; i++)
                {
                    var p = panes[i];
                    if (p == maximizedPane) { p.Visible = true; p.SetBounds(r.X, r.Y, r.Width, r.Height); }
                    else p.Visible = false;
                }
                return;
            }
            // Restore visibility if we were previously maximized.
            for (int i = 0; i < panes.Count; i++) if (!panes[i].Visible) panes[i].Visible = true;

            // Auto-hide strip occupies outer edges of ClientRectangle.
            // When a strip is visible we subtract `strip + stripGap` from `avail` so the pane doesn't
            // paint over the strip's inner-edge separator line (which would otherwise be drawn at the same
            // pixel as the pane's outer border and disappear visually).
            int stripGap = Math.Max(2, (int)(2 * dpi));
            var avail = r;
            if (stripLeft.HasContent)
            {
                stripLeft.SetBounds(avail.X, avail.Y, strip, avail.Height);
                avail = new Rectangle(avail.X + strip + stripGap, avail.Y, avail.Width - strip - stripGap, avail.Height);
            }
            else stripLeft.SetBounds(0, 0, 0, 0);
            if (stripRight.HasContent)
            {
                stripRight.SetBounds(avail.Right - strip, avail.Y, strip, avail.Height);
                avail = new Rectangle(avail.X, avail.Y, avail.Width - strip - stripGap, avail.Height);
            }
            else stripRight.SetBounds(0, 0, 0, 0);
            if (stripTop.HasContent)
            {
                stripTop.SetBounds(avail.X, avail.Y, avail.Width, strip);
                avail = new Rectangle(avail.X, avail.Y + strip + stripGap, avail.Width, avail.Height - strip - stripGap);
            }
            else stripTop.SetBounds(0, 0, 0, 0);
            if (stripBottom.HasContent)
            {
                stripBottom.SetBounds(avail.X, avail.Bottom - strip, avail.Width, strip);
                avail = new Rectangle(avail.X, avail.Y, avail.Width, avail.Height - strip - stripGap);
            }
            else stripBottom.SetBounds(0, 0, 0, 0);

            // The *first* pane on each edge gets a splitter on its inner boundary; later panes on the same edge stack without a drag handle.
            for (int i = 0; i < panes.Count; i++)
            {
                var p = panes[i];
                if (p.State != DockState.Docked) continue;
                int size;
                switch (p.Position)
                {
                    case DockPosition.Left:
                        size = Math.Max(40, (int)(avail.Width * p.SizeRatio));
                        p.SetBounds(avail.X, avail.Y, size, avail.Height);
                        if (paneLeft == null)
                        {
                            paneLeft = p;
                            splitLeftRect = new Rectangle(avail.X + size, avail.Y, split, avail.Height);
                        }
                        avail = new Rectangle(avail.X + size + split, avail.Y, avail.Width - size - split, avail.Height);
                        break;
                    case DockPosition.Right:
                        size = Math.Max(40, (int)(avail.Width * p.SizeRatio));
                        p.SetBounds(avail.Right - size, avail.Y, size, avail.Height);
                        if (paneRight == null)
                        {
                            paneRight = p;
                            splitRightRect = new Rectangle(avail.Right - size - split, avail.Y, split, avail.Height);
                        }
                        avail = new Rectangle(avail.X, avail.Y, avail.Width - size - split, avail.Height);
                        break;
                    case DockPosition.Top:
                        size = Math.Max(40, (int)(avail.Height * p.SizeRatio));
                        p.SetBounds(avail.X, avail.Y, avail.Width, size);
                        if (paneTop == null)
                        {
                            paneTop = p;
                            splitTopRect = new Rectangle(avail.X, avail.Y + size, avail.Width, split);
                        }
                        avail = new Rectangle(avail.X, avail.Y + size + split, avail.Width, avail.Height - size - split);
                        break;
                    case DockPosition.Bottom:
                        size = Math.Max(40, (int)(avail.Height * p.SizeRatio));
                        p.SetBounds(avail.X, avail.Bottom - size, avail.Width, size);
                        if (paneBottom == null)
                        {
                            paneBottom = p;
                            splitBottomRect = new Rectangle(avail.X, avail.Bottom - size - split, avail.Width, split);
                        }
                        avail = new Rectangle(avail.X, avail.Y, avail.Width, avail.Height - size - split);
                        break;
                }
            }

            for (int i = 0; i < panes.Count; i++)
            {
                var p = panes[i];
                if (p.State == DockState.Docked && p.Position == DockPosition.Fill)
                {
                    p.SetBounds(avail.X, avail.Y, avail.Width, avail.Height);
                }
            }
        }

        #endregion

        #region Splitter hit-test + drag

        SplitEdge HitTestSplitter(Point client)
        {
            if (splitLeftRect.HasValue && splitLeftRect.Value.Contains(client)) return SplitEdge.Left;
            if (splitRightRect.HasValue && splitRightRect.Value.Contains(client)) return SplitEdge.Right;
            if (splitTopRect.HasValue && splitTopRect.Value.Contains(client)) return SplitEdge.Top;
            if (splitBottomRect.HasValue && splitBottomRect.Value.Contains(client)) return SplitEdge.Bottom;
            return SplitEdge.None;
        }

        DockPane? PaneForEdge(SplitEdge e)
        {
            switch (e)
            {
                case SplitEdge.Left: return paneLeft;
                case SplitEdge.Right: return paneRight;
                case SplitEdge.Top: return paneTop;
                case SplitEdge.Bottom: return paneBottom;
            }
            return null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (locked || maximizedPane != null)
            {
                if (hoverEdge != SplitEdge.None) { hoverEdge = SplitEdge.None; Cursor = Cursors.Default; }
                return;
            }
            if (dragEdge != SplitEdge.None)
            {
                ProcessSplitterDrag(e.Location);
                return;
            }
            var edge = HitTestSplitter(e.Location);
            if (edge != hoverEdge)
            {
                hoverEdge = edge;
                switch (edge)
                {
                    case SplitEdge.Left:
                    case SplitEdge.Right:
                        Cursor = Cursors.SizeWE;
                        break;
                    case SplitEdge.Top:
                    case SplitEdge.Bottom:
                        Cursor = Cursors.SizeNS;
                        break;
                    default:
                        Cursor = Cursors.Default;
                        break;
                }
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            if (locked || maximizedPane != null) return;
            var edge = HitTestSplitter(e.Location);
            if (edge == SplitEdge.None) return;
            var pane = PaneForEdge(edge);
            if (pane == null) return;
            dragEdge = edge;
            dragStartPt = e.Location;
            dragStartRatio = pane.SizeRatio;
            dragAvailAtStart = ComputeAvailAxis(edge);
            Capture = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (dragEdge != SplitEdge.None)
            {
                dragEdge = SplitEdge.None;
                Capture = false;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (dragEdge == SplitEdge.None && hoverEdge != SplitEdge.None)
            {
                hoverEdge = SplitEdge.None;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        Rectangle ComputeAvailAxis(SplitEdge edge)
        {
            // Snapshot of the axis region at drag start: for Left/Right, the width available when
            // the edge-pane was placed; for Top/Bottom, the height. We reconstruct from ClientRectangle
            // minus any preceding auto-hide strip.
            var r = ClientRectangle;
            float dpi = Dpi;
            int strip = (int)(autoHideStripSize * dpi);
            var avail = r;
            if (stripLeft.HasContent) avail = new Rectangle(avail.X + strip, avail.Y, avail.Width - strip, avail.Height);
            if (stripRight.HasContent) avail = new Rectangle(avail.X, avail.Y, avail.Width - strip, avail.Height);
            if (stripTop.HasContent) avail = new Rectangle(avail.X, avail.Y + strip, avail.Width, avail.Height - strip);
            if (stripBottom.HasContent) avail = new Rectangle(avail.X, avail.Y, avail.Width, avail.Height - strip);
            return avail;
        }

        void ProcessSplitterDrag(Point location)
        {
            var pane = PaneForEdge(dragEdge);
            if (pane == null) { dragEdge = SplitEdge.None; return; }
            int minPx = (int)(48 * Dpi);
            int dx = location.X - dragStartPt.X;
            int dy = location.Y - dragStartPt.Y;
            float newRatio = dragStartRatio;
            switch (dragEdge)
            {
                case SplitEdge.Left:
                    {
                        int width = Math.Max(1, dragAvailAtStart.Width);
                        int origSize = (int)(dragStartRatio * width);
                        int size = origSize + dx;
                        int maxSize = width - minPx;
                        if (size < minPx) size = minPx;
                        if (size > maxSize) size = maxSize;
                        newRatio = (float)size / width;
                        break;
                    }
                case SplitEdge.Right:
                    {
                        int width = Math.Max(1, dragAvailAtStart.Width);
                        int origSize = (int)(dragStartRatio * width);
                        int size = origSize - dx;
                        int maxSize = width - minPx;
                        if (size < minPx) size = minPx;
                        if (size > maxSize) size = maxSize;
                        newRatio = (float)size / width;
                        break;
                    }
                case SplitEdge.Top:
                    {
                        int height = Math.Max(1, dragAvailAtStart.Height);
                        int origSize = (int)(dragStartRatio * height);
                        int size = origSize + dy;
                        int maxSize = height - minPx;
                        if (size < minPx) size = minPx;
                        if (size > maxSize) size = maxSize;
                        newRatio = (float)size / height;
                        break;
                    }
                case SplitEdge.Bottom:
                    {
                        int height = Math.Max(1, dragAvailAtStart.Height);
                        int origSize = (int)(dragStartRatio * height);
                        int size = origSize - dy;
                        int maxSize = height - minPx;
                        if (size < minPx) size = minPx;
                        if (size > maxSize) size = maxSize;
                        newRatio = (float)size / height;
                        break;
                    }
            }
            if (Math.Abs(newRatio - pane.SizeRatio) < 0.001f) return;
            BeginBatch();
            try { pane.SizeRatio = newRatio; }
            finally { EndBatch(); }
        }

        #endregion

        #region Rendering

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;
            var scheme = ColorScheme;
            string name = nameof(DockPanel);
            g.Fill(back ?? Colour.BgLayout.Get(name, scheme), rect);

            var colSplit = Colour.FillSecondary.Get(name, scheme);
            var colSplitHover = Colour.PrimaryBg.Get(name, scheme);
            PaintSplitter(g, splitLeftRect, SplitEdge.Left, colSplit, colSplitHover);
            PaintSplitter(g, splitRightRect, SplitEdge.Right, colSplit, colSplitHover);
            PaintSplitter(g, splitTopRect, SplitEdge.Top, colSplit, colSplitHover);
            PaintSplitter(g, splitBottomRect, SplitEdge.Bottom, colSplit, colSplitHover);

            base.OnDraw(e);
        }

        void PaintSplitter(Canvas g, Rectangle? r, SplitEdge edge, Color idle, Color hot)
        {
            if (!r.HasValue) return;
            bool active = (hoverEdge == edge) || (dragEdge == edge);
            g.Fill(active ? hot : idle, r.Value);
        }

        #endregion

        #region Persistence façade

        /// <summary>Serialize layout to XML. See <see cref="DockPersistor"/>.</summary>
        public string GetLayoutXml() { return DockPersistor.Save(this); }

        /// <summary>Restore layout from XML using a content resolver. See <see cref="DockPersistor"/>.</summary>
        public void LoadLayoutXml(string xml, Func<string, IDockContent?> resolver)
        {
            DockPersistor.Load(this, xml, resolver);
        }

        #endregion
    }
}
