[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

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
**Presets** | 预置 | BaseCollection | - |
**BadgeAction** | 日期徽标回调 | Func<DateTime[], List<DateBadge>?>? | `null` |
||||
**PlaceholderStart** | 显示的水印文本S | string`?` | `null` |
**LocalizationPlaceholderStart** | 显示的水印文本S（国际化） | string`?` | `null` |
**PlaceholderEnd** | 显示的水印文本E | string`?` | `null` |
**LocalizationPlaceholderEnd** | 显示的水印文本E（国际化） | string`?` | `null` |
**SwapSvg** | 交换图标SVG | string`?` | `null` |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | 下拉箭头是否显示 | bool | true |
**ShowIcon** | 是否显示图标 | bool | true |
**ValueTimeHorizontal** | 时间值水平对齐 | bool | false |
**InteractiveReset** | 交互重置（是否每次都开始时间选择） | bool | true |
**Picker** | 选择器类型 | [TDatePicker](Enum.md#tdatepicker) | Date |
**EnabledValueTextChange** | 文本改变时是否更新Value值 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | DateTime[]? value `控件当前日期` |
**PresetsClickChanged** | 预置点击时发生 | void | object? value `点击项` |
**ExpandDropChanged** | 下拉展开 属性值更改时发生 | void | bool value `是否展开` |