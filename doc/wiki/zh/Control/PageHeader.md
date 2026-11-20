[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## PageHeader

PageHeader 页头 👚

> 页头位于页容器中，页容器顶部，起到了内容概览和引导页级操作的作用。包括由面包屑、标题、页面内容简介、页面级操作等、页面级导航组成。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**SubFont** | 副标题字体 | Font`Font` | `null` |
**UseForeColorDrawIcons** | 使用前景色绘制按钮图标 | bool | false |
**UseTitleFont** | 使用标题大小 | bool | false |
||||
**BackExtend** | 背景渐变色 | string`?` | `null` |
**BackIcon** | 返回图表 | string`?` | `null` |
**ShowBack** | 是否显示返回按钮 | bool | false |
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string | `null` |
**IconRatio** | 图标比例 | float`?` | `null` |
**LoadingIcon** | 加载图标 | Image`?` | `null` |
**ShowIcon** | 是否显示图标 | bool | false |
||||
**Loading** | 加载状态 | bool | false |
**LoadingSpeedTime** | 加载速度(毫秒) | int | 1000 |
**LoadingSvg** | 加载图标SVG | string`?` | `null` |
||||
**CancelButton** | 点击退出关闭 | bool | false |
**CloseSize** | 关闭按钮大小 | int | 48 |
**DividerShow** | 显示线 | bool | false |
**DragMove** | 是否可以拖动位置 | bool | true |
**EnableButtonTooltip** | 按钮悬浮提示 | bool | false |
**EnableDoubleClickMaximize** | 是否允许双击最大化 | bool | true |
**Mode** | 色彩模式 | [TAMode](Enum.md#tamode) | Auto |
**ShowButton** | 是否显示标题栏按钮 | bool | false |
**UseLeftMargin** | 使用左边边距 | bool | true |
**UseSubCenter** | 副标题居中 | bool | false |
**UseSystemStyleColor** | 使用系统颜色 | bool | false |
**UseTextBold** | 标题使用粗体 | bool | true |
||||
**DividerMargin** | 线边距 | int | 0 |
**Gap** | 间隔 | int`?` | `null` |
**IsMax** | 是否最大化 | bool | false |
**MaximizeBox** | 是否显示最大化按钮 | bool | true |
**MinimizeBox** | 是否显示最小化按钮 | bool | true |
**SubGap** | 副标题与标题间隔 | int | 6 |
||||
**Description** | 描述文本 | string`?` | `null` |
🌏 **LocalizationDescription** | 国际化描述文本 | string`?` | `null` |
**DividerColor** | 线颜色 | Color`?` | `null` |
**DividerThickness** | 线厚度 | float | 1F |
**FullBox** | 是否显示全屏按钮 | bool | false |
**IsFull** | 是否全屏 | bool | false |
**MDI** | 是否仅对父窗口生效 | bool | false |
**SubText** | 副标题 | string`?` | `null` |
🌏 **LocalizationSubText** | 国际化副标题 | string`?` | `null` |