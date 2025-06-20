[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## TabHeader

TabHeader 多标签页头 👚

> 多标签页头。继承于 [PageHeader](PageHeader.md)

- 默认属性：SelectedIndex
- 默认事件：TabChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Radius** | 圆角 | int | 6 |
**RadiusContent** | 内容圆角 | int | 4 |
**OffsetY** | Y偏移量 | int | 0 |
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeHover** | 悬浮文本颜色 | Color`?` | `null` |
**ForeActive** | 激活文本颜色 | Color`?` | `null` |
**BackHover** | 悬浮背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**BorderWidth** | 边框宽度 | float | 0F |
**BorderColor** | 边框颜色 | Color`?` | `null` |
||||
**TabIconRatio** | 图标比例 | float | 1.34F |
**TabCloseRatio** | 关闭按钮比例 | float | 1.408F |
**TabCloseIconRatio** | 关闭图标比例 | float | 0.74F |
**TabGapRatio** | 边距比例 | float | 0.6F |
**TabIconGapRatio** | 图标与文字间距比例 | float | 0.74F |
**TabAddIconRatio** | 新增按钮图标比例 | float | 1.18F |
**TabAddGapRatio** | 新增按钮边距比例 | float | 0.148F |
||||
**DragSort** | 拖拽排序 | bool | false |
**ShowAdd** | 是否显示添加 | bool | false |
||||
**Items** | 数据 `TagTabItem[]` | [TagTabItem[]](#tagtabitem) | [] |
**SelectedIndex** | 选中序号 | int | 0 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**AddClick** | 点击添加按钮时发生 | void ||
**TabChanged** | SelectedIndex 属性值更改时发生 | void | [TagTabItem](#tagtabitem) Value, int Index `序号` |
**TabClosing** | Tab 关闭前发生 | void | [TagTabItem](#tagtabitem) Value, int Index `序号` |


### 数据

#### TagTabItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Visible** | 是否显示 | bool | true |
**Enabled** | 禁用状态 | bool | true |
**Loading** | 加载状态 | bool | false |
**Tag** | 用户定义数据 | object`?` | `null` |