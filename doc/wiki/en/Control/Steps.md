[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Steps
👚

> A navigation bar that guides users through the steps of a task.

- DefaultProperty：Current
- DefaultEvent：ItemClick

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Data `StepsItemCollection` | StepsItemCollection | `null` |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**MilestoneAnimation** | Whether to display animation in milestone mode | bool | true |
||||
**MilestoneMode** | New milestone mode | bool | false |
**PauseLayout** | Pause Layout | bool | false |
||||
**Gap** | Gap | int | 8 |
**MilestoneTimelineThickness** | Display thickness of milestone timeline | int | 16 |
||||
**Current** | Get or set the current step `Start counting from 0. In the sub Step element, the status attribute can be used to override the status` | int | 0 |
**MilestoneCurrentCompleted** | Current milestone completed | bool | false |
**MilestoneTimeFormat** | Milestone time point display format | string`?` | `null` |
**MilestoneType** | Display type of milestone time point | [TMilestoneType](Enum.md#tmilestonetype) | Day |
**Status** | The status of the current step | [TStepState](Enum.md#tstepstate) | Process |
**Vertical** | Vertical direction | bool | false |
||||
|||
||||
|||

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ItemClick** | Occurred when clicking on an item | void | MouseEventArgs e, [StepsItem](#stepsitem) value |


### Data

#### StepsItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |
**Title** | Title | string | `Required` |
🌏 **LocalizationTitle** | International Title | string`?` | `null` |
||||
**ForeColor** | Timeline foreground color | Color`?` | `null` |
||||
**BackColor** | Timeline background color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconSize** | Icon size | int`?` | `null` |
||||
**Visible** | Is it displayed | bool | true |
||||
**Description** | Description | string`?` | `null` |
🌏 **LocalizationDescription** | International Description | string`?` | `null` |
**MilestoneTimePoint** | Milestone time point | DateTime`?` | `null` |
**SubTitle** | Subtitle | string`?` | `null` |
🌏 **LocalizationSubTitle** | International Subtitle | string`?` | `null` |