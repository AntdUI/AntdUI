[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Modal

> Display a modal dialog box, providing a title, content area, and action buttons.

### Modal.Config

> Configure Modal

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Form** | Belonging window | Form`?` | `Cannot use mask when empty` |
**Title** | Title | string | `Required` |
**Content** | Control/Content | object | `Required` |
**Width** | Modal width | int | 416 |
**Font** | Font | Font`?` | `null` |
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**Keyboard** | Does it support disabling keyboard ESC | bool | true |
**Mask** | Display Mask | bool | true |
**MaskClosable** | Click whether to allow the mask to be closed | bool | true |
**CloseIcon** | Display close icon | bool | false |
**Tag** | User defined data | object`?` | `null` |
||||
**Padding** | Padding | Size | 24, 20 |
**BtnHeight** | Button bar height | int | 38 |
**CancelText** | Cancel button text | string | "Cancel" |
**CancelFont** | Cancel button font | Font`?` | `null` |
**OkText** | Confirm button text | string | "OK" |
**OkType** | Confirm button type | [TTypeMini](Enum.md#ttypemini) | Primary |
**OkFont** | Confirm button Font | Font`?` | `null` |
**OnOk** | Confirm callback | `Func<Config, bool>?` | `null` |
||||
**Btns** | Custom button | [Btn[]](#modal.btn) | `null` |
**OnBtns** | Custom button callback | Action<[Button](#button)> | `null` |
**OnButtonStyle** | Custom button style callback | Action<string, [Button](Button)> | `null` |
||||
**LoadingDisableCancel** | Disable the cancel button during loading | bool | false |
**Draggable** | Drag and drop window | bool | true |
**Close()** | Active close | void | |

### Modal.Btn

> Configure Button

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Button name | string | `Required` |
**Text** | Button text | string | `Required` |
**Type** | Button type | [TTypeMini](Enum.md#ttypemini) | Default |
**Fore** | Text color | Color`?` | `null` |
**Back** | background color | Color`?` | `null` |
**Tag** | User defined data | object`?` | `null` |

***

### UserControl Monitoring Load Example

~~~csharp
public partial class UserControl1 : UserControl, AntdUI.ControlEvent
{
    public void LoadCompleted()
    {
        System.Diagnostics.Debug.WriteLine("Load");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        System.Diagnostics.Debug.WriteLine("Close");
    }
}
~~~