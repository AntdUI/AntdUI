[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Transfer
👚

> Double column transfer choice box.

- DefaultProperty：Text
- DefaultEvent：TransferChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**SourceTitle** | Left List Title | string? | `null` |
**TargetTitle** | Right List Title | string? | `null` |
||||
**ShowSelectAll** | Display all check box | bool | true |
**OneWay** | Is it unidirectional mode `only from left to right` | bool | false |
||||
**ItemHeight** | List item height | int? | `null` |
**ItemRadius** | Rounded corners of list items | int | 4 |
**PanelRadius** | List box rounded corners | int | 6 |
||||
**ForeColor** | Text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
**ButtonForeColor** | Button text color | Color`?` | `null` |
**ButtonBackColor** | Button background color | Color`?` | `null` |
**ButtonBackHover** | Button hover background color | Color`?` | `null` |
**ButtonBackActive** | Button activation background color | Color`?` | `null` |
**ButtonBackDisable** | Button to disable background color | Color`?` | `null` |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 0F |
||||
**Items** | Data [TransferItem](#transferitem) | List<[TransferItem](#transferitem)> | [] |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**TransferChanged** | Shuttle box option change event | void |  |
**Search** | search event | void |  |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Reload** | Reload data | void |  |
**GetSourceItems** | Retrieve source list items | List<[TransferItem](#transferitem)> |  |
**GetTargetItems** | Get target list items | List<[TransferItem](#transferitem)> |  |
**SetSourceSearchText** | Set source list search text | void | string text `Search Text` |
**SetTargetSearchText** | Set target list search text | void | string text `Search Text` |

### Data

#### TransferItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Value** | Value | object? | `null` |
**Selected** | Selected or not | bool | false |
**IsTarget** | Is it on the target list | bool | false |
**Tag** | User defined data | object`?` | `null` |