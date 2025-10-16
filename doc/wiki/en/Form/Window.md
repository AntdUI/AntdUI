[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Window

Native borderless window

> A perfect borderless window with native features. Inherited from [BaseForm](BaseForm)

### Propertie

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Resizable** | Adjust window size to enable | bool | true |
**Dark** | Dark Mode | bool | false |
**Mode** | Color mode | [TAMode](../Control/Enum.md#tamode) | Auto 
**ScreenRectangle** | Get or set the screen area of the form | Rectangle |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DraggableMouseDown** | | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool ||