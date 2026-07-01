namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Text item for the <see cref="HxDropdownMenu"/>.
/// </summary>
public partial class HxDropdownItemText
{
	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS class for the underlying <c>li</c> container (wrapping the main <c>span</c> inside).
	/// </summary>
	[Parameter] public string ContainerCssClass { get; set; }

	/// <summary>
	/// Item icon (use <see cref="BootstrapIcon" />).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu item icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }
}
