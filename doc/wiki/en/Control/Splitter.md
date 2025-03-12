[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Splitter
👚

> Split panels to isolate. Inherited from [SplitContainer](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Layout/Containers/SplitContainer.cs)

- DefaultProperty：Text
- DefaultEvent：SplitterMoved

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**SplitterBack** | Slide background | Color`?` | `null` |
**SplitterBackMove** | Slide to move background | Color`?` | `null` |
**ArrowColor** | Arrow Color | Color`?` | `null` |
**ArrawColorHover** | Arrow hover color | Color`?` | `null` |
**ArrawBackColor** | Arrow background color | Color`?` | `null` |
||||
**SplitterWidth** | Thickness of the splitter | int | 4 |
**SplitterSize** | Slide size | int | 20 |
**SplitterDistance** | The distance between the splitter and the left or upper edge | int | |
**Panel1MinSize** | The minimum distance between the splitter and the left or upper edge of Panel1 | int | 25 |
**Panel2MinSize** | The minimum distance between the splitter and the right or lower edge of Panel2 | int | 25 |
||||
**CollapsePanel** | Collapse after clicking Panel | ADCollapsePanel | None |
**Orientation** | Is the splitter horizontal or vertical | Orientation | Vertical |
**SplitPanelState** | Current folding state | bool | true |
**Lazy** | Delayed rendering | bool | true |

### Method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Collapse** | Collapse | void | |
**Expand** | Expand | void | |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SplitPanelStateChanged** | Occurred when the value of the SplitPanelState property is changed | void | bool value |