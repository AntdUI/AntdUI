[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Notification

> Prompt notification message globally.

### Notification.Config

> Configure Notification

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Form** | Belonging window | Form | `Required` |
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**Font** | Font | Font`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
|||||
**Title** | Title | string | `Required` |
🌏 **LocalizationTitle** | International Title | string`?` | `null` |
**FontTitle** | Title font | Font`?` | `null` |
**FontStyleTitle** | Title font style | FontStyle`?` | `null` |
|||||
**Radius** | Rounded corners | int | 10 |
**Align** | Align | [TAlignFrom](Enum.md#talignfrom) | Right |
**Padding** | Padding | Size | 24, 20 |
**AutoClose** | Automatic shutdown time(s) `0 equals not closing` | int | 6 |
**ClickClose** | Can I click to close it | bool | true |
**CloseIcon** | Display close icon | bool | false |
**TopMost** | Topped | bool | false |
**Tag** | User defined data | object`?` | `null` |
**Link** | Hyperlink | [Modal.ConfigLink](#modal.configlink)`?` | `null` |
**ShowInWindow** | Pop up in the window | bool | false |
**OnClose** | Close callback | Action`?` | `null` |

#### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**close_all** | Close all | void | |
**close_id** | Close specified ID | void | string id |

### Modal.ConfigLink

> Configure Hyperlink

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Button text | string | `Required` |
**Call** | Click on callback | Action | `Required` |