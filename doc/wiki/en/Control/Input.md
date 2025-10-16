[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Input
👚

> Through mouse or keyboard input content, it is the most basic form field wrapper.

- DefaultProperty：Text
- DefaultEvent：TextChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**ForeColor** | Text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Activate background color | Color`?` | `null` |
||||
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**BorderWidth** | Border width | float | 0F |
**BorderColor** | Border color | Color`?` | `null` |
**BorderHover** | Hover border color | Color`?` | `null` |
**BorderActive** | Activate border color | Color`?` | `null` |
||||
**SelectionColor** | Select color | Color | 102, 0, 127, 255 |
||||
**CaretColor** | Caret color | Color`?` | `null` |
**CaretSpeed** | Caret speed | int | 1000 |
||||
**WaveSize** | Wave size `Click animation` | int | 4 |
**Radius** | Rounded corners | int | 6 |
**Round** | Rounded corner style | bool | false |
**Status** | Set verification status | [TType](Enum.md#ttype) | None |
||||
**AllowClear** | Support clearing | bool | false |
**AutoScroll** | Show scrollbars | bool | false |
**Text** | Text | string ||
🌏 **LocalizationText** | International Text | string`?` | `null` |
**ImeMode** | IME (Input Method Editor) Status | ImeMode | NoControl |
**EmojiFont** | EmojiFont | string | Segoe UI Emoj |
**AcceptsTab** | Does multi line editing allow the input of tab characters | bool | false |
**Multiline** | Multiline | bool | false |
**LineHeight** | Multi row height | int | 0 |
**ReadOnly** | Read only | bool | false |
**PlaceholderText** | Watermark Text | string`?` | `null` |
🌏 **LocalizationPlaceholderText** | International Watermark Text | string`?` | `null` |
**PlaceholderColor** | Watermark color | Color`?` | `null` |
**PlaceholderColorExtend** | Watermark gradient color | string`?` | `null` |
**LostFocusClearSelection** | Loss of focus, clear selection | bool | true |
**HandShortcutKeys** 🔴 | Process shortcut keys | bool | true |
||||
**TextAlign** | Text alignment | HorizontalAlignment | Left |
**UseSystemPasswordChar** | Use password box | bool | false |
**PasswordChar** | Custom password characters | char | (char)0 |
**PasswordCopy** | Passwords can be copied | bool | false |
**PasswordPaste** | Password can be pasted | bool | true |
**MaxLength** | Maximum Text Length | int | 32767 |
||||
**IconRatio** | Icon Scale | float | 0.7F |
**IconGap** | Ratio of icon to text spacing | float | 0.25F |
**Prefix** | Prefix | Image`?` | `null` |
**PrefixFore** | Prefix foreground | Color`?` | `null` |
**PrefixSvg** | Prefix SVG | string`?` | `null` |
**PrefixText** | Prefix text | string`?` | `null` |
||||
**Suffix** | Suffix | Image`?` | `null` |
**SuffixFore** | Suffix foreground | Color`?` | `null` |
**SuffixSvg** | Suffix SVG | string`?` | `null` |
**SuffixText** | Suffix text | string`?` | `null` |
||||
**JoinLeft** | Connect left area `Combination button` | bool | false |
**JoinRight** | Connect right area `Combination button` | bool | false |
||||
**RightToLeft** | Reverse | RightToLeft | No |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**AppendText** | Append text to the current text | void | string text `Additional Text` |
**Clear** | Clear all text | void ||
**ClearUndo** | Clear undo buffer information | void ||
**Copy** | Copy | void ||
**Cut** | Cut | void ||
**Paste** | Paste | void ||
**Undo** | Undo | void ||
**Select** | Text selection range | void | int start, int length |
**SelectAll** | Select all texts | void ||
**DeselectAll** | Uncheck All | void ||
**ScrollToCaret** | Scroll the content to the current insertion symbol position | void ||
**ScrollToEnd** | Scroll to the bottom of the content | void ||

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**PrefixClick** | Occurrence when Prefix is clicked | void | MouseEventArgs e |
**SuffixClick** | Occurrence when Suffix is clicked | void | MouseEventArgs e |
**ClearClick** 🔴 | Clear occurs when clicked | void | MouseEventArgs e |
**VerifyChar** 🔴 | Occurred during character verification | void | char Char `input character`,string? ReplaceText `replace text`, bool Result  |
**VerifyKeyboard** 🔴 | Occurred during keyboard verification | void | Keys KeyData, bool Result |

### Input Intercept strings

> Not through `KeyPress`, but through `VerifyChar` or rewriting `Verify`

> The following is a simulation of [InputNumber](#inputnumber) to achieve only numerical input

``` csharp
private void Input1_VerifyChar(object sender, AntdUI.InputVerifyCharEventArgs e)
{
    NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
    string decimalSeparator = numberFormatInfo.NumberDecimalSeparator,
        groupSeparator = numberFormatInfo.NumberGroupSeparator, negativeSign = numberFormatInfo.NegativeSign;
    string keyInput = e.Char.ToString();
    if (char.IsDigit(e.Char))
    {
        e.Result = true; // Numbers can be
    }
    else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) || keyInput.Equals(negativeSign))
    {
        e.Result = true; // The decimal separator can be used
    }
    else if (e.Char == '\b')
    {
        e.Result = true; // The Backspace key can be used
    }
    else if (e.Char == '。')
    {
        e.ReplaceText = ".";
        e.Result = true; // Replace Chinese period with English period
    }
    else
    {
        e.Result = false;
    }
}
```

***


## InputNumber
👚

> Enter a number within certain range with the mouse or keyboard. Inherited from [Input](#input)

- DefaultProperty：Value
- DefaultEvent：ValueChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Minimum** | Minimum value | decimal`?` | `null` |
**Maximum** | Maximum value | decimal`?` | `null` |
**Value** | Current value | decimal | 0 |
||||
**ShowControl** | Controller | bool | true |
**DecimalPlaces** | Number of decimal places displayed | int | 0 |
**ThousandsSeparator** | Do you want to display the thousand separator | bool | false |
**Hexadecimal** | Should values be displayed in hexadecimal format | bool | false |
**InterceptArrowKeys** | Does the arrow key continuously increase/decrease when pressed | bool | true |
**Increment** | The amount of increase/decrease each time the arrow key is clicked | decimal | 1 |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ValueChanged** | Occurred when the value of the Value property is changed | void | decimal value |