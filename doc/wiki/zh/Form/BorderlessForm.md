[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

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
**ShowInTaskbar** | 确定窗体是否出现在 Windows 任务栏中 | bool | true |
||||
**UseDwm** | 使用DWM阴影 `使用系统阴影后颜色、边框、圆角等不生效` | bool | true |
**Shadow** | 阴影大小 | int | 10 |
**ShadowColor** | 阴影颜色 | Color | 100, 0, 0, 0 |
**ShadowPierce** | 鼠标穿透 | bool | false |
||||
**BorderWidth** | 边框宽度 | int | 1 |
**BorderColor** | 边框颜色 | Color | 180, 0, 0, 0 |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**DraggableMouseDown** | 拖动窗口 | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool | point: 客户端坐标 |
**MaxRestore** | 最大化/还原窗口 | bool ||
**Max** | 最大化窗口 | void ||
**FullRestore** | 全屏/还原窗口 | bool ||
**Full** | 全屏窗口 | void ||
**NoFull** | 退出全屏 | void ||
**RefreshDWM** | 刷新DWM区域 | void ||