[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Collapse
👚

> A content area which can be collapsed and expanded.

- DefaultProperty：Items
- DefaultEvent：ExpandChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**AutoSize** | Auto size | bool | false |
**ForeColor** | Text color | Color`?` | `null` |
**ForeActive** | Text active color | Color`?` | `null` |
**HeaderBg** | Head background | Color`?` | `null` |
**HeaderPadding** | Head padding | Size | 16, 12 |
**ContentPadding** | Content padding | Size | 16, 16 |
||||
**BorderWidth** | Border width | float | 1F |
**BorderColor** | Border color | Color`?` | `null` |
||||
**Radius** | Rounded corners | int | 6 |
**Gap** | Gap | int | 0 |
**Unique** | Keep only one unfolded | bool | false |
**UniqueFull** | One expanded full | bool | false |
**AnimationSpeed** | Expand/collapse animation speed | int | 100 |
**FontExpand** | Expanded title font | Font`?` | `null` |
**TooltipConfig** | Text overflow tooltip configuration | TooltipConfig`?` | `null` |
||||
**Items** | Data collection | CollapseItemCollection | - |

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
**Expand** | Whether to expand | bool | false |
**Full** | Whether to fill remaining space | bool | false |
**Text** | Text | string | "" |
🌏 **LocalizationText** | International text | string`?` | `null` |
**Buttons** | Button collection | CollapseGroupButtonCollection | - |