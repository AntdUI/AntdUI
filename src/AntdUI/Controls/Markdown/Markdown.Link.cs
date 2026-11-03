// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AntdUI
{
    partial class Markdown
    {
        #region 属性

        bool linkNavigation = true;
        /// <summary>
        /// 启用内置跳转：#锚点 滚动定位、基于 BasePath 的相对 Markdown 文件在同一控件内切换
        /// </summary>
        [Description("启用内置跳转（#锚点 / 相对 Markdown 文件）"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool LinkNavigation
        {
            get => linkNavigation;
            set
            {
                if (linkNavigation == value) return;
                linkNavigation = value;
                OnPropertyChanged(nameof(LinkNavigation));
            }
        }

        /// <summary>
        /// 文档锚点（标题）列表，按文档顺序排列
        /// </summary>
        [Browsable(false)]
        public IList<MarkdownAnchor> Anchors => renderer.Anchors();

        #endregion

        #region 链接解析

        /// <summary>
        /// 解析链接（类型 / 解析后地址 / 锚点 / 目标文件），供点击事件与外部主动调用共用
        /// </summary>
        /// <param name="url">链接原始地址</param>
        public MarkdownLinkEventArgs ParseLink(string url) => BuildLink(url ?? "");

        MarkdownLinkEventArgs BuildLink(string url) => renderer.BuildLink(url);

        /// <summary>
        /// 锚点标准化：URL 解码 + GitHub slug 化（同时兼容 #标题 与 #title-slug 两种写法）
        /// </summary>
        public static string NormalizeAnchor(string? anchor) => MarkdownRenderer.NormalizeAnchor(anchor);

        #endregion

        #region 定位

        /// <summary>待定位锚点（布局尚未完成时暂存，布局结束后执行滚动）</summary>
        string? pendingAnchor;

        /// <summary>
        /// 滚动定位到指定锚点（#标题），布局尚未就绪时延迟到布局结束后定位
        /// </summary>
        /// <param name="anchor">锚点（可带 #，兼容 #标题 与 #title-slug 两种写法）</param>
        /// <returns>锚点存在（或已延迟定位）返回 true；文档已布局但没有该锚点返回 false</returns>
        public bool ScrollToAnchor(string? anchor)
        {
            string key = NormalizeAnchor(anchor);
            if (key.Length == 0) return false;
            int y;
            if (!renderer.TryGetAnchor(key, out y))
            {
                // 句柄未创建 / 尚未布局：锚点位置未知，延迟到布局结束后再定位
                if (!IsHandleCreated || !renderer.HasContent)
                {
                    pendingAnchor = key;
                    return true;
                }
                return false;
            }
            pendingAnchor = null;
            ApplyAnchor(y);
            return true;
        }

        /// <summary>
        /// 跳转到链接（#锚点 / 相对 Markdown 文件），返回是否已处理
        /// </summary>
        public bool NavigateTo(string? url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            return Navigate(BuildLink(url!));
        }

        /// <summary>
        /// 内置跳转：锚点滚动 / 同控件内切换 Markdown 文件
        /// </summary>
        bool Navigate(MarkdownLinkEventArgs e)
        {
            if (!linkNavigation) return false;
            switch (e.Kind)
            {
                case MarkdownLinkKind.Anchor:
                    return ScrollToAnchor(e.Anchor);
                case MarkdownLinkKind.Document:
                    return LoadDocument(e.FilePath, e.Anchor);
                default:
                    return false;
            }
        }

        /// <summary>
        /// 在同一控件内加载 Markdown 文件（BasePath 切换为文件所在目录，滚动条重置后可再定位锚点）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="anchor">加载后定位的锚点</param>
        public bool LoadDocument(string? path, string? anchor = null)
        {
            if (string.IsNullOrEmpty(path)) return false;
            string text;
            try { text = File.ReadAllText(path!); }
            catch { return false; }
            string? dir = null;
            try { dir = Path.GetDirectoryName(path); } catch { }
            if (!string.IsNullOrEmpty(dir)) BasePath = dir;
            SetMarkdown(text, true);
            if (!string.IsNullOrEmpty(anchor)) ScrollToAnchor(anchor);
            return true;
        }

        int FindAnchorY(string key)
        {
            int y;
            if (renderer.TryGetAnchor(key, out y)) return y;
            return -1;
        }

        void ApplyAnchor(int y)
        {
            if (!IsHandleCreated) return;
            int target = y - (int)Math.Round(4 * Dpi);
            // 内容不足一屏时滚动范围为空，必须夹到 0，否则内容会被滚出可视区
            int max = ScrollBar.VrValueI;
            if (max < 0) max = 0;
            if (target > max) target = max;
            ScrollBar.Value = target > 0 ? target : 0;
        }

        #endregion
    }

    /// <summary>
    /// Markdown 文档锚点（标题）
    /// </summary>
    public class MarkdownAnchor
    {
        public MarkdownAnchor(string slug, string text, int level, int y)
        {
            Slug = slug;
            Text = text;
            Level = level;
            Y = y;
        }

        /// <summary>
        /// 锚点（GitHub 风格 slug，可直接用于 #锚点 链接）
        /// </summary>
        public string Slug { get; private set; }

        /// <summary>
        /// 标题文本
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 标题层级（1-6）
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 内容坐标 Y（布局完成后有效）
        /// </summary>
        public int Y { get; private set; }
    }
}