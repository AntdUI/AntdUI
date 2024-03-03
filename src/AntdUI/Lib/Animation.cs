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
        public static int TotalFrames(int interval, int lastTime)
        {
            return lastTime % interval > 0 ? lastTime / interval + 1 : lastTime / interval;
        }

        /// <summary>
        /// 执行动画
        /// </summary>
        /// <param name="currentFrames">当前帧</param>
        /// <param name="totalFrames">总帧</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="type">动画类型</param>
        /// <returns>当前数值</returns>
        public static float Animate(double currentFrames, double totalFrames, float maxValue, AnimationType type)
        {
            return Animate(currentFrames / totalFrames, maxValue, type);
        }

        /// <summary>
        /// 执行动画
        /// </summary>
        /// <param name="progress">帧进度</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="type">动画类型</param>
        /// <returns>当前数值</returns>
        public static float Animate(double progress, float maxValue, AnimationType type)
        {
            return (float)(maxValue * type.CalculateValue(progress));
        }
        public static double Animate(double progress, double maxValue, AnimationType type)
        {
            return maxValue * type.CalculateValue(progress);
        }

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