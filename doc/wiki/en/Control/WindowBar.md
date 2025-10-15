[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## WindowBar
👚

- DefaultProperty：Text
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value | 
:--|:--|:--|:--|
**Mode** | Color mode | [TAMode](Enum.md#tamode) | Auto |
**Loading** | Loading State | bool | false |
||||
**Text** | Text | string`?` | `null` |
**SubText** | Subtext | string`?` | `null` |
||||
**ShowIcon** | Display icon or not | bool | true |
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string | `null` |
||||
**MaximizeBox** | Do you want to display the maximize button | bool | true |
**MinimizeBox** | Whether to display the minimize button | bool | true |
**DragMove** | Can I drag the position | bool | true |
**CloseSize** | Close button size | int | 48 |
||||
**UseLeft** | Use pixels on the left side | int | 0 |
**UseSystemStyleColor** | Use system colors | bool | false |
**DividerMargin** | Line and margin at both ends | int | 0 |
||||
**DividerShow** | Display the dividing line at the bottom | bool | false |
**DividerColor** | Line color | Color`?` | `null` |
**DividerThickness** | Line thickness | float | 1F |
||||
**CancelButton** | Click to exit and close | bool | false |