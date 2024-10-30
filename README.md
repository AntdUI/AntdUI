<div align="center">

<img height="100" src="src/logo.png">

<h1>AntdUI</h1>

[![AntDesign](https://img.shields.io/badge/AntDesign%20-5.0-1677ff?style=for-the-badge&logo=antdesign)](https://ant-design.antgroup.com/components/overview-cn)
[![NuGet](https://img.shields.io/nuget/v/AntdUI.svg?style=for-the-badge&label=AntdUI&logo=nuget)](https://www.nuget.org/packages/AntdUI)
[![Download](https://img.shields.io/nuget/dt/antdui?style=for-the-badge)](https://www.nuget.org/packages/AntdUI)
[![QQGroup](https://img.shields.io/badge/QQ群-328884096-f74658?style=for-the-badge&logo=tencentqq)](https://qm.qq.com/cgi-bin/qm/qr?k=ZfuHy4LqYC57DYTWAUWkQD9EjdVfvx3y&jump_from=webapi&authKey=4sAgZN0XlFHx+4MW9PdkiGgg435QfKcQdu5lKi1Fp4PP0O+DL6NaKAcV8ybCLM97)
[![License](https://img.shields.io/badge/license-Apache%202.0-4EB1BA.svg?style=for-the-badge)](http://www.apache.org/licenses/LICENSE-2.0)

中文・[English](README-en.md)・[文档](doc/wiki/zh/Home.md)・[演示](https://gitee.com/mubaiyanghua/antdui-demo)

</div>

![banner](doc/pre/banner.png)

### 🦄 介绍

基于 Ant Design 设计语言的 WinForm UI 界面库，致力于将现代美观的前端设计风格带入到桌面应用程序中。采用纯GDI绘图，不需任何图片资源，全面支持AOT，最低兼容 `.NET Framework 4.0`

### ✨ 特性

- 🌈 现代化的设计风格
- 🎨 精细绘制与流畅动画
- 🚀 在 Winform 上呈现最佳阴影效果
- 📦 无边框窗口，保留原生窗口特性
- 💎 3D翻转特效
- 👚 主题自定义
- 🦜 SVG 矢量图
- 👓 DPI 自适应
- 🌍 全球化支持

### 🖥 环境

- .NET 6.0及更高版本。
- .NET Framework4.8及以上。
- .NET Framework4.0及以上。

### 🌴 控件

⬇️| 通用 `2` | 动画 | 禁用 |
:---:|:--|:--:|:--:|
➡️| [**Button** 按钮](doc/zh/控件/Button.md) | ✅ | ✅ |
➡️| [**FloatButton** 悬浮按钮](doc/zh/控件/FloatButton.md) | ✅ | ❎ |
||||
⬇️| 布局 `4` | 动画 | 禁用 |
➡️| [**Divider** 分割线](doc/zh/控件/Divider.md) | ❎ | ❎ |
➡️| [**StackPanel** 堆栈布局](doc/zh/控件/StackPanel.md) | ❎ | ❎ |
➡️| [**FlowPanel** 流动布局](doc/zh/控件/FlowPanel.md) | ❎ | ❎ |
➡️| [**GridPanel** 格栅布局](doc/zh/控件/GridPanel.md) | ❎ | ❎ |
||||
⬇️| 导航 `6` | 动画 | 禁用 |
➡️| [**Breadcrumb** 面包屑](doc/zh/控件/Breadcrumb.md) | ✅ | ❎ |
➡️| [**Dropdown** 下拉菜单](doc/zh/控件/Dropdown.md) | ✅ | ✅ |
➡️| [**Menu** 导航菜单](doc/zh/控件/Menu.md) | ✅ | ❎ |
➡️| [**PageHeader** 页头](doc/zh/控件/PageHeader.md) | ✅ | ❎ |
➡️| [**Pagination** 分页](doc/zh/控件/Pagination.md) | ✅ | ✅ |
➡️| [**Steps** 步骤条](doc/zh/控件/Steps.md) | ❎ | ❎ |
||||
⬇️| 数据录入 `13` | 动画 | 禁用 |
➡️| [**Checkbox** 多选框](doc/zh/控件/Checkbox.md) | ✅ | ✅ |
➡️| [**ColorPicker** 颜色选择器](doc/zh/控件/ColorPicker.md) | ✅ | ✅ |
➡️| [**DatePicker** 日期选择框](doc/zh/控件/DatePicker.md) | ✅ | ✅ |
➡️| [**DatePickerRange** 日期范围选择框](doc/zh/控件/DatePicker#DatePickerRange.md) | ✅ | ✅ |
➡️| [**Input** 输入框](doc/zh/控件/Input.md) | ✅ | ✅ |
➡️| [**InputNumber** 数字输入框](doc/zh/控件/Input#InputNumber.md) | ✅ | ✅ |
➡️| [**Radio** 单选框](doc/zh/控件/Radio.md) | ✅ | ✅ |
➡️| [**Rate** 评分](doc/zh/控件/Rate.md) | ✅ | ❎ |
➡️| [**Select** 选择器](doc/zh/控件/Select.md) | ✅ | ✅ |
➡️| [**Slider** 滑动输入条](doc/zh/控件/Slider.md) | ✅ | ❎ |
➡️| [**Switch** 开关](doc/zh/控件/Switch.md) | ✅ | ✅ |
➡️| [**TimePicker** 时间选择框](doc/zh/控件/TimePicker.md) | ✅ | ✅ |
➡️| [**UploadDragger** 拖拽上传](doc/zh/控件/UploadDragger.md) | ✅ | ❎ |
||||
⬇️| 数据展示 `16` | 动画 | 禁用 |
➡️| [**Avatar** 头像](doc/zh/控件/Avatar.md) | ❎ | ❎ |
➡️| [**Badge** 徽标数](doc/zh/控件/Badge.md) | ✅ | ❎ |
➡️| [**Calendar** 日历](doc/zh/控件/Calendar.md) | ✅ | ❎ |
➡️| [**Panel** 面板](doc/zh/控件/Panel.md) | ✅ | ❎ |
➡️| [**Carousel** 走马灯](doc/zh/控件/Carousel.md) | ✅ | ❎ |
➡️| [**Collapse** 折叠面板](doc/zh/控件/Collapse.md) | ✅ | ❎ |
➡️| [**Preview** 图片预览](doc/zh/控件/Preview.md) | ✅ | ✅ |
➡️| [**Popover** 气泡卡片](doc/zh/控件/Popover.md) | ✅ | ❎ |
➡️| [**Segmented** 分段控制器](doc/zh/控件/Segmented.md) | ✅ | ✅ |
➡️| [**Table** 表格](doc/zh/控件/Table.md) | ✅ | ❎ |
➡️| [**Tabs** 标签页](doc/zh/控件/Tabs.md) | ✅ | ❎ |
➡️| [**Tag** 标签](doc/zh/控件/Tag.md) | ✅ | ❎ |
➡️| [**Timeline** 时间轴](doc/zh/控件/Timeline.md) | ❎ | ❎ |
➡️| [**Tooltip** 文字提示](doc/zh/控件/Tooltip.md) | ✅ | ❎ |
➡️| [**Tree** 树形控件](doc/zh/控件/Tree.md) | ✅ | ✅ |
➡️| [**Label** 文本](doc/zh/控件/Label.md) | ✅ | ❎ |
||||
⬇️| 反馈 `7` | 动画 | 禁用 |
➡️| [**Alert** 警告提示](doc/zh/控件/Alert.md) | ✅ | ❎ |
➡️| [**Drawer** 抽屉](doc/zh/控件/Drawer.md) | ✅ | ❎ |
➡️| [**Message** 全局提示](doc/zh/控件/Message.md) | ✅ | ❎ |
➡️| [**Modal** 对话框](doc/zh/控件/Modal.md) | ✅ | ❎ |
➡️| [**Notification** 通知提醒框](doc/zh/控件/Notification.md) | ✅ | ❎ |
➡️| [**Progress** 进度条](doc/zh/控件/Progress.md) | ✅ | ❎ |
➡️| [**Spin** 加载中](doc/zh/控件/Spin.md) | ✅ | ❎ |
||||
⬇️| 聊天 `2` | 动画 | 禁用 |
➡️| **MsgList** 好友消息列表 | ✅ | ❎ |
➡️| **ChatList** 气泡聊天列表 | ✅ | ❎ |
||||
⬇️| 其他 `5` | 动画 | 禁用 |
➡️| [**WindowBar** 窗口栏](doc/zh/控件/WindowBar.md) | ✅ | ❎ |
➡️| [**Battery** 电量](doc/zh/控件/Battery.md) | ✅ | ❎ |
➡️| [**Signal** 信号强度](doc/zh/控件/Signal.md) | ✅ | ❎ |
➡️| [**ContextMenuStrip** 右键菜单](doc/zh/控件/ContextMenuStrip.md) | ✅ | ❎ |
➡️| **Image3D** 图片3D | ✅ | ❎ |

### 🐿️ 捐赠 🥣💲🐖👚
![Payment](doc/pre/Payment.png)

### 🎨 截图

#### ChatUI

> 纯GDI，不是TextBox等拖控件

![ChatUI](doc/screenshot/ChatUI.gif)

#### 控件

| **Button** 按钮 | **Badge** 徽标数 |
| :--: | :--: |
| ![Button](doc/screenshot/Button.gif) | ![Badge](doc/screenshot/Badge.gif) |
| **Carousel** 走马灯 | **Input** 输入框 |
| ![Carousel](doc/screenshot/Carousel.gif) | ![Input](doc/screenshot/Input.gif) |
| **Progress** 进度条 | **Avatar** 头像 |
| ![Progress](doc/screenshot/Progress.gif) | ![Avatar](doc/screenshot/Avatar.gif) |
| **Checkbox** 多选框 | **Radio** 单选框 |
| ![Checkbox](doc/screenshot/Checkbox.gif) | ![Radio](doc/screenshot/Radio.gif) |
| **Tooltip** 提示 | **Panel** 面板 |
| ![Tooltip](doc/screenshot/Tooltip.gif) | ![Panel](doc/screenshot/Panel.gif) |
| **Tabs** 标签页 | **Alert** 警告提示 |
| ![Tabs](doc/screenshot/Tabs.gif) | ![Alert](doc/screenshot/Alert.gif) |
| **Segmented** 分段控制器 | **Menu** 导航菜单 |
| ![Segmented](doc/screenshot/Segmented.gif) | ![Menu](doc/screenshot/Menu.gif) |
| **Divider** 分割线 | **Slider** 滑动条 |
| ![Divider](doc/screenshot/Divider.gif) | ![Slider](doc/screenshot/Slider.gif) |
| **Message** 全局提示 | **Notification** 通知提醒框 |
| ![Message](doc/screenshot/Message.gif) | ![Notification](doc/screenshot/Notification.gif) |
| **Switch** 开关 | **Table** 表格 |
| ![Switch](doc/screenshot/Switch.gif) | ![Table](doc/screenshot/Table.gif) |


## 📢 特别声明

AntdUI 项目已加入 [dotNET China](https://gitee.com/dotnetchina)  组织。<br/>

![dotnetchina](https://gitee.com/dotnetchina/home/raw/master/assets/dotnetchina-raw.png "dotNET China LOGO")


🦦 招募小伙伴一起维护项目