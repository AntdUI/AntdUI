[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Table
👚

> 展示行列数据。

- 默认属性：Columns
- 默认事件：CellClick

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**DataSource** | 数据数组 | [object](TableCell.md#icell)`?` | `支持DataTable，Class等` |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ColumnFont** | 表头字体 | Font`?` | `null` |
**ColumnFore** | 表头文本色 | Color`?` | `null` |
**RowSelectedFore** | 表格行选中字色 | Color`?` | `null` |
||||
**Back** | 背景颜色 | Color`?` | `null` |
**ColumnBack** | 表头背景色 | Color`?` | `null` |
||||
**DragHandleIconSize** | 拖拽手柄图标大小 | int | 14 |
||||
**AnimationTime** | 动画时长（ms） | int | 100 |
||||
**SelectedIndex** | 选中行 | int | -1 |
**AutoSizeColumnsMode** | 列宽自动调整模式 | [ColumnsMode](Enum.md#columnsmode) | Auto |
**BorderRenderMode** | 边框渲染模式 | TableBorderMode | None |
**CheckSize** | 复选框大小 | int | 16 |
**ColumnDragSort** | 列拖拽排序 | bool | false |
**DragHandleSize** | 拖拽手柄大小 | int | 24 |
**EditAutoHeight** | 编辑模式自动高度 | bool | false |
**EditMode** | 编辑模式 | [TEditMode](Enum.md#teditmode) | None |
**EditSelection** | 编辑模式下的默认文本选择动作 | [TEditSelection](Enum.md#teditselection) | None |
**EnableHeaderResizing** | 手动调整列头宽度 | bool | false |
**FilterShowCheckBg** | 筛选显示复选背景 | bool`?` | `null` |
**MouseClickPenetration** | 鼠标点击穿透 | bool | true |
**MultipleRows** | 多选行 | bool | false |
**PauseLayout** | 暂停布局 | bool | false |
**RowSelectedBg** | 表格行选中背景色 | Color`?` | `null` |
**SelectedIndexs** | 选中多行 | int[] | |
**ShowCheckBg** | 显示复选背景 | bool | true |
**ShowTip** | 省略文字提示 | bool | true |
**VirtualMode** | 虚拟模式（大数据量时仅渲染可见行） | bool | false |
**VisibleHeader** | 显示表头 | bool | true |
**RowHoverBg** | 表格行悬浮背景色 | Color`?` | `null` |
**CellFocusedBg** | 焦点列背景色 | Color`?` | `null` |
**CellFocusedBorder** | 焦点列边框色 | Color`?` | `null` |
**CellFocusedStyle** | 焦点列样式 | TableCellFocusedStyle`?` | `null` |
**ClipboardCopyFocusedCell** | 是否启用单元格复制 | bool | false |
**EditLostFocus** | 失去焦点退出编辑模式 | bool | true |
**EnableFocusNavigation** | 是否启用焦点自动跳转 | bool | false |
**FocusNavigationAutoScroll** | 焦点跳转时自动滚动到新行 | bool | true |
**FocusNavigationAutoSelectRow** | 焦点跳转时自动选中行 | bool | true |
**LostFocusClearSelection** | 焦点离开清空选中 | bool | false |
||||
**BorderColor** | 表格边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**Radius** | 圆角 | int | 0 |
**BorderCellWidth** | 单元格边框宽度 | float | 1F |
**Bordered** | 显示列边框 | bool | false |
**BorderHigh** | 高精度边框（已过时） | bool`?` | `null` |
||||
**CellImpactHeight** | 单元格调整高度 | bool`?` | `null` |
**Columns** | 表格列的配置 | [ColumnCollection](TableColumn.md#column) | `null` |
**FixedHeader** | 固定表头 | bool | true |
**Gap** | 间距 | int | 12 |
**GapCell** | 单元格内间距 | int`?` | 6 |
**Gaps** | 间距 | Size | `12, 12` |
**GapTree** | 树间距 | int | 12 |
**RowHeight** | 行高 | int`?` | `null` |
**RowHeightHeader** | 表头行高 | int`?` | `null` |
**ScrollBarAvoidHeader** | 滚动条从表头下方开始绘制（避免遮挡表头） | bool | false |
**SortOrderSize** | 排序大小 | int`?` | `null` |
**StackedHeaderRows** | 堆叠表头行 | StackedHeaderRow[]`?` | `null` |
**SwitchSize** | 开关大小 | int | 16 |
**TreeArrowStyle** | 树表格的箭头样式 | TableTreeStyle | Button |
**TreeButtonSize** | 树开关按钮大小 | int | 16 |
||||
**ClipboardCopy** | 行复制 | bool | true |
**DefaultExpand** | 默认是否展开 `树` | bool | false |
**EditInputStyle** | 编辑模式输入框样式 | [TEditInputStyle](Enum.md#teditinputstyle) | Default |
**Empty** | 是否显示空样式 | bool | true |
**EmptyHeader** | 空是否显示表头 | bool | false |
**EmptyImage** | 数据为空显示图片 | Image`?` | `null` |
**EmptyText** | 数据为空显示文字 | string | No data |
**FilterRealTime** | 筛选实时生效 | bool | false |
**FilterSortOrder** | 筛选启用排序 | bool`?` | `null` |
**HandShortcutKeys** | 处理快捷键 | bool | true |
**Summary** | 总结栏 | object`?` | `null` |
**SummaryCustomize** | 是否启用内置汇总定制功能 | bool | false |
**TooltipConfig** | 超出文字提示配置 | TooltipConfig`?` | `null` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedIndexsReal** | 选中真实行 | int[] ||
**SelectedsReal** | 选中真实行数据 | object[] ||
**SelectedsTree** | 选中真实行数据（适配树表格折叠展开） | object[] ||
**GetVisibleRange** | 获取可见区域行范围（0开始，不含表头） | int[]? | |
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
**Sort** | 按列SortMode排序 | void | Column col `列` |
**SortColumnsIndex** | 获取表头排序序号 | int[] ||
**SetSortColumnsIndex** | 设置表头排序序号 | void | int[]? data `序号数据` |
**SortColumnsList** | 获取表头排序数据 | Column[] ||
**GetColumnRealIndex** | 获取表头真实索引 | int | Column column `表头` |
**ToDataTable** | 导出表格数据 | DataTable`?` ||
**LoadLayout** | 刷新布局 | void ||
**EnterEditMode** | 进入编辑模式 | void | int row `行`, int column `列` |
**EnterEditMode** | 进入行编辑模式：一行多个单元格同时进入编辑状态 | bool | int row `行`, params int[] columns `列索引集合` |
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
**NonCellClick** | 单击空白时发生 | void | MouseEventArgs args `点击` |
**NonCellDoubleClick** | 双击空白时发生 | void | MouseEventArgs args `点击` |
**CellClickBegin** | 单击之前发生 | void | MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Rectangle rect `表格区域`, bool handled `是否处理` |
**CellHover** | 滑动时发生 | void | MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Rectangle? rect `表格区域` |
**CellButtonClick** | 单击按钮时发生 | void | CellLink btn `触发按钮`, MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellButtonDown** | 按下按钮时发生 | void | CellLink btn `触发按钮`, MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellButtonUp** | 放下按钮时发生 | void | CellLink btn `触发按钮`, MouseEventArgs args `点击`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellFocused** | 单元格获得焦点时发生 | void | object? record `原始行`, RowType type `行类型`, int rowIndex `行序号`, int columnIndex `列序号`, Column column `列对象`, Rectangle rect `表格区域` |
||||
**CellBeginEdit** | 编辑前发生 | bool `返回true继续编辑` | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellBeginEditInputStyle** | 编辑前文本框样式发生 | void | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, ref Input input `文本框对象` |
**CellEndEdit** | 编辑后发生 | bool `返回true应用编辑` | string value `修改后值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellEndValueEdit** | 编辑后发生 | bool `返回true应用编辑` | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellEditComplete** | 编辑完成后发生 | void | object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Column column `列对象` |
**CellEditEnter** | 单元格输入模式下按下回车键时发生 | void | object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, Column column `列对象` |
||||
**SetRowStyle** | 设置行样式 | [CellStyleInfo?](#cellstyleinfo) | object? record `原始行`, int rowIndex `行序号` |
|||||
**SortRows** | 行排序时发生 | void | int columnIndex `列序号` |
**SortModeChanged** | 点击排序后发生 | bool `返回true继续排序` | SortMode sortMode `排序方式`, Column column `表头` |
**CustomSort** | 自定义排序（Comparison\<string\>） | int `负数x在前、正数y在前、0相等` | string x `值x`, string y `值y` |
**SortRowsTree** | 树行排序时发生 | void | object record `原始行`, int from `原位置`, int to `目标位置`, bool handled `是否处理` |
**ExpandChanged** | 展开改变时发生 | void | object record `原始行`, bool expand `是否展开` |
|||||
**ColumnIndexChanging** | 列拖放到新位置时发生（Cancel=true 时取消） | void | int indexSource `原位置`, int indexSourceReal `真实原位置索引`, int indexTarget `目标位置`, bool cancel `是否取消拖放` |
**ColumnIndexChanged** | 列拖放到新位置后发生 | void | int indexSource `原位置`, int indexSourceReal `真实原位置索引`, int indexTarget `目标位置` |
|||||
**FilterPopupBegin** | 筛选窗口弹出前发生 | void | Column column `筛选列`, FilterOption? option `筛选选项`, IList<object>? customSource `当前列的自定义数据源`, Font? font `筛选栏字体`, int? height `筛选栏高度`, bool cancel `是否取消弹出` |
**FilterPopupEnd** | 筛选窗口关闭前发生 | void | FilterOption option `当前筛选参数`, object[]? records `筛选后的记录`, bool cancel `是否取消` |
**FilterDataChanged** | 筛选数据变更后发生 | void | object[]? records `筛选后的记录` |
|||||
**SummaryCustomizeChanged** | 汇总定制功能状态更改时发生 | void | bool value `状态值` |
**CustomSummaryCalculate** | 每行或行自定义汇总计算结束时发生 | void | Column column `当前汇总的列`, object record `汇总的行对象`, int rowIndex `当前汇总行索引`, bool finalize `是否为完成时的汇总运算`, double? totalValue `可设置自定义汇总值` |
**SelectIndexChanged** | 选中索引更改时发生 | void | |
||||
**RowPaint** | 绘制行时发生 | void | Canvas g `画板`, Rectangle rect `区域`, object? record `原始行`, RowType type `行类型`, int rowIndex `行序号` |
**RowPaintBegin** | 绘制行前发生 | void | Canvas g `画板`, Rectangle rect `区域`, object? record `原始行`, RowType type `行类型`, int rowIndex `行序号` |
**CellPaint** | 绘制单元格时发生 | void | Canvas g `画板`, Rectangle rect `区域`, Rectangle rectreal `真实区域`, object? record `原始行`, RowType type `行类型`, int rowIndex `行序号`, int index `序号`, Column column `列对象` |
**CellPaintBegin** | 绘制单元格之前发生 | void | Canvas g `画板`, Rectangle rect `区域`, Rectangle rectreal `真实区域`, object? record `原始行`, RowType type `行类型`, int rowIndex `行序号`, int index `序号`, Column column `列对象`, bool handled `是否处理`, SolidBrush? cellFore `单元格前景色`, Brush? cellBack `单元格背景笔刷`, Font? cellFont `单元格字体` |

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
**ForeColor** | 文字颜色 | Color`?` ||
||||
**BackColor** | 背景颜色 | Color`?` ||

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