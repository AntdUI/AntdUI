// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// OutlookBar — Vertical navigation pane with expandable panels, inspired by Outlook's navigation bar.
    /// Renders panel headers as stacked buttons; selected panel's items/content fill the body.
    /// Supports body scrolling, header/body splitter for adjusting visible header count,
    /// a footer icon strip for overflow panels, and a popover item list when collapsed.
    /// </summary>
    [Description("OutlookBar 导航栏")]
    [ToolboxItem(true)]
    [DefaultProperty("Panels")]
    [DefaultEvent("SelectedPanelChanged")]
    public class OutlookBar : IControl
    {
        public OutlookBar()
        {
            base.BackColor = Color.Transparent;
            scroll = new ScrollBar(this, enabledY: true, enabledX: false);
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Enter:
                case Keys.Space:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled) return;
            var sel = SelectedPanel;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (sel != null && sel.Items.Count > 0)
                    {
                        int from = selectedItem == null ? -1 : sel.Items.IndexOf(selectedItem);
                        for (int i = from + 1; i < sel.Items.Count; i++)
                        {
                            if (sel.Items[i].Enabled) { SelectedItem = sel.Items[i]; e.Handled = true; return; }
                        }
                    }
                    break;
                case Keys.Up:
                    if (sel != null && sel.Items.Count > 0)
                    {
                        int from = selectedItem == null ? sel.Items.Count : sel.Items.IndexOf(selectedItem);
                        for (int i = from - 1; i >= 0; i--)
                        {
                            if (sel.Items[i].Enabled) { SelectedItem = sel.Items[i]; e.Handled = true; return; }
                        }
                    }
                    break;
                case Keys.Right:
                    for (int i = selectedIndex + 1; i < panels.Count; i++)
                    {
                        if (panels[i].Visible && panels[i].Enabled) { SelectedIndex = i; e.Handled = true; return; }
                    }
                    break;
                case Keys.Left:
                    for (int i = selectedIndex - 1; i >= 0; i--)
                    {
                        if (panels[i].Visible && panels[i].Enabled) { SelectedIndex = i; e.Handled = true; return; }
                    }
                    break;
                case Keys.Enter:
                case Keys.Space:
                    if (selectedItem != null && selectedItem.Enabled)
                    {
                        selectedItem.RaiseClick();
                        ItemClick?.Invoke(this, new OutlookItemEventArgs(selectedItem));
                        e.Handled = true;
                    }
                    break;
            }
        }

        #region Properties

        Color? back;
        [Description("Background color"), Category("Appearance"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(BackColor));
            }
        }

        Color? fore;
        [Description("Foreground color"), Category("Appearance"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        int headerHeight = 32;
        /// <summary>Height of each panel header button.</summary>
        [Description("Header height"), Category("Layout"), DefaultValue(32)]
        public int HeaderHeight
        {
            get => headerHeight;
            set
            {
                if (headerHeight == value) return;
                headerHeight = value;
                Invalidate();
                OnPropertyChanged(nameof(HeaderHeight));
            }
        }

        int itemHeight = 36;
        /// <summary>Height of each navigation item in the body.</summary>
        [Description("Item height"), Category("Layout"), DefaultValue(36)]
        public int ItemHeight
        {
            get => itemHeight;
            set
            {
                if (itemHeight == value) return;
                itemHeight = value;
                Invalidate();
                OnPropertyChanged(nameof(ItemHeight));
            }
        }

        int footerHeight = 32;
        /// <summary>Height of the footer icon strip (when any panel is demoted to footer).</summary>
        [Description("Footer icon strip height"), Category("Layout"), DefaultValue(32)]
        public int FooterHeight
        {
            get => footerHeight;
            set
            {
                if (footerHeight == value) return;
                footerHeight = value;
                Invalidate();
                OnPropertyChanged(nameof(FooterHeight));
            }
        }

        int radius = 0;
        [Description("Corner radius"), Category("Appearance"), DefaultValue(0)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        bool expanded = true;
        int expandedWidth = 240; // remembers the width to restore when re-expanding
        /// <summary>Whether the bar is expanded (full width) or collapsed (icon-only rail).
        /// Toggling this smoothly animates the control's width between <see cref="CollapsedWidth"/> and the last expanded width.</summary>
        [Description("Expanded"), Category("Behavior"), DefaultValue(true)]
        public bool Expanded
        {
            get => expanded;
            set
            {
                if (expanded == value) return;
                if (expanded) expandedWidth = animating ? animTargetWidth : Width; // save pre-collapse width
                expanded = value;
                int target = expanded ? Math.Max(expandedWidth, collapsedWidth + 1) : collapsedWidth;
                ClosePopover();
                ExpandedChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged(nameof(Expanded));
                StartWidthAnimation(target);
            }
        }

        #region Width animation

        System.Windows.Forms.Timer? animTimer;
        int animStartWidth;
        int animTargetWidth;
        long animStartTicks;
        const int AnimDurationMs = 180;
        bool animating;

        void StartWidthAnimation(int targetWidth)
        {
            if (!IsHandleCreated || DesignMode || AntdUI.Config.Animation == false)
            {
                Width = targetWidth;
                PerformLayout();
                Invalidate();
                return;
            }
            if (animTimer == null)
            {
                animTimer = new System.Windows.Forms.Timer { Interval = 16 };
                animTimer.Tick += OnAnimTick;
            }
            animTimer.Stop(); // guard against overlapping starts while a prior animation is still ticking
            animStartWidth = Width;
            animTargetWidth = targetWidth;
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
                animTimer!.Stop();
                animating = false;
                Width = animTargetWidth;
                PerformLayout();
                Invalidate();
                return;
            }
            // Ease-out cubic for a natural feel.
            double eased = 1.0 - Math.Pow(1.0 - t, 3);
            int w = (int)Math.Round(animStartWidth + (animTargetWidth - animStartWidth) * eased);
            Width = w;
            Invalidate();
        }

        #endregion

        int collapsedWidth = 60;
        /// <summary>Width of the bar when collapsed (icon-only rail).</summary>
        [Description("Collapsed width"), Category("Layout"), DefaultValue(60)]
        public int CollapsedWidth
        {
            get => collapsedWidth;
            set
            {
                if (collapsedWidth == value) return;
                collapsedWidth = value;
                if (!expanded && Width != value) Width = value;
                OnPropertyChanged(nameof(CollapsedWidth));
            }
        }

        int toggleStripHeight = 26;
        /// <summary>Height of the top toggle strip that hosts the collapse/expand affordance.</summary>
        [Description("Toggle strip height"), Category("Layout"), DefaultValue(26)]
        public int ToggleStripHeight
        {
            get => toggleStripHeight;
            set
            {
                if (toggleStripHeight == value) return;
                toggleStripHeight = Math.Max(0, value);
                PerformLayout();
                Invalidate();
                OnPropertyChanged(nameof(ToggleStripHeight));
            }
        }

        bool headerSplitResize = true;
        /// <summary>
        /// When true, a splitter line appears between the selected header and body,
        /// and the user can drag it vertically to promote/demote headers to the footer strip (Outlook behavior).
        /// </summary>
        [Description("Allow dragging header/body splitter to change visible header count"), Category("Behavior"), DefaultValue(true)]
        public bool HeaderSplitResize
        {
            get => headerSplitResize;
            set
            {
                if (headerSplitResize == value) return;
                headerSplitResize = value;
                Invalidate();
                OnPropertyChanged(nameof(HeaderSplitResize));
            }
        }

        List<OutlookPanel> panels = new List<OutlookPanel>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Panels"), Category("Data")]
        public List<OutlookPanel> Panels
        {
            get => panels;
            set
            {
                panels = value ?? new List<OutlookPanel>();
                selectedIndex = panels.Count > 0 ? 0 : -1;
                Invalidate();
            }
        }

        int selectedIndex = -1;
        [Description("Selected panel index"), Category("Data"), DefaultValue(-1)]
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex == value || value < -1) return;
                if (value >= panels.Count) value = panels.Count - 1;
                selectedIndex = value;
                selectedItem = null; // selection is scoped to the current panel
                scroll.Value = 0;
                SyncContentControlParenting();
                Invalidate();
                SelectedPanelChanged?.Invoke(this, new IntEventArgs(value));
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        [Browsable(false)]
        public OutlookPanel? SelectedPanel => (selectedIndex >= 0 && selectedIndex < panels.Count) ? panels[selectedIndex] : null;

        OutlookItem? selectedItem;
        /// <summary>Persistent item selection within the currently-selected panel. Set to null to clear.
        /// Changing this fires <see cref="SelectedItemChanged"/>. Cleared automatically when the selected panel changes.</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OutlookItem? SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem == value) return;
                selectedItem = value;
                Invalidate();
                if (value != null) SelectedItemChanged?.Invoke(this, new OutlookItemEventArgs(value));
            }
        }

        /// <summary>
        /// Max number of panel headers visible in the stack before trailing ones are demoted to the footer icon strip.
        /// A value of 0 (default) means no forced overflow — all headers stack until the body has no room.
        /// Setting this is equivalent to dragging the header/body splitter upward.
        /// </summary>
        [Description("Max panels visible in stack before overflow to footer"), Category("Behavior"), DefaultValue(0)]
        public int PanelsVisible
        {
            get
            {
                int vis = 0;
                for (int i = 0; i < panels.Count; i++) if (panels[i].Visible && !panels[i].InFooter) vis++;
                return Math.Max(0, vis - extraFooterOverflow);
            }
            set
            {
                if (value < 0) value = 0;
                int vis = 0;
                for (int i = 0; i < panels.Count; i++) if (panels[i].Visible && !panels[i].InFooter) vis++;
                if (value > vis) value = vis;
                int overflow = vis - value;
                if (overflow == extraFooterOverflow) return;
                extraFooterOverflow = overflow;
                PerformLayout();
                Invalidate();
                PanelsVisibleChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged(nameof(PanelsVisible));
            }
        }

        #endregion

        #region Events

        [Description("Selected panel changed"), Category("Behavior")]
        public event IntEventHandler? SelectedPanelChanged;

        [Description("Expanded changed"), Category("Behavior")]
        public event EventHandler? ExpandedChanged;

        [Description("Item clicked"), Category("Behavior")]
        public event OutlookItemEventHandler? ItemClick;

        /// <summary>Raised when the persistent <see cref="SelectedItem"/> changes.
        /// Distinct from <see cref="ItemClick"/> — Click fires on every activation,
        /// SelectedItemChanged only fires when the selection transitions.</summary>
        [Description("Selected item changed"), Category("Behavior")]
        public event OutlookItemEventHandler? SelectedItemChanged;

        [Description("PanelsVisible changed"), Category("Behavior")]
        public event EventHandler? PanelsVisibleChanged;

        #endregion

        #region Fields

        readonly FormatFlags s_l = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;
        readonly ScrollBar scroll;

        // Reusable paint scratch — avoids per-paint List<int> and Font allocations.
        readonly List<int> _stack = new List<int>(8);
        readonly List<int> _footer = new List<int>(4);
        Font? f_headerSel, f_headerReg, f_item, f_badge;
        void EnsureFonts()
        {
            if (f_headerSel == null) f_headerSel = new Font(Font.FontFamily, 9F, FontStyle.Bold);
            if (f_headerReg == null) f_headerReg = new Font(Font.FontFamily, 9F, FontStyle.Regular);
            if (f_item == null) f_item = new Font(Font.FontFamily, 8.5F, FontStyle.Regular);
            if (f_badge == null) f_badge = new Font(Font.FontFamily, 7.5F, FontStyle.Bold);
        }
        void DisposeFonts()
        {
            f_headerSel?.Dispose(); f_headerSel = null;
            f_headerReg?.Dispose(); f_headerReg = null;
            f_item?.Dispose(); f_item = null;
            f_badge?.Dispose(); f_badge = null;
        }
        protected override void OnFontChanged(EventArgs e) { DisposeFonts(); base.OnFontChanged(e); Invalidate(); }

        /// <summary>Count of headers currently allowed to render above/within the regular stack (vs demoted to footer).</summary>
        int extraFooterOverflow = 0;

        /// <summary>The scroll-visible portion of the selected body; used for mouse-wheel/mouse hit-testing.</summary>
        Rectangle bodyRect;
        /// <summary>The splitter drag area (top of the body); used for hit-testing.</summary>
        Rectangle splitterRect;
        /// <summary>The footer icon strip area.</summary>
        Rectangle footerRect;

        bool splitterHover;
        bool splitterDown;
        int splitterDownY;
        int splitterDownOverflow;

        // The panel whose ContentControl is currently hosted in our Controls collection for expanded-mode body rendering.
        // Tracks the "owner" so we can unparent cleanly on switch/dispose without touching Controls every paint.
        OutlookPanel? hostedPanel;
        // Last body bounds applied to the hosted ContentControl — re-layout only when this changes.
        Rectangle lastHostedBounds;

        // Top toggle-strip: built-in collapse/expand button (left arrow when expanded, right arrow when collapsed).
        Rectangle toggleRect;
        bool toggleHover;

        // Tooltip (reuses AntdUI's TooltipForm — same pattern as Segmented / Menu / Rate).
        TooltipForm? toolTip;
        object? tooltipTarget; // OutlookItem or OutlookPanel currently anchoring the tip

        #endregion

        #region Rendering

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            float dpi = Dpi;
            float _radius = radius * dpi;
            int _headerH = (int)(headerHeight * dpi);
            int _itemH = (int)(itemHeight * dpi);
            int _footerH = (int)(footerHeight * dpi);
            int _toggleH = (int)(toggleStripHeight * dpi);

            using (var path = rect.RoundPath(_radius))
            {
                g.Fill(back ?? Colour.BgContainer.Get(nameof(OutlookBar), ColorScheme), path);
            }

            var toggleBand = new Rectangle(rect.X, rect.Y, rect.Width, _toggleH);
            rect = new Rectangle(rect.X, rect.Y + _toggleH, rect.Width, rect.Height - _toggleH);

            _stack.Clear();
            _footer.Clear();
            var stack = _stack;
            var footer = _footer;
            for (int i = 0; i < panels.Count; i++)
            {
                if (!panels[i].Visible) continue;
                if (panels[i].InFooter) footer.Add(i);
                else stack.Add(i);
            }

            int maxStackable = ComputeMaxStackable(rect.Height, _headerH, _footerH, footer.Count > 0 || extraFooterOverflow > 0);
            int toDemote = extraFooterOverflow;
            if (stack.Count - toDemote > maxStackable) toDemote = stack.Count - maxStackable;
            if (toDemote < 0) toDemote = 0;
            if (toDemote > stack.Count) toDemote = stack.Count;
            for (int k = 0; k < toDemote; k++)
            {
                int idx = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                footer.Insert(0, idx);
            }

            bool hasFooter = footer.Count > 0;
            int actualFooterH = hasFooter ? _footerH : 0;

            int aboveCount = 0, belowCount = 0;
            for (int k = 0; k < stack.Count; k++)
            {
                int i = stack[k];
                if (i <= selectedIndex) aboveCount++;
                else belowCount++;
            }

            // If selected panel is in the footer, render no stack selection — pick nothing for body.
            bool selectedInStack = false;
            for (int k = 0; k < stack.Count; k++) if (stack[k] == selectedIndex) { selectedInStack = true; break; }

            int topHeadersH = aboveCount * _headerH;
            int bottomHeadersH = belowCount * _headerH;
            int bodyH = rect.Height - topHeadersH - bottomHeadersH - actualFooterH;
            if (bodyH < 0) bodyH = 0;

            var colText = fore ?? Colour.Text.Get(nameof(OutlookBar), ColorScheme);
            var colTextDisabled = Colour.TextQuaternary.Get(nameof(OutlookBar), ColorScheme);
            var colPrimary = Colour.Primary.Get(nameof(OutlookBar), ColorScheme);
            var colPrimaryBg = Colour.PrimaryBg.Get(nameof(OutlookBar), ColorScheme);
            var colHoverBg = Colour.HoverBg.Get(nameof(OutlookBar), ColorScheme);
            var colBgElevated = Colour.BgElevated.Get(nameof(OutlookBar), ColorScheme);
            var colBorder = Colour.BorderColor.Get(nameof(OutlookBar), ColorScheme);
            var colDivider = Colour.BorderSecondary.Get(nameof(OutlookBar), ColorScheme);
            var colSplit = Colour.Split.Get(nameof(OutlookBar), ColorScheme);
            var colTextSecondary = Colour.TextSecondary.Get(nameof(OutlookBar), ColorScheme);
            var colBadgeBg = Colour.Error.Get(nameof(OutlookBar), ColorScheme);
            var colBadgeText = Color.White; // badges ride on the Error token — white is the AntD convention regardless of theme

            EnsureFonts();
            var palette = new Palette(colText, colTextDisabled, colPrimary, colPrimaryBg, colHoverBg, colBgElevated, colDivider, colBadgeBg, colBadgeText, f_headerSel!, f_headerReg!, f_item!, f_badge!);

            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].HeaderRect = RectangleF.Empty;
                panels[i].FooterRect = RectangleF.Empty;
            }

            float y = rect.Y;
            for (int k = 0; k < stack.Count; k++)
            {
                int i = stack[k];
                if (i > selectedIndex) break;
                DrawPanelHeader(g, panels[i], i, new RectangleF(rect.X, y, rect.Width, _headerH), dpi, in palette);
                y += _headerH;
            }

            if (bodyH > 0 && selectedInStack && SelectedPanel != null)
            {
                bodyRect = new Rectangle(rect.X, (int)y, rect.Width, bodyH);
                var selPanel = SelectedPanel;
                if (selPanel.ContentControl != null && selPanel == hostedPanel && selPanel.ContentControl.Parent == this)
                {
                    g.Fill(colBgElevated, bodyRect);
                    // Inset the hosted control by 1px on left/right/bottom so the bar's chrome (dividers + outer
                    // card border) keeps a clean 1px gutter around any user control (TreeView, etc.) — the same
                    // visual treatment the item-list body gets.
                    var hostedInner = new Rectangle(bodyRect.X + 1, bodyRect.Y, bodyRect.Width - 2, bodyRect.Height - 1);
                    if (lastHostedBounds != hostedInner)
                    {
                        selPanel.ContentControl.Bounds = hostedInner;
                        lastHostedBounds = hostedInner;
                    }
                    if (!selPanel.ContentControl.Visible) selPanel.ContentControl.Visible = true;
                }
                else DrawBody(g, bodyRect, _itemH, dpi, in palette);

                // Body gutter lines — integer 1px stroke (not 1×Dpi) to avoid fractional-DPI anti-aliasing that
                // darkens thin lines. Left / right / bottom only; the top edge abuts the selected panel header
                // which already draws its own divider, so a fourth line would stack into a visibly heavier band.
                g.DrawLine(colDivider, 1, new Point(bodyRect.X, bodyRect.Y), new Point(bodyRect.X, bodyRect.Bottom - 1));
                g.DrawLine(colDivider, 1, new Point(bodyRect.Right - 1, bodyRect.Y), new Point(bodyRect.Right - 1, bodyRect.Bottom - 1));
                g.DrawLine(colDivider, 1, new Point(bodyRect.X, bodyRect.Bottom - 1), new Point(bodyRect.Right - 1, bodyRect.Bottom - 1));

                if (headerSplitResize && stack.Count > 0)
                {
                    int gripH = Math.Max(3, (int)(4 * dpi));
                    splitterRect = new Rectangle(rect.X, (int)y, rect.Width, gripH);
                    if (splitterHover || splitterDown) g.Fill(colPrimaryBg, splitterRect);
                    int cx = rect.X + rect.Width / 2;
                    int gw = (int)(18 * dpi);
                    int gy = (int)y + gripH / 2;
                    g.DrawLine(colSplit, 1 * dpi, new PointF(cx - gw / 2, gy), new PointF(cx + gw / 2, gy));
                }
                else splitterRect = Rectangle.Empty;

                y += bodyH;
            }
            else
            {
                bodyRect = Rectangle.Empty;
                splitterRect = Rectangle.Empty;
            }

            for (int k = 0; k < stack.Count; k++)
            {
                int i = stack[k];
                if (i <= selectedIndex) continue;
                DrawPanelHeader(g, panels[i], i, new RectangleF(rect.X, y, rect.Width, _headerH), dpi, in palette);
                y += _headerH;
            }

            if (hasFooter)
            {
                footerRect = new Rectangle(rect.X, rect.Bottom - actualFooterH, rect.Width, actualFooterH);
                DrawFooterStrip(g, footerRect, footer, dpi, in palette);
            }
            else footerRect = Rectangle.Empty;

            DrawToggleStrip(g, toggleBand, dpi, colDivider, colHoverBg, colTextSecondary, colPrimary);

            // Border around the full bar (toggle band + content).
            var fullRect = new Rectangle(toggleBand.X, toggleBand.Y, toggleBand.Width, toggleBand.Height + rect.Height);
            using (var path = fullRect.RoundPath(_radius))
            {
                g.Draw(colBorder, 1.5f * dpi, path);
            }

            base.OnDraw(e);
        }

        void DrawToggleStrip(Canvas g, Rectangle band, float dpi, Color colDivider, Color colHoverBg, Color colIcon, Color colIconActive)
        {
            // Divider beneath
            g.DrawLine(colDivider, 1 * dpi,
                new PointF(band.X, band.Bottom - 1), new PointF(band.Right, band.Bottom - 1));

            int iconSize = (int)(16 * dpi);
            int btn = (int)(22 * dpi);
            int pad = (int)(4 * dpi);
            Rectangle btnRect;
            if (expanded)
            {
                // Right-aligned affordance (reads as "« collapse").
                btnRect = new Rectangle(band.Right - pad - btn, band.Y + (band.Height - btn) / 2, btn, btn);
            }
            else
            {
                // Centered affordance (reads as "» expand").
                btnRect = new Rectangle(band.X + (band.Width - btn) / 2, band.Y + (band.Height - btn) / 2, btn, btn);
            }
            toggleRect = btnRect;

            if (toggleHover)
            {
                using (var p = btnRect.RoundPath(3 * dpi)) { g.Fill(colHoverBg, p); }
            }

            var iconRect = new Rectangle(btnRect.X + (btn - iconSize) / 2, btnRect.Y + (btn - iconSize) / 2, iconSize, iconSize);
            string svg = expanded ? SvgDb.IcoLeft : SvgDb.IcoRight;
            using (var bmp = SvgExtend.GetImgExtend(svg, iconRect, toggleHover ? colIconActive : colIcon))
            {
                if (bmp != null) g.Image(bmp, iconRect);
            }
        }

        int ComputeMaxStackable(int totalH, int headerH, int footerH, bool hasFooter)
        {
            int avail = totalH - (hasFooter ? footerH : 0);
            // Need at least some body height; if not, allow all headers and accept overflow.
            int minBody = headerH;
            int stackable = (avail - minBody) / Math.Max(1, headerH);
            if (stackable < 0) stackable = 0;
            return stackable;
        }

        /// <summary>Bundle of per-paint colour/font tokens passed by <c>in</c> to the draw helpers.
        /// Keeps signatures narrow and makes the "resolve once per paint" contract explicit.</summary>
        readonly struct Palette
        {
            public readonly Color Text, TextDisabled, Primary, PrimaryBg, HoverBg, BgElevated, Divider, BadgeBg, BadgeText;
            public readonly Font FontSel, FontReg, FontItem, FontBadge;
            public Palette(Color t, Color td, Color p, Color pb, Color hb, Color be, Color div, Color badgeBg, Color badgeText, Font fs, Font fr, Font fi, Font fb)
            { Text = t; TextDisabled = td; Primary = p; PrimaryBg = pb; HoverBg = hb; BgElevated = be; Divider = div; BadgeBg = badgeBg; BadgeText = badgeText; FontSel = fs; FontReg = fr; FontItem = fi; FontBadge = fb; }
        }

        /// <summary>Render a badge pill centred vertically on <paramref name="rightCenter"/> (its right edge anchored there).
        /// Returns the badge's drawn size so the caller can subtract it from the text area.</summary>
        Size DrawBadge(Canvas g, string text, PointF rightCenter, float dpi, in Palette c)
        {
            var sz = g.MeasureString(text, c.FontBadge);
            int w = (int)Math.Max(sz.Width + 10 * dpi, 18 * dpi);
            int h = (int)(16 * dpi);
            var rect = new Rectangle((int)(rightCenter.X - w), (int)(rightCenter.Y - h / 2f), w, h);
            using (var path = rect.RoundPath(h / 2f)) g.Fill(c.BadgeBg, path);
            g.String(text, c.FontBadge, c.BadgeText, rect, FormatFlags.Center | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis);
            return new Size(w, h);
        }

        void DrawPanelHeader(Canvas g, OutlookPanel panel, int index, RectangleF rect, float dpi, in Palette c)
        {
            panel.HeaderRect = rect;
            bool isSelected = (index == selectedIndex);
            bool isHover = panel.HeaderHover && !isSelected;

            if (isSelected) g.Fill(c.PrimaryBg, rect);
            else if (isHover) g.Fill(c.HoverBg, rect);

            g.DrawLine(c.Divider, 1 * dpi, new PointF(rect.X, rect.Y), new PointF(rect.Right, rect.Y));

            float pad = 10 * dpi;
            float iconSize = 18 * dpi;

            // Collapsed: reserve the scrollbar column so header icons align vertically with body item icons.
            float reserveR = expanded ? 0f : scroll.SIZE;
            float iconX = expanded ? rect.X + pad : rect.X + (rect.Width - reserveR - iconSize) / 2;
            float iconY = rect.Y + (rect.Height - iconSize) / 2;
            var iconRect = new RectangleF(iconX, iconY, iconSize, iconSize);

            Color iconColor = isSelected ? c.Primary : (panel.Enabled ? c.Text : c.TextDisabled);
            DrawIcon(g, panel.IconSvg, panel.Image, iconRect, iconColor);

            if (expanded)
            {
                float textX = iconRect.Right + 8 * dpi;
                float textRight = rect.Right - pad;
                if (!string.IsNullOrEmpty(panel.Badge))
                {
                    var sz = DrawBadge(g, panel.Badge!, new PointF(textRight, rect.Y + rect.Height / 2), dpi, in c);
                    textRight -= sz.Width + 6 * dpi;
                }
                var textRect = new RectangleF(textX, rect.Y, Math.Max(0, textRight - textX), rect.Height);
                g.String(panel.Text ?? "", isSelected ? c.FontSel : c.FontReg, iconColor, textRect, s_l);
            }
        }

        void DrawBody(Canvas g, Rectangle rect, int itemH, float dpi, in Palette c)
        {
            var panel = SelectedPanel;
            if (panel == null) return;

            g.Fill(c.BgElevated, rect);

            int topPad = (int)(4 * dpi);
            int totalItemsH = panel.Items.Count * itemH + topPad * 2;
            scroll.SizeChange(rect);
            scroll.SetVrSize(totalItemsH);
            int sy = scroll.Value;

            g.SetClip(rect);
            g.TranslateTransform(0, -sy);

            float pad = 10 * dpi;
            float iconSize = 20 * dpi;
            int y = rect.Y + topPad;
            int viewTop = rect.Y + sy;
            int viewBottom = rect.Bottom + sy;
            float scrollReserve = scroll.ShowY ? scroll.SIZE : 0f;
            float contentWidth = rect.Width - scrollReserve;

            for (int idx = 0; idx < panel.Items.Count; idx++)
            {
                var item = panel.Items[idx];
                if (y + itemH < viewTop) { y += itemH; item.Rect = RectangleF.Empty; continue; }
                if (y > viewBottom) { item.Rect = RectangleF.Empty; y += itemH; continue; }

                var itemRect = new RectangleF(rect.X + 2 * dpi, y, contentWidth - 4 * dpi, itemH);
                item.Rect = new RectangleF(itemRect.X, itemRect.Y - sy, itemRect.Width, itemRect.Height);

                bool isSelected = selectedItem == item;
                bool isHover = hoverItem == item && item.Enabled;
                if ((isSelected || isHover) && item.Enabled)
                {
                    using (var path = Rectangle.Round(itemRect).RoundPath(4 * dpi))
                    {
                        g.Fill(isSelected ? c.PrimaryBg : c.HoverBg, path);
                    }
                }

                Color itemColor = !item.Enabled ? c.TextDisabled : (isSelected ? c.Primary : c.Text);
                float iconX = expanded ? itemRect.X + pad : rect.X + (rect.Width - scroll.SIZE - iconSize) / 2;
                float iconY = itemRect.Y + (itemH - iconSize) / 2;
                var iconRect = new RectangleF(iconX, iconY, iconSize, iconSize);
                item.IconRect = iconRect;

                DrawIcon(g, item.IconSvg, item.Image, iconRect, itemColor);

                if (expanded && !string.IsNullOrEmpty(item.Text))
                {
                    float textX = iconRect.Right + 8 * dpi;
                    float textRight = itemRect.Right - pad;
                    // Badge on the right, before the text's right edge.
                    if (!string.IsNullOrEmpty(item.Badge))
                    {
                        var badgeSize = DrawBadge(g, item.Badge!, new PointF(textRight, itemRect.Y + itemH / 2), dpi, c);
                        textRight -= badgeSize.Width + 6 * dpi;
                    }
                    var textRect = new RectangleF(textX, itemRect.Y, Math.Max(0, textRight - textX), itemH);
                    item.TextRect = textRect;
                    var font = isSelected ? c.FontSel : c.FontItem;
                    g.String(item.Text!, font, itemColor, textRect, s_l);
                }

                y += itemH;
            }

            g.ResetTransform();
            g.ResetClip();
            scroll.Paint(g, ColorScheme);
        }

        void DrawFooterStrip(Canvas g, Rectangle rect, List<int> footer, float dpi, in Palette c)
        {
            g.DrawLine(c.Divider, 1 * dpi, new Point(rect.X, rect.Y), new Point(rect.Right, rect.Y));

            int n = footer.Count;
            if (n == 0) return;
            float slot = (float)rect.Width / n;
            float iconSize = 18 * dpi;

            for (int k = 0; k < n; k++)
            {
                int i = footer[k];
                var p = panels[i];
                var cellRect = new RectangleF(rect.X + slot * k, rect.Y, slot, rect.Height);
                p.FooterRect = cellRect;

                bool isSelected = (i == selectedIndex);
                bool isHover = p.FooterHover && !isSelected;
                if (isSelected) g.Fill(c.PrimaryBg, cellRect);
                else if (isHover) g.Fill(c.HoverBg, cellRect);

                float iconX = cellRect.X + (cellRect.Width - iconSize) / 2;
                float iconY = cellRect.Y + (cellRect.Height - iconSize) / 2;
                var iconRect = new RectangleF(iconX, iconY, iconSize, iconSize);

                Color iconColor = isSelected ? c.Primary : (p.Enabled ? c.Text : c.TextDisabled);
                DrawIcon(g, p.IconSvg, p.Image, iconRect, iconColor);
            }
        }

        void DrawIcon(Canvas g, string? svg, Image? img, RectangleF iconRect, Color color)
        {
            if (!string.IsNullOrEmpty(svg))
            {
                var svgRect = Rectangle.Round(iconRect);
                using (var bmp = SvgExtend.GetImgExtend(svg!, svgRect, color))
                {
                    if (bmp != null) g.Image(bmp, svgRect);
                }
            }
            else if (img != null)
            {
                g.Image(Rectangle.Round(iconRect), img, TFit.Contain);
            }
        }

        #endregion

        #region Mouse

        OutlookItem? hoverItem;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (splitterDown)
            {
                int delta = e.Y - splitterDownY;
                int step = Math.Max(1, (int)(headerHeight * Dpi));
                // Dragging down → fewer headers demoted (show more headers). Dragging up → more demoted.
                int want = splitterDownOverflow - (delta / step);
                if (want < 0) want = 0;
                if (want > panels.Count) want = panels.Count;
                if (want != extraFooterOverflow)
                {
                    extraFooterOverflow = want;
                    Invalidate();
                }
                return;
            }

            // ScrollBar returns false when it consumed the event.
            if (!scroll.MouseMove(e.X, e.Y)) { Invalidate(); return; }

            var pt = new PointF(e.X, e.Y);
            bool changed = false;

            bool newToggleHover = !toggleRect.IsEmpty && toggleRect.Contains(e.Location);
            if (newToggleHover != toggleHover) { toggleHover = newToggleHover; changed = true; }

            // Splitter hover
            bool newSplitterHover = headerSplitResize && !splitterRect.IsEmpty && splitterRect.Contains(e.Location);
            if (newSplitterHover != splitterHover)
            {
                splitterHover = newSplitterHover;
                changed = true;
            }

            // Single pass: update panel header/footer hover AND capture which panel (if any) the cursor is on.
            OutlookPanel? hoveredPanel = null;
            for (int i = 0; i < panels.Count; i++)
            {
                var p = panels[i];
                bool was = p.HeaderHover;
                p.HeaderHover = !p.HeaderRect.IsEmpty && p.HeaderRect.Contains(pt);
                if (was != p.HeaderHover) changed = true;

                bool wasF = p.FooterHover;
                p.FooterHover = !p.FooterRect.IsEmpty && p.FooterRect.Contains(pt);
                if (wasF != p.FooterHover) changed = true;

                if (hoveredPanel == null && (p.HeaderHover || p.FooterHover)) hoveredPanel = p;
            }

            OutlookItem? newHover = null;
            if (!bodyRect.IsEmpty && bodyRect.Contains(e.Location))
            {
                var panel = SelectedPanel;
                if (panel != null)
                {
                    foreach (var item in panel.Items)
                    {
                        if (item.Enabled && !item.Rect.IsEmpty && item.Rect.Contains(pt)) { newHover = item; break; }
                    }
                }
            }

            if (newHover != hoverItem)
            {
                hoverItem = newHover;
                changed = true;
                if (newHover != null && !string.IsNullOrEmpty(newHover.Tooltip)) OpenTipForItem(newHover);
                else if (tooltipTarget is OutlookItem) CloseTip();
            }

            if (newHover == null)
            {
                if (hoveredPanel != null && !string.IsNullOrEmpty(hoveredPanel.Tooltip))
                {
                    if (tooltipTarget != hoveredPanel) OpenTipForPanel(hoveredPanel);
                }
                else if (tooltipTarget is OutlookPanel) CloseTip();
            }

            if (changed)
            {
                bool hand = toggleHover || splitterHover || newHover != null;
                if (!hand)
                {
                    for (int i = 0; i < panels.Count; i++)
                    {
                        if (panels[i].HeaderHover || panels[i].FooterHover) { hand = true; break; }
                    }
                }
                SetCursor(hand);
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            bool changed = false;
            foreach (var p in panels)
            {
                if (p.HeaderHover) { p.HeaderHover = false; changed = true; }
                if (p.FooterHover) { p.FooterHover = false; changed = true; }
            }
            if (hoverItem != null) { hoverItem = null; changed = true; }
            if (splitterHover) { splitterHover = false; changed = true; }
            if (toggleHover) { toggleHover = false; changed = true; }
            CloseTip();
            if (changed) { SetCursor(false); Invalidate(); }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            CloseTip();
            if (e.Button != MouseButtons.Left) return;

            // Scrollbar gets first crack at mouse-down (returns false when consumed).
            if (!scroll.MouseDown(e.X, e.Y)) return;

            if (!toggleRect.IsEmpty && toggleRect.Contains(e.Location))
            {
                Expanded = !Expanded;
                return;
            }

            if (headerSplitResize && !splitterRect.IsEmpty && splitterRect.Contains(e.Location))
            {
                splitterDown = true;
                splitterDownY = e.Y;
                splitterDownOverflow = extraFooterOverflow;
                return;
            }

            var pt = new PointF(e.X, e.Y);

            // Header click
            for (int i = 0; i < panels.Count; i++)
            {
                if (!panels[i].Visible || !panels[i].Enabled) continue;
                if (!panels[i].HeaderRect.IsEmpty && panels[i].HeaderRect.Contains(pt))
                {
                    if (!expanded) { SelectedIndex = i; ShowCollapsedPopover(i); }
                    else SelectedIndex = i;
                    return;
                }
                if (!panels[i].FooterRect.IsEmpty && panels[i].FooterRect.Contains(pt))
                {
                    if (!expanded) { SelectedIndex = i; ShowCollapsedPopover(i); }
                    else SelectedIndex = i;
                    return;
                }
            }

            if (!bodyRect.IsEmpty && bodyRect.Contains(e.Location))
            {
                var panelSel = SelectedPanel;
                if (panelSel != null)
                {
                    foreach (var item in panelSel.Items)
                    {
                        if (item.Enabled && !item.Rect.IsEmpty && item.Rect.Contains(pt))
                        {
                            SelectedItem = item;
                            item.RaiseClick();
                            ItemClick?.Invoke(this, new OutlookItemEventArgs(item));
                            return;
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (splitterDown) { splitterDown = false; Invalidate(); }
            if (!scroll.MouseUp()) Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scroll.MouseWheel(e);
            base.OnMouseWheel(e);
        }

        #endregion

        #region Collapsed popover

        /// <summary>
        /// Unified collapsed-click entry. If the panel has a ContentControl, re-parent it into a
        /// borderless popover form for the duration; otherwise fall back to a Menu-based popover
        /// populated from panel.Items.
        /// </summary>
        OutlookFlyoutPanel? flyoutPanel;
        OutlookFlyoutHostForm? flyoutHost;

        void ShowCollapsedPopover(int panelIndex)
        {
            if (expanded || DesignMode) return;
            if (panelIndex < 0 || panelIndex >= panels.Count) return;
            ClosePopover();
            OpenFloatingFlyout(panels[panelIndex]);
        }

        void OpenFloatingFlyout(OutlookPanel panel)
        {
            flyoutPanel = new OutlookFlyoutPanel(this);
            flyoutPanel.SetPanel(panel);
            flyoutPanel.ItemActivated += OnFlyoutItemActivated;

            float dpi = Dpi;
            int w = FlyoutPreferredWidth(panel, dpi);
            int requestedH = FlyoutPreferredHeight(panel, dpi);
            var rect = ComputeFlyoutLocation(w, requestedH, dpi);
            flyoutHost = new OutlookFlyoutHostForm(this)
            {
                StartPosition = FormStartPosition.Manual,
                Bounds = rect,
            };
            flyoutPanel.Dock = DockStyle.Fill;
            flyoutHost.Controls.Add(flyoutPanel);
            flyoutHost.FormClosed += (s, ev) =>
            {
                if (flyoutHost == s)
                {
                    flyoutPanel?.DetachHostedContent();
                    flyoutPanel?.Dispose();
                    flyoutPanel = null;
                    flyoutHost = null;
                    Invalidate();
                }
            };
            flyoutHost.Show(FindForm());
        }

        void OnFlyoutItemActivated(OutlookItem item)
        {
            ItemClick?.Invoke(this, new OutlookItemEventArgs(item));
            if (flyoutHost != null) { try { flyoutHost.Close(); } catch { /* ignore */ } }
        }

        int FlyoutPreferredWidth(OutlookPanel? panel, float dpi)
        {
            var cc = panel?.ContentControl;
            int w = (int)(240 * dpi);
            if (cc != null && cc.Width > w) w = cc.Width;
            return Math.Max(w, (int)(200 * dpi));
        }

        int FlyoutPreferredHeight(OutlookPanel? panel, float dpi)
        {
            var cc = panel?.ContentControl;
            if (cc != null && cc.Height > 0) return Math.Max(cc.Height, (int)(280 * dpi));
            int itemH = Math.Max((int)(itemHeight * dpi), (int)(32 * dpi));
            int header = (int)(32 * dpi);
            int itemCount = panel?.Items.Count ?? 0;
            return Math.Min((int)(420 * dpi), header + (int)(8 * dpi) + Math.Max(itemH, itemCount * itemH + (int)(8 * dpi)));
        }

        /// <summary>
        /// Compute the flyout's screen position. The flyout spans the full height of the collapsed rail
        /// (top-to-bottom of the bar in screen coords) regardless of which icon was clicked — reads as a
        /// "drawer" sliding out of the rail rather than a popup floating near the cursor.
        /// </summary>
        Rectangle ComputeFlyoutLocation(int w, int requestedHeight, float dpi)
        {
            var barScreen = RectangleToScreen(ClientRectangle);
            var work = Screen.FromControl(this).WorkingArea;
            int x = barScreen.Right + (int)(4 * dpi);
            if (x + w > work.Right) x = barScreen.Left - w - (int)(4 * dpi);
            if (x < work.Left) x = work.Left;
            int y = barScreen.Top;
            int height = barScreen.Height;
            // Clamp to working area so the flyout never exceeds the screen.
            if (y < work.Top) { height -= work.Top - y; y = work.Top; }
            if (y + height > work.Bottom) height = work.Bottom - y;
            if (height < requestedHeight / 2) height = Math.Min(requestedHeight, work.Bottom - y);
            return new Rectangle(x, y, w, height);
        }

        void ClosePopover()
        {
            if (flyoutHost != null)
            {
                try { if (!flyoutHost.IsDisposed) flyoutHost.Close(); }
                catch { /* ignore */ }
            }
        }

        /// <summary>
        /// Host for the flyout. Plain <see cref="Form"/> (not <see cref="LayeredFormPopover"/>) because the flyout
        /// can host an arbitrary user <see cref="Control"/> via <see cref="OutlookPanel.ContentControl"/> — layered
        /// windows composite via a single alpha-blended bitmap and can't render child window handles (a user's
        /// TreeView/ListView etc. wouldn't paint). Auto-dismisses on deactivate.
        /// </summary>
        sealed class OutlookFlyoutHostForm : Form
        {
            public OutlookFlyoutHostForm(OutlookBar owner)
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                KeyPreview = true;
                BackColor = Colour.BgContainer.Get(nameof(OutlookBar), owner.ColorScheme);
            }

            protected override void OnDeactivate(EventArgs e)
            {
                base.OnDeactivate(e);
                if (Controls.Count == 0) return;
                BeginInvoke(new Action(() => { try { Close(); } catch { /* ignore */ } }));
            }
        }

        /// <summary>
        /// Make sure the currently selected panel's ContentControl (if any) is parented into this bar,
        /// and any previously hosted ContentControl from a different panel is unparented. Called whenever
        /// selection changes or panel membership shifts.
        /// </summary>
        void SyncContentControlParenting()
        {
            if (DesignMode) return;

            // If the flyout is currently showing the ContentControl (floating or pinned), leave it alone —
            // the flyout owns parentage until it closes / swaps panels.
            if (flyoutPanel != null && flyoutPanel.Panel?.ContentControl is Control flyoutCC && flyoutCC.Parent != this) return;

            var sel = SelectedPanel;
            Control? desired = sel?.ContentControl;

            // Unparent the previously-hosted control if the panel changed or no longer has a ContentControl.
            if (hostedPanel != null && (hostedPanel != sel || desired == null))
            {
                var prev = hostedPanel.ContentControl;
                if (prev != null && prev.Parent == this)
                {
                    Controls.Remove(prev);
                }
                hostedPanel = null;
                lastHostedBounds = Rectangle.Empty;
            }

            if (desired != null && sel != null)
            {
                if (desired.Parent != this)
                {
                    // If it currently lives somewhere else (e.g. designer parent or a closed popover), take custody.
                    desired.Parent?.Controls.Remove(desired);
                    // Strip any Dock — we position explicitly via Bounds = bodyRect each paint.
                    // Without this, Dock=Fill (common default) would make the control fill the entire bar and hide headers.
                    desired.Dock = DockStyle.None;
                    desired.Anchor = AnchorStyles.None;
                    Controls.Add(desired);
                }
                hostedPanel = sel;
                lastHostedBounds = Rectangle.Empty;
                desired.Visible = true;
                desired.BringToFront();
            }
        }

        #endregion

        #region Tooltip

        void CloseTip()
        {
            if (toolTip != null)
            {
                try { toolTip.IClose(); } catch { /* ignore */ }
                toolTip = null;
            }
            tooltipTarget = null;
        }

        void OpenTipForItem(OutlookItem item) => OpenTip(item, item.Tooltip, Rectangle.Round(item.Rect));

        void OpenTipForPanel(OutlookPanel panel)
        {
            var src = panel.HeaderRect.IsEmpty ? panel.FooterRect : panel.HeaderRect;
            OpenTip(panel, panel.Tooltip, Rectangle.Round(src));
        }

        void OpenTip(object target, string? text, Rectangle anchor)
        {
            if (string.IsNullOrEmpty(text)) { CloseTip(); return; }
            if (tooltipTarget == target && toolTip != null) return;
            CloseTip();
            if (DesignMode || !IsHandleCreated) return;
            if (anchor.Width <= 0 || anchor.Height <= 0) return;
            try
            {
                toolTip = new TooltipForm(this, anchor, text!, new TooltipConfig { Font = Font, ArrowAlign = TAlign.Right });
                toolTip.Show(this);
                tooltipTarget = target;
            }
            catch { toolTip = null; }
        }

        #endregion

        #region Layout / Lifecycle

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // First time we can actually host a child — parent the selected panel's ContentControl.
            SyncContentControlParenting();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Detach any hosted ContentControl first so the base Control.Dispose doesn't dispose it.
                // The panel's ContentControl lifetime is owned by the caller, never by us.
                if (hostedPanel?.ContentControl is Control hosted && hosted.Parent == this)
                {
                    Controls.Remove(hosted);
                }
                hostedPanel = null;

                // Flyout path: release the hosted ContentControl before disposing the flyout panel.
                if (flyoutPanel != null)
                {
                    flyoutPanel.DetachHostedContent();
                    if (Controls.Contains(flyoutPanel)) Controls.Remove(flyoutPanel);
                    try { flyoutPanel.Dispose(); } catch { /* ignore */ }
                    flyoutPanel = null;
                }
                if (flyoutHost != null)
                {
                    try { flyoutHost.Close(); } catch { /* ignore */ }
                    flyoutHost = null;
                }

                CloseTip();
                DisposeFonts();
                animTimer?.Stop();
                animTimer?.Dispose();
                animTimer = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
