[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Collapse

Collapse 折叠面板 👚

> 可以折叠/展开的内容区域。

- 默认属性：Items
- 默认事件：ExpandChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
**HeaderBg** | 折叠面板头部背景 | Color`?` | `null` |
**HeaderPadding** | 折叠面板头部内边距 | Size | 16, 12 |
**ContentPadding** | 折叠面板内容内边距 | Size | 16, 16 |
||||
**BorderWidth** | 边框宽度 | float | 1F |
**BorderColor** | 边框颜色 | Color`?` | `null` |
||||
**Radius** | 圆角 | int |6 |
**Gap** | 间距 | int | 0 |
**Unique** | 只保持一个展开 | bool | false |
||||
**Items** | 数据 `CollapseItem[]` | [CollapseItem[]](#collapseitem) | [] |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ExpandChanged** | Expand 属性值更改时发生 | void | [CollapseItem](#collapseitem) value `对象`, bool Expand `是否展开` |


### 数据

#### CollapseItem

> 继承于 [ScrollableControl](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Scrolling/ScrollableControl.cs)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Expand** | 展开 | bool | true |
**Full** 🔴 | 是否铺满剩下空间 | bool | false |
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |