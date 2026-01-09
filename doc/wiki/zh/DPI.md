[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

## DPI 适配指南

### 核心要求

> 要实现 DPI 适配，**必须继承 AntdUI 提供的窗口类**，如：
> - `AntdUI.BaseForm`
> - `AntdUI.Window`
> - `AntdUI.BorderlessForm`
> 
> 这些窗口类内部已处理 DPI 缩放逻辑，`AutoHandDpi = true` 会自动启用缩放功能

### 适配步骤

#### 1. 设计器缩放设置（2.2.10 以下版本必填，2.2.10 及以上版本可选）

> **对于 2.2.10 及以上版本**：已适配 `AutoScaleMode.Dpi` 和 `AutoScaleMode.Font`，**无需强制使用 100% 缩放设计**，可直接在当前 DPI 下设计界面
> 
> **对于 2.2.10 以下版本**：**必须使用 100% 缩放设计界面**，否则在其他 DPI 倍数下可能显示不全

##### 设置方法（2.2.10 以下版本或需要时）

#### .NET Core 系列 👏

[修复 Visual Studio 中 Windows 窗体设计器的 HDPI/缩放问题](https://learn.microsoft.com/zh-cn/visualstudio/designers/disable-dpi-awareness?view=vs-2022)

> 在 Visual Studio 中，将项目文件 `.csproj` 中的属性 `ForceDesignerDPIUnaware` 设置为 `true`

```xml
<PropertyGroup>
   <ForceDesignerDPIUnaware>true</ForceDesignerDPIUnaware>
</PropertyGroup>
```

**方法 2：命令行启动 VS**
```shell
devenv.exe /noScale
```

**方法 3：修改系统缩放**
将 Windows 桌面缩放修改至 `100%`

#### 2. 启用 DPI 感知

> **必须启用 DPI 感知**，否则在高 DPI 下会被系统强制拉伸导致模糊

##### .NET Core 系列

> [Application.SetHighDpiMode(HighDpiMode.SystemAware)](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)

```csharp
internal static class Program
{
    [STAThread]
    static void Main()
    {
        // 设置 DPI 感知模式，选择适合你的模式
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        
        Application.Run(new YourForm()); // 确保 YourForm 继承自 AntdUI 窗口类
    }
}
```

##### .NET Framework
通过清单文件启用高 DPI 支持，详见 [Windows 窗体中的高 DPI 支持](https://learn.microsoft.com/zh-cn/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)

### 支持的 DPI 感知模式

| 模式 | 描述 |
| :-- | :-- |
| `PerMonitor` | 当前显示器 DPI |
| `PerMonitorV2` | 当前显示器 DPI（增强版，推荐） |
| `DpiUnawareGdiScaled` | 完全不感知（GDI 优化） |
| `SystemAware` | 系统 DPI |
| `DpiUnaware` | 完全不感知 |

### 布局异常处理

> 如果启用 DPI 感知后布局仍有异常，可尝试以下解决方案：

#### 1. 使用推荐 DPI 适配

> 需要将每个 `.Designer.cs` 中的 `AutoScaleMode` 移除/恢复默认值，移除 `AutoScaleDimensions` 和 `AutoScaleFactor` 也不受影响

#### 2. 切换 DPI 模式

```csharp
// 在程序启动时设置
AntdUI.Config.DpiMode = DpiMode.Compatible;
```

### 其他优化建议

1. **字体模糊问题**
   ```csharp
   AntdUI.Config.TextRenderingHighQuality = true;
   ```

2. **字体垂直居中问题**
   ```csharp
   AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "宋体");
   ```