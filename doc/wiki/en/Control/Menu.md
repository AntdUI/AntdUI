[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Menu
👚

> A versatile menu for navigation.

- DefaultProperty：Items
- DefaultEvent：SelectChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data `MenuItem[]` | [MenuItem[]](#menuitem) | [] |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeActive** | Activate Text color | Color`?` | `null` |
||||
**ShowSubBack** | Display submenu background | bool | false |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**DropIconGap** | Dropdown icon margin ratio | float | 0.25 |
**DropIconRatio** | Dropdown icon ratio | float | 0.7 |
**IconGap** | Icon and text gap ratio | int`?` | `null` |
**IconRatio** | Icon Scale | float | 1.2F |
||||
**AutoCollapse** | Auto Collapse | bool | false |
**Mode** | Menu Type | [TMenuMode](Enum.md#tmenumode) | Inline |
**MouseRightCtrl** | Mouse right control | bool | true |
**PauseLayout** | Pause Layout | bool | false |
**Trigger** | Trigger dropdown behavior | [Trigger](Enum.md#trigger) | Hover |
**UseMnemonic** | If true, the first character preceded by (&) in the text is used as the menu item mnemonic | bool | true |
**FocusMode** | Focus mode | [TFocusMode](Enum.md#tfocusmode) | None |
**FocusModeAlign** | Focus mode alignment | [TAlignMini](Enum.md#talignmini) | None |
**FocusModeColor** | Focus mode color | Color`?` | `null` |
**FocusModeSize** | Focus mode size | int | 2 |
**FocusModeSizeRatio** | Focus mode size ratio | float`?` | `null` |
||||
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
||||
**ArrowRatio** | Arrow ratio | float`?` | `null` |
**Gap** | Gap | int`?` | `null` |
**HorizontalAlign** | Horizontal alignment `horizontal mode only and no scrollbar, None is center` | [TOrientation](Enum.md#torientation) | Left |
**Indent** | Tree like indentation | bool | false |
**InlineIndent** | Indent width | int`?` | `null` |
**itemMargin** | Menu item outer margin | int`?` | `null` |
**ScrollBarBlock** | Scroll bar block | bool | false |
**DropDownOffset** | Dropdown menu offset | Size | 0 × 0 |
**DropDownPadding** | Dropdown margin | Size | 16 × 10 |
||||
**Collapsed** | Whether to fold or not | bool | false |
**Theme** | Color mode (Obsolete, use ColorScheme) | [TAMode](Enum.md#tamode) | Auto |
**TooltipConfig** | Text overflow tooltip config | TooltipConfig`?` | `null` |
**Unique** | Keep only one submenu expanded | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectChanged** | Occurred when the value of the Select attribute is changed | void | [MenuItem](#menuitem) item |
**ItemClick** | Occurred when item is clicked | void | [MenuItem](#menuitem) item |
**SelectChanging** | Occurred before the Select attribute value changes | bool | [MenuItem](#menuitem) item |
**CustomButtonClick** | Occurred when custom button is clicked | void | MenuButton button, [MenuItem](#menuitem) item |
**NonItemClick** | Occurs when a blank area item is clicked | void |
**ItemPaintBegin** | Occurs before an item is painted | void | object sender, MenuItemPaintBeginEventArgs e |
**ItemPaint** | Occurs when an item is painted | void | object sender, MenuItemPaintEventArgs e |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndex** | Select the first layer | void | int index, bool focus `set focus` = true |
**SelectIndex** | Select the second layer | void | int i1 `index 1` , int i2 `index 2`, bool focus `set focus` = true |
**SelectIndex** | Select the third layer | void | int i1 `index 1` , int i2 `index 2`  , int i3 `index 3`, bool focus `set focus` = true |
||||
**Select** | Select menu | void | MenuItem item, bool focus `set focus` = true |
**Remove** | Remove menu | void | MenuItem item |
**USelect** | Cancel all selections | void | None |
**HitTest** | Hit test | MenuItem`?` | int x `X coordinate`, int y `Y coordinate` |
**GetSelectIndex** | Get selected item index | int | MenuItem item |
**FindID** | Find node by ID | MenuItem`?` | string id |
**FindName** | Find node by name | MenuItem`?` | string name |
**Focus** | Set focus | void | MenuItem menuItem, bool force = false |


### Data

#### MenuItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**Font** | Custom Font | Font`?` | `null` |
**BadgeFore** | Badge foreground color | Color`?` | `null` |
**FontSub** | Custom sub-text font | Font`?` | `null` |
||||
**BadgeBack** | Badge background color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string | `null` |
**IconActive** | Icon activate | Image`?` | `null` |
**IconActiveSvg** | Icon activate SVG | string | `null` |
||||
**Enabled** | Enable | bool | true |
**Visible** | Is it displayed | bool | true |
**BadgeMode** | Badge mode (hollow out) | bool | false |
**CanExpand** | Can it be expanded | bool | `Read only` |
**Select** | Select | bool | false |
||||
**BadgeBorderColor** | Badge border color | Color`?` | `null` |
**BadgeBorderWidth** | Badge border width | float`?` | `null` |
||||
**BadgeAlign** | Badge align | [TAlign](Enum.md#talign) | Right |
**BadgeOffsetX** | Badge offset X | int | 1 |
**BadgeOffsetY** | Badge offset Y | int | 1 |
**BadgeSize** | Badge size ratio | float | .6F |
**PARENTITEM** | Parent object | [MenuItem](#menuitem)`?` | `null` |
**Reverse** | Reverse `outermost items only, right in horizontal mode, bottom with upward expand in vertical mode` | bool | false |
||||
**Badge** | Badge content | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |
**Button** | Custom button collection | List<MenuButton>`?` |  |
**Expand** | Expand | bool | true |
**RightText** | Right text (shortcut) | string`?` | `null` |
🌏 **LocalizationRightText** | Right text | string`?` | `null` |
**Sub** | Subset ♾️ | [MenuItem[]](#menuitem) | [] |
**SubText** | Subtext | string`?` | `null` |
🌏 **LocalizationSubText** | Subtext | string`?` | `null` |

#### MenuButton

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |
||||
**BadgeFore** | Badge foreground color | Color`?` | `null` |
||||
**BadgeBack** | Badge background color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconActive** | Icon activate | Image`?` | `null` |
**IconActiveSvg** | Icon activate SVG | string`?` | `null` |
||||
**Enabled** | Enable | bool | true |
**Visible** | Is it displayed | bool | true |
**BadgeMode** | Badge mode (hollow out) | bool | false |
||||
**BadgeBorderColor** | Badge border color | Color`?` | `null` |
**BadgeBorderWidth** | Badge border width | float`?` | `null` |
||||
**BadgeAlign** | Badge align | [TAlign](Enum.md#talign) | Right |
**BadgeOffsetX** | Badge offset X | int | 1 |
**BadgeOffsetY** | Badge offset Y | int | 1 |
**BadgeSize** | Badge size ratio | float | .6F |
||||
**Badge** | Badge content | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |