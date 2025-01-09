[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## Notification

Notification 通知提醒框

> 全局展示通知提醒信息。

### Notification.Config

> 配置通知提醒框

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** 🔴 | ID | string`?` | `null` |
**Form** | 所属窗口 | Form | `必填` |
**Icon** | 图标 | [TType](Enum.md#ttype) | None |
**Font** | 字体 | Font`?` | `null` |
**Text** | 文本 | string | `必填` |
|||||
**Title** | 标题 | string | `必填` |
**FontTitle** 🔴 | 标题字体 | Font`?` | `null` |
**FontStyleTitle** 🔴 | 标题字体样式 | FontStyle`?` | `null` |
|||||
**Radius** | 圆角 | int | 10 |
**Align** | 方向 | [TAlignFrom](Enum.md#talignfrom) | Right |
**Padding** 🔴 | 边距 | Size | 24, 20 |
**AutoClose** | 自动关闭时间（秒）`0等于不关闭` | int | 6 |
**ClickClose** 🔴 | 是否可以点击关闭 | bool | true |
**CloseIcon** | 是否显示关闭图标 | bool | false |
**TopMost** | 是否置顶 | bool | false |
**Tag** | 用户定义数据 | object`?` | `null` |
**Link** | 超链接 | [Modal.ConfigLink](#modal.configlink)`?` | `null` |
**ShowInWindow** 🔴 | 弹出在窗口 | bool | false |

#### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**close_all** | 关闭全部 | void | |

### Modal.ConfigLink

> 配置超链接

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 按钮文字 | string | `必填` |
**Call** | 点击回调 | Action | `必填` |