[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Label

Label 文本 👚

> 显示一段文本。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string ||
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextAlign** | 文本位置 | ContentAlignment | MiddleLeft |
**TextMultiLine** | 是否多行 | bool | true |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ColorExtend** | 文字渐变色 | string`?` | `null` |
||||
**IconGap** | 图标与文本间隔 | int | 0 |
**IconRatio** | 图标比例 | float | 0.7F |
**PrefixIcon** | 前缀图标 | Image`?` | `null` |
**SuffixIcon** | 后缀图标 | Image`?` | `null` |
||||
**AutoSize** | 自动大小 | bool | false |
**AutoEllipsis** | 文本超出自动处理 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
**AutoSizePadding** | 自动大小填充边距（仅 MiddleCenter） | bool | false |
**ShowTooltip** | 超出文字显示 Tooltip | bool | true |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度（需设置边框颜色） | float | 0F |
**Radius** | 圆角 | int | 0 |
**BorderAlign** | 边框方向 | [TAlignBorder](Enum.md#talignborder) | ALL |
**BorderTextGap** | 边框距文本区域距离（null 按控件区域渲染） | int`?` | `null` |
||||
**Shadow** | 阴影大小 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |
**ShadowOpacity** | 阴影透明度 | float | 0.3F |
||||
**Rotate** | 旋转 | [TRotate](Enum.md#trotate) | None |
||||
**Highlight** | 缀标完全展示 | bool | true |
**Prefix** | 前缀文本 | string`?` | `null` |
🌏 **LocalizationPrefix** | 国际化前缀 | string`?` | `null` |
**PrefixColor** | 前缀颜色 | Color`?` | `null` |
**PrefixColour** | 前缀颜色枚举 | Colour`?` | `null` |
**PrefixSvg** | 前缀SVG | string`?` | `null` |
**Suffix** | 后缀文本 | string`?` | `null` |
🌏 **LocalizationSuffix** | 国际化后缀 | string`?` | `null` |
**SuffixColor** | 后缀颜色 | Color`?` | `null` |
**SuffixColour** | 后缀颜色枚举 | Colour`?` | `null` |
**SuffixSvg** | 后缀SVG | string`?` | `null` |
**TooltipConfig** | 超出文字提示配置 | TooltipConfig`?` | `null` |