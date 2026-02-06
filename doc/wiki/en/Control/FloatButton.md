[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## FloatButton

> A button that floats at the top of the page.

### FloatButton.Config

> Configure FloatButton

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Target** | Belonging target | Target | `Required` |
**Form** 🔴 | Belonging window | Form | `Required` |
**Control** 🔴 | Belonging Control | Control`?` ||
**Font** | Font | Font`?` ||
**Align** | Align | [TAlign](Enum.md#talign) | BR |
**Vertical** | Is it in the vertical direction | bool | true |
**TopMost** | Topped | bool | false |
**Size** | Size | int | 40 |
**MarginX** | MarginX | int | 24 |
**MarginY** | MarginY | int | 24 |
**Gap** | Gap | int | 40 |
**Btns** | Button List | [ConfigBtn[]](#floatbutton.configbtn) | `Required` |
**Call** | Click on callback | Action<ConfigBtn> | `Required` |

### FloatButton.ConfigBtn

> Configure ConfigBtn

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Name | string | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Fore** | Text color | Color`?` | `null` |
**Enabled** | Enable | bool | true |
**Loading** | Loading | bool | false |
**LoadingValue** | Loading progress | float | 0.3F |
**Round** | Rounded corner style | bool | true |
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
**Radius** | Rounded corners | int | 6 |
**Tag** | User defined data | object`?` | `null` |
||||
**Icon** | Custom Icon | Image`?` | `null` |
**IconSvg** | Custom Icon SVG | string`?` | `null` |
**IconSize** | Icon size `Default automatic size` | Size`?` | `null` |
||||
**Badge** | Badge text | string`?` | `null` |
**BadgeSvg** | Badge SVG | string`?` | `null` |
**BadgeAlign** | Badge align | [TAlign](Enum.md#talign) | TR |
**BadgeSize** | Badge size | float | 0.6F |
**BadgeMode** | Badge mode (hollow out) | bool | false |
**BadgeOffsetX** | Badge offset X | int | 0 |
**BadgeOffsetY** | Badge offset Y | int | 0 |
**BadgeFore** | Badge foreground color | Color`?` | `null` |
**BadgeBack** | Badge background color | Color`?` | `null` |
**BadgeBorderColor** | Badge border color | Color`?` | `null` |
**BadgeBorderWidth** | Badge border width | float`?` | `null` |
||||
**Tooltip** | The content of bubbles | string`?` | `null` |
🌏 **LocalizationTooltip** | International tooltip | string`?` | `null` |