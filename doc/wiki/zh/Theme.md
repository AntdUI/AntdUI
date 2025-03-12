[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

内置AntDesign色彩算法

> 参考地址：[https://ant-design.antgroup.com/docs/spec/colors-cn](https://ant-design.antgroup.com/docs/spec/colors-cn)

---

默认品牌色

模式|HEX|
:--:|:--:|
浅色|#1677FF|
深色|#1668DC|

### 方法

名称 | 描述 | 参数 | 自动补色 |
:--|:--|:--|:--:|
**SetPrimary** | 设置品牌色 | Color primary |✅|
**SetSuccess** | 设置成功色 | Color success |✅|
**SetWarning** | 设置警戒色 | Color warning |✅|
**SetError** | 设置错误色 | Color error |✅|
**SetInfo** | 设置信息色 | Color info |✅|


#### 自定义主题

> 全局设置主题色

``` csharp
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154));
```

> 为 Button 单独设置主题色

``` csharp
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154), "Button");
```

#### 当前主题设置品牌色

``` csharp
AntdUI.Style.SetPrimary(Color.FromArgb(0, 173, 154));
```

#### 启用深色模式

> 窗口背景与字体颜色需要自己设置

``` csharp
AntdUI.Config.IsDark = true;
```

#### 获取色卡数据库

``` csharp
var primary = AntdUI.Style.Db.Primary;// 获取当前主题品牌色
```

#### 色卡配置文件

> 建议使用HEX格式

``` csharp
var dir = new Dictionary<string, string> {
    { "Primary", "#1677FF" },
    { "PrimaryButton", "#1677FF" } // 为 Button 单独设置主题色
};
AntdUI.Style.LoadCustom(dir);
```


### 静态帮助类

#### 颜色转换

名称 | 描述 | 返回值 | 参数 |
:--|:--|:--|:--|
**ToHSV** | 颜色转HSV | HSV | Color color `颜色` |
**HSVToColor** | HSV转颜色 | Color | HSV hsv, float alpha = 1 `透明度` |
**HSVToColor** | HSV转颜色 | Color | float hue `色相`, float saturation `饱和度`, float value `明度`, float alpha = 1 `透明度` |
||||
**ToHSL** | 颜色转HSL | HSL | Color color `颜色` |
**HSLToColor** | HSL转颜色 | Color | HSL hsl, float alpha = 1 `透明度` |
**HSLToColor** | HSL转颜色 | Color | float hue `色相`, float saturation `饱和度`, float lightness `亮度`, float alpha = 1 `透明度` |
||||
**ToColor** | HEX转成RGB | Color | string hex |
**ToHex** | RGB转成HEX | string | Color color |
||||
**rgba** | 转颜色 | Color | int r, int g, int b, float alpha = 1 `透明度` |
**rgba** | 转颜色 | Color | Color color, float alpha = 1 `透明度` |