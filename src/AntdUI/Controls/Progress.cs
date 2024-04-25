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
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Progress 进度条
    /// </summary>
    /// <remarks>展示操作的当前进度。</remarks>
    [Description("Progress 进度条")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    public class Progress : IControl
    {
        #region 属性

        #region 背景

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

        Color? fill;
        /// <summary>
        /// 进度条颜色
        /// </summary>
        [Description("进度条颜色"), Category("外观"), DefaultValue(null)]
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

        #endregion

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

        /// <summary>
        /// MINI模式
        /// </summary>
        [Description("MINI模式"), Category("外观"), DefaultValue(false)]
        public bool Mini { get; set; } = false;

        TShape shape = TShape.Round;
        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category("外观"), DefaultValue(TShape.Round)]
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
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
                text = value;
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

        #region 进度条

        float _value = 0F;
        /// <summary>
        /// 进度条
        /// </summary>
        [Description("进度条 0F-1F"), Category("数据"), DefaultValue(0F)]
        public float Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                Invalidate();
            }
        }

        #endregion

        bool loading = false;
        float AnimationLoadingValue = 0F;
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
                    ThreadLoading = new ITask(this, () =>
                    {
                        AnimationLoadingValue = AnimationLoadingValue.Calculate(0.01F);
                        if (AnimationLoadingValue > 1)
                        {
                            AnimationLoadingValue = 0;
                            Invalidate();
                            Thread.Sleep(1000);
                        }
                        Invalidate();
                        return loading;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                }
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

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle.PaddingRect(Padding);
            var g = e.Graphics.High();
            bool enabled = Enabled;
            Color _color = fill.HasValue ? fill.Value : Style.Db.Primary, _back = back.HasValue ? back.Value : Style.Db.FillSecondary;
            if (Mini)
            {
                var font_size = g.MeasureString(text, Font);
                rect.IconRectL(font_size, out var icon_rect, out var text_rect, .7F);
                PaintText(g, text, new RectangleF(text_rect.X + 8, text_rect.Y, text_rect.Width - 8, text_rect.Height), Helper.stringFormatLeft, enabled);

                float w = radius * Config.Dpi;
                using (var brush = new Pen(_back, w))
                {
                    g.DrawEllipse(brush, icon_rect);
                }
                if (_value > 0)
                {
                    float max = 360F * _value;
                    using (var brush = new Pen(_color, w))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, icon_rect, -90, max);
                    }

                    if (loading && AnimationLoadingValue > 0)
                    {
                        int a = (int)(60 * (1f - AnimationLoadingValue));
                        using (var brush = new Pen(Color.FromArgb(a, Style.Db.BgBase), w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, icon_rect, -90, max * AnimationLoadingValue);
                        }
                    }
                }
            }
            else
            {
                if (shape == TShape.Circle)
                {
                    float w = radius * Config.Dpi;
                    float prog_size;
                    if (rect.Width == rect.Height) prog_size = rect.Width - w;
                    else if (rect.Width > rect.Height) prog_size = rect.Height - w;
                    else prog_size = rect.Width - w;
                    var rect_prog = new RectangleF(rect.X + (rect.Width - prog_size) / 2, rect.Y + (rect.Height - prog_size) / 2, prog_size, prog_size);
                    using (var brush = new Pen(_back, w))
                    {
                        g.DrawEllipse(brush, rect_prog);
                    }
                    if (_value > 0)
                    {
                        if (icon != TType.None)
                        {
                            var size = rect_prog.Width * 0.5F;
                            var rect_icon = new RectangleF(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size);
                            switch (icon)
                            {
                                case TType.Success:
                                    g.PaintIconComplete(rect_icon, _color);
                                    break;
                                case TType.Info:
                                    g.PaintIconInfo(rect_icon, _color);
                                    break;
                                case TType.Warn:
                                    g.PaintIconWarn(rect_icon, _color);
                                    break;
                                case TType.Error:
                                    g.PaintIconError(rect_icon, _color);
                                    break;
                            }
                        }
                        float max = 360F * _value;
                        using (var brush = new Pen(_color, w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, rect_prog, -90, max);
                        }

                        if (loading && AnimationLoadingValue > 0)
                        {
                            int a = (int)(60 * (1f - AnimationLoadingValue));
                            using (var brush = new Pen(Color.FromArgb(a, Style.Db.BgBase), w))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_prog, -90, max * AnimationLoadingValue);
                            }
                        }
                    }
                }
                else
                {
                    float _radius = radius * Config.Dpi;
                    if (shape == TShape.Round) _radius = rect.Height;

                    using (var path = rect.RoundPath(_radius))
                    {
                        using (var brush = new SolidBrush(_back))
                        {
                            g.FillPath(brush, path);
                        }
                        if (_value > 0)
                        {
                            var _w = rect.Width * _value;
                            if (_w > _radius)
                            {
                                using (var path_prog = new RectangleF(rect.X, rect.Y, _w, rect.Height).RoundPath(_radius))
                                {
                                    using (var brush = new SolidBrush(_color))
                                    {
                                        g.FillPath(brush, path_prog);
                                    }
                                }

                                if (loading && AnimationLoadingValue > 0)
                                {
                                    int a = (int)(60 * (1f - AnimationLoadingValue));
                                    using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.BgBase)))
                                    {
                                        using (var path_prog = new RectangleF(rect.X, rect.Y, _w * AnimationLoadingValue, rect.Height).RoundPath(_radius))
                                        {
                                            g.FillPath(brush, path_prog);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                using (var brush = new SolidBrush(_color))
                                {
                                    g.FillEllipse(brush, new RectangleF(rect.X, rect.Y, _w, rect.Height));
                                }
                            }
                        }
                    }
                }
                PaintText(g, text, rect, Helper.stringFormatCenter, enabled);
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #endregion
    }
}