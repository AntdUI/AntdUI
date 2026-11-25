[首頁](Home.md)・[更新日誌](../zh/UpdateLog.md)・[設定](Config.md)・[主題](Theme.md)

內建 AntDesign 色彩演算法

> 參考網址：[https://ant-design.antgroup.com/docs/spec/colors-cn](https://ant-design.antgroup.com/docs/spec/colors-cn)

---

## 預設品牌色彩

| 模式 | HEX |
| :--: | :--: |
| 淺色 | #1677FF |
| 深色 | #1668DC |

---

## 主題色彩自訂

### 1. 智慧色彩體系產生（建議）

透過以下方法設定基礎色，系統會**自動計算並產生完整的色彩體系**，包括懸停、按下等狀態的顏色變化，確保視覺一致性：

| 方法 | 說明 | 參數 | 自動產生色彩體系 |
| :-- | :-- | :-- | :--: |
| **SetPrimary** | 設定品牌主色 | Color primary | ✅ |
| **SetSuccess** | 設定成功色 | Color success | ✅ |
| **SetWarning** | 設定警告色 | Color warning | ✅ |
| **SetError** | 設定錯誤色 | Color error | ✅ |
| **SetInfo** | 設定訊息色 | Color info | ✅ |

#### 使用範例

``` csharp
// 自動產生完整的品牌色彩體系（包括懸停、按下等狀態）
AntdUI.Style.SetPrimary(Color.FromArgb(0, 173, 154));
```

### 2. 單一顏色值設定

以下方法**僅設定單一顏色值**，不會自動產生色彩體系，適用於需要精確控制特定顏色的情境：

#### 全域設定單一顏色值

``` csharp
// 僅設定單一主色值，不產生色彩體系
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154));
```

#### 為元件單獨設定顏色值

``` csharp
// 僅為 Button 元件設定單一主色值，不產生色彩體系
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154), "Button");
```

### 3. 色彩體系設定檔

透過設定檔批次設定色彩，**建議使用 HEX 格式**：

``` csharp
var colorConfig = new Dictionary<string, string> {
	{ "Primary", "#ED4192" },          // 品牌主色
	{ "PrimaryButton", "#E0282E" },    // 為 Button 單獨設定主色
	{ "Success", "#52C41A" },          // 成功色
	{ "Warning", "#FAAD14" },          // 警告色
	{ "Error", "#F5222D" },            // 錯誤色
	{ "Info", "#1890FF" }              // 訊息色
};
AntdUI.Style.LoadCustom(colorConfig);
```

---

## .NET 10 跟隨系統色彩模式

> 🔴 僅 .NET 10 及以上可用

從 .NET 10 起，WinForms 新增了系統色彩模式 API，可讓應用程式跟隨 Windows 的淺色 / 深色設定自動切換。`AntdUI.Config` 已與之整合：

- 第一次存取 `AntdUI.Config` 時，其**靜態建構函式**會讀取 `Application.ColorMode` 決定初始明暗：`Classic` → 淺色、`Dark` → 深色、`System` → 依系統目前的值（`Application.SystemColorMode`）。
- 之後設定 `Config.Mode` / `Config.IsDark` 時，會同步呼叫 `Application.SetColorMode`，讓系統元件與 AntdUI 一起切換。

### 使用方法

**如果要跟隨系統自動切換，務必在存取 `AntdUI.Config`（或任何 AntdUI 元件）之前**呼叫 `Application.SetColorMode(SystemColorMode.System)`：

```csharp
using System.Windows.Forms;

internal static class Program
{
	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize();

		// 🔴 必須在第一次存取 AntdUI.Config 之前呼叫
		Application.SetColorMode(SystemColorMode.System);

		Application.Run(new MainForm());
	}
}
```

### 為什麼必須在 Config 之前

`Config` 的靜態建構函式只會在**第一次存取**時執行一次，並在那一刻讀取 `Application.ColorMode` 作為初始值。若先存取了 `Config`（例如某個元件在初始化時讀取了 `Config.Dpi`），靜態建構函式已依預設值完成初始化，之後再呼叫 `SetColorMode(SystemColorMode.System)` 就不會再影響 AntdUI 的初始主題。

### 手動指定明暗

如果不需要跟隨系統，直接設定 `Config.Mode` 即可（會同步到系統）：

```csharp
AntdUI.Config.Mode = AntdUI.TMode.Dark;  // 同步呼叫 Application.SetColorMode(Dark)
AntdUI.Config.IsDark = true;             // 等效寫法
```

---

## 主題模式管理

### 1. 啟用深色模式

> 啟用深色模式後，視窗背景與字型顏色需要自行設定

``` csharp
// 啟用深色模式
AntdUI.Config.IsDark = true;
```

### 2. 主題切換設定

#### 全域主題設定

> 在 `Program.cs` 中全域設定主題切換

``` csharp
// 設定全域主題切換
AntdUI.Config.Theme()
	.Dark("#000", "#fff") // 深色模式背景色和文字色
	.Light("#fff", "#000"); // 淺色模式背景色和文字色
```

#### 視窗主題設定

> 以 `AntdUI.BaseForm` 為基礎的視窗可以自訂主題切換設定

``` csharp
// 在視窗中設定主題切換
public partial class Form1 : AntdUI.BaseForm
{
	public Form1()
	{
		InitializeComponent();
		
		// 自訂視窗主題切換設定
Theme()
	.Dark("#1e1e1e", "#ffffff") // 深色模式背景色和文字色
	.Light("#ffffff", "#000000") // 淺色模式背景色和文字色
	.Header(header1, Color.FromArgb(240, 242, 245), Color.FromArgb(18, 18, 18)) // 頁首顏色
	.Button(btnTheme) // 主題切換按鈕
	.CallBefore(dark => Console.WriteLine("主題切換前：" + dark)) // 主題切換前回呼
	.Call(dark => Console.WriteLine("主題切換後：" + dark)) // 主題切換後回呼
	.FormBorderColor(Color.LightGray, Color.DarkGray) // 自訂視窗邊框顏色
	// 或者使用主題色風格的視窗邊框
	// .FormBorderColorPrimary()
	// 或者使用預設邊框色
	// .FormBorderColor()
	}
	
	// 主題切換按鈕點擊事件
	private void btnTheme_Click(object sender, EventArgs e)
	{
		// 切換主題模式
		AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
	}
}
```

#### 主題切換設定方法

| 方法 | 說明 | 參數 |
| :-- | :-- | :-- |
| **Dark** | 設定深色模式顏色 | Color back, Color fore / string back, string fore |
| **Light** | 設定淺色模式顏色 | Color back, Color fore / string back, string fore |
| **Header** | 設定頁首顏色 | PageHeader header, Color light, Color dark / string light, string dark |
| **Button** | 設定主題切換按鈕 | Button button |
| **Call** | 設定主題切換回呼 | Action<bool> call |
| **CallBefore** | 設定主題切換前回呼 | Action<bool> call |
| **Light** | 設定淺色模式回呼 | Action call |
| **Dark** | 設定深色模式回呼 | Action call |
| **FormBorderColor** | 設定視窗邊框顏色 | Color light, Color dark |
| **FormBorderColorPrimary** | 設定視窗邊框顏色（主題色風格） | 無 |
| **FormBorderColor** | 設定視窗邊框顏色（預設邊框色） | 無 |
| **ClearFormBorderColor** | 清除視窗邊框顏色 | 無 |

---

## 色彩工具

### 存取色彩體系資料庫

``` csharp
// 取得目前主題的色彩體系
var primaryPalette = AntdUI.Style.Db.Primary; // 品牌主色彩體系
var successPalette = AntdUI.Style.Db.Success; // 成功色彩體系
var warningPalette = AntdUI.Style.Db.Warning; // 警告色彩體系
var errorPalette = AntdUI.Style.Db.Error;     // 錯誤色彩體系
var infoPalette = AntdUI.Style.Db.Info;       // 訊息色彩體系
```

### 顏色轉換方法

| 方法 | 說明 | 傳回值 | 參數 |
| :-- | :-- | :-- | :-- |
| **ToHSV** | 顏色轉 HSV | HSV | Color color `顏色` |
| **HSVToColor** | HSV 轉顏色 | Color | HSV hsv, float alpha = 1 `透明度` |
| **HSVToColor** | HSV 轉顏色 | Color | float hue `色相`, float saturation `飽和度`, float value `明度`, float alpha = 1 `透明度` |
||||
| **ToHSL** | 顏色轉 HSL | HSL | Color color `顏色` |
| **HSLToColor** | HSL 轉顏色 | Color | HSL hsl, float alpha = 1 `透明度` |
| **HSLToColor** | HSL 轉顏色 | Color | float hue `色相`, float saturation `飽和度`, float lightness `亮度`, float alpha = 1 `透明度` |
||||
| **ToColor** | 字串轉顏色（HEX/rgb/rgba/hsl） | Color | string str `顏色字串`，例如 `#1677FF`、`rgb(22, 119, 255)`、`hsl(0 100% 63%)` |
| **ToHex** | RGB 轉成 HEX | string | Color color |
||||
| **rgba** | 轉顏色 | Color | int r, int g, int b, float alpha = 1 `透明度` |
| **rgba** | 轉顏色 | Color | Color color, float alpha = 1 `透明度` |