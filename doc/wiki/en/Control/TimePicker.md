[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## TimePicker
👚

> To select/input a time. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Current time | TimeSpan | `00:00:00` |
**Format** | Format | string | HH:mm:ss |
||||
**ShowIcon** | Display icon or not | bool | true |
||||
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |
**ShowButtonNow** | Show now button | bool | true |
||||
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
||||
**ExpandDrop** | Expand dropdown menu | bool | false |
**ValueTimeHorizontal** | Horizontal alignment of time item | bool | false |


### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | TimeSpan value |
**ExpandDropChanged** | Occurred when the value of the ExpandDrop property is changed | void | bool value |