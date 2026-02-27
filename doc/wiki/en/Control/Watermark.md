[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Watermark
👚

> Add watermark to page or control, support text and image watermark.

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Opacity** | Watermark opacity | float | 0.15F |
**Rotate** | Rotation angle | int | -22 |
**Width** | Watermark width | int | 200 |
**Height** | Watermark height | int | 100 |
**Gap** | Watermark gap [x, y] | int[] | [100, 100] |
**Offset** | Watermark offset [x, y] | int[] | [50, 50] |
**IsScreen** | Screen watermark | bool | false |
**SubFontSize** | Sub content font size ratio | float | 0.8F |
**Font** | Font | Font`?` | `null` |
**ForeColor** | Font color | Color`?` | `null` |
**TextAlign** | Text alignment | FormatFlags | Center |
**Enabled** | Enabled | bool | true |
**Content** | Watermark content | string | `null` |
**SubContent** | Sub watermark content | string`?` | `null` |
**Image** | Watermark image | Image`?` | `null` |
**ImageSvg** | Watermark image SVG | string`?` | `null` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**open** | Open watermark | Form`?` | Control `target`, string `content`, string `subContent` |
**open** | Open watermark | Form`?` | Config `config` |

### Example

```csharp
// Simplified usage
Watermark.open(this, "AntdUI Watermark", "2024-01-01");

// Full configuration
var config = new Watermark.Config(this, "AntdUI")
	.SetSub("Powered by AntdUI")
	.SetOpacity(0.2f)
	.SetRotate(-30)
	.SetGap(150, 150);
Watermark.open(config);
```