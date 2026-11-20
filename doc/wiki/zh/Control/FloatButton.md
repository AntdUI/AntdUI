[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## FloatButton

FloatButton 悬浮按钮

> 悬浮按钮。

### FloatButton.Config

> 配置悬浮按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Font** | 字体 | Font`?` | `null` |
||||
**Size** | 大小 | int | 40 |
**Align** | 方向 | [TAlign](Enum.md#talign) | BR |
**Gap** | 间距 | int | 40 |
**MarginX** | 边距X | int | 24 |
**MarginY** | 边距Y | int | 24 |
||||
**Btns** | 按钮列表 | [ConfigBtn[]](#floatbutton.configbtn) | `必填` |
**Call** | 点击回调 | Action<ConfigBtn> | `必填` |
**Control** | 所属控件 | Control`?` ||
**Form** | 所属窗口 | Form | `必填` |
**Target** | 所属目标 | Target | `必填` |
**TopMost** | 是否置顶 | bool | false |
**Vertical** | 是否垂直方向 | bool | true |

### FloatButton.ConfigBtn

> 配置按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 名称 | string | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文字及其它内容 | string`?` | `null` |
🌏 **LocalizationText** | 国际化（文本） | string`?` | `null` |
**Type** | 设置按钮类型 | [TTypeMini](Enum.md#ttypemini) | Default |
||||
**Fore** | 文字颜色 | Color`?` | `null` |
**BadgeFore** | 徽标前景颜色 | Color`?` | `null` |
||||
**BadgeBack** | 徽标背景颜色 | Color`?` | `null` |
||||
**Icon** | 自定义图标 | Image`?` | `null` |
**IconSvg** | 自定义图标SVG | string`?` | `null` |
**IconSize** | 图标大小 | Size`?` | `null` |
||||
**Loading** | 加载 | bool | false |
**LoadingValue** | 加载进度 | float | 0.3F |
||||
**Enabled** | 使能 | bool | true |
**BadgeMode** | 徽标模式（镂空） | bool | false |
||||
**Radius** | 圆角 | int | 6 |
**BadgeBorderColor** | 徽标边框颜色 | Color`?` | `null` |
**BadgeBorderWidth** | 徽标边框宽度 | float`?` | `null` |
**Round** | 圆角样式 | bool | true |
||||
**BadgeAlign** | 徽标方向 | [TAlign](Enum.md#talign) | TR |
**BadgeOffsetX** | 徽标偏移X | int | 0 |
**BadgeOffsetY** | 徽标偏移Y | int | 0 |
**BadgeSize** | 徽标大小 | float | 0.6F |
||||
**Badge** | 徽标文本 | string`?` | `null` |
**BadgeSvg** | 徽标SVG | string`?` | `null` |
**Tooltip** | 气泡提示 | string`?` | `null` |
🌏 **LocalizationTooltip** | 国际化（气泡提示） | string`?` | `null` |