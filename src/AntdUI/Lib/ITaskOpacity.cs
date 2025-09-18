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

using System;
using System.Windows.Forms;

namespace AntdUI
{
    public class ITaskOpacity : IDisposable
    {
        string key;
        Control control;
        Action action;
        public ITaskOpacity(string k, ILayeredFormOpacityDown _control)
        {
            key = k;
            control = _control;
            action = () =>
            {
                if (_control.RunAnimation) return;
                _control.Print();
            };
        }
        public ITaskOpacity(string k, ILayeredForm _control)
        {
            key = k;
            control = _control;
            action = () =>
            {
                _control.Print();
            };
        }
        public ITaskOpacity(string k, Form _control)
        {
            key = k;
            control = _control;
            action = () =>
            {
                _control.Invalidate();
            };
        }
        public ITaskOpacity(string k, IControl _control)
        {
            key = k;
            control = _control;
            action = () =>
            {
                _control.Invalidate();
            };
        }

        ITask? Thread;

        bool enable = true;
        public bool Enable
        {
            get => enable;
            set
            {
                if (enable == value) return;
                enable = value;
                action();
            }
        }

        bool _switch = false;
        public bool Switch
        {
            get => _switch;
            set
            {
                if (value && !enable) value = false;
                if (_switch == value) return;
                _switch = value;
                if (Config.HasAnimation(key))
                {
                    Thread?.Dispose();
                    Animation = true;
                    var prog = (int)(MaxValue * .078F);
                    if (value)
                    {
                        Thread = new ITask(control, () =>
                        {
                            Value += prog;
                            if (Value > MaxValue) { Value = MaxValue; return false; }
                            action();
                            return true;
                        }, 10, () =>
                        {
                            Value = MaxValue;
                            Animation = false;
                            action();
                        });
                    }
                    else
                    {
                        Thread = new ITask(control, () =>
                        {
                            Value -= prog;
                            if (Value < 1) { Value = 0; return false; }
                            action();
                            return true;
                        }, 10, () =>
                        {
                            Value = 0;
                            Animation = false;
                            action();
                        });
                    }
                }
                else
                {
                    Value = _switch ? MaxValue : 0;
                    action();
                }
            }
        }

        bool _down = false;
        public bool Down
        {
            get => _down;
            set
            {
                if (_down == value) return;
                _down = value;
                Thread?.Dispose();
                action();
            }
        }

        public void Clear()
        {
            Thread?.Dispose();
            Thread = null;
            _switch = _down = false;
        }

        public bool SetSwitch(bool value, ref int hand, ref int count)
        {
            if (enable)
            {
                if (value) hand++;
                if (_switch == value) return _switch;
                if (Config.HasAnimation(key)) Switch = value;
                else
                {
                    _switch = value;
                    count++;
                }
                return _switch;
            }
            else
            {
                if (_switch)
                {
                    if (Config.HasAnimation(key)) Switch = false;
                    else
                    {
                        _switch = false;
                        count++;
                    }
                }
                return false;
            }
        }

        public int MaxValue { get; set; } = 255;
        public int Value { get; private set; }
        public bool Animation { get; private set; }
        public void Dispose() => Thread?.Dispose();
    }
}