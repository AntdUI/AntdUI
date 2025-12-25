[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Window

原生无边框窗口

> 拥有原生特性的完美无边框窗口。继承于 [BaseForm](BaseForm)

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Resizable** | 调整窗口大小 | bool | true |
**Dark** | 深色模式 | bool | false |
**Mode** | 色彩模式 | [TAMode](../Control/Enum.md#tamode) | Auto |
**ScreenRectangle** | 获取或设置窗体屏幕区域 | Rectangle |
**IsMax** | 是否最大化 | bool | false |
**AutoHandDpi** | 自动处理DPI | bool | true |
**DisableTheme** | 是否禁用主题 | bool | false |
**IsFull** | 是否全屏 | bool | false |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**DraggableMouseDown** | 拖动窗口 | void ||
**ResizableMouseDown** | 调整窗口大小（鼠标按下） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool ||
**ResizableMouseMove** | 调整窗口大小（鼠标移动） | bool | point: 客户端坐标 |
**Min** | 最小化 | void ||
**Max** | 最大化 | void ||
**MaxRestore** | 最大化/还原 | bool ||
**Full** | 全屏 | void ||
**NoFull** | 取消全屏 | void ||
**FullRestore** | 全屏/还原 | bool ||
**Dpi** | 获取DPI | float ||
**AutoDpi** | 处理DPI | void | control: 控件 |
**AutoDpi** | 处理DPI | void | dpi: DPI值, control: 控件 |
**Theme** | 获取主题配置 | ThemeConfig ||
**ThemeClear** | 清除主题配置 | void ||