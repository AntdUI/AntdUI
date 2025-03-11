[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Switch

Switch 开关 👚

> 开关选择器。

- 默认属性：Checked
- 默认事件：CheckedChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**ForeColor** | 文字颜色 | Color`?` | `null` |
**Fill** | 填充颜色 | Color`?` | `null` |
**FillHover** | 悬停颜色 | Color`?` | `null` |
||||
**Checked** | 选中状态 | bool | false |
**CheckedText** | 选中时显示的文本 | string`?` | `null` |
🌏 **LocalizationCheckedText** | 国际化文本 | string`?` | `null` |
**UnCheckedText** | 未选中时显示的文本 | string`?` | `null` |
🌏 **LocalizationUnCheckedText** | 国际化文本 | string`?` | `null` |
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
||||
**WaveSize** | 波浪大小 `点击动画` | int | 4 |
**Gap** | 按钮与边框间距 | int | 2 |
||||
**Loading** 🔴 | 加载中 | bool | false |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**CheckedChanged** | Checked 属性值更改时发生 | void | bool value `选中状态` |