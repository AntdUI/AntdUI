[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## HyperlinkLabel
👚

> Display text with hyperlinks, support custom styles and event handling.

- DefaultProperty：Text
- DefaultEvent：LinkClicked

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text content, support `<a href="...">...</a>` syntax | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text position | ContentAlignment | MiddleLeft |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**AutoSize** | Auto size | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**LinkAutoNavigation** | Automatically open hyperlinks with default browser | bool | false |
**HoverStyle** | Link style when mouse hovers | LinkAppearance`?` | `null` |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowOpacity** | Shadow opacity | float | 0.3F |
||||
**LinkPadding** | Distance between links and surrounding characters | Padding | 2, 0, 2, 0 |
**NormalStyle** | Link style in normal state | LinkAppearance`?` | `null` |

### LinkAppearance Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**UnderlineColor** | Underline color | Color`?` | `null` |
**UnderlineThickness** | Underline thickness (0 for no underline) | int | 1 |
||||
**LinkColor** | Link text color | Color`?` | `null` |
**LinkStyle** | Link font style | FontStyle | Regular |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LinkClicked** | Occurred when a link is clicked | void | LinkClickedEventArgs `e` |

### Example

```csharp
// Basic usage
hyperlinkLabel1.Text = "Visit <a href='https://ant.design'>Ant Design</a> website";

// Custom style
hyperlinkLabel1.NormalStyle.Underline = false;
hyperlinkLabel1.HoverStyle.Color = Color.Red;
hyperlinkLabel1.HoverStyle.Underline = true;

// Disable auto navigation
hyperlinkLabel1.LinkAutoNavigation = false;
```