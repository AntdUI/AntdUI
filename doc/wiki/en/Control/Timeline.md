[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Timeline
👚

> Vertical display timeline.

- DefaultProperty：Items
- DefaultEvent：ItemClick

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
**FontDescription** | Description font | Font`?` | `null` |
**Gap** | Gap | int`?` | `null` |
||||
**Items** | Data `TimelineItem[]` | [TimelineItem[]](#timelineitem) | [] |
||||
**PauseLayout** | Pause Layout | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ItemClick** | Occurred when clicking on an item | void | MouseEventArgs e, [TimelineItem](#timelineitem) value |


### Data

#### TimelineItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**Visible** | Is it displayed | bool | true |
**Description** | Description | string`?` | `null` |
🌏 **LocalizationDescription** | International Description | string`?` | `null` |
**Type** | Color Type | [TTypeMini](Enum.md#ttypemini) | Primary |
**Fill** | Fill color | Color`?` | `null` |
**Tag** | User defined data | object`?` | `null` |