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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Signal 信号强度
    /// </summary>
    /// <remarks>展示设备信号。</remarks>
    [Description("Signal 信号强度")]
    [ToolboxItem(true)]
    public class Signal : IControl
    {
        #region 属性

        Color? fill = null;
        /// <summary>
        /// 填充颜色
        /// </summary>
        [Description("填充颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 满格颜色
        /// </summary>
        [Description("满格颜色"), Category("外观"), DefaultValue(null)]
        public Color? FillFully { get; set; }

        /// <summary>
        /// 警告颜色
        /// </summary>
        [Description("警告颜色"), Category("外观"), DefaultValue(null)]
        public Color? FillWarn { get; set; }

        /// <summary>
        /// 危险颜色
        /// </summary>
        [Description("危险颜色"), Category("外观"), DefaultValue(null)]
        public Color? FillDanger { get; set; }

        int vol = 0;
        /// <summary>
        /// 信号强度
        /// </summary>
        [Description("信号强度"), Category("外观"), DefaultValue(0)]
        public int Value
        {
            get => vol;
            set
            {
                if (value < 0) value = 0;
                else if (value > 5) value = 5;
                if (vol == value) return;
                vol = value;
                Invalidate();
            }
        }

        int loading_vol = 0;
        bool loading = false;
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
                ThreadLoading?.Dispose();
                if (loading)
                {
                    loading_vol = 0;
                    bool add = true;
                    ThreadLoading = new ITask(this, () =>
                    {
                        if (add)
                        {
                            loading_vol += 10;
                            if (loading_vol == 100) add = false;
                        }
                        else
                        {
                            loading_vol -= 10;
                            if (loading_vol == 0) add = true;
                        }
                        Invalidate();
                        return loading;
                    }, 80, () =>
                    {
                        Invalidate();
                    });
                }
                else Invalidate();
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadLoading?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadLoading = null;

        #endregion

        #region 线样式

        bool styleLine = false;
        /// <summary>
        /// 启用线样式
        /// </summary>
        [Description("启用线样式"), Category("外观"), DefaultValue(false)]
        public bool StyleLine
        {
            get => styleLine;
            set
            {
                if (styleLine == value) return;
                styleLine = value;
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
            var rect = _rect.PaddingRect(Padding);
            var g = e.Graphics.High();
            int dot_size = rect.Width > rect.Height ? rect.Height : rect.Width;
            var rect_dot = new Rectangle(rect.X + (rect.Width - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
            if (styleLine)
            {
                int gap = (int)(dot_size * .12F), gap2 = gap * 2;
                float onew = (dot_size - gap * 4F) / 5F;

                int h1 = gap2 * 4, h2 = gap2 * 3, h3 = gap2 * 2, h4 = gap2;

                RectangleF rect_1 = new RectangleF(rect_dot.X, rect_dot.Y + h1, onew, rect_dot.Height - h1),
                    rect_2 = new RectangleF(rect_dot.X + gap + onew, rect_dot.Y + h2, onew, rect_dot.Height - h2),
                    rect_3 = new RectangleF(rect_dot.X + (gap + onew) * 2, rect_dot.Y + h3, onew, rect_dot.Height - h3),
                    rect_4 = new RectangleF(rect_dot.X + (gap + onew) * 3, rect_dot.Y + h4, onew, rect_dot.Height - h4),
                    rect_5 = new RectangleF(rect_dot.X + (gap + onew) * 4, rect_dot.Y, onew, rect_dot.Height);
                if (loading)
                {
                    Color color1 = fill ?? Style.Db.FillQuaternary, color2 = FillFully ?? Style.Db.Success;
                    using (var path = new GraphicsPath())
                    {
                        path.AddRectangle(rect_1);
                        path.AddRectangle(rect_2);
                        path.AddRectangle(rect_3);
                        path.AddRectangle(rect_4);
                        path.AddRectangle(rect_5);

                        using (var brush = new LinearGradientBrush(rect_dot, color2, color1, 0F))
                        {
                            brush.InterpolationColors = new ColorBlend(3)
                            {
                                Colors = new Color[] { color2, color2, color1 },
                                Positions = new float[] { 0, loading_vol / 100F, 1F }
                            };
                            g.Fill(brush, path);
                            g.Draw(color2, Config.Dpi, path);
                        }
                    }
                }
                else
                {
                    if (vol == 0)
                    {
                        using (var brush_bg = new SolidBrush(fill ?? Style.Db.FillQuaternary))
                        {
                            g.Fill(brush_bg, rect_1);
                            g.Fill(brush_bg, rect_2);
                            g.Fill(brush_bg, rect_3);
                            g.Fill(brush_bg, rect_4);
                            g.Fill(brush_bg, rect_5);
                        }
                    }
                    else if (vol == 1)
                    {
                        using (var brush_bg = new SolidBrush(fill ?? Style.Db.FillQuaternary))
                        using (var brush = new SolidBrush(FillDanger ?? Style.Db.Error))
                        {
                            g.Fill(brush, rect_1);
                            g.Fill(brush_bg, rect_2);
                            g.Fill(brush_bg, rect_3);
                            g.Fill(brush_bg, rect_4);
                            g.Fill(brush_bg, rect_5);
                        }
                    }
                    else if (vol == 2)
                    {
                        using (var brush_bg = new SolidBrush(fill ?? Style.Db.FillQuaternary))
                        using (var brush = new SolidBrush(FillDanger ?? Style.Db.Error))
                        {
                            g.Fill(brush, rect_1);
                            g.Fill(brush, rect_2);
                            g.Fill(brush_bg, rect_3);
                            g.Fill(brush_bg, rect_4);
                            g.Fill(brush_bg, rect_5);
                        }
                    }
                    else if (vol == 3)
                    {
                        using (var brush_bg = new SolidBrush(fill ?? Style.Db.FillQuaternary))
                        using (var brush = new SolidBrush(FillWarn ?? Style.Db.Warning))
                        {
                            g.Fill(brush, rect_1);
                            g.Fill(brush, rect_2);
                            g.Fill(brush, rect_3);
                            g.Fill(brush_bg, rect_4);
                            g.Fill(brush_bg, rect_5);
                        }
                    }
                    else if (vol == 4)
                    {
                        using (var brush_bg = new SolidBrush(fill ?? Style.Db.FillQuaternary))
                        using (var brush = new SolidBrush(FillFully ?? Style.Db.Success))
                        {
                            g.Fill(brush, rect_1);
                            g.Fill(brush, rect_2);
                            g.Fill(brush, rect_3);
                            g.Fill(brush, rect_4);
                            g.Fill(brush_bg, rect_5);
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(FillFully ?? Style.Db.Success))
                        {
                            g.Fill(brush, rect_1);
                            g.Fill(brush, rect_2);
                            g.Fill(brush, rect_3);
                            g.Fill(brush, rect_4);
                            g.Fill(brush, rect_5);
                        }
                    }
                }
            }
            else
            {
                var rect_pie = new Rectangle(rect_dot.X, rect_dot.Y + rect_dot.Height / 2 / 2, rect_dot.Width, rect_dot.Height);

                if (loading)
                {
                    Color color1 = fill ?? Style.Db.FillQuaternary, color2 = FillFully ?? Style.Db.Success;
                    using (var pen = new Pen(color2, Config.Dpi))
                    using (var brush = new LinearGradientBrush(rect_pie, color1, color2, 90F))
                    {
                        brush.InterpolationColors = new ColorBlend(2)
                        {
                            Colors = new Color[] { color1, color2, color2 },
                            Positions = new float[] { 0, loading_vol / 100F, 1F }
                        };
                        g.FillPie(brush, rect_pie, -135, 90);
                        g.DrawPie(pen, rect_pie, -135, 90);
                    }
                }
                else
                {
                    int onew = (int)(dot_size * .12F);

                    float size1 = rect_dot.Width - onew,
                        size2 = size1 - onew * 3F,
                        size3 = size2 - onew * 2F;

                    float y1 = rect_pie.Y + onew / 2F,
                        y2 = y1 + onew * 1.5F,
                        y3 = y2 + onew;

                    RectangleF rect_1 = new RectangleF(rect_dot.X + (rect_dot.Width - size1) / 2F, y1, size1, size1),
                        rect_2 = new RectangleF(rect_dot.X + (rect_dot.Width - size2) / 2F, y2, size2, size2),
                        rect_3 = new RectangleF(rect_dot.X + (rect_dot.Width - size3) / 2F, y3, size3, size3);
                    if (vol == 0)
                    {
                        using (var pen = new Pen(fill ?? Style.Db.FillQuaternary, onew))
                        using (var brush = new SolidBrush(pen.Color))
                        {
                            g.DrawArc(pen, rect_1, -135, 90);
                            g.DrawArc(pen, rect_2, -135, 90);
                            g.FillPie(brush, rect_3.X, rect_3.Y, rect_3.Width, rect_3.Height, -135, 90);
                        }
                    }
                    else if (vol == 1)
                    {
                        using (var pen = new Pen(fill ?? Style.Db.FillQuaternary, onew))
                        using (var brush = new SolidBrush(FillDanger ?? Style.Db.Error))
                        {
                            g.DrawArc(pen, rect_1, -135, 90);
                            g.DrawArc(pen, rect_2, -135, 90);
                            g.FillPie(brush, rect_3.X, rect_3.Y, rect_3.Width, rect_3.Height, -135, 90);
                        }
                    }
                    else if (vol == 2)
                    {
                        using (var pen = new Pen(fill ?? Style.Db.FillQuaternary, onew))
                        using (var brush = new SolidBrush(FillWarn ?? Style.Db.Warning))
                        using (var penw = new Pen(brush.Color, onew))
                        {
                            g.DrawArc(pen, rect_1, -135, 90);
                            g.DrawArc(pen, rect_2, -135, 90);
                            g.FillPie(brush, rect_3.X, rect_3.Y, rect_3.Width, rect_3.Height, -135, 90);
                        }
                    }
                    else if (vol == 3)
                    {
                        using (var pen = new Pen(fill ?? Style.Db.FillQuaternary, onew))
                        using (var brush = new SolidBrush(FillWarn ?? Style.Db.Warning))
                        using (var penw = new Pen(brush.Color, onew))
                        {
                            g.DrawArc(pen, rect_1, -135, 90);
                            g.DrawArc(penw, rect_2, -135, 90);
                            g.FillPie(brush, rect_3.X, rect_3.Y, rect_3.Width, rect_3.Height, -135, 90);
                        }
                    }
                    else if (vol == 4)
                    {
                        using (var pen = new Pen(FillFully ?? Style.Db.Success, onew))
                        using (var brush = new SolidBrush(pen.Color))
                        {
                            g.DrawArc(pen, rect_1, -135, 90);
                            g.DrawArc(pen, rect_2, -135, 90);
                            g.FillPie(brush, rect_3.X, rect_3.Y, rect_3.Width, rect_3.Height, -135, 90);
                        }
                    }
                    else
                    {
                        using (var pen = new Pen(FillFully ?? Style.Db.SuccessActive, onew))
                        using (var brush = new SolidBrush(pen.Color))
                        {
                            g.DrawArc(pen, rect_1, -135, 90);
                            g.DrawArc(pen, rect_2, -135, 90);
                            g.FillPie(brush, rect_3.X, rect_3.Y, rect_3.Width, rect_3.Height, -135, 90);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #endregion
    }
}