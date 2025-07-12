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

using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 按钮
    /// </summary>
    public partial class CellButton : CellLink
    {
        /// <summary>
        /// 按钮
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">文本</param>
        public CellButton(string id, string? text = null) : base(id, text) { }

        /// <summary>
        /// 按钮
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">文本</param>
        /// <param name="_type">类型</param>
        public CellButton(string id, string text, TTypeMini _type) : base(id, text) { type = _type; }

        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                OnPropertyChanged();
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        public Color? BackActive { get; set; }

        string? backExtend;
        /// <summary>
        /// 背景渐变色
        /// </summary>
        public string? BackExtend
        {
            get => backExtend;
            set
            {
                if (backExtend == value) return;
                backExtend = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 默认样式

        Color? defaultback;
        /// <summary>
        /// Default模式背景颜色
        /// </summary>
        public Color? DefaultBack
        {
            get => defaultback;
            set
            {
                if (defaultback == value) return;
                defaultback = value;
                if (type == TTypeMini.Default) OnPropertyChanged();
            }
        }

        Color? defaultbordercolor;
        /// <summary>
        /// Default模式边框颜色
        /// </summary>
        public Color? DefaultBorderColor
        {
            get => defaultbordercolor;
            set
            {
                if (defaultbordercolor == value) return;
                defaultbordercolor = value;
                if (type == TTypeMini.Default) OnPropertyChanged();
            }
        }

        #endregion

        #region 边框

        internal float borderWidth = 0;
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 图标

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                OnPropertyChanged(true);
            }
        }

        float icongap = .25F;
        /// <summary>
        /// 图标与文字间距比例
        /// </summary>
        public float IconGap
        {
            get => icongap;
            set
            {
                if (icongap == value) return;
                icongap = value;
                OnPropertyChanged(true);
            }
        }

        Image? icon;
        /// <summary>
        /// 图标
        /// </summary>
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                OnPropertyChanged(true);
            }
        }

        string? iconSvg;
        /// <summary>
        /// 图标SVG
        /// </summary>
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => iconSvg != null || icon != null;

        /// <summary>
        /// 悬停图标
        /// </summary>
        public Image? IconHover { get; set; }

        /// <summary>
        /// 悬停图标SVG
        /// </summary>
        public string? IconHoverSvg { get; set; }

        /// <summary>
        /// 悬停图标动画时长
        /// </summary>
        public int IconHoverAnimation { get; set; } = 200;

        TAlignMini iconPosition = TAlignMini.Left;
        /// <summary>
        /// 按钮图标组件的位置
        /// </summary>
        public TAlignMini IconPosition
        {
            get => iconPosition;
            set
            {
                if (iconPosition == value) return;
                iconPosition = value;
                OnPropertyChanged(true);
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

        TShape shape = TShape.Default;
        /// <summary>
        /// 形状
        /// </summary>
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                OnPropertyChanged(true);
            }
        }

        TTypeMini type = TTypeMini.Default;
        /// <summary>
        /// 类型
        /// </summary>
        public TTypeMini Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged();
            }
        }

        bool ghost = false;
        /// <summary>
        /// 幽灵属性，使按钮背景透明
        /// </summary>
        public bool Ghost
        {
            get => ghost;
            set
            {
                if (ghost == value) return;
                ghost = value;
                OnPropertyChanged();
            }
        }

        internal float ArrowProg = -1F;
        bool showArrow = false;
        /// <summary>
        /// 下拉框箭头是否显示
        /// </summary>
        public bool ShowArrow
        {
            get => showArrow;
            set
            {
                if (showArrow == value) return;
                showArrow = value;
                OnPropertyChanged(true);
            }
        }

        bool isLink = false;
        /// <summary>
        /// 下拉框箭头是否链接样式
        /// </summary>
        public bool IsLink
        {
            get => isLink;
            set
            {
                if (isLink == value) return;
                isLink = value;
                if (showArrow) OnPropertyChanged();
            }
        }

        /// <summary>
        /// 间距
        /// </summary>
        public int? Gap { get; set; }

        #endregion

        #region 设置

        public CellButton SetFore(Color? value)
        {
            fore = value;
            return this;
        }
        public CellButton SetBack(Color? value)
        {
            back = value;
            return this;
        }

        public CellButton SetBack(Color? value, Color? hover, Color? active = null)
        {
            back = value;
            BackHover = hover;
            BackActive = active;
            return this;
        }

        public CellButton SetBack(string? value)
        {
            backExtend = value;
            return this;
        }

        public CellButton SetDefaultBack(Color? value)
        {
            defaultback = value;
            return this;
        }

        public CellButton SetDefault(Color? value, Color? bor)
        {
            defaultback = value;
            defaultbordercolor = bor;
            return this;
        }

        public CellButton SetBorder(float value = 1F)
        {
            borderWidth = value;
            return this;
        }

        #region 图标

        public CellButton SetIcon(Image? img)
        {
            icon = img;
            return this;
        }

        public CellButton SetIcon(string? svg)
        {
            iconSvg = svg;
            return this;
        }

        public CellButton SetIcon(Image? img, Image? hover)
        {
            icon = img;
            IconHover = hover;
            return this;
        }

        public CellButton SetIcon(string? svg, string? hover)
        {
            iconSvg = svg;
            IconHoverSvg = hover;
            return this;
        }

        public CellButton SetIconHover(string? svg, int animation = 200)
        {
            IconHoverSvg = svg;
            IconHoverAnimation = animation;
            return this;
        }
        public CellButton SetIconHover(Image? img, int animation = 200)
        {
            IconHover = img;
            IconHoverAnimation = animation;
            return this;
        }

        public CellButton SetIconPosition(TAlignMini align)
        {
            iconPosition = align;
            return this;
        }

        #endregion

        public CellButton SetIconRatio(float value = 1F)
        {
            iconratio = value;
            return this;
        }
        public CellButton SetIconGap(float value = 0F)
        {
            icongap = value;
            return this;
        }

        public CellButton SetRadius(int value = 0)
        {
            radius = value;
            return this;
        }
        public CellButton SetShape(TShape value = TShape.Round)
        {
            shape = value;
            return this;
        }
        public CellButton SetType(TTypeMini value = TTypeMini.Primary)
        {
            type = value;
            return this;
        }
        public CellButton SetGhost(bool value = true)
        {
            ghost = value;
            return this;
        }
        public CellButton SetArrow(bool value = true)
        {
            showArrow = value;
            return this;
        }
        public CellButton SetIsLink(bool value = true)
        {
            isLink = value;
            return this;
        }

        public CellButton SetArrow(bool value, bool link)
        {
            showArrow = value;
            isLink = value;
            return this;
        }

        public CellButton SetArrowProg(float value = 1F)
        {
            ArrowProg = value;
            return this;
        }

        #endregion
    }
}