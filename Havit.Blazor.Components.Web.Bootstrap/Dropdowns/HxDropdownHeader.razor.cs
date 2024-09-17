namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/#headers">Dropdown menu header</see> for <see cref="HxDropdownMenu"/>.
/// </summary>
public partial class HxDropdownHeader
{
	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Item icon (use <see cref="BootstrapIcon" />).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu item icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }
}
