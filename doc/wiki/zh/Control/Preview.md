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
**Form** | 所属窗口 | Form | `必填` |
**Content** | 内容 | `IList<Image>` / `IList<object>` | `必填` |
**ContentCount** | 内容数量 | int | `必填` |
**SelectIndex** | 当前选择序号 | int | 0 |
**Fit** | 图片布局 | [TFit](Enum.md#tfit)`?` | `null` |
**Keyboard** | 是否支持键盘 esc 关闭 | bool | true |
**Tag** | 用户定义数据 | object`?` | `null` |
**OnSelectIndexChanged** | SelectIndex 改变回调 | Func<int, bool>`?` | `null` |
||||
**ShowBtn** | 是否显示按钮 | bool | true |
**ShowDefaultBtn** | 是否显示默认按钮 | bool | true |
**BtnSize** | 按钮大小 | Size | 42, 46 |
**BtnIconSize** | 按钮图标大小 | int | 18 |
**BtnLRSize** | 左右按钮大小 | int | 40 |
**ContainerPadding** | 容器边距 | int | 24 |
**BtnPadding** | 按钮边距 | Size | 12, 32 |
**Btns** | 自定义按钮 | [Btn[]](#btn) | `null` |
**OnBtns** | 自定义按钮回调 | Action<string, BtnEvent>`?` | `null` |

### Btn

> 自定义按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 按钮名称 | string | `必填` |
**IconSvg** | 图标SVG | string | `必填` |
**Tag** | 用户定义数据 | object`?` | `null` |

### BtnEvent

> 按钮事件参数

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Index** | 数据序号 | int | |
**Data** | 元数据 | object`?` | |
**Tag** | Btn的Tag | object`?` | |