using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Shared.Components;

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

	private ComponentApiDocModel model;

	private bool hasApi => model.HasValues || CssVariables is not null;
	private bool isDelegate => model.IsDelegate;
	private bool isEnum => model.IsEnum;
	private bool hasParameters => model.Parameters.Any();
	private bool hasProperties => model.Properties.Any();
	private bool hasEvents => model.Events.Any();
	private bool hasMethods => !model.IsEnum && model.Methods.Any();
	private bool hasStaticProperties => model.StaticProperties.Any();
	private bool hasStaticMethods => !model.IsEnum && model.StaticMethods.Any();
	private bool hasCssVariables => CssVariables is not null;

	protected override void OnParametersSet()
	{
		model = ComponentApiDocModelBuilder.BuildModel(this.Type);
	}
}
