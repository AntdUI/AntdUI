// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// IDockContent — contract for anything hosted in a <see cref="DockPanel"/>.
    /// </summary>
    public interface IDockContent
    {
        /// <summary>Stable identifier for persistence (required by <see cref="DockPersistor"/>).</summary>
        string PersistId { get; set; }

        /// <summary>Title shown on the tab / title bar.</summary>
        string DockTitle { get; set; }

        /// <summary>Optional SVG icon key (see <see cref="SvgDb"/>).</summary>
        string? DockIconSvg { get; set; }

        /// <summary>Current dock state.</summary>
        DockState DockState { get; set; }

        /// <summary>The hosted control.</summary>
        Control ContentControl { get; }

        /// <summary>Whether this content exposes a close button.</summary>
        bool CanClose { get; set; }

        /// <summary>Whether this content may be dragged out into a <see cref="DockFloatWindow"/>.</summary>
        bool CanFloat { get; set; }

        /// <summary>Whether this content may be auto-hidden onto an edge strip.</summary>
        bool CanAutoHide { get; set; }

        /// <summary>When true, closing the content hides it (kept in memory for later <c>ShowContent</c>) instead of removing it.</summary>
        bool HideOnClose { get; set; }

        /// <summary>Owning pane (assigned internally by <see cref="DockPanel"/>). Null when floating or unattached.</summary>
        DockPane? OwnerPane { get; set; }

        /// <summary>Fires when DockState changes.</summary>
        event EventHandler? DockStateChanged;
    }

    /// <summary>
    /// Default <see cref="IDockContent"/> implementation wrapping any WinForms <see cref="Control"/>.
    /// </summary>
    [ToolboxItem(false)]
    public class DockContent : IDockContent
    {
        public DockContent(Control control, string title)
        {
            ContentControl = control;
            dockTitle = title;
            persistId = title;
        }

        public DockContent(Control control, string title, string? iconSvg) : this(control, title)
        {
            DockIconSvg = iconSvg;
        }

        public DockContent(Control control, string persistId, string title, string? iconSvg)
        {
            ContentControl = control;
            dockTitle = title;
            this.persistId = persistId;
            DockIconSvg = iconSvg;
        }

        string persistId;
        public string PersistId
        {
            get => persistId;
            set => persistId = value ?? string.Empty;
        }

        string dockTitle;
        public string DockTitle
        {
            get => dockTitle;
            set
            {
                if (dockTitle == value) return;
                dockTitle = value ?? string.Empty;
                DockStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string? DockIconSvg { get; set; }

        DockState dockState = DockState.Docked;
        public DockState DockState
        {
            get => dockState;
            set
            {
                if (dockState == value) return;
                dockState = value;
                DockStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Control ContentControl { get; }
        public bool CanClose { get; set; } = true;
        public bool CanFloat { get; set; } = true;
        public bool CanAutoHide { get; set; } = true;

        /// <summary>When true, the close button on a tab/titlebar hides the content instead of removing it.
        /// Callers can later restore it via <see cref="DockPanel.ShowContent(IDockContent, DockPosition)"/>.</summary>
        public bool HideOnClose { get; set; } = false;

        [Browsable(false)]
        public DockPane? OwnerPane { get; set; }

        public event EventHandler? DockStateChanged;
    }

    /// <summary>Event args for a dock-content event.</summary>
    public class DockContentEventArgs : EventArgs
    {
        public IDockContent Content { get; }
        public DockContentEventArgs(IDockContent content) { Content = content; }
    }

    /// <summary>Cancellable event args: set <see cref="CancelEventArgs.Cancel"/> to true to prevent the close/remove.</summary>
    public class DockContentCancelEventArgs : CancelEventArgs
    {
        public IDockContent Content { get; }
        public DockContentCancelEventArgs(IDockContent content) { Content = content; }
    }

    public delegate void DockContentEventHandler(object sender, DockContentEventArgs e);

    public delegate void DockContentCancelEventHandler(object sender, DockContentCancelEventArgs e);
}
