[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Calendar
👚

> A container that displays data in calendar form.

- DefaultProperty：Date
- DefaultEvent：DateChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
||||
**Radius** | Rounded corners | int | 6 |
||||
**Full** | Is it fully supported | bool | false |
**ShowChinese** | Display Lunar Calendar | bool | false |
**ShowButtonToDay** | Display today | bool | true |
||||
**Value** | Current date | DateTime | `DateTime.Now` |
**MinDate** | Min date | DateTime`?` | `null` |
**MaxDate** | Max date | DateTime`?` | `null` |

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

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LoadBadge** | Load Badge | void | |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DateChanged** | Occurred when the Value changes | void | DateTime value |