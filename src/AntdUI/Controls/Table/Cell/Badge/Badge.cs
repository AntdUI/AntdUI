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
    /// 徽标
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellBadge : ICell
    {
        /// <summary>
        /// 徽标
        /// </summary>
        public CellBadge() { }

        /// <summary>
        /// 徽标
        /// </summary>
        /// <param name="text">文本</param>
        public CellBadge(string? text) { _text = text; }

        /// <summary>
        /// 徽标
        /// </summary>
        /// <param name="state">状态</param>
        public CellBadge(TState state) { _state = state; }

        /// <summary>
        /// 徽标
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="text">文本</param>
        public CellBadge(TState state, string? text)
        {
            _state = state;
            _text = text;
        }

        #region 属性

        Color? fore;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                OnPropertyChanged();
            }
        }

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                OnPropertyChanged();
            }
        }

        TState _state = TState.Default;
        /// <summary>
        /// 状态
        /// </summary>
        public TState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                var old = _state;
                _state = value;
                if (value == TState.Processing) OnPropertyChanged(true);
                else if (old == TState.Processing) OnPropertyChanged(true);
                else OnPropertyChanged();
            }
        }

        float dotratio = .4F;
        /// <summary>
        /// 点比例
        /// </summary>
        public float DotRatio
        {
            get => dotratio;
            set
            {
                if (dotratio == value) return;
                dotratio = value;
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

        #endregion

        #region 设置

        public CellBadge SetText(string? value, string? localization = null)
        {
            _text = value;
            LocalizationText = localization;
            return this;
        }
        public CellBadge SetLocalizationText(string? value)
        {
            LocalizationText = value;
            return this;
        }
        public CellBadge SetFore(Color? value)
        {
            fore = value;
            return this;
        }
        public CellBadge SetFill(Color? value)
        {
            fill = value;
            return this;
        }

        public CellBadge SetState(TState value = TState.Success)
        {
            _state = value;
            return this;
        }

        public CellBadge SetDotRatio(float value)
        {
            dotratio = value;
            return this;
        }

        #endregion

        public override string? ToString() => Text;
    }
}