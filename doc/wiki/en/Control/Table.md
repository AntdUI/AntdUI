[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Table
👚

> A table displays rows of data.

- DefaultProperty：Columns
- DefaultEvent：CellClick

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**DataSource** | Data | [object](TableCell.md#icell)`?` | `Support DataTable, Class, etc` |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ColumnFont** | Header font | Font`?` | `null` |
**ColumnFore** | Header text color | Color`?` | `null` |
**RowSelectedFore** | Table row selection color | Color`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**ColumnBack** | Header background color | Color`?` | `null` |
||||
**DragHandleIconSize** | Drag and drop handle icon size | int | 14 |
||||
**AnimationTime** | Animation duration (ms) | int | 100 |
||||
**SelectedIndex** | Select Index | int | -1 |
**AutoSizeColumnsMode** | Column width automatic adjustment mode | [ColumnsMode](Enum.md#columnsmode) | Auto |
**BorderRenderMode** | Border render mode | TableBorderMode | None |
**CheckSize** | Checkbox size | int | 16 |
**ColumnDragSort** | Column drag and drop sorting | bool | false |
**DragHandleSize** | Drag and drop handle size | int | 24 |
**EditAutoHeight** | Edit mode auto height | bool | false |
**EditMode** | Edit mode | [TEditMode](Enum.md#teditmode) | None |
**EditSelection** | Default text selection action in edit mode | [TEditSelection](Enum.md#teditselection) | None |
**EnableHeaderResizing** | Manually adjust the column head width | bool | false |
**FilterShowCheckBg** | Filter display checkbox background | bool`?` | `null` |
**MouseClickPenetration** | Mouse click penetration | bool | true |
**MultipleRows** | Enable Multiple Choice Rows | bool | false |
**PauseLayout** | Pause layout | bool | false |
**RowSelectedBg** | Select background color for table rows | Color`?` | `null` |
**SelectedIndexs** | Select multiple rows | int[] | |
**ShowCheckBg** | Display checkbox background | bool | true |
**ShowTip** | Omit text prompts | bool | true |
**VirtualMode** | Virtual mode (only visible rows are rendered for large data) | bool | false |
**VisibleHeader** | Display header | bool | true |
**RowHoverBg** | Table row hover background color | Color`?` | `null` |
**CellFocusedBg** | Focus column background color | Color`?` | `null` |
**CellFocusedBorder** | Focus column border color | Color`?` | `null` |
**CellFocusedStyle** | Focus column style | TableCellFocusedStyle`?` | `null` |
**ClipboardCopyFocusedCell** | Whether to enable cell copying | bool | false |
**EditLostFocus** | Lose focus to exit edit mode | bool | true |
**EnableFocusNavigation** | Whether to enable automatic focus navigation | bool | false |
**FocusNavigationAutoScroll** | Auto scroll to new row on focus navigation | bool | true |
**FocusNavigationAutoSelectRow** | Auto select row on focus navigation | bool | true |
**LostFocusClearSelection** | Loss of focus, clear selection | bool | false |
||||
**BorderColor** | Table Border Color | Color`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**Radius** | Rounded corners | int | 0 |
**BorderCellWidth** | Cell border width | float | 1F |
**Bordered** | Display Bordered | bool | false |
**BorderHigh** | High-precision border (obsolete) | bool`?` | `null` |
||||
**CellImpactHeight** | Cell adjustment height | bool`?` | `null` |
**Columns** | Table column configuration | [ColumnCollection](TableColumn.md#column) | `null` |
**FixedHeader** | Fixed header | bool | true |
**Gap** | Gap | int | 12 |
**GapCell** | Cell padding | int`?` | 6 |
**Gaps** | Gap | Size | `12, 12` |
**GapTree** | Tree gap | int | 12 |
**RowHeight** | Row height | int`?` | `null` |
**RowHeightHeader** | Header row height | int`?` | `null` |
**ScrollBarAvoidHeader** | Scrollbar starts drawing from below the header (avoid blocking the header) | bool | false |
**SortOrderSize** | Sort size | int`?` | `null` |
**StackedHeaderRows** | Stacked header rows | StackedHeaderRow[]`?` | `null` |
**SwitchSize** | Switch size | int | 16 |
**TreeArrowStyle** | Tree table arrow style | TableTreeStyle | Button |
**TreeButtonSize** | Tree switch button size | int | 16 |
||||
**ClipboardCopy** | Copy rows | bool | true |
**DefaultExpand** | Whether to expand by default `Tree` | bool | false |
**EditInputStyle** | Edit mode input box style | [TEditInputStyle](Enum.md#teditinputstyle) | Default |
**Empty** | Display empty style or not | bool | true |
**EmptyHeader** | Is the header displayed when empty | bool | false |
**EmptyImage** | Display image with empty data | Image`?` | `null` |
**EmptyText** | Display text when data is empty | string | No data |
**FilterRealTime** | Filter real-time effect | bool | false |
**FilterSortOrder** | Filter enable sorting | bool`?` | `null` |
**HandShortcutKeys** | Process shortcut keys | bool | true |
**Summary** | Summary column | object`?` | `null` |
**SummaryCustomize** | Whether to enable built-in summary customization | bool | false |
**TooltipConfig** | Omitted text prompt configuration | TooltipConfig`?` | `null` |

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
**EnterEditMode** | Enter row edit mode (multiple cells in one row edit simultaneously) | bool | int row, params int[] columns |
**SortIndex** | Get sorting sequence number | int[] ||
**SortList** | Get sorted data | object[] ||
**Sort** | Sort by column SortMode | void | Column col `Column` |
**SortColumnsIndex** | Obtain the sorting sequence number of the header | int[] ||
**SortColumnsList** | Get header sort data | Column[] ||
**GetColumnRealIndex** | Get the real index of the header | int | Column column `Header` |
**SetSortIndex** | Set sort index | void | int[] data |
**SetSortList** | Set sort list | void | object[] data |
**SetSortColumnsIndex** | Set header sort index | void | int[] data |
**GetRow** | Get the specified row data | IRow`?` | int index |
**RowCount** | Total number of rows | int ||
**GetRowEnable** | Get row enable | bool | int i |
**GetRowEnable** | Get row enable | bool | object record |
**SetRowEnable** | Set row enable | void | int i, bool value, bool ui `Refresh UI` |
**SetRowEnable** | Set row enable | void | object record, bool value, bool ui `Refresh UI` |
**SetSelected** | Set selected row | void | object record, bool expand `Whether to expand` |
**SelectedIndexsReal** | Get selected real rows | int[] ||
**SelectedsReal** | Get selected real row data | object[] ||
**SelectedsTree** | Get selected real row data (adapts to tree expand/collapse) | object[] ||
**GetVisibleRange** | Get the row range of the visible area (0-based, header excluded) | int[]? | |
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
**NonCellClick** | Occurs when clicking on a blank area | void | MouseEventArgs args |
**NonCellDoubleClick** | Occurs when double-clicking on a blank area | void | MouseEventArgs args |
**CellClickBegin** | Occurs before clicking | void | MouseEventArgs args, object? record, int rowIndex, int columnIndex, Rectangle rect, bool handled |
**CellHover** | Occurs when hovering | void | MouseEventArgs args, object? record, int rowIndex, int columnIndex, Rectangle? rect |
**CellButtonClick** | Appears when the button is clicked | void | CellLink btn, MouseEventArgs args, object? record, int rowIndex, int columnIndex |
**CellButtonDown** | Occurs when a button is pressed | void | CellLink btn, MouseEventArgs args, object? record, int rowIndex, int columnIndex |
**CellButtonUp** | Occurs when a button is released | void | CellLink btn, MouseEventArgs args, object? record, int rowIndex, int columnIndex |
**CellFocused** | Occurs when a cell gains focus | void | object? record, RowType type, int rowIndex, int columnIndex, Column column, Rectangle rect |
||||
**CellBeginEdit** | Occurred before editing | bool `Return true to continue editing` | object? value, object? record, int rowIndex, int columnIndex |
**CellBeginEditInputStyle** | Text box style before editing occurs | void | object? value, object? record, int rowIndex, int columnIndex, ref Input input |
**CellEndEdit** | Occurred after editing | bool `Return true for application editing` | string value `Modified value`, object? record, int rowIndex, int columnIndex |
**CellEndValueEdit** | Occurred after editing | bool `Return true for application editing` | object? value, object? record, int rowIndex, int columnIndex |
**CellEditComplete** | Occurs after editing is complete | void | object? record, int rowIndex, int columnIndex, Column column |
**CellEditEnter** | Occurs when the Enter key is pressed in cell edit mode | void | object? record, int rowIndex, int columnIndex, Column column |
||||
**SetRowStyle** | Set row style | [CellStyleInfo?](#cellstyleinfo) | object? record, int rowIndex |
||||
**SortRows** | Occurred during row sorting | void | int columnIndex |
**SortModeChanged** | Occurs after clicking to sort | bool `Return true to continue sorting` | SortMode sortMode `Sort mode`, Column column `Header` |
**CustomSort** | Custom sorting (Comparison\<string\>) | int `Negative = x first, positive = y first, 0 = equal` | string x `Value x`, string y `Value y` |
**SortRowsTree** | Occurs when sorting tree rows | void | object record `Original record`, int from `Source position`, int to `Target position`, bool handled `Whether handled` |
**ExpandChanged** | Occurs when the expand state changes | void | object record `Original record`, bool expand `Whether expanded` |
||||
**ColumnIndexChanging** | Occurs when a column is dragged to a new position (Cancel=true to cancel) | void | int indexSource `Source position`, int indexSourceReal `Real source index`, int indexTarget `Target position`, bool cancel `Whether to cancel the drag` |
**ColumnIndexChanged** | Occurs after a column is dragged to a new position | void | int indexSource `Source position`, int indexSourceReal `Real source index`, int indexTarget `Target position` |
||||
**FilterPopupBegin** | Occurs before the filter popup opens | void | Column column `Filter column`, FilterOption? option `Filter option`, IList<object>? customSource `Custom data source of the column`, Font? font `Filter bar font`, int? height `Filter bar height`, bool cancel `Whether to cancel the popup` |
**FilterPopupEnd** | Occurs before the filter popup closes | void | FilterOption option `Current filter parameters`, object[]? records `Filtered records`, bool cancel `Whether to cancel` |
**FilterDataChanged** | Occurs after filter data changes | void | object[]? records `Filtered records` |
||||
**SummaryCustomizeChanged** | Occurred when summary customize changed | void | bool value |
**CustomSummaryCalculate** | Occurs when custom summary calculation of each row or row ends | void | Column column `Current summary column`, object record `Summary row object`, int rowIndex `Current summary row index`, bool finalize `Whether it is the finalize summary calculation`, double? totalValue `Settable custom summary value` |
**SelectIndexChanged** | Occurred when selected index changed | void | |
||||
**RowPaint** | Occurs when a row is painted | void | Canvas g, Rectangle rect, object? record, RowType type, int rowIndex |
**RowPaintBegin** | Occurs before a row is painted | void | Canvas g, Rectangle rect, object? record, RowType type, int rowIndex |
**CellPaint** | Occurs when a cell is painted | void | Canvas g, Rectangle rect, Rectangle rectreal, object? record, RowType type, int rowIndex, int index, Column column |
**CellPaintBegin** | Occurs before a cell is painted | void | Canvas g, Rectangle rect, Rectangle rectreal, object? record, RowType type, int rowIndex, int index, Column column, bool handled, SolidBrush? cellFore, Brush? cellBack, Font? cellFont |

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
**ForeColor** | Text color | Color`?` ||
||||
**BackColor** | Background color | Color`?` ||

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