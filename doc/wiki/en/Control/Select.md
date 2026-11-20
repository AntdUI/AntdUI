[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Select
👚

> A dropdown menu for displaying choices. Inherited from [Input](Input)

- DefaultProperty：Text
- DefaultEvent：SelectedIndexChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data [More Styles](../DropdownStyles.md) | BaseCollection | `null` |
**AutoText** | Auto set text | bool | true |
||||
**AutoPrefixSvg** | Auto set dropdown prefix | bool | false |
**CloseIcon** | Display close icon | bool | false |
**ShowIcon** | Show icon | bool | true |
||||
**SelectedIndex** | Select Index | int | -1 |
**SelectedValue** | Select Value | object`?` | `null` |
**ListAutoWidth** | List automatic width | bool | false |
**WheelModifyEnabled** | Mouse wheel modify value | bool | true |
**DropDownEmptyClose** | Dropdown empty close | bool | false |
||||
**MaxCount** | Maximum of displayed items in the list | int | 4 |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**ArrowKeyDropDown** | Whether the Down arrow opens the drop-down | bool | true |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**DropDownPadding** | Pull down margin | Size | 12, 5 |
**DropDownTextAlign** | Dropdown text alignment | [TAlign](Enum.md#talign) | Left |
||||
**ClickEnd** | Click to the end `No nodes to click` | bool | false |
**Empty** | Empty still dropdown | bool | false |
**Expand** | Whether the arrow is expanded (UI) | bool | false |
**ExpandDrop** | Expand dropdown menu | bool | false |
**List** | Is it a list style `Like Dropdown` | bool | false |
**ClickSwitchDropdown** | Click to switch dropdown menu | bool | true |
**DropDownRadius** | Pull down rounded corner | int`?` | `null` |
**EnterDropDown** | Allow enter key to trigger dropdown | bool | true |
||||
|||

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectedIndexChanged** | Occurred when the SelectedIndex property value is changed | void | int index |
**SelectedIndexsChanged** | Occurred when changing the multi-layer tree structure | void | int x `Column`, int y `Row` |
**SelectedValueChanged** | Occurred when the SelectedValue property value is changed | void | object? value |
**ExpandDropChanged** | Occurred when the ExpandDrop property value is changed | void | bool value |
**FilterChanged** | Control filter Text changes that occur | IList<object>`?` | string value `Search For` |
**ClosedItem** | Occurred when closing a certain item | void | object? value |
**DrawItem** | Triggered before child item external rendering | bool `Whether rendering was handled` | DrawItemEventArgs e `Event parameters, including canvas, rectangle, item information, etc.` |


***


## SelectMultiple
👚

> Pull down selector. Inherited from [Input](Input)

- DefaultProperty：Text
- DefaultEvent：SelectedValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data [More Styles](../DropdownStyles.md) | object[] | [] |
||||
**SelectedValue** | Select Value | object[] | [] |
**AutoHeight** | Automatic height | bool | false |
**CanDelete** | Can it be deleted | bool | true |
**CheckMode** | Checkbox mode | bool | false |
**ListAutoWidth** | List automatic width | bool | true |
||||
**Gap** | Gap | int | 2 |
**MaxChoiceCount** | Maximum selected quantity | int | 0 |
**MaxCount** | Maximum of displayed items in the list | int | 4 |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**DropDownPadding** | Pull down margin | Size | 12, 5 |
||||
**List** | Is it a list style `Like Dropdown` | bool | false |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectAllItems** | Select all projects | void | |
**ClearSelect** | Clear Selection | void | |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectedValueChanged** | Occurred when the SelectedValue property value is changed | void | object[] value |