[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## ColorPicker

ColorPicker 颜色选择器 👚

> 提供颜色选取的组件。

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**OriginalBackColor** 🔴 | 原装背景颜色 | Color | Transparent |
||||
**AutoSize** | 自动大小 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum#tautosize) | None |
**Mode** 🔴 | 颜色模式 | [TColorMode](Enum#tcolormode) | Hex |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
||||
**BorderWidth** | 边框宽度 | float | 0F |
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderHover** | 悬停边框颜色 | Color`?` | `null` |
**BorderActive** | 激活边框颜色 | Color`?` | `null` |
||||
**WaveSize** | 波浪大小 `点击动画` | int | 4 |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
**ShowText** | 显示Hex文字 | bool | false |
||||
**JoinLeft** | 连接左边 `组合按钮` | bool | false |
**JoinRight** | 连接右边 `组合按钮` | bool | false |
||||
**Value** | 颜色的值 | Color | Style.Db.Primary `主题色` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | Color value `颜色的值` |