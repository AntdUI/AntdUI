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
using System.Windows.Forms;

namespace AntdUI.Captcha
{
    /// <summary>
    /// PuzzleCaptcha 拼图验证码
    /// </summary>
    /// <remarks>拼图验证码控件，用于人机验证。</remarks>
    [Description("PuzzleCaptcha 拼图验证码")]
    [ToolboxItem(true)]
    [DefaultProperty("Image")]
    [Obsolete("beta")]
    [Designer(typeof(IControlDesigner))]
    public class PuzzleCaptcha : IControl
    {
        public PuzzleCaptcha() : base(ControlType.Select)
        {
            SetStyle(ControlStyles.UserMouse, true);
        }

        protected override Size DefaultSize => new Size(350, 200);

        #region 属性

        private Color? fore;

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

        private Image? image;

        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public Image? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                generate = false;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImage));
            }
        }

        int puzzleSize = 50;
        /// <summary>
        /// 拼图块大小
        /// </summary>
        [Description("拼图块大小"), Category("外观"), DefaultValue(50)]
        public int PuzzleSize
        {
            get => puzzleSize;
            set
            {
                if (puzzleSize == value) return;
                puzzleSize = value;
                generate = false;
                Invalidate();
                OnPropertyChanged(nameof(PuzzleSize));
            }
        }

        private PuzzleCaptchaState state = PuzzleCaptchaState.Default;

        /// <summary>
        /// 验证状态
        /// </summary>
        [Description("验证状态"), Category("行为"), DefaultValue(PuzzleCaptchaState.Default)]
        public PuzzleCaptchaState State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;
                Invalidate();
                OnPropertyChanged(nameof(State));
                OnStateChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region 私有字段

        private Point puzzlePosition; // 拼图块在背景中的正确位置
        private Point currentPuzzlePosition; // 拼图块当前位置
        private bool isDragging = false;
        private int dragStartX = 0;
        private int sliderPosition = 0;
        private const int SliderThumbWidth = 50;
        private const int TolerancePixels = 5; // 容错像素

        #endregion

        #region 事件

        /// <summary>
        /// 状态改变事件
        /// </summary>
        [Description("状态改变事件"), Category("行为")]
        public event EventHandler? StateChanged;

        /// <summary>
        /// 验证完成事件
        /// </summary>
        [Description("验证完成事件"), Category("行为")]
        public event EventHandler<PuzzleCaptchaEventArgs>? VerificationCompleted;

        protected virtual void OnStateChanged(EventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }

        protected virtual void OnVerificationCompleted(PuzzleCaptchaEventArgs e)
        {
            VerificationCompleted?.Invoke(this, e);
        }

        #endregion

        #region 拼图生成

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            generate = false;
        }

        bool generate = false;
        private void GeneratePuzzle(Rectangle crect, int puzzleSize)
        {
            Random rand = new Random();
            int maxX = crect.Width - puzzleSize, maxY = crect.Height - puzzleSize, gap = (int)(10 * Dpi);

            puzzlePosition = new Point(
               crect.X + rand.Next(puzzleSize * 2, maxX),
                crect.Y + rand.Next(gap, maxY)
            );

            // 初始化拼图块当前位置（左侧）
            currentPuzzlePosition = new Point(crect.X + gap, puzzlePosition.Y);
            sliderPosition = 0;
        }

        #endregion

        #region 鼠标交互

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && state == PuzzleCaptchaState.Default)
            {
                Rectangle sliderThumbRect = GetSliderThumbRect();
                var clientPoint = e.Location;
                if (sliderThumbRect.Contains(clientPoint))
                {
                    isDragging = true;
                    dragStartX = clientPoint.X;
                    Cursor = Cursors.Hand;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDragging)
            {
                var clientPoint = e.Location;
                int deltaX = clientPoint.X - dragStartX;
                int newPosition = sliderPosition + deltaX;

                // 计算滑块最大位置（考虑滑块自身宽度）
                int maxSliderPosition = sliderRect.Width - (int)(SliderThumbWidth * Dpi);
                // 限制滑块位置在有效范围内
                newPosition = newPosition < 0 ? 0 : (newPosition > maxSliderPosition ? maxSliderPosition : newPosition);
                sliderPosition = newPosition;

                // 计算拼图可移动的有效范围（使用Padding属性）
                var rect = DisplayRectangle;
                int displayWidth = rect.Width;
                int maxPuzzleMoveDistance = displayWidth - puzzleSize;

                // 计算拼图X坐标边界（考虑左右内边距）
                int minPuzzleX = rect.X + Padding.Left;
                int maxPuzzleX = rect.Right - puzzleSize - Padding.Right;

                // 计算滑块位置百分比（0到1之间）
                double progress = maxSliderPosition > 0 ? (double)sliderPosition / maxSliderPosition : 0;
                // 根据百分比计算拼图X坐标
                int puzzleX = (int)(minPuzzleX + progress * maxPuzzleMoveDistance);

                // 最终限制拼图X坐标
                puzzleX = puzzleX < minPuzzleX ? minPuzzleX : (puzzleX > maxPuzzleX ? maxPuzzleX : puzzleX);

                // 限制拼图Y坐标（考虑上下内边距）
                int minPuzzleY = rect.Y + Padding.Top;
                int maxPuzzleY = rect.Bottom - puzzleSize - Padding.Bottom;
                int puzzleY = currentPuzzlePosition.Y;
                puzzleY = puzzleY < minPuzzleY ? minPuzzleY : (puzzleY > maxPuzzleY ? maxPuzzleY : puzzleY);

                currentPuzzlePosition = new Point(puzzleX, puzzleY);

                dragStartX = clientPoint.X;
                Invalidate();
            }
            else
            {
                Rectangle sliderThumbRect = GetSliderThumbRect();
                Cursor = sliderThumbRect.Contains(e.Location) && state == PuzzleCaptchaState.Default ? Cursors.Hand : Cursors.Default;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isDragging)
            {
                isDragging = false;
                Cursor = Cursors.Default;
                CheckVerification();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!isDragging)
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region 验证逻辑

        private void CheckVerification()
        {
            // 检查拼图块是否接近正确位置
            int deltaX = Math.Abs(currentPuzzlePosition.X - puzzlePosition.X);

            if (deltaX <= (int)(TolerancePixels * Dpi))
            {
                // 验证成功
                currentPuzzlePosition = puzzlePosition;
                State = PuzzleCaptchaState.Success;
                OnVerificationCompleted(new PuzzleCaptchaEventArgs(true));
            }
            else
            {
                // 验证失败，重置
                State = PuzzleCaptchaState.Failed;
                OnVerificationCompleted(new PuzzleCaptchaEventArgs(false));

                // 延迟重置
                Timer resetTimer = new Timer { Interval = 1000 };
                resetTimer.Tick += (s, e) =>
                {
                    resetTimer.Stop();
                    resetTimer.Dispose();
                    ResetPuzzle();
                };
                resetTimer.Start();
            }
        }

        /// <summary>
        /// 重置拼图
        /// </summary>
        public void ResetPuzzle()
        {
            generate = false;
            State = PuzzleCaptchaState.Default;
            Invalidate();
        }

        #endregion

        public override Rectangle DisplayRectangle => ClientRectangle.DeflateRect(Padding);

        #region 绘制

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);

            var g = e.Canvas;

            var rect = DisplayRectangle;

            var rh = g.MeasureString(Config.NullText, Font).Height;

            int sliderHeight = rh * 2, gap = (int)(rh * 0.32F), gap2 = gap * 2, psize = (int)(puzzleSize * Dpi);

            var rect_img = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - sliderHeight - gap2);

            sliderRect = new Rectangle(rect.X, rect.Bottom - sliderHeight, rect.Width, sliderHeight);

            #region 绘制图片区域

            if (!generate)
            {
                generate = true;
                GeneratePuzzle(rect_img, psize);
            }

            // 绘制背景图片
            if (image == null)
            {
                using (var bmp = DrawDefaultImage(rect_img))
                {
                    g.Image(bmp, rect_img);
                    DrawPuzzlePiece(g, rect_img, bmp, psize);
                }
            }
            else
            {
                g.Image(image, rect_img);
                DrawPuzzlePiece(g, rect_img, image, psize);
            }


            #endregion

            // 绘制滑块区域
            DrawSlider(g);
        }

        Rectangle sliderRect;

        Bitmap DrawDefaultImage(Rectangle rect)
        {
            var bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(bmp).HighLay())
            {
                // 绘制一个简单的风景图作为示例
                using (var brush = new LinearGradientBrush(rect, Style.Db.PrimaryBg, Style.Db.PrimaryColor, 90f))
                {
                    g.Fill(brush, rect);
                }

                // 绘制山峦
                Point[] mountains = {
                    new Point(0, rect.Height),
                    new Point(0 + rect.Width / 4, rect.Y + rect.Height / 2),
                    new Point(0 + rect.Width / 2, rect.Y + rect.Height / 3),
                    new Point(0 + rect.Width * 3 / 4, rect.Y + rect.Height / 2),
                    new Point(rect.Width, rect.Height)
                };

                using (var mountainBrush = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.FillClosedCurve(mountainBrush, mountains);
                }

                int size = (int)(40 * Dpi), gap = (int)(12 * Dpi);
                // 绘制太阳
                var sunRect = new Rectangle(rect.Right - size - gap, rect.Y + gap, size, size);
                using (var sunBrush = new SolidBrush(Style.Db.Warning))
                {
                    g.FillEllipse(sunBrush, sunRect);
                }

            }
            return bmp;
        }

        /// <summary>
        /// 绘制拼图块
        /// </summary>
        private void DrawPuzzlePiece(Canvas g, Rectangle rect_img, Image image, int size)
        {
            if (state == PuzzleCaptchaState.Success) return;
            float bor = 2 * Dpi;
            Rectangle holeRect = new Rectangle(puzzlePosition.X, puzzlePosition.Y, size, size), puzzleRect = new Rectangle(currentPuzzlePosition.X, currentPuzzlePosition.Y, size, size);

            using (var holePath = CreatePuzzlePath(holeRect))
            {
                g.Fill(Style.Db.Fill, holePath);

                // 绘制边框
                using (var pen = new Pen(Style.Db.BgBase, bor))
                {
                    g.Draw(pen, holePath);
                }
            }
            // 创建拼图块路径
            using (var piecePath = CreatePuzzlePath(puzzleRect))
            {
                float scaleX = (float)rect_img.Width / image.Width, scaleY = (float)rect_img.Height / image.Height;
                float relativeX = (float)(holeRect.X - rect_img.X) / rect_img.Width, relativeY = (float)(holeRect.Y - rect_img.Y) / rect_img.Height;

                int srcX = (int)(relativeX * image.Width), srcY = (int)(relativeY * image.Height);
                int srcWidth = (int)(size / scaleX), srcHeight = (int)(size / scaleY);

                // 确保源区域在图片范围内
                srcX = Math.Max(0, Math.Min(srcX, image.Width - srcWidth));
                srcY = Math.Max(0, Math.Min(srcY, image.Height - srcHeight));

                var srcRect = new Rectangle(srcX, srcY, srcWidth, srcHeight);

                g.SetClip(piecePath);
                g.Image(image, puzzleRect, srcRect, GraphicsUnit.Pixel);
                g.ResetClip();

                // 绘制拼图块边框
                Color borderColor = state switch
                {
                    PuzzleCaptchaState.Failed => Style.Db.Error,
                    _ => Style.Db.BgBase
                };

                using (var pen = new Pen(borderColor, bor))
                {
                    g.Draw(pen, piecePath);
                }
            }
        }

        GraphicsPath CreatePuzzlePath(Rectangle rect)
        {
            var path = new GraphicsPath();

            // 创建简单的拼图形状（带有凸起和凹陷）
            int notch = (int)(rect.Width * 0.2F), notch2 = notch * 2;

            float right = rect.Y + rect.Height / 2F, bottom = rect.X + rect.Width / 2F;

            // 从左上角开始绘制路径
            path.StartFigure();

            // 顶部边 - 从左上角到右上角
            path.AddLine(rect.X, rect.Y, rect.Right, rect.Y);

            // 右侧边 - 从右上角到右侧凹口上方
            path.AddLine(rect.Right, rect.Y, rect.Right, right - notch);

            // 右侧半圆凹口（向内凹陷，逆时针绘制左半圆）
            path.AddArc(rect.Right - notch, right - notch, notch2, notch2, 270, -180);

            // 右侧边 - 从右侧凹口下方到右下角
            path.AddLine(rect.Right, right + notch, rect.Right, rect.Bottom);

            // 底部边 - 从右下角到底部凹口右侧
            path.AddLine(rect.Right, rect.Bottom, bottom + notch, rect.Bottom);

            // 底部半圆凹口（向内凹陷，逆时针绘制上半圆）
            path.AddArc(bottom - notch, rect.Bottom - notch, notch2, notch2, 0, -180);

            // 底部边 - 从底部凹口左侧到左下角
            path.AddLine(bottom - notch, rect.Bottom, rect.X, rect.Bottom);

            // 左侧边 - 从左下角到左上角（保留完整的左侧边）
            path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y);

            path.CloseFigure();

            return path;
        }

        private void DrawSlider(Canvas g)
        {
            // 绘制滑块轨道
            g.Fill(Style.Db.BorderColor, sliderRect);

            // 绘制滑块提示文本
            string sliderText = state switch
            {
                PuzzleCaptchaState.Success => "验证成功",
                PuzzleCaptchaState.Failed => "验证失败",
                _ => "向右拖动滑块填充拼图"
            };

            Color textColor = fore ?? Style.Db.Text;
            using (var brush = new SolidBrush(textColor))
            {
                g.String(sliderText, Font, brush, sliderRect);
            }

            int sliderThumbWidth = (int)(SliderThumbWidth * Dpi), ico_size = (int)(sliderRect.Height * 0.4F);

            // 绘制进度
            if (sliderPosition > 0)
            {
                var progressRect = new Rectangle(sliderRect.X, sliderRect.Y, sliderPosition + sliderThumbWidth, sliderRect.Height);
                Color progressColor = state switch
                {
                    PuzzleCaptchaState.Success => Style.Db.SuccessBg,
                    PuzzleCaptchaState.Failed => Style.Db.ErrorBg,
                    _ => Style.Db.PrimaryBg
                };
                g.Fill(progressColor, progressRect);
            }

            // 绘制滑块
            var thumbRect = GetSliderThumbRect();
            Color thumbColor = state switch
            {
                PuzzleCaptchaState.Success => Style.Db.Success,
                PuzzleCaptchaState.Failed => Style.Db.Error,
                _ => Style.Db.BgBase
            };

            using (var brush = new SolidBrush(thumbColor))
            using (var pen = new Pen(Style.Db.PrimaryBorder, Dpi))
            {
                g.Fill(brush, thumbRect);
                g.Draw(pen, thumbRect);
            }

            var icoRect = new Rectangle(thumbRect.X + (thumbRect.Width - ico_size) / 2, thumbRect.Y + (thumbRect.Height - ico_size) / 2, ico_size, ico_size);
            switch (state)
            {
                case PuzzleCaptchaState.Success:
                    g.GetImgExtend("CheckOutlined", icoRect, Style.Db.SuccessColor);
                    break;
                case PuzzleCaptchaState.Failed:
                    g.GetImgExtend("CloseOutlined", icoRect, Style.Db.ErrorColor);
                    break;
                default:
                    g.GetImgExtend("ArrowRightOutlined", icoRect, Style.Db.Text);
                    break;
            }
        }

        #endregion

        #region 辅助方法

        private Rectangle GetSliderThumbRect() => new Rectangle(sliderRect.X + sliderPosition, sliderRect.Y, (int)(SliderThumbWidth * Dpi), sliderRect.Height);

        #endregion
    }

    /// <summary>
    /// 拼图验证码状态
    /// </summary>
    public enum PuzzleCaptchaState
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        Default,

        /// <summary>
        /// 验证成功
        /// </summary>
        Success,

        /// <summary>
        /// 验证失败
        /// </summary>
        Failed
    }

    /// <summary>
    /// 拼图验证码事件参数
    /// </summary>
    public class PuzzleCaptchaEventArgs : EventArgs
    {
        public PuzzleCaptchaEventArgs(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool Success { get; }
    }
}