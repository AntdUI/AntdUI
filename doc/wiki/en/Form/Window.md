[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Window

Native borderless window (with native title bar features)

> A perfect borderless window with native features. Inherited from [BaseForm](BaseForm); see [BaseForm](BaseForm) for available properties / methods.
>
> 🔴 `Dark` (bool) is obsolete; use `Mode` (TAMode) instead. See [BaseForm obsolete property](BaseForm.md#obsolete-property).

### Properties

Name | Description | Type | Default Value
:--|:--|:--|:--
**Resizable** | Whether the window can be resized | bool | true |
**BorderColor** | Window border color (`null` uses the theme default) | Color`?` | `null` |
**ShowInTaskbar** | Whether it appears in the Windows taskbar (overrides base property) | bool | true |
|||||
**Location** | Form location (overridden; returns screen-area location when maximized) | Point | |
**Size** | Form size (overridden; returns screen-area size when maximized) | Size | |
**ClientSize** | Size of the form's client area | Size | |
**Width** | Form width | int | |
**Height** | Form height | int | |
|||||
**Top** | Top coordinate (read-only mapping) | int | |
**Left** | Left coordinate (read-only mapping) | int | |
**Right** | Right coordinate (read-only) | int | |
**Bottom** | Bottom coordinate (read-only) | int | |
|||||
**ScreenRectangle** | Get or set the form's screen area | Rectangle | |
**CanHandMessage** | Static global switch: enable mouse message filtering (used for edge-drag resizing) | bool | true |
**IsMax** | Whether maximized (read-only) | bool | false |
**AutoHandDpi** | Auto handle DPI | bool | true |
**DisableTheme** | Whether to disable theme | bool | false |
**IsFull** | Whether full screen | bool | false |

### Methods

Name | Description | Return Value | Parameters
:--|:--|:--|:--
**DraggableMouseDown** | Drag the window (call on mouse down) | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse move) | bool ||
**ResizableMouseMove** | Adjust window size (mouse move) | bool | Point point `client coordinates` |
|||||
**Min** | Minimize | void ||
**Max** | Maximize | void ||
**MaxRestore** | Toggle maximize / restore | bool ||
**Full** | Full screen | void ||
**NoFull** | Cancel full screen | void ||
**FullRestore** | Toggle full screen / restore | bool ||
|||||
**Dpi** | Get DPI | float ||
**AutoDpi** | Handle DPI | void | Control control |
**AutoDpi** | Handle DPI | void | float dpi, Control control |
**RefreshDWM** | Refresh the DWM area | void ||
**Theme** | Get theme configuration | ThemeConfig ||
**ThemeClear** | Clear theme configuration | void ||

### Example

```csharp
public partial class Form1 : AntdUI.Window
{
	public Form1()
	{
		InitializeComponent();
		Mode = AntdUI.TAMode.Auto;                    // Follow global theme (recommended)
		BorderColor = Color.FromArgb(200, 0, 0, 0);   // Custom border color
	}
}
```