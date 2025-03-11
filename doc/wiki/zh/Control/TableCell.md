[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

[返回 Table](Table.md)

## ICell

> 丰富的单元格

#### CellText

> 文字

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Back** | 背景颜色 | Color`?` ||
**Font** | 字体 | Font`?` ||
||||
**IconRatio** | 图标比例 | float | 0.7F |
**Prefix** | 前缀 | Image`?` ||
**PrefixSvg** | 前缀SVG | string`?` ||
**Suffix** | 后缀 | Image`?` ||
**SuffixSvg** | 后缀SVG | string`?` ||
||||
**Text** | 文本 | string`?` ||

#### CellBadge

> 徽标

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Fill** | 颜色 | Color`?` ||
||||
**State** | 状态 | [TState](Enum.md#tstate) | Default |
**Text** | 文本 | string`?` |

#### CellTag

> 标签

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Back** | 背景颜色 | Color`?` ||
**BorderWidth** | 边框宽度 | float |1F|
||||
**Type** | 类型 | [TTypeMini](Enum.md#ttypemini) | Default |
**Text** | 文本 | string`?` ||

#### CellImage

> 图片

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**BorderColor** | 边框颜色 | Color`?` ||
**BorderWidth** | 边框宽度 | float |0F|
**Radius** | 圆角 | int |6|
||||
**Round** | 圆角样式 | bool |false|
**Size** | 自定义大小 | Size`?` ||
||||
**Image** | 图片 | Image`?` | `null` |
**ImageSvg** | 图片SVG | string`?` | `null` |
**FillSvg** | SVG填充颜色 | Color`?` ||
**ImageFit** | 图片布局 | [TFit](Enum.md#tfit) | Fill |
||||
**Tooltip** | 文本提示 | string`?` ||

#### CellButton

> 按钮，继承于 [CellLink](#celllink)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Back** | 背景颜色 | Color`?` ||
**BackHover** | 悬停背景颜色 | Color`?` ||
**BackActive** | 激活背景颜色 | Color`?` ||
||||
**DefaultBack** | Default模式背景颜色 | Color`?` ||
**DefaultBorderColor** | Default模式边框颜色 | Color`?` ||
||||
**Radius** | 圆角 | int |6|
**BorderWidth** | 边框宽度 | float |0F|
||||
**IconRatio** | 图标比例 | float | 0.7F |
**Image** | 图像 | Image`?` | `null` |
**ImageSvg** | 图像SVG | string`?` | `null` |
**ImageHover** | 悬停图像 | Image`?` | `null` |
**ImageHoverSvg** | 悬停图像SVG | string`?` | `null` |
**ImageHoverAnimation** | 悬停图像动画时长 | int | 200 |
||||
**Shape** | 形状 | [TShape](Enum.md#tshape) | Default |
**Ghost** | 幽灵属性 `使按钮背景透明` | bool |false |
**ShowArrow** | 显示箭头 | bool |false |
**IsLink** | 箭头链接样式 | bool |false |
||||
**Type** | 类型 | [TTypeMini](Enum.md#ttypemini) | Default |
**Text** | 文本 | string`?` ||

#### CellLink

> 按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Id** | ID | string ||
**Enabled** | 启用 | bool |true|
||||
**Text** | 文本 | string`?` ||
**TextAlign** | 文本位置 | ContentAlignment | MiddleCenter |
||||
**Tooltip** | 文本提示 | string`?` ||

#### CellProgress

> 进度条

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Back** | 背景颜色 | Color`?` ||
**Fill** | 进度条颜色 | Color`?` ||
||||
**Radius** | 圆角 | int |6|
**Shape** | 形状 | [TShape](Enum.md#tshape) | Default |
||||
**Value** | 进度条 `0.0-1.0` | float |0F|

#### CellDivider

> 分割线