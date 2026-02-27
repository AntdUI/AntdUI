[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

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
**Gap** | 间距 | int | 8 |
**GapIndent** | 间距缩进 | int`?` | `null` |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
**IconRatio** | 图标比例 | float | 1F |
**Checkable** | 节点前添加 Checkbox 复选框 | bool | false |
**CheckStrictly** | Checkable 状态下节点选择完全受控 `父子节点选中状态不再关联` | bool | true |
**BlockNode** | 节点占据一行 | bool | false |
**Multiple** | 支持点选多个节点 | bool | false |
||||
**Items** | 数据 `TreeItem[]` | [TreeItem[]](#treeitem) | [] |
**SelectItem** | 选择项 | [TreeItem](#treeitem)`?` | `null` |
||||
**Empty** | 是否显示空样式 | bool | true |
**EmptyText** | 数据为空显示文字 | string`?` | `null` |
**EmptyImage** | 数据为空显示图片 | Image`?` | `null` |
||||
**PauseLayout** | 暂停布局 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectChanged** | Select 属性值更改时发生 | void | [TreeItem](#treeitem) item `项`, Rectangle rect `项区域`, TreeCType type `类型`, MouseEventArgs args `点击` |
**CheckedChanged** | Checked 属性值更改时发生 | void | [TreeItem](#treeitem) item `项`, bool value `选中值` |
**BeforeExpand** | Expand 更改前发生 | bool `返回true继续展开` | [TreeItem](#treeitem) item `项`, bool value `展开值` |
**AfterExpand** | Expand 更改后发生 | void | [TreeItem](#treeitem) item `项`, bool value `展开值` |
**NodeMouseClick** | 点击项事件 | void | [TreeItem](#treeitem) item `项`, Rectangle rect `项区域`, TreeCType type `类型`, MouseEventArgs args `点击` |
**NodeMouseDoubleClick** | 双击项事件 | void | [TreeItem](#treeitem) item `项`, Rectangle rect `项区域`, TreeCType type `类型`, MouseEventArgs args `点击` |
**NodeMouseMove** | 移动项事件 | void | [TreeItem](#treeitem) item `项`, Rectangle rect `项区域`, bool hover `悬停值` |
**NodeMouseDown** | 鼠标按下事件 | void | [TreeItem](#treeitem) item `项`, Rectangle rect `项区域`, TreeCType type `类型`, MouseEventArgs args `点击` |
**NodeMouseUp** | 鼠标松开事件 | void | [TreeItem](#treeitem) item `项`, Rectangle rect `项区域`, TreeCType type `类型`, MouseEventArgs args `点击` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ExpandAll** | 展开全部 | void | bool value `true 展开、false 收起` |
**GetCheckeds** | 获取所有选中项 | List<[TreeItem](#treeitem)> | bool Indeterminate `是否包含 Indeterminate` = true |
**Select** | 选择指定项 | bool | [TreeItem](#treeitem) item, bool focus `设置焦点` = true |
**SelectID** | 选择指定项（ID） | bool | string id `ID`, bool focus `设置焦点` = true |
**SelectName** | 选择指定项（Name） | bool | string name `名称`, bool focus `设置焦点` = true |
**USelect** | 取消全部选择 | void | bool clear `清空选择项` = true |
**SetCheckeds** | 全选/全不选 | void | bool check `是否选中` |
**Focus** | 跳转指定项 | void | [TreeItem](#treeitem) item, int gap `间隙` = 0, bool force `强制` = false |
**VisibleAll** | 设置全部 Visible | void | bool value `是否可见` = true |
**Remove** | 移除菜单 | void | [TreeItem](#treeitem) item `项` |
**ReverseCheckItem** | 反选节点项 | void | [TreeItem](#treeitem) item `项` |
**GetSelects** | 获取所有选择项 | List<[TreeItem](#treeitem)> | 无 |
**FindID** | 根据节点id查询节点 | [TreeItem](#treeitem)`?` | string id `ID` |
**FindName** | 根据节点name查询节点 | [TreeItem](#treeitem)`?` | string name `名称` |
**Search** | 搜索筛选 | void | string search `搜索文本` |
**HitTest** | 命中测试 | [TreeItem](#treeitem)`?` | int x `X坐标`, int y `Y坐标`, out TreeCType type `类型` |


### 数据

#### TreeItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | 名称 | string`?` | `null` |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**SubTitle** | 子标题 | string | `null` |
🌏 **LocalizationSubTitle** | 国际化子标题 | string`?` | `null` |
**Fore** | 字体颜色 | Color`?` |`null`|
**ForeSub** | 子文本颜色 | Color`?` |`null`|
**Back** | 背景颜色 | Color`?` |`null`|
**Visible** | 是否显示 | bool | true |
**Enabled** | 禁用状态 | bool | true |
**Checkable** | 节点前添加 Checkbox 复选框 | bool | true |
**Loading** | 加载状态 | bool | false |
**Expand** | 展开 | bool | true |
**CanExpand** | 是否可以展开 | bool | `只读` |
**Checked** | 选中状态 | bool | false |
**CheckState** | 选中状态 | CheckState | `Unchecked` |
**Sub** | 子集合 ♾️ | [TreeItem[]](#treeitem) | [] |
**Tag** | 用户定义数据 | object`?` | `null` |
||||
**PARENTITEM** | 父级对象 | [TreeItem](#treeitem)`?` | `null` |