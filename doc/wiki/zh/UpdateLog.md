[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

### `2.2.10` 2026-01-08

- ➕ LabelLed
- ➕ Table MouseClickPenetration
- ➕ Table FilterRealTime
- ➕ Input string[] Lines
- ➕ ChatList ItemImageClick 图片点击事件
- ✍️ Popover 优化视觉流畅
- ✍️ Popover 边距
- ✍️ Table 点击选中行交互
- ✍️ Table 完善编辑模式与回车跳转
- ✍️ Table 总结栏未响应
- ✍️ Table 文字太窄不显示
- ✍️ Tooltip 优化交互
- ✍️ Tooltip 闪现
- ✍️ Button 悬浮过度动画
- ✍️ 窗口 关闭逻辑判断
- 🧹 Table 筛选

### `2.2.9` 2026-01-03

- ➕ ButtonShadow 阴影按钮
- ➕ ChatList EmojiRatio
- ➕ ChatList EnabledClickImage
- ➕ ChatList 图片缓存
- ➕ InputNumber EnabledValueTextChange
- ➕ DatePicker/TimePicker EnabledValueTextChange
- ✍️ Modal MaskClosable false 依旧可以关闭
- ✍️ Table FocusedCell 刷新表格后消失
- ✍️ Table 焦点与配置跳转交互冲突

### `2.2.7` 2026-01-01 🎇

- ➕ Tree 项 Checkable
- ➕ Tree/Menu 更细腻效率更高的展开动画
- ➕ ColorPicker DropDownFontRatio
- ➕ Input PaddGap
- ➕ 适配 AutoScaleMode.Dpi
- ✍️ Tree/Menu 打断动画
- ✍️ Tree BeforeExpand 覆盖面
- ✍️ Select 点击 Clear 弹出下拉
- ✍️ Table 下拉筛选适配插槽
- ✍️ DatePickerRange 开始日期删除后光标错位
- ✍️ Pagination RecordsPerPageText 无法重置
- ✍️ Drawer 抑制闪烁
- ✍️ 下拉框与父容器可见区域显示
- ✍️ Mask 残留、Modal 异常关闭
- ✍️ 事件触发顺序，防止多次触发
- ✍️ 助记键 在 UserControl 下无法识别
- ✍️ Mnemonic 与 Input 冲突
- 🧹 ColorPicker

### `2.2.6` 2025-12-25 🎄

- ➕ Chart 显示XY轴标签
- ➕ Segmented 边框宽度 边框颜色
- ➕ Config DropDownMarginFurther
- ✍️ 下拉框 在Win7下仅有滚动条才激活
- ✍️ Calendar 当天日期刷新问题
- ✍️ Popover 弹出 ShowDialog 交互后会导致关闭
- ✍️ FlowLayoutPanel 鼠标滚动
- ✍️ Mnemonic 与 Input 冲突
- ✍️ Transfer 设计器添加项不刷新
- ✍️ Chart Y 错位

### `2.2.5` 2025-12-19

- ➕ Table Column 拖放事件 ColumnIndexChanging, ColumnIndexChanged
- ✍️ NET4.0 NET4.6 控件错位

### `2.2.4` 2025-12-18 📐

> ⚠ NET4.0 NET4.6 控件错位

- ➕ 菜单展开安全三角区 `Amazon 2013`
- ➕ DPI 适配 PerMonitorV2 与 AutoScaleMode.Font
- ➕ Table SetSelected 选中行方法
- ➕ Table SummaryCustomize 属性，用于启用内置汇总定制功能，支持自定义汇总
- ➕ Table ScrollToEnd
- ➕ Table Summary 汇总功能增强
- ➕ Tree ForeSub
- ➕ Tree FindID/FindName 方法
- ➕ Input ScrollLine
- ➕ Input WordWrap
- ➕ Select AutoText
- ➕ Preview Fit
- ➕ Checkbox/Radio useMnemonic
- ➕ 丰富徽标 文本颜色和边框属性
- ✍️ Button MiddleLeft 错位
- ✍️ DatePickerRange 光标交互
- ✍️ Table ConfigureFocusNavigation 一列不生效
- ✍️ Table Tree 行排序
- ✍️ Table Fill 滚动条遮挡
- ✍️ Table ScrollLine 错位
- ✍️ Table 初始悬浮态
- ✍️ 优化跳转 Table 筛选合并
- ✍️ Tree 节点折叠打开后，同级节点 NodeMouseMove事件，`e.Item` 错误为折叠的子节点
- ✍️ Input 开放按键事件触发
- ✍️ Input 整理滚动条
- ✍️ Input 空回车交互
- ✍️ Input 取消键盘拦截
- ✍️ FloatButton Closing 关闭拦截后依旧消失
- ✍️ ContextMenuStrip 键盘上下交互
- ✍️ UseHook 默认启用
- 🧹 LayeredForm 消失判定

### `2.2.3` 2025-12-10

- ➕ ImagePreview 暴露操作按钮方法
- ➕ Table EditLostFocus
- ➕ Modal DefaultAcceptButton
- ➕ Calendar Back/Fore 颜色属性
- ➕ Input InsertText
- ✍️ PageHeader 标题栏、增强各方向圆角
- ✍️ Tree 项Text带换行的情况下重叠问题
- ✍️ Collapse 收缩后圆角缺角
- ✍️ Form 多次打开缩放
- ✍️ Dropdown Trigger.Hover 鼠标离开下拉不消失
- ✍️ Tabs Card 隐藏第一个后面头不显示
- ✍️ DatePickerRange 时间部分，无法选择
- ✍️ Menu 箭头精度
- ✍️ Menu 扩宽默认下拉边距
- ✍️ Input 向下查找
- ✍️ 优化 Input 多次渲染
- ✍️ Button TextMultiLine 换行不生效
- ✍️ Table 编辑模式错位
- ✍️ Table 复选框联动
- ✍️ Table 适配 int 复选框值
- ✍️ Menu 初始箭头方向
- ✍️ ImagePreview 图片初始错位
- ✍️ PageHeader 最小化按钮某字体不显示
- ✍️ DatePicker MaxDate 没有正确应用
- ✍️ 组合 Emoji 字素簇拆分异常
- 🧹 移除 PARENTITEM
- 🧹 重构动画

### `2.2.1` 2025-11-21

- ➕ Tree NodeMouseDown/NodeMouseUp
- ➕ Calendar 鼠标事件
- ➕ Button ToggleChanged
- ➕ Table CellButton Loading
- ➕ Config FocusBorderEnabled
- ➕ Modal UserControl 控制方法
- ➕ Config Resizable/Min/MaximumSize 等
- ➕ Spin/Message 适配 异步
- ✍️ OnClosing > OnFormClosing 导致窗口关闭被拦截
- ✍️ ColorPicker 下拉空异常
- ✍️ ColorPicker 单独 ShowReset 错位
- ✍️ ImagePreview 设计器异常
- ✍️ Popover Control 错位
- ✍️ 字符换行不居中
- ✍️ 移除多余的滚轮计算

### `2.2.0` 2025-11-14

> 🎉 NET 10.0

- ➕ Table 复选支持int 0,1
- ➕ Table 焦点自动跳转增加 自动选中行、自动滚动到新行
- ➕ Table BorderRenderMode 三种模式
- ➕ Table CellSwitch
- ➕ Table 丰富 EditSelection
- ➕ DatePicker/Select/TimePicker ExpandDropChanged
- ➕ Preview 补全更多定义属性
- ➕ ImagePreview 图片预览控件
- ➕ Menu MenuDividerItem 分割线
- ➕ Menu ScrollBarBlock 内嵌模式下滚动条重叠
- ➕ Input SelectLast
- ➕ Segmented Reverse
- ➕ ContextMenuStrip ParentItem
- ➕ Button UseMnemonic
- ➕ Button AutoToggle
- ✍️ ContextMenuStrip 调整屏幕边缘碰撞 子菜单在左侧弹出
- ✍️ Message 换行后图标大小异常
- ✍️ TabHeader 空数据不显示添加按钮
- ✍️ Tree 抑制异常
- ✍️ Label 阴影顺序
- ✍️ Select 下拉多屏错位
- ✍️ Select 鼠标滚轮 DividerSelectItem 选中
- ✍️ Select Placement TR 位置异常
- ✍️ Popover 箭头位置显示有误
- ✍️ DatePickerRange 键盘回车刷新下拉
- ✍️ DatePickerRange TextAlign 选中不到开始时间最后
- ✍️ Input 补全左右键
- ✍️ Menu USelect 修复UI刷新
- ✍️ Table 焦点跳转 调整 Tree 模式下 Table[RowIndex][ColumnIndex] 获取的值不正确情况
- ✍️ Table 换行居中问题
- ✍️ Input 键盘回车
- ✍️ Mask 隐藏后依旧显示
- ✍️ 覆盖新渲染

### `2.1.13` 2025-10-24

- ➕ Select 下拉 最大高度滚动条
- ➕ Table 单元格焦点跳转导航（配置跳转顺序）
- ➕ Table 单元格 CellEditEnter 事件
- ➕ TabHeader Add 按钮 图标设置属性
- ➕ Tabs EnablePageScrolling 滚轮切换焦点页 (默认开启)
- ➕ Table DataSource 增加 动态对象、字典对象 支持
- ✍️ Input Dpi 1 时不显示光标
- 🧹 规范事件

### `2.1.12` 2025-10-17

- ✍️ Carousel 内存溢出
- ✍️ UploadDragger 管理员只有一个能上传
- ✍️ Table ColumnSelect 为空时无法正常选择数据
- ✍️ Table 编辑模式 Win7 下焦点

### `2.1.11` 2025-10-15

- ➕ Table 全幅截图 DrawBitmap(bool full)
- ➕ Pagination SizeChangerTabStop 用于控制切换器是否可以通过Tab获取焦点
- ✍️ Button 传递 双击到 MouseUp
- ✍️ Modal 按钮栏颜色异常
- ✍️ BaseForm 深色主题切换不生效
- ✍️ Mask 无法随父窗口关闭
- ✍️ 下拉 无动画重复Hook
- ✍️ Tree 抑制 并发异常
- ✍️ Table ColumnSelect 适配最佳宽度
- ✍️ Popover 父窗口隐藏后依旧存在

### `2.1.10` 2025-10-13

- ➕ Table ScrollBarAvoidHeader 滚动条从表头下方开始绘制
- ➕ Message/Notification/Modal EnableSound 播放提示音
- ➕ Spin 错误回调
- ➕ TooltipComponent 开放 Delay
- ➕ Modal/Drawer ManualActivateParent
- ➕ Table.Filter ColumnSelect 的支持
- ✍️ Table 编辑模式
- ✍️ Table.Filter 失效问题
- ✍️ Table.ColumnSelect.CellType = Both 不显示 IconSvg 的问题

### `2.1.9` 2025-10-11

- ➕ SelectMultiple TagBordered 应用 Status 状态色
- ➕ Button 丰富 ForeHover/ForeActive
- ➕ Table Cell系列 国际化
- ➕ Spin Indicator 自定义加载指示符
- ➕ Alert IconSvg自定义图标 和 CloseChanged事件
- ➕ Select EnterDropDown 属性 用于部分场景中是否允许回车键触发下拉框
- ✍️ 滚动条适配 ColorScheme
- ✍️ Spin 隐藏后依旧显示
- ✍️ Table 编辑模式回滚失去焦点关闭
- ✍️ Table 点击空白处退出编辑模式
- ✍️ Collapse ItemButton.Visible=false 仍然响应 ButtonClick 事件
- ✍️ Collapse Button 无法隐藏
- ✍️ 渲染 释放异常

### `2.1.8` 2025-09-28

- ➕ 全局主题配置
- ➕ PageHeader BackIcon
- ✍️ 色卡分配符号
- ✍️ HyperlinkLabel 超链接内容不能正常解析
- ✍️ DatePicker/Select 下拉并发消息异常
- ✍️ PageHeader 渐变色 对其
- ✍️ 丰富 全局主题配置
- ✍️ 优化 遮罩 稳定性
- 🧹 Table 编辑模式交互逻辑

### `2.1.7` 2025-09-24

- ➕ HyperlinkLabel 控件
- ➕ Modal/Message/Notification/FloatButton/ContextMenuStrip Target 支持 更丰富的目标
- ➕ Popover/Modal/Drawer 色彩模式
- ➕ Popover Config Back, Fore属性设置
- ➕ StackPanel RightToLeft 支持
- ➕ PageHeader UseForeColorDrawIcons 属性
- ✍️ DatePickerRange 下拉渲染不激活Value
- ✍️ Collapse 修正FontExpand.Size < Font.Size时不显示的问题
- ✍️ DatePicker 系下拉徽标
- ✍️ Table 激活态和总结列
- ✍️ Binding 移动行不刷新
- ✍️ Table BorderHigh 精度
- ✍️ Watermark 内存溢出
- ✍️ 优化代码逻辑和内存溢出
- 🧹 FlowLayoutPanel 重构滚动条

### `2.1.6` 2025-09-19

- ➕ Config DpiMode
- ➕ InputNumber ValueFormatter 事件
- ✍️ InputNumber 圆角 UI 异常
- ✍️ Tree 复选框适配禁用项
- ✍️ Tooltip Hook 关闭敏感
- ✍️ Table 全选
- ✍️ Menu 子菜单UI
- ✍️ 下拉坐标计算

### `2.1.5` 2025-09-17

- ➕ Config UseHook `用于 Addins 开发`
- ➕ Transfer 穿梭框
- ➕ Table BorderHigh
- ➕ Table 键盘全选交互
- ➕ PuzzleCaptcha 拼图验证码
- ➕ Menu 下拉参数
- ✍️ Table 开放 图标配置
- ✍️ ChatList 图片重叠
- ✍️ Popover 适配暗色模式
- ✍️ VirtualMode Table 全选
- ✍️ Table 键盘数组越界
- ✍️ Message/Notification 内存溢出
- ✍️ PageHeader 最小化后未恢复按钮状态
- ✍️ Table AutoSizeColumnsMode.Auto 不显示右边
- ✍️ SelectMultiple 选择搜索项时自动清除搜索内容

### `2.1.4` 2025-09-09

- ➕ Menu ItemClick
- ➕ SelectItem 子项 MaxCount
- ➕ Table Column MinWidth
- ➕ Table ScrollColumn 方法
- ➕ Preview 支持 GIF
- ✍️ Table Tree 异常
- ✍️ Table 复制单元格是类名
- ✍️ Table AutoSizeColumnsMode.Auto 失效
- ✍️ Table Tree 异常
- ✍️ Table 筛选下异常
- ✍️ Table 内容溢出
- ✍️ Table 边框线粗细不一致
- ✍️ 图表控件 添加极坐标和雷达图支持并优化图表计算
- ✍️ GridPanel 行没有优先扣固定值
- ✍️ FlowPanel/StackPanel 调整滚动条绘制顺序以避免覆盖问题
- ✍️ Menu USelect 方法失效
- ✍️ VirtualPanel 模糊头

### `2.1.3` 2025-09-04

- ➕ Table VirtualMode 虚拟模式
- ✍️ Menu 自定义菜单设计器异常
- ✍️ Segmented Full 精度

### `2.1.2` 2025-09-03

> 因 `VirtualMode` 存在 Debug 代码，请使用 >= `2.1.3`

- ➕ Table VirtualMode 虚拟模式
- ➕ Table 树支持集合
- ➕ Table 支持嵌套字段反射
- ➕ ContextMenuStrip 比例配置
- ➕ Collapse AutoSize
- ➕ Label AutoSizePadding
- ➕ Menu 键盘交互
- ➕ Menu FocusMode 焦点模式
- ➕ Menu 自定义按钮
- ➕ Tree 键盘交互
- ✍️ TabHeader Add按钮位置异常
- ✍️ Input TextAlign 多行错位
- ✍️ FloatButton MarginX 错位
- ✍️ DatePickerRange ValueChanged 重复触发
- ✍️ Select Insert 空集合 无法插入
- ✍️ Notification Bottom 多显示器无法显示
- ✍️ Drawer/Popover 多显示器闪窗口
- ✍️ Button 垂直多行文字
- ✍️ ToolTip 关闭
- 🧹 Chart 对齐标准
- 🧹 Watermark 对齐标准
- 🧹 丰富 GridPanel

### `2.1.1` 2025-08-29 📊

- ➕ Chart 图表
- ➕ Watermark 水印
- ➕ Chat/MsgList (聊天控件) 消息列表右键菜单和点击事件支持
- ➕ Tabs IconGap
- ➕ Alert 鼠标移入时暂停轮播的开关属性
- ➕ SelectDown 藏渲染
- ➕ DatePickerRange InteractiveReset
- ➕ 控件 DrawBitmap 方法
- ➕ Modal Config DialogResult
- ✍️ Select 回归 CloseIcon
- ✍️ SvgDb 移除只读
- ✍️ Table Summary Render 生效
- ✍️ CollapseGroup 不显示UI
- ✍️ Menu 折叠后抖动
- ✍️ Button 多行文本异常
- ✍️ AutoSize 调整 Button/Checkbox/Radio/ColorPicker
- ✍️ Tabs Margin
- ✍️ Lable/Badge AutoSize 没有边距
- ✍️ TabHeader 关闭异常
- 🧹 重构 Tabs 布局

### `2.1.0` 2025-08-21

- ➕ 原生滚动条适配深色模式
- ➕ 新的 MouseHover 处理
- ➕ Config MouseHoverDelay
- ➕ Menu 布局属性 重构布局
- ➕ Menu ArrowRatio
- ➕ Input Ctrl+Back
- ➕ Alert LoopOverflow、TextAlign
- ➕ Select WheelModifyEnabled
- ➕ Button VirtualWidth 属性
- ➕ ChatList 主题属性
- ➕ ChatList ScrollLine 方法
- ➕ ChatList ItemGap
- ➕ ChatList IsAtBottom / IsAtRight
- ➕ Table Filter Apply重载函数
- ➕ Table 复制焦点单元格属性 ClipboardCopyFocusedCell
- ➕ Segmented ItemGap
- ➕ Tree TreeItem CheckedStrictly
- ➕ Tree TreeItem FullPath 方法
- ➕ Modal ContentPadding
- ➕ Calendar ItemPaint 事件
- ➕ Tabs TextCenter
- ✍️ Window 适配 Win7 NoAero
- ✍️ ContextMenuStrip Win7 子菜单异常
- ✍️ Menu 闪空白下拉
- ✍️ Menu 优化二级菜单
- ✍️ Table 键盘上下切换
- ✍️ Table 空数据排序异常
- ✍️ Table 小数点排序
- ✍️ Table FocusedCell 切换数据未清空
- ✍️ Pagination 初始化错位
- ✍️ Pagination 长度0修改页数重置当前页
- ✍️ Modal 关闭按钮错位
- ✍️ Tabs Page 优化加载
- ✍️ Steps 优化里程碑模式
- ✍️ 下拉 屏幕碰撞
- ✍️ ScrollBar 边距错位
- ✍️ ChatList 优化 GPT 多文本性能
- 🧹 DatePicker
- 🧹 TimePicker
- 🧹 DatePickerRange

### `2.0.17` 2025-08-06

- ➕ Layered 缓存渲染（性能优化）
- ➕ Select AutoPrefixSvg
- ➕ Table Filter 国际化
- ✍️ Drawer 闪烁优化
- ✍️ Modal OnBtns 回调异常后卡死
- ✍️ BorderlessForm 刚启动没有阴影
- ✍️ Tree 集合刷新
- ✍️ Menu InlineNoText 模式下提示可能闪动
- ✍️ Steps 扩大 Index字体
- ✍️ DatePickerRange 点击月

### `2.0.16` 2025-08-02

- ➕ Table / Segmented / FloatButton Tooltip 延迟弹出
- ➕ Select 适配 选中值后多语言
- ➕ Table Gaps 属性，分别调整XY间距
- ➕ Progress 圆形下 IconSvg并支持旋转动画
- ➕ Modal UseIconPadding
- ➕ 支持短HEX
- ✍️ Menu 卡点闪子菜单
- ✍️ Menu ShowSubBack 错位
- ✍️ Table 排序/筛选图标 位置
- ✍️ Steps 优化里程碑绘制
- ✍️ Tooltip 释放异常
- ✍️ Input 前后缀布局
- ✍️ 设计器下 窗口错位
- ✍️ DPI 优化

### `2.0.15` 2025-07-29

- ➕ Table FocusedCell 属性，CellFocused 事件
- ➕ Table GetColumnByFieldKey(string key)
- ➕ Table OpenTip 重载单次显示样式
- ➕ Collapse FontExpand 属性
- ➕ Collapse CollapseItem IconSvg 支持显示图标
- ✍️ Table SelectedIndex 排序后获取不正确
- ✍️ Table LineBreak 无效
- ✍️ Table Tip 闪BUG
- ✍️ Table 合并单元格编辑模式错位
- ✍️ Image3D 调整阴影绘制顺序
- ✍️ Spin 再次释放报错
- ✍️ Tree 初始后未布局
- ✍️ Collapse 优化性能
- ✍️ Image3D 公开动画处理方法 RunAnimation
- ✍️ Menu 错位
- ✍️ Tabs 超出选择错位

### `2.0.14` 2025-07-25

- ➕ Table 支持 Emoji
- ➕ Table 支持键盘上/下键切换焦点单元格
- ➕ Table VisibleColumns 按当前顺序获取可见的列
- ✍️ ContextMenuStrip 重构
- ✍️ Tooltip 并发不消失
- ✍️ Table DataTable 数据排序后赋值异常
- ✍️ Table 优化 点击排序/拖动交互
- ✍️ Tooltip 文本位置和碰撞公式
- ✍️ Table 优化 FilterList()
- ✍️ Table 公开 OpenTip, CloseTip 用于外部控制ToolTip
- ✍️ Input Y滚动条不跟手
- ✍️ Input MaxLength 异常
- ✍️ Button Loading MouseUp 不响应
- ✍️ Tree 初始化 Loading 不响应
- ✍️ 丰富项扩展设置方法

### `2.0.13` 2025-07-22 🎨

- ➕ Steps 里程碑模式 MilestoneCurrentCompleted 属性，用于切换事件流向
- ➕ Table ColumnSelect，支持显示图标+文本和编辑（适用于枚举/状态）
- ➕ Table 焦点列/行 FocusedColumn, FocusedRecord 属性
- ✍️ Table 滚动条遮挡和右侧浮动列
- ✍️ Table 支持键盘左/右箭头切换焦点单元格
- ✍️ Table 优化单元格焦点同行切换显示
- ✍️ 对齐 5.0 色彩算法

### ~~`2.0.12`~~ 2025-07-18

> 存在 Debug 代码，请使用 >= `2.0.13`

- ➕ TabHeader RightGap 右侧间隙
- ➕ Table KeyTreeArrowStyle 树表格箭头样式
- ➕ Table CellFocusedStyle/CellFocusedBg/CellFocusedBorder 实现显示焦点单元格
- ➕ Table ScrollLine (object record) 方法，用于跳转到指定行
- ➕ Steps MilestoneMode 里程碑模式
- ➕ Collapse Button Visible 属性
- ✍️ 对齐 5.0 色彩算法
- ✍️ FloatButton 修改主题色不刷新
- ✍️ TabHeader 优化选项卡标题参数并关闭未更改事件
- ✍️ Select SelectItem Text
- ✍️ Select Top 弹出 筛选后Y异常
- ✍️ Table 所有事件坐标
- ✍️ Table Tooltip 错位
- ✍️ Table 优化响应速度
- ✍️ Input 光标位置
- ✍️ Input 圆角前后缀遮挡文本
- ✍️ InputNumber 第一次 遮挡后缀
- ✍️ Collapse ExpandChanged 补充坐标信息
- ✍️ ColorEditor 多次渲染
- ✍️ Date 最大值异常

### `2.0.10` 2025-07-12

- ➕ Table EditSelection 属性，用于编辑时可默认全选
- ➕ Table EditAutoHeight
- ➕ Table RowPaint 事件
- ➕ Table Column DisplayFormat 属性，支持数值的格式化显示
- ➕ Table Column.Filter Apply函数，用于外部执行筛选
- ➕ Collapse UniqueFull
- ➕ Splitter SplitterWidth 未生效
- ➕ Tree Loading 状态
- ✍️ Select 下拉错位
- ✍️ Select 筛选后值异常
- ✍️ SelectMultiple 超出索引
- ✍️ Input 滚动动画
- ✍️ SliderRange 色卡对齐
- ✍️ 下拉弹窗 低质量
- ✍️ Table 丰富 CellPaintBegin 事件参数
- ✍️ Table 优化筛选功能，新增筛选结果变化事件FilterDataChanged，优化设置Summary时会清除筛选的问题
- ✍️ Table 调整鼠标交互
- ✍️ Divider 垂直文本

### `2.0.9` 2025-07-07

- ➕ Table 支持数据列筛选功能 (BETA)
- ➕ Transfer 穿梭框组件 (BETA)
- ➕ Table Column ReadOnly 只读属性
- ➕ Table 编辑模式 Excel样式
- ➕ Collapse GroupButton Tooltip属性，支持工具提示
- ➕ Collapse AnimationSpeed 调整动画速度
- ➕ Modal Btn增加 DialogResult 回调
- ➕ ColorPicker ShowSymbol属性，增加恢复颜色设置的功能
- ➕ Menu Flatten 属性, 实现菜单的平展/收缩, 并在平展模式下显示工具提示
- ➕ Menu InlineNoText
- ➕ Button DisplayStyle
- ➕ LabelTime DragMove
- ✍️ Table 内容超出
- ✍️ Menu 弹出下拉宽度不够
- ✍️ Menu Mode InlineNoText，子菜单不显示箭头
- ✍️ Menu Mode InlineNoText 错位
- ✍️ Modal CancelButton事件
- ✍️ Collapse GroupButton Edit 事件响应
- ✍️ Collapse GroupButton 响应细节
- ✍️ Collapse GroupButton Text 为空时，Icon不居中
- ✍️ Input 获得焦点后光标移到文本末尾
- ✍️ Input 空格被过滤
- ✍️ Tree 交互
- ✍️ 重构下拉

### `2.0.7` 2025-06-27 🌵

- ➕ Shield
- ➕ Collapse Button / Switch
- ➕ Draw / DrawBg
- ➕ Config EmptyImage
- ➕ Tree Empty
- ✍️ Table 边框边距
- ✍️ Table AutoSizeColumnsMode.Fill 布局
- ✍️ Table 堆栈头交互区域
- ✍️ Table 编辑模式文本高度不够
- ✍️ Table Paint 开放 Column
- ✍️ Collapse Buttons 响应鼠标移动
- ✍️ Input 右侧图标不显示
- ✍️ Input 重构键盘
- ✍️ Input 回退交互
- ✍️ Tree 补全方法
- ✍️ Steps 图标SVG色彩
- ✍️ Breadcrumb 布局
- ✍️ Pagination 布局

### `2.0.6` 2025-06-20

- ➕ Table GetColumnRealIndex
- ➕ Input IconRatioRight
- ➕ FlowPanel/GridPanel/StackPanel 背景，边框，设计器边框
- ➕ TabHeader 多标签页头
- ✍️ Table Summary 移除列复选框
- ✍️ LabelTime 适配 Padding
- ✍️ InputNumber 失去焦点顺序
- ✍️ 优化 Input Round 图标边距
- ✍️ BackImage 错位
- ✍️ Canvas DrawText 错位 补全省略
- ✍️ Label AutoEllipsis
- ✍️ Measure Text
- ✍️ Button DialogResult
- ✍️ 补充 Tag

### `2.0.5` 2025-06-17

- ✍️ Table 列拖拽错位
- ✍️ Table 为空线错位
- ✍️ Button 焦点
- ✍️ Menu 横向布局应用 Gap
- ✍️ 日历最大值最小值前后

### `2.0.4` 2025-06-14

- ➕ Tree BeforeExpand、AfterExpand
- ➕ Preview SelectIndex
- ➕ Preview OnSelectIndexChanged
- ➕ Message/Notification contains
- ➕ DatePickerRange Picker
- ➕ Menu SelectChanging
- ➕ Pagination DropDownRadius
- ➕ BaseForm 色彩模式变化配置
- ➕ Table EditInputStyle
- ➕ Input TextTotalLine
- ➕ Calendar SetMinMax/SetBadge 方法
- ✍️ Table 列拖动+隐藏 异常
- ✍️ Table 键盘上下激活 Summary
- ✍️ Table 换行文字消失
- ✍️ Table 合并单元格边框
- ✍️ Input 文字拥挤
- ✍️ Input 重构光标交互
- ✍️ Dropdown DropDownTextAlign 无效
- ✍️ Modal 居中父窗体调整

### `2.0.3` 2025-06-09

- ➕ Table SortOrderSize
- ➕ Table 事件补充 Column
- ➕ Table GapCell
- ➕ Table CellImpactHeight
- ➕ Table ICell ImpactHeight
- ➕ Table ICell 补全 设置
- ➕ Table Tag Gap
- ➕ Avatar 鼠标悬浮交互
- ➕ Progress UseTextCenter
- ➕ Select DropDownEmptyClose
- ➕ Pagination 增加页面标签文本标签文字提示修改
- ➕ Helper SearchWeightSort
- ➕ Tooltip 补全 方向
- ➕ 图片共存显示
- ✍️ ILayeredFormAnimate 在 Dispose 时可能引发的字典访问异常
- ✍️ Tree 多选
- ✍️ Tree Hover 判定
- ✍️ Table 列固定标题栏边框杂边
- ✍️ Table Shift+滚动条
- ✍️ Menu Select 方法
- ✍️ Menu Padding 滚动条
- ✍️ TimePicker 滚动条问题
- ✍️ IconState 错位
- ✍️ Lable Emoji 错位

### `2.0.2` 2025-05-30

- ➕ Select/Dropdown/Table DropDownTextAlign
- ➕ Table CellPaintBegin/CellHover 事件
- ➕ Table CellCheckbox/CellRadio 单元格
- ✍️ Table 右浮动悬浮背景
- ✍️ PageHeader 最大化按钮变化
- ✍️ 抑制 Win7 No Aero 闪烁
- ✍️ 优化 阴影并发
- ✍️ 优化 DatePickerRange 交互
- ✍️ 日期选择布局
- ✍️ Table 合并边框
- ✍️ 适配 Win7 滚动条无焦点不响应

### `2.0.1` 2025-05-26

- ➕ Label IconGap
- ➕ NoticeOverflowClose
- ➕ Message/Notification MaxCount
- ➕ Select 项丰富样式属性
- ➕ Dropdown/Select Empty
- ➕ PageHeader MDI
- ➕ Table SetSortList
- ➕ Select/Dropdown/Table DropDownTextAlign
- ✍️ Lable 居中布局
- ✍️ InputNumber 后缀遮挡
- ✍️ 统一为空渲染
- ✍️ Table 容器内部对齐
- ✍️ 蒙版 边框对齐窗口
- ✍️ Steps 项隐藏

### `2.0.0` 2025-05-19 🥣💲🐖👚

- ➕ Select 补全 PropertyChanged
- ➕ Table 树行排序事件
- ➕ Table HitTest
- ➕ Message TopMost
- ➕ Tree HitTest
- ➕ Tree 事件 交互标志
- ➕ Tree GapIndent
- ➕ Tree Index序号
- ➕ Menu 项徽标
- ➕ Menu HitTest
- ➕ Menu Index序号
- ➕ Button JoinMode
- ➕ Input JoinMode
- ➕ ColorPicker JoinMode
- ➕ ColorPicker 双击下拉交互
- ➕ DatePicker ShowButtonToDay
- ➕ TimePicker ShowButtonNow
- ➕ TimePicker 适配时分秒单独显示
- ✍️ Input ClearStyle 修复未生效
- ✍️ Input 方向消息
- ✍️ ColorPicker RGB 模式 Input 坐标异常
- ✍️ Table 浮动列背景
- ✍️ Table DataTable 编辑模式设置值
- ✍️ Table 插槽列排序
- ✍️ InputNumber - ✍️ TextAlign.Right
- ✍️ Avatar PlayGIF 设置无效

### `1.9.12` 2025-04-27

- ➕ Table 多级下拉
- ➕ VirtualPanel 指定容器内区域渲染
- ✍️ SelectMultiple SelectAllItems 无法取消
- ✍️ Menu 子菜单 Visible 无效
- ✍️ Table 固定列 颜色不生效
- ✍️ Input 在WPS/Addin键盘失效
- ✍️ Spin 渲染抽搐
- ✍️ Table LostFocusClearSelection 异常
- ✍️ 移除 渲染队列
- ✍️ 移除 Input 主动GC
- ✍️ Table 多数据全选卡顿
- ✍️ Modal 默认字体

### `1.9.11` 2025-04-18

- ➕ 单独 ColorScheme
- ➕ Modal DefaultFocus
- ➕ Tabs EnableSwitch
- ➕ Menu Horizontal 新增超出显示
- ➕ Panel BackExtend
- ➕ Message Modal Notication 自定义图标
- ➕ Menu Horizontal 超出显示
- ➕ Table 适配 BindingSource
- ➕ Table column support display name
- ➕ Input Variant 形态
- ➕ Input ClearClick 事件
- ➕ ScrollBar 补充事件
- ✍️ Table BindingSource 第一行外刷新问题
- ✍️ Table 合并单元格覆盖表头
- ✍️ Table 不同类型反射Set
- ✍️ Table 移动异常
- ✍️ Table Binding 不刷新
- ✍️ Table 编辑模式消失焦点异常
- ✍️ Notification 渲染问题
- ✍️ Button AnimationBlink 覆盖背景
- ✍️ Input 圆角问题
- ✍️ Tooltip 多屏弹窗位置异常

### `1.9.10` 2025-04-12

- ➕ Table Summary 总结栏
- ➕ Table TemplateColumn
- ➕ Table CellPaint
- ➕ Table GapTree
- ➕ Input SetStyle
- ➕ Tabs TabPage 禁用效果
- ✍️ Table 表头背景错位
- ✍️ Table 预设列宽
- ✍️ Table Tree 错位
- ✍️ Tree 主动选中触发事件
- ✍️ Input 选中位置多回车位
- ✍️ Input 光标乱跳
- ✍️ Menu 启用 Unique 未处理多处展开
- ✍️ Panel ShadowOpacityAnimation 控制 MessageFilter
- ✍️ ContextMenuStrip 无动画无法点击
- ✍️ SegmentedItem 无色IconSvg
- ✍️ ColorPicker 在 100% 缩放下偏移
- ✍️ Message 动画丢失
- ✍️ EventHub 内存泄漏

### `1.9.9` 2025-04-03

- ➕ 更细腻的 动画使能 配置
- ✍️ Input SelectionStart
- ✍️ Table SelectIndexChanged 多次触发
- ✍️ Progress 宽度0异常
- ✍️ ContextMenuStrip 交互
- ✍️ Spin TopLevel 多窗口嵌套异常
- ✍️ Input 加入主动GC
- ✍️ Input 鼠标交互

### `1.9.8` 2025-03-29

- ➕ 渐变色 支持 多组颜色以及百分比
- ➕ Table CellBeginEditInputStyle 控件覆盖
- ➕ Menu 交互属性 MouseRightCtrl
- ➕ Input AnimationBlink
- ✍️ Spin TopLevel 多窗口嵌套异常
- ✍️ Table CellEndEdit 未修改也触发
- ✍️ Window 闪原生标题
- ✍️ 动画速率过快

### `1.9.7` 2025-03-25

- ➕ InputNumber WheelModifyEnabled 交互属性
- ➕ Table CellProgress Size
- ➕ BaseCollection ToArray ToList
- ➕ Button 过度闪烁动画
- ➕ DatePicker 选择器类型属性
- ✍️ Splitter Lazy 不生效
- ✍️ Table 圆角被覆盖
- ✍️ ColorPicker 并发问题
- ✍️ Window 默认最大化抽搐
- ✍️ Splitter 无法拖动

### `1.9.6` 2025-03-20

- ➕ Dropdown ItemClick
- ✍️ Table 右侧 Fixed 重影
- ✍️ Table Cell DropDown 无法触发
- ✍️ Tabs 适配语言切换
- ✍️ Collapse 修改 Text 刷新

### `1.9.5` 2025-03-19

- ➕ Table CellButtonDown CellButtonUp
- ➕ Config EmptyImageSvg
- ➕ Popover Padding
- ➕ Table CellRectangle 方法
- ➕ Table Column WidthPixel
- ✍️ Drawer 父容器置顶后内部下拉被遮挡
- ✍️ Input 滚动条交互错位
- ✍️ Calendar 语言切换
- ✍️ Table 编辑模式中弹出 Modal 无遮罩
- ✍️ Pagination ShowSizeChanger 切换语言水印文本不变
- ✍️ Table 浮动列圆角
- ✍️ Table BindingList 多次触发布局
- ✍️ Dropdown 联动 SelectedValue
- ✍️ SelectMultiple 清空后水印错位
- ✍️ Splitter 去除设计器限制拖动
- ✍️ 优化渲染并发

### `1.9.4` 2025-03-14

- ➕ Tabs DragOrder 拖动顺序
- ➕ Segmented SegmentedItem Hover 图标属性
- ➕ Segmented BarBg BarColor Tooltip
- ➕ Input VerifyKeyboard 快捷键拦截
- ➕ Panel RadiusAlign
- ➕ Tree 支持点选多个节点
- ✍️ Table MultipleRows 右键生效
- ✍️ Tooltip 多屏异常
- ✍️ DatePicker Text 清空后 Value 依旧存在值
- ✍️ Table 双击触发两次点击
- ✍️ 浅色模式误入暗色API
- ✍️ Window 抑制 重复渲染

### `1.9.3` 2025-03-11

- ➕ Preview 可以显示文本数据
- ➕ PageHeader IconRatio
- ➕ Tabs TabClick Cancel 可取消跳转
- ➕ Splitter 延时渲染属性
- ➕ Splitter 折叠功能最小宽度限制
- ➕ GridPanel 增加"弹簧"
- ✍️ Button 多行宽度碰撞
- ✍️ Modal 父窗口隐藏处理
- ✍️ 布局容器在 Popover 等控件未布局
- ✍️ Select FilterChanged与线程调用
- ✍️ DPI 适配 SplitContainer.PanelMinSize
- ✍️ PageHeader 隐藏三金刚键后依旧生效
- ✍️ PageHeader UseLeftMargin

### `1.9.2` 2025-03-06

- ➕ Table StackedHeader 堆叠表头
- ➕ Table Column DragSort
- ➕ Alert LoopInfinite
- ➕ Tooltip 区域碰撞
- ✍️ FlowPanel 控件重叠
- ✍️ Tabs Card 异常
- ✍️ 优化边框裁剪
- ✍️ Table 列 Style 背景无法跟随横向滚动条
- ✍️ 折叠面板按比例进行复原

### `1.9.1` 2025-03-04

- ➕ Tabs ContainsTabPage
- ➕ Drawer wait
- ➕ Splitter 折叠功能
- ➕ Select 属性 CloseIcon，事件 ClosedItem
- ➕ ControlEvent 在 Modal/Drawer/Popover 中通知加载成功
- ✍️ 补全 TextRenderingHighQuality Size 转换
- ✍️ Input 使用 WM_CHAR 替换 KeyPress
- ✍️ DatePicker等控件 清空内容后 失焦还会填充Value
- ✍️ Table 编辑模式文本框高度异常
- ✍️ Input Emoji 判断
- ✍️ 窗口最大值二次放大

### `1.9.0` 2025-02-27 🥥

- ➕ Table 合并单元格
- ➕ Table 列在编辑模式下可单独设置是否编辑
- ➕ Table ColBreak 表头自动换行属性及相关方法
- ➕ FlowPanel 子控件 Index属性
- ➕ GridPanel 子控件 Index属性
- ➕ PageHeader UseLeftMargin
- ➕ Menu Text属性 多语言特性
- ➕ Badge 角标 国际化语言特性
- ➕ 文本高质量呈现
- ✍️ Table BindingList 绑定空数据后 ToDataTable 异常
- ✍️ Table 合并单元格 同级别异常
- ✍️ Table BindingList 多次绑定
- ✍️ Table 初始化显示
- ✍️ Tabs SelectTab 使用 Name 判断
- ✍️ Tabs 主动控件被移除
- ✍️ Tabs TabPage Text属性非构造调用时的自适应
- ✍️ Select 子节点区域碰撞
- ✍️ Input 删除后光标位置异常
- ✍️ Input AOT 无法复制
- ✍️ Input 多选Tab 切换焦点
- ✍️ SelectMultiple AutoHeight
- ✍️ Button 渐变色空大小异常
- ✍️ Tour 窗口置顶气泡遮盖
- ✍️ Spin 异步抛出异常
- ✍️ GridPanel 置顶某行高度平均
- ✍️ 部分键盘移除拦截

### `1.8.9` 2025-02-18

- ➕ Tour 漫游式引导
- ➕ Popover Focus
- ➕ 适配国际化 TooltipComponent，EmptyText
- ➕ Chat.MsgList 丰富头像属性，防止滚动条遮挡
- ➕ Table 树展开方法与事件
- ➕ Table 适用于DataSource为AntList的场景的Refresh方法
- ✍️ Table 表头变更增删未生效
- ✍️ Table Column.SetColAlign() 赋值不正确
- ✍️ Table Refresh 适配 AntList
- ✍️ Table 多选第一个
- ✍️ Table CellText 适配 Ellipsis
- ✍️ Spin 在部分容器中加载问题
- ✍️ Spin 方法返回 Task
- ✍️ Spin 在原生窗口不显示
- ✍️ Tabs Card 切换异常
- ✍️ Tabs 删除 TabPage 数组下标越界
- ✍️ Tabs Card2样式边框遮挡
- ✍️ Input 换行错位、多行第一行交互
- ✍️ SelectMultiple ClearSelect()及SelectAllItems()方法
- ✍️ 优化渲染队列
- ✍️ GridPanel 自定义行高度
- ✍️ MsgList 预留 滚动条空间
- ✍️ Tree 最小化 滚动条初始
- ✍️ 修正全部可能隐藏初始滚动条
- ✍️ Visible 恢复原生

### `1.8.8` 2025-02-11

- ➕ 布局容器 ScrollControlIntoView
- ➕ Slider SetMinMax
- ➕ ColorPicker ShowClose
- ➕ Input 多行支持文本方向
- ✍️ Tabs 切换异常，超出部分卡片UI
- ✍️ Input MaxLength 替换交互
- ✍️ Tree BlockNode 交互

### `1.8.7` 2025-02-08

- ➕ Input VerifyChar 验证字符事件
- ➕ Input HandShortcutKeys
- ✍️ 容器 无法穿透 ValidateChildren
- ✍️ Input 最大值 无法限制粘贴
- ✍️ Input 多行交互
- ✍️ Spin 中途不显示
- ✍️ Timeline 添加 Description 区域异常
- ✍️ Tabs 超出显示切换滚动
- ✍️ 多行列宽设定相同时控制失效
- ✍️ Input 光标统一滚动条UI
- ✍️ Tooltip 没有使用对象字体
- ✍️ Window 无边框处理大小

### `1.8.6` 2025-01-23

- ➕ 国际化 Table 表头/ContextMenuStrip/LabelTime
- ➕ Splitter 分隔面板
- ➕ LabelTime AutoWidth
- ➕ Tabs TabClick
- ✍️ Table 表头丰富接口
- ✍️ Menu 代码设置 Select
- ✍️ Mask 在原生窗口的判断
- ✍️ GridPanel 单行列宽失效
- ✍️ Input 光标

### `1.8.5` 2025-01-21

- ➕ 国际化 Tabs/Menu/Breadcrumb/Collapse/FloatButton/Message/Notification/Segmented/Select/Steps/Timeline/Tree
- ➕ Config TouchThreshold
- ➕ Tree 获取区域方法
- ➕ Segmented 徽标
- ➕ Table CustomSort
- ➕ GridPanel Span 扩展行高设置
- ✍️ Menu/PageHeader 主题
- ✍️ FloatButton 补全徽标能力
- ✍️ Button Loading 键盘交互
- ✍️ SelectMultiple 清空不触发事件
- ✍️ Input SelectionStart 光标问
- ✍️ Table 负数排序

### `1.8.4` 2025-01-16
- ➕ VirtualPanel 公开一些属性
- ➕ Tree SelectItem 丰富属性方法
- ➕ Tabs IStyle GetTabRect
- ➕ Modal 标题可为空
- ➕ Segmented 图标上下支持换行
- ✍️ Select Value 值无法设置到 Sub子项目
- ✍️ TimePicker ValueTimeHorizontal 错位
- ✍️ Select 焦点下设置属性弹出UI
- ✍️ Drawer 内弹出窗口置底
- ✍️ Input 多行首行回车无法选中
- ✍️ Menu 无阴影无法选中展开菜单
- ✍️ 键盘交互丰富 新增回车支持

### `1.8.3` 2025-01-10
- ➕ Label Rotate 旋转
- ➕ Checkbox / Radio 焦点样式
- ➕ Drawer DisplayDelay
- ✍️ VirtualPanel 脏渲染区域
- ✍️ Spin 适配 Form
- ✍️ StackPanel 设计器复制粘贴

### `1.8.2` 2025-01-09
- ➕ SelectMultiple 多行样式，AutoHeight
- ➕ Button/Switch/ColorPicker 焦点样式
- ➕ Button LoadingRespondClick
- ➕ Button Loading -1 支持
- ➕ Style Clear 清空样式
- ✍️ ShowInTaskbar 大小变更
- ✍️ Input 键盘消息
- ✍️ Alert 文本靠边
- ✍️ ShowInTaskbar 窗口缩小
- ✍️ 丰富 Preview OnBtns 返回内容，优化体验
- ✍️ LabelTime 国际化
- ✍️ 统一圆角计算

### `1.8.1` 2025-01-07
[移除 WindowBar 请使用 PageHeader+ShowButton]
- ➕ Menu Gap
- ➕ Select 筛选权重
- ➕ PageHeader UseSubCenter
- ✍️ Message 等高频 渲染错位
- ✍️ Input 无法 Tab 到下一个
- ✍️ 规范 Table Cell 间距
- ✍️ Drawer 抑制闪烁
- ✍️ Window ShowInTaskbar 无边框异常
- ✍️ Input 滚动条隐藏样式无效
- ✍️ 控件 Tab 焦点问题

### `1.8.0` 2024-12-31 🐍
- ➕ 日期徽标功能对齐
- ➕ 横向滚动快捷键
- ➕ PageHeader SubFont，BackExtend
- ➕ PageHeader 全屏
- ➕ Switch Loading
- ✍️ Drawer 渲染问题
- ✍️ 部分组件掉帧
- ✍️ OS 在 framework 旧问题
- ✍️ Tree 拖拽

### `1.7.14` 2024-12-26
- ➕ UploadDragger 点击选择文件 ClickHand 点击上传等属性
- ➕ UploadDragger 拖拽文件夹
- ✍️ Button 鼠标离开后依然响应点击
- ✍️ 优化 PageHeader CancelButton 交互
- ✍️ Tabs Clear 后异常
- ✍️ Spin 在 Drawer 中不生效
- ✍️ Modal BtnHeight=0 Keyboard 无效
- ✍️ Tabs Card2

### `1.7.13` 2024-12-20
- ➕ Table 排序模式、SortModeChanged
- ➕ 触屏点击 Config.TouchClickEnabled
- ➕ Table SelectIndexChanged
- ➕ 日期徽标新增字符串和圆角支持
- ➕ Modal 拖拽窗口
- ✍️ Table PARENT 异常
- ✍️ 布局 控件初始化问题

### `1.7.12` 2024-12-16
- ✍️ Alert Loop 空文本卡死
- ✍️ Tree 交互新增点击图标
- ✍️ 按钮回车多重触发
- ✍️ Popover 自定义控件内部 TopMost
- ✍️ Tree 隐藏后依然响应事件
- ✍️ Label 自动宽度 空值
- ✍️ Timeline Gap

### `1.7.11` 2024-12-11
- ✍️ DPI 处理 隐藏后异常

### `1.7.10` 2024-12-09
- ➕ Timeline / Steps 开放 Gap
- ➕ Table 新增 DropDown 下拉
- ✍️ Button 回车 触发两次 Click
- ✍️ Table 编辑多行、编辑模式不同类型恢复旧值、RowHoverBg 透明闪烁
- ✍️ Pagination 圆角
- ✍️ Progress 大小0异常

### `1.7.9` 2024-12-03
- ➕ VirtualPanel 单个刷新
- ✍️ Table 多个 CellText 错位
- ✍️ DatePicker Format 解析异常
- ✍️ Table 单元格修改，绑定数据类型为uint16时报错
- ✍️ ProcessCmdKey
- ✍️ Tree 无数据不触发 Paint
- ✍️ Button 下拉箭头居中
- ✍️ Input 上下 异常
- ✍️ DateChanged 多次触发

### `1.7.8` 2024-11-28
- ➕ Table 新增禁用行
- ➕ Badge DotRatio/Gap
- ➕ Menu IconActive
- ➕ IconState
- ✍️ Table 编辑模式复制异常
- ✍️ 日期选择最大值
- ✍️ ColorPicker AllowClear
- ✍️ Table ToDataTable 插槽
- ✍️ DataBindings 不支持AOT
- ✍️ Window 大小变化
- ✍️ Splitter DPI 异常
- ✍️ Loading 加入背景
- ✍️ 图片 渲染并发
- ✍️ 公开 Table.ICell 容器
- ✍️ 色卡集中

### `1.7.7` 2024-11-25
- ➕ Table 多行选中
- ➕ Table 处理快捷键
- ➕ Table ToDataTable 插槽
- ➕ Menu 滚动条和屏幕边缘碰撞
- ✍️ Table DataTable 无法应用编辑
- ✍️ Button 背景重叠
- ✍️ 适配 DataBindings
- ✍️ TreeCheckedEventArgs 值

### `1.7.6` 2024-11-21
- ✍️ Table 复选框不显示
- ✍️ Tree 父级多选状态异常
- ✍️ Modal 闪退

### `1.7.5` 2024-11-20
- ➕ 颜色混色算法
- ➕ Collapse Full 铺满
- ➕ Table 新增 全局文本色、ToDataTable 方法
- ➕ Slider 滑轨背景色属性 TrackColor
- ➕ Menu GetSelectIndex
- ➕ 增强 Spin 新增进度，可中途修改参数
- ✍️ Table 表头刷新
- ✍️ Check/Radio 空文本不显示
- ✍️ Drawer 卡顿回滚
- ✍️ 初步检查内存溢出
- ✍️ 移除 Menu 折叠修改宽度
- ✍️ Control 禁用问题
- ✍️ Tabs 动态第一次不显示
- ✍️ Table 删除树节点子项不刷新
- ✍️ Input 多行滚动条遮文字
- ✍️ 拖拽文件能力下放全部
- ✍️ 移除右键菜单鼠标离开消失

### `1.7.4` 2024-11-15
- ➕ Table ColumnSort 拖拽手柄列
- ➕ ColorPicker AllowClear
- ➕ TimePicker/DatePicker ValueTimeHorizontal
- ✍️ 优化拖拽体验
- ✍️ Tabs 小重构
- ✍️ Alert 修复边框
- ✍️ 默认文字模糊

### `1.7.3` 2024-11-14

> 🎉 NET 9.0

- ✍️ Menu 弹出长度不够
- ✍️ Window 设计器存在Size
- ✍️ 优化 Select 筛选体验
- ✍️ DatePicker 英文显示不全
- ✍️ Table EmptyHeader 显示表头不居中

### `1.7.1` 2024-11-12
- ➕ Table SortColumns
- ✍️ Table EnterEditMode 滚动条位置不正确
- ✍️ Preview 不显示按钮
- ✍️ ContextMenuStrip 多个
- ✍️ 徽标颜色

### `1.7.0` 2024-11-11 🌍

> 全球化支持

- ➕ LabelTime
- ➕ FloatButton Enabled/Loading
- ➕ Select DropDownRadius
- ➕ Dropdown SelectedValue/DropDownRadius
- ➕ Button TextCenterHasIcon
- ➕ Segmented RightToLeft 反向/单项禁用
- ➕ Segmented SelectIndexChanging
- ➕ Menu Trigger
- ✍️ Menu 三级颜色异常
- ✍️ 徽标文字宽度
- ✍️ Table 表头阔高
- ✍️ Tabs 集合修改 SelectedIndex 异常
- ✍️ InputNumber ReadOnly
- ✍️ Tabs 修改徽标不刷新
- ✍️ Switch 点击动画
- ✍️ Window NET6.0下大小异常
- ✍️ ContextMenuStrip Sub 无法点击
- ✍️ Message/Notification close_id 逻辑
- ✍️ Button Ghost 禁用背景
- ✍️ 开放键盘事件
- ✍️ 日期本地化

### `1.6.16` 2024-10-31
- ➕ Button IconGap
- ➕ Input IconGap
- ➕ Segmented IconGap/BarPosition/AutoSize
- ➕ Collapse ExpandChanged
- ➕ Label ColorExtend
- ✍️ Menu 横向下拉错位
- ✍️ Button 父容器禁用后无效
- ✍️ 优化 下拉 Trigger.Hover 无法交互
- ✍️ ColorPicker 下拉箭头对齐
- ✍️ 补全 Badge

### `1.6.15` 2024-10-29
- ➕ Button 闪烁动画
- ✍️ Model UserControl 宽度异常
- ✍️ Menu 子菜单禁用
- ✍️ ColorPicker DisabledAlpha
- ✍️ Table 补全大部分字体大小取整
- ✍️ ContextMenuStrip 无法点击

### `1.6.14` 2024-10-26

> ⚠ ContextMenuStrip 无法点击

- ➕ OriginalBackColor 设置原装背景色
- ✍️ 窗口的最大化全屏交互
- ✍️ 窗口初始最大化缩放失效
- ✍️ 已知的线程等问题优化
- ✍️ ColorPicker 动画与异常
- ✍️ Framework 窗口还原Size异常
- ✍️ Tabs 开放鼠标事件
- ✍️ Slider Mark 点击错位

### `1.6.13` 2024-10-22
- ➕ Input 新增 ImeMode
- ➕ Select CheckMode/DropDownPadding
- ✍️ SVG 颜色失真
- ✍️ Notification 不关闭
- ✍️ Menu 缩放后应用自定义样式
- ✍️ 控件动画闪烁、闪退
- ✍️ Message/Notification 缓存不生效

### `1.6.11` 2024-10-19

> ⚠ 严重的闪退问题

- ➕ Table Style 列样式
- ➕ Message/Notification ClickClose
- ➕ Menu/Tree 新增 IconRatio
- ✍️ MsgQueue 死循环
- ✍️ DatePicker 时间部分超出UI
- ✍️ Input 的键盘和光标交互
- ✍️ 优化 Input 相关点击交互
- ✍️ Carousel 垂直超出渲染
- ✍️ Window 启动时最大化失效
- ✍️ 优化分层窗口动画性能

### `1.6.10` 2024-10-16
- ➕ UploadDragger 拖拽上传
- ➕ Input 前后缀前景色
- ➕ VirtualPanel 鼠标事件
- ➕ Panel BorderStyle
- ➕ Modal Close 主动关闭
- ➕ 徽标 SVG/方向
- ✍️ Drawer 可拖动
- ✍️ 优化 Progress Step 动画
- ✍️ Input 光标显示、Home/End 快捷键交互
- ✍️ Message AutoClose 0 时不关闭
- ✍️ Notification Text 显示不全

### `1.6.9` 2024-10-14
- ➕ Progress Step 动画
- ➕ Signal 动画
- ➕ Table 行高
- ➕ 滚动条新增最小大小
- ✍️ Alert 图标过大
- ✍️ Input 光标
- ✍️ Tooltip 文本改变无效
- ✍️ Model 图标对齐字体
- ✍️ Message/Notification 位置
- ✍️ SelectMultiple 固定 ListAutoWidth
- ✍️ 右键菜单 多个

### `1.6.8` 2024-10-10
- ➕ Button 进度 水波环形
- ✍️ SelectMultiple Tag 显示
- ✍️ BorderlessForm UseDwm 系列问题
- ✍️ Table 光标类型交互
- ✍️ Progress StepSize 适配 0 铺满
- ✍️ 右键菜单 多个

### `1.6.7` 2024-10-08
- ➕ Tabs TypExceed
- ➕ Table 排序事件，补全键盘
- ➕ 自定义 Cursor 设置
- ✍️ ContextMenuStrip 关闭动画无法点击
- ✍️ Tabs 统一超出部分
- ✍️ Tabs 设计器模式下不缩放
- ✍️ 触屏滚动异常

### `1.6.6` 2024-09-30
- ➕ Borderless DWM支持
- ➕ Table 丰富键盘交互
- ➕ SliderRange 滑动范围输入条
- ➕ Tabs ReadOnly 控制是否关闭
- ✍️ Select 下拉样式调整
- ✍️ Window 改变坐标大小变化
- ✍️ DatePickerRange 校验
- ✍️ Tabs 统一超出部分渲染
- ✍️ 下拉自动宽度计算
- ✍️ 开放部分内部方法
- ✍️ 优化触摸动画

### `1.6.5` 2024-09-26
- ➕ Slider Marks 刻度标记
- ✍️ Slider 样式对齐
- ✍️ 运行后修改 Size
- ✍️ Tree MouseDoubleClick
- ✍️ Select/Calendar/ColorPicker 等控件的弹出位置
- ✍️ Menu 图标SVG非空判断
- ✍️ Tabs 补充布局刷新
- ✍️ VirtualPanel 切换深色模式模糊标题栏
- ✍️ Window 窗口大小问题
- ✍️ 优化 Input 过长的动画
- ✍️ Input 光标最后不显示
- ✍️ 调整其他过长动画

### `1.6.4` 2024-09-24
- ➕ Progress 新增步骤进度条
- ✍️ Window 最大化还原后大小问题
- ✍️ 深度优化 Progress UI
- ✍️ Progress ShowInTaskbar 异常
- ✍️ Input 一些快捷键动画
- ✍️ SelectMultiple 删除项切换下拉
- ✍️ 优化 Input 查找点击的光标位置
- ✍️ Alert 改变字体后刷新
- ✍️ CellText 渲染超出

### `1.6.3` 2024-09-21
- ➕ 适配触屏
- ➕ Modal 自定义按钮新增线程
- ✍️ Window 坐标大小 问题
- ✍️ 原生窗口DPI
- ✍️ Tabs 缩放异常
- ✍️ ChatList 布局
- ✍️ MsgList 颜色
- ✍️ VirtualPanel 改变大小布局异常
- ✍️ BorderlessForm 在隐藏时改变大小导致阴影显示
- ✍️ Input 多行换行光标显示异常

### `1.6.2` 2024-09-19
- ➕ Label Tooltip 配置
- ➕ Input 水印颜色配置，渐变色背景
- ➕ SelectMultiple Tag 色彩配置
- ➕ Select ClickSwitchDropdown
- ➕ Badge AutoSize/TextAlign
- ➕ Signal 补全属性
- ✍️ Modal 隐藏窗口调整大小图标
- ✍️ 补全所有 TextChanged
- ✍️ SelectMultiple 长度超出后无法交互
- ✍️ SelectMultiple 超长内容不显示
- ✍️ Input 相关的控件下拉交互
- ✍️ Input 光标不显示
- ✍️ Input 光标交互

### `1.6.1` 2024-09-17
- ➕ Signal 信号强度
- ➕ Table 适配 BindingList
- ✍️ Input 光标，新增光标属性
- ✍️ Tag AOT 关闭异常
- ✍️ Carousel 鼠标交互错位
- ✍️ VirtualPanel 瀑布流布局异常
- ✍️ Message/Notification 动画衔接

### `1.6.0` 2024-09-14

> ℹ 新版本已移除 FocusExpandDropdown

- ➕ Collapse 折叠面板
- ➕ Input 密码框可粘贴配置
- ➕ Modal/Message/Notification 边距配置
- ➕ Calendar ShowButtonToDay
- ➕ 日期全系 最大最小日期范围
- ➕ Progress LoadingFull
- ➕ Segmented 禁用样式
- ➕ Input Win32 获取剪贴板
- ✍️ Button 切换 颜色动画突兀
- ✍️ Message/Notification 新增队列优化并发
- ✍️ Carousel 支持垂直滑动
- ✍️ Tabs ItemSize 左右
- ✍️ FocusExpandDropdown
- ✍️ Segmented 适配空文本样式

### `1.5.9` 2024-09-07
- ➕ Breadcrumb 面包屑
- ➕ PageHeader 页头
- ➕ VirtualPanel 空显
- ➕ Panel padding
- ✍️ ColorPicker 适配RGB模式
- ✍️ AntList 没数据的时候 Insert 无效
- ✍️ VirtualPanel 优化瀑布流
- ✍️ Select 无数据键盘卡死
- ✍️ ColorPicker RGB 异常
- ✍️ ContextMenuStrip 渲染问题，托盘交互
- ✍️ Divider 文字缺失
- ✍️ InputNumber 长按委托

### `1.5.8` 2024-09-05
- ➕ Preview 动态加载图
- ➕ ColorPicker DisabledAlpha Trigger Placement 颜色模式
- ➕ Win32 剪贴板
- ➕ FloatButton 新增 前景色 图标大小 间距
- ✍️ Timeline 适配SVG
- ✍️ 日期 徽标回调范围
- ✍️ WindowBar 在 Window 下的最大化按钮状态
- ✍️ 开放滚动条属性
- ✍️ 丰富 Menu 方法 新增是否焦点
- ✍️ Tree 修复更多级无法触发 CheckStrictly
- ✍️ WindowBar 焦点回车关闭
- ✍️ Tabs 顺序设置 SelectedIndex 时异常
- ✍️ 优化 LayeredForm 并发
- ✍️ TimelineItem 的 Text 设计器无法换行编辑
- ✍️ Drawer 变大、变更 Drawer 加载顺序
- ✍️ Drawer 释放后 SizeChanged 异常
- ✍️ Window 呈现

### `1.5.7` 2024-08-29 🌼

> 规范事件 使用上有重大变化，查看 [issue #IANFNJ](https://gitee.com/AntdUI/AntdUI/issues/IANFNJ)

- ➕ Icon 图标库
- ➕ VirtualPanel 虚拟容器
- ➕ Rate 新增 AllowClear，Character 字符支持
- ✍️ VirtualPanel 新增脏渲染
- ✍️ 已知的控件下标-1
- ✍️ Menu 丰富方法
- ✍️ SelectMultiple ListAutoWidth 无效
- ✍️ 表格绑定MVVM
- ✍️ Segmented 控件动态减少项 选中项下标异常
- ✍️ Input 多行水印可换行
- ✍️ Pagination 线程访问
- ✍️ Dropdown Hover 一级无法显示
- ✍️ ChatList 适配字素簇分割 [SVG与BASE64图]
- ✍️ Tabs Card2 样式添加卡片滚动条
- ✍️ 移除部分过时属性
- ✍️ 移除重复布局代码

### `1.5.5` 2024-08-24 🌼
- ➕ Button Toggle 切换
- ➕ Step IconSvg
- ➕ Tabs ItemSize
- ➕ 修正文本渲染
- ➕ 丰富 ShowInWindow
- ✍️ 优化 Pagination UI
- ✍️ Pagination SizeChangerWidth 无效
- ✍️ Input List 下 AllowClear 无效
- ✍️ 优化 Menu 弹出层交互体验
- ✍️ InputNumber 优化 交互前生效
- ✍️ Dropdown Trigger.Hover 多级无法停留
- ✍️ Label 前后缀错位
- ✍️ Input 低素质下前后缀文本错位
- ✍️ 优化 Button 大小计算
- ✍️ 优化 Button Loading 粗细
- ✍️ DatePickerRange 无法正常输入
- ✍️ 修正 AutoSize
- ✍️ 图渲染缺损

### `1.5.4` 2024-08-20
- ➕ Checkbox / Radio TextAlign
- ➕ Button 支持图标方向，规范图标命名
- ➕ Modal 自定义按钮样式
- ➕ Tabs Card2
- ✍️ Modal 未监控主题变化
- ✍️ FlowPanel 内部控件超出边界
- ✍️ Modal 点击Mask最小化
- ✍️ Panel ArrowSize 属性支持重绘
- ✍️ Layered 渲染 异常闪退

### `1.5.3` 2024-08-17
- ➕ InputNumber 最大值最小值可不生效
- ➕ Layered 逐帧渲染 优化渲染频率
- ➕ Select IconSvg
- ➕ Select Enable 交互
- ➕ SelectMultiple CanDelete
- ➕ WindowBar 线
- ➕ NotifyProperty 支持空字段 [net40以上]
- ➕ AntList 丰富构造函数
- ✍️ Modal 导致父窗口最小化
- ✍️ Tooltip 多行异常
- ✍️ 取消 CreateHandle 使用导致多次触发
- ✍️ FloatButton 释放后异常
- ✍️ 优化 Modal BtnHeight 0
- ✍️ 增强 Select SelectedValue
- ✍️ SelectMultiple 选择后依旧有水印

### `1.5.2` 2024-08-14
- ➕ Table 更多大小属性
- ✍️ Table Binding 表头未读取
- ✍️ Menu Vertical 异常
- ✍️ Input 中文水印不显示
- ✍️ Select List 异常
- ✍️ Tabs 拖控件报错
- ✍️ Table 第一次无数据加载

### `1.5.1` 2024-08-06
- ➕ Tabs 超出交互
- ➕ Table Render 插槽
- ✍️ Menu 收缩后无法点击
- ✍️ Drawer 适配 Form
- ✍️ Table 浮动列错位
- ✍️ Table Tree 适配 AntItem
- ✍️ Table 缺少字段不异常

### `1.5.0` 2024-08-02
- ➕ Button BackExtend 渐变色
- ➕ ScrollBarHide 隐藏滚动条样式
- ➕ DPI 自定义
- ✍️ Progress 外观
- ✍️ WaveSize 适配 DPI
- ✍️ 第一次 DPI
- ✍️ 简化全局事件通知
- ✍️ Table Binding Add
- ✍️ 优化 滚动条样式动画

### `1.4.9` 2024-08-01
- ➕ Menu Unique
- ➕ Tabs Centered
- ➕ Table 丰富 。按钮，图片 Tooltip
- ➕ Table 分割线 CellDivider
- ➕ Table 默认是否展开 DefaultExpand
- ✍️ InputNumber 等失去焦点验证
- ✍️ Table EmptyHeader
- ✍️ Table 刷新数据
- ✍️ 部分框架下中文不显示

### `1.4.8` 2024-07-31
- ➕ TimePicker 秒显示
- ➕ Table Tree 树
- ➕ Table AntItem 支持AOT
- ✍️ Table EmptyHeader
- ✍️ Table 刷新数据
- ✍️ Table 全选状态
- ✍️ Tag 等控件文本不刷新

### `1.4.7` 2024-07-25
- ➕ Modal 动态修改文字
- ✍️ Collection 优化并发
- ✍️ Notification TopMost 引发 ShowDialog 退出
- ✍️ Table EmptyHeader，Tag等不刷新问题
- ✍️ Tabs 补全 SelectedTab 属性
- ✍️ Tree 初始化 父级 CheckState 异常
- ✍️ 优化 Table 复选框样式

### `1.4.6` 2024-07-24 💎

> Tabs 重构 [beta] 使用上有重大变化，查看 [issue #IAEPO9](https://gitee.com/AntdUI/AntdUI/issues/IAEPO9)

- ➕ Tabs 图标，关闭按钮、适配四个方向与徽标
- ➕ Switch 文本
- ➕ Modal 动态修改文字
- ➕ Table Check / Radio / Switch 新增 AutoCheck
- ✍️ Table Check / Radio 多列触发刷新
- ✍️ Table 复选框样式
- ✍️ Modal 等待过程中取消的异常
- ✍️ Input 单前缀错位
- ✍️ Tabs Dpi，补全方向

### `1.4.6-beta` 2024-07-22

> Tabs 重构 [beta] 使用上有重大变化，查看 [issue #IAEPO9](https://gitee.com/AntdUI/AntdUI/issues/IAEPO9)

- ➕ Tabs 图标，关闭按钮、适配四个方向与徽标
- ➕ Select 筛选托管
- ➕ Drawer UserControl Size 监控
- ➕ Select SubText 子文本
- ➕ AutoCheck
- ✍️ Tooltip 置顶不显示
- ✍️ Table 表头边框异常
- ✍️ Button MouseClick
- ✍️ Table 点击滚动条触发
- ✍️ Button Image 禁用样式
- ✍️ FloatButton TopMost 不显示 Tooltip
- ✍️ Table 表格 Fixed后行颜色失效
- ✍️ Select 筛选高度和位置异常

### `1.4.5` 2024-07-15
- ➕ Tree Gap，单项 Fore Back
- ➕ Table 获取表头/列
- ➕ BorderlessForm ShadowPierce
- ➕ Drawer 加载关闭回调
- ➕ Message close_all
- ➕ 开放全局 Spin
- ✍️ Spin 位置不移动
- ✍️ Button Link 错位
- ✍️ Table 重复触发获取Data
- ✍️ Menu 缩进后三级菜单无法显示

### `1.4.4` 2024-07-11
- ✍️ Table 空数据
- ✍️ Table 仅1列边框
- ✍️ Menu 横向子菜单错位
- ✍️ Tree 图标同步文本颜色
- ✍️ Tree 多余折叠空间
- ✍️ Modal 释放未检测
- ✍️ Borderless 无法拖动

### `1.4.3` 2024-07-01
- ➕ Calendar 刷新徽标 LoadBadge
- ➕ Input Shift 点击交互
- ➕ 全局配置 NoticeWindowOffsetXY
- ✍️ SelectMultiple 下拉筛选错位
- ✍️ DPI 全部 Anchor 错位
- ✍️ Spin 错位

### `1.4.2` 2024-06-27 📅
- ➕ Calendar 日历
- ➕ InputNumber ShowControl
- ✍️ BorderlessForm Resizable，阴影 Close
- ✍️ SelectMultiple 前后缀错位
- ✍️ Color Alpha 异常
- ✍️ Progress 错位

### `1.4.1` 2024-06-25
- ➕ ColorPicker Input
- ➕ Tooltip CustomWidth
- ➕ Tree SubTitle
- ✍️ SelectionText 少字
- ✍️ InputNumber、DatePicker 交互
- ✍️ 右键菜单滚动条子菜单错位
- ✍️ Input 丰富键盘交互

### `1.4.0` 2024-06-20
- ✍️ ColumnSwitch 不触发
- ✍️ Table 触发刷新方式
- ✍️ Input 复制少文本
- ✍️ Input 删除

### `1.3.6` 2024-06-19
- ✍️ StackPanel 滚动条占用
- ✍️ Pagination Select 位置挡住
- ✍️ Label AutoSize
- ✍️ Table 修复交互

### `1.3.5` 2024-06-18
- ➕ Table Visible、VisibleHeader
- ➕ Progress ShowText、State
- ➕ Progress ShowInTaskbar 任务栏显示
- ✍️ Table 后台不渲染、渲染错位
- ✍️ Table 同时开启拖拽和排序交互
- ✍️ Input 选择文本到开头以外
- ✍️ StackPanel 滚动条占用

### `1.3.4` 2024-06-17
- ➕ Select GroupSelectItem 组
- ✍️ Pagination Gap
- ✍️ Table 修改模式并发
- ✍️ Table 空表头 Fixed 异常
- ✍️ Tabs SelectedIndex 代码设置无效
- ✍️ Tree 右键不生效

### `1.3.3` 2024-06-14
- ➕ Table 行样式文字颜色，选中文字颜色
- ➕ 全局 Config 阴影使能
- ➕ Label AutoEllipsis Tooltip
- ➕ Pagination Gap 间距，PageSizeOptions 下拉 PageSize
- ➕ Tree NodeMouseClick 右键
- ➕ Slider Align 方向
- ➕ ChatList 图片Base64格式渲染功能
- ✍️ Table 突然显示边框
- ✍️ Button 移除双击

### `1.3.2` 2024-06-13
- ➕ ContextMenuStrip 超出屏幕滚动条
- ➕ SelectDown 超出屏幕滚动条
- ➕ Spin 自定义字体
- ➕ ChatList SVG图片解析
- ➕ MsgList ItemSelected事件
- ➕ Table Radius 圆角与 BorderColor
- ✍️ Table IsLink 错位，重复 SizeChanged
- ✍️ Button 点击频率低

### `1.3.1` 2024-06-07
- ➕ ContextMenuStrip 新增 使能，修复子菜单 TopMost
- ➕ Table Button 图标
- ✍️ Table SelectedIndex 延迟
- ✍️ Button Hover图片错位
- ✍️ Input 复制交互

### `1.3.0` 2024-06-05
- ➕ Preview 图片预览
- ➕ Avatar 加载中
- ✍️ Input 补全 Shift 交互
- ✍️ Table 拖拽列头偏移
- ✍️ Table Progress 不刷新
- ✍️ BaseForm DPI 适配布局
- ✍️ Menu Visible 依旧可以点击

### `1.2.10` 2024-06-04
- ➕ Table 表头样式
- ✍️ Modal 窗口非必填
- ✍️ Menu 子级无法选中
- ✍️ Input 选中超出索引
- ✍️ Table 修复排序

### `1.2.9` 2024-05-31
- ➕ Avatar 支持GIF
- ➕ Segmented 间距，线样式
- ➕ ChatList ItemClick 点击事件
- ➕ Model 按钮字体与按钮高度设置
- ➕ Notification 标题字体设置
- ✍️ Avatar 修复方图
- ✍️ Table ColumnsMode.Fill
- ✍️ 修复自定义无关闭图标

### `1.2.8` 2024-05-30
- ➕ DatePicker TextChanged交互
- ➕ ContextMenuStrip 自定义坐标与方向
- ➕ Segmented ItemClick
- ➕ Tree 全选/全不全方法
- ➕ Modal LoadingDisableCancel 加载时禁用取消按钮
- ➕ FocusExpandDropdown 焦点时展开下拉
- ➕ FormMask 叠加模板
- ➕ Table AutoSizeColumnsMode
- ✍️ Tabs 移除最后一个报错
- ✍️ DatePickerRange
- ✍️ BorderlessForm 最大化交互
- ✍️ DatePickerRange 并支持时间
- ✍️ 优化 Window

### `1.2.7` 2024-05-23
- ➕ Input SelectionColor 选中颜色
- ➕ Input LostFocusClearSelection 焦点离开清空选中
- ➕ ContextMenuStrip 无焦点属性
- ✍️ Window 边框
- ✍️ 优化内存溢出

### `1.2.6` 2024-05-23
- ➕ Table 表头 MaxWidth
- ✍️ Table 交互宽度等问题
- ✍️ 无边框大小问题
- ✍️ 遮罩在 Framework 下无法判断Win11问题

### `1.2.5` 2024-05-22 📮
- ➕ Battery 电量
- ➕ Chat聊天控件
- ➕ Message 开放 Align 方向
- ➕ Table ColumnDragSort 列拖拽排序
- ✍️ Window 顶部线条

### `1.2.4` 2024-05-20
- ➕ Table 表头排序，简化键盘，优化 Tip 交互
- ➕ Avatar 支持SVG
- ✍️ Table - ✍️ 编辑模式不消失
- ✍️ SelectMultiple 补全 全选项，清空项目 方法，新增超出计数
- ✍️ BorderlessForm 交互修复

### `1.2.3` 2024-05-15
- ➕ Table EnterEditMode 主动进入编辑模式方法，文本框样式事件
- ➕ Alert LoopSpeed 速率
- ➕ 无边框窗口 BorderlessForm
- ✍️ 清空表格

### `1.2.2` 2024-05-14
- ➕ 开放图标比例
- ➕ Label SVG
- ➕ Select OnlineCustom颜色自定义
- ➕ Button 图标过度动画，Default 模式属性
- ➕ Table CellText 前后缀
- ✍️ Table 一些触发刷新，前置背景修复
- ✍️ Table 同时显示空表头和空提示
- ✍️ Input 回车异常，多行键盘交互
- ✍️ Panel 回滚背景
- ✍️ 完善徽标

### `1.2.1` 2024-05-10 🐲
- ➕ Table 选中行，省略文字提示，奇偶行颜色，宽度 fill 填充
- ➕ Table EnableHeaderResizing 手动调整列头宽度
- ➕ Panel 阴影方向
- ➕ Carousel 滚动交互
- ✍️ Input - ✍️ 回车输入
- ✍️ 按钮非空文本
- ✍️ Menu 无箭头
- ✍️ Steps 横向宽度分配不均
- ✍️ Input 内容过长滚动条太小
- ✍️ Window 顶部白边

### `1.2.0` 2024-05-05
- ➕ Menu 新增模式
- ✍️ Menu 弹出宽度
- ✍️ Input 边框模糊

### `1.1.9` 2024-04-30
- ➕ Input AutoScroll 显示滚动条，LineHeight 设置多行行高
- ➕ Table EmptyHeader
- ➕ Input PasswordCopy 密码复制
- ➕ Segmented IconAlign 图标方向
- ✍️ TopMost 焦点异常关闭
- ✍️ Input 密码框切换，修复前后缀样式
- ✍️ InputNumber 修复修改文本交互
- ✍️ Menu 不显示，新增自定义字体
- ✍️ Input ReadOnly 依旧可以下拉OR修改
- ✍️ InputNumber 按钮渲染效果
- ✍️ Table EmptyHeader 空数据不生效
- ✍️ Tree 补全 IconSvg
- ✍️ Table Progress 铺满
- ✍️ Progress 渲染
- ✍️ SVG适配AOT
- ✍️ 紧急修复一些TopMost问题


### `1.1.7` 2024-04-25 🌏
- ➕ 国际化 Internationale
- ➕ Table Switch开关，表头 LineBreak Ellipsis
- ➕ Table 复制方法，ScrollLine 滚动到指定行
- ➕ Input 字素簇分割
- ➕ Notification OnClose回调
- ➕ Segmented 竖向显示
- ✍️ Select 增强过滤功能
- ✍️ Tree 获取所有选中项
- ✍️ Input 补全Delete
- ✍️ Tree 开放部分属性

### `1.1.6` 2024-04-07
- ➕ SelectMultiple 多选器
- ➕ 表格 CellText Back
- ✍️ 全部 Back、Fore改用自带
- ✍️ Input修复粘贴触发字符校验
- ✍️ 表格修改模式异步
- ✍️ Menu折叠重复触发
- ✍️ Input 光标位置与TextAlign不符
- ✍️ Select下拉筛选样式错误
- ✍️ Tag补齐背景
- ✍️ 优化滚动条

### `1.1.5` 2024-04-01
- ➕ Button RespondRealAreas 响应真实区域
- ➕ BackColor系统属性初步替代（Button，Input，Avatar）
- ➕ Input 尝鲜颜色选取设计器
- ➕ Spin 文本
- ➕ Input Verify校验字符
- ➕ Label 标签
- ➕ 新的滚动条
- ✍️ Table 单条数据显示Y滚动条
- ✍️ Select下拉隐藏交互
- ✍️ 适配所有禁用动画
- ✍️ Tabs修复首次样式
- ✍️ Spin end不执行
- ✍️ Panel 已知异常
- ✍️ 修复DPI位置计算
- 
### `1.1.3` 2024-03-22 🐪
- ➕ Menu 史诗级更新 折叠，色彩模式，修复暗色模式UI
调整顺序
- ➕ GridPanel 格栅布局
- ➕ Table行复制
- ✍️ 3个布局控件
- ✍️ Tree BlockNode 选中偏移
- ✍️ Table Input异常
- ✍️ Tree 控件滚动条
- ✍️ 表格选中状态消失
- ✍️ Input 前后缀点击
- ✍️ X滚动条

### `1.1.2` 2024-03-18
- ➕ Notification TopMost
- ➕ Timeline与Steps ItemClick
- ➕ InputNumber 滚轮
- ➕ Tabs 隐藏头
- ➕ 宋体大于某个大小下拉无显示
- ➕ Window OnPreFilterMessage
- ✍️ Table 删除元素时报下标越界的异常
- ✍️ Table 控件调用 Binding 方法不显示数据的问题
- ✍️ FlowPanel 修复Size变化后滚动位置异常
- ✍️ Input 文本框禁止粘贴、新增Delete、光标错位
- ✍️ Table 复选框触发旧值

### `1.1.1` 2024-03-13
- ✍️ Input 密码框 🚑
- ✍️ WindowBar可自定义关闭按钮大小

### `1.1.0` 2024-03-12
- ➕ Input 补充 BackgroundImage
- ✍️ Select 选中态SelectItem
- ✍️ Input 修正Text null
- ✍️ Select 清空操作
- ✍️ 移除 InputNumber AllowClear
- ✍️ Table 单按钮列拉长

### `1.0.9` 2024-03-08 🎹
- ➕ TimePicker
- ➕ Table 宽度 Auto
- ➕ Table RowSelectedBg
- ➕ Input 透明支持
- ➕ Input ScrollToCaret 藏渲染提高效率
- ✍️ Input 设置MaxLength 无效
- ✍️ Input OnTextChanged 不触发
- ✍️ Input 空文本光标问题
- ✍️ InputNumber 输入限制
- ✍️ Pagination Cureent页码问题
- ✍️ AutoSize高度不够
- ✍️ 日期选择徽标覆盖
- ✍️ Model字体释放
- ✍️ 今日高亮效果被遮挡
- ✍️ TopMost弹窗被遮挡
- ✍️ 调整WinBar的按钮大小计算
- ✍️ Menu与Tree设置展开无效
- ✍️ 修正Button图标位置
- ✍️ 某些字体偏移
- ✍️ 轻量化SVG
- ✍️ 一些回车问题

### `1.0.7` 2024-02-29
- ➕ FlowPanel 流动布局
- ➕ StackPanel 堆栈布局
- ➕ ContextMenuStrip 延迟回调
- ➕ Panel 背景图片
- ➕ StackPanel 滚动条
- ✍️ 优化渐变动画效率
- ✍️ ContextMenuStrip 回调
- ✍️ WinBar MaximizeBox禁用依旧可以双击放大
- ✍️ 右键菜单释放
- ✍️ Model暗色模式背景
- ✍️ 表格滚动后 FixedHeader 无法点击全选
- ✍️ 适配表格修改模式
- ✍️ Steps修改图标没有重新渲染
- ✍️ 表格Text区域与边框

### `1.0.6` 2024-02-21
- ➕ AntList
- ➕ WindowBar UseSystemStyleColor
- ➕ Tag 关闭事件
- ➕ FlowLayoutPanel
- ✍️ 表格触发刷新
- ✍️ Step设计器布局异常
- ✍️ 日期选择等焦点问题
- ✍️ Model遮罩不消失问题
- ✍️ 设计器图片报错问题
- ✍️ Model父窗口隐藏
- ✍️ FlowLayoutPanel滚动条错位
- ✍️ 解除时间范围不能同一天
- ✍️ 优化部分Model闪烁

### `1.0.5` 2024-01-31
- ➕ DPI适配 TableLayoutPanel
- ➕ FloatButton TopMost
- ➕ Model Width
- ➕ WindowBar 色彩模式
- ➕ 开放Tree区域与移动事件
- ✍️ DatePickerRange AllowClear无效，年月高度，回车交互
- ✍️ Drawer父窗口坐标改动异常
- ✍️ FloatButton 提示位置
- ✍️ Notification内容布局
- ✍️ Message 适配DPI
- ✍️ Table 内容替换重置复选

### `1.0.4` 2024-01-29
- ✍️ Table 表格 修改响应
- ✍️ Modal 对话框 自定义按钮未关闭
- ✍️ Slider 滑动输入条负数Min问题并适配DPI
- ✍️ 优化 Drawer 抽屉 性能
- ✍️ 优化 Mask 性能
- ✍️ 优化List设定初始化长度