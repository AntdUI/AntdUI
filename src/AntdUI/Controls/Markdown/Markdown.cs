// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Markdown 渲染控件
    /// </summary>
    /// <remarks>支持基础样式、表格、引用、列表、代码（含 Diff）高亮、分隔线、粗体/删除线、超链接、图片等，并针对 AI 流式输出做了增量解析与布局优化。</remarks>
    [Description("Markdown 渲染控件")]
    [ToolboxItem(true)]
    [Designer(typeof(IControlDesigner))]
    [Obsolete("预览版：未经充分测试 Preview version: Not fully tested", false)]
    public partial class Markdown : IControl
    {
        #region 初始化

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        readonly MarkdownParser parser = new MarkdownParser();
        readonly StringBuilder buffer = new StringBuilder();

        public Markdown() : base(ControlType.Select)
        {
            ScrollBar = new ScrollBar(this, true, false);
        }

        #endregion

        #region 属性

        string? mdtext;
        bool mdtextDirty;
        /// <summary>
        /// Markdown 文本
        /// </summary>
        [Description("Markdown 文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null), Browsable(true)]
        public override string? Text
        {
#pragma warning disable CS8764
            get
            {
                // 流式追加时惰性同步，避免每次 Append 都生成完整字符串
                if (mdtextDirty)
                {
                    mdtext = buffer.ToString();
                    mdtextDirty = false;
                }
                return mdtext;
            }
#pragma warning restore CS8764
            set
            {
                if (mdtext == value && !mdtextDirty) return;
                mdtext = value;
                mdtextDirty = false;
                SetMarkdown(value, false);
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        string? basePath;
        /// <summary>
        /// 相对路径的基路径（本地目录或 URL），用于解析图片与链接的相对地址
        /// </summary>
        [Description("相对路径的基路径（本地目录或 URL）"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(null)]
        public string? BasePath
        {
            get => basePath;
            set
            {
                if (basePath == value) return;
                basePath = value;
                // 基准路径变化后旧的相对路径解析结果失效，释放并重新请求
                DisposeImages();
                LoadLayout(true);
                RequestImages();
                OnPropertyChanged(nameof(BasePath));
            }
        }

        bool autoScroll = true;
        /// <summary>
        /// 流式追加时自动滚动到底部
        /// </summary>
        [Description("流式追加时自动滚动到底部"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool AutoScroll
        {
            get => autoScroll;
            set
            {
                if (autoScroll == value) return;
                autoScroll = value;
                OnPropertyChanged(nameof(AutoScroll));
            }
        }

        int streamInterval = 30;
        /// <summary>
        /// 流式刷新间隔（毫秒）
        /// </summary>
        [Description("流式刷新间隔（毫秒）"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(30)]
        public int StreamInterval
        {
            get => streamInterval;
            set
            {
                if (streamInterval == value) return;
                streamInterval = value > 0 ? value : 30;
                if (streamTimer != null) streamTimer.Interval = streamInterval;
                OnPropertyChanged(nameof(StreamInterval));
            }
        }

        bool linkUnderline;
        /// <summary>
        /// 超链接始终显示下划线
        /// </summary>
        [Description("超链接始终显示下划线"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool LinkUnderline
        {
            get => linkUnderline;
            set
            {
                if (linkUnderline == value) return;
                linkUnderline = value;
                Invalidate();
                OnPropertyChanged(nameof(LinkUnderline));
            }
        }

        Font? codeFont;
        /// <summary>
        /// 代码字体
        /// </summary>
        [Description("代码字体"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Font? CodeFont
        {
            get => codeFont;
            set
            {
                if (codeFont == value) return;
                codeFont = value;
                DisposeFont();
                InvalidateMeasure();
                LoadLayout(true);
                OnPropertyChanged(nameof(CodeFont));
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 设置 Markdown 文本（立即解析并布局）
        /// </summary>
        public void SetMarkdown(string? markdown) => SetMarkdown(markdown, false);

        /// <summary>
        /// 设置 Markdown 文本（立即解析并布局）
        /// </summary>
        /// <param name="markdown">Markdown 文本</param>
        /// <param name="resetScroll">是否重置滚动条</param>
        public void SetMarkdown(string? markdown, bool resetScroll)
        {
            ClearSelection();
            StopStream();
            buffer.Length = 0;
            if (!string.IsNullOrEmpty(markdown)) buffer.Append(markdown);
            mdtext = markdown;
            mdtextDirty = false;
            docOld = null;
            pendingAnchor = null;
            if (resetScroll) ScrollBar.Clear();
            // 句柄未创建时 ApplyParse 只解析文本，布局与图片请求延迟到 OnHandleCreated 执行，
            // 否则初始化阶段（构造函数 / InitializeComponent）设置的图片永远等不到请求
            ApplyParse(true, false);
        }

        /// <summary>
        /// 流式追加 Markdown 文本（适用于 AI 逐字输出）
        /// </summary>
        public void Append(string? text)
        {
            if (string.IsNullOrEmpty(text)) return;
            ClearSelection();
            buffer.Append(text);
            mdtextDirty = true;
            streamDirty = true;
            var timer = EnsureTimer();
            if (!timer.Enabled) timer.Start();
        }

        /// <summary>
        /// 流式追加一行 Markdown 文本
        /// </summary>
        public void AppendLine(string? text)
        {
            if (string.IsNullOrEmpty(text)) Append("\n\n");
            else Append(text + "\n\n");
        }

        /// <summary>
        /// 结束流式输出（立即完整解析并校准布局）
        /// </summary>
        public void CompleteStreaming()
        {
            StopStream();
            ApplyParse(true, false);
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        public void Clear()
        {
            StopStream();
            buffer.Length = 0;
            mdtext = null;
            mdtextDirty = false;
            docOld = null;
            pendingAnchor = null;
            ScrollBar.Clear();
            ApplyParse(true, false);
        }

        /// <summary>
        /// 滚动到顶部
        /// </summary>
        public void ScrollToTop() => ScrollBar.Value = 0;

        /// <summary>
        /// 滚动到底部
        /// </summary>
        public void ScrollToBottom() => ScrollBar.Value = ScrollBar.VrValueI;

        /// <summary>
        /// 是否处于底部
        /// </summary>
        public bool IsAtBottom => ScrollBar.Value >= ScrollBar.VrValueI - 2;

        /// <summary>
        /// 内容高度
        /// </summary>
        [Browsable(false)]
        public int ContentHeight => contentHeight;

        #endregion

        #region 流式刷新

        Timer? streamTimer;
        bool streamDirty;

        Timer EnsureTimer()
        {
            if (streamTimer == null)
            {
                streamTimer = new Timer { Interval = streamInterval };
                streamTimer.Tick += StreamTimerTick;
            }
            return streamTimer;
        }

        void StopStream()
        {
            streamDirty = false;
            if (streamTimer != null) streamTimer.Stop();
        }

        void StreamTimerTick(object? sender, EventArgs e)
        {
            if (!streamDirty)
            {
                if (streamTimer != null) streamTimer.Stop();
                return;
            }
            streamDirty = false;
            bool keepBottom = autoScroll && IsAtBottom;
            ApplyParse(false, keepBottom);
        }

        void ApplyParse(bool full, bool keepBottom)
        {
            try { doc = parser.Parse(buffer.ToString()); }
            catch { doc = new MDDocument(); }
            // 句柄未创建时不做布局（LoadLayout 会频繁初始化），
            // 布局与图片请求统一由 OnHandleCreated 补做，避免“先 SetMarkdown 后显示”时图片永不加载
            if (!IsHandleCreated) return;
            LoadLayout(full);
            RequestImages();
            if (keepBottom) ScrollBar.Value = ScrollBar.VrValueI;
        }

        #endregion

        #region 生命周期

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout(true);
            // 首次布局完成后再请求图片（此时才具备排版宽度与句柄）
            RequestImages();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout(true);
            base.OnSizeChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            DisposeFont();
            InvalidateMeasure();
            LoadLayout(true);
            base.OnFontChanged(e);
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                downPoint = e.Location;
                movedAfterDown = false;
                if (selectable) SelectMouseDown(e.X - Padding.Left, e.Y - Padding.Top);
            }
            if (ScrollBar.MouseDown(e.X, e.Y)) OnTouchDown(e.X, e.Y);
            if (!Focused) Focus();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ScrollBar.MouseUp();
            OnTouchUp();
            if (selectable) SelectMouseUp();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left && downPoint.HasValue)
            {
                int dx = e.X - downPoint.Value.X, dy = e.Y - downPoint.Value.Y;
                if (!movedAfterDown && (Math.Abs(dx) > 2 || Math.Abs(dy) > 2)) movedAfterDown = true;
            }
            if (ScrollBar.MouseMove(e.X, e.Y))
            {
                if (selectable && e.Button == MouseButtons.Left && movedAfterDown) SelectMouseMove(e.X - Padding.Left, e.Y - Padding.Top);
                else
                {
                    var pos = ClientRectangle.PaddingRect(Padding);
                    var frag = HitTestFragment(e.X - pos.X, e.Y - pos.Y);
                    string? url = frag == null ? null : frag.Url;
                    if (url == null && frag != null && frag.Kind == MDInlineKind.Image) url = frag.Src;
                    if (url != hoverUrl)
                    {
                        hoverUrl = url;
                        Cursor = url == null ? (selectable ? Cursors.IBeam : Cursors.Default) : Cursors.Hand;
                        Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
            if (hoverUrl != null)
            {
                hoverUrl = null;
                Cursor = selectable ? Cursors.IBeam : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }

        protected override bool OnTouchScrollX(int x, int y, int value) => ScrollBar.MouseWheelXCore(value);
        protected override bool OnTouchScrollY(int x, int y, int value) => ScrollBar.MouseWheelYCore(value);

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button != MouseButtons.Left || movedAfterDown) return;
            var pos = ClientRectangle.PaddingRect(Padding);
            var frag = HitTestFragment(e.X - pos.X, e.Y - pos.Y);
            if (frag == null) return;
            string? url = frag.Url;
            if (url == null && frag.Kind == MDInlineKind.Image) url = frag.Src;
            if (string.IsNullOrEmpty(url)) return;
            var link = BuildLink(url!);
            // 先由外部决定是否需要跳转（Handled = true 可完全接管）
            OnLinkClick(link);
            if (link.Handled) return;
            // 内置跳转：#锚点 定位、基于 BasePath 的相对 Markdown 文件切换（外部地址交由外部打开）
            Navigate(link);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (!selectable) return;
            var mouse = PointToClient(MousePosition);
            SelectWord(mouse.X - Padding.Left, mouse.Y - Padding.Top);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        if (selectable)
                        {
                            SelectAll();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        return;
                    case Keys.C:
                        if (selectable && HasSelection)
                        {
                            Copy(true);
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        return;
                }
            }
            if (e.KeyCode == Keys.Escape && HasSelection)
            {
                ClearSelection();
                e.Handled = true;
                return;
            }
            base.OnKeyDown(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        Point? downPoint;
        bool movedAfterDown;

        #endregion

        #region 事件

        /// <summary>
        /// 点击超链接时发生（控件内部不处理任何业务逻辑）
        /// </summary>
        [Description("点击超链接时发生"), Category(nameof(CategoryAttribute.Action))]
        public event EventHandler<MarkdownLinkEventArgs>? LinkClick;

        /// <summary>
        /// 需要加载图片时发生（控件内部不实现任何下载逻辑）
        /// </summary>
        [Description("需要加载图片时发生"), Category(nameof(CategoryAttribute.Action))]
        public event EventHandler<MarkdownImageEventArgs>? ImageRequest;

        protected virtual void OnLinkClick(MarkdownLinkEventArgs e) => LinkClick?.Invoke(this, e);

        /// <summary>
        /// 请求图片（扩展点，控件内部不实现任何下载逻辑）
        /// </summary>
        protected virtual void OnImageRequest(string url, string? alt) => ImageRequest?.Invoke(this, new MarkdownImageEventArgs(url, alt));

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (streamTimer != null)
            {
                streamTimer.Stop();
                streamTimer.Dispose();
                streamTimer = null;
            }
            DisposeFont();
            DisposeImages();
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }
    }
}