[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Modal

> Display a modal dialog box, providing a title, content area, and action buttons.

### Modal.Config

> Configure Modal

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Target** | Belonging target | Target | `Required` |
**Form** | Belonging window | Form`?` | `Cannot use mask when empty` | `deprecated, use Target instead` |
**Title** | Title | string`?` | `null` |
**Content** | Control/Content | object | `Required` |
**ContentPadding** | Content padding | Size | 0, 0 |
**UseIconPadding** | Use icon padding | bool | true |
**Width** | Modal width | int | 416 |
**Font** | Font | Font`?` | `null` |
**ColorScheme** | Color scheme | [TAMode](Enum.md#tamode) | Auto |
**Keyboard** | Does it support keyboard ESC | bool | true |
**Mask** | Display Mask | bool | true |
**MaskClosable** | Click whether to allow the mask to be closed | bool | true |
**ManualActivateParent** | Manually activate parent | bool | false |
**CloseIcon** | Display close icon | bool | false |
**DefaultFocus** | Default focus | bool | false |
**DefaultAcceptButton** | Default accept button | bool | true |
**CancelFont** | Cancel button font | Font`?` | `null` |
**OkFont** | Confirm button font | Font`?` | `null` |
**BtnHeight** | Button bar height | int | 38 |
**Padding** | Padding | Size | 24, 20 |
**CancelText** | Cancel button text | string`?` | "Cancel" |
**OkText** | Confirm button text | string | "OK" |
**OkType** | Confirm button type | [TTypeMini](Enum.md#ttypemini) | Primary |
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**IconCustom** | Custom icon | IconInfo`?` | `null` |
**OnOk** | Confirm callback | `Func<Config, bool>?` | `null` |
**Tag** | User defined data | object`?` | `null` |
**LoadingDisableCancel** | Disable the cancel button during loading | bool | false |
**Draggable** | Drag and drop window | bool | true |
**EnableSound** | Enable sound | bool | false |
**Btns** | Custom button | [Btn[]](#modal.btn) | `null` |
**OnBtns** | Custom button callback | `Func<Button, bool>?` | `null` |
**OnButtonStyle** | Custom button style callback | Action<string, Button>`?` | `null` |
**Resizable** | Resizable | bool | false |
**MinimumSize** | Minimum size | Size`?` | `null` |
**MaximumSize** | Maximum size | Size`?` | `null` |

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
**Text** | Button text | string | `Required` |
**Type** | Button type | [TTypeMini](Enum.md#ttypemini) | Default |
**Fore** | Text color | Color`?` | `null` |
**Back** | Background color | Color`?` | `null` |
**DialogResult** | Dialog result | DialogResult | None |
**Tag** | User defined data | object`?` | `null` |

### Modal.TextLine

> Text content

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text content | string | `Required` |
**Gap** | Gap | int | 0 |
**Fore** | Text color | Color`?` | `null` |
**Font** | Font | Font`?` | `null` |
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