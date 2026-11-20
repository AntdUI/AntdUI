[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Descriptions

Descriptions 描述列表 👚

> 成组展示多个只读字段，常见于详情页的信息展示。

- 默认属性：Items

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 集合 | DescriptionsItemCollection |
**Title** | 标题 | string`?` | `null` |
🌏 **LocalizationTitle** | 标题 | string`?` | `null` |
**TextAlign** | 内容文本位置 | ContentAlignment | MiddleLeft |
**TextWrap** | 内容超出是否换行（否=显示省略号） | bool | true |
||||
**ForeColor** | 内容文字颜色 | Color`?` | `null` |
**LabelFore** | 标签文字颜色 | Color`?` | `null` |
**TitleFontRatio** | 标题字体比例 | float | 1.15F |
**TitleFore** | 标题文字颜色 | Color`?` | `null` |
||||
**Back** | 背景颜色 | Color`?` | `null` |
**BackExtend** | 背景渐变色 | string`?` | `null` |
**LabelBack** | 标签背景颜色（带边框模式生效） | Color`?` | `null` |
||||
**IconGap** | 图标与文本间隔 | int | 4 |
**IconRatio** | 图标比例 | float | .7F |
**TitleIcon** | 标题图标 | Image`?` | `null` |
**TitleIconSvg** | 标题图标SVG | string`?` | `null` |
||||
**AutoSize** | 自动大小 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
**PauseLayout** | 暂停布局 | bool | false |
**SizeMode** | 设置列表的大小 | TSizeMode | Default |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**Radius** | 圆角 | int | 6 |
**Bordered** | 是否展示边框 | bool | false |
**BorderStyle** | 边框样式 | DashStyle | Solid |
||||
**BadgeGap** | 徽标与文本间距 | int | 6 |
**BadgeRatio** | 徽标点比例 | float | .4F |
**Column** | 一行的 DescriptionsItem 数量 | int | 3 |
**ColumnWidth** | 列宽模式 | TDescriptionsColumnWidth | Auto |
**ItemLayout** | 描述布局 | TDescriptionsLayout | Horizontal |
**LabelAlign** | 标签文本位置 | ContentAlignment | MiddleLeft |
**LabelRatio** | 标签最大占比（自动宽度时生效） | float | .65F |
**LabelWidth** | 标签宽度（0=自动宽度） | int | 0 |
**MaxColumnRatio** | 单列最大占比（列宽模式为内容时生效） | float | .6F |
||||
**Colon** | 是否显示标签后面的冒号 | bool | true |

### 数据

#### DescriptionsItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | 名称 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 文本 | string`?` | `null` |
**Value** | 原始值（优先于文本，配合格式化） | object`?` | `null` |
**ContentType** | 内容类型（文本/徽标） | TDescriptionsContentType | Text |
**Format** | 文本格式化（标准格式字符串或含 {0} 的复合格式） | string`?` | `null` |
**Label** | 标签 | string`?` | `null` |
🌏 **LocalizationLabel** | 标签 | string`?` | `null` |
||||
**ForeColor** | 内容文字颜色 | Color`?` | `null` |
**LabelFore** | 标签文字颜色 | Color`?` | `null` |
||||
**Fill** | 徽标颜色 | Color`?` | `null` |
||||
**Visible** | 是否可见 | bool | true |
||||
**Span** | 包含列的数量（小于等于0=铺满当前行剩余部分） | int | 1 |
**State** | 状态（徽标色彩） | [TState](Enum.md#tstate) | Default |

### 方法

名称 | 描述 | 返回值 | 参数
:--|:--|:--|:--|
**Relayout** | 重新布局 | void ||

### 枚举

#### TDescriptionsLayout

> 描述布局

| 值 | 描述 |
|:--|:--|
| Horizontal | 水平 |
| Vertical | 垂直 |

#### TDescriptionsColumnWidth

> 列宽模式

| 值 | 描述 |
|:--|:--|
| Auto | 自动（带边框按内容比例，无边框等分） |
| Equal | 等分 |
| Content | 按内容比例 |

#### TDescriptionsContentType

> 描述列表内容类型

| 值 | 描述 |
|:--|:--|
| Text | 文本 |
| Badge | 徽标（圆点+文本） |

#### TSizeMode

> 设置列表的大小（共享枚举）

| 值 | 描述 |
|:--|:--|
| Default | 默认 |
| Middle | 中等 |
| Small | 小型 |

### 示例

> 创建描述列表，展示基本信息与徽标状态

```csharp
var descriptions = new AntdUI.Descriptions
{
	Dock = DockStyle.Top,
	Title = "用户信息",
	Column = 2,
	Bordered = true,
	ItemLayout = TDescriptionsLayout.Horizontal,
	ColumnWidth = TDescriptionsColumnWidth.Auto
};

descriptions.Items.Add(new AntdUI.DescriptionsItem("姓名", "张三"));
descriptions.Items.Add(new AntdUI.DescriptionsItem("年龄", "28"));
descriptions.Items.Add(new AntdUI.DescriptionsItem("部门", "技术部"));
descriptions.Items.Add(new AntdUI.DescriptionsItem("状态", "在线")
{
	ContentType = TDescriptionsContentType.Badge,
	State = TState.Success
});

Controls.Add(descriptions);
```
