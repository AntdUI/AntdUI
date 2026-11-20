[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Select

Select 选择器 👚

> 下拉选择器。继承于 [Input](Input)

- 默认属性：Text
- 默认事件：SelectedIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 数据 [更多样式](../DropdownStyles.md) | BaseCollection | [] |
**AutoText** | 自动设置文本 | bool | true |
||||
**AutoPrefixSvg** | 自动设置下拉前缀 | bool | false |
**CloseIcon** | 是否显示关闭图标 | bool | false |
**ShowIcon** | 是否显示图标 | bool | true |
||||
**SelectedIndex** | 选中序号 | int | -1 |
**SelectedValue** | 选中值 | object`?` | `null` |
**ListAutoWidth** | 是否列表自动宽度 | bool | false |
**WheelModifyEnabled** | 鼠标滚轮修改值 | bool | true |
**DropDownEmptyClose** | 下拉为空关闭 | bool | false |
||||
**MaxCount** | 列表最多显示条数 | int | 4 |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**ArrowKeyDropDown** | Whether the Down arrow opens the drop-down | bool | true |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**DropDownPadding** | 下拉边距 | Size | 12, 5 |
**DropDownTextAlign** | 下拉文本方向 | [TAlign](Enum.md#talign) | Left |
||||
**ClickEnd** | 点击到最里层 `无节点才能点击` | bool | false |
**Empty** | 为空依旧下拉 | bool | false |
**Expand** | 箭头是否展开(UI) | bool | false |
**ExpandDrop** | 展开下拉菜单 | bool | false |
**List** | 是否列表样式 `与Dropdown一样` | bool | false |
**ClickSwitchDropdown** | 点击切换下拉 | bool | true |
**DropDownRadius** | 下拉圆角 | int`?` | `null` |
**EnterDropDown** | 是否允许回车键触发下拉框 | bool | true |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedIndexChanged** | SelectedIndex 属性值更改时发生 | void | int index `序号` |
**SelectedIndexsChanged** | 多层树结构更改时发生 | void | int x `第几列`, int y `第几行` |
**SelectedValueChanged** | SelectedValue 属性值更改时发生 | void | object? value `数值` |
**ExpandDropChanged** | 下拉展开 属性值更改时发生 | void | bool value `展开状态` |
**FilterChanged** | 控制筛选 Text更改时发生 | IList<object>`?` | string value `搜索内容` |
**ClosedItem** | 关闭某项 时发生 | void | object? value `数值` |
**DrawItem** | 子项外部渲染前触发 | bool `是否处理了渲染` | DrawItemEventArgs e `事件参数，包含画布、矩形、项目信息等` |


***


## SelectMultiple

Select 多选器 👚

> 下拉多选器。继承于 [Input](Input)

- 默认属性：Text
- 默认事件：SelectedValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 数据 [更多样式](../DropdownStyles.md) | object[] | [] |
||||
**SelectedValue** | 选中值 | object[] | [] |
**AutoHeight** | 自动高度 | bool | false |
**CanDelete** | 是否可以删除 | bool | true |
**CheckMode** | 复选框模式 | bool | false |
**ListAutoWidth** | 是否列表自动宽度 | bool | true |
||||
**Gap** | 间距 | int | 2 |
**MaxChoiceCount** | 最大选中数量 | int | 0 |
**MaxCount** | 列表最多显示条数 | int | 4 |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum.md#talignfrom) | BL |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**DropDownPadding** | 下拉边距 | Size | 12, 5 |
||||
**List** | 是否列表样式 `与Dropdown一样` | bool | false |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectAllItems** | 全选项目 | void | |
**ClearSelect** | 清空选中 | void | |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedValueChanged** | SelectedValue 属性值更改时发生 | void | object[] value `数组` |