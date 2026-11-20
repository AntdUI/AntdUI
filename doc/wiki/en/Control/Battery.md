[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Battery
👚

> Display device battery level.

- DefaultProperty：Value
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Value** | Progress value | int | 0 |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**BackColor** | Background color | Color`?` | `null` |
**OriginalBackColor** | Original background color | Color | Transparent |
||||
**ShowText** | Display Text | bool | true |
||||
**Radius** | Rounded corners | int | 4 |
||||
**DotSize** | Dot size | int | 8 |
||||
**FillDanger** | Dangerous battery color | Color | 255, 77, 79 |
**FillFully** | Full charge color | Color | 0, 210, 121 |
**FillWarn** | Warning battery color | Color | 250, 173, 20 |
**ValueDanger** | Dangerous battery threshold | int | 20 |
**ValueWarn** | Warning battery threshold | int | 30 |