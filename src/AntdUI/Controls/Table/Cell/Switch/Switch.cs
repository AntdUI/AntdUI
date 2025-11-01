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

using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 开关
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellSwitch : ICell
    {
        /// <summary>
        /// 开关
        /// </summary>
        public CellSwitch() { }

        /// <summary>
        /// 开关
        /// </summary>
        /// <param name="Checked">复选值</param>
        public CellSwitch(bool Checked = true)
        {
            _checked = Checked;
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

        #region 文本

        string? _checkedText, _unCheckedText;

        [Description("选中时显示的文本"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? CheckedText
        {
            get => Localization.GetLangI(LocalizationCheckedText, _checkedText);
            set
            {
                if (_checkedText == value) return;
                _checkedText = value;
                if (_checked) OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 国际化选中时显示的文本
        /// </summary>
        public string? LocalizationCheckedText { get; set; }

        /// <summary>
        /// 未选中时显示的文本
        /// </summary>
        public string? UnCheckedText
        {
            get => Localization.GetLangI(LocalizationUnCheckedText, _unCheckedText);
            set
            {
                if (_unCheckedText == value) return;
                _unCheckedText = value;
                if (!_checked) OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 国际化未选中时显示的文本
        /// </summary>
        public string? LocalizationUnCheckedText { get; set; }

        #endregion

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

        /// <summary>
        /// 悬停颜色
        /// </summary>
        public Color? FillHover { get; set; }

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
                CheckedChanged?.Invoke(this, new BoolEventArgs(value));
                OnCheck();
            }
        }

        bool AnimationCheck = false;
        float AnimationCheckValue = 0;
        void OnCheck()
        {
            ThreadCheck?.Dispose();
            if (PARENT.PARENT.IsHandleCreated && Config.HasAnimation(nameof(Table)))
            {
                if (Config.HasAnimation(nameof(Table)))
                {
                    AnimationCheck = true;
                    if (_checked)
                    {
                        ThreadCheck = new ITask(PARENT.PARENT, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(0.1F);
                            if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                            OnPropertyChanged();
                            return true;
                        }, 10, () =>
                        {
                            AnimationCheck = false;
                            OnPropertyChanged();
                        });
                    }
                    else
                    {
                        ThreadCheck = new ITask(PARENT.PARENT, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(-0.1F);
                            if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                            OnPropertyChanged();
                            return true;
                        }, 10, () =>
                        {
                            AnimationCheck = false;
                            OnPropertyChanged();
                        });
                    }
                }
                else
                {
                    AnimationCheckValue = _checked ? 1F : 0F;
                    OnPropertyChanged();
                }
            }
        }

        ITask? ThreadCheck, ThreadHover;

        float AnimationHoverValue = 0;
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
                    if (PARENT.PARENT.IsHandleCreated && Config.HasAnimation(nameof(Table)))
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(PARENT.PARENT, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(0.1F);
                                if (AnimationHoverValue > 1) { AnimationHoverValue = 1F; return false; }
                                OnPropertyChanged();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                OnPropertyChanged();
                            });
                        }
                        else
                        {
                            ThreadHover = new ITask(PARENT.PARENT, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(-0.1F);
                                if (AnimationHoverValue <= 0) { AnimationHoverValue = 0F; return false; }
                                OnPropertyChanged();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                OnPropertyChanged();
                            });
                        }
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

        #region 加载中

        bool loading = false;
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                if (PARENT.PARENT.IsHandleCreated && Config.HasAnimation(nameof(Table)))
                {
                    if (loading)
                    {
                        bool ProgState = false;
                        ThreadLoading = new ITask(PARENT.PARENT, () =>
                        {
                            if (ProgState)
                            {
                                LineAngle = LineAngle.Calculate(9F);
                                LineWidth = LineWidth.Calculate(0.6F);
                                if (LineWidth > 75) ProgState = false;
                            }
                            else
                            {
                                LineAngle = LineAngle.Calculate(9.6F);
                                LineWidth = LineWidth.Calculate(-0.6F);
                                if (LineWidth < 6) ProgState = true;
                            }
                            if (LineAngle >= 360) LineAngle = 0;
                            OnPropertyChanged();
                            return true;
                        }, 10);
                    }
                    else ThreadLoading?.Dispose();
                }
                OnPropertyChanged();
            }
        }

        ITask? ThreadLoading;
        internal float LineWidth = 6, LineAngle = 0;

        #endregion

        #endregion

        #region 设置

        public CellSwitch SetCheckedText(string? value, string? localization = null)
        {
            _checkedText = value;
            LocalizationCheckedText = localization;
            return this;
        }
        public CellSwitch SetUnCheckedText(string? value, string? localization = null)
        {
            _unCheckedText = value;
            LocalizationUnCheckedText = localization;
            return this;
        }
        public CellSwitch SetFore(Color? value)
        {
            _fore = value;
            return this;
        }
        public CellSwitch SetFont(Font? value)
        {
            _font = value;
            return this;
        }
        public CellSwitch SetFill(Color? value)
        {
            fill = value;
            return this;
        }
        public CellSwitch SetFillHover(Color? value)
        {
            FillHover = value;
            return this;
        }
        public CellSwitch SetChecked(bool value = true)
        {
            _checked = value;
            return this;
        }
        public CellSwitch SetEnabled(bool value = false)
        {
            enabled = value;
            return this;
        }
        public CellSwitch SetAutoCheck(bool value = false)
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

        public override string? ToString() => _checked.ToString();
    }
}