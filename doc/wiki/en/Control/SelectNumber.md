[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## SelectNumber
👚

> A numeric dropdown selector that supports custom ranges, steps, and formatting.

- DefaultProperty：Value
- DefaultEvent：SelectedIndexChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Current value | decimal | 0 |
**Minimum** | Minimum value | decimal`?` | `null` |
**Maximum** | Maximum value | decimal`?` | `null` |
**Increment** | The amount to increase/decrease when clicking arrow keys | decimal | 1 |
**DecimalPlaces** | Number of decimal places to display | int | 0 |
**ThousandsSeparator** | Whether to show thousands separator | bool | false |
**Hexadecimal** | Whether the value should be displayed in hexadecimal | bool | false |
**ShowControl** | Show controller | bool | true |
**WheelModifyEnabled** | Mouse wheel modify value | bool | true |
**InterceptArrowKeys** | Whether to continuously increase/decrease when arrow keys are pressed | bool | true |
**EnabledValueTextChange** | Whether to update Value when text changes | bool | false |
**ReadOnly** | Read only | bool | false |
**Text** | Text | string ||
**ForeColor** | Text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
**BorderWidth** | Border width | float | 1F |
**BorderColor** | Border color | Color`?` | `null` |
**BorderHover** | Hover border color | Color`?` | `null` |
**BorderActive** | Active border color | Color`?` | `null` |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
**Status** | Set verification status | [TType](Enum.md#ttype) | None |
**Variant** | Variant | [TVariant](Enum.md#tvariant) | Outlined |
**IconRatio** | Icon Scale | float | 0.7F |
**IconRatioRight** | Right icon ratio | float`?` | `null` |
**IconGap** | Ratio of icon to text spacing | float | 0.25F |
**PaddGap** | Border spacing ratio | float | 0.4F |
**Prefix** | Prefix | Image`?` | `null` |
**PrefixFore** | Prefix foreground | Color`?` | `null` |
**PrefixSvg** | Prefix SVG | string`?` | `null` |
**PrefixText** | Prefix text | string`?` | `null` |
**HasPrefix** | Whether to include prefix | bool | `false` |
**Suffix** | Suffix | Image`?` | `null` |
**SuffixFore** | Suffix foreground | Color`?` | `null` |
**SuffixSvg** | Suffix SVG | string`?` | `null` |
**SuffixText** | Suffix text | string`?` | `null` |
**HasSuffix** | Whether to include suffix | bool | `false` |
**JoinMode** | Join Mode | [TJoinMode](Enum.md#tjoinmode) | None |
**RightToLeft** | Reverse | RightToLeft | No |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the Value property value changes | void | decimal `value` |
**ValueFormatter** | Format numeric values for display | string | decimal `value` |
**SelectedIndexChanged** | Occurred when the selected item index changes | void | EventArgs `e` |
**PrefixClick** | Occurrence when Prefix is clicked | void | MouseEventArgs `e` |
**SuffixClick** | Occurrence when Suffix is clicked | void | MouseEventArgs `e` |
**ClearClick** | Clear occurs when clicked | void | MouseEventArgs `e` |
**DrawItem** | Occurs when drawing items | void | DrawItemEventArgs `e` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Focus** | Set focus | void ||
**Clear** | Clear text | void ||

### Example

```csharp
// Basic usage
selectNumber1.Minimum = 0;
selectNumber1.Maximum = 100;
selectNumber1.Increment = 5;

// Decimal settings
selectNumber2.DecimalPlaces = 2;
selectNumber2.Increment = 0.1M;

// Hexadecimal display
selectNumber3.Hexadecimal = true;
selectNumber3.Minimum = 0;
selectNumber3.Maximum = 255;

// Custom formatting
selectNumber4.ValueFormatter += (sender, e) =>
{
	return $"{e.Value}%";
};

// Event handling
selectNumber1.ValueChanged += (sender, e) =>
{
	MessageBox.Show($"Current value: {e.Value}");
};
```