[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Segmented
👚

> Display multiple options and allow users to select a single option.

- DefaultProperty：Items
- DefaultEvent：SelectIndexChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**AutoSize** | Auto Size | bool | false |
||||
**Full** | Is it fully covered | bool | false |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
|||
**ForeColor** | Text color | Color`?` | `null` |
**ForeHover** | Hover text color | Color`?` | `null` |
**ForeActive** | Activate text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
|||
**Gap** | Gap | int | 0 |
**ItemGap** | Item gap | float | 0.6F |
**Vertical** | Is it vertical | bool | false |
**IconAlign** | Icon alignment direction | [TAlignMini](Enum.md#talignmini) | Top |
**IconRatio** | Icon Scale | float`?` | `null` |
**IconGap** | Ratio of icon to text spacing | float | 0.2F |
|||
**BarPosition** | Bar position | [TAlignMini](Enum.md#talignmini) | None |
**BarBg** | Show bar background | bool | false |
**BarColor** | Bar background color | Color`?` | `null` |
**BarSize** | Bar size | float | 3F |
**BarPadding** | Bar padding | int | 0 |
**BarRadius** | Bar rounded corners | int | 0 |
|||
**BorderWidth** | Border width | float | 0F |
**BorderColor** | Border color | Color`?` | `null` |
**ItemBorderWidth** | Item border width | float | 0F |
**ItemBorderColor** | Item border color | Color`?` | `null` |
|||
**RightToLeft** | Reverse | RightToLeft | No |
**TooltipConfig** | Text prompt configuration | TooltipConfig`?` | `null` |
|||
**Items** | Set `SegmentedItemCollection` | SegmentedItemCollection | `null` |
**SelectIndex** | Select Index | int | -1 |
|||
**PauseLayout** | Pause Layout | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndexChanged** | Occurred when the SelectIndex property value is changed | void | int index |
**SelectIndexChanging** | Occurred before the SelectIndex property value is changed | bool | int index |
**ItemClick** | Occurrence upon item click | void | MouseEventArgs e, SegmentedItem value |

### Data

#### SegmentedItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconActive** | Icon activate | Image`?` | `null` |
**IconActiveSvg** | Icon activate SVG | string`?` | `null` |
|||||
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Enabled** | Enable | bool | true |
**Tag** | User defined data | object`?` | `null` |
||||
**Badge** | Badge text | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |
**BadgeAlign** | Badge align | [TAlignFrom](Enum.md#talignfrom) | TR |
**BadgeSize** | Badge size | float | 0.6F |
**BadgeMode** | Badge mode (hollow out) | bool | false |
**BadgeOffsetX** | Badge offset X | float | 0 |
**BadgeOffsetY** | Badge offset Y | float | 0 |
**BadgeBack** | Badge background color | Color`?` | `null` |