[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Descriptions
👚

> Display multiple read-only fields in a group.

- DefaultProperty：Items

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Collection | DescriptionsItemCollection |
**Title** | Title | string`?` | `null` |
🌏 **LocalizationTitle** | Title | string`?` | `null` |
**TextAlign** | Content text alignment | ContentAlignment | MiddleLeft |
**TextWrap** | Whether to wrap when content overflows (no = show ellipsis) | bool | true |
||||
**ForeColor** | Content text color | Color`?` | `null` |
**LabelFore** | Label text color | Color`?` | `null` |
**TitleFontRatio** | Title font ratio | float | 1.15F |
**TitleFore** | Title text color | Color`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
**LabelBack** | Label background color (effective in bordered mode) | Color`?` | `null` |
||||
**IconGap** | Icon and text gap | int | 4 |
**IconRatio** | Icon ratio | float | .7F |
**TitleIcon** | Title icon | Image`?` | `null` |
**TitleIconSvg** | Title icon SVG | string`?` | `null` |
||||
**AutoSize** | Auto Size | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**PauseLayout** | Pause Layout | bool | false |
**SizeMode** | Set list size | TSizeMode | Default |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**Radius** | Rounded corners | int | 6 |
**Bordered** | Whether to show border | bool | false |
**BorderStyle** | Border Style | DashStyle | Solid |
||||
**BadgeGap** | Badge-text gap | int | 6 |
**BadgeRatio** | Badge dot ratio | float | .4F |
**Column** | Number of DescriptionsItem per row | int | 3 |
**ColumnWidth** | Column width mode | TDescriptionsColumnWidth | Auto |
**ItemLayout** | Description layout | TDescriptionsLayout | Horizontal |
**LabelAlign** | Label text alignment | ContentAlignment | MiddleLeft |
**LabelRatio** | Max label ratio (effective when auto width) | float | .65F |
**LabelWidth** | Label width (0 = auto width) | int | 0 |
**MaxColumnRatio** | Max ratio of a single column (effective when column width mode is content) | float | .6F |
||||
**Colon** | Whether to show colon after label | bool | true |

### Data

#### DescriptionsItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | Name | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | Text | string`?` | `null` |
**Value** | Raw value (takes precedence over text, used with formatting) | object`?` | `null` |
**ContentType** | Content type (text/badge) | TDescriptionsContentType | Text |
**Format** | Text format (standard format string or composite format containing {0}) | string`?` | `null` |
**Label** | Label | string`?` | `null` |
🌏 **LocalizationLabel** | Label | string`?` | `null` |
||||
**ForeColor** | Content text color | Color`?` | `null` |
**LabelFore** | Label text color | Color`?` | `null` |
||||
**Fill** | Badge color | Color`?` | `null` |
||||
**Visible** | Is it visible | bool | true |
||||
**Span** | Number of columns spanned (less than or equal to 0 = fill the remaining part of the current row) | int | 1 |
**State** | State (badge color) | [TState](Enum.md#tstate) | Default |

### Methods

Name | Description | Return Value | Parameters
:--|:--|:--|:--|
**Relayout** | Relayout | void ||

### Enum

#### TDescriptionsLayout

> Description layout

| Value | Description |
|:--|:--|
| Horizontal | Horizontal |
| Vertical | Vertical |

#### TDescriptionsColumnWidth

> Column width mode

| Value | Description |
|:--|:--|
| Auto | Auto (content ratio when bordered, equally divided when borderless) |
| Equal | Equally divided |
| Content | Content ratio |

#### TDescriptionsContentType

> Descriptions content type

| Value | Description |
|:--|:--|
| Text | Text |
| Badge | Badge (dot + text) |

#### TSizeMode

> Set list size (shared enum)

| Value | Description |
|:--|:--|
| Default | Default |
| Middle | Middle |
| Small | Small |

### Example

> Create a description list showing basic information and a badge status

```csharp
var descriptions = new AntdUI.Descriptions
{
	Dock = DockStyle.Top,
	Title = "User Information",
	Column = 2,
	Bordered = true,
	ItemLayout = TDescriptionsLayout.Horizontal,
	ColumnWidth = TDescriptionsColumnWidth.Auto
};

descriptions.Items.Add(new AntdUI.DescriptionsItem("Name", "Zhang San"));
descriptions.Items.Add(new AntdUI.DescriptionsItem("Age", "28"));
descriptions.Items.Add(new AntdUI.DescriptionsItem("Department", "Engineering"));
descriptions.Items.Add(new AntdUI.DescriptionsItem("Status", "Online")
{
	ContentType = TDescriptionsContentType.Badge,
	State = TState.Success
});

Controls.Add(descriptions);
```
