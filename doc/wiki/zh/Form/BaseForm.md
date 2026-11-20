[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## BaseForm

支持 DPI 的基础原生窗口，是 [Window](Window.md) 与 [BorderlessForm](BorderlessForm.md) 的基类，继承自 `System.Windows.Forms.Form`。

> 🔴 过时属性提醒：`Dark`（bool）已标记 `Obsolete`，请改用 `Mode`（TAMode）。详见下方[过时属性](#过时属性)。

### 属性

名称 | 描述 | 类型 | 默认值
:--|:--|:--|:--
**AutoHandDpi** | 自动处理 DPI（窗体创建及 DPI 变化时自动缩放） | bool | true |
**Mode** | 色彩模式：`Auto` 跟随全局配置、`Light` 浅色、`Dark` 深色 | [TAMode](../Control/Enum.md#tamode) | Auto |
**DisableTheme** | 是否禁用主题 | bool | false |
**FormBorderStyle** | 窗体边框和标题栏的外观与行为（重写隐藏基类属性） | FormBorderStyle | Sizable |
|||||
**IsMax** | 是否最大化（只读） | bool | false |
**IsFull** | 是否全屏 | bool | false |
**EnableHitTest** | 鼠标拖拽窗体边缘调整大小是否启用 | bool | true |
**Dpi** | 当前窗体 DPI（1=100%、1.25=125%，只读） | float | 自动 |
|||||
**Dark** 🔴 | ~~深色模式~~ 已过时，请改用 `Mode` | bool | false |

### 过时属性

> `Dark` 已标记 `[Obsolete("use Mode")]` 且 `Browsable(false)`，设计器中不再显示。它仅为兼容旧代码保留，**新代码请使用 `Mode`**。

```csharp
// ❌ 已过时
form.Dark = true;

// ✅ 推荐：显式指定深色
form.Mode = AntdUI.TAMode.Dark;

// ✅ 推荐：跟随全局 AntdUI.Config.Mode 自动切换（默认）
form.Mode = AntdUI.TAMode.Auto;

// 读取当前是否为深色（代替读取 Dark）
bool isDark = form.Mode == AntdUI.TAMode.Dark
	|| (form.Mode == AntdUI.TAMode.Auto && AntdUI.Config.IsDark);
```

> `Mode` 取值：`Auto`（跟随全局配置，默认）、`Light`（强制浅色）、`Dark`（强制深色）。设置后窗体会同步切换原生标题栏与内部控件主题。

### 方法

名称 | 描述 | 返回值 | 参数
:--|:--|:--|:--
**Min** | 最小化 | void ||
**Max** | 最大化 | void ||
**MaxRestore** | 最大化 / 还原切换 | bool ||
**Full** | 全屏 | void ||
**NoFull** | 取消全屏 | void ||
**FullRestore** | 全屏 / 还原切换 | bool ||
|||||
**AutoDpi** | 按当前 DPI 处理控件缩放 | void | Control control `控件` |
**AutoDpi** | 按指定 DPI 处理控件缩放 | void | float dpi `DPI值`, Control control `控件` |
**RefreshDWM** | 刷新 DWM 区域（基类为空实现，子类重写） | void ||
|||||
**Theme** | 获取窗体主题配置对象 | ThemeConfig ||
**ThemeClear** | 清除窗体主题配置 | void ||
**SetCursor** | 设置鼠标指针（手型 / 默认） | void | bool val `true=手型` |
**OnCreated** | 窗体句柄创建后触发（可重写以初始化） | void ||
|||||
**DraggableMouseDown** | 拖动窗口（鼠标按下时调用） | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool | Point point `客户端坐标` |