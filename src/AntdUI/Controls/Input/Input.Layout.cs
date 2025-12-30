// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
            base.OnHandleCreated(e);
            FixFontWidth(true);
        }

        #region 确定字体宽度

        CacheFont[]? cache_font;
        CacheCaret[]? cache_caret;
        bool HasEmoji = false;
        bool FixFontWidth(bool force = false)
        {
            HasEmoji = false;
            var text = Text;
            if (force)
            {
                return Helper.GDI(g =>
                {
                    float dpi = Dpi;
                    int font_height = g.MeasureString(Config.NullText, Font).Height, rdcount = 0;
                    if (isempty)
                    {
                        TextTotalLine = 0;
                        if (ScrollX.SetValue(0)) rdcount++;
                        if (ScrollY.SetValue(0)) rdcount++;
                        cache_font = null;
                        cache_caret = null;
                    }
                    else FixFontWidth(g, text, force, ref font_height);
                    CaretInfo.Height = font_height;
                    return CalculateRect() || rdcount > 0;
                });
            }
            else
            {
                if (isempty)
                {
                    TextTotalLine = 0;
                    bool set_x = ScrollX.SetValue(0), set_y = ScrollY.SetValue(0);
                    cache_font = null;
                    cache_caret = null;
                    return CalculateRect() || set_x || set_y;
                }
                else
                {
                    return Helper.GDI(g =>
                    {
                        int font_height = g.MeasureString(Config.NullText, Font).Height;
                        if (text == null)
                        {
                            CaretInfo.Height = font_height;
                            return false;
                        }
                        FixFontWidth(g, text, force, ref font_height);
                        CaretInfo.Height = font_height;
                        return CalculateRect();
                    });
                }
            }
        }

        void FixFontWidth(Canvas g, string text, bool force, ref int fontHeight)
        {
            var font_widths = new List<CacheFont>(text.Length);
            int index = 0;
            if (IsPassWord)
            {
                var sizefont = g.MeasureString(PassWordChar, Font);
                int w = sizefont.Width;
                if (fontHeight < sizefont.Height) fontHeight = sizefont.Height;
                foreach (char it in text)
                {
                    font_widths.Add(new CacheFont(index, it.ToString(), false, w));
                    index++;
                }
            }
            else
            {
                Dictionary<string, CacheFont> font_dir;
                if (force || cache_font == null) font_dir = new Dictionary<string, CacheFont>(0);
                else
                {
                    font_dir = new Dictionary<string, CacheFont>(font_widths.Count);
                    foreach (var it in cache_font)
                    {
                        if (it.emoji || font_dir.ContainsKey(it.text)) continue;
                        font_dir.Add(it.text, it);
                    }
                }

                int font_height = fontHeight;
                GraphemeSplitter.Each(text, 0, (str, nStart, nLen, nType) =>
                {
                    string txt = str.Substring(nStart, nLen);
                    if (nType == 18 || nType == 4)
                    {
                        HasEmoji = true;
                        font_widths.Add(new CacheFont(index, txt, true, 0));
                    }
                    else
                    {
                        if (font_dir.TryGetValue(txt, out var find))
                        {
                            if (font_height < find.rect.Height) font_height = find.rect.Height;
                            font_widths.Add(new CacheFont(index, txt, false, find.width));
                        }
                        else
                        {
                            if (txt == "\t")
                            {
                                var sizefont = g.MeasureString(" ", Font);
                                if (font_height < sizefont.Height) font_height = sizefont.Height;
                                font_widths.Add(new CacheFont(index, txt, false, (int)Math.Ceiling(sizefont.Width * 8F)));
                            }
                            else if (txt == "\n" || txt == "\r\n")
                            {
                                var sizefont = g.MeasureString(" ", Font);
                                if (font_height < sizefont.Height) font_height = sizefont.Height;
                                font_widths.Add(new CacheFont(index, txt, false, sizefont.Width));
                            }
                            else
                            {
                                var sizefont = g.MeasureString(txt, Font);
                                if (font_height < sizefont.Height) font_height = sizefont.Height;
                                font_widths.Add(new CacheFont(index, txt, false, sizefont.Width));
                            }
                        }
                    }
                    index++;
                    return true;
                });
                fontHeight = font_height;
                if (HasEmoji)
                {
                    foreach (var it in font_widths)
                    {
                        if (it.emoji) it.width = font_height;
                    }
                }
            }
            for (int i = 0; i < font_widths.Count; i++) font_widths[i].i = i;
            cache_font = font_widths.ToArray();
            SetStyle();
        }

        internal class CacheFont
        {
            public CacheFont(int index, string _text, bool _emoji, int _width)
            {
                i = index;
                text = _text;
                emoji = _emoji;
                width = _width;
            }
            public int i { get; set; }
            public int line { get; set; }
            public string text { get; set; }
            public Rectangle rect { get; set; }
            public bool ret { get; set; }
            public bool hide { get; set; }
            public bool emoji { get; set; }
            public int width { get; set; }

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

        internal class CacheCaret
        {
            public int x { get; set; }
            public int y { get; set; }

            /// <summary>
            /// Caret 内序号
            /// </summary>
            public int index { get; set; }

            /// <summary>
            /// 真实的文本序号
            /// </summary>
            public int i { get; set; }
        }

        #endregion

        internal Rectangle rect_text, rect_l, rect_r;
        internal Rectangle rect_d_ico, rect_d_l, rect_d_r;

        internal Rectangle? RECTDIV;
        internal int UR = 0;
        internal bool CalculateRect()
        {
            var rect = RECTDIV.HasValue ? RECTDIV.Value.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Dpi, joinMode, JoinLeft, JoinRight) : ReadRectangle;
            int sps = (int)(CaretInfo.Height * .4F), sps2 = sps * 2;
            rect.Width -= UR;
            RectAuto(rect, sps, sps2);
            int rdcount = 0;
            if (cache_font == null)
            {
                TextTotalLine = 0;
                oldtmpY = oldtmpX = null;
                ScrollX.Clear();
                ScrollY.Clear();
                if (ModeRange)
                {
                    int center = rect_text.Width / 2;
                    int h2 = CaretInfo.Height / 2;
                    rect_d_ico = new Rectangle(rect_text.X + center - h2, rect_text.Y + ((rect_text.Height - CaretInfo.Height) / 2), CaretInfo.Height, CaretInfo.Height);
                    rect_d_l = new Rectangle(rect_text.X, rect_text.Y, center - h2, rect_text.Height);
                    rect_d_r = new Rectangle(rect_d_l.Right + CaretInfo.Height, rect_text.Y, rect_d_l.Width, rect_text.Height);
                }
                if (CaretInfo.SetXY(rect_text.X, rect_text.Y)) rdcount++;
            }
            else
            {
                int maxx = 0;
                if (multiline)
                {
                    var rectText = rect_text;
                    if (ScrollY.Show) rectText.Width -= (int)(16 * Dpi);
                    int lineHeight = CaretInfo.Height + (lineheight > 0 ? (int)(lineheight * Dpi) : 0);
                    int usex = 0, usey = 0, line = 0;
                    foreach (var it in cache_font)
                    {
                        if (it.text == "\n" || it.text == "\r\n")
                        {
                            it.hide = it.ret = true;
                            it.line = line;
                            line++;
                            if (usex > maxx) maxx = usex;
                            usey += lineHeight;
                            usex = 0;
                            it.rect = new Rectangle(rectText.X + usex, rectText.Y + usey, 0, CaretInfo.Height);
                            continue;
                        }
                        else if (it.text == " " || it.text == "\t") it.hide = true;
                        else
                        {
                            if (it.text == "\r")
                            {
                                it.hide = true;
                                it.rect = new Rectangle(rectText.X + usex, rectText.Y + usey, it.width, CaretInfo.Height);
                                continue;
                            }
                            else if (wordwrap && usex + it.width > rectText.Width)
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
                    TextTotalLine = line;
                    HandTextAlignCore(cache_font);
                }
                else
                {
                    TextTotalLine = 1;
                    HandTextAlign(cache_font);
                }

                if (cache_font.Length == 0)
                {
                    TextTotalLine = 0;
                    isempty = true;
                    _text = "";
                    cache_font = null;
                    cache_caret = null;
                    return true;
                }

                var last = cache_font[cache_font.Length - 1];

                var carets = new List<CacheCaret>(cache_font.Length + 2)
                {
                    new CacheCaret { x = cache_font[0].rect.X, y = rect_text.Y, i = 0,index=0 }
                };
                int tmp = 1;
                for (int i = 0; i < cache_font.Length; i++)
                {
                    var it = cache_font[i];
                    if (it.ret)
                    {
                        if (i > 0)
                        {
                            var it_up = cache_font[i - 1];
                            carets.Add(new CacheCaret
                            {
                                index = tmp,
                                i = i,
                                x = it_up.rect.Right,
                                y = it_up.rect.Y
                            });
                        }
                        else
                        {
                            carets.Add(new CacheCaret
                            {
                                index = tmp,
                                i = i + 1,
                                x = it.rect.X,
                                y = it.rect.Y
                            });
                        }
                    }
                    else
                    {
                        carets.Add(new CacheCaret
                        {
                            index = tmp,
                            i = i,
                            x = it.rect.X,
                            y = it.rect.Y
                        });
                    }
                    tmp++;
                }
                carets.Add(new CacheCaret
                {
                    index = tmp,
                    i = cache_font.Length,
                    x = last.rect.Right,
                    y = last.rect.Y
                });
                cache_caret = carets.ToArray();

                ScrollX.Max = last.rect.Right - rect_text.Right;
                switch (textalign)
                {
                    case HorizontalAlignment.Center:
                        if (ScrollX.Max > 0) ScrollX.Min = -ScrollX.Max;
                        else
                        {
                            ScrollX.Min = ScrollX.Max;
                            ScrollX.Max = -ScrollX.Max;
                        }
                        break;
                    case HorizontalAlignment.Right:
                        ScrollX.Min = cache_font[0].rect.Right - rect.Right + sps;
                        ScrollX.Max = 0;
                        break;
                    default:
                        ScrollX.Min = 0;
                        break;
                }
                ScrollY.Max = last.rect.Bottom - rect.Height + sps;
                if (multiline)
                {
                    if (ScrollX.SetValue(0)) rdcount++;
                    if (wordwrap) ScrollX.Show = false;
                    else
                    {
                        ScrollX.Max = maxx - rect_text.Right;
                        ScrollX.Show = maxx > rect.Right;
                    }
                    ScrollY.Show = last.rect.Bottom > rect.Bottom;
                    if (ScrollY.Show)
                    {
                        if (ScrollY.Value > ScrollY.Max && ScrollY.SetValue(ScrollY.Max)) rdcount++;
                    }
                    else if (ScrollY.SetValue(0)) rdcount++;
                }
                else
                {
                    oldtmpY = oldtmpX = null;
                    ScrollY.Show = false;
                    ScrollY.Value = 0;
                    if (textalign == HorizontalAlignment.Right) ScrollX.Show = last.rect.Right < rect.Right;
                    else ScrollX.Show = last.rect.Right > rect_text.Right;
                    if (ScrollX.Show)
                    {
                        if (ScrollX.Value > ScrollX.Max && ScrollX.SetValue(ScrollX.Max)) rdcount++;
                    }
                    else if (ScrollX.SetValue(0)) rdcount++;
                }
            }
            if (SetCaretPostion()) rdcount++;
            return rdcount > 0;
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

                int tabindex = GetTabIndex(cache_font);
                List<int> i_l = new List<int>(cache_font.Length), i_r = new List<int>(i_l.Count);
                if (tabindex == -1)
                {
                    for (int i = 0; i < cache_font.Length; i++)
                    {
                        var it = cache_font[i];
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
                        if (tabindex > 0)
                        {
                            var rect_tmp = cache_font[tabindex].rect;
                            rect_tmp.Offset(left, 0);
                            cache_font[tabindex].rect = rect_tmp;
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
                        if (tabindex > 0)
                        {
                            var rect_tmp = cache_font[tabindex].rect;
                            rect_tmp.Offset(left, 0);
                            cache_font[tabindex].rect = rect_tmp;
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
                var last = list[list.Count - 1];
                if (last.rect.Right < rect_text.Right)
                {
                    int w = rect_text.Right - last.rect.Right;
                    foreach (var it in list)
                    {
                        var rect_tmp = it.rect;
                        rect_tmp.Offset(w, 0);
                        it.rect = rect_tmp;
                    }
                }
                list.Clear();
            }
        }
        void HandTextAlignCenter(ref List<CacheFont> list)
        {
            if (list.Count > 0)
            {
                var last = list[list.Count - 1];
                if (last.rect.Right < rect_text.Right)
                {
                    int w = (rect_text.Right - last.rect.Right) / 2;
                    foreach (var it in list)
                    {
                        var rect_tmp = it.rect;
                        rect_tmp.Offset(w, 0);
                        it.rect = rect_tmp;
                    }
                }
                list.Clear();
            }
        }
        int GetTabIndex(CacheFont[] cache_font)
        {
            foreach (var it in cache_font)
            {
                if (it.text == "\t") return it.i;
            }
            return -1;
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
                int icon_size = (int)(read_height * iconratio), icon_right_size = icon_size;
                if (iconratioRight.HasValue) icon_right_size = (int)(read_height * iconratioRight.Value);
                if (has_prefixText) Helper.GDI(g => RectLR(rect, read_height, sps, sps2, g.MeasureString(prefixText, Font).Width, read_height, icon_right_size, icon_right_size));
                else if (has_prefix) RectLR(rect, read_height, sps, sps2, icon_size, icon_size, icon_right_size, icon_right_size);
                else RectR(rect, read_height, sps, sps2, icon_right_size, icon_right_size);
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
                                            int icon_size = (int)(read_height * (iconratioRight ?? iconratio));
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
                            int icon_size = (int)(read_height * iconratio), icon_right_size = icon_size;
                            if (iconratioRight.HasValue) icon_right_size = (int)(read_height * iconratioRight.Value);
                            if (has_prefix && has_suffix) RectLR(rect, read_height, sps, sps2, icon_size, icon_size, icon_right_size, icon_right_size);
                            else if (has_prefix) RectL(rect, read_height, sps, sps2, icon_size, icon_size);
                            else RectR(rect, read_height, sps, sps2, icon_right_size, icon_right_size);
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
            int ul = -1, sp = (int)(read_height * icongap);
            var round = RectLR(sp, sps, w_L, h_L, w_R, h_R, rect, out int hasx, out int hasr);
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
                int text_y = rect.Y + (rect.Height - read_height) / 2, icon_l_y = (rect.Height - h_L) / 2, icon_r_y = (rect.Height - h_R) / 2;
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, text_y + uy, rect.Width - hasr - ux, read_height - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, text_y, rect.Width - hasr, read_height);

                if (round[0]) rect_l = new Rectangle(rect.X + icon_l_y, rect.Y + icon_l_y, w_L, h_L);
                else rect_l = new Rectangle(rect.X + sps, rect.Y + icon_l_y, w_L, h_L);
                if (round[1]) rect_r = new Rectangle(rect.Right - w_R - icon_r_y, rect.Y + icon_r_y, w_R, h_R);
                else rect_r = new Rectangle(rect_text.Right + sp, rect.Y + icon_r_y, w_R, h_R);
            }
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }
        bool[] RectLR(int sp, int sps, int w_L, int h_L, int w_R, int h_R, Rectangle rect, out int hasx, out int hasr)
        {
            if (multiline)
            {
                hasx = sps + w_L + sp;
                hasr = w_L + w_R + ((sps + sp) * 2);
                return new bool[] { false, false };
            }
            else
            {
                bool round_L = round && w_L == h_L, round_R = round && w_R == h_R;
                if (round_L && round_R)
                {
                    hasx = rect.Height - sp;
                    hasr = (rect.Height - sp) * 2;
                }
                else if (round_L)
                {
                    hasx = rect.Height - sp;
                    hasr = w_L + w_R + ((sps + sp) * 2);
                }
                else if (round_R)
                {
                    hasx = sps + w_L + sp;
                    hasr = w_L + w_R + ((sps + sp) * 2);
                }
                else
                {
                    hasx = sps + w_L + sp;
                    hasr = w_L + w_R + ((sps + sp) * 2);
                }
                return new bool[] { round_L, round_R };
            }
        }

        #region 前缀布局

        void RectL(Rectangle rect, int read_height, int sps, int sps2, int w)
        {
            int ul = -1, sp = (int)(read_height * icongap);
            bool round = RectL(sp, sps, w, rect, false, out int hasx);
            int hasx2 = hasx + sps;
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
                int y = (rect.Height - read_height) / 2, ry = rect.Y + y;
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, ry + uy, rect.Width - hasx2 - ux, read_height - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, ry, rect.Width - hasx2, read_height);
                if (round) rect_l = new Rectangle(rect.X + y, ry, w, read_height);
                else rect_l = new Rectangle(rect.X + sps, ry, w, read_height);
            }
            rect_r.Width = 0;
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }
        void RectL(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int ul = -1, sp = (int)(read_height * icongap);
            bool round = RectL(sp, sps, w, rect, true, out int hasx);
            int hasx2 = hasx + sps;
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
                int text_y = rect.Y + (rect.Height - read_height) / 2, icon_l_y = (rect.Height - h) / 2;
                if (useLeft[0] > 0 || useLeft[1] > 0)
                {
                    int ux = useLeft[0], uy = useLeft[1];
                    rect_text = new Rectangle(rect.X + hasx + ux, text_y + uy, rect.Width - hasx2 - ux, read_height - uy);
                    ul = uy;
                }
                else rect_text = new Rectangle(rect.X + hasx, text_y, rect.Width - hasx2, read_height);
                if (round) rect_l = new Rectangle(rect.X + icon_l_y, rect.Y + icon_l_y, w, h);
                else rect_l = new Rectangle(rect.X + sps, rect.Y + icon_l_y, w, h);
            }
            rect_r.Width = 0;
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }
        bool RectL(int sp, int sps, int w, Rectangle rect, bool can_round, out int hasx)
        {
            if (multiline)
            {
                hasx = sps + w + sp;
                return false;
            }
            else if (round && can_round)
            {
                hasx = rect.Height - sp;
                return true;
            }
            else
            {
                hasx = sps + w + sp;
                return false;
            }
        }

        #endregion

        #region 后缀布局

        void RectR(Rectangle rect, int read_height, int sps, int sps2, int w)
        {
            int ul = -1, sp = (int)(read_height * icongap);
            RectL(sp, sps, w, rect, false, out int hasr);
            int hasr2 = hasr + sps;
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
                rect_text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - hasr2, rect.Height - sps2);
                rect_r = new Rectangle(rect_text.Right + sp, rect_text.Y, w, read_height);
            }
            else
            {
                int icon_r_y = (rect.Height - read_height) / 2;
                rect_text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasr2, read_height);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + icon_r_y, w, read_height);
            }
            rect_l.Width = 0;
            if (ul > -1) UseLeftAutoHeight(read_height, ul);
        }
        void RectR(Rectangle rect, int read_height, int sps, int sps2, int w, int h)
        {
            int ul = -1, sp = (int)(read_height * icongap);
            bool round = RectL(sp, sps, w, rect, false, out int hasr);
            int hasr2 = hasr + sps;
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
                rect_text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - hasr2, rect.Height - sps2);
                rect_r = new Rectangle(rect_text.Right + sp, rect.Y + sps + (read_height - h) / 2, w, h);
            }
            else
            {
                int icon_r_y = (rect.Height - h) / 2;
                rect_text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - read_height) / 2, rect.Width - hasr2, read_height);
                if (round) rect_r = new Rectangle(rect.Right - w - icon_r_y, rect.Y + icon_r_y, w, h);
                else rect_r = new Rectangle(rect_text.Right + sp, rect.Y + icon_r_y, w, h);
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