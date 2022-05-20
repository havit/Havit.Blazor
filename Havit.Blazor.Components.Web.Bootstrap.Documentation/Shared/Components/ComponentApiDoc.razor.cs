using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public partial class ComponentApiDoc
{
	[Parameter] public RenderFragment ChildContent { get; set; }

	[Parameter] public RenderFragment MainContent { get; set; }

	[Parameter] public RenderFragment CssVariables { get; set; }

	/// <summary>
	/// A type to generate the documentation for
	/// </summary>
	[Parameter] public Type Type { get; set; }

	[Parameter] public bool Delegate { get; set; }

	[Inject] protected IComponentApiDocModelBuilder ComponentApiDocModelBuilder { get; set; }
	[Inject] protected ILogger<ComponentApiDoc> Logger { get; set; }

	private ComponentApiDocModel model;
	private TimeSpan? startTime;

	protected override void OnParametersSet()
	{
		startTime ??= DateTime.Now.TimeOfDay;

		model = ComponentApiDocModelBuilder.BuildModel(this.Type, this.Delegate);

		Logger.LogWarning($"ComponentApiDoc({this.GetHashCode()})_DownloadFileAndGetSummaries: Elapsed {(DateTime.Now.TimeOfDay - startTime.Value).TotalMilliseconds} ms");
	}


	protected override void OnAfterRender(bool firstRender)
	{
		Logger.LogWarning($"ComponentApiDoc({this.GetHashCode()})_OnAfterRender: Elapsed {(DateTime.Now.TimeOfDay - startTime.Value).TotalMilliseconds} ms");
		startTime = null;
	}
}
