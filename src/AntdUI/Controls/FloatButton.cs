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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// FloatButton 悬浮按钮
    /// </summary>
    /// <remarks>悬浮按钮。</remarks>
    public static class FloatButton
    {
        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static FormFloatButton? open(Form form, ConfigBtn[] btns, Action<ConfigBtn> call) => open(new Config(form, btns, call));

        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static FormFloatButton? open(Form form, Control content, ConfigBtn[] btns, Action<ConfigBtn> call)
        {
            return open(new Config(form, btns, call)
            {
                Control = content
            });
        }

        /// <summary>
        /// FloatButton 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Config config(Form form, ConfigBtn[] btns, Action<ConfigBtn> call) => new Config(form, btns, call);

        /// <summary>
        /// FloatButton 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Config config(Form form, Control content, ConfigBtn[] btns, Action<ConfigBtn> call)
        {
            return new Config(form, btns, call)
            {
                Control = content
            };
        }

        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="config">配置</param>
        public static FormFloatButton? open(this Config config)
        {
            if (config.Form.IsHandleCreated)
            {
                if (config.Form.InvokeRequired) return ITask.Invoke(config.Form, new Func<FormFloatButton?>(() => open(config)));
                var floatButton = new LayeredFormFloatButton(config);
                floatButton.Show(config.Form);
                return floatButton;
            }
            return null;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="btns">按钮</param>
            /// <param name="call">回调</param>
            public Config(Form form, ConfigBtn[] btns, Action<ConfigBtn> call)
            {
                Form = form;
                Btns = btns;
                Call = call;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 所属控件
            /// </summary>
            public Control? Control { get; set; }

            /// <summary>
            /// 方向
            /// </summary>
            public TAlign Align { get; set; } = TAlign.BR;

            /// <summary>
            /// 是否垂直方向
            /// </summary>
            public bool Vertical { get; set; } = true;

            /// <summary>
            /// 是否置顶
            /// </summary>
            public bool TopMost { get; set; }

            /// <summary>
            /// 大小
            /// </summary>
            public int Size { get; set; } = 40;

            /// <summary>
            /// 边距X
            /// </summary>
            public int MarginX { get; set; } = 24;

            /// <summary>
            /// 边距Y
            /// </summary>
            public int MarginY { get; set; } = 24;

            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; } = 40;

            /// <summary>
            /// 按钮列表
            /// </summary>
            public ConfigBtn[] Btns { get; set; }

            /// <summary>
            /// 点击回调
            /// </summary>
            public Action<ConfigBtn> Call { get; set; }
        }

        /// <summary>
        /// 配置 按钮
        /// </summary>
        public class ConfigBtn : NotifyProperty, BadgeConfig
        {
            /// <summary>
            /// 配置 按钮
            /// </summary>
            /// <param name="name">名称</param>
            public ConfigBtn(string name)
            {
                Name = name;
            }

            /// <summary>
            /// 配置 按钮
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="icon">图标</param>
            public ConfigBtn(string name, Image icon)
            {
                Name = name;
                Icon = icon;
            }

            /// <summary>
            /// 配置 按钮
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="text">标题/SVG</param>
            /// <param name="isSVG">是否SVG</param>
            public ConfigBtn(string name, string text, bool isSVG = false)
            {
                Name = name;
                if (isSVG) IconSvg = text;
                else Text = text;
            }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            bool enabled = true;
            /// <summary>
            /// 使能
            /// </summary>
            public bool Enabled
            {
                get => enabled;
                set
                {
                    if (enabled == value) return;
                    enabled = value;
                    OnPropertyChanged("Enabled");
                }
            }

            bool loading = false;
            internal int AnimationLoadingValue = 0;
            /// <summary>
            /// 加载
            /// </summary>
            public bool Loading
            {
                get => loading;
                set
                {
                    if (loading == value) return;
                    loading = value;
                    OnPropertyChanged("Loading");
                }
            }

            /// <summary>
            /// 加载进度
            /// </summary>
            public float LoadingValue { get; set; } = .3F;

            Color? fore;
            /// <summary>
            /// 文字颜色
            /// </summary>
            public Color? Fore
            {
                get => fore;
                set
                {
                    if (fore == value) return;
                    fore = value;
                    OnPropertyChanged("Fore");
                }
            }

            Image? icon;
            /// <summary>
            /// 自定义图标
            /// </summary>
            public Image? Icon
            {
                get => icon;
                set
                {
                    if (icon == value) return;
                    icon = value;
                    OnPropertyChanged("Icon");
                }
            }

            string? iconSvg;
            /// <summary>
            /// 自定义图标SVG
            /// </summary>
            public string? IconSvg
            {
                get => iconSvg;
                set
                {
                    if (iconSvg == value) return;
                    iconSvg = value;
                    OnPropertyChanged("IconSvg");
                }
            }

            Size? iconSize;
            /// <summary>
            /// 图标大小
            /// </summary>
            public Size? IconSize
            {
                get => iconSize;
                set
                {
                    if (iconSize == value) return;
                    iconSize = value;
                    OnPropertyChanged("IconSize");
                }
            }

            string? text = null;
            /// <summary>
            /// 文字及其它内容
            /// </summary>
            public string? Text
            {
                get => Localization.GetLangI(LocalizationText, text, new string?[] { "{id}", Name });
                set
                {
                    if (text == value) return;
                    text = value;
                    OnPropertyChanged("Text");
                }
            }

            /// <summary>
            /// 国际化（文本）
            /// </summary>
            public string? LocalizationText { get; set; }

            /// <summary>
            /// 气泡的内容
            /// </summary>
            public string? Tooltip { get; set; }

            TTypeMini type = TTypeMini.Default;
            /// <summary>
            /// 设置按钮类型
            /// </summary>
            public TTypeMini Type
            {
                get => type;
                set
                {
                    if (type == value) return;
                    type = value;
                    OnPropertyChanged("Type");
                }
            }

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            bool round = true;
            /// <summary>
            /// 圆角样式
            /// </summary>
            public bool Round
            {
                get => round;
                set
                {
                    if (round == value) return;
                    round = value;
                    OnPropertyChanged("Round");
                }
            }

            string? badge;
            /// <summary>
            /// 徽标文本
            /// </summary>
            public string? Badge
            {
                get => badge;
                set
                {
                    if (badge == value) return;
                    badge = value;
                    OnPropertyChanged("Badge");
                }
            }

            string? badgeSvg = null;
            /// <summary>
            /// 徽标SVG
            /// </summary>
            public string? BadgeSvg
            {
                get => badgeSvg;
                set
                {
                    if (badgeSvg == value) return;
                    badgeSvg = value;
                    OnPropertyChanged("Badge");
                }
            }

            TAlignFrom badgeAlign = TAlignFrom.TR;
            /// <summary>
            /// 徽标方向
            /// </summary>
            public TAlignFrom BadgeAlign
            {
                get => badgeAlign;
                set
                {
                    if (badgeAlign == value) return;
                    badgeAlign = value;
                    OnPropertyChanged("Badge");
                }
            }

            /// <summary>
            /// 徽标大小
            /// </summary>
            public float BadgeSize { get; set; } = .6F;

            bool badgeMode = false;
            /// <summary>
            /// 徽标模式（镂空）
            /// </summary>
            public bool BadgeMode
            {
                get => badgeMode;
                set
                {
                    if (badgeMode == value) return;
                    badgeMode = value;
                    OnPropertyChanged("BadgeMode");
                }
            }

            /// <summary>
            /// 徽标偏移X
            /// </summary>
            public int BadgeOffsetX { get; set; }

            /// <summary>
            /// 徽标偏移Y
            /// </summary>
            public int BadgeOffsetY { get; set; }

            /// <summary>
            /// 徽标背景颜色
            /// </summary>
            public Color? BadgeBack { get; set; }

            #region 内部

            internal bool hover = false;
            internal Rectangle rect;
            internal Rectangle rect_read;
            internal Rectangle rect_icon;
            internal Bitmap? shadow_temp = null;

            #endregion
        }
    }
}