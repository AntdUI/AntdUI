[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Tooltip

Tooltip 文字提示 👚

> 简单的文字提示气泡框。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Font** | 字体 | Color | `系统默认` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**Radius** | 圆角 | int | 6 |
**ArrowAlign** | 箭头方向 | [TAlign](Enum.md#talign) | None |
**ArrowSize** | 箭头大小 | int | 8 |
**CustomWidth** | 设定宽度 | int`?` | `null` |

### 静态方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**open** | 文字提示 | void | Control control `所属控件`, string text `文本`, [TAlign](Enum.md#talign) ArrowAlign = TAlign.Top `箭头方向` |
**open** | 文字提示 | void | Control control `所属控件`, string text `文本`, Rectangle rect `偏移量，用于容器内项`, [TAlign](Enum.md#talign) ArrowAlign = TAlign.Top `箭头方向` |
**open** | 文字提示 | void | [TooltipConfig](#tooltipconfig) `配置` |


### Component

#### TooltipComponent

名称 | 描述 | 类型 |
:--|:--|:--|
**Tip** | 文本 | string |


### 配置

#### TooltipConfig

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Font** | 字体 | Color`?` | `null` |
**Radius** | 圆角 | int | 6 |
**ArrowAlign** | 箭头方向 | [TAlign](Enum.md#talign) | None |
**ArrowSize** | 箭头大小 | int | 8 |
**Offset** | 偏移量 | Rectangle / RectangleF | `null` |
**CustomWidth** 🔴 | 设定宽度 | int`?` | `null` |