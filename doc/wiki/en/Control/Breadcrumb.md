[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Breadcrumb
👚

> Display the current location within a hierarchy. And allow going back to states higher up in the hierarchy.

- DefaultProperty：Items
- DefaultEvent：ItemClick

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
**ForeActive** | Activate Text color | Color`?` | `null` |
||||
**Radius** | Rounded corners | int | 4 |
**Gap** | Gap | int | 12 |
||||
**Items** | Data `BreadcrumbItem[]` | [BreadcrumbItem[]](#breadcrumbitem) | [] |
||||
**PauseLayout** | Pause Layout | bool | false |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ItemClick** | Appears when clicking on an item | void | [BreadcrumbItem](#breadcrumbitem) item |


### Data

#### BreadcrumbItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` |`null`|
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |