📖 **AntdUI** Ant Design UI

中文・[English](../en/Home.md)・[更新日志](UpdateLog.md)・[⚙️ 配置](Config.md)・[👚 主题](Theme.md)・[🦜 SVG](SVG.md)

基于 [dotnet Winforms](https://github.com/dotnet/winforms) 开发的界面库

<details>
<summary><strong>安装</strong></summary>

### NuGet安装

> 👏 推荐使用NuGet快速安装

#### 通过Visual Studio可视化安装
![nuget](Img/NuGet.png)

#### 通过PM命令安装
PM> `Install-Package AntdUI`

---

### 下载源码

> 打开AntdUI的码云地址：[https://gitee.com/antdui/AntdUI](https://gitee.com/antdui/AntdUI)
![downcode](Img/DownCode.png)

解压后双击打开 `AntdUI.sln` 解决方案，将 `examples/Overview` 项目设为启动项目，`F5` 启动

#### 源码下载无法编译？

> 编译器要求 **Visual Studio 2022** 以及以上

[Visual Studio 安装 旧版本(.NET Framework 4.0 和 4.5)](InstallOldVersionFramework.md)

#### 看不到工具箱？

需将 `AntdUI.csproj` 内 `TargetFrameworks` 只保留自己项目使用的框架版本，然后重新生成

> 操作完还是无法显示，重启VS让其重新加载，**多重新生成确保dll是最新的**

</details>

---

<details>
<summary>注意事项</summary>

#### 源码下载无法编译❓

编译器要求 **Visual Studio 2022** 以及以上，[Visual Studio 安装 旧版本(.NET Framework 4.0 和 4.5)](InstallOldVersionFramework.md)

####

#### 为什么设计器里面的窗口显示不全❓

HDPI问题，**应使用100%缩放来设计界面**
- 使用CMD `devenv.exe /noScale`
- 👏 [解决 Visual Studio 中 Windows 窗体设计器的 HDPI/缩放问题](https://learn.microsoft.com/zh-cn/visualstudio/designers/disable-dpi-awareness?view=vs-2022) `<ForceDesignerDpiUnaware>true</ForceDesignerDpiUnaware>`
- 桌面右键显示设置 将缩放修改至 `100%`

####

#### 那我如何启用DPI支持呢❓

CORE 可以轻而易举的解决[Application.SetHighDpiMode(HighDpiMode.SystemAware)](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.forms.application.sethighdpimode?view=windowsdesktop-8.0)；`Framework` 系，需要通过清单启用 [Windows 窗体中的高 DPI 支持](https://learn.microsoft.com/zh-cn/dotnet/desktop/winforms/high-dpi-support-in-windows-forms?view=netframeworkdesktop-4.8)

####

#### HDPI 下为何设计器与编译后的布局不一致❓

将每个`.Designer.cs` 中的 `AutoScaleMode` 移除/恢复默认值，移除 `AutoScaleFactor` 也不受影响

####

#### 适配DPI后字体依旧模糊❓

[解决字体模糊问题](BlurredFont.md)

####

</details>

---

<details open>
<summary><strong>🧰 控件</strong></summary>

### 通用 `2`

#### [Button 按钮](控件/Button.md)
[![Button](Icon/Button.jpg)](控件/Button.md)

#### [FloatButton 悬浮按钮](控件/FloatButton.md)
[![FloatButton](Icon/FloatButton.jpg)](控件/FloatButton.md)

### 布局 `4`

#### [Divider 分割线](控件/Divider.md)
[![Divider](Icon/Divider.jpg)](控件/Divider.md)

#### [StackPanel 堆栈布局](控件/StackPanel.md)
[![StackPanel](Icon/StackPanel.jpg)](控件/StackPanel.md)

#### [FlowPanel 流动布局](控件/FlowPanel.md)
[![FlowPanel](Icon/FlowPanel.jpg)](控件/FlowPanel.md)

#### [GridPanel 格栅布局](控件/GridPanel.md)
[![GridPanel](Icon/GridPanel.jpg)](控件/GridPanel.md)


### 导航 `6`

#### [Breadcrumb 面包屑](控件/Breadcrumb.md)
[![Breadcrumb](Icon/Breadcrumb.jpg)](控件/Breadcrumb.md)

#### [Dropdown 下拉菜单](控件/Dropdown.md)
[![Dropdown](Icon/Dropdown.jpg)](控件/Dropdown.md)

#### [Menu 导航菜单](控件/Menu.md)
#### [PageHeader 页头](控件/PageHeader.md)
#### [Pagination 分页](控件/Pagination.md)
#### [Steps 步骤条](控件/Steps.md)


### 数据录入 `13`

#### [Checkbox 多选框](控件/Checkbox.md)
#### [ColorPicker 颜色选择器](控件/ColorPicker.md)
#### [DatePicker 日期选择框](控件/DatePicker.md)
#### [DatePickerRange 日期范围选择框](控件/DatePicker#datepickerrange.md)
#### [Input 输入框](控件/Input.md)
#### [InputNumber 数字输入框](控件/Input#inputnumber.md)
#### [Radio 单选框](控件/Radio.md)
#### [Rate 评分](控件/Rate.md)
#### [Select 选择器](控件/Select.md)
#### [Slider 滑动输入条](控件/Slider.md)
#### [SliderRange 滑动范围输入条](控件/Slider#sliderrange.md)
#### [Switch 开关](控件/Switch.md)
#### [TimePicker 时间选择框](控件/TimePicker.md)
#### [UploadDragger 拖拽上传](控件/UploadDragger.md)
[![UploadDragger](Icon/UploadDragger.jpg)](控件/UploadDragger.md)


### 数据展示 `16`

#### [Avatar 头像](控件/Avatar.md)
#### [Badge 徽标数](控件/Badge.md)
#### [Calendar 日历](控件/Calendar.md)
#### [Panel 面板](控件/Panel.md)
#### [Carousel 走马灯](控件/Carousel.md)
#### [Collapse 折叠面板](控件/Collapse.md)
#### [Preview 图片预览](控件/Preview.md)
#### [Popover 气泡卡片](控件/Popover.md)
#### [Segmented 分段控制器](控件/Segmented.md)
#### [Table 表格](控件/Table.md)
#### [Tabs 标签页](控件/Tabs.md)
#### [Tag 标签](控件/Tag.md)
#### [Timeline 时间轴](控件/Timeline.md)
#### [Tooltip 文字提示](控件/Tooltip.md)
#### [Tree 树形控件](控件/Tree.md)
#### [Label 文本](控件/Label.md)


### 反馈 `7`

#### [Alert 警告提示](控件/Alert.md)
#### [Drawer 抽屉](控件/Drawer.md)
#### [Message 全局提示](控件/Message.md)
#### [Modal 对话框](控件/Modal.md)
#### [Notification 通知提醒框](控件/Notification.md)
#### [Progress 进度条](控件/Progress.md)
#### [Spin 加载中](控件/Spin.md)


### 其他 `5`

#### [WindowBar 窗口栏](控件/WindowBar.md)
#### [Battery 电量](控件/Battery.md)
#### [Signal 信号强度](控件/Signal.md)
#### [ContextMenuStrip 右键菜单](控件/ContextMenuStrip.md)
#### [Image3D 图片3D](控件/Image3D.md)

</details>

---

<details open>
<summary><strong>🪟 窗口</strong></summary>

#### [Window](窗口/Window.md)
#### [BorderlessForm](窗口/BorderlessForm.md)
#### [BaseForm](窗口/BaseForm.md)

</details>