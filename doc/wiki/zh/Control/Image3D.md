[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Image3D

Image3D 图片3D 👚

> 用于展示带有3D切换动画效果的图片控件。

- 默认属性：Image
- 默认事件：Click

### 属性

#### 外观属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Back** | 背景颜色 | Color | Transparent |
**Radius** | 圆角 | int | 0 |
**Round** | 圆角样式 | bool | false |
**Image** | 图片 | Image`?` | `null` |
**ImageFit** | 图片布局 | [TFit](Enum.md#tfit) | Cover |

#### 动画属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Vertical** | 是否竖向 | bool | false |
**Speed** | 速度 | int | 10 |
**Duration** | 持续时间(毫秒) | int | 400 |

#### 阴影属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Shadow** | 阴影大小 | int | 0 |
**ShadowColor** | 阴影颜色 | Color`?` | `null` |
**ShadowOpacity** | 阴影透明度 | float | 0.3F |
**ShadowOffsetX** | 阴影偏移X | int | 0 |
**ShadowOffsetY** | 阴影偏移Y | int | 0 |

#### 悬浮属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**EnableHover** | 启用悬浮交互 | bool | false |
**HoverFore** | 悬浮前景 | Color`?` | `null` |
**HoverBack** | 悬浮背景 | Color`?` | `null` |
**HoverImage** | 悬浮图标 | Image`?` | `null` |
**HoverImageSvg** | 悬浮图标SVG | string`?` | `null` |
**HoverImageRatio** | 悬浮图标比例 | float | 0.4F |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**RunAnimation** | 执行动画过渡 | void | Image`?` `切换到新图片` |

### 示例

```csharp
// 基本使用
image3D1.Image = Image.FromFile("test.jpg");

// 切换图片（带3D动画）
image3D1.RunAnimation(Image.FromFile("new.jpg"));

// 配置动画
image3D1.Vertical = true; // 竖向动画
image3D1.Speed = 15; // 动画速度
image3D1.Duration = 600; // 动画持续时间

// 配置外观
image3D1.Radius = 8; // 圆角
image3D1.Shadow = 4; // 阴影大小

// 配置悬浮效果
image3D1.EnableHover = true;
image3D1.HoverBack = Color.FromArgb(100, 0, 0, 0);
image3D1.HoverImageSvg = "<svg>...</svg>";
```