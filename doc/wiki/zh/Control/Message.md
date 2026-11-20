[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Message

Message 全局提示

> 全局展示操作反馈信息。

### Message.Config

> 配置全局提示

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**Font** | 字体 | Font`?` | `null` |
**Fore** | 自定义前景色 | Color`?` | `null` |
||||
**Back** | 自定义背景色 | Color`?` | `null` |
||||
**Icon** | 图标 | [TType](Enum.md#ttype) | None |
**IconCustom** | 自定义图标 | IconInfo`?` | `null` |
||||
**AutoClose** | 自动关闭时间（秒）`0等于不关闭` | int | 6 |
**ClickClose** | 是否可以点击关闭 | bool | true |
**EnableSound** | 是否启用声音 | bool | false |
**ShowInWindow** | 弹出在窗口 | bool`?` | `null` |
||||
**Radius** | 圆角 | int | 6 |
||||
**Align** | 方向 | [TAlignFrom](Enum.md#talignfrom) | Top |
**MaxWidth** | 最大宽度 | int`?` | `null` |
**Padding** | 边距 | Size | 12, 9 |
||||
**Call** | 加载回调 | object`?` | `null` |
**Form** | 所属窗口（已过时，使用 Target） | Form | `必填` |
**Target** | 所属目标 | Target | `必填` |
**TopMost** | 是否置顶 | bool | false |

#### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**close_all** | 关闭全部 | void | |
**close_id** | 关闭指定id | void | string id |
**contains** | 判断提示ID是否存在队列 | bool | string id, bool time_expand = false |

> loading业务方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**OK** | 成功 | void | string text |
**Error** | 异常 | void | string text |
**Warn** | 警告 | void | string text |
**Info** | 信息 | void | string text |
**Refresh** | 刷新UI | void ||