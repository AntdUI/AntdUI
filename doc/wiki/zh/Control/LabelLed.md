[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## LabelLed

LED文本控件 👚

> 显示一段LED样式的文本。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string ||
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
|||
**FontSize** | 字体大小 | int`?` | `null` |
**EmojiFont** | Emoji字体 | string`?` | `null` |
|||
**DotSize** | 点阵大小 | int | 4 |
**DotGap** | 点阵距离 | int | 2 |
**TextScale** | 文本大小比例 | float | 1F |
**DotShape** | 点阵形状 | [LedDotShape](#leddotshape) | Square |
|||
**DotColor** | 点阵颜色 | Color`?` | `null` |
**ShowOffLed** | 是否显示未发光LED | bool | false |
**OffDotColor** | 未发光LED颜色 | Color`?` | `null` |
|||
**Back** | 背景颜色 | Color`?` | `null` |
**BackExtend** | 背景颜色 | string`?` | `null` |
|||
**Shadow** | 阴影大小 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOpacity** | 阴影透明度 | float | 0.3F |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |

### 枚举

#### LedDotShape

| 值 | 描述 |
|:--|:--|
| Square | 方形 |
| Diamond | 菱形 |
| Circle | 圆形 |