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
                Style.Db = value == TMode.Light ? new Theme.Light() : new Theme.Dark();
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

        /// <summary>
        /// 启用动画
        /// </summary>
        public static bool Animation { get; set; } = true;

        /// <summary>
        /// 阴影使能
        /// </summary>
        public static bool ShadowEnabled { get; set; } = true;

        /// <summary>
        /// 弹出是否在窗口里而不是在系统里（Message/Notification）
        /// </summary>
        public static bool ShowInWindow { get; set; }

        /// <summary>
        /// 通知消息边界偏移量XY（Message/Notification）
        /// </summary>
        public static int NoticeWindowOffsetXY { get; set; }

        /// <summary>
        /// 文本呈现的质量
        /// </summary>
        public static System.Drawing.Text.TextRenderingHint? TextRenderingHint { get; set; } = null;

        /// <summary>
        /// 默认字体
        /// </summary>
        public static Font? Font { get; set; } = null;

        /// <summary>
        /// 滚动条隐藏样式
        /// </summary>
        public static bool ScrollBarHide { get; set; }

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

        internal const string NullText = "龍Qq";

        internal static void SetDpi(float dpi)
        {
            dpione = false;
            if (_dpi == dpi) return;
            _dpi = dpi;
            if (!_dpi_custom.HasValue) EventHub.Dispatch(EventType.DPI, dpi);
        }

        internal static void SetDpi(Graphics g)
        {
            SetDpi(g.DpiX / 96F);
        }
    }
}