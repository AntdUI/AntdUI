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

namespace AntdUI
{
    /// <summary>
    /// 文字
    /// </summary>
    public partial class CellText : ICell
    {
        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="text">文本</param>
        public CellText(string text) { _text = text; }

        /// <summary>
        /// 文字
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="fore">文字颜色</param>
        public CellText(string text, Color fore)
        {
            _text = text;
            _fore = fore;
        }

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
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged(true);
            }
        }

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

        Image? prefix = null;
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

        string? prefixSvg = null;
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
        public bool HasPrefix
        {
            get => prefixSvg != null || prefix != null;
        }

        Image? suffix = null;
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

        string? suffixSvg = null;
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
        public bool HasSuffix
        {
            get => suffixSvg != null || suffix != null;
        }

        #endregion

        public override string? ToString()
        {
            return _text;
        }
    }
}