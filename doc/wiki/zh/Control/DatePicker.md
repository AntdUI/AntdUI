[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## DatePicker

DatePicker 日期选择框 👚

> 输入或选择日期的控件。继承于 [Input](Input)

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Format** | 格式化 | string | yyyy-MM-dd `HH:mm:ss 可显示时分秒选择框` |
||||
**Value** | 控件当前日期 | DateTime`?` | `null` |
**MinDate** | 最小日期 | DateTime`?` | `null` |
**MaxDate** | 最大日期 | DateTime`?` | `null` |
**Presets** | 预置菜单 | object[] | [] |
||||
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**ShowIcon** | 是否显示图标 | bool | true |
**ValueTimeHorizontal** | 时间值水平对齐 | bool | false |

### 日期上的徽标

~~~ csharp
BadgeAction = dates =>
{
    // dates 参数为 DateTime[] 数组长度固定为2，返回UI上显示的开始日期与结束日期
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

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | DateTime? value `控件当前日期` |
**PresetsClickChanged** | 预置点击时发生 | void | object? value `点击项` |


***


## DatePickerRange

DatePickerRange 日期范围选择框 👚

> 输入或选择日期范围的控件。继承于 [Input](Input)

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Format** | 格式化 | string | yyyy-MM-dd `HH:mm:ss 可显示时分秒选择框` |
||||
**Value** | 控件当前日期 | DateTime[]`?` | `null` |
**MinDate** | 最小日期 | DateTime`?` | `null` |
**MaxDate** | 最大日期 | DateTime`?` | `null` |
**Presets** | 预置菜单 | object[] | [] |
||||
**PlaceholderStart** | 显示的水印文本S | string`?` | `null` |
**PlaceholderEnd** | 显示的水印文本E | string`?` | `null` |
**SwapSvg** | 交换图标SVG | string`?` | `null` |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**ShowIcon** | 是否显示图标 | bool | true |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | DateTime[]? value `控件当前日期` |
**PresetsClickChanged** | 预置点击时发生 | void | object? value `点击项` |