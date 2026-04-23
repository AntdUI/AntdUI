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
    /// DockForm — optional <see cref="BorderlessForm"/>-based wrapper that adapts a Form into
    /// an <see cref="IDockContent"/>. The form's root <see cref="UserControl"/> is exposed as
    /// <see cref="ContentControl"/>; close/float/autohide are delegated to the owning DockPanel.
    /// </summary>
    [ToolboxItem(false)]
    public class DockForm : BorderlessForm, IDockContent
    {
        public DockForm()
        {
            // Host panel that becomes the IDockContent.ContentControl
            host = new UserControl();
            host.Dock = DockStyle.Fill;
            Controls.Add(host);
            persistId = Name;
        }

        readonly UserControl host;

        string persistId;
        [Description("Stable persistence id"), Category("Docking"), DefaultValue("")]
        public string PersistId
        {
            get { return persistId; }
            set { persistId = value ?? string.Empty; }
        }

        string dockTitle = "";
        [Description("Dock title"), Category("Docking"), DefaultValue("")]
        public string DockTitle
        {
            get { return dockTitle; }
            set
            {
                if (dockTitle == value) return;
                dockTitle = value ?? string.Empty;
                Text = dockTitle;
                if (DockStateChanged != null) DockStateChanged.Invoke(this, EventArgs.Empty);
            }
        }

        [Description("Dock icon (SVG key)"), Category("Docking"), DefaultValue(null)]
        public string? DockIconSvg { get; set; }

        DockState dockState = DockState.Docked;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DockState DockState
        {
            get { return dockState; }
            set
            {
                if (dockState == value) return;
                dockState = value;
                if (DockStateChanged != null) DockStateChanged.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>Host panel for DockForm's child controls — serves as <see cref="IDockContent.ContentControl"/>.</summary>
        [Browsable(false)]
        public Control ContentControl { get { return host; } }

        [Description("Allow close"), Category("Docking"), DefaultValue(true)]
        public bool CanClose { get; set; } = true;

        [Description("Allow float"), Category("Docking"), DefaultValue(true)]
        public bool CanFloat { get; set; } = true;

        [Description("Allow auto-hide"), Category("Docking"), DefaultValue(true)]
        public bool CanAutoHide { get; set; } = true;

        /// <summary>Allow docking (if false, the DockForm behaves as a plain borderless dialog).</summary>
        [Description("Allow dock"), Category("Docking"), DefaultValue(true)]
        public bool AllowDock { get; set; } = true;

        [Description("Hide rather than close when the close button is pressed"), Category("Docking"), DefaultValue(false)]
        public bool HideOnClose { get; set; } = false;

        /// <summary>Preferred dock zone when first added.</summary>
        [Description("Preferred dock zone"), Category("Docking"), DefaultValue(DockPosition.Right)]
        public DockPosition DockZone { get; set; } = DockPosition.Right;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DockPane? OwnerPane { get; set; }

        public event EventHandler? DockStateChanged;

        /// <summary>Attach this DockForm as content of the given DockPanel. Reparents child host.</summary>
        public void AttachTo(DockPanel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));
            panel.AddContent(this, DockZone);
        }
    }
}
