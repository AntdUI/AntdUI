[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Notification

Notification 通知提醒框

> 全局展示通知提醒信息。

### Notification.Config

> 配置通知提醒框

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Title** | 标题 | string | `必填` |
🌏 **LocalizationTitle** | 国际化标题 | string`?` | `null` |
||||
**Font** | 字体 | Font`?` | `null` |
**Fore** | 自定义前景色 | Color`?` | `null` |
**FontStyleTitle** | 标题字体样式 | FontStyle`?` | `null` |
**FontTitle** | 标题字体 | Font`?` | `null` |
||||
**Back** | 自定义背景色 | Color`?` | `null` |
||||
**Icon** | 图标 | [TType](Enum.md#ttype) | None |
**CloseIcon** | 是否显示关闭图标 | bool | true |
||||
**AutoClose** | 自动关闭时间（秒）`0等于不关闭` | int | 6 |
**ClickClose** | 是否可以点击关闭 | bool | true |
**EnableSound** | 是否启用声音 | bool | false |
**OnClose** | 关闭回调 | Action`?` | `null` |
**ShowInWindow** | 弹出在窗口 | bool | false |
||||
**Radius** | 圆角 | int | 10 |
||||
**Align** | 方向 | [TAlignFrom](Enum.md#talignfrom) | Right |
**Padding** | 边距 | Size | 24, 20 |
||||
**Form** | 所属窗口（已过时，使用Target） | Form | `必填` |
**Link** | 超链接 | [Modal.ConfigLink](#modal.configlink)`?` | `null` |
**Links** | 超链接组合 | IConfigControl[]`?` | `null` |
**Target** | 所属目标 | Target | `必填` |
**TopMost** | 是否置顶 | bool | false |
||||
|||||
||||
|||||

#### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**close_all** | 关闭全部 | void | |
**close_id** | 关闭指定id | void | string id |

### Modal.ConfigLink

> 配置超链接

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 按钮文字 | string | `必填` |
||||
**Call** | 点击回调 | Action | `必填` |