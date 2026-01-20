// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Tooltip
    {
        /// <summary>
        /// Tooltip 文字提示
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="text">文本</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, string text, TAlign ArrowAlign = TAlign.Top) => open(new Config(control, text) { ArrowAlign = ArrowAlign });

        /// <summary>
        /// Tooltip 文字提示
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="text">文本</param>
        /// <param name="rect">偏移量</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, string text, Rectangle rect, TAlign ArrowAlign = TAlign.Top) => open(new Config(control, text) { Offset = rect, ArrowAlign = ArrowAlign });

        /// <summary>
        /// Tooltip 文字提示
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(Config config)
        {
            if (config.Control.IsHandleCreated)
            {
                if (config.Control.InvokeRequired) return ITask.Invoke(config.Control, new Func<Form?>(() => open(config)));
                var tip = new TooltipForm(config.Control, config.Text, config);
                tip.Show(config.Control);
                return tip;
            }
            return null;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config : ITooltipConfig
        {
            /// <summary>
            /// Tooltip 配置
            /// </summary>
            /// <param name="control">所属控件</param>
            /// <param name="text">文本</param>
            public Config(Control control, string text)
            {
                Font = control.Font;
                Control = control;
                Text = text;
            }

            /// <summary>
            /// 所属控件
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            /// 偏移量
            /// </summary>
            public Rectangle? Offset { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 文本
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            /// <summary>
            /// 箭头大小
            /// </summary>
            public int? ArrowSize { get; set; }

            /// <summary>
            /// 箭头方向
            /// </summary>
            public TAlign ArrowAlign { get; set; } = TAlign.Top;

            /// <summary>
            /// 自定义宽度
            /// </summary>
            public int? CustomWidth { get; set; }

            /// <summary>
            /// 背景色
            /// </summary>
            public Color? Back { get; set; }

            /// <summary>
            /// 前景色
            /// </summary>
            public Color? Fore { get; set; }

            #region 设置

            public Config SetOffset(Rectangle? value)
            {
                Offset = value;
                return this;
            }
            public Config SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public Config SetRadius(int value = 0)
            {
                Radius = value;
                return this;
            }
            public Config SetArrow(int? value)
            {
                ArrowSize = value;
                return this;
            }
            public Config SetArrow(TAlign value)
            {
                ArrowAlign = value;
                return this;
            }
            public Config SetCustomWidth(int? value)
            {
                CustomWidth = value;
                return this;
            }
            public Config SetBack(Color? value)
            {
                Back = value;
                return this;
            }
            public Config SetFore(Color? value)
            {
                Fore = value;
                return this;
            }

            #endregion
        }
    }
}