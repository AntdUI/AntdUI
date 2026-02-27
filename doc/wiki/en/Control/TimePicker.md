[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## TimePicker
👚

> To select/input a time. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Format** | Format | string | HH:mm:ss |
||||
**Value** | Current time | TimeSpan | `00:00:00` |
||||
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**ShowIcon** | Display icon or not | bool | true |
**ValueTimeHorizontal** | Horizontal alignment of time item | bool | false |
**ShowButtonNow** | Show now button | bool | true |
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |


### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | TimeSpan value |
**ExpandDropChanged** | Occurred when the value of the ExpandDrop property is changed | void | bool value |