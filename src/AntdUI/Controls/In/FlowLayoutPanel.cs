// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI.In
{
    [ToolboxItem(true)]
    [Designer(typeof(IControlDesigner))]
    public class FlowLayoutPanel : IControl
    {
        #region 属性

        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        [Description("是否显示滚动条"), Category("外观"), DefaultValue(false)]
        public bool AutoScroll
        {
            get => Panel.AutoScroll;
            set
            {
                if (Panel.AutoScroll == value) return;
                Panel.AutoScroll = value;
                if (IsHandleCreated) InitScroll();
                OnPropertyChanged(nameof(AutoScroll));
            }
        }

        #region 原生

        /// <summary>
        /// 指定控件布局的方向
        /// </summary>
        [Description("指定控件布局的方向"), Category("布局"), DefaultValue(FlowDirection.LeftToRight)]
        public FlowDirection FlowDirection
        {
            get => Panel.FlowDirection;
            set => Panel.FlowDirection = value;
        }

        /// <summary>
        /// 指示在控件边界是将内容换行还是将内容剪裁
        /// </summary>
        [Description("指示在控件边界是将内容换行还是将内容剪裁"), Category("布局"), DefaultValue(true)]
        public bool WrapContents
        {
            get => Panel.WrapContents;
            set => Panel.WrapContents = value;
        }

        public bool GetFlowBreak(Control control) => Panel.GetFlowBreak(control);

        public void SetFlowBreak(Control control, bool value) => Panel.SetFlowBreak(control, value);

        #endregion

        #region 为空

        /// <summary>
        /// 是否显示空样式
        /// </summary>
        [Description("是否显示空样式"), Category("外观"), DefaultValue(false)]
        public bool Empty { get; set; }

        /// <summary>
        /// 数据为空显示文字
        /// </summary>
        [Description("数据为空显示文字"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? EmptyText { get; set; }

        /// <summary>
        /// 数据为空显示图片
        /// </summary>
        [Description("数据为空显示图片"), Category("外观"), DefaultValue(null)]
        public Image? EmptyImage { get; set; }

        #endregion

        #endregion

        #region 核心

        private FlowLayoutPanelCore Panel = new FlowLayoutPanelCore
        {
            Name = "__IN__",
            Dock = DockStyle.Fill
        };
        public XScrollBar? XScroll;
        public YScrollBar? YScroll;

        public FlowLayoutPanel()
        {
            base.Controls.Add(Panel);
        }

        #region 控件

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control == null) return;
            if (e.Control.Name == "__IN__" || e.Control.Name == "__BARX__" || e.Control.Name == "__BARY__")
            {
                base.OnControlAdded(e);
                return;
            }
            if (DesignMode)
            {
                base.OnControlAdded(e);
                return;
            }
            Add(e.Control);
        }

        public void Remove(Control control) => Panel.Controls.Remove(control);
        public void RemoveAt(int index) => Panel.Controls.RemoveAt(index);

        public void Add(Control control) => Panel.Controls.Add(control);

        public void Clear() => Panel.Controls.Clear();

        [Browsable(false)]
        public new ControlCollection Controls => Panel.Controls;

        [Browsable(false)]
        public Control? this[string name] => Panel.Controls.ContainsKey(name) ? Panel.Controls[name] : null;

        [Browsable(false)]
        public Control? this[int index] => (index >= 0 && index < Panel.Controls.Count) ? Panel.Controls[index] : null;
        public void ScrollControlIntoView(Control? activeControl) => Panel?.ScrollControlIntoView(activeControl);

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Panel.LoadScroll = ScrollInfo;
            Panel.DrawEmpty = g =>
            {
                if (Empty) g.High().PaintEmpty(ClientRectangle, Font, Colour.Text.Get(nameof(FlowLayoutPanel)), EmptyText, EmptyImage);
            };
            InitScroll();
            IOnSizeChanged();
        }

        void InitScroll()
        {
            if (Panel.AutoScroll)
            {
                if (XScroll == null)
                {
                    int h = SystemInformation.HorizontalScrollBarHeight, w = Panel.Width;
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
                    int w = SystemInformation.VerticalScrollBarWidth, h = Panel.Height;
                    YScroll = new YScrollBar
                    {
                        Name = "__BARY__",
                        Dock = DockStyle.Right,
                        MinimumSize = new Size(w, 0),
                        Size = new Size(w, h),
                        Radius = 0
                    };
                    YScroll.ValueChanged += ScrollY_ValueChanged;
                    base.Controls.Add(YScroll);
                }
                Panel.SendToBack();
            }
            else
            {
                if (YScroll != null)
                {
                    YScroll.ValueChanged -= ScrollY_ValueChanged;
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

        private void ScrollX_ValueChanged(object? sender, IntEventArgs e) => XScroll?.SetValue(Panel.HorizontalScroll);
        private void ScrollY_ValueChanged(object? sender, IntEventArgs e) => YScroll?.SetValue(Panel.VerticalScroll);

        private void ScrollInfo()
        {
            if (YScroll == null || XScroll == null) return;
            YScroll.LoadValue(Panel.VerticalScroll);
            XScroll.LoadValue(Panel.HorizontalScroll);
        }

        #endregion

        #region 原生

        protected override void OnContextMenuStripChanged(EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
            if (Panel != null) Panel.ContextMenuStrip = ContextMenuStrip;
        }

        #endregion

        #region 布局

        internal class FlowLayoutPanelCore : System.Windows.Forms.FlowLayoutPanel
        {
            public FlowLayoutPanelCore()
            {
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.UserPaint, true);
                UpdateStyles();
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                LoadScroll?.Invoke();
                base.OnSizeChanged(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                LoadScroll?.Invoke();
                base.OnPaint(e);
                if (Controls == null || Controls.Count == 0) DrawEmpty?.Invoke(e.Graphics);
            }

            protected override void OnScroll(ScrollEventArgs se)
            {
                LoadScroll?.Invoke();
                base.OnScroll(se);
            }

            public Action? LoadScroll;
            public Action<Graphics>? DrawEmpty;
        }

        #endregion
    }
}