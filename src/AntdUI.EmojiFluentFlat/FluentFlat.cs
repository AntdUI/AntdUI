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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

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
                Emoji.Add(s.Substring(0, i), s.Substring(i + 1));
            }
        }

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