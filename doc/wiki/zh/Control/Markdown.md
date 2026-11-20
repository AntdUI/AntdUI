[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Markdown

Markdown 渲染控件 📝

> 渲染 Markdown 文本：支持标题、表格、引用、列表、代码（含 Diff）高亮、分隔线、粗体/删除线、超链接、图片与 GitHub 风格提示块，并针对 AI 流式输出做了增量解析与布局优化。
>
> 控件内部不下载图片，而是通过 `ImageRequest` 事件把地址交给外部；下载完成后调用 `SetImage` 回填即可。当前为预览版（未充分测试）。

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | Markdown 文本 | string`?` | `null` |
||||
**Fore** | 文字颜色 | Color`?` | `null` |
**CodeFont** | 代码字体 | Font`?` | `null` |
**CodeFore** | 代码文本颜色 | Color`?` | `null` |
**HeadingFore** | 标题颜色 | Color`?` | `null` |
**InlineCodeFore** | 行内代码文本颜色 | Color`?` | `null` |
**LinkFore** | 超链接颜色 | Color`?` | `null` |
**QuoteFore** | 引用文本颜色 | Color`?` | `null` |
||||
**Back** | 背景颜色 | Color`?` | `null` |
**CodeBack** | 代码块背景颜色 | Color`?` | `null` |
**InlineCodeBack** | 行内代码背景颜色 | Color`?` | `null` |
**TableHeaderBack** | 表格表头背景颜色 | Color`?` | `null` |
||||
**AlertCautionIcon** | Caution 提示块图标（图标名称或 SVG 代码） | string`?` | "StopOutlined" |
**AlertImportantIcon** | Important 提示块图标（图标名称或 SVG 代码） | string`?` | "InfoOutlined" |
**AlertNoteIcon** | Note 提示块图标（图标名称或 SVG 代码） | string`?` | "InfoCircleOutlined" |
**AlertTipIcon** | Tip 提示块图标（图标名称或 SVG 代码） | string`?` | "BulbOutlined" |
**AlertWarningIcon** | Warning 提示块图标（图标名称或 SVG 代码） | string`?` | "WarningOutlined" |
||||
**AutoScroll** | 流式追加时自动滚动到底部 | bool | true |
**Selectable** | 是否允许鼠标选中内容并复制 | bool | true |
**SelectionColor** | 选中区域背景颜色 | Color`?` | `null` |
||||
**Radius** | 圆角 | int | 6 |
**TableBorder** | 表格边框颜色 | Color`?` | `null` |
||||
**CodePadding** | 代码块内边距（相对行高倍数） | float | 0.62F |
**LinkUnderline** | 超链接始终显示下划线 | bool | false |
**ListIndent** | 列表层级缩进 | int | 22 |
**QuoteIndent** | 引用缩进 | int | 16 |
**StreamInterval** | 流式刷新间隔（毫秒） | int | 30 |
**TableCellPadding** | 表格单元格左右内边距（0 = 自动按字体占比 6px/13px） | int | 0 |
**TableStripe** | 表格斑马纹颜色 | Color`?` | `null` |
||||
**AlertCautionColor** | Caution 提示块颜色 | Color`?` | `null` |
**AlertImportantColor** | Important 提示块颜色 | Color`?` | `null` |
**AlertNoteColor** | Note 提示块颜色 | Color`?` | `null` |
**AlertTipColor** | Tip 提示块颜色 | Color`?` | `null` |
**AlertWarningColor** | Warning 提示块颜色 | Color`?` | `null` |
**BasePath** | 相对路径的基路径（本地目录或 URL），用于解析图片与链接的相对地址 | string`?` | `null` |
**BlockSpacing** | 块间距（相对行高倍数） | float | 0.62F |
**CodeComment** | 代码注释颜色 | Color`?` | `null` |
**CodeKeyword** | 代码关键字颜色 | Color`?` | `null` |
**CodeNumber** | 代码数字颜色 | Color`?` | `null` |
**CodeString** | 代码字符串颜色 | Color`?` | `null` |
**CodeType** | 代码类型颜色 | Color`?` | `null` |
**LinkNavigation** | 启用内置跳转（#锚点 / 相对 Markdown 文件） | bool | true |
**QuoteBar** | 引用竖线颜色 | Color`?` | `null` |
**RuleColor** | 分隔线颜色 | Color`?` | `null` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SetMarkdown** | 设置 Markdown 文本（立即解析并布局） | void | string? markdown `Markdown 文本`, bool resetScroll `是否重置滚动条` |
**Append** | 流式追加 Markdown 文本（适用于 AI 逐字输出） | void | string? text `文本` |
**AppendLine** | 流式追加一行 Markdown 文本 | void | string? text `文本` |
**CompleteStreaming** | 结束流式输出（立即完整解析并校准布局） | void ||
**Clear** | 清空内容 | void ||
**ScrollToTop** | 滚动到顶部 | void ||
**ScrollToBottom** | 滚动到底部 | void ||
**ScrollToAnchor** | 滚动定位到指定锚点（#标题），布局尚未就绪时延迟定位 | bool | string? anchor `锚点（可带 #）` |
**NavigateTo** | 跳转到链接（#锚点 / 相对 Markdown 文件），返回是否已处理 | bool | string? url `地址` |
**LoadDocument** | 在同一控件内加载 Markdown 文件（BasePath 切换为文件所在目录） | bool | string? path `路径`, string? anchor = null `锚点` |
**SelectAll** | 全选 | void ||
**Copy** | 复制选中内容到剪贴板 | bool | bool keepFormat = true `是否保留 Markdown 格式` |
**GetSelectedText** | 获取选中文本（可保留 Markdown 格式） | string | bool keepFormat = true `是否保留 Markdown 格式` |
**ClearSelection** | 清除选区 | void ||
**SetImage** | 设置图片（供外部下载完成后回调，支持跨线程调用） | void | string url `图片原始地址`, byte[]? content `图片内容，null 表示加载失败` |
**ClearImageCache** | 清空图片缓存（随后按新的解析结果重新请求） | void ||
**ResolveUrl** | 将相对路径解析为可访问地址（基于 BasePath） | string | string url `相对地址` |
**ParseLink** | 解析链接（类型 / 解析后地址 / 锚点 / 目标文件） | MarkdownLinkEventArgs | string url `链接原始地址` |
**NormalizeAnchor** | 锚点标准化：URL 解码 + GitHub 风格 slug 化 | string | string? anchor `锚点` |

> `SetMarkdown` 另有单参数重载；`SetImage` 另有带 `contentType`（如 `image/png`、`image/svg+xml`）参数的重载，可自动识别普通图片流与 SVG 文本。

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**LinkClick** | 点击超链接时发生（控件内部不处理任何业务逻辑） | void | object? sender `事件源`, MarkdownLinkEventArgs e `Url 原始地址、ResolvedUrl 解析后地址、Kind 链接类型、Anchor 锚点、FilePath 目标文件、Handled 是否已处理` |
**ImageRequest** | 需要加载图片时发生（控件内部不实现任何下载逻辑） | void | object? sender `事件源`, MarkdownImageEventArgs e `Url 图片原始地址、Alt 替代文本` |

### 示例

```csharp
var markdown = new AntdUI.Markdown { Dock = DockStyle.Fill };
Controls.Add(markdown);

// 标题、粗体、代码块、表格与链接
string md = "# 标题\n\n这是一段 **粗体** 文本。\n\n```csharp\nConsole.WriteLine(\"Hello AntdUI\");\n```\n\n| 名称 | 值 |\n| --- | --- |\n| A | 1 |\n\n[AntdUI](https://github.com/AntdUI/AntdUI)";
markdown.Text = md;

// AI 逐字输出：Append 追加片段，结束后 CompleteStreaming 完成解析与布局校准
markdown.Clear();
markdown.Append("正在生成...");
markdown.Append("\n\n## 新段落");
markdown.CompleteStreaming();

// 图片由外部加载，下载完成后回填（支持普通图片流与 SVG 文本）
markdown.ImageRequest += (s, e) =>
{
	byte[]? bytes = DownloadImage(e.Url); // 外部自行下载
	markdown.SetImage(e.Url, bytes);
};
```
