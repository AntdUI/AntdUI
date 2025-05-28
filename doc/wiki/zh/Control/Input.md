[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Input

Input 输入框 👚

> 通过鼠标或键盘输入内容，是最基础的表单域的包装。

- 默认属性：Text
- 默认事件：TextChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**BackColor** | 背景颜色 | Color`?` | `null` |
**BackExtend** | 背景渐变色 | string`?` | `null` |
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**BackgroundImage** | 背景图片 | Image`?` | `null` |
**BackgroundImageLayout** | 背景图片布局 | [TFit](Enum.md#tfit) | Fill |
||||
**BorderWidth** | 边框宽度 | float | 0F |
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderHover** | 悬停边框颜色 | Color`?` | `null` |
**BorderActive** | 激活边框颜色 | Color`?` | `null` |
||||
**SelectionColor** | 选中颜色 | Color | 102, 0, 127, 255 |
||||
**CaretColor** | 光标颜色 | Color`?` | `null` |
**CaretSpeed** | 光标速度 | int | 1000 |
||||
**WaveSize** | 波浪大小 `点击动画` | int | 4 |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
**Status** | 设置校验状态 | [TType](Enum.md#ttype) | None |
||||
**AllowClear** | 支持清除 | bool | false |
**AutoScroll** | 显示滚动条 | bool | false |
**Text** | 文本 | string ||
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**ImeMode** | IME(输入法编辑器)状态 | ImeMode | NoControl |
**EmojiFont** | Emoji字体 | string | Segoe UI Emoj |
**AcceptsTab** | 多行编辑是否允许输入制表符 | bool | false |
**Multiline** | 多行文本 | bool | false |
**LineHeight** | 多行行高 | int | 0 |
**ReadOnly** | 只读 | bool | false |
**PlaceholderText** | 水印文本 | string`?` | `null` |
🌏 **LocalizationPlaceholderText** | 国际化水印文本 | string`?` | `null` |
**PlaceholderColor** | 水印颜色 | Color`?` | `null` |
**PlaceholderColorExtend** | 水印渐变色 | string`?` | `null` |
**LostFocusClearSelection** | 焦点离开清空选中 | bool | true |
**HandShortcutKeys** 🔴 | 处理快捷键 | bool | true |
||||
**TextAlign** | 文本对齐方向 | HorizontalAlignment | Left |
**UseSystemPasswordChar** | 使用密码框 | bool | false |
**PasswordChar** | 自定义密码字符 | char | (char)0 |
**PasswordCopy** | 密码可以复制 | bool | false |
**PasswordPaste** | 密码可以粘贴 | bool | true |
**MaxLength** | 文本最大长度 | int | 32767 |
||||
**IconRatio** | 图标比例 | float | 0.7F |
**IconGap** | 图标与文字间距比例 | float | 0.25F |
**Prefix** | 前缀 | Image`?` | `null` |
**PrefixFore** | 前缀前景色 | Color`?` | `null` |
**PrefixSvg** | 前缀SVG | string`?` | `null` |
**PrefixText** | 前缀文本 | string`?` | `null` |
||||
**Suffix** | 后缀 | Image`?` | `null` |
**SuffixFore** | 后缀前景色 | Color`?` | `null` |
**SuffixSvg** | 后缀SVG | string`?` | `null` |
**SuffixText** | 后缀文本 | string`?` | `null` |
||||
**JoinLeft** | 连接左边 `组合按钮` | bool | false |
**JoinRight** | 连接右边 `组合按钮` | bool | false |
||||
**RightToLeft** | 反向 | RightToLeft | No |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**AppendText** | 将文本追加到当前文本中 | void | string text `追加的文本` |
**Clear** | 清除所有文本 | void ||
**ClearUndo** | 清除撤消缓冲区信息 | void ||
**Copy** | 复制 | void ||
**Cut** | 剪贴 | void ||
**Paste** | 粘贴 | void ||
**Undo** | 撤消 | void ||
**Select** | 文本选择范围 | void | int start `第一个字符的位置`, int length `字符长度` |
**SelectAll** | 选择所有文本 | void ||
**DeselectAll** | 取消全部选中 | void ||
**ScrollToCaret** | 内容滚动到当前插入符号位置 | void ||
**ScrollToEnd** | 内容滚动到最下面 | void ||

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**PrefixClick** | 前缀 点击时发生 | void | MouseEventArgs e |
**SuffixClick** | 后缀 点击时发生 | void | MouseEventArgs e |
**ClearClick** 🔴 | 清空 点击时发生 | void | MouseEventArgs e |
**VerifyChar** 🔴 | 验证字符时发生 | void | char Char `输入字符`,string? ReplaceText `替换文本`, bool Result `验证结果`  |
**VerifyKeyboard** 🔴 | 验证键盘时发生 | void | Keys KeyData, bool Result `验证结果` |

### Input 拦截字符串

> 不是通过 `KeyPress` 而是 `VerifyChar` 或重写 `Verify`

> 下面是模仿 [InputNumber](#inputnumber) 实现仅数字输入

``` csharp
private void Input1_VerifyChar(object sender, AntdUI.InputVerifyCharEventArgs e)
{
    NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
    string decimalSeparator = numberFormatInfo.NumberDecimalSeparator,
        groupSeparator = numberFormatInfo.NumberGroupSeparator, negativeSign = numberFormatInfo.NegativeSign;
    string keyInput = e.Char.ToString();
    if (char.IsDigit(e.Char))
    {
        e.Result = true; // 数字可以
    }
    else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) || keyInput.Equals(negativeSign))
    {
        e.Result = true; // 小数分隔符可以
    }
    else if (e.Char == '\b')
    {
        e.Result = true; // Backspace键可以
    }
    else if (e.Char == '。')
    {
        e.ReplaceText = ".";
        e.Result = true; // 中文句号替换为英文句号
    }
    else
    {
        e.Result = false;
    }
}
```

***


## InputNumber

InputNumber 数字输入框 👚

> 通过鼠标或键盘，输入范围内的数值。继承于 [Input](#input)

- 默认属性：Value
- 默认事件：ValueChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Minimum** | 最小值 | decimal`?` | `null` |
**Maximum** | 最大值 | decimal`?` | `null` |
**Value** | 当前值 | decimal | 0 |
||||
**ShowControl** | 显示控制器 | bool | true |
**DecimalPlaces** | 显示的小数点位数 | int | 0 |
**ThousandsSeparator** | 是否显示千分隔符 | bool | false |
**Hexadecimal** | 值是否应以十六进制显示 | bool | false |
**InterceptArrowKeys** | 当按下箭头键时，是否持续增加/减少 | bool | true |
**Increment** | 每次单击箭头键时增加/减少的数量 | decimal | 1 |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ValueChanged** | Value 属性值更改时发生 | void | decimal value `当前值` |