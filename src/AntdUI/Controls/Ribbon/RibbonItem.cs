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
    /// RibbonItem — A single clickable item within a RibbonItemGroup.
    /// </summary>
    [ToolboxItem(false)]
    public sealed class RibbonItem
    {
        /// <summary>Text label for the item.</summary>
        [Description("Text label"), Category("Appearance"), DefaultValue(null)]
        public string? Text { get; set; }

        /// <summary>SVG icon name or raw SVG content.</summary>
        [Description("SVG icon name or content"), Category("Appearance"), DefaultValue(null)]
        public string? IconSvg { get; set; }

        /// <summary>Bitmap image icon (used if IconSvg is null).</summary>
        [Description("Image icon"), Category("Appearance"), DefaultValue(null)]
        public Image? Image { get; set; }

        /// <summary>Display size: Large (icon above text) or Small (inline).</summary>
        [Description("Display size"), Category("Appearance"), DefaultValue(RibbonItemSize.Large)]
        public RibbonItemSize Size { get; set; } = RibbonItemSize.Large;

        /// <summary>Whether the item is enabled.</summary>
        [Description("Enabled"), Category("Behavior"), DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <summary>Whether the item is visible.</summary>
        [Description("Visible"), Category("Behavior"), DefaultValue(true)]
        public bool Visible { get; set; } = true;

        /// <summary>User-defined tag data.</summary>
        [Description("Tag"), Category("Data"), DefaultValue(null)]
        public object? Tag { get; set; }

        /// <summary>Tooltip text.</summary>
        [Description("Tooltip"), Category("Behavior"), DefaultValue(null)]
        public string? Tooltip { get; set; }

        /// <summary>Whether this item acts as a toggle button.</summary>
        [Description("Toggle mode"), Category("Behavior"), DefaultValue(false)]
        public bool Toggle { get; set; }

        /// <summary>Checked state (when Toggle is true).</summary>
        [Description("Checked"), Category("Data"), DefaultValue(false)]
        public bool Checked { get; set; }

        /// <summary>Renders this item as a split button — primary click fires <see cref="Click"/>,
        /// a separate caret area at the right fires <see cref="DropDownOpening"/> so the consumer can
        /// attach any dropdown (ContextMenuStrip, AntdUI Menu, colour picker, etc.).</summary>
        [Description("Split button (primary + caret)"), Category("Behavior"), DefaultValue(false)]
        public bool IsSplit { get; set; }

        /// <summary>Fires when the item is clicked.</summary>
        public event EventHandler? Click;

        /// <summary>Fires when the caret part of a split button is clicked. The consumer should show
        /// a dropdown anchored to <see cref="CaretRect"/> (screen coords via <c>control.RectangleToScreen</c>).</summary>
        public event EventHandler? DropDownOpening;

        internal void RaiseClick() => Click?.Invoke(this, EventArgs.Empty);
        internal void RaiseDropDownOpening() => DropDownOpening?.Invoke(this, EventArgs.Empty);

        // Layout state, computed by the owning Ribbon during paint; hover/press state is tracked by the Ribbon
        // itself (single `hoverItem` / `pressedItem` pointer) rather than mirrored onto every item.
        internal RectangleF Rect;
        internal RectangleF IconRect;
        internal RectangleF TextRect;
        /// <summary>Caret hit-rect for split-button items (empty when <see cref="IsSplit"/> is false).
        /// Public so <see cref="DropDownOpening"/> consumers can anchor their menu — use
        /// <c>control.RectangleToScreen(Rectangle.Round(item.CaretRect))</c>.</summary>
        public RectangleF CaretRect { get; internal set; }

        public RibbonItem() { }

        public RibbonItem(string text, string? iconSvg = null, EventHandler? click = null)
        {
            Text = text;
            IconSvg = iconSvg;
            if (click != null) Click += click;
        }
    }

    /// <summary>
    /// Size mode for a RibbonItem.
    /// </summary>
    public enum RibbonItemSize
    {
        /// <summary>Large icon above text.</summary>
        Large,
        /// <summary>Small icon inline with text.</summary>
        Small
    }

    /// <summary>
    /// Event args for ribbon item events.
    /// </summary>
    public class RibbonItemEventArgs : EventArgs
    {
        public RibbonItem Item { get; }
        public RibbonItemEventArgs(RibbonItem item) => Item = item;
    }

    public delegate void RibbonItemEventHandler(object sender, RibbonItemEventArgs e);
}
