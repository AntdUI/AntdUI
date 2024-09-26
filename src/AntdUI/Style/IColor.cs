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

namespace AntdUI.Theme
{
    public interface IColor<T>
    {
        #region 品牌色

        /// <summary>
        /// 品牌色
        /// </summary>
        T Primary { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        T PrimaryColor { get; set; }

        /// <summary>
        /// 主色悬浮态（按钮、开关、复选框）
        /// </summary>
        T PrimaryHover { get; set; }

        /// <summary>
        /// 主色激活态（按钮动画）
        /// </summary>
        T PrimaryActive { get; set; }

        /// <summary>
        /// 主色背景色（按钮底部、下拉激活、文本框激活、菜单激活）
        /// </summary>
        T PrimaryBg { get; set; }

        void SetPrimary(T primary);

        #endregion

        #region 成功色

        /// <summary>
        /// 成功色
        /// </summary>
        T Success { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        T SuccessColor { get; set; }

        /// <summary>
        /// 成功色的背景颜色
        /// </summary>
        T SuccessBg { get; set; }

        /// <summary>
        /// 成功色的描边色
        /// </summary>
        T SuccessBorder { get; set; }

        /// <summary>
        /// 成功色的悬浮态
        /// </summary>
        T SuccessHover { get; set; }

        /// <summary>
        /// 成功色的激活态
        /// </summary>
        T SuccessActive { get; set; }

        void SetSuccess(T success);

        #endregion

        #region 警戒色

        /// <summary>
        /// 警戒色
        /// </summary>
        T Warning { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        T WarningColor { get; set; }

        /// <summary>
        /// 警戒色的背景颜色
        /// </summary>
        T WarningBg { get; set; }

        /// <summary>
        /// 警戒色的描边色
        /// </summary>
        T WarningBorder { get; set; }

        /// <summary>
        /// 警戒色的悬浮态
        /// </summary>
        T WarningHover { get; set; }

        /// <summary>
        /// 警戒色的激活态
        /// </summary>
        T WarningActive { get; set; }

        void SetWarning(T warning);

        #endregion

        #region 错误色

        /// <summary>
        /// 错误色
        /// </summary>
        T Error { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        T ErrorColor { get; set; }

        /// <summary>
        /// 警戒色的背景颜色（按钮底部）
        /// </summary>
        T ErrorBg { get; set; }

        /// <summary>
        /// 警戒色的描边色
        /// </summary>
        T ErrorBorder { get; set; }

        /// <summary>
        /// 错误色的悬浮态
        /// </summary>
        T ErrorHover { get; set; }

        /// <summary>
        /// 错误色的激活态
        /// </summary>
        T ErrorActive { get; set; }

        void SetError(T error);

        #endregion

        #region 信息色

        /// <summary>
        /// 信息色
        /// </summary>
        T Info { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        T InfoColor { get; set; }

        /// <summary>
        /// 信息色的背景颜色（按钮底部）
        /// </summary>
        T InfoBg { get; set; }

        /// <summary>
        /// 信息色的描边色
        /// </summary>
        T InfoBorder { get; set; }

        /// <summary>
        /// 信息色的悬浮态
        /// </summary>
        T InfoHover { get; set; }

        /// <summary>
        /// 信息色的激活态
        /// </summary>
        T InfoActive { get; set; }

        void SetInfo(T info);

        #endregion

        T DefaultBg { get; set; }
        T DefaultColor { get; set; }
        T DefaultBorder { get; set; }

        T TagDefaultBg { get; set; }
        T TagDefaultColor { get; set; }

        #region 中性色

        /// <summary>
        /// 基础文本色
        /// </summary>
        T TextBase { get; set; }

        /// <summary>
        /// 一级文本色（菜单颜色、非激活下颜色、小清除按钮悬浮态）
        /// </summary>
        T Text { get; set; }

        /// <summary>
        /// 二级文本色
        /// </summary>
        T TextSecondary { get; set; }

        /// <summary>
        /// 三级文本色（小清除按钮）
        /// </summary>
        T TextTertiary { get; set; }

        /// <summary>
        /// 四级文本色（禁用色）
        /// </summary>
        T TextQuaternary { get; set; }

        /// <summary>
        /// 基础背景色
        /// </summary>
        T BgBase { get; set; }

        /// <summary>
        /// 组件的容器背景色 例如：默认按钮、输入框等。务必不要将其与 `colorBgElevated` 混淆。
        /// </summary>
        T BgContainer { get; set; }

        /// <summary>
        /// 浮层容器背景色，在暗色模式下该 token 的色值会比 `colorBgContainer` 要亮一些。例如：模态框、弹出框、菜单等。
        /// </summary>
        T BgElevated { get; set; }

        /// <summary>
        /// 该色用于页面整体布局的背景色，只有需要在页面中处于 B1 的视觉层级时才会使用该 token，其他用法都是错误的
        /// </summary>
        T BgLayout { get; set; }

        /// <summary>
        /// 一级填充色
        /// </summary>
        T Fill { get; set; }

        /// <summary>
        /// 二级填充色（分页悬浮态、菜单悬浮态）
        /// </summary>
        T FillSecondary { get; set; }

        /// <summary>
        /// 三级填充色（下拉悬浮态）
        /// </summary>
        T FillTertiary { get; set; }

        /// <summary>
        /// 四级填充色（幽灵按钮底部）
        /// </summary>
        T FillQuaternary { get; set; }

        /// <summary>
        /// 边框颜色
        /// </summary>
        T BorderColor { get; set; }
        T BorderSecondary { get; set; }

        /// <summary>
        /// 禁用边框颜色
        /// </summary>
        T BorderColorDisable { get; set; }

        #endregion

        /// <summary>
        /// 用于作为分割线的颜色，此颜色和 BorderSecondary 的颜色一致，但是用的是透明色
        /// </summary>
        T Split { get; set; }

        /// <summary>
        /// 选项悬浮态背景颜色
        /// </summary>
        T HoverBg { get; set; }

        /// <summary>
        /// 选项悬浮态文本颜色
        /// </summary>
        T HoverColor { get; set; }

        T SliderHandleColorDisabled { get; set; }
    }
}