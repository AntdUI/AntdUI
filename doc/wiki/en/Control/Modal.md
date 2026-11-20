[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Modal

> Display a modal dialog box, providing a title, content area, and action buttons.

### Modal.Config

> Configure Modal

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Tag** | User defined data | object`?` | `null` |
**Title** | Title | string`?` | `null` |
**Content** | Control/Content | object | `Required` |
||||
**Font** | Font | Font`?` | `null` |
**CancelFont** | Cancel button font | Font`?` | `null` |
**OkFont** | Confirm button font | Font`?` | `null` |
||||
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**CloseIcon** | Display close icon | bool | false |
**IconCustom** | Custom icon | IconInfo`?` | `null` |
**UseIconPadding** | Use icon padding | bool | true |
||||
**LoadingDisableCancel** | Disable the cancel button during loading | bool | false |
||||
**CancelText** | Cancel button text | string`?` | "Cancel" |
**Draggable** | Drag and drop window | bool | true |
**EnableSound** | Enable sound | bool | false |
**MaskClosable** | Click whether to allow the mask to be closed | bool | true |
**DefaultFocus** | Default focus | bool | false |
||||
**BtnHeight** | Button bar height | int | 38 |
**ContentPadding** | Content padding | Size | 0, 0 |
**MaximumSize** | Maximum size | Size`?` | `null` |
**MinimumSize** | Minimum size | Size`?` | `null` |
**Padding** | Padding | Size | 24, 20 |
**Width** | Modal width | int | 416 |
||||
**Btns** | Custom button | [Btn[]](#modal.btn) | `null` |
**ColorScheme** | Color scheme | [TAMode](Enum.md#tamode) | Auto |
**DefaultAcceptButton** | Default accept button | bool | true |
**Form** | Belonging window | Form`?` | `Cannot use mask when empty` | `deprecated, use Target instead` |
**Keyboard** | Does it support keyboard ESC | bool | true |
**ManualActivateParent** | Manually activate parent | bool | false |
**Mask** | Display Mask | bool | true |
**OkText** | Confirm button text | string | "OK" |
**OkType** | Confirm button type | [TTypeMini](Enum.md#ttypemini) | Primary |
**OnBtns** | Custom button callback | `Func<Button, bool>?` | `null` |
**OnButtonStyle** | Custom button style callback | Action<string, Button>`?` | `null` |
**OnOk** | Confirm callback | `Func<Config, bool>?` | `null` |
**Resizable** | Resizable | bool | false |
**Target** | Belonging target | Target | `Required` |

#### Methods

Name | Description | Return Type | Parameters |
:--|:--|:--|:--|
**Close()** | Active close | void | |
**DialogResult()** | Set dialog result | void | DialogResult result = DialogResult.OK |

### Modal.Btn

> Configure Button

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Button name | string | `Required` |
**Tag** | User defined data | object`?` | `null` |
**Text** | Button text | string | `Required` |
**Type** | Button type | [TTypeMini](Enum.md#ttypemini) | Default |
||||
**Fore** | Text color | Color`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
||||
**DialogResult** | Dialog result | DialogResult | None |

### Modal.TextLine

> Text content

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Tag** | User defined data | object`?` | `null` |
**Text** | Text content | string | `Required` |
||||
**Font** | Font | Font`?` | `null` |
**Fore** | Text color | Color`?` | `null` |
||||
**Gap** | Gap | int | 0 |

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