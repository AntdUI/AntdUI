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
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        protected override void OnCreateControl()
        {
            FixFontWidth(true);
            base.OnCreateControl();
        }

        #region 确定字体宽度

        CacheFont[]? cache_font = null;
        bool HasEmoji = false;
        void FixFontWidth(bool force = false)
        {
            HasEmoji = false;
            if (force)
            {
                Helper.GDI(g =>
                {
                    float dpi = g.DpiX / 96F;
                    int font_height = 0;
                    if (isempty)
                    {
                        ScrollX = ScrollY = 0;
                        font_height = (int)Math.Ceiling(g.MeasureString(Config.NullText, Font, 10000, sf_font).Height);
                        cache_font = null;
                    }
                    else
                    {
                        var font_widths = new List<CacheFont>();
                        if (IsPassWord)
                        {
                            var sizefont = g.MeasureString(PassWordChar, Font, 10000, sf_font);
                            int w = (int)Math.Ceiling(sizefont.Width);
                            font_height = (int)Math.Ceiling(sizefont.Height);
                            foreach (char it in _text) font_widths.Add(new CacheFont(it.ToString(), false, w));
                        }
                        else
                        {
                            GraphemeSplitter.Each(_text, 0, (str, nStart, nLen) =>
                            {
                                string txt = str.Substring(nStart, nLen);
                                var unicodeInfo = CharUnicodeInfo.GetUnicodeCategory(txt[0]);
                                if (IsEmoji(unicodeInfo))
                                {
                                    HasEmoji = true;
                                    font_widths.Add(new CacheFont(txt, true, 0));
                                }
                                else
                                {
                                    if (txt == "\t")
                                    {
                                        var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                                    }
                                    else if (txt == "\n" || txt == "\r\n")
                                    {
                                        var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width)));
                                    }
                                    else
                                    {
                                        var sizefont = g.MeasureString(txt, Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width)));
                                    }
                                }


                                return true;
                            });
                            if (HasEmoji)
                            {
                                using (var font = new Font(EmojiFont, Font.Size))
                                {
                                    foreach (var it in font_widths)
                                    {
                                        if (it.emoji)
                                        {
                                            var sizefont = g.MeasureString(it.text, font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                            it.width = (int)Math.Ceiling(sizefont.Width);
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < font_widths.Count; i++) { font_widths[i].i = i; }
                        cache_font = font_widths.ToArray();
                    }
                    CurrentCaret.Height = font_height;
                    CalculateRect();
                });
            }
            else
            {
                if (isempty)
                {
                    ScrollX = ScrollY = 0;
                    cache_font = null;
                    CalculateRect();
                }
                else
                {
                    Helper.GDI(g =>
                    {
                        int font_height = 0;
                        if (_text == null) return;
                        var font_widths = new List<CacheFont>();
                        if (IsPassWord)
                        {
                            var sizefont = g.MeasureString(PassWordChar, Font, 10000, sf_font);
                            int w = (int)Math.Ceiling(sizefont.Width);
                            font_height = (int)Math.Ceiling(sizefont.Height);
                            foreach (char it in _text) font_widths.Add(new CacheFont(it.ToString(), false, w));
                        }
                        else
                        {
                            var font_dir = new Dictionary<string, CacheFont>();
                            if (cache_font != null)
                            {
                                foreach (var it in cache_font) if (!it.emoji && !font_dir.ContainsKey(it.text)) font_dir.Add(it.text, it);
                            }
                            GraphemeSplitter.Each(_text, 0, (str, nStart, nLen) =>
                            {
                                string txt = str.Substring(nStart, nLen);
                                var unicodeInfo = CharUnicodeInfo.GetUnicodeCategory(txt[0]);

                                if (IsEmoji(unicodeInfo))
                                {
                                    HasEmoji = true;
                                    font_widths.Add(new CacheFont(txt, true, 0));
                                }
                                else
                                {
                                    if (font_dir.TryGetValue(txt, out var find))
                                    {
                                        if (font_height < find.rect.Height) font_height = find.rect.Height;
                                        font_widths.Add(new CacheFont(txt, false, find.width));
                                    }
                                    else
                                    {
                                        if (txt == "\t")
                                        {
                                            var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                            font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                                        }
                                        else if (txt == "\n" || txt == "\r\n")
                                        {
                                            var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                            font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width)));
                                        }
                                        else
                                        {
                                            var sizefont = g.MeasureString(txt, Font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                            font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width)));
                                        }
                                    }
                                }


                                return true;
                            });

                            if (HasEmoji)
                            {
                                using (var font = new Font(EmojiFont, Font.Size))
                                {
                                    foreach (var it in font_widths)
                                    {
                                        if (it.emoji)
                                        {
                                            var sizefont = g.MeasureString(it.text, font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                            it.width = (int)Math.Ceiling(sizefont.Width);
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < font_widths.Count; i++) { font_widths[i].i = i; }
                        cache_font = font_widths.ToArray();
                        CurrentCaret.Height = font_height;
                        CalculateRect();
                    });
                }
            }
        }
        bool IsEmoji(UnicodeCategory unicodeInfo)
        {
            return unicodeInfo == UnicodeCategory.Surrogate || unicodeInfo == UnicodeCategory.OtherSymbol ||
                 unicodeInfo == UnicodeCategory.MathSymbol ||
                  unicodeInfo == UnicodeCategory.EnclosingMark ||
                   unicodeInfo == UnicodeCategory.NonSpacingMark ||
                  unicodeInfo == UnicodeCategory.ModifierLetter;
        }

        class CacheFont
        {
            public CacheFont(string _text, bool _emoji, int _width)
            {
                text = _text;
                emoji = _emoji;
                width = _width;
            }
            public int i { get; set; }
            public string text { get; set; }
            public Rectangle rect_old { get; set; }
            public Rectangle rect { get; set; }
            public bool emoji { get; set; }
            public int retun { get; set; }
            public int width { get; set; }
            internal bool show { get; set; }
        }

        #endregion

        internal virtual bool ModeRange { get => false; }
        internal virtual void ModeRangeCaretPostion(bool Null) { }

        internal Rectangle rect_text, rect_l, rect_r;
        internal Rectangle rect_d_ico, rect_d_l, rect_d_r;
        internal void CalculateRect()
        {
            var rect = ReadRectangle;
            int sps = (int)(CurrentCaret.Height * .4F), sps2 = sps * 2;
            RectAuto(rect, sps, sps2);
            if (cache_font == null)
            {
                if (ModeRange)
                {
                    int center = rect_text.Width / 2;
                    int h2 = CurrentCaret.Height / 2;
                    rect_d_ico = new Rectangle(rect_text.X + center - h2, rect_text.Y + ((rect_text.Height - CurrentCaret.Height) / 2), CurrentCaret.Height, CurrentCaret.Height);
                    rect_d_l = new Rectangle(rect_text.X, rect_text.Y, center - h2, rect_text.Height);
                    rect_d_r = new Rectangle(rect_d_l.Right + CurrentCaret.Height, rect_text.Y, rect_d_l.Width, rect_text.Height);
                }
                CurrentCaret.Location = rect_text.Location;
            }
            else
            {
                if (multiline)
                {
                    int lineHeight = CurrentCaret.Height + (lineheight > 0 ? (int)(lineheight * Config.Dpi) : 0);
                    int usex = 0, usey = 0;
                    foreach (var it in cache_font)
                    {
                        it.show = false;
                        if (it.text == "\r")
                        {
                            it.retun = 2;
                            it.rect = new Rectangle(rect_text.X + usex, rect_text.Y + usey, it.width, CurrentCaret.Height);
                            continue;
                        }
                        else if (it.text == "\n" || it.text == "\r\n")
                        {
                            it.retun = 1;
                            it.rect_old = new Rectangle(rect_text.X + usex, rect_text.Y + usey, it.width, CurrentCaret.Height);
                            usey += lineHeight;
                            usex = 0;
                            it.rect = new Rectangle(rect_text.X + usex, rect_text.Y + usey, it.width, CurrentCaret.Height);
                            continue;
                        }
                        else if (usex + it.width > rect_text.Width)
                        {
                            usey += lineHeight;
                            usex = 0;
                        }
                        it.rect = new Rectangle(rect_text.X + usex, rect_text.Y + usey, it.width, CurrentCaret.Height);
                        usex += it.width;
                    }
                }
                else
                {
                    int usex = 0;
                    if (ModeRange)
                    {
                        int center = rect_text.Width / 2;
                        int h2 = CurrentCaret.Height / 2;
                        rect_d_ico = new Rectangle(rect_text.X + center - h2, rect_text.Y + ((rect_text.Height - CurrentCaret.Height) / 2), CurrentCaret.Height, CurrentCaret.Height);
                        rect_d_l = new Rectangle(rect_text.X, rect_text.Y, center - h2, rect_text.Height);
                        rect_d_r = new Rectangle(rect_d_l.Right + CurrentCaret.Height, rect_text.Y, rect_d_l.Width, rect_text.Height);
                        int GetTabIndex()
                        {
                            foreach (var it in cache_font)
                            {
                                if (it.text == "\t")
                                {
                                    return it.i;
                                }
                            }
                            return -1;
                        }
                        int tabindex = GetTabIndex();
                        List<int> i_l = new List<int>(cache_font.Length), i_r = new List<int>(i_l.Count);
                        if (tabindex == -1)
                        {
                            for (int i = 0; i < cache_font.Length; i++)
                            {
                                var it = cache_font[i];
                                it.show = true;
                                it.rect = new Rectangle(rect_d_l.X + usex, rect_text.Y, it.width, CurrentCaret.Height);
                                usex += it.width;
                                i_l.Add(i);
                            }
                        }
                        else if (tabindex > 0)
                        {
                            for (int i = 0; i < tabindex; i++)
                            {
                                var it = cache_font[i];
                                it.show = true;
                                it.rect = new Rectangle(rect_d_l.X + usex, rect_text.Y, it.width, CurrentCaret.Height);
                                usex += it.width;
                                i_l.Add(i);
                            }
                            var left = cache_font[tabindex - 1].rect;
                            cache_font[tabindex].rect = new Rectangle(left.Right, left.Y, 0, left.Height);

                            int user = 0;
                            for (int i = tabindex + 1; i < cache_font.Length; i++)
                            {
                                var it = cache_font[i];
                                it.show = true;
                                it.rect = new Rectangle(rect_d_r.X + user, rect_text.Y, it.width, CurrentCaret.Height);
                                user += it.width;
                                i_r.Add(i);
                            }
                        }
                        else
                        {
                            int user = 0;
                            for (int i = tabindex + 1; i < cache_font.Length; i++)
                            {
                                var it = cache_font[i];
                                it.show = true;
                                it.rect = new Rectangle(rect_d_r.X + user, rect_text.Y, it.width, CurrentCaret.Height);
                                user += it.width;
                                i_r.Add(i);
                            }
                        }
                        if (textalign == HorizontalAlignment.Right)
                        {
                            if (i_l.Count > 0)
                            {
                                int left = rect_d_l.Right - cache_font[i_l[i_l.Count - 1]].rect.Right;
                                foreach (var i in i_l)
                                {
                                    var it = cache_font[i];
                                    var rect_tmp = it.rect;
                                    rect_tmp.Offset(left, 0);
                                    it.rect = rect_tmp;
                                }
                            }
                            if (i_r.Count > 0)
                            {
                                int right = rect_d_r.Right - cache_font[i_r[i_r.Count - 1]].rect.Right;
                                foreach (var i in i_r)
                                {
                                    var it = cache_font[i];
                                    var rect_tmp = it.rect;
                                    rect_tmp.Offset(right, 0);
                                    it.rect = rect_tmp;
                                }
                            }
                        }
                        else if (textalign == HorizontalAlignment.Center)
                        {
                            if (i_l.Count > 0)
                            {
                                int left = (rect_d_l.Right - cache_font[i_l[i_l.Count - 1]].rect.Right) / 2;
                                foreach (var i in i_l)
                                {
                                    var it = cache_font[i];
                                    var rect_tmp = it.rect;
                                    rect_tmp.Offset(left, 0);
                                    it.rect = rect_tmp;
                                }
                            }
                            if (i_r.Count > 0)
                            {
                                int right = (rect_d_r.Right - cache_font[i_r[i_r.Count - 1]].rect.Right) / 2;
                                foreach (var i in i_r)
                                {
                                    var it = cache_font[i];
                                    var rect_tmp = it.rect;
                                    rect_tmp.Offset(right, 0);
                                    it.rect = rect_tmp;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in cache_font)
                        {
                            it.show = true;
                            it.rect = new Rectangle(rect_text.X + usex, rect_text.Y, it.width, CurrentCaret.Height);
                            usex += it.width;
                        }

                        if (textalign == HorizontalAlignment.Right)
                        {
                            int y = -1;
                            var list = new List<CacheFont>();
                            Action action = () =>
                            {
                                if (list.Count > 0)
                                {
                                    int w = rect_text.Right - list[list.Count - 1].rect.Right;
                                    foreach (var it in list)
                                    {
                                        var rect_tmp = it.rect;
                                        rect_tmp.Offset(w, 0);
                                        it.rect = rect_tmp;
                                    }
                                    list.Clear();
                                }
                            };
                            foreach (var it in cache_font)
                            {
                                if (it.rect.Y != y)
                                {
                                    y = it.rect.Y;
                                    action();
                                }
                                list.Add(it);
                            }
                            action();
                        }
                        else if (textalign == HorizontalAlignment.Center)
                        {
                            int y = -1;
                            var list = new List<CacheFont>();
                            Action action = () =>
                            {
                                if (list.Count > 0)
                                {
                                    int w = (rect_text.Right - list[list.Count - 1].rect.Right) / 2;
                                    foreach (var it in list)
                                    {
                                        var rect_tmp = it.rect;
                                        rect_tmp.Offset(w, 0);
                                        it.rect = rect_tmp;
                                    }
                                    list.Clear();
                                }
                            };
                            foreach (var it in cache_font)
                            {
                                if (it.rect.Y != y)
                                {
                                    y = it.rect.Y;
                                    action();
                                }
                                list.Add(it);
                            }
                            action();
                        }
                    }
                }

                var last = cache_font[cache_font.Length - 1];
                ScrollXMax = last.rect.Right - rect_text.Right;
                switch (textalign)
                {
                    case HorizontalAlignment.Center:
                        if (ScrollXMax > 0) ScrollXMin = -ScrollXMax;
                        else
                        {
                            ScrollXMin = ScrollXMax;
                            ScrollXMax = -ScrollXMax;
                        }
                        break;
                    case HorizontalAlignment.Right:
                        ScrollXMin = cache_font[0].rect.Right - rect.Right + sps;
                        ScrollXMax = 0;
                        break;
                    default:
                        ScrollXMin = 0;
                        break;
                }
                ScrollYMax = last.rect.Bottom - rect.Height + sps;
                if (multiline)
                {
                    ScrollX = 0;
                    ScrollXShow = false;
                    ScrollYShow = last.rect.Bottom > rect.Bottom;
                    if (ScrollYShow)
                    {
                        if (ScrollY > ScrollYMax) ScrollY = ScrollYMax;
                    }
                    else ScrollY = 0;
                }
                else
                {
                    ScrollYShow = false;
                    ScrollY = 0;
                    if (textalign == HorizontalAlignment.Right) ScrollXShow = last.rect.Right < rect.Right;
                    else ScrollXShow = last.rect.Right > rect_text.Right;
                    if (ScrollXShow)
                    {
                        if (ScrollX > ScrollXMax) ScrollX = ScrollXMax;
                    }
                    else ScrollX = 0;
                }
            }
            SetCaretPostion();
        }
        internal virtual bool HasLeft() => false;
        internal virtual int UseLeft(Rectangle rect, bool delgap) { return 0; }

        #region 最终区域计算

        void RectAuto(Rectangle rect, int sps, int sps2)
        {
            int read_height = CurrentCaret.Height;
            bool has_prefixText = prefixText != null, has_suffixText = suffixText != null, has_prefix = HasPrefix, has_suffix = HasSuffix;

            if (is_clear)
            {
                int icon_size = (int)(read_height * iconratio);
                if (has_prefixText)
                {
                    Helper.GDI(g =>
                    {
                        Size size_L = g.MeasureString(prefixText, Font).Size();
                        RectLR(rect, read_height, sps, sps2, size_L.Width, size_L.Height, icon_size, icon_size);
                    });
                }
                else if (has_prefix) RectLR(rect, read_height, sps, sps2, icon_size, icon_size, icon_size, icon_size);
                else RectR(rect, read_height, sps, sps2, icon_size, icon_size);
            }
            else
            {
                if (has_prefixText || has_suffixText || has_prefix || has_suffix)
                {
                    if (has_prefixText || has_suffixText)
                    {
                        Helper.GDI(g =>
                        {
                            if (has_prefixText && has_suffixText)
                            {
                                Size size_L = g.MeasureString(prefixText, Font).Size(), size_R = g.MeasureString(suffixText, Font).Size();
                                RectLR(rect, read_height, sps, sps2, size_L.Width, size_L.Height, size_R.Width, size_R.Height);
                            }
                            else
                            {
                                if (has_prefix || has_suffix)
                                {
                                    if (has_prefixText)
                                    {
                                        Size size_L = g.MeasureString(prefixText, Font).Size();
                                        if (has_suffix)
                                        {
                                            int icon_size = (int)(read_height * iconratio);
                                            RectLR(rect, read_height, sps, sps2, size_L.Width, size_L.Height, icon_size, icon_size);
                                        }
                                        else RectL(rect, read_height, sps, sps2, size_L.Width, size_L.Height);
                                    }
                                    else
                                    {
                                        Size size_R = g.MeasureString(suffixText, Font).Size();
                                        if (has_prefix)
                                        {
                                            int icon_size = (int)(read_height * iconratio);
                                            RectLR(rect, read_height, sps, sps2, icon_size, icon_size, size_R.Width, size_R.Height);
                                        }
                                        else RectR(rect, read_height, sps, sps2, size_R.Width, size_R.Height);
                                    }
                                }
                                else
                                {
                                    if (has_prefixText)
                                    {
                                        Size size_L = g.MeasureString(prefixText, Font).Size();
                                        RectL(rect, read_height, sps, sps2, size_L.Width, size_L.Height);
                                    }
                                    else
                                    {
                                        Size size_R = g.MeasureString(suffixText, Font).Size();
                                        RectR(rect, read_height, sps, sps2, size_R.Width, size_R.Height);
                                    }
                                }
                            }
                        });
                    }
                    else
                    {
                        if (has_prefix || has_suffix)
                        {
                            int icon_size = (int)(read_height * iconratio);
                            if (has_prefix && has_suffix) RectLR(rect, read_height, sps, sps2, icon_size, icon_size, icon_size, icon_size);
                            else if (has_prefix) RectL(rect, read_height, sps, sps2, icon_size, icon_size);
                            else RectR(rect, read_height, sps, sps2, icon_size, icon_size);
                        }
                    }
                }
                else
                {
                    if (HasLeft())
                    {
                        int useLeft = UseLeft(rect, false);
                        if (useLeft > 0)
                        {
                            rect.X += useLeft;
                            rect.Width -= useLeft;
                        }
                    }
                    rect_l.Width = rect_r.Width = 0;
                    if (multiline) rect_text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2, rect.Height - sps2);
                    else rect_text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - read_height) / 2, rect.Width - sps2, read_height);
                }
            }
        }
        void RectLR(Rectangle rect, int read_height, int sps, int sps2, int w_L, int h_L, int w_R, int h_R)
        {
            int sp = (int)(read_height * .25F), hasx = sps + w_L + sp, hasr = w_L + w_R + ((sps + sp) * 2),
                useLeft = HasLeft() ? UseLeft(new Rectangle(rect.X + hasx, rect.Y, rect.Width - hasr, rect.Height), true) : 0;
            if (multiline)
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + sps + (read_height - h_L) / 2, w_L, h_L);
                if (useLeft > 0) rect_text = new Rectangle(rect.X + hasx + useLeft, rect.Y + sps, rect.Width - hasr - useLeft, rect.Height - sps2);
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + sps, rect.Width - w_L - w_R - ((sps + sp) * 2), rect.Height - sps2);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + sps + (read_height - h_R) / 2, w_R, h_R);
            }
            else
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h_L) / 2, w_L, h_L);
                if (useLeft > 0) rect_text = new Rectangle(rect.X + hasx + useLeft, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasr - useLeft, read_height);
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasr, read_height);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + (rect.Height - h_R) / 2, w_R, h_R);
            }
        }
        void RectL(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int sp = (int)(read_height * .25F), hasx = sps + w + sp,
                useLeft = HasLeft() ? UseLeft(new Rectangle(rect.X + hasx, rect.Y, rect.Width - hasx, rect.Height), true) : 0;
            if (multiline)
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + sps + (read_height - h) / 2, w, h);
                if (useLeft > 0) rect_text = new Rectangle(rect.X + hasx + useLeft, rect.Y + sps, rect.Width - hasx - useLeft, rect.Height - sps2);
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + sps, rect.Width - sps, rect.Height - sps2);
            }
            else
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h) / 2, w, h);
                if (useLeft > 0) rect_text = new Rectangle(rect.X + hasx + useLeft, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasx - useLeft, read_height);
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasx, read_height);
            }
            rect_r.Width = 0;
        }
        void RectR(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int sp = (int)(read_height * .25F);
            if (HasLeft())
            {
                int useLeft = UseLeft(new Rectangle(rect.X, rect.Y, rect.Width - sp, rect.Height), false);
                if (useLeft > 0)
                {
                    rect.X += useLeft;
                    rect.Width -= useLeft;
                }
            }
            if (multiline)
            {
                rect_text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2 - w - sp, rect.Height - sps2);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + sps + (read_height - h) / 2, w, h);
            }
            else
            {
                rect_text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - read_height) / 2, rect.Width - sps2 - w - sp, read_height);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + (rect.Height - h) / 2, w, h);
            }
            rect_l.Width = 0;
        }

        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            CalculateRect();
            base.OnSizeChanged(e);
        }
    }
}