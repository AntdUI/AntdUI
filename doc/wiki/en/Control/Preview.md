[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Preview

> Picture preview box.

### Preview.Config

> Configure Preview

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Form** | Belonging window | Form | `Required` |
**Content** | Picture content | `IList<Image>` |`Required`|
**Tag** | User defined data | object`?` | `null` |
||||
**Btns** | Custom button | [Btn[]](#preview.btn) | `null` |
**OnBtns** | Custom button callback | Action<string, object?> | `null` |

### Preview.Btn

> Configure Custom button

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Button name | string | `Required` |
**IconSvg** | Icon SVG | string | `Required` |
||||
**Tag** | User defined data | object`?` | `null` |