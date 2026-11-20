[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Avatar

Avatar 头像 👚

> 用来代表用户或事物，支持图片、图标或字符展示。

- 默认属性：Image
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**HoverFore** | 悬浮前景 | Color`?` | `null` |
||||
**BackColor** | 背景颜色 | Color | Transparent |
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
**HoverBack** | 悬浮背景 | Color`?` | `null` |
||||
**Image** | 图片 | Image`?` | `null` |
**ImageFit** | 图片布局 | [TFit](Enum.md#tfit) | Cover |
**ImageSvg** | 图片SVG | string`?` | `null` |
||||
**Loading** | 加载状态 | bool | false |
**LoadingProgress** | 加载进度 `0F-1F` | float | 0F |
||||
**PlayGIF** | 播放GIF | bool | true |
**EnableHover** | 启用悬浮交互 | bool | false |
**HoverImage** | 悬浮图标 | Image`?` | `null` |
**HoverImageRatio** | 悬浮图标比例 | float | 0.4F |
**HoverImageSvg** | 悬浮图标SVG | string`?` | `null` |
||||
**BorderColor** | 边框颜色 | Color | 246, 248, 250 |
**BorderWidth** | 边框宽度 | float | 0F |
**Radius** | 圆角 | int | 0 |
**Round** | 圆角样式 | bool | false |
||||
**Shadow** | 阴影大小 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |
**ShadowOpacity** | 阴影透明度 | float | 0.3F |
||||
**Square** | 正方形约束 | bool | false |