// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace AntdUI.Chat
{
    /// <summary>
    /// Markdown 消息项（AI 消息直出）
    /// </summary>
    /// <remarks>
    /// 复用 <see cref="MarkdownRenderer"/> 的解析 / 排版 / 绘制 / 选区能力，在聊天气泡内渲染完整 Markdown：
    /// 标题、列表、表格、引用、代码块（含 Diff 高亮）、图片、超链接、粗体 / 删除线等。
    /// 支持鼠标拖选 + Ctrl+C / Ctrl+A 复制，以及流式追加（<see cref="Append"/>）实现逐字输出。
    /// </remarks>
    public class MdChatItem : IChatItem
    {
        public MdChatItem(string? markdown)
        {
            _text = markdown ?? "";
            buffer.Append(_text);
        }
        public MdChatItem(string? markdown, Image? icon, string? name) : this(markdown)
        {
            _icon = icon;
            _name = name;
        }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        public string? ID { get; set; }

        /// <summary>
        /// 本人
        /// </summary>
        [Description("本人"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool Me { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Description("时间"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        public string? Time { get; set; }

        Image? _icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Image? Icon
        {
            get => _icon;
            set
            {
                if (_icon == value) return;
                _icon = value;
                Invalidates();
            }
        }

        string? _name;
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                Invalidates();
            }
        }

        string _text;
        readonly StringBuilder buffer = new StringBuilder();

        /// <summary>
        /// Markdown 文本
        /// </summary>
        [Description("Markdown 文本"), Category(nameof(CategoryAttribute.Appearance))]
        public string Text
        {
            get => _text;
            set
            {
                string v = value ?? "";
                if (_text == v) return;
                _text = v;
                buffer.Length = 0;
                buffer.Append(v);
                textDirty = true;
                PARENT?.LoadLayout(this, true);
            }
        }

        string? basePath;
        /// <summary>
        /// 相对路径的基路径（本地目录或 URL），用于解析图片与链接的相对地址
        /// </summary>
        [Description("相对路径基路径（本地目录或 URL）"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(null)]
        public string? BasePath
        {
            get => basePath;
            set
            {
                if (basePath == value) return;
                basePath = value;
                if (renderer != null)
                {
                    renderer.BasePath = value;
                    renderer.ClearImageCache();
                }
                layoutDirty = true;
                PARENT?.LoadLayout(this, true);
            }
        }

        bool selectable = true;
        /// <summary>
        /// 是否允许鼠标选中内容并复制
        /// </summary>
        [Description("是否允许选中复制"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool Selectable
        {
            get => selectable;
            set
            {
                if (selectable == value) return;
                selectable = value;
                if (renderer != null) renderer.Selectable = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 文字颜色（为空时使用 Markdown 默认主题色）
        /// </summary>
        [Description("文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                if (renderer != null) renderer.Fore = value;
                Invalidate();
            }
        }
        Color? fore;

        /// <summary>
        /// 点击超链接时发生
        /// </summary>
        [Description("点击超链接时发生"), Category(nameof(CategoryAttribute.Action))]
        public event EventHandler<MarkdownLinkEventArgs>? LinkClick;

        #region 加载

        AnimationTask? task;
        internal bool showlinedot = false;
        bool loading;
        /// <summary>
        /// 加载中（流式输出中：末尾显示闪烁光标）
        /// </summary>
        [Description("加载中"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                task?.Dispose();
                task = null;
                showlinedot = false;
                if (PARENT == null) return;
                if (value)
                {
                    task = new AnimationTask(new AnimationLoopConfig(PARENT, () =>
                    {
                        showlinedot = !showlinedot;
                        Invalidate();
                        return loading;
                    }, 200).SetPriority());
                }
                else PARENT.Invalidate();
            }
        }

        internal void InitLoading()
        {
            if (loading && task == null)
            {
                if (PARENT == null) return;
                task = new AnimationTask(new AnimationLoopConfig(PARENT, () =>
                {
                    showlinedot = !showlinedot;
                    Invalidate();
                    return loading;
                }, 200).SetPriority());
            }
        }

        #endregion

        #region 流式输出

        bool textDirty = true, layoutDirty = true;

        /// <summary>
        /// 流式追加 Markdown 文本（适用于 AI 逐字输出）
        /// </summary>
        public void Append(string? text)
        {
            if (string.IsNullOrEmpty(text)) return;
            buffer.Append(text);
            Text = buffer.ToString();
        }

        /// <summary>
        /// 流式追加一行 Markdown 文本
        /// </summary>
        public void AppendLine(string? text)
        {
            if (string.IsNullOrEmpty(text)) Append("\n\n");
            else Append(text + "\n\n");
        }

        #endregion

        #region 排版引擎

        MarkdownRenderer? renderer;
        int lastAvail = -1;
        internal int contentWidth, contentHeight;

        MarkdownRenderer Renderer(Font font, float dpi, TAMode scheme, string name)
        {
            if (renderer == null)
            {
                renderer = new MarkdownRenderer(font, dpi, scheme, name)
                {
                    BasePath = basePath,
                    Changed = Invalidate,
                    Relayout = () =>
                    {
                        layoutDirty = true;
                        PARENT?.LoadLayout(this, true);
                    },
                    ImageRequest = (url, alt) => PARENT?.OnMdChatImageRequest(this, url, alt),
                };
                renderer.Selectable = selectable;
                renderer.Fore = fore;
            }
            else renderer.SetHost(font, dpi, scheme, name);
            return renderer;
        }

        bool requestPending;

        /// <summary>
        /// 请求未加载的图片（延后到消息循环，内部不实现下载逻辑）
        /// </summary>
        void RequestImages()
        {
            var parent = PARENT;
            if (parent == null || !parent.IsHandleCreated || requestPending) return;
            requestPending = true;
            try
            {
                requestPending = false;
                renderer?.RequestImages();
            }
            catch { requestPending = false; }
        }

        /// <summary>
        /// 计算位置与高度（由 ChatList 调用）
        /// </summary>
        internal int SetRect(Rectangle _rect, int y, Canvas g, Font font, int max_width, int gap, int spilt, int spilt2, int image_size, bool iconLess, float dpi, TAMode scheme, string name)
        {
            var rd = Renderer(font, dpi, scheme, name);
            int avail = max_width - spilt2;
            if (avail < 40) avail = 40;
            bool changed = textDirty || layoutDirty;
            bool full = changed || avail != lastAvail;
            if (textDirty)
            {
                rd.Parse(_text);
                textDirty = false;
            }
            layoutDirty = false;
            lastAvail = avail;
            rd.Layout(g, avail, full);

            // 内容较窄时收缩气泡宽度（AI 消息较短时不再铺满整行）
            int cw = rd.ContentWidth, ch = rd.ContentHeight;
            if (cw > 0 && cw + (int)(2 * dpi) < avail)
            {
                int target = Math.Max((int)(40 * dpi), cw + (int)(2 * dpi));
                if (target < avail)
                {
                    rd.Layout(g, target, true);
                    if (rd.ContentWidth > cw) cw = rd.ContentWidth;
                    ch = rd.ContentHeight;
                }
            }
            if (cw <= 0) cw = Math.Min(avail, (int)(48 * dpi));
            if (cw > avail) cw = avail;
            contentWidth = cw;
            contentHeight = ch;
            // 图片请求延后到消息循环：避免在父级排版（GDI 作用域）内再次触发布局
            if (changed) RequestImages();

            var msglen = new Size(cw + spilt2, ch + spilt2);
            if (string.IsNullOrEmpty(_name))
            {
                rect = new Rectangle(_rect.X, _rect.Y + y, _rect.Width, msglen.Height);
                if (Me)
                {
                    if (iconLess) rect_icon = new Rectangle(rect.Right, rect.Y, 0, 0);
                    else rect_icon = new Rectangle(rect.Right - gap - image_size, rect.Y, image_size, image_size);
                    rect_read = new Rectangle(rect_icon.X - spilt - msglen.Width, rect_icon.Y, msglen.Width, msglen.Height);
                }
                else
                {
                    if (iconLess) rect_icon = new Rectangle(0, rect.Y, 0, 0);
                    else rect_icon = new Rectangle(rect.X + gap, rect.Y, image_size, image_size);
                    rect_read = new Rectangle(rect_icon.Right + spilt, rect_icon.Y, msglen.Width, msglen.Height);
                }
            }
            else
            {
                rect = new Rectangle(_rect.X, _rect.Y + y, _rect.Width, msglen.Height + gap);
                var size_name = g.MeasureString(_name, font).Width;
                if (Me)
                {
                    if (iconLess) rect_icon = new Rectangle(rect.Right, rect.Y, 0, 0);
                    else rect_icon = new Rectangle(rect.Right - gap - image_size, rect.Y, image_size, image_size);
                    rect_name = new Rectangle(rect_icon.X - spilt - msglen.Width, rect_icon.Y, size_name, gap);
                    rect_read = new Rectangle(rect_icon.X - spilt - msglen.Width, rect_name.Bottom, msglen.Width, msglen.Height);
                }
                else
                {
                    if (iconLess) rect_icon = new Rectangle(0, rect.Y, 0, 0);
                    else rect_icon = new Rectangle(rect.X + gap, rect.Y, image_size, image_size);
                    rect_name = new Rectangle(rect_icon.Right + spilt, rect_icon.Y, size_name, gap);
                    rect_read = new Rectangle(rect_name.X, rect_name.Bottom, msglen.Width, msglen.Height);
                }
            }
            rect_text = new Rectangle(rect_read.X + spilt, rect_read.Y + spilt, msglen.Width - spilt2, msglen.Height - spilt2);

            var timeText = Time;
            if (timeText != null)
            {
                var size_time = g.MeasureString(timeText, font);
                rect_time = new Rectangle(Me ? rect_read.Left : rect_read.Right - size_time.Width, rect_name.Top, size_time.Width, size_time.Height);
            }
            return rect.Height;
        }

        internal Rectangle rect_read { get; set; }
        internal Rectangle rect_name { get; set; }
        internal Rectangle rect_text { get; set; }
        internal Rectangle rect_icon { get; set; }
        internal Rectangle rect_time { get; set; }

        #endregion

        #region 绘制

        /// <summary>
        /// 绘制 Markdown 内容（气泡由 ChatList 绘制）
        /// </summary>
        internal void Paint(Canvas g, Color caret)
        {
            if (renderer == null) return;
            renderer.Draw(g, rect_text, 0, Math.Max(rect_text.Height, contentHeight));
            if (showlinedot)
            {
                int w = Math.Max(2, (int)(3 * (PARENT?.Dpi ?? 1F)));
                var rect = renderer.LastCaret(w, contentHeight);
                using (var brush = new SolidBrush(caret))
                {
                    g.Fill(brush, new Rectangle(rect_text.X + rect.X, rect_text.Y + rect.Y, rect.Width, rect.Height));
                }
            }
        }

        #endregion

        #region 交互

        internal bool ContainsRead(int x, int y, int sx, int sy) => rect_text.Contains(new Point(x + sx, y + sy));
        internal bool ContainsReadIcon(int x, int y, int sx, int sy) => rect_icon.Contains(new Point(x + sx, y + sy));

        /// <summary>
        /// 选中区域背景颜色（由 ChatList 统一下发）
        /// </summary>
        internal Color? SelectionColor
        {
            set
            {
                if (renderer != null) renderer.SelectionColor = value;
            }
        }

        internal bool HasSelection => renderer != null && renderer.HasSelection;
        internal string GetSelectedText(bool keepFormat) => renderer == null ? "" : renderer.GetSelectedText(keepFormat);
        internal void SelectAll() => renderer?.SelectAll();
        internal void ClearSelection() => renderer?.ClearSelection();

        internal bool SelectDown(int x, int y, int sx, int sy)
        {
            if (renderer == null || !selectable || !ContainsRead(x, y, sx, sy)) return false;
            renderer.SelectMouseDown(x + sx - rect_text.X, y + sy - rect_text.Y);
            return true;
        }

        internal void SelectMove(int x, int y, int sx, int sy)
        {
            if (renderer == null) return;
            renderer.SelectMouseMove(x + sx - rect_text.X, y + sy - rect_text.Y);
        }

        internal void SelectUp() => renderer?.SelectMouseUp();

        /// <summary>
        /// 双击选词
        /// </summary>
        internal bool SelectWord(int x, int y, int sx, int sy)
        {
            if (renderer == null || !selectable || !ContainsRead(x, y, sx, sy)) return false;
            renderer.SelectWord(x + sx - rect_text.X, y + sy - rect_text.Y);
            return true;
        }

        /// <summary>
        /// 更新悬停链接，返回是否悬停在可点击内容上
        /// </summary>
        internal bool UpdateHover(int x, int y, int sx, int sy)
        {
            if (renderer == null) return false;
            MDFragment? frag = null;
            if (ContainsRead(x, y, sx, sy)) frag = renderer.HitTest(x + sx - rect_text.X, y + sy - rect_text.Y);
            string? url = frag == null ? null : frag.Url;
            if (url == null && frag != null && frag.Kind == MDInlineKind.Image) url = frag.Src;
            renderer.HoverUrl = url;
            return !string.IsNullOrEmpty(url);
        }

        /// <summary>
        /// 点击内容，命中链接时触发 <see cref="LinkClick"/>
        /// </summary>
        internal void Click(int x, int y, int sx, int sy)
        {
            if (renderer == null || !ContainsRead(x, y, sx, sy)) return;
            var frag = renderer.HitTest(x + sx - rect_text.X, y + sy - rect_text.Y);
            if (frag == null) return;
            string? url = frag.Url;
            if (url == null && frag.Kind == MDInlineKind.Image) url = frag.Src;
            if (string.IsNullOrEmpty(url)) return;
            LinkClick?.Invoke(this, renderer.BuildLink(url!));
        }

        #endregion

        #region 图片

        /// <summary>
        /// 设置图片（下载完成后回调，支持跨线程调用）
        /// </summary>
        internal void SetImage(string url, byte[]? content, string? contentType) => renderer?.SetImage(url, content, contentType);

        /// <summary>
        /// 清空图片缓存
        /// </summary>
        internal void ClearImageCache() => renderer?.ClearImageCache();

        #endregion

        #region 设置

        public MdChatItem SetID(string? value)
        {
            ID = value;
            return this;
        }

        public MdChatItem SetName(string? value)
        {
            _name = value;
            return this;
        }

        public MdChatItem SetIcon(Image? img)
        {
            _icon = img;
            return this;
        }

        public MdChatItem SetLoading(bool value = true)
        {
            Loading = value;
            return this;
        }

        public MdChatItem SetMe(bool value = true)
        {
            Me = value;
            return this;
        }

        public MdChatItem SetText(string? value)
        {
            Text = value ?? "";
            return this;
        }

        public MdChatItem SetBasePath(string? value)
        {
            BasePath = value;
            return this;
        }

        public MdChatItem SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        public MdChatItem SetTime(DateTime value)
        {
            if (value.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")) Time = value.ToString("HH:mm:ss");
            else Time = value.ToString("yyyy-MM-dd HH:mm:ss");
            return this;
        }
        public MdChatItem SetTime(string? value)
        {
            Time = value;
            return this;
        }

        #endregion

        public override void Dispose()
        {
            task?.Dispose();
            task = null;
            renderer?.Dispose();
            renderer = null;
            base.Dispose();
        }
    }
}