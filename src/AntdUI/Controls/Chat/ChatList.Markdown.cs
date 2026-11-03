// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;

namespace AntdUI.Chat
{
    /// <summary>
    /// ChatList 的 Markdown 消息支持（<see cref="MdChatItem"/>）
    /// </summary>
    partial class ChatList
    {
        #region 绘制

        void PaintMdItem(Canvas g, MdChatItem md, int sy, float radius, SolidBrush forebubble, SolidBrush bgbubble, SolidBrush bgActiveBubble, SolidBrush forebubbleme, SolidBrush bgbubbleme, SolidBrush bgActiveBubbleme, string name)
        {
            var fore = md.Me ? forebubbleme : forebubble;
            using (var path = md.rect_read.RoundPath(radius))
            {
                using (var brush = new SolidBrush(Colour.TextTertiary.Get(ColorScheme, name, Name)))
                {
                    if (!string.IsNullOrEmpty(md.Name)) g.String(md.Name, Font, brush, md.rect_name, SFL);
                    if (ShowTimeFocused && FocusedChatItem == md && md.Time != null) g.String(md.Time, Font, brush, md.rect_time, SFL);
                }
                if (md.Me)
                {
                    g.Fill(bgbubbleme, path);
                    if (md.HasSelection) g.Fill(bgActiveBubbleme, path);
                }
                else
                {
                    g.Fill(bgbubble, path);
                    if (md.HasSelection) g.Fill(bgActiveBubble, path);
                }
                if (SelectionColor.HasValue) md.SelectionColor = SelectionColor;
                md.Paint(g, fore.Color);
            }
        }

        #endregion

        #region 图片

        /// <summary>
        /// Markdown 消息需要加载图片时发生（内部不实现下载逻辑）
        /// </summary>
        [Description("Markdown 消息需要加载图片时发生"), Category(nameof(CategoryAttribute.Action))]
        public event EventHandler<MarkdownImageEventArgs>? MarkdownImageRequest;

        internal void OnMdChatImageRequest(MdChatItem item, string url, string? alt)
        {
            MarkdownImageRequest?.Invoke(this, new MarkdownImageEventArgs(url, alt));
            // 图片在导出图片/本地文件场景已在内部加载完成，这里统一补一次重绘
            Invalidate();
        }

        /// <summary>
        /// 设置 Markdown 消息中的图片（下载完成后回调，支持跨线程调用）
        /// </summary>
        /// <param name="url">图片原始地址（与 <see cref="MarkdownImageEventArgs.Url"/> 一致）</param>
        /// <param name="content">图片内容，null 表示加载失败</param>
        /// <param name="contentType">内容类型（如 image/png、image/svg+xml），为空时按内容自动识别</param>
        public void SetMarkdownImage(string url, byte[]? content, string? contentType = null)
        {
            if (items == null || items.Count == 0) return;
            foreach (var it in items)
            {
                if (it is MdChatItem md) md.SetImage(url, content, contentType);
            }
        }

        /// <summary>
        /// 清空所有 Markdown 消息的图片缓存
        /// </summary>
        public void ClearMarkdownImageCache()
        {
            if (items == null || items.Count == 0) return;
            foreach (var it in items)
            {
                if (it is MdChatItem md) md.ClearImageCache();
            }
        }

        #endregion
    }
}