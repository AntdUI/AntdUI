[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## HyperlinkCheckbox
👚

> Display checkbox with hyperlinks, support custom styles and event handling.

- DefaultProperty：Checked
- DefaultEvent：CheckedChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text content, support `<a href="...">...</a>` syntax | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text position | ContentAlignment | MiddleLeft |
**NormalStyle** | Link style in normal state | LinkAppearance`?` | `null` |
**HoverStyle** | Link style when mouse hovers | LinkAppearance`?` | `null` |
**LinkPadding** | Distance between links and surrounding characters | Padding | 2, 0, 2, 0 |
**LinkAutoNavigation** | Automatically open hyperlinks with default browser | bool | false |
**AutoSize** | Auto size | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**ForeColor** | Text color | Color`?` | `null` |
**Checked** | Checked state | bool | false |
**CheckAlign** | Checkbox position | ContentAlignment | MiddleLeft |
**ThreeState** | Three-state mode | bool | false |
**CheckState** | Checkbox state | CheckState | Unchecked |

### LinkAppearance Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**LinkColor** | Link text color | Color`?` | `null` |
**LinkStyle** | Link font style | FontStyle | Regular |
**UnderlineColor** | Underline color | Color`?` | `null` |
**UnderlineThickness** | Underline thickness (0 for no underline) | int | 1 |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LinkClicked** | Occurred when a link is clicked | void | LinkClickedEventArgs `e` |
**CheckedChanged** | Occurred when checked state changes | void | EventArgs `e` |

### Example

```csharp
// Basic usage
hyperlinkCheckbox1.Text = "I agree to the <a href='https://example.com/terms'>Terms of Service</a>";

// Custom link style
hyperlinkCheckbox1.NormalStyle.LinkColor = Color.Blue;
hyperlinkCheckbox1.HoverStyle.LinkColor = Color.Red;
hyperlinkCheckbox1.HoverStyle.Underline = true;

// Enable auto navigation
hyperlinkCheckbox1.LinkAutoNavigation = true;

// Handle link click event
hyperlinkCheckbox1.LinkClicked += (sender, e) =>
{
	MessageBox.Show($"Clicked link: {e.Text}\nURL: {e.Href}");
};
```