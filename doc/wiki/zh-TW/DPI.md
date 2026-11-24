[首頁](Home.md)・[更新日誌](../zh/UpdateLog.md)・[設定](Config.md)・[主題](Theme.md)

# DPI 適應指南

> 本文適合剛接觸 WinForms 的開發者，從「為什麼」講起，再提供可分步驟照做的設定方案。不同 .NET 版本的做法不同，請依你的專案類型選擇對應章節。

## 一、先搞懂：為什麼需要 DPI 適應？

Windows 允許使用者在「顯示設定」裡調整縮放比例（100%、125%、150%、200%……）。縮放後，1 個「邏輯像素」不再等於 1 個「實體像素」。如果程式沒有宣告自己支援縮放，Windows 會把整個介面當成一張圖片放大——**文字和圖示就會變得模糊**；如果宣告了卻處理不當，元件又會錯位、顯示不完整。

所以 DPI 適應要解決兩件事：

| 問題 | 由誰負責 | 解決辦法 |
| :-- | :-- | :-- |
| ① 告訴系統「我支援高 DPI，別把我當圖片拉伸」 | 你的程式（啟動時宣告） | 啟用 DPI 感知（本文步驟二） |
| ② 介面元素依縮放比例正確排列、不變形 | AntdUI 視窗類別 + 設計工具設定 | 繼承 AntdUI 視窗（步驟一）+ 設計工具設定（步驟三） |

> 兩個步驟都做對，介面才會既清晰又不錯位。

## 二、三步驟總覽

1. **繼承 AntdUI 視窗類別** —— 讓 AntdUI 接管縮放邏輯
2. **啟用 DPI 感知** —— 依 .NET Core / .NET Framework 選擇對應做法
3. **處理設計工具縮放** —— 依 AntdUI 版本決定是否需要以 100% 設計

以下逐步說明。

## 步驟一：繼承 AntdUI 視窗類別

> 🔴 這是 AntdUI 適應 DPI 的**前提**，未滿足的話，後續設定的效果有限。

讓視窗繼承 AntdUI 提供的視窗類別，而不是標準的 `System.Windows.Forms.Form`：

- `AntdUI.BaseForm`
- `AntdUI.Window`
- `AntdUI.BorderlessForm`

這些視窗類別內部已處理 DPI 縮放邏輯，`AutoHandDpi = true`（預設開啟）時會自動縮放視窗及子元件，並回應執行中切換顯示器 DPI 的情況。

```csharp
// 修改視窗基底類別的繼承來源
public partial class Form1 : AntdUI.BaseForm
{
	public Form1()
	{
		InitializeComponent();
	}
}
```

> 如需關閉自動縮放，可設定 `AutoHandDpi = false`，一般不建議。

## 步驟二：啟用 DPI 感知（依框架選擇）

> **為什麼要做**：若程式沒有宣告 DPI 感知，Windows 會將其視為「不感知 DPI」，在高 DPI 螢幕上直接以點陣圖拉伸，導致介面模糊。此步驟必須在**建立任何視窗之前**完成。

請依你的專案目標框架選擇：

### A. .NET Core / .NET 5 及以上（建議）👏

包含 .NET 5/6/7/8/9/10。

#### 方式 1：專案屬性（.NET 6+ 建議，最簡單）

在專案檔 `.csproj` 中設定 `ApplicationHighDpiMode`：

```xml
<PropertyGroup>
   <OutputType>WinExe</OutputType>
   <TargetFramework>net8.0-windows</TargetFramework>
   <UseWindowsForms>true</UseWindowsForms>
   <ApplicationHighDpiMode>PerMonitorV2</ApplicationHighDpiMode>
</PropertyGroup>
```

新版 WinForms 範本的 `Program.cs` 會自動產生 `ApplicationConfiguration.Initialize();`，它在編譯時依專案設定產生，內部已包含 DPI 感知設定：

```csharp
internal static class Program
{
	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize(); // 已包含 DPI 感知、字型等初始化
		Application.Run(new Form1());
	}
}
```

#### 方式 2：以程式碼明確設定（.NET Core 3.x / 需要精確控制）👏

在 `Main` 中、**建立任何視窗之前**呼叫 `Application.SetHighDpiMode`：

```csharp
internal static class Program
{
	[STAThread]
	static void Main()
	{
		// 🔴 必須在 Application.Run / 建立視窗之前呼叫，且每個處理序只能設定一次
		Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

		Application.Run(new Form1()); // Form1 需繼承 AntdUI 視窗類別
	}
}
```

> **為什麼必須最早呼叫**：高 DPI 模式每個處理序只能設定一次；一旦建立了視窗（控制代碼），就無法再變更。若專案同時以資訊清單檔宣告了 DPI 模式，此方法將無法覆寫清單中的設定。
>
> 官方文件：[Application.SetHighDpiMode](https://learn.microsoft.com/zh-tw/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)・[.NET 6 起的專案層級設定](https://learn.microsoft.com/zh-tw/dotnet/desktop/winforms/whats-new/net60?view=netdesktop-8.0#project-level-application-settings)

### B. .NET Framework

.NET Framework 無法在程式碼裡呼叫 `SetHighDpiMode`，需透過設定檔宣告。需要 **.NET Framework 4.7+**，且執行在 **Windows 10 Creators Update（1703）及以上**。

#### 方式 1：app.config（4.7+ 建議）

在 `App.config` 中加入以下設定，宣告 `PerMonitorV2`：

```xml
<configuration>
   <startup>...</startup>

   <System.Windows.Forms.ApplicationConfigurationSection>
	  <add key="DpiAwareness" value="PerMonitorV2" />
   </System.Windows.Forms.ApplicationConfigurationSection>
</configuration>
```

同時建議在 `app.manifest` 中宣告相容 Windows 10（否則部分功能不會生效）：

```xml
<compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
   <application>
	  <!-- Windows 10 -->
	  <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" />
   </application>
</compatibility>
```

進入點 `Main` 維持一般寫法即可：

```csharp
[STAThread]
static void Main()
{
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);
	Application.Run(new Form1());
}
```

#### 方式 2：app.manifest（舊專案相容）

在 `app.manifest` 的 `<application>` 節點下加入：

```xml
<application xmlns="urn:schemas-microsoft-com:asm.v3">
   <windowsSettings>
	  <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true/pm</dpiAware>
	  <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">permonitorv2</dpiAwareness>
   </windowsSettings>
</application>
```

> **為什麼建議 app.config 而非 manifest**：manifest 中的 DPI 設定會**覆寫** app.config 中的宣告，導致 app.config 失效。新式專案請優先使用 app.config 或專案屬性，方便統一管理。

官方參考：[Windows Forms 中的高 DPI 支援](https://learn.microsoft.com/zh-tw/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)

## 步驟三：設計工具縮放設定

> **為什麼要做**：Visual Studio 設計工具本身在不同縮放比例的電腦上開啟視窗時，若未統一基準，元件的座標和大小會依設計時的 DPI 寫入 `.Designer.cs`，到其他縮放比例的電腦上就可能對不齊或顯示不完整。

### AntdUI 2.2.10 及以上（建議升級）

已支援 `AutoScaleMode.Dpi` 和 `AutoScaleMode.Font`，**不需強制以 100% 縮放設計**，可直接在目前的 DPI 下設計介面。

### AntdUI 2.2.10 以下

**必須以 100% 縮放設計介面**，否則在其他 DPI 倍數下可能顯示不完整。讓設計工具「以為」自己在 100% 縮放下運作：

#### .NET Core / .NET 5+

在專案檔 `.csproj` 中設定 `ForceDesignerDPIUnaware`：

```xml
<PropertyGroup>
   <ForceDesignerDPIUnaware>true</ForceDesignerDPIUnaware>
</PropertyGroup>
```

或以下兩種方式擇一：

- **以命令列啟動 VS**：`devenv.exe /noScale`
- **修改系統縮放**：暫時將 Windows 桌面縮放改為 `100%`

官方參考：[修正 Visual Studio 中 Windows Forms 設計工具的 HDPI/縮放問題](https://learn.microsoft.com/zh-tw/visualstudio/designers/disable-dpi-awareness?view=vs-2022)

#### .NET Framework

將 Windows 桌面縮放暫時改為 `100%` 後，再用設計工具編輯視窗；或升級到 AntdUI 2.2.10+ 以避開此限制。

## DPI 感知模式速查

| 模式 | 說明 | 備註 |
| :-- | :-- | :-- |
| `PerMonitorV2` | 目前顯示器 DPI（增強版） | ✅ 建議，支援跨螢幕、動態 DPI 變化 |
| `PerMonitor` | 目前顯示器 DPI | 支援多螢幕不同縮放 |
| `SystemAware` | 系統 DPI | 所有螢幕一致，啟動時決定 |
| `DpiUnawareGdiScaled` | 不感知（GDI 最佳化縮放） | 由 GDI 縮放，文字相對清晰 |
| `DpiUnaware` | 完全不感知 | ❌ 會以點陣圖拉伸而導致模糊 |

## AntdUI 的 DpiMode（版面相容開關）

除了系統層級的 DPI 感知，AntdUI 本身還提供 `Config.DpiMode` 控制內部縮放策略：

| 值 | 說明 |
| :-- | :-- |
| `DpiMode.Default` | 預設模式，搭配繼承的視窗類別自動處理縮放 |
| `DpiMode.Compatible` | 相容模式，使用整體 `Scale` 縮放，用於版面異常時的備用方案 |

```csharp
// 在程式啟動時設定（一般不需修改）
AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;
```

> 取得 / 自訂 DPI：
> ```csharp
> float dpi = AntdUI.Config.Dpi;       // 1=100%、1.25=125%，以此類推
> AntdUI.Config.SetDpi(1.5F);          // 自訂 DPI
> AntdUI.Config.SetDpi(null);          // 恢復自動取得
> ```

## 常見問題排查

### 1. 字型模糊

```csharp
AntdUI.Config.TextRenderingHighQuality = true;
```

也可調整文字呈現品質：

```csharp
AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
```

### 2. 字型垂直置中問題

部分字型（如宋體）在高 DPI 下算繪會有偏移，可指定需要修正的字型清單：

```csharp
AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "宋体");
```

![CorrectionTextRendering](../zh/Img/CorrectionTextRendering.jpg)

### 3. 版面異常 / 元件錯位

請依序嘗試：

1. 確認視窗已繼承 AntdUI 視窗類別（步驟一）；
2. 確認已正確啟用 DPI 感知（步驟二）；
3. 將各 `.Designer.cs` 中的 `AutoScaleMode` 移除或還原為預設值（移除 `AutoScaleDimensions`、`AutoScaleFactor` 也不受影響）；
4. 切換 AntdUI 相容模式：`AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;`

## 官方參考

- [Windows Forms 中的高 DPI 支援](https://learn.microsoft.com/zh-tw/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)
- [Application.SetHighDpiMode](https://learn.microsoft.com/zh-tw/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)
- [修正 VS 設計工具 HDPI/縮放問題](https://learn.microsoft.com/zh-tw/visualstudio/designers/disable-dpi-awareness?view=vs-2022)