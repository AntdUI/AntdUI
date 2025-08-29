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

using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// 水印组件
    /// 参考Ant Design Watermark组件：https://ant.design/components/watermark-cn
    /// </summary>
    public class Watermark
    {
        /// <summary>
        /// 水印配置类
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 目标控件
            /// </summary>
            public Control Target { get; set; }

            /// <summary>
            /// 水印文字内容
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// 副内容
            /// </summary>
            public string SubContent { get; set; }

            /// <summary>
            /// 水印图片
            /// </summary>
            public Image? Image { get; set; }

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
            /// Z轴层级
            /// </summary>
            public int ZIndex { get; set; } = 9;

            /// <summary>
            /// 水印间距 [x, y]
            /// </summary>
            public int[] Gap { get; set; } = new int[] { 100, 100 };

            /// <summary>
            /// 水印偏移量 [x, y]
            /// </summary>
            public int[] Offset { get; set; }

            /// <summary>
            /// 字体配置
            /// </summary>
            public FontConfig Font { get; set; } = new FontConfig();

            /// <summary>
            /// 是否启用
            /// </summary>
            public bool Enabled { get; set; } = true;

            /// <summary>
            /// 是否为屏幕水印
            /// </summary>
            public bool IsScreen { get; set; } = false;

            public Config(Control target, string content, string subContent = "")
            {
                Target = target;
                Content = content;
                SubContent = subContent;
                Offset = new int[] { Gap[0] / 2, Gap[1] / 2 };
            }
        }

        /// <summary>
        /// 字体配置类
        /// </summary>
        public class FontConfig
        {
            /// <summary>
            /// 字体颜色
            /// </summary>
            public Color Color { get; set; } = Color.FromArgb(38, 0, 0, 0);

            /// <summary>
            /// 字体大小
            /// </summary>
            public float FontSize { get; set; } = 14;

            /// <summary>
            /// 字体粗细
            /// </summary>
            public FontWeight FontWeight { get; set; } = FontWeight.Normal;

            /// <summary>
            /// 字体族
            /// </summary>
            public string FontFamily { get; set; } = "Microsoft YaHei";

            /// <summary>
            /// 字体样式
            /// </summary>
            public System.Drawing.FontStyle FontStyle { get; set; } = System.Drawing.FontStyle.Regular;

            /// <summary>
            /// 文本对齐方式
            /// </summary>
            public StringAlignment TextAlign { get; set; } = StringAlignment.Center;
        }

        /// <summary>
        /// 字体粗细枚举
        /// </summary>
        public enum FontWeight
        {
            Normal = 400,
            Light = 300,
            Bold = 700
        }

        /// <summary>
        /// 开启水印
        /// </summary>
        /// <param name="config">水印配置</param>
        /// <returns>水印窗体</returns>
        public static FormWatermark? open(Config config)
        {
            if (config?.Target == null || !config.Enabled)
                return null;

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
        public static FormWatermark? open(Control target, string content, string subContent = "")
        {
            var config = new Config(target, content, subContent);
            return open(config);
        }
    }

    /// <summary>
    /// 水印窗体接口
    /// </summary>
    public interface FormWatermark
    {
        /// <summary>
        /// 关闭水印
        /// </summary>
        void Close();
    }

    /// <summary>
    /// 水印窗体实现
    /// </summary>
    internal class LayeredFormWatermark : ILayeredFormOpacity, FormWatermark
    {
        Watermark.Config config;
        internal bool topMost = false;

        public LayeredFormWatermark(Watermark.Config _config) : base(255)
        {
            config = _config;
            if (config.IsScreen)
            {
                // 屏幕水印
                var screen = Screen.PrimaryScreen.Bounds;
                SetSize(screen.Width, screen.Height);
                SetLocation(screen.X, screen.Y);
                topMost = true;
            }
            else
            {
                // 控件水印
                UpdateWatermarkPosition();
            }

            // 监听目标控件的位置和大小变化
            if (!config.IsScreen)
            {
                config.Target.LocationChanged += Target_LocationChanged;
                config.Target.SizeChanged += Target_SizeChanged;
                config.Target.VisibleChanged += Target_VisibleChanged;

                // 监听父容器的移动事件
                var parent = config.Target.Parent;
                while (parent != null)
                {
                    parent.LocationChanged += Parent_LocationChanged;
                    parent.SizeChanged += Parent_SizeChanged;
                    parent = parent.Parent;
                }

                // 监听Form的移动事件
                var form = GetTopLevelForm(config.Target);
                if (form != null)
                {
                    form.LocationChanged += Form_LocationChanged;
                    form.SizeChanged += Form_SizeChanged;
                }
            }

            // 监听系统事件
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        private void Target_LocationChanged(object? sender, EventArgs e)
        {
            if (config.Target != null && !config.IsScreen)
            {
                UpdateWatermarkPosition();
            }
        }

        private void Target_SizeChanged(object? sender, EventArgs e)
        {
            if (config.Target != null && !config.IsScreen)
            {
                UpdateWatermarkPosition();
            }
        }

        private void Parent_LocationChanged(object? sender, EventArgs e)
        {
            if (config.Target != null && !config.IsScreen)
            {
                UpdateWatermarkPosition();
            }
        }

        private void Parent_SizeChanged(object? sender, EventArgs e)
        {
            if (config.Target != null && !config.IsScreen)
            {
                UpdateWatermarkPosition();
            }
        }

        private void Form_LocationChanged(object? sender, EventArgs e)
        {
            if (config.Target != null && !config.IsScreen)
            {
                UpdateWatermarkPosition();
            }
        }

        private void Form_SizeChanged(object? sender, EventArgs e)
        {
            if (config.Target != null && !config.IsScreen)
            {
                UpdateWatermarkPosition();
            }
        }

        private void UpdateWatermarkPosition()
        {
            if (config.Target != null && !config.IsScreen)
            {
                var rect = config.Target.RectangleToScreen(config.Target.ClientRectangle);
                SetSize(rect.Width, rect.Height);
                SetLocation(rect.X, rect.Y);
                Print();
            }
        }

        private Form? GetTopLevelForm(Control control)
        {
            while (control != null)
            {
                if (control is Form form)
                {
                    return form;
                }
                control = control.Parent;
            }
            return null;
        }

        private void Target_VisibleChanged(object? sender, EventArgs e)
        {
            if (config.Target != null)
            {
                if (config.Target.Visible)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }

        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
        {
            if (config.IsScreen)
            {
                var screen = Screen.PrimaryScreen.Bounds;
                SetSize(screen.Width, screen.Height);
                SetLocation(screen.X, screen.Y);
                Print();
            }
        }

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                if (!config.Enabled) return original_bmp;

                var font = new Font(config.Font.FontFamily, config.Font.FontSize, config.Font.FontStyle);
                var textColor = config.Font.Color;
                var brush = new SolidBrush(textColor);

                // 计算水印间距
                var gapX = config.Gap[0];
                var gapY = config.Gap[1];
                var patternWidth = config.Width + gapX;
                var patternHeight = config.Height + gapY;

                // 计算起始偏移
                var startX = config.Offset[0] % patternWidth;
                var startY = config.Offset[1] % patternHeight;

                // 绘制水印图案
                for (int y = -startY; y < rect.Height; y += patternHeight)
                {
                    for (int x = -startX; x < rect.Width; x += patternWidth)
                    {
                        DrawWatermarkItem(g, x, y, font, brush);
                    }
                }

                font.Dispose();
                brush.Dispose();
            }
            return original_bmp;
        }

        private void DrawWatermarkItem(Graphics g, int x, int y, Font font, Brush brush)
        {
            // 保存当前状态
            var state = g.Save();

            // 设置旋转
            if (config.Rotate != 0)
            {
                var centerX = x + config.Width / 2f;
                var centerY = y + config.Height / 2f;
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(config.Rotate);
                g.TranslateTransform(-centerX, -centerY);
            }

            // 绘制图片水印
            if (config.Image != null)
            {
                DrawImageWatermark(g, x, y);
            }

            // 绘制文字水印
            if (!string.IsNullOrEmpty(config.Content))
            {
                DrawTextWatermark(g, x, y, font, brush);
            }

            // 恢复状态
            g.Restore(state);
        }

        private void DrawImageWatermark(Graphics g, int x, int y)
        {
            if (config.Image == null) return;

            var imageWidth = Math.Min(config.Width, config.Image.Width);
            var imageHeight = Math.Min(config.Height, config.Image.Height);
            var drawX = x + (config.Width - imageWidth) / 2f;
            var drawY = y + (config.Height - imageHeight) / 2f;

            // 创建半透明图片
            var colorMatrix = new ColorMatrix();
            colorMatrix.Matrix33 = 0.15f; // 透明度

            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            g.DrawImage(config.Image,
                new Rectangle((int)drawX, (int)drawY, imageWidth, imageHeight),
                0, 0, config.Image.Width, config.Image.Height,
                GraphicsUnit.Pixel, imageAttributes);

            imageAttributes.Dispose();
        }

        private void DrawTextWatermark(Graphics g, int x, int y, Font font, Brush brush)
        {
            if (string.IsNullOrEmpty(config.Content)) return;

            var format = new StringFormat
            {
                Alignment = config.Font.TextAlign,
                LineAlignment = StringAlignment.Center
            };

            // 计算文字区域
            var contentHeight = (float)config.Height;
            var subContentHeight = 0f;
            var spacing = 4f; // 主内容和副内容之间的间距

            if (!string.IsNullOrEmpty(config.SubContent))
            {
                // 如果有副内容，计算实际需要的空间
                var subFont = new Font(config.Font.FontFamily, config.Font.FontSize * 0.8f, config.Font.FontStyle);

                // 测量主内容文字高度
                var mainSize = g.MeasureString(config.Content, font, config.Width, format);

                // 测量副内容文字高度
                var subSize = g.MeasureString(config.SubContent, subFont, config.Width, format);

                // 计算总高度
                var totalTextHeight = mainSize.Height + spacing + subSize.Height;

                // 如果总高度超过水印高度，则按比例缩放
                if (totalTextHeight > config.Height)
                {
                    var scale = config.Height / totalTextHeight;
                    contentHeight = mainSize.Height * scale;
                    subContentHeight = subSize.Height * scale;
                    spacing = spacing * scale;
                }
                else
                {
                    contentHeight = mainSize.Height;
                    subContentHeight = subSize.Height;
                }

                subFont.Dispose();
            }

            // 计算垂直居中位置
            var totalHeight = contentHeight + (string.IsNullOrEmpty(config.SubContent) ? 0 : spacing + subContentHeight);
            var startY = y + (config.Height - totalHeight) / 2f;

            // 绘制主内容
            var mainRect = new RectangleF(x, startY, config.Width, contentHeight);
            g.DrawString(config.Content, font, brush, mainRect, format);

            // 绘制副内容
            if (!string.IsNullOrEmpty(config.SubContent))
            {
                var subFont = new Font(config.Font.FontFamily, config.Font.FontSize * 0.8f, config.Font.FontStyle);
                var subRect = new RectangleF(x, startY + contentHeight + spacing, config.Width, subContentHeight);
                g.DrawString(config.SubContent, subFont, brush, subRect, format);
                subFont.Dispose();
            }

            format.Dispose();
        }

        public new void Close()
        {
            // 移除事件监听
            if (config.Target != null && !config.IsScreen)
            {
                config.Target.LocationChanged -= Target_LocationChanged;
                config.Target.SizeChanged -= Target_SizeChanged;
                config.Target.VisibleChanged -= Target_VisibleChanged;

                // 移除父容器的事件监听
                var parent = config.Target.Parent;
                while (parent != null)
                {
                    parent.LocationChanged -= Parent_LocationChanged;
                    parent.SizeChanged -= Parent_SizeChanged;
                    parent = parent.Parent;
                }

                // 移除Form的事件监听
                var form = GetTopLevelForm(config.Target);
                if (form != null)
                {
                    form.LocationChanged -= Form_LocationChanged;
                    form.SizeChanged -= Form_SizeChanged;
                }
            }

            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;

            base.Close();
        }

        public override string name => "Watermark";
    }
}
