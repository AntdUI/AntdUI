[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## ColorPicker

ColorPicker 颜色选择器 👚

> 提供颜色选取的组件。

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
||||
**AutoSize** | 自动大小 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
**Mode** | 颜色模式 | [TColorMode](Enum.md#tcolormode) | Hex |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
||||
**BorderWidth** | 边框宽度 | float | 1F |
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderHover** | 悬停边框颜色 | Color`?` | `null` |
**BorderActive** | 激活边框颜色 | Color`?` | `null` |
||||
**WaveSize** | 波浪大小 `点击动画` | int | 4 |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
**ShowText** | 显示Hex文字 | bool | false |
**ShowSymbol** | 显示自定义符号(长度<4) | bool | false |
**Text** | 文本 | string | `""` |
||||
**JoinLeft** 🔴 | 连接左边 `组合按钮` | bool | false |
**JoinRight** 🔴 | 连接右边 `组合按钮` | bool | false |
**JoinMode** | 组合模式 | [TJoinMode](Enum.md#tjoinmode) | None |
||||
**Value** | 颜色的值 | Color | Style.Db.Primary `主题色` |
**DisabledAlpha** | 禁用透明度 | bool | false |
**AllowClear** | 支持清除 | bool | false |
**ShowClose** | 显示关闭按钮 | bool | false |
**ShowReset** | 显示还原按钮 | bool | false |
**HasValue** | 是否包含值 | bool | true |
**ValueClear** | 获取颜色值 | Color`?` | `null` |
**Presets** | 预设的颜色 | Color[] | `null` |
||||
**Trigger** | 触发下拉的行为 | [Trigger](Enum.md#trigger) | Click |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | 下拉箭头是否显示 | bool | true |
**DropDownFontRatio** | 下拉字体比例 | float | 0.9F |

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