namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Offcanvas render mode.
	/// </summary>
	public enum OffcanvasRenderMode
	{
		/// <summary>
		/// Offcanvas content is rendered only when it is open.
		/// Suitable for item-detail, item-edit, etc.
		/// </summary>
		OpenOnly = 0,

		/// <summary>
		/// Offcanvas content is always rendered (and hidden with CSS if not open).
		/// Needed for HxFilterForm with HxChipList.
		/// </summary>
		Always = 1
	}
}