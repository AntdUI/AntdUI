[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Pagination
👚

> A long list can be divided into several pages, and only one page will be loaded at a time.

- DefaultProperty：Current
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Current** | Current page number | int | 1 |
**Total** | Total number of data items | int | 0 |
**PageSize** | Number of data items per page | int | 10 |
**MaxPageTotal** | Maximum display total page count | int | 0 |
**PageTotal** | Total pages | int | 1 `Read only` |
||||
**ShowSizeChanger** | Do you want to display the `PageSize` switcher | bool | false |
**SizeChangerWidth** | `SizeChanger` Width | int | 0 `0 Auto Width` |
**PageSizeOptions** | Specify how many `dropdown selections` can be displayed per page | int[]? | null |
||||
**Fill** | Colour | Color`?` | `null` |
||||
**Gap** | Gap | int | 8 |
**Radius** | Rounded corners | int | 6 |
**BorderWidth** | Border width | float | 1F |
||||
**TextDesc** | Proactively display content `ShowTotalChanged becomes invalid after setting non null` | string`?` | `null` |
🌏 **LocalizationTextDesc** | International Proactively display content | string`?` | `null` |
**RightToLeft** | Reverse | RightToLeft | No |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**InitData** | Initialization `Do not trigger events` | void | int Current = 1, int PageSize = 10 |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | int current, int total, int pageSize, int pageTotal |
**ShowTotalChanged** | Used to display the total amount of data | string `Return content for displaying text` | int current, int total, int pageSize, int pageTotal |