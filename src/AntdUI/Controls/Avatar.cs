﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Avatar 头像
    /// </summary>
    /// <remarks>用来代表用户或事物，支持图片、图标或字符展示。</remarks>
    [Description("Avatar 头像")]
    [ToolboxItem(true)]
    [DefaultProperty("Image")]
    public class Avatar : IControl, ShadowConfig
    {
        #region 属性

        Color back = Color.Transparent;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(typeof(Color), "Transparent")]
        public Color Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public new string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                if (value != null && value.Length > 1) value = value.Substring(0, 1);
                text = value;
                Invalidate();
            }
        }

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
            }
        }

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                Invalidate();
            }
        }

        #region 图片

        Image? image = null;
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片"), Category("外观"), DefaultValue(null)]
        public Image? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                Invalidate();
            }
        }

        TFit imageFit = TFit.Cover;
        /// <summary>
        /// 图片布局
        /// </summary>
        [Description("图片布局"), Category("外观"), DefaultValue(TFit.Cover)]
        public TFit ImageFit
        {
            get => imageFit;
            set
            {
                if (imageFit == value) return;
                imageFit = value;
                Invalidate();
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
                Invalidate();
            }
        }

        Color borColor = Color.FromArgb(246, 248, 250);
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(typeof(Color), "246, 248, 250")]
        public Color BorderColor
        {
            get => borColor;
            set
            {
                if (borColor == value) return;
                borColor = value;
                if (borderWidth > 0) Invalidate();
            }
        }

        #endregion

        #region 阴影

        int shadow = 0;
        [Description("阴影大小"), Category("阴影"), DefaultValue(0)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                Invalidate();
            }
        }

        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        public Color? ShadowColor { get; set; }

        float shadowOpacity = 0.3F;
        [Description("阴影透明度"), Category("阴影"), DefaultValue(0.3F)]
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
            }
        }

        int shadowOffsetX = 0;
        [Description("阴影偏移X"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetX
        {
            get => shadowOffsetX;
            set
            {
                if (shadowOffsetX == value) return;
                shadowOffsetX = value;
                Invalidate();
            }
        }

        int shadowOffsetY = 0;
        [Description("阴影偏移Y"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetY
        {
            get => shadowOffsetY;
            set
            {
                if (shadowOffsetY == value) return;
                shadowOffsetY = value;
                Invalidate();
            }
        }

        #endregion

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var _rect = ClientRectangle;
            if (_rect.Width == 0 || _rect.Height == 0) return;
            var g = e.Graphics.High();
            float _radius = radius * Config.Dpi;
            if (borderWidth > 0)
            {
                var rect = ReadRectangle;
                if (shadow > 0 && shadowOpacity > 0) g.PaintShadow(this, _rect, rect, _radius, round);
                FillRect(g, rect, back, _radius, round);
                if (image != null) g.PaintImg(rect, image, imageFit, _radius, round);
                else PaintText(g, text, rect, stringCenter, Enabled);
                DrawRect(g, rect, borColor, borderWidth * Config.Dpi, _radius, round);
            }
            else
            {
                var rect = ReadRectangle;
                if (shadow > 0 && shadowOpacity > 0) g.PaintShadow(this, _rect, rect, _radius, round);
                FillRect(g, rect, back, _radius, round);
                if (image != null) g.PaintImg(rect, image, imageFit, _radius, round);
                else PaintText(g, text, rect, stringCenter, Enabled);
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        readonly StringFormat stringCenter = Helper.SF_ALL();

        void FillRect(Graphics g, Rectangle rect, Color color, float radius, bool round)
        {
            using (var brush = new SolidBrush(color))
            {
                if (round)
                {
                    g.FillEllipse(brush, rect);
                }
                else if (radius > 0)
                {
                    using (var path = rect.RoundPath(radius))
                    {
                        g.FillPath(brush, path);
                    }
                }
                else
                {
                    g.FillRectangle(brush, rect);
                }
            }
        }

        void DrawRect(Graphics g, Rectangle rect, Color color, float width, float radius, bool round)
        {
            using (var pen = new Pen(color, width))
            {
                if (round)
                {
                    g.DrawEllipse(pen, rect);
                }
                else if (radius > 0)
                {
                    using (var path = rect.RoundPath(radius))
                    {
                        g.DrawPath(pen, path);
                    }
                }
                else
                {
#if NET40 || NET46 || NET48 || NET6_0
                    g.DrawRectangles(pen, new RectangleF[] { rect });
#else
                    g.DrawRectangle(pen, rect);
#endif
                }
            }
        }

        #endregion

        public override Rectangle ReadRectangle
        {
            get
            {
                if (borderWidth > 0) return ClientRectangle.PaddingRect(Padding, borderWidth * Config.Dpi / 2F);
                else return ClientRectangle.PaddingRect(Padding);
            }
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                if (borderWidth > 0)
                {
                    var rect = ReadRectangle;
                    if (round)
                    {
                        var path = new GraphicsPath();
                        path.AddEllipse(rect);
                        return path;
                    }
                    else if (radius > 0) return rect.RoundPath(radius * Config.Dpi);
                    else
                    {
                        var path = new GraphicsPath();
                        path.AddRectangle(rect);
                        return path;
                    }
                }
                else
                {
                    var rect = ReadRectangle;
                    if (round)
                    {
                        var path = new GraphicsPath();
                        path.AddEllipse(rect);
                        return path;
                    }
                    else if (radius > 0) return rect.RoundPath(radius * Config.Dpi);
                    else
                    {
                        var path = new GraphicsPath();
                        path.AddRectangle(rect);
                        return path;
                    }
                }
            }
        }

        #endregion
    }
}