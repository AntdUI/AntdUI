[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Menu
👚

> A versatile menu for navigation.

- DefaultProperty：Items
- DefaultEvent：SelectChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
**ForeActive** | Activate Text color | Color`?` | `null` |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
**Indent** | Tree like indentation | bool | false |
**ShowSubBack** | Display submenu background | bool | false |
**Unique** | Keep only one submenu expanded | bool | false |
**Trigger** | Trigger dropdown behavior | [Trigger](Enum.md#trigger) | Click |
**Gap** | Gap | int | 0 |
**IconRatio** | Icon Scale | float | 1.2F |
||||
**Theme** | Color mode | [TAMode](Enum.md#tamode) | Auto |
**Mode** | Menu Type | [TMenuMode](Enum.md#tmenumode) | Inline |
**AutoCollapse** | Auto Collapse | bool | false |
**Collapsed** | Whether to fold or not | bool | false |
||||
**Items** | Data `MenuItem[]` | [MenuItem[]](#menuitem) | [] |
||||
**PauseLayout** | Pause Layout | bool | false |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectChanged** | Occurred when the value of the Select attribute is changed | void | [MenuItem](#menuitem) item |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndex** | Select the first layer | void | int index, bool focus `set focus` = true |
**SelectIndex** | Select the second layer | void | int i1 `index 1` , int i2 `index 2`, bool focus `set focus` = true |
**SelectIndex** | Select the third layer | void | int i1 `index 1` , int i2 `index 2`  , int i3 `index 3`, bool focus `set focus` = true |
||||
**Select** | Select menu | void | MenuItem item, bool focus `set focus` = true |
**Remove** | Remove menu | void | MenuItem item |


### Data

#### MenuItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string | `null` |
**IconActive** | Icon activate | Image`?` | `null` |
**IconActiveSvg** | Icon activate SVG | string | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Font** | Custom Font | Font`?` | `null` |
**Visible** | Is it displayed | bool | true |
**Enabled** | Enable | bool | true |
**Select** | Select | bool | false |
**Expand** | Expand | bool | true |
**CanExpand** | Can it be expanded | bool | `Read only` |
**Sub** | Subset ♾️ | [MenuItem[]](#menuitem) | [] |
**Tag** | User defined data | object`?` | `null` |
||||
**PARENTITEM** | Parent object | [MenuItem](#menuitem)`?` | `null` |