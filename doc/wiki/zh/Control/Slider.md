[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Slider

Slider 滑动输入条 👚

> 滑动型输入器，展示当前值和可选范围。

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Value** | 当前值 | int | 0 |
||||
**Fill** | 颜色 | Color`?` | `null` |
**FillHover** | 悬停颜色 | Color`?` | `null` |
**FillActive** | 激活颜色 | Color`?` | `null` |
||||
**KeyboardModifyEnabled** | 方向键修改值，开启后支持 Tab 和点击获取焦点 | bool | true |
**ShowValue** | 是否显示数值 | bool | false |
**WheelModifyEnabled** | 鼠标滚轮按 Increment 修改值；Dots 模式跳到相邻刻度 | bool | true |
**DotSizeActive** | 点激活大小 | int | 12 |
||||
**Align** | 方向 | [TAlignMini](Enum.md#talignmini) | Left |
**DotSize** | 点大小 | int | 10 |
**LineSize** | 线条粗细 | int | 4 |
**MarkTextGap** | 刻度文本间距 | int | 4 |
**MaxValue** | 最大值 | int | 100 |
**MinValue** | 最小值 | int | 0 |
||||
**Dots** | 是否只能拖拽到刻度上 | bool | false |
**Increment** | 键盘、滚轮和拖动步长，必须大于 0；以 MinValue 为起点，保留两端点。Dots 与 Marks 优先；直接赋值不受步长限制 | int | 1 |
**Marks** | 刻度标记 `SliderMarkItem[]` | [SliderMarkItem[]](#slidermarkitem) | [] |
**TrackColor** | 滑轨颜色 | Color`?` | `null` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | int value `当前值` |
**ValueFormatChanged** | Value格式化时发生 `ShowValue = true 发生` | string | int value `当前值` |


### 数据

#### SliderMarkItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文本 | string`?` | `null` |
**Value** | 值 | int | 0 |
||||
**Fore** | 文本颜色 | Color`?` | `null` |
