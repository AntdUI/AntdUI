[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Slider
👚

> A Slider component for displaying current value and intervals in range.

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Fill** | Colour | Color`?` | `null` |
**FillHover** | Hover color | Color`?` | `null` |
**FillActive** | Activate color | Color`?` | `null` |
**TrackColor** 🔴 | Slide color | Color`?` | `null` |
||||
**MinValue** | Minimum value | int | 0 |
**MaxValue** | Maximum value | int | 100 |
**Value** | Current value | int | 0 |
||||
**Align** | Align | [TAlignMini](Enum.md#talignmini) | Left |
**ShowValue** | Whether to display numerical values | bool | false |
**LineSize** | line Thickness | int | 4 |
**DotSize** | Dot size | int | 10 |
**DotSizeActive** | Click to activate size | int | 12 |
||||
**Dots** | Can only be dragged onto the scale | bool | false |
**Marks** | Tick marks `SliderMarkItem[]` | [SliderMarkItem[]](#slidermarkitem) | [] |
**MarkTextGap** | Scale text spacing | int | 4 |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | int value |
**ValueFormatChanged** | Occurred during Value formatting `ShowValue = true occur` | string | int value |


### Data

#### SliderMarkItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Value | int | 0 |
**Fore** | Text color | Color`?` | `null` |
**Text** | Text | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |


***


## SliderRange
👚

> A Slider component for displaying current value and intervals in range. Inherited from [Slider](Slider)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value2** | Value2 | int | 10 |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Value2Changed** | Occurred when the value of the Value2 property is changed | void | int value2 |