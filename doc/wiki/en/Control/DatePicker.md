[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## DatePicker
👚

> To select or input a date. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Format** | Format | string | yyyy-MM-dd `HH:mm:ss Display hour minute second selection box` |
||||
**Value** | Current date | DateTime`?` | `null` |
**MinDate** | Min date | DateTime`?` | `null` |
**MaxDate** | Max date | DateTime`?` | `null` |
**Presets** | Presets Menu | object[] | [] |
||||
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**ShowIcon** | Display icon or not | bool | true |
**ValueTimeHorizontal** | Horizontal alignment of time item | bool | false |

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

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the Value changes | void | DateTime? value |
**PresetsClickChanged** | Occurrence upon preset click | void | object? value `Click on item` |


***


## DatePickerRange 👚

> Enter or select a date range. Inherited from [Input](Input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Format** | Format | string | yyyy-MM-dd `HH:mm:ss Display hour minute second selection box` |
||||
**Value** | Current date | DateTime[]`?` | `null` |
**MinDate** | Min date | DateTime`?` | `null` |
**MaxDate** | Max date | DateTime`?` | `null` |
**Presets** | Presets Menu | object[] | [] |
||||
**PlaceholderStart** | Displayed watermark text S | string`?` | `null` |
**PlaceholderEnd** | Displayed watermark text E | string`?` | `null` |
**SwapSvg** | Exchange icon SVG | string`?` | `null` |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |
**ShowIcon** | Display icon or not | bool | true |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | DateTime[]? value |
**PresetsClickChanged** | Occurrence upon preset click | void | object? value `Click on item` |