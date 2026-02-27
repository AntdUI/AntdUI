[Home](../Home.md)・[UpdateLog](../UpdateLog.md)・[Config](../Config.md)・[Theme](../Theme.md)

## Tour
👚

> A bubble component used to guide users through product features step by step.

### Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**open** | Open tour guide | TourForm | Form `Form`, Action<Result> `Step Call`, Action<Popover> `Popover Call` |
**open** | Open tour guide | TourForm | Config `Config` |

### Configuration

#### Config Properties

Name | Description | Type | Default Value |
:--|:--|:--|:--|
**Form** | Form | Form | - |
**Scale** | Scale | float | 1.04F |
**MaskClosable** | Whether clicking the mask allows closing | bool | true |
**ClickNext** | Click next | bool | true |
**StepCall** | Step call back | Action<Result> | - |
**PopoverCall** | Popover call back | Action<Popover>`?` | `null` |

#### Config Methods

Name | Description | Return Value | Parameters |
:--|:--|:--|:--|
**SetScale** | Set scale | Config | float `Value` |
**SetMaskClosable** | Set whether clicking the mask allows closing | Config | bool `Value` |
**SetClickNext** | Set click next | Config | bool `Value` |
**SetCall** | Set step call back | Config | Action<Result> `Value` |
**SetStepCall** | Set popover call back | Config | Action<Popover> `Value` |

### Helper Classes

#### Result Class

**Property**

Name | Description | Type |
:--|:--|:--|
**Index** | Step index | int |

**Method**

Name | Description | Parameters |
:--|:--|:--|
**Set** | Set control | Control `Control` |
**Set** | Set rectangle | Rectangle `Rectangle` |
**Close** | Close | - |

#### Popover Class

**Property**

Name | Description | Type |
:--|:--|:--|
**Form** | Form | Form |
**Tour** | Tour | TourForm |
**Index** | Step index | int |
**Rect** | Display area | Rectangle`?` |

#### TourForm Interface

**Method**

Name | Description | Parameters |
:--|:--|:--|
**Close** | Close | - |
**IClose** | Internal close | bool `Is Dispose` |
**Previous** | Previous step | - |
**Next** | Next step | - |
**LoadData** | Load data | - |

### Example

```csharp
// Basic usage
Tour.open(this, (result) => {
	switch (result.Index)
	{
		case 0:
			result.Set(button1); // Guide to button1
			break;
		case 1:
			result.Set(textBox1); // Guide to textBox1
			break;
		case 2:
			result.Set(new Rectangle(100, 100, 200, 100)); // Guide to specified area
			break;
		case 3:
			result.Close(); // Close guide
			break;
	}
}, (popover) => {
	// Create popover content here
	var form = new Form();
	form.Text = $"Step {popover.Index + 1}";
	form.Size = new Size(200, 100);
	// Set popover position
	// ...
	form.Show(popover.Form);
});

// Full configuration
var config = new Tour.Config(this, (result) => {
	// Step callback
}) 
.SetScale(1.1F) // Set scale
.SetMaskClosable(false) // Click mask not close
.SetClickNext(false); // Click not auto next

Tour.open(config);
```