[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

## 前言

[Winforms](https://github.com/dotnet/winforms) 目前 `NET 9.0` 并不支持跨平台，仅仅是支持在 Windows 上运行的 AOT 编译。要了解更多信息，您可以跟踪官方团队。如果仍需要考虑跨平台请参考如下：

- [Avalonia](https://github.com/avaloniaui/avalonia)
- [.NET MAUI](https://github.com/dotnet/maui)
- [Electron](https://github.com/electron/electron)

不想改变太多？

- [Gtk](https://github.com/mono/gtk-sharp) `👏 最简单`
- [CPF](https://github.com/wsxhm/CPF) `⚠ 设计器收费`
- [Modern.Forms](https://github.com/modern-forms/Modern.Forms)

> 他们是不错的 Winforms 继任者

## AOT

> [Winforms](https://github.com/dotnet/winforms) 目前是不支持AOT的。`csproj` 中添加 `_SuppressWinFormsTrimError = true` 才可以互忽略不支持提示

### 1、添加 [WinFormsComInterop](https://github.com/kant2002/WinFormsComInterop) 引用

**NuGet 搜索 `WinFormsComInterop`**，引用到项目中并在 `Program.cs` 中添加以下代码：

``` csharp
ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
```

如果引用了 [WebView2](https://aka.ms/webview) 则改为以下代码：

> 注意：即便是最新版 `0.5.0` 仍有大部分API无法正常使用，请自行测试。

``` csharp
ComWrappers.RegisterForMarshalling(WinFormsComInterop.WebView2.WebView2ComWrapper.Instance);
```

### 2、修改 `csproj` 文件

``` xml
<PropertyGroup>
  <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
  <CustomResourceTypesSupport>true</CustomResourceTypesSupport>
  <PublishTrimmed>true</PublishTrimmed>
  <_SuppressWinFormsTrimError>true</_SuppressWinFormsTrimError>
  <PublishAot>true</PublishAot>
</PropertyGroup>
```

### 3、添加 `rd.xml`

> 如资源文件后无法启动
> 请添加以下 `rd.xml` 文件到项目中。

> 本示例仅支持资源文件，若部分DLL不正常任需根据内容调整

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

⚠ 修改 `csproj` 文件

``` xml
<ItemGroup>
  <RdXmlFile Include="rd.xml" />
</ItemGroup>
```

### 4、支持Win7或更低版本

**NuGet 搜索 `VC-LTL` 与 `YY-Thunks`**，引用到项目中

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

### 5、其他问题

#### 以上步骤完成后，依旧无法编译

确保C++编译工具集（如`C++桌面开发`）已安装。

#### 能运行但部分功能异常如JSON序列化异常

> JSON 源生成器，对于Json序列化使用源生成器方式构建。

> 仅 `System.Text.Json` 支持

新建一个实体类
```csharp
public class TestModel
{
}
```

创建源生成器
```csharp
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TestModel))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

```

将Json字符串转为实体对象
```csharp
var data = System.Text.Json.JsonSerializer.Deserialize(json_string, SourceGenerationContext.Default.TestModel);
```

#### Table 显示空白

Demo有表格AOT示例，并且在发布页面也可以下载AOT生成后的exe，支持MVVM，AOT不支持反射。[example/Demo/Controls/TableAOT.cs](https://gitee.com/AntdUI/AntdUI/blob/main/example/Demo/Controls/TableAOT.cs#L348)