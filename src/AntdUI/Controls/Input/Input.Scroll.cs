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

using System.Drawing;

namespace AntdUI
{
    partial class Input
    {
        #region 参数

        Rectangle ScrollRect;
        RectangleF ScrollSlider;
        /// <summary>
        /// 滑块全高度（不算边距）
        /// </summary>
        float ScrollSliderFull;
        bool scrollhover = false;
        bool ScrollHover
        {
            get => scrollhover;
            set
            {
                if (scrollhover == value) return;
                scrollhover = value;
                Invalidate();
            }
        }
        int scrollx = 0, scrolly = 0, ScrollXMin = 0, ScrollXMax = 0, ScrollYMax = 0;
        int ScrollX
        {
            get => scrollx;
            set
            {
                if (SetScrollX(value)) Invalidate();
            }
        }
        int ScrollY
        {
            get => scrolly;
            set
            {
                if (SetScrollY(value)) Invalidate();
            }
        }
        bool SetScrollX(int value)
        {
            if (value > ScrollXMax) value = ScrollXMax;
            if (value < ScrollXMin) value = ScrollXMin;
            if (scrollx == value) return false;
            scrollx = value;
            if (SyncScrollObj is Input input) input.ScrollX = scrollx;
            return true;
        }
        bool SetScrollY(int value)
        {
            if (value > ScrollYMax) value = ScrollYMax;
            if (value < 0) value = 0;
            if (scrolly == value) return false;
            scrolly = value;
            CaretInfo.flag = true;
            if (SyncScrollObj is Input input) input.ScrollY = scrolly;
            return true;
        }

        bool ScrollXShow = false, ScrollYShow = false, ScrollYDown = false;

        #endregion

        #region 滚动动画

        string? oldtmp;
        bool ScrollIFTo(Rectangle r, bool rd = true)
        {
            if (ScrollYShow)
            {
                string tmp = r.Y.ToString() + lineheight.ToString();
                if (tmp == oldtmp) return false;
                oldtmp = tmp;
                int caretY = CaretInfo.Y - ScrollY, threshold = (int)(rect_text.Height * 0.3f);
                // 判断是否超出阈值，是则直接跳转
                if (caretY < rect_text.Y - threshold || caretY + CaretInfo.Height > rect_text.Bottom + threshold)
                {
                    // 直接跳转逻辑
                    if (caretY < rect_text.Y)
                    {
                        if (SetScrollY(r.Y - rect_text.Y) && rd) Invalidate();
                    }
                    else if (caretY + CaretInfo.Height > rect_text.Bottom)
                    {
                        if (SetScrollY(r.Bottom - rect_text.Height + CaretInfo.Height) && rd) Invalidate();
                    }
                    return false;
                }
                // 未超出阈值，使用动画滚动
                ITask.Run(() => ScrollToY(r));
                return true;
            }
            else if (ScrollXShow)
            {
                string tmp = r.X.ToString() + cache_font?.Length.ToString();
                if (tmp == oldtmp) return false;
                oldtmp = tmp;
                int caretX = CaretInfo.X - ScrollX, threshold = (int)(rect_text.Width * 0.3f);
                // 判断是否超出阈值，是则直接跳转
                if (caretX < rect_text.X - threshold || caretX + CaretInfo.Width > rect_text.Right + threshold)
                {
                    if (caretX < rect_text.X)
                    {
                        if (SetScrollX(r.X - rect_text.X) && rd) Invalidate();
                    }
                    else if (caretX + CaretInfo.Width > rect_text.Right)
                    {
                        if (SetScrollX(r.Right - rect_text.Width + CaretInfo.Width) && rd) Invalidate();
                    }
                    return false;
                }
                // 未超出阈值，使用动画滚动
                ITask.Run(() => ScrollToX(r));
                return true;
            }
            else
            {
                oldtmp = null;
                if (SetScrollX(0) || SetScrollY(0))
                {
                    if (rd) Invalidate();
                }
            }
            return false;
        }

        void ScrollToY(Rectangle r)
        {
            int stepSize = CaretInfo.Height, count = 0;
            int caretY = CaretInfo.Y - ScrollY;
            if (caretY < rect_text.Y) ScrollToYUP(r, stepSize, ref count);
            else if (caretY + CaretInfo.Height > rect_text.Bottom) ScrollToYDOWN(r, stepSize, ref count);
            else return;
        }
        void ScrollToYUP(Rectangle r, int stepSize, ref int count)
        {
            while (true)
            {
                int caretY = CaretInfo.Y - ScrollY;
                if (caretY < rect_text.Y)
                {
                    int targetY = ScrollY - stepSize;
                    bool rd = SetScrollY(targetY);
                    if (ScrollY == targetY)
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
                int caretY = CaretInfo.Y - ScrollY;
                if (caretY + CaretInfo.Height > rect_text.Bottom)
                {
                    int targetY = ScrollY + stepSize;
                    bool rd = SetScrollY(targetY);
                    if (ScrollY == targetY)
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
            int caretX = CaretInfo.X - ScrollX;
            if (caretX < rect_text.X) ScrollToXUP(r, stepSize, ref count);
            else if (caretX + CaretInfo.Width > rect_text.Right) ScrollToXDOWN(r, stepSize, ref count);
            else return;
        }
        void ScrollToXUP(Rectangle r, int stepSize, ref int count)
        {
            while (true)
            {
                int caretX = CaretInfo.X - ScrollX;
                if (caretX < rect_text.X)
                {
                    int targetX = ScrollX - stepSize;
                    bool rd = SetScrollX(targetX);
                    if (ScrollX == targetX)
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
                int caretX = CaretInfo.X - ScrollX;
                if (caretX + CaretInfo.Width > rect_text.Right)
                {
                    int targetX = ScrollX + stepSize;
                    bool rd = SetScrollX(targetX);
                    if (ScrollX == targetX)
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