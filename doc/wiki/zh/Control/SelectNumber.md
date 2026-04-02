[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## SelectNumber

SelectNumber 数字选择器 👚

> 数字下拉选择器，支持自定义范围、步长和格式化。

- 默认属性：Value
- 默认事件：SelectedIndexChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Value** | 当前值 | decimal | 0 |
**Minimum** | 最小值 | decimal`?` | `null` |
**Maximum** | 最大值 | decimal`?` | `null` |
**Increment** | 每次单击箭头键时增加/减少的数量 | decimal | 1 |
**DecimalPlaces** | 显示的小数点位数 | int | 0 |
**ThousandsSeparator** | 是否显示千分隔符 | bool | false |
**Hexadecimal** | 值是否应以十六进制显示 | bool | false |
**ShowControl** | 显示控制器 | bool | true |
**WheelModifyEnabled** | 鼠标滚轮修改值 | bool | true |
**InterceptArrowKeys** | 当按下箭头键时，是否持续增加/减少 | bool | true |
**EnabledValueTextChange** | 文本改变时是否更新Value值 | bool | false |
**ReadOnly** | 只读 | bool | false |
**Text** | 文本 | string ||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
**BackExtend** | 背景渐变色 | string`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderHover** | 悬停边框颜色 | Color`?` | `null` |
**BorderActive** | 激活边框颜色 | Color`?` | `null` |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
**Status** | 设置校验状态 | [TType](Enum.md#ttype) | None |
**Variant** | 形态 | [TVariant](Enum.md#tvariant) | Outlined |
**IconRatio** | 图标比例 | float | 0.7F |
**IconRatioRight** | 右图标比例 | float`?` | `null` |
**IconGap** | 图标与文字间距比例 | float | 0.25F |
**PaddGap** | 边框间距比例 | float | 0.4F |
**Prefix** | 前缀 | Image`?` | `null` |
**PrefixFore** | 前缀前景色 | Color`?` | `null` |
**PrefixSvg** | 前缀SVG | string`?` | `null` |
**PrefixText** | 前缀文本 | string`?` | `null` |
**HasPrefix** | 是否包含前缀 | bool | `false` |
**Suffix** | 后缀 | Image`?` | `null` |
**SuffixFore** | 后缀前景色 | Color`?` | `null` |
**SuffixSvg** | 后缀SVG | string`?` | `null` |
**SuffixText** | 后缀文本 | string`?` | `null` |
**HasSuffix** | 是否包含后缀 | bool | `false` |
**JoinMode** | 组合模式 | [TJoinMode](Enum.md#tjoinmode) | None |
**RightToLeft** | 反向 | RightToLeft | No |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | decimal `value` |
**ValueFormatter** | 格式化数值以供显示 | string | decimal `value` |
**SelectedIndexChanged** | 选中项索引改变时发生 | void | EventArgs `e` |
**PrefixClick** | 前缀 点击时发生 | void | MouseEventArgs `e` |
**SuffixClick** | 后缀 点击时发生 | void | MouseEventArgs `e` |
**ClearClick** | 清空 点击时发生 | void | MouseEventArgs `e` |
**DrawItem** | 绘制项时发生 | void | DrawItemEventArgs `e` |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**Focus** | 设置焦点 | void ||
**Clear** | 清空文本 | void ||

### 示例

```csharp
// 基本使用
selectNumber1.Minimum = 0;
selectNumber1.Maximum = 100;
selectNumber1.Increment = 5;

// 小数设置
selectNumber2.DecimalPlaces = 2;
selectNumber2.Increment = 0.1M;

// 十六进制显示
selectNumber3.Hexadecimal = true;
selectNumber3.Minimum = 0;
selectNumber3.Maximum = 255;

// 自定义格式化
selectNumber4.ValueFormatter += (sender, e) =>
{
	return $"{e.Value}%";
};

// 事件处理
selectNumber1.ValueChanged += (sender, e) =>
{
	MessageBox.Show($"当前值: {e.Value}");
};
```