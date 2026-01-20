// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AntdUI.Core
{
    public class CanvasGDI : Canvas
    {
        public Graphics g;
        public CanvasGDI(Graphics gdi)
        {
            g = gdi;
            if (Config._dpi_custom.HasValue) Dpi = Config._dpi_custom.Value;
            else Dpi = gdi.DpiX / 96F;
        }

        #region MeasureString

        public Size MeasureString(string? text, Font font) => MeasureString(text, font, 0, FormatFlags.Center);
        public Size MeasureString(string? text, Font font, int width) => MeasureString(text, font, width, FormatFlags.Center);
        public Size MeasureString(string? text, Font font, int width, StringFormat format) => g.MeasureString(text, font, width, format).Size();
        public Size MeasureString(string? text, Font font, int width, FormatFlags format = FormatFlags.Center) => g.MeasureString(text, font, width, Helper.TF(format, true)).Size();
        public Region[] MeasureCharacterRanges(string? text, Font font, Rectangle rect, FormatFlags format = FormatFlags.Center) => g.MeasureCharacterRanges(text, font, rect, Helper.TF(format, true));
        public Region[] MeasureCharacterRanges(string? text, Font font, Rectangle rect, StringFormat format) => g.MeasureCharacterRanges(text, font, rect, format);

        #endregion

        #region String

        public void String(string? text, Font font, Color color, Rectangle rect, StringFormat format)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, rect, format);
            }
        }
        public void String(string? text, Font font, Brush brush, Rectangle rect, StringFormat format)
        {
            if (text == null) return;
            CorrectionTextRendering.CORE(font, text, ref rect);
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), rect, format);
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, rect, format);
        }
        public void String(string? text, Font font, Color color, RectangleF rect, StringFormat format)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, rect, format);
            }
        }
        public void String(string? text, Font font, Brush brush, RectangleF rect, StringFormat format)
        {
            if (text == null) return;
            CorrectionTextRendering.CORE(font, text, ref rect);
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), rect, format);
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, rect, format);
        }

        public void String(string? text, Font font, Color color, Rectangle rect, FormatFlags format = FormatFlags.Center)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, rect, format);
            }
        }
        public void String(string? text, Font font, Brush brush, Rectangle rect, FormatFlags format = FormatFlags.Center)
        {
            if (text == null) return;
            CorrectionTextRendering.CORE(font, text, ref rect);
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), rect, Helper.TF(format));
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, rect, Helper.TF(format));
        }
        public void String(string? text, Font font, Color color, RectangleF rect, FormatFlags format = FormatFlags.Center)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, rect, format);
            }
        }
        public void String(string? text, Font font, Brush brush, RectangleF rect, FormatFlags format = FormatFlags.Center)
        {
            if (text == null) return;
            CorrectionTextRendering.CORE(font, text, ref rect);
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), rect, Helper.TF(format));
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, rect, Helper.TF(format));
        }

        public void String(string? text, Font font, Color color, int x, int y)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, x, y);
            }
        }

        public void String(string? text, Font font, Brush brush, int x, int y)
        {
            if (text == null) return;
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), new Point(x, y), null);
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, x, y);
        }

        public void String(string? text, Font font, Color color, float x, float y)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, x, y);
            }
        }

        public void String(string? text, Font font, Brush brush, float x, float y)
        {
            if (text == null) return;
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), new PointF(x, y), null);
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, x, y);
        }


        public void String(string? text, Font font, Color color, Point point)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, point);
            }
        }

        public void String(string? text, Font font, Brush brush, Point point)
        {
            if (text == null) return;
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), point, null);
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, point);
        }


        public void String(string? text, Font font, Color color, PointF point)
        {
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, point);
            }
        }

        public void String(string? text, Font font, Brush brush, PointF point)
        {
            if (text == null) return;
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, StringPathFontSize(font), point, null);
                    Fill(brush, path);
                }
            }
            else g.DrawString(text, font, brush, point);
        }

        float StringPathFontSize(Font font)
        {
            switch (font.Unit)
            {
                case GraphicsUnit.Point:
                    return font.Size * (g.DpiY / 72);
                case GraphicsUnit.Pixel:
                    return font.Size;
                case GraphicsUnit.Inch:
                    return font.Size * g.DpiY;
                case GraphicsUnit.Display:
                    return font.Size * .01F * g.DpiY;
                case GraphicsUnit.Document:
                    return font.Size * (1 / 300F) * g.DpiY;
                case GraphicsUnit.Millimeter:
                    return font.Size * (1 / 25.4F) * g.DpiY;
                case GraphicsUnit.World:
                    return font.Size * g.PageScale;
                default:
                    return font.Size * (g.DpiY / 72);
            }
        }

        #endregion

        #region MeasureText

        public Size MeasureText(string? text, Font font) => MeasureText(text, font, 0, FormatFlags.Center);
        public Size MeasureText(string? text, Font font, int width) => MeasureText(text, font, width, FormatFlags.Center);
        public Size MeasureText(string? text, Font font, int width, StringFormat format)
        {
            if (SvgDb.Emoji.Count == 0 || text == null) return MeasureString(text, font, width, format);
            else
            {
                var characters = new List<TMPChar>(text.Length);
                int emojiCount = 0;
                GraphemeSplitter.Each(text, (txt, ntype) =>
                {
                    if ((ntype == 18 || ntype == 4) && SvgDb.Emoji.ContainsKey(txt))
                    {
                        characters.Add(new TMPChar(txt, true));
                        emojiCount++;
                    }
                    else characters.Add(new TMPChar(txt, false));
                });
                if (emojiCount > 0) return MeasureText(ref characters, width, MeasureText(font, width, ref characters));
                else return MeasureString(text, font, width, format);
            }
        }
        public Size MeasureText(string? text, Font font, int width, FormatFlags format = FormatFlags.Center)
        {
            if (SvgDb.Emoji.Count == 0 || text == null) return MeasureString(text, font, width, format);
            else
            {
                var characters = new List<TMPChar>(text.Length);
                int emojiCount = 0;
                GraphemeSplitter.Each(text, (txt, ntype) =>
                {
                    if ((ntype == 18 || ntype == 4) && SvgDb.Emoji.ContainsKey(txt))
                    {
                        characters.Add(new TMPChar(txt, true));
                        emojiCount++;
                    }
                    else characters.Add(new TMPChar(txt, false));
                });
                if (emojiCount > 0) return MeasureText(ref characters, width, MeasureText(font, width, ref characters));
                else return MeasureString(text, font, width, format);
            }
        }

        int MeasureText(Font font, int width, ref List<TMPChar> characters)
        {
            int h = MeasureString(Config.NullText, font).Height;
            bool h_change = false;
            foreach (var it in characters)
            {
                if (it.emoji) it.w = h;
                else
                {
                    var size = MeasureString(it.txt, font, width);
                    it.w = size.Width;
                    if (h > size.Height)
                    {
                        h = size.Height;
                        h_change = true;
                    }
                }
            }
            if (h_change)
            {
                foreach (var it in characters)
                {
                    if (it.emoji) it.w = h;
                }
            }
            return h;
        }

        Size MeasureText(ref List<TMPChar> characters, int width, int height)
        {
            int w = 0, h = height;
            if (width > 0)
            {
                int x = 0, line = 0;
                for (int i = 0; i < characters.Count; i++)
                {
                    var it = characters[i];
                    if (it.emoji)
                    {
                        x += it.w;
                        if (x + DrawTextNextChar(characters, i + 1) > width)
                        {
                            x = 0;
                            h += height;
                            line++;
                        }
                        else w += it.w;
                    }
                    else
                    {
                        x += it.w;
                        if (x + DrawTextNextChar(characters, i + 1) > width)
                        {
                            x = 0;
                            h += height;
                            line++;
                        }
                        else w += it.w;
                    }
                    it.line = line;
                }
                if (w > width) w = width;
            }
            else
            {
                foreach (var it in characters) w += it.w;
            }
            return new Size(w, h);
        }

        internal class TMPChar
        {
            public TMPChar(string t, bool e)
            {
                txt = t;
                emoji = e;
            }
            public string txt { get; set; }
            public bool emoji { get; set; }
            public int w { get; set; }
            public int line { get; set; }
        }

        #endregion

        #region DrawText

        public void DrawText(string? text, Font font, Color color, Rectangle rect, StringFormat format)
        {
            using (var brush = new SolidBrush(color))
            {
                DrawText(text, font, brush, rect, format);
            }
        }

        public void DrawText(string? text, Font font, Brush brush, Rectangle rect, StringFormat format)
        {
            if (SvgDb.Emoji.Count == 0) String(text, font, brush, rect, format);
            else
            {
                if (text == null) return;
                var characters = new List<TMPChar>(text.Length);
                int emojiCount = 0;
                GraphemeSplitter.Each(text, (txt, ntype) =>
                {
                    if ((ntype == 18 || ntype == 4) && SvgDb.Emoji.ContainsKey(txt))
                    {
                        characters.Add(new TMPChar(txt, true));
                        emojiCount++;
                    }
                    else characters.Add(new TMPChar(txt, false));
                });
                if (emojiCount > 0) DrawText(text, font, brush, rect, characters, format);
                else String(text, font, brush, rect, format);
            }
        }

        void DrawText(string? text, Font font, Brush brush, Rectangle rect, List<TMPChar> characters, StringFormat format)
        {
            CorrectionTextRendering.CORE(font, text, ref rect);
            int lineHeight = MeasureText(font, rect.Width, ref characters);
            var sizeT = MeasureText(ref characters, rect.Width, lineHeight);
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    float fontsize = StringPathFontSize(font);

                    bool wrap = format.FormatFlags.HasFlag(StringFormatFlags.NoWrap);
                    bool ellipsis = format.Trimming.HasFlag(StringTrimming.EllipsisCharacter);

                    int y;
                    if (format.LineAlignment == StringAlignment.Center) y = rect.Y + (rect.Height - sizeT.Height) / 2;
                    else if (format.LineAlignment == StringAlignment.Far) y = rect.Bottom - sizeT.Height;
                    else y = rect.Y;

                    DrawText(characters, path, fontsize, rect.X, y, font, brush, lineHeight, rect, wrap, ellipsis, format.Alignment);

                    Fill(brush, path);
                }
            }
            else
            {
                bool wrap = format.FormatFlags.HasFlag(StringFormatFlags.NoWrap);
                bool ellipsis = format.Trimming.HasFlag(StringTrimming.EllipsisCharacter);

                int y;
                if (format.LineAlignment == StringAlignment.Center) y = rect.Y + (rect.Height - sizeT.Height) / 2;
                else if (format.LineAlignment == StringAlignment.Far) y = rect.Bottom - sizeT.Height;
                else y = rect.Y;

                DrawText(characters, rect.X, y, font, brush, lineHeight, rect, wrap, ellipsis, format.Alignment);
            }
        }

        void DrawText(List<TMPChar> characters, GraphicsPath path, float fontsize, int x, int y, Font font, Brush brush, int lineHeight, Rectangle rect, bool wrap, bool ellipsis, StringAlignment alignment)
        {
            int use_x = 0, use_y = 0, use_line = 0;
            switch (alignment)
            {
                case StringAlignment.Far:
                    x = rect.Right - DrawTextLineWidth(characters, use_line);
                    break;
                case StringAlignment.Center:
                    x = rect.X + (rect.Width - DrawTextLineWidth(characters, use_line)) / 2;
                    break;
            }
            for (int i = 0; i < characters.Count; i++)
            {
                var it = characters[i];
                if (DrawTextNextChar(characters, i + 1, rect, lineHeight, use_y, use_line, wrap, ellipsis))
                {
                    string ellipsisText = "...";
                    Size ellipsisSize = MeasureString(ellipsisText, font);
                    path.AddString(ellipsisText, font.FontFamily, (int)font.Style, fontsize, new Rectangle(x + use_x, y + use_y, ellipsisSize.Width, lineHeight), Helper.TF(FormatFlags.Center));
                    return;
                }
                if (use_line < it.line)
                {
                    use_line = it.line;
                    use_x = 0;
                    use_y += lineHeight;
                    switch (alignment)
                    {
                        case StringAlignment.Far:
                            x = rect.Right - DrawTextLineWidth(characters, it.line);
                            break;
                        case StringAlignment.Center:
                            x = rect.X + (rect.Width - DrawTextLineWidth(characters, it.line)) / 2;
                            break;
                    }
                }
                if (it.emoji)
                {
                    var svg = SvgDb.Emoji[it.txt];
                    var rect_ico = new Rectangle(x + use_x, y + use_y, lineHeight, lineHeight);
                    if (brush is SolidBrush solid) SvgExtend.GetImgExtend(this, svg, rect_ico, solid.Color);
                    else SvgExtend.GetImgExtend(this, svg, rect_ico);
                }
                else path.AddString(it.txt, font.FontFamily, (int)font.Style, fontsize, new Rectangle(x + use_x, y + use_y, it.w, lineHeight), Helper.TF(FormatFlags.Center));
                use_x += it.w;
            }
        }
        void DrawText(List<TMPChar> characters, int x, int y, Font font, Brush brush, int lineHeight, Rectangle rect, bool wrap, bool ellipsis, StringAlignment alignment)
        {
            int use_x = 0, use_y = 0, use_line = 0;
            switch (alignment)
            {
                case StringAlignment.Far:
                    x = rect.Right - DrawTextLineWidth(characters, use_line);
                    break;
                case StringAlignment.Center:
                    x = rect.X + (rect.Width - DrawTextLineWidth(characters, use_line)) / 2;
                    break;
            }
            for (int i = 0; i < characters.Count; i++)
            {
                var it = characters[i];
                if (DrawTextNextChar(characters, i + 1, rect, lineHeight, use_y, use_line, wrap, ellipsis))
                {
                    string ellipsisText = "...";
                    Size ellipsisSize = MeasureString(ellipsisText, font);
                    String(ellipsisText, font, brush, new Rectangle(x + use_x, y + use_y, ellipsisSize.Width, lineHeight));
                    return;
                }
                if (use_line < it.line)
                {
                    use_line = it.line;
                    use_x = 0;
                    use_y += lineHeight;
                    switch (alignment)
                    {
                        case StringAlignment.Far:
                            x = rect.Right - DrawTextLineWidth(characters, it.line);
                            break;
                        case StringAlignment.Center:
                            x = rect.X + (rect.Width - DrawTextLineWidth(characters, it.line)) / 2;
                            break;
                    }
                }
                if (it.emoji)
                {
                    var svg = SvgDb.Emoji[it.txt];
                    var rect_ico = new Rectangle(x + use_x, y + use_y, lineHeight, lineHeight);
                    if (brush is SolidBrush solid) SvgExtend.GetImgExtend(this, svg, rect_ico, solid.Color);
                    else SvgExtend.GetImgExtend(this, svg, rect_ico);
                }
                else String(it.txt, font, brush, new Rectangle(x + use_x, y + use_y, it.w, lineHeight));
                use_x += it.w;
            }
        }


        public void DrawText(string? text, Font font, Color color, Rectangle rect, FormatFlags format = FormatFlags.Center)
        {
            using (var brush = new SolidBrush(color))
            {
                DrawText(text, font, brush, rect, format);
            }
        }

        public void DrawText(string? text, Font font, Brush brush, Rectangle rect, FormatFlags format = FormatFlags.Center)
        {
            if (SvgDb.Emoji.Count == 0) String(text, font, brush, rect, format);
            else
            {
                if (text == null) return;
                var characters = new List<TMPChar>(text.Length);
                int emojiCount = 0;
                GraphemeSplitter.Each(text, (txt, ntype) =>
                {
                    if ((ntype == 18 || ntype == 4) && SvgDb.Emoji.ContainsKey(txt))
                    {
                        characters.Add(new TMPChar(txt, true));
                        emojiCount++;
                    }
                    else characters.Add(new TMPChar(txt, false));
                });
                if (emojiCount > 0) DrawText(text, font, brush, rect, characters, format);
                else String(text, font, brush, rect, format);
            }
        }

        void DrawText(string? text, Font font, Brush brush, Rectangle rect, List<TMPChar> characters, FormatFlags format)
        {
            CorrectionTextRendering.CORE(font, text, ref rect);
            int lineHeight = MeasureText(font, rect.Width, ref characters);
            var sizeT = MeasureText(ref characters, rect.Width, lineHeight);
            if (Config.TextRenderingHighQuality)
            {
                using (var path = new GraphicsPath())
                {
                    float fontsize = StringPathFontSize(font);

                    bool wrap = format.HasFlag(FormatFlags.NoWrap);
                    bool ellipsis = format.HasFlag(FormatFlags.EllipsisCharacter);

                    int y;
                    if (format.HasFlag(FormatFlags.VerticalCenter)) y = rect.Y + (rect.Height - sizeT.Height) / 2;
                    else if (format.HasFlag(FormatFlags.Bottom)) y = rect.Bottom - sizeT.Height;
                    else y = rect.Y;

                    DrawText(characters, path, fontsize, rect.X, y, font, brush, lineHeight, rect, wrap, ellipsis, format);
                    Fill(brush, path);
                }
            }
            else
            {
                bool wrap = format.HasFlag(FormatFlags.NoWrap);
                bool ellipsis = format.HasFlag(FormatFlags.EllipsisCharacter);

                int y;
                if (format.HasFlag(FormatFlags.VerticalCenter)) y = rect.Y + (rect.Height - sizeT.Height) / 2;
                else if (format.HasFlag(FormatFlags.Bottom)) y = rect.Bottom - sizeT.Height;
                else y = rect.Y;

                DrawText(characters, rect.X, y, font, brush, lineHeight, rect, wrap, ellipsis, format);
            }
        }

        void DrawText(List<TMPChar> characters, GraphicsPath path, float fontsize, int x, int y, Font font, Brush brush, int lineHeight, Rectangle rect, bool wrap, bool ellipsis, FormatFlags format)
        {
            int use_x = 0, use_y = 0, use_line = 0;
            if (format.HasFlag(FormatFlags.HorizontalCenter)) x = rect.X + (rect.Width - DrawTextLineWidth(characters, use_line)) / 2;
            else if (format.HasFlag(FormatFlags.Right)) x = rect.Right - DrawTextLineWidth(characters, use_line);
            for (int i = 0; i < characters.Count; i++)
            {
                var it = characters[i];
                if (DrawTextNextChar(characters, i + 1, rect, lineHeight, use_y, use_line, wrap, ellipsis))
                {
                    string ellipsisText = "...";
                    Size ellipsisSize = MeasureString(ellipsisText, font);
                    path.AddString(ellipsisText, font.FontFamily, (int)font.Style, fontsize, new Rectangle(x + use_x, y + use_y, ellipsisSize.Width, lineHeight), Helper.TF(FormatFlags.Center));
                    return;
                }
                if (use_line < it.line)
                {
                    use_line = it.line;
                    use_x = 0;
                    use_y += lineHeight;
                    if (format.HasFlag(FormatFlags.HorizontalCenter)) x = rect.X + (rect.Width - DrawTextLineWidth(characters, it.line)) / 2;
                    else if (format.HasFlag(FormatFlags.Right)) x = rect.Right - DrawTextLineWidth(characters, it.line);
                }
                if (it.emoji)
                {
                    var svg = SvgDb.Emoji[it.txt];
                    var rect_ico = new Rectangle(x + use_x, y + use_y, lineHeight, lineHeight);
                    if (brush is SolidBrush solid) SvgExtend.GetImgExtend(this, svg, rect_ico, solid.Color);
                    else SvgExtend.GetImgExtend(this, svg, rect_ico);
                }
                else path.AddString(it.txt, font.FontFamily, (int)font.Style, fontsize, new Rectangle(x + use_x, y + use_y, it.w, lineHeight), Helper.TF(FormatFlags.Center));
                use_x += it.w;
            }
        }
        void DrawText(List<TMPChar> characters, int x, int y, Font font, Brush brush, int lineHeight, Rectangle rect, bool wrap, bool ellipsis, FormatFlags format)
        {
            int use_x = 0, use_y = 0, use_line = 0;
            if (format.HasFlag(FormatFlags.HorizontalCenter)) x = rect.X + (rect.Width - DrawTextLineWidth(characters, use_line)) / 2;
            else if (format.HasFlag(FormatFlags.Right)) x = rect.Right - DrawTextLineWidth(characters, use_line);
            for (int i = 0; i < characters.Count; i++)
            {
                var it = characters[i];
                if (DrawTextNextChar(characters, i + 1, rect, lineHeight, use_y, use_line, wrap, ellipsis))
                {
                    string ellipsisText = "...";
                    Size ellipsisSize = MeasureString(ellipsisText, font);
                    String(ellipsisText, font, brush, new Rectangle(x + use_x, y + use_y, ellipsisSize.Width, lineHeight));
                    return;
                }
                if (use_line < it.line)
                {
                    use_line = it.line;
                    use_x = 0;
                    use_y += lineHeight;
                    if (format.HasFlag(FormatFlags.HorizontalCenter)) x = rect.X + (rect.Width - DrawTextLineWidth(characters, it.line)) / 2;
                    else if (format.HasFlag(FormatFlags.Right)) x = rect.Right - DrawTextLineWidth(characters, it.line);
                }
                if (it.emoji)
                {
                    var svg = SvgDb.Emoji[it.txt];
                    var rect_ico = new Rectangle(x + use_x, y + use_y, lineHeight, lineHeight);
                    if (brush is SolidBrush solid) SvgExtend.GetImgExtend(this, svg, rect_ico, solid.Color);
                    else SvgExtend.GetImgExtend(this, svg, rect_ico);
                }
                else String(it.txt, font, brush, new Rectangle(x + use_x, y + use_y, it.w, lineHeight));
                use_x += it.w;
            }
        }

        int DrawTextLineWidth(List<TMPChar> characters, int i)
        {
            int w = 0;
            foreach (var it in characters)
            {
                if (it.line == i) w += it.w;
            }
            return w;
        }
        int DrawTextNextChar(List<TMPChar> characters, int i)
        {
            if (characters.Count > i) return characters[i].w;
            return 0;
        }
        bool DrawTextNextChar(List<TMPChar> characters, int i, Rectangle rect, int lineHeight, int use_y, int use_line, bool wrap, bool ellipsis)
        {
            if (characters.Count > i)
            {
                var it = characters[i];
                if (use_line < it.line)
                {
                    if (wrap || rect.Height < use_y + lineHeight * 2) return ellipsis;
                }
            }
            return false;
        }

        #endregion

        #region Image

        public bool Image(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttr)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, null);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool Image(Image image, float x, float y, float w, float h)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, x, y, w, h);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool Image(Image image, int x, int y, int w, int h)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, x, y, w, h);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool Image(Image image, int x, int y)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, x, y);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool Icon(Icon icon, Rectangle rect)
        {
            try
            {
                lock (icon)
                {
                    g.DrawIcon(icon, rect);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool Image(Image image, Rectangle rect)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, rect);
                    return true;
                }
            }
            catch { }
            return false;
        }
        public bool Image(Image image, RectangleF rect)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, rect);
                    return true;
                }
            }
            catch { }
            return false;
        }
        public bool Image(Image image, Rectangle destRect, Rectangle srcRect) => Image(image, destRect, srcRect, GraphicsUnit.Pixel);
        public bool Image(Image image, RectangleF destRect, RectangleF srcRect) => Image(image, destRect, srcRect, GraphicsUnit.Pixel);
        public bool Image(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, destRect, srcRect, srcUnit);
                    return true;
                }
            }
            catch { }
            return false;
        }
        public bool Image(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            try
            {
                lock (image)
                {
                    g.DrawImage(image, destRect, srcRect, srcUnit);
                    return true;
                }
            }
            catch { }
            return false;
        }

        #region 图片透明度

        public bool Image(Image bmp, Rectangle rect, float opacity)
        {
            try
            {
                lock (bmp)
                {
                    if (opacity >= 1F)
                    {
                        g.DrawImage(bmp, rect);
                        return true;
                    }
                    using (var attributes = new ImageAttributes())
                    {
                        var matrix = new ColorMatrix { Matrix33 = opacity };
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        g.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                    }
                    return true;
                }
            }
            catch { }
            return false;
        }
        public bool Image(Image bmp, Rectangle destRect, Rectangle srcRect, float opacity) => Image(bmp, destRect, srcRect, opacity, GraphicsUnit.Pixel);
        public bool Image(Image bmp, Rectangle destRect, Rectangle srcRect, float opacity, GraphicsUnit srcUnit)
        {
            try
            {
                lock (bmp)
                {
                    if (opacity >= 1F)
                    {
                        Image(bmp, destRect, srcRect, srcUnit);
                        return true;
                    }
                    using (var attributes = new ImageAttributes())
                    {
                        var matrix = new ColorMatrix { Matrix33 = opacity };
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        g.DrawImage(bmp, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, attributes);
                    }
                    return true;
                }
            }
            catch { }
            return false;
        }
        public bool Image(Image bmp, RectangleF destRect, RectangleF srcRect, float opacity) => Image(bmp, destRect, srcRect, opacity, GraphicsUnit.Pixel);
        public bool Image(Image bmp, RectangleF destRect, RectangleF srcRect, float opacity, GraphicsUnit srcUnit)
        {
            try
            {
                lock (bmp)
                {
                    if (opacity >= 1F)
                    {
                        Image(bmp, destRect, srcRect, srcUnit);
                        return true;
                    }
                    using (var attributes = new ImageAttributes())
                    {
                        var matrix = new ColorMatrix { Matrix33 = opacity };
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        var points = new[]
                        {
                            destRect.Location,
                            new PointF(destRect.X + destRect.Width, destRect.Y),
                            new PointF(destRect.X, destRect.Y + destRect.Height)
                        };
                        g.DrawImage(bmp, points, srcRect, srcUnit, attributes);
                    }
                    return true;
                }
            }
            catch { }
            return false;
        }

        #endregion

        public bool Image(RectangleF rect, Image image, TFit fit)
        {
            if (rect.Width > 0 && rect.Height > 0)
            {
                try
                {
                    switch (fit)
                    {
                        case TFit.Fill:
                            g.DrawImage(image, rect);
                            break;
                        case TFit.None:
                            g.DrawImage(image, new RectangleF(rect.X + (rect.Width - image.Width) / 2, rect.Y + (rect.Height - image.Height) / 2, image.Width, image.Height));
                            break;
                        case TFit.Contain:
                            PaintImgContain(this, image, rect);
                            break;
                        case TFit.Cover:
                            PaintImgCover(this, image, rect);
                            break;
                    }
                    return true;
                }
                catch { }
            }
            return false;
        }
        public bool Image(RectangleF rect, Image image, TFit fit, float radius, bool round)
        {
            if (rect.Width > 0 && rect.Height > 0)
            {
                try
                {
                    if (round || radius > 0)
                    {
                        using (var bmp = new Bitmap((int)rect.Width, (int)rect.Height))
                        {
                            using (var g2 = Graphics.FromImage(bmp).High())
                            {
                                PaintImg(g2, new RectangleF(0, 0, rect.Width, rect.Height), image, fit);
                            }
                            using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                            {
                                brush.TranslateTransform(rect.X, rect.Y);
                                if (round) g.FillEllipse(brush, rect);
                                else
                                {
                                    using (var path = rect.RoundPath(radius))
                                    {
                                        g.FillPath(brush, path);
                                    }
                                }
                            }
                        }
                    }
                    else PaintImg(this, rect, image, fit);
                    return true;
                }
                catch { }
            }
            return false;
        }
        public bool Image(RectangleF rect, Image image, TFit fit, float radius, TShape shape)
        {
            if (rect.Width > 0 && rect.Height > 0)
            {
                try
                {
                    if (shape == TShape.Circle || shape == TShape.Round || radius > 0)
                    {
                        using (var bmp = new Bitmap((int)rect.Width, (int)rect.Height))
                        {
                            using (var g2 = Graphics.FromImage(bmp).High())
                            {
                                PaintImg(g2, new RectangleF(0, 0, rect.Width, rect.Height), image, fit);
                            }
                            using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                            {
                                brush.TranslateTransform(rect.X, rect.Y);
                                if (shape == TShape.Circle) g.FillEllipse(brush, rect);
                                else
                                {
                                    using (var path = rect.RoundPath(radius))
                                    {
                                        g.FillPath(brush, path);
                                    }
                                }
                            }
                        }
                    }
                    else PaintImg(this, rect, image, fit);
                    return true;
                }
                catch { }
            }
            return false;
        }

        static bool PaintImg(Canvas g, RectangleF rect, Image image, TFit fit)
        {
            try
            {
                switch (fit)
                {
                    case TFit.Fill:
                        g.Image(image, rect);
                        break;
                    case TFit.None:
                        g.Image(image, new RectangleF(rect.X + (rect.Width - image.Width) / 2, rect.Y + (rect.Height - image.Height) / 2, image.Width, image.Height));
                        break;
                    case TFit.Contain:
                        PaintImgContain(g, image, rect);
                        break;
                    case TFit.Cover:
                        PaintImgCover(g, image, rect);
                        break;
                }
                return true;
            }
            catch { }
            return false;
        }
        static void PaintImgCover(Canvas g, Image image, RectangleF rect)
        {
            float originWidth = image.Width, originHeight = image.Height;
            if (originWidth == originHeight)
            {
                if (rect.Width == rect.Height) g.Image(image, rect);
                else if (rect.Width > rect.Height) g.Image(image, new RectangleF(rect.X, rect.Y + (rect.Height - rect.Width) / 2, rect.Width, rect.Width));
                else g.Image(image, new RectangleF(rect.X + (rect.Width - rect.Height) / 2, rect.Y, rect.Height, rect.Height));
                return;
            }
            float destWidth = rect.Width, destHeight = rect.Height;
            float currentWidth, currentHeight;
            if ((originWidth * destHeight) > (originHeight * destWidth))
            {
                currentHeight = destHeight;
                currentWidth = (originWidth * destHeight) / originHeight;
            }
            else
            {
                currentWidth = destWidth;
                currentHeight = (destWidth * originHeight) / originWidth;
            }
            g.Image(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }
        static void PaintImgContain(Canvas g, Image image, RectangleF rect)
        {
            float originWidth = image.Width, originHeight = image.Height;
            if (originWidth == originHeight)
            {
                if (rect.Width == rect.Height) g.Image(image, rect);
                else if (rect.Width > rect.Height) g.Image(image, new RectangleF(rect.X + (rect.Width - rect.Height) / 2, rect.Y, rect.Height, rect.Height));
                else g.Image(image, new RectangleF(rect.X, rect.Y + (rect.Height - rect.Width) / 2, rect.Width, rect.Width));
                return;
            }
            float destWidth = rect.Width, destHeight = rect.Height;
            float currentWidth, currentHeight;
            if ((originWidth * destHeight) > (originHeight * destWidth))
            {
                currentWidth = destWidth;
                currentHeight = (destWidth * originHeight) / originWidth;
            }
            else
            {
                currentHeight = destHeight;
                currentWidth = (originWidth * destHeight) / originHeight;
            }
            g.Image(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }

        #endregion

        #region Fill

        public void Fill(Brush brush, GraphicsPath path)
        {
            try
            {
                g.FillPath(brush, path);
            }
            catch { }
        }
        public void Fill(Brush brush, Rectangle rect) => g.FillRectangle(brush, rect);
        public void Fill(Brush brush, RectangleF rect) => g.FillRectangle(brush, rect);
        public void Fill(Brush brush, int x, int y, int w, int h) => g.FillRectangle(brush, x, y, w, h);
        public void Fill(Brush brush, float x, float y, float w, float h) => g.FillRectangle(brush, x, y, w, h);

        public void Fill(Color color, GraphicsPath path)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, path);
            }
        }
        public void Fill(Color color, Rectangle rect)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, rect);
            }
        }
        public void Fill(Color color, RectangleF rect)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, rect);
            }
        }
        public void Fill(Color color, int x, int y, int w, int h)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, x, y, w, h);
            }
        }
        public void Fill(Color color, float x, float y, float w, float h)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, x, y, w, h);
            }
        }

        public void FillEllipse(Brush brush, Rectangle rect) => g.FillEllipse(brush, rect);
        public void FillEllipse(Brush brush, RectangleF rect) => g.FillEllipse(brush, rect);
        public void FillEllipse(Color color, Rectangle rect)
        {
            using (var brush = new SolidBrush(color))
            {
                FillEllipse(brush, rect);
            }
        }
        public void FillEllipse(Color color, RectangleF rect)
        {
            using (var brush = new SolidBrush(color))
            {
                FillEllipse(brush, rect);
            }
        }

        public void FillPolygon(Brush brush, Point[] points) => g.FillPolygon(brush, points);
        public void FillPolygon(Brush brush, PointF[] points) => g.FillPolygon(brush, points);
        public void FillPolygon(Color color, Point[] points)
        {
            using (var brush = new SolidBrush(color))
            {
                FillPolygon(brush, points);
            }
        }
        public void FillPolygon(Color color, PointF[] points)
        {
            using (var brush = new SolidBrush(color))
            {
                FillPolygon(brush, points);
            }
        }

        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle) => g.FillPie(brush, rect, startAngle, sweepAngle);
        public void FillPie(Brush brush, RectangleF rect, float startAngle, float sweepAngle) => g.FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        public void FillPie(Brush brush, float x, float y, float w, float h, float startAngle, float sweepAngle) => g.FillPie(brush, x, y, w, h, startAngle, sweepAngle);

        public void FillClosedCurve(Brush brush, params Point[] points) => g.FillClosedCurve(brush, points);
        public void FillClosedCurve(Brush brush, params PointF[] points) => g.FillClosedCurve(brush, points);
        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode) => g.FillClosedCurve(brush, points, fillmode);
        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode) => g.FillClosedCurve(brush, points, fillmode);

        #endregion

        #region Draw

        public void Draw(Pen pen, GraphicsPath path) => g.DrawPath(pen, path);
        public void Draw(Pen pen, Rectangle rect) => g.DrawRectangle(pen, rect);
        public void Draw(Pen pen, RectangleF rect) => g.DrawRectangles(pen, new RectangleF[] { rect });

        public void Draw(Brush brush, float width, GraphicsPath path)
        {
            using (var pen = new Pen(brush, width))
            {
                Draw(pen, path);
            }
        }
        public void Draw(Color color, float width, GraphicsPath path)
        {
            using (var pen = new Pen(color, width))
            {
                Draw(pen, path);
            }
        }
        public void Draw(Color color, float width, Rectangle rect)
        {
            using (var pen = new Pen(color, width))
            {
                Draw(pen, rect);
            }
        }
        public void Draw(Color color, float width, RectangleF rect)
        {
            using (var pen = new Pen(color, width))
            {
                Draw(pen, rect);
            }
        }
        public void Draw(Color color, float width, DashStyle dashStyle, GraphicsPath path)
        {
            using (var pen = new Pen(color, width))
            {
                pen.DashStyle = dashStyle;
                Draw(pen, path);
            }
        }
        public void Draw(Color color, float width, DashStyle dashStyle, Rectangle rect)
        {
            using (var pen = new Pen(color, width))
            {
                pen.DashStyle = dashStyle;
                Draw(pen, rect);
            }
        }
        public void Draw(Color color, float width, DashStyle dashStyle, RectangleF rect)
        {
            using (var pen = new Pen(color, width))
            {
                pen.DashStyle = dashStyle;
                Draw(pen, rect);
            }
        }

        public void DrawEllipse(Pen pen, Rectangle rect) => g.DrawEllipse(pen, rect);
        public void DrawEllipse(Pen pen, RectangleF rect) => g.DrawEllipse(pen, rect);
        public void DrawEllipse(Color color, float width, Rectangle rect)
        {
            using (var pen = new Pen(color, width))
            {
                g.DrawEllipse(pen, rect);
            }
        }
        public void DrawEllipse(Color color, float width, RectangleF rect)
        {
            using (var pen = new Pen(color, width))
            {
                g.DrawEllipse(pen, rect);
            }
        }

        public void DrawPolygon(Pen pen, Point[] points) => g.DrawPolygon(pen, points);
        public void DrawPolygon(Pen pen, PointF[] points) => g.DrawPolygon(pen, points);
        public void DrawPolygon(Color color, float width, Point[] points)
        {
            using (var pen = new Pen(color, width))
            {
                DrawPolygon(pen, points);
            }
        }
        public void DrawPolygon(Color color, float width, PointF[] points)
        {
            using (var pen = new Pen(color, width))
            {
                DrawPolygon(pen, points);
            }
        }

        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            try
            {
                g.DrawArc(pen, rect, startAngle, sweepAngle);
            }
            catch { }
        }
        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            try
            {
                g.DrawArc(pen, rect, startAngle, sweepAngle);
            }
            catch { }
        }
        public void DrawArc(Color color, float width, Rectangle rect, float startAngle, float sweepAngle)
        {
            using (var pen = new Pen(color, width))
            {
                DrawArc(pen, rect, startAngle, sweepAngle);
            }
        }
        public void DrawArc(Color color, float width, RectangleF rect, float startAngle, float sweepAngle)
        {
            using (var pen = new Pen(color, width))
            {
                DrawArc(pen, rect, startAngle, sweepAngle);
            }
        }

        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle) => g.DrawPie(pen, rect, startAngle, sweepAngle);
        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle) => g.DrawPie(pen, rect, startAngle, sweepAngle);
        public void DrawPie(Color color, float width, Rectangle rect, float startAngle, float sweepAngle)
        {
            using (var pen = new Pen(color, width))
            {
                DrawPie(pen, rect, startAngle, sweepAngle);
            }
        }
        public void DrawPie(Color color, float width, RectangleF rect, float startAngle, float sweepAngle)
        {
            using (var pen = new Pen(color, width))
            {
                DrawPie(pen, rect, startAngle, sweepAngle);
            }
        }

        public void DrawLine(Color color, float width, Point pt1, Point pt2)
        {
            using (var pen = new Pen(color, width))
            {
                DrawLine(pen, pt1, pt2);
            }
        }
        public void DrawLine(Color color, float width, PointF pt1, PointF pt2)
        {
            using (var pen = new Pen(color, width))
            {
                DrawLine(pen, pt1, pt2);
            }
        }
        public void DrawLine(Color color, float width, int x, int y, int x2, int y2)
        {
            using (var pen = new Pen(color, width))
            {
                DrawLine(pen, x, y, x2, y2);
            }
        }
        public void DrawLine(Color color, float width, float x, float y, float x2, float y2)
        {
            using (var pen = new Pen(color, width))
            {
                DrawLine(pen, x, y, x2, y2);
            }
        }

        public void DrawLine(Pen pen, Point pt1, Point pt2) => g.DrawLine(pen, pt1, pt2);
        public void DrawLine(Pen pen, PointF pt1, PointF pt2) => g.DrawLine(pen, pt1, pt2);
        public void DrawLine(Pen pen, int x, int y, int x2, int y2) => g.DrawLine(pen, x, y, x2, y2);
        public void DrawLine(Pen pen, float x, float y, float x2, float y2) => g.DrawLine(pen, x, y, x2, y2);

        public void DrawLines(Pen pen, Point[] points) => g.DrawLines(pen, points);
        public void DrawLines(Pen pen, PointF[] points) => g.DrawLines(pen, points);
        public void DrawLines(Color color, float width, Point[] points)
        {
            using (var pen = new Pen(color, width))
            {
                DrawLines(pen, points);
            }
        }
        public void DrawLines(Color color, float width, PointF[] points)
        {
            using (var pen = new Pen(color, width))
            {
                DrawLines(pen, points);
            }
        }

        public void DrawCurve(Pen pen, Point[] points) => g.DrawCurve(pen, points);
        public void DrawCurve(Pen pen, PointF[] points) => g.DrawCurve(pen, points);
        public void DrawCurve(Color color, float width, Point[] points)
        {
            using (var pen = new Pen(color, width))
            {
                DrawCurve(pen, points);
            }
        }
        public void DrawCurve(Color color, float width, PointF[] points)
        {
            using (var pen = new Pen(color, width))
            {
                DrawCurve(pen, points);
            }
        }

        #endregion

        #region Base

        public GraphicsState Save() => g.Save();
        public void Restore(GraphicsState state) => g.Restore(state);
        public void SetClip(Rectangle rect) => g.SetClip(rect);
        public void SetClip(RectangleF rect) => g.SetClip(rect);
        public void SetClip(GraphicsPath path) => g.SetClip(path);
        public void SetClip(Rectangle rect, CombineMode combineMode) => g.SetClip(rect, combineMode);
        public void SetClip(RectangleF rect, CombineMode combineMode) => g.SetClip(rect, combineMode);
        public void SetClip(GraphicsPath path, CombineMode combineMode) => g.SetClip(path, combineMode);
        public void SetClip(Region region, CombineMode combineMode) => g.SetClip(region, combineMode);
        public void ResetClip() => g.ResetClip();
        public void ResetTransform() => g.ResetTransform();
        public void TranslateTransform(float dx, float dy) => g.TranslateTransform(dx, dy);
        public void TranslateTransform(float dx, float dy, MatrixOrder order) => g.TranslateTransform(dx, dy, order);
        public void RotateTransform(float angle) => g.RotateTransform(angle);
        public void RotateTransform(float angle, MatrixOrder order) => g.RotateTransform(angle, order);
        public void ScaleTransform(float sx, float sy) => g.ScaleTransform(sx, sy);
        public void ScaleTransform(float sx, float sy, MatrixOrder order) => g.ScaleTransform(sx, sy, order);

        public float DpiX => g.DpiX;
        public float DpiY => g.DpiY;
        public Matrix Transform
        {
            get => g.Transform;
            set => g.Transform = value;
        }
        public CompositingMode CompositingMode
        {
            get => g.CompositingMode;
            set => g.CompositingMode = value;
        }
        public SmoothingMode SmoothingMode
        {
            get => g.SmoothingMode;
            set => g.SmoothingMode = value;
        }
        public Region Clip
        {
            get => g.Clip;
            set => g.Clip = value;
        }
        public RectangleF RegionBounds(Region region) => region.GetBounds(g);
        public void Dispose() => g.Dispose();

        #endregion

        #region DPI

        public float Dpi { get; private set; }

        #endregion
    }
}