# HAVIT Blazor Bootstrap Component Library

HAVIT Blazor is a comprehensive .NET Blazor component library that provides Bootstrap v5 components for ASP.NET Blazor applications. It includes components for forms, buttons, data display, layout, navigation, and modals, plus additional features like gRPC client utilities and Google Tag Manager integration.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Coding Standards

Always follow the coding standards defined in `.editorconfig`:

### Key Standards
- **Indentation**: Use tabs with size 4
- **Usings**: Sort system directives first, don't separate import directive groups
- **Accessibility modifiers**: Required for non-interface members
- **Braces**: Always use braces for code blocks (warning level)
- **var keyword**: Prefer implicit types (var) unless the type is not obvious from the right-hand side or it would reduce readability
- **_underscoreName**: Required for private fields (allowed for primary constructor parameters)
- Do not use Nullable Reference Types

### EditorConfig Reference
The `.editorconfig` file in the repository root contains the complete coding standards. Key points:
- Tab indentation (size 4) for all files
- Trim trailing whitespace
- Spell checking enabled with custom exclusion dictionary
- C#-specific rules for modifiers, parentheses, and code style preferences

## Project Structure

### Key Projects
- `Havit.Blazor.Components.Web.Bootstrap/` - Main Bootstrap component library (NET)
- `Havit.Blazor.Components.Web/` - Core web components (NET)
- `Havit.Blazor.Documentation/` - Documentation site (NET WebAssembly)
- `Havit.Blazor.TestApp/` - Current test application (NET)
- `BlazorAppTest/` - Legacy test application (NET) - OBSOLETE, do not add any tests here

### Test Projects
- `Havit.Blazor.Components.Web.Tests/` - Core component tests
- `Havit.Blazor.Components.Web.Bootstrap.Tests/` - Bootstrap component tests
- `Havit.Blazor.Grpc.Core.Tests/` - gRPC core functionality tests
- `Havit.Blazor.Grpc.Client.Tests/` - gRPC client tests

### Testing Framework
- Tests use **xUnit** (`xunit.v3`) running on the **Microsoft.Testing.Platform** (MTP). Do **not** reintroduce MSTest or downgrade to VSTest (`Microsoft.NET.Test.Sdk`).
- Use xUnit conventions: `[Fact]` for simple tests, `[Theory]` with `[InlineData]`/`[MemberData]` for data-driven tests, and the `Assert.*` xUnit API (e.g. `Assert.Equal`, `Assert.True`, `Assert.Single`, `Assert.Empty`, `Assert.Contains`, `Assert.Throws<T>`). xUnit analyzers are treated as errors (warnings-as-errors is enabled), so prefer the analyzer-suggested assertions.
- Per-test setup/teardown uses the class constructor and `IDisposable.Dispose()`; assembly-wide setup/teardown uses `[assembly: AssemblyFixture(typeof(...))]`.
- E2E tests (`Havit.Blazor.E2ETests/`) use **Playwright** via `Microsoft.Playwright.Xunit.v3` (base class `PageTest`). Moq and Playwright remain the mocking/automation libraries.
- Every test project references `Microsoft.Testing.Extensions.TrxReport`; produce a TRX report with `dotnet test -- --report-trx`.

### Important Files
- `Directory.Build.props` - Global build properties
- `Directory.Packages.props` - Central package version management
- `.editorconfig` - Coding standards and style rules
- `.github/workflows/dotnet.yml` - CI/CD pipeline

## Common Tasks

### Working with Components
The main component library follows these patterns:
- Components are in `Havit.Blazor.Components.Web.Bootstrap/`
- Follow Bootstrap naming conventions
- Follow coding standards defined in `.editorconfig`
- Provide `/// <summary>` documentation comments

### Adding New Components
1. Create component in appropriate folder under `Havit.Blazor.Components.Web.Bootstrap/`
2. Follow existing naming patterns (prefix with `Hx`)
3. Follow `.editorconfig` coding standards (tabs, accessibility modifiers, etc.)
4. Add tests in corresponding `Tests` project
5. Update documentation if needed

### Working with Icons
Bootstrap icons are generated via source generator from `Havit.Bootstrap/Icons/bootstrap-icons.json`. If icon references fail, this indicates a source generator issue.

## Development Workflow
1. Install .NET prerequisites (one-time setup)
2. `dotnet restore`
3. Make your changes following `.editorconfig` standards
4. Build with warnings as errors: `s`
5. Run tests: `dotnet test` or `dotnet test --project path/to/YourComponentTests.csproj` to run specific tests
6. Validate changes manually

**Important:** Every code change must be verified with a build that treats warnings as errors (`-warnaserror`). Do not consider a change complete until the build passes with zero warnings.

### Azure DevOps
When referring to Azure DevOps, it's the **DEV** project.



