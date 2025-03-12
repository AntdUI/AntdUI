[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tag
👚

> Used for marking and categorization.

- DefaultProperty：Text
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**AutoSize** | Auto Size | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
||||
**ForeColor** | Text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
||||
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**BorderWidth** | Border width | float | 0F |
||||
**Radius** | Rounded corners | int | 6 |
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
**CloseIcon** | Display close icon | bool | false |
||||
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text position | ContentAlignment | MiddleCenter |
**AutoEllipsis** | Text exceeds automatic processing | bool | false |
**TextMultiLine** | Multiple lines | bool | false |
||||
**Image** | Image | Image`?` | `null` |
**ImageSvg** | Image SVG | string`?` | `null` |
**ImageSize** | Image size `Default automatic size` | Size | 0 × 0 |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CloseChanged** | Occurred during Close | bool ||