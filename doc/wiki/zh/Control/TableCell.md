[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

[返回 Table](Table.md)

## ICell

> 丰富的单元格

#### CellText

> 文字

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` ||
||||
**Font** | 字体 | Font`?` ||
**Fore** | 字体颜色 | Color`?` ||
||||
**Back** | 背景颜色 | Color`?` ||
||||
**IconRatio** | 图标比例 | float | 0.7F |
||||
**Prefix** | 前缀 | Image`?` ||
**PrefixSvg** | 前缀SVG | string`?` ||
**Suffix** | 后缀 | Image`?` ||
**SuffixSvg** | 后缀SVG | string`?` ||

#### CellBadge

> 徽标

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` |
||||
**Fore** | 字体颜色 | Color`?` ||
||||
**Fill** | 颜色 | Color`?` ||
||||
**State** | 状态 | [TState](Enum.md#tstate) | Default |

#### CellTag

> 标签

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` ||
**Type** | 类型 | [TTypeMini](Enum.md#ttypemini) | Default |
||||
**Fore** | 字体颜色 | Color`?` ||
||||
**Back** | 背景颜色 | Color`?` ||
||||
**BorderWidth** | 边框宽度 | float |1F|

#### CellImage

> 图片

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Image** | 图片 | Image`?` | `null` |
**ImageFit** | 图片布局 | [TFit](Enum.md#tfit) | Fill |
**ImageSvg** | 图片SVG | string`?` | `null` |
||||
**BorderColor** | 边框颜色 | Color`?` ||
**BorderWidth** | 边框宽度 | float |0F|
**Radius** | 圆角 | int |6|
**Round** | 圆角样式 | bool |false|
||||
**Size** | 自定义大小 | Size`?` ||
||||
**FillSvg** | SVG填充颜色 | Color`?` ||
**Tooltip** | 文本提示 | string`?` ||

#### CellButton

> 按钮，继承于 [CellLink](#celllink)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` ||
**Type** | 类型 | [TTypeMini](Enum.md#ttypemini) | Default |
||||
**Fore** | 字体颜色 | Color`?` ||
||||
**Back** | 背景颜色 | Color`?` ||
**DefaultBack** | Default模式背景颜色 | Color`?` ||
**BackHover** | 悬停背景颜色 | Color`?` ||
**BackActive** | 激活背景颜色 | Color`?` ||
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**IconGap** | 图标与文字间距比例 | float | 0.25F |
**IconPosition** | 按钮图标组件的位置 | [TAlignMini](Enum.md#talignmini) | Left |
**IconRatio** | 图标比例 | float | 0.7F |
**IconHover** | 悬停图标 | Image`?` | `null` |
**IconHoverAnimation** | 悬停图标动画时长 | int | 200 |
**IconHoverSvg** | 悬停图标SVG | string`?` | `null` |
||||
**Ghost** | 幽灵属性 `使按钮背景透明` | bool |false |
**IsLink** | 箭头链接样式 | bool |false |
**ShowArrow** | 显示箭头 | bool |false |
||||
**BorderWidth** | 边框宽度 | float |0F|
**Radius** | 圆角 | int |6|
**Shape** | 形状 | [TShape](Enum.md#tshape) | Default |
**DefaultBorderColor** | Default模式边框颜色 | Color`?` ||

#### CellLink

> 超链接

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Id** | ID | string ||
**Text** | 文本 | string`?` ||
**TextAlign** | 文本位置 | ContentAlignment | MiddleCenter |
||||
**Enabled** | 启用 | bool |true|
||||
**Tooltip** | 文本提示 | string`?` ||

#### CellProgress

> 进度条

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Value** | 进度条 `0.0-1.0` | float |0F|
||||
**Back** | 背景颜色 | Color`?` ||
**Fill** | 进度条颜色 | Color`?` ||
||||
**Radius** | 圆角 | int |6|
**Shape** | 形状 | [TShape](Enum.md#tshape) | Default |

#### CellDivider

> 分割线

#### CellCheckbox

> 复选框

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` ||
🌏 **LocalizationText** | 国际化文本 | string`?` ||
||||
**Font** | 字体 | Font`?` ||
**Fore** | 字体颜色 | Color`?` ||
||||
**Fill** | 填充颜色 | Color`?` ||
||||
**Checked** | 选中状态 | bool | false |
**Enabled** | 启用 | bool | true |
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
**CheckState** | 选中状态 | CheckState | Unchecked |

#### CellRadio

> 单选框

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` ||
🌏 **LocalizationText** | 国际化文本 | string`?` ||
||||
**Font** | 字体 | Font`?` ||
**Fore** | 字体颜色 | Color`?` ||
||||
**Fill** | 填充颜色 | Color`?` ||
||||
**Checked** | 选中状态 | bool | false |
**Enabled** | 启用 | bool | true |
**AutoCheck** | 点击时自动改变选中状态 | bool | true |

#### CellSwitch

> 开关

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` ||
🌏 **LocalizationText** | 国际化文本 | string`?` ||
||||
**Fore** | 字体颜色 | Color`?` ||
||||
**Checked** | 选中状态 | bool | false |
**Enabled** | 启用 | bool | true |
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
**CheckedText** | 选中时显示的文本 | string`?` | `null` |