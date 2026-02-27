[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## DatePicker
👚

> To select or input a date. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Format** | Format | string | yyyy-MM-dd `HH:mm:ss Display hour minute second selection box` |
||||
**Value** | Current date | DateTime`?` | `null` |
**MinDate** | Min date | DateTime`?` | `null` |
**MaxDate** | Max date | DateTime`?` | `null` |
**Presets** | Presets | BaseCollection | - |
**BadgeAction** | Date badge callback | Func<DateTime[], List<DateBadge>?>? | `null` |
||||
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**ShowIcon** | Display icon or not | bool | true |
**ValueTimeHorizontal** | Horizontal alignment of time item | bool | false |
**ShowButtonToDay** | Show today | bool | true |
**Picker** | Picker type | [TDatePicker](Enum.md#tdatepicker) | Date |
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |

### Badge on the date

~~~ csharp
BadgeAction = dates =>
{
	// The dates parameter is DateTime[], and the array length is fixed at 2. It returns the start and end dates displayed on the UI
	// DateTime start_date = dates[0], end_date = dates[1];
	var now = dates[1];
	return new List<AntdUI.DateBadge> {
		new AntdUI.DateBadge(now.ToString("yyyy-MM-dd"),0,Color.FromArgb(112, 237, 58)),
		new AntdUI.DateBadge(now.AddDays(1).ToString("yyyy-MM-dd"),5),
		new AntdUI.DateBadge(now.AddDays(-2).ToString("yyyy-MM-dd"),99),
		new AntdUI.DateBadge(now.AddDays(-6).ToString("yyyy-MM-dd"),998),
	};
};
~~~

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the Value changes | void | DateTime? value |
**PresetsClickChanged** | Occurrence upon preset click | void | object? value `Click on item` |
**ExpandDropChanged** | Occurs when the dropdown expand property changes | void | bool value `Whether to expand` |