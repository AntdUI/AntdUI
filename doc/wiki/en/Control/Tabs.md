[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tabs
👚

> Tabs make it easy to explore and switch between different views.

- DefaultProperty：TabPages
- DefaultEvent：SelectedIndexChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
||||
**Fill** | Colour | Color`?` | `null` |
**FillHover** | Hover color | Color`?` | `null` |
**FillActive** | Activate color | Color`?` | `null` |
||||
**Alignment** | Alignment | TabAlignment |Top|
**Centered** | Centered display of tags | bool | false |
||||
**TypExceed** | Exceeding UI type | [TabTypExceed](Enum.md#tabtypexceed) | Button |
**ScrollBack** | Scroll bar color | Color`?` | `null` |
**ScrollBackHover** | Scroll bar hover color | Color`?` | `null` |
**ScrollFore** | Scroll bar text color | Color`?` | `null` |
**ScrollForeHover** | Scroll bar hovering text color | Color`?` | `null` |
||||
**Gap** | Gap | int | 8 |
**IconRatio** | Icon Scale | float | 0.7F |
**ItemSize** | Custom item size | int? | `null` |
||||
**Type** | Type | [TabType](Enum.md#tabtype) | Line |
**Style** | Style type | [IStyle](#istyle) | `Nonnull` |
||||
**TabMenuVisible** | Whether to display the head | bool | true |
||||
**Pages** | Set `TabCollection` | [TabCollection](#tabpage) | [] |
**SelectedIndex** | Select Index | int | 0 |
**SelectedTab** | Current item | [TabPage](#tabpage)`?` |`null`|

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectTab** | Selected items | void | string tabPageName |
**SelectTab** | Selected items | void | [TabPage](#tabpage) tabPage |
**SelectTab** | Selected items | void | int index |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectedIndexChanged** | Occurred when the SelectedIndex property value is changed | void | int index |
**ClosingPage** | Occurred before closing the page | bool | [TabPage](#tabpage) page |

### IStyle

#### StyleLine

> Line style

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Size** | Bar size | int | 3 |
**Padding** | Bar padding | int | 8 |
**Radius** | Bar rounded corners | int | 0 |
**BackSize** | Bar background size | int | 1 |
||||
**Back** | Bar background | Color`?` | `null` |

#### StyleCard

> Card style

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Radius** | Card rounded corners | int | 6 |
**Border** | Border size | int | 1 |
**Gap** | Card spacing | int | 2 |
**Closable** | Closable | bool | false |
||||
**Fill** | Card color | Color`?` | `null` |
**FillHover** | Card hover color | Color`?` | `null` |
**FillActive** | Card activate color | Color`?` | `null` |
**BorderColor** | Card border color | Color`?` | `null` |
**BorderActive** | Card border activate color | Color`?` | `null` |



### Data

#### TabPage

> Inherited from [ScrollableControl](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Scrolling/ScrollableControl.cs)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
||||
**Badge** | Badge text | string`?` | `null` |
**BadgeSize** | Badge size | float | 0.6F |
**BadgeBack** | Badge background color | Color`?` | `null` |
**BadgeOffsetX** | Badge offset X | int | 0 |
**BadgeOffsetY** | Badge offset Y | int | 0 |
||||
**Text** | Display Text | string ||
**Visible** | Is it displayed | bool | true |
**ReadOnly** | Read only | bool | false |