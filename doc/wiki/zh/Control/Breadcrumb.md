[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Breadcrumb

Breadcrumb 面包屑 👚

> 显示当前页面在系统层级结构中的位置，并能向上返回。

- 默认属性：Items
- 默认事件：ItemClick

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeActive** | 激活字体颜色 | Color`?` | `null` |
||||
**Radius** | 圆角 | int | 4 |
**Gap** | 间距 | int | 12 |
||||
**Items** | 数据 `BreadcrumbItem[]` | [BreadcrumbItem[]](#breadcrumbitem) | [] |
||||
**PauseLayout** | 暂停布局 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ItemClick** | 点击项时发生 | void | [BreadcrumbItem](#breadcrumbitem) item `项` |


### 数据

#### BreadcrumbItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` |`null`|
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |