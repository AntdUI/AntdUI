[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Statistic
👚

> Statistic value display control, optionally with a pie or line chart.

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Value content | string`?` | `null` |
🌏 **LocalizationText** | Text | string`?` | `null` |
🌏 **LocalizationValue** | Value content | string`?` | `null` |
**TextRatio** | Text height ratio | float`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**Radius** | Rounded corners | int | 6 |
**BorderStyle** | Border Style | DashStyle | Solid |
||||
**ChartLineColor** | Chart line color | Color`?` | `null` |
**ChartLineData** | Chart line data | double[]`?` | `null` |
**ChartLineRatio** | Chart height ratio | float`?` | `null` |
**ValueRatio** | Value height ratio | float`?` | `null` |
||||
**ChartPieBg** | Chart pie background color | Color`?` | `null` |
**ChartPieColor** | Chart pie color | Color`?` | `null` |
**ChartPieData** | Chart pie data | float`?` | `null` |

### Example

> Create a statistic control with a label, a pie chart and a line chart

```csharp
var statistic = new AntdUI.Statistic
{
	Dock = DockStyle.Top,
	Height = 160,
	Radius = 8,
	Back = Color.White,
	BorderWidth = 1F,
	BorderColor = Color.LightGray,
	BorderStyle = DashStyle.Solid,
	Text = "Monthly Sales",
	Value = "¥ 128,600",
	ChartPieData = 0.72F,
	ChartPieBg = Color.FromArgb(230, 230, 230),
	ChartPieColor = Color.FromArgb(22, 119, 255)
};

var statisticTrend = new AntdUI.Statistic
{
	Dock = DockStyle.Top,
	Height = 160,
	Text = "Visitors Trend",
	Value = "8,942",
	ChartLineData = new double[] { 12, 28, 19, 35, 42, 38, 56 },
	ChartLineColor = Color.FromArgb(19, 194, 194)
};

Controls.Add(statisticTrend);
Controls.Add(statistic);
```
