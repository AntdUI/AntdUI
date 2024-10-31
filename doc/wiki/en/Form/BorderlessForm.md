[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## BorderlessForm

Borderless Shadow Window

> Borderless shadow window implemented based on `FormBorderStyle.None`. Inherited from [BaseForm](BaseForm)

### Propertie

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Resizable** | Adjust window size to enable | bool | true |
**Dark** | Dark Mode | bool | false |
**Mode** | Color mode | [TAMode](../Control/Enum#tamode) | Auto 
**Radius** | Rounded corners | int | 0 |
||||
**UseDwm** 🔴 | `dwmapi` | bool | true |
**Shadow** | Shadow size | int | 10 |
**ShadowColor** | | Color | 100, 0, 0, 0 |
**ShadowPierce** 🔴 | Mouse penetration | bool | false |
||||
**BorderWidth** | | float | 0F |
**BorderColor** | | Color | 246, 248, 250 |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DraggableMouseDown** | | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool ||