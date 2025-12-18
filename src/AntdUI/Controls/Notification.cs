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
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Notification 通知提醒框
    /// </summary>
    /// <remarks>全局展示通知提醒信息。</remarks>
    public static class Notification
    {
        /// <summary>
        /// 最大显示数量
        /// </summary>
        public static int? MaxCount { get; set; }

        /// <summary>
        /// 成功通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void success(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null) => open(new Config(form, title, text, TType.Success, align, font, autoClose));

        /// <summary>
        /// 信息通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void info(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null) => open(new Config(form, title, text, TType.Info, align, font, autoClose));

        /// <summary>
        /// 警告通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void warn(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null) => open(new Config(form, title, text, TType.Warn, align, font, autoClose));

        /// <summary>
        /// 失败通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void error(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null) => open(new Config(form, title, text, TType.Error, align, font, autoClose));

        /// <summary>
        /// 普通通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void open(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null) => open(new Config(form, title, text, TType.None, align, font, autoClose));

        /// <summary>
        /// Notification 通知提醒框
        /// </summary>
        /// <param name="config">配置</param>
        public static void open(this Config config) => MsgQueue.Add(config);

        /// <summary>
        /// 关闭全部
        /// </summary>
        public static void close_all()
        {
            var close_list = new System.Collections.Generic.List<NotificationFrm>(10);
            foreach (var it in ILayeredFormAnimate.list)
            {
                foreach (var item in it.Value)
                {
                    if (item is NotificationFrm notification) close_list.Add(notification);
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
                var close_list = new System.Collections.Generic.List<NotificationFrm>();
                foreach (var it in ILayeredFormAnimate.list)
                {
                    foreach (var item in it.Value)
                    {
                        if (item is NotificationFrm notification && notification.config.ID == id)
                        {
                            close_list.Add(notification);
                            isadd = false;
                        }
                    }
                }
                if (isadd) MsgQueue.volley.Add("N" + id);
                if (close_list.Count == 0) return;
                foreach (var it in close_list) it.CloseMe();
            }
        }

        /// <summary>
        /// 判断通知ID是否存在队列
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="time_expand">是否延长时间</param>
        public static bool contains(string id, bool time_expand = false) => MsgQueue.contains("N" + id, time_expand);

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            public Config(Target target)
            {
                Target = target;
            }
            public Config(Target target, TType icon, TAlignFrom _align)
            {
                Target = target;
                Align = _align;
                Icon = icon;
            }
            public Config(Target target, string title, string text, TType icon, TAlignFrom align)
            {
                Target = target;
                Title = title;
                Text = text;
                Align = align;
                Icon = icon;
            }
            public Config(Target target, string title, string text, TType icon, TAlignFrom align, Font? font)
            {
                Target = target;
                Font = font;
                Title = title;
                Text = text;
                Align = align;
                Icon = icon;
            }
            public Config(Target target, string title, string text, TType icon, TAlignFrom align, Font? font, int? autoClose)
            {
                Target = target;
                Font = font;
                Title = title;
                Text = text;
                Align = align;
                Icon = icon;
                if (autoClose.HasValue) AutoClose = autoClose.Value;
            }

            #region 窗口

            public Config(Form form) : this(new Target(form)) { }
            public Config(Form form, TType icon, TAlignFrom align) : this(new Target(form), icon, align) { }
            public Config(Form form, string title, string text, TType icon, TAlignFrom align) : this(new Target(form), title, text, icon, align) { }
            public Config(Form form, string title, string text, TType icon, TAlignFrom align, Font? font) : this(new Target(form), title, text, icon, align, font) { }
            public Config(Form form, string title, string text, TType icon, TAlignFrom align, Font? font, int? autoClose) : this(new Target(form), title, text, icon, align, font, autoClose) { }

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

            string? title;
            /// <summary>
            /// 标题
            /// </summary>
            public string? Title
            {
                get => Localization.GetLangI(LocalizationTitle, title, new string?[] { "{id}", ID });
                set => title = value;
            }

            /// <summary>
            /// 国际化（标题）
            /// </summary>
            public string? LocalizationTitle { get; set; }

            /// <summary>
            /// 标题字体
            /// </summary>
            public Font? FontTitle { get; set; }

            /// <summary>
            /// 标题字体样式
            /// </summary>
            public FontStyle? FontStyleTitle { get; set; }

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
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 方向
            /// </summary>
            public TAlignFrom Align { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 10;

            /// <summary>
            /// 自动关闭时间（秒）0等于不关闭
            /// </summary>
            public int AutoClose { get; set; } = 6;

            /// <summary>
            /// 是否可以点击关闭
            /// </summary>
            public bool ClickClose { get; set; } = true;

            /// <summary>
            /// 是否显示关闭图标
            /// </summary>
            public bool CloseIcon { get; set; } = true;

            /// <summary>
            /// 是否置顶
            /// </summary>
            public bool TopMost { get; set; }

            /// <summary>
            /// 超链接回调
            /// </summary>
            public ConfigLink? Link { get; set; }

            /// <summary>
            /// 关闭回调
            /// </summary>
            public Action? OnClose { get; set; }

            /// <summary>
            /// 边距
            /// </summary>
            public Size Padding { get; set; } = new Size(24, 20);

            /// <summary>
            /// 弹出在窗口
            /// </summary>
            public bool? ShowInWindow { get; set; }

            /// <summary>
            /// 是否启用声音
            /// </summary>
            public bool EnableSound { get; set; }

            #region 设置

            public Config SetID(string? value)
            {
                ID = value;
                return this;
            }

            public Config SetTitle(string? value, string? localization = null)
            {
                title = value;
                LocalizationTitle = localization;
                return this;
            }
            public Config SetTitle(Font? value)
            {
                FontTitle = value;
                return this;
            }
            public Config SetTitle(FontStyle? value)
            {
                FontStyleTitle = value;
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

            public Config SetCloseIcon(bool value = false)
            {
                CloseIcon = value;
                return this;
            }

            public Config SetTopMost(bool value = true)
            {
                TopMost = value;
                return this;
            }

            public Config SetAlign(TAlignFrom value = TAlignFrom.TR)
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

            public Config SetOnClose(Action? value)
            {
                OnClose = value;
                return this;
            }

            public Config SetEnableSound(bool value = true)
            {
                EnableSound = value;
                return this;
            }

            public Config SetLink(string text, Func<bool> call)
            {
                Link = new ConfigLink(text, call);
                return this;
            }

            public Config SetLink(string text, object tag, Func<bool> call)
            {
                Link = new ConfigLink(text, call).SetTag(tag);
                return this;
            }

            #endregion
        }

        public class ConfigLink
        {
            public ConfigLink(string text, Func<bool> call)
            {
                Text = text;
                Call = call;
            }

            /// <summary>
            /// 连接文本
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// 点击回调
            /// </summary>
            public Func<bool> Call { get; set; }

            /// <summary>
            /// 用户定义数据
            /// </summary>
            public object? Tag { get; set; }

            public ConfigLink SetTag(object? value)
            {
                Tag = value;
                return this;
            }
        }
    }

    internal class NotificationFrm : ILayeredFormAnimate
    {
        Font font_title;
        internal Notification.Config config;
        int shadow_size = 10;
        public NotificationFrm(Notification.Config _config, string? id)
        {
            config = _config;
            Tag = id;
            if (config.TopMost) Helper.SetTopMost(Handle);
            else config.Target.SetTopMost(Handle);
            shadow_size = (int)(shadow_size * Dpi);
            config.Target.SetFontConfig(config.Font, this);
            config.Target.SetIcon(this);
            font_title = config.FontTitle ?? new Font(Font.FontFamily, Font.Size * 1.14F, config.FontStyleTitle ?? Font.Style);
            Helper.GDI(g => SetSize(RenderMeasure(g, shadow_size)));
            close_button = new ITaskOpacity(name, this);
        }

        protected override void Dispose(bool disposing)
        {
            config.OnClose?.Invoke();
            config.OnClose = null;
            close_button.Dispose();
            shadow_temp?.Dispose();
            shadow_temp = null;
            base.Dispose(disposing);
        }

        public override string name => nameof(Notification);
        internal override TAlignFrom Align => config.Align;
        internal override bool ActiveAnimation => false;

        public bool IInit()
        {
            if (SetPosition(config.Target, config.ShowInWindow ?? Config.ShowInWindowByNotification)) return true;
            if (config.AutoClose > 0)
            {
                ITask.Run(() =>
                {
                    Sleep(config.AutoClose);
                    CloseMe();
                });
            }
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

        #region 渲染

        readonly FormatFlags s_f = FormatFlags.Center | FormatFlags.NoWrapEllipsis, s_f_left = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis, s_f_left_left = FormatFlags.Left | FormatFlags.Top;

        public override Bitmap? PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = rect.PaddingRect(Padding, shadow_size);
            Bitmap rbmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(rbmp).High())
            {
                using (var path = DrawShadow(g, rect, rect_read))
                {
                    g.Fill(Colour.BgElevated.Get(nameof(Notification)), path);
                }
                if (config.IconCustom != null) g.PaintIcons(config.IconCustom, rect_icon);
                else if (config.Icon != TType.None) g.PaintIcons(config.Icon, rect_icon, "Notification", TAMode.Auto);

                if (config.CloseIcon)
                {
                    if (close_button.Animation)
                    {
                        using (var path = rect_close.RoundPath((int)(4 * Dpi)))
                        {
                            g.Fill(Helper.ToColor(close_button.Value, Colour.FillSecondary.Get(nameof(Notification))), path);
                        }
                        g.PaintIconClose(rect_close, Colour.Text.Get(nameof(Notification)), .6F);
                    }
                    else if (close_button.Switch)
                    {
                        using (var path = rect_close.RoundPath((int)(4 * Dpi)))
                        {
                            g.Fill(Colour.FillSecondary.Get(nameof(Notification)), path);
                        }
                        g.PaintIconClose(rect_close, Colour.Text.Get(nameof(Notification)), .6F);
                    }
                    else g.PaintIconClose(rect_close, Colour.TextTertiary.Get(nameof(Notification)), .6F);
                }
                using (var brush = new SolidBrush(Colour.TextBase.Get(nameof(Notification))))
                {
                    g.DrawText(config.Title, font_title, brush, rect_title, s_f_left);
                    g.DrawText(config.Text, Font, brush, rect_txt, s_f_left_left);
                }
                if (config.Link != null)
                {
                    using (var pen = new Pen(Colour.Primary.Get(nameof(Notification)), Dpi))
                    {
                        g.DrawText(config.Link.Text, Font, Colour.Primary.Get(nameof(Notification)), rect_link_text, s_f);
                        g.DrawLines(pen, TAlignMini.Right.TriangleLines(rect_links));
                    }
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
            var path = rect_read.RoundPath((int)(config.Radius * Dpi));
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
                {
                    shadow_temp?.Dispose();
                    shadow_temp = path.PaintShadow(rect_client.Width, rect_client.Height);
                }
                g.Image(shadow_temp.Bitmap, rect_client, 0.2F);
            }
            return path;
        }

        Rectangle rect_icon, rect_title, rect_txt, rect_close;
        Rectangle rect_link_text, rect_links;
        Size RenderMeasure(Canvas g, int shadow)
        {
            int shadow2 = shadow * 2;
            float dpi = Dpi;

            var size_title = g.MeasureText(config.Title, font_title, 10000, s_f_left);
            int paddingx = (int)(config.Padding.Width * dpi), paddingy = (int)(config.Padding.Height * dpi), t_max_width = (int)Math.Ceiling(360 * dpi);
            int sp = (int)(8 * dpi), close_size = (int)Math.Ceiling(22F * dpi);
            if (size_title.Width > t_max_width)
            {
                t_max_width = size_title.Width;
                if (config.CloseIcon) t_max_width += close_size + sp;
            }
            var size_desc = g.MeasureText(config.Text, Font, t_max_width);
            int width_title = (config.CloseIcon ? size_title.Width + close_size + sp : size_title.Width), width_desc = size_desc.Width;
            int max_width = width_desc > width_title ? width_desc : width_title;
            if (config.Icon == TType.None && config.IconCustom == null)
            {
                rect_title = new Rectangle(shadow + paddingx, shadow + paddingy, max_width, size_title.Height);

                int h = size_title.Height;

                if (config.CloseIcon) rect_close = new Rectangle(rect_title.Right - close_size, rect_title.Y, close_size, close_size);

                rect_txt = new Rectangle(shadow + paddingx, rect_title.Bottom + sp, rect_title.Width, size_desc.Height);

                if (size_desc.Height > 0) h += size_desc.Height + sp;

                if (config.Link != null)
                {
                    var size_link = g.MeasureText(config.Link.Text, Font, 10000, s_f);
                    rect_link_text = new Rectangle(rect_title.X, rect_txt.Bottom + sp, size_link.Width, size_link.Height);
                    rect_links = new Rectangle(rect_link_text.Right, rect_link_text.Y, rect_link_text.Height, rect_link_text.Height);
                    h += size_link.Height + sp;
                }
                return new Size(max_width + paddingx * 2 + shadow2, h + paddingy * 2 + shadow2);
            }
            else
            {
                int icon_size = (int)Math.Ceiling(size_title.Height * 1.14F), icon_sp = icon_size / 2;

                rect_icon = new Rectangle(shadow + paddingx, shadow + paddingy, icon_size, icon_size);
                rect_title = new Rectangle(rect_icon.X + rect_icon.Width + icon_sp, shadow + paddingy, max_width, icon_size);

                int h = icon_size;

                rect_txt = new Rectangle(rect_title.X, rect_title.Bottom + sp, rect_title.Width, size_desc.Height);
                if (config.CloseIcon) rect_close = new Rectangle(rect_title.Right - close_size, rect_title.Y, close_size, close_size);

                if (size_desc.Height > 0) h += size_desc.Height + sp;

                if (config.Link != null)
                {
                    var size_link = g.MeasureText(config.Link.Text, Font, 10000, s_f);
                    rect_link_text = new Rectangle(rect_title.X, rect_txt.Bottom + sp, size_link.Width, size_link.Height);
                    rect_links = new Rectangle(rect_link_text.Right, rect_link_text.Y, rect_link_text.Height, rect_link_text.Height);
                    h += size_link.Height + sp;
                }
                return new Size(max_width + icon_size + icon_sp + paddingx * 2 + shadow2, h + paddingy * 2 + shadow2);
            }
        }

        #endregion

        #region 鼠标

        ITaskOpacity close_button;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (config.CloseIcon)
            {
                close_button.MaxValue = Colour.FillSecondary.Get(nameof(Notification), TAMode.Auto).A;
                close_button.Switch = rect_close.Contains(e.X, e.Y);
                SetCursor(close_button.Switch);
                if (close_button.Switch)
                {
                    base.OnMouseMove(e);
                    return;
                }
            }
            if (config.Link != null) SetCursor(rect_link_text.Contains(e.X, e.Y));
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (config.Link != null && rect_link_text.Contains(e.X, e.Y))
            {
                if (!config.Link.Call()) return;
            }
            if (config.ClickClose) CloseMe();
            base.OnMouseClick(e);
        }

        #endregion
    }
}