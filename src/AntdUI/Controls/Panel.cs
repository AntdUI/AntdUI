// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
            }
        }

        #region 阴影

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
                OnSizeChanged(EventArgs.Empty);
            }
        }

        Color? shadowColor;
        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
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
                OnSizeChanged(EventArgs.Empty);
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
                OnSizeChanged(EventArgs.Empty);
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
            }
        }

        #endregion

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        #region 箭头

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("箭头"), DefaultValue(8)]
        public int ArrowSize { get; set; } = 8;

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
                OnSizeChanged(EventArgs.Empty);
            }
        }

        Color? borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
            }
        }

        #endregion

        #endregion

        public override Rectangle DisplayRectangle
        {
            get => ClientRectangle.DeflateRect(Padding, this, borderWidth * Config.Dpi);
        }

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            Color _back = back.HasValue ? back.Value : Style.Db.BgContainer;
            using (var brush = new SolidBrush(_back))
            {
                using (var path = DrawShadow(g, rect, rect_read))
                {
                    g.FillPath(brush, path);
                    if (borderWidth > 0)
                    {
                        using (var brush_bor = new Pen(borderColor.HasValue ? borderColor.Value : Style.Db.BorderColor, borderWidth * Config.Dpi))
                        {
                            g.DrawPath(brush_bor, path);
                        }
                    }
                }
                if (ArrowAlign != TAlign.None) g.FillPolygon(brush, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="rect_read">真实区域</param>
        GraphicsPath DrawShadow(Graphics g, Rectangle rect_client, RectangleF rect_read)
        {
            var path = rect_read.RoundPath(radius * Config.Dpi);
            if (shadow > 0)
            {
                int shadow = (int)(Shadow * Config.Dpi), shadowOffsetX = (int)(ShadowOffsetX * Config.Dpi), shadowOffsetY = (int)(ShadowOffsetY * Config.Dpi);
                if (shadow_temp == null || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
                {
                    shadow_temp?.Dispose();
                    shadow_temp = path.PaintShadow(rect_client.Width, rect_client.Height, shadowColor.HasValue ? shadowColor.Value : Style.Db.TextBase, shadow);
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix();
                    if (AnimationHover) matrix.Matrix33 = AnimationHoverValue;
                    else if (ExtraMouseHover) matrix.Matrix33 = shadowOpacityHover;
                    else matrix.Matrix33 = shadowOpacity;
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(shadow_temp, new Rectangle(rect_client.X + shadowOffsetX, rect_client.Y + shadowOffsetY, rect_client.Width, rect_client.Height), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return path;
        }

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(this, borderWidth * Config.Dpi);
        }

        public override GraphicsPath RenderRegion
        {
            get => ReadRectangle.RoundPath(radius * Config.Dpi);
        }

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
                if (Enabled)
                {
                    if (Config.Animation && shadow > 0 && shadowOpacityHover > 0 && shadowOpacityHover > shadowOpacity)
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
            Application.RemoveMessageFilter(this);
            EventManager.Instance.RemoveListener(1, this);
            shadow_temp?.Dispose();
            shadow_temp = null;
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;

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

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Application.AddMessageFilter(this);
            EventManager.Instance.AddListener(1, this);
        }

        #region 主题变化

        public void HandleEvent(int eventId, IEventArgs? args)
        {
            switch (eventId)
            {
                case 1:
                    shadow_temp?.Dispose();
                    shadow_temp = null;
                    break;
            }
        }

        #endregion
    }

    internal class IControlDesigner : ParentControlDesigner
    {

    }
}