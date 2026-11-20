[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

[返回 Table](Table.md)

## Column

> 多样表头

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Title** | 显示文字 | string ||
🌏 **LocalizationTitle** | 国际化显示文字 | string`?` | `null` |
||||
**ReadOnly** | 只读 | bool |false|
**Visible** | 是否显示 | bool|true|
**DisplayFormat** | 格式化显示 | string`?` ||
**DragSort** | 列可拖拽 | bool |true|
**Editable** | 列可编辑 | bool |true|
**SortMode** | 排序模式 | SortMode |NONE|
||||
**Align** | 对齐方式 | ColumnAlign |ColumnAlign.Left|
**ColAlign** | 表头对齐方式 | ColumnAlign`?` | `null` |
**Fixed** | 列是否固定 | bool |false|
**KeyTree** | 树形列 | string`?` ||
**LineBreak** | 自动换行 | bool |false|
**MaxWidth** | 列最大宽度 | string`?` ||
**MinWidth** | 列最小宽度 | string`?` ||
**SummaryItem** | 汇总栏选项 | SummaryItemOption`?` ||
**Width** | 列宽度 | string`?` ||
**Wrap** | 单元格内子元素自动换行 | bool |false|
**WrapCount** | 每行子元素数量，>0 按数量换行，<=0 回退宽度贪心换行 | int |0|
**WrapGapX** | 行内水平间距模式 | [TableWrapGap](#tablewrapgap) |Fixed|
**WrapGapY** | 行间垂直间距模式 | [TableWrapGap](#tablewrapgap) |Fixed|
||||
**ColBreak** | 表头自动换行 | bool |false|
**ColStyle** | 标题列样式 | CellStyleInfo`?` ||
**Ellipsis** | 超过宽度将自动省略 | bool |false|
**Filter** | 用户筛选选项 | FilterOption`?` ||
**HasFilter** | 存在筛选 | bool |false| (只读)
**Key** | 绑定名称 | string ||
**Render** | 插槽 | Func<object? `当前值`, object `行元数据`, int `行号`, object?>? | 返回格式化后数据 |
**SortOrder** | 启用排序 | bool |false|
**Style** | 列样式 | CellStyleInfo`?` ||

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SetWrap** | 设置单元格内子元素换行 | [Column](#column) | int count = 0 `0 关闭换行，>0 按数量换行，<0 按宽度自适应换行` |
**SetWrapGapX** | 设置行内水平间距模式 | [Column](#column) | [TableWrapGap](#tablewrapgap) value |
**SetWrapGapY** | 设置行间垂直间距模式 | [Column](#column) | [TableWrapGap](#tablewrapgap) value |

### 枚举

#### TableWrapGap

> 换行间距模式

 名称 | 描述 | 值 |
 :--|:--|:--|
 **Fixed** | 固定间距 | 0 |
 **SpaceBetween** | 等分占满（首尾与行间间隙一致） | 1 |

#### ColumnCheck

> 复选框表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Checked** | 选中状态 | bool | false |
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
**CheckState** | 选中状态 | CheckState | Unchecked |
||||
**Call** | 复选回调 | Func<bool `改变后check值`, object? `行元数据`, int `行`, int `列`, bool>`?` | 返回最终选中值 |
**Key** | 绑定名称 | string ||
||||
**全选** ||||

#### ColumnRadio

> 单选框表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Title** | 显示文字 | string ||
||||
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
||||
**Call** | 复选回调 | Func<bool `改变后check值`, object? `行元数据`, int `行`, int `列`, bool>`?` | 返回最终选中值 |
**Key** | 绑定名称 | string ||

#### ColumnSwitch

> 开关表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Title** | 显示文字 | string ||
||||
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
||||
**Call** | 复选回调 | Func<bool `改变后check值`, object? `行元数据`, int `行`, int `列`, bool>`?` | 返回最终选中值 |
**Key** | 绑定名称 | string ||

#### ColumnSort

> 拖拽手柄列。继承于 [Column](#column)