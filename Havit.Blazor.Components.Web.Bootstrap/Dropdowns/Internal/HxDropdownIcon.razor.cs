namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxDropdownIcon : ComponentBase
{
	/// <summary>
	/// Item icon (use <see cref="BootstrapIcon" />).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu item icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }
}
