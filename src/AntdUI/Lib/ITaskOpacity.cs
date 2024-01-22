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
using System.Windows.Forms;

namespace AntdUI
{
    public class ITaskOpacity : IDisposable
    {
        Control control;
        Action action;
        public ITaskOpacity(ILayeredForm _control)
        {
            control = _control;
            action = new Action(() =>
            {
                _control.Print();
            });
        }
        public ITaskOpacity(Form _control)
        {
            control = _control;
            action = new Action(() =>
            {
                _control.Invalidate();
            });
        }
        public ITaskOpacity(IControl _control)
        {
            control = _control;
            action = new Action(() =>
            {
                _control.Invalidate();
            });
        }

        ITask? Thread = null;

        bool _switch = false;
        public bool Switch
        {
            get => _switch;
            set
            {
                if (_switch != value)
                {
                    _switch = value;
                    if (Config.Animation)
                    {
                        Thread?.Dispose();
                        Animation = true;
                        var prog = (int)(MaxValue * 0.078F);
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
                    else { Value = _switch ? MaxValue : 0; action(); }
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

        public int MaxValue { get; set; } = 255;
        public int Value { get; private set; }
        public bool Animation { get; private set; }
        public void Dispose()
        {
            Thread?.Dispose();
        }
    }
}