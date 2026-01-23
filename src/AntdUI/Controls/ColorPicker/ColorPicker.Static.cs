// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class ColorPicker
    {
        /// <summary>
        /// ColorPicker 颜色选择器
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="call">回调</param>
        public static Form? open(Control control, Action<Color> call) => open(new Config(new Target(control), call));

        /// <summary>
        /// ColorPicker 颜色选择器
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="call">回调</param>
        /// <param name="def">默认颜色</param>
        public static Form? open(Control control, Action<Color> call, Color def) => open(new Config(new Target(control), call).SetValue(def));

        /// <summary>
        /// ColorPicker 颜色选择器
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="call">回调</param>
        public static Form? open(Target target, Action<Color> call) => open(new Config(target, call));

        /// <summary>
        /// ColorPicker 颜色选择器
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="call">回调</param>
        /// <param name="def">默认颜色</param>
        public static Form? open(Target target, Action<Color> call, Color def) => open(new Config(target, call).SetValue(def));

        /// <summary>
        /// ColorPicker 颜色选择器
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(Config config)
        {
            if (config.Target.IsCreated(out var invoke, out var obj))
            {
                if (invoke) return ITask.Invoke(obj!, new Func<Form?>(() => open(config)));
                var frm = new LayeredFormColorPicker(config);
                config.Target.Show(frm);
                return frm;
            }
            return null;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config : IColorPicker
        {
            /// <summary>
            /// ColorPicker 配置
            /// </summary>
            /// <param name="target">所属目标</param>
            public Config(Target target)
            {
                Target = target;
            }

            /// <summary>
            /// ColorPicker 配置
            /// </summary>
            /// <param name="target">所属目标</param>
            /// <param name="call">确认回调</param>
            public Config(Target target, Action<Color> call)
            {
                Target = target;
                Call = call;
            }

            /// <summary>
            /// 所属目标
            /// </summary>
            public Target Target { get; set; }

            /// <summary>
            /// 颜色的值
            /// </summary>
            public Color? Value { get; set; }

            /// <summary>
            /// 颜色模式
            /// </summary>
            public TColorMode Mode { get; set; } = TColorMode.Hex;

            /// <summary>
            /// 禁用透明度
            /// </summary>
            public bool DisabledAlpha { get; set; }

            /// <summary>
            /// 支持清除
            /// </summary>
            public bool AllowClear { get; set; }

            /// <summary>
            /// 显示关闭按钮
            /// </summary>
            public bool ShowClose { get; set; }

            /// <summary>
            /// 显示还原按钮
            /// </summary>
            public bool ShowReset { get; set; }

            /// <summary>
            /// 预设的颜色
            /// </summary>
            public Color[]? Presets { get; set; }

            /// <summary>
            /// 确定回调
            /// </summary>
            public Action<Color>? Call { get; set; }

            /// <summary>
            /// 弹出方向
            /// </summary>
            public TAlignFrom Placement { get; set; } = TAlignFrom.BL;

            /// <summary>
            /// 偏移量
            /// </summary>
            public Rectangle? Offset { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 字体比例
            /// </summary>
            public float FontRatio { get; set; } = 0.9F;

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            /// <summary>
            /// 箭头大小
            /// </summary>
            public int ArrowSize { get; set; } = 8;

            /// <summary>
            /// 色彩模式
            /// </summary>
            public TAMode ColorScheme { get; set; } = TAMode.Auto;

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            #region 设置

            public Config SetValue(Color? value)
            {
                Value = value;
                return this;
            }
            public Config SetMode(TColorMode value)
            {
                Mode = value;
                return this;
            }
            public Config SetDisabledAlpha(bool value = true)
            {
                DisabledAlpha = value;
                return this;
            }
            public Config SetAllowClear(bool value = true)
            {
                AllowClear = value;
                return this;
            }
            public Config SetShowClose(bool value = true)
            {
                ShowClose = value;
                return this;
            }
            public Config SetShowReset(bool value = true)
            {
                ShowReset = value;
                return this;
            }
            public Config SetPresets(params Color[] value)
            {
                Presets = value;
                return this;
            }
            public Config ClearPresets()
            {
                Presets = null;
                return this;
            }

            public Config SetPlacement(TAlignFrom value)
            {
                Placement = value;
                return this;
            }
            public Config SetAlign(TAlignFrom value) => SetPlacement(value);

            public Config SetOffset(Rectangle? value)
            {
                Offset = value;
                return this;
            }
            public Config SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public Config SetFont(float value)
            {
                FontRatio = value;
                return this;
            }
            public Config SetRadius(int value = 0)
            {
                Radius = value;
                return this;
            }
            public Config SetArrow(int value)
            {
                ArrowSize = value;
                return this;
            }
            public Config SetColorScheme(TAMode value)
            {
                ColorScheme = value;
                return this;
            }
            public Config SetTag(object? value)
            {
                Tag = value;
                return this;
            }
            public Config SetCall(Action<Color>? value)
            {
                Call = value;
                return this;
            }

            #endregion
        }
    }

    internal interface IColorPicker
    {
        bool DisabledAlpha { get; set; }
        bool AllowClear { get; set; }
        bool ShowClose { get; set; }
        bool ShowReset { get; set; }
        Color[]? Presets { get; set; }
        TColorMode Mode { get; set; }
        TAMode ColorScheme { get; set; }
        TAlignFrom Placement { get; set; }
    }
}