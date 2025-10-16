[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Select
👚

> A dropdown menu for displaying choices. Inherited from [Input](Input)

- DefaultProperty：Text
- DefaultEvent：SelectedIndexChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**List** | Is it a list style `Like Dropdown` | bool | false |
**ListAutoWidth** | List automatic width | bool | true |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**MaxCount** | Maximum of displayed items in the list | int | 4 |
**DropDownRadius** 🔴 | Pull down rounded corner | int`?` | `null` |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**DropDownPadding** | Pull down margin | Size | 12, 5 |
**ClickEnd** | Click to the end | bool | false |
**ClickSwitchDropdown** | Click to switch dropdown menu | bool | true |
**CloseIcon** 🔴 | Display close icon | bool | false |
||||
**Items** | Data [More Styles](../DropdownStyles.md) | object[] | [] |
**SelectedIndex** | Select Index | int | -1 |
**SelectedValue** | Select Value | object`?` | `null` |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectedIndexChanged** | Occurred when the SelectedIndex property value is changed | void | int index |
**SelectedIndexsChanged** | Occurred when changing the multi-layer tree structure | void | int x `Column`, int y `Row` |
**SelectedValueChanged** | Occurred when the SelectedValue property value is changed | void | object? value |
**FilterChanged** | Control filter Text changes that occur | IList<object>`?` | string value `Search For` |
**ClosedItem** | Occurred when closing a certain item | void | object? value |


***


## SelectMultiple
👚

> Pull down selector. Inherited from [Input](Input)

- DefaultProperty：Text
- DefaultEvent：SelectedValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**AutoHeight** 🔴 | Automatic height | bool | false |
**Gap** 🔴 | Gap | int | 2 |
**List** | Is it a list style `Like Dropdown` | bool | false |
**ListAutoWidth** | List automatic width | bool | true |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**MaxCount** | Maximum of displayed items in the list | int | 4 |
**MaxChoiceCount** | Maximum selected quantity | int | 0 |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**DropDownPadding** | Pull down margin | Size | 12, 5 |
**CheckMode** | Checkbox mode | bool | false |
**CanDelete** | Can it be deleted | bool | true |
||||
**Items** | Data [More Styles](../DropdownStyles.md) | object[] | [] |
**SelectedValue** | Select Value | object[] | |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectAllItems** | Select all projects | void | |
**ClearSelect** | Clear Selection | void | |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectedValueChanged** | Occurred when the SelectedValue property value is changed | void | object[] value |