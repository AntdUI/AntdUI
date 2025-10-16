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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Panel 面板
    /// </summary>
    /// <remarks>内容区域。</remarks>
    [Description("Panel 面板")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [Designer(typeof(IControlDesigner))]
    public class Panel : IControl, ShadowConfig, IMessageFilter, IEventListener
    {
        #region 属性

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
                Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        TAlignRound radiusAlign = TAlignRound.ALL;
        [Description("圆角方向"), Category("外观"), DefaultValue(TAlignRound.ALL)]
        public TAlignRound RadiusAlign
        {
            get => radiusAlign;
            set
            {
                if (radiusAlign == value) return;
                radiusAlign = value;
                Invalidate();
                OnPropertyChanged(nameof(RadiusAlign));
            }
        }

        #region 阴影

        Padding _padding = new Padding(0);
        [Description("内边距"), Category("外观"), DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public Padding padding
        {
            get => _padding;
            set
            {
                if (_padding == value) return;
                _padding = value;
                shadow_temp?.Dispose();
                shadow_temp = null;
                IOnSizeChanged();
                OnPropertyChanged(nameof(padding));
            }
        }

        int shadow = 0;
        /// <summary>
        /// 阴影大小
        /// </summary>
        [Description("阴影"), Category("外观"), DefaultValue(0)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                shadow_temp?.Dispose();
                shadow_temp = null;
                IOnSizeChanged();
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
                IOnSizeChanged();
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
                IOnSizeChanged();
                OnPropertyChanged(nameof(ShadowOffsetY));
            }
        }

        float shadowOpacity = 0.1F;
        /// <summary>
        /// 阴影透明度
        /// </summary>
        [Description("阴影透明度"), Category("阴影"), DefaultValue(0.1F)]
        public float ShadowOpacity
        {
            get => shadowOpacity;
            set
            {
                if (shadowOpacity == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                shadowOpacity = value;
                AnimationHoverValue = shadowOpacity;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOpacity));
            }
        }

        bool shadowOpacityAnimation = false;
        /// <summary>
        /// 阴影透明度动画使能
        /// </summary>
        [Description("阴影透明度动画使能"), Category("阴影"), DefaultValue(false)]
        public bool ShadowOpacityAnimation
        {
            get => shadowOpacityAnimation;
            set
            {
                if (shadowOpacityAnimation == value) return;
                shadowOpacityAnimation = value;
                if (shadowOpacityAnimation)
                {
                    if (IsHandleCreated) Application.AddMessageFilter(this);
                }
                else Application.RemoveMessageFilter(this);
            }
        }

        float shadowOpacityHover = 0.3F;
        /// <summary>
        /// 悬停阴影后透明度
        /// </summary>
        [Description("悬停阴影后透明度"), Category("阴影"), DefaultValue(0.3F)]
        public float ShadowOpacityHover
        {
            get => shadowOpacityHover;
            set
            {
                if (shadowOpacityHover == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                shadowOpacityHover = value;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOpacityHover));
            }
        }

        TAlignMini shadowAlign = TAlignMini.None;
        /// <summary>
        /// 阴影方向
        /// </summary>
        [Description("阴影方向"), Category("阴影"), DefaultValue(TAlignMini.None)]
        public TAlignMini ShadowAlign
        {
            get => shadowAlign;
            set
            {
                if (shadowAlign == value) return;
                shadowAlign = value;
                shadow_temp?.Dispose();
                shadow_temp = null;
                IOnSizeChanged();
                OnPropertyChanged(nameof(ShadowAlign));
            }
        }

        #endregion

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

        #region 箭头

        int arrwoSize = 8;
        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("箭头"), DefaultValue(8)]
        public int ArrowSize
        {
            get => arrwoSize;
            set
            {
                if (arrwoSize == value) return;
                arrwoSize = value;
                Invalidate();
                OnPropertyChanged(nameof(ArrowSize));
            }
        }

        TAlign arrowAlign = TAlign.None;
        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("箭头"), DefaultValue(TAlign.None)]
        public TAlign ArrowAlign
        {
            get => arrowAlign;
            set
            {
                if (arrowAlign == value) return;
                arrowAlign = value;
                Invalidate();
                OnPropertyChanged(nameof(ArrowAlign));
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

        public override Rectangle DisplayRectangle => ClientRectangle.DeflateRect(Padding, this, shadowAlign, borderWidth);

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect_read = ReadRectangle;
            float _radius = radius * Config.Dpi;
            using (var path = DrawShadow(g, _radius, e.Rect, rect_read))
            {
                using (var brush = backExtend.BrushEx(rect_read, back ?? Colour.BgContainer.Get(nameof(Panel), ColorScheme)))
                {
                    g.Fill(brush, path);
                }
                if (backImage != null) g.Image(rect_read, backImage, backFit, _radius, false);
                if (borderWidth > 0) g.Draw(borderColor ?? Colour.BorderColor.Get(nameof(Panel), ColorScheme), borderWidth * Config.Dpi, borderStyle, path);
            }
            if (ArrowAlign != TAlign.None) g.FillPolygon(back ?? Colour.BgContainer.Get(nameof(Panel), ColorScheme), ArrowAlign.AlignLines(ArrowSize, e.Rect, rect_read));
            base.OnDraw(e);
        }

        Bitmap? shadow_temp;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="rect_read">真实区域</param>
        GraphicsPath DrawShadow(Canvas g, float radius, Rectangle rect_client, Rectangle rect_read)
        {
            var path = rect_read.RoundPath(radius, shadowAlign, radiusAlign);
            if (shadow > 0)
            {
                int shadow = (int)(Shadow * Config.Dpi), shadowOffsetX = (int)(ShadowOffsetX * Config.Dpi), shadowOffsetY = (int)(ShadowOffsetY * Config.Dpi);
                if (shadow_temp == null || shadow_temp.PixelFormat == PixelFormat.DontCare || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
                {
                    shadow_temp?.Dispose();
                    shadow_temp = path.PaintShadowO(rect_client.Width, rect_client.Height, shadowColor ?? Colour.TextBase.Get(nameof(Panel), ColorScheme), shadow);
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix();
                    if (AnimationHover) matrix.Matrix33 = AnimationHoverValue;
                    else if (ExtraMouseHover) matrix.Matrix33 = shadowOpacityHover;
                    else matrix.Matrix33 = shadowOpacity;
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.Image(shadow_temp, new Rectangle(rect_client.X + shadowOffsetX, rect_client.Y + shadowOffsetY, rect_client.Width, rect_client.Height), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return path;
        }

        public override Rectangle ReadRectangle => ClientRectangle.DeflateRect(_padding).PaddingRect(this, shadowAlign, borderWidth / 2F);

        public override GraphicsPath RenderRegion => ReadRectangle.RoundPath(radius * Config.Dpi);

        #endregion

        #region 鼠标

        float AnimationHoverValue = 0.1F;
        bool AnimationHover = false;
        bool _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                if (Enabled && ShadowOpacityAnimation && shadow > 0 && shadowOpacityHover > 0 && shadowOpacityHover > shadowOpacity)
                {
                    if (Config.HasAnimation(nameof(Panel)))
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        float addvalue = shadowOpacityHover / 12F;
                        if (value)
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(addvalue);
                                if (AnimationHoverValue >= shadowOpacityHover) { AnimationHoverValue = shadowOpacityHover; return false; }
                                Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                        else
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(-addvalue);
                                if (AnimationHoverValue <= shadowOpacity) { AnimationHoverValue = shadowOpacity; return false; }
                                Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                    }
                    else Invalidate();
                }
            }
        }

        public void SetMouseHover(bool val) { ExtraMouseHover = val; }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadHover?.Dispose();
            ThreadHover = null;
            ShadowOpacityAnimation = false;
            shadow_temp?.Dispose();
            shadow_temp = null;
            base.Dispose(disposing);
        }
        ITask? ThreadHover;

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ExtraMouseHover = true;
        }

        #region 离开监控


        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x2a1 || m.Msg == 0x2a3)
            {
                ExtraMouseHover = ClientRectangle.Contains(PointToClient(MousePosition));
                return false;
            }
            return false;
        }

        #endregion

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ExtraMouseHover = false;
        }

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
            if (ShadowOpacityAnimation) Application.AddMessageFilter(this);
        }

        #region 主题变化

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.THEME:
                    shadow_temp?.Dispose();
                    shadow_temp = null;
                    break;
            }
        }

        #endregion
    }

    internal class IControlDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {

    }
}