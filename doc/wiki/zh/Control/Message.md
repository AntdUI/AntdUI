[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Message

Message 全局提示

> 全局展示操作反馈信息。

### Message.Config

> 配置全局提示

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Form** | 所属窗口 | Form | `必填` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Icon** | 图标 | [TType](Enum.md#ttype) | None |
**Font** | 字体 | Font | `null` |
**Radius** | 圆角 | int | 6 |
**AutoClose** | 自动关闭时间（秒）`0等于不关闭` | int | 6 |
**ClickClose** | 是否可以点击关闭 | bool | true |
**Align** | 方向 | [TAlignFrom](Enum.md#talignfrom) | Top |
**Padding** | 边距 | Size | 12, 9 |
**ShowInWindow** | 弹出在窗口 | bool | false |
**Call** | 加载回调 | Action<Config>`?` | `null` |

#### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**close_all** | 关闭全部 | void | |
**close_id** | 关闭指定id | void | string id |

> loading业务方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**OK** | 成功 | void | string text |
**Error** | 异常 | void | string text |
**Warn** | 警告 | void | string text |
**Info** | 信息 | void | string text |
**Refresh** | 刷新UI | void ||