[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Rate
👚

> Used for rating operation on something.

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Current value | float | 0F |
||||
**Fill** | colour | Color | 250, 219, 20 |
||||
**AutoSize** | Auto Width | bool | false |
**AllowClear** | Support clearing | bool | false |
**AllowHalf** | Is half selection allowed | bool | false |
||||
**Character** | Custom Characters SVG | string`?` | `null` |
🌏 **LocalizationCharacter** | Internationalized Custom characters | string`?` | `null` |
**Count** | Star total | int | 5 |
**TooltipConfig** | Text tooltip configuration | TooltipConfig`?` | `null` |
**Tooltips** | Customize the prompt information for each item | string[]`?` | `null` |
||||
|||
||||
|||
||||
|||

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | float value |