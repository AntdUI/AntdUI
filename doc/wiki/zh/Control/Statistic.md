[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Statistic

Statistic 统计数值 👚

> 展示统计数值，可附带饼图或折线图。

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Value** | 数值内容 | string`?` | `null` |
🌏 **LocalizationText** | 文本 | string`?` | `null` |
🌏 **LocalizationValue** | 数值内容 | string`?` | `null` |
**TextRatio** | 文本高度比例 | float`?` | `null` |
||||
**Back** | 背景颜色 | Color`?` | `null` |
**BackExtend** | 背景渐变色 | string`?` | `null` |
**BackgroundImage** | 背景图片 | Image`?` | `null` |
**BackgroundImageLayout** | 背景图片布局 | [TFit](Enum.md#tfit) | Fill |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**Radius** | 圆角 | int | 6 |
**BorderStyle** | 边框样式 | DashStyle | Solid |
||||
**ChartLineColor** | 图表折线图颜色 | Color`?` | `null` |
**ChartLineData** | 图表折线图数据 | double[]`?` | `null` |
**ChartLineRatio** | 图表高度比例 | float`?` | `null` |
**ValueRatio** | 值高度比例 | float`?` | `null` |
||||
**ChartPieBg** | 图表饼图背景颜色 | Color`?` | `null` |
**ChartPieColor** | 图表饼图颜色 | Color`?` | `null` |
**ChartPieData** | 图表饼图数据 | float`?` | `null` |

### 示例

> 创建带标签、饼图与折线图的统计数值控件

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
	Text = "本月销售额",
	Value = "¥ 128,600",
	ChartPieData = 0.72F,
	ChartPieBg = Color.FromArgb(230, 230, 230),
	ChartPieColor = Color.FromArgb(22, 119, 255)
};

var statisticTrend = new AntdUI.Statistic
{
	Dock = DockStyle.Top,
	Height = 160,
	Text = "访问趋势",
	Value = "8,942",
	ChartLineData = new double[] { 12, 28, 19, 35, 42, 38, 56 },
	ChartLineColor = Color.FromArgb(19, 194, 194)
};

Controls.Add(statisticTrend);
Controls.Add(statistic);
```
