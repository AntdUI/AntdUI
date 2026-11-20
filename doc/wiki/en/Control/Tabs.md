[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tabs
👚

> Tabs make it easy to explore and switch between different views.

- DefaultProperty：Pages
- DefaultEvent：SelectedIndexChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Type** | Type | [TabType](Enum.md#tabtype) | Line |
**TextCenter** | Text centered alignment (only effective in Left/Right direction) | bool | false |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ScrollFore** | Scroll bar text color | Color`?` | `null` |
||||
**Fill** | Color | Color`?` | `null` |
**ScrollBack** | Scroll bar color | Color`?` | `null` |
**FillHover** | Hover color | Color`?` | `null` |
**FillActive** | Active color | Color`?` | `null` |
||||
**IconGap** | Icon to text spacing ratio | float | 0.25F |
**IconRatio** | Icon ratio | float | 0.7F |
||||
**SelectedIndex** | Selected index | int | 0 |
**Centered** | Centered display of tabs | bool | false |
**CloseDisposePage** | Release after closing page | bool | false |
**DragOrder** | Drag order | bool | false |
**EnablePageCloseByMouseDoubleClick** | Mouse double click close enable | bool | true |
**EnablePageCloseByMouseMiddle** | Mouse middle button close enable | bool | true |
**EnablePageScrolling** | Mouse wheel switch focus page enable | bool | true |
**EnableSwitch** | Switch enable | bool | true |
**SelectedTab** | Current item | [TabPage](#tabpage)`?` | `null` |
**TabMenuVisible** | Whether to display the header | bool | true |
**ScrollBackHover** | Scroll bar hover color | Color`?` | `null` |
**ScrollForeHover** | Scroll bar hover text color | Color`?` | `null` |
||||
**Alignment** | Alignment | TabAlignment | Top |
**Gap** | Gap | int | 8 |
**ItemSize** | Custom item size | int? | `null` |
**Pages** | Collection `TabCollection` | [TabCollection](#tabpage) | [] |
**Rotate** | Rotate (used for vertical display in Left/Right) | [TRotate](Enum.md#trotate) | None |
||||
**Style** | Style type | [IStyle](#istyle) | `Nonnull` |
**TypExceed** | Exceeding UI type | [TabTypExceed](Enum.md#tabtypexceed) | Button |

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
**Back** | Bar background | Color`?` | `null` |
**BackSize** | Bar background size | int | 1 |
||||
**Closable** | Closable | bool | false |
||||
**Radius** | Bar rounded corners | int | 0 |
||||
**Size** | Bar size | int | 3 |
**Padding** | Bar padding | int | 8 |

#### StyleCard

> Card style

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fill** | Card color | Color`?` | `null` |
**FillHover** | Card hover color | Color`?` | `null` |
**FillActive** | Card activate color | Color`?` | `null` |
||||
**Closable** | Closable | bool | false |
**BorderActive** | Card border activate color | Color`?` | `null` |
||||
**BorderColor** | Card border color | Color`?` | `null` |
**Radius** | Card rounded corners | int | 6 |
**Border** | Border size | int | 1 |
||||
**Gap** | Card spacing | int | 2 |



### Data

#### TabPage

> Inherited from [ScrollableControl](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Scrolling/ScrollableControl.cs)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Display Text | string ||
||||
**BadgeBack** | Badge background color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
||||
**ReadOnly** | Read only | bool | false |
**Visible** | Is it displayed | bool | true |
**Showed** | Visible | bool | false
||||
**BadgeOffsetX** | Badge offset X | int | 0 |
**BadgeOffsetY** | Badge offset Y | int | 0 |
**BadgeSize** | Badge size | float | 0.6F |
**Dock** | Defines the control edges to bind to the container | DockStyle | Fill
||||
**Badge** | Badge text | string`?` | `null` |
||||
**🌏 LocalizationText** | Text | string`?` | `null`