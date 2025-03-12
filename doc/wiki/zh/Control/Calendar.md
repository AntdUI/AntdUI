[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Calendar

Calendar 日历 👚

> 按照日历形式展示数据的容器。

- 默认属性：Date
- 默认事件：DateChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
||||
**Radius** | 圆角 | int | 6 |
||||
**Full** | 是否撑满 | bool | false |
**ShowChinese** | 显示农历 | bool | false |
**ShowButtonToDay** | 显示今天 | bool | true |
||||
**Value** | 控件当前日期 | DateTime | `DateTime.Now` |
**MinDate** | 最小日期 | DateTime`?` | `null` |
**MaxDate** | 最大日期 | DateTime`?` | `null` |

### 日期上的徽标

~~~ csharp
BadgeAction = dates =>
{
    // dates 参数为 DateTime[] 数组长度固定为2，返回UI上显示的开始日期与结束日期
    // DateTime start_date = dates[0], end_date = dates[1];
    var now = dates[1];
    return new List<AntdUI.DateBadge> {
        new AntdUI.DateBadge(now.ToString("yyyy-MM-dd"),0,Color.FromArgb(112, 237, 58)),
        new AntdUI.DateBadge(now.AddDays(1).ToString("yyyy-MM-dd"),5),
        new AntdUI.DateBadge(now.AddDays(-2).ToString("yyyy-MM-dd"),99),
        new AntdUI.DateBadge(now.AddDays(-6).ToString("yyyy-MM-dd"),998),
    };
};
~~~

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**LoadBadge** | 加载徽标 | void | |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**DateChanged** | 日期 改变时发生 | void | DateTime value `控件当前日期` |