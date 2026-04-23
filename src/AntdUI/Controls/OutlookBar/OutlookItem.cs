// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// OutlookItem — A single navigation item within an OutlookPanel.
    /// </summary>
    [ToolboxItem(false)]
    public sealed class OutlookItem
    {
        /// <summary>Display text.</summary>
        [Description("Text"), Category("Appearance"), DefaultValue(null)]
        public string? Text { get; set; }

        /// <summary>SVG icon name or raw SVG content.</summary>
        [Description("SVG icon"), Category("Appearance"), DefaultValue(null)]
        public string? IconSvg { get; set; }

        /// <summary>Bitmap image (used when IconSvg is null).</summary>
        [Description("Image"), Category("Appearance"), DefaultValue(null)]
        public Image? Image { get; set; }

        /// <summary>Whether this item is enabled.</summary>
        [Description("Enabled"), Category("Behavior"), DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <summary>User-defined tag.</summary>
        [Description("Tag"), Category("Data"), DefaultValue(null)]
        public object? Tag { get; set; }

        /// <summary>Tooltip text.</summary>
        [Description("Tooltip"), Category("Behavior"), DefaultValue(null)]
        public string? Tooltip { get; set; }

        /// <summary>Optional badge text rendered as a pill at the right edge (e.g. "42" unread count, "NEW").
        /// Null or empty = no badge.</summary>
        [Description("Badge text"), Category("Appearance"), DefaultValue(null)]
        public string? Badge { get; set; }

        /// <summary>Fires when clicked.</summary>
        public event EventHandler? Click;

        internal void RaiseClick() => Click?.Invoke(this, EventArgs.Empty);

        // -- Layout state --
        internal RectangleF Rect;
        internal RectangleF IconRect;
        internal RectangleF TextRect;

        public OutlookItem() { }

        public OutlookItem(string text, string? iconSvg = null, EventHandler? click = null)
        {
            Text = text;
            IconSvg = iconSvg;
            if (click != null) Click += click;
        }
    }

    /// <summary>
    /// Event args for outlook item events.
    /// </summary>
    public class OutlookItemEventArgs : EventArgs
    {
        public OutlookItem Item { get; }
        public OutlookItemEventArgs(OutlookItem item) => Item = item;
    }

    public delegate void OutlookItemEventHandler(object sender, OutlookItemEventArgs e);
}
