[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Label
👚

> Display a piece of text.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string ||
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text position | ContentAlignment | MiddleLeft |
**TextMultiLine** | Multiple lines | bool | true |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ColorExtend** | Text gradient color | string`?` | `null` |
||||
**IconGap** | Icon and text gap | int | 0 |
**IconRatio** | Icon Scale | float | 0.7F |
**PrefixIcon** | Prefix icon | Image`?` | `null` |
**SuffixIcon** | Suffix icon | Image`?` | `null` |
||||
**AutoSize** | Auto Size | bool | false |
**AutoEllipsis** | Text exceeds automatic processing | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
**AutoSizePadding** | Auto size padding (only MiddleCenter) | bool | false |
**ShowTooltip** | Exceeding text display Tooltip | bool | true |
||||
**BorderColor** | Border color | Color`?` | `null` |
**BorderWidth** | Border width (border color must be set) | float | 0F |
**Radius** | Rounded corners | int | 0 |
**BorderAlign** | Border alignment | [TAlignBorder](Enum.md#talignborder) | ALL |
**BorderTextGap** | Distance from border to text area (null renders by control area) | int`?` | `null` |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |
**ShadowOpacity** | Shadow Transparency | float | 0.3F |
||||
**Rotate** | Rotate | [TRotate](Enum.md#trotate) | None |
||||
**Highlight** | Full display of tags | bool | true |
**Prefix** | Prefix text | string`?` | `null` |
🌏 **LocalizationPrefix** | International Prefix | string`?` | `null` |
**PrefixColor** | Prefix color | Color`?` | `null` |
**PrefixColour** | Prefix color enum | Colour`?` | `null` |
**PrefixSvg** | Prefix SVG | string`?` | `null` |
**Suffix** | Suffix text | string`?` | `null` |
🌏 **LocalizationSuffix** | International Suffix | string`?` | `null` |
**SuffixColor** | Suffix color | Color`?` | `null` |
**SuffixColour** | Suffix color enum | Colour`?` | `null` |
**SuffixSvg** | Suffix SVG | string`?` | `null` |
**TooltipConfig** | Text overflow tooltip configuration | TooltipConfig`?` | `null` |