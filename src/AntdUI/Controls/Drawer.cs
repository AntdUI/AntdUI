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
    /// <summary>
    /// Drawer 抽屉
    /// </summary>
    /// <remarks>屏幕边缘滑出的浮层面板。</remarks>
    public static class Drawer
    {
        /// <summary>
        /// Drawer 抽屉
        /// </summary>
        /// <param name="form">所属控件</param>
        /// <param name="content">控件</param>
        /// <param name="Align">方向</param>
        public static Form? open(Form form, Control content, TAlignMini Align = TAlignMini.Right) => open(new Config(form, content) { Align = Align });

        #region 配置

        /// <summary>
        /// Drawer 配置
        /// </summary>
        /// <param name="form">所属控件</param>
        /// <param name="content">控件</param>
        /// <param name="Align">方向</param>
        public static Config config(Form form, Control content, TAlignMini Align = TAlignMini.Right) => new Config(form, content) { Align = Align };

        #endregion

        /// <summary>
        /// Drawer 抽屉
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(this Config config)
        {
            if (config.Form.IsHandleCreated)
            {
                if (config.Form.InvokeRequired) return ITask.Invoke(config.Form, new Func<Form?>(() => open(config)));
                if (config.Mask)
                {
                    var mask = new LayeredFormMask(config.Form);
                    mask.Show(config.Form);
                    var frm = new LayeredFormDrawer(config, mask);
                    ITask.Run(() =>
                    {
                        if (config.DisplayDelay > 0) System.Threading.Thread.Sleep(config.DisplayDelay);
                        if (frm.isclose) return;
                        config.Form.BeginInvoke(new Action(() =>
                        {
                            frm.Show(mask);
                        }));
                    });
                    return frm;
                }
                else
                {
                    var frm = new LayeredFormDrawer(config);
                    frm.Show(config.Form);
                    return frm;
                }
            }
            return null;
        }

#if !NET40

        /// <summary>
        /// Drawer 抽屉 等待
        /// </summary>
        /// <param name="form">所属控件</param>
        /// <param name="content">控件</param>
        /// <param name="Align">方向</param>
        public static async System.Threading.Tasks.Task<Config?> wait(Form form, Control content, TAlignMini Align = TAlignMini.Right) => await wait(new Config(form, content) { Align = Align });

        /// <summary>
        /// Drawer 抽屉 等待
        /// </summary>
        /// <param name="config">配置</param>
        public static async System.Threading.Tasks.Task<Config?> wait(this Config config)
        {
            return await ITask.Run(() =>
            {
                if (config.Form.IsHandleCreated)
                {
                    using (var resetEvent = new System.Threading.ManualResetEvent(false))
                    {
                        if (config.Mask)
                        {
                            var mask = ITask.Invoke(config.Form, new Func<LayeredFormMask>(() =>
                            {
                                var mask = new LayeredFormMask(config.Form);
                                mask.Show(config.Form);
                                return mask;
                            }));
                            var frm = ITask.Invoke(config.Form, new Func<LayeredFormDrawer>(() => new LayeredFormDrawer(config, mask)));
                            if (config.DisplayDelay > 0) System.Threading.Thread.Sleep(config.DisplayDelay);
                            if (frm.isclose) return config;
                            config.Form.BeginInvoke(new Action(() =>
                            {
                                frm.Show(mask);
                                frm.FormClosed += (a, b) => resetEvent.Set();
                            }));
                        }
                        else
                        {
                            var frm = new LayeredFormDrawer(config);
                            frm.Show(config.Form);
                            frm.FormClosed += (a, b) => resetEvent.Set();
                        }
                        resetEvent.Wait();
                    }
                    return config;
                }
                return null;
            });
        }

#endif

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// Drawer 配置
            /// </summary>
            /// <param name="form">所属控件</param>
            /// <param name="content">控件</param>
            public Config(Form form, Control content)
            {
                Form = form;
                Content = content;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; set; }

            /// <summary>
            /// 控件
            /// </summary>
            public Control Content { get; set; }

            /// <summary>
            /// 色彩模式
            /// </summary>
            public TAMode ColorScheme { get; set; } = TAMode.Auto;

            /// <summary>
            /// 是否展示遮罩
            /// </summary>
            public bool Mask { get; set; } = true;

            /// <summary>
            /// 点击蒙层是否允许关闭
            /// </summary>
            public bool MaskClosable { get; set; } = true;

            /// <summary>
            /// 关闭后手动激活父窗口
            /// </summary>
            public bool ManualActivateParent { get; set; }

            /// <summary>
            /// 边距
            /// </summary>
            public int Padding { get; set; } = 24;

            /// <summary>
            /// 方向
            /// </summary>
            public TAlignMini Align { get; set; } = TAlignMini.Right;

            /// <summary>
            /// 是否释放
            /// </summary>
            public bool Dispose { get; set; } = true;

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            /// <summary>
            /// 加载回调
            /// </summary>
            public Action? OnLoad { get; set; }

            /// <summary>
            /// 关闭回调
            /// </summary>
            public Action? OnClose { get; set; }

            /// <summary>
            /// 显示延迟
            /// </summary>
            public int DisplayDelay { get; set; } = 100;

            #region 设置

            public Config SetColorScheme(TAMode value)
            {
                ColorScheme = value;
                return this;
            }
            public Config SetMask(bool value = false)
            {
                Mask = value;
                return this;
            }
            public Config SetMaskClosable(bool value = false)
            {
                MaskClosable = value;
                return this;
            }
            public Config SetManualActivateParent(bool value = true)
            {
                ManualActivateParent = value;
                return this;
            }
            public Config SetPadding(int value)
            {
                Padding = value;
                return this;
            }
            public Config SetAlign(TAlignMini value = TAlignMini.Left)
            {
                Align = value;
                return this;
            }
            public Config SetDispose(bool value = false)
            {
                Dispose = value;
                return this;
            }
            public Config SetTag(object? value)
            {
                Tag = value;
                return this;
            }
            public Config SetOnLoad(Action? value)
            {
                OnLoad = value;
                return this;
            }
            public Config SetOnClose(Action? value)
            {
                OnClose = value;
                return this;
            }
            public Config SetDisplayDelay(int value = 0)
            {
                DisplayDelay = value;
                return this;
            }

            #endregion
        }
    }
}