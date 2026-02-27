[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Progress

Progress 进度条 👚

> 展示操作的当前进度。

- 默认属性：Value
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
**Back** | 背景颜色 | Color`?` | `null` |
**Fill** | 进度条颜色 | Color`?` | `null` |
||||
**Radius** | 圆角 | int | 0 |
**Shape** | 形状 | [TShapeProgress](Enum.md#tshapeprogress) | Round |
**IconRatio** | 图标比例 | float | 0.7F |
**ValueRatio** | 进度条比例 | float | 0.4F |
||||
**UseSystemText** | 使用系统文本 | bool | false |
**ShowTextDot** | 显示进度文本小数点位数 | int | 0 |
**State** | 样式 | [TType](Enum.md#ttype) | None |
**ShowInTaskbar** | 任务栏中显示进度 | bool | false |
||||
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextUnit** | 单位文本 | string`?` | % |
🌏 **LocalizationTextUnit** | 国际化单位文本 | string`?` | `null` |
**Value** | 进度条 `0F-1F` | float | 0F |
**Loading** | 加载状态 | bool | false |
**LoadingFull** | 动画铺满 | bool | false |
**Animation** | 动画时长 | int | 200 |
**UseTextCenter** | 使文本居中显示 | bool | false |
||||
**Steps** | 进度条总共步数 | int | 3 |
**StepSize** | 步数大小 | int | 14 |
**StepGap** | 步数间隔 | int | 2 |
||||
**IconCircle** | 圆形进度下的图标 | Image`?` | `null` |
**IconSvgCircle** | 圆形进度下的图标SVG | string`?` | `null` |
**IconCircleAngle** | 圆形图标是否旋转 | bool | false |
**IconCirclePadding** | 圆形图标边距 | int | 8 |
**IconCircleColor** | 圆形图标颜色 | Color`?` | `null` |
**ContainerControl** | 窗口对象 | ContainerControl`?` | `null` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueFormatChanged** | Value格式化时发生 | string | float value `进度` |