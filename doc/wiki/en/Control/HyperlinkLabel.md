[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## HyperlinkLabel
👚

> Display text with hyperlinks, support custom styles and event handling.

- DefaultProperty：Text
- DefaultEvent：LinkClicked

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text content, support `<a href="...">...</a>` syntax | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**NormalStyle** | Normal state link style | LinkAppearance | `Default Style` |
**HoverStyle** | Hover state link style | LinkAppearance | `Default Style` |
**LinkPadding** | Distance between link and surrounding characters | int | 2 |
**LinkAutoNavigation** | Whether to automatically open links | bool | true |
**TextAlign** | Text alignment | ContentAlignment | TopLeft |
**Shadow** | Enable shadow effect | bool | false |
**ShadowSize** | Shadow size | int | 2 |
**ShadowColor** | Shadow color | Color | Color.FromArgb(255, 0, 0, 0) |
**ShadowOpacity** | Shadow opacity | float | 0.2F |
**ShadowOffset** | Shadow offset | Point | 1, 1 |

### LinkAppearance Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Color** | Link color | Color | Color.FromArgb(255, 10, 76, 178) |
**HoverColor** | Hover color | Color | Color.FromArgb(255, 79, 126, 194) |
**FontStyle** | Font style | FontStyle | FontStyle.Regular |
**Underline** | Underline | bool | true |
**HoverUnderline** | Hover underline | bool | true |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LinkClicked** | Occurred when a link is clicked | void | string `href`, string `text` |

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