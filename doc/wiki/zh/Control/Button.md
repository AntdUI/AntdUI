[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Button

Button 按钮 👚

> 按钮用于开始一个即时操作。

- 默认属性：Text
- 默认事件：Click

### 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Text** | 文本 | string`?` | `null` |
🌏 **LocalizationText** | 国际化文本 | string`?` | `null` |
**Type** | 类型 | [TTypeMini](Enum.md#ttypemini) | Default |
**TextAlign** | 文本位置 | ContentAlignment | MiddleCenter |
**TextMultiLine** | 是否多行 | bool | false |
||||
**ForeColor** | 文字颜色 | Color`?` | `null` |
**ForeHover** | 悬停文字颜色 | Color`?` | `null` |
**ForeActive** | 激活文字颜色 | Color`?` | `null` |
||||
**BackColor** | 背景颜色 | Color`?` | `null` |
**BackExtend** | 背景渐变色 | string`?` | `null` |
**BackgroundImage** | 背景图片 | Image`?` | `null` |
**BackgroundImageLayout** | 背景图片布局 | [TFit](Enum.md#tfit) | Fill |
**DefaultBack** | Default模式背景颜色 | Color`?` | `null` |
**OriginalBackColor** | 原装背景颜色 | Color | Transparent |
**BackHover** | 悬停背景颜色 | Color`?` | `null` |
**BackActive** | 激活背景颜色 | Color`?` | `null` |
||||
**Icon** | 图标 | Image`?` | `null` |
**IconSvg** | 图标SVG | string`?` | `null` |
**IconGap** | 图标与文字间距比例 | float | 0.25F |
**IconPosition** | 按钮图标组件的位置 | [TAlignMini](Enum.md#talignmini) | Left |
**IconRatio** | 图标比例 | float | 0.7F |
**IconSize** | 图标大小 `不设置为自动大小` | Size | 0 × 0 |
**TextCenterHasIcon** | 文本居中显示(包含图标后) | bool | false |
**IconHover** | 悬停图标 | Image`?` | `null` |
**IconHoverAnimation** | 悬停图标动画时长 | int | 200 |
**IconHoverSvg** | 悬停图标SVG | string`?` | `null` |
||||
**WaveColor** | 波浪颜色 | Color`?` | `null` |
**WaveShadow** | 波浪投影 | bool | false |
**WaveShadowSize** | 波浪投影大小 | int`?` | `null` |
**WaveSize** | 波浪大小 `点击动画` | int | 4 |
**WaveColorExtend** | 波浪渐变色 | string`?` | `null` |
||||
**AutoSize** | 自动大小 | bool | false |
**AutoEllipsis** | 文本超出自动处理 | bool | false |
**AutoSizeMode** | 自动大小模式 | [TAutoSize](Enum.md#tautosize) | None |
**DisplayStyle** | 指定显示图像还是文本 | [TButtonDisplayStyle](Enum.md#tbuttondisplaystyle) | Default |
**Ghost** | 幽灵属性 `使按钮背景透明` | bool | false |
**IsLink** | 箭头链接样式 | bool | false |
**RespondRealAreas** | 响应真实区域 | bool | false |
**ShowArrow** | 显示箭头 | bool | false |
**UseMnemonic** | 助记键 `前面有(&)号的第一个字符将用作按钮的助记键` | bool | true |
||||
**BorderWidth** | 边框宽度 | float | 0F |
**Radius** | 圆角 | int | 6 |
**Shape** | 形状 | [TShape](Enum.md#tshape) | Default |
**DefaultBorderColor** | Default模式边框颜色 | Color`?` | `null` |
||||
**ArrowProg** | 箭头角度 | float | -1F |
**VirtualWidth** | 虚拟宽度 | int`?` | `null` |
||||
|**切换**||
**AutoToggle** | 点击时自动改变选中状态 | bool | false |
**Toggle** | 选中状态 | bool | false |
**ToggleText** | 切换文本 | string`?` | `null` |
🌏 **LocalizationToggleText** | 切换文本 | string`?` | `null` |
**ToggleIcon** | 切换图标 | Image`?` | `null` |
**ToggleIconSvg** | 切换图标SVG | string`?` | `null` |
**ToggleIconHover** | 切换悬停图标 | Image`?` | `null` |
**ToggleIconHoverSvg** | 切换悬停图标SVG | string`?` | `null` |
**IconToggleAnimation** | 图标切换动画时长 | int | 200 |
**ToggleFore** | 切换文字颜色 | Color`?` | `null` |
**ToggleForeHover** | 切换悬停文字颜色 | Color`?` | `null` |
**ToggleForeActive** | 切换激活文字颜色 | Color`?` | `null` |
**ToggleType** | 切换类型 | [TTypeMini](Enum.md#ttypemini)`?` | `null` |
**ToggleBack** | 切换背景颜色 | Color`?` | `null` |
**ToggleBackExtend** | 切换背景渐变色 | string`?` | `null` |
**ToggleBackHover** | 切换悬停背景颜色 | Color`?` | `null` |
**ToggleBackActive** | 切换激活背景颜色 | Color`?` | `null` |
||||
|**加载**||
**Loading** | 加载状态 `true屏蔽点击` | bool | false |
**LoadingSpeedTime** | 加载速度(毫秒) | int | 1000 |
**LoadingIcon** | 加载图标 | Image`?` | `null` |
**LoadingSvg** | 加载图标SVG | string`?` | `null` |
**LoadingRespondClick** | 加载响应点击 | bool | false |
**LoadingValue** | 加载进度 `0F-1F` | float | 0.3F |
**LoadingWaveValue** | 水波进度 `0F-1F` | float | 0F |
**LoadingWaveColor** | 水波颜色 | Color`?` | `null` |
**LoadingWaveVertical** | 水波是否垂直 | bool | false |
**LoadingWaveSize** | 水波大小 | int | 2 |
**LoadingWaveCount** | 水波数量 | int | 1 |
||||
|**状态**||
**ExtraMouseDown** | 激活状态 | bool | false |
**ExtraMouseHover** | 悬停状态 | bool | false |
**HasFocus** | 是否存在焦点 | bool | false |
||||
|**组合**||
**JoinMode** | 组合模式 | [TJoinMode](Enum.md#tjoinmode) | None |
**JoinLeft** | 连接左边 | bool | false |
**JoinRight** | 连接右边 | bool | false |

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**AnimationBlink** | 开始闪烁动画 | void | int interval 间隔(毫秒), params Color[] colors 闪烁颜色 |
**AnimationBlinkTransition** | 开始闪烁动画（颜色过渡动画） | void | int interval 间隔(毫秒), params Color[] colors 闪烁颜色 |
**StopAnimationBlink** | 停止闪烁动画 | void | |

### 事件

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ToggleChanged** | Toggle 改变时发生 | void | |