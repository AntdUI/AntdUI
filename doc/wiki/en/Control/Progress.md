[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Progress
👚

> Display the current progress of the operation.

- DefaultProperty：Value
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Value** | Progress bar `0F-1F` | float | 0F |
**TextUnit** | Unit text | string`?` | % |
🌏 **LocalizationTextUnit** | International Unit text | string`?` | `null` |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**Fill** | Progress bar color | Color`?` | `null` |
**FillExtend** | Progress bar gradient color | string`?` | `null` |
||||
**IconCircle** | Icon under circular progress | Image`?` | `null` |
**IconCircleAngle** | Whether the circular icon rotates | bool | false |
**IconCircleColor** | Circular icon color | Color`?` | `null` |
**IconCirclePadding** | Circular icon margin | int | 8 |
**IconRatio** | Icon Scale | float | 0.7F |
**IconSvgCircle** | Icon SVG under circular progress | string`?` | `null` |
||||
**Animation** | Animation duration | int | 200 |
||||
**Loading** | Loading State | bool | false |
**LoadingColor** | Loading flow effect overlay color (`null` auto-follows `State`) | Color`?` | `null` |
**LoadingDuration** | Loading flow animation duration | int | 1000 |
**LoadingFull** | Animated Full | bool | false |
**LoadingOpacity** | Loading flow effect opacity | float | 1F |
||||
**ShowInTaskbar** | Display progress in taskbar | bool | false |
**ShowTextDot** | Display the decimal places of progress text | int | 0 |
**UseSystemText** | Using system text | bool | false |
**UseTextCenter** | Center text display | bool | false |
||||
**Radius** | Rounded corners | int | 0 |
**Shape** | Shape | [TShapeProgress](Enum.md#tshapeprogress) | Round |
||||
**GapDegree** | Gauge progress bar gap angle, value 0 ~ 295 | int | 0 |
**GapPlacement** | Gauge progress bar gap position | [TAlignMini](Enum.md#talignmini) | Bottom |
**StepGap** | Step gap | int | 2 |
**Steps** | Total number of steps in the progress bar | int | 3 |
**StepSize** | Step size | int | 14 |
**ValueRatio** | Progress bar ratio | float | 0.4F |
||||
**ContainerControl** | Window object | ContainerControl`?` | `null` |
**Segments** | Segment collection | ProgressSegment[]`?` | `null` |
**State** | Style | [TType](Enum.md#ttype) | None |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueFormatChanged** | Occurred during Value formatting | string | float value
