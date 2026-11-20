[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## WindowBar

WindowBar 窗口栏 👚

> 窗口栏。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 | 
:--|:--|:--|:--|
**Text** | 文本 | string`?` | `null` |
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string | `null` |
**ShowIcon** | 是否显示图标 | bool | true |
||||
**Loading** | 加载状态 | bool | false |
||||
**CancelButton** | 点击退出关闭 | bool | false |
**CloseSize** | 关闭按钮大小 | int | 48 |
**DividerShow** | 显示线 | bool | false |
**DragMove** | 是否可以拖动位置 | bool | true |
**Mode** | 色彩模式 | [TAMode](Enum.md#tamode) | Auto |
**UseLeft** | 左侧使用 | int | 0 |
**UseSystemStyleColor** | 使用系统颜色 | bool | false |
||||
**DividerMargin** | 线边距 | int | 0 |
**MaximizeBox** | 是否显示最大化按钮 | bool | true |
**MinimizeBox** | 是否显示最小化按钮 | bool | true |
||||
**DividerColor** | 线颜色 | Color`?` | `null` |
**DividerThickness** | 线厚度 | float | 1F |
**SubText** | 副标题 | string`?` | `null` |