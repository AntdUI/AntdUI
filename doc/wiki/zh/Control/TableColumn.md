[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

[返回 Table](Table.md)

## Column

> 多样表头

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Key** | 绑定名称 | string ||
**Title** | 显示文字 | string ||
🌏 **LocalizationTitle** | 国际化显示文字 | string`?` | `null` |
||||
**Visible** | 是否显示 | bool|true|
**Align** | 对齐方式 | ColumnAlign |ColumnAlign.Left|
**ColAlign** | 表头对齐方式 | ColumnAlign`?` | `null` |
**Width** | 列宽度 | string`?` ||
**MaxWidth** | 列最大宽度 | string`?` ||
||||
**Fixed** | 列是否固定 | bool |false|
**Ellipsis** | 超过宽度将自动省略 | bool |false|
**LineBreak** | 自动换行 | bool |false|
**ColBreak** | 表头自动换行 | bool |false|
**SortOrder** | 启用排序 | bool |false|
**SortMode** | 排序模式 | SortMode |NONE|
**Editable** | 列可编辑 | bool |true|
**DragSort** | 列可拖拽 | bool |true|
**KeyTree** | 树形列 | string`?` ||
||||
**Style** | 列样式 | CellStyleInfo`?` ||
**ColStyle** | 标题列样式 | CellStyleInfo`?` ||
**Render** | 插槽 | Func<object? `当前值`, object `行元数据`, int `行号`, object?>? | 返回格式化后数据 |

#### ColumnCheck

> 复选框表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Key** | 绑定名称 | string ||
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
**全选** ||||
**Checked** | 选中状态 | bool | false |
**CheckState** | 选中状态 | CheckState | Unchecked |
||||
**Call** | 复选回调 | Func<bool `改变后check值`, object? `行元数据`, int `行`, int `列`, bool>`?` | 返回最终选中值 |

#### ColumnRadio

> 单选框表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Key** | 绑定名称 | string ||
**Title** | 显示文字 | string ||
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
**Call** | 复选回调 | Func<bool `改变后check值`, object? `行元数据`, int `行`, int `列`, bool>`?` | 返回最终选中值 |

#### ColumnSwitch

> 开关表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Key** | 绑定名称 | string ||
**Title** | 显示文字 | string ||
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
**Call** | 复选回调 | Func<bool `改变后check值`, object? `行元数据`, int `行`, int `列`, bool>`?` | 返回最终选中值 |

#### ColumnSort

> 拖拽手柄列。继承于 [Column](#column)