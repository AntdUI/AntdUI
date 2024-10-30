[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## Drawer

Drawer 抽屉

> 屏幕边缘滑出的浮层面板。

### Drawer.Config

> 配置抽屉

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Form** | 所属窗口 | Form | `必填` |
**Content** | 控件 | Control | `必填` |
**Mask** | 是否展示遮罩 | bool | true |
**MaskClosable** | 点击蒙层是否允许关闭 | bool | true |
**Padding** | 边距 | int | 24 |
**Align** | 方向 | [TAlignMini](Enum#talignmini) | Right |
**Dispose** 🔴 | 是否释放 | bool | true |
**Tag** | 用户定义数据 | object`?` | `null` |
**OnLoad** 🔴 | 加载回调 | Action`?` | `null` |
**OnClose** 🔴 | 关闭回调 | Action`?` | `null` |