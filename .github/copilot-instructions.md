# HAVIT Blazor Bootstrap Component Library

HAVIT Blazor is a comprehensive .NET Blazor component library that provides Bootstrap 5.3 components for ASP.NET Blazor applications. It includes components for forms, buttons, data display, layout, navigation, and modals, plus additional features like gRPC client utilities and Google Tag Manager integration.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Essential Prerequisites
Install the required SDKs and tools in this exact order:

1. **Install .NET 9.0 SDK** (REQUIRED for building):
   ```bash
   curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.101
   export PATH="/home/runner/.dotnet:$PATH"
   ```

2. **Install .NET 9.0 Runtime** (REQUIRED for running tests):
   ```bash
   curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.7 --runtime dotnet
   ```

3. **Install Node.js and npm** (REQUIRED for CSS builds):
   ```bash
   sudo apt-get update && sudo apt-get install -y nodejs npm
   ```

4. **Verify installations**:
   ```bash
   dotnet --version  # Should show 9.0.101
   node --version    # Should show v20.19.4+
   npm --version     # Should show 10.8.2+
   ```

### Build Process
NEVER CANCEL build or test commands. All timeouts below are based on measured performance:

1. **Restore dependencies** (takes ~13 seconds. NEVER CANCEL. Set timeout to 60+ seconds):
   ```bash
   export PATH="/home/runner/.dotnet:$PATH"
   dotnet restore
   ```

2. **Build Bootstrap CSS** (takes ~5 seconds. NEVER CANCEL. Set timeout to 30+ seconds):
   ```bash
   cd Havit.Bootstrap
   npm install  # Takes ~62 seconds first time. NEVER CANCEL. Set timeout to 180+ seconds
   npm run css  # Builds CSS files to dist/css/
   ```

3. **Build the solution** (takes ~49 seconds with known issues. NEVER CANCEL. Set timeout to 120+ seconds):
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

### CSS Development
When modifying Bootstrap styles:

1. **Edit SCSS files** in `Havit.Bootstrap/scss/` directory
2. **Build CSS** with `npm run css` (5 seconds)
3. **Copy to Blazor project**:
   ```bash
   cd Havit.Bootstrap
   npm run publish-to-blazor
   ```

## Validation

### Manual Testing Scenarios
After making changes, always validate by:

1. **Verify CSS builds work**: Run `npm run css` in Havit.Bootstrap folder
2. **Verify core tests pass**: Run the three test projects listed above
3. **Check component compilation**: Build individual component projects

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
- `Havit.Bootstrap/` - CSS/SCSS build process with Bootstrap customizations

### Test Projects
- `Havit.Blazor.Components.Web.Tests/` - Core component tests (38 tests)
- `Havit.Blazor.Components.Web.Bootstrap.Tests/` - Bootstrap component tests
- `Havit.Blazor.Grpc.Core.Tests/` - gRPC core functionality tests (2 tests)
- `Havit.Blazor.Grpc.Client.Tests/` - gRPC client tests (1 test)

### Important Files
- `Directory.Build.props` - Global build properties
- `Directory.Packages.props` - Central package version management
- `Havit.Bootstrap/package.json` - npm build scripts for CSS
- `.github/workflows/dotnet.yml` - CI/CD pipeline

## Common Tasks

### Working with Components
The main component library follows these patterns:
- Components are in `Havit.Blazor.Components.Web.Bootstrap/`
- Follow Bootstrap naming conventions
- Use CSS variables when possible
- Provide `/// <summary>` documentation comments

### Adding New Components
1. Create component in appropriate folder under `Havit.Blazor.Components.Web.Bootstrap/`
2. Follow existing naming patterns (prefix with `Hx`)
3. Add tests in corresponding `Tests` project
4. Update documentation if needed

### CSS Customization
1. Edit SCSS files in `Havit.Bootstrap/scss/`
2. Run `npm run css` to compile
3. Run `npm run publish-to-blazor` to copy files

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
- **First npm install**: Takes ~62 seconds, subsequent runs are faster
- **CSS build**: Takes ~5 seconds consistently  
- **dotnet restore**: Takes ~13 seconds
- **Full solution build**: Takes ~49 seconds (may fail with known issues)
- **Test execution**: Takes ~6 seconds for working test suites

## Development Workflow
1. Install prerequisites (one-time setup)
2. `dotnet restore` (13s)
3. Make your changes
4. Build CSS if needed: `cd Havit.Bootstrap && npm run css` (5s)
5. Build components: `dotnet build --no-restore` (49s, may have known failures)
6. Run tests: `dotnet test` (6s) 
7. Validate changes manually

Always use the exact timeout values specified above and NEVER CANCEL long-running operations.