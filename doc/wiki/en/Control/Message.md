[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Message

> Display global messages as feedback in response to user operations.

### Message.Config

> Configure Message

#### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Target** | Target | Target | `Required` |
**Form** | Belonging window (Obsolete, use Target) | Control`?` | `null` |
**Text** | Text | string | `Required` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Icon** | Icon | [TType](Enum.md#ttype) | None |
**IconCustom** | Custom Icon | IconInfo`?` | `null` |
**Font** | Font | Font`?` | `null` |
**Radius** | Rounded corners | int | 6 |
**AutoClose** | Automatic shutdown time(s) `0 equals not closing` | int | 6 |
**ClickClose** | Can I click to close it | bool | true |
**TopMost** | Whether to top | bool | false |
**Align** | Align | [TAlignFrom](Enum.md#talignfrom) | Top |
**Padding** | Padding | Size | 12, 9 |
**ShowInWindow** | Pop up in the window | bool`?` | `null` |
**EnableSound** | Whether to enable sound | bool | false |
**MaxWidth** | Max width | int`?` | `null` |
**Back** | Custom background color | Color`?` | `null` |
**Fore** | Custom foreground color | Color`?` | `null` |
**Call** | Load callback | object`?` | `null` |

#### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**close_all** | Close all | void | |
**close_id** | Close specified ID | void | string id |
**contains** | Determine if the prompt ID exists in the queue | bool | string id, bool time_expand = false |

> Loading business method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**OK** | Success | void | string text |
**Error** | Error | void | string text |
**Warn** | Warn | void | string text |
**Info** | Info | void | string text |
**Refresh** | Refresh UI | void ||