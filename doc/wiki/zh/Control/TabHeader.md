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
**LeftGap** | 左侧边距 | int | 0 |
**RightGap** | 右侧边距 | int | 0 |
||||
**DragSort** | 拖拽排序 | bool | false |
**ShowAdd** | 是否显示添加 | bool | false |
**AddIconSvg** | 新增按钮Svg图标（默认 PlusOutlined） | string`?` | `null` |
||||
**Items** | 数据 `TagTabItem[]` | [TagTabItem](#tagtabitem) | [] |
**SelectedIndex** | 选中序号 | int | 0 |
**SelectedItem** | 选中选项 | [TagTabItem](#tagtabitem)`?` | `null` |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**AddClick** | 点击添加按钮时发生 | void ||
**TabChanged** | SelectedIndex 属性值更改时发生 | void | [TagTabItem](#tagtabitem) Value, int Index `序号` |
**TabSelectedItemChanged** | 选项选中事件 | void | [TagTabItem](#tagtabitem) Value, int Index `序号` |
**TabClosing** | Tab 关闭前发生 | void | [TagTabItem](#tagtabitem) Value, int Index `序号` |


### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**AddTab** | 添加标签 | void | [TagTabItem](#tagtabitem) item, bool select `是否选中` |
**AddTab** | 添加标签 | void | string text, Image`?` icon `图标` |
**AddTab** | 添加标签 | void | string text, string`?` iconsvg `图标SVG` |
**InsertTab** | 插入标签 | void | int index `索引`, [TagTabItem](#tagtabitem) item, bool select `是否选中` |
**InsertTab** | 插入标签 | void | int index `索引`, string text, Image`?` icon `图标` |
**RemoveTab** | 移除标签 | void | int index `索引` |
**Select** | 选择标签 | void | [TagTabItem](#tagtabitem) item |
**HitTest** | 点击测试 | [TagTabItem](#tagtabitem)`?` | int x `X坐标`, int y `Y坐标`, out int i `索引` |

### 数据

#### TagTabItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Visible** | 是否显示 | bool | true |
**Enabled** | 禁用状态 | bool | true |
**Loading** | 加载状态 | bool | false |
**ShowClose** | 是否显示关闭 | bool | true |
**Tag** | 用户定义数据 | object`?` | `null` |
||||
**Badge** | 徽标内容 | string`?` | `null` |
**BadgeSvg** | 徽标SVG | string`?` | `null` |
**BadgeAlign** | 徽标方向 | [TAlign](Enum.md#talign) | Right |
**BadgeSize** | 徽标比例 | float | 0.6F |
**BadgeMode** | 徽标模式（镂空） | bool | false |
**BadgeFore** | 徽标前景颜色 | Color`?` | `null` |
**BadgeBack** | 徽标背景颜色 | Color`?` | `null` |
**BadgeBorderColor** | 徽标边框颜色 | Color`?` | `null` |