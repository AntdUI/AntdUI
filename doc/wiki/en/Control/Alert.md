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
**IconSvg** | Custom icon | string`?` | `null` |
**CloseIcon** | Show close icon | bool | false |
**IconRatio** | Icon ratio | float | 0.86F |
**IconGap** | Icon text gap ratio | float | 0.4F |
**Loop** | Text carousel | bool | false |
**LoopOverflow** | Carousel only when text overflows | bool | false |
**LoopSpeed** | Text carousel speed | int | 10 |
**LoopInfinite** | Endless carousel text | bool | true |
**LoopPauseOnMouseEnter** | Pause carousel on mouse enter | bool | true |