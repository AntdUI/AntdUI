[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Panel
👚

> A container for displaying information.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Back** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
**Radius** | Rounded corners | int | 6 |
**RadiusAlign** | Rounded corners direction | [TAlignRound](Enum.md#talignround) | ALL |
**ArrowAlign** | Arrow direction | [TAlign](Enum.md#talign) | None |
**ArrowSize** | Arrow size | int | 8 |
||||
**BorderWidth** | Border width | float | 0F |
**BorderColor** | Border color | Color`?` | `null` |
**BorderStyle** | Border Style | DashStyle | Solid |
||||
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOpacity** | Shadow Transparency | float | 0.1F |
**ShadowOpacityHover** | Transparency after hovering shadows | float | 0.3F |
**ShadowOpacityAnimation** | Shadow Transparency Animation Enable | bool | false |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowAlign** | Shadow direction | [TAlignMini](Enum.md#talignmini) | None |
**InnerPadding** | Inner padding | Padding | 0, 0, 0, 0 |
**padding** | padding | Padding | 0, 0, 0, 0 | `obsolete, use InnerPadding` |