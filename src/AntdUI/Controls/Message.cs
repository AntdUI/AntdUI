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
using System.Threading;
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
        /// 成功提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void success(Form form, string text, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, text, TType.Success, font, autoClose));
        }

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void info(Form form, string text, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, text, TType.Info, font, autoClose));
        }

        /// <summary>
        /// 警告提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void warn(Form form, string text, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, text, TType.Warn, font, autoClose));
        }

        /// <summary>
        /// 失败提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void error(Form form, string text, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, text, TType.Error, font, autoClose));
        }

        /// <summary>
        /// 加载提示
        /// </summary>
        /// <param name="form">窗口</param>
        /// <param name="text">提示内容</param>
        /// <param name="call">耗时任务</param>
        /// <param name="font">字体</param>
        /// <param name="autoClose">自动关闭时间（秒）0等于不关闭</param>
        public static void loading(Form form, string text, Action<Config> call, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, text, TType.None, font, autoClose) { Call = call });
        }
        public static void open(Form form, string text, Font? font = null, int? autoClose = null)
        {
            open(new Config(form, text, TType.None, font, autoClose));
        }

        /// <summary>
        /// Message 全局提示
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
                    new MessageFrm(config).Show(config.Form);
                }
                catch { }
            }
        }

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
            if (close_list.Count > 0)
            {
                foreach (var item in close_list) item.IClose();
            }
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            public Config(Form _form, string _text, TType _icon)
            {
                Form = _form;
                Text = _text;
                Icon = _icon;
            }
            public Config(Form _form, string _text, TType _icon, Font? _font)
            {
                Form = _form;
                Font = _font;
                Text = _text;
                Icon = _icon;
            }
            public Config(Form _form, string _text, TType _icon, Font? _font, int? autoClose)
            {
                Form = _form;
                Font = _font;
                Text = _text;
                Icon = _icon;
                if (autoClose.HasValue) AutoClose = autoClose.Value;
            }
            /// <summary>
            /// 所属窗口
            /// </summary>
            public Form Form { get; set; }

            /// <summary>
            /// 文本
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// 图标
            /// </summary>
            public TType Icon { get; set; }

            /// <summary>
            /// 加载回调
            /// </summary>
            public Action<Config>? Call { get; set; }

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
            /// 方向
            /// </summary>
            public TAlignFrom Align { get; set; } = TAlignFrom.Top;

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
            public void Refresh()
            {
                refresh?.Invoke();
            }
        }
    }

    internal class MessageFrm : ILayeredFormAnimate
    {
        Message.Config config;
        public MessageFrm(Message.Config _config)
        {
            config = _config;
            config.Form.SetTopMost(Handle);
            loading = _config.Call != null;
            if (config.Font != null) Font = config.Font;
            else if (Config.Font != null) Font = Config.Font;
            else Font = config.Form.Font;
            Icon = config.Form.Icon;
            Helper.GDI(g =>
            {
                SetSize(RenderMeasure(g));
            });
            IInit();
        }
        internal override TAlignFrom Align => config.Align;

        bool loading = false, loadingend = true;
        int AnimationLoadingValue = 0;
        ITask? ThreadLoading = null;
        public void IInit()
        {
            SetPosition(config.Form);
            if (loading)
            {
                ThreadLoading = new ITask(this, i =>
                {
                    AnimationLoadingValue = i;
                    Print();
                    return loading;
                }, 20, 360, 10);
            }
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
                    try
                    {
                        config.Call(config);
                    }
                    catch { }
                    loading = false;
                    ThreadLoading?.Dispose();
                    if (IRefresh())
                    {
                        loadingend = true;
                        return;
                    }
                }
                loadingend = true;
                if (config.AutoClose > 0)
                {
                    ITask.Run(() =>
                    {
                        Thread.Sleep(config.AutoClose * 1000);
                        IClose();
                    });
                }
                else IClose();
            });
        }

        bool IRefresh()
        {
            var oldw = TargetRect.Width;
            if (IsHandleCreated)
            {
                Helper.GDI(g =>
                {
                    SetSize(RenderMeasure(g));
                });
                SetPositionCenter(oldw);
                return false;
            }
            else return true;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (loadingend) IClose();
            base.OnMouseClick(e);
        }

        #region 渲染

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
                if (loading)
                {
                    using (var brush = new Pen(Style.Db.Fill, 3F))
                    {
                        g.DrawEllipse(brush, rect_loading);
                    }
                    using (var brush = new Pen(Style.Db.Primary, 3F))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect_loading, AnimationLoadingValue, 100);
                    }
                }
                else if (config.Icon != TType.None) g.PaintIcons(config.Icon, rect_icon);
                using (var brush = new SolidBrush(Style.Db.TextBase))
                {
                    g.DrawString(config.Text, Font, brush, rect_txt, Helper.stringFormatLeft);
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
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null || (shadow_temp.Width != rect_client.Width || shadow_temp.Height != rect_client.Height))
                {
                    shadow_temp?.Dispose();
                    shadow_temp = path.PaintShadow(rect_client.Width, rect_client.Height);
                }
                g.DrawImage(shadow_temp, rect_client, 0.2F);
            }
            return path;
        }

        Rectangle rect_icon, rect_loading, rect_txt;
        Size RenderMeasure(Graphics g)
        {
            float dpi = Config.Dpi;
            var size = g.MeasureString(config.Text, Font);
            int px = (int)(14 * dpi), sp = (int)(8 * dpi);
            int width = (int)Math.Ceiling(size.Width), height = (int)Math.Ceiling(size.Height + (12 * dpi) * 2);

            if (loading)
            {
                int icon_size = (int)(size.Height * 0.86F);
                rect_icon = new Rectangle(px, (height - icon_size) / 2, icon_size, icon_size);
                rect_txt = new Rectangle(rect_icon.Right + sp, 0, width, height);

                int loading_size = (int)(icon_size * 0.86F);
                rect_loading = new Rectangle(rect_icon.X + (rect_icon.Width - loading_size) / 2, rect_icon.Y + (rect_icon.Height - loading_size) / 2, loading_size, loading_size);

                return new Size(width + icon_size + sp + px * 2, height);
            }
            else if (config.Icon == TType.None)
            {
                rect_txt = new Rectangle(px, 0, width, height);
                return new Size(width + px * 2, height);
            }
            else
            {
                int icon_size = (int)(size.Height * 0.86F);
                rect_icon = new Rectangle(px, (height - icon_size) / 2, icon_size, icon_size);
                rect_txt = new Rectangle(rect_icon.Right + sp, 0, width, height);
                return new Size(width + icon_size + sp + px * 2, height);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ThreadLoading?.Dispose();
            base.Dispose(disposing);
        }
    }
}