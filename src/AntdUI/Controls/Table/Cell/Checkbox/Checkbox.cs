// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// 复选框
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellCheckbox : ICell
    {
        /// <summary>
        /// 复选框
        /// </summary>
        public CellCheckbox() { }

        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="Checked">复选值</param>
        public CellCheckbox(bool Checked = true)
        {
            _checked = Checked;
        }

        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="text">文本</param>
        public CellCheckbox(string? text) { _text = text; }

        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="Checked">复选值</param>
        public CellCheckbox(string? text, bool Checked = true)
        {
            _text = text;
            _checked = Checked;
        }

        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="fore">文字颜色</param>
        public CellCheckbox(string? text, Color fore)
        {
            _text = text;
            _fore = fore;
        }

        #region 属性

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

        Color? fill;
        /// <summary>
        /// 填充颜色
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

        #region 选中状态

        bool AnimationCheck = false;
        float AnimationCheckValue = 0;
        bool _checked = false;
        /// <summary>
        /// 选中状态
        /// </summary>
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                OnCheck();
                CheckState = value ? CheckState.Checked : CheckState.Unchecked;
                CheckedChanged?.Invoke(this, new BoolEventArgs(value));
                OnPropertyChanged();
            }
        }

        internal CheckState checkStateOld = CheckState.Unchecked;
        CheckState checkState = CheckState.Unchecked;
        /// <summary>
        /// 选中状态
        /// </summary>
        public CheckState CheckState
        {
            get => checkState;
            set
            {
                if (checkState == value) return;
                checkState = value;
                bool __checked = value == CheckState.Checked;
                if (_checked != __checked)
                {
                    _checked = __checked;
                    OnCheck();
                }
                if (value != CheckState.Unchecked) checkStateOld = value;
            }
        }

        void OnCheck()
        {
            ThreadCheck?.Dispose();
            try
            {
                if (PARENT.PARENT.IsHandleCreated && Config.HasAnimation(nameof(Table)))
                {
                    if (!_checked && checkStateOld == CheckState.Checked && CheckState == CheckState.Indeterminate)
                    {
                        AnimationCheckValue = 1F;
                        OnPropertyChanged();
                    }
                    else
                    {
                        AnimationCheck = true;
                        ThreadCheck = new AnimationTask(new AnimationLinearFConfig(PARENT.PARENT, i =>
                        {
                            AnimationCheckValue = i;
                            OnPropertyChanged();
                            return true;
                        }, 20).SetValue(AnimationCheckValue, _checked, 0.2F).SetEnd(() => AnimationCheck = false));
                    }
                }
                else AnimationCheckValue = _checked ? 1F : 0F;
            }
            catch { }
        }

        #endregion

        AnimationTask? ThreadCheck, ThreadHover;

        int AnimationHoverValue = 0;
        bool AnimationHover = false;
        bool _mouseHover = false;
        internal bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                if (enabled)
                {
                    if (Config.HasAnimation(nameof(Table)))
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        ThreadHover = new AnimationTask(new AnimationLinearConfig(PARENT.PARENT, i =>
                        {
                            AnimationHoverValue = i;
                            OnPropertyChanged();
                            return true;
                        }, 10).SetValueColor(AnimationHoverValue, value, 20).SetEnd(() => AnimationHover = false));
                    }
                    else AnimationHoverValue = 255;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        [Description("点击时自动改变选中状态"), Category("行为"), DefaultValue(true)]
        public bool AutoCheck { get; set; } = true;

        #endregion

        #region 设置

        public CellCheckbox SetText(string? value, string? localization = null)
        {
            _text = value;
            LocalizationText = localization;
            return this;
        }
        public CellCheckbox SetLocalizationText(string? value)
        {
            LocalizationText = value;
            return this;
        }

        public CellCheckbox SetText(string? value)
        {
            _text = value;
            return this;
        }
        public CellCheckbox SetFore(Color? value)
        {
            _fore = value;
            return this;
        }
        public CellCheckbox SetFont(Font? value)
        {
            _font = value;
            return this;
        }
        public CellCheckbox SetFill(Color? value)
        {
            fill = value;
            return this;
        }
        public CellCheckbox SetChecked(bool value = true)
        {
            _checked = value;
            return this;
        }
        public CellCheckbox SetChecked(CheckState value)
        {
            checkState = value;
            return this;
        }
        public CellCheckbox SetEnabled(bool value = false)
        {
            enabled = value;
            return this;
        }
        public CellCheckbox SetAutoCheck(bool value = false)
        {
            AutoCheck = value;
            return this;
        }

        #endregion

        #region 事件

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? CheckedChanged;

        #endregion

        public override string? ToString() => Text;
    }
}