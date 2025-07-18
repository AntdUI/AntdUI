[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)

## Preface

[Winforms](https://github.com/dotnet/winforms) currently does not support cross platform support in `NET 9.0`, only AOT compilation running on Windows. To learn more information, you can follow the official team. If cross platform considerations are still necessary, please refer to the following:

- [Avalonia](https://github.com/avaloniaui/avalonia)
- [.NET MAUI](https://github.com/dotnet/maui)
- [Electron](https://github.com/electron/electron)

Don't want to change too much?

- [Gtk](https://github.com/mono/gtk-sharp) `👏 Easily`
- [CPF](https://github.com/wsxhm/CPF) `⚠ Designer charge`
- [Modern.Forms](https://github.com/modern-forms/Modern.Forms)

> They are good Winforms successors

## AOT

> [Winforms](https://github.com/dotnet/winforms) currently does not support AOT. Add `_SuppressWinFormsTrimError = true` in `csproj` to ignore unsupported prompts

### 1. Add [WinFormsComInterop](https://github.com/kant2002/WinFormsComInterop) reference

**Search for `WinFormsComInterop` on NuGet**, reference it to the project, and add the following code in `Program.cs`:

``` csharp
ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
```

If [WebView2](https://aka.ms/webview) is referenced, change to the following code:

> Note: Even with the latest version `0.5.0`, most APIs still cannot be used properly. Please test them yourself.

``` csharp
ComWrappers.RegisterForMarshalling(WinFormsComInterop.WebView2.WebView2ComWrapper.Instance);
```

### 2. Modify `csproj` file

``` xml
<PropertyGroup>
  <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
  <CustomResourceTypesSupport>true</CustomResourceTypesSupport>
  <PublishTrimmed>true</PublishTrimmed>
  <_SuppressWinFormsTrimError>true</_SuppressWinFormsTrimError>
  <PublishAot>true</PublishAot>
</PropertyGroup>
```

### 3. Add `rd.xml`

> If the resource file cannot be started, please add the following `rd.xml` file to the project.

> This example only supports resource files. If some DLLs are not working properly, they need to be adjusted according to the content

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<Directives>
  <Application>
    <Assembly Name="System.Resources.Extensions">
      <Type Name="System.Resources.Extensions.RuntimeResourceSet" Dynamic="Required All" />
      <Type Name="System.Resources.Extensions.DeserializingResourceReader" Dynamic="Required All" />
    </Assembly>
    <Assembly Name="System.Drawing">
      <Type Name="System.Drawing.Bitmap" Dynamic="Required All" />
    </Assembly>
  </Application>
</Directives>
```

⚠ Modify `csproj` file

``` xml
<ItemGroup>
  <RdXmlFile Include="rd.xml" />
</ItemGroup>
```

### 4. Support Win7 or lower versions

**NuGet searches `VC-LTL` and `YY-Thunks`**, references to projects

``` xml
<PropertyGroup>
  <WindowsSupportedOSPlatformVersion>5.1</WindowsSupportedOSPlatformVersion>
  <TargetPlatformMinVersion>5.1</TargetPlatformMinVersion>
  <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
  <CustomResourceTypesSupport>true</CustomResourceTypesSupport>
  <PublishTrimmed>true</PublishTrimmed>
  <_SuppressWinFormsTrimError>true</_SuppressWinFormsTrimError>
  <PublishAot>true</PublishAot>
  <OptimizationPreference>Size</OptimizationPreference>
</PropertyGroup>
```

---

### 5. Other issues

#### After completing the above steps, it still cannot be compiled

Ensure that the C++compilation toolset (such as `C++ desktop development`) is installed.

#### Can run, but some features are abnormal, such as JSON serialization exception

> JSON source generator, built using the source generator method for JSON serialization.

> Only `System.Text.Json` is supported

Create a new entity class
```csharp
public class TestModel
{
}
```

Create Source Generator
```csharp
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TestModel))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

```

Convert JSON strings to entity objects
```csharp
var data = System.Text.Json.JsonSerializer.Deserialize(json_string, SourceGenerationContext.Default.TestModel);
```

#### Table shows blank

The demo has table AOT examples, and the AOT generated exe can also be downloaded on the publishing page. It supports MVVM, but AOT does not support reflection. [example/Demo/Controls/TableAOT.cs](https://github.com/AntdUI/AntdUI/blob/main/example/Demo/Controls/TableAOT.cs#L348)