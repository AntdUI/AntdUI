[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## HyperlinkCheckbox

HyperlinkCheckbox 超链接多选框 👚

> 显示带有超链接的多选框，支持自定义样式和事件处理。

- 默认属性：Checked
- 默认事件：CheckedChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本内容，支持 `<a href="...">...</a>` 语法 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextAlign** | 文本位置 | ContentAlignment | MiddleLeft |
**NormalStyle** | 常规状态下链接的样式 | LinkAppearance`?` | `null` |
**HoverStyle** | 鼠标悬停时链接的样式 | LinkAppearance`?` | `null` |
**LinkPadding** | 链接与周围字符之间的距离 | Padding | 2, 0, 2, 0 |
**LinkAutoNavigation** | 自动调用默认浏览器打开超链接 | bool | false |
**AutoSize** | 自动大小 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
**ForeColor** | 文字颜色 | Color`?` | `null` |
**Checked** | 选中状态 | bool | false |
**CheckAlign** | 复选框位置 | ContentAlignment | MiddleLeft |
**ThreeState** | 三态模式 | bool | false |
**CheckState** | 复选框状态 | CheckState | Unchecked |

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
**CheckedChanged** | 当选中状态改变时发生 | void | EventArgs `e` |

### 示例

```csharp
// 基本使用
hyperlinkCheckbox1.Text = "我同意 <a href='https://example.com/terms'>服务条款</a>";

// 自定义链接样式
hyperlinkCheckbox1.NormalStyle.LinkColor = Color.Blue;
hyperlinkCheckbox1.HoverStyle.LinkColor = Color.Red;
hyperlinkCheckbox1.HoverStyle.Underline = true;

// 启用自动导航
hyperlinkCheckbox1.LinkAutoNavigation = true;

// 处理链接点击事件
hyperlinkCheckbox1.LinkClicked += (sender, e) =>
{
	MessageBox.Show($"点击了链接: {e.Text}\nURL: {e.Href}");
};
```