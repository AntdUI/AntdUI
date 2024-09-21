<div align="center">

<img height="100" src="src/logo.png">

<h1>AntdUI</h1>

Winform UI library use Ant Design 5.0

[![AntDesign](https://img.shields.io/badge/AntDesign%20-5.0-1677ff?style=for-the-badge&logo=antdesign)](https://ant-design.antgroup.com/components/overview-cn)
[![NuGet](https://img.shields.io/nuget/v/AntdUI.svg?style=for-the-badge&label=AntdUI&logo=nuget)](https://www.nuget.org/packages/AntdUI)
[![Download](https://img.shields.io/nuget/dt/antdui?style=for-the-badge)](https://www.nuget.org/packages/AntdUI)
[![QQGroup](https://img.shields.io/badge/QQ群-328884096-f74658?style=for-the-badge&logo=tencentqq)](https://qm.qq.com/cgi-bin/qm/qr?k=ZfuHy4LqYC57DYTWAUWkQD9EjdVfvx3y&jump_from=webapi&authKey=4sAgZN0XlFHx+4MW9PdkiGgg435QfKcQdu5lKi1Fp4PP0O+DL6NaKAcV8ybCLM97)
[![License](https://img.shields.io/badge/license-Apache%202.0-4EB1BA.svg?style=for-the-badge)](http://www.apache.org/licenses/LICENSE-2.0)

中文・[English](README-en.md)・[文档](https://gitee.com/antdui/AntdUI/wikis)

</div>

![banner](screenshot/Pre/banner.png?raw=true)

### ✨特性

- 🌈 纯GDI绘制，没有图片，支持AOT
- 🎨 高质量绘图，高性能动画
- 🚀 Winform上最阴影的阴影效果
- 📦 无边框窗口，拥有原生窗口特性
- 💎 3D翻转效果
- 👚 主题配置
- 🦜 SVG矢量图
- 👓 DPI适配
- 🌍 国际化

### 🖥支持环境

- .NET 6.0及以上。
- .NET Framework4.8及以上。
- .NET Framework4.0及以上。

### 🌴控件

⬇️| 通用 `2` | 动画 | 禁用 |
:---:|:--|:--:|:--:|
➡️| [**Button** 按钮](https://gitee.com/antdui/AntdUI/wikis/控件/Button) | ✅ | ✅ |
➡️| [**FloatButton** 悬浮按钮](https://gitee.com/antdui/AntdUI/wikis/控件/FloatButton) | ✅ | ❎ |
||||
⬇️| 布局 `4` | 动画 | 禁用 |
➡️| [**Divider** 分割线](https://gitee.com/antdui/AntdUI/wikis/控件/Divider) | ❎ | ❎ |
➡️| **StackPanel** 堆栈布局 | ❎ | ❎ |
➡️| **FlowPanel** 流动布局 | ❎ | ❎ |
➡️| **GridPanel** 格栅布局 | ❎ | ❎ |
||||
⬇️| 导航 `6` | 动画 | 禁用 |
➡️| [**Breadcrumb** 面包屑](https://gitee.com/antdui/AntdUI/wikis/控件/Breadcrumb) | ✅ | ❎ |
➡️| [**Dropdown** 下拉菜单](https://gitee.com/antdui/AntdUI/wikis/控件/Dropdown) | ✅ | ✅ |
➡️| [**Menu** 导航菜单](https://gitee.com/antdui/AntdUI/wikis/控件/Menu) | ✅ | ❎ |
➡️| [**PageHeader** 页头](https://gitee.com/antdui/AntdUI/wikis/控件/PageHeader) | ✅ | ❎ |
➡️| [**Pagination** 分页](https://gitee.com/antdui/AntdUI/wikis/控件/Pagination) | ✅ | ✅ |
➡️| [**Steps** 步骤条](https://gitee.com/antdui/AntdUI/wikis/控件/Steps) | ❎ | ❎ |
||||
⬇️| 数据录入 `12` | 动画 | 禁用 |
➡️| [**Checkbox** 多选框](https://gitee.com/antdui/AntdUI/wikis/控件/Checkbox) | ✅ | ✅ |
➡️| [**ColorPicker** 颜色选择器](https://gitee.com/antdui/AntdUI/wikis/控件/ColorPicker) | ✅ | ✅ |
➡️| [**DatePicker** 日期选择框](https://gitee.com/antdui/AntdUI/wikis/控件/DatePicker) | ✅ | ✅ |
➡️| [**DatePickerRange** 日期范围选择框](https://gitee.com/antdui/AntdUI/wikis/控件/DatePicker#DatePickerRange) | ✅ | ✅ |
➡️| [**Input** 输入框](https://gitee.com/antdui/AntdUI/wikis/控件/Input) | ✅ | ✅ |
➡️| [**InputNumber** 数字输入框](https://gitee.com/antdui/AntdUI/wikis/控件/Input#InputNumber) | ✅ | ✅ |
➡️| [**Radio** 单选框](https://gitee.com/antdui/AntdUI/wikis/控件/Radio) | ✅ | ✅ |
➡️| [**Rate** 评分](https://gitee.com/antdui/AntdUI/wikis/控件/Rate) | ✅ | ❎ |
➡️| [**Select** 选择器](https://gitee.com/antdui/AntdUI/wikis/控件/Select) | ✅ | ✅ |
➡️| [**Slider** 滑动输入条](https://gitee.com/antdui/AntdUI/wikis/控件/Slider) | ✅ | ❎ |
➡️| [**Switch** 开关](https://gitee.com/antdui/AntdUI/wikis/控件/Switch) | ✅ | ✅ |
➡️| [**TimePicker** 时间选择框](https://gitee.com/antdui/AntdUI/wikis/控件/TimePicker) | ✅ | ✅ |
||||
⬇️| 数据展示 `16` | 动画 | 禁用 |
➡️| [**Avatar** 头像](https://gitee.com/antdui/AntdUI/wikis/控件/Avatar) | ❎ | ❎ |
➡️| [**Badge** 徽标数](https://gitee.com/antdui/AntdUI/wikis/控件/Badge) | ✅ | ❎ |
➡️| [**Calendar** 日历](https://gitee.com/antdui/AntdUI/wikis/控件/Calendar) | ✅ | ❎ |
➡️| [**Panel** 面板](https://gitee.com/antdui/AntdUI/wikis/控件/Panel) | ✅ | ❎ |
➡️| [**Carousel** 走马灯](https://gitee.com/antdui/AntdUI/wikis/控件/Carousel) | ✅ | ❎ |
➡️| [**Collapse** 折叠面板](https://gitee.com/antdui/AntdUI/wikis/控件/Collapse) | ✅ | ❎ |
➡️| [**Preview** 图片预览](https://gitee.com/antdui/AntdUI/wikis/控件/Preview) | ✅ | ✅ |
➡️| [**Popover** 气泡卡片](https://gitee.com/antdui/AntdUI/wikis/控件/Popover) | ✅ | ❎ |
➡️| [**Segmented** 分段控制器](https://gitee.com/antdui/AntdUI/wikis/控件/Segmented) | ✅ | ✅ |
➡️| [**Table** 表格](https://gitee.com/antdui/AntdUI/wikis/控件/Table) | ✅ | ❎ |
➡️| [**Tabs** 标签页](https://gitee.com/antdui/AntdUI/wikis/控件/Tabs) | ✅ | ❎ |
➡️| [**Tag** 标签](https://gitee.com/antdui/AntdUI/wikis/控件/Tag) | ✅ | ❎ |
➡️| [**Timeline** 时间轴](https://gitee.com/antdui/AntdUI/wikis/控件/Timeline) | ❎ | ❎ |
➡️| [**Tooltip** 文字提示](https://gitee.com/antdui/AntdUI/wikis/控件/Tooltip) | ✅ | ❎ |
➡️| [**Tree** 树形控件](https://gitee.com/antdui/AntdUI/wikis/控件/Tree) | ✅ | ✅ |
➡️| [**Label** 文本](https://gitee.com/antdui/AntdUI/wikis/控件/Label) | ✅ | ❎ |
||||
⬇️| 反馈 `7` | 动画 | 禁用 |
➡️| [**Alert** 警告提示](https://gitee.com/antdui/AntdUI/wikis/控件/Alert) | ✅ | ❎ |
➡️| [**Drawer** 抽屉](https://gitee.com/antdui/AntdUI/wikis/控件/Drawer) | ✅ | ❎ |
➡️| [**Message** 全局提示](https://gitee.com/antdui/AntdUI/wikis/控件/Message) | ✅ | ❎ |
➡️| [**Modal** 对话框](https://gitee.com/antdui/AntdUI/wikis/控件/Modal) | ✅ | ❎ |
➡️| [**Notification** 通知提醒框](https://gitee.com/antdui/AntdUI/wikis/控件/Notification) | ✅ | ❎ |
➡️| [**Progress** 进度条](https://gitee.com/antdui/AntdUI/wikis/控件/Progress) | ✅ | ❎ |
➡️| [**Spin** 加载中](https://gitee.com/antdui/AntdUI/wikis/控件/Spin) | ✅ | ❎ |
||||
⬇️| 聊天 `2` | 动画 | 禁用 |
➡️| **MsgList** 好友消息列表 | ✅ | ❎ |
➡️| **ChatList** 气泡聊天列表 | ✅ | ❎ |
||||
⬇️| 其他 `5` | 动画 | 禁用 |
➡️| [**WindowBar** 窗口栏](https://gitee.com/antdui/AntdUI/wikis/控件/WindowBar) | ✅ | ❎ |
➡️| [**Battery** 电量](https://gitee.com/antdui/AntdUI/wikis/控件/Battery) | ✅ | ❎ |
➡️| [**Signal** 信号强度](https://gitee.com/antdui/AntdUI/wikis/控件/Signal) | ✅ | ❎ |
➡️| [**ContextMenuStrip** 右键菜单](https://gitee.com/antdui/AntdUI/wikis/控件/ContextMenuStrip) | ✅ | ❎ |
➡️| **Image3D** 图片3D | ✅ | ❎ |

### 🐿️捐赠 🥣💲🐖👚
![Payment](screenshot/Pre/Payment.png?raw=true)

### 🎨截图

#### ChatUI

> 纯GDI，不是TextBox等拖控件

![ChatUI](screenshot/ChatUI.gif?raw=true)

#### 控件

| **Button** 按钮 | **Badge** 徽标数 |
| :--: | :--: |
| ![Button](screenshot/Button.gif?raw=true) | ![Badge](screenshot/Badge.gif?raw=true) |
| **Carousel** 走马灯 | **Input** 输入框 |
| ![Carousel](screenshot/Carousel.gif?raw=true) | ![Input](screenshot/Input.gif?raw=true) |
| **Progress** 进度条 | **Avatar** 头像 |
| ![Progress](screenshot/Progress.gif?raw=true) | ![Avatar](screenshot/Avatar.gif?raw=true) |
| **Checkbox** 多选框 | **Radio** 单选框 |
| ![Checkbox](screenshot/Checkbox.gif?raw=true) | ![Radio](screenshot/Radio.gif?raw=true) |
| **Tooltip** 提示 | **Panel** 面板 |
| ![Tooltip](screenshot/Tooltip.gif?raw=true) | ![Panel](screenshot/Panel.gif?raw=true) |
| **Tabs** 标签页 | **Alert** 警告提示 |
| ![Tabs](screenshot/Tabs.gif?raw=true) | ![Alert](screenshot/Alert.gif?raw=true) |
| **Segmented** 分段控制器 | **Menu** 导航菜单 |
| ![Segmented](screenshot/Segmented.gif?raw=true) | ![Menu](screenshot/Menu.gif?raw=true) |
| **Divider** 分割线 | **Slider** 滑动条 |
| ![Divider](screenshot/Divider.gif?raw=true) | ![Slider](screenshot/Slider.gif?raw=true) |
| **Message** 全局提示 | **Notification** 通知提醒框 |
| ![Message](screenshot/Message.gif?raw=true) | ![Notification](screenshot/Notification.gif?raw=true) |
| **Switch** 开关 | **Table** 表格 |
| ![Switch](screenshot/Switch.gif?raw=true) | ![Table](screenshot/Table.gif?raw=true) |

🦦 招募小伙伴一起维护项目