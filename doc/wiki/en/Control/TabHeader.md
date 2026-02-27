[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## TabHeader
👚

> A multi tag page header.

- DefaultProperty：SelectedIndex
- DefaultEvent：TabChanged

### Properties

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
**TabCloseIconRatio** | Close icon ratio | float | 0.74F |
**TabGapRatio** | Margin ratio | float | 0.6F |
**TabIconGapRatio** | Ratio of icon to text spacing | float | 0.74F |
**TabAddIconRatio** | New button icon ratio | float | 1.18F |
**TabAddGapRatio** | Add button margin ratio | float | 0.148F |
**LeftGap** | Left margin | int | 0 |
**RightGap** | Right margin | int | 0 |
||||
**DragSort** | Drag and drop sorting | bool | false |
**ShowAdd** | Whether to show add | bool | false |
**AddIconSvg** | New button Svg icon (default PlusOutlined) | string`?` | `null` |
||||
**Items** | Data `TagTabItem[]` | [TagTabItem](#tagtabitem) | [] |
**SelectedIndex** | Selected index | int | 0 |
**SelectedItem** | Selected item | [TagTabItem](#tagtabitem)`?` | `null` |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**AddClick** | Occurred when clicking the add button | void ||
**TabChanged** | Occurred when the SelectedIndex property value is changed | void | [TagTabItem](#tagtabitem) Value, int Index |
**TabSelectedItemChanged** | Item selected event | void | [TagTabItem](#tagtabitem) Value, int Index |
**TabClosing** | Occurred before Tab closes | void | [TagTabItem](#tagtabitem) Value, int Index |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**AddTab** | Add tab | void | [TagTabItem](#tagtabitem) item, bool select `Whether to select` |
**AddTab** | Add tab | void | string text, Image`?` icon |
**AddTab** | Add tab | void | string text, string`?` iconsvg |
**InsertTab** | Insert tab | void | int index, [TagTabItem](#tagtabitem) item, bool select `Whether to select` |
**InsertTab** | Insert tab | void | int index, string text, Image`?` icon |
**RemoveTab** | Remove tab | void | int index |
**Select** | Select tab | void | [TagTabItem](#tagtabitem) item |
**HitTest** | Hit test | [TagTabItem](#tagtabitem)`?` | int x, int y, out int i |

### Data

#### TagTabItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Visible** | Whether to display | bool | true |
**Enabled** | Enabled state | bool | true |
**Loading** | Loading state | bool | false |
**ShowClose** | Whether to show close | bool | true |
**Tag** | User defined data | object`?` | `null` |
||||
**Badge** | Badge content | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |
**BadgeAlign** | Badge alignment | [TAlign](Enum.md#talign) | Right |
**BadgeSize** | Badge size ratio | float | 0.6F |
**BadgeMode** | Badge mode (hollow) | bool | false |
**BadgeFore** | Badge foreground color | Color`?` | `null` |
**BadgeBack** | Badge background color | Color`?` | `null` |
**BadgeBorderColor** | Badge border color | Color`?` | `null` |