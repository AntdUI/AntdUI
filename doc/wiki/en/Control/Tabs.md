[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tabs
👚

> Tabs make it easy to explore and switch between different views.

- DefaultProperty：Pages
- DefaultEvent：SelectedIndexChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
||||
**Fill** | Color | Color`?` | `null` |
**FillHover** | Hover color | Color`?` | `null` |
**FillActive** | Active color | Color`?` | `null` |
||||
**Alignment** | Alignment | TabAlignment | Top |
**Centered** | Centered display of tabs | bool | false |
**TextCenter** | Text centered alignment (only effective in Left/Right direction) | bool | false |
||||
**TypExceed** | Exceeding UI type | [TabTypExceed](Enum.md#tabtypexceed) | Button |
**EnableSwitch** | Switch enable | bool | true |
**EnablePageScrolling** | Mouse wheel switch focus page enable | bool | true |
**ScrollBack** | Scroll bar color | Color`?` | `null` |
**ScrollBackHover** | Scroll bar hover color | Color`?` | `null` |
**ScrollFore** | Scroll bar text color | Color`?` | `null` |
**ScrollForeHover** | Scroll bar hover text color | Color`?` | `null` |
||||
**Gap** | Gap | int | 8 |
**IconRatio** | Icon ratio | float | 0.7F |
**IconGap** | Icon to text spacing ratio | float | 0.25F |
**ItemSize** | Custom item size | int? | `null` |
||||
**Type** | Type | [TabType](Enum.md#tabtype) | Line |
**Style** | Style type | [IStyle](#istyle) | `Nonnull` |
**Rotate** | Rotate (used for vertical display in Left/Right) | [TRotate](Enum.md#trotate) | None |
**DragOrder** | Drag order | bool | false |
||||
**TabMenuVisible** | Whether to display the header | bool | true |
||||
**Pages** | Collection `TabCollection` | [TabCollection](#tabpage) | [] |
**SelectedIndex** | Selected index | int | 0 |
**SelectedTab** | Current item | [TabPage](#tabpage)`?` | `null` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectTab** | Select tab | void | string tabPageName |
**SelectTab** | Select tab | void | [TabPage](#tabpage) tabPage |
**SelectTab** | Select tab | void | int index |
**ContainsTabPage** | Check if mouse is on tab | [TabPage](#tabpage)`?` | int x, int y |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectedIndexChanged** | Occurred when the SelectedIndex property value is changed | void | int index |
**SelectedIndexChanging** | Occurred before the SelectedIndex property value is changed | bool | int index |
**ClosingPage** | Occurred before closing the page | bool | [TabPage](#tabpage) page |
**TabClick** | Occurred when clicking on a tab | bool | [TabPage](#tabpage) page, int index |

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