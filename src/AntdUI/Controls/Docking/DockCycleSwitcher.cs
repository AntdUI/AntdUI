// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Visual-Studio-style switcher popup for <see cref="DockPanel"/> — shown while the user holds
    /// <c>Ctrl+Tab</c> (or <c>Ctrl+F6</c>), advances selection per press, and activates the chosen
    /// content on Ctrl release. Borderless, non-focusable, themed via AntDUI tokens.
    /// </summary>
    internal sealed class DockCycleSwitcher : Form, IMessageFilter
    {
        readonly DockPanel panel;
        readonly List<IDockContent> items;
        int selectedIndex;
        Font? headerFont;

        // Win32 key constants (KeyEventArgs codes mirror VK_* on Windows).
        const int WM_KEYUP = 0x0101;
        const int WM_SYSKEYUP = 0x0105;
        const int VK_CONTROL = 0x11;

        // Foreground-loss dismiss: Form.Deactivate fires on Alt+Tab / task-switcher / click-into-
        // another-app via WinForms' normal event pipeline. WM_ACTIVATEAPP bypasses IMessageFilter
        // (SendMessage delivery, not the message queue), so the Deactivate event is the right hook.
        Form? ownerForm;

        DockCycleSwitcher(DockPanel panel, List<IDockContent> items, int initialIndex)
        {
            this.panel = panel;
            this.items = items;
            selectedIndex = initialIndex;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            BackColor = Style.Db.BgElevated;
        }

        /// <summary>Return true so the form never takes activation — the underlying content keeps focus.</summary>
        protected override bool ShowWithoutActivation => true;

        const int WS_EX_NOACTIVATE = 0x08000000;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        public static DockCycleSwitcher? Open(DockPanel panel, bool forward)
        {
            var list = new List<IDockContent>();
            for (int i = 0; i < panel.Panes.Count; i++) list.AddRange(panel.Panes[i].Contents);
            for (int i = 0; i < panel.FloatWindows.Count; i++) list.AddRange(panel.FloatWindows[i].Pane.Contents);
            if (list.Count < 2) return null;

            int initial = panel.ActiveContent != null ? list.IndexOf(panel.ActiveContent) : -1;
            if (initial < 0) initial = 0;
            int start = forward ? (initial + 1) % list.Count : (initial - 1 + list.Count) % list.Count;

            var sw = new DockCycleSwitcher(panel, list, start);
            sw.ComputeBounds(panel);
            Application.AddMessageFilter(sw);
            sw.Show();
            // Subscribe Deactivate AFTER Show — even with ShowWithoutActivation this avoids any
            // reentrant Close during activation plumbing, matching RibbonPopup's pattern.
            sw.ownerForm = panel.FindForm();
            if (sw.ownerForm != null) sw.ownerForm.Deactivate += sw.OnOwnerFormDeactivate;
            return sw;
        }

        void OnOwnerFormDeactivate(object? sender, EventArgs e)
        {
            // Foreground-loss → cancel the switcher without committing. Matches RibbonPopup's
            // Deactivate hook; centralised exit on app-switch.
            try { Close(); } catch { /* ignore */ }
        }

        void ComputeBounds(DockPanel owner)
        {
            float dpi = owner.IsHandleCreated ? owner.Dpi : 1f;
            int rowH = (int)(26 * dpi);
            int pad = (int)(10 * dpi);
            int headerH = (int)(24 * dpi);
            int w = (int)(320 * dpi);
            int h = headerH + pad + rowH * items.Count + pad;

            var screenCenter = owner.RectangleToScreen(owner.ClientRectangle);
            Bounds = new Rectangle(
                screenCenter.X + (screenCenter.Width - w) / 2,
                screenCenter.Y + (screenCenter.Height - h) / 2,
                w, h);
        }

        public void Advance(bool forward)
        {
            if (items.Count == 0) return;
            selectedIndex = forward
                ? (selectedIndex + 1) % items.Count
                : (selectedIndex - 1 + items.Count) % items.Count;
            Invalidate();
        }

        public void Commit()
        {
            if (selectedIndex >= 0 && selectedIndex < items.Count) panel.ActivateContent(items[selectedIndex]);
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.RemoveMessageFilter(this);
                if (ownerForm != null) { ownerForm.Deactivate -= OnOwnerFormDeactivate; ownerForm = null; }
                headerFont?.Dispose();
                headerFont = null;
            }
            base.Dispose(disposing);
        }

        bool IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if ((m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP) && ((int)m.WParam) == VK_CONTROL)
            {
                Commit();
                return true;
            }
            return false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float dpi = panel.IsHandleCreated ? panel.Dpi : 1f;
            int rowH = (int)(26 * dpi);
            int pad = (int)(10 * dpi);
            int headerH = (int)(24 * dpi);

            var fullRect = new Rectangle(0, 0, Width - 1, Height - 1);
            var border = Style.Db.BorderColor;
            var textColor = Style.Db.Text;
            var secondary = Style.Db.TextSecondary;
            var accentBg = Style.Db.PrimaryBg;
            var accent = Style.Db.Primary;

            using (var pen = new Pen(border, Math.Max(1f, dpi)))
            {
                g.DrawRectangle(pen, fullRect);
                g.DrawLine(pen, pad, headerH, Width - pad, headerH);
            }

            headerFont ??= new Font(Font.FontFamily, 9f, FontStyle.Bold);
            var headerRect = new Rectangle(pad, 0, Width - pad * 2, headerH);
            TextRenderer.DrawText(g, "Active Files", headerFont, headerRect, secondary,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            for (int i = 0; i < items.Count; i++)
            {
                var row = new Rectangle(pad / 2, headerH + pad + rowH * i, Width - pad, rowH);
                bool selected = i == selectedIndex;
                if (selected)
                {
                    using var b = new SolidBrush(accentBg);
                    g.FillRectangle(b, row);
                    using var accentBar = new SolidBrush(accent);
                    g.FillRectangle(accentBar, row.X, row.Y, (int)(2 * dpi), row.Height);
                }

                string title = items[i].DockTitle ?? string.Empty;
                var tRect = new Rectangle(row.X + (int)(10 * dpi), row.Y, row.Width - (int)(10 * dpi), row.Height);
                TextRenderer.DrawText(g, title, Font, tRect, selected ? accent : textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
            }
        }
    }
}
