﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
    /// Modal 对话框
    /// </summary>
    /// <remarks>模态对话框。</remarks>
    public static class Modal
    {
        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static DialogResult open(Form form, string title, string content)
        {
            return open(new Config(form, title, content));
        }

        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static DialogResult open(Form form, string title, object content)
        {
            return open(new Config(form, title, content));
        }

        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="icon">图标</param>
        public static DialogResult open(Form form, string title, string content, TType icon)
        {
            return open(new Config(form, title, content, icon));
        }

        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="config">配置</param>
        public static DialogResult open(this Config config)
        {
            if (config.Form.IsHandleCreated)
            {
                if (config.Form.InvokeRequired)
                {
                    var dialog = DialogResult.None;
                    config.Form.Invoke(new Action(() =>
                    {
                        dialog = open(config);
                    }));
                    return dialog;
                }
                var frm = new LayeredFormModal(config);
                if (config.Mask)
                {
                    var ifrm = new LayeredFormMask(config.Form);
                    ifrm.Show(config.Form);
                    frm.FormClosed += (s1, e1) =>
                    {
                        ifrm.IClose();
                    };
                    return frm.ShowDialog(ifrm);
                }
                else
                {
                    return frm.ShowDialog(config.Form);
                }
            }
            return DialogResult.None;
        }

        #region 配置

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static Config config(Form form, string title, string content)
        {
            return new Config(form, title, content);
        }

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">控件/Obj</param>
        public static Config config(Form form, string title, object content)
        {
            return new Config(form, title, content);
        }

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="icon">图标</param>
        public static Config config(Form form, string title, string content, TType icon)
        {
            return new Config(form, title, content, icon);
        }

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">控件/Obj</param>
        /// <param name="icon">图标</param>
        public static Config config(Form form, string title, object content, TType icon)
        {
            return new Config(form, title, content, icon);
        }

        #endregion

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// Model 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="title">标题</param>
            /// <param name="content">内容</param>
            public Config(Form form, string title, string content)
            {
                Form = form;
                Title = title;
                Content = content;
            }

            /// <summary>
            /// Model 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="title">标题</param>
            /// <param name="content">控件/内容</param>
            public Config(Form form, string title, object content)
            {
                Form = form;
                Title = title;
                Content = content;
            }

            /// <summary>
            /// Model 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="title">标题</param>
            /// <param name="content">内容</param>
            /// <param name="icon">图标</param>
            public Config(Form form, string title, string content, TType icon)
            {
                Form = form;
                Title = title;
                Content = content;
                Icon = icon;
            }

            /// <summary>
            /// Model 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="title">标题</param>
            /// <param name="content">控件/内容</param>
            /// <param name="icon">图标</param>
            public Config(Form form, string title, object content, TType icon)
            {
                Form = form;
                Title = title;
                Content = content;
                Icon = icon;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 控件/内容
            /// </summary>
            public object Content { get; set; }

            /// <summary>
            /// 消息框宽度
            /// </summary>
            public int Width { get; set; } = 416;

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 是否支持键盘 esc 关闭
            /// </summary>
            public bool Keyboard { get; set; } = true;

            /// <summary>
            /// 是否展示遮罩
            /// </summary>
            public bool Mask { get; set; } = true;

            /// <summary>
            /// 点击蒙层是否允许关闭
            /// </summary>
            public bool MaskClosable { get; set; } = true;

            /// <summary>
            /// 是否显示关闭图标
            /// </summary>
            public bool CloseIcon { get; set; } = false;

            /// <summary>
            /// 取消按钮文字
            /// </summary>
            public string? CancelText { get; set; } = "取消";

            /// <summary>
            /// 确认按钮文字
            /// </summary>
            public string OkText { get; set; } = "确定";

            /// <summary>
            /// 确认按钮类型
            /// </summary>
            public TTypeMini OkType { get; set; } = TTypeMini.Primary;

            /// <summary>
            /// 图标
            /// </summary>
            public TType Icon { get; set; } = TType.None;

            /// <summary>
            /// 确定回调
            /// </summary>
            public Func<Config, bool>? OnOk { get; set; }

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            #region 自定义按钮

            /// <summary>
            /// 自定义按钮
            /// </summary>
            public Btn[]? Btns { get; set; }

            /// <summary>
            /// 自定义按钮回调
            /// </summary>
            public Action<Button>? OnBtns { get; set; }

            #endregion
        }

        /// <summary>
        /// 按钮
        /// </summary>
        public class Btn
        {
            /// <summary>
            /// 自定义按钮
            /// </summary>
            /// <param name="name">按钮名称</param>
            /// <param name="text">按钮文字</param>
            /// <param name="type">按钮类型</param>
            public Btn(string name, string text, TTypeMini type = TTypeMini.Default)
            {
                Name = name;
                Text = text;
                Type = type;
            }

            /// <summary>
            /// 自定义按钮
            /// </summary>
            /// <param name="name">按钮名称</param>
            /// <param name="text">按钮文字</param>
            /// <param name="fore">字体颜色</param>
            /// <param name="back">背景颜色</param>
            /// <param name="type">按钮类型</param>
            public Btn(string name, string text, Color fore, Color back, TTypeMini type = TTypeMini.Default)
            {
                Name = name;
                Text = text;
                Fore = fore;
                Back = back;
                Type = type;
            }

            /// <summary>
            /// 按钮名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 按钮文字
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 按钮类型
            /// </summary>
            public TTypeMini Type { get; set; }

            /// <summary>
            /// 文字颜色
            /// </summary>
            public Color? Fore { get; set; }

            /// <summary>
            /// 背景颜色
            /// </summary>
            public Color? Back { get; set; }

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }
        }

        /// <summary>
        /// 多行文本
        /// </summary>
        [Obsolete("使用 TextRow 来替代")]
        public class TextLine
        {
            public TextLine(string text)
            {
                Text = text;
            }
            public TextLine(string text, int gap)
            {
                Text = text;
                Gap = gap;
            }
            public TextLine(string text, int gap, Color fore)
            {
                Text = text;
                Gap = gap;
                Fore = fore;
            }
            public TextLine(string text, Color fore)
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
        }

        #region 丰富文本框

        public class TextRow : ITextRow
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
        }
        public class TextRows : ITextRow
        {
            public TextRows(TextCell[] text)
            {
                Text = text;
            }
            public TextRows(TextCell[] text, Color fore)
            {
                Text = text;
                Fore = fore;
            }

            /// <summary>
            /// 文字s
            /// </summary>
            public TextCell[] Text { get; set; }

            /// <summary>
            /// 文字颜色
            /// </summary>
            public Color? Fore { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }
        }

        public interface ITextRow
        {
            /// <summary>
            /// 文字颜色
            /// </summary>
            Color? Fore { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            Font? Font { get; set; }
        }

        public class TextCell
        {
            public TextCell(string text)
            {
                Text = text;
            }
            public TextCell(string text, int gap)
            {
                Text = text;
                Gap = gap;
            }
            public TextCell(string text, int gap, Color fore)
            {
                Text = text;
                Gap = gap;
                Fore = fore;
            }
            public TextCell(string text, Color fore)
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
        }

        #endregion
    }
}