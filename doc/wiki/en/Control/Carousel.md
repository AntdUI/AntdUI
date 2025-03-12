[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Carousel
👚

> A set of carousel areas.

- DefaultProperty：Image
- DefaultEvent：SelectIndexChanged

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Touch** | Gesture sliding | bool | true |
**TouchOut** | Slide outside | bool | false |
**Autoplay** | Automatic play | bool | false |
**Autodelay** | Automatic switching delay(s) | int | 4 |
||||
**DotSize** | Panel indicator dot size | Size | 28 × 4 |
**DotMargin** | Panel indicator dot margin | int | 12 |
**DotPosition** | Position of panel indicator dot | [TAlignMini](Enum.md#talignmini) | None |
||||
**Radius** | Rounded corners | int | 0 |
**Round** | Rounded corner style | bool | false |
||||
**Image** | Image Collection `CarouselItem[]` | [CarouselItem[]](#carouselitem) | [] |
**ImageFit** | Image layout | [TFit](Enum.md#tfit) | Cover |
**SelectIndex** | Select index | int | 0 |

### Event

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SelectIndexChanged** | Occurred when the SelectIndex property value is changed | void | int index |

### Data

#### CarouselItem

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Img** | Image | Image`?` | `null` |
**Tag** | User defined data | object`?` | `null` |