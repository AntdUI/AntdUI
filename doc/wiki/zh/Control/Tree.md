[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Tree

Tree 树形控件 👚

> 多层次的结构列表。

- 默认属性：Items
- 默认事件：SelectChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 数据 `TreeItem[]` | [TreeItem[]](#treeitem) | [] |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeActive** | 激活字体颜色 | Color`?` | `null` |
||||
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**DragHandleIconSize** | 拖拽手柄图标大小 | int | 14 |
**IconRatio** | 图标比例 | float | 1F |
||||
**Checkable** | 节点前添加 Checkbox 复选框 | bool | false |
**CheckStrictly** | Checkable 状态下节点选择完全受控 `父子节点选中状态不再关联` | bool | true |
**DragAutoExpandDelay** | 拖拽悬停自动展开延迟（ms） | int | 1000 |
**Draggable** | 是否启用节点拖拽 | bool | true |
**DragHandleVisible** | 是否显示拖拽手柄 | bool | true |
**Multiple** | 支持点选多个节点 | bool | false |
**PauseLayout** | 暂停布局 | bool | false |
**SelectItem** | 选择项 | [TreeItem](#treeitem)`?` | `null` |
**VirtualMode** | 虚拟模式（大数据量时仅渲染可见行） | bool | false |
||||
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
||||
**Gap** | 间距 | int | 8 |
**GapIndent** | 间距缩进 | int`?` | `null` |
||||
**BlockNode** | 节点占据一行 | bool | false |
**Empty** | 是否显示空样式 | bool | true |
**EmptyImage** | 数据为空显示图片 | Image`?` | `null` |
**EmptyText** | 数据为空显示文字 | string`?` | `null` |

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
**Drop** | 拖拽放置时发生 | void | [TreeItem](#treeitem) from `源项`, [TreeItem](#treeitem) to `目标项`, [TreeDropMode](#treedropmode) mode `放置模式` |
**DropDone** | 拖拽完成后发生 | void | [TreeItem](#treeitem) from `源项`, [TreeItem](#treeitem) to `目标项`, [TreeDropMode](#treedropmode) mode `放置模式` |

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
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**Fore** | 字体颜色 | Color`?` |`null`|
**BadgeFore** | 徽标前景颜色 | Color`?` | `null`
**ForeSub** | 子文本颜色 | Color`?` |`null`|
||||
**Back** | 背景颜色 | Color`?` |`null`|
**BadgeBack** | 徽标背景颜色 | Color`?` | `null`
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
||||
**Loading** | 加载状态 | bool | false |
||||
**Checked** | 选中状态 | bool | false |
**Enabled** | 禁用状态 | bool | true |
**Visible** | 是否显示 | bool | true |
**BadgeMode** | 徽标模式（镂空） | bool | false
**CanExpand** | 是否可以展开 | bool | `只读` |
**Checkable** | 节点前添加 Checkbox 复选框 | bool | true |
**CheckState** | 选中状态 | CheckState | `Unchecked` |
**Select** | 激活状态 | bool | false |
||||
**BadgeBorderColor** | 徽标边框颜色 | Color`?` | `null`
**BadgeBorderWidth** | 徽标边框宽度 | float`?` | `null`
||||
**BadgeAlign** | 徽标方向 | [TAlign](Enum.md#talign) | Right
**BadgeOffsetX** | 徽标偏移X | int | 1
**BadgeOffsetY** | 徽标偏移Y | int | 1
**BadgeSize** | 徽标比例 | float | .6F
**PARENTITEM** | 父级对象 | [TreeItem](#treeitem)`?` | `null` |
||||
**Badge** | 徽标内容 | string`?` | `null`
**BadgeSvg** | 徽标SVG | string`?` | `null`
**Expand** | 展开 | bool | true |
**Sub** | 子集合 ♾️ | [TreeItem[]](#treeitem) | [] |
**SubTitle** | 子标题 | string | `null` |
🌏 **LocalizationSubTitle** | 国际化子标题 | string`?` | `null` |
### 枚举

#### TreeDropMode

> 拖拽放置模式

 名称 | 描述 | 值 |
 :--|:--|:--|
 **Before** | 放置到目标节点前 | 0 |
 **Inside** | 放置到目标节点内（作为子节点） | 1 |
 **After** | 放置到目标节点后 | 2 |