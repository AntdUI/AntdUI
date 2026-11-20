[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## UploadDragger
👚

> Drag the file to a specific area for uploading. Alternatively, by selecting upload.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextDesc** | Text description | string`?` | `null` |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `InboxOutlined` |
**IconRatio** | Icon Scale | float | 1.92F |
||||
**Multiselect** | Multiple files | bool | true |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**Radius** | Rounded corners | int | 8 |
**BorderStyle** | Border Style | DashStyle | Dash |
||||
**ClickHand** | Click to upload | bool | true |
**Filter** | Filename filter string | string`?` | `null` Refer to OpenFileDialog format |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ManualSelection** | Manually trigger file selection | void | |
**SetFilter** | Set commonly used filters | void | FilterType filterType [Flags] |


### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DragChanged** | Occurrence when dragging files | void | string[] files `file list` |