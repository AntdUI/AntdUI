[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## DatePickerRange
👚

> Enter or select a date range. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Format** | Format | string | yyyy-MM-dd `HH:mm:ss Display hour minute second selection box` |
||||
**Value** | Current date | DateTime[]`?` | `null` |
**MinDate** | Min date | DateTime`?` | `null` |
**MaxDate** | Max date | DateTime`?` | `null` |
**Presets** | Presets | BaseCollection | - |
**BadgeAction** | Date badge callback | Func<DateTime[], List<DateBadge>?>? | `null` |
||||
**PlaceholderStart** | Displayed watermark text S | string`?` | `null` |
**LocalizationPlaceholderStart** | Displayed watermark text S (Internationalization) | string`?` | `null` |
**PlaceholderEnd** | Displayed watermark text E | string`?` | `null` |
**LocalizationPlaceholderEnd** | Displayed watermark text E (Internationalization) | string`?` | `null` |
**SwapSvg** | Exchange icon SVG | string`?` | `null` |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | true |
**ShowIcon** | Display icon or not | bool | true |
**ValueTimeHorizontal** | Horizontal alignment of time item | bool | false |
**InteractiveReset** | Interactive reset (whether to start time selection every time) | bool | true |
**Picker** | Picker type | [TDatePicker](Enum.md#tdatepicker) | Date |
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | DateTime[]? value |
**PresetsClickChanged** | Occurrence upon preset click | void | object? value `Click on item` |
**ExpandDropChanged** | Occurs when the dropdown expand property changes | void | bool value `Whether to expand` |