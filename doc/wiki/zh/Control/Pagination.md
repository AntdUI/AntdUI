[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Pagination

Pagination 分页 👚

> 采用分页的形式分隔长列表，每次只加载一个页面。

- 默认属性：Current
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**TextDesc** | 主动显示内容 `设置非空后 ShowTotalChanged 失效` | string`?` | `null` |
🌏 **LocalizationTextDesc** | 国际化主动显示内容 | string`?` | `null` |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
||||
**Fill** | 颜色 | Color`?` | `null` |
||||
**ShowSizeChanger** | 是否展示 `PageSize` 切换器 | bool | false |
||||
**BorderWidth** | 边框宽度 | float | 1F |
**Radius** | 圆角 | int | 6 |
||||
**Gap** | 间距 | int | 8 |
**MaxPageTotal** | 最大显示总页数 | int | 0 |
**PageSize** | 每页条数 | int | 10 |
**PageSizeOptions** | 指定每页可以显示多少条 `下拉选择` | int[]? | null |
**PageTotal** | 总页数 | int | 1 `只读` |
**RecordsPerPageText** | 每页记录数文本，标签提醒文本 | string`?` | `null` |
**SizeChangerTabStop** | 是否允许 PageSize 切换器通过 Tab 键获取焦点 | bool | true |
**SizeChangerWidth** | `SizeChanger` 宽度 | int | 0 `0 自动宽度` |
||||
**Current** | 当前页数 | int | 1 |
**RightToLeft** | 反向 | RightToLeft | No |
**Total** | 数据总数 | int | 0 |
**DropDownRadius** | 下拉圆角 | int`?` | `null` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**InitData** | 初始化 `不触发事件` | void | int Current = 1, int PageSize = 10 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | int current `当前页数`, int total `数据总数`, int pageSize `每页条数`, int pageTotal `总页数` |
**ShowTotalChanged** | 用于显示数据总量 | string `返回内容用于显示文本` | int current `当前页数`, int total `数据总数`, int pageSize `每页条数`, int pageTotal `总页数` |