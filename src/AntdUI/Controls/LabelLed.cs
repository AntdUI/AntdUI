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
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    /// <summary>
    /// LED 文本控件。
    /// </summary>
    /// <remarks>显示一段LED样式的文本。</remarks>
    [Description("LED 文本")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class LabelLed : IControl, ShadowConfig
    {
        #region 属性

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public override string? Text
        {
#pragma warning disable CS8764, CS8766
            get => this.GetLangI(LocalizationText, text);
#pragma warning restore CS8764, CS8766
            set
            {
                if (text == value) return;
                text = value;
                cache.Clear();
                Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        int? fontSize;
        /// <summary>
        /// 字体大小
        /// </summary>
        [Description("字体大小"), Category("外观"), DefaultValue(null)]
        public int? FontSize
        {
            get => fontSize;
            set
            {
                if (fontSize == value) return;
                fontSize = value;
                cache.Clear();
                Invalidate();
                OnPropertyChanged(nameof(FontSize));
            }
        }

        /// <summary>
        /// Emoji字体
        /// </summary>
        [Description("Emoji字体"), Category("外观"), DefaultValue("Segoe UI Emoji")]
        public string EmojiFont { get; set; } = "Segoe UI Emoji";

        int dotSize = 4;
        /// <summary>
        /// 点阵大小
        /// </summary>
        [Description("点阵大小"), Category("外观"), DefaultValue(4)]
        public int DotSize
        {
            get => dotSize;
            set
            {
                if (dotSize == value) return;
                dotSize = Math.Max(1, value);
                Invalidate();
                OnPropertyChanged(nameof(DotSize));
            }
        }

        int dotGap = 2;
        /// <summary>
        /// 点阵距离
        /// </summary>
        [Description("点阵距离"), Category("外观"), DefaultValue(2)]
        public int DotGap
        {
            get => dotGap;
            set
            {
                if (dotGap == value) return;
                dotGap = Math.Max(0, value);
                Invalidate();
                OnPropertyChanged(nameof(DotGap));
            }
        }

        float textScale = 1F;
        /// <summary>
        /// 文本大小比例
        /// </summary>
        [Description("文本大小比例"), Category("外观"), DefaultValue(1F)]
        public float TextScale
        {
            get => textScale;
            set
            {
                if (Math.Abs(textScale - value) < float.Epsilon) return;
                if (value <= 0) value = 0.1F;
                textScale = value;
                Invalidate();
                OnPropertyChanged(nameof(TextScale));
            }
        }

        LedDotShape dotShape = LedDotShape.Square;
        /// <summary>
        /// 点阵形状
        /// </summary>
        [Description("点阵形状"), Category("外观"), DefaultValue(LedDotShape.Square)]
        public LedDotShape DotShape
        {
            get => dotShape;
            set
            {
                if (dotShape == value) return;
                dotShape = value;
                Invalidate();
                OnPropertyChanged(nameof(DotShape));
            }
        }

        Color? dotColor;
        /// <summary>
        /// 点阵颜色
        /// </summary>
        [Description("点阵颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? DotColor
        {
            get => dotColor;
            set
            {
                if (dotColor == value) return;
                dotColor = value;
                Invalidate();
                OnPropertyChanged(nameof(DotColor));
            }
        }

        bool showOffLed = false;
        /// <summary>
        /// 是否显示未发光LED
        /// </summary>
        [Description("是否显示未发光LED"), Category("外观"), DefaultValue(false)]
        public bool ShowOffLed
        {
            get => showOffLed;
            set
            {
                if (showOffLed == value) return;
                showOffLed = value;
                Invalidate();
                OnPropertyChanged(nameof(ShowOffLed));
            }
        }

        Color? offDotColor;
        /// <summary>
        /// 未发光LED颜色
        /// </summary>
        [Description("未发光LED颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? OffDotColor
        {
            get => offDotColor;
            set
            {
                if (offDotColor == value) return;
                offDotColor = value;
                Invalidate();
                OnPropertyChanged(nameof(OffDotColor));
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(Back));
            }
        }

        string? backExtend;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public string? BackExtend
        {
            get => backExtend;
            set
            {
                if (backExtend == value) return;
                backExtend = value;
                Invalidate();
                OnPropertyChanged(nameof(BackExtend));
            }
        }

        #endregion

        #region 阴影

        int shadow = 0;
        /// <summary>
        /// 阴影大小
        /// </summary>
        [Description("阴影大小"), Category("阴影"), DefaultValue(0)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                Invalidate();
                OnPropertyChanged(nameof(Shadow));
            }
        }

        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ShadowColor { get; set; }

        float shadowOpacity = 0.3F;
        /// <summary>
        /// 阴影透明度
        /// </summary>
        [Description("阴影透明度"), Category("阴影"), DefaultValue(0.3F)]
        public float ShadowOpacity
        {
            get => shadowOpacity;
            set
            {
                if (Math.Abs(shadowOpacity - value) < float.Epsilon) return;
                shadowOpacity = Math.Max(0, Math.Min(1, value));
                Invalidate();
                OnPropertyChanged(nameof(ShadowOpacity));
            }
        }

        int shadowOffsetX = 0;
        /// <summary>
        /// 阴影偏移X
        /// </summary>
        [Description("阴影偏移X"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetX
        {
            get => shadowOffsetX;
            set
            {
                if (shadowOffsetX == value) return;
                shadowOffsetX = value;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOffsetX));
            }
        }

        int shadowOffsetY = 0;
        /// <summary>
        /// 阴影偏移Y
        /// </summary>
        [Description("阴影偏移Y"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetY
        {
            get => shadowOffsetY;
            set
            {
                if (shadowOffsetY == value) return;
                shadowOffsetY = value;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOffsetY));
            }
        }

        #endregion

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);
            var g = e.Canvas;
            var rect = e.Rect.PaddingRect(Padding);
            if (rect.Width <= 0 || rect.Height <= 0) return;
            if (back.HasValue || backExtend != null)
            {
                using (var brushback = backExtend.BrushEx(rect, back ?? Colour.BgContainer.Get(nameof(LabelLed), ColorScheme)))
                {
                    g.Fill(brushback, rect);
                }
            }
            var txt = Text;
            if (txt == null || string.IsNullOrEmpty(txt)) return;

            var fore = DotColor ?? Colour.Primary.Get(nameof(LabelLed), ColorScheme);
            if (fontSize.HasValue)
            {
                var patterns = GetPatterns(txt, fontSize.Value);
                if (shadow > 0)
                {
                    using (var bmp = new Bitmap(Width, Height))
                    {
                        using (var g2 = Graphics.FromImage(bmp).HighLay())
                        {
                            RenderLed(g2, ClientRectangle.PaddingRect(Padding), patterns, fontSize.Value, fontSize.Value, ShadowColor ?? fore, false);
                        }
                        Helper.Blur(bmp, shadow);
                        g.Image(bmp, new Rectangle(shadowOffsetX, shadowOffsetY, bmp.Width, bmp.Height), shadowOpacity);
                    }
                }
                RenderLed(g, rect, patterns, fontSize.Value, fontSize.Value, fore, true);
            }
            else
            {
                int rows, cols;
                GetFontSize(out rows, out cols);
                var patterns = GetPatterns(txt);
                if (shadow > 0)
                {
                    using (var bmp = new Bitmap(Width, Height))
                    {
                        using (var g2 = Graphics.FromImage(bmp).HighLay())
                        {
                            RenderLed(g2, ClientRectangle.PaddingRect(Padding), patterns, rows, cols, ShadowColor ?? fore, false);
                        }
                        Helper.Blur(bmp, shadow);
                        g.Image(bmp, new Rectangle(shadowOffsetX, shadowOffsetY, bmp.Width, bmp.Height), shadowOpacity);
                    }
                }
                RenderLed(g, rect, patterns, rows, cols, fore, true);
            }
        }

        void RenderLed(Canvas g, Rectangle rect, int[][] patterns, int rows, int cols, Color color, bool drawbg)
        {
            int dot = (int)Math.Max(1, Math.Round(dotSize * Dpi * textScale)), gap = (int)Math.Round(dotGap * Dpi * textScale);
            int pitch = dot + gap, charGap = gap * 2;

            int charWidth = cols * pitch - gap, charHeight = rows * pitch - gap;

            int totalWidth = patterns.Length == 0 ? 0 : patterns.Length * charWidth + Math.Max(0, patterns.Length - 1) * charGap;

            int startX = rect.X, startY = rect.Y;
            if (rect.Width > totalWidth) startX += (rect.Width - totalWidth) / 2;
            if (rect.Height > charHeight) startY += (rect.Height - charHeight) / 2;

            using (var brushOn = new SolidBrush(color))
            {
                if (drawbg && showOffLed)
                {
                    using (var brushOff = new SolidBrush(offDotColor ?? Helper.ToColor(40, color)))
                    {
                        for (int i = 0; i < patterns.Length; i++)
                        {
                            var pattern = patterns[i];
                            int offsetX = startX + i * (charWidth + charGap);
                            for (int r = 0; r < rows; r++)
                            {
                                var row = pattern[r];
                                for (int c = 0; c < cols; c++)
                                {
                                    bool isOn = (row & (1 << (cols - 1 - c))) != 0;
                                    int x = offsetX + c * pitch, y = startY + r * pitch;
                                    DrawDot(g, isOn ? brushOn : brushOff, x, y, dot);
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < patterns.Length; i++)
                    {
                        var pattern = patterns[i];
                        for (int r = 0; r < rows; r++)
                        {
                            var row = pattern[r];
                            for (int c = 0; c < cols; c++)
                            {
                                if ((row & (1 << (cols - 1 - c))) != 0)
                                {
                                    int offsetX = startX + i * (charWidth + charGap);
                                    int x = offsetX + c * pitch, y = startY + r * pitch;
                                    DrawDot(g, brushOn, x, y, dot);
                                }
                            }
                        }
                    }
                }
            }
        }
        void RenderLed(Canvas g, Rectangle rect, byte[][] patterns, int rows, int cols, Color color, bool drawbg)
        {
            int dot = (int)Math.Max(1, Math.Round(dotSize * Dpi * textScale)), gap = (int)Math.Round(dotGap * Dpi * textScale);
            int pitch = dot + gap, charGap = gap * 2;

            int charWidth = cols * pitch - gap, charHeight = rows * pitch - gap;

            int totalWidth = patterns.Length == 0 ? 0 : patterns.Length * charWidth + Math.Max(0, patterns.Length - 1) * charGap;

            int startX = rect.X, startY = rect.Y;
            if (rect.Width > totalWidth) startX += (rect.Width - totalWidth) / 2;
            if (rect.Height > charHeight) startY += (rect.Height - charHeight) / 2;

            using (var brushOn = new SolidBrush(color))
            {
                if (drawbg && showOffLed)
                {
                    using (var brushOff = new SolidBrush(offDotColor ?? Helper.ToColor(40, color)))
                    {
                        for (int i = 0; i < patterns.Length; i++)
                        {
                            var pattern = patterns[i];
                            int offsetX = startX + i * (charWidth + charGap);
                            for (int r = 0; r < rows; r++)
                            {
                                for (int c = 0; c < cols; c++)
                                {
                                    int byteIndex = (r * cols + c) / 8, bitIndex = 7 - (r * cols + c) % 8;
                                    bool isOn = (pattern[byteIndex] & (1 << bitIndex)) == 0;
                                    int x = offsetX + c * pitch, y = startY + r * pitch;
                                    DrawDot(g, isOn ? brushOn : brushOff, x, y, dot);
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < patterns.Length; i++)
                    {
                        var pattern = patterns[i];
                        for (int r = 0; r < rows; r++)
                        {
                            for (int c = 0; c < cols; c++)
                            {
                                int byteIndex = (r * cols + c) / 8, bitIndex = 7 - (r * cols + c) % 8;
                                bool isOn = (pattern[byteIndex] & (1 << bitIndex)) == 0;
                                if (isOn)
                                {
                                    int offsetX = startX + i * (charWidth + charGap);
                                    int x = offsetX + c * pitch, y = startY + r * pitch;
                                    DrawDot(g, brushOn, x, y, dot);
                                }
                            }
                        }
                    }
                }
            }
        }

        void DrawDot(Canvas g, Brush brush, int x, int y, int size)
        {
            switch (dotShape)
            {
                case LedDotShape.Circle:
                    g.FillEllipse(brush, new Rectangle(x, y, size, size));
                    break;
                case LedDotShape.Diamond:
                    using (var path = new GraphicsPath())
                    {
                        float h = size / 2F;
                        path.AddPolygon(new PointF[]
                        {
                            new PointF(x + h, y),
                            new PointF(x + size, y + h),
                            new PointF(x + h, y + size),
                            new PointF(x, y + h)
                        });
                        g.Fill(brush, path);
                    }
                    break;
                case LedDotShape.Square:
                default:
                    g.Fill(brush, new Rectangle(x, y, size, size));
                    break;
            }
        }

        #endregion

        #region 数据

        #region DIV

        protected override void OnFontChanged(EventArgs e)
        {
            cache.Clear();
            base.OnFontChanged(e);
        }

        ConcurrentDictionary<string, byte[]> cache = new ConcurrentDictionary<string, byte[]>();
        byte[][] GetPatterns(string text, int size)
        {
            var list = new List<byte[]>(text.Length);
            using (var font = new Font(Font.FontFamily, size, Font.Style, GraphicsUnit.Pixel))
            {
                var rect = new Rectangle(0, 0, size, size);
                Font? fontEmoji = null;
                GraphemeSplitter.Each(text, 0, (str, nStart, nLen, nType) =>
                {
                    string txt = str.Substring(nStart, nLen);
                    if (cache.TryGetValue(txt, out var find)) list.Add(find);
                    else
                    {
                        if (nType == 18 || nType == 4)
                        {
                            if (fontEmoji == null) fontEmoji = new Font(EmojiFont, Font.Size);
                            using (var bmp = new Bitmap(size, size))
                            {
                                using (var g = Graphics.FromImage(bmp).HighLay())
                                {
                                    g.String(txt, fontEmoji, Brushes.White, rect);
                                }
                                var tmp = Helper.ConvertImageToDotMatrix(bmp, true);
                                cache.TryAdd(txt, tmp);
                                list.Add(tmp);
                            }
                        }
                        else
                        {
                            using (var bmp = new Bitmap(size, size))
                            {
                                using (var g = Graphics.FromImage(bmp).HighLay())
                                {
                                    g.String(txt, font, Brushes.White, rect);
                                }
                                var tmp = Helper.ConvertImageToDotMatrix(bmp, true);
                                cache.TryAdd(txt, tmp);
                                list.Add(tmp);
                            }
                        }
                    }
                    return true;
                });
                fontEmoji?.Dispose();
            }
            return list.ToArray();
        }

        #endregion

        void GetFontSize(out int rows, out int cols)
        {
            rows = 7;
            cols = 5;
        }

        int[][] GetPatterns(string txt)
        {
            var list = new List<int[]>(txt.Length);
            foreach (var ch in txt) list.Add(GetPattern(ch));
            return list.ToArray();
        }

        int[] GetPattern(char ch)
        {
            GetFontSize(out int rows, out int cols);
            if (Font5x7.TryGetValue(ch, out var pattern)) return pattern;
            return new int[7];
        }

        static readonly Dictionary<char, int[]> Font5x7 = new Dictionary<char, int[]>
        {
            ['0'] = new[] { 0b01110, 0b10001, 0b10011, 0b10101, 0b11001, 0b10001, 0b01110 },
            ['1'] = new[] { 0b00100, 0b01100, 0b00100, 0b00100, 0b00100, 0b00100, 0b01110 },
            ['2'] = new[] { 0b01110, 0b10001, 0b00001, 0b00010, 0b00100, 0b01000, 0b11111 },
            ['3'] = new[] { 0b11110, 0b00001, 0b00001, 0b01110, 0b00001, 0b00001, 0b11110 },
            ['4'] = new[] { 0b00010, 0b00110, 0b01010, 0b10010, 0b11111, 0b00010, 0b00010 },
            ['5'] = new[] { 0b11111, 0b10000, 0b11110, 0b00001, 0b00001, 0b10001, 0b01110 },
            ['6'] = new[] { 0b00110, 0b01000, 0b10000, 0b11110, 0b10001, 0b10001, 0b01110 },
            ['7'] = new[] { 0b11111, 0b00001, 0b00010, 0b00100, 0b01000, 0b01000, 0b01000 },
            ['8'] = new[] { 0b01110, 0b10001, 0b10001, 0b01110, 0b10001, 0b10001, 0b01110 },
            ['9'] = new[] { 0b01110, 0b10001, 0b10001, 0b01111, 0b00001, 0b00010, 0b01100 },

            ['A'] = new[] { 0b01110, 0b10001, 0b10001, 0b11111, 0b10001, 0b10001, 0b10001 },
            ['B'] = new[] { 0b11110, 0b10001, 0b10001, 0b11110, 0b10001, 0b10001, 0b11110 },
            ['C'] = new[] { 0b01110, 0b10001, 0b10000, 0b10000, 0b10000, 0b10001, 0b01110 },
            ['D'] = new[] { 0b11110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b11110 },
            ['E'] = new[] { 0b11111, 0b10000, 0b10000, 0b11110, 0b10000, 0b10000, 0b11111 },
            ['F'] = new[] { 0b11111, 0b10000, 0b10000, 0b11110, 0b10000, 0b10000, 0b10000 },
            ['G'] = new[] { 0b01110, 0b10001, 0b10000, 0b10111, 0b10001, 0b10001, 0b01110 },
            ['H'] = new[] { 0b10001, 0b10001, 0b10001, 0b11111, 0b10001, 0b10001, 0b10001 },
            ['I'] = new[] { 0b01110, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b01110 },
            ['J'] = new[] { 0b00001, 0b00001, 0b00001, 0b00001, 0b10001, 0b10001, 0b01110 },
            ['K'] = new[] { 0b10001, 0b10010, 0b10100, 0b11000, 0b10100, 0b10010, 0b10001 },
            ['L'] = new[] { 0b10000, 0b10000, 0b10000, 0b10000, 0b10000, 0b10000, 0b11111 },
            ['M'] = new[] { 0b10001, 0b11011, 0b10101, 0b10101, 0b10001, 0b10001, 0b10001 },
            ['N'] = new[] { 0b10001, 0b10001, 0b11001, 0b10101, 0b10011, 0b10001, 0b10001 },
            ['O'] = new[] { 0b01110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110 },
            ['P'] = new[] { 0b11110, 0b10001, 0b10001, 0b11110, 0b10000, 0b10000, 0b10000 },
            ['Q'] = new[] { 0b01110, 0b10001, 0b10001, 0b10001, 0b10101, 0b10010, 0b01101 },
            ['R'] = new[] { 0b11110, 0b10001, 0b10001, 0b11110, 0b10100, 0b10010, 0b10001 },
            ['S'] = new[] { 0b01111, 0b10000, 0b10000, 0b01110, 0b00001, 0b00001, 0b11110 },
            ['T'] = new[] { 0b11111, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100 },
            ['U'] = new[] { 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110 },
            ['V'] = new[] { 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01010, 0b00100 },
            ['W'] = new[] { 0b10001, 0b10001, 0b10001, 0b10101, 0b10101, 0b11011, 0b10001 },
            ['X'] = new[] { 0b10001, 0b10001, 0b01010, 0b00100, 0b01010, 0b10001, 0b10001 },
            ['Y'] = new[] { 0b10001, 0b10001, 0b01010, 0b00100, 0b00100, 0b00100, 0b00100 },
            ['Z'] = new[] { 0b11111, 0b00001, 0b00010, 0b00100, 0b01000, 0b10000, 0b11111 },

            ['a'] = new[] { 0, 0, 0b01110, 0b00001, 0b01111, 0b10001, 0b01111 },
            ['b'] = new[] { 0b10000, 0b10000, 0b11110, 0b10001, 0b10001, 0b10001, 0b11110 },
            ['c'] = new[] { 0, 0, 0b01111, 0b10000, 0b10000, 0b10000, 0b01111 },
            ['d'] = new[] { 0b00001, 0b00001, 0b01111, 0b10001, 0b10001, 0b10001, 0b01111 },
            ['e'] = new[] { 0, 0, 0b01110, 0b10001, 0b11111, 0b10000, 0b01110 },
            ['f'] = new[] { 0b00100, 0b01000, 0b11110, 0b01000, 0b01000, 0b01000, 0b01000 },
            ['g'] = new[] { 0, 0, 0b01111, 0b10001, 0b10001, 0b01111, 0b00001, 0b01110 },
            ['h'] = new[] { 0b10000, 0b10000, 0b11110, 0b10001, 0b10001, 0b10001, 0b10001 },
            ['i'] = new[] { 0b00100, 0, 0b01100, 0b00100, 0b00100, 0b00100, 0b01110 },
            ['j'] = new[] { 0b00001, 0, 0b00001, 0b00001, 0b00001, 0b10001, 0b01110 },
            ['k'] = new[] { 0b10000, 0b10000, 0b10010, 0b10100, 0b11000, 0b10100, 0b10010 },
            ['l'] = new[] { 0b01100, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b01110 },
            ['m'] = new[] { 0, 0, 0b11010, 0b10101, 0b10101, 0b10001, 0b10001 },
            ['n'] = new[] { 0, 0, 0b11110, 0b10001, 0b10001, 0b10001, 0b10001 },
            ['o'] = new[] { 0, 0, 0b01110, 0b10001, 0b10001, 0b10001, 0b01110 },
            ['p'] = new[] { 0, 0, 0b11110, 0b10001, 0b10001, 0b11110, 0b10000, 0b10000 },
            ['q'] = new[] { 0, 0, 0b01111, 0b10001, 0b10001, 0b01111, 0b00001, 0b00001 },
            ['r'] = new[] { 0, 0, 0b11110, 0b10000, 0b10000, 0b10000, 0b10000 },
            ['s'] = new[] { 0, 0, 0b01111, 0b10000, 0b01110, 0b00001, 0b11110 },
            ['t'] = new[] { 0b00100, 0b00100, 0b11110, 0b00100, 0b00100, 0b00100, 0b00010 },
            ['u'] = new[] { 0, 0, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110 },
            ['v'] = new[] { 0, 0, 0b10001, 0b10001, 0b10001, 0b01010, 0b00100 },
            ['w'] = new[] { 0, 0, 0b10001, 0b10001, 0b10101, 0b10101, 0b01010 },
            ['x'] = new[] { 0, 0, 0b10001, 0b01010, 0b00100, 0b01010, 0b10001 },
            ['y'] = new[] { 0, 0, 0b10001, 0b10001, 0b10001, 0b01111, 0b00001, 0b01110 },
            ['z'] = new[] { 0, 0, 0b11111, 0b00010, 0b00100, 0b01000, 0b11111 },

            ['-'] = new[] { 0, 0, 0, 0b11111, 0, 0, 0 },
            ['.'] = new[] { 0, 0, 0, 0, 0, 0b01100, 0b01100 },
            [':'] = new[] { 0, 0b01100, 0b01100, 0, 0b01100, 0b01100, 0 },
            ['!'] = new[] { 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0, 0b00100 },
            ['?'] = new[] { 0b01110, 0b10001, 0b00001, 0b00010, 0b00100, 0, 0b00100 },
            ['/'] = new[] { 0b00001, 0b00010, 0b00100, 0b01000, 0b10000, 0, 0 },
            ['\\'] = new[] { 0b10000, 0b01000, 0b00100, 0b00010, 0b00001, 0, 0 },
            ['@'] = new[] { 0b01110, 0b10001, 0b10101, 0b10101, 0b10111, 0b10000, 0b01111 },
            ['#'] = new[] { 0b01010, 0b01010, 0b11111, 0b01010, 0b11111, 0b01010, 0b01010 },
            ['$'] = new[] { 0b00100, 0b01110, 0b10100, 0b01110, 0b00101, 0b01110, 0b00100 },
            ['%'] = new[] { 0b11001, 0b11010, 0b00010, 0b00100, 0b01000, 0b01011, 0b10011 },
            ['^'] = new[] { 0b00100, 0b01010, 0b10001, 0, 0, 0, 0 },
            ['&'] = new[] { 0b01100, 0b10010, 0b01100, 0b10001, 0b01101, 0b10010, 0b01101 },
            ['*'] = new[] { 0, 0b00100, 0b10101, 0b01110, 0b10101, 0b00100, 0 },
            ['('] = new[] { 0b00010, 0b00100, 0b01000, 0b01000, 0b01000, 0b00100, 0b00010 },
            [')'] = new[] { 0b01000, 0b00100, 0b00010, 0b00010, 0b00010, 0b00100, 0b01000 },
            ['+'] = new[] { 0, 0b00100, 0b00100, 0b11111, 0b00100, 0b00100, 0 },
            ['='] = new[] { 0, 0, 0b11111, 0, 0b11111, 0, 0 },
            ['['] = new[] { 0b01110, 0b01000, 0b01000, 0b01000, 0b01000, 0b01000, 0b01110 },
            [']'] = new[] { 0b01110, 0b00010, 0b00010, 0b00010, 0b00010, 0b00010, 0b01110 },
            ['{'] = new[] { 0b00010, 0b00100, 0b00100, 0b01000, 0b00100, 0b00100, 0b00010 },
            ['}'] = new[] { 0b01000, 0b00100, 0b00100, 0b00010, 0b00100, 0b00100, 0b01000 },
            ['|'] = new[] { 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100 },
            ['<'] = new[] { 0b00010, 0b00100, 0b01000, 0b10000, 0b01000, 0b00100, 0b00010 },
            ['>'] = new[] { 0b01000, 0b00100, 0b00010, 0b00001, 0b00010, 0b00100, 0b01000 },
            [','] = new[] { 0, 0, 0, 0, 0, 0b01100, 0b00100 },
            [';'] = new[] { 0, 0b01100, 0b01100, 0, 0b01100, 0b00100, 0 },
            ['"'] = new[] { 0b01010, 0b01010, 0b01010, 0, 0, 0, 0 },
            ['\''] = new[] { 0b00100, 0b00100, 0b00100, 0, 0, 0, 0 },
            ['_'] = new[] { 0, 0, 0, 0, 0, 0, 0b11111 },
            ['×'] = new[] { 0, 0b10001, 0b01010, 0b00100, 0b01010, 0b10001, 0 },
            ['≤'] = new[] { 0, 0b00010, 0b00100, 0b01000, 0b00100, 0b00010, 0b11111 },
            ['≥'] = new[] { 0, 0b01000, 0b00100, 0b00010, 0b00100, 0b01000, 0b11111 },

            ['°'] = new[] { 0b00100, 0b01010, 0b10001, 0b01010, 0b00100, 0, 0 },
            ['Ω'] = new[] { 0, 0b01110, 0b10001, 0b10001, 0b10001, 0b01010, 0b10001 },
            ['∑'] = new[] { 0, 0b11111, 0b00100, 0b01000, 0b00100, 0b00010, 0b11111 },
            ['£'] = new[] { 0b01110, 0b10000, 0b11110, 0b10000, 0b11110, 0b10000, 0b11111 },
            ['•'] = new[] { 0, 0, 0, 0b00100, 0, 0, 0 },
            ['…'] = new[] { 0, 0, 0, 0, 0b10101, 0b10101, 0b10101 },
            ['→'] = new[] { 0, 0b00100, 0b00010, 0b11111, 0b00010, 0b00100, 0 },
            ['←'] = new[] { 0, 0b00100, 0b01000, 0b11111, 0b01000, 0b00100, 0 },
            ['↑'] = new[] { 0, 0b00100, 0b01110, 0b10101, 0b00100, 0b00100, 0 },
            ['↓'] = new[] { 0, 0b00100, 0b00100, 0b10101, 0b01110, 0b00100, 0 },
            ['⇒'] = new[] { 0b00100, 0b00010, 0b11111, 0b00010, 0b00100, 0, 0 },
            ['⇐'] = new[] { 0b00100, 0b01000, 0b11111, 0b01000, 0b00100, 0, 0 },
            ['⇑'] = new[] { 0b00100, 0b01110, 0b10101, 0b00100, 0b00100, 0, 0 },
            ['⇓'] = new[] { 0b00100, 0b00100, 0b10101, 0b01110, 0b00100, 0, 0 },
            ['○'] = new[] { 0b01110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110 },
            ['●'] = new[] { 0b01110, 0b11111, 0b11111, 0b11111, 0b11111, 0b11111, 0b01110 },
            ['◇'] = new[] { 0, 0b00100, 0b01010, 0b10001, 0b01010, 0b00100, 0 },
            ['◆'] = new[] { 0, 0b00100, 0b01110, 0b11111, 0b01110, 0b00100, 0 },
            ['♥'] = new[] { 0, 0b01010, 0b10101, 0b10001, 0b01010, 0b00100, 0 },
            ['♦'] = new[] { 0, 0, 0b00100, 0b01010, 0b00100, 0, 0 }
        };

        /// <summary>
        /// 增加LED字符
        /// </summary>
        /// <param name="key">字符</param>
        /// <param name="value">显示</param>
        public static void Add(char key, int[] value)
        {
            if (Font5x7.ContainsKey(key)) Font5x7[key] = value;
            else Font5x7.Add(key, value);
        }

        #endregion
    }

    public enum LedDotShape
    {
        Square,
        Diamond,
        Circle
    }
}