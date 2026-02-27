[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Table
👚

> A table displays rows of data.

- DefaultProperty：Columns
- DefaultEvent：CellClick

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Gap** | Gap | int | 12 |
**Gaps** | Gap | Size | `12, 12` |
**GapCell** | Cell padding | int`?` | 6 |
**GapTree** | Tree gap | int | 12 |
**Radius** | Rounded corners | int | 0 |
**FixedHeader** | Fixed header | bool | true |
**VisibleHeader** | Display header | bool | true |
**Bordered** | Display Bordered | bool | false |
**RowHeight** | Row height | int`?` | `null` |
**RowHeightHeader** | Header row height | int`?` | `null` |
**CellImpactHeight** | Cell adjustment height | bool`?` | `null` |
||||
**CheckSize** | Checkbox size | int | 16 |
**SwitchSize** | Switch size | int | 16 |
**TreeButtonSize** | Tree switch button size | int | 16 |
**DragHandleSize** | Drag and drop handle size | int | 24 |
**DragHandleIconSize** | Drag and drop handle icon size | int | 14 |
**SortOrderSize** | Sort size | int`?` | `null` |
||||
**EnableHeaderResizing** | Manually adjust the column head width | bool | false |
**ColumnDragSort** | Column drag and drop sorting | bool | false |
**LostFocusClearSelection** | Loss of focus, clear selection | bool | false |
**MouseClickPenetration** | Mouse click penetration | bool | true |
**ScrollBarAvoidHeader** | Scrollbar starts drawing from below the header | bool | false |
**AutoSizeColumnsMode** | Column width automatic adjustment mode | [ColumnsMode](Enum.md#columnsmode) | Auto |
**VirtualMode** | Virtual mode | bool | false |
||||
**ClipboardCopy** | Copy rows | bool | true |
**ClipboardCopyFocusedCell** | Whether to enable cell copying | bool | false |
**EditMode** | Edit mode | [TEditMode](Enum.md#teditmode) | None |
**EditSelection** | Default text selection action in edit mode | [TEditSelection](Enum.md#teditselection) | None |
**EditInputStyle** | Edit mode input box style | [TEditInputStyle](Enum.md#teditinputstyle) | Default |
**EditAutoHeight** | Edit mode auto height | bool | false |
**EditLostFocus** | Lose focus to exit edit mode | bool | true |
**ShowTip** | Omit text prompts | bool | true |
**HandShortcutKeys** | Process shortcut keys | bool | true |
||||
**DefaultExpand** | Whether to expand by default `Tree` | bool | false |
**TreeArrowStyle** | Tree table arrow style | TableTreeStyle | Button |
**FilterRealTime** | Filter real-time effect | bool | false |
**AnimationTime** | Animation duration (ms) | int | 100 |
**SummaryCustomize** | Whether to enable built-in summary customization | bool | false |
**PauseLayout** | Pause layout | bool | false |
**TooltipConfig** | Omitted text prompt configuration | TooltipConfig`?` | `null` |
||||
**Empty** | Display empty style or not | bool | true |
**EmptyText** | Display text when data is empty | string | No data |
**EmptyImage** | Display image with empty data | Image`?` | `null` |
**EmptyHeader** | Is the header displayed when empty | bool | false |
||||
**ForeColor** | Text color | Color`?` | `null` |
**RowHoverBg** | Table row hover background color | Color`?` | `null` |
**RowSelectedBg** | Select background color for table rows | Color`?` | `null` |
**RowSelectedFore** | Table row selection color | Color`?` | `null` |
**BorderColor** | Table Border Color | Color`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**BorderCellWidth** | Cell border width | float | 1F |
**BorderHigh** | High-precision border (obsolete) | bool`?` | `null` |
**BorderRenderMode** | Border render mode | TableBorderMode | None |
**ColumnFont** | Header font | Font`?` | `null` |
**ColumnBack** | Header background color | Color`?` | `null` |
**ColumnFore** | Header text color | Color`?` | `null` |
||||
**CellFocusedStyle** | Focus column style | TableCellFocusedStyle`?` | `null` |
**CellFocusedBg** | Focus column background color | Color`?` | `null` |
**CellFocusedBorder** | Focus column border color | Color`?` | `null` |
||||
**SelectedIndex** | Select Index | int | -1 |
**SelectedIndexs** | Select multiple rows | int[] | |
**MultipleRows** | Enable Multiple Choice Rows | bool | false |
||||
**Columns** | Table column configuration | [ColumnCollection](TableColumn.md#column) | `null` |
**DataSource** | Data | [object](TableCell.md#icell)`?` | `Support DataTable, Class, etc` |
**Summary** | Summary column | object`?` | `null` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ScrollLine** | Scroll to the specified line | int | int i, bool force `Force scrolling` |
**ScrollLine** | Scroll to the specified line | int | object record, bool force `Force scrolling` |
**ScrollColumn** | Scroll to the specified column | int | int i, bool force `Force scrolling` |
**ScrollColumn** | Scroll to the specified column | int | string column, bool force `Force scrolling` |
**ScrollColumn** | Scroll to the specified column | int | Column column, bool force `Force scrolling` |
**ScrollToEnd** | Scroll to the bottom | void ||
**CopyData** | Copy table data | bool | int row |
**CopyData** | Copy table data | bool | int[] row |
**CopyData** | Copy table data | bool | int row, int column |
**CopyData** | Copy table data | bool | CELL cell |
**EnterEditMode** | Enter editing mode | void | int row, int column |
**SortIndex** | Get sorting sequence number | int[] ||
**SortList** | Get sorted data | object[] ||
**SortColumnsIndex** | Obtain the sorting sequence number of the header | int[] ||
**SortColumnsList** | Get header sort data | Column[] ||
**SetSortIndex** | Set sort index | void | int[] data |
**SetSortList** | Set sort list | void | object[] data |
**SetSortColumnsIndex** | Set header sort index | void | int[] data |
**GetRow** | Get the specified row data | IRow`?` | int index |
**GetRowEnable** | Get row enable | bool | int i |
**GetRowEnable** | Get row enable | bool | object record |
**SetRowEnable** | Set row enable | void | int i, bool value, bool ui `Refresh UI` |
**SetRowEnable** | Set row enable | void | object record, bool value, bool ui `Refresh UI` |
**SetSelected** | Set selected row | void | object record, bool expand `Whether to expand` |
**SelectedIndexsReal** | Get selected real rows | int[] ||
**SelectedsReal** | Get selected real row data | object[] ||
**ToDataTable** | Export table data | DataTable`?` ||
**LoadLayout** | Refresh Layout | bool ||
**Refresh** | Refresh UI | void ||
**Refresh** | Refresh UI | void | AntList<T> list |
||||
**ExpandAll** | Expand all | void ||
**Expand** | Unfold or fold | void | object record `Row metadata`, bool value `fold` |
|Merge Cell|||
**AddMergedRegion** | Add merged cells | void | CellRange range |
**AddMergedRegion** | Add multiple merged cells | void | CellRange[] ranges |
**ContainsMergedRegion** | Determine whether the merged cells exist | bool | CellRange range |
**ClearMergedRegion** | Clear all merged cells | void ||

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | bool value, object? record, int rowIndex, int columnIndex |
**CheckedOverallChanged** | Occurred when the global CheckState property value is changed | void | ColumnCheck column, CheckState value |
**CellClick** | Appears when clicking on a cell | void | MouseEventArgs args, object? record, int rowIndex, int columnIndex, Rectangle rect |
**CellDoubleClick** | Occurred when double clicking a cell | void | MouseEventArgs args, object? record, int rowIndex, int columnIndex, Rectangle rect |
**CellButtonClick** | Appears when the button is clicked | void | CellLink btn, MouseEventArgs args, object? record, int rowIndex, int columnIndex |
**CellFocused** | Occurs when a cell gains focus | void | object? record, RowType type, int rowIndex, int columnIndex, Column column, Rectangle rect |
||||
**CellBeginEdit** | Occurred before editing | bool `Return true to continue editing` | object? value, object? record, int rowIndex, int columnIndex |
**CellBeginEditInputStyle** | Text box style before editing occurs | void | object? value, object? record, int rowIndex, int columnIndex, ref Input input |
**CellEndEdit** | Occurred after editing | bool `Return true for application editing` | string value `Modified value`, object? record, int rowIndex, int columnIndex |
||||
**SetRowStyle** | Set row style | [CellStyleInfo?](#cellstyleinfo) | object? record, int rowIndex |
**SortRows** | Occurred during row sorting | void | int columnIndex |
**FilterChanged** | Occurs when filter conditions change | void | Column column |
**SummaryCustomizeChanged** | Occurred when summary customize changed | void | bool value |
**SelectIndexChanged** | Occurred when selected index changed | void | |

> Alternating background colors

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

> Customize Row Styles

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**BackColor** | Background color | Color`?` ||
**ForeColor** | Text color | Color`?` ||

----


Precautions before use：

> **Ask**: How to implement MVVM❓
> ---
> Answer: Use class inheritance of `NotifyProperty` or `INotifyPropertyChanged`, and trigger VNet (string `field name`) when `set`

> **Ask**: Why did inserting and deleting tables not trigger interface refresh❓
> --
> Answer: Use a `BindingList` as the List and use `Binding(BindingList<T> list)` to bind and implement monitoring when setting up data
> ``` csharp
> var list = new BindingList<MyClass>(10);
> for (int i = 0; i < 10; i++) list.Add(new MyClass(i));
> table.Binding(list);
> ```