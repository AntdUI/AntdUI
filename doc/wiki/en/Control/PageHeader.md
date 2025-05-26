[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## PageHeader
👚

> A header with common actions and design elements built in.

- DefaultProperty：Text
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Mode** | Color mode | [TAMode](Enum.md#tamode) | Auto |
**Loading** | Loading State | bool | false |
**BackExtend** | Background gradient color | string`?` | `null` |
||||
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**SubText** | Subtext | string`?` | `null` |
🌏 **LocalizationSubText** | International Subtext | string`?` | `null` |
**Description** | Description text | string`?` | `null` |
🌏 **LocalizationDescription** | International Description text | string`?` | `null` |
**UseTitleFont** | Use title font size | bool | false |
**UseTextBold** | Title in bold | bool | true |
**UseSubCenter** 🔴 | Subtext centered | bool | false |
**UseLeftMargin** 🔴 | Use left margin | bool | true |
**SubFont** | Subtext font | Font`Font` | `null` |
||||
**Gap** | Gap | int`?` | `null` |
**SubGap** | Subtitle and title spacing | int | 6 |
||||
**ShowIcon** | Display icon or not | bool | false |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string | `null` |
**IconRatio** | Icon Scale | float`?` | `null` |
||||
**ShowBack** | Whether to display the return button | bool | false |
**ShowButton** | Display title bar button or not | bool | false |
**MaximizeBox** | Do you want to display the maximize button | bool | true |
**MinimizeBox** | Whether to display the minimize button | bool | true |
**FullBox** | Whether to display the full screen button | bool | false |
**DragMove** | Can I drag the position | bool | true |
**CloseSize** | Close button size | int | 48 |
**MDI** 🔴 | Is it only effective for the parent window | bool | false |
||||
**UseSystemStyleColor** | Use system colors | bool | false |
**CancelButton** | Click to exit and close | bool | false |
||||
**DividerShow** | Display the dividing line at the bottom | bool | false |
**DividerColor** | Line color | Color`?` | `null` |
**DividerThickness** | Line thickness | float | 1F |
**DividerMargin** | Line and margin at both ends | int | 0 |