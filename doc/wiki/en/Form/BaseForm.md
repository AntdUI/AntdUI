[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## BaseForm

Basic native window supporting DPI

### Propertie

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**AutoHandDpi** | Auto process DPI enable | bool | true |
**Dark** | Dark Mode | bool | false |
**Mode** | Color mode | [TAMode](../Control/Enum.md#tamode) | Auto |
**IsMax** 🔴 | Is it maximizing | bool | false `ReadOnly` |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Min** | Minimize | void ||
**Max** | Maximize | void ||
**MaxRestore** | Maximize/Restore | void ||
**FullRestore** | Full screen/Restore | void ||
**Full** | Full screen | void ||
**NoFull** | Cancel full screen | void ||
||||
**Dpi** | Get DPI | float ||
**AutoDpi** | DPI Scaling | void | Control control |
**AutoDpi** | DPI Scaling | void | float dpi, Control control |
||||
**DraggableMouseDown** | | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool ||