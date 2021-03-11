namespace Havit.Blazor.Components.Web.Bootstrap
{
	public enum DrawerRenderMode
	{
		/// <summary>
		/// Drawer content is rendered only when the drawer is open.
		/// Suitable for item-detail, item-edit, etc.
		/// </summary>
		OpenOnly = 0,

		/// <summary>
		/// Drawer content is always rendered (and hidden with CSS if not open).
		/// Needed for HxFilterForm with HxChipList.
		/// </summary>
		Always = 1
	}
}