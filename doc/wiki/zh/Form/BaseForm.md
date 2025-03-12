[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## BaseForm

支持DPI的基础原生窗口

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**AutoHandDpi** | 自动处理DPI | bool | true |
**Dark** | 深色模式 | bool | false |
**Mode** | 色彩模式 | [TAMode](../Control/Enum.md#tamode) | Auto |
**IsMax** 🔴 | 是否最大化 | bool | false |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**Min** | 最小化 | void ||
**Max** | 最大化 | void ||
**MaxRestore** | 最大化/还原 | void ||
**FullRestore** | 全屏/还原 | void ||
**Full** | 全屏 | void ||
**NoFull** | 取消全屏 | void ||
||||
**Dpi** | 获取DPI | float ||
**AutoDpi** | 处理DPI | void | Control control `控件` |
**AutoDpi** | 处理DPI | void | float dpi, Control control `控件` |
||||
**DraggableMouseDown** | 拖动窗口 | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||