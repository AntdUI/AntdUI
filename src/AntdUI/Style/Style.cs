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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    public static class Style
    {
        #region 色卡

        public static Theme.IColor Db;
        static Style() { Db = new Theme.IColor(); }
        public static Color Get(this Colour id, string control)
        {
            string key = id.ToString() + control;
            if (colors.TryGetValue(key, out var color)) return color;
            return Get(id);
        }
        public static Color Get(this Colour id, string control, TAMode mode)
        {
            string key = id.ToString() + control;
            if (colors.TryGetValue(key, out var color)) return color;
            return Get(id, mode);
        }

        public static Color Get(this Colour id)
        {
            string key = id.ToString();
            if (colors.TryGetValue(key, out var color)) return color;
            return GetSystem(id, Config.Mode);
        }
        public static Color Get(this Colour id, TAMode mode)
        {
            string key = id.ToString();
            if (colors.TryGetValue(key, out var color)) return color;
            switch (mode)
            {
                case TAMode.Light: return GetSystem(id, TMode.Light);
                case TAMode.Dark: return GetSystem(id, TMode.Dark);
                case TAMode.Auto:
                default:
                    return GetSystem(id, Config.Mode);
            }
        }
        public static Color GetSystem(this Colour id, TMode mode)
        {
            switch (mode)
            {
                case TMode.Light:
                    switch (id)
                    {
                        case Colour.Primary: return "#1677FF".ToColor();
                        case Colour.PrimaryHover: return "#4096FF".ToColor();
                        case Colour.PrimaryColor: return Color.White;
                        case Colour.PrimaryActive: return "#0958D9".ToColor();
                        case Colour.PrimaryBg: return "#E6F4FF".ToColor();
                        case Colour.PrimaryBgHover: return "#BAE0FF".ToColor();
                        case Colour.PrimaryBorder: return "#91CAFF".ToColor();
                        case Colour.PrimaryBorderHover: return "#69B1FF".ToColor();

                        case Colour.Success: return "#52C41A".ToColor();
                        case Colour.SuccessColor: return Color.White;
                        case Colour.SuccessBg: return "#F6FFED".ToColor();
                        case Colour.SuccessBorder: return "#B7EB8F".ToColor();
                        case Colour.SuccessHover: return "#95DE64".ToColor();
                        case Colour.SuccessActive: return "#389E0D".ToColor();

                        case Colour.Warning: return "#FAAD14".ToColor();
                        case Colour.WarningColor: return Color.White;
                        case Colour.WarningBg: return "#FFFBE6".ToColor();
                        case Colour.WarningBorder: return "#FFE58F".ToColor();
                        case Colour.WarningHover: return "#FFD666".ToColor();
                        case Colour.WarningActive: return "#D48806".ToColor();

                        case Colour.Error: return "#FF4D4F".ToColor();
                        case Colour.ErrorColor: return Color.White;
                        case Colour.ErrorBg: return "#FFF2F0".ToColor();
                        case Colour.ErrorBorder: return "#FFCCC7".ToColor();
                        case Colour.ErrorHover: return "#FF7875".ToColor();
                        case Colour.ErrorActive: return "#D9363E".ToColor();

                        case Colour.Info: return "#1677FF".ToColor();
                        case Colour.InfoColor: return Color.White;
                        case Colour.InfoBg: return "#E6F4FF".ToColor();
                        case Colour.InfoBorder: return "#91CAFF".ToColor();
                        case Colour.InfoHover: return "#69B1FF".ToColor();
                        case Colour.InfoActive: return "#0958D9".ToColor();

                        case Colour.DefaultBg: return Color.White;
                        case Colour.DefaultColor: return rgba(0, 0, 0, 0.88F);
                        case Colour.DefaultBorder: return "#D9D9D9".ToColor();

                        case Colour.TagDefaultBg: return "#FAFAFA".ToColor();
                        case Colour.TagDefaultColor: return rgba(0, 0, 0, 0.88F);

                        case Colour.TextBase: return Color.Black;
                        case Colour.Text: return rgba(0, 0, 0, 0.88F);//224.4
                        case Colour.TextSecondary: return rgba(0, 0, 0, 0.65F);//165.75
                        case Colour.TextTertiary: return rgba(0, 0, 0, 0.45F);//114.75
                        case Colour.TextQuaternary: return rgba(0, 0, 0, 0.25F);//63.75

                        case Colour.BgBase: return Color.White;
                        case Colour.BgContainer: return Color.White;
                        case Colour.BgElevated: return Color.White;
                        case Colour.BgLayout: return "#F5F5F5".ToColor();

                        case Colour.Fill: return rgba(0, 0, 0, 0.18F);//45.9
                        case Colour.FillSecondary: return rgba(0, 0, 0, 0.06F);//15.3
                        case Colour.FillTertiary: return rgba(0, 0, 0, 0.04F);//10.2
                        case Colour.FillQuaternary: return rgba(0, 0, 0, 0.02F);//5.1

                        case Colour.BorderColor: return "#D9D9D9".ToColor();
                        case Colour.BorderSecondary: return "#F0F0F0".ToColor();

                        case Colour.BorderColorDisable: return Color.FromArgb(217, 217, 217);

                        case Colour.Split: return rgba(5, 5, 5, 0.06F);//15.3

                        case Colour.HoverBg: return rgba(0, 0, 0, 0.06F);
                        case Colour.HoverColor: return rgba(0, 0, 0, 0.88F);

                        case Colour.SliderHandleColorDisabled: return "#BFBFBF".ToColor();
                    }
                    break;
                case TMode.Dark:
                default:
                    switch (id)
                    {
                        case Colour.Primary: return "#1668DC".ToColor();
                        case Colour.PrimaryHover: return "#3C89E8".ToColor();
                        case Colour.PrimaryColor: return Color.White;
                        case Colour.PrimaryActive: return "#1554AD".ToColor();
                        case Colour.PrimaryBg: return "#111A2C".ToColor();
                        case Colour.PrimaryBgHover: return "#112545".ToColor();
                        case Colour.PrimaryBorder: return "#15325B".ToColor();
                        case Colour.PrimaryBorderHover: return "#15417E".ToColor();

                        case Colour.Success: return "#49AA19".ToColor();
                        case Colour.SuccessColor: return Color.White;
                        case Colour.SuccessBg: return "#162312".ToColor();
                        case Colour.SuccessBorder: return "#274916".ToColor();
                        case Colour.SuccessHover: return "#306317".ToColor();
                        case Colour.SuccessActive: return "#3C8618".ToColor();

                        case Colour.Warning: return "#D89614".ToColor();
                        case Colour.WarningColor: return Color.White;
                        case Colour.WarningBg: return "#2B2111".ToColor();
                        case Colour.WarningBorder: return "#594214".ToColor();
                        case Colour.WarningHover: return "#7C5914".ToColor();
                        case Colour.WarningActive: return "#AA7714".ToColor();

                        case Colour.Error: return "#DC4446".ToColor();
                        case Colour.ErrorColor: return Color.White;
                        case Colour.ErrorBg: return "#2C1618".ToColor();
                        case Colour.ErrorBorder: return "#5B2526".ToColor();
                        case Colour.ErrorHover: return "#E86E6B".ToColor();
                        case Colour.ErrorActive: return "#AD393A".ToColor();

                        case Colour.Info: return "#1668DC".ToColor();
                        case Colour.InfoColor: return Color.White;
                        case Colour.InfoBg: return "#111A2C".ToColor();
                        case Colour.InfoBorder: return "#15325B".ToColor();
                        case Colour.InfoHover: return "#15417E".ToColor();
                        case Colour.InfoActive: return "#1554AD".ToColor();

                        case Colour.DefaultBg: return "#141414".ToColor();
                        case Colour.DefaultColor: return rgba(255, 255, 255, 0.85F);
                        case Colour.DefaultBorder: return "#424242".ToColor();

                        case Colour.TagDefaultBg: return "#1D1D1D".ToColor();
                        case Colour.TagDefaultColor: return rgba(255, 255, 255, 0.85F);

                        case Colour.TextBase: return Color.White;
                        case Colour.Text: return rgba(255, 255, 255, 0.85F);//216.75
                        case Colour.TextSecondary: return rgba(255, 255, 255, 0.65F);//165.75
                        case Colour.TextTertiary: return rgba(255, 255, 255, 0.45F);//114.75
                        case Colour.TextQuaternary: return rgba(255, 255, 255, 0.25F);//63.75

                        case Colour.BgBase: return Color.Black;
                        case Colour.BgContainer: return "#141414".ToColor();
                        case Colour.BgElevated: return "#1F1F1F".ToColor();
                        case Colour.BgLayout: return Color.Black;

                        case Colour.Fill: return rgba(255, 255, 255, 0.15F);//38.25
                        case Colour.FillSecondary: return rgba(255, 255, 255, 0.12F);//30.6
                        case Colour.FillTertiary: return rgba(255, 255, 255, 0.08F);//20.4
                        case Colour.FillQuaternary: return rgba(255, 255, 255, 0.04F);//10.2

                        case Colour.BorderColor: return "#424242".ToColor();
                        case Colour.BorderSecondary: return "#303030".ToColor();

                        case Colour.BorderColorDisable: return Color.FromArgb(66, 66, 66);

                        case Colour.Split: return rgba(253, 253, 253, 0.12F);//30.6

                        case Colour.HoverBg: return rgba(255, 255, 255, 0.06F);
                        case Colour.HoverColor: return rgba(255, 255, 255, 0.88F);

                        case Colour.SliderHandleColorDisabled: return "#4F4F4F".ToColor();
                    }
                    break;
            }
            return Color.Transparent;
        }

        static Dictionary<string, Color> colors = new Dictionary<string, Color>();
        public static void Set(this Colour id, Color value)
        {
            string key = id.ToString();
            if (colors.ContainsKey(key)) colors[key] = value;
            else colors.Add(key, value);
        }

        public static void Set(this Colour id, Color value, string control)
        {
            string key = id.ToString() + control;
            if (colors.ContainsKey(key)) colors[key] = value;
            else colors.Add(key, value);
        }

        public static void SetPrimary(Color primary)
        {
            Colour.Primary.Set(primary);
            var colors = primary.GenerateColors();
            if (Config.Mode == TMode.Light)
            {
                Colour.PrimaryBg.Set(colors[0]);
                Colour.PrimaryBgHover.Set(colors[1]);
                Colour.PrimaryBorder.Set(colors[2]);
                Colour.PrimaryBorderHover.Set(colors[3]);
            }
            else
            {
                Colour.PrimaryBg.Set(colors[9]);
                Colour.PrimaryBgHover.Set(colors[8]);
                Colour.PrimaryBorder.Set(colors[5]);
                Colour.PrimaryBorderHover.Set(colors[6]);
            }

            Colour.PrimaryHover.Set(colors[4]);
            Colour.PrimaryActive.Set(colors[6]);
        }
        public static void SetSuccess(Color success)
        {
            Colour.Success.Set(success);
            var colors = success.GenerateColors();
            if (Config.Mode == TMode.Light)
            {
                Colour.SuccessBg.Set(colors[0]);
                Colour.SuccessHover.Set(colors[2]);
                Colour.SuccessBorder.Set(colors[2]);
            }
            else
            {
                Colour.SuccessBg.Set(colors[9]);
                Colour.SuccessHover.Set(colors[5]);
                Colour.SuccessBorder.Set(colors[5]);
            }
            Colour.SuccessActive.Set(colors[6]);
        }
        public static void SetWarning(Color warning)
        {
            Colour.Warning.Set(warning);
            var colors = warning.GenerateColors();
            if (Config.Mode == TMode.Light)
            {
                Colour.WarningBg.Set(colors[0]);
                Colour.WarningHover.Set(colors[2]);
                Colour.WarningBorder.Set(colors[2]);
            }
            else
            {
                Colour.WarningBg.Set(colors[9]);
                Colour.WarningHover.Set(colors[5]);
                Colour.WarningBorder.Set(colors[5]);
            }
            Colour.WarningActive.Set(colors[6]);
        }
        public static void SetError(Color error)
        {
            Colour.Error.Set(error);
            var colors = error.GenerateColors();
            if (Config.Mode == TMode.Light)
            {
                Colour.ErrorBg.Set(colors[0]);
                Colour.ErrorHover.Set(colors[2]);
                Colour.ErrorBorder.Set(colors[2]);
            }
            else
            {
                Colour.ErrorBg.Set(colors[9]);
                Colour.ErrorHover.Set(colors[5]);
                Colour.ErrorBorder.Set(colors[5]);
            }
            Colour.ErrorActive.Set(colors[6]);
        }
        public static void SetInfo(Color info)
        {
            Colour.Info.Set(info);
            var colors = info.GenerateColors();
            if (Config.Mode == TMode.Light)
            {
                Colour.InfoBg.Set(colors[0]);
                Colour.InfoHover.Set(colors[2]);
                Colour.InfoBorder.Set(colors[2]);
            }
            else
            {
                Colour.InfoBg.Set(colors[9]);
                Colour.InfoHover.Set(colors[5]);
                Colour.InfoBorder.Set(colors[5]);
            }
            Colour.InfoActive.Set(colors[6]);
        }

        /// <summary>
        /// 加载自定义主题
        /// </summary>
        /// <param name="color">色卡</param>
        public static void LoadCustom(this Dictionary<string, Color> color)
        {
            colors = color;
            EventHub.Dispatch(EventType.THEME);
        }

        /// <summary>
        /// 加载自定义主题
        /// </summary>
        /// <param name="color">色卡</param>
        public static void LoadCustom(this Dictionary<string, string> color)
        {
            var tmp = new Dictionary<string, Color>(color.Count);
            foreach (var it in color) tmp.Add(it.Key, it.Value.ToColor());
            colors = tmp;
            EventHub.Dispatch(EventType.THEME);
        }

        /// <summary>
        /// 清空自定义
        /// </summary>
        public static void Clear()
        {
            colors.Clear();
            EventHub.Dispatch(EventType.THEME);
        }

        #endregion

        /// <summary>
        /// 色彩模式（浅色、暗色）
        /// </summary>
        /// <returns>true Light;false Dark</returns>
        public static bool ColorMode(this Color color) => ((color.R * 299 + color.G * 587 + color.B * 114) / 1000) > 128;

        #region 生成色卡

        static float warmDark = 0.5F;     // 暖色调暗收音机
        static float warmRotate = -26;  // 暖色旋转度
        static float coldDark = 0.55F;     // 冷色调暗收音机
        static float coldRotate = 10;   // 冷色旋转度
        public static Color shade(this Color shadeColor)
        {
            // 暖色和冷色会在不同的收音机中变暗，并以不同的程度旋转
            // 暖色
            if (shadeColor.R > shadeColor.B)
            {
                return shadeColor.darken(shadeColor.ToHSL().l * warmDark).spin(warmRotate).HSLToColor();
            }
            // 冷色
            return shadeColor.darken(shadeColor.ToHSL().l * coldDark).spin(coldRotate).HSLToColor();
        }
        public static HSL darken(this Color color, float amount)
        {
            var hsl = color.ToHSL();
            hsl.l -= amount / 100F;
            hsl.l = clamp01(hsl.l);
            return hsl;
        }
        static HSL spin(this HSL hsl, float amount)
        {
            var hue = (hsl.h + amount) % 360F;
            hsl.h = hue < 0F ? 360F + hue : hue;
            return hsl;
        }
        static float clamp01(float val)
        {
            return Math.Min(1F, Math.Max(0F, val));
        }

        public static List<Color> GenerateColors(this Color primaryColor)
        {
            var hsv = primaryColor.ToHSV();
            var colors = new List<Color>(lightColorCount + darkColorCount);
            // 主色前
            for (var i = lightColorCount; i > 0; i--) colors.Add(GenerateColor(hsv, i, true));
            // 主色
            colors.Add(primaryColor);
            // 主色后
            for (var i = 1; i <= darkColorCount; i++) colors.Add(GenerateColor(hsv, i, false));
            return colors;
        }

        /// <summary>
        /// 生成色卡
        /// </summary>
        /// <param name="hsv">色调/饱和度/亮度</param>
        /// <param name="i">序号</param>
        /// <param name="isLight">是否浅色</param>
        public static Color GenerateColor(HSV hsv, int i, bool isLight)
        {
            // i 为index与6的相对距离
            return HSVToColor(getHue(hsv, i, isLight), getSaturation(hsv, i, isLight), getValue(hsv, i, isLight));
        }

        static int hueStep = 2;
        static int darkColorCount = 4, lightColorCount = 5;
        static float saturationStep = 0.16F, saturationStep2 = 0.05F;
        static float brightnessStep1 = 0.05F, brightnessStep2 = 0.15F;
        public static float getHue(HSV hsv, int i, bool isLight)
        {
            float hue;
            if (hsv.h >= 60 && hsv.h <= 240) hue = isLight ? hsv.h - hueStep * i : hsv.h + hueStep * i;
            else hue = isLight ? hsv.h + hueStep * i : hsv.h - hueStep * i;
            if (hue < 0) hue += 360F;
            else if (hue >= 360) hue -= 360F;
            return hue;
        }
        public static float getSaturation(HSV hsv, int i, bool isLight)
        {
            // grey color don't change saturation
            if (hsv.h == 0 && hsv.s == 0) return hsv.s;
            float saturation;
            if (isLight) saturation = hsv.s - saturationStep * i;
            else if (i == darkColorCount) saturation = hsv.s + saturationStep;
            else saturation = hsv.s + saturationStep2 * i;
            if (saturation > 1) saturation = 1;
            if (isLight && i == lightColorCount && saturation > 0.1) saturation = 0.1F;
            if (saturation < 0.06) saturation = 0.06F;
            return saturation;//保留两位小数
        }
        public static float getValue(HSV hsv, int i, bool isLight)
        {
            float value;
            if (isLight) value = hsv.v + brightnessStep1 * i;
            else value = hsv.v - brightnessStep2 * i;
            if (value > 1) value = 1;
            return value;
        }

        #endregion

        #region 颜色转换

        #region HSV

        /// <summary>
        /// 颜色转HSV
        /// </summary>
        public static HSV ToHSV(this Color color)
        {
            float min = Math.Min(Math.Min(color.R, color.G), color.B) / 255F, max = Math.Max(Math.Max(color.R, color.G), color.B) / 255F;
            return new HSV(color.GetHue(), max == 0F ? 0F : (max - min) / max, max);
        }

        /// <summary>
        /// HSV转颜色
        /// </summary>
        public static Color HSVToColor(this HSV hsv, float alpha = 1)
        {
            return HSVToColor(hsv.h, hsv.s, hsv.v, alpha);
        }

        /// <summary>
        /// HSV转颜色
        /// </summary>
        /// <param name="hue">色相</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="value">明度</param>
        /// <param name="alpha">透明度</param>
        public static Color HSVToColor(float hue, float saturation, float value, float alpha = 1)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            float f = hue / 60F - (float)Math.Floor(hue / 60D);

            float v = value;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);

            if (hi == 0)
                return rgba(v, t, p, alpha);
            if (hi == 1)
                return rgba(q, v, p, alpha);
            if (hi == 2)
                return rgba(p, v, t, alpha);
            if (hi == 3)
                return rgba(p, q, v, alpha);
            if (hi == 4)
                return rgba(t, p, v, alpha);

            return rgba(v, p, q, alpha);
        }

        #endregion

        #region HSL

        /// <summary>
        /// 颜色转HSL
        /// </summary>
        public static HSL ToHSL(this Color color)
        {
            float min = Math.Min(Math.Min(color.R, color.G), color.B) / 255F, max = Math.Max(Math.Max(color.R, color.G), color.B) / 255F;

            float lightness = (max + min) / 2F;

            if (lightness == 0F || min == max)
            {
                return new HSL(color.GetHue(), 0F, lightness);
            }
            else if (lightness > 0F && lightness <= 0.5F)
            {
                return new HSL(color.GetHue(), (max - min) / (max + min), lightness);
            }

            return new HSL(color.GetHue(), (max - min) / (2F - (max + min)), lightness);
        }

        /// <summary>
        /// HSL转颜色
        /// </summary>
        public static Color HSLToColor(this HSL hsl, float alpha = 1)
        {
            return HSLToColor(hsl.h, hsl.s, hsl.l, alpha);
        }

        /// <summary>
        /// HSL转颜色
        /// </summary>
        /// <param name="hue">色相</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="lightness">亮度</param>
        /// <param name="alpha">透明度</param>
        public static Color HSLToColor(float hue, float saturation, float lightness, float alpha = 1)
        {
            float _saturation = saturation;
            float _lightness = lightness;
            float c = (1 - Math.Abs(2 * _lightness - 1)) * _saturation;
            float _hue = (hue % 360) / 60;
            float x = c * (1 - Math.Abs(_hue % 2 - 1));
            float r = 0, g = 0, b = 0;
            if (_hue >= 0 && _hue < 1)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (_hue >= 1 && _hue <= 2)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (_hue >= 2 && _hue <= 3)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (_hue > 3 && _hue <= 4)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (_hue > 4 && _hue <= 5)
            {
                r = x;
                g = 9;
                b = c;
            }
            else if (_hue > 5 && _hue <= 6)
            {
                r = c;
                g = 0;
                b = x;
            }
            var m = _lightness - c / 2;
            return rgba(Math.Abs(r + m), Math.Abs(g + m), Math.Abs(b + m), alpha);
        }

        #endregion

        public static Color rgba(int r, int g, int b, float a = 1)
        {
            return Color.FromArgb((int)Math.Round(255F * a), r, g, b);
        }
        public static Color rgba(this Color color, float a = 1)
        {
            return rgba(color.R, color.G, color.B, a);
        }
        public static Color rgba(float r, float g, float b, float a = 1)
        {
            if (r < 0) r = 0F;
            else if (r > 1) r = 1F;
            if (g < 0) g = 0F;
            else if (g > 1) g = 1F;
            if (b < 0) b = 0F;
            else if (b > 1) b = 1F;
            return Color.FromArgb((int)Math.Round(255F * a), (int)Math.Round(255F * r), (int)Math.Round(255F * g), (int)Math.Round(255F * b));
        }

        public static Color ToColor(this string? str)
        {
            if (str == null) return Color.Black;
            try
            {
                if (str.StartsWith("rgba"))
                {
                    str = str.Substring(4).Trim().TrimStart('(').TrimEnd(')');
                    var arr = str.Split(new string[] { " , ", ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 4 && int.TryParse(arr[0], out int r) && int.TryParse(arr[1], out int g) && int.TryParse(arr[2], out int b) && float.TryParse(arr[3], out float a)) return rgba(r, g, b, a);
                }
                else if (str.StartsWith("rgb"))
                {
                    str = str.Substring(3).Trim().TrimStart('(').TrimEnd(')');
                    var arr = str.Split(new string[] { " , ", ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 3 && int.TryParse(arr[0], out int r) && int.TryParse(arr[1], out int g) && int.TryParse(arr[2], out int b)) return Color.FromArgb(r, g, b);
                }
                else if (str.Length > 5)
                {
                    if (str.StartsWith("#")) str = str.Substring(1);
                    if (str.Length == 6) return Color.FromArgb(str.Substring(0, 2).HexToInt(), str.Substring(2, 2).HexToInt(), str.Substring(4, 2).HexToInt());
                    else if (str.Length == 8) return Color.FromArgb(str.Substring(6, 2).HexToInt(), str.Substring(0, 2).HexToInt(), str.Substring(2, 2).HexToInt(), str.Substring(4, 2).HexToInt());
                }
            }
            catch
            {
            }
            return Color.Black;
        }

        /// <summary>
        /// 颜色：RGB转成16进制
        /// </summary>
        /// <returns></returns>
        public static string ToHex(this Color color)
        {
            if (color.A == 255) return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", color.R, color.G, color.B, color.A);
        }

        static int HexToInt(this string str) => int.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier);

        #endregion

        public static Color BlendColors(this Color baseColor, int alpha, Color overlay) => BlendColors(baseColor, Helper.ToColor(alpha, overlay));

        /// <summary>
        /// 颜色混合
        /// </summary>
        /// <param name="baseColor">基础色</param>
        /// <param name="overlay">叠加色</param>
        /// <returns>混合后颜色</returns>
        public static Color BlendColors(this Color baseColor, Color overlay)
        {
            byte baseAlpha = baseColor.A, overlayAlpha = overlay.A, alpha = (byte)(overlayAlpha + (baseAlpha * (255 - overlayAlpha) / 255));
            if (alpha == 0) return Color.Transparent;
            else
            {
                byte r = (byte)((overlay.R * overlayAlpha + baseColor.R * baseAlpha * (255 - overlayAlpha) / 255) / alpha),
                    g = (byte)((overlay.G * overlayAlpha + baseColor.G * baseAlpha * (255 - overlayAlpha) / 255) / alpha),
                    b = (byte)((overlay.B * overlayAlpha + baseColor.B * baseAlpha * (255 - overlayAlpha) / 255) / alpha);
                return Color.FromArgb(alpha, r, g, b);
            }
        }
    }

    public enum Colour
    {
        #region 品牌色

        /// <summary>
        /// 品牌色
        /// </summary>
        Primary,

        /// <summary>
        /// 文本颜色
        /// </summary>
        PrimaryColor,

        /// <summary>
        /// 主色悬浮态（按钮、开关、复选框）
        /// </summary>
        PrimaryHover,

        /// <summary>
        /// 主色激活态（按钮动画）
        /// </summary>
        PrimaryActive,

        /// <summary>
        /// 主色背景色（按钮底部、下拉激活、文本框激活、菜单激活）
        /// </summary>
        PrimaryBg,

        /// <summary>
        /// 主色背景悬浮态
        /// </summary>
        PrimaryBgHover,

        /// <summary>
        /// 主色的描边色
        /// </summary>
        PrimaryBorder,

        /// <summary>
        /// 主色描边色悬浮态
        /// </summary>
        PrimaryBorderHover,

        #endregion

        #region 成功色

        /// <summary>
        /// 成功色
        /// </summary>
        Success,

        /// <summary>
        /// 文本颜色
        /// </summary>
        SuccessColor,

        /// <summary>
        /// 成功色的背景颜色
        /// </summary>
        SuccessBg,

        /// <summary>
        /// 成功色的描边色
        /// </summary>
        SuccessBorder,

        /// <summary>
        /// 成功色的悬浮态
        /// </summary>
        SuccessHover,

        /// <summary>
        /// 成功色的激活态
        /// </summary>
        SuccessActive,

        #endregion

        #region 警戒色

        /// <summary>
        /// 警戒色
        /// </summary>
        Warning,

        /// <summary>
        /// 文本颜色
        /// </summary>
        WarningColor,

        /// <summary>
        /// 警戒色的背景颜色
        /// </summary>
        WarningBg,

        /// <summary>
        /// 警戒色的描边色
        /// </summary>
        WarningBorder,

        /// <summary>
        /// 警戒色的悬浮态
        /// </summary>
        WarningHover,

        /// <summary>
        /// 警戒色的激活态
        /// </summary>
        WarningActive,

        #endregion

        #region 错误色

        /// <summary>
        /// 错误色
        /// </summary>
        Error,

        /// <summary>
        /// 文本颜色
        /// </summary>
        ErrorColor,

        /// <summary>
        /// 警戒色的背景颜色（按钮底部）
        /// </summary>
        ErrorBg,

        /// <summary>
        /// 警戒色的描边色
        /// </summary>
        ErrorBorder,

        /// <summary>
        /// 错误色的悬浮态
        /// </summary>
        ErrorHover,

        /// <summary>
        /// 错误色的激活态
        /// </summary>
        ErrorActive,

        #endregion

        #region 信息色

        /// <summary>
        /// 信息色
        /// </summary>
        Info,

        /// <summary>
        /// 文本颜色
        /// </summary>
        InfoColor,

        /// <summary>
        /// 信息色的背景颜色（按钮底部）
        /// </summary>
        InfoBg,

        /// <summary>
        /// 信息色的描边色
        /// </summary>
        InfoBorder,

        /// <summary>
        /// 信息色的悬浮态
        /// </summary>
        InfoHover,

        /// <summary>
        /// 信息色的激活态
        /// </summary>
        InfoActive,

        #endregion

        DefaultBg,
        DefaultColor,
        DefaultBorder,

        TagDefaultBg,
        TagDefaultColor,

        #region 中性色

        /// <summary>
        /// 基础文本色
        /// </summary>
        TextBase,

        /// <summary>
        /// 一级文本色（菜单颜色、非激活下颜色、小清除按钮悬浮态）
        /// </summary>
        Text,

        /// <summary>
        /// 二级文本色
        /// </summary>
        TextSecondary,

        /// <summary>
        /// 三级文本色（小清除按钮）
        /// </summary>
        TextTertiary,

        /// <summary>
        /// 四级文本色（禁用色）
        /// </summary>
        TextQuaternary,

        /// <summary>
        /// 基础背景色
        /// </summary>
        BgBase,

        /// <summary>
        /// 组件的容器背景色 例如：默认按钮、输入框等。务必不要将其与 `colorBgElevated` 混淆。
        /// </summary>
        BgContainer,

        /// <summary>
        /// 浮层容器背景色，在暗色模式下该 token 的色值会比 `colorBgContainer` 要亮一些。例如：模态框、弹出框、菜单等。
        /// </summary>
        BgElevated,

        /// <summary>
        /// 该色用于页面整体布局的背景色，只有需要在页面中处于 B1 的视觉层级时才会使用该 token，其他用法都是错误的
        /// </summary>
        BgLayout,

        /// <summary>
        /// 一级填充色
        /// </summary>
        Fill,

        /// <summary>
        /// 二级填充色（分页悬浮态、菜单悬浮态）
        /// </summary>
        FillSecondary,

        /// <summary>
        /// 三级填充色（下拉悬浮态）
        /// </summary>
        FillTertiary,

        /// <summary>
        /// 四级填充色（幽灵按钮底部）
        /// </summary>
        FillQuaternary,

        /// <summary>
        /// 边框颜色
        /// </summary>
        BorderColor,
        BorderSecondary,

        /// <summary>
        /// 禁用边框颜色
        /// </summary>
        BorderColorDisable,

        #endregion

        /// <summary>
        /// 用于作为分割线的颜色，此颜色和 BorderSecondary 的颜色一致，但是用的是透明色
        /// </summary>
        Split,

        /// <summary>
        /// 选项悬浮态背景颜色
        /// </summary>
        HoverBg,

        /// <summary>
        /// 选项悬浮态文本颜色
        /// </summary>
        HoverColor,

        SliderHandleColorDisabled
    }

    public class HSL
    {
        public HSL(float hue, float saturation, float lightness)
        {
            h = hue;
            s = saturation;
            l = lightness;
        }
        public HSL() { }
        /// <summary>
        /// 色相 取值范围为[0,360]
        /// </summary>
        public float h { get; set; }
        /// <summary>
        /// 饱和度 取值范围为[0,100]，表示颜色的深浅程度
        /// </summary>
        public float s { get; set; }
        /// <summary>
        /// 亮度 取值范围为[0,100]，表示颜色的明暗程度
        /// </summary>
        public float l { get; set; }
    }
    public class HSV
    {
        public HSV(float hue, float saturation, float value)
        {
            h = hue;
            s = saturation;
            v = value;
        }
        public HSV() { }
        /// <summary>
        /// 色相
        /// </summary>
        public float h { get; set; }
        /// <summary>
        /// 饱和度
        /// </summary>
        public float s { get; set; }
        /// <summary>
        /// 明度
        /// </summary>
        public float v { get; set; }
    }
}