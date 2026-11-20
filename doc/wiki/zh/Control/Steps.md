[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Steps

Steps 步骤条 👚

> 引导用户按照流程完成任务的导航条。

- 默认属性：Current
- 默认事件：ItemClick

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 数据 `StepsItem[]` | [StepsItem[]](#stepsitem) | [] |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
||||
**MilestoneAnimation** | 里程碑模式下是否动画展示 | bool | true |
||||
**MilestoneMode** | 全新里程碑模式 | bool | false |
**PauseLayout** | 暂停布局 | bool | false |
||||
**Gap** | 间距 | int | 8 |
**MilestoneTimelineThickness** | 里程碑时间轴的显示厚度 | int | 16 |
||||
**Current** | 指定当前步骤 `从 0 开始记数。在子 Step 元素中，可以通过 status 属性覆盖状态` | int | 0 |
**MilestoneCurrentCompleted** | 当前里程碑已完成 | bool | false |
**MilestoneTimeFormat** | 里程碑时间点显示格式 | string`?` | `null` |
**MilestoneType** | 里程碑时间点的显示类型 | TMilestoneType | Day |
**Status** | 指定当前步骤的状态 | [TStepState](Enum.md#tstepstate) | Process |
**Vertical** | 垂直方向 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ItemClick** | 点击项时发生 | void | MouseEventArgs e, [StepsItem](#stepsitem) value |


### 数据

#### StepsItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | 名称 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
**Title** | 标题 | string | `必填` |
🌏 **LocalizationTitle** | 国际化标题 | string`?` | `null` |
||||
**ForeColor** | 时间轴前景色 | Color`?` | `null` |
||||
**BackColor** | 时间轴背景色 | Color`?` | `null`
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**IconSize** | 图标大小 | int`?` | `null` |
||||
**Visible** | 是否显示 | bool | true |
||||
**Description** | 详情描述 | string`?` | `null` |
🌏 **LocalizationDescription** | 国际化详情描述 | string`?` | `null` |
**MilestoneTimePoint** | 里程碑时间点 | DateTime`?` | `null`
**SubTitle** | 子标题 | string`?` | `null` |
🌏 **LocalizationSubTitle** | 国际化子标题 | string`?` | `null` |