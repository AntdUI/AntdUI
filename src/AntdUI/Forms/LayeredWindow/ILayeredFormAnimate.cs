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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public abstract class ILayeredFormAnimate : ILayeredForm
    {
        internal static Dictionary<TAlignFrom, List<ILayeredFormAnimate>> list = new Dictionary<TAlignFrom, List<ILayeredFormAnimate>>();

        internal virtual TAlignFrom Align => TAlignFrom.TR;

        int start_X = 0, end_X = 0, start_Y = 0, end_Y = 0;

        #region 位置

        public int readY
        {
            get => TargetRect.Y + TargetRect.Height;
        }

        internal void SetPosition(Form form)
        {
            Rectangle workingArea;
            if (Config.ShowInWindow) workingArea = new Rectangle(form.Location, form.Size);
            else workingArea = Screen.FromControl(form).WorkingArea;
            int width = TargetRect.Width, height = TargetRect.Height;
            switch (Align)
            {
                case TAlignFrom.Top:
                    int xt = start_X = end_X = workingArea.X + (workingArea.Width - width) / 2;
                    end_Y = TopY(workingArea);
                    SetLocation(xt, start_Y = end_Y - height / 2);
                    break;
                case TAlignFrom.Bottom:
                    int xb = start_X = end_X = workingArea.X + (workingArea.Width - width) / 2;
                    end_Y = BottomY(workingArea);
                    SetLocation(xb, start_Y = end_Y + height / 2);
                    break;
                case TAlignFrom.TL:
                    end_X = workingArea.X;
                    SetLocation(start_X = end_X - width / 3, start_Y = end_Y = TopY(workingArea));
                    break;
                case TAlignFrom.TR:
                    end_X = workingArea.X + workingArea.Width - width;
                    SetLocation(start_X = end_X + width / 3, start_Y = end_Y = TopY(workingArea));
                    break;

                case TAlignFrom.BL:
                    end_X = workingArea.X;
                    SetLocation(start_X = end_X - width / 3, start_Y = end_Y = BottomY(workingArea));
                    break;
                case TAlignFrom.BR:
                    end_X = workingArea.X + workingArea.Width - width;
                    SetLocation(start_X = end_X + width / 3, start_Y = end_Y = BottomY(workingArea));
                    break;
            }
            Add();
        }
        void Add()
        {
            if (list.TryGetValue(Align, out var its))
            {
                its.Add(this);
                if (Align == TAlignFrom.Top || Align == TAlignFrom.TL || Align == TAlignFrom.TR)
                {
                    its.Sort((x, y) =>
                    {
                        return x.end_Y - y.end_Y;
                    });
                }
                else
                {
                    its.Sort((x, y) =>
                    {
                        return y.end_Y - x.end_Y;
                    });
                }
            }
            else list.Add(Align, new List<ILayeredFormAnimate> { this });
        }

        #region 核心

        int TopY(Rectangle workingArea)
        {
            int offset = (int)(Config.NoticeWindowOffsetXY * Config.Dpi);
            var y = TopYCore(workingArea, offset);
            if (y < workingArea.Bottom - TargetRect.Height) return y;
            else
            {
                var find_0 = list[Align][0];
                find_0.IRClose();
                list[Align].Remove(find_0);
                int y_temp = offset;
                lock (list)
                {
                    foreach (var it in list[Align])
                    {
                        it.SetPositionY(y_temp);
                        it.Print();
                        y_temp += it.TargetRect.Height;
                    }
                }
                return TopYCore(workingArea, offset);
            }
        }
        internal int TopYCore(Rectangle workingArea, int offset)
        {
            if (list.TryGetValue(Align, out var its) && its.Count > 0)
            {
                int y_temp = workingArea.Y + offset;
                while (true)
                {
                    int y_temp_end = y_temp + TargetRect.Height;
                    var find = its.Find(a => a.end_Y >= y_temp && a.readY <= y_temp_end);
                    if (find == null)
                    {
                        return y_temp;
                    }
                    y_temp += TargetRect.Height;
                }
            }
            else return workingArea.Y + offset;
        }
        int BottomY(Rectangle workingArea)
        {
            int offset = (int)(Config.NoticeWindowOffsetXY * Config.Dpi);
            var y = BottomYCore(workingArea, offset);
            if (y >= 0) return y;
            else
            {
                var find_0 = list[Align][0];
                find_0.IRClose();
                list[Align].Remove(find_0);
                int y_temp = workingArea.Bottom - TargetRect.Height - offset;
                lock (list)
                {
                    foreach (var it in list[Align])
                    {
                        it.SetPositionYB(y_temp);
                        it.Print();
                        y_temp -= it.TargetRect.Height;
                    }
                }
                return BottomYCore(workingArea, offset);
            }
        }
        internal int BottomYCore(Rectangle workingArea, int offset)
        {
            int y_temp = workingArea.Bottom - TargetRect.Height - offset;
            if (list.TryGetValue(Align, out var its) && its.Count > 0)
            {
                while (true)
                {
                    int y_temp_end = y_temp + TargetRect.Height;
                    var find = its.Find(a => a.end_Y >= y_temp && a.readY <= y_temp_end);
                    if (find == null)
                    {
                        return y_temp;
                    }
                    y_temp -= TargetRect.Height;
                }
            }
            else return y_temp;
        }

        #endregion

        internal void SetPositionCenter(int w)
        {
            if (Align == TAlignFrom.Top || Align == TAlignFrom.Bottom)
            {
                int x = TargetRect.X + (w - TargetRect.Width) / 2;
                SetLocationX(x);
                start_X = end_X = x;
            }
            else if (Align == TAlignFrom.TR || Align == TAlignFrom.BR)
            {
                int x = TargetRect.X - (TargetRect.Width - w);
                SetLocationX(x);
                start_X = end_X = x;
            }
            Print();
        }
        internal void SetPositionY(int y)
        {
            SetLocationY(y);
            end_Y = y;
            start_Y = y - TargetRect.Height / 2;
        }
        internal void SetPositionYB(int y)
        {
            SetLocationY(y);
            end_Y = y;
            start_Y = y + TargetRect.Height / 2;
        }

        #endregion

        ITask? task_start = null;
        bool run_end = false, ok_end = false;
        protected override void OnLoad(EventArgs e)
        {
            if (Config.Animation)
            {
                var t = Animation.TotalFrames(10, 200);
                task_start = new ITask(start_X == end_X ? i =>
                {
                    var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                    SetAnimateValueY(start_Y + (int)((end_Y - start_Y) * val), (byte)(240 * val));
                    return true;
                }
                : i =>
                {
                    var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                    SetAnimateValueX(start_X + (int)((end_X - start_X) * val), (byte)(240 * val));
                    return true;
                }, 10, t, () =>
                {
                    SetAnimateValue(end_X, end_Y, 240);
                });
            }
            else SetAnimateValue(end_X, end_Y, 240);
            base.OnLoad(e);
        }

        #region 设置动画参数

        void SetAnimateValueX(int x, byte _alpha)
        {
            if (TargetRect.X != x || alpha != _alpha)
            {
                SetLocationX(x);
                alpha = _alpha;
                Print();
            }
        }
        void SetAnimateValueY(int y, byte _alpha)
        {
            if (TargetRect.Y != y || alpha != _alpha)
            {
                SetLocationY(y);
                alpha = _alpha;
                Print();
            }
        }
        void SetAnimateValue(int x, int y, byte _alpha)
        {
            if (TargetRect.X != x || TargetRect.Y != y || alpha != _alpha)
            {
                SetLocation(x, y);
                alpha = _alpha;
                Print();
            }
        }

        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            list[Align].Remove(this);
            task_start?.Dispose();
            if (!ok_end)
            {
                e.Cancel = true;

                if (Config.Animation)
                {
                    if (!run_end)
                    {
                        run_end = true;
                        var t = Animation.TotalFrames(10, 200);
                        new ITask(start_X == end_X ? (i) =>
                        {
                            var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            SetAnimateValueY(end_Y - (int)((end_Y - start_Y) * val), (byte)(240 * (1F - val)));
                            return true;
                        }
                        : (i) =>
                        {
                            var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            SetAnimateValueX(end_X - (int)((end_X - start_X) * val), (byte)(240 * (1F - val)));
                            return true;
                        }, 10, t, () =>
                        {
                            ok_end = true;
                            IClose(true);
                        });
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
            list[Align].Remove(this);
            task_start?.Dispose();
            base.Dispose(disposing);
        }

        public void IRClose()
        {
            ok_end = true;
            IClose(true);
        }
    }
}