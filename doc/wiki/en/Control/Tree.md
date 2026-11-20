[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tree
👚

> Multiple-level structure list.

- DefaultProperty：Items
- DefaultEvent：SelectChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data `TreeItem[]` | [TreeItem[]](#treeitem) | [] |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeActive** | Activate Text color | Color`?` | `null` |
||||
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**DragHandleIconSize** | Drag and drop handle icon size | int | 14 |
**IconRatio** | Icon Scale | float | 1F |
||||
**Checkable** | Add Checkbox checkbox in front of the node | bool | false |
**CheckStrictly** | Node selection is fully controlled in Checkable state `The selected status of parent-child nodes is no longer associated` | bool | true |
**DragAutoExpandDelay** | Drag hover auto expand delay (ms) | int | 1000 |
**Draggable** | Enable node drag and drop | bool | true |
**DragHandleVisible** | Show drag handle | bool | true |
**Multiple** | Support selecting multiple nodes | bool | false |
**PauseLayout** | Pause Layout | bool | false |
**SelectItem** | Selected item | [TreeItem](#treeitem)`?` | `null` |
**VirtualMode** | Virtual mode (only visible rows are rendered for large data) | bool | false |
||||
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
||||
**Gap** | Gap | int | 8 |
**GapIndent** | Gap indent | int`?` | `null` |
||||
**BlockNode** | Nodes occupy a row | bool | false |
**Empty** | Is it displayed | bool | true |
**EmptyImage** | Display image with empty data | Image`?` | `null` |
**EmptyText** | Display text when data is empty | string`?` | `null` |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectChanged** | Occurred when the value of the Select attribute is changed | void | [TreeItem](#treeitem) item, Rectangle rect, TreeCType type, MouseEventArgs args |
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | [TreeItem](#treeitem) item, bool value `Select Value` |
**BeforeExpand** | Occurred before Expand is changed | bool `Return true to continue expanding` | [TreeItem](#treeitem) item, bool value `Expand value` |
**AfterExpand** | Occurred after Expand is changed | void | [TreeItem](#treeitem) item, bool value `Expand value` |
**NodeMouseClick** | Click Item Event | void | [TreeItem](#treeitem) item, Rectangle rect, TreeCType type, MouseEventArgs args |
**NodeMouseDoubleClick** | Double click event | void | [TreeItem](#treeitem) item, Rectangle rect, TreeCType type, MouseEventArgs args |
**NodeMouseMove** | Mobile event | void | [TreeItem](#treeitem) item, Rectangle rect, bool hover |
**NodeMouseDown** | Mouse down event | void | [TreeItem](#treeitem) item, Rectangle rect, TreeCType type, MouseEventArgs args |
**NodeMouseUp** | Mouse up event | void | [TreeItem](#treeitem) item, Rectangle rect, TreeCType type, MouseEventArgs args |
**Drop** | Occurred when drag drop | void | [TreeItem](#treeitem) from, [TreeItem](#treeitem) to, [TreeDropMode](#treedropmode) mode |
**DropDone** | Occurred after drag drop completed | void | [TreeItem](#treeitem) from, [TreeItem](#treeitem) to, [TreeDropMode](#treedropmode) mode |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ExpandAll** | Expand all | void | bool value `true expand, false collapse` |
**GetCheckeds** | Get all selected items | List<[TreeItem](#treeitem)> | bool Indeterminate `whether to include Indeterminate` = true |
**Select** | Select specified item | bool | [TreeItem](#treeitem) item, bool focus `set focus` = true |
**SelectID** | Select specified item (ID) | bool | string id `ID`, bool focus `set focus` = true |
**SelectName** | Select specified item (Name) | bool | string name `name`, bool focus `set focus` = true |
**USelect** | Deselect all | void | bool clear `clear selection` = true |
**SetCheckeds** | Select All/Select None | void | bool check `whether to select` |
**Focus** | Jump to specified item | void | [TreeItem](#treeitem) item, int gap `gap` = 0, bool force `force` = false |
**VisibleAll** | Set all Visible | void | bool value `whether visible` = true |
**Remove** | Remove menu | void | [TreeItem](#treeitem) item `item` |
**ReverseCheckItem** | Reverse select node item | void | [TreeItem](#treeitem) item `item` |
**GetSelects** | Get all selected items | List<[TreeItem](#treeitem)> | None |
**FindID** | Find node by ID | [TreeItem](#treeitem)`?` | string id `ID` |
**FindName** | Find node by name | [TreeItem](#treeitem)`?` | string name `name` |
**Search** | Search filter | void | string search `search text` |
**HitTest** | Hit test | [TreeItem](#treeitem)`?` | int x `X coordinate`, int y `Y coordinate`, out TreeCType type `type` |


### Data

#### TreeItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**Fore** | Font color | Color`?` |`null`|
**BadgeFore** | Badge foreground color | Color`?` | `null`
**ForeSub** | Sub-text color | Color`?` | `null`
||||
**Back** | Background color | Color`?` |`null`|
**BadgeBack** | Badge background color | Color`?` | `null`
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
||||
**Loading** | Loading State | bool | false
||||
**Checked** | Checked state | bool | false |
**Enabled** | Enable | bool | true |
**Visible** | Is it displayed | bool | true |
**BadgeMode** | Badge mode (hollow out) | bool | false
**CanExpand** | Can it be expanded | bool | `Read only` |
**Checkable** | Add Checkbox checkbox in front of the node | bool | true |
**CheckState** | Checked state | CheckState | `Unchecked` |
**Select** | Active state | bool | false |
||||
**BadgeBorderColor** | Badge border color | Color`?` | `null`
**BadgeBorderWidth** | Badge border width | float`?` | `null`
||||
**BadgeAlign** | Badge align | [TAlign](Enum.md#talign) | Right
**BadgeOffsetX** | Badge offset X | int | 1
**BadgeOffsetY** | Badge offset Y | int | 1
**BadgeSize** | Badge size ratio | float | .6F
**PARENTITEM** | Parent object | [TreeItem](#treeitem)`?` | `null` |
||||
**Badge** | Badge content | string`?` | `null`
**BadgeSvg** | Badge SVG | string`?` | `null`
**Expand** | Expand | bool | true |
**Sub** | Subset ♾️ | [TreeItem[]](#treeitem) | [] |
**SubTitle** | Subtitle | string | `null` |
🌏 **LocalizationSubTitle** | International Subtitle | string`?` | `null` |
### Enum

#### TreeDropMode

> Drag drop mode

 Name | Description | Value |
 :--|:--|:--|
 **Before** | Drop before target node | 0 |
 **Inside** | Drop inside target node (as child) | 1 |
 **After** | Drop after target node | 2 |