// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    partial class Input
    {
        #region 参数

        ScrollInfo ScrollX { get; set; }
        ScrollInfo ScrollY { get; set; }

        public class ScrollInfo
        {
            internal Input control;
            public ScrollInfo(Input input) { control = input; }
            public Rectangle Rect { get; set; }
            public RectangleF Slider { get; set; }
            /// <summary>
            /// 滑块全高度（不算边距）
            /// </summary>
            public float SliderFull { get; set; }

            bool scrollhover = false;
            public bool Hover
            {
                get => scrollhover;
                set
                {
                    if (scrollhover == value) return;
                    scrollhover = value;
                    control.Invalidate();
                }
            }

            internal int val = 0;
            public int Value
            {
                get => val;
                set
                {
                    if (SetValue(value)) control.Invalidate();
                }
            }

            public int Min { get; set; }
            public int Max { get; set; }

            public virtual bool SetValue(int value)
            {
                if (value > Max) value = Max;
                if (value < Min) value = Min;
                if (val == value) return false;
                val = value;
                if (control.SyncScrollObj is Input input) input.ScrollX.Value = val;
                return true;
            }

            public bool Show { get; set; }
            public bool Down { get; set; }

            public void Clear()
            {
                Show = false;
                val = Min = Max = 0;
            }
        }

        public class ScrollYInfo : ScrollInfo
        {
            public ScrollYInfo(Input input) : base(input) { }
            public override bool SetValue(int value)
            {
                if (value > Max) value = Max;
                if (value < Min) value = Min;
                if (val == value) return false;
                val = value;
                control.CaretInfo.flag = true;
                if (control.SyncScrollObj is Input input) input.ScrollY.Value = val;
                return true;
            }
        }

        #endregion

        #region 滚动动画

        string? oldtmpY, oldtmpX;
        bool ScrollIFTo(Rectangle r, bool rd = true)
        {
            if (ScrollY.Show)
            {
                if (ScrollIFToY(r, rd) || (ScrollX.Show && ScrollIFToX(r, rd))) return true;
                return false;
            }
            else if (ScrollX.Show) return ScrollIFToX(r, rd);
            else
            {
                oldtmpY = oldtmpX = null;
                if (ScrollX.SetValue(0) || ScrollY.SetValue(0))
                {
                    if (rd) Invalidate();
                }
            }
            return false;
        }
        bool ScrollIFToY(Rectangle r, bool rd = true)
        {
            string tmp = r.Y.ToString() + lineheight.ToString();
            if (tmp == oldtmpY) return false;
            oldtmpY = tmp;
            int caretY = CaretInfo.Y - ScrollY.Value, threshold = (int)(rect_text.Height * 0.3f);
            // 判断是否超出阈值，是则直接跳转
            if (caretY < rect_text.Y - threshold || caretY + CaretInfo.Height > rect_text.Bottom + threshold)
            {
                // 直接跳转逻辑
                if (caretY < rect_text.Y)
                {
                    if (ScrollY.SetValue(r.Y - rect_text.Y) && rd) Invalidate();
                }
                else if (caretY + CaretInfo.Height > rect_text.Bottom)
                {
                    if (ScrollY.SetValue(r.Bottom - rect_text.Height + CaretInfo.Height) && rd) Invalidate();
                }
                return false;
            }
            // 未超出阈值，使用动画滚动
            ITask.Run(() => ScrollToY(r));
            return true;
        }
        bool ScrollIFToX(Rectangle r, bool rd = true)
        {
            string tmp = r.X.ToString() + cache_font?.Length.ToString();
            if (tmp == oldtmpX) return false;
            oldtmpX = tmp;
            int caretX = CaretInfo.X - ScrollX.Value, threshold = (int)(rect_text.Width * 0.3f);
            // 判断是否超出阈值，是则直接跳转
            if (caretX < rect_text.X - threshold || caretX + CaretInfo.Width > rect_text.Right + threshold)
            {
                if (caretX < rect_text.X)
                {
                    if (ScrollX.SetValue(r.X - rect_text.X) && rd) Invalidate();
                }
                else if (caretX + CaretInfo.Width > rect_text.Right)
                {
                    if (ScrollX.SetValue(r.Right - rect_text.Width + CaretInfo.Width) && rd) Invalidate();
                }
                return false;
            }
            // 未超出阈值，使用动画滚动
            ITask.Run(() => ScrollToX(r));
            return true;
        }

        void ScrollToY(Rectangle r)
        {
            int stepSize = CaretInfo.Height, count = 0;
            int caretY = CaretInfo.Y - ScrollY.Value;
            if (caretY < rect_text.Y) ScrollToYUP(r, stepSize, ref count);
            else if (caretY + CaretInfo.Height > rect_text.Bottom) ScrollToYDOWN(r, stepSize, ref count);
            else return;
        }
        void ScrollToYUP(Rectangle r, int stepSize, ref int count)
        {
            while (true)
            {
                int caretY = CaretInfo.Y - ScrollY.Value;
                if (caretY < rect_text.Y)
                {
                    int targetY = ScrollY.Value - stepSize;
                    bool rd = ScrollY.SetValue(targetY);
                    if (ScrollY.Value == targetY)
                    {
                        count++;
                        if (rd) Invalidate();
                        SleepGear(count);
                    }
                    else return;
                }
                else return;
            }
        }
        void ScrollToYDOWN(Rectangle r, int stepSize, ref int count)
        {
            while (true)
            {
                int caretY = CaretInfo.Y - ScrollY.Value;
                if (caretY + CaretInfo.Height > rect_text.Bottom)
                {
                    int targetY = ScrollY.Value + stepSize;
                    bool rd = ScrollY.SetValue(targetY);
                    if (ScrollY.Value == targetY)
                    {
                        count++;
                        if (rd) Invalidate();
                        SleepGear(count);
                    }
                    else return;
                }
                else return;
            }
        }

        void ScrollToX(Rectangle r)
        {
            int stepSize = CaretInfo.Height, count = 0;
            int caretX = CaretInfo.X - ScrollX.Value;
            if (caretX < rect_text.X) ScrollToXUP(r, stepSize, ref count);
            else if (caretX + CaretInfo.Width > rect_text.Right) ScrollToXDOWN(r, stepSize, ref count);
            else return;
        }
        void ScrollToXUP(Rectangle r, int stepSize, ref int count)
        {
            while (true)
            {
                int caretX = CaretInfo.X - ScrollX.Value;
                if (caretX < rect_text.X)
                {
                    int targetX = ScrollX.Value - stepSize;
                    bool rd = ScrollX.SetValue(targetX);
                    if (ScrollX.Value == targetX)
                    {
                        count++;
                        if (rd) Invalidate();
                        SleepGear(count);
                    }
                    else return;
                }
                else return;
            }
        }
        void ScrollToXDOWN(Rectangle r, int stepSize, ref int count)
        {
            while (true)
            {
                int caretX = CaretInfo.X - ScrollX.Value;
                if (caretX + CaretInfo.Width > rect_text.Right)
                {
                    int targetX = ScrollX.Value + stepSize;
                    bool rd = ScrollX.SetValue(targetX);
                    if (ScrollX.Value == targetX)
                    {
                        count++;
                        if (rd) Invalidate();
                        SleepGear(count);
                    }
                    else return;
                }
                else return;
            }
        }

        void SleepGear(int count)
        {
            if (count > 7) System.Threading.Thread.Sleep(1);
            else if (count > 5) System.Threading.Thread.Sleep(10);
            else if (count > 3) System.Threading.Thread.Sleep(30);
            else System.Threading.Thread.Sleep(50);
        }

        #endregion

        object? SyncScrollObj;
        public Input SyncScroll(Input input)
        {
            SyncScrollObj = input;
            return this;
        }
    }
}