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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            FixFontWidth(true);
            base.OnHandleCreated(e);
        }

        #region 确定字体宽度

        CacheFont[]? cache_font = null;
        bool HasEmoji = false;
        void FixFontWidth(bool force = false)
        {
            HasEmoji = false;
            var text = Text;
            if (force)
            {
                Helper.GDI(g =>
                {
                    float dpi = Config.Dpi;
                    int font_height = g.MeasureString(Config.NullText, Font, 10000, sf_font).Height;
                    if (isempty)
                    {
                        ScrollX = ScrollY = 0;
                        cache_font = null;
                    }
                    else
                    {
                        var font_widths = new List<CacheFont>(text.Length);
                        if (IsPassWord)
                        {
                            var sizefont = g.MeasureString(PassWordChar, Font, 10000, sf_font);
                            int w = sizefont.Width;
                            if (font_height < sizefont.Height) font_height = sizefont.Height;
                            foreach (char it in text) font_widths.Add(new CacheFont(it.ToString(), false, w));
                        }
                        else
                        {
                            GraphemeSplitter.Each(text, 0, (str, nStart, nLen, nType) =>
                            {
                                string txt = str.Substring(nStart, nLen);
                                if (nType == 18)
                                {
                                    HasEmoji = true;
                                    font_widths.Add(new CacheFont(txt, true, 0));
                                }
                                else
                                {
                                    if (txt == "\t")
                                    {
                                        var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = sizefont.Height;
                                        font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                                    }
                                    else if (txt == "\n" || txt == "\r\n")
                                    {
                                        var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = sizefont.Height;
                                        font_widths.Add(new CacheFont(txt, false, sizefont.Width));
                                    }
                                    else
                                    {
                                        var sizefont = g.MeasureString(txt, Font, 10000, sf_font);
                                        if (font_height < sizefont.Height) font_height = sizefont.Height;
                                        font_widths.Add(new CacheFont(txt, false, sizefont.Width));
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
                                            if (font_height < sizefont.Height) font_height = sizefont.Height;
                                            it.width = sizefont.Width;
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < font_widths.Count; i++) { font_widths[i].i = i; }
                        cache_font = font_widths.ToArray();
                        SetStyle();
                    }
                    CaretInfo.Height = font_height;
                    CalculateRect();
                    GC.Collect();
                });
            }
            else
            {
                if (isempty)
                {
                    ScrollX = ScrollY = 0;
                    cache_font = null;
                    CalculateRect();
                    GC.Collect();
                }
                else
                {
                    Helper.GDI(g =>
                    {
                        int font_height = g.MeasureString(Config.NullText, Font, 10000, sf_font).Height;
                        if (text == null)
                        {
                            CaretInfo.Height = font_height;
                            return;
                        }
                        var font_widths = new List<CacheFont>(text.Length);
                        if (IsPassWord)
                        {
                            var sizefont = g.MeasureString(PassWordChar, Font, 10000, sf_font);
                            int w = sizefont.Width;
                            if (font_height < sizefont.Height) font_height = sizefont.Height;
                            foreach (char it in text) font_widths.Add(new CacheFont(it.ToString(), false, w));
                        }
                        else
                        {
                            var font_dir = new Dictionary<string, CacheFont>(font_widths.Count);
                            if (cache_font != null)
                            {
                                foreach (var it in cache_font) if (!it.emoji && !font_dir.ContainsKey(it.text)) font_dir.Add(it.text, it);
                            }
                            GraphemeSplitter.Each(text, 0, (str, nStart, nLen, nType) =>
                            {
                                string txt = str.Substring(nStart, nLen);
                                if (nType == 18)
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
                                            if (font_height < sizefont.Height) font_height = sizefont.Height;
                                            font_widths.Add(new CacheFont(txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                                        }
                                        else if (txt == "\n" || txt == "\r\n")
                                        {
                                            var sizefont = g.MeasureString(" ", Font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = sizefont.Height;
                                            font_widths.Add(new CacheFont(txt, false, sizefont.Width));
                                        }
                                        else
                                        {
                                            var sizefont = g.MeasureString(txt, Font, 10000, sf_font);
                                            if (font_height < sizefont.Height) font_height = sizefont.Height;
                                            font_widths.Add(new CacheFont(txt, false, sizefont.Width));
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
                                            if (font_height < sizefont.Height) font_height = sizefont.Height;
                                            it.width = sizefont.Width;
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < font_widths.Count; i++) { font_widths[i].i = i; }
                        cache_font = font_widths.ToArray();
                        SetStyle();
                        CaretInfo.Height = font_height;
                        CalculateRect();
                        GC.Collect();
                    });
                }
            }
        }


        internal class CacheFont
        {
            public CacheFont(string _text, bool _emoji, int _width)
            {
                text = _text;
                emoji = _emoji;
                width = _width;
            }
            public int i { get; set; }
            public int line { get; set; }
            public string text { get; set; }
            public Rectangle rect { get; set; }
            public Rectangle? rect2 { get; set; }
            public bool ret { get; set; }
            public bool emoji { get; set; }
            public int width { get; set; }
            internal bool show { get; set; }

            #region 样式

            /// <summary>
            /// 字体
            /// </summary>
            public Font? font { get; set; }

            /// <summary>
            /// 文本颜色
            /// </summary>
            public Color? fore { get; set; }

            /// <summary>
            /// 背景色
            /// </summary>
            public Color? back { get; set; }

            #endregion

            public override string ToString() => text;
        }

        #endregion

        internal Rectangle rect_text, rect_l, rect_r;
        internal Rectangle rect_d_ico, rect_d_l, rect_d_r;

        internal Rectangle? RECTDIV = null;
        List<int> LineBreakNumber = new List<int>(0);
        internal void CalculateRect()
        {
            var rect = RECTDIV.HasValue ? RECTDIV.Value.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, JoinLeft, JoinRight) : ReadRectangle;
            int sps = (int)(CaretInfo.Height * .4F), sps2 = sps * 2;
            RectAuto(rect, sps, sps2);
            if (cache_font == null)
            {
                ScrollXShow = ScrollYShow = false;
                scrollx = scrolly = ScrollXMin = ScrollXMax = ScrollYMax = 0;
                if (LineBreakNumber.Count > 0) LineBreakNumber.Clear();
                if (ModeRange)
                {
                    int center = rect_text.Width / 2;
                    int h2 = CaretInfo.Height / 2;
                    rect_d_ico = new Rectangle(rect_text.X + center - h2, rect_text.Y + ((rect_text.Height - CaretInfo.Height) / 2), CaretInfo.Height, CaretInfo.Height);
                    rect_d_l = new Rectangle(rect_text.X, rect_text.Y, center - h2, rect_text.Height);
                    rect_d_r = new Rectangle(rect_d_l.Right + CaretInfo.Height, rect_text.Y, rect_d_l.Width, rect_text.Height);
                }
                CaretInfo.Place = false;
                CaretInfo.SetXY(rect_text.X, rect_text.Y);
            }
            else
            {
                if (multiline)
                {
                    var _retnot = new List<int>();
                    var rectText = rect_text;
                    if (ScrollYShow) rectText.Width -= 16;
                    int lineHeight = CaretInfo.Height + (lineheight > 0 ? (int)(lineheight * Config.Dpi) : 0);
                    int usex = 0, usey = 0, line = 0, retindex = -1;
                    foreach (var it in cache_font)
                    {
                        it.show = false;
                        if (it.text == "\n" || it.text == "\r\n")
                        {
                            if (usex == 0) retindex++;
                            else retindex = -1;
                            it.ret = true;
                            it.line = line;
                            line++;
                            it.rect2 = new Rectangle(rectText.X + usex, rectText.Y + usey, 0, CaretInfo.Height);
                            if (retindex > 0) _retnot.Add(rectText.Y + usey - lineHeight);
                            usey += lineHeight;
                            usex = 0;
                            it.rect = new Rectangle(rectText.X + usex, rectText.Y + usey, 0, CaretInfo.Height);
                            continue;
                        }
                        else
                        {
                            retindex = -1;
                            if (it.text == "\r")
                            {
                                it.rect = new Rectangle(rectText.X + usex, rectText.Y + usey, it.width, CaretInfo.Height);
                                continue;
                            }
                            else if (usex + it.width > rectText.Width)
                            {
                                line++;
                                usey += lineHeight;
                                usex = 0;
                            }
                        }
                        it.line = line;
                        it.rect = new Rectangle(rectText.X + usex, rectText.Y + usey, it.width, CaretInfo.Height);
                        usex += it.width;
                    }
                    HandTextAlignCore(cache_font);
                    LineBreakNumber = _retnot;
                }
                else
                {
                    if (LineBreakNumber.Count > 0) LineBreakNumber.Clear();
                    HandTextAlign(cache_font);
                }
                if (cache_font.Length == 0)
                {
                    isempty = true;
                    _text = "";
                    cache_font = null;
                    GC.Collect();
                    return;
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

        #region 处理文本方向

        void HandTextAlign(CacheFont[] cache_font)
        {
            int usex = 0;
            if (ModeRange)
            {
                int center = rect_text.Width / 2;
                int h2 = CaretInfo.Height / 2;
                rect_d_ico = new Rectangle(rect_text.X + center - h2, rect_text.Y + ((rect_text.Height - CaretInfo.Height) / 2), CaretInfo.Height, CaretInfo.Height);
                rect_d_l = new Rectangle(rect_text.X, rect_text.Y, center - h2, rect_text.Height);
                rect_d_r = new Rectangle(rect_d_l.Right + CaretInfo.Height, rect_text.Y, rect_d_l.Width, rect_text.Height);
                int GetTabIndex()
                {
                    foreach (var it in cache_font)
                    {
                        if (it.text == "\t") return it.i;
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
                        it.rect = new Rectangle(rect_d_l.X + usex, rect_text.Y, it.width, CaretInfo.Height);
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
                        it.rect = new Rectangle(rect_d_l.X + usex, rect_text.Y, it.width, CaretInfo.Height);
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
                        it.rect = new Rectangle(rect_d_r.X + user, rect_text.Y, it.width, CaretInfo.Height);
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
                        it.rect = new Rectangle(rect_d_r.X + user, rect_text.Y, it.width, CaretInfo.Height);
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
                    it.rect = new Rectangle(rect_text.X + usex, rect_text.Y, it.width, CaretInfo.Height);
                    usex += it.width;
                }
                HandTextAlignCore(cache_font);
            }
        }
        void HandTextAlignCore(CacheFont[] cache_font)
        {
            if (textalign == HorizontalAlignment.Right)
            {
                int y = -1;
                var list = new List<CacheFont>(cache_font.Length);
                foreach (var it in cache_font)
                {
                    if (it.rect.Y != y)
                    {
                        y = it.rect.Y;
                        HandTextAlignRight(ref list);
                    }
                    list.Add(it);
                }
                HandTextAlignRight(ref list);
            }
            else if (textalign == HorizontalAlignment.Center)
            {
                int y = -1;
                var list = new List<CacheFont>(cache_font.Length);
                foreach (var it in cache_font)
                {
                    if (it.rect.Y != y)
                    {
                        y = it.rect.Y;
                        HandTextAlignCenter(ref list);
                    }
                    list.Add(it);
                }
                HandTextAlignCenter(ref list);
            }
        }
        void HandTextAlignRight(ref List<CacheFont> list)
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
        }
        void HandTextAlignCenter(ref List<CacheFont> list)
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
        }

        #endregion

        #region 最终区域计算

        void RectAuto(Rectangle rect, int sps, int sps2)
        {
            int read_height = CaretInfo.Height;
            string? prefixText = PrefixText, suffixText = SuffixText;
            bool has_prefixText = prefixText != null, has_suffixText = suffixText != null, has_prefix = HasPrefix, has_suffix = HasSuffix;

            if (is_clear)
            {
                int icon_size = (int)(read_height * iconratio);
                if (has_prefixText)
                {
                    Helper.GDI(g =>
                    {
                        RectLR(rect, read_height, sps, sps2, g.MeasureString(prefixText, Font).Width, read_height, icon_size, icon_size);
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
                            if (has_prefixText && has_suffixText) RectLR(rect, read_height, sps, sps2, g.MeasureString(prefixText, Font).Width, read_height, g.MeasureString(suffixText, Font).Width, read_height);
                            else
                            {
                                if (has_prefix || has_suffix)
                                {
                                    if (has_prefixText)
                                    {
                                        if (has_suffix)
                                        {
                                            int icon_size = (int)(read_height * iconratio);
                                            RectLR(rect, read_height, sps, sps2, g.MeasureString(prefixText, Font).Width, read_height, icon_size, icon_size);
                                        }
                                        else RectL(rect, read_height, sps, sps2, g.MeasureString(prefixText, Font).Width);
                                    }
                                    else
                                    {
                                        if (has_prefix)
                                        {
                                            int icon_size = (int)(read_height * iconratio);
                                            RectLR(rect, read_height, sps, sps2, icon_size, icon_size, g.MeasureString(suffixText, Font).Width, read_height);
                                        }
                                        else RectR(rect, read_height, sps, sps2, g.MeasureString(suffixText, Font).Width);
                                    }
                                }
                                else
                                {
                                    if (has_prefixText) RectL(rect, read_height, sps, sps2, g.MeasureString(prefixText, Font).Width);
                                    else RectR(rect, read_height, sps, sps2, g.MeasureString(suffixText, Font).Width);
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
                    int ul = -1;
                    if (HasLeft())
                    {
                        var useLeft = UseLeft(rect, CaretInfo.Height, false);
                        if (useLeft[0] > 0 || useLeft[1] > 0)
                        {
                            int ux = useLeft[0], uy = useLeft[1];
                            rect.X += ux;
                            rect.Width -= ux;

                            rect.Y += uy;
                            rect.Height -= uy;
                            ul = uy;
                        }
                    }
                    rect_l.Width = rect_r.Width = 0;
                    if (multiline) rect_text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2, rect.Height - sps2);
                    else rect_text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - read_height) / 2, rect.Width - sps2, read_height);
                    if (ul > -1) UseLeftAutoHeight(read_height, ul);
                }
            }
        }
        void RectLR(Rectangle rect, int read_height, int sps, int sps2, int w_L, int h_L, int w_R, int h_R)
        {
            int ul = -1, sp = (int)(read_height * icongap), hasx = sps + w_L + sp, hasr = w_L + w_R + ((sps + sp) * 2);
            var useLeft = HasLeft() ? UseLeft(new Rectangle(rect.X + hasx, rect.Y, rect.Width - hasr, rect.Height), CaretInfo.Height, true) : new int[] { 0, 0 };
            if (multiline)
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + sps + (read_height - h_L) / 2, w_L, h_L);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, rect.Y + sps + uy, rect.Width - hasr - ux, rect.Height - sps2 - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + sps, rect.Width - w_L - w_R - ((sps + sp) * 2), rect.Height - sps2);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + sps + (read_height - h_R) / 2, w_R, h_R);
            }
            else
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h_L) / 2, w_L, h_L);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, (rect.Y + (rect.Height - read_height) / 2) + uy, rect.Width - hasr - ux, read_height - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasr, read_height);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + (rect.Height - h_R) / 2, w_R, h_R);
            }
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }

        #region 前缀布局

        void RectL(Rectangle rect, int read_height, int sps, int sps2, int w)
        {
            int ul = -1, sp = (int)(read_height * icongap), hasx = sps + w + sp, hasx2 = sps2 + w + sp;
            var useLeft = HasLeft() ? UseLeft(new Rectangle(rect.X + hasx, rect.Y, rect.Width - hasx, rect.Height), CaretInfo.Height, true) : new int[] { 0, 0 };
            if (multiline)
            {
                int y = rect.Y + sps, h = rect.Height - sps2;
                rect_l = new Rectangle(rect.X + sps, y, w, read_height);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, y + uy, rect.Width - hasx2 - ux, h - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, y, rect.Width - hasx2, h);
            }
            else
            {
                int y = rect.Y + (rect.Height - read_height) / 2;
                rect_l = new Rectangle(rect.X + sps, y, w, read_height);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, y + uy, rect.Width - hasx2 - ux, read_height - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, y, rect.Width - hasx2, read_height);
            }
            rect_r.Width = 0;
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }
        void RectL(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int ul = -1, sp = (int)(read_height * icongap), hasx = sps + w + sp, hasx2 = sps2 + w + sp;
            var useLeft = HasLeft() ? UseLeft(new Rectangle(rect.X + hasx, rect.Y, rect.Width - hasx, rect.Height), CaretInfo.Height, true) : new int[] { 0, 0 };
            if (multiline)
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + sps + (read_height - h) / 2, w, h);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, rect.Y + sps + uy, rect.Width - hasx2 - ux, rect.Height - sps2 - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + sps, rect.Width - hasx2, rect.Height - sps2);
            }
            else
            {
                rect_l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h) / 2, w, h);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, (rect.Y + (rect.Height - read_height) / 2) + uy, rect.Width - hasx2 - ux, read_height - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasx2, read_height);
            }
            rect_r.Width = 0;
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }

        #endregion

        #region 后缀布局

        void RectR(Rectangle rect, int read_height, int sps, int sps2, int w)
        {
            int ul = -1, sp = (int)(read_height * icongap);
            if (HasLeft())
            {
                var useLeft = UseLeft(new Rectangle(rect.X, rect.Y, rect.Width - sp, rect.Height), CaretInfo.Height, false);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect.X += ux;
                    rect.Width -= ux;
                    rect.Y += uy;
                    rect.Height -= uy;
                    ul = uy;
                }
            }
            if (multiline)
            {
                rect_text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2 - w - sp, rect.Height - sps2);
                rect_r = new Rectangle(rect_text.Right + sp, rect_text.Y, w, read_height);
            }
            else
            {
                rect_text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - read_height) / 2, rect.Width - sps2 - w - sp, read_height);
                rect_r = new Rectangle(rect_text.Right + sp, rect_text.Y, w, rect_text.Height);
            }
            rect_l.Width = 0;
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }
        void RectR(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int ul = -1, sp = (int)(read_height * icongap);
            if (HasLeft())
            {
                var useLeft = UseLeft(new Rectangle(rect.X, rect.Y, rect.Width - sp, rect.Height), CaretInfo.Height, false);
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect.X += ux;
                    rect.Width -= ux;
                    rect.Y += uy;
                    rect.Height -= uy;
                    ul = uy;
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
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }

        #endregion

        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            CalculateRect();
            base.OnSizeChanged(e);
        }
    }
}