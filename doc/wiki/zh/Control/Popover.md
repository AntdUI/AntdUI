[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Popover

Popover 气泡卡片

> 弹出气泡式的卡片浮层。

### Popover.Config

> 配置气泡卡片

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Control** | 所属控件 | Form | `必填` |
**Title** | 标题 | string |`null`|
**Content** | 控件/内容 | object |`必填`|
**Font** | 字体 | Font |`null`|
**AutoClose** |自动关闭时间（秒）`0等于不关闭` | int |0|
**Radius** | 圆角 | int | 6 |
**ArrowAlign** | 箭头方向 | [TAlign](Enum.md#talign) | Bottom |
**ArrowSize** | 箭头大小 | int | 8 |
**Offset** | 偏移量 | Rectangle / RectangleF | `null` |
**CustomPoint** 🔴 | 自定义位置 | Rectangle`?` |`null`|
**Focus** 🔴 | 获取焦点 | bool | true |
**Tag** | 用户定义数据 | object`?` | `null` |
||||
**OnControlLoad** | 控件显示后回调 | `Action?` | `null` |

### Popover.TextRow

> 多列文本 `使用 数组/集合 给到 Content` 
> 用于单行显示多个色彩的文本或交互链接 ![TextRow](Popover.TextRow.png)
> ```csharp
> AntdUI.Popover.open(button1, new AntdUI.Popover.TextRow[] {
>     new AntdUI.Popover.TextRow("您有"),
>     new AntdUI.Popover.TextRow("3", 2, AntdUI.Style.Db.Primary),
>     new AntdUI.Popover.TextRow("条新零售待确认订单等待处理"),
>     new AntdUI.Popover.TextRow("查看", 2, AntdUI.Style.Db.Primary) {
>         Call = () => {
>             MessageBox.Show("点击查看");
>         }
>     },
> }, AntdUI.TAlign.BL);
> ```

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文字 | string | `必填` |
**Gap** | 间距 `左右间距` | int | 0 |
||||
**Fore** | 文字颜色 | Color`?` | `null` |
**Font** | 字体 | Font`?` | `null` |
||||
**Call** | 点击回调 `设置后鼠标悬停可点击` | Action | `null` |

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