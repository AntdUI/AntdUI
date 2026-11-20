[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Window

原生无边框窗口（带系统标题栏特性）

> 拥有原生特性的完美无边框窗口。继承于 [BaseForm](BaseForm)，可用属性 / 方法参见 [BaseForm](BaseForm)。
>
> 🔴 `Dark`（bool）已过时，请改用 `Mode`（TAMode），详见 [BaseForm 过时属性](BaseForm.md#过时属性)。

### 属性

名称 | 描述 | 类型 | 默认值
:--|:--|:--|:--
**Resizable** | 是否允许调整窗口大小 | bool | true |
**BorderColor** | 窗口边框颜色（`null` 为主题默认色） | Color`?` | `null` |
**ShowInTaskbar** | 是否显示在 Windows 任务栏（重写基类属性） | bool | true |
|||||
**Location** | 窗体位置（重写，最大化时返回屏幕区域位置） | Point | |
**Size** | 窗体大小（重写，最大化时返回屏幕区域大小） | Size | |
**ClientSize** | 窗体客户端区域大小 | Size | |
**Width** | 窗体宽度 | int | |
**Height** | 窗体高度 | int | |
|||||
**Top** | 窗体顶部坐标（只读映射） | int | |
**Left** | 窗体左侧坐标（只读映射） | int | |
**Right** | 窗体右侧坐标（只读） | int | |
**Bottom** | 窗体底部坐标（只读） | int | |
|||||
**ScreenRectangle** | 获取或设置窗体在屏幕上的区域 | Rectangle | |
**CanHandMessage** | 静态全局开关：是否启用鼠标消息过滤（用于边缘拖拽调整大小） | bool | true |
**IsMax** | 是否最大化（只读） | bool | false |
**AutoHandDpi** | 自动处理 DPI | bool | true |
**DisableTheme** | 是否禁用主题 | bool | false |
**IsFull** | 是否全屏 | bool | false |

### 方法

名称 | 描述 | 返回值 | 参数
:--|:--|:--|:--
**DraggableMouseDown** | 拖动窗口（鼠标按下时调用） | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool | Point point `客户端坐标` |
|||||
**Min** | 最小化 | void ||
**Max** | 最大化 | void ||
**MaxRestore** | 最大化 / 还原切换 | bool ||
**Full** | 全屏 | void ||
**NoFull** | 取消全屏 | void ||
**FullRestore** | 全屏 / 还原切换 | bool ||
|||||
**Dpi** | 获取 DPI | float ||
**AutoDpi** | 处理 DPI | void | Control control `控件` |
**AutoDpi** | 处理 DPI | void | float dpi `DPI值`, Control control `控件` |
**RefreshDWM** | 刷新 DWM 区域 | void ||
**Theme** | 获取主题配置 | ThemeConfig ||
**ThemeClear** | 清除主题配置 | void ||

### 使用示例

```csharp
public partial class Form1 : AntdUI.Window
{
	public Form1()
	{
		InitializeComponent();
		Mode = AntdUI.TAMode.Auto;          // 跟随全局主题（推荐）
		BorderColor = Color.FromArgb(200, 0, 0, 0); // 自定义边框色
	}
}
```