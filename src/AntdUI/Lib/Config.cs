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
                EventManager.Instance.Dispatch(1);
            }
        }

        public static bool IsLight
        {
            get => mode == TMode.Light;
            set
            {
                Mode = value ? TMode.Light : TMode.Dark;
                EventManager.Instance.Dispatch(1);
            }
        }
        public static bool IsDark
        {
            get => mode == TMode.Dark;
            set
            {
                Mode = value ? TMode.Dark : TMode.Light;
                EventManager.Instance.Dispatch(1);
            }
        }

        #endregion

        /// <summary>
        /// 启用动画
        /// </summary>
        public static bool Animation { get; set; } = true;

        /// <summary>
        /// 弹出是否在窗口里而不是在系统里（Message/Notification）
        /// </summary>
        public static bool ShowInWindow { get; set; } = false;

        /// <summary>
        /// 文本呈现的质量
        /// </summary>
        public static System.Drawing.Text.TextRenderingHint? TextRenderingHint { get; set; } = null;

        /// <summary>
        /// 默认字体
        /// </summary>
        public static Font? Font { get; set; } = null;

        public static float Dpi { get; private set; } = 1F;

        internal const string NullText = "Qq";

        internal static void SetDpi(float dpi)
        {
            Dpi = dpi;
        }

        internal static void SetDpi(Graphics g)
        {
            Dpi = g.DpiX / 96F;
        }
    }
}