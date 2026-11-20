[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## PageHeader
👚

> A header with common actions and design elements built in.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**SubFont** | Subtext font | Font`?` | `null` |
**UseForeColorDrawIcons** | Use foreground color to draw button icons | bool | false |
**UseTitleFont** | Use title font size | bool | false |
||||
**BackExtend** | Background gradient color | string`?` | `null` |
**BackIcon** | Return icon | string`?` | `null` |
**ShowBack** | Whether to display the return button | bool | false |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconRatio** | Icon Scale | float`?` | `null` |
**LoadingIcon** | Loading icon | Image`?` | `null` |
**ShowIcon** | Display icon or not | bool | false |
||||
**Loading** | Loading State | bool | false |
**LoadingSpeedTime** | Loading speed (ms) | int | 1000 |
**LoadingSvg** | Loading icon SVG | string`?` | `null` |
||||
**CancelButton** | Click to exit and close | bool | false |
**CloseSize** | Close button size | int | 48 |
**DividerShow** | Display the dividing line at the bottom | bool | false |
**DragMove** | Can I drag the position | bool | true |
**EnableButtonTooltip** | Button hover tooltip | bool | false |
**EnableDoubleClickMaximize** | Allow double-click maximization | bool | true |
**Mode** | Color mode | [TAMode](Enum.md#tamode) | Auto | `obsolete, use ColorScheme` |
**ShowButton** | Display title bar button or not | bool | false |
**UseLeftMargin** | Use left margin | bool | true |
**UseSubCenter** | Subtext centered | bool | false |
**UseSystemStyleColor** | Use system colors | bool | false |
**UseTextBold** | Title in bold | bool | true |
||||
**DividerMargin** | Line and margin at both ends | int | 0 |
**Gap** | Gap | int`?` | `null` |
**IsMax** | Whether maximized | bool | false |
**MaximizeBox** | Do you want to display the maximize button | bool | true |
**MinimizeBox** | Whether to display the minimize button | bool | true |
**SubGap** | Subtitle and title spacing | int | 6 |
||||
**Description** | Description text | string`?` | `null` |
🌏 **LocalizationDescription** | International Description text | string`?` | `null` |
**DividerColor** | Line color | Color`?` | `null` |
**DividerThickness** | Line thickness | float | 1F |
**FullBox** | Whether to display the full screen button | bool | false |
**IsFull** | Whether fullscreen | bool | false |
**MDI** | Is it only effective for the parent window | bool | false |
**SubText** | Subtext | string`?` | `null` |
🌏 **LocalizationSubText** | International Subtext | string`?` | `null` |