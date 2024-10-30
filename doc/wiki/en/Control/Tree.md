[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## Tree

Tree 树形控件 👚

> 多层次的结构列表。

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
**Gap** 🔴 | 间距 | int | 8 |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
**Checkable** | 节点前添加 Checkbox 复选框 | bool | false |
**CheckStrictly** | Checkable 状态下节点选择完全受控 `父子节点选中状态不再关联` | bool | true |
**BlockNode** | 节点占据一行 | bool | false |
||||
**Items** | 数据 `TreeItem[]` | [TreeItem[]](#treeitem) | [] |
||||
**PauseLayout** | 暂停布局 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectChanged** | Select 属性值更改时发生 | void | MouseEventArgs args `点击`, [TreeItem](#treeitem) item `数值`, Rectangle rect `项区域` |
**CheckedChanged** | Checked 属性值更改时发生 | void | [TreeItem](#treeitem) item `数值`, bool value `选中值` |
**NodeMouseClick** | 点击项事件 | void | MouseEventArgs args `点击`, [TreeItem](#treeitem) item `数值`, Rectangle rect `项区域` |
**NodeMouseDoubleClick** | 双击项事件 | void | MouseEventArgs args `点击`, [TreeItem](#treeitem) item `数值`, Rectangle rect `项区域` |
**NodeMouseMove** | 移动项事件 | void | [TreeItem](#treeitem) item `数值`, Rectangle rect `项区域`, bool hover `悬停值` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ExpandAll** | 展开全部 | void | bool value `true 展开、false 收起` |
**GetCheckeds** | 获取所有选中项 | List<[TreeItem](#treeitem)> ||


### 数据

#### TreeItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 名称 | string`?` | `null` |
**IconRatio** 🔴 | 图标比例 | float | 1F |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**Text** | 文本 | string | `必填` |
**Fore** 🔴 | 字体颜色 | Color`?` |`null`|
**Back** 🔴 | 背景颜色 | Color`?` |`null`|
**Visible** | 是否显示 | bool | true |
**Enabled** | 禁用状态 | bool | true |
**Expand** | 展开 | bool | true |
**CanExpand** | 是否可以展开 | bool | `只读` |
**Checked** | 选中状态 | bool | false |
**CheckState** | 选中状态 | CheckState | `Unchecked` |
**Sub** | 子集合 ♾️ | [TreeItem[]](#treeitem) | [] |
**Tag** | 用户定义数据 | object`?` | `null` |