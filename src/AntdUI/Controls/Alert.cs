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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Alert 警告提示
    /// </summary>
    /// <remarks>警告提示，展现需要关注的信息。</remarks>
    [Description("Alert 警告提示")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [Designer(typeof(IControlDesigner))]
    public class Alert : IControl
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

        float borWidth = 0F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(0F)]
        public float BorderWidth
        {
            get => borWidth;
            set
            {
                if (borWidth == value) return;
                borWidth = value;
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
                font_size = null;
                text = value;
                Invalidate();
            }
        }

        string? textTitle = null;
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"), Category("外观"), DefaultValue(null)]
        public string? TextTitle
        {
            get => textTitle;
            set
            {
                if (textTitle == value) return;
                textTitle = value;
                Invalidate();
            }
        }

        TType icon = TType.None;
        /// <summary>
        /// 样式
        /// </summary>
        [Description("样式"), Category("外观"), DefaultValue(TType.None)]
        public TType Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidate();
            }
        }

        bool loop = false;
        /// <summary>
        /// 文本轮播
        /// </summary>
        [Description("文本轮播"), Category("外观"), DefaultValue(false)]
        public bool Loop
        {
            get => loop;
            set
            {
                if (loop == value) return;
                loop = value;
                StartTask();
            }
        }

        #region 动画

        ITask? task = null;
        private void StartTask()
        {
            task?.Dispose();
            if (loop)
            {
                task = new ITask(this, () =>
                {
                    if (font_size.HasValue)
                    {
                        val += 1F;
                        if (val > font_size.Value.Width)
                        {
                            if (Width > font_size.Value.Width) val = 0F;
                            else val = -(Width - Padding.Horizontal);
                        }
                        Invalidate();
                    }
                    return loop;
                }, 10);
            }
        }

        #endregion

        #region 参数

        float val = 0;
        SizeF? font_size = null;

        #endregion

        /// <summary>
        /// 显示区域（容器）
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get => ClientRectangle.DeflateRect(Padding);
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = DisplayRectangle;
            var g = e.Graphics.High();
            if (icon == TType.None)
            {
                if (loop)
                {
                    if (font_size == null) font_size = g.MeasureString(text, Font);
                    g.SetClip(rect);
                    PaintText(g, rect, font_size.Value, ForeColor);
                    g.ResetClip();
                }
                else
                {
                    if (textTitle == null)
                    {
                        var size = g.MeasureString(text, Font);
                        font_size = size;
                        float icon_size = size.Height * 0.86F, gap = icon_size * 0.4F;

                        using (var brush = new SolidBrush(ForeColor))
                        {
                            var rect_txt = new RectangleF(rect.X + gap, rect.Y, rect.Width - gap * 2, rect.Height);
                            g.DrawString(text, Font, brush, rect_txt, stringLeft);
                        }
                    }
                    else
                    {
                        using (var font_title = new Font(Font.FontFamily, Font.Size * 1.14F, Font.Style))
                        {
                            var size_title = g.MeasureString(textTitle, font_title);
                            float icon_size = size_title.Height * 1.2F, gap = icon_size * 0.5F;

                            using (var brush = new SolidBrush(ForeColor))
                            {
                                var rect_txt = new RectangleF(rect.X + gap, rect.Y + gap, rect.Width - (gap * 2), size_title.Height);
                                g.DrawString(textTitle, font_title, brush, rect_txt, stringLeft);

                                var desc_y = rect_txt.Bottom + icon_size * 0.33F;
                                var rect_txt_desc = new RectangleF(rect_txt.X, desc_y, rect_txt.Width, rect.Height - (desc_y + gap));
                                g.DrawString(text, Font, brush, rect_txt_desc, stringLTEllipsis);
                            }
                        }
                    }
                }
            }
            else
            {
                float _radius = radius * Config.Dpi;
                Color back, bor_color, color = Style.Db.Text;
                switch (icon)
                {
                    case TType.Success:
                        back = Style.Db.SuccessBg;
                        bor_color = Style.Db.SuccessBorder;
                        break;
                    case TType.Info:
                        back = Style.Db.InfoBg;
                        bor_color = Style.Db.InfoBorder;
                        break;
                    case TType.Warn:
                        back = Style.Db.WarningBg;
                        bor_color = Style.Db.WarningBorder;
                        break;
                    case TType.Error:
                        back = Style.Db.ErrorBg;
                        bor_color = Style.Db.ErrorBorder;
                        break;
                    default:
                        back = Style.Db.SuccessBg;
                        bor_color = Style.Db.SuccessBorder;
                        break;
                }
                using (var path = rect.RoundPath(_radius))
                {
                    using (var brush = new SolidBrush(back))
                    {
                        g.FillPath(brush, path);
                    }
                    g.SetClip(path);
                    if (loop)
                    {
                        if (font_size == null) font_size = g.MeasureString(text, Font);
                        float icon_size = font_size.Value.Height * 0.86F, gap = icon_size * 0.4F;
                        var rect_icon = new RectangleF(gap, rect.Y + (rect.Height - icon_size) / 2F, icon_size, icon_size);
                        PaintText(g, rect, font_size.Value, color, back, _radius);
                        PaintIcon(g, rect_icon);
                    }
                    else
                    {
                        var size = g.MeasureString(text, Font);
                        font_size = size;
                        if (textTitle == null)
                        {
                            float icon_size = size.Height * 0.86F, gap = icon_size * 0.4F;

                            var rect_icon = new RectangleF(rect.X + gap, rect.Y + (rect.Height - icon_size) / 2F, icon_size, icon_size);
                            PaintIcon(g, rect_icon);
                            using (var brush = new SolidBrush(color))
                            {
                                var rect_txt = new RectangleF(rect_icon.X + rect_icon.Width + gap, rect.Y, rect.Width - (rect_icon.Width + gap * 2), rect.Height);
                                g.DrawString(text, Font, brush, rect_txt, stringLeft);
                            }
                        }
                        else
                        {
                            using (var font_title = new Font(Font.FontFamily, Font.Size * 1.14F, Font.Style))
                            {
                                var size_title = g.MeasureString(textTitle, font_title);
                                float icon_size = size_title.Height * 1.2F, gap = icon_size * 0.5F;

                                var rect_icon = new RectangleF(rect.X + gap, rect.Y + gap, icon_size, icon_size);
                                PaintIcon(g, rect_icon);

                                using (var brush = new SolidBrush(color))
                                {
                                    var rect_txt = new RectangleF(rect_icon.X + rect_icon.Width + icon_size / 2F, rect_icon.Y, rect.Width - (rect_icon.Width + gap * 2), rect_icon.Height);
                                    g.DrawString(textTitle, font_title, brush, rect_txt, stringLeft);

                                    var desc_y = rect_txt.Bottom + icon_size * 0.2F;
                                    var rect_txt_desc = new RectangleF(rect_txt.X, desc_y, rect_txt.Width, rect.Height - (desc_y + gap));
                                    g.DrawString(text, Font, brush, rect_txt_desc, stringLTEllipsis);
                                }
                            }
                        }
                    }

                    g.ResetClip();
                    if (borWidth > 0)
                    {
                        using (var brush_bor = new Pen(bor_color, borWidth * Config.Dpi))
                        {
                            g.DrawPath(brush_bor, path);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        readonly StringFormat stringLTEllipsis = Helper.SF_Ellipsis(tb: StringAlignment.Near, lr: StringAlignment.Near);
        readonly StringFormat stringCenter = Helper.SF_NoWrap();
        readonly StringFormat stringLeft = Helper.SF_ALL(lr: StringAlignment.Near);

        /// <summary>
        /// 绘制图标
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_icon">图标位置</param>
        void PaintIcon(Graphics g, RectangleF rect_icon)
        {
            switch (icon)
            {
                case TType.Success:
                    using (var brush = new SolidBrush(Style.Db.Success))
                    {
                        g.FillEllipse(brush, rect_icon);
                    }
                    g.PaintIconComplete(rect_icon, Style.Db.BgBase);
                    break;
                case TType.Info:
                    using (var brush = new SolidBrush(Style.Db.Info))
                    {
                        g.FillEllipse(brush, rect_icon);
                    }
                    g.PaintIconInfo(rect_icon, Style.Db.BgBase);
                    break;
                case TType.Warn:
                    using (var brush = new SolidBrush(Style.Db.Warning))
                    {
                        g.FillEllipse(brush, rect_icon);
                    }
                    g.PaintIconWarn(rect_icon, Style.Db.BgBase);
                    break;
                case TType.Error:
                    using (var brush = new SolidBrush(Style.Db.Error))
                    {
                        g.FillEllipse(brush, rect_icon);
                    }
                    g.PaintIconError(rect_icon, Style.Db.BgBase);
                    break;
            }
        }

        /// <summary>
        /// 渲染文字
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">区域</param>
        /// <param name="size">文字大小</param>
        /// <param name="fore">文字颜色</param>
        void PaintText(Graphics g, RectangleF rect, SizeF size, Color fore)
        {
            using (var brush = new SolidBrush(fore))
            {
                var rect_txt = new RectangleF(rect.X - val, rect.Y, size.Width, rect.Height);
                g.DrawString(text, Font, brush, rect_txt, stringCenter);
                if (rect.Width > size.Width)
                {
                    var maxw = rect.Width + rect_txt.Width / 2;
                    var rect_txt2 = new RectangleF(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                    while (rect_txt2.X < maxw)
                    {
                        g.DrawString(text, Font, brush, rect_txt2, stringCenter);
                        rect_txt2.X = rect_txt2.Right;
                    }
                }
            }
        }

        /// <summary>
        /// 渲染文字（渐变遮罩）
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">区域</param>
        /// <param name="size">文字大小</param>
        /// <param name="fore">文字颜色</param>
        /// <param name="back">背景颜色</param>
        void PaintText(Graphics g, RectangleF rect, SizeF size, Color fore, Color back, float radius)
        {
            using (var brush = new SolidBrush(fore))
            {
                var rect_txt = new RectangleF(rect.X - val, rect.Y, size.Width, rect.Height);
                g.DrawString(text, Font, brush, rect_txt, stringCenter);
                if (rect.Width > size.Width)
                {
                    var maxw = rect.Width + rect_txt.Width / 2;
                    var rect_txt2 = new RectangleF(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                    while (rect_txt2.X < maxw)
                    {
                        g.DrawString(text, Font, brush, rect_txt2, stringCenter);
                        rect_txt2.X = rect_txt2.Right;
                    }
                }
            }
            var rect_icon_l = new RectangleF(rect.X, rect.Y, size.Height * 2, rect.Height);
            using (var brush = new LinearGradientBrush(rect_icon_l, back, Color.Transparent, 0F))
            {
                rect_icon_l.Width -= 1F;
                g.FillRectangle(brush, rect_icon_l);
                g.FillRectangle(brush, rect_icon_l);
            }
            var rect_icon_r = new RectangleF(rect.Right - rect_icon_l.Width, rect_icon_l.Y, rect_icon_l.Width, rect_icon_l.Height);
            using (var brush = new LinearGradientBrush(rect_icon_r, Color.Transparent, back, 0F))
            {
                rect_icon_r.X += 1F;
                rect_icon_r.Width -= 1F;
                g.FillRectangle(brush, rect_icon_r);
                g.FillRectangle(brush, rect_icon_r);
            }
        }

        #endregion

        public override Rectangle ReadRectangle
        {
            get => DisplayRectangle;
        }

        public override GraphicsPath RenderRegion
        {
            get => DisplayRectangle.RoundPath(radius * Config.Dpi);
        }

        #endregion

        #region 事件

        protected override void OnMouseEnter(EventArgs e)
        {
            task?.Dispose();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (loop) StartTask();
            base.OnMouseLeave(e);
        }

        #endregion
    }
}