[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)

> `AntdUI.Config` is used for global configuration. It is recommended to complete settings in the `Main` method of `Program.cs`, before creating any form.

### Color Mode

> Default Light color mode

#### Set color mode

``` csharp
AntdUI.Config.Mode = AntdUI.TMode.Light;
```

> 🔴 .NET 10 and above: setting `Mode` also calls `Application.SetColorMode` to switch system controls together with AntdUI. To follow the system theme, call `Application.SetColorMode(SystemColorMode.System);` **before accessing `AntdUI.Config`**, because the static constructor of `Config` reads the system color mode as its initial value on first access. See [Theme](Theme.md#net-10-follow-system-color-mode).

#### Is it in light color mode

``` csharp
bool islight = AntdUI.Config.IsLight;
AntdUI.Config.IsLight = true;// Set to light color mode
```

#### Is it in dark mode

``` csharp
bool isdark = AntdUI.Config.IsDark;
AntdUI.Config.IsDark = true;// Set to dark mode
```

#### Configure global switching themes 🔴

> After configuration, forms that inherit [BaseForm](Form/BaseForm.md) will automatically switch themes

``` csharp
AntdUI.Config.Theme()
.Light("#fff", "#000") // Light mode background white (#fff) | foreground black (#000)
.Dark("#000", "#fff") // Dark mode background black (#000) | foreground white (#fff)
.Header("#f3f3f3", "#111111"); // PageHeader header head background light color light gray (#f3f3f3) | dark background color dark gray (#111111)
```

> For more theme capabilities (brand colors, color algorithms, following the system), see [Theme](Theme.md).

#### Theme configuration object

> `Theme()` is the convenient entry point (auto-creates when null); `ThemeConfig` is the readable/writable underlying property, allowing direct assignment of a custom config object

``` csharp
AntdUI.IThemeConfig? config = AntdUI.Config.ThemeConfig; // get (may be null)
AntdUI.Config.ThemeConfig = config;                      // set directly
```

#### Clear theme configuration

``` csharp
AntdUI.Config.ThemeClear();
```

### Animation

> Default animation on

``` csharp
AntdUI.Config.Animation = false;
```

#### Enable / disable animation by control

> You can disable animation for specific control types (control names); `params` accepts multiple names

``` csharp
AntdUI.Config.DisableAnimation("Button", "Input"); // Disable animation for Button, Input
AntdUI.Config.EnableAnimation("Button"); // Restore Button animation (remove from disable list)
AntdUI.Config.ClearAnimationData(); // Clear all disable settings, restore everything
```

#### Query whether animation is enabled

``` csharp
bool enabled = AntdUI.Config.HasAnimation("Button"); // Query by control name
bool enabled2 = AntdUI.Config.HasAnimation("Button", "btnOK"); // Query by control name / control Name
```

### Table Cell Focused Style

> Global default is `TableCellFocusedStyle.None` (no focus); can be set to dashed box or solid fill

``` csharp
AntdUI.Config.DefaultCellFocusedStyle = AntdUI.TableCellFocusedStyle.Solid;
```

### Touch Screen

#### Touch Screen Enabled

> Default Enable touch

``` csharp
AntdUI.Config.TouchEnabled = true;
```

#### Touch Threshold

> Default `20` (pixels), the displacement threshold that triggers a touch determination

``` csharp
AntdUI.Config.TouchThreshold = 20;
```

#### Touch Click Enabled

``` csharp
AntdUI.Config.TouchClickEnabled = true;
```

#### Touch Keyboard Mode

> Default `Accessibility`; options are `Disabled`, `OpenOsk` (invoke the system on-screen keyboard)

``` csharp
AntdUI.Config.TouchKeyboard = AntdUI.TouchKeyboardMode.Accessibility;
```

#### Inertial Scrolling Parameters

> Used to tune the inertial scrolling feel of lists and scrollable areas

``` csharp
AntdUI.Config.TouchVelocityMin = .1F;   // Minimum velocity to trigger inertial scrolling (pixels/ms)
AntdUI.Config.TouchVelocityEnd = .01F;  // Minimum velocity to stop inertial scrolling (pixels/ms)
AntdUI.Config.TouchInterval = 20;       // Inertial scrolling frame interval (ms)
AntdUI.Config.TouchDecay = .94F;        // Inertial scrolling per-frame decay factor
```

### Mouse Hover Delay

> Delay before mouse hover triggers, default `100` milliseconds

``` csharp
AntdUI.Config.MouseHoverDelay = 100;
```

### Shadow

#### Shadow Enabled

> Default shadow on

``` csharp
AntdUI.Config.ShadowEnabled = false;
```

#### Shadow Compatible Mode (supports multiple colors)

``` csharp
AntdUI.Config.ShadowModeCompatible = true;
```

#### Shadow Size and Opacity

``` csharp
AntdUI.Config.ShadowSize = 10;       // Shadow size, default 10 pixels
AntdUI.Config.ShadowOpacity = 0.2F;  // Shadow opacity, default 0.2
```

### Focus Border

> Focus border is enabled by default

``` csharp
AntdUI.Config.FocusBorderEnabled = false;
```

### Dropdown Margin

> Make the dropdown appear further away (to avoid covering content)

``` csharp
AntdUI.Config.DropDownMarginFurther = true;
```

### Emoji

#### Emoji Enabled

> Default Emoji enabled

``` csharp
AntdUI.Config.EmojiEnabled = true;
```

#### Emoji Font

> Default `Segoe UI Emoji`

``` csharp
AntdUI.Config.EmojiFont = "Segoe UI Emoji";
```

### Use Hook

> Enabled by default in Release (disabled by default in DEBUG), used for global message hooks

``` csharp
AntdUI.Config.UseHook = true;
```

### ScrollBar

#### ScrollBar Hidden Style

> Default continuous display `false`

``` csharp
AntdUI.Config.ScrollBarHide = false;
```

#### MinimumSize of ScrollBar

> `ScrollMinSizeY` defaults to `30`, `ScrollMinSizeX` defaults to `null`

``` csharp
AntdUI.Config.ScrollMinSizeY = 30;
AntdUI.Config.ScrollMinSizeX = 30;
```

### Popup in the window Message/Notification

> Default screen popup

``` csharp
AntdUI.Config.ShowInWindow = true;
```

<details>
<summary>Separate Config</summary>

> Popup in the window (Message)
``` csharp
AntdUI.Config.ShowInWindowByMessage = true;
```

> Popup in the window (Notification)
``` csharp
AntdUI.Config.ShowInWindowByNotification = true;
```

</details>

### Message/Notification Boundary Offset XY

> Default 0

``` csharp
AntdUI.Config.NoticeWindowOffsetXY = 0;
```

### Close Message/Notification immediately after exceeding the display limit

> Default, wait for old ones to close before displaying

``` csharp
AntdUI.Config.NoticeOverflowClose = false;
```

### Text Rendering Quality

``` csharp
AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
```

### Text Rendering HighQuality

``` csharp
AntdUI.Config.TextRenderingHighQuality = true;
```

### Default Font 🔴

> This property is marked Obsolete. For a global font, use the project-level default font setting instead

``` csharp
AntdUI.Config.Font = new Font("Microsoft YaHei UI", 10);
```

### Get DPI

> 1=100%、1.25=125%，and so on

``` csharp
float dpi = AntdUI.Config.Dpi;
```

### DPI Mode

> Default `DpiMode.Default`; switch to `DpiMode.Compatible` when layout is abnormal. See [DPI](DPI.md)

``` csharp
AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;
```

### Custom DPI

> Pass `null` to restore automatic detection

``` csharp
AntdUI.Config.SetDpi(1.5F);
AntdUI.Config.SetDpi(null); // restore automatic
```

### Set Correction Text Rendering

``` csharp
AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "Microsoft YaHei"); //List of fonts that need to be corrected
```

![CorrectionTextRendering](Img/CorrectionTextRendering.jpg)

### Empty Image

> Placeholder image used for loading / empty states. You can set the image and aspect ratio, or specify SVG for light / dark separately

``` csharp
AntdUI.Config.EmptyImage = myImage;      // Custom empty image
AntdUI.Config.EmptyImageRatio = 2.98F;   // Aspect ratio, default 2.98

AntdUI.Config.SetEmptyImageSvg(lightSvg, darkSvg); // Set light / dark SVG
AntdUI.Config.ClearEmptyImageSvg();                // Clear custom SVG
```

### Null Text Placeholder

> The constant `Config.NullText` is used internally to distinguish real empty values (normally no need to change)

``` csharp
string nullText = AntdUI.Config.NullText;
```