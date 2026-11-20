[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Collapse

Collapse 折叠面板 👚

> 可以折叠/展开的内容区域。

- 默认属性：Items
- 默认事件：ExpandChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 数据集合 | CollapseItemCollection | - |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**FontExpand** | 展开后的标题字体 | Font`?` | `null` |
**ForeActive** | 文字激活颜色 | Color`?` | `null` |
||||
**AnimationSpeed** | 展开/折叠的动画速度 | int | 100 |
||||
**AutoSize** | 自动大小 | bool | false |
**EnableResizing** | 手动调整大小 | bool | false |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**Radius** | 圆角 | int | 6 |
||||
**ContentPadding** | 折叠面板内容内边距 | Size | 16, 16 |
**Gap** | 间距 | int | 0 |
**HeaderPadding** | 折叠面板头部内边距 | Size | 16, 12 |
**ResizingLineColor** | 高度调整线的颜色 (默认按Primary) | Color`?` | `null` |
**ResizingLineThickness** | 高度调整线的粗细 (设为0时，表示禁用) | float | 2 |
||||
**HeaderBg** | 折叠面板头部背景 | Color`?` | `null` |
**TooltipConfig** | 超出文字提示配置 | TooltipConfig`?` | `null` |
**Unique** | 只保持一个展开 | bool | false |
**UniqueFull** | 一个展开铺满 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ExpandChanged** | Expand 属性值更改时发生 | void | [CollapseItem](#collapseitem) value `对象`, bool Expand `是否展开` |
**ExpandingChanged** | Expanding 属性值更改时发生 | void | [CollapseItem](#collapseitem) value `对象`, bool Expand `是否展开` |
**ButtonClick** | CollapseItem上的按钮单击时发生 | void | [CollapseItem](#collapseitem) value `对象`, CollapseGroupButton button `按钮` |

### 数据

#### CollapseItem

> 继承于 [ScrollableControl](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Scrolling/ScrollableControl.cs)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string | "" |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**Icon** | 图标 | Image`?` | `null`
**IconSvg** | 图标SVG | string`?` | `null`
||||
**Buttons** | 按钮集合 | CollapseGroupButtonCollection | - |
**Expand** | 是否展开 | bool | false |
**Full** | 是否铺满剩下空间 | bool | false |