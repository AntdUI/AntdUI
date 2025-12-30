// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 图片
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellImage : ICell
    {
        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="img">图片</param>
        public CellImage(Bitmap img) { image = img; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="img">图片</param>
        public CellImage(Image img) { image = img; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="svg">SVG</param>
        public CellImage(string svg) { imageSvg = svg; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="svg">SVG</param>
        /// <param name="svgcolor">填充颜色</param>
        public CellImage(string svg, Color svgcolor) { imageSvg = svg; fillSvg = svgcolor; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="_radius">圆角</param>
        public CellImage(Bitmap img, int _radius) { image = img; radius = _radius; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="_radius">圆角</param>
        public CellImage(Image img, int _radius) { image = img; radius = _radius; }

        #region 属性

        #region 边框

        Color? bordercolor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color? BorderColor
        {
            get => bordercolor;
            set
            {
                if (bordercolor == value) return;
                bordercolor = value;
                if (borderwidth > 0) OnPropertyChanged();
            }
        }

        float borderwidth = 0F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth
        {
            get => borderwidth;
            set
            {
                if (borderwidth == value) return;
                borderwidth = value;
                OnPropertyChanged();
            }
        }

        #endregion

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                OnPropertyChanged();
            }
        }

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                OnPropertyChanged();
            }
        }

        Size? size;
        /// <summary>
        /// 自定义大小
        /// </summary>
        public Size? Size
        {
            get => size;
            set
            {
                if (size == value) return;
                size = value;
                OnPropertyChanged(true);
            }
        }

        TFit imageFit = TFit.Cover;
        /// <summary>
        /// 图片布局
        /// </summary>
        public TFit ImageFit
        {
            get => imageFit;
            set
            {
                if (imageFit == value) return;
                imageFit = value;
                OnPropertyChanged();
            }
        }

        Image? image;
        /// <summary>
        /// 图片
        /// </summary>
        public Image? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                OnPropertyChanged();
            }
        }

        string? imageSvg;
        /// <summary>
        /// 图片SVG
        /// </summary>
        public string? ImageSvg
        {
            get => imageSvg;
            set
            {
                if (imageSvg == value) return;
                imageSvg = value;
                OnPropertyChanged();
            }
        }

        Color? fillSvg;
        /// <summary>
        /// SVG填充颜色
        /// </summary>
        public Color? FillSvg
        {
            get => fillSvg;
            set
            {
                if (fillSvg == value) fillSvg = value;
                fillSvg = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 文本提示
        /// </summary>
        public string? Tooltip { get; set; }

        #endregion

        #region 设置

        #region 图

        public CellImage SetImage(Image img)
        {
            image = img;
            return this;
        }

        public CellImage SetImage(string svg)
        {
            imageSvg = svg;
            return this;
        }

        public CellImage SetImage(string svg, Color? fill)
        {
            imageSvg = svg;
            fillSvg = fill;
            return this;
        }

        public CellImage SetImageFit(TFit fit = TFit.Fill)
        {
            ImageFit = fit;
            return this;
        }

        #endregion

        #region 边框

        public CellImage SetBorder(Color? color)
        {
            bordercolor = color;
            return this;
        }
        public CellImage SetBorder(float value = 1F)
        {
            borderwidth = value;
            return this;
        }
        public CellImage SetBorder(Color? color, float value = 1F)
        {
            bordercolor = color;
            borderwidth = value;
            return this;
        }

        #endregion

        #region 大小

        public CellImage SetSize(Size _size)
        {
            size = _size;
            return this;
        }
        public CellImage SetSize(int _size)
        {
            size = new Size(_size, _size);
            return this;
        }
        public CellImage SetSize(int w, int h)
        {
            size = new Size(w, h);
            return this;
        }

        #endregion

        public CellImage SetRound(bool value = true)
        {
            round = value;
            return this;
        }

        public CellImage SetRadius(int value = 0)
        {
            radius = value;
            return this;
        }
        public CellImage SetTooltip(string? tooltip)
        {
            Tooltip = tooltip;
            return this;
        }

        #endregion

        public override string? ToString() => null;
    }
}