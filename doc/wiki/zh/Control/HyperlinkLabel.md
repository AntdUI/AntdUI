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
**NormalStyle** | 正常状态链接样式 | LinkAppearance | `默认样式` |
**HoverStyle** | 悬停状态链接样式 | LinkAppearance | `默认样式` |
**LinkPadding** | 链接与周围字符的距离 | int | 2 |
**LinkAutoNavigation** | 是否自动打开链接 | bool | true |
**TextAlign** | 文本对齐方式 | ContentAlignment | TopLeft |
**Shadow** | 启用阴影效果 | bool | false |
**ShadowSize** | 阴影大小 | int | 2 |
**ShadowColor** | 阴影颜色 | Color | Color.FromArgb(255, 0, 0, 0) |
**ShadowOpacity** | 阴影透明度 | float | 0.2F |
**ShadowOffset** | 阴影偏移量 | Point | 1, 1 |

### LinkAppearance 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Color** | 链接颜色 | Color | Color.FromArgb(255, 10, 76, 178) |
**HoverColor** | 悬停颜色 | Color | Color.FromArgb(255, 79, 126, 194) |
**FontStyle** | 字体样式 | FontStyle | FontStyle.Regular |
**Underline** | 下划线 | bool | true |
**HoverUnderline** | 悬停下划线 | bool | true |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**LinkClicked** | 当点击链接时发生 | void | string `href`, string `text` |

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