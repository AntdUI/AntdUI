[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Notification

> Prompt notification message globally.

### Notification.Config

> Configure Notification

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Target** | Belonging target | Target | `Required` |
**Form** | Belonging window (obsolete, use Target) | Control`?` | `null` |
**Title** | Title | string`?` | `null` |
🌏 **LocalizationTitle** | International Title | string`?` | `null` |
**FontTitle** | Title font | Font`?` | `null` |
**FontStyleTitle** | Title font style | FontStyle`?` | `null` |
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**IconCustom** | Custom icon | IconInfo`?` | `null` |
**Font** | Font | Font`?` | `null` |
**Align** | Align | [TAlignFrom](Enum.md#talignfrom) | TR |
**Radius** | Rounded corners | int | 10 |
**AutoClose** | Automatic shutdown time(s) `0 equals not closing` | int | 6 |
**ClickClose** | Can I click to close it | bool | true |
**CloseIcon** | Display close icon | bool | true |
**TopMost** | Topped | bool | false |
**Link** | Hyperlink | IConfigControl`?` | `null` |
**Links** | Hyperlink combination | IConfigControl[]`?` | `null` |
**Padding** | Padding | Size | 24, 20 |
**ShowInWindow** | Pop up in the window | bool`?` | `null` |
**EnableSound** | Whether to enable sound | bool | false |
**Back** | Custom background color | Color`?` | `null` |
**Fore** | Custom foreground color | Color`?` | `null` |
**OnClose** | Close callback | Action`?` | `null` |

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
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Fore** | Text color | Color`?` | `null` |
**Link** | Whether to display label | bool | true |
**Call** | Click on callback | Func<bool>`?` | `null` |
**Tag** | User defined data | object`?` | `null` |