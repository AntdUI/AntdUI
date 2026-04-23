// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// RibbonItemGroup — A named group of items within a RibbonTabPage, rendered with a title footer and vertical separator.
    /// </summary>
    [ToolboxItem(false)]
    public sealed class RibbonItemGroup
    {
        /// <summary>Group title shown below items.</summary>
        [Description("Group title"), Category("Appearance"), DefaultValue(null)]
        public string? Text { get; set; }

        /// <summary>Items within this group.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Items"), Category("Data")]
        public List<RibbonItem> Items { get; set; } = new List<RibbonItem>();

        /// <summary>Whether this group is visible.</summary>
        [Description("Visible"), Category("Behavior"), DefaultValue(true)]
        public bool Visible { get; set; } = true;

        /// <summary>Whether to show the vertical separator after this group.</summary>
        [Description("Show separator"), Category("Appearance"), DefaultValue(true)]
        public bool ShowSeparator { get; set; } = true;

        /// <summary>User-defined tag.</summary>
        [Description("Tag"), Category("Data"), DefaultValue(null)]
        public object? Tag { get; set; }

        // -- Layout state (internal, computed by parent) --
        internal RectangleF Rect;
        internal RectangleF TitleRect;
        internal RectangleF SeparatorRect;

        public RibbonItemGroup() { }

        public RibbonItemGroup(string text, params RibbonItem[] items)
        {
            Text = text;
            Items.AddRange(items);
        }
    }
}
