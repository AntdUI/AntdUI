[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)

## DPI Adaptation Guide

### Core Requirements

> To achieve DPI adaptation, **you must inherit from AntdUI window classes**, such as:
> - `AntdUI.BaseForm`
> - `AntdUI.Window`
> - `AntdUI.BorderlessForm`
> 
> These window classes have built-in DPI scaling logic, and `AutoHandDpi = true` automatically enables scaling functionality

### Adaptation Steps

#### 1. Designer Scaling Settings (Required for version 2.2.10 and below, optional for version 2.2.10 and above)

> **For version 2.2.10 and above**：Already adapted to `AutoScaleMode.Dpi` and `AutoScaleMode.Font`, **no need to force 100% scaling design**, you can design the interface directly at current DPI
> 
> **For version 2.2.10 and below**：**Must use 100% scaling to design the interface**, otherwise it may not display completely at other DPI multiples

##### Setting Methods (For version 2.2.10 and below or when needed)

#### .NET Core Series 👏

[Fix HDPI/scaling issues with Windows Forms Designer in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/designers/disable-dpi-awareness?view=vs-2022)

> In Visual Studio 2022 version 17.8 or later, set the property `ForceDesignerDPIUnaware` in the project file `.csproj` to `true`

```xml
<PropertyGroup>
   <ForceDesignerDPIUnaware>true</ForceDesignerDPIUnaware>
</PropertyGroup>
```

**Method 2: Start VS via Command Line**
```shell
devenv.exe /noScale
```

**Method 3: Modify System Scaling**
Change Windows desktop scaling to `100%`

#### 2. Enable DPI Awareness

> **DPI awareness must be enabled**, otherwise the application will be forcibly scaled by the system on high DPI displays, resulting in blurriness

##### .NET Core Series

> [Application.SetHighDpiMode(HighDpiMode.SystemAware)](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)

```csharp
internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Set DPI awareness mode, choose the one that suits you
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        
        Application.Run(new YourForm()); // Ensure YourForm inherits from AntdUI window class
    }
}
```

##### .NET Framework
Enable high DPI support through manifest file, see [High DPI support in Windows Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)

### Supported DPI Awareness Modes

| Mode | Description |
| :-- | :-- |
| `PerMonitor` | Current monitor DPI |
| `PerMonitorV2` | Current monitor DPI (Enhanced, recommended) |
| `DpiUnawareGdiScaled` | Fully unaware (GDI optimized) |
| `SystemAware` | System DPI |
| `DpiUnaware` | Fully unaware |

### Layout Exception Handling

> If layout issues still occur after enabling DPI awareness, try the following solutions:

#### 1. Use Recommended DPI Adaptation

> Need to remove/restore the default value of `AutoScaleMode` in each `.Designer.cs` file. Removing `AutoScaleDimensions` and `AutoScaleFactor` also has no effect

#### 2. Switch DPI Mode

```csharp
// Set at program startup
AntdUI.Config.DpiMode = DpiMode.Compatible;
```

### Other Optimization Suggestions

1. **Font Blurriness Issue**
   ```csharp
   AntdUI.Config.TextRenderingHighQuality = true;
   ```

2. **Font Vertical Centering Issue**
   ```csharp
   AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "Microsoft YaHei");
   ```