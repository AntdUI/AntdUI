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

using System;
using System.ComponentModel;

namespace AntdUI
{
    public abstract class ILayeredFormOpacity : ILayeredForm, LayeredFormAsynLoad
    {
        ITask? task_start;
        bool run_end = false, ok_end = false;
        public byte maxalpha = 240;
        protected override void OnLoad(EventArgs e)
        {
            if (Config.HasAnimation(name))
            {
                if (this is SpinForm)
                {
                    var t = Animation.TotalFrames(10, 80);
                    task_start = new ITask((i) =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        alpha = (byte)(maxalpha * val);
                        return true;
                    }, 10, t, () =>
                    {
                        alpha = maxalpha;
                        LoadOK();
                    });
                }
                else
                {
                    var t = Animation.TotalFrames(10, 80);
                    task_start = new ITask((i) =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        SetAnimateValue((byte)(maxalpha * val));
                        return true;
                    }, 10, t, IStart);
                }
            }
            else IStart();
            base.OnLoad(e);
        }

        #region 设置动画参数

        System.Drawing.Bitmap? bmp_tmp;
        void SetAnimateValue(byte _alpha, bool isrint = false)
        {
            if (isrint)
            {
                alpha = _alpha;
                Print(true);
                return;
            }
            if (alpha == _alpha) return;
            alpha = _alpha;
            if (IsHandleCreated && TargetRect.Width > 0 && TargetRect.Height > 0)
            {
                try
                {
                    if (bmp_tmp == null) bmp_tmp = PrintBit();
                    if (bmp_tmp == null) return;
                    if (Print(bmp_tmp) == RenderResult.Invalid) bmp_tmp = null;
                }
                catch { }
            }
        }

        #endregion

        public abstract string name { get; }

        /// <summary>
        /// 是否正在加载
        /// </summary>
        [Description("是否正在加载"), Category("参数"), DefaultValue(true)]
        public bool IsLoad { get; set; } = true;

        /// <summary>
        /// 加载完成回调
        /// </summary>
        [Description("加载完成回调"), Category("参数"), DefaultValue(null)]
        public Action? LoadCompleted { get; set; }

        public virtual void LoadOK()
        {
            IsLoad = false;
            LoadCompleted?.Invoke();
        }
        public virtual void ClosingAnimation() { }

        protected override void OnClosing(CancelEventArgs e)
        {
            task_start?.Dispose();
            if (!ok_end)
            {
                e.Cancel = true;
                if (Config.HasAnimation(name))
                {
                    if (!run_end)
                    {
                        ClosingAnimation();
                        run_end = true;
                        var t = Animation.TotalFrames(10, 80);
                        new ITask((i) =>
                        {
                            var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            SetAnimateValue((byte)(maxalpha * (1F - val)));
                            return true;
                        }, 10, t, IEnd);
                    }
                }
                else IEnd();
            }
            base.OnClosing(e);
        }

        #region 启动/结束

        void IStart()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
            SetAnimateValue(maxalpha, true);
            LoadOK();
        }
        void IEnd()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
            ok_end = true;
            IClose(true);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            task_start?.Dispose();
            task_start = null;
            base.Dispose(disposing);
        }
    }
}