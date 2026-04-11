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

namespace AntdUI.In
{
    [ToolboxItem(true)]
    [Designer(typeof(IControlDesigner))]
    public class Panel : IControl
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            InitScroll();
        }

        /// <summary>
        /// 显示区域
        /// </summary>
        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Padding);

        #region 滚动条

        bool autoscroll = false;
        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        [Description("是否显示滚动条"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool AutoScroll
        {
            get => autoscroll;
            set
            {
                if (autoscroll == value) return;
                autoscroll = value;
                if (IsHandleCreated) InitScroll();
                OnPropertyChanged(nameof(AutoScroll));
            }
        }

        private AntdUI.Panel.PanelCore? IPanel;
        public XScrollBar? XScroll;
        public YScrollBar? YScroll;

        #region 滚动条

        void InitScroll()
        {
            if (autoscroll)
            {
                if (DesignMode) return;
                if (IPanel == null)
                {
                    IPanel = new AntdUI.Panel.PanelCore(ScrollInfo)
                    {
                        Name = "__IN__",
                        Dock = DockStyle.Fill,
                        AutoScroll = true,
                        Bounds = DisplayRectangle
                    };
                    var controls = new List<Control>(IPanel.Controls.Count);
                    foreach (Control it in base.Controls)
                    {
                        if (it.Name == "__IN__" || it.Name == "__BARX__" || it.Name == "__BARY__") continue;
                        controls.Add(it);
                    }
                    if (controls.Count > 0) IPanel.Controls.AddRange(controls.ToArray());
                    base.Controls.Add(IPanel);
                }
                if (XScroll == null)
                {
                    int h = SystemInformation.HorizontalScrollBarHeight, w = IPanel.Width;
                    XScroll = new XScrollBar
                    {
                        Name = "__BARX__",
                        Dock = DockStyle.Bottom,
                        MinimumSize = new Size(0, h),
                        Size = new Size(w, h),
                        Radius = 0
                    };
                    XScroll.ValueChanged += ScrollX_ValueChanged;
                    base.Controls.Add(XScroll);
                }
                if (YScroll == null)
                {
                    int w = SystemInformation.VerticalScrollBarWidth, h = IPanel.Height;
                    YScroll = new YScrollBar
                    {
                        Name = "__BARY__",
                        Dock = DockStyle.Right,
                        MinimumSize = new Size(w, 0),
                        Size = new Size(w, h),
                        Radius = 0
                    };
                    YScroll.ShowChanged += ScrollY_ShowChanged;
                    YScroll.ValueChanged += ScrollY_ValueChanged;
                    base.Controls.Add(YScroll);
                }
            }
            else
            {
                if (IPanel != null)
                {
                    if (IPanel.Controls.Count > 0)
                    {
                        var controls = new List<Control>(IPanel.Controls.Count);
                        foreach (Control it in IPanel.Controls) controls.Add(it);
                        base.Controls.AddRange(controls.ToArray());
                    }
                    IPanel?.Dispose();
                    IPanel = null;
                }
                if (YScroll != null)
                {
                    YScroll.ValueChanged -= ScrollY_ValueChanged;
                    YScroll.ShowChanged -= ScrollY_ShowChanged;
                    YScroll.Dispose();
                    YScroll = null;
                }
                if (XScroll != null)
                {
                    XScroll.ValueChanged -= ScrollX_ValueChanged;
                    XScroll.Dispose();
                    XScroll = null;
                }
            }
        }

        private void ScrollY_ShowChanged(object? sender, BoolEventArgs e)
        {
            if (XScroll == null || YScroll == null) return;
            XScroll.RadiusRB = !e.Value;
        }
        private void ScrollX_ValueChanged(object? sender, IntEventArgs e)
        {
            if (IPanel == null || XScroll == null) return;
            XScroll.SetValue(IPanel.HorizontalScroll);
        }
        private void ScrollY_ValueChanged(object? sender, IntEventArgs e)
        {
            if (IPanel == null || YScroll == null) return;
            YScroll.SetValue(IPanel.VerticalScroll);
        }

        private void ScrollInfo()
        {
            if (IPanel == null || YScroll == null || XScroll == null) return;
            YScroll.LoadValue(IPanel.VerticalScroll);
            XScroll.LoadValue(IPanel.HorizontalScroll);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control == null) return;
            if (IPanel == null || e.Control.Name == "__IN__" || e.Control.Name == "__BARX__" || e.Control.Name == "__BARY__")
            {
                base.OnControlAdded(e);
                return;
            }
            if (DesignMode)
            {
                base.OnControlAdded(e);
                return;
            }
            IPanel.Controls.Add(e.Control);
        }

        public void Add(Control control) => Controls.Add(control);

        [Browsable(false)]
        public new ControlCollection Controls
        {
            get
            {
                if (IPanel == null) return base.Controls;
                return IPanel.Controls;
            }
        }

        public void ScrollControlIntoView(Control? activeControl) => IPanel?.ScrollControlIntoView(activeControl);

        protected override void OnContextMenuStripChanged(EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
            if (IPanel != null) IPanel.ContextMenuStrip = ContextMenuStrip;
        }

        #endregion

        #endregion
    }
}