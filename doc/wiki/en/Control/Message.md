[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Message

> Display global messages as feedback in response to user operations.

### Message.Config

> Configure Message

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Form** | Belonging window | Form | `Required` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**Font** | Font | Font | `null` |
**Radius** | Rounded corners | int | 6 |
**AutoClose** | Automatic shutdown time(s) `0 equals not closing` | int | 6 |
**ClickClose** | Can I click to close it | bool | true |
**Align** | Align | [TAlignFrom](Enum.md#talignfrom) | Top |
**Padding** | Padding | Size | 12, 9 |
**ShowInWindow** | Pop up in the window | bool | false |
**Call** | Load callback | Action<Config>`?` | `null` |

#### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**close_all** | Close all | void | |
**close_id** | Close specified ID | void | string id |

> Loading business method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**OK** | Success | void | string text |
**Error** | Error | void | string text |
**Warn** | Warn | void | string text |
**Info** | Info | void | string text |
**Refresh** | Refresh UI | void ||