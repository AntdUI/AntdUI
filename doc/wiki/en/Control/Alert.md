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
**TextAlign** | Text alignment | ContentAlignment | MiddleLeft |
**TextTitle** | Title | string`?` | `null` |
🌏 **LocalizationTextTitle** | International Title | string`?` | `null` |
||||
**TextColor** | Custom text color | Color`?` | `null` |
**TextTitleColor** | Custom title color | Color`?` | `null` |
||||
**Icon** | Style | [TType](Enum.md#ttype) | None |
**IconSvg** | Custom icon SVG | string`?` | `null` |
**CloseIcon** | Whether to show close icon | bool | false |
**IconCustom** | Custom Icon | Image`?` | `null` |
**IconGap** | Icon text gap ratio | float`?` | `null` |
**IconRatio** | Icon ratio | float`?` | `null` |
||||
**Loop** | Text carousel | bool | false |
**LoopInfinite** | Endless carousel text | bool | true |
**LoopOverflow** | Overflow carousel | bool | false |
**LoopPauseOnMouseEnter** | Pause carousel on mouse enter | bool | true |
**LoopSpeed** | Text carousel speed | int | 10 |
||||
**BorderWidth** | Border width | float | 0F |
**Radius** | Rounded corners | int | 6 |