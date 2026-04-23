// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.ComponentModel;

namespace AntdUI
{
    /// <summary>
    /// RibbonTabPage — Represents a single tab within the Ribbon, containing groups of items.
    /// </summary>
    [ToolboxItem(false)]
    public sealed class RibbonTabPage
    {
        /// <summary>Tab header text.</summary>
        [Description("Tab text"), Category("Appearance"), DefaultValue(null)]
        public string? Text { get; set; }

        /// <summary>Groups within this tab page.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Groups"), Category("Data")]
        public List<RibbonItemGroup> Groups { get; set; } = new List<RibbonItemGroup>();

        /// <summary>Whether this tab is visible.</summary>
        [Description("Visible"), Category("Behavior"), DefaultValue(true)]
        public bool Visible { get; set; } = true;

        /// <summary>Whether this tab is enabled.</summary>
        [Description("Enabled"), Category("Behavior"), DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <summary>User-defined tag.</summary>
        [Description("Tag"), Category("Data"), DefaultValue(null)]
        public object? Tag { get; set; }

        /// <summary>Marks this tab as contextual — shown only in specific situations (e.g. "Picture Tools"
        /// visible only when a picture is selected). Contextual tabs paint a coloured accent strip above
        /// their label and are typically toggled via <see cref="Visible"/>.</summary>
        [Description("Contextual tab (paints accent strip)"), Category("Appearance"), DefaultValue(false)]
        public bool IsContextual { get; set; }

        /// <summary>Accent colour for a contextual tab's strip + selected-state tint. Defaults to the
        /// theme <c>Colour.Primary</c> token when null.</summary>
        [Description("Contextual accent colour"), Category("Appearance"), DefaultValue(null)]
        public System.Drawing.Color? ContextColor { get; set; }

        // -- Layout state (internal) --
        internal System.Drawing.RectangleF TabRect;
        internal bool TabHover;

        public RibbonTabPage() { }

        public RibbonTabPage(string text, params RibbonItemGroup[] groups)
        {
            Text = text;
            Groups.AddRange(groups);
        }
    }
}
