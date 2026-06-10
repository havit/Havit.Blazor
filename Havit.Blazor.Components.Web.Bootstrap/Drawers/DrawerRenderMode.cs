namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Drawer render mode.
/// </summary>
public enum DrawerRenderMode
{
	/// <summary>
	/// Drawer content is rendered only when it is open.
	/// Suitable for item-detail, item-edit, etc.
	/// This setting applies only when <see cref="DrawerResponsiveBreakpoint.None"/> is set. For all other values, the content is always rendered (to be available for the mobile version).
	/// </summary>
	OpenOnly = 0,

	/// <summary>
	/// Drawer content is always rendered (and hidden with CSS if not open).
	/// Needed for HxFilterForm with HxChipList.
	/// </summary>
	Always = 1
}
