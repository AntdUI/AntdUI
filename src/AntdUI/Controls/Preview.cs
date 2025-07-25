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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Preview 图片预览
    /// </summary>
    /// <remarks>图片预览框。</remarks>
    public static class Preview
    {
        /// <summary>
        /// Preview 图片预览
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(this Config config)
        {
            if (config.Form.IsHandleCreated)
            {
                if (config.Form.InvokeRequired) return ITask.Invoke(config.Form, new Func<Form?>(() => open(config)));
                var frm = new LayeredFormPreview(config);
                frm.Show(config.Form);
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
            /// Preview 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="bmp">图片</param>
            public Config(Form form, Image bmp) : this(form, new Image[] { bmp }) { }

            /// <summary>
            /// Preview 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="bmps">多个图片</param>
            public Config(Form form, IList<Image> bmps)
            {
                Form = form;
                Content = bmps;
                ContentCount = bmps.Count;
            }

            /// <summary>
            /// Preview 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="list">数据</param>
            /// <param name="call">回调</param>
            public Config(Form form, IList<object> list, Func<int, object, Image?> call)
            {
                Form = form;
                Content = new object[] {
                    list,
                    call
                };
                ContentCount = list.Count;
            }

            /// <summary>
            /// Preview 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="list">数据</param>
            /// <param name="call">回调</param>
            public Config(Form form, IList<object> list, Func<int, object, Action<float, string?>, Image?> call)
            {
                Form = form;
                Content = new object[] {
                    list,
                    call
                };
                ContentCount = list.Count;
            }

            /// <summary>
            /// Preview 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="list">多个图片和图片对应的文字和文字样式</param>
            public Config(Form form, IList<ImageTextContent> list)
            {
                Form = form;
                Content = list;
                ContentCount = list.Count;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; set; }

            public int SelectIndex { get; set; }

            /// <summary>
            /// 内容
            /// </summary>
            public object Content { get; set; }

            /// <summary>
            /// 内容数量
            /// </summary>
            public int ContentCount { get; set; }

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            /// <summary>
            /// SelectIndex 改变回调
            /// </summary>
            public Func<int, bool>? OnSelectIndexChanged { get; set; }

            #region 自定义按钮

            /// <summary>
            /// 自定义按钮
            /// </summary>
            public Btn[]? Btns { get; set; }

            /// <summary>
            /// 自定义按钮回调
            /// </summary>
            public Action<string, BtnEvent>? OnBtns { get; set; }

            #endregion
        }

        public class BtnEvent
        {
            public BtnEvent(int index, object? data, object? tag)
            {
                Index = index;
                Data = data;
                Tag = tag;
            }

            /// <summary>
            /// 数据序号
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// 元数据
            /// </summary>
            public object? Data { get; set; }

            /// <summary>
            /// Btn的Tag
            /// </summary>
            public object? Tag { get; set; }
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
            /// <param name="svg">图标SVG</param>
            public Btn(string name, string svg)
            {
                Name = name;
                IconSvg = svg;
            }

            /// <summary>
            /// 按钮名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 图标SVG
            /// </summary>
            public string IconSvg { get; set; }

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }
        }

        /// <summary>
        /// 图片和文本内容
        /// </summary>
        public class ImageTextContent
        {
            public ImageTextContent(Image image)
            {
                Image = image;
            }

            public ImageTextContent(Image image, string? text)
            {
                Image = image;
                Text = text;
            }

            public ImageTextContent(Image image, string? text, Color? foreColor)
            {
                Image = image;
                Text = text;
                ForeColor = foreColor;
            }

            public Image Image { get; set; }

            /// <summary>
            /// 显示文本
            /// </summary>
            public string? Text { get; set; }

            /// <summary>
            /// 文本字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 文本颜色
            /// </summary>
            public Color? ForeColor { get; set; }

            /// <summary>
            /// 文本位置
            /// </summary>
            public ContentAlignment TextAlign { get; set; } = ContentAlignment.TopCenter;
        }
    }
}