// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    /// <summary>
    /// 动画
    /// </summary>
    public static class Animation
    {
        /// <summary>
        /// 返回总帧数
        /// </summary>
        /// <param name="interval">动画间隔</param>
        /// <param name="lastTime">动画时长（ms）</param>
        /// <returns>动画总帧数</returns>
        public static int TotalFrames(int interval, int lastTime) => lastTime % interval > 0 ? lastTime / interval + 1 : lastTime / interval;

        /// <summary>
        /// 执行动画
        /// </summary>
        /// <param name="currentFrames">当前帧</param>
        /// <param name="totalFrames">总帧</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="type">动画类型</param>
        /// <returns>当前数值</returns>
        public static float Animate(double currentFrames, double totalFrames, float maxValue, AnimationType type) => Animate(currentFrames / totalFrames, maxValue, type);

        /// <summary>
        /// 执行动画
        /// </summary>
        /// <param name="progress">帧进度</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="type">动画类型</param>
        /// <returns>当前数值</returns>
        public static float Animate(double progress, float maxValue, AnimationType type) => (float)(maxValue * type.CalculateValue(progress));
        public static double Animate(double progress, double maxValue, AnimationType type) => maxValue * type.CalculateValue(progress);

        internal static double CalculateValue(this AnimationType type, double v)
        {
            switch (type)
            {
                case AnimationType.Liner:
                    return v;
                case AnimationType.Ease:
                    return Math.Sqrt(v);
                case AnimationType.Ball:
                    return Math.Sqrt(1.0 - Math.Pow(v - 1, 2));
                case AnimationType.Resilience:
                    return -10.0 / 6.0 * v * (v - 1.6);
                default: return 1;
            }
        }
    }

    /// <summary>
    /// 动画类型
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// 以同一速度移动
        /// </summary>
        Liner,
        /// <summary>
        /// 逐渐减速
        /// </summary>
        Ease,
        /// <summary>
        /// 加速并减速
        /// </summary>
        Ball,
        /// <summary>
        /// 弹性一样的动画
        /// </summary>
        Resilience
    }
}