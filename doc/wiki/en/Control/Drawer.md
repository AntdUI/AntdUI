[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Drawer

> A panel that slides out from the edge of the screen.

### Drawer.Config

> Configure Drawer

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Form** | Belonging window | Form | `Required` |
**Content** | Content | Control | `Required` |
**Mask** | Display mask or not | bool | true |
**MaskClosable** | Click whether to allow the mask to be closed | bool | true |
**Padding** | Padding | int | 24 |
**Align** | Align | [TAlignMini](Enum.md#talignmini) | Right |
**Dispose** | Should it be released | bool | true |
**Tag** | User defined data | object`?` | `null` |
**OnLoad** | Load callback | Action`?` | `null` |
**OnClose** | Close callback | Action`?` | `null` |
**DisplayDelay** 🔴 | Display Delay `Adding delay can effectively avoid competing with Mask animation` | int | 100 |

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

### Example of blockage/waiting

~~~csharp
private async void button1_Click(object sender, EventArgs e)
{
    var usercontrol = new UserControl1(form);
    await AntdUI.Drawer.wait(form, usercontrol, AntdUI.TAlignMini.Left);
    System.Diagnostics.Debug.WriteLine("End：" + usercontrol.ToString());
}
~~~