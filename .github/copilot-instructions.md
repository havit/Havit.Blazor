# HAVIT Blazor Bootstrap Component Library

HAVIT Blazor is a comprehensive .NET Blazor component library that provides Bootstrap 5.3 components for ASP.NET Blazor applications. It includes components for forms, buttons, data display, layout, navigation, and modals, plus additional features like gRPC client utilities and Google Tag Manager integration.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Essential Prerequisites
Install the required .NET SDKs in this exact order:

1. **Install .NET 9.0 SDK** (REQUIRED for building):
   ```bash
   curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.101
   export PATH="/home/runner/.dotnet:$PATH"
   ```

2. **Install .NET 9.0 Runtime** (REQUIRED for running tests):
   ```bash
   curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.7 --runtime dotnet
   ```

3. **Verify installations**:
   ```bash
   dotnet --version  # Should show 9.0.101
   ```

### Build Process
NEVER CANCEL build or test commands. All timeouts below are based on measured performance:

1. **Restore dependencies** (takes ~13 seconds. NEVER CANCEL. Set timeout to 60+ seconds):
   ```bash
   export PATH="/home/runner/.dotnet:$PATH"
   dotnet restore
   ```

2. **Build the solution** (takes ~49 seconds with known issues. NEVER CANCEL. Set timeout to 120+ seconds):
   ```bash
   export PATH="/home/runner/.dotnet:$PATH"
   dotnet build --no-restore
   ```
   **Known Issues:** Build may fail with Bootstrap icon source generator errors due to analyzer version mismatch. This is a known limitation with .NET SDK 9.0.101.

### Testing
Run tests to validate your changes (takes ~6 seconds. NEVER CANCEL. Set timeout to 30+ seconds):

```bash
export PATH="/home/runner/.dotnet:$PATH"
# Run specific test projects that work reliably:
dotnet test Havit.Blazor.Components.Web.Tests/Havit.Blazor.Components.Web.Tests.csproj --no-restore -p:TargetFramework=net8.0
dotnet test Havit.Blazor.Grpc.Core.Tests/Havit.Blazor.Grpc.Core.Tests.csproj --no-restore -p:TargetFramework=net8.0
dotnet test Havit.Blazor.Grpc.Client.Tests/Havit.Blazor.Grpc.Client.Tests.csproj --no-restore -p:TargetFramework=net8.0
```

Expected Results:
- Havit.Blazor.Components.Web.Tests: 38 tests, all pass
- Havit.Blazor.Grpc.Core.Tests: 2 tests, all pass  
- Havit.Blazor.Grpc.Client.Tests: 1 test, all pass

## Coding Standards

Always follow the coding standards defined in `.editorconfig`:

### Key Standards
- **Indentation**: Use tabs with size 4
- **Usings**: Sort system directives first, don't separate import directive groups
- **Accessibility modifiers**: Required for non-interface members
- **Braces**: Always use braces for code blocks (warning level)
- **var keyword**: Avoid using var, prefer explicit types
- **Documentation**: Provide `/// <summary>` documentation comments for public APIs

### EditorConfig Reference
The `.editorconfig` file in the repository root contains the complete coding standards. Key points:
- Tab indentation (size 4) for all files
- Trim trailing whitespace
- Spell checking enabled with custom exclusion dictionary
- C#-specific rules for modifiers, parentheses, and code style preferences

## Validation

### Manual Testing Scenarios
After making changes, always validate by:

1. **Verify core tests pass**: Run the three test projects listed above
2. **Check component compilation**: Build individual component projects

### Build Validation  
Always run these validation steps before committing:

```bash
export PATH="/home/runner/.dotnet:$PATH"
# Quick validation build (targets working components)
dotnet build Havit.Blazor.Components.Web/Havit.Blazor.Components.Web.csproj --no-restore -p:TargetFramework=net8.0
dotnet build Havit.Blazor.Grpc.Core/Havit.Blazor.Grpc.Core.csproj --no-restore -p:TargetFramework=net8.0
```

## Project Structure

### Key Projects
- `Havit.Blazor.Components.Web.Bootstrap/` - Main Bootstrap component library (NET 8.0/9.0)
- `Havit.Blazor.Components.Web/` - Core web components (NET 8.0/9.0)
- `Havit.Blazor.Documentation/` - Documentation site (NET 9.0 WebAssembly)
- `Havit.Blazor.TestApp/` - Current test application (NET 9.0)
- `BlazorAppTest/` - Legacy test application (NET 9.0) - OBSOLETE

### Test Projects
- `Havit.Blazor.Components.Web.Tests/` - Core component tests (38 tests)
- `Havit.Blazor.Components.Web.Bootstrap.Tests/` - Bootstrap component tests
- `Havit.Blazor.Grpc.Core.Tests/` - gRPC core functionality tests (2 tests)
- `Havit.Blazor.Grpc.Client.Tests/` - gRPC client tests (1 test)

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

## Troubleshooting

### Build Issues
- **"does not support targeting .NET 9.0"**: Install .NET 9.0 SDK using the install script above
- **Bootstrap icon errors**: Known issue with source generator. Try building individual projects with net8.0 target
- **"Text file busy" during runtime install**: Normal warning, installation still succeeds

### Test Issues  
- **"You must install or update .NET to run this application"**: Install .NET 9.0 runtime using the script above
- **Tests timeout**: Use longer timeout values, tests may take up to 30 seconds in some environments

### Performance
- **dotnet restore**: Takes ~13 seconds
- **Full solution build**: Takes ~49 seconds (may fail with known issues)
- **Test execution**: Takes ~6 seconds for working test suites

## Development Workflow
1. Install .NET prerequisites (one-time setup)
2. `dotnet restore` (13s)
3. Make your changes following `.editorconfig` standards
4. Build components: `dotnet build --no-restore` (49s, may have known failures)
5. Run tests: `dotnet test` (6s) 
6. Validate changes manually

Always use the exact timeout values specified above and NEVER CANCEL long-running operations.