[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## LabelLed
👚

> LED Text Control

> Display a segment of LED style text.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string ||
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text alignment | ContentAlignment | MiddleCenter |
**TextScale** | Text scale | float | 1F |
||||
**EmojiFont** | Emoji Font | string`?` | `null` |
**FontSize** | Font size | int`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**BackExtend** | Background color | string`?` | `null` |
||||
**ShowOffLed** | Show off LED | bool | false |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowOpacity** | Shadow opacity | float | 0.3F |
||||
**DotGap** | Dot distance | int | 2 |
**DotSize** | Dot size | int | 4 |
||||
**DotColor** | Dot color | Color`?` | `null` |
**DotShape** | Dot shape | [LedDotShape](#leddotshape) | Square |
**OffDotColor** | Off LED color | Color`?` | `null` |
||||
|||
||||
|||
||||
|||
||||
|||
||||
|||

### Enum

#### LedDotShape

| Value | Description |
|:--|:--|
| Square | Square |
| Diamond | Diamond |
| Circle | Circle |