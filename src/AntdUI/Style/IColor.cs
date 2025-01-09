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

namespace AntdUI.Theme
{
    public class IColor
    {
        public Color Primary => Style.Get(Colour.Primary);
        public Color PrimaryColor => Style.Get(Colour.PrimaryColor);
        public Color PrimaryHover => Style.Get(Colour.PrimaryHover);
        public Color PrimaryActive => Style.Get(Colour.PrimaryActive);
        public Color PrimaryBg => Style.Get(Colour.PrimaryBg);
        public Color PrimaryBgHover => Style.Get(Colour.PrimaryBgHover);
        public Color PrimaryBorder => Style.Get(Colour.PrimaryBorder);
        public Color PrimaryBorderHover => Style.Get(Colour.PrimaryBorderHover);

        [System.Obsolete("use Style.SetPrimary")]
        public void SetPrimary(Color primary) => Style.SetPrimary(primary);

        public Color Success => Style.Get(Colour.Success);
        public Color SuccessColor => Style.Get(Colour.SuccessColor);
        public Color SuccessBg => Style.Get(Colour.SuccessBg);
        public Color SuccessBorder => Style.Get(Colour.SuccessBorder);
        public Color SuccessHover => Style.Get(Colour.SuccessHover);
        public Color SuccessActive => Style.Get(Colour.SuccessActive);

        [System.Obsolete("use Style.SetSuccess")]
        public void SetSuccess(Color success) => Style.SetSuccess(success);

        public Color Warning => Style.Get(Colour.Warning);
        public Color WarningColor => Style.Get(Colour.WarningColor);
        public Color WarningBg => Style.Get(Colour.WarningBg);
        public Color WarningBorder => Style.Get(Colour.WarningBorder);
        public Color WarningHover => Style.Get(Colour.WarningHover);
        public Color WarningActive => Style.Get(Colour.WarningActive);

        [System.Obsolete("use Style.SetWarning")]
        public void SetWarning(Color warning) => Style.SetWarning(warning);

        public Color Error => Style.Get(Colour.Error);
        public Color ErrorColor => Style.Get(Colour.ErrorColor);
        public Color ErrorBg => Style.Get(Colour.ErrorBg);
        public Color ErrorBorder => Style.Get(Colour.ErrorBorder);
        public Color ErrorHover => Style.Get(Colour.ErrorHover);
        public Color ErrorActive => Style.Get(Colour.ErrorActive);

        [System.Obsolete("use Style.SetError")]
        public void SetError(Color error) => Style.SetError(error);

        public Color Info => Style.Get(Colour.Info);
        public Color InfoColor => Style.Get(Colour.InfoColor);
        public Color InfoBg => Style.Get(Colour.InfoBg);
        public Color InfoBorder => Style.Get(Colour.InfoBorder);
        public Color InfoHover => Style.Get(Colour.InfoHover);
        public Color InfoActive => Style.Get(Colour.InfoActive);

        [System.Obsolete("use Style.SetInfo")]
        public void SetInfo(Color info) => Style.SetInfo(info);

        public Color DefaultBg => Style.Get(Colour.DefaultBg);
        public Color DefaultColor => Style.Get(Colour.DefaultColor);
        public Color DefaultBorder => Style.Get(Colour.DefaultBorder);

        public Color TagDefaultBg => Style.Get(Colour.TagDefaultBg);
        public Color TagDefaultColor => Style.Get(Colour.TagDefaultColor);

        public Color TextBase => Style.Get(Colour.TextBase);
        public Color Text => Style.Get(Colour.Text);
        public Color TextSecondary => Style.Get(Colour.TextSecondary);
        public Color TextTertiary => Style.Get(Colour.TextTertiary);
        public Color TextQuaternary => Style.Get(Colour.TextQuaternary);

        public Color BgBase => Style.Get(Colour.BgBase);
        public Color BgContainer => Style.Get(Colour.BgContainer);
        public Color BgElevated => Style.Get(Colour.BgElevated);
        public Color BgLayout => Style.Get(Colour.BgLayout);

        public Color Fill => Style.Get(Colour.Fill);
        public Color FillSecondary => Style.Get(Colour.FillSecondary);
        public Color FillTertiary => Style.Get(Colour.FillTertiary);
        public Color FillQuaternary => Style.Get(Colour.FillQuaternary);

        public Color BorderColor => Style.Get(Colour.BorderColor);
        public Color BorderSecondary => Style.Get(Colour.BorderSecondary);

        public Color BorderColorDisable => Style.Get(Colour.BorderColorDisable);

        public Color Split => Style.Get(Colour.Split);

        public Color HoverBg => Style.Get(Colour.HoverBg);

        public Color HoverColor => Style.Get(Colour.HoverColor);

        public Color SliderHandleColorDisabled => Style.Get(Colour.SliderHandleColorDisabled);
    }
}