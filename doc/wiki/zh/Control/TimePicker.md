[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## TimePicker

TimePicker 时间选择框 👚

> 输入或选择时间的控件。继承于 [Input](Input)

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Format** | 格式化 | string | HH:mm:ss |
||||
**Value** | 控件当前日期 | TimeSpan | `00:00:00` |
||||
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**ShowIcon** | 是否显示图标 | bool | true |
**ValueTimeHorizontal** | 时间值水平对齐 | bool | false |


### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | TimeSpan value `控件当前时间` |