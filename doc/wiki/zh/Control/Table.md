[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Table
👚

> 展示行列数据。

- 默认属性：Columns
- 默认事件：CellClick

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Gap** | 间距 | int | 12 |
**Gaps** | 间距 | Size | `12, 12` |
**GapCell** | 单元格内间距 | int`?` | 6 |
**GapTree** | 树间距 | int | 12 |
**Radius** | 圆角 | int | 0 |
**FixedHeader** | 固定表头 | bool | true |
**VisibleHeader** | 显示表头 | bool | true |
**Bordered** | 显示列边框 | bool | false |
**RowHeight** | 行高 | int`?` | `null` |
**RowHeightHeader** | 表头行高 | int`?` | `null` |
**CellImpactHeight** | 单元格调整高度 | bool`?` | `null` |
||||
**CheckSize** | 复选框大小 | int | 16 |
**SwitchSize** | 开关大小 | int | 16 |
**TreeButtonSize** | 树开关按钮大小 | int | 16 |
**DragHandleSize** | 拖拽手柄大小 | int | 24 |
**DragHandleIconSize** | 拖拽手柄图标大小 | int | 14 |
**SortOrderSize** | 排序大小 | int`?` | `null` |
||||
**EnableHeaderResizing** | 手动调整列头宽度 | bool | false |
**ColumnDragSort** | 列拖拽排序 | bool | false |
**LostFocusClearSelection** | 焦点离开清空选中 | bool | false |
**MouseClickPenetration** | 鼠标点击穿透 | bool | true |
**ScrollBarAvoidHeader** | 滚动条从表头下方开始绘制 | bool | false |
**AutoSizeColumnsMode** | 列宽自动调整模式 | [ColumnsMode](Enum.md#columnsmode) | Auto |
**VirtualMode** | 虚拟模式 | bool | false |
||||
**ClipboardCopy** | 行复制 | bool | true |
**ClipboardCopyFocusedCell** | 是否启用单元格复制 | bool | false |
**EditMode** | 编辑模式 | [TEditMode](Enum.md#teditmode) | None |
**EditSelection** | 编辑模式下的默认文本选择动作 | [TEditSelection](Enum.md#teditselection) | None |
**EditInputStyle** | 编辑模式输入框样式 | [TEditInputStyle](Enum.md#teditinputstyle) | Default |
**EditAutoHeight** | 编辑模式自动高度 | bool | false |
**EditLostFocus** | 失去焦点退出编辑模式 | bool | true |
**ShowTip** | 省略文字提示 | bool | true |
**HandShortcutKeys** | 处理快捷键 | bool | true |
||||
**DefaultExpand** | 默认是否展开 `树` | bool | false |
**TreeArrowStyle** | 树表格的箭头样式 | TableTreeStyle | Button |
**FilterRealTime** | 筛选实时生效 | bool | false |
**AnimationTime** | 动画时长（ms） | int | 100 |
**SummaryCustomize** | 是否启用内置汇总定制功能 | bool | false |
**PauseLayout** | 暂停布局 | bool | false |
**TooltipConfig** | 超出文字提示配置 | TooltipConfig`?` | `null` |
||||
**Empty** | 是否显示空样式 | bool | true |
**EmptyText** | 数据为空显示文字 | string | No data |
**EmptyImage** | 数据为空显示图片 | Image`?` | `null` |
**EmptyHeader** | 空是否显示表头 | bool | false |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**RowHoverBg** | 表格行悬浮背景色 | Color`?` | `null` |
**RowSelectedBg** | 表格行选中背景色 | Color`?` | `null` |
**RowSelectedFore** | 表格行选中字色 | Color`?` | `null` |
**BorderColor** | 表格边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**BorderCellWidth** | 单元格边框宽度 | float | 1F |
**BorderHigh** | 高精度边框（已过时） | bool`?` | `null` |
**BorderRenderMode** | 边框渲染模式 | TableBorderMode | None |
**ColumnFont** | 表头字体 | Font`?` | `null` |
**ColumnBack** | 表头背景色 | Color`?` | `null` |
**ColumnFore** | 表头文本色 | Color`?` | `null` |
||||
**CellFocusedStyle** | 焦点列样式 | TableCellFocusedStyle`?` | `null` |
**CellFocusedBg** | 焦点列背景色 | Color`?` | `null` |
**CellFocusedBorder** | 焦点列边框色 | Color`?` | `null` |
||||
**SelectedIndex** | 选中行 | int | -1 |
**SelectedIndexs** | 选中多行 | int[] | |
**MultipleRows** | 多选行 | bool | false |
||||
**Columns** | 表格列的配置 | [ColumnCollection](TableColumn.md#column) | `null` |
**DataSource** | 数据数组 | [object](TableCell.md#icell)`?` | `支持DataTable，Class等` |
**Summary** | 总结栏 | object`?` | `null` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedIndexsReal** | 选中真实行 | int[] ||
**SelectedsReal** | 选中真实行数据 | object[] ||
**GetRow** | 获取指定行的数据 | IRow`?` | int index `序号` |
**RowCount** | 行总数 | int ||
**Refresh** | 刷新界面 | void ||
**Refresh<T>** | 刷新界面（适用AntList<T>场景） | void | AntList<T>? list `数据列表` |
**SetSelected** | 设置选中行 | void | object record `行数据`, bool expand `是否展开关联父级` |
**GetRowEnable** | 获取行使能 | bool | int i `行` |
**GetRowEnable** | 获取行使能 | bool | object record `行对象` |
**SetRowEnable** | 设置行使能 | void | int i `行`, bool value `值`, bool ui `是否刷新ui` |
**SetRowEnable** | 设置行使能 | void | object record `行对象`, bool value `值`, bool ui `是否刷新ui` |
**ScrollLine** | 滚动到指定行 | int `返回滚动量` | int i `行`, bool force `是否强制滚动` |
**ScrollLine** | 滚动到指定行 | int `返回滚动量` | object record `行对象`, bool force `是否强制滚动` |
**ScrollToEnd** | 内容滚动到最下面 | void ||
**ScrollColumn** | 滚动到指定列 | int `返回滚动量` | int i `列`, bool force `是否强制滚动` |
**ScrollColumn** | 滚动到指定列 | int `返回滚动量` | string column `表头key`, bool force `是否强制滚动` |
**ScrollColumn** | 滚动到指定列 | int `返回滚动量` | Column column `表头`, bool force `是否强制滚动` |
**CopyData** | 复制表格数据 | bool | int row `行` |
**CopyData** | 复制表格数据 | bool | int[] row `行数组` |
**CopyData** | 复制表格数据 | bool | int row `行`, int column `列` |
**CopyData** | 复制表格数据 | bool | CELL cell `单元格` |
**SortIndex** | 获取排序序号 | int[] ||
**SetSortIndex** | 设置排序序号 | void | int[]? data `序号数据` |
**SortList** | 获取排序数据 | object[] ||
**SetSortList** | 设置排序数据 | void | object[]? data `排序数据` |
**SortColumnsIndex** | 获取表头排序序号 | int[] ||
**SetSortColumnsIndex** | 设置表头排序序号 | void | int[]? data `序号数据` |
**SortColumnsList** | 获取表头排序数据 | Column[] ||
**GetColumnRealIndex** | 获取表头真实索引 | int | Column column `表头` |
**ToDataTable** | 导出表格数据 | DataTable`?` ||
**LoadLayout** | 刷新布局 | void ||
**EnterEditMode** | 进入编辑模式 | void | int row `行`, int column `列` |
||||
**ExpandAll** | 展开全部 | void ||
**Expand** | 展开或折叠 | void | object record `行元数据`, bool value `折叠值` |
|合并单元格|||
**AddMergedRegion** | 新增合并单元格 | void | CellRange range |
**AddMergedRegion** | 新增多个合并单元格 | void | CellRange[] ranges |
**ContainsMergedRegion** | 判断合并单元格是否存在 | bool | CellRange range |
**ClearMergedRegion** | 清空全部合并单元格 | void ||

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**CheckedChanged** | Checked 属性值更改时发生 | void | bool value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CheckedOverallChanged** | 全局 CheckState 属性值更改时发生 | void | ColumnCheck column `表头对象`, CheckState value `数值` |
**CellClick** | 单击时发生 | void | MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Rectangle rect `表格区域` |
**CellDoubleClick** | 双击时发生 | void | MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Rectangle rect `表格区域` |
**CellButtonClick** | 单击按钮时发生 | void | CellLink btn `触发按钮`, MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellFocused** | 单元格获得焦点时发生 | void | object? record `原始行`, RowType type `行类型`, int rowIndex `行序号`, int columnIndex `列序号`, Column column `列对象`, Rectangle rect `表格区域` |
||||
**CellBeginEdit** | 编辑前发生 | bool `返回true继续编辑` | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellBeginEditInputStyle** | 编辑前文本框样式发生 | void | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, ref Input input `文本框对象` |
**CellEndEdit** | 编辑后发生 | bool `返回true应用编辑` | string value `修改后值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
||||
**SetRowStyle** | 设置行样式 | [CellStyleInfo?](#cellstyleinfo) | object? record `原始行`, int rowIndex `行序号` |
**SortRows** | 行排序时发生 | void | int columnIndex `列序号` |
**FilterChanged** | 筛选条件更改时发生 | void | Column column `列对象` |
**SummaryCustomizeChanged** | 汇总定制功能状态更改时发生 | void | bool value `状态值` |
**SelectIndexChanged** | 选中索引更改时发生 | void | |

> 奇偶交替背景色

```csharp
private AntdUI.Table.CellStyleInfo? table1_SetRowStyle(object sender, AntdUI.TableSetRowStyleEventArgs e)
{
	if (e.Index % 2 == 0)
	{
		return new AntdUI.Table.CellStyleInfo
		{
			BackColor = Color.WhiteSmoke
		};
	}
	return null;
}
```

### CellStyleInfo

> 自定义行样式

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**BackColor** | 背景颜色 | Color`?` ||
**ForeColor** | 文字颜色 | Color`?` ||

----


使用之前的注意事项：

> **问**：如何实现MVVM❓
> ---
> 答：使用类继承 `NotifyProperty` OR `INotifyPropertyChanged`，并在`set`时触发 OnPropertyChanged(string `字段名称`)

> **问**：为何插入、删除表格没有触发界面刷新❓
> ---
> 答：使用 `BindingList` 作为List，**并在设置数据时**使用`Binding(BindingList<T> list)`来绑定实现监控
> ``` csharp
> var list = new BindingList<我的类>(10);
> for (int i = 0; i < 10; i++) list.Add(new 我的类(i));
> table.Binding(list);
> ```