[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Avatar
👚

> Used to represent users or things, supporting the display of images, icons, or characters.

- DefaultProperty：Image
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**HoverFore** | Hover foreground | Color`?` | `null` |
||||
**BackColor** | Background color | Color | Transparent |
**OriginalBackColor** | Original background color | Color | Transparent |
**HoverBack** | Hover background | Color`?` | `null` |
||||
**Image** | Image | Image`?` | `null` |
**ImageFit** | Image layout | [TFit](Enum.md#tfit) | Cover |
**ImageSvg** | Image SVG | string`?` | `null` |
||||
**Loading** | Loading State | bool | false |
**LoadingProgress** | Loading progress `0F-1F` | float | 0F |
||||
**PlayGIF** | Play GIF | bool | true |
**EnableHover** | Enable hover interaction | bool | false |
**HoverImage** | Hover icon | Image`?` | `null` |
**HoverImageRatio** | Hover icon ratio | float | 0.4F |
**HoverImageSvg** | Hover icon SVG | string`?` | `null` |
||||
**BorderColor** | Border color | Color | 246, 248, 250 |
**BorderWidth** | Border width | float | 0F |
**Radius** | Rounded corners | int | 0 |
**Round** | Rounded corner style | bool | false |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowOpacity** | Shadow Transparency | float | 0.3F |
||||
**Square** | Square constraint | bool | false |