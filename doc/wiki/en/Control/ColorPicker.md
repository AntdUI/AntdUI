[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## ColorPicker
👚

> Used for color selection.

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string | `""` |
**Value** | Value of color | Color | Colour.Primary.Get(nameof(ColorPicker)) `Theme color` |
||||
**ForeColor** | Text color | Color`?` | `null` |
**DropDownFontRatio** | Dropdown font ratio | float | 0.9F |
||||
**BackColor** | Background color | Color`?` | `null` |
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**WaveSize** | Wave size `Click animation` | int | 4 |
||||
**AutoSize** | Auto Size | bool | false |
**AllowClear** | Support clearing | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**JoinMode** | Combination mode | [TJoinMode](Enum.md#tjoinmode) | None |
**Mode** | Color mode | [TColorMode](Enum.md#tcolormode) | Hex |
**ShowClose** | Display the close button | bool | false |
**ShowReset** | Display reset button | bool | false |
**ShowSymbol** | Display custom symbol (length<4) | bool | false |
**ShowText** | Display Hex text | bool | false |
**Trigger** | Trigger dropdown behavior | [Trigger](Enum.md#trigger) | Click |
**BorderHover** | Hover border color | Color`?` | `null` |
**BorderActive** | Activate border color | Color`?` | `null` |
**HasFocus** | Whether focus exists | bool | false |
**DisabledAlpha** | Disable transparency | bool | false |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
||||
**Placement** | Menu pop-up location | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | Is the dropdown arrow displayed | bool | true |
||||
**HasValue** | Whether it contains value | bool | true |
**JoinLeft** | Connect left area `Combination button` (Obsolete, use JoinMode) | bool | false |
**JoinRight** | Connect right area `Combination button` (Obsolete, use JoinMode) | bool | false |
**Presets** | Preset colors | Color[] | `null` |
**ValueClear** | Get color value | Color`?` | `null` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ClearValue** | Clear value | void | |
**ClearValue** | Clear value | void | Color def `Default Color` |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | Color value |
**ValueFormatChanged** | Occurred when Value is formatted | string | Color value |