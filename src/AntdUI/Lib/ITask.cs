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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntdUI
{
    public class ITask : IDisposable
    {
        public bool IsRun = false;

        /// <summary>
        /// 循环任务
        /// </summary>
        /// <param name="action">回调</param>
        /// <param name="interval">间隔</param>
        /// <param name="max">最大值</param>
        /// <param name="add">更新量</param>
        public ITask(Control control, Func<int, bool> action, int interval, int max, int add, Action? end = null)
        {
            bool ok = true;
            IsRun = true;
            task = Run(() =>
            {
                int val = 0;
                while (true)
                {
                    if (token.Wait(control))
                    {
                        ok = false;
                        return;
                    }
                    else
                    {
                        val += add;
                        if (val > max) val = 0;
                        if (action(val)) Thread.Sleep(interval);
                        else return;
                    }
                }
            }).ContinueWith(action =>
            {
                if (ok && end != null) end();
                Dispose();
            });
        }
        public ITask(Control control, Action<float> action, int interval, float max, float add, Action? end = null)
        {
            bool ok = true;
            IsRun = true;
            task = Run(() =>
            {
                float val = 0;
                while (true)
                {
                    if (token.Wait(control))
                    {
                        ok = false;
                        return;
                    }
                    else
                    {
                        val += add;
                        if (val > max) val = 0;
                        action(val);
                        Thread.Sleep(interval);
                    }
                }
            }).ContinueWith(action =>
            {
                if (ok && end != null) end();
                Dispose();
            });
        }

        public ITask(Control control, Func<bool> action, int interval, Action? end = null, int sleep = 0)
        {
            bool ok = true;
            IsRun = true;
            task = Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                while (true)
                {
                    if (token.Wait(control))
                    {
                        ok = false;
                        return;
                    }
                    else
                    {
                        if (action()) Thread.Sleep(interval);
                        else return;
                    }
                }
            }).ContinueWith(action =>
            {
                if (ok && end != null) end();
                Dispose();
            });
        }
        public ITask(Func<int, bool> action, int interval, int totalFrames, Action end, int sleep = 0)
        {
            IsRun = true;
            bool ok = true;
            task = Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                for (int i = 0; i < totalFrames; i++)
                {
                    if (token.Wait())
                    {
                        ok = false;
                        return;
                    }
                    else
                    {
                        if (action(i + 1)) Thread.Sleep(interval);
                        else return;
                    }
                }
            }).ContinueWith(action =>
            {
                if (ok) end();
                Dispose();
            });
        }
        public ITask(bool _is, int interval, int totalFrames, float cold, AnimationType type, Action<int, float> action, Action end)
        {
            IsRun = true;
            bool ok = true;
            task = Run(() =>
            {
                double init_val = 1D;
                if (_is)
                {
                    if (cold > -1) init_val = cold;
                    for (int i = 0; i < totalFrames; i++)
                    {
                        if (token.Wait())
                        {
                            ok = false;
                            return;
                        }
                        else
                        {
                            int currentFrames = i + 1;
                            double progress = ((currentFrames * 1.0) / totalFrames);
                            var prog = (float)(init_val - Animation.Animate(progress, init_val, type));
                            Tag = prog;
                            action(currentFrames, prog);
                            Thread.Sleep(interval);
                        }
                    }
                }
                else
                {
                    if (cold > -1) init_val = cold;
                    else init_val = 0;
                    for (int i = 0; i < totalFrames; i++)
                    {
                        if (token.Wait())
                        {
                            ok = false;
                            return;
                        }
                        else
                        {
                            int currentFrames = i + 1;
                            double progress = ((currentFrames * 1.0) / totalFrames);
                            var prog = (float)(Animation.Animate(progress, 1D + init_val, type) - init_val);
                            if (prog < 0) return;
                            Tag = prog;
                            action(currentFrames, prog);
                            Thread.Sleep(interval);
                        }
                    }
                }
            }).ContinueWith((action =>
            {
                if (ok)
                {
                    Tag = null;
                    end();
                }
                Dispose();
            }));
        }

        Task task;
        public void Wait() => task.Wait();

        public object? Tag { get; set; }

        public void Cancel()
        {
            token?.Cancel();
        }

        public void Dispose()
        {
            if (token != null)
            {
                token?.Cancel();
                token?.Dispose();
                token = null;
            }
            IsRun = false;
            GC.SuppressFinalize(this);
        }

        CancellationTokenSource? token = new CancellationTokenSource();

        public static Task Run(Action action, Action? end = null)
        {
            if (end == null)
            {
#if NET40
                return Task.Factory.StartNew(action);
#else
                return Task.Run(action);
#endif
            }
#if NET40
            return Task.Factory.StartNew(action).ContinueWith(action => { end(); });
#else
            return Task.Run(action).ContinueWith(action => { end(); });
#endif
        }

        public static Task<TResult> Run<TResult>(Func<TResult> action)
        {
#if NET40
            return Task.Factory.StartNew(action);
#else
            return Task.Run(action);
#endif
        }
    }
}