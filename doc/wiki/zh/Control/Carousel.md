[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Carousel

Carousel 走马灯 👚

> 旋转木马，一组轮播的区域。

- 默认属性：Image
- 默认事件：SelectIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Touch** | 手势滑动 | bool | true |
**TouchOut** | 滑动到外面 | bool | false |
**Autoplay** | 自动切换 | bool | false |
**Autodelay** | 自动切换延迟(s) | int | 4 |
||||
**DotSize** | 面板指示点大小 | Size | 28 × 4 |
**DotMargin** | 面板指示点边距 | int | 12 |
**DotPosition** | 面板指示点位置 | [TAlignMini](Enum.md#talignmini) | None |
||||
**Radius** | 圆角 | int | 0 |
**Round** | 圆角样式 | bool | false |
||||
**Image** | 图片集合 `CarouselItem[]` | [CarouselItem[]](#carouselitem) | [] |
**ImageFit** | 图片布局 | [TFit](Enum.md#tfit) | Cover |
**SelectIndex** | 选择序号 | int | 0 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SelectIndexChanged** | SelectIndex 属性值更改时发生 | void | int index `序号` |

### 数据

#### CarouselItem

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Img** | 图片 | Image`?` | `null` |
**Tag** | 用户定义数据 | object`?` | `null` |