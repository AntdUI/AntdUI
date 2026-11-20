[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## BorderlessForm

Borderless Shadow Window

> Borderless shadow window implemented based on `FormBorderStyle.None`. Inherited from [BaseForm](BaseForm); see [BaseForm](BaseForm) for available properties / methods.
>
> 🔴 `Dark` (bool) is obsolete; use `Mode` (TAMode) instead. See [BaseForm obsolete property](BaseForm.md#obsolete-property).

### Properties

Name | Description | Type | Default Value
:--|:--|:--|:--
**Radius** | Corner radius | int | 0 |
**Resizable** | Whether the window can be resized | bool | true |
**ShowInTaskbar** | Whether it appears in the Windows taskbar (overrides base property) | bool | true |
|||||
**UseDwm** | Use DWM system shadow. When `true`, custom color / border / radius settings below have no effect | bool | true |
**Shadow** | Shadow size (`0` disables custom shadow) | int | 10 |
**ShadowColor** | Shadow color | Color | 100, 0, 0, 0 |
**ShadowPierce** | Mouse pass-through over the shadow area | bool | false |
|||||
**BorderWidth** | Border width | int | 1 |
**BorderColor** | Border color (`null` uses the theme default) | Color`?` | `null` |
|||||
**IsMax** | Whether maximized (read-only) | bool | false |
**IsFull** | Whether full screen | bool | false |
**AutoHandDpi** | Auto handle DPI | bool | true |
**DisableTheme** | Whether to disable theme | bool | false |

> Tip: `UseDwm = true` uses the Windows system shadow with better performance, but custom appearance settings (`Shadow` / `ShadowColor` / `Radius` / `BorderWidth` / `BorderColor`) have no effect. Set it to `false` when you need a highly customized appearance.

### Methods

Name | Description | Return Value | Parameters
:--|:--|:--|:--
**DraggableMouseDown** | Drag the window (call on mouse down) | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse move) | bool ||
**ResizableMouseMove** | Adjust window size (mouse move) | bool | Point point `client coordinates` |
|||||
**MaxRestore** | Toggle maximize / restore | bool ||
**Max** | Maximize window | void ||
**FullRestore** | Toggle full screen / restore | bool ||
**Full** | Full screen window | void ||
**NoFull** | Exit full screen | void ||
|||||
**RefreshDWM** | Refresh the DWM area | void ||
**Dpi** | Get DPI | float ||
**Theme** | Get theme configuration | ThemeConfig ||
**ThemeClear** | Clear theme configuration | void ||

### Example

```csharp
public partial class Form1 : AntdUI.BorderlessForm
{
	public Form1()
	{
		InitializeComponent();
		Mode = AntdUI.TAMode.Auto;   // Follow global theme (recommended)
		UseDwm = false;              // Disable system shadow to enable custom appearance
		Radius = 12;                 // Corner radius
		Shadow = 20;                 // Custom shadow size
	}
}
```