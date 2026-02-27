[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Progress
👚

> Display the current progress of the operation.

- DefaultProperty：Value
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ForeColor** | Text color | Color`?` | `null` |
**Back** | Background color | Color`?` | `null` |
**Fill** | Progress bar color | Color`?` | `null` |
||||
**Radius** | Rounded corners | int | 0 |
**Shape** | Shape | [TShapeProgress](Enum.md#tshapeprogress) | Round |
**IconRatio** | Icon Scale | float | 0.7F |
**ValueRatio** | Progress bar ratio | float | 0.4F |
||||
**UseSystemText** | Using system text | bool | false |
**ShowTextDot** | Display the decimal places of progress text | int | 0 |
**State** | Style | [TType](Enum.md#ttype) | None |
**ShowInTaskbar** | Display progress in taskbar | bool | false |
||||
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextUnit** | Unit text | string`?` | % |
🌏 **LocalizationTextUnit** | International Unit text | string`?` | `null` |
**Value** | Progress bar `0F-1F` | float | 0F |
**Loading** | Loading State | bool | false |
**LoadingFull** | Animated Full | bool | false |
**Animation** | Animation duration | int | 200 |
**UseTextCenter** | Center text display | bool | false |
||||
**Steps** | Total number of steps in the progress bar | int | 3 |
**StepSize** | Step size | int | 14 |
**StepGap** | Step gap | int | 2 |
||||
**IconCircle** | Icon under circular progress | Image`?` | `null` |
**IconSvgCircle** | Icon SVG under circular progress | string`?` | `null` |
**IconCircleAngle** | Whether the circular icon rotates | bool | false |
**IconCirclePadding** | Circular icon margin | int | 8 |
**IconCircleColor** | Circular icon color | Color`?` | `null` |
**ContainerControl** | Window object | ContainerControl`?` | `null` |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueFormatChanged** | Occurred during Value formatting | string | float value |