[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## ImagePreview

ImagePreview 图片预览 👚

> 用于常驻显示和预览图片，支持多种交互操作和自定义配置。

- 默认属性：Image
- 默认事件：SelectIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 图片项集合 | ImagePreviewItemCollection | `新集合` |
**SelectIndex** | 当前选中的图片索引 | int | 0 |
**Image** | 直接设置的图片 | Image`?` | `null` |
**Call** | 异步加载图片的委托 | Func<Image>`?` | `null` |
**CallProg** | 带进度的异步加载委托 | Func<Action<Image, float>, Task>`?` | `null` |
**Fit** | 图片适应方式 | [TImageFit](Enum.md#timagefit) | Contain |
**FlipX** | 水平翻转 | bool | false |
**FlipY** | 垂直翻转 | bool | false |
**Zoom** | 缩放比例 | float | 1F |
**ZoomStep** | 缩放步长 | float | 0.1F |
**ZoomMin** | 最小缩放比例 | float | 0.1F |
**ZoomMax** | 最大缩放比例 | float | 10F |
**Button** | 是否显示默认按钮 | bool | true |
**ButtonSize** | 按钮大小 | int | 32 |
**ButtonIconSize** | 按钮图标大小 | int | 18 |
**ButtonMargin** | 按钮边距 | int | 10 |
**ButtonGap** | 按钮间距 | int | 10 |
**ButtonAutoHide** | 自动隐藏按钮 | bool | false |
**ButtonAutoHideDelay** | 自动隐藏延迟(毫秒) | int | 2000 |
**CustomButtons** | 自定义按钮集合 | ImagePreviewButtonCollection | `新集合` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**LoadImg** | 加载图片 | void | bool `是否重置` |
**PlayGif** | 播放GIF动画 | void | Image `图片`, FrameDimension `帧维度`, int `帧数量` |
**ScaleImg** | 缩放图片 | RectangleF | Rectangle `控件矩形`, float `DPI` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectIndexChanged** | 当选择的图片索引改变时触发 | void | int `索引` |
**ButtonClick** | 当点击按钮时触发 | void | ImagePreviewButton `按钮` |

### 示例

```csharp
// 基本使用
imagePreview1.Image = Image.FromFile("test.jpg");

// 添加多张图片
imagePreview1.Items.Add(new ImagePreviewItem(Image.FromFile("img1.jpg")));
imagePreview1.Items.Add(new ImagePreviewItem(Image.FromFile("img2.jpg")));
imagePreview1.Items.Add(new ImagePreviewItem(Image.FromFile("img3.jpg")));

// 异步加载图片
imagePreview1.Call = () => {
    // 模拟异步加载
    Thread.Sleep(1000);
    return Image.FromFile("async.jpg");
};

// 带进度的异步加载
imagePreview1.CallProg = async (setImg) => {
    // 模拟下载进度
    for (int i = 0; i <= 100; i += 10) {
        await Task.Delay(100);
        // 更新进度
        setImg(null, i / 100f);
    }
    // 加载完成
    setImg(Image.FromFile("prog.jpg"), 1f);
};

// 添加自定义按钮
var customBtn = new ImagePreviewButton()
    .SetSvg("<svg>...</svg>")
    .SetOnClick(btn => {
        MessageBox.Show("自定义按钮被点击");
    });
imagePreview1.CustomButtons.Add(customBtn);
```