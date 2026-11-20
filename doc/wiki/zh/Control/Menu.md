[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Menu

Menu 导航菜单 👚

> 为页面和功能提供导航的菜单列表。

- 默认属性：Items
- 默认事件：SelectChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Items** | 数据 `MenuItem[]` | [MenuItem[]](#menuitem) | [] |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeActive** | 激活字体颜色 | Color`?` | `null` |
||||
**ShowSubBack** | 显示子菜单背景 | bool | false |
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**DropIconGap** | 下拉图标边距比例 | float | 0.25 |
**DropIconRatio** | 下拉图标比例 | float | 0.7 |
**IconGap** | 图标与文字间距比例 | int`?` | `null` |
**IconRatio** | 图标比例 | float | 1.2F |
||||
**AutoCollapse** | 自动折叠 | bool | false |
**Mode** | 菜单类型 | [TMenuMode](Enum.md#tmenumode) | Inline |
**MouseRightCtrl** | 鼠标右键控制 | bool | true |
**PauseLayout** | 暂停布局 | bool | false |
**Trigger** | 触发下拉的行为 | [Trigger](Enum.md#trigger) | Hover |
**UseMnemonic** | 如果为 true，则文本中前面有(&)号的第一个字符将用作菜单项的助记键 | bool | true |
**FocusMode** | 焦点模式 | [TFocusMode](Enum.md#tfocusmode) | None |
**FocusModeAlign** | 焦点模式方向 | [TAlignMini](Enum.md#talignmini) | None |
**FocusModeColor** | 焦点模式颜色 | Color`?` | `null` |
**FocusModeSize** | 焦点模式大小 | int | 2 |
**FocusModeSizeRatio** | 焦点模式大小比例 | float`?` | `null` |
||||
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
||||
**ArrowRatio** | 箭头比例 | float`?` | `null` |
**Gap** | 间距 | int`?` | `null` |
**HorizontalAlign** | 水平对齐 `仅水平模式且未出现滚动条时生效，None 为居中` | [TOrientation](Enum.md#torientation) | Left |
**Indent** | 常规缩进 `和Tree那样缩进` | bool | false |
**InlineIndent** | 缩进宽度 | int`?` | `null` |
**itemMargin** | 菜单项外间距 | int`?` | `null` |
**ScrollBarBlock** | 滚动条遮挡 | bool | false |
**DropDownOffset** | 下拉菜单偏移量 | Size | 0 × 0 |
**DropDownPadding** | 下拉边距 | Size | 16 × 10 |
||||
**Collapsed** | 是否折叠 | bool | false |
**Theme** | 色彩模式 (已过时，使用 ColorScheme) | [TAMode](Enum.md#tamode) | Auto |
**TooltipConfig** | 超出文字提示配置 | TooltipConfig`?` | `null` |
**Unique** | 只保持一个子菜单的展开 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectChanged** | Select 属性值更改时发生 | void | [MenuItem](#menuitem) item `项` |
**ItemClick** | 点击项时发生 | void | [MenuItem](#menuitem) item `项` |
**SelectChanging** | Select 属性值更改前发生 | bool | [MenuItem](#menuitem) item `项` |
**CustomButtonClick** | 自定义按钮点击时发生 | void | MenuButton button `按钮`, [MenuItem](#menuitem) item `项` |
**NonItemClick** | 空白项目点击事件 | void |
**ItemPaintBegin** | 绘制项之前发生 | void | object sender, MenuItemPaintBeginEventArgs e |
**ItemPaint** | 绘制项时发生 | void | object sender, MenuItemPaintEventArgs e |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectIndex** | 选中第一层 | void | int index `序号`, bool focus `设置焦点` = true |
**SelectIndex** | 选中第二层 | void | int i1 `序号1` , int i2 `序号2`, bool focus `设置焦点` = true |
**SelectIndex** | 选中第三层 | void | int i1 `序号1` , int i2 `序号2`  , int i3 `序号3`, bool focus `设置焦点` = true |
||||
**Select** | 选中菜单 | void | MenuItem item `项`, bool focus `设置焦点` = true |
**Remove** | 移除菜单 | void | MenuItem item `项` |
**USelect** | 取消全部选择 | void | 无 |
**HitTest** | 命中测试 | MenuItem`?` | int x `X坐标`, int y `Y坐标` |
**GetSelectIndex** | 获取选中项索引 | int | MenuItem item `项` |
**FindID** | 根据节点id查询节点 | MenuItem`?` | string id `ID` |
**FindName** | 根据节点name查询节点 | MenuItem`?` | string name `名称` |
**Focus** | 设置焦点 | void | MenuItem menuItem `菜单项`, bool force `强制` = false |


### 数据

#### MenuItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | 名称 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
**Text** | 文本 | string | `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
||||
**Font** | 自定义字体 | Font`?` | `null` |
**BadgeFore** | 徽标前景颜色 | Color`?` | `null` |
**FontSub** | 自定义子文本字体 | Font`?` | `null` |
||||
**BadgeBack** | 徽标背景颜色 | Color`?` | `null` |
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string | `null` |
**IconActive** | 图标激活 | Image`?` | `null` |
**IconActiveSvg** | 图标激活SVG | string | `null` |
||||
**Enabled** | 禁用状态 | bool | true |
**Visible** | 是否显示 | bool | true |
**BadgeMode** | 徽标模式（镂空） | bool | false |
**CanExpand** | 是否可以展开 | bool | `只读` |
**Select** | 是否选中 | bool | false |
||||
**BadgeBorderColor** | 徽标边框颜色 | Color`?` | `null` |
**BadgeBorderWidth** | 徽标边框宽度 | float`?` | `null` |
||||
**BadgeAlign** | 徽标方向 | [TAlign](Enum.md#talign) | Right |
**BadgeOffsetX** | 徽标偏移X | int | 1 |
**BadgeOffsetY** | 徽标偏移Y | int | 1 |
**BadgeSize** | 徽标比例 | float | .6F |
**PARENTITEM** | 父级对象 | [MenuItem](#menuitem)`?` | `null` |
**Reverse** | 反向 `仅最外层生效，水平贴右、垂直贴底且展开朝上` | bool | false |
||||
**Badge** | 徽标内容 | string`?` | `null` |
**BadgeSvg** | 徽标SVG | string`?` | `null` |
**Button** | 自定义按钮的集合 | List<MenuButton>`?` |  |
**Expand** | 展开 | bool | true |
**RightText** | 右侧文本（快捷键） | string`?` | `null` |
🌏 **LocalizationRightText** | 右侧文本 | string`?` | `null` |
**Sub** | 子集合 ♾️ | [MenuItem[]](#menuitem) | [] |
**SubText** | 子文本 | string`?` | `null` |
🌏 **LocalizationSubText** | 子文本 | string`?` | `null` |

#### MenuButton

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ID** | ID | string`?` | `null` |
**Name** | 名称 | string`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |
||||
**BadgeFore** | 徽标前景颜色 | Color`?` | `null` |
||||
**BadgeBack** | 徽标背景颜色 | Color`?` | `null` |
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**IconActive** | 图标激活 | Image`?` | `null` |
**IconActiveSvg** | 图标激活SVG | string`?` | `null` |
||||
**Enabled** | 禁用状态 | bool | true |
**Visible** | 是否显示 | bool | true |
**BadgeMode** | 徽标模式（镂空） | bool | false |
||||
**BadgeBorderColor** | 徽标边框颜色 | Color`?` | `null` |
**BadgeBorderWidth** | 徽标边框宽度 | float`?` | `null` |
||||
**BadgeAlign** | 徽标方向 | [TAlign](Enum.md#talign) | Right |
**BadgeOffsetX** | 徽标偏移X | int | 1 |
**BadgeOffsetY** | 徽标偏移Y | int | 1 |
**BadgeSize** | 徽标比例 | float | .6F |
||||
**Badge** | 徽标内容 | string`?` | `null` |
**BadgeSvg** | 徽标SVG | string`?` | `null` |