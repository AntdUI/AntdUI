// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AntdUI.Chat
{
    /// <summary>
    /// ChatList 气泡聊天列表
    /// </summary>
    /// <remarks>气泡聊天列表。</remarks>
    [Description("ChatList 气泡聊天列表")]
    [ToolboxItem(true)]
    public class ChatList : IControl
    {
        #region 属性

        ChatItemCollection? items;
        /// <summary>
        /// 数据集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("数据集合"), Category("数据")]
        public ChatItemCollection Items
        {
            get
            {
                items ??= new ChatItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        /// <summary>
        /// Emoji字体
        /// </summary>
        [Description("Emoji字体"), Category("外观"), DefaultValue("Segoe UI Emoji")]
        public string EmojiFont { get; set; } = "Segoe UI Emoji";

        /// <summary>
        /// 表情图标比例
        /// </summary>
        [Description("表情图标比例"), Category("外观"), DefaultValue(1F)]
        public float EmojiRatio { get; set; } = 1F;

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        #region 主题

        /// <summary>
        /// 选中颜色
        /// </summary>
        [Description("选中颜色"), Category("外观"), DefaultValue(typeof(Color), "60, 96, 165, 250")]
        public Color SelectionColor = Color.FromArgb(60, 96, 165, 250);

        /// <summary>
        /// 选中颜色(我)
        /// </summary>
        [Description("选中颜色(我)"), Category("外观"), DefaultValue(typeof(Color), "96, 165, 250")]
        public Color SelectionColorMe = Color.FromArgb(96, 165, 250);

        /// <summary>
        /// 气泡文本颜色(对方)
        /// </summary>
        [Description("气泡文本颜色(对方)"), Category("外观"), DefaultValue(null)]
        public Color? ForeBubble { get; set; }

        /// <summary>
        /// 气泡背景色(对方)
        /// </summary>
        [Description("气泡背景色(对方)"), Category("外观"), DefaultValue(null)]
        public Color? BackBubble { get; set; }

        /// <summary>
        /// 气泡激活背景色(对方)
        /// </summary>
        [Description("气泡激活背景色(对方)"), Category("外观"), DefaultValue(null)]
        public Color? BackActiveBubble { get; set; }

        /// <summary>
        /// 气泡文本颜色(我)
        /// </summary>
        [Description("气泡文本颜色(我)"), Category("外观"), DefaultValue(null)]
        public Color? ForeBubbleMe { get; set; }

        /// <summary>
        /// 气泡背景色(我)
        /// </summary>
        [Description("气泡背景色(我)"), Category("外观"), DefaultValue(null)]
        public Color? BackBubbleMe { get; set; }

        /// <summary>
        /// 气泡激活背景色(我)
        /// </summary>
        [Description("气泡激活背景色(我)"), Category("外观"), DefaultValue(null)]
        public Color? BackActiveBubbleMe { get; set; }

        #endregion

        /// <summary>
        /// 隐藏图标
        /// </summary>
        [Description("隐藏图标"), Category("外观"), DefaultValue(false)]
        public bool IconLess { get; set; }

        /// <summary>
        /// 每项之间的间隙
        /// </summary>
        [Description("每项之间的间隙"), Category("外观"), DefaultValue(0)]
        public int ItemGap { get; set; }

        /// <summary>
        /// 气泡间隙
        /// </summary>
        [Description("气泡间隙"), Category("外观"), DefaultValue(.75F)]
        public float BubbleGap { get; set; } = .75F;

        /// <summary>
        /// 点击图片使能
        /// </summary>
        [Description("点击图片使能"), Category("行为"), DefaultValue(false)]
        public bool EnabledClickImage { get; set; }

        /// <summary>
        /// 仅焦点项才显示时间
        /// </summary>
        [Description("仅焦点项才显示时间"), Category("行为"), DefaultValue(false)]
        public bool ShowTimeFocused { get; set; }

        /// <summary>
        /// 焦点消息项
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IChatItem? FocusedChatItem { get; protected set; }

        #endregion

        #region 方法

        public bool AddToBottom(IChatItem it, bool force = false)
        {
            if (force)
            {
                Items.Add(it);
                ToBottom();
                return true;
            }
            else
            {
                bool isbutt = IsBottom;
                Items.Add(it);
                if (isbutt) ToBottom();
                return isbutt;
            }
        }

        public bool IsBottom
        {
            get
            {
                if (ScrollBar.Show) return ScrollBar.Value == ScrollBar.VrValueI;
                else return true;
            }
        }

        public void ToBottom() => ScrollBar.Value = ScrollBar.VrValueI;

        /// <summary>
        /// 滚动到指定行
        /// </summary>
        /// <param name="i">行</param>
        /// <param name="force">是否强制滚动</param>
        /// <returns>返回滚动量</returns>
        public int ScrollLine(int i, bool force = false)
        {
            if (items == null || !ScrollBar.ShowY) return 0;
            return ScrollLine(i, items, force);
        }

        int ScrollLine(int i, ChatItemCollection items, bool force = false)
        {
            if (!ScrollBar.ShowY) return 0;
            var selectRow = items[i];
            int sy = ScrollBar.ValueY;
            if (force)
            {
                ScrollBar.ValueY = items[i].rect.Y;
                return sy - ScrollBar.ValueY;
            }
            else
            {
                if (selectRow.rect.Y < sy || selectRow.rect.Bottom > sy + Height)
                {
                    ScrollBar.ValueY = items[i].rect.Y;
                    return sy - ScrollBar.ValueY;
                }
            }
            return 0;
        }

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            float sy = ScrollBar.Value, radius = Dpi * 8F;
            g.TranslateTransform(0, -sy);
            using (var selection = new SolidBrush(SelectionColor))
            using (var selectionme = new SolidBrush(SelectionColorMe))
            using (var foreBubble = new SolidBrush(ForeBubble ?? Color.Black))
            using (var bgBubble = new SolidBrush(BackBubble ?? Color.White))
            using (var bgActiveBubble = new SolidBrush(BackActiveBubble ?? Colour.FillQuaternary.Get(nameof(ChatList), ColorScheme)))

            using (var foreBubbleme = new SolidBrush(ForeBubbleMe ?? Color.White))
            using (var bgBubbleme = new SolidBrush(BackBubbleMe ?? Color.FromArgb(0, 153, 255)))
            using (var bgActiveBubbleme = new SolidBrush(BackActiveBubbleMe ?? Color.FromArgb(0, 134, 224)))
            {
                foreach (var it in items) PaintItem(g, it, e.Rect, sy, radius, selection, selectionme, foreBubble, bgBubble, bgActiveBubble, foreBubbleme, bgBubbleme, bgActiveBubbleme);
            }
            g.ResetTransform();
            ScrollBar.Paint(g, ColorScheme);
            base.OnDraw(e);
        }

        readonly FormatFlags SFL = FormatFlags.Top | FormatFlags.HorizontalCenter;

        void PaintItem(Canvas g, IChatItem it, Rectangle rect, float sy, float radius, SolidBrush selection, SolidBrush selectionme, SolidBrush forebubble, SolidBrush bgbubble, SolidBrush bgActiveBubble, SolidBrush forebubbleme, SolidBrush bgbubbleme, SolidBrush bgActiveBubbleme)
        {
            it.show = it.rect.Y > sy - rect.Height - it.rect.Height && it.rect.Bottom < ScrollBar.Value + ScrollBar.ReadSize + it.rect.Height;
            if (it.show)
            {
                if (it is TextChatItem text)
                {
                    using (var path = text.rect_read.RoundPath(radius))
                    {
                        using (var brush = new SolidBrush(Colour.TextTertiary.Get(nameof(ChatList), ColorScheme)))
                        {
                            g.String(text.Name, Font, brush, text.rect_name, SFL);
                            if (ShowTimeFocused && text.Time != null) g.String(text.Time, Font, brush, text.rect_time, SFL);
                        }
                        if (text.Me)
                        {
                            g.Fill(bgbubbleme, path);
                            if (text.selectionLength > 0) g.Fill(bgActiveBubbleme, path);
                            PaintItemText(g, text, forebubbleme, selectionme);
                        }
                        else
                        {
                            g.Fill(bgbubble, path);
                            if (text.selectionLength > 0) g.Fill(bgActiveBubble, path);
                            PaintItemText(g, text, forebubble, selection);
                        }
                    }
                    if (IconLess) return;
                    if (text.Icon != null) g.Image(text.rect_icon, text.Icon, TFit.Cover, 0, true);
                }
            }
        }

        void PaintItemText(Canvas g, TextChatItem text, SolidBrush fore, SolidBrush selection)
        {
            if (text.selectionLength > 0)
            {
                int end = text.selectionStartTemp + text.selectionLength - 1;
                if (end > text.cache_font.Length - 1) end = text.cache_font.Length - 1;
                CacheFont first = text.cache_font[text.selectionStartTemp];
                for (int i = text.selectionStartTemp; i <= end; i++)
                {
                    var last = text.cache_font[i];
                    if (i == end) g.Fill(selection, new Rectangle(first.rect.X, first.rect.Y, last.rect.Right - first.rect.X, first.rect.Height));
                    else if (first.rect.Y != last.rect.Y || last.retun)
                    {
                        last = text.cache_font[i - 1];
                        g.Fill(selection, new Rectangle(first.rect.X, first.rect.Y, last.rect.Right - first.rect.X, first.rect.Height));
                        first = text.cache_font[i];
                    }
                }
            }
            if (text.HasEmoji)
            {
                using (var font = new Font(EmojiFont, Font.Size))
                {
                    foreach (var it in text.cache_font)
                    {
                        switch (it.type)
                        {
                            case GraphemeSplitter.STRE_TYPE.STR:
                                if (it.emoji)
                                {
                                    if (SvgDb.Emoji.TryGetValue(it.text, out var svg)) PaintItemTextEmoji(g, fore, it, svg);
                                    else g.String(it.text, font, fore, it.rect);
                                }
                                else g.String(it.text, Font, fore, it.rect);
                                break;
                            case GraphemeSplitter.STRE_TYPE.SVG:
                                PaintItemTextSvg(g, text, fore, selection, it);
                                break;
                            case GraphemeSplitter.STRE_TYPE.BASE64IMG:
                                PaintItemTextBasee64(g, text, fore, selection, it);
                                break;
                        }
                    }
                }
            }
            else
            {
                foreach (var it in text.cache_font)
                {
                    switch (it.type)
                    {
                        case GraphemeSplitter.STRE_TYPE.STR:
                            g.String(it.text, Font, fore, it.rect);
                            break;
                        case GraphemeSplitter.STRE_TYPE.SVG:
                            PaintItemTextSvg(g, text, fore, selection, it);
                            break;
                        case GraphemeSplitter.STRE_TYPE.BASE64IMG:
                            PaintItemTextBasee64(g, text, fore, selection, it);
                            break;
                    }
                }
            }

            if (text.showlinedot)
            {
                int size = (int)(2 * Dpi), w = size * 3;
                if (text.cache_font.Length > 0)
                {
                    var rect = text.cache_font[text.cache_font.Length - 1].rect;
                    using (var brush = new SolidBrush(Color.FromArgb(0, 153, 255)))
                    {
                        g.Fill(brush, new Rectangle(rect.Right - w / 2, rect.Bottom - size, w, size));
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(Color.FromArgb(0, 153, 255)))
                    {
                        g.Fill(brush, new Rectangle(text.rect_read.X + (text.rect_read.Width - w) / 2, text.rect_read.Bottom - size, w, size));
                    }
                }
            }
        }

        #region 绘制图片

        ConcurrentDictionary<string, Image> image_cache = new ConcurrentDictionary<string, Image>();
        void DisposeCache()
        {
            if (image_cache.Count == 0) return;
            foreach (var it in image_cache.Keys)
            {
                if (image_cache.TryRemove(it, out var bmp)) bmp.Dispose();
            }
        }

        void PaintItemTextEmoji(Canvas g, SolidBrush fore, CacheFont it, string svg)
        {
            if (image_cache.TryGetValue(it.text, out var bmp)) g.Image(bmp, it.rect);
            else
            {
                var bmp_svg = SvgExtend.SvgToBmp(svg, it.rect.Width, it.rect.Height, fore.Color);
                if (bmp_svg == null) return;
                g.Image(bmp_svg, it.rect);
                if (!image_cache.TryAdd(it.text, bmp_svg)) bmp_svg.Dispose();
            }
        }
        void PaintItemTextSvg(Canvas g, TextChatItem text, SolidBrush fore, SolidBrush selection, CacheFont it)
        {
            var id = it.i.ToString() + it.text.Length;
            if (text.image_cache.TryGetValue(id, out var bmp)) g.Image(it.rect, bmp, TFit.Cover, 0, false);
            else
            {
                var bmp_svg = SvgExtend.SvgToBmp(it.text, g);
                if (bmp_svg == null) return;
                g.Image(it.rect, bmp_svg, TFit.Cover, 0, false);
                if (!text.image_cache.TryAdd(id, bmp_svg)) bmp_svg.Dispose();
            }
        }
        void PaintItemTextBasee64(Canvas g, TextChatItem text, SolidBrush fore, SolidBrush selection, CacheFont it)
        {
            var id = it.i.ToString() + it.text.Length;
            if (text.image_cache.TryGetValue(id, out var bmp)) g.Image(it.rect, bmp, TFit.Contain, 0, false);
            else
            {
                using (var ms = new MemoryStream(Convert.FromBase64String(it.text.Substring(it.text.IndexOf(";base64,") + 8))))
                {
                    var bmp_base64 = Image.FromStream(ms);
                    g.Image(it.rect, bmp_base64, TFit.Contain, 0, false);
                    if (!text.image_cache.TryAdd(id, bmp_base64)) bmp_base64.Dispose();
                }
            }
        }

        #endregion

        public ChatList() { ScrollBar = new ScrollBar(this); }

        protected override void Dispose(bool disposing)
        {
            ScrollBar.Dispose();
            if (items != null)
            {
                foreach (var it in items) it.Dispose();
            }
            DisposeCache();
            base.Dispose(disposing);
        }

        #endregion

        #region 鼠标

        IChatItem? mouseDown;
        int mouseDT = 0;
        Point oldMouseDown;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (ScrollBar.MouseDown(e.X, e.Y))
            {
                if (items == null || items.Count == 0) return;
                Focus();
                int scrolly = ScrollBar.Value;
                foreach (IChatItem it in Items)
                {
                    if (it.show && it.Contains(e.Location, 0, scrolly))
                    {
                        mouseDT = 0;
                        mouseDown = it;
                        if (it is TextChatItem text)
                        {
                            text.SelectionLength = 0;
                            if (text.ContainsReadIcon(e.X, e.Y, 0, scrolly)) mouseDT = 2;
                            else if (text.ContainsRead(e.X, e.Y, 0, scrolly))
                            {
                                mouseDownMove = false;
                                mouseDT = 1;
                                oldMouseDown = e.Location;
                                text.SelectionStart = GetCaretPostion(text, e.X, e.Y + scrolly);
                            }
                        }
                        OnItemClick(it, e);
                    }
                    else if (it is TextChatItem text) text.SelectionLength = 0;
                }
            }
        }

        bool mouseDownMove = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int scrolly = ScrollBar.Value;
            if (mouseDT == 1 && mouseDown is TextChatItem textChatItem)
            {
                FocusedChatItem = textChatItem;
                if (ShowTimeFocused) Invalidate(textChatItem.rect);
                int cx = e.X - oldMouseDown.X, cy = e.Y - oldMouseDown.Y;
                if (mouseDownMove) OnTextChatItemMove(textChatItem, oldMouseDown.X + cx, oldMouseDown.Y + scrolly + cy);
                else if (e.Button == MouseButtons.Left)
                {
                    int threshold = (int)(Config.TouchThreshold * Dpi);
                    if (Math.Abs(cx) > threshold || Math.Abs(cy) > threshold)
                    {
                        OnTextChatItemMove(textChatItem, oldMouseDown.X + cx, oldMouseDown.Y + scrolly + cy);
                        mouseDownMove = true;
                    }
                    else return;
                }
                SetCursor(CursorType.IBeam);
            }
            else if (ScrollBar.MouseMove(e.X, e.Y))
            {
                if (items == null || items.Count == 0) return;
                int count = 0, hand = 0, ibeam = 0;
                foreach (IChatItem it in Items)
                {
                    if (it.show && it.Contains(e.Location, 0, scrolly))
                    {
                        count++;
                        if (it is TextChatItem text)
                        {
                            FocusedChatItem = text;
                            if (text.ContainsReadIcon(e.X, e.Y, 0, scrolly)) hand++;
                            else if (text.ContainsRead(e.X, e.Y, 0, scrolly))
                            {
                                if (EnabledClickImage)
                                {
                                    var bmp = text.ContainsReadImage(e.X, e.Y, 0, scrolly);
                                    if (bmp == null) ibeam++;
                                    else hand++;
                                }
                                else ibeam++;
                            }
                        }
                    }
                }
                if (hand > 0) SetCursor(true);
                else if (ibeam > 0) SetCursor(CursorType.IBeam);
                else SetCursor(false);
                if (count > 0) Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mouseDown == null)
            {
                mouseDownMove = false;
                mouseDT = 0;
                return;
            }
            if (mouseDown is TextChatItem text) OnTextChatItemClick(e, text, e.X, e.Y);
            mouseDown = null;
            mouseDownMove = false;
            mouseDT = 0;
            ScrollBar.MouseUp();
        }

        #region 鼠标交互

        protected virtual void OnTextChatItemMove(TextChatItem text, int x, int y)
        {
            var index = GetCaretPostion(text, x, y);
            if (text.selectionStart == index) text.SelectionLength = 0;
            else if (index > text.selectionStart)
            {
                text.SelectionLength = Math.Abs(index - text.selectionStart);
                text.SelectionStart = text.selectionStart;
            }
            else
            {
                text.SelectionLength = Math.Abs(index - text.selectionStart);
                text.SelectionStart = index;
            }
            Invalidate();
        }
        protected virtual void OnTextChatItemClick(MouseEventArgs e, TextChatItem text, int x, int y)
        {
            int scrolly = ScrollBar.Value;
            if (mouseDownMove) OnTextChatItemMove(text, x, y + scrolly);
            else if (mouseDT == 2)
            {
                if (text.ContainsReadIcon(e.X, e.Y, 0, scrolly)) OnItemIconClick(text, e);
            }
            else if (EnabledClickImage && mouseDT == 1)
            {
                var bmp = text.ContainsReadImage(x, y, 0, scrolly);
                if (bmp == null) return;
                var id = bmp.i.ToString() + bmp.text.Length;
                if (text.image_cache.TryGetValue(id, out var img))
                {
                    var bmpup = OnItemImageClick(text, e, img);
                    if (bmpup != null)
                    {
                        text.image_cache[id].Dispose();
                        text.image_cache[id] = bmpup;
                        Invalidate();
                    }
                    return;
                }
            }
        }

        #endregion

        protected override void OnMouseLeave(EventArgs e)
        {
            ScrollBar.Leave();
            SetCursor(false);
            base.OnMouseLeave(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            ILeave();
            base.OnLostFocus(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            ILeave();
            base.OnLeave(e);
        }

        void ILeave()
        {
            ScrollBar.Leave();
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (IChatItem it in Items)
            {
                if (it is TextChatItem text) text.SelectionLength = 0;
            }
            if (count > 0) Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 单击时发生
        /// </summary>
        [Description("单击时发生"), Category("行为")]
        public event ClickEventHandler? ItemClick;

        /// <summary>
        /// 消息头像单击时发生
        /// </summary>
        [Description("消息头像单击时发生"), Category("行为")]
        public event ClickEventHandler? ItemIconClick;

        /// <summary>
        /// 消息图片单击时发生
        /// </summary>
        [Description("消息图片单击时发生"), Category("行为")]
        public event ClickImageEventHandler? ItemImageClick;

        protected virtual void OnItemClick(IChatItem item, MouseEventArgs e) => ItemClick?.Invoke(this, new ChatItemEventArgs(item, e));
        protected virtual void OnItemIconClick(IChatItem item, MouseEventArgs e) => ItemIconClick?.Invoke(this, new ChatItemEventArgs(item, e));
        protected virtual Image? OnItemImageClick(IChatItem item, MouseEventArgs e, Image img)
        {
            if (ItemImageClick == null) return null;
            var arge = new ChatItemImageClickEventArgs(item, e, img);
            ItemImageClick(this, new ChatItemImageClickEventArgs(item, e, img));
            return arge.ImageUpdated;
        }

        #endregion

        #region 键盘

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.A:
                    SelectAll();
                    break;
                case Keys.Control | Keys.C:
                    Copy();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void SelectAll()
        {
            foreach (IChatItem it in Items)
            {
                if (it is TextChatItem text && text.SelectionLength > 0)
                {
                    text.SelectionStart = 0;
                    text.selectionLength = text.cache_font.Length;
                    return;
                }
            }
        }

        void Copy()
        {
            foreach (IChatItem it in Items)
            {
                if (it is TextChatItem text && text.SelectionLength > 0)
                {
                    var _text = GetSelectionText(text);
                    if (_text == null) return;
                    this.ClipboardSetText(_text);
                    return;
                }
            }
        }
        string? GetSelectionText(TextChatItem text)
        {
            if (text.cache_font == null) return null;
            else
            {
                if (text.selectionLength > 0)
                {
                    int start = text.selectionStart, end = text.selectionLength;
                    int end_temp = start + end;
                    var texts = new List<string>(end);
                    foreach (var it in text.cache_font)
                    {
                        if (it.i >= start && end_temp > it.i) texts.Add(it.text);
                    }
                    return string.Join("", texts);
                }
                return null;
            }
        }

        #endregion

        #region 文本

        /// <summary>
        /// 通过坐标系查找光标位置
        /// </summary>
        int GetCaretPostion(TextChatItem item, int x, int y)
        {
            foreach (var it in item.cache_font)
            {
                if (it.rect.X <= x && it.rect.Right >= x && it.rect.Y <= y && it.rect.Bottom >= y)
                {
                    if (x > it.rect.X + it.rect.Width / 2) return it.i + 1;
                    else return it.i;
                }
            }
            var nearest = FindNearestFont(x, y, item.cache_font);
            if (nearest == null)
            {
                if (x > item.cache_font[item.cache_font.Length - 1].rect.Right) return item.cache_font.Length;
                else return 0;
            }
            else
            {
                if (x > nearest.rect.X + nearest.rect.Width / 2) return nearest.i + 1;
                else return nearest.i;
            }
        }

        /// <summary>
        /// 寻找最近的矩形和距离的辅助方法
        /// </summary>
        CacheFont? FindNearestFont(int x, int y, CacheFont[] cache_font)
        {
            CacheFont first = cache_font[0], last = cache_font[cache_font.Length - 1];
            if (x < first.rect.X && y < first.rect.Y) return first;
            else if (x > last.rect.X && y > last.rect.Y) return last;
            double minDistance = int.MaxValue;
            CacheFont? result = null;
            for (int i = 0; i < cache_font.Length; i++)
            {
                var it = cache_font[i];
                // 计算点到矩形四个边的最近距离，取最小值作为当前矩形的最近距离
                int distanceToLeft = Math.Abs(x - (it.rect.Left + it.rect.Width / 2)),
                    distanceToTop = Math.Abs(y - (it.rect.Top + it.rect.Height / 2));
                double currentMinDistance = new int[] { distanceToLeft, distanceToTop }.Average();

                // 如果当前矩形的最近距离比之前找到的最近距离小，更新最近距离和最近矩形信息
                if (currentMinDistance < minDistance)
                {
                    minDistance = currentMinDistance;
                    result = it;
                }
            }
            return result;
        }

        #endregion

        #region 布局

        protected override void OnFontChanged(EventArgs e)
        {
            font_dir.Clear();
            base.OnFontChanged(e);
            LoadLayout();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            LoadLayout();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout();
        }

        bool CanLayout()
        {
            if (IsHandleCreated)
            {
                var rect = ClientRectangle;
                if (items == null || items.Count == 0 || rect.Width == 0 || rect.Height == 0) return false;
                return true;
            }
            return false;
        }

        int oldy = 0;
        internal void LoadLayout(bool print = false)
        {
            if (CanLayout())
            {
                var rect = ClientRectangle;
                int y = Helper.GDI(g =>
                {
                    int y = 0;
                    var size = g.MeasureString(Config.NullText, Font).Height;
                    int item_height = (int)Math.Ceiling(size * 1.714),
                        gap = (int)Math.Round(item_height * BubbleGap),
                        itemGap = (int)(ItemGap * Dpi),
                        spilt = item_height - gap, spilt2 = spilt * 2, max_width = (int)(rect.Width * .8F) - item_height;
                    y = spilt;
                    foreach (var it in items!)
                    {
                        it.PARENT = this;
                        if (it is TextChatItem text) y += text.SetRect(rect, y, g, Font, FixFontWidth(g, Font, text, max_width, spilt2), size, spilt, spilt2, item_height, IconLess) + gap + itemGap;
                    }
                    return y;
                });
                oldy = y;
                ScrollBar.SetVrSize(y);
                ScrollBar.SizeChange(rect);
            }
            if (print) Invalidate();
        }
        internal void LoadLayout(TextChatItem chatItem, bool print = false)
        {
            if (CanLayout())
            {
                int index = items!.IndexOf(chatItem);
                if (index == items.Count - 1)
                {
                    var rect = ClientRectangle;
                    int old = chatItem.rect.Height;
                    Helper.GDI(g =>
                    {
                        var size = g.MeasureString(Config.NullText, Font).Height;
                        int item_height = (int)Math.Ceiling(size * 1.714),
                            gap = (int)Math.Round(item_height * BubbleGap),
                            itemGap = (int)(ItemGap * Dpi),
                            spilt = item_height - gap, spilt2 = spilt * 2, max_width = (int)(rect.Width * .8F) - item_height;

                        int newh = chatItem.SetRect(rect, chatItem.rect.Y, g, Font, FixFontWidth(g, Font, chatItem, max_width, spilt2), size, spilt, spilt2, item_height, IconLess);
                        if (old == newh) return;
                        else
                        {
                            int y = oldy + (newh - old);
                            oldy = y;
                            ScrollBar.SetVrSize(y);
                        }
                    });
                }
                else
                {
                    LoadLayout(print);
                    return;
                }
            }
            if (print) Invalidate();
        }

        #region 字体

        ConcurrentDictionary<string, Size> font_dir = new ConcurrentDictionary<string, Size>();

        public void ClearCache() => font_dir.Clear();

        internal Size FixFontWidth(Canvas g, Font Font, TextChatItem item, int max_width, int spilt)
        {
            item.HasEmoji = false;
            int font_height = 0;
            var font_widths = new List<CacheFont>(item.Text.Length);
            GraphemeSplitter.EachT(item.Text, 0, (str, type, nStart, nLen, nType) =>
            {
                string it = str.Substring(nStart, nLen);
                switch (type)
                {
                    case GraphemeSplitter.STRE_TYPE.BASE64IMG:
                        var id_base64 = nStart.ToString() + nLen;
                        if (item.image_cache.TryGetValue(id_base64, out var bmp_base64))
                        {
                            int imgWidth = bmp_base64.Width, imgHeight = bmp_base64.Height;
                            if (imgWidth > max_width)
                            {
                                float scaleRatio = (float)max_width / imgWidth;
                                imgWidth = max_width;
                                imgHeight = (int)(imgHeight * scaleRatio);
                            }
                            font_widths.Add(new CacheFont(it, false, imgWidth, type).SetImageHeight(imgHeight));
                        }
                        else
                        {
                            using (var ms = new MemoryStream(Convert.FromBase64String(it.Substring(it.IndexOf(";base64,") + 8))))
                            {
                                var image = Image.FromStream(ms);
                                int imgWidth = image.Width, imgHeight = image.Height;
                                if (imgWidth > max_width)
                                {
                                    float scaleRatio = (float)max_width / imgWidth;
                                    imgWidth = max_width;
                                    imgHeight = (int)(imgHeight * scaleRatio);
                                }
                                font_widths.Add(new CacheFont(it, false, imgWidth, type).SetImageHeight(imgHeight));
                                if (!item.image_cache.TryAdd(id_base64, image)) image.Dispose();
                            }
                        }
                        break;
                    case GraphemeSplitter.STRE_TYPE.SVG:
                        var id_svg = nStart.ToString() + nLen;
                        if (item.image_cache.TryGetValue(id_svg, out var bmp_svg))
                        {
                            int svgWidth = bmp_svg.Width, svgHeight = bmp_svg.Height;
                            font_widths.Add(new CacheFont(it, false, svgWidth, type).SetImageHeight(svgHeight));
                        }
                        else
                        {
                            var svgImage = SvgExtend.SvgToBmp(it, font_height, font_height, null);
                            if (svgImage != null)
                            {
                                int svgWidth = svgImage.Width, svgHeight = svgImage.Height;
                                font_widths.Add(new CacheFont(it, false, svgWidth, type).SetImageHeight(svgHeight));
                                if (!item.image_cache.TryAdd(id_svg, svgImage)) svgImage.Dispose();
                            }
                        }
                        break;
                    default:
                        if (nType == 18 || (nType == 4 && SvgDb.Emoji.ContainsKey(it)))
                        {
                            item.HasEmoji = true;
                            font_widths.Add(new CacheFont(it, true, 0, type));
                        }
                        else
                        {
                            if (it == "\t" || it == "\n" || it == "\r\n")
                            {
                                var sizefont = FixFontWidth(g);
                                if (font_height < sizefont.Height) font_height = sizefont.Height;
                                font_widths.Add(new CacheFont(it, false, sizefont.Width, type));
                            }
                            else
                            {
                                var sizefont = FixFontWidth(g, it);
                                if (font_height < sizefont.Height) font_height = sizefont.Height;
                                font_widths.Add(new CacheFont(it, false, sizefont.Width, type));
                            }
                        }
                        break;
                }
                return true;
            });

            if (item.HasEmoji)
            {
                if (font_height == 0) font_height = FixFontWidth(g).Height;
                int tmp = (int)(font_height * EmojiRatio);
                foreach (var it in font_widths)
                {
                    if (it.emoji) it.width = tmp;
                }
            }

            for (int j = 0; j < font_widths.Count; j++) { font_widths[j].i = j; }
            item.cache_font = font_widths.ToArray();

            int usex = 0, usey = 0, oy = 0, maxx = 0, maxy = 0, tmp_font_height = font_height;
            if (item.HasEmoji)
            {
                tmp_font_height = (int)(font_height * EmojiRatio);
                oy = (tmp_font_height - font_height) / 2;
            }
            int? tmpimgsize = null;
            foreach (var it in item.cache_font)
            {
                if (it.text == "\r")
                {
                    it.retun = true;
                    continue;
                }
                if (it.text == "\n" || it.text == "\r\n")
                {
                    it.retun = true;
                    usey += tmp_font_height;
                    usex = 0;
                    continue;
                }
                else if (usex + it.width > max_width)
                {
                    if (tmpimgsize.HasValue)
                    {
                        usey += tmpimgsize.Value;
                        tmpimgsize = null;
                    }
                    else usey += tmp_font_height;
                    usex = 0;
                }
                if (it.imageHeight.HasValue)
                {
                    it.rect = new Rectangle(usex, usey + oy, it.width, it.imageHeight.Value);
                    tmpimgsize = it.imageHeight.Value;
                }
                else it.rect = new Rectangle(usex, usey + oy, it.width, tmp_font_height);
                if (maxx < it.rect.Right) maxx = it.rect.Right;
                if (maxy < it.rect.Bottom) maxy = it.rect.Bottom;
                usex += it.width;
            }

            return new Size(maxx + spilt, maxy + spilt);
        }

        Size FixFontWidth(Canvas g)
        {
            string text = " ";
            if (font_dir.TryGetValue(text, out var sizefont)) return sizefont;
            else
            {
                var size = g.MeasureString(text, Font);
                size.Width = (int)Math.Ceiling(size.Width * 8F);
                font_dir.TryAdd(text, size);
                return size;
            }
        }
        Size FixFontWidth(Canvas g, string text)
        {
            if (font_dir.TryGetValue(text, out var sizefont)) return sizefont;
            else
            {
                var size = g.MeasureString(text, Font);
                font_dir.TryAdd(text, size);
                return size;
            }
        }

        #endregion

        #endregion
    }

    public class ChatItemCollection : iCollection<IChatItem>
    {
        public ChatItemCollection(ChatList it)
        {
            BindData(it);
        }

        internal ChatItemCollection BindData(ChatList it)
        {
            action = render =>
            {
                if (render) it.LoadLayout(true);
                else it.Invalidate();
            };
            return this;
        }
    }

    public class TextChatItem : IChatItem
    {
        public TextChatItem(string text)
        {
            _text = text;
        }
        public TextChatItem(string text, Image? icon)
        {
            _text = text;
            _icon = icon;
        }
        public TextChatItem(string text, Image? icon, string name)
        {
            _text = text;
            _name = name;
            _icon = icon;
        }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        /// <summary>
        /// 本人
        /// </summary>
        [Description("本人"), Category("行为"), DefaultValue(false)]
        public bool Me { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Description("时间"), Category("数据"), DefaultValue(null)]
        public string? Time { get; set; }

        Image? _icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
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
        [Description("名称"), Category("外观"), DefaultValue(null)]
        public string? Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                Invalidate();
            }
        }

        string _text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观")]
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                DisposeCache();
                if (loading) PARENT?.LoadLayout(this, true);
                else Invalidates();
            }
        }

        AnimationTask? task;
        internal bool showlinedot = false;
        bool loading;
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
                if (value && PARENT != null)
                {
                    task = new AnimationTask(new AnimationLoopConfig(PARENT, () =>
                    {
                        showlinedot = !showlinedot;
                        Invalidate();
                        return loading;
                    }, 200).SetPriority());
                }
                else Invalidate();
            }
        }

        #region 布局

        internal int SetRect(Rectangle _rect, int y, Canvas g, Font font, Size msglen, int gap, int spilt, int spilt2, int image_size, bool iconLess = false)
        {
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
                    rect_name = new Rectangle(rect_icon.X - spilt - msglen.Width + msglen.Width - size_name, rect_icon.Y, size_name, gap);
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

            foreach (var it in cache_font) it.SetOffset(rect_text.X, rect_text.Y);
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

        internal bool ContainsRead(int x, int y, int sx, int sy) => rect_text.Contains(new Point(x + sx, y + sy));
        internal bool ContainsReadIcon(int x, int y, int sx, int sy) => rect_icon.Contains(new Point(x + sx, y + sy));
        internal CacheFont? ContainsReadImage(int x, int y, int sx, int sy)
        {
            foreach (var it in cache_font)
            {
                switch (it.type)
                {
                    case GraphemeSplitter.STRE_TYPE.STR:
                        continue;
                    case GraphemeSplitter.STRE_TYPE.SVG:
                    case GraphemeSplitter.STRE_TYPE.BASE64IMG:
                        if (it.rect.Contains(new Point(x + sx, y + sy))) return it;
                        break;
                }
            }
            return null;
        }

        #endregion

        #region 字体相关

        internal bool HasEmoji = false;
        internal CacheFont[] cache_font = new CacheFont[0];

        internal int selectionStart = 0, selectionStartTemp = 0, selectionLength = 0;
        /// <summary>
        /// 所选文本的起点
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int SelectionStart
        {
            get => selectionStart;
            set
            {
                if (value < 0) value = 0;
                else if (value > 0)
                {
                    if (cache_font == null) value = 0;
                    else if (value > cache_font.Length) value = cache_font.Length;
                }
                if (selectionStart == value) return;
                selectionStart = selectionStartTemp = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 所选文本的长度
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int SelectionLength
        {
            get => selectionLength;
            set
            {
                if (selectionLength == value) return;
                selectionLength = value;
                Invalidate();
            }
        }

        #endregion

        internal ConcurrentDictionary<string, Image> image_cache = new ConcurrentDictionary<string, Image>();

        #region 设置

        public TextChatItem SetID(string? value)
        {
            ID = value;
            return this;
        }

        public TextChatItem SetName(string? value)
        {
            _name = value;
            return this;
        }

        public TextChatItem SetIcon(Image? img)
        {
            _icon = img;
            return this;
        }

        public TextChatItem SetLoading(bool value = true)
        {
            loading = value;
            return this;
        }

        public TextChatItem SetMe(bool value = true)
        {
            Me = value;
            return this;
        }

        public TextChatItem SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        public TextChatItem SetTime(DateTime value)
        {
            if (value.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")) Time = value.ToString("HH:mm:ss");
            else Time = value.ToString("yyyy-MM-dd HH:mm:ss");
            return this;
        }
        public TextChatItem SetTime(string? value)
        {
            Time = value;
            return this;
        }

        #endregion

        public void DisposeCache()
        {
            if (image_cache.Count == 0) return;
            foreach (var it in image_cache.Keys)
            {
                if (image_cache.TryRemove(it, out var bmp)) bmp.Dispose();
            }
        }

        public override void Dispose()
        {
            DisposeCache();
            base.Dispose();
        }
    }

    public abstract class IChatItem : IDisposable
    {
        public IChatItem() { }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        public void Invalidate() => PARENT?.Invalidate();
        public void Invalidates() => PARENT?.LoadLayout(true);

        internal bool show { get; set; }

        public ChatList? PARENT { get; set; }

        public Rectangle rect { get; set; }

        public bool Contains(Point point, int x, int y) => rect.Contains(new Point(point.X + x, point.Y + y));

        public virtual void Dispose()
        {
        }
    }

    internal class CacheFont
    {
        public CacheFont(string _text, bool _emoji, int _width, GraphemeSplitter.STRE_TYPE Type)
        {
            text = _text;
            emoji = _emoji;
            width = _width;
            type = Type;
        }
        public int i { get; set; }
        public string text { get; set; }
        Rectangle _rect;
        public Rectangle rect
        {
            get => _rect;
            set { _rect = value; }
        }
        internal void SetOffset(int x, int y) => _rect.Offset(x, y);
        public bool emoji { get; set; }
        public bool retun { get; set; }
        public int width { get; set; }
        public GraphemeSplitter.STRE_TYPE type { get; set; }
        public int? imageHeight { get; set; }

        public CacheFont SetImageHeight(int? value)
        {
            imageHeight = value;
            return this;
        }
    }
}