[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)・[SVG](SVG.md)

### Color Mode

> Default Light color mode

#### Set color mode

``` csharp
AntdUI.Config.Mode = AntdUI.TMode.Light;
```

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

### Animation Off

> Default animation on

``` csharp
AntdUI.Config.Animation = false;
```

### Touch Screen Enabled 🔴

> Default Enable touch

``` csharp
AntdUI.Config.TouchEnabled = true;
```

### Shadow Enabled 🔴

> Default shadow on

``` csharp
AntdUI.Config.ShadowEnabled = false;
```

### ScrollBar Hidden Style 🔴

> Default continuous display `false`

``` csharp
AntdUI.Config.ScrollBarHide = false;
```

### MinimumSize of ScrollBar Y 🔴

> Default `30`

``` csharp
AntdUI.Config.ScrollMinSizeY = 30;
```

### Popup in the window Message/Notification

> Default screen popup

``` csharp
AntdUI.Config.ShowInWindow = true;
```

<details>
<summary>Separate Config 🔴</summary>

> Popup in the window（Message）
``` csharp
AntdUI.Config.ShowInWindowByMessage = true;
```

> Popup in the window（Notification）
``` csharp
AntdUI.Config.ShowInWindowByNotification = true;
```

</details>

### Message/Notification Boundary Offset XY

> Default 0

``` csharp
AntdUI.Config.NoticeWindowOffsetXY = 0;
```

### Text Rendering Quality

``` csharp
AntdUI.Config.TextRenderingHint = System.Drawing.Text.ClearTypeGridFit;
```

### Default Font

``` csharp
AntdUI.Config.Font = new Font("Microsoft YaHei UI", 10);
```

### Get DPI

> 1=100%、1.25=125%，and so on

``` csharp
float dpi = AntdUI.Config.Dpi;
```

### Custom DPI

``` csharp
AntdUI.Config.SetDpi(1.5F);
```

### Set Correction Text Rendering

``` csharp
AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "Microsoft YaHei"); //List of fonts that need to be corrected
```

![CorrectionTextRendering](Img/CorrectionTextRendering.jpg)