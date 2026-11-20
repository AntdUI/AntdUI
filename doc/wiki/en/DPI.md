[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)

# DPI Adaptation Guide

> This guide is written for developers new to WinForms. It starts with the "why" and then gives step-by-step instructions. The approach differs by .NET version — choose the section that matches your project type.

## 1. First, understand: why DPI adaptation is needed?

Windows lets users change the display scale (100%, 125%, 150%, 200%, ...). After scaling, one "logical pixel" no longer equals one "physical pixel". If a program doesn't declare that it understands scaling, Windows enlarges the whole UI like an image — **text and icons become blurry**. If it declares support but handles it poorly, controls become misaligned or clipped.

So DPI adaptation solves two problems:

| Problem | Who is responsible | Solution |
| :-- | :-- | :-- |
| ① Tell the system "I support high DPI, don't stretch me like an image" | Your app (declared at startup) | Enable DPI awareness (step 2) |
| ② Lay out UI elements correctly at the scale factor | AntdUI window classes + designer settings | Inherit an AntdUI window (step 1) + designer settings (step 3) |

> Both steps must be done correctly for the UI to be sharp and well-aligned.

## 2. Three steps overview

1. **Inherit an AntdUI window class** — let AntdUI handle scaling
2. **Enable DPI awareness** — choose the approach for .NET Core / .NET Framework
3. **Handle designer scaling** — decide whether 100% design is needed based on the AntdUI version

Each step is explained below.

## Step 1: Inherit an AntdUI window class

> 🔴 This is a **prerequisite** for AntdUI's DPI adaptation; without it the later settings have limited effect.

Make your form inherit from an AntdUI window class instead of the standard `System.Windows.Forms.Form`:

- `AntdUI.BaseForm`
- `AntdUI.Window`
- `AntdUI.BorderlessForm`

These window classes have built-in DPI scaling logic. With `AutoHandDpi = true` (the default), they automatically scale the form and child controls, and respond when the user moves the window to a monitor with a different DPI.

```csharp
// Change the base class of the form
public partial class Form1 : AntdUI.BaseForm
{
	public Form1()
	{
		InitializeComponent();
	}
}
```

> To disable automatic scaling, set `AutoHandDpi = false` — not recommended in general.

## Step 2: Enable DPI awareness (choose by framework)

> **Why**: If the app doesn't declare DPI awareness, Windows treats it as DPI-unaware and bitmap-stretches it on high-DPI screens, causing blurriness. This must be done **before any form is created**.

Choose based on your project's target framework:

### A. .NET Core / .NET 5+ (recommended) 👏

Includes .NET 5/6/7/8/9/10.

#### Option 1: Project property (.NET 6+, simplest)

Set `ApplicationHighDpiMode` in the project file `.csproj`:

```xml
<PropertyGroup>
   <OutputType>WinExe</OutputType>
   <TargetFramework>net8.0-windows</TargetFramework>
   <UseWindowsForms>true</UseWindowsForms>
   <ApplicationHighDpiMode>PerMonitorV2</ApplicationHighDpiMode>
</PropertyGroup>
```

The `Program.cs` of newer WinForms templates automatically generates `ApplicationConfiguration.Initialize();`, which is generated at compile time from project settings and already includes DPI awareness configuration:

```csharp
internal static class Program
{
	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize(); // includes DPI awareness, fonts, etc.
		Application.Run(new Form1());
	}
}
```

#### Option 2: Set in code (.NET Core 3.x / need precise control)👏

In `Main`, call `Application.SetHighDpiMode` **before creating any form**:

```csharp
internal static class Program
{
	[STAThread]
	static void Main()
	{
		// 🔴 Must be called before Application.Run / creating any form; once per process
		Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

		Application.Run(new Form1()); // Form1 must inherit an AntdUI window class
	}
}
```

> **Why it must be earliest**: The high DPI mode can only be set once per process; once a form (handle) is created it can't be changed. If the project also declares a DPI mode in a manifest, this method cannot override the manifest setting.
>
> Official docs: [Application.SetHighDpiMode](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)・[Project-level settings since .NET 6](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/whats-new/net60?view=netdesktop-8.0#project-level-application-settings)

### B. .NET Framework

.NET Framework cannot call `SetHighDpiMode` in code; declare it via configuration files. Requires **.NET Framework 4.7+** running on **Windows 10 Creators Update (1703) or later**.

#### Option 1: app.config (recommended for 4.7+)

Add the following to `App.config` to declare `PerMonitorV2`:

```xml
<configuration>
   <startup>...</startup>

   <System.Windows.Forms.ApplicationConfigurationSection>
	  <add key="DpiAwareness" value="PerMonitorV2" />
   </System.Windows.Forms.ApplicationConfigurationSection>
</configuration>
```

It's also recommended to declare Windows 10 compatibility in `app.manifest` (otherwise some features don't take effect):

```xml
<compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
   <application>
	  <!-- Windows 10 -->
	  <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" />
   </application>
</compatibility>
```

The entry point `Main` stays conventional:

```csharp
[STAThread]
static void Main()
{
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);
	Application.Run(new Form1());
}
```

#### Option 2: app.manifest (for legacy projects)

Add the following under the `<application>` node in `app.manifest`:

```xml
<application xmlns="urn:schemas-microsoft-com:asm.v3">
   <windowsSettings>
	  <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true/pm</dpiAware>
	  <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">permonitorv2</dpiAwareness>
   </windowsSettings>
</application>
```

> **Why app.config is preferred over manifest**: DPI settings in the manifest **override** those in app.config, making app.config ineffective. Modern projects should prefer app.config or project properties for unified management.

Official reference: [High DPI support in Windows Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)

## Step 3: Designer scaling settings

> **Why**: Visual Studio Designer opens forms at different scales on different machines. Without a consistent baseline, control coordinates and sizes are written into `.Designer.cs` at the design-time DPI and may be misaligned or clipped on machines with other scales.

### AntdUI 2.2.10+ (recommended upgrade)

Already adapted to `AutoScaleMode.Dpi` and `AutoScaleMode.Font`, so **there's no need to force 100% scaling** — you can design at the current DPI.

### AntdUI below 2.2.10

**You must design at 100% scaling**, otherwise the UI may be clipped at other DPI factors. Make the designer "think" it's at 100%:

#### .NET Core / .NET 5+

Set `ForceDesignerDPIUnaware` in `.csproj`:

```xml
<PropertyGroup>
   <ForceDesignerDPIUnaware>true</ForceDesignerDPIUnaware>
</PropertyGroup>
```

Or choose either alternative:

- **Start VS from the command line**: `devenv.exe /noScale`
- **Change system scaling**: temporarily set Windows desktop scaling to `100%`

Official reference: [Fix HDPI/scaling issues with Windows Forms Designer](https://learn.microsoft.com/en-us/visualstudio/designers/disable-dpi-awareness?view=vs-2022)

#### .NET Framework

Temporarily set Windows desktop scaling to `100%` before editing forms in the designer; or upgrade to AntdUI 2.2.10+ to avoid this limitation.

## DPI awareness modes quick reference

| Mode | Description | Notes |
| :-- | :-- | :-- |
| `PerMonitorV2` | Current monitor DPI (enhanced) | ✅ Recommended; supports cross-monitor and dynamic DPI changes |
| `PerMonitor` | Current monitor DPI | Supports different scales per monitor |
| `SystemAware` | System DPI | Uniform across screens, decided at startup |
| `DpiUnawareGdiScaled` | Unaware (GDI-optimized scaling) | GDI scaling, relatively sharp text |
| `DpiUnaware` | Fully unaware | ❌ bitmap-stretched, causes blurriness |

## AntdUI DpiMode (layout compatibility switch)

Beyond system-level DPI awareness, AntdUI provides `Config.DpiMode` to control its internal scaling strategy:

| Value | Description |
| :-- | :-- |
| `DpiMode.Default` | Default mode, automatically handles scaling with inherited window classes |
| `DpiMode.Compatible` | Compatible mode, uses overall `Scale` scaling as a fallback for abnormal layouts |

```csharp
// Set at program startup (normally no need to change)
AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;
```

> Get / customize DPI:
> ```csharp
> float dpi = AntdUI.Config.Dpi;       // 1=100%, 1.25=125%, and so on
> AntdUI.Config.SetDpi(1.5F);          // custom DPI
> AntdUI.Config.SetDpi(null);          // restore automatic detection
> ```

## Troubleshooting

### 1. Blurry text

```csharp
AntdUI.Config.TextRenderingHighQuality = true;
```

You can also adjust the text rendering quality:

```csharp
AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
```

### 2. Vertical text centering issue

Some fonts (e.g. SimSun) render with an offset at high DPI; specify the fonts to correct:

```csharp
AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "Microsoft YaHei");
```

![CorrectionTextRendering](Img/CorrectionTextRendering.jpg)

### 3. Abnormal layout / misaligned controls

Try in order:

1. Confirm the form inherits an AntdUI window class (step 1);
2. Confirm DPI awareness is enabled correctly (step 2);
3. Remove or restore the default value of `AutoScaleMode` in each `.Designer.cs` (removing `AutoScaleDimensions` and `AutoScaleFactor` also has no effect);
4. Switch AntdUI compatible mode: `AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;`

## Official references

- [High DPI support in Windows Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)
- [Application.SetHighDpiMode](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)
- [Fix HDPI/scaling issues with VS Designer](https://learn.microsoft.com/en-us/visualstudio/designers/disable-dpi-awareness?view=vs-2022)