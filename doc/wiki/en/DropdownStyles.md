[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)d)

## SelectItem

> Support richer UI

Name | Description | Type | Empty | Default Value|
:--|:--|:--|:--:|:--|
**Online** | Online status `1 is green dot, 0 is red dot` | int`?` |✅| `null` |
**OnlineCustom** | Custom colors online | Color`?` |✅| `null` |
**Enable** | | bool |⛔| true |
**Icon** | | Image`?` |✅| `null` |
**IconSvg** | | string`?` |✅| `null` |
**Text** | Display Text | string |⛔| `Required` |
**SubText** | Sub text | string`?` |✅| `null` |
**Sub** | Sub option ♾️ | `List<object>?` |✅| `null` |
**Tag** | Raw | object |⛔| `Required` |
|||||
**TagFore** 🔴 | Tag text color | Color`?` |✅| `null` |
**TagBack** 🔴 | Tag background color | Color`?` |✅| `null` |
**TagBackExtend** 🔴 | Tag background gradient color | string`?` |✅| `null` |

## DividerSelectItem

> Divider