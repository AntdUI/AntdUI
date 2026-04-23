// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// DockNavigator — compass overlay shown during drag-to-dock.
    /// Draws TWO compasses:
    ///  * an outer 4-zone compass (Left/Right/Top/Bottom) fixed against the <see cref="DockPanel"/>'s edges;
    ///  * an inner 5-zone compass (Left/Right/Top/Bottom + Center) centred on the <see cref="DockPane"/> under the cursor.
    /// Hovered zones paint a semi-transparent blue preview rectangle showing the predicted drop bounds.
    /// Theme-aware via the Canvas colour tokens. Layered, click-through top-level form.
    /// </summary>
    [ToolboxItem(false)]
    internal class DockNavigator : IDisposable
    {
        NavForm? form;

        public DockZone? HitZone => form?.HitZone;
        public DockPane? HitPane => form?.HitPane;

        public void Show(DockPanel panel)
        {
            form = new NavForm(panel);
            form.Show();
            form.UpdateHit(Cursor.Position); // Forces hit evaluation and initial Print
        }

        public void Update(Point screenPoint) => form?.UpdateHit(screenPoint);

        public void Dispose()
        {
            form?.Dispose();
            form = null;
        }

        sealed class NavForm : ILayeredForm
        {
            readonly DockPanel dockpanel;
            public DockZone HitZone;
            public DockPane? HitPane;

            // Outer compass rects (panel-edge anchored).
            Rectangle outerL, outerR, outerT, outerB;
            // Inner compass rects (centred on HitPane). Valid only when HitPane != null.
            Rectangle innerL, innerR, innerT, innerB, innerC;
            bool hasInner;

            // Cached paint objects — re-coloured per zone rather than allocated per draw call.
            // PrintBit + 9 DrawZone invocations each used to allocate 3 GDI+ objects; the compass
            // repaints on every WM_MOVING during a drag, so the savings add up.

            public NavForm(DockPanel panel)
            {
                dockpanel = panel;
                var origin = panel.PointToScreen(Point.Empty);
                SetLocation(origin.X, origin.Y);
                SetSize(panel.Size.Width, panel.Size.Height);
                alpha = 255;
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= (int)Win32.User32.WindowStylesEx.WS_EX_TRANSPARENT;
                    return cp;
                }
            }

            protected override void WndProc(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == (int)Win32.User32.WindowMessage.WM_NCHITTEST)
                {
                    m.Result = (IntPtr)Win32.User32.HitTestValues.HTTRANSPARENT;
                    return;
                }
                base.WndProc(ref m);
            }

            public void UpdateHit(Point screenPoint)
            {
                // Convert screen coord to local client coord
                var origin = new Point(TargetRect.X, TargetRect.Y);
                var client = new Point(screenPoint.X - origin.X, screenPoint.Y - origin.Y);

                DockZone oldZone = HitZone;
                DockPane? oldPane = HitPane;
                HitPane = dockpanel.HitTestPane(screenPoint);
                LayoutZones();

                // Priority: inner compass (when present) beats outer.
                HitZone = DockZone.None;
                if (hasInner)
                {
                    if (innerC.Contains(client)) HitZone = DockZone.Center;
                    else if (innerL.Contains(client)) HitZone = DockZone.InnerLeft;
                    else if (innerR.Contains(client)) HitZone = DockZone.InnerRight;
                    else if (innerT.Contains(client)) HitZone = DockZone.InnerTop;
                    else if (innerB.Contains(client)) HitZone = DockZone.InnerBottom;
                }
                if (HitZone == DockZone.None)
                {
                    if (outerL.Contains(client)) HitZone = DockZone.OuterLeft;
                    else if (outerR.Contains(client)) HitZone = DockZone.OuterRight;
                    else if (outerT.Contains(client)) HitZone = DockZone.OuterTop;
                    else if (outerB.Contains(client)) HitZone = DockZone.OuterBottom;
                }

                if (oldZone != HitZone || oldPane != HitPane) Print();
            }

            float GetDpi()
            {
                try
                {
                    return dockpanel.Dpi;
                }
                catch
                {
                    return Dpi;
                }
            }

            void LayoutZones()
            {
                int cw = TargetRect.Width, ch = TargetRect.Height;
                float dpi = GetDpi();
                int zs = (int)(44 * dpi);
                int gap = (int)(6 * dpi);
                int margin = (int)(18 * dpi);

                // Outer compass — anchored against the panel's edges.
                outerL = new Rectangle(margin, (ch - zs) / 2, zs, zs);
                outerR = new Rectangle(cw - margin - zs, (ch - zs) / 2, zs, zs);
                outerT = new Rectangle((cw - zs) / 2, margin, zs, zs);
                outerB = new Rectangle((cw - zs) / 2, ch - margin - zs, zs, zs);

                // Inner compass — centred on the hit pane, if any.
                hasInner = false;
                if (HitPane != null)
                {
                    var pb = HitPane.Bounds; // in DockPanel client coords == NavForm client coords
                    if (pb.Width >= zs * 3 + gap * 4 && pb.Height >= zs * 3 + gap * 4)
                    {
                        hasInner = true;
                        int cx = pb.X + pb.Width / 2;
                        int cy = pb.Y + pb.Height / 2;
                        innerC = new Rectangle(cx - zs / 2, cy - zs / 2, zs, zs);
                        innerL = new Rectangle(innerC.X - zs - gap, cy - zs / 2, zs, zs);
                        innerR = new Rectangle(innerC.Right + gap, cy - zs / 2, zs, zs);
                        innerT = new Rectangle(cx - zs / 2, innerC.Y - zs - gap, zs, zs);
                        innerB = new Rectangle(cx - zs / 2, innerC.Bottom + gap, zs, zs);
                    }
                }
            }

            public override Bitmap? PrintBit()
            {
                var rect = TargetRectXY;
                var rbmp = new Bitmap(rect.Width, rect.Height);
                using (var g = Graphics.FromImage(rbmp).High(Dpi))
                {
                    LayoutZones();
                    var scheme = dockpanel.ColorScheme;
                    var baseBg = Colour.BgElevated.Get(nameof(DockPanel), scheme);
                    var hoverBg = Colour.PrimaryBg.Get(nameof(DockPanel), scheme);
                    var border = Colour.BorderSecondary.Get(nameof(DockPanel), scheme);
                    var active = Colour.Primary.Get(nameof(DockPanel), scheme);
                    var icon = Colour.TextSecondary.Get(nameof(DockPanel), scheme);

                    // Paint the drop-preview rectangle first so the compass buttons sit on top.
                    var preview = ComputePreviewRect();
                    if (preview.HasValue)
                    {
                        int alpha = 90;
                        var px = Color.FromArgb(alpha, hoverBg.R, hoverBg.G, hoverBg.B);
                        using (var b = new SolidBrush(px)) g.Fill(b, preview.Value);
                        using (var p = new Pen(active, Math.Max(1f, GetDpi()))) g.Draw(p, preview.Value);
                    }
                    // Outer compass
                    DrawZone(g, outerL, DockZone.OuterLeft, SvgDb.IcoLeft, baseBg, hoverBg, border, active, icon);
                    DrawZone(g, outerR, DockZone.OuterRight, SvgDb.IcoRight, baseBg, hoverBg, border, active, icon);
                    DrawZone(g, outerT, DockZone.OuterTop, SvgDb.IcoUp, baseBg, hoverBg, border, active, icon);
                    DrawZone(g, outerB, DockZone.OuterBottom, SvgDb.IcoDown, baseBg, hoverBg, border, active, icon);
                    // Inner compass
                    if (hasInner)
                    {
                        DrawZone(g, innerL, DockZone.InnerLeft, SvgDb.IcoLeft, baseBg, hoverBg, border, active, icon);
                        DrawZone(g, innerR, DockZone.InnerRight, SvgDb.IcoRight, baseBg, hoverBg, border, active, icon);
                        DrawZone(g, innerT, DockZone.InnerTop, SvgDb.IcoUp, baseBg, hoverBg, border, active, icon);
                        DrawZone(g, innerB, DockZone.InnerBottom, SvgDb.IcoDown, baseBg, hoverBg, border, active, icon);
                        DrawZone(g, innerC, DockZone.Center, SvgDb.IcoBorder, baseBg, hoverBg, border, active, icon);
                    }
                }
                return rbmp;
            }

            Rectangle? ComputePreviewRect()
            {
                var cr = TargetRectXY;
                switch (HitZone)
                {
                    case DockZone.OuterLeft: return new Rectangle(cr.X, cr.Y, cr.Width / 2, cr.Height);
                    case DockZone.OuterRight: return new Rectangle(cr.X + cr.Width / 2, cr.Y, cr.Width - cr.Width / 2, cr.Height);
                    case DockZone.OuterTop: return new Rectangle(cr.X, cr.Y, cr.Width, cr.Height / 2);
                    case DockZone.OuterBottom: return new Rectangle(cr.X, cr.Y + cr.Height / 2, cr.Width, cr.Height - cr.Height / 2);
                }
                if (HitPane == null) return null;
                var pb = HitPane.Bounds;
                switch (HitZone)
                {
                    case DockZone.InnerLeft: return new Rectangle(pb.X, pb.Y, pb.Width / 2, pb.Height);
                    case DockZone.InnerRight: return new Rectangle(pb.X + pb.Width / 2, pb.Y, pb.Width - pb.Width / 2, pb.Height);
                    case DockZone.InnerTop: return new Rectangle(pb.X, pb.Y, pb.Width, pb.Height / 2);
                    case DockZone.InnerBottom: return new Rectangle(pb.X, pb.Y + pb.Height / 2, pb.Width, pb.Height - pb.Height / 2);
                    case DockZone.Center: return pb;
                }
                return null;
            }

            void DrawZone(Canvas g, Rectangle r, DockZone z, string svgKey, Color baseBg, Color hoverBg, Color border, Color active, Color iconColor)
            {
                bool isHit = HitZone == z;
                float dpi = GetDpi();
                // Opaque fill first so anti-aliased rounded-corner edges blend with the zone background
                // instead of the Magenta TransparencyKey (which would leave visible pink fringe pixels).
                using (var path = r.RoundPath(6 * dpi))
                {
                    g.Fill(isHit ? hoverBg : baseBg, path);
                    g.Draw(isHit ? active : border, (isHit ? 2f : 1f) * dpi, path);
                }
                int pad = r.Width / 4;
                var iconRect = new Rectangle(r.X + pad, r.Y + pad, r.Width - pad * 2, r.Height - pad * 2);
                g.Svg(svgKey, iconRect, isHit ? active : iconColor);
            }
        }
    }
}
