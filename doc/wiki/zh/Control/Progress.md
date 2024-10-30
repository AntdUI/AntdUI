[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

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
**Shape** 🔴 | 形状 | [TShapeProgress](Enum#tshapeprogress) | Round |
**IconRatio** | 图标比例 | float | 0.7F |
**ValueRatio** 🔴 | 进度条比例 | float | 0.4F |
||||
**UseSystemText** 🔴 | 使用系统文本 | bool | false |
**ShowTextDot** 🔴 | 显示进度文本小数点位数 | int | 0 |
**State** | 样式 | [TType](Enum#ttype) | None |
**ShowInTaskbar** | 任务栏中显示进度 | bool | false |
||||
**Text** | 文本 | string`?` | `null` |
**TextUnit** 🔴 | 单位文本 | string`?` | % |
**Value** | 进度条 `0F-1F` | float | 0F |
**Loading** | 加载状态 | bool | false |
**LoadingFull** | 动画铺满 | bool | false |
**Loading** | 加载状态 | bool | false |
**Animation** | 动画时长 | int | 200 |
||||
**Steps** 🔴 | 进度条总共步数 | int | 3 |
**StepSize** 🔴 | 步数大小 | int | 14 |
**StepGap** 🔴 | 步数间隔 | int | 2 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueFormatChanged** 🔴 | Value格式化时发生 | string | float value `进度` |