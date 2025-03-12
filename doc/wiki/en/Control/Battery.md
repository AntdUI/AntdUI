[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Battery
👚

> Display device battery level.

- DefaultProperty：Value
- DefaultEvent：Click

### Property

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**ForeColor** | Text color | Color`?` | `null` |
**BackColor** | Background color | Color`?` | `null` |
**Radius** | Rounded corners | int | 4 |
**DotSize** | Dot size | int | 8 |
**Value** | Progress value | int | 0 |
||||
**ShowText** | Display Text | bool | true |
**FillFully** | Full charge color | Color | 0, 210, 121 |
**FillWarn** | Warning battery color | Color | 250, 173, 20 |
**FillDanger** | Dangerous battery color | Color | 255, 77, 79 |
**ValueWarn** 🔴 | Warning battery threshold | int | 30 |
**ValueDanger** 🔴 | Dangerous electricity threshold | int | 20 |