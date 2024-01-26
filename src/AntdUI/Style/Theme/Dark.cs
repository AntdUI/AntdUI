// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
    public class Dark : IColor<Color>
    {
        public Color Primary { get; set; } = "#1668DC".ToColor();
        public Color PrimaryColor { get; set; } = Color.White;
        public Color PrimaryHover { get; set; } = "#3C89E8".ToColor();
        public Color PrimaryActive { get; set; } = "#1554AD".ToColor();
        public Color PrimaryBg { get; set; } = "#111A2C".ToColor();
        public void SetPrimary(Color primary)
        {
            Primary = primary;
            var colors = primary.GenerateColors();
            PrimaryBg = colors[9];
            PrimaryHover = colors[4];
            PrimaryActive = colors[6];
        }

        public Color Success { get; set; } = "#49AA19".ToColor();
        public Color SuccessColor { get; set; } = Color.White;
        public Color SuccessBg { get; set; } = "#162312".ToColor();
        public Color SuccessBorder { get; set; } = "#274916".ToColor();
        public Color SuccessHover { get; set; } = "#306317".ToColor();
        public Color SuccessActive { get; set; } = "#3C8618".ToColor();
        public void SetSuccess(Color success)
        {
            Success = success;
            var colors = success.GenerateColors();
            SuccessBg = colors[9];
            SuccessBorder = SuccessHover = colors[5];
            SuccessActive = colors[6];
        }
        public Color Warning { get; set; } = "#D89614".ToColor();
        public Color WarningColor { get; set; } = Color.White;
        public Color WarningBg { get; set; } = "#2B2111".ToColor();
        public Color WarningBorder { get; set; } = "#594214".ToColor();
        public Color WarningHover { get; set; } = "#7C5914".ToColor();
        public Color WarningActive { get; set; } = "#AA7714".ToColor();
        public void SetWarning(Color warning)
        {
            Warning = warning;
            var colors = warning.GenerateColors();
            WarningBg = colors[9];
            WarningBorder = WarningHover = colors[5];
            WarningActive = colors[6];
        }
        public Color Error { get; set; } = "#DC4446".ToColor();
        public Color ErrorColor { get; set; } = Color.White;
        public Color ErrorBg { get; set; } = "#2C1618".ToColor();
        public Color ErrorBorder { get; set; } = "#5B2526".ToColor();
        public Color ErrorHover { get; set; } = "#E86E6B".ToColor();
        public Color ErrorActive { get; set; } = "#AD393A".ToColor();
        public void SetError(Color error)
        {
            Error = error;
            var colors = error.GenerateColors();
            ErrorBg = colors[9];
            ErrorBorder = ErrorHover = colors[5];
            ErrorActive = colors[6];
        }
        public Color Info { get; set; } = "#1668DC".ToColor();
        public Color InfoColor { get; set; } = Color.White;
        public Color InfoBg { get; set; } = "#111A2C".ToColor();
        public Color InfoBorder { get; set; } = "#15325B".ToColor();
        public Color InfoHover { get; set; } = "#15417E".ToColor();
        public Color InfoActive { get; set; } = "#1554AD".ToColor();
        public void SetInfo(Color info)
        {
            Info = info;
            var colors = info.GenerateColors();
            InfoBg = colors[9];
            InfoBorder = InfoHover = colors[5];
            InfoActive = colors[6];
        }

        public Color DefaultBg { get; set; } = "#141414".ToColor();
        public Color DefaultColor { get; set; } = Style.rgba(255, 255, 255, 0.85F);
        public Color DefaultBorder { get; set; } = "#424242".ToColor();

        public Color TagDefaultBg { get; set; } = "#1D1D1D".ToColor();
        public Color TagDefaultColor { get; set; } = Style.rgba(255, 255, 255, 0.85F);

        public Color TextBase { get; set; } = Color.White;
        public Color Text { get; set; } = Style.rgba(255, 255, 255, 0.85F);//216.75
        public Color TextSecondary { get; set; } = Style.rgba(255, 255, 255, 0.65F);//165.75
        public Color TextTertiary { get; set; } = Style.rgba(255, 255, 255, 0.45F);//114.75
        public Color TextQuaternary { get; set; } = Style.rgba(255, 255, 255, 0.25F);//63.75
        public Color BgBase { get; set; } = Color.Black;
        public Color BgContainer { get; set; } = "#141414".ToColor();
        public Color BgElevated { get; set; } = "#1F1F1F".ToColor();
        public Color BgLayout { get; set; } = Color.Black;
        public Color Fill { get; set; } = Style.rgba(255, 255, 255, 0.15F);//38.25
        public Color FillSecondary { get; set; } = Style.rgba(255, 255, 255, 0.12F);//30.6
        public Color FillTertiary { get; set; } = Style.rgba(255, 255, 255, 0.08F);//20.4
        public Color FillQuaternary { get; set; } = Style.rgba(255, 255, 255, 0.04F);//10.2

        public Color BorderColor { get; set; } = "#424242".ToColor();
        public Color BorderSecondary { get; set; } = "#303030".ToColor();

        public Color BorderColorDisable { get; set; } = Color.FromArgb(66, 66, 66);

        public Color Split { get; set; } = Style.rgba(253, 253, 253, 0.12F);//30.6

        public Color HoverBg { get; set; } = Style.rgba(255, 255, 255, 0.06F);

        public Color HoverColor { get; set; } = Style.rgba(255, 255, 255, 0.88F);
    }
}