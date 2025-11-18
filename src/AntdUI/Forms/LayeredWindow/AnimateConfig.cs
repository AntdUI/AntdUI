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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public class OpacityAnimateConfig : IAnimateConfig
    {
        ILayeredForm form;
        Action call_start, call_end;
        public OpacityAnimateConfig(ILayeredForm layered, Action start, Action end, byte alpha = 255)
        {
            form = layered;
            call_start = start;
            call_end = end;
            Alpha = alpha;
        }

        #region 属性

        /// <summary>
        /// 透明度
        /// </summary>
        public byte Alpha { get; set; }

        #endregion

        #region 动画

        ITask? task;
        bool ok_end = false;

        public void Start(string name)
        {
            if (Config.HasAnimation(name))
            {
                var t = Animation.TotalFrames(10, 80);
                if (form is SpinForm)
                {
                    task = new ITask(i =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        form.alpha = (byte)(Alpha * val);
                        return true;
                    }, 10, t, CallStartNo);
                }
                else
                {
                    task = new ITask(i =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        SetAnimateValue((byte)(Alpha * val));
                        return true;
                    }, 10, t, CallStart);
                }
            }
            else CallStart();
        }

        public bool End(string name, CloseReason closeReason)
        {
            switch (closeReason)
            {
                case CloseReason.UserClosing:
                    return End(name);
                default:
                    CallEnd();
                    return false;
            }
        }

        bool End(string name)
        {
            if (ok_end)
            {
                CallEnd();
                return false;
            }
            ok_end = true;
            task?.Dispose();
            if (Config.HasAnimation(name))
            {
                call_end();
                var t = Animation.TotalFrames(10, 80);
                task = new ITask((i) =>
                {
                    var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                    SetAnimateValue((byte)(Alpha * (1F - val)));
                    return true;
                }, 10, t, CallEnd, 0, true);
                return true;
            }
            else CallEnd();
            return false;
        }

        void CallStart()
        {
            Dispose();
            SetAnimateValue(Alpha, true);
            call_start();
        }
        void CallStartNo()
        {
            Dispose();
            form.alpha = Alpha;
            call_start();
        }

        void CallEnd()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
            form.IClose(true);
        }

        #endregion

        #region 设置动画参数

        Bitmap? bmp_tmp;
        void SetAnimateValue(byte _alpha, bool isrint = false)
        {
            if (isrint)
            {
                form.alpha = _alpha;
                form.Print(true);
                return;
            }
            if (form.alpha == _alpha) return;
            form.alpha = _alpha;
            if (form.IsHandleCreated && form.TargetRect.Width > 0 && form.TargetRect.Height > 0)
            {
                try
                {
                    bmp_tmp ??= form.Printmap();
                    if (bmp_tmp == null) return;
                    if (form.Print(bmp_tmp) == RenderResult.Invalid) bmp_tmp = null;
                }
                catch { }
            }
        }

        #endregion

        public void Dispose()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
            task?.Dispose();
            task = null;
        }
        public void DisposeBmp()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
        }
    }

    public class PushAnimateConfig : IAnimateConfig
    {
        ILayeredForm form;
        Action call_start, call_end;
        public PushAnimateConfig(ILayeredForm layered, Action start, Action end)
        {
            form = layered;
            call_start = start;
            call_end = end;
        }

        #region 属性

        public bool Inverted { get; set; }

        #endregion

        #region 动画

        ITask? task;
        bool run_end = false, ok_end = false;

        public void Start(string name)
        {
            if (Config.HasAnimation(name))
            {
                var t = Animation.TotalFrames(10, 100);
                if (Inverted)
                {
                    var tr = form.TargetRect;
                    int _y = tr.Y, _height = tr.Height;
                    task = new ITask((i) =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        int height = (int)(_height * val);
                        SetAnimateValue(_y + (_height - height), height, val);
                        return true;
                    }, 10, t, CallStart);
                }
                else
                {
                    int _height = form.TargetRect.Height;
                    task = new ITask((i) =>
                    {
                        var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                        int height = (int)(_height * val);
                        SetAnimateValue((int)(_height * val), val);
                        return true;
                    }, 10, t, CallStart);
                }
            }
            else CallStart();
        }
        public bool End(string name, CloseReason closeReason)
        {
            switch (closeReason)
            {
                case CloseReason.UserClosing:
                    return End(name);
                default:
                    CallEnd();
                    return false;
            }
        }

        bool End(string name)
        {
            if (ok_end) return false;
            else if (run_end) return true;
            task?.Dispose();
            if (Config.HasAnimation(name))
            {
                call_end();
                run_end = true;
                var t = Animation.TotalFrames(10, 100);
                if (Inverted)
                {
                    var tr = form.TargetRect;
                    int _y = tr.Y, _height = tr.Height;
                    new ITask(i =>
                    {
                        var val = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                        int height = (int)(_height * val);
                        SetAnimateValue(_y + (_height - height), height, val);
                        return true;
                    }, 10, t, CallEnd, 0, true);
                }
                else
                {
                    int _height = form.TargetRect.Height;
                    new ITask(i =>
                    {
                        var val = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                        SetAnimateValue((int)(_height * val), val);
                        return true;
                    }, 10, t, CallEnd, 0, true);
                }

                return true;
            }
            else CallEnd();
            return false;
        }

        void CallStart()
        {
            AnimateHeight = -1;
            Dispose();
            form.alpha = 255;
            form.Print(true);
            call_start();
        }

        void CallEnd()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
            ok_end = true;
            form.IClose(true);
        }

        #endregion

        #region 设置动画参数

        Bitmap? bmp_tmp;

        void SetAnimateValue(int y, int height, float alpha) => SetAnimateValue(y, height, (byte)(255 * alpha));
        void SetAnimateValue(int height, float alpha) => SetAnimateValue(height, (byte)(255 * alpha));

        void SetAnimateValue(int y, int height, byte _alpha)
        {
            if (AnimateY != y || AnimateHeight != height || form.alpha != _alpha)
            {
                AnimateY = y;
                AnimateHeight = height;
                form.alpha = _alpha;
                if (height == 0) return;
                try
                {
                    var tr = form.TargetRect;
                    var rect = new Rectangle(tr.X, y, tr.Width, height);
                    bmp_tmp ??= form.Printmap();
                    if (bmp_tmp == null) return;
                    if (form.Print(bmp_tmp, rect) == RenderResult.Invalid) bmp_tmp = null;
                }
                catch { }
            }
        }

        int AnimateY = -1, AnimateHeight = -1;
        void SetAnimateValue(int height, byte _alpha)
        {
            if (AnimateHeight != height || form.alpha != _alpha)
            {
                AnimateHeight = height;
                form.alpha = _alpha;
                if (height == 0) return;
                try
                {
                    var tr = form.TargetRect;
                    var rect = new Rectangle(tr.X, tr.Y, tr.Width, height);
                    bmp_tmp ??= form.Printmap();
                    if (bmp_tmp == null) return;
                    if (form.Print(bmp_tmp, rect) == RenderResult.Invalid) bmp_tmp = null;
                }
                catch { }
            }
        }

        #endregion

        public void Dispose()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
            task?.Dispose();
            task = null;
        }
        public void DisposeBmp()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
        }
    }

    public interface IAnimateConfig : IDisposable
    {
        void Start(string name);
        bool End(string name, CloseReason closeReason);
        void DisposeBmp();
    }
}