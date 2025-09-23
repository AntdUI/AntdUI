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
    /// <summary>
    /// FloatButton 悬浮按钮
    /// </summary>
    /// <remarks>悬浮按钮。</remarks>
    public static class FloatButton
    {
        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static FormFloatButton? open(Target target, ConfigBtn[] btns, Action<ConfigBtn> call) => open(new Config(target, btns, call));

        /// <summary>
        /// FloatButton 配置
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Config config(Target target, ConfigBtn[] btns, Action<ConfigBtn> call) => new Config(target, btns, call);

        #region 窗口

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
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static FormFloatButton? open(Control content, ConfigBtn[] btns, Action<ConfigBtn> call) => open(new Config(content, btns, call));

        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        [Obsolete]
        public static FormFloatButton? open(Form form, Control content, ConfigBtn[] btns, Action<ConfigBtn> call) => open(new Config(content, btns, call));

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
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Config config(Control content, ConfigBtn[] btns, Action<ConfigBtn> call) => new Config(content, btns, call);

        /// <summary>
        /// FloatButton 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        [Obsolete]
        public static Config config(Form form, Control content, ConfigBtn[] btns, Action<ConfigBtn> call) => new Config(content, btns, call);

        #endregion

        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="config">配置</param>
        public static FormFloatButton? open(this Config config)
        {
            if (config.Target.IsCreated(out var invoke, out var obj))
            {
                if (invoke) return ITask.Invoke(obj!, new Func<FormFloatButton?>(() => open(config)));
                var floatButton = new LayeredFormFloatButton(config);
                config.Target.Show(floatButton);
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
            /// FloatButton 配置
            /// </summary>
            /// <param name="target">目标</param>
            /// <param name="btns">按钮</param>
            /// <param name="call">回调</param>
            public Config(Target target, ConfigBtn[] btns, Action<ConfigBtn> call)
            {
                Target = target;
                Btns = btns;
                Call = call;
            }

            /// <summary>
            /// FloatButton 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="btns">按钮</param>
            /// <param name="call">回调</param>
            public Config(Form form, ConfigBtn[] btns, Action<ConfigBtn> call) : this(new Target(form), btns, call) { }

            /// <summary>
            /// FloatButton 配置
            /// </summary>
            /// <param name="control">所属控件</param>
            /// <param name="btns">按钮</param>
            /// <param name="call">回调</param>
            public Config(Control control, ConfigBtn[] btns, Action<ConfigBtn> call) : this(new Target(control), btns, call) { }

            /// <summary>
            /// 所属目标
            /// </summary>
            public Target Target { get; set; }

            /// <summary>
            /// 所属窗口
            /// </summary>
            [Obsolete("use Target")]
            public Form Form => Target.GetForm!;

            /// <summary>
            /// 所属控件
            /// </summary>
            [Obsolete("use Target")]
            public Control? Control => Target.GetControl!;

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

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

            #region 设置

            public Config SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public Config SetAlign(TAlign value = TAlign.BL)
            {
                Align = value;
                return this;
            }
            public Config SetVertical(bool value = false)
            {
                Vertical = value;
                return this;
            }
            public Config SetSize(bool value = true)
            {
                TopMost = value;
                return this;
            }
            public Config SetSize(int value)
            {
                Size = value;
                return this;
            }
            public Config SetMargin(int x, int y)
            {
                MarginX = x;
                MarginY = y;
                return this;
            }
            public Config SetMarginX(int value)
            {
                MarginX = value;
                return this;
            }
            public Config SetMarginY(int value)
            {
                MarginY = value;
                return this;
            }
            public Config SetGap(int value)
            {
                Gap = value;
                return this;
            }
            public Config SetBtns(params ConfigBtn[] value)
            {
                Btns = value;
                return this;
            }
            public Config SetCall(Action<ConfigBtn> value)
            {
                Call = value;
                return this;
            }

            #endregion
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
                    OnPropertyChanged(nameof(Enabled));
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
                    OnPropertyChanged(nameof(Loading));
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
                    OnPropertyChanged(nameof(Fore));
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
                    OnPropertyChanged(nameof(Icon));
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
                    OnPropertyChanged(nameof(IconSvg));
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
                    OnPropertyChanged(nameof(IconSize));
                }
            }

            string? text;
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
                    OnPropertyChanged(nameof(Text));
                }
            }

            /// <summary>
            /// 国际化（文本）
            /// </summary>
            public string? LocalizationText { get; set; }

            string? tooltip;
            /// <summary>
            /// 气泡提示
            /// </summary>
            public string? Tooltip
            {
                get => Localization.GetLangI(LocalizationTooltip, tooltip, new string?[] { "{id}", Name });
                set => tooltip = value;
            }

            /// <summary>
            /// 国际化（气泡提示）
            /// </summary>
            public string? LocalizationTooltip { get; set; }

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
                    OnPropertyChanged(nameof(Type));
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
                    OnPropertyChanged(nameof(Round));
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
                    OnPropertyChanged(nameof(Badge));
                }
            }

            string? badgeSvg;
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
                    OnPropertyChanged(nameof(Badge));
                }
            }

            TAlign badgeAlign = TAlign.TR;
            /// <summary>
            /// 徽标方向
            /// </summary>
            public TAlign BadgeAlign
            {
                get => badgeAlign;
                set
                {
                    if (badgeAlign == value) return;
                    badgeAlign = value;
                    OnPropertyChanged(nameof(Badge));
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
                    OnPropertyChanged(nameof(BadgeMode));
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
            internal Bitmap? shadow_temp;

            #endregion

            #region 设置

            public ConfigBtn SetFore(Color? value)
            {
                fore = value;
                return this;
            }

            public ConfigBtn SetText(string? value, string? localization = null)
            {
                LocalizationText = localization;
                text = value;
                return this;
            }

            public ConfigBtn SetTooltip(string? value, string? localization = null)
            {
                LocalizationTooltip = localization;
                tooltip = value;
                return this;
            }

            public ConfigBtn SetType(TTypeMini value = TTypeMini.Primary)
            {
                type = value;
                return this;
            }

            public ConfigBtn SetRadius(int value = 0)
            {
                Radius = value;
                return this;
            }

            public ConfigBtn SetRound(bool value = false)
            {
                round = value;
                return this;
            }

            public ConfigBtn SetEnabled(bool value = false)
            {
                enabled = value;
                return this;
            }

            public ConfigBtn SetLoading(bool value = true, float size = .3F)
            {
                loading = value;
                LoadingValue = size;
                return this;
            }

            public ConfigBtn SetLoadingValue(float value = 0F)
            {
                LoadingValue = value;
                return this;
            }

            #region 图标

            public ConfigBtn SetIcon(Image? img, Size? size = null)
            {
                icon = img;
                iconSize = size;
                return this;
            }

            public ConfigBtn SetIcon(string? svg, Size? size = null)
            {
                iconSvg = svg;
                iconSize = size;
                return this;
            }

            #endregion

            #region 徽标

            public ConfigBtn SetBadge(string? value = " ", TAlign align = TAlign.TR)
            {
                badge = value;
                badgeAlign = align;
                return this;
            }
            public ConfigBtn SetBadgeSvg(string? value, TAlign align = TAlign.TR)
            {
                badgeSvg = value;
                badgeAlign = align;
                return this;
            }
            public ConfigBtn SetBadgeOffset(int x, int y)
            {
                BadgeOffsetX = x;
                BadgeOffsetY = y;
                return this;
            }
            public ConfigBtn SetBadgeSize(float value)
            {
                BadgeSize = value;
                return this;
            }
            public ConfigBtn SetBadgeBack(Color? value)
            {
                BadgeBack = value;
                return this;
            }

            #endregion

            #endregion

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }
        }
    }
}