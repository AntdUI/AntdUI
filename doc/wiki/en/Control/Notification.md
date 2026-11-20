[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Notification

> Prompt notification message globally.

### Notification.Config

> Configure Notification

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Title** | Title | string`?` | `null` |
🌏 **LocalizationTitle** | International Title | string`?` | `null` |
||||
**Font** | Font | Font`?` | `null` |
**Fore** | Custom foreground color | Color`?` | `null` |
**FontStyleTitle** | Title font style | FontStyle`?` | `null` |
**FontTitle** | Title font | Font`?` | `null` |
||||
**Back** | Custom background color | Color`?` | `null` |
||||
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**CloseIcon** | Display close icon | bool | true |
**IconCustom** | Custom icon | IconInfo`?` | `null` |
||||
**AutoClose** | Automatic shutdown time(s) `0 equals not closing` | int | 6 |
**ClickClose** | Can I click to close it | bool | true |
**EnableSound** | Whether to enable sound | bool | false |
**OnClose** | Close callback | Action`?` | `null` |
**ShowInWindow** | Pop up in the window | bool`?` | `null` |
||||
**Radius** | Rounded corners | int | 10 |
||||
**Align** | Align | [TAlignFrom](Enum.md#talignfrom) | TR |
**Padding** | Padding | Size | 24, 20 |
||||
**Form** | Belonging window (obsolete, use Target) | Control`?` | `null` |
**Link** | Hyperlink | IConfigControl`?` | `null` |
**Links** | Hyperlink combination | IConfigControl[]`?` | `null` |
**Target** | Belonging target | Target | `Required` |
**TopMost** | Topped | bool | false |

#### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**close_all** | Close all | void | |
**close_id** | Close specified ID | void | string id |
**contains** | Determine if the notification ID exists in the queue | bool | string id, bool time_expand = false |

### Notification.ConfigLink

> Configure Hyperlink

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Tag** | User defined data | object`?` | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**Fore** | Text color | Color`?` | `null` |
||||
**Call** | Click on callback | Func<bool>`?` | `null` |
**Link** | Whether to display label | bool | true |