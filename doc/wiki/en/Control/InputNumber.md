[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## InputNumber
👚

> Enter a number within certain range with the mouse or keyboard. Inherited from [Input](Input.md)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Minimum** | Minimum value | decimal`?` | `null` |
**Maximum** | Maximum value | decimal`?` | `null` |
**Value** | Current value | decimal | 0 |
||||
**ShowControl** | Show controller | bool | true |
**WheelModifyEnabled** | Mouse wheel modify value | bool | true |
**DecimalPlaces** | Number of decimal places displayed | int | 0 |
**ThousandsSeparator** | Whether to show thousands separator | bool | false |
**Hexadecimal** | Whether values should be displayed in hexadecimal | bool | false |
**InterceptArrowKeys** | Whether to continuously increase/decrease when arrow keys are pressed | bool | true |
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |
**Increment** | The amount to increase/decrease each time the arrow key is clicked | decimal | 1 |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | decimal value |
**ValueFormatter** | Format the numeric value for display | void | InputNumberEventArgs e |