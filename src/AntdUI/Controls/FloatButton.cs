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
    /// FloatButton 悬浮按钮
    /// </summary>
    /// <remarks>悬浮按钮。</remarks>
    public static class FloatButton
    {
        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Form? open(Form form, ConfigBtn[] btns, Action<ConfigBtn> call)
        {
            return open(new Config(form, btns, call));
        }

        /// <summary>
        /// FloatButton 悬浮按钮
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Form? open(Form form, Control content, ConfigBtn[] btns, Action<ConfigBtn> call)
        {
            return open(new Config(form, btns, call)
            {
                Control = content
            });
        }

        /// <summary>
        /// FloatButton 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Config config(Form form, ConfigBtn[] btns, Action<ConfigBtn> call)
        {
            return new Config(form, btns, call);
        }

        /// <summary>
        /// FloatButton 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="content">所属控件</param>
        /// <param name="btns">按钮</param>
        /// <param name="call">回调</param>
        public static Config config(Form form, Control content, ConfigBtn[] btns, Action<ConfigBtn> call)
        {
            return new Config(form, btns, call)
            {
                Control = content
            };
        }

        /// <summary>
        /// FloatButton 悬浮按钮
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
                var floatButton = new LayeredFormFloatButton(config);
                floatButton.Show(config.Form);
                return floatButton;
            }
            return null;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 配置
            /// </summary>
            /// <param name="form">所属窗口</param>
            /// <param name="btns">按钮</param>
            /// <param name="call">回调</param>
            public Config(Form form, ConfigBtn[] btns, Action<ConfigBtn> call)
            {
                Form = form;
                Btns = btns;
                Call = call;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 所属控件
            /// </summary>
            public Control? Control { get; set; }

            /// <summary>
            /// 方向
            /// </summary>
            public TAlign Align { get; set; } = TAlign.BR;

            /// <summary>
            /// 是否垂直方向
            /// </summary>
            public bool Vertical { get; set; } = true;

            /// <summary>
            /// 是否置顶
            /// </summary>
            public bool TopMost { get; set; }

            /// <summary>
            /// 大小
            /// </summary>
            public int Size { get; set; } = 40;

            /// <summary>
            /// 边距X
            /// </summary>
            public int MarginX { get; set; } = 24;

            /// <summary>
            /// 边距Y
            /// </summary>
            public int MarginY { get; set; } = 24;

            /// <summary>
            /// 按钮列表
            /// </summary>
            public ConfigBtn[] Btns { get; set; }

            /// <summary>
            /// 点击回调
            /// </summary>
            public Action<ConfigBtn> Call { get; set; }
        }

        /// <summary>
        /// 配置 按钮
        /// </summary>
        public class ConfigBtn : BadgeConfig
        {
            /// <summary>
            /// 配置 按钮
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="icon">图标</param>
            public ConfigBtn(string name, Bitmap icon)
            {
                Name = name;
                Icon = icon;
            }

            /// <summary>
            /// 配置 按钮
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="text">标题/SVG</param>
            /// <param name="isSVG">是否SVG</param>
            public ConfigBtn(string name, string text, bool isSVG = false)
            {
                Name = name;
                if (isSVG) IconSvg = text;
                else Text = text;
            }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 自定义图标
            /// </summary>
            public Bitmap? Icon { get; set; }

            /// <summary>
            /// 自定义图标SVG
            /// </summary>
            public string? IconSvg { get; set; }

            /// <summary>
            /// 文字及其它内容
            /// </summary>
            public string? Text { get; set; }

            /// <summary>
            /// 气泡的内容
            /// </summary>
            public string? Tooltip { get; set; }

            /// <summary>
            /// 设置按钮类型
            /// </summary>
            public TTypeMini Type { get; set; } = TTypeMini.Default;

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            /// <summary>
            /// 设置按钮形状
            /// </summary>
            public TShape Shape { get; set; } = TShape.Circle;

            /// <summary>
            /// 徽标文本
            /// </summary>
            public string? Badge { get; set; }

            /// <summary>
            /// 徽标大小
            /// </summary>
            public float BadgeSize { get; set; } = 9F;

            /// <summary>
            /// 徽标背景颜色
            /// </summary>
            public Color? BadgeBack { get; set; }

            #region 内部

            internal bool hover = false;
            internal Rectangle rect;
            internal Rectangle rect_read;
            internal RectangleF rect_icon;
            internal Bitmap? shadow_temp = null;

            #endregion
        }
    }
}