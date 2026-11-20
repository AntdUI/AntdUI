[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## WindowBar
👚

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value | 
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string | `null` |
**ShowIcon** | Display icon or not | bool | true |
||||
**Loading** | Loading State | bool | false |
||||
**CancelButton** | Click to exit and close | bool | false |
**CloseSize** | Close button size | int | 48 |
**DividerShow** | Display the dividing line at the bottom | bool | false |
**DragMove** | Can I drag the position | bool | true |
**Mode** | Color mode | [TAMode](Enum.md#tamode) | Auto |
**UseLeft** | Use pixels on the left side | int | 0 |
**UseSystemStyleColor** | Use system colors | bool | false |
||||
**DividerMargin** | Line and margin at both ends | int | 0 |
**MaximizeBox** | Do you want to display the maximize button | bool | true |
**MinimizeBox** | Whether to display the minimize button | bool | true |
||||
**DividerColor** | Line color | Color`?` | `null` |
**DividerThickness** | Line thickness | float | 1F |
**SubText** | Subtext | string`?` | `null` |