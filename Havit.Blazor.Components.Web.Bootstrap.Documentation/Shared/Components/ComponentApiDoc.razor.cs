using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

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

	[Inject] protected IComponentApiDocModelBuilder ComponentApiDocModelBuilder { get; set; }

	private List<SectionTitle> sectionTitles = new();

	private ComponentApiDocModel model;

	public void RegisterSectionTitle(SectionTitle sectionTitle)
	{
		if (!sectionTitles.Contains(sectionTitle))
		{
			sectionTitles.Add(sectionTitle);
			StateHasChanged();
		}
	}

	protected override void OnParametersSet()
	{
		model = ComponentApiDocModelBuilder.BuildModel(this.Type);
	}
}
