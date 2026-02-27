[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Watermark

Watermark 水印 👚

> 为页面或控件添加水印，支持文字和图片水印。

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Opacity** | 水印透明度 | float | 0.15F |
**Rotate** | 旋转角度 | int | -22 |
**Width** | 水印宽度 | int | 200 |
**Height** | 水印高度 | int | 100 |
**Gap** | 水印间距 [x, y] | int[] | [100, 100] |
**Offset** | 水印偏移量 [x, y] | int[] | [50, 50] |
**IsScreen** | 屏幕水印 | bool | false |
**SubFontSize** | 副内容字体大小比例 | float | 0.8F |
**Font** | 字体 | Font`?` | `null` |
**ForeColor** | 字体颜色 | Color`?` | `null` |
**TextAlign** | 文本对齐方式 | FormatFlags | Center |
**Enabled** | 是否启用 | bool | true |
**Content** | 水印内容 | string | `null` |
**SubContent** | 副水印内容 | string`?` | `null` |
**Image** | 水印图片 | Image`?` | `null` |
**ImageSvg** | 水印图片SVG | string`?` | `null` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**open** | 打开水印 | Form`?` | Control `目标控件`, string `主内容`, string `副内容` |
**open** | 打开水印 | Form`?` | Config `水印配置` |

### 示例

```csharp
// 简化版使用
Watermark.open(this, "AntdUI Watermark", "2024-01-01");

// 完整配置
var config = new Watermark.Config(this, "AntdUI")
	.SetSub("Powered by AntdUI")
	.SetOpacity(0.2f)
	.SetRotate(-30)
	.SetGap(150, 150);
Watermark.open(config);
```