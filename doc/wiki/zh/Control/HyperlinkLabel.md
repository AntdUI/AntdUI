[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## HyperlinkLabel

HyperlinkLabel 超链接文本 👚

> 显示带有超链接的文本，支持自定义样式和事件处理。

- 默认属性：Text
- 默认事件：LinkClicked

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本内容，支持 `<a href="...">...</a>` 语法 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextAlign** | 文本位置 | ContentAlignment | MiddleLeft |
**Shadow** | 阴影大小 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOpacity** | 阴影透明度 | float | 0.3F |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |
**NormalStyle** | 常规状态下链接的样式 | LinkAppearance`?` | `null` |
**HoverStyle** | 鼠标悬停时链接的样式 | LinkAppearance`?` | `null` |
**LinkPadding** | 链接与周围字符之间的距离 | Padding | 2, 0, 2, 0 |
**LinkAutoNavigation** | 自动调用默认浏览器打开超链接 | bool | false |
**AutoSize** | 自动大小 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
**ForeColor** | 文字颜色 | Color`?` | `null` |

### LinkAppearance 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**LinkColor** | 链接呈现的文本颜色 | Color`?` | `null` |
**LinkStyle** | 链接的字体样式 | FontStyle | Regular |
**UnderlineColor** | 下划线颜色 | Color`?` | `null` |
**UnderlineThickness** | 下划线厚度（0为不显示下划线） | int | 1 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**LinkClicked** | 当点击链接时发生 | void | LinkClickedEventArgs `e` |

### 示例

```csharp
// 基本使用
hyperlinkLabel1.Text = "访问 <a href='https://ant.design'>Ant Design</a> 官网";

// 自定义样式
hyperlinkLabel1.NormalStyle.Underline = false;
hyperlinkLabel1.HoverStyle.Color = Color.Red;
hyperlinkLabel1.HoverStyle.Underline = true;

// 禁用自动导航
hyperlinkLabel1.LinkAutoNavigation = false;
```