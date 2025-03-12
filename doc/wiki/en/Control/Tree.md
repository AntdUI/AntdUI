[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tree
👚

> Multiple-level structure list.

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
**Gap** | Gap | int | 8 |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
**IconRatio** | Icon Scale | float | 1F |
**Checkable** | Add Checkbox checkbox in front of the node | bool | false |
**CheckStrictly** | Node selection is fully controlled in Checkable state `The selected status of parent-child nodes is no longer associated` | bool | true |
**BlockNode** | Nodes occupy a row | bool | false |
||||
**Items** | Data `TreeItem[]` | [TreeItem[]](#treeitem) | [] |
||||
**PauseLayout** | Pause Layout | bool | false |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectChanged** | Occurred when the value of the Select attribute is changed | void | MouseEventArgs args, [TreeItem](#treeitem) item, Rectangle rect |
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | [TreeItem](#treeitem) item, bool value `Select Value` |
**NodeMouseClick** | Click Item Event | void | MouseEventArgs args, [TreeItem](#treeitem) item, Rectangle rect |
**NodeMouseDoubleClick** | Double click event | void | MouseEventArgs args, [TreeItem](#treeitem) item, Rectangle rect |
**NodeMouseMove** | Mobile event | void | [TreeItem](#treeitem) item, Rectangle rect, bool hover |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ExpandAll** | Expand all | void | bool value `true expand, false collapse` |
**GetCheckeds** | Get all selected items | List<[TreeItem](#treeitem)> ||
**Select** | Select specified item | bool | [TreeItem](#treeitem) item |
**USelect** | Deselect all | void ||
**SetCheckeds** | Select All/Select None | void ||
**Focus** | Jump to specified item | bool | [TreeItem](#treeitem) item |


### Data

#### TreeItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**SubTitle** | Subtitle | string | `null` |
🌏 **LocalizationSubTitle** | International Subtitle | string`?` | `null` |
**Fore** | Font color | Color`?` |`null`|
**Back** | Background color | Color`?` |`null`|
**Visible** | Is it displayed | bool | true |
**Enabled** | Enable | bool | true |
**Expand** | Expand | bool | true |
**CanExpand** | Can it be expanded | bool | `Read only` |
**Checked** | Checked state | bool | false |
**CheckState** | Checked state | CheckState | `Unchecked` |
**Sub** | Subset ♾️ | [TreeItem[]](#treeitem) | [] |
**Tag** | User defined data | object`?` | `null` |
||||
**PARENTITEM** | Parent object | [TreeItem](#treeitem)`?` | `null` |