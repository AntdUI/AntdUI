[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Slider
👚

> A Slider component for displaying current value and intervals in range.

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Current value | int | 0 |
||||
**Fill** | Colour | Color`?` | `null` |
**FillHover** | Hover color | Color`?` | `null` |
**FillActive** | Activate color | Color`?` | `null` |
||||
**KeyboardModifyEnabled** | Modify value with arrow keys; enable Tab and click focus | bool | true |
**ShowValue** | Whether to display numerical values | bool | false |
**WheelModifyEnabled** | Modify value with the mouse wheel using Increment, or move to an adjacent mark in Dots mode | bool | true |
**DotSizeActive** | Click to activate size | int | 12 |
||||
**Align** | Align | [TAlignMini](Enum.md#talignmini) | Left |
**DotSize** | Dot size | int | 10 |
**LineSize** | line Thickness | int | 4 |
**MarkTextGap** | Scale text spacing | int | 4 |
**MaxValue** | Maximum value | int | 100 |
**MinValue** | Minimum value | int | 0 |
||||
**Dots** | Can only be dragged onto the scale | bool | false |
**Increment** | Keyboard, wheel and drag increment (> 0), relative to MinValue; endpoints remain reachable. Dots with Marks takes priority; direct assignments are unchanged | int | 1 |
**Marks** | Tick marks `SliderMarkItemCollection` | SliderMarkItemCollection | `null` |
**TrackColor** | Slide color | Color`?` | `null` |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | int value |
**ValueFormatChanged** | Occurred during Value formatting `ShowValue = true occur` | string | int value |


### Data

#### SliderMarkItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string`?` | `null` |
**Value** | Value | int | 0 |
||||
**Fore** | Text color | Color`?` | `null` |
