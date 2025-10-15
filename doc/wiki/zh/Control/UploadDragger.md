[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## UploadDragger

UploadDragger 拖拽上传 👚

> 文件选择上传和拖拽上传控件。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextDesc** | 文本描述 | string`?` | `null` |
**Radius** | 圆角 | int | 8 |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` ||
**Back** | 背景颜色 | Color`?` | `null` |
||||
**BackgroundImage** | 背景图片 | Image`?` | `null` |
**BackgroundImageLayout** | 背景图片布局 | [TFit](Enum.md#tfit) | Fill |
||||
**IconRatio** | 图标比例 | float | 1.92F |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
||||
**BorderWidth** | 边框宽度 | float | 1F |
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderStyle** | 边框样式 | DashStyle | Solid |
||||
**ClickHand** 🔴 | 点击上传 | bool | true |
**Multiselect** 🔴 | 多个文件 | bool | true |
**Filter** 🔴 | 文件名筛选器字符串 | string`?` | `null` 参考 OpenFileDialog 格式 |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ManualSelection** | 手动触发选择文件 | void | |
**SetFilter** | 设置常用Filter | void | FilterType filterType `可叠加` |


### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**DragChanged** | 文件拖拽后时发生 | void | string[] files `文件列表` |