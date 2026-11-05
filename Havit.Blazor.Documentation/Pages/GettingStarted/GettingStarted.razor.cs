namespace Havit.Blazor.Documentation.Pages.GettingStarted;

public partial class GettingStarted(
	NavigationManager navigationManager)
{
	[SupplyParameterFromQuery(Name = nameof(SetupModel.TargetFramework))] public string TargetFrameworkQuery { get; set; }
	[SupplyParameterFromQuery(Name = nameof(SetupModel.ProjectSetup))] public string ProjectSetupQuery { get; set; }
	[SupplyParameterFromQuery(Name = nameof(SetupModel.BwaRenderMode))] public string BwaRenderModeQuery { get; set; }
	[SupplyParameterFromQuery(Name = nameof(SetupModel.BootstrapTheme))] public string BootstrapThemeQuery { get; set; }
	[SupplyParameterFromQuery(Name = nameof(SetupModel.SamplePagesCreated))] public string SamplePagesCreatedQuery { get; set; }

	private readonly NavigationManager _navigationManager = navigationManager;

	private SetupModel _setup = new SetupModel();

	protected override void OnParametersSet()
	{
		if (Enum.TryParse<TargetFramework>(TargetFrameworkQuery, true, out var targetFramework))
		{
			_setup.TargetFramework = targetFramework;
		}
		if (Enum.TryParse<ProjectSetup>(ProjectSetupQuery, true, out var projectSetup))
		{
			_setup.ProjectSetup = projectSetup;
		}
		if (Enum.TryParse<BwaRenderMode>(BwaRenderModeQuery, true, out var bwaRenderMode))
		{
			_setup.BwaRenderMode = bwaRenderMode;
		}
		if (Enum.TryParse<BootstrapTheme>(BootstrapThemeQuery, true, out var bootstrapTheme))
		{
			_setup.BootstrapTheme = bootstrapTheme;
		}
		if (bool.TryParse(SamplePagesCreatedQuery, out var samplePagesCreated))
		{
			_setup.SamplePagesCreated = samplePagesCreated;
		}
	}

	private void ChangeSetup(SetupModel newSetup)
	{
		if (newSetup != _setup)
		{
			_setup = newSetup;

			UpdateUri();
		}
	}

	private void UpdateUri()
	{
		_navigationManager.NavigateTo(GetSetupUri(_setup), replace: true);
	}

	private string GetSetupUri(SetupModel setup)
	{
		return _navigationManager.GetUriWithQueryParameters(new Dictionary<string, object>
		{
			{ nameof(SetupModel.TargetFramework), setup.TargetFramework.ToString() },
			{ nameof(SetupModel.ProjectSetup), setup.ProjectSetup.ToString() },
			{ nameof(SetupModel.BwaRenderMode), setup.BwaRenderMode.ToString() },
			{ nameof(SetupModel.BootstrapTheme), setup.BootstrapTheme.ToString() },
			{ nameof(SetupModel.SamplePagesCreated), setup.SamplePagesCreated.ToString() }
		});
	}

	private bool HasClientProject()
	{
		if ((_setup.ProjectSetup == ProjectSetup.BlazorWebApp)
			&& ((_setup.BwaRenderMode == BwaRenderMode.Auto) || (_setup.BwaRenderMode == BwaRenderMode.Wasm)))
		{
			return true;
		}
		return false;
	}

	private bool HasStaticFileAssets()
	{
		if (_setup.TargetFramework == TargetFramework.Net8)
		{
			return false;
		}
		if (_setup.ProjectSetup == ProjectSetup.WasmStandalone)
		{
			return false;
		}
		return true;
	}

	private string GetHtmlHostFile()
	{
		if (_setup.ProjectSetup == ProjectSetup.WasmStandalone)
		{
			return "wwwroot/index.html";
		}
		return "App.razor";
	}

	private string GetSampleFilesBootstrapFolder()
	{
		if (_setup.TargetFramework == TargetFramework.Net8)
		{
			return "wwwroot/bootstrap";
		}

		return "wwwroot/lib/bootstrap";
	}

	private record SetupModel
	{
		public TargetFramework TargetFramework { get; set; } = TargetFramework.Net10;
		public ProjectSetup ProjectSetup { get; set; } = ProjectSetup.BlazorWebApp;
		public BwaRenderMode BwaRenderMode { get; set; } = BwaRenderMode.Auto;
		public BootstrapTheme BootstrapTheme { get; set; } = BootstrapTheme.HavitBlazor;
		public bool SamplePagesCreated { get; set; } = false;
	}

	private static readonly TargetFramework[] _targetFrameworks = [TargetFramework.Net10, TargetFramework.Net9, TargetFramework.Net8];
	private static readonly ProjectSetup[] _projectSetups = [ProjectSetup.BlazorWebApp, ProjectSetup.Server, ProjectSetup.WasmStandalone];
	private static readonly BwaRenderMode[] _bwaRenderModes = [BwaRenderMode.Auto, BwaRenderMode.Server, BwaRenderMode.Wasm, BwaRenderMode.None];
	private static readonly bool[] _samplePagesOptions = [false, true];
	private static readonly BootstrapTheme[] _bootstrapThemes = [BootstrapTheme.HavitBlazor, BootstrapTheme.PlainCdn, BootstrapTheme.PlainProject, BootstrapTheme.Custom];

	private static string GetSetupOptionText<TValue>(TValue value) => value switch
	{
		TargetFramework.Net10 => ".NET 10",
		TargetFramework.Net9 => ".NET 9",
		TargetFramework.Net8 => ".NET 8",
		ProjectSetup.BlazorWebApp => "Blazor Web App",
		ProjectSetup.Server => "Blazor Server App",
		ProjectSetup.WasmStandalone => "Blazor WebAssembly Standalone App",
		BwaRenderMode.Auto => "Auto",
		BwaRenderMode.Server => "Server",
		BwaRenderMode.Wasm => "WebAssembly",
		BwaRenderMode.None => "None",
		BootstrapTheme.HavitBlazor => "Havit.Blazor",
		BootstrapTheme.PlainCdn => "standard Bootstrap from CDN",
		BootstrapTheme.PlainProject => "standard Bootstrap from project",
		BootstrapTheme.Custom => "Custom",
		false => "Empty project",
		true => "Sample pages included",
		_ => value.ToString()
	};

	private enum TargetFramework { Net8, Net9, Net10 }
	private enum ProjectSetup { BlazorWebApp, Server, WasmStandalone }
	private enum BwaRenderMode { Auto, Server, Wasm, None }
	private enum BootstrapTheme { HavitBlazor, PlainCdn, PlainProject, Custom }
}
