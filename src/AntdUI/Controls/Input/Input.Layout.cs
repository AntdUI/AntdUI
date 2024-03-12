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
                        font_height = (int)Math.Ceiling(g.MeasureString("Qq", Font, 10000, sf_font).Height);
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
                            foreach (char it in _text) font_widths.Add(new CacheFont(PassWordChar, false, w));
                        }
                        else
                        {
                            bool iseone = false;
                            foreach (char it in _text)
                            {
                                string txt = it.ToString();
                                var unicodeInfo = CharUnicodeInfo.GetUnicodeCategory(it);
                                if (IsEmoji(unicodeInfo))
                                {
                                    HasEmoji = true;
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
                                    if (it == '\t')
                                    {
                                        var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                                    }
                                    else if (it == '\n') font_widths.Add(new CacheFont(txt, false, 0));
                                    else
                                    {
                                        var sizefont = g.MeasureString(txt, Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width)));
                                    }
                                }
                            }

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
                            foreach (char it in _text) font_widths.Add(new CacheFont(PassWordChar, false, w));
                        }
                        else
                        {
                            var font_dir = new Dictionary<string, CacheFont>();
                            if (cache_font != null)
                            {
                                foreach (var it in cache_font) if (!it.emoji && !font_dir.ContainsKey(it.text)) font_dir.Add(it.text, it);
                            }
                            bool iseone = false;
                            foreach (char it in _text)
                            {
                                string txt = it.ToString();

                                var unicodeInfo = CharUnicodeInfo.GetUnicodeCategory(it);
                                if (IsEmoji(unicodeInfo))
                                {
                                    HasEmoji = true;
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
                                    if (font_dir.TryGetValue(txt, out var find))
                                    {
                                        if (font_height < find.rect.Height) font_height = find.rect.Height;
                                        font_widths.Add(new CacheFont(txt, false, find.width));
                                    }
                                    else
                                    {
                                        if (it == '\t')
                                        {
                                            var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                            font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                                        }
                                        else if (it == '\n') font_widths.Add(new CacheFont(txt, false, 0));
                                        else
                                        {
                                            var sizefont = g.MeasureString(txt, Font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = (int)Math.Ceiling(sizefont.Height);
                                            font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width)));
                                        }
                                    }
                                }
                            }

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
            //return unicodeInfo == UnicodeCategory.Surrogate;
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
            public Rectangle rect { get; set; }
            public bool emoji { get; set; }
            public bool retun { get; set; }
            public int width { get; set; }
            internal bool show { get; set; }
        }

        #endregion

        internal Rectangle rect_text, rect_l, rect_r;
        internal void CalculateRect()
        {
            var rect = ReadRectangle;
            int sps = (int)(CurrentCaret.Height * .4F), sps2 = sps * 2;
            RectAuto(rect, sps, sps2);
            if (cache_font == null) CurrentCaret.Location = rect_text.Location;
            else
            {
                if (multiline)
                {
                    int usex = 0, usey = 0;
                    foreach (var it in cache_font)
                    {
                        it.show = false;
                        if (it.text == "\r")
                        {
                            it.retun = true;
                            it.rect = new Rectangle(rect_text.X + usex, rect_text.Y + usey, it.width, CurrentCaret.Height);
                            continue;
                        }
                        else if (it.text == "\n")
                        {
                            it.retun = true;
                            usey += CurrentCaret.Height;
                            usex = 0;
                            it.rect = new Rectangle(rect_text.X + usex, rect_text.Y + usey, it.width, CurrentCaret.Height);
                            continue;
                        }
                        else if (usex + it.width > rect_text.Width)
                        {
                            usey += CurrentCaret.Height;
                            usex = 0;
                        }
                        it.rect = new Rectangle(rect_text.X + usex, rect_text.Y + usey, it.width, CurrentCaret.Height);
                        usex += it.width;
                    }
                }
                else
                {
                    int usex = 0;
                    foreach (var it in cache_font)
                    {
                        it.show = true;
                        it.rect = new Rectangle(rect_text.X + usex, rect_text.Y, it.width, CurrentCaret.Height);
                        usex += it.width;
                    }
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

        #region 最终区域计算

        void RectAuto(Rectangle rect, int sps, int sps2)
        {
            int read_height = CurrentCaret.Height;
            bool has_prefixText = prefixText != null, has_suffixText = suffixText != null, has_image = HasImage, has_suffix = HasSuffix;

            if (is_clear)
            {
                int icon_size = (int)(read_height * .7F);
                if (has_prefixText)
                {
                    Helper.GDI(g =>
                    {
                        Size size_L = g.MeasureString(prefixText, Font).Size();
                        RectLR(rect, read_height, sps, sps2, size_L.Width, size_L.Height, icon_size, icon_size);
                    });
                }
                else if (has_image) RectLR(rect, read_height, sps, sps2, icon_size, icon_size, icon_size, icon_size);
                else RectR(rect, read_height, sps, sps2, icon_size, icon_size);
            }
            else
            {
                if (has_prefixText || has_suffixText || has_image || has_suffix)
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
                                if (has_image || has_suffix)
                                {
                                    if (has_prefixText)
                                    {
                                        Size size_L = g.MeasureString(prefixText, Font).Size();
                                        if (has_suffix)
                                        {
                                            int icon_size = (int)(read_height * .7F);
                                            RectLR(rect, read_height, sps, sps2, size_L.Width, size_L.Height, icon_size, icon_size);
                                        }
                                        else RectL(rect, read_height, sps, sps2, size_L.Width, size_L.Height);
                                    }
                                    else
                                    {
                                        Size size_R = g.MeasureString(suffixText, Font).Size();
                                        if (has_image)
                                        {
                                            int icon_size = (int)(read_height * .7F);
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
                        if (has_image || has_suffix)
                        {
                            int icon_size = (int)(read_height * .7F);
                            if (has_image && has_suffix) RectLR(rect, read_height, sps, sps2, icon_size, icon_size, icon_size, icon_size);
                            else if (has_image) RectL(rect, read_height, sps, sps2, icon_size, icon_size);
                            else RectR(rect, read_height, sps, sps2, icon_size, icon_size);
                        }
                    }
                }
                else
                {
                    rect_l.Width = rect_r.Width = 0;
                    if (multiline) rect_text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2, rect.Height - sps2);
                    else rect_text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - read_height) / 2, rect.Width - sps2, read_height);
                }
            }
        }
        void RectLR(Rectangle rect, int read_height, int sps, int sps2, int w_L, int h_L, int w_R, int h_R)
        {
            int sp = (int)(read_height * .25F);
            if (multiline)
            {
                rect_text = new Rectangle(rect.X + sps + w_L + sp, rect.Y + sps, rect.Width - w_L - w_R - ((sps + sp) * 2), rect.Height - sps2);
                rect_l = new Rectangle(rect.X + sps, rect.Y + sps + (read_height - h_L) / 2, w_L, h_L);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + sps + (read_height - h_R) / 2, w_R, h_R);
            }
            else
            {
                rect_text = new Rectangle(rect.X + sps + w_L + sp, rect.Y + (rect.Height - read_height) / 2, rect.Width - w_L - w_R - ((sps + sp) * 2), read_height);
                rect_l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h_L) / 2, w_L, h_L);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + (rect.Height - h_R) / 2, w_R, h_R);
            }
        }
        void RectL(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int sp = (int)(read_height * .25F);
            if (multiline)
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + sps + (read_height - h) / 2, w, h);
                rect_text = new Rectangle(rect.X + sps + w + sp, rect.Y + sps, rect.Width - sps2 - w - sp, rect.Height - sps2);
            }
            else
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h) / 2, w, h);
                rect_text = new Rectangle(rect.X + sps + w + sp, rect.Y + (rect.Height - read_height) / 2, rect.Width - sps2 - w - sp, read_height);
            }

            rect_r.Width = 0;
        }
        void RectR(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int sp = (int)(read_height * .25F);
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