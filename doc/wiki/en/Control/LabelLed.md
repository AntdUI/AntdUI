[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## LabelLed

LED Text Control 👚

> Display a segment of LED style text.

- DefaultProperty：Text
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string ||
🌏 **LocalizationText** | International Text | string`?` | `null` |
|||
**FontSize** | Font size | int`?` | `null` |
**EmojiFont** | Emoji Font | string | `Segoe UI Emoji` |
|||
**DotSize** | Dot size | int | 4 |
**DotGap** | Dot distance | int | 2 |
**TextScale** | Text scale | float | 1F |
**DotShape** | Dot shape | [LedDotShape](#leddotshape) | Square |
|||
**DotColor** | Dot color | Color`?` | `null` |
**ShowOffLed** | Show off LED | bool | false |
**OffDotColor** | Off LED color | Color`?` | `null` |
|||
**Back** | Background color | Color`?` | `null` |
**BackExtend** | Background color | string`?` | `null` |
|||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOpacity** | Shadow opacity | float | 0.3F |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |

### Enum

#### LedDotShape

| Value | Description |
|:--|:--|
| Square | Square |
| Diamond | Diamond |
| Circle | Circle |