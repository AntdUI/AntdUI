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
using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    public static class Style
    {
        public static Theme.IColor<Color> Db = new Theme.Light();

        /// <summary>
        /// 加载自定义主题
        /// </summary>
        /// <param name="style"></param>
        public static void LoadCustom(Theme.IColor<Color> style)
        {
            Db = style;
            EventHub.Dispatch(EventType.THEME);
        }

        public static void LoadCustom(Theme.IColor<string> style)
        {
            Db.Primary = style.Primary.ToColor();
            Db.PrimaryColor = style.PrimaryColor.ToColor();
            Db.PrimaryHover = style.PrimaryHover.ToColor();
            Db.PrimaryActive = style.PrimaryActive.ToColor();
            Db.PrimaryBg = style.PrimaryBg.ToColor();

            Db.Success = style.Success.ToColor();
            Db.SuccessColor = style.SuccessColor.ToColor();
            Db.SuccessBg = style.SuccessBg.ToColor();
            Db.SuccessBorder = style.SuccessBorder.ToColor();
            Db.SuccessHover = style.SuccessHover.ToColor();
            Db.SuccessActive = style.SuccessActive.ToColor();

            Db.Warning = style.Warning.ToColor();
            Db.WarningColor = style.WarningColor.ToColor();
            Db.WarningBg = style.WarningBg.ToColor();
            Db.WarningBorder = style.WarningBorder.ToColor();
            Db.WarningHover = style.WarningHover.ToColor();
            Db.WarningActive = style.WarningActive.ToColor();

            Db.Error = style.Error.ToColor();
            Db.ErrorColor = style.ErrorColor.ToColor();
            Db.ErrorBg = style.ErrorBg.ToColor();
            Db.ErrorBorder = style.ErrorBorder.ToColor();
            Db.ErrorHover = style.ErrorHover.ToColor();
            Db.ErrorActive = style.ErrorActive.ToColor();

            Db.Info = style.Info.ToColor();
            Db.InfoColor = style.InfoColor.ToColor();
            Db.InfoBg = style.InfoBg.ToColor();
            Db.InfoBorder = style.InfoBorder.ToColor();
            Db.InfoHover = style.InfoHover.ToColor();
            Db.InfoActive = style.InfoActive.ToColor();

            Db.DefaultBg = style.DefaultBg.ToColor();
            Db.DefaultColor = style.DefaultColor.ToColor();
            Db.DefaultBorder = style.DefaultBorder.ToColor();

            Db.TagDefaultBg = style.TagDefaultBg.ToColor();
            Db.TagDefaultColor = style.TagDefaultColor.ToColor();

            Db.TextBase = style.TextBase.ToColor();
            Db.Text = style.Text.ToColor();
            Db.TextSecondary = style.TextSecondary.ToColor();
            Db.TextTertiary = style.TextTertiary.ToColor();
            Db.TextQuaternary = style.TextQuaternary.ToColor();

            Db.BgBase = style.BgBase.ToColor();
            Db.BgContainer = style.BgContainer.ToColor();
            Db.BgElevated = style.BgElevated.ToColor();
            Db.BgLayout = style.BgLayout.ToColor();

            Db.Fill = style.Fill.ToColor();
            Db.FillSecondary = style.FillSecondary.ToColor();
            Db.FillTertiary = style.FillTertiary.ToColor();
            Db.FillQuaternary = style.FillQuaternary.ToColor();

            Db.BorderColor = style.BorderColor.ToColor();
            Db.BorderSecondary = style.BorderSecondary.ToColor();
            Db.BorderColorDisable = style.BorderColorDisable.ToColor();

            Db.Split = style.Split.ToColor();

            Db.HoverBg = style.HoverBg.ToColor();
            Db.HoverColor = style.HoverColor.ToColor();

            Db.SliderHandleColorDisabled = style.SliderHandleColorDisabled.ToColor();

            EventHub.Dispatch(EventType.THEME);
        }

        /// <summary>
        /// 色彩模式（浅色、暗色）
        /// </summary>
        /// <returns>true Light;false Dark</returns>
        public static bool ColorMode(this Color color)
        {
            return ((color.R * 299 + color.G * 587 + color.B * 114) / 1000) > 128;
        }

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
            for (var i = lightColorCount; i > 0; i--)
            {
                colors.Add(GenerateColor(hsv, i, true));
            }
            // 主色
            colors.Add(primaryColor);
            // 主色后
            for (var i = 1; i <= darkColorCount; i++)
            {
                colors.Add(GenerateColor(hsv, i, false));
            }
            return colors;
        }

        /// <summary>
        /// 生成色卡
        /// </summary>
        /// <param name="h">色调</param>
        /// <param name="s">饱和度</param>
        /// <param name="v">亮度</param>
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

        /// <summary>
        /// 颜色：16进制转成RGB
        /// </summary>
        /// <param name="hex">设置16进制颜色 [返回RGB]</param>
        /// <returns></returns>
        public static Color ToColor(this string hex)
        {
            try
            {
                if (hex != null && hex.Length > 5)
                {
                    if (hex.StartsWith("#")) hex = hex.Substring(1);
                    if (hex.Length == 6) return Color.FromArgb(hex.Substring(0, 2).HexToInt(), hex.Substring(2, 2).HexToInt(), hex.Substring(4, 2).HexToInt());
                    else if (hex.Length == 8) return Color.FromArgb(hex.Substring(6, 2).HexToInt(), hex.Substring(0, 2).HexToInt(), hex.Substring(2, 2).HexToInt(), hex.Substring(4, 2).HexToInt());
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

        static int HexToInt(this string str)
        {
            return int.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

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