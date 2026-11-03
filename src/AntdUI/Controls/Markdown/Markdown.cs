// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
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

        /// <summary>
        /// 排版引擎（与控件窗口解耦，聊天列表的 Markdown 气泡复用同一套实现）
        /// </summary>
        internal readonly MarkdownRenderer renderer;

        readonly StringBuilder buffer = new StringBuilder();

        public Markdown() : base(ControlType.Select)
        {
            ScrollBar = new ScrollBar(this, true, false);
            renderer = new MarkdownRenderer(Font, Dpi, ColorScheme, Name ?? nameof(Markdown))
            {
                Changed = Invalidate,
                Relayout = () => LoadLayout(true),
                PropertyChanged = OnPropertyChanged,
                ImageRequest = (url, alt) => OnImageRequest(url, alt),
            };
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
                renderer.BasePath = value;
                // 基准路径变化后旧的相对路径解析结果失效，释放并重新请求（内部含重新布局）
                renderer.ClearImageCache();
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
                renderer.LinkUnderline = value;
                Invalidate();
                OnPropertyChanged(nameof(LinkUnderline));
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
            pendingAnchor = null;
            ScrollBar.Clear();
            renderer.Clear();
            if (IsHandleCreated) LoadLayout(true);
            Invalidate();
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
        public int ContentHeight => renderer.ContentHeight;

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
            renderer.Parse(buffer.ToString());
            // 句柄未创建时不做布局（LoadLayout 会频繁初始化），
            // 布局与图片请求统一由 OnHandleCreated 补做，避免“先 SetMarkdown 后显示”时图片永不加载
            if (!IsHandleCreated) return;
            LoadLayout(full);
            renderer.RequestImages();
            if (keepBottom) ScrollBar.Value = ScrollBar.VrValueI;
        }

        #endregion

        #region 生命周期

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout(true);
            // 首次布局完成后再请求图片（此时才具备排版宽度与句柄）
            renderer.RequestImages();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout(true);
            base.OnSizeChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            LoadLayout(true);
            base.OnFontChanged(e);
        }

        #endregion

        #region 布局

        internal bool rendererLayouting;

        /// <summary>
        /// 重新排版（控件侧负责滚动条占位与两趟校准，排版本身由引擎完成）
        /// </summary>
        internal void LoadLayout(bool full = false)
        {
            if (rendererLayouting) return;
            if (!IsHandleCreated) return;
            var rect = ClientRectangle.PaddingRect(Padding);
            if (rect.Width <= 0 || rect.Height <= 0) return;
            renderer.SetHost(Font, Dpi, ColorScheme, Name ?? nameof(Markdown));
            rendererLayouting = true;
            try
            {
                this.GDI(g =>
                {
                    int reserve = ScrollBar.ShowY ? ScrollBar.SIZE : 0;
                    for (int pass = 0; pass < 2; pass++)
                    {
                        int w = rect.Width - reserve;
                        if (w <= 0) return;
                        renderer.Layout(g, w, pass == 0 ? full : true);
                        ScrollBar.SetVrSize(renderer.ContentHeight, rect);
                        int now = ScrollBar.ShowY ? ScrollBar.SIZE : 0;
                        if (now == reserve) break;
                        reserve = now;
                    }
                });
            }
            finally { rendererLayouting = false; }
            // 延迟锚点定位：文档刚切换 / 句柄刚创建时布局在这一步才就绪
            if (pendingAnchor != null)
            {
                string key = pendingAnchor;
                pendingAnchor = null;
                int y;
                if (renderer.TryGetAnchor(key, out y)) ApplyAnchor(y);
            }
            Invalidate();
        }

        #endregion

        #region 鼠标

        bool selectable => renderer.Selectable;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                downPoint = e.Location;
                movedAfterDown = false;
                if (selectable) renderer.SelectMouseDown(e.X - Padding.Left, e.Y - Padding.Top + ScrollBar.Value);
            }
            if (ScrollBar.MouseDown(e.X, e.Y)) OnTouchDown(e.X, e.Y);
            if (!Focused) Focus();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ScrollBar.MouseUp();
            OnTouchUp();
            if (selectable) renderer.SelectMouseUp();
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
                if (selectable && e.Button == MouseButtons.Left && movedAfterDown) renderer.SelectMouseMove(e.X - Padding.Left, e.Y - Padding.Top + ScrollBar.Value);
                else
                {
                    var pos = ClientRectangle.PaddingRect(Padding);
                    var frag = renderer.HitTest(e.X - pos.X, e.Y - pos.Y + ScrollBar.Value);
                    string? url = frag == null ? null : frag.Url;
                    if (url == null && frag != null && frag.Kind == MDInlineKind.Image) url = frag.Src;
                    if (url != renderer.HoverUrl)
                    {
                        renderer.HoverUrl = url;
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
            if (renderer.HoverUrl != null)
            {
                renderer.HoverUrl = null;
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
            var frag = renderer.HitTest(e.X - pos.X, e.Y - pos.Y + ScrollBar.Value);
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
            renderer.SelectWord(mouse.X - Padding.Left, mouse.Y - Padding.Top + ScrollBar.Value);
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

        #region 绘制

        protected override void OnDraw(DrawEventArgs e)
        {
            var rect = e.Rect.PaddingRect(Padding);
            renderer.SyncEnv(Dpi, ColorScheme, Name ?? nameof(Markdown));
            renderer.Draw(e.Canvas, rect, ScrollBar.Value, rect.Height);
            ScrollBar.Paint(e.Canvas, ColorScheme);
            base.OnDraw(e);
        }

        #endregion

        #region 选择

        /// <summary>
        /// 是否存在选区
        /// </summary>
        [Browsable(false)]
        public bool HasSelection => renderer.HasSelection;

        /// <summary>
        /// 选中的文本（保留 Markdown 格式）
        /// </summary>
        [Browsable(false)]
        public string SelectedText => renderer.SelectedText;

        /// <summary>
        /// 清除选区
        /// </summary>
        public void ClearSelection() => renderer.ClearSelection();

        /// <summary>
        /// 全选
        /// </summary>
        public void SelectAll() => renderer.SelectAll();

        /// <summary>
        /// 复制选中内容到剪贴板
        /// </summary>
        /// <param name="keepFormat">是否保留 Markdown 格式</param>
        public bool Copy(bool keepFormat = true) => this.ClipboardSetText(renderer.GetSelectedText(keepFormat));

        /// <summary>
        /// 获取选中文本
        /// </summary>
        /// <param name="keepFormat">是否保留 Markdown 格式</param>
        public string GetSelectedText(bool keepFormat = true) => renderer.GetSelectedText(keepFormat);

        #endregion

        #region 图片

        /// <summary>
        /// 设置图片（供外部下载完成后回调，支持跨线程调用）
        /// </summary>
        /// <param name="url">图片原始地址（与 <see cref="MarkdownImageEventArgs.Url"/> 一致）</param>
        /// <param name="content">图片内容：普通图片流（PNG/JPG/GIF/BMP 等）或 SVG 文本，null 表示加载失败</param>
        public void SetImage(string url, byte[]? content) => renderer.SetImage(url, content, null);

        /// <summary>
        /// 设置图片内容（总入口，自动识别 SVG 文本 / 普通图片流，支持跨线程调用）
        /// </summary>
        /// <param name="url">图片原始地址（与 <see cref="MarkdownImageEventArgs.Url"/> 一致）</param>
        /// <param name="content">图片内容，null 表示加载失败</param>
        /// <param name="contentType">内容类型（如 image/png、image/svg+xml;charset=utf-8），为空时按内容自动识别</param>
        public void SetImage(string url, byte[]? content, string? contentType) => renderer.SetImage(url, content, contentType);

        /// <summary>
        /// 清空图片缓存（随后会按新的解析结果重新请求）
        /// </summary>
        public void ClearImageCache() => renderer.ClearImageCache();

        /// <summary>
        /// 将相对路径解析为可访问地址（基于 <see cref="BasePath"/>）
        /// </summary>
        public string ResolveUrl(string url) => renderer.ResolveUrl(url);

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
            renderer.Dispose();
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }
    }
}
