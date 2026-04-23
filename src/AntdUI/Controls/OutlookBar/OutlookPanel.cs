// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// OutlookPanel — A named navigation panel containing child items and an optional content area.
    /// Analogous to legacy ExNavigationPanel.
    /// </summary>
    [ToolboxItem(false)]
    public class OutlookPanel
    {
        /// <summary>Panel header text.</summary>
        [Description("Header text"), Category("Appearance"), DefaultValue(null)]
        public string? Text { get; set; }

        /// <summary>SVG icon for the panel header / footer.</summary>
        [Description("SVG icon"), Category("Appearance"), DefaultValue(null)]
        public string? IconSvg { get; set; }

        /// <summary>Image icon for the panel header / footer.</summary>
        [Description("Image"), Category("Appearance"), DefaultValue(null)]
        public System.Drawing.Image? Image { get; set; }

        /// <summary>Navigation items within this panel (shown in the body when selected).</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Items"), Category("Data")]
        public List<OutlookItem> Items { get; set; } = new List<OutlookItem>();

        /// <summary>Optional hosted content control (shown instead of items when set).</summary>
        [Description("Content control"), Category("Data"), DefaultValue(null)]
        public Control? ContentControl { get; set; }

        /// <summary>Whether this panel is visible.</summary>
        [Description("Visible"), Category("Behavior"), DefaultValue(true)]
        public bool Visible { get; set; } = true;

        /// <summary>Whether this panel is enabled.</summary>
        [Description("Enabled"), Category("Behavior"), DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// When true, this panel is always shown as a footer icon rather than a full-width header button.
        /// Panels that cannot fit in the header area are automatically demoted to the footer as well.
        /// </summary>
        [Description("Pin panel to footer icon strip"), Category("Behavior"), DefaultValue(false)]
        public bool InFooter { get; set; } = false;

        /// <summary>User-defined tag.</summary>
        [Description("Tag"), Category("Data"), DefaultValue(null)]
        public object? Tag { get; set; }

        /// <summary>Tooltip shown on the panel header / footer icon.</summary>
        [Description("Tooltip"), Category("Behavior"), DefaultValue(null)]
        public string? Tooltip { get; set; }

        /// <summary>Optional badge text rendered on the header (e.g. "12" unread mail count).
        /// Null or empty = no badge. Hidden in the collapsed icon-only rail.</summary>
        [Description("Badge text"), Category("Appearance"), DefaultValue(null)]
        public string? Badge { get; set; }

        // -- Layout state --
        internal System.Drawing.RectangleF HeaderRect;
        internal System.Drawing.RectangleF FooterRect;
        internal bool HeaderHover;
        internal bool FooterHover;

        public OutlookPanel() { }

        public OutlookPanel(string text, string? iconSvg = null, params OutlookItem[] items)
        {
            Text = text;
            IconSvg = iconSvg;
            Items.AddRange(items);
        }
    }
}
