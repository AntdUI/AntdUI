// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AntdUI
{
    public class FluentFlat
    {
        public static Dictionary<string, string> Emoji;
        static FluentFlat()
        {
            var datas = DecompressString(Properties.Resources.D).Split('|');
            Emoji = new Dictionary<string, string>(datas.Length);
            foreach (string s in datas)
            {
                var i = s.IndexOf(":");
                Emoji.Add(s.Substring(0, i), Rest(s.Substring(i + 1)));
            }
        }

        static string Rest(string svg) => "<svg " + svg.Replace("[VBD", "viewBox=\"").Replace("[VB2", "viewBox=\"0 0 1024 1024\">").Replace("[PD", "<path d=\"").Replace("[PE", "</path>").Replace("[PG", "\"></path>") + "</svg>";

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="text">待压缩的字符串</param>
        /// <returns>压缩后的 Base64 编码字符串</returns>
        static string CompressString(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(buffer, 0, buffer.Length);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="compressedText">压缩后的 Base64 编码字符串</param>
        /// <returns>解压缩后的字符串</returns>
        static string DecompressString(string compressedText)
        {
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(compressedText)))
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gZipStream.CopyTo(outputStream);
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }
}