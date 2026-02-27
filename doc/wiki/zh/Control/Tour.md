[首页](../Home.md)・[更新日志](../UpdateLog.md)・[配置](../Config.md)・[主题](../Theme.md)

## Tour

Tour 漫游式引导 👚

> 用于分步引导用户了解产品功能的气泡组件。

### 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**open** | 打开漫游式引导 | TourForm | Form `所属窗口`, Action<Result> `步骤回调`, Action<Popover> `气泡回调` |
**open** | 打开漫游式引导 | TourForm | Config `配置` |

### 配置

#### Config 属性

名称 | 描述 | 类型 | 默认值 |
:--|:--|:--|:--|
**Form** | 所属窗口 | Form | - |
**Scale** | 缩放 | float | 1.04F |
**MaskClosable** | 点击蒙层是否允许关闭 | bool | true |
**ClickNext** | 点击下一步 | bool | true |
**StepCall** | 步骤回调 | Action<Result> | - |
**PopoverCall** | 气泡回调 | Action<Popover>`?` | `null` |

#### Config 方法

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**SetScale** | 设置缩放 | Config | float `值` |
**SetMaskClosable** | 设置点击蒙层是否允许关闭 | Config | bool `值` |
**SetClickNext** | 设置点击下一步 | Config | bool `值` |
**SetCall** | 设置步骤回调 | Config | Action<Result> `值` |
**SetStepCall** | 设置气泡回调 | Config | Action<Popover> `值` |

### 辅助类

#### Result 类

**属性**

名称 | 描述 | 类型 |
:--|:--|:--|
**Index** | 第几步 | int |

**方法**

名称 | 描述 | 参数 |
:--|:--|:--|
**Set** | 设置控件 | Control `控件` |
**Set** | 设置区域 | Rectangle `区域` |
**Close** | 关闭 | - |

#### Popover 类

**属性**

名称 | 描述 | 类型 |
:--|:--|:--|
**Form** | 所属窗口 | Form |
**Tour** | 引导 | TourForm |
**Index** | 第几步 | int |
**Rect** | 显示区域 | Rectangle`?` |

#### TourForm 接口

**方法**

名称 | 描述 | 参数 |
:--|:--|:--|
**Close** | 关闭 | - |
**IClose** | 内部关闭 | bool `是否释放` |
**Previous** | 上一步 | - |
**Next** | 下一步 | - |
**LoadData** | 加载数据 | - |

### 示例

```csharp
// 基本使用
Tour.open(this, (result) => {
	switch (result.Index)
	{
		case 0:
			result.Set(button1); // 引导到按钮1
			break;
		case 1:
			result.Set(textBox1); // 引导到文本框1
			break;
		case 2:
			result.Set(new Rectangle(100, 100, 200, 100)); // 引导到指定区域
			break;
		case 3:
			result.Close(); // 关闭引导
			break;
	}
}, (popover) => {
	// 在这里创建气泡内容
	var form = new Form();
	form.Text = $"步骤 {popover.Index + 1}";
	form.Size = new Size(200, 100);
	// 设置气泡位置
	// ...
	form.Show(popover.Form);
});

// 完整配置
var config = new Tour.Config(this, (result) => {
	// 步骤回调
}) 
.SetScale(1.1F) // 设置缩放
.SetMaskClosable(false) // 点击蒙层不关闭
.SetClickNext(false); // 点击不自动下一步

Tour.open(config);
```