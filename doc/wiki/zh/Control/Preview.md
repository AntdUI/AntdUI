[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Preview

Preview 图片预览

> 图片预览框。

### Preview.Config

> 配置图片预览

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Form** | 所属窗口 | Form | `必填` |
**Content** | 图片内容 | `IList<Image>` |`必填`|
**Tag** | 用户定义数据 | object`?` | `null` |
||||
**Btns** | 自定义按钮 | [Btn[]](#preview.btn) | `null` |
**OnBtns** | 自定义按钮回调 | Action<string, object?> | `null` |

### Preview.Btn

> 自定义按钮

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Name** | 按钮名称 | string | `必填` |
**IconSvg** | 图标SVG | string | `必填` |
||||
**Tag** | 用户定义数据 | object`?` | `null` |