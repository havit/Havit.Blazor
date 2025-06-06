namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// This component creates a grouping and keeps a contract for <see cref="HxScrollToValidationMessageContainer"/>
/// so that it can scroll to the input group properly
/// </summary>
public partial class HxValidationScrollableGroup
{
	[Parameter, EditorRequired] public RenderFragment ChildContent { get; set; }
}