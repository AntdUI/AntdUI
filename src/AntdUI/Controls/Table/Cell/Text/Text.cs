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

using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 文字
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellText : ICell
    {
        /// <summary>
        /// 文字
        /// </summary>
        public CellText() { }

        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="text">文本</param>
        public CellText(string? text) { _text = text; }

        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="fore">文字颜色</param>
        public CellText(string? text, Color fore)
        {
            _text = text;
            _fore = fore;
        }

        #region 属性

        Color? _back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back
        {
            get => _back;
            set
            {
                if (_back == value) return;
                _back = value;
                OnPropertyChanged();
            }
        }

        Color? _fore;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color? Fore
        {
            get => _fore;
            set
            {
                if (_fore == value) return;
                _fore = value;
                OnPropertyChanged();
            }
        }

        Font? _font;
        /// <summary>
        /// 字体
        /// </summary>
        public Font? Font
        {
            get => _font;
            set
            {
                if (_font == value) return;
                _font = value;
                OnPropertyChanged();
            }
        }

        string? _text;
        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get => Localization.GetLangI(LocalizationText, _text);
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 国际化文本
        /// </summary>
        public string? LocalizationText { get; set; }

        #region 图标

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                OnPropertyChanged(true);
            }
        }

        Image? prefix;
        /// <summary>
        /// 前缀
        /// </summary>
        public Image? Prefix
        {
            get => prefix;
            set
            {
                if (prefix == value) return;
                prefix = value;
                OnPropertyChanged(true);
            }
        }

        string? prefixSvg;
        /// <summary>
        /// 前缀SVG
        /// </summary>
        public string? PrefixSvg
        {
            get => prefixSvg;
            set
            {
                if (prefixSvg == value) return;
                prefixSvg = value;
                OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 是否包含前缀
        /// </summary>
        public bool HasPrefix => prefixSvg != null || prefix != null;

        Image? suffix;
        /// <summary>
        /// 后缀
        /// </summary>
        public Image? Suffix
        {
            get => suffix;
            set
            {
                if (suffix == value) return;
                suffix = value;
                OnPropertyChanged(true);
            }
        }

        string? suffixSvg;
        /// <summary>
        /// 后缀SVG
        /// </summary>
        public string? SuffixSvg
        {
            get => suffixSvg;
            set
            {
                if (suffixSvg == value) return;
                suffixSvg = value;
                OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 是否包含后缀
        /// </summary>
        public bool HasSuffix => suffixSvg != null || suffix != null;

        #endregion

        #endregion

        #region 设置

        public CellText SetText(string? value, string? localization = null)
        {
            _text = value;
            LocalizationText = localization;
            return this;
        }
        public CellText SetLocalizationText(string? value)
        {
            LocalizationText = value;
            return this;
        }

        public CellText SetBack(Color? value)
        {
            _back = value;
            return this;
        }
        public CellText SetFore(Color? value)
        {
            _fore = value;
            return this;
        }
        public CellText SetFont(Font? value)
        {
            _font = value;
            return this;
        }
        public CellText SetPrefix(Image? value)
        {
            prefix = value;
            return this;
        }
        public CellText SetPrefix(string? value)
        {
            prefixSvg = value;
            return this;
        }
        public CellText SetSuffix(Image? value)
        {
            suffix = value;
            return this;
        }
        public CellText SetSuffix(string? value)
        {
            suffixSvg = value;
            return this;
        }
        public CellText SetIconRatio(float value)
        {
            iconratio = value;
            return this;
        }

        #endregion

        public override string? ToString() => Text;
    }
}