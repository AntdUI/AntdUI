[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

[Return to Table](Table.md)

## ICell

> Rich Cells

#### CellText

> Text

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` ||
||||
**Font** | Font | Font`?` ||
**Fore** | Font color | Color`?` ||
||||
**Back** | Background color | Color`?` ||
||||
**IconRatio** | Icon Scale | float | 0.7F |
||||
**Prefix** | Prefix | Image`?` ||
**PrefixSvg** | Prefix SVG | string`?` ||
**Suffix** | Suffix | Image`?` ||
**SuffixSvg** | Suffix SVG | string`?` ||

#### CellBadge

> Badge

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` |
||||
**Fore** | Font color | Color`?` ||
||||
**Fill** | Colour | Color`?` ||
||||
**State** | State | [TState](Enum.md#tstate) | Default |

#### CellTag

> Tag

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` ||
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
||||
**Fore** | Font color | Color`?` ||
||||
**Back** | Background color | Color`?` ||
||||
**BorderWidth** | Border width | float |1F|

#### CellImage

> Image

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Image** | Image | Image`?` | `null` |
**ImageFit** | Image layout | [TFit](Enum.md#tfit) | Fill |
**ImageSvg** | Image SVG | string`?` | `null` |
||||
**BorderColor** | Border color | Color`?` ||
**BorderWidth** | Border width | float |0F|
**Radius** | Rounded corners | int |6|
**Round** | Rounded corner style | bool |false|
||||
**Size** | Custom size | Size`?` ||
||||
**FillSvg** | SVG Fill Color | Color`?` ||
**Tooltip** | Text Tips | string`?` ||

#### CellButton

> Button，Inherited from [CellLink](#celllink)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` ||
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
||||
**Fore** | Font color | Color`?` ||
||||
**Back** | Background color | Color`?` ||
**DefaultBack** | Default type background color | Color`?` ||
**BackHover** | Hover background color | Color`?` ||
**BackActive** | Activate background color | Color`?` ||
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconGap** | Ratio of icon to text spacing | float | 0.25F |
**IconPosition** | Location of button icon components | [TAlignMini](Enum.md#talignmini) | Left |
**IconRatio** | Icon Scale | float | 0.7F |
**IconHover** | Hover icon | Image`?` | `null` |
**IconHoverAnimation** | Hover icon animation duration | int | 200 |
**IconHoverSvg** | Hover icon SVG | string`?` | `null` |
||||
**Ghost** | Ghost attribute `Transparent button background` | bool | false |
**IsLink** | Arrow Link Style | bool | false |
**ShowArrow** | Display arrows | bool | false |
||||
**BorderWidth** | Border width | float |0F|
**Radius** | Rounded corners | int |6|
**Shape** | Shape | [TShape](Enum.md#tshape) | Default |
**DefaultBorderColor** | Default type border color | Color`?` ||

#### CellLink

> Link

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Id** | ID | string ||
**Text** | Text | string`?` ||
**TextAlign** | Text position | ContentAlignment | MiddleCenter |
||||
**Enabled** | Enable | bool |true|
||||
**Tooltip** | Text Tips | string`?` ||

#### CellProgress

> Progress

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Progress `0.0-1.0` | float |0F|
||||
**Back** | Background color | Color`?` ||
**Fill** | Progress bar color | Color`?` ||
||||
**Radius** | Rounded corners | int |6|
**Shape** | Shape | [TShape](Enum.md#tshape) | Default |

#### CellDivider

> Divider

#### CellCheckbox

> Checkbox

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` ||
🌏 **LocalizationText** | International Text | string`?` ||
||||
**Font** | Font | Font`?` ||
**Fore** | Font color | Color`?` ||
||||
**Fill** | Fill color | Color`?` ||
||||
**Checked** | Checked state | bool | false |
**Enabled** | Enabled | bool | true |
**AutoCheck** | Click to automatically change the selected status | bool | true |
**CheckState** | Checked state | CheckState | Unchecked |

#### CellRadio

> Radio

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` ||
🌏 **LocalizationText** | International Text | string`?` ||
||||
**Font** | Font | Font`?` ||
**Fore** | Font color | Color`?` ||
||||
**Fill** | Fill color | Color`?` ||
||||
**Checked** | Checked state | bool | false |
**Enabled** | Enabled | bool | true |
**AutoCheck** | Click to automatically change the selected status | bool | true |

#### CellSwitch

> Switch

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` ||
🌏 **LocalizationText** | International Text | string`?` ||
||||
**Fore** | Font color | Color`?` ||
||||
**Checked** | Checked state | bool | false |
**Enabled** | Enabled | bool | true |
**AutoCheck** | Click to automatically change the selected status | bool | true |
**CheckedText** | Text displayed when selected | string`?` | `null` |