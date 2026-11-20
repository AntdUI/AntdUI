[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

> 下拉选项的样式对象，用于 `Select`、`Combo` 等控件的 `Items`，支持自定义图标、在线状态、子文本、标签及主题色。

## SelectItem

> 支持更丰富UI

名称 | 描述 | 类型 | 为空 | 默认值 |
:--|:--|:--|:--:|:--|
**Online** | 在线状态 `1为绿点，0为红点` | int`?` |✅| `null` |
**OnlineCustom** | 在线自定义颜色 | Color`?` |✅| `null` |
**Enable** | 是否启用 | bool |⛔| true |
|||||
**Icon** | 图标 | Image`?` |✅| `null` |
**IconSvg** | 图标SVG | string`?` |✅| `null` |
**IconRatio** | 图标比例 | float`?` |✅| `null` |
|||||
**Text** | 显示文本 | string |⛔| `必填` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**SubText** | 显示子文本 | string`?` |✅| `null` |
🌏 **LocalizationSubText** | 国际化子文本 | string`?` | `null` |
**Sub** | 子选项 ♾️ | `List<object>?` |✅| `null` |
**MaxCount** | 子选项最大显示数量 | int |⛔| `0` |
**Tag** | 原数据 | object |⛔| `必填` |
|||||
**TagFore** 🔴 | 标签文字颜色 | Color`?` |✅| `null` |
**TagBack** 🔴 | 标签背景颜色 | Color`?` |✅| `null` |
**TagBackExtend** 🔴 | 标签背景渐变色 | string`?` |✅| `null` |
|||||
**Fore** 🔴 | 文字颜色 | Color`?` |✅| `null` |
**ForeSub** 🔴 | 子文字颜色 | Color`?` |✅| `null` |
**BackActive** 🔴 | 激活背景颜色 | Color`?` |✅| `null` |
**BackActiveExtend** 🔴 | 激活背景渐变色 | string`?` |✅| `null` |

### 链式设置方法

> 以下方法均返回 `SelectItem` 自身，可连续调用

名称 | 描述 | 参数
:--|:--|:--
**SetText** | 设置显示文本及国际化文本 | string value `文本`, string? localization = null `国际化文本` |
**SetSubText** | 设置子文本及国际化文本 | string? value `子文本`, string? localization = null `国际化子文本` |
**SetEnable** | 设置是否启用 | bool value = false |
**SetOnline** | 设置在线状态 | int? value = 1 `1为绿点，0为红点` |
**SetOnline** | 设置在线自定义颜色 | Color? value |
**SetOnline** | 同时设置在线状态与自定义颜色 | int? value, Color? color |
**SetIcon** | 设置图标 | Image? value |
**SetIcon** | 设置SVG图标 | string? svg |
**SetIcon** | 同时设置图标与SVG | Image? value, string? svg |
**SetIconRatio** | 设置图标比例 | float? value |
**SetSub** | 设置子选项 | params object[] value |
**SetSub** | 设置子选项最大显示数量（对应 MaxCount） | int value |
**SetFore** | 设置文字颜色 | Color? value |
**SetFore** | 同时设置文字与子文字颜色 | Color? value, Color? sub |
**SetForeSub** | 设置子文字颜色 | Color? value |
**SetBackActive** | 设置激活背景颜色 | Color? value |
**SetBackActive** | 设置激活背景渐变色 | string? value |
**SetTagFore** | 设置标签文字颜色 | Color? value |
**SetTagBack** | 设置标签背景颜色 | Color? value |
**SetTagBack** | 设置标签背景渐变色 | string? value |

#### 使用示例

```csharp
select.Items = new object[] {
	new AntdUI.SelectItem("张三", 1)
		.SetIcon(SvgDb.GetSvg("user"))
		.SetOnline(1)
		.SetSubText("在线 · 研发部")
		.SetTagBack("#E6F4FF")
		.SetFore("#1677FF"),
	new AntdUI.DividerSelectItem(),
	new AntdUI.SelectItem("李四", 2)
};
```

## DividerSelectItem

> 分割线（无属性，直接放入 Items 即可分隔选项）
