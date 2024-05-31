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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
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
        /// 成功通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void success(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, title, text, TType.Success, align, font, autoClose));
        }

        /// <summary>
        /// 信息通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void info(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, title, text, TType.Info, align, font, autoClose));
        }

        /// <summary>
        /// 警告通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void warn(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, title, text, TType.Warn, align, font, autoClose));
        }

        /// <summary>
        /// 失败通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void error(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, title, text, TType.Error, align, font, autoClose));
        }

        /// <summary>
        /// 普通通知
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="align">位置</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void open(Form form, string title, string text, TAlignFrom align = TAlignFrom.TR, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, title, text, TType.None, align, font, autoClose));
        }

        /// <summary>
        /// Notification 通知提醒框
        /// </summary>
        /// <param name="config">配置</param>
        public static void open(this Config config)
        {
            if (config.Form.IsHandleCreated)
            {
                try
                {
                    if (config.Form.InvokeRequired)
                    {
                        config.Form.BeginInvoke(new Action(() =>
                        {
                            open(config);
                        }));
                        return;
                    }
                    if (config.TopMost) new NotificationFrm(config).Show();
                    else new NotificationFrm(config).Show(config.Form);
                }
                catch { }
            }
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            public Config(Form _form, string _title, string _text, TType _icon, TAlignFrom _align)
            {
                Form = _form;
                Title = _title;
                Text = _text;
                Align = _align;
                Icon = _icon;
            }
            public Config(Form _form, string _title, string _text, TType _icon, TAlignFrom _align, Font? _font)
            {
                Form = _form;
                Font = _font;
                Title = _title;
                Text = _text;
                Align = _align;
                Icon = _icon;
            }
            public Config(Form _form, string _title, string _text, TType _icon, TAlignFrom _align, Font? _font, int? autoClose)
            {
                Form = _form;
                Font = _font;
                Title = _title;
                Text = _text;
                Align = _align;
                Icon = _icon;
                if (autoClose.HasValue) AutoClose = autoClose.Value;
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
            /// 标题字体
            /// </summary>
            public Font? FontTitle { get; set; }

            /// <summary>
            /// 标题字体样式
            /// </summary>
            public FontStyle? FontStyleTitle { get; set; }

            /// <summary>
            /// 文本
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// 图标
            /// </summary>
            public TType Icon { get; set; }

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
        }

        public class ConfigLink
        {
            public ConfigLink(string text, Action call)
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
            public Action Call { get; set; }
        }
    }

    internal class NotificationFrm : ILayeredFormAnimate
    {
        Font font_title;
        Notification.Config config;
        public NotificationFrm(Notification.Config _config)
        {
            config = _config;
            if (config.TopMost) TopMost = true;
            else TopMost = config.Form.TopMost;
            if (config.Font != null) Font = config.Font;
            else if (Config.Font != null) Font = Config.Font;
            else Font = config.Form.Font;
            font_title = config.FontTitle ?? new Font(Font.FontFamily, Font.Size * 1.14F, config.FontStyleTitle ?? Font.Style);
            Icon = config.Form.Icon;
            Helper.GDI(g =>
            {
                SetSize(RenderMeasure(g));
            });
            close_button = new ITaskOpacity(this);
            IInit();
        }
        protected override void Dispose(bool disposing)
        {
            config.OnClose?.Invoke();
            config.OnClose = null;
            close_button.Dispose();
            base.Dispose(disposing);
        }
        internal override TAlignFrom Align => config.Align;

        public void IInit()
        {
            SetPosition(config.Form);
            if (config.AutoClose > 0)
            {
                ITask.Run(() =>
                {
                    Thread.Sleep(config.AutoClose * 1000);
                    IClose();
                });
            }
        }

        #region 渲染

        private readonly StringFormat stringFormat = Helper.SF_Ellipsis(StringAlignment.Near, StringAlignment.Near);

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = rect.PaddingRect(Padding, 10);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var path = DrawShadow(g, rect, rect_read))
                {
                    using (var brush = new SolidBrush(Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path);
                    }
                }
                if (config.Icon != TType.None)
                {
                    switch (config.Icon)
                    {
                        case TType.Success:
                            using (var brush = new SolidBrush(Style.Db.Success))
                            {
                                g.FillEllipse(brush, rect_icon);
                            }
                            g.PaintIconComplete(rect_icon, Style.Db.BgBase);
                            break;
                        case TType.Info:
                            using (var brush = new SolidBrush(Style.Db.Info))
                            {
                                g.FillEllipse(brush, rect_icon);
                            }
                            g.PaintIconInfo(rect_icon, Style.Db.BgBase);
                            break;
                        case TType.Warn:
                            using (var brush = new SolidBrush(Style.Db.Warning))
                            {
                                g.FillEllipse(brush, rect_icon);
                            }
                            g.PaintIconWarn(rect_icon, Style.Db.BgBase);
                            break;
                        case TType.Error:
                            using (var brush = new SolidBrush(Style.Db.Error))
                            {
                                g.FillEllipse(brush, rect_icon);
                            }
                            g.PaintIconError(rect_icon, Style.Db.BgBase);
                            break;
                    }
                }

                if (config.CloseIcon)
                {
                    if (close_button.Animation)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(close_button.Value, Style.Db.FillSecondary)))
                        {
                            using (var path = rect_close.RoundPath((int)(4 * Config.Dpi)))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        g.PaintIconError(rect_close, Style.Db.Text, 0.7F);
                    }
                    else if (close_button.Switch)
                    {
                        using (var brush = new SolidBrush(Style.Db.FillSecondary))
                        {
                            using (var path = rect_close.RoundPath((int)(4 * Config.Dpi)))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        g.PaintIconError(rect_close, Style.Db.Text, 0.7F);
                    }
                    else
                    {
                        g.PaintIconError(rect_close, Style.Db.TextTertiary, 0.7F);
                    }
                }
                using (var brush = new SolidBrush(Style.Db.TextBase))
                {
                    g.DrawString(config.Title, font_title, brush, rect_title, Helper.stringFormatLeft);
                    g.DrawString(config.Text, Font, brush, rect_txt, stringFormat);
                }
                if (config.Link != null)
                {
                    using (var brush = new SolidBrush(Style.Db.Primary))
                    using (var pen = new Pen(Style.Db.Primary, 1F * Config.Dpi))
                    {
                        g.DrawString(config.Link.Text, Font, brush, rect_link_text, Helper.stringFormatLeft);
                        g.DrawLines(pen, TAlignMini.Right.TriangleLines(rect_links));
                    }
                }
            }
            return original_bmp;
        }

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="rect_read">真实区域</param>
        GraphicsPath DrawShadow(Graphics g, Rectangle rect_client, RectangleF rect_read)
        {
            var path = rect_read.RoundPath((int)(config.Radius * Config.Dpi));
            if (shadow_temp == null || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
            {
                shadow_temp?.Dispose();
                shadow_temp = path.PaintShadow(rect_client.Width, rect_client.Height);
            }
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix { Matrix33 = 0.2F };
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(shadow_temp, rect_client, 0, 0, rect_client.Width, rect_client.Height, GraphicsUnit.Pixel, attributes);
            }
            return path;
        }

        Rectangle rect_icon, rect_title, rect_txt, rect_close;
        Rectangle rect_link_text, rect_links;
        Size RenderMeasure(Graphics g)
        {
            float dpi = g.DpiX / 96F;

            var size_title = g.MeasureString(config.Title, font_title);
            int px = (int)(24 * dpi), py = (int)(20 * dpi), t_max_width = (int)Math.Ceiling(360 * dpi);
            int sp = (int)(8 * dpi), close_size = (int)Math.Ceiling(22F * dpi), icon_size = (int)Math.Ceiling(size_title.Height * 1.14F);
            if (size_title.Width > t_max_width)
            {
                t_max_width = (int)Math.Ceiling(size_title.Width);
                if (config.CloseIcon) t_max_width += close_size + sp;
            }
            var size_desc = g.MeasureString(config.Text, Font, t_max_width);
            float width_title = (config.CloseIcon ? size_title.Width + close_size + sp : size_title.Width), width_desc = size_desc.Width;
            int max_width = (int)Math.Ceiling(width_desc > width_title ? width_desc : width_title);
            if (config.Icon == TType.None)
            {
                int titleH = (int)Math.Ceiling(size_title.Height), descH = (int)Math.Ceiling(size_desc.Height); ;

                rect_title = new Rectangle(px, py, max_width, titleH);

                if (config.CloseIcon) rect_close = new Rectangle(rect_title.Right - close_size, rect_title.Y, close_size, close_size);

                int desc_y = rect_title.Bottom + sp;

                rect_txt = new Rectangle(px, desc_y, rect_title.Width, descH);
                int temp_height = rect_txt.Bottom;
                if (config.Link != null)
                {
                    var size_link = g.MeasureString(config.Link.Text, Font);
                    int link_w = (int)Math.Ceiling(size_link.Width), link_h = (int)Math.Ceiling(size_link.Height);
                    rect_link_text = new Rectangle(rect_title.X, temp_height + sp, link_w, link_h);
                    rect_links = new Rectangle(rect_link_text.Right, rect_link_text.Y, rect_link_text.Height, rect_link_text.Height);
                    temp_height = rect_link_text.Bottom;
                }
                return new Size(rect_title.Right + px, temp_height + py);
            }
            else
            {
                int descH = (int)Math.Ceiling(size_desc.Height);

                rect_icon = new Rectangle(px, px, icon_size, icon_size);
                rect_title = new Rectangle(rect_icon.X + rect_icon.Width + icon_size / 2, px, max_width, icon_size);

                var desc_y = rect_title.Bottom + sp;
                rect_txt = new Rectangle(rect_title.X, desc_y, rect_title.Width, descH);
                if (config.CloseIcon) rect_close = new Rectangle(rect_title.Right - close_size, rect_title.Y, close_size, close_size);

                int temp_height = rect_txt.Bottom;
                if (config.Link != null)
                {
                    var size_link = g.MeasureString(config.Link.Text, Font);
                    int link_w = (int)Math.Ceiling(size_link.Width), link_h = (int)Math.Ceiling(size_link.Height);
                    rect_link_text = new Rectangle(rect_title.X, temp_height + sp, link_w, link_h);
                    rect_links = new Rectangle(rect_link_text.Right, rect_link_text.Y, rect_link_text.Height, rect_link_text.Height);
                    temp_height = rect_link_text.Bottom;
                }

                return new Size(rect_title.Right + px, temp_height + py);
            }
        }

        #endregion

        #region 鼠标

        ITaskOpacity close_button;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (config.CloseIcon)
            {
                close_button.MaxValue = Style.Db.FillSecondary.A;
                close_button.Switch = rect_close.Contains(e.Location);
                SetCursor(close_button.Switch);
                if (close_button.Switch)
                {
                    base.OnMouseMove(e);
                    return;
                }
            }
            if (config.Link != null) SetCursor(rect_link_text.Contains(e.Location));
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (config.Link != null && rect_link_text.Contains(e.Location)) config.Link.Call();
            IClose();
            base.OnMouseClick(e);
        }

        #endregion
    }
}