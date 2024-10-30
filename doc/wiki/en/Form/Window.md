[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)・[SVG](../SVG.md)

## Window

原生无边框窗口

> 拥有原生特性的完美无边框窗口。继承于 [BaseForm](BaseForm)

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Resizable** | 调整窗口大小 | bool | true |
**Dark** | 深色模式 | bool | false |
**Mode** | 色彩模式 | [TAMode](../Control/Enum#tamode) | Auto |
**ScreenRectangle** | 获取或设置窗体屏幕区域 | Rectangle |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**DraggableMouseDown** | 拖动窗口 | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||