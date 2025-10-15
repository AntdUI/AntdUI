[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## TabHeader
👚

> A multi tag page header.

- DefaultProperty：SelectedIndex
- DefaultEvent：TabChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Radius** | Rounded corners | int | 6 |
**RadiusContent** | Content rounded corners | int | 4 |
**OffsetY** | Y offset | int | 0 |
**ForeColor** | Text color | Color`?` | `null` |
**ForeHover** | Hover text color | Color`?` | `null` |
**ForeActive** | Activate text color | Color`?` | `null` |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**BorderWidth** | Border width | float | 0F |
**BorderColor** | Border color | Color`?` | `null` |
||||
**TabIconRatio** | Icon ratio | float | 1.34F |
**TabCloseRatio** | Close button ratio | float | 1.408F |
**TabCloseIconRatio** | Turn off icon proportion | float | 0.74F |
**TabGapRatio** | Margin ratio | float | 0.6F |
**TabIconGapRatio** | Ratio of icon to text spacing | float | 0.74F |
**TabAddIconRatio** | Proportion of newly added button icons | float | 1.18F |
**TabAddGapRatio** | Add button margin ratio | float | 0.148F |
||||
**DragSort** | Drag and drop sorting | bool | false |
**ShowAdd** | Display Add | bool | false |
||||
**Items** | Data `TagTabItem[]` | [TagTabItem[]](#tagtabitem) | [] |
**SelectedIndex** | Select Index | int | 0 |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**AddClick** | Occurred when clicking the add button | void ||
**TabChanged** | Occurred when the SelectedIndex property value is changed | void | [TagTabItem](#tagtabitem) Value, int Index |
**TabClosing** | Occurred before Tab closes | void | [TagTabItem](#tagtabitem) Value, int Index |


### Data

#### TagTabItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Visible** | Is it displayed | bool | true |
**Enabled** | Enable | bool | true |
**Loading** | Loading | bool | false |
**Tag** | User defined data | object`?` | `null` |