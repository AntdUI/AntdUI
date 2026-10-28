// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    /// <summary>
    /// Markdown 链接类型
    /// </summary>
    public enum MarkdownLinkKind
    {
        /// <summary>外部地址（http / https / mailto 等，由外部决定如何打开）</summary>
        External,
        /// <summary>文档内锚点（#标题），由控件滚动定位</summary>
        Anchor,
        /// <summary>基于 BasePath 解析到的本地 Markdown 文件</summary>
        Document,
    }

    /// <summary>
    /// Markdown 超链接事件参数
    /// </summary>
    public class MarkdownLinkEventArgs : EventArgs
    {
        public MarkdownLinkEventArgs(string url, string resolvedUrl, MarkdownLinkKind kind, string? anchor, string? filePath)
        {
            Url = url;
            ResolvedUrl = resolvedUrl;
            Kind = kind;
            Anchor = anchor;
            FilePath = filePath;
        }

        /// <summary>
        /// 链接原始地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 基于 BasePath 解析后的地址（#锚点 原样返回）
        /// </summary>
        public string ResolvedUrl { get; private set; }

        /// <summary>
        /// 链接类型
        /// </summary>
        public MarkdownLinkKind Kind { get; private set; }

        /// <summary>
        /// 锚点（已解码、已转为 slug，不含 #）；<see cref="MarkdownLinkKind.Document"/> 时为目标文件内的锚点
        /// </summary>
        public string? Anchor { get; private set; }

        /// <summary>
        /// 目标 Markdown 文件绝对路径（仅 <see cref="MarkdownLinkKind.Document"/>）
        /// </summary>
        public string? FilePath { get; private set; }

        /// <summary>
        /// 是否已处理（设置为 true 后控件不再执行内置跳转）
        /// </summary>
        public bool Handled { get; set; }
    }

    /// <summary>
    /// Markdown 图片请求事件参数
    /// </summary>
    public class MarkdownImageEventArgs : EventArgs
    {
        public MarkdownImageEventArgs(string url, string? alt)
        {
            Url = url;
            Alt = alt;
        }

        /// <summary>
        /// 图片原始地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 图片替代文本
        /// </summary>
        public string? Alt { get; private set; }
    }
}