[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Alert

Alert 警告提示 👚

> 警告提示，展现需要关注的信息。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextTitle** | 标题 | string`?` | `null` |
🌏 **LocalizationTextTitle** | 国际化标题 | string`?` | `null` |
**Radius** | 圆角 | int | 6 |
**BorderWidth** | 边框宽度 | float | 0F |
**Icon** | 样式 | [TType](Enum.md#ttype) | None |
**IconSvg** | 自定义图标 | string`?` | `null` |
**CloseIcon** | 显示关闭图标 | bool | false |
**IconRatio** | 图标比例 | float | 0.86F |
**IconGap** | 图标与文字间距比例 | float | 0.4F |
**Loop** | 文本轮播 | bool | false |
**LoopOverflow** | 仅文本溢出时轮播 | bool | false |
**LoopSpeed** | 文本轮播速率 | int | 10 |
**LoopInfinite** | 轮播文本无尽 | bool | true |
**LoopPauseOnMouseEnter** | 鼠标移入时暂停轮播 | bool | true |