[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Splitter

Splitter 分隔面板 👚

> 自由切分指定区域。继承于 [SplitContainer](https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/System/Windows/Forms/Layout/Containers/SplitContainer.cs)

- 默认属性：Text
- 默认事件：SplitterMoved

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**SplitterBack** | 滑块背景 | Color`?` | `null` |
**SplitterBackMove** | 滑块移动背景 | Color`?` | `null` |
**ArrowColor** | 箭头颜色 | Color`?` | `null` |
**ArrawColorHover** | 鼠标悬浮箭头颜色 | Color`?` | `null` |
**ArrawBackColor** | 箭头背景色 | Color`?` | `null` |
||||
**SplitterWidth** | 拆分器的粗细 | int | 4 |
**SplitterSize** | 滑块大小 | int | 20 |
**SplitterDistance** | 拆分器与左边缘或上边缘的距离 | int | |
**Panel1MinSize** | 拆分器与 Panel1 的左边缘或上边缘的最小距离 | int | 25 |
**Panel2MinSize** | 拆分器与 Panel2 的右边缘或下边缘的最小距离 | int | 25 |
||||
**CollapsePanel** | 点击后收起的Panel | ADCollapsePanel | None |
**Orientation** | 拆分器是水平的还是垂直的 | Orientation | Vertical |
**SplitPanelState** | 当前折叠状态 | bool | true |
**Lazy** | 延时渲染 | bool | true |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**Collapse** | 折叠 | void | |
**Expand** | 展开 | void | |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SplitPanelStateChanged** | SplitPanelState 属性值更改时发生 | void | bool value `当前值` |