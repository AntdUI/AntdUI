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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;

namespace AntdUI
{
    [ToolboxItem(true)]
    public class Image3D : IControl, ShadowConfig
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

        Image? image;
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
                if (image != null && value != null)
                {
                    var t = Animation.TotalFrames(Speed, Duration);
                    float _radius = radius * Config.Dpi;
                    ITask.Run(() =>
                    {
                        var _rect = ClientRectangle;
                        var rect = _rect.PaddingRect(Padding);
                        using (var bmpo = new Bitmap(_rect.Width, _rect.Height))
                        using (var bmpo2 = new Bitmap(_rect.Width, _rect.Height))
                        {
                            using (var g = Graphics.FromImage(bmpo).High())
                            {
                                if (shadow > 0 && shadowOpacity > 0) g.PaintShadow(this, _rect, rect, _radius, round);
                                g.Image(rect, image, imageFit, _radius, round);
                            }
                            using (var g = Graphics.FromImage(bmpo2).High())
                            {
                                if (shadow > 0 && shadowOpacity > 0) g.PaintShadow(this, _rect, rect, _radius, round);
                                g.Image(rect, value, imageFit, _radius, round);
                            }
                            var images = new List<Bitmap>(t);
                            if (Vertical)
                            {
                                for (int i = 0; i < t; i++)
                                {
                                    var prog = Animation.Animate(i + 1, t, 180, AnimationType.Ease);
                                    var cube = new Cube(bmpo.Width, bmpo.Height, 1);
                                    if (prog > 90)
                                    {
                                        prog = prog - 180;
                                        cube.RotateX = prog;
                                        cube.calcCube(_rect.Location);
                                        var bmp = cube.ToBitmap(bmpo2);
                                        bmp.Tag = cube.CentreX();
                                        images.Add(bmp);
                                    }
                                    else
                                    {
                                        cube.RotateX = prog;
                                        cube.calcCube(_rect.Location);
                                        var bmp = cube.ToBitmap(bmpo);
                                        bmp.Tag = cube.CentreX();
                                        images.Add(bmp);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < t; i++)
                                {
                                    var prog = Animation.Animate(i + 1, t, 180, AnimationType.Ease);
                                    var cube = new Cube(bmpo.Width, bmpo.Height, 1);
                                    if (prog > 90)
                                    {
                                        prog = prog - 180;
                                        cube.RotateY = prog;
                                        cube.calcCube(_rect.Location);
                                        var bmp = cube.ToBitmap(bmpo2);
                                        bmp.Tag = cube.CentreY();
                                        images.Add(bmp);
                                    }
                                    else
                                    {
                                        cube.RotateY = prog;
                                        cube.calcCube(_rect.Location);
                                        var bmp = cube.ToBitmap(bmpo);
                                        bmp.Tag = cube.CentreY();
                                        images.Add(bmp);
                                    }
                                }
                            }
                            for (int i = 0; i < images.Count; i++)
                            {
                                run = images[i];
                                Invalidate();
                                Thread.Sleep(Speed);
                            }
                        }
                        run?.Dispose();
                        run = null;
                        image = value;
                        Invalidate();
                    });
                }
                else
                {
                    image = value;
                    Invalidate();
                }
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

        /// <summary>
        /// 是否竖向
        /// </summary>
        [Description("是否竖向"), Category("动画"), DefaultValue(false)]
        public bool Vertical { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        [Description("速度"), Category("动画"), DefaultValue(10)]
        public int Speed { get; set; } = 10;

        /// <summary>
        /// 速度
        /// </summary>
        [Description("速度"), Category("动画"), DefaultValue(400)]
        public int Duration { get; set; } = 400;


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
                Invalidate();
            }
        }

        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ShadowColor { get; set; }

        float shadowOpacity = 0.3F;
        /// <summary>
        /// 阴影透明度
        /// </summary>
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
                Invalidate();
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
                Invalidate();
            }
        }

        #endregion

        #endregion

        #region 渲染

        Bitmap? run;
        protected override void OnDraw(DrawEventArgs e)
        {
            if (image == null)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            var rect = e.Rect.PaddingRect(Padding);
            float _radius = radius * Config.Dpi;
            FillRect(g, rect, back, _radius, round);
            if (run != null && run.Tag is PointF point) g.Image(run, point.X, point.Y, run.Width, run.Height);
            else
            {
                if (shadow > 0 && shadowOpacity > 0) g.PaintShadow(this, e.Rect, rect, _radius, round);
                g.Image(rect, image, imageFit, _radius, round);
            }
            base.OnDraw(e);
        }

        #region 渲染帮助

        void FillRect(Canvas g, RectangleF rect, Color color, float radius, bool round)
        {
            if (round)
            {
                g.FillEllipse(color, rect);
            }
            else if (radius > 0)
            {
                using (var path = rect.RoundPath(radius))
                {
                    g.Fill(color, path);
                }
            }
            else g.Fill(color, rect);
        }

        #endregion

        #endregion
    }
}