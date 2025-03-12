[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Avatar 👚

> Used to represent users or things, supporting the display of images, icons, or characters.

- DefaultProperty：Image
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**BackColor** | Background color | Color`?` |`null` |
**BorderWidth** | Border width | float | 0F |
**BorderColor** | Border color | Color | 246, 248, 250 |
||||
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
||||
**Image** | Image | Image`?` | `null` |
**ImageSvg** | Image SVG | string`?` | `null` |
**ImageFit** | Image layout | [TFit](Enum.md#tfit) | Cover |
**PlayGIF** | Play GIF | bool | true |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOpacity** | Shadow Transparency | float | 0.3F |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
||||
**Loading** 🔴 | Loading State | bool | false |
**LoadingProgress** 🔴 | Loading progress `0F-1F` | float | 0F |