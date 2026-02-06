[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## ImagePreview
👚

> Used for resident display and preview of images, supporting multiple interactive operations and custom configurations.

- DefaultProperty：SelectIndex
- DefaultEvent：SelectIndexChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Image** | Image item collection | ImagePreviewItemCollection | `New Collection` |
**SelectIndex** | Currently selected image index | int | 0 |
**Fit** | Image fit mode | [TFit](Enum.md#tfit) | Contain |
**ShowBtn** | Show buttons | bool | true |
**ShowDefaultBtn** | Show default buttons | bool | true |
**BtnSize** | Button size | Size | `42, 46` |
**BtnIconSize** | Button icon size | int | 18 |
**BtnLRSize** | Left/right button size | int | 40 |
**ContainerPadding** | Container padding | int | 24 |
**BtnPadding** | Button padding | Size | `12, 32` |
**CustomButton** | Custom button collection | ImagePreviewButtonCollection | `New Collection` |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LoadImg** | Load image | void | bool `Render` |
**ZoomToControl** | Zoom image to fit control | void |  |
**FlipY** | Flip image vertically | void |  |
**FlipX** | Flip image horizontally | void |  |
**RotateL** | Rotate image left | void |  |
**RotateR** | Rotate image right | void |  |
**ZoomOut** | Zoom out image | void |  |
**ZoomIn** | Zoom in image | void |  |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndexChanged** | Occurred when selected image index changed | void | int `Index` |
**ButtonClick** | Occurred when button is clicked | void | ImagePreviewItem `Item`, string `Button ID`, object`?` `Tag` |

### Data

#### ImagePreviewItem

> Image item

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Img** | Image | Image`?` | `null` |
**Call** | Async loading image delegate | Func<int, ImagePreviewItem, Image>`?` | `null` |
**CallProg** | Async loading with progress delegate | Func<int, ImagePreviewItem, Action<float, string?>, Image>`?` | `null` |
**Tag** | User defined data | object`?` | `null` |

#### ImagePreviewButton

> Custom button

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Button name | string | `null` |
**IconSvg** | Icon SVG | string | `null` |
**Tag** | User defined data | object`?` | `null` |

### Example

```csharp
// Basic usage
var item = new ImagePreviewItem().SetImage(Image.FromFile("test.jpg"));
imagePreview1.Image.Add(item);

// Add multiple images
imagePreview1.Image.Add(new ImagePreviewItem().SetImage(Image.FromFile("img1.jpg")));
imagePreview1.Image.Add(new ImagePreviewItem().SetImage(Image.FromFile("img2.jpg")));
imagePreview1.Image.Add(new ImagePreviewItem().SetImage(Image.FromFile("img3.jpg")));

// Async loading
imagePreview1.Image.Add(new ImagePreviewItem().SetImage((index, item) => {
    // Simulate async loading
    Thread.Sleep(1000);
    return Image.FromFile("async.jpg");
}));

// Async loading with progress
imagePreview1.Image.Add(new ImagePreviewItem().SetImage((index, item, progress) => {
    // Simulate download progress
    for (int i = 0; i <= 100; i += 10) {
        Thread.Sleep(100);
        // Update progress
        progress(i / 100f, $"Loading {i}%");
    }
    // Loading complete
    return Image.FromFile("prog.jpg");
}));

// Add custom button
var customBtn = new ImagePreviewButton().SetName("custom").SetIcon("<svg>...</svg>");
imagePreview1.CustomButton.Add(customBtn);
```