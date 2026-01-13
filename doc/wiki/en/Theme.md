[Home](Home.md)・[UpdateLog](UpdateLog.md)・[Config](Config.md)・[Theme](Theme.md)

Built in AntDesign color algorithm

> reference address: [https://ant-design.antgroup.com/docs/spec/colors-cn](https://ant-design.antgroup.com/docs/spec/colors-cn)

---

## Default Brand Colors

| Mode | HEX |
| :--: | :--: |
| Light | #1677FF |
| Dark | #1668DC |

---

## Theme Color Customization

### 1. Smart Color System Generation (Recommended)

By setting base colors through the following methods, the system will **automatically calculate and generate a complete color system** including hover, pressed, and other state variations, ensuring visual consistency:

| Method | Description | Parameters | Auto Color System |
| :-- | :-- | :-- | :--: |
| **SetPrimary** | Set brand primary color | Color primary | ✅ |
| **SetSuccess** | Set success color | Color success | ✅ |
| **SetWarning** | Set warning color | Color warning | ✅ |
| **SetError** | Set error color | Color error | ✅ |
| **SetInfo** | Set info color | Color info | ✅ |

#### Usage Example

``` csharp
// Automatically generates a complete brand color system (including hover, pressed states)
AntdUI.Style.SetPrimary(Color.FromArgb(0, 173, 154));
```

### 2. Single Color Value Setting

The following method **only sets a single color value** and does not automatically generate a color system, suitable for scenarios where precise control over specific colors is needed:

#### Global Single Color Value Setting

``` csharp
// Only sets a single primary color value, no color system generated
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154));
```

#### Component-Specific Color Value Setting

``` csharp
// Only sets a single primary color value for Button component, no color system generated
AntdUI.Style.Set(AntdUI.Colour.Primary, Color.FromArgb(0, 173, 154), "Button");
```

### 3. Color System Configuration File

Batch set colors through configuration files, **HEX format is recommended**:

``` csharp
var colorConfig = new Dictionary<string, string> {
    { "Primary", "#ED4192" },          // Brand primary color
    { "PrimaryButton", "#E0282E" },    // Primary color for Button only
    { "Success", "#52C41A" },          // Success color
    { "Warning", "#FAAD14" },          // Warning color
    { "Error", "#F5222D" },            // Error color
    { "Info", "#1890FF" }              // Info color
};
AntdUI.Style.LoadCustom(colorConfig);
```

---

## Theme Mode Management

### 1. Enable Dark Mode

> After enabling dark mode, window background and font colors need to be set manually

``` csharp
// Enable dark mode
AntdUI.Config.IsDark = true;
```

### 2. Theme Switching Configuration

#### Global Theme Configuration

> Set global theme switching configuration in `Program.cs`

``` csharp
// Set global theme switching configuration
AntdUI.Config.Theme()
    .Dark("#000", "#fff") // Dark mode background and text color
    .Light("#fff", "#000"); // Light mode background and text color
```

#### Window Theme Configuration

> Windows based on `AntdUI.BaseForm` can customize theme switching configuration

``` csharp
// Set theme switching configuration in window
public partial class Form1 : AntdUI.BaseForm
{
    public Form1()
    {
        InitializeComponent();
        
        // Customize window theme switching configuration
        Theme()
            .Dark("#1e1e1e", "#ffffff") // Dark mode background and text color
            .Light("#ffffff", "#000000") // Light mode background and text color
            .Header(header1, Color.FromArgb(240, 242, 245), Color.FromArgb(18, 18, 18)) // Page header color
            .Button(btnTheme); // Theme switch button
    }
    
    // Theme switch button click event
    private void btnTheme_Click(object sender, EventArgs e)
    {
        // Toggle theme mode
        AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
    }
}
```

#### Theme Switching Configuration Methods

| Method | Description | Parameters |
| :-- | :-- | :-- |
| **Dark** | Set dark mode colors | Color back, Color fore / string back, string fore |
| **Light** | Set light mode colors | Color back, Color fore / string back, string fore |
| **Header** | Set page header colors | PageHeader header, Color light, Color dark / string light, string dark |
| **Button** | Set theme switch button | Button button |
| **Call** | Set theme switch callback | Action<bool> call |
| **Light** | Set light mode callback | Action call |
| **Dark** | Set dark mode callback | Action call |

---

## Color Tools

### Access Color System Database

``` csharp
// Get current theme's color systems
var primaryPalette = AntdUI.Style.Db.Primary; // Brand primary color system
var successPalette = AntdUI.Style.Db.Success; // Success color system
var warningPalette = AntdUI.Style.Db.Warning; // Warning color system
var errorPalette = AntdUI.Style.Db.Error;     // Error color system
var infoPalette = AntdUI.Style.Db.Info;       // Info color system
```

### Color Conversion Methods

| Method | Description | Return Value | Parameters |
| :-- | :-- | :-- | :-- |
| **ToHSV** | Color to HSV | HSV | Color color |
| **HSVToColor** | HSV to Color | Color | HSV hsv, float alpha = 1 |
| **HSVToColor** | HSV to Color | Color | float hue, float saturation, float value, float alpha = 1 |
||||
| **ToHSL** | Color to HSL | HSL | Color color |
| **HSLToColor** | HSL to Color | Color | HSL hsl, float alpha = 1 |
| **HSLToColor** | HSL to Color | Color | float hue, float saturation, float lightness, float alpha = 1 |
||||
| **ToColor** | HEX to RGB | Color | string hex |
| **ToHex** | RGB to HEX | string | Color color |
||||
| **rgba** | Convert to color | Color | int r, int g, int b, float alpha = 1 |
| **rgba** | Convert to color | Color | Color color, float alpha = 1 |