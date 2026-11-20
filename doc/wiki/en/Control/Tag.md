[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tag
👚

> Used for marking and categorization.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
**TextAlign** | Text position | ContentAlignment | MiddleCenter |
**TextMultiLine** | Multiple lines | bool | false |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**BackColor** | Background color | Color`?` | `null` |
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**Image** | Image | Image`?` | `null` |
**CloseIcon** | Display close icon | bool | false |
**ImageSvg** | Image SVG | string`?` | `null` |
||||
**AutoSize** | Auto Size | bool | false |
**AutoEllipsis** | Text exceeds automatic processing | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
||||
**BorderWidth** | Border width | float | 1F |
**Radius** | Rounded corners | int | 6 |
||||
**ImageSize** | Image size `Default automatic size` | Size | 0 × 0 |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CloseChanged** | Occurred during Close | bool ||