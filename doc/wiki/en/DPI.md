[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)d)

## DPI

> According to document 3, DPI adaptation can be completed, provided that the window inherits the [BaseForm](Form/BaseForm.md), 
> [Window](Form/Window.md) / [BorderlessForm](Form/BorderlessForm.md) both inherit from [BaseForm](Form/BaseForm.md), and `AutoHandDpi = true` will enable scaling

### 1. Modify VS scaling

> **The interface should be designed with 100% zoom**, otherwise it will not display completely at other resolution multiples

#### .NET Core Series 👏

[Fix HDPI/scaling issues with Windows Forms Designer in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/designers/disable-dpi-awareness?view=vs-2022)

> In Visual Studio 2022 version 17.8 or later, set the property `ForceDeviceDPIUnaware` in the project file `.csproj` to `true`
>
>```xml
><PropertyGroup>
>   ...
>   <ForceDesignerDPIUnaware>true</ForceDesignerDPIUnaware>
></PropertyGroup>

#### Starting VS with CMD

> Can create fixed shortcuts

```shell
devenv.exe /noScale
```

#### Modify system scaling

Windows desktop right-click display settings will change zoom to `100%`


### 2. Enable DPI awareness

#### .NET Core Series 👏

> [Application.SetHighDpiMode(HighDpiMode.SystemAware)](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)
> ``` csharp
> internal static class Program
> {
>     /// <summary>
>     ///  The main entry point for the application.
>     /// </summary>
>     [STAThread]
>     static void Main()
>     {
>         ...
>         Application.SetHighDpiMode(HighDpiMode.SystemAware);
>         Application.Run(new Form1());
>     }
> }
> ```

#### .NET Framework

> high DPI support in Windows Forms needs to be enabled through a checklist [High DPI support in Windows Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)

### 3. Why is the designer and compiled layout inconsistent under HDPI

> Take each one Remove/restore the default value of `AutoScaleMode` in `.Designer.cs`, and removing `AutoScaleFactor` is not affected

---

### 4. Other issues

#### After adapting to DPI, the font still appears blurry

> [Resolve the issue of blurry fonts](BlurredFont.md)

#### The font has jagged edges （beta🔴）

> [AntdUI.Config.TextRenderingHighQuality](Config.md#TextRenderingHighQuality)
> ``` csharp
> internal static class Program
> {
>     /// <summary>
>     ///  The main entry point for the application.
>     /// </summary>
>     [STAThread]
>     static void Main()
>     {
>         ...
>         AntdUI.Config.TextRenderingHighQuality = true;
>         Application.Run(new Form1());
>     }
> }
> ```

#### The font is not vertically centered （beta🔴）

> ``` csharp
> AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "Microsoft YaHei"); //List of fonts that need to be corrected
> ```
> ![CorrectionTextRendering](Img/CorrectionTextRendering.jpg)