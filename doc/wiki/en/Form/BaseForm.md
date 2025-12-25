[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## BaseForm

Basic native window supporting DPI

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**AutoHandDpi** | Auto process DPI enable | bool | true |
**Dark** | Dark Mode | bool | false |
**Mode** | Color mode | [TAMode](../Control/Enum.md#tamode) | Auto |
**IsMax** | Whether it is maximized | bool | false |
**DisableTheme** | Whether to disable theme | bool | false |
**IsFull** | Whether it is full screen | bool | false |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Min** | Minimize | void ||
**Max** | Maximize | void ||
**MaxRestore** | Maximize/Restore | bool ||
**Full** | Full screen | void ||
**NoFull** | Cancel full screen | void ||
**FullRestore** | Full screen/Restore | bool ||
**Dpi** | Get DPI | float ||
**AutoDpi** | DPI Scaling | void | Control control |
**AutoDpi** | DPI Scaling | void | float dpi, Control control |
**Theme** | Get theme configuration | ThemeConfig ||
**ThemeClear** | Clear theme configuration | void ||
**DraggableMouseDown** | Drag window | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool ||