[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## Select

Select 选择器 👚

> 下拉选择器。继承于 [Input](Input)

- 默认属性：Text
- 默认事件：SelectedIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**List** | 是否列表样式 `与Dropdown一样` | bool | false |
**ListAutoWidth** | 是否列表自动宽度 | bool | true |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum#talignfrom) | BL |
**MaxCount** | 列表最多显示条数 | int | 4 |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**DropDownPadding** 🔴 | 下拉边距 | Size | 12, 5 |
**ClickEnd** | 点击到最里层 `无节点才能点击` | bool | false |
**ClickSwitchDropdown** 🔴 | 点击切换下拉 | bool | true |
||||
**Items** | 数据 [更多样式](../DropdownStyles.md) | object[] | [] |
**SelectedIndex** | 选中序号 | int | -1 |
**SelectedValue** | 选中值 | object`?` | `null` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedIndexChanged** | SelectedIndex 属性值更改时发生 | void | int index `序号` |
**SelectedIndexsChanged** | 多层树结构更改时发生 | void | int x `第几列`, int y `第几行` |
**SelectedValueChanged** | SelectedValue 属性值更改时发生 | void | object? value `数值` |


***


## SelectMultiple

Select 多选器 👚

> 下拉多选器。继承于 [Input](Input)

- 默认属性：Text
- 默认事件：SelectedValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**List** | 是否列表样式 `与Dropdown一样` | bool | false |
**ListAutoWidth** | 是否列表自动宽度 | bool | true |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum#talignfrom) | BL |
**MaxCount** | 列表最多显示条数 | int | 4 |
**MaxChoiceCount** | 最大选中数量 | int | 0 |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**DropDownPadding** 🔴 | 下拉边距 | Size | 12, 5 |
**CheckMode** 🔴 | 复选框模式 | bool | false |
**CanDelete** 🔴 | 是否可以删除 | bool | true |
||||
**Items** | 数据 [更多样式](../DropdownStyles.md) | object[] | [] |
**SelectedValue** | 选中值 | object[] | |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectAllItems** | 全选项目 | void | |
**ClearSelect** | 清空选中 | void | |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedValueChanged** | SelectedValue 属性值更改时发生 | void | object[] value `数组` |