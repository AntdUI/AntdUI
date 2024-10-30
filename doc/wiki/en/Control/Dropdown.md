[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## Dropdown

Dropdown 选择器 👚

> 向下弹出的列表。继承于 [Button](Button)

- 默认属性：Text
- 默认事件：SelectedValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ListAutoWidth** | 列表自动宽度 | bool | true |
**Trigger** | 触发下拉的行为 | [Trigger](Enum#trigger) | Click |
**Placement** | 菜单弹出位置 | [TAlignFrom](Enum#talignfrom) | BL |
**MaxCount** | 列表最多显示条数 | int | 4 |
**DropDownArrow** | 下拉箭头是否显示 | bool | false |
**DropDownPadding** 🔴 | 下拉边距 | Size | 12, 5 |
**ClickEnd** | 点击到最里层 `无节点才能点击` | bool | false |
||||
**Items** | 数据 [更多样式](../下拉更多样式) | object[] | [] |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectedValueChanged** | SelectedValue 属性值更改时发生 | void | object? value `数值` |