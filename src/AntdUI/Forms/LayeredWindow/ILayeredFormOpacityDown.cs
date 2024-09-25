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
using System.Windows.Forms;

namespace AntdUI
{
    public abstract class ILayeredFormOpacityDown : ILayeredForm
    {
        ITask? task_start = null;
        bool run_end = false, ok_end = false;

        internal int EndHeight = 0;
        internal bool Inverted = false;

        public override bool MessageEnable => true;

        protected override void OnLoad(EventArgs e)
        {
            if (Config.Animation)
            {
                var t = Animation.TotalFrames(10, 100);
                if (Inverted)
                {
                    int endY = TargetRect.Y;
                    task_start = new ITask((i) =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        int height = (int)(EndHeight * val);
                        SetAnimateValue(endY + (EndHeight - height), height, val);
                        return true;
                    }, 10, t, () =>
                    {
                        SetAnimateValue(endY, EndHeight, 255);
                        LoadOK();
                    });
                }
                else
                {
                    task_start = new ITask((i) =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        SetAnimateValue((int)(EndHeight * val), val);
                        return true;
                    }, 10, t, () =>
                    {
                        SetAnimateValue(EndHeight, 255);
                        LoadOK();
                    });
                }
            }
            else
            {
                if (Inverted)
                {
                    int endY = TargetRect.Y;
                    SetAnimateValue(endY, EndHeight, 255);
                }
                else SetAnimateValue(EndHeight, 255);
                LoadOK();
            }
            base.OnLoad(e);
        }

        #region 设置动画参数

        void SetAnimateValue(int y, int height, float alpha)
        {
            SetAnimateValue(y, height, (byte)(255 * alpha));
        }
        void SetAnimateValue(int height, float alpha)
        {
            SetAnimateValue(height, (byte)(255 * alpha));
        }
        void SetAnimateValue(int y, int height, byte _alpha)
        {
            if (TargetRect.Y != y || TargetRect.Height != height || alpha != _alpha)
            {
                SetLocationY(y);
                SetSizeH(height);
                alpha = _alpha;
                Print();
            }
        }
        void SetAnimateValue(int height, byte _alpha)
        {
            if (TargetRect.Height != height || alpha != _alpha)
            {
                SetSizeH(height);
                alpha = _alpha;
                Print();
            }
        }

        #endregion

        internal override bool CanLoadMessage { get; set; } = false;
        public virtual void LoadOK() { CanLoadMessage = true; LoadMessage(); }

        protected override void OnClosing(CancelEventArgs e)
        {
            task_start?.Dispose();
            if (!ok_end)
            {
                e.Cancel = true;
                if (Config.Animation)
                {
                    if (!run_end)
                    {
                        run_end = true;
                        var t = Animation.TotalFrames(10, 100);
                        if (Inverted)
                        {
                            int y = TargetRect.Y;
                            new ITask(i =>
                            {
                                var val = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                int height = (int)(EndHeight * val);
                                SetAnimateValue(y + (EndHeight - height), height, val);
                                return true;
                            }, 10, t, () =>
                            {
                                ok_end = true;
                                IClose(true);
                            });
                        }
                        else
                        {
                            new ITask(i =>
                            {
                                var val = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                SetAnimateValue((int)(EndHeight * val), val);
                                return true;
                            }, 10, t, () =>
                            {
                                ok_end = true;
                                IClose(true);
                            });
                        }
                    }
                }
                else
                {
                    ok_end = true;
                    IClose(true);
                }
            }
            base.OnClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            task_start?.Dispose();
            base.Dispose(disposing);
        }

        internal void CLocation(Point Point, TAlignFrom Placement, bool DropDownArrow, int ArrowSize, int Padding, int Width, int Height, Rectangle Rect, ref bool Inverted, ref TAlign ArrowAlign, bool Collision = false)
        {
            switch (Placement)
            {
                case TAlignFrom.Top:
                    Inverted = true;
                    if (DropDownArrow)
                    {
                        ArrowAlign = TAlign.Top;
                        SetLocation((Point.X + Rect.X) + (Rect.Width - Width) / 2, Point.Y - Height + Rect.Y - ArrowSize);
                    }
                    else SetLocation((Point.X + Rect.X) + (Rect.Width - Width) / 2, Point.Y - Height + Rect.Y);
                    break;
                case TAlignFrom.TL:
                    Inverted = true;
                    if (DropDownArrow)
                    {
                        int x = Point.X + Rect.X - Padding, y = Point.Y - Height + Rect.Y - ArrowSize;
                        ArrowAlign = TAlign.TL;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x > (screen.X + screen.Width) - TargetRect.Width)
                            {
                                ArrowAlign = TAlign.TR;
                                x = Point.X + (Rect.X + Rect.Width) - Width + Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    else
                    {
                        int x = Point.X + Rect.X - Padding, y = Point.Y - Height + Rect.Y;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x > (screen.X + screen.Width) - TargetRect.Width)
                            {
                                x = Point.X + (Rect.X + Rect.Width) - Width + Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    break;
                case TAlignFrom.TR:
                    Inverted = true;
                    if (DropDownArrow)
                    {
                        int x = Point.X + (Rect.X + Rect.Width) - Width + Padding, y = Point.Y - Height + Rect.Y - ArrowSize;
                        ArrowAlign = TAlign.TR;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x < 0)
                            {
                                ArrowAlign = TAlign.TL;
                                x = Point.X + Rect.X - Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    else
                    {
                        int x = Point.X + (Rect.X + Rect.Width) - Width + Padding, y = Point.Y - Height + Rect.Y;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x < 0)
                            {
                                x = Point.X + Rect.X - Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    break;
                case TAlignFrom.Bottom:
                    if (DropDownArrow)
                    {
                        ArrowAlign = TAlign.Bottom;
                        SetLocation((Point.X + Rect.X) + (Rect.Width - Width) / 2, Point.Y + Rect.Bottom + ArrowSize);
                    }
                    else SetLocation((Point.X + Rect.X) + (Rect.Width - Width) / 2, Point.Y + Rect.Bottom);
                    break;
                case TAlignFrom.BR:
                    if (DropDownArrow)
                    {
                        ArrowAlign = TAlign.BR;
                        int x = Point.X + (Rect.X + Rect.Width) - Width + Padding, y = Point.Y + Rect.Bottom + ArrowSize;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x < 0)
                            {
                                ArrowAlign = TAlign.BL;
                                x = Point.X + Rect.X - Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    else
                    {
                        int x = Point.X + (Rect.X + Rect.Width) - Width + Padding, y = Point.Y + Rect.Bottom;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x < 0)
                            {
                                x = Point.X + Rect.X - Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    break;
                case TAlignFrom.BL:
                default:
                    if (DropDownArrow)
                    {
                        int x = Point.X + Rect.X - Padding, y = Point.Y + Rect.Bottom + ArrowSize;
                        ArrowAlign = TAlign.BL;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x > (screen.X + screen.Width) - TargetRect.Width)
                            {
                                ArrowAlign = TAlign.BR;
                                x = Point.X + (Rect.X + Rect.Width) - Width + Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    else
                    {
                        int x = Point.X + Rect.X - Padding, y = Point.Y + Rect.Bottom;
                        SetLocation(x, y);
                        if (Collision)
                        {
                            var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                            if (x > (screen.X + screen.Width) - TargetRect.Width)
                            {
                                x = Point.X + (Rect.X + Rect.Width) - Width + Padding;
                                SetLocation(x, y);
                            }
                        }
                    }
                    break;
            }
        }
    }
}