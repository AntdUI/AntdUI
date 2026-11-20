[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Markdown

Markdown rendering control 📝

> Renders Markdown text with headings, tables, quotes, lists, code (including Diff) highlighting, rules, bold/strikethrough, hyperlinks, images and GitHub-style alerts. Parsing and layout are optimized incrementally for AI streaming output.
>
> The control does not download images itself: subscribe to `ImageRequest` to receive the URL, then call `SetImage` when loading completes. Currently a preview version (not fully tested).

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Markdown text | string`?` | `null` |
||||
**Fore** | Text color | Color`?` | `null` |
**CodeFont** | Code font | Font`?` | `null` |
**CodeFore** | Code text color | Color`?` | `null` |
**HeadingFore** | Heading color | Color`?` | `null` |
**InlineCodeFore** | Inline code text color | Color`?` | `null` |
**LinkFore** | Hyperlink color | Color`?` | `null` |
**QuoteFore** | Quote text color | Color`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**CodeBack** | Code block background color | Color`?` | `null` |
**InlineCodeBack** | Inline code background color | Color`?` | `null` |
**TableHeaderBack** | Table header background color | Color`?` | `null` |
||||
**AlertCautionIcon** | Caution block icon (icon name or SVG code) | string`?` | "StopOutlined" |
**AlertImportantIcon** | Important block icon (icon name or SVG code) | string`?` | "InfoOutlined" |
**AlertNoteIcon** | Note block icon (icon name or SVG code) | string`?` | "InfoCircleOutlined" |
**AlertTipIcon** | Tip block icon (icon name or SVG code) | string`?` | "BulbOutlined" |
**AlertWarningIcon** | Warning block icon (icon name or SVG code) | string`?` | "WarningOutlined" |
||||
**AutoScroll** | Auto scroll to bottom when streaming appends | bool | true |
**Selectable** | Whether to allow mouse selection and copy | bool | true |
**SelectionColor** | Selection background color | Color`?` | `null` |
||||
**Radius** | Rounded corners | int | 6 |
**TableBorder** | Table Border Color | Color`?` | `null` |
||||
**CodePadding** | Code padding (relative to line height) | float | 0.62F |
**LinkUnderline** | Hyperlinks always show underline | bool | false |
**ListIndent** | List indent | int | 22 |
**QuoteIndent** | Quote indent | int | 16 |
**StreamInterval** | Stream refresh interval (ms) | int | 30 |
**TableCellPadding** | Table cell left/right padding (0 = auto by font ratio 6px/13px) | int | 0 |
**TableStripe** | Table stripe color | Color`?` | `null` |
||||
**AlertCautionColor** | Caution block color | Color`?` | `null` |
**AlertImportantColor** | Important block color | Color`?` | `null` |
**AlertNoteColor** | Note block color | Color`?` | `null` |
**AlertTipColor** | Tip block color | Color`?` | `null` |
**AlertWarningColor** | Warning block color | Color`?` | `null` |
**BasePath** | Base path for relative paths (local directory or URL), used to resolve relative image and link addresses | string`?` | `null` |
**BlockSpacing** | Block spacing (relative to line height) | float | 0.62F |
**CodeComment** | Code comment color | Color`?` | `null` |
**CodeKeyword** | Code keyword color | Color`?` | `null` |
**CodeNumber** | Code number color | Color`?` | `null` |
**CodeString** | Code string color | Color`?` | `null` |
**CodeType** | Code type color | Color`?` | `null` |
**LinkNavigation** | Enable built-in navigation (# anchor / relative Markdown file) | bool | true |
**QuoteBar** | Quote bar color | Color`?` | `null` |
**RuleColor** | Rule color | Color`?` | `null` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SetMarkdown** | Set Markdown text (parse and layout immediately) | void | string? markdown `Markdown text`, bool resetScroll `Whether to reset the scrollbar` |
**Append** | Append Markdown text in streaming mode (for AI character-by-character output) | void | string? text `Text` |
**AppendLine** | Append one line of Markdown text in streaming mode | void | string? text `Text` |
**CompleteStreaming** | Finish streaming output (fully parse and recalibrate the layout immediately) | void ||
**Clear** | Clear content | void ||
**ScrollToTop** | Scroll to the top | void ||
**ScrollToBottom** | Scroll to the bottom | void ||
**ScrollToAnchor** | Scroll to the specified anchor (# heading); deferred until layout is ready if necessary | bool | string? anchor `Anchor (may include #)` |
**NavigateTo** | Navigate to a link (# anchor / relative Markdown file); returns whether it was handled | bool | string? url `URL` |
**LoadDocument** | Load a Markdown file in the same control (BasePath switches to the file's directory) | bool | string? path `Path`, string? anchor = null `Anchor` |
**SelectAll** | Select all | void ||
**Copy** | Copy the selection to the clipboard | bool | bool keepFormat = true `Whether to keep Markdown formatting` |
**GetSelectedText** | Get the selected text (Markdown formatting can be preserved) | string | bool keepFormat = true `Whether to keep Markdown formatting` |
**ClearSelection** | Clear the selection | void ||
**SetImage** | Set an image (call back after an external download completes; cross-thread safe) | void | string url `Original image URL`, byte[]? content `Image content, null means loading failed` |
**ClearImageCache** | Clear the image cache (images are requested again from the new parse result) | void ||
**ResolveUrl** | Resolve a relative path to an accessible URL (based on BasePath) | string | string url `Relative URL` |
**ParseLink** | Parse a link (kind / resolved URL / anchor / target file) | MarkdownLinkEventArgs | string url `Original link URL` |
**NormalizeAnchor** | Normalize an anchor: URL decoding + GitHub-style slug | string | string? anchor `Anchor` |

> `SetMarkdown` also has a single-parameter overload. `SetImage` also has an overload taking `contentType` (e.g. `image/png`, `image/svg+xml`), which automatically detects ordinary image streams and SVG text.

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LinkClick** | Occurs when a hyperlink is clicked (no business logic is handled inside the control) | void | object? sender `Event source`, MarkdownLinkEventArgs e `Url original URL, ResolvedUrl resolved URL, Kind link kind, Anchor anchor, FilePath target file, Handled whether handled` |
**ImageRequest** | Occurs when an image needs to be loaded (no download logic is implemented inside the control) | void | object? sender `Event source`, MarkdownImageEventArgs e `Url image URL, Alt alternative text` |

### Example

```csharp
var markdown = new AntdUI.Markdown { Dock = DockStyle.Fill };
Controls.Add(markdown);

// Heading, bold, code block, table and link
string md = "# Heading\n\nThis is **bold** text.\n\n```csharp\nConsole.WriteLine(\"Hello AntdUI\");\n```\n\n| Name | Value |\n| --- | --- |\n| A | 1 |\n\n[AntdUI](https://github.com/AntdUI/AntdUI)";
markdown.Text = md;

// AI character-by-character output: Append fragments, then CompleteStreaming to finish parsing and layout
markdown.Clear();
markdown.Append("Generating...");
markdown.Append("\n\n## New section");
markdown.CompleteStreaming();

// Images are loaded externally and fed back when done (ordinary image streams and SVG text are supported)
markdown.ImageRequest += (s, e) =>
{
	byte[]? bytes = DownloadImage(e.Url); // download outside the control
	markdown.SetImage(e.Url, bytes);
};
```
