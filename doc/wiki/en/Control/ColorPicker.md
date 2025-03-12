[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## ColorPicker
👚

> Used for color selection.

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**AutoSize** | Auto Size | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**Mode** | Color mode | [TColorMode](Enum.md#tcolormode) | Hex |
||||
**ForeColor** | Text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
||||
**BorderWidth** | Border width | float | 0F |
**BorderColor** | Border color | Color`?` | `null` |
**BorderHover** | Hover border color | Color`?` | `null` |
**BorderActive** | Activate border color | Color`?` | `null` |
||||
**WaveSize** | Wave size `Click animation` | int | 4 |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
**ShowText** | Display Hex text | bool | false |
||||
**JoinLeft** | Connect left area `Combination button` | bool | false |
**JoinRight** | Connect right area `Combination button` | bool | false |
||||
**Value** | Value of color | Color | Style.Db.Primary `Theme color` |
**DisabledAlpha** 🔴 | Disable transparency | bool | false |
**AllowClear** 🔴 | Support clearing | bool | false |
**ShowClose** 🔴 | Display the close button | bool | false |
||||
**Trigger** | Trigger dropdown behavior | [Trigger](Enum.md#trigger) | Click |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | false |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ClearValue** | Clear value | void | |
**ClearValue** | Clear value | void | Color def `Default Color` |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | Color value |