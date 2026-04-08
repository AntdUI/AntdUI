// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace AntdUI
{
    partial class Helper
    {
        const int PropertyTagFrameDelay = 0x5100;
        public static int GIFInfo(Image image, out FrameDimension fd)
        {
            fd = new FrameDimension(image.FrameDimensionsList[0]);
            return image.GetFrameCount(fd);
        }
        public static bool IsGIF(Image? image)
        {
            if (image == null) return false;
            try
            {
                var fd = new FrameDimension(image.FrameDimensionsList[0]);
                return image.GetFrameCount(fd) > 1;
            }
            catch { return false; }
        }
        public static int[] GIFDelays(Image image, int count)
        {
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

        static ConcurrentDictionary<Image, Task> tasks = new ConcurrentDictionary<Image, Task>();
        public static bool GIFCanPlay(Image image)
        {
            if (tasks.TryGetValue(image, out _)) return false;
            else return true;
        }
        public static bool GIFPlay(Image image, Func<Image, bool> isRun, Action r)
        {
            if (tasks.TryGetValue(image, out var tmp)) return true;
            int count = GIFInfo(image, out var fd);
            if (count > 1)
            {
                var task = ITask.Run(() =>
                {
                    var delays = GIFDelays(image, count);
                    while (isRun(image))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (isRun(image))
                            {
                                try
                                {
                                    image.SelectActiveFrame(fd, i);
                                }
                                catch (ArgumentException)
                                {
                                    tasks.TryRemove(image, out _);
                                    return;
                                }
                                catch { }
                                r();
                                Thread.Sleep(Math.Max(delays[i], 10));
                            }
                            else
                            {
                                tasks.TryRemove(image, out _);
                                image.SelectActiveFrame(fd, 0);
                                return;
                            }
                        }
                    }
                }, r);
                tasks.TryAdd(image, task);
                return true;
            }
            else return false;
        }
    }
}