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
    public class Light : IColor<Color>
    {
        public Color Primary { get; set; } = "#1677FF".ToColor();
        public Color PrimaryColor { get; set; } = Color.White;
        public Color PrimaryHover { get; set; } = "#4096FF".ToColor();
        public Color PrimaryActive { get; set; } = "#0958D9".ToColor();
        public Color PrimaryBg { get; set; } = "#E6F4FF".ToColor();
        public void SetPrimary(Color primary)
        {
            Primary = primary;
            var colors = primary.GenerateColors();
            PrimaryBg = colors[0];
            PrimaryHover = colors[4];
            PrimaryActive = colors[6];
        }

        public Color Success { get; set; } = "#52C41A".ToColor();
        public Color SuccessColor { get; set; } = Color.White;
        public Color SuccessBg { get; set; } = "#F6FFED".ToColor();
        public Color SuccessBorder { get; set; } = "#B7EB8F".ToColor();
        public Color SuccessHover { get; set; } = "#95DE64".ToColor();
        public Color SuccessActive { get; set; } = "#389E0D".ToColor();
        public void SetSuccess(Color success)
        {
            Success = success;
            var colors = success.GenerateColors();
            SuccessBg = colors[0];
            SuccessBorder = SuccessHover = colors[2];
            SuccessActive = colors[6];
        }
        public Color Warning { get; set; } = "#FAAD14".ToColor();
        public Color WarningColor { get; set; } = Color.White;
        public Color WarningBg { get; set; } = "#FFFBE6".ToColor();
        public Color WarningBorder { get; set; } = "#FFE58F".ToColor();
        public Color WarningHover { get; set; } = "#FFD666".ToColor();
        public Color WarningActive { get; set; } = "#D48806".ToColor();
        public void SetWarning(Color warning)
        {
            Warning = warning;
            var colors = warning.GenerateColors();
            WarningBg = colors[0];
            WarningBorder = WarningHover = colors[2];
            WarningActive = colors[6];
        }
        public Color Error { get; set; } = "#FF4D4F".ToColor();
        public Color ErrorColor { get; set; } = Color.White;
        public Color ErrorBg { get; set; } = "#FFF2F0".ToColor();
        public Color ErrorBorder { get; set; } = "#FFCCC7".ToColor();
        public Color ErrorHover { get; set; } = "#FF7875".ToColor();
        public Color ErrorActive { get; set; } = "#D9363E".ToColor();
        public void SetError(Color error)
        {
            Error = error;
            var colors = error.GenerateColors();
            ErrorBg = colors[0];
            ErrorBorder = ErrorHover = colors[2];
            ErrorActive = colors[6];
        }
        public Color Info { get; set; } = "#1677FF".ToColor();
        public Color InfoColor { get; set; } = Color.White;
        public Color InfoBg { get; set; } = "#E6F4FF".ToColor();
        public Color InfoBorder { get; set; } = "#91CAFF".ToColor();
        public Color InfoHover { get; set; } = "#69B1FF".ToColor();
        public Color InfoActive { get; set; } = "#0958D9".ToColor();
        public void SetInfo(Color info)
        {
            Info = info;
            var colors = info.GenerateColors();
            InfoBg = colors[0];
            InfoBorder = InfoHover = colors[2];
            InfoActive = colors[6];
        }

        public Color DefaultBg { get; set; } = Color.White;
        public Color DefaultColor { get; set; } = Style.rgba(0, 0, 0, 0.88F);
        public Color DefaultBorder { get; set; } = "#D9D9D9".ToColor();

        public Color TagDefaultBg { get; set; } = "#FAFAFA".ToColor();
        public Color TagDefaultColor { get; set; } = Style.rgba(0, 0, 0, 0.88F);

        public Color TextBase { get; set; } = Color.Black;
        public Color Text { get; set; } = Style.rgba(0, 0, 0, 0.88F);//224.4
        public Color TextSecondary { get; set; } = Style.rgba(0, 0, 0, 0.65F);//165.75
        public Color TextTertiary { get; set; } = Style.rgba(0, 0, 0, 0.45F);//114.75
        public Color TextQuaternary { get; set; } = Style.rgba(0, 0, 0, 0.25F);//63.75

        public Color BgBase { get; set; } = Color.White;
        public Color BgContainer { get; set; } = Color.White;
        public Color BgElevated { get; set; } = Color.White;
        public Color BgLayout { get; set; } = "#F5F5F5".ToColor();

        public Color Fill { get; set; } = Style.rgba(0, 0, 0, 0.18F);//45.9
        public Color FillSecondary { get; set; } = Style.rgba(0, 0, 0, 0.06F);//15.3
        public Color FillTertiary { get; set; } = Style.rgba(0, 0, 0, 0.04F);//10.2
        public Color FillQuaternary { get; set; } = Style.rgba(0, 0, 0, 0.02F);//5.1

        public Color BorderColor { get; set; } = "#D9D9D9".ToColor();
        public Color BorderSecondary { get; set; } = "#F0F0F0".ToColor();

        public Color BorderColorDisable { get; set; } = Color.FromArgb(217, 217, 217);

        public Color Split { get; set; } = Style.rgba(5, 5, 5, 0.06F);//15.3

        public Color HoverBg { get; set; } = Style.rgba(0, 0, 0, 0.06F);

        public Color HoverColor { get; set; } = Style.rgba(0, 0, 0, 0.88F);
    }
}