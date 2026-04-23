// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// RibbonPopup — A borderless popup that displays a collapsed Ribbon tab's content as an overlay
    /// anchored below the ribbon's tab strip.
    /// </summary>
    internal class RibbonPopup : BorderlessForm, IMessageFilter
    {
        readonly Ribbon owner;
        readonly RibbonTabPage page;

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_MBUTTONDOWN = 0x0207;
        const int WM_NCLBUTTONDOWN = 0x00A1;
        const int WM_NCRBUTTONDOWN = 0x00A4;
        const int WM_NCMBUTTONDOWN = 0x00A7;

        // Tracked so Dispose can unsubscribe. The popup closes when its owning form deactivates —
        // that event fires on the exact foreground-loss transition we care about (Alt+Tab, task
        // switcher, clicking into another app) and is delivered through WinForms' normal event
        // path, unlike WM_ACTIVATEAPP which is SendMessage'd and bypasses IMessageFilter entirely.
        Form? ownerForm;

        /// <summary>Raised when a ribbon item is activated (clicked) inside the popup.</summary>
        public event Action<RibbonItem>? ItemActivated;

        public RibbonPopup(Ribbon owner, RibbonTabPage page)
        {
            this.owner = owner;
            this.page = page;

            ShowInTaskbar = false;
            TopMost = true;
            // Opt out of DWM window shadow — the popup is a drawer that extends the ribbon's tab row,
            // not a floating dialog; DWM's default shadow reads as a heavy "this is a form" affordance.
            UseDwm = false;
            Shadow = 1;
            StartPosition = FormStartPosition.Manual;
            BackColor = Colour.BgContainer.Get(nameof(Ribbon), owner.ColorScheme);

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            // Outside-click dismiss via message filter — does NOT fire for clicks on the owner ribbon,
            // so clicks on a different tab reach Ribbon.OnMouseDown with `popup` still non-null and
            // swap the popup in place (one click instead of two).
            Application.AddMessageFilter(this);
        }

        bool IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg != WM_LBUTTONDOWN && m.Msg != WM_RBUTTONDOWN && m.Msg != WM_MBUTTONDOWN
                && m.Msg != WM_NCLBUTTONDOWN && m.Msg != WM_NCRBUTTONDOWN && m.Msg != WM_NCMBUTTONDOWN) return false;
            if (IsDisposed || !IsHandleCreated) return false;

            // Clicks inside the popup pass through unchanged; clicks on the owner ribbon pass through
            // so Ribbon.OnMouseDown can swap the popup's tab in place (same-tab → close, different-tab → re-open).
            var ctl = Control.FromHandle(m.HWnd);
            if (IsInTree(ctl, this) || IsInTree(ctl, owner)) return false;

            try { Close(); } catch { /* swallow */ }
            return false;
        }

        void OnOwnerFormDeactivate(object? sender, EventArgs e)
        {
            // Foreground-loss dismiss. Subscribed in ShowBelow so we capture both Alt+Tab (app loses
            // foreground) and click-into-another-app paths in a single event handler.
            try { Close(); } catch { /* swallow */ }
        }

        static bool IsInTree(Control? c, Control root)
        {
            for (var p = c; p != null; p = p.Parent) if (p == root) return true;
            return false;
        }

        /// <summary>Show the popup anchored below the ribbon's tab strip, spanning the ribbon's full width
        /// so it visually extends the tab row as a drawer (matching Office's collapsed-ribbon behaviour).</summary>
        public void ShowBelow(Ribbon ribbon)
        {
            float dpi = ribbon.Dpi;
            int w = ribbon.ClientRectangle.Width;
            int h = (int)(ribbon.ContentHeight * dpi) + 4;

            int yOffset = (int)((ribbon.TabHeight + (ribbon.QuickAccessItems.Count > 0 ? ribbon.QuickAccessHeight : 0)) * dpi);
            var screenPt = ribbon.PointToScreen(new Point(0, yOffset));

            ownerForm = ribbon.FindForm();
            if (ownerForm != null) Show(ownerForm);
            else Show();
            // Re-apply bounds after Show() — BorderlessForm's shadow skin can snap the window to a
            // different size on activation; setting via SetBounds after Show() forces the final
            // geometry and guarantees the popup spans the ribbon's full width.
            SetBounds(screenPt.X, screenPt.Y, w, h);
            // Subscribe to Deactivate AFTER Show completes. If subscribed before, the activation
            // transfer during Show() itself fires Deactivate on ownerForm (focus moves to the popup),
            // which would re-enter Close() mid-CreateHandle and throw ObjectDisposedException.
            if (ownerForm != null) ownerForm.Deactivate += OnOwnerFormDeactivate;
        }

        #region Painting

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Suppressed: OnPaint renders the full surface via CanvasGDI.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            float dpi = owner.Dpi;
            // Match the main ribbon's rendering-quality setup so text/vectors read crisp at the same
            // scale rather than falling back to GDI+'s SystemDefault hint (which looks blurry on HiDPI).
            var canvas = e.Graphics.High(dpi);
            var scheme = owner.ColorScheme;

            canvas.Fill(Colour.BgContainer.Get(nameof(Ribbon), scheme), rect);
            canvas.Draw(Colour.BorderColor.Get(nameof(Ribbon), scheme), 1f, rect);

            var contentRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
            owner.DrawTabContent(canvas, page, contentRect, dpi);
        }

        #endregion

        #region Mouse — hit-test against item Rects populated during paint

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var pt = new PointF(e.X, e.Y);

            RibbonItem? newHover = null;
            foreach (var group in page.Groups)
            {
                if (!group.Visible) continue;
                foreach (var item in group.Items)
                {
                    if (item.Visible && item.Enabled && item.Rect.Contains(pt))
                    {
                        newHover = item;
                        break;
                    }
                }
                if (newHover != null) break;
            }

            bool newHoverCaret = newHover != null && newHover.IsSplit && newHover.CaretRect.Contains(pt);
            if (newHover != owner.hoverItem || newHoverCaret != owner.hoverCaret)
            {
                owner.hoverItem = newHover;
                owner.hoverCaret = newHoverCaret;
                Cursor = newHover != null ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (owner.hoverItem != null || owner.hoverCaret)
            {
                owner.hoverItem = null;
                owner.hoverCaret = false;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            if (owner.hoverItem != null && owner.hoverItem.Enabled)
            {
                owner.pressedItem = owner.hoverItem;
                owner.pressedCaret = owner.hoverCaret;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (owner.pressedItem == null) return;

            var pt = new PointF(e.X, e.Y);
            var pressed = owner.pressedItem;
            bool wasCaret = owner.pressedCaret;
            owner.pressedItem = null;
            owner.pressedCaret = false;
            Invalidate();

            bool inside = pressed.Rect.Contains(pt);
            if (inside && pressed.Enabled)
            {
                if (pressed.IsSplit && wasCaret && pressed.CaretRect.Contains(pt))
                {
                    pressed.RaiseDropDownOpening();
                    try { Close(); } catch { }
                }
                else
                {
                    // Notify owner which will raise the public event, toggle Checked, and close us.
                    ItemActivated?.Invoke(pressed);
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.RemoveMessageFilter(this);
                if (ownerForm != null) { ownerForm.Deactivate -= OnOwnerFormDeactivate; ownerForm = null; }
                owner.hoverItem = null;
                owner.pressedItem = null;
                owner.hoverCaret = false;
                owner.pressedCaret = false;
            }
            base.Dispose(disposing);
        }
    }
}
