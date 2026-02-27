[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Alert
👚

> Display warning messages that require attention.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextTitle** | Title | string`?` | `null` |
🌏 **LocalizationTextTitle** | International Title | string`?` | `null` |
**TextAlign** | Text alignment | ContentAlignment | MiddleLeft |
**Radius** | Rounded corners | int | 6 |
**BorderWidth** | Border width | float | 0F |
**Icon** | Style | [TType](Enum.md#ttype) | None |
**IconSvg** | Custom icon SVG | string`?` | `null` |
**CloseIcon** | Whether to show close icon | bool | false |
**IconRatio** | Icon ratio | float`?` | `null` |
**IconGap** | Icon text gap ratio | float`?` | `null` |
**Loop** | Text carousel | bool | false |
**LoopOverflow** | Overflow carousel | bool | false |
**LoopSpeed** | Text carousel speed | int | 10 |
**LoopInfinite** | Endless carousel text | bool | true |
**LoopPauseOnMouseEnter** | Pause carousel on mouse enter | bool | true |