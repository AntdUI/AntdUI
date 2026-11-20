[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Preview

Preview 图片预览

> 图片预览框。

### 使用方法

```csharp
// 单张图片
Preview.open(new Preview.Config(this, image));

// 多张图片
Preview.open(new Preview.Config(this, new Image[] { image1, image2 }));

// 带回调的图片
Preview.open(new Preview.Config(this, list, (index, item) => GetImage(item)));
```

### Preview.Config

> 配置图片预览

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Tag** | 用户定义数据 | object`?` | `null` |
**Content** | 内容 | `IList<Image>` / `IList<object>` | `必填` |
||||
**BtnIconSize** | 按钮图标大小 | int | 18 |
||||
**Loop** | 是否允许循环切换（超过1张图时生效） | bool | false |
**MaskClosable** | 点击空白处是否允许关闭 | bool | true |
**OnSelectIndexChanged** | SelectIndex 改变回调 | Func<int, bool>`?` | `null` |
**SelectIndex** | 当前选择序号 | int | 0 |
**ShowBtn** | 是否显示按钮 | bool | true |
**ShowDefaultBtn** | 是否显示默认按钮 | bool | true |
||||
**BtnLRSize** | 左右按钮大小 | int | 40 |
**BtnPadding** | 按钮边距 | Size | 12, 32 |
**BtnSize** | 按钮大小 | Size | 42, 46 |
**ContainerPadding** | 容器边距 | int | 24 |
||||
**Btns** | 自定义按钮 | [Btn[]](#btn) | `null` |
**ContentCount** | 内容数量 | int | `必填` |
**Fit** | 图片布局 | [TFit](Enum.md#tfit)`?` | `null` |
**Form** | 所属窗口 | Form | `必填` |
**Keyboard** | 是否支持键盘 esc 关闭 | bool | true |
**OnBtns** | 自定义按钮回调 | Action<string, BtnEvent>`?` | `null` |

### Btn

> 自定义按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 按钮名称 | string | `必填` |
**Tag** | 用户定义数据 | object`?` | `null` |
||||
**IconSvg** | 图标SVG | string | `必填` |

### BtnEvent

> 按钮事件参数

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Tag** | Btn的Tag | object`?` | |
||||
**Data** | 元数据 | object`?` | |
**Index** | 数据序号 | int | |

### ImageTextContent

> 图片和文本内容

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 显示文本 | string`?` | `null` |
**TextAlign** | 文本位置 | ContentAlignment | TopCenter |
**TextPadding** | 文本边距 | Padding | 0 |
||||
**Font** | 文本字体 | Font`?` | `null` |
**ForeColor** | 文本颜色 | Color`?` | `null` |
||||
**Image** | 图片 | Image | `必填` |