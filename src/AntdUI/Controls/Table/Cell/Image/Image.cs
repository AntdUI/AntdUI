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

using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 图片
    /// </summary>
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

        Size? size = null;
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

        Bitmap? image;
        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                OnPropertyChanged();
            }
        }

        string? imageSvg = null;
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

        public override string? ToString() => null;
    }
}