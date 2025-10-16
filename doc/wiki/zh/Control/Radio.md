[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Radio

Radio 单选框 👚

> 单选框。

- 默认属性：Checked
- 默认事件：CheckedChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**AutoSize** | 自动大小 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**Fill** | 填充颜色 | Color`?` | `null` |
||||
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextAlign** 🔴 | 文本位置 | ContentAlignment | MiddleLeft |
**Checked** | 选中状态 | bool | false |
**AutoCheck** | 点击时自动改变选中状态 | bool | true |
||||
**RightToLeft** | 反向 | RightToLeft | No |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**CheckedChanged** | Checked 属性值更改时发生 | void | bool value `选中状态` |