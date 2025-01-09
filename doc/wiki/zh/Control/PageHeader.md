[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## PageHeader

PageHeader 页头 👚

> 页头位于页容器中，页容器顶部，起到了内容概览和引导页级操作的作用。包括由面包屑、标题、页面内容简介、页面级操作等、页面级导航组成。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Mode** | 色彩模式 | [TAMode](Enum.md#tamode) | Auto |
**Loading** | 加载状态 | bool | false |
||||
**Text** | 文本 | string`?` | `null` |
**SubText** | 副标题 | string`?` | `null` |
**Description** | 描述文本 | string`?` | `null` |
**UseTitleFont** | 使用标题大小 | bool | false |
**UseTextBold** | 标题使用粗体 | bool | true |
||||
**Gap** | 间隔 | int`?` | `null` |
**SubGap** | 副标题与标题间隔 | int | 6 |
||||
**ShowIcon** | 是否显示图标 | bool | false |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string | `null` |
||||
**ShowBack** | 是否显示返回按钮 | bool | false |
**ShowButton** | 是否显示标题栏按钮 | bool | false |
**MaximizeBox** | 是否显示最大化按钮 | bool | true |
**MinimizeBox** | 是否显示最小化按钮 | bool | true |
**DragMove** | 是否可以拖动位置 | bool | true |
**CloseSize** | 关闭按钮大小 | int | 48 |
||||
**UseSystemStyleColor** | 使用系统颜色 | bool | false |
**CancelButton** | 点击退出关闭 | bool | false |
||||
**DividerShow** | 显示线 | bool | false |
**DividerColor** | 线颜色 | Color`?` | `null` |
**DividerThickness** | 线厚度 | float | 1F |
**DividerMargin** | 线边距 | int | 0 |