[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Alert
👚

> Display warning messages that require attention.

- DefaultProperty：Text
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextTitle** | Title | string`?` | `null` |
🌏 **LocalizationTextTitle** | International Title | string`?` | `null` |
**Radius** | Rounded corners | int | 6 |
**BorderWidth** | Border width | float | 0F |
**Icon** | Style | [TType](Enum.md#ttype) | None |
**Loop** | Text carousel | bool | false |
**LoopSpeed** | Text carousel speed | int | 10 |
**LoopInfinite** 🔴 | Endless carousel text | bool | true |
**LoopPauseOnMouseEnter** 🔴 | Loop pause on mouse enter | bool | true |