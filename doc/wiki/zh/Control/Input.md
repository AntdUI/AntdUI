[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Input

Input 输入框 👚

> 通过鼠标或键盘输入内容，是最基础的表单域的包装。

- 默认属性：Text
- 默认事件：TextChanged

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string ||
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**TextAlign** | 文本对齐方向 | HorizontalAlignment | Left |
**TextTotalLine** | 文本总行数 | int | 0 |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**EmojiFont** | Emoji字体 | string`?` | `null` |
**PrefixFore** | 前缀前景色 | Color`?` | `null` |
**SuffixFore** | 后缀前景色 | Color`?` | `null` |
||||
**BackColor** | 背景颜色 | Color`?` | `null` |
**BackExtend** | 背景渐变色 | string`?` | `null` |
**BackgroundImage** | 背景图片 | Image`?` | `null` |
**BackgroundImageLayout** | 背景图片布局 | [TFit](Enum.md#tfit) | Fill |
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
||||
**IconGap** | 图标与文字间距比例 | float | 0.25F |
**IconRatio** | 图标比例 | float | 0.7F |
**IconRatioRight** | 右图标比例 | float`?` | `null` |
**LoadingIcon** | 加载图标 | Image`?` | `null` |
||||
**LoadingWaveColor** | 水波颜色 | Color`?` | `null` |
**LoadingWaveCount** | 水波数量 | int | 1 |
**LoadingWaveSize** | 水波大小 | int | 2 |
**LoadingWaveValue** | 水波进度 | float | 0F |
**LoadingWaveVertical** | 水波是否垂直 | bool | false |
**WaveSize** | 波浪大小 `点击动画` | int | 4 |
||||
**Loading** | 加载状态 | bool | false |
**LoadingSpeedTime** | 加载速度(毫秒) | int | 1000 |
**LoadingSvg** | 加载图标SVG | string`?` | `null` |
**LoadingValue** | 加载进度 | float | 0.3F |
||||
**ReadOnly** | 只读 | bool | false |
**AllowClear** | 支持清除 | bool | false |
**AutoScroll** | 显示滚动条 | bool | false |
**CaretVisible** | 确定光标是可见的还是隐藏的 | bool | true |
**HideSelection** | 失去焦点时是否隐藏选定内容 | bool | true |
**ImeMode** | IME(输入法编辑器)状态 | ImeMode | NoControl |
**JoinMode** | 组合模式 | [TJoinMode](Enum.md#tjoinmode) | None |
**SelectionColor** | 选中颜色 | Color | 102, 0, 127, 255 |
**SetTextSelectionEnd** | 设置文本后选中到最后 | bool | false |
**UseContextMenu** | 使用默认右键菜单 | bool | true |
**UseSystemPasswordChar** | 使用密码框 | bool | false |
**VirtualMode** | 虚拟模式 | bool | false |
**BorderHover** | 悬停边框颜色 | Color`?` | `null` |
**BorderActive** | 激活边框颜色 | Color`?` | `null` |
**LostFocusClearSelection** | 焦点离开清空选中 | bool | true |
||||
**BorderColor** | 边框颜色 | Color`?` | `null` |
**BorderWidth** | 边框宽度 | float | 1F |
**Radius** | 圆角 | int | 6 |
**Round** | 圆角样式 | bool | false |
||||
**AcceptsTab** | 多行编辑是否允许输入制表符 | bool | false |
**AdapterSystemMnemonic** | 适配系统助记词 | bool | false |
**LineHeight** | 多行行高 | int | 0 |
**Lines** | 多行文本集合 | string[] | 
**MaxLength** | 文本最大长度 | int | 32767 |
**Multiline** | 多行文本 | bool | false |
**PaddGap** | 边框间距比例 | float | 0.4F |
**PaddingLR** | 左右边距 | int | 0 |
**PrefixWidth** | 前缀宽度 | int`?` | `null` |
**SuffixWidth** | 后缀宽度 | int`?` | `null` |
**WordWrap** | 自动换行 | bool | true |
||||
**AcceptsEscape** | 多行编辑是否允许输入退出键 | bool | false |
**CaretColor** | 光标颜色 | Color`?` | `null` |
**CaretSpeed** | 光标速度 | int | 1000 |
**HandShortcutKeys** | 处理快捷键 `已过时` | bool | true |
**HasPrefix** | 是否包含前缀 | bool | `false` |
**HasSuffix** | 是否包含后缀 | bool | `false` |
**IsTextEmpty** | 文本是否为空 | bool | true |
**JoinLeft** | 连接左边 `组合按钮` `已过时` | bool | false |
**JoinRight** | 连接右边 `组合按钮` `已过时` | bool | false |
**PasswordChar** | 自定义密码字符 | char | (char)0 |
**PasswordCopy** | 密码可以复制 | bool | false |
**PasswordPaste** | 密码可以粘贴 | bool | true |
**PlaceholderColor** | 水印颜色 | Color`?` | `null` |
**PlaceholderText** | 水印文本 | string`?` | `null` |
🌏 **LocalizationPlaceholderText** | 国际化水印文本 | string`?` | `null` |
**Prefix** | 前缀 | Image`?` | `null` |
**PrefixFormat** | 前缀对齐方式 | FormatFlags | `FormatFlags.Center \| FormatFlags.NoWrap` |
**PrefixSvg** | 前缀SVG | string`?` | `null` |
**PrefixText** | 前缀文本 | string`?` | `null` |
🌏 **LocalizationPrefixText** | 国际化前缀文本 | string`?` | `null` |
**RightToLeft** | 反向 | RightToLeft | No |
**Status** | 设置校验状态 | [TType](Enum.md#ttype) | None |
**Suffix** | 后缀 | Image`?` | `null` |
**SuffixFormat** | 后缀对齐方式 | FormatFlags | `FormatFlags.Center \| FormatFlags.NoWrap` |
**SuffixSvg** | 后缀SVG | string`?` | `null` |
**SuffixText** | 后缀文本 | string`?` | `null` |
🌏 **LocalizationSuffixText** | 国际化后缀文本 | string`?` | `null` |
**Variant** | 形态 | [TVariant](Enum.md#tvariant) | Outlined |
**PlaceholderColorExtend** | 水印渐变色 | string`?` | `null` |
||||
|||

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**AppendText** | 将文本追加到当前文本中 | void | string text `追加的文本` |
**AppendText** | 追加文本到末尾 | void | string text `追加的文本`, TextOpConfig config `文本配置` |
**InsertText** | 在指定位置插入文本 | void | int startIndex `开始位置`, string text `文本`, TextOpConfig config `文本配置` |
**Clear** | 清除所有文本 | void ||
**ClearUndo** | 清除撤消缓冲区信息 | void ||
**Copy** | 复制 | void ||
**Cut** | 剪贴 | void ||
**Paste** | 粘贴 | void ||
**Undo** | 撤消 | void ||
**Redo** | 重做 | void ||
**Select** | 文本选择范围 | void | int start `第一个字符的位置`, int length `字符长度` |
**SelectAll** | 选择所有文本 | void ||
**SelectLast** | 选择最后一个字符 | void ||
**DeselectAll** | 取消全部选中 | void ||
**ScrollToCaret** | 内容滚动到当前插入符号位置 | void ||
**ScrollToEnd** | 内容滚动到最下面 | void ||
**ScrollLine** | 滚动到指定行 | void | int i `行索引` |
**EnterText** | 当前位置插入文本 | void | string text `文本`, bool ismax `是否限制MaxLength` |
**SetStyle** | 设置样式 | bool | int start `第一个字符的位置`, int length `字符长度`, Font? font `字体`, Color? fore `文本颜色`, Color? back `背景颜色` |
**SetStyle** | 设置样式 | bool | TextStyle style `文本样式`, bool rd `是否渲染` |
**ClearStyle** | 清空样式 | void ||
**GetSelectionText** | 获取当前选中文本 | string? ||
**SelectedText** | 获取设置当前选中文本 | string? ||
**IndexOf** | 查找指定字符串首次出现的位置 | int | string value `要查找的字符串` |
**IndexOf** | 从指定位置开始查找字符串首次出现的位置 | int | string value `要查找的字符串`, int startIndex `开始搜索的位置` |
**LastIndexOf** | 查找指定字符串最后一次出现的位置 | int | string value `要查找的字符串` |
**Substring** | 从指定位置开始截取子字符串 | string | int startIndex `开始截取的位置` |
**Substring** | 从指定位置开始截取指定长度的子字符串 | string | int startIndex `开始截取的位置`, int length `要截取的长度` |
||||
**AnimationBlink** | 开始闪烁动画 | void | int interval `动画间隔时长（毫秒）`, params Color[] colors `色彩值` |
**AnimationBlinkTransition** | 开始颜色过渡闪烁动画 | void | int interval `动画间隔时长（毫秒）`, params Color[] colors `色彩值` |
**AnimationBlinkTransition** | 开始颜色过渡闪烁动画 | void | int interval `动画间隔时长（毫秒）`, int transition_interval `过渡动画间隔时长（毫秒）`, AnimationType animationType `过渡动画类型`, params Color[] colors `色彩值` |
**StopAnimationBlink** | 停止闪烁动画 | void ||

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**PrefixClick** | 前缀 点击时发生 | void | MouseEventArgs e |
**SuffixClick** | 后缀 点击时发生 | void | MouseEventArgs e |
**ClearClick** | 清空 点击时发生 | void | MouseEventArgs e |
**VerifyChar** | 验证字符时发生 | void | char Char `输入字符`,string? ReplaceText `替换文本`, bool Result `验证结果`  |
**VerifyKeyboard** | 验证键盘时发生 | void | Keys KeyData, bool Result `验证结果` |

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