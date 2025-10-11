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
    /// 超链接
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellLink : ICell
    {
        /// <summary>
        /// 超链接
        /// </summary>
        /// <param name="id">id</param>
        public CellLink(string id) { Id = id; }

        /// <summary>
        /// 超链接
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="text">文本</param>
        public CellLink(string id, string? text) { Id = id; this.text = text; }

        #region 属性

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        #region 文本

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get => Localization.GetLangI(LocalizationText, text, new string?[] { "{id}", Id });
            set
            {
                if (text == value) return;
                text = value;
                OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 国际化文本
        /// </summary>
        public string? LocalizationText { get; set; }

        internal StringFormat stringFormat = Helper.SF_NoWrap();

        ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 文本位置
        /// </summary>
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
                OnPropertyChanged();
            }
        }

        #endregion

        bool enabled = true;
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) enabled = value;
                enabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 文本提示
        /// </summary>
        public string? Tooltip { get; set; }

        #endregion

        #region 设置

        public CellLink SetText(string? value, string? localization = null)
        {
            text = value;
            LocalizationText = localization;
            return this;
        }
        public CellLink SetLocalizationText(string? value)
        {
            LocalizationText = value;
            return this;
        }
        public CellLink SetTextAlign(ContentAlignment align = ContentAlignment.MiddleLeft)
        {
            textAlign = align;
            textAlign.SetAlignment(ref stringFormat);
            return this;
        }
        public CellLink SetEnabled(bool value = false)
        {
            enabled = value;
            return this;
        }
        public CellLink SetTooltip(string? tooltip)
        {
            Tooltip = tooltip;
            return this;
        }


        #endregion

        public override string? ToString() => Text;
    }
}