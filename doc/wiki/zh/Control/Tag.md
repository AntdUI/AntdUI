[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Tag

Tag 标签页 👚

> 进行标记和分类的小标签。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
||||
**AutoSize** | 自动大小 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
||||
**BackgroundImage** | 背景图片 | Image`?` | `null` |
**BackgroundImageLayout** | 背景图片布局 | [TFit](Enum.md#tfit) | Fill |
||||
**BorderWidth** | 边框宽度 | float | 0F |
||||
**Radius** | 圆角 | int | 6 |
**Type** | 类型 | [TTypeMini](Enum.md#ttypemini) | Default |
**CloseIcon** | 是否显示关闭图标 | bool | false |
||||
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextAlign** | 文本位置 | ContentAlignment | MiddleCenter |
**AutoEllipsis** | 文本超出自动处理 | bool | false |
**TextMultiLine** | 是否多行 | bool | false |
||||
**Image** | 图像 | Image`?` | `null` |
**ImageSvg** | 图像SVG | string`?` | `null` |
**ImageSize** | 图像大小 `不设置为自动大小` | Size | 0 × 0 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**CloseChanged** | Close时发生 | bool ||