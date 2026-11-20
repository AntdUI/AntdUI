[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## BaseForm

Basic native window supporting DPI. It is the base class of [Window](Window.md) and [BorderlessForm](BorderlessForm.md), inheriting from `System.Windows.Forms.Form`.

> 🔴 Obsolete property notice: `Dark` (bool) is marked `Obsolete`; use `Mode` (TAMode) instead. See [Obsolete property](#obsolete-property) below.

### Properties

Name | Description | Type | Default Value
:--|:--|:--|:--
**AutoHandDpi** | Auto handle DPI (scale the form automatically on creation and DPI changes) | bool | true |
**Mode** | Color mode: `Auto` follows global config, `Light`, `Dark` | [TAMode](../Control/Enum.md#tamode) | Auto |
**DisableTheme** | Whether to disable theme | bool | false |
**FormBorderStyle** | Appearance and behavior of the form border and title bar (hides base property) | FormBorderStyle | Sizable |
|||||
**IsMax** | Whether maximized (read-only) | bool | false |
**IsFull** | Whether full screen | bool | false |
**EnableHitTest** | Enable resizing the window by dragging its edges | bool | true |
**Dpi** | Current form DPI (1=100%, 1.25=125%, read-only) | float | Auto |
|||||
**Dark** 🔴 | ~~Dark mode~~ Obsolete, use `Mode` instead | bool | false |

### Obsolete property

> `Dark` is marked `[Obsolete("use Mode")]` and `Browsable(false)`. It is kept only for backward compatibility; **new code should use `Mode`**.

```csharp
// ❌ Obsolete
form.Dark = true;

// ✅ Recommended: explicitly set dark
form.Mode = AntdUI.TAMode.Dark;

// ✅ Recommended: follow global AntdUI.Config.Mode automatically (default)
form.Mode = AntdUI.TAMode.Auto;

// Read whether currently dark (instead of reading Dark)
bool isDark = form.Mode == AntdUI.TAMode.Dark
	|| (form.Mode == AntdUI.TAMode.Auto && AntdUI.Config.IsDark);
```

> `Mode` values: `Auto` (follow global config, default), `Light` (force light), `Dark` (force dark). Setting it switches the native title bar and internal controls to the matching theme.

### Methods

Name | Description | Return Value | Parameters
:--|:--|:--|:--
**Min** | Minimize | void ||
**Max** | Maximize | void ||
**MaxRestore** | Toggle maximize / restore | bool ||
**Full** | Full screen | void ||
**NoFull** | Cancel full screen | void ||
**FullRestore** | Toggle full screen / restore | bool ||
|||||
**AutoDpi** | Scale a control by the current DPI | void | Control control |
**AutoDpi** | Scale a control by the specified DPI | void | float dpi, Control control |
**RefreshDWM** | Refresh the DWM area (empty in base, overridden in subclasses) | void ||
|||||
**Theme** | Get the form theme configuration object | ThemeConfig ||
**ThemeClear** | Clear the form theme configuration | void ||
**SetCursor** | Set the mouse cursor (hand / default) | void | bool val `true=hand` |
**OnCreated** | Fired after the form handle is created (override to initialize) | void ||
|||||
**DraggableMouseDown** | Drag the window (call on mouse down) | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse move) | bool ||
**ResizableMouseMove** | Adjust window size (mouse move) | bool | Point point `client coordinates` |