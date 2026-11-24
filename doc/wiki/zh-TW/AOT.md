[首頁](Home.md)・[更新日誌](../zh/UpdateLog.md)・[設定](../zh/Config.md)・[主題](../zh/Theme.md)

## 前言

[Winforms](https://github.com/dotnet/winforms) 目前在 `NET 9.0` 和 `NET 10.0` 並不支援跨平台，僅支援在 Windows 上執行的 AOT 編譯。若想瞭解更多資訊，可以追蹤官方團隊的動態。如果仍需要考慮跨平台，請參考以下專案：

- [Avalonia](https://github.com/avaloniaui/avalonia)
- [.NET MAUI](https://github.com/dotnet/maui)
- [Electron](https://github.com/electron/electron)

不想改動太多？

- [Gtk](https://github.com/mono/gtk-sharp) `👏 最簡單`
- [CPF](https://github.com/wsxhm/CPF) `⚠ 設計工具需付費`
- [Modern.Forms](https://github.com/modern-forms/Modern.Forms)

> 它們都是不錯的 Winforms 接班人

## AOT

> [Winforms](https://github.com/dotnet/winforms) 目前在 `.NET 9.0` 及以下版本不支援 AOT。在 `csproj` 中加入 `_SuppressWinFormsTrimError = true` 才能忽略不支援的提示
> 
> **重要提示**：在 `.NET 10.0` 版本中，WinForms 已內建對 AOT 的支援，**不需要**加入 `WinFormsComInterop` 參考

### 1、加入 [WinFormsComInterop](https://github.com/kant2002/WinFormsComInterop) 參考（僅適用於 .NET 9.0 及以下版本）

**僅在您使用 .NET 9.0 或更低版本時才需要此步驟**

**在 NuGet 搜尋 `WinFormsComInterop`**，加入專案參考，並在 `Program.cs` 中加入以下程式碼：

``` csharp
ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
```

如果有參考 [WebView2](https://aka.ms/webview)，則改為以下程式碼：

> 注意：即使是最新版 `0.5.0`，仍有大部分 API 無法正常使用，請自行測試。

``` csharp
ComWrappers.RegisterForMarshalling(WinFormsComInterop.WebView2.WebView2ComWrapper.Instance);
```

### 2、修改 `csproj` 檔案

``` xml
<PropertyGroup>
  <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
  <CustomResourceTypesSupport>true</CustomResourceTypesSupport>
  <PublishTrimmed>true</PublishTrimmed>
  <_SuppressWinFormsTrimError>true</_SuppressWinFormsTrimError>
  <PublishAot>true</PublishAot>
</PropertyGroup>
```

### 3、加入 `rd.xml`

> 若加入資源檔後無法啟動
> 請將以下 `rd.xml` 檔案加入專案中。

> 本範例僅支援資源檔，若部分 DLL 不正常，仍需依內容調整

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

⚠ 修改 `csproj` 檔案

``` xml
<ItemGroup>
  <RdXmlFile Include="rd.xml" />
</ItemGroup>
```

### 4、支援 Win7 或更低版本

**在 NuGet 搜尋 `VC-LTL` 與 `YY-Thunks`**，加入專案參考

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

### 5、其他問題

#### 完成以上步驟後，依舊無法編譯

請確認已安裝 C++ 編譯工具集（如 `使用 C++ 的桌面開發`）。

#### 可以執行但部分功能異常，例如 JSON 序列化異常

> JSON 原始碼產生器：Json 序列化請使用原始碼產生器的方式建置。

> 僅 `System.Text.Json` 支援

新增一個實體類別
```csharp
public class TestModel
{
}
```

建立原始碼產生器
```csharp
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TestModel))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

```

將 Json 字串轉為實體物件
```csharp
var data = System.Text.Json.JsonSerializer.Deserialize(json_string, SourceGenerationContext.Default.TestModel);
```

#### Table 顯示空白

Demo 有表格 AOT 範例，在發行頁面也可以下載 AOT 產生後的 exe，支援 MVVM，AOT 不支援反射。[example/Demo/Controls/TableAOT.cs](https://gitee.com/AntdUI/AntdUI/blob/main/example/Demo/Controls/TableAOT.cs#L348)