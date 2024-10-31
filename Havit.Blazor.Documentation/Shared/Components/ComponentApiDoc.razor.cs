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

	[CascadingParameter] public CanonicalLinkManager CanonicalLinkManager { get; set; }

	private bool? _mainComponent;

	[Inject] protected IComponentApiDocModelBuilder ComponentApiDocModelBuilder { get; set; }
	[Inject] protected NavigationManager NavigationManager { get; set; }

	private bool HasApi => _model.HasValues || CssVariables is not null;
	private bool IsDelegate => _model.IsDelegate;
	private bool IsEnum => _model.IsEnum;
	private bool HasParameters => _model.Parameters.Any();
	private bool HasProperties => _model.Properties.Any();
	private bool HasEvents => _model.Events.Any();
	private bool HasMethods => !_model.IsEnum && _model.Methods.Any();
	private bool HasStaticProperties => _model.StaticProperties.Any();
	private bool HasStaticMethods => !_model.IsEnum && _model.StaticMethods.Any();
	private bool HasCssVariables => CssVariables is not null;

	private ComponentApiDocModel _model;

	protected override void OnParametersSet()
	{
		if (CanonicalLinkManager is not null)
		{
			_mainComponent ??= CanonicalLinkManager.RegisterComponentApiDoc();
		}

		_model = ComponentApiDocModelBuilder.BuildModel(Type);
	}

	private string GetRelativeCanonicalUrl(string plainTypeName)
	{
		bool isComponent = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Contains("components");
		return $"{(isComponent ? "components" : "types")}/{plainTypeName}";
	}
}
