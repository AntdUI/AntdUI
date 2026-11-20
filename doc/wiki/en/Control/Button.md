[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Button
👚

> To trigger an operation.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
**Type** | Type | [TTypeMini](Enum.md#ttypemini) | Default |
**TextAlign** | Text position | ContentAlignment | MiddleCenter |
**TextMultiLine** | Multiple lines | bool | false |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ForeHover** | Hover text color | Color`?` | `null` |
**ForeActive** | Active text color | Color`?` | `null` |
||||
**BackColor** | Background color | Color`?` | `null` |
**BackExtend** | Background gradient color | string`?` | `null` |
**BackgroundImage** | Background image | Image`?` | `null` |
**BackgroundImageLayout** | Background image layout | [TFit](Enum.md#tfit) | Fill |
**DefaultBack** | Default type background color | Color`?` | `null` |
**OriginalBackColor** | Original background color | Color | Transparent |
**BackHover** | Hover background color | Color`?` | `null` |
**BackActive** | Active background color | Color`?` | `null` |
||||
**Icon** | Icon | Image`?` | `null` |
**IconSvg** | Icon SVG | string`?` | `null` |
**IconGap** | Ratio of icon to text spacing | float | 0.25F |
**IconPosition** | Location of button icon components | [TAlignMini](Enum.md#talignmini) | Left |
**IconRatio** | Icon Scale | float | 0.7F |
**IconSize** | Icon size `Default automatic size` | Size | 0 × 0 |
**TextCenterHasIcon** | Text centered display(After including icons) | bool | false |
**IconHover** | Hover icon | Image`?` | `null` |
**IconHoverAnimation** | Hover icon animation duration | int | 200 |
**IconHoverSvg** | Hover icon SVG | string`?` | `null` |
||||
**WaveColor** | Wave color | Color`?` | `null` |
**WaveShadow** | Wave shadow | bool | false |
**WaveShadowSize** | Wave shadow size | int`?` | `null` |
**WaveSize** | Wave size `Click animation` | int | 4 |
**WaveColorExtend** | Wave gradient color | string`?` | `null` |
||||
**AutoSize** | Auto Size | bool | false |
**AutoEllipsis** | Text exceeds automatic processing | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**DisplayStyle** | Specify to display image or text | [TButtonDisplayStyle](Enum.md#tbuttondisplaystyle) | Default |
**Ghost** | Ghost attribute `Transparent button background` | bool | false |
**IsLink** | Arrow Link Style | bool | false |
**RespondRealAreas** | Response button real area | bool | false |
**ShowArrow** | Display arrows | bool | false |
**UseMnemonic** | Mnemonic key `The first character with (&) will be used as the mnemonic key` | bool | true |
||||
**BorderWidth** | Border width | float | 0F |
**Radius** | Rounded corners | int | 6 |
**Shape** | Shape | [TShape](Enum.md#tshape) | Default |
**DefaultBorderColor** | Default type border color | Color`?` | `null` |
||||
**ArrowProg** | Arrow angle | float | -1F |
**VirtualWidth** | Virtual width | int`?` | `null` |
||||
|**Switch**||
**AutoToggle** | Auto toggle state on click | bool | false |
**Toggle** | Toggle state | bool | false |
**ToggleText** | Toggle text | string`?` | `null` |
🌏 **LocalizationToggleText** | Toggle text | string`?` | `null` |
**ToggleIcon** | Toggle icon | Image`?` | `null` |
**ToggleIconSvg** | Toggle icon SVG | string`?` | `null` |
**ToggleIconHover** | Toggle Hover icon | Image`?` | `null` |
**ToggleIconHoverSvg** | Toggle Hover icon SVG | string`?` | `null` |
**IconToggleAnimation** | Icon toggling animation duration | int | 200 |
**ToggleFore** | Toggle text color | Color`?` | `null` |
**ToggleForeHover** | Toggle hover text color | Color`?` | `null` |
**ToggleForeActive** | Toggle active text color | Color`?` | `null` |
**ToggleType** | Toggle type | [TTypeMini](Enum.md#ttypemini)`?` | `null` |
**ToggleBack** | Toggle background color | Color`?` | `null` |
**ToggleBackExtend** | Toggle background gradient color | string`?` | `null` |
**ToggleBackHover** | Toggle hover background color | Color`?` | `null` |
**ToggleBackActive** | Toggle to activate background color | Color`?` | `null` |
||||
|**Loading**||
**Loading** | Loading State `true Block clicks` | bool | false |
**LoadingSpeedTime** | Loading speed (ms) | int | 1000 |
**LoadingIcon** | Loading icon | Image`?` | `null` |
**LoadingSvg** | Loading icon SVG | string`?` | `null` |
**LoadingRespondClick** | Loading respond click | bool | false |
**LoadingValue** | Loading progress `0F-1F` | float | 0.3F |
**LoadingWaveValue** | Water wave progress `0F-1F` | float | 0F |
**LoadingWaveColor** | Water wave color | Color`?` | `null` |
**LoadingWaveVertical** | Is the water wave vertical | bool | false |
**LoadingWaveSize** | Water wave size | int | 2 |
**LoadingWaveCount** | Number of water waves | int | 1 |
||||
|**State**||
**ExtraMouseDown** | Active state | bool | false |
**ExtraMouseHover** | Hover state | bool | false |
**HasFocus** | Whether focus exists | bool | false |
||||
|**Combination**||
**JoinMode** | Combination mode | [TJoinMode](Enum.md#tjoinmode) | None |
**JoinLeft** | Connect left area | bool | false |
**JoinRight** | Connect right area | bool | false |

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**AnimationBlink** | Start blinking animation | void | int interval 间隔(毫秒), params Color[] colors 闪烁颜色 |
**AnimationBlinkTransition** | Start blinking animation (color transition) | void | int interval 间隔(毫秒), params Color[] colors 闪烁颜色 |
**StopAnimationBlink** | Stop blinking animation | void | |

### Events

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ToggleChanged** | Occurs when Toggle changes | void | |