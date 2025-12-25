[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Window

Native borderless window

> A perfect borderless window with native features. Inherited from [BaseForm](BaseForm)

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Resizable** | Adjust window size to enable | bool | true |
**Dark** | Dark Mode | bool | false |
**Mode** | Color mode | [TAMode](../Control/Enum.md#tamode) | Auto 
**ScreenRectangle** | Get or set the screen area of the form | Rectangle |
**IsMax** | Whether it is maximized | bool | false |
**AutoHandDpi** | Auto handle DPI | bool | true |
**DisableTheme** | Whether to disable theme | bool | false |
**IsFull** | Whether it is full screen | bool | false |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DraggableMouseDown** | Drag window | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool | point: Client coordinates |
**Min** | Minimize | void ||
**Max** | Maximize | void ||
**MaxRestore** | Maximize/Restore | bool ||
**Full** | Full screen | void ||
**NoFull** | Cancel full screen | void ||
**FullRestore** | Full screen/Restore | bool ||
**Dpi** | Get DPI | float ||
**AutoDpi** | Handle DPI | void | control: Control |
**AutoDpi** | Handle DPI | void | dpi: DPI value, control: Control |
**Theme** | Get theme configuration | ThemeConfig ||
**ThemeClear** | Clear theme configuration | void ||