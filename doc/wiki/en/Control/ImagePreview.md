[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## ImagePreview
👚

> Used for resident display and preview of images, supporting multiple interactive operations and custom configurations.

- DefaultProperty：Image
- DefaultEvent：SelectIndexChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Items** | Image item collection | ImagePreviewItemCollection | `New Collection` |
**SelectIndex** | Currently selected image index | int | 0 |
**Image** | Directly set image | Image`?` | `null` |
**Call** | Async loading image delegate | Func<Image>`?` | `null` |
**CallProg** | Async loading with progress delegate | Func<Action<Image, float>, Task>`?` | `null` |
**Fit** | Image fit mode | [TImageFit](Enum.md#timagefit) | Contain |
**FlipX** | Horizontal flip | bool | false |
**FlipY** | Vertical flip | bool | false |
**Zoom** | Zoom ratio | float | 1F |
**ZoomStep** | Zoom step | float | 0.1F |
**ZoomMin** | Minimum zoom ratio | float | 0.1F |
**ZoomMax** | Maximum zoom ratio | float | 10F |
**Button** | Show default buttons | bool | true |
**ButtonSize** | Button size | int | 32 |
**ButtonIconSize** | Button icon size | int | 18 |
**ButtonMargin** | Button margin | int | 10 |
**ButtonGap** | Button gap | int | 10 |
**ButtonAutoHide** | Auto hide buttons | bool | false |
**ButtonAutoHideDelay** | Auto hide delay(ms) | int | 2000 |
**CustomButtons** | Custom button collection | ImagePreviewButtonCollection | `New Collection` |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**LoadImg** | Load image | void | bool `Reset` |
**PlayGif** | Play GIF animation | void | Image `Image`, FrameDimension `Frame Dimension`, int `Frame Count` |
**ScaleImg** | Scale image | RectangleF | Rectangle `Control Rectangle`, float `DPI` |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndexChanged** | Occurred when selected image index changed | void | int `Index` |
**ButtonClick** | Occurred when button is clicked | void | ImagePreviewButton `Button` |

### Example

```csharp
// Basic usage
imagePreview1.Image = Image.FromFile("test.jpg");

// Add multiple images
imagePreview1.Items.Add(new ImagePreviewItem(Image.FromFile("img1.jpg")));
imagePreview1.Items.Add(new ImagePreviewItem(Image.FromFile("img2.jpg")));
imagePreview1.Items.Add(new ImagePreviewItem(Image.FromFile("img3.jpg")));

// Async loading
imagePreview1.Call = () => {
    // Simulate async loading
    Thread.Sleep(1000);
    return Image.FromFile("async.jpg");
};

// Async loading with progress
imagePreview1.CallProg = async (setImg) => {
    // Simulate download progress
    for (int i = 0; i <= 100; i += 10) {
        await Task.Delay(100);
        // Update progress
        setImg(null, i / 100f);
    }
    // Loading complete
    setImg(Image.FromFile("prog.jpg"), 1f);
};

// Add custom button
var customBtn = new ImagePreviewButton()
    .SetSvg("<svg>...</svg>")
    .SetOnClick(btn => {
        MessageBox.Show("Custom button clicked");
    });
imagePreview1.CustomButtons.Add(customBtn);
```