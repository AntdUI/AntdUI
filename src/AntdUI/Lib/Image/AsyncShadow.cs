// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AntdUI
{
    public class AsyncShadow : IDisposable
    {
        Panel control;
        Func<Rectangle, GraphicsPath> call;
        public AsyncShadow(Panel c, Func<Rectangle, GraphicsPath> action)
        {
            control = c;
            call = action;
        }

        Bitmap? temp;
        string? wh;
        bool has = false;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        public void Draw(Canvas g, Rectangle rect_client, Rectangle rect_read, float dpi, int shadow, int shadowOffsetX, int shadowOffsetY, Color shadowColor, float opacity)
        {
            int size = (int)(shadow * dpi);
            if (temp == null || temp.PixelFormat == PixelFormat.DontCare)
            {
                DisposeTmp();
                tag = new object[] { rect_client, rect_read, size, shadowColor };
                Core();
            }
            else
            {
                var wh2 = rect_client.Width + "_" + rect_client.Height;
                if (wh != wh2)
                {
                    wh = wh2;
                    tag = new object[] { rect_client, rect_read, size, shadowColor };
                    Core();
                }
            }
            if (temp == null) return;
            int offsetX = (int)(shadowOffsetX * dpi), offsetY = (int)(shadowOffsetY * dpi);
            if (offsetX == 0 && offsetY == 0) g.Image(temp, rect_client, opacity);
            else g.Image(temp, new Rectangle(rect_client.X + offsetX, rect_client.Y + offsetY, rect_client.Width, rect_client.Height), opacity);
        }

        object[]? tag;
        bool run = false;
        string? code;
        void Core()
        {
            if (control.ShadowDelay > 0)
            {
                code = DateTime.Now.Ticks.ToString();
                if (run) return;
                run = true;
                ITask.Run(() =>
                {
                    int count = 0;
                    while (true)
                    {
                        var cid = code;
                        if (has) System.Threading.Thread.Sleep(control.ShadowDelay);
                        if (cid == code)
                        {
                            Rectangle rect_client = (Rectangle)tag![0], rect_read = (Rectangle)tag[1];
                            using (var path = call(rect_read))
                            {
                                var tmp = path.PaintShadowO(rect_client.Width, rect_client.Height, (Color)tag[3], (int)tag[2]);
                                wh = tmp.Width + "_" + tmp.Height;
                                temp?.Dispose();
                                temp = tmp;
                                has = true;
                                control.Invalidate();
                            }
                            run = false;
                            return;
                        }
                        count++;
                    }
                });
            }
            else
            {
                Rectangle rect_client = (Rectangle)tag![0], rect_read = (Rectangle)tag[1];
                using (var path = call(rect_read))
                {
                    var tmp = path.PaintShadowO(rect_client.Width, rect_client.Height, (Color)tag[3], (int)tag[2]);
                    wh = tmp.Width + "_" + tmp.Height;
                    temp?.Dispose();
                    temp = tmp;
                    has = true;
                    control.Invalidate();
                }
            }
        }

        public void Change() => wh = null;

        public void DisposeTmp()
        {
            temp?.Dispose();
            temp = null;
        }

        public void Dispose()
        {
            has = false;
            DisposeTmp();
        }
    }
}