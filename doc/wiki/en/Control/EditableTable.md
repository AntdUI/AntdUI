[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## EditableTable

Editable table with row-level editing ✏️

> An editable table where clicking Edit puts the whole row (multiple cells) into edit mode at once and the action column turns into Save/Cancel. Saving performs row-level validation; cancelling reverts the original values (newly added rows are removed).
>
> This is a [Table](Table.md) variant contributed via PR #182. It inherits from `Table`, so **all Table properties, methods and events also apply** — see [Table](Table.md). Columns and data binding work exactly as in Table; editors are configured in code with `SetEditor`.

- DefaultProperty：Columns (inherited from Table)
- DefaultEvent：CellClick (inherited from Table)

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ShowCopyButton** | Whether to show the Copy button in the built-in action column | bool | true |
**ShowDeleteButton** | Whether to show the Delete button in the built-in action column | bool | true |
**ShowEditButton** | Whether to show the Edit button in the built-in action column | bool | true |
||||
**ErrorColumns** | Keys of columns that failed validation (read-only) | string[] | empty array |
**IsRowEditing** | Whether a row is currently being edited (read-only) | bool | false |
||||
**BuiltinActions** | Whether to enable the built-in action column (Edit/Save/Cancel/Delete buttons, added automatically by AddRecord/BeginRowEdit) | bool | false |
**EditingRecord** | The record currently being edited (read-only) | object`?` | `null` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SetEditor** | Register a column editor (the column is automatically made editable); returns the editor for chaining validation rules | [CellEditor](#celleditor) | string columnKey `Column binding name (Key)`, TEditorType type `Editor type` |
**AddRecord** | Add a new row and immediately enter row edit mode | bool | object record `New row record`, int position = -1 `Insert position (-1 = end, 0 = first row)` |
**CopyRecord** | Copy a row and immediately enter row edit mode | bool | object record `Row record to copy` |
**BeginRowEdit** | Put a row into edit mode (the current row is saved first, if any) | bool | object record `Row record` |
**SaveRow** | Save the current row: commit all editor values → row-level validation → raise RowSaving (can be intercepted) | bool ||
**CancelRow** | Cancel editing and restore the values from before edit mode (rows added via AddRecord are removed) | void ||
**DeleteRow** | Delete a row (with a confirmation dialog by default) | void | object record `Row record`, bool confirm = true `Whether to show the confirmation dialog` |

> `SaveRow` returns `false` when validation fails or saving is intercepted; the whole row stays in edit mode and can be corrected and saved again. On success it returns `true` and exits edit mode. Failed columns can be read from `ErrorColumns`.

#### CellEditor

> Column editor configuration returned by `SetEditor`. Configure it fluently with the methods below; the resulting settings can be read from `Type`, `Required`, `RequiredMessage`, `MaxLength`, `Min` and `Max`.

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SetRequired** | Set whether the value is required and the error message (validated on save; a failure blocks saving and marks the cell red) | CellEditor | bool required = true `Whether required`, string? message = null `Error message (default "{column} cannot be empty")` |
**SetMaxLength** | Set the maximum text length (0 = unlimited) | CellEditor | int length `Maximum length` |
**SetRange** | Set the minimum and maximum numeric values | CellEditor | decimal? min `Minimum`, decimal? max `Maximum` |
**SetItems** | Set the drop-down items (SelectItem or raw values) | CellEditor | params object[] items `Items` |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**RowSaving** | Occurs before a row is saved; can be intercepted (e.Cancel = true blocks saving and keeps the row in edit mode) | void | object? sender `Event source`, RowSavingEventArgs e `Record row record (with edited values), Origin snapshot of the row before editing, Cancel whether to cancel saving` |
**RowSaved** | Occurs after a row has been saved | void | object? sender `Event source`, RowEventArgs e `Record row record` |
**RowCanceled** | Occurs after a row edit is cancelled | void | object? sender `Event source`, RowEventArgs e `Record row record` |
**RowDeleted** | Occurs after a row is deleted | void | object? sender `Event source`, RowEventArgs e `Record row record` |
**RowAdded** | Occurs after a new row is added (before entering edit mode) | void | object? sender `Event source`, RowEventArgs e `Record new row record` |

### TEditorType

> Cell editor type

Name | Description
:--|:--|
**Text** | Text |
**Number** | Number |
**Date** | Date |
**Select** | Drop-down selection |

### Example

```csharp
// 1. Define the data class (property names must match column Keys)
public class User
{
	public string Name { get; set; } = "";
	public int Age { get; set; }
	public string Role { get; set; } = "";
}

// 2. Create the EditableTable and bind data
var table = new AntdUI.EditableTable
{
	Dock = DockStyle.Fill,
	BuiltinActions = true, // enable the built-in Edit/Copy/Delete action column
	DataSource = new BindingList<User>
	{
		new User { Name = "Alice", Age = 28, Role = "Dev" },
		new User { Name = "Bob", Age = 32, Role = "QA" }
	}
};
table.Columns.Add(new AntdUI.Column("Name", "Name"));
table.Columns.Add(new AntdUI.Column("Age", "Age"));
table.Columns.Add(new AntdUI.Column("Role", "Role"));
Controls.Add(table);

// 3. Register editors: required text with a length limit, a ranged number, and a drop-down
table.SetEditor("Name", AntdUI.TEditorType.Text)
	 .SetRequired(true, "Name cannot be empty")
	 .SetMaxLength(20);
table.SetEditor("Age", AntdUI.TEditorType.Number)
	 .SetRange(1, 120);
table.SetEditor("Role", AntdUI.TEditorType.Select)
	 .SetItems("Dev", "QA", "Ops");

// 4. Intercept saving (optional)
table.RowSaving += (s, e) =>
{
	if (e.Record is User u && u.Name == "admin") e.Cancel = true;
};
```

Row-edit flow: clicking Edit at the end of a row calls `BeginRowEdit`, putting the whole row into edit mode and replacing the action buttons with Save/Cancel. Clicking Save calls `SaveRow`: it writes the editor values back to the record, validates every configured column and raises `RowSaving` (`e.Cancel = true` intercepts the save). If validation fails or saving is intercepted, the row stays in edit mode, invalid cells are marked red with a message, and you can correct the values and save again. Clicking Cancel calls `CancelRow`, restoring the snapshot; if the row was created by `AddRecord`, it is removed from the data source. You can also call `AddRecord`, `CopyRecord`, `BeginRowEdit`, `SaveRow`, `CancelRow` and `DeleteRow` directly in code.
