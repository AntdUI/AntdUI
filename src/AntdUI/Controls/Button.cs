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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Button 按钮
    /// </summary>
    /// <remarks>按钮用于开始一个即时操作。</remarks>
    [Description("Button 按钮")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Button : IControl, IButtonControl, IEventListener
    {
        public Button() : base(ControlType.Button)
        {
            base.BackColor = Color.Transparent;
        }

        #region 属性

        /// <summary>
        /// 原装背景颜色
        /// </summary>
        [Description("原装背景颜色"), Category("外观"), DefaultValue(typeof(Color), "Transparent")]
        public Color OriginalBackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(BackColor));
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

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackActive { get; set; }

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

        #region 默认样式

        Color? defaultback;
        /// <summary>
        /// Default模式背景颜色
        /// </summary>
        [Description("Default模式背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? DefaultBack
        {
            get => defaultback;
            set
            {
                if (defaultback == value) return;
                defaultback = value;
                if (type == TTypeMini.Default) Invalidate();
                OnPropertyChanged(nameof(DefaultBack));
            }
        }

        Color? defaultbordercolor;
        /// <summary>
        /// Default模式边框颜色
        /// </summary>
        [Description("Default模式边框颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? DefaultBorderColor
        {
            get => defaultbordercolor;
            set
            {
                if (defaultbordercolor == value) return;
                defaultbordercolor = value;
                if (type == TTypeMini.Default) Invalidate();
                OnPropertyChanged(nameof(DefaultBorderColor));
            }
        }

        #endregion

        #region 边框

        float borderWidth = 0;
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
                Invalidate();
                OnPropertyChanged(nameof(BorderWidth));
            }
        }

        #endregion

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category("外观"), DefaultValue(4)]
        public int WaveSize { get; set; } = 4;

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        TShape shape = TShape.Default;
        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category("外观"), DefaultValue(TShape.Default)]
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Shape));
            }
        }

        TButtonDisplayStyle displayStyle = TButtonDisplayStyle.Default;
        /// <summary>
        /// 指定显示图像还是文本
        /// </summary>
        [Description("指定显示图像还是文本"), Category("外观"), DefaultValue(TButtonDisplayStyle.Default)]
        public TButtonDisplayStyle DisplayStyle
        {
            get => displayStyle;
            set
            {
                if (displayStyle == value) return;
                displayStyle = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(DisplayStyle));
            }
        }

        TTypeMini type = TTypeMini.Default;
        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型"), Category("外观"), DefaultValue(TTypeMini.Default)]
        public TTypeMini Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                Invalidate();
                OnPropertyChanged(nameof(Type));
            }
        }

        bool ghost = false;
        /// <summary>
        /// 幽灵属性，使按钮背景透明
        /// </summary>
        [Description("幽灵属性，使按钮背景透明"), Category("外观"), DefaultValue(false)]
        public bool Ghost
        {
            get => ghost;
            set
            {
                if (ghost == value) return;
                ghost = value;
                Invalidate();
                OnPropertyChanged(nameof(Ghost));
            }
        }

        /// <summary>
        /// 响应真实区域
        /// </summary>
        [Description("响应真实区域"), Category("行为"), DefaultValue(false)]
        public bool RespondRealAreas { get; set; }

        bool showArrow = false;
        /// <summary>
        /// 显示箭头
        /// </summary>
        [Description("显示箭头"), Category("行为"), DefaultValue(false)]
        public bool ShowArrow
        {
            get => showArrow;
            set
            {
                if (showArrow == value) return;
                showArrow = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(ShowArrow));
            }
        }

        bool isLink = false;
        /// <summary>
        /// 箭头链接样式
        /// </summary>
        [Description("箭头链接样式"), Category("行为"), DefaultValue(false)]
        public bool IsLink
        {
            get => isLink;
            set
            {
                if (isLink == value) return;
                isLink = value;
                Invalidate();
                OnPropertyChanged(nameof(IsLink));
            }
        }

        /// <summary>
        /// 箭头角度
        /// </summary>
        [Browsable(false), Description("箭头角度"), Category("外观"), DefaultValue(-1F)]
        public float ArrowProg { get; set; } = -1F;

        #region 文本

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => this.GetLangI(LocalizationText, text);
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (text == value) return;
                text = value;
                if (BeforeAutoSize()) Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        StringFormat stringFormat = Helper.SF_NoWrap();
        ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("外观"), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
                Invalidate();
                OnPropertyChanged(nameof(TextAlign));
            }
        }

        int? virtualWidth;
        [Description("虚拟宽度"), Category("外观"), DefaultValue(null)]
        public int? VirtualWidth
        {
            get => virtualWidth;
            set
            {
                if (virtualWidth == value) return;
                virtualWidth = value;
                Invalidate();
            }
        }

        bool textCenterHasIcon = false;
        /// <summary>
        /// 文本居中显示(包含图标后)
        /// </summary>
        [Description("文本居中显示(包含图标后)"), Category("外观"), DefaultValue(false)]
        public bool TextCenterHasIcon
        {
            get => textCenterHasIcon;
            set
            {
                if (textCenterHasIcon == value) return;
                textCenterHasIcon = value;
                if (HasIcon) Invalidate();
                OnPropertyChanged(nameof(TextCenterHasIcon));
            }
        }

        bool autoEllipsis = false;
        /// <summary>
        /// 文本超出自动处理
        /// </summary>
        [Description("文本超出自动处理"), Category("行为"), DefaultValue(false)]
        public bool AutoEllipsis
        {
            get => autoEllipsis;
            set
            {
                if (autoEllipsis == value) return;
                autoEllipsis = value;
                stringFormat.Trimming = value ? StringTrimming.EllipsisCharacter : StringTrimming.None;
                OnPropertyChanged(nameof(AutoEllipsis));
            }
        }

        bool textMultiLine = false;
        /// <summary>
        /// 是否多行
        /// </summary>
        [Description("是否多行"), Category("行为"), DefaultValue(false)]
        public bool TextMultiLine
        {
            get => textMultiLine;
            set
            {
                if (textMultiLine == value) return;
                textMultiLine = value;
                stringFormat.FormatFlags = value ? 0 : StringFormatFlags.NoWrap;
                Invalidate();
                OnPropertyChanged(nameof(TextMultiLine));
            }
        }

        #endregion

        #region 图标

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        float icongap = .25F;
        /// <summary>
        /// 图标与文字间距比例
        /// </summary>
        [Description("图标与文字间距比例"), Category("外观"), DefaultValue(.25F)]
        public float IconGap
        {
            get => icongap;
            set
            {
                if (icongap == value) return;
                icongap = value;
                Invalidate();
                OnPropertyChanged(nameof(IconGap));
            }
        }

        Image? icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Icon));
            }
        }

        string? iconSvg;
        /// <summary>
        /// 图标SVG
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(IconSvg));
            }
        }

        public Button SetIcon(Image? value, string? svg = null)
        {
            icon = value;
            iconSvg = svg;
            Invalidate();
            return this;
        }
        public Button SetIcon(string? value, Image? img = null)
        {
            icon = img;
            iconSvg = value;
            Invalidate();
            return this;
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => iconSvg != null || icon != null;

        /// <summary>
        /// 图标大小
        /// </summary>
        [Description("图标大小"), Category("外观"), DefaultValue(typeof(Size), "0, 0")]
        public Size IconSize { get; set; }

        /// <summary>
        /// 悬停图标
        /// </summary>
        [Description("悬停图标"), Category("外观"), DefaultValue(null)]
        public Image? IconHover { get; set; }

        /// <summary>
        /// 悬停图标SVG
        /// </summary>
        [Description("悬停图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconHoverSvg { get; set; }

        /// <summary>
        /// 悬停图标动画时长
        /// </summary>
        [Description("悬停图标动画时长"), Category("外观"), DefaultValue(200)]
        public int IconHoverAnimation { get; set; } = 200;

        TAlignMini iconPosition = TAlignMini.Left;
        /// <summary>
        /// 按钮图标组件的位置
        /// </summary>
        [Description("按钮图标组件的位置"), Category("外观"), DefaultValue(TAlignMini.Left)]
        public TAlignMini IconPosition
        {
            get => iconPosition;
            set
            {
                if (iconPosition == value) return;
                iconPosition = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(IconPosition));
            }
        }

        #endregion

        #region 切换

        #region 动画

        bool AnimationIconToggle = false;
        float AnimationIconToggleValue = 0F;

        #endregion

        bool toggle = false;
        /// <summary>
        /// 选中状态
        /// </summary>
        [Description("选中状态"), Category("切换"), DefaultValue(false)]
        public bool Toggle
        {
            get => toggle;
            set
            {
                if (value == toggle) return;
                toggle = value;
                if (Config.HasAnimation(nameof(Button)))
                {
                    if (IconToggleAnimation > 0 && HasIcon && HasToggleIcon)
                    {
                        ThreadIconHover?.Dispose();
                        ThreadIconHover = null;
                        AnimationIconHover = false;

                        ThreadIconToggle?.Dispose();
                        AnimationIconToggle = true;
                        var t = Animation.TotalFrames(10, IconToggleAnimation);
                        if (value)
                        {
                            ThreadIconToggle = new ITask((i) =>
                            {
                                AnimationIconToggleValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                Invalidate();
                                return true;
                            }, 10, t, () =>
                            {
                                AnimationIconToggleValue = 1F;
                                AnimationIconToggle = false;
                                Invalidate();
                            });
                        }
                        else
                        {
                            ThreadIconToggle = new ITask((i) =>
                            {
                                AnimationIconToggleValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                Invalidate();
                                return true;
                            }, 10, t, () =>
                            {
                                AnimationIconToggleValue = 0F;
                                AnimationIconToggle = false;
                                Invalidate();
                            });
                        }
                    }
                    else Invalidate();
                }
                else Invalidate();
                OnPropertyChanged(nameof(Toggle));
            }
        }

        Image? iconToggle;
        /// <summary>
        /// 切换图标
        /// </summary>
        [Description("切换图标"), Category("切换"), DefaultValue(null)]
        public Image? ToggleIcon
        {
            get => iconToggle;
            set
            {
                if (iconToggle == value) return;
                iconToggle = value;
                if (toggle) Invalidate();
                OnPropertyChanged(nameof(ToggleIcon));
            }
        }

        string? iconSvgToggle;
        /// <summary>
        /// 切换图标SVG
        /// </summary>
        [Description("切换图标SVG"), Category("切换"), DefaultValue(null)]
        public string? ToggleIconSvg
        {
            get => iconSvgToggle;
            set
            {
                if (iconSvgToggle == value) return;
                iconSvgToggle = value;
                if (toggle) Invalidate();
                OnPropertyChanged(nameof(ToggleIconSvg));
            }
        }

        /// <summary>
        /// 是否包含切换图标
        /// </summary>
        public bool HasToggleIcon => iconSvgToggle != null || iconToggle != null;

        /// <summary>
        /// 切换悬停图标
        /// </summary>
        [Description("切换悬停图标"), Category("切换"), DefaultValue(null)]
        public Image? ToggleIconHover { get; set; }

        /// <summary>
        /// 切换悬停图标SVG
        /// </summary>
        [Description("切换悬停图标SVG"), Category("切换"), DefaultValue(null)]
        public string? ToggleIconHoverSvg { get; set; }

        /// <summary>
        /// 图标切换动画时长
        /// </summary>
        [Description("图标切换动画时长"), Category("切换"), DefaultValue(200)]
        public int IconToggleAnimation { get; set; } = 200;

        Color? foreToggle;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("切换文字颜色"), Category("切换"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ToggleFore
        {
            get => foreToggle;
            set
            {
                if (foreToggle == value) foreToggle = value;
                foreToggle = value;
                if (toggle) Invalidate();
                OnPropertyChanged(nameof(ToggleFore));
            }
        }

        TTypeMini? typeToggle;
        /// <summary>
        /// 切换类型
        /// </summary>
        [Description("切换类型"), Category("切换"), DefaultValue(null)]
        public TTypeMini? ToggleType
        {
            get => typeToggle;
            set
            {
                if (typeToggle == value) return;
                typeToggle = value;
                if (toggle) Invalidate();
                OnPropertyChanged(nameof(ToggleType));
            }
        }

        #region 背景

        Color? backToggle;
        /// <summary>
        /// 切换背景颜色
        /// </summary>
        [Description("切换背景颜色"), Category("切换"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ToggleBack
        {
            get => backToggle;
            set
            {
                if (backToggle == value) return;
                backToggle = value;
                if (toggle) Invalidate();
                OnPropertyChanged(nameof(ToggleBack));
            }
        }

        string? backExtendToggle;
        /// <summary>
        /// 切换背景渐变色
        /// </summary>
        [Description("切换背景渐变色"), Category("切换"), DefaultValue(null)]
        public string? ToggleBackExtend
        {
            get => backExtendToggle;
            set
            {
                if (backExtendToggle == value) return;
                backExtendToggle = value;
                if (toggle) Invalidate();
                OnPropertyChanged(nameof(ToggleBackExtend));
            }
        }

        /// <summary>
        /// 切换悬停背景颜色
        /// </summary>
        [Description("切换悬停背景颜色"), Category("切换"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ToggleBackHover { get; set; }

        /// <summary>
        /// 切换激活背景颜色
        /// </summary>
        [Description("切换激活背景颜色"), Category("切换"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ToggleBackActive { get; set; }

        #endregion

        #endregion

        #region 加载动画

        bool loading = false;
        int AnimationLoadingValue = 0;
        int AnimationLoadingWaveValue = 0;
        /// <summary>
        /// 加载状态
        /// </summary>
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                SetCursor(_mouseHover && Enabled && !value);
                BeforeAutoSize();
                ThreadLoading?.Dispose();
                if (loading)
                {
                    AnimationClickValue = 0;
                    ThreadLoading = new ITask(this, i =>
                    {
                        AnimationLoadingWaveValue += 1;
                        if (AnimationLoadingWaveValue > 100) AnimationLoadingWaveValue = 0;
                        AnimationLoadingValue = i;
                        Invalidate();
                        return loading;
                    }, 10, 360, 6, () =>
                    {
                        Invalidate();
                    });
                }
                else Invalidate();
                OnPropertyChanged(nameof(Loading));
            }
        }

        /// <summary>
        /// 加载响应点击
        /// </summary>
        [Description("加载响应点击"), Category("行为"), DefaultValue(false)]
        public bool LoadingRespondClick { get; set; }

        /// <summary>
        /// 加载进度
        /// </summary>
        [Description("加载进度"), Category("加载"), DefaultValue(0.3F)]
        public float LoadingValue { get; set; } = 0.3F;

        #region 水波进度

        /// <summary>
        /// 水波进度
        /// </summary>
        [Description("水波进度"), Category("加载"), DefaultValue(0F)]
        public float LoadingWaveValue { get; set; }

        /// <summary>
        /// 水波颜色
        /// </summary>
        [Description("水波颜色"), Category("加载"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LoadingWaveColor { get; set; }

        /// <summary>
        /// 水波是否垂直
        /// </summary>
        [Description("水波是否垂直"), Category("加载"), DefaultValue(false)]
        public bool LoadingWaveVertical { get; set; }

        /// <summary>
        /// 水波大小
        /// </summary>
        [Description("水波大小"), Category("加载"), DefaultValue(2)]
        public int LoadingWaveSize { get; set; } = 2;

        /// <summary>
        /// 水波数量
        /// </summary>
        [Description("水波数量"), Category("加载"), DefaultValue(1)]
        public int LoadingWaveCount { get; set; } = 1;

        #endregion

        #endregion

        #region 组合

        TJoinMode joinMode = TJoinMode.None;
        /// <summary>
        /// 组合模式
        /// </summary>
        [Description("组合模式"), Category("外观"), DefaultValue(TJoinMode.None)]
        public TJoinMode JoinMode
        {
            get => joinMode;
            set
            {
                if (joinMode == value) return;
                joinMode = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(JoinMode));
            }
        }

        bool joinLeft = false;
        /// <summary>
        /// 连接左边
        /// </summary>
        [Obsolete("use JoinMode"), Browsable(false), Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft
        {
            get => joinLeft;
            set
            {
                if (joinLeft == value) return;
                joinLeft = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(JoinLeft));
            }
        }

        bool joinRight = false;
        /// <summary>
        /// 连接右边
        /// </summary>
        [Obsolete("use JoinMode"), Browsable(false), Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight
        {
            get => joinRight;
            set
            {
                if (joinRight == value) return;
                joinRight = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(JoinRight));
            }
        }

        #endregion

        ITask? ThreadHover;
        ITask? ThreadIconHover;
        ITask? ThreadIconToggle;
        ITask? ThreadClick;
        ITask? ThreadLoading;

        #region 点击动画

        bool AnimationClick = false;
        float AnimationClickValue = 0;

        public void ClickAnimation()
        {
            if (WaveSize > 0 && Config.HasAnimation(nameof(Button)))
            {
                ThreadClick?.Dispose();
                AnimationClickValue = 0;
                AnimationClick = true;
                ThreadClick = new ITask(this, () =>
                {
                    if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                    else AnimationClickValue += AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                    if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                    Invalidate();
                    return true;
                }, 50, () =>
                {
                    AnimationClick = false;
                    Invalidate();
                });
            }
        }

        #endregion

        #region 悬停动画

        bool _mouseDown = false;
        bool ExtraMouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
                Invalidate();
            }
        }

        int AnimationHoverValue = 0;
        bool AnimationHover = false;
        bool AnimationIconHover = false;
        float AnimationIconHoverValue = 0F;
        bool _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                bool enabled = Enabled;
                SetCursor(value && enabled && !loading);
                if (enabled)
                {
                    var backHover = GetColorO();
                    int alpha = backHover.A;
                    if (Config.HasAnimation(nameof(Button)))
                    {
                        if (IconHoverAnimation > 0 && ((toggle && HasToggleIcon && (ToggleIconHoverSvg != null || ToggleIconHover != null)) || (HasIcon && (IconHoverSvg != null || IconHover != null))))
                        {
                            ThreadIconHover?.Dispose();
                            AnimationIconHover = true;
                            var t = Animation.TotalFrames(10, IconHoverAnimation);
                            if (value)
                            {
                                ThreadIconHover = new ITask((i) =>
                                {
                                    AnimationIconHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationIconHoverValue = 1F;
                                    AnimationIconHover = false;
                                    Invalidate();
                                });
                            }
                            else
                            {
                                ThreadIconHover = new ITask((i) =>
                                {
                                    AnimationIconHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationIconHoverValue = 0F;
                                    AnimationIconHover = false;
                                    Invalidate();
                                });
                            }
                        }
                        if (alpha > 0)
                        {
                            int addvalue = alpha / 12;
                            ThreadHover?.Dispose();
                            AnimationHover = true;
                            if (value)
                            {
                                ThreadHover = new ITask(this, () =>
                                {
                                    AnimationHoverValue += addvalue;
                                    if (AnimationHoverValue > alpha) { AnimationHoverValue = alpha; return false; }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationHover = false;
                                    Invalidate();
                                });
                            }
                            else
                            {
                                ThreadHover = new ITask(this, () =>
                                {
                                    if (AnimationHoverValue > alpha) AnimationHoverValue = alpha;
                                    else AnimationHoverValue -= addvalue;
                                    if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationHover = false;
                                    Invalidate();
                                });
                            }
                        }
                        else
                        {
                            AnimationHoverValue = alpha;
                            Invalidate();
                        }
                    }
                    else AnimationHoverValue = alpha;
                    Invalidate();
                }
            }
        }

        #endregion

        #region 闪烁动画

        Color? colorBlink;
        ITask? ThreadAnimateBlink;
        /// <summary>
        /// 闪烁动画状态
        /// </summary>
        public bool AnimationBlinkState = false;

        /// <summary>
        /// 开始闪烁动画
        /// </summary>
        /// <param name="interval">动画间隔时长（毫秒）</param>
        /// <param name="colors">色彩值</param>
        public void AnimationBlink(int interval, params Color[] colors)
        {
            ThreadAnimateBlink?.Dispose();
            if (colors.Length > 1)
            {
                AnimationBlinkState = true;
                int index = 0, len = colors.Length;
                ThreadAnimateBlink = new ITask(this, () =>
                {
                    colorBlink = colors[index];
                    index++;
                    if (index > len - 1) index = 0;
                    Invalidate();
                    return AnimationBlinkState;
                }, interval, Invalidate);
            }
        }

        /// <summary>
        /// 开始闪烁动画（颜色过度动画）
        /// </summary>
        /// <param name="interval">动画间隔时长（毫秒）</param>
        /// <param name="colors">色彩值</param>
        public void AnimationBlinkTransition(int interval, params Color[] colors) => AnimationBlinkTransition(interval, 10, AnimationType.Liner, colors);

        /// <summary>
        /// 开始闪烁动画（颜色过度动画）
        /// </summary>
        /// <param name="interval">动画间隔时长（毫秒）</param>
        /// <param name="transition_interval">过度动画间隔时长（毫秒）</param>
        /// <param name="animationType">过度动画类型</param>
        /// <param name="colors">色彩值</param>
        public void AnimationBlinkTransition(int interval, int transition_interval, AnimationType animationType, params Color[] colors)
        {
            ThreadAnimateBlink?.Dispose();
            if (colors.Length > 1 && interval > transition_interval)
            {
                AnimationBlinkState = true;
                int index = 0, len = colors.Length;
                Color tmp = colors[index];
                index++;
                if (index > len - 1) index = 0;
                var t = Animation.TotalFrames(transition_interval, interval);
                ThreadAnimateBlink = new ITask(this, () =>
                {
                    Color start = tmp, end = colors[index];
                    index++;
                    if (index > len - 1) index = 0;
                    tmp = end;
                    new ITask(i =>
                    {
                        var prog = Animation.Animate(i, t, 1F, animationType);
                        colorBlink = start.BlendColors(Helper.ToColorN(prog, end));
                        Invalidate();
                        return AnimationBlinkState;
                    }, transition_interval, t, () =>
                    {
                        colorBlink = end;
                        Invalidate();
                    }).Wait();
                    return AnimationBlinkState;
                });
            }
        }

        public void StopAnimationBlink()
        {
            AnimationBlinkState = false;
            ThreadAnimateBlink?.Dispose();
        }

        #endregion

        #endregion

        #region 渲染

        bool init = false;
        protected override void OnDraw(DrawEventArgs e)
        {
            init = true;
            var g = e.Canvas;
            Rectangle rect = e.Rect.PaddingRect(Padding), rect_read = ReadRectangle;
            if (rect_read.Width == 0 || rect_read.Height == 0) return;
            float _radius = (shape == TShape.Round || shape == TShape.Circle) ? rect_read.Height : radius * Config.Dpi;
            if (backImage != null) g.Image(rect_read, backImage, backFit, _radius, shape);
            bool is_default = type == TTypeMini.Default, enabled = Enabled;
            if (toggle && typeToggle.HasValue) is_default = typeToggle.Value == TTypeMini.Default;
            if (is_default)
            {
                GetDefaultColorConfig(out var _fore, out var _color, out var _back_hover, out var _back_active);
                using (var path = Path(rect_read, _radius))
                {
                    #region 动画

                    if (AnimationClick)
                    {
                        float maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue);
                        if (shape == TShape.Circle)
                        {
                            if (maxw > maxh) maxw = maxh;
                            else maxh = maxw;
                        }
                        float alpha = 100 * (1F - AnimationClickValue);
                        using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, shape))
                        {
                            path_click.AddPath(path, false);
                            g.Fill(Helper.ToColor(alpha, _color), path_click);
                        }
                    }

                    #endregion

                    if (enabled)
                    {
                        if (!ghost)
                        {
                            if (WaveSize > 0) PaintShadow(g, rect_read, path, Colour.FillQuaternary.Get(nameof(Button), ColorScheme), _radius);
                            g.Fill(defaultback ?? Colour.DefaultBg.Get(nameof(Button), ColorScheme), path);
                        }
                        if (borderWidth > 0)
                        {
                            PaintLoadingWave(g, path, rect_read);
                            float border = borderWidth * Config.Dpi;
                            if (ExtraMouseDown)
                            {
                                g.Draw(_back_active, border, path);
                                PaintTextLoading(g, Text, _back_active, rect_read, enabled, _radius);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Helper.ToColor(AnimationHoverValue, _back_hover);
                                g.Draw(Colour.DefaultBorder.Get(nameof(Button), ColorScheme).BlendColors(colorHover), border, path);
                                PaintTextLoading(g, Text, _fore.BlendColors(colorHover), rect_read, enabled, _radius);
                            }
                            else if (ExtraMouseHover)
                            {
                                g.Draw(_back_hover, border, path);
                                PaintTextLoading(g, Text, _back_hover, rect_read, enabled, _radius);
                            }
                            else
                            {
                                if (AnimationBlinkState && colorBlink.HasValue) g.Draw(colorBlink.Value, border, path);
                                else g.Draw(defaultbordercolor ?? Colour.DefaultBorder.Get(nameof(Button), ColorScheme), border, path);
                                PaintTextLoading(g, Text, _fore, rect_read, enabled, _radius);
                            }
                        }
                        else
                        {
                            if (ExtraMouseDown) g.Fill(_back_active, path);
                            else if (AnimationHover) g.Fill(Helper.ToColor(AnimationHoverValue, _back_hover), path);
                            else if (ExtraMouseHover) g.Fill(_back_hover, path);
                            PaintLoadingWave(g, path, rect_read);
                            PaintTextLoading(g, Text, _fore, rect_read, enabled, _radius);
                        }
                    }
                    else
                    {
                        PaintLoadingWave(g, path, rect_read);
                        if (!ghost) g.Fill(Colour.FillTertiary.Get(nameof(Button), "bgDisabled", ColorScheme), path);
                        PaintTextLoading(g, Text, Colour.TextQuaternary.Get(nameof(Button), "foreDisabled", ColorScheme), rect_read, enabled, _radius);
                    }
                }
            }
            else
            {
                GetColorConfig(out var _fore, out var _back, out var _back_hover, out var _back_active);
                using (var path = Path(rect_read, _radius))
                {
                    #region 动画

                    if (AnimationClick)
                    {
                        float maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue);
                        if (shape == TShape.Circle)
                        {
                            if (maxw > maxh) maxw = maxh;
                            else maxh = maxw;
                        }
                        float alpha = 100 * (1F - AnimationClickValue);
                        using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, shape))
                        {
                            path_click.AddPath(path, false);
                            g.Fill(Helper.ToColor(alpha, _back), path_click);
                        }
                    }

                    #endregion

                    if (ghost)
                    {
                        PaintLoadingWave(g, path, rect_read);

                        #region 绘制背景

                        if (borderWidth > 0)
                        {
                            float border = borderWidth * Config.Dpi;
                            if (ExtraMouseDown)
                            {
                                g.Draw(_back_active, border, path);
                                PaintTextLoading(g, Text, _back_active, rect_read, enabled, _radius);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Helper.ToColor(AnimationHoverValue, _back_hover);
                                g.Draw((enabled ? _back : Colour.FillTertiary.Get(nameof(Button), ColorScheme)).BlendColors(colorHover), border, path);
                                PaintTextLoading(g, Text, _back.BlendColors(colorHover), rect_read, enabled, _radius);
                            }
                            else if (ExtraMouseHover)
                            {
                                g.Draw(_back_hover, border, path);
                                PaintTextLoading(g, Text, _back_hover, rect_read, enabled, _radius);
                            }
                            else
                            {
                                if (enabled)
                                {
                                    if (toggle)
                                    {
                                        using (var brushback = backExtendToggle.BrushEx(rect_read, _back))
                                        {
                                            g.Draw(brushback, border, path);
                                        }
                                    }
                                    else
                                    {
                                        using (var brushback = backExtend.BrushEx(rect_read, _back))
                                        {
                                            g.Draw(brushback, border, path);
                                        }
                                    }
                                }
                                else g.Draw(Colour.FillTertiary.Get(nameof(Button), ColorScheme), border, path);
                                PaintTextLoading(g, Text, enabled ? _back : Colour.TextQuaternary.Get(nameof(Button), "foreDisabled", ColorScheme), rect_read, enabled, _radius);
                            }
                        }
                        else PaintTextLoading(g, Text, enabled ? _back : Colour.TextQuaternary.Get(nameof(Button), "foreDisabled", ColorScheme), rect_read, enabled, _radius);

                        #endregion
                    }
                    else
                    {
                        if (enabled && WaveSize > 0) PaintShadow(g, rect_read, path, _back.rgba(Config.Mode == TMode.Dark ? 0.15F : 0.1F), _radius);

                        #region 绘制背景

                        if (enabled)
                        {
                            if (toggle)
                            {
                                using (var brush = backExtendToggle.BrushEx(rect_read, _back))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else
                            {
                                using (var brush = backExtend.BrushEx(rect_read, _back))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                        }
                        else g.Fill(Colour.FillTertiary.Get(nameof(Button), "bgDisabled", ColorScheme), path);

                        if (ExtraMouseDown) g.Fill(_back_active, path);
                        else if (AnimationHover) g.Fill(Helper.ToColor(AnimationHoverValue, _back_hover), path);
                        else if (ExtraMouseHover) g.Fill(_back_hover, path);

                        #endregion

                        PaintLoadingWave(g, path, rect_read);
                        PaintTextLoading(g, Text, enabled ? _fore : Colour.TextQuaternary.Get(nameof(Button), "foreDisabled", ColorScheme), rect_read, enabled, _radius);
                    }
                }
            }
            base.OnDraw(e);
        }

        #region 渲染帮助

        /// <summary>
        /// 绘制阴影
        /// </summary>
        void PaintShadow(Canvas g, Rectangle rect, GraphicsPath path, Color color, float radius)
        {
            float wave = (WaveSize * Config.Dpi / 2);
            using (var path_shadow = new RectangleF(rect.X, rect.Y + wave, rect.Width, rect.Height).RoundPath(radius))
            {
                path_shadow.AddPath(path, false);
                g.Fill(color, path_shadow);
            }
        }

        void PaintLoadingWave(Canvas g, GraphicsPath path, Rectangle rect)
        {
            if (loading && LoadingWaveValue > 0)
            {
                using (var brush = new SolidBrush(LoadingWaveColor ?? Colour.Fill.Get(nameof(Button), ColorScheme)))
                {
                    if (LoadingWaveValue >= 1) g.Fill(brush, path);
                    else if (LoadingWaveCount > 0)
                    {
                        var state = g.Save();
                        g.SetClip(path);
                        g.ResetTransform();
                        int len = (int)(LoadingWaveSize * Config.Dpi), count = LoadingWaveCount * 2 + 2;
                        if (count < 6) count = 6;
                        if (LoadingWaveVertical)
                        {
                            int pvalue = (int)(rect.Height * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                pvalue = rect.Height - pvalue + rect.Y;
                                int wd = rect.Width / LoadingWaveCount, wd2 = wd * 2, pvalue2 = pvalue - len, rr = rect.X + wd * count;
                                using (var path_line = new GraphicsPath())
                                {
                                    g.TranslateTransform(-(wd + wd2 * (AnimationLoadingWaveValue / 100F)), 0);
                                    path_line.AddLine(rr, pvalue, rr, rect.Bottom);
                                    path_line.AddLine(rr, rect.Bottom, rect.X, rect.Bottom);
                                    path_line.AddLine(rect.X, rect.Bottom, rect.X, pvalue);
                                    bool to = true;
                                    var line = new List<PointF>(count);
                                    for (int i = 0; i < count + 1; i++)
                                    {
                                        line.Add(new PointF(rect.X + wd * i, to ? pvalue : pvalue2));
                                        to = !to;
                                    }
                                    path_line.AddCurve(line.ToArray());
                                    g.Fill(brush, path_line);
                                }
                            }
                        }
                        else
                        {
                            int pvalue = (int)(rect.Width * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                pvalue += rect.X;
                                int wd = rect.Height / LoadingWaveCount, wd2 = wd * 2, pvalue2 = pvalue + len, rb = rect.Y + wd * count;

                                using (var path_line = new GraphicsPath())
                                {
                                    g.TranslateTransform(0, -(wd + wd2 * (AnimationLoadingWaveValue / 100F)));
                                    path_line.AddLine(pvalue, rb, rect.X, rb);
                                    path_line.AddLine(rect.X, rb, rect.X, rect.Y);
                                    path_line.AddLine(rect.X, rect.Y, pvalue, rect.Y);
                                    bool to = true;
                                    var line = new List<PointF>(count);
                                    for (int i = 0; i < count + 1; i++)
                                    {
                                        line.Add(new PointF(to ? pvalue : pvalue2, rect.Y + wd * i));
                                        to = !to;
                                    }
                                    path_line.AddCurve(line.ToArray());
                                    g.Fill(brush, path_line);
                                }
                            }
                        }
                        g.Restore(state);
                    }
                    else
                    {
                        if (LoadingWaveVertical)
                        {
                            int pvalue = (int)(rect.Height * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                var state = g.Save();
                                g.SetClip(new Rectangle(rect.X, rect.Y + rect.Height - pvalue, rect.Width, pvalue));
                                g.Fill(brush, path);
                                g.Restore(state);
                            }
                        }
                        else
                        {
                            int pvalue = (int)(rect.Width * LoadingWaveValue);
                            if (pvalue > 0)
                            {
                                var state = g.Save();
                                g.SetClip(new Rectangle(rect.X, rect.Y, pvalue, rect.Height));
                                g.Fill(brush, path);
                                g.Restore(state);
                            }
                        }
                    }
                }
            }
        }
        Size MeasureText(Canvas g, string? text, Rectangle rect, out int txt_height, out bool multiLine)
        {
            var font_height = g.MeasureText(Config.NullText, Font);
            txt_height = font_height.Height;
            multiLine = false;
            if (text == null) return font_height;
            else
            {
                var font_size = g.MeasureText(text, Font);
                if (font_size.Width > rect.Width && (textMultiLine || text.Contains("\n")))
                {
                    multiLine = true;
                    return g.MeasureText(text, Font, rect.Width);
                }
                else return font_size;
            }
        }
        Size MeasureText(Canvas g, string? text, out int txt_height)
        {
            var font_height = g.MeasureText(Config.NullText, Font);
            txt_height = font_height.Height;
            if (text == null) return font_height;
            else return g.MeasureText(text, Font);
        }
        void PaintTextLoading(Canvas g, string? text, Color color, Rectangle rect_read, bool enabled, float radius)
        {
            if (enabled && hasFocus && WaveSize > 0)
            {
                float wave = (WaveSize * Config.Dpi / 2), wave2 = wave * 2, r = radius + wave;
                var rect_focus = new RectangleF(rect_read.X - wave, rect_read.Y - wave, rect_read.Width + wave2, rect_read.Height + wave2);
                using (var path_focus = Path(rect_focus, r))
                {
                    g.Draw(Colour.PrimaryBorder.Get(nameof(Button), ColorScheme), wave, path_focus);
                }
            }
            bool has_loading = loading && LoadingValue > -1;
            var font_size = MeasureText(g, text, rect_read, out int txt_height, out bool textLine);
            if (virtualWidth.HasValue) font_size.Width = virtualWidth.Value;
            if (text == null || displayStyle == TButtonDisplayStyle.Image)
            {
                //没有文字
                var rect = GetIconRectCenter(font_size, rect_read);
                if (has_loading)
                {
                    float loading_size = rect_read.Height * 0.06F;
                    using (var brush = new Pen(color, loading_size))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect, AnimationLoadingValue, LoadingValue * 360F);
                    }
                }
                else
                {
                    if (PaintIcon(g, color, rect, false, enabled) && showArrow)
                    {
                        int size = (int)(txt_height * IconRatio);
                        var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                        PaintTextArrow(g, rect_arrow, color);
                    }
                }
            }
            else
            {
                if (textMultiLine && font_size.Width > rect_read.Width) font_size.Width = rect_read.Width;
                Rectangle rect_text;
                if (displayStyle == TButtonDisplayStyle.Default)
                {
                    bool has_left = has_loading || HasIcon, has_right = showArrow;
                    if (has_left || has_right)
                    {
                        if (has_left && has_right)
                        {
                            rect_text = RectAlignLR(g, txt_height, textLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l, out var rect_r);

                            if (has_loading)
                            {
                                float loading_size = rect_l.Height * .14F;
                                using (var brush = new Pen(color, loading_size))
                                {
                                    brush.StartCap = brush.EndCap = LineCap.Round;
                                    g.DrawArc(brush, rect_l, AnimationLoadingValue, LoadingValue * 360F);
                                }
                            }
                            else PaintIcon(g, color, rect_l, true, enabled);

                            PaintTextArrow(g, rect_r, color);
                        }
                        else if (has_left)
                        {
                            rect_text = RectAlignL(g, txt_height, textLine, textCenterHasIcon, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l);
                            if (has_loading)
                            {
                                float loading_size = rect_l.Height * .14F;
                                using (var brush = new Pen(color, loading_size))
                                {
                                    brush.StartCap = brush.EndCap = LineCap.Round;
                                    g.DrawArc(brush, rect_l, AnimationLoadingValue, LoadingValue * 360F);
                                }
                            }
                            else PaintIcon(g, color, rect_l, true, enabled);
                        }
                        else
                        {
                            rect_text = RectAlignR(g, txt_height, textLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_r);

                            PaintTextArrow(g, rect_r, color);
                        }
                    }
                    else
                    {
                        int sps = (int)(txt_height * .4F), sps2 = sps * 2;
                        rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                        PaintTextAlign(rect_read, ref rect_text);
                    }
                }
                else
                {
                    if (has_loading)
                    {
                        rect_text = RectAlignL(g, txt_height, textLine, textCenterHasIcon, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l);
                        float loading_size = rect_l.Height * .14F;
                        using (var brush = new Pen(color, loading_size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, rect_l, AnimationLoadingValue, LoadingValue * 360F);
                        }
                    }
                    else
                    {
                        int sps = (int)(txt_height * .4F), sps2 = sps * 2;
                        rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                        PaintTextAlign(rect_read, ref rect_text);
                    }
                }
                g.DrawText(text, Font, color, rect_text, stringFormat);
            }
        }

        void PaintTextArrow(Canvas g, Rectangle rect, Color color)
        {
            using (var pen = new Pen(color, 2F * Config.Dpi))
            {
                pen.StartCap = pen.EndCap = LineCap.Round;
                if (isLink)
                {
                    float size_arrow = rect.Width / 2F;
                    g.TranslateTransform(rect.X + size_arrow, rect.Y + size_arrow);
                    g.RotateTransform(-90F);
                    g.DrawLines(pen, new RectangleF(-size_arrow, -size_arrow, rect.Width, rect.Height).TriangleLines(ArrowProg));
                    g.ResetTransform();
                }
                else g.DrawLines(pen, rect.TriangleLines(ArrowProg));
            }
        }

        internal static Rectangle RectAlignL(Canvas g, int font_Height, bool multiLine, bool textCenter, Font font, TAlignMini iconPosition, float iconratio, float icongap, Size font_size, Rectangle rect_read, out Rectangle rect_l)
        {
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * icongap);
            if (multiLine && (iconPosition == TAlignMini.Left || iconPosition == TAlignMini.Right))
            {
                int rw = icon_size + sp;
                if (font_size.Width + rw > rect_read.Width) font_size.Width = rect_read.Width - rw;
            }
            Rectangle rect_text;
            if (textCenter)
            {
                switch (iconPosition)
                {
                    case TAlignMini.Top:
                        int t_x = rect_read.Y + ((rect_read.Height - font_size.Height) / 2);
                        rect_text = new Rectangle(rect_read.X, t_x, rect_read.Width, font_size.Height);
                        rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, t_x - icon_size - sp, icon_size, icon_size);
                        break;
                    case TAlignMini.Bottom:
                        int b_x = rect_read.Y + ((rect_read.Height - font_size.Height) / 2);
                        rect_text = new Rectangle(rect_read.X, b_x, rect_read.Width, font_size.Height);
                        rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, b_x + font_size.Height + sp, icon_size, icon_size);
                        break;
                    case TAlignMini.Right:
                        int r_x = rect_read.X + ((rect_read.Width - font_size.Width) / 2);
                        rect_text = new Rectangle(r_x, rect_read.Y, font_size.Width, rect_read.Height);
                        rect_l = new Rectangle(r_x + font_size.Width + sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        break;
                    case TAlignMini.Left:
                    default:
                        int l_x = rect_read.X + ((rect_read.Width - font_size.Width) / 2);
                        rect_text = new Rectangle(l_x, rect_read.Y, font_size.Width, rect_read.Height);
                        rect_l = new Rectangle(l_x - icon_size - sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        break;
                }
            }
            else
            {
                switch (iconPosition)
                {
                    case TAlignMini.Top:
                        int t_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                        rect_text = new Rectangle(rect_read.X, t_x + icon_size + sp, rect_read.Width, font_size.Height);
                        rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, t_x, icon_size, icon_size);
                        break;
                    case TAlignMini.Bottom:
                        int b_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                        rect_text = new Rectangle(rect_read.X, b_x, rect_read.Width, font_size.Height);
                        rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, b_x + font_size.Height + sp, icon_size, icon_size);
                        break;
                    case TAlignMini.Right:
                        int r_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp)) / 2);
                        rect_text = new Rectangle(r_x, rect_read.Y, font_size.Width, rect_read.Height);
                        rect_l = new Rectangle(r_x + font_size.Width + sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        break;
                    case TAlignMini.Left:
                    default:
                        int l_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp)) / 2);
                        rect_text = new Rectangle(l_x + icon_size + sp, rect_read.Y, font_size.Width, rect_read.Height);
                        rect_l = new Rectangle(l_x, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        break;
                }
            }
            return rect_text;
        }
        internal static Rectangle RectAlignLR(Canvas g, int font_Height, bool multiLine, Font font, TAlignMini iconPosition, float iconratio, float icongap, Size font_size, Rectangle rect_read, out Rectangle rect_l, out Rectangle rect_r)
        {
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * icongap), sps = (int)(font_Height * .4F);
            if (multiLine && (iconPosition == TAlignMini.Left || iconPosition == TAlignMini.Right))
            {
                int rw = (icon_size + sp) * 2;
                if (font_size.Width + rw > rect_read.Width) font_size.Width = rect_read.Width - rw;
            }
            Rectangle rect_text;
            switch (iconPosition)
            {
                case TAlignMini.Top:
                    int t_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                    rect_text = new Rectangle(rect_read.X, t_x + icon_size + sp, rect_read.Width, font_size.Height);
                    rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, t_x, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, rect_text.Y + (rect_text.Height - icon_size) / 2, icon_size, icon_size);
                    break;
                case TAlignMini.Bottom:
                    int b_x = rect_read.Y + ((rect_read.Height - (font_size.Height + icon_size + sp)) / 2);
                    rect_text = new Rectangle(rect_read.X, b_x, rect_read.Width, font_size.Height);
                    rect_l = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, b_x + font_size.Height + sp, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, rect_text.Y + (rect_text.Height - icon_size) / 2, icon_size, icon_size);
                    break;
                case TAlignMini.Right:
                    int r_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp + sps)) / 2), r_y = rect_read.Y + (rect_read.Height - icon_size) / 2;
                    rect_text = new Rectangle(r_x, rect_read.Y, font_size.Width, rect_read.Height);
                    rect_l = new Rectangle(r_x + font_size.Width + sp, r_y, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.X + sps, r_y, icon_size, icon_size);
                    break;
                case TAlignMini.Left:
                default:
                    int l_x = rect_read.X + ((rect_read.Width - (font_size.Width + icon_size + sp + sps)) / 2), l_y = rect_read.Y + (rect_read.Height - icon_size) / 2;
                    rect_text = new Rectangle(l_x + icon_size + sp, rect_read.Y, font_size.Width, rect_read.Height);
                    rect_l = new Rectangle(l_x, l_y, icon_size, icon_size);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, l_y, icon_size, icon_size);
                    break;
            }
            return rect_text;
        }
        internal static Rectangle RectAlignR(Canvas g, int font_Height, bool multiLine, Font font, TAlignMini iconPosition, float iconratio, float icongap, Size font_size, Rectangle rect_read, out Rectangle rect_r)
        {
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * icongap), sps = (int)(font_Height * .4F), rsps = icon_size + sp;
            if (multiLine && (iconPosition == TAlignMini.Left || iconPosition == TAlignMini.Right))
            {
                int rw = icon_size + sp;
                if (font_size.Width + rw > rect_read.Width) font_size.Width = rect_read.Width - rw;
            }
            Rectangle rect_text;
            switch (iconPosition)
            {
                case TAlignMini.Bottom:
                case TAlignMini.Right:
                    rect_text = new Rectangle(rect_read.X + rsps, rect_read.Y, rect_read.Width - rsps, rect_read.Height);
                    rect_r = new Rectangle(rect_read.X + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                    break;
                case TAlignMini.Top:
                case TAlignMini.Left:
                default:
                    rect_text = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width - rsps, rect_read.Height);
                    rect_r = new Rectangle(rect_read.Right - icon_size - sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                    break;
            }
            return rect_text;
        }

        void PaintTextAlign(Rectangle rect_read, ref Rectangle rect_text)
        {
            switch (textAlign)
            {
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    rect_text.Y = rect_read.Y;
                    rect_text.Height = rect_read.Height;
                    break;
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                case ContentAlignment.TopCenter:
                    rect_text.Height = rect_read.Height - rect_text.Y;
                    break;
            }
        }

        #region 渲染图标

        /// <summary>
        /// 居中的图标绘制区域
        /// </summary>
        /// <param name="font_size">字体大小</param>
        /// <param name="rect_read">客户区域</param>
        Rectangle GetIconRectCenter(Size font_size, Rectangle rect_read)
        {
            if (IconSize.Width > 0 && IconSize.Height > 0)
            {
                int w = (int)(IconSize.Width * Config.Dpi), h = (int)(IconSize.Height * Config.Dpi);
                return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - h) / 2, w, h);
            }
            else
            {
                int w = (int)(font_size.Height * IconRatio * 1.125F);
                return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - w) / 2, w, w);
            }
        }


        /// <summary>
        /// 渲染图标
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rect_o">区域</param>
        /// <param name="hastxt">包含文本</param>
        /// <param name="enabled">使能</param>
        bool PaintIcon(Canvas g, Color? color, Rectangle rect_o, bool hastxt, bool enabled)
        {
            var rect = hastxt ? GetIconRect(rect_o) : rect_o;
            if (AnimationIconHover)
            {
                PaintCoreIcon(g, rect, color, 1F - AnimationIconHoverValue);
                PaintCoreIconHover(g, rect, color, AnimationIconHoverValue);
                return false;
            }
            else if (AnimationIconToggle)
            {
                float d = 1F - AnimationIconToggleValue;
                if (ExtraMouseHover)
                {
                    if (!PaintCoreIcon(g, IconHover, IconHoverSvg, rect, color, d)) PaintCoreIcon(g, icon, iconSvg, rect, color, d);
                    if (!PaintCoreIcon(g, ToggleIconHover, ToggleIconHoverSvg, rect, color, AnimationIconToggleValue)) PaintCoreIcon(g, ToggleIcon, ToggleIconSvg, rect, color, AnimationIconToggleValue);
                }
                else
                {
                    PaintCoreIcon(g, icon, iconSvg, rect, color, d);
                    PaintCoreIcon(g, iconToggle, iconSvgToggle, rect, color, AnimationIconToggleValue);
                }
                return false;
            }
            else
            {
                if (enabled)
                {
                    if (ExtraMouseHover)
                    {
                        if (PaintCoreIconHover(g, rect, color)) return false;
                    }
                    if (PaintCoreIcon(g, rect, color)) return false;
                }
                else
                {
                    if (ExtraMouseHover)
                    {
                        if (PaintCoreIconHover(g, rect, color, .3F)) return false;
                    }
                    if (PaintCoreIcon(g, rect, color, .3F)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 图标绘制区域
        /// </summary>
        /// <param name="rectl">图标区域</param>
        Rectangle GetIconRect(Rectangle rectl)
        {
            if (IconSize.Width > 0 && IconSize.Height > 0)
            {
                int w = (int)(IconSize.Width * Config.Dpi), h = (int)(IconSize.Height * Config.Dpi);
                return new Rectangle(rectl.X + (rectl.Width - w) / 2, rectl.Y + (rectl.Height - h) / 2, w, h);
            }
            else return rectl;
        }

        bool PaintCoreIcon(Canvas g, Rectangle rect, Color? color, float opacity = 1F) => toggle ? PaintCoreIcon(g, iconToggle, iconSvgToggle, rect, color, opacity) : PaintCoreIcon(g, icon, iconSvg, rect, color, opacity);
        bool PaintCoreIconHover(Canvas g, Rectangle rect, Color? color, float opacity = 1F) => toggle ? PaintCoreIcon(g, ToggleIconHover, ToggleIconHoverSvg, rect, color, opacity) : PaintCoreIcon(g, IconHover, IconHoverSvg, rect, color, opacity);

        bool PaintCoreIcon(Canvas g, Image? icon, string? iconSvg, Rectangle rect, Color? color, float opacity = 1F)
        {
            int count = 0;
            if (icon != null)
            {
                g.Image(icon, rect, opacity);
                count++;
            }
            if (iconSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(iconSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.Image(_bmp, rect, opacity);
                        count++;
                    }
                }
            }
            return count > 0;
        }

        #endregion

        public GraphicsPath Path(RectangleF rect, float radius)
        {
            if (shape == TShape.Circle)
            {
                var path = new GraphicsPath();
                path.AddEllipse(rect);
                return path;
            }
            switch (joinMode)
            {
                case TJoinMode.Left:
                    return rect.RoundPath(radius, true, false, false, true);
                case TJoinMode.Right:
                    return rect.RoundPath(radius, false, true, true, false);
                case TJoinMode.LR:
                case TJoinMode.TB:
                    return rect.RoundPath(0);
                case TJoinMode.Top:
                    return rect.RoundPath(radius, true, true, false, false);
                case TJoinMode.Bottom:
                    return rect.RoundPath(radius, false, false, true, true);
                case TJoinMode.None:
                default:
                    if (joinLeft && joinRight) return rect.RoundPath(0);
                    else if (joinRight) return rect.RoundPath(radius, true, false, false, true);
                    else if (joinLeft) return rect.RoundPath(radius, false, true, true, false);
                    return rect.RoundPath(radius);
            }
        }

        #endregion

        #region 帮助

        Color GetColorO()
        {
            if (toggle)
            {
                if (typeToggle.HasValue) return GetColorO(typeToggle.Value);
                else return GetColorO(type);
            }
            return GetColorO(type);
        }
        Color GetColorO(TTypeMini type)
        {
            Color color;
            switch (type)
            {
                case TTypeMini.Default:
                    if (borderWidth > 0) color = Colour.PrimaryHover.Get(nameof(Button), ColorScheme);
                    else color = Colour.FillSecondary.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Success:
                    color = Colour.SuccessHover.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Error:
                    color = Colour.ErrorHover.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Info:
                    color = Colour.InfoHover.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Warn:
                    color = Colour.WarningHover.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Primary:
                default:
                    color = Colour.PrimaryHover.Get(nameof(Button), ColorScheme);
                    break;
            }
            if (BackHover.HasValue) color = BackHover.Value;
            return color;
        }

        void GetDefaultColorConfig(out Color Fore, out Color Color, out Color backHover, out Color backActive)
        {
            Fore = Colour.DefaultColor.Get(nameof(Button), ColorScheme);
            Color = Colour.Primary.Get(nameof(Button), ColorScheme);
            if (borderWidth > 0)
            {
                backHover = Colour.PrimaryHover.Get(nameof(Button), ColorScheme);
                backActive = Colour.PrimaryActive.Get(nameof(Button), ColorScheme);
            }
            else
            {
                backHover = Colour.FillSecondary.Get(nameof(Button), ColorScheme);
                backActive = Colour.Fill.Get(nameof(Button), ColorScheme);
            }
            if (toggle)
            {
                if (foreToggle.HasValue) Fore = foreToggle.Value;
                if (ToggleBackHover.HasValue) backHover = ToggleBackHover.Value;
                if (ToggleBackActive.HasValue) backActive = ToggleBackActive.Value;
            }
            else
            {
                if (fore.HasValue) Fore = fore.Value;
                if (BackHover.HasValue) backHover = BackHover.Value;
                if (BackActive.HasValue) backActive = BackActive.Value;
            }
            if (AnimationBlinkState && colorBlink.HasValue) Color = colorBlink.Value;
            if (loading && LoadingValue > -1)
            {
                Fore = Color.FromArgb(165, Fore);
                Color = Color.FromArgb(165, Color);
            }
        }

        void GetColorConfig(out Color Fore, out Color Back, out Color backHover, out Color backActive)
        {
            if (toggle)
            {
                if (typeToggle.HasValue) GetColorConfig(typeToggle.Value, out Fore, out Back, out backHover, out backActive);
                else GetColorConfig(type, out Fore, out Back, out backHover, out backActive);

                if (foreToggle.HasValue) Fore = foreToggle.Value;
                if (backToggle.HasValue) Back = backToggle.Value;
                if (ToggleBackHover.HasValue) backHover = ToggleBackHover.Value;
                if (ToggleBackActive.HasValue) backActive = ToggleBackActive.Value;
                if (loading && LoadingValue > -1) Back = Color.FromArgb(165, Back);
                return;
            }
            GetColorConfig(type, out Fore, out Back, out backHover, out backActive);
            if (fore.HasValue) Fore = fore.Value;
            if (back.HasValue) Back = back.Value;
            if (BackHover.HasValue) backHover = BackHover.Value;
            if (BackActive.HasValue) backActive = BackActive.Value;
            if (AnimationBlinkState && colorBlink.HasValue) Back = colorBlink.Value;
            if (loading && LoadingValue > -1) Back = Color.FromArgb(165, Back);
        }

        void GetColorConfig(TTypeMini type, out Color Fore, out Color Back, out Color backHover, out Color backActive)
        {
            switch (type)
            {
                case TTypeMini.Error:
                    Back = Colour.Error.Get(nameof(Button), ColorScheme);
                    Fore = Colour.ErrorColor.Get(nameof(Button), ColorScheme);
                    backHover = Colour.ErrorHover.Get(nameof(Button), ColorScheme);
                    backActive = Colour.ErrorActive.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Success:
                    Back = Colour.Success.Get(nameof(Button), ColorScheme);
                    Fore = Colour.SuccessColor.Get(nameof(Button), ColorScheme);
                    backHover = Colour.SuccessHover.Get(nameof(Button), ColorScheme);
                    backActive = Colour.SuccessActive.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Info:
                    Back = Colour.Info.Get(nameof(Button), ColorScheme);
                    Fore = Colour.InfoColor.Get(nameof(Button), ColorScheme);
                    backHover = Colour.InfoHover.Get(nameof(Button), ColorScheme);
                    backActive = Colour.InfoActive.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Warn:
                    Back = Colour.Warning.Get(nameof(Button), ColorScheme);
                    Fore = Colour.WarningColor.Get(nameof(Button), ColorScheme);
                    backHover = Colour.WarningHover.Get(nameof(Button), ColorScheme);
                    backActive = Colour.WarningActive.Get(nameof(Button), ColorScheme);
                    break;
                case TTypeMini.Primary:
                default:
                    Back = Colour.Primary.Get(nameof(Button), ColorScheme);
                    Fore = Colour.PrimaryColor.Get(nameof(Button), ColorScheme);
                    backHover = Colour.PrimaryHover.Get(nameof(Button), ColorScheme);
                    backActive = Colour.PrimaryActive.Get(nameof(Button), ColorScheme);
                    break;
            }
        }

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, shape, joinMode, joinLeft, joinRight);

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect = ReadRectangle;
                return Path(rect, (shape == TShape.Round || shape == TShape.Circle) ? rect.Height : radius * Config.Dpi);
            }
        }
        #endregion

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RespondRealAreas)
            {
                var rect_read = ReadRectangle;
                using (var path = Path(rect_read, radius * Config.Dpi))
                {
                    ExtraMouseHover = path.IsVisible(e.X, e.Y);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (RespondRealAreas) return;
            ExtraMouseHover = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ExtraMouseHover = false;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ExtraMouseHover = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (CanClick(e.X, e.Y))
            {
                init = false;
                Focus();
                base.OnMouseDown(e);
                ExtraMouseDown = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (ExtraMouseDown)
            {
                if (CanClick(e.X, e.Y))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        ClickAnimation();
                        OnClick(e);
                    }
                    OnMouseClick(e);
                }
                ExtraMouseDown = false;
            }
        }

        #endregion

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(false)]
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                if (base.AutoSize == value) return;
                base.AutoSize = value;
                if (value)
                {
                    if (autoSize == TAutoSize.None) autoSize = TAutoSize.Auto;
                }
                else autoSize = TAutoSize.None;
                BeforeAutoSize();
            }
        }

        TAutoSize autoSize = TAutoSize.None;
        /// <summary>
        /// 自动大小模式
        /// </summary>
        [Description("自动大小模式"), Category("外观"), DefaultValue(TAutoSize.None)]
        public TAutoSize AutoSizeMode
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
                base.AutoSize = autoSize != TAutoSize.None;
                BeforeAutoSize();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            BeforeAutoSize();
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (autoSize == TAutoSize.None) return base.GetPreferredSize(proposedSize);
            else if (autoSize == TAutoSize.Width) return new Size(PSize.Width, base.GetPreferredSize(proposedSize).Height);
            else if (autoSize == TAutoSize.Height) return new Size(base.GetPreferredSize(proposedSize).Width, PSize.Height);
            return PSize;
        }

        public Size PSize
        {
            get
            {
                return Helper.GDI(g =>
                {
                    var font_size = MeasureText(g, Text, out int txt_height);
                    int icon_size = (int)(txt_height * iconratio), gap = (int)(txt_height * 1.02F), wave = (int)(WaveSize * Config.Dpi), wave2 = wave;
                    int height = Math.Max(font_size.Height, icon_size);
                    if (Shape == TShape.Circle || string.IsNullOrEmpty(Text) || displayStyle == TButtonDisplayStyle.Image)
                    {
                        int s = height + wave + gap;
                        return new Size(s, s);
                    }
                    else
                    {
                        if (joinMode > 0) wave2 = 0;
                        else if (joinLeft || joinRight) wave2 = 0;
                        bool has_icon = (loading && LoadingValue > -1) || (HasIcon && displayStyle == TButtonDisplayStyle.Default);
                        if (has_icon && showArrow)
                        {
                            int sp = (int)(txt_height * icongap);
                            int y = gap + wave, x = y + wave2 + (icon_size + sp) * 2;
                            if (IconPosition == TAlignMini.Top || IconPosition == TAlignMini.Bottom) return new Size(font_size.Width + y, font_size.Height + x);
                            else return new Size(font_size.Width + x, height + y);
                        }
                        else if (has_icon)
                        {
                            int sp = (int)(txt_height * icongap);
                            int y = gap + wave, x = y + wave2 + icon_size + sp;
                            if (IconPosition == TAlignMini.Top || IconPosition == TAlignMini.Bottom) return new Size(font_size.Width + y, font_size.Height + x);
                            else return new Size(font_size.Width + x, height + y);
                        }
                        else if (showArrow)
                        {
                            int sp = (int)(txt_height * icongap);
                            int y = gap + wave, x = y + wave2 + icon_size + sp;
                            return new Size(font_size.Width + x, height + y);
                        }
                        return new Size(font_size.Width + gap + wave + wave2, height + gap + wave);
                    }
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired) return ITask.Invoke(this, BeforeAutoSize);
            var PS = PSize;
            switch (autoSize)
            {
                case TAutoSize.Width:
                    if (Width == PS.Width) return true;
                    Width = PS.Width;
                    break;
                case TAutoSize.Height:
                    if (Height == PS.Height) return true;
                    Height = PS.Height;
                    break;
                case TAutoSize.Auto:
                default:
                    if (Width == PS.Width && Height == PS.Height) return true;
                    Size = PS;
                    break;
            }
            return false;
        }

        #endregion

        #region 按钮点击

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode is Keys.Space && CanClick())
            {
                ClickAnimation();
                OnClick(EventArgs.Empty);
                e.Handled = true;
            }
        }

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        /// <summary>
        /// 是否默认按钮
        /// </summary>
        public void NotifyDefault(bool value) { }

        public void PerformClick()
        {
            if (CanSelect && CanClick())
            {
                ClickAnimation();
                OnClick(EventArgs.Empty);
            }
        }

        bool CanClick(Point e) => CanClick(e.X, e.Y);
        bool CanClick(int x, int y)
        {
            if (loading) return LoadingRespondClick;
            else
            {
                if (RespondRealAreas)
                {
                    var rect_read = ReadRectangle;
                    using (var path = Path(rect_read, radius * Config.Dpi))
                    {
                        return path.IsVisible(x, y);
                    }
                }
                else return ClientRectangle.Contains(x, y);
            }
        }

        bool CanClick()
        {
            if (loading) return LoadingRespondClick;
            else return true;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler? DoubleClick
        {
            add => base.DoubleClick += value;
            remove => base.DoubleClick -= value;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event MouseEventHandler? MouseDoubleClick
        {
            add => base.MouseDoubleClick += value;
            remove => base.MouseDoubleClick -= value;
        }

        #endregion

        #region 焦点

        bool hasFocus = false;
        /// <summary>
        /// 是否存在焦点
        /// </summary>
        [Browsable(false)]
        [Description("是否存在焦点"), Category("行为"), DefaultValue(false)]
        public bool HasFocus
        {
            get => hasFocus;
            private set
            {
                if (value && (_mouseDown || _mouseHover)) value = false;
                if (hasFocus == value) return;
                hasFocus = value;
                Invalidate();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (init) HasFocus = true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            HasFocus = false;
            base.OnLostFocus(e);
        }

        #endregion

        #region 语言变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.LANG:
                    BeforeAutoSize();
                    break;
            }
        }

        #endregion

        protected override void OnClick(EventArgs e)
        {
            Form? form = FindForm();
            if (form != null) form.DialogResult = DialogResult;
            base.OnClick(e);
        }

        protected override void Dispose(bool disposing)
        {
            ThreadClick?.Dispose();
            ThreadHover?.Dispose();
            ThreadIconHover?.Dispose();
            ThreadIconToggle?.Dispose();
            ThreadLoading?.Dispose();
            ThreadAnimateBlink?.Dispose();
            base.Dispose(disposing);
        }
    }
}