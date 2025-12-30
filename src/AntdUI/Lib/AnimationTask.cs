// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntdUI
{
    public class AnimationTask : IDisposable
    {
        #region 旧

        /// <summary>
        /// 循环任务
        /// </summary>
        /// <param name="control">委托对象</param>
        /// <param name="action">回调</param>
        /// <param name="interval">间隔</param>
        /// <param name="end">结束回调</param>
        /// <param name="sleep">运行前睡眠</param>
        public AnimationTask(Control control, Func<bool> action, int interval, Action? end = null, int sleep = 0)
        {
            Run(() =>
            {
                while (true)
                {
                    if (token.Wait(control)) return false;
                    else
                    {
                        if (action()) Thread.Sleep(interval);
                        else return true;
                    }
                }
            }, new IAnimationConfig().SetEnd(end).SetSleep(sleep).SetPriority());
        }
        public AnimationTask(Control control, Func<bool> action)
        {
            Run(() =>
            {
                while (true)
                {
                    if (token.Wait(control)) return false;
                    else if (!action()) return false;
                }
            }, new IAnimationConfig().SetPriority());
        }

        /// <summary>
        /// 循环任务
        /// </summary>
        /// <param name="action">回调</param>
        /// <param name="interval">间隔</param>
        /// <param name="totalFrames">总帧数</param>
        /// <param name="end">结束回调</param>
        /// <param name="sleep">运行前睡眠</param>
        public AnimationTask(Func<int, bool> action, int interval, int totalFrames, Action end, int sleep = 0, bool isend = false)
        {
            Run(() =>
            {
                for (int i = 0; i < totalFrames; i++)
                {
                    if (token.Wait()) return false;
                    else
                    {
                        if (action(i + 1)) Thread.Sleep(interval);
                        else return true;
                    }
                }
                return true;
            }, new IAnimationConfig().SetEnd(end).SetSleep(sleep).SetEndRCall(isend).SetPriority());
        }

        Task? task;
        public void Wait() => task?.Wait();
        public object? Tag { get; set; }

        #endregion

        #region 核心

        CancellationTokenSource? token;

        public AnimationTask(IAnimationConfig config)
        {
            if (config is AnimationLinearConfig animationLinearConfig) Run(animationLinearConfig);
            else if (config is AnimationLinearFConfig animationLinearFConfig) Run(animationLinearFConfig);
            else if (config is AnimationLoopConfig animationLoopConfig) Run(animationLoopConfig);
            else if (config is AnimationFixedConfig animationFixedConfig) Run(animationFixedConfig);
            else if (config is AnimationFixed2Config animationFixed2Config) Run(animationFixed2Config);
        }

        void Run(AnimationLinearConfig config)
        {
            Run(() =>
            {
                if (config.Value.HasValue)
                {
                    int val = config.Value.Value;
                    if (config.Add > 0)
                    {
                        while (true)
                        {
                            if (token.Wait(config.Control)) return false;
                            else
                            {
                                val += config.Add;
                                if (val >= config.Max)
                                {
                                    val = config.Max;
                                    config.Call(val);
                                    return true;
                                }
                                if (config.Call(val)) Thread.Sleep(config.Interval);
                                else return true;
                            }
                        }
                    }
                    else
                    {

                        while (true)
                        {
                            if (token.Wait(config.Control)) return false;
                            else
                            {
                                val += config.Add;
                                if (val <= config.Max)
                                {
                                    val = config.Max;
                                    config.Call(val);
                                    return true;
                                }
                                if (config.Call(val)) Thread.Sleep(config.Interval);
                                else return true;
                            }
                        }
                    }
                }
                else
                {
                    int val = 0;
                    while (true)
                    {
                        if (token.Wait(config.Control)) return false;
                        else
                        {
                            val += config.Add;
                            if (val > config.Max) val = 0;
                            if (config.Call(val)) Thread.Sleep(config.Interval);
                            else return true;
                        }
                    }
                }
            }, config);
        }
        void Run(AnimationLinearFConfig config)
        {
            Run(() =>
            {
                if (config.Value.HasValue)
                {
                    float val = config.Value.Value;
                    if (config.Add > 0)
                    {
                        while (true)
                        {
                            if (token.Wait(config.Control)) return false;
                            else
                            {
                                val += config.Add;
                                if (val >= config.Max)
                                {
                                    val = config.Max;
                                    config.Call(val);
                                    return true;
                                }
                                if (config.Call(val)) Thread.Sleep(config.Interval);
                                else return true;
                            }
                        }
                    }
                    else
                    {

                        while (true)
                        {
                            if (token.Wait(config.Control)) return false;
                            else
                            {
                                val += config.Add;
                                if (val <= config.Max)
                                {
                                    val = config.Max;
                                    config.Call(val);
                                    return true;
                                }
                                if (config.Call(val)) Thread.Sleep(config.Interval);
                                else return true;
                            }
                        }
                    }
                }
                else
                {
                    float val = 0;
                    while (true)
                    {
                        if (token.Wait(config.Control)) return false;
                        else
                        {
                            val += config.Add;
                            if (val > config.Max) val = 0;
                            if (config.Call(val)) Thread.Sleep(config.Interval);
                            else return true;
                        }
                    }
                }
            }, config);
        }
        void Run(AnimationLoopConfig config)
        {
            Run(() =>
            {
                while (true)
                {
                    if (token.Wait(config.Control)) return false;
                    else
                    {
                        if (config.Call()) Thread.Sleep(config.Interval);
                        else return true;
                    }
                }
            }, config);
        }
        void Run(AnimationFixedConfig config)
        {
            Run(() =>
            {
                if (config.Arrow)
                {
                    if (config.LR)
                    {
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                config.Call(Animation.Animate((i + 1), config.TotalFrames, 2F, config.Type) - 1F);
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                config.Call(-(Animation.Animate((i + 1), config.TotalFrames, 2F, config.Type) - 1F));
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                }
                else
                {
                    if (config.LR)
                    {
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                config.Call(Animation.Animate((i + 1), config.TotalFrames, 1F, config.Type));
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                config.Call(1F - Animation.Animate((i + 1), config.TotalFrames, 1F, config.Type));
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                }
                return true;
            }, config);
        }
        void Run(AnimationFixed2Config config)
        {
            Run(() =>
            {
                double init_val = 1D;
                if (config.Call is Action<int, float, float> call_arrow)
                {
                    if (config.LR)
                    {
                        if (config.Value > -1) init_val = config.Value;
                        else init_val = 0;
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                int currentFrames = i + 1;
                                double progress = ((currentFrames * 1.0) / config.TotalFrames);
                                var prog = (float)(Animation.Animate(progress, 1D + init_val, config.Type) - init_val);
                                if (prog < 0) return true;
                                Tag = prog;

                                var ArrowProg = Animation.Animate(currentFrames, config.TotalFrames, 2F, config.Type) - 1F;
                                call_arrow(currentFrames, prog, ArrowProg);
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                    else
                    {
                        if (config.Value > -1) init_val = config.Value;
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                int currentFrames = i + 1;
                                double progress = ((currentFrames * 1.0) / config.TotalFrames);
                                var prog = (float)(init_val - Animation.Animate(progress, init_val, config.Type));
                                Tag = prog;

                                var ArrowProg = -(Animation.Animate(currentFrames, config.TotalFrames, 2F, config.Type) - 1F);
                                call_arrow(currentFrames, prog, ArrowProg);
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                }
                else if (config.Call is Action<int, float> call)
                {
                    if (config.LR)
                    {
                        if (config.Value > -1) init_val = config.Value;
                        else init_val = 0;
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                int currentFrames = i + 1;
                                double progress = ((currentFrames * 1.0) / config.TotalFrames);
                                var prog = (float)(Animation.Animate(progress, 1D + init_val, config.Type) - init_val);
                                if (prog < 0) return true;
                                Tag = prog;
                                call(currentFrames, prog);
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                    else
                    {
                        if (config.Value > -1) init_val = config.Value;
                        for (int i = 0; i < config.TotalFrames; i++)
                        {
                            if (token.Wait()) return false;
                            else
                            {
                                int currentFrames = i + 1;
                                double progress = ((currentFrames * 1.0) / config.TotalFrames);
                                var prog = (float)(init_val - Animation.Animate(progress, init_val, config.Type));
                                Tag = prog;
                                call(currentFrames, prog);
                                Thread.Sleep(config.Interval);
                            }
                        }
                    }
                }
                return true;
            }, config);
        }

        void Run(Func<bool> action, IAnimationConfig config)
        {
            token = new CancellationTokenSource();
            switch (config.Priority)
            {
                case TPriority.High:
                    new Thread(() =>
                    {
                        if (config.Sleep > 0) Thread.Sleep(config.Sleep);
                        bool ok = true;
                        try
                        {
                            ok = action();
                        }
                        catch { }
                        RunEnd(ok, config);
                    })
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.AboveNormal
                    }.Start();
                    break;
                case TPriority.Normal:
                    new Thread(() =>
                    {
                        if (config.Sleep > 0) Thread.Sleep(config.Sleep);
                        bool ok = true;
                        try
                        {
                            ok = action();
                        }
                        catch { }
                        RunEnd(ok, config);
                    })
                    {
                        IsBackground = true
                    }.Start();
                    break;
                default:
                    bool ok = true;
                    task = ITask.Run(() =>
                    {
                        if (config.Sleep > 0) Thread.Sleep(config.Sleep);
                        ok = action();
                    }).ContinueWith(action => RunEnd(ok, config));
                    break;
            }
        }
        void RunEnd(bool ok, IAnimationConfig config)
        {
            if (config.EndRCall)
            {
                Dispose();
                Tag = null;
                config.CallEnd?.Invoke();
            }
            else
            {
                if (ok)
                {
                    Tag = null;
                    config.CallEnd?.Invoke();
                }
                Dispose();
            }
        }

        #endregion

        public void Dispose()
        {
            if (token == null) return;
            try
            {
                token.Cancel();
                token.Dispose();
                token = null;
            }
            catch { }
        }
    }

    #region 配置

    /// <summary>
    /// 线性增量动画
    /// </summary>
    public class AnimationLinearConfig : IAnimationConfig
    {
        /// <summary>
        /// 线性增量动画
        /// </summary>
        /// <param name="control">句柄</param>
        /// <param name="call">任务</param>
        /// <param name="interval">动画间隔</param>
        /// <param name="max">最大值</param>
        /// <param name="add">更新量</param>
        public AnimationLinearConfig(Control control, Func<int, bool> call, int interval, int max, int add)
        {
            Control = control;
            Call = call;
            Interval = interval;
            Max = max;
            Add = add;
        }

        /// <summary>
        /// 线性增量动画
        /// </summary>
        /// <param name="control">句柄</param>
        /// <param name="call">任务</param>
        /// <param name="interval">动画间隔</param>
        public AnimationLinearConfig(Control control, Func<int, bool> call, int interval)
        {
            Control = control;
            Call = call;
            Interval = interval;
        }

        /// <summary>
        /// 句柄
        /// </summary>
        public Control Control { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        public Func<int, bool> Call { get; set; }

        /// <summary>
        /// 动画间隔
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// 更新量
        /// </summary>
        public int Add { get; set; }

        public int? Value { get; set; }

        #region 设置

        public AnimationLinearConfig SetControl(Control value)
        {
            Control = value;
            return this;
        }
        public AnimationLinearConfig SetCall(Func<int, bool> value)
        {
            Call = value;
            return this;
        }
        public AnimationLinearConfig SetInterval(int value)
        {
            Interval = value;
            return this;
        }
        public AnimationLinearConfig SetMax(int value)
        {
            Max = value;
            return this;
        }
        public AnimationLinearConfig SetAdd(int value)
        {
            Add = value;
            return this;
        }
        public AnimationLinearConfig SetValue(int? value)
        {
            Value = value;
            return this;
        }
        public AnimationLinearConfig SetValueColor(int value, bool sw, int add, int? max = null, int? min = null)
        {
            Value = value;
            if (sw)
            {
                Add = add;
                Max = max ?? 255;
            }
            else
            {
                Add = -add;
                Max = min ?? 0;
            }
            return this;
        }
        public AnimationLinearConfig SetValueColor(int value, int add)
        {
            Value = value;
            Add = add;
            if (add > 0) Max = 255;
            else Max = 0;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// 线性增量动画 float
    /// </summary>
    public class AnimationLinearFConfig : IAnimationConfig
    {
        /// <summary>
        /// 线性增量动画 float
        /// </summary>
        /// <param name="control">句柄</param>
        /// <param name="call">任务</param>
        /// <param name="interval">动画间隔</param>
        /// <param name="max">最大值</param>
        /// <param name="add">更新量</param>
        public AnimationLinearFConfig(Control control, Func<float, bool> call, int interval, int max, int add)
        {
            Control = control;
            Call = call;
            Interval = interval;
            Max = max;
            Add = add;
        }

        /// <summary>
        /// 线性增量动画 float
        /// </summary>
        /// <param name="control">句柄</param>
        /// <param name="call">任务</param>
        /// <param name="interval">动画间隔</param>
        public AnimationLinearFConfig(Control control, Func<float, bool> call, int interval)
        {
            Control = control;
            Call = call;
            Interval = interval;
        }

        /// <summary>
        /// 句柄
        /// </summary>
        public Control Control { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        public Func<float, bool> Call { get; set; }

        /// <summary>
        /// 动画间隔
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// 更新量
        /// </summary>
        public float Add { get; set; }

        public float? Value { get; set; }

        #region 设置

        public AnimationLinearFConfig SetControl(Control value)
        {
            Control = value;
            return this;
        }
        public AnimationLinearFConfig SetCall(Func<float, bool> value)
        {
            Call = value;
            return this;
        }
        public AnimationLinearFConfig SetInterval(int value)
        {
            Interval = value;
            return this;
        }
        public AnimationLinearFConfig SetMax(float value)
        {
            Max = value;
            return this;
        }
        public AnimationLinearFConfig SetAdd(float value)
        {
            Add = value;
            return this;
        }
        public AnimationLinearFConfig SetValue(float? value)
        {
            Value = value;
            return this;
        }
        public AnimationLinearFConfig SetValue(float value, bool sw, float add)
        {
            Value = value;
            if (sw)
            {
                Add = add;
                Max = 1F;
            }
            else
            {
                Add = -add;
                Max = 0F;
            }
            return this;
        }
        public AnimationLinearFConfig SetValue(float value, float add)
        {
            Value = value;
            Add = add;
            if (add > 0) Max = 1F;
            else Max = 0F;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// 循环动画
    /// </summary>
    public class AnimationLoopConfig : IAnimationConfig
    {
        public AnimationLoopConfig(Control control, Func<bool> call, int interval)
        {
            Control = control;
            Call = call;
            Interval = interval;
        }
        /// <summary>
        /// 句柄
        /// </summary>
        public Control Control { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        public Func<bool> Call { get; set; }

        /// <summary>
        /// 动画间隔
        /// </summary>
        public int Interval { get; set; }

        #region 设置

        public AnimationLoopConfig SetControl(Control value)
        {
            Control = value;
            return this;
        }
        public AnimationLoopConfig SetCall(Func<bool> value)
        {
            Call = value;
            return this;
        }
        public AnimationLoopConfig SetInterval(int value)
        {
            Interval = value;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// 固定帧率动画
    /// </summary>
    public class AnimationFixedConfig : IAnimationConfig
    {
        public AnimationFixedConfig(Action<float> call, int interval, int totalFrames, bool sw, AnimationType value = AnimationType.Ball)
        {
            Call = call;
            Interval = interval;
            TotalFrames = totalFrames;
            LR = sw;
            Type = value;
        }
        /// <summary>
        /// 任务
        /// </summary>
        public Action<float> Call { get; set; }

        /// <summary>
        /// 动画间隔
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 总帧数
        /// </summary>
        public int TotalFrames { get; set; }

        public bool LR { get; set; }
        public AnimationType Type { get; set; }

        public bool Arrow { get; set; }

        #region 设置

        public AnimationFixedConfig SetCall(Action<float> value)
        {
            Call = value;
            return this;
        }
        public AnimationFixedConfig SetInterval(int value)
        {
            Interval = value;
            return this;
        }
        public AnimationFixedConfig SetTotalFrames(int value)
        {
            TotalFrames = value;
            return this;
        }
        public AnimationFixedConfig SetArrow(bool value = true)
        {
            Arrow = value;
            return this;
        }

        public AnimationFixedConfig SetType(bool sw, AnimationType value = AnimationType.Ball)
        {
            LR = sw;
            Type = value;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// 固定帧率动画
    /// </summary>
    public class AnimationFixed2Config : IAnimationConfig
    {
        public AnimationFixed2Config(Action<int, float> call, int interval, int totalFrames, float value, bool sw, AnimationType type = AnimationType.Ball)
        {
            Call = call;
            Interval = interval;
            TotalFrames = totalFrames;
            Value = value;
            LR = sw;
            Type = type;
        }
        public AnimationFixed2Config(Action<int, float, float> call, int interval, int totalFrames, float value, bool sw, AnimationType type = AnimationType.Ball)
        {
            Call = call;
            Interval = interval;
            TotalFrames = totalFrames;
            Value = value;
            LR = sw;
            Type = type;
        }
        /// <summary>
        /// 任务
        /// </summary>
        public object Call { get; set; }

        /// <summary>
        /// 动画间隔
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 总帧数
        /// </summary>
        public int TotalFrames { get; set; }

        public bool LR { get; set; }
        public AnimationType Type { get; set; }

        public float Value { get; set; }

        #region 设置

        public AnimationFixed2Config SetCall(Action<int, float> value)
        {
            Call = value;
            return this;
        }
        public AnimationFixed2Config SetCall(Action<int, float, float> value)
        {
            Call = value;
            return this;
        }
        public AnimationFixed2Config SetInterval(int value)
        {
            Interval = value;
            return this;
        }
        public AnimationFixed2Config SetTotalFrames(int value)
        {
            TotalFrames = value;
            return this;
        }
        public AnimationFixed2Config SetValue(float value)
        {
            Value = value;
            return this;
        }

        public AnimationFixed2Config SetType(bool sw, AnimationType value = AnimationType.Ball)
        {
            LR = sw;
            Type = value;
            return this;
        }

        #endregion
    }

    public class IAnimationConfig
    {
        /// <summary>
        /// 运行之前是否睡眠
        /// </summary>
        public int Sleep { get; set; }

        /// <summary>
        /// 结束模式
        /// </summary>
        public bool EndRCall { get; set; }

        /// <summary>
        /// 结束后回调
        /// </summary>
        public Action? CallEnd { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public TPriority Priority { get; set; } = TPriority.High;

        #region 设置

        public IAnimationConfig SetSleep(int value = 100)
        {
            Sleep = value;
            return this;
        }
        public IAnimationConfig SetEndRCall(bool value = true)
        {
            EndRCall = value;
            return this;
        }
        public IAnimationConfig SetEnd(Action? value)
        {
            CallEnd = value;
            return this;
        }
        public IAnimationConfig SetPriority(TPriority value = TPriority.Pool)
        {
            Priority = value;
            return this;
        }

        #endregion
    }

    public enum TPriority
    {
        /// <summary>
        /// 高优先级
        /// </summary>
        High,
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 线程池
        /// </summary>
        Pool
    }

    #endregion
}