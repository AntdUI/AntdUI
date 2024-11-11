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
    public class Button : IControl, IButtonControl
    {
        public Button()
        {
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
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
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
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
            }
        }

        string? backExtend = null;
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

        Image? backImage = null;
        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public override Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
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
            }
        }

        /// <summary>
        /// 箭头角度
        /// </summary>
        [Browsable(false), Description("箭头角度"), Category("外观"), DefaultValue(-1F)]
        public float ArrowProg { get; set; } = -1F;

        #region 文本

        bool textLine = false;
        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => text;
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (text == value) return;
                text = value;
                if (text == null) textLine = false;
                else textLine = text.Contains(Environment.NewLine);
                if (BeforeAutoSize()) Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

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
                if (loading || HasIcon || showArrow)
                {
                    value = ContentAlignment.MiddleCenter;
                    if (textAlign == value) return;
                }
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
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
            }
        }

        Image? icon = null;
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
            }
        }

        string? iconSvg = null;
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
            }
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => iconSvg != null || icon != null;

        /// <summary>
        /// 图标大小
        /// </summary>
        [Description("图标大小"), Category("外观"), DefaultValue(typeof(Size), "0, 0")]
        public Size IconSize { get; set; } = new Size(0, 0);

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
                if (Config.Animation)
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
            }
        }

        Image? iconToggle = null;
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
            }
        }

        string? iconSvgToggle = null;
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
            }
        }

        TTypeMini? typeToggle = null;
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
            }
        }

        string? backExtendToggle = null;
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
            }
        }

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

        bool joinLeft = false;
        /// <summary>
        /// 连接左边
        /// </summary>
        [Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft
        {
            get => joinLeft;
            set
            {
                if (joinLeft == value) return;
                joinLeft = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        bool joinRight = false;
        /// <summary>
        /// 连接右边
        /// </summary>
        [Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight
        {
            get => joinRight;
            set
            {
                if (joinRight == value) return;
                joinRight = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }


        ITask? ThreadHover = null;
        ITask? ThreadIconHover = null;
        ITask? ThreadIconToggle = null;
        ITask? ThreadClick = null;
        ITask? ThreadLoading = null;

        #region 点击动画

        bool AnimationClick = false;
        float AnimationClickValue = 0;

        public void ClickAnimation()
        {
            if (WaveSize > 0 && Config.Animation)
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
                SetCursor(value && Enabled && !loading);
                if (Enabled)
                {
                    var backHover = GetColorO();
                    int alpha = backHover.A;
                    if (Config.Animation)
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
        ITask? ThreadAnimateBlink = null;
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
                if (AnimationBlinkState)
                {
                    int index = 0, len = colors.Length;
                    ThreadAnimateBlink = new ITask(this, () =>
                    {
                        colorBlink = colors[index];
                        index++;
                        if (index > len - 1) index = 0;
                        Invalidate();
                        return AnimationBlinkState;
                    }, interval, () =>
                    {
                        Invalidate();
                    });
                }
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

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            Rectangle rect = ClientRectangle.PaddingRect(Padding), rect_read = ReadRectangle;
            float _radius = (shape == TShape.Round || shape == TShape.Circle) ? rect_read.Height : radius * Config.Dpi;
            if (backImage != null) g.Image(rect_read, backImage, backFit, _radius, shape);
            bool is_default = type == TTypeMini.Default;
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

                    if (Enabled)
                    {
                        if (!ghost)
                        {
                            #region 绘制阴影

                            if (WaveSize > 0)
                            {
                                using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                                {
                                    path_shadow.AddPath(path, false);
                                    g.Fill(Style.Db.FillQuaternary, path_shadow);
                                }
                            }

                            #endregion

                            g.Fill(defaultback ?? Style.Db.DefaultBg, path);
                        }
                        if (borderWidth > 0)
                        {
                            PaintLoadingWave(g, path, rect_read);
                            float border = borderWidth * Config.Dpi;
                            if (ExtraMouseDown)
                            {
                                g.Draw(_back_active, border, path);
                                PaintTextLoading(g, text, _back_active, rect_read, Enabled);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Helper.ToColor(AnimationHoverValue, _back_hover);
                                g.Draw(Style.Db.DefaultBorder, border, path);
                                g.Draw(colorHover, border, path);
                                PaintTextLoading(g, text, _fore, colorHover, rect_read);
                            }
                            else if (ExtraMouseHover)
                            {
                                g.Draw(_back_hover, border, path);
                                PaintTextLoading(g, text, _back_hover, rect_read, Enabled);
                            }
                            else
                            {
                                if (AnimationBlinkState && colorBlink.HasValue) g.Draw(colorBlink.Value, border, path);
                                else g.Draw(defaultbordercolor ?? Style.Db.DefaultBorder, border, path);
                                PaintTextLoading(g, text, _fore, rect_read, Enabled);
                            }
                        }
                        else
                        {
                            if (ExtraMouseDown) g.Fill(_back_active, path);
                            else if (AnimationHover) g.Fill(Helper.ToColor(AnimationHoverValue, _back_hover), path);
                            else if (ExtraMouseHover) g.Fill(_back_hover, path);
                            PaintLoadingWave(g, path, rect_read);
                            PaintTextLoading(g, text, _fore, rect_read, Enabled);
                        }
                    }
                    else
                    {
                        PaintLoadingWave(g, path, rect_read);
                        if (!ghost) g.Fill(Style.Db.FillTertiary, path);
                        PaintTextLoading(g, text, Style.Db.TextQuaternary, rect_read, Enabled);
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
                                PaintTextLoading(g, text, _back_active, rect_read, Enabled);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Helper.ToColor(AnimationHoverValue, _back_hover);
                                g.Draw(Enabled ? _back : Style.Db.FillTertiary, border, path);
                                g.Draw(colorHover, border, path);
                                PaintTextLoading(g, text, _back, colorHover, rect_read);
                            }
                            else if (ExtraMouseHover)
                            {
                                g.Draw(_back_hover, border, path);
                                PaintTextLoading(g, text, _back_hover, rect_read, Enabled);
                            }
                            else
                            {
                                if (Enabled)
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
                                else g.Draw(Style.Db.FillTertiary, border, path);
                                PaintTextLoading(g, text, Enabled ? _back : Style.Db.TextQuaternary, rect_read, Enabled);
                            }
                        }
                        else PaintTextLoading(g, text, Enabled ? _back : Style.Db.TextQuaternary, rect_read, Enabled);

                        #endregion
                    }
                    else
                    {
                        #region 绘制阴影

                        if (Enabled && WaveSize > 0)
                        {
                            using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                            {
                                path_shadow.AddPath(path, false);
                                g.Fill(_back.rgba(Config.Mode == TMode.Dark ? 0.15F : 0.1F), path_shadow);
                            }
                        }

                        #endregion

                        #region 绘制背景

                        if (Enabled)
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
                        else g.Fill(Style.Db.FillTertiary, path);

                        if (ExtraMouseDown) g.Fill(_back_active, path);
                        else if (AnimationHover) g.Fill(Helper.ToColor(AnimationHoverValue, _back_hover), path);
                        else if (ExtraMouseHover) g.Fill(_back_hover, path);

                        #endregion

                        PaintLoadingWave(g, path, rect_read);
                        PaintTextLoading(g, text, Enabled ? _fore : Style.Db.TextQuaternary, rect_read, Enabled);
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        void PaintLoadingWave(Canvas g, GraphicsPath path, Rectangle rect)
        {
            if (loading && LoadingWaveValue > 0)
            {
                using (var brush = new SolidBrush(LoadingWaveColor ?? Style.Db.Fill))
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
        void PaintTextLoading(Canvas g, string? text, Color color, Rectangle rect_read, bool enabled)
        {
            var font_size = g.MeasureString(text ?? Config.NullText, Font);
            if (text == null)
            {
                //没有文字
                var rect = GetIconRectCenter(font_size, rect_read);
                if (loading)
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
                    if (PaintIcon(g, color, rect, false, Enabled) && showArrow)
                    {
                        int size = (int)(font_size.Height * IconRatio);
                        var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_arrow.Width / 2;
                                g.TranslateTransform(rect_arrow.X + size_arrow, rect_arrow.Y + size_arrow);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_arrow.Width, rect_arrow.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else g.DrawLines(pen, rect_arrow.TriangleLines(ArrowProg));
                        }
                    }
                }
            }
            else
            {
                bool has_left = loading || HasIcon, has_right = showArrow;
                Rectangle rect_text;
                if (has_left || has_right)
                {
                    if (has_left && has_right)
                    {
                        rect_text = RectAlignLR(g, textLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l, out var rect_r);

                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, LoadingValue * 360F);
                            }
                        }
                        else PaintIcon(g, color, rect_l, true, Enabled);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else g.DrawLines(pen, rect_r.TriangleLines(ArrowProg));
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        rect_text = RectAlignL(g, textLine, textCenterHasIcon, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l);
                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, LoadingValue * 360F);
                            }
                        }
                        else PaintIcon(g, color, rect_l, true, Enabled);
                    }
                    else
                    {
                        rect_text = RectAlignR(g, textLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_r);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else g.DrawLines(pen, rect_r.TriangleLines(ArrowProg));
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                    PaintTextAlign(rect_read, ref rect_text);
                }
                using (var brush = new SolidBrush(color))
                {
                    g.String(text, Font, brush, rect_text, stringFormat);
                }
            }
        }
        void PaintTextLoading(Canvas g, string? text, Color color, Color colorHover, Rectangle rect_read)
        {
            var font_size = g.MeasureString(text ?? Config.NullText, Font);
            if (text == null)
            {
                var rect = GetIconRectCenter(font_size, rect_read);
                if (loading)
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
                    if (PaintIcon(g, color, rect, false, true))
                    {
                        if (showArrow)
                        {
                            int size = (int)(font_size.Height * IconRatio);
                            var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                            using (var pen = new Pen(color, 2F * Config.Dpi))
                            using (var penHover = new Pen(colorHover, pen.Width))
                            {
                                pen.StartCap = pen.EndCap = LineCap.Round;
                                if (isLink)
                                {
                                    int size_arrow = rect_arrow.Width / 2;
                                    g.TranslateTransform(rect_arrow.X + size_arrow, rect_arrow.Y + size_arrow);
                                    g.RotateTransform(-90F);
                                    var rect_arrow_lines = new Rectangle(-size_arrow, -size_arrow, rect_arrow.Width, rect_arrow.Height).TriangleLines(ArrowProg);
                                    g.DrawLines(pen, rect_arrow_lines);
                                    g.DrawLines(penHover, rect_arrow_lines);
                                    g.ResetTransform();
                                }
                                else
                                {
                                    var rect_arrow_lines = rect_arrow.TriangleLines(ArrowProg);
                                    g.DrawLines(pen, rect_arrow_lines);
                                    g.DrawLines(penHover, rect_arrow_lines);
                                }
                            }
                        }
                    }
                    else PaintIcon(g, colorHover, rect, false, true);
                }
            }
            else
            {
                bool has_left = loading || HasIcon, has_right = showArrow;
                Rectangle rect_text;
                if (has_left || has_right)
                {
                    if (has_left && has_right)
                    {
                        rect_text = RectAlignLR(g, textLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l, out var rect_r);

                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, LoadingValue * 360F);
                            }
                        }
                        else
                        {
                            PaintIcon(g, color, rect_l, true, true);
                            PaintIcon(g, colorHover, rect_l, true, true);
                        }

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                var rect_arrow = new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        rect_text = RectAlignL(g, textLine, textCenterHasIcon, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l);
                        if (loading)
                        {
                            float loading_size = rect_l.Height * .14F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, LoadingValue * 360F);
                            }
                        }
                        else
                        {
                            PaintIcon(g, color, rect_l, true, true);
                            PaintIcon(g, colorHover, rect_l, true, true);
                        }
                    }
                    else
                    {
                        rect_text = RectAlignR(g, textLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_r);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                int size_arrow = rect_r.Width / 2;
                                g.TranslateTransform(rect_r.X + size_arrow, rect_r.Y + size_arrow);
                                g.RotateTransform(-90F);
                                var rect_arrow = new Rectangle(-size_arrow, -size_arrow, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                    PaintTextAlign(rect_read, ref rect_text);
                }
                using (var brush = new SolidBrush(color))
                using (var brushHover = new SolidBrush(colorHover))
                {
                    g.String(text, Font, brush, rect_text, stringFormat);
                    g.String(text, Font, brushHover, rect_text, stringFormat);
                }
            }
        }

        internal static Rectangle RectAlignL(Canvas g, bool textLine, bool textCenter, Font font, TAlignMini iconPosition, float iconratio, float icongap, Size font_size, Rectangle rect_read, out Rectangle rect_l)
        {
            int font_Height = font_size.Height;
            if (textLine && (iconPosition == TAlignMini.Top || iconPosition == TAlignMini.Bottom)) font_Height = g.MeasureString(Config.NullText, font).Height;
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * icongap);
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
        internal static Rectangle RectAlignLR(Canvas g, bool textLine, Font font, TAlignMini iconPosition, float iconratio, float icongap, Size font_size, Rectangle rect_read, out Rectangle rect_l, out Rectangle rect_r)
        {
            int font_Height = font_size.Height;
            if (textLine && (iconPosition == TAlignMini.Top || iconPosition == TAlignMini.Bottom)) font_Height = g.MeasureString(Config.NullText, font).Height;
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * icongap), sps = (int)(font_size.Height * .4F);
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
        internal static Rectangle RectAlignR(Canvas g, bool textLine, Font font, TAlignMini iconPosition, float iconratio, float icongap, Size font_size, Rectangle rect_read, out Rectangle rect_r)
        {
            int font_Height = font_size.Height;
            if (textLine && (iconPosition == TAlignMini.Top || iconPosition == TAlignMini.Bottom)) font_Height = g.MeasureString(Config.NullText, font).Height;
            int icon_size = (int)(font_Height * iconratio), sp = (int)(font_Height * icongap), sps = (int)(font_size.Height * .4F), rsps = icon_size + sp;
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
                if (Enabled)
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
            if (iconSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(iconSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.Image(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (icon != null)
            {
                g.Image(icon, rect, opacity);
                return true;
            }
            return false;
        }

        #endregion

        public GraphicsPath Path(RectangleF rect_read, float _radius)
        {
            if (shape == TShape.Circle)
            {
                var path = new GraphicsPath();
                path.AddEllipse(rect_read);
                return path;
            }
            if (joinLeft && joinRight) return rect_read.RoundPath(0);
            else if (joinRight) return rect_read.RoundPath(_radius, true, false, false, true);
            else if (joinLeft) return rect_read.RoundPath(_radius, false, true, true, false);
            return rect_read.RoundPath(_radius);
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
                    if (borderWidth > 0) color = Style.Db.PrimaryHover;
                    else color = Style.Db.FillSecondary;
                    break;
                case TTypeMini.Success:
                    color = Style.Db.SuccessHover;
                    break;
                case TTypeMini.Error:
                    color = Style.Db.ErrorHover;
                    break;
                case TTypeMini.Info:
                    color = Style.Db.InfoHover;
                    break;
                case TTypeMini.Warn:
                    color = Style.Db.WarningHover;
                    break;
                case TTypeMini.Primary:
                default:
                    color = Style.Db.PrimaryHover;
                    break;
            }
            if (BackHover.HasValue) color = BackHover.Value;
            return color;
        }

        void GetDefaultColorConfig(out Color Fore, out Color Color, out Color backHover, out Color backActive)
        {
            Fore = Style.Db.DefaultColor;
            Color = Style.Db.Primary;
            if (borderWidth > 0)
            {
                backHover = Style.Db.PrimaryHover;
                backActive = Style.Db.PrimaryActive;
            }
            else
            {
                backHover = Style.Db.FillSecondary;
                backActive = Style.Db.Fill;
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
            if (loading)
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
                if (loading) Back = Color.FromArgb(165, Back);
                return;
            }
            GetColorConfig(type, out Fore, out Back, out backHover, out backActive);
            if (fore.HasValue) Fore = fore.Value;
            if (back.HasValue) Back = back.Value;
            if (BackHover.HasValue) backHover = BackHover.Value;
            if (BackActive.HasValue) backActive = BackActive.Value;
            if (AnimationBlinkState && colorBlink.HasValue) back = colorBlink.Value;
            if (loading) Back = Color.FromArgb(165, Back);
        }

        void GetColorConfig(TTypeMini type, out Color Fore, out Color Back, out Color backHover, out Color backActive)
        {
            switch (type)
            {
                case TTypeMini.Error:
                    Back = Style.Db.Error;
                    Fore = Style.Db.ErrorColor;
                    backHover = Style.Db.ErrorHover;
                    backActive = Style.Db.ErrorActive;
                    break;
                case TTypeMini.Success:
                    Back = Style.Db.Success;
                    Fore = Style.Db.SuccessColor;
                    backHover = Style.Db.SuccessHover;
                    backActive = Style.Db.SuccessActive;
                    break;
                case TTypeMini.Info:
                    Back = Style.Db.Info;
                    Fore = Style.Db.InfoColor;
                    backHover = Style.Db.InfoHover;
                    backActive = Style.Db.InfoActive;
                    break;
                case TTypeMini.Warn:
                    Back = Style.Db.Warning;
                    Fore = Style.Db.WarningColor;
                    backHover = Style.Db.WarningHover;
                    backActive = Style.Db.WarningActive;
                    break;
                case TTypeMini.Primary:
                default:
                    Back = Style.Db.Primary;
                    Fore = Style.Db.PrimaryColor;
                    backHover = Style.Db.PrimaryHover;
                    backActive = Style.Db.PrimaryActive;
                    break;
            }
        }

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, shape, joinLeft, joinRight);
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = (shape == TShape.Round || shape == TShape.Circle) ? rect_read.Height : radius * Config.Dpi;
                return Path(rect_read, _radius);
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
                    ExtraMouseHover = path.IsVisible(e.Location);
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
            if (CanClick(e.Location))
            {
                Focus();
                base.OnMouseDown(e);
                ExtraMouseDown = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (ExtraMouseDown)
            {
                if (CanClick(e.Location))
                {
                    base.OnMouseUp(e);
                    if (e.Button == MouseButtons.Left)
                    {
                        ClickAnimation();
                        OnClick(e);
                    }
                    OnMouseClick(e);
                }
                ExtraMouseDown = false;
            }
            else base.OnMouseUp(e);
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

        internal Size PSize
        {
            get
            {
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureString(text ?? Config.NullText, Font);
                    int gap = (int)(20 * Config.Dpi), wave = (int)(WaveSize * Config.Dpi);
                    if (Shape == TShape.Circle || string.IsNullOrEmpty(text))
                    {
                        int s = font_size.Height + wave + gap;
                        return new Size(s, s);
                    }
                    else
                    {
                        int m = wave * 2;
                        if (joinLeft || joinRight) m = 0;
                        bool has_icon = loading || HasIcon;
                        if (has_icon || showArrow)
                        {
                            if (has_icon && (IconPosition == TAlignMini.Top || IconPosition == TAlignMini.Bottom))
                            {
                                int size = (int)Math.Ceiling(font_size.Height * 1.2F);
                                return new Size(font_size.Width + m + gap + size, font_size.Height + wave + gap + size);
                            }
                            int height = font_size.Height + wave + gap;
                            if (has_icon && showArrow) return new Size(font_size.Width + m + gap + font_size.Height * 2, height);
                            else if (has_icon) return new Size(font_size.Width + m + gap + (int)Math.Ceiling(font_size.Height * 1.2F), height);
                            else return new Size(font_size.Width + m + gap + (int)Math.Ceiling(font_size.Height * .8F), height);
                        }
                        else return new Size(font_size.Width + m + gap, font_size.Height + wave + gap);
                    }
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        internal bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired)
            {
                bool flag = false;
                Invoke(new Action(() =>
                {
                    flag = BeforeAutoSize();
                }));
                return flag;
            }
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
            if (e.KeyCode is Keys.Enter or Keys.Space)
            {
                ClickAnimation();
                OnClick(EventArgs.Empty);
                e.Handled = true;
            }
            base.OnKeyUp(e);
        }

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        /// <summary>
        /// 是否默认按钮
        /// </summary>
        public void NotifyDefault(bool value)
        {

        }

        public void PerformClick()
        {
            ClickAnimation();
            OnClick(EventArgs.Empty);
        }

        bool CanClick()
        {
            if (loading) return false;
            else
            {
                if (RespondRealAreas)
                {
                    var e = PointToClient(MousePosition);
                    var rect_read = ReadRectangle;
                    using (var path = Path(rect_read, radius * Config.Dpi))
                    {
                        return path.IsVisible(e);
                    }
                }
                else return true;
            }
        }
        bool CanClick(Point e)
        {
            if (loading) return false;
            else
            {
                if (RespondRealAreas)
                {
                    var rect_read = ReadRectangle;
                    using (var path = Path(rect_read, radius * Config.Dpi))
                    {
                        return path.IsVisible(e);
                    }
                }
                else return true;
            }
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