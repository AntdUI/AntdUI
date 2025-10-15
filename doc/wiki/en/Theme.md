[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)d)

Built in AntDesign color algorithm

> reference address: [https://ant-design.antgroup.com/docs/spec/colors-cn](https://ant-design.antgroup.com/docs/spec/colors-cn)

---

Default brand color

mode|HEX|
:--:|:--:|
Light|#1677FF|
Dark|#1668DC|

### method

Name | Description | Parameters | Auto Color Matching |
:--|:--|:--|:--:|
**SetPrimary** | Set brand color | Color primary |✅|
**SetSuccess** | Set success color | Color success |✅|
**SetWarning** | Set warning color | Color warning |✅|
**SetError** | Set error color | Color error |✅|
**SetInfo** | Set info color | Color info |✅|


#### Custom Theme

> Global setting theme color

``` csharp
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154));
```

> Set a separate theme color for the Button

``` csharp
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154), "Button");
```

#### Current theme setting brand color

``` csharp
AntdUI.Style.SetPrimary(Color.FromArgb(0, 173, 154));
```

#### Enable dark mode

> Window background and font color need to be set by oneself

``` csharp
AntdUI.Config.IsDark = true;
```

#### Retrieve color card database

``` csharp
var primary = AntdUI.Style.Db.Primary;// Get the current theme brand color
```

#### Color card configuration file

> Suggest using HEX format

``` csharp
var dir = new Dictionary<string, string> {
    { "Primary", "#1677FF" },
    { "PrimaryButton", "#1677FF" } // Set a separate theme color for the Button
};
AntdUI.Style.LoadCustom(dir);
```


### Static Help Class

#### color conversion

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**ToHSV** | Color to HSV | HSV | Color color |
**HSVToColor** | HSV to Color | Color | HSV hsv, float alpha = 1 |
**HSVToColor** | HSV to Color | Color | float hue, float saturation, float value, float alpha = 1 |
||||
**ToHSL** | Color to HSL | HSL | Color color |
**HSLToColor** | HSL to Color | Color | HSL hsl, float alpha = 1 |
**HSLToColor** | HSL to Color | Color | float hue, float saturation, float lightness, float alpha = 1 |
||||
**ToColor** | HEX to RGB | Color | string hex |
**ToHex** | RGB to HEX | string | Color color |
||||
**rgba** | | Color | int r, int g, int b, float alpha = 1 |
**rgba** | | Color | Color color, float alpha = 1 |