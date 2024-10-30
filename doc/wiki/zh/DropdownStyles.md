[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)・[SVG](SVG.md)

## SelectItem

> 支持更丰富UI

名称 | 描述 | 类型 | 为空 | 默认值 |
:--|:--|:--|:--:|:--|
**Online** | 在线状态 `1为绿点，0为红点` | int`?` |✅| `null` |
**OnlineCustom** | 在线自定义颜色 | Color`?` |✅| `null` |
**Enable** | 是否启用 | bool |⛔| true |
**Icon** | 图标 | Image`?` |✅| `null` |
**IconSvg** | 图标SVG | string`?` |✅| `null` |
**Text** | 显示文本 | string |⛔| `必填` |
**SubText** | 显示子文本 | string`?` |✅| `null` |
**Sub** | 子选项 ♾️ | `List<object>?` |✅| `null` |
**Tag** | 原数据 | object |⛔| `必填` |
|||||
**TagFore** 🔴 | 标签文字颜色 | Color`?` |✅| `null` |
**TagBack** 🔴 | 标签背景颜色 | Color`?` |✅| `null` |
**TagBackExtend** 🔴 | 标签背景渐变色 | string`?` |✅| `null` |

## DividerSelectItem

> 分割线