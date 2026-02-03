// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Tour 漫游式引导
    /// </summary>
    /// <remarks>用于分步引导用户了解产品功能的气泡组件。</remarks>
    public static class Tour
    {
        public static TourForm open(Form form, Action<Result> call, Action<Popover>? popover = null) => new Config(form, call, popover).open();

        public static TourForm open(this Config config)
        {
            Control? tmp = null;
            var mask = new LayeredFormTour(config, i =>
            {
                var data = new Result(i);
                config.StepCall(data);
                if (tmp != null)
                {
                    tmp.SizeChanged -= tmp_SizeChanged;
                    tmp.LocationChanged -= tmp_SizeChanged;
                }
                if (data.Data == null) return null;
                if (data.Data is Control control)
                {
                    tmp = control;
                    control.SizeChanged += tmp_SizeChanged;
                    control.LocationChanged += tmp_SizeChanged;
                    return GetControlPath(control, config.Form.Location, config.Scale);
                }
                else if (data.Data is Rectangle rect) return rect;
                return null;
            });
            mask.Show(config.Form);
            return mask;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// Tour 配置
            /// </summary>
            public Config(Form form, Action<Result> call)
            {
                Form = form;
                StepCall = call;
            }

            /// <summary>
            /// Tour 配置
            /// </summary>
            public Config(Form form, Action<Result> call, Action<Popover>? popoverCall)
            {
                Form = form;
                StepCall = call;
                PopoverCall = popoverCall;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; set; }

            /// <summary>
            /// 缩放
            /// </summary>
            public float Scale { get; set; } = 1.04F;

            /// <summary>
            /// 点击蒙层是否允许关闭
            /// </summary>
            public bool MaskClosable { get; set; } = true;

            /// <summary>
            /// 点击下一步
            /// </summary>
            public bool ClickNext { get; set; } = true;

            /// <summary>
            /// 步骤回调
            /// </summary>

            public Action<Result> StepCall { get; set; }

            /// <summary>
            /// 气泡回调
            /// </summary>

            public Action<Popover>? PopoverCall { get; set; }

            #region 设置

            public Config SetScale(float value)
            {
                Scale = value;
                return this;
            }
            public Config SetMaskClosable(bool value = false)
            {
                MaskClosable = value;
                return this;
            }
            public Config SetClickNext(bool value = false)
            {
                ClickNext = value;
                return this;
            }
            public Config SetCall(Action<Result> value)
            {
                StepCall = value;
                return this;
            }
            public Config SetStepCall(Action<Popover> value)
            {
                PopoverCall = value;
                return this;
            }

            #endregion
        }

        public class Result
        {
            public Result(int index)
            {
                Index = index;
            }

            /// <summary>
            /// 第几步
            /// </summary>
            public int Index { get; private set; }

            internal object? Data { get; set; }

            /// <summary>
            /// 设置控件
            /// </summary>
            public void Set(Control control) => Data = control;

            /// <summary>
            /// 设置区域
            /// </summary>
            public void Set(Rectangle rect) => Data = rect;

            /// <summary>
            /// 关闭
            /// </summary>
            public void Close() => Data = null;
        }

        public class Popover
        {
            public Popover(Form form, TourForm tour, int index)
            {
                Form = form;
                Tour = tour;
                Index = index;
            }

            public Popover(Form form, TourForm tour, int index, Rectangle? rect)
            {
                Form = form;
                Tour = tour;
                Index = index;
                Rect = rect;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; private set; }

            /// <summary>
            /// 引导
            /// </summary>
            public TourForm Tour { get; private set; }

            /// <summary>
            /// 第几步
            /// </summary>
            public int Index { get; private set; }

            /// <summary>
            /// 显示区域
            /// </summary>
            public Rectangle? Rect { get; private set; }
        }

        internal static Action? tmpCall;
        static void tmp_SizeChanged(object? sender, EventArgs e) => tmpCall?.Invoke();

        static Rectangle GetControlPath(Control control, Point point, float scale)
        {
            var pointscreen = control.PointToScreen(Point.Empty);
            if (scale > 1F)
            {
                int wo = control.Width, ho = control.Height, w = (int)(wo * scale), h = (int)(ho * scale);
                return new Rectangle(pointscreen.X - point.X + (wo - w) / 2, pointscreen.Y - point.Y + (ho - h) / 2, w, h);
            }
            else
            {
                return new Rectangle(pointscreen.X - point.X, pointscreen.Y - point.Y, control.Width, control.Height);
            }
        }
    }

    public interface TourForm
    {
        bool IsHandleCreated { get; }
        bool IsDisposed { get; }
        void Close();
        void IClose(bool isdispose = false);
        void Previous();
        void Next();
        void LoadData();
    }

    internal class LayeredFormTour : ILayeredFormOpacity, TourForm
    {
        int Radius = 0, Bor = 0;
        bool HasBor = false;
        Tour.Config config;
        Func<int, Rectangle?> call;
        int i = 0;
        internal bool topMost = false;
        public LayeredFormTour(Tour.Config _config, Func<int, Rectangle?> _call) : base(240)
        {
            config = _config;
            call = _call;
            current_rect = call(i);
            if (current_rect == null)
            {
                Close();
                return;
            }
            topMost = config.Form.SetTopMost(Handle);
            SetDpi(config.Form);
            HasBor = config.Form.FormFrame(out Radius, out Bor);
            if (config.Form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(config.Form.Size);
                SetLocation(config.Form.Location);
                Size = config.Form.Size;
                Location = config.Form.Location;
            }
        }

        public override string name => nameof(Tour);

        Rectangle? current_rect;

        protected override void OnLoad(EventArgs e)
        {
            Tour.tmpCall = LoadData;
            if (config.Form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(config.Form.Size);
                SetLocation(config.Form.Location);
                Size = config.Form.Size;
                Location = config.Form.Location;
            }
            config.Form.LocationChanged += Form_LSChanged;
            config.Form.SizeChanged += Form_LSChanged;
            base.OnLoad(e);
        }

        private void Form_LSChanged(object? sender, EventArgs e)
        {
            if (config.Form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(config.Form.Size);
                SetLocation(config.Form.Location);
                Size = config.Form.Size;
                Location = config.Form.Location;
            }
            temp?.Dispose(); temp = null;
            Print();
        }

        protected override void Dispose(bool disposing)
        {
            Tour.tmpCall = null;
            config.Form.LocationChanged -= Form_LSChanged;
            config.Form.SizeChanged -= Form_LSChanged;
            temp?.Dispose();
            temp = null;
            base.Dispose(disposing);
        }

        Bitmap? temp = null;
        bool isOK = false;
        public override void LoadOK()
        {
            temp?.Dispose();
            temp = null;
            isOK = true;
            base.LoadOK();
            config.PopoverCall?.Invoke(new Tour.Popover(this, this, i, current_rect));
        }

        public override void ClosingAnimation()
        {
            Tour.tmpCall = null;
            isOK = false;
            base.ClosingAnimation();
        }

        public override Bitmap? PrintBit()
        {
            Rectangle rect_read = TargetRectXY, rect = HasBor ? new Rectangle(Bor, 0, rect_read.Width - Bor * 2, rect_read.Height - Bor) : rect_read;
            if (isOK) return PrintBmp(rect_read, rect);
            if (temp == null || (temp.Width != rect_read.Width || temp.Height != rect_read.Height))
            {
                temp?.Dispose();
                temp = PrintBmp(rect_read, rect);
            }
            if (temp == null) return null;
            return new Bitmap(temp);
        }

        Bitmap PrintBmp(Rectangle rect_read, Rectangle rect)
        {
            var bmp = new Bitmap(rect_read.Width, rect_read.Height);
            using (var g = Graphics.FromImage(bmp).High(Dpi))
            {
                using (var brush = new SolidBrush(Color.FromArgb(115, 0, 0, 0)))
                {
                    using (var path = rect.RoundPath(Radius))
                    {
                        if (current_rect.HasValue) path.AddRectangle(current_rect.Value);
                        g.Fill(brush, path);
                    }
                }
            }
            return bmp;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (config.ClickNext) Next();
            else if (config.MaskClosable) IClose();
        }

        /// <summary>
        /// 跳转上一个
        /// </summary>
        public void Previous()
        {
            i--;
            LoadData();
        }

        /// <summary>
        /// 跳转下一个
        /// </summary>
        public void Next()
        {
            i++;
            LoadData();
        }

        AnimationTask? task_start;
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void LoadData()
        {
            int index = i;
            var rect = call(index);
            if (rect == null) IClose();
            else
            {
                temp?.Dispose();
                temp = null;
                task_start?.Dispose();
                task_start = null;
                if (Config.HasAnimation(name) && current_rect.HasValue)
                {
                    config.PopoverCall?.Invoke(new Tour.Popover(this, this, index));
                    Rectangle old = current_rect.Value, current = rect.Value;
                    current_rect = old;
                    var t = Animation.TotalFrames(10, 200);
                    task_start = new AnimationTask(i =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        current_rect = new Rectangle(old.X + (int)((current.X - old.X) * val), old.Y + (int)((current.Y - old.Y) * val),
                            old.Width + (int)((current.Width - old.Width) * val), old.Height + (int)((current.Height - old.Height) * val));
                        Print();
                        return true;
                    }, 10, t, () =>
                    {
                        config.PopoverCall?.Invoke(new Tour.Popover(this, this, index, current));
                        current_rect = current;
                        Print();
                        task_start = null;
                    });
                }
                else
                {
                    config.PopoverCall?.Invoke(new Tour.Popover(this, this, index, rect));
                    current_rect = rect;
                    Print();
                }
            }
        }
    }
}