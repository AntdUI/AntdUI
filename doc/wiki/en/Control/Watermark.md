[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Watermark
👚

> Add watermark to page or control, support text and image watermark.

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Opacity** | Watermark opacity | float | 0.15F |
**Rotate** | Rotation angle | int | -22 |
**Width** | Watermark width | int | 200 |
**Height** | Watermark height | int | 100 |
**Gap** | Watermark gap | Size | 0, 0 |
**Offset** | Watermark offset | Point | 0, 0 |
**AutoResize** | Auto resize | bool | true |
**Monitor** | Screen watermark | bool | false |
**SubFontSizeRatio** | Sub content font size ratio | float | 0.8F |
**Font** | Font | Font`?` | `null` |
**ForeColor** | Font color | Color | Color.FromArgb(255, 0, 0, 0) |
**SubFont** | Sub content font | Font`?` | `null` |
**SubForeColor** | Sub content font color | Color`?` | `null` |
**Align** | Alignment | ContentAlignment | MiddleCenter |
**Content** | Watermark content | string | `null` |
**SubContent** | Sub watermark content | string`?` | `null` |
**Image** | Watermark image | Image`?` | `null` |
**Svg** | SVG watermark | string`?` | `null` |
**SvgColor** | SVG color | Color`?` | `null` |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**open** | Open watermark | LayeredFormWatermark | Control `target`, string `content`, string `subContent` |
**open** | Open watermark | LayeredFormWatermark | Config `config` |

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