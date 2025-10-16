[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Dropdown
👚

> A dropdown list. Inherited from [Button](Button)

- DefaultProperty：Text
- DefaultEvent：SelectedValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ListAutoWidth** | List automatic width | bool | true |
**Trigger** | Trigger dropdown behavior | [Trigger](Enum.md#trigger) | Click |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**MaxCount** | Maximum of displayed items in the list | int | 4 |
**DropDownRadius** 🔴 | Pull down rounded corner | int`?` | `null` |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**DropDownPadding** | Pull down margin | Size | 12, 5 |
**ClickEnd** | Click to the end | bool | false |
||||
**Items** | Data [More Styles](../DropdownStyles.md) | object[] | [] |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectedValueChanged** | Occurred when the SelectedValue property value is changed | void | object? value |