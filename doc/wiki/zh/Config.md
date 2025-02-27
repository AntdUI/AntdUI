[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)・[SVG](SVG.md)

### 色彩模式

> 默认浅色模式

#### 设置获取色彩模式

``` csharp
AntdUI.Config.Mode = AntdUI.TMode.Light;
```

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

### 关闭动画

> 默认开启动画

``` csharp
AntdUI.Config.Animation = false;
```

### 触屏使能 🔴

> 默认启用触屏使能

``` csharp
AntdUI.Config.TouchEnabled = true;
```

### 阴影使能 🔴

> 默认启用阴影

``` csharp
AntdUI.Config.ShadowEnabled = false;
```

### 滚动条隐藏样式 🔴

> 默认一直显示 `false`

``` csharp
AntdUI.Config.ScrollBarHide = false;
```

### 滚动条最小大小Y 🔴

> 默认 `30`

``` csharp
AntdUI.Config.ScrollMinSizeY = 30;
```

### 窗口内弹出 Message/Notification

> 默认屏幕弹出

``` csharp
AntdUI.Config.ShowInWindow = true;
```

<details>
<summary>针对配置 🔴</summary>

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

### 文本呈现质量

``` csharp
AntdUI.Config.TextRenderingHint = System.Drawing.Text.ClearTypeGridFit;
```

### 文本高质量呈现 🔴

``` csharp
AntdUI.Config.TextRenderingHighQuality = true;
```

### 默认字体

``` csharp
AntdUI.Config.Font = new Font("微软雅黑", 10);
```

### 获取DPI

> 1=100%、1.25=125%，以此类推

``` csharp
float dpi = AntdUI.Config.Dpi;
```

### 自定义DPI

``` csharp
AntdUI.Config.SetDpi(1.5F);
```

### 设置修正文本渲染

``` csharp
AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI", "宋体"); //需要修正的字体列表
```

![CorrectionTextRendering](Img/CorrectionTextRendering.jpg)