[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Splitter
👚

> Split panels to isolate. Inherited from [SplitContainer](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Layout/Containers/SplitContainer.cs)

- DefaultProperty：Text
- DefaultEvent：SplitterMoved

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**SplitterBack** | Slide background | Color`?` | `null` |
||||
**ArrawBackHover** | Mouse hover arrow background color | Color`?` | `null` |
**ArrawColorHover** | Arrow hover color | Color`?` | `null` |
||||
**ArrowColor** | Arrow Color | Color`?` | `null` |
**Orientation** | Is the splitter horizontal or vertical | Orientation | Vertical |
**Panel1MinSize** | The minimum distance between the splitter and the left or upper edge of Panel1 | int | 25 |
**Panel2MinSize** | The minimum distance between the splitter and the right or lower edge of Panel2 | int | 25 |
**SplitPanelState** | Current folding state | bool | true |
**SplitterBackMove** | Slide to move background | Color`?` | `null` |
**SplitterDistance** | The distance between the splitter and the left or upper edge | int | |
**SplitterSize** | Slide size | int | 20 |
**SplitterWidth** | Thickness of the splitter | int | 4 |
||||
**ArrawBackColor** | Arrow background color | Color`?` | `null` |
**CollapsePanel** | Collapse after clicking Panel | ADCollapsePanel | None |
**Lazy** | Delayed rendering | bool | true |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Collapse** | Collapse | void | |
**Expand** | Expand | void | |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SplitPanelStateChanged** | Occurred when the value of the SplitPanelState property is changed | void | bool value |