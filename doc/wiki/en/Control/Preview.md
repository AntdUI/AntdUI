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
**Form** | Belonging window | Form | `Required` |
**Content** | Content | `IList<Image>` / `IList<object>` | `Required` |
**ContentCount** | Content count | int | `Required` |
**SelectIndex** | Current selected index | int | 0 |
**Fit** | Image fit | [TFit](Enum.md#tfit)`?` | `null` |
**Keyboard** | Support keyboard esc to close | bool | true |
**Tag** | User defined data | object`?` | `null` |
**OnSelectIndexChanged** | SelectIndex changed callback | Func<int, bool>`?` | `null` |
||||
**ShowBtn** | Show button or not | bool | true |
**ShowDefaultBtn** | Show default button or not | bool | true |
**BtnSize** | Button size | Size | 42, 46 |
**BtnIconSize** | Button icon size | int | 18 |
**BtnLRSize** | Left/right button size | int | 40 |
**ContainerPadding** | Container padding | int | 24 |
**BtnPadding** | Button padding | Size | 12, 32 |
**Btns** | Custom button | [Btn[]](#btn) | `null` |
**OnBtns** | Custom button callback | Action<string, BtnEvent>`?` | `null` |

### Btn

> Custom button

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Name** | Button name | string | `Required` |
**IconSvg** | Icon SVG | string | `Required` |
**Tag** | User defined data | object`?` | `null` |

### BtnEvent

> Button event parameters

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Index** | Data index | int | |
**Data** | Metadata | object`?` | |
**Tag** | Btn's Tag | object`?` | |