[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Segmented

Segmented 分段控制器 👚

> 分段控制器。

- 默认属性：Items
- 默认事件：SelectIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 集合 `SegmentedItem[]` | [SegmentedItem[]](#segmenteditem) | [] |
**TextAlign** | 文本水平对齐 | [TAlignFlow](Enum.md#talignflow) | Center |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeHover** | 悬停文字颜色 | Color`?` | `null` |
**ForeActive** | 激活文字颜色 | Color`?` | `null` |
||||
**BackColor** | 背景颜色 | Color`?` | `null` |
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**IconAlign** | 图标对齐方向 | [TAlignMini](Enum.md#talignmini) | Top |
**IconGap** | 图标与文字间距比例 | float | 0.2F |
**IconRatio** | 图标比例 | float`?` | `null` |
||||
**AutoSize** | 自动大小 | bool | false |
**PauseLayout** | 暂停布局 | bool | false |
**SelectIndex** | 选择序号 | int | 0 |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 0F |
**Radius** | 圆角 | int | 6 |
**ItemBorderColor** | 项边框颜色 | Color`?` | `null` |
**ItemBorderWidth** | 项边框宽度 | float | 0F |
**Round** | 圆角样式 | bool | false |
||||
**Shadow** | 阴影 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |
**ShadowOpacity** | 阴影透明度 | float | 0.2F |
||||
**BarPadding** | 条边距 | int | 0 |
**BarPosition** | 线条位置 | [TAlignMini](Enum.md#talignmini) | None |
**BarSize** | 条大小 | float | 3F |
**Gap** | 间距 | int | 0 |
**ItemGap** | 项间距 | float | 0.6F |
||||
**BarBg** | 显示条背景 | bool | false |
**BarColor** | 条背景色 | Color`?` | `null` |
**BarRadius** | 条圆角 | int | 0 |
**Full** | 是否铺满 | bool | false |
**RightToLeft** | 反向 | RightToLeft | No |
**TooltipConfig** | 超出文字提示配置 | TooltipConfig`?` | `null` |
**Vertical** | 是否竖向 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectIndexChanged** | SelectIndex 属性值更改时发生 | void | int index `序号` |
**ItemClick** | 项点击时发生 | void | MouseEventArgs e `点击`, SegmentedItem value |

### 数据

#### SegmentedItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**ForeColor** | 前景颜色 | Color`?` | `null` |
**BadgeFore** | 徽标前景颜色 | Color`?` | `null`
||||
**BadgeBack** | 徽标背景颜色 | Color`?` | `null` |
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**IconHover** | 图标悬浮态 | Image`?` | `null`
**IconHoverSvg** | 图标悬浮态SVG | string`?` | `null`
**IconActive** | 图标激活 | Image`?` | `null` |
**IconActiveSvg** | 图标激活SVG | string`?` | `null` |
||||
**Enabled** | 使能 | bool | true |
**BadgeMode** | 徽标模式（镂空） | bool | false |
||||
**BadgeBorderColor** | 徽标边框颜色 | Color`?` | `null`
**BadgeBorderWidth** | 徽标边框宽度 | float`?` | `null`
||||
**BadgeAlign** | 徽标方向 | [TAlignFrom](Enum.md#talignfrom) | TR |
**BadgeOffsetX** | 徽标偏移X | float | 0 |
**BadgeOffsetY** | 徽标偏移Y | float | 0 |
**BadgeSize** | 徽标大小 | float | 0.6F |
**Reverse** | 是否反向 | bool | false
||||
**Badge** | 徽标文本 | string`?` | `null` |
**BadgeSvg** | 徽标SVG | string`?` | `null` |
**Tooltip** | 提示 | string`?` | `null`
||||
|||||
||||
**🌏 LocalizationTooltip** | 提示 | string`?` | `null`
||||
**🌏 LocalizationTooltip** | 提示 | string`?` | `null` |