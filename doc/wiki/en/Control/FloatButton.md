[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## FloatButton

FloatButton 悬浮按钮

> 悬浮按钮。

### FloatButton.Config

> 配置悬浮按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Form** | 所属窗口 | Form | `必填` |
**Font** | 字体 | Font`?` ||
**Control** | 所属控件 | Control`?` ||
**Align** | 方向 | [TAlign](Enum.md#talign) | BR |
**Vertical** | 是否垂直方向 | bool | true |
**TopMost** | 是否置顶 | bool | false |
**Size** | 大小 | int | 40 |
**MarginX** | 边距X | int | 24 |
**MarginY** | 边距Y | int | 24 |
**Gap** 🔴 | 间距 | int | 40 |
**Btns** | 按钮列表 | [ConfigBtn[]](#floatbutton.configbtn) | `必填` |
**Call** | 点击回调 | Action<ConfigBtn> | `必填` |

### FloatButton.ConfigBtn

> 配置按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 名称 | string | `null` |
**Text** | 文本 | string`?` | `null` |
**Fore** 🔴 | 文字颜色 | Color`?` | `null` |
**Tooltip** | 气泡的内容 | string`?` | `null` |
**Round** 🔴 | 圆角样式 | bool | true |
**Type** | 类型 | [TTypeMini](Enum.md#ttypemini) | Default |
**Radius** | 圆角 | int | 6 |
||||
**Icon** | 自定义图标 | Image`?` | `null` |
**IconSvg** | 自定义图标SVG | string`?` | `null` |
**IconSize** 🔴 | 图标大小 `不设置为自动大小` | Size | 0 × 0 |
||||
**Badge** | 徽标文本 | string`?` | `null` |
**BadgeSize** | 徽标字体大小 | float | 9F |
**BadgeBack** | 徽标背景颜色 | Color`?` | `null` |