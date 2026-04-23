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
    /// Ribbon — A tabbed toolbar control with collapsible groups and items, styled with AntDUI's design tokens.
    /// </summary>
    /// <remarks>Office-style ribbon toolbar with tab strip, groups, and items.</remarks>
    [Description("Ribbon 工具栏")]
    [ToolboxItem(true)]
    [DefaultProperty("TabPages")]
    [DefaultEvent("SelectedTabChanged")]
    public class Ribbon : IControl
    {
        public Ribbon()
        {
            base.BackColor = Color.Transparent;
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        #region Properties

        Color? back;
        /// <summary>Background color override.</summary>
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
        /// <summary>Foreground color override.</summary>
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

        int radius = 0;
        /// <summary>Corner radius.</summary>
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

        int tabHeight = 32;
        /// <summary>Height of the tab strip area.</summary>
        [Description("Tab strip height"), Category("Layout"), DefaultValue(32)]
        public int TabHeight
        {
            get => tabHeight;
            set
            {
                if (tabHeight == value) return;
                tabHeight = value;
                Invalidate();
                OnPropertyChanged(nameof(TabHeight));
            }
        }

        int quickAccessHeight = 24;
        /// <summary>Height of the Quick Access Toolbar band shown above the tab strip (when <see cref="QuickAccessItems"/> is non-empty).</summary>
        [Description("QAT height"), Category("Layout"), DefaultValue(24)]
        public int QuickAccessHeight
        {
            get => quickAccessHeight;
            set { if (quickAccessHeight == value) return; quickAccessHeight = Math.Max(0, value); Invalidate(); OnPropertyChanged(nameof(QuickAccessHeight)); }
        }

        readonly List<RibbonItem> quickAccessItems = new List<RibbonItem>();
        /// <summary>Pinned commands shown in the Quick Access Toolbar (thin band above the tab strip).
        /// Small icon buttons; no text. Empty = QAT hidden.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("QAT pinned items"), Category("Data")]
        public List<RibbonItem> QuickAccessItems => quickAccessItems;

        int contentHeight = 90;
        /// <summary>Height of the content/body area (groups + items).</summary>
        [Description("Content area height"), Category("Layout"), DefaultValue(90)]
        public int ContentHeight
        {
            get => contentHeight;
            set
            {
                if (contentHeight == value) return;
                contentHeight = value;
                Invalidate();
                OnPropertyChanged(nameof(ContentHeight));
            }
        }

        int groupTitleHeight = 18;
        /// <summary>Height of the group title footer.</summary>
        [Description("Group title height"), Category("Layout"), DefaultValue(18)]
        public int GroupTitleHeight
        {
            get => groupTitleHeight;
            set
            {
                if (groupTitleHeight == value) return;
                groupTitleHeight = value;
                Invalidate();
                OnPropertyChanged(nameof(GroupTitleHeight));
            }
        }

        bool showCollapseToggle = true;
        /// <summary>Show a small chevron button at the right end of the tab strip that toggles <see cref="Collapsed"/>
        /// — matches the native Excel/Word affordance so end-users don't need a host-supplied button.</summary>
        [Description("Show built-in collapse toggle button"), Category("Behavior"), DefaultValue(true)]
        public bool ShowCollapseToggle
        {
            get => showCollapseToggle;
            set { if (showCollapseToggle == value) return; showCollapseToggle = value; Invalidate(); OnPropertyChanged(nameof(ShowCollapseToggle)); }
        }

        bool collapsed = false;
        /// <summary>Whether the ribbon content is collapsed (only tabs visible). Transition animates.</summary>
        [Description("Collapsed"), Category("Behavior"), DefaultValue(false)]
        public bool Collapsed
        {
            get => collapsed;
            set
            {
                if (collapsed == value) return;
                collapsed = value;
                if (!collapsed) ClosePopup();
                StartCollapseAnim(collapsed ? 0f : 1f);
                CollapsedChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged(nameof(Collapsed));
            }
        }

        // Collapse animation progress: 1 = fully expanded content, 0 = collapsed (tabs only).
        float collapseProg = 1f;
        System.Windows.Forms.Timer? collapseTimer;
        float collapseStart;
        float collapseTarget;
        long collapseStartTicks;
        const int CollapseDurationMs = 180;

        void StartCollapseAnim(float target)
        {
            if (!IsHandleCreated || DesignMode || AntdUI.Config.Animation == false)
            {
                collapseProg = target;
                ApplyAnimatedHeight();
                Invalidate();
                return;
            }
            if (collapseTimer == null)
            {
                collapseTimer = new System.Windows.Forms.Timer { Interval = 16 };
                collapseTimer.Tick += OnCollapseTick;
            }
            // Stop first so a mid-flight toggle resets its start-ticks cleanly — the ease-out origin
            // is the current `collapseProg`, which is already mid-interpolation on a rapid re-toggle.
            collapseTimer.Stop();
            collapseStart = collapseProg;
            collapseTarget = target;
            collapseStartTicks = Environment.TickCount;
            collapseTimer.Start();
        }

        void OnCollapseTick(object? sender, EventArgs e)
        {
            long elapsed = Environment.TickCount - collapseStartTicks;
            double t = Math.Min(1.0, (double)elapsed / CollapseDurationMs);
            double eased = 1.0 - Math.Pow(1.0 - t, 3);
            collapseProg = (float)(collapseStart + (collapseTarget - collapseStart) * eased);
            if (t >= 1.0)
            {
                collapseTimer!.Stop();
                collapseProg = collapseTarget;
            }
            ApplyAnimatedHeight();
            Invalidate();
        }

        /// <summary>Resizes the control (when <see cref="AutoCollapseHeight"/> is on) to track the
        /// current collapse progress — expanded height when <c>collapseProg==1</c>, collapsed
        /// height when <c>0</c>, linear interpolation in between. This is what frees the screen
        /// real estate below the ribbon as it minimises, matching Excel/Word behaviour.</summary>
        void ApplyAnimatedHeight()
        {
            if (!autoCollapseHeight || !IsHandleCreated || DesignMode) return;
            float dpi = Dpi;
            int qat = quickAccessItems.Count > 0 ? (int)(quickAccessHeight * dpi) : 0;
            int tab = (int)(tabHeight * dpi);
            int content = (int)(contentHeight * dpi * collapseProg);
            int target = qat + tab + content;
            if (Height != target) Height = target;
        }

        bool autoCollapseHeight = true;
        /// <summary>When true (default), the Ribbon resizes its own <see cref="Control.Height"/> as
        /// it collapses/expands so the area below is released back to the host layout — matches
        /// Excel/Word. Set to false if the host wants to drive the ribbon's height manually.</summary>
        [Description("Auto-size Height to track Collapsed state"), Category("Behavior"), DefaultValue(true)]
        public bool AutoCollapseHeight
        {
            get => autoCollapseHeight;
            set { if (autoCollapseHeight == value) return; autoCollapseHeight = value; if (value) ApplyAnimatedHeight(); OnPropertyChanged(nameof(AutoCollapseHeight)); }
        }

        List<RibbonTabPage> tabPages = new List<RibbonTabPage>();
        /// <summary>Collection of tab pages.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Tab pages"), Category("Data")]
        public List<RibbonTabPage> TabPages
        {
            get => tabPages;
            set
            {
                tabPages = value ?? new List<RibbonTabPage>();
                selectedIndex = tabPages.Count > 0 ? 0 : -1;
                Invalidate();
            }
        }

        int selectedIndex = -1;
        /// <summary>Index of the currently selected tab.</summary>
        [Description("Selected tab index"), Category("Data"), DefaultValue(-1)]
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex == value || value < -1) return;
                if (value >= tabPages.Count) value = tabPages.Count - 1;
                selectedIndex = value;
                ClosePopup();
                Invalidate();
                SelectedTabChanged?.Invoke(this, new IntEventArgs(value));
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        /// <summary>Gets the currently selected tab page.</summary>
        [Browsable(false)]
        public RibbonTabPage? SelectedTab => (selectedIndex >= 0 && selectedIndex < tabPages.Count) ? tabPages[selectedIndex] : null;

        #endregion

        #region Events

        /// <summary>Fires when the selected tab changes.</summary>
        [Description("Selected tab changed"), Category("Behavior")]
        public event IntEventHandler? SelectedTabChanged;

        /// <summary>Fires when collapsed state changes.</summary>
        [Description("Collapsed changed"), Category("Behavior")]
        public event EventHandler? CollapsedChanged;

        /// <summary>Fires when a ribbon item is clicked.</summary>
        [Description("Item clicked"), Category("Behavior")]
        public event RibbonItemEventHandler? ItemClick;

        #endregion

        #region Rendering

        readonly FormatFlags s_c = FormatFlags.Center | FormatFlags.NoWrapEllipsis;
        readonly FormatFlags s_cb = FormatFlags.Center | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;
        readonly FormatFlags s_lb = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;

        // Cached fonts — created lazily, disposed on font/handle change
        Font? f_tab, f_tab_bold, f_group, f_large, f_small;

        void EnsureFonts()
        {
            if (f_tab == null) f_tab = new Font(Font.FontFamily, 8F, FontStyle.Regular);
            if (f_tab_bold == null) f_tab_bold = new Font(Font.FontFamily, 8F, FontStyle.Bold);
            // Group-title font: bold, one point larger than the ribbon's base font so the label row
            // reads as a section heading rather than a fine-print footnote.
            if (f_group == null) f_group = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            if (f_large == null) f_large = new Font(Font.FontFamily, 7.5F, FontStyle.Regular);
            if (f_small == null) f_small = new Font(Font.FontFamily, 8F, FontStyle.Regular);
        }

        void DisposeFonts()
        {
            f_tab?.Dispose(); f_tab = null;
            f_tab_bold?.Dispose(); f_tab_bold = null;
            f_group?.Dispose(); f_group = null;
            f_large?.Dispose(); f_large = null;
            f_small?.Dispose(); f_small = null;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            DisposeFonts();
            base.OnFontChanged(e);
            Invalidate();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // Snap to the correct height once the handle exists — otherwise a ribbon instantiated
            // with Collapsed=true or with AutoCollapseHeight on a new form would briefly show at
            // whatever Height the designer set before the first user interaction resized it.
            ApplyAnimatedHeight();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ClosePopup();
            DisposeFonts();
            collapseTimer?.Stop();
            collapseTimer?.Dispose();
            collapseTimer = null;
            base.OnHandleDestroyed(e);
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            EnsureFonts();

            float dpi = Dpi;
            float _radius = radius * dpi;
            int _tabH = (int)(tabHeight * dpi);
            int _contentH = (int)(contentHeight * dpi * collapseProg);
            int _groupTitleH = (int)(groupTitleHeight * dpi);
            int _qatH = quickAccessItems.Count > 0 ? (int)(quickAccessHeight * dpi) : 0;
            int totalH = _qatH + _tabH + _contentH;

            var palette = BuildPalette();

            var fullRect = new Rectangle(rect.X, rect.Y, rect.Width, Math.Min(totalH, rect.Height));

            using (var path = fullRect.RoundPath(_radius)) g.Fill(palette.Bg, path);

            // Quick Access Toolbar — thin band above the tab strip; drawn before tabs so its border sits under them.
            if (_qatH > 0)
            {
                var qatRect = new Rectangle(rect.X, rect.Y, rect.Width, _qatH);
                using (var path = qatRect.RoundPath(_radius, TAlignRound.Top)) g.Fill(palette.TabBg, path);
                DrawQuickAccess(g, qatRect, dpi, in palette);
                g.DrawLine(palette.Divider, 1 * dpi,
                    new PointF(rect.X, rect.Y + _qatH), new PointF(rect.X + rect.Width, rect.Y + _qatH));
            }

            var tabStripRect = new Rectangle(rect.X, rect.Y + _qatH, rect.Width, _tabH);
            if (_qatH > 0) g.Fill(palette.TabBg, tabStripRect);
            else using (var path = tabStripRect.RoundPath(_radius, TAlignRound.Top)) g.Fill(palette.TabBg, path);
            DrawTabStrip(g, tabStripRect, dpi, in palette);

            g.DrawLine(palette.Divider, 1 * dpi,
                new PointF(rect.X, tabStripRect.Bottom), new PointF(rect.X + rect.Width, tabStripRect.Bottom));

            if (_contentH > 0)
            {
                // Clamp to available client height so the group-title row never gets clipped when the
                // host gives us less space than GetPreferredHeight() would use.
                int availableH = Math.Max(0, rect.Height - _qatH - _tabH);
                int drawH = Math.Min(_contentH, availableH);
                if (drawH > 0)
                {
                    var contentRect = new Rectangle(rect.X, tabStripRect.Bottom, rect.Width, drawH);
                    // Clip to the shrinking content rect during collapse — DrawLargeItem positions
                    // items via `(areaH - totalItemH) / 2`, which goes negative when areaH drops
                    // below the item height mid-animation and makes icons bleed up into the tab
                    // strip for the last few frames before vanishing. Clipping keeps the transition
                    // smooth.
                    var prevClip = g.Clip;
                    try
                    {
                        g.SetClip(contentRect);
                        DrawContent(g, contentRect, _groupTitleH, dpi, in palette);
                    }
                    finally
                    {
                        if (prevClip != null) g.Clip = prevClip;
                    }
                }
            }

            using (var path = fullRect.RoundPath(_radius)) g.Draw(palette.Border, 1.5f * dpi, path);

            if (fullRect.Bottom + 1 < rect.Bottom)
            {
                g.DrawLine(ShadowColor, 1 * dpi,
                    new PointF(fullRect.X + _radius, fullRect.Bottom + 1),
                    new PointF(fullRect.Right - _radius, fullRect.Bottom + 1));
            }

            base.OnDraw(e);
        }

        // Card-lift shadow — a subtle 7% black line below the ribbon card. Const keeps the allocation
        // out of the hot paint path and signals that the value is deliberately raw (not a theme token).
        static readonly Color ShadowColor = Color.FromArgb(18, 0, 0, 0);

        /// <summary>Per-paint colour/font bundle. Resolved once in <see cref="OnDraw"/> and passed by
        /// <c>in</c> to each draw helper so token lookups don't recur per tab / group / item.</summary>
        readonly struct Palette
        {
            public readonly Color Bg, TabBg, Border, Divider, Primary, PrimaryBg, HoverBg, FillSecondary, Split, Text, TextTertiary, TextQuaternary;
            public Palette(Color bg, Color tabBg, Color border, Color divider, Color primary, Color primaryBg, Color hoverBg, Color fillSecondary, Color split, Color text, Color textTertiary, Color textQuaternary)
            { Bg = bg; TabBg = tabBg; Border = border; Divider = divider; Primary = primary; PrimaryBg = primaryBg; HoverBg = hoverBg; FillSecondary = fillSecondary; Split = split; Text = text; TextTertiary = textTertiary; TextQuaternary = textQuaternary; }
        }

        void DrawQuickAccess(Canvas g, Rectangle band, float dpi, in Palette p)
        {
            float pad = 4 * dpi;
            float iconSize = band.Height - pad * 2;
            float btnW = iconSize + pad * 2;
            float x = band.X + pad;
            foreach (var qi in quickAccessItems)
            {
                if (!qi.Visible) continue;
                var btn = new RectangleF(x, band.Y + pad, btnW, iconSize);
                qi.Rect = btn;
                bool isHover = qi == hoverItem;
                bool isPressed = qi == pressedItem;
                if (qi.Enabled && (isHover || isPressed || qi.Checked))
                {
                    Color bg = isPressed ? p.FillSecondary : (qi.Checked ? p.PrimaryBg : p.HoverBg);
                    using (var path = Rectangle.Round(btn).RoundPath(3 * dpi)) g.Fill(bg, path);
                }
                var iconRect = new RectangleF(btn.X + pad, btn.Y, iconSize, iconSize);
                qi.IconRect = iconRect;
                DrawItemIcon(g, qi, iconRect, dpi, in p);
                x += btnW + pad;
                if (x > band.Right) break;
            }
        }

        // Right-edge chevron toggle — computed once per paint, hit-tested in OnMouseDown/Move.
        RectangleF collapseToggleRect;
        bool hoverCollapseToggle;

        void DrawTabStrip(Canvas g, Rectangle rect, float dpi, in Palette p)
        {
            if (tabPages.Count == 0) return;

            float tabPadX = 16 * dpi;
            float tabPadY = 4 * dpi;
            float x = rect.X + tabPadX / 2;

            // Reserve the collapse-toggle slot on the far right. Tabs still render left-to-right; the
            // toggle is drawn after the loop so it always sits above the tab strip even if tabs are absent.
            float toggleSize = rect.Height - tabPadY * 2;
            float toggleMargin = 6 * dpi;
            if (showCollapseToggle)
            {
                collapseToggleRect = new RectangleF(rect.Right - toggleMargin - toggleSize, rect.Y + tabPadY, toggleSize, toggleSize);
            }
            else collapseToggleRect = RectangleF.Empty;

            var font = f_tab!;
            var fontBold = f_tab_bold!;

            for (int i = 0; i < tabPages.Count; i++)
            {
                var page = tabPages[i];
                if (!page.Visible) continue;

                var textSize = g.MeasureString(page.Text ?? "", font);
                float tabW = textSize.Width + tabPadX;
                float tabH = rect.Height - tabPadY * 2;
                var tabRect = new RectangleF(x, rect.Y + tabPadY, tabW, tabH);
                page.TabRect = tabRect;

                bool isSelected = (i == selectedIndex) && !collapsed;
                bool isPopupOpenTab = (i == selectedIndex) && collapsed && popup != null;
                bool isHover = page.TabHover && !isSelected && !isPopupOpenTab;

                // Contextual tabs override the indicator / accent colour so users can distinguish them from
                // regular tabs (e.g. an orange "Picture Tools" strip). Non-contextual tabs use the theme Primary.
                Color accent = page.IsContextual && page.ContextColor.HasValue ? page.ContextColor.Value : p.Primary;

                // Contextual accent strip painted above the tab whenever the tab is visible — signals the tab's
                // "special" status even when not selected.
                if (page.IsContextual)
                {
                    var accentStrip = new RectangleF(tabRect.X + 4 * dpi, tabRect.Y - 2 * dpi, tabRect.Width - 8 * dpi, 2 * dpi);
                    g.Fill(Color.FromArgb(180, accent.R, accent.G, accent.B), accentStrip);
                }

                if (isSelected || isPopupOpenTab)
                {
                    var selBg = page.IsContextual ? Color.FromArgb(30, accent.R, accent.G, accent.B) : p.PrimaryBg;
                    using (var path = Rectangle.Round(tabRect).RoundPath(4 * dpi)) g.Fill(selBg, path);
                    var barRect = new RectangleF(tabRect.X + 4 * dpi, tabRect.Bottom - 2.5f * dpi, tabRect.Width - 8 * dpi, 2.5f * dpi);
                    using (var path = Rectangle.Round(barRect).RoundPath(1.5f * dpi)) g.Fill(accent, path);
                    g.String(page.Text ?? "", fontBold, accent, tabRect, s_cb);
                }
                else
                {
                    if (isHover)
                    {
                        using (var path = Rectangle.Round(tabRect).RoundPath(4 * dpi)) g.Fill(p.HoverBg, path);
                    }
                    var textColor = !page.Enabled ? p.TextQuaternary : (page.IsContextual ? accent : p.Text);
                    g.String(page.Text ?? "", font, textColor, tabRect, s_cb);
                }

                x += tabW + 2 * dpi;
            }

            if (showCollapseToggle && !collapseToggleRect.IsEmpty)
            {
                if (hoverCollapseToggle)
                {
                    using var path = Rectangle.Round(collapseToggleRect).RoundPath(4 * dpi);
                    g.Fill(p.HoverBg, path);
                }
                // Collapsed → chevron points down (hints "open"); expanded → points up (hints "close").
                string glyph = collapsed ? SvgDb.IcoDown : SvgDb.IcoUp;
                Color glyphColor = hoverCollapseToggle ? p.Primary : p.TextTertiary;
                float pad = 5 * dpi;
                var glyphRect = Rectangle.Round(new RectangleF(
                    collapseToggleRect.X + pad, collapseToggleRect.Y + pad,
                    collapseToggleRect.Width - pad * 2, collapseToggleRect.Height - pad * 2));
                using var bmp = SvgExtend.GetImgExtend(glyph, glyphRect, glyphColor);
                if (bmp != null) g.Image(bmp, glyphRect);
            }
        }

        /// <summary>Renders a tab page's content (groups + items) into the supplied rect. Called by <see cref="RibbonPopup"/> when the ribbon is collapsed.</summary>
        internal void DrawTabContent(Canvas g, RibbonTabPage tab, Rectangle rect, float dpi)
        {
            EnsureFonts();
            var palette = BuildPalette();
            int _groupTitleH = (int)(groupTitleHeight * dpi);
            DrawContentCore(g, tab, rect, _groupTitleH, dpi, in palette);
        }

        /// <summary>Builds the per-paint <see cref="Palette"/> snapshot. Single source of truth for
        /// which theme tokens the ribbon reads — paint paths differ in layout, not in colour.</summary>
        Palette BuildPalette()
        {
            var scheme = ColorScheme;
            var name = nameof(Ribbon);
            return new Palette(
                back ?? Colour.BgContainer.Get(name, scheme),
                Colour.BgElevated.Get(name, scheme),
                Colour.BorderColor.Get(name, scheme),
                Colour.BorderSecondary.Get(name, scheme),
                Colour.Primary.Get(name, scheme),
                Colour.PrimaryBg.Get(name, scheme),
                Colour.HoverBg.Get(name, scheme),
                Colour.FillSecondary.Get(name, scheme),
                Colour.Split.Get(name, scheme),
                fore ?? Colour.Text.Get(name, scheme),
                Colour.TextTertiary.Get(name, scheme),
                Colour.TextQuaternary.Get(name, scheme));
        }

        void DrawContent(Canvas g, Rectangle rect, int groupTitleH, float dpi, in Palette p)
        {
            var tab = SelectedTab;
            if (tab == null) return;
            DrawContentCore(g, tab, rect, groupTitleH, dpi, in p);
        }

        void DrawContentCore(Canvas g, RibbonTabPage tab, Rectangle rect, int groupTitleH, float dpi, in Palette p)
        {
            float groupPad = 8 * dpi;
            float itemPad = 4 * dpi;
            float sepWidth = 1 * dpi;
            float x = rect.X + groupPad;
            float itemsAreaH = rect.Height - groupTitleH;

            foreach (var group in tab.Groups)
            {
                if (!group.Visible) continue;

                float groupStartX = x;

                // Layout items: walk the items list, batching consecutive Small items into columns of 3.
                float ix = x;
                int i = 0;
                var items = group.Items;
                while (i < items.Count)
                {
                    var item = items[i];
                    if (!item.Visible) { i++; continue; }

                    if (item.Size == RibbonItemSize.Large)
                    {
                        DrawLargeItem(g, item, ref ix, rect.Y, itemsAreaH, itemPad, dpi, in p);
                        i++;
                    }
                    else
                    {
                        // Gather up to 3 consecutive small items into a column
                        int colStart = i;
                        int colCount = 0;
                        while (i < items.Count && colCount < 3)
                        {
                            var s = items[i];
                            if (!s.Visible) { i++; continue; }
                            if (s.Size != RibbonItemSize.Small) break;
                            colCount++;
                            i++;
                        }
                        if (colCount > 0)
                            DrawSmallColumn(g, items, colStart, colCount, ref ix, rect.Y, itemsAreaH, itemPad, dpi, in p);
                    }
                }

                float groupWidth = ix - groupStartX;
                if (groupWidth < 30 * dpi) groupWidth = 30 * dpi;

                group.Rect = new RectangleF(groupStartX, rect.Y, groupWidth, rect.Height);

                var titleRect = new RectangleF(groupStartX, rect.Y + itemsAreaH, groupWidth, groupTitleH);
                group.TitleRect = titleRect;

                g.String(group.Text ?? "", f_group!, p.TextTertiary, titleRect, s_c);

                x = groupStartX + groupWidth + groupPad;

                if (group.ShowSeparator)
                {
                    float sepX = x - groupPad / 2;
                    var sepRect = new RectangleF(sepX, rect.Y + 6 * dpi, sepWidth, rect.Height - 12 * dpi);
                    group.SeparatorRect = sepRect;
                    g.Fill(p.Split, sepRect);
                }
            }
        }

        void DrawLargeItem(Canvas g, RibbonItem item, ref float x, float y, float areaH, float pad, float dpi, in Palette p)
        {
            float iconSize = 28 * dpi;
            float caretW = item.IsSplit ? 14 * dpi : 0;
            float itemW = Math.Max(48 * dpi, iconSize + 8 * dpi) + caretW;
            float textH = 16 * dpi;
            float totalItemH = iconSize + textH + 4 * dpi;

            var itemRect = new RectangleF(x, y + (areaH - totalItemH) / 2 - 2 * dpi, itemW, totalItemH + 4 * dpi);
            item.Rect = itemRect;

            var primaryRect = item.IsSplit
                ? new RectangleF(itemRect.X, itemRect.Y, itemRect.Width - caretW, itemRect.Height)
                : itemRect;
            var caretRect = item.IsSplit
                ? new RectangleF(itemRect.Right - caretW, itemRect.Y, caretW, itemRect.Height)
                : RectangleF.Empty;
            item.CaretRect = caretRect;

            bool isHover = item == hoverItem;
            bool isPressed = item == pressedItem;
            DrawItemBg(g, item, itemRect, primaryRect, caretRect, isHover, isPressed, 4 * dpi, in p);

            var iconRect = new RectangleF(primaryRect.X + (primaryRect.Width - iconSize) / 2, itemRect.Y + 4 * dpi, iconSize, iconSize);
            item.IconRect = iconRect;
            DrawItemIcon(g, item, iconRect, dpi, in p);

            var textRect = new RectangleF(primaryRect.X, iconRect.Bottom + 2 * dpi, primaryRect.Width, textH);
            item.TextRect = textRect;
            if (!string.IsNullOrEmpty(item.Text))
            {
                Color textColor = item.Enabled ? (item.Checked ? p.Primary : p.Text) : p.TextQuaternary;
                g.String(item.Text, f_large!, textColor, textRect, s_c);
            }

            if (item.IsSplit) DrawCaretGlyph(g, item, caretRect, dpi, in p);

            x += itemW + pad;
        }

        /// <summary>Draws a column of up to 3 small items stacked vertically (Office pattern).</summary>
        void DrawSmallColumn(Canvas g, List<RibbonItem> items, int startIdx, int count, ref float x, float y, float areaH, float pad, float dpi, in Palette p)
        {
            if (count <= 0) return;

            float rowH = 22 * dpi;
            float iconSize = 16 * dpi;
            float hPad = 4 * dpi;

            // Compute max text width in this column so all rows share the same width
            float maxTextW = 0;
            for (int k = 0; k < count; k++)
            {
                var it = items[startIdx + k];
                if (!string.IsNullOrEmpty(it.Text))
                {
                    var sz = g.MeasureString(it.Text, f_small!);
                    if (sz.Width > maxTextW) maxTextW = sz.Width;
                }
            }

            float colW = iconSize + maxTextW + hPad * 2 + (maxTextW > 0 ? 4 * dpi : 0);
            float stackH = rowH * count;
            float yStart = y + (areaH - stackH) / 2;

            // Column widens to accommodate caret on any split item
            float caretW = 0;
            for (int k = 0; k < count; k++) if (items[startIdx + k].IsSplit) { caretW = 12 * dpi; break; }
            colW += caretW;

            for (int k = 0; k < count; k++)
            {
                var item = items[startIdx + k];
                float ry = yStart + rowH * k;
                var itemRect = new RectangleF(x, ry, colW, rowH);
                item.Rect = itemRect;

                float rowCaretW = item.IsSplit ? 12 * dpi : 0;
                var primaryRect = item.IsSplit
                    ? new RectangleF(itemRect.X, itemRect.Y, itemRect.Width - rowCaretW, itemRect.Height)
                    : itemRect;
                var caretRect = item.IsSplit
                    ? new RectangleF(itemRect.Right - rowCaretW, itemRect.Y, rowCaretW, itemRect.Height)
                    : RectangleF.Empty;
                item.CaretRect = caretRect;

                bool isHover = item == hoverItem;
                bool isPressed = item == pressedItem;
                DrawItemBg(g, item, itemRect, primaryRect, caretRect, isHover, isPressed, 3 * dpi, in p);

                var iconRect = new RectangleF(primaryRect.X + hPad, itemRect.Y + (rowH - iconSize) / 2, iconSize, iconSize);
                item.IconRect = iconRect;
                DrawItemIcon(g, item, iconRect, dpi, in p);

                if (!string.IsNullOrEmpty(item.Text))
                {
                    var textRect = new RectangleF(iconRect.Right + 4 * dpi, itemRect.Y, maxTextW + 2 * dpi, rowH);
                    item.TextRect = textRect;
                    Color textColor = item.Enabled ? (item.Checked ? p.Primary : p.Text) : p.TextQuaternary;
                    g.String(item.Text, f_small!, textColor, textRect, s_lb);
                }

                if (item.IsSplit) DrawCaretGlyph(g, item, caretRect, dpi, in p);
            }

            x += colW + pad;
        }

        /// <summary>Draws item background with optional split-between primary and caret regions.</summary>
        void DrawItemBg(Canvas g, RibbonItem item, RectangleF itemRect, RectangleF primaryRect, RectangleF caretRect, bool isHover, bool isPressed, float radius, in Palette p)
        {
            if (!item.Enabled) return;

            if (!item.IsSplit)
            {
                if (isHover || isPressed || item.Checked)
                {
                    Color bgColor = isPressed ? p.FillSecondary : (item.Checked ? p.PrimaryBg : p.HoverBg);
                    using var path = Rectangle.Round(itemRect).RoundPath(radius);
                    g.Fill(bgColor, path);
                }
                return;
            }

            bool primHover = isHover && !hoverCaret;
            bool primPressed = isPressed && !pressedCaret;
            bool crtHover = isHover && hoverCaret;
            bool crtPressed = isPressed && pressedCaret;

            if (primHover || primPressed || item.Checked)
            {
                Color bgColor = primPressed ? p.FillSecondary : (item.Checked ? p.PrimaryBg : p.HoverBg);
                using var path = Rectangle.Round(primaryRect).RoundPath(radius, TAlignRound.Left);
                g.Fill(bgColor, path);
            }
            if (crtHover || crtPressed)
            {
                Color bgColor = crtPressed ? p.FillSecondary : p.HoverBg;
                using var path = Rectangle.Round(caretRect).RoundPath(radius, TAlignRound.Right);
                g.Fill(bgColor, path);
            }
            // Subtle vertical divider between primary and caret (only visible when either side is active).
            if (isHover || isPressed)
            {
                float divX = caretRect.X;
                g.DrawLine(p.Split, 1, new PointF(divX, itemRect.Y + 3f), new PointF(divX, itemRect.Bottom - 3f));
            }
        }

        /// <summary>Renders a small down-caret glyph centered inside <paramref name="caretRect"/>.</summary>
        void DrawCaretGlyph(Canvas g, RibbonItem item, RectangleF caretRect, float dpi, in Palette p)
        {
            Color caretColor = item.Enabled ? p.Text : p.TextQuaternary;
            float gs = 8 * dpi;
            var glyphRect = Rectangle.Round(new RectangleF(
                caretRect.X + (caretRect.Width - gs) / 2,
                caretRect.Y + (caretRect.Height - gs) / 2,
                gs, gs));
            using var bmp = SvgExtend.GetImgExtend(SvgDb.IcoDown, glyphRect, caretColor);
            if (bmp != null) g.Image(bmp, glyphRect);
        }

        void DrawItemIcon(Canvas g, RibbonItem item, RectangleF iconRect, float dpi, in Palette p)
        {
            Color iconColor = item.Enabled ? (item.Checked ? p.Primary : p.Text) : p.TextQuaternary;

            if (!string.IsNullOrEmpty(item.IconSvg))
            {
                var svgRect = Rectangle.Round(iconRect);
                using (var bmp = SvgExtend.GetImgExtend(item.IconSvg, svgRect, iconColor))
                {
                    if (bmp != null) g.Image(bmp, svgRect);
                }
            }
            else if (item.Image != null)
            {
                g.Image(Rectangle.Round(iconRect), item.Image, TFit.Contain);
            }
        }

        #endregion

        #region Mouse interaction

        // Hover/press state is shared between the main ribbon and its collapsed-mode RibbonPopup
        // so that drawing routines (which live on Ribbon) read a single source of truth regardless
        // of which surface is active.
        internal RibbonItem? hoverItem;
        internal RibbonItem? pressedItem;
        internal bool hoverCaret;     // hover is on the caret half of a split-button
        internal bool pressedCaret;   // press was on the caret half

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var pt = new PointF(e.X, e.Y);

            // Tab hover
            bool tabChanged = false;
            bool anyTabHover = false;
            for (int i = 0; i < tabPages.Count; i++)
            {
                var page = tabPages[i];
                bool wasHover = page.TabHover;
                page.TabHover = page.TabRect.Contains(pt);
                if (page.TabHover) anyTabHover = true;
                if (wasHover != page.TabHover) tabChanged = true;
            }
            if (tabChanged)
            {
                SetCursor(anyTabHover);
                Invalidate();
            }

            // QAT hover (applies whether collapsed or not).
            RibbonItem? newHover = null;
            foreach (var qi in quickAccessItems)
            {
                if (qi.Visible && qi.Enabled && qi.Rect.Contains(pt)) { newHover = qi; break; }
            }

            var tab = SelectedTab;
            if (newHover == null && tab != null && !collapsed)
            {
                foreach (var group in tab.Groups)
                {
                    foreach (var item in group.Items)
                    {
                        if (item.Visible && item.Enabled && item.Rect.Contains(pt)) { newHover = item; break; }
                    }
                    if (newHover != null) break;
                }
            }

            bool newHoverCaret = newHover != null && newHover.IsSplit && newHover.CaretRect.Contains(pt);
            bool newHoverToggle = showCollapseToggle && !collapseToggleRect.IsEmpty && collapseToggleRect.Contains(pt);
            if (newHover != hoverItem || newHoverCaret != hoverCaret || newHoverToggle != hoverCollapseToggle)
            {
                hoverItem = newHover;
                hoverCaret = newHoverCaret;
                hoverCollapseToggle = newHoverToggle;
                SetCursor(newHover != null || anyTabHover || newHoverToggle);
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            bool changed = false;
            foreach (var page in tabPages)
            {
                if (page.TabHover) { page.TabHover = false; changed = true; }
            }
            if (hoverItem != null) { hoverItem = null; changed = true; }
            if (hoverCaret) { hoverCaret = false; changed = true; }
            if (hoverCollapseToggle) { hoverCollapseToggle = false; changed = true; }
            if (changed)
            {
                SetCursor(false);
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            var pt = new PointF(e.X, e.Y);

            // Collapse toggle chevron
            if (showCollapseToggle && !collapseToggleRect.IsEmpty && collapseToggleRect.Contains(pt))
            {
                Collapsed = !Collapsed;
                return;
            }

            // Tab click
            for (int i = 0; i < tabPages.Count; i++)
            {
                if (!tabPages[i].Visible || !tabPages[i].Enabled) continue;
                if (!tabPages[i].TabRect.Contains(pt)) continue;

                if (collapsed)
                {
                    // Re-click same tab while popup open → dismiss.
                    if (popup != null && i == selectedIndex)
                    {
                        ClosePopup();
                        Invalidate();
                    }
                    else
                    {
                        if (i != selectedIndex)
                        {
                            selectedIndex = i;
                            SelectedTabChanged?.Invoke(this, new IntEventArgs(i));
                            OnPropertyChanged(nameof(SelectedIndex));
                        }
                        OpenPopup();
                    }
                }
                else
                {
                    SelectedIndex = i;
                }
                return;
            }

            // Item press — QAT items work even when collapsed; ribbon body items only when expanded.
            if (hoverItem != null && hoverItem.Enabled)
            {
                bool isQat = quickAccessItems.Contains(hoverItem);
                if (isQat || !collapsed)
                {
                    pressedItem = hoverItem;
                    pressedCaret = hoverCaret;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (pressedItem != null)
            {
                var pt = new PointF(e.X, e.Y);
                bool wasInside = pressedItem.Rect.Contains(pt);

                if (wasInside && pressedItem.Enabled)
                {
                    // Split-button caret side fires DropDownOpening; primary side fires Click/Toggle.
                    if (pressedItem.IsSplit && pressedCaret && pressedItem.CaretRect.Contains(pt))
                    {
                        pressedItem.RaiseDropDownOpening();
                    }
                    else
                    {
                        if (pressedItem.Toggle) pressedItem.Checked = !pressedItem.Checked;
                        pressedItem.RaiseClick();
                        ItemClick?.Invoke(this, new RibbonItemEventArgs(pressedItem));
                    }
                }

                pressedItem = null;
                pressedCaret = false;
                Invalidate();
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            // Double-click on tab strip toggles collapsed
            float dpi = Dpi;
            int _tabH = (int)(tabHeight * dpi);
            if (e.Y < _tabH)
            {
                Collapsed = !Collapsed;
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            // Let arrow/Enter/Space/Escape reach OnKeyDown instead of being routed to the form.
            switch (keyData)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Enter:
                case Keys.Space:
                case Keys.Escape:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled) return;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    for (int i = selectedIndex - 1; i >= 0; i--)
                    {
                        if (tabPages[i].Visible && tabPages[i].Enabled) { SelectedIndex = i; e.Handled = true; return; }
                    }
                    break;
                case Keys.Right:
                    for (int i = selectedIndex + 1; i < tabPages.Count; i++)
                    {
                        if (tabPages[i].Visible && tabPages[i].Enabled) { SelectedIndex = i; e.Handled = true; return; }
                    }
                    break;
                case Keys.Enter:
                case Keys.Space:
                    if (hoverItem != null && hoverItem.Enabled)
                    {
                        if (hoverItem.Toggle) hoverItem.Checked = !hoverItem.Checked;
                        hoverItem.RaiseClick();
                        ItemClick?.Invoke(this, new RibbonItemEventArgs(hoverItem));
                        e.Handled = true;
                    }
                    break;
                case Keys.Escape:
                    if (popup != null) { ClosePopup(); e.Handled = true; }
                    break;
            }
        }

        #endregion

        #region Collapsed popup

        RibbonPopup? popup;

        void OpenPopup()
        {
            var tab = SelectedTab;
            if (tab == null) return;
            ClosePopup();

            popup = new RibbonPopup(this, tab);
            popup.FormClosed += Popup_Closed;
            popup.ItemActivated += Popup_ItemActivated;
            popup.ShowBelow(this);
            Invalidate();
        }

        internal void ClosePopup()
        {
            if (popup == null) return;
            var p = popup;
            popup = null;
            p.FormClosed -= Popup_Closed;
            p.ItemActivated -= Popup_ItemActivated;
            if (!p.IsDisposed)
            {
                try { p.Close(); } catch { }
            }
            Invalidate();
        }

        void Popup_Closed(object? sender, FormClosedEventArgs e)
        {
            if (popup == sender) popup = null;
            Invalidate();
        }

        void Popup_ItemActivated(RibbonItem item)
        {
            if (item.Toggle) item.Checked = !item.Checked;
            item.RaiseClick();
            ItemClick?.Invoke(this, new RibbonItemEventArgs(item));
            ClosePopup();
        }

        #endregion

        #region Layout

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <summary>Gets the preferred height (QAT + tab + content) based on collapsed state and QAT presence.</summary>
        public int GetPreferredHeight()
        {
            float dpi = Dpi;
            int qat = quickAccessItems.Count > 0 ? (int)(quickAccessHeight * dpi) : 0;
            if (collapsed) return qat + (int)(tabHeight * dpi);
            return qat + (int)((tabHeight + contentHeight) * dpi);
        }

        #endregion
    }
}
