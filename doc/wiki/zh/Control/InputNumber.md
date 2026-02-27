[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## InputNumber

InputNumber 数字输入框 👚

> 通过鼠标或键盘，输入范围内的数值。继承于 [Input](Input.md)

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Minimum** | 最小值 | decimal`?` | `null` |
**Maximum** | 最大值 | decimal`?` | `null` |
**Value** | 当前值 | decimal | 0 |
||||
**ShowControl** | 显示控制器 | bool | true |
**WheelModifyEnabled** | 鼠标滚轮修改值 | bool | true |
**DecimalPlaces** | 显示的小数点位数 | int | 0 |
**ThousandsSeparator** | 是否显示千分隔符 | bool | false |
**Hexadecimal** | 值是否应以十六进制显示 | bool | false |
**InterceptArrowKeys** | 当按下箭头键时，是否持续增加/减少 | bool | true |
**EnabledValueTextChange** | 文本改变时是否更新Value值 | bool | false |
**Increment** | 每次单击箭头键时增加/减少的数量 | decimal | 1 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | decimal value `当前值` |
**ValueFormatter** | 格式化数值以供显示 | void | InputNumberEventArgs e |