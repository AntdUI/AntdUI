// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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

        internal FormatFlags s_f = FormatFlags.Center | FormatFlags.NoWrap;

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
                s_f = textAlign.SetAlignment(s_f);
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
            s_f = textAlign.SetAlignment(s_f);
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