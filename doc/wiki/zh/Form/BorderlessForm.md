[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)・[SVG](../SVG.md)

## BorderlessForm

无边框阴影窗口

> 基于 `FormBorderStyle.None` 实现的无边框阴影窗口。继承于 [BaseForm](BaseForm)

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Resizable** | 调整窗口大小 | bool | true |
**Dark** | 深色模式 | bool | false |
**Mode** | 色彩模式 | [TAMode](../Control/Enum.md#tamode) | Auto |
**Radius** | 圆角 | int | 0 |
||||
**UseDwm** 🔴 | 使用DWM阴影 `使用系统阴影后颜色、边框、圆角等不生效` | bool | true |
**Shadow** | 阴影大小 | int | 10 |
**ShadowColor** | 阴影颜色 | Color | 100, 0, 0, 0 |
**ShadowPierce** 🔴 | 鼠标穿透 | bool | false |
||||
**BorderWidth** | 边框宽度 | float | 0F |
**BorderColor** | 边框颜色 | Color | 246, 248, 250 |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**DraggableMouseDown** | 拖动窗口 | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||