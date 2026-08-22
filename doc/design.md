# AntdUI 设计文档

> 本文档基于对 AntdUI 源码的深度阅读与归纳，旨在说明该 WinForm UI 库的整体架构、核心机制，以及从中提炼出的 WinForm 自绘 UI 开发心得。

---

## 1. 项目定位

**AntdUI** 是一套将 [Ant Design](https://ant-design.antgroup.com/) 设计语言落地到 Windows 桌面的 WinForm 控件库。

| 维度 | 说明 |
|------|------|
| 渲染技术 | 纯 GDI 矢量绘图，不依赖位图皮肤资源 |
| 目标框架 | .NET Framework 4.6+ / .NET 6/8/9 Windows，支持 AOT |
| 设计参照 | Ant Design 5.x 色彩体系与交互规范 |
| 差异化能力 | 高质量抗锯齿文字、Emoji 渲染、可打断动效、分层窗口阴影、DPI 感知、SVG 图标 |

与典型 WinForm 皮肤库（贴图 + 系统控件包装）不同，AntdUI 选择**完全接管绘制与交互**，从底层重建控件行为，以获得跨 DPI、跨主题的一致视觉体验。

---

## 2. 整体架构

```
AntdUI/
├── src/AntdUI/
│   ├── Controls/          # 业务控件（Button、Table、Input…）
│   │   ├── Core/          # Canvas 抽象层、GDI 实现
│   │   ├── Input/         # 输入类控件（按职责拆分的 partial）
│   │   ├── Table/         # 表格（布局/渲染/事件/数据分离）
│   │   ├── Tabs/          # 标签页样式策略
│   │   ├── Scroll/        # 自绘滚动条
│   │   └── Layout/        # 流式/堆栈/虚拟面板
│   ├── Forms/             # BaseForm → Window / BorderlessForm
│   │   └── LayeredWindow/ # 分层弹出窗（下拉、Modal、Message…）
│   ├── Style/             # 主题色卡、IColor 数据库
│   ├── Enum/              # 全局枚举（TType、TAMode、AnimationType…）
│   ├── Events/            # 自定义 EventArgs / 委托定义
│   ├── Localization/      # 多语言
│   └── Lib/               # 工具：Animation、ITask、EventHub、Win32、SVG
├── example/Demo/          # 全控件演示
└── doc/wiki/              # 用户向 API 文档
```

### 2.1 分层职责

```mermaid
flowchart TB
    subgraph App["应用层"]
        Demo["example/Demo"]
        UserApp["用户 WinForm 应用"]
    end

    subgraph Controls["控件层"]
        IC["IControl 基类"]
        Complex["Table / Input / Menu 等复杂控件"]
    end

    subgraph Render["渲染层"]
        Canvas["Canvas 接口"]
        GDI["CanvasGDI 实现"]
        Helper["Helper.GDI 扩展（阴影/徽标/路径）"]
    end

    subgraph Infra["基础设施层"]
        Style["Style 主题"]
        Config["Config 全局配置"]
        EventHub["EventHub 事件总线"]
        ITask["ITask 动画调度"]
        Win32["Win32 / Vanara P/Invoke"]
    end

    subgraph Window["窗口层"]
        BaseForm["BaseForm"]
        Window["Window（无边框+DWM）"]
        Layered["ILayeredForm（分层透明窗）"]
    end

    UserApp --> IC
    Demo --> IC
    IC --> Canvas
    Canvas --> GDI
    GDI --> Helper
    IC --> Style
    IC --> Config
    IC --> ITask
    Window --> EventHub
    Layered --> Win32
    Complex --> IC
```

---

## 3. 核心基类：IControl

所有可视化控件的根是 `AntdUI.IControl`（继承 `System.Windows.Forms.Control`）。它是整个自绘体系的契约中心。

### 3.1 ControlStyles 策略

构造函数根据 `ControlType` 设置不同的 `ControlStyles`：

| ControlType | 特点 |
|-------------|------|
| `Default` | 不可选中（`Selectable = false`），适合纯展示控件 |
| `Select` | 可选中，适合输入/选择类 |
| `Button` | 可选中 + 禁用 `StandardClick`，自行处理点击逻辑 |

共同启用的样式：

- `UserPaint` + `AllPaintingInWmPaint`：完全自绘，不走系统主题绘制
- `OptimizedDoubleBuffer` + `DoubleBuffer`：双缓冲，消除闪烁
- `SupportsTransparentBackColor`：支持透明背景
- `ResizeRedraw`：尺寸变化时自动重绘

**心得**：现代 WinForm 自绘控件应默认开启上述组合；放弃系统绘制是获得一致跨版本视觉的前提。

### 3.2 绘制管线

```
OnPaint
  └─ Graphics.High() → CanvasGDI
       ├─ OnDrawBg(DrawEventArgs)   // 背景层，可订阅 DrawBg 事件
       ├─ OnDraw(DrawEventArgs)     // 内容层，子类重写 + Draw 事件
       └─ PaintBadge(g)             // 徽标叠加层（BadgeConfig）
```

关键 API：

- `protected virtual void OnDraw(DrawEventArgs e)` — 子类绘制入口
- `public event DrawEventHandler? Draw` — 外部可挂接绘制扩展
- `public Bitmap? DrawBitmap()` — 离屏渲染，用于截图/导出

**心得**：将绘制分为 **Bg / Content / Overlay** 三层，比单一 `OnPaint` 更易扩展主题、动画叠加和徽标等横切关注点。

### 3.3 RenderRegion（绘制区域裁剪）

非矩形控件（圆角、圆形）需重写 `RenderRegion` 返回 `GraphicsPath`。`Spin` 等覆盖层会据此裁剪，避免矩形遮罩破坏圆角视觉。

```csharp
protected override GraphicsPath RenderRegion =>
    ClientRectangle.RoundPath(radius * Config.Dpi);
```

### 3.4 线程安全属性访问

`Visible`、`Enabled` 等属性在 setter 中检测 `InvokeRequired`，确保跨线程 UI 更新安全。动画任务 `ITask` 也通过 `CancellationToken` 与控件生命周期绑定。

---

## 4. 渲染系统：Canvas

### 4.1 抽象与实现

| 类型 | 职责 |
|------|------|
| `Canvas`（interface） | 统一绘制 API：文字、图片、路径填充、阴影、SVG |
| `CanvasGDI` | 基于 `Graphics` 的实现，通过 `Graphics.High()` 获取 |
| `Helper.GDI` | 扩展方法：圆角路径、阴影、徽标、颜色混合、DPI 工具 |

### 4.2 高质量文字

`CanvasGDI.String()` 在 `Config.TextRenderingHighQuality = true` 时，通过 `GraphicsPath.AddString` + 填充路径实现抗锯齿文字；否则回退 `DrawString`。

同时集成 `CorrectionTextRendering` 修正特定字体的度量偏差，并用占位符 `"龍Qq"`（`Config.NullText`）辅助测量含 Emoji 的文本高度。

**心得**：GDI 原生 `DrawString` 在高分屏和中文/Emoji 混排时问题多；路径化文字或 DirectWrite 封装是专业 UI 库的必经之路。AntdUI 选择路径化方案，兼顾 .NET Framework 4.x 兼容性。

### 4.3 矢量与 SVG

库内嵌完整 SVG 解析/渲染栈（`Lib/SVG/`），控件通过 `BadgeSvg`、图标属性等直接消费 SVG 字符串，无需外部图片资源。

### 4.4 阴影系统

`Helper.GDI.PaintShadow()` 基于 `GraphicsPath` 多层偏移填充实现柔和阴影，是 AntdUI 视觉质感的核心之一。阴影参数通过 `ShadowConfig` 接口在控件间复用。

### 4.5 资源生命周期

规范要求所有 `Brush`、`Pen`、`GraphicsPath`、`Bitmap` 使用 `using` 释放。`Canvas` 实现 `IDisposable`，绘制状态通过 `g.Save()` / `g.Restore()` 保护（如 Table 单元格绘制）。

---

## 5. 主题与样式系统

### 5.1 色彩架构

```
Config.Mode (TMode.Light/Dark)     ← 全局明暗
    ↓ EventHub.Dispatch(THEME)
Style.Db (Theme.IColor)            ← 运行时色卡数据库
Style.Get(Colour, control, mode)   ← 按控件名取色
控件 ColorScheme (TAMode)          ← 控件级覆盖（Auto/Light/Dark）
```

- `Style.Set(Colour.Primary, color, "Button")` 支持全局/按控件覆盖
- `Style.LoadCustom(Dictionary)` 支持配置文件批量加载
- 内置 Ant Design 色彩算法（Primary/Success/Warning/Error 及 Hover/Active/Bg 变体）

### 5.2 EventHub 事件总线

```csharp
EventHub.Dispatch(EventType.THEME);  // 主题切换
EventHub.Dispatch(EventType.DPI);    // DPI 变化
EventHub.Dispatch(EventType.LANG);    // 语言切换
```

监听者通过 `IEventListener` 或 `BaseForm.Theme()` 注册，`WeakReference` 避免内存泄漏。`Table` 等复杂控件实现 `IEventListener` 在主题/DPI 变化时批量 `Invalidate()`。

**心得**：WinForm 缺乏 WPF 的 ResourceDictionary 动态资源；用**静态色卡 + 事件总线广播**是轻量且有效的替代方案。

### 5.3 DPI 适配

- `Config.Dpi`：当前 DPI 缩放比（默认从 `Graphics.DpiX / 96` 获取）
- 所有尺寸常量（圆角、滚动条宽度、边框）乘以 `Config.Dpi`
- `BaseForm.AutoHandDpi` + `Window` 自动处理 DPI 变更
- 设计器要求 100% 缩放，`AutoScaleMode` 应移除

---

## 6. 动画系统

### 6.1 设计目标

AntdUI 强调**可打断的舒适动效**——用户快速操作时动画不阻塞、可立即响应新手势。

### 6.2 核心组件

| 组件 | 作用 |
|------|------|
| `Animation` | 缓动函数：Liner / Ease / Ball / Resilience |
| `ITask` | 基于 `Task` + `CancellationTokenSource` 的帧循环调度器 |
| `Config.Animation` | 全局动画开关 |
| `Config.HasAnimation(control)` | 按控件名细粒度禁用 |

### 6.3 典型模式

以 `Button` 为例：

1. **悬停动画**：`AnimationHoverValue` 递增/递减，每 10ms `Invalidate()` 一帧
2. **点击波纹**：`AnimationClickValue` 从 1→0，绘制扩散圆形遮罩
3. **颜色过渡**：`AnimationBlinkTransition` 用 `BlendColors` 插值
4. **打断**：新手势触发时 `ThreadHover?.Dispose()` 取消旧 `ITask`

```csharp
// ITask 构造：控件 + 帧回调 + 间隔 + 结束回调
ThreadHover = new ITask(this, () => {
    AnimationHoverValue += addvalue;
    if (AnimationHoverValue > alpha) return false; // 结束
    Invalidate();
    return true; // 继续下一帧
}, 10, () => { AnimationHover = false; Invalidate(); });
```

**心得**：

- 不要用 `Timer` 直接驱动复杂动画；用可取消的帧任务 + `Invalidate(脏矩形)` 更可控
- 动画状态应是**绘制参数**（float alpha、offset），而非中间控件
- 全局开关 + 按控件禁用，兼顾性能敏感场景

---

## 7. 窗口体系

### 7.1 继承链

```
Form
 └─ BaseForm          # 双缓冲、主题、DWM 主题色
      └─ Window       # 无边框、自定义缩放、IMessageFilter
           └─ BorderlessForm  # 纯无边框变体
```

`Window` 通过 `CreateParams`、DWM API、消息过滤实现：

- 自定义标题栏区域（配合 `PageHeader` / `WindowBar`）
- 边缘拖拽缩放（`Resizable` + `ReadMessage`）
- 最大化/还原时 DWM 区域刷新

### 7.2 分层窗口 ILayeredForm

弹出类 UI（Dropdown、DatePicker、Message、Modal、Tooltip、Drawer）继承 `ILayeredForm`：

| 特性 | 实现 |
|------|------|
| 透明/半透明 | `WS_EX_LAYERED` + `UpdateLayeredWindow`（`Win32.SetBits`） |
| 无任务栏 | `ShowInTaskbar = false` |
| 无焦点弹出 | `ShowWithoutActivation`、扩展样式 `0x08000000` |
| 离屏渲染 | `PrintBit()` → Bitmap → 更新分层窗口 |
| 缓存优化 | `RenderCache` 避免重复分配 Bitmap |
| 消息过滤 | `IMessageFilter` 处理外部点击关闭 |

渲染循环：`内容变化 → Print() → Printmap() → Win32.SetBits(memDc, bmp, rect, handle, alpha)`

**心得**：WinForm 原生 `Popup` 控件无法做阴影、圆角、动画；**分层窗口 + 离屏位图**是下拉/Toast/Modal 的行业标准解法。注意 `memDc` 和 `hBitmap` 在 `Dispose` 时必须释放。

### 7.3 静态弹出 API 模式

`Message`、`Notification`、`Modal`、`Drawer` 等采用 **静态类 + Config 配置对象 + 队列** 模式：

```csharp
Message.success(form, "操作成功");
Modal.open(new Modal.Config(form) { Title = "确认", Content = "..." });
```

弹出窗体由库内部管理生命周期，用户无需手动 `new Form()`。`Config` 对象承载所有可配置项（位置、动画、自动关闭、回调）。

---

## 8. 事件定制体系

AntdUI 在 WinForm 标准事件之上，构建了三层事件扩展机制。

### 8.1 绘制事件（Draw / DrawBg）

任何 `IControl` 子类可订阅 `Draw` / `DrawBg`，在不继承的情况下注入自定义绘制：

```csharp
myPanel.Draw += (s, e) => e.Canvas.DrawText("水印", font, color, e.Rect);
```

### 8.2 语义化自定义 EventArgs

`Events/` 目录按控件拆分，定义携带业务上下文的参数类型，避免用户从坐标反推行列：

| 事件类型 | 典型参数 |
|----------|----------|
| `TableClickEventArgs` | Record, RowIndex, ColumnIndex, Column, Rect |
| `TablePaintBeginEventArgs` | Canvas, 单元格矩形, Record, Handled 标志 |
| `TableHoverEventArgs` | 悬停单元格完整上下文 |
| `IntEventArgs` / `ObjectNEventArgs` | 泛型值包装 |

### 8.3 绘制拦截（Handled 模式）

`Table.CellPaintBegin` 是最典型的**绘制拦截**：

```csharp
void PaintItem(Canvas g, ...) {
    if (CellPaintBegin == null) PaintItemCore(...);
    else {
        var args = new TablePaintBeginEventArgs(...);
        CellPaintBegin(this, args);
        if (args.Handled) {
            // 用户完全接管绘制
        } else {
            // 用户可覆盖 CellBack / CellFont / CellFore 后走默认逻辑
            PaintItemCore(..., args.CellFont ?? Font, args.CellFore ?? fore);
        }
    }
    CellPaint?.Invoke(...); // 绘制后钩子
}
```

**心得**：复杂控件应提供 **Before（可拦截）→ Core（默认）→ After（装饰）** 三段式绘制扩展，比单纯 `OwnerDraw` 更灵活。

### 8.4 消息级定制

| 机制 | 场景 |
|------|------|
| `WndProc` 重写 | 触屏指针消息转鼠标消息（`WM_POINTERDOWN` → `WM_LBUTTONDOWN`） |
| `IMessageFilter` | 全局鼠标消息拦截（弹出层点击外部关闭、窗口边缘缩放） |
| `Application.AddMessageFilter` | `ILayeredForm`、`Window` 按需注册/注销 |
| 禁用 `StandardClick` | 按钮类控件自行区分点击/双击/长按 |

### 8.5 延迟与防抖事件

`IControl` 内置鼠标悬停延迟（`Config.MouseHoverDelay` + `Stopwatch` + `ITask`），子类重写 `OnMouseHover(x, y)` 实现 Tooltip 预览等。触屏滚动有惯性缓冲动画。

### 8.6 数据绑定辅助

`OnPropertyChanged([CallerMemberName])` 遍历 `DataBindings` 自动 `WriteValue()`，使自定义属性 setter 与 WinForm 数据绑定兼容。

---

## 9. 复杂控件架构模式

### 9.1 Partial Class 职责拆分

以 `Table` 为例，单控件拆为 15+ 文件：

| 文件 | 职责 |
|------|------|
| `Table.cs` | 属性、数据入口 |
| `Table.Data.cs` | 数据源解析 |
| `Table.Layout.cs` | 行列布局计算 |
| `Table.Render.cs` | 绘制主逻辑 |
| `Table.Mouse.cs` | 鼠标命中测试 |
| `Table.Keyboard.cs` | 键盘导航 |
| `Table.Event.cs` | 事件定义与触发 |
| `Table.Filter.cs` | 筛选 |
| `Table.Template.cs` | 行模板/样式行 |
| `Cell/*.cs` | 各单元格类型的 GetSize/SetRect/Paint |

**心得**：超过 2000 行的 WinForm 控件必须按**数据/布局/渲染/输入/事件**五维拆分，否则无法维护。

### 9.2 单元格策略模式（ICell）

```
ICell
 ├─ GetSize()    # 测量
 ├─ SetRect()    # 布局
 ├─ PaintBack()  # 背景
 └─ Paint()      # 内容
```

具体类型：`CellText`、`CellButton`、`CellCheckbox`、`CellImage`、`CellTag`、`CellLink`…

列定义通过 `Column.Render` 指定单元格类型，实现**同一表格混合多种编辑器/展示器**。

### 9.3 自绘滚动条

`ScrollBar` 不是 `Control`，而是附加在 `IControl` / `FlowPanel` / `ILayeredForm` 上的逻辑组件：

- 维护 `ScrollY`/`ScrollX` 偏移量
- 命中测试与拖拽
- 通过 `Invalidate(rect)` 局部重绘
- 尺寸随 `Config.Dpi` 缩放

**心得**：系统 `VScrollBar` 无法自定义外观；自绘滚动条是列表/表格类控件的标配，且应与布局引擎解耦。

### 9.4 虚拟化面板 VirtualPanel

大数据列表通过虚拟化只渲染可见区域，结合 `ScrollBar` 和 `ITask` 滚动动画，避免创建大量子控件。

### 9.5 Input 复合控件

`Input` 拆分为 `Input.cs`、`Input.Render.cs`、`Input.Keyboard.cs`、`Input.Mouse.cs` 等，内部管理：

- 文本排版与光标
- IME 输入
- 前缀/后缀/清除按钮
- 密码模式、数字模式
- 验证状态动画

**心得**：输入框是 WinForm 自绘难度最高的控件之一，需完全接管键盘消息和 IME，不能依赖 `TextBox` 包装。

---

## 10. 布局系统

| 控件 | 模型 |
|------|------|
| `StackPanel` | 单向堆叠（水平/垂直） |
| `FlowPanel` | 流式换行 |
| `GridPanel` | 格栅（类 CSS Grid 简化版） |
| `Splitter` | 可拖拽分隔 |
| `ContainerPanel` | 带标题/边框的容器 |
| `VirtualPanel` | 虚拟化列表 |

共同特征：

- 继承 `IControl`，自绘背景与内容
- 内置 `ScrollBar`
- 子项以逻辑对象（非 `Control` 实例）管理，减少 HWND 数量

**心得**：少用 `Panel.Controls.Add()` 堆积 HWND；逻辑子项 + 自绘能显著提升大量条目时的性能。

---

## 11. 全局配置 Config

`AntdUI.Config` 静态类集中管理跨控件行为：

| 配置项 | 默认值 | 说明 |
|--------|--------|------|
| `Mode` | Light | 全局明暗主题 |
| `Animation` | true | 动画总开关 |
| `Dpi` | 自动检测 | 缩放比 |
| `ShadowEnabled` | true | 阴影 |
| `TouchEnabled` | true | 触屏滚动 |
| `MouseHoverDelay` | 200ms | 悬停延迟 |
| `TextRenderingHighQuality` | false | 路径化文字 |
| `ScrollBarHide` | false | 滚动条隐藏样式 |
| `ShowInWindow` | false | 通知锚定到窗口 |

---

## 12. WinForm UI 开发核心心得

以下是从 AntdUI 源码中提炼的、可复用到任何 WinForm 自绘项目的原则。

### 12.1 自绘（Owner Draw）是地基

1. **全面 UserPaint**：一旦有一个控件走系统绘制，整体风格就会割裂。
2. **双缓冲是底线**：`OptimizedDoubleBuffer` 不够时，用离屏 `Bitmap` 手动双缓冲（分层窗口必须如此）。
3. **绘制参数化**：把所有视觉状态（hover、pressed、disabled、animation progress）收敛为 float/Color 字段，在 `OnDraw` 中统一消费。
4. **局部 Invalidate**：滚动、悬停行变化时只刷新脏矩形，万级表格才能流畅。
5. **测量与绘制分离**：先 `GetSize`/`Measure`，再 `SetRect`/`Paint`，避免布局抖动。

### 12.2 事件定制是差异化竞争力

1. **不要只暴露 Click**：表格需要 `CellClick(Record, RowIndex, Column)`，树需要 `NodeExpand(Node)`——事件参数应携带业务上下文。
2. **Handled 拦截模式**：让用户能在默认行为前后插入逻辑，而不是只能完全替换。
3. **WndProc 是最后手段**：触屏、DPI、非客户区消息等系统级行为，必须在消息层处理；但应封装在基类，不泄漏给业务代码。
4. **IMessageFilter 管理弹出层生命周期**：下拉/Tooltip 的"点击外部关闭"几乎无法靠控件树事件实现。
5. **取消 StandardClick**：自绘按钮若需ripple/长按/拖拽，必须禁用系统点击合成。

### 12.3 动画哲学

1. **可打断 > 华丽**：新交互立即 `Dispose()` 旧 `ITask`，比播完完整动画更重要。
2. **动画 = 重绘触发器**：每帧只改一个 float 并 `Invalidate()`，不创建新控件。
3. **缓动函数表**：`AnimationType` 枚举 + `CalculateValue(progress)` 比硬编码 ease 公式更易调参。
4. **提供全局关闭**：企业环境、远程桌面、自动化测试都需要 `Config.Animation = false`。

### 12.4 窗口与弹出层

1. **无边框 ≠ 无系统能力**：通过 DWM、Snap Layout、任务栏缩略图等 API 保留原生体验。
2. **弹出层用分层窗口**：阴影、圆角、淡入淡出依赖 per-pixel alpha，普通 `Form.Opacity` 不够。
3. **静态 API + Config**：`Message.open(config)` 比让用户管理 Form 实例更贴近 Ant Design 使用习惯。
4. **弹出定位统一**：`TAlign`/`TAlignFrom` 枚举 + 屏幕/workarea 碰撞检测，避免每个控件重复写定位逻辑。

### 12.5 主题与可维护性

1. **色卡 ID 化**：`Colour.Primary` + 控件名，不要硬编码 `Color.FromArgb`。
2. **事件总线刷新**：主题切换时一条广播，所有监听控件 `Invalidate()`，避免遍历控件树。
3. **枚举优于魔法字符串**：`TTypeMini`、`TAMode`、`TFit` 等贯穿全库，智能提示友好。
4. **Config 嵌套类**：`Message.Config`、`Modal.Config` 将配置与实现隔离，API 稳定。

### 12.6 性能与兼容性

1. **控制 HWND 数量**：1000 个 `Button` 控件 ≠ 1000 次绘制；逻辑列表 + 一次绘制更轻。
2. **GDI 资源 using**：泄漏在长时间运行的桌面应用中会 OOM。
3. **多目标框架**：`#if NET40` 条件编译保持 .NET Framework 4.x 兼容，是企业 WinForm 的刚需。
4. **AOT 友好**：避免反射生成控件、少用动态 IL；AntdUI 以源生成和静态注册为主。
5. **设计器 DPI**：`ForceDesignerDpiUnaware` + 100% 缩放设计，运行时靠 `Config.Dpi` 缩放。

### 12.7 开发流程建议

1. 新控件先定义：**属性 → 布局测量 → OnDraw → 鼠标命中 → 键盘 → 动画 → 事件 → Demo**
2. 在 `example/Demo` 添加演示页，是全控件回归测试的最有效方式。
3. 遵循 `CONTRIBUTING.zh.md`：`IControl` + `Canvas` + `ScrollBar` + `RenderRegion` 四件套。

---

## 13. 典型数据流示例

### 13.1 按钮点击全流程

```
用户按下鼠标
  → OnMouseDown（Button 重写）
  → 启动 ITask 点击动画（AnimationClickValue = 1）
  → 每帧 Invalidate
  → OnDraw 绘制波纹遮罩
  → OnMouseUp → 触发 Click 事件
  → 取消/完成动画
```

### 13.2 下拉选择全流程

```
用户点击 Select
  → 计算弹出位置（TAlignFrom + 屏幕边界）
  → new LayeredFormSelectDown（ILayeredForm）
  → PrintBit 绘制下拉列表
  → Win32.SetBits 显示分层窗口
  → IMessageFilter 监听外部点击
  → 选中项 → 更新 Select 显示 → 关闭分层窗口
```

### 13.3 主题切换全流程

```
Config.IsDark = true
  → EventHub.Dispatch(THEME)
  → BaseForm.ThemeConfig.HandleEvent
  → Table / Menu / … IEventListener.HandleEvent
  → 各控件 Invalidate
  → OnDraw 通过 Style.Get(Colour, "控件名", ColorScheme) 取新色
```

---

## 14. 依赖与技术选型

| 技术 | 用途 |
|------|------|
| System.Drawing (GDI+) | 主渲染后端 |
| Vanara.PInvoke | 类型安全的 Win32/DWM API |
| 自研 SVG 栈 | 矢量图标 |
| Task + CancellationToken | 动画调度 |
| WeakReference + ConcurrentDictionary | EventHub 监听者管理 |

未使用 WPF、Skia、SharpDX——这是**纯 WinForm 技术栈**下的最大化方案，换取了框架兼容性与部署简单性。

---

## 15. 扩展阅读

- 用户 API 文档：[doc/wiki/zh/Home.md](wiki/zh/Home.md)
- 贡献规范：[CONTRIBUTING.zh.md](../CONTRIBUTING.zh.md)
- 主题配置：[doc/wiki/zh/Theme.md](wiki/zh/Theme.md)
- DPI 适配：[doc/wiki/zh/DPI.md](wiki/zh/DPI.md)
- 全局配置：[doc/wiki/zh/Config.md](wiki/zh/Config.md)

---

## 16. 总结

AntdUI 的本质是：**用 WinForm 的壳，走自绘的路，复刻 Ant Design 的魂**。

其架构核心可归纳为五个词：

> **IControl**（统一绘制契约）· **Canvas**（高质量 GDI 封装）· **ITask**（可打断动画）· **ILayeredForm**（分层弹出）· **EventHub**（全局状态广播）

对于 WinForm UI 开发者，最大的启示是：不要与系统控件搏斗，而是**接管绘制、定制事件、统一管理主题与弹出层**——这是做出"不像 WinForm"的 WinForm 应用的唯一路径。
