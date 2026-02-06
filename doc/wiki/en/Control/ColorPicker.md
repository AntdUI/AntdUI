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
**BorderWidth** | Border width | float | 1F |
**BorderColor** | Border color | Color`?` | `null` |
**BorderHover** | Hover border color | Color`?` | `null` |
**BorderActive** | Activate border color | Color`?` | `null` |
||||
**WaveSize** | Wave size `Click animation` | int | 4 |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
**ShowText** | Display Hex text | bool | false |
**ShowSymbol** | Display custom symbol (length<4) | bool | false |
**Text** | Text | string | `""` |
||||
**JoinLeft** 🔴 | Connect left area `Combination button` | bool | false |
**JoinRight** 🔴 | Connect right area `Combination button` | bool | false |
**JoinMode** | Combination mode | [TJoinMode](Enum.md#tjoinmode) | None |
||||
**Value** | Value of color | Color | Style.Db.Primary `Theme color` |
**DisabledAlpha** | Disable transparency | bool | false |
**AllowClear** | Support clearing | bool | false |
**ShowClose** | Display the close button | bool | false |
**ShowReset** | Display reset button | bool | false |
**HasValue** | Whether it contains value | bool | true |
**ValueClear** | Get color value | Color`?` | `null` |
**Presets** | Preset colors | Color[] | `null` |
||||
**Trigger** | Trigger dropdown behavior | [Trigger](Enum.md#trigger) | Click |
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | true |
**DropDownFontRatio** | Dropdown font ratio | float | 0.9F |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ClearValue** | Clear value | void | |
**ClearValue** | Clear value | void | Color def `Default Color` |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | Color value |
**ValueFormatChanged** | Occurred when Value is formatted | string | Color value |