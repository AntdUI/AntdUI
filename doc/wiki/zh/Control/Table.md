[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Table

Table 表格 👚

> 展示行列数据。

- 默认属性：Text
- 默认事件：CellClick

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Gap** | 间距 | int | 12 |
**Radius** 🔴 | 圆角 | int | 0 |
**CheckSize** | 复选框大小 | int | 16 |
**SwitchSize** 🔴 | 开关大小 | int | 16 |
**TreeButtonSize** 🔴 | 树开关按钮大小 | int | 16 |
**FixedHeader** | 固定表头 | bool | true |
**VisibleHeader** 🔴 | 显示表头 | bool | true |
**Bordered** | 显示列边框 | bool | false |
**RowHeight** 🔴 | 行高 | int`?` | `null` |
**RowHeightHeader** 🔴 | 表头行高 | int`?` | `null` |
||||
**EnableHeaderResizing** | 手动调整列头宽度 | bool | false |
**ColumnDragSort** | 列拖拽排序 | bool | false |
**LostFocusClearSelection** | 焦点离开清空选中 | bool | false |
**AutoSizeColumnsMode** 🔴 | 列宽自动调整模式 | [ColumnsMode](Enum#columnsmode) | Auto |
||||
**ClipboardCopy** | 行复制 | bool | true |
**EditMode** | 编辑模式 | [TEditMode](Enum#teditmode) | None |
**ShowTip** | 省略文字提示 | bool | true |
**DefaultExpand** 🔴 | 默认是否展开 `树` | bool | false |
||||
**Empty** | 是否显示空样式 | bool | true |
**EmptyText** | 数据为空显示文字 | string | No data |
**EmptyImage** | 数据为空显示图片 | Image`?` | `null` |
**EmptyHeader** | 空是否显示表头 | bool | false |
||||
**RowSelectedBg** | 表格行选中背景色 | Color`?` | `null` |
**RowSelectedFore** 🔴 | 表格行选中字色 | Color`?` | `null` |
**BorderColor** 🔴 | 表格边框颜色 | Color`?` | `null` |
**ColumnFont** 🔴 | 表头字体 | Font`?` | `null` |
**ColumnBack** 🔴 | 表头背景色 | Color`?` | `null` |
**ColumnFore** 🔴 | 表头文本色 | Color`?` | `null` |
||||
**SelectedIndex** | 选中行 | int | -1 |
||||
**Columns** | 表格列的配置 | [ColumnCollection](#column) | `null` |
**DataSource** | 数据数组 | [object](#datasource)`?` | `支持DataTable，Class等` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ScrollLine** | 滚动到指定行 | void | int i |
**CopyData** | 复制表格数据 | void |int row `行`|
**CopyData** | 复制表格数据 | void |int row `行`, int column `列`|
**EnterEditMode** | 进入编辑模式 | void |int row `行`, int column `列`|
**SortIndex** 🔴 | 获取排序序号 | int[] ||
**SortList** 🔴 | 获取排序数据 | object[] ||

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**CheckedChanged** | Checked 属性值更改时发生 | void | bool value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CheckedOverallChanged** | 全局 CheckState 属性值更改时发生 | void | ColumnCheck column `表头对象`, CheckState value `数值` |
**CellClick** | 单击时发生 | void | MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Rectangle rect `表格区域` |
**CellDoubleClick** | 双击时发生 | void | MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Rectangle rect `表格区域` |
**CellButtonClick** | 单击按钮时发生 | void | CellLink btn `触发按钮`, MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
||||
**CellBeginEdit** | 编辑前发生 | bool `返回true继续编辑` | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellBeginEditInputStyle** | 编辑前文本框样式发生 | void | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, ref Input input `文本框对象` |
**CellEndEdit** | 编辑后发生 | bool `返回true应用编辑` | string value `修改后值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
||||
**SetRowStyle** | 设置行样式 | [CellStyleInfo?](#cellstyleinfo) | object? record `原始行`, int rowIndex `行序号` |
**SortRows** 🔴 | 行排序时发生 | void | int columnIndex `列序号` |

> 奇偶交替背景色

```csharp
private AntdUI.Table.CellStyleInfo? Table1_SetRowStyle(object sender, object? record, int rowIndex)
{
    if (rowIndex % 2 == 0)
    {
        return new AntdUI.Table.CellStyleInfo
        {
            BackColor = Color.WhiteSmoke
        };
    }
    return null;
}
```

----

### Column

> 多样表头

名称 | 描述 | 类型 | 必填 | 默认值 |
:--|:--|:--|:--:|:--|
**Key** | 绑定名称 | string |✅||
**Title** | 显示文字 | string |✅||
|||||
**Visible** 🔴 | 是否显示 | bool|⛔|true|
**Align** | 对齐方式 | ColumnAlign |⛔|ColumnAlign.Left|
**ColAlign** 🔴 | 表头对齐方式 | ColumnAlign`?` |⛔| `null` |
**Width** | 列宽度 | string`?` |⛔||
**MaxWidth** 🔴 | 列最大宽度 | string`?` |⛔||
|||||
**Fixed** | 列是否固定 | bool |⛔|false|
**Ellipsis** | 超过宽度将自动省略 | bool |⛔|false|
**LineBreak** 🔴 | 自动换行 | bool |⛔|false|
**SortOrder** 🔴 | 启用排序 | bool |⛔|false|
**KeyTree** 🔴 | 树形列 | string`?` |⛔||

#### ColumnCheck

> 复选框表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 必填 | 默认值 |
:--|:--|:--|:--:|:--|
**Key** | 绑定名称 | string |✅||

#### ColumnRadio

> 单选框表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 必填 | 默认值 |
:--|:--|:--|:--:|:--|
**Key** | 绑定名称 | string |✅||
**Title** | 显示文字 | string |✅||

#### ColumnSwitch

> 开关表头。继承于 [Column](#column)

名称 | 描述 | 类型 | 必填 | 默认值 |
:--|:--|:--|:--:|:--|
**Key** | 绑定名称 | string |✅||
**Title** | 显示文字 | string |✅||
**Call** | 改变回调 | Func<bool, object?, int, int, bool>`?` |||


----


### DataSource

> 丰富的单元格

#### CellText

> 文字

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Back** | 背景颜色 | Color`?` ||
**Font** | 字体 | Font`?` ||
||||
**IconRatio** 🔴 | 图标比例 | float | 0.7F |
**Prefix** 🔴 | 前缀 | Image`?` ||
**PrefixSvg** 🔴 | 前缀SVG | string`?` ||
**Suffix** 🔴 | 后缀 | Image`?` ||
**SuffixSvg** 🔴 | 后缀SVG | string`?` ||
||||
**Text** | 文本 | string`?` ||

#### CellBadge

> 徽标

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Fill** | 颜色 | Color`?` ||
||||
**State** | 状态 | [TState](Enum#tstate) | Default |
**Text** | 文本 | string`?` |

#### CellTag

> 标签

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Back** | 背景颜色 | Color`?` ||
**BorderWidth** | 边框宽度 | float |1F|
||||
**Type** | 类型 | [TTypeMini](Enum#ttypemini) | Default |
**Text** | 文本 | string`?` ||

#### CellImage

> 图片

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**BorderColor** | 边框颜色 | Color`?` ||
**BorderWidth** | 边框宽度 | float |0F|
**Radius** | 圆角 | int |6|
||||
**Round** | 圆角样式 | bool |false|
**Size** | 自定义大小 | Size`?` ||
||||
**Image** | 图片 | Image`?` | `null` |
**ImageSvg** | 图片SVG | string`?` | `null` |
**FillSvg** | SVG填充颜色 | Color`?` ||
**ImageFit** | 图片布局 | [TFit](Enum#tfit) | Fill |
||||
**Tooltip** 🔴 | 文本提示 | string`?` ||

#### CellButton

> 按钮，继承于 [CellLink](#celllink)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Fore** | 字体颜色 | Color`?` ||
**Back** | 背景颜色 | Color`?` ||
**BackHover** | 悬停背景颜色 | Color`?` ||
**BackActive** | 激活背景颜色 | Color`?` ||
||||
**DefaultBack** 🔴 | Default模式背景颜色 | Color`?` ||
**DefaultBorderColor** 🔴 | Default模式边框颜色 | Color`?` ||
||||
**Radius** | 圆角 | int |6|
**BorderWidth** | 边框宽度 | float |0F|
||||
**IconRatio** 🔴 | 图标比例 | float | 0.7F |
**Image** 🔴 | 图像 | Image`?` | `null` |
**ImageSvg** 🔴 | 图像SVG | string`?` | `null` |
**ImageHover** 🔴 | 悬停图像 | Image`?` | `null` |
**ImageHoverSvg** 🔴 | 悬停图像SVG | string`?` | `null` |
**ImageHoverAnimation** 🔴 | 悬停图像动画时长 | int | 200 |
||||
**Shape** | 形状 | [TShape](Enum#tshape) | Default |
**Ghost** | 幽灵属性 `使按钮背景透明` | bool |false |
**ShowArrow** | 显示箭头 | bool |false |
**IsLink** | 箭头链接样式 | bool |false |
||||
**Type** | 类型 | [TTypeMini](Enum#ttypemini) | Default |
**Text** | 文本 | string`?` ||

#### CellLink

> 按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Id** | ID | string ||
**Enabled** | 启用 | bool |true|
||||
**Text** | 文本 | string`?` ||
**TextAlign** | 文本位置 | ContentAlignment | MiddleCenter |
||||
**Tooltip** 🔴 | 文本提示 | string`?` ||

#### CellProgress

> 进度条

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Back** | 背景颜色 | Color`?` ||
**Fill** | 进度条颜色 | Color`?` ||
||||
**Radius** | 圆角 | int |6|
**Shape** | 形状 | [TShape](Enum#tshape) | Default |
||||
**Value** | 进度条 `0.0-1.0` | float |0F|

#### CellDivider 🔴

> 分割线

----

### CellStyleInfo

> 自定义行样式

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**BackColor** | 背景颜色 | Color`?` ||
**ForeColor** 🔴 | 文字颜色 | Color`?` ||

----


使用之前的注意事项：

> **问**：如何实现MVVM❓
> 答：使用类继承 `NotifyProperty` OR `INotifyPropertyChanged`，并在`set`时触发 OnPropertyChanged(string `字段名称`)

> **问**：为何插入、删除表格没有触发界面刷新❓
> 答：使用 `BindingList` 或 `AntList` 作为List，**并在设置数据时**使用`Binding(AntList<T> list)`来绑定实现监控
``` csharp
var list = new AntdUI.AntList<我的类>(10);
for (int i = 0; i < 10; i++) list.Add(new 我的类(i));
table.Binding(list);
```
### 
