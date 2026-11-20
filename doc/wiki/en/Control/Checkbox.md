[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Checkbox
👚

> Collect user's choices.

- DefaultProperty：Checked
- DefaultEvent：CheckedChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text position | ContentAlignment | MiddleLeft |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**Fill** | Fill color | Color`?` | `null` |
||||
**AutoSize** | Auto Size | bool | false |
**Checked** | Checked state | bool | false |
**AutoCheck** | Auto check on click | bool | true |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**CheckState** | Checked state | CheckState | Unchecked |
**UseMnemonic** | Mnemonic key | bool | true |
**HasFocus** | Whether focus exists | bool | false |
||||
**RightToLeft** | Reverse | RightToLeft | No |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | bool value `Checked state` |