// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AntdUI
{
    /// <summary>
    /// Button 阴影按钮
    /// </summary>
    /// <remarks>按钮用于开始一个即时操作。</remarks>
    [Description("ButtonShadow 阴影按钮")]
    [ToolboxItem(true)]
    public class ButtonShadow : Button
    {
        #region 属性

        #region 阴影

        int shadow = 4;
        /// <summary>
        /// 阴影大小
        /// </summary>
        [Description("阴影"), Category("外观"), DefaultValue(4)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = WaveSize = value;
                shadow_temp?.Dispose();
                shadow_temp = null;
                OnPropertyChanged(nameof(Shadow));
            }
        }

        Color? shadowColor;
        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ShadowColor
        {
            get => shadowColor;
            set
            {
                if (shadowColor == value) return;
                shadowColor = value;
                shadow_temp?.Dispose();
                shadow_temp = null;
                Invalidate();
                OnPropertyChanged(nameof(ShadowColor));
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
                shadow_temp?.Dispose();
                shadow_temp = null;
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
                shadow_temp?.Dispose();
                shadow_temp = null;
                OnPropertyChanged(nameof(ShadowOffsetY));
            }
        }

        float shadowOpacity = 0.2F;
        /// <summary>
        /// 阴影透明度
        /// </summary>
        [Description("阴影透明度"), Category("阴影"), DefaultValue(0.2F)]
        public float ShadowOpacity
        {
            get => shadowOpacity;
            set
            {
                if (shadowOpacity == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                shadowOpacity = value;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOpacity));
            }
        }

        #endregion

        #endregion

        public override void ClickAnimation()
        { }
        protected override void PaintShadow(Canvas g, Rectangle rect, Rectangle rect_read, GraphicsPath path, Color color, float radius) => DrawShadow(g, rect, path);

        Bitmap? shadow_temp;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        void DrawShadow(Canvas g, Rectangle rect_client, GraphicsPath path)
        {
            if (shadow > 0)
            {
                int shadow = (int)(Shadow * Dpi), shadowOffsetX = (int)(ShadowOffsetX * Dpi), shadowOffsetY = (int)(ShadowOffsetY * Dpi);
                if (shadow_temp == null || shadow_temp.PixelFormat == PixelFormat.DontCare || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
                {
                    shadow_temp?.Dispose();
                    shadow_temp = path.PaintShadowO(rect_client.Width, rect_client.Height, shadowColor ?? Colour.TextBase.Get(nameof(Panel), ColorScheme), shadow);
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix();
                    matrix.Matrix33 = shadowOpacity;
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.Image(shadow_temp, new Rectangle(rect_client.X + shadowOffsetX, rect_client.Y + shadowOffsetY, rect_client.Width, rect_client.Height), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            shadow_temp?.Dispose();
            base.Dispose(disposing);
        }
    }
}