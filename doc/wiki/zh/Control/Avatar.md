[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Avatar

Avatar 头像 👚

> 用来代表用户或事物，支持图片、图标或字符展示。

- 默认属性：Image
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
||||
**BackColor** | 背景颜色 | Color`?` |`null` |
**BorderWidth** | 边框宽度 | float | 0F |
**BorderColor** | 边框颜色 | Color | 246, 248, 250 |
||||
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
||||
**Image** | 图片 | Image`?` | `null` |
**ImageSvg** | 图片SVG | string`?` | `null` |
**ImageFit** | 图片布局 | [TFit](Enum.md#tfit) | Cover |
**PlayGIF** | 播放GIF | bool | true |
||||
**Shadow** | 阴影大小 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOpacity** | 阴影透明度 | float | 0.3F |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |
||||
**Loading** 🔴 | 加载状态 | bool | false |
**LoadingProgress** 🔴 | 加载进度 `0F-1F` | float | 0F |