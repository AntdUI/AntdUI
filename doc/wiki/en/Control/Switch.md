[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Switch
👚

> Used to toggle between two states.

- DefaultProperty：Checked
- DefaultEvent：CheckedChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
**Fill** | Fill color | Color`?` | `null` |
**FillHover** | Hover color | Color`?` | `null` |
||||
**Checked** | Checked state | bool | false |
**CheckedText** | Text displayed when selected | string`?` | `null` |
🌏 **LocalizationCheckedText** | International CheckedText | string`?` | `null` |
**UnCheckedText** | Text displayed when not selected | string`?` | `null` |
🌏 **LocalizationUnCheckedText** | International UnCheckedText | string`?` | `null` |
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**WaveSize** | Wave size `Click animation` | int | 4 |
**Gap** | Distance between button and border | int | 2 |
||||
**Loading** 🔴 | Loading | bool | false |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | bool value `Checked state` |