[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Segmented
👚

> Display multiple options and allow users to select a single option.

- DefaultProperty：Items
- DefaultEvent：SelectIndexChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**AutoSize** | Auto Size | bool | false |
||||
**Full** | Is it fully covered | bool | false |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeHover** | Hover text color | Color`?` | `null` |
**ForeActive** | Activate text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**Gap** | Gap | int | 0 |
**Vertical** | Is it vertical | bool | false |
**IconAlign** | Icon alignment direction | [TAlignMini](Enum.md#talignmini) | Top |
**IconRatio** | Icon Scale | float`?` | `null` |
**IconGap** | Ratio of icon to text spacing | float | 0.2F |
||||
**BarPosition** | Bar position | [TAlignMini](Enum.md#talignmini) | None |
**BarSize** | Bar size | float | 3F |
**BarPadding** | Bar padding | int | 0 |
**BarRadius** | Bar rounded corners | int | 0 |
||||
**Items** | Set `SegmentedItem[]` | [SegmentedItem[]](#segmenteditem) | [] |
**SelectIndex** | Select Index | int | 0 |
||||
**PauseLayout** | Pause Layout | bool | false |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndexChanged** | Occurred when the SelectIndex property value is changed | void | int index |
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