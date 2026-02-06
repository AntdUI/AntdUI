[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Transfer

Transfer 穿梭框 👚

> 双栏穿梭选择框，用于在两个区域之间移动元素。

- 默认属性：Text
- 默认事件：TransferChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**SourceTitle** | 左侧列表标题 | string? | `null` |
**TargetTitle** | 右侧列表标题 | string? | `null` |
||||
**ShowSelectAll** | 是否显示全选勾选框 | bool | true |
**OneWay** | 是否单向模式 `只能从左到右` | bool | false |
**ShowSearch** | 是否显示搜索框 | bool | true |
**ChangeToBottom** | 是否将按钮显示在底部 | bool | false |
||||
**ItemHeight** | 列表项高度 | int? | `null` |
**PanelRadius** | 列表框圆角 | int | 6 |
**PanelBack** | 面板背景颜色 | Color`?` | `null` |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
**ButtonForeColor** | 按钮文字颜色 | Color`?` | `null` |
**ButtonBackColor** | 按钮背景颜色 | Color`?` | `null` |
**ButtonBackHover** | 按钮悬停背景颜色 | Color`?` | `null` |
**ButtonBackActive** | 按钮激活背景颜色 | Color`?` | `null` |
**ButtonBackDisable** | 按钮禁用背景颜色 | Color`?` | `null` |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 0F |
||||
**Items** | 数据 [TransferItem](#transferitem) | List<[TransferItem](#transferitem)> | [] |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**Reload** | 重新加载数据 | void |  |
**GetSourceItems** | 获取源列表项 | List<[TransferItem](#transferitem)> |  |
**GetTargetItems** | 获取目标列表项 | List<[TransferItem](#transferitem)> |  |
**SetSourceSearchText** | 设置源列表搜索文本 | void | string text `搜索文本` |
**SetTargetSearchText** | 设置目标列表搜索文本 | void | string text `搜索文本` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**TransferChanged** | 穿梭框选项变化事件 | void |  |
**Search** | 搜索事件 | void |  |
**InputStyle** | 输入框样式事件 | void | Input input `输入框`, bool isSource `是否为源列表` |

### 数据

#### TransferItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Value** | 值 | object? | `null` |
**Selected** | 是否选中 | bool | false |
**IsTarget** | 是否在目标列表 | bool | false |
**Tag** | 用户定义数据 | object`?` | `null` |