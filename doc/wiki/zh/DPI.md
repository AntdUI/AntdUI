[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

# DPI 适配指南

> 本文面向刚接触 WinForms 的开发者，从"为什么"讲起，再给出可分步照做的配置方案。不同 .NET 版本的做法不同，请按你的项目类型选择对应章节。

## 一、先搞懂：为什么需要 DPI 适配？

Windows 允许用户在「显示设置」里调整缩放比例（100%、125%、150%、200%……）。缩放后，1 个"逻辑像素"不再等于 1 个"物理像素"。如果程序不声明自己懂缩放，Windows 会把整张界面当成一张图片放大——**文字和图标就会发虚、模糊**；如果声明了却处理不当，控件又会错位、显示不全。

所以 DPI 适配要解决两件事：

| 问题 | 由谁负责 | 解决办法 |
| :-- | :-- | :-- |
| ① 告诉系统"我支持高 DPI，别把我当图片拉伸" | 你的程序（启动时声明） | 启用 DPI 感知（本文步骤二） |
| ② 界面元素按缩放比例正确排布、不变形 | AntdUI 窗口类 + 设计器设置 | 继承 AntdUI 窗口（步骤一）+ 设计器设置（步骤三） |

> 只有两步都做对，界面才会既清晰又不错位。

## 二、三步走总览

1. **继承 AntdUI 窗口类** —— 让 AntdUI 接管缩放逻辑
2. **启用 DPI 感知** —— 按 .NET Core / .NET Framework 选择对应做法
3. **处理设计器缩放** —— 按 AntdUI 版本决定是否需要 100% 设计

下面逐步说明。

## 步骤一：继承 AntdUI 窗口类

> 🔴 这是 AntdUI 适配 DPI 的**前提**，不满足则后续配置效果有限。

让窗体继承 AntdUI 提供的窗口类，而不是标准的 `System.Windows.Forms.Form`：

- `AntdUI.BaseForm`
- `AntdUI.Window`
- `AntdUI.BorderlessForm`

这些窗口类内部已处理 DPI 缩放逻辑，`AutoHandDpi = true`（默认开启）时会自动缩放窗体及子控件、并响应运行中切换显示器 DPI 的场景。

```csharp
// 修改窗体基类的继承来源
public partial class Form1 : AntdUI.BaseForm
{
	public Form1()
	{
		InitializeComponent();
	}
}
```

> 如需关闭自动缩放，可设置 `AutoHandDpi = false`，一般不建议。

## 步骤二：启用 DPI 感知（按框架选择）

> **为什么要做**：若程序不声明 DPI 感知，Windows 会将其视为"不感知 DPI"，在高 DPI 屏幕上直接位图拉伸，导致界面模糊。此步骤必须在**创建任何窗体之前**完成。

请根据你的项目目标框架选择：

### A. .NET Core / .NET 5 及以上（推荐）👏

包括 .NET 5/6/7/8/9/10。

#### 方式 1：项目属性（.NET 6+ 推荐，最简单）

在项目文件 `.csproj` 中设置 `ApplicationHighDpiMode`：

```xml
<PropertyGroup>
   <OutputType>WinExe</OutputType>
   <TargetFramework>net8.0-windows</TargetFramework>
   <UseWindowsForms>true</UseWindowsForms>
   <ApplicationHighDpiMode>PerMonitorV2</ApplicationHighDpiMode>
</PropertyGroup>
```

新版 WinForms 模板的 `Program.cs` 会自动生成 `ApplicationConfiguration.Initialize();`，它在编译期根据项目设置生成，内部已包含 DPI 感知配置：

```csharp
internal static class Program
{
	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize(); // 已包含 DPI 感知、字体等初始化
		Application.Run(new Form1());
	}
}
```

#### 方式 2：代码显式设置（.NET Core 3.x / 需要精确控制）👏

在 `Main` 中、**创建任何窗体之前**调用 `Application.SetHighDpiMode`：

```csharp
internal static class Program
{
	[STAThread]
	static void Main()
	{
		// 🔴 必须在 Application.Run / 创建窗体之前调用，且每个进程只能设置一次
		Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

		Application.Run(new Form1()); // Form1 需继承 AntdUI 窗口类
	}
}
```

> **为什么必须最早调用**：高 DPI 模式每个进程只能设置一次；一旦创建了窗体（句柄），就无法再更改。若项目同时用清单文件声明了 DPI 模式，该方法将无法覆盖清单中的设置。
>
> 官方文档：[Application.SetHighDpiMode](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)・[.NET 6 起的项目级设置](https://learn.microsoft.com/zh-cn/dotnet/desktop/winforms/whats-new/net60?view=netdesktop-8.0#project-level-application-settings)

### B. .NET Framework

.NET Framework 无法在代码里调用 `SetHighDpiMode`，需通过配置文件声明。要求 **.NET Framework 4.7+** 且运行在 **Windows 10 创意者更新（1703）及以上**。

#### 方式 1：app.config（4.7+ 推荐）

在 `App.config` 中加入以下配置，声明 `PerMonitorV2`：

```xml
<configuration>
   <startup>...</startup>

   <System.Windows.Forms.ApplicationConfigurationSection>
	  <add key="DpiAwareness" value="PerMonitorV2" />
   </System.Windows.Forms.ApplicationConfigurationSection>
</configuration>
```

同时建议在 `app.manifest` 中声明兼容 Windows 10（否则部分特性不生效）：

```xml
<compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
   <application>
	  <!-- Windows 10 -->
	  <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" />
   </application>
</compatibility>
```

入口 `Main` 保持常规写法即可：

```csharp
[STAThread]
static void Main()
{
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);
	Application.Run(new Form1());
}
```

#### 方式 2：app.manifest（旧项目兼容）

在 `app.manifest` 的 `<application>` 节点下加入：

```xml
<application xmlns="urn:schemas-microsoft-com:asm.v3">
   <windowsSettings>
	  <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true/pm</dpiAware>
	  <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">permonitorv2</dpiAwareness>
   </windowsSettings>
</application>
```

> **为什么推荐 app.config 而非 manifest**：manifest 中的 DPI 设置会**覆盖** app.config 中的声明，导致 app.config 失效。现代项目优先用 app.config 或项目属性，便于统一管理。

官方参考：[Windows 窗体中的高 DPI 支持](https://learn.microsoft.com/zh-cn/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)

## 步骤三：设计器缩放设置

> **为什么要做**：Visual Studio 设计器本身在不同缩放的机器上打开窗体，若未统一基准，控件的坐标和尺寸会按设计时的 DPI 被写入 `.Designer.cs`，到其他缩放的机器上就可能对不齐或显示不全。

### AntdUI 2.2.10 及以上（推荐升级）

已适配 `AutoScaleMode.Dpi` 和 `AutoScaleMode.Font`，**无需强制 100% 缩放设计**，可直接在当前 DPI 下设计界面。

### AntdUI 2.2.10 以下

**必须使用 100% 缩放设计界面**，否则在其他 DPI 倍数下可能显示不全。让设计器"以为"自己在 100% 缩放下工作：

#### .NET Core / .NET 5+

在项目文件 `.csproj` 中设置 `ForceDesignerDPIUnaware`：

```xml
<PropertyGroup>
   <ForceDesignerDPIUnaware>true</ForceDesignerDPIUnaware>
</PropertyGroup>
```

或两种方式任选其一：

- **命令行启动 VS**：`devenv.exe /noScale`
- **修改系统缩放**：把 Windows 桌面缩放临时改为 `100%`

官方参考：[修复 Visual Studio 中 Windows 窗体设计器的 HDPI/缩放问题](https://learn.microsoft.com/zh-cn/visualstudio/designers/disable-dpi-awareness?view=vs-2022)

#### .NET Framework

将 Windows 桌面缩放临时改为 `100%` 后再用设计器编辑窗体；或升级到 AntdUI 2.2.10+ 以避免该限制。

## DPI 感知模式速查

| 模式 | 描述 | 说明 |
| :-- | :-- | :-- |
| `PerMonitorV2` | 当前显示器 DPI（增强版） | ✅ 推荐，支持跨屏、动态 DPI 变化 |
| `PerMonitor` | 当前显示器 DPI | 支持多屏不同缩放 |
| `SystemAware` | 系统 DPI | 全屏统一，启动时决定 |
| `DpiUnawareGdiScaled` | 不感知（GDI 优化缩放） | 由 GDI 缩放，文字相对清晰 |
| `DpiUnaware` | 完全不感知 | ❌ 会位图拉伸导致模糊 |

## AntdUI 的 DpiMode（布局兼容开关）

除了系统层面的 DPI 感知，AntdUI 自身还提供 `Config.DpiMode` 控制内部缩放策略：

| 值 | 描述 |
| :-- | :-- |
| `DpiMode.Default` | 默认模式，配合继承的窗口类自动处理缩放 |
| `DpiMode.Compatible` | 兼容模式，使用整体 `Scale` 缩放，用于布局异常时的兜底 |

```csharp
// 在程序启动时设置（一般无需修改）
AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;
```

> 获取 / 自定义 DPI：
> ```csharp
> float dpi = AntdUI.Config.Dpi;       // 1=100%、1.25=125%，以此类推
> AntdUI.Config.SetDpi(1.5F);          // 自定义 DPI
> AntdUI.Config.SetDpi(null);          // 恢复自动获取
> ```

## 常见问题排查

### 1. 字体模糊

```csharp
AntdUI.Config.TextRenderingHighQuality = true;
```

也可调整文本呈现质量：

```csharp
AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
```

### 2. 字体垂直居中问题

部分字体（如宋体）在高 DPI 下渲染偏移，可指定需要修正的字体列表：

```csharp
AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "宋体");
```

![CorrectionTextRendering](Img/CorrectionTextRendering.jpg)

### 3. 布局异常 / 控件错位

按顺序尝试：

1. 确认窗体已继承 AntdUI 窗口类（步骤一）；
2. 确认已正确启用 DPI 感知（步骤二）；
3. 将各 `.Designer.cs` 中的 `AutoScaleMode` 移除或恢复默认值（移除 `AutoScaleDimensions`、`AutoScaleFactor` 也不受影响）；
4. 切换 AntdUI 兼容模式：`AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;`

## 官方参考

- [Windows 窗体中的高 DPI 支持](https://learn.microsoft.com/zh-cn/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)
- [Application.SetHighDpiMode](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)
- [修复 VS 设计器 HDPI/缩放问题](https://learn.microsoft.com/zh-cn/visualstudio/designers/disable-dpi-awareness?view=vs-2022)