[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## BorderlessForm

无边框阴影窗口

> 基于 `FormBorderStyle.None` 实现的无边框阴影窗口。继承于 [BaseForm](BaseForm)，可用属性 / 方法参见 [BaseForm](BaseForm)。
>
> 🔴 `Dark`（bool）已过时，请改用 `Mode`（TAMode），详见 [BaseForm 过时属性](BaseForm.md#过时属性)。

### 属性

名称 | 描述 | 类型 | 默认值
:--|:--|:--|:--
**Radius** | 圆角大小 | int | 0 |
**Resizable** | 是否允许调整窗口大小 | bool | true |
**ShowInTaskbar** | 是否显示在 Windows 任务栏（重写基类属性） | bool | true |
|||||
**UseDwm** | 使用 DWM 系统阴影。`true` 时下方颜色 / 边框 / 圆角等自定义项不生效 | bool | true |
**Shadow** | 阴影大小（`0` 关闭自定义阴影） | int | 10 |
**ShadowColor** | 阴影颜色 | Color | 100, 0, 0, 0 |
**ShadowPierce** | 鼠标穿透阴影区域 | bool | false |
|||||
**BorderWidth** | 边框宽度 | int | 1 |
**BorderColor** | 边框颜色（`null` 为主题默认色） | Color`?` | `null` |
|||||
**IsMax** | 是否最大化（只读） | bool | false |
**IsFull** | 是否全屏 | bool | false |
**AutoHandDpi** | 自动处理 DPI | bool | true |
**DisableTheme** | 是否禁用主题 | bool | false |

> 提示：`UseDwm = true` 时使用 Windows 系统阴影，性能更好，但 `Shadow` / `ShadowColor` / `Radius` / `BorderWidth` / `BorderColor` 等自定义外观将不生效；需要高度自定义外观时设为 `false`。

### 方法

名称 | 描述 | 返回值 | 参数
:--|:--|:--|:--
**DraggableMouseDown** | 拖动窗口（鼠标按下时调用） | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool | Point point `客户端坐标` |
|||||
**MaxRestore** | 最大化 / 还原切换 | bool ||
**Max** | 最大化窗口 | void ||
**FullRestore** | 全屏 / 还原切换 | bool ||
**Full** | 全屏窗口 | void ||
**NoFull** | 退出全屏 | void ||
|||||
**RefreshDWM** | 刷新 DWM 区域 | void ||
**Dpi** | 获取 DPI | float ||
**Theme** | 获取主题配置 | ThemeConfig ||
**ThemeClear** | 清除主题配置 | void ||

### 使用示例

```csharp
public partial class Form1 : AntdUI.BorderlessForm
{
	public Form1()
	{
		InitializeComponent();
		Mode = AntdUI.TAMode.Auto;   // 跟随全局主题（推荐）
		UseDwm = false;              // 关闭系统阴影以启用自定义外观
		Radius = 12;                 // 圆角
		Shadow = 20;                 // 自定义阴影大小
	}
}
```