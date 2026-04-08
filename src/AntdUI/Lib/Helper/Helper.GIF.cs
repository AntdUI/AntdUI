// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace AntdUI
{
    partial class Helper
    {
        public static int GIFInfo(Image image, out FrameDimension fd)
        {
            fd = new FrameDimension(image.FrameDimensionsList[0]);
            return image.GetFrameCount(fd);
        }
        public static int[] GIFDelays(Image image, int count)
        {
            int PropertyTagFrameDelay = 0x5100;
            var propItem = image.GetPropertyItem(PropertyTagFrameDelay);
            if (propItem != null)
            {
                var bytes = propItem.Value;
                if (bytes != null)
                {
                    int[] delays = new int[count];
                    for (int i = 0; i < delays.Length; i++) delays[i] = BitConverter.ToInt32(bytes, i * 4) * 10;
                    return delays;
                }
            }
            int[] delaysd = new int[count];
            for (int i = 0; i < delaysd.Length; i++) delaysd[i] = 100;
            return delaysd;
        }
        public static Task? GIFPlay(Image image, Func<Image, bool> isRun, Action r)
        {
            int count = GIFInfo(image, out var fd);
            if (count > 1)
            {
                return ITask.Run(() =>
                {
                    var _lock = new object();
                    var delays = GIFDelays(image, count);
                    while (isRun(image))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (isRun(image))
                            {
                                lock (_lock)
                                {
                                    try
                                    {
                                        image.SelectActiveFrame(fd, i);
                                    }
                                    catch { }
                                }
                                r();
                                Thread.Sleep(Math.Max(delays[i], 10));
                            }
                            else
                            {
                                image.SelectActiveFrame(fd, 0);
                                return;
                            }
                        }
                    }
                }, r);
            }
            else return null;
        }
    }
}