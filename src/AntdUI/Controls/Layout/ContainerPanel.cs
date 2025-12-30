// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    [Designer(typeof(IControlDesigner))]
    public class ContainerPanel : IControl
    {
        #region 属性

        int radius = 0;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
                OnPropertyChanged(nameof(Radius));
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
        /// 背景渐变色
        /// </summary>
        [Description("背景渐变色"), Category("外观"), DefaultValue(null)]
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

        Image? backImage;
        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public new Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImage));
            }
        }

        TFit backFit = TFit.Fill;
        /// <summary>
        /// 背景图片布局
        /// </summary>
        [Description("背景图片布局"), Category("外观"), DefaultValue(TFit.Fill)]
        public new TFit BackgroundImageLayout
        {
            get => backFit;
            set
            {
                if (backFit == value) return;
                backFit = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImageLayout));
            }
        }

        #endregion

        #region 边框

        float borderWidth = 0F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(0F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                IOnSizeChanged();
                OnPropertyChanged(nameof(BorderWidth));
            }
        }

        Color? borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                if (borderWidth > 0) Invalidate();
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        DashStyle borderStyle = DashStyle.Solid;
        /// <summary>
        /// 边框样式
        /// </summary>
        [Description("边框样式"), Category("边框"), DefaultValue(DashStyle.Solid)]
        public DashStyle BorderStyle
        {
            get => borderStyle;
            set
            {
                if (borderStyle == value) return;
                borderStyle = value;
                if (borderWidth > 0) Invalidate();
                OnPropertyChanged(nameof(BorderStyle));
            }
        }

        #endregion

        #endregion

        #region 渲染

        public bool PaintBack(Canvas g)
        {
            var rect = ReadRectangle;
            if (rect.Width > 0 && rect.Height > 0)
            {
                float _radius = radius * Dpi;
                bool hasBack = back.HasValue || backExtend != null, hasBor = borderWidth > 0;
                if (hasBack || hasBor)
                {
                    using (var path = rect.RoundPath(radius))
                    {
                        if (hasBack)
                        {
                            using (var brush = backExtend.BrushEx(rect, back ?? Colour.BgContainer.Get(nameof(Panel), ColorScheme)))
                            {
                                g.Fill(brush, path);
                            }
                        }
                        if (backImage != null) g.Image(rect, backImage, backFit, _radius, false);
                        if (hasBor) g.Draw(borderColor ?? Colour.BorderColor.Get(nameof(Panel), ColorScheme), borderWidth * Dpi, borderStyle, path);
                    }
                }
                else if (backImage != null) g.Image(rect, backImage, backFit, _radius, false);

                if (DesignMode)
                {
                    if (hasBor) return true;
                    using (var path = rect.RoundPath(radius))
                    {
                        g.Draw(borderColor ?? Colour.Text.Get(nameof(Panel), ColorScheme), Dpi, DashStyle.Dash, path);
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}