[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Rate
👚

> Used for rating operation on something.

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**AutoSize** | Auto Width | bool | false |
||||
**Fill** | colour | Color | 250, 219, 20 |
||||
**AllowClear** | Support clearing | bool | false |
**AllowHalf** | Is half selection allowed | bool | false |
**Count** | Star total | int | 5 |
**Value** | Current value | float | 0F |
||||
**Tooltips** | Customize the prompt information for each item | string[]`?` | `null` |
**Character** | Custom Characters SVG | string`?` | `null` |
🌏 **LocalizationCharacter** | Internationalized Custom characters | string`?` | `null` |
**TooltipConfig** | Text tooltip configuration | [TTooltipConfig](Enum.md#ttipconfig) | `null` |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | float value |