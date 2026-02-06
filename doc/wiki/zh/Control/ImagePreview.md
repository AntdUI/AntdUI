[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## ImagePreview

ImagePreview 图片预览 👚

> 用于常驻显示和预览图片，支持多种交互操作和自定义配置。

- 默认属性：SelectIndex
- 默认事件：SelectIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Image** | 图片集合 | ImagePreviewItemCollection | `新集合` |
**SelectIndex** | 当前选中的图片索引 | int | 0 |
**Fit** | 图片适应方式 | [TFit](Enum.md#tfit) | Contain |
**ShowBtn** | 是否显示按钮 | bool | true |
**ShowDefaultBtn** | 是否显示默认按钮 | bool | true |
**BtnSize** | 按钮大小 | Size | `42, 46` |
**BtnIconSize** | 按钮图标大小 | int | 18 |
**BtnLRSize** | 左右按钮大小 | int | 40 |
**ContainerPadding** | 容器边距 | int | 24 |
**BtnPadding** | 按钮边距 | Size | `12, 32` |
**CustomButton** | 自定义按钮集合 | ImagePreviewButtonCollection | `新集合` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**LoadImg** | 加载图片 | void | bool `是否渲染` |
**ZoomToControl** | 缩放图片以适应控件 | void |  |
**FlipY** | 垂直翻转图片 | void |  |
**FlipX** | 水平翻转图片 | void |  |
**RotateL** | 向左旋转图片 | void |  |
**RotateR** | 向右旋转图片 | void |  |
**ZoomOut** | 缩小图片 | void |  |
**ZoomIn** | 放大图片 | void |  |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectIndexChanged** | 当选择的图片索引改变时触发 | void | int `索引` |
**ButtonClick** | 当点击按钮时触发 | void | ImagePreviewItem `项`, string `按钮ID`, object`?` `标签` |

### 数据

#### ImagePreviewItem

> 图片项

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Img** | 图片 | Image`?` | `null` |
**Call** | 异步加载图片的委托 | Func<int, ImagePreviewItem, Image>`?` | `null` |
**CallProg** | 带进度的异步加载委托 | Func<int, ImagePreviewItem, Action<float, string?>, Image>`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |

#### ImagePreviewButton

> 自定义按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 按钮名称 | string | `null` |
**IconSvg** | 图标SVG | string | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |

### 示例

```csharp
// 基本使用
var item = new ImagePreviewItem().SetImage(Image.FromFile("test.jpg"));
imagePreview1.Image.Add(item);

// 添加多张图片
imagePreview1.Image.Add(new ImagePreviewItem().SetImage(Image.FromFile("img1.jpg")));
imagePreview1.Image.Add(new ImagePreviewItem().SetImage(Image.FromFile("img2.jpg")));
imagePreview1.Image.Add(new ImagePreviewItem().SetImage(Image.FromFile("img3.jpg")));

// 异步加载图片
imagePreview1.Image.Add(new ImagePreviewItem().SetImage((index, item) => {
    // 模拟异步加载
    Thread.Sleep(1000);
    return Image.FromFile("async.jpg");
}));

// 带进度的异步加载
imagePreview1.Image.Add(new ImagePreviewItem().SetImage((index, item, progress) => {
    // 模拟下载进度
    for (int i = 0; i <= 100; i += 10) {
        Thread.Sleep(100);
        // 更新进度
        progress(i / 100f, $"加载中 {i}%");
    }
    // 加载完成
    return Image.FromFile("prog.jpg");
}));

// 添加自定义按钮
var customBtn = new ImagePreviewButton().SetName("custom").SetIcon("<svg>...</svg>");
imagePreview1.CustomButton.Add(customBtn);
```