[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## EditableTable

EditableTable 可编辑表格 ✏️

> 行级编辑表格：点击「编辑」后整行多个单元格同时进入编辑状态，操作列变为「保存 / 取消」；保存时执行行级校验，取消则还原原始值（新增行会被移除）。
>
> 这是通过 PR #182 贡献的 [Table](Table.md) 变体，继承自 `Table`，因此 [Table](Table.md) 的所有属性、方法与事件同样适用；列集合与数据绑定方式也与 Table 一致。编辑器通过 `SetEditor` 在代码中配置。

- 默认属性：Columns（继承自 Table）
- 默认事件：CellClick（继承自 Table）

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ShowCopyButton** | 内置操作列是否显示「复制」按钮 | bool | true |
**ShowDeleteButton** | 内置操作列是否显示「删除」按钮 | bool | true |
**ShowEditButton** | 内置操作列是否显示「编辑」按钮 | bool | true |
||||
**ErrorColumns** | 校验失败的列 Key 集合（只读） | string[] | 空数组 |
**IsRowEditing** | 是否有行正在编辑（只读） | bool | false |
||||
**BuiltinActions** | 是否启用内置操作列（编辑/保存/取消/删除按钮，由 AddRecord/BeginRowEdit 自动补充） | bool | false |
**EditingRecord** | 当前编辑中的行记录（只读） | object`?` | `null` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SetEditor** | 注册列编辑器（列将自动设为可编辑），返回编辑器用于链式配置校验规则 | [CellEditor](#celleditor) | string columnKey `列绑定名称（Key）`, TEditorType type `编辑器类型` |
**AddRecord** | 新增一行并立即进入行编辑 | bool | object record `新行数据对象`, int position = -1 `插入位置（-1 为末尾，0 为第一行）` |
**CopyRecord** | 复制一行并立即进入行编辑 | bool | object record `被复制的行记录` |
**BeginRowEdit** | 让一行进入行编辑（保存当前正在编辑的行后再切换） | bool | object record `行记录` |
**SaveRow** | 保存当前编辑行：写回所有编辑控件值 → 行级校验 → 触发 RowSaving（可拦截） | bool ||
**CancelRow** | 取消编辑并还原到进入编辑前的值（AddRecord 新增的行取消后被移除） | void ||
**DeleteRow** | 删除行（默认弹窗确认） | void | object record `行记录`, bool confirm = true `是否弹窗确认` |

> `SaveRow` 在校验失败或保存被拦截时返回 `false`，此时整行编辑控件保持活跃，修改后可再次保存；成功时返回 `true` 并结束编辑态。错误字段可通过 `ErrorColumns` 读取。

#### CellEditor

> 列编辑器配置，由 `SetEditor` 返回；通过以下方法链式配置，配置结果可通过 `Type`、`Required`、`RequiredMessage`、`MaxLength`、`Min`、`Max` 读取。

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SetRequired** | 设置是否必填及提示文案（保存时校验，失败拦截保存并标红单元格） | CellEditor | bool required = true `是否必填`, string? message = null `提示文案（默认 "{列名}不能为空"）` |
**SetMaxLength** | 设置最大文本长度（0 不限制） | CellEditor | int length `最大长度` |
**SetRange** | 设置数值最小值与最大值 | CellEditor | decimal? min `最小值`, decimal? max `最大值` |
**SetItems** | 设置下拉选项（SelectItem 或原始值） | CellEditor | params object[] items `选项集合` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**RowSaving** | 保存行时发生，可拦截（e.Cancel = true 拦截保存，行保持编辑态） | void | object? sender `事件源`, RowSavingEventArgs e `Record 行记录（已含编辑后的值）、Origin 进入编辑前的原始行快照、Cancel 是否取消保存` |
**RowSaved** | 行保存完成后发生 | void | object? sender `事件源`, RowEventArgs e `Record 行记录` |
**RowCanceled** | 行取消编辑后发生 | void | object? sender `事件源`, RowEventArgs e `Record 行记录` |
**RowDeleted** | 行删除后发生 | void | object? sender `事件源`, RowEventArgs e `Record 行记录` |
**RowAdded** | 新行添加后发生（进入编辑前） | void | object? sender `事件源`, RowEventArgs e `Record 新行记录` |

### TEditorType

> 单元格编辑器类型

名称 | 描述
:--|:--|
**Text** | 文本 |
**Number** | 数字 |
**Date** | 日期 |
**Select** | 下拉选择 |

### 示例

```csharp
// 1. 定义数据类（属性名需与列 Key 一致）
public class User
{
	public string Name { get; set; } = "";
	public int Age { get; set; }
	public string Role { get; set; } = "";
}

// 2. 创建 EditableTable 并绑定数据
var table = new AntdUI.EditableTable
{
	Dock = DockStyle.Fill,
	BuiltinActions = true, // 启用内置“编辑/复制/删除”操作列
	DataSource = new BindingList<User>
	{
		new User { Name = "张三", Age = 28, Role = "开发" },
		new User { Name = "李四", Age = 32, Role = "测试" }
	}
};
table.Columns.Add(new AntdUI.Column("Name", "姓名"));
table.Columns.Add(new AntdUI.Column("Age", "年龄"));
table.Columns.Add(new AntdUI.Column("Role", "角色"));
Controls.Add(table);

// 3. 注册列编辑器：文本必填且限制长度，数字限制范围，下拉提供选项
table.SetEditor("Name", AntdUI.TEditorType.Text)
	 .SetRequired(true, "姓名不能为空")
	 .SetMaxLength(20);
table.SetEditor("Age", AntdUI.TEditorType.Number)
	 .SetRange(1, 120);
table.SetEditor("Role", AntdUI.TEditorType.Select)
	 .SetItems("开发", "测试", "运维");

// 4. 保存前拦截（可选）
table.RowSaving += (s, e) =>
{
	if (e.Record is User u && u.Name == "admin") e.Cancel = true;
};
```

行编辑流程：点击行尾「编辑」调用 `BeginRowEdit`，整行同时进入编辑状态，操作列变为「保存 / 取消」。点击「保存」调用 `SaveRow`：先把编辑控件的值写回记录，再按各列的编辑器规则做行级校验，并触发 `RowSaving`（`e.Cancel = true` 可拦截）。校验失败或被拦截时行保持编辑态，错误单元格标红并显示提示，修改后可再次保存。点击「取消」调用 `CancelRow` 用快照还原原值；若该行由 `AddRecord` 新增，则直接从数据源移除。也可以在代码中直接调用 `AddRecord`、`CopyRecord`、`BeginRowEdit`、`SaveRow`、`CancelRow`、`DeleteRow`。
