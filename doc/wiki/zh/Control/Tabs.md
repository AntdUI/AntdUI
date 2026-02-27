[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Tabs

Tabs 标签页 👚

> 选项卡切换组件。

- 默认属性：Pages
- 默认事件：SelectedIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
||||
**Fill** | 颜色 | Color`?` | `null` |
**FillHover** | 悬停颜色 | Color`?` | `null` |
**FillActive** | 激活颜色 | Color`?` | `null` |
||||
**Alignment** | 位置 | TabAlignment |Top|
**Centered** | 标签居中展示 | bool | false |
**TextCenter** | 文本是否居中对齐（仅在Left/Right方向生效） | bool | false |
||||
**TypExceed** | 超出UI类型 | [TabTypExceed](Enum.md#tabtypexceed) | Button |
**EnableSwitch** | 切换使能 | bool | true |
**EnablePageScrolling** | 鼠标滚轮切换焦点页使能 | bool | true |
**ScrollBack** | 滚动条颜色 | Color`?` | `null` |
**ScrollBackHover** | 滚动条悬停颜色 | Color`?` | `null` |
**ScrollFore** | 滚动条文本颜色 | Color`?` | `null` |
**ScrollForeHover** | 滚动条悬停文本颜色 | Color`?` | `null` |
||||
**Gap** | 间距 | int | 8 |
**IconRatio** | 图标比例 | float | 0.7F |
**IconGap** | 图标与文字间距比例 | float | 0.25F |
**ItemSize** | 自定义项大小 | int? | `null` |
||||
**Type** | 类型 | [TabType](Enum.md#tabtype) | Line |
**Style** | 样式类型 | [IStyle](#istyle) | `非空` |
**Rotate** | 旋转（用于 Left/Right 时竖排显示） | [TRotate](Enum.md#trotate) | None |
**DragOrder** | 拖动顺序 | bool | false |
||||
**TabMenuVisible** | 是否显示头 | bool | true |
||||
**Pages** | 集合 `TabCollection` | [TabCollection](#tabpage) | [] |
**SelectedIndex** | 选择序号 | int | 0 |
**SelectedTab** | 当前项 | [TabPage](#tabpage)`?` |`null`|

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectTab** | 选中项 | void | string tabPageName |
**SelectTab** | 选中项 | void | [TabPage](#tabpage) tabPage |
**SelectTab** | 选中项 | void | int index `序号` |
**ContainsTabPage** | 判断鼠标是否在标签上 | [TabPage](#tabpage)`?` | int x `X坐标`, int y `Y坐标` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedIndexChanged** | SelectedIndex 属性值更改时发生 | void | int index `序号` |
**SelectedIndexChanging** | SelectedIndex 属性值更改前发生 | bool | int index `序号` |
**ClosingPage** | 关闭页面前发生 | bool | [TabPage](#tabpage) `页` page |
**TabClick** | 点击标签时发生 | bool | [TabPage](#tabpage) `页` page, int index `序号` |

### IStyle

#### StyleLine

> 线条样式

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Size** | 条大小 | int | 3 |
**Padding** | 条边距 | int | 8 |
**Radius** | 条圆角 | int | 0 |
**BackSize** | 条背景大小 | int | 1 |
||||
**Back** | 条背景 | Color`?` | `null` |

#### StyleCard

> 卡片样式

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Radius** | 卡片圆角 | int | 6 |
**Border** | 边框大小 | int | 1 |
**Gap** | 卡片间距 | int | 2 |
**Closable** | 可关闭 | bool | false |
||||
**Fill** | 卡片颜色 | Color`?` | `null` |
**FillHover** | 卡片悬停颜色 | Color`?` | `null` |
**FillActive** | 卡片激活颜色 | Color`?` | `null` |
**BorderColor** | 卡片边框颜色 | Color`?` | `null` |
**BorderActive** | 卡片边框激活颜色 | Color`?` | `null` |



### 数据

#### TabPage

> 继承于 [ScrollableControl](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Scrolling/ScrollableControl.cs)

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
||||
**Badge** | 徽标内容 | string`?` | `null` |
**BadgeSize** | 徽标比例 | float | 0.6F |
**BadgeBack** | 徽标背景颜色 | Color`?` | `null` |
**BadgeOffsetX** | 徽标偏移X | int | 0 |
**BadgeOffsetY** | 徽标偏移Y | int | 0 |
||||
**Text** | 显示文本 | string ||
**Visible** | 是否显示 | bool | true |
**ReadOnly** | 只读 | bool | false |