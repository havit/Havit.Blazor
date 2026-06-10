namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/breadcrumb/">Bootstrap Breadcrumb</see> component.<br />
/// Indicates the current page's location within a navigational hierarchy.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxBreadcrumb">https://havit.blazor.eu/components/HxBreadcrumb</see>
/// </summary>
public partial class HxBreadcrumb
{
	/// <summary>
	/// Child content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Custom divider content rendered into the <c>breadcrumb-divider</c> elements between items (any icon, text character, or markup).
	/// When <c>null</c> (default), the divider is left empty and Bootstrap renders the built-in chevron
	/// (customizable globally via the <c>--bs-breadcrumb-divider-icon</c> CSS variable).
	/// </summary>
	[Parameter] public RenderFragment DividerTemplate { get; set; }

	private int _itemCounter;

	/// <summary>
	/// Called by <see cref="HxBreadcrumbItem"/> on initialization to determine the item position (dividers are rendered between items).
	/// </summary>
	internal int RegisterItem() => _itemCounter++;
}
