[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Spin
👚

> Used for the loading status of a page or a block.

- DefaultProperty：Text
- DefaultEvent：Click

### Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
🌏 **LocalizationText** | International Text | string`?` | `null` |
||||
**ForeColor** | Text color | Color`?` | `null` |
||||
**Fill** | Colour | Color`?` | `null` |
||||
**CirSize** | Loading circle size | int`?` | `null` |
**CirWidth** | Loading circle thickness | int`?` | `null` |
||||
**Indicator** | Loading indicator | Image`?` | `null` |
**IndicatorSvg** | Loading indicator SVG | string`?` | `null` |

### Methods

> All `IControl` that inherit FHIR trol support the `Spin` method

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**Spin** | LOADING | void | Action action `Commission to wait for`, Action? end = null `Post completion callback` |
**Spin** | LOADING | void | [Spin.Config](#spin.config) `Config`, Action action `Commission to wait for`, Action? end = null `Post completion callback` |

### Static Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**open** | LOADING | Task | Control control `Control body`, Action<Config> action `Delegate to wait for`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, string text `Loading text`, Action<Config> action `Delegate to wait for`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, Action<Config> action `Delegate to wait for`, CancellationTokenSource? token `Cancellation token`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, string text `Loading text`, Action<Config> action `Delegate to wait for`, CancellationTokenSource? token `Cancellation token`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, Config config `Custom configuration`, Action<Config> action `Delegate to wait for`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, Func<Config, Task> action `Async delegate to wait for`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, string text `Loading text`, Func<Config, Task> action `Async delegate to wait for`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, Func<Config, Task> action `Async delegate to wait for`, CancellationTokenSource? token `Cancellation token`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, string text `Loading text`, Func<Config, Task> action `Async delegate to wait for`, CancellationTokenSource? token `Cancellation token`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |
**open** | LOADING | Task | Control control `Control body`, Config config `Custom configuration`, Func<Config, Task> action `Async delegate to wait for`, Action? end = null `Callback after completion`, Action<Exception>? error = null `Callback when error occurs` |

### Config

#### Spin.Config

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Text** | Text | string`?` | `null` |
**Value** | Progress | float`?` | `null` |
||||
**Color** | Colour | Color`?` | `null` |
**Font** | Font | Font`?` | `null` |
**Fore** | Text color | Color`?` | `null` |
||||
**Back** | Background color | Color`?` | `null` |
||||
**CancelText** | Cancel button text | string`?` | `Cancel` |
**CancelTokenSource** | Cancellation token source | CancellationTokenSource`?` | `null` |
||||
**Radius** | Rounded corners | int`?` | `null` |
||||
**Indicator** | Loading indicator | Image`?` | `null` |
**IndicatorSvg** | Loading indicator SVG | string`?` | `null` |
**Rate** | Progress rate | float`?` | `null` |

***

### Code Example

```csharp
panel1.Spin(config => {
	// Time consuming code
	sleep(1000);
},()=>{
	//Loading completed
});
```

```csharp
AntdUI.Spin.open(panel1, config => {
	// Time consuming code
	sleep(1000);
},()=>{
	//Loading completed
});
```

![SpinRun](SpinRun.png)