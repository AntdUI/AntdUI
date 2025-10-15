[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Collapse
👚

> A content area which can be collapsed and expanded.

- DefaultProperty：Items
- DefaultEvent：ExpandChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
**HeaderBg** | Head background | Color`?` | `null` |
**HeaderPadding** | Head margin | Size | 16, 12 |
**ContentPadding** | Content margin | Size | 16, 16 |
||||
**BorderWidth** | Border width | float | 1F |
**BorderColor** | Border color | Color`?` | `null` |
||||
**Radius** | Rounded corners | int |6 |
**Gap** | Gap | int | 0 |
**Unique** | Keep only one unfolded | bool | false |
||||
**Items** | Data `CollapseItem[]` | [CollapseItem[]](#collapseitem) | [] |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ExpandChanged** | Occurrence when Expand attribute value changes | void | [CollapseItem](#collapseitem) value, bool Expand `Expand or not` |


### Data

#### CollapseItem

> Inherited from [ScrollableControl](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Scrolling/ScrollableControl.cs)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Expand** | Expand | bool | true |
**Full** 🔴 | Is the remaining space fully filled | bool | false |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |