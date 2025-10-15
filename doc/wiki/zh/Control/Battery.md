[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Battery

Battery 电量 👚

> 展示设备电量。

- 默认属性：Value
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
**Radius** | 圆角 | int | 4 |
**DotSize** | 点大小 | int | 8 |
**Value** | 进度条 | int | 0 |
||||
**ShowText** | 显示文本 | bool | true |
**FillFully** | 满电颜色 | Color | 0, 210, 121 |
**FillWarn** | 警告电量颜色 | Color | 250, 173, 20 |
**FillDanger** | 危险电量颜色 | Color | 255, 77, 79 |
**ValueWarn** 🔴 | 警告电量阈值 | int | 30 |
**ValueDanger** 🔴 | 危险电量阈值 | int | 20 |