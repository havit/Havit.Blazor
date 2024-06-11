using System.ComponentModel;

namespace Havit.Blazor.Components.Web.Internal;

// This is used by components to move its body rendering to the end of the render queue so we can collect
// the list of child components first. It has to be public only because it's used from .razor logic.

/// <summary>
/// For internal use only. Do not use.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class HxDeferInternal : ComponentBase
{
	/// <summary>
	/// For internal use only. Do not use.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// For internal use only. Do not use.
	/// </summary>
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.AddContent(0, ChildContent);
	}
}

