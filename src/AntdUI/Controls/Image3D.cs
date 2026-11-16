// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

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
        [Description("背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(typeof(Color), "Transparent")]
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
        [Description("圆角"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0)]
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
        [Description("圆角样式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
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
        [Description("图片"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Image? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                RunAnimation(value);
            }
        }

        TFit imageFit = TFit.Cover;
        /// <summary>
        /// 图片布局
        /// </summary>
        [Description("图片布局"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TFit.Cover)]
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

        #region 文本

        TAlignMini textalign = TAlignMini.Bottom;
        /// <summary>
        /// 文本对齐方向
        /// </summary>
        [Description("文本对齐方向"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TAlignMini.Bottom)]
        public TAlignMini TextAlign
        {
            get => textalign;
            set
            {
                if (textalign == value) return;
                textalign = value;
                Invalidate();
                OnPropertyChanged(nameof(TextAlign));
            }
        }

        bool showText = false;
        /// <summary>
        /// 显示文本
        /// </summary>
        [Description("显示文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool ShowText
        {
            get => showText;
            set
            {
                if (showText == value) return;
                showText = value;
                Invalidate();
                OnPropertyChanged(nameof(ShowText));
            }
        }

        Padding _textPadding = new Padding(0);
        /// <summary>
        /// 文本边距
        /// </summary>
        [Description("文本边距"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public Padding TextPadding
        {
            get => _textPadding;
            set
            {
                if (_textPadding == value) return;
                _textPadding = value;
                if (showText) Invalidate();
                OnPropertyChanged(nameof(TextPadding));

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

        #region 阴影

        int shadow = 0;
        /// <summary>
        /// 阴影大小
        /// </summary>
        [Description("阴影大小"), Category("阴影"), DefaultValue(0)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                Invalidate();
                OnPropertyChanged(nameof(Shadow));
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
                OnPropertyChanged(nameof(ShadowOpacity));
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
                Invalidate();
                OnPropertyChanged(nameof(ShadowOffsetY));
            }
        }

        #endregion

        #region 悬浮

        /// <summary>
        /// 启用悬浮交互
        /// </summary>
        [Description("启用悬浮交互"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool EnableHover { get; set; }

        /// <summary>
        /// 悬浮前景
        /// </summary>
        [Description("悬浮前景"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Color? HoverFore { get; set; }

        /// <summary>
        /// 悬浮背景
        /// </summary>
        [Description("悬浮背景"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Color? HoverBack { get; set; }

        /// <summary>
        /// 悬浮图标
        /// </summary>
        [Description("悬浮图标"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public Image? HoverImage { get; set; }

        /// <summary>
        /// 悬浮图标SVG
        /// </summary>
        [Description("悬浮图标SVG"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? HoverImageSvg { get; set; }

        /// <summary>
        /// 悬浮图标比例
        /// </summary>
        [Description("悬浮图标比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.4F)]
        public float HoverImageRatio { get; set; } = .4F;

        #endregion

        #region 3D倾斜

        bool tilt = false;
        /// <summary>
        /// 启用3D倾斜（悬停时跟随鼠标）
        /// </summary>
        [Description("启用3D倾斜（悬停时跟随鼠标）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool Tilt
        {
            get => tilt;
            set
            {
                if (tilt == value) return;
                tilt = value;
                if (!tilt) ClearTilt();
            }
        }

        int tiltMax = 15;
        /// <summary>
        /// 3D倾斜最大角度
        /// </summary>
        [Description("3D倾斜最大角度"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(15)]
        public int TiltMax
        {
            get => tiltMax;
            set
            {
                if (tiltMax == value) return;
                if (value < 0) value = 0;
                else if (value > 60) value = 60;
                tiltMax = value;
                if (tilt && image != null && tiltLoopRunning == 1) UpdateTilt(tiltMouse.X, tiltMouse.Y);
                else Invalidate();
            }
        }

        #endregion

        #endregion

        #region 动画

        CancellationTokenSource? animationCancel;

        /// <summary>
        /// 执行动画过渡
        /// </summary>
        /// <param name="value">切换到新图片</param>
        public void RunAnimation(Image? value)
        {
            if (image != null && value != null)
            {
                // 动画期间由动画帧接管显示，先清掉倾斜帧
                ClearTilt();
                var t = Animation.TotalFrames(Speed, Duration);
                float _radius = radius * Dpi;
                // 取消令牌：新动画开始时使旧动画线程尽快退出，避免并发竞争 run / image
                var cts = new CancellationTokenSource();
                var token = cts.Token;
                var oldcts = Interlocked.Exchange(ref animationCancel, cts);
                try { oldcts?.Cancel(); } catch { }
                ITask.Run(() =>
                {
                    var _rect = ClientRectangle;
                    if (_rect.Width <= 0 || _rect.Height <= 0)
                    {
                        if (!token.IsCancellationRequested)
                        {
                            image = value;
                            Invalidate();
                        }
                        return;
                    }
                    var rect = _rect.PaddingRect(Padding);
                    // 已退役帧延迟释放（保留最近数帧），避免与 UI 线程绘制发生“使用已释放位图”的竞态
                    var retired = new Queue<Bitmap>();
                    try
                    {
                        using (var bmpo = new Bitmap(_rect.Width, _rect.Height))
                        using (var bmpo2 = new Bitmap(_rect.Width, _rect.Height))
                        {
                            using (var g = Graphics.FromImage(bmpo).High(Dpi))
                            using (var g2 = Graphics.FromImage(bmpo2).High(Dpi))
                            {
                                PaintImage(g, image, _radius, _rect, rect);
                                PaintImage(g2, value, _radius, _rect, rect);
                            }
                            // 源像素整段仅提取一次，整个动画期间所有帧复用（原先每帧重复提取）
                            using (var srco = BgraImage.FromBitmap(bmpo))
                            using (var srcn = BgraImage.FromBitmap(bmpo2))
                            {
                                var sw = Stopwatch.StartNew();
                                for (int i = 0; i < t; i++)
                                {
                                    if (token.IsCancellationRequested) return;
                                    var prog = Animation.Animate(i + 1, t, 180, AnimationType.Ease);
                                    var cube = new Cube(bmpo.Width, bmpo.Height, 1);
                                    bool isnew = prog > 90;
                                    if (Vertical) cube.RotateX = isnew ? prog - 180 : prog;
                                    else cube.RotateY = isnew ? prog - 180 : prog;
                                    cube.calcCube(_rect.Location);
                                    // 逐帧即时渲染，无需预缓存全部帧后再播放
                                    var frame = cube.ToBitmap(isnew ? srcn : srco);
                                    frame.Tag = Vertical ? cube.CentreX() : cube.CentreY();
                                    var old = Interlocked.Exchange<Bitmap>(ref this.frame, frame);
                                    if (old != null) retired.Enqueue(old);
                                    while (retired.Count > 3) retired.Dequeue().Dispose();
                                    Invalidate();
                                    // 按目标时间戳对齐帧间隔，渲染耗时不再叠加到动画时长
                                    long target = (long)(i + 1) * Speed;
                                    int wait;
                                    while ((wait = (int)(target - sw.ElapsedMilliseconds)) > 0)
                                    {
                                        if (token.IsCancellationRequested) return;
                                        Thread.Sleep(wait > 16 ? 16 : wait);
                                    }
                                }
                            }
                        }
                        if (!token.IsCancellationRequested)
                        {
                            var last = Interlocked.Exchange(ref frame, null);
                            if (last != null) retired.Enqueue(last);
                            image = value;
                            Invalidate();
                            // 留出一帧绘制窗口再释放最后一批帧
                            Thread.Sleep(64);
                        }
                    }
                    finally
                    {
                        while (retired.Count > 0) retired.Dequeue().Dispose();
                        cts.Dispose();
                    }
                });
            }
            else
            {
                image = value;
                Invalidate();
            }
        }

        #endregion

        #region 渲染

        Bitmap? frame;
        protected override void OnDraw(DrawEventArgs e)
        {
            if (image == null)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            if (frame != null && frame.Tag is PointF point) g.Image(frame, point.X, point.Y, frame.Width, frame.Height);
            else if (tiltFrame != null && tiltFrame.Tag is PointF tpoint) g.Image(tiltFrame, tpoint.X, tpoint.Y, tiltFrame.Width, tiltFrame.Height);
            else PaintImage(g, image, radius * Dpi, e.Rect, e.Rect.PaddingRect(Padding));
            base.OnDraw(e);
        }

        void PaintImage(Canvas g, Image image, float radius, Rectangle rectT, Rectangle rect)
        {
            if (shadow > 0 && shadowOpacity > 0) g.PaintShadow(this, rectT, rect, radius, round);
            FillRect(g, rect, back, radius, round);
            g.Image(rect, image, imageFit, radius, round);
            if (Hover)
            {
                int size = (int)((rect.Width > rect.Height ? rect.Height : rect.Width) * HoverImageRatio);
                FillRect(g, rect, HoverBack ?? Colour.TextTertiary.Get(ColorScheme, nameof(Image3D), Name), radius, round);
                var rect_hover = new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size);
                if (HoverImage != null) g.Image(HoverImage, rect_hover);
                if (HoverImageSvg != null) g.Svg(HoverImageSvg, rect_hover, HoverFore ?? Colour.BgBase.Get(ColorScheme, nameof(Image3D), Name));
            }
            if (borderWidth > 0)
            {
                using (var path = rect.RoundPath(radius))
                {
                    g.Draw(borderColor ?? Colour.BorderColor.Get(ColorScheme, nameof(Panel), Name), borderWidth * Dpi, borderStyle, path);
                }
            }
            if (ShowText) g.String(Text, Font, ForeColor, rect.DeflateRect(_textPadding, Dpi), TextFormat);
        }

        #region 渲染帮助

        FormatFlags TextFormat
        {
            get
            {
                switch (textalign)
                {
                    case TAlignMini.Top: return FormatFlags.Top | FormatFlags.HorizontalCenter;
                    case TAlignMini.Bottom: return FormatFlags.Bottom | FormatFlags.HorizontalCenter;
                    case TAlignMini.Left: return FormatFlags.Left | FormatFlags.VerticalCenter;
                    case TAlignMini.Right: return FormatFlags.Right | FormatFlags.VerticalCenter;
                    default: return FormatFlags.Center;
                }
            }
        }

        void FillRect(Canvas g, RectangleF rect, Color color, float radius, bool round)
        {
            if (round) g.FillEllipse(color, rect);
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

        #region 3D倾斜渲染

        Bitmap? tiltFrame;
        Bitmap? tiltRetired;
        Point tiltMouse = Point.Empty;
        float tiltX = 0, tiltY = 0;              // 当前倾斜角
        float tiltTargetX = 0, tiltTargetY = 0;  // 目标倾斜角
        int tiltLoopRunning = 0;
        CancellationTokenSource? tiltAnim;

        /// <summary>
        /// 立即停止倾斜并复位（UI 线程调用）
        /// </summary>
        void ClearTilt()
        {
            var anim = Interlocked.Exchange(ref tiltAnim, null);
            try { anim?.Cancel(); } catch { }
            tiltTargetX = tiltTargetY = 0;
            tiltX = tiltY = 0;
            ClearTiltFrame();
        }

        void ClearTiltFrame()
        {
            var frame = Interlocked.Exchange(ref tiltFrame, null);
            try { frame?.Dispose(); } catch { }
            Invalidate();
        }

        /// <summary>
        /// 根据鼠标位置更新倾斜目标角（相对中心归一化到 [-1, 1]）
        /// </summary>
        void UpdateTilt(int mx, int my)
        {
            tiltMouse = new Point(mx, my);
            var _rect = ClientRectangle;
            if (_rect.Width <= 0 || _rect.Height <= 0) return;
            float cx = _rect.Width / 2F, cy = _rect.Height / 2F;
            float nx = (mx - cx) / cx;
            if (nx < -1) nx = -1;
            else if (nx > 1) nx = 1;
            float ny = (my - cy) / cy;
            if (ny < -1) ny = -1;
            else if (ny > 1) ny = 1;
            // 鼠标偏向哪一侧，该侧即向远处倾斜（经典 3D card tilt 手感）
            // 注意：calcCube 的 X 投影是镜像的（3D 左边缘投影到屏幕右侧），故 Y 轴角度取反才能与 X 轴手感一致
            tiltTargetX = ny * tiltMax;
            tiltTargetY = -nx * tiltMax;
            StartTiltAnimation();
        }

        /// <summary>
        /// 启动倾斜过渡线程（已在运行则仅更新目标角）
        /// </summary>
        void StartTiltAnimation()
        {
            if (Interlocked.CompareExchange(ref tiltLoopRunning, 1, 0) != 0) return;
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            tiltAnim = cts;
            ITask.Run(() =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        if (IsDisposed) return;
                        float dx = tiltTargetX - tiltX, dy = tiltTargetY - tiltY;
                        if (Math.Abs(dx) < 0.05F && Math.Abs(dy) < 0.05F)
                        {
                            // 已到达目标
                            tiltX = tiltTargetX;
                            tiltY = tiltTargetY;
                            if (tiltX == 0 && tiltY == 0) return; // 回到平面，收尾清帧（finally）
                            Thread.Sleep(15); // 悬停待机，等待目标变化
                            continue;
                        }
                        // 指数趋近：跟鼠标快、回平面慢，过渡不突兀
                        float k = (tiltTargetX == 0 && tiltTargetY == 0) ? 0.15F : 0.3F;
                        tiltX += dx * k;
                        tiltY += dy * k;
                        RenderTilt(tiltX, tiltY);
                        Thread.Sleep(10);
                    }
                }
                finally
                {
                    ClearTiltFrame();
                    var retired = tiltRetired;
                    tiltRetired = null;
                    try { retired?.Dispose(); } catch { }
                    Interlocked.Exchange(ref tiltLoopRunning, 0);
                    try { cts.Dispose(); } catch { }
                }
            });
        }

        /// <summary>
        /// 按倾斜角合成当前外观并做透视变换（过渡线程调用）
        /// </summary>
        void RenderTilt(float rotateX, float rotateY)
        {
            var _rect = ClientRectangle;
            if (_rect.Width <= 0 || _rect.Height <= 0)
            {
                ClearTiltFrame();
                return;
            }
            var rect = _rect.PaddingRect(Padding);
            using (var bmpo = new Bitmap(_rect.Width, _rect.Height))
            {
                using (var g = Graphics.FromImage(bmpo).High(Dpi))
                {
                    PaintImage(g, image!, radius * Dpi, _rect, rect);
                }
                using (var src = BgraImage.FromBitmap(bmpo))
                {
                    var cube = new Cube(bmpo.Width, bmpo.Height, 1)
                    {
                        RotateX = rotateX,
                        RotateY = rotateY
                    };
                    cube.calcCube(_rect.Location);
                    var frame = cube.ToBitmap(src);
                    // 帧内容是投影四边形按包围盒裁剪的结果；投影以 drawOrigin 为中心展开，
                    // 需平移半个宽高才落回控件矩形（与翻转动画 CentreX/CentreY 的 +width/2 同理）
                    float tx = Math.Min(Math.Min(cube.a.X, cube.b.X), Math.Min(cube.c.X, cube.d.X)) + bmpo.Width / 2F;
                    float ty = Math.Min(Math.Min(cube.a.Y, cube.b.Y), Math.Min(cube.c.Y, cube.d.Y)) + bmpo.Height / 2F;
                    frame.Tag = new PointF(tx, ty);
                    var old = Interlocked.Exchange(ref tiltFrame, frame);
                    // 延迟一拍释放旧帧，避免 UI 线程正在绘制
                    var retired = tiltRetired;
                    tiltRetired = old;
                    try { retired?.Dispose(); } catch { }
                }
            }
            Invalidate();
        }

        #endregion

        #region 键盘

        bool hover = false;
        bool Hover
        {
            get => hover;
            set
            {
                if (hover == value) return;
                hover = value;
                Invalidate();
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (EnableHover) Hover = true;
            else if (Hover) Hover = false;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (tilt && image != null && frame == null) UpdateTilt(e.X, e.Y);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Hover = false;
            if (tilt)
            {
                // 目标归零，由过渡线程平滑收回平面（避免效果瞬间消失）
                tiltTargetX = 0;
                tiltTargetY = 0;
                StartTiltAnimation();
            }
        }

        #endregion
    }
}