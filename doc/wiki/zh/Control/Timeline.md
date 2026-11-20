[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Timeline

Timeline 时间轴 👚

> 垂直展示的时间流信息。

- 默认属性：Items
- 默认事件：ItemClick

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 数据 `TimelineItem[]` | [TimelineItem[]](#timelineitem) | [] |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**FontDescription** | 描述字体 | Font`?` | `null` |
||||
**PauseLayout** | 暂停布局 | bool | false |
||||
**Gap** | 间距 | int`?` | `null` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ItemClick** | 点击项时发生 | void | MouseEventArgs e, [TimelineItem](#timelineitem) value |


### 数据

#### TimelineItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | 名称 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Type** | 颜色类型 | [TTypeMini](Enum.md#ttypemini) | Primary |
||||
**DescriptionFore** | 描述文本颜色 | Color`?` | `null`
||||
**Fill** | 填充颜色 | Color`?` | `null` |
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
||||
**Visible** | 是否显示 | bool | true |
||||
**Description** | 详情描述 | string`?` | `null` |
🌏 **LocalizationDescription** | 国际化详情描述 | string`?` | `null` |