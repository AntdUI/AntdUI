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
        public static DialogResult open(Form form, string title, string content) => open(new Config(form, title, content));

        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static DialogResult open(Form form, string title, object content) => open(new Config(form, title, content));

        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="content">内容</param>
        public static DialogResult open(Form form, Control content) => open(new Config(form, content));

        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="icon">图标</param>
        public static DialogResult open(Form form, string title, string content, TType icon) => open(new Config(form, title, content, icon));

        /// <summary>
        /// Model 对话框
        /// </summary>
        /// <param name="config">配置</param>
        public static DialogResult open(this Config config)
        {
            if (config.Form == null || config.Form.WindowState == FormWindowState.Minimized || !config.Form.Visible)
            {
                config.Mask = config.MaskClosable = false;
                var dialogResultN = DialogResult.None;
                ModalCount++;
                dialogResultN = new LayeredFormModal(config).ShowDialog();
                ModalCount--;
                return dialogResultN;
            }
            if (!config.Form.IsHandleCreated) config.Mask = config.MaskClosable = false;
            if (config.Form.InvokeRequired) return ITask.Invoke(config.Form, new Func<DialogResult>(() => open(config)));
            var frm = new LayeredFormModal(config);
            ModalCount++;
            DialogResult dialogResult;
            if (config.Mask) dialogResult = frm.ShowDialog(config.Form.FormMask(frm));
            else dialogResult = frm.ShowDialog();
            ModalCount--;
            return dialogResult;
        }

        public static int ModalCount { get; private set; }

        #region 配置

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static Config config(Form form, string title, string content) => new Config(form, title, content);

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static Config config(string title, string content) => new Config(title, content);

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">控件/Obj</param>
        public static Config config(Form form, string title, object content) => new Config(form, title, content);

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">控件/Obj</param>
        public static Config config(string title, object content) => new Config(title, content);

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="icon">图标</param>
        public static Config config(Form form, string title, string content, TType icon) => new Config(form, title, content, icon);

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="icon">图标</param>
        public static Config config(string title, string content, TType icon) => new Config(title, content, icon);

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="form">所属窗口</param>
        /// <param name="title">标题</param>
        /// <param name="content">控件/Obj</param>
        /// <param name="icon">图标</param>
        public static Config config(Form form, string title, object content, TType icon) => new Config(form, title, content, icon);

        /// <summary>
        /// Model 配置
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">控件/Obj</param>
        /// <param name="icon">图标</param>
        public static Config config(string title, object content, TType icon) => new Config(title, content, icon);

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
            /// <param name="content">内容</param>
            public Config(Form form, Control content)
            {
                Form = form;
                Content = content;
                Padding = new Size(0, 0);
            }

            /// <summary>
            /// Model 配置
            /// </summary>
            /// <param name="title">标题</param>
            /// <param name="content">内容</param>
            public Config(string title, string content)
            {
                Mask = MaskClosable = false;
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
            /// <param name="title">标题</param>
            /// <param name="content">控件/内容</param>
            public Config(string title, object content)
            {
                Mask = MaskClosable = false;
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
            /// <param name="title">标题</param>
            /// <param name="content">内容</param>
            /// <param name="icon">图标</param>
            public Config(string title, string content, TType icon)
            {
                Mask = MaskClosable = false;
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
            /// Model 配置
            /// </summary>
            /// <param name="title">标题</param>
            /// <param name="content">控件/内容</param>
            /// <param name="icon">图标</param>
            public Config(string title, object content, TType icon)
            {
                Mask = MaskClosable = false;
                Title = title;
                Content = content;
                Icon = icon;
            }

            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form? Form { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string? Title { get; set; }

            /// <summary>
            /// 控件/内容
            /// </summary>
            public object Content { get; set; }

            /// <summary>
            /// 内容边距
            /// </summary>
            public Size ContentPadding { get; set; }

            /// <summary>
            /// 使用图标边距
            /// </summary>
            public bool UseIconPadding { get; set; } = true;

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
            public bool CloseIcon { get; set; }

            /// <summary>
            /// 默认是否焦点
            /// </summary>
            public bool DefaultFocus { get; set; }

            /// <summary>
            /// 取消按钮字体
            /// </summary>
            public Font? CancelFont { get; set; }

            /// <summary>
            /// 确认按钮字体
            /// </summary>
            public Font? OkFont { get; set; }

            /// <summary>
            /// 按钮栏高度
            /// </summary>
            public int BtnHeight { get; set; } = 38;

            /// <summary>
            /// 边距
            /// </summary>
            public Size Padding { get; set; } = new Size(24, 20);

            string? canceltext = Localization.Get("Cancel", "取消");
            /// <summary>
            /// 取消按钮文字
            /// </summary>
            public string? CancelText
            {
                get => canceltext;
                set
                {
                    if (canceltext == value) return;
                    canceltext = value;
                    Layered?.SetCancelText(value);
                }
            }

            string oktext = Localization.Get("OK", "确定");
            /// <summary>
            /// 确认按钮文字
            /// </summary>
            public string OkText
            {
                get => oktext;
                set
                {
                    if (oktext == value) return;
                    oktext = value;
                    Layered?.SetOkText(value);
                }
            }

            internal LayeredFormModal? Layered;

            /// <summary>
            /// 确认按钮类型
            /// </summary>
            public TTypeMini OkType { get; set; } = TTypeMini.Primary;

            /// <summary>
            /// 图标
            /// </summary>
            public TType Icon { get; set; } = TType.None;

            public IconInfo? IconCustom { get; set; }

            /// <summary>
            /// 确定回调
            /// </summary>
            public Func<Config, bool>? OnOk { get; set; }

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            /// <summary>
            /// 加载时禁用取消按钮
            /// </summary>
            public bool LoadingDisableCancel { get; set; }

            /// <summary>
            /// 拖拽窗口
            /// </summary>
            public bool Draggable { get; set; } = true;

            #region 自定义按钮

            /// <summary>
            /// 自定义按钮
            /// </summary>
            public Btn[]? Btns { get; set; }

            /// <summary>
            /// 自定义按钮回调
            /// </summary>
            public Func<Button, bool>? OnBtns { get; set; }

            #endregion

            /// <summary>
            /// 自定义按钮样式回调
            /// </summary>
            public Action<string, Button>? OnButtonStyle { get; set; }

            public void Close()
            {
                if (Layered == null) return;
                Layered.BeginInvoke(Layered.Close);
            }

            public void DialogResult(DialogResult result = System.Windows.Forms.DialogResult.OK)
            {
                if (Layered == null) return;
                Layered.BeginInvoke(() => Layered.DialogResult = result);
            }

            #region 设置

            public Config SetContentPadding(int x, int y)
            {
                ContentPadding = new Size(x, y);
                return this;
            }
            public Config SetContentPadding(int size)
            {
                ContentPadding = new Size(size, size);
                return this;
            }
            public Config SetContentPadding(Size size)
            {
                ContentPadding = size;
                return this;
            }
            public Config SetUseIconPadding(bool value = false)
            {
                UseIconPadding = value;
                return this;
            }
            public Config SetWidth(int value)
            {
                Width = value;
                return this;
            }
            public Config SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public Config SetKeyboard(bool value = false)
            {
                Keyboard = value;
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
            public Config SetCloseIcon(bool value = true)
            {
                CloseIcon = value;
                return this;
            }
            public Config SetDefaultFocus(bool value = true)
            {
                DefaultFocus = value;
                return this;
            }
            public Config SetInput(Input value)
            {
                DefaultFocus = true;
                value.KeyPress += (a, b) =>
                {
                    if (b.KeyChar == 13)
                    {
                        b.Handled = true;
                        if (string.IsNullOrEmpty(value.Text)) return;
                        DialogResult();
                    }
                };
                return this;
            }
            public Config SetBtnHeight(int value = 0)
            {
                BtnHeight = value;
                return this;
            }
            public Config SetPadding(int x, int y)
            {
                Padding = new Size(x, y);
                return this;
            }
            public Config SetPadding(int size)
            {
                Padding = new Size(size, size);
                return this;
            }
            public Config SetPadding(Size size)
            {
                Padding = size;
                return this;
            }

            #region 按钮

            public Config SetCancel(Font? font)
            {
                CancelFont = font;
                return this;
            }
            public Config SetCancel(string? value)
            {
                CancelText = value;
                return this;
            }
            public Config SetCancel(string? value, Font? font)
            {
                CancelText = value;
                CancelFont = font;
                return this;
            }

            public Config SetOk(string value, TTypeMini type)
            {
                OkText = value;
                OkType = type;
                return this;
            }
            public Config SetOk(Font? font)
            {
                OkFont = font;
                return this;
            }
            public Config SetOk(string value)
            {
                OkText = value;
                return this;
            }
            public Config SetOk(string value, Font? font)
            {
                OkText = value;
                OkFont = font;
                return this;
            }
            public Config SetOk(TTypeMini value = TTypeMini.Error)
            {
                OkType = value;
                return this;
            }
            public Config SetOk(Func<Config, bool>? value)
            {
                OnOk = value;
                return this;
            }

            #endregion

            public Config SetTag(object? value)
            {
                Tag = value;
                return this;
            }
            public Config SetLoadingDisableCancel(bool value = true)
            {
                LoadingDisableCancel = value;
                return this;
            }
            public Config SetDraggable(bool value = false)
            {
                Draggable = value;
                return this;
            }

            #region 图标

            public Config SetIcon(TType icon = TType.Success)
            {
                IconCustom = null;
                Icon = icon;
                return this;
            }

            public Config SetIcon(string svg) => SetIcon(new IconInfo(svg));
            public Config SetIcon(string svg, Color? fill) => SetIcon(new IconInfo(svg, fill));
            public Config SetIcon(string svg, Color back, bool round) => SetIcon(new IconInfo(svg) { Back = back, Round = round });
            public Config SetIcon(string svg, Color back, int radius) => SetIcon(new IconInfo(svg) { Back = back, Radius = radius });
            public Config SetIcon(string svg, Color? fill, Color back, bool round) => SetIcon(new IconInfo(svg, fill) { Back = back, Round = round });
            public Config SetIcon(string svg, Color? fill, Color back, int radius) => SetIcon(new IconInfo(svg, fill) { Back = back, Radius = radius });

            public Config SetIcon(IconInfo iconInfo)
            {
                IconCustom = iconInfo;
                return this;
            }

            #endregion

            #region 自定义按钮

            public Config SetBtns(params Btn[] value)
            {
                Btns = value;
                return this;
            }
            public Config SetBtns(Func<Button, bool>? call, params Btn[] value)
            {
                Btns = value;
                OnBtns = call;
                return this;
            }
            public Config SetBtns(Func<Button, bool>? value)
            {
                OnBtns = value;
                return this;
            }
            public Config SetButtonStyle(Action<string, Button>? value)
            {
                OnButtonStyle = value;
                return this;
            }

            #endregion

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
            /// 是否执行回调
            /// </summary>
            public DialogResult DialogResult { get; set; } = DialogResult.None;

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }
        }

        /// <summary>
        /// 多行文本
        /// </summary>
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

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }
        }
    }

    public interface ControlEvent
    {
        void LoadCompleted();
    }
}