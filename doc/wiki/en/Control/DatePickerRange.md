[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## DatePickerRange
👚

> Enter or select a date range. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Current date | DateTime[]`?` | `null` |
**Format** | Format | string | yyyy-MM-dd `HH:mm:ss Display hour minute second selection box` |
**LocalizationPlaceholderEnd** | Displayed watermark text E (Internationalization) | string`?` | `null` |
**LocalizationPlaceholderStart** | Displayed watermark text S (Internationalization) | string`?` | `null` |
||||
**ShowIcon** | Display icon or not | bool | true |
||||
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |
**InteractiveReset** | Interactive reset (whether to start time selection every time) | bool | true |
||||
**MaxDate** | Max date | DateTime`?` | `null` |
**MinDate** | Min date | DateTime`?` | `null` |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | true |
||||
**BadgeAction** | Date badge callback | Func<DateTime[], List<DateBadge>?>? | `null` |
**ExpandDrop** | Expand dropdown menu | bool | false |
**Picker** | Picker type | [TDatePicker](Enum.md#tdatepicker) | Date |
**PlaceholderEnd** | Displayed watermark text E | string`?` | `null` |
**PlaceholderStart** | Displayed watermark text S | string`?` | `null` |
**Presets** | Presets | BaseCollection | - |
**SwapSvg** | Exchange icon SVG | string`?` | `null` |
**ValueTimeHorizontal** | Horizontal alignment of time item | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | DateTime[]? value |
**PresetsClickChanged** | Occurrence upon preset click | void | object? value `Click on item` |
**ExpandDropChanged** | Occurs when the dropdown expand property changes | void | bool value `Whether to expand` |