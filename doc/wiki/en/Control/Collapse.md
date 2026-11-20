[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Collapse
👚

> A content area which can be collapsed and expanded.

- DefaultProperty：Items
- DefaultEvent：ExpandChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data collection | CollapseItemCollection | - |
||||
**ForeColor** | Text color | Color`?` | `null` |
**FontExpand** | Expanded title font | Font`?` | `null` |
**ForeActive** | Text active color | Color`?` | `null` |
||||
**AnimationSpeed** | Expand/collapse animation speed | int | 100 |
||||
**AutoSize** | Auto size | bool | false |
**EnableResizing** | Manual resize | bool | false |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**Radius** | Rounded corners | int | 6 |
||||
**ContentPadding** | Content padding | Size | 16, 16 |
**Gap** | Gap | int | 0 |
**HeaderPadding** | Head padding | Size | 16, 12 |
**ResizingLineColor** | Resize line color (default follows Primary) | Color`?` | `null` |
**ResizingLineThickness** | Resize line thickness (0 to disable) | float | 2 |
||||
**HeaderBg** | Head background | Color`?` | `null` |
**TooltipConfig** | Text overflow tooltip configuration | TooltipConfig`?` | `null` |
**Unique** | Keep only one unfolded | bool | false |
**UniqueFull** | One expanded full | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ExpandChanged** | Occurs when Expand property value changes | void | [CollapseItem](#collapseitem) value, bool Expand `Expand or not` |
**ExpandingChanged** | Occurs when Expanding property value changes | void | [CollapseItem](#collapseitem) value, bool Expand `Expand or not` |
**ButtonClick** | Occurs when button on CollapseItem is clicked | void | [CollapseItem](#collapseitem) value, CollapseGroupButton button `Button` |

### Data

#### CollapseItem

> Inherited from [ScrollableControl](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Scrolling/ScrollableControl.cs)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string | "" |
🌏 **LocalizationText** | International text | string`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null`
**IconSvg** | Icon SVG | string`?` | `null`
||||
**Buttons** | Button collection | CollapseGroupButtonCollection | - |
**Expand** | Whether to expand | bool | false |
**Full** | Whether to fill remaining space | bool | false |