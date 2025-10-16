[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## UploadDragger
👚

> Drag the file to a specific area for uploading. Alternatively, by selecting upload.

- DefaultProperty：Text
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextDesc** | Text description | string`?` | `null` |
**Radius** | Rounded corners | int | 8 |
||||
**ForeColor** | Text color | Color`?` | `null` ||
**Back** | Background color | Color`?` | `null` |
||||
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**IconRatio** | Icon Scale | float | 1.92F |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
||||
**BorderWidth** | Border width | float | 1F |
**BorderColor** | Border color | Color`?` | `null` |
**BorderStyle** | Border Style | DashStyle | Solid |
||||
**ClickHand** 🔴 | Click to upload | bool | true |
**Multiselect** 🔴 | Multiple files | bool | true |
**Filter** 🔴 | Filename filter string | string`?` | `null` Refer to OpenFileDialog format |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ManualSelection** | Manually trigger file selection | void | |
**SetFilter** | Set commonly used filters | void | FilterType filterType [Flags] |


### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DragChanged** | Occurrence when dragging files | void | string[] files `file list` |