![banner](screenshot/banner.png?raw=true)

<p align="center">
    <a href="README.md">中文</a>
    ❤
    <a href="README-en.md">English</a>
</p>

# AntdUI
Winform UI library use Ant Design 5.0

[![AntDesign](https://img.shields.io/badge/AntDesign%20-5.0-1677ff?style=for-the-badge&logo=antdesign)](https://ant-design.antgroup.com/components/overview-cn)
[![NuGet](https://img.shields.io/nuget/v/AntdUI.svg?style=for-the-badge&label=AntdUI&logo=nuget)](https://www.nuget.org/packages/AntdUI)
[![QQGroup](https://img.shields.io/badge/QQ群-328884096-f74658?style=for-the-badge&logo=tencentqq)](https://qm.qq.com/cgi-bin/qm/qr?k=ZfuHy4LqYC57DYTWAUWkQD9EjdVfvx3y&jump_from=webapi&authKey=4sAgZN0XlFHx+4MW9PdkiGgg435QfKcQdu5lKi1Fp4PP0O+DL6NaKAcV8ybCLM97)
[![License](https://img.shields.io/badge/license-Apache%202.0-4EB1BA.svg?style=for-the-badge)](http://www.apache.org/licenses/LICENSE-2.0)

**[文档 Wiki](https://gitee.com/antdui/AntdUI/wikis)** 👈点击跳转，请查看后再提问

---

![DEMO](screenshot/Pre/Demo.png?raw=true)
![OverView](screenshot/Pre/OverView.png?raw=true)
![Scale150](screenshot/Pre/Scale150.png?raw=true)

### ✨特性

- 🌈 纯GDI绘制，没有图片，支持AOT
- 🎨 高质量绘图，高性能动画
- 🚀 Winform上最阴影的阴影效果
- 📦 无边框窗口，拥有原生窗口特性
- 💎 3D翻转效果
- 👚 主题配置
- 🦜 SVG矢量图
- 👓 DPI适配

### 🖥支持环境

- .NET 6.0及以上。
- .NET Framework4.8及以上。
- .NET Framework4.0及以上。

### 🌴控件

:arrow_down: | 通用 `2` | 动画 | 禁用 |
:---:|:--|:--:|:--:|
:arrow_right: | **Button** 按钮 | ✅ | ✅ |
:arrow_right: | **FloatButton** 悬浮按钮 | ✅ | ❎ |
||||
:arrow_down: | 布局 `4` | 动画 | 禁用 |
:arrow_right: | **Divider** 分割线 | ❎ | ❎ |
:arrow_right: | **StackPanel** 堆栈布局 | ❎ | ❎ |
:arrow_right: | **FlowPanel** 流动布局 | ❎ | ❎ |
:arrow_right: | **GridPanel** 格栅布局 | ❎ | ❎ |
||||
:arrow_down: | 导航 `4` | 动画 | 禁用 |
:arrow_right: | **Dropdown** 下拉菜单 | ✅ | ✅ |
:arrow_right: | **Menu** 导航菜单 | ✅ | ❎ |
:arrow_right: | **Pagination** 分页 | ✅ | ✅ |
:arrow_right: | **Steps** 步骤条 | ❎ | ❎ |
||||
:arrow_down: | 数据录入 `12` | 动画 | 禁用 |
:arrow_right: | **Checkbox** 多选框 | ✅ | ✅ |
:arrow_right: | **ColorPicker** 颜色选择器 | ✅ | ✅ |
:arrow_right: | **DatePicker** 日期选择框 | ✅ | ✅ |
:arrow_right: | **DatePickerRange** 日期范围选择框 | ✅ | ✅ |
:arrow_right: | **Input** 输入框 | ✅ | ✅ |
:arrow_right: | **InputNumber** 数字输入框 | ✅ | ✅ |
:arrow_right: | **Radio** 单选框 | ✅ | ✅ |
:arrow_right: | **Rate** 评分 | ✅ | ❎ |
:arrow_right: | **Select** 选择器 | ✅ | ✅ |
:arrow_right: | **Slider** 滑动输入条 | ✅ | ❎ |
:arrow_right: | **Switch** 开关 | ✅ | ✅ |
:arrow_right: | **TimePicker** 时间选择框 | ✅ | ✅ |
||||
:arrow_down: | 数据展示 `13` | 动画 | 禁用 |
:arrow_right: | **Avatar** 头像 | ❎ | ❎ |
:arrow_right: | **Badge** 徽标数 | ✅ | ❎ |
:arrow_right: | **Panel** 面板 | ✅ | ❎ |
:arrow_right: | **Carousel** 走马灯 | ✅ | ❎ |
:arrow_right: | **Popover** 气泡卡片 | ✅ | ❎ |
:arrow_right: | **Segmented** 分段控制器 | ✅ | ❎
:arrow_right: | **Table** 表格 | ✅ | ❎ | |
:arrow_right: | **Tabs** 标签页 | ✅ | ❎ |
:arrow_right: | **Tag** 标签 | ✅ | ❎ |
:arrow_right: | **Timeline** 时间轴 | ❎ | ❎ |
:arrow_right: | **Tooltip** 文字提示 | ✅ | ❎ |
:arrow_right: | **Tree** 树形控件 | ✅ | ✅ |
:arrow_right: | **Lable** 标签 | ✅ | ❎ |
||||
:arrow_down: | 反馈 `7` | 动画 | 禁用 |
:arrow_right: | **Alert** 警告提示 | ✅ | ❎ |
:arrow_right: | **Drawer** 抽屉 | ✅ | ❎ |
:arrow_right: | **Message** 全局提示 | ✅ | ❎ |
:arrow_right: | **Modal** 对话框 | ✅ | ❎ |
:arrow_right: | **Notification** 通知提醒框 | ✅ | ❎ |
:arrow_right: | **Progress** 进度条 | ✅ | ❎ |
:arrow_right: | **Spin** 加载中 | ✅ | ❎ |
||||
:arrow_down: | 其他 `3` | 动画 | 禁用 |
:arrow_right: | **WindowBar** 窗口栏 | ✅ | ❎ |
:arrow_right: | **ContextMenuStrip** 右键菜单 | ✅ | ❎ |
:arrow_right: | **Image3D** 图片3D | ✅ | ❎ |  

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