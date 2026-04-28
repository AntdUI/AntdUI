[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Transfer
👚

> Double column transfer choice box.

- DefaultProperty：Text
- DefaultEvent：TransferChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**SourceTitle** | Left List Title | string? | `null` |
**TargetTitle** | Right List Title | string? | `null` |
||||
**ShowSelectAll** | Display all check box | bool | true |
**OneWay** | Is it unidirectional mode (only from left to right) | bool | false |
**ShowSearch** | Whether to show search box | bool | false |
**ChangeToBottom** | Change to bottom | bool | false |
**DragSort** | Drag sort | bool | false |
||||
**SearchPlaceholderSource** | Source list search placeholder text | string? | `null` |
**SearchPlaceholderTarget** | Target list search placeholder text | string? | `null` |
🌏 **LocalizationSearchPlaceholderSource** | Source list search placeholder text localization | string? | `null` |
🌏 **LocalizationSearchPlaceholderTarget** | Target list search placeholder text localization | string? | `null` |
||||
**ItemHeight** | List item height | int? | `null` |
**PanelRadius** | List box rounded corners | int | 6 |
**PanelBack** | Panel background color | Color`?` | `null` |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeActive** | Active text color | Color`?` | `null` |
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
**Items** | Data [TransferItem](#transferitem) | TransferItemCollection | [] |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**TransferChanged** | Shuttle box option change event | void | TransferEventArgs e `Event parameters, including source and target list items` |
**Search** | Search event | void | SearchEventArgs e `Event parameters, including search text and whether it is a source list` |
**InputStyle** | Input style event | void | InputStyleEventArgs e `Event parameters, including input box and whether it is a source list` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Reload** | Reload data | void |  |
**GetSourceItems** | Retrieve source list items | List<[TransferItem](#transferitem)> |  |
**GetTargetItems** | Get target list items | List<[TransferItem](#transferitem)> |  |
**SetSourceSearchText** | Set source list search text | void | string text `Search Text` |
**SetTargetSearchText** | Set target list search text | void | string text `Search Text` |
**SetTargetItems** | Set target items in specified order (by TransferItem reference) | void | IEnumerable<TransferItem> targetItems `Target item collection, ordered` |
**SetTargetItemsById** | Set target items in specified order (by ID matching) | void | IEnumerable<string> targetIds `Target item ID collection, ordered` |

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