[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Input
👚

> Through mouse or keyboard input content, it is the most basic form field wrapper.

- DefaultProperty：Text
- DefaultEvent：TextChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**ForeColor** | Text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
||||
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
||||
**BorderWidth** | Border width | float | 1F |
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
**Variant** | Variant | [TVariant](Enum.md#tvariant) | Outlined |
||||
**AllowClear** | Support clearing | bool | false |
**AutoScroll** | Show scrollbars | bool | false |
**Text** | Text | string ||
🌏 **LocalizationText** | International Text | string`?` | `null` |
**IsTextEmpty** | Is text empty | bool | true |
**TextTotalLine** | Total lines of text | int | 0 |
**ImeMode** | IME (Input Method Editor) Status | ImeMode | NoControl |
**EmojiFont** | EmojiFont | string`?` | `null` |
**AcceptsTab** | Does multi line editing allow the input of tab characters | bool | false |
**Multiline** | Multiline | bool | false |
**WordWrap** | Auto wrap | bool | true |
**LineHeight** | Multi row height | int | 0 |
**ReadOnly** | Read only | bool | false |
**PlaceholderText** | Watermark Text | string`?` | `null` |
🌏 **LocalizationPlaceholderText** | International Watermark Text | string`?` | `null` |
**PlaceholderColor** | Watermark color | Color`?` | `null` |
**PlaceholderColorExtend** | Watermark gradient color | string`?` | `null` |
**LostFocusClearSelection** | Loss of focus, clear selection | bool | true |
||||
**TextAlign** | Text alignment | HorizontalAlignment | Left |
**UseSystemPasswordChar** | Use password box | bool | false |
**PasswordChar** | Custom password characters | char | (char)0 |
**PasswordCopy** | Passwords can be copied | bool | false |
**PasswordPaste** | Password can be pasted | bool | true |
**MaxLength** | Maximum Text Length | int | 32767 |
||||
**IconRatio** | Icon Scale | float | 0.7F |
**IconRatioRight** | Right icon ratio | float`?` | `null` |
**IconGap** | Ratio of icon to text spacing | float | 0.25F |
**PaddGap** | Border spacing ratio | float | 0.4F |
**Prefix** | Prefix | Image`?` | `null` |
**PrefixFore** | Prefix foreground | Color`?` | `null` |
**PrefixSvg** | Prefix SVG | string`?` | `null` |
**PrefixText** | Prefix text | string`?` | `null` |
🌏 **LocalizationPrefixText** | International Prefix Text | string`?` | `null` |
**HasPrefix** | Whether to include prefix | bool | `false` |
||||
**Suffix** | Suffix | Image`?` | `null` |
**SuffixFore** | Suffix foreground | Color`?` | `null` |
**SuffixSvg** | Suffix SVG | string`?` | `null` |
**SuffixText** | Suffix text | string`?` | `null` |
🌏 **LocalizationSuffixText** | International Suffix Text | string`?` | `null` |
**HasSuffix** | Whether to include suffix | bool | `false` |
||||
**JoinMode** | Join Mode | [TJoinMode](Enum.md#tjoinmode) | None |
**JoinLeft** | Connect left area `Combination button` `Obsolete` | bool | false |
**JoinRight** | Connect right area `Combination button` `Obsolete` | bool | false |
||||
**AdapterSystemMnemonic** | Adapt to system mnemonics | bool | false |
**HandShortcutKeys** | Handle shortcut keys `Obsolete` | bool | true |
||||
**RightToLeft** | Reverse | RightToLeft | No |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**AppendText** | Append text to the current text | void | string text `Additional Text` |
**AppendText** | Append text to the end | void | string text `Additional Text`, TextOpConfig config `Text configuration` |
**InsertText** | Insert text at the specified position | void | int startIndex `Start position`, string text `Text`, TextOpConfig config `Text configuration` |
**Clear** | Clear all text | void ||
**ClearUndo** | Clear undo buffer information | void ||
**Copy** | Copy | void ||
**Cut** | Cut | void ||
**Paste** | Paste | void ||
**Undo** | Undo | void ||
**Redo** | Redo | void ||
**Select** | Text selection range | void | int start, int length |
**SelectAll** | Select all texts | void ||
**SelectLast** | Select the last character | void ||
**DeselectAll** | Uncheck All | void ||
**ScrollToCaret** | Scroll the content to the current insertion symbol position | void ||
**ScrollToEnd** | Scroll to the bottom of the content | void ||
**ScrollLine** | Scroll to the specified line | void | int i `Line index` |
**EnterText** | Insert text at current position | void | string text `Text`, bool ismax `Whether to limit MaxLength` |
**SetStyle** | Set style | bool | int start `First character position`, int length `Character length`, Font? font `Font`, Color? fore `Text color`, Color? back `Background color` |
**SetStyle** | Set style | bool | TextStyle style `Text style`, bool rd `Whether to render` |
**ClearStyle** | Clear style | void ||
**GetSelectionText** | Get the currently selected text | string? ||
**SelectedText** | Get or set the currently selected text | string? ||
**IndexOf** | Find the first occurrence of a specified string | int | string value `String to find` |
**IndexOf** | Find the first occurrence of a string starting from the specified position | int | string value `String to find`, int startIndex `Start search position` |
**LastIndexOf** | Find the last occurrence of a specified string | int | string value `String to find` |
**Substring** | Extract substring starting from the specified position | string | int startIndex `Start position` |
**Substring** | Extract substring of specified length starting from the specified position | string | int startIndex `Start position`, int length `Length to extract` |
||||
**AnimationBlink** | Start blinking animation | void | int interval `Animation interval (milliseconds)`, params Color[] colors `Color values` |
**AnimationBlinkTransition** | Start color transition blinking animation | void | int interval `Animation interval (milliseconds)`, params Color[] colors `Color values` |
**AnimationBlinkTransition** | Start color transition blinking animation | void | int interval `Animation interval (milliseconds)`, int transition_interval `Transition animation interval (milliseconds)`, AnimationType animationType `Transition animation type`, params Color[] colors `Color values` |
**StopAnimationBlink** | Stop blinking animation | void ||

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**PrefixClick** | Occurrence when Prefix is clicked | void | MouseEventArgs e |
**SuffixClick** | Occurrence when Suffix is clicked | void | MouseEventArgs e |
**ClearClick** | Clear occurs when clicked | void | MouseEventArgs e |
**VerifyChar** | Occurred during character verification | void | char Char `input character`,string? ReplaceText `replace text`, bool Result  |
**VerifyKeyboard** | Occurred during keyboard verification | void | Keys KeyData, bool Result |

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