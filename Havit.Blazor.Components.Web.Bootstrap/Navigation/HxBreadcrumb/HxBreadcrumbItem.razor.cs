namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Item for <see cref="HxBreadcrumb"/>.
/// </summary>
public partial class HxBreadcrumbItem
{
	/// <summary>
	/// Item text (usually a name of the page).
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// The link of the breadcrumb (a page where the user will be led on click).
	/// </summary>
	[Parameter] public string Href { get; set; }

	/// <summary>
	/// Determines whether the <c>HxBreadcrumbItem</c> is active (use for a page that the user is currently on).
	/// </summary>
	[Parameter] public bool Active { get; set; }

	/// <summary>
	/// Item content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }
}
