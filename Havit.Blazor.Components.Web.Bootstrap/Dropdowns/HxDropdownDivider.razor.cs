namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Divider for the <see cref="HxDropdownMenu"/>.
/// </summary>
public partial class HxDropdownDivider
{
	/// <summary>
	/// Additional CSS class for the underlying <c>li&gt;hr</c> element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS class for the underlying <c>li</c> container (wrapping the main <c>hr</c> inside).
	/// </summary>
	[Parameter] public string ContainerCssClass { get; set; }
}
