[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Calendar
👚

> A container that displays data in calendar form.

- DefaultProperty：Date
- DefaultEvent：DateChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
||||
**Radius** | Rounded corners | int | 6 |
||||
**Full** | Whether to fill | bool | false |
**ShowChinese** | Show lunar calendar | bool | false |
**ShowButtonToDay** | Show today | bool | true |
||||
**Value** | Current date | DateTime | `DateTime.Now` |
**MinDate** | Min date | DateTime`?` | `null` |
**MaxDate** | Max date | DateTime`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
**Fore** | Text color | Color`?` | `null` |

### Badge on the date

~~~ csharp
BadgeAction = dates =>
{
	// The dates parameter is FHIR [], and the array length is fixed at 2. It returns the start and end dates displayed on the UI
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

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LoadBadge** | Load badge | void | |
**SetBadge** | Set badge | void | Dictionary<string, DateBadge> dir |
**SetBadge** | Set badge | void | IList<DateBadge> dir |
**SetMinMax** | Set min and max date | void | DateTime min, DateTime max |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DateChanged** | Occurred when the value changes | void | DateTime value |