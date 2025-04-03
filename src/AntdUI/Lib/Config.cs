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

using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public class Config
    {
        #region 色彩模式

        static TMode mode = TMode.Light;
        /// <summary>
        /// 色彩模式
        /// </summary>
        public static TMode Mode
        {
            get => mode;
            set
            {
                mode = value;
                EventHub.Dispatch(EventType.THEME, value);
            }
        }

        public static bool IsLight
        {
            get => mode == TMode.Light;
            set
            {
                Mode = value ? TMode.Light : TMode.Dark;
                EventHub.Dispatch(EventType.THEME, value);
            }
        }
        public static bool IsDark
        {
            get => mode == TMode.Dark;
            set
            {
                Mode = value ? TMode.Dark : TMode.Light;
                EventHub.Dispatch(EventType.THEME, value);
            }
        }

        #endregion

        #region 动画使能

        /// <summary>
        /// 动画使能
        /// </summary>
        public static bool Animation { get; set; } = true;

        internal static List<string>? AnimationData;

        /// <summary>
        /// 启用动画
        /// </summary>
        public static void EnableAnimation(params string[] controls)
        {
            if (AnimationData == null) return;
            foreach (var it in controls) AnimationData.Remove(it);
        }

        /// <summary>
        /// 禁用动画
        /// </summary>
        public static void DisableAnimation(params string[] controls)
        {
            if (AnimationData == null) AnimationData = new List<string>(controls);
            else
            {
                foreach (var it in controls)
                {
                    if (AnimationData.Contains(it)) continue;
                    AnimationData.Add(it);
                }
            }
        }

        /// <summary>
        /// 清除动画数据
        /// </summary>
        public static void ClearAnimationData() => AnimationData = null;

        public static bool HasAnimation(string control)
        {
            if (Animation)
            {
                if (AnimationData == null) return true;
                if (AnimationData.Contains(control)) return false;
                return true;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// 触屏使能
        /// </summary>
        public static bool TouchEnabled { get; set; } = true;

        /// <summary>
        /// 触屏阈值
        /// </summary>
        public static int TouchThreshold { get; set; } = 20;

        /// <summary>
        /// 触屏点击使能
        /// </summary>
        public static bool TouchClickEnabled { get; set; }

        /// <summary>
        /// 阴影使能
        /// </summary>
        public static bool ShadowEnabled { get; set; } = true;

        #region 弹出在窗口

        /// <summary>
        /// 弹出是否在窗口里而不是在系统里（Message/Notification）
        /// </summary>
        public static bool ShowInWindow { get; set; }

        /// <summary>
        /// 弹出是否在窗口里而不是在系统里（Message）
        /// </summary>
        public static bool ShowInWindowByMessage { get; set; }

        /// <summary>
        /// 弹出是否在窗口里而不是在系统里（Notification）
        /// </summary>
        public static bool ShowInWindowByNotification { get; set; }

        #endregion

        /// <summary>
        /// 通知消息边界偏移量XY（Message/Notification）
        /// </summary>
        public static int NoticeWindowOffsetXY { get; set; }

        /// <summary>
        /// 文本呈现的质量
        /// </summary>
        public static System.Drawing.Text.TextRenderingHint? TextRenderingHint { get; set; }

        /// <summary>
        /// 文本高质量呈现
        /// </summary>
        public static bool TextRenderingHighQuality { get; set; }

        /// <summary>
        /// 默认字体
        /// </summary>
        public static Font? Font { get; set; }

        #region 滚动条

        /// <summary>
        /// 滚动条隐藏样式
        /// </summary>
        public static bool ScrollBarHide { get; set; }

        /// <summary>
        /// 滚动条最小大小Y
        /// </summary>
        public static int ScrollMinSizeY { get; set; } = 30;

        #endregion

        #region DPI

        static bool dpione = true;
        static float _dpi = 1F;
        static float? _dpi_custom;
        public static float Dpi
        {
            get
            {
                if (dpione) Helper.GDI(g => g.DpiX);
                if (_dpi_custom.HasValue) return _dpi_custom.Value;
                return _dpi;
            }
        }

        /// <summary>
        /// 自定义DPI
        /// </summary>
        /// <param name="dpi">值</param>
        public static void SetDpi(float? dpi)
        {
            if (_dpi_custom == dpi) return;
            _dpi_custom = dpi;
            if (dpi.HasValue) EventHub.Dispatch(EventType.DPI, dpi.Value);
            else EventHub.Dispatch(EventType.DPI, _dpi);
        }

        #endregion

        public const string NullText = "龍Qq";

        internal static void SetDpi(float dpi)
        {
            dpione = false;
            if (_dpi == dpi) return;
            _dpi = dpi;
            if (!_dpi_custom.HasValue) EventHub.Dispatch(EventType.DPI, dpi);
        }

        internal static void SetDpi(Graphics g) => SetDpi(g.DpiX / 96F);

        /// <summary>
        /// 设置修正文本渲染
        /// </summary>
        /// <param name="families">需要修正的字体列表</param>
        public static void SetCorrectionTextRendering(params string[] families)
        {
            foreach (var it in families) CorrectionTextRendering.Set(it);
        }

        #region 空白图

        public static float EmptyImageRatio = 2.98F;
        internal static string[]? EmptyImageSvg;
        /// <summary>
        /// 设置空白图片
        /// </summary>
        /// <param name="light">浅色</param>
        /// <param name="dark">深色</param>
        public static void SetEmptyImageSvg(string light, string dark) => EmptyImageSvg = new string[] { light, dark };
        public static void ClearEmptyImageSvg() => EmptyImageSvg = null;

        #endregion
    }
}