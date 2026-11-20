[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Carousel
👚

> A set of carousel areas.

- DefaultProperty：Image
- DefaultEvent：SelectIndexChanged

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Image** | Image Collection `CarouselItemCollection` | [CarouselItemCollection](#carouselitem) | [] |
**ImageFit** | Image layout | [TFit](Enum.md#tfit) | Cover |
||||
**Autodelay** | Automatic switching delay(s) | int | 4 |
**Autoplay** | Automatic play | bool | false |
**SelectIndex** | Select index | int | 0 |
**DotFocusedColor** | Focused panel indicator dot color | Color`?` | `null` |
||||
**Radius** | Rounded corners | int | 0 |
**Round** | Rounded corner style | bool | false |
||||
**DotMargin** | Panel indicator dot margin | int | 12 |
**DotPosition** | Position of panel indicator dot | [TAlignMini](Enum.md#talignmini) | None |
**DotSize** | Panel indicator dot size | Size | 28 × 4 |
||||
**Touch** | Gesture sliding | bool | true |
**TouchOut** | Slide outside | bool | false |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndexChanged** | Occurred when the SelectIndex property value is changed | void | int index |

### Data

#### CarouselItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Tag** | User defined data | object`?` | `null` |
||||
**Img** | Image | Image`?` | `null` |