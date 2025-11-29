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
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Message 全局提示
    /// </summary>
    /// <remarks>全局展示操作反馈信息。</remarks>
    public static class Message
    {
        /// <summary>
        /// 最大显示数量
        /// </summary>
        public static int? MaxCount { get; set; }

        #region 目标

        /// <summary>
        /// 成功提示
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void success(Target target, string text, Font? font = null, int? autoClose = null) => open(new Config(target, text, TType.Success, font, autoClose));

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void info(Target target, string text, Font? font = null, int? autoClose = null) => open(new Config(target, text, TType.Info, font, autoClose));

        /// <summary>
        /// 警告提示
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void warn(Target target, string text, Font? font = null, int? autoClose = null) => open(new Config(target, text, TType.Warn, font, autoClose));

        /// <summary>
        /// 失败提示
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void error(Target target, string text, Font? font = null, int? autoClose = null) => open(new Config(target, text, TType.Error, font, autoClose));

        /// <summary>
        /// 加载提示
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">提示内容</param>
        /// <param name="call">耗时任务</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void loading(Target target, string text, Action<Config> call, Font? font = null, int? autoClose = null) => open(new Config(target, text, TType.None, font, autoClose).SetCall(call));

        /// <summary>
        /// 加载提示
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="text">提示内容</param>
        /// <param name="call">耗时任务</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void loading(Target target, string text, Func<Config, Task> call, Font? font = null, int? autoClose = null) => open(new Config(target, text, TType.None, font, autoClose).SetCall(call));

        public static void open(Target target, string text, Font? font = null, int? autoClose = null) => open(new Config(target, text, TType.None, font, autoClose));

        #endregion

        #region 窗口

        /// <summary>
        /// 成功提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void success(Form form, string text, Font? font = null, int? autoClose = null) => open(new Config(form, text, TType.Success, font, autoClose));

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void info(Form form, string text, Font? font = null, int? autoClose = null) => open(new Config(form, text, TType.Info, font, autoClose));

        /// <summary>
        /// 警告提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void warn(Form form, string text, Font? font = null, int? autoClose = null) => open(new Config(form, text, TType.Warn, font, autoClose));

        /// <summary>
        /// 失败提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void error(Form form, string text, Font? font = null, int? autoClose = null) => open(new Config(form, text, TType.Error, font, autoClose));

        /// <summary>
        /// 加载提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="call">耗时任务</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void loading(Form form, string text, Action<Config> call, Font? font = null, int? autoClose = null) => open(new Config(form, text, TType.None, font, autoClose).SetCall(call));

        /// <summary>
        /// 加载提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="call">耗时任务</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void loading(Form form, string text, Func<Config, Task> call, Font? font = null, int? autoClose = null) => open(new Config(form, text, TType.None, font, autoClose).SetCall(call));

        public static void open(Form form, string text, Font? font = null, int? autoClose = null) => open(new Config(form, text, TType.None, font, autoClose));

        #endregion

        /// <summary>
        /// Message 全局提示
        /// </summary>
        /// <param name="config">配置</param>
        public static void open(this Config config) => MsgQueue.Add(config);

        /// <summary>
        /// 关闭全部
        /// </summary>
        public static void close_all()
        {
            var close_list = new System.Collections.Generic.List<MessageFrm>(10);
            foreach (var it in ILayeredFormAnimate.list)
            {
                foreach (var item in it.Value)
                {
                    if (item is MessageFrm message) close_list.Add(message);
                }
            }
            if (close_list.Count == 0) return;
            foreach (var it in close_list) it.CloseMe();
        }

        /// <summary>
        /// 关闭指定id
        /// </summary>
        public static void close_id(string id)
        {
            if (ILayeredFormAnimate.list.Count > 0 || MsgQueue.queue.Count > 0)
            {
                bool isadd = true;
                var close_list = new System.Collections.Generic.List<MessageFrm>();
                foreach (var it in ILayeredFormAnimate.list)
                {
                    foreach (var item in it.Value)
                    {
                        if (item is MessageFrm message && message.config.ID == id)
                        {
                            close_list.Add(message);
                            isadd = false;
                        }
                    }
                }
                if (isadd) MsgQueue.volley.Add("M" + id);
                if (close_list.Count == 0) return;
                foreach (var it in close_list) it.CloseMe();
            }
        }

        /// <summary>
        /// 判断提示ID是否存在队列
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="time_expand">是否延长时间</param>
        public static bool contains(string id, bool time_expand = false) => MsgQueue.contains("M" + id, time_expand);

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            public Config(Target target)
            {
                Target = target;
            }
            public Config(Target target, string text)
            {
                Target = target;
                Text = text;
            }
            public Config(Target target, string text, TType icon)
            {
                Target = target;
                Text = text;
                Icon = icon;
            }
            public Config(Target target, string text, Font? font)
            {
                Target = target;
                Font = font;
                Text = text;
            }
            public Config(Target target, string text, TType icon, Font? font)
            {
                Target = target;
                Font = font;
                Text = text;
                Icon = icon;
            }
            public Config(Target target, string text, TType icon, Font? font, int? autoClose)
            {
                Target = target;
                Font = font;
                Text = text;
                Icon = icon;
                if (autoClose.HasValue) AutoClose = autoClose.Value;
            }

            #region 窗口

            public Config(Form form) : this(new Target(form)) { }
            public Config(Form form, string text) : this(new Target(form), text) { }
            public Config(Form form, string text, TType icon) : this(new Target(form), text, icon) { }
            public Config(Form form, string text, Font? font) : this(new Target(form), text, font) { }
            public Config(Form form, string text, TType icon, Font? font) : this(new Target(form), text, icon, font) { }
            public Config(Form form, string text, TType icon, Font? font, int? autoClose) : this(new Target(form), text, icon, font, autoClose) { }

            #endregion

            /// <summary>
            /// ID
            /// </summary>
            public string? ID { get; set; }

            /// <summary>
            /// 所属目标
            /// </summary>
            public Target Target { get; set; }

            /// <summary>
            /// 所属窗口
            /// </summary>
            [Obsolete("use Target")]
            public Control Form => Target.GetForm!;

            string? text;
            /// <summary>
            /// 文本
            /// </summary>
            public string? Text
            {
                get => Localization.GetLangI(LocalizationText, text, new string?[] { "{id}", ID });
                set => text = value;
            }

            /// <summary>
            /// 国际化（文本）
            /// </summary>
            public string? LocalizationText { get; set; }

            /// <summary>
            /// 图标
            /// </summary>
            public TType Icon { get; set; }

            public IconInfo? IconCustom { get; set; }

            /// <summary>
            /// 加载回调
            /// </summary>
            public object? Call { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            /// <summary>
            /// 自动关闭时间（秒）0等于不关闭
            /// </summary>
            public int AutoClose { get; set; } = 6;

            /// <summary>
            /// 是否可以点击关闭
            /// </summary>
            public bool ClickClose { get; set; } = true;

            /// <summary>
            /// 是否置顶
            /// </summary>
            public bool TopMost { get; set; }

            /// <summary>
            /// 方向
            /// </summary>
            public TAlignFrom Align { get; set; } = TAlignFrom.Top;

            /// <summary>
            /// 边距
            /// </summary>
            public Size Padding { get; set; } = new Size(12, 9);

            /// <summary>
            /// 弹出在窗口
            /// </summary>
            public bool? ShowInWindow { get; set; }

            /// <summary>
            /// 是否启用声音
            /// </summary>
            public bool EnableSound { get; set; }

            public void OK(string text)
            {
                Icon = TType.Success;
                Text = text;
                Refresh();
            }
            public void Error(string text)
            {
                Icon = TType.Error;
                Text = text;
                Refresh();
            }
            public void Warn(string text)
            {
                Icon = TType.Warn;
                Text = text;
                Refresh();
            }
            public void Info(string text)
            {
                Icon = TType.Info;
                Text = text;
                Refresh();
            }

            internal Action? refresh;
            public void Refresh() => refresh?.Invoke();

            #region 设置

            public Config SetID(string? value)
            {
                ID = value;
                return this;
            }

            public Config SetText(string? value, string? localization = null)
            {
                text = value;
                LocalizationText = localization;
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

            public Config SetCall(Action? value)
            {
                Call = value;
                return this;
            }
            public Config SetCall(Action<Config>? value)
            {
                Call = value;
                return this;
            }
            public Config SetCall(Func<Config, Task>? value)
            {
                Call = value;
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

            public Config SetAutoClose(int value = 0)
            {
                AutoClose = value;
                return this;
            }

            public Config SetClickClose(bool value = false)
            {
                ClickClose = value;
                return this;
            }

            public Config SetTopMost(bool value = true)
            {
                TopMost = value;
                return this;
            }

            public Config SetAlign(TAlignFrom value = TAlignFrom.Bottom)
            {
                Align = value;
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

            public Config SetShowInWindow(bool? value = true)
            {
                ShowInWindow = value;
                return this;
            }

            public Config SetEnableSound(bool value = true)
            {
                EnableSound = value;
                return this;
            }

            #endregion
        }
    }

    public class IconInfo
    {
        public IconInfo(string svg) { Svg = svg; }
        public IconInfo(string svg, Color? fill)
        {
            Svg = svg;
            Fill = fill;
        }

        /// <summary>
        /// SVG
        /// </summary>
        public string Svg { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color? Fill { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back { get; set; }

        /// <summary>
        /// 背景圆角
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// 背景是否圆角
        /// </summary>
        public bool Round { get; set; }

        /// <summary>
        /// 与背景偏移量
        /// </summary>
        public int Offset { get; set; } = 1;

        #region 设置

        public IconInfo SetFill(Color? value)
        {
            Fill = value;
            return this;
        }
        public IconInfo SetBack(Color? value)
        {
            Back = value;
            return this;
        }
        public IconInfo SetRadius(float value)
        {
            Radius = value;
            return this;
        }
        public IconInfo SetRound(bool value = true)
        {
            Round = value;
            return this;
        }
        public IconInfo SetOffset(int value = 0)
        {
            Offset = value;
            return this;
        }

        #endregion
    }

    internal class MessageFrm : ILayeredFormAnimate
    {
        internal Message.Config config;
        int shadow_size = 10;
        public MessageFrm(Message.Config _config, string? id)
        {
            config = _config;
            Tag = id;
            if (config.TopMost) Helper.SetTopMost(Handle);
            else config.Target.SetTopMost(Handle);
            shadow_size = (int)(shadow_size * Config.Dpi);
            loading = _config.Call != null;
            config.Target.SetFontConfig(config.Font, this);
            config.Target.SetIcon(this);
            Helper.GDI(g => SetSize(RenderMeasure(g, shadow_size)));
        }

        public override string name => nameof(Message);
        internal override TAlignFrom Align => config.Align;
        internal override bool ActiveAnimation => false;

        bool loading = false, loadingend = true;
        int AnimationLoadingValue = -1;
        AnimationTask? ThreadLoading;
        public bool IInit()
        {
            if (SetPosition(config.Target, config.ShowInWindow ?? Config.ShowInWindowByMessage)) return true;
            loadingend = false;
            ITask.Run(() =>
            {
                if (config.Call != null)
                {
                    config.refresh += () =>
                    {
                        if (IRefresh())
                        {
                            loadingend = true;
                            return;
                        }
                    };
                    var tmp = DateTime.Now;
                    try
                    {
                        if (config.Call is Func<Message.Config, Task> func) func(config).Wait();
                        else if (config.Call is Action<Message.Config> action_config) action_config(config);
                        else if (config.Call is Action action) action();
                    }
                    catch { }
                    var time = DateTime.Now - tmp;
                    if (time.TotalSeconds < 1) Thread.Sleep(1000 - (int)(time.TotalMilliseconds));
                    loading = false;
                    ThreadLoading?.Dispose();
                    if (IRefresh())
                    {
                        loadingend = true;
                        return;
                    }
                }
            }, () =>
            {
                loadingend = true;
                if (config.AutoClose > 0)
                {
                    Sleep(config.AutoClose);
                    CloseMe();
                }
            });
            // 播放声音
            if (config.EnableSound)
            {
                MessageType soundType = config.Icon switch
                {
                    TType.Success => MessageType.Information,
                    TType.Info => MessageType.Information,
                    TType.Warn => MessageType.Warning,
                    TType.Error => MessageType.Error,
                    _ => MessageType.Information
                };
                SystemSoundHelper.PlaySound(soundType);
            }
            PlayAnimation();
            return false;
        }

        public override void RunLoading()
        {
            if (loading)
            {
                if (loadingend) return;
                AnimationLoadingValue = 0;
                ThreadLoading = new AnimationTask(new AnimationLinearConfig(this, i =>
                {
                    if (loading)
                    {
                        AnimationLoadingValue = i;
                        Print();
                        return true;
                    }
                    return false;
                }, 20, 360, 10));
            }
        }

        bool IRefresh()
        {
            var oldw = TargetRect.Width;
            if (IsHandleCreated)
            {
                DisposeAnimation();
                Helper.GDI(g => SetSize(RenderMeasure(g, shadow_size)));
                SetPositionCenter(oldw);
                return false;
            }
            else return true;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (loadingend && config.ClickClose) CloseMe();
            base.OnMouseClick(e);
        }

        #region 渲染

        readonly FormatFlags s_f_left = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;
        public override Bitmap? PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = rect.PaddingRect(Padding, shadow_size);
            Bitmap rbmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(rbmp).High())
            {
                using (var path = DrawShadow(g, rect, rect_read))
                {
                    g.Fill(Colour.BgElevated.Get(nameof(Message)), path);
                }
                if (loading)
                {
                    var bor3 = 3F * Config.Dpi;
                    g.DrawEllipse(Colour.Fill.Get(nameof(Message)), bor3, rect_loading);
                    if (AnimationLoadingValue > -1)
                    {
                        using (var pen = new Pen(Colour.Primary.Get(nameof(Message)), bor3))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            g.DrawArc(pen, rect_loading, AnimationLoadingValue, 100);
                        }
                    }
                }
                else if (config.IconCustom != null) g.PaintIcons(config.IconCustom, rect_icon);
                else if (config.Icon != TType.None) g.PaintIcons(config.Icon, rect_icon, "Message", TAMode.Auto);
                using (var brush = new SolidBrush(Colour.TextBase.Get(nameof(Message))))
                {
                    g.DrawText(config.Text, Font, brush, rect_txt, s_f_left);
                }
            }
            return rbmp;
        }

        SafeBitmap? shadow_temp;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="rect_read">真实区域</param>
        GraphicsPath DrawShadow(Canvas g, Rectangle rect_client, Rectangle rect_read)
        {
            var path = rect_read.RoundPath((int)(config.Radius * Config.Dpi));
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
                {
                    shadow_temp?.Dispose();
                    shadow_temp = path.PaintShadow(rect_client.Width, rect_client.Height);
                }
                g.Image(shadow_temp.Bitmap, rect_client, .2F);
            }
            return path;
        }

        Rectangle rect_icon, rect_loading, rect_txt;
        Size RenderMeasure(Canvas g, int shadow)
        {
            int shadow2 = shadow * 2;
            float dpi = Config.Dpi;
            var size = g.MeasureText(config.Text, Font, 10000, s_f_left);
            int paddingx = (int)(config.Padding.Width * dpi), paddingy = (int)(config.Padding.Height * dpi), sp = (int)(8 * dpi), height = size.Height + paddingy * 2;
            if (loading)
            {
                int icon_size = (int)(g.MeasureText(Config.NullText, Font).Height * .86F);
                rect_icon = new Rectangle(shadow + paddingx, shadow + (height - icon_size) / 2, icon_size, icon_size);
                rect_txt = new Rectangle(rect_icon.Right + sp, shadow, size.Width, height);

                int loading_size = (int)(icon_size * .86F);
                rect_loading = new Rectangle(rect_icon.X + (rect_icon.Width - loading_size) / 2, rect_icon.Y + (rect_icon.Height - loading_size) / 2, loading_size, loading_size);

                return new Size(size.Width + icon_size + sp + paddingx * 2 + shadow2, height + shadow2);
            }
            else if (config.Icon == TType.None && config.IconCustom == null)
            {
                rect_txt = new Rectangle(shadow + paddingx, shadow, size.Width, height);
                return new Size(size.Width + paddingx * 2 + shadow2, height + shadow2);
            }
            else
            {
                int icon_size = (int)(g.MeasureText(Config.NullText, Font).Height * .86F);
                rect_icon = new Rectangle(shadow + paddingx, shadow + (height - icon_size) / 2, icon_size, icon_size);
                rect_txt = new Rectangle(rect_icon.Right + sp, shadow, size.Width, height);
                return new Size(size.Width + icon_size + sp + paddingx * 2 + shadow2, height + shadow2);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ThreadLoading?.Dispose();
            shadow_temp?.Dispose();
            shadow_temp = null;
            base.Dispose(disposing);
        }
    }
}