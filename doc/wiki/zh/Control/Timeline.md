[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Timeline

Timeline 时间轴 👚

> 垂直展示的时间流信息。

- 默认属性：Items
- 默认事件：ItemClick

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
**FontDescription** | 描述字体 | Font`?` | `null` |
**Gap** | 间距 | int`?` | `null` |
||||
**Items** | 数据 `TimelineItem[]` | [TimelineItem[]](#timelineitem) | [] |
||||
**PauseLayout** | 暂停布局 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ItemClick** | 点击项时发生 | void | MouseEventArgs e, [TimelineItem](#timelineitem) value |


### 数据

#### TimelineItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 名称 | string`?` | `null` |
**Text** | 文本 | string | `必填` |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**Visible** | 是否显示 | bool | true |
**Description** | 详情描述 | string`?` | `null` |
**Type** | 颜色类型 | [TTypeMini](Enum.md#ttypemini) | Primary |
**Fill** | 填充颜色 | Color`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |