namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Text content for the <see cref="HxNavbar"/>.
/// </summary>
public partial class HxNavbarText
{
	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }
}
