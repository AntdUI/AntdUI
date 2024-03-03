﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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

namespace AntdUI
{
    public abstract class ILayeredFormOpacityDown : ILayeredForm
    {
        ITask? task_start = null;
        bool run_end = false, ok_end = false;

        internal int EndHeight = 0;
        internal bool Inverted = false;

        public override bool MessageClose => true;

        protected override void OnLoad(EventArgs e)
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

        public virtual void LoadOK() { }

        protected override void OnClosing(CancelEventArgs e)
        {
            task_start?.Dispose();
            if (!ok_end)
            {
                e.Cancel = true;
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
            base.OnClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            task_start?.Dispose();
            base.Dispose(disposing);
        }
    }
}