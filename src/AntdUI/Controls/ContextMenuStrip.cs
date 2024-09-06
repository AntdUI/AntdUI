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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// 右键菜单
    /// </summary>
    public static class ContextMenuStrip
    {
        /// <summary>
        /// ContextMenuStrip 右键菜单
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="call">点击回调</param>
        /// <param name="items">内容</param>
        public static Form? open(Control control, Action<ContextMenuStripItem> call, IContextMenuStripItem[] items, int sleep = 0)
        {
            return open(new Config(control, call, items, sleep));
        }

        /// <summary>
        /// ContextMenuStrip 右键菜单
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="notifyIcon">托盘</param>
        /// <param name="call">点击回调</param>
        /// <param name="items">内容</param>
        public static Form? open(Control control, NotifyIcon notifyIcon, Action<ContextMenuStripItem> call, IContextMenuStripItem[] items, int sleep = 0)
        {
            return open(new Config(control, call, items, sleep)
            {
                TopMost = true,
                Align = TAlign.TL
            });
        }

        /// <summary>
        /// ContextMenuStrip 右键菜单
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(this Config config)
        {
            if (config.Control.IsHandleCreated)
            {
                if (config.Control.InvokeRequired)
                {
                    Form? form = null;
                    config.Control.Invoke(new Action(() =>
                    {
                        form = open(config);
                    }));
                    return form;
                }
                var frm = new LayeredFormContextMenuStrip(config);
                frm.Show(config.Control);
                return frm;
            }
            return null;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            public Config(Control control, Action<ContextMenuStripItem> call, IContextMenuStripItem[] items, int sleep = 0)
            {
                Control = control;
                Call = call;
                Items = items;
                CallSleep = sleep;
            }

            /// <summary>
            /// 所属控件
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            /// 菜单内容
            /// </summary>
            public IContextMenuStripItem[] Items { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            /// <summary>
            /// 是否置顶
            /// </summary>
            public bool TopMost { get; set; }

            /// <summary>
            /// 延迟回调
            /// </summary>
            public int CallSleep { get; set; }

            /// <summary>
            /// 是否抢占焦点
            /// </summary>
            public bool UFocus { get; set; }

            /// <summary>
            /// 坐标
            /// </summary>
            public Point? Location { get; set; }

            /// <summary>
            /// 方向
            /// </summary>
            public TAlign Align { get; set; } = TAlign.BR;

            /// <summary>
            /// 点击回调
            /// </summary>
            public Action<ContextMenuStripItem> Call { get; set; }
        }
    }

    /// <summary>
    /// 右键菜单项
    /// </summary>
    public class ContextMenuStripItem : IContextMenuStripItem
    {
        /// <summary>
        /// 右键菜单项
        /// </summary>
        /// <param name="text">文本</param>
        public ContextMenuStripItem(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 右键菜单项
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="subtext">子文本</param>
        public ContextMenuStripItem(string text, string subtext)
        {
            Text = text;
            SubText = subtext;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 子文本
        /// </summary>
        public string? SubText { get; set; }

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color? Fore { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public Bitmap? Icon { get; set; }

        /// <summary>
        /// 图标SVG
        /// </summary>
        public string? IconSvg { get; set; }

        /// <summary>
        /// 使能
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 选中
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 子项
        /// </summary>
        public IContextMenuStripItem[]? Sub { get; set; }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        public object? Tag { get; set; }
    }

    /// <summary>
    /// 右键菜单分割项
    /// </summary>
    public class ContextMenuStripItemDivider : IContextMenuStripItem { }

    public class IContextMenuStripItem { }
}