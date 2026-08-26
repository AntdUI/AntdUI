// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI.Theme
{
    public class IColor
    {
        /// <summary>
        /// 品牌色
        /// </summary>
        public Color Primary => Style.Get(Colour.Primary);

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color PrimaryColor => Style.Get(Colour.PrimaryColor);

        /// <summary>
        /// 主色悬浮态（按钮、开关、复选框）
        /// </summary>
        public Color PrimaryHover => Style.Get(Colour.PrimaryHover);

        /// <summary>
        /// 主色激活态（按钮动画）
        /// </summary>
        public Color PrimaryActive => Style.Get(Colour.PrimaryActive);

        /// <summary>
        /// 主色背景色（按钮底部、下拉激活、文本框激活、菜单激活）
        /// </summary>
        public Color PrimaryBg => Style.Get(Colour.PrimaryBg);

        /// <summary>
        /// 主色背景悬浮态
        /// </summary>
        public Color PrimaryBgHover => Style.Get(Colour.PrimaryBgHover);

        /// <summary>
        /// 主色的描边色
        /// </summary>
        public Color PrimaryBorder => Style.Get(Colour.PrimaryBorder);

        /// <summary>
        /// 主色描边色悬浮态
        /// </summary>
        public Color PrimaryBorderHover => Style.Get(Colour.PrimaryBorderHover);

        /// <summary>
        /// 设置品牌色
        /// </summary>
        /// <param name="primary">品牌色</param>
        [System.Obsolete("use Style.SetPrimary")]
        public void SetPrimary(Color primary) => Style.SetPrimary(primary);

        /// <summary>
        /// 成功色
        /// </summary>
        public Color Success => Style.Get(Colour.Success);

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color SuccessColor => Style.Get(Colour.SuccessColor);

        /// <summary>
        /// 成功色的背景颜色
        /// </summary>
        public Color SuccessBg => Style.Get(Colour.SuccessBg);

        /// <summary>
        /// 成功色的描边色
        /// </summary>
        public Color SuccessBorder => Style.Get(Colour.SuccessBorder);

        /// <summary>
        /// 成功色的悬浮态
        /// </summary>
        public Color SuccessHover => Style.Get(Colour.SuccessHover);

        /// <summary>
        /// 成功色的激活态
        /// </summary>
        public Color SuccessActive => Style.Get(Colour.SuccessActive);

        /// <summary>
        /// 设置成功色
        /// </summary>
        /// <param name="success">成功色</param>
        [System.Obsolete("use Style.SetSuccess")]
        public void SetSuccess(Color success) => Style.SetSuccess(success);

        /// <summary>
        /// 警戒色
        /// </summary>
        public Color Warning => Style.Get(Colour.Warning);

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color WarningColor => Style.Get(Colour.WarningColor);

        /// <summary>
        /// 警戒色的背景颜色
        /// </summary>
        public Color WarningBg => Style.Get(Colour.WarningBg);

        /// <summary>
        /// 警戒色的描边色
        /// </summary>
        public Color WarningBorder => Style.Get(Colour.WarningBorder);

        /// <summary>
        /// 警戒色的悬浮态
        /// </summary>
        public Color WarningHover => Style.Get(Colour.WarningHover);

        /// <summary>
        /// 警戒色的激活态
        /// </summary>
        public Color WarningActive => Style.Get(Colour.WarningActive);

        /// <summary>
        /// 设置警戒色
        /// </summary>
        /// <param name="warning">警戒色</param>
        [System.Obsolete("use Style.SetWarning")]
        public void SetWarning(Color warning) => Style.SetWarning(warning);

        /// <summary>
        /// 错误色
        /// </summary>
        public Color Error => Style.Get(Colour.Error);

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color ErrorColor => Style.Get(Colour.ErrorColor);

        /// <summary>
        /// 错误色的背景颜色（按钮底部）
        /// </summary>
        public Color ErrorBg => Style.Get(Colour.ErrorBg);

        /// <summary>
        /// 错误色的描边色
        /// </summary>
        public Color ErrorBorder => Style.Get(Colour.ErrorBorder);

        /// <summary>
        /// 错误色的悬浮态
        /// </summary>
        public Color ErrorHover => Style.Get(Colour.ErrorHover);

        /// <summary>
        /// 错误色的激活态
        /// </summary>
        public Color ErrorActive => Style.Get(Colour.ErrorActive);

        /// <summary>
        /// 设置错误色
        /// </summary>
        /// <param name="error">错误色</param>
        [System.Obsolete("use Style.SetError")]
        public void SetError(Color error) => Style.SetError(error);

        /// <summary>
        /// 信息色
        /// </summary>
        public Color Info => Style.Get(Colour.Info);

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color InfoColor => Style.Get(Colour.InfoColor);

        /// <summary>
        /// 信息色的背景颜色（按钮底部）
        /// </summary>
        public Color InfoBg => Style.Get(Colour.InfoBg);

        /// <summary>
        /// 信息色的描边色
        /// </summary>
        public Color InfoBorder => Style.Get(Colour.InfoBorder);

        /// <summary>
        /// 信息色的悬浮态
        /// </summary>
        public Color InfoHover => Style.Get(Colour.InfoHover);

        /// <summary>
        /// 信息色的激活态
        /// </summary>
        public Color InfoActive => Style.Get(Colour.InfoActive);

        /// <summary>
        /// 设置信息色
        /// </summary>
        /// <param name="info">信息色</param>
        [System.Obsolete("use Style.SetInfo")]
        public void SetInfo(Color info) => Style.SetInfo(info);

        /// <summary>
        /// 默认背景色
        /// </summary>
        public Color DefaultBg => Style.Get(Colour.DefaultBg);

        /// <summary>
        /// 默认文本色
        /// </summary>
        public Color DefaultColor => Style.Get(Colour.DefaultColor);

        /// <summary>
        /// 默认描边色
        /// </summary>
        public Color DefaultBorder => Style.Get(Colour.DefaultBorder);

        /// <summary>
        /// 标签默认背景色
        /// </summary>
        public Color TagDefaultBg => Style.Get(Colour.TagDefaultBg);

        /// <summary>
        /// 标签默认文本色
        /// </summary>
        public Color TagDefaultColor => Style.Get(Colour.TagDefaultColor);

        /// <summary>
        /// 基础文本色
        /// </summary>
        public Color TextBase => Style.Get(Colour.TextBase);

        /// <summary>
        /// 一级文本色（菜单颜色、非激活下颜色、小清除按钮悬浮态）
        /// </summary>
        public Color Text => Style.Get(Colour.Text);

        /// <summary>
        /// 二级文本色
        /// </summary>
        public Color TextSecondary => Style.Get(Colour.TextSecondary);

        /// <summary>
        /// 三级文本色（小清除按钮）
        /// </summary>
        public Color TextTertiary => Style.Get(Colour.TextTertiary);

        /// <summary>
        /// 四级文本色（禁用色）
        /// </summary>
        public Color TextQuaternary => Style.Get(Colour.TextQuaternary);

        /// <summary>
        /// 基础背景色
        /// </summary>
        public Color BgBase => Style.Get(Colour.BgBase);

        /// <summary>
        /// 组件的容器背景色 例如：默认按钮、输入框等。务必不要将其与 `colorBgElevated` 混淆。
        /// </summary>
        public Color BgContainer => Style.Get(Colour.BgContainer);

        /// <summary>
        /// 浮层容器背景色，在暗色模式下该 token 的色值会比 `colorBgContainer` 要亮一些。例如：模态框、弹出框、菜单等。
        /// </summary>
        public Color BgElevated => Style.Get(Colour.BgElevated);

        /// <summary>
        /// 该色用于页面整体布局的背景色，只有需要在页面中处于 B1 的视觉层级时才会使用该 token，其他用法都是错误的
        /// </summary>
        public Color BgLayout => Style.Get(Colour.BgLayout);

        /// <summary>
        /// 一级填充色
        /// </summary>
        public Color Fill => Style.Get(Colour.Fill);

        /// <summary>
        /// 二级填充色（分页悬浮态、菜单悬浮态）
        /// </summary>
        public Color FillSecondary => Style.Get(Colour.FillSecondary);

        /// <summary>
        /// 三级填充色（下拉悬浮态）
        /// </summary>
        public Color FillTertiary => Style.Get(Colour.FillTertiary);

        /// <summary>
        /// 四级填充色（幽灵按钮底部）
        /// </summary>
        public Color FillQuaternary => Style.Get(Colour.FillQuaternary);

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor => Style.Get(Colour.BorderColor);
        /// <summary>
        /// 二级边框色
        /// </summary>
        public Color BorderSecondary => Style.Get(Colour.BorderSecondary);

        /// <summary>
        /// 禁用边框颜色
        /// </summary>
        public Color BorderColorDisable => Style.Get(Colour.BorderColorDisable);

        /// <summary>
        /// 用于作为分割线的颜色，此颜色和 BorderSecondary 的颜色一致，但是用的是透明色
        /// </summary>
        public Color Split => Style.Get(Colour.Split);

        /// <summary>
        /// 选项悬浮态背景颜色
        /// </summary>
        public Color HoverBg => Style.Get(Colour.HoverBg);

        /// <summary>
        /// 选项悬浮态文本颜色
        /// </summary>
        public Color HoverColor => Style.Get(Colour.HoverColor);

        /// <summary>
        /// 滑块手柄禁用色
        /// </summary>
        public Color SliderHandleColorDisabled => Style.Get(Colour.SliderHandleColorDisabled);

        /// <summary>
        /// Tooltip 的前景色
        /// </summary>
        public Color TextSpotlight => Style.Get(Colour.TextSpotlight);

        /// <summary>
        /// Tooltip 的背景色
        /// </summary>
        public Color BgSpotlight => Style.Get(Colour.BgSpotlight);

        /// <summary>
        /// 开关手柄背景
        /// </summary>
        public Color SwitchHandleBg => Style.Get(Colour.SwitchHandleBg);
    }
}