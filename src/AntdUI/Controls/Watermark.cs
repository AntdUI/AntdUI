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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Watermark 水印
    /// </summary>
    /// <remarks>给页面的某个区域加上水印。</remarks>
    public class Watermark
    {
        /// <summary>
        /// 开启水印
        /// </summary>
        /// <param name="config">水印配置</param>
        /// <returns>水印窗体</returns>
        public static Form? open(Config config)
        {
            if (config?.Target == null || !config.Enabled) return null;
            try
            {
                // 创建新的水印窗体
                var watermarkForm = new LayeredFormWatermark(config);
                // 显示水印
                watermarkForm.Show(config.Target);
                return watermarkForm;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 开启水印（简化版本）
        /// </summary>
        /// <param name="target">目标控件</param>
        /// <param name="content">水印内容</param>
        /// <param name="subContent">副内容</param>
        /// <returns>水印窗体</returns>
        public static Form? open(Control target, string content, string? subContent = null) => open(new Config(target, content, subContent));

        #region 配置

        /// <summary>
        /// 水印配置类
        /// </summary>
        public class Config
        {
            public Config(Control target, string content, string? subContent = null)
            {
                Target = target;
                Content = content;
                SubContent = subContent;
                Offset = new int[] { Gap[0] / 2, Gap[1] / 2 };
            }

            /// <summary>
            /// 目标控件
            /// </summary>
            public Control Target { get; set; }

            float opacity = 0.15F;
            /// <summary>
            /// 透明度
            /// </summary>
            public float Opacity
            {
                get => opacity;
                set
                {
                    if (opacity == value) return;
                    opacity = value;
                    if (lay == null) return;
                    lay.alpha = (byte)Math.Round(255 * Style.rgbfloat(value));
                    lay.Print();
                }
            }

            /// <summary>
            /// 水印文字内容
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// 副内容
            /// </summary>
            public string? SubContent { get; set; }

            /// <summary>
            /// 副内容字体大小比例
            /// </summary>
            public float SubFontSize { get; set; } = 0.8F;

            /// <summary>
            /// 水印图片
            /// </summary>
            public Image? Image { get; set; }

            /// <summary>
            /// 水印图片SVG
            /// </summary>
            public string? ImageSvg { get; set; }

            /// <summary>
            /// 水印宽度
            /// </summary>
            public int Width { get; set; } = 200;

            /// <summary>
            /// 水印高度
            /// </summary>
            public int Height { get; set; } = 100;

            /// <summary>
            /// 旋转角度
            /// </summary>
            public int Rotate { get; set; } = -22;

            /// <summary>
            /// 水印间距 [x, y]
            /// </summary>
            public int[] Gap { get; set; } = new int[] { 100, 100 };

            /// <summary>
            /// 水印偏移量 [x, y]
            /// </summary>
            public int[] Offset { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 字体颜色
            /// </summary>
            public Color? ForeColor { get; set; }

            /// <summary>
            /// 文本对齐方式
            /// </summary>
            public StringAlignment TextAlign { get; set; } = StringAlignment.Center;

            /// <summary>
            /// 是否启用
            /// </summary>
            public bool Enabled { get; set; } = true;

            /// <summary>
            /// 是否为屏幕水印
            /// </summary>
            public bool IsScreen { get; set; } = false;

            internal LayeredFormWatermark? lay = null;

            public void Print() => lay?.Print();

            #region 设置

            public Config SetOpacity(float value)
            {
                Opacity = value;
                return this;
            }
            public Config SetSub(string? value)
            {
                SubContent = value;
                return this;
            }
            public Config SetSub(float value)
            {
                SubFontSize = value;
                return this;
            }
            public Config SetImage(Image? value)
            {
                Image = value;
                return this;
            }
            public Config SetImage(string? value)
            {
                ImageSvg = value;
                return this;
            }
            public Config SetSize(int w, int h)
            {
                Width = w;
                Height = h;
                return this;
            }
            public Config SetWidth(int value)
            {
                Width = value;
                return this;
            }
            public Config SetHeight(int value)
            {
                Height = value;
                return this;
            }
            public Config SetRotate(int value)
            {
                Rotate = value;
                return this;
            }
            public Config SetGap(int value)
            {
                Gap = new int[] { value, value };
                return this;
            }
            public Config SetGap(int x, int y)
            {
                Gap = new int[] { x, y };
                return this;
            }
            public Config SetOffset(int value)
            {
                Offset = new int[] { value, value };
                return this;
            }
            public Config SetOffset(int x, int y)
            {
                Offset = new int[] { x, y };
                return this;
            }
            public Config SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public Config SetFont(Font? value, Color? color)
            {
                Font = value;
                ForeColor = color;
                return this;
            }
            public Config SetFore(Color? value)
            {
                ForeColor = value;
                return this;
            }
            public Config SetTextAlign(StringAlignment value = StringAlignment.Near)
            {
                TextAlign = value;
                return this;
            }
            public Config SetEnabled(bool value = false)
            {
                Enabled = value;
                return this;
            }
            public Config SetIsScreen(bool value = true)
            {
                IsScreen = value;
                return this;
            }

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// 水印窗体实现
    /// </summary>
    internal class LayeredFormWatermark : ILayeredFormOpacity
    {
        Watermark.Config config;

        public LayeredFormWatermark(Watermark.Config _config) : base((byte)Math.Round(255 * Style.rgbfloat(_config.Opacity)))
        {
            config = _config;
            config.lay = this;
            Font = _config.Font ?? _config.Target.Font;
            if (config.IsScreen)
            {
                // 屏幕水印
                var screen = Screen.PrimaryScreen!.Bounds;
                SetSize(screen.Width, screen.Height);
                SetLocation(screen.X, screen.Y);
                TopMost = true;
            }
            else
            {
                config.Target.SetTopMost(Handle);
                if (config.Target is Form form)
                {
                    SetSize(form.Size);
                    SetLocation(form.Location);
                    HasBor = form.FormFrame(out Radius, out Bor);
                }
                else
                {
                    SetSize(config.Target.Size);
                    SetLocation(config.Target.PointToScreen(Point.Empty));
                    if (config.Target is IControl icontrol) RenderRegion = () => icontrol.RenderRegion;
                }
            }
        }

        Func<GraphicsPath>? RenderRegion;
        int Radius = 0, Bor = 0;
        bool HasBor = false;

        #region 渲染

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            if (config.Enabled)
            {
                using (var g = Graphics.FromImage(original_bmp).HighLay())
                using (var brush = new SolidBrush(config.ForeColor ?? Colour.FillSecondary.Get(nameof(Watermark))))
                {
                    // 计算水印间距
                    int gapX = (int)(config.Gap[0] * Config.Dpi), gapY = (int)(config.Gap[1] * Config.Dpi);
                    int width = (int)(config.Width * Config.Dpi), height = (int)(config.Height * Config.Dpi), patternWidth = width + gapX, patternHeight = height + gapY;

                    // 计算起始偏移
                    int startX = (int)(config.Offset[0] * Config.Dpi) % patternWidth, startY = (int)(config.Offset[1] * Config.Dpi) % patternHeight;

                    #region 移除非客户区域


                    if (RenderRegion == null)
                    {
                        if (HasBor)
                        {
                            Rectangle rect_read = new Rectangle(Bor, 0, rect.Width - Bor * 2, rect.Height - Bor);
                            if (Radius > 0)
                            {
                                using (var path = rect_read.RoundPath(Radius))
                                {
                                    g.SetClip(path);
                                }
                            }
                            else g.SetClip(rect_read);
                        }
                        else if (Radius > 0)
                        {
                            using (var path = rect.RoundPath(Radius))
                            {
                                g.SetClip(path);
                            }
                        }
                    }
                    else
                    {
                        using (var path = RenderRegion())
                        {
                            g.SetClip(path);
                        }
                    }

                    #endregion

                    // 绘制水印图案
                    for (int y = -startY; y < rect.Height; y += patternHeight)
                    {
                        for (int x = -startX; x < rect.Width; x += patternWidth)
                        {
                            DrawWatermarkItem(g, x, y, width, height, Font, brush);
                        }
                    }
                }
            }
            return original_bmp;
        }

        void DrawWatermarkItem(Canvas g, int x, int y, int w, int h, Font font, Brush brush)
        {
            // 保存当前状态
            var state = g.Save();

            // 设置旋转
            if (config.Rotate != 0)
            {
                var centerX = x + w / 2f;
                var centerY = y + h / 2f;
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(config.Rotate);
                g.TranslateTransform(-centerX, -centerY);
            }

            // 绘制图片水印
            if (config.Image != null) DrawImageWatermark(g, x, y, w, h);

            // 绘制文字水印
            if (!string.IsNullOrEmpty(config.Content)) DrawTextWatermark(g, x, y, w, h, font, brush);

            // 恢复状态
            g.Restore(state);
        }

        void DrawImageWatermark(Canvas g, int x, int y, int w, int h)
        {
            if (config.Image == null) return;

            int imageWidth = Math.Min(w, config.Image.Width), imageHeight = Math.Min(h, config.Image.Height);
            float drawX = x + (w - imageWidth) / 2f, drawY = y + (h - imageHeight) / 2f;

            g.Image(config.Image, new Rectangle((int)drawX, (int)drawY, imageWidth, imageHeight));
        }

        void DrawTextWatermark(Canvas g, int x, int y, int w, int h, Font font, Brush brush)
        {
            if (string.IsNullOrEmpty(config.Content)) return;

            using (var format = Helper.SF(lr: config.TextAlign))
            {
                // 计算文字区域
                if (string.IsNullOrEmpty(config.SubContent))
                {
                    // 绘制主内容
                    var mainRect = new Rectangle(x, y + (h - h) / 2, w, h);
                    g.DrawText(config.Content, font, brush, mainRect, format);
                }
                else
                {
                    int contentHeight = h, subContentHeight = 0, spacing = (int)(4 * Config.Dpi); // 主内容和副内容之间的间距
                    // 如果有副内容，计算实际需要的空间
                    using (var subFont = new Font(Font.FontFamily, Font.Size * config.SubFontSize, Font.Style))
                    {
                        Size mainSize = g.MeasureText(config.Content, font, w, format), subSize = g.MeasureText(config.SubContent, subFont, w, format);

                        // 计算总高度
                        int totalTextHeight = mainSize.Height + spacing + subSize.Height;

                        // 如果总高度超过水印高度，则按比例缩放
                        if (totalTextHeight > h)
                        {
                            var scale = h / totalTextHeight;
                            contentHeight = mainSize.Height * scale;
                            subContentHeight = subSize.Height * scale;
                            spacing *= scale;
                        }
                        else
                        {
                            contentHeight = mainSize.Height;
                            subContentHeight = subSize.Height;
                        }
                        // 计算垂直居中位置
                        int totalHeight = contentHeight + spacing + subContentHeight, startY = y + (h - totalHeight) / 2;

                        // 绘制主内容
                        var mainRect = new Rectangle(x, startY, w, contentHeight);
                        g.DrawText(config.Content, font, brush, mainRect, format);

                        // 绘制副内容
                        var subRect = new Rectangle(x, startY + contentHeight + spacing, w, subContentHeight);
                        g.DrawText(config.SubContent, subFont, brush, subRect, format);
                    }
                }
            }
        }

        #endregion

        #region 坐标

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (config.IsScreen) Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            else
            {
                config.Target.VisibleChanged += Target_VisibleChanged;
                var parent = config.Target.FindPARENT();
                if (parent == null) return;
                tmp = parent;
                parent.LocationChanged += Parent_LSChanged;
                parent.SizeChanged += Parent_LSChanged;
            }
        }

        private void Parent_LSChanged(object? sender, EventArgs e)
        {
            var rect = TargetRect;
            bool isPoint = true, isSize = true;
            if (config.Target is Form form)
            {
                var point = form.Location;
                var size = form.Size;
                SetLocation(point);
                SetSize(size);
                isPoint = rect.X == point.X && rect.Y == point.Y;
                isSize = rect.Width == size.Width && rect.Height == size.Height;
            }
            else
            {
                var point = config.Target.PointToScreen(Point.Empty);
                var size = config.Target.Size;
                SetLocation(point);
                SetSize(size);
                isPoint = rect.X == point.X && rect.Y == point.Y;
                isSize = rect.Width == size.Width && rect.Height == size.Height;
            }
            if (isPoint && isSize) return;
            else if (isSize) PrintCache();
            else Print();
        }

        private void Target_VisibleChanged(object? sender, EventArgs e)
        {
            if (config.Target.Visible) Show();
            else Hide();
        }

        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
        {
            var screen = Screen.PrimaryScreen!.Bounds;
            SetSize(screen.Width, screen.Height);
            SetLocation(screen.X, screen.Y);
            Print();
        }

        #endregion

        Form? tmp;
        protected override void Dispose(bool disposing)
        {
            // 移除事件监听
            if (config.IsScreen) Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
            else
            {
                config.Target.VisibleChanged -= Target_VisibleChanged;
                // 移除父容器的事件监听
                if (tmp == null) return;
                tmp.LocationChanged -= Parent_LSChanged;
                tmp.SizeChanged -= Parent_LSChanged;
            }
            base.Dispose(disposing);
        }

        public override string name => nameof(Watermark);
    }
}