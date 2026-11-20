[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Preview

> Picture preview box.

### Usage

```csharp
// Single image
Preview.open(new Preview.Config(this, image));

// Multiple images
Preview.open(new Preview.Config(this, new Image[] { image1, image2 }));

// Images with callback
Preview.open(new Preview.Config(this, list, (index, item) => GetImage(item)));
```

### Preview.Config

> Configure Preview

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Tag** | User defined data | object`?` | `null` |
**Content** | Content | `IList<Image>` / `IList<object>` | `Required` |
||||
**BtnIconSize** | Button icon size | int | 18 |
||||
**Loop** | Whether to loop when switching (takes effect with more than one image) | bool | false |
**MaskClosable** | Whether clicking the blank area closes the preview | bool | true |
**OnSelectIndexChanged** | SelectIndex changed callback | Func<int, bool>`?` | `null` |
**SelectIndex** | Current selected index | int | 0 |
**ShowBtn** | Show button or not | bool | true |
**ShowDefaultBtn** | Show default button or not | bool | true |
||||
**BtnLRSize** | Left/right button size | int | 40 |
**BtnPadding** | Button padding | Size | 12, 32 |
**BtnSize** | Button size | Size | 42, 46 |
**ContainerPadding** | Container padding | int | 24 |
||||
**Btns** | Custom button | [Btn[]](#btn) | `null` |
**ContentCount** | Content count | int | `Required` |
**Fit** | Image fit | [TFit](Enum.md#tfit)`?` | `null` |
**Form** | Belonging window | Form | `Required` |
**Keyboard** | Support keyboard esc to close | bool | true |
**OnBtns** | Custom button callback | Action<string, BtnEvent>`?` | `null` |

### Btn

> Custom button

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Button name | string | `Required` |
**Tag** | User defined data | object`?` | `null` |
||||
**IconSvg** | Icon SVG | string | `Required` |

### BtnEvent

> Button event parameters

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Tag** | Btn's Tag | object`?` | |
||||
**Data** | Metadata | object`?` | |
**Index** | Data index | int | |

### ImageTextContent

> Image with text content

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Display text | string`?` | `null` |
**TextAlign** | Text alignment | ContentAlignment | TopCenter |
**TextPadding** | Text padding | Padding | 0 |
||||
**Font** | Text font | Font`?` | `null` |
**ForeColor** | Text color | Color`?` | `null` |
||||
**Image** | Image | Image | `Required` |