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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Popover 气泡卡片
    /// </summary>
    /// <remarks>弹出气泡式的卡片浮层。</remarks>
    public static class Popover
    {
        /// <summary>
        /// Popover 气泡卡片
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, string title, string content, TAlign ArrowAlign = TAlign.Bottom) => open(new Config(control, title, content) { ArrowAlign = ArrowAlign });

        /// <summary>
        /// Popover 气泡卡片
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="content">内容</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, string content, TAlign ArrowAlign = TAlign.Bottom) => open(new Config(control, content) { ArrowAlign = ArrowAlign });

        /// <summary>
        /// Popover 气泡卡片
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="title">标题</param>
        /// <param name="content">控件/内容</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, string title, object content, TAlign ArrowAlign = TAlign.Bottom) => open(new Config(control, title, content) { ArrowAlign = ArrowAlign });

        /// <summary>
        /// Popover 气泡卡片
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="content">控件/内容</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, object content, TAlign ArrowAlign = TAlign.Bottom) => open(new Config(control, content) { ArrowAlign = ArrowAlign });

        /// <summary>
        /// Popover 气泡卡片
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(this Config config)
        {
            if (config.Control.IsHandleCreated)
            {
                if (config.Control.InvokeRequired) return ITask.Invoke(config.Control, new Func<Form?>(() => open(config)));
                var popover = new LayeredFormPopover(config);
                popover.Show(config.Control);
                return popover;
            }
            return null;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// Popover 配置
            /// </summary>
            /// <param name="control">所属控件</param>
            /// <param name="title">标题</param>
            /// <param name="content">内容</param>
            public Config(Control control, string title, string content)
            {
                Control = control;
                Title = title;
                Content = content;
            }

            /// <summary>
            /// Popover 配置
            /// </summary>
            /// <param name="control">所属控件</param>
            /// <param name="content">内容</param>
            public Config(Control control, string content)
            {
                Control = control;
                Content = content;
            }

            /// <summary>
            /// Popover 配置
            /// </summary>
            /// <param name="control">所属控件</param>
            /// <param name="content">控件/内容</param>
            public Config(Control control, string title, object content)
            {
                Control = control;
                Title = title;
                Content = content;
            }

            /// <summary>
            /// Popover 配置
            /// </summary>
            /// <param name="control">所属控件</param>
            /// <param name="content">控件/内容</param>
            public Config(Control control, object content)
            {
                Control = control;
                Content = content;
            }

            /// <summary>
            /// 所属控件
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            /// 偏移量
            /// </summary>
            public object? Offset { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string? Title { get; set; }

            /// <summary>
            /// 控件/内容
            /// </summary>
            public object Content { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 背景色
            /// </summary>

            public Color? Back { get; set; }

            /// <summary>
            /// 文本色
            /// </summary>
            public Color? Fore { get; set; }

            /// <summary>
            /// 色彩模式
            /// </summary>
            public TMode? ColorScheme { get; set; }

            /// <summary>
            /// 控件显示后回调
            /// </summary>
            public Action? OnControlLoad { get; set; }

            /// <summary>
            /// 控件关闭前回调
            /// </summary>
            public Action<object, CancelEventArgs>? OnClosing { get; set; }

            /// <summary>
            /// 自动关闭时间（秒）0等于不关闭
            /// </summary>
            public int AutoClose { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            /// <summary>
            /// 边距
            /// </summary>
            public Size Padding { get; set; } = new Size(14, 12);

            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; } = 4;

            /// <summary>
            /// 箭头大小
            /// </summary>
            public float ArrowSize { get; set; } = 8F;

            /// <summary>
            /// 箭头方向
            /// </summary>
            public TAlign ArrowAlign { get; set; } = TAlign.Bottom;

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            /// <summary>
            /// 自定义位置
            /// </summary>
            public Rectangle? CustomPoint { get; set; }

            /// <summary>
            /// 获取焦点
            /// </summary>
            public bool Focus { get; set; } = true;

            public float? Dpi { get; set; }

            #region 设置

            public Config SetOffset(Rectangle? value)
            {
                Offset = value;
                return this;
            }
            public Config SetOffset(RectangleF value)
            {
                Offset = value;
                return this;
            }
            public Config SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public Config SetFore(Color? value)
            {
                Fore = value;
                return this;
            }
            public Config SetBack(Color? value)
            {
                Back = value;
                return this;
            }
            public Config SetColorScheme(TMode? value)
            {
                ColorScheme = value;
                return this;
            }
            public Config SetAutoClose(int value)
            {
                AutoClose = value;
                return this;
            }
            public Config SetRadius(int value = 0)
            {
                Radius = value;
                return this;
            }
            public Config SetPadding(int x, int y)
            {
                Padding = new Size(x, y);
                return this;
            }
            public Config SetPadding(int value)
            {
                Padding = new Size(value, value);
                return this;
            }
            public Config SetGap(int value)
            {
                Gap = value;
                return this;
            }
            public Config SetArrow(float value)
            {
                ArrowSize = value;
                return this;
            }
            public Config SetArrow(TAlign value)
            {
                ArrowAlign = value;
                return this;
            }
            public Config SetCustomPoint(Rectangle? value)
            {
                CustomPoint = value;
                return this;
            }
            public Config SetFocus(bool value = false)
            {
                Focus = value;
                return this;
            }
            public Config SetTag(object? value)
            {
                Tag = value;
                return this;
            }
            public Config SetOnControlLoad(Action? value)
            {
                OnControlLoad = value;
                return this;
            }
            public Config SetOnClosing(Action<object, CancelEventArgs>? value)
            {
                OnClosing = value;
                return this;
            }
            public Config SetDpi(float value)
            {
                Dpi = value;
                return this;
            }

            #endregion
        }

        /// <summary>
        /// 多列文本
        /// </summary>
        public class TextRow
        {
            public TextRow(string text)
            {
                Text = text;
            }
            public TextRow(string text, int gap)
            {
                Text = text;
                Gap = gap;
            }
            public TextRow(string text, int gap, Color fore)
            {
                Text = text;
                Gap = gap;
                Fore = fore;
            }
            public TextRow(string text, Color fore)
            {
                Text = text;
                Fore = fore;
            }

            /// <summary>
            /// 文字
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; }

            /// <summary>
            /// 文字颜色
            /// </summary>
            public Color? Fore { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 点击回调
            /// </summary>
            public Action? Call { get; set; }

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            #region 设置

            public TextRow SetGap(int value)
            {
                Gap = value;
                return this;
            }
            public TextRow SetFore(Color? value)
            {
                Fore = value;
                return this;
            }
            public TextRow SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public TextRow SetCall(Action? value)
            {
                Call = value;
                return this;
            }
            public TextRow SetTag(object? value)
            {
                Tag = value;
                return this;
            }

            #endregion
        }
    }
}