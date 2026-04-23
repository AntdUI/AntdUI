// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    /// <summary>
    /// Where a DockPane sits inside the DockPanel.
    /// </summary>
    public enum DockPosition
    {
        /// <summary>Not placed.</summary>
        None,
        /// <summary>Docked to the left edge.</summary>
        Left,
        /// <summary>Docked to the right edge.</summary>
        Right,
        /// <summary>Docked to the top edge.</summary>
        Top,
        /// <summary>Docked to the bottom edge.</summary>
        Bottom,
        /// <summary>Fills the remaining center space.</summary>
        Fill,
        /// <summary>Collapsed into an auto-hide edge strip.</summary>
        AutoHide
    }

    /// <summary>
    /// Visible state of a docked content.
    /// </summary>
    public enum DockState
    {
        /// <summary>Normal docked state.</summary>
        Docked,
        /// <summary>Floating in its own window.</summary>
        Float,
        /// <summary>Auto-hidden behind a tab strip.</summary>
        AutoHide,
        /// <summary>Hidden entirely.</summary>
        Hidden
    }

    /// <summary>
    /// Hit-zone returned by the floating navigator during a drag.
    /// Outer zones dock against the DockPanel edge; inner zones split (or tabify) the hit pane.
    /// </summary>
    public enum DockZone
    {
        /// <summary>No zone (cancel dock).</summary>
        None,
        /// <summary>Outer left: dock to the left edge of the DockPanel.</summary>
        OuterLeft,
        /// <summary>Outer right: dock to the right edge of the DockPanel.</summary>
        OuterRight,
        /// <summary>Outer top: dock to the top edge of the DockPanel.</summary>
        OuterTop,
        /// <summary>Outer bottom: dock to the bottom edge of the DockPanel.</summary>
        OuterBottom,
        /// <summary>Inner left: split the hit pane so the new content takes its left half.</summary>
        InnerLeft,
        /// <summary>Inner right: split the hit pane so the new content takes its right half.</summary>
        InnerRight,
        /// <summary>Inner top: split the hit pane so the new content takes its top half.</summary>
        InnerTop,
        /// <summary>Inner bottom: split the hit pane so the new content takes its bottom half.</summary>
        InnerBottom,
        /// <summary>Center: tabify into the hit pane.</summary>
        Center,

        // Back-compat aliases (legacy API used simple Left/Right/Top/Bottom → treat as outer).
        /// <summary>Legacy alias for <see cref="OuterLeft"/>.</summary>
        Left = OuterLeft,
        /// <summary>Legacy alias for <see cref="OuterRight"/>.</summary>
        Right = OuterRight,
        /// <summary>Legacy alias for <see cref="OuterTop"/>.</summary>
        Top = OuterTop,
        /// <summary>Legacy alias for <see cref="OuterBottom"/>.</summary>
        Bottom = OuterBottom
    }
}
