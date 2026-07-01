namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Offcanvas render mode.
/// </summary>
public enum OffcanvasRenderMode
{
	/// <summary>
	/// Offcanvas content is rendered only when it is open.
	/// Suitable for item-detail, item-edit, etc.
	/// This setting applies only when <see cref="OffcanvasResponsiveBreakpoint.None"/> is set. For all other values, the content is always rendered (to be available for the mobile version).
	/// </summary>
	OpenOnly = 0,

	/// <summary>
	/// Offcanvas content is always rendered (and hidden with CSS if not open).
	/// Needed for HxFilterForm with HxChipList.
	/// </summary>
	Always = 1
}
