# Information and Guidelines for Contributors
Thank you for contributing to HAVIT Blazor and making it even better. We are happy about every contribution! Issues, fixes, enahncements, new components...

## Coding Standards
* Align with [official C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
	and [Framework Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/)
* Respect our `.editorconfig` requirements (you can suggest changing them).

## Naming Guidelines
* Preserve original [Bootstrap](https://getbootstrap.com/) naming whenever possible (adjust to comply with .NET coding standards).
* Follow existing [Blazor](https://github.com/dotnet/aspnetcore/tree/main/src/Components) naming whenever possible.
* Prefix all event parameters with `On`, e.g. `OnClosed`, `OnShown`, `OnFileUploaded` (except `XyChanged` callbacks).
* Suffix all `RenderFragment`-parameters with `Template` (except very specific cases such as `HxGrid.Columns`), e.g. `HeaderTemplate`,  `ItemTemplate`, `FooterTemplate`.
* Suffix all `Func`-parameters returning a projection with `Selector`, e.g. `TextSelector`,  `CssClassSelector`, `ValueSelector`. See [`HxSelect`](https://havit.blazor.eu/components/HxSelect) for samples.

## Design Guidelines
* Prefer ease of user over complex functionality.
* Allow interception of the events in derived components by using the virtual `InvokeOnEventNameAsync()` methods
```csharp
[Parameter] public EventCallback OnClosed { get; set; }
protected virtual Task InvokeOnClosedAsync() => OnClosed.InvokeAsync();
```
* Allow setting application-wide defaults by using the `Defaults` static property. Allow extension of the defaults in derived components by using the `GetDefaults()` virtual method.
```csharp
public static ComponentNameSettings Defaults { get; set; } = new();
protected virtual ComponentNameSettings GetDefaults() => Defaults;
```
* Use *CSS variables* whenever possible. Prefer existing variables over creating new ones (use derived values if possible).
* Provide `/// <summary>` comments (extracted to API documentation).
* Create simple demos presenting individual features. [Bootstrap docs](https://getbootstrap.com/docs/5.3/getting-started/introduction/) might be inspiration for you.