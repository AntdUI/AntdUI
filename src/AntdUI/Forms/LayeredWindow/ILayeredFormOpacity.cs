// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
    public abstract class ILayeredFormOpacity : ILayeredForm
    {
        ITask? task_start = null;
        bool run_end = false, ok_end = false;
        internal byte maxalpha = 240;
        protected override void OnLoad(EventArgs e)
        {
            var t = Animation.TotalFrames(10, 80);
            task_start = new ITask((i) =>
            {
                var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                SetAnimateValue((byte)(maxalpha * val));
                return true;
            }, 10, t, () =>
            {
                SetAnimateValue(maxalpha);
                LoadOK();
            });
            base.OnLoad(e);
        }

        #region 设置动画参数

        void SetAnimateValue(byte _alpha)
        {
            if (alpha != _alpha)
            {
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
                    var t = Animation.TotalFrames(10, 80);
                    new ITask((i) =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        SetAnimateValue((byte)(maxalpha * (1F - val)));
                        return true;
                    }, 10, t, () =>
                    {
                        ok_end = true;
                        IClose(true);
                    });
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