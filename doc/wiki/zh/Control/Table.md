[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Table

Table 表格 👚

> 展示行列数据。

- 默认属性：Text
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
**ShowTip** | 省略文字提示 | bool | true |
**HandShortcutKeys** 🔴 | 处理快捷键 | bool | true |
||||
**DefaultExpand** | 默认是否展开 `树` | bool | false |
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
**ScrollLine** | 滚动到指定行 | void | int i |
**CopyData** | 复制表格数据 | void |int row `行`|
**CopyData** | 复制表格数据 | void |int row `行`, int column `列`|
**EnterEditMode** | 进入编辑模式 | void |int row `行`, int column `列`|
**SortIndex** | 获取排序序号 | int[] ||
**SortList** | 获取排序数据 | object[] ||
**SortColumnsIndex** | 获取表头排序序号 | int[] ||
**SortList** | 获取排序数据 | object[] ||
**ScrollLine** | 滚动到指定行 | void | int i `行`,bool force `是否强制滚动` |
**GetRowEnable** | 获取行使能 | bool | int i `行` |
**SetRowEnable** | 设置行使能 | void | int i `行`, bool value `值`, bool ui `是否刷新ui` |
**ToDataTable** | 导出表格数据 | DataTable`?` ||
**LoadLayout** | 刷新布局 | void ||
**Refresh** | 刷新界面 | void ||
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
**CellFocused** | 单元格获得焦点时发生 | void | object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
||||
**CellBeginEdit** | 编辑前发生 | bool `返回true继续编辑` | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
**CellBeginEditInputStyle** | 编辑前文本框样式发生 | void | object? value `数值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号`, ref Input input `文本框对象` |
**CellEndEdit** | 编辑后发生 | bool `返回true应用编辑` | string value `修改后值`, object? record `原始行`, int rowIndex `行序号`, int columnIndex `列序号` |
||||
**SetRowStyle** | 设置行样式 | [CellStyleInfo?](#cellstyleinfo) | object? record `原始行`, int rowIndex `行序号` |
**SortRows** | 行排序时发生 | void | int columnIndex `列序号` |
**FilterChanged** | 筛选条件更改时发生 | void | Column column `列对象` |

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