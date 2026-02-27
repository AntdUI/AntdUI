[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tooltip
👚

> Simple text popup box.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Font** | Font | Font | `System default` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**Radius** | Rounded corners | int | 6 |
**ArrowAlign** | Arrow direction | [TAlign](Enum.md#talign) | Top |
**ArrowSize** | Arrow size | int`?` | `null` |
**CustomWidth** | Custom Width | int`?` | `null` |
**Back** | Background color | Color`?` | `null` |
**Fore** | Foreground color | Color`?` | `null` |

### Static Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**open** | Tooltip | void | Control control `Belonging Control`, string text, [TAlign](Enum.md#talign) ArrowAlign = TAlign.Top `Arrow direction` |
**open** | Tooltip | void | Control control `Belonging Control`, string text, Rectangle rect `Offset, used for items inside the container`, [TAlign](Enum.md#talign) ArrowAlign = TAlign.Top `Arrow direction` |
**open** | Tooltip | void | [TooltipConfig](#tooltipconfig) |


### Component

#### TooltipComponent

Name | Description | Type |
:--|:--|:--|
**Tip** | Text | string |


### Config

#### TooltipConfig

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Font** | Font | Color`?` | `null` |
**Radius** | Rounded corners | int | 6 |
**ArrowAlign** | Arrow direction | [TAlign](Enum.md#talign) | None |
**ArrowSize** | Arrow size | int | 8 |
**Offset** | Offset | Rectangle / RectangleF | `null` |
**CustomWidth** | Custom Width | int`?` | `null` |