[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## FloatButton

> A button that floats at the top of the page.

### FloatButton.Config

> Configure FloatButton

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Font** | Font | Font`?` | `null` |
||||
**Size** | Size | int | 40 |
**Align** | Align | [TAlign](Enum.md#talign) | BR |
**Gap** | Gap | int | 40 |
**MarginX** | MarginX | int | 24 |
**MarginY** | MarginY | int | 24 |
||||
**Btns** | Button List | [ConfigBtn[]](#floatbutton.configbtn) | `Required` |
**Call** | Click on callback | Action<ConfigBtn> | `Required` |
**Control** | Belonging Control | Control`?` ||
**Form** | Belonging window | Form | `Required` |
**Target** | Belonging target | Target | `Required` |
**TopMost** | Topped | bool | false |
**Vertical** | Is it in the vertical direction | bool | true |

### FloatButton.ConfigBtn

> Configure ConfigBtn

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Name | string | `null` |
**Tag** | User defined data | object`?` | `null` |
**Text** | Text and other content | string`?` | `null` |
🌏 **LocalizationText** | Internationalization (Text) | string`?` | `null` |
**Type** | Set button type | [TTypeMini](Enum.md#ttypemini) | Default |
||||
**Fore** | Text color | Color`?` | `null` |
**BadgeFore** | Badge foreground color | Color`?` | `null` |
||||
**BadgeBack** | Badge background color | Color`?` | `null` |
||||
**Icon** | Custom Icon | Image`?` | `null` |
**IconSvg** | Custom Icon SVG | string`?` | `null` |
**IconSize** | Icon size | Size`?` | `null` |
||||
**Loading** | Loading | bool | false |
**LoadingValue** | Loading progress | float | 0.3F |
||||
**Enabled** | Enable | bool | true |
**BadgeMode** | Badge mode (hollow out) | bool | false |
||||
**Radius** | Rounded corners | int | 6 |
**BadgeBorderColor** | Badge border color | Color`?` | `null` |
**BadgeBorderWidth** | Badge border width | float`?` | `null` |
**Round** | Rounded corner style | bool | true |
||||
**BadgeAlign** | Badge align | [TAlign](Enum.md#talign) | TR |
**BadgeOffsetX** | Badge offset X | int | 0 |
**BadgeOffsetY** | Badge offset Y | int | 0 |
**BadgeSize** | Badge size | float | 0.6F |
||||
**Badge** | Badge text | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |
**Tooltip** | Tooltip | string`?` | `null` |
🌏 **LocalizationTooltip** | Internationalization (Tooltip) | string`?` | `null` |