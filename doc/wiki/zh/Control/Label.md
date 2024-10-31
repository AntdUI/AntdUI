[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Label

Label 文本 👚

> 显示一段文本。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ColorExtend** 🔴 | 文字渐变色 | string`?` | `null` |
||||
**Text** | 文本 | string ||
**TextAlign** | 文本位置 | ContentAlignment | MiddleLeft |
**AutoEllipsis** | 文本超出自动处理 | bool | false |
**TextMultiLine** | 是否多行 | bool | true |
||||
**IconRatio** 🔴 | 图标比例 | float | 0.7F |
**Prefix** | 前缀 | string`?` | `null` |
**PrefixSvg** 🔴 | 前缀SVG | string`?` | `null` |
**PrefixColor** | 前缀颜色 | Color`?` | `null` |
**Suffix** | 后缀文本 | string`?` | `null` |
**SuffixSvg** 🔴 | 后缀SVG | string`?` | `null` |
**SuffixColor** | 后缀颜色 | Color`?` | `null` |
**Highlight** | 缀标完全展示 | bool | true |
**ShowTooltip** 🔴 | 超出文字显示 Tooltip | bool | true |
||||
**Shadow** | 阴影大小 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOpacity** | 阴影透明度 | float | 0.3F |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |