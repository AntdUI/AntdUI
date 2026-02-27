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
**Back** | Background color | Color | Transparent |
**Radius** | Radius | int | 0 |
**Round** | Round style | bool | false |
**Image** | Image | Image`?` | `null` |
**ImageFit** | Image fit | [TFit](Enum.md#tfit) | Cover |

#### Animation Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Vertical** | Is vertical | bool | false |
**Speed** | Speed | int | 10 |
**Duration** | Duration(ms) | int | 400 |

#### Shadow Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOpacity** | Shadow opacity | float | 0.3F |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |

#### Hover Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**EnableHover** | Enable hover interaction | bool | false |
**HoverFore** | Hover foreground | Color`?` | `null` |
**HoverBack** | Hover background | Color`?` | `null` |
**HoverImage** | Hover image | Image`?` | `null` |
**HoverImageSvg** | Hover image SVG | string`?` | `null` |
**HoverImageRatio** | Hover image ratio | float | 0.4F |

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