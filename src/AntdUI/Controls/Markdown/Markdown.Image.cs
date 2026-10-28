// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    partial class Markdown
    {
        readonly Dictionary<string, MDImageEntry> imageCache = new Dictionary<string, MDImageEntry>(StringComparer.Ordinal);

        sealed class MDImageEntry
        {
            public Image? Image;
            public bool Complete;

            public MDImageEntry SetOK(Image? value)
            {
                Complete = true;
                Image = value;
                return this;
            }
        }

        #region 图片缓存

        /// <summary>
        /// 设置图片（供外部下载完成后回调，支持跨线程调用）
        /// </summary>
        /// <param name="url">图片原始地址（与 <see cref="MarkdownImageEventArgs.Url"/> 一致）</param>
        /// <param name="content">图片内容：普通图片流（PNG/JPG/GIF/BMP 等）或 SVG 文本，null 表示加载失败</param>
        public void SetImage(string url, byte[]? content) => SetImage(url, content, null);

        /// <summary>
        /// 设置图片内容（总入口，自动识别 SVG 文本 / 普通图片流，支持跨线程调用）
        /// </summary>
        /// <param name="url">图片原始地址（与 <see cref="MarkdownImageEventArgs.Url"/> 一致）</param>
        /// <param name="content">图片内容，null 表示加载失败</param>
        /// <param name="contentType">内容类型（如 image/png、image/svg+xml;charset=utf-8），为空时按内容自动识别</param>
        public void SetImage(string url, byte[]? content, string? contentType)
        {
            var image = DecodeImage(content, contentType);
            if (!imageCache.TryGetValue(url, out var entry))
            {
                entry = new MDImageEntry();
                imageCache[url] = entry;
            }
            if (!ReferenceEquals(entry.Image, image)) DisposeImage(entry.Image);
            entry.SetOK(image);
            InvalidateImageLayout();
        }

        /// <summary>
        /// 清空图片缓存（随后会按新的解析结果重新请求）
        /// </summary>
        public void ClearImageCache()
        {
            DisposeImages();
            LoadLayout(true);
            RequestImages();
        }

        /// <summary>
        /// 释放缓存中由控件解码创建的图片（外部传入的不释放）
        /// </summary>
        void DisposeImages()
        {
            foreach (var entry in imageCache.Values) DisposeImage(entry.Image);
            imageCache.Clear();
        }

        internal Image? GetImage(string? url)
        {
            if (url == null) return null;
            if (imageCache.TryGetValue(url, out var entry)) return entry.Image;
            return null;
        }

        static void DisposeImage(Image? image)
        {
            if (image == null) return;
            try { image.Dispose(); } catch { }
        }

        #endregion

        #region 图片请求

        /// <summary>
        /// 是否正在按队列批量请求图片（期间合并重新布局）
        /// </summary>
        bool requestingImages;
        bool imageLayoutDirty;

        /// <summary>
        /// 图片变化后刷新布局（批量请求期间合并为一次）
        /// </summary>
        void InvalidateImageLayout()
        {
            if (requestingImages)
            {
                imageLayoutDirty = true;
                return;
            }
            LoadLayout(true);
        }

        /// <summary>
        /// 收集并请求未加载的图片（内部不实现任何下载逻辑）
        /// </summary>
        /// <remarks>
        /// 请求队列来自文档模型（<see cref="doc"/>）而不是排版结果（lines）：
        /// 排版结果是布局的中间产物，会被重新布局清空重建，且未加载的图片在排版中尺寸为占位值，
        /// 一旦以排版结果为准就会漏请求（历史上只请求到第一张图片）。
        /// </remarks>
        void RequestImages()
        {
            var document = doc;
            if (document == null) return;
            var queue = new List<string>();
            var alts = new List<string?>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var blocks = document.Blocks;
            for (int i = 0; i < blocks.Count; i++) CollectImage(blocks[i], queue, alts, seen);
            if (queue.Count == 0) return;
            requestingImages = true;
            try
            {
                for (int i = 0; i < queue.Count; i++)
                {
                    string src = queue[i];
                    if (imageCache.TryGetValue(src, out var entry))
                    {
                        if (entry.Complete || entry.Image != null) continue;
                    }
                    RequestImage(src, alts[i]);
                }
            }
            finally
            {
                requestingImages = false;
                if (imageLayoutDirty)
                {
                    imageLayoutDirty = false;
                    LoadLayout(true);
                }
            }
        }

        /// <summary>
        /// 递归收集块内图片（表格单元格、块引用子块）
        /// </summary>
        static void CollectImage(MDBlock block, List<string> queue, List<string?> alts, HashSet<string> seen)
        {
            if (block.Rows != null)
            {
                foreach (var row in block.Rows)
                {
                    if (row == null) continue;
                    foreach (var cell in row)
                    {
                        if (cell == null) continue;
                        CollectImage(cell.Inlines, queue, alts, seen);
                    }
                }
            }
            else CollectImage(block.Inlines, queue, alts, seen);
            if (block.Children != null)
            {
                foreach (var child in block.Children)
                {
                    if (child != null) CollectImage(child, queue, alts, seen);
                }
            }
        }

        static void CollectImage(List<MDInline> inlines, List<string> queue, List<string?> alts, HashSet<string> seen)
        {
            foreach (var inline in inlines)
            {
                if (inline.Kind != MDInlineKind.Image) continue;
                string? src = inline.Url;
                if (string.IsNullOrEmpty(src)) continue;
                if (!seen.Add(src!)) continue;
                queue.Add(src!);
                alts.Add(inline.Text);
            }
        }

        /// <summary>
        /// 发起单个图片请求（不阻塞、不 await，全部走回调/续接）
        /// </summary>
        void RequestImage(string url, string? alt)
        {
            // 本地路径（基于 BasePath 的相对路径 / 本地绝对路径 / data URI）：直接读取，
            // 读取与解码都放线程池，避免大文件卡 UI；不触发网络事件
            try
            {
                if (IsRemoteUrl(url))
                {
                    var entry = new MDImageEntry();
                    imageCache[url] = entry;
                    OnImageRequest(url, alt);
                    return;
                }
                var local = LoadLocalImage(ResolveUrl(url));
                var localEntry = new MDImageEntry { Image = local, Complete = true };
                imageCache[url] = localEntry;
                if (local != null) InvalidateImageLayout();
            }
            catch
            {
            }
        }

        #endregion

        #region 图片解码

        /// <summary>
        /// 解码图片内容（自动识别 SVG 文本 / 普通图片流）
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="contentType">内容类型，可为空（按内容嗅探）</param>
        static Image? DecodeImage(byte[]? content, string? contentType)
        {
            if (content == null || content.Length == 0) return null;
            try
            {
                // SVG 是 XML 文本，不是图片流，不能直接交给 Bitmap 解码
                if (IsSvgContent(content, contentType))
                {
                    var svg = DecodeSvg(content);
                    if (svg != null) return svg;
                    // 类型标注不可靠（或 SVG 解析失败）时，回退按图片流解码
                }
                // 注意：Bitmap 生命周期内需保持流打开，不可释放
                var ms = new System.IO.MemoryStream(content);
                return new Bitmap(ms);
            }
            catch { return null; }
        }

        /// <summary>
        /// 是否为 SVG 内容
        /// </summary>
        static bool IsSvgContent(byte[] content, string? contentType)
        {
            if (!string.IsNullOrEmpty(contentType))
            {
                string type = contentType!.Trim();
                // 例如 image/svg+xml;charset=utf-8
                if (type.StartsWith("image/svg", StringComparison.OrdinalIgnoreCase)) return true;
                if (type.StartsWith("text/xml", StringComparison.OrdinalIgnoreCase)) return true;
                if (type.StartsWith("application/xml", StringComparison.OrdinalIgnoreCase)) return true;
                // 其他明确的图片类型（image/png、image/jpeg…）
                if (type.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return false;
            }
            // 无可靠类型：按内容嗅探（跳过 BOM 与空白，'<' 开头视为 SVG/XML）
            int i = SkipPreamble(content);
            return i < content.Length && content[i] == (byte)'<';
        }

        /// <summary>
        /// SVG 文本转图片
        /// </summary>
        static Image? DecodeSvg(byte[] content)
        {
            try
            {
                var doc = SvgExtend.SvgDocument(TextOf(content));
                if (doc == null) return null;
                return doc.Draw();
            }
            catch { return null; }
        }

        /// <summary>
        /// 内容转文本（识别 BOM）
        /// </summary>
        static string TextOf(byte[] content)
        {
            if (content.Length >= 3 && content[0] == 0xEF && content[1] == 0xBB && content[2] == 0xBF) return System.Text.Encoding.UTF8.GetString(content, 3, content.Length - 3);
            if (content.Length >= 2 && content[0] == 0xFF && content[1] == 0xFE) return System.Text.Encoding.Unicode.GetString(content, 2, content.Length - 2);
            if (content.Length >= 2 && content[0] == 0xFE && content[1] == 0xFF) return System.Text.Encoding.BigEndianUnicode.GetString(content, 2, content.Length - 2);
            return System.Text.Encoding.UTF8.GetString(content);
        }

        /// <summary>
        /// 跳过 BOM 与空白字符
        /// </summary>
        static int SkipPreamble(byte[] content)
        {
            int i = 0;
            if (content.Length >= 3 && content[0] == 0xEF && content[1] == 0xBB && content[2] == 0xBF) i = 3;
            while (i < content.Length)
            {
                byte b = content[i];
                if (b == (byte)' ' || b == (byte)'\t' || b == (byte)'\r' || b == (byte)'\n') i++;
                else break;
            }
            return i;
        }

        #endregion

        #region 本地图片

        /// <summary>
        /// 是否为需要网络请求的远程地址
        /// </summary>
        static bool IsRemoteUrl(string url)
        {
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)) return true;
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) return true;
            if (url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase)) return true;
            if (url.StartsWith("//", StringComparison.Ordinal)) return true;
            if (url.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        /// <summary>
        /// 读取本地图片（复制到内存，避免句柄占用文件）
        /// </summary>
        static Image? LoadLocalImage(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return null;
                // data URI：内嵌 Base64 / URL 编码内容
                if (path.StartsWith("data:", StringComparison.OrdinalIgnoreCase)) return DecodeDataUri(path);
                if (!System.IO.File.Exists(path)) return null;
                return DecodeImage(System.IO.File.ReadAllBytes(path), null);
            }
            catch { return null; }
        }

        /// <summary>
        /// 解析 data URI（data:[&lt;mime&gt;][;base64],&lt;data&gt;）
        /// </summary>
        static Image? DecodeDataUri(string uri)
        {
            int comma = uri.IndexOf(',');
            if (comma < 0) return null;
            string meta = uri.Substring(5, comma - 5);   // 去掉 "data:"
            string data = uri.Substring(comma + 1);
            string mime = meta;
            int semi = meta.IndexOf(';');
            if (semi >= 0) mime = meta.Substring(0, semi);
            if (meta.IndexOf("base64", StringComparison.OrdinalIgnoreCase) >= 0) return DecodeImage(Convert.FromBase64String(data), mime);
            // 非 base64：按 URL 编码的文本处理（常见于内联 SVG）
            return DecodeImage(System.Text.Encoding.UTF8.GetBytes(Uri.UnescapeDataString(data)), mime);
        }

        #endregion

        #region 路径解析

        /// <summary>
        /// 将相对路径解析为可访问地址
        /// </summary>
        public string ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return url ?? "";
            string value = url.Trim();
            if (value[0] == '#') return value;
            if (StartsWithProtocol(value)) return value;
            var root = BasePath;
            if (string.IsNullOrEmpty(root)) return value;
            if (StartsWithProtocol(root!))
            {
                try
                {
                    var baseUri = new Uri(root!);
                    return new Uri(baseUri, value).ToString();
                }
                catch { return value; }
            }
            if (System.IO.Path.IsPathRooted(value)) return value;
            try { return System.IO.Path.GetFullPath(System.IO.Path.Combine(root!, value)); }
            catch { return value; }
        }

        static bool StartsWithProtocol(string value)
        {
            int p = value.IndexOf(':');
            if (p <= 0) return false;
            // Windows 盘符（如 C:\dir）不是协议
            if (p == 1 && char.IsLetter(value[0])) return false;
            for (int i = 0; i < p; i++)
            {
                char c = value[i];
                if (!char.IsLetterOrDigit(c) && c != '+' && c != '-' && c != '.') return false;
            }
            return true;
        }

        #endregion
    }
}