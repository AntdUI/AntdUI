[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)

> Style objects for dropdown items, used in the `Items` of controls such as `Select` and `Combo`. Supports custom icons, online status, sub-text, tags and theme colors.

## SelectItem

> Support richer UI

Name | Description | Type | Empty | Default Value|
:--|:--|:--|:--:|:--|
**Online** | Online status `1 is green dot, 0 is red dot` | int`?` |✅| `null` |
**OnlineCustom** | Custom colors online | Color`?` |✅| `null` |
**Enable** | Whether enabled | bool |⛔| true |
|||||
**Icon** | Icon | Image`?` |✅| `null` |
**IconSvg** | SVG icon | string`?` |✅| `null` |
**IconRatio** | Icon ratio | float`?` |✅| `null` |
|||||
**Text** | Display Text | string |⛔| `Required` |
🌏 **LocalizationText** | Localization text | string`?` | `null` |
**SubText** | Sub text | string`?` |✅| `null` |
🌏 **LocalizationSubText** | Localization sub text | string`?` | `null` |
**Sub** | Sub option ♾️ | `List<object>?` |✅| `null` |
**MaxCount** | Maximum number of sub options displayed | int |⛔| `0` |
**Tag** | Raw | object |⛔| `Required` |
|||||
**TagFore** 🔴 | Tag text color | Color`?` |✅| `null` |
**TagBack** 🔴 | Tag background color | Color`?` |✅| `null` |
**TagBackExtend** 🔴 | Tag background gradient color | string`?` |✅| `null` |
|||||
**Fore** 🔴 | Text color | Color`?` |✅| `null` |
**ForeSub** 🔴 | Sub text color | Color`?` |✅| `null` |
**BackActive** 🔴 | Active background color | Color`?` |✅| `null` |
**BackActiveExtend** 🔴 | Active background gradient color | string`?` |✅| `null` |

### Fluent setter methods

> All methods below return the `SelectItem` itself, so calls can be chained

Name | Description | Parameters
:--|:--|:--
**SetText** | Set display text and localization text | string value `text`, string? localization = null `localization text` |
**SetSubText** | Set sub text and localization sub text | string? value `sub text`, string? localization = null `localization sub text` |
**SetEnable** | Set whether enabled | bool value = false |
**SetOnline** | Set online status | int? value = 1 `1 green dot, 0 red dot` |
**SetOnline** | Set custom online color | Color? value |
**SetOnline** | Set online status and custom color together | int? value, Color? color |
**SetIcon** | Set icon | Image? value |
**SetIcon** | Set SVG icon | string? svg |
**SetIcon** | Set icon and SVG together | Image? value, string? svg |
**SetIconRatio** | Set icon ratio | float? value |
**SetSub** | Set sub options | params object[] value |
**SetSub** | Set maximum number of sub options displayed (maps to MaxCount) | int value |
**SetFore** | Set text color | Color? value |
**SetFore** | Set text and sub text colors together | Color? value, Color? sub |
**SetForeSub** | Set sub text color | Color? value |
**SetBackActive** | Set active background color | Color? value |
**SetBackActive** | Set active background gradient color | string? value |
**SetTagFore** | Set tag text color | Color? value |
**SetTagBack** | Set tag background color | Color? value |
**SetTagBack** | Set tag background gradient color | string? value |

#### Example

```csharp
select.Items = new object[] {
	new AntdUI.SelectItem("Alice", 1)
		.SetIcon(SvgDb.GetSvg("user"))
		.SetOnline(1)
		.SetSubText("Online · Engineering")
		.SetTagBack("#E6F4FF")
		.SetFore("#1677FF"),
	new AntdUI.DividerSelectItem(),
	new AntdUI.SelectItem("Bob", 2)
};
```

## DividerSelectItem

> Divider (no properties, just add it into Items to separate options)
