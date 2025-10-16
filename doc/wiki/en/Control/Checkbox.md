[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Checkbox
👚

> Collect user's choices.

- DefaultProperty：Checked
- DefaultEvent：CheckedChanged

### Property

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
**TextAlign** 🔴 | Text position | ContentAlignment | MiddleLeft |
**Checked** | Checked state | bool | false |
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**RightToLeft** | Reverse | RightToLeft | No |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | bool value `Checked state` |