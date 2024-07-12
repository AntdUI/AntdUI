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
        public static Form? open(Form form, Control content, TAlignMini Align = TAlignMini.Right)
        {
            return open(new Config(form, content) { Align = Align });
        }

        #region 配置

        /// <summary>
        /// Drawer 配置
        /// </summary>
        /// <param name="form">所属控件</param>
        /// <param name="content">控件</param>
        /// <param name="Align">方向</param>
        public static Config config(Form form, Control content, TAlignMini Align = TAlignMini.Right)
        {
            return new Config(form, content) { Align = Align };
        }

        #endregion

        /// <summary>
        /// Drawer 抽屉
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(this Config config)
        {
            if (config.Form.IsHandleCreated)
            {
                if (config.Form.InvokeRequired)
                {
                    Form? form = null;
                    config.Form.Invoke(new Action(() =>
                    {
                        form = open(config);
                    }));
                    return form;
                }
                var frm = new LayeredFormDrawer(config);
                if (config.Mask)
                {
                    var formMask = config.Form.FormMask(config.MaskClosable, frm);
                    if (config.MaskClosable)
                    {
                        formMask.Click += (s1, e1) =>
                        {
                            frm.IClose();
                        };
                    }
                    frm.Disposed += (s1, e1) =>
                    {
                        formMask.IClose();
                    };
                    ITask.Run(() =>
                    {
                        System.Threading.Thread.Sleep(200);
                        config.Form.BeginInvoke(new Action(() =>
                        {
                            frm.Show(formMask);
                        }));
                    });
                }
                else frm.Show(config.Form);
                return frm;
            }
            return null;
        }

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
            /// 是否展示遮罩
            /// </summary>
            public bool Mask { get; set; } = true;

            /// <summary>
            /// 点击蒙层是否允许关闭
            /// </summary>
            public bool MaskClosable { get; set; } = true;

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
        }
    }
}