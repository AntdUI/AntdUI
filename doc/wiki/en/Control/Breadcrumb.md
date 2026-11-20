[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Breadcrumb
👚

> Display the current location within a hierarchy. And allow going back to states higher up in the hierarchy.

- DefaultProperty：Items
- DefaultEvent：ItemClick

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data | BreadcrumbItemCollection | [] |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeActive** | Activate Text color | Color`?` | `null` |
||||
**PauseLayout** | Pause Layout | bool | false |
||||
**Radius** | Rounded corners | int | 4 |
||||
**Gap** | Gap | int | 12 |
||||
**Separator** | Gets or sets the separator | string | "/" |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ItemClick** | Appears when clicking on an item | void | [BreadcrumbItem](#breadcrumbitem) item |


### Data

#### BreadcrumbItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**Fore** | Text color | Color`?` | `null`
**ForeActive** | Activate Text color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
||||
**Separator** | Gets or sets the custom trailing separator | string`?` | `null` |