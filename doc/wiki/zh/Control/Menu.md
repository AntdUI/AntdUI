[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Menu

Menu 导航菜单 👚

> 为页面和功能提供导航的菜单列表。

- 默认属性：Items
- 默认事件：SelectChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeActive** | 激活字体颜色 | Color`?` | `null` |
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
**Indent** | 常规缩进 `和Tree那样缩进` | bool | false |
**ShowSubBack** | 显示子菜单背景 | bool | false |
**Unique** 🔴 | 只保持一个子菜单的展开 | bool | false |
**Trigger** 🔴 | 触发下拉的行为 | [Trigger](Enum.md#trigger) | Click |
||||
**Theme** | 色彩模式 | [TAMode](Enum.md#tamode) | Auto |
**Mode** 🔴 | 菜单类型 | [TMenuMode](Enum.md#tmenumode) | Inline |
**AutoCollapse** | 自动折叠 | bool | false |
**Collapsed** | 是否折叠 | bool | false |
||||
**Items** | 数据 `MenuItem[]` | [MenuItem[]](#menuitem) | [] |
||||
**PauseLayout** | 暂停布局 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectChanged** | Select 属性值更改时发生 | void | [MenuItem](#menuitem) item `数值` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectIndex** | 选中第一层 | void | int index `序号`, bool focus `设置焦点` = true |
**SelectIndex** | 选中第二层 | void | int i1 `序号1` , int i2 `序号2`, bool focus `设置焦点` = true |
**SelectIndex** | 选中第三次 | void | int i1 `序号1` , int i2 `序号2`  , int i3 `序号3`, bool focus `设置焦点` = true |
||||
**Select** 🔴 | 选中菜单 | void | MenuItem item `项`, bool focus `设置焦点` = true |
**Remove** 🔴 | 移除菜单 | void | MenuItem item `项` |


### 数据

#### MenuItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**IconRatio** 🔴 | 图标比例 | float | 1.2F |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string | `null` |
**Text** | 文本 | string | `必填` |
**Font** 🔴 | 自定义字体 | Font`?` | `null` |
**Visible** | 是否显示 | bool | true |
**Enabled** | 禁用状态 | bool | true |
**Select** | 是否选中 | bool | false |
**Expand** | 展开 | bool | true |
**CanExpand** | 是否可以展开 | bool | `只读` |
**Sub** | 子集合 ♾️ | [MenuItem[]](#menuitem) | [] |
**Tag** | 用户定义数据 | object`?` | `null` |