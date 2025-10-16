[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

[Return to Table](Table.md)

## Column

> Diverse header

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Key** | Bind field name | string ||
**Title** | Display text | string ||
🌏 **LocalizationTitle** | International Display text | string`?` | `null` |
||||
**Visible** | Is it displayed | bool|true|
**Align** | Align | ColumnAlign |ColumnAlign.Left|
**ColAlign** | Header Align | ColumnAlign`?` | `null` |
**Width** | Column Width | string`?` ||
**MaxWidth** | Maximum width of column | string`?` ||
||||
**Fixed** | Is the column fixed | bool |false|
**Ellipsis** | Exceeding the width will be automatically omitted | bool |false|
**LineBreak** | Automatic line wrapping | bool |false|
**ColBreak** | Automatic line wrapping in the header | bool |false|
**SortOrder** | Enable sorting | bool |false|
**SortMode** | Sort Mode | SortMode |NONE|
**Editable** | Column editable | bool |true|
**DragSort** | Column can be dragged and dropped | bool |true|
**KeyTree** | Tree Column | string`?` ||
||||
**Style** | Column Style | CellStyleInfo`?` ||
**ColStyle** | Title column style | CellStyleInfo`?` ||
**Render** | SLOT | Func<object? `Current value`, object `Row metadata`, int `rowIndex`, object?>? | Return formatted data |

#### ColumnCheck

> Checkbox header. Inherited from [Column](#column)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Key** | Bind field name | string ||
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**Checked** | Checked state | bool | false |
**CheckState** | Checked state | CheckState | Unchecked |
||||
**Call** | Checkbox callback | Func<bool `Check value after change`, object? `Row metadata`, int `rowIndex`, int `columnIndex`, bool>`?` | Return to the final Select Value |

#### ColumnRadio

> Radio header. Inherited from [Column](#column)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Key** | Bind field name | string ||
**Title** | Display text | string ||
**AutoCheck** | Click to automatically change the selected status | bool | true |
**Call** | Checkbox callback | Func<bool `Check value after change`, object? `Row metadata`, int `rowIndex`, int `columnIndex`, bool>`?` | Return to the final Select Value |

#### ColumnSwitch

> Switch header. Inherited from [Column](#column)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Key** | Bind field name | string ||
**Title** | Display text | string ||
**AutoCheck** | Click to automatically change the selected status | bool | true |
**Call** | Checkbox callback | Func<bool `Check value after change`, object? `Row metadata`, int `rowIndex`, int `columnIndex`, bool>`?` | Return to the final Select Value |

#### ColumnSort

> Drag and drop handle column. Inherited from [Column](#column)