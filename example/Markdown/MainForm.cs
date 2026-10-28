// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace MarkdownDemo
{
    /// <summary>
    /// Markdown 控件演示：左侧 Input 实时编辑，右侧实时渲染；
    /// 支持单页文档导航（#锚点定位 / 相对 Markdown 文件切换）与拖入 md 文件
    /// </summary>
    public partial class MainForm : AntdUI.Window
    {
        /// <summary>
        /// 网络图片下载客户端（本地相对路径图片由控件基于 BasePath 直接读取，仅网络 URL 触发 ImageRequest）
        /// </summary>
        static readonly HttpClient HttpClient = new HttpClient();

        public MainForm()
        {
            InitializeComponent();

            input.TextChanged += Input_TextChanged;
            input.AllowDrop = true;
            input.DragEnter += DragEnter_File;
            input.DragDrop += DragDrop_File;
            render.AllowDrop = true;
            render.DragEnter += DragEnter_File;
            render.DragDrop += DragDrop_File;
            AllowDrop = true;
            DragEnter += DragEnter_File;
            DragDrop += DragDrop_File;

            // 网络图片：异步下载后回传给控件
            render.ImageRequest += Render_ImageRequest;
            // 链接点击：外部地址交给宿主打开，md 文件在同一控件内切换，锚点由控件滚动定位
            render.LinkClick += Render_LinkClick;

            // 演示文档写入程序目录下 docs/，相对链接由 BasePath 解析（先挂好事件，加载即触发渲染）
            EnsureDocs();
            LoadPage(IndexPath);
        }

        #region 单页导航

        static string DocsRoot => Path.Combine(AppContext.BaseDirectory, "docs");
        static string IndexPath => Path.Combine(DocsRoot, "index.md");

        /// <summary>
        /// 链接点击：由宿主决定是否跳转（Handled = true 表示已接管，控件不再执行内置跳转）
        /// </summary>
        void Render_LinkClick(object? sender, AntdUI.MarkdownLinkEventArgs e)
        {
            switch (e.Kind)
            {
                case AntdUI.MarkdownLinkKind.External:
                    // 外部地址：交给系统浏览器
                    e.Handled = true;
                    OpenExternal(e.ResolvedUrl);
                    break;
                case AntdUI.MarkdownLinkKind.Document:
                    // 相对 Markdown 文件：同一个控件内替换内容（单页跳转，不保留返回历史）
                    e.Handled = true;
                    LoadPage(e.FilePath!, e.Anchor);
                    break;
                case AntdUI.MarkdownLinkKind.Anchor:
                    // 不设置 Handled：控件内置滚动定位到标题
                    break;
            }
        }

        void OpenExternal(string url)
        {
            try
            {
                if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, "打开链接失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 在同一控件内加载文档（BasePath 切换为文件所在目录，必要时定位锚点）
        /// </summary>
        void LoadPage(string path, string? anchor = null)
        {
            try
            {
                if (!File.Exists(path))
                {
                    AntdUI.Message.error(this, "文件不存在：" + path);
                    return;
                }
                render.BasePath = Path.GetDirectoryName(path);
                input.Text = File.ReadAllText(path);
                render.ScrollToTop();
                if (!string.IsNullOrEmpty(anchor)) render.ScrollToAnchor(anchor);
                header1.Description = "当前文档：" + Path.GetFileName(path);
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, "打开失败：" + ex.Message);
            }
        }

        #endregion

        #region 图片

        /// <summary>
        /// 网络图片加载（async void + await 即可，控件不会阻塞等待；回填、解码、重排都由控件内部接管）
        /// </summary>
        async void Render_ImageRequest(object? sender, AntdUI.MarkdownImageEventArgs e)
        {
            // HttpClient 是静态复用的：连接池复用，避免每张图新建连接
            using (var response = await HttpClient.GetAsync(e.Url))
            {
                if (!response.IsSuccessStatusCode) return;
                byte[] content = await response.Content.ReadAsByteArrayAsync();
                // 响应类型不一定是图片流：例如 image/svg+xml 会返回 image/svg+xml;charset=utf-8（SVG 是 XML 文本）
                // 因此把原始内容与 Content-Type 一起交给控件，由控件统一识别 SVG / 普通图片（解码在线程池，不卡 UI）
                render.SetImage(e.Url, content, response.Content.Headers.ContentType?.ToString());
            }
        }

        #endregion

        #region 编辑

        void Input_TextChanged(object? sender, EventArgs e) => SyncRender();

        /// <summary>
        /// 编辑内容变化后实时更新渲染（Markdown 控件内部做增量布局）
        /// </summary>
        void SyncRender()
        {
            render.SetMarkdown(input.Text, true);
            render.ScrollToTop();
        }

        #endregion

        #region 拖入 Markdown 文件

        void DragEnter_File(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0 && IsMarkdown(files[0])) e.Effect = DragDropEffects.Copy;
            }
        }

        void DragDrop_File(object? sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0) return;
            string file = files[0];
            if (!IsMarkdown(file)) return;
            LoadPage(file);
        }

        static bool IsMarkdown(string file)
        {
            string ext = Path.GetExtension(file).ToLower();
            return ext == ".md" || ext == ".markdown" || ext == ".mdx";
        }

        #endregion

        #region 演示文档

        /// <summary>
        /// 写入演示文档（index.md / guide.md），用于演示 #锚点 与相对路径的单页跳转
        /// </summary>
        static void EnsureDocs()
        {
            try
            {
                Directory.CreateDirectory(DocsRoot);
                File.WriteAllText(IndexPath, IndexDoc);
                File.WriteAllText(Path.Combine(DocsRoot, "guide.md"), GuideDoc);
            }
            catch { }
        }

        const string IndexDoc = @"# AntdUI Markdown 文档

左侧可编辑，右侧实时渲染。整个演示是**单页导航**：点击目录或正文里的链接，只在右侧面板内切换内容，不保留返回历史。

> [!TIP]
> - 目录跳转：`[表格](#表格)` —— 控件内滚动定位到标题
> - 跨文档跳转：`[guide.md](guide.md)` —— 基于 `BasePath` 定位文件，同一控件内加载
> - 外部链接：由 `LinkClick` 事件交给宿主打开

## 目录

| 章节 | 跳转方式 |
| :--- | :---: |
| [表格](#表格) | 锚点 |
| [代码高亮](#代码高亮) | 锚点 |
| [提示块](#提示块) | 锚点 |
| [guide.md](guide.md) | 相对路径 |
| [guide.md 的表格](guide.md#表格示例) | 相对路径 + 锚点 |
| [AntdUI 仓库](https://github.com/AntdUI/AntdUI) | 外部地址 |

## 表格

表格外边框与纵向边框由一个父容器统一绘制（圆角描边 + 内部分隔线），单元格文本按列对齐方式排版。

| 左对齐 | 居中 | 右对齐 |
| :--- | :---: | ---: |
| 普通文本 | **加粗** | 1,280 |
| `行内代码` | [回到表格](#表格) | 36.5% |
| 超长内容会自动换行，行高取同一行中最高的单元格，网格与文本始终对齐 | 居中文本 | 99 |

## 代码高亮

```csharp
public class MarkdownTable : IControl
{
    protected override void OnDraw(DrawEventArgs e)
    {
        base.OnDraw(e);
        // 表格父容器：外边框 + 纵向边框
        e.Canvas.Draw(ColorScheme.Border, 1, path);
    }
}
```

```diff
+ 新增：表格父容器边框
- 移除：逐行线条拼接
```

## 提示块

> [!NOTE]
> 提示块内的表格同样使用父容器绘制边框。

| 名称 | 默认值 |
| :--- | :--- |
| `TableBorder` | `ColorScheme.Border` |
| `TableHeaderBack` | `BgBase` |
| `TableStripe` | `#f6f8fa` |

> [!WARNING]
> 表格宽度由内容决定，超过可视宽度时按比例压缩列宽。

## 图片

内嵌 SVG（`data:image/svg+xml;base64,...` 不是图片流，控件会自动识别为 SVG 文本渲染）：

![SVG 内联图片](data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNjAiIGhlaWdodD0iODAiPjxyZWN0IHdpZHRoPSIyNjAiIGhlaWdodD0iODAiIHJ4PSIxMCIgZmlsbD0iIzE2NzdmZiIvPjxjaXJjbGUgY3g9IjQwIiBjeT0iNDAiIHI9IjE4IiBmaWxsPSIjZmZmZmZmIiBvcGFjaXR5PSIwLjkiLz48cmVjdCB4PSI3MCIgeT0iMjYiIHdpZHRoPSIxNTAiIGhlaWdodD0iMTIiIHJ4PSI2IiBmaWxsPSIjZmZmZmZmIiBvcGFjaXR5PSIwLjkiLz48cmVjdCB4PSI3MCIgeT0iNDYiIHdpZHRoPSIxMDAiIGhlaWdodD0iMTAiIHJ4PSI1IiBmaWxsPSIjZmZmZmZmIiBvcGFjaXR5PSIwLjYiLz48L3N2Zz4=)

---

[前往 guide.md](guide.md) ｜ [AntdUI 主页](https://github.com/AntdUI/AntdUI)
";

        const string GuideDoc = @"# guide.md 分页文档

[返回首页](index.md) ｜ [首页的表格](index.md#表格) ｜ [本页表格](#表格示例)

> [!IMPORTANT]
> 这是同一个 `Markdown` 控件内加载的第二个文档：切换后不提供“返回”，需要回到首页请点击上方链接。

## 表格示例

| 边框 | 绘制方式 |
| :--- | :--- |
| 外边框 | 父容器圆角描边 |
| 纵向边框 | 列分隔线贯穿整表 |
| 横向边框 | 行分隔线（末行由外框承担） |

## 列表

1. 有序列表第一项
2. 有序列表第二项
   - 嵌套无序列表
   - 嵌套无序列表

## 引用

> 引用内的相对链接同样基于 `BasePath` 解析：[回首页](index.md)。
";
        #endregion
    }
}
