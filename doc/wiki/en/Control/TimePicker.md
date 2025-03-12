[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## TimePicker
👚

> To select/input a time. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

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


### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | TimeSpan value |