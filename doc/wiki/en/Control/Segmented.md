[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Segmented
👚

> Display multiple options and allow users to select a single option.

- DefaultProperty：Items
- DefaultEvent：SelectIndexChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Set `SegmentedItemCollection` | SegmentedItemCollection | `null` |
**TextAlign** | Text horizontal alignment | [TAlignFlow](Enum.md#talignflow) | Center |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeHover** | Hover text color | Color`?` | `null` |
**ForeActive** | Activate text color | Color`?` | `null` |
||||
**BackColor** | Background color | Color`?` | `null` |
**OriginalBackColor** | Original background color | Color | Transparent |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**IconAlign** | Icon alignment direction | [TAlignMini](Enum.md#talignmini) | Top |
**IconGap** | Ratio of icon to text spacing | float | 0.2F |
**IconRatio** | Icon Scale | float`?` | `null` |
||||
**AutoSize** | Auto Size | bool | false |
**PauseLayout** | Pause Layout | bool | false |
**SelectIndex** | Select Index | int | -1 |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 0F |
**Radius** | Rounded corners | int | 6 |
**ItemBorderColor** | Item border color | Color`?` | `null` |
**ItemBorderWidth** | Item border width | float | 0F |
**Round** | Rounded corner style | bool | false |
||||
**Shadow** | Shadow | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowOpacity** | Shadow Transparency | float | 0.2F |
||||
**BarPadding** | Bar padding | int | 0 |
**BarPosition** | Bar position | [TAlignMini](Enum.md#talignmini) | None |
**BarSize** | Bar size | float | 3F |
**Gap** | Gap | int | 0 |
**ItemGap** | Item gap | float | 0.6F |
||||
**BarBg** | Show bar background | bool | false |
**BarColor** | Bar background color | Color`?` | `null` |
**BarRadius** | Bar rounded corners | int | 0 |
**Full** | Is it fully covered | bool | false |
**RightToLeft** | Reverse | RightToLeft | No |
**TooltipConfig** | Text prompt configuration | TooltipConfig`?` | `null` |
**Vertical** | Is it vertical | bool | false |
||||
|||
||||
|||
||||
|||
||||
|||
||||
|||
||||
|||
||||
|||

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
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**ForeColor** | Foreground color | Color`?` | `null` |
**BadgeFore** | Badge foreground color | Color`?` | `null`
||||
**BadgeBack** | Badge background color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconHover** | Icon hover state | Image`?` | `null`
**IconHoverSvg** | Icon hover state SVG | string`?` | `null`
**IconActive** | Icon activate | Image`?` | `null` |
**IconActiveSvg** | Icon activate SVG | string`?` | `null` |
||||
**Enabled** | Enable | bool | true |
**BadgeMode** | Badge mode (hollow out) | bool | false |
||||
**BadgeBorderColor** | Badge border color | Color`?` | `null`
**BadgeBorderWidth** | Badge border width | float`?` | `null`
||||
**BadgeAlign** | Badge align | [TAlignFrom](Enum.md#talignfrom) | TR |
**BadgeOffsetX** | Badge offset X | float | 0 |
**BadgeOffsetY** | Badge offset Y | float | 0 |
**BadgeSize** | Badge size | float | 0.6F |
**Reverse** | Whether reversed | bool | false
||||
**Badge** | Badge text | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |
**Tooltip** | Tooltip | string`?` | `null`
||||
|||||
||||
**🌏 LocalizationTooltip** | Tooltip | string`?` | `null`
||||
**🌏 LocalizationTooltip** | Tooltip | string`?` | `null` |