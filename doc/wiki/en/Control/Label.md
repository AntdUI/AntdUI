[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Label
👚

> Display a piece of text.

- DefaultProperty：Text
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**AutoSize** | Auto Size | bool | false |
**AutoSizeMode** | Auto size mode | [TAutoSize](Enum.md#tautosize) | None |
||||
**ForeColor** | Text color | Color`?` | `null` |
**ColorExtend** | Text gradient color | string`?` | `null` |
||||
**Text** | Text | string ||
🌏 **LocalizationText** | International Text | string`?` | `null` |
**TextAlign** | Text position | ContentAlignment | MiddleLeft |
**AutoEllipsis** | Text exceeds automatic processing | bool | false |
**TextMultiLine** | Multiple lines | bool | true |
||||
**IconRatio** | Icon Scale | float | 0.7F |
**Prefix** | Prefix text | string`?` | `null` |
🌏 **LocalizationPrefix** | International Prefix | string`?` | `null` |
**PrefixSvg** | Prefix SVG | string`?` | `null` |
**PrefixColor** | Prefix color | Color`?` | `null` |
**Suffix** | Suffix text | string`?` | `null` |
🌏 **LocalizationSuffix** | International Suffix | string`?` | `null` |
**SuffixSvg** | Suffix SVG | string`?` | `null` |
**SuffixColor** | Suffix color | Color`?` | `null` |
**Highlight** | Full display of tags | bool | true |
**ShowTooltip** | Exceeding text display Tooltip | bool | true |
**Rotate** 🔴 | Rotate | [TRotate](Enum.md#trotate) | None |
||||
**Shadow** | Shadow size | int | 0 |
**ShadowColor** | Shadow color | Color`?` | `null` |
**ShadowOpacity** | Shadow Transparency | float | 0.3F |
**ShadowOffsetX** | Shadow offset X | int | 0 |
**ShadowOffsetY** | Shadow offset Y | int | 0 |