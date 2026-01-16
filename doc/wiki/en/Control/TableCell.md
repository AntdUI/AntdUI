[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

[Return to Table](Table.md)

## ICell

> Rich Cells

#### CellText

> Text

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fore** | Font color | Color`?` ||
**Back** | Background color | Color`?` ||
**Font** | Font | Font`?` ||
||||
**IconRatio** | Icon Scale | float | 0.7F |
**Prefix** | Prefix | Image`?` ||
**PrefixSvg** | Prefix SVG | string`?` ||
**Suffix** | Suffix | Image`?` ||
**SuffixSvg** | Suffix SVG | string`?` ||
||||
**Text** | Text | string`?` ||

#### CellBadge

> Badge

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fore** | Font color | Color`?` ||
**Fill** | Colour | Color`?` ||
||||
**State** | State | [TState](Enum.md#tstate) | Default |
**Text** | Text | string`?` |

#### CellTag

> Tag

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fore** | Font color | Color`?` ||
**Back** | Background color | Color`?` ||
**BorderWidth** | Border width | float |1F|
||||
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
**Text** | Text | string`?` ||

#### CellImage

> Image

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**BorderColor** | Border color | Color`?` ||
**BorderWidth** | Border width | float |0F|
**Radius** | Rounded corners | int |6|
||||
**Round** | Rounded corner style | bool |false|
**Size** | Custom size | Size`?` ||
||||
**Image** | Image | Image`?` | `null` |
**ImageSvg** | Image SVG | string`?` | `null` |
**FillSvg** | SVG Fill Color | Color`?` ||
**ImageFit** | Image layout | [TFit](Enum.md#tfit) | Fill |
||||
**Tooltip** | Text Tips | string`?` ||

#### CellButton

> Button，Inherited from [CellLink](#celllink)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fore** | Font color | Color`?` ||
**Back** | Background color | Color`?` ||
**BackHover** | Hover background color | Color`?` ||
**BackActive** | Activate background color | Color`?` ||
||||
**DefaultBack** | Default type background color | Color`?` ||
**DefaultBorderColor** | Default type border color | Color`?` ||
||||
**Radius** | Rounded corners | int |6|
**BorderWidth** | Border width | float |0F|
||||
**IconRatio** | Icon Scale | float | 0.7F |
**IconGap** | Ratio of icon to text spacing | float | 0.25F |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconHover** | Hover icon | Image`?` | `null` |
**IconHoverSvg** | Hover icon SVG | string`?` | `null` |
**IconHoverAnimation** | Hover icon animation duration | int | 200 |
**IconPosition** | Location of button icon components | [TAlignMini](Enum.md#talignmini) | Left |
||||
**Shape** | Shape | [TShape](Enum.md#tshape) | Default |
**Ghost** | Ghost attribute `Transparent button background` | bool | false |
**ShowArrow** | Display arrows | bool | false |
**IsLink** | Arrow Link Style | bool | false |
||||
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
**Text** | Text | string`?` ||

#### CellLink

> Link

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Id** | ID | string ||
**Enabled** | Enable | bool |true|
||||
**Text** | Text | string`?` ||
**TextAlign** | Text position | ContentAlignment | MiddleCenter |
||||
**Tooltip** | Text Tips | string`?` ||

#### CellProgress

> Progress

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Back** | Background color | Color`?` ||
**Fill** | Progress bar color | Color`?` ||
||||
**Radius** | Rounded corners | int |6|
**Shape** | Shape | [TShape](Enum.md#tshape) | Default |
||||
**Value** | Progress `0.0-1.0` | float |0F|

#### CellDivider

> Divider

#### CellCheckbox

> Checkbox

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fore** | Font color | Color`?` ||
**Font** | Font | Font`?` ||
**Fill** | Fill color | Color`?` ||
**Enabled** | Enabled | bool | true |
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**Text** | Text | string`?` ||
🌏 **LocalizationText** | International Text | string`?` ||
||||
**Checked** | Checked state | bool | false |
**CheckState** | Checked state | CheckState | Unchecked |

#### CellRadio

> Radio

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fore** | Font color | Color`?` ||
**Font** | Font | Font`?` ||
**Fill** | Fill color | Color`?` ||
**Enabled** | Enabled | bool | true |
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**Text** | Text | string`?` ||
🌏 **LocalizationText** | International Text | string`?` ||
||||
**Checked** | Checked state | bool | false |

#### CellSwitch

> Switch

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fore** | Font color | Color`?` ||
**Enabled** | Enabled | bool | true |
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**Text** | Text | string`?` ||
🌏 **LocalizationText** | International Text | string`?` ||
||||
**Checked** | Checked state | bool | false |