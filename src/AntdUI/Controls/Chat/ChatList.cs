// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        [Description("Emoji字体"), Category("外观"), DefaultValue("Segoe UI Emoji")]
        public string EmojiFont { get; set; } = "Segoe UI Emoji";

        #endregion

        #region 方法

        public void AddToBottom(IChatItem it)
        {
            Items.Add(it);
            scroll.Value = scroll.VrValueI;
        }

        public bool AddIsBottom(IChatItem it)
        {
            if (scroll.Show)
            {
                bool isbutt = scroll.Value == scroll.VrValueI;
                Items.Add(it);
                if (isbutt) scroll.Value = scroll.VrValueI;
                return isbutt;
            }
            else { Items.Add(it); return true; }
        }

        public void ToBottom()
        {
            scroll.Value = scroll.VrValueI;
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;

            var g = e.Graphics.High();
            float sy = scroll.Value, radius = Config.Dpi * 8F;
            g.TranslateTransform(0, -sy);

            foreach (IChatItem it in items) PaintItem(g, it, rect, sy, radius);

            g.ResetTransform();
            scroll.Paint(g);
            base.OnPaint(e);
        }

        StringFormat SFL = Helper.SF(tb: StringAlignment.Near);

        void PaintItem(Graphics g, IChatItem it, Rectangle rect, float sy, float radius)
        {
            it.show = it.Show && it.rect.Y > sy - rect.Height - it.rect.Height && it.rect.Bottom < scroll.Value + scroll.ReadSize + it.rect.Height;
            if (it.show)
            {
                if (it is TextChatItem text)
                {
                    using (var path = text.rect_read.RoundPath(radius))
                    {
                        using (var brush = new SolidBrush(Style.Db.TextTertiary))
                        {
                            g.DrawString(text.Name, Font, brush, text.rect_name, SFL);
                        }
                        if (text.Me)
                        {
                            using (var brush = new SolidBrush(Color.FromArgb(0, 153, 255)))
                            {
                                g.FillPath(brush, path);
                            }
                            if (text.selectionLength > 0)
                            {
                                using (var brush = new SolidBrush(Color.FromArgb(0, 134, 224)))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            using (var brush = new SolidBrush(Color.White))
                            {
                                PaintItemText(g, text, brush);
                            }
                        }
                        else
                        {
                            using (var brush = new SolidBrush(Color.White))
                            {
                                g.FillPath(brush, path);
                            }
                            if (text.selectionLength > 0)
                            {
                                using (var brush = new SolidBrush(Style.Db.FillQuaternary))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            using (var brush = new SolidBrush(Color.Black))
                            {
                                PaintItemText(g, text, brush);
                            }
                        }
                    }
                    if (text.Icon != null) g.PaintImg(text.rect_icon, text.Icon, TFit.Cover, 0, true);

                }
            }
        }

        void PaintItemText(Graphics g, TextChatItem text, SolidBrush fore)
        {
            if (text.selectionLength > 0)
            {
                int end = text.selectionStartTemp + text.selectionLength - 1;
                if (end > text.cache_font.Length - 1) end = text.cache_font.Length - 1;
                CacheFont first = text.cache_font[text.selectionStartTemp];
                using (var brush = new SolidBrush(Color.FromArgb(text.Me ? 255 : 60, 96, 165, 250)))
                {
                    for (int i = text.selectionStartTemp; i <= end; i++)
                    {
                        var last = text.cache_font[i];
                        if (i == end) g.FillRectangle(brush, new Rectangle(first.rect.X, first.rect.Y, last.rect.Right - first.rect.X, first.rect.Height));
                        else if (first.rect.Y != last.rect.Y || last.retun)
                        {
                            last = text.cache_font[i - 1];
                            g.FillRectangle(brush, new Rectangle(first.rect.X, first.rect.Y, last.rect.Right - first.rect.X, first.rect.Height));
                            first = text.cache_font[i];
                        }
                    }
                }
            }
            if (text.HasEmoji)
            {
                using (var font = new Font(EmojiFont, Font.Size))
                {
                    foreach (var itt in text.cache_font)
                    {
                        if (itt.svgImage != null)
                        {
                            g.PaintImg(itt.rect, itt.svgImage, TFit.Cover, 0, false);
                        }
                        else if (itt.isImage) // 检查是否是图片
                        {
                            g.PaintImg(itt.rect, itt.image, TFit.Contain, 0, false);
                        }
                        else if (itt.emoji)
                        {
                            g.DrawString(itt.text, font, fore, itt.rect, m_sf);
                        }
                        else
                        {
                            g.DrawString(itt.text, Font, fore, itt.rect, m_sf);
                        }
                    }
                }
            }
            else
            {
                foreach (var itt in text.cache_font)
                {
                    if (itt.svgImage != null)
                    {
                        g.PaintImg(itt.rect, itt.svgImage, TFit.Cover, 0, false);
                    }
                    else if (itt.isImage) // 检查是否是图片
                    {
                        g.PaintImg(itt.rect, itt.image, TFit.Contain, 0, false);
                    }
                    else
                    {
                        g.DrawString(itt.text, Font, fore, itt.rect, m_sf);
                    }
                }
            }

            if (text.showlinedot)
            {
                int size = (int)(2 * Config.Dpi), w = size * 3;
                if (text.cache_font.Length > 0)
                {
                    var rect = text.cache_font[text.cache_font.Length - 1].rect;
                    using (var brush = new SolidBrush(Color.FromArgb(0, 153, 255)))
                    {
                        g.FillRectangle(brush, new Rectangle(rect.Right - w / 2, rect.Bottom - size, w, size));
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(Color.FromArgb(0, 153, 255)))
                    {
                        g.FillRectangle(brush, new Rectangle(text.rect_read.X + (text.rect_read.Width - w) / 2, text.rect_read.Bottom - size, w, size));
                    }
                }
            }

        }

        internal ScrollBar scroll;
        public ChatList() { scroll = new ScrollBar(this); }

        protected override void Dispose(bool disposing)
        {
            scroll.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region 鼠标

        TextChatItem? mouseDown = null;
        Point oldMouseDown;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (scroll.MouseDown(e.Location))
            {
                if (items == null || items.Count == 0) return;
                Focus();
                int scrolly = scroll.Value;
                foreach (IChatItem it in Items)
                {
                    if (it.show && it.Contains(e.Location, 0, scrolly))
                    {
                        if (it is TextChatItem text)
                        {
                            text.SelectionLength = 0;
                            if (e.Button == MouseButtons.Left && text.ContainsRead(e.Location, 0, scrolly))
                            {
                                oldMouseDown = e.Location;
                                text.SelectionStart = GetCaretPostion(text, e.Location.X, e.Location.Y + scrolly);

                                mouseDown = text;
                            }
                        }
                        ItemClick?.Invoke(this, e, it);
                    }
                    else if (it is TextChatItem text) text.SelectionLength = 0;
                }
            }
        }

        bool mouseDownMove = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int scrolly = scroll.Value;
            if (mouseDown != null)
            {
                mouseDownMove = true;
                Cursor = Cursors.IBeam;
                var index = GetCaretPostion(mouseDown, oldMouseDown.X + (e.Location.X - oldMouseDown.X), oldMouseDown.Y + scrolly + (e.Location.Y - oldMouseDown.Y));
                mouseDown.SelectionLength = Math.Abs(index - mouseDown.selectionStart);
                if (index > mouseDown.selectionStart) mouseDown.selectionStartTemp = mouseDown.selectionStart;
                else mouseDown.selectionStartTemp = index;
                Invalidate();
            }
            else if (scroll.MouseMove(e.Location))
            {
                if (items == null || items.Count == 0) return;
                int count = 0, hand = 0, ibeam = 0;
                foreach (IChatItem it in Items)
                {
                    if (it.show && it.Contains(e.Location, 0, scrolly))
                    {
                        if (it is TextChatItem text)
                        {
                            if (text.ContainsRead(e.Location, 0, scrolly)) ibeam++;
                        }
                        //if (it.Contains(e.Location, 0, (int)scrollY.Value, out var change))
                        //{
                        //    hand++;
                        //}
                        //if (change) count++;
                    }
                }
                if (ibeam > 0) Cursor = Cursors.IBeam;
                else SetCursor(hand > 0);
                if (count > 0) Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mouseDown != null && mouseDownMove)
            {
                int scrolly = scroll.Value;
                var index = GetCaretPostion(mouseDown, e.Location.X, e.Location.Y + scrolly);
                if (mouseDown.selectionStart == index) mouseDown.SelectionLength = 0;
                else if (index > mouseDown.selectionStart)
                {
                    mouseDown.SelectionLength = Math.Abs(index - mouseDown.selectionStart);
                    mouseDown.SelectionStart = mouseDown.selectionStart;
                }
                else
                {
                    mouseDown.SelectionLength = Math.Abs(index - mouseDown.selectionStart);
                    mouseDown.SelectionStart = index;
                }
                Invalidate();
            }
            mouseDown = null;
            scroll.MouseUp();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scroll.Leave();
            SetCursor(false);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            ILeave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ILeave();
        }

        void ILeave()
        {
            scroll.Leave();
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
            scroll.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="args">点击</param>
        /// <param name="item">消息框</param>
        public delegate void ClickEventHandler(object sender, MouseEventArgs args, IChatItem item);

        /// <summary>
        /// 单击时发生
        /// </summary>
        [Description("单击时发生"), Category("行为")]
        public event ClickEventHandler? ItemClick;

        #endregion

        #region 键盘

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.A:
                    SelectAll();
                    return true;
                case Keys.Control | Keys.C:
                    Copy();
                    return true;
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
                    Clipboard.SetText(_text);
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
            var rect = ChangeList();
            scroll.SizeChange(rect);
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ChangeList();
            scroll.SizeChange(rect);
            base.OnSizeChanged(e);
        }

        StringFormat m_sf = Helper.SF_MEASURE_FONT();
        internal Rectangle ChangeList()
        {
            var rect = ClientRectangle;
            if (items == null || items.Count == 0) return rect;
            if (rect.Width == 0 || rect.Height == 0) return rect;

            int y = 0;
            Helper.GDI(g =>
            {
                var size = (int)Math.Ceiling(g.MeasureString(Config.NullText, Font).Height);
                int item_height = (int)Math.Ceiling(size * 1.714),
                    gap = (int)Math.Round(item_height * 0.75),
                    spilt = item_height - gap, spilt2 = spilt * 2, max_width = (int)(rect.Width * 0.8F) - item_height;
                y = spilt;
                foreach (IChatItem it in items)
                {
                    it.PARENT = this;
                    if (it is TextChatItem text)
                    {
                        y += text.SetRect(rect, y, g, Font, FixFontWidth(g, Font, text, max_width, spilt2), size, spilt, spilt2, item_height) + gap;
                    }
                }
            });
            scroll.SetVrSize(y);
            return rect;
        }

        #region 字体

        internal Size FixFontWidth(Graphics g, Font Font, TextChatItem item, int max_width, int spilt)
        {
            item.HasEmoji = false;
            int font_height = 0;

            var font_widths = new List<CacheFont>(item.Text.Length);
            string base64Pattern = @"data:image/(?<type>.+?);base64,(?<data>[A-Za-z0-9+/=]+)";
            var regex = new Regex(base64Pattern);
            bool iseone = false;
            char[] textChars = item.Text.ToCharArray();
            int i = 0;

            while (i < textChars.Length)
            {
                char it = textChars[i];
                var unicodeInfo = CharUnicodeInfo.GetUnicodeCategory(it);

                string remainingText = item.Text.Substring(i);
                var match = regex.Match(remainingText);
                if (match.Success && match.Index == 0)
                {
                    string base64Data = match.Groups["data"].Value;
                    string imageData = match.Value;
                    byte[] imageBytes = Convert.FromBase64String(base64Data);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        Image image = Image.FromStream(ms);
                        int imgWidth = image.Width;
                        int imgHeight = image.Height;

                        // Adjust image size to fit within max_width if necessary
                        if (imgWidth > max_width)
                        {
                            float scaleRatio = (float)max_width / imgWidth;
                            imgWidth = max_width;
                            imgHeight = (int)(imgHeight * scaleRatio);
                        }

                        font_widths.Add(new CacheFont(imageData, false, imgWidth) { isImage = true, image = image, width = imgWidth });
                        font_height = Math.Max(font_height, imgHeight);
                        i += imageData.Length;
                        continue;
                    }
                }

                // Check if it's an SVG image
                if (it == '<' && textChars.Skip(i).Take(4).SequenceEqual(new[] { '<', 's', 'v', 'g' }))
                {
                    int endIndex = item.Text.IndexOf("</svg>", i, StringComparison.OrdinalIgnoreCase);
                    if (endIndex != -1)
                    {
                        string svgText = item.Text.Substring(i, endIndex - i + 6); // Include the ending tag
                        i = endIndex + 6; // Move past the processed SVG text

                        Image? svgImage = SvgExtend.SvgToBmp(svgText);
                        if (svgImage != null)
                        {
                            int svgWidth = svgImage.Width;
                            int svgHeight = svgImage.Height;
                            if (font_height < svgHeight) font_height = svgHeight;
                            font_widths.Add(new CacheFont(svgText, false, svgWidth, svgImage, _isSvg: true));
                        }
                        continue;
                    }
                }

                // Process other characters
                string txt = it.ToString();
                if (IsEmoji(unicodeInfo))
                {
                    item.HasEmoji = true;
                    if (unicodeInfo == UnicodeCategory.Surrogate)
                    {
                        if (iseone)
                        {
                            iseone = false;
                            font_widths[font_widths.Count - 1].text += txt;
                            continue;
                        }
                        else iseone = true;
                    }
                    else iseone = false;
                    font_widths.Add(new CacheFont(txt, true, 0));
                }
                else
                {
                    iseone = false;
                    if (it == '\t' || it == '\n')
                    {
                        var sizefont = g.MeasureString(" ", Font, 10000, m_sf);
                        if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                    }
                    else
                    {
                        var sizefont = g.MeasureString(txt, Font, 10000, m_sf);
                        if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width)));
                    }
                }

                i++;
            }

            if (item.HasEmoji)
            {
                using (var font = new Font(EmojiFont, Font.Size))
                {
                    foreach (var it in font_widths)
                    {
                        if (it.emoji)
                        {
                            var sizefont = g.MeasureString(it.text, font, 10000, m_sf);
                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                            it.width = (int)Math.Ceiling(sizefont.Width);
                        }
                    }
                }
            }

            for (int j = 0; j < font_widths.Count; j++) { font_widths[j].i = j; }
            item.cache_font = font_widths.ToArray();

            int usex = 0, usey = 0, maxx = 0, maxy = 0;
            foreach (var it in item.cache_font)
            {
                if (it.text == "\r")
                {
                    it.retun = true;
                    continue;
                }
                if (it.text == "\n")
                {
                    it.retun = true;
                    usey += font_height;
                    usex = 0;
                    continue;
                }
                else if (usex + it.width > max_width)
                {
                    usey += font_height;
                    usex = 0;
                }
                it.rect = new Rectangle(usex, usey, it.width, font_height);
                if (maxx < it.rect.Right) maxx = it.rect.Right;
                if (maxy < it.rect.Bottom) maxy = it.rect.Bottom;
                usex += it.width;
            }

            return new Size(maxx + spilt, maxy + spilt);
        }
        bool IsEmoji(UnicodeCategory unicodeInfo)
        {
            //return unicodeInfo == UnicodeCategory.Surrogate;
            return unicodeInfo == UnicodeCategory.Surrogate || unicodeInfo == UnicodeCategory.OtherSymbol ||
                 unicodeInfo == UnicodeCategory.MathSymbol ||
                  unicodeInfo == UnicodeCategory.EnclosingMark ||
                   unicodeInfo == UnicodeCategory.NonSpacingMark ||
                  unicodeInfo == UnicodeCategory.ModifierLetter;
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
                if (render) it.ChangeList();
                it.Invalidate();
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
        public TextChatItem(string text, Bitmap? icon)
        {
            _text = text;
            _icon = icon;
        }
        public TextChatItem(string text, Bitmap? icon, string name)
        {
            _text = text;
            _name = name;
            _icon = icon;
        }

        /// <summary>
        /// 本人
        /// </summary>
        [Description("本人"), Category("行为"), DefaultValue(false)]
        public bool Me { get; set; }

        Image? _icon = null;
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
                OnPropertyChanged("Icon");
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
                OnPropertyChanged("Name");
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
                Invalidates();
            }
        }

        ITask? task;
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
                    task = new ITask(PARENT, () =>
                    {
                        showlinedot = !showlinedot;
                        Invalidate();
                        return loading;
                    }, 200);
                }
                else Invalidate();
            }
        }


        internal int SetRect(Rectangle _rect, int y, Graphics g, Font font, Size msglen, int gap, int spilt, int spilt2, int image_size)
        {
            if (string.IsNullOrEmpty(_name))
            {
                rect = new Rectangle(_rect.X, _rect.Y + y, _rect.Width, msglen.Height);
                if (Me)
                {
                    rect_icon = new Rectangle(rect.Right - gap - image_size, rect.Y, image_size, image_size);
                    rect_read = new Rectangle(rect_icon.X - spilt - msglen.Width, rect_icon.Y, msglen.Width, msglen.Height);
                }
                else
                {
                    rect_icon = new Rectangle(rect.X + gap, rect.Y, image_size, image_size);
                    rect_read = new Rectangle(rect_icon.Right + spilt, rect_icon.Y, msglen.Width, msglen.Height);
                }
            }
            else
            {
                rect = new Rectangle(_rect.X, _rect.Y + y, _rect.Width, msglen.Height + gap);
                var size_name = (int)Math.Ceiling(g.MeasureString(_name, font).Width);
                if (Me)
                {
                    rect_icon = new Rectangle(rect.Right - gap - image_size, rect.Y, image_size, image_size);
                    rect_name = new Rectangle(rect_icon.X - spilt - msglen.Width + msglen.Width - size_name, rect_icon.Y, size_name, gap);
                    rect_read = new Rectangle(rect_icon.X - spilt - msglen.Width, rect_name.Bottom, msglen.Width, msglen.Height);
                }
                else
                {
                    rect_icon = new Rectangle(rect.X + gap, rect.Y, image_size, image_size);
                    rect_name = new Rectangle(rect_icon.Right + spilt, rect_icon.Y, size_name, gap);
                    rect_read = new Rectangle(rect_name.X, rect_name.Bottom, msglen.Width, msglen.Height);
                }
            }
            rect_text = new Rectangle(rect_read.X + spilt, rect_read.Y + spilt, msglen.Width - spilt2, msglen.Height - spilt2);

            foreach (var it in cache_font)
            {
                it.SetOffset(rect_text.Location);
            }

            Show = true;
            return rect.Height;
        }

        internal Rectangle rect_read { get; set; }
        internal Rectangle rect_name { get; set; }
        internal Rectangle rect_text { get; set; }
        internal Rectangle rect_icon { get; set; }

        internal bool ContainsRead(Point point, int x, int y)
        {
            return rect_text.Contains(new Point(point.X + x, point.Y + y));
        }

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
    }
    public class IChatItem : NotifyProperty
    {
        public IChatItem() { }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal void Invalidate()
        {
            PARENT?.Invalidate();
        }
        internal void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ChangeList();
            PARENT.Invalidate();
        }

        internal bool show { get; set; }
        internal bool Show { get; set; }

        internal ChatList? PARENT { get; set; }

        internal Rectangle rect { get; set; }

        internal bool Contains(Point point, int x, int y)
        {
            return rect.Contains(new Point(point.X + x, point.Y + y));
        }
    }

    internal class CacheFont
    {
        public CacheFont(string _text, bool _emoji, int _width, Image? _svgImage = null, bool _isSvg = false)
        {
            text = _text;
            emoji = _emoji;
            width = _width;
            svgImage = _svgImage;
            isSvg = _isSvg;
        }
        public int i { get; set; }
        public string text { get; set; }
        Rectangle _rect;
        public Rectangle rect
        {
            get => _rect;
            set { _rect = value; }
        }
        internal void SetOffset(Point point)
        {
            _rect.Offset(point);
        }
        public bool emoji { get; set; }
        public bool retun { get; set; }
        public int width { get; set; }
        public Image? svgImage { get; set; } // 新增svgImage字段
        public bool isSvg { get; set; } // 新增isSvg字段
        // 新增 isImage 和 image 属性
        public bool isImage { get; set; } = false;
        public Image? image { get; set; } = null;
    }
}
