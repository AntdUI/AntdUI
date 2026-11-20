[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## TabHeader
👚

> A multi tag page header.

- DefaultProperty：SelectedIndex
- DefaultEvent：TabChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data `TagTabItem[]` | [TagTabItem](#tagtabitem) | [] |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeHover** | Hover text color | Color`?` | `null` |
**ForeActive** | Activate text color | Color`?` | `null` |
||||
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**AddIconSvg** | New button Svg icon (default PlusOutlined) | string`?` | `null` |
**TabAddIconRatio** | New button icon ratio | float | 1.18F |
**TabCloseIconRatio** | Close icon ratio | float | 0.74F |
**TabIconGapRatio** | Ratio of icon to text spacing | float | 0.74F |
**TabIconRatio** | Icon ratio | float | 1.34F |
||||
**SelectedIndex** | Selected index | int | 0 |
**DragSort** | Drag and drop sorting | bool | false |
**SelectedItem** | Selected item | [TagTabItem](#tagtabitem)`?` | `null` |
**ShowAdd** | Whether to show add | bool | false |
**TabCloseRatio** | Close button ratio | float | 1.408F |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 0F |
**Radius** | Rounded corners | int | 6 |
**RadiusContent** | Content rounded corners | int | 4 |
||||
**LeftGap** | Left margin | int | 0 |
**OffsetY** | Y offset | int | 0 |
**RightGap** | Right margin | int | 0 |
**TabAddGapRatio** | Add button margin ratio | float | 0.148F |
**TabGapRatio** | Margin ratio | float | 0.6F |

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
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**BadgeFore** | Badge foreground color | Color`?` | `null` |
||||
**BadgeBack** | Badge background color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**LoadingIcon** | Loading icon | Image`?` | `null`
||||
**Loading** | Loading state | bool | false |
**LoadingSvg** | Loading icon SVG | string`?` | `null`
||||
**Enabled** | Enabled state | bool | true |
**Visible** | Whether to display | bool | true |
**BadgeMode** | Badge mode (hollow) | bool | false |
**ShowClose** | Whether to show close | bool | true |
||||
**BadgeBorderColor** | Badge border color | Color`?` | `null` |
**BadgeBorderWidth** | Badge border width | float`?` | `null`
||||
**BadgeAlign** | Badge alignment | [TAlign](Enum.md#talign) | Right |
**BadgeOffsetX** | Badge offset X | int | 1
**BadgeOffsetY** | Badge offset Y | int | 1
**BadgeSize** | Badge size ratio | float | 0.6F |
||||
**Badge** | Badge content | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |