[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## InputNumber
👚

> Enter a number within certain range with the mouse or keyboard. Inherited from [Input](Input.md)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Current value | decimal | 0 |
||||
**AlwaysShowControl** | Whether to always show the controller, even when not focused | bool | false |
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |
**KeyboardModifyEnabled** | Modify value with Up/Down using Increment | bool | true |
**ShowControl** | Show controller | bool | true |
**WheelModifyEnabled** | Mouse wheel modify value | bool | true |
||||
**InterceptArrowKeys** | Whether to continuously increase/decrease when arrow keys are pressed | bool | true |
**Maximum** | Maximum value | decimal`?` | `null` |
**Minimum** | Minimum value | decimal`?` | `null` |
||||
**DecimalPlaces** | Number of decimal places displayed | int | 0 |
**Hexadecimal** | Whether values should be displayed in hexadecimal | bool | false |
**Increment** | The amount to increase/decrease each time the arrow key is clicked | decimal | 1 |
**ThousandsSeparator** | Whether to show thousands separator | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | decimal value |
**ValueFormatter** | Format the numeric value for display | void | InputNumberEventArgs e |
