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
    /// OutlookFlyoutPanel — content surface shown when an <see cref="OutlookBar"/> is collapsed and a rail icon is clicked.
    /// Renders a simple themed title bar + body; the body hosts either the panel's <see cref="OutlookPanel.ContentControl"/>
    /// or an inline item list. No pin / close chrome — the hosting form auto-dismisses on deactivate.
    /// </summary>
    [ToolboxItem(false)]
    public sealed class OutlookFlyoutPanel : IControl
    {
        readonly OutlookBar owner;
        OutlookPanel? panel;
        Control? hostedContent;

        Rectangle headerRect;
        Rectangle bodyRect;
        readonly List<Rectangle> itemRects = new List<Rectangle>();
        int hoverItemIndex = -1;

        /// <summary>Fires when the user activates an item in the inline list (no <see cref="OutlookPanel.ContentControl"/>).</summary>
        public event Action<OutlookItem>? ItemActivated;

        public OutlookFlyoutPanel(OutlookBar owner)
        {
            this.owner = owner;
            base.BackColor = Color.Transparent;
            SetStyle(ControlStyles.Selectable, false);
        }

        Font? f_title, f_item;
        void EnsureFonts()
        {
            if (f_title == null) f_title = new Font(Font.FontFamily, 9F, FontStyle.Bold);
            if (f_item == null) f_item = new Font(Font.FontFamily, 9F, FontStyle.Regular);
        }
        void DisposeFonts()
        {
            f_title?.Dispose(); f_title = null;
            f_item?.Dispose(); f_item = null;
        }
        protected override void OnFontChanged(EventArgs e) { DisposeFonts(); base.OnFontChanged(e); Invalidate(); }

        /// <summary>The panel currently displayed.</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OutlookPanel? Panel { get { return panel; } }

        /// <summary>Swap the displayed panel. Re-parents the new panel's <see cref="OutlookPanel.ContentControl"/> (if any); the previous control is released (not disposed).</summary>
        public void SetPanel(OutlookPanel newPanel)
        {
            if (panel == newPanel) return;
            ReleaseHostedContent();
            panel = newPanel;
            AdoptHostedContent();
            PerformLayout();
            Invalidate();
        }

        /// <summary>Release (but do not dispose) the hosted control. Call before disposing the flyout so the user's control survives.</summary>
        public void DetachHostedContent()
        {
            ReleaseHostedContent();
            panel = null;
        }

        void AdoptHostedContent()
        {
            var cc = panel?.ContentControl;
            if (cc == null) return;
            cc.Parent?.Controls.Remove(cc);
            cc.Dock = DockStyle.None;
            cc.Anchor = AnchorStyles.None;
            Controls.Add(cc);
            hostedContent = cc;
        }

        void ReleaseHostedContent()
        {
            if (hostedContent != null && Controls.Contains(hostedContent)) Controls.Remove(hostedContent);
            hostedContent = null;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            LayoutContentArea();
        }

        void LayoutContentArea()
        {
            float dpi = Dpi;
            int headerH = (int)(32 * dpi);
            headerRect = new Rectangle(0, 0, Width, headerH);
            bodyRect = new Rectangle(0, headerH, Width, Math.Max(0, Height - headerH));
            if (hostedContent != null)
            {
                // Inset so the body's gutter lines (drawn in OnDraw) are visible around the hosted control.
                var hostedInner = new Rectangle(bodyRect.X + 1, bodyRect.Y, bodyRect.Width - 2, bodyRect.Height - 1);
                if (hostedContent.Bounds != hostedInner) hostedContent.Bounds = hostedInner;
                if (!hostedContent.Visible) hostedContent.Visible = true;
            }
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            EnsureFonts();
            var scheme = owner.ColorScheme;
            string name = nameof(OutlookBar);
            var colBg = Colour.BgContainer.Get(name, scheme);
            var colHeaderBg = Colour.BgElevated.Get(name, scheme);
            var colText = Colour.Text.Get(name, scheme);
            var colTextSecondary = Colour.TextSecondary.Get(name, scheme);
            var colPrimary = Colour.Primary.Get(name, scheme);
            var colHover = Colour.HoverBg.Get(name, scheme);
            var colBorder = Colour.BorderColor.Get(name, scheme);
            var colDivider = Colour.BorderSecondary.Get(name, scheme);
            float dpi = Dpi;

            g.Fill(colBg, rect);
            g.Fill(colHeaderBg, headerRect);
            g.DrawLine(colDivider, 1 * dpi,
                new PointF(headerRect.X, headerRect.Bottom),
                new PointF(headerRect.Right, headerRect.Bottom));

            int titlePad = (int)(12 * dpi);
            var titleRect = new Rectangle(titlePad, 0, Math.Max(0, Width - titlePad * 2), headerRect.Height);
            g.String(panel?.Text ?? string.Empty, f_title!, colText, titleRect,
                FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis);

            if (hostedContent == null && panel != null)
            {
                DrawItems(g, bodyRect, dpi, colText, colTextSecondary, colHover, colPrimary);
            }

            // Body gutter — matches the expanded-bar body frame (3 sides, integer 1px to avoid DPI darkening).
            // Top edge abuts the header divider and is skipped to prevent a doubled line.
            g.DrawLine(colDivider, 1, new Point(bodyRect.X, bodyRect.Y), new Point(bodyRect.X, bodyRect.Bottom - 1));
            g.DrawLine(colDivider, 1, new Point(bodyRect.Right - 1, bodyRect.Y), new Point(bodyRect.Right - 1, bodyRect.Bottom - 1));
            g.DrawLine(colDivider, 1, new Point(bodyRect.X, bodyRect.Bottom - 1), new Point(bodyRect.Right - 1, bodyRect.Bottom - 1));

            g.Draw(colBorder, 1.5f * dpi, rect);
            base.OnDraw(e);
        }

        void DrawItems(Canvas g, Rectangle area, float dpi, Color colText, Color colTextSecondary, Color colHover, Color colPrimary)
        {
            itemRects.Clear();
            if (panel == null) return;

            int itemH = Math.Max((int)(owner.ItemHeight * dpi), (int)(28 * dpi));
            int pad = (int)(4 * dpi);
            int iconSize = (int)(16 * dpi);
            int y = area.Y + pad;

            for (int i = 0; i < panel.Items.Count; i++)
            {
                var item = panel.Items[i];
                var rect = new Rectangle(area.X + pad, y, area.Width - pad * 2, itemH);
                itemRects.Add(rect);

                if (i == hoverItemIndex && item.Enabled)
                {
                    using (var path = rect.RoundPath(3 * dpi)) g.Fill(colHover, path);
                }

                var iconRect = new Rectangle(rect.X + (int)(8 * dpi), rect.Y + (itemH - iconSize) / 2, iconSize, iconSize);
                var iconColor = item.Enabled ? colText : colTextSecondary;
                if (!string.IsNullOrEmpty(item.IconSvg))
                {
                    using (var bmp = SvgExtend.GetImgExtend(item.IconSvg!, iconRect, iconColor))
                    {
                        if (bmp != null) g.Image(bmp, iconRect);
                    }
                }
                else if (item.Image != null)
                {
                    g.Image(iconRect, item.Image, TFit.Contain);
                }

                var textRect = new Rectangle(iconRect.Right + (int)(8 * dpi), rect.Y, rect.Right - iconRect.Right - (int)(12 * dpi), itemH);
                g.String(item.Text ?? string.Empty, f_item!, iconColor, textRect,
                    FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis);

                y += itemH;
                if (y > area.Bottom) break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = -1;
            if (hostedContent == null && bodyRect.Contains(e.Location))
            {
                for (int i = 0; i < itemRects.Count; i++)
                {
                    if (itemRects[i].Contains(e.Location)) { newHover = i; break; }
                }
            }
            if (newHover != hoverItemIndex)
            {
                hoverItemIndex = newHover;
                SetCursor(hoverItemIndex >= 0);
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (hoverItemIndex != -1)
            {
                hoverItemIndex = -1;
                SetCursor(false);
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            if (hostedContent == null && panel != null && hoverItemIndex >= 0 && hoverItemIndex < panel.Items.Count)
            {
                var item = panel.Items[hoverItemIndex];
                if (item.Enabled)
                {
                    item.RaiseClick();
                    ItemActivated?.Invoke(item);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { ReleaseHostedContent(); DisposeFonts(); }
            base.Dispose(disposing);
        }
    }
}
