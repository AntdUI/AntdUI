[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Modal

Modal 对话框

> 模态对话框。

### Modal.Config

> 配置对话框

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Target** | 所属目标 | Target | `null` |
**Form** | 所属窗口 | Form`?` | `为空无法使用Mask` | `已废弃，使用 Target` |
**Title** | 标题 | string | `必填` |
**Content** | 控件/内容 | object | `必填` |
**Width** | 消息框宽度 | int | 416 |
**Font** | 字体 | Font`?` | `null` |
**Icon** | 图标 | [TType](Enum.md#ttype) | None |
**IconCustom** | 自定义图标 | IconInfo`?` | `null` |
**Keyboard** | 是否支持键盘 esc 关闭 | bool | true |
**Mask** | 是否展示遮罩 | bool | true |
**MaskClosable** | 点击蒙层是否允许关闭 | bool | true |
**CloseIcon** | 是否显示关闭图标 | bool | false |
**Tag** | 用户定义数据 | object`?` | `null` |
||||
**ContentPadding** | 内容边距 | Size | 0, 0 |
**UseIconPadding** | 使用图标边距 | bool | true |
**Padding** | 边距 | Size | 24, 20 |
**BtnHeight** | 按钮栏高度 | int | 38 |
**CancelText** | 取消按钮文字 | string | "取消" |
**CancelFont** | 取消按钮字体 | Font`?` | `null` |
**OkText** | 确认按钮文字 | string | "确定" |
**OkType** | 确认按钮类型 | [TTypeMini](Enum.md#ttypemini) | Primary |
**OkFont** | 确认按钮字体 | Font`?` | `null` |
**OnOk** | 确定回调 | `Func<Config, bool>?` | `null` |
||||
**Btns** | 自定义按钮 | [Btn[]](#modal.btn) | `null` |
**OnBtns** | 自定义按钮回调 | `Func<Button, bool>?` | `null` |
**OnButtonStyle** | 自定义按钮样式回调 | Action<string, [Button](Button)> | `null` |
||||
**LoadingDisableCancel** | 加载时禁用取消按钮 | bool | false |
**Draggable** | 拖拽窗口 | bool | true |
**EnableSound** | 启用声音 | bool | false |
**ManualActivateParent** | 关闭后手动激活父窗口 | bool | false |
**DefaultFocus** | 默认是否焦点 | bool | false |
**DefaultAcceptButton** | 默认接受OK按钮 | bool | true |
**ColorScheme** | 色彩模式 | [TAMode](Enum.md#tamode) | Auto |
**Resizable** | 是否可调整大小 | bool | false |
**MinimumSize** | 最小大小 | Size`?` | `null` |
**MaximumSize** | 最大大小 | Size`?` | `null` |
**Close()** | 主动关闭 | void | |
**DialogResult()** | 设置对话框结果 | void | |

### Modal.Btn

> 配置自定义按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 按钮名称 | string | `必填` |
**Text** | 按钮文字 | string | `必填` |
**Type** | 按钮类型 | [TTypeMini](Enum.md#ttypemini) | Default |
**Fore** | 文字颜色 | Color`?` | `null` |
**Back** | 背景颜色 | Color`?` | `null` |
**DialogResult** | 对话框结果 | DialogResult | None |
**Tag** | 用户定义数据 | object`?` | `null` |

### Modal.TextLine

> 多行文本

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文字 | string | `必填` |
**Gap** | 间距 | int | 0 |
**Fore** | 文字颜色 | Color`?` | `null` |
**Font** | 字体 | Font`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |

***

### UserControl 监控 Load 示例

~~~csharp
public partial class UserControl1 : UserControl, AntdUI.ControlEvent
{
    public void LoadCompleted()
    {
        System.Diagnostics.Debug.WriteLine("Load");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        System.Diagnostics.Debug.WriteLine("Close");
    }
}
~~~