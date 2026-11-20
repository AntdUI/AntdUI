[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Image3D
👚

> A control for displaying images with 3D transition animation effects.

- DefaultProperty：Image
- DefaultEvent：Click

### Properties

#### Appearance Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**TextAlign** | Text alignment | [TAlignMini](Enum.md#talignmini) | Bottom |
**TextPadding** | Text padding | Padding | 0, 0, 0, 0 |
||||
**Back** | Background color | Color | Transparent |
||||
**Image** | Image | Image`?` | `null` |
**ImageFit** | Image fit | [TFit](Enum.md#tfit) | Cover |
||||
**ShowText** | Display Text | bool | false |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width | float | 0F |
**Radius** | Radius | int | 0 |
**BorderStyle** | Border Style | [DashStyle](Enum.md#dashstyle) | Solid |
**Round** | Round style | bool | false |

#### Animation Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Duration** | Duration(ms) | int | 400 |
**Speed** | Speed | int | 10 |
||||
**TiltMax** | Max 3D tilt angle | int | 15 |
||||
**Tilt** | Enable 3D tilt (follows mouse on hover) | bool | false |
**Vertical** | Is vertical | bool | false |

#### Shadow Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowOpacity** | Shadow opacity | float | 0.3F |

#### Hover Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**HoverFore** | Hover foreground | Color`?` | `null` |
||||
**HoverBack** | Hover background | Color`?` | `null` |
||||
**EnableHover** | Enable hover interaction | bool | false |
**HoverImage** | Hover image | Image`?` | `null` |
**HoverImageRatio** | Hover image ratio | float | 0.4F |
**HoverImageSvg** | Hover image SVG | string`?` | `null` |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**RunAnimation** | Run animation transition | void | Image`?` `Switch to new image` |

### Example

```csharp
// Basic usage
image3D1.Image = Image.FromFile("test.jpg");

// Switch image (with 3D animation)
image3D1.RunAnimation(Image.FromFile("new.jpg"));

// Configure animation
image3D1.Vertical = true; // Vertical animation
image3D1.Speed = 15; // Animation speed
image3D1.Duration = 600; // Animation duration

// Configure appearance
image3D1.Radius = 8; // Radius
image3D1.Shadow = 4; // Shadow size

// Configure hover effect
image3D1.EnableHover = true;
image3D1.HoverBack = Color.FromArgb(100, 0, 0, 0);
image3D1.HoverImageSvg = "<svg>...</svg>";
```