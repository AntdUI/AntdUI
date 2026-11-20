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
**AutoContainerBgTransparent** | Inner container background transparent | bool | false |
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**ShadowOpacityAnimation** | Shadow Transparency Animation Enable | bool | false |
||||
**AutoScroll** | Whether to display scrollbar | bool | false |
**ShadowOpacityHover** | Transparency after hovering shadows | float | 0.3F |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 0F |
**Radius** | Rounded corners | int | 6 |
**BorderStyle** | Border Style | DashStyle | Solid |
**RadiusAlign** | Rounded corners direction | [TAlignRound](Enum.md#talignround) | ALL |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowAlign** | Shadow direction | [TAlignMini](Enum.md#talignmini) | None |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowDelay** | Shadow delay milliseconds | int | 100 |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowOpacity** | Shadow Transparency | float | 0.1F |
||||
**ArrowAlign** | Arrow direction | [TAlign](Enum.md#talign) | None |
**ArrowSize** | Arrow size | int | 8 |
**InnerPadding** | Inner padding | Padding | 0, 0, 0, 0 |
**padding** | padding | Padding | 0, 0, 0, 0 | `obsolete, use InnerPadding` |