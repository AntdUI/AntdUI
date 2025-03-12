[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Steps
👚

> A navigation bar that guides users through the steps of a task.

- DefaultProperty：Current
- DefaultEvent：ItemClick

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
||||
**Current** | Get or set the current step `Start counting from 0. In the sub Step element, the status attribute can be used to override the status` | int | 0 |
**Status** | The status of the current step | [TStepState](Enum.md#tstepstate) | Process |
**Vertical** | Vertical direction | bool | false |
**Gap** | Gap | int | 8 |
**Items** | Data `StepsItem[]` | [StepsItem[]](#stepsitem) | [] |
||||
**PauseLayout** | Pause Layout | bool | false |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ItemClick** | Occurred when clicking on an item | void | MouseEventArgs e, [StepsItem](#stepsitem) value |


### Data

#### StepsItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconSize** | Icon size | int`?` | `null` |
**Visible** | Is it displayed | bool | true |
**Title** | Title | string | `Required` |
🌏 **LocalizationTitle** | International Title | string`?` | `null` |
**SubTitle** | Subtitle | string`?` | `null` |
🌏 **LocalizationSubTitle** | International Subtitle | string`?` | `null` |
**Description** | Description | string`?` | `null` |
🌏 **LocalizationDescription** | International Description | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |