[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Checkbox
👚

> Collect user's choices.

- DefaultProperty：Checked
- DefaultEvent：CheckedChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**AutoSize** | Auto Size | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
||||
**ForeColor** | Text color | Color`?` | `null` |
**Fill** | Fill color | Color`?` | `null` |
||||
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text position | ContentAlignment | MiddleLeft |
**UseMnemonic** | Mnemonic key | bool | true |
**Checked** | Checked state | bool | false |
**CheckState** | Checked state | CheckState | Unchecked |
**AutoCheck** | Auto check on click | bool | true |
||||
**RightToLeft** | Reverse | RightToLeft | No |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | bool value `Checked state` |