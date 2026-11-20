[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

[Return to Table](Table.md)

## Column

> Diverse header

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Title** | Display text | string ||
🌏 **LocalizationTitle** | International Display text | string`?` | `null` |
||||
**ReadOnly** | Read only | bool |false|
**Visible** | Is it displayed | bool|true|
**DisplayFormat** | Display format | string`?` ||
**DragSort** | Column can be dragged and dropped | bool |true|
**Editable** | Column editable | bool |true|
**SortMode** | Sort Mode | SortMode |NONE|
||||
**Align** | Align | ColumnAlign |ColumnAlign.Left|
**ColAlign** | Header Align | ColumnAlign`?` | `null` |
**Fixed** | Is the column fixed | bool |false|
**KeyTree** | Tree Column | string`?` ||
**LineBreak** | Automatic line wrapping | bool |false|
**MaxWidth** | Maximum width of column | string`?` ||
**MinWidth** | Column minimum width | string`?` ||
**SummaryItem** | Summary column options | SummaryItemOption`?` ||
**Width** | Column Width | string`?` ||
**Wrap** | Wrap child elements within cell | bool |false|
**WrapCount** | Number of items per row, >0 wrap by count, <=0 wrap by width | int |0|
**WrapGapX** | Horizontal gap mode within row | [TableWrapGap](#tablewrapgap) |Fixed|
**WrapGapY** | Vertical gap mode between rows | [TableWrapGap](#tablewrapgap) |Fixed|
||||
**ColBreak** | Automatic line wrapping in the header | bool |false|
**ColStyle** | Title column style | CellStyleInfo`?` ||
**Ellipsis** | Exceeding the width will be automatically omitted | bool |false|
**Filter** | User filter options | FilterOption`?` ||
**HasFilter** | Has filter | bool |false| (Read only)
**Key** | Bind field name | string ||
**Render** | SLOT | Func<object? `Current value`, object `Row metadata`, int `rowIndex`, object?>? | Return formatted data |
**SortOrder** | Enable sorting | bool |false|
**Style** | Column Style | CellStyleInfo`?` ||

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SetWrap** | Set cell child element wrapping | [Column](#column) | int count = 0 `0 disable, >0 wrap by count, <0 wrap by width` |
**SetWrapGapX** | Set horizontal gap mode within row | [Column](#column) | [TableWrapGap](#tablewrapgap) value |
**SetWrapGapY** | Set vertical gap mode between rows | [Column](#column) | [TableWrapGap](#tablewrapgap) value |

### Enum

#### TableWrapGap

> Wrap gap mode

 Name | Description | Value |
 :--|:--|:--|
 **Fixed** | Fixed gap | 0 |
 **SpaceBetween** | Evenly distributed (equal gaps before, between, and after) | 1 |

#### ColumnCheck

> Checkbox header. Inherited from [Column](#column)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Checked** | Checked state | bool | false |
**AutoCheck** | Click to automatically change the selected status | bool | true |
**CheckState** | Checked state | CheckState | Unchecked |
||||
**Call** | Checkbox callback | Func<bool `Check value after change`, object? `Row metadata`, int `rowIndex`, int `columnIndex`, bool>`?` | Return to the final Select Value |
**Key** | Bind field name | string ||

#### ColumnRadio

> Radio header. Inherited from [Column](#column)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Title** | Display text | string ||
||||
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**Call** | Checkbox callback | Func<bool `Check value after change`, object? `Row metadata`, int `rowIndex`, int `columnIndex`, bool>`?` | Return to the final Select Value |
**Key** | Bind field name | string ||

#### ColumnSwitch

> Switch header. Inherited from [Column](#column)

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Title** | Display text | string ||
||||
**AutoCheck** | Click to automatically change the selected status | bool | true |
||||
**Call** | Checkbox callback | Func<bool `Check value after change`, object? `Row metadata`, int `rowIndex`, int `columnIndex`, bool>`?` | Return to the final Select Value |
**Key** | Bind field name | string ||

#### ColumnSort

> Drag and drop handle column. Inherited from [Column](#column)