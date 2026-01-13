[首页](Home.md)・[更新日志](UpdateLog.md)・[配置](Config.md)・[主题](Theme.md)

内置AntDesign色彩算法

> 参考地址：[https://ant-design.antgroup.com/docs/spec/colors-cn](https://ant-design.antgroup.com/docs/spec/colors-cn)

---

## 默认品牌色彩

| 模式 | HEX |
| :--: | :--: |
| 浅色 | #1677FF |
| 深色 | #1668DC |

---

## 主题色彩定制

### 1. 智能色彩体系生成（推荐）

通过以下方法设置基础颜色，系统会**自动计算并生成完整的色彩体系**，包括悬浮、按下等状态的颜色变化，确保视觉一致性：

| 方法 | 描述 | 参数 | 自动生成色彩体系 |
| :-- | :-- | :-- | :--: |
| **SetPrimary** | 设置品牌主色 | Color primary | ✅ |
| **SetSuccess** | 设置成功色 | Color success | ✅ |
| **SetWarning** | 设置警告色 | Color warning | ✅ |
| **SetError** | 设置错误色 | Color error | ✅ |
| **SetInfo** | 设置信息色 | Color info | ✅ |

#### 使用示例

``` csharp
// 自动生成完整的品牌色彩体系（包括悬浮、按下等状态）
AntdUI.Style.SetPrimary(Color.FromArgb(0, 173, 154));
```

### 2. 单个颜色值设置

以下方法**仅设置单个颜色值**，不会自动生成色彩体系，适用于需要精确控制特定颜色的场景：

#### 全局设置单个颜色值

``` csharp
// 仅设置单个主色值，不生成色彩体系
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154));
```

#### 为组件单独设置颜色值

``` csharp
// 仅为 Button 组件设置单个主色值，不生成色彩体系
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154), "Button");
```

### 3. 色彩体系配置文件

通过配置文件批量设置色彩，**建议使用HEX格式**：

``` csharp
var colorConfig = new Dictionary<string, string> {
    { "Primary", "#ED4192" },          // 品牌主色
    { "PrimaryButton", "#E0282E" },    // 为 Button 单独设置主色
    { "Success", "#52C41A" },          // 成功色
    { "Warning", "#FAAD14" },          // 警告色
    { "Error", "#F5222D" },            // 错误色
    { "Info", "#1890FF" }              // 信息色
};
AntdUI.Style.LoadCustom(colorConfig);
```

---

## 主题模式管理

### 1. 启用深色模式

> 启用深色模式后，窗口背景与字体颜色需要自行设置

``` csharp
// 启用深色模式
AntdUI.Config.IsDark = true;
```

### 2. 主题切换配置

#### 全局主题配置

> 在 `Program.cs` 中全局设置主题切换配置

``` csharp
// 设置全局主题切换配置
AntdUI.Config.Theme()
    .Dark("#000", "#fff") // 深色模式背景色和文本色
    .Light("#fff", "#000"); // 浅色模式背景色和文本色
```

#### 窗口主题配置

> 基于 `AntdUI.BaseForm` 的窗口可以定制主题切换配置

``` csharp
// 在窗口中设置主题切换配置
public partial class Form1 : AntdUI.BaseForm
{
    public Form1()
    {
        InitializeComponent();
        
        // 定制窗口主题切换配置
        Theme()
            .Dark("#1e1e1e", "#ffffff") // 深色模式背景色和文本色
            .Light("#ffffff", "#000000") // 浅色模式背景色和文本色
            .Header(header1, Color.FromArgb(240, 242, 245), Color.FromArgb(18, 18, 18)) // 页面头部颜色
            .Button(btnTheme); // 主题切换按钮
    }
    
    // 主题切换按钮点击事件
    private void btnTheme_Click(object sender, EventArgs e)
    {
        // 切换主题模式
        AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
    }
}
```

#### 主题切换配置方法

| 方法 | 描述 | 参数 |
| :-- | :-- | :-- |
| **Dark** | 设置深色模式颜色 | Color back, Color fore / string back, string fore |
| **Light** | 设置浅色模式颜色 | Color back, Color fore / string back, string fore |
| **Header** | 设置页面头部颜色 | PageHeader header, Color light, Color dark / string light, string dark |
| **Button** | 设置主题切换按钮 | Button button |
| **Call** | 设置主题切换回调 | Action<bool> call |
| **Light** | 设置浅色模式回调 | Action call |
| **Dark** | 设置深色模式回调 | Action call |

---

## 色彩工具

### 访问色彩体系数据库

``` csharp
// 获取当前主题的色彩体系
var primaryPalette = AntdUI.Style.Db.Primary; // 品牌主色彩体系
var successPalette = AntdUI.Style.Db.Success; // 成功色彩体系
var warningPalette = AntdUI.Style.Db.Warning; // 警告色彩体系
var errorPalette = AntdUI.Style.Db.Error;     // 错误色彩体系
var infoPalette = AntdUI.Style.Db.Info;       // 信息色彩体系
```

### 颜色转换方法

| 方法 | 描述 | 返回值 | 参数 |
| :-- | :-- | :-- | :-- |
| **ToHSV** | 颜色转HSV | HSV | Color color `颜色` |
| **HSVToColor** | HSV转颜色 | Color | HSV hsv, float alpha = 1 `透明度` |
| **HSVToColor** | HSV转颜色 | Color | float hue `色相`, float saturation `饱和度`, float value `明度`, float alpha = 1 `透明度` |
||||
| **ToHSL** | 颜色转HSL | HSL | Color color `颜色` |
| **HSLToColor** | HSL转颜色 | Color | HSL hsl, float alpha = 1 `透明度` |
| **HSLToColor** | HSL转颜色 | Color | float hue `色相`, float saturation `饱和度`, float lightness `亮度`, float alpha = 1 `透明度` |
||||
| **ToColor** | HEX转成RGB | Color | string hex |
| **ToHex** | RGB转成HEX | string | Color color |
||||
| **rgba** | 转颜色 | Color | int r, int g, int b, float alpha = 1 `透明度` |
| **rgba** | 转颜色 | Color | Color color, float alpha = 1 `透明度` |