[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Slider

Slider 滑动输入条 👚

> 滑动型输入器，展示当前值和可选范围。

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fill** | 颜色 | Color`?` | `null` |
**FillHover** | 悬停颜色 | Color`?` | `null` |
**FillActive** | 激活颜色 | Color`?` | `null` |
**TrackColor** 🔴 | 滑轨颜色 | Color`?` | `null` |
||||
**MinValue** | 最小值 | int | 0 |
**MaxValue** | 最大值 | int | 100 |
**Value** | 当前值 | int | 0 |
||||
**Align** | 方向 | [TAlignMini](Enum.md#talignmini) | Left |
**ShowValue** | 是否显示数值 | bool | false |
**LineSize** | 线条粗细 | int | 4 |
**DotSize** | 点大小 | int | 10 |
**DotSizeActive** | 点激活大小 | int | 12 |
||||
**Dots** | 是否只能拖拽到刻度上 | bool | false |
**Marks** | 刻度标记 `SliderMarkItem[]` | [SliderMarkItem[]](#slidermarkitem) | [] |
**MarkTextGap** | 刻度文本间距 | int | 4 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | int value `当前值` |
**ValueFormatChanged** | Value格式化时发生 `ShowValue = true 发生` | string | int value `当前值` |


### 数据

#### SliderMarkItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Value** | 值 | int | 0 |
**Fore** | 文本颜色 | Color`?` | `null` |
**Text** | 文本 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |


***


## SliderRange

SliderRange 滑动范围输入条 👚

> 滑动型输入器，展示当前值和可选范围。继承于 [Slider](Slider)

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Value2** | 当前值2 | int | 10 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**Value2Changed** | Value 属性值更改时发生 | void | int value2 `当前值` |