[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## ColorPicker

ColorPicker 颜色选择器 👚

> 提供颜色选取的组件。

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string | `""` |
**Value** | 颜色的值 | Color | Colour.Primary.Get(nameof(ColorPicker)) `主题色` |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**DropDownFontRatio** | 下拉字体比例 | float | 0.9F |
||||
**BackColor** | 背景颜色 | Color`?` | `null` |
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
||||
**WaveSize** | 波浪大小 `点击动画` | int | 4 |
||||
**AutoSize** | 自动大小 | bool | false |
**AllowClear** | 支持清除 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
**JoinMode** | 组合模式 | [TJoinMode](Enum.md#tjoinmode) | None |
**Mode** | 颜色模式 | [TColorMode](Enum.md#tcolormode) | Hex |
**ShowClose** | 显示关闭按钮 | bool | false |
**ShowReset** | 显示还原按钮 | bool | false |
**ShowSymbol** | 显示自定义符号(长度<4) | bool | false |
**ShowText** | 显示Hex文字 | bool | false |
**Trigger** | 触发下拉的行为 | [Trigger](Enum.md#trigger) | Click |
**BorderHover** | 悬停边框颜色 | Color`?` | `null` |
**BorderActive** | 激活边框颜色 | Color`?` | `null` |
**HasFocus** | 是否存在焦点 | bool | false |
**DisabledAlpha** | 禁用透明度 | bool | false |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
||||
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | 下拉箭头是否显示 | bool | true |
||||
**HasValue** | 是否包含值 | bool | true |
**JoinLeft** | 连接左边 `组合按钮`（已过时，使用JoinMode） | bool | false |
**JoinRight** | 连接右边 `组合按钮`（已过时，使用JoinMode） | bool | false |
**Presets** | 预设的颜色 | Color[] | `null` |
**ValueClear** | 获取颜色值 | Color`?` | `null` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ClearValue** | 清空值 | void | |
**ClearValue** | 清空值 | void | Color def `默认色` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | Color value `颜色的值` |
**ValueFormatChanged** | Value格式化时发生 | string | Color value `颜色的值` |