[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

> `AntdUI.Config` 用于全局配置，建议在 `Program.cs` 的 `Main` 方法中、创建窗体之前完成设置。

### 色彩模式

> 默认浅色模式

#### 设置获取色彩模式

``` csharp
AntdUI.Config.Mode = AntdUI.TMode.Light;
```

> 🔴 .NET 10 及以上：设置 `Mode` 会同步调用 `Application.SetColorMode`，让系统控件与 AntdUI 一起切换明暗。若要跟随系统，请在**访问 `AntdUI.Config` 之前**调用 `Application.SetColorMode(SystemColorMode.System);`，因为 `Config` 的静态构造函数会在首次访问时读取系统颜色模式作为初值。详见 [主题](Theme.md#net-10-跟随系统颜色模式)。

#### 是否浅色模式

``` csharp
bool islight = AntdUI.Config.IsLight;
AntdUI.Config.IsLight = true;// 设置为浅色模式
```

#### 是否深色模式

``` csharp
bool isdark = AntdUI.Config.IsDark;
AntdUI.Config.IsDark = true;// 设置为深色模式
```

#### 配置全局切换主题 🔴

> 配置后，继承 [BaseForm](Form/BaseForm.md) 的窗体会自动切换主题

``` csharp
AntdUI.Config.Theme()
.Light("#fff", "#000") // 浅色模式背景 白色(#fff) | 前景 黑色(#000)
.Dark("#000", "#fff") // 深色模式背景 黑色(#000) | 前景 白色(#fff)
.Header("#f3f3f3", "#111111"); // PageHeader 头部背景 浅色背景色 浅灰色(#f3f3f3) | 深色背景色 深灰色(#111111)
```

> 更多主题能力（品牌色、色彩算法、跟随系统等）见 [主题](Theme.md)。

#### 主题配置对象

> `Theme()` 是便捷入口（为空时自动创建）；`ThemeConfig` 是可读写的底层属性，可直接赋值自定义配置对象

``` csharp
AntdUI.IThemeConfig? config = AntdUI.Config.ThemeConfig; // 获取（可能为 null）
AntdUI.Config.ThemeConfig = config;                      // 直接设置
```

#### 清空主题配置

``` csharp
AntdUI.Config.ThemeClear();
```

### 动画

> 默认开启动画

``` csharp
AntdUI.Config.Animation = false;
```

#### 按控件开关动画

> 可对指定控件类型（控件名称）单独禁用动画，`params` 支持传入多个名称

``` csharp
AntdUI.Config.DisableAnimation("Button", "Input"); // 禁用 Button、Input 的动画
AntdUI.Config.EnableAnimation("Button"); // 恢复 Button 的动画（移除禁用项）
AntdUI.Config.ClearAnimationData(); // 清除所有禁用设置，全部恢复
```

#### 查询动画是否启用

``` csharp
bool enabled = AntdUI.Config.HasAnimation("Button"); // 按控件名查询
bool enabled2 = AntdUI.Config.HasAnimation("Button", "btnOK"); // 同时按控件名 / 控件 Name 查询
```

### 表格单元格焦点样式

> 全局默认 `TableCellFocusedStyle.None`（无焦点），可设为虚线框或实线填充

``` csharp
AntdUI.Config.DefaultCellFocusedStyle = AntdUI.TableCellFocusedStyle.Solid;
```

### 触屏

#### 触屏使能

> 默认启用触屏使能

``` csharp
AntdUI.Config.TouchEnabled = true;
```

#### 触屏阈值

> 默认 `20`（像素），触发触摸判定的位移阈值

``` csharp
AntdUI.Config.TouchThreshold = 20;
```

#### 触屏点击使能

``` csharp
AntdUI.Config.TouchClickEnabled = true;
```

#### 屏幕键盘模式

> 默认 `Accessibility`（无障碍），可选 `Disabled`（禁用）、`OpenOsk`（调用系统屏幕键盘 osk）

``` csharp
AntdUI.Config.TouchKeyboard = AntdUI.TouchKeyboardMode.Accessibility;
```

#### 惯性滚动参数

> 用于调节列表、滚动区域的触摸惯性滚动手感

``` csharp
AntdUI.Config.TouchVelocityMin = .1F;   // 触发惯性滚动的最小速度（像素/毫秒）
AntdUI.Config.TouchVelocityEnd = .01F;  // 惯性滚动停止的最小速度（像素/毫秒）
AntdUI.Config.TouchInterval = 20;       // 惯性滚动帧间隔（毫秒）
AntdUI.Config.TouchDecay = .94F;        // 惯性滚动每帧衰减系数
```

### 鼠标悬停时长

> 鼠标悬停触发的延时，默认 `100` 毫秒

``` csharp
AntdUI.Config.MouseHoverDelay = 100;
```

### 阴影

#### 阴影使能

> 默认启用阴影

``` csharp
AntdUI.Config.ShadowEnabled = false;
```

#### 阴影兼容模式（支持多色）

``` csharp
AntdUI.Config.ShadowModeCompatible = true;
```

#### 阴影大小与透明度

``` csharp
AntdUI.Config.ShadowSize = 10;       // 阴影大小，默认 10 像素
AntdUI.Config.ShadowOpacity = 0.2F;  // 阴影透明度，默认 0.2
```

### 焦点边框

> 默认启用焦点边框

``` csharp
AntdUI.Config.FocusBorderEnabled = false;
```

### 下拉边距

> 下拉框距离更远（避免遮挡）

``` csharp
AntdUI.Config.DropDownMarginFurther = true;
```

### Emoji

#### Emoji使能

> 默认启用Emoji

``` csharp
AntdUI.Config.EmojiEnabled = true;
```

#### Emoji字体

> 默认 `Segoe UI Emoji`

``` csharp
AntdUI.Config.EmojiFont = "Segoe UI Emoji";
```

### 使用钩子

> Release 默认启用（DEBUG 下默认关闭），用于全局消息钩子

``` csharp
AntdUI.Config.UseHook = true;
```

### 滚动条

#### 滚动条隐藏样式

> 默认一直显示 `false`

``` csharp
AntdUI.Config.ScrollBarHide = false;
```

#### 滚动条最小大小

> `ScrollMinSizeY` 默认 `30`，`ScrollMinSizeX` 默认 `null`

``` csharp
AntdUI.Config.ScrollMinSizeY = 30;
AntdUI.Config.ScrollMinSizeX = 30;
```

### 窗口内弹出 Message/Notification

> 默认屏幕弹出

``` csharp
AntdUI.Config.ShowInWindow = true;
```

<details>
<summary>针对配置</summary>

> 弹出是否在窗口里而不是在系统里（Message）
``` csharp
AntdUI.Config.ShowInWindowByMessage = true;
```

> 弹出是否在窗口里而不是在系统里（Notification）
``` csharp
AntdUI.Config.ShowInWindowByNotification = true;
```

</details>

### 通知消息边界偏移量XY（Message/Notification）

> 默认 0

``` csharp
AntdUI.Config.NoticeWindowOffsetXY = 0;
```

### 通知超出显示后立即关闭旧的（Message/Notification）

> 默认超出后等待有旧的关闭才显示

``` csharp
AntdUI.Config.NoticeOverflowClose = false;
```

### 文本呈现质量

``` csharp
AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
```

### 文本高质量呈现

``` csharp
AntdUI.Config.TextRenderingHighQuality = true;
```

### 默认字体 🔴

> 该属性已标记过时（Obsolete），如需全局字体请使用项目级默认字体设置

``` csharp
AntdUI.Config.Font = new Font("微软雅黑", 10);
```

### 获取DPI

> 1=100%、1.25=125%，以此类推

``` csharp
float dpi = AntdUI.Config.Dpi;
```

### DPI 模式

> 默认 `DpiMode.Default`，异常布局时可切换为 `DpiMode.Compatible` 兼容模式。详见 [DPI](DPI.md)

``` csharp
AntdUI.Config.DpiMode = AntdUI.DpiMode.Compatible;
```

### 自定义DPI

> 传入 `null` 可恢复自动获取

``` csharp
AntdUI.Config.SetDpi(1.5F);
AntdUI.Config.SetDpi(null); // 恢复自动
```

### 设置修正文本渲染

``` csharp
AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "宋体"); //需要修正的字体列表
```

![CorrectionTextRendering](Img/CorrectionTextRendering.jpg)

### 空白图

> 加载中 / 空状态使用的占位图，可设置图片与宽高比，或用 SVG 分别指定浅色 / 深色

``` csharp
AntdUI.Config.EmptyImage = myImage;      // 自定义空白图
AntdUI.Config.EmptyImageRatio = 2.98F;   // 宽高比，默认 2.98

AntdUI.Config.SetEmptyImageSvg(lightSvg, darkSvg); // 设置浅色 / 深色 SVG
AntdUI.Config.ClearEmptyImageSvg();                // 清除自定义 SVG
```

### 空文本占位符

> 常量 `Config.NullText`，内部用于区分真正的空值（一般无需修改）

``` csharp
string nullText = AntdUI.Config.NullText;
```