[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Table
👚

> A table displays rows of data.

- DefaultProperty：Text
- DefaultEvent：CellClick

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Gap** | Gap | int | 12 |
**Radius** | Rounded corners | int | 0 |
**FixedHeader** | Fixed header | bool | true |
**VisibleHeader** | Display header | bool | true |
**Bordered** | Display Bordered | bool | false |
**RowHeight** | Row height | int`?` | `null` |
**RowHeightHeader** | Header row height | int`?` | `null` |
||||
**CheckSize** | Checkbox size | int | 16 |
**SwitchSize** | Switch size | int | 16 |
**TreeButtonSize** | Tree switch button size | int | 16 |
**DragHandleSize** | Drag and drop handle size | int | 24 |
**DragHandleIconSize** | Drag and drop handle icon size | int | 14 |
||||
**EnableHeaderResizing** | Manually adjust the column head width | bool | false |
**ColumnDragSort** | Column drag and drop sorting | bool | false |
**LostFocusClearSelection** | Loss of focus, clear selection | bool | false |
**AutoSizeColumnsMode** | Column width automatic adjustment mode | [ColumnsMode](Enum.md#columnsmode) | Auto |
||||
**ClipboardCopy** | Copy rows | bool | true |
**EditMode** | Edit mode | [TEditMode](Enum.md#teditmode) | None |
**ShowTip** | Omit text prompts | bool | true |
**HandShortcutKeys** 🔴 | Process shortcut keys | bool | true |
||||
**DefaultExpand** | Whether to expand by default `Tree` | bool | false |
||||
**Empty** | Display empty style or not | bool | true |
**EmptyText** | Display text when data is empty | string | No data |
**EmptyImage** | Display image with empty data | Image`?` | `null` |
**EmptyHeader** | Is the header displayed when empty | bool | false |
||||
**ForeColor** | Text color | Color`?` | `null` |
**RowSelectedBg** | Select background color for table rows | Color`?` | `null` |
**RowSelectedFore** | Table row selection color | Color`?` | `null` |
**BorderColor** | Table Border Color | Color`?` | `null` |
**ColumnFont** | Header font | Font`?` | `null` |
**ColumnBack** | Header background color | Color`?` | `null` |
**ColumnFore** | Header text color | Color`?` | `null` |
||||
**SelectedIndex** | Select Index | int | -1 |
**SelectedIndexs** 🔴 | Select multiple rows | int[] | |
**MultipleRows** | Enable Multiple Choice Rows | bool | false |
||||
**Columns** | Table column configuration | [ColumnCollection](TableColumn.md#column) | `null` |
**DataSource** | Data | [object](TableCell.md#icell)`?` | `Support DataTable, Class, etc` |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ScrollLine** | Scroll to the specified line | void | int i |
**CopyData** | Copy table data | void |int row|
**CopyData** | Copy table data | void |int row, int column |
**EnterEditMode** | Enter editing mode | void |int row, int column |
**SortIndex** | Get sorting sequence number | int[] ||
**SortList** | Get sorted data | object[] ||
**SortColumnsIndex** | Obtain the sorting sequence number of the header | int[] ||
**SortList** | Get sorted data | object[] ||
**ScrollLine** | Scroll to the specified line | void | int i,bool force `Force scrolling` |
**GetRowEnable** | Obtain the ability to exercise | bool | int i |
**SetRowEnable** | Set exercise capability | void | int i, bool value, bool ui `Refresh UI` |
**ToDataTable** | Export table data | DataTable`?` ||
**LoadLayout** | Refresh Layout | void ||
**Refresh** | Refresh UI | void ||
||||
**ExpandAll** | Expand all | void ||
**Expand** | Unfold or fold | void | object record `Row metadata`, bool value `fold` |
|Merge Cell|||
**AddMergedRegion** | Add merged cells | void | CellRange range |
**AddMergedRegion** | Add multiple merged cells | void | CellRange[] ranges |
**ContainsMergedRegion** | Determine whether the merged cells exist | bool | CellRange range |
**ClearMergedRegion** | Clear all merged cells | void ||

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**CheckedChanged** | Occurred when the value of the Checked attribute is changed | void | bool value, object? record, int rowIndex, int columnIndex |
**CheckedOverallChanged** | Occurred when the global CheckState property value is changed | void | ColumnCheck column, CheckState value |
**CellClick** | Appears when clicking on a cell | void | MouseEventArgs args, object? record, int rowIndex, int columnIndex, Rectangle rect |
**CellDoubleClick** | Occurred when double clicking a cell | void | MouseEventArgs args, object? record, int rowIndex, int columnIndex, Rectangle rect |
**CellButtonClick** | Appears when the button is clicked | void | CellLink btn, MouseEventArgs args, object? record, int rowIndex, int columnIndex |
||||
**CellBeginEdit** | Occurred before editing | bool `Return true to continue editing` | object? value, object? record, int rowIndex, int columnIndex |
**CellBeginEditInputStyle** | Text box style before editing occurs | void | object? value, object? record, int rowIndex, int columnIndex, ref Input input |
**CellEndEdit** | Occurred after editing | bool `Return true for application editing` | string value `Modified value`, object? record, int rowIndex, int columnIndex |
||||
**SetRowStyle** | Set row style | [CellStyleInfo?](#cellstyleinfo) | object? record, int rowIndex |
**SortRows** | Occurred during row sorting | void | int columnIndex |

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