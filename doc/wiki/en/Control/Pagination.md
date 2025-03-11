[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Pagination

Pagination 分页 👚

> 采用分页的形式分隔长列表，每次只加载一个页面。

- 默认属性：Current
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Current** | 当前页数 | int | 1 |
**Total** | 数据总数 | int | 0 |
**PageSize** | 每页条数 | int | 10 |
**MaxPageTotal** | 最大显示总页数 | int | 0 |
**PageTotal** | 总页数 | int | 1 `只读` |
||||
**ShowSizeChanger** | 是否展示 `PageSize` 切换器 | bool | false |
**SizeChangerWidth** | `SizeChanger` 宽度 | int | 0 `0 自动宽度` |
**PageSizeOptions** | 指定每页可以显示多少条 `下拉选择` | int[]? | null |
||||
**Fill** | 颜色 | Color`?` | `null` |
||||
**Gap** | 间距 | int | 8 |
**Radius** | 圆角 | int | 6 |
**BorderWidth** | 边框宽度 | float | 1F |
||||
**TextDesc** | 主动显示内容 `设置非空后 ShowTotalChanged 失效` | string`?` | `null` |
🌏 **LocalizationTextDesc** | 国际化主动显示内容 | string`?` | `null` |
**RightToLeft** | 反向 | RightToLeft | No |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**InitData** | 初始化 `不触发事件` | void | int Current = 1, int PageSize = 10 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | int current `当前页数`, int total `数据总数`, int pageSize `每页条数`, int pageTotal `总页数` |
**ShowTotalChanged** | 用于显示数据总量 | string `返回内容用于显示文本` | int current `当前页数`, int total `数据总数`, int pageSize `每页条数`, int pageTotal `总页数` |