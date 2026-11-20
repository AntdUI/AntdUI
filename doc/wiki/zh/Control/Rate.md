[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Rate

Rate 评分 👚

> 评分组件。

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Value** | 当前值 | float | 0F |
||||
**Fill** | 颜色 | Color | 250, 219, 20 |
||||
**AutoSize** | 自动宽度 | bool | false |
**AllowClear** | 支持清除 | bool | false |
**AllowHalf** | 是否允许半选 | bool | false |
||||
**Character** | 自定义字符SVG | string`?` | `null` |
🌏 **LocalizationCharacter** | 国际化自定义字符 | string`?` | `null` |
**Count** | Star 总数 | int | 5 |
**TooltipConfig** | 文字提示配置 | [TTooltipConfig](Enum.md#ttipconfig) | `null` |
**Tooltips** | 自定义每项的提示信息 | string[]`?` | `null` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | float value `当前值` |