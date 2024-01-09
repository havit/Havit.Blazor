namespace Havit.Blazor.Documentation.Shared.Components;

public partial class ComponentApiDocCssVariable
{
	/// <summary>
	/// Name of the css variable.
	/// </summary>
	[Parameter] public string Name { get; set; }
	/// <summary>
	/// Default value for the css variable.
	/// </summary>
	[Parameter] public string Default { get; set; }
	[Parameter] public RenderFragment ChildContent { get; set; }
}
