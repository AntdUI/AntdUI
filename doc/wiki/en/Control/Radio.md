[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Radio
👚

> Used to select a single state from multiple options.

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
**AutoCheck** | Click to automatically change the selected status | bool | true |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**UseMnemonic** | Support mnemonic key | bool | true |
**HasFocus** | Focus state | bool | false | `Read only` |
||||
**RightToLeft** | Reverse | RightToLeft | No |
||||
|||
||||
|||
||||
|||

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | bool value `Checked state` |