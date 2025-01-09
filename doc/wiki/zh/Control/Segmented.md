[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Segmented

Segmented 分段控制器 👚

> 分段控制器。

- 默认属性：Items
- 默认事件：SelectIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**OriginalBackColor** 🔴 | 原装背景颜色 | Color | Transparent |
||||
**AutoSize** 🔴 | 自动大小 | bool | false |
||||
**Full** | 是否铺满 | bool | false |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeHover** | 悬停文字颜色 | Color`?` | `null` |
**ForeActive** | 激活文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**Gap** | 间距 | int | 0 |
**Vertical** | 是否竖向 | bool | false |
**IconAlign** 🔴 | 图标对齐方向 | [TAlignMini](Enum.md#talignmini) | Top |
**IconRatio** 🔴 | 图标比例 | float`?` | `null` |
**IconGap** 🔴 | 图标与文字间距比例 | float | 0.2F |
||||
**BarPosition** 🔴 | 线条位置 | [TAlignMini](Enum.md#talignmini) | None |
**BarSize** 🔴 | 条大小 | float | 3F |
**BarPadding** 🔴 | 条边距 | int | 0 |
**BarRadius** 🔴 | 条圆角 | int | 0 |
||||
**Items** | 集合 `SegmentedItem[]` | [SegmentedItem[]](#segmenteditem) | [] |
**SelectIndex** | 选择序号 | int | 0 |
||||
**PauseLayout** | 暂停布局 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectIndexChanged** | SelectIndex 属性值更改时发生 | void | int index `序号` |
**ItemClick** | 项点击时发生 | void | MouseEventArgs e `点击`, SegmentedItem value |

### 数据

#### SegmentedItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Icon** | 图标S | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**IconActive** | 图标激活 | Image`?` | `null` |
**IconActiveSvg** | 图标激活SVG | string`?` | `null` |
|||||
**Text** | 文本 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |