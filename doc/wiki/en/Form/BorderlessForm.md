[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## BorderlessForm

Borderless Shadow Window

> Borderless shadow window implemented based on `FormBorderStyle.None`. Inherited from [BaseForm](BaseForm)

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Resizable** | Adjust window size to enable | bool | true |
**Dark** | Dark Mode | bool | false |
**Mode** | Color mode | [TAMode](../Control/Enum.md#tamode) | Auto 
**Radius** | Rounded corners | int | 0 |
**ShowInTaskbar** | Whether the form appears in the Windows taskbar | bool | true |
||||
**UseDwm** | Use DWM shadow `Custom colors, borders, and rounded corners do not take effect when using system shadows` | bool | true |
**Shadow** | Shadow size | int | 10 |
**ShadowColor** | Shadow color | Color | 100, 0, 0, 0 |
**ShadowPierce** | Mouse penetration | bool | false |
||||
**BorderWidth** | Border width | int | 1 |
**BorderColor** | Border color | Color | 180, 0, 0, 0 |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**DraggableMouseDown** | Drag window | void ||
**ResizableMouseDown** | Adjust window size (mouse press) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool ||
**ResizableMouseMove** | Adjust window size (mouse movement) | bool | point: Client coordinates |
**MaxRestore** | Maximize/Restore window | bool ||
**Max** | Maximize window | void ||
**FullRestore** | Full screen/Restore window | bool ||
**Full** | Full screen window | void ||
**NoFull** | Exit full screen | void ||
**RefreshDWM** | Refresh DWM area | void ||