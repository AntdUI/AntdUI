// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
            action = () => _control.Print();
        }
        public ITaskOpacity(string k, Form _control)
        {
            key = k;
            control = _control;
            action = () => _control.Invalidate();
        }
        public ITaskOpacity(string k, IControl _control)
        {
            key = k;
            control = _control;
            action = () => _control.Invalidate();
        }

        AnimationTask? Thread;

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
                if (SwitchDown == value) return;
                _switch = value;
                if (Config.HasAnimation(key))
                {
                    Thread?.Dispose();
                    Animation = true;
                    var prog = (int)(MaxValue * .078F);
                    if (value)
                    {
                        Thread = new AnimationTask(new AnimationLinearConfig(control, i =>
                        {
                            Value = i;
                            action();
                            return true;
                        }, 10).SetAdd(prog).SetMax(MaxValue).SetValue(Value).SetEnd(() =>
                        {
                            Value = MaxValue;
                            Animation = false;
                            action();
                        }));
                    }
                    else
                    {
                        Down = false;
                        Thread = new AnimationTask(new AnimationLinearConfig(control, i =>
                        {
                            Value = i;
                            action();
                            return true;
                        }, 10).SetAdd(-prog).SetMax(0).SetValue(Value).SetEnd(() =>
                        {
                            Value = 0;
                            Animation = false;
                            action();
                        }));
                    }
                }
                else
                {
                    if (_switch) Value = MaxValue;
                    else
                    {
                        Down = false;
                        Value = _switch ? MaxValue : 0;
                    }
                    action();
                }
            }
        }

        public bool SwitchDown => _down || _switch;

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
            Switch = Down = false;
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