# Information and Guidelines for Contributors
Thank you for contributing to HAVIT Blazor and making it even better. We are happy about every contribution! Issues, fixes, enahncements, new components...

## Coding Standards
* Align with [official C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
	and [Framework Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/)
* Respect our `.editorconfig` requirements (you can suggest changing them).
* Please check your `git config core.autocrlf` if you get `IDE0055`. Git might checkout `\n` instead of `\r\n` and causes the build to fail.

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

## Run accessibility Tests (Havit.Blazor.E2ETests)
The accessibility checks are implemented in `Havit.Blazor.E2ETests` using Playwright and `Playwright.Axe`.

### Install browser dependencies
Build the test project and install the Playwright browsers before running tests.
```pwsh
# Build the test project first, then install the browsers.
# Replace net10.0 with the target framework you are using if different.
dotnet build Havit.Blazor.E2ETests\Havit.Blazor.E2ETests.csproj
Havit.Blazor.E2ETests\bin\Debug\net10.0\playwright.ps1 install
```

### Run accessibility tests
The Axe execution is enabled by the environment variable `ACCESSIBILITYTESTS`.

```pwsh
$env:ACCESSIBILITYTESTS = "true"
dotnet test Havit.Blazor.E2ETests\Havit.Blazor.E2ETests.csproj
```

In CMD:
```cmd
set ACCESSIBILITYTESTS=true
dotnet test Havit.Blazor.E2ETests\Havit.Blazor.E2ETests.csproj
```

### What the test run does
- The accessibility audit runs automatically after each E2E test when `ACCESSIBILITYTESTS` is enabled.
- Only `Serious` and `Critical` Axe violations fail the test.
- A Markdown report is written to the temporary folder if violations are found.

### What is checked
The tests run Axe with the following tags:
- `wcag2a`
- `wcag2aa`
- `wcag21a`
- `wcag21aa`
- `EN-301-549`

The configuration is defined in `Havit.Blazor.E2ETests\TestAppTestBase.cs`.

### Notes
- The environment variable controls the accessibility overlay mode.
- `Havit.Blazor.E2ETests.csproj` also defines an `ACCESSIBILITYTESTS` configuration, but the runtime execution of Axe is governed by the `ACCESSIBILITYTESTS` environment variable in code.
- If you need to change which rules are ignored or which violations fail, edit `RunAxe()` in `Havit.Blazor.E2ETests\TestAppTestBase.cs`.
